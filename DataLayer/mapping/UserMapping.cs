using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Entity.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.mapping
{
     public class UserMapping: IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            //builder ===> har gohi bekheim bokhorim mishe  dige
            
        }
    }
}
