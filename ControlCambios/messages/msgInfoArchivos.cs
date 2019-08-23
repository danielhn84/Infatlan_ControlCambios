using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgInfoArchivos
    {
        public String tipo { get; set; }
        public String idcambio { get; set; }
        public String deposito1 { get; set; }
        public String deposito2 { get; set; }
        public String deposito3 { get; set; }
        public String usuario { get; set; }
    }

    public class msgInfoArchivosQueryResponse
    {
        public msgInfoArchivosQueryResponseItem[] resultSet1 { get; set; }
    }

    public class msgInfoArchivosQueryResponseItem
    {
        public String idCambio { get; set; }
        public String deposito1 { get; set; }
        public String deposito2 { get; set; }
        public String deposito3 { get; set; }
    }
}

