Imports System.Configuration.ConfigurationManager
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports NexusProvider.SAMForInsurance
Imports System.Collections.Generic
Imports Nexus
Imports System.Collections
Imports System.Web.UI.WebControls
Imports System.Linq
Imports System.Data
Imports System.Xml
Imports System.Activities.Expressions

Namespace Nexus
    Partial Class Controls_CashListItemtab
        Inherits System.Web.UI.UserControl

        Public Const kIsValidationEnabled As String = "IS_VALIDATION_ENABLED"
        Public Const kMediaTypeID As String = "MEDIATYPE_ID"
        Dim oWebservice As NexusProvider.ProviderBase
        Dim oCashListItems As NexusProvider.PaymentCashListItemType
        Dim oReceiptCashListItems As NexusProvider.ReceiptCashListItemType
        Dim oCashListItem As New NexusProvider.PaymentItems
        Dim oUpdateCashListItem As NexusProvider.PaymentItems

        Shared sCreatedBy As String
        Dim oInstalmentPlanDetailsCollection As New NexusProvider.InstalmentPlanDetailsCollection
        Dim dOverallSelectedAmount As Decimal = 0
        Dim dMaxAmountToWriteOff As Decimal = 0
        Dim dDifferenceAmount As Decimal = 0
        Dim oPaymentHubEnabled As NexusProvider.OptionTypeSetting
        Dim hdnTabName As HiddenField
        Dim hdnAddMoreCashList As HiddenField
        Dim CashListItemID As HiddenField
        Dim hfType As HiddenField
        Dim hfMode As HiddenField
        Dim bIsIncludePaymentTypeClaimPayment As Boolean
        Dim bApprovalMessage As Boolean = False



        Private Sub setDefaultDatesAndHiddenControlValues()
            'Collection date is always not allowed to be greater than the system date
            rngCollectionDate.MaximumValue = Date.Now.ToShortDateString

            'ChequeDate Cantnot be greater than the system date
            'and ChequeDate Cannot be More Than 150 Days Old
            rngChequeDate.MinimumValue = Date.Now.AddDays(-150).ToShortDateString
            rngChequeDate.MaximumValue = Date.Now.ToShortDateString

            'will use for the warningmessage (function name: WarningMsg) for Media Type CHEQUE
            hiddentxtwarningMinChequeDate.Value = Date.Now.AddDays(-90).ToShortDateString
            hiddentxtwarningchequedate.Value = Date.Now.AddDays(-150).ToShortDateString

            'set Current Date as default value for Collection Date control
            Cash_List_Item__Collection_Date.Text = Date.Now.ToShortDateString()

            'set Current Date as default value for Transaction Date control and if needed will overwrite below
            Cash_List_Item__Transaction_Date.Text = Date.Now.ToShortDateString()

        End Sub
        Sub FillMediaType()
            oWebservice = New NexusProvider.ProviderManager().Provider
            Dim oMediaList As NexusProvider.LookupListCollection
            Dim oReceiptMediaList As New NexusProvider.LookupListCollection
            Dim oPaymentMediaList As New NexusProvider.LookupListCollection
            Dim v_sOptionList As System.Xml.XmlElement = Nothing
            Dim oDictMediaReferenceMandatory As New Dictionary(Of String, Integer)
            oMediaList = oWebservice.GetList(NexusProvider.ListType.PMLookup, "MediaType", True, False, , , , v_sOptionList)
            'Sort the list

            oMediaList.Sort(NexusProvider.DataItemTypes.Description, NexusProvider.Direction.Asc)

            Dim hCurrentOptionColl As New Hashtable()
            Dim oMediaTypeDetails As New Hashtable()
            'Load the xml element 
            If v_sOptionList IsNot Nothing Then
                Dim sXML As String = v_sOptionList.OuterXml
                Dim xmlDoc As New System.Xml.XmlDocument()
                xmlDoc.LoadXml(sXML)

                If oMediaList IsNot Nothing Then
                    For iMediaCount As Integer = 0 To oMediaList.Count - 1
                        If xmlDoc.ChildNodes IsNot Nothing Then
                            For iCount As Integer = 0 To xmlDoc.ChildNodes(0).ChildNodes.Count - 1
                                Dim iMediaTypeId, iReceipt, iPayment, iIsAdditionalDetails, isReceiptPrintedAutomatically As Integer
                                Dim nIsValidationEnabled As Integer
                                Dim iIsMediaReferenceMandatory As Integer
                                Dim sMediaTypeCode As String = ""
                                For iChildCount As Integer = 0 To xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes.Count - 1
                                    If xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).Name.Trim.ToUpper = "MEDIATYPE_ID" Then
                                        iMediaTypeId = CInt(xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).InnerText)
                                    ElseIf xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).Name.Trim.ToUpper = "CODE" Then
                                        sMediaTypeCode = CStr(xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).InnerText)
                                    ElseIf xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).Name.Trim.ToUpper = "IS_RECEIPT" Then
                                        iReceipt = CInt(xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).InnerText)
                                    ElseIf xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).Name.Trim.ToUpper = "IS_PAYMENT" Then
                                        iPayment = CInt(xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).InnerText)
                                    ElseIf xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).Name.Trim.ToUpper = "IS_ADDITIONAL_DETAILS" Then
                                        iIsAdditionalDetails = CInt(xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).InnerText)
                                    ElseIf xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).Name.Trim.ToUpper = "IS_MEDIA_REFERENCE_MANDATORY" Then
                                        iIsMediaReferenceMandatory = CInt(xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).InnerText)
                                        If oDictMediaReferenceMandatory.ContainsKey(sMediaTypeCode.Trim) = False Then
                                            oDictMediaReferenceMandatory.Add(sMediaTypeCode.Trim, iIsMediaReferenceMandatory)
                                        End If
                                    ElseIf xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).Name.Trim.ToUpper = "IS_RECEIPT_PRINTED_AUTOMATICALLY" Then
                                        isReceiptPrintedAutomatically = CInt(xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).InnerText)
                                        If oDictMediaReferenceMandatory.ContainsKey(sMediaTypeCode.Trim + "_IS_RECEIPT_PRINTED_AUTOMATICALLY") = False Then
                                            oDictMediaReferenceMandatory.Add(sMediaTypeCode.Trim + "_IS_RECEIPT_PRINTED_AUTOMATICALLY", isReceiptPrintedAutomatically)
                                        End If
                                    End If
                                    If xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).Name.Trim.ToUpper = kIsValidationEnabled Then
                                        nIsValidationEnabled = CInt(xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).InnerText)
                                    End If
                                Next
                                If oMediaList(iMediaCount).Key = iMediaTypeId Then
                                    If Not (ViewState("PaymentHubEnabled") <> "1" AndAlso sMediaTypeCode.Trim() = "OCP") Then
                                        If iReceipt > 0 Then
                                            oReceiptMediaList.Add(oMediaList(iMediaCount))
                                        End If

                                        If iPayment > 0 Then
                                            oPaymentMediaList.Add(oMediaList(iMediaCount))
                                        End If

                                        If iIsAdditionalDetails > 0 Then
                                            'Store the Is Additional Details with MediaType ID
                                            If hCurrentOptionColl.ContainsKey(oMediaList(iMediaCount).Code.Trim) = False Then
                                                hCurrentOptionColl.Add(oMediaList(iMediaCount).Code.Trim, iIsAdditionalDetails)
                                            End If
                                        End If

                                        'Store Details in the follwing format, this has been made generic so can be extended in future
                                        'Code | ColumnName
                                        If nIsValidationEnabled > 0 Then
                                            oMediaTypeDetails.Add(oMediaList(iMediaCount).Code.Trim + "|" + kIsValidationEnabled, nIsValidationEnabled)
                                            oMediaTypeDetails.Add(oMediaList(iMediaCount).Code.Trim + "|" + kMediaTypeID, iMediaTypeId)
                                        End If

                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                    Next
                End If
                ' GISLookup_PaymentType.Value = "AGPAY"

                oReceiptMediaList.Sort(NexusProvider.DataItemTypes.Description, NexusProvider.Direction.Asc)
                If Session("CashListItemFirstLoad") = True OrElse Request.QueryString("Type") = "Task" Then
                    If Request.QueryString.HasKeys() Then
                        GISLookup_ReceiptType.Value = "STD"
                    End If
                    If PnlReceiptType.Visible = True Then
                        lblReceiptTypeHeading.Text = GetLocalResourceObject("lbl_ReceiptType")
                        GISLookup_MediaType.Items.Clear()
                        GISLookup_MediaType.DataSource = oReceiptMediaList
                        GISLookup_MediaType.DataTextField = "Description"
                        GISLookup_MediaType.DataValueField = "Code"
                        GISLookup_MediaType.DataBind()

                    ElseIf liPaymentType.Visible = True Then
                        lblReceiptTypeHeading.Text = GetLocalResourceObject("lbl_PaymentType")
                        GISLookup_MediaType.Items.Clear()
                        GISLookup_MediaType.DataSource = oPaymentMediaList
                        GISLookup_MediaType.DataTextField = "Description"
                        GISLookup_MediaType.DataValueField = "Code"
                        GISLookup_MediaType.DataBind()
                    End If
                    GISLookup_MediaType.Items.Insert(0, New ListItem(GetLocalResourceObject("lbl_MediaDefaultText"), ""))
                End If
                'Store the Is Additional Details with MediaType ID

                'Set Default Media Type For Pay Claim
                If Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.Authorise Or Session(CNMode) = Mode.Recommend Or Session(CNUnAllocatedClaimPayment) IsNot Nothing Then
                    Dim iMediaTypeId As Integer
                    Dim iBankAccountId As Integer
                    GetBankAccountDefault(iMediaTypeId, iBankAccountId, 3)
                    If Session(CNUnAllocatedClaimPayment) IsNot Nothing AndAlso iMediaTypeId = 0 Then
                        iMediaTypeId = CType(Session(CNUnAllocatedClaimPayment), NexusProvider.UnallocatedClaimPayments).PayeeMediaTypeKey
                    End If
                    If iMediaTypeId > 0 Then
                        Dim strMediaTypeCode As String
                        strMediaTypeCode = GetCodeForKey(NexusProvider.ListType.PMLookup, iMediaTypeId, "MediaType", True, Session(CNBranchCode))
                        GISLookup_MediaType.ClearSelection()
                        GISLookup_MediaType.SelectedValue = strMediaTypeCode
                    End If
                End If
                ViewState("CurrentOptionColl") = hCurrentOptionColl
                ViewState("MediaTypeDetails") = oMediaTypeDetails
                Dim MediaRefMandatoryCacheID As Guid
                MediaRefMandatoryCacheID = Guid.NewGuid()
                ViewState.Add("MediaRefMandatoryCacheID", MediaRefMandatoryCacheID.ToString)
                Cache.Insert(ViewState("MediaRefMandatoryCacheID"), oDictMediaReferenceMandatory, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

            End If
        End Sub

        Function IsClaimIncludedInPayment() As Boolean
            oWebservice = New NexusProvider.ProviderManager().Provider
            Dim oList As NexusProvider.LookupListCollection
            Dim v_sOptionList As System.Xml.XmlElement = Nothing
            'oList = oWebservice.GetList(NexusProvider.ListType.PMLookup, "Debtor_User_Groups", True, False, GISLookup_ReceiptType.Text, -1, GISLookup_ReceiptType.Value, v_sOptionList)

            'Dim hCurrentOptionColl As New Hashtable()
            If Session("PaymentType") = "CP" Then
                'Load the xml element 
                oList = oWebservice.GetList(NexusProvider.ListType.PMLookup, "Debtor_User_Groups", True, False, GISLookup_ReceiptType.Text, -1, GISLookup_ReceiptType.Value, v_sOptionList)
                If v_sOptionList IsNot Nothing Then
                    Dim sXML As String = v_sOptionList.OuterXml
                    Dim xmlDoc As New System.Xml.XmlDocument()
                    xmlDoc.LoadXml(sXML)

                    If oList IsNot Nothing Then
                        For iMediaCount As Integer = 0 To oList.Count - 1
                            If xmlDoc.ChildNodes IsNot Nothing Then
                                For iCount As Integer = 0 To xmlDoc.ChildNodes(0).ChildNodes.Count - 1
                                    Dim iDebtorUserGrpType, iOption As Integer
                                    For iChildCount As Integer = 0 To xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes.Count - 1
                                        If xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).Name.Trim.ToUpper = "DEBTOR_USER_GROUPS_TYPE_ID" Then
                                            iDebtorUserGrpType = CInt(xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).InnerText)
                                        End If
                                        If xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).Name.Trim.ToUpper = "IS_PAYMENT_TYPE_CLAIM_PAYMENT" Then
                                            iOption = CInt(xmlDoc.ChildNodes(0).ChildNodes(iCount).ChildNodes(iChildCount).InnerText)
                                        End If
                                    Next
                                    If iDebtorUserGrpType = 6 Then
                                        Return (iOption = 1)
                                        Exit For
                                    End If
                                Next
                            End If
                        Next
                    End If
                End If
            Else
                Return True
            End If
        End Function

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "PaymentConfirmation",
                                                    "<script language=""JavaScript"" type=""text/javascript"">function PaymentConfirmation(){var r= confirm('" & GetLocalResourceObject("msg_AnotherPayment").ToString() & "'); document.getElementById('" & hidChkChoice.ClientID & "').value=r;}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ClaimCloseConfirmation",
                                                        "<script language=""JavaScript"" type=""text/javascript"">function ClaimCloseConfirmation(){var r= confirm('" & GetLocalResourceObject("msg_CloseClaim").ToString() & "'); document.getElementById('" & hidChlClaimClose.ClientID & "').value=r; if (document.getElementById('" & hidChkPaymentMsg.ClientID & "').value ==  '0' && r == false) {PaymentConfirmation();}}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ApprovalAlert", "<script language=""JavaScript"" type=""text/javascript"">function ApprovalAlert(){alert('" & GetLocalResourceObject("msg_ApprovalAlert").ToString() & "')}</script>")
            oWebservice = New NexusProvider.ProviderManager().Provider

            oPaymentHubEnabled = oWebservice.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.SystemOptionPaymentHubEnabled)
            ViewState("PaymentHubEnabled") = oPaymentHubEnabled.OptionValue
            Dim oPaymentHubDetails As NexusProvider.PaymentHubDetails
            oPaymentHubDetails = Session(CNPaymentHubDetails)
            If Session(CNQuote) IsNot Nothing AndAlso oPaymentHubDetails IsNot Nothing AndAlso oPaymentHubDetails.ResultDescription = PaymentHub.ResultDescription.IncorrectCardDetailsEntered Then
                Response.Redirect("~/secure/TransactionConfirmation.aspx")
            End If
            If Not IsPostBack Then
                setDefaultDatesAndHiddenControlValues()
            End If
        End Sub
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                Session("btnOKClicked") = Nothing
            End If
            sCreatedBy = Nothing
            txtTotalAmount.Visible = False
            lblTotalAmount.Visible = False
            rvtxtTotalAmount.Enabled = False
            If (Request.QueryString("Mode") = "CR") Then
                btnOk.Visible = True
            End If
            If Not Request.QueryString("CashListItemKey") Is Nothing AndAlso Not String.IsNullOrEmpty(Request.QueryString("CashListItemKey")) Then
                oWebservice = New NexusProvider.ProviderManager().Provider
                oCashListItems = oWebservice.GetPaymentCashListItemDetails(Request.QueryString("CashListItemKey"))
                sCreatedBy = oCashListItems.UserName
            End If
            Dim oNexusFrameWork As Nexus.Library.Config.NexusFrameWork = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork)
            hdnTabName = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("hdnTabName"), HiddenField)
            hdnAddMoreCashList = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("hdnAddMoreCashList"), HiddenField)
            CashListItemID = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("hfCashListItemID"), HiddenField)
            hfMode = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("hfMode"), HiddenField)
            hfType = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("hfType"), HiddenField)

            Dim aa As Boolean = CBool(Session("CashListItemFirstLoad"))

            If Request("__EVENTARGUMENT") = "EditClick" Or Request("__EVENTARGUMENT") = "ViewClick" Then
                Session("ModeValue") = hfMode.Value
                Session("Type") = hfType.Value
                Session("SetFlag") = 0
                If GISLookup_ReceiptType.Value = "INST" Then
                    ddlInstalmentPlan.Enabled = False
                End If
            End If
            If Request("__EVENTARGUMENT") = "ViewAllocationClick" Then
                Session("ModeValue") = "Allocation"
            End If
            If Request("__EVENTARGUMENT") = "EditClick" AndAlso Session("ModeValue") = "IP" Then
                Session("btnOKClicked") = Nothing
            End If


            If Request.QueryString("Mode") = "AP" OrElse Request.QueryString("Mode") = "CR" OrElse Request.QueryString("Mode") = "Payment" OrElse Request.QueryString("Mode") = "VP" OrElse Request.QueryString("Mode") = "DP" OrElse Request.QueryString("Mode") = "PayNow" OrElse ((((Request("__EVENTARGUMENT") IsNot Nothing And Request("__EVENTARGUMENT") = "CRCashListItem") And (Session(CNCashListItem) IsNot Nothing And hdnTabName.Value = "tab-CashListItem")) Or (Session(CNCashListItem) IsNot Nothing And hdnTabName.Value = "tab-CashListItem" And Session("AddMoreCashList") = "Yes") Or (Page.IsPostBack And Session(CNCashListItem) IsNot Nothing And hdnTabName.Value = "tab-CashListItem"))) Then
                If Visible Then

                    If Request.QueryString("error") IsNot Nothing AndAlso Request.QueryString("error") = "Token" Then
                        Session(CNPaymentHubDetails) = Nothing
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowValidation", "alert('" + GetLocalResourceObject("msg_ErrorTokenRegistration").ToString() + "');", True)

                    End If
                    'if control is NOT visible and NOT postback, aviod all actions on page_load
                    Dim iFlag As Integer = Session("SetFlag")
                    Dim sType As String = IIf(String.IsNullOrEmpty(Session("Type")), Request.QueryString("Type"), Session("Type"))
                    Dim sSetFocusOnControl As String = Nothing
                    'test 
                    'setDefaultDatesAndHiddenControlValues()


                    If sType <> "Task" Then
                        sCreatedBy = String.Empty
                    End If


                    If sType = "Task" And Not String.IsNullOrEmpty(Request.QueryString("CashListItemKey")) Then
                        'This code executes when this page is called from task i.e Run the Task
                        'Approval of Cash/Cheque Payments(Multi Step Approval) item through Work Manager

                        oWebservice = New NexusProvider.ProviderManager().Provider
                        oCashListItems = oWebservice.GetPaymentCashListItemDetails(Request.QueryString("CashListItemKey"))
                        Session(CNTimeStamp) = oCashListItems.TimeStamp

                        Dim oPaymentCashList As New NexusProvider.PaymentCashListItemType
                        ManageControlsForTaskActivity() 'this will manage the task for task activity
                        'Load the Media Type as per IsReceipt or IsPayment
                        FillMediaType()
                        GetAuthorizationComment()
                        liCommentTab.Style.Clear()
                        With oCashListItems
                            sCreatedBy = .UserName
                            GISLookup_PaymentType.Value = .TypeCode.Trim
                            If .IsProduceDocument Then
                                chkProduceDocument.Checked = True
                            Else
                                chkProduceDocument.Checked = False
                            End If

                            Cash_List_Item__Transaction_Date.Text = .TransactionDate
                            GISLookup_MediaType.SelectedValue = .MediaTypeCode.Trim
                            txtMediaReference.Text = .MediaReference
                            txtBankReference.Text = .BankReference
                            txtOurReference.Text = .OurReference
                            txtTheirReference.Text = .TheirReference

                            txtAccount.Text = .AccountShortCode
                            txtAllocationStatus.Text = .AllocationStatusCode
                            ddlStatus.Value = .StatusCode
                            txtName.Text = .ContactName
                            txtDetails.Text = .FurtherDetails
                            If .Amount < 0 Then
                                txtAmount.Text = .Amount * -1
                            Else
                                txtAmount.Text = .Amount
                            End If

                            If .Bank IsNot Nothing Then
                                txtPayeeName.Text = .Bank.PayeeName
                                txtAccountCode.Text = .Bank.AccountCode
                                txtExpiryDate.Text = .Bank.ExpiryDate
                                txtBranchCode.Text = .Bank.BranchCode
                                txtReference1.Text = .Bank.Reference1
                                txtReference2.Text = .Bank.Reference2
                                txtBIC.Text = .Bank.BIC
                                txtIBAN.Text = .Bank.IBAN
                            End If

                            If .ContactAddress IsNot Nothing Then
                                PayNow_Address.Address1 = .ContactAddress.Address1.Trim
                                PayNow_Address.Address2 = .ContactAddress.Address2.Trim
                                PayNow_Address.Address3 = .ContactAddress.Address3.Trim
                                PayNow_Address.Address4 = .ContactAddress.Address4.Trim
                                PayNow_Address.CountryCode = .ContactAddress.CountryCode.Trim
                                PayNow_Address.Postcode = .ContactAddress.PostCode.Trim
                            End If

                        End With

                        sSetFocusOnControl = GISLookup_PaymentType.ClientID

                    ElseIf Session("ModeValue") = "IP" Then
                        'User is doing the Insurer Payement and clicked on Pay Button
                        lblWriteOffAmount.Visible = True
                        txtWriteOffAmount.Visible = True

                        ManageControlsForInsurerPayments() 'this will manage the controls for "Insurer Payments"

                        If Session(CNCashListItem) IsNot Nothing AndAlso TryCast(Session(CNCashListItem), NexusProvider.PaymentCashListItemType) IsNot Nothing Then
                            sSetFocusOnControl = GISLookup_PaymentType.ClientID
                        Else
                            sSetFocusOnControl = GISLookup_ReceiptType.ClientID
                        End If

                        'This should be called only in case of edit records from Insure Payments
                        If Not String.IsNullOrEmpty(CashListItemID.Value()) Then
                            PopulateUpdateObject()
                        End If
                    ElseIf Session("ModeValue") = "CR" Then 'Cash/Cheque Receipts/Payments
                        ManageCashChequeListReceiptControls()
                        If GISLookup_MediaType.SelectedValue = String.Empty Then
                            FillMediaType()
                        End If

                        'This code will retreive the address if user directly type the account code
                        Me.txtAccount.Attributes.Add("onblur", "javascript:return getAccount('" & UpdateAddress.ClientID.ToString & "');")
                        If txtAccount.Text <> "" Then
                            Session("PartyKey") = hiddenTempText.Value
                        End If
                        If CashListItemID IsNot Nothing Then
                            If Not String.IsNullOrEmpty(CashListItemID.Value()) Then
                                If Session("ModeType") = "Payment" Then 'Payments
                                    If CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType).PaymentItems.Count > 0 Then
                                        oCashListItem = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType).PaymentItems.Item(CType(CashListItemID.Value(), Integer))
                                    End If
                                    sSetFocusOnControl = GISLookup_PaymentType.ClientID
                                ElseIf Session("ModeType") = "Receipt" Then 'Receipts
                                    If CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType).ReceiptItems.Count > 0 Then
                                        oCashListItem = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType).ReceiptItems.Item(CType(CashListItemID.Value(), Integer))
                                    End If
                                    sSetFocusOnControl = GISLookup_ReceiptType.ClientID
                                    'Page.ClientScript.RegisterStartupScript(GetType(String), "HidePaymentTab", "HidePaymentTab();", True)
                                    ScriptManager.RegisterStartupScript(Me, Page.GetType(), "HidePaymentTab", "HidePaymentTab();", True)
                                End If

                                'issue - on postback does not allow to change value
                                'PopulateUpdateObject()

                                If Request("__EVENTARGUMENT") = "EditClick" Then
                                    PopulateUpdateObject()
                                End If
                                If Request("__EVENTARGUMENT") = "ViewClick" Then
                                    PopulateUpdateObject()
                                    ManageControlsForNextClick()


                                    Dim oNexusConfig As Nexus.Library.Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork)
                                    Dim btnCashListItemNext As LinkButton = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnCashListItemNext"), LinkButton)
                                    Dim btnCashListItemCancel As LinkButton = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnCashListItemCancel"), LinkButton)
                                    btnCashListItemNext.Visible = False
                                    btnCashListItemCancel.Visible = False

                                    Dim btnClose2 As LinkButton = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnClose2"), LinkButton)
                                    btnClose2.Visible = True
                                End If
                            End If
                        End If

                        If Session("ModeType") = "Payment" Then 'Payments
                            PnlPayeeInformation.Visible = False
                            sSetFocusOnControl = GISLookup_PaymentType.ClientID
                        Else
                            'Cash/Cheque Receipts
                            PnlPayeeInformation.Visible = False
                            sSetFocusOnControl = GISLookup_ReceiptType.ClientID
                            'Page.ClientScript.RegisterStartupScript(GetType(String), "HidePaymentTab", "HidePaymentTab();", True)
                            ScriptManager.RegisterStartupScript(Me, Page.GetType(), "HidePaymentTab", "HidePaymentTab();", True)
                        End If
                    ElseIf Session("ModeValue") = "Receipt" Then 'Cash/Cheque Receipts/Payments
                        ManageCashChequeListReceiptControls()

                        FillMediaType()

                        'This code will retreive the address if user directly type the account code
                        Me.txtAccount.Attributes.Add("onblur", "javascript:return getAccount('" & UpdateAddress.ClientID.ToString & "');")

                        If Not String.IsNullOrEmpty(CashListItemID.Value()) Then
                            If Session("ModeType") = "Payment" Then 'Payments
                                If CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType).PaymentItems.Count > 0 Then
                                    oCashListItem = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType).PaymentItems.Item(CType(CashListItemID.Value(), Integer))
                                End If
                                sSetFocusOnControl = GISLookup_PaymentType.ClientID
                            ElseIf Session("ModeType") = "Receipt" Then 'Receipts
                                If CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType).ReceiptItems.Count > 0 Then
                                    oCashListItem = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType).ReceiptItems.Item(CType(CashListItemID.Value(), Integer))
                                End If
                                sSetFocusOnControl = GISLookup_ReceiptType.ClientID
                                'Page.ClientScript.RegisterStartupScript(GetType(String), "HidePaymentTab", "HidePaymentTab();", True)
                                ScriptManager.RegisterStartupScript(Me, Page.GetType(), "HidePaymentTab", "HidePaymentTab();", True)
                            End If
                            PopulateUpdateObject()
                        End If
                        If Session("ModeType") = "Payment" Then 'Payments
                            PnlPayeeInformation.Visible = False
                            sSetFocusOnControl = GISLookup_PaymentType.ClientID
                        Else
                            'Cash/Cheque Receipts
                            PnlPayeeInformation.Visible = False
                            sSetFocusOnControl = GISLookup_ReceiptType.ClientID
                            'Page.ClientScript.RegisterStartupScript(GetType(String), "HidePaymentTab", "HidePaymentTab();", True)
                            ScriptManager.RegisterStartupScript(Me, Page.GetType(), "HidePaymentTab", "HidePaymentTab();", True)
                        End If
                    ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then 'MTA Cancellation Payment

                        ManageControlsForMTACancellation()
                        DisplayAccountInformation_MTC()

                        PnlPayeeInformation.Visible = False
                        sSetFocusOnControl = GISLookup_PaymentType.ClientID
                    ElseIf Session("ModeValue") = "INS" Then
                        'User is doing the Insurer Payement and clicked on Pay Button
                        lblWriteOffAmount.Visible = True
                        txtWriteOffAmount.Visible = True

                        Session("ModeType") = "Receipt"
                        DisplayControls()
                        FillMediaType()
                        ManageCashChequeListReceiptControls() 'this will manage the controls for "Instalments"

                        sSetFocusOnControl = GISLookup_ReceiptType.ClientID
                        btnAccount.Enabled = False
                        txtAccount.Enabled = False
                    ElseIf Session("ModeValue") = "INSDEPOSIT" Then
                        Dim iMediaTypeId As Integer
                        Dim sMediaTypeCode As String = String.Empty
                        Dim iBankAccountId As Integer
                        Dim sBankAccountCode As String
                        GetBankAccountDefault(iMediaTypeId, iBankAccountId, 2) ''get current client details

                        If iBankAccountId > 0 Then
                            sMediaTypeCode = GetCodeForKey(NexusProvider.ListType.PMLookup, iMediaTypeId, "MediaType", True)
                            sBankAccountCode = GetCodeForKey(NexusProvider.ListType.PMLookup, iBankAccountId, "BankAccount", True)
                        End If
                        Session("ModeType") = "Receipt"
                        DisplayControls()
                        FillMediaType()
                        ManageCashChequeListReceiptControls() 'this will manage the controls for "Instalments"

                        sSetFocusOnControl = GISLookup_ReceiptType.ClientID
                        btnAccount.Enabled = False
                        txtAccount.Enabled = False
                        Cash_List_Item__Transaction_Date.Text = Date.Today
                        If Not String.IsNullOrEmpty(sMediaTypeCode) Then
                            GISLookup_MediaType.SelectedValue = sMediaTypeCode
                        End If

                        ''set default value to Instalment Deposit if present in the dropdown
                        If GISLookup_ReceiptType.Items.FindItemByCode("INSTDEPT") IsNot Nothing Then
                            GISLookup_ReceiptType.Value = "INSTDEPT"
                            GISLookup_ReceiptType.Enabled = False
                        End If
                    Else
                        Dim sOption As String
                        Dim oClaimOpen As NexusProvider.ClaimOpen = Session(CNClaim)
                        If oClaimOpen IsNot Nothing Then
                            oWebservice = New NexusProvider.ProviderManager().Provider
                            sOption = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsGrossClaimPaymentAmount, NexusProvider.RiskTypeOptions.None, Session(CNProductCode), Nothing)
                            If String.IsNullOrEmpty(sOption) Then
                                hIsGrossClaimPaymentAmount.Value = "0"
                            Else
                                hIsGrossClaimPaymentAmount.Value = sOption
                            End If
                            sOption = String.Empty
                            sOption = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, Nothing, NexusProvider.RiskTypeOptions.ClaimsIsPostTaxes, Nothing, oClaimOpen.RiskType)
                            If String.IsNullOrEmpty(sOption) Then
                                hClaimsIsPostTaxes.Value = "0"
                            Else
                                hClaimsIsPostTaxes.Value = sOption
                            End If
                        End If
                        DisplayControls() 'like Claim Payment Processing, NB/MTA/Renewals, Pay Claim
                        sSetFocusOnControl = Cash_List_Item__Transaction_Date.ClientID
                    End If

                    'set the focus on the appropriate control depends on the condition applied
                    'To set the Focus
                    Page.SetFocus(sSetFocusOnControl)
                End If
                ScriptManager.RegisterStartupScript(Me, Page.GetType(), "ShowHideInstalTab", "ShowHideInstalTab();", True)
                Session("CashListItemFirstLoad") = False
            End If
            'Updation of the Address based on the Account Key
            If Request("__EVENTARGUMENT") = "RefreshIP" Then
                'Reset the Address control
                txtName.Text = String.Empty
                PayNow_Address.Address1 = String.Empty
                PayNow_Address.Address2 = String.Empty
                PayNow_Address.Address3 = String.Empty
                PayNow_Address.Address4 = String.Empty
                PayNow_Address.CountryCode = String.Empty
                PayNow_Address.Postcode = String.Empty

                If Session("ModeType") = "Payment" Or Session("ModeType") = "Receipt" Then
                    If String.IsNullOrEmpty(hiddenTempText.Value) = False AndAlso hiddenTempText.Value <> "0" AndAlso Session("PartyKey") <> "" Then
                        'AndAlso String.IsNullOrEmpty(hPartyKey.Value) = True Or hPartyKey.Value.Trim = "0" Then
                        'This code will retreive the address if user directly type the account code
                        ' If hPartyKey.Value = "" Then
                        GetPartyKey()
                        Session("PartyKey") = hPartyKey.Value
                        ' End If

                    End If
                End If

                If Session("ModeType") = "Receipt" Then
                    'Cash/Cheque Receipts
                    PnlPayeeInformation.Visible = False

                    'Page.ClientScript.RegisterStartupScript(GetType(String), "HidePaymentTab", "HidePaymentTab();", True)
                    ScriptManager.RegisterStartupScript(Me, Page.GetType(), "HidePaymentTab", "HidePaymentTab();", True)
                End If

                oWebservice = New NexusProvider.ProviderManager().Provider
                hPartyKey.Value = Session("PartyKey")
                If hPartyKey.Value IsNot Nothing AndAlso hPartyKey.Value.Trim.Length <> 0 AndAlso hPartyKey.Value.Trim <> 0 Then
                    'populate Address Type
                    DisplayAddressInformation()

                    'For Bank Guarantee
                    If GISLookup_ReceiptType.Value = "BGDEPT" AndAlso hiddenAccountKey.Value <> "" Then

                        Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                        Dim oBankGuaranteePolicy As NexusProvider.BankGuaranteePolicy

                        pnlBGDebtDetails.Visible = True

                        oBankGuaranteePolicy = oWebservice.GetPoliciesOnBankGuaranteeForReceipt(CInt(hiddenAccountKey.Value), NexusProvider.BGGetPoliciesActionTypeType.OutStandingPremium, CInt(hPartyKey.Value))
                        grdvBGDebtDetails.DataSource = oBankGuaranteePolicy.PartyBGPolicyDetails
                        grdvBGDebtDetails.DataBind()
                        ViewState("BG") = oBankGuaranteePolicy
                    End If
                End If
                If hdnIsInstalment.Value = "1" Then
                    If Not (String.IsNullOrEmpty(hPartyKey.Value) OrElse hPartyKey.Value.Equals("0")) Then
                        FindPremiumFinancePlans()
                        GetFinancePlanDetails()
                        liTenderedAmount.Visible = True
                    Else
                        InitialiseInstalment()
                    End If
                End If
                Dim changeTab1 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(1) a').tab('show')});"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab1", changeTab1, True)
                'Session("hfActiveTab") = 1
                Session("hfPreviousTab") = 1
            End If

            If (Request("__EVENTARGUMENT") = "Refresh") Then
                Response.Redirect("~/secure/WorkManager.aspx", False)
            End If
            'cleaning up
            oWebservice = Nothing
            'This will populate search account modal 
            If HttpContext.Current.Session.IsCookieless Then
                btnAccount.OnClientClick = "tb_show(null ,'" & System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/FindAccount.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=650' , null);return false;"
            Else
                btnAccount.OnClientClick = "tb_show(null ,'" & System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "/Modal/FindAccount.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=650' , null);return false;"
            End If
            If Request("__EVENTARGUMENT") = "Refresh" Then
                Response.Redirect("~/secure/WorkManager.aspx")
            End If

            If Request("__EVENTARGUMENT") = "WriteOFF" Then
                WriteOff(False)
            End If
            If Request("__EVENTARGUMENT") = "TakeExactAmount" Then
                TakeExactAmount()
            End If
            Dim oTempInstalmentPlanDetailsCollection As New NexusProvider.InstalmentPlanDetailsCollection
            Dim bJumpInToAdd As Boolean = False
            Dim bJumpInToRemove As Boolean = False
            Dim bRecordExist As Boolean = True
            Dim iCount As Integer = 0
            If Request("__EVENTARGUMENT") = "CHECKED" Then
                bJumpInToAdd = True
                bJumpInToRemove = False
            End If

            If Request("__EVENTARGUMENT") = "UNCHECKED" Then
                bJumpInToAdd = False
                bJumpInToRemove = True
            End If

            If Page.IsPostBack OrElse Request.QueryString("Mode") = "AP" Then
                'If Page.IsPostBack Then
                If Session("INSTALMENTPLANDETAILS") IsNot Nothing Then
                    oInstalmentPlanDetailsCollection = Session("INSTALMENTPLANDETAILS")
                    oTempInstalmentPlanDetailsCollection = Session("INSTALMENTPLANDETAILS")
                    For Each oInstPlnDet As NexusProvider.InstalmentPlanDetails In oTempInstalmentPlanDetailsCollection
                        dOverallSelectedAmount = dOverallSelectedAmount + oInstPlnDet.InstalmentDetails.Amount
                    Next
                    txtOverAllSelectedTotal.Text = String.Format("{0:0.00}", dOverallSelectedAmount)

                End If
                'oInstalmentPlanDetailsCollection = New NexusProvider.InstalmentPlanDetailsCollection
                If grdInstallmentQuotes.Rows.Count > 0 AndAlso bJumpInToAdd Then
                    For iCount = 0 To grdInstallmentQuotes.Rows.Count - 1
                        Dim chkSelected As CheckBox
                        chkSelected = DirectCast(grdInstallmentQuotes.Rows(iCount).FindControl("chkSelectedInstalment"), CheckBox)
                        If chkSelected.Checked = True Then
                            If Session("INSTALMENTPLANDETAILS") Is Nothing OrElse DirectCast(Session("INSTALMENTPLANDETAILS"), NexusProvider.InstalmentPlanDetailsCollection).Count = 0 Then
                                Dim oInstalmentPlanDetails As NexusProvider.InstalmentPlanDetails
                                oInstalmentPlanDetails = New NexusProvider.InstalmentPlanDetails
                                oInstalmentPlanDetails.FinancePlanKey = ddlInstalmentPlan.SelectedValue
                                oInstalmentPlanDetails.FinancePlanVersion = ViewState("PlanVersion")
                                oInstalmentPlanDetails.InstalmentDetails = New NexusProvider.Instalment
                                oInstalmentPlanDetails.InstalmentDetails.Amount = grdInstallmentQuotes.Rows(iCount).Cells(4).Text.Trim()
                                oInstalmentPlanDetails.InstalmentDetails.DueDate = grdInstallmentQuotes.Rows(iCount).Cells(2).Text.Trim()
                                oInstalmentPlanDetails.InstalmentDetails.BatchRef = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("BatchRef"))
                                oInstalmentPlanDetails.InstalmentDetails.Commission = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("Commission"))
                                oInstalmentPlanDetails.InstalmentDetails.ExportDate = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("ExportDate"))
                                oInstalmentPlanDetails.InstalmentDetails.Fee = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("Fee"))
                                oInstalmentPlanDetails.InstalmentDetails.InstalmentNumber = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("InstalmentNumber"))
                                oInstalmentPlanDetails.InstalmentDetails.InstalmentReasonCode = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("InstalmentReasonCode"))
                                oInstalmentPlanDetails.InstalmentDetails.PaymentDate = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("PaymentDate"))
                                oInstalmentPlanDetails.InstalmentDetails.PFInstalmentsKey = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("PFInstalmentsKey"))
                                oInstalmentPlanDetails.InstalmentDetails.PFTransactionKey = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("PFTransactionKey"))
                                oInstalmentPlanDetails.InstalmentDetails.PostedDate = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("PostedDate"))
                                oInstalmentPlanDetails.InstalmentDetails.Reason = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("Reason"))
                                oInstalmentPlanDetails.InstalmentDetails.Status = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("Status"))
                                oInstalmentPlanDetails.InstalmentDetails.StatusCode = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("StatusCode"))
                                oInstalmentPlanDetails.InstalmentDetails.StatusDescription = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("StatusDescription"))
                                oInstalmentPlanDetails.InstalmentDetails.Tax = Convert.ToDouble(grdInstallmentQuotes.DataKeys(iCount).Values("Tax"))
                                oInstalmentPlanDetails.InstalmentDetails.TransactionDescription = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("TransactionDescription"))
                                oInstalmentPlanDetailsCollection.Add(oInstalmentPlanDetails)
                                Session("INSTALMENTPLANDETAILS") = oInstalmentPlanDetailsCollection
                                txtOverAllSelectedTotal.Text = String.Format("{0:0.00}", (dOverallSelectedAmount + oInstalmentPlanDetails.InstalmentDetails.Amount))

                                txtAmount.Text = txtOverAllSelectedTotal.Text
                            Else
                                Dim oInstalmentPlanDetails As NexusProvider.InstalmentPlanDetails
                                oInstalmentPlanDetails = New NexusProvider.InstalmentPlanDetails
                                For Each oInstPlnDet As NexusProvider.InstalmentPlanDetails In oTempInstalmentPlanDetailsCollection
                                    If oInstPlnDet.InstalmentDetails IsNot Nothing AndAlso oInstPlnDet.InstalmentDetails.PFInstalmentsKey = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("PFInstalmentsKey")) Then
                                        bRecordExist = True
                                        Exit For
                                    Else
                                        bRecordExist = False
                                    End If
                                Next
                                If Not bRecordExist Then
                                    oInstalmentPlanDetails.FinancePlanKey = ddlInstalmentPlan.SelectedValue
                                    oInstalmentPlanDetails.FinancePlanVersion = ViewState("PlanVersion")
                                    oInstalmentPlanDetails.InstalmentDetails = New NexusProvider.Instalment
                                    oInstalmentPlanDetails.InstalmentDetails.Amount = grdInstallmentQuotes.Rows(iCount).Cells(4).Text.Trim()
                                    oInstalmentPlanDetails.InstalmentDetails.DueDate = grdInstallmentQuotes.Rows(iCount).Cells(2).Text.Trim()
                                    oInstalmentPlanDetails.InstalmentDetails.BatchRef = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("BatchRef"))
                                    oInstalmentPlanDetails.InstalmentDetails.Commission = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("Commission"))
                                    oInstalmentPlanDetails.InstalmentDetails.ExportDate = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("ExportDate"))
                                    oInstalmentPlanDetails.InstalmentDetails.Fee = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("Fee"))
                                    oInstalmentPlanDetails.InstalmentDetails.InstalmentNumber = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("InstalmentNumber"))
                                    oInstalmentPlanDetails.InstalmentDetails.InstalmentReasonCode = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("InstalmentReasonCode"))
                                    oInstalmentPlanDetails.InstalmentDetails.PaymentDate = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("PaymentDate"))
                                    oInstalmentPlanDetails.InstalmentDetails.PFInstalmentsKey = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("PFInstalmentsKey"))
                                    oInstalmentPlanDetails.InstalmentDetails.PFTransactionKey = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("PFTransactionKey"))
                                    oInstalmentPlanDetails.InstalmentDetails.PostedDate = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("PostedDate"))
                                    oInstalmentPlanDetails.InstalmentDetails.Reason = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("Reason"))
                                    oInstalmentPlanDetails.InstalmentDetails.Status = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("Status"))
                                    oInstalmentPlanDetails.InstalmentDetails.StatusCode = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("StatusCode"))
                                    oInstalmentPlanDetails.InstalmentDetails.StatusDescription = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("StatusDescription"))
                                    oInstalmentPlanDetails.InstalmentDetails.Tax = Convert.ToDouble(grdInstallmentQuotes.DataKeys(iCount).Values("Tax"))
                                    oInstalmentPlanDetails.InstalmentDetails.TransactionDescription = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("TransactionDescription"))
                                    txtOverAllSelectedTotal.Text = String.Format("{0:0.00}", (dOverallSelectedAmount + oInstalmentPlanDetails.InstalmentDetails.Amount))

                                    txtAmount.Text = txtOverAllSelectedTotal.Text
                                End If
                                If oInstalmentPlanDetails.FinancePlanKey <> 0 Then
                                    oInstalmentPlanDetailsCollection.Add(oInstalmentPlanDetails)
                                    Session("INSTALMENTPLANDETAILS") = oInstalmentPlanDetailsCollection
                                End If

                            End If

                        End If
                    Next
                ElseIf bJumpInToRemove Then
                    If Session("INSTALMENTPLANDETAILS") IsNot Nothing Then
                        Dim nIndexToRemove As Integer = Nothing
                        oInstalmentPlanDetailsCollection = Session("INSTALMENTPLANDETAILS")
                        'Dim oPolicy = (dele From ps In oInstalmentPlanDetailsCollection Where ps.InstalmentDetails.PFInstalmentsKey = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("PFInstalmentsKey"))  ) Order By ps.quoteversion Descending)
                        For nRecordCount As Integer = 0 To oInstalmentPlanDetailsCollection.Count - 1
                            If oInstalmentPlanDetailsCollection IsNot Nothing AndAlso oInstalmentPlanDetailsCollection(nRecordCount).InstalmentDetails.InstalmentNumber = hdInstalmentNumber.Value AndAlso oInstalmentPlanDetailsCollection(nRecordCount).FinancePlanKey = ddlInstalmentPlan.SelectedValue Then
                                dOverallSelectedAmount = dOverallSelectedAmount - oInstalmentPlanDetailsCollection(nRecordCount).InstalmentDetails.Amount
                                oInstalmentPlanDetailsCollection.Remove(nRecordCount)
                                Exit For
                            End If
                        Next
                        txtOverAllSelectedTotal.Text = String.Format("{0:0.00}", dOverallSelectedAmount)
                        txtAmount.Text = txtOverAllSelectedTotal.Text
                        Session("INSTALMENTPLANDETAILS") = oInstalmentPlanDetailsCollection
                    End If
                End If

            End If
            If Request("__EVENTARGUMENT") = "ContinueAfterMulticurrency" Then
                If Session(CNCashListCurrencyRates) IsNot Nothing Then
                    Dim rates = CType(Session(CNCashListCurrencyRates), Nexus.Constants.Session.CashListCurrencyRates)
                    If Not String.IsNullOrEmpty(CashListItemID.Value) Then
                        ' Editing existing item - target only that specific index
                        If Session("ModeType") = "Payment" Then
                            Dim oCashListItemType = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                            Dim item = oCashListItemType.PaymentItems(CInt(CashListItemID.Value))
                            ApplyRates(item, rates)
                            Session(CNCashListItem) = oCashListItemType
                        Else
                            Dim oReceiptCashListItemType = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                            Dim item = oReceiptCashListItemType.ReceiptItems(CInt(CashListItemID.Value))
                            ApplyRates(item, rates)
                            Session(CNCashListItem) = oReceiptCashListItemType
                        End If
                    End If
                    Session.Remove(CNCashListCurrencyRates)
                End If
            End If
            'End If
            '  End If
            'End If
        End Sub

        Private Sub ManageControlsForInsurerPayments()
            'this will manage the controls for "Insurer Payments - Receipts or Payments or Claim Payment"
            If Not Session(CNCurrency) Is Nothing Then
                Session("Currency") = Session(CNCurrency)
            End If
            ManageControlsDependsonAuthorityLevel() 'This will help to manage the controls depends on Authority level

            Dim oReceiptCashListItem As NexusProvider.ReceiptCashListItemType
            Dim oPaymentCashListItem As NexusProvider.PaymentCashListItemType
            Dim dMarkedAmount As Decimal
            Dim dWriteOffAmount As Decimal
            Dim oCurrency As New NexusProvider.Currency
            Dim dTotalAmount As Decimal

            If Session("Type").Trim() = PaymentType.R.ToString() Then
                'User is doing Insurer Payment - Receipts

                liReceiptType.Visible = True
                GISLookup_ReceiptType.Value = "STD"
                liPaymentType.Visible = False
                PnlPayeeInformation.Visible = False

                'Load the Media Type as per IsReceipt or IsPayment
                FillMediaType()

                'initialise the value from session
                oReceiptCashListItem = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)

                If Not String.IsNullOrEmpty(CashListItemID.Value()) Then
                    Dim RowIndex As Integer = CashListItemID.Value()
                    oCashListItem = oReceiptCashListItem.ReceiptItems(RowIndex)
                End If

                'set the values to the relevant controls
                Cash_List_Item__Transaction_Date.Text = oReceiptCashListItem.CoreCashList.ListDate
                rngTransactionDate.MaximumValue = oReceiptCashListItem.CoreCashList.ListDate

                If oReceiptCashListItem IsNot Nothing And oReceiptCashListItem.ReceiptItems.Count > 0 Then
                    GISLookup_MediaType.SelectedValue = oReceiptCashListItem.ReceiptItems(0).MediaTypeCode.Trim
                End If

                'currency conversion
                If oReceiptCashListItem.CoreCashList.CurrencyCode.Trim.ToUpper <> Session(CNCurrency).ToString.Trim.ToUpper Then
                    'Retreival of BaseAmount i.e. Loss Amount
                    dMarkedAmount = Math.Abs(Session(CNTotalAmount))

                    oCurrency.AccountCode = Session(CNAccountName)
                    oCurrency.TransactionCurrencyCode = oReceiptCashListItem.CoreCashList.CurrencyCode
                    oCurrency.Mode = "ALL"
                    oCurrency = oWebservice.GetCurrencyExchangeRates(oCurrency, Session(CNTransBranchCode))
                    'Calculate the New Total Amount as per the choice
                    dTotalAmount = 0
                    dTotalAmount = Math.Round((dMarkedAmount / oCurrency.TransactionCurrencyRate), 2)

                    txtAmount.Text = dTotalAmount

                ElseIf oReceiptCashListItem.CoreCashList.CurrencyCode.Trim.ToUpper = Session(CNCurrency).ToString.Trim.ToUpper Then
                    dMarkedAmount = Session(CNTotalAmount)
                    dWriteOffAmount = Session(CNTotalWriteOffAmount)

                    If dMarkedAmount < 0 Then
                        txtAmount.Text = dMarkedAmount * -1
                    Else
                        txtAmount.Text = dMarkedAmount
                    End If

                    If dWriteOffAmount <> 0 Then
                        txtWriteOffAmount.Text = dWriteOffAmount
                    Else
                        txtWriteOffAmount.Text = 0
                    End If
                End If

            ElseIf Session("Type").Trim() = PaymentType.P.ToString() Or Session("Type").Trim() = PaymentType.CP.ToString() Then
                'User is doing Insurer Payment - Payments or Claim Payment
                Dim oMultiStepApproval As NexusProvider.OptionTypeSetting = Nothing
                Dim bIsIncludePaymentTypeClaimPayment As Boolean
                liReceiptType.Visible = False
                liPaymentType.Visible = True

                'Load the Media Type as per IsReceipt or IsPayment
                FillMediaType()
                GISLookup_PaymentType.Value = "AGPAY"
                'Checking of Multi Step Approval
                'if it is ON then Pending status is "Pending" otherwise "ISSUED (ISS)"
                oMultiStepApproval = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 65)
                bIsIncludePaymentTypeClaimPayment = IsClaimIncludedInPayment()
                If oMultiStepApproval.OptionValue = "1" And bIsIncludePaymentTypeClaimPayment Then
                    ddlStatus.Value = "PENDING"
                Else
                    ddlStatus.Value = "ISS"
                End If
                'oMultiStepApproval = Nothing

                'Collection Date should NOT visible for Payments or Claim Payment
                liCollectionDate.Visible = False
                rqdCollectionDate.Enabled = False
                'Cash_List_Item__Collection_Date.CssClass = "field-medium"
                rngCollectionDate.Enabled = False
                liComments.Visible = False
                txtAmount.Enabled = False
                'initialise the value from session
                oPaymentCashListItem = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)

                If Not String.IsNullOrEmpty(CashListItemID.Value()) Then
                    Dim RowIndex As Integer = CashListItemID.Value()
                    oCashListItem = oPaymentCashListItem.PaymentItems(RowIndex)
                End If

                'set the values to the relevant controls
                Cash_List_Item__Transaction_Date.Text = oPaymentCashListItem.CoreCashList.ListDate
                rngTransactionDate.MaximumValue = oPaymentCashListItem.CoreCashList.ListDate

                If oPaymentCashListItem IsNot Nothing And oPaymentCashListItem.PaymentItems.Count > 0 Then
                    GISLookup_MediaType.SelectedValue = oPaymentCashListItem.PaymentItems(0).MediaTypeCode.Trim
                End If

                'currency conversion
                If Session(CNTransCurr) IsNot Nothing AndAlso oPaymentCashListItem.CoreCashList.CurrencyCode.Trim.ToUpper <> Session(CNTransCurr).ToString.Trim.ToUpper Then
                    'Retreival of BaseAmount i.e. Loss Amount
                    dMarkedAmount = Math.Abs(Session(CNTotalAmount))
                    dWriteOffAmount = Math.Abs(Session(CNTotalWriteOffAmount))

                    oCurrency.AccountCode = Session(CNAccountName)
                    oCurrency.TransactionCurrencyCode = oPaymentCashListItem.CoreCashList.CurrencyCode
                    oCurrency.Mode = "ALL"
                    oCurrency = oWebservice.GetCurrencyExchangeRates(oCurrency, Session(CNTransBranchCode))
                    'Calculate the New Total Amount as per the choice
                    dTotalAmount = 0
                    dTotalAmount = Math.Round((dMarkedAmount / oCurrency.TransactionCurrencyRate), 2)
                    dWriteOffAmount = Math.Round((dWriteOffAmount / oCurrency.TransactionCurrencyRate), 2)

                    txtAmount.Text = dTotalAmount

                    If dWriteOffAmount <> 0 Then
                        txtWriteOffAmount.Text = dWriteOffAmount
                    Else
                        txtWriteOffAmount.Text = 0
                    End If

                ElseIf oPaymentCashListItem.CoreCashList.CurrencyCode.Trim.ToUpper = Session(CNTransCurr).ToString.Trim.ToUpper Then
                    dMarkedAmount = Session(CNTotalAmount)
                    dWriteOffAmount = Session(CNTotalWriteOffAmount)

                    If dMarkedAmount < 0 Then
                        txtAmount.Text = dMarkedAmount * -1
                    Else
                        txtAmount.Text = dMarkedAmount
                    End If

                    If dWriteOffAmount <> 0 Then
                        txtWriteOffAmount.Text = dWriteOffAmount
                    Else
                        txtWriteOffAmount.Text = 0
                    End If
                End If
            End If

            pnlPayeeInfo.Visible = False

            'Page.ClientScript.RegisterStartupScript(GetType(String), "HidePaymentTab", "HidePaymentTab();", True)
            ScriptManager.RegisterStartupScript(Me, Page.GetType(), "HidePaymentTab", "HidePaymentTab();", True)
            pnlBankInfo.Visible = False
            txtBankReference.Enabled = False

            txtAmount.Enabled = False

            'set the key in text box from session and populate the address
            txtAccount.Text = Session(CNAccountName)
            hPartyKey.Value = Session(CNPartyKey)
            DisplayAddressInformation()


            'cleaning up
            oReceiptCashListItem = Nothing
            oPaymentCashListItem = Nothing

        End Sub

        Private Sub ManageCashChequeListReceiptControls()
            'this mentod will manage the controls for "Cash\Cheque - Receipt or Payment"

            ManageControlsDependsonAuthorityLevel() 'This will help to manage the controls depends on Authority level

            'User should be able to select account for Payments and Receipts both
            btnAccount.Enabled = True
            txtAccount.Enabled = True
            rqdAccount.Enabled = True
            txtAccount.CssClass = txtAccount.CssClass & " field-mandatory"
            rqdAmount.Enabled = True
            txtAmount.CssClass = txtAmount.CssClass & " field-mandatory"
            btnCancel.Attributes.Add("onclick", "javascript:return CancelConfirmation();")

            If Session("ModeType") = "Payment" Then 'Cash/Cheque Payment
                Dim oMultiStepApproval As NexusProvider.OptionTypeSetting = Nothing
                Dim oPaymentCashListItem As New NexusProvider.PaymentCashListItemType
                Dim oGetList As New NexusProvider.LookupList
                Dim bIsIncludePaymentTypeClaimPayment As Boolean
                oPaymentCashListItem = Session(CNCashListItem)
                If oPaymentCashListItem IsNot Nothing Then
                    Cash_List_Item__Transaction_Date.Text = oPaymentCashListItem.CoreCashList.ListDate
                    rngTransactionDate.MaximumValue = oPaymentCashListItem.CoreCashList.ListDate
                End If
                GISLookup_ReceiptType.Value = String.Empty
                'Checking of Multi Step Approval
                'if it is ON then Pending status is "Pending" otherwise "ISSUED (ISS)"
                oMultiStepApproval = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 65)
                bIsIncludePaymentTypeClaimPayment = IsClaimIncludedInPayment()
                If oMultiStepApproval.OptionValue = "1" And bIsIncludePaymentTypeClaimPayment Then
                    ddlStatus.Value = "PENDING"
                Else
                    ddlStatus.Value = "ISS"
                End If
                'oMultiStepApproval = Nothing

                'user is doing cash cheque payment so make Collection Date and Comments invisible
                liCollectionDate.Visible = False
                liComments.Visible = False
                rqdCollectionDate.Enabled = False
                'Cash_List_Item__Collection_Date.CssClass = "field-medium"
                rngCollectionDate.Enabled = False

                liReceiptType.Visible = False
                liPaymentType.Visible = True
                GISLookup_PaymentType.Value = "AGPAY"
                'GISLookup_PaymentType.Enabled = False
            ElseIf Session("ModeType") = "Receipt" Then 'Cash/Cheque Receipt
                Dim oReceiptCashListItem As New NexusProvider.ReceiptCashListItemType
                If Session(CNCashListItem) Is Nothing Then
                    Session.Add(CNCashListItem, New NexusProvider.ReceiptCashListItemType)
                End If
                oReceiptCashListItem = Session(CNCashListItem)

                Cash_List_Item__Transaction_Date.Text = oReceiptCashListItem.CoreCashList.ListDate
                rngTransactionDate.MaximumValue = oReceiptCashListItem.CoreCashList.ListDate
                If Request("__EVENTARGUMENT") = "EditClick" Or Request("__EVENTARGUMENT") = "ViewClick" Then
                    If GISLookup_ReceiptType.Value = "INST" AndAlso oReceiptCashListItem IsNot Nothing Then
                        If oReceiptCashListItem.InstalmentPlanCollection.Count > 0 Then
                            ddlInstalmentPlan.SelectedValue = oReceiptCashListItem.InstalmentPlanCollection(0).FinancePlanKey.ToString()
                        End If
                    End If
                End If
                'GISLookup_ReceiptType.Value = "STD"

                pnlPayeeInfo.Visible = False
                'Page.ClientScript.RegisterStartupScript(GetType(String), "HidePaymentTab", "HidePaymentTab();", True)
                ScriptManager.RegisterStartupScript(Me, Page.GetType(), "HidePaymentTab", "HidePaymentTab();", True)
                PnlPayeeInformation.Visible = False
                pnlBankInfo.Visible = False

                rqdCollectionDate.Enabled = True
                Cash_List_Item__Collection_Date.CssClass = Cash_List_Item__Collection_Date.CssClass & " field-mandatory"

                rqdTransactionDate.Enabled = True
                Cash_List_Item__Transaction_Date.CssClass = Cash_List_Item__Transaction_Date.CssClass & " field-mandatory"
                rqdMediaType.Enabled = True
                GISLookup_MediaType.CssClass = GISLookup_MediaType.CssClass & " field-mandatory"
                rqdMediaReference.Enabled = False
                'txtMediaReference.CssClass = "field-medium"

                liReceiptType.Visible = True
                liPaymentType.Visible = False



            End If
            If Session("AddMoreCashList") = "Yes" AndAlso Session("CashListItemFirstLoad") = True Then
                GISLookup_MediaType.SelectedIndex = 0
                CashListItem_Receipt_Cheque.Visible = False

            End If
        End Sub



        Private Sub ManageControlsForMTACancellation()
            'this mentod will manage the controls for "MTA Cancellation"
            Dim oPaymentCashListItem As NexusProvider.PaymentCashListItemType
            Dim dAmountToPay As Decimal

            ManageControlsDependsonAuthorityLevel() 'This will help to manage the controls depends on Authority level

            liReceiptType.Visible = False
            liPaymentType.Visible = True

            'Load the Media Type as per IsReceipt or IsPayment
            FillMediaType()

            ddlStatus.Value = "ISS"

            'Set Payment Type to REFUND as this is MTA Cancellation
            GISLookup_PaymentType.Value = "REFUND"
            GISLookup_PaymentType.Enabled = False

            'Collection Date should NOT visible for Payments or Claim Payment
            liCollectionDate.Visible = False
            rqdCollectionDate.Enabled = False
            rngCollectionDate.Enabled = False
            liComments.Visible = False

            'initialise the value from session
            oPaymentCashListItem = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)

            If Not String.IsNullOrEmpty(CashListItemID.Value()) Then
                Dim RowIndex As Integer = CashListItemID.Value()
                oCashListItem = oPaymentCashListItem.PaymentItems(RowIndex)
            End If

            'set the values to the relevant controls
            Cash_List_Item__Transaction_Date.Text = oPaymentCashListItem.CoreCashList.ListDate
            rngTransactionDate.MaximumValue = oPaymentCashListItem.CoreCashList.ListDate

            'dAmountToPay = Session(CNAmountToPay)
            dAmountToPay = Math.Round(Convert.ToDecimal(Session(CNAmountToPay)), 2)

            If dAmountToPay < 0 Then
                txtAmount.Text = dAmountToPay * -1
            Else
                txtAmount.Text = dAmountToPay
            End If

            'pnlPayeeInfo.Visible = False
            'pnlBankInfo.Visible = False

            txtAmount.Enabled = False
            txtAccount.Enabled = False

            'set the key in text box from session and populate the address
            txtAccount.Text = CType(Session(CNQuote), NexusProvider.Quote).ClientCode.ToUpper()
            txtOurReference.Text = CType(Session(CNQuote), NexusProvider.Quote).InsuranceFileRef.ToUpper()
            txtTheirReference.Text = CType(Session(CNQuote), NexusProvider.Quote).InsuranceFileRef.ToUpper()
            DisplayAddressInformation()

            'cleaning up
            oPaymentCashListItem = Nothing

        End Sub

        Private Sub ManageControlsDependsonAuthorityLevel()
            'This method will check for user Authority and manage the "Comments" and others control

            Dim oUserAuthority As New NexusProvider.UserAuthority

            'Check the Authority  to override the collection date
            oUserAuthority.UserCode = Session(CNLoginName) 'Login Name
            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.CanOverrideCollectionDate

            'call SAM Method to check the authority
            oWebservice = New NexusProvider.ProviderManager().Provider
            oWebservice.GetUserAuthorityValue(oUserAuthority)


            If Not String.IsNullOrEmpty(oUserAuthority.UserAuthorityValue) AndAlso CBool(oUserAuthority.UserAuthorityValue) Then 'Check for user permission to change Collection Date

                Cash_List_Item__Collection_Date.Enabled = True
                CollectionDate_CalenderLookup.Enabled = True
                CollectionDate_CalenderLookup.HLevel = "1"
                txtComments.Enabled = True

                rqdCollectionDate.Enabled = True
                Cash_List_Item__Collection_Date.CssClass = Cash_List_Item__Collection_Date.CssClass & " field-mandatory"
                rngCollectionDate.Enabled = True
                custvldComments.Enabled = True
            End If

            If (Session(CNPayNowReceipt) IsNot Nothing And Session("ModeValue") Is Nothing _
                                               And Session("ModeType") Is Nothing And Session(CNClaim) Is Nothing _
                                               And Session(CNTotalForQuoteCollection) Is Nothing) Or
                Session("ModeType") = "Receipt" Or
                 Session("Type") IsNot Nothing AndAlso Session("Type").Trim() = PaymentType.R.ToString() Then

                'Checking of the Pre-Payments setting
                Dim oOptionType As New NexusProvider.OptionTypeSetting
                oOptionType = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 87)
                If oOptionType.OptionValue = "1" Then 'Pre-Payments is ON
                    liCollectionDate.Visible = True
                Else
                    liCollectionDate.Visible = False
                    Cash_List_Item__Collection_Date.Enabled = False
                    CollectionDate_CalenderLookup.Enabled = False
                    rqdCollectionDate.Enabled = False
                    'Cash_List_Item__Collection_Date.CssClass = "field-medium"
                    rngCollectionDate.Enabled = False
                    custvldComments.Enabled = False
                End If
            End If

            'cleaning up
            oUserAuthority = Nothing
        End Sub

        Public Sub DisplayControls()
            'This method will populate the default values as well as enable/disable the controls

            ManageControlsDependsonAuthorityLevel() 'This will help to manage the controls depends on Authority level

            If Session(CNTotalForQuoteCollection) IsNot Nothing Then

                FillMediaType()
                'User is doing the Quote Collection Process
                Dim oQuote As NexusProvider.Quote
                Dim arrQuoteCollectionFiles As New ArrayList
                Dim iInsuranceFileKey As Integer

                arrQuoteCollectionFiles = Session(CNQuoteCollectionFiles)
                iInsuranceFileKey = arrQuoteCollectionFiles(0)

                'make SAM call to update the Quote in session
                oQuote = oWebservice.GetHeaderAndSummariesByKey(iInsuranceFileKey)
                Session(CNQuote) = oQuote

                DisplayAccountInformation_QuoteCollectionProcess() 'set the Account Information and make enable\disable

                DisplayAddressInformation() 'set the addrress for the selected agent or party
                ManageControlsForQuoteCollectionProcess() 'this will manage the controls need to displayed and hide

                'cleaning up
                oQuote = Nothing
                arrQuoteCollectionFiles = Nothing
                iInsuranceFileKey = Nothing

            ElseIf Session(CNPayNowReceipt) IsNot Nothing And Session("ModeValue") Is Nothing _
                                    And Session("ModeType") Is Nothing And Session(CNClaim) Is Nothing _
                                    And Session(CNTotalForQuoteCollection) Is Nothing _
                                    And Session(CNUnAllocatedClaimPayment) Is Nothing Then
                'New Business\MTA\Renewals

                If Session(CNQuote) IsNot Nothing Then
                    FillMediaType()

                    DisplayAccountInformation_NewBusiness() 'set the Account Information and make enable\disable

                    DisplayAddressInformation() 'set the addrress for the selected agent or party


                    ManageControlsForNewBusiness() 'this will manage the controls need to displayed and hide
                End If

            ElseIf Session(CNClaim) IsNot Nothing And Session("ModeType") Is Nothing And CType(Session(CNMode), Mode) = Mode.PayClaim Then   'Claim Payments
                'User is doing Pay claim
                Dim oPayment As NexusProvider.ClaimPayment = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).Payment
                Dim oClaimReserve As NexusProvider.ClaimPerilReservePaymentTypeCollection = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).ClaimReserve
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)

                Dim oMultiStepApproval As NexusProvider.OptionTypeSetting = Nothing
                Dim oCurrency As New NexusProvider.Currency
                Dim bFound As Boolean = False
                Dim dPaymentAmount, dTotalAmount, dBaseAmount As Decimal
                Dim sPaymentCurrency As String = Nothing
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

                Dim sIsPaymentsReadOnly As String
                sIsPaymentsReadOnly = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsPaymentsReadOnly, NexusProvider.RiskTypeOptions.None, Session(CNProductCode), Nothing)


                If sIsPaymentsReadOnly = "1" Then
                    dPaymentAmount = GetScriptPerilPaidAmount(sPaymentCurrency, True)
                ElseIf oClaimReserve IsNot Nothing Then
                    For iCount As Integer = 0 To oPayment.ClaimPaymentItem.Count - 1
                        If hIsGrossClaimPaymentAmount.Value = "1" Then
                            dPaymentAmount += oPayment.ClaimPaymentItem(iCount).PaymentAmount
                        Else
                            dPaymentAmount += oPayment.ClaimPaymentItem(iCount).PaymentAmount + oPayment.ClaimPaymentItem(iCount).TaxAmount
                        End If
                        'dPaymentAmount += oPayment.ClaimPaymentItem(iCount).PaymentAmount

                        If oPayment.ClaimPaymentItem(iCount).PayQueue > 0 Then
                            sPaymentCurrency = oPayment.ClaimPaymentItem(iCount).CurrencyCode.Trim
                        End If
                    Next
                End If
                dTotalAmount = dPaymentAmount

                'Setting of the Account Code
                txtAccount.Text = oPayment.PartyPaidCode

                'Checking of Multi Step Approval
                'if it is ON then Pending status is "Pending" otherwise "ISSUED (ISS)"
                oMultiStepApproval = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 65)
                bIsIncludePaymentTypeClaimPayment = IsClaimIncludedInPayment()
                If oMultiStepApproval.OptionValue = "1" And bIsIncludePaymentTypeClaimPayment Then
                    ddlStatus.Value = "PENDING"
                Else
                    ddlStatus.Value = "ISS"
                End If

                'Need to trap the currency code and run the SAM method to retreive back the exchange rate
                'Check whether Payment Currency and Selcted Currency at cashlist is same or not
                'This commented code wil be required for PN 67759
                If sIsPaymentsReadOnly = "0" AndAlso oPayment.CashList.CurrencyCode.Trim.ToUpper = Session(CNCurrenyCode).ToString.Trim.ToUpper Then
                    dTotalAmount = 0
                    For iCount As Integer = 0 To oPayment.ClaimPaymentItem.Count - 1
                        If hIsGrossClaimPaymentAmount.Value = "1" Then
                            dTotalAmount += oPayment.ClaimPaymentItem(iCount).LossPaymentAmount
                        Else
                            dTotalAmount += oPayment.ClaimPaymentItem(iCount).LossPaymentAmount + oPayment.ClaimPaymentItem(iCount).TaxAmount
                        End If
                        'dTotalAmount += oPayment.ClaimPaymentItem(iCount).LossPaymentAmount
                    Next
                End If

                'Payment currency and selected currency at cashlist is not same
                'then check whether it is similar to the Policy currency
                'if it is the similar to the policy currenct then display the converted amount otherwise we need to convert it
                If oPayment.CashList.CurrencyCode.Trim.ToUpper <> Session(CNCurrenyCode).ToString.Trim.ToUpper _
                AndAlso oPayment.CashList.CurrencyCode.Trim.ToUpper <> sPaymentCurrency.Trim.ToUpper Then
                    'Retreival of BaseAmount i.e. Loss Amount
                    dBaseAmount = 0
                    If sIsPaymentsReadOnly = "0" Then
                        For iCount As Integer = 0 To oPayment.ClaimPaymentItem.Count - 1
                            dBaseAmount += oPayment.ClaimPaymentItem(iCount).LossPaymentAmount
                        Next
                    Else
                        dBaseAmount = dTotalAmount
                    End If
                    oCurrency.AccountCode = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen).ClientShortName
                    oCurrency.TransactionCurrencyCode = oPayment.CashList.CurrencyCode
                    oCurrency.Mode = "ALL"
                    oCurrency = oWebservice.GetCurrencyExchangeRates(oCurrency, oQuote.BranchCode)
                    'Calculate the New Total Amount as per the choice
                    dTotalAmount = 0
                    dTotalAmount = Math.Round((dBaseAmount / oCurrency.TransactionCurrencyRate), 2)
                End If

                'Negation to Positive, if any
                If dTotalAmount < 0 Then
                    txtAmount.Text = dTotalAmount * -1
                Else
                    txtAmount.Text = dTotalAmount
                End If

                ManageControlsForPayClaim() 'will manage the controls during Pay Claim Process

                DisplayAddressInformation() 'set the addrress for the selected agent or party
                If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ShowSummary = False Then
                    SkipSummaryPage()
                End If

            ElseIf Session(CNUnAllocatedClaimPayment) IsNot Nothing Or Session(CNMode) = Mode.Recommend Then 'Authorize Claim Payments/ Claim Payments Processing
                'Authorize Claim Payments/ Claim Payments Processing

                'Claim Payments Processing

                ManageControlForClaimPaymentProcessing() 'this will manage the controls need to displayed and hide

                DisplayAccountInformation_ClaimPayProcessing() 'set the Account Information and make enable\disable
            ElseIf Session("ModeValue") = "INS" Then
                Dim oFinancePlanDetails As New NexusProvider.FinancePlanDetails
                If Session(CNFinancePlanDetails) IsNot Nothing Then
                    oFinancePlanDetails = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PremiumFinanceDetails
                    txtTotalAmount.Visible = True
                    lblTotalAmount.Visible = True
                    rvtxtTotalAmount.Enabled = True
                    txtAccount.Text = oFinancePlanDetails.ClientCode
                    txtAmount.Text = oFinancePlanDetails.SettlementAmount
                    txtTotalAmount.Text = txtAmount.Text
                End If
            ElseIf Session("ModeValue") = "INSDEPOSIT" Then
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim oAccountColl As NexusProvider.AccountSearchResultCollection
                Dim oAccountSearchCr As New NexusProvider.AccountSearchCriteria
                Dim oReceiptCashListItem As NexusProvider.ReceiptCashListItemType
                oReceiptCashListItem = Session(CNCashListItem)
                Dim oCurrency As New NexusProvider.Currency
                ' oQuote.AgentCode = "0001"
                If oQuote.BusinessTypeCode = "DIRECT" Then 'Direct Business/Customer
                    oAccountSearchCr.ShortCode = oQuote.ClientCode
                Else
                    oAccountSearchCr.ShortCode = oQuote.AgentCode
                End If
                oAccountColl = oWebservice.FindAccounts(oAccountSearchCr)
                txtTotalAmount.Visible = True
                lblTotalAmount.Visible = True
                rvtxtTotalAmount.Enabled = True
                txtAccount.Text = oAccountColl(0).ShortCode
                If oReceiptCashListItem IsNot Nothing AndAlso oReceiptCashListItem.CoreCashList IsNot Nothing AndAlso oQuote IsNot Nothing AndAlso oReceiptCashListItem.CoreCashList.CurrencyCode.ToUpper() <> oQuote.CurrencyCode.ToUpper() Then
                    oCurrency.TransactionCurrencyCode = oReceiptCashListItem.CoreCashList.CurrencyCode
                    oCurrency.Mode = "ALL"
                    oCurrency = oWebservice.GetCurrencyExchangeRates(oCurrency, oQuote.BranchCode)
                    oQuote.InstDepositAmount = Math.Round((oQuote.InstDepositAmount / oCurrency.TransactionCurrencyRate), 2)
                End If
                txtAmount.Text = oQuote.InstDepositAmount
                txtTotalAmount.Text = txtAmount.Text

                DisplayAddressInformation() 'set the addrress for the selected agent or party
            ElseIf Session("ModeType") IsNot Nothing Then
                If Session("ModeType") = "Receipt" Then
                    'Page.ClientScript.RegisterStartupScript(GetType(String), "HidePaymentTab", "HidePaymentTab();", True)
                    ScriptManager.RegisterStartupScript(Me, Page.GetType(), "HidePaymentTab", "HidePaymentTab();", True)
                ElseIf Session("ModeType") = "Payment" Then

                End If
            End If

        End Sub

        Private Sub ManageControlsForQuoteCollectionProcess()
            'will manage the controls during Quote Collection Process

            GISLookup_ReceiptType.Value = "STD"
            GISLookup_ReceiptType.Enabled = False

            'Value for GISLookup_MediaType should be defaulted to "CASH"
            GISLookup_MediaType.SelectedValue = "CA"
            pnlPayeeInfo.Visible = False
            'Page.ClientScript.RegisterStartupScript(GetType(String), "HidePaymentTab", "HidePaymentTab();", True)
            ScriptManager.RegisterStartupScript(Me, Page.GetType(), "HidePaymentTab", "HidePaymentTab();", True)
            PnlPayeeInformation.Visible = False
            pnlBankInfo.Visible = False
            rqdAccount.Enabled = True
            txtAccount.CssClass = txtAccount.CssClass & " field-mandatory"
            rqdAmount.Enabled = True
            txtAmount.CssClass = txtAmount.CssClass & " field-mandatory"
            rqdCollectionDate.Enabled = True
            Cash_List_Item__Collection_Date.CssClass = Cash_List_Item__Collection_Date.CssClass & " field-mandatory"
            rqdTransactionDate.Enabled = True
            Cash_List_Item__Transaction_Date.CssClass = Cash_List_Item__Transaction_Date.CssClass & " field-mandatory"
            rqdMediaType.Enabled = True
            GISLookup_MediaType.CssClass = GISLookup_MediaType.CssClass & " field-mandatory"
            rqdMediaReference.Enabled = False
            'txtMediaReference.CssClass = "field-medium"

            'Populate the Total Amount for all the Quotes selected trhough Quote COllection
            txtAmount.Text = Session(CNTotalForQuoteCollection)

            txtAmount.Attributes.Add("onblur", "CheckAmount()")
            liTenderedAmount.Visible = True
            txtTendered.Text = txtAmount.Text
            txtTendered.Attributes.Add("onblur", "CheckTenderedAmount('" + GetLocalResourceObject("lbl_TenderAmtErrorMsg") + "');")
            liChangeAmount.Visible = True
            txtChange.Text = "0.0"
        End Sub

        Private Sub ManageControlsForNewBusiness()
            'will manage the controls during NB/MTA/Renewals
            Dim oPayNowReceipt As NexusProvider.AddPayNowReceipt = CType(Session(CNPayNowReceipt), NexusProvider.AddPayNowReceipt)
            Dim oQuote As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)

            Dim oCurrency As New NexusProvider.Currency
            GISLookup_ReceiptType.Value = "STD"
            GISLookup_ReceiptType.Enabled = False




            'Value for GISLookup_MediaType should be defaulted to "CASH"
            'GISLookup_MediaType.SelectedValue = "CA"
            If GISLookup_MediaType.Items IsNot Nothing AndAlso GISLookup_MediaType.Items.Count > 0 Then
                For iCount As Integer = 0 To GISLookup_MediaType.Items.Count - 1
                    If GISLookup_MediaType.Items(iCount).Value.Trim.ToUpper = "CA" Then
                        GISLookup_MediaType.SelectedIndex = iCount
                    End If
                Next
            End If

            txtAmount.Attributes.Add("onblur", "CheckAmount()")
            liTenderedAmount.Visible = True
            txtTendered.Text = txtAmount.Text
            txtTendered.Attributes.Add("onblur", "CheckTenderedAmount('" + GetLocalResourceObject("lbl_TenderAmtErrorMsg") + "');")
            liChangeAmount.Visible = True
            txtChange.Text = "0.0"
            pnlPayeeInfo.Visible = False
            'Page.ClientScript.RegisterStartupScript(GetType(String), "HidePaymentTab", "HidePaymentTab();", True)
            If (Session("ModeValue") = "PayNow" AndAlso Session("ModeType") <> "Payment") Then
                ScriptManager.RegisterStartupScript(Me, Page.GetType(), "HidePaymentTab", "HidePaymentTab();", True)
            End If
            PnlPayeeInformation.Visible = False
            pnlBankInfo.Visible = False
            rqdAccount.Enabled = True
            txtAccount.CssClass = txtAccount.CssClass & " field-mandatory"
            rqdAmount.Enabled = True
            txtAmount.CssClass = txtAmount.CssClass & " field-mandatory"
            rqdCollectionDate.Enabled = True
            Cash_List_Item__Collection_Date.CssClass = Cash_List_Item__Collection_Date.CssClass & " field-mandatory"
            rqdTransactionDate.Enabled = True
            Cash_List_Item__Transaction_Date.CssClass = Cash_List_Item__Transaction_Date.CssClass & " field-mandatory"
            rqdMediaType.Enabled = True
            GISLookup_MediaType.CssClass = GISLookup_MediaType.CssClass & " field-mandatory"
            rqdMediaReference.Enabled = False
            'txtMediaReference.CssClass = "field-medium"

            If (Request.QueryString("Mode") = "PayNow") Then
                'Cash_List_Item__Collection_Date.Visible = False
                liCollectionDate.Visible = False
            End If

            'currency Conversion
            If oPayNowReceipt IsNot Nothing AndAlso oQuote IsNot Nothing AndAlso oPayNowReceipt.Receipt.CurrencyCode.ToUpper() <> oQuote.CurrencyCode.ToUpper() Then
                oCurrency.TransactionCurrencyCode = oPayNowReceipt.Receipt.CurrencyCode
                oCurrency.Mode = "ALL"
                oCurrency = oWebservice.GetCurrencyExchangeRates(oCurrency, oQuote.BranchCode)
                Session(CNAmountToPay) = Math.Round((Session(CNAmountToPay) / oCurrency.TransactionCurrencyRate), 2)
            End If
            'populate the Amount from session
            If CDec(Session(CNAmountToPay)) < 0 Then
                'Set Payment Type to REFUND as return premium
                liReceiptType.Visible = False
                liPaymentType.Visible = True
                GISLookup_PaymentType.Value = "REFUND"
                GISLookup_PaymentType.Enabled = False
                ' txtAmount.Text = CDec(Session(CNAmountToPay)) * -1
                txtAmount.Text = Math.Round(Convert.ToDecimal(Session(CNAmountToPay)), 2) * -1
                ' txtTendered.Text = CDec(Session(CNAmountToPay)) * -1
                txtTendered.Text = Math.Round(Convert.ToDecimal(Session(CNAmountToPay)), 2) * -1
            Else
                ' txtAmount.Text = CDec(Session(CNAmountToPay))
                txtAmount.Text = Math.Round(Convert.ToDecimal(Session(CNAmountToPay)), 2)
                'txtTendered.Text = CDec(Session(CNAmountToPay))
                txtTendered.Text = Format(Math.Round(CDbl(Session(CNAmountToPay)), 2), "#0.00")
            End If

            'set the Transaction Date using session

            Cash_List_Item__Transaction_Date.Text = oPayNowReceipt.Receipt.ListDate.ToShortDateString()

            'cleaning up
            oPayNowReceipt = Nothing

        End Sub

        Private Sub DisplayAccountInformation_NewBusiness()
            'set the Account Information and make enable\disable, User is doing NB, MTA or Renewals

            Dim oQuote As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)
            Dim oParty As NexusProvider.BaseParty = CType(Session(CNParty), NexusProvider.BaseParty)

            If (Session(CNAgentType) IsNot Nothing AndAlso Session(CNAgentType).ToString = "Commission Account") Or oQuote.BusinessTypeCode = "DIRECT" Then
                'If AgentType is "Commission Account" 
                'or User is doing DirectBusiness\DirectCustomer then display Client short name NOT Agent short name
                Select Case True
                    Case TypeOf oParty Is NexusProvider.CorporateParty
                        If String.IsNullOrEmpty(CType(oParty, NexusProvider.CorporateParty).ClientSharedData.ShortName) = False Then
                            txtAccount.Text = CType(oParty, NexusProvider.CorporateParty).ClientSharedData.ShortName.Trim
                        ElseIf String.IsNullOrEmpty(CType(oParty, NexusProvider.CorporateParty).UserName) = False Then
                            txtAccount.Text = CType(oParty, NexusProvider.CorporateParty).UserName.Trim
                        End If
                    Case TypeOf oParty Is NexusProvider.PersonalParty
                        If String.IsNullOrEmpty(CType(oParty, NexusProvider.PersonalParty).ClientSharedData.ShortName) = False Then
                            txtAccount.Text = CType(oParty, NexusProvider.PersonalParty).ClientSharedData.ShortName.Trim
                        ElseIf String.IsNullOrEmpty(CType(oParty, NexusProvider.PersonalParty).UserName) = False Then
                            txtAccount.Text = CType(oParty, NexusProvider.PersonalParty).UserName.Trim
                        End If
                End Select
                hPartyKey.Value = oQuote.PartyKey
            ElseIf Session(CNAgentType) IsNot Nothing AndAlso Session(CNAgentType).ToString = "Intermediary" Then
                'If AgentType is "Intermediary Account" 
                'or User is doing DirectBusiness\DirectCustomer then display Client short name NOT Agent short name
                Select Case True
                    Case TypeOf oParty Is NexusProvider.CorporateParty
                        ' txtAccount.Text = CType(oParty, NexusProvider.CorporateParty).ClientSharedData.ShortName.Trim
                        If String.IsNullOrEmpty(CType(oParty, NexusProvider.CorporateParty).ClientSharedData.ShortName) = False Then
                            txtAccount.Text = CType(oParty, NexusProvider.CorporateParty).ClientSharedData.ShortName.Trim
                        ElseIf String.IsNullOrEmpty(CType(oParty, NexusProvider.CorporateParty).UserName) = False Then
                            txtAccount.Text = CType(oParty, NexusProvider.CorporateParty).UserName.Trim
                        End If
                    Case TypeOf oParty Is NexusProvider.PersonalParty
                        ' txtAccount.Text = CType(oParty, NexusProvider.PersonalParty).ClientSharedData.ShortName.Trim
                        If String.IsNullOrEmpty(CType(oParty, NexusProvider.PersonalParty).ClientSharedData.ShortName) = False Then
                            txtAccount.Text = CType(oParty, NexusProvider.PersonalParty).ClientSharedData.ShortName.Trim
                        ElseIf String.IsNullOrEmpty(CType(oParty, NexusProvider.PersonalParty).UserName) = False Then
                            txtAccount.Text = CType(oParty, NexusProvider.PersonalParty).UserName.Trim
                        End If
                End Select
                hPartyKey.Value = oQuote.PartyKey

                'User should be able to change the Account field only for Intermediary Agent Types
                btnAccount.Enabled = True
                txtAccount.Enabled = True
            Else
                'for Agency Business
                'display Agent short name
                If String.IsNullOrEmpty(oQuote.AgentCode) = False Then
                    txtAccount.Text = oQuote.AgentCode
                ElseIf String.IsNullOrEmpty(oQuote.AgentCode) = True AndAlso oQuote.InsuranceFileKey > 0 Then
                    txtAccount.Text = RetreiveAgentCode()
                End If
                hPartyKey.Value = oQuote.Agent
            End If

            txtName.Text = oQuote.InsuredName

            'cleaning up
            oQuote = Nothing
            oParty = Nothing
        End Sub

        Private Sub DisplayAccountInformation_MTC()

            'Similar to spu_GetAccountIDfromInsuranceFileCnt in BO
            Dim oQuote As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)
            Dim oParty As NexusProvider.BaseParty = CType(Session(CNParty), NexusProvider.BaseParty)

            If (Session(CNAgentType) IsNot Nothing AndAlso Session(CNAgentType).ToString = "Commission Account") Or oQuote.BusinessTypeCode = "DIRECT" Then
                Select Case True
                    Case TypeOf oParty Is NexusProvider.CorporateParty
                        If String.IsNullOrEmpty(CType(oParty, NexusProvider.CorporateParty).ClientSharedData.ShortName) = False Then
                            txtAccount.Text = CType(oParty, NexusProvider.CorporateParty).ClientSharedData.ShortName.Trim
                        ElseIf String.IsNullOrEmpty(CType(oParty, NexusProvider.CorporateParty).UserName) = False Then
                            txtAccount.Text = CType(oParty, NexusProvider.CorporateParty).UserName.Trim
                        End If
                    Case TypeOf oParty Is NexusProvider.PersonalParty
                        If String.IsNullOrEmpty(CType(oParty, NexusProvider.PersonalParty).ClientSharedData.ShortName) = False Then
                            txtAccount.Text = CType(oParty, NexusProvider.PersonalParty).ClientSharedData.ShortName.Trim
                        ElseIf String.IsNullOrEmpty(CType(oParty, NexusProvider.PersonalParty).UserName) = False Then
                            txtAccount.Text = CType(oParty, NexusProvider.PersonalParty).UserName.Trim
                        End If
                End Select
                hPartyKey.Value = oQuote.PartyKey
            ElseIf Session(CNAgentType) IsNot Nothing Then
                If String.IsNullOrEmpty(oQuote.AgentCode) = False Then
                    txtAccount.Text = oQuote.AgentCode
                ElseIf String.IsNullOrEmpty(oQuote.AgentCode) = True AndAlso oQuote.InsuranceFileKey > 0 Then
                    txtAccount.Text = RetreiveAgentCode()
                End If
                hPartyKey.Value = oQuote.Agent
            End If

            txtName.Text = oQuote.InsuredName

            oQuote = Nothing
            oParty = Nothing
        End Sub


        Function RetreiveAgentCode() As String
            Dim sAgentCode As String = Nothing
            Dim oAgentDetailsPolicy As NexusProvider.AgentDetailsForPolicy
            Dim oQuote As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)

            'make SAM call to retreive the agent details, if missing
            oWebservice = New NexusProvider.ProviderManager().Provider
            oAgentDetailsPolicy = oWebservice.GetAgentDetailsForPolicy(oQuote.InsuranceFileKey)

            If oAgentDetailsPolicy IsNot Nothing Then
                sAgentCode = oAgentDetailsPolicy.Shortname.Trim
                oQuote.AgentCode = oAgentDetailsPolicy.Shortname.Trim

                Session(CNQuote) = oQuote
            End If

            'cleaning up
            oAgentDetailsPolicy = Nothing

            Return sAgentCode
        End Function
        Private Sub DisplayAccountInformation_QuoteCollectionProcess()
            'set the Account Information and make enable\disable, User is coming through Quote Collection Process

            Dim oQuote As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)

            If (Session(CNAgentType) IsNot Nothing AndAlso Session(CNAgentType).ToString = "Commission Account") Or oQuote.BusinessTypeCode = "DIRECT" Then
                'If AgentType is "Commission Account" 
                'or User is doing DirectBusiness\DirectCustomer then display Client short name NOT Agent short name

                txtAccount.Text = GetPartyCode() 'need to get the PartyShortName(PartyCode) using SAM call

                hPartyKey.Value = oQuote.PartyKey
            Else
                'for Agency Business
                Dim sAgentType As String = Nothing

                sAgentType = GetTypeOfAgent() 'need to get the Agent Type using SAM call

                If sAgentType = "Intermediary" Then
                    'User should be able to change the Account field only for Intermediary Agent Types
                    btnAccount.Enabled = True
                    txtAccount.Enabled = True
                End If

                'display Agent short name
                If String.IsNullOrEmpty(oQuote.AgentCode) = False Then
                    txtAccount.Text = oQuote.AgentCode
                ElseIf String.IsNullOrEmpty(oQuote.AgentCode) = True AndAlso oQuote.InsuranceFileKey > 0 Then
                    txtAccount.Text = RetreiveAgentCode()
                End If
                hPartyKey.Value = oQuote.Agent
            End If

            txtName.Text = oQuote.InsuredName

        End Sub

        Private Function GetPartyCode() As String
            'returns the PartyShortName(PartyCode) for current selected quote
            Dim oQuote As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)
            Dim oTempParty As New NexusProvider.BaseParty
            Dim sPartyShortName As String = Nothing

            oWebservice = New NexusProvider.ProviderManager().Provider
            oTempParty = oWebservice.GetParty(oQuote.PartyKey)

            Select Case True
                Case TypeOf oTempParty Is NexusProvider.CorporateParty
                    '    sPartyShortName = CType(oTempParty, NexusProvider.CorporateParty).ClientSharedData.ShortName.Trim
                    If String.IsNullOrEmpty(CType(oTempParty, NexusProvider.CorporateParty).ClientSharedData.ShortName) = False Then
                        sPartyShortName = CType(oTempParty, NexusProvider.CorporateParty).ClientSharedData.ShortName.Trim
                    ElseIf String.IsNullOrEmpty(CType(oTempParty, NexusProvider.CorporateParty).UserName) = False Then
                        sPartyShortName = CType(oTempParty, NexusProvider.CorporateParty).UserName.Trim
                    End If
                Case TypeOf oTempParty Is NexusProvider.PersonalParty
                    ' sPartyShortName = CType(oTempParty, NexusProvider.PersonalParty).ClientSharedData.ShortName.Trim
                    If String.IsNullOrEmpty(CType(oTempParty, NexusProvider.PersonalParty).ClientSharedData.ShortName) = False Then
                        sPartyShortName = CType(oTempParty, NexusProvider.PersonalParty).ClientSharedData.ShortName.Trim
                    ElseIf String.IsNullOrEmpty(CType(oTempParty, NexusProvider.PersonalParty).UserName) = False Then
                        sPartyShortName = CType(oTempParty, NexusProvider.PersonalParty).UserName.Trim
                    End If
            End Select

            'cleaning up
            oWebservice = Nothing
            oQuote = Nothing
            oTempParty = Nothing

            ViewState("PartyName") = sPartyShortName ' stored in viewstate to use in custom validator custvldSelectOneoftheAccount
            Return sPartyShortName

        End Function

        Private Function GetTypeOfAgent() As String
            'returns the TypeOfAgent for current selected quote
            Dim oTempParty As NexusProvider.PartyCollection
            Dim oTempSearchCriteria As New NexusProvider.PartySearchCriteria
            Dim sAgentType As String = Nothing

            oTempSearchCriteria.AgentType = Nothing
            oTempSearchCriteria.ShortName = CType(Session(CNQuote), NexusProvider.Quote).AgentCode
            oTempSearchCriteria.PartyType = CType(Session(CNAgentDetails), NexusProvider.UserDetails).PartyType
            oTempSearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.AG)

            'Find The AgentType through SAM Call
            oWebservice = New NexusProvider.ProviderManager().Provider
            oTempParty = oWebservice.FindParty(oTempSearchCriteria)

            If oTempParty IsNot Nothing Then
                If oTempParty.Count > 0 Then
                    sAgentType = oTempParty(0).AgentType.Trim
                End If
            End If

            'cleaning up
            oTempParty = Nothing
            oTempSearchCriteria = Nothing

            Return sAgentType

        End Function

        Private Sub DisplayAddressInformation()
            'set the addrress for the selected agent or party

            If txtName.Text.Trim.Length = 0 Then
                If hPartyKey.Value <> "" Then
                    GetPartyKey()
                End If

            End If
            If Not (Session(CNMode) = Mode.Authorise Or Session(CNMode) = Mode.Recommend) Then
                If (Not (Session(CNClaim) IsNot Nothing And Session("ModeType") Is Nothing And CType(Session(CNMode), Mode) = Mode.PayClaim)) Then   'Claim Payments
                    If String.IsNullOrEmpty(hPartyKey.Value) = False AndAlso hPartyKey.Value <> "0" Then
                        Dim oAddress As NexusProvider.Address
                        oWebservice = New NexusProvider.ProviderManager().Provider
                        oAddress = oWebservice.GetAddress(Nothing, hPartyKey.Value, Nothing)
                        PayNow_Address.Address = oAddress
                        'Populating Party Bank Details
                        DisplayAccountTypeInformation()

                        'claning up
                        oAddress = Nothing
                    End If
                Else
                    'Claim Payments
                    Dim oPayment As NexusProvider.ClaimPayment = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).Payment
                    If oPayment.Payee IsNot Nothing Then

                        If oPayment.Payee.Address IsNot Nothing Then
                            PayNow_Address.Address1 = oPayment.Payee.Address.Address1.Trim
                            PayNow_Address.Address2 = oPayment.Payee.Address.Address2.Trim
                            PayNow_Address.Address3 = oPayment.Payee.Address.Address3.Trim
                            If oPayment.Payee.Address.Address4 IsNot Nothing Then
                                PayNow_Address.Address4 = oPayment.Payee.Address.Address4.Trim
                            End If
                            If (oPayment.Payee.Address.CountryCode IsNot Nothing) Then
                                PayNow_Address.CountryCode = oPayment.Payee.Address.CountryCode.Trim
                            End If

                            PayNow_Address.Postcode = oPayment.Payee.Address.PostCode.Trim

                        End If
                    End If
                    'Populating Party Bank Details
                    DisplayAccountTypeInformation()
                End If
            Else
                'Populating Party Bank Details
                DisplayAccountTypeInformation()
            End If

        End Sub

        Public Sub ManageControlsForTaskActivity()
            'item through Work Manager, user is coming through task for Approving Payments

            Dim oUserAuthority As New NexusProvider.UserAuthority

            oUserAuthority.UserCode = Session(CNLoginName)
            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.HasPaymentsAuthority

            'get the user limit for payment authority
            oWebservice = New NexusProvider.ProviderManager().Provider
            oWebservice.GetUserAuthorityValue(oUserAuthority)

            'store in the hidden variable to validate
            txtLimit.Value = oUserAuthority.UserAuthorityOptionalValue2
            hidHasPaymentsAuthority.Value = oUserAuthority.UserAuthorityValue.ToString()
            'cleaning up
            oWebservice = Nothing
            oUserAuthority = Nothing
            'btnDecline.Visible = True

            'make these controls visible
            GISLookup_PaymentType.Visible = True
            If Session("ModeValue") = "AP" Then
                btnDecline.Visible = False
                btnCancel.Visible = True
                btnApprove.Visible = True
            ElseIf Session("ModeValue") = "DP" Then
                btnApprove.Visible = False
                btnCancel.Visible = True
                btnDecline.Visible = True
            ElseIf Session("ModeValue") = "VP" Then
                btnApprove.Visible = False
                btnDecline.Visible = False
                btnCancel.Visible = True
                btnCancel.Text = GetLocalResourceObject("lbl_Close")
            Else
                btnDecline.Visible = True
                btnApprove.Visible = True
            End If

            liPaymentType.Visible = True

            'make these controls invisible
            btnOk.Visible = False
            ' btnCancel.Visible = False
            btnAccount.Enabled = False

            txtMediaReference.Enabled = False
            txtOurReference.Enabled = False
            txtBankReference.Enabled = False
            txtOurReference.Enabled = False
            txtTheirReference.Enabled = False
            txtAmount.Enabled = False
            txtChequeHolderName.Enabled = False
            Cash_List_Item__InstrumentNumber.Enabled = False
            txtBankLocation.Enabled = False
            txtBankBranch.Enabled = False
            GISLookup_BankList.Enabled = False
            Cash_List_Item__Cheque_Date.Enabled = False
            GISLookup_ChequeType.Enabled = False
            GISLookup_ChequeClearingTypeList.Enabled = False

            GISLookup_MediaType.Enabled = False
            GISLookup_ReceiptType.Visible = False
            GISLookup_PaymentType.Enabled = False

            liReceiptType.Visible = False
            liComments.Visible = False
            liCollectionDate.Visible = False
            chkProduceDocument.Enabled = False

            liCollectionDate.Visible = False
            Cash_List_Item__Transaction_Date.Enabled = False
            TransactionDate_uctCalendarLookup.Enabled = False
            PayNow_Address.Enabled = False
            pnlPayeeInfo.Visible = False
            'Page.ClientScript.RegisterStartupScript(GetType(String), "HidePaymentTab", "HidePaymentTab();", True)
            ScriptManager.RegisterStartupScript(Me, Page.GetType(), "HidePaymentTab", "HidePaymentTab();", True)
            pnlBankInfo.Visible = False
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "DisableAddressFileds", "DisableAddressFileds();", True)

            txtName.Enabled = False
            txtDetails.Enabled = False
            txtTendered.Enabled = False
            'if user is coming through task manager then show the Approve button and add the event

            cstUserAuthoriseLimit.Enabled = True
            btnApprove.Attributes.Add("onClick", "return checkAuthorisationAmount('" + GetLocalResourceObject("lbl_ApprovalConfirmMsg") + "','" + GetLocalResourceObject("lbl_ApprovalWarningMsg") + "');")
            btnDecline.Attributes.Add("onClick", "return DeclineWarning('" + GetLocalResourceObject("msg_DeclineWarnning") + "');")

        End Sub
        Function CheckAdditionalDetails() As Integer
            'Show Additional Details
            Dim iIsAddtionalDetails As Integer = 0
            Dim hCurrentOptionColl As New Hashtable()
            If ("CurrentOptionColl") IsNot Nothing Then

                hCurrentOptionColl = CType(ViewState("CurrentOptionColl"), Hashtable)
                If hCurrentOptionColl.ContainsKey(Trim(GISLookup_MediaType.SelectedValue)) = True Then

                    For Each hData As DictionaryEntry In hCurrentOptionColl
                        If hData.Key = Trim(GISLookup_MediaType.SelectedValue) Then
                            iIsAddtionalDetails = hData.Value
                            Exit For
                        End If
                    Next
                End If
            End If

            Return iIsAddtionalDetails
        End Function

        Sub ClaimEnableDisableCheque(ByVal bStatus As Boolean)
            CashListItem_Receipt_Cheque.Visible = bStatus
            CashListItem_Receipt_CC.Visible = False
            CashListItem_Receipt_Cheque.Visible = bStatus
            rqdChequeHolderName.Enabled = bStatus
            rqdChequeDate.Enabled = bStatus
            txtMediaReference.Enabled = bStatus
            rqdMediaReference.Enabled = bStatus
            ChequeDate_CalendarLookup.Enabled = bStatus
            rqdBankLisk.Enabled = bStatus
            rngChequeDate.Enabled = bStatus
            custvldChequeDate.Enabled = bStatus
            'liBankReference.Visible = bStatus
            '  rqdBankReference.Enabled = bStatus
            liTenderedAmount.Visible = False
            liChangeAmount.Visible = False

            If liCollectionDate.Visible = True Then
                CustChkFutureChequeDate.Enabled = bStatus
            End If

            'Show Additional Details
            If CheckAdditionalDetails() > 0 Then
                InstrumentNumberQM.Visible = bStatus
                rqdInstrumentNumber.Enabled = bStatus
                BankLocationQM.Visible = bStatus
                rqtxtBankLocation.Enabled = bStatus
                ChequeTypeQM.Visible = bStatus
                rqChequeType.Enabled = bStatus
                BankBranchQM.Visible = bStatus
                rqBankBranch.Enabled = bStatus
                ChequeClearingTypeQM.Visible = bStatus
                rqlChequeClearingType.Enabled = bStatus
            End If
        End Sub
        Sub ClaimEnableDisableCC(ByVal bStatus As Boolean)
            CashListItem_Receipt_CC.Enabled = bStatus
            CashListItem_Receipt_CC.Visible = bStatus
            CashListItem_Receipt_Cheque.Visible = False
            rqdCardNumber.Enabled = bStatus
            rqdExpiryDate.Enabled = bStatus
            rqdNameOnCard.Enabled = bStatus
            rqdChequeHolderName.Enabled = False
            rqdChequeDate.Enabled = False
            txtMediaReference.Enabled = False
            rqdMediaReference.Enabled = False
            'txtMediaReference.CssClass = "field-medium"
            rqdBankLisk.Enabled = False
            rngChequeDate.Enabled = False
            custvldChequeDate.Enabled = bStatus
            liTenderedAmount.Visible = False
            liChangeAmount.Visible = False
            CstVldCCDate.Enabled = bStatus
            cstVldCVV.Enabled = bStatus

            'Show Additional Details
            If CheckAdditionalDetails() > 0 Then
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                If oQuote IsNot Nothing Then
                    txtNameOnCard.Text = oQuote.InsuredName
                End If

                IssueNumberQM.Visible = bStatus
                PinQM.Visible = bStatus
                TypeofCardQM.Visible = bStatus
                IssuingBankQM.Visible = bStatus
                SlipQM.Visible = bStatus
                rqdExpiryDate.Enabled = bStatus
                revExpiryDate.Enabled = bStatus
                rqdCustomer.Enabled = bStatus
                rqTypeofCard.Enabled = bStatus
                rqIssuingBank.Enabled = bStatus
                rqSlip.Enabled = bStatus
                CustomerQM.Visible = bStatus

            End If
        End Sub

        Sub EnableDisableChequeControlsOnChange(ByVal bStatus As Boolean)
            txtChequeHolderName.Enabled = bStatus
            Cash_List_Item__InstrumentNumber.Enabled = bStatus
            txtBankLocation.Enabled = bStatus
            txtBankBranch.Enabled = bStatus
            GISLookup_BankList.Enabled = bStatus
            Cash_List_Item__Cheque_Date.Enabled = bStatus
            GISLookup_ChequeType.Enabled = bStatus
            GISLookup_ChequeClearingTypeList.Enabled = bStatus
        End Sub

        Sub OtherEnableDisableCheque(ByVal bStatus As Boolean)
            If (Session(CNQuoteMode) = QuoteMode.FullQuote Or Session(CNQuoteMode) = QuoteMode.MTAQuote _
                 Or Session(CNQuoteCollectionFiles) IsNot Nothing Or Session("ModeType") = "Receipt" Or Session("ModeType") = "Payment" _
                 Or (Session("ModeValue") = "IP" AndAlso Session("Type").Trim() = PaymentType.R.ToString())) Then
                If Session(CNQuote) IsNot Nothing Then
                    Dim oQuote As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)
                    txtChequeHolderName.Text = oQuote.InsuredName
                End If
                'Page.ClientScript.RegisterStartupScript(GetType(String), "HidePaymentTab", "HidePaymentTab();", True) 'Hiding Payment Tab
                ScriptManager.RegisterStartupScript(Me, Page.GetType(), "HidePaymentTab", "HidePaymentTab();", True)
                Cash_List_Item__Cheque_Date.Text = DateTime.Now.ToShortDateString()
                rngChequeDate.Enabled = bStatus
                custvldChequeDate.Enabled = bStatus
                rqdChequeDate.Enabled = bStatus

                rqdChequeHolderName.Enabled = bStatus
                ChequeDate_CalendarLookup.Enabled = bStatus
                revExpiryDate.Enabled = False
                rqdExpiryDate.Enabled = False
                rqdCustomer.Enabled = False
                rqdNameOnCard.Enabled = False
                revStartDate.Enabled = False

                CashListItem_Receipt_Cheque.Visible = bStatus
                CashListItem_Receipt_Cheque.Enabled = bStatus

                'liBankReference.Visible = bStatus
                'rqdBankReference.Enabled = bStatus
                liTenderedAmount.Visible = False
                liChangeAmount.Visible = False

                If liCollectionDate.Visible = True Then
                    CustChkFutureChequeDate.Enabled = bStatus
                End If

                'Show Additional Details
                If CheckAdditionalDetails() > 0 Then
                    If Trim(GISLookup_MediaType.SelectedValue) = "CQ" Then
                        ltAmount.Text = GetLocalResourceObject("lbl_InstrumentAmount")
                        ltBankList.Text = GetLocalResourceObject("lbl_InstrumentBankList")
                        lblChequeDate.Text = GetLocalResourceObject("lbl_InstrumentChequeDate")
                    End If

                    rqdBankLisk.Enabled = bStatus
                    InstrumentNumberQM.Visible = bStatus
                    rqdInstrumentNumber.Enabled = bStatus
                    BankLocationQM.Visible = bStatus
                    rqtxtBankLocation.Enabled = bStatus
                    ChequeTypeQM.Visible = bStatus
                    rqChequeType.Enabled = bStatus
                    BankBranchQM.Visible = bStatus
                    rqBankBranch.Enabled = bStatus
                    ChequeClearingTypeQM.Visible = bStatus
                    rqlChequeClearingType.Enabled = bStatus

                    If Trim(GISLookup_MediaType.SelectedValue) = "CQ" AndAlso Request("__EVENTARGUMENT") = "ViewClick" Then
                        txtChequeHolderName.Enabled = False
                        Cash_List_Item__InstrumentNumber.Enabled = False
                        txtBankLocation.Enabled = False
                        txtBankBranch.Enabled = False
                        GISLookup_BankList.Enabled = False
                        Cash_List_Item__Cheque_Date.Enabled = False
                        GISLookup_ChequeType.Enabled = False
                        GISLookup_ChequeClearingTypeList.Enabled = False
                    ElseIf Trim(GISLookup_MediaType.SelectedValue) = "CQ" AndAlso Request("__EVENTARGUMENT") = "EditClick" Then
                        txtChequeHolderName.Enabled = True
                        Cash_List_Item__InstrumentNumber.Enabled = True
                        txtBankLocation.Enabled = True
                        txtBankBranch.Enabled = True
                        GISLookup_BankList.Enabled = True
                        Cash_List_Item__Cheque_Date.Enabled = True
                        GISLookup_ChequeType.Enabled = True
                        GISLookup_ChequeClearingTypeList.Enabled = True
                    ElseIf Trim(GISLookup_MediaType.SelectedValue) = "CA" AndAlso Request("__EVENTARGUMENT") = "ViewClick" Then
                        CashListItem_Receipt_Cheque.Visible = False

                    ElseIf Trim(GISLookup_MediaType.SelectedValue) = "CA" AndAlso Request("__EVENTARGUMENT") = "EditClick" Then
                        CashListItem_Receipt_Cheque.Visible = False

                    End If

                Else
                    rqdBankLisk.Enabled = False
                    InstrumentNumberQM.Visible = False
                    rqdInstrumentNumber.Enabled = False
                    BankLocationQM.Visible = False
                    rqtxtBankLocation.Enabled = False
                    ChequeTypeQM.Visible = False
                    rqChequeType.Enabled = False
                    BankBranchQM.Visible = False
                    rqBankBranch.Enabled = False
                    ChequeClearingTypeQM.Visible = False
                    rqlChequeClearingType.Enabled = False

                    If Trim(GISLookup_MediaType.SelectedValue) = "CQ" AndAlso Request("__EVENTARGUMENT") = "ViewClick" Then
                        txtChequeHolderName.Enabled = False
                        Cash_List_Item__InstrumentNumber.Enabled = False
                        txtBankLocation.Enabled = False
                        txtBankBranch.Enabled = False
                        GISLookup_BankList.Enabled = False
                        Cash_List_Item__Cheque_Date.Enabled = False
                        GISLookup_ChequeType.Enabled = False
                        GISLookup_ChequeClearingTypeList.Enabled = False
                    ElseIf Trim(GISLookup_MediaType.SelectedValue) = "CA" AndAlso Request("__EVENTARGUMENT") = "ViewClick" Then
                        CashListItem_Receipt_Cheque.Visible = False

                    End If
                End If
            End If
        End Sub
        Sub OtherEnableDisableCC(ByVal bStatus As Boolean)
            If (Session(CNQuoteMode) = QuoteMode.FullQuote Or Session(CNQuoteMode) = QuoteMode.MTAQuote _
                 Or Session(CNQuoteCollectionFiles) IsNot Nothing Or Session("ModeType") = "Receipt" Or Session("ModeType") = "Payment" _
                 Or (Session("ModeValue") = "IP" AndAlso Session("Type").Trim() = PaymentType.R.ToString())) Then
                CashListItem_Receipt_CC.Visible = bStatus
                CashListItem_Receipt_CC.Enabled = bStatus
                ddlCustomer.SelectedIndex = 1
                revExpiryDate.Enabled = bStatus
                rqdExpiryDate.Enabled = bStatus
                rqdCustomer.Enabled = bStatus

                revStartDate.Enabled = bStatus
                rqdCardNumber.Enabled = bStatus
                rngChequeDate.Enabled = False
                custvldChequeDate.Enabled = bStatus
                rqdChequeDate.Enabled = False
                rqdBankLisk.Enabled = False
                rqdChequeHolderName.Enabled = False
                ChequeDate_CalendarLookup.Enabled = False
                liTenderedAmount.Visible = False
                liChangeAmount.Visible = False
                CstVldCCDate.Enabled = bStatus
                rqdNameOnCard.Enabled = bStatus
                cstVldCVV.Enabled = bStatus

                'Show Additional Details
                If CheckAdditionalDetails() > 0 Then
                    '  rqdNameOnCard.Enabled = bStatus
                    Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                    If oQuote IsNot Nothing Then
                        txtNameOnCard.Text = oQuote.InsuredName
                    End If

                    IssueNumberQM.Visible = bStatus
                    PinQM.Visible = bStatus
                    TypeofCardQM.Visible = bStatus
                    IssuingBankQM.Visible = bStatus
                    SlipQM.Visible = bStatus
                    rqdExpiryDate.Enabled = bStatus
                    revExpiryDate.Enabled = bStatus
                    rqdCustomer.Enabled = bStatus
                    rqTypeofCard.Enabled = bStatus
                    rqIssuingBank.Enabled = bStatus
                    rqSlip.Enabled = bStatus
                    CustomerQM.Visible = bStatus
                Else
                    ' rqdNameOnCard.Enabled = False
                End If
            End If

        End Sub
        Protected Sub GISLookup_MediaType_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles GISLookup_MediaType.SelectedIndexChanged
            Dim sValue As String = String.Empty
            If Session(CNClaim) IsNot Nothing Then 'Claim Payments
                Select Case Trim(GISLookup_MediaType.SelectedValue)

                    Case "BACS", "CHAPS", "CA", "DDM"
                        CashListItem_Receipt_Cheque.Visible = False
                        CashListItem_Receipt_CC.Visible = False
                        rqdChequeHolderName.Enabled = False
                        rqdChequeDate.Enabled = False
                        txtMediaReference.Enabled = False
                        rqdMediaReference.Enabled = False
                        'txtMediaReference.CssClass = "field-medium"
                        rqdBankLisk.Enabled = False
                        rngChequeDate.Enabled = False
                        custvldChequeDate.Enabled = False
                        ' liBankReference.Visible = False
                        ' rqdBankReference.Enabled = False
                        liTenderedAmount.Visible = False
                        liChangeAmount.Visible = False

                    Case "BD", "CQ", "DD", "SWIFT", "PDQ", "PF", "SO"
                        ClaimEnableDisableCC(False)
                        CashListItem_Receipt_CC.Visible = False

                        ClaimEnableDisableCheque(False)

                    Case "CC"
                        ClaimEnableDisableCheque(False)
                        CashListItem_Receipt_Cheque.Visible = False

                        ClaimEnableDisableCC(False)
                End Select
            Else
                'New Business,Cash/Cheque Reeipts/Payments, Mark For Collecion,Insurer Payments
                'Authorise Claim Payments, Claim Payment Processing
                Select Case Trim(GISLookup_MediaType.SelectedValue)
                    Case "BD", "DD", "PF", "SO", "CQ"
                        OtherEnableDisableCC(False)
                        CashListItem_Receipt_CC.Visible = False

                        ltAmount.Text = GetLocalResourceObject("lbl_Amount")
                        ltBankList.Text = GetLocalResourceObject("lbl_BankList")
                        lblChequeDate.Text = GetLocalResourceObject("lbl_ChequeDate")

                        OtherEnableDisableCheque(True)
                        EnableDisableChequeControlsOnChange(True)
                    Case "CC" 'CC stands for CREDIT CARD
                        OtherEnableDisableCheque(False)
                        CashListItem_Receipt_Cheque.Visible = False
                        ltAmount.Text = GetLocalResourceObject("lbl_Amount")
                        OtherEnableDisableCC(True)


                    Case "CA", "OCP" 'CA stands for CASH
                        ltAmount.Text = GetLocalResourceObject("lbl_Amount")
                        OtherEnableDisableCC(False)
                        OtherEnableDisableCheque(False)
                        CashListItem_Receipt_Cheque.Visible = False
                        CashListItem_Receipt_CC.Visible = False

                        If (Session(CNQuoteMode) = QuoteMode.FullQuote Or Session(CNQuoteMode) = QuoteMode.MTAQuote _
                            Or Session(CNQuoteCollectionFiles) IsNot Nothing Or Session("ModeType") = "Receipt" Or Session("ModeType") = "Payment" _
                            Or (Session("ModeValue") = "IP" AndAlso Session("Type").Trim() = PaymentType.R.ToString())) Then
                            'if user is NOt doing Payment and NOT doing task activity then only both the controls(liTenderedAmount and liChangeAmount) should be visilbe

                            txtAmount.Attributes.Add("onblur", "CheckAmount()")

                            liTenderedAmount.Visible = True
                            txtTendered.Text = txtAmount.Text
                            txtTendered.Attributes.Add("onblur", "CheckTenderedAmount('" + GetLocalResourceObject("lbl_TenderAmtErrorMsg") + "');")

                            liChangeAmount.Visible = True
                            txtChange.Text = "0.00"
                        End If


                    Case Else
                        CheckMediaTypeDetails(kIsValidationEnabled, sValue)
                        If sValue = "1" Then
                            rqdMediaReference.Enabled = True
                            txtMediaReference.CssClass = "field-medium form-control field-mandatory"
                            Dim cstScriptValidation As New CustomValidator
                            cstScriptValidation.IsValid = False
                            'look for a validation message in the page resources, but if there is not one defined add a default message
                            cstScriptValidation.ErrorMessage = IIf(GetLocalResourceObject("lbl_MediaReferenceError") Is Nothing, "Bank details have failed validation", GetLocalResourceObject("lbl_MediaReferenceError"))
                            cstScriptValidation.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                            'add the validator to the page, this will have the effect of making the page invalid
                            Page.Validators.Add(cstScriptValidation)
                        Else
                            rqdMediaReference.Enabled = False
                            txtMediaReference.CssClass = "field-medium form-control"
                        End If

                        ltAmount.Text = GetLocalResourceObject("lbl_Amount")
                        rngChequeDate.Enabled = False
                        rqdChequeDate.Enabled = False
                        rqdBankLisk.Enabled = False
                        rqdChequeHolderName.Enabled = False
                        ChequeDate_CalendarLookup.Enabled = True
                        revExpiryDate.Enabled = False
                        rqdExpiryDate.Enabled = False
                        rqdCustomer.Enabled = False
                        rqdNameOnCard.Enabled = False
                        revStartDate.Enabled = False
                        liTenderedAmount.Visible = False
                        liChangeAmount.Visible = False
                        OtherEnableDisableCheque(False)

                End Select
            End If
            MediaReferenceMadatoryorNot(False)

            Dim oDictMediaReferenceMandatory As Dictionary(Of String, Integer) = Nothing
            If ViewState("MediaRefMandatoryCacheID") IsNot Nothing Then
                oDictMediaReferenceMandatory = CType(Cache.Item(ViewState("MediaRefMandatoryCacheID")), Dictionary(Of String, Integer))
            End If
            If oDictMediaReferenceMandatory IsNot Nothing AndAlso oDictMediaReferenceMandatory.ContainsKey(GISLookup_MediaType.SelectedValue.Trim + "_IS_RECEIPT_PRINTED_AUTOMATICALLY") Then
                If (Session("ModeType") IsNot Nothing AndAlso (Session("ModeType").ToString().ToUpper().Trim = "RECEIPT" OrElse Session("ModeType").ToString().ToUpper().Trim = "PAYMENT") OrElse Session("ModeValue") = "IP") OrElse
                    (Session("Type") IsNot Nothing AndAlso Session("Type").Trim() = PaymentType.R.ToString()) Then
                    chkProduceDocument.Checked = IIf(CInt(oDictMediaReferenceMandatory.Item(GISLookup_MediaType.SelectedValue.Trim + "_IS_RECEIPT_PRINTED_AUTOMATICALLY")) = 1, True, False)
                    upUpdateProdoc.Update()
                End If
            Else
                chkProduceDocument.Checked = False
                upUpdateProdoc.Update()
            End If
        End Sub
        '' <summary>
        '' Ok Button of Claim Summary Page
        '' </summary>
        '' <param name="sender"></param>
        '' <param name="e"></param>
        '' <remarks></remarks>
        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Dim callbtnOk As Boolean
            If (Session("btnOKClicked") IsNot Nothing) Then
                callbtnOk = False
            Else
                Session("btnOKClicked") = 1
                callbtnOk = True
            End If
            'If all condition are satisfied
            If Page.IsValid Then
                Dim iFlag As Integer = CType(Session("SetFlag"), Integer)
                Dim sType As String = Session("Type")

                'Record the Session Variable to Produce the Documents - Start
                If chkProduceDocument.Checked = True Then
                    Session(CNProduceDocument) = True
                    If PnlReceiptType.Visible = True Then
                        Session(CNReceiptMode) = GISLookup_ReceiptType.Value
                        Session(CNPaymentMode) = Nothing
                    Else
                        Session(CNPaymentMode) = GISLookup_PaymentType.Value
                        Session(CNReceiptMode) = Nothing
                    End If
                Else
                    Session(CNProduceDocument) = Nothing
                    Session(CNReceiptMode) = Nothing
                    Session(CNPaymentMode) = Nothing
                End If
                'Record the Session Variable to Produce the Documents - End

                If iFlag = 1 Then
                    If Session("ModeType") = "Payment" Then 'Cash Cheque payments
                        Dim sValue As String = String.Empty
                        CheckMediaTypeDetails(kIsValidationEnabled, sValue)
                        If sValue = "1" Then
                            'Call SAM Method to Validate
                            sValue = String.Empty
                            Dim oValidationDetailsCollection As NexusProvider.ValidationDetailsCollection
                            sValue = GetDescriptionForCode(NexusProvider.ListType.PMLookup, PayNow_Address.CountryCode, "Country")
                            Dim nCountryID As Integer = GetKeyForDescription(NexusProvider.ListType.PMLookup, sValue, "Country")
                            Dim sAccountType As String = String.Empty
                            If IsNumeric(ddlAccountType.SelectedValue) Then
                                sAccountType = ddlAccountType.SelectedValue
                            End If

                            Dim sAccountNumber As String = txtBranchCode.Text + "|" + txtAccountCode.Text + "|" + sAccountType
                            sValue = String.Empty
                            CheckMediaTypeDetails(kMediaTypeID, sValue)
                            oValidationDetailsCollection = oWebservice.ValidateBankAccountNumber(sValue, nCountryID, sAccountNumber, "", sBankName:=hidBankCode.Value)
                            If oValidationDetailsCollection(0).ValidationMessageDataset <> "" AndAlso oValidationDetailsCollection(0).ValidationMessageDataset IsNot Nothing Then
                                'If the Collection returna any message from Back office Script
                                Dim cstScriptValidation As New CustomValidator
                                cstScriptValidation.IsValid = False
                                'look for a validation message in the page resources, but if there is not one defined add a default message
                                cstScriptValidation.ErrorMessage = IIf(GetLocalResourceObject("msgValidationScriptError") Is Nothing, "Bank details have failed validation", GetLocalResourceObject("msgValidationScriptError"))
                                cstScriptValidation.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                                'add the validator to the page, this will have the effect of making the page invalid
                                Page.Validators.Add(cstScriptValidation)

                                Exit Sub
                            ElseIf Not oValidationDetailsCollection(0).IsValid Then
                                'If the Collection does not returns any BO script message, and IsValid key is false, then a custom message is passed 
                                Dim cstScriptValidation As New CustomValidator
                                cstScriptValidation.IsValid = False
                                'look for a validation message in the page resources, but if there is not one defined add a default message
                                cstScriptValidation.ErrorMessage = IIf(GetLocalResourceObject("msgValidationScriptError") Is Nothing, "Bank details have failed validation", GetLocalResourceObject("msgValidationScriptError"))
                                cstScriptValidation.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                                'add the validator to the page, this will have the effect of making the page invalid
                                Page.Validators.Add(cstScriptValidation)
                                Exit Sub
                            End If

                        End If
                        oCashListItems = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                        PopulateObject()
                        If callbtnOk Then
                            oCashListItems.PaymentItems.Add(oCashListItem)
                            Session(CNCashListItem) = oCashListItems
                        End If

                        Server.Transfer("~/secure/payment/CashListItems.aspx?Mode=INST")
                        Session("ModeValue") = "INST"
                        ''CashListItems
                        'Dim changeTab2 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(2) a').tab('show')});"
                        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab2", changeTab2, True)
                        'Session("hfActiveTab") = 2
                        Session("hfPreviousTab") = 2
                    Else 'Bank Guarantee and Cash/Cheque Recipts

                        oReceiptCashListItems = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                        PopulateObject()
                        If oCashListItem.TypeCode.Trim.ToUpper = "BGDEPT" Then
                            Dim oBankGuaranteePolicy As NexusProvider.BankGuaranteePolicy = CType(ViewState("BG"), NexusProvider.BankGuaranteePolicy)
                            If grdvBGDebtDetails.Rows.Count > 0 Then
                                For iCount As Integer = 0 To grdvBGDebtDetails.Rows.Count - 1
                                    Dim chkSelected As CheckBox
                                    chkSelected = DirectCast(grdvBGDebtDetails.Rows(iCount).FindControl("chkAmtSelect"), CheckBox)
                                    If chkSelected.Checked = True Then
                                        For iTempVar As Integer = 0 To oBankGuaranteePolicy.PartyBGPolicyDetails.Count - 1
                                            If grdvBGDebtDetails.Rows(iCount).Cells(2).Text.Trim.ToUpper = oBankGuaranteePolicy.PartyBGPolicyDetails(iTempVar).PolicyRef.Trim.ToUpper Then
                                                Dim oReceiptCashListItemTypePolicies As New NexusProvider.ReceiptCashListItemTypePolicies
                                                oReceiptCashListItemTypePolicies.BGKey = oBankGuaranteePolicy.PartyBGPolicyDetails(iTempVar).BGKey
                                                oReceiptCashListItemTypePolicies.AmountTobeAllocated = oBankGuaranteePolicy.PartyBGPolicyDetails(iTempVar).OutstandingPolicyAmt
                                                oReceiptCashListItemTypePolicies.InsuranceFileKey = oBankGuaranteePolicy.PartyBGPolicyDetails(iTempVar).PolicyKey
                                                oReceiptCashListItemTypePolicies.PolicyRef = oBankGuaranteePolicy.PartyBGPolicyDetails(iTempVar).PolicyRef
                                                oCashListItem.Policies.Add(oReceiptCashListItemTypePolicies)
                                            End If
                                        Next
                                    End If
                                Next
                            End If
                        End If

                        If oCashListItem.TypeCode.Trim.ToUpper = "INST" Then
                            Dim oInstalmentPlanDetailsCollection As New NexusProvider.InstalmentPlanDetailsCollection
                            If Session("INSTALMENTPLANDETAILS") IsNot Nothing Then
                                oInstalmentPlanDetailsCollection = DirectCast(Session("INSTALMENTPLANDETAILS"), NexusProvider.InstalmentPlanDetailsCollection)
                                oReceiptCashListItems.InstalmentPlanCollection = oInstalmentPlanDetailsCollection
                            Else
                                Dim sMsgSelectOneInstalment As String = GetLocalResourceObject("msg_SelectOneInstalment")
                                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){alert('" + sMsgSelectOneInstalment + "');document.getElementById('liPaymentTab').style.display = 'none';document.getElementById('tabInstalments').click();document.getElementById('tabInstalments').show();});</script>")
                                Exit Sub
                            End If
                        End If
                        If callbtnOk Then
                            oReceiptCashListItems.ReceiptItems.Add(oCashListItem)
                            Session(CNCashListItem) = oReceiptCashListItems
                        End If
                        If oInstalmentPlanDetailsCollection IsNot Nothing AndAlso oInstalmentPlanDetailsCollection.Count > 0 Then
                            Dim nReceiptDifferenceOption As Integer = 2
                            'If Instalments are selected from different plan and then the instalment amount is overwritten
                            If txtCurrentPlanSelectedTotal.Text <> 0 AndAlso txtCurrentPlanSelectedTotal.Text <> txtOverAllSelectedTotal.Text AndAlso txtAmount.Text <> txtOverAllSelectedTotal.Text Then
                                Dim sMessageAmountLess As String = GetLocalResourceObject("msg_AmountLess")
                                Dim sMessageMultiplePlan As String = GetLocalResourceObject("msg_MultiplePlan")
                                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){alert('" + sMessageAmountLess + "');alert('" + sMessageMultiplePlan + "');document.getElementById('liPaymentTab').style.display = 'none';document.getElementById('tabInstalments').click();document.getElementById('tabInstalments').show();});</script>")
                                Exit Sub
                                'If Instalments are selected from same plan and then the instalment amount is overwritten
                            ElseIf txtAmount.Text <> txtOverAllSelectedTotal.Text AndAlso txtCurrentPlanSelectedTotal.Text = txtOverAllSelectedTotal.Text Then
                                Dim oFinancePlanDetails As NexusProvider.PremiumFinancePlan = Nothing
                                Dim oWebService As NexusProvider.ProviderBase
                                Try
                                    oWebService = New NexusProvider.ProviderManager().Provider
                                    oFinancePlanDetails = oWebService.GetHeaderAndSummariesPFPlanByKey("", ddlInstalmentPlan.SelectedValue, ViewState("PlanVersion"), Session(CNBranchCode))
                                    If oFinancePlanDetails IsNot Nothing AndAlso oFinancePlanDetails.PremiumFinanceDetails IsNot Nothing Then
                                        nReceiptDifferenceOption = oFinancePlanDetails.PremiumFinanceDetails.Receipt_Difference_Option
                                    End If
                                Catch ex As Exception
                                    Throw ex
                                Finally
                                    oWebService = Nothing
                                End Try

                                If nReceiptDifferenceOption = 2 Then
                                    Dim sURL As String = String.Empty
                                    If HttpContext.Current.Session.IsCookieless Then
                                        sURL = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/InstalmentRcptDiff.aspx?modal=true&KeepThis=true&TB_iframe=true&height=300&width=750"
                                    Else
                                        sURL = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "Modal/InstalmentRcptDiff.aspx?modal=true&KeepThis=true&TB_iframe=true&height=300&width=750"
                                    End If
                                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show",
                                    "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);  document.getElementById('liPaymentTab').style.display = 'none';});</script>")

                                    Exit Sub
                                ElseIf nReceiptDifferenceOption = 1 Then
                                    If txtCurrentPlanSelectedTotal.Text <> 0 AndAlso Convert.ToDecimal(txtAmount.Text) > Convert.ToDecimal(txtOverAllSelectedTotal.Text) Then
                                        Dim sMessageAmountGreater As String = GetLocalResourceObject("msg_AmountGreater")
                                        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){alert('" + sMessageAmountGreater + "');document.getElementById('liPaymentTab').style.display = 'none';document.getElementById('tabInstalments').click();document.getElementById('tabInstalments').show();});</script>")
                                        Exit Sub
                                    Else
                                        TakeExactAmount()
                                    End If


                                ElseIf nReceiptDifferenceOption = 0 Then
                                    BindWriteOffReason()

                                    Exit Sub
                                End If

                            End If
                        End If
                        If (IsInstamentForReceiptType() OrElse oCashListItem.TypeCode.Trim.ToUpper = "INST") AndAlso oCashListItem.MediaTypeCode.Trim.ToUpper <> "OCP" Then
                            Server.Transfer("~/secure/payment/CashListItems.aspx?TypeTrans=INST")
                            Session("TypeTrans") = "INST"
                            'CashListItems
                            ' Dim changeTab2 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(2) a').tab('show')});"
                            ' ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab2", changeTab2, True)
                            'Session("hfActiveTab") = 2
                            Session("hfPreviousTab") = 2
                        ElseIf oCashListItem.MediaTypeCode.Trim.ToUpper = "OCP" Then
                            Dim oPaymentHubDetail As NexusProvider.PaymentHubDetails = oCashListItem.PaymentHubDetails
                            Session(CNPaymentHubDetails) = oPaymentHubDetail
                            If (IsInstamentForReceiptType() OrElse oCashListItem.TypeCode.Trim.ToUpper = "INST") Then
                                oPaymentHubDetail.ReturnURL = "~/secure/payment/CashListItems.aspx?TypeTrans=INST"
                            Else
                                oPaymentHubDetail.ReturnURL = "~/secure/payment/CashListItems.aspx?TypeTrans=" & sType
                            End If

                            oPaymentHubDetail.CashListItemIndex = oReceiptCashListItems.ReceiptItems.Count - 1
                            oPaymentHubDetail.TransactionAmount = oCashListItem.Amount
                            oPaymentHubDetail.PartyKey = CInt(hPartyKey.Value)
                            oPaymentHubDetail.TransactionCurrency = oReceiptCashListItems.CoreCashList.CurrencyCode 'GBP
                            If (IsInstamentForReceiptType() OrElse oCashListItem.TypeCode.Trim.ToUpper = "INST") Then
                                Server.Transfer("~/secure/Payment/OnlineCardPayment.aspx?RequestType=TokenRegistration&TypeTrans=INST" & "&PartyKey=" + hPartyKey.Value & "&CashListItemIndex=" & oReceiptCashListItems.ReceiptItems.Count - 1)
                            Else
                                Server.Transfer("~/secure/Payment/OnlineCardPayment.aspx?RequestType=TokenRegistration&TypeTrans=" & sType & "&PartyKey=" + hPartyKey.Value & "&CashListItemIndex=" & oReceiptCashListItems.ReceiptItems.Count - 1)
                            End If

                        Else
                            Server.Transfer("~/secure/payment/CashListItems.aspx?TypeTrans=" & sType)
                            Session("TypeTrans") = sType

                            'CashListItems
                            ' Dim changeTab2 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(2) a').tab('show')});"
                            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab2", changeTab2, True)
                            'Session("hfActiveTab") = 2
                            Session("hfPreviousTab") = 2
                        End If
                    End If
                ElseIf Session("ModeValue") = "IP" Then 'Insurer Payments
                    If Session("Type").Trim() = PaymentType.R.ToString() Then
                        oReceiptCashListItems = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                        PopulateObject()
                        If oReceiptCashListItems.ReceiptItems.Count > 0 Then
                            For iCount As Integer = 0 To oReceiptCashListItems.ReceiptItems.Count - 1
                                oReceiptCashListItems.ReceiptItems.Remove(iCount)
                            Next
                        End If
                        If callbtnOk Then
                            oReceiptCashListItems.ReceiptItems.Add(oCashListItem)
                            Session(CNCashListItem) = oReceiptCashListItems
                        End If
                    ElseIf Session("Type").Trim() = PaymentType.P.ToString() Or Session("Type").Trim() = PaymentType.CP.ToString() Then

                        oCashListItems = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                        PopulateObject()
                        If oCashListItems.PaymentItems.Count > 0 Then
                            For iCount As Integer = 0 To oCashListItems.PaymentItems.Count - 1
                                oCashListItems.PaymentItems.Remove(iCount)
                            Next
                        End If
                        If callbtnOk Then
                            oCashListItems.PaymentItems.Add(oCashListItem)
                            Session(CNCashListItem) = oCashListItems
                        End If
                    Else
                        ' DO Nothing
                    End If

                    Server.Transfer("~/secure/payment/CashListItems.aspx?EVENTARGUMENT=IPRefresh&Mode=IP&Type=" + sType + "&PartyKey=" + hPartyKey.Value)
                    Session("EVENTARGUMENT") = "IPRefresh"
                    Session("ModeValue") = "IP"
                    Session("Type") = sType
                    Session("PartyKey") = hPartyKey.Value
                    'CashListItems
                    'Dim changeTab2 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(2) a').tab('show')});"
                    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab2", changeTab2, True)
                    'Session("hfActiveTab") = 2
                    Session("hfPreviousTab") = 2
                ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then 'MTA Cancellation

                    oCashListItems = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                    PopulateObject()
                    If oCashListItems.PaymentItems.Count > 0 Then
                        For iCount As Integer = 0 To oCashListItems.PaymentItems.Count - 1
                            oCashListItems.PaymentItems.Remove(iCount)
                        Next
                    End If
                    If callbtnOk Then
                        oCashListItems.PaymentItems.Add(oCashListItem)
                        oCashListItems.AccountShortCode = CType(Session(CNAccountName), String)
                    End If
                    Dim oQuote As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)

                    oWebservice = New NexusProvider.ProviderManager().Provider
                    oWebservice.CreatePaymentCashListWithItems(oCashListItems)

                    Dim oPayment As NexusProvider.Payment
                    Select Case UCase(GISLookup_MediaType.SelectedItem.Text)
                        Case "CASH"
                            oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cash, CDec(Session(CNAmountToPay)))
                            oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashListItem
                        Case "CREDIT CARD"
                            oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.CreditCard, CDec(Session(CNAmountToPay)))
                            oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashListItem
                        Case "BANKERS DRAFT"
                            oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.BankersDraft, CDec(Session(CNAmountToPay)))
                            oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashListItem
                        Case "DIRECT DEBIT"
                            oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.DebitCard, CDec(Session(CNAmountToPay)))
                            oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashListItem
                        Case "CHEQUE"
                            oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cheque, CDec(Session(CNAmountToPay)))
                            oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashListItem
                        Case Else
                            oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.AllOthers, CDec(Session(CNAmountToPay)))
                            oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashListItem
                    End Select

                    Dim oBaseParty As New NexusProvider.BaseParty

                    Dim oAccountSearchCr As New NexusProvider.AccountSearchCriteria
                    Dim oAccountColl As NexusProvider.AccountSearchResultCollection
                    Dim iAccountKey As Integer
                    Dim sPrePaymentOption As String 'for checking if PrePayment option is enable
                    sPrePaymentOption = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 87).OptionValue

                    If oQuote.BusinessTypeCode = "DIRECT" Then 'Direct Business/Customer
                        oBaseParty = oWebservice.GetParty(oQuote.PartyKey)
                        Select Case True
                            Case TypeOf oBaseParty Is NexusProvider.CorporateParty
                                With CType(oBaseParty, NexusProvider.CorporateParty)
                                    If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                        oBaseParty.UserName = .ClientSharedData.ShortName.Trim
                                    ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                        oBaseParty.UserName = .UserName.Trim
                                    End If
                                End With
                            Case TypeOf oBaseParty Is NexusProvider.PersonalParty
                                With CType(oBaseParty, NexusProvider.PersonalParty)
                                    If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                        oBaseParty.UserName = .ClientSharedData.ShortName.Trim
                                    ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                        oBaseParty.UserName = .UserName.Trim
                                    End If
                                End With
                        End Select
                        oAccountSearchCr.ShortCode = oBaseParty.UserName
                        oAccountColl = oWebservice.FindAccounts(oAccountSearchCr)
                        iAccountKey = oAccountColl(0).AccountKey
                    Else

                        oAccountSearchCr.ShortCode = oQuote.AgentCode
                        oAccountColl = oWebservice.FindAccounts(oAccountSearchCr)
                        iAccountKey = oAccountColl(0).AccountKey

                    End If

                    oPayment.PayNowDetails = Nothing
                    Dim oPayNowPaymentDetails As New NexusProvider.PaymentType
                    oPayNowPaymentDetails.InsuranceFileRef = oQuote.InsuranceFileRef
                    oPayNowPaymentDetails.CashListKey = oCashListItems.CashListKey
                    oPayNowPaymentDetails.CashListItemKey = oCashListItems.PaymentCashList(0).CashListItemKey
                    oPayNowPaymentDetails.TransDetailKey = oCashListItems.PaymentCashList(0).TransDetailKey
                    oPayNowPaymentDetails.PaymentAccountID = iAccountKey
                    oPayNowPaymentDetails.PaymentTypeCode = GISLookup_PaymentType.Value
                    oPayNowPaymentDetails.MediaTypeCode = GISLookup_MediaType.SelectedValue
                    oPayNowPaymentDetails.MediaReference = txtMediaReference.Text.ToString().Trim()
                    oPayNowPaymentDetails.OurReference = txtOurReference.Text.ToString().Trim()
                    oPayNowPaymentDetails.TheirReference = txtTheirReference.Text.ToString().Trim()
                    oPayment.PayNowPaymentDetails = oPayNowPaymentDetails

                    'set appropriate session values here to indicate payment taken and then redirect to end page
                    Session(CNPayment) = oPayment
                    If sPrePaymentOption Is Nothing Or sPrePaymentOption = "0" Then
                        Session(CNPaid) = True
                        Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                    Else
                        Session(CNPaid) = False
                        Response.Redirect("~/secure/payment/PrePayment.aspx", False)
                    End If

                ElseIf Session("ModeValue") = "CR" And Request.QueryString("CashListItemID") <> "" Then
                    'Cash/Cheque Recipts/Payments for Edit mode
                    If Session("ModeType") = "Payment" Then 'Payments
                        oUpdateCashListItem = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType).PaymentItems.Item(CType(Request("CashListItemID"), Integer))
                        Dim oPaymentCashListItems As NexusProvider.PaymentCashListItemType = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                        UpdateObject()
                        'oPaymentCashListItems.PaymentItems.Update(oUpdateCashListItem, Request.QueryString("CashListItemID"))
                        Session(CNCashListItem) = oPaymentCashListItems
                    Else 'Receipts
                        oUpdateCashListItem = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType).ReceiptItems.Item(CType(Request("CashListItemID"), Integer))
                        Dim oReceiptCashListItems As NexusProvider.ReceiptCashListItemType = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                        UpdateObject()
                        Session(CNCashListItem) = oReceiptCashListItems
                    End If

                    If oUpdateCashListItem.MediaTypeCode.Trim.ToUpper = "OCP" Then
                        Dim oPaymentHubDetail As NexusProvider.PaymentHubDetails = oUpdateCashListItem.PaymentHubDetails
                        oPaymentHubDetail.ReturnURL = "~/secure/payment/CashListItems.aspx?EVENTARGUMENT=Refresh&Mode=" + Session("ModeType").ToString + "&Type=" + sType + ""
                        Session(CNPaymentHubDetails) = oPaymentHubDetail

                        Server.Transfer("~/secure/Payment/OnlineCardPayment.aspx?RequestType=TokenRegistration&TypeTrans=" & sType & "&PartyKey=" + hPartyKey.Value & "&CashListItemIndex=" & oPaymentHubDetail.CashListItemIndex)
                    Else
                        Server.Transfer("~/secure/payment/CashListItems.aspx?EVENTARGUMENT=Refresh&Mode=" + Session("ModeType").ToString + "&Type=" + sType + "")
                        Session("EVENTARGUMENT") = "Refresh"
                        Session("ModeValue") = Session("ModeType")
                        Session("Type") = sType

                        'CashListItems
                        ' Dim changeTab2 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(2) a').tab('show')});"
                        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab2", changeTab2, True)
                        'Session("hfActiveTab") = 2
                        Session("hfPreviousTab") = 2
                    End If

                ElseIf Session(CNClaim) IsNot Nothing AndAlso Session(CNUnAllocatedClaimPayment) Is Nothing AndAlso Session(CNMode) <> Mode.Recommend Then
                    ' This functionality is for Claim Payments
                    Dim oPayment As NexusProvider.ClaimPayment = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).Payment
                    Dim oPaymentItem As New NexusProvider.PaymentCashListItemType
                    Dim oClaimResponse As NexusProvider.ClaimResponse = Nothing
                    Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                    Dim bTimeStamp As Byte() = CType(Session(CNClaimTimeStamp), Byte())
                    oWebservice = New NexusProvider.ProviderManager().Provider
                    GISLookup_PaymentType.Value = "CLP"
                    Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

                    With oPaymentItem
                        .AccountShortCode = oPayment.PartyPaidCode
                        .MediaTypeCode = GISLookup_MediaType.SelectedValue
                        .TypeCode = GISLookup_PaymentType.Value
                        .TransactionDate = CDate(IIf(Trim(Cash_List_Item__Transaction_Date.Text) <> String.Empty, Trim(Cash_List_Item__Transaction_Date.Text), DateTime.MinValue))
                        .Amount = CDec(Trim(txtAmount.Text))
                        .BankPaymentType.AccountCode = Trim(txtAccountCode.Text)
                        .BankPaymentType.BranchCode = Trim(txtBranchCode.Text)
                        .BankPaymentType.ExpiryDate = CDate(IIf(Trim(txtExpiryDate.Text) <> String.Empty, Trim(Convert.ToDateTime("01" + "/" + txtExpiryDate.Text)), DateTime.MinValue))
                        .BankPaymentType.PayeeName = Trim(txtPayeeName.Text)
                        .BankPaymentType.Reference1 = Trim(txtReference1.Text)
                        .BankPaymentType.Reference2 = Trim(txtReference2.Text)
                        .BankReference = Trim(txtBankReference.Text)
                        .FurtherDetails = txtDetails.Text
                        .MediaReference = Trim(txtMediaReference.Text)
                        .OurReference = Trim(txtOurReference.Text)
                        .TheirReference = Trim(txtTheirReference.Text)
                        .StatusCode = ddlStatus.Value
                        .BankPaymentType.BIC = Trim(txtBIC.Text)
                        .BankPaymentType.IBAN = Trim(txtIBAN.Text)
                        'Get OnScreen Address info
                        Dim oAddress As New NexusProvider.Address
                        If PayNow_Address.Address IsNot Nothing Then
                            oAddress.Address1 = PayNow_Address.Address1.Trim
                            oAddress.Address2 = PayNow_Address.Address2.Trim
                            oAddress.Address3 = PayNow_Address.Address3.Trim
                            oAddress.Address4 = PayNow_Address.Address4.Trim
                            oAddress.CountryCode = Trim(PayNow_Address.CountryCode)
                            oAddress.PostCode = PayNow_Address.Postcode.Trim
                        End If
                        .ContactAddress = oAddress
                        .ChequeDate = oPayment.PaymentDate
                    End With
                    If callbtnOk Then
                        oPayment.CashList.PaymentCashListItemType.Add(oPaymentItem)
                    End If
                    If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ShowSummary = False Then
                        SkipSummaryPage()
                    Else
                        Response.Redirect("~/Claims/summary.aspx", False)
                    End If

                ElseIf (Session(CNQuoteMode) = QuoteMode.FullQuote Or Session(CNQuoteMode) = QuoteMode.MTAQuote _
                Or Session(CNQuoteCollectionFiles) IsNot Nothing) AndAlso Session("ModeValue") <> "INSDEPOSIT" Then
                    'New Business, Mark For Collection
                    If Session(CNQuoteCollectionFiles) Is Nothing Then
                        Session.Remove(CNCashListItem)
                    End If


                    oWebservice = New NexusProvider.ProviderManager().Provider
                    Dim oPayNowReceipt As NexusProvider.AddPayNowReceipt = CType(Session(CNPayNowReceipt), NexusProvider.AddPayNowReceipt)
                    Dim oPayNowReceipts As New NexusProvider.AddPayNowReceiptCollection
                    Dim oQuote As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)


                    'Updation of the object with values
                    With oPayNowReceipt
                        .Receipt.ReceiptTypeCode = Trim(GISLookup_ReceiptType.Value)
                        .Receipt.TransactionDate = CDate(Trim(Cash_List_Item__Transaction_Date.Text))
                        .Receipt.MediaTypeCode = Trim(GISLookup_MediaType.SelectedValue)
                        .Receipt.Address1 = PayNow_Address.Address1
                        .Receipt.Address2 = PayNow_Address.Address2
                        .Receipt.Address3 = PayNow_Address.Address3
                        .Receipt.Address4 = PayNow_Address.Address4
                        .Receipt.PostalCode = PayNow_Address.Postcode
                        .Receipt.CountryCode = PayNow_Address.CountryCode
                        .Receipt.Amount = CType(Trim(txtAmount.Text), Double)
                        .Receipt.CollectionDate = CDate(Trim(Cash_List_Item__Collection_Date.Text))
                        .Receipt.Comments = Trim(txtComments.Text)
                        .Receipt.OurReference = Trim(txtOurReference.Text)
                        .Receipt.MediaReference = Trim(txtMediaReference.Text)
                        .Receipt.TheirReference = Trim(txtTheirReference.Text)
                        If Not String.IsNullOrEmpty(Trim(Cash_List_Item__Collection_Date.Text)) Then
                            .Receipt.CollectionDateSpecified = True
                        Else
                            .Receipt.CollectionDateSpecified = False
                        End If

                        .Receipt.ContactName = txtName.Text

                        'Mark For Collection - Start
                        If Session(CNQuoteCollectionFiles) Is Nothing Then
                            Select Case Trim(GISLookup_MediaType.SelectedValue)
                                Case "BD", "DD", "PF", "CQ"
                                    Dim oPayment As NexusProvider.Payment = New NexusProvider.Payment(NexusProvider.PaymentTypes.BankersDraft, CDec(Session(CNAmountToPay)))
                                    .Receipt.ChequeName = txtChequeHolderName.Text
                                    .Receipt.ChequeDate = Cash_List_Item__Cheque_Date.Text
                                    If Not String.IsNullOrEmpty(Trim(Cash_List_Item__Cheque_Date.Text)) Then
                                        .Receipt.ChequeDateSpecified = True
                                    Else
                                        .Receipt.ChequeDateSpecified = False
                                    End If
                                    .Receipt.BankReference = txtBankReference.Text

                                    If CheckAdditionalDetails() > 0 Then

                                        .Receipt.InstrumentNumber = Cash_List_Item__InstrumentNumber.Text
                                        .Receipt.DraweeBankBranch = txtBankBranch.Text
                                        .Receipt.DraweeBankLocation = txtBankLocation.Text
                                        .Receipt.DraweeBankName = GISLookup_BankList.Value
                                        .Receipt.ChequeClearingType = GISLookup_ChequeClearingTypeList.Value
                                        .Receipt.ChequeType = GISLookup_ChequeType.Value
                                    End If

                                    oPayment.PayNowDetails = oPayNowReceipt.Receipt
                                    Session(CNPayment) = oPayment
                                Case "CA", "BACS", "CHAPS", "SO", "EFT", "MFT", "CAP"
                                    Dim oPayment As NexusProvider.Payment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cash, CDec(Session(CNAmountToPay)))
                                    oPayment.PayNowDetails = oPayNowReceipt.Receipt
                                    Session(CNPayment) = oPayment

                                Case "CC"
                                    Dim oPayment As NexusProvider.Payment = New NexusProvider.Payment(NexusProvider.PaymentTypes.CreditCard, CDec(Session(CNAmountToPay)))
                                    .Receipt.CCManualAuthCode = txtManualAuth.Text
                                    .Receipt.CCExpiryDate = Cash_List_Item__Expiry_Date.Text
                                    .Receipt.CCName = txtNameOnCard.Text
                                    .Receipt.CCNumber = txtCardNumber.Text
                                    .Receipt.CCStartDate = Cash_List_Item__Start_Date.Text

                                    If CheckAdditionalDetails() > 0 Then

                                        .Receipt.CCIssue = txtIssueNumber.Text
                                        .Receipt.CCPin = txtPin.Text
                                        .Receipt.CCCustomer = ddlCustomer.SelectedValue
                                        .Receipt.CCTypeOfCard = GISLookup_TypeofCard.Value
                                        .Receipt.CCIssueBank = GISLookup_IssuingBank.Value
                                        .Receipt.CCSlipNumber = txtTransactionSlip.Text
                                    End If
                                    oPayment.PayNowDetails = oPayNowReceipt.Receipt
                                    Session(CNPayment) = oPayment
                                Case "OCP"
                                    Dim oPayment As NexusProvider.Payment = New NexusProvider.Payment(NexusProvider.PaymentTypes.PaymentHub, CDec(Session(CNAmountToPay)))
                                    Dim oPayNowPaymentDetails_OCP As New NexusProvider.PaymentType
                                    Dim oPaymentHubDetail As New NexusProvider.PaymentHubDetails
                                    oPaymentHubDetail.ReturnURL = "~/secure/TransactionConfirmation.aspx"
                                    If CDec(Session(CNAmountToPay)) > 0 Then
                                        oPaymentHubDetail.TransactionAmount = CDec(Session(CNAmountToPay))
                                        oPaymentHubDetail.RequestType = PaymentHub.RequestType.Payment
                                    ElseIf CDec(Session(CNAmountToPay)) < 0 Then
                                        oPaymentHubDetail.TransactionAmount = CDec(Session(CNAmountToPay)) * -1
                                        oPaymentHubDetail.RequestType = PaymentHub.RequestType.Refund
                                    End If

                                    oPaymentHubDetail.TransactionCurrency = oPayNowReceipt.Receipt.CurrencyCode
                                    Session(CNPaymentHubDetails) = oPaymentHubDetail
                                    oPayNowPaymentDetails_OCP.MediaTypeCode = "OCP"
                                    oPayment.PayNowPaymentDetails = oPayNowPaymentDetails_OCP
                                    oPayment.PayNowDetails = oPayNowReceipt.Receipt
                                    Session(CNPayment) = oPayment
                                    Response.Redirect("~/secure/Payment/OnlineCardPayment.aspx?PartyKey=" + Convert.ToString(oQuote.PartyKey))

                            End Select
                        End If
                        'Mark For Collection - End

                    End With

                    'Check the BusinessType in Quote Object.
                    If oQuote.BusinessTypeCode = "DIRECT" Then 'Direct Business /Customer
                        oPayNowReceipt.PartyKey = oQuote.PartyKey
                    ElseIf hPartyKey.Value IsNot Nothing AndAlso hPartyKey.Value.Trim.Length <> 0 AndAlso hPartyKey.Value.Trim <> "0" Then
                        oPayNowReceipt.PartyKey = CType(hPartyKey.Value, Integer)
                    End If

                    'Checking of the Pre-Payments setting
                    Dim oOptionType As New NexusProvider.OptionTypeSetting
                    oOptionType = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 87)
                    If oOptionType.OptionValue = "1" Then 'Pre-Payments is ON
                        If oPayNowReceipt.PartyKey = 0 Then
                            oPayNowReceipt.PartyKey = oQuote.PartyKey
                        End If
                        oWebservice.AddPayNowReceipt(oPayNowReceipt, oQuote.BranchCode)
                        Response.Redirect("~/secure/payment/PrePayment.aspx", False)
                    Else 'Pre-Payments is OFF
                        Select Case UCase(GISLookup_MediaType.SelectedItem.Text)
                            Case "CASH"
                                Dim oPayment As NexusProvider.Payment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cash, CDec(Session(CNAmountToPay)))
                                oPayment.PayNowDetails = oPayNowReceipt.Receipt
                                oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashListItem
                                Session(CNPayment) = oPayment
                            Case "CREDIT CARD"
                                Dim oPayment As NexusProvider.Payment = New NexusProvider.Payment(NexusProvider.PaymentTypes.CreditCard, CDec(Session(CNAmountToPay)))
                                oPayment.PayNowDetails = oPayNowReceipt.Receipt
                                oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashListItem
                                Session(CNPayment) = oPayment
                            Case "BANKERS DRAFT"
                                Dim oPayment As NexusProvider.Payment = New NexusProvider.Payment(NexusProvider.PaymentTypes.BankersDraft, CDec(Session(CNAmountToPay)))
                                oPayment.PayNowDetails = oPayNowReceipt.Receipt
                                oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashListItem
                                Session(CNPayment) = oPayment
                            Case "DIRECT DEBIT"
                                Dim oPayment As NexusProvider.Payment = New NexusProvider.Payment(NexusProvider.PaymentTypes.DebitCard, CDec(Session(CNAmountToPay)))
                                oPayment.PayNowDetails = oPayNowReceipt.Receipt
                                oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashListItem
                                Session(CNPayment) = oPayment
                            Case "CHEQUE"
                                Dim oPayment As NexusProvider.Payment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cheque, CDec(Session(CNAmountToPay)))
                                oPayment.PayNowDetails = oPayNowReceipt.Receipt
                                oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashListItem
                                Session(CNPayment) = oPayment
                        End Select

                        If Session(CNQuoteCollectionFiles) Is Nothing Then 'New Business
                            Session(CNPaid) = True
                            Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                        Else
                            'Mark For Collection - Start
                            'this will only be in case of Quote COllection Process when we call 
                            'CreateReceipt method seperately as we dont require paynow process to be called here
                            Dim oPayment As NexusProvider.Payment = Nothing
                            If Session(CNPayment) IsNot Nothing Then
                                oPayment = Session(CNPayment)
                            Else
                                oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cash)
                            End If

                            Dim oReceiptCashListCollection As New NexusProvider.ReceiptCashListCollection
                            oReceiptCashListItems = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                            PopulateObject()
                            If callbtnOk Then
                                oReceiptCashListItems.ReceiptItems.Add(oCashListItem)
                                oReceiptCashListCollection = oWebservice.CreateReceiptcashListWithItem(oReceiptCashListItems)
                            End If

                            Dim oCreditTransactionColl As New NexusProvider.CreditTransactionCollection
                            Dim oCreditTransaction As New NexusProvider.CreditTransaction
                            'Find the Account ID not Account Key
                            'Populate the Account regarding the Direct Business And Agency Business
                            'If Agency Business prepopulate the Agent Code
                            'Else if Direct Business then Client Code
                            Dim oBaseParty As New NexusProvider.BaseParty
                            oBaseParty = oWebservice.GetParty(oQuote.PartyKey)
                            oWebservice = New NexusProvider.ProviderManager().Provider
                            Dim oAccountSearchCr As New NexusProvider.AccountSearchCriteria
                            Dim oAccountColl As NexusProvider.AccountSearchResultCollection
                            'Here we want to find the Direct business or agnecy business if direct then get the party code
                            If oQuote.BusinessTypeCode = "DIRECT" Then 'Direct Business/Customer

                                Select Case True
                                    Case TypeOf oBaseParty Is NexusProvider.CorporateParty
                                        With CType(oBaseParty, NexusProvider.CorporateParty)
                                            '     oBaseParty.UserName = .ClientSharedData.ShortName.Trim
                                            If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                                oBaseParty.UserName = .ClientSharedData.ShortName.Trim
                                            ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                                oBaseParty.UserName = .UserName.Trim
                                            End If
                                        End With
                                    Case TypeOf oBaseParty Is NexusProvider.PersonalParty
                                        With CType(oBaseParty, NexusProvider.PersonalParty)
                                            'oBaseParty.UserName = .ClientSharedData.ShortName.Trim
                                            If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                                oBaseParty.UserName = .ClientSharedData.ShortName.Trim
                                            ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                                oBaseParty.UserName = .UserName.Trim
                                            End If
                                        End With
                                End Select
                                oAccountSearchCr.ShortCode = oBaseParty.UserName
                                oAccountColl = oWebservice.FindAccounts(oAccountSearchCr)
                                oCreditTransaction.AccountKey = oAccountColl(0).AccountKey
                            Else

                                oAccountSearchCr.ShortCode = oQuote.AgentCode
                                oAccountColl = oWebservice.FindAccounts(oAccountSearchCr)
                                oCreditTransaction.AccountKey = oAccountColl(0).AccountKey

                            End If

                            oCreditTransaction.Amount = CType(txtAmount.Text, Double)
                            oCreditTransaction.CollectionDate = Cash_List_Item__Collection_Date.Text
                            oCreditTransaction.TransDetailKey = oReceiptCashListCollection(0).TransDetailKey
                            oCreditTransactionColl.Add(oCreditTransaction)
                            oPayment.CreditTransaction = oCreditTransactionColl
                            oPayment.PayNowDetails = Nothing
                            'set appropriate session values here to indicate payment taken and then redirect to end page
                            Session(CNPayment) = oPayment
                            Session(CNPaid) = True
                            Response.Redirect("~/secure/QuoteCollectionConfirmation.aspx", False)

                            'Mark For Collection - End
                        End If
                    End If
                ElseIf Session(CNClaim) IsNot Nothing AndAlso Session(CNUnAllocatedClaimPayment) Is Nothing AndAlso Session(CNMode) = Mode.Recommend Then
                    oCashListItems = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                    PopulateObject() 'Population of the fields with values
                    oWebservice = New NexusProvider.ProviderManager().Provider

                    If Session(CNMode) = Mode.Recommend Then
                        oCashListItem.SkipPosting = True
                        oCashListItems.PaymentItems.Add(oCashListItem)
                        Try
                            oWebservice.CreatePaymentCashListWithItems(oCashListItems)
                            oWebservice.AddCashClaimLink(Session(CNClaimPaymentKey), oCashListItems.PaymentCashList(0).CashListItemKey)
                            Response.Redirect("~/secure/AuthoriseClaimPayments.aspx")
                        Catch ex As NexusProvider.NexusException
                            If ex.Errors(0).Code = "331" Then   'Code : 330 :: Description: DebtorUserGroupsAreNotSetup
                                Dim cstDebtorUserGroups As New CustomValidator
                                cstDebtorUserGroups.IsValid = False
                                'look for a validation message in the page resources, but if there is not one defined add a default message
                                cstDebtorUserGroups.ErrorMessage = IIf(GetLocalResourceObject("cstDebtorUserGroups") Is Nothing, "Debtor User Groups are not setup. Please contact your system administrator", GetLocalResourceObject("cstDebtorUserGroups"))
                                cstDebtorUserGroups.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                                'add the validator to the page, this will have the effect of making the page invalid
                                Page.Validators.Add(cstDebtorUserGroups)
                                Exit Sub
                            End If
                        End Try

                    End If
                ElseIf Session(CNUnAllocatedClaimPayment) IsNot Nothing AndAlso Session(CNMode) = Mode.Authorise Then

                    Dim oAllocationDetailsCollections As New NexusProvider.AllocationDetailsCollections
                    Dim oTrasactionDetails As New NexusProvider.AllocationDetailsCollections
                    Dim oUnallocatedClaimPayments As NexusProvider.UnallocatedClaimPayments = CType(Session(CNUnAllocatedClaimPayment), NexusProvider.UnallocatedClaimPayments)
                    Dim iAccountKey As Integer = oUnallocatedClaimPayments.AccountKey
                    Dim dAmount As Double = oUnallocatedClaimPayments.Amount
                    Dim oAllocationDetails As New NexusProvider.AllocationDetails
                    Dim oAllocation As NexusProvider.Allocation
                    Dim oTransAllocationDetails As New NexusProvider.AllocationDetails
                    Dim bIsUpdated As Boolean
                    Try
                        'Fetch CashList Details
                        Dim oCashClaimLink As NexusProvider.CashClaimLink
                        oCashClaimLink = oWebservice.GetCashClaimLink(Session(CNClaimPaymentKey), Session(CNBranchCode))

                        If oCashClaimLink IsNot Nothing AndAlso oCashClaimLink.CashListKey > 0 Then
                            'fetch cashlistitems
                            Dim oPaymentCashListItemsCollection As NexusProvider.PaymentCashListItemTypeCollection
                            Dim oPaymentCashListItem As New NexusProvider.PaymentCashListItemType
                            oPaymentCashListItemsCollection = oWebservice.GetPaymentTypeCashListItem(oCashClaimLink.CashListItemKey)

                            Dim oPaymentItem As NexusProvider.PaymentItems
                            If oPaymentCashListItemsCollection IsNot Nothing AndAlso oPaymentCashListItemsCollection.Count > 0 Then
                                For Each oCashListItemRet As NexusProvider.PaymentItems In oPaymentCashListItemsCollection(0).PaymentItems
                                    oPaymentCashListItem.CoreCashList.BankAccountCode = oPaymentCashListItemsCollection(0).CoreCashList.BankAccountCode
                                    oPaymentCashListItem.CoreCashList.CurrencyCode = oPaymentCashListItemsCollection(0).CoreCashList.CurrencyCode
                                    oPaymentCashListItem.CoreCashList.ListDate = oPaymentCashListItemsCollection(0).CoreCashList.ListDate
                                    oPaymentCashListItem.CoreCashList.Reference = oPaymentCashListItemsCollection(0).CoreCashList.Reference
                                    oPaymentCashListItem.CoreCashList.StatusCode = oPaymentCashListItemsCollection(0).CoreCashList.StatusCode
                                    oPaymentCashListItem.CoreCashList.TypeCode = oPaymentCashListItemsCollection(0).CoreCashList.TypeCode
                                    oPaymentCashListItem.CoreCashList.CashListKey = oCashClaimLink.CashListKey
                                    oPaymentItem = New NexusProvider.PaymentItems
                                    With oPaymentItem
                                        .Address = oCashListItemRet.Address
                                        .AccountShortCode = oCashListItemRet.AccountShortCode
                                        .AllocationStatusCode = oCashListItemRet.AllocationStatusCode
                                        .Amount = oCashListItemRet.Amount
                                        .Bank = oCashListItemRet.Bank
                                        .BankReference = oCashListItemRet.BankReference
                                        .CashListItemKey = oCashListItemRet.CashListItemKey
                                        .ContactName = oCashListItemRet.ContactName
                                        .CreditCard = oCashListItemRet.CreditCard
                                        .FurtherDetails = oCashListItemRet.FurtherDetails
                                        .IsProduceDocument = oCashListItemRet.IsProduceDocument
                                        .Letter = oCashListItemRet.Letter
                                        .MediaReference = oCashListItemRet.MediaReference
                                        .MediaTypeCode = oCashListItemRet.MediaTypeCode
                                        .OurReference = oCashListItemRet.OurReference
                                        .SkipPosting = False
                                        .StatusCode = oCashListItemRet.StatusCode
                                        .TaxAmount = oCashListItemRet.TaxAmount
                                        .TaxBandCode = oCashListItemRet.TaxBandCode
                                        .TaxBandKey = oCashListItemRet.TaxBandKey
                                        .TheirReference = oCashListItemRet.TheirReference
                                        .TransactionDate = oCashListItemRet.TransactionDate
                                        .TypeCode = oCashListItemRet.TypeCode
                                    End With
                                    oPaymentCashListItem.PaymentItems.Add(oPaymentItem)
                                Next
                            End If
                            'post cashlist here
                            oWebservice.CreatePaymentCashListWithItems(oPaymentCashListItem)

                            If ddlStatus.Value = "ISS" Then
                                'Finding of the Transdetails Key
                                Dim oAccountDetails As New NexusProvider.AccountDetails
                                Dim oAccountDetailsDefaults As New NexusProvider.AccountDetailsDefaults
                                oAccountDetails.DocumentRef = oUnallocatedClaimPayments.DocumentRef
                                oAccountDetails.AccountKey = oUnallocatedClaimPayments.AccountKey
                                Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                                Dim sSourceIds As String = String.Empty
                                For iCount As Integer = 0 To oUserDetails.ListOfBranches.Count - 1
                                    If oUserDetails.ListOfBranches(iCount).BranchKey > 0 Then
                                        sSourceIds = sSourceIds & oUserDetails.ListOfBranches(iCount).BranchKey & ","
                                    End If
                                Next
                                If Not String.IsNullOrEmpty(sSourceIds) Then
                                    sSourceIds = Left(sSourceIds, Len(sSourceIds) - 1)
                                    oAccountDetails.SourceArray = sSourceIds
                                Else
                                    sSourceIds = "1"
                                    oAccountDetails.SourceArray = sSourceIds
                                End If
                                oAccountDetailsDefaults = oWebservice.GetAccountDetails(oAccountDetails)
                                'Assignment of the Transdetails Key
                                oAllocationDetails.TransdetailKey = oAccountDetailsDefaults.AccountDetails(0).TransDetailKeys
                                oAllocationDetailsCollections.Add(oAllocationDetails)
                                oTrasactionDetails = oWebservice.GetTransactionDetails(iAccountKey, oAllocationDetailsCollections)

                                For Each oTempAllocationDetails As NexusProvider.AllocationDetails In oTrasactionDetails
                                    oAllocation = New NexusProvider.Allocation
                                    oAllocation.AllocationAmount = oTempAllocationDetails.Amount
                                    oAllocation.AllocationTimeStamp = oTempAllocationDetails.AllocationTimeStamp
                                    oAllocation.AllocationTransdetailKey = oTempAllocationDetails.TransdetailKey
                                    oTransAllocationDetails.Allocation.Add(oAllocation)
                                    oAllocation = Nothing
                                Next
                                oTransAllocationDetails.AccountKey = iAccountKey
                                oTransAllocationDetails.CashListItemKey = oPaymentCashListItem.PaymentCashList(0).CashListItemKey
                                oTransAllocationDetails.Amount = -dAmount
                                oTransAllocationDetails.TransdetailKey = oPaymentCashListItem.PaymentCashList(0).TransDetailKey
                                'Allocation done here
                                bIsUpdated = oWebservice.UpdateAllocation(oTransAllocationDetails)
                                If bIsUpdated Then
                                    Response.Redirect("~/secure/AuthoriseClaimPayments.aspx")
                                End If
                            Else
                                Response.Redirect("~/secure/AuthoriseClaimPayments.aspx")
                            End If
                        Else
                            'Authorise CLaim Payments,Claim Payment processing
                            oCashListItems = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                            PopulateObject() 'Population of the fields with values
                            oCashListItems.PaymentItems.Add(oCashListItem)

                            'Finding of the Transdetails Key
                            oWebservice = New NexusProvider.ProviderManager().Provider
                            oWebservice.CreatePaymentCashListWithItems(oCashListItems)
                            If ddlStatus.Value = "ISS" Then
                                Dim oAccountDetails As New NexusProvider.AccountDetails
                                Dim oAccountDetailsDefaults As New NexusProvider.AccountDetailsDefaults
                                oAccountDetails.DocumentRef = oUnallocatedClaimPayments.DocumentRef
                                oAccountDetails.AccountKey = oUnallocatedClaimPayments.AccountKey
                                oAccountDetailsDefaults = oWebservice.GetAccountDetails(oAccountDetails)
                                'Assignment of the Transdetails Key
                                oAllocationDetails.TransdetailKey = oAccountDetailsDefaults.AccountDetails(0).TransDetailKeys
                                oAllocationDetailsCollections.Add(oAllocationDetails)
                                oTrasactionDetails = oWebservice.GetTransactionDetails(iAccountKey, oAllocationDetailsCollections)

                                For Each oTempAllocationDetails As NexusProvider.AllocationDetails In oTrasactionDetails
                                    oAllocation = New NexusProvider.Allocation
                                    oAllocation.AllocationAmount = oTempAllocationDetails.Amount
                                    oAllocation.AllocationTimeStamp = oTempAllocationDetails.AllocationTimeStamp
                                    oAllocation.AllocationTransdetailKey = oTempAllocationDetails.TransdetailKey
                                    oTransAllocationDetails.Allocation.Add(oAllocation)
                                    oAllocation = Nothing
                                Next
                                oTransAllocationDetails.AccountKey = iAccountKey
                                oTransAllocationDetails.CashListItemKey = oCashListItems.PaymentCashList(0).CashListItemKey
                                oTransAllocationDetails.Amount = -dAmount
                                oTransAllocationDetails.TransdetailKey = oCashListItems.PaymentCashList(0).TransDetailKey

                                bIsUpdated = oWebservice.UpdateAllocation(oTransAllocationDetails)
                                If bIsUpdated Then
                                    Response.Redirect("~/secure/AuthoriseClaimPayments.aspx")
                                End If
                            Else
                                Response.Redirect("~/secure/AuthoriseClaimPayments.aspx")
                            End If
                        End If
                    Catch ex As NexusProvider.NexusException
                        If ex.Errors(0).Code = "331" Then   'Code : 331 :: Description: DebtorUserGroupsAreNotSetup

                            Dim cstDebtorUserGroups As New CustomValidator
                            cstDebtorUserGroups.IsValid = False
                            'look for a validation message in the page resources, but if there is not one defined add a default message
                            cstDebtorUserGroups.ErrorMessage = IIf(GetLocalResourceObject("cstDebtorUserGroups") Is Nothing, "Debtor User Groups are not setup. Please contact your system administrator", GetLocalResourceObject("cstDebtorUserGroups"))
                            cstDebtorUserGroups.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                            'add the validator to the page, this will have the effect of making the page invalid
                            Page.Validators.Add(cstDebtorUserGroups)
                            Exit Sub
                        End If
                    Finally
                        oWebservice = Nothing
                        Session(CNUnAllocatedClaimPayment) = Nothing
                        oAllocationDetails = Nothing
                        oTransAllocationDetails = Nothing
                        oAllocationDetailsCollections = Nothing
                        oTrasactionDetails = Nothing
                    End Try

                ElseIf Session(CNUnAllocatedClaimPayment) IsNot Nothing Then

                    'Authorise CLaim Payments,Claim Payment processing
                    oCashListItems = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                    PopulateObject() 'Population of the fields with values

                    If GISLookup_PaymentType IsNot Nothing AndAlso Trim(GISLookup_PaymentType.Value) = "CLP" Then
                        oCashListItem.Amount = -1 * oCashListItem.Amount
                        oCashListItem.Amount_tendered = -1 * oCashListItem.Amount_tendered
                    End If

                    oCashListItems.PaymentItems.Add(oCashListItem)
                    Dim oAllocationDetailsCollections As New NexusProvider.AllocationDetailsCollections
                    Dim oTrasactionDetails As New NexusProvider.AllocationDetailsCollections
                    Dim oUnallocatedClaimPayments As NexusProvider.UnallocatedClaimPayments = CType(Session(CNUnAllocatedClaimPayment), NexusProvider.UnallocatedClaimPayments)
                    Dim iAccountKey As Integer = oUnallocatedClaimPayments.AccountKey
                    Dim dAmount As Double = oUnallocatedClaimPayments.Amount
                    Dim oAllocationDetails As NexusProvider.AllocationDetails
                    Dim oAllocation As NexusProvider.Allocation
                    Dim oTransAllocationDetails As New NexusProvider.AllocationDetails
                    Dim bIsUpdated As Boolean
                    Try
                        'Finding of the Transdetails Key
                        oWebservice = New NexusProvider.ProviderManager().Provider
                        oWebservice.CreatePaymentCashListWithItems(oCashListItems)
                        Dim oAccountDetails As New NexusProvider.AccountDetails
                        Dim oAccountDetailsDefaults As New NexusProvider.AccountDetailsDefaults
                        oAccountDetails.DocumentRef = oUnallocatedClaimPayments.DocumentRef
                        oAccountDetails.AccountKey = oUnallocatedClaimPayments.AccountKey
                        oAccountDetailsDefaults = oWebservice.GetAccountDetails(oAccountDetails)
                        'Assignment of the Transdetails Key
                        For Each oTempAccountDetails As NexusProvider.AccountDetails In oAccountDetailsDefaults.AccountDetails
                            oAllocationDetails = New NexusProvider.AllocationDetails
                            oAllocationDetails.TransdetailKey = oTempAccountDetails.TransDetailKeys
                            oAllocationDetailsCollections.Add(oAllocationDetails)
                        Next

                        oTrasactionDetails = oWebservice.GetTransactionDetails(iAccountKey, oAllocationDetailsCollections)

                        For Each oTempAllocationDetails As NexusProvider.AllocationDetails In oTrasactionDetails
                            oAllocation = New NexusProvider.Allocation
                            oAllocation.AllocationAmount = oTempAllocationDetails.Amount
                            oAllocation.AllocationTimeStamp = oTempAllocationDetails.AllocationTimeStamp
                            oAllocation.AllocationTransdetailKey = oTempAllocationDetails.TransdetailKey
                            oTransAllocationDetails.Allocation.Add(oAllocation)
                            oAllocation = Nothing
                        Next
                        oTransAllocationDetails.AccountKey = iAccountKey
                        oTransAllocationDetails.CashListItemKey = oCashListItems.PaymentCashList(0).CashListItemKey
                        oTransAllocationDetails.Amount = -dAmount
                        oTransAllocationDetails.TransdetailKey = oCashListItems.PaymentCashList(0).TransDetailKey

                        If oTransAllocationDetails.TransdetailKey > 0 Then
                            bIsUpdated = oWebservice.UpdateAllocation(oTransAllocationDetails)
                        Else
                            bIsUpdated = True
                        End If
                        oWebservice.AddCashClaimLink(oUnallocatedClaimPayments.ClaimPaymentKey, oCashListItems.PaymentCashList(0).CashListItemKey)
                        If bIsUpdated Then
                            If Session(CNMode) IsNot Nothing AndAlso Session(CNMode) = Mode.Authorise Then
                                Response.Redirect("~/secure/AuthoriseClaimPayments.aspx", False)
                            Else
                                Response.Redirect("~/secure/payment/claimpaymentprocessing.aspx", False)
                            End If
                        End If
                    Finally
                        oWebservice = Nothing
                        Session(CNUnAllocatedClaimPayment) = Nothing
                        oAllocationDetails = Nothing
                        oTransAllocationDetails = Nothing
                        oAllocationDetailsCollections = Nothing
                        oTrasactionDetails = Nothing
                    End Try
                ElseIf Session("ModeValue") = "INS" Then
                    Dim oFinancePlanTransactionsCollection As New NexusProvider.FinancePlanTransactionsCollection
                    Dim oFinancePlanDetails As New NexusProvider.FinancePlanDetails
                    Dim oReceiptCashListCollection As NexusProvider.ReceiptCashListCollection
                    If Session(CNFinancePlanDetails) IsNot Nothing Then
                        oFinancePlanTransactionsCollection = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).Transactions
                        oFinancePlanDetails = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PremiumFinanceDetails
                    End If

                    oReceiptCashListItems = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                    PopulateObject()
                    If callbtnOk Then
                        oReceiptCashListItems.ReceiptItems.Add(oCashListItem)
                        Session(CNCashListItem) = oReceiptCashListItems
                    End If

                    Dim oAllocationDetailsCollections As New NexusProvider.AllocationDetailsCollections
                    Dim oTrasactionDetails As New NexusProvider.AllocationDetailsCollections
                    Dim iAccountKey As Integer = oFinancePlanTransactionsCollection(0).AccountKey
                    Dim dAmount As Double = oFinancePlanDetails.SettlementAmount
                    Dim oAllocationDetails As New NexusProvider.AllocationDetails

                    Dim oTransAllocationDetails As New NexusProvider.AllocationDetails
                    Dim oAllocation As NexusProvider.Allocation
                    Dim bIsUpdated As Boolean = True

                    Try
                        'Finding of the Transdetails Key from Response object of CreateReceiptcashListWithItem
                        oWebservice = New NexusProvider.ProviderManager().Provider
                        oReceiptCashListCollection = oWebservice.CreateReceiptcashListWithItem(oReceiptCashListItems)
                        ''If Plan is settled DebitTransDetailKey has to be used for auto allocation which is of 'SED'
                        If Session(CNDebitTransDetailkey) IsNot Nothing AndAlso CType(Session(CNDebitTransDetailkey), Integer) <> 0 Then
                            'Assignment of the Transdetails Key
                            oAllocationDetails.TransdetailKey = CType(Session(CNDebitTransDetailkey), Integer)
                            oAllocationDetailsCollections.Add(oAllocationDetails)
                            oTrasactionDetails = oWebservice.GetTransactionDetails(iAccountKey, oAllocationDetailsCollections)

                            For Each oTempAllocationDetails As NexusProvider.AllocationDetails In oTrasactionDetails
                                oAllocation = New NexusProvider.Allocation
                                oAllocation.AllocationAmount = oTempAllocationDetails.Amount
                                oAllocation.AllocationTimeStamp = oTempAllocationDetails.AllocationTimeStamp
                                oAllocation.AllocationTransdetailKey = oTempAllocationDetails.TransdetailKey
                                oTransAllocationDetails.Allocation.Add(oAllocation)
                                oAllocation = Nothing
                            Next
                            oTransAllocationDetails.AccountKey = iAccountKey
                            oTransAllocationDetails.CashListItemKey = oReceiptCashListCollection(0).CashListItemKey
                            oTransAllocationDetails.Amount = -dAmount
                            oTransAllocationDetails.TransdetailKey = oReceiptCashListCollection(0).TransDetailKey

                            bIsUpdated = oWebservice.UpdateAllocation(oTransAllocationDetails)
                        End If
                        If bIsUpdated Then
                            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "RedirectToPremiumFinancePlan", "RedirectToPremiumFinancePlan();", True)
                        End If
                    Catch ex As Exception
                    Finally
                        If bIsUpdated Then
                            Session(CNDebitTransDetailkey) = 0
                        End If
                    End Try
                ElseIf Session("ModeValue") = "INSDEPOSIT" Then
                    oReceiptCashListItems = New NexusProvider.ReceiptCashListItemType
                    PopulateObject() ''check values updated from UI
                    oReceiptCashListItems = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                    If callbtnOk Then
                        oReceiptCashListItems.ReceiptItems.Add(oCashListItem)
                        Session(CNCashListItem) = oReceiptCashListItems
                    End If

                    If GISLookup_MediaType.SelectedValue = "OCP" AndAlso Session(CNInstalmentMediaType) IsNot Nothing AndAlso Session(CNInstalmentMediaType).ToString().ToUpper() = "CREDIT CARD" AndAlso ViewState("PaymentHubEnabled") = "1" AndAlso Session(CNCardDetails) IsNot Nothing Then
                        Dim oPaymentItem As NexusProvider.PaymentHubDetails
                        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                        Dim oCreditCard As NexusProvider.CreditCard = Session(CNCardDetails)
                        If Session(CNPaymentHubDetails) Is Nothing Then
                            oPaymentItem = New NexusProvider.PaymentHubDetails
                        Else
                            oPaymentItem = Session(CNPaymentHubDetails)
                        End If
                        oPaymentItem.TransactionAmount = txtAmount.Text
                        oPaymentItem.TransactionCurrency = oReceiptCashListItems.CoreCashList.CurrencyCode
                        oPaymentItem.ReturnURL = "~/secure/TransactionConfirmation.aspx?Mode=INSDEPOSIT"
                        oPaymentItem.TokenID = oCreditCard.TrackingNumber
                        oPaymentItem.IntegrationToken = oCreditCard.AuthCode
                        PaymentHubProcessPurchase(oPaymentItem)
                        If oPaymentItem.ResultDescription = "0" Then
                            Session(CNPaid) = True
                        Else
                            oPaymentItem.ResultDescription = PaymentHub.ResultDescription.Declined
                            Session(CNPaid) = False
                        End If
                        Session(CNPaymentHubDetails) = oPaymentItem
                        Response.Redirect(oPaymentItem.ReturnURL, False)
                    Else
                        Response.Redirect("~/secure/TransactionConfirmation.aspx?Mode=INSDEPOSIT", False)
                    End If
                End If
            End If
        End Sub

        Private Function IsDateFormat(ByVal str As String) As Boolean
            Dim strArray As String() = str.Split("/")
            Dim result As Boolean = True
            If IsDate("01/" + str) Then
                If strArray(0).Length = 2 And strArray(1).Length = 2 Then
                    If (strArray(0) > 0 And strArray(0) <= 12) And (strArray(1) > 0 And strArray(1) < 99) Then
                        result = False
                    End If
                End If
            End If
            Return result
        End Function

        Protected Sub ManageControlsForPayClaim()
            'will manage the controls during Pay Claim Process

            'for Pay Claim Process, Collection Date and Comments should be disabled
            liComments.Visible = False
            liCollectionDate.Visible = False
            rqdCollectionDate.Enabled = False
            'Cash_List_Item__Collection_Date.CssClass = "field-medium"
            rngCollectionDate.Enabled = False

            'for Pay Claim Process, Receipt Type should be hide and Payment Type should be visible 
            liReceiptType.Visible = False
            liPaymentType.Visible = True
            GISLookup_PaymentType.Value = "CLP"
            GISLookup_PaymentType.Enabled = False
            FillMediaType()

            chkProduceDocument.Enabled = False
            txtAmount.Enabled = False
            MediaReferenceMadatoryorNot(bEnabled:=False)
            'btnOk.Text = GetLocalResourceObject("btn_Next")
            'btnCancel.Text = GetLocalResourceObject("btn_Back")

        End Sub
        Protected Sub MediaReferenceMadatoryorNot(bEnabled As Boolean)

            Dim oDictMediaReferenceMandatory As Dictionary(Of String, Integer)
            If ViewState("MediaRefMandatoryCacheID") IsNot Nothing Then
                oDictMediaReferenceMandatory = CType(Cache.Item(ViewState("MediaRefMandatoryCacheID")), Dictionary(Of String, Integer))
            End If
            rqdMediaReference.Enabled = bEnabled

            If oDictMediaReferenceMandatory IsNot Nothing AndAlso oDictMediaReferenceMandatory.ContainsKey(GISLookup_MediaType.SelectedValue) Then
                rqdMediaReference.Enabled = IIf(CInt(oDictMediaReferenceMandatory.Item(GISLookup_MediaType.SelectedValue.Trim)) = 1, True, False)
                If rqdMediaReference.Enabled Then
                    txtMediaReference.CssClass = "field-medium form-control field-mandatory"
                Else
                    txtMediaReference.CssClass = "field-medium form-control"
                End If
                updMediaRefrence.Update()
            End If
        End Sub
        Protected Sub ManageControlForClaimPaymentProcessing()
            'will manage the controls during Claim Paymwent Processing
            Dim oMultiStepApproval As NexusProvider.OptionTypeSetting = Nothing
            Dim oGetList As New NexusProvider.LookupList
            Dim bIsIncludePaymentTypeClaimPayment As Boolean

            liReceiptType.Visible = False
            liPaymentType.Visible = True
            liCollectionDate.Visible = False
            liComments.Visible = False
            pnlBankInfo.Visible = False
            FillMediaType()

            'Checking of Multi Step Approval
            'if it is ON then Pending status is "Pending" otherwise "ISSUED (ISS)"
            oMultiStepApproval = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 65)

            bIsIncludePaymentTypeClaimPayment = IsClaimIncludedInPayment()
            If oMultiStepApproval.OptionValue = "1" And bIsIncludePaymentTypeClaimPayment Then
                ddlStatus.Value = "PENDING"
                btnOk.Attributes.Add("onclick", "javascript:ApprovalAlert();")
            Else
                ddlStatus.Value = "ISS"
            End If

            GISLookup_PaymentType.Value = "CLP"
            GISLookup_PaymentType.Enabled = False
            'ddlStatus.Value = "ISS"
            txtMediaReference.Enabled = False

            rqdCollectionDate.Enabled = False
            'Cash_List_Item__Collection_Date.CssClass = "field-medium"
            rngCollectionDate.Enabled = False

            chkProduceDocument.Enabled = False

        End Sub
        Protected Sub ManageControlsForNextClick()

            liComments.Visible = False
            liCollectionDate.Visible = False
            rqdCollectionDate.Enabled = False
            rngCollectionDate.Enabled = False
            If Session("ModeType") = "Payment" Then 'Cash/Cheque Payment
                liReceiptType.Visible = False
                liPaymentType.Visible = True

            ElseIf Session("ModeType") = "Receipt" Then 'Cash/Cheque Receipt
                liReceiptType.Visible = True
                liPaymentType.Visible = False
            End If
            GISLookup_PaymentType.Enabled = False
            GISLookup_ReceiptType.Enabled = False
            GISLookup_MediaType.Enabled = False
            chkProduceDocument.Enabled = False
            txtAmount.Enabled = False
            txtChequeHolderName.Enabled = False
            Cash_List_Item__InstrumentNumber.Enabled = False
            txtBankLocation.Enabled = False
            txtBankBranch.Enabled = False
            GISLookup_BankList.Enabled = False
            Cash_List_Item__Cheque_Date.Enabled = False
            GISLookup_ChequeType.Enabled = False
            GISLookup_ChequeClearingTypeList.Enabled = False
            txtAccount.Enabled = False
            txtMediaReference.Enabled = False
            txtTheirReference.Enabled = False
            txtOurReference.Enabled = False
            txtTendered.Enabled = False
            MediaReferenceMadatoryorNot(bEnabled:=False)
            txtName.Enabled = False
            If Trim(GISLookup_MediaType.SelectedValue) = "CQ" AndAlso Session("AddMoreCashList") = "No" AndAlso Session("CashListItemFirstLoad") = False Then
                txtChequeHolderName.Enabled = False
                Cash_List_Item__InstrumentNumber.Enabled = False
                txtBankLocation.Enabled = False
                txtBankBranch.Enabled = False
                GISLookup_BankList.Enabled = False
                Cash_List_Item__Cheque_Date.Enabled = False
                GISLookup_ChequeType.Enabled = False
                GISLookup_ChequeClearingTypeList.Enabled = False
            ElseIf Trim(GISLookup_MediaType.SelectedValue) = "CA" AndAlso Session("AddMoreCashList") = "No" AndAlso Session("CashListItemFirstLoad") = False Then
                CashListItem_Receipt_Cheque.Visible = False

            End If
            'to disable address filed need JS function.
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "DisableAddressFileds", "DisableAddressFileds();", True)

            txtDetails.Enabled = False

        End Sub

        Private Sub DisplayAccountInformation_ClaimPayProcessing()
            'display Account Code by making SAM call using Account Name
            Dim oUnallocatedClaimPayment As NexusProvider.UnallocatedClaimPayments = CType(Session(CNUnAllocatedClaimPayment), NexusProvider.UnallocatedClaimPayments)
            Dim oAccountSearchCriteria As New NexusProvider.AccountSearchCriteria
            Dim oAccountSearchResultCollection As NexusProvider.AccountSearchResultCollection

            If oUnallocatedClaimPayment IsNot Nothing Then
                If String.IsNullOrEmpty(oUnallocatedClaimPayment.AccountName) = False Then
                    oAccountSearchCriteria.ShortCode = oUnallocatedClaimPayment.AccountCode
                End If
                If String.IsNullOrEmpty(oUnallocatedClaimPayment.AccountCode) = False Then
                    oAccountSearchCriteria.ShortCode = oUnallocatedClaimPayment.AccountCode.Trim
                End If

                oWebservice = New NexusProvider.ProviderManager().Provider
                oAccountSearchResultCollection = oWebservice.FindAccounts(oAccountSearchCriteria)

                If oAccountSearchResultCollection IsNot Nothing AndAlso oAccountSearchResultCollection.Count > 0 Then
                    For iCount As Integer = 0 To oAccountSearchResultCollection.Count - 1
                        If oUnallocatedClaimPayment IsNot Nothing Then
                            If oAccountSearchResultCollection(iCount).AccountKey = oUnallocatedClaimPayment.AccountKey _
                                            AndAlso oAccountSearchResultCollection(iCount).ShortCode IsNot Nothing Then
                                txtAccount.Text = oAccountSearchResultCollection(iCount).ShortCode.Trim
                                hPartyKey.Value = oAccountSearchResultCollection(iCount).PartyKey
                                'For Claim Payment Processing user should not be able to change the Account
                                txtAccount.Enabled = False
                                btnAccount.Enabled = False
                                Exit For
                            End If
                        End If
                    Next
                End If
                If txtAccount.Text = "CLMPAYABLE" Then
                    txtPayeeName.Text = oUnallocatedClaimPayment.PayeeName
                    txtAccountCode.Text = oUnallocatedClaimPayment.PayeeAccountNo
                    txtBranchCode.Text = oUnallocatedClaimPayment.PayeeShortCode
                    txtMediaReference.Text = oUnallocatedClaimPayment.MediaReference
                    txtOurReference.Text = oUnallocatedClaimPayment.OurRef
                    Cash_List_Item__Transaction_Date.Text = oUnallocatedClaimPayment.DateOfPayment.ToShortDateString()

                    Dim nMediaTypeId As Integer = 0
                    nMediaTypeId = oUnallocatedClaimPayment.PayeeMediaTypeKey

                    If nMediaTypeId > 0 Then
                        Dim strMediaTypeCode As String
                        strMediaTypeCode = GetCodeForKey(NexusProvider.ListType.PMLookup, nMediaTypeId, "MediaType", True, Session(CNBranchCode))
                        GISLookup_MediaType.SelectedValue = strMediaTypeCode
                    End If

                End If
            Else

                If Session(CNClaim) IsNot Nothing And Session(CNMode) = Mode.Recommend Then   'Claim Payments
                    'User is doing Recommend claim
                    Dim iPartyBankKey As Integer
                    Dim iPerilIndex As Integer
                    Dim iPaymentIndex As Integer
                    Dim bFoundPayment As Boolean
                    Dim oClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                    For lCount As Integer = 0 To oClaim.ClaimPeril.Count - 1
                        For lInnerCount As Integer = 0 To oClaim.ClaimPeril(lCount).ClaimPayment.Count - 1
                            If Session(CNClaimPaymentKey) = oClaim.ClaimPeril(lCount).ClaimPayment(lInnerCount).BaseClaimPaymentKey Then
                                bFoundPayment = True
                                iPaymentIndex = lInnerCount
                                Exit For
                            End If
                        Next
                        If bFoundPayment Then
                            iPerilIndex = lCount
                            Session(CNClaimPerilIndex) = iPerilIndex
                            Exit For
                        End If
                    Next

                    If oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).PartyPaidCode IsNot Nothing Then
                        txtAccount.Text = oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).PartyPaidCode
                    ElseIf oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.CLMPAYABLE Then
                        txtAccount.Text = "CLMPAYABLE"
                    End If
                Else
                    txtAccount.Text = Session(CNAccountName)
                End If


                txtAccount.Enabled = False
                btnAccount.Enabled = False
            End If
            If oUnallocatedClaimPayment IsNot Nothing Then
                Dim dMarkedAmount As Decimal
                Dim oCurrency As New NexusProvider.Currency
                Dim dTotalAmount As Decimal
                Dim oPaymentCashListItem As NexusProvider.PaymentCashListItemType

                oPaymentCashListItem = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                dMarkedAmount = CDec(oUnallocatedClaimPayment.Amount)

                If oPaymentCashListItem.CoreCashList.CurrencyCode.ToString.ToUpper <> oUnallocatedClaimPayment.CurrencyCode.ToString.ToUpper Then
                    oCurrency.AccountCode = Session(CNAccountName)
                    oCurrency.TransactionCurrencyCode = oPaymentCashListItem.CoreCashList.CurrencyCode
                    oCurrency.Mode = "ALL"
                    oCurrency = oWebservice.GetCurrencyExchangeRates(oCurrency, Session(CNTransBranchCode))
                    'Calculate the New Total Amount as per the choice
                    dTotalAmount = 0
                    dTotalAmount = Math.Round((dMarkedAmount / oCurrency.TransactionCurrencyRate), 2)
                Else
                    dTotalAmount = oUnallocatedClaimPayment.Amount
                End If
                If CDec(oUnallocatedClaimPayment.Amount) < 0 Then
                    txtAmount.Text = CDec(dTotalAmount) * -1
                Else
                    txtAmount.Text = CDec(dTotalAmount)
                End If
                If oUnallocatedClaimPayment.MediaTypeCode IsNot Nothing AndAlso oUnallocatedClaimPayment.MediaTypeCode.Length > 0 Then
                    GISLookup_MediaType.SelectedValue = oUnallocatedClaimPayment.MediaTypeCode.Trim
                End If
                txtOurReference.Text = oUnallocatedClaimPayment.OurRef
                txtTheirReference.Text = oUnallocatedClaimPayment.TheirRef
            Else
                If CDec(Session(CNAmountToPay)) < 0 Then
                    txtAmount.Text = CDec(Session(CNAmountToPay)) * -1
                Else
                    txtAmount.Text = CDec(Session(CNAmountToPay))
                End If
            End If
            txtAmount.Enabled = False
            'Display Adress Information
            DisplayAddressInformation()

            'cleaning up
            oUnallocatedClaimPayment = Nothing
            oAccountSearchCriteria = Nothing
            oAccountSearchResultCollection = Nothing

        End Sub

        Private Sub PopulateObject()
            'Updation of the objects with the fields values
            oCashListItem.AccountShortCode = Trim(txtAccount.Text)
            If Session("ModeType") = "Payment" Then
                oCashListItem.Amount = -CDbl(Trim(txtAmount.Text))
            ElseIf Session(CNUnAllocatedClaimPayment) IsNot Nothing Then
                Dim oUnallocatedClaimPayment As NexusProvider.UnallocatedClaimPayments = CType(Session(CNUnAllocatedClaimPayment), NexusProvider.UnallocatedClaimPayments)
                Dim dMarkedAmount As Decimal
                Dim oCurrency As New NexusProvider.Currency
                Dim dTotalAmount As Decimal
                Dim oPaymentCashListItem As NexusProvider.PaymentCashListItemType

                oPaymentCashListItem = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                dMarkedAmount = CDec(oUnallocatedClaimPayment.Amount)

                If oPaymentCashListItem.CoreCashList.CurrencyCode.ToString.ToUpper <> oUnallocatedClaimPayment.CurrencyCode.ToString.ToUpper Then
                    oCurrency.AccountCode = Session(CNAccountName)
                    oCurrency.TransactionCurrencyCode = oPaymentCashListItem.CoreCashList.CurrencyCode
                    oCurrency.Mode = "ALL"
                    oCurrency = oWebservice.GetCurrencyExchangeRates(oCurrency, Session(CNTransBranchCode))
                    'Calculate the New Total Amount as per the choice
                    dTotalAmount = 0
                    dTotalAmount = Math.Round((dMarkedAmount / oCurrency.TransactionCurrencyRate), 2)
                Else
                    dTotalAmount = oUnallocatedClaimPayment.Amount
                End If
                oCashListItem.Amount = dTotalAmount
            ElseIf Session("ModeType") = "Receipt" Then
                'If Session("INSTALMENTPLANDETAILS") IsNot Nothing AndAlso CDbl(txtTendered.Text) < CDbl(txtAmount.Text) Then
                '    oCashListItem.Amount = Trim(txtTendered.Text)
                'Else
                '    oCashListItem.Amount = Trim(txtAmount.Text)
                'End If

                oCashListItem.Amount = Trim(txtAmount.Text)
                txtTendered.Text = Trim(txtAmount.Text)
            ElseIf Session("ModeValue") = "IP" Then
                'set the updated amount into session
                oCashListItem.Amount = Session(CNTotalAmount)
                If oCashListItem.Amount < 0 Then
                    oCashListItem.Amount = Decimal.Parse(txtAmount.Text) * -1
                Else
                    oCashListItem.Amount = Decimal.Parse(txtAmount.Text)
                End If
            ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                oCashListItem.Amount = Session(CNAmountToPay)
                If oCashListItem.Amount < 0 Then
                    oCashListItem.Amount = Decimal.Parse(txtAmount.Text) * -1
                Else
                    oCashListItem.Amount = Decimal.Parse(txtAmount.Text)
                End If
            Else
                oCashListItem.Amount = Trim(txtAmount.Text)
            End If
            oCashListItem.Amount_tendered = Decimal.Parse(txtAmount.Text)
            If Trim(txtChange.Text).Length > 0 Then
                oCashListItem.Original_amount = Decimal.Parse(txtChange.Text)
            Else
                oCashListItem.Original_amount = 0
            End If

            oCashListItem.MediaReference = Trim(txtMediaReference.Text)
            oCashListItem.MediaTypeCode = GISLookup_MediaType.SelectedValue

            oCashListItem.MediaTypeCode = GISLookup_MediaType.SelectedValue

            If Session("ModeValue") = "IP" Then 'Insurer Payments

                If Session("Type").Trim() = PaymentType.P.ToString() Or Session("Type") = PaymentType.CP.ToString() Then
                    oCashListItem.StatusCode = "ISS"
                ElseIf Session("Type") = PaymentType.R.ToString() Then
                    oCashListItem.StatusCode = Nothing
                End If

            Else
                oCashListItem.StatusCode = "ISS"
            End If

            If chkProduceDocument.Checked Then
                oCashListItem.Letter = "Y"
                oCashListItem.IsProduceDocument = True
            Else
                oCashListItem.Letter = "N"
                oCashListItem.IsProduceDocument = False
            End If

            oCashListItem.Address = New NexusProvider.Address
            oCashListItem.Address.Address1 = If(PayNow_Address.Address1 Is Nothing, "", PayNow_Address.Address1.Trim)
            oCashListItem.Address.Address2 = If(PayNow_Address.Address2 Is Nothing, "", PayNow_Address.Address2.Trim)
            oCashListItem.Address.Address3 = If(PayNow_Address.Address3 Is Nothing, "", PayNow_Address.Address3.Trim)
            oCashListItem.Address.Address4 = If(PayNow_Address.Address4 Is Nothing, "", PayNow_Address.Address4.Trim)
            oCashListItem.Address.CountryCode = PayNow_Address.CountryCode
            oCashListItem.Address.PostCode = PayNow_Address.Postcode
            oCashListItem.OurReference = Trim(txtOurReference.Text)
            oCashListItem.AllocationStatusCode = "U"
            'Set the Selected BankKey
            If IsNumeric(ddlAccountType.SelectedValue) Then
                oCashListItem.Bank.BankKey = ddlAccountType.SelectedValue
            End If

            If GISLookup_MediaType.SelectedValue <> "" Then
                oCashListItem.Bank.AccountCode = Trim(txtAccountCode.Text)
                oCashListItem.Bank.BranchCode = Trim(txtBranchCode.Text)
                If (txtExpiryDate.Text) <> "" Then
                    If Trim(txtExpiryDate.Text).Length = 5 Then
                        txtExpiryDate.Text = "01/" & txtExpiryDate.Text
                        txtExpiryDate.Text = Convert.ToString(Convert.ToDateTime(txtExpiryDate.Text).AddMonths(1).AddDays(-1))
                    End If
                    oCashListItem.Bank.ExpiryDate = Trim(Convert.ToDateTime(txtExpiryDate.Text))
                    'oCashListItem.Bank.ExpiryDate = "01/" + Trim(txtExpiryDate.Text).ToString()
                    oCashListItem.Bank.ExpiryDateSpecified = True
                Else
                    oCashListItem.Bank.ExpiryDateSpecified = False
                End If
                oCashListItem.Bank.Reference1 = Trim(txtReference1.Text)
                oCashListItem.Bank.Reference2 = Trim(txtReference2.Text)
                oCashListItem.Bank.PayeeName = Trim(txtPayeeName.Text)
                oCashListItem.Bank.BIC = Trim(txtBIC.Text)
                oCashListItem.Bank.IBAN = Trim(txtIBAN.Text)
            End If

            oCashListItem.BankReference = Trim(txtBankReference.Text)

            'In case of any payment cheque details or credit card details is not required
            If (Session(CNQuoteMode) = QuoteMode.FullQuote Or Session(CNQuoteMode) = QuoteMode.MTAQuote _
                Or Session(CNQuoteCollectionFiles) IsNot Nothing Or Session("ModeType") = "Receipt" OrElse Session("ModeType") = "Payment" _
                Or (Session("ModeValue") = "IP" AndAlso Session("Type").Trim() = PaymentType.R.ToString())) Then
                If GISLookup_MediaType.SelectedValue = "BD" Or GISLookup_MediaType.SelectedValue = "DD" Or GISLookup_MediaType.SelectedValue = "PF" Or GISLookup_MediaType.SelectedValue = "SO" Or GISLookup_MediaType.SelectedValue = "CQ" Then
                    oCashListItem.Bank.PayeeName = txtChequeHolderName.Text.Trim
                    oCashListItem.Bank.Code = GISLookup_BankList.Value
                    oCashListItem.Bank.ChequeDate = Trim(Cash_List_Item__Cheque_Date.Text)
                    oCashListItem.Bank.Code = GISLookup_BankList.Value
                    If CheckAdditionalDetails() > 0 Then
                        oCashListItem.Bank.InstrumentNumber = Cash_List_Item__InstrumentNumber.Text
                        oCashListItem.Bank.DraweeBankBranch = txtBankBranch.Text
                        oCashListItem.Bank.DraweeBankLocation = txtBankLocation.Text
                        oCashListItem.Bank.DraweeBankName = GISLookup_BankList.Value
                        oCashListItem.Bank.ChequeClearingType = GISLookup_ChequeClearingTypeList.Value
                        oCashListItem.Bank.ChequeType = GISLookup_ChequeType.Value
                    End If

                ElseIf GISLookup_MediaType.SelectedValue = "CC" Then
                    If Session(CNQuoteCollectionFiles) IsNot Nothing Then
                        oCashListItem.CreditCard.Pin = txtPin.Text
                        oCashListItem.CreditCard.AuthCode = txtAuthCode.Text
                    End If

                    oCashListItem.CreditCard.Number = txtCardNumber.Text.Trim
                    oCashListItem.CreditCard.NameOnCreditCard = txtNameOnCard.Text.Trim
                    oCashListItem.CreditCard.StartDate = Cash_List_Item__Start_Date.Text.Trim
                    oCashListItem.CreditCard.ExpiryDate = Cash_List_Item__Expiry_Date.Text.Trim
                    oCashListItem.CreditCard.ManualAuthCode = txtManualAuth.Text.Trim


                    If CheckAdditionalDetails() > 0 Then

                        oCashListItem.CreditCard.CCIssue = txtIssueNumber.Text
                        oCashListItem.CreditCard.CCPin = txtPin.Text
                        If ddlCustomer.SelectedValue = GetLocalResourceObject("lbl_ddlCustomer_Present_Value") Then
                            oCashListItem.CreditCard.CustomerPresent = True
                        Else
                            oCashListItem.CreditCard.CustomerPresent = False
                        End If

                        oCashListItem.CreditCard.TypeCode = GISLookup_TypeofCard.Value
                        oCashListItem.CreditCard.CCIssueBank = GISLookup_IssuingBank.Value
                        oCashListItem.CreditCard.CCSlipNumber = txtTransactionSlip.Text
                    End If
                ElseIf GISLookup_MediaType.SelectedValue = "OCP" Then
                    oCashListItem.PaymentHubDetails.TransactionAmount = oCashListItem.Amount
                    oCashListItem.PaymentHubDetails.TransactionCurrency = Session(CNCurrenyCode)
                End If
            End If

            oCashListItem.ContactName = Trim(txtName.Text)
            oCashListItem.FurtherDetails = Trim(txtDetails.Text)

            oCashListItem.TheirReference = Trim(txtTheirReference.Text)
            oCashListItem.TransactionDate = CDate(Cash_List_Item__Transaction_Date.Text)
            oCashListItem.Collection_Date = CDate(Cash_List_Item__Collection_Date.Text)
            If IsDate(Cash_List_Item__Cheque_Date.Text) Then
                oCashListItem.Bank.ChequeDate = CDate(Cash_List_Item__Cheque_Date.Text)
            End If

            If Session("ModeValue") = "IP" Then 'Insurer Payments

                If Session("Type").Trim() = PaymentType.P.ToString() Or Session("Type").Trim() = PaymentType.CP.ToString() Then
                    oCashListItem.TypeCode = GISLookup_PaymentType.Value.Trim()
                ElseIf Session("Type").Trim() = PaymentType.R.ToString() Then
                    oCashListItem.TypeCode = GISLookup_ReceiptType.Value.Trim()
                End If
            ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                If Session(CNAmountToPay) < 0 Then
                    oCashListItem.TypeCode = Trim(GISLookup_PaymentType.Value)
                    oCashListItem.StatusCode = ddlStatus.Value
                Else
                    oCashListItem.TypeCode = Trim(GISLookup_ReceiptType.Value)
                End If
            Else

                'Cash/Cheque Paymnets or Claim Payment Processing/Authorise Claim Payments
                If Session("ModeType") = "Payment" Or Session(CNUnAllocatedClaimPayment) IsNot Nothing Or Session(CNMode) = Mode.Recommend Then
                    oCashListItem.TypeCode = Trim(GISLookup_PaymentType.Value)
                    oCashListItem.StatusCode = ddlStatus.Value
                Else
                    oCashListItem.TypeCode = Trim(GISLookup_ReceiptType.Value)
                End If

            End If

        End Sub

        Private Sub UpdateObject()
            'Updation of the objects with the fields values
            oUpdateCashListItem.AccountShortCode = Trim(txtAccount.Text)
            If Session("ModeType") = "Payment" Then
                oUpdateCashListItem.Amount = -CDbl(Trim(txtAmount.Text))
            Else
                oUpdateCashListItem.Amount = Trim(txtAmount.Text)
            End If

            oUpdateCashListItem.MediaReference = Trim(txtMediaReference.Text)
            oUpdateCashListItem.MediaTypeCode = GISLookup_MediaType.SelectedValue
            oUpdateCashListItem.StatusCode = Trim(txtAllocationStatus.Text)
            If chkProduceDocument.Checked Then
                oUpdateCashListItem.Letter = "Y"
                oUpdateCashListItem.IsProduceDocument = True
            Else
                oUpdateCashListItem.Letter = "N"
                oUpdateCashListItem.IsProduceDocument = False
            End If
            oUpdateCashListItem.Address = New NexusProvider.Address
            oUpdateCashListItem.Address.Address1 = PayNow_Address.Address1
            oUpdateCashListItem.Address.Address2 = PayNow_Address.Address2
            oUpdateCashListItem.Address.Address3 = PayNow_Address.Address3
            oUpdateCashListItem.Address.Address4 = PayNow_Address.Address4
            oUpdateCashListItem.Address.CountryCode = PayNow_Address.CountryCode
            oUpdateCashListItem.Address.PostCode = PayNow_Address.Postcode
            oUpdateCashListItem.OurReference = Trim(txtOurReference.Text)
            oUpdateCashListItem.AllocationStatusCode = "U"
            oUpdateCashListItem.Bank = New NexusProvider.Bank
            oUpdateCashListItem.Bank.AccountCode = Trim(txtAccountCode.Text)
            oUpdateCashListItem.Bank.BranchCode = Trim(txtBranchCode.Text)
            'Set the Selected BankKey
            If IsNumeric(ddlAccountType.SelectedValue) Then
                oUpdateCashListItem.Bank.BankKey = ddlAccountType.SelectedValue
            End If
            If (txtExpiryDate.Text) <> "" Then
                oUpdateCashListItem.Bank.ExpiryDate = Trim(Convert.ToDateTime("01" + "/" + txtExpiryDate.Text))
                'oUpdateCashListItem.Bank.ExpiryDate = "01/" + Trim(txtExpiryDate.Text)
                oUpdateCashListItem.Bank.ExpiryDateSpecified = True
            Else
                oUpdateCashListItem.Bank.ExpiryDateSpecified = False
            End If
            oUpdateCashListItem.Bank.Reference1 = Trim(txtReference1.Text)
            oUpdateCashListItem.Bank.Reference2 = Trim(txtReference2.Text)
            oUpdateCashListItem.Bank.PayeeName = Trim(txtPayeeName.Text)
            oUpdateCashListItem.BankReference = Trim(txtBankReference.Text)
            oUpdateCashListItem.Bank.BIC = Trim(txtBIC.Text)
            oUpdateCashListItem.Bank.IBAN = Trim(txtIBAN.Text)

            'In case of any payment cheque details or credit card details is not required
            If (Session(CNQuoteMode) = QuoteMode.FullQuote Or Session(CNQuoteMode) = QuoteMode.MTAQuote _
                Or Session(CNQuoteCollectionFiles) IsNot Nothing Or Session("ModeType") = "Receipt" _
                Or (Session("ModeValue") = "IP" AndAlso Session("Type").Trim() = PaymentType.R.ToString())) Then

                If GISLookup_MediaType.SelectedValue = "BD" Or GISLookup_MediaType.SelectedValue = "DD" Or GISLookup_MediaType.SelectedValue = "PF" Or GISLookup_MediaType.SelectedValue = "SO" Or GISLookup_MediaType.SelectedValue = "CQ" Then
                    oUpdateCashListItem.Bank.PayeeName = txtChequeHolderName.Text.Trim
                    oUpdateCashListItem.Bank.Code = GISLookup_BankList.Value
                    oUpdateCashListItem.Bank.ChequeDate = Trim(Cash_List_Item__Cheque_Date.Text)

                    If CheckAdditionalDetails() > 0 Then
                        oUpdateCashListItem.Bank.InstrumentNumber = Cash_List_Item__InstrumentNumber.Text
                        oUpdateCashListItem.Bank.DraweeBankBranch = txtBankBranch.Text
                        oUpdateCashListItem.Bank.DraweeBankLocation = txtBankLocation.Text
                        oUpdateCashListItem.Bank.DraweeBankName = GISLookup_BankList.Value
                        oUpdateCashListItem.Bank.ChequeClearingType = GISLookup_ChequeClearingTypeList.Value
                        oUpdateCashListItem.Bank.ChequeType = GISLookup_ChequeType.Value
                    End If

                ElseIf GISLookup_MediaType.SelectedValue = "CC" Then
                    oUpdateCashListItem.CreditCard.Number = txtCardNumber.Text.Trim
                    oUpdateCashListItem.CreditCard.NameOnCreditCard = txtNameOnCard.Text.Trim
                    oUpdateCashListItem.CreditCard.StartDate = Cash_List_Item__Start_Date.Text.Trim
                    oUpdateCashListItem.CreditCard.ExpiryDate = Cash_List_Item__Expiry_Date.Text.Trim
                    oUpdateCashListItem.CreditCard.ManualAuthCode = txtManualAuth.Text.Trim

                    If CheckAdditionalDetails() > 0 Then

                        oUpdateCashListItem.CreditCard.CCIssue = txtIssueNumber.Text
                        oUpdateCashListItem.CreditCard.CCPin = txtPin.Text

                        If ddlCustomer.SelectedValue = GetLocalResourceObject("lbl_ddlCustomer_Present_Value") Then
                            oUpdateCashListItem.CreditCard.CustomerPresent = True
                        Else
                            oUpdateCashListItem.CreditCard.CustomerPresent = False
                        End If

                        oUpdateCashListItem.CreditCard.TypeCode = GISLookup_TypeofCard.Value
                        oUpdateCashListItem.CreditCard.CCIssueBank = GISLookup_IssuingBank.Value
                        oUpdateCashListItem.CreditCard.CCSlipNumber = txtTransactionSlip.Text
                    End If
                ElseIf GISLookup_MediaType.SelectedValue = "OCP" Then
                    oUpdateCashListItem.PaymentHubDetails.CashListItemIndex = CInt(CashListItemID.Value())
                    oUpdateCashListItem.PaymentHubDetails.TransactionAmount = oUpdateCashListItem.Amount
                    oUpdateCashListItem.PaymentHubDetails.PartyKey = CInt(hPartyKey.Value)
                    oUpdateCashListItem.PaymentHubDetails.TransactionID = New Guid().ToString()
                End If
            End If
            oUpdateCashListItem.ContactName = Trim(txtName.Text)
            oUpdateCashListItem.FurtherDetails = Trim(txtDetails.Text)
            oUpdateCashListItem.StatusCode = Trim(ddlStatus.Value)
            oUpdateCashListItem.TheirReference = Trim(txtTheirReference.Text)
            oUpdateCashListItem.TransactionDate = CDate(Cash_List_Item__Transaction_Date.Text)
            If Not String.IsNullOrEmpty(Cash_List_Item__Cheque_Date.Text) Then
                oUpdateCashListItem.Bank.ChequeDate = Trim(Cash_List_Item__Cheque_Date.Text)
            End If
            oUpdateCashListItem.Bank.Code = GISLookup_BankList.Value
            If Session("ModeValue") = "IP" Then 'Insurer Payments
                If Session("Type").Trim() = PaymentType.P.ToString() Or Session("Type").Trim() = PaymentType.CP.ToString() Then
                    oCashListItem.TypeCode = GISLookup_PaymentType.Value.Trim()
                ElseIf Session("Type").Trim() = PaymentType.R.ToString() Then
                    oCashListItem.TypeCode = GISLookup_ReceiptType.Value.Trim()
                End If
            Else
                'Cash/Cheque Paymnets 
                If Session("ModeType") = "Payment" Then
                    oUpdateCashListItem.TypeCode = Trim(GISLookup_PaymentType.Value)
                Else
                    oUpdateCashListItem.TypeCode = Trim(GISLookup_ReceiptType.Value)
                End If
            End If

        End Sub

        Private Sub PopulateUpdateObject()
            'Updation of thefields values with objects values during Edit Mode
            txtAccount.Text = oCashListItem.AccountShortCode
            hiddenTempText.Value = txtAccount.Text
            If Session("ModeType") = "Payment" Or
            (Session("ModeValue") IsNot Nothing AndAlso Session("ModeValue") = "IP" AndAlso (Session("Type") IsNot Nothing AndAlso Session("Type").Trim() = PaymentType.P.ToString() Or Session("Type").Trim() = PaymentType.CP.ToString())) Then
                txtAmount.Text = oCashListItem.Amount * -1
            Else
                txtAmount.Text = oCashListItem.Amount
                'Page.ClientScript.RegisterStartupScript(GetType(String), "HidePaymentTab", "HidePaymentTab();", True)
                ScriptManager.RegisterStartupScript(Me, Page.GetType(), "HidePaymentTab", "HidePaymentTab();", True)
            End If

            txtMediaReference.Text = oCashListItem.MediaReference
            GISLookup_MediaType.SelectedValue = oCashListItem.MediaTypeCode
            txtAccount.Enabled = True
            If Session("ModeValue") = "IP" Then
                txtAmount.Enabled = False
            Else
                txtAmount.Enabled = True
            End If

            GISLookup_MediaType.Enabled = True
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtTendered.Enabled = True
            chkProduceDocument.Enabled = True
            txtName.Enabled = True
            txtDetails.Enabled = True
            txtChequeHolderName.Enabled = True
            Cash_List_Item__InstrumentNumber.Enabled = True
            txtBankLocation.Enabled = True
            txtBankBranch.Enabled = True
            GISLookup_BankList.Enabled = True
            Cash_List_Item__Cheque_Date.Enabled = True
            GISLookup_ChequeType.Enabled = True
            GISLookup_ChequeClearingTypeList.Enabled = True
            If GISLookup_MediaType.SelectedValue = "BD" Or GISLookup_MediaType.SelectedValue = "DD" Or GISLookup_MediaType.SelectedValue = "PF" Or GISLookup_MediaType.SelectedValue = "SO" Or GISLookup_MediaType.SelectedValue = "CQ" Then
                OtherEnableDisableCC(False)
                OtherEnableDisableCheque(True)

                txtChequeHolderName.Text = oCashListItem.Bank.PayeeName
                GISLookup_BankList.Value = oCashListItem.Bank.Code
                Cash_List_Item__Cheque_Date.Text = oCashListItem.Bank.ChequeDate.ToShortDateString

                If CheckAdditionalDetails() > 0 Then
                    Cash_List_Item__InstrumentNumber.Text = oCashListItem.Bank.InstrumentNumber
                    txtBankBranch.Text = oCashListItem.Bank.DraweeBankBranch
                    txtBankLocation.Text = oCashListItem.Bank.DraweeBankLocation
                    GISLookup_BankList.Value = oCashListItem.Bank.DraweeBankName
                    GISLookup_ChequeClearingTypeList.Value = oCashListItem.Bank.ChequeClearingType
                    GISLookup_ChequeType.Value = oCashListItem.Bank.ChequeType
                End If

            ElseIf GISLookup_MediaType.SelectedValue = "CC" Then
                OtherEnableDisableCheque(False)
                OtherEnableDisableCC(True)

                txtCardNumber.Text = oCashListItem.CreditCard.Number
                txtNameOnCard.Text = oCashListItem.CreditCard.NameOnCreditCard
                Cash_List_Item__Start_Date.Text = oCashListItem.CreditCard.StartDate
                Cash_List_Item__Expiry_Date.Text = oCashListItem.CreditCard.ExpiryDate
                txtManualAuth.Text = oCashListItem.CreditCard.ManualAuthCode
                If oCashListItem.CreditCard.CustomerPresent = True Then
                    ddlCustomer.Items(0).Selected = True
                Else
                    ddlCustomer.Items(1).Selected = True
                End If

                If CheckAdditionalDetails() > 0 Then

                    txtIssueNumber.Text = oCashListItem.CreditCard.CCIssue
                    txtPin.Text = oCashListItem.CreditCard.CCPin
                    GISLookup_TypeofCard.Value = oCashListItem.CreditCard.TypeCode
                    GISLookup_IssuingBank.Value = oCashListItem.CreditCard.CCIssueBank
                    txtTransactionSlip.Text = oCashListItem.CreditCard.CCSlipNumber
                End If

            ElseIf GISLookup_MediaType.SelectedValue = "CA" Then

                If (Session(CNQuoteMode) = QuoteMode.FullQuote Or Session(CNQuoteMode) = QuoteMode.MTAQuote _
                    Or Session(CNQuoteCollectionFiles) IsNot Nothing Or Session("ModeType") = "Receipt" _
                    Or (Session("ModeValue") = "IP" AndAlso Session("Type").Trim() = PaymentType.R.ToString())) Then
                    'if user is NOt doing Payment and NOT doing task activity then only both the controls(liTenderedAmount and liChangeAmount) should be visilbe
                    Cash_List_Item__Cheque_Date.Text = oCashListItem.Bank.ChequeDate.ToShortDateString
                    txtAmount.Attributes.Add("onblur", "CheckAmount()")

                    liTenderedAmount.Visible = True
                    txtTendered.Text = txtAmount.Text
                    txtTendered.Attributes.Add("onblur", "CheckTenderedAmount('" + GetLocalResourceObject("lbl_TenderAmtErrorMsg") + "');")
                    liChangeAmount.Visible = True
                    txtChange.Text = "0.0"
                End If
            End If
            '''''txtAllocationStatus.Text = oCashListItem.StatusCode
            If oCashListItem.IsProduceDocument Then
                chkProduceDocument.Checked = True
            Else
                chkProduceDocument.Checked = False
            End If
            txtBankReference.Text = oCashListItem.BankReference
            txtOurReference.Text = oCashListItem.OurReference
            txtTheirReference.Text = oCashListItem.TheirReference
            txtMediaReference.Text = oCashListItem.MediaReference
            txtPayeeName.Text = oCashListItem.Bank.PayeeName
            txtAccountCode.Text = oCashListItem.Bank.AccountCode
            If CStr(oCashListItem.Bank.ExpiryDate) <> Date.MinValue Then
                txtExpiryDate.Text = CStr(oCashListItem.Bank.ExpiryDate)
            Else
                txtExpiryDate.Text = ""
            End If
            txtReference1.Text = oCashListItem.Bank.Reference1
            txtReference2.Text = oCashListItem.Bank.Reference2
            txtBranchCode.Text = oCashListItem.Bank.BranchCode
            txtBIC.Text = oCashListItem.Bank.BIC
            txtIBAN.Text = oCashListItem.Bank.IBAN
            PayNow_Address.Address1 = oCashListItem.Address.Address1
            PayNow_Address.Address2 = oCashListItem.Address.Address2
            PayNow_Address.Address3 = oCashListItem.Address.Address3
            PayNow_Address.Address4 = oCashListItem.Address.Address4
            PayNow_Address.CountryCode = oCashListItem.Address.CountryCode
            PayNow_Address.Postcode = oCashListItem.Address.PostCode
            txtDetails.Text = oCashListItem.FurtherDetails
            txtName.Text = oCashListItem.ContactName
            GISLookup_PaymentType.Value = oCashListItem.TypeCode
            GISLookup_PaymentType.Enabled = True
            Cash_List_Item__Transaction_Date.Text = oCashListItem.TransactionDate.ToShortDateString
            txtAllocationStatus.Text = oCashListItem.AllocationStatusCode
            'Bank Guarantee or Cash/Cheque Receipt
            If Session("ModeType") = "Receipt" Then
                GISLookup_ReceiptType.Value = oCashListItem.TypeCode
                GISLookup_ReceiptType.Enabled = True
                pnlBGDebtDetails.Visible = True
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oBankGuaranteePolicy As NexusProvider.BankGuaranteePolicy
                ' Dim sResult() As String = Session(CNResultSet).ToString.Split(",")
                ' calls the web method for obtaining data
                Dim oAccountSearchCriteria As New NexusProvider.AccountSearchCriteria
                Dim oAccountSearchResultCollection As NexusProvider.AccountSearchResultCollection
                oAccountSearchCriteria.ShortCode = oCashListItem.AccountShortCode
                oAccountSearchResultCollection = oWebservice.FindAccounts(oAccountSearchCriteria)
                oBankGuaranteePolicy = oWebservice.GetPoliciesOnBankGuaranteeForReceipt(oAccountSearchResultCollection(0).AccountKey, NexusProvider.BGGetPoliciesActionTypeType.OutStandingPremium, oAccountSearchResultCollection(0).PartyKey)
                grdvBGDebtDetails.DataSource = oBankGuaranteePolicy.PartyBGPolicyDetails
                grdvBGDebtDetails.DataBind()
                ViewState("BG") = oBankGuaranteePolicy

                For iCount As Integer = 0 To oCashListItem.Policies.Count - 1

                    For jCount As Integer = 0 To grdvBGDebtDetails.Rows.Count - 1
                        If grdvBGDebtDetails.Rows(jCount).Cells(2).Text.Trim.ToUpper = oCashListItem.Policies(iCount).PolicyRef.Trim.ToUpper Then
                            DirectCast(grdvBGDebtDetails.Rows(jCount).FindControl("chkAmtSelect"), CheckBox).Checked = True
                        End If
                    Next

                Next

            End If

        End Sub

        Protected Sub GISLookup_ReceiptType_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles GISLookup_ReceiptType.SelectedIndexChange
            'Dim b As bACTCashlistitem = Nothing

            If IsInstamentForReceiptType() Then
                hdnIsInstalment.Value = 1
                If Not (String.IsNullOrEmpty(hPartyKey.Value) OrElse hPartyKey.Value.Equals("0")) Then
                    FindPremiumFinancePlans()
                    GetFinancePlanDetails()
                    liTenderedAmount.Visible = True
                End If
            Else
                hdnIsInstalment.Value = 0
                ScriptManager.RegisterStartupScript(Me, Page.GetType(), "ShowHideInstalTab", "ShowHideInstalTab();", True)

            End If
            If GISLookup_ReceiptType.Value = "BGDEPT" Then
                pnlBGDebtDetails.Visible = True

                If String.IsNullOrEmpty(Me.hiddenAccountKey.Value) = False AndAlso Me.hiddenAccountKey.Value <> "0" Then
                    pnlBGDebtDetails.Visible = True
                    Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oBankGuaranteePolicy As NexusProvider.BankGuaranteePolicy
                    Dim iPartyKey As Integer
                    If String.IsNullOrEmpty(hPartyKey.Value) = False AndAlso hPartyKey.Value.Trim <> "0" Then
                        iPartyKey = Integer.Parse(hPartyKey.Value)
                    End If
                    'Dim sResult() As String = Session(CNResultSet).ToString.Split(",")
                    oBankGuaranteePolicy = oWebservice.GetPoliciesOnBankGuaranteeForReceipt(CInt(Me.hiddenAccountKey.Value), NexusProvider.BGGetPoliciesActionTypeType.OutStandingPremium, iPartyKey)
                    grdvBGDebtDetails.DataSource = oBankGuaranteePolicy.PartyBGPolicyDetails
                    grdvBGDebtDetails.DataBind()
                    ViewState("BG") = oBankGuaranteePolicy
                End If
            Else
                pnlBGDebtDetails.Visible = False

            End If

        End Sub

        Protected Sub chkAmtSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles chkAmtSelect.CheckedChanged
            Dim dPremiumAmount As Double = 0
            Dim dAmount As Double = 0
            If txtAmount.Text.Trim.Length > 0 Then
                dAmount = txtAmount.Text
            End If

            If dAmount = 0 Then
                For iTempVar As Integer = 0 To grdvBGDebtDetails.Rows.Count - 1
                    Dim chkSelected As CheckBox
                    chkSelected = DirectCast(grdvBGDebtDetails.Rows(iTempVar).FindControl("chkAmtSelect"), CheckBox)
                    chkSelected.Checked = False
                Next
            Else
                For iTempVar As Integer = 0 To grdvBGDebtDetails.Rows.Count - 1
                    Dim chkSelected As CheckBox
                    chkSelected = DirectCast(grdvBGDebtDetails.Rows(iTempVar).FindControl("chkAmtSelect"), CheckBox)
                    If chkSelected.Checked Then
                        dPremiumAmount += Convert.ToDouble(grdvBGDebtDetails.Rows(iTempVar).Cells(4).Text)
                        If dPremiumAmount > dAmount Then
                            chkSelected.Checked = False
                        End If
                    End If
                Next
            End If

        End Sub

        Protected Sub custvldComments_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles custvldComments.ServerValidate
            'if Collection date is less than Current Date than Comments is required to be entered
            If Cash_List_Item__Collection_Date.Enabled = True And CDate(Cash_List_Item__Collection_Date.Text.Trim) < CDate(Date.Now.ToShortDateString) And txtComments.Text.Trim.Length = 0 Then
                args.IsValid = False
            End If
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

            If Session("ModeValue") = "VP" OrElse Session("ModeValue") = "AP" OrElse Session("ModeValue") = "DP" Then
                Response.Redirect("~/secure/AuthorizePayments.aspx?Type=Task&CashListItemKey=" & Request.QueryString("CashListItemKey") & " & Mode = " & Session("ModeValue") & "", True)
            End If
            If CType(Session(CNMode), Mode) = Mode.PayClaim Then
                'Claim Payment
                Response.Redirect("~/secure/Payment/CashList.aspx", True)
            ElseIf Session(CNUnAllocatedClaimPayment) IsNot Nothing Or Session(CNMode) = Mode.Recommend Then
                'Claim Payment Processing
                Response.Redirect("~/secure/payment/CashList.aspx", False)
            ElseIf Session(CNQuoteCollectionFiles) IsNot Nothing Then
                'Mark For Collection
                Response.Redirect("~/secure/QuoteCollection.aspx", True)
            ElseIf Session("ModeValue") = "IP" Then
                'Insurer Payments
                Dim sType As String = Session("Type")
                Server.Transfer("~/secure/payment/CashListItems.aspx?Mode=IP&Type=" + sType)
                Session("ModeValue") = "IP"
                Session("Type") = sType
                'CashListItems
                Dim changeTab2 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(2) a').tab('show')});"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab2", changeTab2, True)
                'Session("hfActiveTab") = 2
                Session("hfPreviousTab") = 2
            ElseIf Session("ModeType") = "Payment" Or Session("ModeType") = "Receipt" AndAlso Session("ModeValue") <> "INS" AndAlso Session("ModeValue") <> "INSDEPOSIT" Then

                If Session("INSTALMENTPLANDETAILS") IsNot Nothing Then
                    oReceiptCashListItems = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                    If oReceiptCashListItems.ReceiptItems.Count > 0 Then
                        oReceiptCashListItems.ReceiptItems.Remove(oReceiptCashListItems.ReceiptItems.Count - 1)
                    End If
                End If
                Server.Transfer("~/secure/payment/CashListItems.aspx?Mode=INST")
                Session("ModeValue") = "INST"
                'CashListItems
                Dim changeTab2 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(2) a').tab('show')});"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab2", changeTab2, True)
                'Session("hfActiveTab") = 2
                Session("hfPreviousTab") = 2
            ElseIf Session("ModeValue") = "INS" Then
                Response.Redirect("~/PremiumFinance/PremiumFinancePlan.aspx?Type=EditPlan", True)
            ElseIf Session("ModeValue") = "INSDEPOSIT" Then
                'Response.Redirect("~/secure/payment/CashList.aspx?Mode=INSDEPOSIT")
                Session("ModeValue") = "INSDEPOSIT"
                'CashList
                Dim changeTab0 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(0) a').tab('show')});"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab0", changeTab0, True)
                'Session("hfActiveTab") = 0
                Session("hfPreviousTab") = 0
            Else
                Response.Redirect("~/secure/PremiumDisplay.aspx", True)
            End If
        End Sub

        Protected Sub IsClaimPaymentAuthority_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles IsClaimPaymentAuthority.ServerValidate
            If (Session(CNClaim) IsNot Nothing Or Session(CNUnAllocatedClaimPayment) IsNot Nothing) AndAlso Session(CNMode) = Mode.Authorise Then
                Dim oUserAuthority As New NexusProvider.UserAuthority
                oUserAuthority.UserCode = Session(CNLoginName)
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.HasClaimPaymentsAuthority
                oWebservice = New NexusProvider.ProviderManager().Provider
                oWebservice.GetUserAuthorityValue(oUserAuthority)
                If oUserAuthority.UserAuthorityValue = 0 Then
                    args.IsValid = False
                    IsClaimPaymentAuthority.ErrorMessage = GetLocalResourceObject("Err_NoPaymentAuthority")
                ElseIf CDbl(txtAmount.Text.Trim) > oUserAuthority.UserAuthorityOptionalValue2 Then
                    args.IsValid = False
                    IsClaimPaymentAuthority.ErrorMessage = GetLocalResourceObject("Err_OverLimit")
                    IsClaimPaymentAuthority.ErrorMessage = IsClaimPaymentAuthority.ErrorMessage.Replace("#Amount", New Money(oUserAuthority.UserAuthorityOptionalValue2, Session(CNCurrenyCode)).Formatted)
                Else
                    args.IsValid = True
                End If
            ElseIf Session("ModeType") Is Nothing OrElse Session("ModeType") = "Payment" Then
                Dim oUserAuthority As New NexusProvider.UserAuthority
                oUserAuthority.UserCode = Session(CNLoginName)
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.HasPaymentsAuthority
                oWebservice = New NexusProvider.ProviderManager().Provider
                oWebservice.GetUserAuthorityValue(oUserAuthority)

                If oUserAuthority.UserAuthorityValue = "1" AndAlso CDbl(txtAmount.Text.Trim) > oUserAuthority.UserAuthorityOptionalValue2 Then
                    Dim sCurrencyCode As String
                    sCurrencyCode = GetCodeForKey(NexusProvider.ListType.PMLookup, oUserAuthority.UserAuthorityOptionalValue1, "currency", True)
                    args.IsValid = False

                    IsClaimPaymentAuthority.ErrorMessage = GetLocalResourceObject("Err_OverLimit")
                    IsClaimPaymentAuthority.ErrorMessage = IsClaimPaymentAuthority.ErrorMessage.Replace("#Amount", New Money(oUserAuthority.UserAuthorityOptionalValue2, sCurrencyCode).Formatted)
                Else
                    args.IsValid = True
                End If
            End If
        End Sub

        Protected Sub grdvBGDebtDetails_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvBGDebtDetails.Load
            If grdvBGDebtDetails.PageCount = 1 Then
                grdvBGDebtDetails.AllowPaging = False
            End If
        End Sub

        Protected Sub grdvBGDebtDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvBGDebtDetails.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(1).Text = CType(e.Row.DataItem, NexusProvider.PartyBGPolicyDetails).BGDueDate.ToShortDateString
            End If
        End Sub

        Protected Sub custvldSelectOneoftheAccount_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles custvldSelectOneoftheAccount.ServerValidate
            'Validtion need to be check in case of the NB/MTA/REN only
            If Session(CNQuote) IsNot Nothing And Session(CNClaim) Is Nothing Then
                'Selected Account should be of Client or Agent(Intermediary)
                Dim oParty As NexusProvider.BaseParty = CType(Session(CNParty), NexusProvider.BaseParty)
                Dim oQuote As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)

                Dim sAgentCode As String = Nothing
                Dim sUserName As String = Nothing
                Dim sAccountShortCode As String = ""
                Dim iAgentKey As Integer
                If oQuote.AgentCode IsNot Nothing Then
                    sAgentCode = oQuote.AgentCode.Trim
                    iAgentKey = CInt(oQuote.Agent.Trim)

                    If iAgentKey <> 0 Then
                        sAccountShortCode = oWebservice.GetAccountShortCodeFromParty(iAgentKey)
                    End If

                End If

                If oParty IsNot Nothing Then
                    Select Case True
                        Case TypeOf oParty Is NexusProvider.CorporateParty
                            With CType(oParty, NexusProvider.CorporateParty)
                                ' sUserName = .ClientSharedData.ShortName
                                If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                    sUserName = .ClientSharedData.ShortName
                                ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                    sUserName = .UserName.Trim
                                End If
                            End With
                        Case TypeOf oParty Is NexusProvider.PersonalParty
                            With CType(oParty, NexusProvider.PersonalParty)
                                ' sUserName = .ClientSharedData.ShortName
                                If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                    sUserName = .ClientSharedData.ShortName
                                ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                    sUserName = .UserName.Trim
                                End If
                            End With
                    End Select
                ElseIf String.IsNullOrEmpty(ViewState("PartyName")) Then
                    'session has no party i.e. user is coming through Quote Collection Process
                    'ViewState("PartyName") is empty so get it using SAM call
                    sUserName = GetPartyCode()
                Else
                    'session has no party i.e. user is coming through Quote Collection Process
                    sUserName = ViewState("PartyName")
                End If

                If (sAccountShortCode IsNot Nothing AndAlso txtAccount.Text.Trim = sAccountShortCode.Trim) Or (sUserName IsNot Nothing AndAlso txtAccount.Text.Trim.ToUpper = sUserName.Trim.ToUpper) Then
                    args.IsValid = True
                Else
                    custvldSelectOneoftheAccount.ErrorMessage = custvldSelectOneoftheAccount.ErrorMessage.Replace("#ClientCode", sUserName).Replace("#AgentCode", sAgentCode)
                    args.IsValid = False
                End If
            End If
        End Sub

        Protected Sub btnApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApprove.Click
            If Page.IsValid Then

                Dim iClaimPaymentKey As Integer = Request.QueryString("CashListItemKey")
                Dim oPaymentCashList As New NexusProvider.PaymentCashListItemType
                Dim bTempByte As Byte() = {0, 1, 0, 1, 0, 1, 0, 1}

                oPaymentCashList.CashListKey = iClaimPaymentKey
                oPaymentCashList.Declined = False
                oPaymentCashList.TimeStamp = bTempByte
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
                        Dim sErrorMessage As String = IIf(ex.Errors(0).Description Is Nothing, "Payment transaction has not been auto-allocated.", ex.Errors(0).Description)
                        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Auto Allocation", "alert('" + sErrorMessage + "');window.location = '../../secure/AuthorizePayments.aspx?Type=Task&CashListItemKey=" & iClaimPaymentKey & "';", True)
                        Response.Write("<script type='text/javascript'>alert('" & sErrorMessage & "');window.location='../../secure/AuthorizePayments.aspx?Type=Task&CashListItemKey=" & iClaimPaymentKey & "';</script>")

                        Exit Sub
                    ElseIf ex.Errors(0).Code = "339" Then 'Code : 336 :: Description: Unconfirmed/Unhandled Exceptions
                        Dim sReturnCode As NexusProvider.OptionTypeSetting

                        Dim sMessage As String = GetLocalResourceObject("msgAutoAllocationSuccess")
                        Try
                            oWebservice = New NexusProvider.ProviderManager().Provider
                            sReturnCode = oWebservice.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5059)
                            If sReturnCode IsNot Nothing AndAlso sReturnCode.OptionValue IsNot Nothing Then
                                If sReturnCode.OptionValue = "1" Then
                                    ' Response.Redirect("~/secure/PaymentAuthorizationView.aspx?Type=Task&CashListItemKey=" & iClaimPaymentKey)
                                    Response.Write("<script type='text/javascript'>alert('" & sMessage & "');window.location='../../secure/PaymentAuthorizationView.aspx?Type=Task&CashListItemKey=" & iClaimPaymentKey & "';</script>")
                                Else
                                    Response.Redirect("~/secure/PaymentAuthorizationView.aspx?Type=Task&CashListItemKey=" & iClaimPaymentKey)
                                    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "AutoAllocationDisabled", "window.location.href = '../../secure/PaymentAuthorizationView.aspx?Type=Task&CashListItemKey='" & iClaimPaymentKey, False)
                                End If
                            End If
                            Exit Sub
                        Finally
                            oWebservice = Nothing
                            iClaimPaymentKey = Nothing
                        End Try
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
                Response.Redirect("~/secure/PaymentAuthorizationView.aspx?Mode=" & Session("ModeValue") & "&CashListItemKey=" & iClaimPaymentKey)
            End If
        End Sub

        Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
            'Decline
            oWebservice = New NexusProvider.ProviderManager().Provider
            Dim iClaimPaymentKey As Integer = Request.QueryString("CashListItemKey")
            Dim sURL As String
            If Session(CNLoginName).ToString.Trim.ToUpper <> sCreatedBy.Trim.ToUpper Then
                Dim oPaymentCashList As New NexusProvider.PaymentCashListItemType
                Dim bTempByte As Byte() = {0, 1, 0, 1, 0, 1, 0, 1}
                oPaymentCashList.CashListKey = iClaimPaymentKey
                oPaymentCashList.Declined = True
                oPaymentCashList.Comments = txtDecline.Value
                oPaymentCashList.TimeStamp = bTempByte
                Try
                    oWebservice.ApproveCashListItem(oPaymentCashList)
                    VldUser.IsValid = True

                    sURL = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "Modal/AddEvent.aspx?DeclinePartyKey=" & hPartyKey.Value & "&CallingPage=CashListItem&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750"
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show",
                    "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>")

                Catch ex As NexusProvider.NexusException
                    If ex.Errors(0).Code = "330" Then   'Code : 330 :: Description: DebtorUserGroupsAreNotSetup

                        Dim cstDebtorUserGroups As New CustomValidator
                        cstDebtorUserGroups.IsValid = False
                        'look for a validation message in the page resources, but if there is not one defined add a default message
                        cstDebtorUserGroups.ErrorMessage = IIf(GetLocalResourceObject("cstDebtorUserGroups") Is Nothing, "Cannot Proceed- Debtor User Groups are not setup. Please contact your system administrator.", GetLocalResourceObject("cstDebtorUserGroups"))
                        cstDebtorUserGroups.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                        'add the validator to the page, this will have the effect of making the page invalid
                        Page.Validators.Add(cstDebtorUserGroups)
                        Exit Sub
                    ElseIf ex.Errors(0).Code = "331" Then   'Code : 331 :: Description: DebtorUserGroupsAreNotSetup

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
                    iClaimPaymentKey = Nothing
                    oPaymentCashList = Nothing
                    bTempByte = Nothing
                End Try

            End If
        End Sub



        Protected Sub VldUser_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles VldUser.ServerValidate
            'Will be applicable only for Approve Button while doing task activity
            'sCreatedBy is NULL means user is NOT doing task activity, No Need to vaidate the User

            If Not String.IsNullOrEmpty(sCreatedBy) Then
                If Session(CNLoginName).ToString.Trim.ToUpper <> sCreatedBy.Trim.ToUpper Then
                    args.IsValid = True
                Else
                    args.IsValid = False
                End If
            Else
                args.IsValid = True
            End If

        End Sub

        Protected Sub custValidAccount_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles custValidAccount.ServerValidate
            'validate the account code

            Dim oAccountSearchCriteria As New NexusProvider.AccountSearchCriteria
            Dim oAccountSearchResultCollection As NexusProvider.AccountSearchResultCollection

            'create the request
            oAccountSearchCriteria.ShortCode = txtAccount.Text.Trim()

            'make SAM call to validate the account code
            oWebservice = New NexusProvider.ProviderManager().Provider
            oAccountSearchResultCollection = oWebservice.FindAccounts(oAccountSearchCriteria)

            If oAccountSearchResultCollection Is Nothing Then
                'if no record returns i.e. incorrect Account Code, return false
                args.IsValid = False
            Else
                hPartyKey.Value = oAccountSearchResultCollection(0).PartyKey
            End If

            'cleaning up
            oAccountSearchCriteria = Nothing
            oAccountSearchResultCollection = Nothing
        End Sub
        Protected Sub custValidOCPAccount_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles custValidOCPAccount.ServerValidate
            'validate the account code

            If (String.IsNullOrEmpty(hPartyKey.Value) = True Or hPartyKey.Value.Trim = "0") AndAlso Trim(GISLookup_MediaType.SelectedValue) = "OCP" Then
                args.IsValid = False
            End If


        End Sub
        Protected Sub GetPartyKey()
            Dim oAccountSearchCriteria As New NexusProvider.AccountSearchCriteria
            Dim oAccountSearchResultCollection As New NexusProvider.AccountSearchResultCollection

            oAccountSearchCriteria.ShortCode = txtAccount.Text.Trim()
            oAccountSearchCriteria.ShowBalanceSpecified = False
            oAccountSearchCriteria.ShowDeletedSpecified = False
            oAccountSearchCriteria.ExcludeInsurerAgents = False
            oAccountSearchCriteria.IncludeInsurerAgents = False

            oWebservice = New NexusProvider.ProviderManager().Provider
            oAccountSearchResultCollection = oWebservice.FindAccounts(oAccountSearchCriteria)

            If oAccountSearchResultCollection IsNot Nothing Then
                If oAccountSearchResultCollection.Count > 0 Then
                    Me.hiddenAccountKey.Value = oAccountSearchResultCollection(0).AccountKey
                    Me.txtAccount.Text = oAccountSearchResultCollection(0).ShortCode
                    Me.hAccountName.Value = oAccountSearchResultCollection(0).AccountName
                    Me.hPartyKey.Value = oAccountSearchResultCollection(0).PartyKey
                    If oAccountSearchResultCollection(0).ContactName IsNot Nothing _
                    AndAlso oAccountSearchResultCollection(0).ContactName.Trim.Length = 0 Then
                        txtName.Text = oAccountSearchResultCollection(0).AccountName
                        'TO Populate the cheque name or Name of Credit card
                        txtChequeHolderName.Text = oAccountSearchResultCollection(0).AccountName
                        txtNameOnCard.Text = oAccountSearchResultCollection(0).AccountName
                    Else
                        txtName.Text = oAccountSearchResultCollection(0).ContactName
                        'TO Populate the cheque name or Name of Credit card
                        txtChequeHolderName.Text = oAccountSearchResultCollection(0).ContactName
                        txtNameOnCard.Text = oAccountSearchResultCollection(0).ContactName
                    End If
                End If
            Else
                Me.hPartyKey.Value = ""
            End If
            oAccountSearchCriteria = Nothing
            oAccountSearchResultCollection = Nothing
            oWebservice = Nothing
        End Sub

        Protected Sub CstVldCCDate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CstVldCCDate.ServerValidate
            'Expiry Date
            Dim iMonth, iYear As Integer
            If Cash_List_Item__Expiry_Date.Text.Trim.Length > 0 Then
                iMonth = Cash_List_Item__Expiry_Date.Text.Trim.Split("/")(0)
                iYear = Cash_List_Item__Expiry_Date.Text.Trim.Split("/")(1)
                If iMonth <= 0 Or iMonth > 12 Then
                    'If Month is Invalid
                    args.IsValid = False
                    CstVldCCDate.ErrorMessage = GetLocalResourceObject("lbl_InvalidFormat")
                ElseIf iMonth < Now.Month AndAlso iYear <= CInt(Now.Year.ToString.Substring(2, 2)) Then
                    'If Month is Invalid
                    args.IsValid = False
                ElseIf iYear < CInt(Now.Year.ToString.Substring(2, 2)) Then
                    'If Month is Valid
                    args.IsValid = False
                    CstVldCCDate.ErrorMessage = GetLocalResourceObject("lbl_CCExpired")
                End If
            End If
        End Sub
        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'If Session("hfActiveTab") IsNot Nothing Or Session("hfPreviousTab") IsNot Nothing Then 'commented by shipali
            'If Session("hfActiveTab") = 1 Or Session("hfPreviousTab") = 1 Then 'commented by shipali
            Dim aa As Boolean = CBool(Session("CashListItemFirstLoad"))
            'If ViewState("CashListItemFirstLoad") IsNot Nothing AndAlso CBool(ViewState("CashListItemFirstLoad")) Then
            '    ViewState("CashListItemFirstLoad") = False
            'End If
            'if  Not IsPostBack Then
            If Request.QueryString("Mode") = "AP" OrElse Request.QueryString("Mode") = "CR" OrElse Request.QueryString("Mode") = "VP" OrElse Request.QueryString("Mode") = "DP" OrElse (hdnTabName.Value = "tab-CashListItem") Then
                'If Not IsPostBack Then

                'End If
                If Session("ModeType") = "Receipt" Then 'Receipts

                    'Page.ClientScript.RegisterStartupScript(GetType(String), "HidePaymentTab", "HidePaymentTab();", True)
                    ScriptManager.RegisterStartupScript(Me, Page.GetType(), "HidePaymentTab", "HidePaymentTab();", True)
                End If
                If Not IsPostBack Or aa Then

                    If Session("ModeType") = "Payment" Or Session("ModeType") = "Receipt" Then
                        If String.IsNullOrEmpty(hiddenTempText.Value) = False AndAlso hiddenTempText.Value <> "0" AndAlso Session("PartyKey") <> "" Then
                            'AndAlso String.IsNullOrEmpty(hPartyKey.Value) = True Or hPartyKey.Value.Trim = "0" Then
                            'This code will retreive the address if user directly type the account code
                            GetPartyKey()
                        End If
                    End If

                    oWebservice = New NexusProvider.ProviderManager().Provider
                    ' hPartyKey.Value = Session("PartyKey")
                    If hPartyKey.Value IsNot Nothing AndAlso hPartyKey.Value.Trim.Length <> 0 AndAlso hPartyKey.Value.Trim <> 0 Then
                        'if has valid patykey
                        DisplayAddressInformation() 'Populate the address based on account selection
                    ElseIf hPartyKey.Value Is Nothing Or hPartyKey.Value.Trim.Length = 0 Or hPartyKey.Value = "0" Then
                        DisplayAccountTypeInformation()
                    End If
                End If

                'Updation of the Address based on the Account Key
                If Request("__EVENTARGUMENT") = "RefreshIP" Then
                    'Reset the Address control
                    txtName.Text = String.Empty
                    PayNow_Address.Address1 = String.Empty
                    PayNow_Address.Address2 = String.Empty
                    PayNow_Address.Address3 = String.Empty
                    PayNow_Address.Address4 = String.Empty
                    PayNow_Address.CountryCode = String.Empty
                    PayNow_Address.Postcode = String.Empty

                    If Session("ModeType") = "Payment" Or Session("ModeType") = "Receipt" Then
                        If String.IsNullOrEmpty(hiddenTempText.Value) = False AndAlso hiddenTempText.Value <> "0" AndAlso Session("PartyKey") <> "" Then
                            'AndAlso String.IsNullOrEmpty(hPartyKey.Value) = True Or hPartyKey.Value.Trim = "0" Then
                            'This code will retreive the address if user directly type the account code
                            GetPartyKey()
                        End If
                    End If

                    oWebservice = New NexusProvider.ProviderManager().Provider
                    If hPartyKey.Value IsNot Nothing AndAlso hPartyKey.Value.Trim.Length <> 0 AndAlso hPartyKey.Value.Trim <> 0 Then
                        'populate Address Type
                        DisplayAddressInformation()
                    End If
                End If

                'cleaning up
                oWebservice = Nothing

                'If Mode is DP redirect to PaymentAuthorizationComment Page
                If Session("ModeValue") = "DP" Or Session("ModeValue") = "AP" Or Session("ModeValue") = "VP" Then
                    ' Response.Redirect("~/secure/AuthorizePayments.aspx?Type=Task&CashListItemKey=" & Request.QueryString("CashListItemKey") & "&Mode=" & Session("ModeValue"))
                Else

                    'This will populate search account modal 
                    If HttpContext.Current.Session.IsCookieless Then
                        btnAccount.OnClientClick = "tb_show(null ,'" & System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/FindAccount.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=650' , null);return false;"
                        If txtAmount.Text <> "" AndAlso txtLimit.Value <> "" Then
                            If Convert.ToDecimal(txtAmount.Text) < Convert.ToDecimal(txtLimit.Value) Then
                                btnDecline.OnClientClick = "tb_show(null ,'" & System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/Confirmation.aspx?modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=200&width=500' , null);return false;"
                            End If
                        End If
                    Else
                        btnAccount.OnClientClick = "tb_show(null ,'" & System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "/Modal/FindAccount.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=650' , null);return false;"

                        If txtAmount.Text <> "" AndAlso txtLimit.Value <> "" AndAlso Convert.ToDecimal(txtAmount.Text) < Convert.ToDecimal(txtLimit.Value) Then
                            btnDecline.OnClientClick = "tb_show(null ,'" & System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "/Modal/Confirmation.aspx?modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=200&width=500' , null);return false;"
                        End If
                    End If
                End If
            End If
            'End If
        End Sub
        Private Sub DisplayAccountTypeInformation()
            Dim oBankDetails As NexusProvider.BankCollection
            'create a unique key and add this to viewstate
            'this will be used to cache the results of the SAM call
            Dim PartyBankpageCacheID As Guid
            PartyBankpageCacheID = Guid.NewGuid()
            ViewState.Add("PartyBankpageCacheID", PartyBankpageCacheID.ToString)

            If (String.IsNullOrEmpty(hPartyKey.Value) = False AndAlso hPartyKey.Value <> "0") AndAlso Not (Session(CNMode) = Mode.Authorise Or Session(CNMode) = Mode.Recommend) Then
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                oBankDetails = oWebService.GetPartyBankDetails(CInt(hPartyKey.Value))
            End If


            Dim oTempBankDetailsCollection As New NexusProvider.BankCollection
            Dim oTempBankDetails As NexusProvider.Bank

            If Session(CNMode) = Mode.Authorise Or Session(CNMode) = Mode.Recommend Then
                Dim iPartyBankKey As Integer
                Dim iPerilIndex As Integer
                Dim iPaymentIndex As Integer
                Dim bFoundPayment As Boolean
                Dim oClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
                If oClaim IsNot Nothing Then
                    For lCount As Integer = 0 To oClaim.ClaimPeril.Count - 1
                        For lInnerCount As Integer = 0 To oClaim.ClaimPeril(lCount).ClaimPayment.Count - 1
                            If Session(CNClaimPaymentKey) = oClaim.ClaimPeril(lCount).ClaimPayment(lInnerCount).BaseClaimPaymentKey Then
                                bFoundPayment = True
                                iPaymentIndex = lInnerCount
                                Exit For
                            End If
                        Next
                        If bFoundPayment Then
                            iPerilIndex = lCount
                            Session(CNClaimPerilIndex) = iPerilIndex
                            Exit For
                        End If
                    Next

                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    If oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).PartyKey <> 0 Then
                        oBankDetails = oWebService.GetPartyBankDetails(oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).PartyKey)
                    End If

                    If oBankDetails IsNot Nothing Then
                        For icount = 0 To oBankDetails.Count - 1
                            'Cash/Cheque Payment
                            If (oBankDetails.Item(icount).BankPaymentTypeCode = "ANY" Or oBankDetails.Item(icount).BankPaymentTypeCode = "RECPAY" Or oBankDetails.Item(icount).BankPaymentTypeCode = "CLM") And oBankDetails.Item(icount).IsActive Then ' Cash/Cheque Payment Type
                                oTempBankDetailsCollection.Add(oBankDetails(icount))
                            End If
                        Next
                    End If

                    If oTempBankDetailsCollection.Count > 0 Then
                        'Sort collection before binding
                        oTempBankDetailsCollection.SortColumn = "AccountType"
                        oTempBankDetailsCollection.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
                        oTempBankDetailsCollection.Sort()

                        'Populate the drop down list
                        liAccountType.Visible = True
                        ddlAccountType.DataTextField = "AccountType"
                        ddlAccountType.DataValueField = "PartyBankKey"
                        ddlAccountType.DataSource = oTempBankDetailsCollection
                        ddlAccountType.DataBind()
                        ddlAccountType.Items.Insert(0, New ListItem("--Select Account Type--", ""))
                    End If

                    Integer.TryParse(oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).Payee.PartyBankKey, iPartyBankKey)

                    'Set Address 
                    If oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).Payee.Address IsNot Nothing Then
                        PayNow_Address.Address1 = Trim(oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).Payee.Address.Address1)
                        PayNow_Address.Address2 = Trim(oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).Payee.Address.Address2)
                        PayNow_Address.Address3 = Trim(oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).Payee.Address.Address3)
                        PayNow_Address.Address4 = Trim(oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).Payee.Address.Address4)
                        PayNow_Address.CountryCode = Trim(oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).Payee.Address.CountryCode)
                        PayNow_Address.Postcode = Trim(oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).Payee.Address.PostCode)
                    End If

                    If oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex) IsNot Nothing Then
                        txtOurReference.Text = oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).OurRef
                    End If

                    If oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).Payee IsNot Nothing Then
                        txtTheirReference.Text = oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).Payee.TheirReference
                        txtMediaReference.Text = oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).Payee.MediaReference

                        ' SET AccountType  Address based on PartyBankKey
                        If oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).Payee.PartyBankKey <> 0 Then
                            ddlAccountType.SelectedValue = oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).Payee.PartyBankKey
                        End If

                        txtAccountCode.Text = oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).Payee.BankNumber
                        txtPayeeName.Text = oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).Payee.Name
                        txtBranchCode.Text = oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).Payee.BankCode
                        'txtReference1.Text = oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).Payee.Address.Address1
                        'txtReference2.Text = oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).Payee.Address.Address2
                        txtBIC.Text = oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).Payee.BIC
                        txtIBAN.Text = oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).Payee.IBAN
                    End If


                    'Disabling the textboxes
                    ddlAccountType.Enabled = False
                    txtAccountCode.Enabled = False
                    txtPayeeName.Enabled = False
                    txtExpiryDate.Enabled = False
                    txtBranchCode.Enabled = False
                    txtReference1.Enabled = False
                    txtReference2.Enabled = False
                    txtBIC.Enabled = False
                    txtIBAN.Enabled = False
                End If
            ElseIf Session(CNClaim) IsNot Nothing And Session("ModeType") Is Nothing And CType(Session(CNMode), Mode) = Mode.PayClaim Then   'Claim Payments
                Dim oPayment As NexusProvider.ClaimPayment = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).Payment
                Dim oClaimReserve As NexusProvider.ClaimPerilReservePaymentTypeCollection = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).ClaimReserve


                If oBankDetails IsNot Nothing Then
                    For icount = 0 To oBankDetails.Count - 1
                        'Cash/Cheque Payment
                        If (oBankDetails.Item(icount).BankPaymentTypeCode = "ANY" Or oBankDetails.Item(icount).BankPaymentTypeCode = "CLM") And oBankDetails.Item(icount).IsActive Then ' Cash/Cheque Payment Type
                            oTempBankDetailsCollection.Add(oBankDetails(icount))
                        End If
                    Next
                End If
                Dim aa As Boolean = CBool(Session("CashListItemFirstLoad"))
                If Not IsPostBack Then
                    aa = True
                End If
                'If ViewState("CashListItemFirstLoad") IsNot Nothing AndAlso CBool(ViewState("CashListItemFirstLoad")) Then
                '    ViewState("CashListItemFirstLoad") = False
                'End If

                If aa OrElse Request.QueryString("Mode") = "AP" Then
                    'If Not IsPostBack Then
                    If oTempBankDetailsCollection.Count > 0 Then
                        'Populate the drop down list
                        liAccountType.Visible = True
                        ddlAccountType.DataTextField = "AccountType"
                        ddlAccountType.DataValueField = "PartyBankKey"
                        ddlAccountType.DataSource = oTempBankDetailsCollection
                        ddlAccountType.DataBind()
                        ddlAccountType.Items.Insert(0, New ListItem("--Select Account Type--", ""))
                    End If
                    If oPayment IsNot Nothing Then
                        txtOurReference.Text = oPayment.OurRef
                    End If

                    If oPayment.Payee IsNot Nothing Then
                        txtTheirReference.Text = oPayment.Payee.TheirReference
                        txtMediaReference.Text = oPayment.Payee.MediaReference
                        txtAccountCode.Text = oPayment.Payee.BankNumber
                        txtPayeeName.Text = oPayment.Payee.Name
                        txtBranchCode.Text = oPayment.Payee.BankCode
                        txtBIC.Text = oPayment.Payee.BIC
                        txtIBAN.Text = oPayment.Payee.IBAN

                        If oPayment.Payee.PartyBankKey <> 0 Then
                            ddlAccountType.SelectedValue = oPayment.Payee.PartyBankKey
                            'Disabling the textboxes
                            txtAccountCode.Enabled = False
                            txtPayeeName.Enabled = False
                            txtExpiryDate.Enabled = False
                            txtBranchCode.Enabled = False
                            txtReference1.Enabled = False
                            txtReference2.Enabled = False
                            txtBIC.Enabled = False
                            txtIBAN.Enabled = False
                        End If

                    End If
                Else
                    Dim iPartyBankKey As Integer
                    Integer.TryParse(ddlAccountType.SelectedValue, iPartyBankKey)
                    If oTempBankDetailsCollection IsNot Nothing AndAlso oTempBankDetailsCollection.Count > 0 Then
                        For iCount As Integer = 0 To oTempBankDetailsCollection.Count - 1
                            If oTempBankDetailsCollection(iCount).PartyBankKey = iPartyBankKey Then
                                ddlAccountType.SelectedValue = iPartyBankKey
                                txtAccountCode.Text = oTempBankDetailsCollection(iCount).AccountNumber
                                txtPayeeName.Text = oTempBankDetailsCollection(iCount).AccountHolderName
                                txtBranchCode.Text = oTempBankDetailsCollection(iCount).BranchCode
                                txtReference1.Text = oTempBankDetailsCollection(iCount).Reference1
                                txtReference2.Text = oTempBankDetailsCollection(iCount).Reference2
                                txtBIC.Text = oTempBankDetailsCollection(iCount).BIC
                                txtIBAN.Text = oTempBankDetailsCollection(iCount).IBAN

                                'Disabling the textboxes
                                txtAccountCode.Enabled = False
                                txtPayeeName.Enabled = False
                                txtExpiryDate.Enabled = False
                                txtBranchCode.Enabled = False
                                txtReference1.Enabled = False
                                txtReference2.Enabled = False
                                txtBIC.Enabled = False
                                txtIBAN.Enabled = False
                            End If
                        Next
                    End If

                End If

                'Integer.TryParse(oClaim.ClaimPeril(iPerilIndex).ClaimPayment(iPaymentIndex).Payee.PartyBankKey, iPartyBankKey)



            Else
                If oBankDetails IsNot Nothing Then
                    For icount = 0 To oBankDetails.Count - 1
                        'Cash/Cheque Payment
                        If Session("ModeType") = "Payment" _
                        Or (Session("ModeValue") = "IP" AndAlso (Session("Type") IsNot Nothing AndAlso Session("Type").Trim() = PaymentType.P.ToString())) _
                        Or Session("CP") = True Or Session(CNMode) = Mode.PayClaim Or Session(CNUnAllocatedClaimPayment) IsNot Nothing _
                        Or (Session("ModeValue") = "IP" AndAlso (Session("Type") IsNot Nothing AndAlso Session("Type").Trim() = PaymentType.CP.ToString())) Then
                            Select Case CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType).CoreCashList.TypeCode
                                Case "CP"
                                    If (oBankDetails.Item(icount).BankPaymentTypeCode = "ANY" Or oBankDetails.Item(icount).BankPaymentTypeCode = "CLM") And oBankDetails.Item(icount).IsActive Then ' Cash/Cheque Payment Type
                                        oTempBankDetailsCollection.Add(oBankDetails(icount))
                                    End If
                                Case Else
                                    If (oBankDetails.Item(icount).BankPaymentTypeCode = "ANY" Or oBankDetails.Item(icount).BankPaymentTypeCode = "RECPAY") And oBankDetails.Item(icount).IsActive Then ' Cash/Cheque Payment Type
                                        oTempBankDetailsCollection.Add(oBankDetails(icount))
                                    End If
                            End Select

                        End If

                        If (oBankDetails.Item(icount).BankPaymentTypeCode = "ANY" OrElse oBankDetails.Item(icount).BankPaymentTypeCode = "RECPAY") And oBankDetails.Item(icount).IsActive AndAlso Session(CNMTAType) = MTAType.CANCELLATION Then
                            If oBankDetails.Item(icount).IsActive Then
                                oTempBankDetailsCollection.Add(oBankDetails(icount))
                            End If
                        End If

                    Next
                End If


                If oTempBankDetailsCollection.Count > 0 Then

                    'Populate the drop down list
                    liAccountType.Visible = True
                    ddlAccountType.DataTextField = "AccountType"
                    ddlAccountType.DataValueField = "PartyBankKey"
                    ddlAccountType.DataSource = oTempBankDetailsCollection
                    ddlAccountType.DataBind()
                    ddlAccountType.Items.Insert(0, New ListItem("--Select Account Type--", ""))
                    ddlAccountType.Enabled = True

                    ' if count is 1 then it should populated by default
                    If oTempBankDetailsCollection.Count = 1 Then
                        'Set first item as selected from dropdown
                        ddlAccountType.SelectedValue = oTempBankDetailsCollection(0).PartyBankKey

                        txtAccountCode.Text = oTempBankDetailsCollection(0).AccountNumber
                        txtPayeeName.Text = oTempBankDetailsCollection(0).AccountHolderName
                        If Not oTempBankDetailsCollection(0).ExpiryDate.ToShortDateString.Contains("1899") AndAlso
                                                         Not oTempBankDetailsCollection(0).ExpiryDate = Date.MinValue Then
                            txtExpiryDate.Text = oTempBankDetailsCollection(0).ExpiryDate
                        Else
                            txtExpiryDate.Text = ""
                        End If
                        txtBranchCode.Text = oTempBankDetailsCollection(0).BranchCode
                        txtReference1.Text = oTempBankDetailsCollection(0).Reference1
                        txtReference2.Text = oTempBankDetailsCollection(0).Reference2
                        txtBIC.Text = oTempBankDetailsCollection(0).BIC
                        txtIBAN.Text = oTempBankDetailsCollection(0).IBAN

                        'Disabling the textboxes
                        txtAccountCode.Enabled = False
                        txtPayeeName.Enabled = False
                        txtExpiryDate.Enabled = False
                        txtBranchCode.Enabled = False
                        txtReference1.Enabled = False
                        txtReference2.Enabled = False
                        txtBIC.Enabled = False
                        txtIBAN.Enabled = False
                    ElseIf Not String.IsNullOrEmpty(CashListItemID.Value()) AndAlso Session(CNUnAllocatedClaimPayment) IsNot Nothing Then

                        Dim iPartyBankKey As Integer
                        Dim oUnallocatedClaimPayments As NexusProvider.UnallocatedClaimPayments = CType(Session(CNUnAllocatedClaimPayment), NexusProvider.UnallocatedClaimPayments)

                        Integer.TryParse(oCashListItem.Bank.BankKey, iPartyBankKey)
                        If (iPartyBankKey = 0) Then
                            iPartyBankKey = oUnallocatedClaimPayments.PartyBankId
                        End If
                        If oTempBankDetailsCollection IsNot Nothing AndAlso oTempBankDetailsCollection.Count > 0 AndAlso iPartyBankKey > 0 Then
                            For iCount As Integer = 0 To oTempBankDetailsCollection.Count - 1
                                If oTempBankDetailsCollection(iCount).PartyBankKey = iPartyBankKey Then
                                    ddlAccountType.SelectedValue = iPartyBankKey
                                    txtAccountCode.Text = oTempBankDetailsCollection(iCount).AccountNumber
                                    txtPayeeName.Text = oTempBankDetailsCollection(iCount).AccountHolderName
                                    txtBranchCode.Text = oTempBankDetailsCollection(iCount).BranchCode
                                    txtReference1.Text = oTempBankDetailsCollection(iCount).Reference1
                                    txtReference2.Text = oTempBankDetailsCollection(iCount).Reference2
                                    txtBIC.Text = oTempBankDetailsCollection(iCount).BIC
                                    txtIBAN.Text = oTempBankDetailsCollection(iCount).IBAN
                                    If Not oTempBankDetailsCollection(iCount).ExpiryDate.ToShortDateString.Contains("1899") AndAlso
                                                        Not oTempBankDetailsCollection(iCount).ExpiryDate = Date.MinValue Then
                                        txtExpiryDate.Text = oTempBankDetailsCollection(iCount).ExpiryDate
                                    Else
                                        txtExpiryDate.Text = ""
                                    End If
                                    GISLookup_MediaType.SelectedValue = oUnallocatedClaimPayments.MediaTypeCode.Trim
                                    'Disabling the textboxes
                                    txtAccountCode.Enabled = False
                                    txtPayeeName.Enabled = False
                                    txtExpiryDate.Enabled = False
                                    txtBranchCode.Enabled = False
                                    txtReference1.Enabled = False
                                    txtReference2.Enabled = False
                                End If
                            Next
                        Else
                            txtAccountCode.Text = oUnallocatedClaimPayments.PayeeAccountNo
                            txtPayeeName.Text = oUnallocatedClaimPayments.PayeeName
                            txtBranchCode.Text = oUnallocatedClaimPayments.PayeeShortCode
                            GISLookup_MediaType.SelectedValue = oUnallocatedClaimPayments.MediaTypeCode.Trim
                            'Disabling the textboxes
                            txtAccountCode.Enabled = False
                            txtPayeeName.Enabled = False
                            txtExpiryDate.Enabled = False
                            txtBranchCode.Enabled = False
                            txtReference1.Enabled = False
                            txtReference2.Enabled = False
                        End If

                    End If
                ElseIf Session(CNUnAllocatedClaimPayment) IsNot Nothing Then
                    ddlAccountType.Items.Clear()
                    ddlAccountType.Items.Insert(0, New ListItem("--Select Account Type--", ""))
                    ddlAccountType.Enabled = False
                Else
                    ddlAccountType.Items.Clear()
                    ddlAccountType.Items.Insert(0, New ListItem("--Select Account Type--", ""))
                    ddlAccountType.Enabled = False
                    txtAccountCode.Enabled = True
                    txtPayeeName.Enabled = True
                    txtExpiryDate.Enabled = True
                    txtBranchCode.Enabled = True
                    txtReference1.Enabled = True
                    txtReference2.Enabled = True
                    txtBIC.Enabled = True
                    txtIBAN.Enabled = True
                    txtAccountCode.Text = ""
                    txtPayeeName.Text = ""
                    txtExpiryDate.Text = ""
                    txtBranchCode.Text = ""
                    txtReference1.Text = ""
                    txtReference2.Text = ""
                    txtBIC.Text = ""
                    txtIBAN.Text = ""
                End If
                'storing in cache
                Cache.Insert(ViewState("PartyBankpageCacheID"), oTempBankDetailsCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                'Cleaning Up
                oBankDetails = Nothing
                oTempBankDetails = Nothing
                oTempBankDetailsCollection = Nothing
            End If

        End Sub

        Protected Sub ddlAccountType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAccountType.SelectedIndexChanged
            If ddlAccountType.SelectedValue <> "" Then


                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oParty As NexusProvider.BaseParty = CType(Session(CNParty), NexusProvider.BaseParty)
                Dim oBankCollection As NexusProvider.BankCollection = CType(Cache.Item(ViewState("PartyBankpageCacheID")), NexusProvider.BankCollection)

                If oBankCollection Is Nothing Then
                    DisplayAccountTypeInformation()
                    oBankCollection = CType(Cache.Item(ViewState("PartyBankpageCacheID")), NexusProvider.BankCollection)
                End If
                If ddlAccountType.SelectedValue <> "" Then 'If oBankCollection Is Nothing, ddlAccountType is reset
                    If oBankCollection IsNot Nothing Then
                        With oBankCollection
                            For i = 0 To oBankCollection.Count - 1
                                If .Item(i).PartyBankKey = ddlAccountType.SelectedValue Then
                                    txtAccountCode.Text = .Item(i).AccountNumber
                                    txtPayeeName.Text = .Item(i).AccountHolderName
                                    If Not .Item(i).ExpiryDate.ToShortDateString.Contains("1899") AndAlso
                                                     Not .Item(i).ExpiryDate = Date.MinValue Then
                                        txtExpiryDate.Text = .Item(i).ExpiryDate
                                    Else
                                        txtExpiryDate.Text = ""
                                    End If
                                    txtBranchCode.Text = .Item(i).BranchCode
                                    txtReference1.Text = .Item(i).Reference1
                                    txtReference2.Text = .Item(i).Reference2
                                    txtBIC.Text = .Item(i).BIC
                                    txtIBAN.Text = .Item(i).IBAN
                                    hidBankCode.Value = .Item(i).BankCode

                                    'Disabling the textboxes
                                    txtAccountCode.Enabled = False
                                    txtPayeeName.Enabled = False
                                    txtExpiryDate.Enabled = False
                                    txtBranchCode.Enabled = False
                                    txtReference1.Enabled = False
                                    txtReference2.Enabled = False
                                    txtBIC.Enabled = False
                                    txtIBAN.Enabled = False
                                    Exit For
                                Else
                                    'Enabling the textboxes
                                    txtAccountCode.Enabled = True
                                    txtPayeeName.Enabled = True
                                    txtExpiryDate.Enabled = True
                                    txtBranchCode.Enabled = True
                                    txtReference1.Enabled = True
                                    txtReference2.Enabled = True
                                    txtBIC.Enabled = True
                                    txtIBAN.Enabled = True

                                    txtAccountCode.Text = String.Empty
                                    txtPayeeName.Text = String.Empty
                                    txtExpiryDate.Text = String.Empty
                                    txtBranchCode.Text = String.Empty
                                    txtReference1.Text = String.Empty
                                    txtReference2.Text = String.Empty
                                    txtBIC.Text = String.Empty
                                    txtIBAN.Text = String.Empty
                                    hidBankCode.Value = String.Empty
                                End If
                            Next
                        End With
                    End If
                End If
            End If
            If ddlAccountType.SelectedValue = "" Then
                'Enabling the textboxes
                txtAccountCode.Enabled = True
                txtPayeeName.Enabled = True
                txtExpiryDate.Enabled = True
                txtBranchCode.Enabled = True
                txtReference1.Enabled = True
                txtReference2.Enabled = True
                txtBIC.Enabled = True
                txtIBAN.Enabled = True

                txtAccountCode.Text = String.Empty
                txtPayeeName.Text = String.Empty
                txtExpiryDate.Text = String.Empty
                txtBranchCode.Text = String.Empty
                txtReference1.Text = String.Empty
                txtReference2.Text = String.Empty
                txtBIC.Text = String.Empty
                txtIBAN.Text = String.Empty
            End If
        End Sub
        Sub SkipSummaryPage()
            Dim sDisplayReinsurance As String
            oWebservice = New NexusProvider.ProviderManager().Provider
            sDisplayReinsurance = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.Description, NexusProvider.RiskTypeOptions.DisplayClaimReinsurance, CType(Session(CNClaimQuote), NexusProvider.Quote).ProductCode, CType(Session(CNClaimQuote), NexusProvider.Quote).Risks(0).RiskTypeCode)
            'Check the User Authority to display of Reinsurance
            Dim oUserAuthority As New NexusProvider.UserAuthority
            oUserAuthority.UserCode = Session(CNLoginName)
            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.DisplayClaimReinsurance
            oWebservice.GetUserAuthorityValue(oUserAuthority)
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim aa As Boolean = CBool(Session("CashListItemFirstLoad"))
            If Not IsPostBack Then
                aa = True
            End If
            'If ViewState("CashListItemFirstLoad") IsNot Nothing AndAlso CBool(ViewState("CashListItemFirstLoad")) Then
            '    ViewState("CashListItemFirstLoad") = False
            'End If

            If aa Then
                'If Not IsPostBack Then
                If Session(CNMode) = Mode.PayClaim AndAlso (sDisplayReinsurance = "0" Or oUserAuthority.UserAuthorityValue = "0") Then
                    Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue
                    Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)

                    'get the Claim Workflow Setting
                    oRunClaimWorkFlow = oWebservice.GetProductClaimsWorkflowOptions(NexusProvider.ClaimProcessType.ClaimPayment, oQuote.ProductCode)

                    If oRunClaimWorkFlow IsNot Nothing AndAlso oRunClaimWorkFlow.MakeFurtherPayments = True Then
                        Dim oPerilColl As NexusProvider.PerilCollection = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril

                        If oRunClaimWorkFlow.MakeFurtherPayments = True Then
                            'Check the reserve amount vs Payment amount
                            Dim bStatus As Boolean = False
                            For j As Integer = 0 To oPerilColl.Count - 1
                                For i As Integer = 0 To oPerilColl(j).ClaimReserve.Count - 1
                                    If oPerilColl(j).ClaimReserve(i).CurrentReserve <= 0 Then
                                        bStatus = True
                                    Else
                                        bStatus = False
                                        Exit For
                                    End If
                                Next
                                If bStatus = False Then
                                    Exit For
                                End If
                            Next
                            If bStatus = False Then
                                ' btnOk.Attributes.Add("onclick", "javascript:return PaymentConfirmation();")
                            Else
                                hidChkPaymentMsg.Value = "0"
                                'If this value is set then paymentconfirmatio will be called from
                                'ClamCloseConfirmation javascript function
                                'btnOk.Attributes.Add("onclick", "javascript:return ClaimCloseConfirmation();")
                            End If
                        ElseIf oRunClaimWorkFlow.MakeFurtherPayments = False Then
                            'Check the reserve amount vs Payment amount
                            Dim bFound As Boolean = False
                            For j As Integer = 0 To oPerilColl.Count - 1
                                For i As Integer = 0 To oPerilColl(j).ClaimReserve.Count - 1
                                    If oPerilColl(j).ClaimReserve(i).CurrentReserve <= 0 Then
                                        bFound = True
                                    Else
                                        bFound = False
                                        Exit For
                                    End If
                                Next
                                If bFound = False Then
                                    Exit For
                                End If
                            Next
                            If bFound = True Then
                                hidChkPaymentMsg.Value = String.Empty
                                ' btnOk.Attributes.Add("onclick", "javascript:return ClaimCloseConfirmation();")
                            End If
                        End If
                    End If
                End If
            Else
                'if Finish or Next button has hit
                Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
                Dim sBranchCode As String = oQuote.BranchCode
                Dim oClaimDetails As NexusProvider.ClaimOpen = Session.Item(CNClaim)
                Dim bClaimTimeStamp() As Byte = Session.Item(CNClaimTimeStamp)
                Dim oclaimRisk As New NexusProvider.ClaimRisk
                Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oOriginalClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
                Dim oRunClaimWorkFlow As NexusProvider.ProductClaimsWorkflowOptionsValue
                Dim oReturnCode As Object
                'IF display reinsurance is ON 

                'Claim Risk has wrong argument called ClaimKey, actually it is BaseClaimKey
                oclaimRisk.ClaimKey = oClaimDetails.BaseClaimKey
                oclaimRisk.TimeStamp = Session.Item(CNClaimRiskTimeStamp)
                oclaimRisk.XMLDataSet = Session.Item(CNDataSet)

                If Session(CNMode) = Mode.NewClaim Then
                    Dim bClaimBuilder As Boolean
                    Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
                    If bClaimBuilder = True Then
                        'Update the claim builder risk screen
                        'arch issue 268
                        oReturnCode = UpdateClaimRiskCall(oclaimRisk, sBranchCode)
                        If oReturnCode Is Nothing Then
                            Exit Sub
                        End If
                    End If

                    If sDisplayReinsurance = "1" AndAlso oUserAuthority.UserAuthorityValue = "1" AndAlso oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaimDetails.RiskType)).DisplayClaimReinsurance = True Then
                        Response.Redirect("~/claims/ClaimReinsurance.aspx", False)
                    Else
                        'Fire the Bind Claim
                        'oWebservice.BindClaim(oOriginalClaim, bClaimTimeStamp, 1, Nothing, sBranchCode)
                        If BindClaimCall(oOriginalClaim, bClaimTimeStamp, 1, Nothing, sBranchCode) Is Nothing Then
                            Exit Sub
                        End If
                    End If


                ElseIf Session(CNMode) = Mode.EditClaim Then
                    Dim bClaimBuilder As Boolean
                    Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
                    If bClaimBuilder = True Then
                        'Update the claim builder risk screen
                        oReturnCode = UpdateClaimRiskCall(oclaimRisk, sBranchCode)
                    End If

                    If sDisplayReinsurance = "1" AndAlso oUserAuthority.UserAuthorityValue = "1" AndAlso oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaimDetails.RiskType)).DisplayClaimReinsurance = True Then
                        Response.Redirect("~/claims/ClaimReinsurance.aspx", False)
                    Else
                        'Fire the Bind Claim
                        'arch issue 268
                        If BindClaimCall(oOriginalClaim, bClaimTimeStamp, 2, Nothing, sBranchCode) Is Nothing Then
                            Exit Sub
                        End If
                    End If

                ElseIf Session(CNMode) = Mode.PayClaim Then
                    'Fire the Bind Claim
                    Dim oPayment As NexusProvider.ClaimPayment = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).Payment
                    'Need to close the claim if Payment Amount is exceeding or equal to reserve amount for any peril
                    If hidChlClaimClose.Value.Trim.ToUpper = "TRUE" Then
                        oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = True
                        oPayment.CloseClaimOnZeroReserveRecoveryBalance = True
                    Else
                        oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = False
                        oPayment.CloseClaimOnZeroReserveRecoveryBalance = False
                    End If

                    Try
                        Dim bClaimBuilder As Boolean
                        Boolean.TryParse(Session(CNClaimBuilder), bClaimBuilder)
                        If bClaimBuilder = True Then
                            'Update the claim builder risk screen
                            If Not UpdateClaimRiskCall(oclaimRisk, sBranchCode) Then
                                Exit Sub
                            End If
                        End If

                        If sDisplayReinsurance = "1" AndAlso oUserAuthority.UserAuthorityValue = "1" AndAlso oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.RiskTypes.RiskType(Trim(oClaimDetails.RiskType)).DisplayClaimReinsurance = True Then
                            Response.Redirect("~/claims/ClaimReinsurance.aspx", False)
                        Else
                            If Session(CNMode) = Mode.PayClaim AndAlso (Session(CNLockPaymentGrid) IsNot Nothing AndAlso Session(CNLockPaymentGrid) = True) Then
                                'Fire the Bind Claim
                                If BindClaimCall(oOriginalClaim, bClaimTimeStamp, 4, oPayment, sBranchCode) Is Nothing Then
                                    Exit Sub
                                End If
                            Else
                                'Fire the Bind Claim
                                If BindClaimCall(oOriginalClaim, bClaimTimeStamp, 3, oPayment, sBranchCode) Is Nothing Then
                                    Exit Sub
                                End If
                            End If
                        End If

                        'Check to move to the accept another payment
                        oRunClaimWorkFlow = ViewState("RunClaimWorkFlow")

                        If oRunClaimWorkFlow Is Nothing Then
                            'get the Claim Workflow Setting
                            oRunClaimWorkFlow = oWebservice.GetProductClaimsWorkflowOptions(NexusProvider.ClaimProcessType.ClaimPayment, oQuote.ProductCode)
                            ViewState("RunClaimWorkFlow") = oRunClaimWorkFlow
                        End If

                        If oRunClaimWorkFlow.MakeFurtherPayments = True And hidChkChoice.Value.Trim.ToUpper = "TRUE" Then
                            GetLatestDetails() 'Update the session with latest values
                            Session.Remove(CNEnablePayClaim) 'Remove the ReadOnly mode of the Pay Claim
                            Session(CNPayClaim) = Nothing ' Reset the pay claim to accept the another payment
                            'Dummy Cal of PayClaim, to retreive back the latetst claim key
                            PayClaimWithZeroAmount()
                            Dim sUrl As String = CheckClaimBuilder()
                            Response.Redirect(sUrl, False)
                        Else
                            Response.Redirect("~/Claims/Complete.aspx", False)
                        End If

                    Catch ex As NexusProvider.NexusException
                        If ex.Errors(0).Code = "330" Then   'Code : 330 :: Description: DebtorUserGroupsAreNotSetup

                            Dim cstDebtorUserGroups As New CustomValidator
                            cstDebtorUserGroups.IsValid = False
                            'look for a validation message in the page resources, but if there is not one defined add a default message
                            cstDebtorUserGroups.ErrorMessage = IIf(GetLocalResourceObject("cstDebtorUserGroups") Is Nothing, "Cannot Proceed- Debtor User Groups are not setup. Please contact your system administrator.", GetLocalResourceObject("cstDebtorUserGroups"))
                            cstDebtorUserGroups.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                            'add the validator to the page, this will have the effect of making the page invalid
                            Page.Validators.Add(cstDebtorUserGroups)
                            Exit Sub
                        ElseIf ex.Errors(0).Code = "331" Then   'Code : 331 :: Description: DebtorUserGroupsAreNotSetup

                            Dim cstDebtorUserGroups As New CustomValidator
                            cstDebtorUserGroups.IsValid = False
                            'look for a validation message in the page resources, but if there is not one defined add a default message
                            cstDebtorUserGroups.ErrorMessage = IIf(GetLocalResourceObject("cstDebtorUserGroups") Is Nothing, "Cannot Proceed- Debtor User Groups are not setup. Please contact your system administrator.", GetLocalResourceObject("cstDebtorUserGroups"))
                            cstDebtorUserGroups.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                            'add the validator to the page, this will have the effect of making the page invalid
                            Page.Validators.Add(cstDebtorUserGroups)
                            Exit Sub
                        Else
                            Throw
                        End If
                    End Try
                ElseIf Session(CNMode) = Mode.ViewClaim Then
                    Response.Redirect("~/claims/complete.aspx", False)
                End If

                'Update the Claims session variable
                GetClaimDetails(oClaimDetails.ClaimKey, oclaimRisk)
                Response.Redirect("~/claims/complete.aspx", False)

            End If
        End Sub
        Sub GetClaimDetails(ByVal v_iClaimKey As Integer, ByVal oClaimRisk As NexusProvider.ClaimRisk)
            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oOriginalClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim sBranchCode As String = oQuote.BranchCode
            'Retreiving the latest details
            'arch issue 268
            oClaimDetails = GetClaimDetailsCall(v_iClaimKey, sBranchCode)
            'updation of latest session values 
            Session.Item(CNClaimTimeStamp) = oClaimDetails.TimeStamp
            Session.Item(CNClaimRiskTimeStamp) = oClaimRisk.TimeStamp
            Session.Item(CNBaseClaimKey) = oClaimDetails.BaseClaimKey
            Session.Item(CNClaimKey) = oClaimDetails.ClaimKey
            Session.Item(CNClaimNumber) = oClaimDetails.ClaimNumber
            Session.Item(CNDataSet) = oClaimRisk.XMLDataSet
            With oClaimDetails
                oOriginalClaim.CatastropheCode = .CatastropheCode
                oOriginalClaim.BaseClaimKey = .BaseClaimKey
                oOriginalClaim.Claim = .Claim
                oOriginalClaim.ClaimCoInsurer = .ClaimCoInsurer
                oOriginalClaim.ClaimDescription = .ClaimDescription
                oOriginalClaim.ClaimHandlerDescription = .ClaimHandlerDescription
                oOriginalClaim.ClaimKey = .ClaimKey
                oOriginalClaim.ClaimNumber = .ClaimNumber
                oOriginalClaim.ClaimPeril = .ClaimPeril
                oOriginalClaim.ClaimStatus = .ClaimStatus
                oOriginalClaim.ClaimStatusDate = .ClaimStatusDate
                oOriginalClaim.ClaimStatusID = .ClaimStatusID
                oOriginalClaim.ClaimVersion = .ClaimVersion
                oOriginalClaim.ClaimVersionDescription = .ClaimVersionDescription
                oOriginalClaim.ClientClaimNumber = .ClientClaimNumber
                oOriginalClaim.ClientEmail = .ClientEmail
                oOriginalClaim.ClientFaxNo = .ClientFaxNo
                oOriginalClaim.ClientMobileNo = .ClientMobileNo
                oOriginalClaim.ClientName = .ClientName
                oOriginalClaim.ClientShortName = .ClientShortName
                oOriginalClaim.ClientTelNo = .ClientTelNo
                oOriginalClaim.ClientTelNoOff = .ClientTelNoOff
                oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = .CloseClaimOnZeroReserveRecoveryBalance
                oOriginalClaim.Comments = .Comments
                oOriginalClaim.Contact = .Contact
                oOriginalClaim.CurrencyISOCode = .CurrencyISOCode
                oOriginalClaim.Description = .Description
                oOriginalClaim.ExternalHandler = .ExternalHandler
                oOriginalClaim.HandlerCode = .HandlerCode
                oOriginalClaim.IgnoreClaimMaintain = .IgnoreClaimMaintain
                oOriginalClaim.InfoOnly = .InfoOnly
                oOriginalClaim.InsuranceFileKey = .InsuranceFileKey
                oOriginalClaim.InsuranceRef = .InsuranceRef
                oOriginalClaim.InsurerClaimNumber = .InsurerClaimNumber
                oOriginalClaim.IsAllowedClosedClaims = .IsAllowedClosedClaims
                oOriginalClaim.IsDeleted = .IsDeleted
                oOriginalClaim.LastModifiedDate = .LastModifiedDate
                oOriginalClaim.LikelyClaim = .LikelyClaim
                oOriginalClaim.Location = .Location
                oOriginalClaim.LossDate = .LossDate
                oOriginalClaim.LossDateFrom = .LossDateFrom
                oOriginalClaim.LossFromDate = .LossToDate
                oOriginalClaim.LossToDate = .LossToDate
                oOriginalClaim.LossToDateSpecified = .LossToDateSpecified
                oOriginalClaim.Payments = .Payments
                oOriginalClaim.PolicyNumber = .PolicyNumber
                oOriginalClaim.PolicyType = .PolicyType
                oOriginalClaim.PrimaryCause = .PrimaryCause
                oOriginalClaim.PrimaryCauseCode = .PrimaryCauseCode
                oOriginalClaim.PrimaryCauseDescription = .PrimaryCauseDescription
                oOriginalClaim.ProductDescription = .ProductDescription
                oOriginalClaim.ProgressStatusCode = .ProgressStatusCode
                oOriginalClaim.ProgressStatusDescription = .ProgressStatusDescription
                oOriginalClaim.ReportedDate = .ReportedDate
                oOriginalClaim.RiskKey = .RiskKey
                oOriginalClaim.SecondaryCause = .SecondaryCause
                oOriginalClaim.SecondaryCauseCode = .SecondaryCauseCode
                oOriginalClaim.SecondaryCauseDescription = .SecondaryCauseDescription
                oOriginalClaim.TotalCurrentShareValue = .TotalCurrentShareValue
                oOriginalClaim.TotalShare = .TotalShare
                oOriginalClaim.Town = .Town
                oOriginalClaim.TownCode = .TownCode
                oOriginalClaim.UnderwritingYearCode = .UnderwritingYearCode
                oOriginalClaim.UserDefFldACode = .UserDefFldACode
                oOriginalClaim.UserDefFldBCode = .UserDefFldBCode
                oOriginalClaim.UserDefFldCCode = .UserDefFldCCode
                oOriginalClaim.UserDefFldDCode = .UserDefFldECode
                oOriginalClaim.UserDefFldECode = .UserDefFldECode
            End With
            Session.Item(CNClaim) = oOriginalClaim
        End Sub
        Sub GetLatestDetails()
            'Latest Claim Details
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oClaimVersions As NexusProvider.VersionsCollections
            Dim iHighest As Integer = 0
            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim oOpenClaim As New NexusProvider.ClaimOpen
            Dim oClaimRisk As NexusProvider.ClaimRisk = Nothing
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim sBranchCode As String = oQuote.BranchCode

            oClaimVersions = oWebservice.GetVersionsForClaim(Session(CNClaimNumber), sBranchCode)

            'Clear the session Variable
            ClearClaims()

            If oClaimVersions IsNot Nothing Then
                'Find Highest Version
                For iCount As Integer = 0 To oClaimVersions.Count - 1
                    If oClaimVersions(iCount).Version > iHighest Then
                        iHighest = oClaimVersions(iCount).Version
                    End If
                Next
            End If
            For iCount As Integer = 0 To oClaimVersions.Count - 1
                If oClaimVersions(iCount).Version = iHighest Then

                    'arch issue 268
                    oClaimDetails = GetClaimDetailsCall(oClaimVersions(iCount).ClaimKey, sBranchCode)

                    With oClaimDetails
                        oOpenClaim.CatastropheCode = .CatastropheCode
                        oOpenClaim.BaseClaimKey = .BaseClaimKey
                        oOpenClaim.Claim = .Claim
                        oOpenClaim.ClaimCoInsurer = .ClaimCoInsurer
                        oOpenClaim.ClaimDescription = .ClaimDescription
                        oOpenClaim.ClaimHandlerDescription = .ClaimHandlerDescription
                        oOpenClaim.ClaimKey = .ClaimKey
                        oOpenClaim.ClaimNumber = .ClaimNumber
                        oOpenClaim.ClaimPeril = .ClaimPeril
                        oOpenClaim.ClaimStatus = .ClaimStatus
                        oOpenClaim.ClaimStatusDate = .ClaimStatusDate
                        oOpenClaim.ClaimStatusID = .ClaimStatusID
                        oOpenClaim.ClaimVersion = .ClaimVersion
                        oOpenClaim.ClaimVersionDescription = .ClaimVersionDescription
                        oOpenClaim.ClientClaimNumber = .ClientClaimNumber
                        oOpenClaim.ClientEmail = .ClientEmail
                        oOpenClaim.ClientFaxNo = .ClientFaxNo
                        oOpenClaim.ClientMobileNo = .ClientMobileNo
                        oOpenClaim.ClientName = .ClientName
                        oOpenClaim.ClientShortName = oClaimVersions(0).ClientShortName 'IIf(.ClientShortName <> String.Empty, .ClientShortName, Trim(lblClientCode.Text))
                        oOpenClaim.ClientTelNo = .ClientTelNo
                        oOpenClaim.ClientTelNoOff = .ClientTelNoOff
                        oOpenClaim.CloseClaimOnZeroReserveRecoveryBalance = .CloseClaimOnZeroReserveRecoveryBalance
                        oOpenClaim.Comments = .Comments
                        oOpenClaim.Contact = .Contact
                        oOpenClaim.CurrencyISOCode = .CurrencyCode
                        oOpenClaim.Description = .Description
                        oOpenClaim.ExternalHandler = .ExternalHandler
                        oOpenClaim.HandlerCode = .HandlerCode
                        oOpenClaim.IgnoreClaimMaintain = .IgnoreClaimMaintain
                        oOpenClaim.InfoOnly = .InfoOnly
                        oOpenClaim.InsuranceFileKey = .InsuranceFileKey
                        oOpenClaim.InsuranceRef = .InsuranceRef
                        oOpenClaim.InsurerClaimNumber = .InsurerClaimNumber
                        oOpenClaim.IsAllowedClosedClaims = .IsAllowedClosedClaims
                        oOpenClaim.IsDeleted = .IsDeleted
                        oOpenClaim.LastModifiedDate = .LastModifiedDate
                        oOpenClaim.LikelyClaim = .LikelyClaim
                        oOpenClaim.Location = .Location
                        oOpenClaim.LossDate = .LossDate
                        oOpenClaim.LossDateFrom = .LossDateFrom
                        oOpenClaim.LossFromDate = .LossToDate
                        oOpenClaim.LossToDate = .LossToDate
                        oOpenClaim.LossToDateSpecified = .LossToDateSpecified
                        oOpenClaim.Payments = .Payments
                        oOpenClaim.PolicyNumber = .PolicyNumber
                        oOpenClaim.PolicyType = .PolicyType
                        oOpenClaim.PrimaryCause = .PrimaryCause
                        oOpenClaim.PrimaryCauseCode = .PrimaryCauseCode
                        oOpenClaim.PrimaryCauseDescription = .PrimaryCauseDescription
                        oOpenClaim.ProductDescription = .ProductDescription
                        oOpenClaim.ProgressStatusCode = .ProgressStatusCode
                        oOpenClaim.ProgressStatusDescription = .ProgressStatusDescription
                        oOpenClaim.ReportedDate = .ReportedDate
                        oOpenClaim.Reserve = .Reserve
                        oOpenClaim.RiskKey = .RiskKey
                        oOpenClaim.RiskType = CType(Session(CNClaimQuote), NexusProvider.Quote).Risks.FindItemByRiskKey(.RiskKey).RiskTypeCode
                        oOpenClaim.RiskTypeDescription = CType(Session(CNClaimQuote), NexusProvider.Quote).Risks.FindItemByRiskKey(.RiskKey).Description
                        oOpenClaim.SecondaryCause = .SecondaryCause
                        oOpenClaim.SecondaryCauseCode = .SecondaryCauseCode
                        oOpenClaim.SecondaryCauseDescription = .SecondaryCauseDescription
                        oOpenClaim.TotalCurrentShareValue = .TotalCurrentShareValue
                        oOpenClaim.TotalShare = .TotalShare
                        oOpenClaim.Town = .Town
                        oOpenClaim.TownCode = .TownCode
                        oOpenClaim.UnderwritingYearCode = .UnderwritingYearCode
                        oOpenClaim.UserDefFldACode = .UserDefFldACode
                        oOpenClaim.UserDefFldBCode = .UserDefFldBCode
                        oOpenClaim.UserDefFldCCode = .UserDefFldCCode
                        oOpenClaim.UserDefFldDCode = .UserDefFldECode
                        oOpenClaim.UserDefFldECode = .UserDefFldECode
                        'Added for Insurer
                        oOpenClaim.Insurer = .Insurer
                        Session.Item(CNClaimTimeStamp) = .TimeStamp
                        oOpenClaim.CurrencyISOCode = .CurrencyCode
                        Session.Item(CNCurrenyCode) = Trim(.CurrencyCode) 'Changed
                        oOpenClaim.Client = .Client
                        'Session(CNInsurer_Header) = .ClientName
                        Session(CNClaimNumber) = .ClaimNumber
                        Session(CNStatus) = .ClaimStatus

                        If Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.ViewClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                            'Arch issue 268
                            oClaimRisk = GetClaimRiskCall(.BaseClaimKey, .ClaimKey, sBranchCode)
                            Session(CNDataSet) = oClaimRisk.XMLDataSet
                        End If
                    End With
                End If
            Next
            Session(CNClaim) = oOpenClaim
        End Sub
        Protected Sub cstUserAuthoriseLimit_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cstUserAuthoriseLimit.ServerValidate
            If txtAmount.Text <> "" AndAlso txtLimit.Value <> "" Then
                If (Convert.ToDecimal(txtAmount.Text) < Convert.ToDecimal(txtLimit.Value)) AndAlso hidHasPaymentsAuthority.Value = "1" Then
                    args.IsValid = True
                Else
                    args.IsValid = False
                End If
            End If
        End Sub

        ''' <summary>
        ''' Amount is validate to be non-zero and has to be a numeric value.
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="args"></param>
        ''' <remarks></remarks>
        Protected Sub custvltxtAmount_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles custvltxtAmount.ServerValidate
            If IsNumeric(txtAmount.Text) Then
                If CType(txtAmount.Text.Trim, Decimal) > 0.0 Then
                    args.IsValid = True
                Else
                    args.IsValid = False
                End If
            Else
                args.IsValid = False
            End If
        End Sub
        ''' <summary>
        ''' CVV number is validated against non zero and numeric value
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="args"></param>
        ''' <remarks></remarks>
        Protected Sub cstVldCVV_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cstVldCVV.ServerValidate
            If txtPin.Text <> "" Then
                If ((CType(txtPin.Text.Trim, Decimal) < 0.0) OrElse (txtPin.Text.Length < 3) OrElse (txtPin.Text.Length > 3) OrElse txtPin.Text.Contains(".")) Then
                    args.IsValid = False
                Else
                    args.IsValid = True
                End If
            End If
        End Sub


        Protected Sub btnDecline_Click(sender As Object, e As EventArgs) Handles btnDecline.Click
            btnRefresh_Click(sender, e)
            Response.Redirect("~/secure/PaymentAuthorizationView.aspx?Mode=" & Session("ModeValue") & "&CashListItemKey=" & Request.QueryString("CashListItemKey"))

        End Sub
        Protected Sub custvldExpiryDateForPayment_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles custvldExpiryDateForPayment.ServerValidate


            'Expiry Date
            Dim iMonth, iYear As Integer
            If (txtExpiryDate.Text) <> "" Then
                iMonth = txtExpiryDate.Text.Trim.Split("/")(0)
                iYear = txtExpiryDate.Text.Trim.Split("/")(1)
                If iMonth <= 0 Or iMonth > 12 Then
                    'If Month is Invalid
                    args.IsValid = False
                    custvldExpiryDateForPayment.ErrorMessage = GetLocalResourceObject("lbl_InvalidExpiryDate")
                End If
            End If


        End Sub
        ''' <summary>
        ''' To check media type validation
        ''' </summary>
        ''' <param name="sColumnToValidate"></param>
        ''' <param name="r_sValue"></param>
        ''' <remarks></remarks>
        Sub CheckMediaTypeDetails(ByVal sColumnToValidate As String, ByRef r_sValue As String)

            If ViewState("MediaTypeDetails") IsNot Nothing Then
                Dim oMediaTypeDetails As New Hashtable()

                oMediaTypeDetails = CType(ViewState("MediaTypeDetails"), Hashtable)

                If oMediaTypeDetails.ContainsKey(Trim(GISLookup_MediaType.SelectedValue) + "|" + sColumnToValidate) = True Then
                    For Each hData As DictionaryEntry In oMediaTypeDetails
                        If hData.Key = Trim(GISLookup_MediaType.SelectedValue) + "|" + sColumnToValidate Then
                            r_sValue = CStr(hData.Value)
                            Exit For
                        End If
                    Next
                End If

            End If
        End Sub
        Function IsInstamentForReceiptType() As Boolean
            oWebservice = New NexusProvider.ProviderManager().Provider
            Dim oList As NexusProvider.LookupListCollection
            Dim v_sOptionList As System.Xml.XmlElement = Nothing
            Try
                oList = oWebservice.GetList(NexusProvider.ListType.PMLookup, "CashListItem_Receipt_Type", True, False, "is_instalment", 1)
                If oList IsNot Nothing AndAlso oList.Count > 0 Then
                    For Each oListItem As NexusProvider.LookupListItem In oList
                        If GISLookup_ReceiptType.Value = oListItem.Code Then
                            Return True
                        End If
                    Next
                End If
                Return False
            Catch ex As Exception
                Throw
            Finally
                oList = Nothing
            End Try


        End Function



        Public Sub FindPremiumFinancePlans()
            Dim oInstalment As New BaseInstalment
            Dim planCount As Integer = 0
            Dim financePlanDetail As NexusProvider.FinancePlanCollection
            oInstalment.GetPremiumFinancePlan(hPartyKey.Value, "040", Session(CNBranchCode).ToString())
            If CType(Session("FinancePlans"), NexusProvider.FinancePlanCollection) IsNot Nothing AndAlso CType(Session("FinancePlans"), NexusProvider.FinancePlanCollection).Count > 0 Then
                financePlanDetail = CType(Session("FinancePlans"), NexusProvider.FinancePlanCollection)
                Dim oFinancePlanFilteredCollection As New NexusProvider.FinancePlanCollection
                Dim oFinancePlanFilteredRow As NexusProvider.FinancePlan
                For Each oFinancePlanDetailrow As NexusProvider.FinancePlan In financePlanDetail
                    oFinancePlanFilteredRow = New NexusProvider.FinancePlan
                    oFinancePlanFilteredRow.FinancePlanKey = oFinancePlanDetailrow.FinancePlanKey
                    oFinancePlanFilteredRow.FinancePlanVersion = oFinancePlanDetailrow.FinancePlanVersion
                    oFinancePlanFilteredRow.FinanceAmount = oFinancePlanDetailrow.FinanceAmount
                    oFinancePlanFilteredRow.FirstInstalmentAmount = oFinancePlanDetailrow.FirstInstalmentAmount
                    oFinancePlanFilteredRow.Frequency = oFinancePlanDetailrow.Frequency
                    oFinancePlanFilteredRow.FirstInstalmentDate = oFinancePlanDetailrow.FirstInstalmentDate
                    oFinancePlanFilteredRow.InsuranceRef = oFinancePlanDetailrow.FinancePlanKey & " - (" & oFinancePlanDetailrow.InsuranceRef & ")"
                    oFinancePlanFilteredCollection.Add(oFinancePlanFilteredRow)
                Next
                If oFinancePlanFilteredCollection.Count > 0 Then
                    ddlInstalmentPlan.DataTextField = "InsuranceRef"
                    ddlInstalmentPlan.DataValueField = "FinancePlanKey"
                    ddlInstalmentPlan.DataMember = "FinancePlanVersion"
                    ddlInstalmentPlan.DataSource = oFinancePlanFilteredCollection
                    ddlInstalmentPlan.DataBind()
                    txtCurrentPlanSelectedTotal.Text = "0.00"
                    txtOverAllSelectedTotal.Text = "0.00"
                End If
                If financePlanDetail IsNot Nothing Then
                    For icount = 0 To financePlanDetail.Count - 1
                        ddlInstalmentPlan.Items(icount).Attributes.Add("PlanVersion", financePlanDetail(icount).FinancePlanVersion)
                    Next
                End If
                FillPartySession()
            Else
                InitialiseInstalment()
            End If
        End Sub
        Protected Sub FillPartySession()
            Dim oNewParty As NexusProvider.BaseParty
            Dim oWebservice As NexusProvider.ProviderBase
            oWebservice = New NexusProvider.ProviderManager().Provider
            If hPartyKey.Value <> "" Then
                oNewParty = oWebservice.GetParty(hPartyKey.Value)
                Session(CNParty) = oNewParty
            End If
        End Sub
        Protected Sub AddPlanVersionToInstalmentDDL()
            Dim financePlanDetail As NexusProvider.FinancePlanCollection
            financePlanDetail = CType(Session("FinancePlans"), NexusProvider.FinancePlanCollection)
            If financePlanDetail IsNot Nothing Then
                For icount = 0 To financePlanDetail.Count - 1
                    ddlInstalmentPlan.Items(icount).Attributes.Add("PlanVersion", financePlanDetail(icount).FinancePlanVersion)
                Next
            End If
        End Sub

        Public Sub GetFinancePlanDetails()
            If ddlInstalmentPlan.SelectedValue IsNot "" Then
                Dim oBaseInstalment As New BaseInstalment()
                Dim oInstalmentsCollection As New NexusProvider.InstalmentsCollection
                Dim activeInstalmentsCollection As New NexusProvider.InstalmentsCollection
                Dim oFinancePlanDetails As New NexusProvider.FinancePlanDetails
                Dim oFinancePlan As NexusProvider.PremiumFinancePlan
                Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"),
                                                                      Config.NexusFrameWork)
                Dim documentRef As String = String.Empty
                Dim planKey As Integer = 0

                Dim planStatus As String = String.Empty
                Dim planVersion As Integer = 0
                documentRef = Request.QueryString("DocRef")
                planKey = Convert.ToInt32(ddlInstalmentPlan.SelectedValue)
                planVersion = Convert.ToInt32(ddlInstalmentPlan.SelectedItem.Attributes("planversion").ToString())
                ViewState("PlanVersion") = planVersion
                msg_noinstalment.Visible = True
                Try

                    oBaseInstalment.GetPremiumFinancePlanDetails(documentRef, planKey, planVersion, Nothing)


                    If Session(CNFinancePlanDetails) IsNot Nothing Then

                        oInstalmentsCollection = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).Instalments
                        oFinancePlanDetails = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PremiumFinanceDetails

                        If oInstalmentsCollection IsNot Nothing AndAlso oInstalmentsCollection.Count > 0 AndAlso
                                oFinancePlanDetails IsNot Nothing Then
                            hdnPlanStatus.Value = FinancePlanStatusString(oFinancePlanDetails.StatusInd)
                            If (documentRef <> "") Then
                                planKey = oFinancePlanDetails.PFPremiumFinanceKey
                                planStatus = Nexus.Constants.FinancePlanStatusDesc(oFinancePlanDetails.StatusInd)
                                Dim olblTitle As Label =
                                            CType(
                                                CType(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName),
                                                      ContentPlaceHolder).FindControl("lblTitle"),
                                                Label)
                                olblTitle.Text = olblTitle.Text & " " & planKey.ToString() & " - " & planStatus
                            End If
                            If oInstalmentsCollection.Count > 0 Then
                                For iCount As Integer = 0 To oInstalmentsCollection.Count - 1
                                    If oInstalmentsCollection(iCount).StatusDescription.ToLower() = "new" OrElse oInstalmentsCollection(iCount).StatusDescription.ToLower() = "failed" OrElse oInstalmentsCollection(iCount).StatusDescription.ToLower() = "retrying" Then
                                        activeInstalmentsCollection.Add(oInstalmentsCollection(iCount))
                                    End If
                                Next
                            End If


                            oInstalmentsCollection = activeInstalmentsCollection
                            oFinancePlan = Session(CNFinancePlanDetails)
                            oFinancePlan.Instalments = oInstalmentsCollection
                            Session(CNFinancePlanDetails) = oFinancePlan
                            'Bind the grid with retrieved instalments details
                            grdInstallmentQuotes.DataSource = oInstalmentsCollection
                            grdInstallmentQuotes.DataBind()
                            msg_noinstalment.Visible = False
                            liTenderedAmount.Visible = True
                        End If
                    End If

                Catch ex As Exception
                    oInstalmentsCollection = Nothing
                End Try
            Else
                txtCurrentPlanSelectedTotal.Text = "0.00"
                txtOverAllSelectedTotal.Text = "0.00"
            End If
        End Sub
        Protected Sub ddlInstalmentPlan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlInstalmentPlan.SelectedIndexChanged
            AddPlanVersionToInstalmentDDL()
            GetFinancePlanDetails()
            Dim dCurrentPlanTotal As Decimal = 0.0
            Dim oInstalmentPlanDetailsCollection As New NexusProvider.InstalmentPlanDetailsCollection
            If Session("INSTALMENTPLANDETAILS") IsNot Nothing Then
                oInstalmentPlanDetailsCollection = DirectCast(Session("INSTALMENTPLANDETAILS"), NexusProvider.InstalmentPlanDetailsCollection)
            End If
            For Each oInstPlnDet As NexusProvider.InstalmentPlanDetails In oInstalmentPlanDetailsCollection
                If ddlInstalmentPlan.SelectedValue = oInstPlnDet.FinancePlanKey Then
                    If grdInstallmentQuotes.Rows.Count > 0 Then
                        For iCount As Integer = 0 To grdInstallmentQuotes.Rows.Count - 1
                            Dim chkSelected As CheckBox
                            chkSelected = DirectCast(grdInstallmentQuotes.Rows(iCount).FindControl("chkSelectedInstalment"), CheckBox)
                            If grdInstallmentQuotes.Rows(iCount).Cells(1).Text.Trim() = oInstPlnDet.InstalmentDetails.InstalmentNumber Then
                                chkSelected.Checked = True
                                dCurrentPlanTotal = dCurrentPlanTotal + Convert.ToDecimal(grdInstallmentQuotes.Rows(iCount).Cells(4).Text.Trim())
                            End If
                        Next
                    End If
                End If
            Next
            txtCurrentPlanSelectedTotal.Text = dCurrentPlanTotal
            txtAmount.Text = txtOverAllSelectedTotal.Text
        End Sub

        Protected Sub grdInstallmentQuotes_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdInstallmentQuotes.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                If Convert.ToString(grdInstallmentQuotes.DataKeys(e.Row.RowIndex).Values("TransactionDescription")) = "Deposit Instalment" Then
                    e.Row.Cells(1).Text = "Deposit"
                End If
                e.Row.Cells(5).Text = GetCurrencyForDescription(e.Row.Cells(5).Text.Trim())
            End If
            If e.Row.RowType = DataControlRowType.DataRow OrElse e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(3).Visible = False
            End If
        End Sub

        Private Sub TakeExactAmount()
            Dim oFilteredInstalmentPlanDetailsCollection As New NexusProvider.InstalmentPlanDetailsCollection
            Dim dAmountToDistribute As Decimal = Convert.ToDecimal(txtAmount.Text)
            Dim tenderedAmount As Decimal = Convert.ToDecimal(txtTendered.Text)
            If Session("INSTALMENTPLANDETAILS") IsNot Nothing Then
                oFilteredInstalmentPlanDetailsCollection = Session("INSTALMENTPLANDETAILS")
                If oFilteredInstalmentPlanDetailsCollection IsNot Nothing AndAlso oFilteredInstalmentPlanDetailsCollection.Count > 0 Then
                    For nRecordCount As Integer = 0 To oFilteredInstalmentPlanDetailsCollection.Count - 1
                        If nRecordCount >= oFilteredInstalmentPlanDetailsCollection.Count Then
                            Exit For
                        End If
                        If dAmountToDistribute = 0 Then
                            oFilteredInstalmentPlanDetailsCollection.Remove(nRecordCount)
                            nRecordCount = nRecordCount - 1
                        ElseIf dAmountToDistribute > oFilteredInstalmentPlanDetailsCollection(nRecordCount).InstalmentDetails.Amount Then
                            oFilteredInstalmentPlanDetailsCollection(nRecordCount).InstalmentDetails.ActualAmount = tenderedAmount
                            dAmountToDistribute = dAmountToDistribute - oFilteredInstalmentPlanDetailsCollection(nRecordCount).InstalmentDetails.Amount
                        ElseIf dAmountToDistribute < oFilteredInstalmentPlanDetailsCollection(nRecordCount).InstalmentDetails.Amount Then
                            oFilteredInstalmentPlanDetailsCollection(nRecordCount).InstalmentDetails.ActualAmount = tenderedAmount
                            oFilteredInstalmentPlanDetailsCollection(nRecordCount).InstalmentDetails.Amount = dAmountToDistribute
                            oFilteredInstalmentPlanDetailsCollection(nRecordCount).InstalmentDetails.IsPartialPayment = True
                            dAmountToDistribute = 0
                        End If
                    Next
                End If
                oReceiptCashListItems = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                If oFilteredInstalmentPlanDetailsCollection IsNot Nothing AndAlso oFilteredInstalmentPlanDetailsCollection.Count > 0 AndAlso oReceiptCashListItems IsNot Nothing Then
                    Session("INSTALMENTPLANDETAILS") = oFilteredInstalmentPlanDetailsCollection

                    oInstalmentPlanDetailsCollection = DirectCast(Session("INSTALMENTPLANDETAILS"), NexusProvider.InstalmentPlanDetailsCollection)

                    oReceiptCashListItems.InstalmentPlanCollection = oInstalmentPlanDetailsCollection

                    Session(CNCashListItem) = oReceiptCashListItems
                End If

            End If
            'Response.Redirect("~/secure/payment/CashListItems.aspx?Mode=INST&TypeTrans=INST")
            Session("ModeValue") = "INST"
            Session("TypeTrans") = "INST"
            'CashListItems
            Dim changeTab2 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(2) a').tab('show')});"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab2", changeTab2, True)
            'Session("hfActiveTab") = 2
            Session("hfPreviousTab") = 2
        End Sub

        Private Sub WriteOff(ByVal bIsAutomatedProcess As Boolean)
            Dim WriteOffReasonID As String = hdnWriteOffReasonID.Value
            Dim oFilteredInstalmentPlanDetailsCollection As New NexusProvider.InstalmentPlanDetailsCollection
            Dim dMaxRecordCount As Integer = 0
            Dim oUserAuthority As New NexusProvider.UserAuthority
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim bHasAuthoritytoWriteOff As Boolean
            Dim dOverPaymentAmount As Decimal = 0.0
            Dim amount As Decimal = 0.0
            Dim tenderedAmount As Decimal = 0.0
            amount = Convert.ToDecimal(txtAmount.Text)
            tenderedAmount = Convert.ToDecimal(txtTendered.Text)

            oUserAuthority.UserCode = Session(CNLoginName)
            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.HasWriteOffAuthority
            oWebService.GetUserAuthorityValue(oUserAuthority)
            If oUserAuthority.UserAuthorityValue = "1" Then
                bHasAuthoritytoWriteOff = True
                dMaxAmountToWriteOff = oUserAuthority.UserAuthorityOptionalValue2
            Else
                bHasAuthoritytoWriteOff = False
            End If
            dMaxAmountToWriteOff = oUserAuthority.UserAuthorityOptionalValue2
            If Session("INSTALMENTPLANDETAILS") IsNot Nothing Then
                oFilteredInstalmentPlanDetailsCollection = Session("INSTALMENTPLANDETAILS")
                If oFilteredInstalmentPlanDetailsCollection IsNot Nothing AndAlso oFilteredInstalmentPlanDetailsCollection.Count > 0 Then
                    dDifferenceAmount = Convert.ToDecimal(txtOverAllSelectedTotal.Text) - amount
                    dMaxRecordCount = (oFilteredInstalmentPlanDetailsCollection.Count - 1)
                    dOverPaymentAmount = amount - tenderedAmount
                    If Not bHasAuthoritytoWriteOff Then
                        Dim sMessageWriteOffDenied = GetLocalResourceObject("msg_WriteOffDenied")
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "WriteOffDenied", "alert('" + sMessageWriteOffDenied + "');document.getElementById('liPaymentTab').style.display = 'none';document.getElementById('tabInstalments').click();document.getElementById('tabInstalments').show();", True)
                        Exit Sub
                    ElseIf dDifferenceAmount > dMaxAmountToWriteOff OrElse dOverPaymentAmount > dMaxAmountToWriteOff Then
                        Dim sMessageWriteOffLimit As String = GetLocalResourceObject("msg_WriteOffLimit")
                        'to be added in the message
                        sMessageWriteOffLimit = sMessageWriteOffLimit & "\r\n Difference: " & dDifferenceAmount
                        sMessageWriteOffLimit = sMessageWriteOffLimit & "\r\n Your write off limit: " & dMaxAmountToWriteOff
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "WriteOffLimit", "alert('" + sMessageWriteOffLimit + "');document.getElementById('liPaymentTab').style.display = 'none';document.getElementById('tabInstalments').click();document.getElementById('tabInstalments').show();", True)
                        Exit Sub
                    ElseIf dDifferenceAmount <= dMaxAmountToWriteOff AndAlso (Not amount > tenderedAmount) Then
                        oFilteredInstalmentPlanDetailsCollection(dMaxRecordCount).InstalmentDetails.Amount = oFilteredInstalmentPlanDetailsCollection(dMaxRecordCount).InstalmentDetails.Amount - dDifferenceAmount
                        If bIsAutomatedProcess Then
                            oFilteredInstalmentPlanDetailsCollection(dMaxRecordCount).InstalmentDetails.WriteOffReasonID = 0
                        Else
                            oFilteredInstalmentPlanDetailsCollection(dMaxRecordCount).InstalmentDetails.WriteOffReasonID = WriteOffReasonID
                        End If

                        oFilteredInstalmentPlanDetailsCollection(dMaxRecordCount).InstalmentDetails.IsWriteOffPayment = True
                        oFilteredInstalmentPlanDetailsCollection(dMaxRecordCount).InstalmentDetails.ActualAmount = tenderedAmount
                    ElseIf amount > tenderedAmount Then
                        If bIsAutomatedProcess Then
                            oFilteredInstalmentPlanDetailsCollection(dMaxRecordCount).InstalmentDetails.WriteOffReasonID = 0
                        Else
                            oFilteredInstalmentPlanDetailsCollection(dMaxRecordCount).InstalmentDetails.WriteOffReasonID = WriteOffReasonID
                        End If

                        oFilteredInstalmentPlanDetailsCollection(dMaxRecordCount).InstalmentDetails.IsWriteOffPayment = True
                        If dMaxRecordCount = 0 Then
                            oFilteredInstalmentPlanDetailsCollection(dMaxRecordCount).InstalmentDetails.OverPaymentWriteOffAmount = amount
                        ElseIf dMaxRecordCount > 0 Then
                            oFilteredInstalmentPlanDetailsCollection(dMaxRecordCount).InstalmentDetails.OverPaymentWriteOffAmount = amount - oFilteredInstalmentPlanDetailsCollection(dMaxRecordCount).InstalmentDetails.Amount
                        End If


                        oFilteredInstalmentPlanDetailsCollection(dMaxRecordCount).InstalmentDetails.ActualAmount = tenderedAmount
                        'dMaxRecordCount>0 implies more than one record
                    ElseIf amount < tenderedAmount Then
                        If bIsAutomatedProcess Then
                            oFilteredInstalmentPlanDetailsCollection(dMaxRecordCount).InstalmentDetails.WriteOffReasonID = 0
                        Else
                            oFilteredInstalmentPlanDetailsCollection(dMaxRecordCount).InstalmentDetails.WriteOffReasonID = WriteOffReasonID
                        End If

                        oFilteredInstalmentPlanDetailsCollection(dMaxRecordCount).InstalmentDetails.IsWriteOffPayment = True
                        If dMaxRecordCount = 0 Then
                            oFilteredInstalmentPlanDetailsCollection(dMaxRecordCount).InstalmentDetails.OverPaymentWriteOffAmount = amount
                        ElseIf dMaxRecordCount > 0 Then
                            Dim dSettledAmount As Decimal = 0
                            For count As Integer = 0 To dMaxRecordCount - 1
                                dSettledAmount += oFilteredInstalmentPlanDetailsCollection(count).InstalmentDetails.Amount
                            Next
                            oFilteredInstalmentPlanDetailsCollection(dMaxRecordCount).InstalmentDetails.OverPaymentWriteOffAmount = amount - dSettledAmount
                        End If


                        oFilteredInstalmentPlanDetailsCollection(dMaxRecordCount).InstalmentDetails.ActualAmount = tenderedAmount
                    End If
                End If
                oReceiptCashListItems = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)

                If oFilteredInstalmentPlanDetailsCollection IsNot Nothing AndAlso oFilteredInstalmentPlanDetailsCollection.Count > 0 AndAlso oReceiptCashListItems IsNot Nothing Then
                    Session("INSTALMENTPLANDETAILS") = oFilteredInstalmentPlanDetailsCollection
                    oInstalmentPlanDetailsCollection = DirectCast(Session("INSTALMENTPLANDETAILS"), NexusProvider.InstalmentPlanDetailsCollection)
                    oReceiptCashListItems.InstalmentPlanCollection = oInstalmentPlanDetailsCollection

                    Session(CNCashListItem) = oReceiptCashListItems
                End If
            End If
            'Response.Redirect("~/secure/payment/CashListItems.aspx?Mode=INST&TypeTrans=INST")
            Session("ModeValue") = "INST"
            Session("TypeTrans") = "INST"
            'CashListItems
            Dim changeTab2 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(2) a').tab('show')});"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab2", changeTab2, True)
            'Session("hfActiveTab") = 2
            Session("hfPreviousTab") = 2

        End Sub
        Private Sub InitialiseInstalment()
            ddlInstalmentPlan.Items.Clear()
            ddlInstalmentPlan.SelectedIndex = -1
            grdInstallmentQuotes.DataSource = Nothing
            grdInstallmentQuotes.DataBind()
            txtCurrentPlanSelectedTotal.Text = "0.00"
            txtOverAllSelectedTotal.Text = "0.00"
            liTenderedAmount.Visible = False
        End Sub

        Protected Sub BindWriteOffReason()
            Dim oWebService = New NexusProvider.ProviderManager().Provider
            Dim oMediaList As NexusProvider.LookupListCollection
            Dim oReceiptMediaList As New NexusProvider.LookupListCollection
            Dim oPaymentMediaList As New NexusProvider.LookupListCollection
            Dim v_sOptionList As System.Xml.XmlElement = Nothing

            oMediaList = oWebService.GetList(NexusProvider.ListType.PMLookup, "WRITE_OFF_REASON", True, False, , , , v_sOptionList)
            Dim hCurrentOptionColl As New Hashtable()
            Dim iSourceId As Integer
            iSourceId = GetCodeForKey(NexusProvider.ListType.PMLookup, HttpContext.Current.Session(CNBranchCode), "Source", False)
            'Load the xml element 
            If v_sOptionList IsNot Nothing Then
                Dim sXML As String = v_sOptionList.OuterXml
                Dim xmlDoc As New System.Xml.XmlDocument
                xmlDoc.LoadXml(sXML)
                Dim oNodeList As XmlNodeList
                'Filtering the XML with the Description of the UDL
                oNodeList = xmlDoc.SelectNodes("/AdditionalDetails/WRITE_OFF_REASON[is_valid_for_instalments=1 and is_deleted=0]")
                'GENERAL__CONTACT.Items.Clear()

                Dim dRcptDiffAmount As Decimal = txtOverAllSelectedTotal.Text - txtAmount.Text
                Dim sCurrencyCodeForPlan As String = String.Empty
                Dim sURL As String = String.Empty
                Dim sRcptDiffAmount As String = String.Empty

                If oNodeList IsNot Nothing AndAlso oNodeList.Count > 0 Then
                    If grdInstallmentQuotes.Rows.Count > 0 Then
                        sCurrencyCodeForPlan = grdInstallmentQuotes.Rows(0).Cells(5).Text.Trim()
                    End If

                    If Not String.IsNullOrEmpty(sCurrencyCodeForPlan) Then
                        sRcptDiffAmount = Format(Math.Round(CDbl(dRcptDiffAmount), 2), "#0.00") & " " & sCurrencyCodeForPlan
                    End If
                    If HttpContext.Current.Session.IsCookieless Then
                        sURL = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/WriteOffReason.aspx?modal=true&KeepThis=true&TB_iframe=true&height=300&width=750"
                    Else
                        sURL = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "Modal/WriteOffReason.aspx?modal=true&KeepThis=true&TB_iframe=true&height=300&width=750"
                    End If

                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show",
                    "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){TakeRcptDiffConfirmation('" & sRcptDiffAmount & "', '" & sURL & "');  document.getElementById('liPaymentTab').style.display = 'none';});</script>")
                    Exit Sub
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show", "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){alert('No writeoff reason configured.');document.getElementById('liPaymentTab').style.display = 'none';});</script>")

                    Exit Sub
                End If
            End If
        End Sub
        Public Sub okBtnClick(Optional ByRef NextTab As String = "")
            Dim callbtnOk As Boolean
            If (Session("btnOKClicked") IsNot Nothing) Then
                callbtnOk = False
            Else
                Session("btnOKClicked") = 1
                callbtnOk = True
            End If

            If Request("__EVENTARGUMENT") = "ContinueAfterMulticurrency" Then
                Page.Validate("Multicurrency")
            End If

            'If all condition are satisfied
            If Page.IsValid Then
                If Session("ModeValue") = "IR" OrElse Session("ModeValue") = "IP" OrElse Page.IsValid Then
                    If Session("ModeValue") = "IR" OrElse Session("ModeValue") = "IP" OrElse Session("ModeValue") = "PayNow" OrElse
                        (Request("__EVENTARGUMENT") IsNot Nothing And ((Request("__EVENTARGUMENT") = "ContinueAfterMulticurrency" OrElse hdnTabName.Value = "tab-CashListItem") And Session("ModeValue") = "CR") Or (Request("__EVENTARGUMENT") = "CRCashListItem" And Session("AddMoreCashList") = "") Or (Session("AddMoreCashList") = "Yes")) Then
                        Dim iFlag As Integer = CType(Session("SetFlag"), Integer)
                        Dim sType As String = Session("Type")

                        'Record the Session Variable to Produce the Documents - Start
                        If chkProduceDocument.Checked = True Then
                            Session(CNProduceDocument) = True
                            If PnlReceiptType.Visible = True Then
                                Session(CNReceiptMode) = GISLookup_ReceiptType.Value
                                Session(CNPaymentMode) = Nothing
                            Else
                                Session(CNPaymentMode) = GISLookup_PaymentType.Value
                                Session(CNReceiptMode) = Nothing
                            End If
                        Else
                            Session(CNProduceDocument) = Nothing
                            Session(CNReceiptMode) = Nothing
                            Session(CNPaymentMode) = Nothing
                        End If
                        'Record the Session Variable to Produce the Documents - End

                        If iFlag = 1 Then
                            If Session("ModeType") = "Payment" Then 'Cash Cheque payments
                                Dim sValue As String = String.Empty
                                CheckMediaTypeDetails(kIsValidationEnabled, sValue)
                                If sValue = "1" Then
                                    'Call SAM Method to Validate
                                    sValue = String.Empty
                                    Dim oValidationDetailsCollection As NexusProvider.ValidationDetailsCollection
                                    sValue = GetDescriptionForCode(NexusProvider.ListType.PMLookup, PayNow_Address.CountryCode, "Country")
                                    Dim nCountryID As Integer = GetKeyForDescription(NexusProvider.ListType.PMLookup, sValue, "Country")
                                    Dim sAccountType As String = String.Empty
                                    If IsNumeric(ddlAccountType.SelectedValue) Then
                                        sAccountType = ddlAccountType.SelectedValue
                                    End If

                                    Dim sAccountNumber As String = txtBranchCode.Text + "|" + txtAccountCode.Text + "|" + sAccountType
                                    sValue = String.Empty
                                    CheckMediaTypeDetails(kMediaTypeID, sValue)
                                    oWebservice = New NexusProvider.ProviderManager().Provider
                                    oValidationDetailsCollection = oWebservice.ValidateBankAccountNumber(sValue, nCountryID, sAccountNumber, "", sBankName:=hidBankCode.Value)
                                    If oValidationDetailsCollection(0).ValidationMessageDataset <> "" AndAlso oValidationDetailsCollection(0).ValidationMessageDataset IsNot Nothing Then
                                        'If the Collection returna any message from Back office Script
                                        Dim cstScriptValidation As New CustomValidator
                                        cstScriptValidation.IsValid = False
                                        'look for a validation message in the page resources, but if there is not one defined add a default message
                                        cstScriptValidation.ErrorMessage = IIf(GetLocalResourceObject("msgValidationScriptError") Is Nothing, "Bank details have failed validation", GetLocalResourceObject("msgValidationScriptError"))
                                        cstScriptValidation.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                                        'add the validator to the page, this will have the effect of making the page invalid
                                        Page.Validators.Add(cstScriptValidation)

                                        Exit Sub
                                    ElseIf Not oValidationDetailsCollection(0).IsValid Then
                                        'If the Collection does not returns any BO script message, and IsValid key is false, then a custom message is passed 
                                        Dim cstScriptValidation As New CustomValidator
                                        cstScriptValidation.IsValid = False
                                        'look for a validation message in the page resources, but if there is not one defined add a default message
                                        cstScriptValidation.ErrorMessage = IIf(GetLocalResourceObject("msgValidationScriptError") Is Nothing, "Bank details have failed validation", GetLocalResourceObject("msgValidationScriptError"))
                                        cstScriptValidation.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                                        'add the validator to the page, this will have the effect of making the page invalid
                                        Page.Validators.Add(cstScriptValidation)
                                        Exit Sub
                                    End If

                                End If
                                oCashListItems = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                                PopulateObject()

                                If callbtnOk Then
                                    oCashListItems.PaymentItems.Add(oCashListItem)
                                    Session(CNCashListItem) = oCashListItems
                                End If

                                Session("CNCashListItemPending") = oCashListItem
                                If OpenModal(txtAccount.Text.Trim(), txtAmount.Text, Cash_List_Item__Transaction_Date.Text, oCashListItems.CoreCashList.CurrencyCode, "Payment") Then
                                    Return
                                End If

                                Session("ModeValue") = "INST"
                                'CashListItems
                                Dim changeTab2 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(2) a').tab('show')});"
                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab2", changeTab2, True)
                                'Session("hfActiveTab") = 2
                                Session("hfPreviousTab") = 2
                            Else 'Bank Guarantee and Cash/Cheque Recipts

                                oReceiptCashListItems = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                                PopulateObject()
                                If oCashListItem.TypeCode.Trim.ToUpper = "BGDEPT" Then
                                    Dim oBankGuaranteePolicy As NexusProvider.BankGuaranteePolicy = CType(ViewState("BG"), NexusProvider.BankGuaranteePolicy)
                                    If grdvBGDebtDetails.Rows.Count > 0 Then
                                        For iCount As Integer = 0 To grdvBGDebtDetails.Rows.Count - 1
                                            Dim chkSelected As CheckBox
                                            chkSelected = DirectCast(grdvBGDebtDetails.Rows(iCount).FindControl("chkAmtSelect"), CheckBox)
                                            If chkSelected.Checked = True Then
                                                For iTempVar As Integer = 0 To oBankGuaranteePolicy.PartyBGPolicyDetails.Count - 1
                                                    If grdvBGDebtDetails.Rows(iCount).Cells(2).Text.Trim.ToUpper = oBankGuaranteePolicy.PartyBGPolicyDetails(iTempVar).PolicyRef.Trim.ToUpper Then
                                                        Dim oReceiptCashListItemTypePolicies As New NexusProvider.ReceiptCashListItemTypePolicies
                                                        oReceiptCashListItemTypePolicies.BGKey = oBankGuaranteePolicy.PartyBGPolicyDetails(iTempVar).BGKey
                                                        oReceiptCashListItemTypePolicies.AmountTobeAllocated = oBankGuaranteePolicy.PartyBGPolicyDetails(iTempVar).OutstandingPolicyAmt
                                                        oReceiptCashListItemTypePolicies.InsuranceFileKey = oBankGuaranteePolicy.PartyBGPolicyDetails(iTempVar).PolicyKey
                                                        oReceiptCashListItemTypePolicies.PolicyRef = oBankGuaranteePolicy.PartyBGPolicyDetails(iTempVar).PolicyRef
                                                        oCashListItem.Policies.Add(oReceiptCashListItemTypePolicies)
                                                    End If
                                                Next
                                            End If
                                        Next
                                    End If
                                End If

                                If oCashListItem.TypeCode.Trim.ToUpper = "INST" Then
                                    Dim oInstalmentPlanDetailsCollection As New NexusProvider.InstalmentPlanDetailsCollection
                                    If Session("INSTALMENTPLANDETAILS") IsNot Nothing Then
                                        oInstalmentPlanDetailsCollection = DirectCast(Session("INSTALMENTPLANDETAILS"), NexusProvider.InstalmentPlanDetailsCollection)
                                        oReceiptCashListItems.InstalmentPlanCollection = oInstalmentPlanDetailsCollection
                                        'Session("INSTALMENTPLANDETAILS") = Nothing
                                        NextTab = "tab-CashListItems"
                                    Else
                                        Dim sMsgSelectOneInstalment As String = GetLocalResourceObject("msg_SelectOneInstalment")
                                        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){alert('" + sMsgSelectOneInstalment + "');document.getElementById('liPaymentTab').style.display = 'none';document.getElementById('tabInstalments').click();document.getElementById('tabInstalments').show();});</script>")
                                        NextTab = "tab-CashListItem"
                                        Exit Sub
                                    End If
                                End If

                                If callbtnOk Then
                                    oReceiptCashListItems.ReceiptItems.Add(oCashListItem)
                                    Session(CNCashListItem) = oReceiptCashListItems
                                End If

                                Session("CNCashListItemPending") = oCashListItem
                                If OpenModal(txtAccount.Text.Trim(), txtAmount.Text, Cash_List_Item__Transaction_Date.Text, oReceiptCashListItems.CoreCashList.CurrencyCode, "Receipt") Then
                                    Return
                                End If

                                If oInstalmentPlanDetailsCollection IsNot Nothing AndAlso oInstalmentPlanDetailsCollection.Count > 0 Then
                                    Dim nReceiptDifferenceOption As Integer = 2
                                    'If Instalments are selected from different plan and then the instalment amount is overwritten
                                    If txtCurrentPlanSelectedTotal.Text <> 0 AndAlso txtCurrentPlanSelectedTotal.Text <> txtOverAllSelectedTotal.Text AndAlso txtAmount.Text <> txtOverAllSelectedTotal.Text Then
                                        Dim sMessageAmountLess As String = GetLocalResourceObject("msg_AmountLess")
                                        Dim sMessageMultiplePlan As String = GetLocalResourceObject("msg_MultiplePlan")
                                        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){alert('" + sMessageAmountLess + "');alert('" + sMessageMultiplePlan + "');document.getElementById('liPaymentTab').style.display = 'none';document.getElementById('tabInstalments').click();document.getElementById('tabInstalments').show();});</script>")
                                        Exit Sub
                                        'If Instalments are selected from same plan and then the instalment amount is overwritten
                                    ElseIf txtAmount.Text <> txtOverAllSelectedTotal.Text AndAlso txtCurrentPlanSelectedTotal.Text = txtOverAllSelectedTotal.Text Then
                                        Dim oFinancePlanDetails As NexusProvider.PremiumFinancePlan = Nothing
                                        Dim oWebService As NexusProvider.ProviderBase
                                        Try
                                            oWebService = New NexusProvider.ProviderManager().Provider
                                            oFinancePlanDetails = oWebService.GetHeaderAndSummariesPFPlanByKey("", ddlInstalmentPlan.SelectedValue, ViewState("PlanVersion"), Session(CNBranchCode))
                                            If oFinancePlanDetails IsNot Nothing AndAlso oFinancePlanDetails.PremiumFinanceDetails IsNot Nothing Then
                                                nReceiptDifferenceOption = oFinancePlanDetails.PremiumFinanceDetails.Receipt_Difference_Option
                                            End If
                                        Catch ex As Exception
                                            Throw ex
                                        Finally
                                            oWebService = Nothing
                                        End Try

                                        If nReceiptDifferenceOption = 2 Then
                                            Dim sURL As String = String.Empty
                                            If HttpContext.Current.Session.IsCookieless Then
                                                sURL = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/InstalmentRcptDiff.aspx?modal=true&KeepThis=true&TB_iframe=true&height=300&width=750"
                                            Else
                                                sURL = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "Modal/InstalmentRcptDiff.aspx?modal=true&KeepThis=true&TB_iframe=true&height=300&width=750"
                                            End If
                                            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show",
                                        "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);  document.getElementById('liPaymentTab').style.display = 'none';});</script>")

                                            Exit Sub
                                        ElseIf nReceiptDifferenceOption = 1 Then
                                            If txtCurrentPlanSelectedTotal.Text <> 0 AndAlso Convert.ToDecimal(txtAmount.Text) > Convert.ToDecimal(txtOverAllSelectedTotal.Text) Then
                                                Dim sMessageAmountGreater As String = GetLocalResourceObject("msg_AmountGreater")
                                                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert", "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){alert('" + sMessageAmountGreater + "');document.getElementById('liPaymentTab').style.display = 'none';document.getElementById('tabInstalments').click();document.getElementById('tabInstalments').show();});</script>")
                                                Exit Sub
                                            Else
                                                TakeExactAmount()
                                            End If


                                        ElseIf nReceiptDifferenceOption = 0 Then
                                            BindWriteOffReason()
                                            Dim oWebService1 As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                                            Dim oMediaList As NexusProvider.LookupListCollection
                                            Dim v_sOptionList As System.Xml.XmlElement = Nothing
                                            Dim oUserAuthority As New NexusProvider.UserAuthority
                                            Dim bHasAuthoritytoWriteOff As Boolean
                                            Dim oNodeList As XmlNodeList
                                            oMediaList = oWebService1.GetList(NexusProvider.ListType.PMLookup, "WRITE_OFF_REASON", True, False, , , , v_sOptionList)
                                            If v_sOptionList IsNot Nothing Then
                                                Dim sXML As String = v_sOptionList.OuterXml
                                                Dim xmlDoc As New System.Xml.XmlDocument
                                                xmlDoc.LoadXml(sXML)

                                                'Filtering the XML with the Description of the UDL
                                                oNodeList = xmlDoc.SelectNodes("/AdditionalDetails/WRITE_OFF_REASON[is_valid_for_instalments=1 and is_deleted=0]")
                                            End If

                                            oUserAuthority.UserCode = Session(CNLoginName)
                                            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.HasWriteOffAuthority
                                            oWebService1.GetUserAuthorityValue(oUserAuthority)
                                            If oUserAuthority.UserAuthorityValue = "1" Then
                                                bHasAuthoritytoWriteOff = True
                                                dMaxAmountToWriteOff = oUserAuthority.UserAuthorityOptionalValue2
                                            Else
                                                bHasAuthoritytoWriteOff = False
                                            End If
                                            dMaxAmountToWriteOff = oUserAuthority.UserAuthorityOptionalValue2
                                            Dim amount As Decimal = 0.0
                                            amount = Convert.ToDecimal(txtAmount.Text)
                                            dDifferenceAmount = Convert.ToDecimal(txtOverAllSelectedTotal.Text) - amount
                                            If (dDifferenceAmount > dMaxAmountToWriteOff) Then
                                                NextTab = "tab-ReceiptType"
                                                oReceiptCashListItems.ReceiptItems.Clear()
                                                Session("btnOKClicked") = Nothing
                                            ElseIf (oNodeList.Count = 0) Then
                                                If (dDifferenceAmount <> 0.00) Then
                                                    NextTab = "tab-ReceiptType"
                                                    oReceiptCashListItems.ReceiptItems.Clear()
                                                    Session("btnOKClicked") = Nothing
                                                End If
                                            End If
                                            Exit Sub
                                        End If

                                    End If
                                End If
                                If (IsInstamentForReceiptType() OrElse oCashListItem.TypeCode.Trim.ToUpper = "INST") AndAlso oCashListItem.MediaTypeCode.Trim.ToUpper <> "OCP" Then
                                    'Server.Transfer("~/secure/payment/CashListItems.aspx?TypeTrans=INST")
                                    Session("TypeTrans") = "INST"
                                    'CashListItems
                                    Dim changeTab2 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(2) a').tab('show')});"
                                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab2", changeTab2, True)
                                    'Session("hfActiveTab") = 2
                                    Session("hfPreviousTab") = 2
                                ElseIf oCashListItem.MediaTypeCode.Trim.ToUpper = "OCP" Then
                                    Dim oPaymentHubDetail As NexusProvider.PaymentHubDetails = oCashListItem.PaymentHubDetails
                                    Session(CNPaymentHubDetails) = oPaymentHubDetail
                                    If (IsInstamentForReceiptType() OrElse oCashListItem.TypeCode.Trim.ToUpper = "INST") Then
                                        oPaymentHubDetail.ReturnURL = "~/secure/payment/CashListItems.aspx?TypeTrans=INST"
                                    Else
                                        oPaymentHubDetail.ReturnURL = "~/secure/payment/CashListItems.aspx?TypeTrans=" & sType
                                    End If

                                    oPaymentHubDetail.CashListItemIndex = oReceiptCashListItems.ReceiptItems.Count - 1
                                    oPaymentHubDetail.TransactionAmount = oCashListItem.Amount
                                    oPaymentHubDetail.PartyKey = CInt(hPartyKey.Value)
                                    oPaymentHubDetail.TransactionCurrency = oReceiptCashListItems.CoreCashList.CurrencyCode 'GBP
                                    If (IsInstamentForReceiptType() OrElse oCashListItem.TypeCode.Trim.ToUpper = "INST") Then
                                        Server.Transfer("~/secure/Payment/OnlineCardPayment.aspx?RequestType=TokenRegistration&TypeTrans=INST" & "&PartyKey=" + hPartyKey.Value & "&CashListItemIndex=" & oReceiptCashListItems.ReceiptItems.Count - 1)
                                    Else
                                        Server.Transfer("~/secure/Payment/OnlineCardPayment.aspx?RequestType=TokenRegistration&TypeTrans=" & sType & "&PartyKey=" + hPartyKey.Value & "&CashListItemIndex=" & oReceiptCashListItems.ReceiptItems.Count - 1)
                                    End If

                                Else
                                    ' Server.Transfer("~/secure/payment/CashListItems.aspx?TypeTrans=" & sType)
                                    Session("TypeTrans") = sType

                                    'CashListItems
                                    'Dim changeTab2 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(2) a').tab('show')});"
                                    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab2", changeTab2, True)
                                    ''Session("hfActiveTab") = 2
                                    'Session("hfPreviousTab") = 2
                                End If
                            End If
                        ElseIf Session("ModeValue") = "IP" Then 'Insurer Payments
                            If Session("Type").Trim() = PaymentType.R.ToString() Then
                                oReceiptCashListItems = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                                PopulateObject()
                                If oReceiptCashListItems.ReceiptItems.Count > 0 AndAlso Request("__EVENTARGUMENT") <> "ContinueAfterMulticurrency" Then
                                    For iCount As Integer = 0 To oReceiptCashListItems.ReceiptItems.Count - 1
                                        oReceiptCashListItems.ReceiptItems.Remove(iCount)
                                    Next
                                End If

                                If callbtnOk Then
                                    oReceiptCashListItems.ReceiptItems.Add(oCashListItem)
                                    Session(CNCashListItem) = oReceiptCashListItems
                                End If

                                Session("CNCashListItemPending") = oCashListItem
                                If OpenModal(txtAccount.Text.Trim(), txtAmount.Text, Cash_List_Item__Transaction_Date.Text, oReceiptCashListItems.CoreCashList.CurrencyCode, "Receipt") Then
                                    Return
                                End If

                            ElseIf Session("Type").Trim() = PaymentType.P.ToString() Or Session("Type").Trim() = PaymentType.CP.ToString() Then

                                oCashListItems = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                                PopulateObject()
                                If oCashListItems.PaymentItems.Count > 0 AndAlso Request("__EVENTARGUMENT") <> "ContinueAfterMulticurrency" Then
                                    For iCount As Integer = 0 To oCashListItems.PaymentItems.Count - 1
                                        oCashListItems.PaymentItems.Remove(iCount)
                                    Next
                                End If

                                If callbtnOk Then
                                    oCashListItems.PaymentItems.Add(oCashListItem)
                                    Session(CNCashListItem) = oCashListItems
                                End If

                                Session("CNCashListItemPending") = oCashListItem
                                If OpenModal(txtAccount.Text.Trim(), txtAmount.Text, Cash_List_Item__Transaction_Date.Text, oCashListItems.CoreCashList.CurrencyCode, "Payment") Then
                                    Return
                                End If
                            Else
                                ' DO Nothing
                            End If

                            'Server.Transfer("~/secure/payment/CashListItems.aspx?EVENTARGUMENT=IPRefresh&Mode=IP&Type=" + sType + "&PartyKey=" + hPartyKey.Value)
                            Session("EVENTARGUMENT") = "IPRefresh"
                            Session("ModeValue") = "IP"
                            Session("Type") = sType
                            Session("PartyKey") = hPartyKey.Value
                            'CashListItems
                            Dim changeTab2 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(2) a').tab('show')});"
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab2", changeTab2, True)
                            'Session("hfActiveTab") = 2
                            Session("hfPreviousTab") = 2
                        ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then 'MTA Cancellation

                            oCashListItems = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                            PopulateObject()
                            If oCashListItems.PaymentItems.Count > 0 AndAlso Request("__EVENTARGUMENT") <> "ContinueAfterMulticurrency" Then
                                For iCount As Integer = 0 To oCashListItems.PaymentItems.Count - 1
                                    oCashListItems.PaymentItems.Remove(iCount)
                                Next
                            End If

                            If callbtnOk Then
                                oCashListItems.PaymentItems.Add(oCashListItem)
                                oCashListItems.AccountShortCode = CType(Session(CNAccountName), String)
                            End If

                            Session("CNCashListItemPending") = oCashListItem
                            If OpenModal(txtAccount.Text.Trim(), txtAmount.Text, Cash_List_Item__Transaction_Date.Text, oCashListItems.CoreCashList.CurrencyCode, "Payment") Then
                                Return
                            End If
                            Dim oQuote As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)

                            oWebservice = New NexusProvider.ProviderManager().Provider
                            oWebservice.CreatePaymentCashListWithItems(oCashListItems)

                            Dim oPayment As NexusProvider.Payment
                            Select Case UCase(GISLookup_MediaType.SelectedItem.Text)
                                Case "CASH"
                                    oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cash, CDec(Session(CNAmountToPay)))
                                    oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashListItem
                                Case "CREDIT CARD"
                                    oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.CreditCard, CDec(Session(CNAmountToPay)))
                                    oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashListItem
                                Case "BANKERS DRAFT"
                                    oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.BankersDraft, CDec(Session(CNAmountToPay)))
                                    oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashListItem
                                Case "DIRECT DEBIT"
                                    oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.DebitCard, CDec(Session(CNAmountToPay)))
                                    oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashListItem
                                Case "CHEQUE"
                                    oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cheque, CDec(Session(CNAmountToPay)))
                                    oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashListItem
                                Case Else
                                    oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.AllOthers, CDec(Session(CNAmountToPay)))
                                    oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashListItem
                            End Select

                            Dim oBaseParty As New NexusProvider.BaseParty

                            Dim oAccountSearchCr As New NexusProvider.AccountSearchCriteria
                            Dim oAccountColl As NexusProvider.AccountSearchResultCollection
                            Dim iAccountKey As Integer
                            Dim sPrePaymentOption As String 'for checking if PrePayment option is enable
                            sPrePaymentOption = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 87).OptionValue

                            If oQuote.BusinessTypeCode = "DIRECT" Then 'Direct Business/Customer
                                oBaseParty = oWebservice.GetParty(oQuote.PartyKey)
                                Select Case True
                                    Case TypeOf oBaseParty Is NexusProvider.CorporateParty
                                        With CType(oBaseParty, NexusProvider.CorporateParty)
                                            If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                                oBaseParty.UserName = .ClientSharedData.ShortName.Trim
                                            ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                                oBaseParty.UserName = .UserName.Trim
                                            End If
                                        End With
                                    Case TypeOf oBaseParty Is NexusProvider.PersonalParty
                                        With CType(oBaseParty, NexusProvider.PersonalParty)
                                            If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                                oBaseParty.UserName = .ClientSharedData.ShortName.Trim
                                            ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                                oBaseParty.UserName = .UserName.Trim
                                            End If
                                        End With
                                End Select
                                oAccountSearchCr.ShortCode = oBaseParty.UserName
                                oAccountColl = oWebservice.FindAccounts(oAccountSearchCr)
                                iAccountKey = oAccountColl(0).AccountKey
                            Else

                                oAccountSearchCr.ShortCode = oQuote.AgentCode
                                oAccountColl = oWebservice.FindAccounts(oAccountSearchCr)
                                iAccountKey = oAccountColl(0).AccountKey

                            End If

                            oPayment.PayNowDetails = Nothing
                            Dim oPayNowPaymentDetails As New NexusProvider.PaymentType
                            oPayNowPaymentDetails.InsuranceFileRef = oQuote.InsuranceFileRef
                            oPayNowPaymentDetails.CashListKey = oCashListItems.CashListKey
                            oPayNowPaymentDetails.CashListItemKey = oCashListItems.PaymentCashList(0).CashListItemKey
                            oPayNowPaymentDetails.TransDetailKey = oCashListItems.PaymentCashList(0).TransDetailKey
                            oPayNowPaymentDetails.PaymentAccountID = iAccountKey
                            oPayNowPaymentDetails.PaymentTypeCode = GISLookup_PaymentType.Value
                            oPayNowPaymentDetails.MediaTypeCode = GISLookup_MediaType.SelectedValue
                            oPayNowPaymentDetails.MediaReference = txtMediaReference.Text.ToString().Trim()
                            oPayNowPaymentDetails.OurReference = txtOurReference.Text.ToString().Trim()
                            oPayNowPaymentDetails.TheirReference = txtTheirReference.Text.ToString().Trim()
                            If Session(CNCashListCurrencyRates) IsNot Nothing Then
                                Dim rates = CType(Session(CNCashListCurrencyRates), Nexus.Constants.Session.CashListCurrencyRates)
                                Dim paymentItem = CType(Session(CNPayment), NexusProvider.Payment)
                                paymentItem.PayNowDetails.CurrencyBaseDate = rates.CurrencyBaseDate
                                paymentItem.PayNowDetails.CurrencyBaseXrate = rates.CurrencyBaseXrate
                                paymentItem.PayNowDetails.AccountBaseDate = rates.AccountBaseDate
                                paymentItem.PayNowDetails.AccountBaseXrate = rates.AccountBaseXrate
                                paymentItem.PayNowDetails.SystemBaseDate = rates.SystemBaseDate
                                paymentItem.PayNowDetails.SystemBaseXrate = rates.SystemBaseXrate
                                paymentItem.PayNowDetails.OverrideReason = rates.OverrideReason
                            End If
                            oPayment.PayNowPaymentDetails = oPayNowPaymentDetails

                            'set appropriate session values here to indicate payment taken and then redirect to end page
                            Session(CNPayment) = oPayment
                            If sPrePaymentOption Is Nothing Or sPrePaymentOption = "0" Then
                                Session(CNPaid) = True
                                Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                            Else
                                Session(CNPaid) = False
                                Response.Redirect("~/secure/payment/PrePayment.aspx", False)
                            End If

                        ElseIf Session("ModeValue") = "CR" And Not String.IsNullOrEmpty(CashListItemID.Value()) Then
                            'Cash/Cheque Recipts/Payments for Edit mode
                            If Session("ModeType") = "Payment" Then 'Payments
                                oUpdateCashListItem = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType).PaymentItems.Item(CType(CashListItemID.Value(), Integer))
                                Dim oPaymentCashListItems As NexusProvider.PaymentCashListItemType = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                                UpdateObject()

                                Session("CNCashListItemPending") = oUpdateCashListItem
                                If OpenModal(txtAccount.Text.Trim(), txtAmount.Text, Cash_List_Item__Transaction_Date.Text, oPaymentCashListItems.CoreCashList.CurrencyCode, "Payment") Then
                                    Return
                                End If
                                'oPaymentCashListItems.PaymentItems.Update(oUpdateCashListItem, Request.QueryString("CashListItemID"))
                                Session(CNCashListItem) = oPaymentCashListItems

                            Else 'Receipts
                                If (Session(CNCashListItem) IsNot Nothing) Then
                                    oUpdateCashListItem = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType).ReceiptItems.Item(CType(CashListItemID.Value(), Integer))
                                    Dim oReceiptCashListItems As NexusProvider.ReceiptCashListItemType = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                                    UpdateObject()
                                    Session("CNCashListItemPending") = oUpdateCashListItem
                                    If OpenModal(txtAccount.Text.Trim(), txtAmount.Text, Cash_List_Item__Transaction_Date.Text, oReceiptCashListItems.CoreCashList.CurrencyCode, "Payment") Then
                                        Return
                                    End If
                                    Session(CNCashListItem) = oReceiptCashListItems
                                End If
                            End If

                            If oUpdateCashListItem.MediaTypeCode.Trim.ToUpper = "OCP" Then
                                Dim oPaymentHubDetail As NexusProvider.PaymentHubDetails = oUpdateCashListItem.PaymentHubDetails
                                oPaymentHubDetail.ReturnURL = "~/secure/payment/CashListItems.aspx?EVENTARGUMENT=Refresh&Mode=" + Session("ModeType").ToString + "&Type=" + sType + ""
                                Session(CNPaymentHubDetails) = oPaymentHubDetail

                                Server.Transfer("~/secure/Payment/OnlineCardPayment.aspx?RequestType=TokenRegistration&TypeTrans=" & sType & "&PartyKey=" + hPartyKey.Value & "&CashListItemIndex=" & oPaymentHubDetail.CashListItemIndex)
                            Else
                                'Server.Transfer("~/secure/payment/CashListItems.aspx?EVENTARGUMENT=Refresh&Mode=" + Session("ModeType").ToString + "&Type=" + sType + "")
                                Session("EVENTARGUMENT") = "Refresh"
                                Session("ModeValue") = Session("ModeType").ToString
                                Session("Type") = sType
                            End If

                        ElseIf Session(CNClaim) IsNot Nothing AndAlso Session(CNUnAllocatedClaimPayment) Is Nothing AndAlso Session(CNMode) <> Mode.Recommend Then
                            ' This functionality is for Claim Payments
                            Dim oPayment As NexusProvider.ClaimPayment = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).Payment
                            Dim oPaymentItem As New NexusProvider.PaymentCashListItemType
                            Dim oClaimResponse As NexusProvider.ClaimResponse = Nothing
                            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                            Dim bTimeStamp As Byte() = CType(Session(CNClaimTimeStamp), Byte())
                            oWebservice = New NexusProvider.ProviderManager().Provider
                            GISLookup_PaymentType.Value = "CLP"
                            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

                            With oPaymentItem
                                .AccountShortCode = oPayment.PartyPaidCode
                                .MediaTypeCode = GISLookup_MediaType.SelectedValue
                                .TypeCode = GISLookup_PaymentType.Value
                                .TransactionDate = CDate(IIf(Trim(Cash_List_Item__Transaction_Date.Text) <> String.Empty, Trim(Cash_List_Item__Transaction_Date.Text), DateTime.MinValue))
                                .Amount = CDec(Trim(txtAmount.Text))
                                .BankPaymentType.AccountCode = Trim(txtAccountCode.Text)
                                .BankPaymentType.BranchCode = Trim(txtBranchCode.Text)
                                .BankPaymentType.ExpiryDate = CDate(IIf(Trim(txtExpiryDate.Text) <> String.Empty, Trim(txtExpiryDate.Text), DateTime.MinValue))
                                .BankPaymentType.PayeeName = Trim(txtPayeeName.Text)
                                .BankPaymentType.Reference1 = Trim(txtReference1.Text)
                                .BankPaymentType.Reference2 = Trim(txtReference2.Text)
                                .BankReference = Trim(txtBankReference.Text)
                                .FurtherDetails = txtDetails.Text
                                .MediaReference = Trim(txtMediaReference.Text)
                                .OurReference = Trim(txtOurReference.Text)
                                .TheirReference = Trim(txtTheirReference.Text)
                                .StatusCode = ddlStatus.Value
                                .BankPaymentType.BIC = Trim(txtBIC.Text)
                                .BankPaymentType.IBAN = Trim(txtIBAN.Text)
                                'Get OnScreen Address info
                                Dim oAddress As New NexusProvider.Address
                                If PayNow_Address.Address IsNot Nothing Then
                                    oAddress.Address1 = PayNow_Address.Address1.Trim
                                    oAddress.Address2 = PayNow_Address.Address2.Trim
                                    oAddress.Address3 = PayNow_Address.Address3.Trim
                                    oAddress.Address4 = PayNow_Address.Address4.Trim
                                    oAddress.CountryCode = Trim(PayNow_Address.CountryCode)
                                    oAddress.PostCode = PayNow_Address.Postcode.Trim
                                End If
                                .ContactAddress = oAddress
                                .ChequeDate = oPayment.PaymentDate
                            End With

                            If callbtnOk Then
                                oPayment.CashList.PaymentCashListItemType.Add(oPaymentItem)
                            End If

                            If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ShowSummary = False Then
                                SkipSummaryPage()
                            Else
                                Response.Redirect("~/Claims/summary.aspx", False)
                            End If

                        ElseIf (Session(CNQuoteMode) = QuoteMode.FullQuote Or Session(CNQuoteMode) = QuoteMode.MTAQuote _
                    Or Session(CNQuoteCollectionFiles) IsNot Nothing) AndAlso Session("ModeValue") <> "INSDEPOSIT" Then
                            'New Business, Mark For Collection
                            If Session(CNQuoteCollectionFiles) Is Nothing Then
                                Session.Remove(CNCashListItem)
                            End If

                            oWebservice = New NexusProvider.ProviderManager().Provider
                            Dim oPayNowReceipt As NexusProvider.AddPayNowReceipt = CType(Session(CNPayNowReceipt), NexusProvider.AddPayNowReceipt)
                            Dim oPayNowReceipts As New NexusProvider.AddPayNowReceiptCollection
                            Dim oQuote As NexusProvider.Quote = CType(Session(CNQuote), NexusProvider.Quote)

                            If OpenModal(txtAccount.Text.Trim(), txtAmount.Text, Cash_List_Item__Transaction_Date.Text, oPayNowReceipt.Receipt.CurrencyCode, "Receipt") Then
                                Return
                            End If
                            'Updation of the object with values
                            With oPayNowReceipt
                                .Receipt.ReceiptTypeCode = Trim(GISLookup_ReceiptType.Value)
                                .Receipt.TransactionDate = CDate(Trim(Cash_List_Item__Transaction_Date.Text))
                                .Receipt.MediaTypeCode = Trim(GISLookup_MediaType.SelectedValue)
                                .Receipt.Address1 = PayNow_Address.Address1
                                .Receipt.Address2 = PayNow_Address.Address2
                                .Receipt.Address3 = PayNow_Address.Address3
                                .Receipt.Address4 = PayNow_Address.Address4
                                .Receipt.PostalCode = PayNow_Address.Postcode
                                .Receipt.CountryCode = PayNow_Address.CountryCode
                                .Receipt.Amount = CType(Trim(txtAmount.Text), Double)
                                .Receipt.CollectionDate = CDate(Trim(Cash_List_Item__Collection_Date.Text))
                                .Receipt.Comments = Trim(txtComments.Text)
                                .Receipt.OurReference = Trim(txtOurReference.Text)
                                .Receipt.MediaReference = Trim(txtMediaReference.Text)
                                .Receipt.TheirReference = Trim(txtTheirReference.Text)
                                If Not String.IsNullOrEmpty(Trim(Cash_List_Item__Collection_Date.Text)) Then
                                    .Receipt.CollectionDateSpecified = True
                                Else
                                    .Receipt.CollectionDateSpecified = False
                                End If

                                .Receipt.ContactName = txtName.Text

                                'Mark For Collection - Start
                                If Session(CNQuoteCollectionFiles) Is Nothing Then
                                    Select Case Trim(GISLookup_MediaType.SelectedValue)
                                        Case "BD", "DD", "PF", "CQ"
                                            Dim oPayment As NexusProvider.Payment = New NexusProvider.Payment(NexusProvider.PaymentTypes.BankersDraft, CDec(Session(CNAmountToPay)))
                                            .Receipt.ChequeName = txtChequeHolderName.Text
                                            .Receipt.ChequeDate = Cash_List_Item__Cheque_Date.Text
                                            If Not String.IsNullOrEmpty(Trim(Cash_List_Item__Cheque_Date.Text)) Then
                                                .Receipt.ChequeDateSpecified = True
                                            Else
                                                .Receipt.ChequeDateSpecified = False
                                            End If
                                            .Receipt.BankReference = txtBankReference.Text

                                            If CheckAdditionalDetails() > 0 Then

                                                .Receipt.InstrumentNumber = Cash_List_Item__InstrumentNumber.Text
                                                .Receipt.DraweeBankBranch = txtBankBranch.Text
                                                .Receipt.DraweeBankLocation = txtBankLocation.Text
                                                .Receipt.DraweeBankName = GISLookup_BankList.Value
                                                .Receipt.ChequeClearingType = GISLookup_ChequeClearingTypeList.Value
                                                .Receipt.ChequeType = GISLookup_ChequeType.Value
                                            End If

                                            oPayment.PayNowDetails = oPayNowReceipt.Receipt
                                            Session(CNPayment) = oPayment
                                        Case "CA", "BACS", "CHAPS", "SO", "EFT", "MFT", "CAP"
                                            Dim oPayment As NexusProvider.Payment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cash, CDec(Session(CNAmountToPay)))
                                            oPayment.PayNowDetails = oPayNowReceipt.Receipt
                                            Session(CNPayment) = oPayment

                                        Case "CC"
                                            Dim oPayment As NexusProvider.Payment = New NexusProvider.Payment(NexusProvider.PaymentTypes.CreditCard, CDec(Session(CNAmountToPay)))
                                            .Receipt.CCManualAuthCode = txtManualAuth.Text
                                            .Receipt.CCExpiryDate = Cash_List_Item__Expiry_Date.Text
                                            .Receipt.CCName = txtNameOnCard.Text
                                            .Receipt.CCNumber = txtCardNumber.Text
                                            .Receipt.CCStartDate = Cash_List_Item__Start_Date.Text

                                            If CheckAdditionalDetails() > 0 Then

                                                .Receipt.CCIssue = txtIssueNumber.Text
                                                .Receipt.CCPin = txtPin.Text
                                                .Receipt.CCCustomer = ddlCustomer.SelectedValue
                                                .Receipt.CCTypeOfCard = GISLookup_TypeofCard.Value
                                                .Receipt.CCIssueBank = GISLookup_IssuingBank.Value
                                                .Receipt.CCSlipNumber = txtTransactionSlip.Text
                                            End If
                                            oPayment.PayNowDetails = oPayNowReceipt.Receipt
                                            Session(CNPayment) = oPayment
                                        Case "OCP"
                                            Dim oPayment As NexusProvider.Payment = New NexusProvider.Payment(NexusProvider.PaymentTypes.PaymentHub, CDec(Session(CNAmountToPay)))
                                            Dim oPayNowPaymentDetails_OCP As New NexusProvider.PaymentType
                                            Dim oPaymentHubDetail As New NexusProvider.PaymentHubDetails
                                            oPaymentHubDetail.ReturnURL = "~/secure/TransactionConfirmation.aspx"
                                            If CDec(Session(CNAmountToPay)) > 0 Then
                                                oPaymentHubDetail.TransactionAmount = CDec(Session(CNAmountToPay))
                                                oPaymentHubDetail.RequestType = PaymentHub.RequestType.Payment
                                            ElseIf CDec(Session(CNAmountToPay)) < 0 Then
                                                oPaymentHubDetail.TransactionAmount = CDec(Session(CNAmountToPay)) * -1
                                                oPaymentHubDetail.RequestType = PaymentHub.RequestType.Refund
                                            End If

                                            oPaymentHubDetail.TransactionCurrency = oPayNowReceipt.Receipt.CurrencyCode
                                            Session(CNPaymentHubDetails) = oPaymentHubDetail
                                            oPayNowPaymentDetails_OCP.MediaTypeCode = "OCP"
                                            oPayment.PayNowPaymentDetails = oPayNowPaymentDetails_OCP
                                            oPayment.PayNowDetails = oPayNowReceipt.Receipt
                                            Session(CNPayment) = oPayment
                                            Response.Redirect("~/secure/Payment/OnlineCardPayment.aspx?PartyKey=" + Convert.ToString(oQuote.PartyKey))

                                    End Select
                                    If Session(CNCashListCurrencyRates) IsNot Nothing Then
                                        Dim rates = CType(Session(CNCashListCurrencyRates), Nexus.Constants.Session.CashListCurrencyRates)
                                        Dim paymentItem = CType(Session(CNPayment), NexusProvider.Payment)
                                        paymentItem.PayNowDetails.CurrencyBaseDate = rates.CurrencyBaseDate
                                        paymentItem.PayNowDetails.CurrencyBaseXrate = rates.CurrencyBaseXrate
                                        paymentItem.PayNowDetails.AccountBaseDate = rates.AccountBaseDate
                                        paymentItem.PayNowDetails.AccountBaseXrate = rates.AccountBaseXrate
                                        paymentItem.PayNowDetails.SystemBaseDate = rates.SystemBaseDate
                                        paymentItem.PayNowDetails.SystemBaseXrate = rates.SystemBaseXrate
                                        paymentItem.PayNowDetails.OverrideReason = rates.OverrideReason
                                    End If
                                End If
                                'Mark For Collection - End

                            End With

                            'Check the BusinessType in Quote Object.
                            If oQuote.BusinessTypeCode = "DIRECT" Then 'Direct Business /Customer
                                oPayNowReceipt.PartyKey = oQuote.PartyKey
                            ElseIf hPartyKey.Value IsNot Nothing AndAlso hPartyKey.Value.Trim.Length <> 0 AndAlso hPartyKey.Value.Trim <> "0" Then
                                oPayNowReceipt.PartyKey = CType(hPartyKey.Value, Integer)
                            End If

                            'Checking of the Pre-Payments setting
                            Dim oOptionType As New NexusProvider.OptionTypeSetting
                            oOptionType = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 87)
                            If oOptionType.OptionValue = "1" Then 'Pre-Payments is ON
                                If oPayNowReceipt.PartyKey = 0 Then
                                    oPayNowReceipt.PartyKey = oQuote.PartyKey
                                End If
                                oWebservice.AddPayNowReceipt(oPayNowReceipt, oQuote.BranchCode)
                                Response.Redirect("~/secure/payment/PrePayment.aspx", False)
                            Else 'Pre-Payments is OFF
                                Select Case UCase(GISLookup_MediaType.SelectedItem.Text)
                                    Case "CASH"
                                        Dim oPayment As NexusProvider.Payment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cash, CDec(Session(CNAmountToPay)))
                                        oPayment.PayNowDetails = oPayNowReceipt.Receipt
                                        oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashListItem
                                        Session(CNPayment) = oPayment
                                    Case "CREDIT CARD"
                                        Dim oPayment As NexusProvider.Payment = New NexusProvider.Payment(NexusProvider.PaymentTypes.CreditCard, CDec(Session(CNAmountToPay)))
                                        oPayment.PayNowDetails = oPayNowReceipt.Receipt
                                        oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashListItem
                                        Session(CNPayment) = oPayment
                                    Case "BANKERS DRAFT"
                                        Dim oPayment As NexusProvider.Payment = New NexusProvider.Payment(NexusProvider.PaymentTypes.BankersDraft, CDec(Session(CNAmountToPay)))
                                        oPayment.PayNowDetails = oPayNowReceipt.Receipt
                                        oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashListItem
                                        Session(CNPayment) = oPayment
                                    Case "DIRECT DEBIT"
                                        Dim oPayment As NexusProvider.Payment = New NexusProvider.Payment(NexusProvider.PaymentTypes.DebitCard, CDec(Session(CNAmountToPay)))
                                        oPayment.PayNowDetails = oPayNowReceipt.Receipt
                                        oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashListItem
                                        Session(CNPayment) = oPayment
                                    Case "CHEQUE"
                                        Dim oPayment As NexusProvider.Payment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cheque, CDec(Session(CNAmountToPay)))
                                        oPayment.PayNowDetails = oPayNowReceipt.Receipt
                                        oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstCashListItem
                                        Session(CNPayment) = oPayment
                                End Select

                                If Session(CNQuoteCollectionFiles) Is Nothing Then 'New Business
                                    Session(CNPaid) = True
                                    Response.Redirect("~/secure/TransactionConfirmation.aspx", False)
                                Else
                                    'Mark For Collection - Start
                                    'this will only be in case of Quote COllection Process when we call 
                                    'CreateReceipt method seperately as we dont require paynow process to be called here
                                    Dim oPayment As NexusProvider.Payment = Nothing
                                    If Session(CNPayment) IsNot Nothing Then
                                        oPayment = Session(CNPayment)
                                    Else
                                        oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cash)
                                    End If

                                    Dim oReceiptCashListCollection As New NexusProvider.ReceiptCashListCollection
                                    oReceiptCashListItems = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                                    PopulateObject()

                                    If callbtnOk Then
                                        oReceiptCashListItems.ReceiptItems.Add(oCashListItem)
                                        oReceiptCashListCollection = oWebservice.CreateReceiptcashListWithItem(oReceiptCashListItems)
                                    End If

                                    Session("CNCashListItemPending") = oCashListItem
                                    If OpenModal(txtAccount.Text.Trim(), txtAmount.Text, Cash_List_Item__Transaction_Date.Text, oCashListItems.CoreCashList.CurrencyCode, "Payment") Then
                                        Return
                                    End If

                                    Dim oCreditTransactionColl As New NexusProvider.CreditTransactionCollection
                                    Dim oCreditTransaction As New NexusProvider.CreditTransaction
                                    'Find the Account ID not Account Key
                                    'Populate the Account regarding the Direct Business And Agency Business
                                    'If Agency Business prepopulate the Agent Code
                                    'Else if Direct Business then Client Code
                                    Dim oBaseParty As New NexusProvider.BaseParty
                                    oBaseParty = oWebservice.GetParty(oQuote.PartyKey)
                                    oWebservice = New NexusProvider.ProviderManager().Provider
                                    Dim oAccountSearchCr As New NexusProvider.AccountSearchCriteria
                                    Dim oAccountColl As NexusProvider.AccountSearchResultCollection
                                    'Here we want to find the Direct business or agnecy business if direct then get the party code
                                    If oQuote.BusinessTypeCode = "DIRECT" Then 'Direct Business/Customer

                                        Select Case True
                                            Case TypeOf oBaseParty Is NexusProvider.CorporateParty
                                                With CType(oBaseParty, NexusProvider.CorporateParty)
                                                    '     oBaseParty.UserName = .ClientSharedData.ShortName.Trim
                                                    If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                                        oBaseParty.UserName = .ClientSharedData.ShortName.Trim
                                                    ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                                        oBaseParty.UserName = .UserName.Trim
                                                    End If
                                                End With
                                            Case TypeOf oBaseParty Is NexusProvider.PersonalParty
                                                With CType(oBaseParty, NexusProvider.PersonalParty)
                                                    'oBaseParty.UserName = .ClientSharedData.ShortName.Trim
                                                    If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                                        oBaseParty.UserName = .ClientSharedData.ShortName.Trim
                                                    ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                                        oBaseParty.UserName = .UserName.Trim
                                                    End If
                                                End With
                                        End Select
                                        oAccountSearchCr.ShortCode = oBaseParty.UserName
                                        oAccountColl = oWebservice.FindAccounts(oAccountSearchCr)
                                        oCreditTransaction.AccountKey = oAccountColl(0).AccountKey
                                    Else

                                        oAccountSearchCr.ShortCode = oQuote.AgentCode
                                        oAccountColl = oWebservice.FindAccounts(oAccountSearchCr)
                                        oCreditTransaction.AccountKey = oAccountColl(0).AccountKey

                                    End If

                                    oCreditTransaction.Amount = CType(txtAmount.Text, Double)
                                    oCreditTransaction.CollectionDate = Cash_List_Item__Collection_Date.Text
                                    oCreditTransaction.TransDetailKey = oReceiptCashListCollection(0).TransDetailKey
                                    oCreditTransactionColl.Add(oCreditTransaction)
                                    oPayment.CreditTransaction = oCreditTransactionColl
                                    oPayment.PayNowDetails = Nothing
                                    'set appropriate session values here to indicate payment taken and then redirect to end page
                                    Session(CNPayment) = oPayment
                                    Session(CNPaid) = True
                                    Response.Redirect("~/secure/QuoteCollectionConfirmation.aspx", False)

                                    'Mark For Collection - End
                                End If
                            End If
                        ElseIf Session(CNClaim) IsNot Nothing AndAlso Session(CNUnAllocatedClaimPayment) Is Nothing AndAlso Session(CNMode) = Mode.Recommend Then
                            oCashListItems = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                            PopulateObject() 'Population of the fields with values
                            oWebservice = New NexusProvider.ProviderManager().Provider

                            If Session(CNMode) = Mode.Recommend Then
                                oCashListItem.SkipPosting = True
                                oCashListItems.PaymentItems.Add(oCashListItem)
                                Try
                                    oWebservice.CreatePaymentCashListWithItems(oCashListItems)
                                    oWebservice.AddCashClaimLink(Session(CNClaimPaymentKey), oCashListItems.PaymentCashList(0).CashListItemKey)
                                    Response.Redirect("~/secure/AuthoriseClaimPayments.aspx")
                                Catch ex As NexusProvider.NexusException
                                    If ex.Errors(0).Code = "331" Then   'Code : 330 :: Description: DebtorUserGroupsAreNotSetup
                                        Dim cstDebtorUserGroups As New CustomValidator
                                        cstDebtorUserGroups.IsValid = False
                                        'look for a validation message in the page resources, but if there is not one defined add a default message
                                        cstDebtorUserGroups.ErrorMessage = IIf(GetLocalResourceObject("cstDebtorUserGroups") Is Nothing, "Cannot Proceed- Debtor User Groups are not setup. Please contact your system administrator.", GetLocalResourceObject("cstDebtorUserGroups"))
                                        cstDebtorUserGroups.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                                        'add the validator to the page, this will have the effect of making the page invalid
                                        Page.Validators.Add(cstDebtorUserGroups)
                                        Exit Sub
                                    End If
                                End Try

                            End If
                        ElseIf Session(CNUnAllocatedClaimPayment) IsNot Nothing AndAlso Session(CNMode) = Mode.Authorise Then

                            Dim oAllocationDetailsCollections As New NexusProvider.AllocationDetailsCollections
                            Dim oTrasactionDetails As New NexusProvider.AllocationDetailsCollections
                            Dim oUnallocatedClaimPayments As NexusProvider.UnallocatedClaimPayments = CType(Session(CNUnAllocatedClaimPayment), NexusProvider.UnallocatedClaimPayments)
                            Dim iAccountKey As Integer = oUnallocatedClaimPayments.AccountKey
                            Dim dAmount As Double = oUnallocatedClaimPayments.Amount
                            Dim oAllocationDetails As New NexusProvider.AllocationDetails
                            Dim oAllocation As NexusProvider.Allocation
                            Dim oTransAllocationDetails As New NexusProvider.AllocationDetails
                            Dim bIsUpdated As Boolean
                            Try
                                'Fetch CashList Details
                                Dim oCashClaimLink As NexusProvider.CashClaimLink
                                oCashClaimLink = oWebservice.GetCashClaimLink(Session(CNClaimPaymentKey), Session(CNBranchCode))

                                If oCashClaimLink IsNot Nothing AndAlso oCashClaimLink.CashListKey > 0 Then
                                    'fetch cashlistitems
                                    Dim oPaymentCashListItemsCollection As NexusProvider.PaymentCashListItemTypeCollection
                                    Dim oPaymentCashListItem As New NexusProvider.PaymentCashListItemType
                                    oPaymentCashListItemsCollection = oWebservice.GetPaymentTypeCashListItem(oCashClaimLink.CashListItemKey)

                                    Dim oPaymentItem As NexusProvider.PaymentItems
                                    If oPaymentCashListItemsCollection IsNot Nothing AndAlso oPaymentCashListItemsCollection.Count > 0 Then
                                        For Each oCashListItemRet As NexusProvider.PaymentItems In oPaymentCashListItemsCollection(0).PaymentItems
                                            oPaymentCashListItem.CoreCashList.BankAccountCode = oPaymentCashListItemsCollection(0).CoreCashList.BankAccountCode
                                            oPaymentCashListItem.CoreCashList.CurrencyCode = oPaymentCashListItemsCollection(0).CoreCashList.CurrencyCode
                                            oPaymentCashListItem.CoreCashList.ListDate = oPaymentCashListItemsCollection(0).CoreCashList.ListDate
                                            oPaymentCashListItem.CoreCashList.Reference = oPaymentCashListItemsCollection(0).CoreCashList.Reference
                                            oPaymentCashListItem.CoreCashList.StatusCode = oPaymentCashListItemsCollection(0).CoreCashList.StatusCode
                                            oPaymentCashListItem.CoreCashList.TypeCode = oPaymentCashListItemsCollection(0).CoreCashList.TypeCode
                                            oPaymentCashListItem.CoreCashList.CashListKey = oCashClaimLink.CashListKey
                                            oPaymentItem = New NexusProvider.PaymentItems
                                            With oPaymentItem
                                                .Address = oCashListItemRet.Address
                                                .AccountShortCode = oCashListItemRet.AccountShortCode
                                                .AllocationStatusCode = oCashListItemRet.AllocationStatusCode
                                                .Amount = oCashListItemRet.Amount
                                                .Bank = oCashListItemRet.Bank
                                                .BankReference = oCashListItemRet.BankReference
                                                .CashListItemKey = oCashListItemRet.CashListItemKey
                                                .ContactName = oCashListItemRet.ContactName
                                                .CreditCard = oCashListItemRet.CreditCard
                                                .FurtherDetails = oCashListItemRet.FurtherDetails
                                                .IsProduceDocument = oCashListItemRet.IsProduceDocument
                                                .Letter = oCashListItemRet.Letter
                                                .MediaReference = oCashListItemRet.MediaReference
                                                .MediaTypeCode = oCashListItemRet.MediaTypeCode
                                                .OurReference = oCashListItemRet.OurReference
                                                .SkipPosting = False
                                                .StatusCode = oCashListItemRet.StatusCode
                                                .TaxAmount = oCashListItemRet.TaxAmount
                                                .TaxBandCode = oCashListItemRet.TaxBandCode
                                                .TaxBandKey = oCashListItemRet.TaxBandKey
                                                .TheirReference = oCashListItemRet.TheirReference
                                                .TransactionDate = oCashListItemRet.TransactionDate
                                                .TypeCode = oCashListItemRet.TypeCode
                                            End With
                                            oPaymentCashListItem.PaymentItems.Add(oPaymentItem)
                                        Next
                                    End If
                                    'post cashlist here
                                    oWebservice.CreatePaymentCashListWithItems(oPaymentCashListItem)

                                    If ddlStatus.Value = "ISS" Then
                                        'Finding of the Transdetails Key
                                        Dim oAccountDetails As New NexusProvider.AccountDetails
                                        Dim oAccountDetailsDefaults As New NexusProvider.AccountDetailsDefaults
                                        oAccountDetails.DocumentRef = oUnallocatedClaimPayments.DocumentRef
                                        oAccountDetails.AccountKey = oUnallocatedClaimPayments.AccountKey
                                        Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                                        Dim sSourceIds As String = String.Empty
                                        For iCount As Integer = 0 To oUserDetails.ListOfBranches.Count - 1
                                            If oUserDetails.ListOfBranches(iCount).BranchKey > 0 Then
                                                sSourceIds = sSourceIds & oUserDetails.ListOfBranches(iCount).BranchKey & ","
                                            End If
                                        Next
                                        If Not String.IsNullOrEmpty(sSourceIds) Then
                                            sSourceIds = Left(sSourceIds, Len(sSourceIds) - 1)
                                            oAccountDetails.SourceArray = sSourceIds
                                        Else
                                            sSourceIds = "1"
                                            oAccountDetails.SourceArray = sSourceIds
                                        End If
                                        oAccountDetailsDefaults = oWebservice.GetAccountDetails(oAccountDetails)
                                        'Assignment of the Transdetails Key
                                        oAllocationDetails.TransdetailKey = oAccountDetailsDefaults.AccountDetails(0).TransDetailKeys
                                        oAllocationDetailsCollections.Add(oAllocationDetails)
                                        oTrasactionDetails = oWebservice.GetTransactionDetails(iAccountKey, oAllocationDetailsCollections)

                                        For Each oTempAllocationDetails As NexusProvider.AllocationDetails In oTrasactionDetails
                                            oAllocation = New NexusProvider.Allocation
                                            oAllocation.AllocationAmount = oTempAllocationDetails.Amount
                                            oAllocation.AllocationTimeStamp = oTempAllocationDetails.AllocationTimeStamp
                                            oAllocation.AllocationTransdetailKey = oTempAllocationDetails.TransdetailKey
                                            oTransAllocationDetails.Allocation.Add(oAllocation)
                                            oAllocation = Nothing
                                        Next
                                        oTransAllocationDetails.AccountKey = iAccountKey
                                        oTransAllocationDetails.CashListItemKey = oPaymentCashListItem.PaymentCashList(0).CashListItemKey
                                        oTransAllocationDetails.Amount = -dAmount
                                        oTransAllocationDetails.TransdetailKey = oPaymentCashListItem.PaymentCashList(0).TransDetailKey
                                        'Allocation done here
                                        bIsUpdated = oWebservice.UpdateAllocation(oTransAllocationDetails)
                                        If bIsUpdated Then
                                            Response.Redirect("~/secure/AuthoriseClaimPayments.aspx")
                                        End If
                                    Else
                                        Response.Redirect("~/secure/AuthoriseClaimPayments.aspx")
                                    End If
                                Else
                                    'Authorise CLaim Payments,Claim Payment processing
                                    oCashListItems = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                                    PopulateObject() 'Population of the fields with values
                                    oCashListItems.PaymentItems.Add(oCashListItem)

                                    'Finding of the Transdetails Key
                                    oWebservice = New NexusProvider.ProviderManager().Provider
                                    oWebservice.CreatePaymentCashListWithItems(oCashListItems)
                                    If ddlStatus.Value = "ISS" Then
                                        Dim oAccountDetails As New NexusProvider.AccountDetails
                                        Dim oAccountDetailsDefaults As New NexusProvider.AccountDetailsDefaults
                                        oAccountDetails.DocumentRef = oUnallocatedClaimPayments.DocumentRef
                                        oAccountDetails.AccountKey = oUnallocatedClaimPayments.AccountKey
                                        oAccountDetailsDefaults = oWebservice.GetAccountDetails(oAccountDetails)
                                        'Assignment of the Transdetails Key
                                        oAllocationDetails.TransdetailKey = oAccountDetailsDefaults.AccountDetails(0).TransDetailKeys
                                        oAllocationDetailsCollections.Add(oAllocationDetails)
                                        oTrasactionDetails = oWebservice.GetTransactionDetails(iAccountKey, oAllocationDetailsCollections)

                                        For Each oTempAllocationDetails As NexusProvider.AllocationDetails In oTrasactionDetails
                                            oAllocation = New NexusProvider.Allocation
                                            oAllocation.AllocationAmount = oTempAllocationDetails.Amount
                                            oAllocation.AllocationTimeStamp = oTempAllocationDetails.AllocationTimeStamp
                                            oAllocation.AllocationTransdetailKey = oTempAllocationDetails.TransdetailKey
                                            oTransAllocationDetails.Allocation.Add(oAllocation)
                                            oAllocation = Nothing
                                        Next
                                        oTransAllocationDetails.AccountKey = iAccountKey
                                        oTransAllocationDetails.CashListItemKey = oCashListItems.PaymentCashList(0).CashListItemKey
                                        oTransAllocationDetails.Amount = -dAmount
                                        oTransAllocationDetails.TransdetailKey = oCashListItems.PaymentCashList(0).TransDetailKey

                                        bIsUpdated = oWebservice.UpdateAllocation(oTransAllocationDetails)
                                        If bIsUpdated Then
                                            Response.Redirect("~/secure/AuthoriseClaimPayments.aspx")
                                        End If
                                    Else
                                        Response.Redirect("~/secure/AuthoriseClaimPayments.aspx")
                                    End If
                                End If
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
                                End If
                            Finally
                                oWebservice = Nothing
                                Session(CNUnAllocatedClaimPayment) = Nothing
                                oAllocationDetails = Nothing
                                oTransAllocationDetails = Nothing
                                oAllocationDetailsCollections = Nothing
                                oTrasactionDetails = Nothing
                            End Try

                        ElseIf Session(CNUnAllocatedClaimPayment) IsNot Nothing Then

                            'Authorise CLaim Payments,Claim Payment processing
                            oCashListItems = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                            PopulateObject() 'Population of the fields with values

                            If GISLookup_PaymentType IsNot Nothing AndAlso Trim(GISLookup_PaymentType.Value) = "CLP" Then
                                oCashListItem.Amount = -1 * oCashListItem.Amount
                                oCashListItem.Amount_tendered = -1 * oCashListItem.Amount_tendered
                            End If

                            oCashListItems.PaymentItems.Add(oCashListItem)
                            Dim oAllocationDetailsCollections As New NexusProvider.AllocationDetailsCollections
                            Dim oTrasactionDetails As New NexusProvider.AllocationDetailsCollections
                            Dim oUnallocatedClaimPayments As NexusProvider.UnallocatedClaimPayments = CType(Session(CNUnAllocatedClaimPayment), NexusProvider.UnallocatedClaimPayments)
                            Dim iAccountKey As Integer = oUnallocatedClaimPayments.AccountKey
                            Dim dAmount As Double = oUnallocatedClaimPayments.Amount
                            Dim oAllocationDetails As NexusProvider.AllocationDetails
                            Dim oAllocation As NexusProvider.Allocation
                            Dim oTransAllocationDetails As New NexusProvider.AllocationDetails
                            Dim bIsUpdated As Boolean
                            Try
                                'Finding of the Transdetails Key
                                oWebservice = New NexusProvider.ProviderManager().Provider
                                oWebservice.CreatePaymentCashListWithItems(oCashListItems)
                                Dim oAccountDetails As New NexusProvider.AccountDetails
                                Dim oAccountDetailsDefaults As New NexusProvider.AccountDetailsDefaults
                                oAccountDetails.DocumentRef = oUnallocatedClaimPayments.DocumentRef
                                oAccountDetails.AccountKey = oUnallocatedClaimPayments.AccountKey
                                oAccountDetailsDefaults = oWebservice.GetAccountDetails(oAccountDetails)
                                'Assignment of the Transdetails Key
                                For Each oTempAccountDetails As NexusProvider.AccountDetails In oAccountDetailsDefaults.AccountDetails
                                    oAllocationDetails = New NexusProvider.AllocationDetails
                                    oAllocationDetails.TransdetailKey = oTempAccountDetails.TransDetailKeys
                                    oAllocationDetailsCollections.Add(oAllocationDetails)
                                Next

                                oTrasactionDetails = oWebservice.GetTransactionDetails(iAccountKey, oAllocationDetailsCollections)

                                For Each oTempAllocationDetails As NexusProvider.AllocationDetails In oTrasactionDetails
                                    oAllocation = New NexusProvider.Allocation
                                    oAllocation.AllocationAmount = oTempAllocationDetails.Amount
                                    oAllocation.AllocationTimeStamp = oTempAllocationDetails.AllocationTimeStamp
                                    oAllocation.AllocationTransdetailKey = oTempAllocationDetails.TransdetailKey
                                    oTransAllocationDetails.Allocation.Add(oAllocation)
                                    oAllocation = Nothing
                                Next
                                oTransAllocationDetails.AccountKey = iAccountKey
                                oTransAllocationDetails.CashListItemKey = oCashListItems.PaymentCashList(0).CashListItemKey
                                oTransAllocationDetails.Amount = -dAmount
                                oTransAllocationDetails.TransdetailKey = oCashListItems.PaymentCashList(0).TransDetailKey

                                If oTransAllocationDetails.TransdetailKey > 0 Then
                                    bIsUpdated = oWebservice.UpdateAllocation(oTransAllocationDetails)
                                Else
                                    bIsUpdated = True
                                End If
                                oWebservice.AddCashClaimLink(oUnallocatedClaimPayments.ClaimPaymentKey, oCashListItems.PaymentCashList(0).CashListItemKey)
                                If bIsUpdated Then
                                    If Session(CNMode) IsNot Nothing AndAlso Session(CNMode) = Mode.Authorise Then
                                        Response.Redirect("~/secure/AuthoriseClaimPayments.aspx", False)
                                    Else
                                        Response.Redirect("~/secure/payment/claimpaymentprocessing.aspx", False)
                                    End If
                                End If
                            Finally
                                oWebservice = Nothing
                                Session(CNUnAllocatedClaimPayment) = Nothing
                                oAllocationDetails = Nothing
                                oTransAllocationDetails = Nothing
                                oAllocationDetailsCollections = Nothing
                                oTrasactionDetails = Nothing
                            End Try
                        ElseIf Session("ModeValue") = "INS" Then
                            Dim oFinancePlanTransactionsCollection As New NexusProvider.FinancePlanTransactionsCollection
                            Dim oFinancePlanDetails As New NexusProvider.FinancePlanDetails
                            Dim oReceiptCashListCollection As NexusProvider.ReceiptCashListCollection
                            If Session(CNFinancePlanDetails) IsNot Nothing Then
                                oFinancePlanTransactionsCollection = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).Transactions
                                oFinancePlanDetails = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PremiumFinanceDetails
                            End If

                            oReceiptCashListItems = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                            PopulateObject()

                            If Request("__EVENTARGUMENT") <> "ContinueAfterMulticurrency" Then
                                oReceiptCashListItems.ReceiptItems.Add(oCashListItem)
                                Session(CNCashListItem) = oReceiptCashListItems
                            End If

                            Session("CNCashListItemPending") = oCashListItem
                                If OpenModal(txtAccount.Text.Trim(), txtAmount.Text, Cash_List_Item__Transaction_Date.Text, oReceiptCashListItems.CoreCashList.CurrencyCode, "Payment") Then
                                    Return
                                End If

                                Dim oAllocationDetailsCollections As New NexusProvider.AllocationDetailsCollections
                                Dim oTrasactionDetails As New NexusProvider.AllocationDetailsCollections
                                Dim iAccountKey As Integer = oFinancePlanTransactionsCollection(0).AccountKey
                                Dim dAmount As Double = oFinancePlanDetails.SettlementAmount
                                Dim oAllocationDetails As New NexusProvider.AllocationDetails

                                Dim oTransAllocationDetails As New NexusProvider.AllocationDetails
                                Dim oAllocation As NexusProvider.Allocation
                                Dim bIsUpdated As Boolean = True

                                Try
                                    'Finding of the Transdetails Key from Response object of CreateReceiptcashListWithItem
                                    oWebservice = New NexusProvider.ProviderManager().Provider
                                    oReceiptCashListCollection = oWebservice.CreateReceiptcashListWithItem(oReceiptCashListItems)
                                    ''If Plan is settled DebitTransDetailKey has to be used for auto allocation which is of 'SED'
                                    If Session(CNDebitTransDetailkey) IsNot Nothing AndAlso CType(Session(CNDebitTransDetailkey), Integer) <> 0 Then
                                        'Assignment of the Transdetails Key
                                        oAllocationDetails.TransdetailKey = CType(Session(CNDebitTransDetailkey), Integer)
                                        oAllocationDetailsCollections.Add(oAllocationDetails)
                                        oTrasactionDetails = oWebservice.GetTransactionDetails(iAccountKey, oAllocationDetailsCollections)

                                        For Each oTempAllocationDetails As NexusProvider.AllocationDetails In oTrasactionDetails
                                            oAllocation = New NexusProvider.Allocation
                                            oAllocation.AllocationAmount = oTempAllocationDetails.Amount
                                            oAllocation.AllocationTimeStamp = oTempAllocationDetails.AllocationTimeStamp
                                            oAllocation.AllocationTransdetailKey = oTempAllocationDetails.TransdetailKey
                                            oTransAllocationDetails.Allocation.Add(oAllocation)
                                            oAllocation = Nothing
                                        Next
                                        oTransAllocationDetails.AccountKey = iAccountKey
                                        oTransAllocationDetails.CashListItemKey = oReceiptCashListCollection(0).CashListItemKey
                                        oTransAllocationDetails.Amount = -dAmount
                                        oTransAllocationDetails.TransdetailKey = oReceiptCashListCollection(0).TransDetailKey

                                        bIsUpdated = oWebservice.UpdateAllocation(oTransAllocationDetails)
                                    End If
                                    If bIsUpdated Then
                                        ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "RedirectToPremiumFinancePlan", "RedirectToPremiumFinancePlan();", True)
                                    End If
                                Catch ex As Exception
                                Finally
                                    If bIsUpdated Then
                                        Session(CNDebitTransDetailkey) = 0
                                    End If
                                End Try

                            End If
                        End If
                    If Request.QueryString.HasKeys() = False Then
                        Dim oMultiStepApproval As NexusProvider.OptionTypeSetting = Nothing
                        oMultiStepApproval = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 65)
                        If Session(CNClaim) IsNot Nothing AndAlso Session(CNUnAllocatedClaimPayment) Is Nothing AndAlso Session(CNMode) <> Mode.Recommend Then
                            bIsIncludePaymentTypeClaimPayment = IsClaimIncludedInPayment()

                            ' This functionality is for Claim Payments
                            Dim oPayment As NexusProvider.ClaimPayment = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(Session(CNClaimPerilIndex)).Payment
                            Dim oPaymentItem As New NexusProvider.PaymentCashListItemType
                            Dim oClaimResponse As NexusProvider.ClaimResponse = Nothing
                            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                            Dim bTimeStamp As Byte() = CType(Session(CNClaimTimeStamp), Byte())
                            oWebservice = New NexusProvider.ProviderManager().Provider
                            GISLookup_PaymentType.Value = "CLP"
                            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

                            With oPaymentItem
                                .AccountShortCode = oPayment.PartyPaidCode
                                .MediaTypeCode = GISLookup_MediaType.SelectedValue
                                .TypeCode = GISLookup_PaymentType.Value
                                .TransactionDate = CDate(IIf(Trim(Cash_List_Item__Transaction_Date.Text) <> String.Empty, Trim(Cash_List_Item__Transaction_Date.Text), DateTime.MinValue))
                                .Amount = CDec(Trim(txtAmount.Text))
                                .BankPaymentType.AccountCode = Trim(txtAccountCode.Text)
                                .BankPaymentType.BranchCode = Trim(txtBranchCode.Text)
                                .BankPaymentType.ExpiryDate = CDate(IIf(Trim(txtExpiryDate.Text) <> String.Empty, Trim(txtExpiryDate.Text), DateTime.MinValue))
                                .BankPaymentType.PayeeName = Trim(txtPayeeName.Text)
                                .BankPaymentType.Reference1 = Trim(txtReference1.Text)
                                .BankPaymentType.Reference2 = Trim(txtReference2.Text)
                                .BankReference = Trim(txtBankReference.Text)
                                .FurtherDetails = txtDetails.Text
                                .MediaReference = Trim(txtMediaReference.Text)
                                .OurReference = Trim(txtOurReference.Text)
                                .TheirReference = Trim(txtTheirReference.Text)
                                .StatusCode = ddlStatus.Value
                                .BankPaymentType.BIC = Trim(txtBIC.Text)
                                .BankPaymentType.IBAN = Trim(txtIBAN.Text)
                                'Get OnScreen Address info
                                Dim oAddress As New NexusProvider.Address
                                If PayNow_Address.Address IsNot Nothing Then
                                    oAddress.Address1 = If(PayNow_Address.Address1 Is Nothing, "", PayNow_Address.Address1.Trim)
                                    oAddress.Address2 = If(PayNow_Address.Address2 Is Nothing, "", PayNow_Address.Address2.Trim)
                                    oAddress.Address3 = If(PayNow_Address.Address3 Is Nothing, "", PayNow_Address.Address3.Trim)
                                    oAddress.Address4 = If(PayNow_Address.Address4 Is Nothing, "", PayNow_Address.Address4.Trim)
                                    oAddress.CountryCode = Trim(PayNow_Address.CountryCode)
                                    oAddress.PostCode = PayNow_Address.Postcode.Trim
                                End If
                                .ContactAddress = oAddress
                                .ChequeDate = oPayment.PaymentDate
                            End With

                            oPayment.CashList.PaymentCashListItemType.Add(oPaymentItem)

                            If oMultiStepApproval.OptionValue = "1" And bIsIncludePaymentTypeClaimPayment Then
                                bApprovalMessage = True
                            Else
                                bApprovalMessage = False
                            End If
                            If oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID()).Claims.ShowSummary = False Then
                                If bApprovalMessage = True Then
                                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ClaimApprovalAlert", "alert('" & GetLocalResourceObject("msg_ApprovalAlert").ToString() & "');", True)
                                End If
                                SkipSummaryPage()
                            Else
                                If bApprovalMessage = True Then
                                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ClaimApprovalAlert", "alert('" & GetLocalResourceObject("msg_ApprovalAlert").ToString() & "');window.location.href = '../../Claims/summary.aspx';", True)
                                Else
                                    Response.Redirect("~/Claims/summary.aspx", False)
                                End If

                            End If
                        ElseIf Session(CNUnAllocatedClaimPayment) IsNot Nothing Then

                            'Authorise CLaim Payments,Claim Payment processing
                            oCashListItems = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                            PopulateObject() 'Population of the fields with values

                            If GISLookup_PaymentType IsNot Nothing AndAlso Trim(GISLookup_PaymentType.Value) = "CLP" Then
                                oCashListItem.Amount = -1 * oCashListItem.Amount
                                oCashListItem.Amount_tendered = -1 * oCashListItem.Amount_tendered
                            End If

                            oCashListItems.PaymentItems.Add(oCashListItem)
                            Dim oAllocationDetailsCollections As New NexusProvider.AllocationDetailsCollections
                            Dim oTrasactionDetails As New NexusProvider.AllocationDetailsCollections
                            Dim oUnallocatedClaimPayments As NexusProvider.UnallocatedClaimPayments = CType(Session(CNUnAllocatedClaimPayment), NexusProvider.UnallocatedClaimPayments)
                            Dim iAccountKey As Integer = oUnallocatedClaimPayments.AccountKey
                            Dim dAmount As Double = oUnallocatedClaimPayments.Amount
                            Dim oAllocationDetails As NexusProvider.AllocationDetails
                            Dim oAllocation As NexusProvider.Allocation
                            Dim oTransAllocationDetails As New NexusProvider.AllocationDetails
                            Dim bIsUpdated As Boolean
                            Try
                                'Finding of the Transdetails Key
                                oWebservice = New NexusProvider.ProviderManager().Provider
                                oWebservice.CreatePaymentCashListWithItems(oCashListItems)
                                Dim oAccountDetails As New NexusProvider.AccountDetails
                                Dim oAccountDetailsDefaults As New NexusProvider.AccountDetailsDefaults
                                oAccountDetails.DocumentRef = oUnallocatedClaimPayments.DocumentRef
                                oAccountDetails.AccountKey = oUnallocatedClaimPayments.AccountKey
                                oAccountDetailsDefaults = oWebservice.GetAccountDetails(oAccountDetails)
                                'Assignment of the Transdetails Key
                                For Each oTempAccountDetails As NexusProvider.AccountDetails In oAccountDetailsDefaults.AccountDetails
                                    oAllocationDetails = New NexusProvider.AllocationDetails
                                    oAllocationDetails.TransdetailKey = oTempAccountDetails.TransDetailKeys
                                    oAllocationDetailsCollections.Add(oAllocationDetails)
                                Next

                                oTrasactionDetails = oWebservice.GetTransactionDetails(iAccountKey, oAllocationDetailsCollections)

                                For Each oTempAllocationDetails As NexusProvider.AllocationDetails In oTrasactionDetails
                                    oAllocation = New NexusProvider.Allocation
                                    oAllocation.AllocationAmount = oTempAllocationDetails.Amount
                                    oAllocation.AllocationTimeStamp = oTempAllocationDetails.AllocationTimeStamp
                                    oAllocation.AllocationTransdetailKey = oTempAllocationDetails.TransdetailKey
                                    oTransAllocationDetails.Allocation.Add(oAllocation)
                                    oAllocation = Nothing
                                Next
                                oTransAllocationDetails.AccountKey = iAccountKey
                                oTransAllocationDetails.CashListItemKey = oCashListItems.PaymentCashList(0).CashListItemKey
                                oTransAllocationDetails.Amount = -dAmount
                                oTransAllocationDetails.TransdetailKey = oCashListItems.PaymentCashList(0).TransDetailKey

                                If oTransAllocationDetails.TransdetailKey > 0 Then
                                    bIsUpdated = oWebservice.UpdateAllocation(oTransAllocationDetails)
                                Else
                                    bIsUpdated = True
                                End If
                                oWebservice.AddCashClaimLink(oUnallocatedClaimPayments.ClaimPaymentKey, oCashListItems.PaymentCashList(0).CashListItemKey)
                                bIsIncludePaymentTypeClaimPayment = IsClaimIncludedInPayment()
                                If bIsUpdated Then
                                    Session("ModeType") = "BulkCLP"
                                    If Session(CNMode) IsNot Nothing AndAlso Session(CNMode) = Mode.Authorise Then
                                        Response.Redirect("~/secure/AuthoriseClaimPayments.aspx", False)
                                    Else
                                        If oMultiStepApproval.OptionValue = "1" And bIsIncludePaymentTypeClaimPayment Then
                                            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ClaimApprovalAlert", "alert('" & GetLocalResourceObject("msg_ApprovalAlert").ToString() & "');window.location.href = '../../secure/payment/claimpaymentprocessing.aspx';", True)
                                        Else
                                            Response.Redirect("~/secure/payment/claimpaymentprocessing.aspx", False)
                                        End If
                                    End If
                                End If
                            Finally
                                oWebservice = Nothing
                                Session(CNUnAllocatedClaimPayment) = Nothing
                                oAllocationDetails = Nothing
                                oTransAllocationDetails = Nothing
                                oAllocationDetailsCollections = Nothing
                                oTrasactionDetails = Nothing
                            End Try
                        End If
                    ElseIf Request.QueryString("Mode") = "INS" Then
                        Dim oFinancePlanTransactionsCollection As New NexusProvider.FinancePlanTransactionsCollection
                        Dim oFinancePlanDetails As New NexusProvider.FinancePlanDetails
                        Dim oReceiptCashListCollection As NexusProvider.ReceiptCashListCollection
                        If Session(CNFinancePlanDetails) IsNot Nothing Then
                            oFinancePlanTransactionsCollection = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).Transactions
                            oFinancePlanDetails = CType(Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).PremiumFinanceDetails
                        End If

                        oReceiptCashListItems = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                        PopulateObject()

                        If callbtnOk Then
                            oReceiptCashListItems.ReceiptItems.Add(oCashListItem)
                            Session(CNCashListItem) = oReceiptCashListItems
                        End If

                        Dim oAllocationDetailsCollections As New NexusProvider.AllocationDetailsCollections
                        Dim oTrasactionDetails As New NexusProvider.AllocationDetailsCollections
                        Dim iAccountKey As Integer = oFinancePlanTransactionsCollection(0).AccountKey
                        Dim dAmount As Double = oFinancePlanDetails.SettlementAmount
                        Dim oAllocationDetails As New NexusProvider.AllocationDetails

                        Dim oTransAllocationDetails As New NexusProvider.AllocationDetails
                        Dim oAllocation As NexusProvider.Allocation
                        Dim bIsUpdated As Boolean = True

                        Try
                            'Finding of the Transdetails Key from Response object of CreateReceiptcashListWithItem
                            oWebservice = New NexusProvider.ProviderManager().Provider
                            oReceiptCashListCollection = oWebservice.CreateReceiptcashListWithItem(oReceiptCashListItems)
                            ''If Plan is settled DebitTransDetailKey has to be used for auto allocation which is of 'SED'
                            If Session(CNDebitTransDetailkey) IsNot Nothing AndAlso CType(Session(CNDebitTransDetailkey), Integer) <> 0 Then
                                'Assignment of the Transdetails Key
                                oAllocationDetails.TransdetailKey = CType(Session(CNDebitTransDetailkey), Integer)
                                oAllocationDetailsCollections.Add(oAllocationDetails)
                                oTrasactionDetails = oWebservice.GetTransactionDetails(iAccountKey, oAllocationDetailsCollections)

                                For Each oTempAllocationDetails As NexusProvider.AllocationDetails In oTrasactionDetails
                                    oAllocation = New NexusProvider.Allocation
                                    oAllocation.AllocationAmount = oTempAllocationDetails.Amount
                                    oAllocation.AllocationTimeStamp = oTempAllocationDetails.AllocationTimeStamp
                                    oAllocation.AllocationTransdetailKey = oTempAllocationDetails.TransdetailKey
                                    oTransAllocationDetails.Allocation.Add(oAllocation)
                                    oAllocation = Nothing
                                Next
                                oTransAllocationDetails.AccountKey = iAccountKey
                                oTransAllocationDetails.CashListItemKey = oReceiptCashListCollection(0).CashListItemKey
                                oTransAllocationDetails.Amount = -dAmount
                                oTransAllocationDetails.TransdetailKey = oReceiptCashListCollection(0).TransDetailKey

                                bIsUpdated = oWebservice.UpdateAllocation(oTransAllocationDetails)
                            End If
                            If bIsUpdated Then
                                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "RedirectToPremiumFinancePlan", "RedirectToPremiumFinancePlan();", True)
                            End If
                        Catch ex As Exception
                        Finally
                            If bIsUpdated Then
                                Session(CNDebitTransDetailkey) = 0
                            End If
                        End Try
                    ElseIf Session("ModeValue") = "INSDEPOSIT" Then
                        oReceiptCashListItems = New NexusProvider.ReceiptCashListItemType
                        PopulateObject() ''check values updated from UI
                        oReceiptCashListItems = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                        If callbtnOk Then
                            oReceiptCashListItems.ReceiptItems.Add(oCashListItem)
                            Session(CNCashListItem) = oReceiptCashListItems
                        End If
                        Session("CNCashListItemPending") = oCashListItem
                        If OpenModal(txtAccount.Text.Trim(), txtAmount.Text, Cash_List_Item__Transaction_Date.Text, oReceiptCashListItems.CoreCashList.CurrencyCode, "Receipt") Then
                            Return
                        End If
                        If GISLookup_MediaType.SelectedValue = "OCP" AndAlso Session(CNInstalmentMediaType) IsNot Nothing AndAlso Session(CNInstalmentMediaType).ToString().ToUpper() = "CREDIT CARD" AndAlso ViewState("PaymentHubEnabled") = "1" AndAlso Session(CNCardDetails) IsNot Nothing Then
                            Dim oPaymentItem As NexusProvider.PaymentHubDetails
                            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                            Dim oCreditCard As NexusProvider.CreditCard = Session(CNCardDetails)
                            If Session(CNPaymentHubDetails) Is Nothing Then
                                oPaymentItem = New NexusProvider.PaymentHubDetails
                            Else
                                oPaymentItem = Session(CNPaymentHubDetails)
                            End If
                            oPaymentItem.TransactionAmount = txtAmount.Text
                            oPaymentItem.TransactionCurrency = oReceiptCashListItems.CoreCashList.CurrencyCode
                            oPaymentItem.ReturnURL = "~/secure/TransactionConfirmation.aspx?Mode=INSDEPOSIT"
                            oPaymentItem.TokenID = oCreditCard.TrackingNumber
                            oPaymentItem.IntegrationToken = oCreditCard.AuthCode
                            PaymentHubProcessPurchase(oPaymentItem)
                            If oPaymentItem.ResultDescription = "0" Then
                                Session(CNPaid) = True
                            Else
                                oPaymentItem.ResultDescription = PaymentHub.ResultDescription.Declined
                                Session(CNPaid) = False
                            End If
                            Session(CNPaymentHubDetails) = oPaymentItem
                            Response.Redirect(oPaymentItem.ReturnURL, False)
                        Else
                            Response.Redirect("~/secure/TransactionConfirmation.aspx?Mode=INSDEPOSIT", False)
                        End If
                    End If
                End If
                ManageControlsForNextClick()
            Else
                NextTab = "tab-CashListItem"
            End If



        End Sub
        Public Sub cancelBtnClick()
            If Session("ModeValue") = "VP" OrElse Session("ModeValue") = "AP" OrElse Session("ModeValue") = "DP" Then
                Response.Redirect("~/secure/AuthorizePayments.aspx?Type=Task&CashListItemKey=" & Request.QueryString("CashListItemKey") & " & Mode = " & Session("ModeValue") & "", True)
            End If
            If CType(Session(CNMode), Mode) = Mode.PayClaim Then
                'Claim Payment
                Response.Redirect("~/secure/Payment/CashList.aspx", True)
            ElseIf Session(CNUnAllocatedClaimPayment) IsNot Nothing Or Session(CNMode) = Mode.Recommend Then
                'Claim Payment Processing
                Response.Redirect("~/secure/payment/CashList.aspx", False)
            ElseIf Session(CNQuoteCollectionFiles) IsNot Nothing Then
                'Mark For Collection
                Response.Redirect("~/secure/QuoteCollection.aspx", True)
            ElseIf Session("ModeValue") = "IP" Then
                'Insurer Payments
                Dim sType As String = Session("Type")
                'Server.Transfer("~/secure/payment/CashListItems.aspx?Mode=IP&Type=" + sType)
                Session("ModeValue") = "IP"
                Session("Type") = sType
                'CashListItems
                Dim changeTab2 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(2) a').tab('show')});"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab2", changeTab2, True)
                'Session("hfActiveTab") = 2
                Session("hfPreviousTab") = 2
            ElseIf Session("ModeType") = "Payment" Or Session("ModeType") = "Receipt" AndAlso Session("ModeValue") <> "INS" AndAlso Session("ModeValue") <> "INSDEPOSIT" Then

                If Session("INSTALMENTPLANDETAILS") IsNot Nothing Then
                    oReceiptCashListItems = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                    If oReceiptCashListItems.ReceiptItems.Count > 0 Then
                        oReceiptCashListItems.ReceiptItems.Remove(oReceiptCashListItems.ReceiptItems.Count - 1)
                    End If
                    txtOverAllSelectedTotal.Text = String.Empty
                End If
                Session("INSTALMENTPLANDETAILS") = Nothing
                'Server.Transfer("~/secure/payment/CashListItems.aspx?Mode=INST")
                Session("ModeValue") = "INST"
                'CashListItems
            ElseIf Session("ModeValue") = "INS" Then
                Response.Redirect("~/PremiumFinance/PremiumFinancePlan.aspx?Type=EditPlan", True)
            ElseIf Session("ModeValue") = "INSDEPOSIT" Then
                Response.Redirect("~/secure/payment/CashList.aspx?Mode=INSDEPOSIT")
                Session("ModeValue") = "INSDEPOSIT"
                'CashList
                Dim changeTab0 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(0) a').tab('show')});"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab0", changeTab0, True)
                'Session("hfActiveTab") = 0
                Session("hfPreviousTab") = 0
            Else
                Response.Redirect("~/secure/PremiumDisplay.aspx", True)
            End If
        End Sub
        'delete later

        Public Sub PageLoad()
            If Not Page.IsPostBack Then
                Session("btnOKClicked") = Nothing
            End If
            sCreatedBy = Nothing
            txtTotalAmount.Visible = False
            lblTotalAmount.Visible = False
            rvtxtTotalAmount.Enabled = False
            If (Request.QueryString("Mode") = "CR") Then
                btnOk.Visible = True
            End If
            chkProduceDocument.Checked = False
            If Not Request.QueryString("CashListItemKey") Is Nothing AndAlso Not String.IsNullOrEmpty(Request.QueryString("CashListItemKey")) Then
                oWebservice = New NexusProvider.ProviderManager().Provider
                oCashListItems = oWebservice.GetPaymentCashListItemDetails(Request.QueryString("CashListItemKey"))
                sCreatedBy = oCashListItems.UserName
            End If
            Dim oNexusFrameWork As Nexus.Library.Config.NexusFrameWork = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork)
            hdnTabName = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("hdnTabName"), HiddenField)
            hdnAddMoreCashList = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("hdnAddMoreCashList"), HiddenField)
            CashListItemID = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("hfCashListItemID"), HiddenField)
            hfMode = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("hfMode"), HiddenField)
            hfType = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("hfType"), HiddenField)

            Dim aa As Boolean = CBool(Session("CashListItemFirstLoad"))

            If Request("__EVENTARGUMENT") = "EditClick" Then
                Session("ModeValue") = hfMode.Value
                Session("Type") = hfType.Value
                Session("SetFlag") = 0
            End If


            If True Then
                If Visible Then

                    If Request.QueryString("error") IsNot Nothing AndAlso Request.QueryString("error") = "Token" Then
                        Session(CNPaymentHubDetails) = Nothing
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowValidation", "alert('" + GetLocalResourceObject("msg_ErrorTokenRegistration").ToString() + "');", True)

                    End If
                    'if control is NOT visible and NOT postback, aviod all actions on page_load
                    Dim iFlag As Integer = Session("SetFlag")
                    Dim sType As String = IIf(String.IsNullOrEmpty(Session("Type")), Request.QueryString("Type"), Session("Type"))
                    Dim sSetFocusOnControl As String = Nothing
                    'test 
                    'setDefaultDatesAndHiddenControlValues()

                    If sType <> "Task" Then
                        sCreatedBy = String.Empty
                    End If

                    If sType = "Task" And Not String.IsNullOrEmpty(Request.QueryString("CashListItemKey")) Then
                        'This code executes when this page is called from task i.e Run the Task
                        'Approval of Cash/Cheque Payments(Multi Step Approval) item through Work Manager

                        oWebservice = New NexusProvider.ProviderManager().Provider
                        oCashListItems = oWebservice.GetPaymentCashListItemDetails(Request.QueryString("CashListItemKey"))
                        Session(CNTimeStamp) = oCashListItems.TimeStamp

                        Dim oPaymentCashList As New NexusProvider.PaymentCashListItemType
                        ManageControlsForTaskActivity() 'this will manage the task for task activity
                        'Load the Media Type as per IsReceipt or IsPayment
                        FillMediaType()

                        With oCashListItems
                            sCreatedBy = .UserName
                            GISLookup_PaymentType.Value = .TypeCode.Trim
                            If .IsProduceDocument Then
                                chkProduceDocument.Checked = True
                            Else
                                chkProduceDocument.Checked = False
                            End If

                            Cash_List_Item__Transaction_Date.Text = .TransactionDate
                            GISLookup_MediaType.SelectedValue = .MediaTypeCode.Trim
                            txtMediaReference.Text = .MediaReference
                            txtBankReference.Text = .BankReference
                            txtOurReference.Text = .OurReference
                            txtTheirReference.Text = .TheirReference

                            txtAccount.Text = .AccountShortCode
                            txtAllocationStatus.Text = .AllocationStatusCode
                            ddlStatus.Value = .StatusCode
                            txtName.Text = .ContactName
                            txtDetails.Text = .FurtherDetails
                            If .Amount < 0 Then
                                txtAmount.Text = .Amount * -1
                            Else
                                txtAmount.Text = .Amount
                            End If

                            If .Bank IsNot Nothing Then
                                txtPayeeName.Text = .Bank.PayeeName
                                txtAccountCode.Text = .Bank.AccountCode
                                txtExpiryDate.Text = .Bank.ExpiryDate
                                txtBranchCode.Text = .Bank.BranchCode
                                txtReference1.Text = .Bank.Reference1
                                txtReference2.Text = .Bank.Reference2
                                txtBIC.Text = .Bank.BIC
                                txtIBAN.Text = .Bank.IBAN
                            End If

                            If .ContactAddress IsNot Nothing Then
                                PayNow_Address.Address1 = .ContactAddress.Address1.Trim
                                PayNow_Address.Address2 = .ContactAddress.Address2.Trim
                                PayNow_Address.Address3 = .ContactAddress.Address3.Trim
                                PayNow_Address.Address4 = .ContactAddress.Address4.Trim
                                PayNow_Address.CountryCode = .ContactAddress.CountryCode.Trim
                                PayNow_Address.Postcode = .ContactAddress.PostCode.Trim
                            End If

                        End With

                        sSetFocusOnControl = GISLookup_PaymentType.ClientID

                    ElseIf Session("ModeValue") = "IP" Then
                        'User is doing the Insurer Payement and clicked on Pay Button
                        lblWriteOffAmount.Visible = True
                        txtWriteOffAmount.Visible = True

                        ManageControlsForInsurerPayments() 'this will manage the controls for "Insurer Payments"

                        If Session(CNCashListItem) IsNot Nothing AndAlso TryCast(Session(CNCashListItem), NexusProvider.PaymentCashListItemType) IsNot Nothing Then
                            sSetFocusOnControl = GISLookup_PaymentType.ClientID
                        Else
                            sSetFocusOnControl = GISLookup_ReceiptType.ClientID
                        End If

                        'This should be called only in case of edit records from Insure Payments
                        If Not String.IsNullOrEmpty(CashListItemID.Value()) Then
                            PopulateUpdateObject()
                        End If
                    ElseIf Session("ModeValue") = "CR" Then 'Cash/Cheque Receipts/Payments
                        ManageCashChequeListReceiptControls()
                        If GISLookup_MediaType.SelectedValue = String.Empty Then
                            FillMediaType()
                        End If

                        'This code will retreive the address if user directly type the account code
                        Me.txtAccount.Attributes.Add("onblur", "javascript:return getAccount('" & UpdateAddress.ClientID.ToString & "');")
                        If CashListItemID IsNot Nothing Then
                            If Not String.IsNullOrEmpty(CashListItemID.Value()) Then
                                If Session("ModeType") = "Payment" Then 'Payments
                                    oCashListItem = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType).PaymentItems.Item(CType(CashListItemID.Value(), Integer))
                                    sSetFocusOnControl = GISLookup_PaymentType.ClientID
                                ElseIf Session("ModeType") = "Receipt" Then 'Receipts
                                    oCashListItem = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType).ReceiptItems.Item(CType(CashListItemID.Value(), Integer))
                                    sSetFocusOnControl = GISLookup_ReceiptType.ClientID
                                    'Page.ClientScript.RegisterStartupScript(GetType(String), "HidePaymentTab", "HidePaymentTab();", True)
                                    ScriptManager.RegisterStartupScript(Me, Page.GetType(), "HidePaymentTab", "HidePaymentTab();", True)
                                End If

                                'issue - on postback does not allow to change value
                                'PopulateUpdateObject()

                                If Request("__EVENTARGUMENT") = "EditClick" Then
                                    PopulateUpdateObject()
                                End If
                            End If
                        End If

                        If Session("ModeType") = "Payment" Then 'Payments
                            PnlPayeeInformation.Visible = False
                            sSetFocusOnControl = GISLookup_PaymentType.ClientID
                        Else
                            'Cash/Cheque Receipts
                            PnlPayeeInformation.Visible = False
                            sSetFocusOnControl = GISLookup_ReceiptType.ClientID
                            'Page.ClientScript.RegisterStartupScript(GetType(String), "HidePaymentTab", "HidePaymentTab();", True)
                            ScriptManager.RegisterStartupScript(Me, Page.GetType(), "HidePaymentTab", "HidePaymentTab();", True)
                        End If
                    ElseIf Session("ModeValue") = "PayNow" AndAlso (Session("ModeType") = "Receipt" OrElse Session("ModeType") = "Payment") Then
                        FillMediaType()
                        DisplayAccountInformation_NewBusiness() 'set the Account Information and make enable\disable

                        DisplayAddressInformation() 'set the addrress for the selected agent or party

                        ManageControlsForNewBusiness() 'this will manage the controls need to displayed and hide

                        If (Session("ModeType") = "Payment") Then
                            liReceiptType.Visible = False
                            liPaymentType.Visible = True
                            lblReceiptTypeHeading.Text = GetLocalResourceObject("lbl_PaymentType")
                        End If
                        Dim oDictMediaReferenceMandatory As Dictionary(Of String, Integer) = Nothing
                        If ViewState("MediaRefMandatoryCacheID") IsNot Nothing Then
                            oDictMediaReferenceMandatory = CType(Cache.Item(ViewState("MediaRefMandatoryCacheID")), Dictionary(Of String, Integer))
                        End If
                        If oDictMediaReferenceMandatory IsNot Nothing AndAlso oDictMediaReferenceMandatory.ContainsKey(GISLookup_MediaType.SelectedValue.Trim + "_IS_RECEIPT_PRINTED_AUTOMATICALLY") Then
                            If (Session("ModeType") IsNot Nothing AndAlso (Session("ModeType").ToString().ToUpper().Trim = "RECEIPT" OrElse Session("ModeType").ToString().ToUpper().Trim = "PAYMENT") OrElse Session("ModeValue") = "IP") OrElse
                                (Session("Type") IsNot Nothing AndAlso Session("Type").Trim() = PaymentType.R.ToString()) Then
                                chkProduceDocument.Checked = IIf(CInt(oDictMediaReferenceMandatory.Item(GISLookup_MediaType.SelectedValue.Trim + "_IS_RECEIPT_PRINTED_AUTOMATICALLY")) = 1, True, False)
                                upUpdateProdoc.Update()
                            End If
                        Else
                            chkProduceDocument.Checked = False
                            upUpdateProdoc.Update()
                        End If
                        sSetFocusOnControl = Cash_List_Item__Transaction_Date.ClientID

                    ElseIf Session("ModeValue") = "Receipt" Then 'Cash/Cheque Receipts/Payments
                        ManageCashChequeListReceiptControls()

                        FillMediaType()

                        'This code will retreive the address if user directly type the account code
                        Me.txtAccount.Attributes.Add("onblur", "javascript:return getAccount('" & UpdateAddress.ClientID.ToString & "');")

                        If Not String.IsNullOrEmpty(CashListItemID.Value()) Then
                            If Session("ModeType") = "Payment" Then 'Payments
                                oCashListItem = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType).PaymentItems.Item(CType(CashListItemID.Value(), Integer))
                                sSetFocusOnControl = GISLookup_PaymentType.ClientID
                            ElseIf Session("ModeType") = "Receipt" Then 'Receipts
                                oCashListItem = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType).ReceiptItems.Item(CType(CashListItemID.Value(), Integer))
                                sSetFocusOnControl = GISLookup_ReceiptType.ClientID
                                'Page.ClientScript.RegisterStartupScript(GetType(String), "HidePaymentTab", "HidePaymentTab();", True)
                                ScriptManager.RegisterStartupScript(Me, Page.GetType(), "HidePaymentTab", "HidePaymentTab();", True)
                            End If
                            PopulateUpdateObject()
                        End If
                        If Session("ModeType") = "Payment" Then 'Payments
                            PnlPayeeInformation.Visible = False
                            sSetFocusOnControl = GISLookup_PaymentType.ClientID
                        Else
                            'Cash/Cheque Receipts
                            PnlPayeeInformation.Visible = False
                            sSetFocusOnControl = GISLookup_ReceiptType.ClientID
                            'Page.ClientScript.RegisterStartupScript(GetType(String), "HidePaymentTab", "HidePaymentTab();", True)
                            ScriptManager.RegisterStartupScript(Me, Page.GetType(), "HidePaymentTab", "HidePaymentTab();", True)
                        End If
                    ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then 'MTA Cancellation Payment

                        ManageControlsForMTACancellation()
                        DisplayAccountInformation_MTC()

                        PnlPayeeInformation.Visible = False
                        sSetFocusOnControl = GISLookup_PaymentType.ClientID
                    ElseIf Session("ModeValue") = "INS" Then
                        'User is doing the Insurer Payement and clicked on Pay Button
                        lblWriteOffAmount.Visible = True
                        txtWriteOffAmount.Visible = True

                        Session("ModeType") = "Receipt"
                        DisplayControls()
                        FillMediaType()
                        ManageCashChequeListReceiptControls() 'this will manage the controls for "Instalments"

                        sSetFocusOnControl = GISLookup_ReceiptType.ClientID
                        btnAccount.Enabled = False
                        txtAccount.Enabled = False
                    ElseIf Session("ModeValue") = "INSDEPOSIT" Then
                        Dim iMediaTypeId As Integer
                        Dim sMediaTypeCode As String = String.Empty
                        Dim iBankAccountId As Integer
                        Dim sBankAccountCode As String
                        GetBankAccountDefault(iMediaTypeId, iBankAccountId, 2) ''get current client details

                        If iBankAccountId > 0 Then
                            sMediaTypeCode = GetCodeForKey(NexusProvider.ListType.PMLookup, iMediaTypeId, "MediaType", True)
                            sBankAccountCode = GetCodeForKey(NexusProvider.ListType.PMLookup, iBankAccountId, "BankAccount", True)
                        End If
                        Session("ModeType") = "Receipt"
                        DisplayControls()
                        FillMediaType()
                        ManageCashChequeListReceiptControls() 'this will manage the controls for "Instalments"

                        sSetFocusOnControl = GISLookup_ReceiptType.ClientID
                        btnAccount.Enabled = False
                        txtAccount.Enabled = False
                        Cash_List_Item__Transaction_Date.Text = Date.Today
                        If Not String.IsNullOrEmpty(sMediaTypeCode) Then
                            GISLookup_MediaType.SelectedValue = sMediaTypeCode
                        End If

                        ''set default value to Instalment Deposit if present in the dropdown
                        If GISLookup_ReceiptType.Items.FindItemByCode("INSTDEPT") IsNot Nothing Then
                            GISLookup_ReceiptType.Value = "INSTDEPT"
                            GISLookup_ReceiptType.Enabled = False
                        End If
                    Else
                        Dim sOption As String
                        Dim oClaimOpen As NexusProvider.ClaimOpen = Session(CNClaim)
                        If oClaimOpen IsNot Nothing Then
                            oWebservice = New NexusProvider.ProviderManager().Provider
                            sOption = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsGrossClaimPaymentAmount, NexusProvider.RiskTypeOptions.None, Session(CNProductCode), Nothing)
                            If String.IsNullOrEmpty(sOption) Then
                                hIsGrossClaimPaymentAmount.Value = "0"
                            Else
                                hIsGrossClaimPaymentAmount.Value = sOption
                            End If
                            sOption = String.Empty
                            sOption = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, Nothing, NexusProvider.RiskTypeOptions.ClaimsIsPostTaxes, Nothing, oClaimOpen.RiskType)
                            If String.IsNullOrEmpty(sOption) Then
                                hClaimsIsPostTaxes.Value = "0"
                            Else
                                hClaimsIsPostTaxes.Value = sOption
                            End If
                        End If
                        DisplayControls() 'like Claim Payment Processing, NB/MTA/Renewals, Pay Claim
                        sSetFocusOnControl = Cash_List_Item__Transaction_Date.ClientID
                    End If

                    'set the focus on the appropriate control depends on the condition applied
                    'To set the Focus
                    Page.SetFocus(sSetFocusOnControl)
                End If
                ScriptManager.RegisterStartupScript(Me, Page.GetType(), "ShowHideInstalTab", "ShowHideInstalTab();", True)
                Session("CashListItemFirstLoad") = False
                Session("AddMoreCashList") = "No"
            End If
            'Updation of the Address based on the Account Key
            If Request("__EVENTARGUMENT") = "RefreshIP" Then
                'Reset the Address control
                txtName.Text = String.Empty
                PayNow_Address.Address1 = String.Empty
                PayNow_Address.Address2 = String.Empty
                PayNow_Address.Address3 = String.Empty
                PayNow_Address.Address4 = String.Empty
                PayNow_Address.CountryCode = String.Empty
                PayNow_Address.Postcode = String.Empty

                If Session("ModeType") = "Payment" Or Session("ModeType") = "Receipt" Then
                    If String.IsNullOrEmpty(hiddenTempText.Value) = False AndAlso hiddenTempText.Value <> "0" _
                                AndAlso String.IsNullOrEmpty(hPartyKey.Value) = True Or hPartyKey.Value.Trim = "0" Then
                        'This code will retreive the address if user directly type the account code
                        If hPartyKey.Value <> "" Then
                            GetPartyKey()
                        End If

                    End If
                End If

                If Session("ModeType") = "Receipt" Then
                    'Cash/Cheque Receipts
                    PnlPayeeInformation.Visible = False

                    'Page.ClientScript.RegisterStartupScript(GetType(String), "HidePaymentTab", "HidePaymentTab();", True)
                    ScriptManager.RegisterStartupScript(Me, Page.GetType(), "HidePaymentTab", "HidePaymentTab();", True)
                End If

                oWebservice = New NexusProvider.ProviderManager().Provider
                hPartyKey.Value = Session("PartyKey")
                If hPartyKey.Value IsNot Nothing AndAlso hPartyKey.Value.Trim.Length <> 0 AndAlso hPartyKey.Value.Trim <> 0 Then
                    'populate Address Type
                    DisplayAddressInformation()

                    'For Bank Guarantee
                    If GISLookup_ReceiptType.Value = "BGDEPT" AndAlso hiddenAccountKey.Value <> "" Then

                        Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                        Dim oBankGuaranteePolicy As NexusProvider.BankGuaranteePolicy

                        pnlBGDebtDetails.Visible = True

                        oBankGuaranteePolicy = oWebservice.GetPoliciesOnBankGuaranteeForReceipt(CInt(hiddenAccountKey.Value), NexusProvider.BGGetPoliciesActionTypeType.OutStandingPremium, CInt(hPartyKey.Value))
                        grdvBGDebtDetails.DataSource = oBankGuaranteePolicy.PartyBGPolicyDetails
                        grdvBGDebtDetails.DataBind()
                        ViewState("BG") = oBankGuaranteePolicy
                    End If
                End If
                If hdnIsInstalment.Value = "1" Then
                    If Not (String.IsNullOrEmpty(hPartyKey.Value) OrElse hPartyKey.Value.Equals("0")) Then
                        FindPremiumFinancePlans()
                        GetFinancePlanDetails()
                        liTenderedAmount.Visible = True
                    Else
                        InitialiseInstalment()
                    End If
                End If
                Dim changeTab1 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(1) a').tab('show')});"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab1", changeTab1, True)
                'Session("hfActiveTab") = 1
                Session("hfPreviousTab") = 1
            End If

            If (Request("__EVENTARGUMENT") = "Refresh") Then
                Response.Redirect("~/secure/WorkManager.aspx", False)
            End If
            'cleaning up
            oWebservice = Nothing
            'This will populate search account modal 
            If HttpContext.Current.Session.IsCookieless Then
                btnAccount.OnClientClick = "tb_show(null ,'" & System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/FindAccount.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=650' , null);return false;"
            Else
                btnAccount.OnClientClick = "tb_show(null ,'" & System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "/Modal/FindAccount.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=650' , null);return false;"
            End If
            If Request("__EVENTARGUMENT") = "Refresh" Then
                Response.Redirect("~/secure/WorkManager.aspx")
            End If

            If Request("__EVENTARGUMENT") = "WriteOFF" Then
                WriteOff(False)
            End If
            If Request("__EVENTARGUMENT") = "TakeExactAmount" Then
                TakeExactAmount()
            End If
            Dim oTempInstalmentPlanDetailsCollection As New NexusProvider.InstalmentPlanDetailsCollection
            Dim bJumpInToAdd As Boolean = False
            Dim bJumpInToRemove As Boolean = False
            Dim bRecordExist As Boolean = True
            Dim iCount As Integer = 0
            If Request("__EVENTARGUMENT") = "CHECKED" Then
                bJumpInToAdd = True
                bJumpInToRemove = False
            End If

            If Request("__EVENTARGUMENT") = "UNCHECKED" Then
                bJumpInToAdd = False
                bJumpInToRemove = True
            End If

            If Page.IsPostBack OrElse Request.QueryString("Mode") = "AP" Then
                'If Page.IsPostBack Then
                If Session("INSTALMENTPLANDETAILS") IsNot Nothing Then
                    oInstalmentPlanDetailsCollection = Session("INSTALMENTPLANDETAILS")
                    oTempInstalmentPlanDetailsCollection = Session("INSTALMENTPLANDETAILS")
                    For Each oInstPlnDet As NexusProvider.InstalmentPlanDetails In oTempInstalmentPlanDetailsCollection
                        dOverallSelectedAmount = dOverallSelectedAmount + oInstPlnDet.InstalmentDetails.Amount
                    Next
                    txtOverAllSelectedTotal.Text = String.Format("{0:0.00}", dOverallSelectedAmount)

                End If
                'oInstalmentPlanDetailsCollection = New NexusProvider.InstalmentPlanDetailsCollection
                If grdInstallmentQuotes.Rows.Count > 0 AndAlso bJumpInToAdd Then
                    For iCount = 0 To grdInstallmentQuotes.Rows.Count - 1
                        Dim chkSelected As CheckBox
                        chkSelected = DirectCast(grdInstallmentQuotes.Rows(iCount).FindControl("chkSelectedInstalment"), CheckBox)
                        If chkSelected.Checked = True Then
                            If Session("INSTALMENTPLANDETAILS") Is Nothing OrElse DirectCast(Session("INSTALMENTPLANDETAILS"), NexusProvider.InstalmentPlanDetailsCollection).Count = 0 Then
                                Dim oInstalmentPlanDetails As NexusProvider.InstalmentPlanDetails
                                oInstalmentPlanDetails = New NexusProvider.InstalmentPlanDetails
                                oInstalmentPlanDetails.FinancePlanKey = ddlInstalmentPlan.SelectedValue
                                oInstalmentPlanDetails.FinancePlanVersion = ViewState("PlanVersion")
                                oInstalmentPlanDetails.InstalmentDetails = New NexusProvider.Instalment
                                oInstalmentPlanDetails.InstalmentDetails.Amount = grdInstallmentQuotes.Rows(iCount).Cells(4).Text.Trim()
                                oInstalmentPlanDetails.InstalmentDetails.DueDate = grdInstallmentQuotes.Rows(iCount).Cells(2).Text.Trim()
                                oInstalmentPlanDetails.InstalmentDetails.BatchRef = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("BatchRef"))
                                oInstalmentPlanDetails.InstalmentDetails.Commission = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("Commission"))
                                oInstalmentPlanDetails.InstalmentDetails.ExportDate = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("ExportDate"))
                                oInstalmentPlanDetails.InstalmentDetails.Fee = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("Fee"))
                                oInstalmentPlanDetails.InstalmentDetails.InstalmentNumber = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("InstalmentNumber"))
                                oInstalmentPlanDetails.InstalmentDetails.InstalmentReasonCode = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("InstalmentReasonCode"))
                                oInstalmentPlanDetails.InstalmentDetails.PaymentDate = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("PaymentDate"))
                                oInstalmentPlanDetails.InstalmentDetails.PFInstalmentsKey = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("PFInstalmentsKey"))
                                oInstalmentPlanDetails.InstalmentDetails.PFTransactionKey = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("PFTransactionKey"))
                                oInstalmentPlanDetails.InstalmentDetails.PostedDate = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("PostedDate"))
                                oInstalmentPlanDetails.InstalmentDetails.Reason = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("Reason"))
                                oInstalmentPlanDetails.InstalmentDetails.Status = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("Status"))
                                oInstalmentPlanDetails.InstalmentDetails.StatusCode = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("StatusCode"))
                                oInstalmentPlanDetails.InstalmentDetails.StatusDescription = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("StatusDescription"))
                                oInstalmentPlanDetails.InstalmentDetails.Tax = Convert.ToDouble(grdInstallmentQuotes.DataKeys(iCount).Values("Tax"))
                                oInstalmentPlanDetails.InstalmentDetails.TransactionDescription = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("TransactionDescription"))
                                oInstalmentPlanDetailsCollection.Add(oInstalmentPlanDetails)
                                Session("INSTALMENTPLANDETAILS") = oInstalmentPlanDetailsCollection
                                txtOverAllSelectedTotal.Text = String.Format("{0:0.00}", (dOverallSelectedAmount + oInstalmentPlanDetails.InstalmentDetails.Amount))

                                txtAmount.Text = txtOverAllSelectedTotal.Text
                            Else
                                Dim oInstalmentPlanDetails As NexusProvider.InstalmentPlanDetails
                                oInstalmentPlanDetails = New NexusProvider.InstalmentPlanDetails
                                For Each oInstPlnDet As NexusProvider.InstalmentPlanDetails In oTempInstalmentPlanDetailsCollection
                                    If oInstPlnDet.InstalmentDetails IsNot Nothing AndAlso oInstPlnDet.InstalmentDetails.PFInstalmentsKey = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("PFInstalmentsKey")) Then
                                        bRecordExist = True
                                        Exit For
                                    Else
                                        bRecordExist = False
                                    End If
                                Next
                                If Not bRecordExist Then
                                    oInstalmentPlanDetails.FinancePlanKey = ddlInstalmentPlan.SelectedValue
                                    oInstalmentPlanDetails.FinancePlanVersion = ViewState("PlanVersion")
                                    oInstalmentPlanDetails.InstalmentDetails = New NexusProvider.Instalment
                                    oInstalmentPlanDetails.InstalmentDetails.Amount = grdInstallmentQuotes.Rows(iCount).Cells(4).Text.Trim()
                                    oInstalmentPlanDetails.InstalmentDetails.DueDate = grdInstallmentQuotes.Rows(iCount).Cells(2).Text.Trim()
                                    oInstalmentPlanDetails.InstalmentDetails.BatchRef = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("BatchRef"))
                                    oInstalmentPlanDetails.InstalmentDetails.Commission = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("Commission"))
                                    oInstalmentPlanDetails.InstalmentDetails.ExportDate = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("ExportDate"))
                                    oInstalmentPlanDetails.InstalmentDetails.Fee = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("Fee"))
                                    oInstalmentPlanDetails.InstalmentDetails.InstalmentNumber = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("InstalmentNumber"))
                                    oInstalmentPlanDetails.InstalmentDetails.InstalmentReasonCode = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("InstalmentReasonCode"))
                                    oInstalmentPlanDetails.InstalmentDetails.PaymentDate = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("PaymentDate"))
                                    oInstalmentPlanDetails.InstalmentDetails.PFInstalmentsKey = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("PFInstalmentsKey"))
                                    oInstalmentPlanDetails.InstalmentDetails.PFTransactionKey = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("PFTransactionKey"))
                                    oInstalmentPlanDetails.InstalmentDetails.PostedDate = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("PostedDate"))
                                    oInstalmentPlanDetails.InstalmentDetails.Reason = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("Reason"))
                                    oInstalmentPlanDetails.InstalmentDetails.Status = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("Status"))
                                    oInstalmentPlanDetails.InstalmentDetails.StatusCode = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("StatusCode"))
                                    oInstalmentPlanDetails.InstalmentDetails.StatusDescription = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("StatusDescription"))
                                    oInstalmentPlanDetails.InstalmentDetails.Tax = Convert.ToDouble(grdInstallmentQuotes.DataKeys(iCount).Values("Tax"))
                                    oInstalmentPlanDetails.InstalmentDetails.TransactionDescription = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("TransactionDescription"))
                                    txtOverAllSelectedTotal.Text = String.Format("{0:0.00}", (dOverallSelectedAmount + oInstalmentPlanDetails.InstalmentDetails.Amount))

                                    txtAmount.Text = txtOverAllSelectedTotal.Text
                                End If
                                If oInstalmentPlanDetails.FinancePlanKey <> 0 Then
                                    oInstalmentPlanDetailsCollection.Add(oInstalmentPlanDetails)
                                    Session("INSTALMENTPLANDETAILS") = oInstalmentPlanDetailsCollection
                                End If

                            End If

                        End If
                    Next
                ElseIf bJumpInToRemove Then
                    If Session("INSTALMENTPLANDETAILS") IsNot Nothing Then
                        Dim nIndexToRemove As Integer = Nothing
                        oInstalmentPlanDetailsCollection = Session("INSTALMENTPLANDETAILS")
                        'Dim oPolicy = (dele From ps In oInstalmentPlanDetailsCollection Where ps.InstalmentDetails.PFInstalmentsKey = Convert.ToString(grdInstallmentQuotes.DataKeys(iCount).Values("PFInstalmentsKey"))  ) Order By ps.quoteversion Descending)
                        For nRecordCount As Integer = 0 To oInstalmentPlanDetailsCollection.Count - 1
                            If oInstalmentPlanDetailsCollection IsNot Nothing AndAlso oInstalmentPlanDetailsCollection(nRecordCount).InstalmentDetails.InstalmentNumber = hdInstalmentNumber.Value AndAlso oInstalmentPlanDetailsCollection(nRecordCount).FinancePlanKey = ddlInstalmentPlan.SelectedValue Then
                                dOverallSelectedAmount = dOverallSelectedAmount - oInstalmentPlanDetailsCollection(nRecordCount).InstalmentDetails.Amount
                                oInstalmentPlanDetailsCollection.Remove(nRecordCount)
                                Exit For
                            End If
                        Next
                        txtOverAllSelectedTotal.Text = String.Format("{0:0.00}", dOverallSelectedAmount)
                        txtAmount.Text = txtOverAllSelectedTotal.Text
                        Session("INSTALMENTPLANDETAILS") = oInstalmentPlanDetailsCollection
                    End If
                End If

            End If
            'End If
            '  End If
            'End If

        End Sub

        Public Sub ClearFields()
            txtAmount.Text = String.Empty
            txtMediaReference.Text = String.Empty

            GISLookup_MediaType.SelectedValue = String.Empty

            txtAllocationStatus.Text = String.Empty

            txtName.Text = String.Empty
            PayNow_Address.Address1 = String.Empty
            PayNow_Address.Address2 = String.Empty
            PayNow_Address.Address3 = String.Empty
            PayNow_Address.Address4 = String.Empty
            PayNow_Address.CountryCode = String.Empty
            PayNow_Address.Postcode = String.Empty


            txtOurReference.Text = String.Empty
            txtAccount.Text = String.Empty

            txtAccountCode.Text = String.Empty
            txtBranchCode.Text = String.Empty
            'ddlAccountType.SelectedIndex = 0

            txtExpiryDate.Text = String.Empty
            txtReference1.Text = String.Empty
            txtReference2.Text = String.Empty
            txtPayeeName.Text = String.Empty
            txtBankReference.Text = String.Empty
            txtBIC.Text = String.Empty
            txtIBAN.Text = String.Empty

            txtChequeHolderName.Text = String.Empty

            Cash_List_Item__Cheque_Date.Text = String.Empty

            Cash_List_Item__InstrumentNumber.Text = String.Empty
            txtBankBranch.Text = String.Empty
            txtBankLocation.Text = String.Empty
            GISLookup_BankList.Value = String.Empty
            GISLookup_ChequeClearingTypeList.Value = String.Empty
            GISLookup_ChequeType.Value = String.Empty

            txtCardNumber.Text = String.Empty
            txtNameOnCard.Text = String.Empty
            Cash_List_Item__Start_Date.Text = String.Empty
            Cash_List_Item__Expiry_Date.Text = String.Empty
            txtManualAuth.Text = String.Empty


            txtIssueNumber.Text = String.Empty
            txtPin.Text = String.Empty

            GISLookup_TypeofCard.Value = String.Empty
            GISLookup_IssuingBank.Value = String.Empty
            txtTransactionSlip.Text = String.Empty

            txtName.Text = String.Empty
            txtDetails.Text = String.Empty
            'ddlStatus.Value
            txtTheirReference.Text = String.Empty
            Cash_List_Item__Collection_Date.Text = Date.Now.ToShortDateString()
            Cash_List_Item__Transaction_Date.Text = Date.Now.ToShortDateString()


            GISLookup_ReceiptType.Value = "STD"
            GISLookup_PaymentType.Value = "AGPAY"
            txtTendered.Text = String.Empty
            txtAccount.Enabled = True
            txtAmount.Enabled = True
            GISLookup_MediaType.Enabled = True
            txtMediaReference.Enabled = True
            txtTheirReference.Enabled = True
            txtOurReference.Enabled = True
            txtTendered.Enabled = True
            chkProduceDocument.Enabled = True
            chkProduceDocument.checked = False
            GISLookup_ReceiptType.Enabled = True
            GISLookup_PaymentType.Enabled = True
            txtName.Enabled = True
            txtDetails.Enabled = True
            hPartyKey.Value = ""

        End Sub

        Public Sub GetAuthorizationComment()
            Dim oSamProvider As NexusProvider.SAMForInsurance.ProviderSAMForInsuranceV2 = New NexusProvider.SAMForInsurance.ProviderSAMForInsuranceV2()
            Dim oGetAuthorizationComment As NexusProvider.GetAuthorizationComment
            Dim sBranchCode As String = Session(CNBranchCode).ToString()
            Dim nCashListItem_Id As Integer
            oGetAuthorizationComment = New NexusProvider.GetAuthorizationComment
            If Request.QueryString("CashListItemKey") IsNot Nothing And Not String.IsNullOrEmpty(Request.QueryString("CashListItemKey")) Then
                nCashListItem_Id = CType(Request.QueryString("CashListItemKey").Trim, Integer)
            End If
            oGetAuthorizationComment = oSamProvider.GetAuthorizationComment(nCashListItem_Id, sBranchCode)
            txtPrevComments.Text = oGetAuthorizationComment.Authorization_Comment
            oGetAuthorizationComment = Nothing
        End Sub

        Private Function OpenModal(ByVal sAccountShortCode As String, ByVal sAmount As String, ByVal sTransactionDate As String, ByVal sCurrencyCode As String, ByVal sProcessName As String) As Boolean
            Dim oAccountSearchCr As New NexusProvider.AccountSearchCriteria
            Dim oAccountColl As NexusProvider.AccountSearchResultCollection
            Dim iAccountKey As Integer
            Dim sAccountCurrency As String

            oWebservice = New NexusProvider.ProviderManager().Provider
            oAccountSearchCr.ShortCode = sAccountShortCode
            oAccountColl = oWebservice.FindAccounts(oAccountSearchCr)
            iAccountKey = oAccountColl(0).AccountKey

            Dim oCurrencyCollection As NexusProvider.CurrencyCollection = oWebservice.GetCurrenciesByBranch(Session(CNBranchCode).ToString())

            If oCurrencyCollection IsNot Nothing AndAlso oCurrencyCollection.Count > 0 Then
                For i As Integer = 0 To oCurrencyCollection.Count - 1
                    sAccountCurrency = oCurrencyCollection.Item(i).BaseCurrencyCode.ToString()
                    Exit For
                Next
            End If

            If Session("CashListMulticurrency") Is Nothing AndAlso Request("__EVENTARGUMENT") <> "ContinueAfterMulticurrency" AndAlso
                                Not String.IsNullOrEmpty(sCurrencyCode) AndAlso sCurrencyCode.Trim() <> sAccountCurrency.Trim() Then

                Dim sModalUrl As String
                Dim sParams As String = String.Format("&TransactionAmount={0}&TransactionCurrencyCode={1}&EffectiveDateOfExchange={2}&AccountID={3}",
                sAmount, sCurrencyCode, sTransactionDate, iAccountKey)

                If HttpContext.Current.Session.IsCookieless Then
                    sModalUrl = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() & "))" & "/Modal/Multicurrency.aspx?ProcessName=" & sProcessName & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750" & sParams
                Else
                    sModalUrl = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "Modal/Multicurrency.aspx?ProcessName=" & sProcessName & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750" & sParams
                End If

                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "ShowMulticurrency",
                "$(document).ready(function(){tb_show(null,'" & sModalUrl & "', null);});", True)
                Session("CashListMulticurrency") = True
                Return True
            End If

            Session.Remove("CashListMulticurrency")
            Return False
        End Function

        Private Sub ApplyRates(ByVal item As NexusProvider.PaymentItems, ByVal rates As Nexus.Constants.Session.CashListCurrencyRates)
            item.CurrencyBaseDate = rates.CurrencyBaseDate
            item.CurrencyBaseXrate = rates.CurrencyBaseXrate
            item.AccountBaseDate = rates.AccountBaseDate
            item.AccountBaseXrate = rates.AccountBaseXrate
            item.SystemBaseDate = rates.SystemBaseDate
            item.SystemBaseXrate = rates.SystemBaseXrate
            item.OverrideReason = rates.OverrideReason
        End Sub

    End Class
End Namespace
