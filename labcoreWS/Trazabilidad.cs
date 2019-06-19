using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using NLog;

namespace labcoreWS
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'Trazabilidad'
    public class Trazabilidad
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'Trazabilidad'
    {
        private static Logger logLabcore = LogManager.GetCurrentClassLogger();
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'Trazabilidad.Trazabilidad()'
        public Trazabilidad()
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'Trazabilidad.Trazabilidad()'
        { }
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'Trazabilidad.insertarTraza(string, string, string, string, string, DateTime, int)'
        public Boolean insertarTraza(string atencion, string orden, string solicitud, string cups, string evento, DateTime fechaEvt, Int32 nroNota)
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'Trazabilidad.insertarTraza(string, string, string, string, string, DateTime, int)'
        {
            string actualizar = string.Empty;
            bool respuesta = false;
            switch (evento)
            {
                case "ORM^SC":
                    {
                        actualizar = "UPDATE TAT_TRAZA_TAT SET EVT_TMAT=@fechaEvento WHERE TAT_ATEN=" + atencion + " AND TAT_ORDEN=" + orden + " AND TAT_SOLI=" + solicitud + " AND TAT_CUPS='" + cups + "'";
                        break;
                    }
                case "ORU^IP":
                    {
                        actualizar = "UPDATE TAT_TRAZA_TAT SET EVT_VTA=@fechaEvento WHERE TAT_ATEN=" + atencion + " AND TAT_ORDEN=" + orden + " AND TAT_SOLI=" + solicitud + " AND TAT_CUPS='" + cups + "'";
                        break;
                    }
                case "ORU^R01":
                    {
                        actualizar = "UPDATE TAT_TRAZA_TAT SET EVT_RES=@fechaEvento, NRO_NOTA=@numeroNota WHERE TAT_ATEN=" + atencion + " AND TAT_ORDEN=" + orden + " AND TAT_SOLI=" + solicitud + " AND TAT_CUPS='" + cups + "'";
                        break;
                    }
                case "ORM^CA":
                    {
                        actualizar = "UPDATE TAT_TRAZA_TAT SET EVT_CAN=@fechaEvento WHERE TAT_ATEN=" + atencion + " AND TAT_ORDEN=" + orden + " AND TAT_SOLI=" + solicitud + " AND TAT_CUPS='" + cups + "'";
                        break;
                    }
                case "ORM^PE":
                    {
                        actualizar = "UPDATE TAT_TRAZA_TAT SET EVT_VTA=@fechaEvento WHERE TAT_ATEN=" + atencion + " AND TAT_ORDEN=" + orden + " AND TAT_SOLI=" + solicitud + " AND TAT_CUPS='" + cups + "'";
                        break;
                    }
                case "SOL_ENF":
                    {
                        actualizar = "UPDATE TAT_TRAZA_TAT SET EVT_SOLI=@fechaEvento,TAT_SOLI=" + solicitud + " WHERE TAT_ATEN=" + atencion + " AND TAT_ORDEN=" + orden + " AND TAT_CUPS='" + cups + "'";
                        break;
                    }
                case "EVT_VAL":
                    {
                        actualizar = "UPDATE TAT_TRAZA_TAT SET EVT_VAL=@fechaEvento,TAT_SOLI=" + solicitud + " WHERE TAT_ATEN=" + atencion + " AND TAT_ORDEN=" + orden + " AND TAT_CUPS='" + cups + "' AND NRO_NOTA=" + nroNota;
                        break;
                    }
            }
            using (SqlConnection Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX))
            {
                try
                {
                    Conex.Open();
                    SqlTransaction txResultado = Conex.BeginTransaction("TxResultado");
                    SqlCommand cmdActualizar = new SqlCommand(actualizar, Conex, txResultado);
                    cmdActualizar.Parameters.Add("@fechaEvento", System.Data.SqlDbType.DateTime).Value = fechaEvt;
                    cmdActualizar.Parameters.Add("@numeroNota", System.Data.SqlDbType.Int).Value = nroNota;
                    //cmdActualizar.Parameters["@fechaEvento"].Value = fechaEvt;
                    //cmdActualizar.Parameters["@numeroNota"].Value = nroNota;
                    if (cmdActualizar.ExecuteNonQuery() > 0)
                    {
                        txResultado.Commit();
                        respuesta = true;
                        logLabcore.Info("Se actualiza Trazabilidad: " + actualizar);
                    }
                    else
                    {
                        respuesta = false;
                        txResultado.Rollback();
                        logLabcore.Info("No fue posible realizar la actualizacion de Trazabilidad-Metodo insertarTraza():" + actualizar);
                    }
                }
                catch (SqlException sqlExp)
                {

                    logLabcore.Warn(sqlExp.Message, "!!! Se ha presentado una falla SQL Actualizando Trazabilidad !!! " + sqlExp.Message);
                }
            }
            return respuesta;
        }

    }
}