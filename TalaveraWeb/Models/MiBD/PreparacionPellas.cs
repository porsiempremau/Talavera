namespace TalaveraWeb.Models.MiBD
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PreparacionPellas
    {
        public int Id { get; set; }

        public int? Fuente { get; set; }

        [StringLength(10)]
        [Display(Name = "Número de Carga")]
        public string NumCarga { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha vaciado")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaVaciado { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha se saca el barro")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaLevantado { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha pisado de barro")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaInicoPisado { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha fin de pisado de barro")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaFinPisado { get; set; }

        [Display(Name = "Número de pellas")]
        public int? NumPeyas { get; set; }

        public int? Restante { get; set; }

        [Display(Name = "Peso de pellas KG")]
        public int? CargaTotal { get; set; }

        [StringLength(50)]
        public string Editor { get; set; }

        public DateTime? FechaEdicion { get; set; }

        [Display(Name = "Locación")]
        public int? Locacion { get; set; }
    }
}
