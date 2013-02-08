﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CedServicios.RN
{
    public class Sesion
    {
        public static void Cerrar(Entidades.Sesion Sesion)
        {
            Sesion.Usuario = new Entidades.Usuario();
            Sesion.Cuit = new Entidades.Cuit();
            Sesion.UN = new Entidades.UN();
            Sesion.CuitsDelUsuario = new List<Entidades.Cuit>();
            Sesion.UNsDelCuit = new List<Entidades.UN>();
        }
        public static List<string> OpcionesHabilitadas(Entidades.Sesion Sesion)
        {
            List<string> opcionesHabilitadas = new List<string>();
            opcionesHabilitadas.Add("CUIT/Alta de CUIT");
            opcionesHabilitadas.Add("CUIT/Solicitud permiso de administrador de CUIT");
            opcionesHabilitadas.Add("Unidad de Negocio/Alta de UN (sobre un CUIT pre-existente)");
            opcionesHabilitadas.Add("Unidad de Negocio/Solicitud permiso de operador de servicio de una UN existente");
            opcionesHabilitadas.Add("Unidad de Negocio/Solicitud permiso de administrador de UN");
            opcionesHabilitadas.Add("Configuración");
            opcionesHabilitadas.Add("Cerrar sesión");

            List<Entidades.Permiso> permisoAdminSITEVigente = Sesion.Usuario.Permisos.FindAll(delegate(Entidades.Permiso p)
			{
				return p.TipoPermiso.Id=="AdminSITE" && p.WF.Estado=="Vigente";
			});
            if (permisoAdminSITEVigente != null)
            {
                opcionesHabilitadas.Add("Administración Site/Explorador de Usuarios");
                opcionesHabilitadas.Add("Administración Site/Explorador de CUITs");
                opcionesHabilitadas.Add("Administración Site/Explorador de UNs");
                opcionesHabilitadas.Add("Administración Site/Explorador de Puntos de Venta");
                opcionesHabilitadas.Add("Administración Site/Explorador de Clientes");
                opcionesHabilitadas.Add("Administración Site/Explorador de Artículos");
                opcionesHabilitadas.Add("Administración Site/Explorador de Permisos");
                opcionesHabilitadas.Add("Administración Site/Explorador de Configuraciones");
                opcionesHabilitadas.Add("Administración Site/Explorador de Logs");
            }
            if (Sesion.Cuit.Nro != null)
            {
                opcionesHabilitadas.Add("Clientes");
                List<Entidades.Permiso> esAdminCUITdeCUITseleccionado = Sesion.Usuario.Permisos.FindAll(delegate(Entidades.Permiso p)
                {
                    return p.TipoPermiso.Id == "AdminCUIT" && p.Cuit == Sesion.Cuit.Nro && p.WF.Estado == "Vigente";
                });
                if (esAdminCUITdeCUITseleccionado != null)
                {
                    opcionesHabilitadas.Add("CUIT/Modificación datos CUIT");
                    opcionesHabilitadas.Add("Puntos de Venta");
                }
            }
            List<Entidades.Permiso> esAutorizador = Sesion.Usuario.Permisos.FindAll(delegate(Entidades.Permiso p)
            {
                return (p.TipoPermiso.Id == "AdminCUIT" || p.TipoPermiso.Id == "AdminUN" || p.TipoPermiso.Id == "AdminSITE") && p.WF.Estado == "Vigente";
            });
            if (esAutorizador != null)
            {
                opcionesHabilitadas.Add("Autorizaciones/Explorador de Autorizaciones");
                if (RN.Permiso.LeerListaPermisosPteAutoriz(Sesion.Usuario, Sesion).Count != 0)
                {
                    opcionesHabilitadas.Add("Autorizaciones/Explorador de Autorizaciones pendientes");
                }
            }
            if (Sesion.UN.Id != null)
            {
                List<Entidades.Permiso> elUsuarioEsAdministradorDeLaUNSeleccionada = Sesion.Usuario.Permisos.FindAll(delegate(Entidades.Permiso p)
                {
                    return p.TipoPermiso.Id == "AdminUN" && p.IdUN == Sesion.UN.Id && p.Cuit == Sesion.UN.Cuit && p.WF.Estado == "Vigente";
                });
                if (elUsuarioEsAdministradorDeLaUNSeleccionada != null)
                {
                    opcionesHabilitadas.Add("Unidad de Negocio/Modificación datos UN");
                }
                List<Entidades.Permiso> elUsuarioTieneHabilitadoElServicioEFACTParaLaUNSeleccionada = Sesion.Usuario.Permisos.FindAll(delegate(Entidades.Permiso p)
                {
                    return p.TipoPermiso.Id == "eFact" && p.IdUN == Sesion.UN.Id && p.Cuit == Sesion.UN.Cuit && p.WF.Estado == "Vigente";
                });
                //Ojo: no estoy chequeando que la UN siga teniendo el permiso vigente sobre el servicio eFact !!!
                if (elUsuarioTieneHabilitadoElServicioEFACTParaLaUNSeleccionada != null)
                {
                    opcionesHabilitadas.Add("Facturación");
                    opcionesHabilitadas.Add("Artículos");
                }
            }
            return opcionesHabilitadas;
        }
        public static void LeerDatosUsuario(Entidades.Sesion Sesion)
        {
            if (Sesion.Usuario.Id != String.Empty)
            {
                Sesion.Usuario.Permisos = RN.Permiso.LeerListaPermisosVigentesPorUsuario(Sesion.Usuario, Sesion);
                Sesion.CuitsDelUsuario = RN.Cuit.LeerListaCuitsPorUsuario(Sesion);
                if (Sesion.CuitsDelUsuario.Count != 0) 
                { 
                    Sesion.Cuit = Sesion.CuitsDelUsuario[0];
                    Sesion.UNsDelCuit = RN.UN.LeerListaUNsPorCuit(Sesion);
                    if (Sesion.UNsDelCuit.Count != 0)
                    {
                        Sesion.UN = Sesion.UNsDelCuit[0];
                    }
                }
                Sesion.OpcionesHabilitadas = OpcionesHabilitadas(Sesion);
            }
        }
        public static void ArmarListaDeOpcionesHabilitadas(Entidades.Sesion Sesion)
        {
            if (Sesion.Usuario.Id!=String.Empty)
            {
            }
        }
    }
}