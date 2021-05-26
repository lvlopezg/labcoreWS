using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml;
using System.Xml.Serialization;

//*****
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Remoting.Contexts;
using NLog;
using labcoreWS.srLabcoreResultados;

/// <summary>
/// Integracion WebServices SOAP con Labcore
/// </summary>
namespace labcoreWS
{
	/// <summary>
	/// Implementacion del Servicio resultadosWS, para el cargue de Resultados de Labcore
	/// </summary>
	public class resultadosWS : IresultadosWS
    {
        private static readonly Logger logKioscos = LogManager.GetCurrentClassLogger();
        public string getResultados(string nroOrden)
        {
            logKioscos.Info($"Peticion de Resultado:{nroOrden}");
            byte tipoDoc = 0;
            //string archivo = "C:\\Basura";
            //System.IO.StreamWriter sw = new System.IO.StreamWriter(archivo + "\\logKioscos.log", true);
            //sw.WriteLine("--------------------------------------------------------------------------------" + "\r\n");
            //sw.WriteLine("Doc solicitado:" + nroOrden +" Evento:"+DateTime.Now.ToString()+ "\r\n");
            //sw.Close();
            Resultados rpta = new Resultados();
            try
            {
                string Confidenciales = string.Empty;
                string Pendientes = string.Empty;


                srLabcoreResultados.LabLinkHUSIClient clienteWS = new srLabcoreResultados.LabLinkHUSIClient();
                ResultadoPdf XMLdocumento = clienteWS.GetResultado(nroOrden);
                //string XMLdocumento = clienteWS.GetResultado(nroOrden);
                //System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

                //doc.LoadXml(XMLdocumento.ToString());
                //XmlNode Raiz = doc.FirstChild;
                //XmlNodeList registros = doc.GetElementsByTagName("ResultadoPdf");
                //XmlNodeList paciente = ((XmlElement)registros[0]).GetElementsByTagName("PACIENTE");

                //XmlElement pacienteWRK = (XmlElement)paciente[0];
                string Nombre = XMLdocumento.Paciente.Nombre;// pacienteWRK.GetAttribute("NOMBRE");
                string Apellidos = XMLdocumento.Paciente.Apellido;// pacienteWRK.GetAttribute("APELLIDO");
                string Documento = XMLdocumento.Paciente.Documento;// pacienteWRK.GetAttribute("DOCUMENTO");

                //XmlNodeList orden = ((XmlElement)registros[0]).GetElementsByTagName("ORDEN");
                //XmlElement ordernWrk = (XmlElement)orden[0];
                string nroOrdenwRK = XMLdocumento.Orden.Numero;// ordernWrk.GetAttribute("NUMERO");
                if (XMLdocumento.Orden.ResConfidenciales){Confidenciales = "1";} else { Confidenciales = "0"; }// ordernWrk.GetAttribute("RESCONFIDENCIALES");
                if (XMLdocumento.Orden.ResPendientes) { Pendientes = "1"; } else { Pendientes = "0"; }// ordernWrk.GetAttribute("RESPENDIENTES");

                //System.IO.StreamWriter sw1 = new System.IO.StreamWriter(archivo + "\\logKioscos.log", true);
                //using (sw1)
                //{
                //    sw1.WriteLine("Recibido Labcore-- Nombre:" + Nombre + "  Apellidos:" + Apellidos + "  Confidenciales:" + Confidenciales + "  Pendientes:" + Pendientes + " Evento:" + DateTime.Now.ToString() + "\r\n");
                //    sw1.Close();
                //}
                logKioscos.Info($"Recibido Labcore-- Nombre:{ Nombre } Apellidos:{ Apellidos }  Confidenciales:{ Confidenciales } Pendientes:{ Pendientes } Evento:{ DateTime.Now.ToString()}");
                using (SqlConnection conexion = new SqlConnection(Properties.Settings.Default.DBConexion))
                {
                    conexion.Open();
                    string query0 = "SELECT COUNT(NRO_IMP) FROM LABRES_IMP WHERE NRO_ORDEN=" + nroOrdenwRK;
                    SqlCommand sqlCom0 = new SqlCommand(query0, conexion);
                    SqlDataReader sqlReader = sqlCom0.ExecuteReader();
                    if (sqlReader.HasRows)
                    {
                        sqlReader.Read();
                        if (sqlReader.GetInt32(0) > 3)
                        {
                            rpta.status = "3";
                            rpta.Resultado.Add("Estos Resultados Ya se han Impreso, En mas de 3 Oportunidades.");
                            rpta.Nombre = "";
                            rpta.Apellido = "";
                            XmlSerializer salidaImp = new XmlSerializer(rpta.GetType());
                            StringWriter textWriterImp = new StringWriter();
                            salidaImp.Serialize(textWriterImp, rpta);
                            //System.IO.StreamWriter sw4 = new System.IO.StreamWriter(archivo + "\\logKioscos.log", true);
                            //using (sw4)
                            //{
                            //    sw4.WriteLine("Resultados Impresos en mas de 3 Oportunidades. \r\n");
                            //    sw4.Close();
                            //}
                            logKioscos.Info($"Resultados Impresos en mas de 3 Oportunidades. ");
                            return textWriterImp.ToString();
                        }

                    }
                }
                if (int.Parse(Confidenciales) == 0)
                {
                    if (int.Parse(Pendientes) == 0)
                    {
                        //XmlNodeList resultados = XMLdocumento.ResultadoBase64;// ((XmlElement)registros[0]).GetElementsByTagName("ResultadoBase64");

                        string cadenaBase64 = XMLdocumento.ResultadoBase64; ;//
                        //resultados.Item(0).InnerText;
                        if (cadenaBase64.Length > 10)
                        {
                            //Int32 i = 0;
                            //ResultadosResultado resXX = new ResultadosResultado();
                            rpta.Resultado.Add(cadenaBase64);
                            //foreach (XmlElement resultado in resultados)
                            //{
                            //    string valor = resultado.InnerText;
                            //    rpta.Resultado.Add(valor);
                            //    //rpta.Resultado[i].Value = contenidoResultados[i].Value;
                            //    i++;
                            //}
                            rpta.status = "0";
                            rpta.Nombre = Nombre;
                            rpta.Apellido = Apellidos;
                            using (SqlConnection conexion = new SqlConnection(Properties.Settings.Default.DBConexion))
                            {
                                conexion.Open();
                                string insertar = "INSERT INTO LABRES_IMP (NRO_ORDEN,TIPO_DOC,NRO_DOC,NRO_IMP,FECHA_IMP) VALUES(@nroOrden,@tipoDoc,@nroDoc,@nroImp,@Fecha)";
                                SqlCommand sqlIns0 = new SqlCommand(insertar, conexion);
                                sqlIns0.Parameters.Add("@nroOrden",SqlDbType.Int);
                                sqlIns0.Parameters.Add("@tipoDoc",SqlDbType.NVarChar);
                                sqlIns0.Parameters.Add("@nroDoc",SqlDbType.VarChar);
                                sqlIns0.Parameters.Add("@nroImp",SqlDbType.SmallInt);
                                sqlIns0.Parameters.Add("@Fecha",SqlDbType.DateTime);

                                sqlIns0.Parameters["@nroOrden"].Value= nroOrdenwRK;
                                sqlIns0.Parameters["@tipoDoc"].Value= tipoDoc;
                                sqlIns0.Parameters["@nroDoc"].Value= Documento;
                                sqlIns0.Parameters["@nroImp"].Value=1;
                                sqlIns0.Parameters["@Fecha"].Value=DateTime.Now;

                                sqlIns0.ExecuteNonQuery();
                                ////System.IO.StreamWriter sw2 = new System.IO.StreamWriter(archivo + "\\logKioscos.log", true);
                                //using (sw2)
                                //{
                                //    sw2.WriteLine("Guarda--Orden:" + nroOrdenwRK + " Documento:" + Documento + " Evento:" + DateTime.Now.ToString() + "\r\n");
                                //    sw2.Close();
                                //}
                                logKioscos.Info($"Guarda--Orden:{nroOrdenwRK}  Documento:{Documento}  Evento:{DateTime.Now.ToString()}");
                            }
                        }
                        else
                        {
                            rpta.status = "4";
                            rpta.Resultado.Add("");
                        }
                    }
                    else
                    {
                        rpta.status = "1";
                        rpta.Resultado.Add("");
                    }
                }
                else
                {
                    rpta.status = "2";
                    rpta.Resultado.Add("");
                }
                XmlSerializer salida = new XmlSerializer(rpta.GetType());
                StringWriter textWriter = new StringWriter();
                salida.Serialize(textWriter, rpta);

                //System.IO.StreamWriter sw3 = new System.IO.StreamWriter(archivo + "\\logKioscos.log", true);
                //using (sw3)
                //{
                    logKioscos.Info($"Envio a Kiosco--Orden:{nroOrdenwRK}  Documento:{Documento} Evento:{DateTime.Now.ToString()}");
                    //sw3.WriteLine("Envio a Kiosco--Orden:" + nroOrdenwRK + " Documento:" + Documento + " Evento:" + DateTime.Now.ToString() + "\r\n");
                    if (rpta.Resultado.First<string>().Length>0)
                    {
                        logKioscos.Info($"Respuesta:{rpta.Resultado.First<string>().Substring(0, 10)}");
                    //sw3.WriteLine("Respuesta:" + rpta.Resultado.First<string>().Substring(0, 10));
                    }
                    else
                    {
                    logKioscos.Info($" !! Respuesta: No se Imprime !!");
                        //sw3.WriteLine(" !! Respuesta: No se Imprime !!");
                    }

                //}
                return textWriter.ToString();
            }
            catch (SqlException ex1)
            {
                // "Error en Datos:" + ex1.Message;
                rpta.status = "5";
                rpta.Resultado.Add(ex1.Message);
                rpta.Nombre = "";
                rpta.Apellido = "";
                XmlSerializer salida = new XmlSerializer(rpta.GetType());
                StringWriter textWriter = new StringWriter();
                salida.Serialize(textWriter, rpta);
                // string archivo = "C:\\Basura";
                //System.IO.StreamWriter sw5 = new System.IO.StreamWriter(archivo + "\\logKioscos.log", true);
                //using (sw5)
                //{
                //sw5.WriteLine("--------------------------------------------------------------------------------" + "\r\n");
                logKioscos.Info($"Status:{rpta.status}  mensaje:{rpta.Resultado.First<string>()} Evento: {DateTime.Now.ToString()}");
                    //sw5.WriteLine("Status:" + rpta.status + "  mensaje:" + rpta.Resultado.First<string>() + " Evento:" + DateTime.Now.ToString() + "\r\n");
                    //sw5.WriteLine("--------------------------------------------------------------------------------" + "\r\n");
                    //sw5.WriteLine(archivo + "\r\n");
                    //sw5.Close();
                    //}
                return textWriter.ToString();
            }
            catch (EndpointNotFoundException ex2)
            {
                // "Error en comunicaciones:" + ex2.Message;
                rpta.status = "5";
                rpta.Resultado.Add(ex2.Message);
                rpta.Nombre = "";
                rpta.Apellido = "";
                XmlSerializer salida = new XmlSerializer(rpta.GetType());
                StringWriter textWriter = new StringWriter();
                salida.Serialize(textWriter, rpta);
                //System.IO.StreamWriter sw6 = new System.IO.StreamWriter(archivo + "\\logKioscos.log", true);
                //using (sw6)
                //{
                //    sw6.WriteLine("--------------------------------------------------------------------------------" + "\r\n");
                //    sw6.WriteLine("Status:" + rpta.status + "  mensaje:" + rpta.Resultado.First<string>() + " Evento:" + DateTime.Now.ToString() + "\r\n");
                //    sw6.WriteLine("--------------------------------------------------------------------------------" + "\r\n");
                //    sw6.WriteLine(archivo + "\r\n");
                //    sw6.Close();
                //}
                logKioscos.Info($"Status:{rpta.status}  mensaje:{rpta.Resultado.First<string>()} Evento:{DateTime.Now.ToString()}");
                return textWriter.ToString();
            }
            catch (DataException ex3)
            {
                //return "Error de Datos:" + ex3.Message;
                rpta.status = "5";
                rpta.Resultado.Add(ex3.Message);
                rpta.Nombre = "";
                rpta.Apellido = "";
                XmlSerializer salida = new XmlSerializer(rpta.GetType());
                StringWriter textWriter = new StringWriter();
                salida.Serialize(textWriter, rpta);
                //System.IO.StreamWriter sw7 = new System.IO.StreamWriter(archivo + "\\logKioscos.log", true);
                //using (sw7)
                //{
                //    sw7.WriteLine("--------------------------------------------------------------------------------" + "\r\n");
                //    sw7.WriteLine("Status:" + rpta.status + "  mensaje:" + rpta.Resultado.First<string>() + " Evento:" + DateTime.Now.ToString() + "\r\n");
                //    sw7.WriteLine("--------------------------------------------------------------------------------" + "\r\n");
                //    sw7.WriteLine(archivo + "\r\n");
                //    sw7.Close();
                //}
                logKioscos.Info($"Status:{ rpta.status } mensaje: { rpta.Resultado.First<string>()}  Evento: { DateTime.Now.ToString()}");
                return textWriter.ToString();
            }
            catch (FaultException ex4)
            {
                //return "Fallo General:" + ex4.Message + "    Razon:" + ex4.Reason.ToString() + "    Accion:" + ex4.Action;
                rpta.status = "5";
                rpta.Resultado.Add(ex4.Message);
                rpta.Nombre = "";
                rpta.Apellido = "";
                XmlSerializer salida = new XmlSerializer(rpta.GetType());
                StringWriter textWriter = new StringWriter();
                salida.Serialize(textWriter, rpta);
                //System.IO.StreamWriter sw8 = new System.IO.StreamWriter(archivo + "\\logKioscos.log", true);
                //using (sw8)
                //{
                //    sw8.WriteLine("--------------------------------------------------------------------------------" + "\r\n");
                //    sw8.WriteLine("Status:" + rpta.status + "  mensaje:" + rpta.Resultado.First<string>() + " Evento:" + DateTime.Now.ToString() + "\r\n");
                //    sw8.WriteLine("--------------------------------------------------------------------------------" + "\r\n");
                //    sw8.WriteLine(archivo + "\r\n");
                //    sw8.Close();
                //}

                return textWriter.ToString();
            }
            catch (InvalidMessageContractException ex5)
            {
                //return "Error en mensaje:" + ex5.Message;
                rpta.status = "5";
                rpta.Resultado.Add(ex5.Message);
                rpta.Nombre = "";
                rpta.Apellido = "";
                XmlSerializer salida = new XmlSerializer(rpta.GetType());
                StringWriter textWriter = new StringWriter();
                salida.Serialize(textWriter, rpta);
                //System.IO.StreamWriter sw9 = new System.IO.StreamWriter(archivo + "\\logKioscos.log", true);
                //using (sw9)
                //{
                //    sw9.WriteLine("--------------------------------------------------------------------------------" + "\r\n");
                //    sw9.WriteLine("Status:" + rpta.status + "  mensaje:" + rpta.Resultado.First<string>() + " Evento:" + DateTime.Now.ToString() + "\r\n");
                //    sw9.WriteLine("--------------------------------------------------------------------------------" + "\r\n");
                //    sw9.WriteLine(archivo + "\r\n");
                //    sw9.Close();
                //}
                logKioscos.Info($"Status:{rpta.status}  mensaje:{rpta.Resultado.First<string>()} Evento:{DateTime.Now.ToString()}");
                return textWriter.ToString();
            }
            catch (IOException ex6)
            {
                //return "Excepcion de Entrada /Salida:" + ex6.Message;
                rpta.status = "5";
                rpta.Resultado.Add( ex6.Message);
                rpta.Nombre = "";
                rpta.Apellido = "";
                XmlSerializer salida = new XmlSerializer(rpta.GetType());
                StringWriter textWriter = new StringWriter();
                salida.Serialize(textWriter, rpta);
                //System.IO.StreamWriter sw10 = new System.IO.StreamWriter(archivo + "\\logKioscos.log", true);
                //using (sw10)
                //{
                //    sw10.WriteLine("--------------------------------------------------------------------------------" + "\r\n");
                //    sw10.WriteLine("Status:" + rpta.status + "  mensaje:" + rpta.Resultado.First<string>() + " Evento:" + DateTime.Now.ToString() + "\r\n");
                //    sw10.WriteLine("--------------------------------------------------------------------------------" + "\r\n");
                //    sw10.WriteLine(archivo + "\r\n");
                //    sw10.Close();
                //}
                logKioscos.Info($"Status:{rpta.status} mensaje:{rpta.Resultado.First<string>()} Evento:{DateTime.Now.ToString()} ");
                return textWriter.ToString();
            }
            catch (SerializationException ex7)
            {
                //return "Error en la serializacion:" + ex7.Message;
                rpta.status = "5";
                rpta.Resultado.Add(ex7.Message);
                rpta.Nombre = "";
                rpta.Apellido = "";
                XmlSerializer salida = new XmlSerializer(rpta.GetType());
                StringWriter textWriter = new StringWriter();
                salida.Serialize(textWriter, rpta);
                //System.IO.StreamWriter sw11 = new System.IO.StreamWriter(archivo + "\\logKioscos.log", true);
                //using (sw11)
                //{
                //    sw11.WriteLine("--------------------------------------------------------------------------------" + "\r\n");
                //    sw11.WriteLine("Status:" + rpta.status + "  mensaje:" + rpta.Resultado.First<string>() + " Evento:" + DateTime.Now.ToString() + "\r\n");
                //    sw11.WriteLine("--------------------------------------------------------------------------------" + "\r\n");
                //    sw11.WriteLine(archivo + "\r\n");
                //    sw11.Close();
                //}
                logKioscos.Info($"Status:{rpta.status}  mensaje:{rpta.Resultado.First<string>()} Evento:{DateTime.Now.ToString()} ");
                return textWriter.ToString();
            }
            catch (ServerTooBusyException ex8)
            {
                //return "Servidor se encuentra ocupado:" + ex8.Message;
                rpta.status = "5";
                rpta.Resultado.Add(ex8.Message);
                rpta.Nombre = "";
                rpta.Apellido = "";
                XmlSerializer salida = new XmlSerializer(rpta.GetType());
                StringWriter textWriter = new StringWriter();
                salida.Serialize(textWriter, rpta);
                //System.IO.StreamWriter sw12 = new System.IO.StreamWriter(archivo + "\\logKioscos.log", true);
                //using (sw12)
                //{
                //    sw12.WriteLine("--------------------------------------------------------------------------------" + "\r\n");
                //    sw12.WriteLine("Status:" + rpta.status + "  mensaje:" + rpta.Resultado.First<string>() + " Evento:" + DateTime.Now.ToString() + "\r\n");
                //    sw12.WriteLine("--------------------------------------------------------------------------------" + "\r\n");
                //    sw12.WriteLine(archivo + "\r\n");
                //    sw12.Close();
                //}
                logKioscos.Info($"Status:{rpta.status} mensaje:{rpta.Resultado.First<string>()}  Evento:{DateTime.Now.ToString()} ");
                return textWriter.ToString();
            }
            catch (SystemException ex9)
            {
                //return "Error general del Sistema:" + ex9.Message + ex9.StackTrace;
                rpta.status = "6";
                rpta.Resultado.Add(ex9.Message);
                rpta.Nombre = "";
                rpta.Apellido = "";
                XmlSerializer salida = new XmlSerializer(rpta.GetType());
                StringWriter textWriter = new StringWriter();
                salida.Serialize(textWriter, rpta);
                //System.IO.StreamWriter sw13 = new System.IO.StreamWriter(archivo + "\\logKioscos.log", true);
                //using (sw13)
                //{
                //    sw13.WriteLine("--------------------------------------------------------------------------------" + "\r\n");
                //    sw13.WriteLine("Status:" + rpta.status + "  mensaje:" + rpta.Resultado.First<string>() + " Evento:" + DateTime.Now.ToString() + "\r\n");
                //    sw13.WriteLine("--------------------------------------------------------------------------------" + "\r\n");
                //    sw13.WriteLine(archivo + "\r\n");
                //    sw13.Close();
                //}
                logKioscos.Info($"Status:{rpta.status}  mensaje:{rpta.Resultado.First<string>()} Evento:{DateTime.Now.ToString()}");
                return textWriter.ToString();
            }
        }
    }
}




