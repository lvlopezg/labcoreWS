﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace labcoreWS.WSAldea {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://habitatclient.service.soapws.core.axesnet.com/", ConfigurationName="WSAldea.ISmsSendSoap")]
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'ISmsSendSoap'
    public interface ISmsSendSoap {
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'ISmsSendSoap'
        
        [System.ServiceModel.OperationContractAttribute(Action="http://habitatclient.service.soapws.core.axesnet.com/ISmsSendSoap/smsSendSoapRequ" +
            "est", ReplyAction="http://habitatclient.service.soapws.core.axesnet.com/ISmsSendSoap/smsSendSoapResp" +
            "onse")]
        [System.ServiceModel.DataContractFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'ISmsSendSoap.smsSendSoap(string, string, long, string, string, string)'
        string smsSendSoap(string username, string password, long country, string mobile, string message, string @operator);
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'ISmsSendSoap.smsSendSoap(string, string, long, string, string, string)'
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'ISmsSendSoapChannel'
    public interface ISmsSendSoapChannel : labcoreWS.WSAldea.ISmsSendSoap, System.ServiceModel.IClientChannel {
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'ISmsSendSoapChannel'
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'SmsSendSoapClient'
    public partial class SmsSendSoapClient : System.ServiceModel.ClientBase<labcoreWS.WSAldea.ISmsSendSoap>, labcoreWS.WSAldea.ISmsSendSoap {
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'SmsSendSoapClient'
        
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'SmsSendSoapClient.SmsSendSoapClient()'
        public SmsSendSoapClient() {
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'SmsSendSoapClient.SmsSendSoapClient()'
        }
        
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'SmsSendSoapClient.SmsSendSoapClient(string)'
        public SmsSendSoapClient(string endpointConfigurationName) : 
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'SmsSendSoapClient.SmsSendSoapClient(string)'
                base(endpointConfigurationName) {
        }
        
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'SmsSendSoapClient.SmsSendSoapClient(string, string)'
        public SmsSendSoapClient(string endpointConfigurationName, string remoteAddress) : 
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'SmsSendSoapClient.SmsSendSoapClient(string, string)'
                base(endpointConfigurationName, remoteAddress) {
        }
        
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'SmsSendSoapClient.SmsSendSoapClient(string, EndpointAddress)'
        public SmsSendSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'SmsSendSoapClient.SmsSendSoapClient(string, EndpointAddress)'
                base(endpointConfigurationName, remoteAddress) {
        }
        
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'SmsSendSoapClient.SmsSendSoapClient(Binding, EndpointAddress)'
        public SmsSendSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'SmsSendSoapClient.SmsSendSoapClient(Binding, EndpointAddress)'
                base(binding, remoteAddress) {
        }
        
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'SmsSendSoapClient.smsSendSoap(string, string, long, string, string, string)'
        public string smsSendSoap(string username, string password, long country, string mobile, string message, string @operator) {
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'SmsSendSoapClient.smsSendSoap(string, string, long, string, string, string)'
            return base.Channel.smsSendSoap(username, password, country, mobile, message, @operator);
        }
    }
}
