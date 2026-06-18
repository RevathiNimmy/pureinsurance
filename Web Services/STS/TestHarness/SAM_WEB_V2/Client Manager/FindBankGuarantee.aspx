<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FindBankGuarantee.aspx.vb" Inherits="BankGuarantee_FindBankGuarantee" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table cellpadding="2" cellspacing="2" width="100%">
    <tr>
    <td style="height: 28px">
    <asp:Button ID="btnFindClient" runat="server" Text="Client" />
    </td>
    <td style="height: 28px">
    <asp:TextBox ID="txtClient" runat="server"></asp:TextBox>
    </td>
    <td style="height: 28px">
    <asp:Label ID="lblBankGuaranteeRef" runat="server" Text="Bank Guarantee Ref:"></asp:Label>
    </td>
    <td style="height: 28px">
    <asp:TextBox ID="txtBankGuaranteeRef" runat="server"></asp:TextBox>
    </td>
    </tr>
    <tr>
    <td style="height: 28px">
    <asp:Button ID="btnFindAgent" runat="server" Text="Agent" />
    </td>
    <td style="height: 28px">
    <asp:TextBox ID="txtAgent" runat="server"></asp:TextBox>
    </td>
    <td style="height: 28px">
    <asp:Label ID="lblBankName" runat="server" Text="Bank Name"></asp:Label>
    </td>
    <td style="height: 28px">
    <asp:TextBox ID="txtBankName" runat="server"></asp:TextBox>
    </td>
    </tr>
    <tr>
    <td style="height: 28px">
    <asp:Button ID="btnFindInsuranceFile" runat="server" Text="Insurance File" />
    </td>
    <td style="height: 28px">
    <asp:TextBox ID="txtInsuranceFile" runat="server"></asp:TextBox>
    </td>
    <td style="height: 28px">
        &nbsp;<asp:Label ID="lblBgStatus" runat="server" Text="BG status"></asp:Label></td>
    <td style="height: 28px">
        &nbsp;<asp:TextBox ID="txtbgstatus" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td style="height: 28px">
            &nbsp;</td>
    <td style="height: 28px">
        &nbsp;</td>
    <td style="height: 28px">
        &nbsp;<asp:Button ID="btnFind" runat="server" Text="Find Now" /></td>
    <td style="height: 28px">
        &nbsp;<asp:Button ID="btnReset" runat="server" Text="New Search" /></td>
    </tr>
    <tr>
    <td colspan="4">
        <asp:GridView ID="gvBankGuarantees" runat="server">
        </asp:GridView>
    
    </td>
    </tr>
    </table>
        
    </div>
        
        
    </form>
</body>
</html>
