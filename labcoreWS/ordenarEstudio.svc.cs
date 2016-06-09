using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace labcoreWS
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
	public class ordenarEstudio : IordenarEstudio,IDisposable
	{
		SqlConnection Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX);
		SqlConnection Conex2 = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX);
		Utilidades utilLocal = new Utilidades();
		private static Logger logLabcore = LogManager.GetCurrentClassLogger();

        //private static Logger logOrdenes = LogManager.GetCurrentClassLogger();
        //private static Logger logSolicitudes = LogManager.GetCurrentClassLogger();
        //private static Logger logTomaMta = LogManager.GetCurrentClassLogger();
        //private static Logger logVenta = LogManager.GetCurrentClassLogger();
        //private static Logger logResultados = LogManager.GetCurrentClassLogger();

		public async Task<string> ordenarAsync(string solicitud, string orden, string atencion, Int32 NroMsg)
		{
			string[] datosPcte = utilLocal.idenpaciente(atencion);

			MSHClass objetoMSH = new MSHClass();
			PIDClass objetoPID = new PIDClass();
			PV1Class objetoPv1 = new PV1Class();
			IN1Class objetoIn1 = new IN1Class();
			ORCClass objetoOrc = new ORCClass();
			OBRClass objetoObr = new OBRClass();

			#region RegionMSH
			objetoMSH.Msh7 = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
			objetoMSH.Msh9 = "ORM^O01";
			objetoMSH.Msh10 = NroMsg.ToString();
			objetoMSH.Msh11 = "P";
			objetoMSH.Msh12 = "2.3";
			objetoMSH.Msh15 = "AL";

			#endregion
			#region RegionPID
            try
            {
                using (Conex)
                {
                    Conex.Open();
                    string strConsPcte = "SELECT IdCliente,NomCliente,ApeCliente,IdTipoDoc,NumDocumento,IdSexo,FecNacimiento,CorreoE,TelCelular FROM admCliente WHERE idTipoDoc=" + datosPcte[0] + " AND NumDocumento='" + datosPcte[1] + "'";
                    SqlCommand cmdConsPcte = new SqlCommand(strConsPcte, Conex);
                    SqlDataReader PcteReader = cmdConsPcte.ExecuteReader();
                    if (PcteReader.HasRows)
                    {
                        PcteReader.Read();
                        objetoPID.Pid2 = PcteReader.GetByte(3).ToString() + "^" + PcteReader.GetString(4);
                        string[] Apellidos = utilLocal.separarApellidos(PcteReader.GetString(2));
                        if (Apellidos.Length > 1)
                        {
                            objetoPID.Pid5 = Apellidos[0] + "^" + Apellidos[1] + "^" + PcteReader.GetString(1);
                        }
                        else
                        {
                            objetoPID.Pid5 = Apellidos[0] + "^^" + PcteReader.GetString(1);
                        }
                        objetoPID.Pid7 = utilLocal.fechaHL7(PcteReader.GetDateTime(6));
                        if (PcteReader.GetInt16(5) == 1) { objetoPID.Pid8 = "M"; } else { objetoPID.Pid8 = "F"; }
                        if (PcteReader.IsDBNull(8)) { objetoPID.Pid13 = "0"; } else { objetoPID.Pid13 = PcteReader.GetString(8); }
                    }
                }
            }
            catch (SqlException sqlExp1)
            {
                utilLocal.notificaFalla("SqlException Generando HL7:(Solicitud:" + solicitud + " Orden:" + orden + " Atn:" + atencion + "):" + sqlExp1.Message);
                logLabcore.Warn("SqlException Generando HL7:" + sqlExp1.Message + " " + sqlExp1.StackTrace);
                return "SqlException En ordenarEstudio" + sqlExp1.StackTrace;
            }
            catch (Exception exp1)
            {
                utilLocal.notificaFalla("Falla Generando HL7:(Solicitud:" + solicitud + " Orden:" + orden + " Atn:" + atencion + "):" + exp1.Message);
                logLabcore.Warn("Exception Generando HL7 En ordenarAsync():" + exp1.StackTrace);
                return "Exception Generando HL7 ordenarAsync():" + exp1.StackTrace;
            }
			#endregion
			#region RegionPV1
            try
            {
                Conex.ConnectionString = Properties.Settings.Default.LabcoreDBConXX;
                using (Conex)
                {
                    Conex.Open();
                    //PV1
                    string strConsTipoAtn = "SELECT A.IdAtenTipoBase,nomAtenTipoBase,D.IdUbicacion,D.NomUbicacion,B.IdAtencionTipo,B.NomAtencionTipo FROM admAtenTipoBase A,admAtencionTipo B,admAtencion C,genUbicacion D WHERE A.IdAtenTipoBase=B.IdAtenTipoBase AND B.IdAtencionTipo=C.IdAtencionTipo AND C.IdAtencion=" + atencion + " AND C.IdUbicacionActual=D.IdUbicacion";
                    SqlCommand cmdConsTipoAtn = new SqlCommand(strConsTipoAtn, Conex);
                    SqlDataReader tipoAtnReader = cmdConsTipoAtn.ExecuteReader();
                    if (tipoAtnReader.HasRows)
                    {
                        tipoAtnReader.Read();
                        ////////////////////switch(tipoAtnReader.GetString(1))
                        ////////////////////{
                        ////////////////////    case "Hospitalización":
                        ////////////////////        objetoPv1.PV12="I";
                        ////////////////////        break;
                        ////////////////////    case "Urgencias":
                        ////////////////////        objetoPv1.PV12="E" ;      //// quitado a solicitud de Dra Sandra Cardona
                        ////////////////////        break;
                        ////////////////////    case "Ambulatoria":
                        ////////////////////        objetoPv1.PV12="O" ;
                        ////////////////////        break;
                        ////////////////////    case "Consulta Externa":
                        ////////////////////        objetoPv1.PV12="O" ;
                        ////////////////////        break;
                        ////////////////////}
                        objetoPv1.PV12 = tipoAtnReader.GetInt16(4).ToString() + "^" + tipoAtnReader.GetString(5);
                        objetoPv1.PV14 = tipoAtnReader.GetInt16(0) + "^" + tipoAtnReader.GetString(1); //Tipo de Atencion
                        objetoPv1.PV13 = tipoAtnReader.GetInt16(2).ToString() + "^" + tipoAtnReader.GetString(3); // Ubicacion del Paciente Codigo-Descripcion
                        objetoPv1.PV110 = "415^Laboratorio Clinico Central"; // Se envia codigo de Ubicacion y NO el IdUbicacion
                    }

                    string strConsHabCama = "SELECT CodHabitacion,CodCama,C.IdMedico,D.Nompersonal,D.ApePersonal FROM admCama A,admhabitacion B, admAtencion C,hcePersonal D WHERE A.IdHabitacion=B.IdHabitacion AND A.IdCama=C.IdCamaActual AND A.IdAtencion=" + atencion + " AND A.IdAtencion=C.IdAtencion AND C.IdMedico=D.IdPersonal";
                    SqlCommand cmdConsHabCama = new SqlCommand(strConsHabCama, Conex);
                    SqlDataReader ConsHabCamaReader = cmdConsHabCama.ExecuteReader();
                    if (ConsHabCamaReader.HasRows)
                    {
                        ConsHabCamaReader.Read();
                        objetoPv1.PV111 = ConsHabCamaReader.GetString(0) + "^" + ConsHabCamaReader.GetString(1);
                        objetoPv1.PV17 = ConsHabCamaReader.GetSqlInt16(2).ToString() + "^" + ConsHabCamaReader.GetString(3) + " " + ConsHabCamaReader.GetString(4);
                    }
                    // Campos Nuevos Requeridos

                    string strConsTipoSol = "SELECT PRIO_SOLIC FROM TAT_ENC_SOLSAHI WHERE NRO_SOLIC=" + solicitud + " AND NRO_ORDEN=" + orden + " AND NRO_ATEN=" + atencion;
                    SqlCommand cmdConsTipoSol = new SqlCommand(strConsTipoSol, Conex);
                    SqlDataReader ConsTipoSolReader = cmdConsTipoSol.ExecuteReader();
                    if (ConsTipoSolReader.HasRows)
                    {
                        ConsTipoSolReader.Read();
                        objetoPv1.PV15 = ConsTipoSolReader.GetString(0);
                    }

                }
            }
            catch (SqlException sqlExp1)
            {
                utilLocal.notificaFalla("SqlException Generando HL7:(Solicitud:" + solicitud + " Orden:" + orden + " Atn:" + atencion + "):" + sqlExp1.Message);
                logLabcore.Warn("SqlException Generando HL7:" + sqlExp1.Message + " " + sqlExp1.StackTrace);
                return "SqlException En ordenarEstudio" + sqlExp1.StackTrace;
            }
            catch (Exception exp1)
            {
                utilLocal.notificaFalla("Falla Generando HL7:(Solicitud:" + solicitud + " Orden:" + orden + " Atn:" + atencion + "):" + exp1.Message);
                logLabcore.Warn("Exception Generando HL7 En ordenarAsync():" + exp1.StackTrace);
                return "Exception Generando HL7 ordenarAsync():" + exp1.StackTrace;
            }

			#endregion
			#region RegionIN1
			//   SELECT numCon,desContrato,A.idTercero NumDocumento FROM HUSI_Contratos A,genTercero B WHERE a.idTercero=B.idTercero AND A.idAtencion=4368250

			Conex.ConnectionString = Properties.Settings.Default.LabcoreDBConXX;
			using (Conex)
			{
				Conex.Open();
				//PV1
				string strConsContrato = "SELECT numCon,desContrato,NumDocumento FROM HUSI_Contratos A,genTercero B WHERE a.idTercero=B.idTercero AND A.IdAtencion=" + atencion;
				SqlCommand cmdConsContrato = new SqlCommand(strConsContrato, Conex);
				SqlDataReader contratoReader = cmdConsContrato.ExecuteReader();
				if (contratoReader.HasRows)
				{
					contratoReader.Read();
					//objetoIn1.IN12 = contratoReader.GetString(0);
					objetoIn1.IN13 = contratoReader.GetString(2);
					objetoIn1.IN14 =contratoReader.GetString(0)+"^"+contratoReader.GetString(1);

				}

			}

			#endregion
			#region RegionORC
            try
            {
                Conex.ConnectionString = Properties.Settings.Default.LabcoreDBConXX;
                using (Conex)
                {
                    Conex.Open();
                    //IN1
                    string strDatosSolicitud = "SELECT FECHA_ORD,FECHA_SOL, USR_SOLIC,A.NRO_ORDEN FROM TAT_ENC_ORDSAHI A,TAT_ENC_SOLSAHI B WHERE A.NRO_ORDEN=B.NRO_ORDEN AND A.NRO_ATEN=B.NRO_ATEN AND A.NRO_ATEN=" + atencion + "AND A.NRO_ORDEN=" + orden;
                    SqlCommand cmdDatosSolicitud = new SqlCommand(strDatosSolicitud, Conex);
                    SqlDataReader DatosSolicitudReader = cmdDatosSolicitud.ExecuteReader();
                    if (DatosSolicitudReader.HasRows)
                    {
                        DatosSolicitudReader.Read();
                        objetoOrc.ORC2 = "NW";
                        objetoOrc.ORC3 = orden;
                        objetoOrc.ORC4 = solicitud;
                        objetoOrc.ORC5 = atencion;
                        objetoOrc.ORC6 = "NW";
                        objetoOrc.ORC10 = utilLocal.fechaHL7(DatosSolicitudReader.GetDateTime(1));
                        objetoOrc.ORC14 = "HUSI";
                        objetoOrc.ORC12 = DatosSolicitudReader.GetInt32(2).ToString();
                        objetoOrc.ORC16 = utilLocal.fechaHL7(DatosSolicitudReader.GetDateTime(0));
                    }
                }
            }
            catch(SqlException sqlExp1)
            {
                utilLocal.notificaFalla("SqlException Generando HL7:(Solicitud:" + solicitud + " Orden:" + orden + " Atn:" + atencion + "):" + sqlExp1.Message);
                logLabcore.Warn("SqlException Generando HL7:" + sqlExp1.Message+ " " + sqlExp1.StackTrace);
                return "SqlException En ordenarEstudio" + sqlExp1.StackTrace;
            }
            catch(Exception exp1)
            {
                utilLocal.notificaFalla("Falla Generando HL7:(Solicitud:" + solicitud + " Orden:" + orden + " Atn:" + atencion + "):" + exp1.Message);
                logLabcore.Warn("Exception Generando HL7 En ordenarAsync():" + exp1.StackTrace);
                return "Exception Generando HL7 ordenarAsync():" + exp1.StackTrace;
            }
			#endregion
			string resultado = objetoMSH.retornoMSH() + "\n" + objetoPID.retornoPid() + "\n" + objetoPv1.retornoPV1() + "\n" + objetoIn1.retornoIN1() + "\n" + objetoOrc.retornoORC() + "\n";
			#region RegionOBR
            try
            {
                Conex.ConnectionString = Properties.Settings.Default.LabcoreDBConXX;
                using (Conex)  // Segmento OBR y Actualizacion de Trazabilidad
                {
                    // OBR
                    Trazabilidad updateTraza = new Trazabilidad();
                    Conex.Open();
                    string strConsultar = "SELECT NRO_SOLIC,NRO_ORDEN,NRO_ATEN,ID_PROD,COD_CUPS,CANT_SOLI,OBS_SOLI,CodProducto,NomProducto FROM TAT_DET_SOLSAHI,proProducto WHERE COD_CUPS=CodProducto AND NRO_SOLIC=" + solicitud + " AND NRO_ORDEN=" + orden + " AND NRO_ATEN=" + atencion;
                    SqlCommand cmdConsulta = new SqlCommand(strConsultar, Conex);
                    SqlDataReader conCursor = cmdConsulta.ExecuteReader();
                    if (conCursor.HasRows)
                    {
                        int i = 1;
                        while (conCursor.Read())
                        {
                            objetoObr.OBR2 = i.ToString();
                            objetoObr.OBR3 = conCursor.GetInt32(0).ToString();// nro solicitud
                            objetoObr.OBR4 = conCursor.GetString(4) + "^" + conCursor.GetString(8); // Codigo Cups + Nombre Producto
                            objetoObr.OBR13 = conCursor.GetString(6); // observaciones
                            objetoObr.OBR27 = conCursor.GetInt16(5).ToString(); //Cantidad
                            resultado = resultado + objetoObr.retornoOBR() + "\n";
                            updateTraza.insertarTraza(atencion, orden, solicitud, conCursor.GetString(4), "SOL_ENF", DateTime.Now, 0);
                            i++;
                        }
                    }
                }
            }
            catch (SqlException sqlExp1)
            {
                utilLocal.notificaFalla("SqlException Generando HL7:(Solicitud:" + solicitud + " Orden:" + orden + " Atn:" + atencion + "):" + sqlExp1.Message);
                logLabcore.Warn("SqlException Generando HL7:" + sqlExp1.Message + " " + sqlExp1.StackTrace);
                return "SqlException En ordenarEstudio" + sqlExp1.StackTrace;
            }
            catch (Exception exp1)
            {
                utilLocal.notificaFalla("Falla Generando HL7:(Solicitud:" + solicitud + " Orden:" + orden + " Atn:" + atencion + "):" + exp1.Message);
                logLabcore.Warn("Exception Generando HL7 En ordenarAsync():" + exp1.StackTrace);
                return "Exception Generando HL7 ordenarAsync():" + exp1.StackTrace;
            }
			#endregion
			string rpta=string.Empty;
			try
			{
				Conex.ConnectionString = Properties.Settings.Default.LabcoreDBConXX;
				using (Conex)
				{
					Conex.Open();
					string strDatosAten = "SELECT IndActivado,IdAtencionTipo FROM admAtencion WHERE IdAtencion=" + atencion + " AND (IndActivado=0 OR (IdAtencionTipo=8 OR IdAtencionTipo=27))";
					SqlCommand cmdDatosAten = new SqlCommand(strDatosAten, Conex);
					SqlDataReader readerDatosAten = cmdDatosAten.ExecuteReader();
					if (readerDatosAten.HasRows)
					{
						readerDatosAten.Read();
						if (readerDatosAten.GetInt16(1) == 8 || readerDatosAten.GetInt16(1) == 27)
						{
							rpta = "Atencion Ambulatorios/No se Envia";
						}
						else if (!readerDatosAten.GetBoolean(0))
						{
							rpta = "Atencion Cerrada/No se Envia";
						}
					}
					else // Se envia a Labcore y Se escribe a LOG
					{
                        logLabcore.Info("Enviando Solicitud MSG HL7:" + resultado);
						srLabcoreTAT.WSSolicitudesClient cliente = new srLabcoreTAT.WSSolicitudesClient();
						rpta = cliente.GetHL7Msg(resultado);
						logLabcore.Info("Respuesta Labcore Para Solicitud:"+solicitud+":::Respuesta Labcore:::" + rpta);
					}
				}
			}
			catch (EndpointNotFoundException endPointExp)
			{
                utilLocal.notificaFalla("EndpointNotFound Solicitando:(Solicitud:" + solicitud + " Orden:" + orden + " Atn:" + atencion + "):" + endPointExp.Message);
				logLabcore.Warn("EndpointNotFoundException En ordenarEstudio" + endPointExp.StackTrace);
				return "EndpointNotFoundException En ordenarEstudio" + endPointExp.StackTrace;
			}
			catch (ServerTooBusyException serverExp)
			{
                utilLocal.notificaFalla("ServerTooBusy Solicitando:(Solicitud:" + solicitud + " Orden:" + orden + " Atn:" + atencion + "):" + serverExp.Message);
				logLabcore.Warn("ServerTooBusyException En ordenarEstudio" + serverExp.StackTrace);
				return "ServerTooBusyException En ordenarEstudio" + serverExp.StackTrace;
			}
			catch(ChannelTerminatedException channelExp)
			{
                utilLocal.notificaFalla("ChannelTerminated Solicitando:(Solicitud:" + solicitud + " Orden:" + orden + " Atn:" + atencion + "):" + channelExp.Message);
				logLabcore.Warn("ChannelTerminatedException En ordenarEstudio" + channelExp.StackTrace);
				return "ChannelTerminatedException En ordenarEstudio" + channelExp.StackTrace;
			}
			catch(CommunicationException commExp)
			{
                utilLocal.notificaFalla("Communication Solicitando:(Solicitud:" + solicitud + " Orden:" + orden + " Atn:" + atencion + ")" + commExp.Message);
				logLabcore.Warn("CommunicationException En ordenarEstudio" + commExp.StackTrace);
				return "CommunicationException En ordenarEstudio" + commExp.StackTrace;
			}
			catch(TimeoutException toExp)
			{
                utilLocal.notificaFalla("TimeOut Solicitando:(Solicitud:" + solicitud + " Orden:" + orden + " Atn:" + atencion + "):" + toExp.Message);
				logLabcore.Warn("Exception Time Out En ordenarEstudio" + toExp.StackTrace);
				return "Exception Time Out En ordenarEstudio" + toExp.StackTrace;
			}
			catch(Exception exp)
			{
                utilLocal.notificaFalla("Excepcion Solicitando:(Solicitud:" + solicitud + " Orden:" + orden + " Atn:" + atencion + "):" + exp.Message);
				logLabcore.Warn(exp, "Excepcion Enviando Solicitud En ordenarEstudio" + exp.Message + "    " + exp.StackTrace);
				return "Excepcion Enviando Solicitud En ordenarEstudio" + exp.Message + " * " + exp.StackTrace;
			}

			Conex.ConnectionString = Properties.Settings.Default.LabcoreDBConXX;
			using (Conex)
			{
				Conex.Open();
				string strActualizaSol = "UPDATE TAT_ENC_SOLSAHI SET NRO_MSG=" + NroMsg + ", MSG_HL7='" + resultado + "', MSG_RESP='" + rpta + "' WHERE NRO_SOLIC=" + solicitud + " AND NRO_ORDEN=" + orden + "AND NRO_ATEN="+atencion;
				SqlCommand cmdActualizaSol = new SqlCommand(strActualizaSol, Conex);
				if (cmdActualizaSol.ExecuteNonQuery()> 0)
				{
					logLabcore.Info("Solicitud:"+solicitud+"Actualizada en la Tabla TAT_ENC_SOLSAHI");
				}
			}
			return rpta;

		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]

        public string cambioEstado(string msgHL7)
		{
//            msgHL7 = @"MSH|^~\&|LABCORE||SAHI||20160608163232||ORM^O01|LAB0000436834|P|2.3|||AL||||
//PID|2^10475604||| VILLAQUIRAN^DIDIER ELI||19410129000000|M|||||3142330345|||||||||||||||||
//PV1|I|OBS^70||||5416|||||||||||||||||||||||||||||||||||||||||||||
//IN1||830003564|||||||||||||||||
//ORC|SC|000956|275567|5096816|IP||||||||||||||
//OBR|1|5096816|908312^AMINOACIDOS POR NITROPRUSIATO|||||||||||||||||||LAB||F||||||||||||||||||";
            logLabcore.Info("MENSAJE RECIBIDO EN CAMBIO DE ESTADO:" + msgHL7);
			MensajeHL7 mensaje = new MensajeHL7(msgHL7);
			string respuestaERROR = string.Empty;
			string respuesta = string.Empty;
			string[] tipoMsg = mensaje.objMSH.Msh9.Split('^');
			if (tipoMsg[0].Equals("ORM") && mensaje.objORC.ORC6.Equals("SC"))
			{
                logLabcore.Info("Cambio de Estado SC----MSH:" + tipoMsg[0] + "  " + "ORC-6:" + mensaje.objORC.ORC6);
				respuesta = tomaMuestra(mensaje);
			}
			else if (tipoMsg[0].Equals("ORM") && mensaje.objORC.ORC6.Equals("IP"))
			{
				//respuesta = recibirVentaLab(mensaje);
                logLabcore.Info("Cambio de Estado IP----MSH:" + tipoMsg[0] + "  " + "ORC-6:" + mensaje.objORC.ORC6);
				SqlConnection Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX);
				using (Conex)
				{
					try
					{
						respuesta = recibirVentaLab(mensaje);
						//ventaLaboratorio(mensaje);                          //**************************************** ventaLaboratorio
						Conex.Open();
						string strConsultarVta = "SELECT XPROC_ATEN,XPROC_SOLIC,XPROC_ORDEN,XPROC_CONSM,XPROC_MSGVT FROM TAT_VENT_XPROC";
						SqlCommand cmdConsultaVta = new SqlCommand(strConsultarVta, Conex);
						SqlDataReader conCursorVta = cmdConsultaVta.ExecuteReader();
						while (conCursorVta.Read())
						{
							MensajeHL7 msgAsync = DeserializadorHL7(conCursorVta.GetString(4));
							ventaLaboratorio(msgAsync);                          //**************************************** ventaLaboratorio
						}
					}
					catch(Exception expGen1)
					{
						utilLocal.notificaFalla(" Error Procesando IP:"+expGen1.Message+"***Solicitud:"+mensaje.objORC.ORC4);
						logLabcore.Warn("Error" + expGen1.Message + "***Solicitud:" + mensaje.objORC.ORC4 + "       " + expGen1.StackTrace);
					}
				}
			}
			else if(tipoMsg[0].Equals("ORU")&&mensaje.objORC.ORC6.Equals("CM"))
			{
                logLabcore.Info("Cambio de Estado CM----MSH:" + tipoMsg[0] + "  " + "ORC-6:" + mensaje.objORC.ORC6);
				respuesta = resultadosLaboratorio(mensaje);
			}
			else if(tipoMsg[0].Equals("ORM")&& mensaje.objORC.ORC6.Equals("CA"))
			{
				//logLabcore.Info("Cambio de Estado CA----MSH:" + tipoMsg[0] + "  " + "ORC-6:" + mensaje.objORC.ORC6);
				if (mensaje.objORC.ORC17.Length>0)
				{
					using(SqlConnection conexion=new SqlConnection(Properties.Settings.Default.DBConexionXX))
					{
						conexion.Open();
						OBRClass objOBR = new OBRClass(mensaje.segmentosOBR[0]);
						string[] cups = objOBR.OBR4.Split('^');
						string atencion = mensaje.objORC.ORC5;
						string solicitud = mensaje.objORC.ORC4;
						string strConsulta = "SELECT XPROC_IDMOV FROM TAT_VENT_APLI WHERE XPROC_ATEN=" + atencion + " AND XPROC_SOLIC=" + solicitud + " AND XPROC_CODPRO='"+cups[0]+"'";
						SqlCommand cmdConsulta = new SqlCommand(strConsulta, conexion);
						SqlDataReader readConsulta = cmdConsulta.ExecuteReader();
						if (readConsulta.HasRows)
						{
							readConsulta.Read();
							if(readConsulta.GetInt32(0)>0)
							{
								respuesta = cancelarVentaLabAsync(mensaje,readConsulta.GetInt32(0).ToString());
							}
						}
						else
						{
							respuesta = cancelarLaboratorio(mensaje);
						}

					}
				}
				else
				{
					respuesta=cancelarLaboratorio(mensaje);
				}
			}
			else if (tipoMsg[0].Equals("ORM") && mensaje.objORC.ORC6.Equals("PE"))
			{
				//logLabcore.Info("Cambio de Estado PE ----MSH:" + tipoMsg[0] + "  " + "ORC-6:" + mensaje.objORC.ORC6);
				respuesta = "El evento MSH y/o ORC no son validos, como Cambio de Estado, Venta o Resultados:" + tipoMsg[0] + "^" + mensaje.objORC.ORC6;
			}
			else
			{
                tipoMsg[0] = "NA";
                mensaje.objORC.ORC6 = "NA";
				logLabcore.Warn("El evento en MSH y/o en ORC no son validos, como Cambio de Estado, Venta o Resultados:" + tipoMsg[0] + "^" + mensaje.objORC.ORC6 );
				respuesta = "El evento MSH y/o ORC no son validos, como Cambio de Estado, Venta o Resultados:" + tipoMsg[0] + "^" + mensaje.objORC.ORC6;
			}
            try
            {
                Conex2.Open();
                string qryBacklog = "INSERT INTO TAT_MSGS_TAT(TAT_ESTADO_MSG,TAT_TIPO_MSG,TAT_CONT_MSG,TAT_RPTA_MSG,TAT_FECHA_MSG) VALUES(@estado,@tipo,@hl7,@rpta,@fecha)";
                SqlCommand cmdBackLog = new SqlCommand(qryBacklog, Conex2);
                cmdBackLog.Parameters.Add("@estado", SqlDbType.VarChar);
                cmdBackLog.Parameters.Add("@tipo", SqlDbType.VarChar);
                cmdBackLog.Parameters.Add("@hl7", SqlDbType.Text);
                cmdBackLog.Parameters.Add("@rpta", SqlDbType.Text);
                cmdBackLog.Parameters.Add("@fecha", SqlDbType.DateTime);
                cmdBackLog.Parameters["@estado"].Value = tipoMsg[0];
                cmdBackLog.Parameters["@tipo"].Value = mensaje.objORC.ORC6;
                cmdBackLog.Parameters["@hl7"].Value = msgHL7;
                cmdBackLog.Parameters["@rpta"].Value = respuesta;
                cmdBackLog.Parameters["@fecha"].Value = DateTime.Now;
                cmdBackLog.ExecuteNonQuery();
            }
            catch(SqlException sqlExp)
            {
                logLabcore.Info("Se procesento un problema guardando LOG de mensajes en: TAT_MSGS_TAT Mensaje:"+sqlExp.Message+" Numero de Error:"+sqlExp.Number+"StackTrace:"+sqlExp.StackTrace);
            }
			return respuesta;
		}

		string ackRecibidos(MSHClass objMSHRecibido,bool tipo)
		{
			MSHClass objMSH = new MSHClass();
			objMSH.Msh7 = utilLocal.fechaHL7(DateTime.Now);
			objMSH.Msh9 = "ORR^O02";
			objMSH.Msh10 = consecutivoMSG();
			objMSH.Msh11 = "P";
			objMSH.Msh12 = "2.3";
			MSAClass objMSA = new MSAClass();
			if(tipo)
			{
				objMSA.MSA2 = "AA";
				objMSA.MSA3 = "000";
			}
			else
			{
				objMSA.MSA2 = "AR";
				objMSA.MSA3 = "004";
			}
			objMSA.MSA4 = objMSHRecibido.Msh10;
			logLabcore.Info("mensaje de Recibido Enviado: " + objMSH.retornoMSH() + "\n" + objMSA.retornoMSA());
			return objMSH.retornoMSH() + "\n" + objMSA.retornoMSA();
		}

		string ackResultados(MSHClass objMSHRecibido,bool estado)
		{
			MSHClass objMSH = new MSHClass();
			objMSH.Msh7 = utilLocal.fechaHL7(DateTime.Now);
			objMSH.Msh9 = "ORR^O02";
			objMSH.Msh10 = consecutivoMSG();
			objMSH.Msh11 = "P";
			objMSH.Msh12 = "2.3";
			MSAClass objMSA = new MSAClass();
			if (estado)
			{
				objMSA.MSA2 = "AA";
				objMSA.MSA3 = "000";
			}
			else
			{
				objMSA.MSA2 = "AE";
				objMSA.MSA3 = "104";
			}
			objMSA.MSA4 = objMSHRecibido.Msh10;
			return objMSH.retornoMSH() + "\n" + objMSA.retornoMSA();

		}

		string ackVenta(MSHClass objMSHRecibido,string nroSolicitud,Int64 idMovimiento)
		{
			MSHClass objMSH = new MSHClass();
			objMSH.Msh7 = utilLocal.fechaHL7(DateTime.Now);
			objMSH.Msh9 = "ORR^O02";
			objMSH.Msh10 = consecutivoMSG();
			objMSH.Msh11 = "P";
			objMSH.Msh12 = "2.3";
			MSAClass objMSA = new MSAClass();

			objMSA.MSA4 = objMSHRecibido.Msh10;
			if (idMovimiento > 0)
			{
				objMSA.MSA2 = "AA";
				objMSA.MSA3 = idMovimiento.ToString();
			}
			else
			{
				objMSA.MSA2 = "AR";
				objMSA.MSA3 = "000";
			}
			return objMSH.retornoMSH() + "\n" + objMSA.retornoMSA();
		}

		string consecutivoMSG()
		{
			SqlConnection Conex = new SqlConnection(Properties.Settings.Default.DBConexionXX);
			Conex.Open();
			SqlTransaction TX3 = Conex.BeginTransaction("TRx3");
			string strConsNroMsg = "SELECT NRO_MSG FROM TAT_CONS_MSGS WHERE TIPO_MSG='SLAB'";
			SqlCommand cmdConsNroMsg = new SqlCommand(strConsNroMsg, Conex, TX3);
			SqlDataReader nroMsgReader = cmdConsNroMsg.ExecuteReader();
			nroMsgReader.Read();
			Int32 nroMsg = nroMsgReader.GetInt32(0);
			nroMsg++;
			nroMsgReader.Close();
			nroMsgReader.Dispose();
			string strActualizaConsecut = "UPDATE TAT_CONS_MSGS SET NRO_MSG=" + nroMsg + " WHERE TIPO_MSG='SLAB'";
			SqlCommand cmdActualizaConsecut = new SqlCommand(strActualizaConsecut, Conex, TX3);
			if (cmdActualizaConsecut.ExecuteNonQuery() > 0)
			{
				TX3.Commit();
				TX3.Dispose();
				return nroMsg.ToString();
			}
			else
			{
				TX3.Rollback("TRx3");
				TX3.Dispose();
				return "";
			}

		}

		void ventaLaboratorio(MensajeHL7 msg)
		{
            logLabcore.Info("Mensaje Para hacer ventaLaboratorio.  "+msg.ToString() );
			SqlConnection Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX);
			string nroAtencion = string.Empty;
			string nroSolicitud = string.Empty;
			string nroOrden = string.Empty;
			string codUsuario = "";
			Int64 idMovimientoVta;
			using (Conex)
			{
				Conex.Open();
				string strConsultarAtn = "SELECT NRO_ATEN,NRO_SOLIC,NRO_ORDEN FROM TAT_ENC_SOLSAHI WHERE NRO_SOLIC=" + msg.objORC.ORC4;
				SqlCommand cmdConsultaAtn = new SqlCommand(strConsultarAtn, Conex);
				SqlDataReader conCursor = cmdConsultaAtn.ExecuteReader();
				conCursor.Read();
				nroAtencion = conCursor.GetInt32(0).ToString();
				nroSolicitud = conCursor.GetInt32(1).ToString();
				nroOrden = conCursor.GetInt32(2).ToString();
				if(msg.objPV1.PV17.Length==0){codUsuario="0";}else{codUsuario=msg.objPV1.PV17;};

			}
			// **********************************************************
			srLabcoreTAT.WSSolicitudesClient cliente = new srLabcoreTAT.WSSolicitudesClient();
			foreach (string[] segmentoOBR in msg.segmentosOBR)
			{
                //------------------------------ VALIDAR vENTA

                OBRClass objOBR = new OBRClass(segmentoOBR);
                string[] producto = objOBR.OBR4.Split('^');
                string Cups = producto[0];
                Int64 idMovtoValidado =Int64.Parse(validarVenta(nroAtencion, nroSolicitud, Cups).ToString());
                if (idMovtoValidado == 0)
                    //-----------------------------------------------
                {
                    idMovimientoVta = ventaProcedimiento(Cups, nroAtencion, nroSolicitud, msg.objMSH, nroOrden, codUsuario);   // Realiza la venta ********************************
                    Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX);
                    using (Conex)
                    {
                        Conex.Open();
                        string qryProducto = "SELECT IdProducto FROM proProducto WHERE CodProducto='" + Cups + "'";
                        SqlCommand cmdProducto = new SqlCommand(qryProducto, Conex);
                        SqlDataReader readerProducto = cmdProducto.ExecuteReader();
                        readerProducto.Read();
                        Int32 idProducto = readerProducto.GetInt32(0);
                        string textoMensaje = ackVenta(msg.objMSH, nroSolicitud, idMovimientoVta);
                        string rpta = cliente.GetHL7Msg(textoMensaje);
                        if (idMovimientoVta > 0)
                        {
                            logLabcore.Info("Mensaje de Venta de Vendido SAHI-->Labcore Enviado:" + textoMensaje);
                            logLabcore.Info("Respuesta la Venta de Labcore:" + rpta);

                            string qryActualizaVtas = "DELETE FROM TAT_VENT_XPROC WHERE XPROC_SOLIC=" + nroSolicitud + " AND COD_CUPS='" + Cups + "'";
                            logLabcore.Info("Limpia TAT_VENT_XPROC:" + qryActualizaVtas);
                            SqlCommand cmdActualizaVtas = new SqlCommand(qryActualizaVtas, Conex);
                            int nroReg1 = cmdActualizaVtas.ExecuteNonQuery();
                        }
                        else
                        {
                            string texto_NoVta = ackVenta(msg.objMSH, nroSolicitud, 0);
                            rpta = cliente.GetHL7Msg(texto_NoVta);
                            utilLocal.notificaFalla("Mensaje Venta de NO Vendido Enviado:" + nroSolicitud);
                            logLabcore.Info("Mensaje Venta de NO Vendido Enviado:" + texto_NoVta);
                            logLabcore.Info("Rpta Labcore la NO Venta de Labcore:" + rpta);
                        }
                    }
                }
                else
                {
                    string textoMensaje = ackVenta(msg.objMSH, nroSolicitud, idMovtoValidado);
                    string rpta = cliente.GetHL7Msg(textoMensaje);
                    logLabcore.Info("Intento Venta Duplicada del Movimiento: " + idMovtoValidado+", Se retorno el nro de Movimiento");
                    logLabcore.Info("Respuesta de Labcore al mensaje de venta::: "+rpta);
                    Conex.ConnectionString = Properties.Settings.Default.DBConexion;
                    using(Conex)
                    {
                        Conex.Open();
                        string qryActualizaVtas = "DELETE FROM TAT_VENT_XPROC WHERE XPROC_SOLIC=" + nroSolicitud + " AND COD_CUPS='" + Cups + "'";
                        SqlCommand cmdActualizaVtas = new SqlCommand(qryActualizaVtas, Conex);
                        int nroReg1 = cmdActualizaVtas.ExecuteNonQuery();
                    }
                }

			}
			//ackVenta(msg.objMSH, nroSolicitud, retornoLabcore);
		}

		Int64 ventaProcedimiento(string cups, string atencion, string nroSolicitud,MSHClass objMSH,string orden,string usuario)   //******* ventaProcedimientoAsync *************************
		{
			using (SqlConnection conexion = new SqlConnection(Properties.Settings.Default.DBConexionXX))
			{
				int idContrato = 0;
				int idPlan = 0;
				int idTarifa = 0;
				int idProducto = 0;
				//float precioCondTarifa = 0;
				float valorVenta = 0; //Double
				Int16 productoTipo = 0;
				Int16 productoGrupo = 0;
				//int tarifaRela = 0;
				//float valRelacion = 0;
				string ubicacionConsumo = string.Empty;   //*************** Validar con proProductoUbica
                short camaActual = 0;
                short ubicacionActualPac = 0;


				conexion.Open();

                    string qryDatosAtn = "SELECT idCamaActual,idUbicacionActual FROM admAtencion WHERE idAtencion=" + atencion;
                    SqlCommand cmdDatosAtn = new SqlCommand(qryDatosAtn, conexion);
                    SqlDataReader readerDatosAtn = cmdDatosAtn.ExecuteReader();
                    if (readerDatosAtn.HasRows)
                    {
                        readerDatosAtn.Read();//************
                        ubicacionActualPac = readerDatosAtn.GetInt16(1);
                        if (readerDatosAtn.IsDBNull(0))
                        {
                            camaActual = 0;
                        }
                        else
                        {
                            camaActual = readerDatosAtn.GetInt16(0);
                        }
                    }
                    else
                    {
                        camaActual = 0;
                    }
                    readerDatosAtn.Close();
                    readerDatosAtn.Dispose();
                    string qryDatosAtnCon = "SELECT IdContrato,IdPlan,idTarifa,OrdPrioridad FROM admAtencionContrato WHERE IdAtencion=" + atencion + " AND IndHabilitado=1 AND ordprioridad=1";
                    SqlCommand sqlDtaAtnCon = new SqlCommand(qryDatosAtnCon, conexion);
                    SqlDataReader readerDtaAtnCon = sqlDtaAtnCon.ExecuteReader();
                    if (readerDtaAtnCon.HasRows)
                    {
                        readerDtaAtnCon.Read();
                        idContrato = readerDtaAtnCon.GetInt32(0);
                        idPlan = readerDtaAtnCon.GetInt32(1);
                        idTarifa = readerDtaAtnCon.GetInt32(2);
                        readerDtaAtnCon.Close();
                        readerDtaAtnCon.Dispose();
                        string qryDatosProd = "SELECT IdProducto,IdProductoTipo,IdGrupo FROM proProducto where CodProducto='" + cups + "'";
                        SqlCommand cmdDatosProd = new SqlCommand(qryDatosProd, conexion);
                        SqlDataReader readerDatosProd = cmdDatosProd.ExecuteReader();
                        if (readerDatosProd.HasRows)
                        {
                            readerDatosProd.Read();
                            idProducto = readerDatosProd.GetInt32(0);
                            productoTipo = readerDatosProd.GetInt16(1);
                            productoGrupo = readerDatosProd.GetInt16(2);
                            readerDatosProd.Close();
                            readerDatosProd.Dispose();
                            //**************************************************  Centro de Costo.
                            string qryUbicacionCenCos = "SELECT idUbicacion FROM proProductoUbica WHERE idProducto=" + idProducto + " AND indHabilitado=1";
                            SqlCommand ubicacionCenCos = new SqlCommand(qryUbicacionCenCos, conexion);
                            SqlDataReader readerubicacionCenCos = ubicacionCenCos.ExecuteReader();
                            if(readerubicacionCenCos.HasRows)
                            {
                                readerubicacionCenCos.Read();
                                ubicacionConsumo = readerubicacionCenCos.GetInt16(0).ToString();
                                readerubicacionCenCos.Close();
                                readerubicacionCenCos.Dispose();
                            }
                            else
                            {
                                logLabcore.Warn("No hay datos de la Ubicacion del Producto Para Centro de Costos idProducto:" + idProducto);
                                return 0;
                            }
                            //readerubicacionCenCos.Read();
                            //ubicacionConsumo = readerubicacionCenCos.GetInt16(0).ToString();
                            //readerubicacionCenCos.Close();
                            //readerubicacionCenCos.Dispose();
                            valorVenta = venderProducto(atencion, cups);
                            if (valorVenta > 0) //*************************
                            {   /////  segmento para await
                                using (SqlConnection ConexTran = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX))
                                {
                                    ConexTran.Open();
                                    // obtener consecutivo
                                    Int64 consecutivo = NumeroVenta();
                                    string[] datosProducto = utilLocal.productoDatos(idProducto.ToString());
                                    Byte indNoPos = 0;
                                    if (datosProducto[4].Equals("True"))
                                    {
                                        indNoPos = 1;
                                    }
                                    Trazabilidad updateTraza = new Trazabilidad();
                                    SqlTransaction txVenta = ConexTran.BeginTransaction("TxVenta");
                                    string qryInsertarVenta = "INSERT INTO facMovimiento (IdMovimiento,NumMovimiento,CodEsor,CodEnti,IdModulo,IdDestino,IdTransaccion,IdProductoTipo,IdResponsable,IdUbicacionConsumo,IdUbicacionEntrega,IdMovimientoBase,IdProcPrincipal,IdCausal,DocRespaldo,IdTarifa,IdServicio,FecMovimiento,IndFacturado,IndDevuelto,IndCompleto,IndHabilitado,IdUsuarioR,FecRegistro,IndPaquete,Cama,SecTran,SecTranB,IdMovimientoPral,IdDestinoIni,TipoVenta)";
                                    qryInsertarVenta = qryInsertarVenta + " VALUES(" + consecutivo + "," + consecutivo + ",1,1,5," + atencion + ",39," + productoTipo + ",949," + ubicacionConsumo + ",18," + consecutivo + ",2513,NULL,'" + nroSolicitud + "'," + idTarifa + ",2,GETDATE(),0,0,0,1," + usuario + ",GETDATE(),0,'" + camaActual + "',12511963,NULL,NULL,NULL,\'TAT\')";
                                    SqlCommand cmdInsertarVenta = new SqlCommand(qryInsertarVenta, ConexTran, txVenta);
                                    using (ConexTran)
                                    {
                                        if (cmdInsertarVenta.ExecuteNonQuery() > 0)
                                        {
                                            string qryInsertarVentaDet = "INSERT INTO facMovimientoDet(IdMovimiento,IdProducto,NomProducto,CodProducto,Cantidad,CantidadFact,ValVenta,ValTarifaBase,NumAutorizacion,IndPorcentaje,IndRecargo,IdRecargo,IndFactTemp,IndEnlace,IndHabilitado,ValCuotaMod,indAtendido,RutaArchivo,idMedico,indPos,IndFacturado,IdFormato,regCUM,idSolicitud,idGrupoImp,indXml)";
                                            qryInsertarVentaDet = qryInsertarVentaDet + " VALUES(" + consecutivo + "," + idProducto + ",'" + datosProducto[0] + "','" + cups + "',1,0," + valorVenta + ",0,' ',0,0,0,0,0,1,0,NULL,NULL,NULL," + indNoPos + ",0,NULL,NULL,NULL,NULL,NULL)";
                                            SqlCommand cmdInsertarVentaDet = new SqlCommand(qryInsertarVentaDet, ConexTran, txVenta);
                                            if (cmdInsertarVentaDet.ExecuteNonQuery() > 0)
                                            {
                                                txVenta.Commit();
                                                using (SqlConnection Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBCon))
                                                {
                                                    Conex.Open();
                                                    string strInsEncabeza = "INSERT INTO TAT_TRAZA_ENC (ENC_ORDEN,ENC_SOLIC,ENC_ATEN,EVT_ORD,FECHA_EVT,ENC_NOTA) VALUES(@orden,@solicitud,@atencion,@evento,@fecha,@nota)";
                                                    SqlCommand cmdInsEncabeza = new SqlCommand(strInsEncabeza, Conex);
                                                    cmdInsEncabeza.Parameters.Add("@orden", SqlDbType.Int);
                                                    cmdInsEncabeza.Parameters.Add("@solicitud", SqlDbType.Int);
                                                    cmdInsEncabeza.Parameters.Add("@atencion", SqlDbType.Int);
                                                    cmdInsEncabeza.Parameters.Add("@evento", SqlDbType.VarChar);
                                                    cmdInsEncabeza.Parameters.Add("@fecha", SqlDbType.DateTime);
                                                    cmdInsEncabeza.Parameters.Add("@nota", SqlDbType.Int);


                                                    cmdInsEncabeza.Parameters["@orden"].Value = orden;
                                                    cmdInsEncabeza.Parameters["@solicitud"].Value = nroSolicitud;
                                                    cmdInsEncabeza.Parameters["@atencion"].Value = atencion;
                                                    cmdInsEncabeza.Parameters["@evento"].Value = "ORU^IP";
                                                    cmdInsEncabeza.Parameters["@fecha"].Value = DateTime.Now.ToString();
                                                    cmdInsEncabeza.Parameters["@nota"].Value = 0;


                                                    if (cmdInsEncabeza.ExecuteNonQuery() > 0)
                                                    {
                                                        string qryActualizaTraza = "INSERT INTO TAT_TRAZA_ORDEN (NRO_ORDEN,NRO_SOLIC,NRO_ATEN,ID_PROD,COD_CUPS,EVT_ORD,FECHA_EVT,ID_USUA) VALUES(@orden,@solicitud,@atencion,@idProducto,@cups,@evento,@fecha,@usua)";
                                                        SqlCommand cmdActualizaTraza = new SqlCommand(qryActualizaTraza, Conex);

                                                        cmdActualizaTraza.Parameters.Add("@orden", SqlDbType.Int);
                                                        cmdActualizaTraza.Parameters.Add("@solicitud", SqlDbType.Int);
                                                        cmdActualizaTraza.Parameters.Add("@atencion", SqlDbType.Int);
                                                        cmdActualizaTraza.Parameters.Add("@idProducto", SqlDbType.Int);
                                                        cmdActualizaTraza.Parameters.Add("@cups", SqlDbType.VarChar);
                                                        cmdActualizaTraza.Parameters.Add("@evento", SqlDbType.VarChar);
                                                        cmdActualizaTraza.Parameters.Add("@fecha", SqlDbType.DateTime);
                                                        cmdActualizaTraza.Parameters.Add("@usua", SqlDbType.SmallInt);

                                                        cmdActualizaTraza.Parameters["@orden"].Value = orden;
                                                        cmdActualizaTraza.Parameters["@solicitud"].Value = nroSolicitud;
                                                        cmdActualizaTraza.Parameters["@atencion"].Value = atencion;
                                                        cmdActualizaTraza.Parameters["@idProducto"].Value = idProducto;
                                                        cmdActualizaTraza.Parameters["@cups"].Value = cups;
                                                        cmdActualizaTraza.Parameters["@evento"].Value = "ORU^IP";
                                                        cmdActualizaTraza.Parameters["@fecha"].Value = DateTime.Now.ToString();
                                                        cmdActualizaTraza.Parameters["@usua"].Value = Int16.Parse(usuario);

                                                        cmdActualizaTraza.ExecuteNonQuery();
                                                        if (!updateTraza.insertarTraza(atencion, orden, nroSolicitud, cups, "ORU^IP", DateTime.Now, 0))
                                                        {
                                                            logLabcore.Info("No se actualizo la Trazabilidad:Atencion:" + atencion + " Orden:" + orden + " Solicitud:" + nroSolicitud + " Producto:" + cups);
                                                        }
                                                    }
                                                    string strGuardarVenta = "INSERT INTO TAT_VENT_APLI (XPROC_ATEN,XPROC_SOLIC,XPROC_CODPRO,XPROC_IDMOV,XPROC_VALOR,XPROC_FECHA) VALUES(" + atencion + "," + nroSolicitud + ",'" + cups + "'," + consecutivo + "," + valorVenta + ",@fechaVenta)";  ////************* VENTAS VENTAS
                                                    SqlCommand cmdGuardarVenta = new SqlCommand(strGuardarVenta, ConexTran);
                                                    cmdGuardarVenta.Parameters.Add("@fechaVenta", SqlDbType.DateTime);
                                                    cmdGuardarVenta.Parameters["@fechaVenta"].Value = DateTime.Now;
                                                    int nroReg2 = cmdGuardarVenta.ExecuteNonQuery();
                                                    if (nroReg2 > 0)
                                                    {
                                                        string qryActualizaVtas = "DELETE FROM TAT_VENT_XPROC WHERE XPROC_SOLIC=" + nroSolicitud + " AND COD_CUPS='" + cups + "'";
                                                        SqlCommand cmdActualizaVtas = new SqlCommand(qryActualizaVtas, ConexTran);
                                                        int nroReg1 = cmdActualizaVtas.ExecuteNonQuery();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                utilLocal.notificaFalla("No se Inserto el Movimiento de la Venta en la tabla facMovimientoDet, Solicitud:" + nroSolicitud);
                                                logLabcore.Warn("**** No se Inserto el Movimiento de la Venta en la tabla facMovimientoDet, Solicitud:" + nroSolicitud);
                                                txVenta.Rollback("TxVenta");
                                            }
                                        }
                                        else
                                        {
                                            utilLocal.notificaFalla("**** No se Inserto el Movimiento de la Venta en la tabla facMovimiento y facMovimientoDet, Solicitud:" + nroSolicitud);
                                            logLabcore.Warn("**** No se Inserto el Movimiento de la Venta en la tabla facMovimiento y facMovimientoDet, Solicitud:" + nroSolicitud);
                                            txVenta.Rollback("TxVenta");
                                        }
                                    }
                                    // fin de grabacion de la venta
                                    //return float.Parse(readerValorVenta.GetDecimal(0).ToString());
                                    return consecutivo;
                                }
                            }
                            else
                            {
                                logLabcore.Warn("Valor de la Venta fue Cero(0) Para la Solicitud:" + nroSolicitud);
                                utilLocal.notificaFalla("Venta Cero(0) Solicitud:" + nroSolicitud);
                                using (SqlConnection Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBCon))
                                {
                                    Conex.Open();
                                    string strInsertarVta = "INSERT INTO TAT_VENT_NOPROC VALUES(@atencion,@solicitud,@orden,@idProd,@Cups,@consecutivo,@HL7)";
                                    SqlCommand cmdInsertarVta = new SqlCommand(strInsertarVta, Conex);
                                    cmdInsertarVta.Parameters.Add("@atencion", System.Data.SqlDbType.VarChar);
                                    cmdInsertarVta.Parameters.Add("@solicitud", System.Data.SqlDbType.VarChar);
                                    cmdInsertarVta.Parameters.Add("@orden", System.Data.SqlDbType.VarChar);
                                    cmdInsertarVta.Parameters.Add("@idProd", System.Data.SqlDbType.Int);
                                    cmdInsertarVta.Parameters.Add("@Cups", System.Data.SqlDbType.VarChar);
                                    cmdInsertarVta.Parameters.Add("@consecutivo", System.Data.SqlDbType.VarChar);
                                    cmdInsertarVta.Parameters.Add("@HL7", System.Data.SqlDbType.VarChar);

                                    cmdInsertarVta.Parameters["@atencion"].Value = atencion;
                                    cmdInsertarVta.Parameters["@solicitud"].Value = nroSolicitud;
                                    cmdInsertarVta.Parameters["@orden"].Value = orden;
                                    cmdInsertarVta.Parameters["@idProd"].Value = idProducto;
                                    cmdInsertarVta.Parameters["@Cups"].Value = cups;
                                    cmdInsertarVta.Parameters["@consecutivo"].Value = nroSolicitud;
                                    cmdInsertarVta.Parameters["@HL7"].Value = "";
                                    if (cmdInsertarVta.ExecuteNonQuery() > 0)
                                    {
                                        string qryActualizaVtas = "DELETE FROM TAT_VENT_XPROC WHERE XPROC_SOLIC=" + nroSolicitud + " AND COD_CUPS='" + cups + "'";
                                        SqlCommand cmdActualizaVtas = new SqlCommand(qryActualizaVtas, Conex);
                                        int nroReg1 = cmdActualizaVtas.ExecuteNonQuery();
                                    }

                                }
                                return 0;
                            }
                            //**** else de la Ubicacion del producto

                        }
                        else
                        {
                            logLabcore.Warn("No hay datos del Producto Solicitud:" + nroSolicitud);
                            return 0;
                        }
                    }
                    else
                    {
                        logLabcore.Warn("No Hay Informacion en admAtencionContrato para la solicitud:" + nroSolicitud);
                        return 0;
                    }
			}
		}

		string tomaMuestra(MensajeHL7 msg)
		{
			try
			{
				SqlConnection Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX);
				string nroAtencion = string.Empty;
				string nroSolicitud = string.Empty;
				string nroOrden = string.Empty;
				string usuario=string.Empty;
				using (Conex)
				{
					Conex.Open();
					string strConsultarAtn = "SELECT NRO_ATEN,NRO_SOLIC,NRO_ORDEN,USR_SOLIC FROM TAT_ENC_SOLSAHI WHERE NRO_SOLIC=" + msg.objORC.ORC4;
					SqlCommand cmdConsultaAtn = new SqlCommand(strConsultarAtn, Conex);
					SqlDataReader conCursor = cmdConsultaAtn.ExecuteReader();
					if (conCursor.HasRows)
					{
						conCursor.Read();
						nroAtencion = conCursor.GetInt32(0).ToString();
						nroSolicitud = conCursor.GetInt32(1).ToString();
						nroOrden = conCursor.GetInt32(2).ToString();
						usuario=conCursor.GetInt32(3).ToString();
					}
					else
					{
						return ackRecibidos(msg.objMSH, false);
					}
				}
				Trazabilidad updateTraza = new Trazabilidad();
				foreach (string[] segmentoOBR in msg.segmentosOBR)
				{
					OBRClass objOBR = new OBRClass(segmentoOBR);
					string[] producto = objOBR.OBR4.Split('^');
					string Cups = producto[0];

					Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX);
					using (Conex)
					{
						Conex.Open();
						string qryProducto = "SELECT IdProducto FROM proProducto WHERE CodProducto='" + Cups + "'";
						SqlCommand cmdProducto = new SqlCommand(qryProducto, Conex);
						SqlDataReader readerProducto = cmdProducto.ExecuteReader();
						readerProducto.Read();
						Int32 idProducto = readerProducto.GetInt32(0);
						string strInsEncabeza = "INSERT INTO TAT_TRAZA_ENC (ENC_ORDEN,ENC_SOLIC,ENC_ATEN,EVT_ORD,FECHA_EVT,ENC_NOTA) VALUES(@orden,@solicitud,@atencion,@evento,@fecha,@nota)";
						SqlCommand cmdInsEncabeza = new SqlCommand(strInsEncabeza, Conex);
						cmdInsEncabeza.Parameters.Add("@orden", SqlDbType.Int);
						cmdInsEncabeza.Parameters.Add("@solicitud", SqlDbType.Int);
						cmdInsEncabeza.Parameters.Add("@atencion", SqlDbType.Int);
						cmdInsEncabeza.Parameters.Add("@evento", SqlDbType.VarChar);
						cmdInsEncabeza.Parameters.Add("@fecha", SqlDbType.DateTime);
						cmdInsEncabeza.Parameters.Add("@nota", SqlDbType.Int);
						cmdInsEncabeza.Parameters["@orden"].Value = nroOrden;
						cmdInsEncabeza.Parameters["@solicitud"].Value = nroSolicitud;
						cmdInsEncabeza.Parameters["@atencion"].Value = nroAtencion;
						cmdInsEncabeza.Parameters["@evento"].Value = "ORM^SC";
						cmdInsEncabeza.Parameters["@fecha"].Value = DateTime.Now.ToString();
						cmdInsEncabeza.Parameters["@nota"].Value = 0;
						if (cmdInsEncabeza.ExecuteNonQuery() > 0)
						{
							string qryInsertar = "INSERT INTO TAT_TRAZA_ORDEN (NRO_ORDEN,NRO_SOLIC,NRO_ATEN,ID_PROD,COD_CUPS,EVT_ORD,FECHA_EVT,ID_USUA) VALUES(@orden,@solicitud,@atencion,@idProducto,@cups,@evento,@fecha,@idUsua)";
							SqlCommand cmdInsertar = new SqlCommand(qryInsertar, Conex);
							cmdInsertar.Parameters.Add("@orden", SqlDbType.Int);
							cmdInsertar.Parameters.Add("@solicitud", SqlDbType.Int);
							cmdInsertar.Parameters.Add("@atencion", SqlDbType.Int);
							cmdInsertar.Parameters.Add("@idProducto", SqlDbType.Int);
							cmdInsertar.Parameters.Add("@cups", SqlDbType.VarChar);
							cmdInsertar.Parameters.Add("@evento", SqlDbType.VarChar);
							cmdInsertar.Parameters.Add("@fecha", SqlDbType.DateTime);
							//cmdInsertar.Parameters.Add("@nroNota", SqlDbType.Int);
							cmdInsertar.Parameters.Add("@idUsua", SqlDbType.SmallInt);

							cmdInsertar.Parameters["@orden"].Value = nroOrden;
							cmdInsertar.Parameters["@solicitud"].Value = nroSolicitud;
							cmdInsertar.Parameters["@atencion"].Value = nroAtencion;
							cmdInsertar.Parameters["@idProducto"].Value = idProducto;
							cmdInsertar.Parameters["@cups"].Value = Cups;
							cmdInsertar.Parameters["@evento"].Value = "ORM^SC";
							cmdInsertar.Parameters["@fecha"].Value = DateTime.Now.ToString();
							//cmdInsertar.Parameters["@nroNota"].Value=;
							cmdInsertar.Parameters["@idUsua"].Value=usuario;
							if (cmdInsertar.ExecuteNonQuery()>0)
							{
								if (!updateTraza.insertarTraza(nroAtencion, nroOrden, nroSolicitud, Cups, "ORM^SC", DateTime.Now,0))
								{
									logLabcore.Warn("No se actualizo la Trazabilidad TM-1-- Actualiza:TAT_TRAZA_ORDEN y TAT_TRAZA_ENC:::Atencion:" + nroAtencion + " Orden:" + nroOrden + " Solicitud:" + nroSolicitud + " Producto:" + Cups + " Evento:ORM^SC");
									return ackRecibidos(msg.objMSH, false);
								}
								else
								{
									logLabcore.Info("Se actualizo la Trazabilidad--Transaccion Exitosa!!!:Atencion:" + nroAtencion + " Orden:" + nroOrden + " Solicitud:" + nroSolicitud + " Producto:" + Cups + " Evento:ORM^SC");
									return ackRecibidos(msg.objMSH, true);
								}
							}
							else
							{
								logLabcore.Warn("No se Actualizo TM-2 TAT_TRAZA_ORDEN:" + nroAtencion + " Orden:" + nroOrden + " Solicitud:" + nroSolicitud + " Producto:" + Cups + " Evento:ORM^SC");
								return ackRecibidos(msg.objMSH, false);
							}

						}
						else
						{
							logLabcore.Warn("No se Actualizo TM-3 TAT_TRAZA_ENC:" + nroAtencion + " Orden:" + nroOrden + " Solicitud:" + nroSolicitud + " Producto:" + Cups + " Evento:ORM^SC");
							return ackRecibidos(msg.objMSH, false);
						}
					}
				}
				return ackRecibidos(msg.objMSH,true);
			}
			catch (Exception ex1)
			{
				utilLocal.notificaFalla("Excepcion Toma Muestras:" + ex1.Message);
				string respuestaERROR = "Excepcion:" + ex1.Message + " Source:" + ex1.Source + " Stactrace:" + ex1.StackTrace;
				logLabcore.Warn(ex1.Message, "Excepcion en Toma de Muestras:"+respuestaERROR);
				return respuestaERROR;
			}
		}

		string resultadosLaboratorio(MensajeHL7 msg)
		{
			//logLabcore.Info("MENSAJE DE RESULTADOS RECIBIDO:"+msg);
			SqlConnection Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX);
			string codigoCups = string.Empty;
			try
			{
				string respuesta = string.Empty;
				string solicitudResultados = msg.objORC.ORC4;
				Conex.Open();
				string strConsSoli = "SELECT A.NRO_SOLIC,B.NRO_ORDEN,A.NRO_ATEN,B.USR_ORDEN,B.FECHA_ORD FROM TAT_ENC_SOLSAHI A, TAT_ENC_ORDSAHI B WHERE A.NRO_ORDEN=b.NRO_ORDEN AND NRO_SOLIC='" + solicitudResultados + "'";
				SqlCommand cmdConsSoli = new SqlCommand(strConsSoli, Conex);
				SqlDataReader nroMsgReader = cmdConsSoli.ExecuteReader();
				nroMsgReader.Read();
				string solicitud = nroMsgReader.GetInt32(0).ToString();
				string orden = nroMsgReader.GetInt32(1).ToString();
				string atencion = nroMsgReader.GetInt32(2).ToString();
				string idMedico = nroMsgReader.GetInt32(3).ToString();
				DateTime fechaOrden = nroMsgReader.GetDateTime(4);
				int tipoResultado = 0;
				//*********************** Armado de Encabezado **********************************************
				#region Encabezado
				Conex.Close();
				string encabezado = string.Empty;
				string Detalle1 = string.Empty;
				string Detalle2 = string.Empty;
				string DetalleT = string.Empty;
				ORCClass objORC = msg.objORC;
				encabezado = "Estudio Labcore:" + objORC.ORC3 + " Fecha:" + DateTime.Now.ToString() + "\r\n";
				encabezado = encabezado + "Nombre:" + msg.objPID.Pid5 + " Documento:" + msg.objPID.Pid2 + "\r\n";
				encabezado = encabezado + "________________________________________________________" + "\r\n";
                string usrWRK=objORC.ORC12;
				foreach (string[] segmentoOBR in msg.segmentosOBR)
				{
					OBRClass objOBR = new OBRClass(segmentoOBR);
					if (objOBR.OBR23.Equals("MB"))
					{
						tipoResultado = 797;
					}
					else
					{
						tipoResultado = 638;
					}
					string[] datosProd = objOBR.OBR4.Split('^');
					codigoCups = datosProd[0];
					string bacteriologa = string.Empty;
					string validado = string.Empty;
					encabezado = encabezado + "Estudio:" + objOBR.OBR4.ToString() + "\r\n";
					if (objOBR.OBR21.Length > 1) { bacteriologa = objOBR.OBR21; }
					if (objOBR.OBR22.Length > 1) { validado = objOBR.OBR22.Substring(0, 4) + "-" + objOBR.OBR22.Substring(4, 2) + "-" + objOBR.OBR22.Substring(6, 2) + " " + objOBR.OBR22.Substring(8, 2) + ":" + objOBR.OBR22.Substring(10, 2) + ":" + objOBR.OBR22.Substring(12, 2); }
					encabezado = encabezado + objOBR.OBR21.ToString() + "\r\n";
					encabezado = encabezado + "Fecha Validacion:" + validado + "\r\n";
					encabezado = encabezado + "________________________________________________________" + "\r\n";
					encabezado = encabezado + "Nombre Examen        Resultado	Unidades  V.Ref/P.Corte" + "\r\n";
					strucMensaje structura = new strucMensaje(msg.segmentosOBX);
					int[] posicionesTX = structura.ValoresDifTX;
					List<int> posicionTX = new List<int>();
					posicionTX.Add(structura.ordenTX[0] + 1);
					for (int j = 0; j < posicionesTX.Length; j++)
					{
						if (posicionesTX[j] > 1)
						{
							posicionTX.Add(structura.ordenTX[j] + 1);
						}
					}
					foreach (string[] segmentoOBX in msg.segmentosOBX)
					{

						OBXClass objOBX = new OBXClass(segmentoOBX);
						Int16 nroOBX = Int16.Parse(objOBX.OBX2);
						if (objOBX.OBX3.Equals("NM"))
						{
							int longuitud = objOBX.OBX4.Split('^')[1].Length;
							if (longuitud > 20) { longuitud = 20; }
							Detalle1 = Detalle1 + utilLocal.lineaResultado(objOBX.OBX4.Split('^')[1].ToString().Substring(0, longuitud), objOBX.OBX6, objOBX.OBX7, objOBX.OBX8) + " \r\n";
							//Detalle1 = Detalle1 + objOBX.OBX4.Split('^')[1].ToString().Substring(0, longuitud) + " " + objOBX.OBX6 + "  " + objOBX.OBX7 + "  " + objOBX.OBX8 + " \r\n";
							Detalle2 = "";
							foreach (string[] segmentoNTE in msg.segmentosNTE)
							{
								NTEClass objNTE = new NTEClass(segmentoNTE);///// Hacer Debug Interno **************************************************
								if (objNTE.NTE5.Equals(objOBX.OBX2))
								{
									Detalle2 = Detalle2 + " " + objNTE.NTE4 + "\r\n";
								}
							}
							Detalle1 = Detalle1 + Detalle2;
						}
						else if (objOBX.OBX3.Equals("ST"))
						{
							int longuitud = objOBX.OBX4.Split('^')[1].Length;
							if (longuitud > 20) { longuitud = 20; }
							string[] descPruebas = objOBX.OBX4.Split('^');
							string descPrueba = descPruebas[1].Substring(0, longuitud);
							Detalle1 = Detalle1 + utilLocal.lineaResultadoST(descPrueba, objOBX.OBX6, objOBX.OBX7,"") + " \r\n";
							Detalle2 = "";
							foreach (string[] segmentoNTE in msg.segmentosNTE)
							{
								NTEClass objNTE = new NTEClass(segmentoNTE);///// Hacer Debug Interno **************************************************
								if (objNTE.NTE5.Equals(objOBX.OBX2))
								{
									Detalle2 = Detalle2 + " " + objNTE.NTE4 + "\r\n";
								}
								//Detalle2 = Detalle2 + " " + objNTE.NTE4 + "\r\n";
							}
							Detalle1 = Detalle1 + Detalle2+"\r\n";
						}
						else if (objOBX.OBX3.Equals("TX"))
						{
							if(posicionTX.Exists(x => x.Equals(int.Parse(objOBX.OBX2))))
							{
								Detalle1 = Detalle1 +"Estudio:"+objOBX.OBX4.Split('^')[1] + "\r\n";

							}
							//Detalle1 = Detalle1 + objOBX.OBX4 + " " + objOBX.OBX6+"\r\n";
							Detalle1 = Detalle1 +objOBX.OBX6 + "\r\n";
							Detalle2 = "";
							foreach (string[] segmentoNTE in msg.segmentosNTE)
							{
								NTEClass objNTE = new NTEClass(segmentoNTE);///// Hacer Debug Interno **************************************************
								if (objNTE.NTE5.Equals(objOBX.OBX2))
								{
									Detalle2 = Detalle2 + " " + objNTE.NTE4 + "\r\n";
								}
							}
							Detalle1 = Detalle1 + Detalle2 + "\r\n";
						}
						else
						{
							Detalle1 = Detalle1 + objOBX.OBX5 + "  " + objOBX.OBX6 + "  " + objOBX.OBX7 + "  " + objOBX.OBX8 + "\r\n";
							Detalle1 = Detalle1 + " " + objOBX.OBX10 + "  " + objOBX.OBX11 + "  " + objOBX.OBX13 + "  " + objOBX.OBX13 + "\r\n"; // OBX 15 para fecha de Validacion
							foreach (string[] segmentoNTE in msg.segmentosNTE)
							{
								NTEClass objNTE = new NTEClass(segmentoNTE);///// Hacer Debug Interno **************************************************
								Detalle2 = Detalle2 + " " + objNTE.NTE4 + "\r\n";
							}
						}
					}
				}
    			DetalleT = encabezado + Detalle1; //+ Detalle2;
	# endregion
                //*********************** fin de Armado Encabezado *******************************************
				int interpretado = 0;
				int nroNota = 0;
				using (Conex)
				{
					Conex.Open();
                    string strConsNotas = "SELECT ENC_NOTA FROM TAT_TRAZA_ENC WHERE ENC_SOLIC=" + solicitud + " AND ENC_ORDEN=" + orden + " AND ENC_ATEN=" + atencion + " AND ENC_ESQM=" + tipoResultado + " AND EVT_ORD='ORU^R01'";
                    //string strConsNotas = "SELECT ENC_NOTA FROM TAT_TRAZA_ORDEN WHERE NRO_SOLIC=" + solicitud + " AND NRO_ORDEN=" + orden + " AND NRO_ATEN=" + atencion + "AND  AND EVT_ORD='ORU^R01'";
					SqlCommand cmdConsNotas = new SqlCommand(strConsNotas, Conex);
					SqlDataReader readConsNotas = cmdConsNotas.ExecuteReader();
					if (readConsNotas.HasRows)
					{
						readConsNotas.Read();
						nroNota = readConsNotas.GetInt32(0);
					}
					string strValidaInterpreta = "SELECT CASE WHEN (ISNULL(EstadoApDx,0) = 0 AND Interpretacion IS NOT NULL) THEN 1 ELSE 0 END FROM hceEsquemasdeAte WHERE (IdEsquema = 638 OR IdEsquema=797) AND (IdAtencion = " + atencion + ") AND (IdEsquemaDeAtencion = " + nroNota + ")";
					SqlCommand cmdValidaInterpreta = new SqlCommand(strValidaInterpreta, Conex);
					SqlDataReader readValidaInterpreta = cmdValidaInterpreta.ExecuteReader();
					if (readValidaInterpreta.HasRows)
					{
						readValidaInterpreta.Read();
						interpretado = readValidaInterpreta.GetInt32(0);
					}
				}
				string[] segmentoOBRWrk = msg.segmentosOBR[0];
				OBRClass objOBRwrk = new OBRClass(segmentoOBRWrk);
				if (objOBRwrk.OBR25.Equals("R") && nroNota == 0)    // *********************** LABCORE DEBE ENVIAR R cuando se trate de una revalidacion
				{
					logLabcore.Warn("Para realizar Revalidacion debe Existir una Nota de Resultado. Se envio una Revalidacion para un Examen que no se Cargado en Historia");
					return ackResultados(msg.objMSH, false);
				}
				else
				{
					if ((nroNota == 0 && objOBRwrk.OBR25.Equals("F")) || interpretado > 0)//********************** si ya fue interpretado
					{
						// Insertar nota nueva
						try
						{
							using (Conex)
							{
								Conex.ConnectionString = Properties.Settings.Default.LabcoreDBConXX;
								Conex.Open();
								int NumeroNota = utilLocal.consecutivoSistabla("hceNotasAte");
								if(NumeroNota==0)
								{
									logLabcore.Warn("No fue posible obtener el Consecutivo para la Nota de la Tabla hceNotasAte");
									return ackResultados(msg.objMSH, false);
								}
								SqlTransaction txResultado = Conex.BeginTransaction("TxVenta");
								string actHistoria1 = "INSERT INTO hceNotasAte (IdNota, IdAtencion, FecNota, IdUbicacion, DesNota, IdUsuarioR, IdTipoNota)	VALUES (@nota,@atencion,@fechaNota,@ubicacion,@desNota,@usuario,@tipoNota)";
								SqlCommand cmdNotasAte = new SqlCommand(actHistoria1, Conex, txResultado);
								cmdNotasAte.Parameters.Add("@nota",SqlDbType.Int);
								cmdNotasAte.Parameters.Add("@atencion",SqlDbType.Int);
								cmdNotasAte.Parameters.Add("@fechaNota",SqlDbType.DateTime);
								cmdNotasAte.Parameters.Add("@ubicacion",SqlDbType.Int);
								cmdNotasAte.Parameters.Add("@desNota",SqlDbType.Text);
								cmdNotasAte.Parameters.Add("@usuario",SqlDbType.SmallInt);
								cmdNotasAte.Parameters.Add("@tipoNota",SqlDbType.SmallInt);

								cmdNotasAte.Parameters["@nota"].Value=NumeroNota;
								cmdNotasAte.Parameters["@atencion"].Value=atencion;
								cmdNotasAte.Parameters["@fechaNota"].Value=fechaOrden;
								cmdNotasAte.Parameters["@ubicacion"].Value=18;
								cmdNotasAte.Parameters["@desNota"].Value=DetalleT;
								cmdNotasAte.Parameters["@usuario"].Value=idMedico;
								cmdNotasAte.Parameters["@tipoNota"].Value = tipoResultado;
								logLabcore.Info("********************* Valor de tipoNota:" + tipoResultado + "  Nota:"+NumeroNota+"   Atencion:"+atencion+"****************************");
								if (cmdNotasAte.ExecuteNonQuery() > 0)
								{
									string actHistoria2 = "INSERT INTO hceEsquemasdeAte (IdAtencion, IdEsquema, IdEsquemadeAtencion, IdUbicacion, IdMedico, IdTraslado, FecEsquema, IndHabilitado, IndActivado, FecCerrado, EstadoApDx, idOrden) ";
									actHistoria2 = actHistoria2 + "VALUES (@atencion,@esquema,@esquemaAte,@ubicacion, @medico,@traslado,@fechaEsquema,@indicadorHabilitado, @indicadorActivado,@fechaCerrado, @EstadoApDx, @orden)";
									SqlCommand cmdEsquemasAte = new SqlCommand(actHistoria2, Conex, txResultado);
									cmdEsquemasAte.Parameters.Add("@atencion", SqlDbType.Int);
									cmdEsquemasAte.Parameters.Add("@esquema", SqlDbType.Int);
									cmdEsquemasAte.Parameters.Add("@esquemaAte", SqlDbType.Int);
									cmdEsquemasAte.Parameters.Add("@ubicacion", SqlDbType.SmallInt);
									cmdEsquemasAte.Parameters.Add("@medico", SqlDbType.SmallInt);
									cmdEsquemasAte.Parameters.Add("@traslado", SqlDbType.Int);
									cmdEsquemasAte.Parameters.Add("@fechaEsquema", SqlDbType.DateTime);
									cmdEsquemasAte.Parameters.Add("@indicadorHabilitado", SqlDbType.Bit);
									cmdEsquemasAte.Parameters.Add("@indicadorActivado", SqlDbType.Bit);
									cmdEsquemasAte.Parameters.Add("@fechaCerrado", SqlDbType.DateTime);
									cmdEsquemasAte.Parameters.Add("@EstadoApDx", SqlDbType.Int);
									cmdEsquemasAte.Parameters.Add("@orden", SqlDbType.Int);

									cmdEsquemasAte.Parameters["@atencion"].Value=atencion;
									cmdEsquemasAte.Parameters["@esquema"].Value=tipoResultado;
									cmdEsquemasAte.Parameters["@esquemaAte"].Value=NumeroNota;
									cmdEsquemasAte.Parameters["@ubicacion"].Value=18;
									cmdEsquemasAte.Parameters["@medico"].Value=idMedico;
									cmdEsquemasAte.Parameters["@traslado"].Value=1;
									cmdEsquemasAte.Parameters["@fechaEsquema"].Value=DateTime.Now;
									cmdEsquemasAte.Parameters["@indicadorHabilitado"].Value=1;
									cmdEsquemasAte.Parameters["@indicadorActivado"].Value=0;
									cmdEsquemasAte.Parameters["@fechaCerrado"].Value=fechaOrden;
									cmdEsquemasAte.Parameters["@EstadoApDx"].Value=4;
									cmdEsquemasAte.Parameters["@orden"].Value=orden;


									if (cmdEsquemasAte.ExecuteNonQuery() > 0)
									{
										foreach (string[] segmentoOBRx in msg.segmentosOBR)
										{
											using (Conex2)
											{
												OBRClass objOBRx = new OBRClass(segmentoOBRx);
												string[] producto = objOBRx.OBR4.Split('^');
												string Cups = producto[0];
												string validado = string.Empty;
												if (objOBRx.OBR22.Length > 1) { validado = objOBRx.OBR22.Substring(0, 4) + "-" + objOBRx.OBR22.Substring(4, 2) + "-" + objOBRx.OBR22.Substring(6, 2) + " " + objOBRx.OBR22.Substring(8, 2) + ":" + objOBRx.OBR22.Substring(10, 2) + ":" + objOBRx.OBR22.Substring(12, 2); } else { validado = DateTime.Now.ToString(); }
												Trazabilidad updateTraza = new Trazabilidad();

												try
												{
													Conex2.Open();
													string qryProducto = "SELECT IdProducto FROM proProducto WHERE CodProducto='" + Cups + "'";
													SqlCommand cmdProducto = new SqlCommand(qryProducto, Conex, txResultado);
													SqlDataReader readerProducto = cmdProducto.ExecuteReader();
													readerProducto.Read();
													Int32 idProducto = readerProducto.GetInt32(0);
													readerProducto.Close();
                                                    string strInsEncabeza = "INSERT INTO TAT_TRAZA_ENC (ENC_ORDEN,ENC_SOLIC,ENC_ATEN,EVT_ORD,FECHA_EVT,ENC_NOTA,ENC_ESQM) VALUES(@orden,@solicitud,@atencion,@evento,@fecha,@nota,@Esquema)";
													SqlCommand cmdInsEncabeza = new SqlCommand(strInsEncabeza, Conex, txResultado);
													cmdInsEncabeza.Parameters.Add("@orden",SqlDbType.Int);
													cmdInsEncabeza.Parameters.Add("@solicitud", SqlDbType.Int);
													cmdInsEncabeza.Parameters.Add("@atencion", SqlDbType.Int);
													cmdInsEncabeza.Parameters.Add("@evento", SqlDbType.VarChar);
													cmdInsEncabeza.Parameters.Add("@fecha", SqlDbType.DateTime);
													cmdInsEncabeza.Parameters.Add("@nota", SqlDbType.Int);
                                                    cmdInsEncabeza.Parameters.Add("@Esquema",SqlDbType.Int);

													cmdInsEncabeza.Parameters["@orden"].Value=orden;
													cmdInsEncabeza.Parameters["@solicitud"].Value=solicitud;
													cmdInsEncabeza.Parameters["@atencion"].Value=atencion;
													cmdInsEncabeza.Parameters["@evento"].Value="ORU^R01";
													cmdInsEncabeza.Parameters["@fecha"].Value=DateTime.Now;
													cmdInsEncabeza.Parameters["@nota"].Value=NumeroNota;
                                                    cmdInsEncabeza.Parameters["@Esquema"].Value = tipoResultado;

													if (cmdInsEncabeza.ExecuteNonQuery() > 0)
													{
														string qryInsertar = "INSERT INTO TAT_TRAZA_ORDEN (NRO_ORDEN,NRO_SOLIC,NRO_ATEN,ID_PROD,COD_CUPS,EVT_ORD,FECHA_EVT,NRO_NOTA,ID_USUA) VALUES(@orden,@solicitud,@atencion,@producto,@cups,@evento,@fecha,@nroNota,@idUsua)";
														SqlCommand cmdInsertar = new SqlCommand(qryInsertar, Conex, txResultado);
														cmdInsertar.Parameters.Add("@orden",SqlDbType.Int);
														cmdInsertar.Parameters.Add("@solicitud", SqlDbType.Int);
														cmdInsertar.Parameters.Add("@atencion", SqlDbType.Int);
														cmdInsertar.Parameters.Add("@producto", SqlDbType.Int);
														cmdInsertar.Parameters.Add("@cups", SqlDbType.VarChar);
														cmdInsertar.Parameters.Add("@evento", SqlDbType.VarChar);
														cmdInsertar.Parameters.Add("@fecha", SqlDbType.DateTime);
                                                        cmdInsertar.Parameters.Add("@nroNota", SqlDbType.Int);
                                                        cmdInsertar.Parameters.Add("@idUsua", SqlDbType.SmallInt);

														cmdInsertar.Parameters["@orden"].Value=orden;
														cmdInsertar.Parameters["@solicitud"].Value=solicitud;
														cmdInsertar.Parameters["@atencion"].Value=atencion;
														cmdInsertar.Parameters["@producto"].Value=idProducto;
														cmdInsertar.Parameters["@cups"].Value=Cups;
														cmdInsertar.Parameters["@evento"].Value="ORU^R01";
														cmdInsertar.Parameters["@fecha"].Value = DateTime.Now;
                                                        cmdInsertar.Parameters["@nroNota"].Value = NumeroNota;
                                                        cmdInsertar.Parameters["@idUsua"].Value = usrWRK;

														if (cmdInsertar.ExecuteNonQuery() > 0)
														{
															if (!updateTraza.insertarTraza(atencion, orden, solicitud, Cups, "ORU^R01", DateTime.Now, NumeroNota))
															{
																logLabcore.Warn("No se actualizo la Trazabilidad de Resultados-1:Atencion:" + atencion + " Orden:" + orden + " Solicitud:" + solicitud + " Producto:" + Cups + " Evento:ORU^R01");
																txResultado.Rollback();
																return ackResultados(msg.objMSH, false);
															}
															else
															{
																logLabcore.Info("Se actualizo la Trazabilidad de Resultados-2:Atencion:" + atencion + " Orden:" + orden + " Solicitud:" + solicitud + " Producto:" + Cups + " Evento:ORU^R01");
																if (!updateTraza.insertarTraza(atencion, orden, solicitud, Cups, "EVT_VAL", DateTime.Parse(validado), NumeroNota))
																{
																	logLabcore.Warn("No se actualizo la Trazabilidad de Validacion en Resultado:Atencion:" + atencion + " Orden:" + orden + " Solicitud:" + solicitud + " Producto:" + Cups + " Evento:EVT_EVAL");
																	txResultado.Rollback();
																	return ackResultados(msg.objMSH, false);
																}
																else
																{
																	logLabcore.Info("Se actualizo la Trazabilidad de Validacion En Resultados-3:Atencion:" + atencion + " Orden:" + orden + " Solicitud:" + solicitud + " Producto:" + Cups + " Evento:EVT_EVAL");
																	txResultado.Commit();
																	return ackResultados(msg.objMSH, true);
																}
															}
														}
														else
														{
															logLabcore.Warn("No se actualizo la Trazabilidad Basica de Validacion Tabla(TAT_TRAZA_ORDEN) Subiendo Resultados-4:Atencion:" + atencion + " Orden:" + orden + " Solicitud:" + solicitud + " Producto:" + Cups + " Evento:EVT_EVAL");
															txResultado.Rollback();
															return ackResultados(msg.objMSH, false);
														}
													}
													else
													{
														logLabcore.Warn("No se actualizo la Trazabilidad Basica de Validacion Tabla(TAT_TRAZA_ENC) Subiendo Resultados-5:Atencion:" + atencion + " Orden:" + orden + " Solicitud:" + solicitud + " Producto:" + Cups + " Evento:EVT_EVAL");
														txResultado.Rollback();
														return ackResultados(msg.objMSH, false);
													}
												}
												catch (SqlException sqlEx)
												{
													utilLocal.notificaFalla("Excepcion de SQL Guardando resultados-6:"+sqlEx.Message);
													logLabcore.Warn("Excepcion de SQL Guardando resultados-6:",sqlEx.Message);
													return ackResultados(msg.objMSH, false);
												}
											}
										}
										txResultado.Commit();
										return ackResultados(msg.objMSH, true);
									}
									else
									{
										txResultado.Rollback("TxVenta");
										logLabcore.Warn("No fue posible Insertar la Informacion de Resultados en hceEsquemasdeAte");
										return ackResultados(msg.objMSH, false);
									}
								}
								else
								{
									txResultado.Rollback("TxVenta");
									logLabcore.Warn("No fue posible Insertar Informacion en la Tabla:hceNotasAte");
									return ackResultados(msg.objMSH, false);
								}
							}
						}
						catch (SqlException sqlEx1)
						{
							utilLocal.notificaFalla("Excepcion SQL Guardando Resultados-7"+sqlEx1.Message);
							logLabcore.Warn(sqlEx1.Message, "Excepcion SQL Guardando Resultados");
							return ackResultados(msg.objMSH, false);
						}

					}
					else // Buscar Nota y Actualizar Porque No ha Sido Interpretado
					{
						using (Conex)
						{
							if (interpretado == 0)
							{
								try
								{
									using (Conex)
									{
										Conex.ConnectionString = Properties.Settings.Default.LabcoreDBConXX;
										Conex.Open();
	  									SqlTransaction txResultado = Conex.BeginTransaction("TxResultado");
										string strActualizar = "UPDATE hceNotasAte SET desNota =@textoNota + CONVERT(VARCHAR(MAX),desNota) WHERE (idNota = @nroNota) AND (idAtencion = @atencion)";
										SqlCommand cmdActualizar = new SqlCommand(strActualizar, Conex,txResultado);
										cmdActualizar.Parameters.Add("@textoNota", System.Data.SqlDbType.VarChar);
										cmdActualizar.Parameters.Add("@nroNota", System.Data.SqlDbType.Int);
										cmdActualizar.Parameters.Add("@atencion", System.Data.SqlDbType.Int);
										cmdActualizar.Parameters["@textoNota"].Value = DetalleT;
										cmdActualizar.Parameters["@nroNota"].Value = nroNota;
										cmdActualizar.Parameters["@atencion"].Value = atencion;
										if (cmdActualizar.ExecuteNonQuery() > 0)
										{
											using (Conex2)
											{
												foreach (string[] segmentoOBRx in msg.segmentosOBR)
												{
													OBRClass objOBRx = new OBRClass(segmentoOBRx);
													string[] producto = objOBRx.OBR4.Split('^');
													string Cups = producto[0];
													string validado = string.Empty;
													if (objOBRx.OBR22.Length > 1) { validado = objOBRx.OBR22.Substring(0, 4) + "-" + objOBRx.OBR22.Substring(4, 2) + "-" + objOBRx.OBR22.Substring(6, 2) + " " + objOBRx.OBR22.Substring(8, 2) + ":" + objOBRx.OBR22.Substring(10, 2) + ":" + objOBRx.OBR22.Substring(12, 2); } else { validado = DateTime.Now.ToString(); }
													Trazabilidad updateTraza = new Trazabilidad();
													try
													{
														Conex2.Open();
														string qryProducto = "SELECT IdProducto FROM proProducto WHERE CodProducto='" + Cups + "'";
														SqlCommand cmdProducto = new SqlCommand(qryProducto, Conex, txResultado);
														SqlDataReader readerProducto = cmdProducto.ExecuteReader();
														readerProducto.Read();
														Int32 idProducto = readerProducto.GetInt32(0);
														readerProducto.Close();
                                                        string strInsEncabeza = "INSERT INTO TAT_TRAZA_ENC (ENC_ORDEN,ENC_SOLIC,ENC_ATEN,EVT_ORD,FECHA_EVT,ENC_NOTA) VALUES(@orden,@solicitud,@atencion,@evento,@fecha,@nota,@Esquema)";
														SqlCommand cmdInsEncabeza = new SqlCommand(strInsEncabeza, Conex, txResultado);
														cmdInsEncabeza.Parameters.Add("@orden", SqlDbType.Int);
														cmdInsEncabeza.Parameters.Add("@solicitud", SqlDbType.Int);
														cmdInsEncabeza.Parameters.Add("@atencion", SqlDbType.Int);
														cmdInsEncabeza.Parameters.Add("@evento", SqlDbType.VarChar);
														cmdInsEncabeza.Parameters.Add("@fecha", SqlDbType.DateTime);
														cmdInsEncabeza.Parameters.Add("@nota", SqlDbType.Int);
                                                        cmdInsEncabeza.Parameters.Add("@Esquema", SqlDbType.Int);

														cmdInsEncabeza.Parameters["@orden"].Value = orden;
														cmdInsEncabeza.Parameters["@solicitud"].Value = solicitud;
														cmdInsEncabeza.Parameters["@atencion"].Value = atencion;
														cmdInsEncabeza.Parameters["@evento"].Value = "ORU^R01";
														cmdInsEncabeza.Parameters["@fecha"].Value = DateTime.Now;
														cmdInsEncabeza.Parameters["@nota"].Value = nroNota;
                                                        cmdInsEncabeza.Parameters["@Esquema"].Value = tipoResultado;
														///****************
														if (cmdInsEncabeza.ExecuteNonQuery() > 0)
														{
                                                            string qryInsertar = "INSERT INTO TAT_TRAZA_ORDEN (NRO_ORDEN,NRO_SOLIC,NRO_ATEN,ID_PROD,COD_CUPS,EVT_ORD,FECHA_EVT,ID_USUA) VALUES(@orden,@solicitud,@atencion,@producto,@cups,@evento,@fecha,@idUsua)";
															SqlCommand cmdInsertar = new SqlCommand(qryInsertar, Conex, txResultado);
															/////********************
															cmdInsertar.Parameters.Add("@orden", SqlDbType.Int);
															cmdInsertar.Parameters.Add("@solicitud", SqlDbType.Int);
															cmdInsertar.Parameters.Add("@atencion", SqlDbType.Int);
															cmdInsertar.Parameters.Add("@producto", SqlDbType.Int);
															cmdInsertar.Parameters.Add("@cups", SqlDbType.VarChar);
															cmdInsertar.Parameters.Add("@evento", SqlDbType.VarChar);
															cmdInsertar.Parameters.Add("@fecha", SqlDbType.DateTime);
                                                            cmdInsertar.Parameters.Add("@idUsua", SqlDbType.SmallInt);

															cmdInsertar.Parameters["@orden"].Value = orden;
															cmdInsertar.Parameters["@solicitud"].Value = solicitud;
															cmdInsertar.Parameters["@atencion"].Value = atencion;
															cmdInsertar.Parameters["@producto"].Value = idProducto;
															cmdInsertar.Parameters["@cups"].Value = Cups;
															cmdInsertar.Parameters["@evento"].Value = "ORU^R01";
															cmdInsertar.Parameters["@fecha"].Value = DateTime.Now;
                                                            cmdInsertar.Parameters["@idUsua"].Value = usrWRK;
															if (cmdInsertar.ExecuteNonQuery() > 0)
															{

																if (updateTraza.insertarTraza(atencion, orden, solicitud, Cups, "ORU^R01", DateTime.Now, nroNota) && updateTraza.insertarTraza(atencion, orden, solicitud, Cups, "EVT_VAL", DateTime.Parse(validado), nroNota))
																{
                                                                    logLabcore.Info("Se actualizo la Validacion y Cargue de Resultados en Historia Nro Nota:" + nroNota + " Tipo de Esquema:" + tipoResultado);
																	txResultado.Commit();
																	return ackResultados(msg.objMSH, true);
																}
																else
																{
																	logLabcore.Warn("No se actualizo la Trazabilidad de Validacion e Historia Subiendo Resultados-7:Atencion:" + atencion + " Orden:" + orden + " Solicitud:" + solicitud + " Producto:" + Cups);
																	txResultado.Rollback();
																	return ackResultados(msg.objMSH, false);
																}

																////////if (!updateTraza.insertarTraza(atencion, orden, solicitud, Cups, "ORU^R01", DateTime.Now, nroNota))
																////////{
																////////    logLabcore.Warn("No se actualizo la Trazabilidad de Resultados:Atencion:" + atencion + " Orden:" + orden + " Solicitud:" + solicitud + " Producto:" + Cups + " Evento:ORU^R01");
																////////    txResultado.Rollback();
																////////    return ackResultados(msg.objMSH, false);
																////////}
																////////else
																////////{
																////////    logLabcore.Info("Se actualizo la Trazabilidad de Resultados:Atencion:" + atencion + " Orden:" + orden + " Solicitud:" + solicitud + " Producto:" + Cups + " Evento:ORU^R01");
																////////    if (!updateTraza.insertarTraza(atencion, orden, solicitud, Cups, "EVT_VAL", DateTime.Parse(validado), nroNota))
																////////    {
																////////        logLabcore.Warn("No se actualizo la Trazabilidad de Validacion:Atencion:" + atencion + " Orden:" + orden + " Solicitud:" + solicitud + " Producto:" + Cups + " Evento:EVT_EVAL");
																////////        txResultado.Rollback();
																////////        return ackResultados(msg.objMSH, false);
																////////    }
																////////    else
																////////    {
																////////        logLabcore.Info("Se actualizo la Trazabilidad de Validacion:Atencion:" + atencion + " Orden:" + orden + " Solicitud:" + solicitud + " Producto:" + Cups + " Evento:EVT_EVAL");
																////////        txResultado.Commit();
																////////        return ackResultados(msg.objMSH, true);
																////////    }
																////////}
															}
															else
															{
																logLabcore.Warn("No se actualizo la Trazabilidad Basica de Validacion Tabla(TAT_TRAZA_ORDEN) Subiendo Resultados-(8):Atencion:" + atencion + " Orden:" + orden + " Solicitud:" + solicitud + " Producto:" + Cups + " Evento:EVT_EVAL");
																txResultado.Rollback();
																return ackResultados(msg.objMSH, false);
															}
														}
														else
														{
															logLabcore.Warn("No se actualizo la Trazabilidad Basica de Validacion Tabla(TAT_TRAZA_ENC) Subiendo Resultados-(9):Atencion:" + atencion + " Orden:" + orden + " Solicitud:" + solicitud + " Producto:" + Cups + " Evento:EVT_EVAL");
															txResultado.Rollback();
															return ackResultados(msg.objMSH, false);
														}
													}
													catch (SqlException sqlEx)
													{
														logLabcore.Warn("Excepcion de SQL Subiendo Resultados-(10):", sqlEx.Message);
														return ackResultados(msg.objMSH, false);
													}
												}
											}
											txResultado.Commit();
											return ackResultados(msg.objMSH, true);
										}
										else
										{
											txResultado.Rollback("TxVenta");
											logLabcore.Warn("No fue posible Insertar la Informacion de Resultados en hceEsquemasdeAte(11)");
											return ackResultados(msg.objMSH, false);
										}
										//}
										//else
										//{
										//    txVenta.Rollback("TxVenta");
										//    logLabcore.Warn("No fue posible Insertar Informacion en la Tabla:hceNotasAte");
										//    return ackResultados(msg.objMSH, false);
										//}
									}
								}
								catch (SqlException sqlEx1)
								{
									utilLocal.notificaFalla("Excepcion SQL Guardando Resultados:"+sqlEx1.Message);
									logLabcore.Warn(sqlEx1.Message, "Excepcion SQL Guardando Resultados");
									return ackResultados(msg.objMSH, false);
								}
							}
							else
							{
								logLabcore.Warn("La actualizacion de la Nota en hceNotasAte, Fallo. !!!:"+atencion+" Nota:"+nroNota);
								respuesta = ackResultados(msg.objMSH, false);
							}
						}
							return respuesta;
					}
				}
			}
				catch(SqlException sqlExp)
				{
					utilLocal.notificaFalla("Error Procesando Resultados:"+sqlExp.Message);
					logLabcore.Warn("Error Procesando Resultados:",sqlExp.Message);
					return ackResultados(msg.objMSH, false);
				}
		}

		string recibirVentaLab(MensajeHL7 msg)
		{
			string textoMensaje = string.Empty;
			string respuesta = string.Empty;
			string rptaLab = string.Empty;
			string mensajeSRZ = SerializarHL7(msg);
			SqlConnection Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX);
			string nroAtencion = string.Empty;
			string nroSolicitud = string.Empty;
			string nroOrden = string.Empty;
			string[] cupsVentas=msg.segmentosOBR[0];
			string[] cupsVenta=cupsVentas[3].ToString().Split('^');
			string codigoCups = cupsVenta[0];
			Int32 idproducto = 0;
			string codCups=string.Empty;
			try
			{
				using (Conex)
				{
					Conex.Open();
					string strConsultarAtn = "SELECT NRO_ATEN,NRO_SOLIC,NRO_ORDEN,ID_PROD,COD_CUPS FROM TAT_DET_SOLSAHI WHERE NRO_SOLIC=" + msg.objORC.ORC4 + " AND COD_CUPS='" + codigoCups + "'";
					SqlCommand cmdConsultaAtn = new SqlCommand(strConsultarAtn, Conex);
					SqlDataReader conCursor = cmdConsultaAtn.ExecuteReader();
					conCursor.Read();
					nroAtencion = conCursor.GetInt32(0).ToString();
					nroSolicitud = conCursor.GetInt32(1).ToString();
					nroOrden = conCursor.GetInt32(2).ToString();
					idproducto = conCursor.GetInt32(3);
					codCups = conCursor.GetString(4);
					string consecutivoLAB = msg.objMSH.Msh10;
					//********************************************** validar primero si ya existe la solicitud de venta. en TAT_VENT_XPROC
					string strInsertarVta = "INSERT INTO TAT_VENT_XPROC VALUES(@atencion,@solicitud,@orden,@idProd,@Cups,@consecutivo,@HL7)";
					SqlCommand cmdInsertarVta = new SqlCommand(strInsertarVta, Conex);
					cmdInsertarVta.Parameters.Add("@atencion", System.Data.SqlDbType.VarChar);
					cmdInsertarVta.Parameters.Add("@solicitud", System.Data.SqlDbType.VarChar);
					cmdInsertarVta.Parameters.Add("@orden", System.Data.SqlDbType.VarChar);
					cmdInsertarVta.Parameters.Add("@idProd",System.Data.SqlDbType.Int);
					cmdInsertarVta.Parameters.Add("@Cups", System.Data.SqlDbType.VarChar);
					cmdInsertarVta.Parameters.Add("@consecutivo", System.Data.SqlDbType.VarChar);
					cmdInsertarVta.Parameters.Add("@HL7", System.Data.SqlDbType.VarChar);

					cmdInsertarVta.Parameters["@atencion"].Value= nroAtencion;
					cmdInsertarVta.Parameters["@solicitud"].Value=nroSolicitud;
					cmdInsertarVta.Parameters["@orden"].Value=nroOrden;
					cmdInsertarVta.Parameters["@idProd"].Value = idproducto;
					cmdInsertarVta.Parameters["@Cups"].Value = codCups;
					cmdInsertarVta.Parameters["@consecutivo"].Value=consecutivoLAB;
					cmdInsertarVta.Parameters["@HL7"].Value=mensajeSRZ;
					if (cmdInsertarVta.ExecuteNonQuery() > 0)
					{
						textoMensaje = ackRecibidos(msg.objMSH, true);
						srLabcoreTAT.WSSolicitudesClient cliente = new srLabcoreTAT.WSSolicitudesClient();
						rptaLab = cliente.GetHL7Msg(textoMensaje);
						logLabcore.Info("Mensaje Venta Recibida:" + textoMensaje);
						logLabcore.Info("Respuesta Labcore:" + rptaLab);
					}
				}
			}
			catch(SqlException sqlEx)
			{
				utilLocal.notificaFalla("Exp SQL Recibiendo Venta,"+sqlEx.Message);
				logLabcore.Warn("Excepcion SQL Procesando la Venta",sqlEx.Message);
				respuesta = ackRecibidos(msg.objMSH, false);
			}
			catch(Exception exp1)
			{
				utilLocal.notificaFalla("Excp Recibiendo Venta"+exp1.Message);
				logLabcore.Warn("Excepcion Procesando la Venta", exp1.Message+"  "+exp1.StackTrace);
				respuesta = ackRecibidos(msg.objMSH, false);
			}

			return respuesta;
		}

		public string SerializarHL7( MensajeHL7 mensajeOriginal)
		{
			StringBuilder serializado = new StringBuilder();
			XmlSerializer SerializadorHL7 = new XmlSerializer(typeof(MensajeHL7));
			StringWriter swWriter = new StringWriter(serializado);
			SerializadorHL7.Serialize(swWriter, mensajeOriginal);
			return serializado.ToString();
		}

		public MensajeHL7 DeserializadorHL7(string dataXML)
		{
			XmlDocument xDoc = new XmlDocument();
			 xDoc.LoadXml(dataXML);
			 XmlNodeReader xNodeReader = new XmlNodeReader(xDoc.DocumentElement);
			 XmlSerializer xmlSerializer = new XmlSerializer(typeof(MensajeHL7));
			 var mensaje = xmlSerializer.Deserialize(xNodeReader);
			 MensajeHL7 mensajeDeserializado = (MensajeHL7)mensaje;
			 return mensajeDeserializado;
		}

		public string cancelarVentaLabAsync(MensajeHL7 msg, string idMovimientoCancelar)  // *********continuar aqui
		{
			SqlConnection Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX);
			string nroAtencion = string.Empty;
			string nroSolicitud = string.Empty;
			string nroOrden = string.Empty;
			Int64 idMovimientoCan = 0;
			string respuesta = string.Empty;
			string codigoIdCausal = msg.objORC.ORC17;
			//var valorVenta=0;
			using (Conex)
			{
				Conex.Open();
				string strConsultarAtn = "SELECT NRO_ATEN,NRO_SOLIC,NRO_ORDEN FROM TAT_ENC_SOLSAHI WHERE NRO_SOLIC=" + msg.objORC.ORC4;
				SqlCommand cmdConsultaAtn = new SqlCommand(strConsultarAtn, Conex);
				SqlDataReader conCursor = cmdConsultaAtn.ExecuteReader();
				conCursor.Read();
				nroAtencion = conCursor.GetInt32(0).ToString();
				nroSolicitud = conCursor.GetInt32(1).ToString();
				nroOrden = conCursor.GetInt32(2).ToString();
			}
			Conex.ConnectionString = Properties.Settings.Default.LabcoreDBConXX;

			using (Conex)
			{
				Conex.Open();
				string strValidaFacturaAnul = "SELECT A.idFactura,A.indnotacred FROM facfactura A INNER JOIN facfacturadet B ON A.idfactura=B.idfactura WHERE A.indnotacred<>'T' AND B.idmovimiento =" + idMovimientoCancelar;
				SqlCommand cmdValidaFacturaAnul = new SqlCommand(strValidaFacturaAnul,Conex);
				SqlDataReader rdValidaFacturaAnul=cmdValidaFacturaAnul.ExecuteReader();
				if (!rdValidaFacturaAnul.HasRows)
				{
					string strValidaFacturaAnul2 = "SELECT * FROM facfactura A INNER JOIN facmovnofacturado B ON A.idnofacturado=B.idnofacturado WHERE A.IndNotaCred<>'T' AND b.idmovimiento=" + idMovimientoCancelar;
					SqlCommand cmdValidaFacturaAnul2 = new SqlCommand(strValidaFacturaAnul2, Conex);
					SqlDataReader rdValidaFacturaAnul2 = cmdValidaFacturaAnul2.ExecuteReader();
					if (!rdValidaFacturaAnul2.HasRows)
					{
						string strMovFacturado = "SELECT * FROM facMovimiento A INNER JOIN facfacturaDet B ON a.IdMovimiento=B.IdMovimiento WHERE a.IdMovimiento=" + idMovimientoCancelar;
						SqlCommand cmdMovFacturado = new SqlCommand(strMovFacturado,Conex);
						SqlDataReader rdMovFacturado = cmdMovFacturado.ExecuteReader();
						if(!rdMovFacturado.HasRows)
						{
							string strMovReversado = "SELECT * FROM facMovimiento WHERE IdMovimientoBase<>IdMovimiento AND IdMovimiento=" + idMovimientoCancelar;
							SqlCommand cmdMovReversado = new SqlCommand(strMovReversado, Conex);
							SqlDataReader rdMovReversado = cmdMovReversado.ExecuteReader();
							if(!rdMovReversado.HasRows)
							{
								idMovimientoCan=0;
								Int64 NumMovimientoCan=0;
								//************* Insertar en FacMovimiento - facMovimientoDetalle y tabla de cancelacion de trazabilidad.
								string strIdMovimientoCan="SELECT Consecutivo FROM Sistablaventas";
								SqlCommand cmdIdMovimientoCan=new SqlCommand(strIdMovimientoCan,Conex);
								SqlDataReader rdIdMovimientoCan=cmdIdMovimientoCan.ExecuteReader();
								if(rdIdMovimientoCan.HasRows)
								{
									rdIdMovimientoCan.Read();
									idMovimientoCan=rdIdMovimientoCan.GetInt64(0)+1;
									string strActualizaCons="UPDATE Sistablaventas SET Consecutivo=Consecutivo+1";
									SqlCommand cmdActualizaCons=new SqlCommand(strActualizaCons,Conex);
									cmdActualizaCons.ExecuteNonQuery();
								}
								string strNumMovimientoCan="SELECT NumActual FROM genNumeracion WHERE IdTipoDocumento=22";
								SqlCommand cmdNumMovimientoCan=new SqlCommand(strNumMovimientoCan,Conex);
								SqlDataReader rdNumMovimientoCan=cmdNumMovimientoCan.ExecuteReader();
								if(rdNumMovimientoCan.HasRows)
								{
									rdNumMovimientoCan.Read();
									NumMovimientoCan=Int64.Parse((rdNumMovimientoCan.GetFloat(0)+1).ToString());
									string strActualizaNum="UPDATE genNumeracion SET NumActual=NumActual+1 WHERE IdTipoDocumento=22";
									SqlCommand cmdActualizaNum=new SqlCommand(strActualizaNum,Conex);
									cmdActualizaNum.ExecuteNonQuery();
								}
								string strVenta = "SELECT idDestino,IdProductoTipo,IdResponsable,IdUbicacionConsumo,IdUbicacionEntrega,IdProcPrincipal,DocRespaldo,IdTarifa,IdServicio,IndFacturado,IndDevuelto,IndCompleto,IndHabilitado,IndPaquete,Cama,SecTran,SecTranB,IdMovimientoPral,IdDestinoIni,TipoVenta FROM facMovimiento WHERE IdMovimiento=" + idMovimientoCancelar;
								SqlCommand cmdVenta = new SqlCommand(strVenta,Conex);
								SqlDataReader rdVenta = cmdVenta.ExecuteReader();
								rdVenta.Read();
								Int32 Atencion = rdVenta.GetInt32(0);

								Int16 IdTransaccion = 45;
								Int16 IdProductoTipo = rdVenta.GetInt16(1);
								Int32 IdResponsable = rdVenta.GetInt32(2);
								Int16 IdUbicacionConsumo = rdVenta.GetInt16(3);
								Int16 IdUbicacionEntrega = rdVenta.GetInt16(4);
								Int32 IdMovimientoBase = Int32.Parse(idMovimientoCancelar);
								Int32 IdProcPrincipal = rdVenta.GetInt32(5);
								Int32 IdCausal = Int32.Parse(codigoIdCausal); // Viene de labcore
								string DocRespaldo = rdVenta.GetString(6);
								Int32 IdTarifa = rdVenta.GetInt32(7);
								Int32 IdServicio = rdVenta.GetInt32(8);
								DateTime FecMovimiento = DateTime.Now;
								bool IndFacturado = rdVenta.GetBoolean(9);
								bool IndDevuelto = rdVenta.GetBoolean(10);
								bool IndCompleto = rdVenta.GetBoolean(11);
								bool IndHabilitado = rdVenta.GetBoolean(12);
								Int32 IdUsuarioR = rdVenta.GetInt32(2);
								DateTime FecRegistro = DateTime.Now;
								bool IndPaquete = rdVenta.GetBoolean(13);
								string Cama = rdVenta.GetString(14);
								Int32 SecTran = rdVenta.GetInt32(15);
								Int32 SecTranB = 0;
								Int32 IdMovimientoPral = 0;
								Int32 IdDestinoIni = 0;
								if (!rdVenta.IsDBNull(16)) { SecTranB = rdVenta.GetInt32(16); }
								if (!rdVenta.IsDBNull(17)) { IdMovimientoPral = rdVenta.GetInt32(17); }
								if (!rdVenta.IsDBNull(18)) {IdDestinoIni = rdVenta.GetInt32(18); }
								string TipoVenta = rdVenta.GetString(19);             //       1           2           3        4       5         6         7               8            9               10                  11               12                 13          14         15       16        17           18            19           20          21           22          23          24         25    26    27       28          29             30         31
								string strInsertarCancela = "INSERT INTO facMovimiento (IdMovimiento,NumMovimiento,CodEsor,CodEnti,IdModulo,IdDestino,IdTransaccion,IdProductoTipo,IdResponsable,IdUbicacionConsumo,IdUbicacionEntrega,IdMovimientoBase,IdProcPrincipal,IdCausal,DocRespaldo,IdTarifa,IdServicio,FecMovimiento,IndFacturado,IndDevuelto,IndCompleto,IndHabilitado,IdUsuarioR,FecRegistro,IndPaquete,Cama,SecTran,SecTranB,IdMovimientoPral,IdDestinoIni,TipoVenta) ";
								//                                                             1                      2                3         4         5            6                   7                    8                    9                         10                         11                       12                        13                   14                15                16                17                     18                    19                     20                    21                     22                   23                 24                 25               26          27         28               29                30                31
								strInsertarCancela = strInsertarCancela + "VALUES(@idMovimientoCan,@NumMovimientoCan," + 1 + "," + 1 + "," + 5 + ",@atencion,@IdTransaccion,@IdProductoTipo ,@IdResponsable,@IdUbicacionConsumo,@IdUbicacionEntrega,@IdMovimientoBase,@IdProcPrincipal,@IdCausal,@DocRespaldo,@IdTarifa,@IdServicio,@FecMovimiento,@IndFacturado,@IndDevuelto,@IndCompleto,@IndHabilitado,@IdUsuarioR,@FecRegistro,@IndPaquete,@Cama,@SecTran,@SecTranB,@IdMovimientoPral,@IdDestinoIni,@TipoVenta)";
								SqlCommand cmdInsertarCancela = new SqlCommand(strInsertarCancela, Conex);
								cmdInsertarCancela.Parameters.AddWithValue("@idMovimientoCan", idMovimientoCan);
								cmdInsertarCancela.Parameters.AddWithValue("@NumMovimientoCan", NumMovimientoCan);
								cmdInsertarCancela.Parameters.AddWithValue("@Atencion", Atencion);
								cmdInsertarCancela.Parameters.AddWithValue("@IdTransaccion", IdTransaccion);
								cmdInsertarCancela.Parameters.AddWithValue("@IdProductoTipo", IdProductoTipo);
								cmdInsertarCancela.Parameters.AddWithValue("@IdResponsable", IdResponsable);
								cmdInsertarCancela.Parameters.AddWithValue("@IdUbicacionConsumo", IdUbicacionConsumo);
								cmdInsertarCancela.Parameters.AddWithValue("@IdUbicacionEntrega", IdUbicacionEntrega);
								cmdInsertarCancela.Parameters.AddWithValue("@IdMovimientoBase", IdMovimientoBase);
								cmdInsertarCancela.Parameters.AddWithValue("@IdProcPrincipal", IdProcPrincipal);
								cmdInsertarCancela.Parameters.AddWithValue("@IdCausal", IdCausal);
								cmdInsertarCancela.Parameters.AddWithValue("@DocRespaldo", DocRespaldo);
								cmdInsertarCancela.Parameters.AddWithValue("@IdTarifa", IdTarifa);
								cmdInsertarCancela.Parameters.AddWithValue("@IdServicio", IdServicio);
								cmdInsertarCancela.Parameters.AddWithValue("@FecMovimiento", FecMovimiento);
								cmdInsertarCancela.Parameters.AddWithValue("@IndFacturado", IndFacturado);
								cmdInsertarCancela.Parameters.AddWithValue("@IndDevuelto", IndDevuelto);
								cmdInsertarCancela.Parameters.AddWithValue("@IndCompleto", IndCompleto);
								cmdInsertarCancela.Parameters.AddWithValue("@IndHabilitado", IndHabilitado);
								cmdInsertarCancela.Parameters.AddWithValue("@IdUsuarioR", IdUsuarioR);
								cmdInsertarCancela.Parameters.AddWithValue("@FecRegistro", FecRegistro);
								cmdInsertarCancela.Parameters.AddWithValue("@IndPaquete", IndPaquete);
								cmdInsertarCancela.Parameters.AddWithValue("@Cama", Cama);
								cmdInsertarCancela.Parameters.AddWithValue("@SecTran", SecTran);
								cmdInsertarCancela.Parameters.AddWithValue("@SecTranB", SecTranB);
								cmdInsertarCancela.Parameters.AddWithValue("@IdMovimientoPral", IdMovimientoPral);
								cmdInsertarCancela.Parameters.AddWithValue("@IdDestinoIni", IdDestinoIni);
								cmdInsertarCancela.Parameters.AddWithValue("@TipoVenta", TipoVenta);

								if(cmdInsertarCancela.ExecuteNonQuery()>0)
								{                               //        0              1           2           3       4          5          6            7             8               9        10            11       12          13           14            15       16            17         18      19       20          21       22       23          24      25
									string strConsDetalleVta = "SELECT IdMovimiento,IdProducto,NomProducto,CodProducto,Cantidad,CantidadFact,ValVenta,ValTarifaBase,NumAutorizacion,IndPorcentaje,IndRecargo,IdRecargo,IndFactTemp,IndEnlace,IndHabilitado,ValCuotaMod,indAtendido,RutaArchivo,idMedico,indPos,IndFacturado,IdFormato,regCUM,idSolicitud,idGrupoImp,indXml FROM facMovimientoDet WHERE IdMovimiento=" + idMovimientoCancelar;
									SqlCommand cmdConsDetalleVta = new SqlCommand(strConsDetalleVta, Conex);
									SqlDataReader readConsDetalleVta = cmdConsDetalleVta.ExecuteReader();
									if(readConsDetalleVta.HasRows)
									{
										readConsDetalleVta.Read();
										Double valorVenta = readConsDetalleVta.GetDouble(6) * (-1);
										string qryInsertarVentaDet = "INSERT INTO facMovimientoDet(IdMovimiento,IdProducto,NomProducto,CodProducto,Cantidad,CantidadFact,ValVenta,ValTarifaBase,NumAutorizacion,IndPorcentaje,IndRecargo,IdRecargo,IndFactTemp,IndEnlace,IndHabilitado,ValCuotaMod,indAtendido,RutaArchivo,idMedico,indPos,IndFacturado,IdFormato,regCUM,idSolicitud,idGrupoImp,indXml)";
										qryInsertarVentaDet = qryInsertarVentaDet + "   VALUES(@idMovimientoCan,@IdProducto,@NomProducto,@CodProducto,@cantidad,@CantidadFact,@Valventa,@ValtarifaBase,@NumAutorizacion,@IndPorcentaje,@IndRecargo,@IdRecargo,@IndFactTemp,@IndEnlace,@IndHabilitado,@ValCuotaMod,@indAtendido,@RutaArchivo,@idMedico,@indPos,@IndFacturado,@IdFormato,@regCUM,@idSolicitud,@idGrupoImp,@indXml)";
										SqlCommand cmdInsertarVentaDet = new SqlCommand(qryInsertarVentaDet, Conex);
										cmdInsertarVentaDet.Parameters.AddWithValue("@idMovimientoCan", idMovimientoCan);
										cmdInsertarVentaDet.Parameters.AddWithValue("@IdProducto",readConsDetalleVta.GetInt32(1) );
										cmdInsertarVentaDet.Parameters.AddWithValue("@NomProducto", readConsDetalleVta.GetString(2));
										cmdInsertarVentaDet.Parameters.AddWithValue("@CodProducto",readConsDetalleVta.GetString(3));
										cmdInsertarVentaDet.Parameters.AddWithValue("@Cantidad",-1 );
										cmdInsertarVentaDet.Parameters.AddWithValue("@CantidadFact", 0);
										cmdInsertarVentaDet.Parameters.AddWithValue("@ValVenta", valorVenta);
										cmdInsertarVentaDet.Parameters.AddWithValue("@ValTarifaBase", 0);
										cmdInsertarVentaDet.Parameters.AddWithValue("@NumAutorizacion"," ");
										cmdInsertarVentaDet.Parameters.AddWithValue("@IndPorcentaje", 0);
										cmdInsertarVentaDet.Parameters.AddWithValue("@IndRecargo", 0);
										cmdInsertarVentaDet.Parameters.AddWithValue("@IdRecargo", 0);
										cmdInsertarVentaDet.Parameters.AddWithValue("@IndFactTemp", 0);
										cmdInsertarVentaDet.Parameters.AddWithValue("@IndEnlace", 0);
										cmdInsertarVentaDet.Parameters.AddWithValue("@IndHabilitado", 0);
										cmdInsertarVentaDet.Parameters.AddWithValue("@ValCuotaMod",1 );
										cmdInsertarVentaDet.Parameters.AddWithValue("@indAtendido", 0);
										cmdInsertarVentaDet.Parameters.AddWithValue("@RutaArchivo", DBNull.Value);
										cmdInsertarVentaDet.Parameters.AddWithValue("@idMedico", DBNull.Value);
										cmdInsertarVentaDet.Parameters.AddWithValue("@indPos", DBNull.Value);
										cmdInsertarVentaDet.Parameters.AddWithValue("@IndFacturado",readConsDetalleVta.GetInt32(20));
										cmdInsertarVentaDet.Parameters.AddWithValue("@IdFormato", 0);
										cmdInsertarVentaDet.Parameters.AddWithValue("@regCUM", DBNull.Value);
										cmdInsertarVentaDet.Parameters.AddWithValue("@idSolicitud", DBNull.Value);
										cmdInsertarVentaDet.Parameters.AddWithValue("@idGrupoImp", DBNull.Value);
										cmdInsertarVentaDet.Parameters.AddWithValue("@indXml", DBNull.Value);
										if (cmdInsertarVentaDet.ExecuteNonQuery() > 0)
										{
											Trazabilidad updateTraza = new Trazabilidad();
											string strInsEncabeza = "INSERT INTO TAT_TRAZA_ENC (ENC_ORDEN,ENC_SOLIC,ENC_ATEN,EVT_ORD,FECHA_EVT,ENC_NOTA) VALUES(@orden,@solicitud,@atencion,@evento,@fecha,@nota)";
											SqlCommand cmdInsEncabeza = new SqlCommand(strInsEncabeza, Conex);
											cmdInsEncabeza.Parameters.Add("@orden", SqlDbType.Int);
											cmdInsEncabeza.Parameters.Add("@solicitud", SqlDbType.Int);
											cmdInsEncabeza.Parameters.Add("@atencion", SqlDbType.Int);
											cmdInsEncabeza.Parameters.Add("@evento", SqlDbType.VarChar);
											cmdInsEncabeza.Parameters.Add("@fecha", SqlDbType.DateTime);
											cmdInsEncabeza.Parameters.Add("@nota", SqlDbType.Int);

											cmdInsEncabeza.Parameters["@orden"].Value = nroOrden;
											cmdInsEncabeza.Parameters["@solicitud"].Value = nroSolicitud;
											cmdInsEncabeza.Parameters["@atencion"].Value = nroAtencion;
											cmdInsEncabeza.Parameters["@evento"].Value = "ORU^CA";
											cmdInsEncabeza.Parameters["@fecha"].Value = DateTime.Now;
											cmdInsEncabeza.Parameters["@nota"].Value = 0;
											if (cmdInsEncabeza.ExecuteNonQuery() > 0)
											{
												string qryActualizaTraza = "INSERT INTO TAT_TRAZA_ORDEN (NRO_ORDEN,NRO_SOLIC,NRO_ATEN,ID_PROD,COD_CUPS,EVT_ORD,FECHA_EVT) VALUES(@orden,@solicitud,@atencion,@producto,@cups,@evento,@fecha)";
												SqlCommand cmdActualizaVtas = new SqlCommand(qryActualizaTraza, Conex);
												cmdActualizaVtas.Parameters.Add("@orden", SqlDbType.Int);
												cmdActualizaVtas.Parameters.Add("@solicitud", SqlDbType.Int);
												cmdActualizaVtas.Parameters.Add("@atencion", SqlDbType.Int);
												cmdActualizaVtas.Parameters.Add("@producto", SqlDbType.Int);
												cmdActualizaVtas.Parameters.Add("@cups", SqlDbType.VarChar);
												cmdActualizaVtas.Parameters.Add("@evento", SqlDbType.VarChar);
												cmdActualizaVtas.Parameters.Add("@fecha", SqlDbType.DateTime);

												cmdActualizaVtas.Parameters["@orden"].Value = nroOrden;
												cmdActualizaVtas.Parameters["@solicitud"].Value = nroSolicitud;
												cmdActualizaVtas.Parameters["@atencion"].Value = nroAtencion;
												cmdActualizaVtas.Parameters["@producto"].Value = readConsDetalleVta.GetInt32(1);
												cmdActualizaVtas.Parameters["@cups"].Value = readConsDetalleVta.GetString(3);
												cmdActualizaVtas.Parameters["@evento"].Value = "ORU^CA";
												cmdActualizaVtas.Parameters["@fecha"].Value = DateTime.Now;
												cmdActualizaVtas.ExecuteNonQuery();
												if (!updateTraza.insertarTraza(nroAtencion, nroOrden, nroSolicitud, readConsDetalleVta.GetString(3), "ORM^CA", DateTime.Now,0))
												{
													logLabcore.Warn("No se actualizo la Trazabilidad Para La Cancelacion-1:Atencion:" + nroAtencion + " Orden:" + nroOrden + " Solicitud:" + nroSolicitud + " Producto:" + readConsDetalleVta.GetString(3));
												}

											}
											string strGuardarVenta = "INSERT INTO TAT_VENT_APLI (XPROC_ATEN,XPROC_SOLIC,XPROC_CODPRO,XPROC_IDMOV,XPROC_VALOR) VALUES(" + Atencion + "," + nroSolicitud + ",'" + readConsDetalleVta.GetInt32(1) + "'," + idMovimientoCan + "," + valorVenta*(-1) + ")";
											SqlCommand cmdGuardarVenta = new SqlCommand(strGuardarVenta, Conex);
											cmdGuardarVenta.ExecuteNonQuery();
											string textoMensaje = ackVenta(msg.objMSH, nroSolicitud, NumMovimientoCan);
											srLabcoreTAT.WSSolicitudesClient cliente = new srLabcoreTAT.WSSolicitudesClient();
											string rpta = cliente.GetHL7Msg(textoMensaje);
											logLabcore.Info("Mensaje Cancelacion Enviado:" + textoMensaje);
											logLabcore.Info("Respuesta a la Cancelacion de Labcore:" + rpta);

										}
										else
										{
											respuesta = ackCancelarVenta(msg.objMSH, nroSolicitud, "010", idMovimientoCan);
										}
									}

								}

								respuesta=ackCancelarVenta(msg.objMSH, nroSolicitud, "000", idMovimientoCan);
							}
							else
							{
								respuesta = ackCancelarVenta(msg.objMSH, nroSolicitud, "001", idMovimientoCan);
								logLabcore.Info("Movimiento de Venta Ya fue Reversado:"+respuesta);
								// Ack para este caso:Movimiento Ya fue Reversado
							}
						}
						else
						{
							respuesta = ackCancelarVenta(msg.objMSH, nroSolicitud, "002", idMovimientoCan);
							logLabcore.Info("Movimiento de Venta Ya fue Facturado:" + respuesta);
							// Ack para este caso:Movimiento Ya fue Facturado
						}

					}
					else
					{
						// Ack para este caso:Fcatura Anulada
						respuesta = ackCancelarVenta(msg.objMSH, nroSolicitud, "003", idMovimientoCan);
						logLabcore.Info("Movimiento de Venta Ya fue Facturado:" + respuesta);
					}
				}
				else
				{
					respuesta = ackCancelarVenta(msg.objMSH, nroSolicitud, "003", idMovimientoCan);
				}

			}

			return respuesta;
		}

		string ackCancelarVenta(MSHClass objMSHRecibido, string nroSolicitud, string codigo,Int64 idMovimientoCan)
		{
			MSHClass objMSH = new MSHClass();
			objMSH.Msh7 = utilLocal.fechaHL7(DateTime.Now);
			objMSH.Msh9 = "ORR^O02";
			objMSH.Msh10 = consecutivoMSG();
			objMSH.Msh11 = "P";
			objMSH.Msh12 = "2.3";
			MSAClass objMSA = new MSAClass();

			objMSA.MSA4 = objMSHRecibido.Msh10;
			if (idMovimientoCan > 0)
			{
				objMSA.MSA2 = "AA";
				objMSA.MSA3 = idMovimientoCan.ToString();
			}
			else if(codigo.Equals("001"))
			{
				objMSA.MSA2 = "AR";
				objMSA.MSA3 = "001";
			}
			else if (codigo.Equals("002"))
			{
				objMSA.MSA2 = "AR";
				objMSA.MSA3 = "001";
			}
			else if (codigo.Equals("003"))
			{
				objMSA.MSA2 = "AR";
				objMSA.MSA3 = "003";
			}
			else if (codigo.Equals("004"))
			{
				objMSA.MSA2 = "AR";
				objMSA.MSA3 = "004";
			}
			return objMSH.retornoMSH() + "\n" + objMSA.retornoMSA();

		}

		//////string procesarPendiente(MensajeHL7 msg)
		//////{
		//////    string respuesta = string.Empty;
		//////    SqlConnection Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX);
		//////    string nroAtencion = string.Empty;
		//////    string nroSolicitud = string.Empty;
		//////    string nroOrden = string.Empty;
		//////    //var valorVenta=0;
		//////    using (Conex)
		//////    {
		//////        Conex.Open();
		//////        string strConsultarAtn = "SELECT NRO_ATEN,NRO_SOLIC,NRO_ORDEN FROM TAT_ENC_SOLSAHI WHERE NRO_SOLIC=" + msg.objORC.ORC4;
		//////        SqlCommand cmdConsultaAtn = new SqlCommand(strConsultarAtn, Conex);
		//////        SqlDataReader conCursor = cmdConsultaAtn.ExecuteReader();
		//////        conCursor.Read();
		//////        nroAtencion = conCursor.GetInt32(0).ToString();
		//////        nroSolicitud = conCursor.GetInt32(1).ToString();
		//////        nroOrden = conCursor.GetInt32(2).ToString();
		//////    }
		//////    // **********************************************************
		//////    foreach (string[] segmentoOBR in msg.segmentosOBR)
		//////    {
		//////        OBRClass objOBR = new OBRClass(segmentoOBR);
		//////        string[] producto = objOBR.OBR4.Split('^');  //******************************************** este debe ser el campo  4----- Se coloca temporalmente en 5 para pruebas
		//////        string Cups = producto[0];
		//////        //float idMovimientoVta = ventaProcedimiento(Cups, nroAtencion, nroSolicitud, msg.objMSH,nroOrden);
		//////        Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX);
		//////        using (Conex)
		//////        {
		//////            Conex.Open();
		//////            string qryProducto = "SELECT IdProducto FROM proProducto WHERE CodProducto='" + Cups + "'";
		//////            SqlCommand cmdProducto = new SqlCommand(qryProducto, Conex);
		//////            SqlDataReader readerProducto = cmdProducto.ExecuteReader();
		//////            readerProducto.Read();
		//////            Int32 idProducto = readerProducto.GetInt32(0);
		//////            string strInsEncabeza = "INSERT INTO TAT_TRAZA_ENC (ENC_ORDEN,ENC_SOLIC,ENC_ATEN,EVT_ORD,FECHA_EVT,ENC_NOTA) VALUES(" + nroOrden + "," + nroSolicitud + "," + nroAtencion + ",'ORM^PE','" + DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss") + "'," + 0 + ")";
		//////            SqlCommand cmdInsEncabeza = new SqlCommand(strInsEncabeza, Conex);
		//////            if (cmdInsEncabeza.ExecuteNonQuery() > 0)
		//////            {
		//////                string qryInsertar = "INSERT INTO TAT_TRAZA_ORDEN (NRO_ORDEN,NRO_SOLIC,NRO_ATEN,ID_PROD,COD_CUPS,EVT_ORD,FECHA_EVT) VALUES(" + nroOrden + "," + nroSolicitud + "," + nroAtencion + "," + idProducto + "," + Cups + ",'ORM^PE','" + DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss") + "')";
		//////                SqlCommand cmdInsertar = new SqlCommand(qryInsertar, Conex);
		//////                if (cmdInsertar.ExecuteNonQuery() > 0)
		//////                {
		//////                    respuesta = ackRecibidos(msg.objMSH, true);
		//////                }
		//////                else
		//////                {
		//////                    respuesta = ackRecibidos(msg.objMSH, false);
		//////                }
		//////            }
		//////        }
		//////    }
		//////    return respuesta;
		//////}

		public string cancelarLaboratorio(MensajeHL7 msg)
		{
			SqlConnection Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX);
			string nroAtencion = string.Empty;
			string nroSolicitud = string.Empty;
			string nroOrden = string.Empty;
			Int32 idMovimientoCan = 0;
			Int32 idProducto = 0;
			//var valorVenta=0;
			string[] segmentoOBR = msg.segmentosOBR[0];
			OBRClass objOBR = new OBRClass(segmentoOBR);
			string[] producto = objOBR.OBR4.Split('^');  //******************************************** este debe ser el campo  4----- Se coloca temporalmente en 5 para pruebas
			string Cups = producto[0];
			using (Conex)
			{
				Conex.Open();
				string strConsultarAtn = "SELECT NRO_ATEN,NRO_SOLIC,NRO_ORDEN FROM TAT_ENC_SOLSAHI WHERE NRO_SOLIC=" + msg.objORC.ORC4;
				SqlCommand cmdConsultaAtn = new SqlCommand(strConsultarAtn, Conex);
				SqlDataReader conCursor = cmdConsultaAtn.ExecuteReader();
				conCursor.Read();
				nroAtencion = conCursor.GetInt32(0).ToString();
				nroSolicitud = conCursor.GetInt32(1).ToString();
				nroOrden = conCursor.GetInt32(2).ToString();

				string qryProducto = "SELECT IdProducto FROM proProducto WHERE CodProducto='" + Cups + "'";
				SqlCommand cmdProducto = new SqlCommand(qryProducto, Conex);
				SqlDataReader readerProducto = cmdProducto.ExecuteReader();
				readerProducto.Read();
				idProducto = readerProducto.GetInt32(0);
			}
			//string idMovimientoCancelar = msg.objORC.ORC7;     // ****************** Ojo..... Verificar Datos Recibidos de Labcore.
			string codigoIdCausal = msg.objORC.ORC17;
			using (Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX))
			{
				Trazabilidad updateTraza = new Trazabilidad();
				Conex.Open();
				string strInsEncabeza = "INSERT INTO TAT_TRAZA_ENC (ENC_ORDEN,ENC_SOLIC,ENC_ATEN,EVT_ORD,FECHA_EVT,ENC_NOTA) VALUES(@orden,@solicitud,@atencion,@evento,@fecha,@nota)";
				SqlCommand cmdInsEncabeza = new SqlCommand(strInsEncabeza, Conex);
				cmdInsEncabeza.Parameters.Add("@orden", SqlDbType.Int);
				cmdInsEncabeza.Parameters.Add("@solicitud", SqlDbType.Int);
				cmdInsEncabeza.Parameters.Add("@atencion", SqlDbType.Int);
				cmdInsEncabeza.Parameters.Add("@evento", SqlDbType.VarChar);
				cmdInsEncabeza.Parameters.Add("@fecha", SqlDbType.DateTime);
				cmdInsEncabeza.Parameters.Add("@nota", SqlDbType.Int);

				cmdInsEncabeza.Parameters["@orden"].Value = nroOrden;
				cmdInsEncabeza.Parameters["@solicitud"].Value = nroSolicitud;
				cmdInsEncabeza.Parameters["@atencion"].Value = nroAtencion;
				cmdInsEncabeza.Parameters["@evento"].Value = "ORM^CA";
				cmdInsEncabeza.Parameters["@fecha"].Value = DateTime.Now;
				cmdInsEncabeza.Parameters["@nota"].Value = 0;
				if (cmdInsEncabeza.ExecuteNonQuery() > 0)
				{
					string strInsertarTraza = "INSERT INTO TAT_TRAZA_ORDEN (NRO_ORDEN,NRO_SOLIC,NRO_ATEN,ID_PROD,COD_CUPS,EVT_ORD,FECHA_EVT) VALUES(@orden,@solicitud,@atencion,@producto,@cups,@evento,@fecha)";
					SqlCommand cmdInsertarTraza = new SqlCommand(strInsertarTraza, Conex);
					cmdInsertarTraza.Parameters.Add("@orden", SqlDbType.Int);
					cmdInsertarTraza.Parameters.Add("@solicitud", SqlDbType.Int);
					cmdInsertarTraza.Parameters.Add("@atencion", SqlDbType.Int);
					cmdInsertarTraza.Parameters.Add("@producto", SqlDbType.Int);
					cmdInsertarTraza.Parameters.Add("@cups", SqlDbType.VarChar);
					cmdInsertarTraza.Parameters.Add("@evento", SqlDbType.VarChar);
					cmdInsertarTraza.Parameters.Add("@fecha", SqlDbType.DateTime);

					cmdInsertarTraza.Parameters["@orden"].Value = nroOrden;
					cmdInsertarTraza.Parameters["@solicitud"].Value = nroSolicitud;
					cmdInsertarTraza.Parameters["@atencion"].Value = nroAtencion;
					cmdInsertarTraza.Parameters["@producto"].Value = idProducto;
					cmdInsertarTraza.Parameters["@cups"].Value = Cups;
					cmdInsertarTraza.Parameters["@evento"].Value = "ORM^CA";
					cmdInsertarTraza.Parameters["@fecha"].Value = DateTime.Now;
					if (cmdInsertarTraza.ExecuteNonQuery()>0)
					{
						string textoMensaje = ackCancelarVenta(msg.objMSH, nroSolicitud, "004", idMovimientoCan);
						srLabcoreTAT.WSSolicitudesClient cliente = new srLabcoreTAT.WSSolicitudesClient();
						string rpta = cliente.GetHL7Msg(textoMensaje);
						if (!updateTraza.insertarTraza(nroAtencion, nroOrden, nroSolicitud, Cups, "ORM^CA", DateTime.Now,0))
						{
							logLabcore.Warn("No se actualizo la Trazabilidad en Cancelacion-1:Atencion:" + nroAtencion + " Orden:" + nroOrden + " Solicitud:" + nroSolicitud + " Producto:" + Cups + " Evento:ORM^CA");
						}
						logLabcore.Info("Mensaje Cancelacion Laboratorio Enviado:" + textoMensaje);
						logLabcore.Info("Respuesta la Cancelacion Laboratorio de Labcore:" + rpta);
						return rpta;
					}
					else
					{
						string textoMensaje = ackRecibidos(msg.objMSH, false);
						srLabcoreTAT.WSSolicitudesClient cliente = new srLabcoreTAT.WSSolicitudesClient();
						string rpta = cliente.GetHL7Msg(textoMensaje);
						logLabcore.Info("Mensaje Cancelacion Laboratorio Enviado:" + textoMensaje);
						logLabcore.Info("Respuesta la Cancelacion Laboratorio de Labcore:" + rpta);
						return rpta;
					}
				}
				else
				{
					string textoMensaje = ackRecibidos(msg.objMSH, false);
					srLabcoreTAT.WSSolicitudesClient cliente = new srLabcoreTAT.WSSolicitudesClient();
					string rpta = cliente.GetHL7Msg(textoMensaje);
					logLabcore.Info("Mensaje Cancelacion Laboratorio Enviado:" + textoMensaje);
					logLabcore.Info("Respuesta la Cancelacion Laboratorio de Labcore:" + rpta);
					return rpta;
				}
			}
		}

		public class regsTarifaCon
		{
			Int32 idTarifaCondicion=0;
			Int16 camposNulos=0;
			float valRelacion=0;
			Byte tipoRelacion=0;
			int contrato=0;
			Int16 tarifaRef=0;
			int plan=0;
			int grupo=0;
			int producto=0;
			int idAtencion=0;

			public Int32 IDTarifaCondicion
			{
				get { return this.idTarifaCondicion; }
				set { this.idTarifaCondicion = value; }
			}

			public Int16 CamposNulos
			{
				get { return this.camposNulos; }
				set { this.camposNulos = value; }
			}

			public float ValRelacion
			{
				get { return this.valRelacion; }
				set { this.valRelacion = value; }
			}
			public Byte TipoRelacion
				{
					get { return this.tipoRelacion; }
					set { this.tipoRelacion = value; }
				}

			public int Contrato
				{
				  get { return this.contrato; }
				  set { this.contrato = value; }
				}
			public Int16 TarifaRef
				{
					get{return this.tarifaRef;}
					set { this.tarifaRef = value; }
				}

			public int Plan
			  {
				  get { return this.plan; }
				  set { this.plan = value; }
			  }

			public int Grupo
			{
				get{return this.grupo;}
				set { this.grupo = value; }
			}

			public int Producto
			{
				get { return this.producto; }
				set { this.producto = value; }
			}

			public int Atencion
			{
				get{return this.idAtencion;}
				set { this.idAtencion = value; }
			}
		}//*******fin de clase

		private Int64 NumeroVenta()
		{
			Int64 numeroVenta = 0;
			using (SqlConnection conexion = new SqlConnection(Properties.Settings.Default.DBConexionXX))
			{
				conexion.Open();
				SqlCommand sentencia = new SqlCommand("spFACConsecutivoVentas", conexion);
				sentencia.CommandType = CommandType.StoredProcedure;
				SqlParameter consecutivo = new SqlParameter("@siguiente", SqlDbType.BigInt);
				consecutivo.Direction = ParameterDirection.Output;
				sentencia.Parameters.Add(consecutivo);
				sentencia.Parameters.Add("@numTabla", SqlDbType.SmallInt);
				sentencia.Parameters["@numTabla"].Value = 200;

				int retorno = sentencia.ExecuteNonQuery();

				if (retorno > 0)
				{
					numeroVenta = Convert.ToInt64(sentencia.Parameters["@siguiente"].Value);
				}

				else
				{
					numeroVenta = 0;
				}
			}
			return numeroVenta;
		}

		void IDisposable.Dispose()
		{
			throw new NotImplementedException();
		}

		public float venderProducto(string Atencion,string cupProducto)
		{
			//string Atencion = df_Atentcion.Text;
			string tarifaRela = string.Empty;
			Int16 idTarifaOriginal = 0;
			int idTarifaRProd = 0;
			float factorTarifa = 0.0F;
			int idProductoObj = 0;
			float precioPropio = 0f;
			Int32 idContrato = 0;
			Int32 idPlan = 0;
			Int16 idGrupo = 0;
			Int16 idProductoTipo = 0;

			precios precio = new precios();
			List<precios> resultadoPrecios = new List<precios>();
			using (SqlConnection conexion = new SqlConnection(Properties.Settings.Default.DBConexionXX))
			{
				conexion.Open();
				string qryDatosAtnCon = "SELECT IdContrato,IdPlan,idTarifa,OrdPrioridad,IdAtenContrato FROM admAtencionContrato WHERE IdAtencion=" + Atencion + " AND IndHabilitado=1 AND ordprioridad=1";
				SqlCommand sqlDtaAtnCon = new SqlCommand(qryDatosAtnCon, conexion);
				SqlDataReader readerDtaAtnCon = sqlDtaAtnCon.ExecuteReader();
				if (readerDtaAtnCon.HasRows)
				{
					readerDtaAtnCon.Read();
					idContrato = readerDtaAtnCon.GetInt32(0);
					idPlan = readerDtaAtnCon.GetInt32(1);
					idTarifaOriginal = short.Parse(readerDtaAtnCon.GetInt32(2).ToString());//idTarifaOriginal del producto
				}
				string qryDatosProd = "SELECT IdProducto,IdProductoTipo,IdGrupo,NomProducto FROM proProducto where CodProducto='" + cupProducto + "'";
				SqlCommand cmdDatosProd = new SqlCommand(qryDatosProd, conexion);
				SqlDataReader readerDatosProd = cmdDatosProd.ExecuteReader();
				if (readerDatosProd.HasRows)
				{
					readerDatosProd.Read();
					idProductoObj = readerDatosProd.GetInt32(0); //idProducto
					idProductoTipo = readerDatosProd.GetInt16(1);
					idGrupo = readerDatosProd.GetInt16(2);
				}
				string qryTarifa = "SELECT idTarifaRel,Valrelacion FROM conTarifa WHERE IdTarifa=" + idTarifaOriginal + " AND indHabilitado=1";
				SqlCommand cmdTarifa = new SqlCommand(qryTarifa, conexion);
				SqlDataReader readerTarifa = cmdTarifa.ExecuteReader();
				if (readerTarifa.HasRows)
				{
					readerTarifa.Read();
					if (readerTarifa.IsDBNull(0))
					{
						idTarifaRProd = idTarifaOriginal;
						factorTarifa = 1;
					}
					else
					{
						idTarifaRProd = readerTarifa.GetInt32(0);
						factorTarifa = float.Parse(readerTarifa.GetDouble(1).ToString());
					}
				}
				//***** Liquida precio propio
				string qryPrecioProp = "select Valor from conTarifaDetalle where IdTarifa=" + idTarifaRProd + " and IdProducto=" + idProductoObj + "";
				SqlCommand cmdPrecioProp = new SqlCommand(qryPrecioProp, conexion);
				SqlDataReader rPrecioProp = cmdPrecioProp.ExecuteReader();
				if (rPrecioProp.HasRows)
				{
					//rPrecioProp.Read();
					//precioPropio = float.Parse(rPrecioProp.GetSqlMoney(0).ToString());
					rPrecioProp.Read();
					precioPropio = float.Parse(rPrecioProp.GetSqlMoney(0).ToString()) * factorTarifa;

				}
				else
				{
					precioPropio = 0.0f;
				}
				precio.tipo = "P";
				precio.valor = precioPropio;
				//*********************
				resultadoPrecios.Add(precio);
				//+++++++++++++++++++++ Liquidar
				string qryconTarifaCond = "SELECT ISNULL(IdTarifaCondicion,0),ISNULL(IdContrato,0),ISNULL(IdTarifaRef,0),ISNULL(IdPlan,0),ISNULL(idGrupo,0),ISNULL(idProductoTipo,0),ISNULL(IdProducto,0),ISNULL(IdAtencion,0),ISNULL(IdTipoRelacion,0),ISNULL(ValRelacion,0) FROM conTarifaCondicion WHERE (IdContrato=" + idContrato + ") AND (IdPlan=" + idPlan + " OR IdPlan is null) AND (IdGrupo=" + idGrupo + " OR  IdGrupo is null) AND(IdProductoTipo=" + idProductoTipo + " OR IdProductoTipo IS NULL)  AND (IdProducto=" + idProductoObj + " or IdProducto IS NULL) AND (idAtencion=" + Atencion + " OR IdAtencion is null) ";
				SqlCommand cmdconTarifaCond = new SqlCommand(qryconTarifaCond, conexion);
				SqlDataReader readerconTarifaCond = cmdconTarifaCond.ExecuteReader();
				List<regsTarifaCon> registros = new List<regsTarifaCon>();
				if (readerconTarifaCond.HasRows)
				{
					Int16 totalNull = 0;
					while (readerconTarifaCond.Read())
					{
						totalNull = 0;
						regsTarifaCon registro = new regsTarifaCon();
						registro.IDTarifaCondicion = readerconTarifaCond.GetInt32(0);
						registro.Contrato = readerconTarifaCond.GetInt32(1);
						registro.TarifaRef = readerconTarifaCond.GetInt16(2);
						registro.Plan = readerconTarifaCond.GetInt32(3);
						registro.Grupo = readerconTarifaCond.GetInt32(4);
						//registro.Producto = readerconTarifaCond.GetInt32(6);
						if (readerconTarifaCond.GetInt32(6) == 0) { registro.Producto = idProductoObj; } else { registro.Producto = readerconTarifaCond.GetInt32(6); }
						registro.Atencion = readerconTarifaCond.GetInt32(7);
						registro.TipoRelacion = readerconTarifaCond.GetByte(8);
						registro.ValRelacion = float.Parse(readerconTarifaCond.GetDouble(9).ToString());

						if (registro.Contrato == 0) { totalNull++; }
						if (registro.TarifaRef == 0) { totalNull++; }
						if (registro.Plan == 0) { totalNull++; }
						if (registro.Grupo == 0) { totalNull++; }
						if (registro.Producto == 0) { totalNull++; }
						if (registro.Atencion == 0) { totalNull++; }
						registro.CamposNulos = totalNull;
						registros.Add(registro);
					}
					int tarifaRef = 0;
					float factorTarifaRef = 0.0F;
					float precioCond = 0f;
					foreach (regsTarifaCon condicionX in registros)
					{
						precio.tipo = "";
						precio.valor = 0;
						if (idContrato == condicionX.Contrato)
						{
							if (condicionX.TarifaRef > 0)
							{
								string qryTarifaRef = "SELECT idTarifaRel,Valrelacion FROM conTarifa WHERE IdTarifa=" + condicionX.TarifaRef + " AND indHabilitado=1";
								SqlCommand cmdTarifaRef = new SqlCommand(qryTarifaRef, conexion);
								SqlDataReader rTarifaRef = cmdTarifaRef.ExecuteReader();
								if (rTarifaRef.HasRows)
								{

									rTarifaRef.Read();
									if (rTarifaRef.IsDBNull(0)) { tarifaRef = 0; } else { tarifaRef = rTarifaRef.GetInt32(0); }
									if (rTarifaRef.IsDBNull(1)) { factorTarifaRef = 0; } else { factorTarifaRef = rTarifaRef.GetInt32(1); }
									//*****
									string qryPrecioCond = "select Valor from conTarifaDetalle where IdTarifa=" + condicionX.TarifaRef + " and IdProducto=" + condicionX.Producto + "";
									SqlCommand cmdPrecioCond = new SqlCommand(qryPrecioCond, conexion);
									SqlDataReader rPrecioCond = cmdPrecioCond.ExecuteReader();
									if (rPrecioCond.HasRows)
									{
										rPrecioCond.Read();
										precioCond = float.Parse(rPrecioCond.GetSqlMoney(0).ToString());
										precio.tipo = "C";
										precio.valor = precioCond;
									}
									else
									{
										precioCond = 0.0f;
										precio.tipo = "C";
									}
								}
								else
								{
									precioCond = 0.0f;
									precio.tipo = "C";
								}
							}
							else if (idContrato == condicionX.Contrato && condicionX.Producto == idProductoObj)
							{
								precio.tipo = "C";
								precio.valor = condicionX.ValRelacion;
							}
						}
						resultadoPrecios.Add(precio);
					}
				}
				precios precioFC;
				precios precioFP;
				precioFC.tipo = "";
				precioFC.valor = 0;
				precioFP.tipo = "";
				precioFP.valor = 0;
				foreach (precios precioFinal in resultadoPrecios)
				{
					if (precioFinal.tipo.Equals("P") && precioFinal.valor > precioFP.valor)
					{
						precioFP = precioFinal;
					}
					else if (precioFinal.tipo.Equals("C") && precioFinal.valor > precioFC.valor)
					{
						precioFC = precioFinal;
					}
				}

				if (precioFC.valor > 0)
				{
					return precioFC.valor;
				}
				else
				{
					return precioFP.valor;
				}

			}
		}

        public Int32 validarVenta(string atencion,string nroSolicitud,string cups)
        {
            Int32 nroVenta = 0;
            Int32 atencionWRK = Int32.Parse(atencion);
            Int32 nroSolicitudWRK = Int32.Parse(nroSolicitud);
            SqlConnection conex = new SqlConnection(Properties.Settings.Default.DBConexion);
            using (conex)
            {
                conex.Open();
                string strValidaVta = "SELECT XPROC_IDMOV FROM TAT_VENT_APLI WHERE XPROC_ATEN=@aten AND XPROC_SOLIC=@solicitud AND XPROC_CODPRO=@producto";
                SqlCommand cmdValidaVta = new SqlCommand(strValidaVta, conex);
                cmdValidaVta.Parameters.Add("@aten", SqlDbType.Int);
                cmdValidaVta.Parameters.Add("@solicitud", SqlDbType.Int);
                cmdValidaVta.Parameters.Add("@producto", SqlDbType.VarChar);
                cmdValidaVta.Parameters["@aten"].Value = atencion;
                cmdValidaVta.Parameters["@solicitud"].Value = nroSolicitud;
                cmdValidaVta.Parameters["@producto"].Value = cups;
                SqlDataReader drValidaVta = cmdValidaVta.ExecuteReader();
                if (drValidaVta.HasRows)
                {
                    drValidaVta.Read();
                    nroVenta = drValidaVta.GetInt32(0);
                }
                else
                {
                    nroVenta=0;
                }
                return nroVenta;
            }
        }
	}

	public struct precios
	{
		public string tipo;
		public float valor;

	}
}
