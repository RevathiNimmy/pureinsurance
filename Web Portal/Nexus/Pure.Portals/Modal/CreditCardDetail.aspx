<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CreditCardDetail.aspx.vb"
    Inherits="Nexus.Modal_CreditCardDetail" MasterPageFile="~/default.master" %>

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

                //CareNumber
                BankData += document.getElementById('<%=txtCardNo.ClientID %>').value + ";";

                //NameonCard
                BankData += document.getElementById('<%=txtNameOnCard.ClientID %>').value + ";";

                //ExpiryDate
                BankData += document.getElementById('<%=txtExpiryDate.ClientID %>').value + ";";

                //StartDate
                BankData += document.getElementById('<%=txtStartDate.ClientID %>').value + ";";

                //IssueNumber
                BankData += document.getElementById('<%=txtIssueNumber.ClientID %>').value + ";";

                //AuthenticationCode
                BankData += document.getElementById('<%=txtAuthCode.ClientID %>').value + ";";

                //ManualAuthentication
                BankData += document.getElementById('<%=txtManualAuth.ClientID %>').value + ";";

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
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblBankPaymentType" runat="server" AssociatedControlID="GISNBankPaymentType" Text="<%$ Resources:lbl_BankPaymentType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <%--<NexusProvider:LookupList ID="GISNBankPaymentType" runat="server" DataItemValue="Code"
                                    DataItemText="Description" Sort="Asc" ListType="PMLookup" ListCode="Bank_Payment_Type"
                                    DefaultText="<%$ Resources:lbl_DefaultText %>" CssClass="field-medium field-mandatory" />--%>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="GISNBankPaymentType" runat="server" CssClass="field-medium field-mandatory form-control form-select"></asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdBankPayment_Type" runat="server" InitialValue="" ControlToValidate="GISNBankPaymentType" ErrorMessage="<%$ Resources:rqderrBankPaymentType %>" Display="none" SetFocusOnError="true" ValidationGroup="BankDetailGroup" Enabled="true"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAccountHolderName" runat="server" AssociatedControlID="txtAccountHolderName" Text="<%$ Resources:lbl_AccountHolderName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtAccountHolderName" runat="server" CssClass="field-mandatory form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rqdAccountHolderName" runat="server" InitialValue="" ControlToValidate="txtAccountHolderName" ErrorMessage="<%$ Resources:rqderrAccountHolderName %>" Display="none" SetFocusOnError="true" ValidationGroup="BankDetailGroup" Enabled="true"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAccountType" runat="server" AssociatedControlID="txtAccountType" Text="<%$ Resources:lbl_AccountType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtAccountType" runat="server" CssClass="field-mandatory form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rqdAccountType" runat="server" InitialValue="" ControlToValidate="txtAccountType" ErrorMessage="<%$ Resources:rqderrAccountType %>" Display="none" SetFocusOnError="true" ValidationGroup="BankDetailGroup" Enabled="true"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCardNo" runat="server" AssociatedControlID="txtCardNo" Text="<%$ Resources:lbl_CardNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtCardNo" runat="server" CssClass="field-mandatory form-control" onkeypress="javascript:return isInteger(event);"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rqdCardNo" runat="server" InitialValue="" ControlToValidate="txtCardNo" ErrorMessage="<%$ Resources:rqderrAccountNumber %>" Display="none" SetFocusOnError="true" ValidationGroup="BankDetailGroup" Enabled="true"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblNameOnCard" runat="server" AssociatedControlID="txtNameOnCard" Text="<%$ Resources:lblNameOnCard %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtNameOnCard" runat="server" CssClass="field-mandatory form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rqdNameonCard" runat="server" InitialValue="" ControlToValidate="txtNameOnCard" ErrorMessage="<%$ Resources:rqderrBankBranchCode %>" Display="none" SetFocusOnError="true" ValidationGroup="BankDetailGroup" Enabled="true"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblExpiryDate" runat="server" AssociatedControlID="txtExpiryDate" Text="<%$ Resources:lblExpiryDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblStartDate" runat="server" AssociatedControlID="txtStartDate" Text="<%$ Resources:lblStartDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblIssueNumber" runat="server" AssociatedControlID="txtIssueNumber" Text="<%$ Resources:lblIssueNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtIssueNumber" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAuthCode" runat="server" AssociatedControlID="txtCSVPin" Text="<%$ Resources:lblAuthCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtAuthCode" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblManualAuth" runat="server" AssociatedControlID="txtManualAuth" Text="<%$ Resources:lblManualAuth %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtManualAuth" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCSVPin" runat="server" AssociatedControlID="txtCSVPin" Text="<%$ Resources:lblCSVPin %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtCSVPin" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblStreet" runat="server" AssociatedControlID="txtStreet" Text="<%$ Resources:lbl_Street %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtStreet" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblLocality" runat="server" AssociatedControlID="txtLocality" Text="<%$ Resources:lbl_Locality %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtLocality" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPostTown" runat="server" AssociatedControlID="txtPostTown" Text="<%$ Resources:lbl_PostTown %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPostTown" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCounty" runat="server" AssociatedControlID="txtCounty" Text="<%$ Resources:lbl_County %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtCounty" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPostCode" runat="server" AssociatedControlID="txtPostCode" Text="<%$ Resources:lbl_PostCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtPostCode" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCountry" runat="server" AssociatedControlID="GISCountry" Text="<%$ Resources:lbl_Country %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="GISCountry" runat="server" DataItemValue="code" DataItemText="Description" Sort="Asc" ListType="PMLookup" ListCode="Country" DefaultText="<%$ Resources:lbl_DefaultText %>" CssClass="field-medium form-control form-select"></NexusProvider:LookupList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnAddCreditCard" runat="server" Text="<%$ Resources:btnAdd %>" ValidationGroup="BankDetailGroup" CausesValidation="true" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnUpdateCreditCard" runat="server" Text="<%$ Resources:btnUpdate %>" Visible="false" ValidationGroup="BankDetailGroup" CausesValidation="true" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_Address_ValidationSummary %>" runat="server" ValidationGroup="BankDetailGroup" CssClass="validation-summary"></asp:ValidationSummary>
        <asp:CustomValidator ID="CustVldValidateAccountNumber" runat="server" Display="None" ValidationGroup="BankDetailGroup" OnServerValidate="CustVldValidateAccountNumber_ServerValidate"></asp:CustomValidator>
        <asp:CustomValidator ID="CustVldValidateAccountType" runat="server" Display="None" ValidationGroup="BankDetailGroup" OnServerValidate="CustVldValidateAccountNumber_ServerValidate"></asp:CustomValidator>
        <asp:HiddenField ID="hidBankMediaCode" runat="server" Value="<%$ Resources:hidBankMediaCode %>"></asp:HiddenField>
        <asp:HiddenField ID="txtMode" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtBankKey" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtAccountCode" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtPostBackTo" runat="server"></asp:HiddenField>
    </div>
</asp:Content>
