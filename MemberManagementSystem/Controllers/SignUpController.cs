using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MemberManagementSystem.Model.Service.SignUp;
using MemberManagementSystem.Models.SignUpModels;
using MemberManagementSystem.Service.SignUp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MemberManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : Controller
    {
        private readonly ISignUpService _signUpService;

        public SignUpController(ISignUpService signUpService)
        {
            // DI
            _signUpService = signUpService;
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
                    var serviceData = new CheckAccountServiceModel()
                    {
                        userAccount = model.userAccount
                    };
                    var serviceResult = await _signUpService.CheckAccount(serviceData).ConfigureAwait(false);

                    if (serviceResult.IsOk)
                    {
                        if (serviceResult.Data)
                        {
                            return Json(serviceResult);
                        }
                        else
                        {
                            return NoContent();
                        }
                    }
                    else
                    {
                        return BadRequest(new { message = serviceResult.Message, model });
                    }
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
                var data = new SignUpServiceModel()
                {
                    userAccount = model.userAccount,
                    userPassword = model.userPassword
                };

                var serviceResult = await _signUpService.SignUp(data).ConfigureAwait(false);

                if (serviceResult.IsOk)
                {
                    return Json(serviceResult);
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
    }
}
