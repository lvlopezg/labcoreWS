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
using System.Web.Services.Protocols;

namespace labcoreWS
{
    /// <summary>
    /// Operaciones para el envio de las Ordenes y Solicitudes desde  SAHI a Labcore.
    /// Esta capa aiende directamente a SAHI. DEsde aqui se consumen servicios SOAP, para la traduccion y envio a LABCOE
    /// </summary>
    [WebService(Namespace = "http://husi.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class wsLabcoreSahi : System.Web.Services.WebService
    {
        private static Logger logLabcore = LogManager.GetCurrentClassLogger();
        //private static Logger logKioscos = LogManager.GetLogger("Kioscos");

        Utilidades utilLocal = new Utilidades();
        //[SoapDocumentMethod(OneWay = true)]
        [WebMethod]
        public string ordenes(string ordenesInput)
        {
            logLabcore.Info("orden:" + ordenesInput);
            //ordenesInput = "<orden idAtencion=\"7585496\" nroOrden=\"11062771\" fechaOrden =\"2021 /07/12 08:11:22\" idUsuario =\"2540\" > <producto><id>70427</id><cups>903880</cups><cant>1</cant><obs>Prioridad:Hospitalario Normal</obs></producto><producto><id>11244</id><cups>902210</cups><cant>1</cant><obs> Prioridad: Hospitalario Prioritario .</obs></producto></orden>";
            try
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                System.Xml.XmlDocument docTabla = new System.Xml.XmlDocument();
                doc.LoadXml(ordenesInput);
                XmlSerializer deserializar = new XmlSerializer(typeof(orden));
                StringReader reader = new StringReader(ordenesInput);
                object obj = deserializar.Deserialize(reader);
                orden ordenWrk = (orden)obj;
                ordenProducto[ ] cupsWrk = ordenWrk.producto;
                ordenProducto[ ] cupsTablaSync = null;
                //SqlConnection Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX);
                using (SqlConnection Con01 = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX))
                {
                    Con01.Open();
                    string strConsultar = "SELECT paramXML FROM hceLabCliTATinvocaWS WHERE idOrden=@orden AND idAtencion=@atencion AND idAccion='OP'";
                    using (SqlCommand cmdConsulta = new SqlCommand(strConsultar, Con01))
                    {
                        cmdConsulta.Parameters.AddWithValue("@orden", ordenWrk.nroOrden);
                        cmdConsulta.Parameters.AddWithValue("@atencion", ordenWrk.idAtencion);
                        SqlDataReader conCursor = cmdConsulta.ExecuteReader();
                        if (conCursor.HasRows)
                        {
                            conCursor.Read();
                            docTabla.LoadXml(conCursor.GetString(0));
                            XmlSerializer deserOrdTabla = new XmlSerializer(typeof(orden));
                            StringReader lector = new StringReader(conCursor.GetString(0));
                            object objII = deserOrdTabla.Deserialize(lector);
                            orden ordenTablaSync = (orden)objII;
                            cupsTablaSync = ordenTablaSync.producto;
                        }
                        else
                        {
                            logLabcore.Warn($"03:No hay Datos de Sincronizacion Para la Orden En Proceso:{ordenWrk.nroOrden}");
                            return "03:No hay Datos de Sincronizacion";
                        }
                        //conCursor.Close();
                        //conCursor.Dispose(); 
                    }
                }
                using (SqlConnection Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBCon))
                {
                    Conex.Open();
                    string strInsertar = "INSERT INTO TAT_ENC_ORDSAHI (NRO_ORDEN,NRO_ATEN,USR_ORDEN,FECHA_ORD) VALUES(@orden,@atencion,@usuario,@fecha)";
                    SqlTransaction TX1 = Conex.BeginTransaction("tr1");
                    SqlCommand cmdInsertar = new SqlCommand(strInsertar, Conex, TX1);
                    cmdInsertar.Parameters.Add("@orden", SqlDbType.Int).Value = ordenWrk.nroOrden;
                    cmdInsertar.Parameters.Add("@atencion", SqlDbType.Int).Value = ordenWrk.idAtencion;
                    cmdInsertar.Parameters.Add("@usuario", SqlDbType.Int).Value = ordenWrk.idUsuario;
                    cmdInsertar.Parameters.Add("@fecha", SqlDbType.DateTime).Value = ordenWrk.fechaOrden;

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

                    SqlCommand cmdInsertardDetalle = new SqlCommand();
                    cmdInsertardDetalle.Parameters.Add("@nroOrden", SqlDbType.Int);//;.Value = Int32.Parse(ordenWrk.nroOrden);
                    cmdInsertardDetalle.Parameters.Add("@idAtencion", SqlDbType.Int);//.Value = Int32.Parse(ordenWrk.idAtencion);
                    cmdInsertardDetalle.Parameters.Add("@idProducto", SqlDbType.Int);//.Value = Int32.Parse(cups.id);
                    cmdInsertardDetalle.Parameters.Add("@CodCups", SqlDbType.VarChar);//.Value = cups.cups;
                    cmdInsertardDetalle.Parameters.Add("@Cantidad", SqlDbType.SmallInt);//.Value = cups.cant;
                    cmdInsertardDetalle.Parameters.Add("@Observaciones", SqlDbType.VarChar);//.Value = cups.obs;
                    if (cmdInsertar.ExecuteNonQuery() > 0 && cupsTablaSync.Length == cupsWrk.Length)
                    {
                        try
                        {
                            foreach (ordenProducto cups in cupsWrk)
                            {
                                cmdInsertardDetalle.CommandText = "INSERT INTO TAT_DET_ORDSAHI (NRO_ORDEN,NRO_ATEN,ID_PROD,COD_CUPS,CANT_EST,OBS_EST) VALUES(@nroOrden,@idAtencion,@idProducto,@CodCups,@Cantidad,@Observaciones)";
                                cmdInsertardDetalle.Parameters["@nroOrden"].Value = Int32.Parse(ordenWrk.nroOrden);
                                cmdInsertardDetalle.Parameters["@idAtencion"].Value = Int32.Parse(ordenWrk.idAtencion);
                                cmdInsertardDetalle.Parameters["@idProducto"].Value = Int32.Parse(cups.id);
                                cmdInsertardDetalle.Parameters["@CodCups"].Value = cups.cups;
                                cmdInsertardDetalle.Parameters["@Cantidad"].Value = cups.cant;
                                cmdInsertardDetalle.Parameters["@Observaciones"].Value = cups.obs;
                                cmdInsertardDetalle.Connection = Conex;
                                cmdInsertardDetalle.Transaction = TX1;
                                if (cmdInsertardDetalle.ExecuteNonQuery() < 1)
                                {
                                    TX1.Rollback("tr1");
                                    return "";
                                }
                                else
                                {
                                    cmdInsTraza.CommandText = "INSERT INTO TAT_TRAZA_ORDEN (NRO_ORDEN,NRO_ATEN,ID_PROD,COD_CUPS,EVT_ORD,FECHA_EVT,ID_USUA) VALUES (@orden,@atencion,@idProd,@cups,'ORD_MED',@fecha," + ordenWrk.idUsuario + ")";
                                    cmdInsTraza.Parameters["@orden"].Value = ordenWrk.nroOrden;
                                    cmdInsTraza.Parameters["@atencion"].Value = ordenWrk.idAtencion;
                                    cmdInsTraza.Parameters["@idProd"].Value = cups.id;
                                    cmdInsTraza.Parameters["@cups"].Value = cups.cups;
                                    cmdInsTraza.Parameters["@fecha"].Value = DateTime.Now.ToString();
                                    cmdInsTraza.Connection = Conex;
                                    cmdInsTraza.Transaction = TX1;
                                    if (cmdInsTraza.ExecuteNonQuery() < 1)
                                    {
                                        TX1.Rollback("tr1");
                                        logLabcore.Warn("04:No fue posible Insertar en:TAT_TRAZA_ORDEN para la Orden En Proceso:" + ordenWrk.nroOrden);
                                        return "";
                                    }
                                    else
                                    {
                                        cmdTraza.CommandText = "INSERT INTO TAT_TRAZA_TAT (TAT_ATEN,TAT_ORDEN,TAT_SOLI,TAT_CUPS,EVT_ORD) VALUES (@atencion,@orden,@solicitud,@cups,@fechaEvento)";
                                        cmdTraza.Parameters["@atencion"].Value = ordenWrk.idAtencion;
                                        cmdTraza.Parameters["@orden"].Value = ordenWrk.nroOrden;
                                        cmdTraza.Parameters["@solicitud"].Value = 0;
                                        cmdTraza.Parameters["@cups"].Value = cups.cups;
                                        cmdTraza.Parameters["@fechaEvento"].Value = DateTime.Parse(ordenWrk.fechaOrden);
                                        cmdTraza.Connection = Conex;
                                        cmdTraza.Transaction = TX1;
                                        if (cmdTraza.ExecuteNonQuery() < 1)
                                        {
                                            TX1.Rollback("tr1");
                                            logLabcore.Warn($"04:No fue posible Insertar en:TAT_TRAZA_TAT para la orden en proceso:{ordenWrk.nroOrden}");
                                            return "";
                                        }
                                        else
                                        {
                                            logLabcore.Warn($"0:Se Inserta en:TAT_TRAZA_TAT para la orden en proceso:{ordenWrk.nroOrden} Estudio:{cups.cups}");
                                        }
                                    }
                                }
                            }

                            logLabcore.Info($"0:Se Han Procesado Todos los Estudios Para la Orden:{ordenWrk.nroOrden}...");
                            string qryActualizaSincroniza = "INSERT INTO hceLabCliTATinvocaWSHist (idAccion,idAtencion,idOrden,idSolicitud,paramXML,fecRegistro) " +
                              "SELECT idAccion, idAtencion, idOrden,0,paramXML,fecRegistro FROM hceLabCliTATinvocaWS " +
                              "WHERE IdAccion = 'OP' AND idAtencion = @idAtencion AND idOrden = @idOrden";
                            SqlCommand cmdActualizaSincroniza = new SqlCommand(qryActualizaSincroniza, Conex);
                            cmdActualizaSincroniza.Parameters.Add("@idAtencion", SqlDbType.Int).Value = Int32.Parse(ordenWrk.idAtencion);
                            cmdActualizaSincroniza.Parameters.Add("@idOrden", SqlDbType.Int).Value = Int32.Parse(ordenWrk.nroOrden);
                            cmdActualizaSincroniza.Transaction = TX1;
                            if (cmdActualizaSincroniza.ExecuteNonQuery() > 0)
                            {
                                logLabcore.Info("***** Se Actualiza Tabla:hceLabCliTATinvocaWSHist de Historicos");
                                cmdBorrar.CommandText = "DELETE FROM hceLabCliTATinvocaWS WHERE idAtencion=" + ordenWrk.idAtencion + " AND idOrden=" + ordenWrk.nroOrden + " AND idAccion='OP'";
                                cmdBorrar.Connection = Conex;
                                cmdBorrar.Transaction = TX1;
                                if (cmdBorrar.ExecuteNonQuery() > 0)
                                {
                                    TX1.Commit();
                                    logLabcore.Info($"Orden: {ordenWrk.nroOrden} en Proceso. Recepcion Y Confirmacion Exitosa en Trazabilidad");
                                    using (SqlConnection ConnCons = new SqlConnection(Properties.Settings.Default.alterno))
                                    {
                                        ConnCons.Open();
                                        string qrySolicitudAut = "SELECT idAccion,idAtencion,idOrden,idSolicitud,paramXML,fecRegistro " +
                                          "FROM hceLabCliTATinvocaWS sa WHERE sa.idAccion='SA' AND sa.idAtencion=@idAtencion AND sa.idOrden=@idOrden1 " +
                                          "ORDER BY sa.idSolicitud";
                                        logLabcore.Info("Buscando Solicitudes (SA) de Urgencias Por Procesar....");
                                        logLabcore.Info($"Query:{qrySolicitudAut}");
                                        SqlCommand cmdSolicitudAut = new SqlCommand(qrySolicitudAut, ConnCons);
                                        cmdSolicitudAut.Parameters.Add("@idAtencion", SqlDbType.Int).Value = Int32.Parse(ordenWrk.idAtencion);
                                        cmdSolicitudAut.Parameters.Add("@idOrden1", SqlDbType.Int).Value = Int32.Parse(ordenWrk.nroOrden);
                                        SqlDataReader rdSolicitudAut = cmdSolicitudAut.ExecuteReader();
                                        if (rdSolicitudAut.HasRows)
                                        {
                                            while (rdSolicitudAut.Read())
                                            {
                                                logLabcore.Info($"Se Encuentra Solicitud (SA) para Procesar:{rdSolicitudAut.GetString(4)}");
                                                string respuestasol=solicitudes(rdSolicitudAut.GetString(4));
                                                logLabcore.Info($"*****Respuesta de Servicio de solictudes:{respuestasol}");
                                                logLabcore.Info($"Se Proceso Solicitud Automatica:idAtencion:{rdSolicitudAut.GetInt32(1)} idOrden:{rdSolicitudAut.GetInt32(2)} idSolicitud:{rdSolicitudAut.GetInt32(1)}");
                                            }
                                        }
                                        else
                                        {
                                            logLabcore.Info($"No se recibe Solicitud Automatica, para la Orden:{ordenWrk.nroOrden} Orden de Procedimiento Originada Diferentea a Urgencias");
                                        }
                                    }
                                    return "00:Recepcion Existosa";
                                }
                                else
                                {
                                    TX1.Rollback("tr1");
                                    logLabcore.Warn("Error Limpiando tabla de sincronizacion. Se Procede a hacer Rollback");
                                    return "03: Error Limpiando tabla de Sincronizacion";
                                }
                            }
                            else
                            {
                                TX1.Rollback("tr1");
                                logLabcore.Warn("****** Error Limpiando tabla de sincronizacion");
                                return "***** 03: Error Actualizando tabla de Sincronizacion de Historicos";
                            }

                        }
                        catch (SqlException sqlEx)
                        {
                            TX1.Rollback("tr1");
                            logLabcore.Warn(sqlEx.Message, "04:Error SQL Recibiendo Orden desde SAHI " + ordenWrk.nroOrden);
                            return "04:" + sqlEx.Message;
                        }
                    }
                    else
                    {
                        logLabcore.Warn("***** 02:Error Guardando Datos Orden");
                        return "02:Error Guardando Datos Orden";
                    }
                }
            }
            catch (System.Xml.XmlException ex1)
            {
                utilLocal.notificaFalla($"01:Excepcion XML Recibiendo Orden desde SAHI:  {ex1.Message}   En Ordenes SAHI");
                logLabcore.Info(ex1, $"***** 01:Excepcion XML Recibiendo Orden desde SAHI:  {ordenesInput}");
                return "01:Excepcion de peticion XML." + ex1.Message;
            }
            catch (SqlException sqlExp)
            {
                utilLocal.notificaFalla("04:Excepcion SQL Recibiendo Orden desde SAHI" + sqlExp.Message + " En Ordenes SAHI");
                logLabcore.Info(sqlExp, "***** 04:Excepcion SQL Recibiendo Orden desde SAHI " + sqlExp.ToString() + "  Linea Numero::::" + sqlExp.LineNumber + "  " + sqlExp.StackTrace.ToString());
                return "01:Excepcion peticion." + sqlExp.StackTrace.ToString();
            }
        }

