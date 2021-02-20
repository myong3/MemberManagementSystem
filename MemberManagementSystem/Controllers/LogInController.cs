using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MemberManagementSystem.Models.LogInModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using MemberManagementSystem.Platform.Utilities;
using MemberManagementSystem.Service.LogIn;
using MemberManagementSystem.Model.Service.LogIn;
using MemberManagementSystem.Service.JWToken;

namespace MemberManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class LogInController : Controller
    {
        private readonly IJWTokenService _jWTokenService;

        private readonly ILogInService _logInService;

        public LogInController(IJWTokenService jWTokenService, ILogInService logInService)
        {
            // DI
            _jWTokenService = jWTokenService;
            _logInService = logInService;
        }

        // GET: api/login
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public IEnumerable<string> Get()
        {
            var validateResult = _jWTokenService.validateTokenExp(Request);
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// POST api/login/LogInGetToken
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
                var result = new LogInResponseViewModel();

                var data = new LogInServiceModel()
                {
                    userAccount = model.userAccount,
                    userPassword = model.userPassword
                };

                var serviceResult = await _logInService.LogInGetToken(data).ConfigureAwait(false);

                if (serviceResult.IsOk)
                {
                    result.IsSuccessLogIn = serviceResult.Data.IsSuccessLogIn;
                    result.Message = serviceResult.Data.Message;
                    result.Access_token = serviceResult.Data.Access_token;
                    if (serviceResult.Data.IsSuccessLogIn)
                    {
                        setTokenCookie(result.Access_token);
                    }
                    return Json(result);
                }
                else
                {
                    return BadRequest(new { message = serviceResult.Message, model });
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
    }
}
