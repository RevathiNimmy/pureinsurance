Imports System.Xml.Serialization

<XmlRoot(ElementName:="Query")>
Public Class Query
    <XmlElement(ElementName:="DataSourceName")>
    Public Property DataSourceName As String
    <XmlElement(ElementName:="CommandType")>
    Public Property CommandType As String
    <XmlElement(ElementName:="CommandText")>
    Public Property CommandText As String
End Class
