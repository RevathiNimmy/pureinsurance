<%@ Page Language="VB" AutoEventWireup="false" CodeFile="4_Perils.aspx.vb" Inherits="OpenClaim_Peril" MaintainScrollPositionOnPostback ="true" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Perils</title>

    <script type="text/javascript" language="Javascript">
   function  LoadWindows()
    {
    window.open("Find Party.aspx","","width=600,height=650,scrollbars=1");
//window.open(url,"","width=600,height=700,scrollbars=1");
    }
    </script>

</head>
<body bgcolor="#ffffff">
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
                        <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
                            ForeColor="#C000C0" Text="Perils" Width="73px"></asp:Label><br />
                        <table>
                            <tr>
                                <td style="width: 684px">
                                    <asp:Menu ID="Menu1" runat="server" BackColor="#F7F6F3" DynamicHorizontalOffset="2"
                                        Font-Names="Verdana" Font-Size="0.8em" ForeColor="#7C6F57" Orientation="Horizontal"
                                        StaticSubMenuIndent="10px" Width="100%">
                                        <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                                        <DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
                                        <DynamicMenuStyle BackColor="#F7F6F3" />
                                        <StaticSelectedStyle BackColor="#5D7B9D" />
                                        <DynamicSelectedStyle BackColor="#5D7B9D" />
                                        <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                                        <Items>
                                            <asp:MenuItem Text="Perils" Value="0"></asp:MenuItem>
                                            <asp:MenuItem Text="Genral Details" Value="1"></asp:MenuItem>
                                        </Items>
                                        <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                                    </asp:Menu>
                                    &nbsp;&nbsp;<br />
                                    <table>
                                        <tr>
                                            <td style="width: 100px">
                                                Progress Status</td>
                                            <td style="width: 100px">
                                    <asp:DropDownList ID="ddlProgressStatus" runat="server" Width="244px">
                                    </asp:DropDownList></td>
                                            <td style="width: 100px">
                                                &nbsp;Satus</td>
                                            <td style="width: 100px">
                                    <asp:TextBox ID="txtStatus" runat="server" Width="137px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px">
                                    Description 
                                            </td>
                                            <td style="width: 100px" colspan="3">
                                    <asp:TextBox ID="txtDescription" runat="server" Width="456px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px">
                                    Primary Cause</td>
                                            <td style="width: 100px">
                                    <asp:DropDownList ID="ddlPrimaryCause" runat="server" Width="136px">
                                    </asp:DropDownList></td>
                                            <td style="width: 100px">
                                                Seconday Cause</td>
                                            <td style="width: 100px">
                                    <asp:DropDownList ID="ddlSecondary" runat="server" Width="192px">
                                    </asp:DropDownList></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 684px">
                                    <asp:MultiView ID="MvPerils" runat="server" ActiveViewIndex="0">
                                        <asp:View ID="Perils" runat="server">
                                            <asp:GridView ID="gvPerils" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                                                Width="100%">
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <Columns>
                                                    <asp:CommandField ShowSelectButton="True" />
                                                </Columns>
                                                <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                                                <EditRowStyle BackColor="#999999" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="Smaller" ForeColor="White" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            </asp:GridView>
                                            <br />
                                            <br />
                                            <asp:Menu ID="Menu2" runat="server" BackColor="#F7F6F3" DynamicHorizontalOffset="2"
                                                Font-Names="Verdana" Font-Size="0.8em" ForeColor="#7C6F57" Orientation="Horizontal"
                                                StaticSubMenuIndent="10px">
                                                <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                                                <DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
                                                <DynamicMenuStyle BackColor="#F7F6F3" />
                                                <StaticSelectedStyle BackColor="#5D7B9D" />
                                                <DynamicSelectedStyle BackColor="#5D7B9D" />
                                                <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                                                <Items>
                                                    <asp:MenuItem Text="Reserves" Value="0"></asp:MenuItem>
                                                    <asp:MenuItem Text="Payments" Value="1"></asp:MenuItem>
                                                </Items>
                                                <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                                            </asp:Menu>
                                            <br />
                                            <asp:MultiView ID="MultiView1" runat="server">
                                                <asp:View ID="View1" runat="server">
                                                    <asp:GridView ID="gvReserves" runat="server" CellPadding="4" ForeColor="#333333"
                                                        GridLines="None" Width="100%">
                                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                        <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                                                        <EditRowStyle BackColor="#999999" />
                                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="Smaller" ForeColor="White" />
                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    </asp:GridView>
                                                </asp:View>
                                                <asp:View ID="View2" runat="server">
                                                    <br />
                                                    <asp:Menu ID="Menu3" runat="server" BackColor="#F7F6F3" DynamicHorizontalOffset="2"
                                                        Font-Names="Verdana" Font-Size="0.8em" ForeColor="#7C6F57" Orientation="Horizontal"
                                                        StaticSubMenuIndent="10px" Width="232px">
                                                        <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                                                        <DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
                                                        <DynamicMenuStyle BackColor="#F7F6F3" />
                                                        <StaticSelectedStyle BackColor="#5D7B9D" />
                                                        <DynamicSelectedStyle BackColor="#5D7B9D" />
                                                        <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                                                        <Items>
                                                            <asp:MenuItem Text="Payment" Value="0"></asp:MenuItem>
                                                            <asp:MenuItem Enabled="False" Text="This Payment" Value="1"></asp:MenuItem>
                                                        </Items>
                                                        <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                                                    </asp:Menu>
                                                    <br />
                                                    <br />
                                                    <asp:MultiView ID="MvPayment" runat="server">
                                                        <asp:View ID="Payment" runat="server">
                                                            <asp:Panel ID="pnlClientDetails" runat="server" Height="50px" Width="688px">
                                                                Claim Information<br />
                                                                <br />
                                                                Risk Type:<asp:Label ID="lblRiskType" runat="server" Width="160px"></asp:Label>&nbsp;
                                                                Loss Currency:
                                                                <asp:Label ID="lblLossCurrency" runat="server" Width="136px"></asp:Label>
                                                                &nbsp; &nbsp;&nbsp; Loss Date
                                                                <asp:Label ID="lblLossDate" runat="server" Width="128px"></asp:Label><br />
                                                                <br />
                                                                <asp:RadioButtonList ID="rblPaymentPartyType" runat="server" RepeatColumns="3" Width="288px"
                                                                    AutoPostBack="True">
                                                                    <asp:ListItem Value="3">Client</asp:ListItem>
                                                                    <asp:ListItem Value="0">ClaimPayble</asp:ListItem>
                                                                    <asp:ListItem Value="1">Party</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                                <asp:TextBox ID="txtShortName" runat="server"></asp:TextBox>
                                                                <asp:Button ID="btnFindParty" runat="server" Text="..." OnClientClick="LoadWindows()" />
                                                                <br />
                                                                <asp:GridView ID="gvPaymentDetails" runat="server" CellPadding="4" ForeColor="#333333"
                                                                    GridLines="None" Width="608px">
                                                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                                    <EditRowStyle BackColor="#999999" />
                                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                    <Columns>
                                                                        <asp:CommandField ShowSelectButton="True" />
                                                                    </Columns>
                                                                </asp:GridView>
                                                                <br />
                                                                <asp:Button ID="btnEditPayment" runat="server" Text="Edit Payment" Enabled="False" /><br />
                                                                <hr />
                                                                <br />
                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlPaymentDetails" runat="server" Height="368px" Width="696px" Visible="False">
                                                                &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;<br />
                                                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                                                <br />
                                                                <table style="width: 636px">
                                                                    <tr>
                                                                        <td style="width: 100px">
                                                                            Risk Type &nbsp; &nbsp;</td>
                                                                        <td style="width: 100px">
                                                                <asp:Label ID="lblRisk" runat="server" Width="192px"></asp:Label></td>
                                                                        <td style="width: 100px">
                                                                            Total Reserve &nbsp;&nbsp;</td>
                                                                        <td style="width: 100px">
                                                                <asp:Label ID="lblTotalReserve" runat="server" Width="160px"></asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 100px">
                                                                For Reserve&nbsp;&nbsp;</td>
                                                                        <td style="width: 100px">
                                                                <asp:Label ID="lblReserve" runat="server" Width="176px"></asp:Label></td>
                                                                        <td style="width: 100px">
                                                                            &nbsp;Paid To date&nbsp;</td>
                                                                        <td style="width: 100px">
                                                                <asp:Label ID="lblPaidToDate" runat="server" Width="176px"></asp:Label></td>
                                                                    </tr>
                                                                </table>
                                                                <br />
                                                                <hr />
                                                                  <table style="width: 614px">
                                                                    <tr>
                                                                        <td style="width: 100px">
                                                                            <strong>Payment Currency </strong>
                                                                        </td>
                                                                        <td style="width: 100px">
                                                                        </td>
                                                                        <td style="width: 100px">
                                                                            <strong>Loss Currency</strong></td>
                                                                        <td style="width: 129px">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 100px">
                                                                            Currency:&nbsp;</td>
                                                                        <td style="width: 100px">
                                                                <asp:DropDownList ID="ddlCurrency" runat="server" Width="152px">
                                                                </asp:DropDownList></td>
                                                                        <td style="width: 100px">
                                                                            Currency: &nbsp;&nbsp;</td>
                                                                        <td style="width: 129px">
                                                                            <asp:Label ID="lblCurrency"
                                                                    runat="server" Text="Label" Width="136px"></asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 100px">
                                                                Currency Rate&nbsp;</td>
                                                                        <td style="width: 100px">
                                                                <asp:Label ID="lblCurrencyRate" runat="server" Width="152px"></asp:Label></td>
                                                                        <td style="width: 100px">
                                                                        </td>
                                                                        <td style="width: 129px">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 100px">
                                                                            Payment Amount :&nbsp;</td>
                                                                        <td style="width: 100px">
                                                                            <asp:TextBox ID="txtPaymentAmount" runat="server"></asp:TextBox></td>
                                                                        <td style="width: 100px">
                                                                            Payment Amount&nbsp;
                                                                            <br />
                                                                        </td>
                                                                        <td style="width: 129px">
                                                                <asp:Label ID="lblLossPaymentAmount" runat="server" Width="128px"></asp:Label></td>
                                                                    </tr>
                                                                </table>
                                                                <br />
                                                                <hr />
                                                                  <table>
                                                                    <tr>
                                                                        <td style="width: 100px">
                                                                Tax Group</td>
                                                                        <td style="width: 100px">
                                                                <asp:DropDownList ID="ddlTaxGroup" runat="server" Width="120px">
                                                                </asp:DropDownList></td>
                                                                        <td style="width: 100px">
                                                                        </td>
                                                                        <td style="width: 100px">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 100px">
                                                                            Tax Amount : &nbsp; &nbsp;&nbsp;&nbsp;</td>
                                                                        <td style="width: 100px">
                                                                <asp:Label ID="lblTaxAmount" runat="server" Width="128px"></asp:Label></td>
                                                                        <td style="width: 100px">
                                                                            Tax Amount:&nbsp;</td>
                                                                        <td style="width: 100px">
                                                                <asp:Label ID="lblLossCurrencyTaxAmount" runat="server" Width="152px"></asp:Label></td>
                                                                    </tr>
                                                                </table>
                                                                <br />
                                                                <hr />
                                                                &nbsp;Net Payment :&nbsp;
                                                                <asp:Label ID="lblNetPayment" runat="server" Width="144px"></asp:Label>
                                                               
                                                                &nbsp; &nbsp; Net Payment &nbsp; &nbsp;
                                                                <asp:Label ID="lblLossCurrencyNetPayment" runat="server" Width="136px"></asp:Label><br />
                                                                <asp:Button ID="btnPaymentDetailOk" runat="server" Text="OK" />
                                                                <asp:Button ID="btnPDCancel" runat="server" Text="Cancel" /></asp:Panel>
                                                            &nbsp;&nbsp;
                                                        </asp:View>
                                                        <asp:View ID="Payee" runat="server">
                                                            <br />
                                                            <br />
                                                            <table>
                                                                <tr>
                                                                    <td style="width: 100px">
                                                                        Media Type</td>
                                                                    <td style="width: 100px">
                                                            <asp:DropDownList ID="ddlMediaType" runat="server" Width="152px">
                                                            </asp:DropDownList></td>
                                                                    <td style="width: 100px">
                                                                        <strong>
                                                                        Media Ref </strong>
                                                                    </td>
                                                                    <td style="width: 109px">
                                                            <asp:TextBox ID="txtMediaReference" runat="server"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtMediaReference"
                                                                            ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator></td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100px">
                                                            Payee Name &nbsp; &nbsp;</td>
                                                                    <td style="width: 100px">
                                                            <asp:TextBox ID="txtPayeeName" runat="server"></asp:TextBox></td>
                                                                    <td style="width: 100px">
                                                                    </td>
                                                                    <td style="width: 109px">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100px">
                                                            Cheque Date 
                                                                    </td>
                                                                    <td style="width: 100px">
                                                            <asp:TextBox ID="txtChequeDate" runat="server"></asp:TextBox></td>
                                                                    <td style="width: 100px">
                                                                    </td>
                                                                    <td style="width: 109px">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100px">
                                                                        Bank Name
                                                                    </td>
                                                                    <td style="width: 100px">
                                                            <asp:TextBox ID="txtBankName" runat="server"></asp:TextBox></td>
                                                                    <td style="width: 100px">
                                                                    </td>
                                                                    <td style="width: 109px">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100px">
                                                            Bank Code 
                                                                    </td>
                                                                    <td style="width: 100px">
                                                            <asp:TextBox ID="txtBankCode" runat="server"></asp:TextBox></td>
                                                                    <td style="width: 100px">
                                                                    </td>
                                                                    <td style="width: 109px">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100px">
                                                            Bank Account No</td>
                                                                    <td style="width: 100px">
                                                            <asp:TextBox ID="txtBankAccountNo" runat="server"></asp:TextBox></td>
                                                                    <td style="width: 100px">
                                                                    </td>
                                                                    <td style="width: 109px">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100px">
                                                            Their Reference 
                                                                    </td>
                                                                    <td style="width: 100px">
                                                            <asp:TextBox ID="txtTheirReference" runat="server"></asp:TextBox></td>
                                                                    <td style="width: 100px">
                                                                    </td>
                                                                    <td style="width: 109px">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                                  </asp:View>
                                                    </asp:MultiView>
                                                </asp:View>
                                                <asp:View ID="GenralDetails" runat="server">
                                                </asp:View>
                                            </asp:MultiView>
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="False" />
                                        </asp:View>
                                    </asp:MultiView></td>
                            </tr>
                            <tr>
                                <td style="width: 684px">
                                    <hr />
                                    <asp:Button ID="btnOk" runat="server" Text="Ok" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
