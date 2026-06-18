<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ProgressBar.ascx.vb"
    Inherits="Nexus.controls_ProgressBar" EnableViewState="false" %>
<div id="Controls_ProgressBar">
    <div class="step-nav">
        <ul class="nav nav-tabs">
            <asp:PlaceHolder ID="phldrQQ" runat="server">
                <li id="pnlProgress1" class="n0" runat="server" visible="false">
                    <asp:Panel ID="Panel1" runat="server">
                        <asp:Label ID="lblQuickQuote" runat="server" Text="<%$ Resources:lbl_QuickQuote %>"></asp:Label>
                    </asp:Panel>
                </li>
            </asp:PlaceHolder>
            <li id="pnlProgress2" class="n1" runat="server">
                <asp:Label ID="lblClientDetails" runat="server" Text="<%$ Resources:lbl_CustDetails %>"></asp:Label></li>
            <li id="pnlProgress3" class="n2" runat="server">
                <asp:Label ID="lblUnderwriting" runat="server" Text="<%$ Resources:lbl_UnderwritingQuestion %>"></asp:Label></li>
            <li id="pnlProgress4" class="n3" runat="server">
                <asp:Label ID="lblCoverSummary" runat="server" Text="<%$ Resources:lbl_Summary %>"></asp:Label></li>
            <li id="pnlProgress5" class="n4" runat="server" visible="false">
                <asp:Label ID="lblStatement" runat="server" Text="<%$ Resources:lbl_Imp_Statement %>"></asp:Label></li>
            <li id="pnlProgress6" class="n5" runat="server">
                <asp:Label ID="lblPayment" runat="server" Text="<%$ Resources:lbl_Payment %>"></asp:Label></li>
            <li id="pnlProgress7" class="n6" runat="server">
                <asp:Label ID="lblConfirmation" runat="server" Text="<%$ Resources:lbl_Confirmation %>"></asp:Label></li>
        </ul>
    </div>
</div>
