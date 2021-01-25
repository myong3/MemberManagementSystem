using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MemberManagementSystem.Models;
using MemberManagementSystem.Models.ProviderModels;
using MemberManagementSystem.Services;
using MemberManagementSystem.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using static MemberManagementSystem.Models.Enums;
using static MemberManagementSystem.Extensions.CommonExtension;
using MemberManagementSystem.DataContext;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MemberManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CRUDController : ControllerBase
    {
        private readonly IDapper _dapper;

        public CRUDController(IDapper dapper)
        {
            // DI
            _dapper = dapper;
        }


        // GET: api/<CRUDController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CRUDController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CRUDController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CRUDController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CRUDController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        // Post: api/crud/LogIn
        /// <summary>傳送文字訊息</summary>
        /// <param name="msg">訊息</param>
        [HttpPost]
        [Route("LogIn")]
        public async Task<object> LogIn ([FromBody] UserViewModel model)
        {
            try
            {
                var querySql = $"Select * from [User] where userPassword = '{model.UserPassword}'";
                var updateSql = $"update [User] set userPassword = 'B' where userPassword = '{model.UserAccount}'";

                var updateModel = new UserModel()
                {
                    userId = 1,
                    userAccount = "Z",
                    userPassword = "Z"
                };

                var insertModel = new UserModel()
                {
                    userAccount = "Z",
                    userPassword = "Z"
                };

                var list = new List<UserModel>();
                list.Add(insertModel); 
                list.Add(insertModel);
                list.Add(insertModel);
                list.Add(insertModel);
                list.Add(insertModel);
                list.Add(insertModel);
                list.Add(insertModel);

                var deleteModel = new UserModel()
                {
                    userId = 11
                };


                var result = await _dapper.InsertAsync(ConnectionString.dbSystex.GetDescriptionText(), insertModel).ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
