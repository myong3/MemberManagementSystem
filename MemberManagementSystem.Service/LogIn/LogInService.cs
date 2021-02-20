using MemberManagementSystem.Model.Service.LogIn;
using MemberManagementSystem.Platform.Utilities;
using MemberManagementSystem.Service.DataAccessLayer.Common;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MemberManagementSystem.Service.LogIn
{
    public class LogInService : ILogInService
    {
        private readonly IUserAccountProvider _userAccountProvider;

        private readonly JwtHelpers _jwt;

        public LogInService(IUserAccountProvider userAccountProvider, JwtHelpers jwt)
        {
            // DI
            _userAccountProvider = userAccountProvider;
            _jwt = jwt;

        }

        public async Task<ServiceResult<LogInResponseServiceModel>> LogInGetToken(LogInServiceModel model)
        {
            try
            {
                var result = new ServiceResult<LogInResponseServiceModel>()
                {
                    IsOk = true
                };

                var queryResult = await _userAccountProvider.QueryAccountDetail(model.userAccount).ConfigureAwait(false);

                if (queryResult == null)
                {
                    result.Data = new LogInResponseServiceModel()
                    {
                        IsSuccessLogIn = false,
                        Message = "找不到您的帳戶，請再嘗試一次"
                    };
                }
                else
                {
                    using (var md5 = MD5.Create())
                    {
                        var hashResult = md5.ComputeHash(Encoding.ASCII.GetBytes(queryResult.userPasswordSalt + model.userPassword));
                        var strResult = BitConverter.ToString(hashResult);
                        var md5Result = strResult.Replace("-", "").ToLower();

                        if (md5Result == queryResult.userPassword)
                        {
                            if (queryResult.userPolicy)
                            {
                                result.Data = new LogInResponseServiceModel()
                                {
                                    IsSuccessLogIn = true,
                                    Message = "管理員登入成功",
                                    Access_token = _jwt.GenerateToken(model.userAccount, true)
                                };
                            }
                            else
                            {
                                result.Data = new LogInResponseServiceModel()
                                {
                                    IsSuccessLogIn = true,
                                    Message = "使用者登入成功",
                                    Access_token = _jwt.GenerateToken(model.userAccount)
                                };
                            }
                        }
                        else
                        {
                            result.Data = new LogInResponseServiceModel()
                            {
                                IsSuccessLogIn = false,
                                Message = "密碼錯誤，請再嘗試一次"
                            };
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return new ServiceResult<LogInResponseServiceModel>(false, 900000, ex.ToString(), ex);
            }
        }
    }
}
