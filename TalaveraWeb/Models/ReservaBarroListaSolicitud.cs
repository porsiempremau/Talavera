using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalaveraWeb.Models
{
    public class ReservaBarroListaSolicitud
    {
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de solicitud")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaMovimiento { get; set; }

        [Required]
        public string Responsable { get; set; }

        public string TipoMovimiento { get; set; }

        [Display(Name = "Catidad de pellas solicitadas")]
        public int? CantidadPellas { get; set; }

        [Display(Name = "Numero de carga")]
        public string NumCarga { get; set; }
                
        public int? Locacion { get; set; }

        [Display(Name = "Observación")]
        [StringLength(250, ErrorMessage = "La longitud maxima para {0} es de {1}")]
        public string Observacion { get; set; }
        public List<PellasDisponibles> lstReservas { get; set; }
    }
}