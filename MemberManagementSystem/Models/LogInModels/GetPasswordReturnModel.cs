using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberManagementSystem.Models.LogInModels
{
    public class GetPasswordReturnModel
    {
        /// <summary>
        /// userPassword
        /// </summary>
        public string userPassword { get; set; }

        /// <summary>
        /// userPasswordSalt
        /// </summary>
        public string userPasswordSalt { get; set; }
    }
}
