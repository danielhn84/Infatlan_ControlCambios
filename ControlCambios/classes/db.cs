using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ControlCambios.classes
{
    public enum ServerType
    {
        ControlCambios
    }
    public class db
    {
        SqlConnection vConexion;
        public db(ServerType vType)
        {
            switch (vType)
            {
                case ServerType.ControlCambios:
                    vConexion = new SqlConnection(ConfigurationManager.AppSettings["SQLServer"]);
                    break;
            }
        }

        public DataTable obtenerDataTable(String vQuery)
        {
            DataTable vDatos = new DataTable();
            try
            {
                SqlDataAdapter vDataAdapter = new SqlDataAdapter(vQuery, vConexion);
                vDataAdapter.Fill(vDatos);
            }
            catch
            {
                throw;
            }
            return vDatos;
        }

        public DataSet obtenerDataSet(String vQuery)
        {
            DataSet vDatos = new DataSet();
            try
            {
                SqlDataAdapter vDataAdapter = new SqlDataAdapter(vQuery, vConexion);
                vDataAdapter.Fill(vDatos);
            }
            catch
            {
                throw;
            }
            return vDatos;
        }

        public int ejecutarSql(String vQuery)
        {
            int vResultado = 0;
            try
            {
                SqlCommand vSqlCommand = new SqlCommand(vQuery, vConexion);
                vSqlCommand.CommandType = CommandType.Text;

                vConexion.Open();
                vResultado = vSqlCommand.ExecuteNonQuery();
                vConexion.Close();
            }
            catch (Exception Ex)
            {
                String vError = Ex.Message;
                vConexion.Close();
                throw;
            }
            return vResultado;
        }

        public int ejecutarSQLGetValue(String vQuery)
        {
            int vResultado = 0;
            try
            {
                SqlCommand vSqlCommand = new SqlCommand(vQuery, vConexion);
                vSqlCommand.CommandType = CommandType.Text;

                vConexion.Open();
                vResultado = (Int32)vSqlCommand.ExecuteScalar();
                vConexion.Close();
            }
            catch (Exception Ex)
            {
                String vError = Ex.Message;
                vConexion.Close();
                throw;
            }
            return vResultado;
        }

        public String ejecutarSQLGetValueString(String vQuery)
        {
            String vResultado = String.Empty;
            try
            {
                SqlCommand vSqlCommand = new SqlCommand(vQuery, vConexion);
                vSqlCommand.CommandType = CommandType.Text;

                vConexion.Open();
                vResultado = (String)vSqlCommand.ExecuteScalar();
                vConexion.Close();
            }
            catch (Exception Ex)
            {
                String vError = Ex.Message;
                vConexion.Close();
                throw;
            }
            return vResultado;
        }
    }
}