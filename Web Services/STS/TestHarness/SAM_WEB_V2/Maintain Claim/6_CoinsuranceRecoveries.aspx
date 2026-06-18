<%@ Page Language="VB" AutoEventWireup="false" CodeFile="6_CoinsuranceRecoveries.aspx.vb"
    Inherits="OpenClaim_CoinsuranceBreakdown" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CoInsurance recoveries</title>
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
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
                        &nbsp;<asp:Label ID="Label2" runat="server" Width="60px"></asp:Label>
                        <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
                            ForeColor="#C000C0" Text="Coinsurance Recoveries Details" Width="240px"></asp:Label><br />
                        <br />
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
                        &nbsp;<br />
                        &nbsp;<asp:RadioButtonList ID="rblSalvageTPRecovery" runat="server" RepeatDirection="Horizontal"
                            Width="320px">
                            <asp:ListItem Selected="True" Value="1">Salvage</asp:ListItem>
                            <asp:ListItem Value="0">Third Party</asp:ListItem>
                        </asp:RadioButtonList><br />
                        &nbsp;
                        <asp:Label ID="lblOutput" runat="server" Width="184px"></asp:Label>
                        <br />
                        <asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" Width="544px">
                            <Items>
                                <asp:MenuItem Text="Reinsurance Recoveries" Value="0"></asp:MenuItem>
                                <asp:MenuItem Text="CoInsurance Recoveries" Value="1"></asp:MenuItem>
                                <asp:MenuItem Text="CoInsurance Recoveries Breakdown" Value="2"></asp:MenuItem>
                            </Items>
                        </asp:Menu>
                        <br />
                        <br />
                        <asp:MultiView ID="mvCoinsurance" runat="server" ActiveViewIndex="0">
                            <asp:View ID="View1" runat="server">
                        <asp:GridView ID="CROutput" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
                            <HeaderStyle BackColor="#5D7B9D" BorderColor="White" Font-Bold="True" ForeColor="White" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <EditRowStyle BackColor="#999999" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        </asp:GridView>
                            </asp:View>
                            <asp:View ID="View2" runat="server">
                                <asp:GridView ID="gvCoinsurence" runat="server" CellPadding="4" ForeColor="#333333"
                                    GridLines="None">
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <EditRowStyle BackColor="#999999" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                </asp:GridView>
                            </asp:View>
                            <asp:View ID="View3" runat="server">
                                <asp:GridView ID="gvCoinsuranceBreakDown" runat="server" CellPadding="4" ForeColor="#333333"
                                    GridLines="None">
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <EditRowStyle BackColor="#999999" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                </asp:GridView>
                            </asp:View>
                        </asp:MultiView>
                    </td>
                </tr>
            </table>
            <%--  &nbsp;<asp:Label ID="Label2" runat="server" Width="60px"></asp:Label>
        <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
            ForeColor="#C000C0" Text="Coinsurance Recoveries Details" Width="240px"></asp:Label><br />
        <br />
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
        &nbsp;<br />
        &nbsp;<asp:RadioButtonList ID="rblSalvageTPRecovery" runat="server" RepeatDirection="Horizontal"
            Width="320px">
            <asp:ListItem Selected="True" Value="1">Salvage</asp:ListItem>
            <asp:ListItem Value="0">Third Party</asp:ListItem>
        </asp:RadioButtonList><br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnGetCoinsuranceRecoveries" runat="server" Text="CoinsuranceRecoveries" Width="154px" />&nbsp;
        <asp:Button ID="btnReinsuranceRecoveries" runat="server" ForeColor="#C000C0" Text="ReinsuranceRecoveries" />
        &nbsp;
        <br />
        <br />
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
        <asp:Button ID="Button1" runat="server" Text="CoInsurance Recoveries  BreakDown" /><br />
        <hr />
        &nbsp;
        <asp:Label ID="lblOutput" runat="server" Width="184px"></asp:Label>
        <asp:GridView ID="CROutput" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
            <HeaderStyle BackColor="#5D7B9D" BorderColor="White" Font-Bold="True" ForeColor="White" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <EditRowStyle BackColor="#999999" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>--%>
        </div>
    </form>
</body>
</html>
