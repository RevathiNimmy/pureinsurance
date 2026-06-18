Imports System.Collections.ObjectModel
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports CMS.Library
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports Nexus.Utils

Namespace Nexus
    Partial Class AuthorizePayments : Inherits Frontend.clsCMSPage
        Private oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim bDisplayCaseOption As Boolean = False
        Dim oWebservice As NexusProvider.ProviderBase
        Dim nClaimID As Integer = 0
        Dim sClaimNumber As String = String.Empty
        Dim nCounter As Integer = 0
        Dim sSourceType As String = String.Empty
        Const m_nIsReferredGridColumn As Integer = 12
        Const m_nRecommendedByGridColumn As Integer = 13
        Const m_nLinkButtonsGridColumn As Integer = 15
        Dim oList As New NexusProvider.LookupListCollection
        Public Const kIsValidationEnabled As String = "IS_VALIDATION_ENABLED"
        Public Const kMediaTypeID As String = "MEDIATYPE_ID"
        Public Const sInterfaceName As String = "AuthorizePayment"
        Dim sColumnList As String = String.Empty

        ''' <summary>
        ''' Page Load
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                Dim oSearchcritaria As Collection = CType(Session(CNSearchPaymentAuthorization), Collection)
                Dim AuthoriseClaimPaymentspageCacheID As Guid
                AuthoriseClaimPaymentspageCacheID = Guid.NewGuid()
                rvtxtDateFrom.MaximumValue = Now.Date
                rvtxtDateTo.MaximumValue = DateTime.Now.Date.AddYears(100).ToString("dd/MM/yyyy")
                ViewState.Add("AuthoriseClaimPaymentspageCacheID", AuthoriseClaimPaymentspageCacheID.ToString)

                oWebservice = New NexusProvider.ProviderManager().Provider
                Dim oUserAuthority As New NexusProvider.UserAuthority
                oUserAuthority.UserCode = Session(CNLoginName)
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.HasPaymentsAuthority
                oWebservice.GetUserAuthorityValue(oUserAuthority)
                sIsAuthoriser.Value = oUserAuthority.UserAuthorityValue
                hiddenLimitAmount.Value = Convert.ToDecimal(oUserAuthority.UserAuthorityOptionalValue2)
                Dim bChkShowAllPayment As Boolean

                'If search criterea in session
                If oSearchcritaria IsNot Nothing Then
                    For lCount As Integer = 0 To oSearchcritaria.Count - 1
                        chklblShowOtherPayments.Checked = bChkShowAllPayment
                        sSourceType = CType(oSearchcritaria.Item(hiddenSource.ID), String)
                        'hiddenSource.Value = ""
                    Next
                End If

                If sSourceType = "" Then
                    If (chklblShowOtherPayments.Checked.ToString().ToUpper = "FALSE" And Not String.IsNullOrEmpty(Request.QueryString("CashListItemKey")) And Not String.IsNullOrEmpty(Request.QueryString("Type"))) Then
                        Session.Remove(CNSearchPaymentAuthorization)
                        hiddenSource.Value = "WMTask"
                    End If
                End If

                If chklblShowOtherPayments.Checked.ToString().ToUpper = "FALSE" And Not String.IsNullOrEmpty(Request.QueryString("CashListItemKey")) And (sSourceType = "WMTask" Or sSourceType = "") Then
                    Session.Remove(CNSearchPaymentAuthorization)
                Else
                    chklblShowOtherPayments.Enabled = False
                    chklblShowOtherPayments.Checked = False
                    lblShowOtherPayments.Visible = False
                    chklblShowOtherPayments.Visible = False
                    hiddenSource.Value = "Menu"

                End If

                If oSearchcritaria IsNot Nothing Then
                    For lCount As Integer = 0 To oSearchcritaria.Count - 1
                        txtPayeeName.Text = CType(oSearchcritaria.Item(txtPayeeName.ID), String)
                        txtDateFrom.Text = CType(oSearchcritaria.Item(txtDateFrom.ID), String)
                        txtDateTo.Text = CType(oSearchcritaria.Item(txtDateTo.ID), String)
                        txtCreatedBy.Text = CType(oSearchcritaria.Item(txtCreatedBy.ID), String)
                        ddlAssignedTo.SelectedValue = CType(oSearchcritaria.Item(ddlAssignedTo.ID), String)
                        ddlPaymentType.SelectedValue = CType(oSearchcritaria.Item(ddlPaymentType.ID), String)
                        hiddenSource.Value = CType(oSearchcritaria.Item(hiddenSource.ID), String)
                        bChkShowAllPayment = oSearchcritaria.Item(chklblShowOtherPayments.ID)
                        chklblShowOtherPayments.Checked = bChkShowAllPayment
                        If chklblShowOtherPayments.Checked.ToString().ToUpper = "TRUE" Then
                            ddlBranch.SelectedValue = CType(oSearchcritaria.Item(ddlBranch.ID), String)
                        End If
                    Next
                End If

                If sIsAuthoriser.Value = "0" Then
                    divHeader.Visible = False
                    divHidden.Visible = True
                    divSubmit.Visible = False
                Else
                    Dim source As String = IIf(String.Equals(sSourceType, "WMTask"), sSourceType, "")
                    Dim sourceMenu As String = IIf(String.Equals(sSourceType, "Menu"), sSourceType, "")
                    If (chklblShowOtherPayments.Checked.ToString().ToUpper = "FALSE" And Not String.IsNullOrEmpty(Request.QueryString("CashListItemKey")) And sSourceType = source) OrElse (sourceMenu = "Menu" AndAlso chklblShowOtherPayments.Checked.ToString().ToUpper = "FALSE") OrElse (chklblShowOtherPayments.Checked.ToString().ToUpper = "TRUE" And Not String.IsNullOrEmpty(Request.QueryString("CashListItemKey")) And sSourceType = source) Then
                        GridRefresh()
                    End If
                    'fillgrid()
                End If

            End If
        End Sub

        ''' <summary>
        ''' Find the Authorised Payment Data 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnFindNow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindNow.Click
            If Page.IsValid Then
                grdvAuthorisepayments.PageIndex = 0
                GridRefresh()
                ClearSearch()
                Dim oSearchcritaria As New Collection
                oSearchcritaria.Add(txtDateFrom.Text.Trim, txtDateFrom.ID)
                oSearchcritaria.Add(txtDateTo.Text.Trim, txtDateTo.ID)
                oSearchcritaria.Add(ddlAssignedTo.SelectedItem.Value.Trim, ddlAssignedTo.ID)
                oSearchcritaria.Add(txtCreatedBy.Text.Trim, txtCreatedBy.ID)
                oSearchcritaria.Add(ddlBranch.SelectedItem.Value.Trim, ddlBranch.ID)
                oSearchcritaria.Add(txtPayeeName.Text.Trim, txtPayeeName.ID)
                oSearchcritaria.Add(ddlPaymentType.SelectedItem.Value.Trim, ddlPaymentType.ID)
                oSearchcritaria.Add(chklblShowOtherPayments.Checked, chklblShowOtherPayments.ID)
                oSearchcritaria.Add(hiddenSource.Value, hiddenSource.ID)
                Session(CNSearchPaymentAuthorization) = oSearchcritaria
            End If
        End Sub
        ''' <summary>
        ''' Fill the Grid with Claim payment Data
        ''' </summary>
        ''' <remarks></remarks>
        Sub GridRefresh()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oAuthorisedPaymentCollection = New NexusProvider.AuthorisedPaymentCollection
            Dim oAutorisedPaymentRequestType = New NexusProvider.AutorisedPaymentRequestType
            Dim sURL As String
            Try
                If (chklblShowOtherPayments.Checked = False And chklblShowOtherPayments.Enabled.ToString().ToUpper = "TRUE" And Not String.IsNullOrEmpty(Request.QueryString("CashListItemKey")) And hiddenSource.Value = "WMTask") Then
                    oAutorisedPaymentRequestType.CashListItemKey = Request.QueryString("CashListItemKey")
                    oAutorisedPaymentRequestType.Branch = "(All Branches)"
                ElseIf chklblShowOtherPayments.Checked = True Then

                    oAutorisedPaymentRequestType.PaymentType = ddlPaymentType.SelectedValue.Trim
                    oAutorisedPaymentRequestType.Branch = ddlBranch.SelectedValue.Trim
                    If txtDateFrom.Text.Trim.Length <> 0 And IsDate(txtDateFrom.Text.Trim) = True Then
                        oAutorisedPaymentRequestType.DateFrom = CDate(txtDateFrom.Text.Trim)
                    End If
                    If txtDateFrom.Text.Trim.Length <> 0 Then
                        oAutorisedPaymentRequestType.DateFrom = CDate(txtDateFrom.Text.Trim)
                    Else
                        oAutorisedPaymentRequestType.DateFrom = Nothing
                    End If
                    If txtDateTo.Text.Trim.Length <> 0 Then
                        oAutorisedPaymentRequestType.DateTo = CDate(txtDateTo.Text.Trim)
                    Else
                        oAutorisedPaymentRequestType.DateTo = Nothing
                    End If
                    If txtCreatedBy.Text.Trim.Length <> 0 Then
                        oAutorisedPaymentRequestType.CreatedBy = txtCreatedBy.Text.Trim
                    Else
                        oAutorisedPaymentRequestType.CreatedBy = Nothing
                    End If

                    If ddlAssignedTo.SelectedValue.Trim.Length <> 0 Then
                        oAutorisedPaymentRequestType.AssignedTo = ddlAssignedTo.SelectedValue.Trim
                    Else
                        oAutorisedPaymentRequestType.AssignedTo = Nothing
                    End If
                    If txtPayeeName.Text.Trim.Length <> 0 Then
                        oAutorisedPaymentRequestType.PayeeName = txtPayeeName.Text.Trim
                    Else
                        oAutorisedPaymentRequestType.PayeeName = Nothing
                    End If
                Else
                    oAutorisedPaymentRequestType.PaymentType = ddlPaymentType.SelectedValue.Trim
                    If txtDateFrom.Text.Trim.Length <> 0 And IsDate(txtDateFrom.Text.Trim) = True Then
                        oAutorisedPaymentRequestType.DateFrom = CDate(txtDateFrom.Text.Trim)
                    End If
                    If txtDateFrom.Text.Trim.Length <> 0 Then
                        oAutorisedPaymentRequestType.DateFrom = CDate(txtDateFrom.Text.Trim)
                    Else
                        oAutorisedPaymentRequestType.DateFrom = Nothing
                    End If
                    If txtDateTo.Text.Trim.Length <> 0 Then
                        oAutorisedPaymentRequestType.DateTo = CDate(txtDateTo.Text.Trim)
                    Else
                        oAutorisedPaymentRequestType.DateTo = Nothing
                    End If
                    If txtCreatedBy.Text.Trim.Length <> 0 Then
                        oAutorisedPaymentRequestType.CreatedBy = txtCreatedBy.Text.Trim
                    Else
                        oAutorisedPaymentRequestType.CreatedBy = Nothing
                    End If

                    If ddlAssignedTo.SelectedValue.Trim.Length <> 0 Then
                        oAutorisedPaymentRequestType.AssignedTo = ddlAssignedTo.SelectedValue.Trim
                    Else
                        oAutorisedPaymentRequestType.AssignedTo = Nothing
                    End If
                    If txtPayeeName.Text.Trim.Length <> 0 Then
                        oAutorisedPaymentRequestType.PayeeName = txtPayeeName.Text.Trim
                    Else
                        oAutorisedPaymentRequestType.PayeeName = Nothing
                    End If
                    oAutorisedPaymentRequestType.Branch = ddlBranch.SelectedValue.Trim
                End If

                oAuthorisedPaymentCollection = oWebService.GetListofUnapprovedPayment(oAutorisedPaymentRequestType)


                grdvAuthorisepayments.Visible = True
                grdvAuthorisepayments.AllowPaging = True
                If (oNexusConfig.FinanceGridSize) = "" Then
                    grdvAuthorisepayments.PageSize = 10
                Else
                    grdvAuthorisepayments.PageSize = CStr(oNexusConfig.FinanceGridSize)
                End If
                GetUserPreferredColumnList()
                grdvAuthorisepayments.DataSource = oAuthorisedPaymentCollection
                grdvAuthorisepayments.DataBind()
                ColumnSelectorExtender1.Visible = True
                ScriptManager.RegisterStartupScript(Page, Me.GetType(), "id", "$get('" + ColumnSelectorExtender1.ClientID + "').style.display='block';", True)

                'ScriptManager.RegisterStartupScript(Page, Me.GetType(), "id", "$get('" + ColumnSelectorExtender.ClientID + "').style.display='none';", True)
                Cache.Insert(ViewState("AuthoriseClaimPaymentspageCacheID"), oAuthorisedPaymentCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
            Catch ex As System.Exception
                Throw
            Finally
                oWebService = Nothing
                oAutorisedPaymentRequestType = Nothing
            End Try

        End Sub
        Protected Sub FindUser()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oFindUsersSearchCriteria As New NexusProvider.FindUsersSearchCriteria
            Dim oUserCollection As New NexusProvider.UserCollection

            Try
                If txtCreatedBy.Text.Trim().Length <> 0 Then
                    oFindUsersSearchCriteria.UserName = txtCreatedBy.Text.Trim()
                    oUserCollection = oWebService.FindUsers(oFindUsersSearchCriteria)
                    Me.txtCreatedByKey.Value = oUserCollection(0).UserId
                End If

            Catch ex As System.Exception

            Finally
                oWebService = Nothing
                oFindUsersSearchCriteria = Nothing
                oUserCollection = Nothing
            End Try
        End Sub
        ''' <summary>
        ''' Authorise Claim Payment Grid Data Bound
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvAuthorisepayments_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvAuthorisepayments.DataBound
            If grdvAuthorisepayments.Rows.Count = 0 Or grdvAuthorisepayments.PageCount = 1 Then
                grdvAuthorisepayments.AllowPaging = False

            End If
            If (grdvAuthorisepayments.Rows.Count > 0) Then
                'grdvAuthorisepayments.HeaderRow.Cells(0).Visible = False
                grdvAuthorisepayments.Columns(0).Visible = False
            End If
        End Sub
        ''' <summary>
        ''' Page Index Change Event For Grid View
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvAuthorisepayments_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvAuthorisepayments.PageIndexChanging
            grdvAuthorisepayments.PageIndex = e.NewPageIndex
            grdvAuthorisepayments.DataSource = CType(Cache.Item(ViewState("AuthoriseClaimPaymentspageCacheID")), NexusProvider.AuthorisedPaymentCollection)
            grdvAuthorisepayments.DataBind()
        End Sub
        ''' <summary>
        ''' For Opening the Cali over view of the selected Claim.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvAuthorisepayments_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvAuthorisepayments.RowCommand

            Dim nCashListItemKey As Integer
            Dim sCreatedBy As String = String.Empty
            Dim mode As String = String.Empty
            Dim dAmount As Decimal

            If e.CommandName <> "Page" AndAlso e.CommandName <> "Sort" Then
                Dim commandArgs As String() = e.CommandArgument.ToString().Split(New Char() {","c})
                nCashListItemKey = CInt(commandArgs(0))
                sCreatedBy = commandArgs(1)
                dAmount = Math.Abs(Convert.ToDecimal(commandArgs(2)))
                Dim limitValue As Decimal = Convert.ToDecimal(hiddenLimitAmount.Value.Trim)


                If e.CommandName = "Authorise" Then

                    If Session(CNLoginName).ToString.Trim.ToUpper = sCreatedBy.Trim.ToUpper Then
                        Dim sErrorMessage = GetLocalResourceObject("Err_InvalidUser").ToString
                        Dim cstUserAuthorisation As New CustomValidator
                        cstUserAuthorisation.IsValid = False
                        'look for a validation message in the page resources, but if there is not one defined add a default message
                        cstUserAuthorisation.ErrorMessage = sErrorMessage
                        cstUserAuthorisation.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                        'add the validator to the page, this will have the effect of making the page invalid
                        Page.Validators.Add(cstUserAuthorisation)
                        Exit Sub
                    End If
                    If dAmount > limitValue Then
                        Dim sErrorMessage = GetLocalResourceObject("lbl_ApprovalWarningMsg")
                        Dim cstAuthorisationLimit As New CustomValidator
                        cstAuthorisationLimit.IsValid = False
                        'look for a validation message in the page resources, but if there is not one defined add a default message
                        cstAuthorisationLimit.ErrorMessage = sErrorMessage
                        cstAuthorisationLimit.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                        'add the validator to the page, this will have the effect of making the page invalid
                        Page.Validators.Add(cstAuthorisationLimit)
                        Exit Sub
                    End If

                    Dim iClaimPaymentKey As Integer = nCashListItemKey
                    Dim oPaymentCashList As New NexusProvider.PaymentCashListItemType
                    Dim bTempByte As Byte() = {0, 1, 0, 1, 0, 1, 0, 1}
                    mode = "AP"
                    oPaymentCashList.CashListKey = iClaimPaymentKey
                    oPaymentCashList.Declined = False
                    oPaymentCashList.TimeStamp = bTempByte
                    oPaymentCashList.CheckValidationOnly = True
                    Try

                        oWebservice = New NexusProvider.ProviderManager().Provider
                        oWebservice.ApproveCashListItem(oPaymentCashList)

                    Catch ex As NexusProvider.NexusException
                        If ex.Errors(0).Code = "331" Then   'Code : 331 :: Description: DebtorUserGroupsAreNotSetup

                            Dim cstDebtorUserGroups As New CustomValidator
                            cstDebtorUserGroups.IsValid = False
                            'look for a validation message in the page resources, but if there is not one defined add a default message
                            cstDebtorUserGroups.ErrorMessage = IIf(GetLocalResourceObject("cstDebtorUserGroups") Is Nothing, "Cannot Proceed- Debtor User Groups are not setup. Please contact your system administrator.", GetLocalResourceObject("cstDebtorUserGroups"))
                            cstDebtorUserGroups.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                            'add the validator to the page, this will have the effect of making the page invalid
                            Page.Validators.Add(cstDebtorUserGroups)
                            Exit Sub
                        ElseIf ex.Errors(0).Code = "332" Then
                            Dim cstSameUser As New CustomValidator
                            cstSameUser.IsValid = False
                            cstSameUser.ErrorMessage = IIf(GetLocalResourceObject("cstSameUser") Is Nothing, "Cannot Proceed- You are either the person who entered this payment or you have authorised a previous step. You can not authorise two steps on the same payment.", GetLocalResourceObject("cstSameUser"))
                            cstSameUser.Display = ValidatorDisplay.None
                            Page.Validators.Add(cstSameUser)
                            Exit Sub
                        ElseIf ex.Errors(0).Code = "336" Then 'Code : 336 :: Description: Unconfirmed/Unhandled Exceptions
                            Dim cstUnconfirmeds As New CustomValidator
                            cstUnconfirmeds.IsValid = False
                            cstUnconfirmeds.ErrorMessage = ex.Errors(0).Detail
                            cstUnconfirmeds.Display = ValidatorDisplay.None
                            Page.Validators.Add(cstUnconfirmeds)
                            Exit Sub
                        ElseIf ex.Errors(0).Code = "1000161" OrElse ex.Errors(0).Code = "1000159" Then    'Code : 1000161 :: Description: Auto Allocation failed
                            Dim sErrorMessage As String = IIf(GetLocalResourceObject("msgAllocationDeclined") Is Nothing, "Payment transaction has not been auto-allocated.", GetLocalResourceObject("msgAllocationDeclined"))
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Auto Allocation", "alert('" + sErrorMessage + "');window.location.href = '../../secure/WorkManager.aspx';", True)
                            Exit Sub
                        Else
                            Throw
                        End If
                    Finally
                        'cleaning up
                        oWebservice = Nothing
                        ' iClaimPaymentKey = Nothing
                        oPaymentCashList = Nothing
                        bTempByte = Nothing
                    End Try
                End If
                If e.CommandName = "Decline" Then
                    oWebservice = New NexusProvider.ProviderManager().Provider
                    Dim iCashListKey As Integer = nCashListItemKey
                    mode = "DP"
                    If Session(CNLoginName).ToString.Trim.ToUpper = sCreatedBy.Trim.ToUpper Then
                        Dim sErrorMessage = GetLocalResourceObject("Err_InvalidUser").ToString
                        Dim cstUserAuthorisation As New CustomValidator
                        cstUserAuthorisation.IsValid = False
                        'look for a validation message in the page resources, but if there is not one defined add a default message
                        cstUserAuthorisation.ErrorMessage = sErrorMessage
                        cstUserAuthorisation.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                        'add the validator to the page, this will have the effect of making the page invalid
                        Page.Validators.Add(cstUserAuthorisation)
                        Exit Sub
                    End If
                    If dAmount > limitValue Then
                        Dim sErrorMessage = GetLocalResourceObject("lbl_ApprovalWarningMsg")
                        Dim cstAuthorisationLimit As New CustomValidator
                        cstAuthorisationLimit.IsValid = False
                        'look for a validation message in the page resources, but if there is not one defined add a default message
                        cstAuthorisationLimit.ErrorMessage = sErrorMessage
                        cstAuthorisationLimit.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                        'add the validator to the page, this will have the effect of making the page invalid
                        Page.Validators.Add(cstAuthorisationLimit)
                        Exit Sub
                    End If
                    If Session(CNLoginName).ToString.Trim.ToUpper <> sCreatedBy.Trim.ToUpper Then
                        Dim oPaymentCashList As New NexusProvider.PaymentCashListItemType
                        Dim bTempByte As Byte() = {0, 1, 0, 1, 0, 1, 0, 1}
                        oPaymentCashList.CashListKey = iCashListKey
                        oPaymentCashList.Declined = True
                        oPaymentCashList.Comments = " "
                        oPaymentCashList.TimeStamp = bTempByte
                        oPaymentCashList.CheckValidationOnly = True
                        Try
                            oWebservice.ApproveCashListItem(oPaymentCashList)
                        Catch ex As NexusProvider.NexusException
                            If ex.Errors(0).Code = "330" Then   'Code : 330 :: Description: DebtorUserGroupsAreNotSetup

                                Dim cstDebtorUserGroups As New CustomValidator
                                cstDebtorUserGroups.IsValid = False
                                'look for a validation message in the page resources, but if there is not one defined add a default message
                                cstDebtorUserGroups.ErrorMessage = IIf(GetLocalResourceObject("cstDebtorUserGroups") Is Nothing, "Debtor User Groups are not setup. Please contact your system administrator", GetLocalResourceObject("cstDebtorUserGroups"))
                                cstDebtorUserGroups.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                                'add the validator to the page, this will have the effect of making the page invalid
                                Page.Validators.Add(cstDebtorUserGroups)
                                Exit Sub
                            ElseIf ex.Errors(0).Code = "331" Then   'Code : 331 :: Description: DebtorUserGroupsAreNotSetup

                                Dim cstDebtorUserGroups As New CustomValidator
                                cstDebtorUserGroups.IsValid = False
                                'look for a validation message in the page resources, but if there is not one defined add a default message
                                cstDebtorUserGroups.ErrorMessage = IIf(GetLocalResourceObject("cstDebtorUserGroups") Is Nothing, "Debtor User Groups are not setup. Please contact your system administrator", GetLocalResourceObject("cstDebtorUserGroups"))
                                cstDebtorUserGroups.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                                'add the validator to the page, this will have the effect of making the page invalid
                                Page.Validators.Add(cstDebtorUserGroups)
                                Exit Sub
                            ElseIf ex.Errors(0).Code = "332" Then
                                Dim cstSameUser As New CustomValidator
                                cstSameUser.IsValid = False
                                cstSameUser.ErrorMessage = IIf(GetLocalResourceObject("cstSameUser") Is Nothing, "Cannot Proceed- You are either the person who entered this payment or you have authorised a previous step.You can not authorise two steps on the same payment.", GetLocalResourceObject("cstSameUser"))
                                cstSameUser.Display = ValidatorDisplay.None
                                Page.Validators.Add(cstSameUser)
                                Exit Sub
                            ElseIf ex.Errors(0).Code = "1000161" Then    'Code : 1000161 :: Description: Auto Allocation failed
                                Dim sErrorMessage As String = IIf(GetLocalResourceObject("msgAllocationDeclined") Is Nothing, "Payment transaction has not been auto-allocated.", GetLocalResourceObject("msgAllocationDeclined"))
                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Auto Allocation", "alert('" + sErrorMessage + "');window.location.href = '../../secure/WorkManager.aspx';", True)
                                Exit Sub
                            ElseIf ex.Errors(0).Code = "1000164" Then 'Code : 1000164 :: Description: StepAuthorizationProcessErrorMessage
                                Dim cstSameUser As New CustomValidator
                                cstSameUser.IsValid = False
                                cstSameUser.ErrorMessage = ex.Errors(0).Detail
                                cstSameUser.Display = ValidatorDisplay.None
                                Page.Validators.Add(cstSameUser)
                                Exit Sub
                            ElseIf ex.Errors(0).Code = "336" Then
                                Dim cstUnconfirmedecline As New CustomValidator
                                cstUnconfirmedecline.IsValid = False
                                'look for a validation message in the page resources, but if there is not one defined add a default message
                                cstUnconfirmedecline.ErrorMessage = ex.Errors(0).Detail
                                cstUnconfirmedecline.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                                'add the validator to the page, this will have the effect of making the page invalid
                                Page.Validators.Add(cstUnconfirmedecline)
                                Exit Sub
                            Else
                                Throw ex
                            End If
                        Finally
                            'cleaning up
                            oWebservice = Nothing
                            iCashListKey = Nothing
                            oPaymentCashList = Nothing
                            bTempByte = Nothing
                        End Try

                    End If

                End If
                If e.CommandName = "Select" Then
                    mode = "VP"
                End If
                Response.Redirect("~/secure/payment/cashListItem.aspx?Type=Task&CashListItemKey=" & nCashListItemKey & "&Mode=" & mode & "")
            End If

        End Sub
        ''' <summary>
        ''' Authorise Payment Grid Row Data Bound event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvAuthorisepayments_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvAuthorisepayments.RowDataBound
            ColumnSelectorExtender1.Visible = True
            'ScriptManager.RegisterStartupScript(Page, Me.GetType(), "id", "$get('" + ColumnSelectorExtender.ClientID + "').style.display='block';", True)
        End Sub

        ''' <summary>
        ''' Method to Open the Claim and redirect to the overview page
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
            Session.Remove(CNSearchPaymentAuthorization)
            txtCreatedBy.Text = String.Empty
            txtCreatedByKey.Value = String.Empty
            txtDateFrom.Text = String.Empty
            txtDateTo.Text = DateTime.Now.ToString("d")
            txtPayeeName.Text = String.Empty
            ddlAssignedTo.SelectedIndex = 0
            ddlBranch.SelectedIndex = 0
            ddlPaymentType.SelectedIndex = 0
            grdvAuthorisepayments.Visible = False
            ddlBranch.Enabled = True
            hiddenSource.Value = String.Empty
            ColumnSelectorExtender1.Visible = True
            'ScriptManager.RegisterStartupScript(Page, Me.GetType(), "id", "$get('" + ColumnSelectorExtender.ClientID + "').style.display='block';", True)
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If HttpContext.Current.Session.IsCookieless Then
                btnCreatedBy.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/FindUser.aspx?modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=700' , null);return false;"
            Else
                btnCreatedBy.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/FindUser.aspx?modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=700' , null);return false;"
            End If
        End Sub
        ''' <summary>
        ''' In Page Initializing event following activites are done
        ''' 1. Bind ddlBranch with sorted branch collection.
        ''' 2. If ‘single branch is assigned to user , then control should be disabled. 
        ''' 3. Default Branch should be ‘picked from resource file.
        '''
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Dim UserGroup As NexusProvider.UserGroupCollection = Nothing
            Dim NewUser As New NexusProvider.UserGroup()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oOptionSettings As NexusProvider.OptionTypeSetting

            txtDateTo.Text = DateTime.Now.ToString("d")
            'If System Option for "Enhanced Case Search" is ON then we need to visible case related search criteria and grid column
            oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5099)

            If oOptionSettings IsNot Nothing AndAlso oOptionSettings.OptionValue IsNot Nothing AndAlso oOptionSettings.OptionValue(0) <> "0" Then
                bDisplayCaseOption = True
            End If
            'Fill Btranches
            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim oBranchCollection As New NexusProvider.BranchCollection
            oBranchCollection = oUserDetails.ListOfBranches
            If oBranchCollection.Count > 1 Then
                'Sort the branches
                oBranchCollection.SortColumn = "Description"
                oBranchCollection.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
                oBranchCollection.Sort()
            End If

            'Add a default item
            ddlBranch.Items.Clear()
            ddlBranch.DataSource = oBranchCollection
            ddlBranch.DataTextField = "Description"
            ddlBranch.DataValueField = "Code"
            ddlBranch.DataBind()

            'Add a default item for (All Branches)
            ddlBranch.Items.Insert(0, "(All Branches)")

            'If single branch is assigned to user, then control should be disabled
            If oBranchCollection.Count = 1 Then
                ddlBranch.SelectedIndex = 1
            End If

            If oBranchCollection.Count > 1 Then
                ddlBranch.Enabled = True
                'HeadOffice is not transaction branch.
                'So if user have selected any other branch then it should be default selected
                If Session(CNBranchCode) IsNot Nothing AndAlso CType(Session(CNBranchCode), String).ToUpper <> "HEADOFF" Then
                    ddlBranch.SelectedValue = CType(Session(CNBranchCode), String).ToUpper
                End If
            End If
            '----------------------------------------
            UserGroup = oWebService.GetUserGroups()
            NewUser.Code = "(All Group)"
            NewUser.Description = "(All Group)"
            NewUser.EffectiveDate = DateTime.Now
            NewUser.IsDeleted = False
            NewUser.IsSysAdmin = True
            NewUser.UserGroupKey = 0
            UserGroup.Add(NewUser)
            Dim oUserGroupCollection As NexusProvider.UserGroupCollection = New NexusProvider.UserGroupCollection()
            For Each oUserGroup As NexusProvider.UserGroup In UserGroup
                If Not oUserGroup.IsDeleted AndAlso oUserGroup.IsDebtorPMUserGroup Then
                    oUserGroupCollection.Add(oUserGroup)
                End If
            Next
            '----------------------------------------
            ddlAssignedTo.DataSource = oUserGroupCollection
            ddlAssignedTo.DataTextField = "Description"
            ddlAssignedTo.DataValueField = "Description"
            ddlAssignedTo.DataBind()
            ddlAssignedTo.Items.Insert(0, "All Group")
            '------------------Payment type ddl----------------------
            Dim oLookupCollection = New NexusProvider.LookupListCollection
            oLookupCollection = GISLookup_PaymentType.Items
            ddlPaymentType.DataSource = oLookupCollection
            ddlPaymentType.DataTextField = "Description"
            ddlPaymentType.DataValueField = "Code"
            ddlPaymentType.DataBind()
            ddlPaymentType.Items.Insert(0, "All  ")
            '------------------Payment type ddl----------------------
        End Sub
        ''' <summary>
        ''' sort the Authorise claim payments grid according to column click.
        ''' we need to store the current sort order in viewstate, and reverse it each time.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdvAuthorisepayments_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdvAuthorisepayments.Sorting

            Dim oCashListItems As NexusProvider.AuthorisedPaymentCollection = Cache.Item(ViewState("AuthoriseClaimPaymentspageCacheID"))
            oCashListItems.SortColumn = e.SortExpression
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
            oCashListItems.SortingOrder = _sortDirection
            oCashListItems.Sort()
            CType(sender, GridView).DataSource = oCashListItems
            CType(sender, GridView).DataBind()
        End Sub
        Protected Sub chklblShowOtherPayments_OnCheckedChange(ByVal sender As Object, ByVal e As EventArgs)
            If chklblShowOtherPayments.Checked = True Then
                Session.Remove(CNSearchPaymentAuthorization)
                txtCreatedBy.Text = String.Empty
                txtCreatedByKey.Value = String.Empty
                txtDateFrom.Text = String.Empty
                txtDateTo.Text = DateTime.Now.ToString("d")
                txtPayeeName.Text = String.Empty
                ddlAssignedTo.SelectedIndex = 0
                ddlBranch.SelectedIndex = 0
                ddlPaymentType.SelectedIndex = 0
                grdvAuthorisepayments.Visible = False
                ddlBranch.Enabled = True
            End If
        End Sub

        Public Sub GetUserPreferredColumnList()
            ColumnSelectorExtender1.Visible = True

            Dim sBranchCode As String = Session(CNBranchCode).ToString()
            Dim oUserPreferredColumns As NexusProvider.UserPreferredColumnList
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            oUserPreferredColumns = New NexusProvider.UserPreferredColumnList

            oUserPreferredColumns = oWebService.GetUserPreferredColumnList(sBranchCode, sInterfaceName)
            sColumnList = oUserPreferredColumns.ColumnList


            If (sColumnList IsNot Nothing) Then
                Dim sColumnListCollection() As String = sColumnList.Split(",")
                For i As Integer = 0 To grdvAuthorisepayments.Columns.Count - 1
                    If grdvAuthorisepayments.Columns.Item(i).HeaderText.ToString <> "" Then
                        grdvAuthorisepayments.Columns.Item(i).Visible = False
                        For colCnt As Integer = 0 To sColumnListCollection.Length - 1
                            If sColumnListCollection(colCnt) = grdvAuthorisepayments.Columns.Item(i).HeaderText Then
                                grdvAuthorisepayments.Columns.Item(i).Visible = True
                            End If
                        Next
                    End If
                Next
            End If

        End Sub

        Public Sub UpdateUserPreferredColumnList()
            Dim oSamProvider As NexusProvider.SAMForInsurance.ProviderSAMForInsuranceV2 = New NexusProvider.SAMForInsurance.ProviderSAMForInsuranceV2()
            Dim oUserPreferredColumns As NexusProvider.UserPreferredColumnList
            Dim sBranchCode As String = Session(CNBranchCode).ToString()
            oUserPreferredColumns = New NexusProvider.UserPreferredColumnList
            sColumnList = ""
            For i As Integer = 0 To ColumnSelectorExtender1.ColumnSelector.Items.Count - 1
                If ColumnSelectorExtender1.ColumnSelector.Items.Item(i).Selected Then
                    sColumnList = sColumnList & IIf(sColumnList = "", "", ",") & ColumnSelectorExtender1.ColumnSelector.Items.Item(i).Value
                End If
            Next

            oUserPreferredColumns.ColumnList = sColumnList
            oUserPreferredColumns.InterfaceName = sInterfaceName
            'oSearchTransactionColumn.IsSplitReceipt = .Item(SearchTransactionColumns.IsSplitReceipt).Selected


            oSamProvider.UpdateUserPreferredColumnList(oUserPreferredColumns, sBranchCode)
        End Sub

        Protected Sub custVldDate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles custVldDate.ServerValidate
            'Validation of Force from and Force to date
            If txtDateFrom.Text.Trim.Length <> 0 And IsDate(txtDateFrom.Text.Trim) = False Then
                args.IsValid = False
                custVldDate.ErrorMessage = GetLocalResourceObject("Err_InvalidDate")
            ElseIf txtDateTo.Text.Trim.Length <> 0 And IsDate(txtDateTo.Text.Trim) = False Then
                args.IsValid = False
                custVldDate.ErrorMessage = GetLocalResourceObject("Err_InvalidDate")
            ElseIf txtDateFrom.Text.Trim.Length <> 0 And txtDateTo.Text.Trim.Length <> 0 Then
                If CDate(txtDateFrom.Text.Trim) > CDate(txtDateTo.Text.Trim) Then
                    args.IsValid = False
                    custVldDate.ErrorMessage = GetLocalResourceObject("Err_InvalidDate")
                End If
            End If
        End Sub

        Private Sub AuthorizePayments_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
            Dim sBranchCode As String = Session(CNBranchCode).ToString()
            ColumnSelectorExtender1.BranchCode = sBranchCode
            ColumnSelectorExtender1.Inetrface = sInterfaceName
            GetUserPreferredColumnList()
        End Sub
    End Class

End Namespace