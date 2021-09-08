namespace BusParkDispatcher.Models.Database
{
    using BusParkDispatcher.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ТипыАвтобусов : DbTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ТипыАвтобусов()
        {
            Автобусы = new HashSet<Автобусы>();
        }

        [Required]
        [StringLength(50)]
        public string Наименование { get; set; }

        [Required]
        [StringLength(150)]
        public string Описание { get; set; }

        [Key]
        public int КодТипа { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Автобусы> Автобусы { get; set; }
    }
}
