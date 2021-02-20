using System;
using System.Collections.Generic;
using System.Text;

namespace MemberManagementSystem.Model.Service.LogIn
{
    public class LogInResponseServiceModel
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
