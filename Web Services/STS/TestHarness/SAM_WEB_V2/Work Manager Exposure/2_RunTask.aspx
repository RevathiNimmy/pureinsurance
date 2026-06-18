<%@ Page Language="VB" AutoEventWireup="false" CodeFile="2_RunTask.aspx.vb" Inherits="Work_Manager_Exposure_2_RunTask" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 600px">
            <tr>
                <td colspan="3" style="height: 41px">
                    <strong>Authorise Payments</strong></td>
            </tr>
            <tr>
                <td colspan="3" style="height: 100px">
                    <hr />
                    &nbsp;<asp:Panel ID="Panel1" runat="server" Height="300px" ScrollBars="Auto" Width="800px">
                        <asp:GridView ID="gvTasks" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                            Width="232px">
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                            <EditRowStyle BackColor="#999999" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" />
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="3" style="height: 42px">
                    <hr />
                    &nbsp;
                    <asp:Button ID="btnViewTask" runat="server" Text="View" />
                    <asp:Button ID="btnAuthorise" runat="server" Text="Authorise" />
                    <asp:Button ID="btnDecline" runat="server" Text="Decline" />
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
