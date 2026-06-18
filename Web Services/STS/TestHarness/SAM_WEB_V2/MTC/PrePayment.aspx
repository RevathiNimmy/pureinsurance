<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PrePayment.aspx.vb" Inherits="New_Business_PrePayment" %>

<%@ Register Src="../UserControl/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
   
 
</head>
<body>
    <form id="form1" runat="server">
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
            <tr style="height: 90%">
                <td>
                    <div>
                        <asp:Label ID="lblSamErrorMessage" runat="server" ForeColor="Red"></asp:Label><br />
                        <br />
                        <table style="width: 692px">
                            <tr>
                                <td style="width: 271px">
                                    Policy Ref &nbsp;
                                    <asp:Label ID="lblPolicyRef" runat="server" Width="132px"></asp:Label></td>
                                <td>
                                    Total Due
                                    <asp:Label ID="lblTotalDue" runat="server" Width="196px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" Width="435px"
                                        AutoPostBack="True">
                                        <asp:ListItem>Agent</asp:ListItem>
                                        <asp:ListItem>Client</asp:ListItem>
                                    </asp:RadioButtonList></td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 109px">
                                    Debit Against &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    <table style="width: 480px">
                                        <tr>
                                            <td style="width: 131px; height: 128px">
                                                <br />
                                                <asp:RadioButtonList ID="rblDebitAgainst" runat="server" Height="84px">
                                                    <asp:ListItem Value="2">OverDraft</asp:ListItem>
                                                    <asp:ListItem Value="1">Floating</asp:ListItem>
                                                    <asp:ListItem>Account</asp:ListItem>
                                                    <asp:ListItem Value="3">UnAllocated</asp:ListItem>
                                                </asp:RadioButtonList></td>
                                            <td style="height: 128px">
                                                &nbsp;<br />
                                                <br />
                                                &nbsp;&nbsp;
                                                <asp:TextBox ID="txtOverDraft" runat="server"></asp:TextBox>&nbsp;<br />
                                                &nbsp;&nbsp;
                                                <asp:TextBox ID="txtFloatBalance" runat="server"></asp:TextBox>&nbsp;<br />
                                                &nbsp;&nbsp;
                                                <asp:TextBox ID="txtAccount" runat="server"></asp:TextBox><br />
                                                &nbsp;&nbsp;
                                                <asp:TextBox ID="txtUnAllocatedCredit" runat="server"></asp:TextBox><br />
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                    <br />
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                    <br />
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Panel ScrollBars=Auto runat=server ID=pnl Width="691px">
                     <asp:GridView ID="gvPrePayment" runat="server" CellPadding="4" ForeColor="#333333"
                        GridLines="None">
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" runat="server" OnCheckedChanged="Checkboxchanged" AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <EditRowStyle BackColor="#999999" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                    
                    </asp:Panel>
                   
                    <asp:Button ID="btnMakeLive" runat="server" Text="Make Live" />
                    <br />
                    <br />
                    <asp:Label ID="lblPolicyNum" runat="server" Visible="False"></asp:Label>
                </td>
            </tr>
            <tr style="height:150px">
            <td>
                <uc2:Footer ID="Footer1" runat="server" />
            
            </td>
            </tr>
        </table>
    </form>
</body>
</html>
