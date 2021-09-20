namespace BusParkDispatcher.Models.Database
{
    using BusParkDispatcher.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Время : DbTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Время()
        {
            ВремяРасписанияОстановки = new HashSet<ВремяРасписанияОстановки>();
        }

        [Column("Время")]
        public TimeSpan Время1 { get; set; }

        [Key]
        public int КодВремени { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ВремяРасписанияОстановки> ВремяРасписанияОстановки { get; set; }
    }
}
