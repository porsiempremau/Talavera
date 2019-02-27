using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalaveraWeb.Models.MiBD
{
    public class PersonalTalavera
    {
        public int Id { get; set; }
        [Required, MaxLength(150)]
        public string Nombre { get; set; }
        [MaxLength(150), Display(Name = "Apellido paterno")]
        public string APaterno { get; set; }
        [MaxLength(150), Display(Name = "Apellido materno")]
        public string AMaterno { get; set; }
        [MaxLength(100)]
        public string Puesto { get; set; }
    }
}