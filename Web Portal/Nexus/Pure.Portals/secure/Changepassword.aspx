<%@ Page Language="VB" MasterPageFile="~/Default.master" AutoEventWireup="false"
    CodeFile="Changepassword.aspx.vb" Inherits="Nexus.secure_Changepassword" Title="Untitled Page" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="secure_Changepassword">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblHeading" runat="server" Text="<%$ Resources:ChangePassword_heading %>"></asp:Literal>
                    <asp:Literal ID="lblHeadingInfo" runat="server" Text="<%$ Resources:ChangePassword_headingInfo %>"></asp:Literal>
                </h1>
            </div>
            <asp:Panel ID="pnlChangePasswordWarning" runat="server" Visible="false">
                <h3>
                    <asp:Label ID="lblChangePasswordForcedReason" runat="server"></asp:Label>
                </h3>
            </asp:Panel>
            <asp:Panel ID="PnlPassword" runat="server">
                <div class="card-body clearfix">
                    <div class="form-horizontal">
                        <legend><span>
                            <asp:Literal ID="lbl_legend_header" runat="server" Text="<%$ Resources:lbl_Legend_header %>"></asp:Literal></span></legend>
                        <div class="form-group form-group-sm col-lg-12 col-md-12 col-sm-12">
                            <asp:Label ID="lblOldPassword" runat="server" AssociatedControlID="txtOldPassword" CssClass="col-md-2 col-sm-3 control-label" Text="<%$ Resources:lbl_OldPassword %>"></asp:Label>
                            <div class="col-md-4 col-sm-6">
                                <asp:TextBox ID="txtOldPassword" runat="server" TextMode="password" TabIndex="1" CssClass="form-control field-mandatory"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="vldrqdOldPassword" runat="server" ControlToValidate="txtOldPassword" Display="None" ErrorMessage="<%$ Resources:lbl_OldPassword %>" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-12 col-md-12 col-sm-12">
                            <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" CssClass="col-md-2 col-sm-3 control-label" Text="<%$ Resources:lbl_NewPassword %>"></asp:Label>
                            <div class="col-md-4 col-sm-6">
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="password" TabIndex="2" CssClass="form-control field-mandatory"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="vldrqdPassword" runat="server" ControlToValidate="txtPassword" Display="None" ErrorMessage="<%$ Resources:lbl_NewPassword %>" SetFocusOnError="true" BorderStyle="Dotted"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="vldPasswordRegex" runat="server" Display="None" ControlToValidate="txtPassword" SetFocusOnError="True" ValidationExpression="" EnableClientScript="true"></asp:RegularExpressionValidator>
                            <asp:CompareValidator ID="vldcmpOldPassword" runat="server" ControlToValidate="txtPassword" ControlToCompare="txtOldPassword" Type="string" Operator="NotEqual" Display="None" EnableClientScript="true" ErrorMessage="<%$ Resources:lbl_OldNewSamePassword %>"></asp:CompareValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-12 col-md-12 col-sm-12">
                            <asp:Label ID="lblConfirmPassword" runat="server" AssociatedControlID="txtConfirmPassword" CssClass="col-md-2 col-sm-3 control-label" Text="<%$ Resources:lbl_ConfirmNewPassword %>"></asp:Label>
                            <div class="col-md-4 col-sm-6">
                                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="password" TabIndex="3" CssClass="form-control field-mandatory"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="vldrqdConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword" Display="None" ErrorMessage="<%$ Resources:lbl_ConfirmNewPassword %>" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="vldcmpPassword" runat="server" ControlToValidate="txtConfirmPassword" ControlToCompare="txtpassword" Type="string" Operator="Equal" Display="None" EnableClientScript="true" ErrorMessage="<%$ Resources:lbl_Success_PwdNotMatch %>"></asp:CompareValidator>
                        </div>
                    </div>
                </div>
                <div class="card-footer">
                    <asp:Literal ID="lblReview" runat="server" Text="<%$ Resources:lbl_Review %>"></asp:Literal>
                    <asp:LinkButton ID="btnSubmit" runat="server" SkinID="btnPrimary" Text="<%$ Resources:AppResources.strings, btn_Submit %>" TabIndex="4"></asp:LinkButton>
                </div>
                <%--no server or client code for this validator, as the status is set from the btnsubmit click and not clientside code can be written--%>
            </asp:Panel>
            <asp:Panel ID="PnlConfirm" runat="server" Visible="false">
                <h3>
                    <asp:Label ID="lblSuccessMessage" runat="server" Text="<%$ Resources:lbl_Success_PwdChange %>"></asp:Label></h3>
            </asp:Panel>
        </div>
        <asp:CustomValidator ID="vldPasswordChangeFailed" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_Failure_PwdChange %>"></asp:CustomValidator>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
