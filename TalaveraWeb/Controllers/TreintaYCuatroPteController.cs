using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalaveraWeb.Models;
using TalaveraWeb.Services;

namespace TalaveraWeb.Controllers
{
    public class TreintaYCuatroPteController : Controller
    {
        TalaveraServices tsvc = new TalaveraServices();
        // GET: TreintaYCuatroPte
        public ActionResult Index()
        {
            ViewBag.lstBarroGranel34pte = tsvc.getReservasBarroGranelFrom(2);
            ViewBag.lstBarroEmpaque34pte = tsvc.getReservasBarroEmpaqueFrom(2);
            return View();
        }

        public ActionResult SolicitarBarroGranel()
        {
            ViewBag.Productos = tsvc.obtenerProductos(1);
            ViewBag.Provedores = tsvc.obtenerProvedores();
            ViewBag.Locaciones = tsvc.obtenerSucursales(2);
            BarroMovimientos bm = new BarroMovimientos();
            return View(bm);
        }

        // POST: MovimientosBarro/Create
        [HttpPost]
        public ActionResult SolicitarBarroGranel(BarroMovimientos pMovB)
        {
            if (ModelState.IsValid)
            {
                pMovB.TipoMovimiento = "In";
                pMovB.PesoTotal = pMovB.Unidades;
                pMovB.OrigenTabla = "Provedores";

                int res = tsvc.addMovimientosBarro(pMovB);
                if (res == 1)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(new HttpStatusCodeResult(202, "No pudo ser generada la solicitud.") );
        }



    }
}