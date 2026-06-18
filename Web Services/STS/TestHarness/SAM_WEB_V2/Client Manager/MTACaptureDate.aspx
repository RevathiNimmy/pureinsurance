<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MTACaptureDate.aspx.vb" Inherits="MTA_MTACaptureDate" Title="MTACaptureDate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="vertical-align: middle;">
            <table style="height: 100%; width: 100%">
                <tr>
                    <td style="vertical-align: middle;">
                        <table border="0" width="50%" style="background: seashell; height: 50%" cellpadding="4">
                            <tr>
                                <td>
                                    MTA Date
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMTADate" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td style="text-align: left;">
                                    <asp:CheckBox ID="chkIsPermanent" runat="server" Text="Permanent MTA" Checked="True" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Reason</td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="ddlMTAReason" runat="server">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>
                                    If Other</td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="txtMTAReason" runat="server">OTHER</asp:TextBox></td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: left">
                                    <asp:Button ID="btnOK" runat="server" Text="Ok" Width="59px" /></td>
                            </tr>
                        </table>
                        <asp:Label ID="lblError" runat="server"></asp:Label></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
