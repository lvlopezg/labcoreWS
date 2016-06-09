using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Services;
using System.Xml.Serialization;
using NLog;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Remoting.Contexts;

namespace labcoreWS
{
    [WebService(Namespace = "http://husi.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class wsLabcoreSahi : System.Web.Services.WebService
    {
        private static Logger logLabcore = LogManager.GetCurrentClassLogger();
        Utilidades utilLocal = new Utilidades();
        //[SoapDocumentMethod(OneWay = true)]
        [WebMethod]
        public string ordenes(string ordenesInput)
        {
            logLabcore.Info("orden:"+ordenesInput);
           // ordenesInput = "<orden idAtencion=\"4888820\" nroOrden=\"6856586\" fechaOrden=\"2015/12/10 08:54:52\" idUsuario=\"3035\"><producto><id>10714</id><cups>902205</cups><cant>1</cant><obs /></producto><producto><id>11244</id><cups>902210</cups><cant>1</cant><obs /></producto></orden>";
           // ordenesInput = "<orden idAtencion=\"4961117\" nroOrden=\"6971187\" fechaOrden=\"2016/01/27 08:10:48\" idUsuario=\"8964\"><producto><id>11234</id><cups>902212</cups><cant>1</cant><obs /></producto><producto><id>5172</id><cups>904903</cups><cant>1</cant><obs /></producto></orden>";
//         //   ordenesInput = "<orden idAtencion=\"4952546\" nroOrden=\"6958406\" fechaOrden=\"2016/04/06 17:24:19\" idUsuario=\"2193\"><producto><id>10714</id><cups>902205</cups><cant>1</cant><obs>cccccccccccccc</obs></producto><producto><id>11244</id><cups>902210</cups><cant>1</cant><obs>5am</obs></producto></orden>";
            //ordenesInput = "<orden idAtencion=\"4842667\" nroOrden=\"6958408\" fechaOrden=\"2016/04/06 18:33:33\" idUsuario=\"3035\"><producto><id>5655</id><cups>903815</cups><cant>1</cant><obs /></producto><producto><id>5656</id><cups>903816</cups><cant>1</cant><obs /></producto><producto><id>5658</id><cups>903818</cups><cant>1</cant><obs /></producto><producto><id>13174</id><cups>903868</cups><cant>1</cant><obs /></producto></orden>";

            //ordenesInput = "<orden idAtencion=\"5054655\" nroOrden=\"7130225\" fechaOrden=\"2016/04/06 13:35:23\" idUsuario=\"7411\"><producto><id>5653</id><cups>903813</cups><cant>1</cant><obs /></producto><producto><id>10282</id><cups>903825</cups><cant>1</cant><obs /></producto><producto><id>11244</id><cups>902210</cups><cant>1</cant><obs /></producto><producto><id>11928</id><cups>903859</cups><cant>1</cant><obs /></producto><producto><id>12921</id><cups>903864</cups><cant>1</cant><obs /></producto><producto><id>11731</id><cups>903856</cups><cant>1</cant><obs /></producto></orden>";
            //ordenesInput = "<orden idAtencion=\"5012829\" nroOrden=\"7130342\" fechaOrden=\"2016/04/06 14:20:37\" idUsuario=\"9521\"><producto><id>2040</id><cups>902045</cups><cant>1</cant><obs>Mañana 5 am </obs></producto></orden>";
            try
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                System.Xml.XmlDocument docTabla = new System.Xml.XmlDocument();
                doc.LoadXml(ordenesInput);
                XmlSerializer deserializar = new XmlSerializer(typeof(orden));
                StringReader reader = new StringReader(ordenesInput);
                object obj = deserializar.Deserialize(reader);
                orden ordenWrk = (orden)obj;
                ordenProducto[] cupsWrk= ordenWrk.producto;
                ordenProducto[] cupsTablaSync=null;

                SqlConnection Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX);
                using (Conex)
                {
                    Conex.Open();
                    string strConsultar = "SELECT paramXML FROM hceLabCliTATinvocaWS WHERE idOrden=@orden AND idAtencion=@atencion AND idAccion='OP'";
                    SqlCommand cmdConsulta = new SqlCommand(strConsultar, Conex);
                    cmdConsulta.Parameters.AddWithValue("@orden", ordenWrk.nroOrden);
                    cmdConsulta.Parameters.AddWithValue("@atencion", ordenWrk.idAtencion);
                    SqlDataReader conCursor = cmdConsulta.ExecuteReader();
                    if(conCursor.HasRows)
                    {
                        conCursor.Read();
                        docTabla.LoadXml(conCursor.GetString(0));
                        XmlSerializer deserOrdTabla = new XmlSerializer(typeof(orden));
                        StringReader lector = new StringReader(conCursor.GetString(0));
                        object objII = deserOrdTabla.Deserialize(lector);
                        orden ordenTablaSync = (orden)objII;
                        cupsTablaSync=ordenTablaSync.producto;
                    }
                    else
                    {
                        logLabcore.Warn("03:No hay Datos de Sincronizacion Para la Orden En Proceso:" + ordenWrk.nroOrden);
                        return "03:No hay Datos de Sincronizacion";
                    }
                    conCursor.Close();
                    conCursor.Dispose();
                    string strInsertar="INSERT INTO TAT_ENC_ORDSAHI (NRO_ORDEN,NRO_ATEN,USR_ORDEN,FECHA_ORD) VALUES(@orden,@atencion,@usuario,@fecha)";
                    SqlTransaction TX1;
                    TX1 = Conex.BeginTransaction("tr1");
                    SqlCommand cmdInsertar = new SqlCommand(strInsertar,Conex,TX1);
                    cmdInsertar.Parameters.Add("@orden", SqlDbType.Int);
                    cmdInsertar.Parameters.Add("@atencion", SqlDbType.Int);
                    cmdInsertar.Parameters.Add("@usuario", SqlDbType.Int);
                    cmdInsertar.Parameters.Add("@fecha", SqlDbType.DateTime);
                    cmdInsertar.Parameters["@orden"].Value = ordenWrk.nroOrden;
                    cmdInsertar.Parameters["@atencion"].Value = ordenWrk.idAtencion;
                    cmdInsertar.Parameters["@usuario"].Value = ordenWrk.idUsuario;
                    cmdInsertar.Parameters["@fecha"].Value = ordenWrk.fechaOrden;

                    SqlCommand cmdInsertardDetalle = new SqlCommand();
                    SqlCommand cmdBorrar = new SqlCommand();
                    SqlCommand cmdInsTraza = new SqlCommand();
                    cmdInsTraza.Parameters.Add("@orden", SqlDbType.Int);
                    cmdInsTraza.Parameters.Add("@atencion", SqlDbType.Int);
                    cmdInsTraza.Parameters.Add("@idProd", SqlDbType.Int);
                    cmdInsTraza.Parameters.Add("@cups", SqlDbType.VarChar);
                    cmdInsTraza.Parameters.Add("@fecha", SqlDbType.DateTime);
                    
                    SqlCommand cmdTraza = new SqlCommand();
                    cmdTraza.Parameters.Add("@atencion", SqlDbType.Int);
                    cmdTraza.Parameters.Add("@orden", SqlDbType.Int);
                    cmdTraza.Parameters.Add("@solicitud", SqlDbType.Int);
                    cmdTraza.Parameters.Add("@cups", SqlDbType.VarChar);
                    cmdTraza.Parameters.Add("@fechaEvento", SqlDbType.DateTime);

                    if (cmdInsertar.ExecuteNonQuery() > 0 && cupsTablaSync.Length == cupsWrk.Length)
                    {
                        try
                        {
                            foreach (ordenProducto cups in cupsWrk)
                            {
                                cmdInsertardDetalle.CommandText = "INSERT INTO TAT_DET_ORDSAHI (NRO_ORDEN,NRO_ATEN,ID_PROD,COD_CUPS,CANT_EST,OBS_EST) VALUES(" + ordenWrk.nroOrden + "," + ordenWrk.idAtencion + "," + cups.id + ",'" + cups.cups + "'," + cups.cant + ",'" + cups.obs + "')";
                                cmdInsertardDetalle.Connection = Conex;
                                cmdInsertardDetalle.Transaction = TX1;
                                if (cmdInsertardDetalle.ExecuteNonQuery() < 1)
                                {
                                    TX1.Rollback("tr1");
                                    return "";
                                }
                                else
                                {
                                    cmdInsTraza.CommandText = "INSERT INTO TAT_TRAZA_ORDEN (NRO_ORDEN,NRO_ATEN,ID_PROD,COD_CUPS,EVT_ORD,FECHA_EVT,ID_USUA) VALUES (@orden,@atencion,@idProd,@cups,'ORD_MED',@fecha," + ordenWrk.idUsuario+ ")";
                                    cmdInsTraza.Parameters["@orden"].Value = ordenWrk.nroOrden;
                                    cmdInsTraza.Parameters["@atencion"].Value = ordenWrk.idAtencion;
                                    cmdInsTraza.Parameters["@idProd"].Value = cups.id;
                                    cmdInsTraza.Parameters["@cups"].Value = cups.cups;
                                    cmdInsTraza.Parameters["@fecha"].Value = DateTime.Now.ToString();

                                    cmdInsTraza.Connection = Conex;
                                    cmdInsTraza.Transaction = TX1;
                                    if (cmdInsTraza.ExecuteNonQuery() < 0)
                                    {
                                        TX1.Rollback("tr1");
                                        logLabcore.Warn("04:No fue posible Insertar en:TAT_TRAZA_ORDEN para la Orden En Proceso:" + ordenWrk.nroOrden);
                                        return "";
                                    }
                                    else
                                    {
                                        cmdTraza.CommandText = "INSERT INTO TAT_TRAZA_TAT (TAT_ATEN,TAT_ORDEN,TAT_SOLI,TAT_CUPS,EVT_ORD) VALUES (@atencion,@orden,@solicitud,@cups,@fechaEvento)";
                                        cmdTraza.Parameters["@atencion"].Value=ordenWrk.idAtencion;
                                        cmdTraza.Parameters["@orden"].Value = ordenWrk.nroOrden;
                                        cmdTraza.Parameters["@solicitud"].Value = 0;
                                        cmdTraza.Parameters["@cups"].Value = cups.cups;
                                        cmdTraza.Parameters["@fechaEvento"].Value = DateTime.Now.ToString();
                                        cmdTraza.Connection = Conex;
                                        cmdTraza.Transaction=TX1;
                                        if(cmdTraza.ExecuteNonQuery()<0)
                                        {
                                            TX1.Rollback("tr1");
                                            logLabcore.Warn("04:No fue posible Insertar en:TAT_TRAZA_TAT para la orden en proceso:" + ordenWrk.nroOrden);
                                            return "";
                                        }
                                    }
                                }
                            }
                            cmdBorrar.CommandText = "DELETE FROM hceLabCliTATinvocaWS WHERE idAtencion=" + ordenWrk.idAtencion + " AND idOrden=" + ordenWrk.nroOrden + " AND idAccion='OP'";
                            cmdBorrar.Connection = Conex;
                            cmdBorrar.Transaction = TX1;
                            if (cmdBorrar.ExecuteNonQuery() > 0)
                            {
                                TX1.Commit();
                                logLabcore.Info("Orden " + ordenWrk.nroOrden + " en Proceso Recepcion Exitosa en Trazabilidad");
                                return "00:Recepcion Existosa";
                            }
                            else
                            {
                                TX1.Rollback("tr1");
                                logLabcore.Warn("Error Limpiando tabla de sincronizacion");
                                return "03: Error Limpiando tabla de Sincronizacion";
                            }
                        }
                        catch(SqlException sqlEx)
                        {
                            TX1.Rollback("tr1");
                            logLabcore.Warn(sqlEx.Message, "04:Error SQL Recibiendo Orden desde SAHI " + ordenWrk.nroOrden);
                            return "04:" + sqlEx.Message;
                        }
                    }
                    else
                    {
                        logLabcore.Warn("02:Error Guardando Datos Orden");
                        return "02:Error Guardando Datos Orden";
                    }
                }
            }
            catch(System.Xml.XmlException ex1)
            {
                utilLocal.notificaFalla("01:Excepcion XML Recibiendo Orden desde SAHI" + ex1.Message + " En Ordenes SAHI");
                logLabcore.Info(ex1, "01:Excepcion XML Recibiendo Orden desde SAHI " + ordenesInput);
                return "01:Excepcion de peticion XML." + ex1.Message;
            }
            catch(SqlException sqlExp)
            {
                utilLocal.notificaFalla("04:Excepcion SQL Recibiendo Orden desde SAHI" + sqlExp.Message + " En Ordenes SAHI");
                logLabcore.Info(sqlExp, "04:Excepcion SQL Recibiendo Orden desde SAHI " + sqlExp.ToString() + "  Linea Numero::::" + sqlExp.LineNumber + "  " + sqlExp.StackTrace.ToString());
                return "01:Excepcion peticion." + sqlExp.StackTrace.ToString();
            }
        }

        [WebMethod]
        //[SoapDocumentMethod(OneWay = true)]
        public string solicitudes(string solicitudInput)
        {
            logLabcore.Info("Solicitud Recibida de SAHI para Enviar:"+solicitudInput);
            //solicitudInput = "<solicitud idAtencion=\"4888820\" nroSolicitud=\"259475\" fechaSolicitud=\"2015/12/10 08:56:40\" prioridad=\"URGENTE\" nroOrden=\"6856586\" idUsuario=\"3035\"><producto><id>10714</id><cups>902205</cups><cant>1</cant><obs /></producto><producto><id>11244</id><cups>902210</cups><cant>1</cant><obs /></producto></solicitud>";
            try
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                System.Xml.XmlDocument docTabla = new System.Xml.XmlDocument();
                doc.LoadXml(solicitudInput);
                XmlSerializer deserializar = new XmlSerializer(typeof(solicitud));
                StringReader reader = new StringReader(solicitudInput);
                object obj = deserializar.Deserialize(reader);
                solicitud solicitudWrk = (solicitud)obj;   // Solicitud Deserializada
                solicitudProducto[] cupsSolicWrk = solicitudWrk.producto; // CUPS de la solicitud.
                solicitudProducto[] cupsTabSolicSync = null; // CUPS  de la solicitud extraidos de la tabla de sincronia
                SqlConnection Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX);
                #region RegionUsing
                using (Conex)
                {
                    Conex.Open();
                    string strConsultar = "SELECT paramXML FROM hceLabCliTATinvocaWS WHERE idOrden=" + solicitudWrk.nroOrden + " AND idAtencion=" + solicitudWrk.idAtencion + " AND idSolicitud=" + solicitudWrk.nroSolicitud+ " AND idAccion='SP'";
                    SqlCommand cmdConsulta = new SqlCommand(strConsultar, Conex);
                    SqlDataReader conCursor = cmdConsulta.ExecuteReader();
                    if (conCursor.HasRows)
                    {
                        conCursor.Read();
                        docTabla.LoadXml(conCursor.GetString(0));
                        XmlSerializer deserTblSol = new XmlSerializer(typeof(solicitud));
                        StringReader lector = new StringReader(conCursor.GetString(0));
                        object objII = deserTblSol.Deserialize(lector);
                        solicitud solicitudTablaSync = (solicitud)objII;
                        cupsTabSolicSync = solicitudTablaSync.producto; // Asigna los CUPS obtenidos de la Tabla de Sincronia
                    }
                    else
                    {
                        logLabcore.Warn("04:Error: No Hay Datos de Sincronizacion para la solicitud" + solicitudWrk.nroSolicitud);
                        return "03:No hay Datos de Sincronizacion";
                    }
                    conCursor.Close();
                    conCursor.Dispose();
                    string strInsertar = "INSERT INTO TAT_ENC_SOLSAHI (NRO_SOLIC,NRO_ORDEN,NRO_ATEN,USR_SOLIC,PRIO_SOLIC,FECHA_SOL) VALUES(@solicitud,@orden,@atencion,@usuario,@prioridad,@fecha)";
                    SqlTransaction TX1;
                    TX1 = Conex.BeginTransaction("TRx1");
                    SqlCommand cmdInsertar = new SqlCommand(strInsertar, Conex, TX1);
                    cmdInsertar.Parameters.Add("@solicitud", SqlDbType.Int);
                    cmdInsertar.Parameters.Add("@orden", SqlDbType.Int);
                    cmdInsertar.Parameters.Add("@atencion", SqlDbType.Int);
                    cmdInsertar.Parameters.Add("@usuario", SqlDbType.Int);
                    cmdInsertar.Parameters.Add("@prioridad", SqlDbType.VarChar);
                    cmdInsertar.Parameters.Add("@fecha", SqlDbType.DateTime);

                    cmdInsertar.Parameters["@solicitud"].Value=solicitudWrk.nroSolicitud;
                    cmdInsertar.Parameters["@orden"].Value = solicitudWrk.nroOrden;
                    cmdInsertar.Parameters["@atencion"].Value = solicitudWrk.idAtencion;
                    cmdInsertar.Parameters["@usuario"].Value = solicitudWrk.idUsuario;
                    cmdInsertar.Parameters["@prioridad"].Value=solicitudWrk.prioridad;
                    cmdInsertar.Parameters["@fecha"].Value=solicitudWrk.fechaSolicitud;

                    SqlCommand cmdInsertardDetalle = new SqlCommand();


                    SqlCommand cmdBorrar = new SqlCommand();
                    SqlCommand cmdInsTraza = new SqlCommand();
                    cmdInsTraza.Parameters.Add("@orden",SqlDbType.Int);
                    cmdInsTraza.Parameters.Add("@solicitud", SqlDbType.Int);
                    cmdInsTraza.Parameters.Add("@atencion", SqlDbType.Int);
                    cmdInsTraza.Parameters.Add("@idProd", SqlDbType.Int);
                    cmdInsTraza.Parameters.Add("@cups", SqlDbType.VarChar);
                    cmdInsTraza.Parameters.Add("@evento",SqlDbType.VarChar);
                    cmdInsTraza.Parameters.Add("@fecha", SqlDbType.DateTime);
                    if (cmdInsertar.ExecuteNonQuery() > 0 && cupsSolicWrk.Length == cupsTabSolicSync.Length)
                    {
                        try
                        {
                            foreach (solicitudProducto cups in cupsSolicWrk)
                            {
                                cmdInsertardDetalle.CommandText = "INSERT INTO TAT_DET_SOLSAHI (NRO_SOLIC,NRO_ORDEN,NRO_ATEN,ID_PROD,COD_CUPS,CANT_SOLI,OBS_SOLI) VALUES(" + solicitudWrk.nroSolicitud + "," + solicitudWrk.nroOrden + "," + solicitudWrk.idAtencion + "," + cups.id + ",'" + cups.cups + "'," + cups.cant + ",'" + cups.obs + "')";
                                cmdInsertardDetalle.Connection = Conex;
                                cmdInsertardDetalle.Transaction = TX1;
                                if (cmdInsertardDetalle.ExecuteNonQuery() < 1)
                                {
                                    TX1.Rollback("TRx1");
                                    logLabcore.Info("04:Error: Insertando solicitud en TAT_DET_SOLSAHI la Solicitud:" + solicitudWrk.nroSolicitud);
                                    return "";
                                }
                                else
                                {
                                    cmdInsTraza.CommandText = "INSERT INTO TAT_TRAZA_ORDEN (NRO_ORDEN,NRO_SOLIC,NRO_ATEN,ID_PROD,COD_CUPS,EVT_ORD,FECHA_EVT,ID_USUA) VALUES (@orden,@solicitud,@atencion,@idProd,@cups,@evento,@fecha,"+solicitudWrk.idUsuario+")";
                                    cmdInsTraza.Parameters["@orden"].Value = solicitudWrk.nroOrden;
                                    cmdInsTraza.Parameters["@solicitud"].Value = solicitudWrk.nroSolicitud;
                                    cmdInsTraza.Parameters["@atencion"].Value = solicitudWrk.idAtencion;
                                    cmdInsTraza.Parameters["@idProd"].Value=cups.id;
                                    cmdInsTraza.Parameters["@cups"].Value=cups.cups;
                                    cmdInsTraza.Parameters["@evento"].Value="SOL_ENF";
                                    cmdInsTraza.Parameters["@fecha"].Value=DateTime.Now.ToString();
                                    cmdInsTraza.Connection = Conex;
                                    cmdInsTraza.Transaction = TX1;
                                    if (cmdInsTraza.ExecuteNonQuery() < 1)
                                    {
                                        TX1.Rollback("TRx1");
                                        logLabcore.Info("04:Error:Insertando en TAT_TRAZA_ORDEN la Solicitud:" + solicitudWrk.nroSolicitud);
                                        return "";
                                    }
                                    //else
                                    //{
                                    //    TX1.Commit();
                                    //}
                                    //***************** AQUI VA TAT_TRAZA_TAT
                                }
                            } 
                            TX1.Commit();
                            //*********** Inicio de Actualizacion de Tabla de Intercambio
                            SqlTransaction TX2;
                            TX2 = Conex.BeginTransaction("TRx2");
                            try
                            {
                                if (cupsSolicWrk.Length == cupsTabSolicSync.Length)
                                {
                                    try
                                    {
                                        cmdBorrar.CommandText = "DELETE FROM hceLabCliTATinvocaWS WHERE idAtencion=" + solicitudWrk.idAtencion + " AND idOrden=" + solicitudWrk.nroOrden + " AND idAccion='SP' AND idSolicitud=" + solicitudWrk.nroSolicitud;
                                        cmdBorrar.Connection = Conex;
                                        cmdBorrar.Transaction = TX2;
                                        if (cmdBorrar.ExecuteNonQuery() > 0)
                                        {
                                            string strConsNroMsg = "SELECT NRO_MSG FROM TAT_CONS_MSGS WHERE TIPO_MSG='SLAB'";
                                            SqlCommand cmdConsNroMsg = new SqlCommand(strConsNroMsg, Conex, TX2);
                                            SqlDataReader nroMsgReader = cmdConsNroMsg.ExecuteReader();
                                            nroMsgReader.Read();
                                            Int32 nroMsg = nroMsgReader.GetInt32(0);
                                            nroMsg++;
                                            nroMsgReader.Close();
                                            nroMsgReader.Dispose();
                                            string strActualizaConsecut = "UPDATE TAT_CONS_MSGS SET NRO_MSG=" + nroMsg + " WHERE TIPO_MSG='SLAB'";
                                            SqlCommand cmdActualizaConsecut = new SqlCommand(strActualizaConsecut, Conex, TX2);
                                            if (cmdActualizaConsecut.ExecuteNonQuery() > 0)
                                            {
                                                TX2.Commit();
                                                entregaSolicitud(solicitudWrk.nroSolicitud, solicitudWrk.nroOrden, solicitudWrk.idAtencion, nroMsg);
                                                logLabcore.Info("05:Recepcion de Solicitud Exitosa:");
                                                return "00:Recepcion Existosa";
                                            }
                                            else
                                            {
                                                TX2.Rollback("TRx2");
                                                logLabcore.Warn("04:Error:No fue posible entregar la solicitud a Servicio TAT");
                                                return "";
                                            }
                                        }
                                        else
                                        {
                                            TX2.Rollback("TRx2");
                                            logLabcore.Warn("04:Error:Limpiando datos de solicitud en tabla de Sincronizacion");
                                            return "03: Error Limpiando tabla de Sincronizacion";
                                        }
                                    }
                                    catch (SqlException SqlExp)
                                    {
                                        logLabcore.Warn(SqlExp.Message, "04:Excepcion en SQL");
                                        return "";
                                    }
                                }
                                else
                                {
                                    logLabcore.Warn("04:Datos Solicitud Inconsistentes desde SAHI:" + solicitudInput);
                                    return "Datos incosnsistentes";
                                }
                            }
                            catch (SqlException sqlEx)
                            {
                                TX2.Rollback("TRx2");
                                logLabcore.Warn(sqlEx.Message, "04:Excepcion en SQL "+sqlEx.StackTrace);
                                utilLocal.notificaFalla("04:Excepcion en SQL " + sqlEx.Message);
                                return "04:" + sqlEx.Message;
                            }
                            //************ Final de Actualizacion de Tabla de Intercambio
                        }
                        catch (SqlException sqlEx)
                        {
                            TX1.Rollback("TRx1");
                            logLabcore.Warn(sqlEx.Message, "04:Excepcion en SQL "+sqlEx.StackTrace);
                            utilLocal.notificaFalla("04:Excepcion en SQL "+sqlEx.Message);
                            return "04:" + sqlEx.Message;
                        }

                    }
                    else
                    {
                        TX1.Rollback("TRx1");
                        utilLocal.notificaFalla("02:Error Guardando Datos Solicitud en Encabezado y DEtalle de Solicitud");
                        logLabcore.Info("04:Error:Guardando Datos Solicitud:" + solicitudInput);
                        return "02:Error Guardando Datos Solicitud";
                    }
                }
                #endregion//// Using ***************************
               
            }
            catch (System.Xml.XmlException ex1)
            {
                utilLocal.notificaFalla("01:Error XML peticion." + ex1.Message+" En Solicitudes SAHI");
                logLabcore.Warn(ex1, "01:Excepcion en XML" + solicitudInput+"***"+ex1.StackTrace);
                return "01:Error peticion." + ex1.Message;
            }
        }

        private async void entregaSolicitud(string solicitud, string orden, string atencion, Int32 NumeroMsg)
        {
            try
            {
                srProxyOrdenar.IordenarEstudioClient clienteOrdenar = new srProxyOrdenar.IordenarEstudioClient();
                string rpta = await clienteOrdenar.ordenarAsync(solicitud, orden, atencion, NumeroMsg);
                logLabcore.Info("Respuesta Recibida en wsLabcoreSahi.asmx:" + solicitud + " Respuesta:" + rpta);
            }
            catch (EndpointNotFoundException endPointExp)
            {
                String _mensaje = "TAT(EndpointNotFound)(Solicitud:" + solicitud + " Orden:" + orden + " Atn:"+atencion+"):" + endPointExp.Message;
                utilLocal.notificaFalla(_mensaje);
                logLabcore.Warn("EndpointNotFoundException en wsLabcoreSahi.asmx" + endPointExp.StackTrace);
            }
            catch (ServerTooBusyException serverExp)
            {
                String _mensaje = "TAT ServerTooBusy:(Solicitud:" + solicitud + " Orden:" + orden + " Atn:" + atencion + ")" + serverExp.Message;
                utilLocal.notificaFalla(_mensaje);
                logLabcore.Warn("ServerTooBusyException en wsLabcoreSahi.asmx" + serverExp.StackTrace);
            }
            catch (ChannelTerminatedException channelExp)
            {
                String _mensaje = "TAT ChannelTerminated:(Solicitud:" + solicitud + " Orden:" + orden + " Atn:" + atencion + ")" + channelExp.Message;
                utilLocal.notificaFalla(_mensaje);
                logLabcore.Warn("ChannelTerminatedException en wsLabcoreSahi.asmx" + channelExp.StackTrace);
            }
            catch (CommunicationException commExp)
            {
                String _mensaje = "TAT CommunicationException:(Solicitud:" + solicitud + " Orden:" + orden + " Atn:" + atencion + ")" + commExp.Message;
                utilLocal.notificaFalla(_mensaje);
                logLabcore.Warn("CommunicationException en wsLabcoreSahi.asmx" + commExp.StackTrace);
            }

            catch (TimeoutException toExp)
            {
                String _mensaje = "TAT Timeout:(Solicitud:" + solicitud + " Orden:" + orden + " Atn:" + atencion + ")" + toExp.Message;
                utilLocal.notificaFalla(_mensaje);
                logLabcore.Warn("Exception Time Out en wsLabcoreSahi.asmx" + toExp.StackTrace);
            }
            catch(Exception exp)
            {
                String _mensaje = "TAT(Solicitud:" + solicitud + " Orden:" + orden + " Atn:" + atencion + "):" + exp.Message;
                utilLocal.notificaFalla(_mensaje);
                logLabcore.Info("Excepcion desde wsLabcoreSahi.asmx Enviado la Solicitud a Trazabilidad:" + solicitud + " mensaje:" + exp.Message+" "+exp.StackTrace.ToString());
            }
        }

    }
}
