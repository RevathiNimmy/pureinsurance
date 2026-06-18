<%@ Page Language="VB" AutoEventWireup="false" CodeFile="wfrmGenerateInvite.aspx.vb" Inherits="PolicyRenewal_wfrmGenerateInvite" %>
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

       <asp:Panel id="pnlDeleteRenewalSelection" runat="server" GroupingText="By Policy" Width="408px" Height="50px">
        <table cellpadding="2" cellspacing="2" style="vertical-align:middle; width:100%;">
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
        <tr align="center">
            <td align="left">
                <asp:CheckBox ID="chkOutputHTML" runat="server"  Text="Output as HTML"/>
            </td>
            <td align="left">
                <asp:CheckBox ID="chkOutputPDF" runat="server"  Text="Output as PDF"/>
            </td>
            <td>
                <asp:CheckBox ID="chkSpoolDocument" runat="server"  Text="Spool Document"/></td>
        </tr>
        <tr>
             <td align="left" valign="top" style="height: 53px" >
             </td>
             <td colspan="2" style="height: 53px">
             <table>
             <tr align="right">
             <td>
            <asp:Button ID="btnOK" runat="server" Text="OK" Width="75px"  />
             <asp:Button ID="btnCancel" runat="server" Text="Cancel"  Width="75" CausesValidation="False"  />
              </td>
             </tr>
             </table>
             
            </td>
        </tr>  
        <tr>
        <td>
        <asp:Label ID="lblOutput" runat="server" Text=""></asp:Label>
        </td>
        <td>
            &nbsp;</td>
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
