using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TalaveraWeb.Models.MiBD;

namespace TalaveraWeb.Models
{
    public class BarroMovimientosConMerma
    {
        public int Id { get; set; }

        [StringLength(5)]
        public string CodigoProducto { get; set; }

        [Column(TypeName = "date")]
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Pedido")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaMovimiento { get; set; }

        [StringLength(2)]
        public string TipoMovimiento { get; set; }

        [Required]
        public double? Unidades { get; set; }

        [Display(Name = "Locación")]
        public int? Locacion { get; set; }

        [Display(Name = "Peso total")]
        public double? PesoTotal { get; set; }
        public int? OrigenTransferencia { get; set; }

        [StringLength(20)]
        public string OrigenTabla { get; set; }

        [StringLength(20)]
        public string OrigenVariacion { get; set; }

        [StringLength(50)]
        public string Editor { get; set; }

        public DateTime? FechaEdicion { get; set; }


        public double Merma { get; set; }
        public BarroMovimientosConMerma()
        {

        }

        public void setBarroMovimientos(BarroMovimientos pBM)
        {
            this.Id = pBM.Id;
            this.CodigoProducto = pBM.CodigoProducto;
            this.FechaMovimiento = pBM.FechaMovimiento;
            this.TipoMovimiento = pBM.TipoMovimiento;
            this.Unidades = pBM.Unidades;
            this.Locacion = pBM.Locacion;
            this.OrigenTabla = pBM.OrigenTabla;
            this.OrigenTransferencia = pBM.OrigenTransferencia;
            this.OrigenVariacion = pBM.OrigenVariacion;
            this.PesoTotal = pBM.PesoTotal;
            this.Editor = pBM.Editor;
            this.FechaEdicion = pBM.FechaEdicion;            
        }

        public BarroMovimientos getBarroMovimientos()
        {
            BarroMovimientos bm =  new BarroMovimientos()
                {
                    Id = this.Id,
                    CodigoProducto = this.CodigoProducto,
                    FechaMovimiento = this.FechaMovimiento,
                    TipoMovimiento = this.TipoMovimiento,
                    Unidades = this.Unidades,
                    Locacion = this.Locacion,
                    OrigenTabla = this.OrigenTabla,
                    OrigenTransferencia = this.OrigenTransferencia,
                    OrigenVariacion = this.OrigenVariacion,
                    PesoTotal = this.PesoTotal,
                    Editor = this.Editor,
                    FechaEdicion = this.FechaEdicion,
                };
            return bm;
        }

    }
}