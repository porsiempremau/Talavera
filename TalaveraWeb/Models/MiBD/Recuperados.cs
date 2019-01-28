namespace TalaveraWeb.Models.MiBD
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Recuperados
    {
        public int Id { get; set; }

        public DateTime? FechaRecuperado { get; set; }

        [StringLength(50)]
        public string Responsable { get; set; }

        public int? Cantidad { get; set; }

        [StringLength(10)]
        public string NumRecuperado { get; set; }

        [StringLength(1)]
        public string TipoMovimiento { get; set; }

        [StringLength(50)]
        public string Editor { get; set; }

        public DateTime? FechaEdicion { get; set; }

        public int? Locacion { get; set; }
    }
}
