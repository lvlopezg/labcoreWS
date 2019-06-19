
namespace labcoreWS.clienteDatosPcte
{
    using System.Runtime.Serialization;
    using System;

    /// <summary>
    /// /Clase que Representa Informacion basica del Paciente.
    /// </summary>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "pacienteS1", Namespace = "http://schemas.datacontract.org/2004/07/husiWSGeneral.Modelos")]
    [System.SerializableAttribute()]

    public partial class pacienteS1 : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
    {

        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ApellidosField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NombreField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string contratoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string edadField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime fechaEgresoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime fechaIngresoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int idAtencionField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int idPacienteField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string lugarIngresoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string numDocumentoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string razonSocialEpsField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string tipoDocumentoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string tipoatencionField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ubicacionActualField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ubicacionEgresoField;

        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ubicacionIngresoField;

        [global::System.ComponentModel.BrowsableAttribute(false)]
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'pacienteS1.ExtensionData'
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'pacienteS1.ExtensionData'
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        /// <summary>
        /// Apellidos del Paciente
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Apellidos
        {
            get
            {
                return this.ApellidosField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ApellidosField, value) != true))
                {
                    this.ApellidosField = value;
                    this.RaisePropertyChanged("Apellidos");
                }
            }
        }
        /// <summary>
        /// Nombres del paciente
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nombre
        {
            get
            {
                return this.NombreField;
            }
            set
            {
                if ((object.ReferenceEquals(this.NombreField, value) != true))
                {
                    this.NombreField = value;
                    this.RaisePropertyChanged("Nombre");
                }
            }
        }
        /// <summary>
        /// Contrato que se encuentra relacionado con el numero de la atencion
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string contrato
        {
            get
            {
                return this.contratoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.contratoField, value) != true))
                {
                    this.contratoField = value;
                    this.RaisePropertyChanged("contrato");
                }
            }
        }
        /// <summary>
        /// Edad del Paciente
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string edad
        {
            get
            {
                return this.edadField;
            }
            set
            {
                if ((object.ReferenceEquals(this.edadField, value) != true))
                {
                    this.edadField = value;
                    this.RaisePropertyChanged("edad");
                }
            }
        }
        /// <summary>
        /// Fecha de Egreso del Paciente
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime fechaEgreso
        {
            get
            {
                return this.fechaEgresoField;
            }
            set
            {
                if ((this.fechaEgresoField.Equals(value) != true))
                {
                    this.fechaEgresoField = value;
                    this.RaisePropertyChanged("fechaEgreso");
                }
            }
        }
        /// <summary>
        /// FEcha del Ingreso del paciente al Hospital
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime fechaIngreso
        {
            get
            {
                return this.fechaIngresoField;
            }
            set
            {
                if ((this.fechaIngresoField.Equals(value) != true))
                {
                    this.fechaIngresoField = value;
                    this.RaisePropertyChanged("fechaIngreso");
                }
            }
        }
        /// <summary>
        /// Numero de Atencion Asignado al Paciente
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int idAtencion
        {
            get
            {
                return this.idAtencionField;
            }
            set
            {
                if ((this.idAtencionField.Equals(value) != true))
                {
                    this.idAtencionField = value;
                    this.RaisePropertyChanged("idAtencion");
                }
            }
        }
        /// <summary>
        /// ID o identificador unico del paciente en el Hospital
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int idPaciente
        {
            get
            {
                return this.idPacienteField;
            }
            set
            {
                if ((this.idPacienteField.Equals(value) != true))
                {
                    this.idPacienteField = value;
                    this.RaisePropertyChanged("idPaciente");
                }
            }
        }
        /// <summary>
        /// Lugar de Ingreso(Servicio) del paciente al Hospital
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string lugarIngreso
        {
            get
            {
                return this.lugarIngresoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.lugarIngresoField, value) != true))
                {
                    this.lugarIngresoField = value;
                    this.RaisePropertyChanged("lugarIngreso");
                }
            }
        }
        /// <summary>
        /// Numero de Documento de Identificacion del Paciente
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string numDocumento
        {
            get
            {
                return this.numDocumentoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.numDocumentoField, value) != true))
                {
                    this.numDocumentoField = value;
                    this.RaisePropertyChanged("numDocumento");
                }
            }
        }
        /// <summary>
        /// Razon Social-Nombre de la EPS, a la cual se encuentra afiliado el Paciente.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string razonSocialEps
        {
            get
            {
                return this.razonSocialEpsField;
            }
            set
            {
                if ((object.ReferenceEquals(this.razonSocialEpsField, value) != true))
                {
                    this.razonSocialEpsField = value;
                    this.RaisePropertyChanged("razonSocialEps");
                }
            }
        }
        /// <summary>
        /// Tipo de Documento de Identificacion del Paciente
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string tipoDocumento
        {
            get
            {
                return this.tipoDocumentoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.tipoDocumentoField, value) != true))
                {
                    this.tipoDocumentoField = value;
                    this.RaisePropertyChanged("tipoDocumento");
                }
            }
        }
        /// <summary>
        /// Tipo de Atencion asignada al paciente.
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string tipoatencion
        {
            get
            {
                return this.tipoatencionField;
            }
            set
            {
                if ((object.ReferenceEquals(this.tipoatencionField, value) != true))
                {
                    this.tipoatencionField = value;
                    this.RaisePropertyChanged("tipoatencion");
                }
            }
        }
        /// <summary>
        /// Ubicacion-Servicio en la cual se encuentra paciente en el momento actual
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ubicacionActual
        {
            get
            {
                return this.ubicacionActualField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ubicacionActualField, value) != true))
                {
                    this.ubicacionActualField = value;
                    this.RaisePropertyChanged("ubicacionActual");
                }
            }
        }
        /// <summary>
        /// Ubicacion de Egreso o servicio desde el cual se determina la salida del paciente
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ubicacionEgreso
        {
            get
            {
                return this.ubicacionEgresoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ubicacionEgresoField, value) != true))
                {
                    this.ubicacionEgresoField = value;
                    this.RaisePropertyChanged("ubicacionEgreso");
                }
            }
        }
        /// <summary>
        /// Ubicacion-Servicio de Ingreso del Paciente
        /// </summary>
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ubicacionIngreso
        {
            get
            {
                return this.ubicacionIngresoField;
            }
            set
            {
                if ((object.ReferenceEquals(this.ubicacionIngresoField, value) != true))
                {
                    this.ubicacionIngresoField = value;
                    this.RaisePropertyChanged("ubicacionIngreso");
                }
            }
        }

