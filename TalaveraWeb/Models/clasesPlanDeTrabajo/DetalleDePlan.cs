using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalaveraWeb.Models.clasesPlanDeTrabajo
{
    public class DetalleDePlan
    {
        public int Id { get; set; }

        public int IdCatalogoTalavera { get; set; }

        public string NombrePieza { get; set; }

        [Required, Display(Name = "Cantidad planeada")]
        public int CatidadPlaneada { get; set; }

        [Display(Name = "Cantidad real")]
        public int CantidadReal { get; set; }

        [Display(Name = "Observación")]
        public string Observacion { get; set; }

        [ScaffoldColumn(false)]
        public string TipoMovimiento { get; set; }
    }
}