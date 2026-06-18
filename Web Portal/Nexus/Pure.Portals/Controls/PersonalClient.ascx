<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PersonalClient.ascx.vb"
    Inherits="Nexus.Controls_PersonalClient" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register Src="~/controls/AddressCntrl.ascx" TagName="AddressCntrl" TagPrefix="uc2" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<div id="Controls_PersonalClient">
    <div class="card">
        <div class="card-body clearfix">
            
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblHeading" runat="server" Text="<%$ Resources:lbl_Heading %>"></asp:Label></legend>
                
            
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblTitle" runat="server" AssociatedControlID="ddlTitle" Text="<%$ Resources:lbl_Title %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9"><NexusProvider:LookupList ID="ddlTitle" runat="server" DataItemValue="description" DataItemText="description" ListType="UserDefined" ListCode="131085" DefaultText="Please Select" CssClass="field-medium field-mandatory form-control form-select"></NexusProvider:LookupList></div>
                        <asp:RequiredFieldValidator ID="vldTitleRequired" runat="server" Display="none" ControlToValidate="ddlTitle" ErrorMessage="<%$ Resources:lbl_Title %>" InitialValue="" SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblFirstName" runat="server" AssociatedControlID="txtFirstName" Text="<%$ Resources:lbl_FirstName%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtFirstName" runat="server" CssClass="field-mandatory form-control"></asp:TextBox></div>
                        <asp:RequiredFieldValidator ID="vldFirstNameRequired" runat="server" Display="none" ControlToValidate="txtFirstName" ErrorMessage="<%$ Resources:lbl_FirstName %>" SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblSurname" runat="server" AssociatedControlID="txtSurname" Text="<%$ Resources:lbl_Surname %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtSurname" runat="server" CssClass="field-mandatory form-control"></asp:TextBox></div>
                        <asp:RequiredFieldValidator ID="vldSurnameRequired" runat="server" Display="none" ControlToValidate="txtSurname" ErrorMessage="<%$ Resources:lbl_Surname %>" SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblDateOfBirth" runat="server" AssociatedControlID="txtDOB" Text="<%$ Resources:lbl_DOB %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9"><div class="input-group"><asp:TextBox ID="txtDOB" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calDateOfBirth" runat="server" LinkedControl="txtDOB" HLevel="1"></uc1:CalendarLookup></div></div>
                        
                        <asp:RequiredFieldValidator ID="vldDateOfBirthRequired" Display="None" ControlToValidate="txtDOB" runat="server" ErrorMessage="<%$ Resources:lbl_ReqDOB %>" SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="custrngvldDateOfBirth" runat="server" Display="None" Enabled="true" ControlToValidate="txtDOB" SetFocusOnError="true"></asp:CustomValidator>
                    </div>
                    <div id="liFileCode" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblFileCode" runat="server" AssociatedControlID="txtFileCode" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="ltFileCode" runat="server" Text="<%$ Resources:lbl_FileCode %>"></asp:Literal>
                        </asp:Label><div class="col-md-8 col-sm-9"><asp:TextBox ID="txtFileCode" runat="server" CssClass="form-control"></asp:TextBox></div>
                    </div>
                </div>
        </div>
        
    </div>
    <uc2:AddressCntrl ID="PersonalCtrlAddress" runat="server"></uc2:AddressCntrl>
    <div class="card">
        <div class="card-body clearfix">
            
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblContactHeading1" runat="server" Text="<%$ Resources:lbl_Contact_heading %>"></asp:Label></legend>
                <ol>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblTelephone" runat="server" AssociatedControlID="txtTelephone" Text="<%$ Resources:lbl_Telephone %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtTelephone" runat="server" CssClass="form-control"></asp:TextBox></div>
                        <asp:RequiredFieldValidator ID="vldTelephoneRequired" Display="none" runat="server" ControlToValidate="txtTelephone" ErrorMessage="<%$ Resources:lbl_Telephone %>" SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblEmailAddress" runat="server" AssociatedControlID="txtEmail" Text="<%$ Resources:lbl_EmailAddress %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox></div>
                        <asp:RequiredFieldValidator ID="vldEmailRequired" runat="server" ControlToValidate="TxtEmail" Display="none" ErrorMessage="<%$ Resources:lbl_EmailAddress %>" SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="vldEmailRegex" runat="server" ControlToValidate="TxtEmail" Display="none" ErrorMessage="<%$ Resources:lbl_InvalidEmailAddress %>" SetFocusOnError="True" ValidationExpression="^[\w-]+(?:\.[\w-]+)*@(?:[\w-]+\.)+[a-zA-Z]{2,7}$" Enabled="false">Invalid</asp:RegularExpressionValidator></div><div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <asp:Panel ID="pnlConfirmEmail" runat="server" Visible="False">
                                    <asp:Label ID="lblEmailAddressConfirm" runat="server" AssociatedControlID="txtConfirmEmail" Text="<%$ Resources:lbl_ConfirmEmailAddress %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtConfirmEmail" runat="server" CssClass="form-control"></asp:TextBox></div>
                                    <asp:RequiredFieldValidator ID="vldConfirmEmailRequired" runat="server" ControlToValidate="txtConfirmEmail" Display="none" ErrorMessage="<%$ Resources:lbl_ConfirmEmailAddress %>" SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="vldConfirmEmailRegex" runat="server" ControlToValidate="txtConfirmEmail" Display="none" ErrorMessage="<%$ Resources:lbl_InvalidConfirmEmailAddress %>" SetFocusOnError="True" ValidationExpression="^[\w-]+(?:\.[\w-]+)*@(?:[\w-]+\.)+[a-zA-Z]{2,7}$" Enabled="false">Invalid</asp:RegularExpressionValidator>
                                    <asp:CompareValidator ID="vldEmailCompare" runat="server" ControlToCompare="txtEmail" ControlToValidate="txtConfirmEmail" Display="none" ErrorMessage="<%$ Resources:lbl_ConfirmEmailAddressInvalid %>" SetFocusOnError="True" Enabled="false">Email's must match</asp:CompareValidator></asp:Panel>
                            </div>
                </ol>
            </div>
        </div>
        
    </div>
</div>
