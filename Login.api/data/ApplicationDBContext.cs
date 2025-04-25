using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Login.api.models;
using Microsoft.EntityFrameworkCore;

namespace Login.api.data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}