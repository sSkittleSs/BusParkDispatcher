namespace BusParkDispatcher.Models.Database
{
    using BusParkDispatcher.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ВремяРасписанияОстановки : DbTable
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int КодВремени { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int КодРасписания { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int КодОстановки { get; set; }

        [ForeignKey("КодВремени")]
        public virtual Время Время { get; set; }

        [ForeignKey("КодРасписания")]
        public virtual Расписания Расписания { get; set; }

        [ForeignKey("КодОстановки")]
        public virtual Остановки Остановки { get; set; }
    }
}
