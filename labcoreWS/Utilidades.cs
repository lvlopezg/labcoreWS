using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using NLog;

namespace labcoreWS
{
    public class Utilidades
    {
        private static Logger logLabcore = LogManager.GetCurrentClassLogger();
        public DateTime formatoFecha(string fecha)
        {
            int year = int.Parse(fecha.Substring(0, 4));
            int mes = int.Parse(fecha.Substring(4, 2));
            int dia = int.Parse(fecha.Substring(6, 2));
            int hora = int.Parse(fecha.Substring(8, 2));
            int minutos = int.Parse(fecha.Substring(10, 2));
            int segs = int.Parse(fecha.Substring(12, 2));
            DateTime fechaFmt = new DateTime(year, mes, dia, hora, minutos, segs);
            return fechaFmt;
        }

        public string fechaHL7(DateTime fechaIn)
        {
            string Mes = fechaIn.Month.ToString();
            if (Mes.Length < 2) { Mes = "0" + Mes; }
            string Dia = fechaIn.Day.ToString();
            if (Dia.Length < 2) { Dia = "0" + Dia; }
            string Hora = fechaIn.Hour.ToString();
            if (Hora.Length < 2) { Hora = "0" + Hora; }
            string Minutos = fechaIn.Minute.ToString();
            if (Minutos.Length < 2) { Minutos = "0" + Minutos; }
            string Segundos = fechaIn.Second.ToString();
            if (Segundos.Length < 2) { Segundos = "0" + Segundos; }
            return fechaIn.Year.ToString() + Mes + Dia + Hora + Minutos + Segundos;

        }

        public string  nombreMedicos(string idMedico)
        {
            SqlConnection Conex = new SqlConnection(Properties.Settings.Default.DBConexionXX);
            Conex.Open();
            string strConsSoli = "SELECT NOM_USUA FROM ASI_USUA WHERE IdUsuario=" + idMedico;
            SqlCommand cmdConsSoli = new SqlCommand(strConsSoli, Conex);
            SqlDataReader nroMsgReader = cmdConsSoli.ExecuteReader();
            nroMsgReader.Read();
            return nroMsgReader.GetString(0);

        }
        public string[] idenpaciente(string atencion)
        {
            SqlConnection Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX);
            using (Conex)
            {
                string[] identificacion=new string[2];
                Conex.Open();
                string strConsultar = "SELECT idTipoDoc, NumDocumento from admCliente A, admAtencion B where A.IdCliente=B.IdCliente AND B.IdAtencion="+atencion;
                SqlCommand cmdConsulta = new SqlCommand(strConsultar, Conex);
                SqlDataReader conCursor = cmdConsulta.ExecuteReader();
                if (conCursor.HasRows)
                {
                    conCursor.Read();
                    identificacion[0] = conCursor.GetByte(0).ToString();
                    identificacion[1] = conCursor.GetString(1);
                }
                else
                {
                    identificacion[0] = "N";
                    identificacion[1] = "N";
                }
                return identificacion;
            }
        }

        public string[] productoDatos(string idProducto)
        {
            string[] datos=new  string[5];
            SqlConnection Conex = new SqlConnection(Properties.Settings.Default.DBConexionXX);
            Conex.Open();
            using (Conex)
            {
                string strConsProd = "SELECT NomProducto,CodProducto,CodLegal,IndHabilitado,IndPos FROM proProducto WHERE IdProducto=" + idProducto;
                SqlCommand cmdConsProd = new SqlCommand(strConsProd, Conex);
                SqlDataReader nroMsgReader = cmdConsProd.ExecuteReader();
                nroMsgReader.Read();
                datos[0] = nroMsgReader.GetString(0);
                datos[1] = nroMsgReader.GetString(1);
                datos[2] = nroMsgReader.GetString(2);
                datos[3] = nroMsgReader.GetBoolean(3).ToString();
                datos[4] = nroMsgReader.GetBoolean(4).ToString();
            }
            return datos;
        }

