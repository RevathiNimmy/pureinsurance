Imports Nexus.Library
Imports CMS.Library
Imports System.Data
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Utils

Namespace Nexus
    Partial Class Controls_RenewalQuotes
        Inherits System.Web.UI.UserControl

        Protected Sub grdvRenQuotes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvRenQuotes.Load
            If grdvRenQuotes.PageCount = 1 Then
                grdvRenQuotes.AllowPaging = False
            End If
        End Sub


        Protected Sub grdvRenQuotes_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvRenQuotes.PageIndexChanging
            grdvRenQuotes.PageIndex = e.NewPageIndex
            grdvRenQuotes.DataSource = Session(CNSearchResults)
            grdvRenQuotes.DataBind()
        End Sub
        Protected Sub grdvRenQuotes_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvRenQuotes.RowCommand
            If Not LCase(e.CommandName).Equals("page") Then
                If e.CommandName = "Details" Then

                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oQuote As NexusProvider.Quote
                    Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                    Try
                        oQuote = oWebService.GetHeaderAndSummariesByKey(e.CommandArgument)

                        If Session(CNParty) Is Nothing Then
                            oParty = oWebService.GetParty(oQuote.PartyKey)
                            Session(CNParty) = oParty
                        End If

                        'Put highest risk key into Session
                        For i As Integer = 0 To oQuote.Risks.Count - 1
                            oWebService.GetRisk(oQuote.Risks(i).Key, i, oQuote, oQuote.BranchCode)
                        Next

                        Session(CNCurrenyCode) = oQuote.CurrencyCode
                        Session(CNQuote) = oQuote

                        'Use the GetDataSetDefinition to interogate the dataset to get the datamodelcode into session
                        GetDataSetDefinition()
                    Finally

                    End Try

                    Session.Remove(CNMTAType)
                    Session.Remove(CNMTATypeDesc)
                    Session.Remove(CNRenewal)
                    Session.Remove(CNRenewalShowPremium)

                    Session.Remove(CNMode)
                    Session(CNMode) = Mode.Buy
                    Session.Remove(CNOI)
                    Session(CNQuoteInSync) = False
                    ' Session(CNInsuranceFileKey) = e.CommandArgument
                    Session(CNRenewal) = True
                    Session.Remove(CNQuoteMode)
                    Session(CNQuoteMode) = QuoteMode.FullQuote
                    Response.Redirect("~/secure/PremiumDisplay.aspx", True)
                End If
            End If

        End Sub
        Protected Sub grdvRenQuotes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvRenQuotes.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(0).Visible = False
                Dim lnkbtnSelect As LinkButton = e.Row.FindControl("lnkbtnSelect")
                lnkbtnSelect.CommandArgument = CType(e.Row.DataItem, NexusProvider.Policy).InsuranceFileKey

                Dim ChkSelect As CheckBox = e.Row.FindControl("chkSelection")
                ChkSelect.Checked = CType(e.Row.DataItem, NexusProvider.Policy).IsSelected
            ElseIf e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(0).Visible = False
            End If

        End Sub
        Public Function GetInsuranceFileKeys() As StringBuilder
            Dim i As Integer
            Dim isChecked As Boolean
            Dim StrInsuranceFiles As New StringBuilder
            'This will Loop through all the Rows inside a GridView
            For i = 0 To grdvRenQuotes.Rows.Count - 1

                Dim row As GridViewRow
                row = grdvRenQuotes.Rows(i)
                'Find the checkbox control if it is checked
                isChecked = CType(row.FindControl("chkSelection"), CheckBox).Checked

                'if checked then append the Policy to the String Builder
                If (isChecked) Then
                    Dim iInsurancefilekey As Integer
                    iInsurancefilekey = grdvRenQuotes.DataKeys(i).Values(1).ToString
                    StrInsuranceFiles.Append(iInsurancefilekey & " ")
                    CType(row.FindControl("chkSelection"), CheckBox).Checked = False
                End If

            Next
            Return StrInsuranceFiles
        End Function

      
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "DeleteConfirmation", _
                "<script language=""JavaScript"" type=""text/javascript"">function DeleteConfirmation(){return confirm('" & GetLocalResourceObject("msg_Delete").ToString() & "');}</script>")

        End Sub

      
        Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click

            Dim StrInsuranceFilesGroup As StringBuilder
            StrInsuranceFilesGroup = GetInsuranceFileKeys()
            If StrInsuranceFilesGroup.Length = 0 Then
                lblNoPoliciesSelected.Visible = True
            Else
                Dim oPolCol As NexusProvider.PolicyCollection = Session(CNSearchResults)
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                For iCount As Integer = 0 To oPolCol.Count - 1
                    If oPolCol(iCount).IsSelected = True Then
                        Dim oQuote As NexusProvider.Quote
                        oQuote = oWebService.GetHeaderAndSummariesByKey(oPolCol(iCount).InsuranceFileKey)
                        oWebService.DeleteRenewal(oQuote, oQuote.BranchCode)
                    End If
                Next
                FillRenewalRecords()
            End If
        End Sub
        Protected Sub PopulateDropDown()
            Dim oLookUP, oStatusLookup As New NexusProvider.LookupListCollection
            Dim oWebService = New NexusProvider.ProviderManager().Provider
            oLookUP = oWebService.GetList(NexusProvider.ListType.PMLookup, "Product", True, False)
            oStatusLookup = oWebService.GetList(NexusProvider.ListType.PMLookup, "Renewal_Status_Type", True, False)
            ddlProductType.Items.Clear()
            RenewalStatusType.Items.Clear()

            For iProductCount As Integer = 0 To oLookUP.Count - 1
                Dim lstProductCount As New ListItem
                lstProductCount.Text = oLookUP(iProductCount).Description
                lstProductCount.Value = Trim(oLookUP(iProductCount).Code)
                ddlProductType.Items.Add(lstProductCount)
                ddlProductType.DataBind()
            Next

            For iStatusCount As Integer = 0 To oStatusLookup.Count - 1
                Dim lstStatusCount As New ListItem
                lstStatusCount.Text = oStatusLookup(iStatusCount).Description
                lstStatusCount.Value = Trim(oStatusLookup(iStatusCount).Code)
                RenewalStatusType.Items.Add(lstStatusCount)
                RenewalStatusType.DataBind()
            Next
            If Session(CNAgentDetails) IsNot Nothing Then
                Dim oUserDetails As NexusProvider.UserDetails
                oUserDetails = Session(CNAgentDetails)
                BranchCode.DataSource = oUserDetails.ListOfBranches
                BranchCode.DataTextField = "Description"
                BranchCode.DataValueField = "Code"
                BranchCode.DataBind()
             
            End If
        End Sub
        Protected Sub clearPanel(ByVal oPolCol As NexusProvider.PolicyCollection)
            If oPolCol IsNot Nothing Then
                If oPolCol.Count > 0 Then
                    pnlButtons.Visible = True
                    GridViewRenewals.Visible = True
                    NoPolicesFound.Visible = False
                    updpnlRenewal.Visible = True
                Else
                    pnlButtons.Visible = False
                    GridViewRenewals.Visible = False
                    NoPolicesFound.Visible = True
                    updpnlRenewal.Visible = False
                End If
            Else
                pnlButtons.Visible = False
                GridViewRenewals.Visible = False
                NoPolicesFound.Visible = True
            End If
        End Sub
        Protected Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
            FillRenewalRecords()
        End Sub
        Protected Sub chkRenewalDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkRenewalDate.CheckedChanged
            If chkRenewalDate.Checked = True Then
                txtRenewalDate.ReadOnly = False
                RenewalDate_CalendarLookup.Enabled = True
            Else
                txtRenewalDate.Text = Date.Today.ToShortDateString
                txtRenewalDate.ReadOnly = True
                RenewalDate_CalendarLookup.Enabled = False
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
          
            'To set the Focus
            Page.SetFocus(RenewalStatusType)

            If HttpContext.Current.Session.IsCookieless Then
                btnStatus.OnClientClick = "tb_show(null , '../Modal/RenewalStatusType.aspx?PostbackTo=" & UpdRenewal.ClientID.ToString & "&modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                'Lapse Link
                btnLapse.OnClientClick = "tb_show(null , '../Modal/RenewalLapseReason.aspx?PostbackTo=" & UpdRenewal.ClientID.ToString & "&modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            Else
                btnStatus.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "Modal/RenewalStatusType.aspx?PostbackTo=" & UpdRenewal.ClientID.ToString & "&modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                'Lapse Link
                btnLapse.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "Modal/RenewalLapseReason.aspx?PostbackTo=" & UpdRenewal.ClientID.ToString & "&modal=true&Type=All&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            End If
            'Status Link
            

            If Not IsPostBack Then
                'Reset The values
                txtAgentCode.Text = String.Empty
                txtAgentKey.Value = String.Empty
                txtClient.Text = String.Empty
                txtClientKey.Value = String.Empty
                txtAgentCode.Attributes.Add("readonly", "readonly")
                txtClient.Attributes.Add("readonly", "readonly")
            End If

            'On Change of the status of the policy
            If Request("__EVENTARGUMENT") = "RefreshPolicy" Then
                FillRenewalRecords()
            End If

            If Request("__EVENTARGUMENT") = "RefreshPolicy_lps" Then
                Session(CNIsLapsed) = True
                Response.Redirect(ConfigurationManager.AppSettings("WebRoot") & "/secure/RenewalManager.aspx")
            End If
            If Not Session(CNIsLapsed) Is Nothing AndAlso CBool(Session(CNIsLapsed)) = True Then
                FillRenewalRecords()
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                Dim oPortalConfig As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
                'check the web config at portal to show email model page
                If oPortalConfig.PolicyLapseEmail = True Then
                    SendMail()
                End If
                Session(CNIsLapsed) = Nothing
                Session.Remove(CNIsLapsed)
            End If

        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
           
            If Not IsPostBack Then
                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                Session(CNParty) = Nothing
                txtRenewalDate.ReadOnly = True
                RenewalDate_CalendarLookup.Enabled = False
                txtRenewalDate.Text = Date.Today.ToShortDateString
                'This will populate the dropdowns from the NexusLookups
                PopulateDropDown()
                If oUserDetails IsNot Nothing Then
                    If oUserDetails.Key > 0 Then
                        AgentPanel.Visible = False
                    Else
                        AgentPanel.Visible = True
                    End If
                Else
                    AgentPanel.Visible = True
                End If
            End If
            If HttpContext.Current.Session.IsCookieless Then
                btnAgentCode.OnClientClick = "tb_show(null ,'../Modal/FindAgent.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                btnClient.OnClientClick = "tb_show(null ,'../secure/agent/FindClient.aspx?RequestPage=BG&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=800' , null);return false;"
            Else
                btnAgentCode.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/FindAgent.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                btnClient.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/secure/agent/FindClient.aspx?RequestPage=BG&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=800' , null);return false;"
            End If
           

        End Sub
        Sub FillRenewalRecords()
            Dim oPortalConfig As Config.Portal = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID())
            Dim oFilterPolCol As NexusProvider.PolicyCollection = New NexusProvider.PolicyCollection
            If UserCanDoTask("RenewalManager") Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                Dim oPolCol As New NexusProvider.PolicyCollection
                Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                Dim sBranchCode As String
                Dim iAgentKey, iPartyKey As Integer
                Dim dRenewalDate As Date

                'Set the Branch Code
                If BranchCode.SelectedValue IsNot Nothing Then
                    If BranchCode.SelectedValue.Trim.Length = 0 Then
                        sBranchCode = String.Empty
                    Else
                        sBranchCode = BranchCode.SelectedValue
                    End If
                Else
                    sBranchCode = BranchCode.SelectedValue
                End If

                'Set the Agent Key
                If txtAgentCode.Text.Trim.Length <> 0 Then
                    iAgentKey = txtAgentKey.Value
                End If

                'Set the Party Key
                If txtClient.Text.Trim.Length <> 0 Then
                    iPartyKey = txtClientKey.Value
                End If

                'Renewal Date
                If chkRenewalDate.Checked Then
                    dRenewalDate = CDate(txtRenewalDate.Text)
                End If

                If oUserDetails IsNot Nothing Then
                    If oUserDetails.Key > 0 Then
                        ' -	Note that if the logged in user is an agent, then the agent code drop down should not be displayed.
                        AgentPanel.Visible = False
                        'Get all Policies in Renewals for all the client against Agent key
                        oPolCol = oWebService.GetPoliciesInRenewal(iPartyKey, Nothing, ddlProductType.SelectedValue, dRenewalDate, True, True, sBranchCode)
                    Else
                        oPolCol = oWebService.GetPoliciesInRenewal(iPartyKey, iAgentKey, ddlProductType.SelectedValue, dRenewalDate, True, True, sBranchCode)
                    End If
                End If

                'Filter Renewals with Status
                For Each oPol As NexusProvider.Policy In oPolCol
                    If oPol.RenewalStatusTypeCode.Trim = RenewalStatusType.SelectedValue Then
                        oFilterPolCol.Add(oPol)
                    End If
                Next
                'Bind The Filtered Collection
                grdvRenQuotes.Visible = True
                grdvRenQuotes.DataSource = oFilterPolCol
                grdvRenQuotes.DataBind()
                Session(CNSearchResults) = oFilterPolCol
                clearPanel(oFilterPolCol)
            End If
        End Sub
        Protected Sub GrdChkSelected(ByVal sender As Object, ByVal e As System.EventArgs)
            lblNoPoliciesSelected.Visible = False
            Dim oPolCol As NexusProvider.PolicyCollection = Session(CNSearchResults)
            For jCount As Integer = 0 To grdvRenQuotes.Rows.Count - 1
                Dim ChkSelected As CheckBox = grdvRenQuotes.Rows(jCount).FindControl("chkSelection")
                If ChkSelected.Checked Then
                    For iCount As Integer = 0 To oPolCol.Count - 1
                        If CInt(grdvRenQuotes.Rows(jCount).Cells(0).Text.Trim) = oPolCol(iCount).InsuranceFileKey Then
                            oPolCol(iCount).IsSelected = True
                            Exit For
                        End If
                    Next
                End If
            Next

            'Deselct unchecked policies
            For jCount As Integer = 0 To grdvRenQuotes.Rows.Count - 1
                Dim ChkSelected As CheckBox = grdvRenQuotes.Rows(jCount).FindControl("chkSelection")
                If ChkSelected.Checked = False Then
                    For iCount As Integer = 0 To oPolCol.Count - 1
                        If oPolCol(iCount).IsSelected = True And CInt(grdvRenQuotes.Rows(jCount).Cells(0).Text.Trim) = oPolCol(iCount).InsuranceFileKey Then
                            oPolCol(iCount).IsSelected = False
                            Exit For
                        End If
                    Next
                End If
            Next

            'Check Whether any policy is selected or not
            Dim bSelected As Boolean = False
            For pCount As Integer = 0 To oPolCol.Count - 1
                If oPolCol(pCount).IsSelected = True Then
                    bSelected = True
                    Exit For
                End If
            Next
            If bSelected = True Then
                btnDelete.Attributes.Add("onclick", "javascript:return DeleteConfirmation();")
            Else
                btnDelete.Attributes.Add("onclick", "")
            End If
            Session(CNSearchResults) = oPolCol
        End Sub

        Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
            txtRenewalDate.Text = Date.Today.ToShortDateString
            chkRenewalDate.Checked = False
            If ddlProductType.Items.Count > 0 Then
                ddlProductType.SelectedIndex = 0
            End If
            If RenewalStatusType.Items.Count > 0 Then
                RenewalStatusType.SelectedIndex = 0
            End If
            If BranchCode.Items.Count > 0 Then
                BranchCode.SelectedIndex = 0
            End If
            txtAgentCode.Text = String.Empty
            txtAgentKey.Value = String.Empty
            txtClient.Text = String.Empty
            txtClientKey.Value = String.Empty
            pnlButtons.Visible = False
            GridViewRenewals.Visible = False
            NoPolicesFound.Visible = False
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
                sURL = "../Modal/SendEmail.aspx?PartyKey=" & oParty.Key & "&key=Issued&InsuranceFileKey=" & oQuote.InsuranceFileKey & "&modal=true&loc=docm&n=p&Riskcheck=true&KeepThis=true&TB_iframe=true&height=300&width=750"
            Else
                sURL = ConfigurationManager.AppSettings("WebRoot") & "Modal/SendEmail.aspx?PartyKey=" & oParty.Key & "&key=Issued&InsuranceFileKey=" & oQuote.InsuranceFileKey & "&modal=true&loc=docm&n=p&Riskcheck=true&KeepThis=true&TB_iframe=true&height=300&width=750"
            End If

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", _
            "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>")
            Exit Sub
        End Sub

    End Class
End Namespace

