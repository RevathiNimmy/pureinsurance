<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ReinsuranceArrangements.aspx.vb"
    Inherits="Maintain_Claim_ReinsuranceArrangements" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    
    <table width="100%" style="height: 100%" border="0" cellspacing="0" cellpadding="0">
                <tr style="height: 10%">
                    <td style="width: 10%; height: 12%;" align="right">
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
                    <td style="height: 35px">
                      
                    </td>
                </tr>
                <tr style="height: 90%">
                    <td>
         <div>
            <table cellpadding="4" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <table cellpadding="4" cellspacing="0">
                            <tr>
                                <td>
                                    Reinsurance Band:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlReinsuranceBand" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvReinsuranceArrangements" runat="server">
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvArrangementLines" runat="server">
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
                    </td>
                </tr>
            </table>
    
    <%--    <div>
            <table cellpadding="4" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <table cellpadding="4" cellspacing="0">
                            <tr>
                                <td>
                                    Reinsurance Band:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlReinsuranceBand" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvReinsuranceArrangements" runat="server">
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvArrangementLines" runat="server">
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>--%>
    </form>
</body>
</html>
