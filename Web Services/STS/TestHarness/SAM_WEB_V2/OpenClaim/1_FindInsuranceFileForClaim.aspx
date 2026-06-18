<%@ Page Language="VB" AutoEventWireup="false" CodeFile="1_FindInsuranceFileForClaim.aspx.vb"
    Inherits="OpenClaim_1_FindInsuranceFile" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Find Insusrance File</title>

    <script language="javascript" type="text/javascript">
    function test()
    {
     alert('Hello');
    }
    
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
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr style="height: 90%">
                    <td>
                        <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
                            ForeColor="#C000C0" Text="Find Insurance file" Width="232px"></asp:Label>&nbsp;
                        <table style="width: 100%">
                            <tr>
                                <td colspan="3">
                                    <asp:Menu ID="mnuFindInsuranceFile" runat="server" BackColor="#F7F6F3" DynamicHorizontalOffset="2"
                                        Font-Names="Verdana" Font-Size="0.8em" ForeColor="#7C6F57" Orientation="Horizontal"
                                        StaticSubMenuIndent="10px" Width="100%">
                                        <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                                        <DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
                                        <DynamicMenuStyle BackColor="#F7F6F3" />
                                        <StaticSelectedStyle BackColor="#5D7B9D" />
                                        <DynamicSelectedStyle BackColor="#5D7B9D" />
                                        <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                                        <Items>
                                            <asp:MenuItem Text="1-General" Value="0"></asp:MenuItem>
                                            <asp:MenuItem Text="2-Advanced" Value="1"></asp:MenuItem>
                                        </Items>
                                        <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                                    </asp:Menu>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 442px">
                                    <asp:MultiView ID="mvFindInsuranceFileForClaim" runat="server" ActiveViewIndex="0">
                                        <asp:View ID="Genral" runat="server">
                                            <table>
                                                <tr>
                                                    <td>
                                                        Policy Number</td>
                                                    <td>
                                                        <asp:TextBox ID="txtPolicyNumber" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Cover Note Sheet Number</td>
                                                    <td>
                                                        <asp:TextBox ID="txtCoverNoteSheetNumber" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Risk Index</td>
                                                    <td>
                                                        <asp:TextBox ID="txtRiskIndex" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <strong>
                                                        Claim Date</strong></td>
                                                    <td>
                                                        <asp:TextBox ID="txtClaimDate" runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtClaimDate"
                                                            ErrorMessage="*" Font-Bold="True"></asp:RequiredFieldValidator></td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                        <asp:View ID="Advanced" runat="server">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <input id="btnShortName" style="width: 96px" type="button" value="Short Name" onclick="Javascript:LoadWindows('../Lookup screens/Find Party.aspx',600,600)" tabindex="-1" /></td>
                                                    <td>
                                                        <asp:TextBox ID="txtShortName" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Post Code</td>
                                                    <td>
                                                        <asp:TextBox ID="txtPostCode" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        In Force From</td>
                                                    <td>
                                                        <asp:TextBox ID="txtInForceFrom" runat="server"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        In Force to</td>
                                                    <td>
                                                        <asp:TextBox ID="txtInForceTo" runat="server"></asp:TextBox></td>
                                                </tr>
                                            </table>
                                        </asp:View>
                                    </asp:MultiView>
                                    <asp:Button ID="btnFind" runat="server" Text="Find Now" /></td>
                                <td colspan="2" style="width: 80px">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="3" style="height: 100px">
                                    <hr />
                                    &nbsp;<asp:Panel ID="Panel1" runat="server" Height="300px" ScrollBars="Auto" Width="800px">
                                        <asp:GridView ID="gvResult" runat="server" Width="232px" CellPadding="4" ForeColor="#333333"
                                            GridLines="None">
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                                            <EditRowStyle BackColor="#999999" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:CommandField ShowSelectButton="True" />
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <hr />
                                    <asp:Button ID="btnOk" runat="server" Text="Ok" UseSubmitBehavior="False" TabIndex="1" Visible="False" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" UseSubmitBehavior="False" TabIndex="2" Visible="False" />
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="False" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <%-- <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
            ForeColor="#C000C0" Text="Find Insurance file" Width="232px"></asp:Label>&nbsp;</div>
        <table style="width: 600px">
            <tr>
                <td colspan="3">
                    <asp:Menu ID="mnuFindInsuranceFile" runat="server" BackColor="#F7F6F3" DynamicHorizontalOffset="2"
                        Font-Names="Verdana" Font-Size="0.8em" ForeColor="#7C6F57" Orientation="Horizontal"
                        StaticSubMenuIndent="10px" Width="100%">
                        <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                        <DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
                        <DynamicMenuStyle BackColor="#F7F6F3" />
                        <StaticSelectedStyle BackColor="#5D7B9D" />
                        <DynamicSelectedStyle BackColor="#5D7B9D" />
                        <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                        <Items>
                            <asp:MenuItem Text="1-Genral" Value="0"></asp:MenuItem>
                            <asp:MenuItem Text="2-Advanced" Value="1"></asp:MenuItem>
                        </Items>
                        <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                    </asp:Menu>
                </td>
            </tr>
            <tr>
                <td style="width: 442px">
                    <asp:MultiView ID="mvFindInsuranceFileForClaim" runat="server" ActiveViewIndex="0">
                        <asp:View ID="Genral" runat="server">
                            Policy Number &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            &nbsp;<asp:TextBox ID="txtPolicyNumber" runat="server"></asp:TextBox><br />
                            Cover Note Sheet Number
                            <asp:TextBox ID="txtCoverNoteSheetNumber" runat="server"></asp:TextBox><br />
                            Risk Index &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:TextBox ID="txtRiskIndex" runat="server"></asp:TextBox><br />
                            <strong>Claim Date &nbsp; &nbsp; &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                &nbsp; &nbsp;
                                <asp:TextBox ID="txtClaimDate" runat="server"></asp:TextBox></strong></asp:View>
                        <asp:View ID="Advanced" runat="server">
                            &nbsp;<input id="btnShortName" style="width: 96px" type="button" value="Short Name" onclick="Javascript:LoadWindows('../Lookup screens/Find Party.aspx',600,600)"/>
                            <asp:TextBox ID="txtShortName" runat="server"></asp:TextBox><br />
                            Post Code &nbsp; &nbsp; &nbsp; &nbsp;
                            <asp:TextBox ID="txtPostCode" runat="server"></asp:TextBox><br />
                            In Force From &nbsp;
                            <asp:TextBox ID="txtInForceFrom" runat="server"></asp:TextBox><br />
                            In Force to &nbsp; &nbsp; &nbsp;&nbsp;
                            <asp:TextBox ID="txtInForceTo" runat="server"></asp:TextBox></asp:View>
                    </asp:MultiView></td>
                <td colspan="2" style="width: 80px">
                    <asp:Button ID="btnFind" runat="server" Text="Find Now" />
                    </td>
            </tr>
            <tr>
                <td colspan="3" style="height: 100px">
                <hr />
                    &nbsp;<asp:Panel ID="Panel1" runat="server" Height="300px" ScrollBars="Auto" Width="800px">
                    <asp:GridView ID="gvResult" runat="server" Width="232px" CellPadding="4" ForeColor="#333333" GridLines="None">
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                        <EditRowStyle BackColor="#999999" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" />
                        </Columns>
                    </asp:GridView>
                    </asp:Panel>
                    &nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="3">
                <hr />
                    <asp:Button ID="btnOk" runat="server" Text="Ok" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" /></td>
            </tr>
        </table>--%>
    </form>
</body>
</html>
