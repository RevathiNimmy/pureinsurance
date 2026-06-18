<%@ Control Language="VB" AutoEventWireup="false" CodeFile="NewAnonymousQuote.ascx.vb"
    Inherits="Nexus.Controls_NewAnonymousQuote" %>
<div id="NewAnonymousQuote-control">
    <div id="pnlNewQuote" runat="server">
        <asp:Label ID="lblSelectProduct" runat="server" AssociatedControlID="litSelectProduct">
            <asp:Literal ID="litSelectProduct" runat="server" Text="<%$ Resources:lblSelectProduct %>"></asp:Literal></asp:Label>
        <asp:DropDownList ID="ddlProductlst" runat="server" CssClass="field-medium form-select"></asp:DropDownList>
    </div>
    <asp:Button ID="btnNewQuote" runat="server" Text="<%$ Resources:btnNewQuote %>" CssClass="submit"></asp:Button>
</div>
