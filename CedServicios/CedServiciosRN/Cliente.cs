﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CedServicios.RN
{
    public class Cliente
    {
        public static List<Entidades.Cliente> ListaPorCuit(bool SoloVigentes, Entidades.Sesion Sesion)
        {
            DB.Cliente db = new DB.Cliente(Sesion);
            return db.ListaPorCuit(SoloVigentes);
        }
        public static List<Entidades.Cliente> ListaPorCuit(bool SoloVigentes, bool ParaCombo, Entidades.Sesion Sesion)
        {
            DB.Cliente db = new DB.Cliente(Sesion);
            return db.ListaPorCuit(SoloVigentes, ParaCombo);
        }
        public static List<Entidades.Cliente> ListaExportacion(Entidades.Usuario Cuenta, Entidades.Sesion Sesion, bool ConSeleccionarComprador)
        {
            DB.Cliente comprador = new DB.Cliente(Sesion);
            List<Entidades.Cliente> lista = comprador.ListaPorCuit(true);
            lista = lista.FindAll(delegate(Entidades.Cliente c)
            {
                return c.Documento.Tipo.Id.Equals("70") || c.RazonSocial.Equals("Seleccionar cliente");
            });
            return lista;
        }
        public static List<Entidades.Cliente> ListaSinExportacion(Entidades.Usuario Cuenta, Entidades.Sesion Sesion, bool ConSeleccionarComprador)
        {
            DB.Cliente comprador = new DB.Cliente(Sesion);
            List<Entidades.Cliente> lista = comprador.ListaPorCuit(true);
            lista = lista.FindAll(delegate(Entidades.Cliente c)
            {
                return !c.Documento.Tipo.Id.Equals("70") || c.RazonSocial.Equals("Seleccionar cliente");
            });
            return lista;
        }
        public static List<Entidades.Cliente> ListaPorCuityTipoyNroDoc(string Cuit, Entidades.Documento Documento, Entidades.Sesion Sesion)
        {
            DB.Cliente db = new DB.Cliente(Sesion);
            return db.ListaPorCuityTipoyNroDoc(Cuit, Documento);
        }
        public static List<Entidades.Cliente> ListaPorCuityRazonSocial(string Cuit, string Razonsocial, Entidades.Sesion Sesion)
        {
            DB.Cliente db = new DB.Cliente(Sesion);
            return db.ListaPorCuityRazonSocial(Cuit, Razonsocial);
        }
        public static List<Entidades.Cliente> ListaPorCuityIdCliente(string Cuit, string IdCliente, Entidades.Sesion Sesion)
        {
            DB.Cliente db = new DB.Cliente(Sesion);
            return db.ListaPorCuityIdCliente(Cuit, IdCliente);
        }
        public static void Leer(Entidades.Cliente cliente, Entidades.Sesion Sesion)
        {
            DB.Cliente comprador = new DB.Cliente(Sesion);
            comprador.Leer(cliente);
        }
        public static void Crear(Entidades.Cliente Cliente, Entidades.Sesion Sesion)
        {
            DB.Cliente db = new DB.Cliente(Sesion);
            Cliente.WF.Estado = "Vigente";
            db.Crear(Cliente);
        }
        public static void DesambiguarClienteNacional(Entidades.Cliente Cliente, Entidades.Sesion Sesion)
        {
            DB.Cliente db = new DB.Cliente(Sesion);
            db.DesambiguarClienteNacional(Cliente);
        }
        public static void Modificar(Entidades.Cliente ClienteDesde, Entidades.Cliente ClienteHasta, Entidades.Sesion Sesion)
        {
            DB.Cliente db = new DB.Cliente(Sesion);
            db.Modificar(ClienteDesde, ClienteHasta);
        }
        public static void CambiarEstado(Entidades.Cliente Cliente, string Estado, Entidades.Sesion Sesion)
        {
            DB.Cliente db = new DB.Cliente(Sesion);
            db.CambiarEstado(Cliente, Estado);
        }
        public static Entidades.Cliente ObternerCopia(Entidades.Cliente Desde)
        {
            Entidades.Cliente hasta = new Entidades.Cliente();
            hasta.Contacto.Nombre = Desde.Contacto.Nombre;
            hasta.Contacto.Telefono = Desde.Contacto.Telefono;
            hasta.Contacto.Email = Desde.Contacto.Email;
            hasta.Cuit = Desde.Cuit;
            hasta.IdCliente = Desde.IdCliente;
            hasta.DesambiguacionCuitPais = Desde.DesambiguacionCuitPais;
            hasta.Documento.Tipo.Id = Desde.Documento.Tipo.Id;
            hasta.Documento.Tipo.Descr = Desde.Documento.Tipo.Descr;
            hasta.Documento.Nro = Desde.Documento.Nro;
            hasta.DatosIdentificatorios.GLN = Desde.DatosIdentificatorios.GLN;
            hasta.DatosIdentificatorios.CodigoInterno = Desde.DatosIdentificatorios.CodigoInterno;
            hasta.DatosImpositivos.DescrCondIngBrutos = Desde.DatosImpositivos.DescrCondIngBrutos;
            hasta.DatosImpositivos.DescrCondIVA = Desde.DatosImpositivos.DescrCondIVA;
            hasta.DatosImpositivos.FechaInicioActividades = Desde.DatosImpositivos.FechaInicioActividades;
            hasta.DatosImpositivos.IdCondIngBrutos = Desde.DatosImpositivos.IdCondIngBrutos;
            hasta.DatosImpositivos.IdCondIVA = Desde.DatosImpositivos.IdCondIVA;
            hasta.DatosImpositivos.NroIngBrutos = Desde.DatosImpositivos.NroIngBrutos;
            hasta.Domicilio.Calle = Desde.Domicilio.Calle;
            hasta.Domicilio.CodPost = Desde.Domicilio.CodPost;
            hasta.Domicilio.Depto = Desde.Domicilio.Depto;
            hasta.Domicilio.Localidad = Desde.Domicilio.Localidad;
            hasta.Domicilio.Manzana = Desde.Domicilio.Manzana;
            hasta.Domicilio.Nro = Desde.Domicilio.Nro;
            hasta.Domicilio.Piso = Desde.Domicilio.Piso;
            hasta.Domicilio.Provincia.Id = Desde.Domicilio.Provincia.Id;
            hasta.Domicilio.Provincia.Descr = Desde.Domicilio.Provincia.Descr;
            hasta.Domicilio.Sector = Desde.Domicilio.Sector;
            hasta.Domicilio.Torre = Desde.Domicilio.Torre;
            hasta.EmailAvisoVisualizacion = Desde.EmailAvisoVisualizacion;
            hasta.PasswordAvisoVisualizacion = Desde.PasswordAvisoVisualizacion;
            hasta.RazonSocial = Desde.RazonSocial;
            hasta.UltActualiz = Desde.UltActualiz;
            hasta.WF.Id = Desde.WF.Id;
            hasta.WF.Estado = Desde.WF.Estado;
            return hasta;
        }
        public static List<Entidades.Cliente> ListaSegunFiltros(string Cuit, string RazSoc, string NroDoc, string Estado, Entidades.Sesion Sesion)
        {
            DB.Cliente cliente = new DB.Cliente(Sesion);
            return cliente.ListaSegunFiltros(Cuit, RazSoc, NroDoc, Estado);
        }
        public static List<Entidades.Cliente> ListaPaging(out int CantidadFilas, int IndicePagina, string OrderBy, string Cuit, string NroDoc, string RazSoc, string Estado, string SessionID, Entidades.Sesion Sesion)
        {
            List<Entidades.Cliente> listaCliente = new List<Entidades.Cliente>();
            DB.Cliente db = new DB.Cliente(Sesion);
            if (OrderBy.Equals(String.Empty))
            {
                OrderBy = "Cuit desc, RazonSocial asc ";
            }
            listaCliente = db.ListaSegunFiltros(Cuit, NroDoc, RazSoc, Estado);
            int cantidadFilas = listaCliente.Count;
            CantidadFilas = cantidadFilas;
            return db.ListaPaging(IndicePagina, OrderBy, SessionID, listaCliente);
        }
    }
}