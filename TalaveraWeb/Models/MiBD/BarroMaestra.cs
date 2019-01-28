namespace TalaveraWeb.Models.MiBD
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BarroMaestra")]
    public partial class BarroMaestra
    {
        public int Id { get; set; }

        [StringLength(5)]
        public string CodigoProducto { get; set; }

        public int? Capacidad { get; set; }

        [StringLength(10)]
        public string Tipo { get; set; }

        [StringLength(50)]
        public string Editor { get; set; }

        public DateTime? FechaEdicion { get; set; }
    }
}
