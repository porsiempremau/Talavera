﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalaveraWeb.Models;
using TalaveraWeb.Models.clasesPlanDeTrabajo;
using TalaveraWeb.Models.MiBD;

namespace TalaveraWeb.Services
{
    public class TalaveraServices
    {
        //TalaveraEntities db = new TalaveraEntities();
        ApplicationDbContext db = new ApplicationDbContext();


        //SUCURSALES
        public string getNombreSucursal(int pLoc)
        {
            return db.Sucursales.Where(x => x.Id == pLoc).Select(y => y.Nombre).FirstOrDefault();
        }

        //Catalogo Talavera
        public CatalogoTalavera getPiezasTalavera(int pId)
        {
            try
            {
                return db.CatalogoTalavera.Find(pId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<CatalogoTalavera> getPiezasTalavera(string texto = "")
        {
            try
            {
                List<CatalogoTalavera> lst = new List<CatalogoTalavera>();
                if (string.IsNullOrEmpty(texto))
                {
                    lst = db.CatalogoTalavera.OrderBy(x => x.NombrePieza).ToList();
                }
                else
                {
                    lst = db.CatalogoTalavera.Where(x => x.NombrePieza.Contains(texto)).OrderBy(y => y.NombrePieza).ToList();
                }
                return lst;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public int addPiezaTalavera(CatalogoTalavera pCT)
        {
            try
            {
                db.CatalogoTalavera.Add(pCT);
                int res = db.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                return -1;
            }            
        }

        public int editCatalogo(CatalogoTalavera pCT)
        {
            try
            {
                db.Entry(pCT).State = EntityState.Modified;
                int res = db.SaveChanges();
                return res;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        public int deleteCatalogo(int pId)
        {
            try
            {
                CatalogoTalavera prov = db.CatalogoTalavera.Find(pId);
                db.CatalogoTalavera.Remove(prov);
                int res = db.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

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

        //PERSONAL TALAVERA
        public List<PersonalTalavera> getPersonalTalavera()
        {
            try
            {
                return db.PersonalTalavera.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int addPersonalTalaver(PersonalTalavera pPT)
        {
            try
            {
                db.PersonalTalavera.Add(pPT);
                int res = db.SaveChanges();
                return res;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        public PersonalTalavera getPersonal(int id)
        {
            try
            {
                return db.PersonalTalavera.Find(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int editPersonal(PersonalTalavera pPT)
        {
            try
            {
                db.Entry(pPT).State = EntityState.Modified;
                int res = db.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int deletePersonal(int pId)
        {
            try
            {
                PersonalTalavera per = db.PersonalTalavera.Find(pId);
                db.PersonalTalavera.Remove(per);
                int res = db.SaveChanges();
                return res;
            }
            catch (Exception ex)
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

                double? Total = (Positivo != null ? Positivo : 0) - (Negativo != null ? Negativo : 0);
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

                double? Total = (Positivo != null ? Positivo : 0) - (Negativo != null ? Negativo : 0);
                ReservaBarro RecBar = new ReservaBarro() { CodigoBarro = tmBarro.CodigoProducto, Tipo = tmBarro.Tipo, Capacidad = tmBarro.Capacidad, Unidades = Total, TotalKg = Total };

                lstReservas.Add(RecBar);
            }

            return lstReservas;
        }

        public List<BarroMovimientos> obtenerHistorialGranel(int Sucursal)
        {
            try
            {
                //Obtengo los registros de ingreso de barro a granel
                var lstIngresosBarroGranel = db.BarroMovimientos.Where(bm => bm.TipoMovimiento == "In" && bm.Locacion == Sucursal).OrderBy(x => x.FechaMovimiento).Take(10).ToList();
                return lstIngresosBarroGranel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public BarroMovimientos getBarroMovimiento(int pId)
        {
            try
            {
                BarroMovimientos bm = db.BarroMovimientos.Find(pId);
                return bm;
            }
            catch(Exception ex)
            {
                return null;
            }
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
                double? Total = (Positivo != null ? Positivo : 0) - (Negativo != null ? Negativo : 0);

                //Obtengo el peso total en Kg.                
                var PositivoKg = lst.Where(x => x.TipoMovimiento.CodigoProducto == item.TipoMovimiento.CodigoProducto && x.TipoMovimiento.TipoMovimiento == "In").Select(y => y.TotalKg).FirstOrDefault();
                var NegativoKg = lst.Where(x => x.TipoMovimiento.CodigoProducto == item.TipoMovimiento.CodigoProducto && x.TipoMovimiento.TipoMovimiento == "Eg").Select(y => y.TotalKg).FirstOrDefault();                
                double? TotalKg = (PositivoKg != null ? PositivoKg : 0) - (NegativoKg != null ? NegativoKg : 0);

                //Detalles del tipo de barro.
                var tmBarro = lstBarros.Where(x => x.CodigoProducto == item.TipoMovimiento.CodigoProducto).Select(y => new { CodigoProducto = y.CodigoProducto, Tipo = y.Tipo, Capacidad = y.Capacidad }).FirstOrDefault();

                ReservaBarro RecBar = new ReservaBarro() { CodigoBarro = tmBarro.CodigoProducto, Tipo = tmBarro.Tipo, Capacidad = tmBarro.Capacidad, Unidades = Total, TotalKg = TotalKg }; // item.TotalKg
                
                lstReservas.Add(RecBar);
            }

            return lstReservas;
        }

        public List<BarroMovimientos> obtenerHistorialEmpaquetados(int Sucursal)
        {
            try
            {
                //Obtengo los registros de ingreso de barro empaquetado
                var lstIngresosBarroEmpaque = db.BarroMovimientos.Where(bm => bm.TipoMovimiento == "In" && bm.Locacion == Sucursal && (bm.CodigoProducto != "B1" && bm.CodigoProducto != "N1") ).OrderBy(x => x.FechaMovimiento).Take(10).ToList();
                return lstIngresosBarroEmpaque;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public double obtenerMermaDePedido(BarroMovimientos pBMCM)
        {
            try
            {
                double? res = db.BarroMovimientos.Where(x => x.OrigenTransferencia == pBMCM.Id && x.Locacion == pBMCM.OrigenTransferencia && x.OrigenTabla == "BarroMovimiento").Select(y => y.Unidades).FirstOrDefault();
                return res != null ? (double)res : 0;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        //Barro en pellas
        public List<ReservaBarro> getReservasPellasFrom(int pLocacion)
        {
            try
            {                
                var lstRes = db.EntregaPellas.Where(x => x.Locacion == pLocacion)
                        .GroupBy(y => y.TipoMovimiento)
                        .Select(z => new { TipoMovimiento = z.Key, Total = z.Sum(s => s.CantidadPellas) })
                        .ToList();                

                //Obtengo las pellas disponibles para cada tipo de carga.
                int? Positivo = 0, Negativo = 0;
                List<ReservaBarro> lstReservas = new List<ReservaBarro>();
                foreach (var item in lstRes)
                {
                    if (item.TipoMovimiento == "I")
                        Positivo = item.Total;
                    if (item.TipoMovimiento == "E")
                        Negativo = item.Total;
                }

                int? Total = (Positivo != null ? Positivo : 0) - (Negativo != null ? Negativo : 0);
                ReservaBarro RecBar = new ReservaBarro() { CodigoBarro = "N/A", Tipo = "Pella 40 kg", Capacidad = null, Unidades = Total, TotalKg = Total * 40 };
                lstReservas.Add(RecBar);

                return lstReservas;
            }
            catch (Exception ex)
            {
                return null;
            }            
        }

        public List<PellasDisponibles> getPellasPorCargaFrom(int pLocacion)
        {
            try
            {
                var lstRes = db.EntregaPellas.Where(x => x.Locacion == pLocacion)
                        .GroupBy(y => new { y.NumCarga, y.TipoMovimiento })
                        .Select(z => new { Grupo = z.Key, Total = z.Sum(s => s.CantidadPellas) })
                        .ToList();

                //Selecciono los distintos codigos de carga.
                var lstCodigosCarga = lstRes.Select(x => x.Grupo.NumCarga).Distinct();

                //Obtengo las pellas disponibles para cada tipo de carga.
                int? Positivo = 0, Negativo = 0;
                List<PellasDisponibles> lstReservas = new List<PellasDisponibles>();
                foreach (var item in lstCodigosCarga)
                {
                    Positivo = lstRes.Where(x => x.Grupo.NumCarga == item && x.Grupo.TipoMovimiento == "I").Select(y => y.Total).FirstOrDefault();
                    Negativo = lstRes.Where(x => x.Grupo.NumCarga == item && x.Grupo.TipoMovimiento == "E").Select(y => y.Total).FirstOrDefault();

                    int? Total = (Positivo != null ? Positivo : 0) - (Negativo != null ? Negativo : 0);
                    if (Total > 0)
                    {
                        PellasDisponibles RecBar = new PellasDisponibles() { Tipo = "Pella 40 kg", Capacidad = null, UnidadesDisponibles = Total, TotalKg = Total * 40, NumeroCarga = item };
                        lstReservas.Add(RecBar);
                    }                    
                }
                 
                return lstReservas;
            }
            catch (Exception ex)
            {
                return null;
            }
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
            try
            {
                List<SelectListItem> lst = db.BarroMaestra
                .Where(y => y.Tipo.Contains(pTipo) && y.Capacidad != 1)
                .Select(x => new SelectListItem() { Text = "Barro " + x.Tipo + " de " + x.Capacidad + " Kg", Value = x.CodigoProducto })
                .ToList();
                if(lst.Count > 0)
                    lst.First().Selected = true;
                return lst;
            }
            catch (Exception ex)
            {
                return null;
            }            
        }

        public string obtenerCodigoDeProducto(string pTipo, int pCapacidad)
        {
            return db.BarroMaestra.Where(x => x.Tipo == pTipo && x.Capacidad == pCapacidad).Select(y => y.CodigoProducto).FirstOrDefault();
        }
        public List<SelectListItem> obtenerProvedores(int? pIdProveedor = 0)
        {
            List<SelectListItem> lst = db.Provedores.Select(x => new SelectListItem() { Text = x.Nombre, Value = x.id.ToString() }).ToList();
                        
            if(pIdProveedor == 0)       //Por default se establece el primer elemento como seleccionado.
                lst.First().Selected = true;
            else         //Si se especificica un id de proveedor, se establece dicho proveedor como elemento seleccionado.            
                lst.Find(x => x.Value == pIdProveedor.ToString()).Selected = true;
            
            return lst;
        }
        
        public List<SelectListItem> obtenerSucursales(int pSucursal = 0)
        {           
            List<SelectListItem> lst;
            //si pSucursal es distinto de cero, regresa solo la sucursal del id solicitado.
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

        public List<SelectListItem> obtenerSucursalesExcepto(int pSucursal = 0)
        {
            //List<SelectListItem> lst = new List<SelectListItem>() {
            //    new SelectListItem() { Text = "34 pte", Value = "34 pte" },
            //    new SelectListItem() { Text = "La Luz", Value = "La Luz" }
            //};
            List<SelectListItem> lst;
            if (pSucursal > 0)
            {
                lst = db.Sucursales.Where(y => y.Id != pSucursal).Select(x => new SelectListItem() { Text = x.Nombre, Value = x.Id.ToString() }).ToList();
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

                double? Total = (Positivo != null ? Positivo : 0) - (Negativo != null ? Negativo : 0);
                ReservaBarro RecBar = new ReservaBarro() { CodigoBarro = tmBarro.CodigoProducto , Tipo = tmBarro.Tipo, Capacidad = tmBarro.Capacidad, Unidades = Total, TotalKg = tmBarro.Capacidad * Total };
                                
                lstReservas.Add(RecBar);            
            }        

            return lstReservas;
        }

        public List<ReservaBarro> getReservasEnKgFrom(int pLocacion, DateTime pFecha, int pId)
        {
            //Obtengo los registros segun el total de ingresos y egresos
            var lst = db.BarroMovimientos                
                .Where(x => x.Locacion == pLocacion && x.FechaMovimiento <= pFecha)
                .Where(x => !db.BarroMovimientos.Where(y => y.OrigenTransferencia == pId && y.OrigenTabla == "PreparacionBarro").Select(z => z.Id).Contains(x.Id))  //Not in list
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

                double? Total = (Positivo != null ? Positivo : 0) - (Negativo != null ? Negativo : 0);
                ReservaBarro RecBar = new ReservaBarro() { CodigoBarro = tmBarro.CodigoProducto, Tipo = tmBarro.Tipo, Capacidad = tmBarro.Capacidad, Unidades = Total, TotalKg = tmBarro.Capacidad * Total };

                lstReservas.Add(RecBar);
            }

            //return lstReservas.GroupBy(x => x.Tipo).Select(y => new ReservaBarro() { CodigoBarro = "N", Tipo = y.Key, Unidades = y.Sum(s => s.TotalKg), TotalKg = y.Sum(s2 => s2.TotalKg) }).ToList();

            return lstReservas;
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
                
        public int editMovimientoBarro(BarroMovimientos pBM)
        {
            try
            {
                db.Entry(pBM).State = EntityState.Modified;
                int res = db.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return -1;
            }
        }

        public int deleteBarroMovimiento(int pId)
        {
            try
            {
                BarroMovimientos BM = db.BarroMovimientos.Find(pId);
                db.BarroMovimientos.Remove(BM);
                int res = db.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return -1;
            }
        }

        public int deleteMermaExistente(BarroMovimientos pBM)
        {
            try
            {
                List<BarroMovimientos> lst = db.BarroMovimientos.Where(x => x.OrigenTransferencia == pBM.Id && x.OrigenTabla == "BarroMovimiento").ToList();
                db.BarroMovimientos.RemoveRange(lst);
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
        public PreparacionBarroConsumo detallePreparacionBarroConsumo(int pId)
        {
            PreparacionBarroConsumo pbc = new PreparacionBarroConsumo();
            PreparacionBarro PreBar = db.PreparacionBarro.Where(x => x.Id == pId).FirstOrDefault();
            pbc.getPreparacionBarro(PreBar);

            var tmp = db.BarroMovimientos
                .Where(x => x.OrigenTransferencia == pId && x.OrigenTabla == "PreparacionBarro" && x.CodigoProducto.Contains("N") && x.TipoMovimiento == "Eg").ToList();
            List<ReservaBarroPreparado> tmpNegro = new List<ReservaBarroPreparado>();
            foreach(var item in tmp)
            {
                tmpNegro.Add(new ReservaBarroPreparado()
                {
                    CodigoBarro = item.CodigoProducto,
                    Tipo = "Negro",
                    Capacidad = int.Parse(item.CodigoProducto.Remove(0, 1)),
                    Unidades = item.Unidades,
                    TotalKg = item.PesoTotal,
                    BarroUsado = item.Unidades.Value
                });
            }
            pbc.lstConsumoBarroNegro = tmpNegro;

            //pbc.lstConsumoBarroNegro = db.BarroMovimientos
            //    .Where(x => x.OrigenTransferencia == pId && x.OrigenTabla == "PreparacionBarro" && x.CodigoProducto.Contains("N") && x.TipoMovimiento == "Eg")
            //    .Select(x => new ReservaBarroPreparado()
            //    {
            //        CodigoBarro = x.CodigoProducto,
            //        Tipo = "Negro",
            //        //Capacidad = int.Parse(x.CodigoProducto.Remove(0,1)),
            //        Unidades = x.Unidades,
            //        TotalKg = x.PesoTotal,
            //        BarroUsado = x.Unidades.Value
            //    })
            //    .ToList();
            //foreach (var item in pbc.lstConsumoBarroNegro)
            //{
            //    item.Capacidad = int.Parse(item.CodigoBarro.Remove(0, 1));
            //}



            var tmp2 = db.BarroMovimientos
                .Where(x => x.OrigenTransferencia == pId && x.OrigenTabla == "PreparacionBarro" && x.CodigoProducto.Contains("B") && x.TipoMovimiento == "Eg").ToList();
            List<ReservaBarroPreparado> tmpBlanco = new List<ReservaBarroPreparado>();
            foreach (var item in tmp2)
            {
                tmpBlanco.Add(new ReservaBarroPreparado()
                {
                    CodigoBarro = item.CodigoProducto,
                    Tipo = "Blanco",
                    Capacidad = int.Parse(item.CodigoProducto.Remove(0, 1)),
                    Unidades = item.Unidades,
                    TotalKg = item.PesoTotal,
                    BarroUsado = item.Unidades.Value
                });
            }
            pbc.lstConsumoBarroBlanco = tmpBlanco;
            
            //pbc.lstConsumoBarroBlanco = db.BarroMovimientos
            //    .Where(x => x.OrigenTransferencia == pId && x.OrigenTabla == "PreparacionBarro" && x.CodigoProducto.Contains("B") && x.TipoMovimiento == "Eg")
            //    .Select(x => new ReservaBarroPreparado()
            //    {
            //        CodigoBarro = x.CodigoProducto,
            //        Tipo = "Blanco",
            //        //Capacidad = int.Parse(x.CodigoProducto.Substring(1)),
            //        Unidades = x.Unidades,
            //        TotalKg = x.PesoTotal,
            //        BarroUsado = x.Unidades.Value
            //    }).ToList();
            //foreach (var item in pbc.lstConsumoBarroBlanco)
            //{
            //    item.Capacidad = int.Parse(item.CodigoBarro.Remove(0, 1));
            //}

            return pbc;
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
        public string getNumeroPreparado(int pLocacion)
        {
            string SiglaCodigo = db.Sucursales.Where(x => x.Id == pLocacion).Select(y => y.SiglaCodigo).FirstOrDefault();
            int Count = db.PreparacionBarro.Where(y => y.NumPreparado.Contains(SiglaCodigo)).Count(x => x.FechaPreparacion.Value.Year == DateTime.Today.Year);            
            string res = DateTime.Today.Year + "p" + SiglaCodigo + (Count + 1);            
            return res;
        }
        
        public int editPreparacionBarro(PreparacionBarro pPreBar)
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

        //Se borran los registros en BarroMovimientos derivados de la preparacion de barro dada.
        public int borrarMovimientosBarroDerivadosDePreparacionBarro(int pPreBarId)
        {
            try
            {
                List<BarroMovimientos> lstElems = db.BarroMovimientos.Where(x => x.OrigenTabla == "PreparacionBarro" && x.OrigenTransferencia == pPreBarId).ToList();
                db.BarroMovimientos.RemoveRange(lstElems);
                int res = db.SaveChanges();
                return res;
            }
            catch(Exception ex)
            {
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
        public List<PreparacionPellas> obtenerPreparacionPellas(int pLoc)
        {
            return db.PreparacionPellas.Where(x => x.Locacion == pLoc).OrderByDescending(x => x.NumCarga).Take(10).ToList();
        }

        //Establece el numero de carga en fuentes, segun el año
        public string getNumeroCarga(int pLocacion)
        {
            string SiglaCodigo = db.Sucursales.Where(x => x.Id == pLocacion).Select(y => y.SiglaCodigo).FirstOrDefault();
            int Count = db.PreparacionPellas.Where(y => y.NumCarga.Contains(SiglaCodigo)).Count(x => x.FechaVaciado.Value.Year == DateTime.Today.Year);
            string res = DateTime.Today.Year + "c" + SiglaCodigo + (Count + 1);
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
        
        public List<string> getPreparadosDisponibles(int pIdSucursal)
        {
            try
            {
                return db.PreparacionBarro.Where(x => x.Estado == "Disponible" && x.Locacion == pIdSucursal).Select(x => x.NumPreparado).ToList();
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
        
        public int addPreparacionPellasRelacionesPreparacionBarro(List<string> plstPreBar, string pPrePell, string pEditor)
        {
            try
            {
                List<prepBarro_prepPellas> lst = new List<prepBarro_prepPellas>();
                foreach (var item in plstPreBar)
                    lst.Add(new prepBarro_prepPellas() { NumPreparado = item, NumCarga = pPrePell, Editor = pEditor, FechaEdicion = DateTime.Now });

                db.prepBarro_prepPellas.AddRange(lst);
                int res = db.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine("addPreparacionPellasRelacionesPreparacionBarro error: " + ex.Message);
                return -1;
            }
        }
                
        //Actualiza en la tabla de PreparacionBarro el campo de Estado de "Disponible" a "Consumido"
        public int editaEstadoPrepBarroPorPreparacionPellas(string pNumeroDeCarga, string pEstado, string pEditor)
        {
            try
            {
                var tmpList = db.prepBarro_prepPellas.Where(x => x.NumCarga == pNumeroDeCarga).Select(y => y.NumPreparado).ToList();
                var friends = db.PreparacionBarro.Where(f => tmpList.Contains(f.NumPreparado)).ToList();
                friends.ForEach(a => a.Estado = pEstado);
                friends.ForEach(a => a.Editor = pEditor);
                friends.ForEach(a => a.FechaEdicion = DateTime.Now);
                int res = db.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public PreparacionPellas detallePreparacionPellas(int pId)
        {
            return db.PreparacionPellas.Where(x => x.Id == pId).FirstOrDefault();
        }

        public List<string> preparacionesAsignadasACarga(string pNumeroDeCarga)
        {
            var tmp = db.prepBarro_prepPellas.Where(x => x.NumCarga == pNumeroDeCarga).Select(y => y.NumPreparado).ToList();
            return tmp;
        }

        public int editPreparacionPellas(PreparacionPellas pPrePell)
        {
            try
            {
                var pp = db.PreparacionPellas.FirstOrDefault(x => x.Id == pPrePell.Id);
                pp.Fuente = pPrePell.Fuente;
                pp.NumCarga = pPrePell.NumCarga;
                pp.FechaVaciado = pPrePell.FechaVaciado;
                pp.FechaLevantado = pPrePell.FechaLevantado;
                pp.FechaInicoPisado = pPrePell.FechaInicoPisado;
                pp.FechaFinPisado = pPrePell.FechaFinPisado;
                pp.NumPeyas = pPrePell.NumPeyas;
                pp.Restante = pPrePell.Restante;
                pp.CargaTotal = pPrePell.CargaTotal;
                pp.Editor = pPrePell.Editor;
                pp.FechaEdicion = pPrePell.FechaEdicion;
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

        public int deletePreparacionPellasRelacionesPreparacionBarro(string pNumCarga)
        {
            try
            {
                List<prepBarro_prepPellas> lst = db.prepBarro_prepPellas.Where(x => x.NumCarga == pNumCarga).ToList();
                db.prepBarro_prepPellas.RemoveRange(lst);
                int res = db.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine("addPreparacionPellasRelacionesPreparacionBarro error: " + ex.Message);
                return -1;
            }
        }

        //ENTREGA PELLAS
        public int addEntregaPellas(int? pCantidadPellas, string pCarga, string pResponsable, int pLocacion, string pTipoMovimiento)
        {
            try
            {
                EntregaPellas EP = new EntregaPellas() { FechaMovimiento = DateTime.Today, Responsable = pResponsable, TipoMovimiento = pTipoMovimiento, CantidadPellas = pCantidadPellas, NumCarga = pCarga, Editor = pResponsable, FechaEdicion = DateTime.Now, Locacion = pLocacion };
                db.EntregaPellas.Add(EP);
                int res = db.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public int addEntregaPellas(EntregaPellas pEnPe)
        {
            try
            {                
                db.EntregaPellas.Add(pEnPe);
                int res = db.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int addEntregaPellas(List<EntregaPellas> pEnPe)
        {
            try
            {
                db.EntregaPellas.AddRange(pEnPe);
                int res = db.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<EntregaPellas> calculaMovimientosEntregaPellas(int pLoc)
        {
            try
            {
                List<EntregaPellas> lst = db.EntregaPellas.Where(x => x.Locacion == pLoc).ToList();
                return lst;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        
        public int editEntregaPellas(int? pCantidadPellas, string pCarga, string pResponsable, int pLocacion)
        {
            try
            {
                var ep = db.EntregaPellas.FirstOrDefault(x => x.NumCarga == pCarga);
                ep.CantidadPellas = pCantidadPellas;
                ep.Responsable = pResponsable;
                ep.Editor = pResponsable;
                ep.FechaEdicion = DateTime.Now;
                ep.Locacion = pLocacion;
                int res = db.SaveChanges();                                
                return res;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int editEntregaPellas(EntregaPellas pEP)
        {
            try
            {
                var ep = db.EntregaPellas.FirstOrDefault(x => x.NumCarga == pEP.NumCarga && x.TipoMovimiento == pEP.TipoMovimiento && x.Responsable == pEP.Responsable);
                ep.Responsable = pEP.Responsable;
                ep.TipoMovimiento = pEP.TipoMovimiento;
                ep.CantidadPellas = pEP.CantidadPellas;
                ep.NumCarga = pEP.NumCarga;
                ep.Editor = pEP.Editor;
                ep.FechaEdicion = pEP.FechaEdicion;
                ep.Locacion = pEP.Locacion;
                ep.Observacion = pEP.Observacion;
                int res = db.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public int deleteEntregaPellas(string pCarga)
        {
            try
            {
                List<EntregaPellas> ep = db.EntregaPellas.Where(x => x.NumCarga == pCarga).ToList();
                db.EntregaPellas.RemoveRange(ep);
                int res = db.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<EntregaPellas> listEngregaPellas(int pLoc)
        {
            return db.EntregaPellas.Where(x => x.Locacion == pLoc && x.TipoMovimiento == "E").ToList();
        }


        //PLAN DE TRABAJO

        public List<SelectListItem> getEtapasDeProduccion()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem() { Value = "Moldes", Text = "Moldes" });            
            lst.Add(new SelectListItem() { Value = "Torno", Text = "Torno" });            
            lst.Add(new SelectListItem() { Value = "Decorado", Text = "Decorado" });
            lst.Add(new SelectListItem() { Value = "Terminado", Text = "Terminado" });
            lst.Add(new SelectListItem() { Value = "Pausado", Text = "Pausado" });

            return lst;
        }

        //Retorna un nivel mas al de la etapa actual.
        public List<SelectListItem> getEtapasDeProduccion(int EtapaActual)
        {
            List<SelectListItem> lst = new List<SelectListItem>();                        
            lst.Add(new SelectListItem() { Value = "0", Text = "Planeación" });

            if (EtapaActual  >= 0)
                lst.Add(new SelectListItem() { Value = "Torno", Text = "Torno" });
            if (EtapaActual  >= 1)
                lst.Add(new SelectListItem() { Value = "2", Text = "Secado" });
            if (EtapaActual >= 2)
                lst.Add(new SelectListItem() { Value = "3", Text = "Horno" });
            if (EtapaActual >= 3)
                lst.Add(new SelectListItem() { Value = "4", Text = "Decoración" });
            if (EtapaActual >= 4)
                lst.Add(new SelectListItem() { Value = "10", Text = "Terminado" });

            lst.Add(new SelectListItem() { Value = "11", Text = "Pausado" });

            return lst;
        }

        public List<PlanDeTrabajoConDetalle> getPlanesTrabajo()
        {
            try
            {
                List<PlanDeTrabajo> lst = db.PlanDeTrabajo.ToList();

                List<PlanDeTrabajoConDetalle> lstConDet = new List<PlanDeTrabajoConDetalle>();
                foreach(var item in lst)
                {
                    PlanDeTrabajoConDetalle tmp = new PlanDeTrabajoConDetalle()
                    {
                        Id = item.Id,
                        IdPersonal = item.IdPersonal,
                        NombrePersonal = db.PersonalTalavera.Where(n => n.Id == item.IdPersonal).Select(nn => nn.Nombre + " " + nn.APaterno).FirstOrDefault(),
                        NumeroOrden = item.NumeroOrden,
                        FechaInicio = item.FechaInicio,
                        FechaFin = item.FechaFin,
                        EtapaPlan = item.EtapaPlan,
                        Observacion = item.Observacion,
                        Detalles = db.MoldeadoMovimientos.Where(x => x.IdOrigen == item.Id).Select(y => new DetalleDePlan()
                                                                                                            {
                                                                                                                Id = y.Id,
                                                                                                                IdCatalogoTalavera = y.IdCatalogoTalavera,
                                                                                                                NombrePieza = db.CatalogoTalavera.Where(zz => zz.Id == y.IdCatalogoTalavera).Select(zzz => zzz.NombrePieza + " " + zzz.Altura + " x " + zzz.Diametro ).FirstOrDefault(),
                                                                                                                CatidadPlaneada = y.CatidadPlaneada,
                                                                                                                CantidadReal = y.CantidadReal,
                                                                                                                Observacion = y.Observacion                            
                                                                                                            }).ToList()
                        
                    };
                    lstConDet.Add(tmp);
                }

                return lstConDet;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int addPlanDeTrabajo(PlanDeTrabajo pPT)
        {
            try
            {
                db.PlanDeTrabajo.Add(pPT);
                int res = db.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        //MOVIMIENTOS DE PIEZAS

        public int addMoldeadoMovimientos(MoldeadoMovimientos pMM)
        {
            try
            {
                db.MoldeadoMovimientos.Add(pMM);
                int res = db.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public int addMoldeadoMovimientos(List<MoldeadoMovimientos> pMM)
        {
            try
            {
                db.MoldeadoMovimientos.AddRange(pMM);
                int res = db.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }


    }
}