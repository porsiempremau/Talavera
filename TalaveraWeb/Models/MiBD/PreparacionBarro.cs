namespace TalaveraWeb.Models.MiBD
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PreparacionBarro")]
    public partial class PreparacionBarro
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha preparación")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaPreparacion { get; set; }

        [StringLength(10)]
        [Display(Name = "Número de preparado")]
        public string NumPreparado { get; set; }

        [Display(Name = "Barro negro")]
        public int? BarroNegro { get; set; }

        [Display(Name = "Barro blanco")]
        public int? BarroBlanco { get; set; }

        public int? Recuperado { get; set; }

        [StringLength(10)]
        [Display(Name = "En piedra")]
        public string EnPiedra { get; set; }

        [StringLength(20)]
        [Display(Name = "Tiempo de agitación")]
        public string TiempoAgitacion { get; set; }

        [Display(Name = "Número de tambos")]
        public int? NumTambos { get; set; }

        [StringLength(10)]
        [Display(Name = "Desperdicio mojado")]
        public string DesperdicioMojado { get; set; }

        [StringLength(50)]
        public string Comentario { get; set; }

        [StringLength(10)]
        public string Estado { get; set; }

        [Display(Name = "Locación")]
        public int? Locacion { get; set; }

        [StringLength(50)]
        public string Editor { get; set; }

        public DateTime? FechaEdicion { get; set; }
    }
}
