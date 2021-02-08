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
using Microsoft.AspNetCore.Authorization;
using System.Net;
using MemberManagementSystem.Models.CRUDModels;
using MemberManagementSystem.Helper;
using Microsoft.AspNetCore.Http;
using Dapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MemberManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class CRUDController : Controller
    {
        private readonly IDapper _dapper;

        private readonly JwtHelpers _jwt;


        public CRUDController(IDapper dapper, JwtHelpers jwt)
        {
            // DI
            _dapper = dapper;

            _jwt = jwt;

        }


        // GET: api/crud
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var validateResult = validateTokenExp(Request);

                var querySql = $"Select * from [Userr]";
                var queryResult = await _dapper.QueryAsync<UserModel>(ConnectionString.localdb.GetDescriptionText(), querySql).ConfigureAwait(false);

                if (queryResult != null)
                {
                    var result = new List<UserResponseViewModel>();

                    foreach (var item in queryResult)
                    {
                        var data = new UserResponseViewModel()
                        {
                            userId = item.userId,
                            userAccount = item.userAccount,
                            userPassword = item.userPassword,
                            userPasswordSalt = item.userPasswordSalt,
                            userPolicy = item.userPolicy,
                            CreateTime = item.CreateTime,
                            UpdateTime = item.UpdateTime,
                            RefreshToken = validateResult
                        };

                        result.Add(data);
                    }

                    return Ok(result);
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

        // Post: api/crud/UpdateProfile
        /// <summary></summary>
        /// <param name="msg">訊息</param>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateViewModel model)
        {
            try
            {
                var updateSql = @"UPDATE [Userr]
                                  SET [userPolicy] = @userPolicy,
                                  [UpdateTime] = @UpdateTime
                                  WHERE [userId] = @userId";

                var param = new DynamicParameters();
                param.Add("@userPolicy", model.userPolicy);
                param.Add("@UpdateTime", DateTime.Now);
                param.Add("@userId", model.userId);


                var updateResult = await _dapper.ExecuteNonQueryAsync(ConnectionString.localdb.GetDescriptionText(), updateSql, param).ConfigureAwait(false);

                var validateResult = validateTokenExpAndPolicy(Request, model);

                var querySql = $"Select * from [Userr]";
                var queryResult = await _dapper.QueryAsync<UserModel>(ConnectionString.localdb.GetDescriptionText(), querySql).ConfigureAwait(false);

                if (queryResult != null)
                {
                    var result = new List<UserResponseViewModel>();
                    var RefreshToken = string.Empty;

                    foreach (var item in queryResult)
                    {
                        var data = new UserResponseViewModel()
                        {
                            userId = item.userId,
                            userAccount = item.userAccount,
                            userPassword = item.userPassword,
                            userPasswordSalt = item.userPasswordSalt,
                            userPolicy = item.userPolicy,
                            CreateTime = item.CreateTime,
                            UpdateTime = item.UpdateTime,
                            RefreshToken = validateResult
                        };

                        result.Add(data);
                    }

                    return Ok(result);
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

        // Post: api/crud/DeleteProfile
        /// <summary></summary>
        /// <param name="msg">訊息</param>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("DeleteProfile")]
        public async Task<IActionResult> DeleteProfile([FromBody] DeleteViewModel model)
        {
            try
            {
                var deleteModel = new UserModel()
                {
                    userId = model.userId
                };

                var deleteResult = await _dapper.DeleteAsync(ConnectionString.localdb.GetDescriptionText(), deleteModel).ConfigureAwait(false);


                var validateResult = validateTokenExp(Request);

                var querySql = $"Select * from [Userr]";
                var queryResult = await _dapper.QueryAsync<UserModel>(ConnectionString.localdb.GetDescriptionText(), querySql).ConfigureAwait(false);

                if (queryResult != null)
                {
                    var result = new List<UserResponseViewModel>();

                    foreach (var item in queryResult)
                    {
                        var data = new UserResponseViewModel()
                        {
                            userId = item.userId,
                            userAccount = item.userAccount,
                            userPassword = item.userPassword,
                            userPasswordSalt = item.userPasswordSalt,
                            userPolicy = item.userPolicy,
                            CreateTime = item.CreateTime,
                            UpdateTime = item.UpdateTime,
                            RefreshToken = validateResult
                        };

                        result.Add(data);
                    }

                    return Ok(result);
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
        public async Task<object> LogIn([FromBody] UserViewModel model)
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

        private string validateTokenExp(HttpRequest request)
        {
            var headers = request.Headers;
            var authorization = headers["Authorization"].ToString();
            var token = authorization.Replace("Bearer ", "");
            return _jwt.ValidateTokenExp(token);
        }

        private string validateTokenExpAndPolicy(HttpRequest request, UpdateViewModel model)
        {
            var headers = request.Headers;
            var authorization = headers["Authorization"].ToString();
            var token = authorization.Replace("Bearer ", "");
            return _jwt.ValidateTokenExpAndPolicy(token, model.userPolicy);
        }
    }
}
