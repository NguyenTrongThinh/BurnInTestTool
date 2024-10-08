using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnInTestTool
{
    internal class DatabaseManager : DbContext
    {
        public DbSet<SysInfoData> SysInfoData { get; set; }
        public DbSet<Ctrsysinfo> CtrSysInfo { get; set; }
        public DbSet<Ecusysinfo> EcuSysInfo { get; set; }
        public DbSet<Pn51xxsysinfo> Pn51XxSysInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=sysinfo.db");
        }
        public void InitializeDatabase()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}
