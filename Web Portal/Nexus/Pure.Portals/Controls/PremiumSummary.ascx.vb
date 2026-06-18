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

    Partial Class Controls_PremiumSummary : Inherits System.Web.UI.UserControl

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

        Protected Sub btnPremiumSummary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPremiumSummary.Click
            ''redirecting to premium summary page
            'Response.Redirect("~/Modal/PremiumSummary.aspx")
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Dim strMsg As String = GetLocalResourceObject("msg_Pending").ToString.Trim
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            If Not String.IsNullOrEmpty(strMsg) AndAlso (oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.AgentPending And oUserDetails.Key = 0) Then
                Dim strURL As String = ResolveUrl("~/Modal/PremiumSummary.aspx") & "?modal=true&Page=IP&KeepThis=true&TB_iframe=true&height=100&width=100"
                btnPremiumSummary.Attributes.Add("onClick", "javascript:return ConfirmAction('" & strURL & "','" & strMsg & "');")
            Else
                Dim strURL As String = "tb_show(null ,'" & ResolveUrl("~/Modal/PremiumSummary.aspx") & "?SetViewMode=" & bViewMode & "&modal=true&Page=IP&KeepThis=true&TB_iframe=true&height=100&width=100' , null);return false;"
                btnPremiumSummary.Attributes.Add("onclick", strURL)
            End If
        End Sub
    End Class
End Namespace
