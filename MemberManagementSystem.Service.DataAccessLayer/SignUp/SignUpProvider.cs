using MemberManagementSystem.Model.DataAccessLayer.SignUp;
using MemberManagementSystem.Model.Service.Common;
using MemberManagementSystem.Model.Service.SignUp;
using MemberManagementSystem.Platform.Utilities.Extensions;
using MemberManagementSystem.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static MemberManagementSystem.Model.Service.Common.Enum;

namespace MemberManagementSystem.Service.DataAccessLayer.SignUp
{
    public class SignUpProvider : ISignUpProvider
    {
        private readonly IDapper _dapper;

        public SignUpProvider(IDapper dapper)
        {
            // DI
            _dapper = dapper;
        }

        public async Task<int> SignUp(AccountDetailModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var insertEntity = new UserModel()
            {
                userAccount = model.userAccount,
                userPassword = model.userPassword,
                userPasswordSalt = model.userPasswordSalt,
                userPolicy = model.userPolicy,
                CreateTime = model.CreateTime,
                UpdateTime = model.UpdateTime,
            };

            return await _dapper.InsertAsync(ConnectionString.localdb.GetDescriptionText(), insertEntity).ConfigureAwait(false);
        }
    }
}
