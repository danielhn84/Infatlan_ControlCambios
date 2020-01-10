using ControlCambios.classes;
using ControlCambios.messages;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlCambios
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "openInformacionModal();", true);
            }
            
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Generales vGenerales = new Generales();
                if (TxUsername.Text.Equals("") || TxPassword.Text.Equals(""))
                    throw new Exception("Por favor ingrese un usuario o password");


                LdapService vLdap = new LdapService();
                Boolean vLogin = vLdap.ValidateCredentials("ADBancat.hn", TxUsername.Text, TxPassword.Text);

                //Boolean vLogin = true; 

                if (vLogin || TxUsername.Text.Equals("admin") || TxUsername.Text.Equals("QA") || TxUsername.Text.Equals("SUP") || TxUsername.Text.Equals("CAB") || TxUsername.Text.Equals("IMP") || TxUsername.Text.Equals("PRO"))
                {
                    msgLogin vRequest = new msgLogin()
                    {
                        username = TxUsername.Text,
                        password = vGenerales.MD5Hash(TxPassword.Text)
                    };

                    String vResponseResult = "";
                    HttpService vConector = new HttpService();

                    //test vTest = vConector.getTest(vRequest);
                    HttpResponseMessage vHttpResponse = vConector.postLogin(vRequest, ref vResponseResult);
                    if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        msgLoginResponse vLoginResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgLoginResponse>(vResponseResult);
                        if (vLoginResponse.resultSet1.Count() > 0)
                        {
                            if (vLoginResponse.resultSet1[0].error.Equals("Success"))
                            {
                                Session["AUTHCLASS"] = vLoginResponse;
                                Session["AUTH"] = true;
                                Logs vLog = new Logs();
                                vLog.postLog("Login", "Usuario ingresado con exito", TxUsername.Text);

                                Response.Redirect("/default.aspx");
                            }
                            else if (vLoginResponse.resultSet1[0].error.Equals("Error"))
                            {
                                Session["AUTH"] = false;
                                Logs vLog = new Logs();
                                vLog.postLog("Login", "Intento fallido de ingreso", TxUsername.Text);

                                throw new Exception(vLoginResponse.resultSet1[0].mensaje);
                            }
                        }
                        else
                            throw new Exception("Usuario no existe o esta desactivado");
                    }
                }
                else
                    throw new Exception("Usuario o contraseña incorrecta");
                
            }
            catch (Exception Ex)
            {
                LbMensaje.Text = Ex.Message;
                TxPassword.Text = String.Empty;
            }
        }
    }
}