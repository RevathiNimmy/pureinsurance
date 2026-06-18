Imports Nexus.Library
Imports CMS.library
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class Modal_AddEvent
        Inherits System.Web.UI.Page
        Private v_iPartyKey As Integer

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim oParty As NexusProvider.BaseParty = Nothing

            'To set the Focus
            Page.SetFocus(ddlContext)

            'Get the Party from the session.
            If Request.QueryString("CallingPage") IsNot Nothing AndAlso Request.QueryString("CallingPage") = "CashListItem" Then
                If Request.QueryString("DeclinePartyKey") IsNot Nothing Then
                    v_iPartyKey = Request.QueryString("DeclinePartyKey")
                End If
            Else

                'Get the Party from the session.
                If Session(CNParty) IsNot Nothing Then
                    oParty = Session.Item(CNParty)
                    v_iPartyKey = oParty.Key
                ElseIf Request.QueryString("PartyKey") IsNot Nothing Then
                    v_iPartyKey = Convert.ToInt32(Request.QueryString("PartyKey"))
                End If
            End If

            If Not IsPostBack Then
                FillEvent()
            End If
        End Sub
        Sub FillEvent()
            If Session(CNClaim) IsNot Nothing Then
                ddlContext.Items.Clear()
                ddlContext.Items.Add(New ListItem("Notes - Claims", "22"))
                ddlContext.Items.Add(New ListItem("Notes - Customer", "20"))
                ddlContext.Items.Add(New ListItem("Notes-Customer Warning", "37"))
            Else
                ddlContext.Items.Clear()
                ddlContext.Items.Add(New ListItem("Notes - Customer", "20"))
                ddlContext.Items.Add(New ListItem("Notes-Customer Warning", "37"))
                ddlContext.Items.Add(New ListItem("Notes-Policy", "23"))
            End If
            ddlContext.Items.Insert(0, New ListItem("EMPTY", ""))
            ddlContext.SelectedIndex = 0
        End Sub
        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oEventDetails As New NexusProvider.EventDetails

            'Try
            If (v_iPartyKey > 0) Then


                If Page.IsValid Then
                    With oEventDetails
                        .EventDate = Now()
                        .PartyKey = v_iPartyKey
                        .RtfText = txtDetails.Text.Trim()
                        .UserName = Session(CNLoginName)
                        .EventLogSubjectKey = Int32.Parse(ddlSubject.Value)

                        If String.IsNullOrEmpty(ddlContext.SelectedValue) Then
                            If Session(CNClaim) IsNot Nothing Then
                                .EventTypeKey = 22
                            Else
                                .EventTypeKey = 20
                            End If
                        Else
                            .EventTypeKey = Int32.Parse(ddlContext.SelectedValue)
                        End If

                        If Session(CNClaim) IsNot Nothing Then
                            Dim oOpenClaim As NexusProvider.ClaimOpen = Session(CNClaim)
                            .InsuranceFileKey = oOpenClaim.InsuranceFileKey
                            .ClaimKey = oOpenClaim.ClaimKey
                        End If

                        If Session(CNQuote) IsNot Nothing Then
                            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                            .InsuranceFileKey = oQuote.InsuranceFileKey
                            .InsuranceFolderKey = oQuote.InsuranceFolderKey
                        End If


                        If ddlContext.SelectedValue = "37" Then
                            'Priority
                            .Priority = ddlPriority.SelectedValue
                            'Status
                            .StatusKey = ddlStatus.SelectedValue
                        End If

                    End With

                    oWebService.AddEvent(oEventDetails)
                End If
                'set up javascript to postback the parent page
                'this will trigger a partial postback and close the thickbox
                ' Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_updated('" & Request.QueryString("PostbackTo") & "');", True)
            End If

            If Request.QueryString("CallingPage") IsNot Nothing AndAlso Request.QueryString("CallingPage") = "CashListItem" Then
                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.OpenWorkManager();", True)
            Else
                Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "Refresh") & ";"
                Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
            End If

        End Sub

    End Class
End Namespace