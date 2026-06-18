<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CurrencyConversion.aspx.vb"   Inherits="CurrencyExchange_CurrencyConversion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<base target="_self" />
    <title>Currency Conversion</title>
    
    <script language ="javascript" type="text/javascript">
    function CLOSED()
    {
      
      top.returnValue = 'CANCELLED'
      //alert(top.returnValue)
      window.close();
      return true;

    }
    
    
    
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td colspan="2">
                        <strong>Transaction</strong></td>
                </tr>
                <tr>
                    <td style="width: 161px">
                        Transaction Currency</td>
                    <td style="width: 288px">
                        <asp:DropDownList ID="ddlTransactionCurrency" runat="server" Enabled="False">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 161px">
                        Date of&nbsp; Exchange</td>
                    <td style="width: 288px">
                        <asp:DropDownList ID="ddlDateofExchange" runat="server" Enabled="False">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 161px; height: 21px">
                    </td>
                    <td style="width: 288px; height: 21px">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <strong>Base Currency</strong></td>
                </tr>
                <tr>
                    <td style="width: 161px">
                        Base Currency</td>
                    <td style="width: 288px">
                        <asp:DropDownList ID="ddlBaseCurrency" runat="server" Enabled="False">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 161px">
                        Base Currency Rate</td>
                    <td style="width: 288px">
                        <asp:TextBox ID="txtBaseCurrencyRate" runat="server" Enabled="False"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 161px">
                    </td>
                    <td style="width: 288px">
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 21px">
                        <strong>System&nbsp; Currency</strong></td>
                </tr>
                <tr>
                    <td style="width: 161px">
                        System Currency</td>
                    <td style="width: 288px">
                        <asp:DropDownList ID="ddlSystemCurrency" runat="server" Enabled="False">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 161px">
                        System Currency Rate</td>
                    <td style="width: 288px">
                        <asp:TextBox ID="txtSystemCurrencyRate" runat="server" Enabled="False"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="width: 161px">
                    </td>
                    <td style="width: 288px">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <strong>Rate Override Reason</strong></td>
                </tr>
                <tr>
                    <td style="width: 161px">
                        Reason:</td>
                    <td style="width: 288px">
                        <asp:DropDownList ID="ddlReason" runat="server" Enabled="False">
                            <asp:ListItem>None</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 161px">
                        <asp:Button ID="btnOk" runat="server" Text="Ok" /><asp:Button ID="btnCancel" runat="server"
                            Text="Cancel" /></td>
                    <td style="width: 288px">
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
