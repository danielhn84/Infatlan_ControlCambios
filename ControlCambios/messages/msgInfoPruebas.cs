using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgInfoPruebas
    {
        public String tipo { get; set; }
        public String idprueba { get; set; }
        public String descripcion { get; set; }
        public String responsable { get; set; }
        public String usuario { get; set; }
        public String estado { get; set; }
    }

    public class msgInfoPruebasCreateResponse
    {
        public msgInfoPruebasCreateResponseItem[] resultSet1 { get; set; }
        public String updateCount1 { get; set; }
    }
    public class msgInfoPruebasCreateResponseItem
    {
        public String idPrueba { get; set; }
    }
    public class msgInfoPruebasQueryResponse
    {
        public msgInfoPruebasQueryResponseItem[] resultSet1 { get; set; }
    }
    public class msgInfoPruebasQueryResponseItem
    {
        public String idPrueba { get; set; }
        public String descripcion { get; set; }
        public String idUsuarioResponsable { get; set; }
        public String estado { get; set; }
    }
}

