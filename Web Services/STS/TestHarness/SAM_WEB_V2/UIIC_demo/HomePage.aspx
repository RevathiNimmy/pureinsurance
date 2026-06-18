<%@ Page Language="VB" AutoEventWireup="false" CodeFile="HomePage.aspx.vb" Inherits="UIIC_demo_HomePage" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/VerticalMenu.ascx" TagName="VerticalMenu" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="100%" style="height: 100%" cellspacing="0" cellpadding="0">
                <tr style="height: 10%">
                    <td style="width: 10%" align="right">
                        <table width="100%">
                            <tr>
                                <td>
                                    <uc2:Header ID="Header1" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                            <tr style="height: 100px">
                                <td>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr style="height: 90%">
                    <td align="center" valign="middle">
                        <uc1:VerticalMenu ID="VerticalMenu1" runat="server" />
                    </td>
                </tr>
                <tr style="height:95px">
                </tr>
                <tr >
                    <td class="footer">
                © Copyright 2008 SSP Limited. UK only.</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
