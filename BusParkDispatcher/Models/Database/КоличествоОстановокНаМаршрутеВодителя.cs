namespace BusParkDispatcher.Models.Database
{
    using BusParkDispatcher.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class КоличествоОстановокНаМаршрутеВодителя : DbTable
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(150)]
        public string ФИО { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long НомерТелефона { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(9)]
        public string РегистрационныйНомерАвтобуса { get; set; }

        public int? КоличествоОстановок { get; set; }
    }
}
