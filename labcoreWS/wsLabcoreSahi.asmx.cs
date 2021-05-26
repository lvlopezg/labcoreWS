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
  /// 
  /// </summary>
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
      logLabcore.Info("orden:" + ordenesInput);
      //ordenesInput = "<orden idAtencion=\"4888820\" nroOrden=\"6856586\" fechaOrden=\"2015/12/10 08:54:52\" idUsuario=\"3035\"><producto><id>10714</id><cups>902205</cups><cant>1</cant><obs /></producto><producto><id>11244</id><cups>902210</cups><cant>1</cant><obs /></producto></orden>";
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
        SqlConnection Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX);
        using (Conex)
        {
          Conex.Open();
          string strConsultar = "SELECT paramXML FROM hceLabCliTATinvocaWS WHERE idOrden=@orden AND idAtencion=@atencion AND idAccion='OP'";
          SqlCommand cmdConsulta = new SqlCommand(strConsultar, Conex);
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
          conCursor.Close();
          conCursor.Dispose();
          string strInsertar = "INSERT INTO TAT_ENC_ORDSAHI (NRO_ORDEN,NRO_ATEN,USR_ORDEN,FECHA_ORD) VALUES(@orden,@atencion,@usuario,@fecha)";
          SqlTransaction TX1 = Conex.BeginTransaction("tr1");
          SqlCommand cmdInsertar = new SqlCommand(strInsertar, Conex, TX1);
          cmdInsertar.Parameters.Add("@orden", SqlDbType.Int).Value = ordenWrk.nroOrden;
          cmdInsertar.Parameters.Add("@atencion", SqlDbType.Int).Value = ordenWrk.idAtencion;
          cmdInsertar.Parameters.Add("@usuario", SqlDbType.Int).Value = ordenWrk.idUsuario;
          cmdInsertar.Parameters.Add("@fecha", SqlDbType.DateTime).Value = ordenWrk.fechaOrden;

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
                  cmdInsTraza.CommandText = "INSERT INTO TAT_TRAZA_ORDEN (NRO_ORDEN,NRO_ATEN,ID_PROD,COD_CUPS,EVT_ORD,FECHA_EVT,ID_USUA) VALUES (@orden,@atencion,@idProd,@cups,'ORD_MED',@fecha," + ordenWrk.idUsuario + ")";
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
                    cmdTraza.Parameters["@atencion"].Value = ordenWrk.idAtencion;
                    cmdTraza.Parameters["@orden"].Value = ordenWrk.nroOrden;
                    cmdTraza.Parameters["@solicitud"].Value = 0;
                    cmdTraza.Parameters["@cups"].Value = cups.cups;
                    cmdTraza.Parameters["@fechaEvento"].Value = DateTime.Now.ToString();
                    cmdTraza.Connection = Conex;
                    cmdTraza.Transaction = TX1;
                    if (cmdTraza.ExecuteNonQuery() < 0)
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
                logLabcore.Info($"Orden {ordenWrk.nroOrden} en Proceso. Recepcion Exitosa en Trazabilidad");
                string qrySolicitudAut = "SELECT idAccion,idAtencion,idOrden,idSolicitud,paramXML,fecRegistro" +
                  "FROM hceLabCliTATinvocaWS sa WHERE sa.idAccion = 'SA' AND sa.idAtencion = @idAtencion AND sa.idOrden = @idOrden" +
                  "ORDER BY sa.idSolicitud";
                SqlCommand cmdSolicitudAut = new SqlCommand(qrySolicitudAut, Conex);
                cmdSolicitudAut.Parameters.Add("@idAtencion", SqlDbType.Int).Value = Int32.Parse(ordenWrk.idAtencion);
                cmdSolicitudAut.Parameters.Add("@idOrden", SqlDbType.Int).Value = Int32.Parse(ordenWrk.nroOrden);
                SqlDataReader rdSolicitudAut = cmdSolicitudAut.ExecuteReader();
                if (rdSolicitudAut.HasRows)
                {
                  while (rdSolicitudAut.Read())
                  {
                    solicitudes(rdSolicitudAut.GetString(4));
                    logLabcore.Info($"Se Proceso Solicitud Automatica:idAtencion:{rdSolicitudAut.GetInt32(1)} idOrden:{rdSolicitudAut.GetInt32(2)} idSolicitud:{rdSolicitudAut.GetInt32(1)}");
                  }
                }
                return "00:Recepcion Existosa";
              }
              else
              {
                TX1.Rollback("tr1");
                logLabcore.Warn("Error Limpiando tabla de sincronizacion");
                return "03: Error Limpiando tabla de Sincronizacion";
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
            logLabcore.Warn("02:Error Guardando Datos Orden");
            return "02:Error Guardando Datos Orden";
          }
        }
      }
      catch (System.Xml.XmlException ex1)
      {
        utilLocal.notificaFalla("01:Excepcion XML Recibiendo Orden desde SAHI" + ex1.Message + " En Ordenes SAHI");
        logLabcore.Info(ex1, "01:Excepcion XML Recibiendo Orden desde SAHI " + ordenesInput);
        return "01:Excepcion de peticion XML." + ex1.Message;
      }
      catch (SqlException sqlExp)
      {
        utilLocal.notificaFalla("04:Excepcion SQL Recibiendo Orden desde SAHI" + sqlExp.Message + " En Ordenes SAHI");
        logLabcore.Info(sqlExp, "04:Excepcion SQL Recibiendo Orden desde SAHI " + sqlExp.ToString() + "  Linea Numero::::" + sqlExp.LineNumber + "  " + sqlExp.StackTrace.ToString());
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
      logLabcore.Info("*******   Solicitud Recibida desde SAHI para Enviar:" + solicitudInput);
      //solicitudInput = "<solicitud idAtencion=\"6795140\" nroSolicitud=\"601042\" fechaSolicitud=\"2019 / 10 / 28 08:43:49\" prioridad=\"URGENTE\" nroOrden=\"9923076\" idUsuario=\"2921\">  <producto> <id>2125</id> <cups>903810</cups> <cant>1</cant> <obs>Prioridad: Hospitalario Prioritario .</obs> </producto> </solicitud>";
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
          Conex.Open();
          string strConsultar = "SELECT paramXML FROM hceLabCliTATinvocaWS WHERE idOrden=" + solicitudWrk.nroOrden + " AND idAtencion=" + solicitudWrk.idAtencion + " AND idSolicitud=" + solicitudWrk.nroSolicitud + " AND idAccion='SP'";
          SqlCommand cmdConsulta = new SqlCommand(strConsultar, Conex);
          SqlDataReader conCursor = cmdConsulta.ExecuteReader();
          if (conCursor.HasRows)
          {
            logLabcore.Info("Se encuentra el registro en tabla de intercambio. ******************************");
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
            logLabcore.Info("******** Solicitud desde SAHI: Se actualiza Encabezado de Solicitudes: TAT_ENC_SOLSAHI *****");
            try
            {
              List<solicitudProducto> cups_x_Procesar = cupsSolicWrk.ToList<solicitudProducto>();
              List<solicitudProducto> cups_Procesados = new List<solicitudProducto>();
              foreach (solicitudProducto cups in cupsSolicWrk)
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
                  logLabcore.Info("********  Se inserta Detalle en tabla TAT_DET_SOLSAHI ************");
                  cmdInsTraza.CommandText = "INSERT INTO TAT_TRAZA_ORDEN (NRO_ORDEN,NRO_SOLIC,NRO_ATEN,ID_PROD,COD_CUPS,EVT_ORD,FECHA_EVT,ID_USUA) VALUES (@orden,@solicitud,@atencion,@idProd,@cups,@evento,@fecha," + solicitudWrk.idUsuario + ")";
                  cmdInsTraza.Parameters["@orden"].Value = solicitudWrk.nroOrden;
                  cmdInsTraza.Parameters["@solicitud"].Value = solicitudWrk.nroSolicitud;
                  cmdInsTraza.Parameters["@atencion"].Value = solicitudWrk.idAtencion;
                  cmdInsTraza.Parameters["@idProd"].Value = cups.id;
                  cmdInsTraza.Parameters["@cups"].Value = cups.cups;
                  cmdInsTraza.Parameters["@evento"].Value = "SOL_ENF";
                  cmdInsTraza.Parameters["@fecha"].Value = DateTime.Now.ToString();
                  cmdInsTraza.Connection = Conex;
                  cmdInsTraza.Transaction = TX1_1;
                  if (cmdInsTraza.ExecuteNonQuery() < 1)
                  {
                    TX1_1.Rollback("TRx1_1");
                    logLabcore.Info("04:Error:Insertando en TAT_TRAZA_ORDEN la Solicitud:" + solicitudWrk.nroSolicitud);
                    return "";
                  }
                  else //***************** AQUI VA TAT_TRAZA_TAT
                  {
                    logLabcore.Info("************* Se inserta registro TAT_TRAZA_ORDEN **************");
                    Int32 solcitudExiste = 0;
                    using (SqlConnection Conn = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX))
                    {
                      Conn.Open();
                      string strConsultaSolicitud = "SELECT TAT_SOLI FROM TAT_TRAZA_TAT WHERE TAT_ATEN=@atencion AND TAT_ORDEN =@orden AND TAT_CUPS=@cups AND TAT_SOLI=" + solicitudWrk.nroSolicitud;
                      SqlCommand cmdConsultaSolicitud = new SqlCommand(strConsultaSolicitud, Conn);
                      cmdConsultaSolicitud.Parameters.Add("@atencion", SqlDbType.Int).Value = solicitudWrk.idAtencion;
                      cmdConsultaSolicitud.Parameters.Add("@orden", SqlDbType.Int).Value = solicitudWrk.nroOrden;
                      cmdConsultaSolicitud.Parameters.Add("@cups", SqlDbType.VarChar).Value = cups.cups.Trim();
                      SqlDataReader rdSolicitud = cmdConsultaSolicitud.ExecuteReader();
                      if (rdSolicitud.HasRows)
                      {
                        rdSolicitud.Read();
                        solcitudExiste = rdSolicitud.GetInt32(0);
                        logLabcore.Info($"Validacion de Solicitud Existe:{solcitudExiste}");
                      }
                    }
                    if (solicitudWrk.nroSolicitud.Equals(solcitudExiste.ToString()) && solcitudExiste == 0)
                    {
                      string strActualizaTraza = "UPDATE TAT_TRAZA_TAT SET TAT_SOLI=@solicitud_tat, EVT_SOLI=@fecha WHERE TAT_ATEN=@atencion AND TAT_ORDEN=@orden AND TAT_CUPS=@cups AND (TAT_SOLI=0 OR TAT_SOLI=@solicitud_tat)";
                      SqlCommand cmdActualizaTraza = new SqlCommand(strActualizaTraza, Conex, TX1_1);
                      cmdActualizaTraza.Parameters.Add("@orden", SqlDbType.Int).Value = solicitudWrk.nroOrden;
                      cmdActualizaTraza.Parameters.Add("@solicitud_tat", SqlDbType.Int).Value = solicitudWrk.nroSolicitud;
                      cmdActualizaTraza.Parameters.Add("@atencion", SqlDbType.Int).Value = solicitudWrk.idAtencion;
                      cmdActualizaTraza.Parameters.Add("@cups", SqlDbType.VarChar).Value = cups.cups.Trim();
                      cmdActualizaTraza.Parameters.Add("@fecha", SqlDbType.DateTime).Value = DateTime.Now;
                      if (cmdActualizaTraza.ExecuteNonQuery() > 0)
                      {
                        cups_Procesados.Add(cups);
                        logLabcore.Info("Se adiciona CUPs:::" + cups.cups);
                        logLabcore.Info("**** !!! Se Actualiza TAT_TRAZA_TAT Exitosamente. !!!" + "  Numero orden:" + solicitudWrk.nroOrden + "  Numero Solicitud:" + solicitudWrk.nroSolicitud, "  Numero Atencion:" + solicitudWrk.idAtencion);
                        //                                            return "";
                      }
                      else
                      {
                        TX1_1.Rollback("TRx1_1");
                        logLabcore.Info("044:Error: Actualizando Solicitud en TAT_TRAZA_TAT Numero orden:" + solicitudWrk.nroOrden + "  Numero Solicitud:" + solicitudWrk.nroSolicitud, "  Numero Atencion:" + solicitudWrk.idAtencion);
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
                      cmdInsertarTraza.Parameters.Add("@cups", SqlDbType.VarChar).Value = cups.cups.Trim();
                      cmdInsertarTraza.Parameters.Add("@fechaEvento", SqlDbType.DateTime).Value = DateTime.Now.ToString();
                      cmdInsertarTraza.Connection = Conex;
                      //cmdTraza.Transaction = TX1;
                      if (cmdInsertarTraza.ExecuteNonQuery() < 0)
                      {
                        TX1_1.Rollback("TRx1_1");
                        logLabcore.Warn("04:No fue posible Insertar en:TAT_TRAZA_TAT para la SOLICITUD en proceso:" + solicitudWrk.nroSolicitud);
                        return "";
                      }
                      else
                      {
                        cups_Procesados.Add(cups);
                        logLabcore.Warn($"Se ha Insertardo en:TAT_TRAZA_TAT para la SOLICITUD en proceso:{ solicitudWrk.nroSolicitud}  CUPS:{cups.cups}");
                      }
                    }
                  }
                }
              }
              if (cups_x_Procesar.Count() == cups_Procesados.Count())
              {
                TX1_1.Commit();
                logLabcore.Info("***********           Inicio de Actualizacion de Tabla de Intercambio       **************");
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
                        logLabcore.Info(" !!       *****     Limpieza de Tabla de Intercambio      ****  !!");
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
                          logLabcore.Info("--------------------------------- Se Actualiza tabla de consecutivos de mensajes enviados:TAT_CONS_MSGS  ---------------");
                          entregaSolicitud(solicitudWrk.nroSolicitud, solicitudWrk.nroOrden, solicitudWrk.idAtencion, nroMsg);
                          TX2.Commit();
                          logLabcore.Info("05:Recepcion de Solicitud Exitosa:Se Realiza el COMMIT de la Transaccion");
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
                  logLabcore.Info("*********      Se ha Presentado una excepcion. mensaje:" + exp1.Message + "     Pila de LLamados:" + exp1.StackTrace);
                  return "*** Se ha presentado una excepcion. ***";
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
              TX1_1.Rollback("TRx1");
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
            TX1_1.Rollback("TRx1");
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
        string rpta = clienteOrdenar.ordenar(solicitud, orden, atencion, NumeroMsg);
        logLabcore.Info("Respuesta Recibida en wsLabcoreSahi.asmx:" + solicitud + " Respuesta Recibida:" + rpta);
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
