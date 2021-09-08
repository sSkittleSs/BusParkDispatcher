using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BusParkDispatcher.Models.Database
{
    public partial class ApplicationContext : DbContext
    {
        public ApplicationContext()
            : base("data source=.\\SQLEXPRESS;initial catalog=АвтобусныйПарк;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework")
            //: base("name=ApplicationContext")
        {
        }

        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Автобусы> Автобусы { get; set; }
        public virtual DbSet<Водители> Водители { get; set; }
        public virtual DbSet<Время> Время { get; set; }
        public virtual DbSet<Маршруты> Маршруты { get; set; }
        public virtual DbSet<МаршрутыОстановки> МаршрутыОстановки { get; set; }
        public virtual DbSet<Остановки> Остановки { get; set; }
        public virtual DbSet<Расписания> Расписания { get; set; }
        public virtual DbSet<ТипыАвтобусов> ТипыАвтобусов { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Автобусы>()
                .HasMany(e => e.Водители)
                .WithMany(e => e.Автобусы)
                .Map(m => m.ToTable("АвтобусыВодители").MapLeftKey("КодАвтобуса").MapRightKey("КодВодителя"));

            modelBuilder.Entity<Время>()
                .HasMany(e => e.Расписания)
                .WithMany(e => e.Время)
                .Map(m => m.ToTable("ВремяРасписание").MapLeftKey("КодВремени").MapRightKey("КодРасписания"));
        }
    }
}
