﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Il codice è stato generato da uno strumento.
//     Versione runtime:2.0.50727.8762
//
//     Le modifiche apportate a questo file possono provocare un comportamento non corretto e andranno perse se
//     il codice viene rigenerato.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.org/XMLSchema")]
[System.Xml.Serialization.XmlRootAttribute("Root", Namespace="http://www.example.org/XMLSchema", IsNullable=false)]
public partial class RootType {
    
    private ConnectionType connectionField;
    
    private DirectionType directionField;
    
    /// <remarks/>
    public ConnectionType Connection {
        get {
            return this.connectionField;
        }
        set {
            this.connectionField = value;
        }
    }
    
    /// <remarks/>
    public DirectionType Direction {
        get {
            return this.directionField;
        }
        set {
            this.directionField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.org/XMLSchema")]
public partial class ConnectionType {
    
    private bool statusField;
    
    private string ipField;
    
    /// <remarks/>
    public bool status {
        get {
            return this.statusField;
        }
        set {
            this.statusField = value;
        }
    }
    
    /// <remarks/>
    public string ip {
        get {
            return this.ipField;
        }
        set {
            this.ipField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.example.org/XMLSchema")]
public partial class DirectionType {
    
    private DirectionTypeDirectionAttr directionAttrField;
    
    private bool directionAttrFieldSpecified;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Qualified)]
    public DirectionTypeDirectionAttr DirectionAttr {
        get {
            return this.directionAttrField;
        }
        set {
            this.directionAttrField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool DirectionAttrSpecified {
        get {
            return this.directionAttrFieldSpecified;
        }
        set {
            this.directionAttrFieldSpecified = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.example.org/XMLSchema")]
public enum DirectionTypeDirectionAttr {
    
    /// <remarks/>
    FORWARD,
    
    /// <remarks/>
    RIGHT,
    
    /// <remarks/>
    LEFT,
}
