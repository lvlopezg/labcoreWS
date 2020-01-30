using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using NLog;
using System.Data;
using System.Threading.Tasks;
using System.Configuration;

namespace labcoreWS
{
    /// <summary>
    /// Clase que contiene varias utlidades que soportan la operacion de los mensajes HL7
    /// </summary>
    public class Utilidades
    {
        private static Logger logLabcore = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Metodo que retorna una fecha  en formato Fecha, apartir de un string continuo utilizado en HL7
        /// </summary>
        /// <param name="fecha">string que contiene la fecha en un string contunuo de los digitos (yyyyMMddHHmmSS)</param>
        /// <returns>Retorna la fecha en formato Fecha</returns>
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
        /// <summary>
        /// Metodo para transforma una fecha en un string continuo de los digitos que hacen parte de la fecha y hora
        /// </summary>
        /// <param name="fechaIn">Fecha a convertir en formato yyyy-MM-dd HH:mm:ss</param>
        /// <returns>Un string continuo con los digitos de la fecha (yyyMMddHHmmss)</returns>
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
        /// <summary>
        /// Metodo que obtiene el nombre de un Medico a partir del IdUsuario
        /// </summary>
        /// <param name="idMedico">idUsuario del Medico en SAHI</param>
        /// <returns>Nombre del Medico de la tabla ASI_USUA</returns>
        public string nombreMedicos(string idMedico)
        {
            SqlConnection Conex = new SqlConnection(Properties.Settings.Default.DBConexionXX);
            Conex.Open();
            string strConsSoli = "SELECT NOM_USUA FROM ASI_USUA WHERE IdUsuario=" + idMedico;
            SqlCommand cmdConsSoli = new SqlCommand(strConsSoli, Conex);
            SqlDataReader nroMsgReader = cmdConsSoli.ExecuteReader();
            nroMsgReader.Read();
            return nroMsgReader.GetString(0);

        }

