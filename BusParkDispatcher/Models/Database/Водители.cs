namespace BusParkDispatcher.Models.Database
{
    using BusParkDispatcher.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Водители : DbTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Водители()
        {
            Автобусы = new HashSet<Автобусы>();
        }

        [Required]
        [StringLength(150)]
        public string ФИО { get; set; }

        public long НомерТелефона { get; set; }

        [Key]
        public int КодВодителя { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Автобусы> Автобусы { get; set; }
    }
}
