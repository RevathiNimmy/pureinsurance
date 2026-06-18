<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master"
    CodeFile="MarkForCollection.aspx.vb" Inherits="secure_payment_MarkForCollection" %>

<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<asp:Content ID="cntScriptIncludes" ContentPlaceHolderID="ScriptIncludes" runat="server">
</asp:Content>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="secure_payment_MarkForCollection">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblPageHeader%>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <p>
                    <asp:Literal ID="litMessage" runat="server" Text="<%$ Resources:litMessage%>"></asp:Literal>
                </p>
            </div>
            <asp:Panel ID="pnlSubmitArea" class="card-footer" runat="server">
                <asp:LinkButton ID="btnCancel" runat="server" SkinID="btnPrimary" Text="<%$ Resources:btnCancel%>"></asp:LinkButton>
                <asp:LinkButton ID="btnOk" runat="server" SkinID="btnPrimary" Text="<%$ Resources:btnOk%>"></asp:LinkButton>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
