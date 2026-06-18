<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ClaimReinsuranceBreakdown.aspx.vb"
    Inherits="ClaimReinsuranceBreakdown" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<base target="_self" />
    <title>Untitled Page</title>
   
</head>
<body style="background-color: #ffffff">
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
                         <table cellpadding="4" cellspacing="0" width="100%" style="background-color: #FFF5EE">
                <tr>
                    <td style="background-color: white">
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
                    <td style="background-color: white">
                    </td>
                </tr>
                <tr>
                    <td style="background-color: white">
                        <asp:MultiView ID="MultiView1" runat="server">
                            <asp:View ID="View1" runat="server">
                                <table border="0" cellpadding="4">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Reinsurance Arrangement" Font-Bold="True"
                                                ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvReinsuranceArrangements" runat="server" CellPadding="4" ForeColor="#333333"
                                                GridLines="None">
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <EditRowStyle BorderStyle="Solid" BackColor="#999999" />
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="Reinsurance Arrangement Lines" Font-Bold="True"
                                                ForeColor="Red"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvArrangementLines" runat="server" CellPadding="4" ForeColor="#333333"
                                                GridLines="None">
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <EditRowStyle BackColor="#999999" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView>
                        <asp:Button ID="btnOk" runat="server" Text="OK" Width="80px" /></td>
                </tr>
            </table>
                    </td>
                </tr>
            </table>
        
        
        
        
        
        
           <%-- <table cellpadding="4" cellspacing="0" width="100%" style="background-color: #FFF5EE">
                <tr>
                    <td style="background-color: white">
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
                    <td style="background-color: white">
                    </td>
                </tr>
                <tr>
                    <td style="background-color: white">
                        <asp:MultiView ID="MultiView1" runat="server">
                            <asp:View ID="View1" runat="server">
                                <table border="0" cellpadding="4">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Reinsurance Arrangement" Font-Bold="True"
                                                ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvReinsuranceArrangements" runat="server" CellPadding="4" ForeColor="#333333"
                                                GridLines="None">
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <EditRowStyle BorderStyle="Solid" BackColor="#999999" />
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Text="Reinsurance Arrangement Lines" Font-Bold="True"
                                                ForeColor="Red"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvArrangementLines" runat="server" CellPadding="4" ForeColor="#333333"
                                                GridLines="None">
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <SelectedRowStyle BackColor="#E2DED6" ForeColor="#333333" Font-Bold="True" />
                                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <EditRowStyle BackColor="#999999" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView>
                        <asp:Button ID="btnOk" runat="server" Text="OK" Width="80px" /></td>
                </tr>
            </table>--%>
        </div>
    </form>
</body>
</html>
