using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace labcoreWS
{
    public class MensajeHL7
    {
        string[] segmentos = new string[80000];
        string[] segMSH = new string[19];
        string[] segPID = new string[30];
        string[] segPV1 = new string[52];
        string[] segPV2 = new string[48];
        string[] segIN1 = new string[20];
        string[] segORC = new string[20];
        string[] segOBR = new string[46];
        string[] segOBX = new string[23];
        string[] segNTE = new string[23];
        string[] segMSA = new string[10];

        public List<string[]> segmentosOBR = new List<string[]>();
        public List<string[]> segmentosOBX = new List<string[]>();
        public List<string[]> segmentosNTE = new List<string[]>();
        public MSHClass objMSH = new MSHClass();
        public PIDClass objPID = new PIDClass();
        public PV1Class objPV1 = new PV1Class();
        public PV2Class objPV2 = new PV2Class();
        public IN1Class objIN1 = new IN1Class();
        public ORCClass objORC = new ORCClass();
        public OBRClass objOBR = new OBRClass();
        public OBXClass objOBX = new OBXClass();
        public NTEClass objNTE = new NTEClass();
        public MSAClass objMSA = new MSAClass();
//        private string msgHL7;
        public MensajeHL7()
        { }

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

        public void nroTipoResultados()
        {
           
        }

    }

    public class strucMensaje
    {
        public List<int> ordenNM = new List<int>();
        public List<int> ordenST = new List<int>();
        public List<int> ordenTX = new List<int>();
        public int[] ValoresDifTX;
        Int16 contador = 0;
        public strucMensaje()
        {}
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