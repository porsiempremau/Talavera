using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TalaveraWeb.Models;
using TalaveraWeb.Models.MiBD;

namespace TalaveraWeb.Models
{
    public class PreparacionBarroConsumo : PreparacionBarro
    {
        public List<ReservaBarroPreparado> lstConsumoBarroNegro { get; set; }
        public List<ReservaBarroPreparado> lstConsumoBarroBlanco { get; set; }

        //Metodos
        public void getPreparacionBarro(PreparacionBarro pPB)
        {
            this.Id = pPB.Id;
            this.FechaPreparacion = pPB.FechaPreparacion;
            this.NumPreparado = pPB.NumPreparado;
            this.BarroNegro = pPB.BarroNegro;
            this.BarroBlanco = pPB.BarroBlanco;
            this.Recuperado = pPB.Recuperado;
            this.EnPiedra = pPB.EnPiedra;
            this.TiempoAgitacion = pPB.TiempoAgitacion;
            this.NumTambos = pPB.NumTambos;
            this.DesperdicioMojado = pPB.DesperdicioMojado;
            this.Comentario = pPB.Comentario;
            this.Estado = pPB.Estado;
            this.Locacion = pPB.Locacion;
        }
    }
}