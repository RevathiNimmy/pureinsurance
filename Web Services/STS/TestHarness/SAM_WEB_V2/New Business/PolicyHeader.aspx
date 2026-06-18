<%@ Page Language="VB" AutoEventWireup="false" MaintainScrollPositionOnPostback="true"
    CodeFile="PolicyHeader.aspx.vb" Title="PolicyHeader" Inherits="MTA_PolicyHeader" %>

<%@ Register Src="../UserControl/VerticalMenu.ascx" TagName="VerticalMenu" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/Footer.ascx" TagName="Footer" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Policy Header</title>

    <script language="javascript" type="text/javascript">
    
    function LoadWindows(url)
    {
   
     window.open(url,"","width=600,height=700,scrollbars=1");
    }
     function Showmodalpopup()
    {
        
         var ReturnChildValue = window.showModalDialog("../CurrencyExchange/CurrencyConversion.aspx", window, "dialogWidth:600px;dialogHeight:400px");
         if (ReturnChildValue != null)
         {
         
          document.getElementById("Cancel").value = ReturnChildValue;
          
         }
    }
    
    </script>

    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
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
                        <asp:Label ID="lblSamErrorMessage" runat="server" ForeColor="Red"></asp:Label>&nbsp;
                        <table border="0">
                            <tr>
                                <td>
                                    <asp:Menu ID="Menu1" runat="server">
                                        <%-- BackColor="#F7F6F3" DynamicHorizontalOffset="2"
                                        Font-Names="Verdana" Font-Size="0.8em" ForeColor="#7C6F57" Orientation="Horizontal"
                                        StaticSubMenuIndent="10px" Width="100%" 
                                        <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                                        <DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
                                        <DynamicMenuStyle BackColor="#F7F6F3" />
                                        <StaticSelectedStyle BackColor="#5D7B9D" />
                                        <DynamicSelectedStyle BackColor="#5D7B9D" />
                                        <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />--%>
                                        <Items>
                                            <asp:MenuItem Text="Main Details" Value="0"></asp:MenuItem>
                                            <asp:MenuItem Text="Addition Details" Value="1"></asp:MenuItem>
                                            <asp:MenuItem Text="Sub-Agent Details" Value="2"></asp:MenuItem>
                                        </Items>
                                        <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                                    </asp:Menu>
                                </td>
                            </tr>
                            <tr>
                                <td class="body">
                                    <asp:MultiView ID="mvPolicyHeaders" runat="server" ActiveViewIndex="0">
                                        <asp:View ID="View1" runat="server">
                                            <table class="body" border="0">
                                                <tr>
                                                    <td>
                                                        <strong>Insured Name</strong></td>
                                                    <td>
                                                        <asp:TextBox ID="txtInsuredName" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtInsuredName"
                                                            ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator></td>
                                                    <td>
                                                        <asp:Label ID="Label4" runat="server" Width="40px"></asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="Label2" runat="server" Text="Alternate Reference"></asp:Label></td>
                                                    <td>
                                                        <asp:TextBox ID="txtAlternateRef" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 26px">
                                                        <strong>Policy No</strong></td>
                                                    <td style="height: 26px">
                                                        <asp:TextBox ID="txtPolicyNo" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td style="height: 26px">
                                                    </td>
                                                    <td style="height: 26px">
                                                        Status</td>
                                                    <td style="height: 26px">
                                                        <asp:TextBox ID="txtStatus" runat="server" Enabled="False">Current</asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Product</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlProduct" runat="server">
                                                        </asp:DropDownList></td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        Policy Status</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlPolictStatus" runat="server">
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Branch Code</strong></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlBranchCode" runat="server" AutoPostBack="True">
                                                        </asp:DropDownList></td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        Analysis Code</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlAnalysisCode" runat="server">
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Sub Branch</strong></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlSubBranch" runat="server">
                                                        </asp:DropDownList></td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <strong>Business Type</strong></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlBusinessType" runat="server" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddlBusinessType"
                                                            ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator></td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 64px">
                                                        <asp:Button ID="btnAgentCode" runat="server" Text="Agent Code" OnClientClick="LoadWindows('FindAgent.aspx')"
                                                            CausesValidation="False" /></td>
                                                    <td style="height: 64px">
                                                        <asp:TextBox ID="txtShortName" runat="server" AutoPostBack="True"></asp:TextBox>
                                                        <asp:Label ID="lblAgentKey" runat="server" BackColor="White" Font-Bold="True" Font-Size="X-Large"
                                                            ForeColor="Red" Height="1px" Text="*" Visible="False" Width="17px" EnableTheming="False"></asp:Label>
                                                        <asp:HiddenField ID="hfPartyKey" runat="server" />
                                                    </td>
                                                    <td style="height: 64px">
                                                    </td>
                                                    <td style="height: 64px">
                                                        <strong>Currency</strong></td>
                                                    <td style="height: 64px">
                                                        <asp:DropDownList ID="ddlCurrency" runat="server" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlCurrency"
                                                            ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnHandler" runat="server" OnClientClick="LoadWindows('FindHandler.aspx')"
                                                            Text="Handler" Width="96px" CausesValidation="False" /></td>
                                                    <td>
                                                        <asp:TextBox ID="txtHandler" runat="server"></asp:TextBox>
                                                        <asp:HiddenField ID="hfHandlerCode" runat="server" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkQuote" runat="server" Text="Quote" Checked="True" Enabled="false" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Regarding</td>
                                                    <td>
                                                        <asp:TextBox ID="txtRegarding" runat="server"></asp:TextBox></td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Cover From</strong></td>
                                                    <td>
                                                        <asp:TextBox ID="txtcoveredFrom" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtcoveredFrom"
                                                            ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator></td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <strong>Cover To</strong></td>
                                                    <td>
                                                        <asp:TextBox ID="txtcoveredTo" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtcoveredTo"
                                                            ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Inception</strong></td>
                                                    <td>
                                                        <asp:TextBox ID="txtinceptionDate" runat="server" Enabled="False"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtinceptionDate"
                                                            ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator></td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <strong>Renewal</strong></td>
                                                    <td>
                                                        <asp:TextBox ID="txtRenewalDate" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtRenewalDate"
                                                            ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>Inception TPI</strong></td>
                                                    <td>
                                                        <asp:TextBox ID="txtInceptionTPI" runat="server" Enabled="False"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtInceptionTPI"
                                                            ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator></td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        Issued</td>
                                                    <td>
                                                        <asp:TextBox ID="txtIssuedDate" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        H/C Expiry</td>
                                                    <td>
                                                        <asp:TextBox ID="txtHCExpiry" runat="server"></asp:TextBox></td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <strong>Quote Expiry</strong></td>
                                                    <td>
                                                        <asp:TextBox ID="txtQuoteexpiry" runat="server" Enabled="False"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtQuoteexpiry"
                                                            ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Policy Deductable</td>
                                                    <td>
                                                        <asp:DropDownList ID="DropDownList1" runat="server">
                                                            <asp:ListItem>(None)</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        Policy Limits</td>
                                                    <td>
                                                        <asp:DropDownList ID="DropDownList2" runat="server">
                                                            <asp:ListItem>(None)</asp:ListItem>
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Policy Type</td>
                                                    <td>
                                                        <asp:Label ID="lblPolicyStatus" runat="server" Width="192px"></asp:Label></td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        Scheme</td>
                                                    <td>
                                                        <asp:Label ID="lblScheme" runat="server"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">
                                                        <asp:Button ID="btnLapseQuote" runat="server" Text="Lapse Quote" CausesValidation="False" />
                                                        <asp:Button ID="btnPolicyTax" runat="server" Text="PolicyTax" CausesValidation="False" />
                                                        <asp:Button ID="btnPolicyFee" runat="server" Text="Policy Fee" CausesValidation="False" />
                                                        <asp:Button ID="btnCommission" runat="server" Text="Commission" CausesValidation="False" /></td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="View2" runat="server">
                                            <table border="0">
                                                <tr>
                                                    <td>
                                                        <strong>Frequency</strong> :</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlFrequencyCode" runat="server">
                                                        </asp:DropDownList></td>
                                                    <td>
                                                        Renewal Method :</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlRenewalmethodCode" runat="server">
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        LTU expiry :&nbsp;</td>
                                                    <td>
                                                        <asp:TextBox ID="txtLTUExpiry" runat="server"></asp:TextBox></td>
                                                    <td>
                                                        Lapse/Cancellation Reason :</td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="ddlLapseCancellation" runat="server">
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Stop Reason :</td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlStopReasoCode" runat="server" Width="200px">
                                                        </asp:DropDownList></td>
                                                    <td>
                                                        Lapse/Cancellation Date :</td>
                                                    <td>
                                                        <asp:TextBox ID="txtLapseCancelDate" runat="server" Width="200px"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Times renewed :</td>
                                                    <td>
                                                        <asp:Label ID="lblTimesRenewed" runat="server"></asp:Label></td>
                                                    <td>
                                                        <asp:CheckBox ID="CkBxReferrredatrenewal" runat="server" Text="Referrred at renewal ?" /></td>
                                                    <td>
                                                        &nbsp;<asp:CheckBox ID="CkBxReferrredonMTA" runat="server" Text="Referred on MTA ?" /></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        Standard Wording<br />
                                                        <asp:GridView ID="gvStandardWording" runat="server" CellPadding="4" ForeColor="#333333"
                                                            GridLines="None" Width="464px">
                                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                            <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                                                            <EditRowStyle BackColor="#999999" />
                                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:CommandField ShowDeleteButton="True" />
                                                            </Columns>
                                                        </asp:GridView>
                                                        <asp:DropDownList ID="ddlPolicyStyle" runat="server">
                                                        </asp:DropDownList>
                                                        <asp:Button ID="btnAddPolicywording" runat="server" Text="Add" OnClientClick="LoadWindows('SelectClauses.aspx')" />
                                                        <br />
                                                        <br />
                                                        <br />
                                                        <asp:Button ID="btnCoinsurers" runat="server" Text="Coinsurers" CausesValidation="False"
                                                            Enabled="false" /></td>
                                                </tr>
                                                <tr style="height: 200px">
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="View3" runat="server">
                                            <asp:Label ID="Label5" runat="server" Font-Names="Arial" Font-Size="8pt" Text="Sub Agents"
                                                Width="176px"></asp:Label>
                                            <asp:GridView ID="gvSubAgents" runat="server" CellPadding="4" ForeColor="#333333"
                                                GridLines="None" Width="520px">
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <Columns>
                                                    <asp:CommandField ShowDeleteButton="True" />
                                                </Columns>
                                                <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                                                <EditRowStyle BackColor="#999999" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            </asp:GridView>
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnAdd" runat="server" Text="Add" CausesValidation="False" /><br />
                                            <asp:Panel ID="Panel1" runat="server" Height="350px" Width="784px">
                                                Cover Note Book
                                                <asp:TextBox ID="txtCoverBook" runat="server"></asp:TextBox>
                                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Cover Note
                                                Sheet
                                                <asp:TextBox ID="txtCoverSheet" runat="server"></asp:TextBox></asp:Panel>
                                        </asp:View>
                                    </asp:MultiView></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="cmdOk" runat="server" Text="Ok" Width="96px" />
                                </td>
                            </tr>
                        </table>
                        <uc3:Footer ID="Footer1" runat="server" />
                    </td>
                </tr>
            </table>
            <input type="hidden" id="Cancel" runat="server" />
        </div>
        <%-- <div>
            <asp:Label ID="lblSamErrorMessage" runat="server" ForeColor="Red"></asp:Label>&nbsp;
        </div>
        <table border="1">
            <tr>
                <td>
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
                            <asp:MenuItem Text="Main Details" Value="0"></asp:MenuItem>
                            <asp:MenuItem Text="Addition Details" Value="1"></asp:MenuItem>
                            <asp:MenuItem Text="Sub-Agent Details" Value="2"></asp:MenuItem>
                        </Items>
                        <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                    </asp:Menu>
                </td>
            </tr>
            <tr>
                <td class="body">
                    <asp:MultiView ID="mvPolicyHeaders" runat="server" ActiveViewIndex="0">
                        <asp:View ID="View1" runat="server">
                            <table class="body">
                                <tr>
                                    <td>
                                        Insured Name</td>
                                    <td>
                                        <asp:TextBox ID="txtInsuredName" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Width="40px"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Alternate Reference" Width="112px"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtAlternateRef" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        Policy No</td>
                                    <td>
                                        <asp:TextBox ID="txtPolicyNo" runat="server"></asp:TextBox></td>
                                    <td>
                                    </td>
                                    <td>
                                        Status</td>
                                    <td>
                                        <asp:TextBox ID="txtStatus" runat="server" Enabled="False">Current</asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        Product</td>
                                    <td>
                                        <asp:DropDownList ID="ddlProduct" runat="server">
                                        </asp:DropDownList></td>
                                    <td>
                                    </td>
                                    <td>
                                        Policy Status</td>
                                    <td>
                                        <asp:DropDownList ID="ddlPolictStatus" runat="server">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>
                                        Branch Code</td>
                                    <td>
                                        <asp:DropDownList ID="ddlBranchCode" runat="server" AutoPostBack="True">
                                        </asp:DropDownList></td>
                                    <td>
                                    </td>
                                    <td>
                                        Analysis Code</td>
                                    <td>
                                        <asp:DropDownList ID="ddlAnalysisCode" runat="server">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>
                                        Sub Branch</td>
                                    <td>
                                        <asp:DropDownList ID="ddlSubBranch" runat="server">
                                        </asp:DropDownList></td>
                                    <td>
                                    </td>
                                    <td>
                                        Business Type</td>
                                    <td>
                                        <asp:DropDownList ID="ddlBusinessType" runat="server" AutoPostBack="True">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnAgentCode" runat="server" Text="Agent Code" OnClientClick="LoadWindows('FindAgent.aspx')" /></td>
                                    <td>
                                        <asp:TextBox ID="txtShortName" runat="server"></asp:TextBox>
                                        <asp:HiddenField ID="hfPartyKey" runat="server" />
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        Currency</td>
                                    <td>
                                        <asp:DropDownList ID="ddlCurrency" runat="server">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnHandler" runat="server" OnClientClick="LoadWindows('FindHandler.aspx')"
                                            Text="Handler" Width="96px" /></td>
                                    <td>
                                        <asp:TextBox ID="txtHandler" runat="server"></asp:TextBox>
                                        <asp:HiddenField ID="hfHandlerCode" runat="server" />
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkQuote" runat="server" Checked="True" Style="z-index: 100; left: 712px;
                                            position: absolute; top: 300px" Text="Quote" Enabled="false" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Regarding</td>
                                    <td>
                                        <asp:TextBox ID="txtRegarding" runat="server"></asp:TextBox></td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Cover From</td>
                                    <td>
                                        <asp:TextBox ID="txtcoveredFrom" runat="server"></asp:TextBox></td>
                                    <td>
                                    </td>
                                    <td>
                                        Cover To</td>
                                    <td>
                                        <asp:TextBox ID="txtcoveredTo" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        Inception</td>
                                    <td>
                                        <asp:TextBox ID="txtinceptionDate" runat="server" Enabled="False"></asp:TextBox></td>
                                    <td>
                                    </td>
                                    <td>
                                        Renewal</td>
                                    <td>
                                        <asp:TextBox ID="txtRenewalDate" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        Inception TPI</td>
                                    <td>
                                        <asp:TextBox ID="txtInceptionTPI" runat="server" Enabled="False"></asp:TextBox></td>
                                    <td>
                                    </td>
                                    <td>
                                        Issued</td>
                                    <td>
                                        <asp:TextBox ID="txtIssuedDate" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        H/C Expiry</td>
                                    <td>
                                        <asp:TextBox ID="txtHCExpiry" runat="server"></asp:TextBox></td>
                                    <td>
                                    </td>
                                    <td>
                                        Quote Expiry</td>
                                    <td>
                                        <asp:TextBox ID="txtQuoteexpiry" runat="server" Enabled="False"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        Policy Deductable</td>
                                    <td>
                                        <asp:DropDownList ID="DropDownList1" runat="server">
                                            <asp:ListItem>(None)</asp:ListItem>
                                        </asp:DropDownList></td>
                                    <td>
                                    </td>
                                    <td>
                                        Policy Limits</td>
                                    <td>
                                        <asp:DropDownList ID="DropDownList2" runat="server">
                                            <asp:ListItem>(None)</asp:ListItem>
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>
                                        Policy Type</td>
                                    <td>
                                        <asp:Label ID="lblPolicyStatus" runat="server" Text="Label" Width="192px"></asp:Label></td>
                                    <td>
                                    </td>
                                    <td>
                                        Scheme</td>
                                    <td>
                                        <asp:Label ID="lblScheme" runat="server" Text="Label"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:Button ID="btnLapseQuote" runat="server" Text="Lapse Quote" />
                                        <asp:Button ID="btnPolicyTax" runat="server" Text="PolicyTax" />
                                        <asp:Button ID="btnPolicyFee" runat="server" Text="Policy Fee" />
                                        <asp:Button ID="btnCommission" runat="server" Text="Commission" /></td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="View2" runat="server">
                            <table border ="1">
                                <tr>
                                    <td>
                                        Frequency :</td>
                                    <td>
                                        <asp:DropDownList ID="ddlFrequencyCode" runat="server" >
                                        </asp:DropDownList></td>
                                    <td>
                                        Renewal Method :</td>
                                    <td>
                                        <asp:DropDownList ID="ddlRenewalmethodCode" runat="server">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>
                                        LTU expiry :&nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="txtLTUExpiry" runat="server"></asp:TextBox></td>
                                    <td>
                                        Lapse/Cancellation Reason :</td>
                                    <td align="left">
                                        &nbsp;<asp:DropDownList ID="ddlLapseCancellation" runat="server">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>
                                        Stop Reason :</td>
                                    <td>
                                        <asp:DropDownList ID="ddlStopReasoCode" runat="server" Width="200px">
                                        </asp:DropDownList></td>
                                    <td>
                                        Lapse/Cancellation Date :</td>
                                    <td>
                                        <asp:TextBox ID="txtLapseCancelDate" runat="server" Width="200px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        Times renewed :</td>
                                    <td>
                                        <asp:Label ID="lblTimesRenewed" runat="server" Text="Label"></asp:Label></td>
                                    <td>
                                        <asp:CheckBox ID="CkBxReferrredatrenewal" runat="server" Text="Referrred at renewal ?" /></td>
                                    <td>
                                        &nbsp;<asp:CheckBox ID="CkBxReferrredonMTA" runat="server" Text="Referred on MTA ?" /></td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        Standard Wording<br />
                                        <asp:GridView ID="gvStandardWording" runat="server" CellPadding="4" ForeColor="#333333"
                                            GridLines="None" Width="464px">
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                                            <EditRowStyle BackColor="#999999" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        </asp:GridView>
                                        <asp:DropDownList ID="ddlPolicyStyle" runat="server">
                                        </asp:DropDownList>
                                        <asp:Button ID="btnAddPolicywording" runat="server" Text="Add" OnClientClick="LoadWindows('FindDocumentTemplates.aspx')" />
                                        <br />
                                        <br />
                                        <br />
                                        <asp:Button ID="btnCoinsurers" runat="server" Text="Coinsurers" /></td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="View3" runat="server">
                            <asp:Label ID="Label5" runat="server" Font-Names="Arial" Font-Size="8pt" Text="Sub Agents"
                                Width="176px"></asp:Label>
                            <asp:GridView ID="gvSubAgents" runat="server" CellPadding="4" ForeColor="#333333"
                                GridLines="None" Width="520px">
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <Columns>
                                    <asp:CommandField ShowDeleteButton="True" />
                                </Columns>
                                <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                                <EditRowStyle BackColor="#999999" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                            &nbsp;&nbsp;
                            <asp:Button ID="btnAdd" runat="server" Text="Add" /><br />
                            <asp:Panel ID="Panel1" runat="server" Font-Names="Arial" Font-Size="8pt" Height="40px"
                                Width="784px">
                                Cover Note Book &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                <asp:TextBox ID="txtCoverBook" runat="server" Width="200px"></asp:TextBox>
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Cover Note
                                Sheet &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                <asp:TextBox ID="txtCoverSheet" runat="server" Width="200px"></asp:TextBox></asp:Panel>
                        </asp:View>
                    </asp:MultiView></td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="cmdOk" runat="server" Text="Ok" Width="96px" />
                </td>
            </tr>
        </table>
        --%>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="False" />
    </form>
</body>
</html>
