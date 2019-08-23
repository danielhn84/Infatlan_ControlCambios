using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgInfoMantenimientos
    {
        public String tipo { get; set; }
        public String idmantenimiento { get; set; }
        public String descripcion { get; set; }
        public String idtipocambio { get; set; }
        public String idlugar { get; set; }
        public String otros { get; set; }
        public String usuario { get; set; }
    }
    public class msgInfoMantenimientosCreateResponse
    {
        public msgInfoMantenimientosCreateResponseItem[] resultSet1 { get; set; }
        public String updateCount1 { get; set; }
    }
    public class msgInfoMantenimientosCreateResponseItem
    {
        public String idMantenimiento { get; set; }
    }

    public class msgInfoMantenimientosQueryResponse
    {
        public msgInfoMantenimientosQueryResponseItem[] resultSet1 { get; set; }
    }
    public class msgInfoMantenimientosQueryResponseItem
    {
        public String idMantenimiento { get; set; }
        public String descripcion { get; set; }
        public String idTipoCambio { get; set; }
        public String idTipoLugarMantenimiento { get; set; }
        public String otros { get; set; }
        public String idTipoMantenimiento { get; set; }
        public String idTipoMantenimientoSub { get; set; }

    }
}
