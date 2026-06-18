<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Contact.aspx.vb" Inherits="Nexus.Controls_Contact"
    MasterPageFile="~/default.master" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        $(document).ready(MainContactType);

        String.prototype.trim = function () {
            return this.replace(/^\s*/, "").replace(/\s*$/, "");
        }

        function UpdateContactData() {

            var ContactData;
            var oDDL;
            //to Fire teh Client Validation first
            Page_ClientValidate();

            if (Page_IsValid == true) {
                //Mode
                ContactData = document.getElementById('<%=txtMode.ClientID %>').value + ";";
                //ContactType
                oDDL = document.getElementById('<%=GISContactType.ClientID %>');
                ContactData += oDDL.options[oDDL.selectedIndex].value + ";";
                //Description
                ContactData += document.getElementById('<%=txtDescription.ClientID %>').value + ";";
                //AreaCode
                if (document.getElementById('<%=txtAreaCode.ClientID %>') != null) {
                    ContactData += document.getElementById('<%=txtAreaCode.ClientID %>').value + ";";
                }
                else {
                    ContactData += "" + ";";
                }
                //Number
                if (document.getElementById('<%=txtNumber.ClientID %>') != null) {
                    ContactData += document.getElementById('<%=txtNumber.ClientID %>').value + ";";
                }
                else {
                    ContactData += "" + ";";
                }
                //Extension
                if (document.getElementById('<%=txtExtension.ClientID %>') != null) {
                    ContactData += document.getElementById('<%=txtExtension.ClientID %>').value + ";";
                }
                else {
                    ContactData += "" + ";";
                }
                //Key
                ContactData += document.getElementById('<%=ContactKey.ClientID %>').value + ";";
                //Contact Type Description
                oDDL = document.getElementById('<%=GISContactType.ClientID %>');
                ContactData += oDDL.options[oDDL.selectedIndex].text + ";";

                self.parent.tb_remove();
                self.parent.ReceiveContactData(ContactData, document.getElementById('<%=txtPostBackTo.ClientID %>').value);
            }
        }

        //Removing 'Main Contact' from the dropdown list- Same as BO.
        //Main contact is added on 'Basic Details' tab- Same as BO. 
        function MainContactType() {
            var list = document.getElementById('<%=GISContactType.ClientID %>');
            if (list.options.length > 0) {
                for (var i = list.options.length - 1; i >= 0; i--) {
                    if (list.options[i].value == "MAIN") {
                        list.remove(i);
                    }
                }
            }
        }
    </script>

    <div id="Modal_Contact">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_Contact_Title %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblContact" runat="server" AssociatedControlID="GISContactType" Text="<%$ Resources:lbl_PnlContact_Contact %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="GISContactType" runat="server" DataItemValue="Code" DataItemText="Description" Sort="Asc" ListType="PMLookup" ListCode="Contact_type" DefaultText="(Please Select)" CssClass="field-medium field-mandatory form-control form-select" OnSelectedIndexChange="GISContactType_SelectedIndexChange" AutoPostBack="true"></NexusProvider:LookupList>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdContact_Type" runat="server" InitialValue="" ControlToValidate="GISContactType" ErrorMessage="<%$ Resources:lbl_ErrMsg_ContactType %>" Display="none" SetFocusOnError="true" ValidationGroup="ContactGroup" Enabled="true"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblDescription" runat="server" AssociatedControlID="TxtDescription" Text="<%$ Resources:lbl_PnlContact_Description %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div id="liAreaCode" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAreaCode" runat="server" AssociatedControlID="TxtAreaCode" Text="<%$ Resources:lbl_PnlContact_AreaCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtAreaCode" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Panel ID="divEmail" runat="server" Visible="false" CssClass="col-md-4 col-sm-3 control-label">

                            <asp:Label ID="lblEmail" runat="server" AssociatedControlID="TxtNumber" Text="E-Mail" class=" control-label lblContact"></asp:Label>
                            <asp:RegularExpressionValidator ID="vldEmailRegex" runat="server" ControlToValidate="txtNumber" Display="none" ErrorMessage="<%$ Resources:lbl_InvalidEmailAddress %>" SetFocusOnError="True" ValidationGroup="ContactGroup" Enabled="false"></asp:RegularExpressionValidator>
                        </asp:Panel>
                        <asp:Panel ID="divNumber" runat="server" CssClass="col-md-4 col-sm-3 control-label">
                            <asp:Label ID="lblNumber" runat="server" AssociatedControlID="TxtNumber" Text="<%$ Resources:lbl_PnlContact_Number %>" class=" lblContact"></asp:Label>

                            <asp:RegularExpressionValidator ID="vldNumberRegex" runat="server" ControlToValidate="txtNumber" Display="none" ErrorMessage="<%$ Resources:lbl_InvalidNumber %>" SetFocusOnError="True" ValidationGroup="ContactGroup" ValidationExpression="^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$"
                                Enabled="false" />
                        </asp:Panel>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtNumber" runat="server" CssClass="field-mandatory form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdContact_Number" runat="server" ControlToValidate="TxtNumber" ErrorMessage="<%$ Resources:lbl_ErrMsg_Number %>" Display="none" SetFocusOnError="true" Enabled="True" ValidationGroup="ContactGroup"></asp:RequiredFieldValidator>

                    </div>

                    <div id="liExtension" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblExtension" runat="server" AssociatedControlID="TxtExtension" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltExtension" runat="server" Text="<%$ Resources:lbl_PnlContact_Extension %>"></asp:Literal>
                        </asp:Label><div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtExtension" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <div class="card-footer">
            <%--<asp:Label ID="lblAddContacts" runat="server" AssociatedControlID="btnAddContacts" />--%>
            <asp:LinkButton ID="btnAddContacts" runat="server" Text="<%$ Resources:btn_Add %>" ValidationGroup="ContactGroup" CausesValidation="true" OnClientClick="UpdateContactData()" SkinID="btnPrimary" Style="margin-right: 25px;"></asp:LinkButton>
            <asp:LinkButton ID="btnUpdateContacts" runat="server" Text="<%$ Resources:btn_Update %>" Visible="false" ValidationGroup="ContactGroup" CausesValidation="true" OnClientClick="UpdateContactData()" SkinID="btnPrimary"></asp:LinkButton>
        </div>
    </div>
    <asp:HiddenField ID="txtPostBackTo" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="txtMode" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="ContactKey" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="ValidEmailRegex" Value="^[a-zA-Z0-9]+([_.-][a-zA-Z0-9]{1,})*@[a-zA-Z]+([-][a-zA-Z]{1,})*(\.[a-zA-Z-]+)*\.([a-zA-Z]{2,})$" runat="server"></asp:HiddenField>
    <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" ValidationGroup="ContactGroup" CssClass="validation-summary"></asp:ValidationSummary>
   
</asp:Content>
