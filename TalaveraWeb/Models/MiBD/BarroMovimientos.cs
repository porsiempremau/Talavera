namespace TalaveraWeb.Models.MiBD
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BarroMovimientos
    {
        public int Id { get; set; }

        [StringLength(5)]
        public string CodigoProducto { get; set; }

        [Column(TypeName = "date")]
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Pedido")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaMovimiento { get; set; }

        [StringLength(2)]
        public string TipoMovimiento { get; set; }

        [Required]
        public double? Unidades { get; set; }

        [Display(Name = "Locación")]
        public int? Locacion { get; set; }

        [Display(Name = "Peso total")]
        public double? PesoTotal { get; set; }
        public int? OrigenTransferencia { get; set; }

        [StringLength(20)]
        public string OrigenTabla { get; set; }

        [StringLength(20)]
        public string OrigenVariacion { get; set; }

        [StringLength(50)]
        public string Editor { get; set; }

        public DateTime? FechaEdicion { get; set; }
    }
}
