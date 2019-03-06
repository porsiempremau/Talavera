using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalaveraWeb.Models;
using TalaveraWeb.Models.clasesPlanDeTrabajo;
using TalaveraWeb.Models.MiBD;
using TalaveraWeb.Services;

namespace TalaveraWeb.Controllers
{
    public class PlanDeTrabajoController : Controller
    {
        TalaveraServices tsvc = new TalaveraServices();
        // GET: PlanDeTrabajo
        public ActionResult Index(int pLoc)
        {
            HttpContext.Application["Locacion"] = pLoc;
            List<PlanDeTrabajoConDetalle> lst = tsvc.getPlanesTrabajo();
            
            return View(lst);
        }

        [HttpPost]
        public PartialViewResult agregarElemento(int indice)
        {
            return PartialView("_DetallePlan", indice);
        }

        public ActionResult Create()
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];
            ViewBag.NombreLocacion = tsvc.getNombreSucursal(ViewBag.Loc);
                        
            List<SelectListItem> lstPersonal = new List<SelectListItem>();
            var lstTmp = tsvc.getPersonalTalavera();
            foreach (var item in lstTmp)
                lstPersonal.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Nombre + " " + item.APaterno });
            ViewBag.Personal = lstPersonal;

            ViewBag.EtapasProduccion = tsvc.getEtapasDeProduccion();

            ViewBag.lstCT = tsvc.getPiezasTalavera();
            ViewBag.PiezaActual = "";

            return View();
        }

        [HttpPost]
        public ActionResult Create(PlanDeTrabajoConDetalle pPTCD)
        {
            ViewBag.Loc = (int)HttpContext.Application["Locacion"];
            //if (ModelState.IsValid)
            //{
            PlanDeTrabajo pt = new PlanDeTrabajo()
                {
                    IdPersonal = pPTCD.IdPersonal,
                    NumeroOrden = pPTCD.NumeroOrden,
                    FechaInicio = pPTCD.FechaInicio,
                    FechaFin = pPTCD.FechaFin,
                    EtapaPlan = pPTCD.EtapaPlan,
                    Observacion = pPTCD.Observacion
                };
            int res = tsvc.addPlanDeTrabajo(pt);
            if (res > 0)
            {
                List<MoldeadoMovimientos> lst = new List<MoldeadoMovimientos>();
                foreach(var item in pPTCD.Detalles)
                {
                    if(item.IdCatalogoTalavera > 0 && item.CatidadPlaneada > 0)
                    {
                        MoldeadoMovimientos mm = new MoldeadoMovimientos()
                        {
                            IdOrigen = res,
                            TablaOrigen = "PlanDeTrabajo",
                            IdCatalogoTalavera = item.IdCatalogoTalavera,
                            CantidadReal = item.CantidadReal,
                            CatidadPlaneada = item.CatidadPlaneada,
                            Observacion = item.Observacion,
                            TipoMovimiento = "In"
                        };
                        lst.Add(mm);
                    }                        
                }
                int res2 = tsvc.addMoldeadoMovimientos(lst);
                if(res2 > 0)
                    return RedirectToAction("Index", new {pLoc = ViewBag.Loc});
            }                    
            //}
            return new HttpStatusCodeResult(201, "Error al tratar de guardar el plan de trabajo.");            
        }

        [HttpPost]
        public PartialViewResult PiesasFiltradas(string inBuscar)
        {
            List<CatalogoTalavera> lstCT = tsvc.getPiezasTalavera(inBuscar);
            return PartialView("_listadoPiezasTalavera", lstCT);
        }
    }
}