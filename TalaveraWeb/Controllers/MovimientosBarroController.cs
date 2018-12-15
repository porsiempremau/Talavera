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
            ViewBag.lst34Pte = tsvc.getReservasFrom(2);
            ViewBag.lstLuz = tsvc.getReservasFrom(1);
            return View();
        }

        // GET: MovimientosBarro/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        

        public ActionResult SolicitarReserva()
        {            
            ViewBag.lst34pte = tsvc.getReservasFrom(2);

            List<SelectListItem> lstTmp = new List<SelectListItem>();
            foreach(var iten in ViewBag.lst34pte)
            {
                lstTmp.Add(new SelectListItem() { Text = iten.Tipo + " " + iten.Capacidad, Value = iten.Tipo.Substring(0, 1) + iten.Capacidad });                
            }

            SolicitarReserva SolRec = new SolicitarReserva();
            //SolRec.lstTipoCapacidad = lstTmp;
            ViewBag.lstTipoCapacidad = lstTmp;
            SolRec.Unidades = 0;
            SolRec.TotalKg = 0;
            
            return View(SolRec);
        }

        [HttpPost]
        public ActionResult SolicitarReserva(SolicitarReserva pSR)
        {
            if( ModelState.IsValid )
            {
                //SelectListItem item = pSR.lstTipoCapacidad.Where(x => x.Selected).FirstOrDefault();                
                BarroMovimientos tmpMovB_in = new BarroMovimientos()
                {
                    FechaMovimiento = DateTime.Today,
                    TipoMovimiento = "In",
                    CodigoProducto = pSR.CodigoBarro, //item.Value,
                    Unidades = (int)pSR.Unidades,
                    Editor = User.Identity.Name,
                    FechaEdicion = DateTime.Now
                ////Locacion = "La Luz",
                ////OrigenTranferencia = "34 pte"
            };

                BarroMovimientos tmpMovB_eg = new BarroMovimientos()
                {
                    FechaMovimiento = DateTime.Today,
                    TipoMovimiento = "Eg",
                    CodigoProducto = pSR.CodigoBarro, //item.Value,
                    Editor = User.Identity.Name,
                    FechaEdicion = DateTime.Now
                //Unidades = pSR.Unidades,
                //Locacion = "34 pte"
            };

                List<BarroMovimientos> lst = new List<BarroMovimientos>();
                lst.Add(tmpMovB_in);
                lst.Add(tmpMovB_eg);

                int res = tsvc.addMovimientosBarro(lst);
                if (res == 2)
                {
                    return RedirectToAction("Index");
                }
            }
            //return HttpStatusCodeResult(201, "No se pudo realizar la consulta");
            return View( new HttpStatusCodeResult(201, "No se pudo realizar la consulta") );
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
