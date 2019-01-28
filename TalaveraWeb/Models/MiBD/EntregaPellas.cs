namespace TalaveraWeb.Models.MiBD
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EntregaPellas
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de solicitud")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaMovimiento { get; set; }

        public string Responsable { get; set; }

        public string TipoMovimiento { get; set; }

        [Display(Name = "Catidad de pellas entregadas")]
        public int? CantidadPellas { get; set; }

        [Display(Name = "Numero de carga")]
        public string NumCarga { get; set; }

        public string Editor { get; set; }

        public DateTime? FechaEdicion { get; set; }

        public int? Locacion { get; set; }

        [Display(Name = "Observación")]
        [StringLength(250, ErrorMessage = "La longitud maxima para {0} es de {1}")]
        public string Observacion { get; set; }
    }
}
