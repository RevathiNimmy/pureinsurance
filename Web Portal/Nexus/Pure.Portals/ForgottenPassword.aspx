<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Default.master"
    CodeFile="ForgottenPassword.aspx.vb" Inherits="Nexus.ForgottenPassword" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="ForgottenPassword">
        
    
            
        
                
                
            
                    
                    <asp:Panel ID="pnlForgottenPassword" runat="server" DefaultButton="btnSubmit" CssClass="card">
                        <div class="card-heading"><h1>
                            <asp:Literal ID="lblForgottenPasswordHeader" runat="server" Text="<%$ Resources: lbl_ForgottenPassword_Header %>" EnableViewState="false"></asp:Literal></h1></div>
                        <div class="card-body clearfix">
                            
                            <div class="form-horizontal">
                                <legend>
                                    <asp:Literal ID="lblPageheader" runat="server" Text="<%$ Resources:lbl_Page_header %>"></asp:Literal>
                                </legend>
                                
                            
                                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:Label ID="lblUserName" runat="server" AssociatedControlID="txtUserName" Text="<%$ Resources:lbl_UserName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtUserName" runat="server" TabIndex="1" CssClass="field-mandatory form-control"></asp:TextBox></div>
                                        <asp:RequiredFieldValidator ID="vldUserName" runat="server" ControlToValidate="txtUserName" Display="none" ErrorMessage="<%$ Resources:lbl_User_err %>" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                        </div>
                        
                    </asp:Panel>
                    <asp:Panel ID="pnlConfirmation" runat="server" Visible="false">
                        <h2>
                            <asp:Label ID="LblEmailConfirmation" runat="server" Text="<%$ Resources:lbl_Email_Confirmation %>"></asp:Label></h2>
                    </asp:Panel>
                    <asp:CustomValidator ID="custvldUserExists" runat="server" Display="None"></asp:CustomValidator>
                    <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
                    <div id="divSubmitArea" runat="server" class="submitarea reviewtxt">
                        <p class="left">
                            <asp:Literal ID="lblReview" runat="server" Text="<%$ Resources:lbl_Review %>"></asp:Literal></p>
                        <asp:Button ID="btnSubmit" CssClass="submit" TabIndex="2" runat="server" Text="<%$ Resources:AppResources.strings, btn_Submit %>"></asp:Button>
                    </div>
                </div>
</asp:Content>
