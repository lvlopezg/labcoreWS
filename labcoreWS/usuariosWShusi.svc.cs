using System;
using System.Data.SqlClient;

namespace labcoreWS
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'usuariosWShusi'
    public class usuariosWShusi : IusuariosWShusi
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'usuariosWShusi'
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="usrWindows"></param>
        /// <returns></returns>
        public string usrWindows(string usrWindows)
        {
            string respuesta = string.Empty;
            using (SqlConnection DBConexion = new SqlConnection(Properties.Settings.Default.DBConexion))
            {
                DBConexion.Open();
                string qryConsulta = "SELECT IdUsuario,cod_usua,nom_usua FROM ASI_USUA WHERE UsuarioWin='" + usrWindows + "' AND  ind_esta='A'";
                SqlCommand cmdConsulta = new SqlCommand(qryConsulta, DBConexion);
                SqlDataReader rdConsulta = cmdConsulta.ExecuteReader();
                if (rdConsulta.HasRows)
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

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'usuariosWShusi.usrSahi(string)'
        public string usrSahi(string usrSAHI)
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'usuariosWShusi.usrSahi(string)'
        {
            //string qryConsulta = "SELECT IdUsuario,UsuarioWin,nom_usua FROM ASI_USUA WHERE cod_usua='" + usrSAHI + "'  ind_esta='A'";

            string respuesta = string.Empty;
            using (SqlConnection DBConexion = new SqlConnection(Properties.Settings.Default.DBConexion))
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
        /// <summary>
        /// Esta operacion retorna informacion de usuario
        /// </summary>
        /// <param name="Idusuario"></param>
        /// <returns>cod_usua|UsuarioWin|nom_usua</returns>
        public string idUsuaXcodUsua(int Idusuario)
        {
            string respuesta = string.Empty;
            using (SqlConnection DBConexion = new SqlConnection(Properties.Settings.Default.DBConexion))
            {
                DBConexion.Open();
                string qryConsulta = @"SELECT cod_usua,UsuarioWin,nom_usua,B.NumRegistro,A.ind_esta FROM ASI_USUA A
INNER JOIN hcePersonal B ON A.IdUsuario = B.IdPersonal
 WHERE A.idUsuario = @Idusuario"; //+@" AND A.ind_esta = 'A'"  se quita la condicion, y se retorna el estado en la respuesta
                SqlCommand cmdConsulta = new SqlCommand(qryConsulta, DBConexion);
                cmdConsulta.Parameters.Add("@Idusuario",System.Data.SqlDbType.Int).Value= Idusuario;
                SqlDataReader rdConsulta = cmdConsulta.ExecuteReader();
                if (rdConsulta.HasRows)
                {
                    string codigo = string.Empty;
                    string usrwindows = string.Empty;
                    string nombre = string.Empty;
                    string nroRegistro = string.Empty;
                    string estado = string.Empty;
                    rdConsulta.Read();
                    if (rdConsulta.IsDBNull(0)) { codigo = ""; } else { codigo = rdConsulta.GetString(0); }
                    if (rdConsulta.IsDBNull(1)) { usrwindows = ""; } else { usrwindows = rdConsulta.GetString(1); }
                    if (rdConsulta.IsDBNull(2)) { nombre = ""; } else { nombre = rdConsulta.GetString(2); }
                    if (rdConsulta.IsDBNull(3)) { nroRegistro = ""; } else { nroRegistro = rdConsulta.GetString(3); }
                    if (rdConsulta.IsDBNull(4)) { estado = ""; } else { estado = rdConsulta.GetString(4); }
                    respuesta = codigo + "|" + usrwindows + "|" + nombre + "|" + nroRegistro + "|" + estado;
                }
                else
                {
                    respuesta = "0|No se encontro el usuario de SAHI:" + Idusuario;
                }
            }
            return respuesta;
        }

        public string usuarioXidPersonal(string idPersonal)
        {
            string respuesta = string.Empty;
            using (SqlConnection DBConexion = new SqlConnection(Properties.Settings.Default.DBConexion))
            {
                DBConexion.Open();
                string qryConsulta = @"SELECT cod_usua,UsuarioWin,nom_usua,B.NumRegistro,A.ind_esta FROM ASI_USUA A
INNER JOIN hcePersonal B ON A.IdUsuario = B.IdPersonal
 WHERE B.idPersonal = @idPersonal";  //+@" AND A.ind_esta = 'A'"  se quita la condicion, y se retorna el estado en la respuesta
                SqlCommand cmdConsulta = new SqlCommand(qryConsulta, DBConexion);
                cmdConsulta.Parameters.Add("@idPersonal",System.Data.SqlDbType.Int).Value= idPersonal;
                SqlDataReader rdConsulta = cmdConsulta.ExecuteReader();
                if (rdConsulta.HasRows)
                {
                    string codigo = string.Empty;
                    string usrwindows = string.Empty;
                    string nombre = string.Empty;
                    string nroRegistro = string.Empty;
                    string estado = string.Empty;
                    rdConsulta.Read();
                    if (rdConsulta.IsDBNull(0)) { codigo = ""; } else { codigo = rdConsulta.GetString(0); }
                    if (rdConsulta.IsDBNull(1)) { usrwindows = ""; } else { usrwindows = rdConsulta.GetString(1); }
                    if (rdConsulta.IsDBNull(2)) { nombre = ""; } else { nombre = rdConsulta.GetString(2); }
                    if (rdConsulta.IsDBNull(3)) { nroRegistro = ""; } else { nroRegistro = rdConsulta.GetString(3); }
                    if (rdConsulta.IsDBNull(4)) { estado = ""; } else { estado = rdConsulta.GetString(4); }
                    respuesta = codigo + "|" + usrwindows + "|" + nombre + "|" + nroRegistro + "|" + estado;
                }
                else
                {
                    respuesta = "0|No se encontro el usuario de SAHI:" + idPersonal;
                }
            }
            return respuesta;
        }
    }
}
