using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.messages
{
    public class msgInfoUsuarios
    {
        public String tipo { get; set; }
        public String usuario { get; set; }
        public String password { get; set; }
        public String nombres { get; set; }
        public String apellidos { get; set; }
        public String telefono { get; set; }
        public String correo { get; set; }
        public String idcargo { get; set; }
        public String estado { get; set; }
    }

    public class msgInfoUsuariosCreateResponse
    {
        public msgInfoUsuariosCreateResponseItem[] resultSet1 { get; set; }
    }

    public class msgInfoUsuariosCreateResponseItem
    {
        public String creado { get; set; }
    }

    public class msgInfoUsuariosOverallResponse
    {
        public msgInfoUsuariosOverallResponseItem[] resultSet1 { get; set; }
    }

    public class msgInfoUsuariosOverallResponseItem
    {
        public String idUsuario { get; set; }
        public String nombres { get; set; }
        public String apellidos { get; set; }
        public String telefono { get; set; }

    }

    public class msgInfoUsuariosQueryResponse
    {
        public msgInfoUsuariosQueryResponseItem[] resultSet1 { get; set; }
    }

    public class msgInfoUsuariosQueryResponseItem
    {
        public String usuario { get; set; }
        public String password { get; set; }
        public String nombres { get; set; }
        public String apellidos { get; set; }
        public String telefono { get; set; }
        public String correo { get; set; }
        public String idcargo { get; set; }
        public String estado { get; set; }
        public String fechaCreacion { get; set; }
    }
}   
