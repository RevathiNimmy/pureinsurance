<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CashListItem.ascx.vb"
    Inherits="Nexus.Controls_CashListItemtab" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/Controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/AddressCntrl.ascx" TagName="AddressCntrl" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



<script type="text/javascript">
    $(document).ready(function () {
        var txtAmount = document.getElementById('<%= txtAmount.ClientId%>');
        var myNum = new Number(txtAmount.value);
        if (myNum.toFixed(2) != 'NaN') {
            txtAmount.value = myNum.toFixed(2);
        }

    });
    var addMoreCashList = $('#hdnAddMoreCashList').val();
    addMoreCashList = '<%=Session("AddMoreCashList")%>'; 
    if (addMoreCashList == "Yes")
        __doPostBack("CashListItem", "RefreshIP");
   

    Date.fromDDMMYYYY = function (s) { return (/^(\d\d?)\D(\d\d?)\D(\d{4})$/).test(s) ? new Date(RegExp.$3, RegExp.$2 - 1, RegExp.$1) : new Date(s) }
    function WarningMsg(oSrc, args) {
        //if user has entered cheque date prior to 3 months from today then take the confirmation before proceeding
        //alert("Cheque date more than 3 months prior to the system date. Do You Want To Continue?");
        var dtChequeDate = Date.fromDDMMYYYY((document.getElementById('<%= Cash_List_Item__Cheque_Date.ClientId%>').value));
        var dtBeforeNintyDays = Date.fromDDMMYYYY((document.getElementById('<%= hiddentxtwarningMinChequeDate.ClientId%>').value));
        var dtBeforeOneFiftyDays = Date.fromDDMMYYYY((document.getElementById('<%= hiddentxtwarningchequedate.ClientId%>').value));

        if (dtChequeDate < dtBeforeNintyDays && dtChequeDate >= dtBeforeOneFiftyDays) {
            //means user has entered cheque date prior to 3 months from today and for safe side consider 5 months also
            var sTakeConfirmation = confirm("Cheque date more than 3 months prior to the system date. Do You Want To Continue?");
            if (sTakeConfirmation == false) {
                args.IsValid = false;
                return false;
            }
        }
        args.IsValid = true;
    }

    function CancelConfirmation() {
        var sTakeConfirmation = confirm("Cancelling will lose any changes. Do you want to cancel?");
        if (sTakeConfirmation == false) {
            return false;
        }
        else {
            return true;
        }
    }
    function DeclineWarning(sWarningMsg) {
        var sConfirmation = confirm(sWarningMsg);
        if (sConfirmation == false) {
            return false;
        }
        else {
            return true;
        }
    }

    function CheckFutureDate(oSrc, args) {
        //if user has entered cheque date after the collection date

        var dtChequeDate = Date.fromDDMMYYYY((document.getElementById('<%= Cash_List_Item__Cheque_Date.ClientId%>').value));
        var dtCollDate = Date.fromDDMMYYYY((document.getElementById('<%= Cash_List_Item__Collection_Date.ClientId%>').value));

        if (dtChequeDate > dtCollDate) {
            //means user has entered cheque date after the collection date
            args.IsValid = false;
            return false;
        }
        args.IsValid = true;
    }

    function ValidateAmount(oSrc, args) {
        var txtAmount = document.getElementById('<%= txtAmount.ClientId%>');
        if (isNaN(txtAmount.value) || parseFloat(txtAmount.value) <= 0) {
            args.IsValid = false;
        }
        else {
            args.IsValid = true;
        }

    }
    function CheckAmount() {

        var txtAmount = document.getElementById('<%= txtAmount.ClientId%>');
        var txtTenderedAmount = document.getElementById('<%= txtTendered.ClientId%>');
        var txtChange = document.getElementById('<%= txtChange.ClientId%>');

        var myNum = new Number(txtAmount.value);
        if (myNum.toFixed(2) != 'NaN') {
            txtAmount.value = myNum.toFixed(2);
        }


        txtTenderedAmount.value = txtAmount.value;
        txtChange.value = '0.00';

        var myNum = new Number(txtTenderedAmount.value);
        if (myNum.toFixed(2) != 'NaN') {
            txtTenderedAmount.value = myNum.toFixed(2);
        }
    }

    function isInteger1(e) {

        var key = window.event ? e.keyCode : e.which;
        var keychar = String.fromCharCode(key);
        var ValidChars = "0123456789.";
        var IsNumber = true;
        if (ValidChars.indexOf(keychar) == -1) {
            IsNumber = false;
        }
        return IsNumber;
    }

    function CheckTenderedAmount(errorMsg) {

        var txtAmount = document.getElementById('<%= txtAmount.ClientId%>');
        var txtTenderedAmount = document.getElementById('<%= txtTendered.ClientId%>');
        var txtChange = document.getElementById('<%= txtChange.ClientId%>');

        var myNum = new Number(txtAmount.value);
        if (myNum.toFixed(2) != 'NaN') {
            txtAmount.value = myNum.toFixed(2);
        }

        if (txtTenderedAmount.value > txtAmount.value) {
            txtChange.value = txtTenderedAmount.value - txtAmount.value;
            var myNum = new Number(txtChange.value);
            if (myNum.toFixed(2) != 'NaN') {
                txtChange.value = myNum.toFixed(2);
            }
        }
        else if (txtTenderedAmount.value < txtAmount.value) {
            alert(errorMsg);

            txtTenderedAmount.value = txtAmount.value;
            txtChange.value = '0.00';
            var myNum = new Number(txtTenderedAmount.value);
            if (myNum.toFixed(2) != 'NaN') {
                txtTenderedAmount.value = myNum.toFixed(2);
            }
        }
    }

    function setAccount(sShortCode, shiddenShortCode, sAccountName, sPartyKey, sCurrencyCode, htype, sLedgerCode, sContactName, sAccountName)
    {
        tb_remove();
        document.getElementById('<%= txtAccount.ClientId%>').value = unescape(sShortCode);
        document.getElementById('<%= hiddenAccountKey.ClientId%>').value = shiddenShortCode;
        document.getElementById('<%= hAccountName.ClientId%>').value = unescape(sAccountName);
        document.getElementById('<%= hPartyKey.ClientId%>').value = sPartyKey;

        if (sLedgerCode == "AG")
        {
            //AG=Agent, need to show the Account Name
            document.getElementById('<%= txtName.ClientId%>').value = unescape(sAccountName);
            //document.getElementById('<%= txtChequeHolderName.ClientId%>').value=sAccountName;
            //document.getElementById('<%= txtNameOnCard.ClientId%>').value=sAccountName;
            if (sAccountName != '')
            {
                document.getElementById('<%= hiddenTempText.ClientID %>').value = document.getElementById('<%= txtAccount.ClientID %>').value;
            }
        }
        else
        {
            //SA=Client, need to show the Contact Name
            document.getElementById('<%= txtName.ClientId%>').value = sContactName;
            //document.getElementById('<%= txtChequeHolderName.ClientId%>').value=sContactName;
            //document.getElementById('<%= txtNameOnCard.ClientId%>').value=sContactName;
            if (sAccountName != '') {
                document.getElementById('<%= hiddenTempText.ClientID %>').value = document.getElementById('<%= txtAccount.ClientID %>').value;
            }
        }

        __doPostBack("CashListItem", "RefreshIP");
    }

    function checkAuthorisationAmount(confirmMsg, sWarningMsg)
    {
        var dPayAmount = document.getElementById('<%= txtAmount.ClientId%>').value;
        var dLimit = document.getElementById('<%= txtLimit.ClientId%>').value;
        var bResponse;

        if (Number(dPayAmount) > Number(dLimit)) {
            //update warning message with the current values            
            alert(sWarningMsg);
            return false;
        }
        else {
            bResponse = window.confirm(confirmMsg);
        }
        if (bResponse) {
            return true;
        }
        else {
            return false;
        }

    }
    function getAccount(bPostBackTo)
    {
        if (document.getElementById('<%= txtAccount.ClientID %>').value == '')
        {
            return false;
        }
        else {
            document.getElementById('<%= hiddenTempText.ClientID %>').value = document.getElementById('<%= txtAccount.ClientID %>').value;
            __doPostBack(bPostBackTo, "RefreshIP");
        }
    }

    function isInteger(e) {
        var key = window.event ? e.keyCode : e.which;
        var keychar = String.fromCharCode(key);
        reg = /\d/;
        return reg.test(keychar);

    }

    function setDecline(sConfirmation) {
        tb_remove();
        document.getElementById('<%= txtDecline.ClientId%>').value = sConfirmation;
        document.getElementById('<%= btnRefresh.ClientId%>').click();
    }

    function OpenWorkManager() {
        tb_remove();
        window.location = "../secure/WorkManager.aspx"
    }
    function setClient(sClientName, sClientKey) {

        tb_remove();

    }

    function pageLoad()
    {
        //this is needed if the trigger is external to the update panel   
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_beginRequest(OnBeginRequest);
        manager.add_endRequest(OnEndRequest);
        if (document.getElementById('<%=hdnIsInstalment.ClientID%>').value == "1") {
            ShowInstalTab()
        } else {
            HideInstalTab();
        }
    }

    function ShowHideInstalTab()
    {
        if (document.getElementById('<%=hdnIsInstalment.ClientID%>').value == "1")
        {
            ShowInstalTab()
        }
        else
        {
            HideInstalTab();
        }
    }


    function OnBeginRequest(sender, args) {

        var postBackElement = args.get_postBackElement();
        if (postBackElement.id == 'chkAmtSelect') {
            $get(uprogQuotes).style.display = "block";
        }
        if (postBackElement.id == 'btnAccount') {
            $get(piAccount).style.display = "block";
        }

        //disable the button (or whatever else we need to do) here
       var btnSubmit = document.getElementById('<%=hdnIsInstalment.ClientId%>');
        var btnCancel = document.getElementById('<%= btnCancel.ClientId%>');

        if (btnSubmit != null) {
            btnSubmit.disabled = true;
        }

        if (btnCancel != null) {
            btnCancel.disabled = true;
        }
    }
    function OnEndRequest(sender, args) {
        //enable the button (or whatever else we need to do) here
       var btnSubmit = document.getElementById('<%= btnOk.ClientId%>');
        var btnCancel = document.getElementById('<%= btnCancel.ClientId%>');

        if (btnSubmit != null) {
            btnSubmit.disabled = false;
        }

        if (btnCancel != null) {
            btnCancel.disabled = false;
        }

    }

    function HidePaymentTab() {
        document.getElementById("tab-payment").style.display = "none";
        document.getElementById("liPaymentTab").style.display = "none";
        document.getElementById("liInstalmentsTab").style.display = "none";
    }

    function RedirectToPremiumFinancePlan() {
        alert('<%= GetLocalResourceObject("msg_SettlementSuccess")  %>');
        window.location = "../../PremiumFinance/PremiumFinancePlan.aspx?Type=EditPlan"
    }

    function isAlphaNumeric(e) {
        var key = window.event ? e.keyCode : e.which;
        var keychar = String.fromCharCode(key);
        reg = /^[a-z0-9]+$/i;
        return reg.test(keychar);
    }
    function HideInstalTab() {
        document.getElementById("liInstalmentsTab").style.display = "none";
    }
    function ShowInstalTab() {
        document.getElementById("liInstalmentsTab").style.display = "block";
    }
    function TotalCurrentPlanSelectedInstalment(ControlID)
    {
        var crTotalAmount = 0.00;
        var OverAllAmount = 0.00;
        var isChecked = false;
        var CleanNumber = 0;
        var ogrdInstallmentQuotes = document.getElementById("<%= grdInstallmentQuotes.ClientID%>")
        if (ogrdInstallmentQuotes != null)
        {
            ogrdInstallmentQuotes = document.getElementById("<%= grdInstallmentQuotes.ClientID%>").getElementsByTagName("tr");
            var txtCurrentPlanSelectedTotal = document.getElementById("<%= txtCurrentPlanSelectedTotal.ClientID%>");
            var txtOverallSelectedTotal = document.getElementById("<%= txtOverAllSelectedTotal.ClientID%>");

            if (ogrdInstallmentQuotes.length > 0) {
                for (rowCount = 1; rowCount < ogrdInstallmentQuotes.length; rowCount++) {
                    if (ogrdInstallmentQuotes[rowCount].cells[0].getElementsByTagName("input")[0].checked == true) {
                        isChecked = true;
                        CleanNumber = ogrdInstallmentQuotes[rowCount].cells[3].innerHTML.replace(',', '');
                        crTotalAmount = crTotalAmount + parseFloat(CleanNumber);
                        OverAllAmount = parseFloat(txtOverallSelectedTotal.value.replace(',', '')) + parseFloat(CleanNumber);
                    }


                }
                txtCurrentPlanSelectedTotal.value = crTotalAmount.toFixed(2);
                document.getElementById("<%= txtAmount.ClientID%>").value = OverAllAmount.toFixed(2);
                document.getElementById('<%= txtTendered.ClientId%>').value = OverAllAmount.toFixed(2);
            }
        }

        var RemovedAmount = 0;

        if (document.getElementById(ControlID).checked)
        {
            __doPostBack("<%=UpdInstalments.UniqueID%>", "CHECKED");
        }
        else
        {

               //Remove the unselected amount from the grid and update the amount textbox, and then do a post back.
               var id = document.getElementById(ControlID).id;
               document.getElementById("<%= hdInstalmentNumber.ClientID%>").value = $("#" + id + "").closest('tr').find('td:eq(1)').text()
               if (ogrdInstallmentQuotes.length > 0) {
                   for (rowCount = 1; rowCount < ogrdInstallmentQuotes.length; rowCount++) {
                       if (document.getElementById("<%= hdInstalmentNumber.ClientID%>").value != "") {
                           if (ogrdInstallmentQuotes[rowCount].cells[1].innerHTML == document.getElementById("<%= hdInstalmentNumber.ClientID%>").value) {
                               RemovedAmount = ogrdInstallmentQuotes[rowCount].cells[3].innerHTML.replace(',', '');
                           }
                       }
                   }
               }
               var UpdatedAmount = parseFloat(txtOverallSelectedTotal.value) - parseFloat(RemovedAmount);
               document.getElementById("<%= txtAmount.ClientID%>").value = UpdatedAmount.toFixed(2);
               __doPostBack("<%=UpdInstalments.ClientID%>", "UNCHECKED");
        }
       }
       function TakeExactAmount() {
           tb_remove();
           var txtAmount = document.getElementById("<%= txtAmount.ClientID%>");
           var txtOverAllSelectedTotal = document.getElementById("<%= txtOverAllSelectedTotal.ClientID%>");
        if (parseFloat(txtAmount.value) > parseFloat(txtOverAllSelectedTotal.value)) {
          
            alert('<%= GetLocalResourceObject("msg_AmountGreater")%>');
            document.getElementById('tabInstalments').click();
            document.getElementById('tabInstalments').show();
        }
        else if (parseFloat(txtAmount.value) < parseFloat(txtOverAllSelectedTotal.value)) {
            __doPostBack("<%=UpdInstalments.UniqueID%>", "TakeExactAmount");
           } 
 }
