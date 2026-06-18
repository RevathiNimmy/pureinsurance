<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ClaimsProgressBar.ascx.vb"
    Inherits="Nexus.controls_ClaimsProgressBar" EnableViewState="false" %>
<div id="Controls_ClaimsProgressBar" class="clm-progress-bar">
    <ul class='nav nav-tabs'>
        <li id="liSearchResults" runat="server">
            <asp:Label ID="hypSearchResults" runat="server" Text="<%$ Resources:hypSearchResults %>"></asp:Label>
        </li>
        <li id="liOverview" runat="server">
            <asp:Label ID="hypOverview" runat="server" Text="<%$ Resources:hypOverview %>"></asp:Label>
        </li>
        <li id="liPerils" runat="server">
            <asp:Label ID="hypPerils" runat="server" Text="<%$ Resources:hypPerils %>"></asp:Label>
        </li>
        <li id="liSummary" runat="server">
            <asp:Label ID="hypSummary" runat="server" Text="<%$ Resources:hypSummary %>"></asp:Label>
        </li>
        <li id="liReinsurance" runat="server">
            <asp:Label ID="hypReinsurance" runat="server" Text="<%$ Resources:hypReinsurance %>"></asp:Label>
        </li>
        <li id="liComplete" runat="server">
            <asp:Label ID="hypComplete" runat="server" Text="<%$ Resources:hypComplete %>"></asp:Label>
        </li>
    </ul>
    <asp:PlaceHolder ID="phTmp" runat="server"></asp:PlaceHolder>
</div>
