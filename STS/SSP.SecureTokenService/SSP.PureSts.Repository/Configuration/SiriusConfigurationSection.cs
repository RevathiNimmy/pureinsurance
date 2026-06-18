using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssp.PureSts.Repository.Configuration
{
    /// <summary>
    /// The IcmConfigurationSection Configuration Section.
    /// </summary>
    public partial class SiriusConfigurationSection : global::System.Configuration.ConfigurationSection
    {
        public const string SectionName = "ssp.Repository"; // "gstt.repositories";

        /// <summary>
        /// The XML name of the <see cref="ConfigurationProvider"/> property.
        /// </summary>
        internal const global::System.String SiriusUserRepositoryName = "sspUserManagement";

        /// <summary>
        /// Gets or sets type of the class that provides custom user validation
        /// </summary>
        [global::System.Configuration.ConfigurationProperty(SiriusUserRepositoryName, IsRequired = false, IsKey = false, IsDefaultCollection = false, DefaultValue = "Ssp.PureSts.Repository.SiriusUserRepository, Ssp.PureSts.Repository")]
        public global::System.String SiriusUserRepository
        {
            get
            {
                return (global::System.String)base[SiriusUserRepositoryName];
            }
            set
            {
                base[SiriusUserRepositoryName] = value;
            }
        }
    }

}
