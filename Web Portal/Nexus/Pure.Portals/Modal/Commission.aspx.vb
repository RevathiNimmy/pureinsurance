Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Library
Imports System.Web.Configuration
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Constants.Session
Imports Nexus.Constants
Imports SiriusFS.SAM.Client
Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Xml
Imports System.Xml.XPath
Imports System.Xml.XmlReader
Imports System.Globalization.CultureInfo
Imports System.Linq
Imports System.Xml.Linq

'<script type="text/javascript">

'    function setDefaultValues() {



'    }   
'</script>
Namespace Nexus
    Partial Class Modal_Commission
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim bViewMode As Boolean = False
            'check if view mode set for specific case
            If Request.QueryString("SetViewMode") IsNot Nothing Then
                bViewMode = Convert.ToBoolean(Request.QueryString("SetViewMode"))
            End If
            Commission.SetViewMode = bViewMode
        End Sub

        Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub
       
    End Class
End Namespace
