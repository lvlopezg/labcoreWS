namespace labcoreWS
{
  using System.Xml.Serialization;

  /// <comentarios/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
  [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
  public partial class solicitud
  {
    private solicitudProducto[ ] productoField;
    private string idAtencionField;
    private string nroSolicitudField;
    private string fechaSolicitudField;
    private string prioridadField;
    private string nroOrdenField;
    private string idUsuarioField;
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute("producto", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public solicitudProducto[ ] producto
    {
      get
      {
        return this.productoField;
      }
      set
      {
        this.productoField = value;
      }
    }
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string idAtencion
    {
      get
      {
        return this.idAtencionField;
      }
      set
      {
        this.idAtencionField = value;
      }
    }
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string nroSolicitud
    {
      get
      {
        return this.nroSolicitudField;
      }
      set
      {
        this.nroSolicitudField = value;
      }
    }
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string fechaSolicitud
    {
      get
      {
        return this.fechaSolicitudField;
      }
      set
      {
        this.fechaSolicitudField = value;
      }
    }
    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string prioridad
    {
      get
      {
        return this.prioridadField;
      }
      set
      {
        this.prioridadField = value;
      }
    }

    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string nroOrden
    {
      get
      {
        return this.nroOrdenField;
      }
      set
      {
        this.nroOrdenField = value;
      }
    }

    /// <comentarios/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string idUsuario
    {
      get
      {
        return this.idUsuarioField;
      }
      set
      {
        this.idUsuarioField = value;
      }
    }
  }

  /// <comentarios/>
  [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
  [System.SerializableAttribute()]
  [System.Diagnostics.DebuggerStepThroughAttribute()]
  [System.ComponentModel.DesignerCategoryAttribute("code")]
  [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
  public partial class solicitudProducto
  {
    private string idField;
    private string cupsField;
    private string cantField;
    private string obsField;
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string id
    {
      get
      {
        return this.idField;
      }
      set
      {
        this.idField = value;
      }
    }
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string cups
    {
      get
      {
        return this.cupsField;
      }
      set
      {
        this.cupsField = value;
      }
    }

    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string cant
    {
      get
      {
        return this.cantField;
      }
      set
      {
        this.cantField = value;
      }
    }
    /// <comentarios/>
    [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string obs
    {
      get
      {
        return this.obsField;
      }
      set
      {
        this.obsField = value;
      }
    }
  }

}
#pragma warning restore CS1587 // El comentario XML no está situado en un elemento válido del idioma
