using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MemberManagementSystem.Extensions;
using MemberManagementSystem.Models.ProviderModels;
using MemberManagementSystem.Models.SignUpModels;
using MemberManagementSystem.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static MemberManagementSystem.Models.Enums;

namespace MemberManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : Controller
    {
        private readonly IDapper _dapper;

        public SignUpController(IDapper dapper)
        {
            // DI
            _dapper = dapper;
        }

        // GET: api/signup
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// POST api/signup/CheckAccount
        /// 確認帳號是否重覆註冊
        /// </summary>
        /// <returns> true:已註冊 ; false:未註冊 </returns>
        [HttpPost]
        [Route("CheckAccount")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]

        public async Task<IActionResult> CheckAccount([FromBody] CheckAccountViewModel model)
        {
            try
            {
                if (model.userAccount != null)
                {
                    var querySql = $"Select * from [Userr] where userAccount = '{model.userAccount}'";
                    var queryResult = await _dapper.QueryFirstOrDefaultAsync<UserModel>(ConnectionString.localdb.GetDescriptionText(), querySql).ConfigureAwait(false);

                    if (queryResult == null)
                    {
                        return NoContent();
                    }
                    return Json(queryResult);

                }
                else
                {
                    return NoContent();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// POST api/signup/signup
        /// 將帳號密碼註冊至資料庫
        /// </summary>
        /// <returns>key value</returns>
        [HttpPost]
        [Route("SignUp")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]

        public async Task<IActionResult> SignUp([FromBody] SignUpViewModel model)
        {
            try
            {
                var salt = string.Empty;
                var insertEntity = new UserModel()
                {
                    userAccount = model.userAccount,
                    userPassword = GetHashPassword(model.userPassword, ref salt),
                    userPasswordSalt = salt,
                    userPolicy = false,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,

                };
                var result = await _dapper.InsertAsync(ConnectionString.localdb.GetDescriptionText(), insertEntity).ConfigureAwait(false);

                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
