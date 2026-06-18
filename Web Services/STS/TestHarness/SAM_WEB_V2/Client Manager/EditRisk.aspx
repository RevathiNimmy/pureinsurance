<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditRisk.aspx.vb" Title="EditRisk" Inherits="MTA_EditRisk" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 279px; height: 307px">
            <tr>
                <td style="width: 100px">
                    Mobile Phone Make</td>
                <td style="width: 100px">
                    <asp:DropDownList ID="cboMobileMake" runat="server" Width="105px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 100px">
                    Mobile Model</td>
                <td style="width: 100px">
                    <asp:DropDownList ID="cboMobileModel" runat="server" Width="105px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 100px">
                    Identification No</td>
                <td style="width: 100px">
                    <asp:TextBox ID="txtIdentification" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 100px">
                    Insured Value</td>
                <td style="width: 100px">
                    <asp:TextBox ID="txtInsuredValue" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 100px">
                    Date of Purchased</td>
                <td style="width: 100px">
                    <asp:TextBox ID="txtDate" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 100px; height: 52px">
                    Insured Occupation</td>
                <td style="width: 100px; height: 52px">
                    <asp:TextBox ID="txtOccupation" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 100px; height: 31px">
                    <asp:Button ID="btnOK" runat="server" Text="OK" Width="104px" /></td>
                <td style="width: 100px; height: 31px">
                    <asp:Button ID="btnCancel" runat="server" Text="CANCEL" Width="104px" /></td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
