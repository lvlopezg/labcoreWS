using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;


namespace labcoreWS
{
    /// <summary>
    /// Contrato del servicio para Ordenar Estudios de Laboratorio
    /// </summary>
    [ServiceContract]
    public interface IordenarEstudio
    {
        /// <summary>
        /// Operacion Para Ordenar estudios de Laboratorio
        /// </summary>
        /// <param name="solicitud">Numero de la Solicitud de Laboratorio</param>
        /// <param name="orden">Numero de la Orden de Laboratorio</param>
        /// <param name="atencion">id de la atencion</param>
        /// <param name="NroMsg">Numero consecutivo para el control de mensajes</param>
        /// <returns></returns>
        [OperationContract]
        Task<string> ordenarAsync(string solicitud, string orden, string atencion, Int32 NroMsg);

        /// <summary>
        /// Operacion de Cambio de Estado de las solicitudes en Proceso
        /// </summary>
        /// <param name="msgHL7">Mensaje en HL7 para el cambio de estado</param>
        /// <returns></returns>
        [OperationContract]
        string cambioEstado(string msgHL7);
    }
    /// <summary>
    /// Clase para Generar el segmento MSH de HL7
    /// </summary>
    [DataContract]
    public class MSHClass
    {
        private string msh1 = "MSH";
        private string msh2 = @"^~\&";
        private string msh3 = "SAHI";
        private string msh4 = string.Empty;
        private string msh5 = "LABCORE";
        private string msh6 = string.Empty;
        private string msh7 = string.Empty;
        private string msh8 = string.Empty;
        private string msh9 = string.Empty;
        private string msh10 = string.Empty;
        private string msh11 = string.Empty;
        private string msh12 = string.Empty;
        private string msh13 = string.Empty;
        private string msh14 = string.Empty;
        private string msh15 = "AL";
        private string msh16 = string.Empty;
        private string msh17 = string.Empty;
        private string msh18 = string.Empty;
        private string msh19 = string.Empty;
        private string msh20 = string.Empty;
        /// <summary>
        /// Contructor de la Clase MSHClass
        /// </summary>
        public MSHClass()
        {

        }
        /// <summary>
        /// Constructor de la Clase MSHClass, con un segmento MSH como parametro
        /// </summary>
        /// <param name="segMSH">Segmento MSH para Normaliar en HL7</param>
        public MSHClass(string[] segMSH)
        {
            Utilidades utls = new Utilidades();
            Msh1 = segMSH[0];
            Msh2 = segMSH[1];
            Msh3 = segMSH[2];
            Msh4 = segMSH[3];
            Msh5 = segMSH[4];
            Msh6 = segMSH[5];
            Msh7 = segMSH[6];
            Msh8 = segMSH[7];
            Msh9 = segMSH[8];
            Msh10 = segMSH[9];
            Msh11 = segMSH[10];
            Msh12 = segMSH[11];
            Msh13 = segMSH[12];
            Msh14 = segMSH[13];
            Msh15 = segMSH[14];
            Msh16 = segMSH[15];
            Msh17 = segMSH[16];
            Msh18 = segMSH[17];
            Msh19 = segMSH[18];
            //Msh20 = segMSH[19];
        }
        #region RegionDefMSH
        /// <summary>
        /// 
        /// </summary>
        public string Msh1
        {
            get { return msh1; }
            set { msh1 = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Msh2
        {
            get { return msh2; }
            set { msh2 = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Msh3
        {
            get { return msh3; }
            set { msh3 = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Msh4
        {
            get { return msh4; }
            set { msh4 = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Msh5
        {
            get { return msh5; }
            set { msh5 = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Msh6
        {
            get { return msh6; }
            set { msh6 = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Msh7
        {
            get { return msh7; }
            set { msh7 = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Msh8
        {
            get { return msh8; }
            set { msh8 = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Msh9
        {
            get { return msh9; }
            set { msh9 = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Msh10
        {
            get { return msh10; }
            set { msh10 = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Msh11
        {
            get { return msh11; }
            set { msh11 = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Msh12
        {
            get { return msh12; }
            set { msh12 = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Msh13
        {
            get { return msh13; }
            set { msh13 = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Msh14
        {
            get { return msh14; }
            set { msh14 = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Msh15
        {
            get { return msh15; }
            set { msh15 = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Msh16
        {
            get { return msh16; }
            set { msh16 = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Msh17
        {
            get { return msh17; }
            set { msh17 = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Msh18
        {
            get { return msh18; }
            set { msh18 = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Msh19
        {
            get { return msh19; }
            set { msh19 = value; }
        }
        /// <summary>
        /// 
        /// </summary>

        public string Msh20
        {
            get { return msh20; }
            set { msh20 = value; }
        }

        #endregion
        /// <summary>
        /// Formatea el segmento y retorna el resultado
        /// </summary>
        /// <returns>Un segmento MSH en HL7</returns>
        public string retornoMSH()
        {
            string spr = "|";
            string valString = msh1.ToString() + spr + msh2.ToString() + spr + msh3.ToString() + spr + msh4.ToString() + spr + msh5.ToString() + spr + msh6.ToString() + spr + msh7.ToString() + spr + msh8.ToString() + spr + msh9.ToString() + spr + msh10.ToString() + spr;
            valString = valString + msh11.ToString() + spr + msh12.ToString() + spr + msh13.ToString() + spr + msh14.ToString() + spr + Msh15.ToString() + spr + msh16.ToString() + spr + msh17.ToString() + spr + msh18.ToString() + spr + msh19.ToString();
            return valString;
        }
    }
    /// <summary>
    /// clase para generar el segmento HL7 PID
    /// </summary>
    [DataContract]
    public class PIDClass
    {
        #region RegionDefLocalPID
        private string pid1 = "PID";
        private string pid2 = string.Empty;
        private string pid3 = string.Empty;
        private string pid4 = string.Empty;
        private string pid5 = string.Empty;
        private string pid6 = string.Empty;
        private string pid7 = string.Empty;
        private string pid8 = string.Empty;
        private string pid9 = string.Empty;
        private string pid10 = string.Empty;
        private string pid11 = string.Empty;
        private string pid12 = string.Empty;
        private string pid13 = string.Empty;
        private string pid14 = string.Empty;
        private string pid15 = string.Empty;
        private string pid16 = string.Empty;
        private string pid17 = string.Empty;
        private string pid18 = string.Empty;
        private string pid19 = string.Empty;
        private string pid20 = string.Empty;
        private string pid21 = string.Empty;
        private string pid22 = string.Empty;
        private string pid23 = string.Empty;
        private string pid24 = string.Empty;
        private string pid25 = string.Empty;
        private string pid26 = string.Empty;
        private string pid27 = string.Empty;
        private string pid28 = string.Empty;
        private string pid29 = string.Empty;
        private string pid30 = string.Empty;
        #endregion
        /// <summary>
        /// /Definicion  del constructor de la clase segmento PID del mensaje HL7
        /// </summary>
        public PIDClass()
        {
        }
        /// /clase para Definicion segmento PID del mensaje HL7
        public PIDClass(string[] segPID)
        {
            Utilidades utls = new Utilidades();
            Pid1 = segPID[0];
            Pid2 = segPID[1];
            Pid3 = segPID[2];
            Pid4 = segPID[3];
            Pid5 = segPID[4];
            Pid6 = segPID[5];
            Pid7 = segPID[6];
            Pid8 = segPID[7];
            Pid9 = segPID[8];
            Pid10 = segPID[9];
            Pid11 = segPID[10];
            Pid12 = segPID[11];
            Pid13 = segPID[12];
            Pid14 = segPID[13];
            Pid15 = segPID[14];
            Pid16 = segPID[15];
            Pid17 = segPID[16];
            Pid18 = segPID[17];
            Pid19 = segPID[18];
            Pid20 = segPID[19];
            Pid21 = segPID[20];
            Pid22 = segPID[21];
            Pid23 = segPID[22];
            Pid24 = segPID[23];
            Pid25 = segPID[24];
            Pid26 = segPID[25];
            Pid27 = segPID[26];
            Pid28 = segPID[27];
            Pid29 = segPID[28];
            Pid30 = segPID[29];
        }

        #region RegionDefPID
        /// <summary>
        /// Campo 1 segmento PID
        /// </summary>
        public string Pid1
        {
            get { return pid1; }
            set { pid1 = value; }
        }
        /// <summary>
        /// Campo 2 segmento PID
        /// </summary>
        public string Pid2
        {
            get { return pid2; }
            set { pid2 = value; }
        }
        /// <summary>
        /// Campo 3 segmento PID
        /// </summary>
        public string Pid3
        {
            get { return pid3; }
            set { pid3 = value; }
        }
        /// <summary>
        /// Campo 4 segmento PID
        /// </summary>
        public string Pid4
        {
            get { return pid4; }
            set { pid4 = value; }
        }
        /// <summary>
        /// Campo 5 segmento PID
        /// </summary>
        public string Pid5
        {
            get { return pid5; }
            set { pid5 = value; }
        }
        /// <summary>
        /// Campo 6 segmento PID
        /// </summary>
        public string Pid6
        {
            get { return pid6; }
            set { pid6 = value; }
        }
        /// <summary>
        /// Campo 7 segmento PID
        /// </summary>
        public string Pid7
        {
            get { return pid7; }
            set { pid7 = value; }
        }
        /// <summary>
        /// Campo 8 segmento PID
        /// </summary>
        public string Pid8
        {
            get { return pid8; }
            set { pid8 = value; }
        }
        /// <summary>
        /// Campo 9 segmento PID
        /// </summary>
        public string Pid9
        {
            get { return pid9; }
            set { pid9 = value; }
        }
        /// <summary>
        /// Campo 10 segmento PID
        /// </summary>
        public string Pid10
        {
            get { return pid10; }
            set { pid10 = value; }
        }
        /// <summary>
        /// Campo 11 segmento PID
        /// </summary>
        public string Pid11
        {
            get { return pid11; }
            set { pid11 = value; }
        }
        /// <summary>
        /// Campo 12 segmento PID
        /// </summary>
        public string Pid12
        {
            get { return pid12; }
            set { pid12 = value; }
        }
        /// <summary>
        /// Campo 13 segmento PID
        /// </summary>
        public string Pid13
        {
            get { return pid13; }
            set { pid13 = value; }
        }
        /// <summary>
        /// Campo 14 segmento PID
        /// </summary>
        public string Pid14
        {
            get { return pid14; }
            set { pid14 = value; }
        }
        /// <summary>
        /// Campo 15 segmento PID
        /// </summary>
        public string Pid15
        {
            get { return pid15; }
            set { pid15 = value; }
        }
        /// <summary>
        /// Campo 16 segmento PID
        /// </summary>
        public string Pid16
        {
            get { return pid16; }
            set { pid16 = value; }
        }
        /// <summary>
        /// Campo 17 segmento PID
        /// </summary>
        public string Pid17
        {
            get { return pid17; }
            set { pid17 = value; }
        }
        /// <summary>
        /// Campo 18 segmento PID
        /// </summary>
        public string Pid18
        {
            get { return pid18; }
            set { pid18 = value; }
        }
        /// <summary>
        /// Campo 19 segmento PID
        /// </summary>
        public string Pid19
        {
            get { return pid19; }
            set { pid19 = value; }
        }
        /// <summary>
        /// Campo 20 segmento PID
        /// </summary>
        public string Pid20
        {
            get { return pid20; }
            set { pid20 = value; }
        }
        /// <summary>
        /// Campo 21 segmento PID
        /// </summary>
        public string Pid21
        {
            get { return pid21; }
            set { pid21 = value; }
        }
        /// <summary>
        /// Campo 22 segmento PID
        /// </summary>
        public string Pid22
        {
            get { return pid22; }
            set { pid22 = value; }
        }
        /// <summary>
        /// Campo 23 segmento PID
        /// </summary>
        public string Pid23
        {
            get { return pid23; }
            set { pid23 = value; }
        }
        /// <summary>
        /// Campo 24 segmento PID
        /// </summary>
        public string Pid24
        {
            get { return pid24; }
            set { pid24 = value; }
        }
        /// <summary>
        /// Campo 25 segmento PID
        /// </summary>
        public string Pid25
        {
            get { return pid25; }
            set { pid25 = value; }
        }
        /// <summary>
        /// Campo 26 segmento PID
        /// </summary>
        public string Pid26
        {
            get { return pid26; }
            set { pid26 = value; }
        }
        /// <summary>
        /// Campo 27 segmento PID
        /// </summary>
        public string Pid27
        {
            get { return pid27; }
            set { pid27 = value; }
        }
        /// <summary>
        /// Campo 28 segmento PID
        /// </summary>
        public string Pid28
        {
            get { return pid28; }
            set { pid28 = value; }
        }
        /// <summary>
        /// Campo 29 segmento PID
        /// </summary>
        public string Pid29
        {
            get { return pid29; }
            set { pid29 = value; }
        }
        /// <summary>
        /// Campo 30 segmento PID
        /// </summary>
        public string Pid30
        {
            get { return pid30; }
            set { pid30 = value; }
        }

        #endregion
        /// <summary>
        /// Metodo que retorna todo el segmento PID en formato HL7
        /// </summary>
        /// <returns></returns>
        public string retornoPid()
        {
            string spr = "|";
            string valorPid = pid1.ToString() + spr + pid2.ToString() + spr + pid3.ToString() + spr + pid4.ToString() + spr + pid5.ToString() + spr + pid6.ToString() + spr + pid7.ToString() + spr + pid8.ToString() + spr + pid9.ToString() + spr + pid10.ToString() + spr;
            valorPid = valorPid + pid11.ToString() + spr + pid12.ToString() + spr + pid13.ToString() + spr + pid14.ToString() + spr + pid15.ToString() + spr + pid16.ToString() + spr + pid17.ToString() + spr + pid18.ToString() + spr + pid19.ToString() + spr + pid20.ToString() + spr;
            valorPid = valorPid + pid21.ToString() + spr + pid22.ToString() + spr + pid23.ToString() + spr + pid24.ToString() + spr + pid25.ToString() + spr + pid26.ToString() + spr + pid27.ToString() + spr + pid28.ToString() + spr + pid29.ToString() + spr + pid30.ToString();
            return valorPid;
        }
    }
    /// <summary>
    /// Clase para generar el segmento PV1 de HL7
    /// </summary>
    [DataContract]
    public class PV1Class
    {
        #region RegionDefLocalPV1
        private string pv11 = "PV1";
        private string pv12 = string.Empty;
        private string pv13 = string.Empty;
        private string pv14 = string.Empty;
        private string pv15 = string.Empty;
        private string pv16 = string.Empty;
        private string pv17 = string.Empty;
        private string pv18 = string.Empty;
        private string pv19 = string.Empty;
        private string pv110 = string.Empty;
        private string pv111 = string.Empty;
        private string pv112 = string.Empty;
        private string pv113 = string.Empty;
        private string pv114 = string.Empty;
        private string pv115 = string.Empty;
        private string pv116 = string.Empty;
        private string pv117 = string.Empty;
        private string pv118 = string.Empty;
        private string pv119 = string.Empty;
        private string pv120 = string.Empty;
        private string pv121 = string.Empty;
        private string pv122 = string.Empty;
        private string pv123 = string.Empty;
        private string pv124 = string.Empty;
        private string pv125 = string.Empty;
        private string pv126 = string.Empty;
        private string pv127 = string.Empty;
        private string pv128 = string.Empty;
        private string pv129 = string.Empty;
        private string pv130 = string.Empty;
        private string pv131 = string.Empty;
        private string pv132 = string.Empty;
        private string pv133 = string.Empty;
        private string pv134 = string.Empty;
        private string pv135 = string.Empty;
        private string pv136 = string.Empty;
        private string pv137 = string.Empty;
        private string pv138 = string.Empty;
        private string pv139 = string.Empty;
        private string pv140 = string.Empty;
        private string pv141 = string.Empty;
        private string pv142 = string.Empty;
        private string pv143 = string.Empty;
        private string pv144 = string.Empty;
        private string pv145 = string.Empty;
        private string pv146 = string.Empty;
        private string pv147 = string.Empty;
        private string pv148 = string.Empty;


        #endregion
        #region RegionDefPV1
        /// <summary>
        /// Campo Uno del Segmento PV1
        /// </summary>
        public string PV11
        {
            get { return pv11; }
            set { pv11 = value; }
        }
        /// <summary>
        /// Campo 2 Segmento PV1
        /// </summary>
        public string PV12
        {
            get { return pv12; }
            set { pv12 = value; }
        }
        /// <summary>
        /// Campo 3 del segmento PV1
        /// </summary>
        public string PV13
        {
            get { return pv13; }
            set { pv13 = value; }
        }
        /// <summary>
        /// Campo 4 del Segmento PV1
        /// </summary>
        public string PV14
        {
            get { return pv14; }
            set { pv14 = value; }
        }
        /// <summary>
        /// Campo 5 del Segmento PV1
        /// </summary>
        public string PV15
        {
            get { return pv15; }
            set { pv15 = value; }
        }
        /// <summary>
        /// Campo 6 del Segmento PV1
        /// </summary>
        public string PV16
        {
            get { return pv16; }
            set { pv16 = value; }
        }
        /// <summary>
        /// Campo 7 del Segmento PV1
        /// </summary>
        public string PV17
        {
            get { return pv17; }
            set { pv17 = value; }
        }
        /// <summary>
        /// Campo 8 del Segmento PV1
        /// </summary>
        public string PV18
        {
            get { return pv18; }
            set { pv18 = value; }
        }
        /// <summary>
        /// Campo * del segmento PV1
        /// </summary>
        public string PV19
        {
            get { return pv19; }
            set { pv19 = value; }
        }
        /// <summary>
        /// campo 10 del Segmento PV1
        /// </summary>
        public string PV110
        {
            get { return pv110; }
            set { pv110 = value; }
        }
        /// <summary>
        /// Campo 11 del Segmento PV1
        /// </summary>
        public string PV111
        {
            get { return pv111; }
            set { pv111 = value; }
        }
        /// <summary>
        /// Campo 12 del Segmento PV1
        /// </summary>
        public string PV112
        {
            get { return pv112; }
            set { pv112 = value; }
        }
        /// <summary>
        /// Campo 13 del Segmento PV1
        /// </summary>
        public string PV113
        {
            get { return pv113; }
            set { pv113 = value; }
        }
        /// <summary>
        /// Campo 14 del Segmento PV1
        /// </summary>
        public string PV114
        {
            get { return pv114; }
            set { pv114 = value; }
        }
        /// <summary>
        /// Campo 15 del Segmento PV1
        /// </summary>
        public string PV115
        {
            get { return pv115; }
            set { pv115 = value; }
        }
        /// <summary>
        /// Campo 16 del segmento PV1
        /// </summary>
        public string PV116
        {
            get { return pv116; }
            set { pv116 = value; }
        }
        /// <summary>
        /// Campo 17 del Segmento PV1
        /// </summary>
        public string PV117
        {
            get { return pv117; }
            set { pv117 = value; }
        }
        /// <summary>
        /// Campo 18 del Segmento PV1
        /// </summary>
        public string PV118
        {
            get { return pv118; }
            set { pv118 = value; }
        }
        /// <summary>
        /// Campo 19 del Segmento PV1
        /// </summary>
        public string PV119
        {
            get { return pv119; }
            set { pv119 = value; }
        }
        /// <summary>
        /// Campo 20 del Segmento PV1
        /// </summary>
        public string PV120
        {
            get { return pv120; }
            set { pv120 = value; }
        }
        /// <summary>
        /// Campo 21 del Segmento PV1
        /// </summary>
        public string PV121
        {
            get { return pv121; }
            set { pv121 = value; }
        }
        /// <summary>
        /// Campo 22 del Segmento PV1
        /// </summary>
        public string PV122
        {
            get { return pv122; }
            set { pv122 = value; }
        }
        /// <summary>
        /// Campo 23 del Segmento PV1
        /// </summary>
        public string PV123
        {
            get { return pv123; }
            set { pv123 = value; }
        }
        /// <summary>
        /// Campo 24 del Segmento PV1
        /// </summary>
        public string PV124
        {
            get { return pv124; }
            set { pv124 = value; }
        }
        /// <summary>
        /// Campo 25 del Segmento PV1
        /// </summary>
        public string PV125
        {
            get { return pv125; }
            set { pv125 = value; }
        }
        /// <summary>
        /// Campo 26 del Segmento PV1
        /// </summary>
        public string PV126
        {
            get { return pv126; }
            set { pv126 = value; }
        }
        /// <summary>
        /// Campo 27 del Segmento PV1
        /// </summary>
        public string PV127
        {
            get { return pv127; }
            set { pv127 = value; }
        }
        /// <summary>
        /// Campo 28 del Segmento PV1
        /// </summary>
        public string PV128
        {
            get { return pv128; }
            set { pv128 = value; }
        }
        /// <summary>
        /// Campo 29 del Segmento PV1
        /// </summary>
        public string PV129
        {
            get { return pv129; }
            set { pv129 = value; }
        }
        /// <summary>
        /// Campo 30 del Segmento PV1
        /// </summary>
        public string PV130
        {
            get { return pv130; }
            set { pv130 = value; }
        }
        /// <summary>
        /// Campo 31 del Segmento PV1
        /// </summary>
        public string PV131
        {
            get { return pv131; }
            set { pv131 = value; }
        }
        /// <summary>
        /// Campo 32 del Segmento PV1
        /// </summary>
        public string PV132
        {
            get { return pv132; }
            set { pv132 = value; }
        }
        /// <summary>
        /// Campo 33 del Segmento PV1
        /// </summary>
        public string PV133
        {
            get { return pv133; }
            set { pv133 = value; }
        }
        /// <summary>
        /// Campo 34 del Segmento PV1
        /// </summary>
        public string PV134
        {
            get { return pv134; }
            set { pv134 = value; }
        }
        /// <summary>
        /// Campo 35 del Segmento PV1
        /// </summary>
        public string PV135
        {
            get { return pv135; }
            set { pv135 = value; }
        }
        /// <summary>
        /// Campo 36 del Segmento PV1
        /// </summary>
        public string PV136
        {
            get { return pv136; }
            set { pv136 = value; }
        }
        /// <summary>
        /// Campo 37 del Segmento PV1
        /// </summary>
        public string PV137
        {
            get { return pv137; }
            set { pv137 = value; }
        }
        /// <summary>
        /// Campo 38 del Segmento PV1
        /// </summary>
        public string PV138
        {
            get { return pv138; }
            set { pv138 = value; }
        }
        /// <summary>
        /// Campo 39 del Segmento PV1
        /// </summary>
        public string PV139
        {
            get { return pv139; }
            set { pv139 = value; }
        }
        /// <summary>
        /// Campo 40 del Segmento PV1
        /// </summary>
        public string PV140
        {
            get { return pv140; }
            set { pv140 = value; }
        }
        /// <summary>
        /// Campo 41 del Segmento PV1
        /// </summary>
        public string PV141
        {
            get { return pv141; }
            set { pv141 = value; }
        }
        /// <summary>
        /// Campo 42 del Segmento PV1
        /// </summary>
        public string PV142
        {
            get { return pv142; }
            set { pv142 = value; }
        }
        /// <summary>
        /// Campo 43 del Segmento PV1
        /// </summary>
        public string PV143
        {
            get { return pv143; }
            set { pv143 = value; }
        }
        /// <summary>
        /// Campo 44 del Segmento PV1
        /// </summary>
        public string PV144
        {
            get { return pv144; }
            set { pv144 = value; }
        }
        /// <summary>
        /// Campo 45 del Segmento PV1
        /// </summary>
        public string PV145
        {
            get { return pv145; }
            set { pv145 = value; }
        }
        /// <summary>
        /// Campo 46 del Segmento PV1
        /// </summary>
        public string PV146
        {
            get { return pv146; }
            set { pv146 = value; }
        }
        /// <summary>
        /// Campo 47 del Segmento PV1
        /// </summary>
        public string PV147
        {
            get { return pv147; }
            set { pv147 = value; }
        }
        /// <summary>
        /// Campo 48 del Segmento PV1
        /// </summary>
        public string PV148
        {
            get { return pv148; }
            set { pv148 = value; }
        }



        #endregion
        /// <summary>
        /// Constructor de  la clase para el Segmento PV1
        /// </summary>
        public PV1Class()
        {

        }
        /// <summary>
        /// Contructor para recibir parametros de la clase, para el segmento PV1
        /// </summary>
        /// <param name="segPV1"></param>
        public PV1Class(string[] segPV1)
        {
            Utilidades utls = new Utilidades();
            pv11 = segPV1[0];
            pv12 = segPV1[1];
            pv13 = segPV1[2];
            pv14 = segPV1[3];
            pv15 = segPV1[4];
            pv16 = segPV1[5];
            pv17 = segPV1[6];
            pv18 = segPV1[7];
            pv19 = segPV1[8];
            pv110 = segPV1[9];
            pv111 = segPV1[10];
            pv112 = segPV1[11];
            pv113 = segPV1[12];
            pv114 = segPV1[13];
            pv115 = segPV1[14];
            pv116 = segPV1[15];
            pv117 = segPV1[16];
            pv118 = segPV1[17];
            pv119 = segPV1[18];
            pv120 = segPV1[19];
            pv121 = segPV1[20];
            pv122 = segPV1[21];
            pv123 = segPV1[22];
            pv124 = segPV1[23];
            pv125 = segPV1[24];
            pv126 = segPV1[25];
            pv127 = segPV1[26];
            pv128 = segPV1[27];
            pv129 = segPV1[28];
            pv130 = segPV1[29];
            pv131 = segPV1[30];
            pv132 = segPV1[31];
            pv133 = segPV1[32];
            pv134 = segPV1[33];
            pv135 = segPV1[34];
            pv136 = segPV1[35];
            pv137 = segPV1[36];
            pv138 = segPV1[37];
            pv139 = segPV1[38];
            pv140 = segPV1[39];
            pv141 = segPV1[40];
            pv142 = segPV1[41];
            pv143 = segPV1[42];
            pv144 = segPV1[43];
            pv145 = segPV1[44];
            pv146 = segPV1[45];
            pv147 = segPV1[46];
            pv148 = segPV1[47];
        }

        /// <summary>
        /// Metodo que retorna el Segmento PV1 armado
        /// </summary>
        /// <returns></returns>
        public string retornoPV1()
        {
            string spr = "|";
            string valorPv1 = pv11.ToString() + spr + pv12.ToString() + spr + pv13.ToString() + spr + pv14.ToString() + spr + pv15.ToString() + spr + pv16.ToString() + spr + pv17.ToString() + spr + pv18.ToString() + spr + pv19.ToString() + spr + pv110.ToString() + spr;
            valorPv1 = valorPv1 + pv111.ToString() + spr + pv112.ToString() + spr + pv113.ToString() + spr + pv114.ToString() + spr + pv115.ToString() + spr + pv116.ToString() + spr + pv117.ToString() + spr + pv118.ToString() + spr + pv119.ToString() + spr + pv120.ToString() + spr;
            valorPv1 = valorPv1 + pv121.ToString() + spr + pv122.ToString() + spr + pv123.ToString() + spr + pv124.ToString() + spr + pv125.ToString() + spr + pv126.ToString() + spr + pv127.ToString() + spr + pv128.ToString() + spr + pv129.ToString() + spr + pv130.ToString() + spr;
            valorPv1 = valorPv1 + pv131.ToString() + spr + pv132.ToString() + spr + pv133.ToString() + spr + pv134.ToString() + spr + pv135.ToString() + spr + pv136.ToString() + spr + pv137.ToString() + spr + pv138.ToString() + spr + pv139.ToString() + spr + pv140.ToString() + spr;
            valorPv1 = valorPv1 + pv141.ToString() + spr + pv142.ToString() + spr + pv143.ToString() + spr + pv144.ToString() + spr + pv145.ToString() + spr + pv146.ToString() + spr + pv147.ToString() + spr + pv148.ToString();
            return valorPv1;
        }
    }
    /// <summary>
    /// Clase para generar el segmento PV2 de HL7
    /// </summary>
    [DataContract]
    public class PV2Class
    {
        #region RegionDefLocalPV2
        private string pv21 = "PV2";
        private string pv22 = string.Empty;
        private string pv23 = string.Empty;
        private string pv24 = string.Empty;
        private string pv25 = string.Empty;
        private string pv26 = string.Empty;
        private string pv27 = string.Empty;
        private string pv28 = string.Empty;
        private string pv29 = string.Empty;
        private string pv210 = string.Empty;
        private string pv211 = string.Empty;
        private string pv212 = string.Empty;
        private string pv213 = string.Empty;
        private string pv214 = string.Empty;
        private string pv215 = string.Empty;
        private string pv216 = string.Empty;
        private string pv217 = string.Empty;
        private string pv218 = string.Empty;
        private string pv219 = string.Empty;
        private string pv220 = string.Empty;
        private string pv221 = string.Empty;
        private string pv222 = string.Empty;
        private string pv223 = string.Empty;
        private string pv224 = string.Empty;
        private string pv225 = string.Empty;
        private string pv226 = string.Empty;
        private string pv227 = string.Empty;
        private string pv228 = string.Empty;
        private string pv229 = string.Empty;
        private string pv230 = string.Empty;
        private string pv231 = string.Empty;
        private string pv232 = string.Empty;
        private string pv233 = string.Empty;
        private string pv234 = string.Empty;
        private string pv235 = string.Empty;
        private string pv236 = string.Empty;
        private string pv237 = string.Empty;
        private string pv238 = string.Empty;
        private string pv239 = string.Empty;
        private string pv240 = string.Empty;
        private string pv241 = string.Empty;
        private string pv242 = string.Empty;
        private string pv243 = string.Empty;
        private string pv244 = string.Empty;
        private string pv245 = string.Empty;
        private string pv246 = string.Empty;
        private string pv247 = string.Empty;
        private string pv248 = string.Empty;
        #endregion
        #region RegionDefPV2
        public string PV21
        {
            get { return pv21; }
            set { pv21 = value; }
        }
        public string PV22
        {
            get { return pv22; }
            set { pv22 = value; }
        }
        public string PV23
        {
            get { return pv23; }
            set { pv23 = value; }
        }
        public string PV24
        {
            get { return pv24; }
            set { pv24 = value; }
        }
        public string PV25
        {
            get { return pv25; }
            set { pv25 = value; }
        }
        public string PV26
        {
            get { return pv26; }
            set { pv26 = value; }
        }
        public string PV27
        {
            get { return pv27; }
            set { pv27 = value; }
        }
        public string PV28
        {
            get { return pv28; }
            set { pv28 = value; }
        }
        public string PV29
        {
            get { return pv29; }
            set { pv29 = value; }
        }
        public string PV210
        {
            get { return pv210; }
            set { pv210 = value; }
        }
        public string PV211
        {
            get { return pv211; }
            set { pv211 = value; }
        }
        public string PV212
        {
            get { return pv212; }
            set { pv212 = value; }
        }
        public string PV213
        {
            get { return pv213; }
            set { pv213 = value; }
        }
        public string PV214
        {
            get { return pv214; }
            set { pv214 = value; }
        }
        public string PV215
        {
            get { return pv215; }
            set { pv215 = value; }
        }
        public string PV216
        {
            get { return pv216; }
            set { pv216 = value; }
        }
        public string PV217
        {
            get { return pv217; }
            set { pv217 = value; }
        }
        public string PV218
        {
            get { return pv218; }
            set { pv218 = value; }
        }
        public string PV219
        {
            get { return pv219; }
            set { pv219 = value; }
        }
        public string PV220
        {
            get { return pv220; }
            set { pv220 = value; }
        }
        public string PV221
        {
            get { return pv221; }
            set { pv221 = value; }
        }
        public string PV222
        {
            get { return pv222; }
            set { pv222 = value; }
        }
        public string PV223
        {
            get { return pv223; }
            set { pv223 = value; }
        }
        public string PV224
        {
            get { return pv224; }
            set { pv224 = value; }
        }
        public string PV225
        {
            get { return pv225; }
            set { pv225 = value; }
        }
        public string PV226
        {
            get { return pv226; }
            set { pv226 = value; }
        }
        public string PV227
        {
            get { return pv227; }
            set { pv227 = value; }
        }
        public string PV228
        {
            get { return pv228; }
            set { pv228 = value; }
        }
        public string PV229
        {
            get { return pv229; }
            set { pv229 = value; }
        }
        public string PV230
        {
            get { return pv230; }
            set { pv230 = value; }
        }
        public string PV231
        {
            get { return pv231; }
            set { pv231 = value; }
        }
        public string PV232
        {
            get { return pv232; }
            set { pv232 = value; }
        }
        public string PV233
        {
            get { return pv233; }
            set { pv233 = value; }
        }
        public string PV234
        {
            get { return pv234; }
            set { pv234 = value; }
        }
        public string PV235
        {
            get { return pv235; }
            set { pv235 = value; }
        }
        public string PV236
        {
            get { return pv236; }
            set { pv236 = value; }
        }
        public string PV237
        {
            get { return pv237; }
            set { pv237 = value; }
        }
        public string PV238
        {
            get { return pv238; }
            set { pv238 = value; }
        }
        public string PV239
        {
            get { return pv239; }
            set { pv239 = value; }
        }
        public string PV240
        {
            get { return pv240; }
            set { pv240 = value; }
        }
        public string PV241
        {
            get { return pv241; }
            set { pv241 = value; }
        }
        public string PV242
        {
            get { return pv242; }
            set { pv242 = value; }
        }
        public string PV243
        {
            get { return pv243; }
            set { pv243 = value; }
        }
        public string PV244
        {
            get { return pv244; }
            set { pv244 = value; }
        }
        public string PV245
        {
            get { return pv245; }
            set { pv245 = value; }
        }
        public string PV246
        {
            get { return pv246; }
            set { pv246 = value; }
        }
        public string PV247
        {
            get { return pv247; }
            set { pv247 = value; }
        }
        public string PV248
        {
            get { return pv248; }
            set { pv248 = value; }
        }


        #endregion

        public PV2Class()
        {

        }

        public PV2Class(string[] segPV2)
        {
            Utilidades utls = new Utilidades();
            pv21 = segPV2[0];
            pv22 = segPV2[1];
            pv23 = segPV2[2];
            pv24 = segPV2[3];
            pv25 = segPV2[4];
            pv26 = segPV2[5];
            pv27 = segPV2[6];
            pv28 = segPV2[7];
            pv29 = segPV2[8];
            pv210 = segPV2[9];
            pv211 = segPV2[10];
            pv212 = segPV2[11];
            pv213 = segPV2[12];
            pv214 = segPV2[13];
            pv215 = segPV2[14];
            pv216 = segPV2[15];
            pv217 = segPV2[16];
            pv218 = segPV2[17];
            pv219 = segPV2[18];
            pv220 = segPV2[19];
            pv221 = segPV2[20];
            pv222 = segPV2[21];
            pv223 = segPV2[22];
            pv224 = segPV2[23];
            pv225 = segPV2[24];
            pv226 = segPV2[25];
            pv227 = segPV2[26];
            pv228 = segPV2[27];
            pv229 = segPV2[28];
            pv230 = segPV2[29];
            pv231 = segPV2[30];
            pv232 = segPV2[31];
            pv233 = segPV2[32];
            pv234 = segPV2[33];
            pv235 = segPV2[34];
            pv236 = segPV2[35];
            pv237 = segPV2[36];
            pv238 = segPV2[37];
            pv239 = segPV2[38];
            pv240 = segPV2[39];
            pv241 = segPV2[40];
            pv242 = segPV2[41];
            pv243 = segPV2[42];
            pv244 = segPV2[43];
            pv245 = segPV2[44];
            pv246 = segPV2[45];
            pv247 = segPV2[46];
            pv248 = segPV2[47];

        }

        public string retornoPV2()
        {
            string spr = "|";
            string valorPv2 = pv21.ToString() + spr + pv22.ToString() + spr + pv23.ToString() + spr + pv24.ToString() + spr + pv25.ToString() + spr + pv26.ToString() + spr + pv27.ToString() + spr + pv28.ToString() + spr + pv29.ToString() + spr + pv210.ToString() + spr;
            valorPv2 = valorPv2 + pv211.ToString() + spr + pv212.ToString() + spr + pv213.ToString() + spr + pv214.ToString() + spr + pv215.ToString() + spr + pv216.ToString() + spr + pv217.ToString() + spr + pv218.ToString() + spr + pv219.ToString() + spr + pv220.ToString() + spr;
            valorPv2 = valorPv2 + pv221.ToString() + spr + pv222.ToString() + spr + pv223.ToString() + spr + pv224.ToString() + spr + pv225.ToString() + spr + pv226.ToString() + spr + pv227.ToString() + spr + pv228.ToString() + spr + pv229.ToString() + spr + pv230.ToString() + spr;
            valorPv2 = valorPv2 + pv231.ToString() + spr + pv232.ToString() + spr + pv233.ToString() + spr + pv234.ToString() + spr + pv235.ToString() + spr + pv236.ToString() + spr + pv237.ToString() + spr + pv238.ToString() + spr + pv239.ToString() + spr + pv240.ToString() + spr;
            valorPv2 = valorPv2 + pv241.ToString() + spr + pv242.ToString() + spr + pv243.ToString() + spr + pv244.ToString() + spr + pv245.ToString() + spr + pv246.ToString() + spr + pv247.ToString() + spr + pv248.ToString();
            return valorPv2;
        }
    }
    /// <summary>
    /// Clase para generar el segmento IN1 de HL7
    /// </summary>
    [DataContract]
    public class IN1Class
    {
        #region RegionDefLocalIN1
        private string in11 = "IN1";
        private string in12 = string.Empty;
        private string in13 = string.Empty;
        private string in14 = string.Empty;
        private string in15 = string.Empty;
        private string in16 = string.Empty;
        private string in17 = string.Empty;
        private string in18 = string.Empty;
        private string in19 = string.Empty;
        private string in110 = string.Empty;
        private string in111 = string.Empty;
        private string in112 = string.Empty;
        private string in113 = string.Empty;
        private string in114 = string.Empty;
        private string in115 = string.Empty;
        private string in116 = string.Empty;
        private string in117 = string.Empty;
        private string in118 = string.Empty;
        private string in119 = string.Empty;
        private string in120 = string.Empty;
        #endregion

        #region RegionDefIN1
        public string IN11
        {
            get { return in11; }
            set { in11 = value; }
        }
        public string IN12
        {
            get { return in12; }
            set { in12 = value; }
        }
        public string IN13
        {
            get { return in13; }
            set { in13 = value; }
        }
        public string IN14
        {
            get { return in14; }
            set { in14 = value; }
        }
        public string IN15
        {
            get { return in15; }
            set { in15 = value; }
        }
        public string IN16
        {
            get { return in16; }
            set { in16 = value; }
        }
        public string IN17
        {
            get { return in17; }
            set { in17 = value; }
        }
        public string IN18
        {
            get { return in18; }
            set { in18 = value; }
        }
        public string IN19
        {
            get { return in19; }
            set { in19 = value; }
        }
        public string IN110
        {
            get { return in110; }
            set { in110 = value; }
        }
        public string IN111
        {
            get { return in111; }
            set { in111 = value; }
        }
        public string IN112
        {
            get { return in112; }
            set { in112 = value; }
        }
        public string IN113
        {
            get { return in113; }
            set { in113 = value; }
        }
        public string IN114
        {
            get { return in114; }
            set { in114 = value; }
        }
        public string IN115
        {
            get { return in115; }
            set { in115 = value; }
        }
        public string IN116
        {
            get { return in116; }
            set { in116 = value; }
        }
        public string IN117
        {
            get { return in117; }
            set { in117 = value; }
        }
        public string IN118
        {
            get { return in118; }
            set { in118 = value; }
        }
        public string IN119
        {
            get { return in119; }
            set { in19 = value; }
        }
        public string IN120
        {
            get { return in120; }
            set { in120 = value; }
        }
        #endregion

        public IN1Class()
        {

        }

        public IN1Class(string[] segIN1)
        {
            Utilidades utls = new Utilidades();
            in11 = segIN1[0];
            in12 = segIN1[1];
            in13 = segIN1[2];
            in14 = segIN1[3];
            in15 = segIN1[4];
            in16 = segIN1[5];
            in17 = segIN1[6];
            in18 = segIN1[7];
            in19 = segIN1[8];
            in110 = segIN1[9];
            in111 = segIN1[10];
            in112 = segIN1[11];
            in113 = segIN1[12];
            in114 = segIN1[13];
            in115 = segIN1[14];
            in116 = segIN1[15];
            in117 = segIN1[16];
            in118 = segIN1[17];
            in119 = segIN1[18];
            in120 = segIN1[19];

        }
        public string retornoIN1()
        {
            string spr = "|";
            string valorIn1 = in11.ToString() + spr + in12.ToString() + spr + in13.ToString() + spr + in14.ToString() + spr + in15.ToString() + spr + in16.ToString() + spr + in17.ToString() + spr + in18.ToString() + spr + in19.ToString() + spr + in110.ToString() + spr;
            valorIn1 = valorIn1 + in111.ToString() + spr + in112.ToString() + spr + in113.ToString() + spr + in114.ToString() + spr + in115.ToString() + spr + in116.ToString() + spr + in117.ToString() + in118.ToString() + spr + in119.ToString() + spr + in120.ToString() + spr;
            return valorIn1;
        }

    }
    /// <summary>
    /// Clase para generar el segmento ORC de HL7
    /// </summary>
    [DataContract]
    public class ORCClass
    {
        #region RegionLocalesORC
        /// <summary>
        /// Nombre de Segmento
        /// </summary>
        private string orc1 = "ORC";
        /// <summary>
        /// Evento
        /// </summary>
        private string orc2 = string.Empty;
        /// <summary>
        /// Numero Labcore
        /// </summary>
        private string orc3 = string.Empty;
        /// <summary>
        /// Numero de solicitud de enfermeria SAHI
        /// </summary>
        private string orc4 = string.Empty;
        /// <summary>
        /// Numero de Atencion
        /// </summary>
        private string orc5 = string.Empty;
        /// <summary>
        /// Operacion informada
        /// </summary>
        private string orc6 = string.Empty;
        private string orc7 = string.Empty;
        private string orc8 = string.Empty;
        private string orc9 = string.Empty;
        private string orc10 = string.Empty;
        private string orc11 = string.Empty;
        private string orc12 = string.Empty;
        private string orc13 = string.Empty;
        private string orc14 = string.Empty;
        private string orc15 = string.Empty;
        private string orc16 = string.Empty;
        private string orc17 = string.Empty;
        private string orc18 = string.Empty;
        private string orc19 = string.Empty;
        private string orc20 = string.Empty;
        #endregion

        #region MyRegionDefORC
        public string ORC1
        {
            get { return orc1; }
            set { orc1 = value; }
        }
        public string ORC2
        {
            get { return orc2; }
            set { orc2 = value; }
        }
        public string ORC3
        {
            get { return orc3; }
            set { orc3 = value; }
        }
        public string ORC4
        {
            get { return orc4; }
            set { orc4 = value; }
        }
        public string ORC5
        {
            get { return orc5; }
            set { orc5 = value; }
        }
        public string ORC6
        {
            get { return orc6; }
            set { orc6 = value; }
        }
        public string ORC7
        {
            get { return orc7; }
            set { orc7 = value; }
        }
        public string ORC8
        {
            get { return orc8; }
            set { orc8 = value; }
        }
        public string ORC9
        {
            get { return orc9; }
            set { orc9 = value; }
        }
        public string ORC10
        {
            get { return orc10; }
            set { orc10 = value; }
        }
        public string ORC11
        {
            get { return orc11; }
            set { orc11 = value; }
        }
        public string ORC12
        {
            get { return orc12; }
            set { orc12 = value; }
        }
        public string ORC13
        {
            get { return orc13; }
            set { orc13 = value; }
        }
        public string ORC14
        {
            get { return orc14; }
            set { orc14 = value; }
        }
        public string ORC15
        {
            get { return orc15; }
            set { orc15 = value; }
        }
        public string ORC16
        {
            get { return orc16; }
            set { orc16 = value; }
        }
        public string ORC17
        {
            get { return orc17; }
            set { orc17 = value; }
        }
        public string ORC18
        {
            get { return orc18; }
            set { orc18 = value; }
        }
        public string ORC19
        {
            get { return orc19; }
            set { orc19 = value; }
        }
        public string ORC20
        {
            get { return orc20; }
            set { orc20 = value; }
        }
        #endregion

        public ORCClass()
        { }

        public ORCClass(string[] segORC)
        {
            Utilidades utls = new Utilidades();
            orc1 = segORC[0];
            orc2 = segORC[1];
            orc3 = segORC[2];
            orc4 = segORC[3];
            orc5 = segORC[4];
            orc6 = segORC[5];
            orc7 = segORC[6];
            orc8 = segORC[7];
            orc9 = segORC[8];
            orc10 = segORC[9];
            orc11 = segORC[10];
            orc12 = segORC[11];
            orc13 = segORC[12];
            orc14 = segORC[13];
            orc15 = segORC[14];
            orc16 = segORC[15];
            orc17 = segORC[16];
            orc18 = segORC[17];
            orc19 = segORC[18];
            orc20 = segORC[19];
        }
        public string retornoORC()
        {
            string spr = "|";
            string valorORC = orc1.ToString() + spr + orc2.ToString() + spr + orc3.ToString() + spr + orc4.ToString() + spr + orc5.ToString() + spr + orc6.ToString() + spr + orc7.ToString() + spr + orc8.ToString() + spr + orc9.ToString() + spr + orc10.ToString() + spr;
            valorORC = valorORC + orc11.ToString() + spr + orc12.ToString() + spr + orc13.ToString() + spr + orc14.ToString() + spr + orc15.ToString() + spr + orc16.ToString() + spr + orc17.ToString() + spr + orc18.ToString() + spr + orc19.ToString() + spr + orc20.ToString();
            return valorORC;
        }

    }
    /// <summary>
    /// Clase para generar el segmento OBR de HL7
    /// </summary>
    [DataContract]
    public class OBRClass
    {
        #region RegionDelLocalesOBR
        private string obr1 = "OBR";
        private string obr2 = string.Empty;
        private string obr3 = string.Empty;
        private string obr4 = string.Empty;
        private string obr5 = string.Empty;
        private string obr6 = string.Empty;
        private string obr7 = string.Empty;
        private string obr8 = string.Empty;
        private string obr9 = string.Empty;
        private string obr10 = string.Empty;
        private string obr11 = string.Empty;
        private string obr12 = string.Empty;
        private string obr13 = string.Empty;
        private string obr14 = string.Empty;
        private string obr15 = string.Empty;
        private string obr16 = string.Empty;
        private string obr17 = string.Empty;
        private string obr18 = string.Empty;
        private string obr19 = string.Empty;
        private string obr20 = string.Empty;
        private string obr21 = string.Empty;
        private string obr22 = string.Empty;
        private string obr23 = string.Empty;
        private string obr24 = string.Empty;
        private string obr25 = string.Empty;
        private string obr26 = string.Empty;
        private string obr27 = string.Empty;
        private string obr28 = string.Empty;
        private string obr29 = string.Empty;
        private string obr30 = string.Empty;
        private string obr31 = string.Empty;
        private string obr32 = string.Empty;
        private string obr33 = string.Empty;
        private string obr34 = string.Empty;
        private string obr35 = string.Empty;
        private string obr36 = string.Empty;
        private string obr37 = string.Empty;
        private string obr38 = string.Empty;
        private string obr39 = string.Empty;
        private string obr40 = string.Empty;
        private string obr41 = string.Empty;
        private string obr42 = string.Empty;
        private string obr43 = string.Empty;

        #endregion

        #region RegionDefOBR
        public string OBR1
        {
            get { return obr1; }
            set { obr1 = value; }
        }
        public string OBR2
        {
            get { return obr2; }
            set { obr2 = value; }
        }
        public string OBR3
        {
            get { return obr3; }
            set { obr3 = value; }
        }
        public string OBR4
        {
            get { return obr4; }
            set { obr4 = value; }
        }
        public string OBR5
        {
            get { return obr5; }
            set { obr5 = value; }
        }
        public string OBR6
        {
            get { return obr6; }
            set { obr6 = value; }
        }
        public string OBR7
        {
            get { return obr7; }
            set { obr7 = value; }
        }
        public string OBR8
        {
            get { return obr8; }
            set { obr8 = value; }
        }
        public string OBR9
        {
            get { return obr9; }
            set { obr9 = value; }
        }
        public string OBR10
        {
            get { return obr10; }
            set { obr10 = value; }
        }
        public string OBR11
        {
            get { return obr11; }
            set { obr11 = value; }
        }
        public string OBR12
        {
            get { return obr12; }
            set { obr12 = value; }
        }
        public string OBR13
        {
            get { return obr13; }
            set { obr13 = value; }
        }
        public string OBR14
        {
            get { return obr14; }
            set { obr14 = value; }
        }
        public string OBR15
        {
            get { return obr15; }
            set { obr15 = value; }
        }
        public string OBR16
        {
            get { return obr16; }
            set { obr16 = value; }
        }
        public string OBR17
        {
            get { return obr17; }
            set { obr17 = value; }
        }
        public string OBR18
        {
            get { return obr18; }
            set { obr18 = value; }
        }
        public string OBR19
        {
            get { return obr19; }
            set { obr19 = value; }
        }
        public string OBR20
        {
            get { return obr20; }
            set { obr20 = value; }
        }
        public string OBR21
        {
            get { return obr21; }
            set { obr21 = value; }
        }
        public string OBR22
        {
            get { return obr22; }
            set { obr22 = value; }
        }
        public string OBR23
        {
            get { return obr23; }
            set { obr23 = value; }
        }
        public string OBR24
        {
            get { return obr24; }
            set { obr24 = value; }
        }
        public string OBR25
        {
            get { return obr25; }
            set { obr25 = value; }
        }
        public string OBR26
        {
            get { return obr26; }
            set { obr26 = value; }
        }
        public string OBR27
        {
            get { return obr27; }
            set { obr27 = value; }
        }
        public string OBR28
        {
            get { return obr28; }
            set { obr28 = value; }
        }
        public string OBR29
        {
            get { return obr29; }
            set { obr29 = value; }
        }
        public string OBR30
        {
            get { return obr30; }
            set { obr30 = value; }
        }
        public string OBR31
        {
            get { return obr31; }
            set { obr31 = value; }
        }
        public string OBR32
        {
            get { return obr32; }
            set { obr32 = value; }
        }
        public string OBR33
        {
            get { return obr33; }
            set { obr33 = value; }
        }
        public string OBR34
        {
            get { return obr34; }
            set { obr34 = value; }
        }
        public string OBR35
        {
            get { return obr35; }
            set { obr35 = value; }
        }
        public string OBR36
        {
            get { return obr36; }
            set { obr36 = value; }
        }
        public string OBR37
        {
            get { return obr37; }
            set { obr37 = value; }
        }
        public string OBR38
        {
            get { return obr38; }
            set { obr38 = value; }
        }
        public string OBR39
        {
            get { return obr39; }
            set { obr39 = value; }
        }
        public string OBR40
        {
            get { return obr40; }
            set { obr40 = value; }
        }
        public string OBR41
        {
            get { return obr41; }
            set { obr41 = value; }
        }
        public string OBR42
        {
            get { return obr42; }
            set { obr42 = value; }
        }
        public string OBR43
        {
            get { return obr43; }
            set { obr43 = value; }
        }


        #endregion

        public OBRClass()
        { }

        public OBRClass(string[] segOBR)
        {
            Utilidades utls = new Utilidades();
            obr1 = segOBR[0];
            obr2 = segOBR[1];
            obr3 = segOBR[2];
            obr4 = segOBR[3];
            obr5 = segOBR[4];
            obr6 = segOBR[5];
            obr7 = segOBR[6];
            obr8 = segOBR[7];
            obr9 = segOBR[8];
            obr10 = segOBR[9];
            obr11 = segOBR[10];
            obr12 = segOBR[11];
            obr13 = segOBR[12];
            obr14 = segOBR[13];
            obr15 = segOBR[14];
            obr16 = segOBR[15];
            obr17 = segOBR[16];
            obr18 = segOBR[17];
            obr19 = segOBR[18];
            obr20 = segOBR[19];
            obr21 = segOBR[20];
            obr22 = segOBR[21];
            obr23 = segOBR[22];
            obr24 = segOBR[23];
            obr25 = segOBR[24];
            obr26 = segOBR[25];
            obr27 = segOBR[26];
            obr28 = segOBR[27];
            obr29 = segOBR[28];
            obr30 = segOBR[29];
            obr31 = segOBR[30];
            obr32 = segOBR[31];
            obr33 = segOBR[32];
            obr34 = segOBR[33];
            obr35 = segOBR[34];
            obr36 = segOBR[35];
            obr37 = segOBR[36];
            obr38 = segOBR[37];
            obr39 = segOBR[38];
            obr40 = segOBR[39];
            obr41 = segOBR[40];
            obr42 = segOBR[41];
            obr43 = segOBR[42];
        }
        public string retornoOBR()
        {
            string spr = "|";
            string valorOBR = obr1.ToString() + spr + obr2.ToString() + spr + obr3.ToString() + spr + obr4.ToString() + spr + obr5.ToString() + spr + obr6.ToString() + spr + obr7.ToString() + spr + obr8.ToString() + spr + obr9.ToString() + spr + obr10.ToString() + spr;
            valorOBR = valorOBR + obr11.ToString() + spr + obr12.ToString() + spr + obr13.ToString() + spr + obr14.ToString() + spr + obr15.ToString() + spr + obr16.ToString() + spr + obr17.ToString() + spr + obr18.ToString() + spr + obr19.ToString() + spr + obr20.ToString() + spr;
            valorOBR = valorOBR + obr21.ToString() + spr + obr22.ToString() + spr + obr23.ToString() + spr + obr24.ToString() + spr + obr25.ToString() + spr + obr26.ToString() + spr + obr27.ToString() + spr + obr28.ToString() + spr + obr29.ToString() + spr + obr30.ToString() + spr;
            valorOBR = valorOBR + obr31.ToString() + spr + obr32.ToString() + spr + obr33.ToString() + spr + obr34.ToString() + spr + obr35.ToString() + spr + obr36.ToString() + spr + obr37.ToString() + spr + obr38.ToString() + spr + obr39.ToString() + spr + obr40.ToString() + spr;
            valorOBR = valorOBR + obr41.ToString() + spr + obr42.ToString() + spr + obr43.ToString();
            return valorOBR;
        }
    }
    /// <summary>
    /// Clase para generar el segmento OBX de HL7
    /// </summary>
    [DataContract]
    public class OBXClass
    {
        #region RegionDelLocalesOBR
        private string obx1 = "OBX";
        private string obx2 = string.Empty;
        private string obx3 = string.Empty;
        private string obx4 = string.Empty;
        private string obx5 = string.Empty;
        private string obx6 = string.Empty;
        private string obx7 = string.Empty;
        private string obx8 = string.Empty;
        private string obx9 = string.Empty;
        private string obx10 = string.Empty;
        private string obx11 = string.Empty;
        private string obx12 = string.Empty;
        private string obx13 = string.Empty;
        private string obx14 = string.Empty;
        private string obx15 = string.Empty;
        //private string obx16 = string.Empty;
        //private string obx17 = string.Empty;
        //private string obx18 = string.Empty;
        //private string obx19 = string.Empty;
        //private string obx20 = string.Empty;
        //private string obx21 = string.Empty;
        //private string obx22 = string.Empty;
        //private string obx23 = string.Empty;
        //private string obx24 = string.Empty;
        //private string obx25 = string.Empty;
        //private string obx26 = string.Empty;
        //private string obx27 = string.Empty;
        //private string obx28 = string.Empty;
        //private string obx29 = string.Empty;
        //private string obx30 = string.Empty;
        #endregion

        #region RegionDefOBX
        public string OBX1
        {
            get { return obx1; }
            set { obx1 = value; }
        }
        public string OBX2
        {
            get { return obx2; }
            set { obx2 = value; }
        }
        public string OBX3
        {
            get { return obx3; }
            set { obx3 = value; }
        }
        public string OBX4
        {
            get { return obx4; }
            set { obx4 = value; }
        }
        public string OBX5
        {
            get { return obx5; }
            set { obx5 = value; }
        }
        public string OBX6
        {
            get { return obx6; }
            set { obx6 = value; }
        }
        public string OBX7
        {
            get { return obx7; }
            set { obx7 = value; }
        }
        public string OBX8
        {
            get { return obx8; }
            set { obx8 = value; }
        }
        public string OBX9
        {
            get { return obx9; }
            set { obx9 = value; }
        }
        public string OBX10
        {
            get { return obx10; }
            set { obx10 = value; }
        }
        public string OBX11
        {
            get { return obx11; }
            set { obx11 = value; }
        }
        public string OBX12
        {
            get { return obx12; }
            set { obx12 = value; }
        }
        public string OBX13
        {
            get { return obx13; }
            set { obx13 = value; }
        }
        public string OBX14
        {
            get { return obx14; }
            set { obx14 = value; }
        }
        public string OBX15
        {
            get { return obx15; }
            set { obx15 = value; }
        }
        //public string OBX16
        //{
        //    get { return obx16; }
        //    set { obx16 = value; }
        //}
        //public string OBX17
        //{
        //    get { return obx17; }
        //    set { obx17 = value; }
        //}
        //public string OBX18
        //{
        //    get { return obx18; }
        //    set { obx18 = value; }
        //}
        //public string OBX19
        //{
        //    get { return obx19; }
        //    set { obx19 = value; }
        //}
        //public string OBX20
        //{
        //    get { return obx20; }
        //    set { obx20 = value; }
        //}
        //public string OBX21
        //{
        //    get { return obx21; }
        //    set { obx21 = value; }
        //}
        //public string OBX22
        //{
        //    get { return obx22; }
        //    set { obx22 = value; }
        //}
        //public string OBX23
        //{
        //    get { return obx23; }
        //    set { obx23 = value; }
        //}
        //public string OBX24
        //{
        //    get { return obx24; }
        //    set { obx24 = value; }
        //}
        //public string OBX25
        //{
        //    get { return obx25; }
        //    set { obx25 = value; }
        //}
        //public string OBX26
        //{
        //    get { return obx26; }
        //    set { obx26 = value; }
        //}
        //public string OBX27
        //{
        //    get { return obx27; }
        //    set { obx27 = value; }
        //}
        //public string OBX28
        //{
        //    get { return obx28; }
        //    set { obx28 = value; }
        //}
        //public string OBX29
        //{
        //    get { return obx29; }
        //    set { obx29 = value; }
        //}
        //public string OBX30
        //{
        //    get { return obx30; }
        //    set { obx30 = value; }
        //}
        #endregion

        public OBXClass()
        { }

        public OBXClass(string[] segOBX)
        {
            Utilidades utls = new Utilidades();
            obx1 = segOBX[0];
            obx2 = segOBX[1];
            obx3 = segOBX[2];
            obx4 = segOBX[3];
            obx5 = segOBX[4];
            obx6 = segOBX[5];
            obx7 = segOBX[6];
            obx8 = segOBX[7];
            obx9 = segOBX[8];
            obx10 = segOBX[9];
            obx11 = segOBX[10];
            obx12 = segOBX[11];
            obx13 = segOBX[12];
            obx14 = segOBX[13];
            obx15 = segOBX[14];
            //obx16 = segOBX[15];
            //obx17 = segOBX[16];
            //obx18 = segOBX[17];
            //obx19 = segOBX[18];
            //obx20 = segOBX[19];
            //obx21 = segOBX[20];
            //obx22 = segOBX[21];
            //obx23 = segOBX[22];
            //obx24 = segOBX[23];
            //obx25 = segOBX[24];
            //obx26 = segOBX[25];
            //obx27 = segOBX[26];
            //obx28 = segOBX[27];
            //obx29 = segOBX[28];
            //obx30 = segOBX[29];
        }
        public string retornoOBX()
        {
            string spr = "|";
            string valorOBX = obx1.ToString() + spr + obx2.ToString() + spr + obx3.ToString() + spr + obx4.ToString() + spr + obx5.ToString() + spr + obx6.ToString() + spr + obx7.ToString() + spr + obx8.ToString() + spr + obx9.ToString() + spr + obx10.ToString() + spr;
            valorOBX = valorOBX + obx11.ToString() + spr + obx12.ToString() + spr + obx13.ToString() + spr + obx14.ToString() + spr + obx15.ToString();// +spr + obx16.ToString() + spr + obx17.ToString() + spr + obx18.ToString() + spr + obx19.ToString() + spr + obx20.ToString() + spr;
            //valorOBX = valorOBX + obx21.ToString() + spr + obx22.ToString() + spr + obx23.ToString() + spr + obx24.ToString() + spr + obx25.ToString() + spr + obx26.ToString() + spr + obx27.ToString() + spr + obx28.ToString() + spr + obx29.ToString() + spr + obx30.ToString();
            return valorOBX;
        }
    }
    /// <summary>
    /// Clase para generarl el segmento NTE de HL7
    /// </summary>
    [DataContract]
    public class NTEClass
    {
        #region RegionDelLocalesNTE
        private string nte1 = "NTE";
        private string nte2 = string.Empty;
        private string nte3 = string.Empty;
        private string nte4 = string.Empty;
        private string nte5 = string.Empty;


        #endregion

        #region RegionDefNTE
        public string NTE1
        {
            get { return nte1; }
            set { nte1 = value; }
        }
        public string NTE2
        {
            get { return nte2; }
            set { nte2 = value; }
        }
        public string NTE3
        {
            get { return nte3; }
            set { nte3 = value; }
        }
        public string NTE4
        {
            get { return nte4; }
            set { nte4 = value; }
        }
        public string NTE5
        {
            get { return nte5; }
            set { nte5 = value; }
        }

        #endregion

        public NTEClass()
        { }

        public NTEClass(string[] segNTE)
        {
            Utilidades utls = new Utilidades();
            nte1 = segNTE[0];
            nte2 = segNTE[1];
            nte3 = segNTE[2];
            nte4 = segNTE[3];
            nte5 = segNTE[4];
        }
        public string retornoNTE()
        {
            string spr = "|";
            string valorNTE = nte1.ToString() + spr + nte2.ToString() + spr + nte3.ToString() + spr + nte4.ToString() + spr + nte5.ToString();
            return valorNTE;
        }
    }
    /// <summary>
    /// Clase para generar el segmento de MSA para respuestas de ACK de HL7
    /// </summary>
    [DataContract]
    public class MSAClass
    {
        #region RegionDefLocalPV1
        private string msa1 = "MSA";
        private string msa2 = string.Empty;
        private string msa3 = string.Empty;
        private string msa4 = string.Empty;
        //private string msa5 = string.Empty;
        //private string msa6 = string.Empty;
        //private string msa7 = string.Empty;
        //private string msa8 = string.Empty;
        //private string msa9 = string.Empty;
        //private string msa10 = string.Empty;

        #endregion
        #region RegionDefMSA
        public string MSA1
        {
            get { return msa1; }
            set { msa1 = value; }
        }
        public string MSA2
        {
            get { return msa2; }
            set { msa2 = value; }
        }
        public string MSA3
        {
            get { return msa3; }
            set { msa3 = value; }
        }
        public string MSA4
        {
            get { return msa4; }
            set { msa4 = value; }
        }
        //public string MSA5
        //{
        //    get { return msa5; }
        //    set { msa5 = value; }
        //}
        //public string MSA6
        //{
        //    get { return msa6; }
        //    set { msa6 = value; }
        //}
        //public string MSA7
        //{
        //    get { return msa7; }
        //    set { msa7 = value; }
        //}
        //public string MSA8
        //{
        //    get { return msa8; }
        //    set { msa8 = value; }
        //}
        //public string MSA9
        //{
        //    get { return msa9; }
        //    set { msa9 = value; }
        //}
        //public string MSA10
        //{
        //    get { return msa10; }
        //    set { msa10 = value; }
        //}

        #endregion

        public MSAClass()
        {
            msa1 = "MSA";
            msa2 = "";
            msa3 = "";
            msa4 = "";
        }

        public MSAClass(string[] segMSA)
        {
            Utilidades utls = new Utilidades();
            msa1 = segMSA[0];
            msa2 = segMSA[1];
            msa3 = segMSA[2];
            msa4 = segMSA[3];
            //msa5 = segMSA[4];
            //msa6 = segMSA[5];
            //msa7 = segMSA[6];
            //msa8 = segMSA[7];
            //msa9 = segMSA[8];
            //msa10 = segMSA[9];


        }

        public string retornoMSA()
        {
            string spr = "|";
            string valorMSA = msa1.ToString() + spr + msa2.ToString() + spr + msa3.ToString() + spr + msa4.ToString();
            return valorMSA;
        }
    }

    /// <summary>
    /// Clase para realizar el cargue de los resultados de los examenes
    /// </summary>
    [DataContract]
    public class resultadoExamen
    {
        resultadoExamen()
        {

        }
        public resultadoExamen(string mensajeHL7)
        {
            string[] segmentoMSH = new string[19];
            MSHClass msh = new MSHClass(segmentoMSH);
        }
    }

}

