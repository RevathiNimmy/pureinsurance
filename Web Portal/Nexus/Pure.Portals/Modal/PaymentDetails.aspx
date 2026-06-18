<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="PaymentDetails.aspx.vb" Inherits="Nexus.Modal_PaymentDetails" Title="PaymentDetails" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script type="text/javascript">
        $(document).ready(function () {

            IsTaxGroupMandatory();
        });

        function IsNegativeReserve(confirmMsg) {

            var optionvalue = document.getElementById('<%=HidNegativeReserve.ClientID %>')
            var balance = document.getElementById('<%=txtBalance.ClientID %>')
            var Netpayment = document.getElementById('<%=txtNetPaymentDisabled.ClientID %>')
            var reservetype = document.getElementById('<%=txtReserveType.ClientID %>')
            var reservetype = document.getElementById('<%=txtReserveType.ClientID %>')
            var IsGrossClaimPaymentAmount = document.getElementById('<%=hIsGrossClaimPaymentAmount.ClientID %>')
            var TaxAmount = document.getElementById('<%=HiddenLossTaxAmountDisabled.ClientID %>')
            
            //enter the payment amount and press OK.  DO NOT tab out of the field, but press OK.
            if (ConvertCurrencyToFloat(Netpayment.value) == 0) {
                Netpayment.value = ConvertCurrencyToFloat(document.getElementById('<%=txtCurrencyRate.ClientID %>').value) * ConvertCurrencyToFloat(document.getElementById('<%=txtPaymentAmount.ClientID %>').value)
            }
           
            if (optionvalue.value == "0" && ConvertCurrencyToFloat(Netpayment.value) > ConvertCurrencyToFloat(balance.value)) {
                // added this functionality due to time constraint, it will be replace by dynamic query
                if (IsGrossClaimPaymentAmount.value == "1") {
                    var result = confirm(confirmMsg)
                    return result;
                } else if ((Math.round((parseFloat(Netpayment.value.replace(',', '')) - parseFloat(TaxAmount.value.replace(',', ''))) * 100) / 100) > parseFloat(balance.value.replace(',', ''))) {
                    var result = confirm(confirmMsg)
                    return result;
                } else {
                    return true;
                }
            }
            else {
                return true;
            }
        }

        function ConvertCurrencyToFloat(value) {
            return parseFloat(value.replace(/,/g, ""));
        }
        function IsNegativeInteger(e) {
            var key = window.event ? e.keyCode : e.which;
            var keychar = String.fromCharCode(key);
            var ValidChars = "-0123456789.";
            var IsNumber = true;
            if (ValidChars.indexOf(keychar) == -1) {
                IsNumber = false;
            }
            return IsNumber;
        }
        function IsTaxGroupMandatory(e) {
            if ((document.getElementById('<%= IsTaxGroupMandatory.ClientId%>').value) == 1)
                $("#<%= ddlTaxGroup.ClientID %>").addClass('field-mandatory');
        }
        function IsValidDecimal(e) {
            var skey = window.event ? e.keyCode : e.which;
            var skeychar = String.fromCharCode(skey);
            var sValidChars = "0123456789.";
            var bIsDecimal = true;
            if (sValidChars.indexOf(skeychar) == -1) {
                bIsDecimal = false;
            }
            if (bIsDecimal == true && skeychar == ".") {
                var oSource = e.target || e.srcElement;
                var sExistingText = oSource.value;
                if (sExistingText.indexOf(".") != -1) {
                    bIsDecimal = false;
                }
            }
            return bIsDecimal;
        }
    </script>

    <asp:ScriptManager ID="smPaymentDetails" runat="server"></asp:ScriptManager>
    <div id="Modal_PaymentDetails">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label ID="lblPaymentDetails" runat="server" Text="Claim Payment Details"></asp:Label>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblReserve" runat="server" Text="<%$ Resources:lbl_Reserve %>"></asp:Label></legend>

                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblReserveType" runat="server" AssociatedControlID="txtReserveType" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltReserveType" runat="server" Text="<%$ Resources:lt_ReserveType%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtReserveType" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblTotalReserve" runat="server" AssociatedControlID="txtTotalReserve" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltTotalReserve" runat="server" Text="<%$ Resources:lt_TotalReserve%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtTotalReserve" runat="server" CssClass="PercTextBox form-control" Enabled="false"></asp:TextBox>
                            </div>
                    </div>
                    <div class="AlternateListItem form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRiskType" runat="server" AssociatedControlID="txtRiskType" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltRiskType" runat="server" Text="<%$ Resources:lt_RiskType%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtRiskType" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPaidToDate" runat="server" AssociatedControlID="txtPaidToDate" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltPaidToDate" runat="server" Text="<%$ Resources:lt_PaidToDate%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtPaidToDate" runat="server" CssClass="PercTextBox form-control" Enabled="false"></asp:TextBox>
                            </div>
                    </div>
                    <div class="AlternateListItem form-group form-group-sm col-lg-6 col-md-6 col-sm-12"></div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblBalance" runat="server" AssociatedControlID="txtBalance" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltBalance" runat="server" Text="<%$ Resources:lt_Balance%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="txtBalance" runat="server" CssClass="PercTextBox form-control" Enabled="false"></asp:TextBox>
                            </div>
                    </div>
                    <div id="liReverseExcess" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblReverseExcess" runat="server" AssociatedControlID="chkReverseExcess" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltRReverseExcess" runat="server" Text="<%$ Resources:lt_ReverseExcess%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                <asp:CheckBox ID="chkReverseExcess" runat="server" OnCheckedChanged="chkReverseExcess_CheckedChanged" AutoPostBack="true" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </div>
                    </div>
                </div>
                <asp:UpdatePanel ID="updateCurrency" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="form-horizontal">
                            <legend>
                                <asp:Label ID="lblPayment" runat="server" Text="<%$ Resources:lbl_Payment %>"></asp:Label></legend>

                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Literal ID="lblPaymentCurrency" runat="server" Text="<%$ Resources:lt_PaymentCurrency %>"></asp:Literal>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Literal ID="lblLossCurrency" runat="server" Text="<%$ Resources:lt_LossCurrency %>"></asp:Literal>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblCurrency" runat="server" AssociatedControlID="ddlCurrency" Text="<%$ Resources:lt_PaymentCurrency %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:DropDownList ID="ddlCurrency" runat="server" AutoPostBack="true" CssClass="field-medium field-mandatory form-control form-select"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblCurrencyInTextBox" runat="server" AssociatedControlID="txtCurrency" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltCurrencyInTextBox" runat="server" Text="<%$ Resources:lbl_Currency%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtCurrency" runat="server" CssClass=" form-control" Enabled="false"></asp:TextBox>
                                    </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblCurrencyRate" runat="server" AssociatedControlID="txtCurrencyRate" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltCurrencyRate" runat="server" Text="<%$ Resources:lbl_CurrencyRate%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtCurrencyRate" runat="server" CssClass="PercTextBox form-control" Enabled="false"></asp:TextBox>
                                    </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblPaymentAmount" runat="server" AssociatedControlID="txtPaymentAmount" Text="<%$ Resources:lbl_PaymentAmount%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="txtPaymentAmountDisabled" runat="server" CssClass="PercTextBox form-control" Enabled="false"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="rqdPaymentAmount" runat="server" ControlToValidate="txtPaymentAmount" SetFocusOnError="true" Display="None" ErrorMessage="<%$ Resources:lbl_Payment_Error %>" ValidationGroup="PaymentDetails"></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblPaymentAmountDisabled" runat="server" AssociatedControlID="txtPaymentAmountDisabled" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltPaymentAmountDisabled" runat="server" Text="<%$ Resources:lbl_PaymentAmount%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtPaymentAmount" runat="server" CssClass="field-mandatory PercTextBox form-control" AutoPostBack="true"></asp:TextBox>
                                    </div>
                            </div>
                            <div id="liBalanceAmount" runat="server" visible="true" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblBalanceAmount" runat="server" AssociatedControlID="txtBalanceAmount" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltBalanceAmount" runat="server" Text="<%$ Resources:lbl_BalanceAmount %>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtBalanceAmount" runat="server" CssClass="PercTextBox form-control" AutoPostBack="true" Enabled="false"></asp:TextBox>
                                    </div>
                            </div>
                            <asp:HiddenField ID="HiddenLossPaymentAmount" runat="server"></asp:HiddenField>

                        </div>
                        <asp:Panel ID="pnlTaxesGroup" runat="server" Visible="false">
                            <asp:UpdatePanel ID="updateTaxGroup" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                <ContentTemplate>
                                    <div class="form-horizontal">
                                        <legend>
                                            <asp:Label ID="lblTaxes" runat="server" Text="<%$ Resources:lbl_taxes %>"></asp:Label></legend>

                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblTaxGroup" runat="server" AssociatedControlID="ddlTaxGroup" Text="<%$ Resources:lbl_TaxGroup %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                            <div class="col-md-8 col-sm-9">
                                                <asp:DropDownList ID="ddlTaxGroup" runat="server" CssClass="field-medium form-control form-select" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblTaxAmount" runat="server" AssociatedControlID="txtTaxAmount" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltTaxAmount" runat="server" Text="<%$ Resources:lbl_TaxAmount%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                                    <asp:TextBox ID="txtTaxAmount" runat="server" CssClass="PercTextBox form-control" Enabled="false"></asp:TextBox>
                                                </div>
                                        </div>
                                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblTaxAmountDisabled" runat="server" AssociatedControlID="txtTaxAmountDisabled" class="col-md-4 col-sm-3 control-label">
                                                <asp:Literal ID="ltTaxAmountDisabled" runat="server" Text="<%$ Resources:lbl_TaxAmount%>"></asp:Literal></asp:Label>
                                            <div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtTaxAmountDisabled" runat="server" CssClass="PercTextBox form-control" Enabled="false"></asp:TextBox>
                                            </div>
                                            <asp:HiddenField ID="HiddenLossTaxAmountDisabled" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="HiddenIsExcess" runat="server"></asp:HiddenField>
                                        </div>
                                        <div class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                            <asp:Label ID="lblScriptedTaxamount" runat="server" Text="<%$ Resources:lblScriptedTaxamount%>" AssociatedControlID="txtScriptedTaxAmount" class="col-md-4 col-sm-3 control-label">                                                       
                                            </asp:Label><div class="col-md-8 col-sm-9">
                                                <asp:TextBox ID="txtScriptedTaxAmount" runat="server" CssClass="PercTextBox form-control" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlTaxGroup" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="ddlCurrency" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="txtPaymentAmount" EventName="TextChanged"></asp:AsyncPostBackTrigger>
                                    <asp:AsyncPostBackTrigger ControlID="chkReverseExcess" EventName="CheckedChanged"></asp:AsyncPostBackTrigger>
                                        <asp:AsyncPostBackTrigger ControlID="txtTaxAmount" EventName="TextChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <Nexus:ProgressIndicator ID="upPaymentDetails" OverlayCssClass="updating" AssociatedUpdatePanelID="updateTaxGroup" runat="server">
                                <progresstemplate>
                                    </progresstemplate>
                            </Nexus:ProgressIndicator>
                        </asp:Panel>
                        <div class="form-horizontal">
                            <legend>
                                <asp:Label ID="lblTotal" runat="server" Text="<%$ Resources:lbl_Total %>"></asp:Label></legend>

                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblNetPayment" runat="server" AssociatedControlID="txtNetPayment" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltNetPayment" runat="server" Text="<%$ Resources:lbl_NetPayment%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtNetPayment" runat="server" CssClass="PercTextBox form-control" Enabled="false"></asp:TextBox>
                                    </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Label ID="lblNetPaymentDisabled" runat="server" AssociatedControlID="txtNetPaymentDisabled" class="col-md-4 col-sm-3 control-label">
                                    <asp:Literal ID="ltNetPaymentDisabled" runat="server" Text="<%$ Resources:lbl_NetPayment%>"></asp:Literal></asp:Label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtNetPaymentDisabled" runat="server" CssClass="PercTextBox form-control" Enabled="false"></asp:TextBox>
                                    </div>
                            </div>
                        </div>
                        <asp:HiddenField ID="hIsGrossClaimPaymentAmount" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hClaimsIsPostTaxes" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hSalvageAndTPRecoveryReservesExcludeTax" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hdnTotalReserve" runat="server"></asp:HiddenField>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCurrency" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="ddlTaxGroup" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="txtPaymentAmount" EventName="TextChanged"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="chkReverseExcess" EventName="CheckedChanged"></asp:AsyncPostBackTrigger>
                            <asp:AsyncPostBackTrigger ControlID="txtTaxAmount" EventName="TextChanged" />
                    </Triggers>
                </asp:UpdatePanel>
                <Nexus:ProgressIndicator ID="upCurrency" OverlayCssClass="updating" AssociatedUpdatePanelID="updateCurrency" runat="server">
                    <progresstemplate>
                        </progresstemplate>
                </Nexus:ProgressIndicator>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:btnCancel %>" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnOk" ValidationGroup="PaymentDetails" runat="server" Text="<%$ Resources:btnOk %>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" ValidationGroup="PaymentDetails" CssClass="validation-summary"></asp:ValidationSummary>
        <asp:CustomValidator ID="CustVldProductRiskMaintenance" runat="server" Display="None" SetFocusOnError="true" ValidationGroup="PaymentDetails"></asp:CustomValidator>
        <asp:CustomValidator ID="CustVldIsTaxGroupMandatory" runat="server" Display="None" SetFocusOnError="true" ValidationGroup="PaymentDetails"></asp:CustomValidator>
        <asp:CustomValidator ID="cvAllowMultipleClaimPayment" runat="server" CssClass="error" ErrorMessage="<%$ Resources:lbl_cvAllowMultipleClaimPayment_error %>" Display="none" ValidationGroup="PaymentDetails"></asp:CustomValidator><asp:CustomValidator ID="IsPaymentAmount" runat="server" Display="None" SetFocusOnError="true" ValidationGroup="PaymentDetails"></asp:CustomValidator>
        <asp:HiddenField ID="HidNegativeReserve" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="HidPaymentCannotExceedReserve" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="IsTaxGroupMandatory" runat="server"></asp:HiddenField>
    </div>
</asp:Content>
