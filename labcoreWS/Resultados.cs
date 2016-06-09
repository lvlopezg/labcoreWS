    using System.Xml.Serialization;
    using System.Collections.Generic;


    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Resultados
    {

        private string nombreField;

        private string apellidoField;

        private string statusField;

        //private ResultadosResultado[] resultadoField;

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute( Order=0, Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Nombre
        {
            get
            {
                return this.nombreField;
            }
            set
            {
                this.nombreField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute( Order=1, Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Apellido
        {
            get
            {
                return this.apellidoField;
            }
            set
            {
                this.apellidoField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute( Order=2, Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        
            public string status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("Resultado", Order=3, Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public List<string> Resultado = new List<string>();
        //public ResultadosResultado[] Resultado
        //{
        //    get
        //    {
        //        return this.resultadoField;
        //    }
        //    set
        //    {
        //        this.resultadoField = value;
        //    }
        //}
        
    }



    ///// <comentarios/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    //[System.SerializableAttribute()]
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    //public partial class ResultadosResultado
    //{

    //    private string valueField;

    //    /// <comentarios/>
    //    [System.Xml.Serialization.XmlTextAttribute()]
    //    public string Value
    //    {
    //        get
    //        {
    //            return this.valueField;
    //        }
    //        set
    //        {
    //            this.valueField = value;
    //        }
    //    }
    //}

    ///// <comentarios/>
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    //[System.SerializableAttribute()]
    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    //[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    //public partial class NewDataSet
    //{

    //    private Resultados[] itemsField;

    //    /// <comentarios/>
    //    [System.Xml.Serialization.XmlElementAttribute("Resultados")]
    //    public Resultados[] Items
    //    {
    //        get
    //        {
    //            return this.itemsField;
    //        }
    //        set
    //        {
    //            this.itemsField = value;
    //        }
    //    }
    //}

