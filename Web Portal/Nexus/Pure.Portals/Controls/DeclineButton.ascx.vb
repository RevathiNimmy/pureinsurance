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
Imports System.Data
Imports Nexus
Imports NexusProvider.Quote
Imports System.Linq
Imports System.Text
Imports System.Xml.Linq
Imports System.IO



Namespace Nexus

    Partial Class Controls_DeclineButton : Inherits System.Web.UI.UserControl

        Private _InsuranceFileKey As Integer
        Private _ClaimKey As Integer
        Private _PartyKey As Integer
        Private bPostBack As Boolean = False

        Public Property PostBack() As Boolean
            Get
                Return IsPostBack
            End Get
            Set(ByVal value As Boolean)
                bPostBack = value
            End Set
        End Property

        Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
            DeclineButton(sender, e)
        End Sub

        Public Sub DeclineButton(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            oQuote.QuoteStatusKey = NexusProvider.Policy.QuoteStatusType.Declined
            oWebService.UpdateQuoteStatus(oQuote)
            Session(CNQuote) = oQuote
            AddEventForDeclinedQuote(oQuote)
            '[start]changes for WPR 73_74
            'If an underwriter (non-agency user) is logged
            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            Dim sDesc As String = String.Empty
            Dim sTask As String = String.Empty
            Dim sTaskGroup As String = String.Empty

            If oUserDetails IsNot Nothing AndAlso oUserDetails.Key = 0 AndAlso oQuote.ContactUserName <> "" Then
                If (Session(CNQuoteMode) = QuoteMode.FullQuote) Then 'If NB
                    sTask = "UNDERNB"
                    sTaskGroup = "UNDER"
                ElseIf (Session(CNQuoteMode) = QuoteMode.MTAQuote) Then
                    sTask = "UNDERMTA"
                    sTaskGroup = "UNDER"
                End If
                sDesc = IIf(GetLocalResourceObject("lblTaskDecline") Is Nothing, "Your Quote with Ref No. XXXXX is Declined", GetLocalResourceObject("lblTaskDecline"))
                sDesc = sDesc.Replace("XXXXX", oQuote.InsuranceFileRef)
                CreateTask(oQuote, sDesc, sTask, sTaskGroup)
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oPortalTemp As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
            End If
            'Will open the Modal/SendMail.aspx pop-up to send the mail
            SendMail()
            Exit Sub
            ''[end]changes for WPR 73_74
        End Sub


        'Protected Sub SendMailjob()
        '    Dim sURL As String
        '    Dim oParty As NexusProvider.BaseParty = Session(CNParty)
        '    Dim oQuote As NexusProvider.Quote
        '    Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
        '    If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
        '        oQuote = Session(CNClaimQuote)
        '    Else
        '        oQuote = Session(CNQuote)
        '    End If


        '    sURL = AppSettings("WebRoot") & "Modal/SendEmail.aspx?PartyKey=" & oParty.Key & "&key=" & "&InsuranceFileKey=" & oQuote.InsuranceFileKey & "&modal=true&loc=docm&n=p&Riskcheck=true&KeepThis=true&TB_iframe=true&height=300&width=750"
        '    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", _
        '    "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>")
        '    Exit Sub

        'End Sub

        ''' <summary>
        ''' will add event for the declined quote
        ''' </summary>
        ''' <param name="oquote"></param>
        ''' <remarks></remarks>
        Private Sub AddEventForDeclinedQuote(ByVal oquote As NexusProvider.Quote)
            Dim oEventDetails As New NexusProvider.EventDetails
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            With oEventDetails
                .EventDate = Now()
                .InsuranceFileKey = oquote.InsuranceFileKey
                .InsuranceFileKeySpecified = True
                .InsuranceFolderKey = oquote.InsuranceFolderKey
                .InsuranceFolderKeySpecified = True
                .PartyKey = oquote.PartyKey
                .RtfText = txtReason.Text
                .UserName = Session(CNLoginName)
                .EventTypeKey = 5
                .EventLogSubjectKey = 1
            End With

            oWebService.AddEvent(oEventDetails)
        End Sub


        Protected Sub SendMail()
            Dim sURL As String
            Dim oParty As NexusProvider.BaseParty = Session(CNParty)
            Dim oQuote As NexusProvider.Quote
            Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                oQuote = Session(CNClaimQuote)
            Else
                oQuote = Session(CNQuote)
            End If

            If HttpContext.Current.Session.IsCookieless Then
                sURL = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/SendEmail.aspx?PartyKey=" & oParty.Key & "&key=Declined&InsuranceFileKey=" & oQuote.InsuranceFileKey & "&PostBack=" & bPostBack & "&modal=true&loc=declined&n=p&Riskcheck=true&KeepThis=true&TB_iframe=true&height=300&width=750"
            Else
                sURL = AppSettings("WebRoot") & "/Modal/SendEmail.aspx?PartyKey=" & oParty.Key & "&key=Declined&InsuranceFileKey=" & oQuote.InsuranceFileKey & "&PostBack=" & bPostBack & "&modal=true&loc=declined&n=p&Riskcheck=true&KeepThis=true&TB_iframe=true&height=300&width=750"
            End If

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", _
            "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>")
            Exit Sub
        End Sub
    End Class
End Namespace
