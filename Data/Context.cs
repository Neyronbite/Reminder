using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Context : BaseContext
    {
        public DbSet<Day> Days { get; set; }
        public DbSet<Event> Events { get; set; }

        public Context() : base()
        {
        }

        public Context(DbContextOptions<BaseContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Day>().HasMany(d => d.Events).WithOne(e => e.Day).HasForeignKey(e => e.DayId);
        }
    }
}
