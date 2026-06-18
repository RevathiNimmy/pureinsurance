Imports CMS.Library
Imports Nexus.Constants.Session
Imports NexusProvider
Namespace Nexus
    Partial Class secure_CashListTab : Inherits Frontend.clsCMSPage
        Dim CashListItemID As HiddenField
        Dim hfViewOption As HiddenField
        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Me.IsPostBack Then
                '     hdnTabName.Value = Request.Form(hdnTabName.UniqueID)
            Else
                If Session("ModeValue") = "PayNow" Then
                    Session("ModeValue") = "PayNow"
                    Session("SetFlag") = 0
                ElseIf Request.QueryString("Mode") = "IP" Then
                    Session("SetFlag") = 0
                Else
                    Session("SetFlag") = 1
                End If
                Session("TransdetailKeyDisc") = Nothing
            End If

            If Request("__EVENTARGUMENT") = "RefreshIP" Then

            Else
                If IsPostBack Then
                    hdnAddMoreCashList.Value = "No"
                    Dim btnCausedPostback As Control = GetControlThatCausedPostBack(Page)
                    'tab1
                    If btnCausedPostback IsNot Nothing Then


                        If btnCausedPostback.ID = "btnCashListItemNext" Then
                            If Session("ModeValue") = "IP" OrElse Request.QueryString("Mode") = "IP" Then
                                Session("ModeValue") = "IP"
                            ElseIf Session("ModeValue") = "PayNow" Then
                                Session("ModeValue") = "PayNow"
                            Else
                                If (Request.QueryString.HasKeys()) Then
                                    If (Session("ModeValue") <> "INSDEPOSIT") Then
                                        Session("ModeValue") = "CR"
                                    End If
                                Else
                                    Session("ModeValue") = "Payment"
                                End If
                                'issue with edit grid then next btn
                                'Session("SetFlag") = 1
                            End If
                        ElseIf btnCausedPostback.ID = "btnPost" Then
                            If Request.QueryString("Mode") = "Receipt" Then
                                If Session(CNCashListItem) IsNot Nothing Then
                                    Dim CashListItem As NexusProvider.PaymentItemsCollection = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType).ReceiptItems
                                    If CashListItem IsNot Nothing And CashListItem.Count > 0 Then
                                        For i As Integer = 0 To CashListItem.Count - 1
                                            If CashListItem.Item(i).TypeCode.Trim.ToUpper = "INST" Then
                                                btnCashListItemsAdd.Visible = False
                                                'btnCashListItemsCancel.Visible=False
                                            End If
                                        Next
                                    End If
                                End If
                            End If
                        End If
                    End If
                Else
                    If Session("CashListItemsFirstLoad") = False Then
                        Session(CNCashListItem) = Nothing
                    End If
                    Session("INSTALMENTPLANDETAILS") = Nothing
                    hdnAmountToPay.Value = Session(CNAmountToPay)
                    If Request.QueryString("Mode") = "IP" Then
                        hdnTotalAmountForIP.Value = Session(CNMarkedAmountSignForCashList)
                    End If
                    hdnTabName.Value = "tab-CashList"
                End If
                'btn
                If (Request("__EVENTTARGET") = "ucCashList") Then

                    If Session("ModeValue") = "IP" Then
                        Session("ModeValue") = "IP"
                    ElseIf Session("ModeValue") = "PayNow" Then
                        Session("ModeValue") = "PayNow"
                    Else
                        If (Request.QueryString.HasKeys()) Then
                            Session("ModeValue") = "CR"
                            Session("SetFlag") = 1
                        Else
                            Session("ModeValue") = "Payment"
                        End If
                    End If


                End If
            End If
            btnCashListItemCancel.Attributes.Add("onclick", "javascript:return CancelConfirmation();")
            btnCashListItemsCancel.Attributes.Add("onclick", "javascript:return CancelConfirmation();")

            If Request("__EVENTARGUMENT") = "ContinueAfterMulticurrency" Then
                Dim NextTab As String = String.Empty
                If (Request.QueryString.HasKeys()) Then
                    If (Request.QueryString("Mode") = "INS") Then
                        Session("ModeValue") = "INS"
                    End If
                End If
                ucCashListItem.PageLoad()
                ucCashListItem.okBtnClick(NextTab)
                If (Request.QueryString("Mode") <> "PayNow" AndAlso Session("ModeValue") <> "INSDEPOSIT") Then
                    hdnTabName.Value = "tab-CashListItems"
                    Session("btnCashListItemNextClick") = True
                    If NextTab <> String.Empty Then
                        hdnTabName.Value = NextTab
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "disableButton", "disableButton('Yes');", True)
                    End If
                    If Not (Session("ModeType") = "BulkCLP" Or (Request.QueryString("Mode") = "INS" AndAlso Session("ModeType") = "Receipt")) Then
                        ucCashListItems.PageLoad()
                    End If
                End If
                Return
            End If

            If Request("__EVENTARGUMENT") = "CancelMulticurrency" Then
                hdnTabName.Value = "tab-ReceiptType"
                ucCashListItem.PageLoad()
                Dim changeTab1 As String = " $(document).ready(function () {$('#divTabs a[href=""#tab-ReceiptType""]').tab('show');});"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab1", changeTab1, True)
                Return
            End If
        End Sub
        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            If Not IsPostBack Then
                Session("ModeValue") = Request.QueryString("Mode")
                hdnModeType.Value = Session("ModeValue")
            Else
                'Session("ModeValue") = IIf(Session("ModeValue") = "Allocation", Session("ModeValue"), Request.QueryString("Mode"))
            End If

            If (Request.QueryString("Mode") = "IP") Then
                Session("ModeValue") = IIf(Session("ModeValue") = "Allocation", Session("ModeValue"), Request.QueryString("Mode"))
                btnCashListItemsCancel.Visible = False
                btnCashListItemsAdd.Visible = False
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "LoadTab", "HideTab('" + Session("ModeValue") + "');", True)
            ElseIf (Request.QueryString("Mode") = "PayNow") OrElse (Request.QueryString("Mode") = "INSDEPOSIT") Then
                btnCashListItemNext.Text = "Ok"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "LoadTab", "HideTab('" + Session("ModeValue") + "');", True)
            End If

            If (Request.QueryString.HasKeys()) Then

            Else
                Session("ModeValue") = "Payment"
            End If

            'Note : The Session are used into cashlist.aspx and internal Controls instead of clearing all session , session should be clear while required code paralleled from 4.0 , Racti
            '(23 Sep 2015)

            'If Not IsPostBack AndAlso Not String.IsNullOrEmpty(sMode) Then
            '    'Cleaning of the session values
            '    ClearQuote()
            '    ClearClaims()
            '    ClearHeader()
            'End If

            'WPR48 
            ' If sMode = "IP" Then '
            '     If Session(CNTransInMultiCurr) = "Yes" Then
            '         Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "alert",
            '"<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){alert('The transactions that you have selected for payment have different transaction currencies.');});</script>")
            '     End If
            ' End If
        End Sub

        'tab 1
        Protected Sub btnCashListNext_Click(sender As Object, e As EventArgs) Handles btnCashListNext.Click
            'Add cash list items control

            Session("CashListItemsFirstLoad") = True
            Session("CashListItemFirstLoad") = True
            ucCashList.okBtnClick()
            hdnTabName.Value = "tab-ReceiptType"
            If Request.QueryString("Mode") = "PayNow" Then
                Session("ModeValue") = "PayNow"
            ElseIf Request.QueryString("Mode") = "IP" Then
                Session("ModeValue") = "IP"
            Else
                If (Request.QueryString.HasKeys()) Then
                    If (Request.QueryString("Mode") = "INS") Then
                        Session("ModeValue") = "INS"
                    ElseIf (Request.QueryString("Mode") = "INSDEPOSIT") Then
                        Session("ModeValue") = "INSDEPOSIT"
                    Else
                        Session("ModeValue") = "CR"
                    End If
                Else
                    Session("ModeValue") = "Payment"
                End If
            End If
            Session("btnOKClicked") = Nothing
            ucCashListItem.PageLoad()
            Dim changeTab1 As String = " $(document).ready(function () {$('#divTabs a[href=""#tab-ReceiptType""]').tab('show');});" 'commented by shipali
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab1", changeTab1, True) 'commented b y shipali

        End Sub

        'tab1
        Protected Sub btnCashListCancel_Click(sender As Object, e As EventArgs) Handles btnCashListCancel.Click
            'Call cancel button code of cash list control
            ucCashList.cancelBtnClick()
        End Sub

        'tab 2
        Protected Sub btnCashListItemNext_Click(sender As Object, e As EventArgs) Handles btnCashListItemNext.Click
            Dim NextTab As String = String.Empty
            If (Request.QueryString.HasKeys()) Then
                If (Request.QueryString("Mode") = "INS") Then
                    Session("ModeValue") = "INS"
                End If
            End If
            ucCashListItem.okBtnClick(NextTab)
            If (Request.QueryString("Mode") <> "PayNow" AndAlso Session("ModeValue") <> "INSDEPOSIT") Then
                hdnTabName.Value = "tab-CashListItems"
                Session("btnCashListItemNextClick") = True
                'installment validation fail then remain on same page.
                If NextTab <> String.Empty Then
                    hdnTabName.Value = NextTab
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "disableButton", "disableButton('Yes');", True)
                End If
                If (Session("ModeType") = "BulkCLP" Or (Request.QueryString("Mode") = "INS" AndAlso Session("ModeType") = "Receipt")) Then

                Else
                    ucCashListItems.PageLoad()
                End If
            End If
            'End If
        End Sub
        'tab 3
        Protected Sub btnCashListItemsAdd_Click(sender As Object, e As EventArgs) Handles btnCashListItemsAdd.Click
            'load cash list item
            Session("CashListItemFirstLoad") = True
            'Dim ucCashListItem As Control = LoadControl("~/Controls/CashListItem.ascx")
            'Dim ucCashListitem1 As Controls_CashListItem = TryCast(ucCashListItem, Controls_CashListItem)
            'ucCashListitem1.ID = "cli"
            Dim oNexusFrameWork As Nexus.Library.Config.NexusFrameWork = CType(System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork)
            CashListItemID = CType(CType(Nexus.Utils.GetMasterPlaceHolder(Page, oNexusFrameWork.MainContainerName), ContentPlaceHolder).FindControl("hfCashListItemID"), HiddenField)
            CashListItemID.Value = String.Empty

            If Session("ModeValue") = "IP" Then
                'btnCashListItemsAdd.PostBackUrl = "~/secure/payment/CashListItem.aspx"
                Session("ModeValue") = "IP"
                Session("PartyKey") = ""
                Session("PARTY_KEY") = ""
                'Session("SetFlag") = 0
                'ucCashListItem = LoadControl("~/Controls/CashListItem.ascx?Mode=IP&Type=" + Session("Type") + "&PartyKey=" + Session("PartyKey"))
                'ucCashListitem1.ModeCashList = "IP"
                'ucCashListitem1.Type = Session("Type") 'will have To use session?
                'ucCashListitem1.PartyKey = Session("PartyKey")


            Else
                'btnCashListItemsAdd.PostBackUrl = "~/secure/payment/CashListItem.aspx"
                'ucCashListItem = LoadControl("~/Controls/CashListItem.ascx?Mode=CR&SetFlag=1&PartyKey=" + Session("PartyKey"))
                'ucCashListitem1.ModeCashList = "CR"
                'ucCashListitem1.SetFlag = "1"
                'ucCashListitem1.PartyKey = Session("PartyKey")
                Session("ModeValue") = "CR"
                Session("SetFlag") = 1
                Session("PartyKey") = ""
                Session("PARTY_KEY") = ""
                hdnTabName.Value = "tab-ReceiptType"
                hdnAddMoreCashList.Value = "Yes"
                Session("AddMoreCashList") = "Yes"
                Session("btnOKClicked") = Nothing
                ucCashListItem.PageLoad()
            End If
            ' phCashlistitem.Controls.Add(ucCashListItem)

            'no click event for add button.
            'updCashList_Item.Update()
            'UcCashListItem.PageLoad()
            'CashListItem
            ' Dim changeTab1 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(1) a').tab('show')});" 'commented by shipali
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab1", changeTab1, True) 'commented b y shipali
            'Session("hfActiveTab") = 1
            'Session("hfPreviousTab") = 1 commented by shipali
            'open 2nd tab with query string


            hdnTabName.Value = "tab-ReceiptType"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "LoadTab", "loadRefresh('Yes');", True)
            ucCashListItem.ClearFields()
            ' Dim changeTab1 As String = " $(document).ready(function () {$('#divTabs a[href=""#tab-ReceiptType""]').tab('show');});" 'commented by shipali
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab1", changeTab1, True) 'commented b y shipali

            ' ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab", "$(document).ready(function () {__doPostBack('<%=btnCashListItemsAdd.UniqueID%>', 'CRAddCashListItem')});", True)
            ' ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab1", "$(document).ready(function () {__doPostBack('<%=btnCashListItemsAdd.UniqueID%>', 'CRAddCashListItem')});", True)

        End Sub

        Protected Sub btnCashListItemsCancel_Click(sender As Object, e As EventArgs) Handles btnCashListItemsCancel.Click
            'Dim ucCashListItem As Control = LoadControl("~/Controls/CashListItem.ascx")
            'phCashlistitems.Controls.Add(ucCashListItem)
            If Session("ModeValue") = "VP" OrElse Session("ModeValue") = "AP" OrElse Session("ModeValue") = "DP" Then
                Response.Redirect("~/secure/AuthorizePayments.aspx?Type=Task&CashListItemKey=" & Request.QueryString("CashListItemKey") & " & Mode = " & Session("ModeValue") & "", True)
            End If

            If Session("ModeValue") = "IP" Then
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
            ElseIf Session("ModeType") = "Payment" Then
                'CashListItems
                Dim changeTab0 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(0) a').tab('show')});"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab0", changeTab0, True)
                'Session("hfActiveTab") = 2
                Session("hfPreviousTab") = 2
                Response.Redirect("~/secure/payment/CashListNew.aspx?Mode=Payment", True)
            ElseIf Session("ModeType") = "Receipt" Then
                Response.Redirect("~/secure/payment/CashListNew.aspx?Mode=Receipt", True)
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
        Protected Sub btnAllocateCancel_Click(sender As Object, e As EventArgs) Handles btnAllocateCancel.Click
            If Session(CNCashListItemAllocationStatus) IsNot Nothing Then ''if coming from after successfull receipt/payment allocation
                If Session("ModeType") = "Payment" Then
                    Dim oCashListItem As NexusProvider.PaymentCashListItemType = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                    Dim oPaymentCashList As NexusProvider.PaymentCashListItemType = CType(Session(CNCashListItemWithTransDetailKey), NexusProvider.PaymentCashListItemType)

                    If oCashListItem.PaymentCashList.Count <= 0 Then ''if all payment allocated successfully redirect to default payment page
                        Session.Remove(CNCashListItemAllocationStatus)
                        Session.Remove(CNTransdetailKeyfromCashList)
                    Else
                        Session.Remove(CNCashListItemAllocationStatus)
                        Session.Remove(CNTransdetailKeyfromCashList)
                        CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType).PaymentCashList = oCashListItem.PaymentCashList ''refresh cash list item session after removing allocated payment
                        CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType).PaymentItems = oCashListItem.PaymentItems
                        Session(CNCashListItemWithTransDetailKey) = oPaymentCashList ''refresh cash list item with tran key session after removing allocated payment
                    End If
                ElseIf Session("ModeType") = "Receipt" Then
                    Dim oCashListItem As NexusProvider.PaymentItemsCollection = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType).ReceiptItems ''cash receipt Items
                    Dim oReceiptCashListCollection As NexusProvider.ReceiptCashListCollection = CType(Session(CNCashListItemWithTransDetailKey), NexusProvider.ReceiptCashListCollection) ''receipt Items with tran detail Key

                    If oCashListItem.Count <= 0 Then ''if all payment allocated successfully redirect to default receipt page
                        Session.Remove(CNCashListItemAllocationStatus)
                        Session.Remove(CNTransdetailKeyfromCashList)
                    Else
                        Session.Remove(CNCashListItemAllocationStatus)
                        Session.Remove(CNTransdetailKeyfromCashList)
                        CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType).ReceiptItems = oCashListItem ''refresh cash list item session after removing allocated receipt
                        Session(CNCashListItemWithTransDetailKey) = oReceiptCashListCollection ''refresh cash list item with tran key session after removing allocated receipt
                    End If
                End If
            End If
            Session("CashListItemsFirstLoad") = True
            ucCashListItems.PageLoad()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "CancelClick", "DisableTabOnAllocateCancel();", True)
            Dim changeTab2 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(2) a').tab('show')});"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab2", changeTab2, True)
            Session("hfPreviousTab") = 0
        End Sub
        Protected Sub btnCashListItemCancel_Click(sender As Object, e As EventArgs) Handles btnCashListItemCancel.Click
            If Session("ModeValue") = "VP" OrElse Session("ModeValue") = "AP" OrElse Session("ModeValue") = "DP" Then
                Response.Redirect("~/secure/AuthorizePayments.aspx?Type=Task&CashListItemKey=" & Request.QueryString("CashListItemKey") & " & Mode = " & Session("ModeValue") & "", True)
            End If

            If Session("ModeValue") = "IP" Then
                'Insurer Payments
                Dim sType As String = Session("Type")
                Response.Redirect("~/secure/InsurerPayments.aspx?Mode=IP", False)
                Session("ModeValue") = "IP"
                Session("Type") = sType
                'CashListItems
                Dim changeTab2 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(2) a').tab('show')});"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab2", changeTab2, True)
                Session("hfPreviousTab") = 2
            ElseIf Session("ModeType") = "Payment" Then
                Response.Redirect("~/secure/payment/CashListNew.aspx?Mode=Payment", True)
            ElseIf Session("ModeType") = "Receipt" Then
                ucCashListItem.cancelBtnClick()
                Response.Redirect("~/secure/payment/CashListNew.aspx?Mode=Receipt", True)
            ElseIf Session("ModeValue") = "INS" Then
                Response.Redirect("~/PremiumFinance/PremiumFinancePlan.aspx?Type=EditPlan", True)
            ElseIf Session("ModeValue") = "INSDEPOSIT" Then
                Session("ModeValue") = "INSDEPOSIT"
                'CashList
                Dim changeTab0 As String = " $(document).ready(function () {$('.tab-cashlist li:eq(0) a').tab('show')});"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "changeTab0", changeTab0, True)
                Session("hfPreviousTab") = 0
            Else
                Response.Redirect("~/secure/PremiumDisplay.aspx", True)
            End If

        End Sub
        Protected Sub btnAllocateOK_Click(sender As Object, e As EventArgs) Handles btnAllocateOK.Click
            ucAllocate.ClickOK()
            ucCashListItems.PageLoad()
            'ucAllocate.SaveGridInObject()

            Dim isBtnDisabled As Boolean = ucAllocate.btnsDisable()
            If isBtnDisabled Then
                btnAllocateOK.Attributes.Add("onclick", "javascript:changeToTab2(); return false;")
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "CancelClick", "DisableTabOnAllocateCancel();", True)

            End If
        End Sub

        Protected Function GetControlThatCausedPostBack(ByVal page As Page) As Control
            Dim ctrl As Control = Nothing
            Dim ctrlName As String = page.Request.Params.[Get]("__EVENTTARGET")
            If Not String.IsNullOrEmpty(ctrlName) Then ctrl = page.FindControl(ctrlName)
            Return ctrl
        End Function
    End Class
End Namespace