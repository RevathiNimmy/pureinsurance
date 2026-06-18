<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CoverNoteSheetEdit.aspx.vb" Inherits="Temp_covennotes_CoverNoteSheetEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
        </div>
    
    </div>
        <table>
            <tr>
                <td style="width: 100px">
                    Sheet Number</td>
                <td style="width: 101px">
                    <asp:TextBox ID="txtSheetNumber" runat="server" Enabled="False"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 100px">
                    Old Cover&nbsp; Note Sheet Number</td>
                <td style="width: 101px">
                    <asp:TextBox ID="txtoldnumber" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 100px; height: 59px;">
                    New Cover&nbsp; Note Sheet Number</td>
                <td style="width: 101px; height: 59px;">
                    <asp:TextBox ID="txtnewnumber" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 100px">
                    Policy Number</td>
                <td style="width: 101px">
                    <asp:TextBox ID="txtPolicyNumber" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 100px">
                    Assigned Date</td>
                <td style="width: 101px">
                    <asp:TextBox ID="txtAssignedDate" runat="server" Enabled="False"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 100px">
                    Sheet status</td>
                <td style="width: 101px">
                    <asp:DropDownList ID="ddlSheetStatus" runat="server">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 100px">
                    Comments</td>
                <td style="width: 101px">
                    <asp:TextBox ID="txtComments" runat="server" Height="171px" Width="317px"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 100px">
                </td>
                <td style="width: 101px">
                    <asp:Button ID="btnEdit" runat="server" Text="Ok" />
                    <asp:Button ID="Btncancel" runat="server" Text="Cancel" /></td>
            </tr>
            <tr>
                <td style="width: 100px">
                </td>
                <td style="width: 101px">
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
