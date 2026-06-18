using System;
using System.Reflection;

namespace Sirius.Architecture.Utility {

    /// <summary>
    /// This class encapsulates the process of reading version information from an assembly.
    /// </summary>
    public class AssemblyVersionInfo {

        #region Private Fields

        private AssemblyName _name;

        private string _company;
        private string _copyright;
        private string _description;
        private string _fileVersion;
        private string _product;
        private string _title;
        private string _trademark;
        private string _version;

        #endregion

        #region Constructors

        /// <summary>
        /// Get the version information from the process executable in the default application domain.
        /// If called from unmanaged code, fall back to the calling assembly, then this assembly.
        /// </summary>
        /// <returns>Version information.</returns>
        public static AssemblyVersionInfo FromEntryAssembly() {

            Assembly assembly = Assembly.GetEntryAssembly();
            if(assembly == null) {
                assembly = Assembly.GetCallingAssembly();
                if(assembly == null) {
                    assembly = Assembly.GetExecutingAssembly();
                }
            }
            return new AssemblyVersionInfo(assembly);
        }

        /// <summary>
        /// Create an instance of this class to retrieve version information from the specified assembly.
        /// </summary>
        /// <param name="assembly">Assembly to read from.</param>
        public AssemblyVersionInfo(Assembly assembly) {

            _name = assembly.GetName();
            _version = _name.Version.ToString();

            foreach(object attribute in assembly.GetCustomAttributes(false)) {
                if(attribute is AssemblyCompanyAttribute) {
                    _company = ((AssemblyCompanyAttribute) attribute).Company;
                } else if(attribute is AssemblyCopyrightAttribute) {
                    _copyright = ((AssemblyCopyrightAttribute) attribute).Copyright;
                } else if(attribute is AssemblyDescriptionAttribute) {
                    _description = ((AssemblyDescriptionAttribute) attribute).Description;
                } else if(attribute is AssemblyFileVersionAttribute) {
                    _fileVersion = ((AssemblyFileVersionAttribute) attribute).Version;
                } else if(attribute is AssemblyProductAttribute) {
                    _product = ((AssemblyProductAttribute) attribute).Product;
                } else if(attribute is AssemblyTitleAttribute) {
                    _title = ((AssemblyTitleAttribute) attribute).Title;
                } else if(attribute is AssemblyTrademarkAttribute) {
                    _trademark = ((AssemblyTrademarkAttribute) attribute).Trademark;
                }
            }
        }

        #endregion

        #region Public Properties - Raw Data

        /// <summary>
        /// Assembly name.
        /// </summary>
        public AssemblyName Name {
            get {
                return _name;
            }
        }

        /// <summary>
        /// Company name.
        /// </summary>
        public string Company {
            get {
                return _company;
            }
        }

        /// <summary>
        /// Copyright statement (<seealso cref="Trademark"/>).
        /// </summary>
        public string Copyright {
            get {
                return _copyright;
            }
        }

        /// <summary>
        /// Assembly description (<seealso cref="Title"/>).
        /// </summary>
        public string Description {
            get {
                return _description;
            }
        }

        /// <summary>
        /// Win32 file version (<seealso cref="Version"/>).
        /// </summary>
        public string FileVersion {
            get {
                return _fileVersion;
            }
        }

        /// <summary>
        /// Product name.
        /// </summary>
        public string Product {
            get {
                return _product;
            }
        }

        /// <summary>
        /// Assembly title (<seealso cref="Description"/>).
        /// </summary>
        public string Title {
            get {
                return _title;
            }
        }

        /// <summary>
        /// Trademark description (<seealso cref="Copyright"/>).
        /// </summary>
        public string Trademark {
            get {
                return _trademark;
            }
        }

        /// <summary>
        /// .NET assembly version (<seealso cref="FileVersion"/>).
        /// </summary>
        public string Version {
            get {
                return _version;
            }
        }

        #endregion

        #region Public Properties - Derived Data

        /// <summary>
        /// The best human-readable representation of the title and version that can be constructed
        /// from the information available. If the title is not available, the assembly name is used.
        /// If the file version is not available, the assembly version is used. If the assembly version
        /// is not available, it is omitted.
        /// </summary>
        public string DisplayTitleAndVersion {
            get {
                string title = _title;
                if(title == null) {
                    title = _name.Name;
                }
                string version = _fileVersion;
                if(version == null) {
                    version = _version;
                }
                if(version != null) {
                    title += " " + version;
                }
                return title.Trim();
            }
        }

        #endregion
    }
}
