<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="CashListItem.aspx.vb" Inherits="Nexus.secure_payment_CashListItem" validateRequest="false" %>

<%@ Register Src="~/Controls/CashListItem.ascx" TagName="CashListItem" TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="secure_payment_CashListItem">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <uc1:CashListItem ID="PayNow_CashListItem" runat="server"></uc1:CashListItem>
    </div>
</asp:Content>