function ReadWriteOffReason(WriteOffReasonID) {

    document.getElementById("<%= hdnWriteOffReasonID.ClientID%>").value = WriteOffReasonID;
        __doPostBack("<%=UpdInstalments.UniqueID%>", "WriteOFF");
        tb_remove();
    }

    function TakeRcptDiffConfirmation(sAmount, url) {
        var sTakeConfirmation = confirm("The receipt amount is different to the instalment amounts. \n\n Confirming this receipt will take a loss of " + sAmount + "  on the instalment");
        if (sTakeConfirmation == false) {
            return false;

        }
        else {
            //  return true;
            tb_show(null, url, null);
        }
    }
    //Start: #51740 & 51745  Hiding temporary the Option "TP Instalment Debt" as discussed with Architect and BA for defects 51740 & 51745
    function HideTemproryReceiptTypeRecord() {
        $("#<%=GISLookup_ReceiptType.ClientID%> option").each(function () {
           var value = $(this).val();
           if (value == "TPPF") {
               $(this).remove();
           }
       });
   }
    jQuery(document).ready(function ($) {
        HideTemproryReceiptTypeRecord();
    });
    var parameter = Sys.WebForms.PageRequestManager.getInstance();

    parameter.add_endRequest(function () {
        HideTemproryReceiptTypeRecord();
    });
    //End: #51740 & 51745  Hiding temporary the Option "TP Instalment Debt" as discussed with Architect and BA for defects 51740 & 51745
</script>

