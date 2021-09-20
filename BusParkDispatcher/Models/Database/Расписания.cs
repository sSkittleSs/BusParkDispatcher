namespace BusParkDispatcher.Models.Database
{
    using BusParkDispatcher.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Расписания : DbTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Расписания()
        {
            ВремяРасписанияОстановки = new HashSet<ВремяРасписанияОстановки>();
            Маршруты = new HashSet<Маршруты>();
        }

        [Column(TypeName = "date")]
        public DateTime Дата { get; set; }

        public bool ЯвляетсяВыходным { get; set; }

        [Key]
        public int КодРасписания { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ВремяРасписанияОстановки> ВремяРасписанияОстановки { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Маршруты> Маршруты { get; set; }
    }
}
