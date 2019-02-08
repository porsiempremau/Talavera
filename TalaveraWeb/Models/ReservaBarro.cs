using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TalaveraWeb.Models
{
    public class ReservaBarro
    {
        public string CodigoBarro { get; set; }
        public string Tipo { get; set; }
        public double? Capacidad { get; set; }
        public double? Unidades { get; set; }
        public double? TotalKg { get; set; }        
    }
}