<%@ Page 
Language="C#" 
MasterPageFile="~/MainMasterPage.master" 
AutoEventWireup="true" 
Inherits="ForgotPassword" 
Title="Untitled Page" Codebehind="ForgotPassword.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="sectionheader"><h2>Forgot Password</h2></div>
    <div class="datamaintenance">
        <div class="forgotpassword">        
            <div class="forgotpasswordcontents">
                <asp:Label AssociatedControlID="lblErrorMessage" runat="Server" ID="lblErrorMessage" Text="Incorrent details. Please try again!" Visible="false" CssClass="error" />
                <asp:Label AssociatedControlID="lblMessage" runat="Server" ID="lblMessage" Text="Enter following details and new password will be sent to the given email address" />
                <div id="divDataEntry" runat="server">
                    <div class="question">
                        <asp:Label AssociatedControlID="tbLogon" runat="Server" ID="lblLogonName" Text="Logon Name:" />
                        <asp:TextBox ID="tbLogon" runat="server" MaxLength="255"/>
                        <ajax:TextBoxWatermarkExtender ID="tbwmUsername" TargetControlID="tbLogon" runat="server" WatermarkText="Type Logon Name Here" WatermarkCssClass="watermark" />
                        <div style="text-align:left;">
                            <asp:RequiredFieldValidator ID="rfvLogon" runat="server" ControlToValidate="tbLogon"
                                ValidationGroup="ForgotPasswordValidation" Display="Static" Text="*" ToolTip="Logon name is mandatory" />
                        </div>                                
                    </div>
                    <div class="question">
                        <asp:Label AssociatedControlID="tbEmail" runat="Server" ID="lblEmail" Text="Email:" />
                        <asp:TextBox ID="tbEmail" runat="server" MaxLength="255" />
                        <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" TargetControlID="tbEmail" runat="server" WatermarkText="Type Your Email Address Here" WatermarkCssClass="watermark" />
                        <div style="text-align:left;">
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                             ControlToValidate="tbEmail" Display="Static" Text="*" ToolTip="Email is mandatory" ValidationGroup="ForgotPasswordValidation" />                                                                      
                            <asp:RegularExpressionValidator ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" ID="revEmailAddress" runat="server"
                                ErrorMessage="Invalid Email Address" ToolTip="Invalid Email Address" Text="*" Display="Static" ControlToValidate="tbEmail" ValidationGroup="ForgotPasswordValidation" >                           
                            </asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class="buttons">
                        <asp:Button ID="btnResetPassword" runat="server" OnClick="btnResetPassword_Click" CausesValidation="true" ValidationGroup="ForgotPasswordValidation" Text="OK" />
                        <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CausesValidation="false" Text ="Cancel" />
                    </div>
                </div>
                <div id="divSuccess" class="buttons" runat="server">
                    <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" CausesValidation="false" Text="OK" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

