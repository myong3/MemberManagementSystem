using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberManagementSystem.Models.LogInModels
{
    public class LogInResponseViewModel
    {

        /// <summary>
        /// IsSuccessLogIn
        /// </summary>
        public bool IsSuccessLogIn { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Access_token
        /// </summary>
        public string Access_token { get; set; }
    }
}
