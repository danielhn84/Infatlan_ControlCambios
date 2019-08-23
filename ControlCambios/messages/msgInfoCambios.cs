using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgInfoCambios
    {
        public String tipo { get; set; }
        public String idcambio { get; set; }
        public String nombre { get; set; }
        public String proveedor { get; set; }
        public String responsable { get; set; }
        public String idresolucion { get; set; }
        public String idcriticidad { get; set; }
        public String idimpacto { get; set; }
        public String idriesgo { get; set; }
        public String observaciones { get; set; }
        public String usuario { get; set; }
        public String idmantenimiento { get; set; }
        public String idcalendario { get; set; }
        public String usuariogrud { get; set; }

    }
    public class msgInfoCambiosCreateResponse
    {
        public msgInfoCambiosCreateResponseItem[] resultSet1 { get; set; }
        public String updateCount1 { get; set; }
    }
    public class msgInfoCambiosCreateResponseItem
    {
        public String idCambio { get; set; }
    }

    public class msgInfoCambiosQueryResponse
    {
        public msgInfoCambiosQueryResponseItem[] resultSet1 { get; set; }
    }

    public class msgInfoCambiosQueryResponseItem
    {
        public String idcambio { get; set; }
        public String mantenimientoNombre { get; set; }
        public String idProveedorServicio { get; set; }
        public String fechaSolicitud { get; set; }
        public String idUsuarioResponsable { get; set; }
        public String idResolucion { get; set; }
        public String idCriticidad { get; set; }
        public String idImpacto { get; set; }
        public String idRiesgo { get; set; }
        public String observaciones { get; set; }
        public String idUsuarioSolicitante { get; set; }
        public String idMantenimiento { get; set; }
        public String idCalendario { get; set; }
        public String pasos { get; set; }
        public String fechaQAInicio { get; set; }
        public String fechaQAFinal { get; set; }
    }
}

