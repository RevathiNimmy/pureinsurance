Imports System.Web.UI.WebControls
Imports System.Web.HttpContext
Imports System.Web.Configuration.WebConfigurationManager

Namespace Portal

    Public Module Culture

        Function GetCultures() As XmlDataSource

            Dim sDataFile As String = "~/Configuration/" & AppSettings("ClientConfigurationFolder") & "/CultureCodes.xml"
            Dim xmlds As New XmlDataSource
            xmlds.DataFile = Current.Server.MapPath(sDataFile)
            xmlds.XPath = "cultures/code"

            Return xmlds

        End Function

    End Module

End Namespace
