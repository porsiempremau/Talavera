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
    
    public partial class PreparacionPellas
    {
        public int Id { get; set; }
        public Nullable<int> Fuente { get; set; }
        public string NumCarga { get; set; }
        public Nullable<System.DateTime> FechaVaciado { get; set; }
        public Nullable<System.DateTime> FechaLevantado { get; set; }
        public Nullable<System.DateTime> FechaInicoPisado { get; set; }
        public Nullable<System.DateTime> FechaFinPisado { get; set; }
        public Nullable<int> NumPeyas { get; set; }
        public Nullable<int> Restante { get; set; }
        public Nullable<int> CargaTotal { get; set; }
    }
}
