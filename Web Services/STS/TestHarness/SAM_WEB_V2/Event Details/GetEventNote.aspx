<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GetEventNote.aspx.vb" Inherits="Event_Details_GetEventNote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <table style="width: 348px">
                <tr>
                    <td style="width: 105px; height: 24px">
                        Context</td>
                    <td style="height: 24px">
                        <asp:TextBox ID="txtcontext" runat="server" Enabled="False"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 105px">
                        Subject</td>
                    <td>
                        <asp:TextBox ID="txtsubject" runat="server" Enabled="False"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 21px">
                        Event Date</td>
                    <td style="height: 21px">
                        <asp:TextBox ID="txtEventDate" runat="server" Enabled="False"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 105px; height: 21px">
                        User Name</td>
                    <td style="height: 21px">
                        <asp:TextBox ID="txtUserName" runat="server" Enabled="False"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 103px">
                        <asp:TextBox ID="txtText" runat="server" Enabled="False" Height="212px" Width="356px"></asp:TextBox></td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnOk" runat="server" Text="OK" Width="68px" />
        <asp:Button ID="BtnCancel" runat="server" Text="Cancel" Width="68px" /></div>
    </form>
</body>
</html>
