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
    [Authorize(Roles = "Administrador")]
    public class BarroMaestraController : Controller
    {
        TalaveraServices tsvc = new TalaveraServices();

        // GET: BarroMaestra
        public ActionResult Index()
        {
            List<BarroMaestra> lstBarro = tsvc.getBarroMestra();
            return View(lstBarro);
        }
                
        // GET: BarroMaestra/Create
        public ActionResult Create()
        {            
            return View();
        }

        // POST: BarroMaestra/Create
        [HttpPost]
        public ActionResult Create(BarroMaestra pBarro)
        {
            if (ModelState.IsValid)
            {
                pBarro.CodigoProducto = pBarro.Tipo.Substring(0, 1) + pBarro.Capacidad;
                BarroMaestra ExiteBarro = tsvc.validaExistenciaBarroMaestra(pBarro);
                if(ExiteBarro == null)
                {
                    pBarro.Editor = User.Identity.Name;
                    pBarro.FechaEdicion = DateTime.Now;
                    int res = tsvc.addBarroMaestra(pBarro);
                    if (res == 1)
                    {
                        return RedirectToAction("Index");
                    }
                }                              
            }
            return View();
        }
                
        // GET: BarroMaestra/Delete/5
        public ActionResult Delete(int id)
        {
            BarroMaestra elem = tsvc.getBarroMaestra(id);
            return View(elem);
        }

        // POST: BarroMaestra/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, BarroMaestra barro)
        {
            
            int res = tsvc.deleteBarroMaestra(id);
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
