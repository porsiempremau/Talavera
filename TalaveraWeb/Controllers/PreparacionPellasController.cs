using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalaveraWeb.Models;
using TalaveraWeb.Services;

namespace TalaveraWeb.Controllers
{
    public class PreparacionPellasController : Controller
    {
        TalaveraServices tsvc = new TalaveraServices();
        public ActionResult Index()
        {
            List<PreparacionPellas> lstPrePell = tsvc.obtenerPreparacionPellas();
            return View(lstPrePell);
        }

        public ActionResult CrearPellas()
        {
            PreparacionPellas PrePell = new PreparacionPellas();
            PrePell.NumCarga = tsvc.getNumeroCarga();
            //PrePell.lstBarroPellas = tsvc.getPreparadosDeCarga(PrePell.NumCarga);
            ViewBag.lstPreparadosDisponibles = tsvc.getPreparadosDisponibles();
            return View(PrePell);
        }

        [HttpPost]
        public ActionResult CrearPellas(PreparacionPellas pPrePell)
        {
            if(ModelState.IsValid)
            {
                int res = tsvc.addPreparacionPellas(pPrePell);
                if (res == 1)
                {
                    return RedirectToAction("Index");
                }
            }                       
            return View(); 
        }
        
        public ActionResult EditarPellas(int id)
        {
            PreparacionPellas PrePell = tsvc.detallePreparacionPellas(id);               
            return View(PrePell);
        }

        [HttpPost]
        public ActionResult EditarPellas(PreparacionPellas pPrePell)
        {
            int res = tsvc.editPreparacionPellas(pPrePell);
            if (res == 1)
            {
                return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(404, "No se pudo actualizar el reporte, vuelva a intentarlo mas tarde");
        }

        public ActionResult BorrarPellas(int id)
        {
            PreparacionPellas PrePell = tsvc.detallePreparacionPellas(id);
            return View(PrePell);
        }

        [HttpPost]
        public ActionResult BorrarPellas(int id, PreparacionPellas PrePell)
        {
            int res = tsvc.deletePreparacionPellas(id);
            if(res == 1)
            {
                return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(404, "No se pudo borrar el reporte, vuelva a interntar mas tarde.");
        }
    }
}