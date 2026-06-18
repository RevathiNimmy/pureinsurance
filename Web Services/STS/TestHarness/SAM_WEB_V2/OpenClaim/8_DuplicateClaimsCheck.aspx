<%@ Page Language="VB" AutoEventWireup="false" CodeFile="8_DuplicateClaimsCheck.aspx.vb"
    Inherits="OpenClaim_3_DuplicateClaimsCheck" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="txtShortName">
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
                        <asp:Label ID="lbl2" runat="server" Width="60px"></asp:Label><asp:Label ID="Label1"
                            runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True" ForeColor="#C000C0"
                            Text="Duplicate Claims Override" Width="200px"></asp:Label>
                        <hr />
                        <table>
                            <tr>
                                <td>
                                    User</td>
                                <td>
                                    <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    Password</td>
                                <td>
                                    <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnOverRideClaims" runat="server" Text="Override Claim" /></td>
                            </tr>
                            <tr>
                                <td>
                                  <asp:Label ID="lblOutput" runat="server" Visible="False" Font-Bold="True" /></td>
                            </tr>
                        </table>
                       
                    </td>
                </tr>
            </table>
            <%-- <asp:Label ID="lbl2" runat="server" Width="60px"></asp:Label><asp:Label ID="Label1"
            runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True" ForeColor="#C000C0"
            Text="Duplicate Claims Override" Width="200px"></asp:Label>
        <hr />
        <strong>User &nbsp; &nbsp; &nbsp;&nbsp; </strong>&nbsp; : &nbsp;<asp:TextBox ID="txtUserName"
            runat="server"></asp:TextBox><br />
        <strong>Password &nbsp; </strong>:
        <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox><br />
        &nbsp;<asp:Button ID="btnOverRideClaims" runat="server" Text="Override Claim" />
        <hr/>
        <asp:Label ID="lblOutput" runat="server" Visible="False" Font-Bold="True" /><br />--%>
        </div>
    </form>
</body>
</html>
