﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace labcoreWS.srLabcoreTAT {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="srLabcoreTAT.IWSSolicitudes")]
    public interface IWSSolicitudes {
        
        // CODEGEN: Generating message contract since the wrapper namespace (urn:WSSolicitudesIntf-IWSSolicitudes) of message RecResultsRequest does not match the default value (http://tempuri.org/)
        [System.ServiceModel.OperationContractAttribute(Action="urn:WSSolicitudesIntf-IWSSolicitudes#RecResults", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        labcoreWS.srLabcoreTAT.RecResultsResponse RecResults(labcoreWS.srLabcoreTAT.RecResultsRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:WSSolicitudesIntf-IWSSolicitudes#RecResults", ReplyAction="*")]
        System.Threading.Tasks.Task<labcoreWS.srLabcoreTAT.RecResultsResponse> RecResultsAsync(labcoreWS.srLabcoreTAT.RecResultsRequest request);
        
        // CODEGEN: Generating message contract since the wrapper namespace (urn:WSSolicitudesIntf-IWSSolicitudes) of message RecSolicitudesRequest does not match the default value (http://tempuri.org/)
        [System.ServiceModel.OperationContractAttribute(Action="urn:WSSolicitudesIntf-IWSSolicitudes#RecSolicitudes", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        labcoreWS.srLabcoreTAT.RecSolicitudesResponse RecSolicitudes(labcoreWS.srLabcoreTAT.RecSolicitudesRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:WSSolicitudesIntf-IWSSolicitudes#RecSolicitudes", ReplyAction="*")]
        System.Threading.Tasks.Task<labcoreWS.srLabcoreTAT.RecSolicitudesResponse> RecSolicitudesAsync(labcoreWS.srLabcoreTAT.RecSolicitudesRequest request);
        
        // CODEGEN: Generating message contract since the wrapper namespace (urn:WSSolicitudesIntf-IWSSolicitudes) of message GetResultPdfRequest does not match the default value (http://tempuri.org/)
        [System.ServiceModel.OperationContractAttribute(Action="urn:WSSolicitudesIntf-IWSSolicitudes#GetResultPdf", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        labcoreWS.srLabcoreTAT.GetResultPdfResponse GetResultPdf(labcoreWS.srLabcoreTAT.GetResultPdfRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:WSSolicitudesIntf-IWSSolicitudes#GetResultPdf", ReplyAction="*")]
        System.Threading.Tasks.Task<labcoreWS.srLabcoreTAT.GetResultPdfResponse> GetResultPdfAsync(labcoreWS.srLabcoreTAT.GetResultPdfRequest request);
        
        // CODEGEN: Generating message contract since the wrapper namespace (urn:WSSolicitudesIntf-IWSSolicitudes) of message GetHL7MsgRequest does not match the default value (http://tempuri.org/)
        [System.ServiceModel.OperationContractAttribute(Action="urn:WSSolicitudesIntf-IWSSolicitudes#GetHL7Msg", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        labcoreWS.srLabcoreTAT.GetHL7MsgResponse GetHL7Msg(labcoreWS.srLabcoreTAT.GetHL7MsgRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:WSSolicitudesIntf-IWSSolicitudes#GetHL7Msg", ReplyAction="*")]
        System.Threading.Tasks.Task<labcoreWS.srLabcoreTAT.GetHL7MsgResponse> GetHL7MsgAsync(labcoreWS.srLabcoreTAT.GetHL7MsgRequest request);
        
        // CODEGEN: Generating message contract since the wrapper namespace (urn:WSSolicitudesIntf-IWSSolicitudes) of message CambioEstadoRequest does not match the default value (http://tempuri.org/)
        [System.ServiceModel.OperationContractAttribute(Action="urn:WSSolicitudesIntf-IWSSolicitudes#CambioEstado", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        labcoreWS.srLabcoreTAT.CambioEstadoResponse CambioEstado(labcoreWS.srLabcoreTAT.CambioEstadoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="urn:WSSolicitudesIntf-IWSSolicitudes#CambioEstado", ReplyAction="*")]
        System.Threading.Tasks.Task<labcoreWS.srLabcoreTAT.CambioEstadoResponse> CambioEstadoAsync(labcoreWS.srLabcoreTAT.CambioEstadoRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="RecResults", WrapperNamespace="urn:WSSolicitudesIntf-IWSSolicitudes", IsWrapped=true)]
    public partial class RecResultsRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public string xmlRes;
        
        public RecResultsRequest() {
        }
        
        public RecResultsRequest(string xmlRes) {
            this.xmlRes = xmlRes;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="RecResultsResponse", WrapperNamespace="urn:WSSolicitudesIntf-IWSSolicitudes", IsWrapped=true)]
    public partial class RecResultsResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public string @return;
        
        public RecResultsResponse() {
        }
        
        public RecResultsResponse(string @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="RecSolicitudes", WrapperNamespace="urn:WSSolicitudesIntf-IWSSolicitudes", IsWrapped=true)]
    public partial class RecSolicitudesRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public string xmlSol;
        
        public RecSolicitudesRequest() {
        }
        
        public RecSolicitudesRequest(string xmlSol) {
            this.xmlSol = xmlSol;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="RecSolicitudesResponse", WrapperNamespace="urn:WSSolicitudesIntf-IWSSolicitudes", IsWrapped=true)]
    public partial class RecSolicitudesResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public string @return;
        
        public RecSolicitudesResponse() {
        }
        
        public RecSolicitudesResponse(string @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetResultPdf", WrapperNamespace="urn:WSSolicitudesIntf-IWSSolicitudes", IsWrapped=true)]
    public partial class GetResultPdfRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public string numorden;
        
        public GetResultPdfRequest() {
        }
        
        public GetResultPdfRequest(string numorden) {
            this.numorden = numorden;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetResultPdfResponse", WrapperNamespace="urn:WSSolicitudesIntf-IWSSolicitudes", IsWrapped=true)]
    public partial class GetResultPdfResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public string @return;
        
        public GetResultPdfResponse() {
        }
        
        public GetResultPdfResponse(string @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetHL7Msg", WrapperNamespace="urn:WSSolicitudesIntf-IWSSolicitudes", IsWrapped=true)]
    public partial class GetHL7MsgRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public string msg;
        
        public GetHL7MsgRequest() {
        }
        
        public GetHL7MsgRequest(string msg) {
            this.msg = msg;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetHL7MsgResponse", WrapperNamespace="urn:WSSolicitudesIntf-IWSSolicitudes", IsWrapped=true)]
    public partial class GetHL7MsgResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public string @return;
        
        public GetHL7MsgResponse() {
        }
        
        public GetHL7MsgResponse(string @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="CambioEstado", WrapperNamespace="urn:WSSolicitudesIntf-IWSSolicitudes", IsWrapped=true)]
    public partial class CambioEstadoRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public string msg;
        
        public CambioEstadoRequest() {
        }
        
        public CambioEstadoRequest(string msg) {
            this.msg = msg;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="CambioEstadoResponse", WrapperNamespace="urn:WSSolicitudesIntf-IWSSolicitudes", IsWrapped=true)]
    public partial class CambioEstadoResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public string @return;
        
        public CambioEstadoResponse() {
        }
        
        public CambioEstadoResponse(string @return) {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IWSSolicitudesChannel : labcoreWS.srLabcoreTAT.IWSSolicitudes, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WSSolicitudesClient : System.ServiceModel.ClientBase<labcoreWS.srLabcoreTAT.IWSSolicitudes>, labcoreWS.srLabcoreTAT.IWSSolicitudes {
        
        public WSSolicitudesClient() {
        }
        
        public WSSolicitudesClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WSSolicitudesClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WSSolicitudesClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WSSolicitudesClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        labcoreWS.srLabcoreTAT.RecResultsResponse labcoreWS.srLabcoreTAT.IWSSolicitudes.RecResults(labcoreWS.srLabcoreTAT.RecResultsRequest request) {
            return base.Channel.RecResults(request);
        }
        
        public string RecResults(string xmlRes) {
            labcoreWS.srLabcoreTAT.RecResultsRequest inValue = new labcoreWS.srLabcoreTAT.RecResultsRequest();
            inValue.xmlRes = xmlRes;
            labcoreWS.srLabcoreTAT.RecResultsResponse retVal = ((labcoreWS.srLabcoreTAT.IWSSolicitudes)(this)).RecResults(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<labcoreWS.srLabcoreTAT.RecResultsResponse> labcoreWS.srLabcoreTAT.IWSSolicitudes.RecResultsAsync(labcoreWS.srLabcoreTAT.RecResultsRequest request) {
            return base.Channel.RecResultsAsync(request);
        }
        
        public System.Threading.Tasks.Task<labcoreWS.srLabcoreTAT.RecResultsResponse> RecResultsAsync(string xmlRes) {
            labcoreWS.srLabcoreTAT.RecResultsRequest inValue = new labcoreWS.srLabcoreTAT.RecResultsRequest();
            inValue.xmlRes = xmlRes;
            return ((labcoreWS.srLabcoreTAT.IWSSolicitudes)(this)).RecResultsAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        labcoreWS.srLabcoreTAT.RecSolicitudesResponse labcoreWS.srLabcoreTAT.IWSSolicitudes.RecSolicitudes(labcoreWS.srLabcoreTAT.RecSolicitudesRequest request) {
            return base.Channel.RecSolicitudes(request);
        }
        
        public string RecSolicitudes(string xmlSol) {
            labcoreWS.srLabcoreTAT.RecSolicitudesRequest inValue = new labcoreWS.srLabcoreTAT.RecSolicitudesRequest();
            inValue.xmlSol = xmlSol;
            labcoreWS.srLabcoreTAT.RecSolicitudesResponse retVal = ((labcoreWS.srLabcoreTAT.IWSSolicitudes)(this)).RecSolicitudes(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<labcoreWS.srLabcoreTAT.RecSolicitudesResponse> labcoreWS.srLabcoreTAT.IWSSolicitudes.RecSolicitudesAsync(labcoreWS.srLabcoreTAT.RecSolicitudesRequest request) {
            return base.Channel.RecSolicitudesAsync(request);
        }
        
        public System.Threading.Tasks.Task<labcoreWS.srLabcoreTAT.RecSolicitudesResponse> RecSolicitudesAsync(string xmlSol) {
            labcoreWS.srLabcoreTAT.RecSolicitudesRequest inValue = new labcoreWS.srLabcoreTAT.RecSolicitudesRequest();
            inValue.xmlSol = xmlSol;
            return ((labcoreWS.srLabcoreTAT.IWSSolicitudes)(this)).RecSolicitudesAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        labcoreWS.srLabcoreTAT.GetResultPdfResponse labcoreWS.srLabcoreTAT.IWSSolicitudes.GetResultPdf(labcoreWS.srLabcoreTAT.GetResultPdfRequest request) {
            return base.Channel.GetResultPdf(request);
        }
        
        public string GetResultPdf(string numorden) {
            labcoreWS.srLabcoreTAT.GetResultPdfRequest inValue = new labcoreWS.srLabcoreTAT.GetResultPdfRequest();
            inValue.numorden = numorden;
            labcoreWS.srLabcoreTAT.GetResultPdfResponse retVal = ((labcoreWS.srLabcoreTAT.IWSSolicitudes)(this)).GetResultPdf(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<labcoreWS.srLabcoreTAT.GetResultPdfResponse> labcoreWS.srLabcoreTAT.IWSSolicitudes.GetResultPdfAsync(labcoreWS.srLabcoreTAT.GetResultPdfRequest request) {
            return base.Channel.GetResultPdfAsync(request);
        }
        
        public System.Threading.Tasks.Task<labcoreWS.srLabcoreTAT.GetResultPdfResponse> GetResultPdfAsync(string numorden) {
            labcoreWS.srLabcoreTAT.GetResultPdfRequest inValue = new labcoreWS.srLabcoreTAT.GetResultPdfRequest();
            inValue.numorden = numorden;
            return ((labcoreWS.srLabcoreTAT.IWSSolicitudes)(this)).GetResultPdfAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        labcoreWS.srLabcoreTAT.GetHL7MsgResponse labcoreWS.srLabcoreTAT.IWSSolicitudes.GetHL7Msg(labcoreWS.srLabcoreTAT.GetHL7MsgRequest request) {
            return base.Channel.GetHL7Msg(request);
        }
        
        public string GetHL7Msg(string msg) {
            labcoreWS.srLabcoreTAT.GetHL7MsgRequest inValue = new labcoreWS.srLabcoreTAT.GetHL7MsgRequest();
            inValue.msg = msg;
            labcoreWS.srLabcoreTAT.GetHL7MsgResponse retVal = ((labcoreWS.srLabcoreTAT.IWSSolicitudes)(this)).GetHL7Msg(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<labcoreWS.srLabcoreTAT.GetHL7MsgResponse> labcoreWS.srLabcoreTAT.IWSSolicitudes.GetHL7MsgAsync(labcoreWS.srLabcoreTAT.GetHL7MsgRequest request) {
            return base.Channel.GetHL7MsgAsync(request);
        }
        
        public System.Threading.Tasks.Task<labcoreWS.srLabcoreTAT.GetHL7MsgResponse> GetHL7MsgAsync(string msg) {
            labcoreWS.srLabcoreTAT.GetHL7MsgRequest inValue = new labcoreWS.srLabcoreTAT.GetHL7MsgRequest();
            inValue.msg = msg;
            return ((labcoreWS.srLabcoreTAT.IWSSolicitudes)(this)).GetHL7MsgAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        labcoreWS.srLabcoreTAT.CambioEstadoResponse labcoreWS.srLabcoreTAT.IWSSolicitudes.CambioEstado(labcoreWS.srLabcoreTAT.CambioEstadoRequest request) {
            return base.Channel.CambioEstado(request);
        }
        
        public string CambioEstado(string msg) {
            labcoreWS.srLabcoreTAT.CambioEstadoRequest inValue = new labcoreWS.srLabcoreTAT.CambioEstadoRequest();
            inValue.msg = msg;
            labcoreWS.srLabcoreTAT.CambioEstadoResponse retVal = ((labcoreWS.srLabcoreTAT.IWSSolicitudes)(this)).CambioEstado(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<labcoreWS.srLabcoreTAT.CambioEstadoResponse> labcoreWS.srLabcoreTAT.IWSSolicitudes.CambioEstadoAsync(labcoreWS.srLabcoreTAT.CambioEstadoRequest request) {
            return base.Channel.CambioEstadoAsync(request);
        }
        
        public System.Threading.Tasks.Task<labcoreWS.srLabcoreTAT.CambioEstadoResponse> CambioEstadoAsync(string msg) {
            labcoreWS.srLabcoreTAT.CambioEstadoRequest inValue = new labcoreWS.srLabcoreTAT.CambioEstadoRequest();
            inValue.msg = msg;
            return ((labcoreWS.srLabcoreTAT.IWSSolicitudes)(this)).CambioEstadoAsync(inValue);
        }
    }
}
