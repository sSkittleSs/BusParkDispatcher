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
        {
        }

        public virtual DbSet<Автобусы> Автобусы { get; set; }
        public virtual DbSet<Водители> Водители { get; set; }
        public virtual DbSet<Время> Время { get; set; }
        public virtual DbSet<ВремяРасписанияОстановки> ВремяРасписанияОстановки { get; set; }
        public virtual DbSet<Маршруты> Маршруты { get; set; }
        public virtual DbSet<Остановки> Остановки { get; set; }
        public virtual DbSet<Расписания> Расписания { get; set; }
        public virtual DbSet<ТипыАвтобусов> ТипыАвтобусов { get; set; }
        public virtual DbSet<КоличествоОстановокНаМаршрутеВодителя> КоличествоОстановокНаМаршрутеВодителя { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Автобусы>()
                .HasMany(e => e.Водители)
                .WithMany(e => e.Автобусы)
                .Map(m => m.ToTable("АвтобусыВодители").MapLeftKey("КодАвтобуса").MapRightKey("КодВодителя"));
        }
    }
}
