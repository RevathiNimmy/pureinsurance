<%@ Page Language="VB" AutoEventWireup="false" CodeFile="BankDetail.aspx.vb" Inherits="Nexus.Modal_BankDetail"
    MasterPageFile="~/default.master" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script type="text/javascript">

        function isInteger(e) {
            var key = window.event ? e.keyCode : e.which;
            var keychar = String.fromCharCode(key);
            reg = /\d/;
            return reg.test(keychar);
        }

        function confirmation(sender, args) {
            var answer = confirm('<%= GetLocalResourceObject("Con_PartyBankData").ToString() %>' + document.getElementById('<%=txtAccountType.ClientID %>').value)
            if (answer) {
                UpdatePartyBankData();
                return true;
            }
            else {
                return false;
            }
        }

        function isAlphaNumeric(e) {
            var key = window.event ? e.keyCode : e.which;
            var keychar = String.fromCharCode(key);
            reg = /^[a-z0-9]+$/i;
            return reg.test(keychar);
        }


        function UpdatePartyBankData() {

            var BankData;
            var oDDL;
            var oBankDDL
            var oCountryDDL; // for Country DropDown
            //to Fire the Client Validation first
            Page_ClientValidate();

            if (Page_IsValid == true) {

                //Mode
                BankData = document.getElementById('<%=txtMode.ClientID %>').value + ";";
                //Bank Payment Type
                oDDL = document.getElementById('<%=GISNBankPaymentType.ClientID %>');
                BankData += oDDL.options[oDDL.selectedIndex].value + ";";
                //AccountHolderName
                BankData += document.getElementById('<%=txtAccountHolderName.ClientID %>').value + ";";

                //AccountType
                BankData += document.getElementById('<%=txtAccountType.ClientID %>').value + ";";

                //AccountNumber
                BankData += document.getElementById('<%=txtAccountNo.ClientID %>').value + ";";

                //BankBranchCode
                BankData += document.getElementById('<%=txtBankBranchCode.ClientID %>').value + ";";

                //BankBranch
                BankData += document.getElementById('<%=txtBankBranch.ClientID %>').value + ";";

                //BankCode
                oBankDDL = document.getElementById('<%=GISLookup_BankList.ClientID %>');
                BankData += oBankDDL.options[oBankDDL.selectedIndex].value + ";";
                //BankName
                BankData += oBankDDL.options[oBankDDL.selectedIndex].innerText + ";";

                //Street
                BankData += document.getElementById('<%=txtStreet.ClientID %>').value + ";";
                //Locality
                BankData += document.getElementById('<%=txtLocality.ClientID %>').value + ";";
                //PostTown
                BankData += document.getElementById('<%=txtPostTown.ClientID %>').value + ";";
                //County
                BankData += document.getElementById('<%=txtCounty.ClientID %>').value + ";";
                //PostCode
                BankData += document.getElementById('<%=txtPostCode.ClientID %>').value + ";";
                //Country
                oCountryDDL = document.getElementById('<%=GISCountry.ClientID %>');
                BankData += oCountryDDL.options[oCountryDDL.selectedIndex].value + ";";

                //BIC
                BankData += document.getElementById('<%=txtBIC.ClientID%>').value + ";";

                //IBAN
                BankData += document.getElementById('<%=txtIBAN.ClientID %>').value + ";";

                //Key
                BankData += document.getElementById('<%=txtBankKey.ClientID %>').value + ";";

                self.parent.tb_remove();
                self.parent.ReceiveBankData(BankData, document.getElementById('<%=txtPostBackTo.ClientID %>').value);
            }
        }
    </script>

    <div id="Modal_BankDetail">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_Title %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-6">
                        <asp:Label ID="lblBankPaymentType" runat="server" AssociatedControlID="GISNBankPaymentType" Text="<%$ Resources:lbl_BankPaymentType %>" class="col-md-4  control-label"></asp:Label>
                        <%--<NexusProvider:LookupList ID="GISNBankPaymentType" runat="server" DataItemValue="Code"
                                    DataItemText="Description" Sort="Asc" ListType="PMLookup" ListCode="Bank_Payment_Type"
                                    DefaultText="<%$ Resources:lbl_DefaultText %>" CssClass="field-medium field-mandatory" />--%>
                        <div class="col-md-8">
                            <asp:DropDownList ID="GISNBankPaymentType" runat="server" CssClass="field-medium field-mandatory form-control form-select"></asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdBankPayment_Type" runat="server" InitialValue="" ControlToValidate="GISNBankPaymentType" ErrorMessage="<%$ Resources:rqderrBankPaymentType %>" Display="none" SetFocusOnError="true" ValidationGroup="BankDetailGroup" Enabled="true"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-6">
                        <asp:Label ID="lblAccountHolderName" runat="server" AssociatedControlID="txtAccountHolderName" Text="<%$ Resources:lbl_AccountHolderName %>" class="col-md-4  control-label"></asp:Label>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtAccountHolderName" runat="server" CssClass="field-mandatory form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rqdAccountHolderName" runat="server" InitialValue="" ControlToValidate="txtAccountHolderName" ErrorMessage="<%$ Resources:rqderrAccountHolderName %>" Display="none" SetFocusOnError="true" ValidationGroup="BankDetailGroup" Enabled="true"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-6">
                        <asp:Label ID="lblAccountType" runat="server" AssociatedControlID="txtAccountType" Text="<%$ Resources:lbl_AccountType %>" class="col-md-4  control-label"></asp:Label>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtAccountType" runat="server" CssClass="field-mandatory form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rqdAccountType" runat="server" InitialValue="" ControlToValidate="txtAccountType" ErrorMessage="<%$ Resources:rqderrAccountType %>" Display="none" SetFocusOnError="true" ValidationGroup="BankDetailGroup" Enabled="true"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-6">
                        <asp:Label ID="lblAcNo" runat="server" AssociatedControlID="txtAccountNo" Text="<%$ Resources:lbl_AccountNumber %>" class="col-md-4  control-label"></asp:Label>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtAccountNo" runat="server" CssClass="field-mandatory form-control" onkeypress="javascript:return isInteger(event);"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rqdAccountNo" runat="server" InitialValue="" ControlToValidate="txtAccountNo" ErrorMessage="<%$ Resources:rqderrAccountNumber %>" Display="none" SetFocusOnError="true" ValidationGroup="BankDetailGroup" Enabled="true"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-6">
                        <asp:Label ID="lblBankBranchCode" runat="server" AssociatedControlID="txtBankBranchCode" Text="<%$ Resources:lbl_BankBranchCode %>" class="col-md-4  control-label"></asp:Label>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtBankBranchCode" runat="server" CssClass="field-mandatory form-control" onkeypress="javascript:return isInteger(event);"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rqdBankBranchCode" runat="server" InitialValue="" ControlToValidate="txtBankBranchCode" ErrorMessage="<%$ Resources:rqderrBankBranchCode %>" Display="none" SetFocusOnError="true" ValidationGroup="BankDetailGroup" Enabled="true"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-6">
                        <asp:Label ID="lblBankBranch" runat="server" AssociatedControlID="txtBankBranch" Text="<%$ Resources:lbl_BankBranch %>" class="col-md-4  control-label"></asp:Label>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtBankBranch" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-6">
                        <asp:Label ID="lbtBankName" runat="server" AssociatedControlID="GISLookup_BankList" Text="<%$ Resources:lbl_BankName %>" class="col-md-4  control-label"></asp:Label>
                        <div class="col-md-8">
                            <NexusProvider:LookupList ID="GISLookup_BankList" runat="server" DataItemText="Description" DataItemValue="Code" DefaultText="<%$ Resources:lbl_DefaultText %>" ListCode="CashListItem_Bank" ListType="PMLookup" CssClass="field-medium field-mandatory form-control form-select"></NexusProvider:LookupList>
                        </div>
                        <asp:RequiredFieldValidator ID="rqdBankName" runat="server" InitialValue="" ControlToValidate="GISLookup_BankList" ErrorMessage="<%$ Resources:rqderrBank %>" Display="none" SetFocusOnError="true" ValidationGroup="BankDetailGroup" Enabled="true"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-6">
                        <asp:Label ID="lblBIC" runat="server" AssociatedControlID="txtBIC" Text="<%$ Resources:lbl_BIC %>" class="col-md-4  control-label"></asp:Label>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtBIC" runat="server" CssClass="form-control" MaxLength="50" onkeypress="javascript:return isAlphaNumeric(event);"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-6">
                        <asp:Label ID="lblIBAN" runat="server" AssociatedControlID="txtIBAN" Text="<%$ Resources:lbl_IBAN %>" class="col-md-4  control-label"></asp:Label>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtIBAN" runat="server" CssClass="form-control" MaxLength="50" onkeypress="javascript:return isAlphaNumeric(event);"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-6">
                        <asp:Label ID="lblStreet" runat="server" AssociatedControlID="txtStreet" Text="<%$ Resources:lbl_Street %>" class="col-md-4  control-label"></asp:Label>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtStreet" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-6">
                        <asp:Label ID="lblLocality" runat="server" AssociatedControlID="txtLocality" Text="<%$ Resources:lbl_Locality %>" class="col-md-4  control-label"></asp:Label>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtLocality" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-6">
                        <asp:Label ID="lblPostTown" runat="server" AssociatedControlID="txtPostTown" Text="<%$ Resources:lbl_PostTown %>" class="col-md-4  control-label"></asp:Label>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtPostTown" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-6">
                        <asp:Label ID="lblCounty" runat="server" AssociatedControlID="txtCounty" Text="<%$ Resources:lbl_County %>" class="col-md-4  control-label"></asp:Label>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtCounty" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-6">
                        <asp:Label ID="lblPostCode" runat="server" AssociatedControlID="txtPostCode" Text="<%$ Resources:lbl_PostCode %>" class="col-md-4  control-label"></asp:Label>
                        <div class="col-md-8">
                            <asp:TextBox ID="txtPostCode" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-6">
                        <asp:Label ID="lblCountry" runat="server" AssociatedControlID="GISCountry" Text="<%$ Resources:lbl_Country %>" class="col-md-4  control-label"></asp:Label>
                        <div class="col-md-8">
                            <NexusProvider:LookupList ID="GISCountry" runat="server" DataItemValue="code" DataItemText="Description" Sort="Asc" ListType="PMLookup" ListCode="Country" DefaultText="<%$ Resources:lbl_DefaultText %>" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnAddBank" runat="server" Text="<%$ Resources:btnAdd %>" ValidationGroup="BankDetailGroup" CausesValidation="true" SkinID="btnPrimary" style="margin-right:25px;"></asp:LinkButton>
                <asp:LinkButton ID="btnUpdateBank" runat="server" Text="<%$ Resources:btnUpdate %>" Visible="false" ValidationGroup="BankDetailGroup" CausesValidation="true" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_Address_ValidationSummary %>" runat="server" ValidationGroup="BankDetailGroup" CssClass="validation-summary"></asp:ValidationSummary>
        <asp:CustomValidator ID="CustVldValidateAccountNumber" runat="server" Display="None" ValidationGroup="BankDetailGroup" OnServerValidate="CustVldValidateAccountNumber_ServerValidate"></asp:CustomValidator>
        <asp:CustomValidator ID="CustVldValidateAccountType" runat="server" Display="None" ValidationGroup="BankDetailGroup" OnServerValidate="CustVldValidateAccountNumber_ServerValidate"></asp:CustomValidator>
        <asp:HiddenField ID="hidBankMediaCode" runat="server" Value="<%$ Resources:hidBankMediaCode %>"></asp:HiddenField>
        <asp:RegularExpressionValidator ID="revBIC" runat="server" Display="None" SetFocusOnError="true" ControlToValidate="txtBIC" ValidationExpression="^[a-zA-Z0-9]*$" ErrorMessage="<%$Resources:lbl_BICError %>" ValidationGroup="BankDetailGroup"></asp:RegularExpressionValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="None" SetFocusOnError="true" ControlToValidate="txtIBAN" ValidationExpression="^[a-zA-Z0-9]*$" ErrorMessage="<%$Resources:lbl_IBANError %>" ValidationGroup="BankDetailGroup"></asp:RegularExpressionValidator>
        <asp:HiddenField ID="txtMode" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtBankKey" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtAccountCode" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtPostBackTo" runat="server"></asp:HiddenField>

    </div>
</asp:Content>
