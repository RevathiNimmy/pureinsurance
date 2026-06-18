<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PayClaim.ascx.vb" Inherits="Nexus.Controls_PayClaim" %>
<%@ Register Src="~/Controls/Instalments.ascx" TagName="Instalments" TagPrefix="uc1" %>
<%@ Register TagPrefix="OC" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register TagPrefix="uc1" TagName="Address" Src="~/Controls/AddressCntrl.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc2" %>

<script language="javascript" type="text/javascript">
    function IsAlphaNumeric(e) {
        var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
        var ret = ((keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (keyCode == 32));
        return ret;
    }

    $(document).ready(function () {
        //SET TAB DEFAULTS
        if ($('#<%= hfRememberTabs.ClientID %>').val() == "1") {
            //$('.nav-tabs li:eq(0) a').tab('show');
            $('#liReseveAndRecovery a').tab('show');
        }
        if ($('#<%= hfRememberTabs.ClientID %>').val() == "2") {            
            $('#liPaymentDetail a').tab('show');            
        }
    });
    function ActiveTabValueSet() {
        $('#<%= hfRememberTabs.ClientID %>').val("2")
        }       
    function isnumeric(ctrlval, e) {
        var key = window.event ? e.keyCode : e.which;
        var keychar = String.fromCharCode(key);

        if (ctrlval.indexOf(".") != -1 && (keychar == '.')) {
            return false;
        }

        ctrlval = ctrlval + keychar;

        if (parseFloat(ctrlval) > 100) {
            return false;
        }

        if ((parseFloat(ctrlval) == 100) && (keychar == '.')) {
            return false;
        }

        if ((keychar >= 0 && keychar < 10) || (keychar == '.')) {
            var x = (ctrlval);
            var decimalSeparator = ".";
            var val = "" + x;
            if (val.indexOf(decimalSeparator) != -1) {
                if (val.indexOf(decimalSeparator) < ctrlval.length - 3) {
                    return false;
                }
            }

            return true;
        }
        else { return false; }
        }      

    function fnValidatePercentage(ctrlval, e) {
        var key = window.event ? e.keyCode : e.which;
        var keychar = String.fromCharCode(key);

        if (ctrlval.indexOf(".") != -1 && (keychar == '.')) {
            return false;
        }

        ctrlval = ctrlval + keychar;

        if (parseFloat(ctrlval) > 100) {
            return false;
        }

        if ((parseFloat(ctrlval) == 100) && (keychar == '.')) {
            return false;
        }

        if ((keychar >= 0 && keychar < 10) || (keychar == '.')) {
            var x = (ctrlval);
            var decimalSeparator = ".";
            var val = "" + x;
            if (val.indexOf(decimalSeparator) != -1) {
                if (val.indexOf(decimalSeparator) < ctrlval.length - 3) {
                    return false;
                }
            }

            return true;
        }
        else { return false; }
    }

    function validateFloatKeyPress(el, evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;

        if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }

        if (charCode == 46 && el.value.indexOf(".") !== -1) {
            return false;
        }

        if (el.value.indexOf(".") !== -1) {
            var range = document.selection.createRange();

            if (range.text != "") {
            }
            else {
                var number = el.value.split('.');
                if (number.length == 2 && number[1].length > 1)
                    return false;
            }
        }

        return true;
    }
    function setConfirmation(sMessage, sChoice, sUrl) {

        var IsConfirm;
        if (sChoice == '1') {
            IsConfirm = window.confirm(sMessage);
            if (IsConfirm) {
                tb_show(null, sUrl, null);
                return false;
            }
        }
        else {

            tb_show(null, sUrl, null);
            return false;
        }


    }
    function setReInsurer(sName, sKey) //setReinsurer
    {
        document.getElementById('<%=txtParty.ClientID %>').value = unescape(sName);
        document.getElementById('<%=hPartyKey.ClientID %>').value = sKey;
        document.getElementById('<%=hInsurerName.ClientID%>').value = sName;
        __doPostBack('updPayClaim_UI', 'ReInsureSelection');
        tb_remove();
    }
    function setOtherParty(sName, sKey, sAgentCode) //setOtherParty
    {
        document.getElementById('<%= txtParty.ClientId%>').value = sAgentCode
        document.getElementById('<%= hPartyKey.ClientId%>').value = sKey;
        __doPostBack('updPayClaim_UI', 'OtherPartySelection');
        tb_remove();
    }
    function setClient(sName, sKey, sAgentCode) //Client
    {
        document.getElementById('<%= txtParty.ClientId%>').value = sName;
        document.getElementById('<%= hPartyKey.ClientId%>').value = sKey;
        __doPostBack('updPayClaim_UI', 'ClientPartySelection');
        tb_remove();
    }
    function pageLoad() {
        //this is needed if the trigger is external to the update panel   
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_beginRequest(OnBeginRequest);
        manager.add_endRequest(OnEndRequest);
    }


    function ShowHideRadioButton() {
        var session = '<%= HttpContext.Current.Session("MODE") %>'
        var payeetype = $("#<%= rblPayee.ClientID%>")[0].cells
        var payeecount = payeetype.length - 1
        if (session == "SalvageClaim" || session == "TPRecovery") {
            for (var i = 0; i <= payeecount; i++) {
                if ((payeetype[i].innerText).toUpperCase() == "INSURER") {
                    payeetype[i].hidden = false;
                    break;
                }
            }
        }
        else {
            for (var i = 0; i <= payeecount; i++) {
                if ((payeetype[i].innerText).toUpperCase() == "INSURER") {
                    payeetype[i].hidden = true;
                    break;
                }
            }
        }
    }

    function OnBeginRequest(sender, args) {

        var postBackElement = args.get_postBackElement();
        if (postBackElement.id == 'btnAccount') {
            $get(piAccount).style.display = "block";
        }

        //disable the button (or whatever else we need to do) here
        var dPay = document.getElementById('divPayments');

        if (dPay != null) {
            dPay.disabled = true;
        }

        var dRec = document.getElementById('divReceipts');

        if (dRec != null) {
            dRec.disabled = true;
        }
    }
    function OnEndRequest(sender, args) {
        //enable the button (or whatever else we need to do) here
        var dPay = document.getElementById('divPayments');

        if (dPay != null) {
            dPay.disabled = false;
        }

        var dRec = document.getElementById('divReceipts');

        if (dRec != null) {
            dRec.disabled = false;
        }

    }
      function fnSalvageEditClick(clickedRowIndex, salvageGridId) {
        var grid = document.getElementById(salvageGridId);
        if (!grid) return true;
        var rows = grid.getElementsByTagName('tr');
        for (var i = 1; i < rows.length; i++) {
            var dataRowIndex = i - 1;
            if (dataRowIndex === clickedRowIndex) continue;
            var links = rows[i].getElementsByTagName('a');
            for (var j = 0; j < links.length; j++) {
                if (links[j].id && links[j].id.indexOf('hypEditPayment') >= 0) {
                    links[j].disabled = true;
                    links[j].style.opacity = '0.4';
                    links[j].style.pointerEvents = 'none';
                    links[j].href = 'javascript:void(0);';
                }
            }
        }
        return true;
    }
    function ProcessClaimNextButton(Description) {

        document.getElementById('<%=hdnDuplicateClaimPaymentReason.ClientID %>').value = Description
        __doPostBack('', 'ProcessClaimPaymentNextButton');

    }
    function ProcessClaimFinishButton(Description) {

        document.getElementById('<%=hdnDuplicateClaimPaymentReason.ClientID %>').value = Description
        __doPostBack('', 'ProcessClaimPaymentFinishButton');

    }