        public Int32 consecutivoSistabla(string tabla)
        {
            //hceNotasAte
            try
            {
                Int32 nroConsecutivo = 0;
                using (SqlConnection Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX))
                {
                    Conex.Open();
                    SqlTransaction txResultados = Conex.BeginTransaction("TxResultados");
                    string NroNota = "SELECT (Consecutivo + 1) FROM sisTabla WITH (NOLOCK) WHERE NomTabla = '" + tabla + "'";
                    SqlCommand cmdNroNota = new SqlCommand(NroNota, Conex, txResultados);
                    
                    SqlDataReader readerNroNota = cmdNroNota.ExecuteReader();
                    if (readerNroNota.HasRows)
                    {
                        readerNroNota.Read();
                        nroConsecutivo = readerNroNota.GetInt32(0);
                        readerNroNota.Close();
                        readerNroNota.Dispose();
                        string qryActualizaConsec = "UPDATE sistabla SET consecutivo = " + nroConsecutivo + " WHERE nomTabla = '" + tabla + "'";
                        SqlCommand cmdUpdNota = new SqlCommand(qryActualizaConsec, Conex, txResultados);
                        if (cmdUpdNota.ExecuteNonQuery() < 0)
                        {
                            logLabcore.Warn("No se Actualizo el Consecutivo:" + nroConsecutivo);
                            txResultados.Rollback();
                        }
                        else
                        {
                            txResultados.Commit();
                        }
                    }
                }
                return nroConsecutivo;
            }
            catch(Exception exp)
            {
                logLabcore.Warn(exp.Message, "Excepcion Obteniendo consecutivo para:" + tabla);
                return 0;
            }
        }

        public string lineaResultado(string titulo,string valor,string unidad,string valorRef)
        {
            string respuesta = string.Empty;
            string spcTitulo = new string(' ', 20-titulo.Length);
            if (valor.Length > 10) { valor = valor.Substring(0, 10); }
            string spcValor = new string(' ', 10-valor.Length);
            if (unidad.Length > 9) { unidad = unidad.Substring(0, 9); }
            string spcUnidad = new string(' ', 9-unidad.Length);
            if(valorRef.Length>12)
            { valorRef = valorRef.Substring(0, 13); }
            int lTotal=56;
            int lValor=valor.Length;
            int lUnidad=unidad.Length;
            int lValorRef=valorRef.Length;
            lTotal=lTotal-titulo.Length;
            respuesta = titulo + spcTitulo+"  ";
            respuesta = respuesta + valor + spcValor + "   ";
            respuesta = respuesta + unidad + spcUnidad;
            respuesta = respuesta + valorRef;
            return respuesta;
        }

        public string lineaResultadoST(string titulo, string valor, string unidad, string valorRef)
        {
            string respuesta = string.Empty;
            string spcTitulo = new string(' ',20 - titulo.Length);
            if (unidad.Length > 1 && valorRef.Length > 1)
            {
                if (valor.Length > 10) { valor = valor.Substring(0, valor.Length); }
                string spcValor = new string(' ', 10 - valor.Length);
                if (unidad.Length > 9) { unidad = unidad.Substring(0, 9); }
                string spcUnidad = new string(' ', 9 - unidad.Length);
                if (valorRef.Length > 12)
                { valorRef = valorRef.Substring(0, 13); }
                //string spcValorRef = new string(' ', 5);
                int lTotal = 56;

                int lValor = valor.Length;
                int lUnidad = unidad.Length;
                int lValorRef = valorRef.Length;
                lTotal = lTotal - titulo.Length;
                respuesta = titulo + spcTitulo + "  ";
                respuesta = respuesta + valor + spcValor + "   ";
                respuesta = respuesta + unidad + spcUnidad;
                respuesta = respuesta + valorRef;
                return respuesta;
            }
            else
            {
                if (valor.Length > 10) { valor = valor.Substring(0, valor.Length); }
                string spcValor = new string(' ',valor.Length);
                int lValor = valor.Length;
                int lUnidad = unidad.Length;
                int lValorRef = valorRef.Length;
                respuesta = titulo + spcTitulo + "  ";
                respuesta = respuesta + valor + spcValor ;
                return respuesta;
            }
        }

        public string[] separarApellidos(string apellidos)
        {
            string[] Apellidos = apellidos.Split(' ');
            string primerApellido = string.Empty;
            string segundoApellido = string.Empty;
            for (int i = 0; i <= Apellidos.Length; i++)
            {
                if (Apellidos[i].Length < 3)
                {
                    primerApellido = primerApellido + " " + Apellidos[i];
                }
                else
                {
                    primerApellido = primerApellido + " " + Apellidos[i];
                    for (int j = i + 1; j < Apellidos.Length; j++)
                    {
                        segundoApellido = segundoApellido + " " + Apellidos[j];
                    }
                    i = Apellidos.Length;
                }

            }
            Apellidos[0] = primerApellido;
            if (Apellidos.Length > 1)
            {
                Apellidos[1] = segundoApellido;
            }
            return Apellidos;
        }

        public string notificaFalla(string mensaje)
        {
            string respuesta = string.Empty;
            string _mensaje =DateTime.Now.ToString()+mensaje;
            string celular = Properties.Resources.nroReporte;
            SqlConnection conexion = new SqlConnection(Properties.Settings.Default.DBConexion);
            using (conexion)
            {
                conexion.Open();
                WSAldea.SmsSendSoapClient mensajeAenviar = new WSAldea.SmsSendSoapClient();
                respuesta = mensajeAenviar.smsSendSoap("Husi", "Husi123*", 57, celular, _mensaje, "");
                string FechaMsg = DateTime.Now.Date.ToShortDateString();
                string HoraMsg = DateTime.Now.ToLongTimeString();
                String[] RptaTot = respuesta.Split('|');
                string codRpta = RptaTot[0];
                string msgRpta = RptaTot[1].Replace("'", "").ToString();
                string Insertar = "INSERT INTO hceInterconsultaSMS (FECHA_MSG,CONT_MSG,COD_RPTA,MSG_RPTA,MSG_NTEL,MSG_ESPE,DOC_PCTE,NRO_SOLI,NOM_PCTE,UBIC_PCTE,TIPO_INTR,NROR_MSG) VALUES (getdate(),'" + _mensaje + "','" + codRpta + "','" + msgRpta + "','" + celular + "','" + 999 + "','',999,'Trazabilidad','Trazabilidad','Errores'," + 1 + ")";
                SqlCommand sqlIns1 = new SqlCommand(Insertar, conexion);
                sqlIns1.ExecuteNonQuery();
            }
            return respuesta;
        }
    }
}