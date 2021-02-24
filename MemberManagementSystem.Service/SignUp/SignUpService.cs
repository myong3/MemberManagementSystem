using MemberManagementSystem.Model.DataAccessLayer.SignUp;
using MemberManagementSystem.Model.Service.Common;
using MemberManagementSystem.Model.Service.SignUp;
using MemberManagementSystem.Platform.Utilities;
using MemberManagementSystem.Platform.Utilities.Extensions;
using MemberManagementSystem.Service.DataAccessLayer.Common;
using MemberManagementSystem.Service.DataAccessLayer.SignUp;
using MemberManagementSystem.Services.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static MemberManagementSystem.Model.Service.Common.Enum;

namespace MemberManagementSystem.Service.SignUp
{
    public class SignUpService : ISignUpService
    {
        private readonly ISignUpProvider _signUpProvider;

        private readonly IUserAccountProvider _userAccountProvider;

        private readonly ILogger<ISignUpService> _logger;

        public SignUpService(ISignUpProvider signUpProvider, IUserAccountProvider userAccountProvider, ILogger<ISignUpService> logger)
        {
            // DI
            _signUpProvider = signUpProvider;
            _userAccountProvider = userAccountProvider;
            _logger = logger;
        }

        public async Task<ServiceResult<bool>> CheckAccount(CheckAccountServiceModel model)
        {
            try
            {
                _logger.LogInformation($"{nameof(CheckAccount)} is start !");
                var result = new ServiceResult<bool>();
                var queryResult = await _userAccountProvider.QueryAccountDetail(model.userAccount).ConfigureAwait(false);

                if (queryResult == null)
                {
                    result.Data = false;
                    result.IsOk = true;
                }
                else
                {
                    result.Data = true;
                    result.IsOk = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ServiceResult<bool>(false, 900000, ex.ToString(), ex);
            }
        }

        public async Task<ServiceResult<int>> SignUp(SignUpServiceModel model)
        {
            try
            {
                var result = new ServiceResult<int>();
                if (string.IsNullOrEmpty(model.userAccount) || string.IsNullOrEmpty(model.userPassword))
                {
                    throw new ArgumentNullException(nameof(model));
                }

                var salt = string.Empty;

                var data = new AccountDetailModel()
                {
                    userAccount = model.userAccount,
                    userPassword = GetHashPassword(model.userPassword, ref salt),
                    userPasswordSalt = salt,
                    userPolicy = false,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };

                var queryResult = await _signUpProvider.SignUp(data).ConfigureAwait(false);

                result.Data = queryResult;
                result.IsOk = true;

                return result;
            }
            catch (Exception ex)
            {
                return new ServiceResult<int>(false, 900000, ex.ToString(), ex);
            }
        }

        private string GetHashPassword(string data, ref string salt)
        {
            #region 取得密碼加密salt
            var possible =
              "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890,./;'[]=-|}{)(*&^%$#@!?~`";
            var lengthOfCode = 8;
            salt = string.Empty;
            Random random = new Random();//亂數種子
            for (var i = 0; i < lengthOfCode; i++)
            {
                var randomNum = random.Next(0, possible.Length);
                salt += possible[randomNum];
            }
            #endregion

            using (var md5 = MD5.Create())
            {
                var hashResult = md5.ComputeHash(Encoding.ASCII.GetBytes(salt + data));
                var strResult = BitConverter.ToString(hashResult);
                var md5Result = strResult.Replace("-", "").ToLower();

                return md5Result;
            }
        }
    }
}
