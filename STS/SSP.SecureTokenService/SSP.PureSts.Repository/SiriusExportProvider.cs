using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ssp.PureSts.Repository.Configuration;
using Ssp.PureSts.Repository.Interfaces;


namespace Ssp.PureSts.Repository
{
    public class SiriusExportProvider : ExportProvider
    {
    
        private Dictionary<string, string> _mappings;

        public SiriusExportProvider()
        {
            var section = ConfigurationManager.GetSection(SiriusConfigurationSection.SectionName) as SiriusConfigurationSection;

            _mappings = new Dictionary<string, string>
            {
                { typeof(ISiriusUserRepository).FullName, section.SiriusUserRepository } 
            };
        }

        protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
        {
            var exports = new List<Export>();

            string implementingType;
            if (_mappings.TryGetValue(definition.ContractName, out implementingType))
            {
                var t = Type.GetType(implementingType);
                if (t == null)
                {
                    throw new InvalidOperationException("Type not found for interface: " + definition.ContractName);
                }

                var instance = t.GetConstructor(Type.EmptyTypes).Invoke(null);
                var exportDefintion = new ExportDefinition(definition.ContractName, new Dictionary<string, object>());
                var toAdd = new Export(exportDefintion, () => instance);

                exports.Add(toAdd);
            }

            return exports;
        }
    
    }
}
