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
    
    public partial class EntregaPellas
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> FechaMovimiento { get; set; }
        public string Responsable { get; set; }
        public string TipoMovimiento { get; set; }
        public Nullable<int> CantidadPellas { get; set; }
        public string NumCarga { get; set; }
        public string Editor { get; set; }
        public Nullable<System.DateTime> FechaEdicion { get; set; }
        public Nullable<int> Locacion { get; set; }
    }
}
