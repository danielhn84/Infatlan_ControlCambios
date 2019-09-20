using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;

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
                        case "7":
                            vActual = Permisos.SupervisorQA;
                            break;
                    }

                    if (vTieneAcceso.Equals(vActual))
                        vResultado = true;
                }
            }
            catch (Exception Ex) { throw Ex; }
            return vResultado;
        }

        public string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }

    public enum Permisos
    {
        Administrador,
        Supervisor,
        QualityAssurance,
        CABManager,
        Implementador,
        Promotor,
        SupervisorQA
    }
}