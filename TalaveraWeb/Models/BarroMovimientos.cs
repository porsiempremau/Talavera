//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TalaveraWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class BarroMovimientos
    {
        public int Id { get; set; }
        public string CodigoProducto { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Pedido")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FechaMovimiento { get; set; }
        public string TipoMovimiento { get; set; }

        [Required]
        public Nullable<int> Unidades { get; set; }
        public Nullable<int> Locacion { get; set; }
        public Nullable<int> OrigenTransferencia { get; set; }
        public string OrigenTabla { get; set; }
        public Nullable<int> PesoTotal { get; set; }

        public BarroMovimientos()
        {
            FechaMovimiento = DateTime.Today;
        }

    }
}