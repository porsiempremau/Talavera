using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalaveraWeb.Models.MiBD
{
    public class PlanDeTrabajo
    {
        public int Id { get; set; }

        [Required]
        public int IdPersonal { get; set; }

        [MaxLength(15), Display(Name = "Numero de orden")]
        public string NumeroOrden { get; set; }
                
        [DataType(DataType.Date), Display(Name = "Fecha de inicio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaInicio { get; set; }
                
        [DataType(DataType.Date), Display(Name = "Fecha final")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaFin { get; set; }

        [MaxLength(15), Display(Name = "Etapa actual")]
        public string EtapaPlan { get; set; }

        [Display(Name = "Observación")]
        public string Observacion { get; set; }
    }
}