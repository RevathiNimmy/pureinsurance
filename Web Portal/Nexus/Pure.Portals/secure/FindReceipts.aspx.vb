Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports System.Web.Configuration
Imports Nexus.Library
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Web.Services

Namespace Nexus

    Partial Class FindReceipts : Inherits Frontend.clsCMSPage

        Dim oCashListReceipt As New NexusProvider.CashListReceipt
        Dim oWebService As NexusProvider.ProviderBase
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
         
            'To set the Focus
            Page.SetFocus(ddlBranchCode)


            Dim breturncode As String
            oWebService = New NexusProvider.ProviderManager().Provider
            breturncode = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 87).OptionValue

            If breturncode = "1" Then
                txtCollectionDateFrom.Enabled = True
                txtCollectionDateTo.Enabled = True
                calCollectionDateFrom.Enabled = True
                calCollectionDateTo.Enabled = True
                calCollectionDateFrom.HLevel = "4"
                calCollectionDateTo.HLevel = "4"
            Else
                txtCollectionDateFrom.Enabled = False
                txtCollectionDateTo.Enabled = False
                calCollectionDateFrom.Enabled = False
                calCollectionDateTo.Enabled = False
                calCollectionDateFrom.HLevel = "2"
                calCollectionDateTo.HLevel = "2"
            End If

            If Not IsPostBack Then
                'Cleaning of the session values
                ClearQuote()
                ClearClaims()
                ClearHeader()
                If CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches.Count > 1 Then
                    liBranch.Visible = True
                    Dim oBranchs As NexusProvider.BranchCollection = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
                    'Sort branch collection before binding
                    oBranchs.SortColumn = "Description"
                    oBranchs.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
                    oBranchs.Sort()
                    ddlBranchCode.DataSource = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
                    ddlBranchCode.DataTextField = "Description"
                    ddlBranchCode.DataValueField = "Code"
                    ddlBranchCode.DataBind()
                    ddlBranchCode.Items.Insert(0, New ListItem(GetLocalResourceObject("lbl_DefaultText"), "-1"))
                End If
                Dim oWebservice As NexusProvider.ProviderBase
                oWebservice = New NexusProvider.ProviderManager().Provider
                Dim oBankAccounts As NexusProvider.AccountDetailsCollection
                oBankAccounts = oWebservice.GetBankAccounts()
                'sort Bank Accounts before binding
                oBankAccounts.SortColumn = "BankAccountName"
                oBankAccounts.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
                oBankAccounts.Sort()
                ddlBankAccount.DataSource = oBankAccounts
                ddlBankAccount.DataTextField = "BankAccountName"
                ddlBankAccount.DataValueField = "Code"
                ddlBankAccount.DataBind()
                ddlBankAccount.Items.Insert(0, New ListItem(GetLocalResourceObject("lbl_DefaultText"), "-1"))
            End If

            If Request("__EVENTARGUMENT") = "RefreshMediaTypeStatus" Then
                GetReceiptsDetails()
            End If
        End Sub

        Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
            If Page.IsValid Then
                GetReceiptsDetails()
            End If
        End Sub

        Public Sub GetReceiptsDetails()
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim sBranchCode As String = IIf((ddlBranchCode.SelectedValue <> "-1"), ddlBranchCode.SelectedValue, String.Empty)
            Dim oPartyCollection As NexusProvider.PartyCollection
            Dim oPartySearchCriteria As New NexusProvider.PartySearchCriteria
            Dim bClientAvailable As Boolean = True
            Dim oCashListReceipts As New NexusProvider.CashListReceipts

            If (Not String.IsNullOrEmpty(Trim(txtClient.Text)) And Not txtClient.Text.Contains("%")) Then
                oPartySearchCriteria.ShortName = Trim(txtClient.Text)
                oPartySearchCriteria.PartyType = NexusProvider.PartyTypeType.GC
                'oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyType.Personal)
                'added following criteria in oPartySearchCriteria when oPartySearchCriteria.PartyType = NexusProvider.PartyTypeType.GC
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.PC)
                oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.CC)
                oPartyCollection = oWebService.FindParty(oPartySearchCriteria)
                If (oPartyCollection IsNot Nothing AndAlso oPartyCollection.Count > 0) Then
                    hdClientID.Value = oPartyCollection(0).Key
                Else
                    bClientAvailable = False
                    hdClientID.Value = String.Empty
                    btnUpdateAll.Visible = False

                End If
            ElseIf txtClient.Text.Contains("%") Then
                hdClientID.Value = String.Empty
            End If
            If bClientAvailable Then
                With oCashListReceipt
                    .BankAccountCode = Trim(IIf((ddlBankAccount.SelectedValue <> "-1"), ddlBankAccount.SelectedValue, String.Empty))
                    If (Trim(txtClient.Text) <> String.Empty) Then
                        .PartyKey = CInt(IIf(String.IsNullOrEmpty(hdClientID.Value), 0, hdClientID.Value))
                        .PartyKeySpecified = IIf(String.IsNullOrEmpty(hdClientID.Value), False, True)
                    Else
                        .PartyKeySpecified = False
                    End If
                    .InsuranceRef = txtPolicy.Text
                    If (Not String.IsNullOrEmpty(txtCollectionDateFrom.Text.Trim())) Then
                        .CollectionDateFrom = FormatDateTime(txtCollectionDateFrom.Text, DateFormat.ShortDate)
                        .CollectionDateFromSpecified = IIf(String.IsNullOrEmpty(txtCollectionDateFrom.Text.Trim()), False, True)
                    End If
                    If (Not String.IsNullOrEmpty(txtCollectionDateTo.Text.Trim())) Then
                        .CollectionDateTo = FormatDateTime(txtCollectionDateTo.Text, DateFormat.ShortDate)
                        .CollectionDateToSpecified = IIf(String.IsNullOrEmpty(txtCollectionDateTo.Text.Trim()), False, True)
                    End If
                    .MediaReference = Trim(txtMediaReference.Text)
                    .MediaTypeStatusCode = Trim(GISMediaTypeStatus.Value)
                    .DrawnBankCode = Trim(GISDrawnBankName.Value)
                    .DocumentRef = Trim(txtDocumentRef.Text)

                    'to limit the search return from SAM
                    oCashListReceipt.MaxRowsToFetch = oPortal.MaxSearchResults
                End With

                oCashListReceipts = oWebService.FindCashListReceipts(oCashListReceipt, sBranchCode)
                Session.Add(CNCashListReceipt, oCashListReceipts)

                If oCashListReceipts IsNot Nothing AndAlso oCashListReceipts.Count > 0 Then

                    btnUpdateAll.Enabled = False
                    If HttpContext.Current.Session.IsCookieless Then
                        btnUpdateAll.Attributes.Add("onclick", "tb_show(null ," & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/UpdateMediaStatus.aspx?PostbackTo=" & UpdFindReceipt.ClientID & "&modal=true&MediaType=" + GISMediaTypeStatus.Value + "&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=400&width=700' , null);return false;")
                    Else
                        btnUpdateAll.Attributes.Add("onclick", "tb_show(null , '../Modal/UpdateMediaStatus.aspx?PostbackTo=" & UpdFindReceipt.ClientID & "&modal=true&MediaType=" + GISMediaTypeStatus.Value + "&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=400&width=700' , null);return false;")
                    End If

                    'validate size of dataset. if 500(configured at portal level) or more results are returned then add a validation message to the screen
                    If oCashListReceipts.Count >= oPortal.MaxSearchResults Then
                        'create a custom validator
                        Dim cstMaxResults As New CustomValidator
                        cstMaxResults.IsValid = False
                        'look for a validation message in the page resources, but if there is not one defined add a default message
                        cstMaxResults.ErrorMessage = IIf(GetLocalResourceObject("cstMaxResults") Is Nothing, "Maximum number of search results exceeded, please refine your search criteria", GetLocalResourceObject("cstMaxResults"))
                        cstMaxResults.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                        'add the validator to the page, this will have the effect of making the page invalid
                        Page.Validators.Add(cstMaxResults)
                    End If

                Else
                    btnUpdateAll.Enabled = False
                End If
            End If

            gvGetReceiptsdetails.Visible = True
            gvGetReceiptsdetails.AllowPaging = True
            gvGetReceiptsdetails.AllowSorting = True
            gvGetReceiptsdetails.DataSource = oCashListReceipts
            gvGetReceiptsdetails.DataBind()
        End Sub
        Protected Sub gvGetReceiptsdetails_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvGetReceiptsdetails.DataBound
            If gvGetReceiptsdetails.Rows.Count = 0 Or gvGetReceiptsdetails.PageCount = 1 Then
                gvGetReceiptsdetails.AllowPaging = False
            End If

            If gvGetReceiptsdetails.HeaderRow IsNot Nothing Then
                Dim cbHeader As CheckBox = CType(gvGetReceiptsdetails.HeaderRow.FindControl("chkSelectAll"), CheckBox)
                Dim IsPayment As Boolean = False
                If cbHeader IsNot Nothing Then
                    For Each gvr As GridViewRow In gvGetReceiptsdetails.Rows
                        If DirectCast(gvr.FindControl("IsPaymentInitiated"), Label).Text = "True" Then
                            IsPayment = True
                            Exit For
                        End If
                    Next

                    cbHeader.Attributes.Add("onclick", "ChangeAllCheckBoxStates(this.checked,'" & GetLocalResourceObject("msg_AllPaymentInitiated").ToString() & "','" & cbHeader.ClientID & "','" & IsPayment & "');")
                End If
            End If
        End Sub

        Protected Sub gvGetReceiptsdetails_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvGetReceiptsdetails.PageIndexChanging
            'rebind data on page change
            gvGetReceiptsdetails.PageIndex = e.NewPageIndex
            gvGetReceiptsdetails.DataSource = DirectCast(Session(CNCashListReceipt), NexusProvider.CashListReceipts)
            gvGetReceiptsdetails.DataBind()

            'check if all of the checkboxes are checked and if so then check the header checkbox
            Dim bAllChecked As Boolean = False
            For jCount As Integer = 0 To gvGetReceiptsdetails.Rows.Count - 1
                Dim ChkSelected As CheckBox = gvGetReceiptsdetails.Rows(jCount).FindControl("chkSelect")
                If ChkSelected.Checked Then
                    'if any are NOT checked then set flag to false
                    bAllChecked = True
                Else
                    bAllChecked = False
                End If
            Next

            Dim ChkSelectAll As CheckBox = gvGetReceiptsdetails.HeaderRow.FindControl("chkSelectAll")
            ChkSelectAll.Checked = bAllChecked

        End Sub

        Protected Sub btnNewsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewsearch.Click
            Me.btnNewsearch.CausesValidation = False
            txtClient.Text = String.Empty
            txtPolicy.Text = String.Empty
            txtCollectionDateFrom.Text = String.Empty
            txtCollectionDateTo.Text = String.Empty

            If ddlBranchCode IsNot Nothing AndAlso ddlBranchCode.Items.Count > 0 Then
                ddlBranchCode.SelectedIndex = 0
            End If

            If ddlBankAccount IsNot Nothing AndAlso ddlBankAccount.Items.Count > 0 Then
                ddlBankAccount.SelectedIndex = 0
            End If

            txtDocumentRef.Text = String.Empty
            txtMediaReference.Text = String.Empty
            GISMediaTypeStatus.Value = GetLocalResourceObject("lbl_DefaultText")
            GISDrawnBankName.Value = GetLocalResourceObject("lbl_DefaultText")
            gvGetReceiptsdetails.DataSource = Nothing
            gvGetReceiptsdetails.DataBind()
            gvGetReceiptsdetails.Visible = False
            btnUpdateAll.Enabled = False
            hdClientID.Value = String.Empty
        End Sub

        Protected Function CheckMediaTypeStatus(ByVal oCashListReceipt As NexusProvider.CashListReceipt) As NexusProvider.MediaTypeStatus
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim oMediaTypeStatus As New NexusProvider.MediaTypeStatus
            With oMediaTypeStatus
                .InsuranceFileKey = oCashListReceipt.InsuranceFileKey
                .LossDateSpecified = False
            End With
            Try
                oWebService.GetPolicyStatusForMediaTypeStatus(oMediaTypeStatus)
            Finally
                oWebService = Nothing
            End Try
            Return oMediaTypeStatus
        End Function
        Protected Sub gvGetReceiptsdetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvGetReceiptsdetails.RowDataBound

            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(0).Visible = False
                e.Row.Cells(1).Visible = False

                Dim chkSelect As CheckBox = CType(e.Row.FindControl("chkSelect"), CheckBox)
                Dim iInsuranceFileKey As Integer = DirectCast(e.Row.DataItem, NexusProvider.CashListReceipt).InsuranceFileKey

                'if iInsuranceFileKey =0 then no need to check the IsPolicyCanceled or IsClaimPaymentInitiated
                If iInsuranceFileKey <> 0 Then

                    Dim oMediaTypeStatus As NexusProvider.MediaTypeStatus = CheckMediaTypeStatus(DirectCast(e.Row.DataItem, NexusProvider.CashListReceipt))

                    If oMediaTypeStatus.IsPolicyCanceled Then
                        chkSelect.Enabled = False
                    End If

                    DirectCast(e.Row.FindControl("IsPaymentInitiated"), Label).Text = oMediaTypeStatus.IsClaimPaymentInitiated

                    If oMediaTypeStatus.IsClaimPaymentInitiated Then
                        chkSelect.Attributes.Add("onClick", "MediaTypeConfirmation('" + GetLocalResourceObject("msg_PaymentInitiated").ToString() & "','" & chkSelect.ClientID & "','" & CType(e.Row.DataItem, NexusProvider.CashListReceipt).CashListReceiptRowID & "')")
                    Else
                        chkSelect.Attributes.Add("onclick", "CallMe('" & chkSelect.ClientID & "','" & CType(e.Row.DataItem, NexusProvider.CashListReceipt).CashListReceiptRowID & "')")
                    End If
                Else
                    chkSelect.Attributes.Add("onclick", "CallMe('" & chkSelect.ClientID & "','" & CType(e.Row.DataItem, NexusProvider.CashListReceipt).CashListReceiptRowID & "')")
                End If

                'If checkbox is not enabled then it should be in list
                If chkSelect.Enabled = True Then
                    'ClientScript.RegisterArrayDeclaration("CheckBoxIDs", String.Concat("'", chkSelect.ClientID, "'"))
                    'ClientScript.RegisterArrayDeclaration("CRRowIDs", String.Concat("'", CType(e.Row.DataItem, NexusProvider.CashListReceipt).CashListReceiptRowID, "'"))
                    ScriptManager.RegisterArrayDeclaration(Page, "CheckBoxIDs", String.Concat("'", chkSelect.ClientID, "'"))
                    ScriptManager.RegisterArrayDeclaration(Page, "CRRowIDs", String.Concat("'", CType(e.Row.DataItem, NexusProvider.CashListReceipt).CashListReceiptRowID, "'"))
                End If

                chkSelect.Checked = DirectCast(e.Row.DataItem, NexusProvider.CashListReceipt).IsSelected

            ElseIf e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(0).Visible = False
                e.Row.Cells(1).Visible = False
            End If

        End Sub
        <WebMethod()> _
        Public Shared Function SelectRecord(ByVal ChkStatus As String, ByVal v_iCashListReceiptRowID As String) As String
            Dim oReceipts As NexusProvider.CashListReceipts = DirectCast(HttpContext.Current.Session(CNCashListReceipt), NexusProvider.CashListReceipts)

            For jCount As Integer = 0 To oReceipts.Count - 1
                If CBool(ChkStatus) = True Then
                    If CInt(v_iCashListReceiptRowID) = oReceipts(jCount).CashListReceiptRowID Then
                        oReceipts(jCount).IsSelected = True
                        Exit For
                    End If
                End If
            Next

            'Deselect unchecked policies
            For jCount As Integer = 0 To oReceipts.Count - 1
                If CBool(ChkStatus) = False Then
                    If CInt(v_iCashListReceiptRowID) = oReceipts(jCount).CashListReceiptRowID Then
                        oReceipts(jCount).IsSelected = False
                        Exit For
                    End If
                End If
            Next

            HttpContext.Current.Session(CNCashListReceipt) = oReceipts

            Return "Success"
        End Function

        Protected Sub gvGetReceiptsdetails_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvGetReceiptsdetails.Sorting

            'sort the Quote & Policy according to the column clicked
            'we need to store the current sort order in viewstate, and reverse it each time
            Dim oCashListReceipts As NexusProvider.CashListReceipts = Session(CNCashListReceipt)
            oCashListReceipts.SortColumn = e.SortExpression
            'check that the sort expression is the same as stored in viewstate as we should start again if reordering by a new column
            Dim _sortDirection As New SortDirection
            If ViewState("SortDirection") = SortDirection.Ascending And ViewState("SortExpression") = e.SortExpression Then
                _sortDirection = SortDirection.Descending
            Else
                _sortDirection = SortDirection.Ascending
            End If
            'store the current sortdirection for comparison on the next sort
            ViewState("SortDirection") = _sortDirection
            'store the SortExpression in viewstate so that we can check if we are sorting by a new column on the next sort
            ViewState("SortExpression") = e.SortExpression
            oCashListReceipts.SortingOrder = _sortDirection
            oCashListReceipts.Sort()
            CType(sender, GridView).DataSource = oCashListReceipts
            CType(sender, GridView).DataBind()
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If HttpContext.Current.Session.IsCookieless Then
                'This will populate search client modal 
                btnClient.OnClientClick = "tb_show(null ,'../secure/agent/FindClient.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700' , null);return false;"
                'This will populate search policy modal 
                btnPolicy.OnClientClick = "tb_show(null ,'../Modal/FindInsuranceFile.aspx?Page=CR&modal=true&KeepThis=true&FromPage=FR&FindClaim=1&TB_iframe=true&height=500&width=700' , null);return false;"
            Else
                'This will populate search client modal 
                btnClient.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/secure/agent/FindClient.aspx?modal=true&KeepThis=true&ClaimFlag=1&ClientType=Claim&TB_iframe=true&height=500&width=700' , null);return false;"
                'This will populate search policy modal 
                btnPolicy.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/FindInsuranceFile.aspx?Page=CR&modal=true&KeepThis=true&FromPage=FR&FindClaim=1&TB_iframe=true&height=500&width=700' , null);return false;"
            End If
        End Sub
    End Class
End Namespace

