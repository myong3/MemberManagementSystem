using MemberManagementSystem.Model.Service.Common;
using MemberManagementSystem.Model.Service.SignUp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MemberManagementSystem.Service.DataAccessLayer.SignUp
{
    public interface ISignUpProvider
    {
        /// <summary>
        /// SignUp
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        Task<int> SignUp(AccountDetailModel model);
    }
}
