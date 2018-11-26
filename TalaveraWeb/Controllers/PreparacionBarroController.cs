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
        public ActionResult Index(int pLoc = 1)
        {
            List<PreparacionBarro> lstPreBar = tsvc.obtenerPreparacionBarro(pLoc);
            ViewBag.Locacion = pLoc;
            return View(lstPreBar);
        }

        // GET: PreparacionBarro/Details/5
        public ActionResult Details(int id)
        {
            PreparacionBarro PreBar = tsvc.detallePreparacionBarro(id);            
            return View(PreBar);
        }

        // GET: PreparacionBarro/Create
        public ActionResult Create(int pLocacion)
        {
            List<ReservaBarro> lstReservas = tsvc.getReservasFrom(pLocacion);
            ViewBag.lstLuz = lstReservas;            
            ViewBag.lstLocaciones = tsvc.obtenerSucursales();
            
            PreparacionBarroConsumo PreBar = new PreparacionBarroConsumo();
            PreBar.FechaPreparacion = DateTime.Today;
            PreBar.NumPreparado = tsvc.getNumeroPreparado();
            PreBar.BarroBlanco = 105;
            PreBar.BarroNegro = 150;
            PreBar.Locacion = pLocacion;
            PreBar.lstConsumoBarroNegro = lstReservas.Where(x => x.Tipo == "Negro")
                                    .Select(y => new ReservaBarroPreparado()
                                    {
                                        CodigoBarro = y.CodigoBarro,
                                        Tipo = y.Tipo,
                                        Capacidad = y.Capacidad,
                                        Unidades = y.Unidades,
                                        TotalKg = y.TotalKg
                                    }).ToList();

            PreBar.lstConsumoBarroBlanco = lstReservas.Where(x => x.Tipo == "Blanco")
                                    .Select(y => new ReservaBarroPreparado()
                                    {
                                        CodigoBarro = y.CodigoBarro,
                                        Tipo = y.Tipo,
                                        Capacidad = y.Capacidad,
                                        Unidades = y.Unidades,
                                        TotalKg = y.TotalKg
                                    }).ToList();

            return View(PreBar);
        }

        // POST: PreparacionBarro/Create
        [HttpPost]
        public ActionResult Create(PreparacionBarroConsumo pPreBar)
        {
            if (ModelState.IsValid)
            {                
                int? tmpBarNeg = pPreBar.BarroNegro;
                int? SumBN = pPreBar.lstConsumoBarroNegro.Select(x => new { TotalSolicitado = x.Capacidad * x.BarroUsado }).Sum(x => x.TotalSolicitado);
                int? SumBB = pPreBar.lstConsumoBarroBlanco.Select(x => new { TotalSolicitado = x.Capacidad * x.BarroUsado }).Sum(x => x.TotalSolicitado);

                List<BarroMovimientos> lst = new List<BarroMovimientos>();
                pPreBar.Estado = "Disponible";
                PreparacionBarro PB = new PreparacionBarro()
                {
                    Id = pPreBar.Id,
                    FechaPreparacion = pPreBar.FechaPreparacion,
                    NumPreparado = pPreBar.NumPreparado,
                    BarroNegro = pPreBar.BarroNegro,
                    BarroBlanco = pPreBar.BarroBlanco,
                    Recuperado = pPreBar.Recuperado,
                    EnPiedra = pPreBar.EnPiedra,
                    TiempoAgitacion = pPreBar.TiempoAgitacion,
                    NumTambos = pPreBar.NumTambos,
                    DesperdicioMojado = pPreBar.DesperdicioMojado,
                    Comentario = pPreBar.Comentario,
                    Estado = pPreBar.Estado,
                    Locacion = pPreBar.Locacion
                };
                
                //Si las recercas asignadas para cubrir el barro Negro solicitado son mayores, se cubre el pedido, en caso contrario se solicita asignar mas recervas.
                if (pPreBar.BarroNegro <= SumBN)
                {
                    //Si las recercas asignadas para cubrir el barro Blanco solicitado son mayores, se cubre el pedido, en caso contrario se solicita asignar mas recervas.
                    if (pPreBar.BarroBlanco <= SumBB)
                    {                        
                        int idPreparacion = tsvc.addPreparacionBarro(PB);
                        if (idPreparacion != -1)
                        {
                            var lstBarMovNegro = calculaBarroMovimientos(pPreBar.lstConsumoBarroNegro, (int)pPreBar.BarroNegro, PB, idPreparacion);
                            lst.AddRange(lstBarMovNegro);

                            var lstBarMovBlanco = calculaBarroMovimientos(pPreBar.lstConsumoBarroBlanco, (int)pPreBar.BarroBlanco, PB, idPreparacion);
                            lst.AddRange(lstBarMovBlanco);

                            //Por ultimo se registran los movimientos en BD.
                            int res2 = tsvc.addMovimientosBarro(lst);
                            if (res2 > 0)
                            {
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            ViewBag.txtMensaje = "No se pudo crear la preparación, vuelva a intentarlo más tarde.";
                        }                        
                    }
                    else //Como las recervas son menores al barro solicitado, se pide una cantidad mayor de recervas.
                    {
                        ViewBag.txtMensaje = "Es necesario que asignes reservas de barro blanco mayores que el barro blanco solicitado para la preparación.";
                    }
                }
                else //Como las recervas son menores al barro solicitado, se pide una cantidad mayor de recervas.
                {
                    ViewBag.txtMensaje = "Es necesario que asignes reservas de barro mayores que el barro negro solicitado para la preparación.";                    
                }                
            }

            List<ReservaBarro> lstReservas = tsvc.getReservasFrom((int)pPreBar.Locacion);
            ViewBag.lstLuz = lstReservas;
            ViewBag.lstLocaciones = tsvc.obtenerSucursales();
            return View(pPreBar);
        }

        public List<BarroMovimientos> calculaBarroMovimientos(List<ReservaBarroPreparado> plstBarroReservaAsignado, int pBarroSolicitado, PreparacionBarro pPreBar, int idPreparacion)
        {
            List<BarroMovimientos> lst = new List<BarroMovimientos>();
            int? SumBN = plstBarroReservaAsignado.Select(x => new { TotalSolicitado = x.Capacidad * x.BarroUsado }).Sum(x => x.TotalSolicitado);

            //foreach (var item in pPreBar.lstConsumoBarroNegro.OrderBy(x => x.CodigoBarro))
            for (int a = 0; a < plstBarroReservaAsignado.Count; a++)
            {
                var item = plstBarroReservaAsignado[a];
                if (item.BarroUsado > 0)
                {
                    //En caso de ser a granel, se hace una resta directa
                    if (item.Capacidad == 1)
                    {
                        int TotalKgUsados = item.BarroUsado * (int)item.Capacidad;
                        pBarroSolicitado = pBarroSolicitado - TotalKgUsados;

                        //Se hace el egreso de barro a granel.
                        //Nota: en el barro a granel "NO" es nesario hacer un ingreso con el sobrante del barro asignado.
                        BarroMovimientos tmpMovB_egNegro = new BarroMovimientos()
                        {
                            CodigoProducto = item.CodigoBarro,
                            FechaMovimiento = DateTime.Today,
                            TipoMovimiento = "Eg",
                            Unidades = item.BarroUsado,
                            Locacion = pPreBar.Locacion,
                            OrigenTransferencia = idPreparacion,
                            OrigenTabla = "PreparacionBarro",
                            PesoTotal = TotalKgUsados
                        };
                        lst.Add(tmpMovB_egNegro);
                    }
                    else  //Para los paquete se va haciendo gradual la resta, para determinar cuantos paquetes son necesrios.
                    {
                        int empaquetadosNecesarios = 0;
                        int TotalKgUsados = 0;
                        //Se ira sumando paquete a paquete hasta cubrir el barro solicitado o agotar las recervas asignadas(BarroUsado).
                        for (int i = 1; i <= item.BarroUsado; i++)
                        {
                            empaquetadosNecesarios = i;
                            pBarroSolicitado = pBarroSolicitado - (int)item.Capacidad;
                            if (pBarroSolicitado <= 0)
                                break;
                        }
                        TotalKgUsados = empaquetadosNecesarios * (int)item.Capacidad;

                        //Se genera el egreso de estas empaquetados.
                        BarroMovimientos tmpMovB_egNegro = new BarroMovimientos()
                        {
                            CodigoProducto = item.CodigoBarro,
                            FechaMovimiento = DateTime.Today,
                            TipoMovimiento = "Eg",
                            Unidades = empaquetadosNecesarios,
                            Locacion = pPreBar.Locacion,
                            OrigenTransferencia = idPreparacion,
                            OrigenTabla = "PreparacionBarro",
                            PesoTotal = TotalKgUsados
                        };
                        lst.Add(tmpMovB_egNegro);

                        //Cuando tmpBarNegro es negativo y dado que estamos en un segmento validador de paquetes
                        //se asume que el restante es debido a que un paquete tubo que ser habierto para cubrir el barro solicitado
                        //por esta razon, el restante sera un ingreso a granel en barromovimientos para no peder el restante del paquete habierto.
                        if (pBarroSolicitado < 0)
                        {
                            BarroMovimientos tmpMovB_inNegro = new BarroMovimientos()
                            {
                                CodigoProducto = tsvc.obtenerCodigoDeProducto(item.Tipo, 1),
                                FechaMovimiento = DateTime.Today,
                                TipoMovimiento = "In",
                                Unidades = pBarroSolicitado * -1,
                                Locacion = pPreBar.Locacion,
                                OrigenTransferencia = idPreparacion,
                                OrigenTabla = "PreparacionBarro",
                                PesoTotal = pBarroSolicitado * -1
                            };
                            lst.Add(tmpMovB_inNegro);
                        }
                    }
                }
            }

            return lst;
        }
        

        [HttpPost]
        public JsonResult ActualizarRecervasFrom(int pLocacion, DateTime pFecha, int pId)
        {
            //Este accion es para consumo interno de Edit.
            List<ReservaBarro> lstRes = tsvc.getReservasEnKgFrom(pLocacion, pFecha, pId);
            return Json(lstRes);
        }

        // GET: PreparacionBarro/Edit/5
        public ActionResult Edit(int id)
        {            
            PreparacionBarroConsumo PreBarCon = tsvc.detallePreparacionBarroConsumo(id);            
            ViewBag.lstRecervas = tsvc.getReservasFrom((int)PreBarCon.Locacion);
            ViewBag.lstLocaciones = tsvc.obtenerSucursales();
            return View(PreBarCon);
        }

        // POST: PreparacionBarro/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, PreparacionBarroConsumo pPreBar)
        {
            if (ModelState.IsValid)
            {                
                int? SumBN = pPreBar.lstConsumoBarroNegro.Select(x => new { TotalSolicitado = x.Capacidad * x.BarroUsado }).Sum(x => x.TotalSolicitado);
                int? SumBB = pPreBar.lstConsumoBarroBlanco.Select(x => new { TotalSolicitado = x.Capacidad * x.BarroUsado }).Sum(x => x.TotalSolicitado);

                List<BarroMovimientos> lst = new List<BarroMovimientos>();
                pPreBar.Estado = "Disponible";
                PreparacionBarro PB = new PreparacionBarro()
                {
                    Id = pPreBar.Id,
                    FechaPreparacion = pPreBar.FechaPreparacion,
                    NumPreparado = pPreBar.NumPreparado,
                    BarroNegro = pPreBar.BarroNegro,
                    BarroBlanco = pPreBar.BarroBlanco,
                    Recuperado = pPreBar.Recuperado,
                    EnPiedra = pPreBar.EnPiedra,
                    TiempoAgitacion = pPreBar.TiempoAgitacion,
                    NumTambos = pPreBar.NumTambos,
                    DesperdicioMojado = pPreBar.DesperdicioMojado,
                    Comentario = pPreBar.Comentario,
                    Estado = pPreBar.Estado,
                    Locacion = pPreBar.Locacion
                };

                //Si las recercas asignadas para cubrir el barro Negro solicitado son mayores, se cubre el pedido, en caso contrario se solicita asignar mas recervas.
                if (pPreBar.BarroNegro <= SumBN)
                {
                    //Si las recercas asignadas para cubrir el barro Blanco solicitado son mayores, se cubre el pedido, en caso contrario se solicita asignar mas recervas.
                    if (pPreBar.BarroBlanco <= SumBB)
                    {
                        int idPreparacion = tsvc.editPreparacionBarro(PB);                        
                        if (idPreparacion != -1)
                        {
                            int elemBorrados = tsvc.borrarMovimientosBarroDerivadosDePreparacionBarro(PB.Id);

                            var lstBarMovNegro = calculaBarroMovimientos(pPreBar.lstConsumoBarroNegro, (int)pPreBar.BarroNegro, PB, idPreparacion);
                            lst.AddRange(lstBarMovNegro);

                            var lstBarMovBlanco = calculaBarroMovimientos(pPreBar.lstConsumoBarroBlanco, (int)pPreBar.BarroBlanco, PB, idPreparacion);
                            lst.AddRange(lstBarMovBlanco);

                            //Por ultimo se registran los movimientos en BD.
                            int res2 = tsvc.addMovimientosBarro(lst);
                            if (res2 > 0)
                            {
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            ViewBag.txtMensaje = "No se pudo crear la preparación, vuelva a intentarlo más tarde.";
                        }
                    }
                    else //Como las recervas son menores al barro solicitado, se pide una cantidad mayor de recervas.
                    {
                        ViewBag.txtMensaje = "Es necesario que asignes reservas de barro blanco mayores que el barro blanco solicitado para la preparación.";
                    }
                }
                else //Como las recervas son menores al barro solicitado, se pide una cantidad mayor de recervas.
                {
                    ViewBag.txtMensaje = "Es necesario que asignes reservas de barro mayores que el barro negro solicitado para la preparación.";
                }              
            }

            ViewBag.lstRecervas = tsvc.getReservasFrom((int)pPreBar.Locacion);
            ViewBag.lstLocaciones = tsvc.obtenerSucursales();
            return View(pPreBar);
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
                int elemBorrados = tsvc.borrarMovimientosBarroDerivadosDePreparacionBarro(id);
                return RedirectToAction("Index");
            }
            else
            {
                return View(new HttpStatusCodeResult(202, "No se pudo realizar el borrado, intentelo mas tarde"));
            }
        }
    }
}
