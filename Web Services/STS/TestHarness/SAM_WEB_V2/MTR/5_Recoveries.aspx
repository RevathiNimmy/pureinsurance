<%@ Page Language="VB" AutoEventWireup="false" CodeFile="5_Recoveries.aspx.vb" Inherits="OpenClaim_Recoveries" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
            ForeColor="#C000C0" Text="Salvage and Third Party Recoveries" Width="256px"></asp:Label><br />
        <br />
        <br />
        <strong>Perils</strong><br />
        <asp:GridView ID="gvPerils" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
            </Columns>
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <EditRowStyle BackColor="#999999" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        <br />
        <br />
        <asp:Button ID="btnSalvage" runat="server" Text="Show Recoveries" /><br />
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
        <br />
        <br />
        <asp:GridView ID="gvRecoveries" runat="server" CellPadding="4" ForeColor="#333333"
            GridLines="None">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <EditRowStyle BackColor="#999999" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:CommandField ShowEditButton="True" />
            </Columns>
        </asp:GridView>
    
    </div>
    <hr />
        <asp:Button ID="btnOk" runat="server" Text="Ok" />
        <br />
        <br />
        <br />
        <br />
        Claim Number :
        <asp:Label ID="lblClaimNumber" runat="server" Width="280px"></asp:Label><br />
        Claim Key: &nbsp; &nbsp; &nbsp;&nbsp;
        <asp:Label ID="lblClaimKey" runat="server" Width="280px"></asp:Label><br />
        <br />
        <br />
        <hr />
        <asp:Button ID="btnCoInsuranceBreakDown" runat="server" Text="CoInsurance BreakDown" /><br />
    </form>
</body>
</html>
