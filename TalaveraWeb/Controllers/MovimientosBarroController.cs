using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalaveraWeb.Models;
using TalaveraWeb.Services;

namespace TalaveraWeb.Controllers
{
    public class MovimientosBarroController : Controller
    {
        TalaveraServices tsvc = new TalaveraServices();

        // GET: MovimientosBarro
        public ActionResult Index()
        {            
            ViewBag.lst34Pte = tsvc.getRecervasFrom("34 pte");
            ViewBag.lstLuz = tsvc.getRecervasFrom("La Luz");
            return View();
        }

        // GET: MovimientosBarro/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MovimientosBarro/Create
        public ActionResult Create()
        {
            ViewBag.Productos = tsvc.obtenerProductos();
            ViewBag.Provedores = tsvc.obtenerProvedores();
            ViewBag.Locaciones = tsvc.obtenerLocaciones();
            MovimientosBarro MovBar = new MovimientosBarro();
            return View(MovBar);
        }

        // POST: MovimientosBarro/Create
        [HttpPost]
        public ActionResult Create(MovimientosBarro pMovB)
        {
            if (ModelState.IsValid)
            {
                pMovB.TipoMovimiento = "In";
                int res = tsvc.addMovimientosBarro(pMovB);
                if (res == 1)
                {
                    return RedirectToAction("Index");
                }                
            }            
            return View();            
        }

        public ActionResult SolicitarRecerva()
        {
            List<RecervaBarro> lst34pte = tsvc.getRecervasFrom("34 pte");

            return View();
        }

        // GET: MovimientosBarro/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MovimientosBarro/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MovimientosBarro/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MovimientosBarro/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
