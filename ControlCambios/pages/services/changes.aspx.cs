using ControlCambios.classes;
using ControlCambios.messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ControlCambios.pages.services
{
    public enum WarningType
    {
        Success,
        Info,
        Warning,
        Danger
    }
    public partial class changes : System.Web.UI.Page
    {
        msgLoginResponse vConfigurations = null;
        // PASO 1
        DataTable vDatosEquipos = new DataTable();
        DataTable vDatosSistemas = new DataTable();
        DataTable vDatosPersonal = new DataTable();
        DataTable vDatosComunicaciones = new DataTable();

        //PASO 1
        DataTable vDatosProcedimientos = new DataTable();
        DataTable vDatosRollback = new DataTable();
        DataTable vDatosPruebas = new DataTable();

        //PASO 3
        DataTable vDatosCabImplementadores = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            String GETIDCAMBIO = Request.QueryString["id"];
            if (!Page.IsPostBack)
            {
                LimpiarSessiones();
                if (Convert.ToBoolean(Session["AUTH"]))
                {
                    vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                    Session["USERTYPE"] = vConfigurations.resultSet1[0].idCargo;
                    //Cargas
                    CargarCatalogoEquipos();
                    CargarCatalogoSistemas();
                    CargarCatalogoUsuarios();
                    CargarCatalogoProveedores();
                    CargarTipoMantenimiento();
                    //Configuraciones
                    CargarSessionesPasos();


                    Session["CIERRE"] = false;
                    if (GETIDCAMBIO != null)
                    {
                        HttpService vConector = new HttpService();
                        msgInfoCambios vInfoCambiosRequest = new msgInfoCambios()
                        {
                            tipo = "3",
                            idcambio = GETIDCAMBIO,
                            usuariogrud = vConfigurations.resultSet1[0].idUsuario
                        };
                        String vResponseCambios = "";
                        HttpResponseMessage vHttpResponseCambios = vConector.PostInfoCambios(vInfoCambiosRequest, ref vResponseCambios);

                        if (vHttpResponseCambios.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            msgInfoCambiosQueryResponse vInfoCambiosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseCambios);
                            if (vInfoCambiosResponse.resultSet1.Count() > 0)
                            {
                                foreach (msgInfoCambiosQueryResponseItem item in vInfoCambiosResponse.resultSet1)
                                {


                                    LbInfoNumeroCambio.Text = "No. " + item.idcambio;


                                    msgInfoUsuarios vRequest = new msgInfoUsuarios()
                                    {
                                        tipo = "2",
                                        usuario = item.idUsuarioSolicitante
                                    };

                                    String vResponseResult = "";
                                    HttpResponseMessage vHttpResponse = vConector.PostInfoUsuarios(vRequest, ref vResponseResult);
                                    if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                                    {
                                        msgInfoUsuariosQueryResponse vUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoUsuariosQueryResponse>(vResponseResult);
                                        foreach (msgInfoUsuariosQueryResponseItem itemUsuarios in vUsuariosResponse.resultSet1)
                                        {
                                            LbInfoCreadoPor.Text = "Creado por <b>" + itemUsuarios.nombres + " " + itemUsuarios.apellidos + "</b>";
                                            LbInfoTelefono.Text = "Telefono <b>" + itemUsuarios.telefono + "</b>";
                                        }
                                    }

                                    if (item.autorizarQA != null)
                                    {
                                        if (item.autorizarQA.Equals("true"))
                                        {
                                            BtnAutorizarQA.Visible = false;
                                        }
                                        else
                                            BtnAutorizarQA.Visible = true;
                                    }
                                    else
                                        BtnAutorizarQA.Visible = true;

                                    if (item.pasos != null)
                                    {
                                        if (item.pasos.Equals("0"))
                                        {
                                            BtnAutorizarQA.Visible = false;
                                            BtnAsignarUsuario.Text = "Asignar QA";
                                            BtnAsignarUsuario.CssClass = "btn btn-light bg-white mr-2 ";
                                        }
                                        if (item.pasos.Equals("1"))
                                        {
                                            NecesitaAutorizacion(vConector, item);
                                            if (vConfigurations.resultSet1[0].idCargo.Equals("1") || vConfigurations.resultSet1[0].idCargo.Equals("4"))
                                            {
                                                BtnGuardarCambio.Text = "Modificar";
                                                BtnAsignarUsuario.Visible = true;
                                                Mensaje("Permisos de Edición", WarningType.Success);
                                            }
                                            else
                                            {
                                                DeshabilitarPaso1();
                                                
                                            }

                                            LIPaso2.Visible = true;
                                            CargarrQADatosPromotor(item.idcambio);

                                        }
                                        if (item.pasos.Equals("2"))
                                        {
                                            NecesitaAutorizacion(vConector, item);
                                            LIPaso2.Visible = true;
                                            LIPaso3.Visible = true;
                                            {
                                                DeshabilitarPaso1();
                                                DeshabilitarPaso2(item.idcambio);
                                            }
                                        }
                                        if (item.pasos.Equals("3"))
                                        {
                                            NecesitaAutorizacion(vConector, item);

                                            LIPaso2.Visible = true;
                                            LIPaso3.Visible = true;
                                            LIPaso4.Visible = true;
                                            DeshabilitarPaso1();
                                            DeshabilitarPaso2(item.idcambio);
                                            DeshabilitarPaso3(item);
                                        }

                                        if (item.pasos.Equals("4"))
                                        {
                                            NecesitaAutorizacion(vConector, item);
                                            LIPaso2.Visible = true;
                                            LIPaso3.Visible = true;
                                            LIPaso4.Visible = true;
                                            LIPaso5.Visible = true;
                                            DeshabilitarPaso1();
                                            DeshabilitarPaso2(item.idcambio);
                                            DeshabilitarPaso3(item);
                                            DeshabilitarPaso4();

                                            Session["CIERRE"] = true;
                                        }
                                        if (item.pasos.Equals("5"))
                                        {
                                            NecesitaAutorizacion(vConector, item);
                                            LIPaso2.Visible = true;
                                            LIPaso3.Visible = true;
                                            LIPaso4.Visible = true;
                                            LIPaso5.Visible = true;
                                            LIPaso6.Visible = true;
                                            DeshabilitarPaso1();
                                            DeshabilitarPaso2(item.idcambio);
                                            DeshabilitarPaso3(item);
                                            DeshabilitarPaso4();
                                            DeshabilitarPaso5(GETIDCAMBIO, vConector);
                                        }
                                        if (item.pasos.Equals("6"))
                                        {
                                            NecesitaAutorizacion(vConector, item);
                                            LIPaso2.Visible = true;
                                            LIPaso3.Visible = true;
                                            LIPaso4.Visible = true;
                                            LIPaso5.Visible = true;
                                            LIPaso6.Visible = true;
                                            DeshabilitarPaso1();
                                            DeshabilitarPaso2(item.idcambio);
                                            DeshabilitarPaso3(item);
                                            DeshabilitarPaso4();
                                            DeshabilitarPaso5(GETIDCAMBIO, vConector);
                                            DeshabilitarPaso6(item.idcambio);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                GETIDCAMBIO = null;
                                Response.Redirect("/pages/services/changes.aspx");
                            }
                        }
                    }

                    if (GETIDCAMBIO != null)
                    {
                        DescargaArchivos(true);
                        CargarInformacionCambio(GETIDCAMBIO);
                        Session["GETIDCAMBIO"] = GETIDCAMBIO;
                        Session["MANTENIMIENTO"] = true;
                        CargarCambio(GETIDCAMBIO);
                    }
                    else
                    {
                        DescargaArchivos(false);
                        Session["GETIDCAMBIO"] = null;
                        Session["MANTENIMIENTO"] = false;

                        //TxVentanaDuracionInicio.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-ddT00:00");

                        try
                        {
                            Generales vGenerales = new Generales();
                            if (!vGenerales.PermisosEntrada(Permisos.Promotor, vConfigurations.resultSet1[0].idCargo))
                            {
                                if (vGenerales.PermisosEntrada(Permisos.Implementador, vConfigurations.resultSet1[0].idCargo))
                                {
                                    Mensaje("Tienes permiso de creación", WarningType.Success);
                                }
                                else
                                {
                                    BtnGuardarCambio.Enabled = false;
                                    BtnGuardarCambio.Text = "No eres promotor";
                                    BtnGuardarCambio.CssClass = "btn btn-primary mr-2";
                                }
                            }
                        }
                        catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }

                    }
                }
                else
                    Response.Redirect("/login.aspx"); 
            }
        }

        private void NecesitaAutorizacion(HttpService vConector, msgInfoCambiosQueryResponseItem item)
        {
            msgInfoAprobaciones vInfoAprobacionesRowRequest = new msgInfoAprobaciones()
            {
                tipo = "3",
                idaprobacion = item.idcambio
            };
            String vResponseRowAprobaciones = "";
            HttpResponseMessage vHttpResponseRowAprobaciones = vConector.PostInfoAprobaciones(vInfoAprobacionesRowRequest, ref vResponseRowAprobaciones);

            if (vHttpResponseRowAprobaciones.StatusCode == System.Net.HttpStatusCode.OK)
            {
                msgInfoAprobacionesQueryResponse vInfoAprobacionesRowsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoAprobacionesQueryResponse>(vResponseRowAprobaciones);
                if (vInfoAprobacionesRowsResponse.resultSet1.Count() > 0)
                {

                    if (vInfoAprobacionesRowsResponse.resultSet1[0].estado.Equals("false"))
                    {
                        BtnAutorizarCambio.Visible = true;
                    }
                }
            }
        }

        private void CargarrQADatosPromotor(String vCambio)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                msgInfoCambios vInfoCambiosRequest = new msgInfoCambios()
                {
                    tipo = "3",
                    idcambio = vCambio,
                    usuariogrud = vConfigurations.resultSet1[0].idUsuario
                };
                String vResponseCambios = "";
                HttpResponseMessage vHttpResponseCambios = vConector.PostInfoCambios(vInfoCambiosRequest, ref vResponseCambios);

                if (vHttpResponseCambios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoCambiosQueryResponse vInfoCambiosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseCambios);
                    if (vInfoCambiosResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoCambiosQueryResponseItem itemArchivos in vInfoCambiosResponse.resultSet1)
                        {
                            CargarInformacionCriticidadQA(itemArchivos.idCriticidad);
                            CargarInformacionImpactoQA(itemArchivos.idImpacto);
                            CargarInformacionRiesgoQA(itemArchivos.idRiesgo);

                            msgInfoCalendarios vRequestCalendarios = new msgInfoCalendarios()
                            {
                                tipo = "3",
                                idcalendario = vCambio
                            };

                            String vResponseInfoCalendarios = "";
                            HttpResponseMessage vHttpResponseInfoCalendarios = vConector.PostInfoCalendarios(vRequestCalendarios, ref vResponseInfoCalendarios);
                            if (vHttpResponseInfoCalendarios.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                msgInfoCalendariosQueryResponse vInfoCalendariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCalendariosQueryResponse>(vResponseInfoCalendarios);
                                if (vInfoCalendariosResponse.resultSet1.Count() > 0)
                                {
                                    foreach (msgInfoCalendariosQueryResponseItem itemCalendario in vInfoCalendariosResponse.resultSet1)
                                    {
                                        TxHorarioInicioPromotor.Text = Convert.ToDateTime(itemCalendario.horaVentanaInicio).ToString("yyyy-MM-ddTHH:mm:ss.ss");
                                        TxHorarioFinalPromotor.Text = Convert.ToDateTime(itemCalendario.horaVentanaFin).ToString("yyyy-MM-ddTHH:mm:ss.ss");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }      
        }

        private void DeshabilitarPaso6(String vCambio)
        {
            DDLCerrarCambio.SelectedIndex = 1;
            DDLCerrarCambio.Enabled = false;
            DDLCerrarCambio.CssClass = "form-control";

            BtnCerrarCambio.Enabled = false;
            BtnCerrarCambio.Text = "Cambio cerrado";
            BtnCerrarCambio.CssClass = "btn btn-success mr-2";


            vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
            HttpService vConector = new HttpService();
            msgInfoCambios vRequestInfoCambios = new msgInfoCambios()
            {
                tipo = "3",
                idcambio = vCambio,
                usuariogrud = vConfigurations.resultSet1[0].idUsuario
            };


            String vResponseCambios = "";
            HttpResponseMessage vHttpResponseCambios = vConector.PostInfoCambios(vRequestInfoCambios, ref vResponseCambios);
            if (vHttpResponseCambios.StatusCode == System.Net.HttpStatusCode.OK)
            {
                msgInfoCambiosQueryResponse vInfoCambiosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseCambios);
                if (vInfoCambiosResponse.resultSet1.Count() > 0)
                {
                    foreach (msgInfoCambiosQueryResponseItem itemArchivos in vInfoCambiosResponse.resultSet1)
                    {
                        if (itemArchivos.archivoCierre != null)
                        {
                            if (!itemArchivos.archivoCierre.Equals(""))
                            {
                                DIVCierreEvidencia.Visible = false;
                                DIVCierreEvidenciaDescargar.Visible = true;
                            }
                            else
                            {
                                DIVCierreEvidencia.Visible = true;
                                DIVCierreEvidenciaDescargar.Visible = false;
                            }
                        }
                        else
                        {
                            DIVCierreEvidencia.Visible = true;
                            DIVCierreEvidenciaDescargar.Visible = false;
                        }
                    }
                }
            }









        }
        private void DeshabilitarPaso5(string GETIDCAMBIO, HttpService vConector)
        {
            msgInfoCambiosCierre vInfoCambiosCierreRequest = new msgInfoCambiosCierre()
            {
                tipo = "3",
                idcambio = GETIDCAMBIO
            };
            String vResponseCambiosCierre = "";
            HttpResponseMessage vHttpResponseCambiosCierre = vConector.PostInfoCambiosCierre(vInfoCambiosCierreRequest, ref vResponseCambiosCierre);

            if (vHttpResponseCambiosCierre.StatusCode == System.Net.HttpStatusCode.OK)
            {
                msgInfoCambiosCierreQueryResponse vInfoCambiosCierreResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosCierreQueryResponse>(vResponseCambiosCierre);
                if (vInfoCambiosCierreResponse.resultSet1.Count() > 0)
                {
                    foreach (msgInfoCambiosCierreQueryResponseItem itemCierre in vInfoCambiosCierreResponse.resultSet1)
                    {
                        CierreItems(itemCierre);
                    }
                }
            }
        }
        private void DeshabilitarPaso4()
        {
            GVProcedimientosImplementacion.Enabled = false;
            GVRollbackImplementacion.Enabled = false;
            GVPruebasImplementacion.Enabled = false;
            BtnImplementacion.Enabled = false;
            BtnImplementacion.Text = "Implementación terminada";
            BtnImplementacion.CssClass = "btn btn-success mr-2";
        }
        private void DeshabilitarPaso3(msgInfoCambiosQueryResponseItem item)
        {
            DDLRevisionQA.SelectedIndex = 1;
            TxRevisionQAInicio.Text = Convert.ToDateTime(item.fechaQAInicio).ToString("yyyy-MM-ddTHH:mm:ss.ss");
            TxRevisionQAFinal.Text = Convert.ToDateTime(item.fechaQAFinal).ToString("yyyy-MM-ddTHH:mm:ss.ss");

            HttpService vConector = new HttpService();
            msgInfoImplementadores vInfoImplementadoresRequest = new msgInfoImplementadores()
            {
                tipo = "2",
                idcambio = item.idcambio
            };

            String vResponseImplementadores = "";
            HttpResponseMessage vHttpResponseImplmentadores = vConector.PostImplementadores(vInfoImplementadoresRequest, ref vResponseImplementadores);
            if (vHttpResponseImplmentadores.StatusCode == System.Net.HttpStatusCode.OK)
            {
                

                msgInfoImplementadoresQueryResponse vInfoImplementadoresResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoImplementadoresQueryResponse>(vResponseImplementadores);
                if (vInfoImplementadoresResponse.resultSet1.Count() > 0)
                {
                    vDatosCabImplementadores = new DataTable();
                    vDatosCabImplementadores.Columns.Add("nombre");
                    vDatosCabImplementadores.Columns.Add("usuario");
                    foreach (msgInfoImplementadoresQueryResponseItem itemImplementadores in vInfoImplementadoresResponse.resultSet1)
                    {

                        msgInfoUsuarios vRequest = new msgInfoUsuarios()
                        {
                            tipo = "2",
                            usuario = itemImplementadores.usuario
                        };

                        String vResponseResult = "";
                        HttpResponseMessage vHttpResponse = vConector.PostInfoUsuarios(vRequest, ref vResponseResult);
                        if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            msgInfoUsuariosQueryResponse vUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoUsuariosQueryResponse>(vResponseResult);
                            foreach (msgInfoUsuariosQueryResponseItem itemUsuarios in vUsuariosResponse.resultSet1)
                            {
                                vDatosCabImplementadores.Rows.Add(
                                    itemUsuarios.nombres + " " + itemUsuarios.apellidos,
                                    itemImplementadores.usuario);
                            }
                        } 
                    }
                }
            }

            GVCABImplementadores.DataSource = vDatosCabImplementadores;
            GVCABImplementadores.DataBind();
            GVCABImplementadores.Enabled = false;


            DDLCABImplementadores.SelectedIndex = 1;
            DDLCABImplementadores.Enabled = false;
            DDLCABImplementadores.CssClass = "form-control";


            BtnCABAgregarImplementador.Enabled = false;
            BtnCABAgregarImplementador.CssClass = "btn btn-success mr-2";

            DDLRevisionQA.Enabled = false;
            DDLRevisionQA.CssClass = "form-control";
            TxRevisionQAInicio.ReadOnly = true;
            TxRevisionQAFinal.ReadOnly = true;

            BtnRevisionQA.Enabled = false;
            BtnRevisionQA.Text = "Cambio certificado";
            BtnRevisionQA.CssClass = "btn btn-success mr-2";
        }
        private void DeshabilitarPaso2(String vCambio)
        {
            DDLCertificacion.SelectedIndex = 1;
            DDLCertificacion.Enabled = false;
            DDLCertificacion.CssClass = "form-control";

            BtnCertificarCambio.Enabled = false;
            BtnCertificarCambio.Text = "Cambio certificado";
            BtnCertificarCambio.CssClass = "btn btn-success mr-2";

            BtnAutorizarQA.Visible = false;
            BtnAutorizarQA.Enabled = false;
            BtnAutorizarQA.Text = "Autorizado";
            BtnAutorizarQA.CssClass = "btn btn-success mr-2";

            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                msgInfoCambios vInfoCambiosRequest = new msgInfoCambios()
                {
                    tipo = "3",
                    idcambio = vCambio,
                    usuariogrud = vConfigurations.resultSet1[0].idUsuario
                };
                String vResponseCambios = "";
                HttpResponseMessage vHttpResponseCambios = vConector.PostInfoCambios(vInfoCambiosRequest, ref vResponseCambios);

                if (vHttpResponseCambios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoCambiosQueryResponse vInfoCambiosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseCambios);
                    if (vInfoCambiosResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoCambiosQueryResponseItem itemArchivos in vInfoCambiosResponse.resultSet1)
                        {
                            if (itemArchivos.archivo != null)
                            {
                                if (!itemArchivos.archivo.Equals(""))
                                {
                                    DIVQAArchivo.Visible = false;
                                    DIVQAArchivoDescarga.Visible = true;
                                }
                            }
                        }
                        CargarrQADatosPromotor(vInfoCambiosResponse.resultSet1[0].idcambio);

                        TxHorarioInicioPromotor.ReadOnly = true;
                        TxHorarioFinalPromotor.ReadOnly = true;
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }

        }

        private void DeshabilitarPaso1()
        {
            BtnGuardarCambio.Enabled = false;
            BtnGuardarCambio.Text = "Bloqueado";
            BtnGuardarCambio.CssClass = "btn btn-primary mr-2";




            BtnAsignarUsuario.Visible = false;

        }

        private void CierreItems(msgInfoCambiosCierreQueryResponseItem itemCierre)
        {
            DDLResultado.SelectedIndex = CargarInformacionDDL(DDLResultado, itemCierre.resultado);
            DDLResultado.Enabled = false;
            DDLResultado.CssClass = "form-control";
            TxCierreObservaciones.Text = itemCierre.observaciones;
            TxCierreObservaciones.ReadOnly = true;
            TxCierreImpacto.Text = itemCierre.impacto;
            TxCierreImpacto.ReadOnly = true;
            TxCierreVentanaInicio.Text = Convert.ToDateTime(itemCierre.fechaVentanaInicio).ToString("yyyy-MM-ddTHH:mm:ss.ss");
            TxCierreVentanaInicio.ReadOnly = true;
            TxCierreVentanaFinal.Text = Convert.ToDateTime(itemCierre.fechaVentanaFin).ToString("yyyy-MM-ddTHH:mm:ss.ss");
            TxCierreVentanaFinal.ReadOnly = true;
            TxCierreDenegacionInicio.Text = Convert.ToDateTime(itemCierre.fechaVentanaDenegacionInicio).ToString("yyyy-MM-ddTHH:mm:ss.ss");
            TxCierreDenegacionInicio.ReadOnly = true;
            TxCierreDenegacionFinal.Text = Convert.ToDateTime(itemCierre.fechaVentanaDenegacionFin).ToString("yyyy-MM-ddTHH:mm:ss.ss");
            TxCierreDenegacionFinal.ReadOnly = true;
            TxCierreRollback.Text = itemCierre.rollback;
            TxCierreRollback.ReadOnly = true;
            TxCierreRollbackInicio.Text = Convert.ToDateTime(itemCierre.fechaRollbackInicio).ToString("yyyy-MM-ddTHH:mm:ss.ss");
            TxCierreRollbackInicio.ReadOnly = true;
            TxCierreRollbackFin.Text = Convert.ToDateTime(itemCierre.fechaRollbackFin).ToString("yyyy-MM-ddTHH:mm:ss.ss");
            TxCierreRollbackFin.ReadOnly = true;
            TxCierreRollbackDenInicio.Text = Convert.ToDateTime(itemCierre.fechaRollbackDenegacionInicio).ToString("yyyy-MM-ddTHH:mm:ss.ss");
            TxCierreRollbackDenInicio.ReadOnly = true;
            TxCierreRollbackDenFin.Text = Convert.ToDateTime(itemCierre.fechaRollbackDenegacionFin).ToString("yyyy-MM-ddTHH:mm:ss.ss");
            TxCierreRollbackDenFin.ReadOnly = true;

            if (itemCierre.deposito1 != null)
            {
                if (itemCierre.deposito1.Equals(""))
                {
                    DivEvidenciaDescarga1.Visible = false;
                    DivEvidenciaSubir1.Visible = true;
                    FUEvidenciaSubir1.Enabled = false;
                }
                else
                {
                    DivEvidenciaDescarga1.Visible = true;
                    DivEvidenciaSubir1.Visible = false;
                }
            }
            else
            {
                FUEvidenciaSubir1.Enabled = false;
                FUEvidenciaSubir1.CssClass = "form-control";
            }
            if (itemCierre.deposito2 != null)
            {
                if (itemCierre.deposito2.Equals(""))
                {
                    DivEvidenciaDescarga2.Visible = false;
                    DivEvidenciaSubir2.Visible = true;
                    FUEvidenciaSubir2.Enabled = false;
                }
                else
                {
                    DivEvidenciaDescarga2.Visible = true;
                    DivEvidenciaSubir2.Visible = false;
                }
            }
            else
            {
                FUEvidenciaSubir2.Enabled = false;
                FUEvidenciaSubir2.CssClass = "form-control";
            }
        }

        void CargarCambio(String vIdCambio)
        {
            try
            {
                LbNumeroCambio.Text = vIdCambio;

                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                msgInfoCambios vInfoCambiosRequest = new msgInfoCambios()
                {
                    tipo = "3",
                    idcambio = vIdCambio,
                    usuariogrud = vConfigurations.resultSet1[0].idUsuario
                };
                String vResponseCambios = "";
                HttpResponseMessage vHttpResponseCambios = vConector.PostInfoCambios(vInfoCambiosRequest, ref vResponseCambios);

                if (vHttpResponseCambios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoCambiosQueryResponse vInfoCambiosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseCambios);
                    if (vInfoCambiosResponse.resultSet1.Count() > 0)
                    {
                        LbCambioDescripcion.Text = vInfoCambiosResponse.resultSet1[0].mantenimientoNombre;

                        Session["ESTADOCAMBIO"] = vInfoCambiosResponse.resultSet1[0].idResolucion;
                        if (vInfoCambiosResponse.resultSet1[0].idResolucion.Equals("1"))
                            Mensaje("Este cambio ya esta cerrado", WarningType.Info);

                    }
                }


            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        private void DescargaArchivos(Boolean vBool)
        {
            DIVDeposito1.Visible = !vBool;
            DIVDeposito2.Visible = !vBool;
            DIVDeposito3.Visible = !vBool;

            DIVDescargarDeposito1.Visible = vBool;
            DIVDescargarDeposito2.Visible = vBool;
            DIVDescargarDeposito3.Visible = vBool;
        }
        public void Mensaje(string vMensaje, WarningType type)
        {
            try
            {
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                Logs vLog = new Logs();
                vLog.postLog("Mensajes", vMensaje, vConfigurations.resultSet1[0].idUsuario);
            }
            catch { }

            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "text", "infatlan.showNotification('top','center','" + vMensaje + "','" + type.ToString().ToLower() + "')", true);
        }
        public void CerrarModal(String vModal)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#" + vModal + "').modal('hide');", true);
        }
        public void CargarInformacionCambio(String vCambio)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                msgInfoCambios vInfoCambiosRequest = new msgInfoCambios()
                {
                    tipo = "3",
                    idcambio = vCambio,
                    usuariogrud = vConfigurations.resultSet1[0].idUsuario
                };
                String vResponseCambios = "";
                HttpResponseMessage vHttpResponseCambios = vConector.PostInfoCambios(vInfoCambiosRequest, ref vResponseCambios);

                if (vHttpResponseCambios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoCambiosQueryResponse vInfoCambiosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseCambios);
                    if (vInfoCambiosResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoCambiosQueryResponseItem itemCambio in vInfoCambiosResponse.resultSet1)
                        {
                            TxNombreMantenimiento.Text = itemCambio.mantenimientoNombre;
                            DDLProveedor.SelectedIndex = CargarInformacionDDL(DDLProveedor, itemCambio.idProveedorServicio);
                            CargarInformacionCriticidad(itemCambio.idCriticidad);
                            CargarInformacionImpacto(itemCambio.idImpacto);
                            CargarInformacionRiesgo(itemCambio.idRiesgo);
                            TxObservaciones.Text = itemCambio.observaciones;

                            Session["MANTENIMIENTOSESSION"] = itemCambio.idMantenimiento;
                            Session["CALENDARIOSESSION"] = itemCambio.idCalendario;

                            if (itemCambio.idUsuarioResponsable.Equals(""))
                            {
                                BtnAsignarUsuario.Text = "Asignar QA";
                                BtnAsignarUsuario.CssClass = "btn btn-light bg-white mr-2 ";
                            }
                            else
                            {
                                BtnAsignarUsuario.Text = "" + itemCambio.idUsuarioResponsable;
                                BtnAsignarUsuario.CssClass = "btn btn-success mr-2 ";
                            }
                            Session["USUARIOASIGNADO"] = itemCambio.idUsuarioResponsable;

                            if (itemCambio.idResolucion.Equals("1"))
                            {
                                BtnAsignarUsuario.CssClass = "btn btn-success mr-2 ";
                                BtnAsignarUsuario.Enabled = false;
                                BtnGuardarCambio.Visible = false;
                            }

                            //MANTENIMIENTOS
                            msgInfoMantenimientos vRequestMantenimiento = new msgInfoMantenimientos()
                            {
                                tipo = "3",
                                idmantenimiento = itemCambio.idMantenimiento
                            };

                            String vResponseMantenimientos = "";
                            HttpResponseMessage vHttpResponseMantenimientos = vConector.PostInfoMantenimientos(vRequestMantenimiento, ref vResponseMantenimientos);
                            if (vHttpResponseMantenimientos.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                msgInfoMantenimientosQueryResponse vInfoMantenimientosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoMantenimientosQueryResponse>(vResponseMantenimientos);
                                if (vInfoMantenimientosResponse.resultSet1.Count() > 0)
                                {
                                    foreach (msgInfoMantenimientosQueryResponseItem itemMantenimiento in vInfoMantenimientosResponse.resultSet1)
                                    {
                                        TxMantenimientoDescripcion.Text = itemMantenimiento.descripcion;
                                        DDLTipoMantenimiento.SelectedIndex = CargarInformacionDDL(DDLTipoMantenimiento, itemMantenimiento.idTipoMantenimiento);
                                        CargarSubTipoMantenimientoDDL(itemMantenimiento.idTipoMantenimiento, itemMantenimiento.idTipoMantenimientoSub);
                                        CargarInformacionTipoCambio(itemMantenimiento.idTipoCambio);
                                        CargarInformacionLugarMantenimiento(itemMantenimiento.idTipoLugarMantenimiento);
                                        TxTicket.Text = itemMantenimiento.otros;
                                    }
                                }
                            }

                            //CAMBIOS
                            msgInfoCalendarios vRequestCalendarios = new msgInfoCalendarios()
                            {
                                tipo = "3",
                                idcalendario = itemCambio.idCalendario
                            };

                            String vResponseInfoCalendarios = "";
                            HttpResponseMessage vHttpResponseInfoCalendarios = vConector.PostInfoCalendarios(vRequestCalendarios, ref vResponseInfoCalendarios);
                            if (vHttpResponseInfoCalendarios.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                msgInfoCalendariosQueryResponse vInfoCalendariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCalendariosQueryResponse>(vResponseInfoCalendarios);
                                if (vInfoCalendariosResponse.resultSet1.Count() > 0)
                                {
                                    foreach (msgInfoCalendariosQueryResponseItem itemCalendario in vInfoCalendariosResponse.resultSet1)
                                    {
                                        TxVentanaDuracionInicio.Text = Convert.ToDateTime(itemCalendario.horaVentanaInicio).ToString("yyyy-MM-ddTHH:mm:ss.ss");
                                        TxVentanaDuracionFin.Text = Convert.ToDateTime(itemCalendario.horaVentanaFin).ToString("yyyy-MM-ddTHH:mm:ss.ss");
                                        TxVentanaDenegacionInicio.Text = Convert.ToDateTime(itemCalendario.horaDenegacionInicio).ToString("yyyy-MM-ddTHH:mm:ss.ss");
                                        TxVentanaDenegacionFin.Text = Convert.ToDateTime(itemCalendario.horaDenegacionFin).ToString("yyyy-MM-ddTHH:mm:ss.ss");
                                    }
                                }
                            }

                            //SISTEMAS - CANALES
                            msgInfoSistemas vRequestSistemas = new msgInfoSistemas()
                            {
                                tipo = "3",
                                idcanal = itemCambio.idcambio
                            };

                            String vResponseCanales = "";
                            HttpResponseMessage vHttpResponseCanales = vConector.PostInfoCanales(vRequestSistemas, ref vResponseCanales);
                            if (vHttpResponseCanales.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                msgInfoSistemasQueryResponse vInfoCanalesResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoSistemasQueryResponse>(vResponseCanales);
                                if (vInfoCanalesResponse.resultSet1.Count() > 0)
                                {
                                    vDatosSistemas = new DataTable();
                                    vDatosSistemas.Columns.Add("idCanal");
                                    vDatosSistemas.Columns.Add("sistema");
                                    vDatosSistemas.Columns.Add("descripcion");
                                    vDatosSistemas.Columns.Add("denegacion");
                                    vDatosSistemas.Columns.Add("inicio");
                                    vDatosSistemas.Columns.Add("final");
                                    foreach (msgInfoSistemasQueryResponseItem itemCanales in vInfoCanalesResponse.resultSet1)
                                    {
                                        vDatosSistemas.Rows.Add(
                                            itemCanales.idCanal,
                                            itemCanales.nombreSistema,
                                            itemCanales.descripcion,
                                            itemCanales.denegacion,
                                            itemCanales.fechaInicio,
                                            itemCanales.fechaFinal);
                                    }

                                    GVSistemas.DataSource = vDatosSistemas;
                                    GVSistemas.DataBind();
                                    Session["DATASISTEMAS"] = vDatosSistemas;
                                }
                            }

                            //EQUIPOS
                            msgInfoEquipos vRequestEquipos = new msgInfoEquipos()
                            {
                                tipo = "3",
                                idequipo = itemCambio.idcambio
                            };

                            String vResponseEquipos = "";
                            HttpResponseMessage vHttpResponseEquipos = vConector.PostInfoEquipos(vRequestEquipos, ref vResponseEquipos);
                            if (vHttpResponseEquipos.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                msgInfoEquiposQueryResponse vInfoEquiposResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoEquiposQueryResponse>(vResponseEquipos);
                                if (vInfoEquiposResponse.resultSet1.Count() > 0)
                                {
                                    vDatosEquipos = new DataTable();
                                    vDatosEquipos.Columns.Add("idEquipo");
                                    vDatosEquipos.Columns.Add("idCatEquipo");
                                    vDatosEquipos.Columns.Add("nombre");
                                    vDatosEquipos.Columns.Add("ip");
                                    vDatosEquipos.Columns.Add("tipoEquipo");
                                    foreach (msgInfoEquiposQueryResponseItem itemEquipos in vInfoEquiposResponse.resultSet1)
                                    {
                                        vDatosEquipos.Rows.Add(
                                            itemEquipos.idEquipo,
                                            itemEquipos.idCatEquipo,
                                            itemEquipos.nombre,
                                            itemEquipos.ip,
                                            itemEquipos.tipoEquipo
                                            );
                                    }
                                    GVEquipos.DataSource = vDatosEquipos;
                                    GVEquipos.DataBind();
                                    Session["DATAEQUIPOS"] = vDatosEquipos;
                                }
                            }

                            //PERSONAL
                            msgInfoPersonal vRequestPersonal = new msgInfoPersonal()
                            {
                                tipo = "3",
                                idpersonal = itemCambio.idcambio
                            };

                            String vResponsPersonal = "";
                            HttpResponseMessage vHttpResponsePersonal = vConector.PostInfoPersonal(vRequestPersonal, ref vResponsPersonal);
                            if (vHttpResponsePersonal.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                msgInfoPersonalQueryResponse vInfoPersonalResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoPersonalQueryResponse>(vResponsPersonal);
                                if (vInfoPersonalResponse.resultSet1.Count() > 0)
                                {
                                    vDatosPersonal = new DataTable();
                                    vDatosPersonal.Columns.Add("idPersonal");
                                    vDatosPersonal.Columns.Add("nombre");
                                    vDatosPersonal.Columns.Add("cargo");
                                    vDatosPersonal.Columns.Add("estado");
                                    foreach (msgInfoPersonalQueryResponseItem itemPersonal in vInfoPersonalResponse.resultSet1)
                                    {
                                        vDatosPersonal.Rows.Add(
                                            itemPersonal.idPersonal,
                                            itemPersonal.nombre,
                                            itemPersonal.cargo,
                                            itemPersonal.cargo
                                            );
                                    }
                                    GVPersonal.DataSource = vDatosPersonal;
                                    GVPersonal.DataBind();
                                    Session["DATAPERSONAL"] = vDatosPersonal;
                                }
                            }

                            //COMUNICACIONES
                            msgInfoComunicaciones vRequestComunicaciones = new msgInfoComunicaciones()
                            {
                                tipo = "3",
                                idcomunicacion = itemCambio.idcambio
                            };

                            String vResponsComunicaciones = "";
                            HttpResponseMessage vHttpResponseComunicaciones = vConector.PostInfoComunicaciones(vRequestComunicaciones, ref vResponsComunicaciones);
                            if (vHttpResponseComunicaciones.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                msgInfoComunicacionesQueryResponse vInfoComunicacionesResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoComunicacionesQueryResponse>(vResponsComunicaciones);
                                if (vInfoComunicacionesResponse.resultSet1.Count() > 0)
                                {

                                    vDatosComunicaciones = new DataTable();
                                    vDatosComunicaciones.Columns.Add("idComunicacion");
                                    vDatosComunicaciones.Columns.Add("cambio");
                                    vDatosComunicaciones.Columns.Add("incidente");
                                    foreach (msgInfoComunicacionesQueryResponseItem itemComunicaciones in vInfoComunicacionesResponse.resultSet1)
                                    {
                                        vDatosComunicaciones.Rows.Add(
                                            itemComunicaciones.idComunicacion,
                                            itemComunicaciones.cambioNormal,
                                            itemComunicaciones.casoIncidente
                                            );
                                    }
                                    GVComunicaciones.DataSource = vDatosComunicaciones;
                                    GVComunicaciones.DataBind();
                                    Session["DATACOMUNICACIONES"] = vDatosComunicaciones;
                                }
                            }

                            //PROCEDIMIENTOS
                            msgInfoProcedimientos vRequestProcedimientos = new msgInfoProcedimientos()
                            {
                                tipo = "3",
                                idprocedimiento = itemCambio.idcambio
                            };

                            String vResponseProcedimientos = "";
                            HttpResponseMessage vHttpResponseProcedimientos = vConector.PostInfoProcedimientos(vRequestProcedimientos, ref vResponseProcedimientos);
                            if (vHttpResponseProcedimientos.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                msgInfoProcedimientosQueryResponse vInfoProcedimientosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoProcedimientosQueryResponse>(vResponseProcedimientos);
                                if (vInfoProcedimientosResponse.resultSet1.Count() > 0)
                                {
                                    vDatosProcedimientos = new DataTable();
                                    vDatosProcedimientos.Columns.Add("idProcedimiento");
                                    vDatosProcedimientos.Columns.Add("responsable");
                                    vDatosProcedimientos.Columns.Add("detalle");
                                    vDatosProcedimientos.Columns.Add("inicio");
                                    vDatosProcedimientos.Columns.Add("fin");
                                    vDatosProcedimientos.Columns.Add("estado");
                                    foreach (msgInfoProcedimientosQueryResponseItem itemProcedimiento in vInfoProcedimientosResponse.resultSet1)
                                    {
                                        vDatosProcedimientos.Rows.Add(
                                            itemProcedimiento.idProcedimiento,
                                            itemProcedimiento.idUsuarioResponsable,
                                            itemProcedimiento.descripcion,
                                            itemProcedimiento.fechaInicio,
                                            itemProcedimiento.fechaFin,
                                            itemProcedimiento.estado
                                            );
                                    }
                                    GVProcedimientos.DataSource = vDatosProcedimientos;
                                    GVProcedimientos.DataBind();
                                    
                                    GVProcedimientosImplementacion.DataSource = vDatosProcedimientos;
                                    GVProcedimientosImplementacion.DataBind();
                                    GVProcedimientosImplementacion.Columns[4].Visible = true;
                                    foreach (GridViewRow row in GVProcedimientosImplementacion.Rows)
                                    {
                                        foreach (msgInfoProcedimientosQueryResponseItem itemProcedimiento in vInfoProcedimientosResponse.resultSet1)
                                        {
                                            if (itemProcedimiento.estado != null)
                                            {
                                                CheckBox chk = row.Cells[4].FindControl("CBProcedimientos") as CheckBox;
                                                if (itemProcedimiento.idProcedimiento.Equals(chk.Attributes["value"]))
                                                {
                                                    if (itemProcedimiento.estado.Equals("true"))
                                                    {
                                                        chk.Checked = true;
                                                    }
                                                    else
                                                    {
                                                        chk.Checked = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    Session["DATAPROCEDIMIENTOS"] = vDatosProcedimientos;
                                }
                            }

                            //ROLLBACK
                            msgInfoRollbacks vRequestRollback = new msgInfoRollbacks()
                            {
                                tipo = "3",
                                idrollback = itemCambio.idcambio
                            };

                            String vResponseRollback = "";
                            HttpResponseMessage vHttpResponseRollback = vConector.PostInfoRollbacks(vRequestRollback, ref vResponseRollback);
                            if (vHttpResponseRollback.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                msgInfoRollbacksQueryResponse vInfoRollbackResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoRollbacksQueryResponse>(vResponseRollback);
                                if (vInfoRollbackResponse.resultSet1.Count() > 0)
                                {
                                    vDatosRollback = new DataTable();
                                    vDatosRollback.Columns.Add("idRollback");
                                    vDatosRollback.Columns.Add("responsable");
                                    vDatosRollback.Columns.Add("detalle");
                                    vDatosRollback.Columns.Add("estado");

                                    foreach (msgInfoRollbacksQueryResponseItem itemRollback in vInfoRollbackResponse.resultSet1)
                                    {
                                        vDatosRollback.Rows.Add(
                                            itemRollback.idRollback,
                                            itemRollback.idUsuarioResponsable,
                                            itemRollback.descripcion,
                                            itemRollback.estado
                                            );
                                    }
                                    GVRollback.DataSource = vDatosRollback;
                                    GVRollback.DataBind();

                                    GVRollbackImplementacion.DataSource = vDatosRollback;
                                    GVRollbackImplementacion.DataBind();
                                    GVRollbackImplementacion.Columns[2].Visible = true;
                                    foreach (GridViewRow row in GVRollbackImplementacion.Rows)
                                    {
                                        foreach (msgInfoRollbacksQueryResponseItem itemRollback in vInfoRollbackResponse.resultSet1)
                                        {
                                            if (itemRollback.estado != null)
                                            {
                                                CheckBox chk = row.Cells[2].FindControl("CBRollback") as CheckBox;
                                                if (itemRollback.idRollback.Equals(chk.Attributes["value"]))
                                                {
                                                    if (itemRollback.estado.Equals("true"))
                                                    {
                                                        chk.Checked = true;
                                                    }
                                                    else
                                                    {
                                                        chk.Checked = false;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    Session["DATAROLLBACK"] = vDatosRollback;
                                }
                            }

                            //PRUEBAS
                            msgInfoPruebas vRequestPruebas = new msgInfoPruebas()
                            {
                                tipo = "3",
                                idprueba = itemCambio.idcambio
                            };

                            String vResponsePruebas = "";
                            HttpResponseMessage vHttpResponsePruebas = vConector.PostInfoPruebas(vRequestPruebas, ref vResponsePruebas);
                            if (vHttpResponsePruebas.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                msgInfoPruebasQueryResponse vInfoPruebasResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoPruebasQueryResponse>(vResponsePruebas);
                                if (vInfoPruebasResponse.resultSet1.Count() > 0)
                                {
                                    vDatosPruebas = new DataTable();
                                    vDatosPruebas.Columns.Add("idPrueba");
                                    vDatosPruebas.Columns.Add("responsable");
                                    vDatosPruebas.Columns.Add("detalle");
                                    vDatosPruebas.Columns.Add("estado");
                                    foreach (msgInfoPruebasQueryResponseItem itemPruebas in vInfoPruebasResponse.resultSet1)
                                    {
                                        vDatosPruebas.Rows.Add(
                                            itemPruebas.idPrueba,
                                            itemPruebas.idUsuarioResponsable,
                                            itemPruebas.descripcion,
                                            itemPruebas.estado
                                            );
                                    }
                                    GVPruebas.DataSource = vDatosPruebas;
                                    GVPruebas.DataBind();

                                    GVPruebasImplementacion.DataSource = vDatosPruebas;
                                    GVPruebasImplementacion.DataBind();
                                    GVPruebasImplementacion.Columns[2].Visible = true;
                                    foreach (GridViewRow row in GVPruebasImplementacion.Rows)
                                    {
                                        foreach (msgInfoPruebasQueryResponseItem itemPruebas in vInfoPruebasResponse.resultSet1)
                                        {
                                            if (itemPruebas.estado != null)
                                            {
                                                CheckBox chk = row.Cells[2].FindControl("CBPruebas") as CheckBox;
                                                if (itemPruebas.idPrueba.Equals(chk.Attributes["value"]))
                                                {
                                                    if (itemPruebas.estado.Equals("true"))
                                                    {
                                                        chk.Checked = true;
                                                    }
                                                    else
                                                    {
                                                        chk.Checked = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    Session["DATAPRUEBAS"] = vDatosPruebas;
                                }
                            }

                            //ARCHIVOS
                            msgInfoArchivos vRequestArchivos = new msgInfoArchivos()
                            {
                                tipo = "3",
                                idcambio = itemCambio.idcambio
                            };

                            String vResponseArchivos = "";
                            HttpResponseMessage vHttpResponseArchivos = vConector.PostInfoArchivos(vRequestArchivos, ref vResponseArchivos);
                            if (vHttpResponseArchivos.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                msgInfoArchivosQueryResponse vInfoPruebasResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoArchivosQueryResponse>(vResponseArchivos);
                                if (vInfoPruebasResponse.resultSet1.Count() > 0)
                                {
                                    foreach (msgInfoArchivosQueryResponseItem itemArchivos in vInfoPruebasResponse.resultSet1)
                                    {
                                        if (itemArchivos.deposito1.Equals(""))
                                        {
                                            DIVDescargarDeposito1.Visible = false;
                                            DIVDeposito1.Visible = true;
                                        }
                                        else
                                        {
                                            LbNombreDeposito1.Text = itemArchivos.depot1nombre;
                                        }
                                        if (itemArchivos.deposito2.Equals(""))
                                        {
                                            DIVDescargarDeposito2.Visible = false;
                                            DIVDeposito2.Visible = true;
                                        }
                                        else
                                        {
                                            LbNombreDeposito2.Text = itemArchivos.depot2nombre;
                                        }
                                        if (itemArchivos.deposito3.Equals(""))
                                        {
                                            DIVDescargarDeposito3.Visible = false;
                                            DIVDeposito3.Visible = true;
                                        }
                                        else
                                        {
                                            LbNombreDeposito3.Text = itemArchivos.depot3nombre;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }
        int CargarInformacionDDL(DropDownList vList, String vValue)
        {
            int vIndex = 0;
            try
            {
                int vContador = 0;
                foreach (ListItem item in vList.Items)
                {
                    if (item.Value.Equals(vValue))
                    {
                        vIndex = vContador;
                    }
                    vContador++;
                }
            }
            catch { throw; }
            return vIndex;
        }
        void CargarInformacionCriticidad(String vValue)
        {
            try
            {
                if (RBCriticidadAlta.Value.Equals(vValue))
                    RBCriticidadAlta.Checked = true;
                if (RBCriticidadMedia.Value.Equals(vValue))
                    RBCriticidadMedia.Checked = true;
                if (RBCriticidadBaja.Value.Equals(vValue))
                    RBCriticidadBaja.Checked = true;
            }
            catch { throw; }
        }
        void CargarInformacionCriticidadQA(String vValue)
        {
            try
            {
                if (CriticidadRadio1.Value.Equals(vValue))
                    CriticidadRadio1.Checked = true;
                if (CriticidadRadio2.Value.Equals(vValue))
                    CriticidadRadio2.Checked = true;
                if (CriticidadRadio3.Value.Equals(vValue))
                    CriticidadRadio3.Checked = true;
            }
            catch { throw; }
        }
        void CargarInformacionImpacto(String vValue)
        {
            try
            {
                if (RBImpactoAlta.Value.Equals(vValue))
                    RBImpactoAlta.Checked = true; 
                if (RBImpactoMedia.Value.Equals(vValue))
                    RBImpactoMedia.Checked = true; 
                if (RBImpactoBaja.Value.Equals(vValue))
                    RBImpactoBaja.Checked = true; 
            }
            catch { throw; }
        }

        void CargarInformacionImpactoQA(String vValue)
        {
            try
            {
                if (ImpactoRadio1.Value.Equals(vValue))
                    ImpactoRadio1.Checked = true;
                if (ImpactoRadio2.Value.Equals(vValue))
                    ImpactoRadio2.Checked = true;
                if (ImpactoRadio3.Value.Equals(vValue))
                    ImpactoRadio3.Checked = true;
            }
            catch { throw; }
        }

        void CargarInformacionRiesgo(String vValue)
        {
            try
            {
                if (RBRiesgoAlta.Value.Equals(vValue))
                    RBRiesgoAlta.Checked = true; 
                if (RBRiesgoMedia.Value.Equals(vValue))
                    RBRiesgoMedia.Checked = true; 
                if (RBRiesgoBaja.Value.Equals(vValue))
                    RBRiesgoBaja.Checked = true; 
            }
            catch { throw; }
        }
        void CargarInformacionRiesgoQA(String vValue)
        {
            try
            {
                if (RiesgoRadio1.Value.Equals(vValue))
                    RiesgoRadio1.Checked = true;
                if (RiesgoRadio2.Value.Equals(vValue))
                    RiesgoRadio2.Checked = true;
                if (RiesgoRadio3.Value.Equals(vValue))
                    RiesgoRadio3.Checked = true;
            }
            catch { throw; }
        }
        void CargarSubTipoMantenimientoDDL(String vValueInicial, String vValueFinal)
        {
            DDLSubtipoMantenimiento.Items.Clear();
            HttpService vConnect = new HttpService();
            msgGeneralesResponse vItems = vConnect.getGenerales("8", vValueInicial);

            DDLSubtipoMantenimiento.Items.Add(new ListItem { Text = "Seleccione una opción", Value = "0" });
            foreach (msgGeneralesResponseItem item in vItems.resultSet1)
            {
                DDLSubtipoMantenimiento.Items.Add(new ListItem { Text = item.id + ". " + item.descripcion, Value = item.id });
            }

            int vContador = 0;
            foreach (ListItem item in DDLSubtipoMantenimiento.Items)
            {
                if (item.Value.Equals(vValueFinal))
                {
                    DDLSubtipoMantenimiento.SelectedIndex = vContador;
                }
                vContador++;
            }
        }
        void CargarInformacionLugarMantenimiento(String vValue)
        {
            try
            {
                if (RBLugarMantenimiento1.Value.Equals(vValue))
                    RBLugarMantenimiento1.Checked = true;
                if (RBLugarMantenimiento2.Value.Equals(vValue))
                    RBLugarMantenimiento2.Checked = true;
                if (RBLugarMantenimiento3.Value.Equals(vValue))
                    RBLugarMantenimiento3.Checked = true;
                if (RBLugarMantenimiento4.Value.Equals(vValue))
                    RBLugarMantenimiento4.Checked = true;
                if (RBLugarMantenimiento5.Value.Equals(vValue))
                    RBLugarMantenimiento5.Checked = true;
            }
            catch { throw; }
        }
        void CargarInformacionTipoCambio(String vValue)
        {
            try
            {
                if (RBMantenimientoTipoCambio1.Value.Equals(vValue))
                    RBMantenimientoTipoCambio1.Checked = true;
                if (RBMantenimientoTipoCambio2.Value.Equals(vValue))
                    RBMantenimientoTipoCambio2.Checked = true;
                if (RBMantenimientoTipoCambio3.Value.Equals(vValue))
                    RBMantenimientoTipoCambio3.Checked = true;
            }
            catch { throw; }
        }

        #region "Cargas Iniciales"
        void CargarSessionesPasos()
        {
            if (Session["DATAEQUIPOS"] != null)
            {
                vDatosEquipos = (DataTable)Session["DATAEQUIPOS"];
                GVEquipos.DataSource = vDatosEquipos;
                GVEquipos.DataBind();
            }
            if (Session["DATASISTEMAS"] != null)
            {
                vDatosSistemas = (DataTable)Session["DATASISTEMAS"];
                GVSistemas.DataSource = vDatosSistemas;
                GVSistemas.DataBind();
            }
            if (Session["DATAPERSONAL"] != null)
            {
                vDatosPersonal = (DataTable)Session["DATAPERSONAL"];
                GVPersonal.DataSource = vDatosPersonal;
                GVPersonal.DataBind();
            }
            if (Session["DATACOMUNICACIONES"] != null)
            {
                vDatosComunicaciones = (DataTable)Session["DATACOMUNICACIONES"];
                GVComunicaciones.DataSource = vDatosComunicaciones;
                GVComunicaciones.DataBind();
            }
            if (Session["DATAPROCEDIMIENTOS"] != null)
            {
                vDatosProcedimientos = (DataTable)Session["DATAPROCEDIMIENTOS"];
                GVProcedimientos.DataSource = vDatosProcedimientos;
                GVProcedimientos.DataBind();
            }
            if (Session["DATAROLLBACK"] != null)
            {
                vDatosRollback = (DataTable)Session["DATAROLLBACK"];
                GVRollback.DataSource = vDatosRollback;
                GVRollback.DataBind();
            }
            if (Session["DATAPRUEBAS"] != null)
            {
                vDatosPruebas = (DataTable)Session["DATAPRUEBAS"];
                GVPruebas.DataSource = vDatosPruebas;
                GVPruebas.DataBind();
            }
        }

        void CargarCatalogoEquipos()
        {
            try
            {
                HttpService vConnect = new HttpService();
                msgCatEquiposQueryResponse vItems = vConnect.getCatEquipos("1");

                DDLEquipo.Items.Add(new ListItem { Text = "Seleccione una opción", Value = "0" });
                DDLEquipoSecundario.Items.Add(new ListItem { Text = "Seleccione una opción", Value = "0" });
                foreach (msgCatEquiposQueryResponseItem item in vItems.resultSet1)
                {
                    DDLEquipo.Items.Add(new ListItem { Text = item.idCatEquipo + ". " + item.nombre, Value = item.idCatEquipo });
                    DDLEquipoSecundario.Items.Add(new ListItem { Text = item.idCatEquipo + ". " + item.nombre, Value = item.idCatEquipo });
                }
            }
            catch (Exception Ex){ LbMensajeSistemas.Text = Ex.Message; }
        }

        void CargarCatalogoSistemas()
        {
            try
            {
                //HttpService vConnect = new HttpService();
                //msgCatSistemasResponse vItems = vConnect.getCatSistemas("1");

                DDLSistema.Items.Add(new ListItem { Text = "Seleccione una opción", Value = "0" });
                //foreach (msgCatSistemasResponseItem item in vItems.resultSet1)
                //{
                //    DDLSistema.Items.Add(new ListItem { Text = item.idCatSistemas + ". " + item.sistema, Value = item.idCatSistemas });
                //}
            }
            catch (Exception Ex) { LbMensajeSistemas.Text = Ex.Message; }
        }

        void CargarCatalogoUsuarios()
        {
            try
            {
                msgInfoUsuarios vRequest = new msgInfoUsuarios()
                {
                    tipo = "4"
                };

                String vResponseResult = "";
                HttpService vConector = new HttpService();

                //test vTest = vConector.getTest(vRequest);
                HttpResponseMessage vHttpResponse = vConector.PostInfoUsuarios(vRequest, ref vResponseResult);
                if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoUsuariosQueryResponse vUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoUsuariosQueryResponse>(vResponseResult);

                    DDLUsuarios.Items.Add(new ListItem { Text = "Seleccione una opción", Value = "0" });
                    DDLProcedimientosResponsable.Items.Add(new ListItem { Text = "Seleccione una opción", Value = "0" });
                    DDLRollBackResponsable.Items.Add(new ListItem { Text = "Seleccione una opción", Value = "0" });
                    DDLPruebasResponsable.Items.Add(new ListItem { Text = "Seleccione una opción", Value = "0" });
                    DDLPersonalNombre.Items.Add(new ListItem { Text = "Seleccione una opción", Value = "0" });

                    DDLCABImplementadores.Items.Add(new ListItem { Text = "Seleccione una opción", Value = "0" });
                    foreach (msgInfoUsuariosQueryResponseItem item in vUsuariosResponse.resultSet1)
                    {
                        if (item.idcargo.Equals("3"))
                            DDLUsuarios.Items.Add(new ListItem { Text = item.nombres + " " + item.apellidos, Value = item.idUsuario });
                        if (item.idcargo.Equals("4"))
                        {
                            DDLProcedimientosResponsable.Items.Add(new ListItem { Text = item.nombres + " " + item.apellidos, Value = item.idUsuario });
                            DDLCABImplementadores.Items.Add(new ListItem { Text = item.nombres + " " + item.apellidos, Value = item.idUsuario });
                        }
                        if (item.idcargo.Equals("4"))
                            DDLRollBackResponsable.Items.Add(new ListItem { Text = item.nombres + " " + item.apellidos, Value = item.idUsuario });
                        if (item.idcargo.Equals("4"))
                            DDLPruebasResponsable.Items.Add(new ListItem { Text = item.nombres + " " + item.apellidos, Value = item.idUsuario });
                        DDLPersonalNombre.Items.Add(new ListItem { Text = item.nombres + " " + item.apellidos, Value = item.idUsuario });
                    }
                }
            }
            catch (Exception Ex) { LbMensajeSistemas.Text = Ex.Message; }
        }

        void CargarCatalogoProveedores()
        {
            try
            {
                DDLProveedor.Items.Add(new ListItem { Text = "Seleccione una opción", Value = "0" });
                DDLProveedor.Items.Add(new ListItem { Text = "1. Infatlan", Value = "1" });
                DDLProveedor.Items.Add(new ListItem { Text = "2. Banco Atlantida", Value = "2" });
                DDLProveedor.Items.Add(new ListItem { Text = "3. AFP Atlantidad", Value = "3" });
                DDLProveedor.Items.Add(new ListItem { Text = "4. Seguros Atlantidad", Value = "4" });
            }
            catch (Exception Ex) { LbMensajeSistemas.Text = Ex.Message; }
        }

        void CargarTipoMantenimiento()
        {
            try
            {
                HttpService vConnect = new HttpService();
                msgGeneralesResponse vItems = vConnect.getGenerales("7","");

                DDLTipoMantenimiento.Items.Add(new ListItem { Text = "Seleccione una opción", Value = "0" });
                foreach (msgGeneralesResponseItem item in vItems.resultSet1)
                {
                    DDLTipoMantenimiento.Items.Add(new ListItem { Text = item.id + ". " + item.descripcion, Value = item.id });
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        #endregion

        #region "Modales"
        protected void BtnCrearSistema_Click(object sender, EventArgs e)
        {
            try
            {
                if (DDLSistema.SelectedIndex == 0)
                    throw new Exception("Por favor seleccione una opcion en sistema");
                if (DDLSistemaDenegacion.SelectedIndex == 0)
                    throw new Exception("Por favor seleccione una opcion en denegacion");

                if (Session["DATASISTEMAS"] == null)
                {
                    vDatosSistemas = new DataTable();
                    vDatosSistemas.Columns.Add("idCanal");
                    vDatosSistemas.Columns.Add("sistema");
                    vDatosSistemas.Columns.Add("descripcion");
                    vDatosSistemas.Columns.Add("denegacion");
                    vDatosSistemas.Columns.Add("inicio");
                    vDatosSistemas.Columns.Add("final");
                }
                else
                {
                    vDatosSistemas = (DataTable)Session["DATASISTEMAS"];
                }

                HttpService vConnect = new HttpService();

                if (DDLSistema.SelectedValue.Equals("1000"))
                {
                    msgCatSistemasQueryResponse vItems = vConnect.getCatSistemas("2", DDLEquipo.SelectedValue);

                    foreach (msgCatSistemasQueryResponseItem item in vItems.resultSet1)
                    {
                        vDatosSistemas.Rows.Add(
                            "",
                            item.sistema,
                            TxSistemaDescripcion.Text,
                            (DDLSistemaDenegacion.SelectedValue.Equals("1") ? "SI" : "NO"),
                            TxSistemaTiempoInicio.Text,
                            TxSistemaTiempoFinal.Text);
                    }
                }
                else
                {
                    msgCatSistemasQueryResponse vItems = vConnect.getCatSistemas("1");

                    foreach (msgCatSistemasQueryResponseItem item in vItems.resultSet1)
                    {
                        if (DDLSistema.SelectedValue.Equals(item.idCatSistemas))
                        {
                            vDatosSistemas.Rows.Add(
                                "",
                                item.sistema,
                                TxSistemaDescripcion.Text,
                                (DDLSistemaDenegacion.SelectedValue.Equals("1") ? "SI" : "NO"),
                                TxSistemaTiempoInicio.Text,
                                TxSistemaTiempoFinal.Text);
                        }

                    }
                }


                GVSistemas.DataSource = vDatosSistemas;
                GVSistemas.DataBind();
                Session["DATASISTEMAS"] = vDatosSistemas;

                try
                {
                    if (Session["DATAEQUIPOS"] == null)
                    {
                        vDatosEquipos = new DataTable();
                        vDatosEquipos.Columns.Add("idEquipo");
                        vDatosEquipos.Columns.Add("idCatEquipo");
                        vDatosEquipos.Columns.Add("nombre");
                        vDatosEquipos.Columns.Add("ip");
                        vDatosEquipos.Columns.Add("tipoEquipo");
                    }
                    else
                    {
                        vDatosEquipos = (DataTable)Session["DATAEQUIPOS"];
                    }


                    msgCatEquiposQueryResponse vItemsEquipos = vConnect.getCatEquipos("1");

                    foreach (msgCatEquiposQueryResponseItem item in vItemsEquipos.resultSet1)
                    {
                        if (DDLEquipo.SelectedValue.Equals(item.idCatEquipo))
                        {
                            Boolean isIn = false;
                            foreach (DataRow itemRow in vDatosEquipos.Rows)
                            {
                                if (itemRow["idCatEquipo"].ToString().Equals(item.idCatEquipo))
                                    isIn = true;
                            }
                            if (!isIn)
                            {
                                vDatosEquipos.Rows.Add(
                                    "",
                                    item.idCatEquipo,
                                    item.nombre,
                                    item.ip,
                                    item.tipoEquipo);
                            }
                        }
                    }

                    GVEquipos.DataSource = vDatosEquipos;
                    GVEquipos.DataBind();
                    Session["DATAEQUIPOS"] = vDatosEquipos;
                    LimpiarEquipos();

                }
                catch (Exception Ex) { LbMensajeSistemas.Text = Ex.Message; UpdateSistemasMensaje.Update(); }

                LimpiarSistemas();
                CerrarModal("SistemasModal");
            }
            catch (Exception Ex) { LbMensajeSistemas.Text = Ex.Message; UpdateSistemasMensaje.Update(); }
        }

        void LimpiarSistemas()
        {
            DDLSistema.SelectedIndex = 0;
            TxSistemaDescripcion.Text = "";
            DDLSistemaDenegacion.SelectedIndex = 0;
            TxSistemaTiempoInicio.Text = "";
            TxSistemaTiempoFinal.Text = "";
            LbMensajeSistemas.Text = "";
            UpdateSistemasMensaje.Update();
        }

        protected void GVSistemas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "DeleteRow")
                {
                    string vIdSistemas = e.CommandArgument.ToString();
                    if (Session["DATASISTEMAS"] != null)
                    {
                        vDatosSistemas = (DataTable)Session["DATASISTEMAS"];

                        DataRow[] result = vDatosSistemas.Select("sistema = '" + vIdSistemas + "'");
                        foreach (DataRow row in result)
                        {
                            if (row["sistema"].ToString().Contains(vIdSistemas))
                                vDatosSistemas.Rows.Remove(row);
                        }
                    }
                }
                GVSistemas.DataSource = vDatosSistemas;
                GVSistemas.DataBind();
                Session["DATASISTEMAS"] = vDatosSistemas;
            }
            catch (Exception Ex) { LbMensajeSistemas.Text = Ex.Message; UpdateSistemasMensaje.Update(); }
        }

        void LimpiarEquipos()
        {
            DDLEquipo.SelectedIndex = 0;
            LbMensajeSistemas.Text = "";
            UpdateSistemasMensaje.Update();
            DIVSistema.Visible = false;
        }

        protected void BtnCrearEquipos_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["DATAEQUIPOS"] == null)
                {
                    vDatosEquipos = new DataTable();
                    vDatosEquipos.Columns.Add("idEquipo");
                    vDatosEquipos.Columns.Add("idCatEquipo");
                    vDatosEquipos.Columns.Add("nombre");
                    vDatosEquipos.Columns.Add("ip");
                    vDatosEquipos.Columns.Add("tipoEquipo");
                }
                else
                {
                    vDatosEquipos = (DataTable)Session["DATAEQUIPOS"];
                }

                HttpService vConnect = new HttpService();
                msgCatEquiposQueryResponse vItems = vConnect.getCatEquipos("1");

                foreach (msgCatEquiposQueryResponseItem item in vItems.resultSet1)
                {
                    if (DDLEquipoSecundario.SelectedValue.Equals(item.idCatEquipo))
                    {
                        Boolean isIn = false;
                        foreach (DataRow itemRow in vDatosEquipos.Rows)
                        {
                            if (itemRow["idCatEquipo"].ToString().Equals(item.idCatEquipo))
                                isIn = true;
                        }
                        if (!isIn)
                        {
                            vDatosEquipos.Rows.Add(
                                "",
                                item.idCatEquipo,
                                item.nombre,
                                item.ip,
                                item.tipoEquipo);
                        }
                    }
                }

                GVEquipos.DataSource = vDatosEquipos;
                GVEquipos.DataBind();
                Session["DATAEQUIPOS"] = vDatosEquipos;
                LimpiarEquiposSecundarios();
                CerrarModal("EquiposModal");

            }
            catch (Exception Ex) { LbMensajeSistemas.Text = Ex.Message; UpdateSistemasMensaje.Update(); }
        }
        void LimpiarEquiposSecundarios()
        {
            DDLEquipoSecundario.SelectedIndex = 0;
            LbEquiposMensaje.Text = "";
            UpdateEquiposMensaje.Update();
        }
        protected void GVEquipos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "DeleteRow")
                {
                    string vIdEquipo = e.CommandArgument.ToString();
                    if (Session["DATAEQUIPOS"] != null)
                    {
                        vDatosEquipos = (DataTable)Session["DATAEQUIPOS"];

                        DataRow[] result = vDatosEquipos.Select("idCatEquipo = " + vIdEquipo);
                        foreach (DataRow row in result)
                        {
                            if (row["idCatEquipo"].ToString().Contains(vIdEquipo))
                                vDatosEquipos.Rows.Remove(row);
                        }
                    }
                }
                GVEquipos.DataSource = vDatosEquipos;
                GVEquipos.DataBind();
                Session["DATAEQUIPOS"] = vDatosEquipos;
            }
            catch (Exception Ex) { LbMensajeSistemas.Text = Ex.Message; }
        }

        protected void BtnCrearPersonal_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["DATAPERSONAL"] == null)
                {
                    vDatosPersonal = new DataTable();
                    vDatosPersonal.Columns.Add("idPersonal");
                    vDatosPersonal.Columns.Add("nombre");
                    vDatosPersonal.Columns.Add("cargo");
                }
                else
                {
                    vDatosPersonal = (DataTable)Session["DATAPERSONAL"];
                }
                vDatosPersonal.Rows.Add(
                    "",
                    DDLPersonalNombre.SelectedValue,
                    TxPersonalCargo.Text); 

                GVPersonal.DataSource = vDatosPersonal;
                GVPersonal.DataBind();
                Session["DATAPERSONAL"] = vDatosPersonal;
                LimpiartPersonal();
                CerrarModal("PersonalModal");
            }
            catch (Exception Ex) { LbMensajePersonal.Text = Ex.Message; UpdatePersonalMensaje.Update(); }
        }

        void LimpiartPersonal()
        {
            DDLPersonalNombre.SelectedIndex = 0;
            TxPersonalCargo.Text = "";
            LbMensajePersonal.Text = "";
            UpdatePersonalMensaje.Update();
        }

        protected void GVPersonal_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "DeleteRow")
                {
                    string vIdPersonal = e.CommandArgument.ToString();
                    if (Session["DATAPERSONAL"] != null)
                    {
                        vDatosPersonal = (DataTable)Session["DATAPERSONAL"];

                        DataRow[] result = vDatosPersonal.Select("nombre = '" + vIdPersonal + "'");
                        foreach (DataRow row in result)
                        {
                            if (row["nombre"].ToString().Contains(vIdPersonal))
                                vDatosPersonal.Rows.Remove(row);
                        }
                    }
                }
                GVPersonal.DataSource = vDatosPersonal;
                GVPersonal.DataBind();
                Session["DATAPERSONAL"] = vDatosPersonal;
            }
            catch (Exception Ex) { LbMensajePersonal.Text = Ex.Message; }
        }

        protected void BtnCrearComunicaciones_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["DATACOMUNICACIONES"] == null)
                {
                    vDatosComunicaciones = new DataTable();
                    vDatosComunicaciones.Columns.Add("idComunicacion");
                    vDatosComunicaciones.Columns.Add("cambio");
                    vDatosComunicaciones.Columns.Add("incidente");
                }
                else
                {
                    vDatosComunicaciones = (DataTable)Session["DATACOMUNICACIONES"];
                }
                vDatosComunicaciones.Rows.Add(
                    "",
                    TxComunicacionesCambio.Text,
                    TxComunicacionesIncidente.Text);

                GVComunicaciones.DataSource = vDatosComunicaciones;
                GVComunicaciones.DataBind();
                Session["DATACOMUNICACIONES"] = vDatosComunicaciones;
                LimpiarComunicaciones();
                CerrarModal("ComunicacionesModal");
            }
            catch (Exception Ex) { LbMensajeComunicaciones.Text = Ex.Message; UpdateComunicacionesMensaje.Update(); }
        }

        void LimpiarComunicaciones()
        {
            TxComunicacionesCambio.Text = "";
            TxComunicacionesIncidente.Text = "";
            LbMensajeComunicaciones.Text = "";
            UpdateComunicacionesMensaje.Update();
        }

        protected void GVComunicaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "DeleteRow")
                {
                    string vIdComunicaciones = e.CommandArgument.ToString();
                    if (Session["DATACOMUNICACIONES"] != null)
                    {
                        vDatosComunicaciones = (DataTable)Session["DATACOMUNICACIONES"];

                        DataRow[] result = vDatosComunicaciones.Select("cambio = '" + vIdComunicaciones + "'");
                        foreach (DataRow row in result)
                        {
                            if (row["cambio"].ToString().Contains(vIdComunicaciones))
                                vDatosComunicaciones.Rows.Remove(row);
                        }
                    }
                }
                GVComunicaciones.DataSource = vDatosComunicaciones;
                GVComunicaciones.DataBind();
                Session["DATACOMUNICACIONES"] = vDatosComunicaciones;
            }
            catch (Exception Ex) { LbMensajeComunicaciones.Text = Ex.Message; }
        }

        protected void BtnCrearProcedimientos_Click(object sender, EventArgs e)
        {
            try
            {
                if (DDLProcedimientosResponsable.SelectedIndex == 0)
                    throw new Exception("Por favor seleccione el responsable del procedimiento");


                if (Session["DATAPROCEDIMIENTOS"] == null)
                {
                    vDatosProcedimientos = new DataTable();
                    vDatosProcedimientos.Columns.Add("idProcedimiento");
                    vDatosProcedimientos.Columns.Add("responsable");
                    vDatosProcedimientos.Columns.Add("detalle");
                    vDatosProcedimientos.Columns.Add("inicio");
                    vDatosProcedimientos.Columns.Add("fin");
                }
                else
                {
                    vDatosProcedimientos = (DataTable)Session["DATAPROCEDIMIENTOS"];
                }
                vDatosProcedimientos.Rows.Add(
                    "",
                    DDLProcedimientosResponsable.SelectedValue,
                    TxProcedimientosDetalle.Text,
                    TxProcedimientosInicio.Text,
                    TxProcedimientosFin.Text);

                GVProcedimientos.DataSource = vDatosProcedimientos;
                GVProcedimientos.DataBind();
                Session["DATAPROCEDIMIENTOS"] = vDatosProcedimientos;
                LimpiarProcedimientos();
                CerrarModal("ProcedimientosModal");
            }
            catch (Exception Ex) { LbMensajeProcedimientos.Text = Ex.Message; UpdateProcedimientosMensaje.Update(); }
        }

        void LimpiarProcedimientos()
        {
            DDLProcedimientosResponsable.SelectedIndex = 0;
            TxProcedimientosDetalle.Text = "";
            TxProcedimientosInicio.Text = "";
            TxProcedimientosFin.Text = "";
            LbMensajeProcedimientos.Text = "";
            UpdateProcedimientosMensaje.Update();
        }

        protected void GVProcedimientos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "DeleteRow")
                {
                    string vIdProcedimientos = e.CommandArgument.ToString();
                    if (Session["DATAPROCEDIMIENTOS"] != null)
                    {
                        vDatosProcedimientos = (DataTable)Session["DATAPROCEDIMIENTOS"];

                        DataRow[] result = vDatosProcedimientos.Select("detalle = '" + vIdProcedimientos + "'");
                        foreach (DataRow row in result)
                        {
                            if (row["detalle"].ToString().Contains(vIdProcedimientos))
                                vDatosProcedimientos.Rows.Remove(row);
                        }
                    }
                }
                GVProcedimientos.DataSource = vDatosProcedimientos;
                GVProcedimientos.DataBind();
                Session["DATAPROCEDIMIENTOS"] = vDatosProcedimientos;
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnCrearRollback_Click(object sender, EventArgs e)
        {
            try
            {
                if (DDLRollBackResponsable.SelectedIndex == 0)
                    throw new Exception("Por favor seleccion un responsable para el rollback");


                if (Session["DATAROLLBACK"] == null)
                {
                    vDatosRollback = new DataTable();
                    vDatosRollback.Columns.Add("idRollback");
                    vDatosRollback.Columns.Add("responsable");
                    vDatosRollback.Columns.Add("detalle");
                    //vDatosRollback.Columns.Add("inicio");
                    //vDatosRollback.Columns.Add("fin");
                }
                else
                {
                    vDatosRollback = (DataTable)Session["DATAROLLBACK"];
                }
                vDatosRollback.Rows.Add(
                    "",
                    DDLRollBackResponsable.SelectedValue,
                    TxRollBackDetalle.Text);
                    //TxRollBackInicio.Text,
                    //TxRollBackFin.Text);

                GVRollback.DataSource = vDatosRollback;
                GVRollback.DataBind();
                Session["DATAROLLBACK"] = vDatosRollback;
                LimpiarRollback();
                CerrarModal("RollbackModal");
            }
            catch (Exception Ex) { LbMensajeRollback.Text = Ex.Message; UpdateRollbackMensaje.Update(); }
        }

        void LimpiarRollback()
        {
            DDLRollBackResponsable.SelectedIndex = 0;
            TxRollBackDetalle.Text = "";
            LbMensajeRollback.Text = "";
            UpdateRollbackMensaje.Update();
        }    

        protected void GVRollback_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "DeleteRow")
                {
                    string vIdRollback = e.CommandArgument.ToString();
                    if (Session["DATAROLLBACK"] != null)
                    {
                        vDatosRollback = (DataTable)Session["DATAROLLBACK"];

                        DataRow[] result = vDatosRollback.Select("detalle = '" + vIdRollback + "'");
                        foreach (DataRow row in result)
                        {
                            if (row["detalle"].ToString().Contains(vIdRollback))
                                vDatosRollback.Rows.Remove(row);
                        }
                    }
                }
                GVRollback.DataSource = vDatosRollback;
                GVRollback.DataBind();
                Session["DATAROLLBACK"] = vDatosRollback;
            }
            catch (Exception Ex) { LbMensajeRollback.Text = Ex.Message; }
        }

        protected void BtnCrearPruebas_Click(object sender, EventArgs e)
        {
            try
            {
                if (DDLPruebasResponsable.SelectedIndex == 0)
                    throw new Exception("Por favor seleccion un responsable para las pruebas");

                if (Session["DATAPRUEBAS"] == null)
                {
                    vDatosPruebas = new DataTable();
                    vDatosPruebas.Columns.Add("idPrueba");
                    vDatosPruebas.Columns.Add("responsable");
                    vDatosPruebas.Columns.Add("detalle");
                }
                else
                {
                    vDatosPruebas = (DataTable)Session["DATAPRUEBAS"];
                }
                vDatosPruebas.Rows.Add(
                    "",
                    DDLPruebasResponsable.SelectedValue,
                    TxPruebasDetalle.Text); ;

                GVPruebas.DataSource = vDatosPruebas;
                GVPruebas.DataBind();
                Session["DATAPRUEBAS"] = vDatosPruebas;
                LimpiarPruebas();
                CerrarModal("PruebasModal");
            }
            catch (Exception Ex) { LbMensajePruebas.Text = Ex.Message; UpdatePruebasMensaje.Update(); }
        }

        void LimpiarPruebas()
        {
            DDLPruebasResponsable.SelectedIndex = 0;
            TxPruebasDetalle.Text = "";
            LbMensajePruebas.Text = "";
            UpdatePruebasMensaje.Update();
        }

        protected void GVPruebas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "DeleteRow")
                {
                    string vIdPruebas = e.CommandArgument.ToString();
                    if (Session["DATAPRUEBAS"] != null)
                    {
                        vDatosPruebas = (DataTable)Session["DATAPRUEBAS"];

                        DataRow[] result = vDatosPruebas.Select("responsable = '" + vIdPruebas + "'");
                        foreach (DataRow row in result)
                        {
                            if (row["responsable"].ToString().Contains(vIdPruebas))
                                vDatosPruebas.Rows.Remove(row);
                        }
                    }
                }
                GVPruebas.DataSource = vDatosPruebas;
                GVPruebas.DataBind();
                Session["DATAPRUEBAS"] = vDatosPruebas;
            }
            catch (Exception Ex) { LbMensajePruebas.Text = Ex.Message; }
        }
        #endregion

        #region "Obtener Datos"
        String ObtenerCriticidad()
        {
            String vResultado = "";
            try
            {
                if (RBCriticidadAlta.Checked)
                    vResultado = RBCriticidadAlta.Value;
                if (RBCriticidadMedia.Checked)
                    vResultado = RBCriticidadMedia.Value;
                if (RBCriticidadBaja.Checked)
                    vResultado = RBCriticidadBaja.Value;
            }
            catch { vResultado = "1"; }
            return vResultado;
        }
        String ObtenerCriticidadQA()
        {
            String vResultado = "";
            try
            {
                if (CriticidadRadio1.Checked)
                    vResultado = CriticidadRadio1.Value;
                if (CriticidadRadio2.Checked)
                    vResultado = CriticidadRadio2.Value;
                if (CriticidadRadio3.Checked)
                    vResultado = CriticidadRadio3.Value;
            }
            catch { vResultado = "1"; }
            return vResultado;
        }

        String ObtenerImpacto()
        {
            String vResultado = "";
            try
            {
                if (RBImpactoAlta.Checked)
                    vResultado = RBImpactoAlta.Value;
                if (RBImpactoMedia.Checked)
                    vResultado = RBImpactoMedia.Value;
                if (RBImpactoBaja.Checked)
                    vResultado = RBImpactoBaja.Value;
            }
            catch { vResultado = "1"; }
            return vResultado;
        }
        String ObtenerImpactoQA()
        {
            String vResultado = "";
            try
            {
                if (ImpactoRadio1.Checked)
                    vResultado = ImpactoRadio1.Value;
                if (ImpactoRadio2.Checked)
                    vResultado = ImpactoRadio2.Value;
                if (ImpactoRadio3.Checked)
                    vResultado = ImpactoRadio3.Value;
            }
            catch { vResultado = "1"; }
            return vResultado;
        }

        String ObtenerRiesgo()
        {
            String vResultado = "";
            try
            {
                if (RBRiesgoAlta.Checked)
                    vResultado = RBRiesgoAlta.Value;
                if (RBRiesgoMedia.Checked)
                    vResultado = RBRiesgoMedia.Value;
                if (RBRiesgoBaja.Checked)
                    vResultado = RBRiesgoBaja.Value;
            }
            catch { vResultado = "1"; }
            return vResultado;
        }
        String ObtenerRiesgoQA()
        {
            String vResultado = "";
            try
            {
                if (RiesgoRadio1.Checked)
                    vResultado = RiesgoRadio1.Value;
                if (RiesgoRadio2.Checked)
                    vResultado = RiesgoRadio2.Value;
                if (RiesgoRadio3.Checked)
                    vResultado = RiesgoRadio3.Value;
            }
            catch { vResultado = "1"; }
            return vResultado;
        }

        String ObtenerTipoCambio()
        {
            String vResultado = "";
            try
            {
                if (RBMantenimientoTipoCambio1.Checked)
                    vResultado = RBMantenimientoTipoCambio1.Value;
                if (RBMantenimientoTipoCambio2.Checked)
                    vResultado = RBMantenimientoTipoCambio2.Value;
                if (RBMantenimientoTipoCambio3.Checked)
                    vResultado = RBMantenimientoTipoCambio3.Value;
            }
            catch { vResultado = "1"; }
            return vResultado;
        }

        string ObtenerLugarMantenimiento()
        {
            String vResultado = "";
            try
            {
                if (RBLugarMantenimiento1.Checked)
                    vResultado = RBLugarMantenimiento1.Value;
                if (RBLugarMantenimiento2.Checked)
                    vResultado = RBLugarMantenimiento2.Value;
                if (RBLugarMantenimiento3.Checked)
                    vResultado = RBLugarMantenimiento3.Value;
                if (RBLugarMantenimiento4.Checked)
                    vResultado = RBLugarMantenimiento4.Value;
                if (RBLugarMantenimiento5.Checked)
                    vResultado = RBLugarMantenimiento5.Value;
            }
            catch { vResultado = "1"; }
            return vResultado;
        }
        #endregion

        protected void BtnCrearUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                if (DDLUsuarios.SelectedIndex == 0)
                {
                    if (!CbTodosQA.Checked)
                    {
                        throw new Exception("Por favor seleccione un usuario valido o presiente la casilla de todos");
                        CerrarModal("UsuarioModal");
                    }
                }
                else
                {
                    BtnAsignarUsuario.Text = DDLUsuarios.SelectedValue;
                    BtnAsignarUsuario.CssClass = "btn btn-success mr-2 ";
                    Session["USUARIOASIGNADO"] = DDLUsuarios.SelectedValue;
                }
                CerrarModal("UsuarioModal");
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnGuardarCambio_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                Validaciones();

                Generales vGenerales = new Generales();

                if (!vGenerales.PermisosEntrada(Permisos.Promotor, vConfigurations.resultSet1[0].idCargo) && !vGenerales.PermisosEntrada(Permisos.Implementador, vConfigurations.resultSet1[0].idCargo))
                    throw new Exception("No tienes permisos para realizar esta accion.");
             


                if (Convert.ToBoolean(Session["CIERRE"]))
                {
                    CierreCambio();
                }
                else
                {
                    if (ObtenerTipoCambio().Equals("1"))
                    {
                        LbWarningTipoCambio.Text = "Estas seleccionando el cambio como Estandard, ¿estas seguro de esto?";
                        UpdateMensajeWarning.Update();
                    }
                    


                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        private void InsertarPruebas(HttpService vConector, string vIdCambioCreado)
        {
            DataTable vDatosPruebasGrid = (DataTable)Session["DATAPRUEBAS"];

            for (int i = 0; i < vDatosPruebasGrid.Rows.Count; i++)
            {

                msgInfoPruebas vRequestPruebas = new msgInfoPruebas()
                {
                    tipo = "1",
                    idprueba = "",
                    descripcion = vDatosPruebasGrid.Rows[i]["detalle"].ToString(),
                    responsable = vDatosPruebasGrid.Rows[i]["responsable"].ToString(),
                    usuario = vConfigurations.resultSet1[0].idUsuario
                };

                String vResponsePruebas = "";
                HttpResponseMessage vHttpResponsePruebas = vConector.PostInfoPruebas(vRequestPruebas, ref vResponsePruebas);
                if (vHttpResponsePruebas.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoPruebasCreateResponse vInfoPruebasResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoPruebasCreateResponse>(vResponsePruebas);
                    if (vInfoPruebasResponse.updateCount1.Equals("1"))
                    {

                        //INGRESO DE IDCAMBIO REL IDEQUIPOS
                        msgRelacionesCambios vRelPruebasRequest = new msgRelacionesCambios()
                        {
                            tipo = "8",
                            subtipo = "1", //CREATE
                            principal = vIdCambioCreado,
                            secundario = vInfoPruebasResponse.resultSet1[0].idPrueba
                        };

                        String vRelPruebasResponse = "";
                        HttpResponseMessage vHttpResponseRelPruebas = vConector.PostRelacionesCambios(vRelPruebasRequest, ref vRelPruebasResponse);

                    }
                }
            }
        }

        private void InsertarRollback(HttpService vConector, string vIdCambioCreado)
        {
            DataTable vDatosRollbackGrid = (DataTable)Session["DATAROLLBACK"];

            for (int i = 0; i < vDatosRollbackGrid.Rows.Count; i++)
            {
                String vInicioGrid = "";
                String vFinalGrid = "";
                //if (!vDatosRollbackGrid.Rows[i]["inicio"].ToString().Equals(""))
                //    vInicioGrid = Convert.ToDateTime(vDatosRollbackGrid.Rows[i]["inicio"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                //if (!vDatosRollbackGrid.Rows[i]["fin"].ToString().Equals(""))
                //    vFinalGrid = Convert.ToDateTime(vDatosRollbackGrid.Rows[i]["fin"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");


                msgInfoRollbacks vRequestRollback = new msgInfoRollbacks()
                {
                    tipo = "1",
                    idrollback = "",
                    descripcion = vDatosRollbackGrid.Rows[i]["detalle"].ToString(),
                    responsable = vDatosRollbackGrid.Rows[i]["responsable"].ToString(),
                    inicio = vInicioGrid,
                    final = vFinalGrid,
                    usuario = vConfigurations.resultSet1[0].idUsuario
                };

                String vResponseRollback = "";
                HttpResponseMessage vHttpResponseRollback = vConector.PostInfoRollbacks(vRequestRollback, ref vResponseRollback);
                if (vHttpResponseRollback.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoRollbacksCreateResponse vInfoRollbackResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoRollbacksCreateResponse>(vResponseRollback);
                    if (vInfoRollbackResponse.updateCount1.Equals("1"))
                    {

                        //INGRESO DE IDCAMBIO REL IDEQUIPOS
                        msgRelacionesCambios vRelRollbackRequest = new msgRelacionesCambios()
                        {
                            tipo = "9",
                            subtipo = "1", //CREATE
                            principal = vIdCambioCreado,
                            secundario = vInfoRollbackResponse.resultSet1[0].idRollback
                        };

                        String vRelRollbackResponse = "";
                        HttpResponseMessage vHttpResponseRelRollback = vConector.PostRelacionesCambios(vRelRollbackRequest, ref vRelRollbackResponse);

                    }
                }
            }
        }

        private void InsertarProcedimientos(HttpService vConector, string vIdCambioCreado)
        {
            DataTable vDatosProcedimientosGrid = (DataTable)Session["DATAPROCEDIMIENTOS"];

            for (int i = 0; i < vDatosProcedimientosGrid.Rows.Count; i++)
            {
                String vInicioGrid = "";
                String vFinalGrid = "";
                if (!vDatosProcedimientosGrid.Rows[i]["inicio"].ToString().Equals(""))
                    vInicioGrid = Convert.ToDateTime(vDatosProcedimientosGrid.Rows[i]["inicio"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                if (!vDatosProcedimientosGrid.Rows[i]["fin"].ToString().Equals(""))
                    vFinalGrid = Convert.ToDateTime(vDatosProcedimientosGrid.Rows[i]["fin"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");


                msgInfoProcedimientos vRequestProcedimientos = new msgInfoProcedimientos()
                {
                    tipo = "1",
                    idprocedimiento = "",
                    descripcion = vDatosProcedimientosGrid.Rows[i]["detalle"].ToString(),
                    responsable = vDatosProcedimientosGrid.Rows[i]["responsable"].ToString(),
                    inicio = vInicioGrid,
                    final = vFinalGrid,
                    usuario = vConfigurations.resultSet1[0].idUsuario
                };

                String vResponseProcedimientos = "";
                HttpResponseMessage vHttpResponseProcedimientos = vConector.PostInfoProcedimientos(vRequestProcedimientos, ref vResponseProcedimientos);
                if (vHttpResponseProcedimientos.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoProcedimientosCreateResponse vInfoProcedimientosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoProcedimientosCreateResponse>(vResponseProcedimientos);
                    if (vInfoProcedimientosResponse.updateCount1.Equals("1"))
                    {

                        //INGRESO DE IDCAMBIO REL IDEQUIPOS
                        msgRelacionesCambios vRelProcedimientosRequest = new msgRelacionesCambios()
                        {
                            tipo = "7",
                            subtipo = "1", //CREATE
                            principal = vIdCambioCreado,
                            secundario = vInfoProcedimientosResponse.resultSet1[0].idProcedimiento
                        };

                        String vRelProcedimientosResponse = "";
                        HttpResponseMessage vHttpResponseRelProcedimientos = vConector.PostRelacionesCambios(vRelProcedimientosRequest, ref vRelProcedimientosResponse);

                    }
                }
            }
        }

        private void InsertarComunicaciones(HttpService vConector, string vIdCambioCreado)
        {
            DataTable vDatosComunicacionesGrid = (DataTable)Session["DATACOMUNICACIONES"];

            for (int i = 0; i < vDatosComunicacionesGrid.Rows.Count; i++)
            {
                msgInfoComunicaciones vRequestComunicaciones = new msgInfoComunicaciones()
                {
                    tipo = "1",
                    idcomunicacion = "",
                    cambionormal = vDatosComunicacionesGrid.Rows[i]["cambio"].ToString(),
                    casoincidente = vDatosComunicacionesGrid.Rows[i]["incidente"].ToString(),
                    usuario = vConfigurations.resultSet1[0].idUsuario
                };

                String vResponsComunicaciones = "";
                HttpResponseMessage vHttpResponseComunicaciones = vConector.PostInfoComunicaciones(vRequestComunicaciones, ref vResponsComunicaciones);
                if (vHttpResponseComunicaciones.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoComunicacionesCreateResponse vInfoComunicacionesResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoComunicacionesCreateResponse>(vResponsComunicaciones);
                    if (vInfoComunicacionesResponse.updateCount1.Equals("1"))
                    {

                        //INGRESO DE IDCAMBIO REL IDEQUIPOS
                        msgRelacionesCambios vRelPersonalRequest = new msgRelacionesCambios()
                        {
                            tipo = "4",
                            subtipo = "1", //CREATE
                            principal = vIdCambioCreado,
                            secundario = vInfoComunicacionesResponse.resultSet1[0].idComunicacion
                        };

                        String vRelComunicacionesResponse = "";
                        HttpResponseMessage vHttpResponseRelComunicaciones = vConector.PostRelacionesCambios(vRelPersonalRequest, ref vRelComunicacionesResponse);

                    }
                }
            }
        }

        private void InsertarPersonal(HttpService vConector, string vIdCambioCreado)
        {
            DataTable vDatosPersonalGrid = (DataTable)Session["DATAPERSONAL"];
            for (int i = 0; i < vDatosPersonalGrid.Rows.Count; i++)
            {
                msgInfoPersonal vRequestPersonal = new msgInfoPersonal()
                {
                    tipo = "1",
                    idpersonal = "",
                    nombre = vDatosPersonalGrid.Rows[i]["nombre"].ToString(),
                    cargo = vDatosPersonalGrid.Rows[i]["cargo"].ToString(),
                    estado = "1",
                    usuario = vConfigurations.resultSet1[0].idUsuario
                };

                String vResponsPersonal = "";
                HttpResponseMessage vHttpResponsePersonal = vConector.PostInfoPersonal(vRequestPersonal, ref vResponsPersonal);
                if (vHttpResponsePersonal.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoPersonalCreateResponse vInfoPersonalResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoPersonalCreateResponse>(vResponsPersonal);
                    if (vInfoPersonalResponse.updateCount1.Equals("1"))
                    {

                        //INGRESO DE IDCAMBIO REL IDEQUIPOS
                        msgRelacionesCambios vRelPersonalRequest = new msgRelacionesCambios()
                        {
                            tipo = "6",
                            subtipo = "1", //CREATE
                            principal = vIdCambioCreado,
                            secundario = vInfoPersonalResponse.resultSet1[0].idPersonal
                        };

                        String vRelPersonalResponse = "";
                        HttpResponseMessage vHttpResponseRelPersonal = vConector.PostRelacionesCambios(vRelPersonalRequest, ref vRelPersonalResponse);

                    }
                }
            }
        }

        private void InsertarEquipos(HttpService vConector, string vIdCambioCreado)
        {
            DataTable vDatosEquiposGrid = (DataTable)Session["DATAEQUIPOS"];
            for (int i = 0; i < vDatosEquiposGrid.Rows.Count; i++)
            {
                msgInfoEquipos vRequestEquipos = new msgInfoEquipos()
                {
                    tipo = "1",
                    idequipo = "",
                    nombre = vDatosEquiposGrid.Rows[i]["nombre"].ToString(),
                    ip = vDatosEquiposGrid.Rows[i]["ip"].ToString(),
                    tipoequipo = vDatosEquiposGrid.Rows[i]["tipoEquipo"].ToString(),
                    usuario = vConfigurations.resultSet1[0].idUsuario,
                    idcatequipo = vDatosEquiposGrid.Rows[i]["idCatEquipo"].ToString()
                };

                String vResponseEquipos = "";
                HttpResponseMessage vHttpResponseEquipos = vConector.PostInfoEquipos(vRequestEquipos, ref vResponseEquipos);
                if (vHttpResponseEquipos.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoEquiposCreateResponse vInfoEquiposResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoEquiposCreateResponse>(vResponseEquipos);
                    if (vInfoEquiposResponse.updateCount1.Equals("1"))
                    {

                        //INGRESO DE IDCAMBIO REL IDEQUIPOS
                        msgRelacionesCambios vRelEquiposRequest = new msgRelacionesCambios()
                        {
                            tipo = "5",
                            subtipo = "1", //CREATE
                            principal = vIdCambioCreado,
                            secundario = vInfoEquiposResponse.resultSet1[0].idEquipo
                        };

                        String vRelEquiposResponse = "";
                        HttpResponseMessage vHttpResponseRelEquipos = vConector.PostRelacionesCambios(vRelEquiposRequest, ref vRelEquiposResponse);

                    }
                }
            }
        }

        private void InsertarSistemas(HttpService vConector, string vIdCambioCreado)
        {
            DataTable vDatosSistemasGrid = (DataTable)Session["DATASISTEMAS"];

            for (int i = 0; i < vDatosSistemasGrid.Rows.Count; i++)
            {
                String vInicioGrig = "";
                String vFinalGrig = "";
                int vDenegacionGrid = (vDatosSistemasGrid.Rows[i]["denegacion"].ToString().Equals("SI") ? 1 : 0);
                if (!vDatosSistemasGrid.Rows[i]["inicio"].ToString().Equals(""))
                    vInicioGrig = Convert.ToDateTime(vDatosSistemasGrid.Rows[i]["inicio"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                if (!vDatosSistemasGrid.Rows[i]["final"].ToString().Equals(""))
                    vFinalGrig = Convert.ToDateTime(vDatosSistemasGrid.Rows[i]["final"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");

                msgInfoSistemas vRequestSistemas = new msgInfoSistemas()
                {
                    tipo = "1",
                    idcanal = "",
                    nombre = vDatosSistemasGrid.Rows[i]["sistema"].ToString(),
                    descripcion = vDatosSistemasGrid.Rows[i]["descripcion"].ToString(),
                    denegacion = vDenegacionGrid.ToString(),
                    inicio = vInicioGrig,
                    fin = vFinalGrig,
                    usuario = vConfigurations.resultSet1[0].idUsuario
                };

                String vResponseCanales = "";
                HttpResponseMessage vHttpResponseCanales = vConector.PostInfoCanales(vRequestSistemas, ref vResponseCanales);
                if (vHttpResponseCanales.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoSistemasCreateResponse vInfoCanalesResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoSistemasCreateResponse>(vResponseCanales);
                    if (vInfoCanalesResponse.updateCount1.Equals("1"))
                    {
                        //INGRESO DE IDCAMBIO REL IDCANALES
                        msgRelacionesCambios vRelCanalesRequest = new msgRelacionesCambios()
                        {
                            tipo = "3",
                            subtipo = "1", //CREATE
                            principal = vIdCambioCreado,
                            secundario = vInfoCanalesResponse.resultSet1[0].idCanal
                        };

                        String vRelCanalesResponse = "";
                        HttpResponseMessage vHttpResponseRelCanales = vConector.PostRelacionesCambios(vRelCanalesRequest, ref vRelCanalesResponse);
                    }
                }
            }
        }

        private void Validaciones()
        {
            if (DDLUsuarios.SelectedValue.Equals("0"))
            {
                if (ObtenerTipoCambio().Equals("2"))
                {
                    if (!CbTodosQA.Checked)
                        throw new Exception("Por favor seleccione un agente de QA o marque el campo todos en Asignar QA");
                }
                
            }
            if (TxNombreMantenimiento.Text.Trim().Equals(""))
                throw new Exception("Por favor ingrese un nombre para el cambio");
            if (TxMantenimientoDescripcion.Text.Trim().Equals(""))
                throw new Exception("Por favor ingrese una descripción para el cambio");           
            if (DDLProveedor.SelectedIndex == 0)
                throw new Exception("Seleccione un proveedor valido");
            //if (Session["USUARIOASIGNADO"] == null)
            //    throw new Exception("Por favor asigne el cambio a alguien");
            if (DDLTipoMantenimiento.SelectedIndex == 0)
                throw new Exception("Seleccione un tipo de mantenimiento valido");
            if (DDLSubtipoMantenimiento.SelectedIndex == 0)
                throw new Exception("Seleccione un subtipo de mantenimiento valido");
            if (TxVentanaDuracionInicio.Text.Equals(""))
                throw new Exception("Por favor ingrese una fecha valida en Ventana Inicio de Calendarios");
            if (TxVentanaDuracionFin.Text.Equals(""))
                throw new Exception("Por favor ingrese una fecha valida en Ventana Fin de Calendarios");

            if (ObtenerTipoCambio().Equals("3"))
            {
                if (TxTicket.Text.Equals(""))
                    throw new Exception("Tipo de cambio Emergencia, por favor escribe el No.Ticket asociado");
            }
        }

        protected void BtnCancelarCambio_Click(object sender, EventArgs e)
        {
            try
            {
                LimpiarSessiones();
                Response.Redirect("/pages/services/changes.aspx");
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        private void LimpiarSessiones()
        {
            Session["DATAEQUIPOS"] = null;
            Session["DATASISTEMAS"] = null;
            Session["DATAPERSONAL"] = null;
            Session["DATACOMUNICACIONES"] = null;
            Session["DATAPROCEDIMIENTOS"] = null;
            Session["DATAROLLBACK"] = null;
            Session["DATAPRUEBAS"] = null;
            Session["USUARIOASIGNADO"] = null;
            Session["GETIDCAMBIO"] = null;
            Session["MANTENIMIENTO"] = null;
            Session["CIERRE"] = null;

            Session["CABIMPLEMTADORES"] = null;

        }

        protected void DDLTipoMantenimiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DDLSubtipoMantenimiento.Items.Clear();
                HttpService vConnect = new HttpService();
                msgGeneralesResponse vItems = vConnect.getGenerales("8", DDLTipoMantenimiento.SelectedValue);

                DDLSubtipoMantenimiento.Items.Add(new ListItem { Text = "Seleccione una opción", Value = "0" });
                foreach (msgGeneralesResponseItem item in vItems.resultSet1)
                {
                    DDLSubtipoMantenimiento.Items.Add(new ListItem { Text = item.id + ". " + item.descripcion, Value = item.id });
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnProcedeCreacion_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                //SI ES MANTENIMIENTO
                if (Convert.ToBoolean(Session["MANTENIMIENTO"]))
                {

                    if (Convert.ToBoolean(Session["CIERRE"]))
                    {
                        CierreCambio();
                    }
                    else
                    {

                        String vGETCambio = Convert.ToString(Session["GETIDCAMBIO"]);
                        String vGETMentenimiento = Convert.ToString(Session["MANTENIMIENTOSESSION"]);
                        String vGETCalendario = Convert.ToString(Session["CALENDARIOSESSION"]);

                        msgInfoCambios vInfoCambiosRequest = new msgInfoCambios()
                        {
                            tipo = "2",
                            idcambio = vGETCambio,
                            nombre = TxNombreMantenimiento.Text,
                            proveedor = DDLProveedor.SelectedValue,
                            responsable = Convert.ToString(Session["USUARIOASIGNADO"]),
                            idresolucion = "0",
                            idcriticidad = ObtenerCriticidad(),
                            idimpacto = ObtenerImpacto(),
                            idriesgo = ObtenerRiesgo(),
                            observaciones = TxObservaciones.Text,
                            usuario = vConfigurations.resultSet1[0].idUsuario,
                            idmantenimiento = vGETMentenimiento,
                            idcalendario = vGETCalendario,
                            usuariogrud = vConfigurations.resultSet1[0].idUsuario,
                            paso = "1"
                        };
                        String vResponseCambios = "";
                        HttpResponseMessage vHttpResponseCambios = vConector.PostInfoCambios(vInfoCambiosRequest, ref vResponseCambios);

                        if (vHttpResponseCambios.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            msgUpdateGeneral vInfoCambiosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseCambios);
                            if (vInfoCambiosResponse.updateCount1.Equals("1"))
                            {
                                //MANTENIMIENTOS
                                msgInfoMantenimientos vRequestMantenimiento = new msgInfoMantenimientos()
                                {
                                    tipo = "2",
                                    idmantenimiento = vGETMentenimiento,
                                    descripcion = TxMantenimientoDescripcion.Text,
                                    idtipocambio = ObtenerTipoCambio(),
                                    idlugar = ObtenerLugarMantenimiento(),
                                    otros = "",
                                    usuario = vConfigurations.resultSet1[0].idUsuario
                                };

                                String vResponseMantenimientos = "";
                                HttpResponseMessage vHttpResponseMantenimientos = vConector.PostInfoMantenimientos(vRequestMantenimiento, ref vResponseMantenimientos);
                                if (vHttpResponseMantenimientos.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    msgUpdateGeneral vInfoMantenimientosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseMantenimientos);
                                    if (vInfoMantenimientosResponse.updateCount1.Equals("1"))
                                    {
                                        //VALIDADOR
                                    }
                                }

                                //CALENDARIOS
                                String vDenegacionInicio = "";
                                String vDenegacionFin = "";
                                if (!TxVentanaDenegacionInicio.Text.Equals(""))
                                    vDenegacionInicio = Convert.ToDateTime(TxVentanaDenegacionInicio.Text).ToString("yyyy-MM-dd HH:mm:ss");
                                if (!TxVentanaDenegacionFin.Text.Equals(""))
                                    vDenegacionFin = Convert.ToDateTime(TxVentanaDenegacionFin.Text).ToString("yyyy-MM-dd HH:mm:ss");

                                msgInfoCalendarios vRequestCalendarios = new msgInfoCalendarios()
                                {
                                    tipo = "2",
                                    idcalendario = vGETCalendario,
                                    descripcion = TxNombreMantenimiento.Text + DateTime.Now,
                                    fechaventana = Convert.ToDateTime(TxVentanaDuracionInicio.Text).ToString("yyyy-MM-dd"),
                                    ventanainicio = Convert.ToDateTime(TxVentanaDuracionInicio.Text).ToString("yyyy-MM-dd HH:mm:ss"),
                                    ventanafin = Convert.ToDateTime(TxVentanaDuracionFin.Text).ToString("yyyy-MM-dd HH:mm:ss"),
                                    denegacioninicio = vDenegacionInicio,
                                    denegacionfin = vDenegacionFin,
                                    usuario = vConfigurations.resultSet1[0].idUsuario
                                };

                                String vResponseInfoCalendarios = "";
                                HttpResponseMessage vHttpResponseInfoCalendarios = vConector.PostInfoCalendarios(vRequestCalendarios, ref vResponseInfoCalendarios);
                                if (vHttpResponseInfoCalendarios.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    msgUpdateGeneral vInfoCalendariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseInfoCalendarios);
                                    if (vInfoCalendariosResponse.updateCount1.Equals("1"))
                                    {
                                        //VALIDADOR
                                    }
                                }

                                //SISTEMAS
                                msgInfoSistemas vRequestSistemas = new msgInfoSistemas()
                                {
                                    tipo = "0",
                                    idcanal = vGETCambio,
                                };

                                String vResponseCanales = "";
                                HttpResponseMessage vHttpResponseCanales = vConector.PostInfoCanales(vRequestSistemas, ref vResponseCanales);
                                if (vHttpResponseCanales.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    msgUpdateGeneral vInfoCanalesResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseCanales);
                                    if (vInfoCanalesResponse.updateCount1.Equals("1"))
                                    {
                                        InsertarSistemas(vConector, vGETCambio);
                                    }
                                    else
                                    {
                                        if (GVSistemas.Rows.Count > 0)
                                            InsertarSistemas(vConector, vGETCambio);
                                    }
                                }

                                //EQUIPOS
                                msgInfoEquipos vRequestEquipos = new msgInfoEquipos()
                                {
                                    tipo = "0",
                                    idequipo = vGETCambio
                                };

                                String vResponseEquipos = "";
                                HttpResponseMessage vHttpResponseEquipos = vConector.PostInfoEquipos(vRequestEquipos, ref vResponseEquipos);
                                if (vHttpResponseEquipos.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    msgUpdateGeneral vInfoEquiposResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseEquipos);
                                    if (vInfoEquiposResponse.updateCount1.Equals("1"))
                                    {
                                        InsertarEquipos(vConector, vGETCambio);
                                    }
                                    else
                                    {
                                        if (GVEquipos.Rows.Count > 0)
                                            InsertarEquipos(vConector, vGETCambio);
                                    }
                                }

                                //PERSONAL
                                msgInfoPersonal vRequestPersonal = new msgInfoPersonal()
                                {
                                    tipo = "0",
                                    idpersonal = vGETCambio
                                };

                                String vResponsPersonal = "";
                                HttpResponseMessage vHttpResponsePersonal = vConector.PostInfoPersonal(vRequestPersonal, ref vResponsPersonal);
                                if (vHttpResponsePersonal.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    msgUpdateGeneral vInfoPersonalResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponsPersonal);
                                    if (vInfoPersonalResponse.updateCount1.Equals("1"))
                                    {
                                        InsertarPersonal(vConector, vGETCambio);
                                    }
                                    else
                                    {
                                        if (GVPersonal.Rows.Count > 0)
                                            InsertarPersonal(vConector, vGETCambio);
                                    }
                                }

                                //COMUNICACIONES
                                msgInfoComunicaciones vRequestComunicaciones = new msgInfoComunicaciones()
                                {
                                    tipo = "0",
                                    idcomunicacion = vGETCambio
                                };

                                String vResponsComunicaciones = "";
                                HttpResponseMessage vHttpResponseComunicaciones = vConector.PostInfoComunicaciones(vRequestComunicaciones, ref vResponsComunicaciones);
                                if (vHttpResponseComunicaciones.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    msgUpdateGeneral vInfoComunicacionesResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponsComunicaciones);
                                    if (vInfoComunicacionesResponse.updateCount1.Equals("1"))
                                    {
                                        InsertarComunicaciones(vConector, vGETCambio);
                                    }
                                    else
                                    {
                                        if (GVComunicaciones.Rows.Count > 0)
                                            InsertarComunicaciones(vConector, vGETCambio);
                                    }
                                }

                                //PROCEDIMIENTOS
                                msgInfoProcedimientos vRequestProcedimientos = new msgInfoProcedimientos()
                                {
                                    tipo = "0",
                                    idprocedimiento = vGETCambio
                                };

                                String vResponseProcedimientos = "";
                                HttpResponseMessage vHttpResponseProcedimientos = vConector.PostInfoProcedimientos(vRequestProcedimientos, ref vResponseProcedimientos);
                                if (vHttpResponseProcedimientos.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    msgUpdateGeneral vInfoProcedimientosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseProcedimientos);
                                    if (vInfoProcedimientosResponse.updateCount1.Equals("1"))
                                    {
                                        InsertarProcedimientos(vConector, vGETCambio);
                                    }
                                    else
                                    {
                                        if (GVProcedimientos.Rows.Count > 0)
                                            InsertarProcedimientos(vConector, vGETCambio);
                                    }
                                }

                                //ROLLBACK
                                msgInfoRollbacks vRequestRollback = new msgInfoRollbacks()
                                {
                                    tipo = "0",
                                    idrollback = vGETCambio
                                };

                                String vResponseRollback = "";
                                HttpResponseMessage vHttpResponseRollback = vConector.PostInfoRollbacks(vRequestRollback, ref vResponseRollback);
                                if (vHttpResponseRollback.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    msgUpdateGeneral vInfoRollbackResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseRollback);
                                    if (vInfoRollbackResponse.updateCount1.Equals("1"))
                                    {
                                        InsertarRollback(vConector, vGETCambio);
                                    }
                                    else
                                    {
                                        if (GVRollback.Rows.Count > 0)
                                            InsertarRollback(vConector, vGETCambio);
                                    }
                                }

                                //PRUEBAS
                                msgInfoPruebas vRequestPruebas = new msgInfoPruebas()
                                {
                                    tipo = "0",
                                    idprueba = vGETCambio
                                };

                                String vResponsePruebas = "";
                                HttpResponseMessage vHttpResponsePruebas = vConector.PostInfoPruebas(vRequestPruebas, ref vResponsePruebas);
                                if (vHttpResponsePruebas.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    msgUpdateGeneral vInfoPruebasResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponsePruebas);
                                    if (vInfoPruebasResponse.updateCount1.Equals("1"))
                                    {
                                        InsertarPruebas(vConector, vGETCambio);
                                    }
                                    else
                                    {
                                        if (GVPruebas.Rows.Count > 0)
                                            InsertarPruebas(vConector, vGETCambio);
                                    }
                                }

                                //ARCHIVOS
                                msgInfoArchivos vRequestArchivos = new msgInfoArchivos()
                                {
                                    tipo = "3",
                                    idcambio = vGETCambio
                                };

                                String vResponseArchivos = "";
                                HttpResponseMessage vHttpResponseArchivos = vConector.PostInfoArchivos(vRequestArchivos, ref vResponseArchivos);
                                if (vHttpResponseArchivos.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    msgInfoArchivosQueryResponse vInfoArchivosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoArchivosQueryResponse>(vResponseArchivos);
                                    if (vInfoArchivosResponse.resultSet1.Count() > 0)
                                    {
                                        foreach (msgInfoArchivosQueryResponseItem itemArchivos in vInfoArchivosResponse.resultSet1)
                                        {
                                            byte[] vFileDeposito1 = null;
                                            byte[] vFileDeposito2 = null;
                                            byte[] vFileDeposito3 = null;

                                            String depot1nombre = String.Empty;
                                            String depot2nombre = String.Empty;
                                            String depot3nombre = String.Empty;
                                            if (itemArchivos.deposito1.Equals(""))
                                            {
                                                HttpPostedFile bufferDeposito1T = FUDeposito1.PostedFile;
                                                if (bufferDeposito1T != null)
                                                {
                                                    depot1nombre = FUDeposito1.FileName;
                                                    Stream vStream = bufferDeposito1T.InputStream;
                                                    BinaryReader vReader = new BinaryReader(vStream);
                                                    vFileDeposito1 = vReader.ReadBytes((int)vStream.Length);
                                                }
                                            }
                                            else
                                                vFileDeposito1 = Convert.FromBase64String(itemArchivos.deposito1);
                                            if (itemArchivos.deposito2.Equals(""))
                                            {
                                                HttpPostedFile bufferDeposito2T = FUDeposito2.PostedFile;
                                                if (bufferDeposito2T != null)
                                                {
                                                    depot2nombre = FUDeposito2.FileName;
                                                    Stream vStream = bufferDeposito2T.InputStream;
                                                    BinaryReader vReader = new BinaryReader(vStream);
                                                    vFileDeposito2 = vReader.ReadBytes((int)vStream.Length);
                                                }
                                            }
                                            else
                                                vFileDeposito2 = Convert.FromBase64String(itemArchivos.deposito2);
                                            if (itemArchivos.deposito3.Equals(""))
                                            {
                                                HttpPostedFile bufferDeposito3T = FUDeposito3.PostedFile;
                                                if (bufferDeposito3T != null)
                                                {
                                                    depot3nombre = FUDeposito3.FileName;
                                                    Stream vStream = bufferDeposito3T.InputStream;
                                                    BinaryReader vReader = new BinaryReader(vStream);
                                                    vFileDeposito3 = vReader.ReadBytes((int)vStream.Length);
                                                }
                                            }
                                            else
                                                vFileDeposito3 = Convert.FromBase64String(itemArchivos.deposito3);

                                            msgInfoArchivos vRequestArchivosUpdate = new msgInfoArchivos()
                                            {
                                                tipo = "2",
                                                idcambio = vGETCambio,
                                                deposito1 = Convert.ToBase64String(vFileDeposito1),
                                                deposito2 = Convert.ToBase64String(vFileDeposito2),
                                                deposito3 = Convert.ToBase64String(vFileDeposito3),
                                                usuario = vConfigurations.resultSet1[0].idUsuario,
                                                depot1nombre = depot1nombre,
                                                depot2nombre = depot2nombre,
                                                depot3nombre = depot3nombre,
                                            };
                                            String vResponseArchivosUpdate = "";
                                            HttpResponseMessage vHttpResponseArchivosUpdate = vConector.PostInfoArchivos(vRequestArchivosUpdate, ref vResponseArchivosUpdate);
                                            if (vHttpResponseArchivosUpdate.StatusCode == System.Net.HttpStatusCode.OK)
                                            {
                                                msgUpdateGeneral vInfoArchivosUpdateResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseArchivosUpdate);
                                                if (vInfoArchivosUpdateResponse.updateCount1.Equals("1"))
                                                {
                                                    //SABER SI TODO ESTA BIEN
                                                }
                                            }
                                        }
                                    }
                                }

                                Session["CAMBIOCREADO"] = vGETCambio;
                                if (!vGETCambio.Equals(""))
                                {

                                    msgInfoAprobaciones vInfoAprobacionesRequest = new msgInfoAprobaciones()
                                    {
                                        tipo = "4",
                                        idaprobacion = vGETCambio
                                    };
                                    String vResponseAprobaciones = "";
                                    HttpResponseMessage vHttpResponseAprobaciones = vConector.PostInfoAprobaciones(vInfoAprobacionesRequest, ref vResponseAprobaciones);
                                    if (vHttpResponseAprobaciones.StatusCode == System.Net.HttpStatusCode.OK)
                                    {
                                        msgUpdateGeneral vInfoAprobacionesResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseAprobaciones);
                                        if (vInfoAprobacionesResponse.updateCount1.Equals("1"))
                                        {
                                            Response.Redirect("/pages/services/status.aspx");
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
                else //----------------------------------------------------------------------------------------------
                {

                    //MANTENIMIENTOS
                    msgInfoMantenimientos vRequestMantenimiento = new msgInfoMantenimientos()
                    {
                        tipo = "1",
                        idmantenimiento = "",
                        descripcion = TxMantenimientoDescripcion.Text,
                        idtipocambio = ObtenerTipoCambio(),
                        idlugar = ObtenerLugarMantenimiento(),
                        otros = TxTicket.Text,
                        usuario = vConfigurations.resultSet1[0].idUsuario
                    };

                    String vIdMantenimientoCreado = "";
                    String vResponseMantenimientos = "";
                    HttpResponseMessage vHttpResponseMantenimientos = vConector.PostInfoMantenimientos(vRequestMantenimiento, ref vResponseMantenimientos);
                    if (vHttpResponseMantenimientos.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        msgInfoMantenimientosCreateResponse vInfoMantenimientosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoMantenimientosCreateResponse>(vResponseMantenimientos);
                        if (vInfoMantenimientosResponse.updateCount1.Equals("1"))
                        {
                            vIdMantenimientoCreado = vInfoMantenimientosResponse.resultSet1[0].idMantenimiento;
                            //INGRESO DE TIPO MANTENIMIENTO REL CON MANTENIMIENTOS
                            msgRelacionesMantenimientos vRelMantenimientosRequestTipo = new msgRelacionesMantenimientos()
                            {
                                tipo = "1",
                                subtipo = "1", //CREATE
                                principal = vIdMantenimientoCreado,
                                secundario = DDLTipoMantenimiento.SelectedValue
                            };

                            String vResponseTipoMantenimiento = "";
                            HttpResponseMessage vHttpResponseRelTipoMantenimientos = vConector.PostRelacionesMantenimientos(vRelMantenimientosRequestTipo, ref vResponseTipoMantenimiento);

                            //INGRESO DE SUBTIPO MANTENIMIENTO REL CON MANTENIMIENTOS
                            msgRelacionesMantenimientos vRelMantenimientosRequestSubTipo = new msgRelacionesMantenimientos()
                            {
                                tipo = "2",
                                subtipo = "1", //CREATE
                                principal = vIdMantenimientoCreado,
                                secundario = DDLSubtipoMantenimiento.SelectedValue
                            };

                            String vResponseSubTipoMantenimiento = "";
                            HttpResponseMessage vHttpResponseRelSubTipoMantenimientos = vConector.PostRelacionesMantenimientos(vRelMantenimientosRequestSubTipo, ref vResponseSubTipoMantenimiento);
                        }
                    }
                    //CALENDARIOS
                    String vIdInfoCalendarioCreado = "";

                    String vDenegacionInicio = "";
                    String vDenegacionFin = "";
                    if (!TxVentanaDenegacionInicio.Text.Equals(""))
                        vDenegacionInicio = Convert.ToDateTime(TxVentanaDenegacionInicio.Text).ToString("yyyy-MM-dd HH:mm:ss");
                    if (!TxVentanaDenegacionFin.Text.Equals(""))
                        vDenegacionFin = Convert.ToDateTime(TxVentanaDenegacionFin.Text).ToString("yyyy-MM-dd HH:mm:ss");

                    msgInfoCalendarios vRequestCalendarios = new msgInfoCalendarios()
                    {
                        tipo = "1",
                        idcalendario = "",
                        descripcion = TxNombreMantenimiento.Text + DateTime.Now,
                        fechaventana = Convert.ToDateTime(TxVentanaDuracionInicio.Text).ToString("yyyy-MM-dd"),
                        ventanainicio = Convert.ToDateTime(TxVentanaDuracionInicio.Text).ToString("yyyy-MM-dd HH:mm:ss"),
                        ventanafin = Convert.ToDateTime(TxVentanaDuracionFin.Text).ToString("yyyy-MM-dd HH:mm:ss"),
                        denegacioninicio = vDenegacionInicio,
                        denegacionfin = vDenegacionFin,
                        usuario = vConfigurations.resultSet1[0].idUsuario
                    };

                    String vResponseInfoCalendarios = "";
                    HttpResponseMessage vHttpResponseInfoCalendarios = vConector.PostInfoCalendarios(vRequestCalendarios, ref vResponseInfoCalendarios);
                    if (vHttpResponseMantenimientos.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        msgInfoCalendariosCreateResponse vInfoCalendariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCalendariosCreateResponse>(vResponseInfoCalendarios);
                        if (vInfoCalendariosResponse.updateCount1.Equals("1"))
                        {
                            vIdInfoCalendarioCreado = vInfoCalendariosResponse.resultSet1[0].idCalendario;
                        }
                    }



                    String vPaso = "1";
                    if (ObtenerTipoCambio().Equals("1"))
                    {
                        vPaso = "3";
                    }
                    if (ObtenerTipoCambio().Equals("3"))
                    {
                        vPaso = "2";
                    }

                    //CAMBIO
                    msgInfoCambios vRequestCambio = new msgInfoCambios()
                    {
                        tipo = "1",
                        nombre = TxNombreMantenimiento.Text,
                        proveedor = DDLProveedor.SelectedValue,
                        responsable = Convert.ToString(Session["USUARIOASIGNADO"]),
                        idresolucion = "0",
                        idcriticidad = ObtenerCriticidad(),
                        idimpacto = ObtenerImpacto(),
                        idriesgo = ObtenerRiesgo(),
                        observaciones = TxObservaciones.Text,
                        usuario = vConfigurations.resultSet1[0].idUsuario,
                        idmantenimiento = vIdMantenimientoCreado,
                        idcalendario = vIdInfoCalendarioCreado,
                        usuariogrud = vConfigurations.resultSet1[0].idUsuario,
                        paso = vPaso
                    };

                    String vResponseCambios = "";
                    HttpResponseMessage vHttpResponseCambios = vConector.PostInfoCambios(vRequestCambio, ref vResponseCambios);
                    if (vHttpResponseCambios.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        msgInfoCambiosCreateResponse vInfoCambiosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosCreateResponse>(vResponseCambios);
                        if (vInfoCambiosResponse.updateCount1.Equals("1"))
                        {
                            String vIdCambioCreado = vInfoCambiosResponse.resultSet1[0].idCambio;
                            //SISTEMAS
                            if (GVSistemas.Rows.Count > 0)
                            {
                                InsertarSistemas(vConector, vIdCambioCreado);
                            }

                            //EQUIPOS
                            if (GVEquipos.Rows.Count > 0)
                            {
                                InsertarEquipos(vConector, vIdCambioCreado);
                            }

                            //PERSONAL
                            if (GVPersonal.Rows.Count > 0)
                            {
                                InsertarPersonal(vConector, vIdCambioCreado);
                            }

                            //COMUNICACIONES
                            if (GVComunicaciones.Rows.Count > 0)
                            {
                                InsertarComunicaciones(vConector, vIdCambioCreado);
                            }

                            //PROCEDIMIENTOS
                            if (GVProcedimientos.Rows.Count > 0)
                            {
                                InsertarProcedimientos(vConector, vIdCambioCreado);
                            }

                            //ROLLBACK
                            if (GVRollback.Rows.Count > 0)
                            {
                                InsertarRollback(vConector, vIdCambioCreado);
                            }

                            //PRUEBAS
                            if (GVPruebas.Rows.Count > 0)
                            {
                                InsertarPruebas(vConector, vIdCambioCreado);
                            }


                            //ARCHIVOS
                            String vNombreDepot1 = String.Empty;
                            HttpPostedFile bufferDeposito1T = FUDeposito1.PostedFile;
                            byte[] vFileDeposito1 = null;
                            if (bufferDeposito1T != null)
                            {
                                vNombreDepot1 = FUDeposito1.FileName;
                                Stream vStream = bufferDeposito1T.InputStream;
                                BinaryReader vReader = new BinaryReader(vStream);
                                vFileDeposito1 = vReader.ReadBytes((int)vStream.Length);
                            }
                            String vNombreDepot2 = String.Empty;
                            HttpPostedFile bufferDeposito2T = FUDeposito2.PostedFile;
                            byte[] vFileDeposito2 = null;
                            if (bufferDeposito2T != null)
                            {
                                vNombreDepot2 = FUDeposito2.FileName;
                                Stream vStream = bufferDeposito2T.InputStream;
                                BinaryReader vReader = new BinaryReader(vStream);
                                vFileDeposito2 = vReader.ReadBytes((int)vStream.Length);
                            }
                            String vNombreDepot3 = String.Empty;
                            HttpPostedFile bufferDeposito3T = FUDeposito3.PostedFile;
                            byte[] vFileDeposito3 = null;
                            if (bufferDeposito3T != null)
                            {
                                vNombreDepot3 = FUDeposito3.FileName;
                                Stream vStream = bufferDeposito3T.InputStream;
                                BinaryReader vReader = new BinaryReader(vStream);
                                vFileDeposito3 = vReader.ReadBytes((int)vStream.Length);
                            }

                            msgInfoArchivos vRequestArchivos = new msgInfoArchivos()
                            {
                                tipo = "1",
                                idcambio = vIdCambioCreado,
                                deposito1 = Convert.ToBase64String(vFileDeposito1), 
                                deposito2 = Convert.ToBase64String(vFileDeposito2), 
                                deposito3 = Convert.ToBase64String(vFileDeposito3), 
                                usuario = vConfigurations.resultSet1[0].idUsuario,
                                depot1nombre = vNombreDepot1,
                                depot2nombre = vNombreDepot2,
                                depot3nombre = vNombreDepot3
                            };

                            String vResponseArchivos = "";
                            HttpResponseMessage vHttpResponseArchivos = vConector.PostInfoArchivos(vRequestArchivos, ref vResponseArchivos);
                            if (vHttpResponseArchivos.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                msgUpdateGeneral vInfoPruebasResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseArchivos);
                                if (vInfoPruebasResponse.updateCount1.Equals("1"))
                                {
                                    //SABER SI TODO ESTA BIEN
                                }
                            }


                            Session["CAMBIOCREADO"] = vIdCambioCreado;
                            if (!vIdCambioCreado.Equals(""))
                            {
                                
                                msgInfoUsuarios vUsuariosRequest = new msgInfoUsuarios() { tipo = "2", usuario = vConfigurations.resultSet1[0].idUsuario };
                                String vUsuarioResponse = String.Empty;
                                HttpResponseMessage vHttpResponseUsuarios = vConector.PostInfoUsuarios(vUsuariosRequest, ref vUsuarioResponse);
                                if (vHttpResponseUsuarios.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    msgInfoUsuariosQueryResponse vInfoUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoUsuariosQueryResponse>(vUsuarioResponse);
                                    if (vInfoUsuariosResponse.resultSet1.Count() > 0)
                                    {
                                        foreach (msgInfoUsuariosQueryResponseItem itemUsuarios in vInfoUsuariosResponse.resultSet1)
                                        {

                                            String Aprobador = String.Empty;
                                            if (ObtenerTipoCambio().Equals("1"))
                                            {
                                                if (itemUsuarios.dependenciaSTD != null)
                                                {
                                                    if (itemUsuarios.dependenciaSTD.Equals(""))
                                                        Aprobador = itemUsuarios.dependencia;
                                                    else
                                                        Aprobador = itemUsuarios.dependenciaSTD;
                                                }
                                                else
                                                    Aprobador = itemUsuarios.dependencia;
                                            }
                                            else
                                                Aprobador = itemUsuarios.dependencia;

                                            msgInfoAprobaciones vInfoAprobacionesRequest = new msgInfoAprobaciones()
                                            {
                                                tipo = "1",
                                                idaprobacion = "",
                                                aprobador = Aprobador,
                                                estado = "0",
                                                usuario = vConfigurations.resultSet1[0].idUsuario
                                            };
                                            String vResponseAprobaciones = "";
                                            HttpResponseMessage vHttpResponseAprobaciones = vConector.PostInfoAprobaciones(vInfoAprobacionesRequest, ref vResponseAprobaciones);
                                            if (vHttpResponseAprobaciones.StatusCode == System.Net.HttpStatusCode.OK)
                                            {
                                                msgInfoAprobacionesCreateResponse vInfoAprobacionesResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoAprobacionesCreateResponse>(vResponseAprobaciones);
                                                if (vInfoAprobacionesResponse.updateCount1.Equals("1"))
                                                {
                                                    //INGRESO DE IDCAMBIO REL IDEQUIPOS
                                                    msgRelacionesCambios vRelAprobacionesRequest = new msgRelacionesCambios()
                                                    {
                                                        tipo = "2",
                                                        subtipo = "1", //CREATE
                                                        principal = vIdCambioCreado,
                                                        secundario = vInfoAprobacionesResponse.resultSet1[0].idAprobacion
                                                    };

                                                    String vRelAprobacionesResponse = "";
                                                    HttpResponseMessage vHttpResponseRelPruebas = vConector.PostRelacionesCambios(vRelAprobacionesRequest, ref vRelAprobacionesResponse);
                                                }
                                            }
                                        }
                                    }
                                }
                                Response.Redirect("/pages/services/status.aspx");
                            }
                        }
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        private void CierreCambio()
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];


                msgInfoCambios vInfoCambiosRequest = new msgInfoCambios()
                {
                    tipo = "3",
                    idcambio = LbNumeroCambio.Text,
                    usuariogrud = vConfigurations.resultSet1[0].idUsuario
                };
                String vResponseCambios = "";
                HttpResponseMessage vHttpResponseCambios = vConector.PostInfoCambios(vInfoCambiosRequest, ref vResponseCambios);

                if (vHttpResponseCambios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoCambiosQueryResponse vInfoCambiosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseCambios);
                    if (vInfoCambiosResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoCambiosQueryResponseItem item in vInfoCambiosResponse.resultSet1)
                        {
                            if (item.pasos != null)
                            {
                                if (item.pasos.Equals("2"))
                                    throw new Exception("No puedes cerrar sin haber terminado el paso 2");
                                if (item.pasos.Equals("3"))
                                    throw new Exception("No puedes cerrar sin haber terminado el paso 3");
                                if (item.pasos.Equals("4"))
                                {
                                    ValidacionesCierre();
                                }
                                if(item.pasos.Equals("5"))
                                    throw new Exception("La resolucion del cambio ya ha sido guardada");
                            }
                        }
                    }
                }

                String vEstado = Convert.ToString(Session["ESTADOCAMBIO"]);
                if (vEstado.Equals("1"))
                    throw new Exception("Este cambio ya esta cerrado");

                String vVentanaDenegacionInicio = "";
                String vVentanaDenegacionFin = "";
                if (!TxCierreDenegacionInicio.Text.Equals(""))
                    vVentanaDenegacionInicio = Convert.ToDateTime(TxCierreDenegacionInicio.Text).ToString("yyyy-MM-dd HH:mm:ss");
                if (!TxCierreDenegacionFinal.Text.Equals(""))
                    vVentanaDenegacionFin = Convert.ToDateTime(TxCierreDenegacionFinal.Text).ToString("yyyy-MM-dd HH:mm:ss");

                String vFechaRollbackInicio = "";
                String vFechaRollbackFinal = "";
                String vFechaRollbackDenInicio = "";
                String vFechaRollbackDenFinal = "";
                if (!TxCierreRollbackInicio.Text.Equals(""))
                    vFechaRollbackInicio = Convert.ToDateTime(TxCierreRollbackInicio.Text).ToString("yyyy-MM-dd HH:mm:ss");
                if (!TxCierreRollbackFin.Text.Equals(""))
                    vFechaRollbackFinal = Convert.ToDateTime(TxCierreRollbackFin.Text).ToString("yyyy-MM-dd HH:mm:ss");
                if (!TxCierreRollbackDenInicio.Text.Equals(""))
                    vFechaRollbackDenInicio = Convert.ToDateTime(TxCierreRollbackDenInicio.Text).ToString("yyyy-MM-dd HH:mm:ss");
                if (!TxCierreRollbackDenFin.Text.Equals(""))
                    vFechaRollbackDenFinal = Convert.ToDateTime(TxCierreRollbackDenFin.Text).ToString("yyyy-MM-dd HH:mm:ss");


                HttpPostedFile bufferDeposito1T = FUEvidenciaSubir1.PostedFile;
                byte[] vFileDeposito1 = null;
                if (bufferDeposito1T != null)
                {
                    Stream vStream = bufferDeposito1T.InputStream;
                    BinaryReader vReader = new BinaryReader(vStream);
                    vFileDeposito1 = vReader.ReadBytes((int)vStream.Length);
                }

                HttpPostedFile bufferDeposito2T = FUEvidenciaSubir2.PostedFile;
                byte[] vFileDeposito2 = null;
                if (bufferDeposito2T != null)
                {
                    Stream vStream = bufferDeposito2T.InputStream;
                    BinaryReader vReader = new BinaryReader(vStream);
                    vFileDeposito2 = vReader.ReadBytes((int)vStream.Length);
                }


                msgInfoCambiosCierre vCambioCierreRequest = new msgInfoCambiosCierre()
                {
                    tipo = "1",
                    idcambio = LbNumeroCambio.Text,
                    observaciones = TxCierreObservaciones.Text,
                    fechavenini = Convert.ToDateTime(TxCierreVentanaInicio.Text).ToString("yyyy-MM-dd HH: mm:ss"),
                    fechavenfin = Convert.ToDateTime(TxCierreVentanaFinal.Text).ToString("yyyy-MM-dd HH: mm:ss"),
                    fechavendenini = vVentanaDenegacionInicio,
                    fechavendenfin = vVentanaDenegacionFin,
                    impacto = TxCierreImpacto.Text,
                    rollback = TxCierreRollback.Text,
                    fecharollini = vFechaRollbackInicio,
                    fecharollfin = vFechaRollbackFinal,
                    fecharolldenini = vFechaRollbackDenInicio,
                    fecharolldenfin = vFechaRollbackDenFinal,
                    usuario = vConfigurations.resultSet1[0].idUsuario,
                    resultado = DDLResultado.SelectedValue,
                    deposito1 = Convert.ToBase64String(vFileDeposito1),
                    deposito2 = Convert.ToBase64String(vFileDeposito2)
                };
                String vResponseCierreCambios = "";
                HttpResponseMessage vHttpResponseCambiosCierre = vConector.PostInfoCambiosCierre(vCambioCierreRequest, ref vResponseCierreCambios);

                if (vHttpResponseCambiosCierre.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgUpdateGeneral vInfoCierreCambiosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseCierreCambios);
                    if (vInfoCierreCambiosResponse.updateCount1.Equals("1"))
                    {
                        msgInfoCambios vInfoCierreCambiosRequest = new msgInfoCambios()
                        {
                            tipo = "6",
                            idcambio = LbNumeroCambio.Text,
                            idresolucion = "0"
                        };
                        HttpResponseMessage vHttpResponseCierreCambios = vConector.PostInfoCambios(vInfoCierreCambiosRequest, ref vResponseCambios);

                        if (vHttpResponseCierreCambios.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            msgUpdateGeneral vInfoCambiosCierreResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseCambios);
                            if (vInfoCambiosCierreResponse.updateCount1.Equals("1"))
                            {
                                TxObservaciones.Text = "";
                                Session["ESTADOCAMBIO"] = "1";

                                msgInfoCambios vInfoCambiosRevisionRequest = new msgInfoCambios()
                                {
                                    tipo = "8",
                                    idcambio = LbNumeroCambio.Text,
                                    paso = "5",
                                    usuariogrud = vConfigurations.resultSet1[0].idUsuario
                                };
                                String vResponseCambiosRevision = "";
                                HttpResponseMessage vHttpResponseCambiosRevision = vConector.PostInfoCambios(vInfoCambiosRevisionRequest, ref vResponseCambiosRevision);

                                if (vHttpResponseCambiosRevision.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    msgUpdateGeneral vInfoCambiosRevisionResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseCambiosRevision);
                                    if (vInfoCambiosRevisionResponse.updateCount1.Equals("1"))
                                    {
                                        if (ObtenerTipoCambio().Equals("1") || ObtenerTipoCambio().Equals("3"))
                                        {
                                            EnviarMailCertificacionSupervisorParaCierre();
                                        }
                                        else
                                        {
                                            EnviarMailCertificacionQARevision();
                                        }
                                            
                                        SeguimientoCambio(LbNumeroCambio.Text);
                                    }
                                    else
                                    {
                                        Mensaje("Error al cerrar el cambio, contacte a mesa de ayuda.", WarningType.Danger);
                                    }
                                }
                            }
                        }


                        
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        private void ValidacionesCierre()
        {
            if (DDLResultado.SelectedValue.Equals("0"))
                throw new Exception("Por favor seleccione un resultado");
            if (TxCierreObservaciones.Text.Equals(""))
                throw new Exception("Por favor ingrese una observación de cierre");
            if (TxCierreImpacto.Text.Equals(""))
                throw new Exception("Por favor ingrese un comentario en el campo impacto del cambio");
            if (TxCierreVentanaInicio.Text.Equals(""))
                throw new Exception("Por favor seleccione un fecha y hora para el inicio de la ventana");
            if (TxCierreVentanaFinal.Text.Equals(""))
                throw new Exception("Por favor seleccione un fecha y hora para el fin de la ventana");
        }

        protected void BtnDescargarDeposito1_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                msgInfoArchivos vRequestArchivos = new msgInfoArchivos()
                {
                    tipo = "3",
                    idcambio = Convert.ToString(Session["GETIDCAMBIO"])
                };

                byte[] fileData = null;
                String vResponseArchivos = "";
                HttpResponseMessage vHttpResponseArchivos = vConector.PostInfoArchivos(vRequestArchivos, ref vResponseArchivos);
                if (vHttpResponseArchivos.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoArchivosQueryResponse vInfoPruebasResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoArchivosQueryResponse>(vResponseArchivos);
                    if (vInfoPruebasResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoArchivosQueryResponseItem itemArchivos in vInfoPruebasResponse.resultSet1)
                        {
                            if (!itemArchivos.deposito1.Equals(""))
                            {
                                fileData = Convert.FromBase64String(itemArchivos.deposito1);//UTF8Encoding.Default.GetBytes(itemArchivos.deposito1);
                            }
                        }
                    }
                }

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.AppendHeader("Content-Type", "application/zip");
                byte[] bytFile = fileData;
                Response.OutputStream.Write(bytFile, 0, bytFile.Length);
                Response.AddHeader("Content-disposition", "attachment;filename=C_" + Convert.ToString(Session["GETIDCAMBIO"]) + "_Deposito1.zip");
                Response.Flush();
                Response.End();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnDescargarDeposito2_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                msgInfoArchivos vRequestArchivos = new msgInfoArchivos()
                {
                    tipo = "3",
                    idcambio = Convert.ToString(Session["GETIDCAMBIO"])
                };

                byte[] fileData = null;
                String vResponseArchivos = "";
                HttpResponseMessage vHttpResponseArchivos = vConector.PostInfoArchivos(vRequestArchivos, ref vResponseArchivos);
                if (vHttpResponseArchivos.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoArchivosQueryResponse vInfoPruebasResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoArchivosQueryResponse>(vResponseArchivos);
                    if (vInfoPruebasResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoArchivosQueryResponseItem itemArchivos in vInfoPruebasResponse.resultSet1)
                        {
                            if (!itemArchivos.deposito2.Equals(""))
                            {
                                fileData = Convert.FromBase64String(itemArchivos.deposito2);//UTF8Encoding.Default.GetBytes(itemArchivos.deposito1);
                            }
                        }
                    }
                }

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.AppendHeader("Content-Type", "application/zip");
                byte[] bytFile = fileData;
                Response.OutputStream.Write(bytFile, 0, bytFile.Length);
                Response.AddHeader("Content-disposition", "attachment;filename=C_" + Convert.ToString(Session["GETIDCAMBIO"]) + "_Deposito2.zip");
                Response.End();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnDescargarDeposito3_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                msgInfoArchivos vRequestArchivos = new msgInfoArchivos()
                {
                    tipo = "3",
                    idcambio = Convert.ToString(Session["GETIDCAMBIO"])
                };

                byte[] fileData = null;
                String vResponseArchivos = "";
                HttpResponseMessage vHttpResponseArchivos = vConector.PostInfoArchivos(vRequestArchivos, ref vResponseArchivos);
                if (vHttpResponseArchivos.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoArchivosQueryResponse vInfoPruebasResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoArchivosQueryResponse>(vResponseArchivos);
                    if (vInfoPruebasResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoArchivosQueryResponseItem itemArchivos in vInfoPruebasResponse.resultSet1)
                        {
                            if (!itemArchivos.deposito3.Equals(""))
                            {
                                fileData = Convert.FromBase64String(itemArchivos.deposito3);//UTF8Encoding.Default.GetBytes(itemArchivos.deposito1);
                            }
                        }
                    }
                }

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.AppendHeader("Content-Type", "application/zip");
                byte[] bytFile = fileData;
                Response.OutputStream.Write(bytFile, 0, bytFile.Length);
                Response.AddHeader("Content-disposition", "attachment;filename=C_" + Convert.ToString(Session["GETIDCAMBIO"]) + "_Deposito3.zip");
                Response.End();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        private string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".doc":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".mpg":
                case "mpeg":
                    return "video/mpeg";
                case ".rtf":
                    return "application/rtf";
                case ".asp":
                    return "text/asp";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                case ".sdxl":
                    return "application/xml";
                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";
                default:
                    return "application/octet-stream";
            }
        }

        protected void BtnRevisionQA_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                Generales vGenerales = new Generales();

                if (!vGenerales.PermisosEntrada(Permisos.CABManager, vConfigurations.resultSet1[0].idCargo))
                    throw new Exception("No tienes permisos para realizar esta accion");


                if (DDLRevisionQA.SelectedValue.Equals("0"))
                    throw new Exception("Por favor seleccione una opción valida");

                if (DDLRevisionQA.SelectedValue.Equals("2"))
                {
                    String vResponseCambios = "";
                    msgInfoCambios vInfoCambiosRequest = new msgInfoCambios()
                    {
                        tipo = "8",
                        idcambio = Convert.ToString(Session["GETIDCAMBIO"]),
                        paso = "0",
                        usuariogrud = vConfigurations.resultSet1[0].idUsuario
                    };
                    HttpResponseMessage vHttpResponseCierreCambios = vConector.PostInfoCambios(vInfoCambiosRequest, ref vResponseCambios);

                    if (vHttpResponseCierreCambios.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        msgUpdateGeneral vInfoCambiosCierreResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseCambios);
                        if (vInfoCambiosCierreResponse.updateCount1.Equals("1"))
                        {
                            EnviarMailCertificacionPromotorRegreso();
                            SeguimientoCambio(Convert.ToString(Session["GETIDCAMBIO"]));
                        }
                    }
                }
                else
                {
                    msgInfoAprobaciones vInfoAprobacionesRowRequest = new msgInfoAprobaciones()
                    {
                        tipo = "3",
                        idaprobacion = Convert.ToString(Session["GETIDCAMBIO"])
                    };
                    String vResponseRowAprobaciones = "";
                    HttpResponseMessage vHttpResponseRowAprobaciones = vConector.PostInfoAprobaciones(vInfoAprobacionesRowRequest, ref vResponseRowAprobaciones);

                    Boolean vAutorizacion = true;
                    String UsuarioAutorizador = String.Empty;
                    if (vHttpResponseRowAprobaciones.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        msgInfoAprobacionesQueryResponse vInfoAprobacionesRowsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoAprobacionesQueryResponse>(vResponseRowAprobaciones);
                        if (vInfoAprobacionesRowsResponse.resultSet1.Count() > 0)
                        {
                            foreach (msgInfoAprobacionesQueryResponseItem itemAprobaciones in vInfoAprobacionesRowsResponse.resultSet1)
                            {
                                if (!itemAprobaciones.estado.Equals("true"))
                                {
                                    vAutorizacion = false;
                                    UsuarioAutorizador = itemAprobaciones.idUsuarioAprobador;
                                }
                            }
                        }
                    }

                    if (vAutorizacion)
                    {

                        vDatosCabImplementadores = (DataTable)Session["CABIMPLEMTADORES"];

                        if (vDatosCabImplementadores == null)
                            throw new Exception("Ingresa los implementadores del cambio");

                        foreach (DataRow item in vDatosCabImplementadores.Rows)
                        {

                            msgInfoImplementadores vInfoImplementadoresRequest = new msgInfoImplementadores()
                            {
                                tipo = "1",
                                idcambio = Convert.ToString(Session["GETIDCAMBIO"]),
                                responsable = item["usuario"].ToString(),
                                usuariocrud = vConfigurations.resultSet1[0].idUsuario
                            };

                            String vResponseImplementadores = "";
                            HttpResponseMessage vHttpResponseImplmentadores = vConector.PostImplementadores(vInfoImplementadoresRequest, ref vResponseImplementadores);
                            if (vHttpResponseImplmentadores.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                msgUpdateGeneral vInfoImplementadoresResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseImplementadores);
                                if (vInfoImplementadoresResponse.updateCount1.Equals("0"))
                                {
                                    throw new Exception("Error al ingresar implementador, contacte a sistemas.");
                                }
                            }

                        }

                        String vTxRevisionInicio = "";
                        String vTxRevisionFinal = "";
                        if (!TxRevisionQAInicio.Text.Equals(""))
                            vTxRevisionInicio = Convert.ToDateTime(TxRevisionQAInicio.Text).ToString("yyyy-MM-dd HH:mm:ss");
                        if (!TxRevisionQAFinal.Text.Equals(""))
                            vTxRevisionFinal = Convert.ToDateTime(TxRevisionQAFinal.Text).ToString("yyyy-MM-dd HH:mm:ss");

                        String vPaso = "3";
                        if (ObtenerTipoCambio().Equals("3"))
                        {
                            vPaso = "4";
                        }

                        msgInfoCambios vInfoCambiosRequest = new msgInfoCambios()
                        {
                            tipo = "7",
                            idcambio = Convert.ToString(Session["GETIDCAMBIO"]),
                            paso = vPaso,
                            proveedor = vTxRevisionInicio,
                            responsable = vTxRevisionFinal,
                            usuariogrud = vConfigurations.resultSet1[0].idUsuario
                        };
                        String vResponseCambios = "";
                        HttpResponseMessage vHttpResponseCambios = vConector.PostInfoCambios(vInfoCambiosRequest, ref vResponseCambios);

                        if (vHttpResponseCambios.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            msgUpdateGeneral vInfoCambiosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseCambios);
                            if (vInfoCambiosResponse.updateCount1.Equals("1"))
                            {

                                if (vPaso.Equals("4"))
                                    EnviarMailCertificacionImplementacion();
                                else
                                    EnviarMailCertificacionCAB();
                                SeguimientoCambio(Convert.ToString(Session["GETIDCAMBIO"]));
                            }
                        }
                    }
                    else
                        throw new Exception("Necesitas que el cambio este aprobado por " + UsuarioAutorizador + " para proceder con la revisión de CAB Manager");
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void DDLSistemaDenegacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (DDLSistemaDenegacion.SelectedValue.Equals("1"))
                {
                    DIVSistemasTiempoInicial.Visible = true;
                    DIVSistemasTiempoFinal.Visible = true;
                }
                else
                {
                    DIVSistemasTiempoInicial.Visible = false;
                    DIVSistemasTiempoFinal.Visible = false;
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void DDLEquipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (DDLEquipo.SelectedValue.Equals("0"))
                {
                    DIVSistema.Visible = false;
                    throw new Exception("Por favor seleccione una opción valida.");
                }
                else
                {
                    DIVSistema.Visible = true;
                }



                DDLSistema.Items.Clear();
                HttpService vConnect = new HttpService();
                msgCatSistemasQueryResponse vItems = vConnect.getCatSistemas("2", DDLEquipo.SelectedValue);

                DDLSistema.Items.Add(new ListItem { Text = "Seleccione una opción", Value = "0" });
                DDLSistema.Items.Add(new ListItem { Text = "Todos", Value = "1000" });
                foreach (msgCatSistemasQueryResponseItem item in vItems.resultSet1)
                {
                    DDLSistema.Items.Add(new ListItem { Text = item.idCatSistemas + ". " + item.sistema, Value = item.idCatSistemas });
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void CBProcedimientos_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                Generales vGenerales = new Generales();

                if (!vGenerales.PermisosEntrada(Permisos.Implementador, vConfigurations.resultSet1[0].idCargo))
                    throw new Exception("No tienes permisos para realizar esta accion");

                msgInfoAprobaciones vInfoAprobacionesRowRequest = new msgInfoAprobaciones()
                {
                    tipo = "3",
                    idaprobacion = Convert.ToString(Session["GETIDCAMBIO"])
                };
                String vResponseRowAprobaciones = "";
                HttpResponseMessage vHttpResponseRowAprobaciones = vConector.PostInfoAprobaciones(vInfoAprobacionesRowRequest, ref vResponseRowAprobaciones);

                Boolean vAutorizacion = true;
                String UsuarioAutorizador = String.Empty;
                if (vHttpResponseRowAprobaciones.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoAprobacionesQueryResponse vInfoAprobacionesRowsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoAprobacionesQueryResponse>(vResponseRowAprobaciones);
                    if (vInfoAprobacionesRowsResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoAprobacionesQueryResponseItem itemAprobaciones in vInfoAprobacionesRowsResponse.resultSet1)
                        {
                            if (!itemAprobaciones.estado.Equals("true"))
                            {
                                vAutorizacion = false;
                                UsuarioAutorizador = itemAprobaciones.idUsuarioAprobador;
                            }
                        }
                    }
                }

                if (vAutorizacion)
                {
                    GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
                    int index = row.RowIndex;
                    CheckBox cb1 = (CheckBox)GVProcedimientosImplementacion.Rows[index].FindControl("CBProcedimientos");
                    string vProcedimientoID = cb1.Attributes["value"];

                    msgInfoProcedimientos vRequestProcedimientos = new msgInfoProcedimientos()
                    {
                        tipo = "4",
                        idprocedimiento = vProcedimientoID,
                        estado = (cb1.Checked ? "1" : "0")
                    };

                    String vResponseProcedimientos = "";
                    HttpResponseMessage vHttpResponseProcedimientos = vConector.PostInfoProcedimientos(vRequestProcedimientos, ref vResponseProcedimientos);
                    if (vHttpResponseProcedimientos.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        msgUpdateGeneral vInfoProcedimientosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseProcedimientos);
                        if (vInfoProcedimientosResponse.updateCount1.Equals("1"))
                        {
                            if (cb1.Checked)
                                Mensaje("Procedimiento cerrado", WarningType.Success);
                            else
                                Mensaje("Procedimiento abierto", WarningType.Info);
                        }
                        else
                            Mensaje("Procedimiento no se ha podido cerrar, contacte a sistemas si lo considera", WarningType.Danger);
                    }
                }
                else
                {
                    GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
                    int index = row.RowIndex;
                    CheckBox cb1 = (CheckBox)GVProcedimientosImplementacion.Rows[index].FindControl("CBProcedimientos");
                    if (cb1.Checked)
                        cb1.Checked = false;
                    throw new Exception("Nesecitas que el cambio este autorizado para proceder");
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void CBRollback_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                Generales vGenerales = new Generales();

                if (!vGenerales.PermisosEntrada(Permisos.Implementador, vConfigurations.resultSet1[0].idCargo))
                    throw new Exception("No tienes permisos para realizar esta accion");


                msgInfoAprobaciones vInfoAprobacionesRowRequest = new msgInfoAprobaciones()
                {
                    tipo = "3",
                    idaprobacion = Convert.ToString(Session["GETIDCAMBIO"])
                };
                String vResponseRowAprobaciones = "";
                HttpResponseMessage vHttpResponseRowAprobaciones = vConector.PostInfoAprobaciones(vInfoAprobacionesRowRequest, ref vResponseRowAprobaciones);

                Boolean vAutorizacion = true;
                String UsuarioAutorizador = String.Empty;
                if (vHttpResponseRowAprobaciones.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoAprobacionesQueryResponse vInfoAprobacionesRowsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoAprobacionesQueryResponse>(vResponseRowAprobaciones);
                    if (vInfoAprobacionesRowsResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoAprobacionesQueryResponseItem itemAprobaciones in vInfoAprobacionesRowsResponse.resultSet1)
                        {
                            if (!itemAprobaciones.estado.Equals("true"))
                            {
                                vAutorizacion = false;
                                UsuarioAutorizador = itemAprobaciones.idUsuarioAprobador;
                            }
                        }
                    }
                }

                if (vAutorizacion)
                {
                    GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
                    int index = row.RowIndex;
                    CheckBox cb1 = (CheckBox)GVRollbackImplementacion.Rows[index].FindControl("CBRollback");
                    string vRollbackID = cb1.Attributes["value"];

                    msgInfoRollbacks vRequestRollback = new msgInfoRollbacks()
                    {
                        tipo = "4",
                        idrollback = vRollbackID,
                        estado = (cb1.Checked ? "1" : "0")
                    };

                    String vResponseRollback = "";
                    HttpResponseMessage vHttpResponseRollback = vConector.PostInfoRollbacks(vRequestRollback, ref vResponseRollback);
                    if (vHttpResponseRollback.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        msgUpdateGeneral vInfoRollbackResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseRollback);
                        if (vInfoRollbackResponse.updateCount1.Equals("1"))
                        {
                            if (cb1.Checked)
                                Mensaje("Rollback cerrado", WarningType.Success);
                            else
                                Mensaje("Rollback abierto", WarningType.Info);
                        }
                        else
                            Mensaje("Rollback no se ha podido cerrar, contacte a sistemas si lo considera", WarningType.Danger);
                    }
                }
                else
                {
                    GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
                    int index = row.RowIndex;
                    CheckBox cb1 = (CheckBox)GVProcedimientosImplementacion.Rows[index].FindControl("CBProcedimientos");
                    if (cb1.Checked)
                        cb1.Checked = false;
                    throw new Exception("Nesecitas que el cambio este autorizado para proceder");
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void CBPruebas_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                Generales vGenerales = new Generales();

                if (!vGenerales.PermisosEntrada(Permisos.Implementador, vConfigurations.resultSet1[0].idCargo))
                    throw new Exception("No tienes permisos para realizar esta accion");


                msgInfoAprobaciones vInfoAprobacionesRowRequest = new msgInfoAprobaciones()
                {
                    tipo = "3",
                    idaprobacion = Convert.ToString(Session["GETIDCAMBIO"])
                };
                String vResponseRowAprobaciones = "";
                HttpResponseMessage vHttpResponseRowAprobaciones = vConector.PostInfoAprobaciones(vInfoAprobacionesRowRequest, ref vResponseRowAprobaciones);

                Boolean vAutorizacion = true;
                String UsuarioAutorizador = String.Empty;
                if (vHttpResponseRowAprobaciones.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoAprobacionesQueryResponse vInfoAprobacionesRowsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoAprobacionesQueryResponse>(vResponseRowAprobaciones);
                    if (vInfoAprobacionesRowsResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoAprobacionesQueryResponseItem itemAprobaciones in vInfoAprobacionesRowsResponse.resultSet1)
                        {
                            if (!itemAprobaciones.estado.Equals("true"))
                            {
                                vAutorizacion = false;
                                UsuarioAutorizador = itemAprobaciones.idUsuarioAprobador;
                            }
                        }
                    }
                }

                if (vAutorizacion)
                {
                    GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
                    int index = row.RowIndex;
                    CheckBox cb1 = (CheckBox)GVPruebasImplementacion.Rows[index].FindControl("CBPruebas");
                    string vPruebasID = cb1.Attributes["value"];

                    msgInfoPruebas vRequestPruebas = new msgInfoPruebas()
                    {
                        tipo = "4",
                        idprueba = vPruebasID,
                        estado = (cb1.Checked ? "1" : "0")
                    };

                    String vResponsePruebas = "";
                    HttpResponseMessage vHttpResponsePruebas = vConector.PostInfoPruebas(vRequestPruebas, ref vResponsePruebas);
                    if (vHttpResponsePruebas.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        msgUpdateGeneral vInfoPruebasResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponsePruebas);
                        if (vInfoPruebasResponse.updateCount1.Equals("1"))
                        {
                            if (cb1.Checked)
                                Mensaje("Prueba cerrada", WarningType.Success);
                            else
                                Mensaje("Prueba abierta", WarningType.Info);
                        }
                        else
                            Mensaje("Prueba no se ha podido cerrar, contacte a sistemas si lo considera", WarningType.Danger);
                    }
                }
                else
                {
                    GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
                    int index = row.RowIndex;
                    CheckBox cb1 = (CheckBox)GVProcedimientosImplementacion.Rows[index].FindControl("CBProcedimientos");
                    if (cb1.Checked)
                        cb1.Checked = false;
                    throw new Exception("Nesecitas que el cambio este autorizado para proceder");
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnCertificarCambio_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                Generales vGenerales = new Generales();

                if (!vGenerales.PermisosEntrada(Permisos.QualityAssurance, vConfigurations.resultSet1[0].idCargo))
                    throw new Exception("No tienes permisos para realizar esta accion");


                if (DDLCertificacion.SelectedValue.Equals("0"))
                    throw new Exception("Por favor seleccione una opción valida");

                Boolean vAutorizado = false;
                String vResponseCambiosConsulta = "";
                msgInfoCambios vInfoCambiosRequestConsulta = new msgInfoCambios()
                {
                    tipo = "3",
                    idcambio = Convert.ToString(Session["GETIDCAMBIO"]),
                    usuariogrud = vConfigurations.resultSet1[0].idUsuario
                };
                HttpResponseMessage vHttpResponseCambiosConsulta = vConector.PostInfoCambios(vInfoCambiosRequestConsulta, ref vResponseCambiosConsulta);
                if (vHttpResponseCambiosConsulta.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoCambiosQueryResponse vInfoCambiosResponseConsulta = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseCambiosConsulta);
                    if (vInfoCambiosResponseConsulta.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoCambiosQueryResponseItem itemCambios in vInfoCambiosResponseConsulta.resultSet1)
                        {
                            if (itemCambios.autorizarQA.Equals("true"))
                                vAutorizado = true;
                        }
                    }
                }
                if (vAutorizado)
                {
                    msgInfoAprobaciones vInfoAprobacionesRowRequest = new msgInfoAprobaciones()
                    {
                        tipo = "3",
                        idaprobacion = Convert.ToString(Session["GETIDCAMBIO"])
                    };
                    String vResponseRowAprobaciones = "";
                    HttpResponseMessage vHttpResponseRowAprobaciones = vConector.PostInfoAprobaciones(vInfoAprobacionesRowRequest, ref vResponseRowAprobaciones);

                    Boolean vAutorizacion = true;
                    String UsuarioAutorizador = String.Empty;
                    if (vHttpResponseRowAprobaciones.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        msgInfoAprobacionesQueryResponse vInfoAprobacionesRowsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoAprobacionesQueryResponse>(vResponseRowAprobaciones);
                        if (vInfoAprobacionesRowsResponse.resultSet1.Count() > 0)
                        {
                            foreach (msgInfoAprobacionesQueryResponseItem itemAprobaciones in vInfoAprobacionesRowsResponse.resultSet1)
                            {
                                if (!itemAprobaciones.estado.Equals("true"))
                                {
                                    vAutorizacion = false;
                                    UsuarioAutorizador = itemAprobaciones.idUsuarioAprobador;
                                }
                            }
                        }
                    }
                    if (vAutorizacion)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalQA();", true);
                    }
                    else
                        throw new Exception("Necesitas que el cambio este aprobado por " + UsuarioAutorizador + " para proceder con la revisión de QA");
                }
                else
                    throw new Exception("Necesita ser aprobado por un supervisor de QA");
              
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        private void SeguimientoCambio(String vCambio)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                msgInfoCambios vInfoCambiosRequest = new msgInfoCambios()
                {
                    tipo = "3",
                    idcambio = vCambio,
                    usuariogrud = vConfigurations.resultSet1[0].idUsuario
                };
                String vResponseCambios = "";
                HttpResponseMessage vHttpResponseCambios = vConector.PostInfoCambios(vInfoCambiosRequest, ref vResponseCambios);

                if (vHttpResponseCambios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoCambiosQueryResponse vInfoCambiosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseCambios);
                    if (vInfoCambiosResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoCambiosQueryResponseItem item in vInfoCambiosResponse.resultSet1)
                        {
                            Response.RedirectPermanent("/pages/services/search.aspx?busqueda=" + item.mantenimientoNombre, true);
                        }
                    }
                }
            }
            catch (Exception Ex){Mensaje(Ex.Message, WarningType.Danger);}
        }
        protected void BtnImplementacion_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                Generales vGenerales = new Generales();

                if (!vGenerales.PermisosEntrada(Permisos.Implementador, vConfigurations.resultSet1[0].idCargo))
                    throw new Exception("No tienes permisos para realizar esta accion");


                if (DDLCertificacion.SelectedValue.Equals("0"))
                    throw new Exception("Por favor seleccione una opción valida");

                Boolean vFlagUnchecked = false;
                foreach (GridViewRow row in GVProcedimientosImplementacion.Rows)
                {
                    CheckBox chk = row.Cells[4].FindControl("CBProcedimientos") as CheckBox;
                    if (!chk.Checked)
                        vFlagUnchecked = true;
                }

                if (vFlagUnchecked)
                    throw new Exception("Para finalizar este paso tienen que estar todos los procedimientos de implementación finalizados (Solo procedimientos)");

                msgInfoAprobaciones vInfoAprobacionesRowRequest = new msgInfoAprobaciones()
                {
                    tipo = "3",
                    idaprobacion = Convert.ToString(Session["GETIDCAMBIO"])
                };
                String vResponseRowAprobaciones = "";
                HttpResponseMessage vHttpResponseRowAprobaciones = vConector.PostInfoAprobaciones(vInfoAprobacionesRowRequest, ref vResponseRowAprobaciones);

                Boolean vAutorizacion = true;
                String UsuarioAutorizador = String.Empty;
                if (vHttpResponseRowAprobaciones.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoAprobacionesQueryResponse vInfoAprobacionesRowsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoAprobacionesQueryResponse>(vResponseRowAprobaciones);
                    if (vInfoAprobacionesRowsResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoAprobacionesQueryResponseItem itemAprobaciones in vInfoAprobacionesRowsResponse.resultSet1)
                        {
                            if (!itemAprobaciones.estado.Equals("true"))
                            {
                                vAutorizacion = false;
                                UsuarioAutorizador = itemAprobaciones.idUsuarioAprobador;
                            }
                        }
                    }
                }

                if (vAutorizacion)
                {
                    String vResponseCambios = "";
                    msgInfoCambios vInfoCambiosRequest = new msgInfoCambios()
                    {
                        tipo = "8",
                        idcambio = Convert.ToString(Session["GETIDCAMBIO"]),
                        paso = "4",
                        usuariogrud = vConfigurations.resultSet1[0].idUsuario
                    };
                    HttpResponseMessage vHttpResponseCierreCambios = vConector.PostInfoCambios(vInfoCambiosRequest, ref vResponseCambios);

                    if (vHttpResponseCierreCambios.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        msgUpdateGeneral vInfoCambiosCierreResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseCambios);
                        if (vInfoCambiosCierreResponse.updateCount1.Equals("1"))
                        {
                            EnviarMailCertificacionImplementacion();
                            SeguimientoCambio(Convert.ToString(Session["GETIDCAMBIO"]));
                        }
                    }
                }
                else
                    throw new Exception("Necesitas que el cambio este aprobado por " + UsuarioAutorizador + " para proceder con la revisión de QA");
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnCerrarCambio_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                Generales vGenerales = new Generales();


                if (ObtenerTipoCambio().Equals("1"))
                {
                    if (!vGenerales.PermisosEntrada(Permisos.Supervisor, vConfigurations.resultSet1[0].idCargo))
                        throw new Exception("No tienes permisos para realizar esta accion");
                }
                else
                {
                    if (!vGenerales.PermisosEntrada(Permisos.QualityAssurance, vConfigurations.resultSet1[0].idCargo))
                        throw new Exception("No tienes permisos para realizar esta accion");
                }


                //if (!vGenerales.PermisosEntrada(Permisos.QualityAssurance, vConfigurations.resultSet1[0].idCargo))
                //    throw new Exception("No tienes permisos para realizar esta accion, QA debe cerrar el cambio.");

                if (DDLCerrarCambio.SelectedValue.Equals("0"))
                    throw new Exception("Por favor seleccione una opción valida");

                HttpPostedFile bufferDeposito1T = FUEvidenciaCierre.PostedFile;
                byte[] vFileDeposito1 = null;
                if (bufferDeposito1T != null)
                {
                    Stream vStream = bufferDeposito1T.InputStream;
                    BinaryReader vReader = new BinaryReader(vStream);
                    vFileDeposito1 = vReader.ReadBytes((int)vStream.Length);
                }

                msgInfoCambios vInfoCierreCambiosRequest = new msgInfoCambios()
                {
                    tipo = "6",
                    idcambio = Convert.ToString(Session["GETIDCAMBIO"]),
                    idresolucion = "1"
                };
                String vResponseCambios = "";
                HttpResponseMessage vHttpResponseCierreCambios = vConector.PostInfoCambios(vInfoCierreCambiosRequest, ref vResponseCambios);

                if (vHttpResponseCierreCambios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgUpdateGeneral vInfoCambiosCierreResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseCambios);
                    if (vInfoCambiosCierreResponse.updateCount1.Equals("1"))
                    {

                        String vResponseCambiosCierre = "";
                        msgInfoCambios vInfoCambiosCierreRequest = new msgInfoCambios()
                        {
                            tipo = "8",
                            idcambio = Convert.ToString(Session["GETIDCAMBIO"]),
                            paso = "6",
                            usuariogrud = vConfigurations.resultSet1[0].idUsuario,
                            archivo = Convert.ToBase64String(vFileDeposito1)
                        };
                        HttpResponseMessage vHttpResponseCambios = vConector.PostInfoCambios(vInfoCambiosCierreRequest, ref vResponseCambiosCierre);

                        if (vHttpResponseCambios.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            msgUpdateGeneral vInfoCambiosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseCambiosCierre);
                            if (vInfoCambiosResponse.updateCount1.Equals("1"))
                            {
                                EnviarMailCertificacionSupervisorCierre();
                                SeguimientoCambio(Convert.ToString(Session["GETIDCAMBIO"]));
                            }
                        }
                    }
                    else
                    {
                        Mensaje("Ha pasado un error al cerrar el cambio, contacte a sistemas", WarningType.Danger); //22692800 Elisa Ramirez #8cbe40
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); CerrarModal("CerrarModal"); }
        }
        protected void EnviarMailCertificacionPromotorRegreso()
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                msgInfoCambios vInfoCambiosRowRequest = new msgInfoCambios()
                {
                    tipo = "3",
                    idcambio = Convert.ToString(Session["GETIDCAMBIO"]),
                    usuariogrud = vConfigurations.resultSet1[0].idUsuario
                };
                String vResponseRowCambios = "";
                HttpResponseMessage vHttpResponseRowCambios = vConector.PostInfoCambios(vInfoCambiosRowRequest, ref vResponseRowCambios);

                if (vHttpResponseRowCambios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoCambiosQueryResponse vInfoCambioRowsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseRowCambios);
                    if (vInfoCambioRowsResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoCambiosQueryResponseItem item in vInfoCambioRowsResponse.resultSet1)
                        {
                            msgInfoUsuarios vRequest = new msgInfoUsuarios()
                            {
                                tipo = "2",
                                usuario = vConfigurations.resultSet1[0].idUsuario
                            };

                            String vResponseResult = "";
                            HttpResponseMessage vHttpResponse = vConector.PostInfoUsuarios(vRequest, ref vResponseResult);
                            if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                msgInfoUsuariosQueryResponse vUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoUsuariosQueryResponse>(vResponseResult);
                                foreach (msgInfoUsuariosQueryResponseItem itemUsuarios in vUsuariosResponse.resultSet1)
                                {
                                    SmtpService vSmtpService = new SmtpService();
                                    vSmtpService.EnviarMensaje(
                                        itemUsuarios.correo,
                                        typeBody.PromotorRegreso,
                                        itemUsuarios.nombres + "(" + itemUsuarios.idUsuario + ")",
                                        item.idcambio,
                                        item.mantenimientoNombre);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Warning); }

        }
        protected void EnviarMailCertificacionQA()
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                msgInfoCambios vInfoCambiosRowRequest = new msgInfoCambios()
                {
                    tipo = "3",
                    idcambio = Convert.ToString(Session["GETIDCAMBIO"]),
                    usuariogrud = vConfigurations.resultSet1[0].idUsuario
                };
                String vResponseRowCambios = "";
                HttpResponseMessage vHttpResponseRowCambios = vConector.PostInfoCambios(vInfoCambiosRowRequest, ref vResponseRowCambios);

                if (vHttpResponseRowCambios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoCambiosQueryResponse vInfoCambioRowsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseRowCambios);
                    if (vInfoCambioRowsResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoCambiosQueryResponseItem item in vInfoCambioRowsResponse.resultSet1)
                        {
                            msgInfoUsuarios vRequest = new msgInfoUsuarios()
                            {
                                tipo = "6",
                            };

                            String vResponseResult = "";
                            HttpResponseMessage vHttpResponse = vConector.PostInfoUsuarios(vRequest, ref vResponseResult);
                            if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                msgInfoUsuariosQueryResponse vUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoUsuariosQueryResponse>(vResponseResult);
                                foreach (msgInfoUsuariosQueryResponseItem itemUsuarios in vUsuariosResponse.resultSet1)
                                {
                                    SmtpService vSmtpService = new SmtpService();
                                    vSmtpService.EnviarMensaje(
                                        itemUsuarios.correo,
                                        typeBody.QA,
                                        itemUsuarios.nombres + "(" + itemUsuarios.idUsuario + ")",
                                        item.idcambio,
                                        item.mantenimientoNombre);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Warning); }
            
        }
        protected void EnviarMailCertificacionCAB()
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                msgInfoCambios vInfoCambiosRowRequest = new msgInfoCambios()
                {
                    tipo = "3",
                    idcambio = Convert.ToString(Session["GETIDCAMBIO"]),
                    usuariogrud = vConfigurations.resultSet1[0].idUsuario
                };
                String vResponseRowCambios = "";
                HttpResponseMessage vHttpResponseRowCambios = vConector.PostInfoCambios(vInfoCambiosRowRequest, ref vResponseRowCambios);

                if (vHttpResponseRowCambios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoCambiosQueryResponse vInfoCambioRowsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseRowCambios);
                    if (vInfoCambioRowsResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoCambiosQueryResponseItem item in vInfoCambioRowsResponse.resultSet1)
                        {
                            if (ObtenerTipoCambio().Equals("1"))
                            {
                                List<String> vUsuariosEnvio = new List<string>();
                                msgInfoProcedimientos vRequestProcedimientos = new msgInfoProcedimientos()
                                {
                                    tipo = "3",
                                    idprocedimiento = item.idcambio
                                };

                                String vResponseProcedimientos = "";
                                HttpResponseMessage vHttpResponseProcedimientos = vConector.PostInfoProcedimientos(vRequestProcedimientos, ref vResponseProcedimientos);
                                if (vHttpResponseProcedimientos.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    msgInfoProcedimientosQueryResponse vInfoProcedimientosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoProcedimientosQueryResponse>(vResponseProcedimientos);
                                    if (vInfoProcedimientosResponse.resultSet1.Count() > 0)
                                    {
                                        foreach (msgInfoProcedimientosQueryResponseItem itemProcedimientos in vInfoProcedimientosResponse.resultSet1)
                                        {
                                            msgInfoUsuarios vRequest = new msgInfoUsuarios()
                                            {
                                                tipo = "2",
                                                usuario = itemProcedimientos.idUsuarioResponsable
                                            };

                                            String vResponseResult = "";
                                            HttpResponseMessage vHttpResponse = vConector.PostInfoUsuarios(vRequest, ref vResponseResult);
                                            if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                                            {
                                                msgInfoUsuariosQueryResponse vUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoUsuariosQueryResponse>(vResponseResult);
                                                foreach (msgInfoUsuariosQueryResponseItem itemUsuarios in vUsuariosResponse.resultSet1)
                                                {
                                                    Boolean vFlag = true;
                                                    for (int i = 0; i < vUsuariosEnvio.Count; i++)
                                                    {
                                                        if (vUsuariosEnvio[i].Equals(itemUsuarios.idUsuario))
                                                            vFlag = false;
                                                    }

                                                    if (vFlag)
                                                    {
                                                        SmtpService vSmtpService = new SmtpService();
                                                        vSmtpService.EnviarMensaje(
                                                            itemUsuarios.correo,
                                                            typeBody.CAB,
                                                            itemUsuarios.nombres + "(" + itemUsuarios.idUsuario + ")",
                                                            item.idcambio,
                                                            item.mantenimientoNombre);

                                                        vUsuariosEnvio.Add(itemUsuarios.idUsuario);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                msgInfoImplementadores vInfoImplementadoresRequest = new msgInfoImplementadores()
                                {
                                    tipo = "2",
                                    idcambio = Convert.ToString(Session["GETIDCAMBIO"])
                                };

                                String vResponseImplementadores = "";
                                HttpResponseMessage vHttpResponseImplmentadores = vConector.PostImplementadores(vInfoImplementadoresRequest, ref vResponseImplementadores);
                                if (vHttpResponseImplmentadores.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    msgInfoImplementadoresQueryResponse vInfoImplementadoresResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoImplementadoresQueryResponse>(vResponseImplementadores);
                                    if (vInfoImplementadoresResponse.resultSet1.Count() > 0)
                                    {
                                        foreach (msgInfoImplementadoresQueryResponseItem itemImplementacion in vInfoImplementadoresResponse.resultSet1)
                                        {
                                            msgInfoUsuarios vRequest = new msgInfoUsuarios()
                                            {
                                                tipo = "2",
                                                usuario = itemImplementacion.usuario
                                            };

                                            String vResponseResult = "";
                                            HttpResponseMessage vHttpResponse = vConector.PostInfoUsuarios(vRequest, ref vResponseResult);
                                            if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                                            {
                                                msgInfoUsuariosQueryResponse vUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoUsuariosQueryResponse>(vResponseResult);
                                                foreach (msgInfoUsuariosQueryResponseItem itemUsuarios in vUsuariosResponse.resultSet1)
                                                {
                                                    SmtpService vSmtpService = new SmtpService();
                                                    vSmtpService.EnviarMensaje(
                                                        itemUsuarios.correo,
                                                        typeBody.CAB,
                                                        itemUsuarios.nombres + "(" + itemUsuarios.idUsuario + ")",
                                                        item.idcambio,
                                                        item.mantenimientoNombre);
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Warning); }
        }
        protected void EnviarMailCertificacionImplementacion()
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                msgInfoCambios vInfoCambiosRowRequest = new msgInfoCambios()
                {
                    tipo = "3",
                    idcambio = Convert.ToString(Session["GETIDCAMBIO"]),
                    usuariogrud = vConfigurations.resultSet1[0].idUsuario
                };
                String vResponseRowCambios = "";
                HttpResponseMessage vHttpResponseRowCambios = vConector.PostInfoCambios(vInfoCambiosRowRequest, ref vResponseRowCambios);

                if (vHttpResponseRowCambios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoCambiosQueryResponse vInfoCambioRowsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseRowCambios);
                    if (vInfoCambioRowsResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoCambiosQueryResponseItem item in vInfoCambioRowsResponse.resultSet1)
                        {

                            msgInfoUsuarios vRequest = new msgInfoUsuarios()
                            {
                                tipo = "2", //ANTES 5
                                usuario = item.idUsuarioSolicitante
                            };

                            String vResponseResult = "";
                            HttpResponseMessage vHttpResponse = vConector.PostInfoUsuarios(vRequest, ref vResponseResult);
                            if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                msgInfoUsuariosQueryResponse vUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoUsuariosQueryResponse>(vResponseResult);
                                foreach (msgInfoUsuariosQueryResponseItem itemUsuarios in vUsuariosResponse.resultSet1)
                                {
                                    SmtpService vSmtpService = new SmtpService();
                                    vSmtpService.EnviarMensaje(
                                        itemUsuarios.correo,
                                        typeBody.PromotorReEnvio,
                                        itemUsuarios.nombres + "(" + itemUsuarios.idUsuario + ")",
                                        item.idcambio,
                                        item.mantenimientoNombre);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Warning); }
            
        }
        protected void EnviarMailCertificacionQARevision()
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                msgInfoCambios vInfoCambiosRowRequest = new msgInfoCambios()
                {
                    tipo = "3",
                    idcambio = Convert.ToString(Session["GETIDCAMBIO"]),
                    usuariogrud = vConfigurations.resultSet1[0].idUsuario
                };
                String vResponseRowCambios = "";
                HttpResponseMessage vHttpResponseRowCambios = vConector.PostInfoCambios(vInfoCambiosRowRequest, ref vResponseRowCambios);

                if (vHttpResponseRowCambios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoCambiosQueryResponse vInfoCambioRowsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseRowCambios);
                    if (vInfoCambioRowsResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoCambiosQueryResponseItem item in vInfoCambioRowsResponse.resultSet1)
                        {

                            if (!item.idUsuarioResponsable.Equals(""))
                            {
                                msgInfoUsuarios vRequest = new msgInfoUsuarios()
                                {
                                    tipo = "2",
                                    usuario = item.idUsuarioResponsable
                                };

                                String vResponseResult = "";
                                HttpResponseMessage vHttpResponse = vConector.PostInfoUsuarios(vRequest, ref vResponseResult);
                                if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    msgInfoUsuariosQueryResponse vUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoUsuariosQueryResponse>(vResponseResult);
                                    foreach (msgInfoUsuariosQueryResponseItem itemUsuarios in vUsuariosResponse.resultSet1)
                                    {
                                        SmtpService vSmtpService = new SmtpService();
                                        vSmtpService.EnviarMensaje(
                                            itemUsuarios.correo,
                                            typeBody.QARevision,
                                            itemUsuarios.nombres + "(" + itemUsuarios.nombres + ")",
                                            item.idcambio,
                                            item.mantenimientoNombre);
                                    }
                                }
                            }
                            else
                            {
                                msgInfoUsuarios vRequest = new msgInfoUsuarios()
                                {
                                    tipo = "11"
                                };

                                String vResponseResult = "";
                                HttpResponseMessage vHttpResponse = vConector.PostInfoUsuarios(vRequest, ref vResponseResult);
                                if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    msgInfoUsuariosQueryResponse vUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoUsuariosQueryResponse>(vResponseResult);
                                    foreach (msgInfoUsuariosQueryResponseItem itemUsuarios in vUsuariosResponse.resultSet1)
                                    {
                                        SmtpService vSmtpService = new SmtpService();
                                        vSmtpService.EnviarMensaje(
                                            itemUsuarios.correo,
                                            typeBody.QARevision,
                                            itemUsuarios.nombres + "(" + itemUsuarios.nombres + ")",
                                            item.idcambio,
                                            item.mantenimientoNombre);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Warning); }
            
        }
        protected void EnviarMailCertificacionSupervisorCierre()
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                msgInfoCambios vInfoCambiosRowRequest = new msgInfoCambios()
                {
                    tipo = "3",
                    idcambio = Convert.ToString(Session["GETIDCAMBIO"]),
                    usuariogrud = vConfigurations.resultSet1[0].idUsuario
                };
                String vResponseRowCambios = "";
                HttpResponseMessage vHttpResponseRowCambios = vConector.PostInfoCambios(vInfoCambiosRowRequest, ref vResponseRowCambios);

                if (vHttpResponseRowCambios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoCambiosQueryResponse vInfoCambioRowsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseRowCambios);
                    if (vInfoCambioRowsResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoCambiosQueryResponseItem item in vInfoCambioRowsResponse.resultSet1)
                        {
                            msgInfoComunicaciones vRequestComunicaciones = new msgInfoComunicaciones()
                            {
                                tipo = "3",
                                idcomunicacion = item.idcambio
                            };

                            String vResponsComunicaciones = "";
                            HttpResponseMessage vHttpResponseComunicaciones = vConector.PostInfoComunicaciones(vRequestComunicaciones, ref vResponsComunicaciones);
                            if (vHttpResponseComunicaciones.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                msgInfoComunicacionesQueryResponse vInfoComunicacionesResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoComunicacionesQueryResponse>(vResponsComunicaciones);
                                if (vInfoComunicacionesResponse.resultSet1.Count() > 0)
                                {
                                    foreach (msgInfoComunicacionesQueryResponseItem itemComunicaciones in vInfoComunicacionesResponse.resultSet1)
                                    {
                                        SmtpService vSmtpService = new SmtpService();
                                        vSmtpService.EnviarMensaje(
                                            itemComunicaciones.casoIncidente,
                                            typeBody.SupervisorCierre,
                                            itemComunicaciones.cambioNormal,
                                            item.idcambio,
                                            item.mantenimientoNombre);
                                    }
                                }
                            }              
                        }
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Warning); }
        }
        protected void EnviarMailCertificacionSupervisorInicio(String vIdCambio)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                msgInfoCambios vInfoCambiosRowRequest = new msgInfoCambios()
                {
                    tipo = "3",
                    idcambio = vIdCambio,
                    usuariogrud = vConfigurations.resultSet1[0].idUsuario
                };
                String vResponseRowCambios = "";
                HttpResponseMessage vHttpResponseRowCambios = vConector.PostInfoCambios(vInfoCambiosRowRequest, ref vResponseRowCambios);

                if (vHttpResponseRowCambios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoCambiosQueryResponse vInfoCambioRowsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseRowCambios);
                    if (vInfoCambioRowsResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoCambiosQueryResponseItem item in vInfoCambioRowsResponse.resultSet1)
                        {

                            if (!item.idUsuarioResponsable.Equals(""))
                            {
                                msgInfoUsuarios vRequest = new msgInfoUsuarios()
                                {
                                    tipo = "2",
                                    usuario = item.idUsuarioResponsable
                                };

                                String vResponseResult = "";
                                HttpResponseMessage vHttpResponse = vConector.PostInfoUsuarios(vRequest, ref vResponseResult);
                                if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    msgInfoUsuariosQueryResponse vUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoUsuariosQueryResponse>(vResponseResult);
                                    foreach (msgInfoUsuariosQueryResponseItem itemUsuarios in vUsuariosResponse.resultSet1)
                                    {
                                        SmtpService vSmtpService = new SmtpService();
                                        vSmtpService.EnviarMensaje(
                                            itemUsuarios.correo,
                                            typeBody.Supervisor,
                                            itemUsuarios.nombres + "(" + itemUsuarios.idUsuario + ")",
                                            item.idcambio,
                                            item.mantenimientoNombre);
                                    }
                                }
                            }
                            else
                            {

                                msgInfoUsuarios vRequest = new msgInfoUsuarios()
                                {
                                    tipo = "5",
                                };

                                String vResponseResult = "";
                                HttpResponseMessage vHttpResponse = vConector.PostInfoUsuarios(vRequest, ref vResponseResult);
                                if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    msgInfoUsuariosQueryResponse vUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoUsuariosQueryResponse>(vResponseResult);
                                    foreach (msgInfoUsuariosQueryResponseItem itemUsuarios in vUsuariosResponse.resultSet1)
                                    {
                                        SmtpService vSmtpService = new SmtpService();
                                        vSmtpService.EnviarMensaje(
                                            itemUsuarios.correo,
                                            typeBody.Supervisor,
                                            itemUsuarios.nombres + "(" + itemUsuarios.idUsuario + ")",
                                            item.idcambio,
                                            item.mantenimientoNombre);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Warning); }
        }

        protected void EnviarMailCertificacionSupervisorParaCierre()
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                msgInfoCambios vInfoCambiosRowRequest = new msgInfoCambios()
                {
                    tipo = "3",
                    idcambio = Convert.ToString(Session["GETIDCAMBIO"]),
                    usuariogrud = vConfigurations.resultSet1[0].idUsuario
                };
                String vResponseRowCambios = "";
                HttpResponseMessage vHttpResponseRowCambios = vConector.PostInfoCambios(vInfoCambiosRowRequest, ref vResponseRowCambios);

                if (vHttpResponseRowCambios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoCambiosQueryResponse vInfoCambioRowsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseRowCambios);
                    if (vInfoCambioRowsResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoCambiosQueryResponseItem item in vInfoCambioRowsResponse.resultSet1)
                        {
                            String vUsuarioSupervisor = String.Empty;
                            msgInfoAprobaciones vInfoAprobacionesRequest = new msgInfoAprobaciones()
                            {
                                tipo = "3",
                                idaprobacion = Convert.ToString(Session["GETIDCAMBIO"])
                            };
                            String vResponseAprobaciones = "";
                            HttpResponseMessage vHttpResponseAprobaciones = vConector.PostInfoAprobaciones(vInfoAprobacionesRequest, ref vResponseAprobaciones);

                            if (vHttpResponseAprobaciones.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                msgInfoAprobacionesQueryResponse vInfoAprobacionesResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoAprobacionesQueryResponse>(vResponseAprobaciones);
                                if (vInfoAprobacionesResponse.resultSet1.Count() > 0)
                                {
                                    foreach (msgInfoAprobacionesQueryResponseItem itemAprobador in vInfoAprobacionesResponse.resultSet1)
                                    {
                                        vUsuarioSupervisor = itemAprobador.idUsuarioAprobador;
                                    }
                                }
                            }

                            msgInfoUsuarios vRequest = new msgInfoUsuarios()
                            {
                                tipo = "2",
                                usuario = vUsuarioSupervisor
                            };

                            String vResponseResult = "";
                            HttpResponseMessage vHttpResponse = vConector.PostInfoUsuarios(vRequest, ref vResponseResult);
                            if (vHttpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                msgInfoUsuariosQueryResponse vUsuariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoUsuariosQueryResponse>(vResponseResult);
                                foreach (msgInfoUsuariosQueryResponseItem itemUsuarios in vUsuariosResponse.resultSet1)
                                {
                                    SmtpService vSmtpService = new SmtpService();
                                    vSmtpService.EnviarMensaje(
                                        itemUsuarios.correo,
                                        typeBody.QARevision,
                                        itemUsuarios.nombres + "(" + itemUsuarios.nombres + ")",
                                        item.idcambio,
                                        item.mantenimientoNombre);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Warning); }
        }

        protected void BtnAgregarProcedimiento_Click(object sender, EventArgs e)
        {
            try
            {
                if (!TxVentanaDuracionInicio.Text.Equals(""))
                {
                    TxProcedimientosInicio.Text = Convert.ToDateTime(TxVentanaDuracionInicio.Text).ToString("yyyy-MM-ddT00:00");
                    TxProcedimientosFin.Text = Convert.ToDateTime(TxVentanaDuracionFin.Text).ToString("yyyy-MM-ddT00:00"); ;
                    UpdateProcedimientos.Update();
                }
            }
            catch (Exception Ex){ Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnAgregarSistema_Click(object sender, EventArgs e)
        {
            try
            {
                if (!TxVentanaDuracionInicio.Text.Equals(""))
                {
                    TxSistemaTiempoInicio.Text = Convert.ToDateTime(TxVentanaDuracionInicio.Text).ToString("yyyy-MM-ddT00:00");
                    TxSistemaTiempoFinal.Text = Convert.ToDateTime(TxVentanaDuracionFin.Text).ToString("yyyy-MM-ddT00:00");
                    UpdateProcedimientos.Update();
                }          
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnEvidenciaDescargar1_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                msgInfoCambiosCierre vRequestCambiosCierre = new msgInfoCambiosCierre()
                {
                    tipo = "3",
                    idcambio = Convert.ToString(Session["GETIDCAMBIO"])
                };

                byte[] fileData = null;
                String vResponseCambiosCierre = "";
                HttpResponseMessage vHttpResponseCambiosCierre = vConector.PostInfoCambiosCierre(vRequestCambiosCierre, ref vResponseCambiosCierre);
                if (vHttpResponseCambiosCierre.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoCambiosCierreQueryResponse vInfoCambiosCierreResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosCierreQueryResponse>(vResponseCambiosCierre);
                    if (vInfoCambiosCierreResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoCambiosCierreQueryResponseItem itemArchivos in vInfoCambiosCierreResponse.resultSet1)
                        {
                            if (!itemArchivos.deposito1.Equals(""))
                            {
                                fileData = Convert.FromBase64String(itemArchivos.deposito1);
                            }
                        }
                    }
                }

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.AppendHeader("Content-Type", "application/zip");
                byte[] bytFile = fileData;
                Response.OutputStream.Write(bytFile, 0, bytFile.Length);
                Response.AddHeader("Content-disposition", "attachment;filename=C_" + Convert.ToString(Session["GETIDCAMBIO"]) + "_Evidencia1.zip");
                Response.End();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnEvidenciaDescargar2_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                msgInfoCambiosCierre vRequestInfoCambiosCierre = new msgInfoCambiosCierre()
                {
                    tipo = "3",
                    idcambio = Convert.ToString(Session["GETIDCAMBIO"])
                };

                byte[] fileData = null;
                String vResponseCambiosCierre = "";
                HttpResponseMessage vHttpResponseCambiosCierre = vConector.PostInfoCambiosCierre(vRequestInfoCambiosCierre, ref vResponseCambiosCierre);
                if (vHttpResponseCambiosCierre.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoCambiosCierreQueryResponse vInfoCambiosCierreResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosCierreQueryResponse>(vResponseCambiosCierre);
                    if (vInfoCambiosCierreResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoCambiosCierreQueryResponseItem itemArchivos in vInfoCambiosCierreResponse.resultSet1)
                        {
                            if (!itemArchivos.deposito2.Equals(""))
                            {
                                fileData = Convert.FromBase64String(itemArchivos.deposito1);
                            }
                        }
                    }
                }

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.AppendHeader("Content-Type", "application/zip");
                byte[] bytFile = fileData;
                Response.OutputStream.Write(bytFile, 0, bytFile.Length);
                Response.AddHeader("Content-disposition", "attachment;filename=C_" + Convert.ToString(Session["GETIDCAMBIO"]) + "_Evidencia2.zip");
                Response.End();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnArchivosCertificacion_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                msgInfoCambios vInfoCambiosRequest = new msgInfoCambios()
                {
                    tipo = "3",
                    idcambio = Convert.ToString(Session["GETIDCAMBIO"]),
                    usuariogrud = vConfigurations.resultSet1[0].idUsuario
                };
                byte[] fileData = null;
                String vResponseCambios = "";
                HttpResponseMessage vHttpResponseCambios = vConector.PostInfoCambios(vInfoCambiosRequest, ref vResponseCambios);

                if (vHttpResponseCambios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoCambiosQueryResponse vInfoCambiosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseCambios);
                    if (vInfoCambiosResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoCambiosQueryResponseItem itemArchivos in vInfoCambiosResponse.resultSet1)
                        {
                            if (!itemArchivos.archivo.Equals(""))
                            {
                                fileData = Convert.FromBase64String(itemArchivos.archivo);
                            }
                        }
                    }
                }

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.AppendHeader("Content-Type", "application/zip");
                byte[] bytFile = fileData;
                Response.OutputStream.Write(bytFile, 0, bytFile.Length);
                Response.AddHeader("Content-disposition", "attachment;filename=C_" + Convert.ToString(Session["GETIDCAMBIO"]) + "_QAArchivo.zip");
                Response.End();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnProcederQA_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                Boolean vAutorizado = false;
                String vResponseCambiosConsulta = "";
                msgInfoCambios vInfoCambiosRequestConsulta = new msgInfoCambios()
                {
                    tipo = "3",
                    idcambio = Convert.ToString(Session["GETIDCAMBIO"]),
                    usuariogrud = vConfigurations.resultSet1[0].idUsuario
                };
                HttpResponseMessage vHttpResponseCambiosConsulta = vConector.PostInfoCambios(vInfoCambiosRequestConsulta, ref vResponseCambiosConsulta);
                if (vHttpResponseCambiosConsulta.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoCambiosQueryResponse vInfoCambiosResponseConsulta = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseCambiosConsulta);
                    if (vInfoCambiosResponseConsulta.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoCambiosQueryResponseItem itemCambios in vInfoCambiosResponseConsulta.resultSet1)
                        {
                            if (itemCambios.autorizarQA.Equals("true"))
                                vAutorizado = true;
                        }
                    }
                }

                if (vAutorizado)
                {
                    if (DDLCertificacion.SelectedValue.Equals("2"))
                    {

                        String vResponseCambios = "";
                        msgInfoCambios vInfoCambiosRequest = new msgInfoCambios()
                        {
                            tipo = "8",
                            idcambio = Convert.ToString(Session["GETIDCAMBIO"]),
                            paso = "0",
                            usuariogrud = vConfigurations.resultSet1[0].idUsuario
                        };
                        HttpResponseMessage vHttpResponseCierreCambios = vConector.PostInfoCambios(vInfoCambiosRequest, ref vResponseCambios);

                        if (vHttpResponseCierreCambios.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            msgUpdateGeneral vInfoCambiosCierreResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseCambios);
                            if (vInfoCambiosCierreResponse.updateCount1.Equals("1"))
                            {
                                EnviarMailCertificacionPromotorRegreso();
                                SeguimientoCambio(Convert.ToString(Session["GETIDCAMBIO"]));
                            }
                        }
                    }
                    else
                    {
                        HttpPostedFile bufferDeposito1T = FUArchivosCertificacion.PostedFile;
                        byte[] vFileDeposito1 = null;
                        if (bufferDeposito1T != null)
                        {
                            Stream vStream = bufferDeposito1T.InputStream;
                            BinaryReader vReader = new BinaryReader(vStream);
                            vFileDeposito1 = vReader.ReadBytes((int)vStream.Length);
                        }

                        String vResponseCambios = "";
                        msgInfoCambios vInfoCambiosRequest = new msgInfoCambios()
                        {
                            tipo = "8",
                            idcambio = Convert.ToString(Session["GETIDCAMBIO"]),
                            paso = "2",
                            usuariogrud = vConfigurations.resultSet1[0].idUsuario,
                            archivo = Convert.ToBase64String(vFileDeposito1),
                            idcriticidad = ObtenerCriticidadQA(),
                            idimpacto = ObtenerImpactoQA(),
                            idriesgo = ObtenerRiesgoQA(),
                            observaciones = TxObservaciones.Text
                        };
                        HttpResponseMessage vHttpResponseCierreCambios = vConector.PostInfoCambios(vInfoCambiosRequest, ref vResponseCambios);

                        if (vHttpResponseCierreCambios.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            msgUpdateGeneral vInfoCambiosCierreResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseCambios);
                            if (vInfoCambiosCierreResponse.updateCount1.Equals("1"))
                            {
                                msgInfoCalendarios vRequestCalendarios = new msgInfoCalendarios()
                                {
                                    tipo = "4",
                                    idcalendario = Convert.ToString(Session["GETIDCAMBIO"]),
                                    ventanainicio = Convert.ToDateTime(TxHorarioInicioPromotor.Text).ToString("yyyy-MM-dd HH:mm:ss"),
                                    ventanafin = Convert.ToDateTime(TxHorarioFinalPromotor.Text).ToString("yyyy-MM-dd HH:mm:ss")
                                };
                                String vResponseInfoCalendarios = "";
                                HttpResponseMessage vHttpResponseInfoCalendarios = vConector.PostInfoCalendarios(vRequestCalendarios, ref vResponseInfoCalendarios);
                                if (vHttpResponseInfoCalendarios.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    msgUpdateGeneral vInfoCalendariosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseInfoCalendarios);
                                    if (vInfoCalendariosResponse.updateCount1.Equals("1"))
                                    {
                                        EnviarMailCertificacionQA();
                                        SeguimientoCambio(Convert.ToString(Session["GETIDCAMBIO"]));
                                    }
                                }
                            }
                        }

                    }
                }
                else
                {
                    throw new Exception("No está autorizado por un Supervisor de QA, ponte en contacto con uno y dile que autorice el cambio.");
                }
                
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnAutorizarCambio_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openAutorizarCambio();", true);

            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Warning); }
        }

        protected void BtnAutorizarCambioEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                if (DDLAutorizarSupervisorOpciones.SelectedValue.Equals("0"))
                {
                    LbAutorizarError.Text = "Por favor seleccione una opción valida.";
                    UpdatePanel19.Update();
                }

                if (DDLAutorizarSupervisorOpciones.SelectedValue.Equals("1"))
                {
                    msgInfoAprobaciones vInfoAprobacionesRowRequest = new msgInfoAprobaciones()
                    {
                        tipo = "2",
                        idaprobacion = Convert.ToString(Session["GETIDCAMBIO"]),
                        aprobador = vConfigurations.resultSet1[0].idUsuario
                    };
                    String vResponseRowAprobaciones = "";
                    HttpResponseMessage vHttpResponseRowAprobaciones = vConector.PostInfoAprobaciones(vInfoAprobacionesRowRequest, ref vResponseRowAprobaciones);

                    if (vHttpResponseRowAprobaciones.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        msgUpdateGeneral vInfoAprobacionesRowsResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseRowAprobaciones);
                        if (vInfoAprobacionesRowsResponse.updateCount1.Equals("1"))
                        {
                            msgInfoMantenimientos vRequestMantenimiento = new msgInfoMantenimientos()
                            {
                                tipo = "3",
                                idmantenimiento = Convert.ToString(Session["GETIDCAMBIO"])
                            };

                            String vResponseMantenimientos = "";
                            HttpResponseMessage vHttpResponseMantenimientos = vConector.PostInfoMantenimientos(vRequestMantenimiento, ref vResponseMantenimientos);
                            if (vHttpResponseMantenimientos.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                msgInfoMantenimientosQueryResponse vInfoMantenimientosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoMantenimientosQueryResponse>(vResponseMantenimientos);
                                if (vInfoMantenimientosResponse.resultSet1.Count() > 0)
                                {
                                    foreach (msgInfoMantenimientosQueryResponseItem item in vInfoMantenimientosResponse.resultSet1)
                                    {
                                        if (item.idTipoCambio.Equals("1"))
                                        {
                                            EnviarMailCertificacionCAB();
                                        }
                                        if (item.idTipoCambio.Equals("2"))
                                        {
                                            EnviarMailCertificacionSupervisorInicio(Convert.ToString(Session["GETIDCAMBIO"]));
                                        }
                                        if (item.idTipoCambio.Equals("3"))
                                        {
                                            EnviarMailCertificacionQA();
                                        }
                                    }
                                }
                            }
                            Mensaje("El cambio ha sido autorizada", WarningType.Success);
                            BtnAutorizarCambio.Visible = false;
                            CerrarModal("AutorizacionSupervisorModal");
                        }
                        else
                        {
                            CerrarModal("AutorizarModal");
                            Mensaje("No tienes permisos para autorizar este cambio", WarningType.Danger);
                        }
                    }
                }

                if (DDLAutorizarSupervisorOpciones.SelectedValue.Equals("2"))
                {
                    String vResponseCambios = "";
                    msgInfoCambios vInfoCambiosRequest = new msgInfoCambios()
                    {
                        tipo = "8",
                        idcambio = Convert.ToString(Session["GETIDCAMBIO"]),
                        paso = "0",
                        usuariogrud = vConfigurations.resultSet1[0].idUsuario
                    };
                    HttpResponseMessage vHttpResponseCierreCambios = vConector.PostInfoCambios(vInfoCambiosRequest, ref vResponseCambios);

                    if (vHttpResponseCierreCambios.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        msgUpdateGeneral vInfoCambiosCierreResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseCambios);
                        if (vInfoCambiosCierreResponse.updateCount1.Equals("1"))
                        {
                            EnviarMailCertificacionPromotorRegreso();
                            SeguimientoCambio(Convert.ToString(Session["GETIDCAMBIO"]));
                        }
                    }
                    CerrarModal("AutorizacionSupervisorModal");
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Warning); }
        }

        protected void BtnResolucion_Click(object sender, EventArgs e)
        {
            try
            {
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                Generales vGenerales = new Generales();


                ValidacionesCierre();
                if (!vGenerales.PermisosEntrada(Permisos.Implementador, vConfigurations.resultSet1[0].idCargo))
                    throw new Exception("No tienes permisos para realizar esta accion, El promotor debe revisar el cambio.");
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModalQARevision();", true);


            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnCABAgregarImplementador_Click(object sender, EventArgs e)
        {
            try
            {
                if (DDLCABImplementadores.SelectedIndex == 0)
                    throw new Exception("Por favor seleccione un implementador");

                if (Session["CABIMPLEMTADORES"] == null)
                {
                    vDatosCabImplementadores = new DataTable();
                    vDatosCabImplementadores.Columns.Add("nombre");
                    vDatosCabImplementadores.Columns.Add("usuario");
                }
                else
                {
                    vDatosCabImplementadores = (DataTable)Session["CABIMPLEMTADORES"];
                }

                vDatosCabImplementadores.Rows.Add(
                    DDLCABImplementadores.SelectedItem.Text,
                    DDLCABImplementadores.SelectedItem.Value);

                GVCABImplementadores.DataSource = vDatosCabImplementadores;
                GVCABImplementadores.DataBind();
                Session["CABIMPLEMTADORES"] = vDatosCabImplementadores;
                DDLCABImplementadores.SelectedIndex = -1;
            }
            catch (Exception Ex) { LbMensajeProcedimientos.Text = Ex.Message; UpdateProcedimientosMensaje.Update(); }
        }

        protected void GVCABImplementadores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "DeleteRow")
                {
                    string vIdUsuario = e.CommandArgument.ToString();
                    if (Session["CABIMPLEMTADORES"] != null)
                    {
                        vDatosCabImplementadores = (DataTable)Session["CABIMPLEMTADORES"];

                        DataRow[] result = vDatosCabImplementadores.Select("usuario = '" + vIdUsuario + "'");
                        foreach (DataRow row in result)
                        {
                            if (row["usuario"].ToString().Contains(vIdUsuario))
                                vDatosCabImplementadores.Rows.Remove(row);
                        }
                    }
                }
                GVCABImplementadores.DataSource = vDatosCabImplementadores;
                GVCABImplementadores.DataBind();
                Session["CABIMPLEMTADORES"] = vDatosCabImplementadores;
            }
            catch (Exception Ex) { LbMensajeProcedimientos.Text = Ex.Message; }
        }

        protected void BtnAutorizarQA_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                Generales vGenerales = new Generales();

                if (!vGenerales.PermisosEntrada(Permisos.SupervisorQA, vConfigurations.resultSet1[0].idCargo))
                    throw new Exception("No tienes permisos para realizar esta accion");

                
                msgInfoCambios vInfoCambiosRequest = new msgInfoCambios()
                {
                    tipo = "9",
                    idcambio = Convert.ToString(Session["GETIDCAMBIO"]),
                    idresolucion = "true"
                };
                String vResponseCambios = "";
                HttpResponseMessage vHttpResponseCambios = vConector.PostInfoCambios(vInfoCambiosRequest, ref vResponseCambios);

                if (vHttpResponseCambios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgUpdateGeneral vInfoCambiosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseCambios);
                    if (vInfoCambiosResponse.updateCount1.Equals("1"))
                    {
                        Mensaje("Cambio autorizado con exito", WarningType.Success);
                        BtnAutorizarQA.Visible = false;
                    }
                    else
                    {
                        Mensaje("No tienes permisos para autorizar este cambio", WarningType.Danger);
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnEnvidenciaCierreDescargar_Click(object sender, EventArgs e)
        {
            try
            {
                HttpService vConector = new HttpService();
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];

                msgInfoCambios vRequestInfoCambios = new msgInfoCambios()
                {
                    tipo = "3",
                    idcambio = Convert.ToString(Session["GETIDCAMBIO"]),
                    usuariogrud = vConfigurations.resultSet1[0].idUsuario
                };

                byte[] fileData = null;
                String vResponseCambios = "";
                HttpResponseMessage vHttpResponseCambios = vConector.PostInfoCambios(vRequestInfoCambios, ref vResponseCambios);
                if (vHttpResponseCambios.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoCambiosQueryResponse vInfoCambiosResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoCambiosQueryResponse>(vResponseCambios);
                    if (vInfoCambiosResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoCambiosQueryResponseItem itemArchivos in vInfoCambiosResponse.resultSet1)
                        {
                            if (!itemArchivos.archivoCierre.Equals(""))
                            {
                                fileData = Convert.FromBase64String(itemArchivos.archivoCierre);
                            }
                        }
                    }
                }

                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.AppendHeader("Content-Type", "application/zip");
                byte[] bytFile = fileData;
                Response.OutputStream.Write(bytFile, 0, bytFile.Length);
                Response.AddHeader("Content-disposition", "attachment;filename=C_" + Convert.ToString(Session["GETIDCAMBIO"]) + "_Evidencia2.zip");
                Response.End();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnCerrarCambio_Click1(object sender, EventArgs e)
        {
            try
            {
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                Generales vGenerales = new Generales();

                if (ObtenerTipoCambio().Equals("1"))
                {
                    if (!vGenerales.PermisosEntrada(Permisos.Supervisor, vConfigurations.resultSet1[0].idCargo))
                        throw new Exception("No tienes permisos para realizar esta accion");
                }
                else
                {
                    if (!vGenerales.PermisosEntrada(Permisos.QualityAssurance, vConfigurations.resultSet1[0].idCargo))
                        throw new Exception("No tienes permisos para realizar esta accion");
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openCierre();", true);
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnBorrarDeposito1_Click(object sender, EventArgs e)
        {
            try
            {
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                Generales vGenerales = new Generales();

                if (!vGenerales.PermisosEntrada(Permisos.Promotor, vConfigurations.resultSet1[0].idCargo))
                    throw new Exception("Solo el promotor puede borrar los archivos");

                HttpService vConector = new HttpService();

                msgInfoArchivos vRequestArchivos = new msgInfoArchivos()
                {
                    tipo = "4",
                    idcambio = Convert.ToString(Session["GETIDCAMBIO"])
                };

                String vResponseArchivos = "";
                HttpResponseMessage vHttpResponseArchivos = vConector.PostInfoArchivos(vRequestArchivos, ref vResponseArchivos);
                if (vHttpResponseArchivos.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgUpdateGeneral vInfoPruebasResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseArchivos);
                    if (vInfoPruebasResponse.updateCount1.Equals("1"))
                    {
                        Mensaje("Deposito 1 ha sido eliminado", WarningType.Info);
                    }
                }
                getDepositos(Convert.ToString(Session["GETIDCAMBIO"]));
                UpdateRefreshDepositos.Update();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnBorrarDeposito2_Click(object sender, EventArgs e)
        {
            try
            {
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                Generales vGenerales = new Generales();
                if (!vGenerales.PermisosEntrada(Permisos.Promotor, vConfigurations.resultSet1[0].idCargo))
                    throw new Exception("Solo el promotor puede borrar los archivos");

                HttpService vConector = new HttpService();

                msgInfoArchivos vRequestArchivos = new msgInfoArchivos()
                {
                    tipo = "5",
                    idcambio = Convert.ToString(Session["GETIDCAMBIO"])
                };

                String vResponseArchivos = "";
                HttpResponseMessage vHttpResponseArchivos = vConector.PostInfoArchivos(vRequestArchivos, ref vResponseArchivos);
                if (vHttpResponseArchivos.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgUpdateGeneral vInfoPruebasResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseArchivos);
                    if (vInfoPruebasResponse.updateCount1.Equals("1"))
                    {
                        Mensaje("Deposito 2 ha sido eliminado", WarningType.Info);
                    }
                }
                getDepositos(Convert.ToString(Session["GETIDCAMBIO"]));
                UpdateRefreshDepositos.Update();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        protected void BtnBorrarDeposito3_Click(object sender, EventArgs e)
        {
            try
            {
                vConfigurations = (msgLoginResponse)Session["AUTHCLASS"];
                Generales vGenerales = new Generales();
                if (!vGenerales.PermisosEntrada(Permisos.Promotor, vConfigurations.resultSet1[0].idCargo))
                    throw new Exception("Solo el promotor puede borrar los archivos");

                HttpService vConector = new HttpService();

                msgInfoArchivos vRequestArchivos = new msgInfoArchivos()
                {
                    tipo = "6",
                    idcambio = Convert.ToString(Session["GETIDCAMBIO"])
                };

                String vResponseArchivos = "";
                HttpResponseMessage vHttpResponseArchivos = vConector.PostInfoArchivos(vRequestArchivos, ref vResponseArchivos);
                if (vHttpResponseArchivos.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgUpdateGeneral vInfoPruebasResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgUpdateGeneral>(vResponseArchivos);
                    if (vInfoPruebasResponse.updateCount1.Equals("1"))
                    {
                        Mensaje("Deposito 3 ha sido eliminado", WarningType.Info);
                    }
                }
                getDepositos(Convert.ToString(Session["GETIDCAMBIO"]));
                UpdateRefreshDepositos.Update();
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        public void getDepositos(String vIdCambio)
        {
            try
            {
                HttpService vConector = new HttpService();

                msgInfoArchivos vRequestArchivos = new msgInfoArchivos()
                {
                    tipo = "3",
                    idcambio = vIdCambio
                };

                String vResponseArchivos = "";
                HttpResponseMessage vHttpResponseArchivos = vConector.PostInfoArchivos(vRequestArchivos, ref vResponseArchivos);
                if (vHttpResponseArchivos.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    msgInfoArchivosQueryResponse vInfoPruebasResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<msgInfoArchivosQueryResponse>(vResponseArchivos);
                    if (vInfoPruebasResponse.resultSet1.Count() > 0)
                    {
                        foreach (msgInfoArchivosQueryResponseItem itemArchivos in vInfoPruebasResponse.resultSet1)
                        {
                            if (itemArchivos.deposito1.Equals(""))
                            {
                                DIVDescargarDeposito1.Visible = false;
                                DIVDeposito1.Visible = true;
                            }
                            else
                            {
                                LbNombreDeposito1.Text = itemArchivos.depot1nombre;
                            }
                            if (itemArchivos.deposito2.Equals(""))
                            {
                                DIVDescargarDeposito2.Visible = false;
                                DIVDeposito2.Visible = true;
                            }
                            else
                            {
                                LbNombreDeposito2.Text = itemArchivos.depot2nombre;
                            }
                            if (itemArchivos.deposito3.Equals(""))
                            {
                                DIVDescargarDeposito3.Visible = false;
                                DIVDeposito3.Visible = true;
                            }
                            else
                            {
                                LbNombreDeposito3.Text = itemArchivos.depot3nombre;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex) { Mensaje(Ex.Message, WarningType.Danger); }
        }

        
    }
}


