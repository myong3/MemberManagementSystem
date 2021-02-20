using MemberManagementSystem.Platform.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MemberManagementSystem.Model.Service.Common;

namespace MemberManagementSystem.Service.JWToken
{
    public class JWTokenService : IJWTokenService
    {
        private readonly JwtHelpers _jwt;

        public JWTokenService(JwtHelpers jwt)
        {
            // DI
            _jwt = jwt;
        }

        public async Task<ServiceResult<string>> GenerateToken(string userName, bool IsAdmin = false, int expireMinutes = 30)
        {
            try
            {
                var result = new ServiceResult<string>();
                result.Data = _jwt.GenerateToken(userName, IsAdmin, expireMinutes);

                return result;
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(false, 900000, ex.ToString(), ex);
            }
        }

        public async Task<ServiceResult<string>> validateTokenExp(HttpRequest request)
        {
            try
            {
                var result = new ServiceResult<string>();
                var headers = request.Headers;
                var authorization = headers["Authorization"].ToString();
                var token = authorization.Replace("Bearer ", "");
                result.Data = _jwt.ValidateTokenExp(token);
                result.IsOk = true;
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceResult<string>(false, 900000, ex.ToString(), ex);
            }
        }

        public async Task<ServiceResult<string>> validateTokenExpAndPolicy(HttpRequest request, AccountDetailModel model)
        {
            var result = new ServiceResult<string>();

            var headers = request.Headers;
            var authorization = headers["Authorization"].ToString();
            var token = authorization.Replace("Bearer ", "");
            result.Data = _jwt.ValidateTokenExpAndPolicy(token, model.userAccount, model.userPolicy);
            result.IsOk = true;

            return result;
        }
    }
}
