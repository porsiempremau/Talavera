﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalaveraWeb.Models.MiBD;

namespace TalaveraWeb.Models
{
    public class PreparacionPellasConPreBar : PreparacionPellas
    {
        public List<string> lstPreBar { get; set; }
                
        public IEnumerable<string> SelectedPreparados { get; set; }
        public IEnumerable<SelectListItem> lstPreparados { get; set; }
        public PreparacionPellasConPreBar() 
        {
            SelectedPreparados = new List<string>();
            lstPreparados = new List<SelectListItem>();
        }

        public PreparacionPellasConPreBar(PreparacionPellas pPP)
        {
            this.Id = pPP.Id;
            this.Fuente = pPP.Fuente;
            this.NumCarga = pPP.NumCarga;
            this.FechaVaciado = pPP.FechaVaciado;
            this.FechaLevantado = pPP.FechaLevantado;
            this.FechaInicoPisado = pPP.FechaInicoPisado;
            this.FechaFinPisado = pPP.FechaFinPisado;
            this.NumPeyas = pPP.NumPeyas;
            this.Restante = pPP.Restante;
            this.CargaTotal = pPP.CargaTotal;
        }

        public void setPreparadoPella(PreparacionPellas pPP)
        {
            this.Id = pPP.Id;
            this.Fuente = pPP.Fuente;
            this.NumCarga = pPP.NumCarga;
            this.FechaVaciado = pPP.FechaVaciado;
            this.FechaLevantado = pPP.FechaLevantado;
            this.FechaInicoPisado = pPP.FechaInicoPisado;
            this.FechaFinPisado = pPP.FechaFinPisado;
            this.NumPeyas = pPP.NumPeyas;
            this.Restante = pPP.Restante;
            this.CargaTotal = pPP.CargaTotal;
        }

        public PreparacionPellas getPreparadoPella()
        {
            PreparacionPellas pp = new PreparacionPellas()
            {
                Id = this.Id,
                Fuente = this.Fuente,
                NumCarga = this.NumCarga,
                FechaVaciado = this.FechaVaciado,
                FechaLevantado = this.FechaLevantado,
                FechaInicoPisado = this.FechaInicoPisado,
                FechaFinPisado = this.FechaFinPisado,
                NumPeyas = this.NumPeyas,
                Restante = this.Restante,
                CargaTotal = this.CargaTotal
            };
            return pp;
        }
    }
}