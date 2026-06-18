<%@ Page Language="VB" AutoEventWireup="false" CodeFile="wfrmFilterRenewals.aspx.vb" Inherits="PolicyRenewal_wfrmFilterRenewals" %>
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

    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Images/loginstylesheet.css" rel="stylesheet" type="text/css" />
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
                <asp:Panel ID="pnlFilterRenewals" runat="server"
                    GroupingText="Filter Criteria" Width="100%">
                    <table cellpadding="2" cellspacing="2" width="100%">
                                         
                        <tr>
                            <td align="left" valign="top">
                                <asp:Label ID="lblBranchCode" runat="server" Text="Branch Code :" Font-Bold="False"></asp:Label></td>
                             <td align="left" valign="top">
                                 
                                 <asp:DropDownList ID="ddlBranchCode" runat="server" AppendDataBoundItems="True">
                                     <asp:ListItem></asp:ListItem>
                                 </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td align="left" valign="top">
                                <asp:Label ID="Label1" runat="server" Text="Product Code :" Font-Bold="False"></asp:Label></td>
                             <td align="left" valign="top">
                                 
                                 <asp:DropDownList ID="ddlProductCode" runat="server" AppendDataBoundItems="True">
                                     <asp:ListItem Value="0">All</asp:ListItem>
                                 </asp:DropDownList></td>
                        </tr>
                        <tr >
                            <td align="left" valign="top">
                                <asp:Label ID="lblAgentCode" runat="server" Text="Agent Code :" Font-Bold="False"></asp:Label></td>
                             <td align="left" valign="top">
                                 <asp:TextBox ID="txtAgentCode" runat="server"></asp:TextBox>
                                 <asp:Button ID="btnFindAgent" runat="server" Text="Find" OnClientClick="LoadWindows('wfrmFindAgent.aspx',600,500)" />
                                 &nbsp; &nbsp; &nbsp; <asp:CheckBox ID="chkDirectBusiness" runat="server" Text="Direct Business Only" /></td>
                        </tr>
                         <tr >
                            <td align="left" valign="top">
                                <asp:Label ID="lblPartyShortName" runat="server" Text="Party Short Name :" Font-Bold="False"></asp:Label></td>
                             <td align="left" valign="top">
                                 <asp:TextBox ID="txtPartyShortName" runat="server"></asp:TextBox>
                                 <asp:Button ID="btnFindParty" runat="server" Text="Find" OnClientClick="LoadWindows('wfrmFindParty.aspx',600,500)" /></td>
                        </tr>
                        <tr >
                            <td align="left" valign="top">
                                <asp:CheckBox ID="chkRenewalDate" runat="server" AutoPostBack="True" Text="Renewal Date :" /></td>
                             <td align="left" valign="top">
                                 <asp:Calendar ID="calRenewalDate" runat="server"></asp:Calendar>
                             </td>
                        </tr>
                    </table>    
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="right" valign="top">
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
