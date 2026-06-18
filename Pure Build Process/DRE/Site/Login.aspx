<%@ Page 
    Language="C#" 
    AutoEventWireup="true" 
    MasterPageFile="~/MainMasterPage.master" 
    Inherits="Login" 
    Codebehind="Login.aspx.cs" 
%>
<%@ Register 
    Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI"
    TagPrefix="ajax" 
%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="sectionheader">
        <h2>Login</h2>
    </div> 
       
    <div class="loginDetails">
        <asp:Login  ID="LoginDRE" 
                    runat="server" 
                    DisplayRememberMe="False" 
                    OnAuthenticate="LoginDRE_Authenticate" 
                    DestinationPageUrl="~/Default.aspx" 
                    TitleText="Please login to proceed"
                    CssClass="loginBox" 
                    PasswordRecoveryText="Forgot Password?" 
                    PasswordRecoveryUrl="~/ForgotPassword.aspx">
             <TitleTextStyle CssClass="loginDetailsHeader" />
        
         </asp:Login>
        
        <ajax:TextBoxWatermarkExtender ID="tbwmUsername" runat="server" WatermarkText="Type Logon Name Here" WatermarkCssClass="watermark" />
        <%--<ajax:TextBoxWatermarkExtender ID="tbwmPassword" runat="server" WatermarkText="Type Password Here" WatermarkCssClass="watermark" />--%>
        
    </div>
    
</asp:Content>
