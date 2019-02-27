using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalaveraWeb.Models.MiBD;
using TalaveraWeb.Services;

namespace TalaveraWeb.Controllers
{
    public class PersonalTalaveraController : Controller
    {
        TalaveraServices tsvc = new TalaveraServices();
        // GET: PersonalTalavera
        public ActionResult Index()
        {
            List<PersonalTalavera> lst = tsvc.getPersonalTalavera();
            return View(lst);
        }

        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Crear(PersonalTalavera PT)
        {
            if (ModelState.IsValid)
            {
                int res = tsvc.addPersonalTalaver(PT);
                if (res >= 1)
                { 
                    return RedirectToAction("Index");
                }
            }
            return new HttpStatusCodeResult(404, "No se pudo registrar, vuelva a intentarlo mas tarde");
        }

        public ActionResult Edit(int id)
        {
            PersonalTalavera pt = tsvc.getPersonal(id);
            return View(pt);
        }

        [HttpPost]
        public ActionResult Edit(PersonalTalavera PT)
        {
            if (ModelState.IsValid)
            {
                int res = tsvc.editPersonal(PT);
                if (res > 0)
                    return RedirectToAction("Index");
            }
            return new HttpStatusCodeResult(201, "No fue posible guardar los cambios, intentelo mas tarde");
        }

        public ActionResult Delete(int id)
        {
            PersonalTalavera CT = tsvc.getPersonal(id);
            return View(CT);
        }
                
        [HttpPost]
        public ActionResult Delete(PersonalTalavera CT)
        {
            int res = tsvc.deletePersonal(CT.Id);
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