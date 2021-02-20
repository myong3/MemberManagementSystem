using MemberManagementSystem.Model.Service.LogIn;
using MemberManagementSystem.Platform.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MemberManagementSystem.Service.LogIn
{
    public interface ILogInService
    {
        /// <summary>
        /// LogInGetToken
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        Task<ServiceResult<LogInResponseServiceModel>> LogInGetToken(LogInServiceModel model);

    }
}
