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
    
    public partial class MovimientosBarro
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> FechaMovimiento { get; set; }
        public string TipoMovimiento { get; set; }
        public string CodigoProducto { get; set; }
        public Nullable<int> Unidades { get; set; }
        public Nullable<int> Provedor { get; set; }
        public string Locacion { get; set; }
        public string OrigenTranferencia { get; set; }
    }
}
