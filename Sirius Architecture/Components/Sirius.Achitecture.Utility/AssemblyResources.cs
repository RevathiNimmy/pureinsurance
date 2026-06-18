using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Sirius.Architecture.Utility {

    /// <summary>
    /// This class encapsulates the process of reading files that were embedded into the resources of an assembly.
    /// </summary>
    public class AssemblyResources {

        #region Private Fields

        private Assembly _assembly;
        private string _rootNamespace;

        #endregion

        #region Constructors

        /// <summary>
        /// Create an instance of this class from a type. The type must belong to the assembly from which the resources
        /// are to be read, and must also belong to the same namespace as the root namespace of the assembly.
        /// </summary>
        /// <param name="type">Type to deduce resource location information from.</param>
        public AssemblyResources(Type type) {

            _assembly = type.Assembly;
            string dummy;
            _rootNamespace = StringSplit.HeadTail(type.FullName, out dummy, ".", StringComparison.Ordinal, StringSplitHeadTailOptions.SearchFromEnd);
        }

        /// <summary>
        /// Create an instance of this class from the specified assembly and root namespace.
        /// </summary>
        /// <param name="assembly">Assembly in which the resources are embedded.</param>
        /// <param name="rootNamespace">Root namespace under which the resources are named.</param>
        public AssemblyResources(Assembly assembly, string rootNamespace) {

            _assembly = assembly;
            _rootNamespace = rootNamespace;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Read an embedded file resource and return the contents as a stream.
        /// </summary>
        /// <param name="fileName">Filename to load.</param>
        /// <returns>The contents of the file.</returns>
        public Stream ReadStream(string fileName) {

            string resourceName = _rootNamespace + (_rootNamespace.Length > 0 ? "." : "") + fileName;
            Stream stream = _assembly.GetManifestResourceStream(resourceName);
            if(stream == null) {
                throw new FileNotFoundException(string.Format(Properties.Resources.ResourceNameNotFoundMessage, resourceName));
            }
            return stream;
        }

        /// <overloads>
        /// Read an embedded text file resource and return the contents as a string.
        /// </overloads>
        /// <summary>
        /// Read an embedded text file resource and return the contents as a string.
        /// </summary>
        /// <param name="fileName">Filename to load.</param>
        /// <returns>The contents of the file.</returns>
        public string ReadTextFile(string fileName) {

            using(StreamReader reader = new StreamReader(ReadStream(fileName))) {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Read an embedded text file resource and return the contents as a string.
        /// </summary>
        /// <param name="fileName">Filename to load.</param>
        /// <param name="encoding">Character encoding to use.</param>
        /// <returns>The contents of the file.</returns>
        public string ReadTextFile(string fileName, Encoding encoding) {

            using(StreamReader reader = new StreamReader(ReadStream(fileName), encoding)) {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Read an embedded text file resource and return the contents as a string.
        /// </summary>
        /// <param name="fileName">Filename to load.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Attempt to deduce the encoding from byte order marks? Warning - this is not very reliable. It's much safer to specify an encoding instead.</param>
        /// <returns>The contents of the file.</returns>
        public string ReadTextFile(string fileName, bool detectEncodingFromByteOrderMarks) {

            using(StreamReader reader = new StreamReader(ReadStream(fileName), detectEncodingFromByteOrderMarks)) {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Read an embedded text file resource and return the contents as a string.
        /// </summary>
        /// <param name="fileName">Filename to load.</param>
        /// <param name="encoding">Character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Attempt to deduce the encoding from byte order marks? Warning - this is not very reliable. It's much safer to specify an encoding instead.</param>
        /// <returns>The contents of the file.</returns>
        public string ReadTextFile(string fileName, Encoding encoding, bool detectEncodingFromByteOrderMarks) {

            using(StreamReader reader = new StreamReader(ReadStream(fileName), encoding, detectEncodingFromByteOrderMarks)) {
                return reader.ReadToEnd();
            }
        }

        #endregion
    }
}
