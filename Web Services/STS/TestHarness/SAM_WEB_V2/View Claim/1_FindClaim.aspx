<%@ Page Language="VB" AutoEventWireup="false" CodeFile="1_FindClaim.aspx.vb" Inherits="View_Claim_FindClaim" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Find Claim</title>

    <script language="javascript" type="text/javascript">
    
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
                                <td>
                                    <uc1:Header ID="Header1" runat="server" />
                                   
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style ="width :100%">
                        <hr />
                    </td>
                </tr>
                <tr style="height: 90%">
                    <td>
                        <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
                            ForeColor="#C000C0" Text="Find Claim" Width="192px"></asp:Label>
            &nbsp;<%--</div>--%>
        <table style="width: 600px">
        <tr>
        <td style ="width :10%">Policy</td>
        <td style ="width :30%"><asp:TextBox ID="txtPolicyNumber" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
        <td>Risk Index</td>
        <td><asp:TextBox ID="txtRiskIndex" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
        <td>Claim</td>
        <td><asp:TextBox ID="txtClaim" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
        <td><input id="btnShortName" style="width: 96px; font-weight: bold;" type="button" value="Short Name"
                        onclick="Javascript:LoadWindows('../Lookup screens/Find Party.aspx',600,600)" /></td>
        <td><asp:TextBox ID="txtShortName" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
        <td>Include Closed Claim</td>
        <td> <asp:CheckBox ID="chkIncludeCloseClaim" runat="server" 
                            Width="176px" /></td>
        </tr>
        <tr>
        <td>Loss Date Start Limit</td>
        <td><asp:TextBox ID="txtInForceFrom" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
        <td>Loss Date Stop Limit</td>
        <td><asp:TextBox ID="txtInForceTo" runat="server"></asp:TextBox></td>
        </tr>
           <%-- <tr>
                <td colspan="3">
                    Policy&nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:TextBox ID="txtPolicyNumber" runat="server"></asp:TextBox><br />
                    Risk Index&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:TextBox ID="txtRiskIndex" runat="server"></asp:TextBox><br />
                    Claim <strong> &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; </strong>
                    <asp:TextBox ID="txtClaim" runat="server"></asp:TextBox><br />
                    <input id="btnShortName" style="width: 96px; font-weight: bold;" type="button" value="Short Name"
                        onclick="Javascript:LoadWindows('../Lookup screens/Find Party.aspx',600,600)" /><strong>
                            &nbsp;</strong>  &nbsp;&nbsp;<asp:TextBox ID="txtShortName" runat="server"></asp:TextBox><br />
                   <%-- <strong>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; </strong>--%>
                   <%-- <asp:CheckBox ID="chkIncludeCloseClaim" runat="server" Text="Include Closed Claim"
                        Width="176px" /><strong> &nbsp; &nbsp;
                            <br />
                        </strong>Loss Date Start Limit<strong> &nbsp; </strong>
                    <asp:TextBox ID="txtInForceFrom" runat="server"></asp:TextBox><br />
                    Loss Date Start Limit<strong> &nbsp; </strong>
                    <asp:TextBox ID="txtInForceTo" runat="server"></asp:TextBox></td>
            </tr>--%>
            <tr>
                <td style="width: 442px">
                    <asp:Button ID="btnFind" runat="server" Text="Find Now" />
                </td>
                <td colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="3" style="width:100%">
                    &nbsp;<%--<hr />--%>
                    <asp:GridView ID="gvResult" runat="server" DataKeyNames="ClaimKey" AutoGenerateColumns="False"
                        CellPadding="4" ForeColor="#333333" GridLines="None">
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" />
                            <asp:BoundField DataField="ClaimNumber" HeaderText="Claim Reference" />
                            <asp:BoundField DataField="InsuranceRef" HeaderText="Policy Reference" />
                            <asp:BoundField DataField="ClientShortName" HeaderText="Client" />
                            <asp:BoundField DataField="ProductDescription" HeaderText="Product Description" />
                            <asp:BoundField DataField="LossDateFrom" HeaderText="Loss Date" />
                        </Columns>
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                        <EditRowStyle BackColor="#999999" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="Smaller" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="4" style="height: 13px">
                   <%-- <hr />--%>
                    <asp:Button ID="btnOk" runat="server" Text="Ok" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                </td>
            </tr>
        </table>
        </td> </tr> </table>
        <%-- <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
                ForeColor="#C000C0" Text="Find Claim" Width="192px"></asp:Label>&nbsp;</div>
        <table style="width: 600px">
            <tr>
                <td colspan="3">
                    &nbsp;Policy &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:TextBox ID="txtPolicyNumber" runat="server"></asp:TextBox><br />
                    Risk Index &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:TextBox ID="txtRiskIndex" runat="server"></asp:TextBox><br />
                    Claim <strong>&nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; </strong>
                        <asp:TextBox ID="txtClaim" runat="server"></asp:TextBox><br />
                        <input id="btnShortName" style="width: 96px; font-weight: bold;" type="button" value="Short Name" onclick="Javascript:LoadWindows('../Lookup screens/Find Party.aspx',600,600)"/><strong>
                            &nbsp;</strong><asp:TextBox ID="txtShortName" runat="server"></asp:TextBox><br />
                    <strong>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; </strong>
                        <asp:CheckBox ID="chkIncludeCloseClaim" runat="server" Text="Include Closed Claim"
                            Width="176px" /><strong>
                        &nbsp; &nbsp;
                        <br />
                            </strong>Loss Date Start Limit<strong> &nbsp; </strong>
                        <asp:TextBox ID="txtInForceFrom" runat="server"></asp:TextBox><br />
                        Loss Date Start Limit<strong> &nbsp; &nbsp; </strong>
                        <asp:TextBox ID="txtInForceTo" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="width: 442px">
                    <asp:Button ID="btnFind" runat="server" Text="Find Now" />
                    </td>
                <td colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="3" style="height: 13px">
                    &nbsp;<hr /><asp:GridView ID="gvResult" runat="server" DataKeyNames="ClaimKey" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None">
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" />
                            <asp:BoundField DataField="ClaimNumber" HeaderText="Claim Reference" />
                            <asp:BoundField DataField="InsuranceRef" HeaderText="Policy Reference" />
                            <asp:BoundField DataField="ClientShortName" HeaderText="Client" />
                            <asp:BoundField DataField="ProductDescription" HeaderText="Product Description" />
                            <asp:BoundField DataField="LossDateFrom" HeaderText="Loss Date" />
                        </Columns>
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                        <EditRowStyle BackColor="#999999" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="Smaller" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    </asp:GridView>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="3" style="height: 13px">
                <hr />
                    <asp:Button ID="btnOk" runat="server" Text="Ok" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                </td>
            </tr>
        </table>--%>
        </div>
    </form>
</body>
</html>
