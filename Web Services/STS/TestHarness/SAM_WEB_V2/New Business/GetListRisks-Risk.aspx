<%@ Page Language="VB" MaintainScrollPositionOnPostback="true" AutoEventWireup="false" CodeFile="GetListRisks-Risk.aspx.vb"
    Title="GetListRisks-Risk" Inherits="GetListRisksRisk" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table border="0" cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td align="left">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label></td>
                </tr>
                <tr>
                    <td align="left">
                        <table border="0" style="background-color: #FFF5EE" cellpadding="4" cellspacing="0"
                            width="900px">
                            <tr>
                                <td>
                                    InsuranceFileKey&nbsp</td>
                                <td>
                                    <asp:Label ID="lblInsuranceFileKey" runat="server" ForeColor="Blue"></asp:Label>&nbsp
                                </td>
                                <td>
                                    ClientCode&nbsp
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblClientCode" runat="server" ForeColor="Blue"></asp:Label>&nbsp</td>
                            </tr>
                            <tr>
                                <td>
                                    InsuranceFileRef&nbsp
                                </td>
                                <td>
                                    <asp:Label ID="lblInsuranceFileRef" runat="server" ForeColor="Blue"></asp:Label>&nbsp
                                </td>
                                <td>
                                    Agent&nbsp
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lblAgent" runat="server" ForeColor="Blue"></asp:Label>&nbsp</td>
                            </tr>
                            <tr>
                                <td>
                                    InceptionDate&nbsp
                                </td>
                                <td>
                                    <asp:Label ID="lblInceptionDate" runat="server" ForeColor="Blue"></asp:Label>&nbsp
                                </td>
                                <td>
                                    CoverStartDate&nbsp
                                </td>
                                <td>
                                    <asp:Label ID="lblCoverStartDate" runat="server" ForeColor="Blue"></asp:Label>&nbsp
                                </td>
                                <td>
                                    ExpiryDate&nbsp
                                </td>
                                <td style="width: 95px">
                                    <asp:Label ID="lblExpiryDate" runat="server" ForeColor="Blue"></asp:Label>&nbsp
                                </td>
                            </tr>
                        </table>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="3">
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
                                <asp:MenuItem Text="Risk" Value="Risk"></asp:MenuItem>
                                <asp:MenuItem Text="Policy Fees" Value="Policy Fees"></asp:MenuItem>
                                <asp:MenuItem Text="Policy Tax" Value="Policy Tax"></asp:MenuItem>
                                <asp:MenuItem Text="Agent Commission" Value="Agent Commission"></asp:MenuItem>
                            </Items>
                            <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                        </asp:Menu>
                        <%--<asp:Menu ID="Menu1" runat="server" Orientation="Horizontal" Width="341px">
                            <Items>
                                
                            </Items>
                        </asp:Menu>--%>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="3">
                        <asp:MultiView ID="MultiView1" runat="server">
                            <asp:View ID="View1" runat="server">
                                <div id="bar" style="overflow: auto; height: 150px; width: 900px;">
                                    <asp:GridView ID="grdLiskRisk" runat="server" CellPadding="4" ForeColor="#333333"
                                        Width="100%">
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <RowStyle ForeColor="#000066" Wrap="False" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButton1" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"RiskKey")%>'
                                                        CommandName="Select" runat="server">Select</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <br />
                                <asp:Button ID="btnAddRisk" runat="server" Text="Add" />
                                <asp:Button ID="btnEditRisk" runat="server" Text="Edit" />
                            </asp:View>
                            <asp:View ID="View2" runat="server">
                                <div id="Div1" style="overflow: auto; height: 150px; width: 800px;">
                                    <asp:GridView ID="grdPolicyFees" runat="server" CellPadding="4" ForeColor="#333333"
                                        GridLines="Both">
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <RowStyle ForeColor="#000066" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                    </asp:GridView>
                                </div>
                            </asp:View>
                            <asp:View ID="View3" runat="server">
                                <div id="Div2" style="overflow: auto; height: 150px; width: 800px;">
                                    <asp:GridView ID="grdPolicyTaxes" runat="server" CellPadding="4" ForeColor="#333333"
                                        GridLines="Both">
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <RowStyle ForeColor="#000066" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                    </asp:GridView>
                                </div>
                            </asp:View>
                            <asp:View ID="View4" runat="server">
                                <div id="Div3" style="overflow: auto; height: 150px; width: 800px;">
                                    <asp:GridView ID="grdAgentCommission" runat="server" CellPadding="4" ForeColor="#333333"
                                        GridLines="Both">
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <RowStyle ForeColor="#000066" />
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                    </asp:GridView>
                                </div>
                            </asp:View>
                        </asp:MultiView>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="3">
                        <table border="0" style="background-color: #FFF5EE" cellspacing="0" cellpadding="4"
                            width="900px">
                            <tr>
                                <td>
                                    Currency&nbsp
                                </td>
                                <td>
                                    <asp:Label ID="lblCurrency" runat="server" ForeColor="Blue"></asp:Label>&nbsp
                                </td>
                                <td>
                                    NetTotal&nbsp
                                </td>
                                <td>
                                    <asp:Label ID="lblNetTotal" runat="server" ForeColor="Blue"></asp:Label>&nbsp
                                </td>
                                <td>
                                    TaxTotal&nbsp
                                </td>
                                <td>
                                    <asp:Label ID="lblTaxTotal" runat="server" ForeColor="Blue"></asp:Label>&nbsp
                                </td>
                                <td>
                                    FeeTotal&nbsp
                                </td>
                                <td>
                                    <asp:Label ID="lblFeeTotal" runat="server" ForeColor="Blue"></asp:Label>&nbsp
                                </td>
                                <td>
                                    GrossTotal&nbsp
                                </td>
                                <td>
                                    <asp:Label ID="lblGrossTotal" runat="server"></asp:Label>&nbsp
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="3" style="text-align: right">
                        <asp:Button ID="btnSaveQuote" runat="server" Text="Save Quote" />
                        <asp:Button ID="btnMakeLive" runat="server" Text="Make Live" /></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
