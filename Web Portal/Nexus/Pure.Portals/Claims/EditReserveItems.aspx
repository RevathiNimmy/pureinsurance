<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditReserveItems.aspx.vb"
    Inherits="Nexus.Framework_EditReserveItems" MasterPageFile="~/default.master" %>

<%@ Register TagPrefix="OC" Namespace="Nexus" %>
<%@ Register Src="~/Controls/ClaimsProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="server">

    <script type="text/javascript">

        function ReviseAmount(dInitialReserve, dRevisedReserve, dAmount, ctrlRevised, sProcessType) {
            if (IsNumeric(dAmount) == true) {
                if (sProcessType == "1") {
                    ctrlRevised.value = parseFloat(dAmount);
                }
                else {
                    ctrlRevised.value = parseFloat(dInitialReserve) + parseFloat(dRevisedReserve) + parseFloat(dAmount);
                }
                if (ctrlRevised.value == 'NaN') {
                    if (sProcessType == "1") {
                        ctrlRevised.value = parseFloat(dInitialReserve); //parseFloat(dAmount) ;
                    }
                    else {
                        ctrlRevised.value = parseFloat(dInitialReserve) + parseFloat(dRevisedReserve);
                    }
                }
            }
            else {
                if (sProcessType == "1") {
                    ctrlRevised.value = parseFloat(dInitialReserve); //parseFloat(dAmount) ;
                }
                else {
                    ctrlRevised.value = parseFloat(dInitialReserve) + parseFloat(dRevisedReserve);
                }
            }
        }

        function IsNumeric(sNumber) {
            var bIsValid = true;

            if (sNumber.length > 0) {
                for (i = 0; i < sNumber.length; i++) {
                    if (sNumber.charAt(i) == '+' || sNumber.charAt(i) == '-'
                        || sNumber.charAt(i) == '.' || sNumber.charAt(i) == ','
                        || parseInt(sNumber.charAt(i)) >= 0) {
                    }
                    else {
                        bIsValid = false;
                    }
                }
            }
            else {
                bIsValid = false;
            }

            return bIsValid;
        }
        function isInteger(e) {
            var key = window.event ? e.keyCode : e.which;
            var keychar = String.fromCharCode(key);
            if (keychar == "-") {
                return true;
            }
            else {
                reg = /\d/;
                return reg.test(keychar);
            }
        }


    </script>

    <div id="Claims_EditReserveItems">
        <uc3:ProgressBar ID="ucProgressBar" runat="server"></uc3:ProgressBar>
        <OC:ImprovedTabIndex ID="TabIndex" runat="server" CssClass="TabContainer" TabContainerClass="page-progress" ActiveTabClass="ActiveTab" DisabledClass="DisabledTab"></OC:ImprovedTabIndex>

        <div class="card">
            <input id="hdnCalculate" runat="server" type="hidden" value="0">
            <div class="card-heading">
                <h1>
                    <asp:Label ID="lblPageTitle" runat="server" Text="<%$ Resources:lblPageTitle %>"></asp:Label></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="grid-card table-responsive">
                        <asp:GridView ID="grdvReserveItem" runat="server" AutoGenerateColumns="false" GridLines="None" EnableViewState="true" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ Resources:lbl_grdvReserveItem_Description_heading %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:lbl_grdvReserveItem_InitialReserve_heading %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInitialReserve" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:lbl_grdvRevisedItem_Amount_heading %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRevisedReserve" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:lbl_grdvReserveItem_Amount_heading %>">
                                    <ItemTemplate>
                                        <asp:TextBox CssClass="largecss" ID="txtAmount" runat="server" size="6" onkeypress="javascript:return isInteger(event);"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:lbl_grdvReserveItem_CurrentReserve_heading %>">
                                    <ItemTemplate>
                                        <asp:TextBox ID="lblCurrentReserve" runat="server" ReadOnly="true" size="6"></asp:TextBox>
                                        <asp:HiddenField ID="HiddenCurrReserve" runat="server"></asp:HiddenField>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CustomValidator ID="rqdvldRevised" runat="server" ErrorMessage="<%$ Resources: lbl_Compare_Revised_Amt %>" Enabled="false"></asp:CustomValidator>
                                        <asp:RangeValidator MaximumValue="79228162514264337593543950335" MinimumValue="-79228162514264337593543950335" Type="Double" ID="RngtxtAmount" ControlToValidate="txtAmount" runat="server" ErrorMessage="<%$ Resources:lbl_Failed_Revised_Amt %>"></asp:RangeValidator>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:btn_Cancel %>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnNext" runat="server" Text="<%$ Resources:btn_Ok %>" CausesValidation="true" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnFinish" runat="server" Text="<%$ Resources:btn_Finish %>" CausesValidation="true" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:CustomValidator ID="IsValidReserve" runat="server" Display="none"></asp:CustomValidator>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>

    </div>
</asp:Content>
