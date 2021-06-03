using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PoWebApi.Models;

namespace PoWebApi.Data
{
    public class PoContext : DbContext
    {
        public PoContext (DbContextOptions<PoContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<PoLine> PoLines { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Employee>(e =>
            {
                e.HasIndex(p => p.Login).IsUnique();
            });
        }

        public DbSet<PoWebApi.Models.PoLine> PoLine { get; set; }
    }
}
