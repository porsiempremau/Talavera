using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TalaveraWeb.Models;
using TalaveraWeb.Models.MiBD;
using TalaveraWeb.Services;

namespace TalaveraWeb.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ProvedoresController : Controller
    {
        TalaveraServices tsvc = new TalaveraServices();
        // GET: Provedores
        public ActionResult Index()
        {
            List<Provedores> lstProv = tsvc.getProvedores();
            if (lstProv != null)
            {
                ViewBag.sinDatos = 0;
            }                
            else
            {
                ViewBag.sinDatos = 1;
                lstProv = new List<Provedores>();
                lstProv.Add( new Provedores() {Nombre = "" } );                
            }
            return View(lstProv);
        }

        // GET: Provedores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provedores Prov = tsvc.getProvedor(id);
            if (Prov == null)
            {
                return HttpNotFound();
            }
            return View(Prov);
        }

        // GET: Provedores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Provedores/Create
        [HttpPost]
        public ActionResult Create(Provedores pProv)
        {
            try
            {
                int resultado = tsvc.newProvedores(pProv);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: Provedores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            
            Provedores Prov = tsvc.getProvedor(id);
            if(Prov == null)
            {
                return HttpNotFound();
            }
            return View(Prov);
        }

        // POST: Provedores/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Provedores pProv)
        {
            try
            {
                int res = tsvc.editProvedores(id, pProv);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error en Edicion:" + ex.Message);
                return View();
            }

        }

        // GET: Provedores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provedores Prov = tsvc.getProvedor(id);
            if (Prov == null)
            {
                return HttpNotFound();
            }
            return View(Prov);
        }

        // POST: Provedores/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Provedores pProv)
        {
            int res = tsvc.deleteProvedores(id, pProv);
            if(res == 1)
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
