<%@ Page Language="VB" AutoEventWireup="false" EnableSessionState="True" CodeFile="GetListRisks_Risk.aspx.vb"
    Title="GetListRisks-Risk" Inherits="GetListRisksRisk" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>

    <script language="javascript" type="text/javascript">
// <!CDATA[



// ]]>
    </script>

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
                <tr style="height: 90%">
                    <td>
                        <asp:Label ID="lblSamErrorMessage" runat="server" ForeColor="Red"></asp:Label><br />
                        <br />
                        <table border="0" cellpadding="2" cellspacing="2" width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label></td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <table border="0" cellpadding="4" cellspacing="0" width="900px" class="body">
                                        <tr>
                                            <td>
                                                InsuranceFileKey&nbsp</td>
                                            <td>
                                                <asp:Label ID="lblInsuranceFileKey" runat="server" ForeColor="Blue" Font-Bold="True"
                                                    Font-Italic="True" Font-Strikeout="False" Font-Underline="False"></asp:Label>&nbsp
                                            </td>
                                            <td>
                                                ClientCode&nbsp
                                            </td>
                                            <td colspan="3">
                                                <asp:Label ID="lblClientCode" runat="server" ForeColor="Blue" Font-Bold="True" Font-Italic="True"
                                                    Font-Strikeout="False" Font-Underline="False"></asp:Label>&nbsp</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                InsuranceFileRef&nbsp
                                            </td>
                                            <td>
                                                <asp:Label ID="lblInsuranceFileRef" runat="server" ForeColor="Blue" Font-Bold="True"
                                                    Font-Italic="True" Font-Strikeout="False" Font-Underline="False"></asp:Label>&nbsp
                                            </td>
                                            <td>
                                                Agent&nbsp
                                            </td>
                                            <td colspan="3">
                                                <asp:Label ID="lblAgent" runat="server" ForeColor="Blue" Font-Bold="True" Font-Italic="True"
                                                    Font-Strikeout="False" Font-Underline="False"></asp:Label>&nbsp</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                InceptionDate
                                            </td>
                                            <td>
                                                <asp:Label ID="lblInceptionDate" runat="server" ForeColor="Blue" Font-Bold="True"
                                                    Font-Italic="True" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                                            </td>
                                            <td>
                                                CoverStartDate
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCoverStartDate" runat="server" ForeColor="Blue" Font-Bold="True"
                                                    Font-Italic="True" Font-Strikeout="False" Font-Underline="False"></asp:Label>
                                            </td>
                                            <td>
                                                ExpiryDate&nbsp
                                            </td>
                                            <td>
                                                <asp:Label ID="lblExpiryDate" runat="server" ForeColor="Blue" Font-Bold="True" Font-Italic="True"
                                                    Font-Strikeout="False" Font-Underline="False"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3" style="height: 43px">
                                    <asp:Menu ID="Menu1" runat="server" BackColor="#F7F6F3" DynamicHorizontalOffset="2"
                                        Font-Names="Verdana" Font-Size="0.8em" ForeColor="#7C6F57" Orientation="Horizontal"
                                        StaticSubMenuIndent="10px">
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
                        </asp:Menu> --%>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3">
                                    <asp:MultiView ID="MultiView1" runat="server">
                                        <asp:View ID="View1" runat="server">
                                            <div id="bar">
                                              
                                                <asp:GridView ID="grdLiskRisk" runat="server" CellPadding="4" ForeColor="#333333"
                                                    Width="28%" GridLines="None" Height="120px">
                                                    <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                                                    <RowStyle ForeColor="#333333" Wrap="False" BackColor="#F7F6F3" />
                                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelectRisk" runat="server" AutoPostBack="True" BorderStyle="None"
                                                                    OnCheckedChanged="chkSelectRisk_CheckedChanged" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LinkButton1" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"RiskKey")%>'
                                                                    CommandName="Edit" runat="server">Edit</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LinkButton2" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"RiskKey")%>'
                                                                    CommandName="Delete" runat="server">Delete</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EditRowStyle BackColor="#999999" />
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                </asp:GridView>
                                            </div>
                                            <asp:Button ID="btnAddRisk" runat="server" Text="Add" />
                                            <asp:Button ID="btnEditRisk" runat="server" Text="Edit" />
                                        </asp:View>
                                        <asp:View ID="View2" runat="server">
                                            <div id="Div1">
                                                <asp:GridView ID="grdPolicyFees" runat="server" CellPadding="4" ForeColor="#333333"
                                                    GridLines="None">
                                                    <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                                                    <RowStyle ForeColor="#333333" BackColor="#F7F6F3" />
                                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <EditRowStyle BackColor="#999999" />
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                </asp:GridView>
                                                <br />
                                                <table id="TABLE2" style="width: 880px" onclick="return TABLE2_onclick()">
                                                    <tr>
                                                        <td>
                                                            <strong>Risk Fees</strong></td>
                                                        <td style="width: 401px">
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Total Fee Eligible for Financing &nbsp;&nbsp;
                                                            <asp:Label ID="lblTotlafeeEligibleForFinancing" runat="server" Width="96px" Font-Bold="True"
                                                                Font-Italic="True" Font-Strikeout="False" Font-Underline="False"></asp:Label></td>
                                                        <td>
                                                            Total Fee Excluded from Financing<asp:Label ID="lblTotlafeeExcludedFromFinancing"
                                                                runat="server" Width="96px" Font-Bold="True" Font-Italic="True" Font-Strikeout="False"
                                                                Font-Underline="False"></asp:Label></td>
                                                        <td>
                                                            Total Risk Fees<asp:Label ID="lblTotalRiskFees" runat="server" Width="56px" Font-Bold="True"
                                                                Font-Italic="True" Font-Strikeout="False" Font-Underline="False"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <strong>Policy fees</strong></td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            Total Fee Eligible for Financing &nbsp; &nbsp;<asp:Label ID="lblPolicyTotlafeeEligibleForFinancing"
                                                                runat="server" Width="96px" Font-Bold="True" Font-Italic="True" Font-Strikeout="False"
                                                                Font-Underline="False"></asp:Label></td>
                                                        <td>
                                                            Total Fee Excluded from Financing<asp:Label ID="lblPolicyTotlafeeExcludedFromFinancing"
                                                                runat="server" Width="96px" Font-Bold="True" Font-Italic="True" Font-Strikeout="False"
                                                                Font-Underline="False"></asp:Label></td>
                                                        <td>
                                                            Total Policy Fees<asp:Label ID="lblTotalPolicyFees" runat="server" Font-Bold="True"
                                                                Font-Italic="True" Font-Strikeout="False" Font-Underline="False"></asp:Label></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </asp:View>
                                        <asp:View ID="View3" runat="server">
                                            <div id="Div2">
                                                <asp:GridView ID="grdPolicyTaxes" runat="server" CellPadding="4" ForeColor="#333333"
                                                    GridLines="None">
                                                    <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                                                    <RowStyle ForeColor="#333333" BackColor="#F7F6F3" />
                                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <EditRowStyle BackColor="#999999" />
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                </asp:GridView>
                                            </div>
                                            <table>
                                                <tr>
                                                    <td style="width: 100px">
                                                        <strong>Risk Taxes</strong></td>
                                                    <td style="width: 100px">
                                                    </td>
                                                    <td style="width: 100px">
                                                    </td>
                                                    <td style="width: 100px">
                                                    </td>
                                                    <td style="width: 100px">
                                                    </td>
                                                    <td style="width: 100px">
                                                    </td>
                                                    <td style="width: 100px">
                                                    </td>
                                                    <td style="width: 100px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100px">
                                                        Total Tax Eligible for Financing : &nbsp;
                                                    </td>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="lblTotlaTaxEligibleForFinancing" runat="server" Width="96px" Font-Bold="True"
                                                            Font-Italic="True" Font-Strikeout="False" Font-Underline="False"></asp:Label></td>
                                                    <td style="width: 100px">
                                                        Total Tax Excluded from Financing :</td>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="lblTotlaTaxExcludedFromFinancing" runat="server" Width="96px" Font-Bold="True"
                                                            Font-Italic="True" Font-Strikeout="False" Font-Underline="False"></asp:Label></td>
                                                    <td style="width: 100px">
                                                        Total Risk Non Client Taxes :</td>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="lblToalRiskNonClienttaxes" runat="server" Width="56px">0.0</asp:Label></td>
                                                    <td style="width: 100px">
                                                        Total Risk Client Taxes :</td>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="lblTotalRiskTaxs" runat="server" Width="56px" Font-Bold="True" Font-Italic="True"
                                                            Font-Strikeout="False" Font-Underline="False"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100px">
                                                        <strong>Policy Taxes</strong></td>
                                                    <td style="width: 100px">
                                                    </td>
                                                    <td style="width: 100px">
                                                    </td>
                                                    <td style="width: 100px">
                                                    </td>
                                                    <td style="width: 100px">
                                                    </td>
                                                    <td style="width: 100px">
                                                    </td>
                                                    <td style="width: 100px">
                                                    </td>
                                                    <td style="width: 100px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100px">
                                                        Total Tax Eligible for Financing : &nbsp;
                                                    </td>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="lblPolicyTotlaTaxEligibleForFinancing" runat="server" Width="96px"
                                                            Font-Bold="True" Font-Italic="True" Font-Strikeout="False" Font-Underline="False"></asp:Label></td>
                                                    <td style="width: 100px">
                                                        Total Tax Excluded from Financing :</td>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="lblPolicyTotlaTaxExcludedFromFinancing" runat="server" Width="96px"
                                                            Font-Bold="True" Font-Italic="True" Font-Strikeout="False" Font-Underline="False"></asp:Label></td>
                                                    <td style="width: 100px">
                                                        Total Policy Non Client Taxes :</td>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="lblTotalPolicyNonClientTaxes" runat="server">0.0</asp:Label></td>
                                                    <td style="width: 100px">
                                                        Total Policy Client Taxes :</td>
                                                    <td style="width: 100px">
                                                        <asp:Label ID="lblTotalPolicyTaxs" runat="server" Font-Bold="True" Font-Italic="True"
                                                            Font-Strikeout="False" Font-Underline="False"></asp:Label></td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="View4" runat="server">
                                            <div id="Div3">
                                                <asp:GridView ID="grdAgentCommission" runat="server" CellPadding="4" ForeColor="#333333"
                                                    GridLines="None">
                                                    <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                                                    <RowStyle ForeColor="#333333" BackColor="#F7F6F3" />
                                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <EditRowStyle BackColor="#999999" />
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                </asp:GridView>
                                                <table style="width: 760px">
                                                    <tr>
                                                        <td colspan="3">
                                                            <strong>Agent Totals<br />
                                                            </strong>
                                                            <table>
                                                                <tr>
                                                                    <td style="width: 100px">
                                                                        <strong>AgentType </strong>
                                                                    </td>
                                                                    <td style="width: 100px">
                                                                        <strong>Total Commission </strong>
                                                                    </td>
                                                                    <td style="width: 100px">
                                                                        <strong>Total Tax </strong>
                                                                    </td>
                                                                    <td style="width: 100px">
                                                                        <strong>TotalNetPremium</strong><br />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100px">
                                                                        Lead agent
                                                                    </td>
                                                                    <td style="width: 100px">
                                                                        <asp:Label ID="lblLeadAgentCommission" runat="server" Text="0.0" Font-Bold="True"
                                                                            Font-Italic="True" Font-Strikeout="False" Font-Underline="False"></asp:Label></td>
                                                                    <td style="width: 100px">
                                                                        <asp:Label ID="lblTotalTax" runat="server" Text="0.0" Font-Bold="True" Font-Italic="True"
                                                                            Font-Strikeout="False" Font-Underline="False"></asp:Label></td>
                                                                    <td style="width: 100px">
                                                                        <asp:Label ID="lblLeadAgentTotalNetPremium" runat="server" Text="0.0" Font-Bold="True"
                                                                            Font-Italic="True" Font-Strikeout="False" Font-Underline="False"></asp:Label></td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100px">
                                                                        Sub-Agnet &nbsp; &nbsp;</td>
                                                                    <td style="width: 100px">
                                                                        <asp:Label ID="lblSubAgentCommission" runat="server" Text="0.0" Font-Bold="True"
                                                                            Font-Italic="True" Font-Strikeout="False" Font-Underline="False"></asp:Label></td>
                                                                    <td style="width: 100px">
                                                                        <asp:Label ID="lblSubAgentTotalTax" runat="server" Text="0.0" Font-Bold="True" Font-Italic="True"
                                                                            Font-Strikeout="False" Font-Underline="False"></asp:Label></td>
                                                                    <td style="width: 100px">
                                                                        <asp:Label ID="lblSubagentPremium" runat="server" Text="0.0" Font-Bold="True" Font-Italic="True"
                                                                            Font-Strikeout="False" Font-Underline="False"></asp:Label></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </asp:View>
                                        <asp:View ID="Installments" runat="server">
                                            <table style="width: 548px">
                                                <tr>
                                                    <td colspan="2" style="height: 21px">
                                                        Use Existing Plan Details</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 247px">
                                                        Bank Details &nbsp;
                                                        <asp:DropDownList ID="ddlBankDetails" runat="server" Width="138px">
                                                        </asp:DropDownList></td>
                                                    <td style="width: 293px">
                                                        Credit Card Details&nbsp;
                                                        <asp:DropDownList ID="ddlCreditCardDetails" runat="server" Width="155px">
                                                        </asp:DropDownList></td>
                                                </tr>
                                            </table>
                                            <br />
                                            <asp:MultiView ID="MultiView2" runat="server">
                                                <asp:View ID="PlansAvailable" runat="server">
                                                    <table>
                                                        <tr>
                                                            <td style="width: 100px">
                                                                Financed Amount</td>
                                                            <td style="width: 100px">
                                                                <asp:TextBox ID="txtFinancedAmount" runat="server"></asp:TextBox></td>
                                                            <td style="width: 100px">
                                                                Total Payable</td>
                                                            <td style="width: 100px">
                                                                <asp:TextBox ID="txtTotalPayable" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100px">
                                                                Transactions</td>
                                                            <td style="width: 100px">
                                                                <asp:TextBox ID="txtTransactions" runat="server"></asp:TextBox></td>
                                                            <td style="width: 100px">
                                                                Instalments</td>
                                                            <td style="width: 100px">
                                                                <asp:TextBox ID="Instalments" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100px">
                                                                Rate:
                                                            </td>
                                                            <td style="width: 100px">
                                                                <asp:TextBox ID="txtRate" runat="server"></asp:TextBox></td>
                                                            <td style="width: 100px">
                                                                APR: &nbsp; &nbsp;</td>
                                                            <td style="width: 100px">
                                                                <asp:TextBox ID="txtAPR" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                    </table>
                                                </asp:View>
                                                <asp:View ID="Details" runat="server">
                                                    BreakDown&nbsp;<table>
                                                        <tr>
                                                            <td style="width: 100px">
                                                                Deposit
                                                            </td>
                                                            <td style="width: 100px">
                                                                <asp:TextBox ID="txtDeposit" runat="server"></asp:TextBox></td>
                                                            <td style="width: 100px">
                                                                Admin Charge&nbsp;</td>
                                                            <td style="width: 100px">
                                                                <asp:TextBox ID="txtAdminCharge" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100px">
                                                                Protection Charge &nbsp;&nbsp;
                                                            </td>
                                                            <td style="width: 100px">
                                                                <asp:TextBox ID="txtProtectionCharge" runat="server"></asp:TextBox></td>
                                                            <td style="width: 100px">
                                                                Interest
                                                            </td>
                                                            <td style="width: 100px">
                                                                <asp:TextBox ID="txtInterest" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                    </table>
                                                    <hr />
                                                    <table>
                                                        <tr>
                                                            <td style="width: 100px">
                                                                First Instalment Date</td>
                                                            <td style="width: 100px">
                                                                <asp:TextBox ID="txtFirstInstalmentDate" runat="server"></asp:TextBox></td>
                                                            <td style="width: 100px">
                                                                First Instalment&nbsp;
                                                            </td>
                                                            <td style="width: 100px">
                                                                <asp:TextBox ID="txtFirstInstalment" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100px">
                                                                Next Instalment Date
                                                            </td>
                                                            <td style="width: 100px">
                                                                <asp:TextBox ID="txtNextInstalmentDate" runat="server"></asp:TextBox></td>
                                                            <td style="width: 100px">
                                                                Next Instalment</td>
                                                            <td style="width: 100px">
                                                                <asp:TextBox ID="txtNextInstalment" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100px">
                                                                Last Instalment Date
                                                            </td>
                                                            <td style="width: 100px">
                                                                <asp:TextBox ID="txtLastInstalmentDate" runat="server"></asp:TextBox></td>
                                                            <td style="width: 100px">
                                                                Last Instalment</td>
                                                            <td style="width: 100px">
                                                                <asp:TextBox ID="txtLastInstalment" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                    </table>
                                                </asp:View>
                                                <asp:View ID="Ovverride" runat="server">
                                                    <table style="width: 560px">
                                                        <tr>
                                                            <td style="width: 238px; height: 22px">
                                                                New Rate &nbsp;&nbsp;<asp:CheckBox ID="chkOverrideInterestRate" runat="server" Text="Ovveride Interest Rate?" /><br />
                                                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                                                <asp:TextBox ID="txtNewRate" runat="server" Width="56px"></asp:TextBox></td>
                                                            <td style="width: 175px; height: 22px">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 238px">
                                                                reference &nbsp; &nbsp;
                                                                <asp:CheckBox ID="CommissionOverride" runat="server" Text="CommissionOverride?" /><br />
                                                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                                                <asp:TextBox ID="txtReference" runat="server" Width="54px"></asp:TextBox></td>
                                                            <td style="width: 175px">
                                                                <asp:CheckBox ID="chkDoNotIncludePayment" runat="server" Text="Do not Include Payment Protection"
                                                                    Width="251px" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 238px; height: 46px">
                                                                deposit &nbsp; &nbsp; &nbsp;&nbsp;
                                                                <asp:CheckBox ID="chkDepositOverride" runat="server" Text="ChkDepositOverride" />
                                                                <br />
                                                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                                                <asp:TextBox ID="txtDeposit2" runat="server" Width="67px"></asp:TextBox></td>
                                                            <td style="width: 175px; height: 46px">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:View>
                                                <asp:View ID="OtherDetails" runat="server">
                                                    Financial Details<br />
                                                    <br />
                                                    <table id="TABLE1" style="width: 545px; height: 116px;" onclick="return TABLE1_onclick()">
                                                        <tr>
                                                            <td style="width: 293px; height: 40px">
                                                                Gross Due From Client</td>
                                                            <td style="width: 2px; height: 40px">
                                                                <asp:TextBox ID="txtGrossDue" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 293px">
                                                                Total Fee Excluded from Financing</td>
                                                            <td style="width: 2px">
                                                                <asp:TextBox ID="txtFeeExcluded" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 293px">
                                                                Total Taxes Excluded from financing</td>
                                                            <td style="width: 2px">
                                                                <asp:TextBox ID="txtTaxesExcluded" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 293px">
                                                                Total Amount able to be financed
                                                            </td>
                                                            <td style="width: 2px">
                                                                <asp:TextBox ID="txtTotalAmount" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                    </table>
                                                    <table style="width: 543px">
                                                        <tr>
                                                            <td style="width: 294px; height: 26px">
                                                                Total Fee Must be Collected as deposit</td>
                                                            <td style="width: 3px; height: 26px">
                                                                <asp:TextBox ID="txtTotalFeeDeposit" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 294px">
                                                                Total taxes must be collected as deposit</td>
                                                            <td style="width: 3px">
                                                                <asp:TextBox ID="txtTotalTaxesDeposit" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 294px">
                                                                Total minimum deposit</td>
                                                            <td style="width: 3px">
                                                                <asp:TextBox ID="txtTotalMinimumDeposit" runat="server"></asp:TextBox></td>
                                                        </tr>
                                                    </table>
                                                </asp:View>
                                            </asp:MultiView></asp:View>
                                    </asp:MultiView>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3">
                                    <table border="0" cellspacing="0" cellpadding="4" width="900px" class="body">
                                        <tr>
                                            <td>
                                                Currency
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCurrency" runat="server" ForeColor="Blue" Font-Bold="True" Font-Italic="True"></asp:Label>
                                            </td>
                                            <td>
                                                NetTotal
                                            </td>
                                            <td>
                                                <asp:Label ID="lblNetTotal" runat="server" ForeColor="Blue" Font-Bold="True" Font-Italic="True"></asp:Label>
                                            </td>
                                            <td>
                                                TaxTotal
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTaxTotal" runat="server" ForeColor="Blue" Font-Bold="True" Font-Italic="True"></asp:Label>
                                            </td>
                                            <td>
                                                FeeTotal&nbsp
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFeeTotal" runat="server" ForeColor="Blue" Font-Bold="True" Font-Italic="True"></asp:Label>
                                            </td>
                                            <td>
                                                GrossTotal&
                                            </td>
                                            <td>
                                                <asp:Label ID="lblGrossTotal" runat="server" Font-Bold="True" Font-Italic="True"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3" style="text-align: left">
                                    &nbsp;<asp:RadioButtonList ID="rblPaymentTerms" runat="server" RepeatDirection="Horizontal"
                                        Width="216px">
                                        <asp:ListItem Value="Inv">Invoice</asp:ListItem>
                                        <asp:ListItem Value="PayNow">Pay Now</asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:Button ID="btnSaveQuote" runat="server" Text="Save Quote" />
                                    <asp:Button ID="btnMakeLive" runat="server" Text="Make Live" /></td>
                            </tr>
                        </table>
                        <asp:Label ID="lblPolicyNum" runat="server" Visible="False"></asp:Label>
                    </td>
                </tr>
                 <tr>
                <td>
                    <uc2:Footer ID="Footer1" runat="server" />
                
                </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
