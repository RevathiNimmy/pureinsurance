<%@ Page Language="VB" AutoEventWireup="false" CodeFile="NewClient.aspx.vb" Inherits="New_Business_NewClient" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 484px">
            <tr>
                <td colspan="3" style="height: 59px">
                    <strong>Please Select a New party and Branch to add
                        <br />
                        From the List<br />
                    </strong>
                </td>
                <td colspan="1" style="height: 59px">
                </td>
            </tr>
            <tr>
                <td style="width: 100px; height: 24px">
                    &nbsp; &nbsp; &nbsp; &nbsp; New Party</td>
                <td style="width: 66px; height: 24px">
                    <asp:DropDownList ID="ddlParty" runat="server">
                        <asp:ListItem>Personal Client</asp:ListItem>
                        <asp:ListItem>Corporate Client</asp:ListItem>                        
                    </asp:DropDownList></td>
                <td style="width: 100px; height: 24px">
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; Branch
                </td>
                <td style="width: 100px; height: 24px">
                    <asp:DropDownList ID="ddlBranch" runat="server">
                        <asp:ListItem>Head Office</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
        </table>
    
    </div>
        <asp:Button ID="btnOk" runat="server" Style="z-index: 100; left: 14px; position: absolute;
            top: 130px" Text="Ok" Width="44px" />
    </form>
</body>
</html>
