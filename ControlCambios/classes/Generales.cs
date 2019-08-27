using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlCambios.classes
{
    public class Generales
    {
        public Boolean PermisosEntrada(Permisos vTieneAcceso, String vPermisoActual)
        {
            Boolean vResultado = false;
            try
            {
                if (vPermisoActual.Equals("1"))
                {
                    vResultado = true;
                }
                else
                {
                    Permisos vActual = new Permisos();
                    switch (vPermisoActual)
                    {
                        case "1":
                            vActual = Permisos.Administrador;
                            break;
                        case "2":
                            vActual = Permisos.Supervisor;
                            break;
                        case "3":
                            vActual = Permisos.QualityAssurance;
                            break;
                        case "4":
                            vActual = Permisos.Implementador;
                            break;
                        case "5":
                            vActual = Permisos.Promotor;
                            break;
                        case "6":
                            vActual = Permisos.CABManager;
                            break;
                    }

                    if (vTieneAcceso.Equals(vActual))
                        vResultado = true;
                }
            }
            catch (Exception Ex) { throw Ex; }
            return vResultado;
        }
    }

    public enum Permisos
    {
        Administrador,
        Supervisor,
        QualityAssurance,
        CABManager,
        Implementador,
        Promotor
    }
}