</script>


<div id="Controls_PayClaim">
    <legend>
        <asp:Literal ID="lblClaimInformation" runat="server" Text="<%$ Resources:lt_ClaimInformation %>"></asp:Literal></legend>
    <div class="md-whiteframe-z0 bg-white">
        <ul id="child-tab" class="nav nav-lines nav-tabs b-danger">
            <li><a href="#tab-details" data-bs-toggle="tab" aria-expanded="true" class="active">
                <asp:Literal ID="liDetails" runat="server" Text="<%$ Resources:lt_liDetails %>"></asp:Literal></a></li>
            <li><a href="#tab-thispayment" data-bs-toggle="tab" aria-expanded="true">
                <asp:Literal ID="ltThisPayment" runat="server" Text="<%$ Resources:lt_ThisPayment %>"></asp:Literal></a></li>
            <li id="liInstalments" runat="server" visible="false"><a href="#tab-recovery-instalments" data-bs-toggle="tab" aria-expanded="true">Instalments</a></li>
        </ul>
        <div class="tab-content clearfix p b-t b-t-2x">
            <div id="tab-details" class="tab-pane animated fadeIn active" role="tabpanel">
                <asp:UpdatePanel ID="updPayClaim_UI" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="card-body clearfix no-padding">
                            <div class="form-horizontal">
                                <legend>
                                    <asp:Label ID="lblPayee" runat="server" Text="<%$ Resources:lt_Payee %>"></asp:Label></legend>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblRiskTypeHeader" runat="server" AssociatedControlID="lblRiskType" Text="<%$ Resources:lbl_RiskType%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <p class="form-control-static font-bold">
                                            <asp:Label ID="lblRiskType" runat="server"></asp:Label>
                                        </p>
                                    </div>

                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblLossCurrencyHeader" runat="server" AssociatedControlID="lblLossCurrency" Text="<%$ Resources:lbl_LossCurrency%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <p class="form-control-static font-bold">
                                            <asp:Label ID="lblLossCurrency" runat="server"></asp:Label>
                                        </p>
                                    </div>

                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblDateOfLossHeader" runat="server" AssociatedControlID="lblDateOfLoss" Text="<%$ Resources:lbl_DateOfLoss%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <p class="form-control-static font-bold">
                                            <asp:Label ID="lblDateOfLoss" runat="server"></asp:Label>
                                        </p>
                                    </div>

                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblPerilInfoHeader" runat="server" AssociatedControlID="lblPerilInfo" Text="<%$ Resources:lbl_PerilInfo%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <p class="form-control-static font-bold">
                                            <asp:Label ID="lblPerilInfo" runat="server"></asp:Label>
                                        </p>
                                    </div>

                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblpayment_To" runat="server" AssociatedControlID="ddlPayment_To" Text="<%$ Resources:lblpayment_To %>" Visible="false" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:DropDownList ID="ddlPayment_To" runat="server" OnSelectedIndexChanged="ddlPayment_To_SelectedIndexChanged" CssClass="field-medium form-control form-select" AutoPostBack="true" Visible="false">
                                        </asp:DropDownList>
                                        <asp:RadioButtonList ID="rblPayee" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="true" CssClass="asp-radio">
                                            <asp:ListItem Text="<%$ Resources:li_ClaimPayable %>" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:li_Party %>" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:li_Agent %>" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:li_Client %>" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:li_Insurer %>" Value="4"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-horizontal">
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="txtParty" Text="<%$ Resources:li_Party%>" ID="lblbtnParty"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtParty" runat="server" CssClass="form-control partial-TxtBox" />
                                        <asp:LinkButton ID="btnParty" runat="server" Text="<%$ Resources:btn_party %>" SkinID="btnPrimary"></asp:LinkButton>
                                        <asp:LinkButton ID="btnClientParty" runat="server" Text="<%$ Resources:btn_Clientparty %>" SkinID="btnPrimary"></asp:LinkButton>
                                    </div>
                                    <asp:HiddenField ID="hPartyKey" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hPartyBankKey" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hInsurerName" runat="server"></asp:HiddenField>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12" runat="server" visible="false" id="dvExGratia">
                                    <asp:Label ID="lblExGratia" runat="server" Visible="false" AssociatedControlID="chkExGratia" Text="<%$ Resources:lbl_ExGratia%> " class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:CheckBox ID="chkExGratia" runat="server" AutoPostBack="True" Visible="false" Text=" " CssClass="asp-check"></asp:CheckBox>
                                    </div>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblUltimatePayee" runat="server" AssociatedControlID="txtUltimatePayee" Text="<%$ Resources:lbl_UltimatePayee%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtUltimatePayee" runat="server" CssClass="field-extralarge form-control" MaxLength="255"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card-body clearfix no-padding">
                            <%-- ATS Contorll --%>
                            <div id="outerDivATS" runat="server" style="display: none;" class="row row-sm">
                                <div class="col-sm-6">
                                    <div class="card p grey-100">
                                        <legend>
                                            <asp:Label ID="lblHeading" runat="server" Text="<%$ Resources:lbl_Heading %>"></asp:Label>
                                        </legend>
                                        <div class="card-body clearfix no-padding">
                                            <div class="form-inline">
                                                <div class="form-group form-group-sm m-r-md">
                                                    <asp:CheckBox ID="chk_DomiciledITA" runat="server" AutoPostBack="true" Text=" " CssClass="asp-check"></asp:CheckBox>
                                                    <asp:Label ID="Label3" runat="server" AssociatedControlID="chk_DomiciledITA" Text="<%$ Resources:chk_DomiciledITA %>"></asp:Label>
                                                </div>
                                                <div class="form-group form-group-sm">
                                                    <asp:Label ID="lbl_PercentageITA" runat="server" AssociatedControlID="txtPercentageITA" Text="<%$ Resources:lbl_PercentageITA%>"></asp:Label>
                                                    <asp:TextBox ID="txtPercentageITA" runat="server" MaxLength="5" CssClass="w-xs form-control PercTextBox" onkeypress="return fnValidatePercentage(this.value,event);"></asp:TextBox>
                                                </div>
                                                <%-- <asp:RangeValidator ID="RangeValidator1" runat="Server" ControlToValidate="txtPercentageITA"
                                                        ErrorMessage="" MaximumValue="100" MinimumValue="0" SetFocusOnError="True" Type="Integer" />--%>
                                                <div class="form-group form-group-sm">
                                                    <asp:Label ID="lbl_TaxnoITA" runat="server" AssociatedControlID="txtTaxnoITA" Text="<%$ Resources:lbl_TaxnoITA%>" Width="60px"></asp:Label>
                                                    <asp:TextBox ID="txtTaxnoITA" runat="server" CssClass="w form-control" MaxLength="30"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="card p grey-100">
                                        <legend>
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:lbl_Heading_Payee %>"></asp:Label>
                                        </legend>
                                        <div class="card-body clearfix no-padding">
                                            <div class="form-inline">
                                                <div class="form-group form-group-sm m-r-md">
                                                    <asp:CheckBox ID="chk_DomicilePTA" runat="server" AutoPostBack="true" Text=" " CssClass="asp-check"></asp:CheckBox>
                                                    <asp:Label ID="Label2" runat="server" Font-Bold="false" AssociatedControlID="chk_DomicilePTA" Text="<%$ Resources:chk_DomicilePTA %>"></asp:Label>
                                                </div>
                                                <div class="form-group form-group-sm">
                                                    <asp:Label ID="lbl_PercentagePTA" runat="server" AssociatedControlID="txtPercentagePTA" Text="<%$ Resources:lbl_PercentagePTA%>"></asp:Label>
                                                    <asp:TextBox ID="txtPercentagePTA" runat="server" CssClass="w-xs form-control PercTextBox" MaxLength="5" onkeypress="return fnValidatePercentage(this.value,event);"></asp:TextBox>
                                                </div>
                                                <div class="form-group form-group-sm">
                                                    <%-- <asp:RangeValidator ID="RangeValidator2" runat="Server" ControlToValidate="txtPercentagePTA"
                                                        ErrorMessage="" MaximumValue="100" MinimumValue="0" SetFocusOnError="True" Type="Currency"/> --%>
                                                    <asp:Label ID="lbl_TaxNoPTA" runat="server" AssociatedControlID="txtTaxNoPTA" Text="<%$ Resources:lbl_TaxNoPTA%>"></asp:Label>
                                                    <asp:TextBox ID="txtTaxNoPTA" runat="server" MaxLength="30" CssClass="w form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card p grey-100">
                                        <legend>
                                            <asp:Label ID="lbl_Heading_Exemption" runat="server" Text="<%$ Resources:lbl_Heading_Exemption %>"></asp:Label></legend>
                                        <div class="card-body clearfix no-padding">
                                            <div class="form-inline">
                                                <div class="form-group form-group-sm m-r-md">
                                                    <asp:CheckBox ID="chk_Taxexempt" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                                                    <asp:Label ID="Label4" runat="server" AssociatedControlID="chk_Taxexempt" Text="<%$ Resources:chk_Taxexempt %>"></asp:Label>
                                                </div>
                                                <div class="form-group form-group-sm m-r-md">
                                                    <asp:CheckBox ID="chk_WHTExempt" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                                                    <asp:Label ID="Label5" runat="server" AssociatedControlID="chk_WHTExempt" Text="<%$ Resources:chk_WHTExempt %>"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%--for salvage and TP recovery--%>
                            <div id="divsalvage" runat="server" style="display: none" class="row row-sm">
                                <div class="col-sm-6">
                                    <div class="card p grey-100">
                                        <legend>
                                            <asp:Label ID="lbl_HeadingRTS" runat="server" Text="<%$ Resources:lbl_HeadingRTS %>"></asp:Label>
                                        </legend>
                                        <div class="card-body clearfix no-padding">
                                            <div class="form-inline">
                                                <div class="form-group form-group-sm m-r-md">
                                                    <asp:CheckBox ID="chk_TaxexemptRTS" runat="server" OnCheckedChanged="chk_TaxexemptRTS_CheckedChanged" AutoPostBack="true" Text=" " CssClass="asp-check"></asp:CheckBox>
                                                    <asp:Label ID="Label6" runat="server" AssociatedControlID="chk_TaxexemptRTS" Text="<%$ Resources:chk_TaxexemptRTS %>"></asp:Label>
                                                </div>
                                                <div class="form-group form-group-sm">
                                                    <asp:Label ID="lbl_PercentageRTS" runat="server" AssociatedControlID="txtPercentageRTS" Text="<%$ Resources:lbl_PercentageRTS%>"></asp:Label>
                                                    <asp:TextBox ID="txtPercentageRTS" runat="server" MaxLength="5" CssClass="w-xs form-control" onkeypress="return isnumeric(this.value,event);"></asp:TextBox>
                                                </div>
                                            </div>
                                            <%--<asp:RangeValidator ID="RangeValidator3" runat="Server" ControlToValidate="txtPercentageRTS"
                                                            ErrorMessage="" MaximumValue="100" MinimumValue="0" SetFocusOnError="True" Type="Integer" />--%>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="card p grey-100">
                                        <legend>
                                            <asp:Label ID="lbl_Heading_ITS" runat="server" Text="<%$ Resources:lbl_Heading_ITS %>"></asp:Label>
                                        </legend>
                                        <div class="card-body clearfix no-padding">
                                            <div class="form-inline">
                                                <div class="form-group form-group-sm m-r-md">
                                                    <asp:CheckBox ID="chk_DomicileITA" runat="server" OnCheckedChanged="chk_DomicileITA_CheckedChanged" AutoPostBack="true" Text=" " CssClass="asp-check"></asp:CheckBox>
                                                    <asp:Label ID="Label7" runat="server" AssociatedControlID="chk_DomicileITA" Text="<%$ Resources:chk_DomicileITA %>"></asp:Label>
                                                </div>
                                                <div class="form-group form-group-sm">
                                                    <asp:Label ID="lbl_Percentage_ITA" runat="server" AssociatedControlID="txtPercentage_ITA" Text="<%$ Resources:lbl_PercentageITA%>"></asp:Label>
                                                    <asp:TextBox ID="txtPercentage_ITA" runat="server" CssClass="w-xs form-control e-cur" MaxLength="5" onkeypress="return isnumeric(this.value,event);"></asp:TextBox>
                                                </div>
                                                <div class="form-group form-group-sm">
                                                    <%-- <asp:RangeValidator ID="RangeValidator4" runat="Server" ControlToValidate="txtPercentage_ITA"
                                                            ErrorMessage="" MaximumValue="100" MinimumValue="0" SetFocusOnError="True" Type="Integer" />--%>
                                                    <asp:Label ID="lbl_TaxNo_ITA" runat="server" AssociatedControlID="txtTaxNo_ITA" Text="<%$ Resources:lbl_TaxNo_PTA%>"></asp:Label>
                                                    <asp:TextBox ID="txtTaxNo_ITA" runat="server" MaxLength="30" CssClass="w  form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%--end of salvage and Recovery--%>
                            <%-- ATS Contorll --%>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="rblPayee" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
                <Nexus:ProgressIndicator ID="piPayClaim_UI" OverlayCssClass="updating" AssociatedUpdatePanelID="updPayClaim_UI" runat="server">
                    <progresstemplate>
                                </progresstemplate>
                </Nexus:ProgressIndicator>
                <legend>
                    <asp:Literal runat="server" ID="ltPageHeading"></asp:Literal>
                </legend>
                <div id="divReceipts" class="grid-card table-responsive">
                    <asp:UpdatePanel ID="updReceipt" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <asp:GridView ID="gvSalvageDetails" runat="server" AllowPaging="True" PagerSettings-Mode="Numeric" AutoGenerateColumns="False" GridLines="None" Enabled="false" Visible="false" OnRowCommand="gvSalvageDetails_RowCommand" OnRowDataBound="gvSalvageDetails_RowDataBound" AllowSorting="true" ShowHeader="true" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                <Columns>
                                    <asp:BoundField DataField="Description" HeaderText="<%$ Resources:lbl_RecoveryType %>" SortExpression="Description"></asp:BoundField>
                                    <asp:BoundField DataField="PartyShortName" HeaderText="<%$ Resources:lbl_RecoveryParty %>" ReadOnly="True"></asp:BoundField>
                                    <Nexus:BoundField DataField="TotalRecovery" HeaderText="<%$ Resources:lbl_TotalReserve %>" SortExpression="TotalRecovery" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:BoundField DataField="ReceiptedAmount" HeaderText="<%$ Resources:lbl_RecoverdTotal %>" SortExpression="ReceiptedAmount" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:BoundField DataField="ReceiptedTaxAmount" HeaderText="<%$ Resources:lbl_RecoverdTax %>" SortExpression="ReceiptedTaxAmount" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:BoundField DataField="LossThisReceiptINCLTax" HeaderText="<%$ Resources:lbl_ThisReceiptINCLTax %>" SortExpression="LossThisReceiptINCLTax" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:BoundField DataField="LossThisReceiptTax" HeaderText="<%$ Resources:lbl_ThisReceiptTax %>" SortExpression="LossThisReceiptTax" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:BoundField DataField="LossThisNet" HeaderText="<%$ Resources:lbl_ThisNet %>" SortExpression="LossThisNet" DataType="Currency"></Nexus:BoundField>
                                    <asp:TemplateField HeaderText="<%$ Resources:lbl_Balance %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBalance" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div class="rowMenu">
                                                <ol id="menu_<%# Eval("BaseRecoveryKey") %>" class="list-inline no-margin">
                                                    <li>
                                                        <asp:LinkButton ID="hypEditPayment" runat="server" Text="<%$ Resources:btn_EditPayment %>" CommandName="Edit" CommandArgument='<%# Container.DataItemIndex %>' SkinID="btnGrid"></asp:LinkButton>
                                                    </li>
                                                </ol>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <%-- <asp:HiddenField ID="hIsGrossClaimPaymentAmount" runat="server" />
                                        <asp:HiddenField ID="hClaimsIsPostTaxes" runat="server" />--%>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvSalvageDetails" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                            <asp:AsyncPostBackTrigger ControlID="gvSalvageDetails" EventName="Load"></asp:AsyncPostBackTrigger>
                            <asp:AsyncPostBackTrigger ControlID="gvSalvageDetails" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                            <asp:AsyncPostBackTrigger ControlID="gvSalvageDetails" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                        </Triggers>
                    </asp:UpdatePanel>
                    <Nexus:ProgressIndicator ID="piReceipt" OverlayCssClass="updating" AssociatedUpdatePanelID="updReceipt" runat="server">
                        <progresstemplate>
                                    </progresstemplate>
                    </Nexus:ProgressIndicator>
                </div>
                <div id="divPayments" class="grid-card table-responsive">
                    <asp:UpdatePanel ID="updPayment" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <asp:GridView ID="gvPaymentDetails" runat="server" AllowPaging="True" PagerSettings-Mode="Numeric" AutoGenerateColumns="False" GridLines="None" Visible="false" OnRowCommand="gvPaymentDetails_RowCommand" OnRowDataBound="gvPaymentDetails_RowDataBound" AllowSorting="true" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                <RowStyle></RowStyle>
                                <Columns>
                                    <asp:BoundField DataField="Description" HeaderText="" SortExpression="Description"></asp:BoundField>
                                    <Nexus:BoundField DataField="TotalReserve" HeaderText="<%$ Resources:lbl_TotalReserve %>" SortExpression="TotalReserve" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:BoundField DataField="ThisRevision" HeaderText="<%$ Resources:lbl_ThisRevision %>" SortExpression="ThisRevision" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:BoundField DataField="PaidToDate" HeaderText="<%$ Resources:lbl_PaidtoDate %>" SortExpression="PaidToDate" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:BoundField DataField="PaidToDateTax" HeaderText="<%$ Resources:lbl_PaidtoDateTax %>" SortExpression="PaidToDateTax" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:BoundField DataField="CurrentReserve" HeaderText="<%$ Resources:lbl_CurrentReserve %>" SortExpression="CurrentReserve" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:BoundField DataField="ThisPaymentINCLTax" HeaderText="<%$ Resources:lbl_ThisPaymentInclTax %>" SortExpression="ThisPaymentINCLTax" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:BoundField DataField="ThisPaymentTax" HeaderText="<%$ Resources:lbl_ThisPaymentTax %>" SortExpression="ThisPaymentTax" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:BoundField DataField="CostToClaim" HeaderText="<%$ Resources:lbl_CostToClaim %>" SortExpression="CostToClaim" DataType="Currency"></Nexus:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div class="rowMenu">
                                                <ol id="menu_<%# Eval("BaseReserveKey") %>" class="list-inline no-margin">
                                                    <li>
                                                        <asp:LinkButton ID="hypEditPayment" runat="server" Text="<%$ Resources:btn_EditPayment %>" CommandName="Edit" CommandArgument='<%# Eval("BaseReserveKey")%>' SkinID="btnGrid"> </asp:LinkButton>
                                                    </li>
                                                </ol>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <Nexus:BoundField DataField="IsExcess" HeaderText="<%$ Resources:lbl_CostToClaim %>" DataType="Boolean"></Nexus:BoundField>
                                </Columns>
                            </asp:GridView>
                            <asp:HiddenField ID="hIsGrossClaimPaymentAmount" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="hClaimsIsPostTaxes" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="hSalvageAndTPRecoveryReservesExcludeTax" runat="server"></asp:HiddenField>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="gvPaymentDetails" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                            <asp:AsyncPostBackTrigger ControlID="gvPaymentDetails" EventName="RowEditing"></asp:AsyncPostBackTrigger>
                            <asp:AsyncPostBackTrigger ControlID="gvPaymentDetails" EventName="Load"></asp:AsyncPostBackTrigger>
                            <asp:AsyncPostBackTrigger ControlID="gvPaymentDetails" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                            <asp:AsyncPostBackTrigger ControlID="gvPaymentDetails" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                        </Triggers>
                    </asp:UpdatePanel>
                    <Nexus:ProgressIndicator ID="piPayment" OverlayCssClass="updating" AssociatedUpdatePanelID="updPayment" runat="server">
                        <progresstemplate>
                                    </progresstemplate>
                    </Nexus:ProgressIndicator>
                </div>
            </div>
            <div id="tab-thispayment" class="tab-pane animated fadeIn" role="tabpanel">
                <asp:UpdatePanel ID="updThisPayment" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <asp:Panel ID="pnlThisPaymentTab" runat="server" Visible="false">
                            <div class="card">
                                <div class="card-heading">
                                    <h1>
                                        <asp:Literal ID="lblThisPayment" runat="server" Text="<%$ Resources:lt_ThisPayment %>"></asp:Literal></h1>
                                </div>
                                <div class="card-body clearfix">
                                    <div class="form-horizontal">
                                        <legend>
                                            <asp:Label ID="ThisPaymentSummary" runat="server" Text="<%$ Resources:lbl_ThisPaymentSummary %>"></asp:Label>
                                        </legend>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblGrossPayment" runat="server" AssociatedControlID="txtGrossPayment" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltGrossPayment" runat="server" Text="<%$ Resources:lbl_GrossPayment%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtGrossPayment" runat="server" CssClass="PercTextBox form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblTotalTax" runat="server" AssociatedControlID="txtTotalTax" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltTotalTax" runat="server" Text="<%$ Resources:lbl_TotalTax%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtTotalTax" runat="server" CssClass="PercTextBox form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblTotalWHTax" runat="server" AssociatedControlID="txtTotalWHTax" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltTotalWHTax" runat="server" Text="<%$ Resources:lbl_TotalWHTax%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtTotalWHTax" runat="server" CssClass="PercTextBox form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblNetPayment" runat="server" AssociatedControlID="txtNetPayment" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltNetPayment" runat="server" Text="<%$ Resources:lbl_NetPayment%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtNetPayment" runat="server" CssClass="PercTextBox form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <legend>
                                        <asp:Literal ID="lblTaxesOnThisPayment" runat="server" Text="<%$ Resources:lbl_TaxesOnThisPayment %>"></asp:Literal></legend>
                                    <div class="grid-card table-responsive">
                                        <asp:GridView ID="gvTaxesonThisPayment" runat="server" PageSize="10" AllowPaging="true" PagerSettings-Mode="Numeric" AutoGenerateColumns="false" GridLines="None" Visible="false" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                            <Columns>
                                                <asp:BoundField HeaderText="ReserveType" DataField="ReserveType" SortExpression="ReserveType"></asp:BoundField>
                                                <asp:BoundField HeaderText="TaxGroup" DataField="TaxGroupCode" SortExpression="TaxGroupCode"></asp:BoundField>
                                                <asp:BoundField HeaderText="TaxBand" DataField="TaxBandCode" SortExpression="TaxBandCode"></asp:BoundField>
                                                <Nexus:BoundField HeaderText="Percentage" DataField="Percentage" DataType="Percentage" SortExpression="Percentage"></Nexus:BoundField>
                                                <Nexus:BoundField HeaderText="Amount" DataField="Amount" DataType="Currency" SortExpression="Amount"></Nexus:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="grid-card table-responsive">
                                        <asp:GridView ID="gvTaxesonThisReceipt" runat="server" PageSize="10" AllowPaging="true" PagerSettings-Mode="Numeric" AutoGenerateColumns="false" GridLines="None" Visible="false" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                            <Columns>
                                                <asp:BoundField HeaderText="RecoveryType" DataField="RecoveryType" SortExpression="RecoveryType"></asp:BoundField>
                                                <asp:BoundField HeaderText="TaxGroup" DataField="TaxGroupCode" SortExpression="TaxGroupCode"></asp:BoundField>
                                                <asp:BoundField HeaderText="TaxBand" DataField="TaxBandCode" SortExpression="TaxBandCode"></asp:BoundField>
                                                <Nexus:BoundField HeaderText="Percentage" DataField="Percentage" DataType="Percentage" SortExpression="Percentage"></Nexus:BoundField>
                                                <Nexus:BoundField HeaderText="Amount" DataField="Amount" DataType="Currency" SortExpression="Amount"></Nexus:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="form-horizontal">
                                        <legend>
                                            <asp:Label ID="lblPaymentDetails" runat="server" Text="<%$ Resources:lbl_PaymentDetails %>"></asp:Label></legend>

                                        <div id="liAccountType" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblAccountType" runat="server" AssociatedControlID="ddlAccountType" Text="<%$ Resources:lbl_AccountType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                            <div class="col-md-8 col-sm-9">
                                                <asp:DropDownList ID="ddlAccountType" OnSelectedIndexChanged="ddlAccountType_SelectedIndexChanged" AutoPostBack="true" runat="server" CssClass="field-medium form-control"></asp:DropDownList>
                                                 <asp:TextBox ID="txtAccountType" Visible ="false" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblMediatype" runat="server" AssociatedControlID="GISLookup_MediaType" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltMediatype" runat="server" Text="<%$ Resources:lbl_Mediatype %>"></asp:Literal><em id="emMediatype" runat="server" visible="false">*</em></asp:Label><div class="col-md-8 col-sm-9">
                                                    <NexusProvider:LookupList ID="GISLookup_MediaType" runat="server" DataItemText="Description" DefaultText="(Please Select)" DataItemValue="Code" ListCode="MediaType" ListType="PMLookup" CssClass="field-medium form-control form-select" AutoPostBack="true"></NexusProvider:LookupList>
                                                </div>
                                            <asp:HiddenField ID="hdMediaTypeValidationId" runat="server" />
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblMediaRef" runat="server" AssociatedControlID="txtMediaRef" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltMediaRef" runat="server" Text="<%$ Resources:lbl_MediaRef%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtMediaRef" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblPayeeName" runat="server" AssociatedControlID="txtPayeeName" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltPayeeName" runat="server" Text="<%$ Resources:lbl_PayeeName%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtPayeeName" runat="server" onkeypress="return IsAlphaNumeric(event);" onpaste="return false;"  MaxLength="60" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div id="liChequeDate" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblChequeDate" runat="server" AssociatedControlID="txtChequeDate" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltChequeDate" runat="server" Text="<%$ Resources:lbl_ChequeDate%>"></asp:Literal></asp:Label>
                                            <div class="col-md-8 col-sm-9">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtChequeDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <uc2:CalendarLookup ID="calChequeDate" runat="server" LinkedControl="txtChequeDate" HLevel="5"></uc2:CalendarLookup>
                                               
                                                    </div>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblBankName" runat="server" AssociatedControlID="txtBankName" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltBankName" runat="server" Text="<%$ Resources:lbl_BankName%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control" MaxLength="255"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblBankCode" runat="server" AssociatedControlID="txtBankCode" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltBankCode" runat="server" Text="<%$ Resources:lbl_BankCode%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtBankCode" runat="server" CssClass="form-control" MaxLength="8"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblBankAccNumber" runat="server" AssociatedControlID="txtBankAccNumber" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltBankAccNumber" runat="server" Text="<%$ Resources:lbl_BankAccNumber%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtBankAccNumber" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div id="liThisReference" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblThisReference" runat="server" AssociatedControlID="txtThisReference" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltThisReference" runat="server" Text="<%$ Resources:lbl_ThisReference%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtThisReference" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblOurRef" runat="server" AssociatedControlID="txtOurRef" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltOurRef" runat="server" Text="<%$ Resources:lbl_OurRef%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtOurRef" runat="server" CssClass="form-control" MaxLength="255"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblBIC" runat="server" AssociatedControlID="txtBIC" Text="<%$ Resources:lbl_BIC %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                            <div class="col-md-8 col-sm-9">
                                                <asp:TextBox ReadOnly="true" ID="txtBIC" runat="server" MaxLength="50" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblIBAN" runat="server" AssociatedControlID="txtIBAN" Text="<%$ Resources:lbl_IBAN %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                            <div class="col-md-8 col-sm-9">
                                                <asp:TextBox ReadOnly="true" ID="txtIBAN" runat="server" MaxLength="50" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div id="liComments" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblComments" runat="server" AssociatedControlID="txtComments" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltComments" runat="server" Text="<%$ Resources:lbl_Comments%>"></asp:Literal>
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtComments" runat="server" CssClass="form-control" TextMode="MultiLine" MaxLength="255"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>
                                    <div id="divAddress" runat="server">
                                        <legend>
                                            <asp:Label ID="lblAddress" runat="server" Text="<%$ Resources:lbl_Address %>"></asp:Label></legend>
                                        <uc1:Address ID="Address" runat="server"></uc1:Address>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="GISLookup_MediaType" EventName="SelectedIndexChange" />
                    </Triggers>
                </asp:UpdatePanel>
                <Nexus:ProgressIndicator ID="piThisPayment" OverlayCssClass="updating" AssociatedUpdatePanelID="updThisPayment" runat="server">
                    <progresstemplate>
                            </progresstemplate>
                </Nexus:ProgressIndicator>
            </div>
            
            <asp:CustomValidator ID="IsPaymentReceived" runat="server" CssClass="error" ErrorMessage="<%$ Resources:lbl_PaymentReceived_Error %>" Display="none"></asp:CustomValidator><asp:CustomValidator ID="CustVldProductRiskMaintenance" runat="server" Display="None" SetFocusOnError="true"></asp:CustomValidator>
            <asp:CustomValidator ID="CustTaxnoITA" runat="server" CssClass="error" ErrorMessage="<%$ Resources:msg_ReqTaxnoITA %>" Display="none"></asp:CustomValidator>

            <asp:CustomValidator ID="CustTaxnoPTA" runat="server" CssClass="error" ErrorMessage="<%$ Resources:msg_ReqTaxnoPTA %>" Display="none"></asp:CustomValidator>

            <asp:CustomValidator ID="CustPerRTS" runat="server" CssClass="error" ErrorMessage="<%$ Resources:msg_ReqPerRTS %>" Display="none"></asp:CustomValidator>
            <asp:CustomValidator ID="custCLMPAYBankValidation" runat="server" CssClass="error" ErrorMessage="<%$ Resources:msg_CLMPAYBankValidationFailed %>" Display="none"></asp:CustomValidator>
            <asp:CustomValidator ID="CustAllowMultipleClaimPayment" runat="server" CssClass="error" ErrorMessage="<%$ Resources:lbl_AllowMultipleClaimPayment_error %>" Display="none"></asp:CustomValidator><asp:HiddenField ID="HidMediaTypeFieldMandatory" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="HidPayClaimWarningConfirmation" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hfRememberTabs" runat="server"></asp:HiddenField>
            <%-- Instalments tab pane --%>
            <div id="tab-recovery-instalments" class="tab-pane animated fadeIn" role="tabpanel">
                <asp:Panel ID="pnlInstalments" runat="server">
                    <asp:Label ID="lblInstalmentMessage" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                    <uc1:Instalments ID="ucInstalments" runat="server"></uc1:Instalments>
                    <div class="p-v-sm text-right">
                        <asp:LinkButton ID="btnSaveInstalmentPlan" runat="server" SkinID="btnPrimary" Text="Save Plan" CausesValidation="false" ValidationGroup="InstalmentPlanSave"></asp:LinkButton>
                    </div>
                </asp:Panel>
            </div>
            <asp:HiddenField ID="HidATSoption" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hfSelectedRecoveryRow" runat="server" Value="-1"></asp:HiddenField>
            <asp:HiddenField ID="hdnDuplicateClaimPaymentReason" runat="server"/>
                <asp:HiddenField ID="hdnDuplicateClaimPayment" runat="server"/>
            <asp:CustomValidator ID="CustVldValidateAccountNumber" runat="server" Enabled="false"
                Display="None" OnServerValidate="CustVldValidateAccountNumber_ServerValidate" ValidateEmptyText="false" EnableClientScript="false" />

        </div>
    </div>

</div>
