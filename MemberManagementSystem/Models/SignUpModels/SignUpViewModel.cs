﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberManagementSystem.Models.SignUpModels
{
    public class SignUpViewModel
    {
        public string userAccount { get; set; }

        public string userPassword { get; set; }

        public string userPasswordSalt { get; set; }

    }
}