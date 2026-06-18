<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RiskReinsuranceArrangements.aspx.vb"
    Inherits="RiskReinsuranceArrangements" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body style="background-color:#FFF5EE">
    <form id="form1" runat="server">
        <div>
            <table cellpadding="4" cellspacing="0" width="100%" style="background-color: #FFF5EE">
                <tr>
                    <td>
                        <table cellpadding="4" cellspacing="0" style="background-color: #FFF5EE">
                            <tr>
                                <td>
                                    Reinsurance Band:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlReinsuranceBand" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Menu ID="Menu1" runat="server" BackColor="#FFFBD6" DynamicHorizontalOffset="2"
                            Font-Names="Verdana" Font-Size="0.8em" ForeColor="#990000" Orientation="Horizontal"
                            StaticSubMenuIndent="10px">
                            <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                            <DynamicHoverStyle BackColor="#990000" ForeColor="White" />
                            <DynamicMenuStyle BackColor="#FFFBD6" />
                            <StaticSelectedStyle BackColor="#FFCC66" />
                            <DynamicSelectedStyle BackColor="#FFCC66" />
                            <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                            <Items>
                                <asp:MenuItem Text="Reinsurance" Value="Reinsurance"></asp:MenuItem>
                                <asp:MenuItem Text="Original Reinsurance" Value="Original Reinsurance"></asp:MenuItem>
                            </Items>
                            <StaticHoverStyle BackColor="#990000" ForeColor="White" />
                        </asp:Menu>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:MultiView ID="MultiView1" runat="server">
                            <asp:View ID="View1" runat="server">
                                <table border="0" cellpadding="4">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Reinsurance Arrangement" Font-Bold="True" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvReinsuranceArrangements" runat="server" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Both" BorderStyle="Solid" >
                                                <FooterStyle BackColor="Tan" />
                                                <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                                <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                                                <HeaderStyle BackColor="Tan" Font-Bold="True" />
                                                <AlternatingRowStyle BackColor="PaleGoldenrod" />
                                                <EditRowStyle BorderStyle="Solid" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="Reinsurance Arrangement Lines" Font-Bold="True" ForeColor="Red"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvArrangementLines" runat="server" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" CellPadding="4" ForeColor="Black">
                                                <FooterStyle BackColor="Tan" />
                                                <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                                <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                                                <HeaderStyle BackColor="Tan" Font-Bold="True" />
                                                <AlternatingRowStyle BackColor="PaleGoldenrod" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="View2" runat="server">
                                <asp:GridView ID="gvOriginalRI" runat="server" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" CellPadding="4" ForeColor="Black">
                                    <FooterStyle BackColor="Tan" />
                                    <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                    <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                                    <HeaderStyle BackColor="Tan" Font-Bold="True" />
                                    <AlternatingRowStyle BackColor="PaleGoldenrod" />
                                </asp:GridView>
                            </asp:View>
                        </asp:MultiView>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
