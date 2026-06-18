<%@ Page Language="VB" AutoEventWireup="false" CodeFile="4_Perils.aspx.vb" Inherits="OpenClaim_Peril" SmartNavigation = "true"%>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Perils</title>
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
                <tr style="height: 90%">
                   <td>
                    <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
            ForeColor="#C000C0" Text="Perils" Width="73px"></asp:Label><br />
        <table>
            <tr>
                <td style="width: 567px">
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
                </td>
            </tr>
            <tr>
                <td style="width: 567px">
                    <asp:MultiView ID="MvPerils" runat="server" ActiveViewIndex="0">
                        <asp:View ID="Perils" runat="server">
                            &nbsp;<br /><asp:GridView ID="gvperils" runat="server" CellPadding="4" ForeColor="#333333"
                                GridLines="None" Width="100%">
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
                                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatColumns="3" Width="432px">
                                            <asp:ListItem>Client</asp:ListItem>
                                            <asp:ListItem Value="0">ClaimPayble</asp:ListItem>
                                            <asp:ListItem>Party</asp:ListItem>
                                        </asp:RadioButtonList><br />
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
                                    <table>
                                    <tr>
                                    <td style ="width :15%">Risk Type</td>
                                    <td style ="width :35%"><asp:Label ID="lblRisk" runat="server" ></asp:Label></td>
                                    <td style ="width :15%">Total Reserve</td>
                                    <td style ="width :35%"><asp:Label ID="lblTotalReserve" runat="server" ></asp:Label></td>
                                    </tr>
                                    <tr>
                                    <td style ="width :15%">For Reserve</td>
                                    <td style ="width :35%"><asp:Label ID="lblReserve" runat="server" ></asp:Label></td>
                                    <td style ="width :15%">Paid To date</td>
                                    <td style ="width :35%"><asp:Label ID="lblPaidToDate" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                    <td colspan ="2"><strong>Payment Currency</strong></td>
                                    <td colspan ="2"><strong>Loss Currency</strong></td>
                                    </tr>
                                    <tr>
                                    <td style ="width :15%">Currency</td>
                                    <td style ="width :35%"><asp:DropDownList ID="DropDownList1" runat="server" >
                                        </asp:DropDownList>
                                        </td>
                                        <td style ="width :15%">Currency</td>
                                        <td style ="width :35%"><asp:Label ID="lblCurrency"
                                            runat="server" Text="Label" ></asp:Label></td>
                                    </tr>
                                    <tr>
                                    <td>Currency Rate</td>
                                    <td><asp:Label ID="lblCurrencyRate" runat="server" ></asp:Label></td>
                                    </tr>
                                    <tr>
                                    <td>Payment Amount</td>
                                    <td><asp:TextBox ID="txtPaymentAmount" runat="server"></asp:TextBox></td>
                                    <td>Payment Amount</td>
                                    <td><asp:Label ID="lblLossPaymentAmount" runat="server" ></asp:Label></td>
                                    </tr>
                                    <tr>
                                    <td>Tax Group</td>
                                    <td><asp:DropDownList ID="ddlTaxGroup" runat="server" Width="120px">
                                        </asp:DropDownList></td>
                                    </tr>
                                     <tr>
                                    <td>Tax Amount</td>
                                    <td><asp:Label ID="lblTaxAmount" runat="server" ></asp:Label></td>
                                    <td>Tax Amount</td>
                                    <td><asp:Label ID="lblLossCurrencyTaxAmount" runat="server" ></asp:Label></td>
                                    </tr>
                                     <tr>
                                    <td>Net Payment</td>
                                    <td><asp:Label ID="lblNetPayment" runat="server" ></asp:Label></td>
                                    <td>Net Payment</td>
                                    <td><asp:Label ID="lblLossCurrencyNetPayment" runat="server"></asp:Label></td>
                                    </tr>
                                    </table>
                                    <asp:Button ID="btnPaymentDetailOk" runat="server" Text="OK" />
                                    <asp:Button ID="btnPDCancel" runat="server" Text="Cancel" /></asp:Panel>
                                    &nbsp;&nbsp;
                                </asp:View>
                                    <asp:View ID="Payee" runat="server">
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        Media Type &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                        <asp:DropDownList ID="ddlMediaType" runat="server" Width="152px">
                                        </asp:DropDownList>
                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Media Ref &nbsp; &nbsp;
                                        <asp:TextBox ID="txtMediaReference" runat="server"></asp:TextBox><br />
                                        <br />
                                        Payee Name &nbsp; &nbsp; &nbsp; &nbsp;
                                        <asp:TextBox ID="txtPayeeName" runat="server"></asp:TextBox><br />
                                        <br />
                                        Cheque Date &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                        <asp:TextBox ID="txtChequeDate" runat="server"></asp:TextBox><br />
                                        <br />
                                        Bank Name &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                        <asp:TextBox ID="txtBankName" runat="server"></asp:TextBox><br />
                                        <br />
                                        Bank Code &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                        <asp:TextBox ID="txtBankCode" runat="server"></asp:TextBox><br />
                                        <br />
                                        Bank Account No &nbsp;&nbsp;
                                        <asp:TextBox ID="txtBankAccountNo" runat="server"></asp:TextBox><br />
                                        <br />
                                        Their Reference &nbsp; &nbsp; &nbsp;
                                        <asp:TextBox ID="txtTheirReference" runat="server"></asp:TextBox></asp:View>
                                
                               
                                
                            </asp:MultiView>
                            </asp:View>
                          
                        <asp:View ID="GenralDetails" runat="server">
                        </asp:View>
                    </asp:MultiView>
                    </asp:View>
                    </asp:MultiView></td>
            </tr>
            <tr>
                <td style="width: 100%">
                <hr />
                    <asp:Button ID="btnOk" runat="server" Text="Ok" /></td>
            </tr>
        </table>
        </td>
                </tr>
            </table>
    
    
    
    
    
    
       <%-- <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
            ForeColor="#C000C0" Text="Perils" Width="73px"></asp:Label><br />
        <table>
            <tr>
                <td style="width: 567px">
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
                </td>
            </tr>
            <tr>
                <td style="width: 567px">
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
                                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatColumns="3" Width="432px">
                                            <asp:ListItem>Client</asp:ListItem>
                                            <asp:ListItem Value="0">ClaimPayble</asp:ListItem>
                                            <asp:ListItem>Party</asp:ListItem>
                                        </asp:RadioButtonList><br />
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
                                        Risk Type&nbsp;
                                        <asp:Label ID="lblRisk" runat="server" Width="192px"></asp:Label>
                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; Total Reserve &nbsp;
                                        <asp:Label ID="lblTotalReserve" runat="server" Width="160px"></asp:Label><br />
                                        <br />
                                        For Reserve&nbsp;
                                        <asp:Label ID="lblReserve" runat="server" Width="176px"></asp:Label>
                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Paid To date&nbsp;
                                        <asp:Label ID="lblPaidToDate" runat="server" Width="176px"></asp:Label><br />
                                        <hr />
                                        <strong>Payment Currency &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                            Loss Currency<br />
                                            <br />
                                        </strong>Currency: &nbsp;
                                        <asp:DropDownList ID="DropDownList1" runat="server" Width="152px">
                                        </asp:DropDownList>
                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;Currency: &nbsp; &nbsp;<asp:Label ID="lblCurrency"
                                            runat="server" Text="Label" Width="136px"></asp:Label><br />
                                        <br />
                                        Currency Rate&nbsp;
                                        <asp:Label ID="lblCurrencyRate" runat="server" Width="152px"></asp:Label><br />
                                        <br />
                                        Payment Amount :&nbsp;&nbsp; &nbsp; &nbsp;<asp:TextBox ID="txtPaymentAmount" runat="server"></asp:TextBox>
                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                        &nbsp;&nbsp; Payment Amount&nbsp;
                                        <asp:Label ID="lblLossPaymentAmount" runat="server" Width="128px"></asp:Label><br />
                                        <hr />
                                        Tax Group &nbsp;&nbsp;
                                        <asp:DropDownList ID="ddlTaxGroup" runat="server" Width="120px">
                                        </asp:DropDownList><br />
                                        <br />
                                        Tax Amount : &nbsp;&nbsp;
                                        <asp:Label ID="lblTaxAmount" runat="server" Width="128px"></asp:Label>
                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                        &nbsp; &nbsp; &nbsp; &nbsp; Tax Amount: &nbsp;&nbsp;
                                        <asp:Label ID="lblLossCurrencyTaxAmount" runat="server" Width="152px"></asp:Label><br />
                                        <hr />
                                        &nbsp;Net Payment :&nbsp;
                                        <asp:Label ID="lblNetPayment" runat="server" Width="144px"></asp:Label>
                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                        &nbsp; &nbsp; Net Payment &nbsp; &nbsp;
                                        <asp:Label ID="lblLossCurrencyNetPayment" runat="server" Width="136px"></asp:Label><br />
                                    <asp:Button ID="btnPaymentDetailOk" runat="server" Text="OK" />
                                    <asp:Button ID="btnPDCancel" runat="server" Text="Cancel" /></asp:Panel>
                                    &nbsp;&nbsp;
                                </asp:View>
                                    <asp:View ID="Payee" runat="server">
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        <br />
                                        Media Type &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                        <asp:DropDownList ID="ddlMediaType" runat="server" Width="152px">
                                        </asp:DropDownList>
                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Media Ref &nbsp; &nbsp;
                                        <asp:TextBox ID="txtMediaReference" runat="server"></asp:TextBox><br />
                                        <br />
                                        Payee Name &nbsp; &nbsp; &nbsp; &nbsp;
                                        <asp:TextBox ID="txtPayeeName" runat="server"></asp:TextBox><br />
                                        <br />
                                        Cheque Date &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                        <asp:TextBox ID="txtChequeDate" runat="server"></asp:TextBox><br />
                                        <br />
                                        Bank Name &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                        <asp:TextBox ID="txtBankName" runat="server"></asp:TextBox><br />
                                        <br />
                                        Bank Code &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                        <asp:TextBox ID="txtBankCode" runat="server"></asp:TextBox><br />
                                        <br />
                                        Bank Account No &nbsp;&nbsp;
                                        <asp:TextBox ID="txtBankAccountNo" runat="server"></asp:TextBox><br />
                                        <br />
                                        Their Reference &nbsp; &nbsp; &nbsp;
                                        <asp:TextBox ID="txtTheirReference" runat="server"></asp:TextBox></asp:View>
                                
                               
                                
                            </asp:MultiView>
                            </asp:View>
                          
                        <asp:View ID="GenralDetails" runat="server">
                        </asp:View>
                    </asp:MultiView>
                    </asp:View>
                    </asp:MultiView></td>
            </tr>
            <tr>
                <td style="width: 567px">
                <hr />
                    <asp:Button ID="btnOk" runat="server" Text="Ok" /></td>
            </tr>
        </table>--%>
    
    </div>
    </form>
</body>
</html>
