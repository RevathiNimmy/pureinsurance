<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddEventNote.aspx.vb" Inherits="Event_Details_AddEventNote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Add Event Note</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 348px">
            <tr>
                <td style="width: 105px; height: 24px;">
                    Context</td>
                <td style="width: 722px; height: 24px;">
                    <asp:DropDownList ID="ddlContext" runat="server" Width="246px" AutoPostBack="True">
                        <asp:ListItem Value="20">Notes - Customer</asp:ListItem>
                        <asp:ListItem Value="37">Notes-Customer Warning</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 105px">
                    Subject</td>
                <td style="width: 722px">
                    <asp:DropDownList ID="ddlSubject" runat="server" Width="244px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 105px; height: 21px">
                    Event Date</td>
                <td style="height: 21px; width: 722px;">
                    <asp:TextBox ID="txtEventDate" runat="server" Enabled="False"></asp:TextBox>
                    &nbsp; 
                    <asp:Label ID="lblpriority" runat="server" Text="Priority"></asp:Label>
                    <asp:DropDownList ID="ddlpriority" runat="server">
                        <asp:ListItem Selected="True">Red</asp:ListItem>
                        <asp:ListItem>Amber</asp:ListItem>
                        <asp:ListItem>Green</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 105px; height: 21px">
                    User Name</td>
                <td style="height: 21px; width: 722px;">
                    <asp:TextBox ID="txtUserName" runat="server" Enabled="False"></asp:TextBox>
                    <asp:Label ID="lblstatus" runat="server" Text="Status "></asp:Label>
                    <asp:DropDownList ID="ddlstatus" runat="server">
                        <asp:ListItem Selected="True" Value="0">Outstanding</asp:ListItem>
                        <asp:ListItem Value="1">Completed</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td colspan="2" style="height: 103px">
                    <asp:TextBox ID="txtText" runat="server" Height="212px" Width="356px"></asp:TextBox></td>
            </tr>
        </table>
    
    </div>
        <asp:Button ID="btnOk" runat="server" Text="OK" Width="68px" />
        <asp:Button ID="BtnCancel" runat="server" Text="Cancel" Width="68px" />
    </form>
</body>
</html>
