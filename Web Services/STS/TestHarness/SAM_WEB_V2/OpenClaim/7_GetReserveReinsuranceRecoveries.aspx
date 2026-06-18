<%@ Page Language="VB" AutoEventWireup="false" CodeFile="7_GetReserveReinsuranceRecoveries.aspx.vb"
    Inherits="GetReserveReinsuranceRecoveries" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

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
                    <table border="0" cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <span style="color: #C000C0"><strong>Branch Code</strong></span></td>
                    <td>
                        <asp:TextBox ID="txtBranchCode" runat="server" ReadOnly="True" Text="HEADOFF"></asp:TextBox>
                    </td>
                    <td rowspan="4" style="width: 50%">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <span style="color: #C000C0"><strong>ClaimPerilKey</strong></span></td>
                    <td>
                        <asp:TextBox ID="txtClaimPerilKey" runat="server"></asp:TextBox>
                         <span style="color: #C000C0">Only Integer value is accepted</span></td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkIsSalvage" runat="server" Text="IsSalvage" ForeColor="#C000C0" /></td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" />
                    </td>
                </tr>
                <tr>
                    <td align="left"  colspan="3">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" ></asp:Label></td>
                </tr>
                <tr>
                    <td align="left" colspan="3">
                        <asp:GridView ID="grdReserveRIRecoveries" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <EditRowStyle BackColor="#999999" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            </td>
                </tr>
            </table>
        
        
        
        
           <%-- <table border="0" cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <span style="color: #C000C0"><strong>Branch Code</strong></span></td>
                    <td>
                        <asp:TextBox ID="txtBranchCode" runat="server" ReadOnly="True" Text="HEADOFF"></asp:TextBox>
                    </td>
                    <td rowspan="4" style="width: 50%">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <span style="color: #C000C0"><strong>ClaimPerilKey</strong></span></td>
                    <td>
                        <asp:TextBox ID="txtClaimPerilKey" runat="server"></asp:TextBox>
                         <span style="color: #C000C0">Only Integer value is accepted</span></td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkIsSalvage" runat="server" Text="IsSalvage" ForeColor="#C000C0" /></td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" />
                    </td>
                </tr>
                <tr>
                    <td align="left"  colspan="3">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" ></asp:Label></td>
                </tr>
                <tr>
                    <td align="left" colspan="3">
                        <asp:GridView ID="grdReserveRIRecoveries" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <EditRowStyle BackColor="#999999" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>--%>
        </div>
    </form>
</body>
</html>
