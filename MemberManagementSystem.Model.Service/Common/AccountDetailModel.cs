using System;
using System.Collections.Generic;
using System.Text;

namespace MemberManagementSystem.Model.Service.Common
{
    public class AccountDetailModel
    {
        /// <summary>
        /// userId
        /// </summary>
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
