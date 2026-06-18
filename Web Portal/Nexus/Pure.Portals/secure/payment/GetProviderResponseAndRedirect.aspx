<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="GetProviderResponseAndRedirect.aspx.vb" Inherits="Nexus.secure_payment_GetProviderResponseAndRedirect" Title="Untitled Page" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<%@ Register Src="~/controls/AddressCntrl.ascx" TagName="AddressCntrl" TagPrefix="uc2" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="smInstalments" runat="server">
    </asp:ScriptManager>
    <uc3:ProgressBar ID="ucProgressBar" runat="server"></uc3:ProgressBar>
    <div id="secure_payment_CardPaymentViaPaymentHub">
       
    </div>
</asp:Content>
