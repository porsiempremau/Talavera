using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalaveraWeb.Models;
using TalaveraWeb.Services;

namespace TalaveraWeb.Controllers
{
    public class PreparacionBarroController : Controller
    {
        TalaveraServices tsvc = new TalaveraServices();
        // GET: PreparacionBarro
        public ActionResult Index()
        {
            List<PreparacionBarro> lstPreBar = tsvc.obtenerPreparacionBarro();
            return View(lstPreBar);
        }

        // GET: PreparacionBarro/Details/5
        public ActionResult Details(int id)
        {
            PreparacionBarro PreBar = tsvc.detallePreparacionBarro(id);
            return View(PreBar);
        }

        // GET: PreparacionBarro/Create
        public ActionResult Create()
        {
            PreparacionBarro PreBar = new PreparacionBarro();
            PreBar.FechaPreparacion = DateTime.Today;
            PreBar.NumPreparado = tsvc.getNumeroPreparado();
            PreBar.BarroBlanco = 105;
            PreBar.BarroNegro = 150;
            return View(PreBar);
        }

        // POST: PreparacionBarro/Create
        [HttpPost]
        public ActionResult Create(PreparacionBarro pPreBar)
        {
            if (ModelState.IsValid)
            {
                int res = tsvc.addPreparacionBarro(pPreBar);
                if (res == 1)
                {
                    return RedirectToAction("Index");
                }                
            }
            return View();
        }

        // GET: PreparacionBarro/Edit/5
        public ActionResult Edit(int id)
        {
            PreparacionBarro PreBar = tsvc.detallePreparacionBarro(id);
            return View(PreBar);
        }

        // POST: PreparacionBarro/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, PreparacionBarro pPreBar)
        {
            int res = tsvc.editPreparacionBarro(id, pPreBar);
            if(res > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        // GET: PreparacionBarro/Delete/5
        public ActionResult Delete(int id)
        {
            PreparacionBarro PreBar = tsvc.detallePreparacionBarro(id);
            return View(PreBar);
        }

        // POST: PreparacionBarro/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            int res = tsvc.deletePreparacionBarro(id);
            if(res > 0)
            {                
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
    }
}
