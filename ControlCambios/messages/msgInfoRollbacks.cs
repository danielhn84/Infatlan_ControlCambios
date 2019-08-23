using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgInfoRollbacks
    {
        public String tipo { get; set; }
        public String idrollback { get; set; }
        public String inicio { get; set; }
        public String final { get; set; }
        public String descripcion { get; set; }
        public String responsable { get; set; }
        public String usuario { get; set; }
        public String estado { get; set; }
    }
    public class msgInfoRollbacksCreateResponse
    {
        public msgInfoRollbacksCreateResponseItem[] resultSet1 { get; set; }
        public String updateCount1 { get; set; }
    }
    public class msgInfoRollbacksCreateResponseItem
    {
        public String idRollback { get; set; }
    }

    public class msgInfoRollbacksQueryResponse
    {
        public msgInfoRollbacksQueryResponseItem[] resultSet1 { get; set; }

    }
    public class msgInfoRollbacksQueryResponseItem
    {
        public String idRollback { get; set; }
        public String fechaInicio { get; set; }
        public String fechaFin { get; set; }
        public String descripcion { get; set; }
        public String idUsuarioResponsable { get; set; }
        public String estado { get; set; }
    }
}
