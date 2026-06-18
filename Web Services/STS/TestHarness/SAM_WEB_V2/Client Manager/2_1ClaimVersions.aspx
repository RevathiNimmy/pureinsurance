<%@ Page Language="VB" AutoEventWireup="false" CodeFile="2_1ClaimVersions.aspx.vb"
    Inherits="View_Claim_1_2ClaimVersions" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="100%" style="height: 100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="height: 10%">
                    <td style="width: 10%" align="right">
                        <table width="100%">
                            <tr>
                                
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr style="height: 90%">
                    <td>
                        <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
                            ForeColor="#C000C0" Text="Claim Versions" Width="192px"></asp:Label><br />
                        <br />
                        <asp:GridView ID="gvClaimVersions" runat="server" CellPadding="4" ForeColor="#333333"
                            GridLines="None" Width="512px" AutoGenerateColumns="False">
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                            <EditRowStyle BackColor="#999999" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" />
                                <asp:BoundField DataField="Version" HeaderText="Version" />
                                <asp:BoundField DataField="Transactiondate" HeaderText="Transaction date" />
                                <asp:BoundField DataField="TransactionType" HeaderText="Transaction Type" />
                                <asp:BoundField DataField="VersionDescription" HeaderText="Version description" />
                                <asp:BoundField DataField="InsuranceRef" HeaderText="Policy No" />
                            </Columns>
                        </asp:GridView>
                        &nbsp;
                    </td>
                </tr>
            </table>
            <%-- <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
            ForeColor="#C000C0" Text="View Claim" Width="192px"></asp:Label><br />
        <br />
        <asp:GridView ID="gvClaimVersions" runat="server" CellPadding="4" ForeColor="#333333"
            GridLines="None" Width="512px" AutoGenerateColumns="False">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
            <EditRowStyle BackColor="#999999" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="Version" HeaderText="Version" />
                <asp:BoundField DataField="Transactiondate" HeaderText="Transaction date" />
                <asp:BoundField DataField="TransactionType" HeaderText="Transaction Type" />
                <asp:BoundField DataField="VersionDescription" HeaderText="Version description" />
                <asp:BoundField DataField="InsuranceRef" HeaderText="Policy No" />
            </Columns>
        </asp:GridView>
        &nbsp;--%>
        </div>
    </form>
</body>
</html>
