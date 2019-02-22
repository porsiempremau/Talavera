using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalaveraWeb.Models.MiBD
{
    public class CatalogoTalavera
    {
        public int Id { get; set; }

        [Display(Name = "Nombre pieza"), MaxLength(250), Required]
        public string NombrePieza { get; set; }

        [Required]
        public double? Altura { get; set; }

        [Display(Name = "Diámetro"), Required]
        public double? Diametro { get; set; }
        public string Tipo { get; set; }
        public string Imagen { get; set; }
    }
}