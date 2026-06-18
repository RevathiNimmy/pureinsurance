<%@ Page Language="VB" MaintainScrollPositionOnPostback="true" AutoEventWireup="false" CodeFile="AttachCoverNote.aspx.vb" Inherits="Claim_Payment_CashList" %>

<%@ Register Src="../UserControl/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Cover Notet</title>
    <base target="_self"/>
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Images/loginstylesheet.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Images/loginstylesheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="100%" style="height: 100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="height: 10%">
                    <td style="width: 10%" align="right">
                        <br />
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
                        <asp:Label ID="lblSamErrorMessage" runat="server" ForeColor="Red"></asp:Label></td>
                </tr>
                <tr>
                    <td valign="top">
                        <table class="body" width="100%" border ="0">
                            <tr>
                                <td >
                                    Risk No.</td>
                                <td style="width:90%">
                                    <asp:Label ID="lblRiskNo" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    Risk Description</td>
                                <td>
                                    <asp:TextBox ID="txtRiskDescription" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    Cover Note Number</td>
                                <td>
                                    <asp:TextBox ID="txtCoverNoteNumber" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    Cover From Date</td>
                                <td>
                                    <asp:TextBox ID="txtCoverFromDate" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>Cover To Date</strong></td>
                                <td>
                                    <asp:TextBox ID="txtCoverToDate" runat="server"></asp:TextBox>
                                    </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnOk" runat="server" Text="Ok" Width="61px" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr><td>
                    &nbsp;</td></tr>
        <tr>
        <td>
            <uc2:Footer ID="Footer1" runat="server" />
        </td>
        </tr>
            </table>
            <%-- <table  class="body">
                <tr>
                    <td>
                        Reference
                    </td>
                    <td>
                        <asp:TextBox ID="txtReference" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        Type</td>
                    <td>
                        <asp:DropDownList ID="ddlType" runat="server" Width="152px">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td>
                        Bank Account</td>
                    <td>
                        <asp:DropDownList ID="ddlBankAccount" runat="server" Width="152px">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td>
                        Currency</td>
                    <td>
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
                    <td>
                        <strong>Date</strong></td>
                    <td>
                        <asp:TextBox ID="txtDate" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        Status</td>
                    <td>
                        <asp:TextBox ID="txtStatus" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        &nbsp;&nbsp;
                    </td>
                </tr>
            </table>--%>
        </div>
        &nbsp;
    </form>
</body>
</html>
