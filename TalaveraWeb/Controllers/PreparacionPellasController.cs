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
            ViewBag.Loc = pLoc;
            ViewBag.NombreLocacion = tsvc.getNombreSucursal(pLoc);
            List<PreparacionPellas> lstPrePell = tsvc.obtenerPreparacionPellas(pLoc);
            return View(lstPrePell);
        }
                
        public JsonResult ValidaRecursos()
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];
            var lstTmp = tsvc.getPreparadosDisponibles(ViewBag.Loc);
            return Json(lstTmp);
        }

        public ActionResult CrearPellas()
        {
            ViewBag.Loc = (int) HttpContext.Application["Locacion"];
            PreparacionPellasConPreBar PrePell = new PreparacionPellasConPreBar();
            PrePell.NumCarga = tsvc.getNumeroCarga(ViewBag.Loc);
            PrePell.Fuente = 1;

            List<SelectListItem> lstBarroDisponible = new List<SelectListItem>();
            var lstTmp = tsvc.getPreparadosDisponibles(ViewBag.Loc);
            foreach (var item in lstTmp)
                lstBarroDisponible.Add(new SelectListItem() { Value = item, Text = item });
            //ViewBag.lstPreparadosDisponibles = lstBarroDisponible;

            PrePell.lstPreparados = lstBarroDisponible;
                                    
            return View(PrePell);
        }
        
        [HttpPost]
        public ActionResult CrearPellas(PreparacionPellasConPreBar pPrePell)
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];
            ViewBag.msgAviso = "";
            if (ModelState.IsValid)
            {
                if(pPrePell.SelectedPreparados.Count() < 2)
                {
                    ViewBag.msgAviso = "Es necesario asignar más de una preparación a la carga";
                    List<SelectListItem> lstBarroDisponible = new List<SelectListItem>();
                    var lstTmp = tsvc.getPreparadosDisponibles(ViewBag.Loc);
                    foreach (var item in lstTmp)
                        lstBarroDisponible.Add(new SelectListItem() { Value = item, Text = item });
                    
                    pPrePell.lstPreparados = lstBarroDisponible;

                    return View(pPrePell);
                }
                else
                {
                    PreparacionPellas pp = pPrePell.getPreparadoPella();
                    pp.Editor = User.Identity.Name;
                    pp.FechaEdicion = DateTime.Now;
                    pp.Locacion = ViewBag.Loc;
                    int res = tsvc.addPreparacionPellas(pp);
                    if (res >= 1)
                    {
                        int res2 = tsvc.addPreparacionPellasRelacionesPreparacionBarro(pPrePell.SelectedPreparados.ToList(), pPrePell.NumCarga, User.Identity.Name);
                        int res3 = tsvc.editaEstadoPrepBarroPorPreparacionPellas(pPrePell.NumCarga, "Consumido", User.Identity.Name);

                        int Loc = (int)HttpContext.Application["Locacion"];
                        return RedirectToAction("Index", new { pLoc = Loc });
                    }
                }                
            }                       
            return new HttpStatusCodeResult(404, "No se pudo registrar la carga, vuelva a intentarlo mas tarde");
        }
        
        public ActionResult DetallePellas(int id)
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];
            ViewBag.NombreLocacion = tsvc.getNombreSucursal(ViewBag.Loc);
            PreparacionPellas PrePell = tsvc.detallePreparacionPellas(id);
            
            PreparacionPellasConPreBar PPCB = new PreparacionPellasConPreBar(PrePell);
            PPCB.SelectedPreparados = tsvc.preparacionesAsignadasACarga(PrePell.NumCarga);
            return View(PPCB);
        }

        public ActionResult EditarPellas(int id)
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];
            ViewBag.NombreLocacion = tsvc.getNombreSucursal(ViewBag.Loc);
            PreparacionPellas PrePell = tsvc.detallePreparacionPellas(id);

            PreparacionPellasConPreBar PPCB = new PreparacionPellasConPreBar(PrePell);
            PPCB.SelectedPreparados = tsvc.preparacionesAsignadasACarga(PrePell.NumCarga);

            List<SelectListItem> lstBarroDisponible = new List<SelectListItem>();
            var lstTmp = tsvc.getPreparadosDisponibles(ViewBag.Loc);
            foreach (var item in lstTmp)
                lstBarroDisponible.Add(new SelectListItem() { Value = item, Text = item, Selected = false });

            foreach (var item in PPCB.SelectedPreparados)            
                lstBarroDisponible.Add(new SelectListItem() { Value = item, Text = item, Selected = true });

            PPCB.lstPreparados = lstBarroDisponible;
            return View(PPCB);
        }

        [HttpPost]
        public ActionResult EditarPellas(PreparacionPellasConPreBar pPPCB)
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];

            if (pPPCB.SelectedPreparados.Count() < 2)
            {
                ViewBag.msgAviso = "Es necesario asignar más de una preparación a la carga";
                List<SelectListItem> lstBarroDisponible = new List<SelectListItem>();
                var lstTmp = tsvc.getPreparadosDisponibles(ViewBag.Loc);
                foreach (var item in lstTmp)
                    lstBarroDisponible.Add(new SelectListItem() { Value = item, Text = item });
                pPPCB.lstPreparados = lstBarroDisponible;

                foreach (var item in pPPCB.SelectedPreparados)
                    lstBarroDisponible.Add(new SelectListItem() { Value = item, Text = item, Selected = true });

                return View(pPPCB);
            }
            else
            {
                PreparacionPellas PrePellEnBD = tsvc.detallePreparacionPellas(pPPCB.Id);
                //Verifico si el numero de pellas es distinto de null, en caso afirmativo.
                //es un indicio que se regitraron pellas nuevas
                if (pPPCB.NumPeyas != null)
                {
                    //en este caso, significa que son nuevas pellas.
                    if (PrePellEnBD.NumPeyas == null)
                    {
                        EntregaPellas EP = new EntregaPellas() {
                            FechaMovimiento = DateTime.Today,
                            Responsable = "PreparacionPellas",
                            TipoMovimiento = "I",
                            CantidadPellas = pPPCB.NumPeyas,
                            NumCarga = pPPCB.NumCarga,
                            Editor = User.Identity.Name,
                            FechaEdicion = DateTime.Now,
                            Locacion = ViewBag.Loc
                        };
                        tsvc.addEntregaPellas(EP);
                    }//Si el numero de pellas es diferente al que hay en BD, implica que se edito el numero de pellas                
                    else if (pPPCB.NumPeyas != PrePellEnBD.NumPeyas)
                    {
                        EntregaPellas EP = new EntregaPellas()
                        {
                            FechaMovimiento = DateTime.Today,
                            Responsable = "PreparacionPellas",
                            TipoMovimiento = "I",
                            CantidadPellas = pPPCB.NumPeyas,
                            NumCarga = pPPCB.NumCarga,
                            Editor = User.Identity.Name,
                            FechaEdicion = DateTime.Now,
                            Locacion = ViewBag.Loc
                        };
                        tsvc.editEntregaPellas(EP);
                    }
                }

                //Borrado de elementos relacionados a la carga.
                int res2 = tsvc.editaEstadoPrepBarroPorPreparacionPellas(pPPCB.NumCarga, "Disponible", User.Identity.Name);
                int res3 = tsvc.deletePreparacionPellasRelacionesPreparacionBarro(pPPCB.NumCarga);

                //Alta de elementos relacionados a carga editada.        
                int res4 = tsvc.addPreparacionPellasRelacionesPreparacionBarro(pPPCB.SelectedPreparados.ToList(), pPPCB.NumCarga, User.Identity.Name);
                int res5 = tsvc.editaEstadoPrepBarroPorPreparacionPellas(pPPCB.NumCarga, "Consumido", User.Identity.Name);

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
            string NumCarga = tsvc.detallePreparacionPellas(id).NumCarga;
            int res = tsvc.deletePreparacionPellas(id);
            if(res >= 1)
            {
                int res0 = tsvc.deleteEntregaPellas(NumCarga);
                int res3 = tsvc.editaEstadoPrepBarroPorPreparacionPellas(NumCarga, "Disponible", User.Identity.Name);
                int res2 = tsvc.deletePreparacionPellasRelacionesPreparacionBarro(NumCarga);
                
                int Loc = (int)HttpContext.Application["Locacion"];
                return RedirectToAction("Index", new { pLoc = Loc });
            }
            return new HttpStatusCodeResult(404, "No se pudo borrar el reporte, vuelva a interntar mas tarde.");
        }
    }
}