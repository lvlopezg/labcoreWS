﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace labcoreWS.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.4.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=wintvtibd01\\hsi_tst;Initial Catalog=HSI_TST;User ID=husi_usr;Password" +
            "=pwdHUSI;MultipleActiveResultSets=True")]
        public string DBConexion {
            get {
                return ((string)(this["DBConexion"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=wintvtibd01\\hsi_tst;Initial Catalog=HSI_TST;User ID=husi_usr;Password" +
            "=pwdHUSI;MultipleActiveResultSets=True")]
        public string LabcoreDBConXX {
            get {
                return ((string)(this["LabcoreDBConXX"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=wintvtibd01\\hsi_tst;Initial Catalog=HSI_TST;User ID=husi_usr;Password" +
            "=pwdHUSI;MultipleActiveResultSets=True")]
        public string LabcoreDBCon {
            get {
                return ((string)(this["LabcoreDBCon"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=wintvtibd01\\hsi_tst;Initial Catalog=HSI_TST;User ID=husi_usr;Password" +
            "=pwdHUSI;MultipleActiveResultSets=True")]
        public string dbProduccion {
            get {
                return ((string)(this["dbProduccion"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=wintvtibd01\\hsi_tst;Initial Catalog=HSI_TST;User ID=husi_usr;Password" +
            "=pwdHUSI;MultipleActiveResultSets=True")]
        public string alterno {
            get {
                return ((string)(this["alterno"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("notificacionessahi@husi.org.co")]
        public string origenCritico {
            get {
                return ((string)(this["origenCritico"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("cgrojas@husi.org.co")]
        public string destinoCritico {
            get {
                return ((string)(this["destinoCritico"]));
            }
        }
    }
}
