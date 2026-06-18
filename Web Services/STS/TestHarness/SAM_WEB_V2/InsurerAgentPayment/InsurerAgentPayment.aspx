<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InsureragentPayment.aspx.vb"
    Inherits="Insurer_Payment_InsureragentPayment" %>

<%@ Register Src="../UserControl/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>InsurerAgentPayment</title>

    <script language="javascript" type="text/javascript">
    
    function LoadWindows(url)
    {
  
    window.open(url,"","width=600,height=700,scrollbars=1");
       }
    
    </script>


    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="100%" style="400px" border="0" cellspacing="0" cellpadding="0">
                <tr style="height: 10%">
                    <td style="width: 10%" align="right">
                        <table width="100%">
                            <tr>
                                <td>
                                    <uc1:Header ID="Header1" runat="server" />
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
               
                    </td>
                </tr>
                <tr style="height: 90%">
                    <td style="height: 90%">
                        <table width="100%">
                <tr>
                    <td colspan="2">
                        <strong>Insurer/AgentPayment</strong></td>
                    <td style="width: 100px">
                    </td>
                    <td style="width: 80px">
                    </td>
                    <td colspan="3" rowspan="2">
                        </td>
                    <td style="width: 100px">
                    </td>
                    <td style="width: 100px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px; height: 41px">
                        AccountCode</td>
                    <td style="width: 138px; height: 41px">
                        <asp:TextBox ID="txtShortName" runat="server"></asp:TextBox>
                        <asp:HiddenField ID="hfAccountKey" runat="server" />
                    </td>
                    <td style="width: 100px; height: 41px">
                        <asp:Button ID="btnFindaccountcode" runat="server" Text="Find" OnClientClick="LoadWindows('FindAccount.aspx')" /></td>
                    <td style="width: 80px; height: 41px">
                        <asp:Button ID="btnFindNow" runat="server" Text="Find Now" Width="105px" />
                    </td>
                    <td style="width: 100px; height: 41px">
                    </td>
                    <td style="width: 100px; height: 41px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px; height: 22px;">
                        Date To</td>
                    <td style="width: 138px; height: 22px;">
                        <asp:TextBox ID="txtDateTo" runat="server"></asp:TextBox>
                        <asp:CheckBox ID="chkDateTo" runat="server" Text="DateTo" AutoPostBack="True" />
                        <asp:Calendar ID="Calendar1" runat="server" Visible="False"></asp:Calendar>
                    </td>
                    <td style="width: 100px; height: 22px;">
                        &nbsp;<asp:RadioButtonList ID="rbtDAte" runat="server" Width="165px">
                            <asp:ListItem Selected="True">EffectiveDate</asp:ListItem>
                            <asp:ListItem Value="radosAccountcurrency">TransactionDate</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td style="width: 80px; height: 22px;">
                    <asp:Button ID="btnNewsearch" runat="server" Text="New Search" />
                        &nbsp;
                    </td>
                    <td style="height: 22px;" colspan="3">
                        &nbsp;&nbsp;
                        <asp:Button ID="btnMark" runat="server" Text="MarkAll"  />
                    </td>
                    <td style="width: 100px; height: 22px;">
                    </td>
                    <td style="width: 100px; height: 22px;">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px; height: 24px;">
                        Payment Group</td>
                    <td style="width: 138px; height: 24px;">
                        <asp:DropDownList ID="ddlPaymentGroup" runat="server" Width="146px">
                        </asp:DropDownList></td>
                    <td style="width: 100px; height: 24px;">
                        Marked Status</td>
                    <td style="width: 80px; height: 24px;">
                        <asp:DropDownList ID="ddlMarkedStatus" runat="server">
                            <asp:ListItem Value="0">No</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="2">Any</asp:ListItem>
                        </asp:DropDownList></td>
                    <td style="width: 100px; height: 24px;">
                    </td>
                    <td style="width: 100px; height: 24px;">
                    </td>
                    <td style="width: 63px; height: 24px;">
                    </td>
                    <td style="width: 100px; height: 24px;">
                    </td>
                    <td style="width: 100px; height: 24px;">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px; height: 26px">
                        Alternate Ref</td>
                    <td style="width: 138px; height: 26px">
                        <asp:TextBox ID="txtAlternateRef" runat="server"></asp:TextBox></td>
                    <td style="width: 100px; height: 26px">
                        View By</td>
                    <td colspan="2" style="height: 26px">
                    </td>
                    <td style="width: 100px; height: 26px">
                    </td>
                    <td style="width: 63px; height: 26px">
                    </td>
                    <td style="width: 100px; height: 26px">
                    </td>
                    <td style="width: 100px; height: 26px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px; height: 21px">
                        Total Marked</td>
                    <td style="width: 138px; height: 21px">
                        &nbsp;
                        <asp:TextBox ID="txtTotalMarked" runat="server"></asp:TextBox></td>
                    <td style="width: 100px; height: 21px">
                        <asp:RadioButtonList ID="rbtCurrecy" runat="server" Width="165px">
                            <asp:ListItem Selected="True" Value="radTransactionCur">Transaction Currency</asp:ListItem>
                            <asp:ListItem Value="radosAccountcurrency">Accountcurrency (Pounds sterling)</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td style="width: 80px; height: 21px">
                    </td>
                    <td colspan="3" rowspan="2">
                    </td>
                    <td style="width: 100px; height: 21px">
                    </td>
                    <td style="width: 100px; height: 21px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px">
                        Total WriteOff</td>
                    <td style="width: 138px">
                        <asp:TextBox ID="txtTotalwriteoff" runat="server"></asp:TextBox>
                    </td>
                    <td style="width: 100px">
                        Month</td>
                    <td style="width: 80px">
                        <asp:DropDownList ID="ddlMonth" runat="server">
                            <asp:ListItem Value="0">All</asp:ListItem>
                            <asp:ListItem Value="1">January</asp:ListItem>
                            <asp:ListItem Value="2">Februray</asp:ListItem>
                            <asp:ListItem Value="3">March</asp:ListItem>
                            <asp:ListItem Value="4">April</asp:ListItem>
                            <asp:ListItem Value="5">May</asp:ListItem>
                            <asp:ListItem Value="6">June</asp:ListItem>
                            <asp:ListItem Value="7">July</asp:ListItem>
                            <asp:ListItem Value="8">August</asp:ListItem>
                            <asp:ListItem Value="9">September</asp:ListItem>
                            <asp:ListItem Value="10">October</asp:ListItem>
                            <asp:ListItem Value="11">November</asp:ListItem>
                            <asp:ListItem Value="12">December</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 100px">
                    </td>
                    <td style="width: 100px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px">
                    </td>
                    <td style="width: 138px">
                    </td>
                    <td style="width: 100px">
                    </td>
                    <td style="width: 80px">
                    </td>
                    <td style="width: 100px">
                    </td>
                    <td style="width: 100px">
                    </td>
                    <td style="width: 63px">
                    </td>
                    <td style="width: 100px">
                    </td>
                    <td style="width: 100px">
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 21px">
                        <strong>Transactions</strong></td>
                    <td style="width: 100px; height: 21px;">
                    </td>
                    <td style="width: 80px; height: 21px;">
                    </td>
                    <td style="width: 100px; height: 21px;">
                    </td>
                    <td style="width: 100px; height: 21px;">
                    </td>
                    <td style="width: 63px; height: 21px;">
                    </td>
                    <td style="width: 100px; height: 21px;">
                    </td>
                    <td style="width: 100px; height: 21px;">
                    </td>
                </tr>
                <tr>
                    <td colspan="7" rowspan="2">
                        <asp:Panel ID="pnlTrans" runat="server" ScrollBars="Auto" Width="700px" Height="400px">
                            <asp:GridView ID="gvTransactions" runat="server" CellPadding="4" ForeColor="#333333"
                                GridLines="None" Width="528px" AutoGenerateColumns="false">
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                                <EditRowStyle BackColor="#999999" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox AutoPostBack="true" OnCheckedChanged="Chk_CheckedChanged" runat="server"
                                                ID="chk" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ResolvedName" HeaderText="ResolvedName" />
                                    <asp:BoundField DataField="InsurerRef" HeaderText="InsurerRef" />
                                    <asp:BoundField DataField="DocumentRef" HeaderText="DocumentRef" />
                                    <asp:BoundField DataField="AlternateReference" HeaderText="AlternateReference" />
                                    <asp:BoundField DataField="EffectiveDate" HeaderText="EffectiveDate" />
                                    <asp:BoundField DataField="AccountingDate" HeaderText="AccountingDate" />
                                    <asp:BoundField DataField="CurrencyAmount" HeaderText="CurrencyAmount" />
                                    <asp:BoundField DataField="PaidAmount" HeaderText="PaidAmount" />
                                    <asp:BoundField DataField="ClientOutstanding" HeaderText="ClientOutstanding" />
                                    <asp:BoundField DataField="ShortName" HeaderText="ShortName" />
                                    <asp:BoundField DataField="FullyPaidAmount" HeaderText="FullyPaidAmount" />
                                    <asp:BoundField DataField="TransdetailId" HeaderText="TransdetailId" />
                                    <asp:BoundField DataField="MarkedAmount" HeaderText="MarkedAmount" />
                                    <asp:BoundField DataField="DocumentId" HeaderText="DocumentId" />
                                    <asp:BoundField DataField="ConsolidateBinder" HeaderText="ConsolidateBinder" />
                                    <asp:BoundField DataField="CompanyId" HeaderText="CompanyId" />
                                    <asp:BoundField DataField="CurrencyId" HeaderText="CurrencyId" />
                                    <asp:BoundField DataField="CurrencyCode" HeaderText="CurrencyCode" />
                                    <asp:BoundField DataField="CurrencyBaseRate" HeaderText="CurrencyBaseRate" />
                                    <asp:BoundField DataField="Spare" HeaderText="Spare" />
                                    <asp:BoundField DataField="PeriodName" HeaderText="PeriodName" />
                                    <asp:BoundField DataField="Month" HeaderText="Month" />
                                    <asp:BoundField DataField="AccountCurrencyId" HeaderText="AccountCurrencyId" />
                                    <asp:BoundField DataField="AccountCurrencyCode" HeaderText="AccountCurrencyCode" />
                                    <asp:BoundField DataField="AccountBaseRate" HeaderText="AccountBaseRate" />
                                    <asp:BoundField DataField="FullyPaidAccountAmount" HeaderText="FullyPaidAccountAmount" />
                                    <asp:BoundField DataField="ClientOutstandingAccountAmount" HeaderText="ClientOutstandingAccountAmount" />
                                    <asp:BoundField DataField="AccountAmount" HeaderText="AccountAmount" />
                                    <asp:BoundField DataField="MarkedAccountAmount" HeaderText="MarkedAccountAmount" />
                                    <asp:BoundField DataField="PaidAccountAmount" HeaderText="PaidAccountAmount" />
                                    <asp:BoundField DataField="BranchCode" HeaderText="BranchCode" />
                                </Columns>
                            </asp:GridView>
                            <asp:GridView ID="gvTest" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                                Width="528px" AutoGenerateColumns="false" Visible="false">
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                                <EditRowStyle BackColor="#999999" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chk" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ResolvedName" HeaderText="ResolvedName" />
                                    <asp:BoundField DataField="InsurerRef" HeaderText="InsurerRef" />
                                    <asp:BoundField DataField="DocumentRef" HeaderText="DocumentRef" />
                                    <asp:BoundField DataField="AlternateReference" HeaderText="AlternateReference" />
                                    <asp:BoundField DataField="EffectiveDate" HeaderText="EffectiveDate" />
                                    <asp:BoundField DataField="AccountingDate" HeaderText="AccountingDate" />
                                    <asp:BoundField DataField="CurrencyAmount" HeaderText="CurrencyAmount" />
                                    <asp:BoundField DataField="PaidAmount" HeaderText="PaidAmount" />
                                    <asp:BoundField DataField="ClientOutstanding" HeaderText="ClientOutstanding" />
                                    <asp:BoundField DataField="ShortName" HeaderText="ShortName" />
                                    <asp:BoundField DataField="FullyPaidAmount" HeaderText="FullyPaidAmount" />
                                    <asp:BoundField DataField="TransdetailId" HeaderText="TransdetailId" />
                                    <asp:BoundField DataField="MarkedAmount" HeaderText="MarkedAmount" />
                                    <asp:BoundField DataField="DocumentId" HeaderText="DocumentId" />
                                    <asp:BoundField DataField="ConsolidateBinder" HeaderText="ConsolidateBinder" />
                                    <asp:BoundField DataField="CompanyId" HeaderText="CompanyId" />
                                    <asp:BoundField DataField="CurrencyId" HeaderText="CurrencyId" />
                                    <asp:BoundField DataField="CurrencyCode" HeaderText="CurrencyCode" />
                                    <asp:BoundField DataField="CurrencyBaseRate" HeaderText="CurrencyBaseRate" />
                                    <asp:BoundField DataField="Spare" HeaderText="Spare" />
                                    <asp:BoundField DataField="PeriodName" HeaderText="PeriodName" />
                                    <asp:BoundField DataField="Month" HeaderText="Month" />
                                    <asp:BoundField DataField="AccountCurrencyId" HeaderText="AccountCurrencyId" />
                                    <asp:BoundField DataField="AccountCurrencyCode" HeaderText="AccountCurrencyCode" />
                                    <asp:BoundField DataField="AccountBaseRate" HeaderText="AccountBaseRate" />
                                    <asp:BoundField DataField="FullyPaidAccountAmount" HeaderText="FullyPaidAccountAmount" />
                                    <asp:BoundField DataField="ClientOutstandingAccountAmount" HeaderText="ClientOutstandingAccountAmount" />
                                    <asp:BoundField DataField="AccountAmount" HeaderText="AccountAmount" />
                                    <asp:BoundField DataField="MarkedAccountAmount" HeaderText="MarkedAccountAmount" />
                                    <asp:BoundField DataField="PaidAccountAmount" HeaderText="PaidAccountAmount" />
                                    <asp:BoundField DataField="BranchCode" HeaderText="BranchCode" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                    <td style="width: 100px">
                        <asp:Button ID="btnDrill" runat="server" Text="Drill" OnClientClick="LoadWindows('FindAccountdetails.aspx')" />
                    </td>
                    <td style="width: 100px">
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px; height: 78px;">
                    </td>
                    <td style="width: 100px; height: 78px;">
                    </td>
                </tr>
                <tr>
                    <td colspan="7" rowspan="1" style="height: 3px">
                        <strong>OutStanding Transaction</strong> <strong>Details</strong></td>
                    <td style="width: 100px; height: 3px">
                    </td>
                    <td style="width: 100px; height: 3px">
                    </td>
                </tr>
                <tr>
                    <td colspan="7" rowspan="3" style="height: 219px">
                        <asp:Panel runat="server" ID="pnlOutTrans" ScrollBars="Auto" Height="200px" Width="700px">
                            <asp:GridView ID="gvOutstandingTrans" runat="server" CellPadding="4" ForeColor="#333333"
                                GridLines="None" Width="528px" AutoGenerateColumns="false">
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                                <EditRowStyle BackColor="#999999" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkeachout" OnCheckedChanged="chkeach_CheckedChanged"
                                                AutoPostBack="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ResolvedName" HeaderText="ResolvedName" />
                                    <asp:BoundField DataField="InsurerRef" HeaderText="InsurerRef" />
                                    <asp:BoundField DataField="DocumentRef" HeaderText="DocumentRef" />
                                    <asp:BoundField DataField="AlternateReference" HeaderText="AlternateReference" />
                                    <asp:BoundField DataField="EffectiveDate" HeaderText="EffectiveDate" />
                                    <asp:BoundField DataField="AccountingDate" HeaderText="AccountingDate" />
                                    <asp:BoundField DataField="CurrencyAmount" HeaderText="CurrencyAmount" />
                                    <asp:BoundField DataField="PaidAmount" HeaderText="PaidAmount" />
                                    <asp:BoundField DataField="ClientOutstanding" HeaderText="ClientOutstanding" />
                                    <asp:BoundField DataField="ShortName" HeaderText="ShortName" />
                                    <asp:BoundField DataField="FullyPaidAmount" HeaderText="FullyPaidAmount" />
                                    <asp:BoundField DataField="TransdetailId" HeaderText="TransdetailId" />
                                    <asp:BoundField DataField="MarkedAmount" HeaderText="MarkedAmount" />
                                    <asp:BoundField DataField="DocumentId" HeaderText="DocumentId" />
                                    <asp:BoundField DataField="ConsolidateBinder" HeaderText="ConsolidateBinder" />
                                    <asp:BoundField DataField="CompanyId" HeaderText="CompanyId" />
                                    <asp:BoundField DataField="CurrencyId" HeaderText="CurrencyId" />
                                    <asp:BoundField DataField="CurrencyCode" HeaderText="CurrencyCode" />
                                    <asp:BoundField DataField="CurrencyBaseRate" HeaderText="CurrencyBaseRate" />
                                    <asp:BoundField DataField="Spare" HeaderText="Spare" />
                                    <asp:BoundField DataField="PeriodName" HeaderText="PeriodName" />
                                    <asp:BoundField DataField="Month" HeaderText="Month" />
                                    <asp:BoundField DataField="AccountCurrencyId" HeaderText="AccountCurrencyId" />
                                    <asp:BoundField DataField="AccountCurrencyCode" HeaderText="AccountCurrencyCode" />
                                    <asp:BoundField DataField="AccountBaseRate" HeaderText="AccountBaseRate" />
                                    <asp:BoundField DataField="FullyPaidAccountAmount" HeaderText="FullyPaidAccountAmount" />
                                    <asp:BoundField DataField="ClientOutstandingAccountAmount" HeaderText="ClientOutstandingAccountAmount" />
                                    <asp:BoundField DataField="AccountAmount" HeaderText="AccountAmount" />
                                    <asp:BoundField DataField="MarkedAccountAmount" HeaderText="MarkedAccountAmount" />
                                    <asp:BoundField DataField="PaidAccountAmount" HeaderText="PaidAccountAmount" />
                                    <asp:BoundField DataField="BranchCode" HeaderText="BranchCode" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                        <asp:Button ID="btnPay" runat="server" Text="Pay" Width="83px" /><asp:Button ID="btnWRITEOFF"
                            runat="server" Text="WRITEOFF" /><asp:Button ID="btnPartPay" runat="server" Text="Partpay" />
                        <asp:Panel ID="pnlWritee" runat="server" Height="50px" Width="125px">
                            <table>
                                <tr>
                                    <td>
                                        <strong>Payment </strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 40px">
                                        Enter The Write Off Amount
                                    </td>
                                    <td style="height: 40px">
                                        <asp:Button ID="btnOK" runat="server" Text="OK" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtWriteoffAmount" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlPartPay" runat="server" Height="50px" Width="125px">
                            <table>
                                <tr>
                                    <td>
                                        <strong>PartPay </strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 40px">
                                        Enter The payment Amount
                                    </td>
                                    <td style="height: 40px">
                                        <asp:Button ID="btnpartpayOk" runat="server" Text="OK" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnpartpayCancel" runat="server" Text="Cancel" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtpartPay" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        </table> 
                        <uc2:Footer ID="Footer1" runat="server" />
                    </td>
                </tr>
            </table>
          
        </div>
    </form>
</body>
</html>
