namespace BusParkDispatcher.Models.Database
{
    using BusParkDispatcher.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Остановки : DbTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Остановки()
        {
            МаршрутыОстановки = new HashSet<МаршрутыОстановки>();
        }

        [Required]
        [StringLength(30)]
        public string Название { get; set; }

        [Required]
        [StringLength(150)]
        public string Описание { get; set; }

        [Key]
        public int КодОстановки { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<МаршрутыОстановки> МаршрутыОстановки { get; set; }
    }
}
