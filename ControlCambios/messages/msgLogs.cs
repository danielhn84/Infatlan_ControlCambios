using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgLogs
    {
        public String tipo { get; set; }
        public String lugar { get; set; }
        public String mensaje { get; set; }
        public String usuario { get; set; }
    }
    public class msgLogsQueryResponse
    {
        public msgLogsQueryResponseItem[] resultSet1 { get; set; }
    }

    public class msgLogsQueryResponseItem
    {
        public String fechaCreacion { get; set; }
        public String usuario { get; set; }
        public String mensaje { get; set; }
        public String lugar { get; set; }
        public String idLog { get; set; }
    }
}