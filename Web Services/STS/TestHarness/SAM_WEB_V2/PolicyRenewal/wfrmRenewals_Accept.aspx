<%@ Page Language="VB" AutoEventWireup="false" CodeFile="wfrmRenewals_Accept.aspx.vb" Inherits="PolicyRenewal_wfrmRenewals" %>
<%@ Register Src="~/UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Footer.ascx"TagName="Footer" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Renewals</title>
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Images/loginstylesheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
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
                <td style="height: 740px">

        <asp:Panel ID="pnlSetStatus" runat="server" Height="50px" Width="300px" Visible="False">
        <table cellpadding="2" cellspacing="2" style="vertical-align:middle; width:100%;">
        <tr align="center">
            <td align="left">
                <asp:Label ID="lblRenewalStatus" runat="server" Text="Renewal Status : "></asp:Label>
            </td>
            <td align="right">
                <asp:DropDownList ID="ddlRenewalStatus" runat="server" Width="300px">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td align="left">
                
            </td>
             <td align="left">
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr align="right">
                        <td style="width: 277px">
                            <asp:Button ID="btnSetStatus" runat="server" Text="OK" OnClientClick="return confirm('All selected records will be set to new status. Do you want to continue?');" Width="75px"/></td>
                        <td>
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" /></td>
                    </tr>
                </table>
            </td>
        </tr>        
    </table>
    </asp:Panel>
    <asp:Panel ID="pnlRenewal" runat="server" GroupingText="Search" ScrollBars="None">
   <table cellpadding="2" cellspacing="2" style="vertical-align:middle; width:100%;">
        <tr align="center">
            <td align="left">
                <asp:Label ID="lblSearch" runat="server" Text="Search"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left" style="height: 218px">
                <asp:GridView ID="gvRenewals" runat="server" AutoGenerateColumns="False" DataKeyNames="PartyKey,InsuranceFolderKey,InsuranceFileKey,RenewalStatusKey,ProductCode,LeadAgentKey">
                    <Columns>
                    <asp:TemplateField HeaderText="Select">
                    <ItemTemplate>
                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelect_SelectChanged"/>
                    </ItemTemplate>
                    </asp:TemplateField>
                        <asp:BoundField HeaderText="PartyKey" DataField="PartyKey" Visible="False" />
                        <asp:BoundField DataField="BranchCode" HeaderText="Branch" />
                        <asp:BoundField HeaderText="Client Code" DataField="PartyName" />
                        <asp:BoundField HeaderText="Policy Number" DataField="InsuranceFileRef" />
                        <asp:BoundField HeaderText="InsuranceFileKey" DataField="InsuranceFileKey" Visible="False" />
                        <asp:BoundField HeaderText="InsuranceFolderKey" DataField="InsuranceFolderKey" Visible="False" />
                        <asp:BoundField HeaderText="Insurance File Status" DataField="InsuranceFileStatusDescription" />
                        <asp:BoundField HeaderText="Insurance File Type" DataField="InsuranceFileTypeDescription" />
                        <asp:BoundField DataField="RenewalStatusTypeCode" HeaderText="RenewalStatusTypeCode" Visible="False" />
                        <asp:BoundField DataField="RenewalStatusTypeDescription" HeaderText="Renewal Status" />
                        <asp:BoundField DataField="CoverStartDate" HeaderText="Cover Start Date" />
                        <asp:BoundField DataField="CoverEndDate" HeaderText="Cover End Date" />
                        <asp:BoundField DataField="RenewalDate" HeaderText="Renewal Date" />
                        <asp:BoundField DataField="RenewalPremium" HeaderText="Renewal Premium" />
                        <asp:BoundField HeaderText="ProductCode" DataField="ProductCode" Visible="False" />
                        <asp:BoundField DataField="ProductDescription" HeaderText="Product" />
                        <asp:BoundField DataField="LeadAgentKey" HeaderText="LeadAgentKey" Visible="False" />
                        <asp:BoundField DataField="LeadAgent" HeaderText="Lead Agent" />
                        <asp:BoundField DataField="AccHandler" HeaderText="Acc.Handler" />
                        <asp:BoundField DataField="ClaimIndicator" HeaderText="Claims Indicator" />
                        <asp:BoundField DataField="IsClosed" HeaderText="Closed" />
                        <asp:BoundField DataField="IsTrueMonthlyPolicy" HeaderText="True Monthly" />
                        <asp:BoundField DataField="AnniversaryCopy" HeaderText="Anniversary Copy" />
                    </Columns>
                    <HeaderStyle BackColor="Silver" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="center" style="height: 64px">
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td align="left" style="height: 47px">
                            <table cellpadding="2" cellspacing="2" width="100">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnAccept" runat="server" Text="Accept" /></td>
                                    <td>
                                        <asp:Button ID="btnSelectAll" runat="server" Text="Select All" /></td>
                                    <td>
                                       <asp:Button ID="btnStatus" runat="server" Text="Status" /></td>
                                    <td>
                                        <asp:Button ID="btnFilter" runat="server" Text="Filter" /></td> 
                                        <%-- <input type ="button" name="Filter" value="Filter" onclick ="PopUp()" />--%>
                                </tr>
                            </table>
                        </td>
                        <td align="right" style="width: 60px; height: 47px;">
                            <asp:Button ID="btnOK" runat="server" Text="OK" Width="75px" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left" style="height: 23px">
                <asp:Label ID="lblItemCount" runat="server" Text="0"></asp:Label>
                <asp:Label ID="lblCountText" runat="server" Text=" Item(s) found"></asp:Label>
            </td>
        </tr>
    </table>
   </asp:Panel>
    <asp:Panel ID="pnlChangePolicyDetails" runat="server" Height="50px" Width="592px" Visible="False">
         <table cellpadding="2" cellspacing="2" style="vertical-align:middle; width:100%;">
            <tr>
                 <td style="width: 517px" align="right">
                     <asp:Panel GroupingText="Policy Details" Height="50px" ID="pnlPolicyDetails" runat="server">
                        <table cellpadding="2" cellspacing="2" style="vertical-align:middle; width:100%;">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblPolicyNumber" runat="server" Text="Policy Number"></asp:Label></td>
                                <td align="left">
                                    <asp:TextBox ID="txtPolicyNumber" runat="server" Width="288px"></asp:TextBox></td>
                                <td align="left">
                                    <asp:Button ID="btnChange" runat="server" Text="Change" Enabled="False" /></td>
                            </tr>
                            <tr>
                                <td align="left" style="height: 23px">
                                    <asp:Label ID="lblCoverStart" runat="server" Text="Cover Start"></asp:Label></td>
                                <td align="left" colspan="2" style="height: 23px">
                                    <asp:TextBox ID="txtCoverStart" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblCoverExpiry" runat="server" Text="Cover Expiry"></asp:Label></td>
                                <td align="left" colspan="2">
                                    <asp:TextBox ID="txtCoverExpiry" runat="server"></asp:TextBox></td>
                             </tr>
                        </table>
                     </asp:Panel>
                 </td>
             </tr>
             <tr>
                 <td style="width: 517px" >
                     <table width="100%">
                         <tr align="right">
                             <td style="width: 402px; height: 51px" align="left">
                                 <asp:Panel ID="pnlPaymentMethod" runat="server" GroupingText="Payment Method" Height="50px"
                                     Width="125px">
                                     <asp:RadioButtonList ID="rblPaymentMethod" runat="server" Width="80px">
                                         <asp:ListItem>Invoice</asp:ListItem>
                                         <asp:ListItem Selected="True" Value="PayNow">Pay Now</asp:ListItem>
                                     </asp:RadioButtonList></asp:Panel>
                              </td>
                             <td style="width: 165px; height: 51px" align="right">
                                 <table >
                                     <tbody>
                                         <tr>
                                             <td align="right" style="height: 25px">
                                                 <asp:Button ID="btnChangeDetailsOK" runat="server" Text="OK" Width="75px" />
                                             </td>
                                             <td align="right" style="height: 25px">
                                                 <asp:Button ID="btnChangeDetailsCancel" runat="server" Text="Cancel" Width="75px" />
                                             </td>
                                         </tr>
                                     </tbody>
                                 </table>
                             </td>
                         </tr>
                     </table>
                 </td>
             </tr>
    </table>
        </asp:Panel>
        <asp:Label id="lblOutput" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr style="height: 90%">
                    <td>
                        <uc2:Footer ID="Footer1" runat="server" />
                    </td>
                    
                </tr>
    </table>
   </div>
    </form>
</body>
</html>
