using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace labcoreWS
{
    [ServiceContract]
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'IresultadosWS'
    public interface IresultadosWS
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'IresultadosWS'
    {
        [OperationContract]
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'IresultadosWS.getResultados(string)'
        string getResultados(string orden);
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'IresultadosWS.getResultados(string)'

    }


    //[DataContract]

    //public class Resultados
    //{

    //    private string nombreField;

    //    private string apellidoField;

    //    private string statusField;

    //    [DataMember(Order=0)]
    //    public string Nombre
    //    {
    //        get
    //        {
    //            return this.nombreField;
    //        }
    //        set
    //        {
    //            this.nombreField = value;
    //        }
    //    }

    //    [DataMember(Order = 1)]
    //    public string Apellido
    //    {
    //        get
    //        {
    //            return this.apellidoField;
    //        }
    //        set
    //        {
    //            this.apellidoField = value;
    //        }
    //    }

    //    [DataMember(Order = 2)]

    //    public string status
    //    {
    //        get
    //        {
    //            return this.statusField;
    //        }
    //        set
    //        {
    //            this.statusField = value;
    //        }
    //    }

    //    [DataMember( Name="Resultado", Order = 3)]
    //    public List<string> Resultado = new List<string>();

    //}

}