        /// <summary>
        /// Metodo que obtiene la identificacion de un paciente en SAHI, a partir de la atencion
        /// </summary>
        /// <param name="atencion">Numero de la atencion para la cual se desea obtener la identidicacion</param>
        /// <returns>Retorna un arreglo de string, con tipo de Documento y Numero de Documento [tipo,numero]</returns>
        public string[] idenpaciente(string atencion)
        {
            SqlConnection Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX);
            using (Conex)
            {
                string[] identificacion = new string[2];
                Conex.Open();
                string strConsultar = "SELECT idTipoDoc, NumDocumento from admCliente A, admAtencion B where A.IdCliente=B.IdCliente AND B.IdAtencion=" + atencion;
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
        /// <summary>
        /// Informacion de un Producto.
        /// </summary>
        /// <param name="idProducto">idProducto del producto en SAHI</param>
        /// <returns>[Nombre Producto,Codigo Producto,Codigo Legal,Indicador de Habilitado,Indicador Pos] </returns>
        public string[] productoDatos(string idProducto)
        {
            string[] datos = new string[5];
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

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'Utilidades.consecutivoSistabla(string)'
        public Int32 consecutivoSistabla(string tabla)
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'Utilidades.consecutivoSistabla(string)'
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
            catch (Exception exp)
            {
                logLabcore.Warn(exp.Message, "Excepcion Obteniendo consecutivo para:" + tabla);
                return 0;
            }
        }

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'Utilidades.lineaResultado(string, string, string, string)'
        public string lineaResultado(string titulo, string valor, string unidad, string valorRef)
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'Utilidades.lineaResultado(string, string, string, string)'
        {
            string respuesta = string.Empty;
            string nvotitulo = tituloLargo(titulo);
            //string spcTitulo = new string((' ', 20-titulo.Length);
            if (valor.Length > 10) { valor = valor.Substring(0, 10); }
            string spcValor = new string(' ', 10 - valor.Length);
            if (unidad.Length > 9) { unidad = unidad.Substring(0, 9); }
            string spcUnidad = new string(' ', 9 - unidad.Length);
            if (valorRef.Length > 12)
            { valorRef = valorRef.Substring(0, 13); }
            int lTotal = 56;
            int lValor = valor.Length;
            int lUnidad = unidad.Length;
            int lValorRef = valorRef.Length;
            lTotal = lTotal - titulo.Length;
            respuesta = nvotitulo;
            respuesta = respuesta + valor + spcValor + "   ";
            respuesta = respuesta + unidad + spcUnidad;
            respuesta = respuesta + valorRef;
            return respuesta;
        }

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'Utilidades.lineaResultadoST(string, string, string, string)'
        public string lineaResultadoST(string titulo, string valor, string unidad, string valorRef)
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'Utilidades.lineaResultadoST(string, string, string, string)'
        {
            string respuesta = string.Empty;
            //string spcTitulo = new string(' ',20 - titulo.Length);
            string nvotitulo = tituloLargo(titulo);
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
                respuesta = nvotitulo + "  ";
                respuesta = respuesta + valor + spcValor + "   ";
                respuesta = respuesta + unidad + spcUnidad;
                respuesta = respuesta + valorRef;
                return respuesta;
            }
            else
            {
                if (valor.Length > 10) { valor = valor.Substring(0, valor.Length); }
                string spcValor = new string(' ', valor.Length);
                int lValor = valor.Length;
                int lUnidad = unidad.Length;
                int lValorRef = valorRef.Length;
                respuesta = nvotitulo + "  ";
                respuesta = respuesta + valor + spcValor;
                return respuesta;
            }
        }

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'Utilidades.tituloLargo(string)'
        public string tituloLargo(string titulo)
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'Utilidades.tituloLargo(string)'
        {
            string nvotitulo = string.Empty;
            if (titulo.Length > 20)
            {
                int chrsRest_Titulo = titulo.Length % 20;
                string spc_chrsRest_Titulo = new string(' ', 20 - chrsRest_Titulo);
                int lineasTitulo = (titulo.Length - chrsRest_Titulo) / 20;
                int ultimo = 0;
                nvotitulo = titulo.Substring(0, 20) + "\r\n";
                for (int x = 1; x < lineasTitulo; x++)
                {
                    nvotitulo = nvotitulo + titulo.Substring(x * 20, 20) + "\r\n";
                    ultimo = x * 20 + 20;
                }
                nvotitulo = nvotitulo + titulo.Substring(ultimo, chrsRest_Titulo) + spc_chrsRest_Titulo;
            }
            else
            {
                nvotitulo = titulo;
            }
            return nvotitulo;
        }
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'Utilidades.separarApellidos(string)'
        public string[] separarApellidos(string apellidos)
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'Utilidades.separarApellidos(string)'
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

