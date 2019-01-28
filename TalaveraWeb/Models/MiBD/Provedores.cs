namespace TalaveraWeb.Models.MiBD
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Provedores
    {
        public int id { get; set; }

        [StringLength(250)]
        public string Nombre { get; set; }

        [StringLength(15)]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [StringLength(15)]
        [Display(Name = "Segundo Teléfono")]
        public string Telefono2 { get; set; }

        [StringLength(50)]
        [Display(Name = "Número")]
        public string Numero { get; set; }

        [StringLength(50)]
        public string Calle { get; set; }

        [StringLength(50)]
        public string Colonia { get; set; }

        [StringLength(50)]
        public string Municipio { get; set; }

        [StringLength(50)]
        public string Estado { get; set; }
    }
}
