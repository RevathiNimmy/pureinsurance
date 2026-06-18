<%@ Page Language="VB" AutoEventWireup="false" CodeFile="6_CoinsuranceBreakDown.aspx.vb"
    Inherits="OpenClaim_5_CoinsuranceBreakDown" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">

     <table width="100%" style="height: 100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="height: 10%">
                    <td style="width: 10%" align="right">
                        <table width="100%">
                            <tr>
                                <td>
                                    <uc1:Header ID="Header1" runat="server" />
                                    
                                </td>
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
                   <div>
            &nbsp;<asp:Label ID="Label2" runat="server" Width="60px"></asp:Label>
            <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
                ForeColor="#C000C0" Text="Coinsurance BreakDown Details" Width="232px"></asp:Label><br />
            <br />
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
            <br />
            <hr />
            &nbsp;
            <asp:Label ID="lblOutput" runat="server" Width="184px"></asp:Label>
            <asp:GridView ID="CROutput" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
                <EmptyDataTemplate>
                    <table style="text-align: center">
                        <tr>
                            <td>
                                No Records Found</td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <HeaderStyle BackColor="#5D7B9D" BorderColor="White" Font-Bold="True" ForeColor="White" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>
        </div>
          
                    </td>
                </tr>
            </table>
                      
       <%-- <div>
            &nbsp;<asp:Label ID="Label2" runat="server" Width="60px"></asp:Label>
            <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
                ForeColor="#C000C0" Text="Coinsurance BreakDown Details" Width="232px"></asp:Label><br />
            <br />
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
            <br />
            <hr />
            &nbsp;
            <asp:Label ID="lblOutput" runat="server" Width="184px"></asp:Label>
            <asp:GridView ID="CROutput" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
                <EmptyDataTemplate>
                    <table style="text-align: center">
                        <tr>
                            <td>
                                No Records Found</td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <HeaderStyle BackColor="#5D7B9D" BorderColor="White" Font-Bold="True" ForeColor="White" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>
        </div>--%>
    </form>
</body>
</html>
