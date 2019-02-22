using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalaveraWeb.Models
{
    public class PellasDisponibles
    {
        public string NumeroCarga { get; set; }        
        public int? UnidadesDisponibles { get; set; }
        public int? UnidadesSolicitadas { get; set; }
        public string Tipo { get; set; }
        public double? Capacidad { get; set; }
        public double? TotalKg { get; set; }
        
    }
}