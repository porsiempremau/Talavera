using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalaveraWeb.Models;
using TalaveraWeb.Models.MiBD;
using TalaveraWeb.Services;

namespace TalaveraWeb.Controllers
{
    [Authorize(Roles = "Administrador, Usuario")]
    public class PreparacionPellasController : Controller
    {
        TalaveraServices tsvc = new TalaveraServices();       
        public ActionResult Index(int pLoc)
        {
            HttpContext.Application["Locacion"] = pLoc;
            ViewBag.NombreLocacion = tsvc.getNombreSucursal(pLoc);
            List<PreparacionPellas> lstPrePell = tsvc.obtenerPreparacionPellas(pLoc);
            return View(lstPrePell);
        }

        public ActionResult CrearPellas()
        {
            ViewBag.Loc = (int) HttpContext.Application["Locacion"];
            PreparacionPellasConPreBar PrePell = new PreparacionPellasConPreBar();
            PrePell.NumCarga = tsvc.getNumeroCarga();
            PrePell.Fuente = 1;

            List<SelectListItem> lstBarroDisponible = new List<SelectListItem>();
            var lstTmp = tsvc.getPreparadosDisponibles(ViewBag.Loc);
            foreach (var item in lstTmp)
                lstBarroDisponible.Add(new SelectListItem() { Value = item, Text = item });
            ViewBag.lstPreparadosDisponibles = lstBarroDisponible;

            return View(PrePell);
        }

        [HttpPost]
        public ActionResult CrearPellas(PreparacionPellasConPreBar pPrePell)
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];
            if (ModelState.IsValid)
            {
                PreparacionPellas pp = pPrePell.getPreparadoPella();
                pp.Editor = User.Identity.Name;
                pp.FechaEdicion = DateTime.Now;
                pp.Locacion = ViewBag.Loc;
                int res = tsvc.addPreparacionPellas(pp);
                if (res >= 1)
                {
                    int res2 = tsvc.addPreparacionPellasRelacionesPreparacionBarro( pPrePell.lstPreBar, pPrePell.NumCarga, User.Identity.Name );
                    int res3 = tsvc.editaEstadoPrepBarroPorPreparacionPellas(pPrePell.lstPreBar, "Consumido", User.Identity.Name);

                    int Loc = (int)HttpContext.Application["Locacion"];
                    return RedirectToAction("Index",new { pLoc = Loc });
                }
            }                       
            return new HttpStatusCodeResult(404, "No se pudo registrar la carga, vuelva a intentarlo mas tarde");
        }
        
        public ActionResult EditarPellas(int id)
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];
            PreparacionPellas PrePell = tsvc.detallePreparacionPellas(id);
            PreparacionPellasConPreBar PPCB = new PreparacionPellasConPreBar(PrePell);
            return View(PPCB);
        }

        [HttpPost]
        public ActionResult EditarPellas(PreparacionPellasConPreBar pPPCB)
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];
            PreparacionPellas PrePellEnBD = tsvc.detallePreparacionPellas(pPPCB.Id);
            //Verifico si el numero de pellas es distinto de null, en caso afirmativo.
            //es un indicio que se regitraron pellas nuevas
            if(pPPCB.NumPeyas != null)
            {
                //en este caso, significa que son nuevas pellas.
                if (PrePellEnBD.NumPeyas == null)
                {
                    tsvc.addEntregaPellas(pPPCB.NumPeyas, pPPCB.NumCarga, User.Identity.Name, ViewBag.Loc, "I");
                }//Si el numero de pellas es diferente al que hay en BD, implica que se edito el numero de pellas                
                else if(pPPCB.NumPeyas != PrePellEnBD.NumPeyas)
                {
                    tsvc.editEntregaPellas(pPPCB.NumPeyas, pPPCB.NumCarga, User.Identity.Name, ViewBag.Loc);
                }                
            }

            PreparacionPellas pp = pPPCB.getPreparadoPella();
            pp.Editor = User.Identity.Name;
            pp.FechaEdicion = DateTime.Now;
            int res = tsvc.editPreparacionPellas(pp);
            if (res == 1)
            {
                int Loc = (int)HttpContext.Application["Locacion"];
                return RedirectToAction("Index", new { pLoc = Loc });
            }
            return new HttpStatusCodeResult(404, "No se pudo actualizar el reporte, vuelva a intentarlo mas tarde");
        }

        public ActionResult BorrarPellas(int id)
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];
            PreparacionPellas PrePell = tsvc.detallePreparacionPellas(id);
            PreparacionPellasConPreBar PPCB = new PreparacionPellasConPreBar(PrePell);
            return View(PPCB);
        }

        [HttpPost]
        public ActionResult BorrarPellas(int id, PreparacionPellasConPreBar pPrePell)
        {            
            int res = tsvc.deletePreparacionPellas(id);
            if(res >= 1)
            {
                int res0 = tsvc.deleteEntregaPellas(pPrePell.NumCarga);
                int res2 = tsvc.deletePreparacionPellasRelacionesPreparacionBarro(pPrePell.NumCarga);
                int res3 = tsvc.editaEstadoPrepBarroPorPreparacionPellas(pPrePell.lstPreBar, "Disponible", User.Identity.Name);
                
                int Loc = (int)HttpContext.Application["Locacion"];
                return RedirectToAction("Index", new { pLoc = Loc });
            }
            return new HttpStatusCodeResult(404, "No se pudo borrar el reporte, vuelva a interntar mas tarde.");
        }
    }
}