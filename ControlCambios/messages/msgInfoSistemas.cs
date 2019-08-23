using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgInfoSistemas
    {
        public String tipo { get; set; }
        public String idcanal { get; set; }
        public String nombre { get; set; }
        public String descripcion { get; set; }
        public String denegacion { get; set; }
        public String inicio { get; set; }
        public String fin { get; set; }
        public String usuario { get; set; }
    }

    public class msgInfoSistemasCreateResponse
    {
        public msgInfoSistemasCreateResponseItem[] resultSet1 { get; set; }
        public String updateCount1 { get; set; }
    }

    public class msgInfoSistemasCreateResponseItem
    {
        public String idCanal { get; set; }
    }

    public class msgInfoSistemasQueryResponse
    {
        public msgInfoSistemasQueryResponseItem[] resultSet1 { get; set; }
    }
    public class msgInfoSistemasQueryResponseItem
    {
        public String idCanal { get; set; }
        public String nombreSistema { get; set; }
        public String descripcion { get; set; }
        public String denegacion { get; set; }
        public String fechaInicio { get; set; }
        public String fechaFinal { get; set; }
    }
}