namespace BusParkDispatcher.Models.Database
{
    using BusParkDispatcher.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Автобусы : DbTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Автобусы()
        {
            Водители = new HashSet<Водители>();
        }

        [Required]
        [StringLength(9)]
        public string РегистрационныйНомер { get; set; }

        public int КоличествоМест { get; set; }

        public int КодТипа { get; set; }

        public int КодМаршрута { get; set; }

        [Key]
        public int КодАвтобуса { get; set; }

        public virtual Маршруты Маршруты { get; set; }

        public virtual ТипыАвтобусов ТипыАвтобусов { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Водители> Водители { get; set; }
    }
}
