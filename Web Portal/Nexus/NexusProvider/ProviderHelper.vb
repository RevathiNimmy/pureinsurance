Public Class ProviderHelper

    ''' <summary>
    ''' Perform an XSLT transform on the provided XML to format into a more readable format.
    ''' </summary>
    ''' <param name="v_sXML">The XML string to be transformed</param>
    ''' <returns>A HTML string containing the format XML</returns>
    ''' <remarks>Uses the XSLT from IE6</remarks>
    Public Shared Function PrettyFormatXMLToHTML(ByVal v_sXML As String) As String

        Dim oXPathDoc As New Xml.XPath.XPathDocument(New IO.StringReader(v_sXML))
        Dim oXSLDoc As New Xml.Xsl.XslCompiledTransform
        Dim oFormattedXML As New System.IO.StringWriter

        oXSLDoc.Load(New Xml.XmlTextReader(New IO.StringReader(My.Resources._default)))
        oXSLDoc.Transform(oXPathDoc, Nothing, oFormattedXML)

        PrettyFormatXMLToHTML = oFormattedXML.ToString
        oFormattedXML.Close()

    End Function

End Class
