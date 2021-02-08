using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MemberManagementSystem.Extensions;
using MemberManagementSystem.Helper;
using MemberManagementSystem.Models.LogInModels;
using MemberManagementSystem.Models.ProviderModels;
using MemberManagementSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static MemberManagementSystem.Models.Enums;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;

namespace MemberManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class LogInController : Controller
    {
        private readonly IDapper _dapper;

        private readonly JwtHelpers _jwt;

        public LogInController(IDapper dapper, JwtHelpers jwt)
        {
            // DI
            _dapper = dapper;
            _jwt = jwt;

        }

        // GET: api/login
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public IEnumerable<string> Get()
        {
            var validateResult = validateTokenExp(Request);
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// POST api/login/GetPassword
        /// 取得account對應的 password & password salt
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("LogInGetToken")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]

        public async Task<IActionResult> LogInGetToken([FromBody] LogInRequestViewModel model)
        {
            try
            {
                var result = new LogInResponseViewModel()
                {
                    IsSuccessLogIn = false
                };

                var querySql = $"Select * from [Userr] where userAccount = '{model.userAccount}'";
                var queryResult = await _dapper.QueryFirstOrDefaultAsync<UserModel>(ConnectionString.localdb.GetDescriptionText(), querySql).ConfigureAwait(false);

                if (queryResult != null)
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
                                result.IsSuccessLogIn = true;
                                result.Message = "管理員登入成功";
                                result.Access_token = _jwt.GenerateToken(model.userAccount, true);
                                setTokenCookie(result.Access_token);
                                return Ok(result);
                            }
                            else
                            {
                                result.IsSuccessLogIn = true;
                                result.Message = "使用者登入成功";
                                result.Access_token = _jwt.GenerateToken(model.userAccount);
                                setTokenCookie(result.Access_token);
                                return Ok(result);
                            }
                        }
                        else
                        {
                            result.Message = "密碼錯誤，請再嘗試一次";
                            return Ok(result);
                        }
                    }
                }
                else
                {
                    result.Message = "找不到您的帳戶，請再嘗試一次";
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string validateTokenExp(HttpRequest request)
        {
            var headers = request.Headers;
            var authorization = headers["Authorization"].ToString();
            var token = authorization.Replace("Bearer ", "");
            return _jwt.ValidateTokenExp(token);
        }
    }
}
