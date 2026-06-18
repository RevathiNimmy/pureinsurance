<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ViewAllocationDetails.aspx.vb" Inherits="MTA_ViewAllocationDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>   
    <table style="width: 472px">
    <tr>
                <td style="width: 530px">
                    &nbsp;<asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" Width="200px" Font-Bold="True" Font-Italic="True" Font-Overline="True" Font-Size="X-Large" ForeColor="Fuchsia" ToolTip="Click Me">
                        <Items>
                            <asp:MenuItem Text="Allocation Details" Value="Risk"></asp:MenuItem>
                        </Items>
                        <DynamicMenuItemStyle Font-Bold="True" />
                    </asp:Menu>
                </td>
            </tr>
    </table>
    <div align="center">
                        <asp:Label ID="Label1" runat="server" Width="184px" Font-Bold="True" Visible="False">No Allocations to display</asp:Label>
                    </div>
     <div align="center">
                        <asp:Label ID="lblOutput" runat="server" Width="184px" Font-Bold="True" Visible="False">CREDITS</asp:Label>
                    </div>
                            <asp:GridView ID="grdCredit" runat="server" BackColor="#FFC0FF" CellSpacing="1">
                <HeaderStyle BackColor="White" BorderColor="White" Font-Bold="False" Font-Italic="False" />
            </asp:GridView>                     
            <div align="center">
                        <asp:Label ID="lblOutput1" runat="server" Width="184px" Font-Bold="True" Visible="False">DEBITS</asp:Label>
                    </div>
        
                                      <asp:GridView ID="grdDebit" runat="server" BackColor="#FFC0FF" CellSpacing="1">
                <HeaderStyle BackColor="White" BorderColor="White" Font-Bold="False" Font-Italic="False" />
            </asp:GridView>    
        <br />
        <br />
        &nbsp;<asp:Button ID="Button1" runat="server" Text="Ok" /></div>              
    </form>
</body>
</html>
