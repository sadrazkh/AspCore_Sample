﻿using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Entity.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Context
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {


        public AppDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // call the base if you are using Identity service.
            // Important
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration()//=> parametr behesh esm kelas mapping ro midim =>baraye har kelas map in apply ro seda mizanim


            // Code here ...
        }
    }
}
