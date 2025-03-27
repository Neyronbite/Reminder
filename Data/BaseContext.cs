using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class BaseContext : DbContext
    {
        protected readonly string connectionString;

        public BaseContext()
        {
            OnCreation();
        }

        public BaseContext(DbContextOptions<BaseContext> options) : base(options)
        {
            OnCreation();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                optionsBuilder.UseSqlite($"Data Source={documentsPath}\\reminder.db");

                optionsBuilder.UseSnakeCaseNamingConvention();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");
        }

        private void OnCreation()
        {
            Batteries.Init();
            Database.EnsureCreated();
            //Database.Migrate();
        }
    }
}
