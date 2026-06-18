using System.Collections.Generic;
using System.Xml.Serialization;

namespace bSIRSharepointApi.Models
{
    [XmlRoot(ElementName = "Field")]
    public class Field
    {

        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "DisplayName")]
        public string DisplayName { get; set; }

        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }

        //[XmlAttribute(AttributeName = "Required")]
        //public bool Required { get; set; }

        [XmlAttribute(AttributeName = "Group")]
        public string Group { get; set; }
    }

    [XmlRoot(ElementName = "FieldRef")]
    public class FieldRef
    {

        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "DisplayName")]
        public string DisplayName { get; set; }

        //[XmlAttribute(AttributeName = "Required")]
        //public bool Required { get; set; }

        //[XmlAttribute(AttributeName = "Hidden")]
        //public bool Hidden { get; set; }
    }

    [XmlRoot(ElementName = "FieldRefs")]
    public class FieldRefs
    {

        [XmlElement(ElementName = "FieldRef")]
        public List<FieldRef> FieldRef { get; set; }
    }

    [XmlRoot(ElementName = "ContentType")]
    public class ContentType
    {

        [XmlElement(ElementName = "FieldRefs")]
        public FieldRefs FieldRefs { get; set; }

        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }

        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "Description")]
        public string Description { get; set; }

        [XmlAttribute(AttributeName = "Group")]
        public string Group { get; set; }

        [XmlAttribute(AttributeName = "BaseType")]
        public string BaseType { get; set; }

        [XmlAttribute(AttributeName = "Version")]
        public int Version { get; set; }

        //[XmlAttribute(AttributeName = "Sealed")]
        //public bool Sealed { get; set; }

        //[XmlAttribute(AttributeName = "Hidden")]
        //public bool Hidden { get; set; }

        //[XmlAttribute(AttributeName = "ReadOnly")]
        //public bool ReadOnly { get; set; }
    }

    [XmlRoot(ElementName = "Elements",Namespace = "http://schemas.microsoft.com/sharepoint/")]
    public class Elements
    {

        [XmlElement(ElementName = "Field")]
        public List<Field> Field { get; set; }

        [XmlElement(ElementName = "ContentType")]
        public ContentType ContentType { get; set; }

        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }


}
