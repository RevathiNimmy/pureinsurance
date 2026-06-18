<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProspectPolicies.aspx.vb"
    Inherits="Nexus.Modal_ProspectPolicies" MasterPageFile="~/default.master" %>

<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">

        function UpdateProspectPolicyData() {
            var ProspectPolicyData;
            var oDDL;
            //to Fire teh Client Validation first
            Page_ClientValidate();

            if (Page_IsValid == true) {
                //Mode
                ProspectPolicyData = document.getElementById('<%=txtMode.ClientID %>').value + ";";
                //POlicy
                oDDL = document.getElementById('<%=GISPolicy.ClientID %>');
                ProspectPolicyData += oDDL.options[oDDL.selectedIndex].value + ";";
                //Date renewal
                ProspectPolicyData += document.getElementById('<%=DATE_RENEWAL.ClientID %>').value + ";";
                //Quote
                ProspectPolicyData += document.getElementById('<%=txtQuote.ClientID %>').value + ";";
                //Premium
                ProspectPolicyData += document.getElementById('<%=txtPremium.ClientID %>').value + ";";
                //Key
                ProspectPolicyData += document.getElementById('<%=txtKey.ClientID %>').value + ";";

                self.parent.tb_remove();
                self.parent.ReceiveProspectPolicyData(ProspectPolicyData, document.getElementById('<%=txtPostBackTo.ClientID %>').value);
            }
        }
    </script>

    <div id="Modal_ProspectPolicies">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_ProspectPolicies_Title %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblType" runat="server" AssociatedControlID="GISPolicy" Text="<%$ Resources:lbl_PnlPPolicies_Type %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="GISPolicy" runat="server" DataItemValue="Code" DataItemText="Description" CssClass="field-medium field-mandatory form-control form-select" Sort="ASC" ListType="PMLookup" ListCode="risk_group" DefaultText="(Please Select)"></NexusProvider:LookupList>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdPolicy" runat="server" ControlToValidate="GISPolicy" ErrorMessage="<%$ Resources:lbl_ErrMsg_Type %>" Display="none" SetFocusOnError="true" Enabled="true" InitialValue="" ValidationGroup="ProspectPolicyGroup"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRenewalDate" runat="server" Text="<%$ Resources:lbl_RenewalDate %>" AssociatedControlID="DATE_RENEWAL" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="DATE_RENEWAL" runat="server" CssClass="field-mandatory form-control"></asp:TextBox><uc1:CalendarLookup ID="calRenewalDate" runat="server" LinkedControl="DATE_RENEWAL" HLevel="1"></uc1:CalendarLookup>
                            </div>
                        </div>

                        <asp:RequiredFieldValidator ID="reqvldRenewalDate" Display="None" ControlToValidate="DATE_RENEWAL" runat="server" ErrorMessage="<%$ Resources:lbl_Date %>" SetFocusOnError="True" ValidationGroup="ProspectPolicyGroup"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cmpvldRenewalDate" runat="server" ErrorMessage="<%$ Resources:lbl_RenewalDateFormat %>" ControlToValidate="DATE_RENEWAL" Enabled="True" SetFocusOnError="true" Type="Date" Operator="DataTypeCheck" ValidationGroup="ProspectPolicyGroup" Display="None"></asp:CompareValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblTimesQuoted" runat="server" AssociatedControlID="txtQuote" Text="<%$ Resources:lbl_PnlPPolicies_TimesQuoted %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox runat="server" ID="txtQuote" CssClass="form-control" MaxLength="10"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdQuote" runat="server" ControlToValidate="txtQuote" ErrorMessage="<%$ Resources:lbl_ErrMsg_Quote %>" Display="none" SetFocusOnError="true" Enabled="false"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RngQuote" runat="server" MinimumValue="0" MaximumValue="" Display="None" ErrorMessage="<%$ Resources:lbl_RngErrMsg_Quote %>" Type="Double" ControlToValidate="txtQuote" ValidationGroup="ProspectPolicyGroup"></asp:RangeValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPremium" runat="server" AssociatedControlID="txtPremium" Text="<%$ Resources:lbl_PnlPPolicies_Premium %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox runat="server" ID="txtPremium" CssClass="form-control" MaxLength="10"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdPremium" runat="server" ControlToValidate="txtPremium" ErrorMessage="<%$ Resources:lbl_ErrMsg_Premium %>" Display="none" SetFocusOnError="true" Enabled="false"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="RngPremium" runat="server" MinimumValue="0" MaximumValue="" Display="None" ErrorMessage="<%$ Resources:lbl_RngErrMsg_Premium %>" Type="Double" ControlToValidate="txtPremium" ValidationGroup="ProspectPolicyGroup"></asp:RangeValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAddProspectPolicy" runat="server" AssociatedControlID="btnAddPolicy" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="card-footer">

                <asp:LinkButton ID="btnUpdatePolicy" runat="server" Text="<%$ Resources:lbl_btnUpdatePolicy %>" Visible="false" ValidationGroup="ProspectPolicyGroup" CausesValidation="true" OnClientClick="UpdateProspectPolicyData()" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnAddPolicy" runat="server" Text="<%$ Resources:lbl_btnAddPolicy %>" ValidationGroup="ProspectPolicyGroup" CausesValidation="true" OnClientClick="UpdateProspectPolicyData()" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:HiddenField ID="txtPostBackTo" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtMode" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="txtKey" runat="server"></asp:HiddenField>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" ValidationGroup="ProspectPolicyGroup" CssClass="validation-summary"></asp:ValidationSummary>

    </div>
</asp:Content>
