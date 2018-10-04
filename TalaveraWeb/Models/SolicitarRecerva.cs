using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TalaveraWeb.Models
{
    public class SolicitarReserva
    { 
        public List<SelectListItem> lstTipoCapacidad { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Rango excedido")]
        public int? Unidades { get; set; }
        public int? TotalKg { get; set; }

        
    }
}