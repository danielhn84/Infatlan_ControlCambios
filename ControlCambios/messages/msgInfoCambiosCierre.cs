using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgInfoCambiosCierre
    {
        public String tipo { get; set; }
        public String idcambio { get; set; }
        public String observaciones { get; set; }
        public String fechavenini { get; set; }
        public String fechavenfin { get; set; }
        public String fechavendenini { get; set; }
        public String fechavendenfin { get; set; }
        public String impacto { get; set; }
        public String rollback { get; set; }
        public String fecharollini { get; set; }
        public String fecharollfin { get; set; }
        public String fecharolldenini { get; set; }
        public String fecharolldenfin { get; set; }
        public String usuario { get; set; }
        public String resultado { get; set; }
        
    }

    public class msgInfoCambiosCierreQueryResponse
    {
        public msgInfoCambiosCierreQueryResponseItem[] resultSet1 { get; set; }
    }

    public class msgInfoCambiosCierreQueryResponseItem
    {
        public String fechaRollbackInicio { get; set; }
        public String idCambio { get; set; }
        public String idUsuarioCierre { get; set; }
        public String fechaVentanaDenegacionFin { get; set; }
        public String observaciones { get; set; }
        public String rollback { get; set; }
        public String fechaCierre { get; set; }
        public String fechaVentanaInicio { get; set; }
        public String impacto { get; set; }
        public String fechaVentanaFin { get; set; }
        public String fechaRollbackFin { get; set; }
        public String fechaRollbackDenegacionInicio { get; set; }
        public String fechaVentanaDenegacionInicio { get; set; }
        public String fechaRollbackDenegacionFin { get; set; }
        public String resultado { get; set; }
        
    }
}
