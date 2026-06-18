Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Constants
Imports Nexus.Constants.Session


Namespace Nexus

    Partial Class secure_agent_FindAgent
        Inherits BaseFindParty

        Private oParty As New NexusProvider.PartyCollection

        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            grdvSearchResults.Visible = True
            Select Case ddlAgentType.SelectedValue
                Case "ALL"
                    Session(CNSearchAgentType) = PartyAgentType.All
                    Session(CNSearchType) = PartyType.AG
                Case "Broker"
                    Session(CNSearchAgentType) = PartyAgentType.Broker
                    Session(CNSearchType) = PartyType.AG
                Case "SubAgent"
                    Session(CNSearchAgentType) = PartyAgentType.SubAgent
                    Session(CNSearchType) = PartyType.AG
                Case "CA"
                    Session(CNSearchAgentType) = PartyAgentType.CommissionAccount
                    Session(CNSearchType) = PartyType.AG
                Case "Inter"
                    Session(CNSearchAgentType) = PartyAgentType.Intermediary
                    Session(CNSearchType) = PartyType.AG
            End Select

            FindParty()

        End Sub
        Protected Sub grdvSearchResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvSearchResults.RowDataBound

            Dim i As Integer = 0
            Dim sAgentName As String = ""
            If e.Row.RowType = DataControlRowType.DataRow Then
                'make visile the sub agent select link
                Dim oSelectlink As LinkButton = CType(e.Row.Cells(4).FindControl("lnkbtnSelect"), LinkButton)

                Select Case True
                    Case TypeOf e.Row.DataItem Is NexusProvider.PersonalParty
                        oParty.Add(CType(e.Row.DataItem, NexusProvider.PersonalParty))
                        'NOTE - this will need to be changed to give each row a unique id
                        'this needs to be matched in markup for the menu (id="Menu_<%# Eval("UserName") %>")
                        e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.PersonalParty).UserName)
                        sAgentName = CType(e.Row.DataItem, NexusProvider.PersonalParty).ResolvedName.Trim
                        For i = 0 To sAgentName.Length - 1
                            If sAgentName.Substring(i, 1) = "'" Then
                                sAgentName = sAgentName.Substring(0, i) + "\" + sAgentName.Substring(i, sAgentName.Length - i)
                                i = i + 1
                            End If
                        Next
                        If Request.QueryString("AgentType") = "SubAgent" Then
                            oSelectlink.Attributes.Add("onclick", "self.parent.setSubAgent('" & PureEncode(sAgentName) & "','" & CType(e.Row.DataItem, NexusProvider.PersonalParty).Key & "','" & PureEncode(CType(e.Row.DataItem, NexusProvider.PersonalParty).UserName) & "')")
                        Else
                            oSelectlink.Attributes.Add("onclick", "self.parent.setAgent('" & PureEncode(sAgentName) & "','" & CType(e.Row.DataItem, NexusProvider.PersonalParty).Key & "','" & PureEncode(CType(e.Row.DataItem, NexusProvider.PersonalParty).UserName) & "','" & CType(e.Row.DataItem, NexusProvider.PersonalParty).AgentType & "')")
                        End If

                    Case TypeOf e.Row.DataItem Is NexusProvider.CorporateParty
                        oParty.Add(CType(e.Row.DataItem, NexusProvider.CorporateParty))
                        'NOTE - this will need to be changed to give each row a unique id
                        'this needs to be matched in markup for the menu (id="Menu_<%# Eval("UserName") %>")
                        e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.CorporateParty).UserName)
                        sAgentName = CType(e.Row.DataItem, NexusProvider.CorporateParty).ResolvedName.Trim
                        For i = 0 To sAgentName.Length - 1
                            If sAgentName.Substring(i, 1) = "'" Then
                                sAgentName = sAgentName.Substring(0, i) + "\" + sAgentName.Substring(i, sAgentName.Length - i)
                                i = i + 1
                            End If
                        Next
                        If Request.QueryString("AgentType") = "SubAgent" Then
                            oSelectlink.Attributes.Add("onclick", "self.parent.setSubAgent('" & PureEncode(sAgentName) & "','" & CType(e.Row.DataItem, NexusProvider.CorporateParty).Key & "','" & PureEncode(CType(e.Row.DataItem, NexusProvider.CorporateParty).UserName) & "')")
                        Else
                            oSelectlink.Attributes.Add("onclick", "self.parent.setAgent('" & PureEncode(sAgentName) & "','" & CType(e.Row.DataItem, NexusProvider.CorporateParty).Key & "','" & PureEncode(CType(e.Row.DataItem, NexusProvider.CorporateParty).UserName) & "','" & CType(e.Row.DataItem, NexusProvider.CorporateParty).AgentType & "')")
                        End If

                End Select

                CType(e.Row.FindControl("ltAddressLine1"), Literal).Text = oParty(e.Row.RowIndex).AddressLine1
                CType(e.Row.FindControl("ltAddressLine2"), Literal).Text = oParty(e.Row.RowIndex).AddressLine2

                If Request.QueryString("FromPage") = "MainDetails" Then
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                    If oQuote Is Nothing Then
                        Exit Sub
                    End If
                    Dim dtAgencyCancellationDate As DateTime
                    Dim dtDateToValidateForAgencyCancellation As DateTime
                    Dim dDefaultDate As Date
                    Dim oValidateCancelledAgentOrBroker As New NexusProvider.OptionTypeSetting
                    oValidateCancelledAgentOrBroker = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1040)

                    dDefaultDate = ToSafeDate("29/12/1899", #12/29/1899#)

                    If Not oValidateCancelledAgentOrBroker.OptionValue Is Nothing Then
                        If oValidateCancelledAgentOrBroker.OptionValue = "1" Then 'CoverStartDate
                            If Request.QueryString("COVERSTARTDATE") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Request.QueryString("COVERSTARTDATE")) Then
                                dtDateToValidateForAgencyCancellation = CDate(Request.QueryString("COVERSTARTDATE"))
                            Else
                                dtDateToValidateForAgencyCancellation = oQuote.CoverStartDate
                            End If
                        ElseIf oValidateCancelledAgentOrBroker.OptionValue = "0" Then 'TransactionDate
                            dtDateToValidateForAgencyCancellation = DateTime.Now
                        End If
                    Else
                        dtDateToValidateForAgencyCancellation = DateTime.Now
                    End If
                    dtAgencyCancellationDate = oParty(e.Row.RowIndex).DateCancelled

                    If (ToSafeDate(dtAgencyCancellationDate, #12/29/1899#) <= dtDateToValidateForAgencyCancellation) And (ToSafeDate(dtAgencyCancellationDate, #12/29/1899#) <> dDefaultDate) Then
                        oSelectlink.Attributes.Add("onclick", "Javascript:alert('" + GetLocalResourceObject("CancelAgentMessage") + "')")
                    End If
                End If
            End If
        End Sub

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'To set the Focus
            Page.SetFocus(txtAgent_code)

            ' If Not IsPostBack Then
            If Request.QueryString("AgentType") = "SubAgent" Then
                ddlAgentType.Items.Clear()
                ddlAgentType.Items.Add(New ListItem(GetLocalResourceObject("ddlAgentType_SubAgent"), "SubAgent"))
                ddlAgentType.SelectedIndex = 0

                If Session(CNSubAgents) Is Nothing Then
                    Session.Add(CNSubAgents, New NexusProvider.SubAgentCollection)
                End If
            ElseIf Request.QueryString("FromPage") <> "MainDetails" Then
                ddlAgentType.Items.Add(New ListItem(GetLocalResourceObject("ddlAgentType_SubAgent"), "SubAgent"))
                ddlAgentType.SelectedIndex = 0
            End If
            'Fill Agent Groups
            Dim oAgentGroups As NexusProvider.PartyCollection = GetAgentGroups(Session(CNBranchCode))
            ddlAgentGroup.DataSource = oAgentGroups
            ddlAgentGroup.DataValueField = "UserName"
            ddlAgentGroup.DataTextField = "Name"
            ddlAgentGroup.DataBind()
            ddlAgentGroup.Items.Insert(0, New ListItem(GetLocalResourceObject("lbl_DefaultItem"), ""))

            'End If



        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            txtAgent_code.Text = String.Empty
            txtName.Text = String.Empty
            txtFileCode.Text = String.Empty
            ddlAgentType.SelectedIndex = 0
            chkIncludeClosedBranches.Checked = False
            ddlAgentGroup.SelectedIndex = 0
            Session.Remove(CNSearchData)
            grdvSearchResults.Visible = False
        End Sub
        Public Function ToSafeDate(ByVal Expression As Object, Optional ByRef Default_Renamed As Date = #12/29/1899#) As Date

            Dim result As Date
            Try
                If Date.TryParse(Expression, result) Then
                    Return result
                Else
                    Return Default_Renamed
                End If
            Catch
                Return Default_Renamed
            End Try

        End Function
    End Class

End Namespace
