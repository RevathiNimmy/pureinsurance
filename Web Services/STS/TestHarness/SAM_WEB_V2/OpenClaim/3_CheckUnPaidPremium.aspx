<%@ Page Language="VB" AutoEventWireup="false" CodeFile="3_CheckUnPaidPremium.aspx.vb"
    Inherits="CheckUnPaidPremium" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Check UnPaid Premium - V2 </title>
</head>
<body text="#9900cc">
    <form id="form1" runat="server">
        <div id="txtShortName">
            <table width="100%" style="height: 100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="height: 10%">
                    <td style="width: 10%" align="right">
                        <table width="100%">
                            <tr>
                                <td >
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
                        <asp:Label ID="lbl2" runat="server" Width="60px"></asp:Label><asp:Label ID="Label1"
                            runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True" ForeColor="#C000C0"
                            Text="Check UnPaid Premium" Width="168px"></asp:Label>
                        <hr />
                        <table border="0" cellspacing="4" width="100%">
                            <tr>
                                <td>
                                    <table cellpadding="2" cellspacing="2" border="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" Text="Policy Number"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPolicyNo" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" Text="Claim Date"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblClaimDate" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnOk" runat="server" Text="Continue" />
                                    <hr />
                                    <asp:GridView ID="gvTransactions" runat="server" CellPadding="4" ForeColor="#333333"
                                        GridLines="None">
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                                        <EditRowStyle BackColor="#999999" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    </asp:GridView>
                                    <br />
                                    <asp:Label ID="lblOutput" runat="server" Visible="False" Font-Bold="True" /><br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <%-- <asp:Label ID="lbl2" runat="server" Width="60px"></asp:Label><asp:Label ID="Label1"
                runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True" ForeColor="#C000C0"
                Text="Check UnPaid Premium" Width="168px"></asp:Label>
            <hr />
            <table border="0" cellspacing="4" width="100%">
                <tr>
                    <td>
                        <table cellpadding="2" cellspacing ="2" border ="0">
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Policy Number"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblPolicyNo" runat="server" ></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Claim Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblClaimDate" runat="server" ></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnOk" runat="server" Text="Continue" />
                        <hr />
                        <asp:GridView ID="gvTransactions" runat="server" CellPadding="4" ForeColor="#333333"
                            GridLines="None">
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                            <EditRowStyle BackColor="#999999" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        </asp:GridView>
                        <br />
                        <asp:Label ID="lblOutput" runat="server" Visible="False" Font-Bold="True" /><br />
                    </td>
                </tr>
            </table>--%>
        </div>
    </form>
</body>
</html>
