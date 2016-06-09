using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace labcoreWS
{
    public class usuariosWShusi : IusuariosWShusi
    {

        public string usrWindows(string usrWindows)
        {
            string respuesta = string.Empty;
            using (SqlConnection DBConexion = new SqlConnection(Properties.Settings.Default.DBConexionXX))
            {
                DBConexion.Open();
                string qryConsulta = "SELECT IdUsuario,cod_usua,nom_usua FROM ASI_USUA WHERE UsuarioWin='" + usrWindows + "' AND  ind_esta='A'";
                SqlCommand cmdConsulta = new SqlCommand(qryConsulta, DBConexion);
                SqlDataReader rdConsulta = cmdConsulta.ExecuteReader();
                if(rdConsulta.HasRows)
                {
                    rdConsulta.Read();
                    respuesta = rdConsulta.GetInt16(0) + "|" + rdConsulta.GetString(2);
                }
                else
                {
                    respuesta = "0|No se encontro el usuario de Windows:" + usrWindows;
                }
            }
            return respuesta;
        }

        public string usrSahi(string usrSAHI)
        {
            //string qryConsulta = "SELECT IdUsuario,UsuarioWin,nom_usua FROM ASI_USUA WHERE cod_usua='" + usrSAHI + "'  ind_esta='A'";

            string respuesta = string.Empty;
            using (SqlConnection DBConexion = new SqlConnection(Properties.Settings.Default.DBConexionXX))
            {
                DBConexion.Open();
                string qryConsulta = "SELECT IdUsuario,UsuarioWin,nom_usua FROM ASI_USUA WHERE cod_usua='" + usrSAHI + "' AND  ind_esta='A'";
                SqlCommand cmdConsulta = new SqlCommand(qryConsulta, DBConexion);
                SqlDataReader rdConsulta = cmdConsulta.ExecuteReader();
                if (rdConsulta.HasRows)
                {
                    rdConsulta.Read();
                    respuesta = rdConsulta.GetInt16(0) + "|" + rdConsulta.GetString(2);
                }
                else
                {
                    respuesta = "0|No se encontro el usuario de SAHI:" + usrSAHI;
                }
            }
            return respuesta;
        }
    }
}
