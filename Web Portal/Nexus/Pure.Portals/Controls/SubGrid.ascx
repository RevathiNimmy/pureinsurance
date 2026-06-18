<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SubGrid.ascx.vb" Inherits="Controls_SubGrid" %>
<div id="Controls_SubGrid">
    <%--<asp:Label runat="server" ID="lblQuoteRef" />--%>
    <asp:GridView ID="grdQuoteReferences" runat="server" AutoGenerateColumns="false" DataKeyNames="InsuranceFolderKey" GridLines="None" CellPadding="0" CellSpacing="0" PageSize="10" AllowPaging="true" AllowSorting="true" PagerSettings-Mode="Numeric" EmptyDataRowStyle-CssClass="noData">
        <Columns>
            <asp:BoundField DataField="Reference" SortExpression="Reference" HeaderText="abc" ItemStyle-CssClass="span-2"></asp:BoundField>
            <asp:BoundField DataField="InsuranceFileTypeCode" SortExpression="InsuranceFileTypeCode" HeaderText="def" ItemStyle-CssClass="span-3"></asp:BoundField>
        </Columns>
    </asp:GridView>
</div>
