using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgInfoProcedimientos
    {
        public String tipo { get; set; }
        public String idprocedimiento { get; set; }
        public String inicio { get; set; }
        public String final { get; set; }
        public String descripcion { get; set; }
        public String responsable { get; set; }
        public String usuario { get; set; }
        public String estado { get; set; }
    }
    public class msgInfoProcedimientosCreateResponse
    {
        public msgInfoProcedimientosCreateResponseItem[] resultSet1 { get; set; }
        public String updateCount1 { get; set; }
    }
    public class msgInfoProcedimientosCreateResponseItem
    {
        public String idProcedimiento { get; set; }
    }

    public class msgInfoProcedimientosQueryResponse
    {
        public msgInfoProcedimientosQueryResponseItem[] resultSet1 { get; set; }
    }

    public class msgInfoProcedimientosQueryResponseItem
    {
        public String idProcedimiento { get; set; }
        public String fechaInicio { get; set; }
        public String fechaFin { get; set; }
        public String descripcion { get; set; }
        public String idUsuarioResponsable { get; set; }
        public String estado { get; set; }
    }
}