        /// <summary>
        /// Notifica la alarma de falla en una operacion.
        /// </summary>
        /// <param name="mensaje">mensaje que se desea enviar.</param>
        /// <returns></returns>
        public string notificaFalla(string mensaje)
        {
            string respuesta = string.Empty;
            string _mensaje = DateTime.Now.ToString() + mensaje;
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string notificaFallaDetalle(string Atencion, string Solicitud, string Cups, string mensaje)
        {
            string respuesta = string.Empty;
            string _mensaje = DateTime.Now.ToString() + mensaje;
            string celular = Properties.Resources.nroReporte;
            SqlConnection conexion = new SqlConnection(Properties.Settings.Default.DBConexion);
            using (conexion)
            {
                conexion.Open();
                string strValidaMSG = "SELECT  FECHA_MSG FROM TAT_MSG_CEL_ENV WHERE ATEN_MSG=@atencion AND SOLI_MSG=@solicitud AND CUPS_MSG=@cups";
                SqlCommand cmdValidaMSG = new SqlCommand(strValidaMSG, conexion);
                cmdValidaMSG.Parameters.Add("@atencion", SqlDbType.VarChar);
                cmdValidaMSG.Parameters.Add("@solicitud", SqlDbType.VarChar);
                cmdValidaMSG.Parameters.Add("@cups", SqlDbType.VarChar);
                cmdValidaMSG.Parameters["@atencion"].Value = Atencion;
                cmdValidaMSG.Parameters["@solicitud"].Value = Solicitud;
                cmdValidaMSG.Parameters["@cups"].Value = Cups;
                SqlDataReader rdValidaMSG = cmdValidaMSG.ExecuteReader();
                if (!rdValidaMSG.HasRows)
                {
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
                    string strInsertMsgs = "INSERT INTO TAT_MSG_CEL_ENV(FECHA_MSG,ATEN_MSG,SOLI_MSG,CUPS_MSG) VALUES(@fecha,@atencion,@solicitud,@cups)";
                    SqlCommand cmdInsertMsgs = new SqlCommand(strInsertMsgs, conexion);
                    cmdInsertMsgs.Parameters.Add("@fecha", SqlDbType.DateTime);
                    cmdInsertMsgs.Parameters.Add("@atencion", SqlDbType.VarChar);
                    cmdInsertMsgs.Parameters.Add("@solicitud", SqlDbType.VarChar);
                    cmdInsertMsgs.Parameters.Add("@cups", SqlDbType.VarChar);
                    cmdInsertMsgs.Parameters["@fecha"].Value = DateTime.Now;
                    cmdInsertMsgs.Parameters["@atencion"].Value = Atencion;
                    cmdInsertMsgs.Parameters["@solicitud"].Value = Solicitud;
                    cmdInsertMsgs.Parameters["@cups"].Value = Cups;
                    cmdInsertMsgs.ExecuteNonQuery();
                }
                else
                {
                    respuesta = "No se envia. Mensaje Repetido";
                }
            }
            return respuesta;
        }

        public async Task validationResultadoCritico(string atencion, string especialidad) {

            clienteDatosPcte.IdatosPacienteClient clientePcte = new clienteDatosPcte.IdatosPacienteClient();
            clienteDatosPcte.pacienteS1 paciente = clientePcte.datosXatencion(Int32.Parse(atencion));
            string mensajeR = "<br> Paciente: " + paciente.Nombre + " " + paciente.Apellidos + " Doc:" + paciente.numDocumento + " tiene resultado crítico de laboratorio.";
			
			//consultar tipo del atencion
			bool envioCorreo = true;			
			using (SqlConnection Conex = new SqlConnection(Properties.Settings.Default.DBConexion))
			{
				try
				{
					Conex.Open();
					string strUpdateTrace = "select t.IdAtenTipoBase,a.IndActivado from admAtencion a INNER JOIN admAtencionTipo t on a.IdAtencionTipo=t.IdAtencionTipo where a.idAtencion = @atencion ";
					SqlCommand cmdupdateTraceVta = new SqlCommand(strUpdateTrace, Conex);
					cmdupdateTraceVta.Parameters.Add("@atencion", System.Data.SqlDbType.Int).Value = atencion;
					var result = await cmdupdateTraceVta.ExecuteReaderAsync();
					var tipobase = 0;
					var estado = false;
					if (result.HasRows)
					{
						result.Read();
						tipobase = result.GetInt16(0);
						estado = result.GetBoolean(1);
					}
					if ((tipobase == 2 || tipobase == 3) && estado)
						envioCorreo = false;
				}
				catch (Exception e)
				{
					logLabcore.Info("Error al enviar el resultado critico correo/msn:: " + e.Message);
				}

			}
			try
			{
				if (envioCorreo)
				{
					mensajeR += " Numero de atención: "+atencion;
					var destino =  Properties.Settings.Default.destinoCritico;
                    var origen = Properties.Settings.Default.origenCritico;

					var correo = new clienteSMS.smsHUSISoapClient("smsHUSISoap");
					var response = correo.correoHusi(origen, destino, " Resultado crítico laboratorio", mensajeR);
                    logLabcore.Info(" Se envio correo");
				}
				else
				{
					mensajeR += " Defina conducta antes de 60 min y regístrela.";
					clienteSMS.smsHUSISoapClient mensajeRC = new clienteSMS.smsHUSISoapClient("smsHUSISoap");
					string rptaSMS = mensajeRC.smsGeneralHusi(paciente.numDocumento, especialidad, "0", paciente.idPaciente.ToString(), paciente.ubicacionActual, mensajeR);
                    logLabcore.Info($" Se envio sms {rptaSMS}");
				}
			}
			catch (Exception e)
			{
                logLabcore.Info("Error al enviar el resultado critico correo/msn:: " + e.Message);
			}
		}
    }
}