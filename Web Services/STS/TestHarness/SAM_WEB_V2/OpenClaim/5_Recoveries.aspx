<%@ Page Language="VB" AutoEventWireup="false" CodeFile="5_Recoveries.aspx.vb" Inherits="OpenClaim_Recoveries" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Recoveries</title>
      <script language="javascript" type="text/javascript">
    function LoadWindows(url,width,height)
    {
    window.open(url,"","width=" + width+ ",height="+ height+",scrollbars=1")
    
    }
    
    </script>
    <link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
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
                ForeColor="#C000C0" Text="Salvage and Third Party Recoveries" Width="256px"></asp:Label><br />
            <br />
            <asp:RadioButtonList ID="rblSalvageTPRecovery" runat="server" AutoPostBack="True"
                RepeatDirection="Horizontal" Width="320px">
                <asp:ListItem Selected="True" Value="1">Salvage</asp:ListItem>
                <asp:ListItem Value="0">Third Party</asp:ListItem>
            </asp:RadioButtonList><br />
            <strong>Perils</strong><br />
            <asp:GridView ID="gvPerils" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns ="false" >
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                    <asp:BoundField DataField="BaseClaimPerilKey" HeaderText="BaseClaimPerilKey" />
                    <asp:BoundField DataField="PerilTypeCode" HeaderText="Peril" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                </Columns>
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>
            &nbsp;<br />
            <br />
            <br />
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
            <br />
            <br />
            <asp:GridView ID="gvRecoveries" runat="server" CellPadding="4" ForeColor="#333333"
                GridLines="None" AutoGenerateColumns="False">
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:CommandField ShowEditButton="True" />
                    <asp:BoundField DataField="Description" HeaderText="Description" ReadOnly="True" />
                    <asp:BoundField DataField="InitialReserve" HeaderText="Initial Reserve" ReadOnly="True" />
                    <asp:BoundField DataField="RevisionAmount" HeaderText="Revision Amount" ReadOnly="True" />
                    <asp:BoundField DataField="ThisRevision" HeaderText="This Revision" />
                    <asp:BoundField DataField="TotalReserve" HeaderText="Total Reserve" ReadOnly="True" />
                </Columns>
            </asp:GridView>
            <br />
            <asp:Button ID="btnAddRecovery" runat="server" Text="Add Recovery" />
                <asp:Label ID="lblErrorMessage" runat="server" Width="272px"></asp:Label><br />
            <asp:Panel ID="pnlAddRecovery" runat="server" Height="72px" Visible="False" Width="392px">
                <br />
                Recovery: &nbsp; &nbsp; &nbsp;
                <asp:DropDownList ID="ddlRecoveries" runat="server" Width="144px">
                </asp:DropDownList><br />
                Party Type: &nbsp; &nbsp;
                <asp:DropDownList ID="ddlRecoveryPartyType" runat="server" Width="144px" AutoPostBack="True">
                    <asp:ListItem Value="">(None)</asp:ListItem>
                    <asp:ListItem Value="AG">Agent</asp:ListItem>
                    <asp:ListItem Value="CL">Client</asp:ListItem>
                    <asp:ListItem Value="IN">Insurer</asp:ListItem>
                    <asp:ListItem Value="OT">Other Party</asp:ListItem>
                </asp:DropDownList><br />
                <br />
                Party: &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                <asp:TextBox ID="txtParty" runat="server"></asp:TextBox>&nbsp;
                <asp:Button ID="btnShortName" runat ="server" CausesValidation="False" Text="..." UseSubmitBehavior="False" /><br />
                <br />
                <br />
                Initial Reserve:<asp:TextBox ID="txtInitialReserve" runat="server"></asp:TextBox>
                <br />
                <asp:Button ID="btnAddRecoverylOk" runat="server" Text="Ok" Width="64px" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" /><br />
                <br />
                </asp:Panel>
      <%--  </div>--%>
        <hr />
        Duplicate Claim Check Override
        <br />
        <asp:CheckBox ID="chkOverrideCLaim" runat="server" AutoPostBack="True" Text="Override Claim" />
        <br />
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblUser" runat="server" Text="User" Font-Bold="True" Visible="False"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtUserName" runat="server" Visible="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPassword" runat="server" Text="Password" Font-Bold="True" Visible="False" />
                </td>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" Visible="False" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <asp:Button ID="btnOk" runat="server" Text="Ok" />
        <br />
        <br />
        <hr />
        <br />
                    </td>
                </tr>
            </table>
            <%--<asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
                ForeColor="#C000C0" Text="Salvage and Third Party Recoveries" Width="256px"></asp:Label><br />
            <br />
            <asp:RadioButtonList ID="rblSalvageTPRecovery" runat="server" AutoPostBack="True"
                RepeatDirection="Horizontal" Width="320px">
                <asp:ListItem Selected="True" Value="1">Salvage</asp:ListItem>
                <asp:ListItem Value="0">Third Party</asp:ListItem>
            </asp:RadioButtonList><br />
            <strong>Perils</strong><br />
            <asp:GridView ID="gvPerils" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                SelectedIndex="0" AutoGenerateColumns ="false" >
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                    <asp:BoundField DataField="BaseClaimPerilKey" HeaderText="BaseClaimPerilKey" />
                    <asp:BoundField DataField="PerilTypeCode" HeaderText="Peril" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                </Columns>
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>
            &nbsp;<br />
            <br />
            <br />
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
            <br />
            <br />
            <asp:GridView ID="gvRecoveries" runat="server" CellPadding="4" ForeColor="#333333"
                GridLines="None" AutoGenerateColumns="False">
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:CommandField ShowEditButton="True" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField DataField="InitialReserve" HeaderText="Initial Reserve" />
                    <asp:BoundField DataField="RevisionAmount" HeaderText="Revision Amount" />
                    <asp:BoundField DataField="ThisRevision" HeaderText="This Revision" />
                    <asp:BoundField DataField="TotalReserve" HeaderText="Total Reserve" />
                </Columns>
            </asp:GridView>
            <br />
            <asp:Button ID="btnAddRecovery" runat="server" Text="Add Recovery" />
                <asp:Label ID="lblErrorMessage" runat="server" Width="272px"></asp:Label><br />
            <asp:Panel ID="pnlAddRecovery" runat="server" Height="72px" Visible="False" Width="392px">
                <br />
                Recovery: &nbsp; &nbsp; &nbsp;
                <asp:DropDownList ID="ddlRecoveries" runat="server" Width="144px">
                </asp:DropDownList><br />
                Initial Reserve:<asp:TextBox ID="txtInitialReserve" runat="server"></asp:TextBox>
                <br />
                <asp:Button ID="btnAddRecoverylOk" runat="server" Text="Ok" Width="64px" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" /><br />
                <br />
                </asp:Panel>
        </div>
        <hr />
        Duplicate Claim Check Override
        <br />
        <asp:CheckBox ID="chkOverrideCLaim" runat="server" AutoPostBack="True" Text="Override Claim" />
        <br />
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblUser" runat="server" Text="User" Font-Bold="True" Visible="False"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtUserName" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPassword" runat="server" Text="Password" Font-Bold="True" Visible="False" />
                </td>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <asp:Button ID="btnOk" runat="server" Text="Ok" />
        <br />
        <br />
        <hr />
        <br />--%>
        </div>
    </form>
</body>
</html>
