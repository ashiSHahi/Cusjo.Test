using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CusjoAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> AppUsers { get; set; }

        public DbSet<Entities> Entities_lu { get; set; }

        public DbSet<UserEntity_assoc> UserEntity_Assocs { get; set; }

    }
}
