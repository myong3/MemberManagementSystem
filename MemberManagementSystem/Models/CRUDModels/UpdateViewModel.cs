using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberManagementSystem.Models.CRUDModels
{
    public class UpdateViewModel
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
