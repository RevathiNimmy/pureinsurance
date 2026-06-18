<%@ Page Language="VB" AutoEventWireup="false" CodeFile="1_FindClaim.aspx.vb" Inherits="View_Claim_FindClaim" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
                ForeColor="#C000C0" Text="Find Claim" Width="192px"></asp:Label>&nbsp;</div>
        <table style="width: 600px">
            <tr>
                <td colspan="3">
                    &nbsp;Policy &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:TextBox ID="txtPolicyNumber" runat="server"></asp:TextBox><br />
                    Risk Index &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:TextBox ID="txtRiskIndex" runat="server"></asp:TextBox><br />
                    <strong>Claim &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        <asp:TextBox ID="txtClaim" runat="server"></asp:TextBox><br />
                        <asp:Button ID="btnShortName" runat="server" Text="ShortName" />
                        &nbsp;<asp:TextBox ID="txtShortName" runat="server"></asp:TextBox><br />
                        &nbsp; &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp;
                        <asp:CheckBox ID="chkIncludeCloseClaim" runat="server" Text="Include Closed Claim"
                            Width="176px" />
                        &nbsp; &nbsp;
                        <br />
                        Loss Date Start Limit &nbsp;
                        <asp:TextBox ID="txtInForceFrom" runat="server"></asp:TextBox><br />
                        Loss Date Start Limit &nbsp; &nbsp; 
                        <asp:TextBox ID="txtInForceTo" runat="server"></asp:TextBox></strong></td>
            </tr>
            <tr>
                <td style="width: 442px">
                    <asp:Button ID="btnFind" runat="server" Text="Find Now" />
                    <asp:Button ID="Button2" runat="server" Text="Button" /></td>
                <td colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="3" style="height: 13px">
                    &nbsp;<hr /><asp:GridView ID="gvResult" runat="server" DataKeyNames="ClaimKey">
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" />
                        </Columns>
                    </asp:GridView>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="3" style="height: 13px">
                <hr />
                    <asp:Button ID="btnOk" runat="server" Text="Ok" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
