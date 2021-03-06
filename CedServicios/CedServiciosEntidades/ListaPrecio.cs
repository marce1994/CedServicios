﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CedServicios.Entidades
{
    [Serializable]
    public class ListaPrecio
    {
        private string cuit;
        private string id;
        private string descr;
        private WF wF;
        private string ultActualiz;
        private int orden;
        private string idTipo;

        public ListaPrecio()
        {
            wF = new WF();
        }
        public ListaPrecio(string IdListaPrecio, string DescrListaPrecio)
        {
            id = IdListaPrecio;
            descr = DescrListaPrecio;
            wF = new WF();
        }
        public ListaPrecio(string IdListaPrecio)
        {
            id = IdListaPrecio;
            wF = new WF();
        }

        public string Cuit
        {
            set
            {
                cuit = value;
            }
            get
            {
                return cuit;
            }
        }
        public string Id
        {
            set
            {
                id = value;
            }
            get
            {
                return id;
            }
        }
        public string Descr
        {
            set
            {
                descr = value;
            }
            get
            {
                return descr;
            }
        }
        public WF WF
        {
            set
            {
                wF = value;
            }
            get
            {
                return wF;
            }
        }
        public string UltActualiz
        {
            set
            {
                ultActualiz = value;
            }
            get
            {
                return ultActualiz;
            }
        }
        public int Orden
        {
            set
            {
                orden = value;
            }
            get
            {
                return orden;
            }
        }
        public string IdTipo
        {
            set
            {
                idTipo = value;
            }
            get
            {
                return idTipo;
            }
        }
        #region Propiedades redundantes
        public string Estado
        {
            get
            {
                return wF.Estado;
            }
        }
        #endregion
    }
}