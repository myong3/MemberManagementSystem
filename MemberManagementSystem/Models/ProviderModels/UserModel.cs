using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberManagementSystem.Models.ProviderModels
{
    [Table("Userr")]
    public class UserModel
    {
        /// <summary>
        /// userId
        /// </summary>
        [Key]
        public int userId { get; set; }

        /// <summary>
        /// userAccount
        /// </summary>
        public string userAccount { get; set; }

        /// <summary>
        /// userPassword
        /// </summary>
        public string userPassword { get; set; }

    }
}