        /// <summary>
        /// Operacion para el envio de Solictudes. consumido por SAHI 
        /// </summary>
        /// <param name="solicitudInput">XML con la informacion de la solicirud</param>
        /// <returns></returns>

        [WebMethod]
        //[SoapDocumentMethod(OneWay = true)]
        public string solicitudes(string solicitudInput)
        {
            logLabcore.Info($"***** Solicitud de Procedimiento Recibida desde SAHI para Enviar: {solicitudInput}");
            //solicitudInput = "<solicitud idAtencion=\"7585787\" nroSolicitud=\"769306\" fechaSolicitud =\"2021/05/30 17:05:44\" prioridad =\"URGENTE\" nroOrden =\"11062747\" idUsuario =\"2193\" ><producto><id>11244</id><cups>902210</cups><cant>1</cant><obs>Prioridad: Hospitalario Urgente .obs reqUrgente prueba</obs></producto></solicitud>";
            try
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                System.Xml.XmlDocument docTabla = new System.Xml.XmlDocument();
                doc.LoadXml(solicitudInput);
                XmlSerializer deserializar = new XmlSerializer(typeof(solicitud));
                StringReader reader = new StringReader(solicitudInput);
                object obj = deserializar.Deserialize(reader);
                solicitud solicitudWrk = (solicitud)obj;   // Solicitud Deserializada
                solicitudProducto[ ] cupsSolicWrk = solicitudWrk.producto; // CUPS de la solicitud.
                solicitudProducto[ ] cupsTabSolicSync = null; // CUPS  de la solicitud extraidos de la tabla de sincronia

                SqlConnection Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX);
                #region RegionUsing
                using (Conex)
                {
                    //string tipo = "SP";
                    using (SqlConnection conect01 = new SqlConnection(Properties.Settings.Default.alterno))
                    {
                        conect01.Open();
                        string strConsultar = "SELECT paramXML FROM hceLabCliTATinvocaWS WHERE idOrden=@idOrden2 AND idAtencion=@idAtencion AND idSolicitud=@idSolicitud";// AND idAccion=@Tipo";
                        using (SqlCommand cmdConsulta = new SqlCommand(strConsultar, conect01))
                        {
                            //SqlCommand cmdConsulta = new SqlCommand(strConsultar, Conex);
                            //cmdConsulta.Parameters.Add("@Tipo", SqlDbType.VarChar).Value = tipo;
                            cmdConsulta.Parameters.Add("@idOrden2", SqlDbType.Int).Value = Int32.Parse(solicitudWrk.nroOrden);
                            cmdConsulta.Parameters.Add("@idAtencion", SqlDbType.Int).Value = Int32.Parse(solicitudWrk.idAtencion);
                            cmdConsulta.Parameters.Add("@idSolicitud", SqlDbType.Int).Value = Int32.Parse(solicitudWrk.nroSolicitud);
                            SqlDataReader conCursor = cmdConsulta.ExecuteReader();
                            if (conCursor.HasRows)
                            {
                                logLabcore.Info($"***** Se Encuentra el Registro para la Solicitud:{solicitudWrk.nroSolicitud} en tabla de intercambio. *****");
                                conCursor.Read();
                                docTabla.LoadXml(conCursor.GetString(0));
                                XmlSerializer deserTblSol = new XmlSerializer(typeof(solicitud));
                                StringReader lector = new StringReader(conCursor.GetString(0));
                                object objII = deserTblSol.Deserialize(lector);
                                solicitud solicitudTablaSync = (solicitud)objII;// solicitudTablaSync es Solicitud Deserializada
                                cupsTabSolicSync = solicitudTablaSync.producto; // Asigna los CUPS obtenidos de la Tabla de Sincronia
                            }
                            else
                            {
                                logLabcore.Warn($"***** 04:Error: No Hay Datos de Sincronizacion para la solicitud:{solicitudWrk.nroSolicitud} Datos Orden:{solicitudWrk.nroOrden} Atencion:{solicitudWrk.idAtencion}  ");
                                return "03:No hay Datos de Sincronizacion";
                            }
                        }
                    }

                    Conex.Open();
                    string strInsertar = "INSERT INTO TAT_ENC_SOLSAHI (NRO_SOLIC,NRO_ORDEN,NRO_ATEN,USR_SOLIC,PRIO_SOLIC,FECHA_SOL) VALUES(@solicitud,@orden,@atencion,@usuario,@prioridad,@fecha)";
                    SqlTransaction TX1_1;
                    TX1_1 = Conex.BeginTransaction("TRx1_1");

                    SqlCommand cmdInsertar = new SqlCommand(strInsertar, Conex, TX1_1);
                    cmdInsertar.Parameters.Add("@solicitud", SqlDbType.Int).Value = solicitudWrk.nroSolicitud;
                    cmdInsertar.Parameters.Add("@orden", SqlDbType.Int).Value = solicitudWrk.nroOrden;
                    cmdInsertar.Parameters.Add("@atencion", SqlDbType.Int).Value = solicitudWrk.idAtencion;
                    cmdInsertar.Parameters.Add("@usuario", SqlDbType.Int).Value = solicitudWrk.idUsuario;
                    cmdInsertar.Parameters.Add("@prioridad", SqlDbType.VarChar).Value = solicitudWrk.prioridad;
                    cmdInsertar.Parameters.Add("@fecha", SqlDbType.DateTime).Value = solicitudWrk.fechaSolicitud;

                    SqlCommand cmdInsertardDetalle = new SqlCommand();
                    SqlCommand cmdBorrar = new SqlCommand();
                    SqlCommand cmdInsTraza = new SqlCommand();
                    cmdInsTraza.Parameters.Add("@orden", SqlDbType.Int);
                    cmdInsTraza.Parameters.Add("@solicitud", SqlDbType.Int);
                    cmdInsTraza.Parameters.Add("@atencion", SqlDbType.Int);
                    cmdInsTraza.Parameters.Add("@idProd", SqlDbType.Int);
                    cmdInsTraza.Parameters.Add("@cups", SqlDbType.VarChar);
                    cmdInsTraza.Parameters.Add("@evento", SqlDbType.VarChar);
                    cmdInsTraza.Parameters.Add("@fecha", SqlDbType.DateTime);
                    if (cmdInsertar.ExecuteNonQuery() > 0 && cupsSolicWrk.Length == cupsTabSolicSync.Length)
                    {
                        logLabcore.Info("***** Solicitud desde SAHI: Se actualiza Encabezado de Solicitudes: TAT_ENC_SOLSAHI *****");
                        try
                        {
                            List<solicitudProducto> cups_x_Procesar = cupsSolicWrk.ToList<solicitudProducto>();
                            List<solicitudProducto> cups_Procesados = new List<solicitudProducto>();
                            foreach (solicitudProducto cups in cups_x_Procesar)
                            {
                                cmdInsertardDetalle.CommandText = "INSERT INTO TAT_DET_SOLSAHI (NRO_SOLIC,NRO_ORDEN,NRO_ATEN,ID_PROD,COD_CUPS,CANT_SOLI,OBS_SOLI) VALUES(" + solicitudWrk.nroSolicitud + "," + solicitudWrk.nroOrden + "," + solicitudWrk.idAtencion + "," + cups.id + ",'" + cups.cups + "'," + cups.cant + ",'" + cups.obs + "')";
                                cmdInsertardDetalle.Connection = Conex;
                                cmdInsertardDetalle.Transaction = TX1_1;
                                if (cmdInsertardDetalle.ExecuteNonQuery() < 1)
                                {
                                    TX1_1.Rollback("TRx1_1");
                                    logLabcore.Info("04:Error: Insertando solicitud en TAT_DET_SOLSAHI la Solicitud:" + solicitudWrk.nroSolicitud);
                                    return "";
                                }
                                else
                                {
                                    logLabcore.Info("*****366 Se insertó Detalle en Tabla: TAT_DET_SOLSAHI *****");
                                    cmdInsTraza.CommandText = "INSERT INTO TAT_TRAZA_ORDEN (NRO_ORDEN,NRO_SOLIC,NRO_ATEN,ID_PROD,COD_CUPS,EVT_ORD,FECHA_EVT,ID_USUA) " +
                                      "VALUES (@orden,@solicitud,@atencion,@idProd,@cups,@evento,@fecha," + solicitudWrk.idUsuario + ")";
                                    cmdInsTraza.Parameters["@orden"].Value = solicitudWrk.nroOrden;
                                    cmdInsTraza.Parameters["@solicitud"].Value = solicitudWrk.nroSolicitud;
                                    cmdInsTraza.Parameters["@atencion"].Value = solicitudWrk.idAtencion;
                                    cmdInsTraza.Parameters["@idProd"].Value = cups.id;
                                    cmdInsTraza.Parameters["@cups"].Value = cups.cups;
                                    cmdInsTraza.Parameters["@evento"].Value = "SOL_ENF";
                                    cmdInsTraza.Parameters["@fecha"].Value = DateTime.Parse(solicitudWrk.fechaSolicitud);
                                    cmdInsTraza.Connection = Conex;
                                    cmdInsTraza.Transaction = TX1_1;
                                    if (cmdInsTraza.ExecuteNonQuery() < 1)
                                    {
                                        TX1_1.Rollback("TRx1_1");
                                        logLabcore.Info($"381 04:Error:Insertando en TAT_TRAZA_ORDEN la Solicitud: {solicitudWrk.nroSolicitud}");
                                        return $"381 04:Error:Insertando en TAT_TRAZA_ORDEN la Solicitud:{solicitudWrk.nroSolicitud}";
                                    }
                                    else
                                    {
                                        logLabcore.Info("*****386 Se insertó registro TAT_TRAZA_ORDEN *****");
                                        Int32 solcitudExiste = 0;
                                        using (SqlConnection Conn = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX))
                                        {
                                            Conn.Open();
                                            string strConsultaSolicitud = "SELECT TAT_SOLI FROM TAT_TRAZA_TAT WHERE TAT_ATEN=@atencion AND TAT_ORDEN=@orden AND TAT_CUPS=@cups AND TAT_SOLI=@Solicitud";
                                            // string strConsultaSolicitud = "SELECT TAT_SOLI FROM TAT_TRAZA_TAT WHERE TAT_ATEN=@atencion AND TAT_ORDEN=@orden AND TAT_CUPS=@cups";
                                            logLabcore.Info($"Consulta TAT_TRAZA_TAT, Para Actualizar:{strConsultaSolicitud}");
                                            logLabcore.Info($"Parametros Utilizados:Atencion: {solicitudWrk.idAtencion} Orden:{solicitudWrk.nroOrden}  Cup:{cups.cups.Trim()} Solicitud:{solicitudWrk.nroSolicitud.Trim()}");
                                            SqlCommand cmdConsultaSolicitud = new SqlCommand(strConsultaSolicitud, Conn);
                                            cmdConsultaSolicitud.Parameters.Add("@atencion", SqlDbType.Int).Value = solicitudWrk.idAtencion;
                                            cmdConsultaSolicitud.Parameters.Add("@orden", SqlDbType.Int).Value = solicitudWrk.nroOrden;
                                            cmdConsultaSolicitud.Parameters.Add("@cups", SqlDbType.VarChar).Value = cups.cups.Trim();
                                            cmdConsultaSolicitud.Parameters.Add("@Solicitud", SqlDbType.Int).Value = Int32.Parse(solicitudWrk.nroSolicitud.Trim());
                                            SqlDataReader rdSolicitud = cmdConsultaSolicitud.ExecuteReader();
                                            if (rdSolicitud.HasRows)
                                            {
                                                rdSolicitud.Read();
                                                solcitudExiste = rdSolicitud.GetInt32(0);
                                                logLabcore.Info($"404*****Validacion de Solicitud Existe:{solcitudExiste}");
                                            }
                                        }

                                        if (Int32.Parse(solicitudWrk.nroSolicitud) > 0 && solcitudExiste == 0)  //todo: validar oportunidad de codigo-
                                        {
                                            DateTime fechaOrden = utilLocal.fechaOrdenTAT(Int32.Parse(solicitudWrk.nroOrden));
                                            string strActualizaTraza = "UPDATE TAT_TRAZA_TAT SET TAT_SOLI=@solicitud_tat, EVT_SOLI=@fecha,EVT_ORD=@fechaorden " +
                                              "WHERE TAT_ATEN=@atencion AND TAT_ORDEN=@orden AND TAT_CUPS=@cups AND (TAT_SOLI=0 OR TAT_SOLI=@solicitud_tat)";
                                            SqlCommand cmdActualizaTraza = new SqlCommand(strActualizaTraza, Conex, TX1_1);
                                            cmdActualizaTraza.Parameters.Add("@orden", SqlDbType.Int).Value = solicitudWrk.nroOrden;
                                            cmdActualizaTraza.Parameters.Add("@solicitud_tat", SqlDbType.Int).Value = solicitudWrk.nroSolicitud;
                                            cmdActualizaTraza.Parameters.Add("@atencion", SqlDbType.Int).Value = solicitudWrk.idAtencion;
                                            cmdActualizaTraza.Parameters.Add("@cups", SqlDbType.VarChar).Value = cups.cups.Trim();
                                            cmdActualizaTraza.Parameters.Add("@fecha", SqlDbType.DateTime).Value = DateTime.Parse(solicitudWrk.fechaSolicitud);
                                            cmdActualizaTraza.Parameters.Add("@fechaOrden", SqlDbType.DateTime).Value = fechaOrden;
                                            if (cmdActualizaTraza.ExecuteNonQuery() > 0)
                                            {
                                                cups_Procesados.Add(cups);
                                                logLabcore.Info("Se adiciona CUPs Procesados(UPD): 422: " + cups.cups);
                                                logLabcore.Info("**** !!! Se Actualiza TAT_TRAZA_TAT Exitosamente. !!! 420" + "  Numero orden:" + solicitudWrk.nroOrden + "  Numero Solicitud:" + solicitudWrk.nroSolicitud, "  Numero Atencion:" + solicitudWrk.idAtencion);
                                                //                                            return "";
                                            }
                                            else
                                            {
                                                TX1_1.Rollback("TRx1_1");
                                                logLabcore.Info("430:Error: No Fue Posible Actualizar la Solicitud en TAT_TRAZA_TAT Numero orden:" + solicitudWrk.nroOrden + "  Numero Solicitud:" + solicitudWrk.nroSolicitud, "  Numero Atencion:" + solicitudWrk.idAtencion);
                                                return "";
                                            }
                                        }
                                        else
                                        {
                                            string strInsertarTraza = "INSERT INTO TAT_TRAZA_TAT (TAT_ATEN,TAT_ORDEN,TAT_SOLI,TAT_CUPS,EVT_ORD) VALUES (@atencion,@orden,@solicitud,@cups,@fechaEvento)";
                                            SqlCommand cmdInsertarTraza = new SqlCommand(strInsertarTraza, Conex, TX1_1);
                                            cmdInsertarTraza.Parameters.Add("@atencion", SqlDbType.Int).Value = solicitudWrk.idAtencion;
                                            cmdInsertarTraza.Parameters.Add("@orden", SqlDbType.Int).Value = solicitudWrk.nroOrden;
                                            cmdInsertarTraza.Parameters.Add("@solicitud", SqlDbType.Int).Value = solicitudWrk.nroSolicitud;
                                            // insertar la fecha de orden
                                            cmdInsertarTraza.Parameters.Add("@cups", SqlDbType.VarChar).Value = cups.cups.Trim();
                                            cmdInsertarTraza.Parameters.Add("@fechaEvento", SqlDbType.DateTime).Value = DateTime.Parse(solicitudWrk.fechaSolicitud);
                                            cmdInsertarTraza.Connection = Conex;
                                            //cmdTraza.Transaction = TX1;
                                            if (cmdInsertarTraza.ExecuteNonQuery() < 1)
                                            {
                                                TX1_1.Rollback("TRx1_1");
                                                logLabcore.Warn("449******04:No fue posible Insertar en:TAT_TRAZA_TAT para la SOLICITUD en proceso:" + solicitudWrk.nroSolicitud);
                                                return "";
                                            }
                                            else
                                            {
                                                cups_Procesados.Add(cups);
                                                logLabcore.Info("Se adiciona CUPs Procesados(INS): 422: " + cups.cups);
                                                logLabcore.Warn($"455*****Se ha Insertardo en:TAT_TRAZA_TAT para la SOLICITUD en proceso:{ solicitudWrk.nroSolicitud}  CUPS:{cups.cups}");
                                            }
                                        }
                                        //srLabcoreTAT
                                    }
                                }
                            }
                            logLabcore.Info($"Valores de Control- CupXProcesar:{cups_x_Procesar.Count()}  cupsProcesados:{cups_Procesados.Count()}");
                            if (cups_x_Procesar.Count() == cups_Procesados.Count())
                            {
                                TX1_1.Commit();
                                logLabcore.Info("*****  Inicio de Actualizacion de Tabla de Intercambio  *****");
                                SqlTransaction TX2;
                                TX2 = Conex.BeginTransaction("TRx2");

                                foreach (solicitudProducto  itemProcesar in cups_Procesados)
                                {
                                    try
                                    {
                                        if (cupsSolicWrk.Length == cupsTabSolicSync.Length)
                                        {
                                            try
                                            {
                                                cmdBorrar.CommandText = "DELETE FROM hceLabCliTATinvocaWS WHERE idAtencion=@idAtencion AND idOrden=@idOrden3 AND idSolicitud=@idSolicitud"; //AND idAccion=@idAccion 
                                                cmdBorrar.Parameters.Add("@idAtencion", SqlDbType.Int).Value = Int32.Parse(solicitudWrk.idAtencion);
                                                cmdBorrar.Parameters.Add("@idOrden3", SqlDbType.Int).Value = Int32.Parse(solicitudWrk.nroOrden);
                                                //cmdBorrar.Parameters.Add("@idAccion", SqlDbType.VarChar).Value = tipo;
                                                cmdBorrar.Parameters.Add("@idSolicitud", SqlDbType.Int).Value = Int32.Parse(solicitudWrk.nroSolicitud);
                                                cmdBorrar.Connection = Conex;
                                                cmdBorrar.Transaction = TX2;
                                                if (cmdBorrar.ExecuteNonQuery() > 0)
                                                {
                                                    logLabcore.Info($"*****  Limpieza de Tabla de Intercambio: Se limpia La Solicitud: {solicitudWrk.nroSolicitud} Orden:{solicitudWrk.nroOrden} Estudio:{solicitudWrk.producto} ****");
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
                                                        logLabcore.Info("----- Se Actualiza tabla de consecutivos de mensajes enviados:TAT_CONS_MSGS  -----");
                                                        entregaSolicitud(solicitudWrk.nroSolicitud, solicitudWrk.nroOrden, solicitudWrk.idAtencion, nroMsg);
                                                        TX2.Commit();
                                                        logLabcore.Info("**** 05:Recepcion de Solicitud Exitosa:Se Realiza el COMMIT de la Transaccion");
                                                        return "00:Proceso de Solicitud Existoso";
                                                    }
                                                    else
                                                    {
                                                        TX2.Rollback("TRx2");
                                                        logLabcore.Warn("***** 04:Error:No fue posible entregar la solicitud a Servicio TAT");
                                                        return "";
                                                    }
                                                }
                                                else
                                                {
                                                    TX2.Rollback("TRx2");
                                                    logLabcore.Warn("***** 04:Error:Limpiando Datos de Solicitud en Tabla de Sincronizacion");
                                                    return "03: Error Limpiando tabla de Sincronizacion";
                                                }
                                            }
                                            catch (SqlException SqlExp)
                                            {
                                                logLabcore.Warn(SqlExp.Message, "04:Excepcion en SQL");
                                                return "";
                                            }
                                            catch (Exception exp10)
                                            {
                                                logLabcore.Warn(exp10.Message, "04:Excepcion en SQL");
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
                                        logLabcore.Warn(sqlEx.Message, "04:Excepcion en SQL " + sqlEx.StackTrace);
                                        utilLocal.notificaFalla("04:Excepcion en SQL " + sqlEx.Message);
                                        return "04:" + sqlEx.Message;
                                    }
                                    catch (Exception exp1)
                                    {
                                        logLabcore.Warn($"********* Se ha Presentado una excepcion. mensaje: {exp1.Message}     Pila de LLamados:{ exp1.StackTrace}");
                                        return "*** Se ha presentado una excepcion. ***";
                                    } 
                                }
                                //************ Final de Actualizacion de Tabla de Intercambio
                            }
                            else
                            {
                                logLabcore.Info("******** ///// Se ha encontrado una diferencia enter el numero de CUPS a procesar:" + cups_x_Procesar.Count() + " y el numero de Procesados:" + cups_Procesados.Count() + "  ////////    ***************");
                                TX1_1.Rollback();
                                return "";
                            }
                        }
                        catch (SqlException sqlEx)
                        {
                            TX1_1.Rollback("TRx1_1");
                            logLabcore.Warn(sqlEx.Message, "04:Excepcion en SQL " + sqlEx.StackTrace);
                            utilLocal.notificaFalla("04:Excepcion en SQL " + sqlEx.Message);
                            return "04:Excepcion en SQL:" + sqlEx.Message;
                        }
                        catch (Exception exp11)
                        {
                            logLabcore.Info("Se ha presentado una excepcion:" + exp11.Message + "     Pila de llamados: " + exp11.StackTrace);
                            return "";
                        }
                    }
                    else
                    {
                        TX1_1.Rollback("TRx1_1");
                        utilLocal.notificaFalla("02:Error Guardando Datos Solicitud en Encabezado y Detalle de Solicitud");
                        logLabcore.Info("04:Error:Guardando Datos Solicitud:" + solicitudInput);
                        return "02:Error Guardando Datos Solicitud";
                    }
                }
                #endregion
            }
            catch (System.Xml.XmlException ex1)
            {
                utilLocal.notificaFalla("01:Error XML peticion." + ex1.Message + " En Solicitudes SAHI");
                logLabcore.Warn(ex1, "01:Excepcion en XML" + solicitudInput + "***" + ex1.StackTrace);
                return "01:Error peticion." + ex1.Message;
            }
            catch (Exception exp2)
            {
                logLabcore.Info("Se ha presentado una excepcion:" + exp2.Message + "    Pila de Llamados:" + exp2.StackTrace);
                return "";
            }
        }

        private void entregaSolicitud(string solicitud, string orden, string atencion, Int32 NumeroMsg)
        {
            try
            {
                srProxyOrdenar.IordenarEstudioClient clienteOrdenar = new srProxyOrdenar.IordenarEstudioClient();
                logLabcore.Info($"EndPoint de Consumo:  {clienteOrdenar.Endpoint.Address.Uri.AbsolutePath}");
                string rpta = clienteOrdenar.ordenar(solicitud, orden, atencion, NumeroMsg);
                //servicioPruebasLabcore.wsLabcoreSahiSoapClient clientePruebas = new servicioPruebasLabcore.wsLabcoreSahiSoapClient();
                //string rpta = clientePruebas.solicitudes(solicitud);


                logLabcore.Info($"Respuesta Recibida en wsLabcoreSahi.asmx:{solicitud}  Respuesta Recibida: {rpta}");
                logLabcore.Info($"***** Envio de Solicitud Terminado.");
            }
            catch (EndpointNotFoundException endPointExp)
            {
                String _mensaje = $"TAT(EndpointNotFound)(Solicitud: {solicitud}   Orden:{orden}  Atn:{atencion} ): {endPointExp.Message}";
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
            catch (Exception exp)
            {
                String _mensaje = "TAT(Solicitud:" + solicitud + " Orden:" + orden + " Atn:" + atencion + "):" + exp.Message;
                utilLocal.notificaFalla(_mensaje);
                logLabcore.Info("Excepcion desde wsLabcoreSahi.asmx Enviado la Solicitud a Trazabilidad:" + solicitud + " mensaje:" + exp.Message + " " + exp.StackTrace.ToString());
            }
        }

    }
}
