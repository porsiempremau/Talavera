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
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class MovimientosBarro
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FechaMovimiento { get; set; }

        [NotMapped]
        [ScaffoldColumn(false)]
        public string TipoMovimiento { get; set; }
        public string CodigoProducto { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Solo se permiten números positivos")]
        public Nullable<int> Unidades { get; set; }
        public Nullable<int> Provedor { get; set; }
        public string Locacion { get; set; }
        public string OrigenTranferencia { get; set; }

        public MovimientosBarro()
        {
            FechaMovimiento = DateTime.Today;
        }
    }
}
