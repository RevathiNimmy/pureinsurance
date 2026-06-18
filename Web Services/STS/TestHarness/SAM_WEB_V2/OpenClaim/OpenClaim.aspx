<%@ Page Language="VB" AutoEventWireup="false" CodeFile="OpenClaim.aspx.vb" Inherits="OpenClaim_OpenClaim" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Open Claim</title>
    <%--<script type="text/javascript">
        Function fnOpenModal()
        {
            window.showModalDialog("ClaimReinsuranceBreakdown.aspx");
        }
    </script>--%>
</head>
<body>
    <form id="form1" runat="server">
        <div>
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
                <tr style="height: 90%">
                    <td>
                        Claim number:<asp:Label ID="lblClaimNumber" runat="server" Width="256px"></asp:Label>&nbsp;<br />
                        &nbsp;&nbsp;
                        <br />
                        <br />
                        <hr />
                        <asp:Button ID="btnCoInsuranceBreakDown" runat="server" Text="CoInsurance BreakDown">
                        </asp:Button>
                        <br />
                        <br />
                        <asp:Button ID="btnRrinsuranceBreakdown" runat="server" Text="Reinsurance Break down" /><br />
                        <br />
                        <br />
                        <asp:CheckBox ID="chkGenerateClaimDocument" runat="server" Text="Generate Claim Document"
                            AutoPostBack="True" />
                        <br />
                        <br />
                        <br />
                        <asp:Button ID="btnGenerateDocument" runat="server" Enabled="False" Text="Generate Document" />
                    </td>
                </tr>
            </table>
            <%-- Claim number:<asp:Label ID="lblClaimNumber" runat="server" Width="256px"></asp:Label>
            <br />
            Claim Key: &nbsp; &nbsp;
            <asp:Label ID="lblClaimKey" runat="server" Width="256px"></asp:Label><br />
            <br />
            <hr />
            <asp:Button ID="btnCoInsuranceBreakDown" runat="server" Text="CoInsurance BreakDown">
            </asp:Button>
            <br />
            <br />
            <asp:Button ID="btnRrinsuranceBreakdown" runat="server" Text="Reinsurance Break down" /><br />
            <br />
            <br />
            <asp:CheckBox ID="chkHandleExternally" runat="server" Text="Handle Claim Externally" /><br />
            <br />
            <br />
            <asp:CheckBox ID="chkGenerateClaimDocument" runat="server" Text="Generate Claim Document"
                AutoPostBack="True" />
            <br />
            <br />
            <br />
            <asp:Button ID="btnGenerateDocument" runat="server" Enabled="False" Text="Generate Document" />--%>
        </div>
    </form>
</body>
</html>
