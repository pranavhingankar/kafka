using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using kafka_3.Models;
using Microsoft.EntityFrameworkCore;

namespace kafka_3.DataAccess
{
    
        public class CRUDDbContext : DbContext
        {
            // DbSet for the Users table
            public DbSet<Users> Users { get; set; }
        public CRUDDbContext(DbContextOptions<CRUDDbContext> options) : base(options)
        {
        }

        // Constructor to configure database connection
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer("Data Source=PRANAV;Initial Catalog=CRUD;Integrated Security=True;Encrypt=False");
            }
        }
    
}
