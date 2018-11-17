using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalaveraWeb.Models;

namespace TalaveraWeb.Services
{
    public class TalaveraServices
    {
        TalaveraEntities db = new TalaveraEntities();

        //PROVEDORES
        public List<Provedores> getProvedores()
        {
            try
            {
                return db.Provedores.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }            
        }

        public int newProvedores(Provedores pProv)
        {
            db.Provedores.Add(pProv);
            int res = db.SaveChanges();
            return res;
        }

        public Provedores getProvedor(int? pId)
        {
            Provedores tmpProv = db.Provedores.Where(x => x.id == pId).FirstOrDefault();
            return tmpProv;
        }

        public int editProvedores(int pId, Provedores pProv)
        {
            db.Entry(pProv).State = EntityState.Modified;
            int res = db.SaveChanges();
            return res;
        }

        public int deleteProvedores(int pId, Provedores pProv)
        {
            try
            {
                Provedores prov = db.Provedores.Find(pId);
                db.Provedores.Remove(prov);
                int res = db.SaveChanges();
                return res;
            }
            catch(Exception ex)
            {                
                return -1;
            }            
        }


        //BARROMAESTRA
        public List<BarroMaestra> getBarroMestra()
        {
            try
            {
                return db.BarroMaestra.ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine("GetBarroMaestra error: " + ex.Message);
                return null;
            }
            
        }

