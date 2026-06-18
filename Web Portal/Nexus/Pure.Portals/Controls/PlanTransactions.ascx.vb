Imports System.Web.Configuration.WebConfigurationManager
Imports System.Web.Configuration
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Linq
Imports Nexus
Imports Nexus.Library
Imports Nexus.Utils
Imports System.Drawing
Imports System.Data
Imports CMS.Library


Partial Class Controls_PlanTransactions
    Inherits System.Web.UI.UserControl
    Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oFinancePlanTransactionsCollection As New NexusProvider.FinancePlanTransactionsCollection

        If Not Page.IsPostBack Then
            Session(CNInsuranceFileKey) = Nothing
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHideButton", "ShowHideButton('hidden');", True)

            If Request.QueryString("modal") IsNot Nothing AndAlso Request.QueryString("modal") = True AndAlso Request.QueryString("Type") Is Nothing AndAlso Request.QueryString("Type") <> "MTA" Then
                pnlClientAccounts.Visible = False
                grdTransactions.Columns(0).Visible = False
                If Session(CNFinancePlanDetails) IsNot Nothing Then
                    oFinancePlanTransactionsCollection = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).Transactions
                    If oFinancePlanTransactionsCollection IsNot Nothing Then
                        grdTransactions.DataSource = oFinancePlanTransactionsCollection
                        grdTransactions.DataBind()
                    End If
                End If
            Else 'will execute in case of adding new pf plan/maintaining existing plan
                If Session(CNInstalmentPlanMode) = InstalmentPlanType.edit Then
                    SetClientCodeFromSession()
                End If

                'the logic to fill grid from gettransactionsdetails method using clientcode as request
                If txtClientCode.Text <> String.Empty Then
                    FillGrid(Nothing, txtClientCode.Text)
                    btnOk.Visible = True
                End If

            End If
            chkHiddenField.Value = ""
            txtDateTo.Text = Date.Today
            If Request.QueryString("modal") IsNot Nothing AndAlso Request.QueryString("modal") = True AndAlso Request.QueryString("Type") IsNot Nothing AndAlso Request.QueryString("Type") = "MTA" Then
                hvInsuranceRef.Value = txtPolicyNo.Text
                btnClientCode.Enabled = False
            End If
        End If

    End Sub

    Protected Sub SetClientCodeFromSession()
        Dim oParty As NexusProvider.BaseParty = CType(Session(CNParty), NexusProvider.BaseParty)
        Dim oQuote As NexusProvider.Quote
        'Create oQuote object from session
        oQuote = CType(Session(CNQuote), NexusProvider.Quote)

        If Not (Session(CNAgentType) IsNot Nothing AndAlso Session(CNAgentType).Trim.ToUpper = "BROKER" AndAlso oQuote.BusinessTypeCode <> "DIRECT") Then
            Select Case True
                Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                    oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                    oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
            End Select
        End If
        If oParty IsNot Nothing Then
            Select Case True
                Case TypeOf oParty Is NexusProvider.CorporateParty
                    With CType(oParty, NexusProvider.CorporateParty)
                        If Session(CNLoginType) = LoginType.Customer Then
                        Else
                            If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                txtClientCode.Text = .ClientSharedData.ShortName.Trim()
                            ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                txtClientCode.Text = .UserName.Trim()
                            End If
                        End If
                    End With
                Case TypeOf oParty Is NexusProvider.PersonalParty
                    With CType(oParty, NexusProvider.PersonalParty)
                        If .ClientSharedData IsNot Nothing AndAlso String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                            txtClientCode.Text = CType(Session(CNParty), NexusProvider.PersonalParty).ClientSharedData.ShortName.Trim()
                        ElseIf String.IsNullOrEmpty(.UserName) = False Then
                            txtClientCode.Text = .UserName.Trim()
                        End If
                    End With
            End Select

            txtAgentKey.Value = CType(oParty.Key, String)
            If Session(CNInstalmentPlanMode) = InstalmentPlanType.edit Then 'Fill Policy ref automatically in textbox to make the query
                If oQuote IsNot Nothing Then
                    txtPolicyNo.Text = oQuote.InsuranceFileRef.Trim()

                End If
            End If
        End If

    End Sub

    Private Sub FillGrid(Optional ByVal oFinancePlanTransactionsCollection As NexusProvider.FinancePlanTransactionsCollection = Nothing, Optional ByVal sShortCode As String = Nothing)
        'Dim oFinancePlanTransactions As NexusProvider.FinancePlanTransactions
        'Dim oAllocationDetails As NexusProvider.AllocationDetails
        Dim oTransactionsCollection As New NexusProvider.FinancePlanTransactionsCollection
        Dim oAllocationDetailsCollections As New NexusProvider.AllocationDetailsCollections
        Dim oInstalment As New Nexus.BaseInstalment
        Dim dtToDate As Date = Nothing
        Dim sPolicyRef As String = Nothing
        Dim sClaimNumber As String = Nothing

        txtTotalSelected.Text = "0.00"

        If txtDateTo.Text <> "" Then
            dtToDate = CType(txtDateTo.Text, Date)
        End If
        If txtPolicyNo.Text <> "" Then
            sPolicyRef = txtPolicyNo.Text
        End If
        If txtClaimNumber.Text <> "" Then
            sClaimNumber = txtClaimNumber.Text.Trim()
        End If

        ' CLR flow: if Claim Number is provided, search for claim recovery transactions
        If Not String.IsNullOrEmpty(sClaimNumber) Then
            hvSourceType.Value = "CLR"
            oInstalment.GetClaimRecoveryTransactions(sShortCode:=txtClientCode.Text, sClaimNumber:=sClaimNumber)
        Else
            hvSourceType.Value = "PF"
            oInstalment.GetTransactionDetails(nAccountKey:=Nothing, oAllocationDetailsCollections:=Nothing, sShortCode:=txtClientCode.Text, dtToDate:=dtToDate,
                                              sInsuranceRef:=sPolicyRef, bIsNewPF:=True, sBranchCode:=Nothing)
        End If

        oTransactionsCollection = Nothing
        If Session(CNTransactionDetails) IsNot Nothing Then
            Session("CNTransactionDetailsTemp") = Session(CNTransactionDetails)
            oTransactionsCollection = CType(Session(CNTransactionDetails), NexusProvider.FinancePlanTransactionsCollection)
            grdTransactions.DataSource = Nothing
            If oTransactionsCollection IsNot Nothing AndAlso oTransactionsCollection.Count > 0 Then
                grdTransactions.DataSource = oTransactionsCollection
            End If
            grdTransactions.DataBind()
        End If
        'End If

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        'btnOk.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/PremiumFinanceQuote.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700' , null);return false;"
        If HttpContext.Current.Session.IsCookieless Then
            btnClientCode.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/secure/agent/FindClient.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&IncludeAgent=true' , null);return false;"
        Else
            btnClientCode.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/secure/agent/FindClient.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&IncludeAgent=true' , null);return false;"
        End If

    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If Page.IsValid Then

            Dim objBaseInstalment As New Nexus.BaseInstalment()
            'Modal will be opened during Plan Maintenance only and not during New Plan creation , so populate new insurancefile key during Add New Plan only.
            If hvInsuranceFileKey.Value IsNot Nothing AndAlso Request.QueryString("modal") Is Nothing Then
                Session(CNInsuranceFileKey) = hvInsuranceFileKey.Value
            End If

            ' Store CLR context in session for downstream use
            If hvSourceType.Value = "CLR" Then
                Session("CNSourceType") = "CLR"
                Session("CNClaimNumber") = txtClaimNumber.Text.Trim()
            Else
                Session("CNSourceType") = "PF"
                Session("CNClaimNumber") = Nothing
            End If

            FillPartySession()
            objBaseInstalment.FillQuoteSession()

            Session(CNAmountToPay) = txtTotalSelected.Text

            Dim sUrl As String
            If Request.QueryString("Type") IsNot Nothing AndAlso Request.QueryString("Type") = "MTA" Then
                Dim nFinancePlanKey As Integer
                Dim nFinancePlanVersion As Integer
                If Request.QueryString("FinancePlanKey") IsNot Nothing AndAlso Request.QueryString("FinancePlanKey") <> "" AndAlso Request.QueryString("FinancePlanVersion") IsNot Nothing AndAlso Request.QueryString("FinancePlanVersion") <> "" Then
                    nFinancePlanKey = Request.QueryString("FinancePlanKey")
                    nFinancePlanVersion = Request.QueryString("FinancePlanVersion")
                End If
                Session(CNInstalmentPlanMode) = InstalmentPlanType.edit
                If HttpContext.Current.Session.IsCookieless Then

                    sUrl = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/PremiumFinanceQuote.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&Type=MTA&FinancePlanKey=" & nFinancePlanKey & "&FinancePlanVersion=" & nFinancePlanVersion
                Else
                    sUrl = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "Modal/PremiumFinanceQuote.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&Type=MTA&FinancePlanKey=" & nFinancePlanKey & "&FinancePlanVersion=" & nFinancePlanVersion
                End If


            Else
                Session(CNInstalmentPlanMode) = InstalmentPlanType.Add
                If HttpContext.Current.Session.IsCookieless Then
                    If hvSelectedPlanTypes.Value = "SEDSRD" Then
                        If HttpContext.Current.Session.IsCookieless Then
                            sUrl = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/PremiumFinanceQuote.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&Type=NewPlanSED"
                        Else
                            sUrl = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "/Modal/PremiumFinanceQuote.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&Type=NewPlanSED"
                        End If


                    Else

                        If HttpContext.Current.Session.IsCookieless Then
                            sUrl = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/PremiumFinanceQuote.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&Type=NewPlan"
                        Else
                            sUrl = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "/Modal/PremiumFinanceQuote.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&Type=NewPlan"
                        End If


                    End If
                Else
                    If hvSelectedPlanTypes.Value = "SEDSRD" Then

                        If HttpContext.Current.Session.IsCookieless Then

                            sUrl = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/PremiumFinanceQuote.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&Type=NewPlanSED"
                        Else
                            sUrl = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "/Modal/PremiumFinanceQuote.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&Type=NewPlanSED"
                        End If

                    Else
                        If HttpContext.Current.Session.IsCookieless Then

                            sUrl = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/PremiumFinanceQuote.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&Type=NewPlan"
                        Else
                            sUrl = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "/Modal/PremiumFinanceQuote.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700&Type=NewPlan"
                        End If



                    End If
                End If


            End If


            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show",
            "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sUrl & "' , null);});</script>")

        End If
    End Sub
    Protected Sub CstmNegativeAmtCheck_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CstmNegativeAmtCheck.ServerValidate
        If Convert.ToDouble(txtTotalSelected.Text) >= 0 Then
            args.IsValid = True
        Else
            CstmNegativeAmtCheck.ErrorMessage = GetLocalResourceObject("msg_NegativeAmount")
            args.IsValid = False
        End If
    End Sub
    Public bIsAlreadyChecked As Boolean = False
    Public iCount As Integer = 0
    Protected Sub FillPartySession()
        Dim oNewParty As NexusProvider.BaseParty
        Dim oWebservice As NexusProvider.ProviderBase
        oWebservice = New NexusProvider.ProviderManager().Provider
        If hvAgentKey.Value <> "" Then
            oNewParty = oWebservice.GetParty(hvAgentKey.Value)
            Session(CNParty) = oNewParty
        End If

    End Sub

    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If chkHiddenField.Value = "" Then
            chkHiddenField.Value = 0
        End If
        Dim oRow As GridViewRow = DirectCast(DirectCast(sender, Control).Parent.Parent, GridViewRow)
        Dim sRequestedkey As String = grdTransactions.DataKeys(oRow.RowIndex).Value.ToString()
        hvInsuranceFileKey.Value = grdTransactions.DataKeys(oRow.RowIndex).Values("InsuranceFileKey").ToString()
        Dim oTransaction As NexusProvider.FinancePlanTransactions
        Dim oTransactionsCollection As New NexusProvider.FinancePlanTransactionsCollection
        Dim oSelectedTransactionsCollection As New NexusProvider.FinancePlanTransactionsCollection
        Dim Amount As Decimal = 0
        Dim bControlUncheck As Boolean = False
        Dim chkCurrentCheckBox As CheckBox = DirectCast(sender, CheckBox)
        bControlUncheck = chkCurrentCheckBox.Checked

        For Each oGridRow As GridViewRow In grdTransactions.Rows

            Dim chkSelect As CheckBox = DirectCast(oGridRow.FindControl("chkSelect"), CheckBox)
            If chkSelect.Checked Then
                bIsAlreadyChecked = True
                iCount += 1
            End If

        Next


        'Uncheck all the previously selected checkbox incase of a new checkbox selection request
        For Each oGridRow As GridViewRow In grdTransactions.Rows

            Dim chkSelect As CheckBox = DirectCast(oGridRow.FindControl("chkSelect"), CheckBox)
            If chkSelect IsNot Nothing Then
                chkSelect.Checked = False
            End If

        Next


        If bIsAlreadyChecked And iCount > 0 AndAlso CInt(chkHiddenField.Value) > iCount Then
            chkHiddenField.Value = 0
            txtTotalSelected.Text = "0.00"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHideButton", "ShowHideButton('hidden');", True)
            Exit Sub

        End If
        'Check similar records in case of a checkbox selection request
        iCount = 0
        For Each oGridRow As GridViewRow In grdTransactions.Rows
            'read the record type selected and compare with other rows in the grid

            If sRequestedkey = oGridRow.Cells(3).Text Then
                Dim chkSelect As CheckBox = DirectCast(oGridRow.FindControl("chkSelect"), CheckBox)
                If chkSelect IsNot Nothing AndAlso bControlUncheck Then
                    If oGridRow.Cells(10).Text.ToUpper() <> "COMM" Then
                        chkSelect.Checked = True
                        iCount += 1
                    End If
                End If
            End If
        Next

        'Get the selected transactions from the grid and throw them into Session(CNTransactionDetails) to execute AddNew process PFPlan
        oTransactionsCollection = CType(Session("CNTransactionDetailsTemp"), NexusProvider.FinancePlanTransactionsCollection)
        Dim oTempRecordCollection = From transaction In oTransactionsCollection Where (transaction.DocRef = sRequestedkey And transaction.Spare <> "COMM") Select transaction
        If oTempRecordCollection IsNot Nothing AndAlso oTempRecordCollection.Count > 0 Then
            For Each oTransactionRecord In oTempRecordCollection
                oTransaction = oTransactionRecord
                oSelectedTransactionsCollection.Add(oTransactionRecord)
                Amount = Amount + DirectCast(oTransactionRecord, NexusProvider.FinancePlanTransactions).OutStandingamount
                If oTransaction.DocumentTypeId = 17 OrElse oTransaction.DocumentTypeId = 15 Then 'For SED and SRD type
                    hvSelectedPlanTypes.Value = "SEDSRD"
                ElseIf oTransaction.DocumentTypeId = 9 OrElse oTransaction.DocumentTypeId = 10 Then 'For ICD and ICC type (Claim Recovery)
                    hvSelectedPlanTypes.Value = "CLR"
                    hvSourceType.Value = "CLR"
                Else
                    hvSelectedPlanTypes.Value = ""
                End If
            Next
        End If
        If bControlUncheck Then
            txtTotalSelected.Text = CType(Amount, String)
        Else
            txtTotalSelected.Text = "0.00"
        End If

        'Assigned to Session(CNTransactionDetails) : execute AddNew process PFPlan
        Session(CNTransactionDetails) = oSelectedTransactionsCollection
        chkHiddenField.Value = iCount
        If iCount > 0 Then
            If UserCanDoTask("AddInstalmentPlan") Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHideButton", "ShowHideButton('visible');", True)
            End If
        Else
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHideButton", "ShowHideButton('hidden');", True)
        End If
    End Sub


    Protected Sub grdTransactions_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdTransactions.PageIndexChanging
        CType(sender, GridView).PageIndex = e.NewPageIndex
        If Session(CNTransactionDetails) IsNot Nothing Then
            CType(sender, GridView).DataSource = CType(Session(CNTransactionDetails), NexusProvider.FinancePlanTransactionsCollection)
            CType(sender, GridView).DataBind()
        End If

    End Sub

    Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
        If hvInsuranceRef.Value <> "" Then
            txtPolicyNo.Text = hvInsuranceRef.Value
        End If
        FillGrid()
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHideButton", "ShowHideButton('Hidden');", True)
    End Sub


    Protected Sub grdTransactions_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdTransactions.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim TempGridRow As GridViewRow = e.Row
            Dim oItem As NexusProvider.FinancePlanTransactions = CType(e.Row.DataItem, NexusProvider.FinancePlanTransactions)
            TempGridRow.Cells(7).Text = New Money(oItem.Amount, oItem.TransactionCurrencyCode.ToString().Trim()).Formatted
            TempGridRow.Cells(8).Text = New Money(oItem.OutStandingamount, oItem.TransactionCurrencyCode.ToString().Trim()).Formatted
            e.Row.Cells(10).Visible = False
        End If
        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(10).Visible = False
        End If

    End Sub

    Protected Sub grdTransactions_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdTransactions.Sorting

    End Sub

    Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
        If grdTransactions.Rows.Count > 0 Then
            grdTransactions.DataSource = Nothing
            grdTransactions.DataBind()
        End If
        txtPolicyNo.Text = String.Empty
        txtClaimNumber.Text = String.Empty
        txtDateTo.Text = Date.Today
        txtClientCode.Text = String.Empty
        txtTotalSelected.Text = "0.00"
        hvSourceType.Value = String.Empty
        If dd1ShowTransactions.Items.Count > 0 Then
            dd1ShowTransactions.SelectedIndex = 0
        End If
    End Sub
End Class
