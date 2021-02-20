using MemberManagementSystem.Model.Service.SignUp;
using MemberManagementSystem.Platform.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MemberManagementSystem.Service.SignUp
{
    public interface ISignUpService
    {
        Task<ServiceResult<bool>> CheckAccount(CheckAccountServiceModel model);

        Task<ServiceResult<int>> SignUp(SignUpServiceModel model);

    }
}
