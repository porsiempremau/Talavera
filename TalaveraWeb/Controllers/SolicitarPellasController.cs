using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalaveraWeb.Services;
using TalaveraWeb.Models;
using TalaveraWeb.Models.MiBD;

namespace TalaveraWeb.Controllers
{
    public class SolicitarPellasController : Controller
    {
        TalaveraServices tsvc = new TalaveraServices();
        // GET: SolicitarPellas
        public ActionResult Solicitar(int pLoc)
        {
            HttpContext.Application["Locacion"] = pLoc;
            ViewBag.Loc = pLoc;
            ViewBag.NombreLocActual = tsvc.getNombreSucursal(pLoc);
            ViewBag.lstLocaciones = tsvc.obtenerSucursalesExcepto(pLoc);

            int IdOtraSucursal = pLoc == 1 ? 2:1;
            ViewBag.lstRecervasPellas = tsvc.getReservasPellasFrom(IdOtraSucursal);

            EntregaPellas EP = new EntregaPellas();
            EP.FechaMovimiento = DateTime.Today;
            return View(EP);
        }

        [HttpPost]
        public ActionResult Solicitar(EntregaPellas pEnPe)
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];
            string NombreLocActual = tsvc.getNombreSucursal(ViewBag.Loc);
            int? tmpResponsable = int.Parse(pEnPe.Responsable); //Id de la sucursal a la que se le pidio los recursos.
            if (ModelState.IsValid)
            {
                pEnPe.Responsable = NombreLocActual;
                pEnPe.TipoMovimiento = "I";
                pEnPe.Editor = User.Identity.Name;
                pEnPe.FechaEdicion = DateTime.Now;
                pEnPe.Locacion = ViewBag.Loc;

                int res = tsvc.addEntregaPellas(pEnPe);                
                if (res >= 1)
                {                    
                    EntregaPellas EgresoEnPe = new EntregaPellas()
                    {
                        FechaMovimiento = DateTime.Now,
                        Responsable = NombreLocActual,
                        TipoMovimiento = "E",
                        CantidadPellas = pEnPe.CantidadPellas,
                        NumCarga = pEnPe.NumCarga,
                        Editor = User.Identity.Name,
                        FechaEdicion = DateTime.Now,
                        Locacion = tmpResponsable
                    };
                    int res2 = tsvc.addEntregaPellas(EgresoEnPe);

                    if(ViewBag.Loc == 1)
                        return RedirectToAction("Index", "LaLuz");
                    else
                        return RedirectToAction("Index", "TreintaYcuatroPte");
                }
            }
            return new HttpStatusCodeResult(404, "No se pudo registrar la solicitud de pellas");
        }

        //Entrega de pella a trabajadores para crear jahuetes.
        public ActionResult Entregar(int pLoc)
        {
            HttpContext.Application["Locacion"] = pLoc;
            ViewBag.Loc = pLoc;
            ViewBag.NombreLocActual = tsvc.getNombreSucursal(pLoc);

            ViewBag.lstRecervasPellas = tsvc.getReservasPellasFrom(pLoc);

            EntregaPellas EP = new EntregaPellas();
            EP.FechaMovimiento = DateTime.Today;

            return View(EP);
        }

        [HttpPost]
        public ActionResult Entregar(EntregaPellas pEnPe)
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];
                        
            if (ModelState.IsValid)
            {                
                pEnPe.TipoMovimiento = "E";
                pEnPe.Editor = User.Identity.Name;
                pEnPe.FechaEdicion = DateTime.Now;
                pEnPe.Locacion = ViewBag.Loc;

                int res = tsvc.addEntregaPellas(pEnPe);
                if (res >= 1)
                {
                    if (ViewBag.Loc == 1)
                        return RedirectToAction("Index", "LaLuz");
                    else
                        return RedirectToAction("Index", "TreintaYcuatroPte");
                }
            }
            return new HttpStatusCodeResult(404, "No se pudo registrar la solicitud de pellas");
        }

        public ActionResult ReporteEntrega(int pLoc)
        {
            ViewBag.Loc = pLoc;
            ViewBag.lstEntrega = tsvc.listEngregaPellas(pLoc);
            return View();
        }
    }
}