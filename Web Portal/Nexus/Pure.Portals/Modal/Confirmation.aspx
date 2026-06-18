<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Confirmation.aspx.vb" Inherits="Nexus.Modal_Confirmation"
    MasterPageFile="~/default.master" EnableViewState="false" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="Modal_Confirmation">
        <div class="card">
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblConfirmation" runat="server" Text="<%$ Resources:lbl_Confirmation %>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblConfirmationText" runat="server" AssociatedControlID="txtConfirmation" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litConfirmation" runat="server" Text="<%$ Resources:lbl_ConfirmationText %>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtConfirmation" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RqdDecline_Comments" runat="server" InitialValue="" ValidationGroup="DeclineGroup" ControlToValidate="txtConfirmation" ErrorMessage="<%$ Resources:lbl_DeclineReason %>" Display="none" SetFocusOnError="true" Enabled="true"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnSubmit" runat="server" Text="<%$ Resources:btn_Submit %>" ValidationGroup="DeclineGroup" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_Decline_ValidationSummary %>" runat="server" ValidationGroup="DeclineGroup" CssClass="validation-summary"></asp:ValidationSummary>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="error" runat="server" ControlToValidate="txtConfirmation" ErrorMessage="<%$ Resources:lbl_ValidationSummary %>"></asp:RequiredFieldValidator>
    </div>
</asp:Content>