        public BarroMaestra getBarroMaestra(int pId)
        {
            try
            {
                BarroMaestra res = db.BarroMaestra.Where(x => x.Id == pId).FirstOrDefault();
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int addBarroMaestra(BarroMaestra pBarro)
        {
            try
            {
                db.BarroMaestra.Add(pBarro);
                int res = db.SaveChanges();
                return res;
            }
            catch(Exception ex)
            {
                return -1;
            }            
        }

        //Valida si ya existe un producto similar, para no duplicar productos de barro
        public BarroMaestra validaExistenciaBarroMaestra(BarroMaestra pBarro)
        {
            try
            {
                BarroMaestra res = db.BarroMaestra.Where(x => x.CodigoProducto == pBarro.CodigoProducto).FirstOrDefault();                
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int deleteBarroMaestra(int pId)
        {
            try
            {
                BarroMaestra barro = db.BarroMaestra.Find(pId);
                db.BarroMaestra.Remove(barro);
                int res = db.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }


        //BARRO EN GRANEL, EMPAQUETADO.
        public int getBarroTotalDe(int pSucursal, string pCodPro)
        {
            try
            {
                var lst = db.BarroMovimientos
                    .Where(x => x.Locacion == pSucursal && x.CodigoProducto == pCodPro)
                    .GroupBy(y => new { y.TipoMovimiento, y.CodigoProducto })
                    .Select(z => new { TipoMovimiento = z.Key, Unidades = z.Sum(s => s.Unidades) })
                    .ToList();

                var Positivo = lst.Where(x => x.TipoMovimiento.TipoMovimiento == "In").Select(y => y.Unidades).FirstOrDefault();
                var Negativo = lst.Where(x => x.TipoMovimiento.TipoMovimiento == "Eg").Select(y => y.Unidades).FirstOrDefault();

                var Barro = db.BarroMaestra.Where(x => x.CodigoProducto == pCodPro).FirstOrDefault();

                int? Total = (Positivo != null ? Positivo : 0) - (Negativo != null ? Negativo : 0);
                ReservaBarro RecBar = new ReservaBarro() { CodigoBarro = pCodPro, Tipo = Barro.Tipo, Capacidad = Barro.Capacidad, Unidades = Total, TotalKg = Barro.Capacidad * Total };

                return 0;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        //Barro en Granel
        public List<ReservaBarro> getReservasBarroGranelFrom(int pLocacion)
        {
            //Obtengo los registros segun el total de ingresos y egresos
            var lstCodigosBarroGranel = db.BarroMaestra.Where(bm => bm.Capacidad == 1).Select(y => y.CodigoProducto).ToList();
            var lst = db.BarroMovimientos
                .Where(x => x.Locacion == pLocacion &&  lstCodigosBarroGranel.Contains(x.CodigoProducto))
                .GroupBy(y => new { y.TipoMovimiento, y.CodigoProducto })
                .Select(z => new { TipoMovimiento = z.Key, Unidades = z.Sum(s => s.Unidades), TotalKg = z.Sum(ss => ss.PesoTotal) })
                .ToList();
            
            //Obtengo los distintos tipos de productos para hacer las sumas correspondientes.
            var lstTiposBarro = lst.GroupBy(x => x.TipoMovimiento.CodigoProducto).Select(y => y.First());

            var lstBarros = db.BarroMaestra.ToList();

            List<ReservaBarro> lstReservas = new List<ReservaBarro>();
            foreach (var item in lstTiposBarro)
            {
                var Positivo = lst.Where(x => x.TipoMovimiento.CodigoProducto == item.TipoMovimiento.CodigoProducto && x.TipoMovimiento.TipoMovimiento == "In").Select(y => y.Unidades).FirstOrDefault();
                var Negativo = lst.Where(x => x.TipoMovimiento.CodigoProducto == item.TipoMovimiento.CodigoProducto && x.TipoMovimiento.TipoMovimiento == "Eg").Select(y => y.Unidades).FirstOrDefault();
                var tmBarro = lstBarros.Where(x => x.CodigoProducto == item.TipoMovimiento.CodigoProducto).Select(y => new { CodigoProducto = y.CodigoProducto, Tipo = y.Tipo, Capacidad = y.Capacidad }).FirstOrDefault();

                int? Total = (Positivo != null ? Positivo : 0) - (Negativo != null ? Negativo : 0);
                ReservaBarro RecBar = new ReservaBarro() { CodigoBarro = tmBarro.CodigoProducto, Tipo = tmBarro.Tipo, Capacidad = tmBarro.Capacidad, Unidades = Total, TotalKg = Total };

                lstReservas.Add(RecBar);
            }

            return lstReservas;
        }

        //Barro empaquetado
        public List<ReservaBarro> getReservasBarroEmpaqueFrom(int pLocacion)
        {
            //Obtengo los registros segun el total de ingresos y egresos
            var lstCodigosBarroEmpaque = db.BarroMaestra.Where(bm => bm.Capacidad != 1).Select(y => y.CodigoProducto).ToList();
            var lst = db.BarroMovimientos
                .Where(x => x.Locacion == pLocacion && lstCodigosBarroEmpaque.Contains(x.CodigoProducto))
                .GroupBy(y => new { y.TipoMovimiento, y.CodigoProducto })
                .Select(z => new { TipoMovimiento = z.Key, Unidades = z.Sum(s => s.Unidades), TotalKg = z.Sum(ss => ss.PesoTotal) })
                .ToList();

            //Obtengo los distintos tipos de productos para hacer las sumas correspondientes.
            var lstTiposBarro = lst.GroupBy(x => x.TipoMovimiento.CodigoProducto).Select(y => y.First());

            var lstBarros = db.BarroMaestra.ToList();

            List<ReservaBarro> lstReservas = new List<ReservaBarro>();
            foreach (var item in lstTiposBarro)
            {
                //Obtengo el peso total de los empaquetados
                var Positivo = lst.Where(x => x.TipoMovimiento.CodigoProducto == item.TipoMovimiento.CodigoProducto && x.TipoMovimiento.TipoMovimiento == "In").Select(y => y.Unidades).FirstOrDefault();
                var Negativo = lst.Where(x => x.TipoMovimiento.CodigoProducto == item.TipoMovimiento.CodigoProducto && x.TipoMovimiento.TipoMovimiento == "Eg").Select(y => y.Unidades).FirstOrDefault();                
                int? Total = (Positivo != null ? Positivo : 0) - (Negativo != null ? Negativo : 0);

                //Obtengo el peso total en Kg.                
                var PositivoKg = lst.Where(x => x.TipoMovimiento.CodigoProducto == item.TipoMovimiento.CodigoProducto && x.TipoMovimiento.TipoMovimiento == "In").Select(y => y.TotalKg).FirstOrDefault();
                var NegativoKg = lst.Where(x => x.TipoMovimiento.CodigoProducto == item.TipoMovimiento.CodigoProducto && x.TipoMovimiento.TipoMovimiento == "Eg").Select(y => y.TotalKg).FirstOrDefault();                
                int? TotalKg = (PositivoKg != null ? PositivoKg : 0) - (NegativoKg != null ? NegativoKg : 0);

                //Detalles del tipo de barro.
                var tmBarro = lstBarros.Where(x => x.CodigoProducto == item.TipoMovimiento.CodigoProducto).Select(y => new { CodigoProducto = y.CodigoProducto, Tipo = y.Tipo, Capacidad = y.Capacidad }).FirstOrDefault();

                ReservaBarro RecBar = new ReservaBarro() { CodigoBarro = tmBarro.CodigoProducto, Tipo = tmBarro.Tipo, Capacidad = tmBarro.Capacidad, Unidades = Total, TotalKg = TotalKg }; // item.TotalKg
                
                lstReservas.Add(RecBar);
            }

            return lstReservas;
        }

        //Catalogos para los Movimientos de barro
        public List<SelectListItem> obtenerProductos(int pCapacidad = 0)
        {
            List<SelectListItem> lst;
            if (pCapacidad != 0)
            {
                lst = db.BarroMaestra.Where(y => y.Capacidad == pCapacidad).Select(x => new SelectListItem() { Text = "Barro " + x.Tipo + " de " + x.Capacidad + " Kg", Value = x.CodigoProducto }).ToList();
            }            
            else
            {
                lst = db.BarroMaestra.Select(x => new SelectListItem() { Text = "Barro " + x.Tipo + " de " + x.Capacidad + " Kg", Value = x.CodigoProducto }).ToList();
            }
            lst.First().Selected = true;
            return lst;
        }

        public List<SelectListItem> obtenerProductosDeTipo(string pTipo)
        {            
            List<SelectListItem> lst = db.BarroMaestra
                .Where(y => y.Tipo.Contains(pTipo) && y.Capacidad != 1)
                .Select(x => new SelectListItem() { Text = "Barro " + x.Tipo + " de " + x.Capacidad + " Kg", Value = x.CodigoProducto })
                .ToList();            
            lst.First().Selected = true;
            return lst;
        }

        public string obtenerCodigoDeProducto(string pTipo, int pCapacidad)
        {
            return db.BarroMaestra.Where(x => x.Tipo == pTipo && x.Capacidad == pCapacidad).Select(y => y.CodigoProducto).FirstOrDefault();
        }
        public List<SelectListItem> obtenerProvedores()
        {
            List<SelectListItem> lst = db.Provedores.Select(x => new SelectListItem() { Text = x.Nombre, Value = x.id.ToString() }).ToList();
            lst.First().Selected = true;
            return lst;
        }
        //Se hizo de esta manera pues no se considero necesario crear un catalogo en BD para 2 locaciones
        public List<SelectListItem> obtenerSucursales(int pSucursal = 0)
        {
            //List<SelectListItem> lst = new List<SelectListItem>() {
            //    new SelectListItem() { Text = "34 pte", Value = "34 pte" },
            //    new SelectListItem() { Text = "La Luz", Value = "La Luz" }
            //};
            List<SelectListItem> lst;
            if (pSucursal > 0)
            {
                lst = db.Sucursales.Where(y => y.Id == pSucursal).Select(x => new SelectListItem() { Text = x.Nombre, Value = x.Id.ToString() }).ToList();
            }
            else
            {
                lst = db.Sucursales.Select(x => new SelectListItem() { Text = x.Nombre, Value = x.Id.ToString() }).ToList();
            }
            lst.First().Selected = true;
            return lst;
        }

        

        //MOVIMIENTOS BARRO
        public List<ReservaBarro> getReservasFrom(int pLocacion)
        {
            //Obtengo los registros segun el total de ingresos y egresos
            var lst = db.BarroMovimientos
                .Where(x => x.Locacion == pLocacion)
                .GroupBy(y => new { y.TipoMovimiento, y.CodigoProducto })
                .Select(z => new { TipoMovimiento = z.Key, Unidades = z.Sum(s => s.Unidades) })
                .ToList();

            //Obtengo los distintos tipos de productos para hacer las sumas correspondientes.
            var lstTiposBarro = lst.GroupBy(x => x.TipoMovimiento.CodigoProducto).Select(y => y.First());

            var lstBarros = db.BarroMaestra.ToList();

            List<ReservaBarro> lstReservas = new List<ReservaBarro>();
            foreach(var item in lstTiposBarro)
            {
                var Positivo = lst.Where(x => x.TipoMovimiento.CodigoProducto == item.TipoMovimiento.CodigoProducto && x.TipoMovimiento.TipoMovimiento == "In").Select(y => y.Unidades).FirstOrDefault();
                var Negativo = lst.Where(x => x.TipoMovimiento.CodigoProducto == item.TipoMovimiento.CodigoProducto && x.TipoMovimiento.TipoMovimiento == "Eg").Select(y => y.Unidades).FirstOrDefault();
                var tmBarro = lstBarros.Where(x => x.CodigoProducto == item.TipoMovimiento.CodigoProducto).Select(y => new { CodigoProducto = y.CodigoProducto, Tipo = y.Tipo, Capacidad = y.Capacidad }).FirstOrDefault();

                int? Total = (Positivo != null ? Positivo : 0) - (Negativo != null ? Negativo : 0);
                ReservaBarro RecBar = new ReservaBarro() { CodigoBarro = tmBarro.CodigoProducto , Tipo = tmBarro.Tipo, Capacidad = tmBarro.Capacidad, Unidades = Total, TotalKg = tmBarro.Capacidad * Total };
                                
                lstReservas.Add(RecBar);            
            }        

            return lstReservas;
        }

        public List<ReservaBarro> getReservasEnKgFrom(int pLocacion)
        {
            //Obtengo los registros segun el total de ingresos y egresos
            var lst = db.BarroMovimientos
                .Where(x => x.Locacion == pLocacion)
                .GroupBy(y => new { y.TipoMovimiento, y.CodigoProducto })
                .Select(z => new { TipoMovimiento = z.Key, Unidades = z.Sum(s => s.Unidades) })
                .ToList();

            //Obtengo los distintos tipos de productos para hacer las sumas correspondientes.
            var lstTiposBarro = lst.GroupBy(x => x.TipoMovimiento.CodigoProducto).Select(y => y.First());

            var lstBarros = db.BarroMaestra.ToList();

            List<ReservaBarro> lstReservas = new List<ReservaBarro>();
            foreach (var item in lstTiposBarro)
            {
                var Positivo = lst.Where(x => x.TipoMovimiento.CodigoProducto == item.TipoMovimiento.CodigoProducto && x.TipoMovimiento.TipoMovimiento == "In").Select(y => y.Unidades).FirstOrDefault();
                var Negativo = lst.Where(x => x.TipoMovimiento.CodigoProducto == item.TipoMovimiento.CodigoProducto && x.TipoMovimiento.TipoMovimiento == "Eg").Select(y => y.Unidades).FirstOrDefault();
                var tmBarro = lstBarros.Where(x => x.CodigoProducto == item.TipoMovimiento.CodigoProducto).Select(y => new { CodigoProducto = y.CodigoProducto, Tipo = y.Tipo, Capacidad = y.Capacidad }).FirstOrDefault();

                int? Total = (Positivo != null ? Positivo : 0) - (Negativo != null ? Negativo : 0);
                ReservaBarro RecBar = new ReservaBarro() { CodigoBarro = tmBarro.CodigoProducto, Tipo = tmBarro.Tipo, Capacidad = tmBarro.Capacidad, Unidades = Total, TotalKg = tmBarro.Capacidad * Total };

                lstReservas.Add(RecBar);
            }

            return lstReservas.GroupBy(x => x.Tipo).Select(y => new ReservaBarro() { CodigoBarro = "N", Tipo = y.Key, Unidades = y.Sum(s => s.TotalKg), TotalKg = y.Sum(s2 => s2.TotalKg) }).ToList();

            //return lstReservas;
        }

        public int addMovimientosBarro(BarroMovimientos pMovB)
        {
            try
            {
                db.BarroMovimientos.Add(pMovB);
                int res = db.SaveChanges();
                return res;
            }
            catch(Exception ex)
            {
                return -1;
            } 
        }
        public int addMovimientosBarro(List<BarroMovimientos> plstMovB)
        {
            try
            {
                db.BarroMovimientos.AddRange(plstMovB);
                int res = db.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        
                
        //PREPARACION DE BARROS
        public List<PreparacionBarro> obtenerPreparacionBarro(int pLocacion)
        {            
            List<PreparacionBarro> lstPreBar = new List<PreparacionBarro>();
            try
            {
                var lst = db.PreparacionBarro.Where(y => y.Estado == "Disponible" && y.Locacion == pLocacion).OrderBy(x => x.FechaPreparacion).ToList();
                if(lst != null)
                {
                    lstPreBar = lst;
                }
            }
            catch (Exception ex)
            {
            }
            return lstPreBar;
        }

        public PreparacionBarro detallePreparacionBarro(int pId)
        {
            return db.PreparacionBarro.Where(x => x.Id == pId).FirstOrDefault();
        }

        public int addPreparacionBarro(PreparacionBarro pPreBar)
        {
            try
            {
                db.PreparacionBarro.Add(pPreBar);
                int res = db.SaveChanges();
                return pPreBar.Id;      //Obtengo el id del nuevo registro 
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        //Establece el numero de preparado, segun el año.
        public string getNumeroPreparado()
        {
            int Count = db.PreparacionBarro.Count(x => x.FechaPreparacion.Value.Year == DateTime.Today.Year);
            string res = DateTime.Today.Year + "p" + (Count + 1);            
            return res;
        }
        
        public int editPreparacionBarro(int pId, PreparacionBarro pPreBar)
        {
            try
            {
                db.Entry(pPreBar).State = EntityState.Modified;
                int res = db.SaveChanges();
                return res;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return -1;
            }
        }

        public int deletePreparacionBarro(int pId)
        {
            try
            {
                PreparacionBarro PreBar = db.PreparacionBarro.Find(pId);
                db.PreparacionBarro.Remove(PreBar);
                int res = db.SaveChanges();
                return res;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return -1;
            }
        }

        //PREPARACION PELLAS
        public List<PreparacionPellas> obtenerPreparacionPellas()
        {
            return db.PreparacionPellas.OrderByDescending(x => x.NumCarga).Take(10).ToList();
        }

        //Establece el numero de carga en fuentes, segun el año
        public string getNumeroCarga()
        {
            int Count = db.PreparacionPellas.Count(x => x.FechaVaciado.Value.Year == DateTime.Today.Year);
            string res = DateTime.Today.Year + "c" + (Count + 1);
            return res;
        }

        public List<prepBarro_prepPellas> getPreparadosDeCarga(string pCarga)
        {
            try
            {
                return db.prepBarro_prepPellas.Where(x => x.NumCarga == pCarga).ToList();
            }
            catch (Exception ex) {
                Console.WriteLine("getPreparados de Carga Error: " + ex.Message);
                return null;
            }            
        }
        
        public List<PreparacionBarro> getPreparadosDisponibles()
        {
            try
            {
                return db.PreparacionBarro.Where(x => x.Estado == "Disponible").ToList();
            }
            catch(Exception ex)
            {
                return null;
            }            
        }

        public int addPreparacionPellas(PreparacionPellas pPrePell)
        {
            try
            {
                db.PreparacionPellas.Add(pPrePell);
                int res = db.SaveChanges();
                return res;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Preparación Pellas Error: " + ex.Message);
                return -1;
            }
        }

        public PreparacionPellas detallePreparacionPellas(int pId)
        {
            return db.PreparacionPellas.Where(x => x.Id == pId).FirstOrDefault();
        }

        public int editPreparacionPellas(PreparacionPellas pPrePell)
        {
            try
            {
                db.Entry(pPrePell).State = EntityState.Modified;
                int res = db.SaveChanges();
                return res;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Edición Preparación Pellas Error: " + ex.Message);
                return -1;
            }            
        }

        public int deletePreparacionPellas(int pId)
        {
            try
            {
                PreparacionPellas tmp = db.PreparacionPellas.Find(pId);
                db.PreparacionPellas.Remove(tmp);
                int res = db.SaveChanges();
                return res;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Delete Preparacion Pellas Error: " + ex.Message);
                return -1;
            }
        }
    }
}