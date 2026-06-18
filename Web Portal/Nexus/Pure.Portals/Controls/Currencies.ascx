<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Currencies.ascx.vb" Inherits="Nexus.Controls_Currencies" %>
<div id="Controls_Currencies">
    <asp:UpdatePanel ID="CurrencyUpdate" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:DropDownList ID="ddlCurrencylst" AutoPostBack="true" runat="server" CssClass=" form-select"></asp:DropDownList>
        </ContentTemplate>
    </asp:UpdatePanel>
    <nexus:ProgressIndicator ID="upCurrencies" OverlayCssClass="updating" AssociatedUpdatePanelID="CurrencyUpdate" runat="server">
        <ProgressTemplate>
        </ProgressTemplate>
    </nexus:ProgressIndicator>
</div>
