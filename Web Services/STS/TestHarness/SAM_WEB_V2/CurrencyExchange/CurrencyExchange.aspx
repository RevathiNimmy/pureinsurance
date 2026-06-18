<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CurrencyExchange.aspx.vb" Inherits="CurrencyExchange_CurrencyExchange" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
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
                <td style="width: 162px; height: 22px;">
                    <strong>Transaction</strong></td>
                <td style="width: 210px; height: 22px;">
                </td>
                <td style="height: 22px;" colspan="2">
                    <strong>Client Account Currency</strong></td>
            </tr>
            <tr>
                <td style="width: 162px">
                    Transaction value</td>
                <td style="width: 210px">
                    <asp:TextBox ID="txtTransaction" runat="server" Enabled="False"></asp:TextBox></td>
                <td style="width: 178px">
                    Account Currency</td>
                <td style="width: 378px">
                    <asp:DropDownList ID="ddlAccountCurrency" runat="server" Enabled="False">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 162px">
                    Transaction Currency</td>
                <td style="width: 210px">
                    <asp:DropDownList ID="ddlTransactionCurrency" runat="server" Enabled="False">
                    </asp:DropDownList></td>
                <td style="width: 178px">
                    Account Currency Rate</td>
                <td style="width: 378px">
                    <asp:TextBox ID="txtAccountCurrencyRate" runat="server" Enabled="False"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 162px">
                    Date of &nbsp;Exchange</td>
                <td style="width: 210px">
                    <asp:DropDownList ID="ddlDateofExchange" runat="server" Enabled="False">
                    </asp:DropDownList></td>
                <td style="width: 178px">
                    Account Currency Amount</td>
                <td style="width: 378px">
                    <asp:TextBox ID="txtAccountCurrencyAmount" runat="server" Enabled="False"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 162px">
                    <strong>Base Currency</strong></td>
                <td style="width: 210px">
                </td>
                <td style="width: 178px">
                </td>
                <td style="width: 378px">
                </td>
            </tr>
            <tr>
                <td style="width: 162px; height: 21px;">
                    Base Currency</td>
                <td style="width: 210px; height: 21px;">
                    <asp:DropDownList ID="ddlBaseCurrency" runat="server" Enabled="False">
                    </asp:DropDownList></td>
                <td style="width: 178px; height: 21px;">
                    <strong>System Currency</strong></td>
                <td style="width: 378px; height: 21px;">
                </td>
            </tr>
            <tr>
                <td style="width: 162px">
                    Base Currency Rate</td>
                <td style="width: 210px">
                    <asp:TextBox ID="txtBaseCurrencyRate" runat="server" Enabled="False"></asp:TextBox></td>
                <td style="width: 178px">
                    System Currency</td>
                <td style="width: 378px">
                    <asp:DropDownList ID="ddlSystemCurrency" runat="server" Enabled="False">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 162px">
                    Base Currency Amount</td>
                <td style="width: 210px">
                    <asp:TextBox ID="txtBaseCurrencyAmount" runat="server" Enabled="False"></asp:TextBox></td>
                <td style="width: 178px">
                    System Currency Rate</td>
                <td style="width: 378px">
                    <asp:TextBox ID="txtSystemCurrencyRate" runat="server" Enabled="False"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 162px">
                    <strong>Rate Override Reason</strong></td>
                <td style="width: 210px">
                </td>
                <td style="width: 178px">
                    System Currency Amount</td>
                <td style="width: 378px">
                    <asp:TextBox ID="SystemCurrencyAmount" runat="server" Enabled="False"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 162px">
                    Reason</td>
                <td style="width: 210px">
                    <asp:DropDownList ID="ddlReason" runat="server" Enabled="False">
                        <asp:ListItem>None</asp:ListItem>
                    </asp:DropDownList></td>
                <td style="width: 178px">
                </td>
                <td style="width: 378px">
                </td>
            </tr>
            <tr>
                <td style="width: 162px; height: 21px;">
                </td>
                <td style="width: 210px; height: 21px;">
                </td>
                <td style="width: 178px; height: 21px;">
                </td>
                <td style="width: 378px; height: 21px;">
                </td>
            </tr>
            <tr>
                <td style="width: 162px">
                </td>
                <td style="width: 210px">
                </td>
                <td style="width: 178px">
                </td>
                <td style="width: 378px">
                </td>
            </tr>
            <tr>
                <td style="width: 162px">
                </td>
                <td style="width: 210px">
                </td>
                <td style="width: 178px">
                </td>
                <td style="width: 378px">
                </td>
            </tr>
            <tr>
                <td style="width: 162px">
                </td>
                <td style="width: 210px">
                </td>
                <td style="width: 178px">
                </td>
                <td style="width: 378px">
                </td>
            </tr>
            <tr>
                <td style="width: 162px">
                    <asp:Button ID="btnOk" runat="server" Text="Ok" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" /></td>
                <td style="width: 210px">
                </td>
                <td style="width: 178px">
                </td>
                <td style="width: 378px">
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
