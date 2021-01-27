using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SignUpController : ControllerBase
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
        /// <param name="account"></param>
        /// <returns> true:已註冊 ; false:未註冊 </returns>
        [HttpPost]
        [Route("CheckAccount")]

        public async Task<bool> CheckAccount([FromBody] CheckAccountViewModel model)
        {
            try
            {
                var result = true;
                var querySql = $"Select * from [Userr] where userAccount = '{model.userAccount}'";
                var queryResult = await _dapper.QueryFirstOrDefaultAsync<UserModel>(ConnectionString.localdb.GetDescriptionText(), querySql).ConfigureAwait(false);

                if (queryResult == null)
                {
                    result = false;
                }
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// POST api/signup/signup
        /// 將帳號密碼註冊至資料庫
        /// </summary>
        /// <param name="account"></param>
        /// <returns> true:已註冊 ; false:未註冊 </returns>
        [HttpPost]
        [Route("SignUp")]

        public async Task<int> SignUp([FromBody] SignUpViewModel model)
        {

            try
            {
                var insertEntity = new UserModel()
                {
                    userAccount = model.userAccount,
                    userPassword = model.userPassword,
                    userPasswordSalt = model.userPasswordSalt,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,

                };
                var result = await _dapper.InsertAsync<UserModel>(ConnectionString.localdb.GetDescriptionText(), insertEntity).ConfigureAwait(false);

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
