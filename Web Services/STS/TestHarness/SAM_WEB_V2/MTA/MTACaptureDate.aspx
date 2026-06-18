<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MTACaptureDate.aspx.vb"
    Inherits="MTA_MTACaptureDate" Title="MTACaptureDate" %>

<%@ Register Src="../UserControl/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
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
                    <div style="vertical-align: middle;">
                        <table style="height: 100%; width: 100%">
                            <tr>
                                <td style="vertical-align: middle;">
                                    <table border="0" width="50%" style="height: 50%" cellpadding="4">
                                        <tr>
                                            <td>
                                                MTA Date
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMTADate" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtMTADate"
                                                    ErrorMessage="*" Font-Bold="true">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:CheckBox ID="chkIsPermanent" runat="server" Text="Permanent MTA" Checked="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Reason</td>
                                            <td style="text-align: left">
                                                <asp:DropDownList ID="ddlMTAReason" runat="server">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                If Other</td>
                                            <td style="text-align: left">
                                                <asp:TextBox ID="txtMTAReason" runat="server">OTHER</asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td colspan="1" style="text-align: right">
                                                <asp:Button ID="btnOK" runat="server" Text="Ok" Width="59px" /></td>
                                            <td>
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" /></td>
                                        </tr>
                                    </table>
                                    <asp:ValidationSummary ID="ValidationSummary1" ShowMessageBox="false" ShowSummary="false"
                                        runat="server"></asp:ValidationSummary>
                                    <asp:Label ID="lblError" runat="server"></asp:Label></td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr style="height:200px">
            <td>
                <uc2:Footer ID="Footer1" runat="server" />
            
            </td>
            </tr>
        </table>
    </form>
</body>
</html>
