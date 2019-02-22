using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalaveraWeb.Models.MiBD;
using TalaveraWeb.Services;

namespace TalaveraWeb.Controllers
{
    public class CatalogoTalaveraController : Controller
    {
        TalaveraServices tvsv = new TalaveraServices();
        // GET: CatalogoTalavera
        public ActionResult Index()
        {
            List<CatalogoTalavera> lstCT = tvsv.getPiezasTalavera();
            return View(lstCT);
        }

        public ActionResult NuevaPieza()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NuevaPieza(CatalogoTalavera pCT)
        {
            if (ModelState.IsValid)
            {
                int res = tvsv.addPiezaTalavera(pCT);
                if(res > 0)
                    return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            CatalogoTalavera CT = tvsv.getPiezasTalavera(id);
            return View(CT);
        }

        [HttpPost]
        public ActionResult Edit(CatalogoTalavera pCT)
        {
            if (ModelState.IsValid)
            {
                int res = tvsv.editCatalogo(pCT);
                if(res > 0)
                    return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(201, "No fue posible guardar los cambios, intentelo mas tarde");
        }

        public ActionResult Delete(int id)
        {
            CatalogoTalavera CT = tvsv.getPiezasTalavera(id);
            return View(CT);
        }

        // POST: Provedores/Delete/5
        [HttpPost]
        public ActionResult Delete(CatalogoTalavera CT)
        {
            int res = tvsv.deleteCatalogo(CT.Id);
            if (res == 1)
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