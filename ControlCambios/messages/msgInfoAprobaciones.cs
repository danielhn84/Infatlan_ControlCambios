using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgInfoAprobaciones
    {
        public String tipo { get; set; }
        public String idaprobacion { get; set; }
        public String aprobador { get; set; }
        public String usuario { get; set; }
        public String estado { get; set; }
    }

    public class msgInfoAprobacionesCreateResponse
    {
        public msgInfoAprobacionesCreateResponseItem[] resultSet1 { get; set; }
        public String updateCount1 { get; set; }
    }
    public class msgInfoAprobacionesCreateResponseItem
    {
        public String idAprobacion { get; set; }
    }

    public class msgInfoAprobacionesQueryResponse
    {
        public msgInfoAprobacionesQueryResponseItem[] resultSet1 { get; set; }
    }

    public class msgInfoAprobacionesQueryResponseItem
    {
        public String fechaCreacion { get; set; }
        public String idUsuarioAprobador { get; set; }
        public String idAprobacion { get; set; }
        public String idUsuarioCreacion { get; set; }
        public String estado { get; set; }
    }
}