#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'pacienteS1.PropertyChanged'
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible de forma pública 'pacienteS1.PropertyChanged'
        /// <summary>
        /// Evento que determina que una propiedad ha cambiado
        /// </summary>
        /// <param name="propertyName"></param>
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    /// <summary>
    /// Interface Publica de DAtos, para la clase Paciente
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "clienteDatosPcte.IdatosPaciente")]
    public interface IdatosPaciente
    {
        /// <summary>
        /// Operacion para obtener la Informacion del Paciente
        /// </summary>
        /// <param name="nroAtencion">Numero de Atencion</param>

        /// <returns>Objeto de la Clase PacienteS1, con la informacion del paciente</returns>
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IdatosPaciente/datosXatencion", ReplyAction = "http://tempuri.org/IdatosPaciente/datosXatencionResponse")]
        labcoreWS.clienteDatosPcte.pacienteS1 datosXatencion(int nroAtencion);
        /// <summary>
        /// Operacion Asincrona para obtener la informacion del Paciente
        /// </summary>
        /// <param name="nroAtencion">Numero de la Atencion del Paciente</param>
        /// <returns>Objeto de la Clase pacienteS1, con la informacion del Paciente</returns>
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IdatosPaciente/datosXatencion", ReplyAction = "http://tempuri.org/IdatosPaciente/datosXatencionResponse")]
        System.Threading.Tasks.Task<labcoreWS.clienteDatosPcte.pacienteS1> datosXatencionAsync(int nroAtencion);
    }
    /// <summary>
    /// 
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IdatosPacienteChannel : labcoreWS.clienteDatosPcte.IdatosPaciente, System.ServiceModel.IClientChannel
    {
    }
    /// <summary>
    /// 
    /// </summary>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class IdatosPacienteClient : System.ServiceModel.ClientBase<labcoreWS.clienteDatosPcte.IdatosPaciente>, labcoreWS.clienteDatosPcte.IdatosPaciente
    {
        /// <summary>
        /// 
        /// </summary>
        public IdatosPacienteClient()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointConfigurationName"></param>
        public IdatosPacienteClient(string endpointConfigurationName) :
                base(endpointConfigurationName)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointConfigurationName"></param>
        /// <param name="remoteAddress"></param>
        public IdatosPacienteClient(string endpointConfigurationName, string remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpointConfigurationName"></param>
        /// <param name="remoteAddress"></param>
        public IdatosPacienteClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
                base(endpointConfigurationName, remoteAddress)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="remoteAddress"></param>
        public IdatosPacienteClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
                base(binding, remoteAddress)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nroAtencion"></param>
        /// <returns></returns>
        public labcoreWS.clienteDatosPcte.pacienteS1 datosXatencion(int nroAtencion)
        {
            return base.Channel.datosXatencion(nroAtencion);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nroAtencion"></param>
        /// <returns></returns>
        public System.Threading.Tasks.Task<labcoreWS.clienteDatosPcte.pacienteS1> datosXatencionAsync(int nroAtencion)
        {
            return base.Channel.datosXatencionAsync(nroAtencion);
        }
    }
}
