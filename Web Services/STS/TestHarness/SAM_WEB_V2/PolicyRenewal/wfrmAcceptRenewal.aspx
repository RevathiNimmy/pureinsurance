<%@ Page Language="VB" AutoEventWireup="false" CodeFile="wfrmAcceptRenewal.aspx.vb" Inherits="PolicyRenewal_wfrmAcceptRenewal" %>
<%@ Register Src="~/UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Footer.ascx"TagName="Footer" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
         <script language="javascript"  type ="text/javascript">
    
    function LoadWindows(url,width,height)
    {
    window.open(url,"","width=" + width+ ",height="+ height+",scrollbars=1")
    
    }
    
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
            <asp:Panel ID="pnlDeleteRenewalSelection" runat="server" GroupingText="By Policy"
                Height="50px" Width="408px">
                <table cellpadding="2" cellspacing="2" style="vertical-align: middle; width: 100%">
                    <tr align="center">
                        <td align="left">
                            <asp:Label ID="lblPolicyRef" runat="server" Text="Policy Ref:"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtInsuranceRef" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvInsuranceRef" runat="server" ControlToValidate="txtInsuranceRef"
                                ErrorMessage="*"></asp:RequiredFieldValidator></td>
                        <td>
                            <asp:Button ID="btnFindInsuranceFile" runat="server" Text="..." OnClientClick="LoadWindows('wfrmFindInsuranceFile.aspx',600,500)" CausesValidation="False" /></td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                        <asp:Panel ID="pnlPaymentMethod" runat="server" GroupingText="Payment Method" Height="50px"
                                     Width="125px">
                                     <asp:RadioButtonList ID="rblPaymentMethod" runat="server" Width="80px">
                                         <asp:ListItem>Invoice</asp:ListItem>
                                         <asp:ListItem Selected="True" Value="PayNow">Pay Now</asp:ListItem>
                                     </asp:RadioButtonList></asp:Panel>
                        </td>
                        <td colspan="2">
                            <table>
                                <tr align="right">
                                    <td style="height: 26px">
                                        <asp:Button ID="btnOK" runat="server" Text="OK" Width="75px" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="75px" CausesValidation="False"/>
                                    </td>
                                </tr>
                            </table>
                         </td>  
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblOutput" runat="server" Text=""></asp:Label>
                        </td>

                    </tr>
                </table>
            </asp:Panel>

    

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
