﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace DataLayer.Entity.User
{
    public class AppUser : IdentityUser
    {
        public bool Active { get; set; }
        public DateTime RegistrationTime { get; set; }

    }
}
