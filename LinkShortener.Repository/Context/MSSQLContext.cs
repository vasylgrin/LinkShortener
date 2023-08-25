using LinkShortener.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkShortener.Repository.Context
{
    internal sealed class MSSQLContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UrlModel> Urls { get; set; }

        public MSSQLContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=LinkShortenerdb;Trusted_Connection=True;");
        }
    }
}
