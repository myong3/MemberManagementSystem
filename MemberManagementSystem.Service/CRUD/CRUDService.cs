using MemberManagementSystem.Model.Service.Common;
using MemberManagementSystem.Platform.Utilities;
using MemberManagementSystem.Service.DataAccessLayer.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MemberManagementSystem.Service.CRUD
{
    public class CRUDService : ICRUDService
    {

        private readonly IUserAccountProvider _userAccountProvider;


        public CRUDService(IUserAccountProvider userAccountProvider)
        {
            // DI
            _userAccountProvider = userAccountProvider;
        }

        /// <summary>
        /// GetAllUserDetail
        /// </summary>
        /// <returns></returns>
        public async Task<ServiceResult<List<AccountDetailModel>>> GetAllUserDetail()
        {
            try
            {
                var result = new ServiceResult<List<AccountDetailModel>>();
                var queryResult = await _userAccountProvider.QueryAllAccountDetail().ConfigureAwait(false);

                result.IsOk = true;
                result.Code = 200000;
                result.Data = queryResult;

                return result;
            }
            catch (Exception ex)
            {
                return new ServiceResult<List<AccountDetailModel>>(false, 900000, ex.ToString(), ex);
            }
        }

        public async Task<ServiceResult<List<AccountDetailModel>>> UpdateUserDetail(AccountDetailModel model)
        {
            try
            {
                var result = new ServiceResult<List<AccountDetailModel>>();
                var UpdateResult = await _userAccountProvider.UpdateAccountDetail(model.userId, model.userPolicy).ConfigureAwait(false);
                var queryResult = await _userAccountProvider.QueryAllAccountDetail().ConfigureAwait(false);
                result.IsOk = true;
                result.Code = 200000;
                result.Data = queryResult;

                return result;
            }
            catch (Exception ex)
            {
                return new ServiceResult<List<AccountDetailModel>>(false, 900000, ex.ToString(), ex);
            }
        }
    }
}
