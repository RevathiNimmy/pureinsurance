Imports SiriusFS.SAM.Client
Imports Microsoft.VisualBasic
Imports System.Xml
Imports System.Xml.XPath
Imports System.Xml.XmlReader
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.HttpContext
Imports Nexus.Utils
Imports Nexus.Library
Imports CMS.Library
Imports System.Globalization.CultureInfo
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class Controls_CommissionUpdate : Inherits System.Web.UI.UserControl
        'set the mode to view for specific case when session mode is not view but control work in view mode
        Private bViewMode As Boolean = False
        ''' <summary>
        ''' property to set view mode in any specific case
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property SetViewMode() As Boolean
            Set(ByVal value As Boolean)
                bViewMode = value
            End Set
        End Property

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim strURL As String = "tb_show(null ,'" & ResolveUrl("~/Modal/Commission.aspx") & "?SetViewMode=" & bViewMode & "&modal=true&Page=IP&KeepThis=true&TB_iframe=true&height=100&width=100' , null);return false;"
            btnCommissionUpdate.Attributes.Add("onclick", strURL)
        End Sub
    End Class
End Namespace
