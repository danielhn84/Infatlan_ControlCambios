using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgLogin
    {
        public String username { get; set; }
        public String password { get; set; }
    }
    public class msgLoginResponse
    {
        public msgLoginResponseItem[] resultSet1 { get; set; }
    }
    public class msgLoginResponseItem
    {
        public String error { get; set; }
        public String mensaje { get; set; }
        public String idUsuario { get; set; }
        public String password { get; set; }
        public String nombres { get; set; }
        public String apellidos { get; set; }
        public String telefono { get; set; }
        public String correo { get; set; }
        public String idCargo { get; set; }
        public String estado { get; set; }

    }
}