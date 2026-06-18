<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CorporateClient.ascx.vb"
    Inherits="Nexus.Controls_CorporateClient" %>
<%@ Register Src="AddressCntrl.ascx" TagName="AddressCntrl" TagPrefix="uc2" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<div id="Controls_CorporateClient">
    <div class="corporate-client">
        <div class="card">
            <div class="card-body clearfix">
                
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblHeading" runat="server" Text="<%$ Resources:lbl_CompDetails_header %>"></asp:Label></legend>
                    
                
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblCompanyName" runat="server" AssociatedControlID="txtCompanyName" Text="<%$ Resources:lbl_CompanyName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtCompanyName" runat="server" CssClass="field-mandatory form-control"></asp:TextBox></div>
                            <asp:RequiredFieldValidator ID="vldCompName" runat="server" Display="none" ControlToValidate="txtCompanyName" ErrorMessage="<%$ Resources:lbl_ErrMsg_companyname %>" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblMainContact" runat="server" AssociatedControlID="txtMainContact" Text="<%$ Resources:lbl_MainContact %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtMainContact" runat="server" CssClass="form-control"></asp:TextBox></div>
                            <asp:RequiredFieldValidator ID="vldMainContact" runat="server" Display="none" ControlToValidate="txtMainContact" EnableClientScript="false" ErrorMessage="<%$ Resources:lbl_ErrMsg_maincontact %>" SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cusvldAddress" runat="server" Display="None" ValidationGroup="CorporateClientGroup"></asp:CustomValidator>
                        </div>
                        <div id="liFileCode" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblFileCode" runat="server" AssociatedControlID="txtFileCode" class="col-md-4 col-sm-3 control-label">
                                <asp:Literal ID="ltFileCode" runat="server" Text="<%$ Resources:lbl_FileCode %>"></asp:Literal>
                            </asp:Label>
                            <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtFileCode" runat="server" CssClass="form-control"></asp:TextBox></div>
                        </div>
                    </div>
            </div>
            
        </div>
        <uc2:AddressCntrl ID="CorpCtrlAddress" runat="server"></uc2:AddressCntrl>
        <div class="card">
            <div class="card-body clearfix">
                
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblContactHeading" runat="server" Text="<%$ Resources:lbl_ContactDetails_header %>"></asp:Label></legend>
                    <ol>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblTelephone" runat="server" AssociatedControlID="txtTelephone" Text="<%$ Resources:lbl_Telephone %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtTelephone" runat="server" CssClass="form-control"></asp:TextBox></div>
                            <asp:RequiredFieldValidator ID="vldTelephoneRequired" runat="server" Display="none" ControlToValidate="txtTelephone" ErrorMessage="<%$ Resources:lbl_ErrMsg_telephone %>" SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" Text="<%$ Resources:lbl_EmailAddress %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox></div>
                            <asp:RequiredFieldValidator ID="vldEmailRequired" runat="server" ControlToValidate="txtEmail" Display="none" ErrorMessage="E-Mail Address" SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="vldEmailRegex" runat="server" ControlToValidate="txtEmail" Display="none" ErrorMessage="<%$ Resources:lbl_ErrMsg_email %>" SetFocusOnError="True" ValidationExpression="^[\w-]+(?:\.[\w-]+)*@(?:[\w-]+\.)+[a-zA-Z]{2,7}$" Enabled="false">Invalid</asp:RegularExpressionValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Panel ID="pnlConfirmEmail" runat="server" Visible="False">
                                <asp:Label ID="lblEmailAddressConfirm" runat="server" AssociatedControlID="txtConfirmEmail" Text="<%$ Resources:lbl_ConfirmEmailAddress %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtConfirmEmail" runat="server" CssClass="text form-control"></asp:TextBox></div>
                                <asp:RequiredFieldValidator ID="vldConfirmEmailRequired" runat="server" ControlToValidate="txtConfirmEmail" Display="none" ErrorMessage="<%$ Resources:lbl_ConfirmEmailAddress %>" SetFocusOnError="True" Enabled="false"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="vldEmailCompare" runat="server" ControlToCompare="txtEmail" ControlToValidate="txtConfirmEmail" Display="none" ErrorMessage="<%$ Resources:lbl_ConfirmEmailAddressInvalid %>" SetFocusOnError="True" Enabled="false">Email's must match</asp:CompareValidator>
                            </asp:Panel>
                        </div>
                    </ol>
                </div>
            </div>
            
        </div>
    </div>
</div>
