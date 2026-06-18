<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CoverNoteSheet.aspx.vb" Inherits="CoverNote_CoverNoteSheet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblSamErrorMessage" runat="server" ForeColor="Red"></asp:Label><br />
        <table style="width: 344px">
            <tr>
                <td style="width: 276px">
                    Sheet Number</td>
                <td style="width: 33px">
                    <asp:TextBox ID="txtSheetNumber" runat="server"></asp:TextBox>&nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 276px">
                    Policy Number</td>
                <td style="width: 33px">
                    <asp:TextBox ID="txtPolicyNumber" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 276px">
                    Assigned date</td>
                <td style="width: 33px">
                    <asp:TextBox ID="txtassignedate" runat="server" Enabled="False"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 276px">
                    Sheet Status</td>
                <td style="width: 33px">
                    <asp:DropDownList ID="ddlSheetStatus" runat="server" Enabled="False">
                        
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 276px; height: 109px">
                    Comments<br />
                </td>
                <td style="width: 33px; height: 109px">
                    <asp:TextBox ID="txtComments" runat="server" Height="200px" Width="336px"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2" style="height: 14px">
                    <asp:Button ID="btnOk" runat="server" Text="Ok" Width="48px" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" /></td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
