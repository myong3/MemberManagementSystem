using MemberManagementSystem.Platform.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MemberManagementSystem.Model.Service.Common;

namespace MemberManagementSystem.Service.JWToken
{
    public interface IJWTokenService
    {
        /// <summary>
        /// GenerateToken
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<string>> GenerateToken(string userName, bool IsAdmin = false, int expireMinutes = 30);

        /// <summary>
        /// validateTokenExp
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ServiceResult<string>> validateTokenExp(HttpRequest request);

        /// <summary>
        /// validateTokenExpAndPolicy
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ServiceResult<string>> validateTokenExpAndPolicy(HttpRequest request, AccountDetailModel model);
    }
}
