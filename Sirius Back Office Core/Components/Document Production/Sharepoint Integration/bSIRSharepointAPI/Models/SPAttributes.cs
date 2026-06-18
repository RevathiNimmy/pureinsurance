using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace bSIRSharepointApi.Models
{
    public class ClsSPAttributes
    {
        /// <summary>
        /// SPAttributes - Field Names
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> SPAttributes()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Elements));
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(this.GetType().Assembly.GetManifestResourceStream("bSIRSharepointApi.elements.xml"));
            Dictionary<string, object> SPAttributesFields = new Dictionary<string, object>();
            using (StringReader reader = new StringReader(xmlDocument.OuterXml))
            {
                var test = (Elements)serializer.Deserialize(reader);

                for (int iCnt = 0; iCnt < test.Field.Count; iCnt++)
                {
                    SPAttributesFields.Add(test.Field[iCnt].Name, test.Field[iCnt].Type);
                }
            }

            return SPAttributesFields;
        }

    }
}
