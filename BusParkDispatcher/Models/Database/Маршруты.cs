namespace BusParkDispatcher.Models.Database
{
    using BusParkDispatcher.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Маршруты : DbTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Маршруты()
        {
            Автобусы = new HashSet<Автобусы>();
        }

        public int НомерМаршрута { get; set; }

        //[Required]
        [StringLength(150)]
        public string Описание { get; set; }

        public int КодРасписания { get; set; }

        [Key]
        public int КодМаршрута { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Автобусы> Автобусы { get; set; }

        [ForeignKey("КодРасписания")]
        public virtual Расписания Расписания { get; set; }
    }
}
