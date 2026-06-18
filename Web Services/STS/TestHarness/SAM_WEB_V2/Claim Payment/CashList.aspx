<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CashList.aspx.vb" Inherits="Claim_Payment_CashList" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Cash List</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <table width="100%" style="height: 100%" border="0" cellspacing="0" cellpadding="0">
            <tr style="height: 10%">
                <td style="width: 10%" align="right">
                    <table width="100%">
                        <tr>
                            <td>
                                <uc1:Header ID="Header1" runat="server" />
                               
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td>
        <table style="width: 352px; height: 336px">
            <tr>
                <td style="width: 155px; height: 21px">
                    Reference
                </td>
                <td style="width: 156px; height: 21px">
                    <asp:TextBox ID="txtReference" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 155px; height: 12px">
                    Type</td>
                <td style="width: 156px; height: 12px">
                    <asp:DropDownList ID="ddlType" runat="server" Width="152px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 155px; height: 21px">
                    Bank Account</td>
                <td style="width: 156px; height: 21px">
                    <asp:DropDownList ID="ddlBankAccount" runat="server" Width="152px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 155px">
                    Currency</td>
                <td style="width: 156px">
                    <asp:DropDownList ID="ddlCurrency" runat="server" Width="152px">
                        <asp:ListItem Value="0">Pounds Sterling </asp:ListItem>
                        <asp:ListItem Value="1">United States Dollars</asp:ListItem>
                        <asp:ListItem Value="2">Euro</asp:ListItem>
                        <asp:ListItem Value="3">Barbados Dollars</asp:ListItem>
                        <asp:ListItem Value="4">Canadian Dollars</asp:ListItem>
                        <asp:ListItem Value="5">Trinidad &amp; Tobago Dollars</asp:ListItem>
                        <asp:ListItem Value="6">South African Rand</asp:ListItem>
                        <asp:ListItem Value="7">Hungarian Forints</asp:ListItem>
                        <asp:ListItem Value="8">Bermudan Dollar</asp:ListItem>
                        <asp:ListItem Value="9">American Currency</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 155px">
                    <strong>
                    Date</strong></td>
                <td style="width: 156px">
                    <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtDate"
                        ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td style="width: 155px; height: 9px">
                    Status</td>
                <td style="width: 156px; height: 9px">
                    <asp:TextBox ID="txtStatus" runat="server" ReadOnly="True">Entered</asp:TextBox></td>
                    <td style="width: 156px; height: 9px">
                    <asp:TextBox ID="TextBox1" runat="server" Visible="False"></asp:TextBox></td>
            </tr>
        </table>
    </td>
    </tr>
    </table>
    </div>
        <asp:Button ID="btnOk" runat="server" Text="Ok" Width="64px" />
        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" Text="Cancel" />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="False" />
        
    </form>
</body>
</html>