<div id="Controls_CashListItem">
    <div class="card">
        <div class="card-heading">
            <h1>
                <asp:Label ID="lblReceiptTypeHeading" runat="server" Text="<%$Resources:lbl_ReceiptType %>"></asp:Label>
            </h1>
            <asp:HiddenField ID="hdInstalmentNumber" runat="server" />
             <asp:HiddenField ID="hdnWriteOffReasonID" runat="server" />
            <asp:HiddenField ID="hdnMaxAmounttoWriteOff" runat="server" />
        </div>
           <div class="card-body clearfix">
            <asp:Panel ID="pnlCashListItem" runat="server" Visible="true" EnableViewState="true">
                <div class="md-whiteframe-z0 bg-white">
                    <ul class="nav nav-lines nav-tabs b-danger">
                        <li>
                            <a data-bs-toggle="tab" href="#tab-general" aria-expanded="true" class="active">
                                <asp:Literal ID="lbltab_general" runat="server" Text="<%$Resources:lbl_tab_general %>"></asp:Literal></a>
                        </li>
                        <li id="liPaymentTab" class="">
                            <a data-bs-toggle="tab" href="#tab-payment" aria-expanded="false">
                                <asp:Literal ID="lbltab_Payment" runat="server" Text="<%$Resources:lbl_tab_Payment %>"></asp:Literal></a>
                        </li>
                        <li class="">
                            <a data-bs-toggle="tab" href="#tab-furtherinfo" aria-expanded="false">
                                <asp:Literal ID="lbltab_furtherinfo" runat="server" Text="<%$Resources:lbl_tab_furtherinfo %>"></asp:Literal></a>
                        </li>
                        <li id="liInstalmentsTab" class="">
                            <a id="tabInstalments" data-bs-toggle="tab" href="#tab_Instalments" aria-expanded="false">
                                <asp:Literal ID="lbltab_Instalments" runat="server" Text="<%$Resources:lbl_tab_Instalments %>"></asp:Literal></a>
                        </li>
                        <li id="liCommentTab" style="display:none" runat="server">
                            <a data-bs-toggle="tab" href="#tab-Comment" aria-expanded="true">
                                <asp:Literal ID="lbltab_comment" runat="server" Text="<%$Resources:lbl_tab_comment %>"></asp:Literal></a>
                        </li>
                    </ul>
                    <div class="tab-content clearfix p b-t b-t-2x">
                        <div id="tab-general" class="tab-pane animated fadeIn active" role="tabpanel">
                            <asp:Panel runat="server" ID="pnlCashList">
                                <div class="form-horizontal">
                                    <div runat="server" id="liReceiptType" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:UpdatePanel ID="upReceiptType" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                                            <ContentTemplate>
                                                <asp:Panel ID="PnlReceiptType" runat="server">
                                                    <asp:Label ID="lblReceiptType" runat="server" AssociatedControlID="GISLookup_ReceiptType" class="col-md-4 col-sm-3 control-label">
                                                        <asp:Literal ID="ltReceiptType" runat="server" Text="<%$ Resources:lbl_ReceiptType %>"></asp:Literal></asp:Label>
                                                    <div class="col-md-8 col-sm-9">
                                                        <NexusProvider:LookupList ID="GISLookup_ReceiptType" AutoPostBack="true" runat="server" DataItemText="Description" DataItemValue="Code" ListCode="CashListItem_Receipt_Type" Sort="Asc" ListType="PMLookup" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                                                    </div>
                                                </asp:Panel>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="GISLookup_ReceiptType" EventName="SelectedIndexChange"></asp:AsyncPostBackTrigger>
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <Nexus:ProgressIndicator ID="piReceiptType" OverlayCssClass="updating" AssociatedUpdatePanelID="upReceiptType" runat="server">
                                            <progresstemplate>
                                                        </progresstemplate>
                                        </Nexus:ProgressIndicator>
                                    </div>
                                    <div id="liPaymentType" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Panel ID="Panel1" runat="server">
                                            <asp:Label ID="lblPaymentType" runat="server" AssociatedControlID="GISLookup_PaymentType" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltPaymentType" runat="server" Text="<%$ Resources:lbl_PaymentType %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                    <NexusProvider:LookupList ID="GISLookup_PaymentType" runat="server" DataItemText="Description" DataItemValue="Code" ListCode="CashListItem_Payment_Type" ListType="PMLookup" Sort="Asc" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                                                </div>
                                        </asp:Panel>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:UpdatePanel ID="upUpdateProdoc" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                        <ContentTemplate>
                                        <asp:Label runat="server" ID="lblProduceDocument" Text="<%$Resources:lbl_ProduceDocument %>" AssociatedControlID="chkProduceDocument" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <asp:CheckBox ID="chkProduceDocument" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                                            </ContentTemplate>
                                </asp:UpdatePanel>
                                    </div>
                                </div>
                            </asp:Panel>




                            <div class="form-horizontal">
                                <legend>
                                    <asp:Label ID="lblTransactionHeading" runat="server" Text="<%$Resources:lbl_TransactionHeading %>"></asp:Label>
                                </legend>

                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblTransactionDate" runat="server" AssociatedControlID="Cash_List_Item__Transaction_Date" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltTransactionDate" runat="server" Text="<%$ Resources:lbl_TransactionDate %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox ID="Cash_List_Item__Transaction_Date" runat="server" ReadOnly="true" CssClass="field-medium field-mandatory form-control"></asp:TextBox><uc2:CalendarLookup ID="TransactionDate_uctCalendarLookup" runat="server" LinkedControl="" HLevel="2"></uc2:CalendarLookup>
                                            </div>
                                        </div>

                                    <asp:RequiredFieldValidator ID="rqdTransactionDate" runat="server" CssClass="error" ControlToValidate="Cash_List_Item__Transaction_Date" ErrorMessage="<%$Resources:lbl_TransactionDateError %>" Display="none" SetFocusOnError="true" ValidationGroup="grpvalcashlistitem"></asp:RequiredFieldValidator>
                                    <asp:RangeValidator ID="rngTransactionDate" runat="server" CssClass="error" ControlToValidate="Cash_List_Item__Transaction_Date" ErrorMessage="<%$Resources:lbl_InvalidFormat %>" Type="Date" MinimumValue="01/01/1900" MaximumValue="12/12/9999" Display="none" SetFocusOnError="true" ValidationGroup="grpvalcashlistitem"></asp:RangeValidator>
                                </div>
                                <div id="liCollectionDate" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblCollectionDate" runat="server" AssociatedControlID="Cash_List_Item__Collection_Date" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltCollectionDate" runat="server" Text="<%$ Resources:lbl_CollectionDate %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                            <div class="input-group">
                                                <asp:TextBox ID="Cash_List_Item__Collection_Date" runat="server" CssClass="field-medium form-control" Enabled="false"></asp:TextBox><uc2:CalendarLookup ID="CollectionDate_CalenderLookup" runat="server" LinkedControl="Cash_List_Item__Collection_Date" Enabled="false" HLevel="1"></uc2:CalendarLookup>
                                            </div>
                                        </div>

                                    <asp:RequiredFieldValidator ID="rqdCollectionDate" runat="server" CssClass="error" ControlToValidate="Cash_List_Item__Collection_Date" ErrorMessage="<%$Resources:lbl_CollectionDateError %>" Display="none" Enabled="false" SetFocusOnError="true" ValidationGroup="grpvalcashlistitem"></asp:RequiredFieldValidator>
                                    <asp:RangeValidator ID="rngCollectionDate" runat="server" CssClass="error" ControlToValidate="Cash_List_Item__Collection_Date" ErrorMessage="<%$Resources:lbl_RangeInvalid %>" Type="Date" MinimumValue="01/01/1900" Display="none" Enabled="false" SetFocusOnError="true" ValidationGroup="grpvalcashlistitem"></asp:RangeValidator>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:UpdatePanel ID="updMediaType" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:Label ID="lblMediaType" runat="server" AssociatedControlID="GISLookup_MediaType" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltMediaType" runat="server" Text="<%$ Resources:lbl_MediaType %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                    <asp:DropDownList ID="GISLookup_MediaType" runat="server" CssClass="field-medium field-mandatory form-control form-select" AutoPostBack="true" OnSelectedIndexChanged="GISLookup_MediaType_SelectedIndexChange"></asp:DropDownList>
                                                </div>
                                            <asp:RequiredFieldValidator ID="rqdMediaType" runat="server" SetFocusOnError="true" CssClass="error" ControlToValidate="GISLookup_MediaType" Display="none" ErrorMessage="<%$Resources:lbl_MediaTypeError %>" ValidationGroup="grpvalcashlistitem"></asp:RequiredFieldValidator>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="GISLookup_MediaType" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <Nexus:ProgressIndicator ID="piMediaType" OverlayCssClass="updating" AssociatedUpdatePanelID="updMediaType" runat="server">
                                        <progresstemplate>
                                                        </progresstemplate>
                                    </Nexus:ProgressIndicator>
                                </div>
                                <div id="liComments" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblComments" runat="server" AssociatedControlID="txtComments" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltComments" runat="server" Text="<%$ Resources:lbl_Comments %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtComments" runat="server" CssClass="field-medium form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:UpdatePanel ID="updMediaRefrence" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:Label ID="lblMediaReference" runat="server" AssociatedControlID="txtMediaReference" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltMediaReference" runat="server" Text="<%$ Resources:lbl_MediaReference %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                    <asp:TextBox ID="txtMediaReference" runat="server" CssClass="field-medium form-control" MaxLength="30"></asp:TextBox>
                                                </div>
                                            <asp:RequiredFieldValidator ID="rqdMediaReference" runat="server" SetFocusOnError="true" CssClass="error" Enabled="false" ControlToValidate="txtMediaReference" Display="none" ErrorMessage="<%$Resources:lbl_MediaReferenceError %>" ValidationGroup="grpvalcashlistitem"></asp:RequiredFieldValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div id="liBankReference" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblBankReference" runat="server" AssociatedControlID="txtBankReference" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltBankReference" runat="server" Text="<%$ Resources:lbl_BankReference %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtBankReference" runat="server" CssClass="field-medium form-control"></asp:TextBox>
                                        </div>

                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblOurReference" runat="server" AssociatedControlID="txtOurReference" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltOurReference" runat="server" Text="<%$ Resources:lbl_OurReference %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtOurReference" runat="server" CssClass="field-medium form-control" MaxLength="30"></asp:TextBox>
                                        </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblTheirReference" runat="server" AssociatedControlID="txtTheirReference" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltTheirReference" runat="server" Text="<%$ Resources:lbl_TheirReference %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtTheirReference" runat="server" CssClass="field-medium form-control" MaxLength="30"></asp:TextBox>
                                        </div>
                                </div>

                            </div>



                            <div class="form-horizontal">
                                <legend>
                                    <asp:Label ID="lblPostingHeading" runat="server" Text="<%$Resources:lbl_PostingHeading %>"></asp:Label>
                                </legend>

                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:UpdatePanel ID="upAccount" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:Label ID="lblAccount" runat="server" AssociatedControlID="txtAccount" Text="<%$ Resources:lbl_Account %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                            <div class="col-md-8 col-sm-9">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtAccount" runat="server" CssClass="field-medium form-control" Enabled="false"></asp:TextBox>
                                                    <span class="input-group-btn">
                                                        <asp:LinkButton ID="btnAccount" SkinID="btnModal" runat="server" CausesValidation="false" Enabled="false"><i class="fa fa-search"></i><span class="btn-fnd-txt">Account</span></asp:LinkButton>
                                                    </span>
                                                </div>
                                            </div>
                                            <asp:RequiredFieldValidator ID="rqdAccount" runat="server" SetFocusOnError="true" CssClass="error" ControlToValidate="txtAccount" Display="none" ErrorMessage="<%$Resources:lbl_AccountError %>" ValidationGroup="grpvalcashlistitem"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="custValidAccount" runat="server" ControlToValidate="txtAccount" Display="None" SetFocusOnError="true" CssClass="error" ErrorMessage="<%$ Resources:lbl_ValidAccount %>" ValidationGroup="grpvalcashlistitem"></asp:CustomValidator>
                                            <asp:CustomValidator ID="custValidOCPAccount" runat="server" ControlToValidate="txtAccount" Display="None" SetFocusOnError="true" CssClass="error" ErrorMessage="<%$ Resources:lbl_ValidOCPAccount %>" ValidationGroup="grpvalcashlistitem"></asp:CustomValidator>
                                            <asp:HiddenField ID="hPartyKey" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="hiddenTempText" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="hAccountName" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="hiddenAccountKey" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="hdnIsInstalment" runat="server" Value="0"></asp:HiddenField>
                                            <asp:HiddenField ID="hdnAllInstalmentKey" runat="server" Value="0"></asp:HiddenField>
                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <Nexus:ProgressIndicator ID="piAccount" OverlayCssClass="updating" AssociatedUpdatePanelID="upAccount" runat="server">
                                        <progresstemplate>
                                                        </progresstemplate>
                                    </Nexus:ProgressIndicator>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblAllocationStatus" runat="server" AssociatedControlID="txtAllocationStatus" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:lbl_AllocationStatus %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtAllocationStatus" runat="server" Text="Unallocated" CssClass="field-medium form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                </div>

                            </div>




                            <div class="form-horizontal">
                                <legend>
                                    <asp:Label ID="lblAmountHeading" runat="server" Text="<%$Resources:lbl_AmountHeading %>"></asp:Label>
                                </legend>

                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltAmount" runat="server" Text="<%$ Resources:lbl_Amount %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="field-medium field-mandatory form-control" Text="0.00" MaxLength="15" onkeypress="javascript:return isInteger1(event);"></asp:TextBox>
                                        </div>
                                                <asp:RangeValidator ID="rvtxtAmount" runat="server" ControlToValidate="txtAmount"
                                                    MinimumValue="0.01" MaximumValue="99999999999999" Display="None" SetFocusOnError="true"
                                                    Type="Double" ErrorMessage="<%$Resources:lbl_NegativeAmountError %>" ValidationGroup="grpvalcashlistitem" />
                                    <asp:RequiredFieldValidator ID="rqdAmount" runat="server" SetFocusOnError="true" CssClass="error" ControlToValidate="txtAmount" Display="none" ErrorMessage="<%$Resources:lbl_AmountError %>" ValidationGroup="grpvalcashlistitem"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="custvltxtAmount" ControlToValidate="txtAmount" SetFocusOnError="true" ErrorMessage="<%$Resources:lbl_NegativeAmountError %>" Display="None" runat="server" OnServerValidate="custvltxtAmount_ServerValidate" ValidationGroup="grpvalcashlistitem"></asp:CustomValidator>
                                     <asp:CustomValidator ID="cstUserAuthoriseLimit" runat="server" SetFocusOnError="true" Display="none" CssClass="error"
                                                                    Enabled="false"  ErrorMessage="<%$ Resources:cstUserAuthoriseLimit %>" ValidationGroup="grpvalcashlistitem"></asp:CustomValidator>
                                </div>
                                <div id="liTenderedAmount" runat="server" visible="True" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblTendered" runat="server" AssociatedControlID="txtTendered" class="col-md-4 col-sm-3 control-label">

                                        <asp:Literal ID="ltTendered" runat="server" Text="<%$ Resources:lbl_Tendered %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtTendered" runat="server" CssClass="field-medium form-control" Text="0.00"></asp:TextBox>
                                        </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblWriteOffAmount" runat="server" AssociatedControlID="txtWriteOffAmount" Visible="false" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltWriteOffAmount" runat="server" Text="<%$ Resources:lbl_WriteOffAmount %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtWriteOffAmount" runat="server" CssClass="field-medium field-mandatory form-control" Text="0.00" MaxLength="15" Visible="false" Enabled="false"></asp:TextBox>
                                        </div>
                                </div>
                                <div id="liChangeAmount" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblChange" runat="server" AssociatedControlID="txtChange" class="col-md-4 col-sm-3 control-label">
                                        <asp:Literal ID="ltChange" runat="server" Text="<%$ Resources:lbl_Change %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                            <asp:TextBox ID="txtChange" runat="server" CssClass="field-medium form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblTotalAmount" runat="server" AssociatedControlID="txtTotalAmount" Text="<%$ Resources:lbl_Total %>" class="col-md-4 col-sm-3 control-label"></asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="field-medium field-mandatory form-control" Enabled="false" Text="0.00"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="reqTotal" runat="server" SetFocusOnError="true" CssClass="error" ControlToValidate="txtTotalAmount" Display="none" Enabled="false" ErrorMessage="<%$Resources:msg_TotalError %>" ValidationGroup="grpvalcashlistitem"></asp:RequiredFieldValidator>
                                    <asp:RangeValidator ID="rvtxtTotalAmount" runat="server" ControlToValidate="txtTotalAmount" MinimumValue="0.1" MaximumValue="99999999999999" Display="None" SetFocusOnError="true" Type="Double" ErrorMessage="<%$Resources:msg_NegativeTotalAmountError %>" ValidationGroup="grpvalcashlistitem"></asp:RangeValidator>
                                </div>

                            </div>


                            <asp:Panel ID="CashListItem_Receipt" runat="server">
                                <asp:UpdatePanel ID="upCashListItem_Receipt" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
                                    <ContentTemplate>
                                        <asp:Panel ID="CashListItem_Receipt_Cheque" runat="server" Visible="false">


                                            <div class="form-horizontal">
                                                <legend>
                                                    <asp:Label ID="lblChequeHeading" runat="server" Text="<%$ Resources:lbl_ChequeHeading %>"></asp:Label>
                                                </legend>

                                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblChequeHolderName" runat="server" AssociatedControlID="txtChequeHolderName" class="col-md-4 col-sm-3 control-label">
                                                        <asp:Literal ID="ltChequeHolderName" runat="server" Text="<%$ Resources:lbl_ChequeHolderName %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                            <asp:TextBox ID="txtChequeHolderName" runat="server" CssClass="field-medium field-mandatory form-control"></asp:TextBox>
                                                        </div>
                                                    <asp:RequiredFieldValidator ID="rqdChequeHolderName" runat="server" SetFocusOnError="true" CssClass="error" ControlToValidate="txtChequeHolderName" Display="none" Enabled="false" ErrorMessage="<%$Resources:lbl_ChequeHolderNameError %>" ValidationGroup="grpvalcashlistitem"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblBankList" runat="server" AssociatedControlID="GISLookup_BankList" class="col-md-4 col-sm-3 control-label">
                                                        <asp:Literal ID="ltBankList" runat="server" Text="<%$ Resources:lbl_BankList %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                            <NexusProvider:LookupList ID="GISLookup_BankList" runat="server" DataItemText="Description" DataItemValue="Code" DefaultText="<%$ Resources:lbl_DefaultText %>" ListCode="CashListItem_Bank" ListType="PMLookup" CssClass="field-medium field-mandatory form-control form-select" Sort="Asc"></NexusProvider:LookupList>
                                                                <asp:RequiredFieldValidator ID="rqdBankLisk" runat="server" SetFocusOnError="true"
                                                                    CssClass="error" ControlToValidate="GISLookup_BankList" Display="none" Enabled="false"
                                                                    ErrorMessage="<%$Resources:lbl_BankListError %>" InitialValue="" ValidationGroup="grpvalcashlistitem"/>
                                                        </div>
                                                   
                                                </div>
                                                <div id="InstrumentNumberQM" visible="false" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblInstrumentNumber" runat="server" AssociatedControlID="Cash_List_Item__InstrumentNumber" Text="<%$ Resources:lbl_InstrumentNumber%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                                    <div class="col-md-8 col-sm-9">
                                                        <asp:TextBox ID="Cash_List_Item__InstrumentNumber" CssClass="field-medium field-mandatory form-control" runat="server"></asp:TextBox>
                                                    </div>
                                                    <asp:RequiredFieldValidator ID="rqdInstrumentNumber" ControlToValidate="Cash_List_Item__InstrumentNumber" runat="server" SetFocusOnError="true" Enabled="false" CssClass="error" Display="none" ErrorMessage="<%$Resources:lblInstrumentNumberError %>" ValidationGroup="grpvalcashlistitem">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblChequeDate" runat="server" AssociatedControlID="Cash_List_Item__Cheque_Date" Text="<%$ Resources:lbl_ChequeDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                                    <div class="col-md-8 col-sm-9">
                                                        <div class="input-group">
                                                            <asp:TextBox ID="Cash_List_Item__Cheque_Date" runat="server" CssClass="field-medium field-mandatory form-control"></asp:TextBox><uc2:CalendarLookup ID="ChequeDate_CalendarLookup" runat="server" LinkedControl="Cash_List_Item__Cheque_Date" HLevel="6"></uc2:CalendarLookup>
                                                        </div>
                                                    </div>

                                                    <asp:RequiredFieldValidator ID="rqdChequeDate" runat="server" CssClass="error" ControlToValidate="Cash_List_Item__Cheque_Date" SetFocusOnError="true" ErrorMessage="<%$Resources:lbl_ChequeDateError %>" Display="none" Enabled="false" ValidationGroup="grpvalcashlistitem"></asp:RequiredFieldValidator>
                                                    <asp:RangeValidator ID="rngChequeDate" runat="server" CssClass="error" ControlToValidate="Cash_List_Item__Cheque_Date" SetFocusOnError="true" ErrorMessage="<%$Resources:lbl_InvalidChequeDate %>" Type="Date" Display="none" Enabled="false" ValidationGroup="grpvalcashlistitem"></asp:RangeValidator>
                                                    <asp:CustomValidator ID="custvldChequeDate" runat="server" CssClass="error" SetFocusOnError="true" Display="None" Enabled="false" ClientValidationFunction="WarningMsg" ValidationGroup="grpvalcashlistitem"></asp:CustomValidator>
                                                    <asp:CustomValidator ID="CustChkFutureChequeDate" runat="server" SetFocusOnError="true" Display="none" ErrorMessage="<%$ Resources:errmsg_ChequeFutureDate %>" Enabled="false" ClientValidationFunction="CheckFutureDate" ValidationGroup="grpvalcashlistitem"></asp:CustomValidator>
                                                </div>
                                                <div id="BankLocationQM" visible="false" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblBankLocation" runat="server" AssociatedControlID="txtBankLocation" Text="<%$ Resources:lbl_BankLocation %>" class="col-md-4 col-sm-3 control-label"></asp:Label><div class="col-md-8 col-sm-9">
                                                        <asp:TextBox ID="txtBankLocation" runat="server" CssClass="field-medium field-mandatory form-control"></asp:TextBox>
                                                    </div>
                                                    <asp:RequiredFieldValidator ID="rqtxtBankLocation" runat="server" SetFocusOnError="true" CssClass="error" ControlToValidate="txtBankLocation" Display="none" Enabled="false" ErrorMessage="<%$Resources:lbl_BankLocationError %>" ValidationGroup="grpvalcashlistitem"></asp:RequiredFieldValidator>
                                                </div>
                                                <div id="ChequeTypeQM" visible="false" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblChequeType" runat="server" AssociatedControlID="GISLookup_ChequeType" Text="<%$ Resources:lbl_ChequeType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                                    <div class="col-md-8 col-sm-9">
                                                        <NexusProvider:LookupList ID="GISLookup_ChequeType" runat="server" DataItemText="Description" DataItemValue="Code" DefaultText="<%$ Resources:lbl_DefaultText %>" ListCode="ChequeType" Sort="Asc" ListType="PMLookup" CssClass="field-medium field-mandatory form-control form-select"></NexusProvider:LookupList>
                                                    </div>
                                                    <asp:RequiredFieldValidator ID="rqChequeType" runat="server" SetFocusOnError="true" CssClass="error" ControlToValidate="GISLookup_ChequeType" Display="none" Enabled="false" ErrorMessage="<%$Resources:lbl_ChequeTypeError %>" InitialValue=" " ValidationGroup="grpvalcashlistitem"></asp:RequiredFieldValidator>
                                                </div>
                                                <div id="BankBranchQM" visible="false" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblBankBranch" runat="server" AssociatedControlID="txtBankBranch" Text="<%$ Resources:lbl_BankBranch %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                                    <div class="col-md-8 col-sm-9">
                                                        <asp:TextBox ID="txtBankBranch" runat="server" CssClass="field-medium field-mandatory form-control"></asp:TextBox>
                                                    </div>
                                                    <asp:RequiredFieldValidator ID="rqBankBranch" runat="server" SetFocusOnError="true" CssClass="error" ControlToValidate="txtBankBranch" Display="none" Enabled="false" ErrorMessage="<%$Resources:lbl_BankBranchError %>" ValidationGroup="grpvalcashlistitem"></asp:RequiredFieldValidator>
                                                </div>
                                                <div id="ChequeClearingTypeQM" visible="false" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblChequeClearingType" runat="server" AssociatedControlID="GISLookup_ChequeClearingTypeList" Text="<%$ Resources:lbl_ChequeClearingType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                                    <div class="col-md-8 col-sm-9">
                                                        <NexusProvider:LookupList ID="GISLookup_ChequeClearingTypeList" runat="server" DataItemText="Description" DataItemValue="Code" DefaultText="<%$ Resources:lbl_DefaultText %>" ListCode="Cheque_Clearing_Type" Sort="Asc" ListType="PMLookup" CssClass="field-medium field-mandatory form-control form-select"></NexusProvider:LookupList>
                                                    </div>
                                                    <asp:RequiredFieldValidator ID="rqlChequeClearingType" runat="server" SetFocusOnError="true" CssClass="error" ControlToValidate="GISLookup_ChequeClearingTypeList" Display="none" Enabled="false" ErrorMessage="<%$Resources:lbl_ChequeClearingTypeListtError %>" InitialValue=" " ValidationGroup="grpvalcashlistitem"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="CashListItem_Receipt_CC" runat="server" Visible="false">
                                            <div class="form-horizontal">
                                                <legend>
                                                    <asp:Label ID="lblCCHeading" runat="server" Text="<%$ Resources:lbl_CCHeading %>"></asp:Label>
                                                </legend>

                                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblCardNumber" runat="server" AssociatedControlID="txtCardNumber" class="col-md-4 col-sm-3 control-label">
                                                        <asp:Literal ID="ltCardNumber" runat="server" Text="<%$ Resources:lbl_CardNumber %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                            <asp:TextBox ID="txtCardNumber" runat="server" CssClass="field-medium form-control" MaxLength="16" onkeypress="javascript:return isInteger(event);"></asp:TextBox>
                                                        </div>
                                                    <asp:RequiredFieldValidator ID="rqdCardNumber" runat="server" SetFocusOnError="true" CssClass="error" ControlToValidate="txtCardNumber" Display="none" Enabled="false" ErrorMessage="<%$Resources:lbl_CardNumberError %>" ValidationGroup="grpvalcashlistitem"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="rqdRangeCardNumber" Display="none" Enabled="false" CssClass="error" ControlToValidate="txtCardNumber" runat="server" SetFocusOnError="true" ErrorMessage="<%$Resources:lbl_CardNumberError %>" ValidationExpression="\d{2}" ValidationGroup="grpvalcashlistitem"></asp:RegularExpressionValidator>
                                                </div>
                                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblNameOnCard" runat="server" AssociatedControlID="txtNameOnCard" class="col-md-4 col-sm-3 control-label">
                                                        <asp:Literal ID="ltNameOnCard" runat="server" Text="<%$ Resources:lbl_NameOnCard %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                            <asp:TextBox ID="txtNameOnCard" runat="server" MaxLength="50" CssClass="field-medium form-control"></asp:TextBox>
                                                        </div>
                                                    <asp:RequiredFieldValidator ID="rqdNameOnCard" runat="server" SetFocusOnError="true" CssClass="error" ControlToValidate="txtNameOnCard" Display="none" Enabled="false" ErrorMessage="<%$Resources:lbl_NameOnCardError %>" ValidationGroup="grpvalcashlistitem"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblExpiryDate" runat="server" AssociatedControlID="Cash_List_Item__Expiry_Date" class="col-md-4 col-sm-3 control-label">
                                                        <asp:Literal ID="ltExpiryDate" runat="server" Text="<%$ Resources:lbl_ExpiryDate %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                            <asp:TextBox ID="Cash_List_Item__Expiry_Date" Columns="5" runat="server" CssClass="field-medium form-control"></asp:TextBox>
                                                        </div>
                                                    <asp:RequiredFieldValidator ID="rqdExpiryDate" runat="server" CssClass="error" ControlToValidate="Cash_List_Item__Expiry_Date" ErrorMessage="<%$Resources:lbl_ExpiryDateError %>" Display="none" Enabled="false" SetFocusOnError="true" ValidationGroup="grpvalcashlistitem"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="revExpiryDate" runat="server" CssClass="error" ControlToValidate="Cash_List_Item__Expiry_Date" ErrorMessage="<%$Resources:lbl_ExpiryDateCustomError %>" ValidationExpression="\d{2}/\d{2}" SetFocusOnError="true" Display="none" Enabled="false" ValidationGroup="grpvalcashlistitem"></asp:RegularExpressionValidator>
                                                </div>
                                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblStartDate" runat="server" AssociatedControlID="Cash_List_Item__Start_Date" class="col-md-4 col-sm-3 control-label">
                                                        <asp:Literal ID="ltStartDate" runat="server" Text="<%$ Resources:lbl_StartDate %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                            <asp:TextBox ID="Cash_List_Item__Start_Date" Columns="5" runat="server" CssClass="field-medium form-control"></asp:TextBox>
                                                        </div>
                                                    <asp:RegularExpressionValidator ID="revStartDate" runat="server" CssClass="error" ControlToValidate="Cash_List_Item__Start_Date" ErrorMessage="<%$Resources:lbl_StartDateCustomError %>" ValidationExpression="\d{2}/\d{2}" SetFocusOnError="true" Display="none" Enabled="false" ValidationGroup="grpvalcashlistitem"></asp:RegularExpressionValidator>
                                                    <asp:CustomValidator ID="CstVldCCDate" runat="server" SetFocusOnError="true" Display="none" Enabled="false" ErrorMessage="<%$ Resources:lbl_InvalidFormat %>" ValidationGroup="grpvalcashlistitem"></asp:CustomValidator>
                                                </div>
                                                <div id="IssueNumberQM" visible="false" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblIssueNumber" AssociatedControlID="txtIssueNumber" runat="server" Text="<%$ Resources:lbl_IssueNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label><div class="col-md-8 col-sm-9">
                                                        <asp:TextBox ID="txtIssueNumber" Columns="5" runat="server" CssClass="field-medium form-control" MaxLength="2" onkeypress="javascript:return isInteger(event);"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div id="PinQM" visible="false" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblPin" AssociatedControlID="txtPin" runat="server" Text="<%$ Resources:lbl_Pin %>" class="col-md-4 col-sm-3 control-label"></asp:Label><div class="col-md-8 col-sm-9">
                                                        <asp:TextBox ID="txtPin" Columns="5" runat="server" CssClass="field-medium form-control" MaxLength="20" onkeypress="javascript:return isInteger(event);"></asp:TextBox>
                                                    </div>
                                                    <asp:CustomValidator ID="cstVldCVV" runat="server" SetFocusOnError="true" Display="none" Enabled="false" ErrorMessage="<%$ Resources:lbl_CVV_Err %>" ValidationGroup="grpvalcashlistitem"></asp:CustomValidator>
                                                </div>
                                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblAuthCode" runat="server" AssociatedControlID="txtAuthCode" Text="<%$ Resources:lbl_AuthCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label><div class="col-md-8 col-sm-9">
                                                        <asp:TextBox ID="txtAuthCode" ReadOnly="true" runat="server" CssClass="field-medium form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblManualAuth" runat="server" AssociatedControlID="txtManualAuth" class="col-md-4 col-sm-3 control-label">
                                                        <asp:Literal ID="ltManualAuth" runat="server" Text="<%$ Resources:lbl_ManualAuth %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                            <asp:TextBox ID="txtManualAuth" runat="server" CssClass="field-medium form-control"></asp:TextBox>
                                                        </div>
                                                </div>
                                                <div id="CustomerQM" visible="false" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblCustomer" runat="server" AssociatedControlID="ddlCustomer" class="col-md-4 col-sm-3 control-label">
                                                        <asp:Literal ID="ltCustomer" runat="server" Text="<%$ Resources:lbl_CustomerList %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                            <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control form-select">
                                                                <asp:ListItem Text="<%$ Resources:lbl_ddlCustomer_Present_Text %>" Value="<%$ Resources:lbl_ddlCustomer_Present_Value %>"></asp:ListItem>
                                                                <asp:ListItem Text="<%$ Resources:lbl_ddlCustomer_NotPresent_Text %>" Value="<%$ Resources:lbl_ddlCustomer_NotPresent_Value %>"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    <asp:RequiredFieldValidator ID="rqdCustomer" runat="server" SetFocusOnError="true" CssClass="error" ControlToValidate="ddlCustomer" Display="none" Enabled="false" ErrorMessage="<%$Resources:lbl_CustomerError %>" ValidationGroup="grpvalcashlistitem"></asp:RequiredFieldValidator>
                                                </div>
                                                <div id="TypeofCardQM" visible="false" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblTypeofCard" runat="server" AssociatedControlID="GISLookup_TypeofCard" Text="<%$ Resources:lbl_TypeofCard %>" class="col-md-4 col-sm-3 control-label"></asp:Label><div class="col-md-8 col-sm-9">
                                                        <NexusProvider:LookupList ID="GISLookup_TypeofCard" runat="server" DataItemText="Description" DataItemValue="Code" DefaultText="<%$ Resources:lbl_DefaultText %>" ListCode="Type_of_Card" Sort="Asc" ListType="PMLookup" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                                                    </div>
                                                    <asp:RequiredFieldValidator ID="rqTypeofCard" runat="server" SetFocusOnError="true" CssClass="error" ControlToValidate="GISLookup_TypeofCard" Display="none" Enabled="false" ErrorMessage="<%$Resources:lbl_TypeofCardError %>" InitialValue=" " ValidationGroup="grpvalcashlistitem"></asp:RequiredFieldValidator>
                                                </div>
                                                <div id="IssuingBankQM" visible="false" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblIssuingBank" runat="server" AssociatedControlID="GISLookup_IssuingBank" Text="<%$ Resources:lbl_IssuingBank %>" class="col-md-4 col-sm-3 control-label"></asp:Label><div class="col-md-8 col-sm-9">
                                                        <NexusProvider:LookupList ID="GISLookup_IssuingBank" runat="server" DataItemText="Description" DataItemValue="Code" DefaultText="<%$ Resources:lbl_DefaultText %>" ListCode="CashListItem_Bank" Sort="Asc" ListType="PMLookup" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                                                    </div>
                                                    <asp:RequiredFieldValidator ID="rqIssuingBank" runat="server" SetFocusOnError="true" CssClass="error" ControlToValidate="GISLookup_TypeofCard" Display="none" Enabled="false" ErrorMessage="<%$Resources:lbl_IssuingBankError %>" InitialValue=" " ValidationGroup="grpvalcashlistitem"></asp:RequiredFieldValidator>
                                                </div>
                                                <div id="SlipQM" visible="false" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblSlip" runat="server" AssociatedControlID="txtTransactionSlip" Text="<%$ Resources:lbl_TransactionSlip %>" class="col-md-4 col-sm-3 control-label"></asp:Label><div class="col-md-8 col-sm-9">
                                                        <asp:TextBox ID="txtTransactionSlip" runat="server" CssClass="field-medium form-control"></asp:TextBox>
                                                    </div>
                                                    <asp:RequiredFieldValidator ID="rqSlip" runat="server" SetFocusOnError="true" CssClass="error" ControlToValidate="txtTransactionSlip" Display="none" Enabled="false" ErrorMessage="<%$Resources:lbl_TransactionSlipError %>" ValidationGroup="grpvalcashlistitem"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <Nexus:ProgressIndicator ID="piCashListItem_Receipt" OverlayCssClass="updating" AssociatedUpdatePanelID="upCashListItem_Receipt" runat="server">
                                    <progresstemplate>
                                            </progresstemplate>
                                </Nexus:ProgressIndicator>
                            </asp:Panel>
                            <asp:Panel ID="PnlPayeeInformation" runat="server">
                                <div class="form-horizontal">
                                    <legend>
                                        <asp:Label ID="lblPaymentInformation" runat="server" Text="<%$Resources:lbl_PaymentInformation %>"></asp:Label>
                                    </legend>
                                    <div id="liStatus" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblStatus" runat="server" AssociatedControlID="ddlStatus" class="col-md-4 col-sm-3 control-label">
                                            <asp:Literal ID="ltStatus" runat="server" Text="<%$ Resources:lt_Status %>"></asp:Literal></asp:Label>
                                        <div class="col-md-8 col-sm-9">
                                            <NexusProvider:LookupList ID="ddlStatus" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="cashlistitem_payment_status" DefaultText="(Please Select)" CssClass="field-medium form-control form-select" Enabled="false"></NexusProvider:LookupList>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:UpdatePanel ID="upBGDebtDetails" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlBGDebtDetails" runat="server" Visible="false">
                                        <legend>
                                            <asp:Literal ID="ltBG" runat="server" Text="<%$Resources:lbl_BGHeading %>"></asp:Literal>
                                        </legend>
                                        <div class="grid-card table-responsive">
                                            <asp:GridView ID="grdvBGDebtDetails" runat="server" AutoGenerateColumns="False" DataKeyNames="BGKey" GridLines="None" AllowPaging="True" PagerSettings-Mode="Numeric" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                                <Columns>
                                                    <asp:BoundField HeaderText="<%$ Resources:lbl_BankGuaranteeRef %>" DataField="BankGuaranteeRef"></asp:BoundField>
                                                    <asp:BoundField HeaderText="<%$ Resources:lbl_BGDueDate %>" DataField="BGDueDate"></asp:BoundField>
                                                    <asp:BoundField HeaderText="<%$ Resources:lbl_PolicyRef %>" DataField="PolicyRef"></asp:BoundField>
                                                    <asp:BoundField HeaderText="<%$ Resources:lbl_PremiumAmount %>" DataField="PremiumAmount"></asp:BoundField>
                                                    <asp:BoundField HeaderText="<%$ Resources:lbl_OutstandingPolicyAmt %>" DataField="OutstandingPolicyAmt"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="SelectAll">
                                                        <ItemTemplate>
                                                            <asp:UpdatePanel ID="upAmtSelect" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                                                                <ContentTemplate>
                                                                    <asp:CheckBox ID="chkAmtSelect" runat="server" AutoPostBack="True" OnCheckedChanged="chkAmtSelect_CheckedChanged" Text=" " CssClass="asp-check"></asp:CheckBox>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="chkAmtSelect" EventName="CheckedChanged"></asp:AsyncPostBackTrigger>
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                            <Nexus:ProgressIndicator ID="piupAmtSelect" OverlayCssClass="updating" AssociatedUpdatePanelID="upAmtSelect" runat="server">
                                                                <progresstemplate>
                                                                                </progresstemplate>
                                                            </Nexus:ProgressIndicator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="grdvBGDebtDetails" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="grdvBGDebtDetails" EventName="Load"></asp:AsyncPostBackTrigger>
                                </Triggers>
                            </asp:UpdatePanel>
                            <Nexus:ProgressIndicator ID="updBGDebtDetail" OverlayCssClass="updating" AssociatedUpdatePanelID="upBGDebtDetails" runat="server">
                                <progresstemplate>
                                        </progresstemplate>
                            </Nexus:ProgressIndicator>
                        </div>
                        <div id="tab-payment" class="tab-pane animated fadeIn" role="tabpanel">
                            <asp:Panel ID="pnlPayeeInfo" runat="server" Visible="true">
                                <asp:UpdatePanel ID="UpdpnlPayee" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                                    <ContentTemplate>
                                        <div class="form-horizontal">
                                            <div id="liAccountType" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                <asp:Label ID="lblAccountType" runat="server" AssociatedControlID="ddlAccountType" Text="Account Type" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                                <div class="col-md-8 col-sm-9">
                                                    <asp:DropDownList ID="ddlAccountType" AutoPostBack="true" runat="server" CssClass="field-medium form-control form-select"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                <asp:Label ID="lblPayeeName" runat="server" AssociatedControlID="txtPayeeName" class="col-md-4 col-sm-3 control-label">
                                                    <asp:Literal ID="ltPayeeName" runat="server" Text="<%$ Resources:lbl_PayeeName%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                        <asp:TextBox ID="txtPayeeName" runat="server" CssClass="field-medium form-control"></asp:TextBox>
                                                    </div>
                                            </div>
                                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                <asp:Label ID="lblAccountCode" runat="server" AssociatedControlID="txtAccountCode" class="col-md-4 col-sm-3 control-label">
                                                    <asp:Literal ID="ltAccountCode" runat="server" Text="<%$ Resources:lbl_AccountCode%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                        <asp:TextBox ID="txtAccountCode" runat="server" CssClass="field-medium form-control"></asp:TextBox>
                                                    </div>
                                            </div>
                                            <div class="AlternateListItem form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                <asp:Label ID="lblExpiryDateForPayment" runat="server" AssociatedControlID="txtExpiryDate" class="col-md-4 col-sm-3 control-label">
                                                    <asp:Literal ID="ltExpiryDateForPayment" runat="server" Text="<%$ Resources:lbl_ExpiryDate%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                        <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="field-medium form-control"></asp:TextBox>
                                                    </div>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="error"
                                                                ControlToValidate="txtExpiryDate" ErrorMessage="<%$Resources:lbl_ExpiryDateCustomError %>"
                                                                ValidationExpression="\d{2}/\d{2}" SetFocusOnError="true" Display="none" Enabled="true" ValidationGroup="grpvalcashlistitem"/>
                                                <asp:CustomValidator ID="custvldExpiryDateForPayment" runat="server" SetFocusOnError="true" Display="none" ErrorMessage="<%$ Resources:lbl_InvalidFormat %>" ValidationGroup="grpvalcashlistitem"></asp:CustomValidator>
                                            </div>
                                          
                                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                <asp:Label ID="lblBranchCode" runat="server" AssociatedControlID="txtBranchCode" class="col-md-4 col-sm-3 control-label">
                                                    <asp:Literal ID="ltBranchCode" runat="server" Text="<%$ Resources:lbl_BranchCode%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                        <asp:TextBox ID="txtBranchCode" runat="server" CssClass="field-medium form-control"></asp:TextBox>
                                                    </div>
                                            </div>
                                            <div class="AlternateListItem form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                <asp:Label ID="lblReference1" runat="server" AssociatedControlID="txtReference1" class="col-md-4 col-sm-3 control-label">
                                                    <asp:Literal ID="ltReference1" runat="server" Text="<%$ Resources:lbl_Reference1%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                        <asp:TextBox ID="txtReference1" runat="server" CssClass="field-medium form-control"></asp:TextBox>
                                                    </div>
                                            </div>
                                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                <asp:Label ID="lblReference2" runat="server" AssociatedControlID="txtReference2" class="col-md-4 col-sm-3 control-label">
                                                    <asp:Literal ID="ltReference2" runat="server" Text="<%$ Resources:lbl_Reference2%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                        <asp:TextBox ID="txtReference2" runat="server" CssClass="field-medium form-control"></asp:TextBox>
                                                    </div>
                                            </div>

                                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                <asp:Label ID="lblBIC" runat="server" AssociatedControlID="txtBIC" Text="<%$ Resources:lbl_BIC %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                                <div class="col-md-8 col-sm-9">
                                                    <asp:TextBox ID="txtBIC" runat="server" CssClass="field-medium form-control" MaxLength="50" onkeypress="javascript:return isAlphaNumeric(event);"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                <asp:Label ID="lblIBAN" runat="server" AssociatedControlID="txtIBAN" Text="<%$ Resources:lbl_IBAN %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                                <div class="col-md-8 col-sm-9">
                                                    <asp:TextBox ID="txtIBAN" runat="server" CssClass="field-medium form-control" MaxLength="50" onkeypress="javascript:return isAlphaNumeric(event);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                          <asp:HiddenField ID="hidBankCode" runat="server"></asp:HiddenField>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlAccountType" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                                    </Triggers>
                                </asp:UpdatePanel>
                                <Nexus:ProgressIndicator ID="piPayee" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdpnlPayee" runat="server">
                                    <progresstemplate>
                                            </progresstemplate>
                                </Nexus:ProgressIndicator>
                            </asp:Panel>
                            <asp:Panel ID="pnlBankInfo" runat="server" Visible="false">
                                <div class="form-horizontal">
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblDatePresented" runat="server" AssociatedControlID="txtDatePresented" class="col-md-4 col-sm-3 control-label">
                                            <asp:Literal ID="ltDatePresented" runat="server" Text="<%$ Resources:lbl_DatePresented%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtDatePresented" runat="server" CssClass="field-medium form-control"></asp:TextBox>
                                            </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:CheckBox ID="chkInPossession" runat="server" Text="<%$ Resources:lbl_InPossession%>" CssClass="asp-check"></asp:CheckBox>
                                    </div>
                                    <div class="AlternateListItem form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblStopRequested" runat="server" AssociatedControlID="txtStopRequested" class="col-md-4 col-sm-3 control-label">
                                            <asp:Literal ID="ltStopRequested" runat="server" Text="<%$ Resources:lbl_StopRequested%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtStopRequested" runat="server" CssClass="field-medium form-control"></asp:TextBox>
                                            </div>
                                    </div>
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblConfirmation" runat="server" AssociatedControlID="txtConfirmation" class="col-md-4 col-sm-3 control-label">
                                            <asp:Literal ID="ltConfirmation" runat="server" Text="<%$ Resources:lbl_Confirmation%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtConfirmation" runat="server" CssClass="field-medium form-control"></asp:TextBox>
                                            </div>
                                    </div>
                                    <div class="AlternateListItem form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblReason" runat="server" AssociatedControlID="txtReason" class="col-md-4 col-sm-3 control-label">
                                            <asp:Literal ID="ltReason" runat="server" Text="<%$ Resources:lbl_Reason%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtReason" runat="server" CssClass="textarea form-control" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                        <div id="tab-furtherinfo" class="tab-pane animated fadeIn" role="tabpanel">
                            <asp:Panel ID="pnlAddress" runat="server">
                                <asp:UpdatePanel ID="UpdateAddress" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                                    <ContentTemplate>
                                        <div class="form-horizontal">
                                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" class="col-md-4 col-sm-3 control-label">
                                                    <asp:Literal ID="ltName" runat="server" Text="<%$ Resources:lbl_Name %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                        <asp:TextBox ID="txtName" runat="server" CssClass="field-medium form-control"></asp:TextBox>
                                                    </div>
                                            </div>
                                            <uc1:AddressCntrl ID="PayNow_Address" runat="server"></uc1:AddressCntrl>
                                            <div class="form-horizontal">

                                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                    <asp:Label ID="lblDetails" runat="server" AssociatedControlID="txtDetails" class="col-md-4 col-sm-3 control-label">
                                                        <asp:Literal ID="ltDetails" runat="server" Text="<%$ Resources:lbl_Details %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                            <asp:TextBox ID="txtDetails" runat="server" TextMode="MultiLine" CssClass="field-medium form-control"></asp:TextBox>
                                                        </div>
                                                </div>

                                            </div>
                                        </div>
                                        <asp:HiddenField ID="hIsGrossClaimPaymentAmount" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="hClaimsIsPostTaxes" runat="server"></asp:HiddenField>
                                         <asp:HiddenField ID="hidHasPaymentsAuthority" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <Nexus:ProgressIndicator ID="upAddress" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdateAddress" runat="server">
                                    <progresstemplate>
                                                </progresstemplate>
                                </Nexus:ProgressIndicator>
                            </asp:Panel>
                        </div>
                         <div id="tab_Instalments" class="tab-pane animated fadeIn" role="tabpanel">
                            
                            <asp:UpdatePanel ID="UpdInstalments" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="PnlSTInstalments" runat="server" >
                                        <asp:HiddenField ID="hdnIsInstalmentsFound" runat="server"></asp:HiddenField>
                                        <div class="form-horizontal">
  
                                            <div id="liInstalmentPlan" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                                <asp:Label ID="Label3" runat="server" AssociatedControlID="ddlInstalmentPlan" class="col-md-4 col-sm-3 control-label">
                                                    <asp:Literal ID="litInstalmentPlan" runat="server" Text="<%$ Resources:lbl_InstalmentPlan %>"></asp:Literal>
                                                </asp:Label><div class="col-md-8 col-sm-9">
                                                  
                                                      <asp:DropDownList ID="ddlInstalmentPlan" runat="server" CssClass="field-medium field-mandatory form-control form-select" AutoPostBack="true" 
                                                          OnSelectedIndexChanged="ddlInstalmentPlan_SelectedIndexChanged" ></asp:DropDownList>

                                                </div>
                                            </div>
                                        </div>
                                        <div class="grid-card table-responsive">
                                            <div class="grid-card table-responsive no-margin">
                                                <asp:GridView ID="grdInstallmentQuotes" runat="server" AutoGenerateColumns="false" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="No Data found" DataKeyNames="PFInstalmentsKey,BatchRef,Commission,ExportDate,Fee,InstalmentNumber,InstalmentReasonCode,PaymentDate,PFTransactionKey,PostedDate,Reason,Status,StatusCode,StatusDescription,TransactionDescription,CurrencyDesc" >
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$ Resources:grdinst_Selected %>" ItemStyle-CssClass="GridItemStyletoCenter">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelectedInstalment" runat="server" Text=" " CssClass="asp-check" onclick="TotalCurrentPlanSelectedInstalment(this.id);"  />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="InstalmentNumber" HeaderText="<%$ Resources:grdinst_InstalmentNo %>"></asp:BoundField>
                                                        <asp:BoundField DataField="DueDate" HeaderText="<%$ Resources:grdinst_Duedate %>" DataFormatString="{0:d}" ></asp:BoundField>
                                                        <asp:BoundField DataField="PostedDate" HeaderText="<%$ Resources:grdinst_PaymentDate %>" DataFormatString="{0:d}"  ></asp:BoundField>
                                                        <Nexus:BoundField DataField="Amount" HeaderText="<%$ Resources:grdinst_Amount %>" DataType="Currency" ></Nexus:BoundField>
                                                         <asp:BoundField DataField="CurrencyDesc" HeaderText="" ></asp:BoundField>
                                                        
                                                        
                                                        <%--<asp:BoundField DataField="PFInstalmentsKey" HeaderText="PFInstalmentsKey" Visible="False"></asp:BoundField>--%>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                            <strong><span id="msg_noinstalment" runat="server"></span></strong>
                                            <asp:HiddenField ID="PlanStatus" runat="server"></asp:HiddenField>
                                        </div>
                                        <div class="form-inline m-t-md">
                                            <div class="form-group form-group-sm">
                                                <asp:Label ID="lblCurrentPlanSelectedTotal" runat="server" AssociatedControlID="txtCurrentPlanSelectedTotal" Text="<%$ Resources:lbl_lblSelectedTotal %>" />
                                                <asp:TextBox ID="txtCurrentPlanSelectedTotal" runat="server" Enabled="false" CssClass="form-control text-right m-r-md" Text="0.00" />
                                            </div>
                                             <div class="form-group form-group-sm">
                                                <asp:Label ID="lblOverAllSelectedTotal" runat="server" AssociatedControlID="txtOverAllSelectedTotal" Text="<%$ Resources:lbl_lblSOverallTotal %>" />
                                                <asp:TextBox ID="txtOverAllSelectedTotal" runat="server" Enabled="false" CssClass="form-control text-right m-r-md" Text="0.00" />
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                             
                            </asp:UpdatePanel>

                            <nexus:ProgressIndicator ID="UpAccDetails" OverlayCssClass="updating" AssociatedUpdatePanelID="" runat="server">
                                <progresstemplate>
                                    </progresstemplate>
                                
                            </nexus:ProgressIndicator>
                        </div>


                        <div id="tab-Comment" class="tab-pane animated fadeIn" role="tabpanel">
                            <asp:Panel ID="PnlComment" runat="server" Visible="true">
                                <asp:UpdatePanel ID="UpdComment" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                                    <ContentTemplate>
                                        <div class="form-horizontal">
                                            <div class="row">
                                                <div class="col-3">
                                                    <asp:Label ID="lbl_Commentstxt" Style="margin-left: 15px; margin-top: 40px;" runat="server" AssociatedControlID="txtPrevComments" Text="<%$ Resources:lblComments%>" class="col-md-3 col-sm-4 control-label"></asp:Label>
                                                </div>
                                                <div class="col-8">
                                                    <asp:TextBox runat="server" Disabled="true" TextMode="MultiLine" Style="margin-top: 20px; margin-bottom: 15px; height: 100px;" CssClass="form-control" ID="txtPrevComments">
                                                    </asp:TextBox>

                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <Nexus:ProgressIndicator ID="PIComment" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdpnlPayee" runat="server">
                                    <progresstemplate>
                                            </progresstemplate>
                                </Nexus:ProgressIndicator>
                            </asp:Panel>
                            
                        </div>



                        <div class="form-inline m-t-md">
                            <div class="form-group form-group-sm">
                                <%--<asp:Label ID="lblSelectedTotal" runat="server" AssociatedControlID="txtSelectedTotal" Text="<%$ Resources:lbl_lblSelectedTotal %>" />--%>
                                <%--<asp:TextBox ID="txtSelectedTotal" runat="server" Enabled="false" TextMode="Number" CssClass="form-control text-right m-r-md" />--%>
                            </div>
                            <%--<asp:LinkButton ID="btnReverseInstalment" runat="server" Text="<%$ Resources:btnReverseInstalment %>" SkinID="btnSecondary" OnClientClick="return Validate();" OnClick="btnReverseInstalment_Click"></asp:LinkButton>--%>
                            <asp:HiddenField ID="hdnReverseInstalmentAuthority" runat="server" />
                            <asp:HiddenField ID="hdnPlanStatus" runat="server" />
                            <asp:HiddenField ID="hdnReverseInstalmentNoOfDays" runat="server" />                           
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div class="card-footer">
            <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:lbl_Cancel %>" Visible="false" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
            <asp:LinkButton ID="btnDecline" runat="server" Text="<%$ Resources:lbl_Decline %>" Visible="false" CausesValidation="false"  SkinID="btnPrimary"></asp:LinkButton>
            <asp:LinkButton ID="btnApprove" runat="server" Text="<%$ Resources:lbl_Approve %>" CausesValidation="true" Visible="false" SkinID="btnPrimary"></asp:LinkButton>
            <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:lbl_Ok %>" Visible="false" CausesValidation="true" SkinID="btnPrimary"></asp:LinkButton>
            <asp:LinkButton ID="btnRefresh" runat="server" Style="display: none" SkinID="btnPrimary"></asp:LinkButton>
         </div>
    </div>
    <asp:HiddenField ID="txtDecline" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="txtLimit" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hiddentxtwarningchequedate" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hiddentxtwarningMinChequeDate" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hidChkChoice" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hidChlClaimClose" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hidChkPaymentMsg" runat="server"></asp:HiddenField>
    <asp:CustomValidator ID="custvldComments" runat="server" CssClass="error" Display="none" Enabled="false" SetFocusOnError="true" ErrorMessage="<%$Resources:lbl_CommentsError %>" ValidationGroup="grpvalcashlistitem"></asp:CustomValidator>
    <asp:CustomValidator ID="IsClaimPaymentAuthority" runat="server" Display="None" ValidationGroup="grpvalcashlistitem"></asp:CustomValidator><asp:CustomValidator ID="VldUser" runat="server" Display="None" CssClass="error" ErrorMessage="<%$ Resources:Err_InvalidUser %>" ValidationGroup="grpvalcashlistitem"></asp:CustomValidator>
    <asp:CustomValidator ID="custvldSelectOneoftheAccount" runat="server" SetFocusOnError="true" Display="none" ErrorMessage="<%$ Resources:errmsg_SelectOneoftheAccount %>" ControlToValidate="txtAccount" Enabled="false" ValidationGroup="grpvalcashlistitem"></asp:CustomValidator>
    <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary" ValidationGroup="grpvalcashlistitem"></asp:ValidationSummary>
    <asp:RegularExpressionValidator ID="revBIC" runat="server" Display="None" ControlToValidate="txtBIC" ValidationExpression="^[a-zA-Z0-9]*$" ErrorMessage="<%$Resources:lbl_BICError %>" ValidationGroup="grpvalcashlistitem"></asp:RegularExpressionValidator>
    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Display="None" ControlToValidate="txtIBAN" ValidationExpression="^[a-zA-Z0-9]*$" ErrorMessage="<%$Resources:lbl_IBANError %>" ValidationGroup="grpvalcashlistitem"></asp:RegularExpressionValidator>

</div>
