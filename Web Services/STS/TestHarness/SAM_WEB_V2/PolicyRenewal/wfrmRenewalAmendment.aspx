<%@ Page Language="VB" AutoEventWireup="false" CodeFile="wfrmRenewalAmendment.aspx.vb" Inherits="PolicyRenewal_wfrmRenewalAmendment" %>
<%@ Register Src="~/UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Footer.ascx"TagName="Footer" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Renewal Amendment</title>
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

    <table cellpadding="2" cellspacing="2" border="1" style="vertical-align:middle; width:100%;">
        <tr align="center">
            <td align="center">
                <asp:Button ID="btnAmend" runat="server" Text="Amend" /></td>
            <td align="center">
                <asp:Button ID="btnAccept" runat="server" Text="Accept" /></td>
            <td align="center">
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" /></td>
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
