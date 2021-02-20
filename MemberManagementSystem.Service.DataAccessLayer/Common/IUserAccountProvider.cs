using MemberManagementSystem.Model.Service.Common;
using MemberManagementSystem.Model.Service.SignUp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MemberManagementSystem.Service.DataAccessLayer.Common
{
    public interface IUserAccountProvider
    {
        /// <summary>
        /// QueryAccountData
        /// </summary>
        /// <param name="account">account</param>
        /// <returns></returns>
        Task<AccountDetailModel> QueryAccountDetail(string account);

        /// <summary>
        /// QueryAccountData
        /// </summary>
        /// <returns></returns>
        Task<List<AccountDetailModel>> QueryAllAccountDetail();

        /// <summary>
        /// UpdateAccountDetail
        /// </summary>
        /// <returns></returns>
        Task<int> UpdateAccountDetail(int userId, bool userPolicy);
    }
}
