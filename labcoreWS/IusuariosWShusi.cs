using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace labcoreWS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IusuariosWShusi" in both code and config file together.
    [ServiceContract]
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'IusuariosWShusi'
    public interface IusuariosWShusi
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'IusuariosWShusi'
    {
        [OperationContract]
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'IusuariosWShusi.usrWindows(string)'
        string usrWindows(string usuario);
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'IusuariosWShusi.usrWindows(string)'

        [OperationContract]
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'IusuariosWShusi.usrSahi(string)'
        string usrSahi(string usuario);
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'IusuariosWShusi.usrSahi(string)'

        [OperationContract]
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'IusuariosWShusi.idUsuaXcodUsua(int)'
        string idUsuaXcodUsua(Int32 Idusuario);
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'IusuariosWShusi.idUsuaXcodUsua(int)'
        [OperationContract]
        string usuarioXidPersonal(string idPersonal);

    }
}
