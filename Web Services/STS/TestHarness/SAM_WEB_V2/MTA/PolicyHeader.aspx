<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PolicyHeader.aspx.vb" Title="PolicyHeader"
    Inherits="MTA_PolicyHeader" %>

<%@ Register Src="../UserControl/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>

    <script language="javascript" type="text/javascript">
    
    function LoadWindows(url)
    {
    window.open(url,"","width=600,height=650,scrollbars=1")
       }
    
    </script>


    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
        <table width="100%" style="height: 100%" border="0" cellspacing="0" cellpadding="0">
            <tr style="height: 10%">
                <td style="width: 10%" align="right">
                    <table width="100%">
                        <tr>
                            <td style="height: 120px">
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
                    <div>
                        &nbsp;</div>
                    <table>
                        <tr>
                            <td style="width: 1066px; height: 41px;">
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
                            <td>
                                <asp:MultiView ID="mvPolicyHeaders" runat="server" ActiveViewIndex="0">
                                    <asp:View ID="View1" runat="server">
                                        <table>
                                            <tr>
                                                <td style="width: 280px; height: 26px;">
                                                    <strong>Insured Name</strong></td>
                                                <td style="width: 395px; height: 26px;">
                                                    <asp:TextBox ID="txtInsuredName" runat="server" Width="200px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtInsuredName"
                                                        ErrorMessage="*" Font-Bold="true" Width="1px"></asp:RequiredFieldValidator></td>
                                                <td style="width: 75616px; height: 26px;">
                                                    <asp:Label ID="Label4" runat="server" Width="40px"></asp:Label></td>
                                                <td style="width: 98211px; height: 26px;">
                                                    <asp:Label ID="Label2" runat="server" Font-Names="Arial" Font-Size="8pt" Text="Alternate Reference"
                                                        Width="112px"></asp:Label></td>
                                                <td style="width: 106990px; height: 26px;">
                                                    <asp:TextBox ID="txtAlternateRef" runat="server" Width="200px"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 280px; height: 18px;">
                                                    <strong>Policy No</strong></td>
                                                <td style="width: 395px; height: 18px;">
                                                    <asp:TextBox ID="txtPolicyNo" runat="server" Width="200px"></asp:TextBox></td>
                                                <td style="width: 75616px; height: 18px">
                                                </td>
                                                <td >
                                                    Status</td>
                                                <td>
                                                     <asp:TextBox ID="txtStatus" runat="server" Enabled="False" Text="Current" >
                                                        </asp:TextBox></td> 
                                            </tr>
                                            <tr>
                                                <td style="width: 280px; font-size: 8pt; font-family: Arial;">
                                                    Product</td>
                                                <td style="width: 395px;">
                                                    <asp:DropDownList ID="ddlProduct" runat="server" Width="152px" Enabled="False">
                                                    </asp:DropDownList></td>
                                                <td style="width: 75616px;">
                                                </td>
                                                <td style="width: 98211px; font-size: 8pt; font-family: Arial;">
                                                    Policy Status</td>
                                                <td style="width: 106990px;">
                                                    <asp:DropDownList ID="ddlPolictStatus" runat="server" Width="200px">
                                                    </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: 8pt; width: 280px; font-family: Arial">
                                                    <strong>Branch Code</strong></td>
                                                <td style="width: 395px">
                                                    <asp:DropDownList ID="ddlBranchCode" runat="server" Width="200px" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlBranchCode"
                                                        ErrorMessage="*" Font-Bold="true"></asp:RequiredFieldValidator></td>
                                                <td style="width: 75616px">
                                                </td>
                                                <td style="font-size: 8pt; width: 98211px; font-family: Arial">
                                                    Analysis Code</td>
                                                <td style="width: 106990px">
                                                    <asp:DropDownList ID="ddlAnalysisCode" runat="server" Width="200px">
                                                    </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: 8pt; width: 280px; font-family: Arial; height: 30px;">
                                                    <strong>Sub Branch</strong></td>
                                                <td style="width: 395px; height: 30px;">
                                                    <asp:DropDownList ID="ddlSubBranch" runat="server" Width="200px">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddlSubBranch"
                                                        ErrorMessage="*" Font-Bold="true"></asp:RequiredFieldValidator></td>
                                                <td style="width: 75616px; height: 30px;">
                                                </td>
                                                <td style="font-size: 8pt; width: 98211px; font-family: Arial; height: 30px;">
                                                    <strong>Business Type</strong></td>
                                                <td style="width: 106990px; height: 30px;">
                                                    <asp:DropDownList ID="ddlBusinessType" runat="server" Width="200px" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlBusinessType"
                                                        ErrorMessage="*" Font-Bold="true">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: 8pt; width: 280px; font-family: Arial">
                                                    <asp:Button ID="btnAgentCode" runat="server" Text="Agent Code" OnClientClick="LoadWindows('FindAgent.aspx')"
                                                        CausesValidation="False" /></td>
                                                <td style="width: 395px">
                                                    <asp:TextBox ID="txtShortName" runat="server" Enabled="False"></asp:TextBox>
                                                    <asp:HiddenField ID="hfAgentKey" runat="server" />
                                                </td>
                                                <td style="width: 75616px">
                                                </td>
                                                <td style="font-size: 8pt; width: 98211px; font-family: Arial">
                                                    <strong>Currency</strong></td>
                                                <td style="width: 106990px">
                                                    <asp:DropDownList ID="ddlCurrency" runat="server" Width="200px" Enabled="False">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="ddlCurrency"
                                                        ErrorMessage="*" Font-Bold="true">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: 8pt; width: 280px; font-family: Arial">
                                                    <asp:Button ID="btnHandler" runat="server" OnClientClick="LoadWindows('FindHandler.aspx')"
                                                        Text="Handler" Width="96px" CausesValidation="False" /></td>
                                                <td style="width: 395px">
                                                    <asp:TextBox ID="txtHandler" runat="server" Enabled="False"></asp:TextBox>
                                                    <asp:HiddenField ID="hfHandlerCode" runat="server" />
                                                </td>
                                                <td style="width: 75616px">
                                                </td>
                                                <td style="width: 98211px">
                                                </td>
                                                <td style="width: 106990px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: 8pt; width: 280px; font-family: Arial">
                                                    Regarding</td>
                                                <td style="width: 395px">
                                                    <asp:TextBox ID="txtRegarding" runat="server" Width="200px"></asp:TextBox></td>
                                                <td style="width: 75616px">
                                                </td>
                                                <td style="width: 98211px">
                                                </td>
                                                <td style="width: 106990px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5" style="background-color: #cc99ff">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: 8pt; width: 280px; font-family: Arial">
                                                    <strong>Cover From</strong></td>
                                                <td>
                                                    <asp:TextBox ID="txtcoveredFrom" runat="server" Width="200px" Enabled="False"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtcoveredFrom"
                                                        ErrorMessage="*" Font-Bold="true" Width="1px"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <strong>Cover To</strong></td>
                                                <td>
                                                    <asp:TextBox ID="txtcoveredTo" runat="server" Width="200px" AutoPostBack="True"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtcoveredTo"
                                                        ErrorMessage="*" Font-Bold="true" Width="1px"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Inception</strong></td>
                                                <td>
                                                    <asp:TextBox ID="txtinceptionDate" runat="server" Width="200px" Enabled="False"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtinceptionDate"
                                                        ErrorMessage="*" Font-Bold="true" Width="1px"></asp:RequiredFieldValidator>&nbsp;
                                                </td>
                                                <td style="font-size: 8pt; width: 75616px; font-family: Arial; height: 26px">
                                                </td>
                                                <td style="font-size: 8pt; width: 98211px; font-family: Arial; height: 26px;">
                                                    <strong>Renewal</strong></td>
                                                <td style="width: 106990px; height: 26px;">
                                                    <asp:TextBox ID="txtRenewalDate" runat="server" Width="200px" Enabled="False"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtRenewalDate"
                                                        ErrorMessage="*" Font-Bold="true">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <strong>Inception TPI</strong></td>
                                                <td>
                                                    <asp:TextBox ID="txtInceptionTPI" runat="server" Width="200px" Enabled="False"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtInceptionTPI"
                                                        ErrorMessage="*" Font-Bold="true" Width="1px"></asp:RequiredFieldValidator>&nbsp;
                                                </td>
                                                <td style="font-size: 8pt; width: 75616px; font-family: Arial">
                                                </td>
                                                <td style="font-size: 8pt; width: 98211px; font-family: Arial">
                                                    Issued</td>
                                                <td style="width: 106990px">
                                                    <asp:TextBox ID="txtIssuedDate" runat="server" Width="200px"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: 8pt; width: 280px; font-family: Arial; height: 26px;">
                                                    H/C Expiry</td>
                                                <td style="font-size: 8pt; width: 395px; font-family: Arial; height: 26px;">
                                                    <asp:TextBox ID="txtHCExpiry" runat="server" Width="200px"></asp:TextBox></td>
                                                <td style="font-size: 8pt; width: 75616px; font-family: Arial; height: 26px;">
                                                </td>
                                                <td style="font-size: 8pt; width: 98211px; font-family: Arial; height: 26px;">
                                                    <strong>Quote Expiry</strong></td>
                                                <td style="width: 106990px; height: 26px;">
                                                    <asp:TextBox ID="txtQuoteexpiry" runat="server" Width="200px" Enabled="False"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtQuoteexpiry"
                                                        ErrorMessage="*" Font-Bold="true">
                                                    </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: 8pt; width: 280px; font-family: Arial">
                                                    Policy Deductable</td>
                                                <td style="font-size: 8pt; width: 395px; font-family: Arial">
                                                    <asp:DropDownList ID="DropDownList1" runat="server" Width="200px" Enabled="False">
                                                        <asp:ListItem>(None)</asp:ListItem>
                                                    </asp:DropDownList></td>
                                                <td style="font-size: 8pt; width: 75616px; font-family: Arial">
                                                </td>
                                                <td style="font-size: 8pt; width: 98211px; font-family: Arial">
                                                    Policy Limits</td>
                                                <td style="width: 106990px">
                                                    <asp:DropDownList ID="DropDownList2" runat="server" Width="200px" Enabled="False">
                                                        <asp:ListItem>(None)</asp:ListItem>
                                                    </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: 8pt; width: 280px; font-family: Arial; height: 21px">
                                                    Policy Type</td>
                                                <td style="font-size: 8pt; width: 395px; font-family: Arial; height: 21px">
                                                    <asp:Label ID="lblPolicyStatus" runat="server" Text="Label" Width="192px"></asp:Label></td>
                                                <td style="font-size: 8pt; width: 75616px; font-family: Arial; height: 21px">
                                                </td>
                                                <td style="font-size: 8pt; width: 98211px; font-family: Arial; height: 21px">
                                                    Scheme</td>
                                                <td style="width: 106990px; height: 21px">
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5" style="font-size: 8pt; font-family: Arial; height: 21px">
                                                    <asp:Button ID="btnLapseQuote" runat="server" Text="Lapse Quote" CausesValidation="False" Visible="False" />
                                                    <asp:Button ID="btnPolicyTax" runat="server" Text="PolicyTax" CausesValidation="False" Visible="False" />
                                                    <asp:Button ID="btnPolicyFee" runat="server" Text="Policy Fee" CausesValidation="False" Visible="False" />
                                                    <asp:Button ID="btnCommission" runat="server" Text="Commission" CausesValidation="False" Visible="False" /></td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                    <asp:View ID="View2" runat="server">
                                        <table>
                                            <tr>
                                                <td style="width: 280px; height: 13px;">
                                                    Frequency :</td>
                                                <td style="width: 449px; height: 13px;">
                                                    <asp:DropDownList ID="ddlFrequencyCode" runat="server" Width="204px">
                                                    </asp:DropDownList></td>
                                                <td style="width: 98211px; height: 13px;">
                                                    Renewal Method :</td>
                                                <td style="width: 106990px; height: 13px;">
                                                    <asp:DropDownList ID="ddlRenewalmethodCode" runat="server" Width="200px">
                                                    </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 280px; height: 7px;">
                                                    LTU expiry :&nbsp;</td>
                                                <td style="width: 449px; height: 7px;">
                                                    <asp:TextBox ID="txtLTUExpiry" runat="server" Width="200px"></asp:TextBox></td>
                                                <td style="width: 98211px; font-size: 8pt; font-family: Arial; height: 7px;">
                                                    Lapse/Cancellation Reason :</td>
                                                <td style="width: 106990px; height: 7px;">
                                                    &nbsp;<asp:DropDownList ID="ddlLapseCancellation" runat="server" Width="200px">
                                                    </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 280px; font-size: 8pt; font-family: Arial; height: 19px;">
                                                    Stop Reason :</td>
                                                <td style="width: 449px; height: 19px;">
                                                    <asp:DropDownList ID="ddlStopReasoCode" runat="server" Width="204px">
                                                    </asp:DropDownList></td>
                                                <td style="width: 98211px; font-size: 8pt; font-family: Arial; height: 19px;">
                                                    Lapse/Cancellation Date :</td>
                                                <td style="width: 106990px; height: 19px;">
                                                    <asp:TextBox ID="txtLapseCancelDate" runat="server" Width="200px"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: 8pt; width: 280px; font-family: Arial; height: 26px;">
                                                    Times renewed :</td>
                                                <td style="width: 449px; height: 26px;">
                                                    <asp:Label ID="lblTimesRenewed" runat="server" Text="Label"></asp:Label></td>
                                                <td style="font-size: 8pt; width: 98211px; font-family: Arial; height: 26px;">
                                                    <asp:CheckBox ID="CkBxReferrredatrenewal" runat="server" Text="Referrred at renewal ?" /></td>
                                                <td style="width: 106990px; height: 26px;">
                                                    &nbsp;<asp:CheckBox ID="CkBxReferrredonMTA" runat="server" Text="Referred on MTA ?" /></td>
                                            </tr>
                                            <tr>
                                                <td style="font-size: 8pt; width: 280px; font-family: Arial">
                                                </td>
                                                <td style="width: 449px">
                                                </td>
                                                <td style="font-size: 8pt; width: 98211px; font-family: Arial">
                                                </td>
                                                <td style="width: 106990px">
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
                                                        <asp:Button ID="btnAddPolicywording" runat="server" Text="Add" OnClientClick="LoadWindows('SelectClauses.aspx')" 
                                                        CausesValidation="False" Enabled="False" />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <asp:Button ID="btnCoinsurers" runat="server" Text="Coinsurers" CausesValidation="False" /></td>
                                            </tr>
                                           <tr style="height:100px">
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
                                        <asp:Button ID="btnAdd" runat="server" Text="Add" CausesValidation="False" /><br />
                                        <asp:Panel ID="Panel1" runat="server" Font-Names="Arial" Font-Size="8pt" Height="40px"
                                            Width="784px">
                                            Cover Note Book &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                            <asp:TextBox ID="txtCoverBook" runat="server" Width="200px"></asp:TextBox>
                                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Cover Note
                                            Sheet &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                            <asp:TextBox ID="txtCoverSheet" runat="server" Width="200px"></asp:TextBox></asp:Panel>
                                            <table width=100%>
                                            <tr style="height:250px">
                                            <td></td>
                                            </tr>
                                            </table>
                                    </asp:View>
                                </asp:MultiView>
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="False" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="cmdOk" runat="server" Text="Ok" Width="96px" />
                            
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" /></td>
                        </tr>
                        
                      
                    </table>
                </td>
            </tr>
            
        </table>
    
          <tr style="height:290px"><td> 
                <uc2:Footer ID="Footer1" runat="server" />

            </td></tr>
    
    </form>
</body>
</html>
