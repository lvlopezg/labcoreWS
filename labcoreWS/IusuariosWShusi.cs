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
    public interface IusuariosWShusi
    {
        [OperationContract]
        string usrWindows(string usuario);

        [OperationContract]
        string usrSahi(string usuario);

        [OperationContract]
        string idUsuaXcodUsua(Int32 Idusuario);

    }
}
