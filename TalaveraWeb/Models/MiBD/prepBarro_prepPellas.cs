namespace TalaveraWeb.Models.MiBD
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class prepBarro_prepPellas
    {
        public int Id { get; set; }

        [StringLength(10)]
        public string NumCarga { get; set; }

        [StringLength(10)]
        public string NumPreparado { get; set; }

        [StringLength(50)]
        public string Editor { get; set; }

        public DateTime? FechaEdicion { get; set; }
    }
}
