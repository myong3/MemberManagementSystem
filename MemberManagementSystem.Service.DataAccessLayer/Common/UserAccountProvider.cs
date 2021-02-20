using MemberManagementSystem.Model.Service.Common;
using MemberManagementSystem.Model.Service.SignUp;
using MemberManagementSystem.Platform.Utilities.Extensions;
using MemberManagementSystem.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static MemberManagementSystem.Model.Service.Common.Enum;
using System.Linq;
using Dapper;

namespace MemberManagementSystem.Service.DataAccessLayer.Common
{
    public class UserAccountProvider : IUserAccountProvider
    {
        private readonly IDapper _dapper;

        public UserAccountProvider(IDapper dapper)
        {
            // DI
            _dapper = dapper;
        }

        public async Task<AccountDetailModel> QueryAccountDetail(string account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            var querySql = $"Select * from [Userr] where userAccount = '{account}'";
            return await _dapper.QueryFirstOrDefaultAsync<AccountDetailModel>(ConnectionString.localdb.GetDescriptionText(), querySql).ConfigureAwait(false);
        }

        public async Task<List<AccountDetailModel>> QueryAllAccountDetail()
        {
            var querySql = $"Select * from [Userr] order by [userId]";
            var result = await _dapper.QueryAsync<AccountDetailModel>(ConnectionString.localdb.GetDescriptionText(), querySql).ConfigureAwait(false);
            return result.ToList();
        }

        public async Task<int> UpdateAccountDetail(int userId, bool userPolicy)
        {
            var updateSql = @"UPDATE [Userr]
                                  SET [userPolicy] = @userPolicy,
                                  [UpdateTime] = @UpdateTime
                                  WHERE [userId] = @userId";

            var param = new DynamicParameters();
            param.Add("@userPolicy", userPolicy);
            param.Add("@UpdateTime", DateTime.Now);
            param.Add("@userId", userId);
            return await _dapper.ExecuteNonQueryAsync(ConnectionString.localdb.GetDescriptionText(), updateSql, param).ConfigureAwait(false);
        }
    }
}
