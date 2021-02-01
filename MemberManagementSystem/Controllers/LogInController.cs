using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemberManagementSystem.Extensions;
using MemberManagementSystem.Models.LogInModels;
using MemberManagementSystem.Models.ProviderModels;
using MemberManagementSystem.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static MemberManagementSystem.Models.Enums;

namespace MemberManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInController : ControllerBase
    {
        private readonly IDapper _dapper;

        public LogInController(IDapper dapper)
        {
            // DI
            _dapper = dapper;
        }

        // GET: api/login
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// POST api/login/GetPassword
        /// 取得account對應的 password & password salt
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetPassword")]

        public async Task<GetPasswordReturnModel>GetPassword([FromBody] GetPasswordViewModel model)
        {
            try
            {
                var querySql = $"Select * from [Userr] where userAccount = '{model.userAccount}'";
                var queryResult = await _dapper.QueryFirstOrDefaultAsync<UserModel>(ConnectionString.localdb.GetDescriptionText(), querySql).ConfigureAwait(false);

                var result = new GetPasswordReturnModel();
                if (queryResult != null)
                {
                    result = new GetPasswordReturnModel()
                    {
                        userPassword = queryResult.userPassword,
                        userPasswordSalt = queryResult.userPasswordSalt
                    };
                }
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
