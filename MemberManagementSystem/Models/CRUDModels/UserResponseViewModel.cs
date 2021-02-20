using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberManagementSystem.Models.CRUDModels
{

    public class UserResponseViewModel
    {
        /// <summary>
        /// User
        /// </summary>
        public List<UserResponseModel> UserList { get; set; }

        /// <summary>
        /// RefreshToken
        /// </summary>
        public string RefreshToken { get; set; }
    }

    public class UserResponseModel
    {
        public int userId { get; set; }

        /// <summary>
        /// userAccount
        /// </summary>
        public string userAccount { get; set; }

        /// <summary>
        /// userPassword
        /// </summary>
        public string userPassword { get; set; }

        /// <summary>
        /// userPolicy
        /// </summary>
        public bool userPolicy { get; set; }

        /// <summary>
        /// userPasswordSalt
        /// </summary>
        public string userPasswordSalt { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// UpdateTime
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
