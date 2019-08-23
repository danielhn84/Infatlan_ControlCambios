using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgInfoComunicaciones
    {
        public String tipo { get; set; }
        public String idcomunicacion { get; set; }
        public String cambionormal { get; set; }
        public String casoincidente { get; set; }
        public String usuario { get; set; }
    }
    public class msgInfoComunicacionesCreateResponse
    {
        public msgInfoComunicacionesCreateResponseItem[] resultSet1 { get; set; }
        public String updateCount1 { get; set; }
    }
    public class msgInfoComunicacionesCreateResponseItem
    {
        public String idComunicacion { get; set; }
    }

    public class msgInfoComunicacionesQueryResponse
    {
        public msgInfoComunicacionesQueryResponseItem[] resultSet1 { get; set; }
    }
    public class msgInfoComunicacionesQueryResponseItem
    {
        public String idComunicacion { get; set; }
        public String cambioNormal { get; set; }
        public String casoIncidente { get; set; }
    }
}

