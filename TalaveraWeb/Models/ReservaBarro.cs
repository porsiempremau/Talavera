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
        public int? Capacidad { get; set; }
        public int? Unidades { get; set; }
        public int? TotalKg { get; set; }        
    }
}