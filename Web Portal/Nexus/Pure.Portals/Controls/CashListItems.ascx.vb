Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Configuration.ConfigurationManager
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace Nexus
    Partial Class Controls_CashListItemstab
        Inherits System.Web.UI.UserControl

        Dim oWebService As NexusProvider.ProviderBase
        Dim dSum As Double = 0
        Dim sCashListItemKey As String = "CashListItemKey"
        Dim sTransDetailKey As String = "TransDetailKey"
        Dim sCashListKey As String = "CashListKey"
        Dim CashListItem As String = "CashListItem"
        Dim sCashListItem As String = "sCashListItem"
        Dim sSelectedAllocationIndex As String = "Selected_Allocation_Index"
        Dim hdnTabName As HiddenField

        Protected Sub BindCashListItem()

            If Session("ModeType") = "Payment" Then 'Cash/Cheque Payments
                If Session(CNCashListItem) IsNot Nothing Then
                    Dim CashListItem As NexusProvider.PaymentItemsCollection = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType).PaymentItems
                    Dim AllocationStatus As String = Convert.ToString(Session(CNCashListItemAllocationStatus))
                    Dim isPosted As Boolean = False
                    dSum = 0
                    If CashListItem IsNot Nothing And CashListItem.Count > 0 Then
                        For i As Integer = 0 To CashListItem.Count - 1
                            If CashListItem.Item(i).AllocationStatusCode <> "Posted" AndAlso CashListItem.Item(i).AllocationStatusCode <> "Allocated" Then
                                If (Session("ModeType") = "Payment" And UserCanDoTask("CashChequePayment")) _
                            Or (Session("ModeValue") = "IP" And UserCanDoTask("InsurerPayment")) _
                            Or ((Session("ModeType") = "Receipt" And UserCanDoTask("CashChequeReceipt"))) Then
                                    btnPost.Enabled = True
                                End If
                            ElseIf CashListItem.Item(i).AllocationStatusCode = "Allocated" Then
                                btnOK.Attributes.Add("onclick", Nothing)
                                btnOK.Visible = True
                            End If
                            If CashListItem.Item(i).AllocationStatusCode = "Posted" Then
                                Dim oNexusConfig As Nexus.Library.Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork)
                                Dim lnkAdd As LinkButton = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnCashListItemsAdd"), LinkButton)
                                Dim lnkCancel As LinkButton = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnCashListItemsCancel"), LinkButton)
                                lnkAdd.Visible = False
                                lnkCancel.Visible = False
                                btnAllocateCashlist.Enabled = True
                                isPosted = True
                                btnOK.Enabled = True
                                btnOK.Attributes.Add("onclick", "javascript:return CRCancelMsgBeforeAllocate();")
                            ElseIf CashListItem.Item(i).AllocationStatusCode = "Allocated" AndAlso AllocationStatus = "completed" Then
                                btnAllocateCashlist.Enabled = False
                                btnOK.Attributes.Add("onclick", Nothing)
                                btnOK.Enabled = True
                            End If
                        Next
                    Else
                        btnPost.Enabled = False
                        btnAllocateCashlist.Enabled = False
                        btnOK.Attributes.Add("onclick", Nothing)
                    End If
                    If (isPosted) Then
                        btnAllocateCashlist.Enabled = True
                        btnOK.Attributes.Add("onclick", "javascript:return CRCancelMsgBeforeAllocate();")
                    End If
                    drgCashListItems.DataSource = CashListItem
                    drgCashListItems.DataBind()
                End If




            ElseIf Session("ModeValue") = "IP" Then 'Insurer Payments
                dSum = 0
                txtTotal.Text = CType(Session(CNTotalAmount), Decimal)

                Dim oCashListItem As NexusProvider.PaymentItemsCollection = Nothing
                If Session("Type") IsNot Nothing Then
                    If Session("Type").Trim() = PaymentType.P.ToString() Or Session("Type").Trim() = PaymentType.CP.ToString() Then
                        oCashListItem = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType).PaymentItems
                    ElseIf Session("Type").Trim() = PaymentType.R.ToString() Then
                        If Session(CNCashListItem) IsNot Nothing Then
                            oCashListItem = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType).ReceiptItems
                        End If
                    End If
                Else
                    ' Do Nothing
                End If

                If oCashListItem IsNot Nothing AndAlso oCashListItem.Count > 0 Then
                    If oCashListItem.Item(0).AllocationStatusCode <> "Posted" Then
                        btnPost.Enabled = True
                        ' btnAdd.Enabled = False
                    End If
                Else
                    btnPost.Enabled = False
                    'btnAdd.Enabled = True
                End If

                drgCashListItems.DataSource = oCashListItem
                drgCashListItems.DataBind()
                oCashListItem = Nothing

            Else 'Cash/Cheque Receipts
                dSum = 0
                If Session(CNCashListItem) IsNot Nothing Then
                    Dim CashListItem As NexusProvider.PaymentItemsCollection = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType).ReceiptItems
                    Dim AllocationStatus As String = Convert.ToString(Session(CNCashListItemAllocationStatus))
                    Dim isPosted As Boolean = False
                    If CashListItem IsNot Nothing And CashListItem.Count > 0 Then
                        For i As Integer = 0 To CashListItem.Count - 1
                            If CashListItem.Item(i).AllocationStatusCode <> "Posted" AndAlso CashListItem.Item(i).AllocationStatusCode <> "Allocated" Then
                                btnPost.Enabled = True
                                btnOK.Visible = False
                            ElseIf CashListItem.Item(i).AllocationStatusCode = "Posted" AndAlso CashListItem.Item(i).AllocationStatusCode <> "Allocated" Then
                                If AllocationStatus <> "completed" Then
                                    btnAllocateCashlist.Visible = True
                                    btnAllocateCashlist.Enabled = True

                                    Dim oNexusConfig As Nexus.Library.Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork)
                                    Dim lnkAdd As LinkButton = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnCashListItemsAdd"), LinkButton)
                                    lnkAdd.Enabled = False
                                End If
                                Session("AddMoreCashList") = "No"
                            End If
                            If CashListItem.Item(i).AllocationStatusCode = "Posted" AndAlso AllocationStatus = "completed" Then
                                btnAllocateCashlist.Enabled = True
                                isPosted = True
                            ElseIf CashListItem.Item(i).AllocationStatusCode = "Allocated" AndAlso AllocationStatus = "completed" Then
                                btnAllocateCashlist.Enabled = False
                                btnOK.Attributes.Add("onclick", Nothing)
                                btnOK.Enabled = True
                            End If
                            If CashListItem.Item(i).MediaTypeCode = "OCP" AndAlso CashListItem.Item(i).PaymentHubDetails.AuthCode Is Nothing Then
                                CashListItem.Item(i).PaymentHubDetails.ResultDescription = Nothing
                                Response.Redirect("~/secure/payment/CashListItem.aspx?Mode=CR&CashListItemID=" & i & "&error=Token")
                            End If
                        Next
                    Else
                        btnPost.Enabled = False
                        btnAllocateCashlist.Enabled = False
                        btnOK.Attributes.Add("onclick", Nothing)
                    End If
                    If (isPosted) Then
                        btnAllocateCashlist.Visible = True
                        btnAllocateCashlist.Enabled = True
                        btnAllocateCashlist.Attributes.Add("onclick", Nothing)
                        btnOK.Attributes.Add("onclick", "javascript:return CRCancelMsgBeforeAllocate();")
                    End If
                    drgCashListItems.DataSource = CashListItem
                    drgCashListItems.DataBind()
                End If

            End If
            If btnAllocateCashlist.Enabled = True Then
                btnOK.Enabled = True
            End If
            txtTotal.Text = dSum.ToString("N2")
        End Sub

        Protected Sub drgCashListItems_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles drgCashListItems.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("Key") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.PaymentItems).Key)

                Dim hypEdit As LinkButton = e.Row.FindControl("hypCashListItem")
                If Session("ModeValue") = "IP" Then
                    hypEdit.Attributes.Add("onclick", "gridEditClick('IP', " & e.Row.RowIndex & ", '" & Session("Type") & "')")
                Else
                    hypEdit.Attributes.Add("onclick", "gridEditClick('CR', " & e.Row.RowIndex & ", 0)")

                End If
                Dim hypView As LinkButton = e.Row.FindControl("hypView")
                If Session("ModeValue") = "IP" Then
                    hypView.Attributes.Add("onclick", "gridViewClick('IP', " & e.Row.RowIndex & ", '" & Session("Type") & "')")
                Else
                    hypView.Attributes.Add("onclick", "gridViewClick('CR', " & e.Row.RowIndex & ", 0)")

                End If

                Dim lblhypViewAllocation As LinkButton = e.Row.FindControl("lblhypViewAllocation")

                Dim dTotal As Double = 0
                dTotal = Convert.ToDouble(e.Row.Cells(3).Text)
                dSum += dTotal

                'Delete Confirmation
                Dim hypDelete As LinkButton = e.Row.FindControl("link_Delete")
                hypDelete.Attributes.Add("onclick", "Javascript:return DeleteConfirmation();")
                Dim chkCashLst As CheckBox = CType(e.Row.FindControl("ChkCashListItem"), CheckBox)
                If e.Row.Cells(5).Text = "Allocated" Then
                    chkCashLst.Enabled = False
                End If

                Dim lnkDocument As LinkButton = e.Row.FindControl("lnkDocument")
                If Not IsNothing(lnkDocument) Then
                    If Session("ModeType") = "Payment" OrElse Session(CNMarkedAmountSignForCashList) < 0 Then
                        lnkDocument.Text = GetLocalResourceObject("lbl_PaymentLetter")
                    ElseIf Session("ModeType") = "Receipt" OrElse Session(CNMarkedAmountSignForCashList) > 0 Then
                        lnkDocument.Text = GetLocalResourceObject("lbl_ReceiptLetter")
                    End If

                    If (e.Row.Cells(5).Text.ToUpper() = "POSTED" Or e.Row.Cells(5).Text.ToUpper() = "ALLOCATED") AndAlso e.Row.Cells(6).Text = "Y" Then
                        lnkDocument.Visible = True
                        lnkDocument.CommandArgument = e.Row.RowIndex
                    Else
                        lnkDocument.Visible = False
                        lnkDocument.CommandArgument = e.Row.RowIndex
                    End If
                End If

                Dim lnkEmail As LinkButton = e.Row.FindControl("lnkEmail")
                If Not IsNothing(lnkEmail) Then
                    If Session("ModeType") = "Payment" OrElse Session(CNMarkedAmountSignForCashList) < 0 Then
                        lnkEmail.Text = GetLocalResourceObject("lbl_EmailPayment")
                    ElseIf Session("ModeType") = "Receipt" OrElse Session(CNMarkedAmountSignForCashList) > 0 Then
                        lnkEmail.Text = GetLocalResourceObject("lbl_EmailReceipt")
                    End If

                    If (e.Row.Cells(5).Text.ToUpper() = "POSTED" Or e.Row.Cells(5).Text.ToUpper() = "ALLOCATED") AndAlso e.Row.Cells(6).Text = "Y" Then
                        lnkEmail.Visible = True
                        lnkEmail.CommandArgument = e.Row.RowIndex
                    Else
                        lnkEmail.Visible = False
                        lnkEmail.CommandArgument = e.Row.RowIndex
                    End If
                End If

                If (e.Row.Cells(5).Text.ToUpper() = "POSTED" Or e.Row.Cells(5).Text.ToUpper() = "ALLOCATED" Or e.Row.Cells(5).Text.ToUpper() = "PENDING") Then

                    Dim hypCashListItem As LinkButton = e.Row.FindControl("hypCashListItem")
                    Dim link_Delete As LinkButton = e.Row.FindControl("link_Delete")
                    'hide edit delete btns
                    hypCashListItem.Visible = False
                    link_Delete.Visible = False
                    hfViewOption.Value = "ViewReceiptAllocation"

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "PostClick", "PostClick()", True)
                    If Session("ModeType") = "Payment" Then
                        hypView.Text = GetLocalResourceObject("lbl_hypViewPayment")
                    ElseIf Session("ModeType") = "Receipt" Then
                        hypView.Text = GetLocalResourceObject("lbl_hypView")
                    End If
                Else
                    lblhypViewAllocation.Visible = False
                    hypView.Visible = False

                End If

                If Session(CNCashListItemWithTransDetailKey) IsNot Nothing Then
                    'check if row is allocated 
                    If e.Row.Cells(5).Text.ToUpper() = "ALLOCATED" Then
                        hfViewOption.Value = "ViewReceiptAllocation"
                        'if yes add client click function.
                        Dim iTransdetailKey As Integer
                        If Session("ModeType") = "Payment" Then
                            Dim oCashListItem As NexusProvider.PaymentCashListItemType = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                            Dim oPaymentCashList As NexusProvider.PaymentCashListItemType = CType(Session(CNCashListItemWithTransDetailKey), NexusProvider.PaymentCashListItemType)
                            Dim TransDetailItem As NexusProvider.PaymentCashList = DirectCast(oPaymentCashList.PaymentCashList(e.Row.RowIndex), NexusProvider.PaymentCashList)

                            iTransdetailKey = TransDetailItem.TransDetailKey

                        ElseIf Session("ModeType") = "Receipt" Then
                            Dim oCashListItem As NexusProvider.PaymentItemsCollection = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType).ReceiptItems ''cash receipt Items
                            Dim oReceiptCashListCollection As NexusProvider.ReceiptCashListCollection = CType(Session(CNCashListItemWithTransDetailKey), NexusProvider.ReceiptCashListCollection) ''receipt Items with tran detail Key

                            iTransdetailKey = oReceiptCashListCollection(e.Row.RowIndex).TransDetailKey

                            If oCashListItem(e.Row.RowIndex).TypeCode = "INST" AndAlso oCashListItem(e.Row.RowIndex).AllocationStatusCode = "Allocated" Then
                                lblhypViewAllocation.Visible = False
                            End If
                        ElseIf Session("ModeType") = "IP" Then
                            lblhypViewAllocation.Visible = False
                        End If
                        Dim arrlistTransidForCashList As New ArrayList


                        Dim oDictTransdetailKey As New Dictionary(Of Object, Object)
                        If Session("TransdetailKeyDisc") IsNot Nothing Then
                            oDictTransdetailKey = DirectCast(Session("TransdetailKeyDisc"), Dictionary(Of Object, Object))
                        End If

                        Dim iCount As Integer = 0
                        For Each Key As ArrayList In oDictTransdetailKey.Keys

                            Dim Val As Object = oDictTransdetailKey(Key)

                            Dim KeyTotalCount As Integer = Convert.ToInt32(DirectCast(Key, ArrayList).Count)

                            If KeyTotalCount > 0 Then
                                For i As Integer = 0 To KeyTotalCount - 1
                                    Dim DisTransKey As Integer = Convert.ToInt32(DirectCast(Key, ArrayList)(i))
                                    If DisTransKey = iTransdetailKey Then
                                        lblhypViewAllocation.Attributes.Add("onclick", "gridViewAllocationClick(" & iCount & ")")
                                        Exit For
                                    End If
                                Next
                            End If
                            iCount = iCount + 1
                        Next
                    Else
                        lblhypViewAllocation.Visible = False
                    End If
                End If


                If Session("TypeTrans") IsNot Nothing AndAlso Session("TypeTrans") = "INST" Then
                    hypEdit.Visible = False
                End If
            End If
        End Sub

        Protected Sub drgCashListItems_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles drgCashListItems.RowDeleting

            If Session("ModeValue") = "IP" Then 'Insurer Payments
                If Session("Type").Trim() = PaymentType.P.ToString() Or Session("Type").Trim() = PaymentType.CP.ToString() Then
                    CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType).PaymentItems.Remove(e.RowIndex)
                ElseIf Session("Type").Trim() = PaymentType.R.ToString() Then
                    CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType).ReceiptItems.Remove(e.RowIndex)
                Else
                    ' Do Nothing
                End If
                ' btnAdd.Enabled = True
            ElseIf Session("ModeType") = "Payment" Then 'Cash/Cheque Payments
                CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType).PaymentItems.Remove(e.RowIndex)
            ElseIf Session("ModeType") = "Receipt" Then 'Cash/Cheque Receipts
                CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType).ReceiptItems.Remove(e.RowIndex)
            End If

            BindCashListItem()
        End Sub

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "Confirmation", "<script language=""JavaScript"" type=""text/javascript"">Function Message(){alert('" & GetLocalResourceObject("msg_Successful").ToString() & "')}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "DeleteConfirmation", "<script language=""JavaScript"" type=""text/javascript"">function DeleteConfirmation(){return confirm('" & GetLocalResourceObject("msg_DeleteConfirmation").ToString() & "')}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ApprovalAlert", "<script language=""JavaScript"" type=""text/javascript"">function ApprovalAlert(){alert('" & GetLocalResourceObject("msg_ApprovalAlert").ToString() & "')}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "IPAllocateAlert", "<script language=""JavaScript"" type=""text/javascript"">function IPAllocateAlert(){alert('" & GetLocalResourceObject("msg_IPAllocateAlert").ToString() & "')}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "IPCancelMsg", "<script language=""JavaScript"" type=""text/javascript"">function IPCancelMsg(){return confirm('" & GetLocalResourceObject("msg_IPCancelMsg").ToString() & "')}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "CRCancelMsgBeforePost", "<script language=""JavaScript"" type=""text/javascript"">function CRCancelMsgBeforePost(){return confirm('" & GetLocalResourceObject("msg_CRCancelMsgBeforePost").ToString() & "')}</script>")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "CRCancelMsgBeforeAllocate", "<script language=""JavaScript"" type=""text/javascript"">function CRCancelMsgBeforeAllocate(){return confirm('" & GetLocalResourceObject("msg_CRCancelMsgBeforeAllocate").ToString() & "')}</script>")
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oMultiStepApproval As NexusProvider.OptionTypeSetting = Nothing
            oMultiStepApproval = oWebservice.GetOptionSetting(NexusProvider.OptionType.ProductOption, 65)
            If Session("ModeType") = "Payment" Then 'Cash/Cheque Payments


                Dim bIsIncludePaymentTypeClaimPayment As Boolean

                bIsIncludePaymentTypeClaimPayment = IsClaimIncludedInPayment()
                If oMultiStepApproval.OptionValue = "1" And bIsIncludePaymentTypeClaimPayment Then
                    '  btnPost.Attributes.Add("onclick", "javascript:ApprovalAlert();")
                    btnAllocateCashlist.Attributes.Add("onclick", "javascript:ApprovalAlert(); return false;")
                End If
            ElseIf Session("ModeType") = "Receipt" Then
                btnAllocateCashlist.Attributes.Add("onclick", Nothing)
                If Session("TypeTrans") <> "INST" Then
                    btnPost.Attributes.Add("onclick", Nothing)
                    'btnCancel.Attributes.Add("onclick", "javascript:return IPCancelMsg();")
                    btnOK.Attributes.Add("onclick", "javascript:return IPCancelMsg();")
                End If
            ElseIf Session("ModeValue") = "IP" Then
                If Session("Type") = "P" And oMultiStepApproval.OptionValue = "1" Then
                    'btnPost.Attributes.Add("onclick", "javascript:return ApprovalAlert();")
                    btnAllocateCashlist.Attributes.Add("onclick", "javascript:ApprovalAlert(); return false;")
                End If
                btnOK.Visible = True
                '   btnCancel.Attributes.Add("onclick", "javascript:return IPCancelMsg();")
            End If
            'End If
            'End If

        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim oNexusFrameWork As Nexus.Library.Config.NexusFrameWork = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork)
            hdnTabName = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("hdnTabName"), HiddenField)

            If (Request("__EVENTARGUMENT") IsNot Nothing And (Request("__EVENTARGUMENT") = "CRAddCashListItem" And hdnTabName.Value = "tab-CashListItems") Or (Request("__EVENTARGUMENT") = "RefreshIP" And hdnTabName.Value = "tab-CashListItems") Or (Session("AddMoreCashList") = "Yes")) Then

                Dim aa As Boolean = CBool(Session("CashListItemsFirstLoad"))

                If Not IsPostBack Then
                    aa = True
                End If
                If aa Then
                    'first time laod

                    'To set the Focus
                    'Page.SetFocus(btnAdd)
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim sReturnCode As NexusProvider.OptionTypeSetting
                    'Get the system Option "Auto Allocate If Able"
                    Try
                        sReturnCode = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5059)
                        If sReturnCode IsNot Nothing AndAlso sReturnCode.OptionValue IsNot Nothing Then
                            If sReturnCode.OptionValue = "1" Then
                                chkAutoAllocate.Enabled = True
                                chkAutoAllocate.Checked = True
                            Else
                                chkAutoAllocate.Checked = False
                            End If
                        End If
                    Finally
                        oWebService = Nothing
                    End Try
                    BindCashListItem()
                    If Session(CNCashListItem) Is Nothing Then
                        If Session("ModeValue") = "IP" Then 'Insurer Payments
                            If Session("Type") IsNot Nothing Then
                                If Session("Type").Trim() = PaymentType.P.ToString() Or Session("Type").Trim() = PaymentType.CP.ToString() Then
                                    Session.Add(CNCashListItem, New NexusProvider.PaymentCashListItemType)
                                ElseIf Session("Type").Trim() = PaymentType.R.ToString() Then
                                    Session.Add(CNCashListItem, New NexusProvider.ReceiptCashListItemType)
                                End If
                            Else
                                ' Do Nothing
                            End If

                            btnOK.Visible = True
                        Else

                            If Session("ModeType") = "Payment" Then 'Cash/Cheque Payments
                                Session.Add(CNCashListItem, New NexusProvider.PaymentCashListItemType)
                            Else 'Cash/Cheque Receipts
                                Session.Add(CNCashListItem, New NexusProvider.ReceiptCashListItemType)
                            End If

                        End If

                    End If

                    If Session("ModeValue") = "IP" Then
                        'btnAdd.PostBackUrl = "~/secure/payment/CashListItem.aspx?Mode=IP&Type=" + Session("Type") + "&PartyKey=" + Session("PartyKey")
                        'Session("ModeValue") = "IP"
                    Else
                        'btnAdd.PostBackUrl = "~/secure/payment/CashListItem.aspx?Mode=CR&SetFlag=1&PartyKey=" + Session("PartyKey")
                        'Session("ModeValue") = "CR"
                        'Session("SetFlag") = 1
                    End If
                End If
                If Session("EVENTARGUMENT") = "Refresh" Then 'Refresh The Value in case of Cash/Cheque Receipts and Payments
                    BindCashListItem()
                    If Session("TypeTrans") <> "INST" Then
                        ' btnCancel.Attributes.Add("onclick", "javascript:return CRCancelMsgBeforePost();")

                        'btnOK.Attributes.Add("onclick", "javascript:return CRCancelMsgBeforePost();")
                    End If
                ElseIf Session("EVENTARGUMENT") = "IPRefresh" Then 'Refresh The Value in case of Insurer Payments
                    BindCashListItem()
                    If drgCashListItems.Rows.Count > 0 And Session("ModeValue") = "IP" Then
                        ' btnAdd.Enabled = False
                    End If
                ElseIf Request.QueryString("frompage") = "Allocate" Then
                    'BindCashListItem()
                    If Session(CNCashListItemAllocationStatus) IsNot Nothing Then ''if coming from after successfull receipt/payment allocation
                        If Session("ModeType") = "Payment" Then
                            Dim oCashListItem As NexusProvider.PaymentCashListItemType = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                            Dim AllocationStatus As String = Convert.ToString(Session(CNCashListItemAllocationStatus))
                            Dim oPaymentCashList As NexusProvider.PaymentCashListItemType = CType(Session(CNCashListItemWithTransDetailKey), NexusProvider.PaymentCashListItemType)

                            If AllocationStatus = "completed" Then
                                Dim arrList As ArrayList = DirectCast(Session(CNTransdetailKeyfromCashList), ArrayList) ''allocated tran detail key
                                For i As Integer = 0 To arrList.Count - 1
                                    Dim TransdetailKey As Int32 = Convert.ToInt32(arrList(i))

                                    For index As Integer = 0 To oPaymentCashList.PaymentCashList.Count - 1
                                        If oPaymentCashList.PaymentCashList(index).TransDetailKey = TransdetailKey Then 'compare TransDetailKey
                                            'For receItmIndex = 0 To oCashListItem.PaymentCashList.Count - 1
                                            If oCashListItem.PaymentCashList(index).AccountShortCode = oPaymentCashList.PaymentCashList(index).AccountShortCode Then 'compare account short code
                                                oCashListItem.PaymentCashList(index).AllocationStatusCode = "Allocated"
                                                oPaymentCashList.PaymentCashList(index).AllocationStatusCode = "Allocated"
                                                oCashListItem.PaymentItems(index).AllocationStatusCode = "Allocated"
                                                Exit For
                                            End If
                                            'Next
                                        End If
                                    Next

                                Next
                            End If

                            If oCashListItem.PaymentCashList.Count <= 0 Then ''if all payment allocated successfully redirect to default payment page
                                Session.Remove(CNCashListItemAllocationStatus)
                                Session.Remove(CNTransdetailKeyfromCashList)
                                'Response.Redirect("~/secure/payment/CashList.aspx?Mode=Payment", False)
                                Session("ModeValue") = "Payment"
                                'CashList
                                Dim changeTab0 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(0) a').tab('show')});"
                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab0", changeTab0, True)

                                Session("hfPreviousTab") = 0
                            Else
                                Session.Remove(CNCashListItemAllocationStatus)
                                Session.Remove(CNTransdetailKeyfromCashList)
                                CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType).PaymentCashList = oCashListItem.PaymentCashList ''refresh cash list item session after removing allocated payment
                                CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType).PaymentItems = oCashListItem.PaymentItems
                                Session(CNCashListItemWithTransDetailKey) = oPaymentCashList ''refresh cash list item with tran key session after removing allocated payment
                                BindCashListItem()
                            End If
                        ElseIf Session("ModeType") = "Receipt" Then
                            Dim oCashListItem As NexusProvider.PaymentItemsCollection = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType).ReceiptItems ''cash receipt Items
                            Dim AllocationStatus As String = Convert.ToString(Session(CNCashListItemAllocationStatus))
                            Dim oReceiptCashListCollection As NexusProvider.ReceiptCashListCollection = CType(Session(CNCashListItemWithTransDetailKey), NexusProvider.ReceiptCashListCollection) ''receipt Items with tran detail Key

                            If AllocationStatus = "completed" Then
                                Dim arrList As ArrayList = DirectCast(Session(CNTransdetailKeyfromCashList), ArrayList) ''allocated tran detail key
                                For i As Integer = 0 To arrList.Count - 1
                                    Dim TransdetailKey As Int32 = Convert.ToInt32(arrList(i))
                                    For index As Integer = 0 To oReceiptCashListCollection.Count - 1
                                        If oReceiptCashListCollection(index).TransDetailKey = TransdetailKey Then 'compare TransDetailKey
                                            'For receItmIndex = 0 To oCashListItem.Count - 1
                                            If oCashListItem.Item(index).AccountShortCode = oReceiptCashListCollection(index).AccountShortCode Then 'compare account short code
                                                oCashListItem.Item(index).AllocationStatusCode = "Allocated"
                                                oReceiptCashListCollection.Item(index).AllocationStatusCode = "Allocated"
                                                Exit For
                                            End If
                                            'Next
                                        End If
                                    Next

                                Next
                            End If

                            If oCashListItem.Count <= 0 Then ''if all payment allocated successfully redirect to default receipt page
                                Session.Remove(CNCashListItemAllocationStatus)
                                Session.Remove(CNTransdetailKeyfromCashList)
                                'Response.Redirect("~/secure/payment/CashList.aspx?Mode=Receipt", False)
                                Session("ModeValue") = "Receipt"
                                'CashList
                                Dim changeTab0 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(0) a').tab('show')});"
                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab0", changeTab0, True)

                                Session("hfPreviousTab") = 0
                            Else
                                Session.Remove(CNCashListItemAllocationStatus)
                                Session.Remove(CNTransdetailKeyfromCashList)
                                CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType).ReceiptItems = oCashListItem ''refresh cash list item session after removing allocated receipt
                                Session(CNCashListItemWithTransDetailKey) = oReceiptCashListCollection ''refresh cash list item with tran key session after removing allocated receipt
                                BindCashListItem()
                            End If
                        End If
                    End If
                    ' btnAdd.Enabled = False
                    'btnOK.Visible = True
                End If
            End If
            'End If
            'End If
        End Sub

        Protected Sub btnPost_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPost.Click

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oNexusConfig As Nexus.Library.Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork)
            Dim lnkAdd As LinkButton = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnCashListItemsAdd"), LinkButton)
            Dim lnkCancel As LinkButton = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnCashListItemsCancel"), LinkButton)
            Dim oMultiStepApproval As NexusProvider.OptionTypeSetting = Nothing
            Dim alTransDetailCollection As ArrayList = CType(Session(CNMarkedTransDetailList), ArrayList)
            oMultiStepApproval = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 65)
            If Session("ModeType") = "Payment" And UserCanDoTask("CashChequePayment") Then 'Cash Cheque Payments
                If oMultiStepApproval.OptionValue = "1" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Auto Allocation", "PostWarning('" & GetLocalResourceObject("msg_ApprovalAlert").ToString() & "')", True)
                End If
                If chkAutoAllocate.Enabled AndAlso chkAutoAllocate.Checked Then
                    btnAllocateCashlist.Visible = False
                    lnkCancel.Visible = False
                    lnkAdd.Visible = False
                    btnOK.Visible = True
                    btnOK.Attributes.Add("onclick", Nothing)

                Else
                    btnAllocateCashlist.Visible = True
                    lnkCancel.Visible = False
                    lnkAdd.Visible = False
                    btnOK.Visible = True
                End If
                btnOK.Attributes.Add("onclick", Nothing)
                If Session("TypeTrans") <> "INST" Then
                    'btnCancel.Attributes.Add("onclick", "javascript:return CRCancelMsgBeforeAllocate();")
                    btnOK.Attributes.Add("onclick", "javascript:return CRCancelMsgBeforeAllocate();")
                End If
                Dim CashListItem As NexusProvider.PaymentCashListItemType = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                CashListItem.AutoAllocateIfAble = chkAutoAllocate.Checked
                If CashListItem.PaymentItems(0).StatusCode <> Nothing AndAlso
                   CashListItem.PaymentItems(0).StatusCode.Trim = "PENDING" Then
                    btnAllocateCashlist.Visible = False
                End If
                Try
                    ' Dim oMultiStepApproval As NexusProvider.OptionTypeSetting = Nothing
                    oMultiStepApproval = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 65)
                    If oMultiStepApproval.OptionValue = "1" Then
                        CashListItem.PaymentItems(0).SkipPosting = True
                    End If

                    If Session(CNTransBranchCode) Is Nothing Then
                        oWebService.CreatePaymentCashListWithItems(CashListItem,, alTransDetailCollection)
                    Else
                        oWebService.CreatePaymentCashListWithItems(CashListItem, Session(CNTransBranchCode).ToString(), alTransDetailCollection)
                    End If

                    ' btnAdd.Enabled = False
                    Session(CNCashListItemWithTransDetailKey) = CashListItem

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
                    Else
                        Throw
                    End If

                Finally
                    oWebService = Nothing
                    CashListItem = Nothing
                End Try
                Dim CashListItems As NexusProvider.PaymentCashListItemType = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                For i As Integer = 0 To CashListItems.PaymentItems.Count - 1
                    If CashListItems.PaymentItems(i).StatusCode.Trim = "PENDING" Then
                        CashListItems.PaymentItems(i).AllocationStatusCode = "Pending"
                    Else
                        CashListItems.PaymentItems(i).AllocationStatusCode = "Posted"
                    End If
                Next
                Session(CNCashListItem) = CashListItems
                Session("AddMoreCashList") = "No"
                BindCashListItem()
                lnkAdd.Enabled = False
                For Each oPaymentItem As NexusProvider.PaymentCashListItemType In CashListItems.PaymentCashList
                    If Not oPaymentItem.AutoAllocatePaymentSuccessful AndAlso chkAutoAllocate.Enabled AndAlso chkAutoAllocate.Checked Then
                        chkAutoAllocate.Checked = False
                        btnPost.Enabled = False
                        btnAllocateCashlist.Visible = True
                        btnOK.Visible = True
                        Dim sErrorMessage As String = IIf(GetLocalResourceObject("msgAllocationDeclined") Is Nothing, "Payment transaction has not been auto-allocated.", GetLocalResourceObject("msgAllocationDeclined"))
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Auto Allocation", "alert('" + sErrorMessage + "')", True)
                        Exit Sub
                    ElseIf oPaymentItem.AutoAllocatePaymentSuccessful AndAlso chkAutoAllocate.Enabled AndAlso chkAutoAllocate.Checked Then
                        btnOK.Attributes.Add("onclick", Nothing)
                    End If
                Next
            ElseIf Session("ModeValue") = "IP" And UserCanDoTask("InsurerPayment") Then 'Insurer Payments
                btnAllocateCashlist.Visible = True
                'btnOK.Attributes.Add("onclick", "javascript:IPAllocateAlert(); return false;")
                Dim oRecieptCashListitem As NexusProvider.ReceiptCashListItemType
                Dim oCashListItem As NexusProvider.PaymentCashListItemType

                txtTotal.Text = CType(Session(CNTotalAmount), Decimal)
                drgCashListItems.Columns(9).Visible = False
                Try

                    Select Case Session("Type")
                        Case PaymentType.R.ToString()
                            oRecieptCashListitem = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                            Dim oReceiptCashListCollection As NexusProvider.ReceiptCashListCollection
                            Dim oReciptCashlist As NexusProvider.ReceiptCashList
                            Try
                                'TODO Pass Branch COde here
                                oReceiptCashListCollection = oWebService.CreateReceiptcashListWithItem(oRecieptCashListitem)
                                oReciptCashlist = oReceiptCashListCollection.Item(0)
                                ViewState.Remove(sCashListItemKey)
                                ViewState.Remove(sTransDetailKey)
                                ViewState.Remove(sCashListKey)
                                ViewState.Add(sCashListItemKey, oReciptCashlist.CashListItemKey)
                                ViewState.Add(sTransDetailKey, oReciptCashlist.TransDetailKey)
                                ViewState.Add(sCashListKey, oReciptCashlist.CashListKey)

                                oRecieptCashListitem.ReceiptItems(0).AllocationStatusCode = "Posted"
                                Session(CNCashListItem) = oRecieptCashListitem
                                BindCashListItem()
                                Session(CNCashListItemWithTransDetailKey) = oReceiptCashListCollection
                                'Catch ex As Exception
                            Finally

                                oReciptCashlist = Nothing
                                oReceiptCashListCollection = Nothing
                                oRecieptCashListitem = Nothing

                            End Try

                        Case PaymentType.P.ToString(), PaymentType.CP.ToString()

                            oCashListItem = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                            oCashListItem.AccountShortCode = Session(CNAccountName)
                            Dim oPaymentCashListCollection As NexusProvider.PaymentCashListCollection
                            Dim oPaymentCashList As NexusProvider.PaymentCashList

                            'Dim oMultiStepApproval As NexusProvider.OptionTypeSetting = Nothing
                            oMultiStepApproval = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 65)
                            If oMultiStepApproval.OptionValue = "1" Then
                                oCashListItem.PaymentItems(0).SkipPosting = True
                                oCashListItem.PaymentItems(0).StatusCode = "PENDING"
                                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "ApprovalAlert", "alert('" & GetLocalResourceObject("msg_ApprovalAlert").ToString() & "');", True)
                            End If

                            Try
                                'TODO Pass Branch COde here
                                oWebService.CreatePaymentCashListWithItems(oCashListItem, , alTransDetailCollection)
                                oPaymentCashListCollection = oCashListItem.PaymentCashList
                                oPaymentCashList = oPaymentCashListCollection.Item(0)
                                ViewState.Remove(sCashListItemKey)
                                ViewState.Remove(sTransDetailKey)
                                ViewState.Remove(sCashListKey)
                                ViewState.Add(sCashListItemKey, oPaymentCashList.CashListItemKey)
                                ViewState.Add(sTransDetailKey, oPaymentCashList.TransDetailKey)
                                ViewState.Add(sCashListKey, oCashListItem.CashListKey)

                                'Catch ex As Exception

                                If oCashListItem.PaymentItems(0).StatusCode.Trim = "PENDING" Then
                                    oCashListItem.PaymentItems(0).AllocationStatusCode = "Pending"
                                    btnAllocateCashlist.Visible = False
                                Else
                                    oCashListItem.PaymentItems(0).AllocationStatusCode = "Posted"
                                End If

                                Session(CNCashListItem) = oCashListItem
                                BindCashListItem()

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
                                Else
                                    Throw
                                End If
                            Finally
                                oPaymentCashListCollection = Nothing
                                oPaymentCashList = Nothing
                                oCashListItem = Nothing
                            End Try

                        Case Else
                            ' Do Nothing

                    End Select

                Finally
                    oWebService = Nothing
                    oRecieptCashListitem = Nothing
                    oCashListItem = Nothing
                End Try

            Else 'Cash Cheque Receipts
                If UserCanDoTask("CashChequeReceipt") Then
                    btnAllocateCashlist.Visible = True
                    If chkAutoAllocate.Enabled AndAlso chkAutoAllocate.Checked Then
                        btnAllocateCashlist.Visible = False
                        lnkCancel.Visible = False
                        lnkAdd.Visible = False
                        btnOK.Visible = True
                        btnOK.Attributes.Add("onclick", Nothing)
                    Else
                        btnAllocateCashlist.Visible = True
                        lnkCancel.Visible = False
                        lnkAdd.Visible = False
                        btnOK.Visible = True
                    End If
                    If Session("ModeType") = "Receipt" Then
                        If Session("TypeTrans") <> "INST" Then
                            '  btnCancel.Attributes.Add("onclick", "javascript:return CRCancelMsgBeforeAllocate();")
                            btnOK.Attributes.Add("onclick", "javascript:return CRCancelMsgBeforeAllocate();")
                        Else
                            btnOK.Visible = True
                        End If
                    End If
                    ' btnAdd.Enabled = False
                    Dim CashListItem As NexusProvider.ReceiptCashListItemType = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                    Dim oReceiptCashListCollection As NexusProvider.ReceiptCashListCollection = Nothing
                    CashListItem.AutoAllocateIfAble = chkAutoAllocate.Checked
                    '' payment hub calling
                    For Each oPaymentItem As NexusProvider.PaymentItems In CashListItem.ReceiptItems
                        If oPaymentItem.MediaTypeCode.Trim = "OCP" Then
                            PaymentHubProcessPurchase(oPaymentItem.PaymentHubDetails)
                            If oPaymentItem.PaymentHubDetails.ResultDescription = "0" Then
                                oPaymentItem.AllocationStatusCode = "Payment Captured"
                            End If
                        End If
                    Next

                    'TODO Pass Branch COde here
                    Try
                        If Session(CNTransBranchCode) Is Nothing Then
                            oReceiptCashListCollection = oWebService.CreateReceiptcashListWithItem(CashListItem)
                        Else
                            oReceiptCashListCollection = oWebService.CreateReceiptcashListWithItem(CashListItem, Session(CNTransBranchCode).ToString())
                        End If
                        Session(CNCashListItemWithTransDetailKey) = oReceiptCashListCollection
                    Catch
                        '' If Payment is successful but Cash list has failed, create a work manager tasks each for the failed item and assign to the task group configured in System Option.
                        For i As Integer = 0 To CashListItem.ReceiptItems.Count - 1
                            If CashListItem.ReceiptItems.Count > 0 Then
                                If CashListItem.ReceiptItems(i).MediaTypeCode = "OCP" AndAlso CashListItem.ReceiptItems(i).AllocationStatusCode = "Payment Captured" Then
                                    CreateWorkManagerTask(CashListItem.ReceiptItems(i).PaymentHubDetails.PartyKey, "Payment Received from the card but Cash receipt process failed for account " & CashListItem.ReceiptItems(i).AccountShortCode, "MEMO", "COMMON", GetAccountHandlerTaskGroup())
                                End If
                            End If
                        Next
                        Exit Sub
                    Finally
                        oWebService = Nothing
                        CashListItem = Nothing
                    End Try
                    Dim CashListItems As NexusProvider.ReceiptCashListItemType = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                    'Dim CashListItems As NexusProvider.PaymentItemsCollection = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType).PaymentItems
                    For i As Integer = 0 To CashListItems.ReceiptItems.Count - 1
                        If CashListItems.ReceiptItems.Count > 0 Then
                            If CashListItems.ReceiptItems(i).TypeCode.Trim.ToUpper = "INST" Then
                                CashListItems.ReceiptItems(i).AllocationStatusCode = "Allocated"
                                btnAllocateCashlist.Visible = False
                                lnkCancel.Visible = False
                                btnOK.Attributes.Add("onclick", Nothing)
                                btnOK.Visible = True
                            ElseIf CashListItems.ReceiptItems(i).TypeCode.Trim.ToUpper <> "BGDEPT" Then
                                If (CashListItems.ReceiptItems(i).MediaTypeCode <> "OCP") Or (CashListItems.ReceiptItems(i).MediaTypeCode = "OCP" AndAlso CashListItems.ReceiptItems(i).AllocationStatusCode = "Payment Captured") Then
                                    CashListItems.ReceiptItems(i).AllocationStatusCode = "Posted"
                                    btnAllocateCashlist.Visible = True
                                End If
                            End If
                        End If

                    Next
                    Session(CNCashListItem) = CashListItems
                    BindCashListItem()
                    If Session("AddMoreCashList") = "No" Then
                        lnkAdd.Visible = False
                        lnkCancel.Visible = False
                        btnOK.Visible = True
                    End If
                    If CashListItems.ReceiptItems.Count > 0 Then
                        If CashListItems.ReceiptItems(0).TypeCode.Trim.ToUpper = "BGDEPT" Then
                            Dim sAgentStartPage As String = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AgentStartPage
                            Response.Redirect(sAgentStartPage, False)
                        End If
                    End If
                    For i As Integer = 0 To oReceiptCashListCollection.Count - 1
                        If Not oReceiptCashListCollection(i).AutoAllocatePaymentSuccessful AndAlso chkAutoAllocate.Enabled AndAlso chkAutoAllocate.Checked Then
                            chkAutoAllocate.Checked = False
                            btnPost.Enabled = False
                            btnAllocateCashlist.Visible = True
                            btnOK.Visible = True
                            Dim sErrorMessage As String = IIf(GetLocalResourceObject("msgAllocationDeclined") Is Nothing, "Receipt transaction has not been auto-allocated.", GetLocalResourceObject("msgAllocationDeclined"))
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Auto Allocation", "alert('" + sErrorMessage + "')", True)
                            Exit Sub
                        ElseIf oReceiptCashListCollection(i).AutoAllocatePaymentSuccessful AndAlso chkAutoAllocate.Enabled AndAlso chkAutoAllocate.Checked Then
                            CashListItems.ReceiptItems(i).AllocationStatusCode = "Allocated"
                            btnAllocateCashlist.Visible = False
                            btnOK.Attributes.Add("onclick", Nothing)
                        End If
                    Next
                    Session(CNCashListItem) = CashListItems
                    BindCashListItem()
                End If
            End If
            btnPost.Enabled = False
            btnAllocateCashlist.Attributes.Remove("onclick")
            'btnOK.Visible = True
            Session("CashListItemsFirstLoad") = False
        End Sub

        'Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        '    Dim oCashListItems As NexusProvider.ReceiptCashListItemType
        '    If Session(CNCashListItem) IsNot Nothing AndAlso Session("ModeType") = "Receipt" Then
        '        oCashListItems = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
        '        If oCashListItems IsNot Nothing AndAlso oCashListItems.ReceiptItems.Count > 0 Then
        '            For iCount As Integer = oCashListItems.ReceiptItems.Count - 1 To 0 Step -1
        '                oCashListItems.ReceiptItems.Remove(iCount)
        '            Next
        '        End If
        '    End If
        '    If Session("ModeType") = "Payment" Or Session("ModeType") = "Receipt" Then 'Cash/Cheque - Payment or Receipt
        '        'Response.Redirect("~/secure/payment/CashList.aspx?Mode=" & Session("ModeType"), False)
        '        Session("ModeValue") = Session("ModeType")
        '        'CashList
        '        Dim changeTab0 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(0) a').tab('show')});"
        '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab0", changeTab0, True)
        '        'Session("hfActiveTab") = 0
        '        Session("hfPreviousTab") = 0
        '    ElseIf Session("ModeValue") = "IP" Then 'Insurer Payments
        '        'Response.Redirect("~/secure/payment/CashList.aspx?Mode=IP", False)
        '        Session("ModeValue") = "IP"
        '        'CashList
        '        Dim changeTab0 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(0) a').tab('show')});"
        '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab0", changeTab0, True)
        '        'Session("hfActiveTab") = 0
        '        Session("hfPreviousTab") = 0
        '    Else
        '        'Response.Redirect("~/secure/payment/CashList.aspx?Mode=Receipt", False)
        '        Session("ModeValue") = "Receipt"
        '        'CashList
        '        Dim changeTab0 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(0) a').tab('show')});"
        '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab0", changeTab0, True)
        '        'Session("hfActiveTab") = 0
        '        Session("hfPreviousTab") = 0
        '    End If

        '    ' code to close the screen on thickbox implementation 
        '    Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.CloseCashListItems();", True)

        'End Sub

        Protected Sub btnAllocateCashlist_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAllocateCashlist.Click
            Dim selectedIndex As List(Of Integer) = New List(Of Integer)
            For Each row In drgCashListItems.Rows
                Dim chkSelected As CheckBox = TryCast(row.Cells(0).Controls(1), CheckBox)
                If chkSelected IsNot Nothing AndAlso chkSelected.Checked Then
                    Dim AllocationIndex = row.RowIndex
                    selectedIndex.Add(AllocationIndex)
                    chkSelected.Enabled = False
                End If

            Next
            Session.Add("sSelectedAllocationIndex", selectedIndex)
            If Session("ModeValue") <> "IP" Then
                Session(CNMultipleAllocation) = True
            End If
            btnAllocateCashlist.Enabled = False
            btnOK.Enabled = False
            Dim oNexusConfig As Nexus.Library.Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork)

            Dim btnAllocateCancel As LinkButton = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnAllocateCancel"), LinkButton)
            Dim btnAllocateOK As LinkButton = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnAllocateOK"), LinkButton)
            Dim btnClose As LinkButton = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnClose"), LinkButton)

            btnAllocateCancel.Visible = True
            btnAllocateOK.Visible = True
            btnClose.Visible = False

            If Session("ModeValue") = "IP" Then
                btnOK.Attributes.Add("onclick", Nothing)
                btnOK.Enabled = True
                Try
                    oWebService = New NexusProvider.ProviderManager().Provider
                    Dim oAllocationDetails As New NexusProvider.AllocationDetails
                    Dim oTransAllocationDetails As New NexusProvider.AllocationDetails
                    Dim oAllocationDetailsCollection As New NexusProvider.AllocationDetailsCollections
                    Dim alTransDetailCollection As ArrayList = CType(Session(CNMarkedTransDetailList), ArrayList)
                    Dim iTransDetailCount As Integer = alTransDetailCollection.Count
                    Dim kSAMInsurerPaymentCalling As Integer = -99
                    Dim oAllocation As New NexusProvider.Allocation
                    Dim oMarkunmark As NexusProvider.MarkUnmarkTransaction

                    If iTransDetailCount > 0 Then
                        For Each oPair As Pair In alTransDetailCollection
                            oAllocationDetails = New NexusProvider.AllocationDetails
                            oAllocationDetails.TransdetailKey = oPair.First
                            oAllocationDetailsCollection.Add(oAllocationDetails)
                            oAllocationDetails = Nothing
                        Next
                    End If

                    oAllocationDetailsCollection = oWebService.GetTransactionDetails(Session(CNAccountkey), oAllocationDetailsCollection, Request.QueryString("BC"))

                    For Each oTempAllocationDetails As NexusProvider.AllocationDetails In oAllocationDetailsCollection
                        oAllocation = New NexusProvider.Allocation
                        For Each oPair As Pair In alTransDetailCollection
                            If Integer.Parse(oPair.First) = oTempAllocationDetails.TransdetailKey Then
                                oAllocation.AllocationAmount = Double.Parse(oPair.Second)
                                Exit For
                            End If
                        Next
                        '   oAllocation.AllocationAmount = oTempAllocationDetails.OutStandingamount 'CType(Session(CNTotalAmount), Decimal) 'oTempAllocationDetails.AllocationAmount
                        oAllocation.AllocationTimeStamp = oTempAllocationDetails.AllocationTimeStamp
                        oAllocation.AllocationTransdetailKey = oTempAllocationDetails.TransdetailKey
                        oTransAllocationDetails.Allocation.Add(oAllocation)
                        oAllocation = Nothing

                    Next

                    oTransAllocationDetails.AccountKey = Session(CNAccountkey)
                    oTransAllocationDetails.CashListItemKey = ViewState(sCashListItemKey)
                    oTransAllocationDetails.Amount = CType(Session(CNTotalAmount), Decimal) * -1
                    oTransAllocationDetails.TransdetailKey = ViewState(sTransDetailKey)
                    oTransAllocationDetails.WriteOffReason = kSAMInsurerPaymentCalling

                    If oWebService.UpdateAllocation(oTransAllocationDetails) Then
                        btnAllocateCashlist.Enabled = False
                        drgCashListItems.Rows(0).Cells(5).Text = "Allocated"
                        drgCashListItems.Columns(8).Visible = False

                        'UnMark the Allocated transaction
                        For Each oTempAllocationDetails As NexusProvider.AllocationDetails In oAllocationDetailsCollection
                            oMarkunmark = New NexusProvider.MarkUnmarkTransaction
                            If Session("Type").Trim() = PaymentType.P.ToString() Or Session("Type").Trim() = PaymentType.CP.ToString() Then
                                oMarkunmark.CurrencyCode = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType).CoreCashList.CurrencyCode
                            ElseIf Session("Type").Trim() = PaymentType.R.ToString() Then
                                oMarkunmark.CurrencyCode = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType).CoreCashList.CurrencyCode
                            Else
                                ' Do Nothing
                            End If

                            oMarkunmark.MarkStatus = NexusProvider.MarkStatusType.UnMark
                            oMarkunmark.PaymentAmount = "0.00"
                            oMarkunmark.TransactionKey = oTempAllocationDetails.TransdetailKey
                            oWebService.MarkUnmarkTransaction(oMarkunmark)
                        Next
                    End If

                    oTransAllocationDetails = Nothing

                Finally
                End Try
            Else
                If Session("ModeType") = "Payment" Then
                    If Session(CNCashListItemWithTransDetailKey) IsNot Nothing Then
                        Dim CashListItem As NexusProvider.PaymentCashListItemType = CType(Session(CNCashListItemWithTransDetailKey), NexusProvider.PaymentCashListItemType)
                        Dim arrlistTransidForCashList As New ArrayList
                        For Each row As GridViewRow In drgCashListItems.Rows
                            Dim chkCashLst As CheckBox = CType(row.FindControl("ChkCashListItem"), CheckBox)
                            If chkCashLst.Checked Then ' Checks if the item in the ListBox is selected or not 
                                Dim sAccountShotCode As String = Convert.ToString(row.Cells(4).Text) 'get short code
                                If CashListItem.PaymentCashList(row.RowIndex).AccountShortCode = sAccountShotCode Then
                                    arrlistTransidForCashList.Add(CashListItem.PaymentCashList(row.RowIndex).TransDetailKey)
                                End If
                            End If
                        Next

                        Session(CNTransdetailKeyfromCashList) = arrlistTransidForCashList
                    End If
                ElseIf Session("ModeType") = "Receipt" Then
                    If Session(CNCashListItemWithTransDetailKey) IsNot Nothing Then
                        Dim oReceiptCashListCollection As NexusProvider.ReceiptCashListCollection = CType(Session(CNCashListItemWithTransDetailKey), NexusProvider.ReceiptCashListCollection)
                        Dim arrlistTransidForCashList As New ArrayList

                        For Each row As GridViewRow In drgCashListItems.Rows
                            Dim chkCashLst As CheckBox = CType(row.FindControl("ChkCashListItem"), CheckBox)
                            If chkCashLst.Checked Then ' Checks if the item in the ListBox is selected or not 
                                Dim sAccountShotCode As String = Convert.ToString(row.Cells(4).Text) 'get short code
                                '    For i As Integer = 0 To oReceiptCashListCollection.Count - 1
                                If oReceiptCashListCollection(row.RowIndex).AccountShortCode = sAccountShotCode Then
                                    arrlistTransidForCashList.Add(oReceiptCashListCollection(row.RowIndex).TransDetailKey)
                                End If
                                '  Next
                            End If
                        Next
                        Session(CNTransdetailKeyfromCashList) = arrlistTransidForCashList
                    End If
                Else
                    If Request.QueryString("frompage") <> "Allocate" Then
                        Dim oReceiptCashListCollection As NexusProvider.ReceiptCashListCollection = CType(Session(CNCashListItemWithTransDetailKey), NexusProvider.ReceiptCashListCollection)
                        Dim arrlistTransidForCashList As New ArrayList
                        For i As Integer = 0 To oReceiptCashListCollection.Count - 1
                            arrlistTransidForCashList.Add(oReceiptCashListCollection(i).TransDetailKey)
                        Next
                        Session(CNTransdetailKeyfromCashList) = arrlistTransidForCashList
                    End If
                End If
                Dim btnCashListItemNext As LinkButton = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnCashListItemNext"), LinkButton)
                Dim btnCashListItemCancel As LinkButton = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName), ContentPlaceHolder).FindControl("btnCashListItemCancel"), LinkButton)
                btnCashListItemNext.Visible = False
                btnCashListItemCancel.Visible = False
                'Response.Redirect("~/Modal/Allocate.aspx?Mode=Allocation", False)
                Session("ModeValue") = "Allocation"
                hdnTabName.Value = "tab-Allocate"
                'CashList

                Session("AllocateFirstLoad") = True
                'Dim changeTab3 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(3) a').tab('show')});" commented byb shipali 
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab3", changeTab3, True) 'commented by shipali

                'Session("hfPreviousTab") = 3
                Dim ucallocate As UserControl = CType(Page.FindControl("ucAllocate"), UserControl)
                Page.LoadControl("~/Controls/allocation.ascx")
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "LoadTab", "loadTab();", True)
                btnAllocateOK.Attributes.Remove("onclick")
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "AllocateBtnsEnable", "AllocateBtnsEnable()", True)
                hfViewOption.Value = "ViewAllocation"
            End If
            ' btnCancel.Visible = False
            'here we want only 4th tab page load and also its first time load.
            'move to 4th(3) tab. 

        End Sub

        Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
            Dim oMultiStepApproval As NexusProvider.OptionTypeSetting = Nothing
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            oMultiStepApproval = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 65)

            If Session("ModeValue") = "IP" Then
                Response.Redirect("~/secure/InsurerPayments.aspx?Mode=IP&PartyKey=" + Request.QueryString("PartyKey"), False)

                If oMultiStepApproval.OptionValue = "1" AndAlso Session("Type").Trim() = PaymentType.P.ToString() Or Session("Type").Trim() = PaymentType.CP.ToString() Then
                    Response.Redirect("~/secure/InsurerPayments.aspx?Mode=IP&PartyKey=" + Request.QueryString("PartyKey"), False)
                Else
                    Response.Redirect("~/secure/InsurerPayments.aspx?Mode=IP&Query=Report&PartyKey=" + Request.QueryString("PartyKey"), False)

                End If

            Else
                Dim sAgentStartPage As String = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).AgentStartPage
                Response.Redirect(sAgentStartPage, False)
            End If
        End Sub

        ''' <summary>
        ''' Function to check if payment is of a Claim Payment type
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function IsClaimIncludedInPayment() As Boolean
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim oList As NexusProvider.LookupListCollection
            Dim v_sOptionList As System.Xml.XmlElement = Nothing
            oList = oWebService.GetList(NexusProvider.ListType.PMLookup, "Debtor_User_Groups", True, False, , , , v_sOptionList)
            'Dim hCurrentOptionColl As New Hashtable()
            If Session("PaymentType") = "CP" Then
                'Load the xml element 
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
            Return False
        End Function
        Sub CreateWorkManagerTask(ByVal nPartyKey As Integer, ByVal v_sTaskDescription As String, ByVal v_sTask As String, ByVal v_sTaskGroup As String, Optional ByVal v_sAllocationUserGroup As String = "UA")
            'Create the work manager task by passing following details
            Dim oWorkManager As New NexusProvider.WorkManager
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oParty As NexusProvider.BaseParty
            oParty = oWebService.GetParty(nPartyKey)
            If oParty IsNot Nothing Then
                'Base on the session value is personal / corporate client is loaded
                Select Case True
                    Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                        oParty = CType(Session(CNParty), NexusProvider.PersonalParty)

                    Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                        oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                End Select
            End If
            Select Case True
                Case TypeOf oParty Is NexusProvider.PersonalParty
                    oParty = CType(oParty, NexusProvider.PersonalParty)
                    oWorkManager.Client = CType(oParty, NexusProvider.PersonalParty).ClientSharedData.ResolvedName
                Case TypeOf oParty Is NexusProvider.CorporateParty
                    oParty = CType(oParty, NexusProvider.CorporateParty)
                    oWorkManager.Client = CType(oParty, NexusProvider.CorporateParty).CompanyName
            End Select

            oWorkManager.DueDate = Now
            oWorkManager.Description = v_sTaskDescription
            'oWorkManager.AllocationUser = oParty.Name
            oWorkManager.AllocationUserGroup = v_sAllocationUserGroup
            oWorkManager.IsUrgent = True
            oWorkManager.IsUrgentForUpdate = 1
            oWorkManager.IsComplete = False
            oWorkManager.IsTaskReview = True
            oWorkManager.Task = v_sTask
            oWorkManager.TaskGroup = v_sTaskGroup
            If HttpContext.Current.Session(CNParty) IsNot Nothing Then
                Dim oWmrk As New NexusProvider.KeyData
                oWmrk.KeyName = "PartyKey"
                oWmrk.KeyValue = CType(HttpContext.Current.Session(CNParty), NexusProvider.BaseParty).Key
                oWorkManager.KeyData.Add(oWmrk)
            End If
            If oWorkManager.TaskGroup IsNot Nothing Then
                oWorkManager.LockName = NexusProvider.SAMForInsurance.PureService.TaskLockName.InvalidValue
                oWebService.CreateWmTask(oWorkManager)
            End If
        End Sub

        Private Function GetAccountHandlerTaskGroup() As String
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sReturnCode As NexusProvider.OptionTypeSetting
            Dim sTaskGroup As String = ""
            Try
                sReturnCode = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5202)
                sTaskGroup = GetCodeForKey(NexusProvider.ListType.PMLookup, Convert.ToInt32(sReturnCode.OptionValue), "PMUser_Group", True)
            Catch ex As System.Exception
                Throw
            Finally
                sReturnCode = Nothing
                oWebService = Nothing
            End Try
            Return sTaskGroup.Trim()
        End Function

        Private Sub drgCashListItems_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles drgCashListItems.RowCommand
            If e.CommandName.ToUpper <> "DELETE" Then
                oWebService = New NexusProvider.ProviderManager().Provider
                Dim accountCode As String = String.Empty
                Dim commandArg As Integer = e.CommandArgument
                Dim documentRef As String = ""
                Dim optionType As New NexusProvider.OptionTypeSetting
                Dim documentDefaults As NexusProvider.DocumentDefaults = New NexusProvider.DocumentDefaults()
                Dim fileLocation As String
                Dim documentTemplateCode As String = String.Empty
                Dim stype As String = ""
                If (Session("Type") IsNot Nothing) Then
                    stype = Session("Type").Trim()
                End If
                Dim partyKey As Integer
                If Not IsNothing(Session(CNCashListItemWithTransDetailKey)) Then
                    If Session("ModeType") = "Receipt" OrElse stype = PaymentType.R.ToString() Then
                        documentTemplateCode = CType(Session(CNCashListItemWithTransDetailKey), NexusProvider.ReceiptCashListCollection)(commandArg).DocumentCode
                        documentRef = CType(Session(CNCashListItemWithTransDetailKey), NexusProvider.ReceiptCashListCollection)(commandArg).DocumentRef.Trim
                        accountCode = CType(Session(CNCashListItemWithTransDetailKey), NexusProvider.ReceiptCashListCollection)(commandArg).AccountShortCode
                    ElseIf Session("ModeType") = "Payment" OrElse stype = PaymentType.P.ToString() Then
                        documentTemplateCode = CType(Session(CNCashListItemWithTransDetailKey), NexusProvider.PaymentCashListItemType).PaymentCashList(commandArg).DocumentCode
                        documentRef = CType(Session(CNCashListItemWithTransDetailKey), NexusProvider.PaymentCashListItemType).PaymentCashList(commandArg).DocumentRef.Trim
                        accountCode = CType(Session(CNCashListItemWithTransDetailKey), NexusProvider.PaymentCashListItemType).PaymentCashList(commandArg).AccountShortCode
                    End If
                    Dim oAccountdetails As New NexusProvider.AccountDetails
                    Dim accountDetailsCollection As New NexusProvider.AccountDetailsDefaults
                    Dim branchcode As String = ""
                    If Session(CNBranchCode) IsNot Nothing Then
                        branchcode = Session(CNBranchCode).ToString()
                    End If
                    If Not String.IsNullOrEmpty(documentTemplateCode) Then
                        documentDefaults.documentTemplateCode = documentTemplateCode.Trim

                        If Not String.IsNullOrEmpty(accountCode) Then
                            Dim AccountSearchCriteria As New NexusProvider.AccountSearchCriteria
                            Dim AccountSearchCollection As NexusProvider.AccountSearchResultCollection
                            AccountSearchCriteria.ShortCode = accountCode.Trim
                            AccountSearchCollection = oWebService.FindAccounts(AccountSearchCriteria)
                            partyKey = AccountSearchCollection.Item(0).PartyKey
                            oAccountdetails.AccountCode = accountCode.Trim
                            oAccountdetails.PartyCnt = partyKey
                            If Not String.IsNullOrEmpty(documentRef) Then
                                oAccountdetails.DocumentRef = documentRef.Trim
                                accountDetailsCollection = oWebService.GetAccountDetails(oAccountdetails, branchcode)
                            End If
                        End If


                        optionType = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5009)

                        Dim documentType As NexusProvider.DocumentType

                        If optionType.OptionValue = "1" Then
                            documentType = NexusProvider.DocumentType.PDF
                            documentDefaults.FileType = "PDF"
                        Else
                            documentType = NexusProvider.DocumentType.DOCX
                            documentDefaults.FileType = "DOCX"
                        End If

                        fileLocation = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork").Portals.Portal(CMS.Library.Portal.GetPortalID()), Nexus.Library.Config.Portal).TempFileLocation & "\" & Guid.NewGuid.ToString & "\" & documentDefaults.documentTemplateCode.Trim + "." + documentDefaults.FileType
                        Dim insurance_file_cnt As Integer = 0
                        Dim insurance_folder_cnt As Integer = 0
                        If Not IsNothing(accountDetailsCollection) Then
                            If partyKey = 0 Then
                                partyKey = accountDetailsCollection.AccountDetails(0).PartyCnt
                            End If
                            insurance_file_cnt = accountDetailsCollection.AccountDetails(0).Insurance_filecnt
                            insurance_folder_cnt = accountDetailsCollection.AccountDetails(0).Insurance_foldercnt
                            If (insurance_folder_cnt = 0) Then
                                insurance_file_cnt = 0
                            End If
                        End If

                        Dim sURL As String = String.Empty

                        Dim row As GridViewRow = drgCashListItems.Rows(Convert.ToInt32(e.CommandArgument))
                        If e.CommandName.ToUpper = "EMAILLETTER" Then

                            If HttpContext.Current.Session.IsCookieless Then
                                sURL = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "Modal/SendEmail.aspx?loc=manual&PartyKey=" & partyKey & "&Document_ref=" & documentRef & "&InsuranceFileKey=0&modal=true&CalledFrom=CashList&ModeType=" & Session("ModeType") & "&KeepThis=true&TB_iframe=true&height=300&width=750"
                            Else
                                sURL = System.Web.Configuration.WebConfigurationManager.AppSettings("WebRoot") & "Modal/SendEmail.aspx?loc=manual&PartyKey=" & partyKey & "&Document_ref=" & documentRef & "&InsuranceFileKey=0&modal=true&CalledFrom=CashList&ModeType=" & Session("ModeType") & "&KeepThis=true&TB_iframe=true&height=300&width=750"
                            End If

                            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "OpenModal", "tb_show(null , '" & sURL & "' , null);", True)
                            Exit Sub
                        Else

                            oWebService.GenerateDocument(partyKey, insurance_file_cnt, insurance_folder_cnt, documentDefaults.documentTemplateCode, documentType, fileLocation, 0, Nothing, Nothing, documentRef)

                            If Not String.IsNullOrEmpty(documentRef) Then
                                documentRef = documentRef.Trim
                            Else
                                documentRef = Session("ModeType")
                            End If

                            documentDefaults.FileLocation = fileLocation
                            documentDefaults.DocumentName = documentRef.Trim + "." + documentDefaults.FileType
                            Session(CNDocumentToDownload) = documentDefaults
                            'test
                            'docFrame.Attributes.Add("src", "~/Secure/Download.aspx")
                            ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "RedirectToDownload", "RedirectToDownload();", True)
                            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "UnblockUI", "$.unblockUI();", True)
                        End If
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "DocumentNotFound", "alert('No document Has been Configured for this process');", True)
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "CashListItemNotFound", "alert('Cash List Item not found');", True)
                End If
                oWebService = Nothing
            End If
        End Sub
        Public Sub cancelBtnClick()
            Dim oCashListItems As NexusProvider.ReceiptCashListItemType
            If Session(CNCashListItem) IsNot Nothing AndAlso Session("ModeType") = "Receipt" Then
                oCashListItems = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                If oCashListItems IsNot Nothing AndAlso oCashListItems.ReceiptItems.Count > 0 Then
                    For iCount As Integer = oCashListItems.ReceiptItems.Count - 1 To 0 Step -1
                        oCashListItems.ReceiptItems.Remove(iCount)
                    Next
                End If
            End If
            If Session("ModeType") = "Payment" Or Session("ModeType") = "Receipt" Then 'Cash/Cheque - Payment or Receipt
                'Response.Redirect("~/secure/payment/CashList.aspx?Mode=" & Session("ModeType"), False)
                Session("ModeValue") = Session("ModeType")
                'CashList
                Dim changeTab0 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(0) a').tab('show')});"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab0", changeTab0, True)

                Session("hfPreviousTab") = 0
            ElseIf Session("ModeValue") = "IP" Then 'Insurer Payments
                'Response.Redirect("~/secure/payment/CashList.aspx?Mode=IP", False)
                Session("ModeValue") = "IP"
                'CashList
                Dim changeTab0 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(0) a').tab('show')});"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab0", changeTab0, True)

                Session("hfPreviousTab") = 0
            Else
                'Response.Redirect("~/secure/payment/CashList.aspx?Mode=Receipt", False)
                Session("ModeValue") = "Receipt"
                'CashList
                Dim changeTab0 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(0) a').tab('show')});"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab0", changeTab0, True)
                'Session("hfActiveTab") = 0
                Session("hfPreviousTab") = 0
            End If

            ' code to close the screen on thickbox implementation 
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.CloseCashListItems();", True)
        End Sub
        Public Sub PageLoad()
            Dim aa As Boolean = CBool(Session("CashListItemsFirstLoad"))

            If Not IsPostBack Then
                aa = True
            End If
            If aa Then
                'first time laod

                'To set the Focus
                ' Page.SetFocus(btnAdd)
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim sReturnCode As NexusProvider.OptionTypeSetting
                'Get the system Option "Auto Allocate If Able"
                Try
                    sReturnCode = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5059)
                    If sReturnCode IsNot Nothing AndAlso sReturnCode.OptionValue IsNot Nothing Then
                        If sReturnCode.OptionValue = "1" Then
                            chkAutoAllocate.Enabled = True
                            chkAutoAllocate.Checked = True
                        Else
                            chkAutoAllocate.Checked = False
                        End If
                    End If
                Finally
                    oWebService = Nothing
                End Try
                BindCashListItem()
                If Session(CNCashListItem) Is Nothing Then
                    If Session("ModeValue") = "IP" Then 'Insurer Payments

                        If Session("Type").Trim() = PaymentType.P.ToString() Or Session("Type").Trim() = PaymentType.CP.ToString() Then
                            Session.Add(CNCashListItem, New NexusProvider.PaymentCashListItemType)
                        ElseIf Session("Type").Trim() = PaymentType.R.ToString() Then
                            Session.Add(CNCashListItem, New NexusProvider.ReceiptCashListItemType)
                        Else
                            ' Do Nothing
                        End If

                        btnOK.Visible = True
                    Else

                        If Session("ModeType") = "Payment" Then 'Cash/Cheque Payments
                            Session.Add(CNCashListItem, New NexusProvider.PaymentCashListItemType)
                        Else 'Cash/Cheque Receipts
                            Session.Add(CNCashListItem, New NexusProvider.ReceiptCashListItemType)
                        End If

                    End If

                End If

                If Session("ModeValue") = "IP" Then
                    'btnAdd.PostBackUrl = "~/secure/payment/CashListItem.aspx?Mode=IP&Type=" + Session("Type") + "&PartyKey=" + Session("PartyKey")
                    'Session("ModeValue") = "IP"
                Else
                    'btnAdd.PostBackUrl = "~/secure/payment/CashListItem.aspx?Mode=CR&SetFlag=1&PartyKey=" + Session("PartyKey")
                    'Session("ModeValue") = "CR"
                    'Session("SetFlag") = 1
                End If
            End If
            If Session("EVENTARGUMENT") = "Refresh" Then 'Refresh The Value in case of Cash/Cheque Receipts and Payments
                BindCashListItem()
                If Session("TypeTrans") <> "INST" Then
                    'btnCancel.Attributes.Add("onclick", "javascript:return CRCancelMsgBeforePost();")

                    'btnOK.Attributes.Add("onclick", "javascript:return CRCancelMsgBeforePost();")
                End If
            ElseIf Session("EVENTARGUMENT") = "IPRefresh" Then 'Refresh The Value in case of Insurer Payments
                BindCashListItem()
                If drgCashListItems.Rows.Count > 0 And Session("ModeValue") = "IP" Then
                    'btnAdd.Enabled = False
                End If
            ElseIf Request.QueryString("frompage") = "Allocate" Then
                'BindCashListItem()
                If Session(CNCashListItemAllocationStatus) IsNot Nothing Then ''if coming from after successfull receipt/payment allocation
                    If Session("ModeType") = "Payment" Then
                        Dim oCashListItem As NexusProvider.PaymentCashListItemType = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                        Dim AllocationStatus As String = Convert.ToString(Session(CNCashListItemAllocationStatus))
                        Dim oPaymentCashList As NexusProvider.PaymentCashListItemType = CType(Session(CNCashListItemWithTransDetailKey), NexusProvider.PaymentCashListItemType)

                        If AllocationStatus = "completed" Then
                            Dim arrList As ArrayList = DirectCast(Session(CNTransdetailKeyfromCashList), ArrayList) ''allocated tran detail key
                            For i As Integer = 0 To arrList.Count - 1
                                Dim TransdetailKey As Int32 = Convert.ToInt32(arrList(i))

                                For index As Integer = 0 To oPaymentCashList.PaymentCashList.Count - 1
                                    If oPaymentCashList.PaymentCashList(index).TransDetailKey = TransdetailKey Then 'compare TransDetailKey
                                        'For receItmIndex = 0 To oCashListItem.PaymentCashList.Count - 1
                                        If oCashListItem.PaymentCashList(index).AccountShortCode = oPaymentCashList.PaymentCashList(index).AccountShortCode Then 'compare account short code
                                            oCashListItem.PaymentCashList.RemoveAt(index) 'remove Allocated item from cash list item session
                                            oCashListItem.PaymentItems.RemoveAt(index)
                                            oPaymentCashList.PaymentCashList.RemoveAt(index) 'remove Allocated item from cash list with tran detail Key item session
                                            Exit For
                                        End If
                                        'Next
                                    End If
                                Next

                            Next
                        End If

                        If oCashListItem.PaymentCashList.Count <= 0 Then ''if all payment allocated successfully redirect to default payment page
                            Session.Remove(CNCashListItemAllocationStatus)
                            Session.Remove(CNTransdetailKeyfromCashList)
                            'Response.Redirect("~/secure/payment/CashList.aspx?Mode=Payment", False)
                            Session("ModeValue") = "Payment"
                            'CashList
                            Dim changeTab0 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(0) a').tab('show')});"
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab0", changeTab0, True)

                        Else
                            Session.Remove(CNCashListItemAllocationStatus)
                            Session.Remove(CNTransdetailKeyfromCashList)
                            CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType).PaymentCashList = oCashListItem.PaymentCashList ''refresh cash list item session after removing allocated payment
                            CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType).PaymentItems = oCashListItem.PaymentItems
                            Session(CNCashListItemWithTransDetailKey) = oPaymentCashList ''refresh cash list item with tran key session after removing allocated payment
                            BindCashListItem()
                        End If
                    ElseIf Session("ModeType") = "Receipt" Then
                        Dim oCashListItem As NexusProvider.PaymentItemsCollection = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType).ReceiptItems ''cash receipt Items
                        Dim AllocationStatus As String = Convert.ToString(Session(CNCashListItemAllocationStatus))
                        Dim oReceiptCashListCollection As NexusProvider.ReceiptCashListCollection = CType(Session(CNCashListItemWithTransDetailKey), NexusProvider.ReceiptCashListCollection) ''receipt Items with tran detail Key

                        If AllocationStatus = "completed" Then
                            Dim arrList As ArrayList = DirectCast(Session(CNTransdetailKeyfromCashList), ArrayList) ''allocated tran detail key
                            For i As Integer = 0 To arrList.Count - 1
                                Dim TransdetailKey As Int32 = Convert.ToInt32(arrList(i))
                                For index As Integer = 0 To oReceiptCashListCollection.Count - 1
                                    If oReceiptCashListCollection(index).TransDetailKey = TransdetailKey Then 'compare TransDetailKey
                                        'For receItmIndex = 0 To oCashListItem.Count - 1
                                        If oCashListItem.Item(index).AccountShortCode = oReceiptCashListCollection(index).AccountShortCode Then 'compare account short code
                                            oCashListItem.RemoveAt(index) 'remove Allocated item from cash list item session
                                            oReceiptCashListCollection.RemoveAt(index) 'remove Allocated item from cash list with tran detail Key item session
                                            Exit For
                                        End If
                                        'Next
                                    End If
                                Next

                            Next
                        End If

                        If oCashListItem.Count <= 0 Then ''if all payment allocated successfully redirect to default receipt page
                            Session.Remove(CNCashListItemAllocationStatus)
                            Session.Remove(CNTransdetailKeyfromCashList)
                            'Response.Redirect("~/secure/payment/CashList.aspx?Mode=Receipt", False)
                            Session("ModeValue") = "Receipt"
                            'CashList
                            Dim changeTab0 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(0) a').tab('show')});"
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab0", changeTab0, True)
                            'Session("hfActiveTab") = 0
                            Session("hfPreviousTab") = 0
                        Else
                            Session.Remove(CNCashListItemAllocationStatus)
                            Session.Remove(CNTransdetailKeyfromCashList)
                            CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType).ReceiptItems = oCashListItem ''refresh cash list item session after removing allocated receipt
                            Session(CNCashListItemWithTransDetailKey) = oReceiptCashListCollection ''refresh cash list item with tran key session after removing allocated receipt
                            BindCashListItem()
                        End If
                    End If
                End If
                'btnAdd.Enabled = False
                'btnOK.Visible = True
            End If
        End Sub
    End Class
End Namespace
