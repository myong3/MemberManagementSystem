using MemberManagementSystem.Model.Service.Common;
using MemberManagementSystem.Platform.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MemberManagementSystem.Service.CRUD
{
    public interface ICRUDService
    {
        /// <summary>
        /// GetAllUserDetail
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<List<AccountDetailModel>>> GetAllUserDetail();

        /// <summary>
        /// GetAllUserDetail
        /// </summary>
        /// <returns></returns>
        Task<ServiceResult<List<AccountDetailModel>>> UpdateUserDetail(AccountDetailModel model);
    }
}
