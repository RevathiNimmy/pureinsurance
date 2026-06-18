<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditRisk.aspx.vb" Title="EditRisk" Inherits="MTA_EditRisk" %>

<%@ Register Src="../UserControl/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
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
            <tr>
                <td>
    <div>
        <table style="width: 279px; height: 307px">
            <tr>
                <td style="width: 100px">
                    Mobile Phone Make</td>
                <td style="width: 100px">
                    <asp:DropDownList ID="cboMobileMake" runat="server" Width="105px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 100px">
                    Mobile Model</td>
                <td style="width: 100px">
                    <asp:DropDownList ID="cboMobileModel" runat="server" Width="105px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 100px">
                    Identification No</td>
                <td style="width: 100px">
                    <asp:TextBox ID="txtIdentification" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 100px">
                    Insured Value</td>
                <td style="width: 100px">
                    <asp:TextBox ID="txtInsuredValue" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 100px">
                    Date of Purchased</td>
                <td style="width: 100px">
                    <asp:TextBox ID="txtDate" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 100px; height: 52px">
                    Insured Occupation</td>
                <td style="width: 100px; height: 52px">
                    <asp:TextBox ID="txtOccupation" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 100px; height: 31px">
                    <asp:Button ID="btnOK" runat="server" Text="OK" Width="104px" /></td>
                <td style="width: 100px; height: 31px">
                    <asp:Button ID="btnCancel" runat="server" Text="CANCEL" Width="104px" /></td>
            </tr>
        </table>
    
    </div>
    </td>
    </tr>
    <tr style="height:180px">
    <td>
        <uc2:Footer ID="Footer1" runat="server" />
    
    </td></tr>
    </table>
    </form>
</body>
</html>
