<%@ Page Language="VB" AutoEventWireup="false" CodeFile="wfrmRunRenewalSelection.aspx.vb" Inherits="PolicyRenewal_wfrmRunRenewalSelection" %>

<%@ Register Src="~/UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Footer.ascx"TagName="Footer" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
     <script language="javascript"  type ="text/javascript">
    
    function LoadWindows(url,width,height)
    {
    window.open(url,"","width=" + width+ ",height="+ height+",scrollbars=1")
    
    }
       
    </script>

    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Images/loginstylesheet.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
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
                <td>
    <table cellpadding="2" cellspacing="2" style="vertical-align:middle; width:100%;">
        <tr >
            <td align="center" >
             <asp:Panel ID="pnlConfirm" runat="server" Height="50px" Width="300px" Visible="False">
        <table cellpadding="2" cellspacing="2" style="vertical-align:middle; width:136%;">
        <tr align="center">
            <td align="left">
                <asp:Label ID="lblConfirmMessage" runat="server" Text=""></asp:Label>
            </td>
            <td align="right" style="width: 9px">
            </td>
        </tr>
        <tr>
             <td align="center">
             <asp:Button ID="btnYes" runat="server" Text="Yes" Width="75px" CausesValidation="False" />
             <asp:Button ID="btnNo" runat="server" Text="No" Width="75px" CausesValidation="False" /> 
            </td>
        </tr>        
    </table>
    </asp:Panel>
                <asp:Panel ID="pnlFilterRenewalSelection" runat="server"
                    GroupingText="Filter Criteria" Width="100%">
                    <table cellpadding="2" cellspacing="2" width="100%">
                                         
                         <tr >
                            <td align="left" valign="top">
                                <asp:Label ID="lblCompareDate" runat="server" Text="End Date :"></asp:Label></td>
                             <td align="left" valign="top">
                                 <asp:Calendar ID="calCompareDate" runat="server"></asp:Calendar>
                             </td>
                        </tr>
                        <tr >
                            <td align="left" valign="top">
                                <asp:CheckBox ID="chkStartDate" runat="server" AutoPostBack="True" Text="Start Date :" /></td>
                             <td align="left" valign="top">
                                 <asp:Calendar ID="calStartDate" runat="server"></asp:Calendar>
                             </td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <asp:Label ID="Label1" runat="server" Text="Product Code :" Font-Bold="False"></asp:Label></td>
                             <td align="left" valign="top">
                                 
                                 <asp:DropDownList ID="ddlProductCode" runat="server" AppendDataBoundItems="True">
                                     <asp:ListItem Value="0">All</asp:ListItem>
                                 </asp:DropDownList>
                                 <asp:RequiredFieldValidator ID="rfvProductCode" runat="server" ControlToValidate="ddlProductCode"
                                     ErrorMessage="*" InitialValue="0"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <asp:Label ID="lblBranchCode" runat="server" Text="Branch Code :" Font-Bold="False"></asp:Label></td>
                             <td align="left" valign="top">
                                 
                                 <asp:DropDownList ID="ddlBranchCode" runat="server" AppendDataBoundItems="True">
                                     <asp:ListItem Value="0"></asp:ListItem>
                                 </asp:DropDownList>
                                 <asp:RequiredFieldValidator ID="rfvBranchCode" runat="server" ControlToValidate="ddlBranchCode"
                                     ErrorMessage="*" InitialValue="0"></asp:RequiredFieldValidator></td>
                        </tr>
                    </table>    
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="right" valign="top" style="height: 34px">
            <table cellpadding="2" cellspacing="2">
                <tr>
                    <td>
                        <asp:Button ID="btnOK" runat="server" Text="OK" Width="75px" />
                    </td>
                    <td>
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="75px" CausesValidation="False" /> 
                        
                    </td>
                    
                </tr>
            </table>
            </td>
        </tr>
         <tr>
            <td align="left" valign="top">
                <asp:Label ID="lblOutput" runat="server" Text="" Visible="false"></asp:Label></td>
        </tr>
    </table>  
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

