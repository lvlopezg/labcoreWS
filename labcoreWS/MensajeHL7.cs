using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace labcoreWS
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'MensajeHL7'
    public class MensajeHL7
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'MensajeHL7'
    {
		readonly string[] segmentos = new string[80000];
		readonly string[] segMSH = new string[19];
		readonly string[] segPID = new string[30];
		readonly string[] segPV1 = new string[52];
		readonly string[] segPV2 = new string[48];
		readonly string[] segIN1 = new string[20];
		readonly string[] segORC = new string[20];
		readonly string[] segOBR = new string[46];
		readonly string[] segOBX = new string[23];
		readonly string[] segNTE = new string[23];
		readonly string[] segMSA = new string[10];

		/// <summary>
		/// Sergmento OBR del Mensaje
		/// </summary>
        public List<string[]> segmentosOBR = new List<string[]>();
		/// <summary>
		/// Segmento OBX del Mensaje
		/// </summary>
        public List<string[]> segmentosOBX = new List<string[]>();
		/// <summary>
		/// Segmento NTE del Mensaje
		/// </summary>
        public List<string[]> segmentosNTE = new List<string[]>();
		/// <summary>
		/// Segmento MSH del Mensaje
		/// </summary>
        public MSHClass objMSH = new MSHClass();
		/// <summary>
		/// Segmento PID del Mensaje: Identificacion del Paciente
		/// </summary>
        public PIDClass objPID = new PIDClass();
		/// <summary>
		/// Segmento PV1 del mensaje: Atencion (Patient Visit)
		/// </summary>
        public PV1Class objPV1 = new PV1Class();
		/// <summary>
		/// Segmento PV2 del Mensaje.
		/// </summary>
        public PV2Class objPV2 = new PV2Class();
		/// <summary>
		/// Segmento IN1 del Mensaje: informaciocion de la Institucion EPS
		/// </summary>
        public IN1Class objIN1 = new IN1Class();
		/// <summary>
		/// Segmento ORC del Mensaje: Informacion de la Orden
		/// </summary>
        public ORCClass objORC = new ORCClass();
		/// <summary>
		/// Segmento OBR del Mensaje
		/// </summary>
        public OBRClass objOBR = new OBRClass();
		/// <summary>
		/// Segmento OBX del Mensaje
		/// </summary>
        public OBXClass objOBX = new OBXClass();
		/// <summary>
		/// Segmento NTE del Mensaje
		/// </summary>
		public NTEClass objNTE = new NTEClass();
		/// <summary>
		/// Segmento MSA de los Mensajes ACK
		/// </summary>
        public MSAClass objMSA = new MSAClass();


		/// <summary>
		/// Constructor de la clase MensajeHL7
		/// </summary>
		public MensajeHL7()
        { }
		/// <summary>
		/// Constructor de la Clase MensajeHL7. con Parametro
		/// </summary>
		/// <param name="msgHL7">String de Mensaje HL/</param>
        public MensajeHL7(string msgHL7)
        {
            string reemplazar = "";
            string SinSaltos = msgHL7.Replace("\r\n", reemplazar).Replace("\n", reemplazar).Replace("\r", reemplazar);
            msgHL7 = SinSaltos;
            if (msgHL7.IndexOf("PID") > 0)
            {
                //msgHL7 = msgHL7.Insert(msgHL7.IndexOf("PID"), "Æ");
                int posicion = msgHL7.IndexOf("PID");
                string valor = msgHL7.Substring(posicion + 3, 3);
                for (int i = 0; i <= msgHL7.Length; i = posicion)
                {
                    if (msgHL7.Substring(posicion + 3, 1).Equals("|"))
                    {
                        msgHL7 = msgHL7.Insert((posicion), "Æ");
                    }
                    posicion = msgHL7.IndexOf("PID", posicion + 4);
                    valor = msgHL7.Substring(posicion + 3, 3);
                    if (posicion == -1)
                    {
                        break;
                    }
                }
            }
            if (msgHL7.IndexOf("PV1") > 0)
            {
                //msgHL7 = msgHL7.Insert(msgHL7.IndexOf("PV1"), "Æ");
                int posicion = msgHL7.IndexOf("PV1");
                string valor = msgHL7.Substring(posicion + 3, 3);
                for (int i = 0; i <= msgHL7.Length; i = posicion)
                {
                    if (msgHL7.Substring(posicion + 3, 1).Equals("|"))
                    {
                        msgHL7 = msgHL7.Insert((posicion), "Æ");
                    }
                    posicion = msgHL7.IndexOf("PV1", posicion + 4);
                    valor = msgHL7.Substring(posicion + 3, 3);
                    if (posicion == -1)
                    {
                        break;
                    }
                }
            }
            if (msgHL7.IndexOf("PV2") > 0)
            {
                //msgHL7 = msgHL7.Insert(msgHL7.IndexOf("PV2"), "Æ");
                int posicion = msgHL7.IndexOf("PV2");
                string valor = msgHL7.Substring(posicion + 3, 3);
                for (int i = 0; i <= msgHL7.Length; i = posicion)
                {
                    if (msgHL7.Substring(posicion + 3, 1).Equals("|"))
                    {
                        msgHL7 = msgHL7.Insert((posicion), "Æ");
                    }
                    posicion = msgHL7.IndexOf("PV2", posicion + 4);
                    valor = msgHL7.Substring(posicion + 3, 3);
                    if (posicion == -1)
                    {
                        break;
                    }
                }
            }
            if (msgHL7.IndexOf("IN1") > 0)
            {
                //msgHL7 = msgHL7.Insert(msgHL7.IndexOf("IN1"), "Æ");
                int posicion = msgHL7.IndexOf("IN1");
                string valor = msgHL7.Substring(posicion + 3, 3);
                for (int i = 0; i <= msgHL7.Length; i = posicion)
                {
                    if (msgHL7.Substring(posicion + 3, 1).Equals("|"))
                    {
                        msgHL7 = msgHL7.Insert((posicion), "Æ");
                    }
                    posicion = msgHL7.IndexOf("IN1", posicion + 4);
                    valor = msgHL7.Substring(posicion + 3, 3);
                    if (posicion == -1)
                    {
                        break;
                    }
                }
            }
            if (msgHL7.IndexOf("ORC") > 0)
            {
                //msgHL7 = msgHL7.Insert(msgHL7.IndexOf("ORC"), "Æ");
                int posicion = msgHL7.IndexOf("ORC");
                string valor = msgHL7.Substring(posicion + 3, 3);
                for (int i = 0; i <= msgHL7.Length; i = posicion)
                {
                    if (msgHL7.Substring(posicion + 3, 1).Equals("|"))
                    {
                        msgHL7 = msgHL7.Insert((posicion), "Æ");
                    }
                    posicion = msgHL7.IndexOf("ORC", posicion + 4);
                    valor = msgHL7.Substring(posicion + 3, 3);
                    if (posicion == -1)
                    {
                        break;
                    }
                }
            }
            if (msgHL7.IndexOf("OBR") > 0)
            {
                //msgHL7 = msgHL7.Insert(msgHL7.IndexOf("OBR"), "Æ");
                int posicion = msgHL7.IndexOf("OBR");
                string valor = msgHL7.Substring(posicion + 3, 3);
                for (int i = 0; i <= msgHL7.Length; i = posicion)
                {
                    if (msgHL7.Substring(posicion + 3, 1).Equals("|"))
                    {
                        msgHL7 = msgHL7.Insert((posicion), "Æ");
                    }
                    posicion = msgHL7.IndexOf("OBR", posicion + 4);
                    valor = msgHL7.Substring(posicion + 3, 3);
                    if (posicion == -1)
                    {
                        break;
                    }
                }
            }
            if (msgHL7.IndexOf("OBX") > 0)
            {
                //int posicion = msgHL7.IndexOf("OBX");
                //for (int i = 0; i <= msgHL7.Length; i = posicion)
                //{
                //    msgHL7 = msgHL7.Insert((posicion), "Æ");
                //    posicion = msgHL7.IndexOf("OBX", posicion + 4);
                //    if (posicion == -1)
                //    {
                //        break;
                //    }
                //}
                int posicion = msgHL7.IndexOf("OBX");
                string valor = msgHL7.Substring(posicion + 3, 3);
                for (int i = 0; i <= msgHL7.Length; i = posicion)
                {
                    if (msgHL7.Substring(posicion + 3, 1).Equals("|") && (msgHL7.Substring(posicion-1, 1).Equals("|")))
                    {
                        msgHL7 = msgHL7.Insert((posicion), "Æ");
                    }
                    posicion = msgHL7.IndexOf("OBX", posicion + 4);
                    valor = msgHL7.Substring(posicion + 3, 3);
                    if (posicion == -1)
                    {
                        break;
                    }
                }
            }
            if (msgHL7.IndexOf("NTE") > 0)
            {
                int posicion = msgHL7.IndexOf("NTE");
                string valor = msgHL7.Substring(posicion+3, 3);
                for (int i = 0; i <= msgHL7.Length; i = posicion)
                {
                    if (msgHL7.Substring(posicion + 3, 1).Equals("|") && (msgHL7.Substring(posicion - 1, 1).Equals("|")))
                    {
                        msgHL7 = msgHL7.Insert((posicion), "Æ");
                    }
                    posicion = msgHL7.IndexOf("NTE", posicion + 4);
                    valor = msgHL7.Substring(posicion+3, 3);
                    if (posicion == -1)
                    {
                        break;
                    }
                }
            }
            //msgHL7 = msgHL7.Replace('\n', 'Æ');

            segmentos = msgHL7.Split('Æ');
            string nroSegObx = string.Empty;
            foreach (string segmentoW in segmentos)
            {
                if (segmentoW.Substring(0, 3).Equals("OBX") )
                {
                    string[] dataOBX = segmentoW.Split('|');
                    nroSegObx = dataOBX[1];
                }

                switch (segmentoW.Substring(0, 3))
                {

                    case "MSH":
                        segMSH = segmentoW.Split('|');
                        objMSH = new MSHClass(segMSH);
                        break;
                    case "PID":
                        segPID = segmentoW.Split('|');
                        objPID = new PIDClass(segPID);
                        break;
                    case "PV1":
                        segPV1 = segmentoW.Split('|');
                        objPV1 = new PV1Class(segPV1);
                        break;
                    case "PV2":
                        segPV2 = segmentoW.Split('|');
                        objPV2 = new PV2Class(segPV2);
                        break;
                    case "IN1":
                        segIN1 = segmentoW.Split('|');
                        objIN1 = new IN1Class(segIN1);
                        break;
                    case "ORC":
                        segORC = segmentoW.Split('|');
                        objORC = new ORCClass(segORC);
                        break;
                    case "OBR":
                        segmentosOBR.Add(segmentoW.Split('|'));
                        //segobr = segmentow.split('|');
                        //objobr = new obrclass(segobr);
                        break;
                    case "OBX":
                        //segOBX = segmentoW.Split('|');
                        //objOBX = new OBXClass(segOBX);
                        segmentosOBX.Add(segmentoW.Split('|'));
                        
                        break;
                    case "NTE":
                        segNTE = segmentoW.Split('|');
                        segNTE[4] = nroSegObx;
                        //objNTE = new NTEClass(segNTE);
                        //segmentosNTE.Add(segmentoW.Split('|'));
                        segmentosNTE.Add(segNTE);
                        break;
                    case "MSA":
                        segMSA = segmentoW.Split('|');
                        objMSA = new MSAClass(segMSA);
                        break;
                }
            }
        }
    }

	/// <summary>
	/// Clase para determinar y definir la estructura del mensaje HL7
	/// </summary>
    public class strucMensaje
    {
		/// <summary>
		/// Lista de los elementos tipo NM del segmento OBX
		/// </summary>
		public List<int> ordenNM = new List<int>();
		/// <summary>
		/// Lista de los Elementos tipo ST del mensaje, en el segmento OBX
		/// </summary>
        public List<int> ordenST = new List<int>();
		/// <summary>
		/// Lista de Elementos Tipo TX del segmento OBX
		/// </summary>
        public List<int> ordenTX = new List<int>();
		/// <summary>
		/// Apoyo para determinar las diferencias
		/// </summary>
        public int[] ValoresDifTX;
		readonly Int16 contador = 0;
		/// <summary>
		/// Constructor por Defecto del la Clase.
		/// </summary>
		public strucMensaje()
        {}

		/// <summary>
		/// Constructor con parametro
		/// </summary>
		/// <param name="segmento"></param>
        public strucMensaje(List<string[]> segmento)
        {
            foreach (string[] rX in segmento)
            {
                switch (rX[2])
                {
                    case "NM":
                        ordenNM.Add(contador);
                        break;
                    case "ST":
                        ordenST.Add(contador);
                        break;
                    case "TX":
                        ordenTX.Add(contador);
                        break;
                }
                contador++;
            }
            if (ordenTX.Count() > 1) 
            {
                ValoresDifTX = new int[ordenTX.Count()];
                int x = ordenTX.Count() - 1;
                int valInicial = ordenTX[x];
                for (int i = x; i > 0; i--)
                {
                    ValoresDifTX[i] = valInicial - ordenTX[i - 1];
                    valInicial = ordenTX[i - 1];
                }
                ValoresDifTX[0] = ordenTX[1] - ordenTX[0];
            }
            else if (ordenTX.Count() == 1)
            {
                ValoresDifTX = new int[1];
                ValoresDifTX[0] = ordenTX[0];
                //ordenTX.Add(0);
            }
            else
            {
                ValoresDifTX = new int[1];
                ValoresDifTX[0] = 0;
                ordenTX.Add(0);
            }
        }
    }
}