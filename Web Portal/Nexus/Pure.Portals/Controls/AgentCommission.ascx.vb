Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports CMS.Library
Imports System.Data
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Xml
Imports Nexus.Utils
Imports Nexus

Namespace Nexus

    Partial Class Controls_AgentCommission
        Inherits System.Web.UI.UserControl

        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oAgentCommission As NexusProvider.AgentCommission
        Dim cTotalAmount As Double = 0.0
        Dim cTotalTax As Double = 0.0
        Dim iInsuranceFilekey As Integer
        Dim cTotalPremium As Double = 0.0
        Dim bIsInBackDatedMode As Boolean = False

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim oQuote As NexusProvider.Quote
            Dim oHeaderAndAgentCommission As NexusProvider.HeaderAndAgentCommission
            Dim oAgentCommissionCollection As NexusProvider.AgentCommissionCollection
            Dim oEditAgentCommission As NexusProvider.EditAgentCommission
            Try

                oQuote = Session(CNQuote)
                iInsuranceFilekey = oQuote.InsuranceFileKey
                bIsInBackDatedMode = IIf(Session(CNBaseInsuranceFileKey) IsNot Nothing AndAlso Session(CNBaseInsuranceFileKey) <> Session(CNInsuranceFileKey), True, False)

                If Request("__EVENTARGUMENT") = "RefreshAgentCommission" Then
                    'On change of commission
                    oEditAgentCommission = DirectCast(Session(CNAgentCommission), NexusProvider.EditAgentCommission)

                    If oEditAgentCommission IsNot Nothing AndAlso oEditAgentCommission.AgentCommission.Count > 0 Then
                        grdvAgentCommission.DataSource = oEditAgentCommission.AgentCommission
                        grdvAgentCommission.DataBind()
                        'this is used to calculate the total Agent Commission amount in the Quote.
                        Dim count As Integer
                        For count = 0 To oEditAgentCommission.AgentCommission.Count - 1
                            cTotalAmount = oEditAgentCommission.AgentCommission(count).CommissionValue + cTotalAmount
                            cTotalTax = oEditAgentCommission.AgentCommission(count).TaxValue + cTotalTax
                            cTotalPremium = oEditAgentCommission.AgentCommission(count).Premium + cTotalPremium
                        Next
                        Session(CNAgentComm) = cTotalAmount + cTotalTax
                        If cTotalPremium >= 0 Then
                            lblAgentCommissionValue.Text = New Money(cTotalAmount, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                        Else
                            lblAgentCommissionValue.Text = New Money(-(cTotalAmount), New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                        End If

                        'we should check that if premium is -900 and commission is -100 then commission is greater than  premium so
                        'we should convert them in + values from - values
                        If cTotalAmount < 0 Then
                            cTotalAmount = Math.Abs(cTotalAmount)
                        End If
                        If cTotalPremium < 0 Then
                            cTotalPremium = Math.Abs(cTotalPremium)
                        End If
                        'end

                        Session(CNCommissionGreaterThanPremium) = False
                        For count = 0 To oEditAgentCommission.AgentCommission.Count - 1
                            If Math.Abs(oEditAgentCommission.AgentCommission(count).CommissionValue) > Math.Abs(oEditAgentCommission.AgentCommission(count).Premium) Then
                                Session(CNCommissionGreaterThanPremium) = True
                                Exit For
                            End If
                        Next
                    Else
                        'Page will be post back if 'Total Commission is more than the Premium'
                        'need to re-bind the grid again
                        grdvAgentCommission.DataSource = Session("AgentCommissionCollection")
                        grdvAgentCommission.DataBind()
                    End If
                Else
                    'On load of the page first time
                    oHeaderAndAgentCommission = oWebService.GetHeaderAndAgentCommissionByKey(iInsuranceFilekey)

                    oAgentCommissionCollection = oHeaderAndAgentCommission.AgentCommission

                    'storing in session to bind the grid again in case of post back and avoid the SAM call
                    Session("AgentCommissionCollection") = oAgentCommissionCollection

                    If oAgentCommissionCollection IsNot Nothing And oAgentCommissionCollection.Count > 0 Then

                        grdvAgentCommission.DataSource = oAgentCommissionCollection
                        grdvAgentCommission.DataBind()

                        'this is used to calculate the total Agent Commission amount in the Quote.
                        Dim count As Integer
                        For count = 0 To oAgentCommissionCollection.Count - 1
                            cTotalAmount = oAgentCommissionCollection(count).CommissionValue + cTotalAmount
                            cTotalTax = oAgentCommissionCollection(count).TaxValue + cTotalTax
                            cTotalPremium = oAgentCommissionCollection(count).Premium + cTotalPremium
                        Next
                        Session(CNAgentComm) = cTotalAmount + cTotalTax
                        If cTotalPremium >= 0 Then
                            lblAgentCommissionValue.Text = New Money(cTotalAmount, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                        Else
                            lblAgentCommissionValue.Text = New Money(-(cTotalAmount), New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                        End If

                        'we should check that if premium is -900 and commission is -100 then commission is greater than  premium so
                        'we should convert them in + values from - values
                        If cTotalAmount < 0 Then
                            cTotalAmount = Math.Abs(cTotalAmount)
                        End If
                        If cTotalPremium < 0 Then
                            cTotalPremium = Math.Abs(cTotalPremium)
                        End If
                        'end

                        Session(CNCommissionGreaterThanPremium) = False
                        For count = 0 To oAgentCommissionCollection.Count - 1
                            If Math.Abs(oAgentCommissionCollection(count).CommissionValue) > Math.Abs(oAgentCommissionCollection(count).Premium) Then
                                Session(CNCommissionGreaterThanPremium) = True
                                Exit For
                            End If
                        Next
                    Else
                        agentcommission_control.Visible = False
                        AgentAmountPanel.Visible = False
                    End If

                End If

            Finally
                oQuote = Nothing
                oHeaderAndAgentCommission = Nothing
                oWebService = Nothing
            End Try

        End Sub

        Protected Sub grdvAgentCommission_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvAgentCommission.Load
            If grdvAgentCommission.PageCount = 1 Then
                grdvAgentCommission.AllowPaging = False
            End If
        End Sub

        Protected Sub grdvAgentCommission_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvAgentCommission.PageIndexChanging
            grdvAgentCommission.DataSource = Session("AgentCommissionCollection")
            grdvAgentCommission.PageIndex = e.NewPageIndex
            grdvAgentCommission.DataBind()
        End Sub



        Protected Sub grdvAgentCommission_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvAgentCommission.RowDataBound

            If e.Row.RowType = DataControlRowType.DataRow Then
                oWebService = New NexusProvider.ProviderManager().Provider
                Dim chkIsLeadAgent As CheckBox = CType(e.Row.FindControl("IsLeadAgent"), CheckBox)
                oAgentCommission = New NexusProvider.AgentCommission
                oAgentCommission = CType(e.Row.DataItem, NexusProvider.AgentCommission)
                chkIsLeadAgent.Checked = oAgentCommission.IsLeadAgent

                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("AgentType") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.AgentCommission).AgentType)

                'Begin - WPR 64 - Commission Maintenance.
                Dim oUserAuthority As New NexusProvider.UserAuthority

                oUserAuthority.UserCode = Session(CNLoginName)
                'Set User User Authority Option As per transaction type
                If Session(CNMTAType) Is Nothing Then 'NB
                    oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.EditDefaultCommissionNBRN
                ElseIf Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Then 'MTA
                    oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.EditDefaultCommissionMTA
                ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then 'MTC
                    oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.EditDefaultCommissionMTC
                ElseIf Session(CNMTAType) = MTAType.REINSTATEMENT Then 'MTR
                    oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.EditDefaultCommissionMTR
                End If
                oWebService.GetUserAuthorityValue(oUserAuthority)

                If oUserAuthority.UserAuthorityValue AndAlso Session(CNMode) <> Mode.View AndAlso bIsInBackDatedMode = False Then
                    Dim oAgentCommEdit As LinkButton = CType(e.Row.FindControl("hypAgentCommEdit"), LinkButton)
                    oAgentCommEdit.Visible = True
                    If HttpContext.Current.Session.IsCookieless Then
                        oAgentCommEdit.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/EditCommission.aspx?Index=" & (e.Row.DataItemIndex) & "&RiskType=" + e.Row.Cells(2).Text & "&CommissionBand=" + e.Row.Cells(3).Text.Trim & "&InsuranceFileKey=" + iInsuranceFilekey.ToString() + "&Agent=" + HttpUtility.UrlEncode(oAgentCommission.Agent) + "&PostbackTo=" & UpdAgentComm.ClientID.ToString & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    Else
                        oAgentCommEdit.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "/Modal/EditCommission.aspx?Index=" & (e.Row.DataItemIndex) & "&RiskType=" + e.Row.Cells(2).Text & "&CommissionBand=" + e.Row.Cells(3).Text.Trim & "&InsuranceFileKey=" + iInsuranceFilekey.ToString() + "&Agent=" + HttpUtility.UrlEncode(oAgentCommission.Agent) + "&PostbackTo=" & UpdAgentComm.ClientID.ToString & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    End If
                End If


                'End - WPR 64 - Commission Maintenance.

                'set format of column CommissionRate based on configuration IsValue
                If oAgentCommission.IsValue Then
                    e.Row.Cells(5).Text = Math.Round(oAgentCommission.CommissionRate, 10).ToString
                Else
                    'if IsValue =false, means this is set as "%" in BO so apply format with "%" symbol
                    e.Row.Cells(5).Text = String.Format("{0:0.00}", oAgentCommission.CommissionRate) & GetLocalResourceObject("lbl_percentage").ToString()
                End If
                oAgentCommission = Nothing
            End If

        End Sub

    End Class

End Namespace
