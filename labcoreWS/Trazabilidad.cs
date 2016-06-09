﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using NLog;

namespace labcoreWS
{
    public class Trazabilidad
    {
        private static Logger logLabcore = LogManager.GetCurrentClassLogger();
        public Trazabilidad()
        { }
        public Boolean insertarTraza(string atencion,string orden,string solicitud,string cups,string evento,DateTime fechaEvt,Int32 nroNota)
        {

            string actualizar = string.Empty;
            bool respuesta = false;
            switch(evento)
            {
                case "ORM^SC":
                    {
                        actualizar = "UPDATE TAT_TRAZA_TAT SET EVT_TMAT=@fechaEvento WHERE TAT_ATEN="+atencion+" AND TAT_ORDEN="+orden+" AND TAT_SOLI="+solicitud+" AND TAT_CUPS='"+cups+"'";
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
                        actualizar = "UPDATE TAT_TRAZA_TAT SET EVT_SOLI=@fechaEvento,TAT_SOLI="+solicitud+" WHERE TAT_ATEN=" + atencion + " AND TAT_ORDEN=" + orden + " AND TAT_CUPS='" + cups + "'";
                        break;

                    }
                case "EVT_VAL":
                    {
                        actualizar = "UPDATE TAT_TRAZA_TAT SET EVT_VAL=@fechaEvento,TAT_SOLI=" + solicitud + " WHERE TAT_ATEN=" + atencion + " AND TAT_ORDEN=" + orden + " AND TAT_CUPS='" + cups + "' AND NRO_NOTA="+nroNota;
                        break;

                    }
            }
            using (SqlConnection Conex = new SqlConnection(Properties.Settings.Default.LabcoreDBConXX))
            {
                try
                {
                    Conex.Open();
                    SqlCommand cmdActualizar = new SqlCommand(actualizar, Conex);
                    cmdActualizar.Parameters.Add("@fechaEvento", System.Data.SqlDbType.DateTime);
                    cmdActualizar.Parameters.Add("@numeroNota", System.Data.SqlDbType.Int);
                    cmdActualizar.Parameters["@fechaEvento"].Value = fechaEvt;
                    cmdActualizar.Parameters["@numeroNota"].Value = nroNota;
                    if (cmdActualizar.ExecuteNonQuery() > 0)
                    {
                        respuesta = true;
                        logLabcore.Info("Se actualiza Trazabilidad: "+actualizar);
                    }
                    else
                    {
                        respuesta = false;
                        logLabcore.Info("No fue posible realizar la actualizacion de Trazabilidad:"+actualizar);
                    }
                }
                catch(SqlException sqlExp)
                {
                    logLabcore.Warn(sqlExp.Message, "!!! Se ha presentado una falla Actualizando Trazabilidad !!! "+sqlExp.Message);
                }
            }
            return respuesta;
        }

    }
}