<%@ Page Language="VB" AutoEventWireup="false" CodeFile="5_Recoveries.aspx.vb" Inherits="OpenClaim_Recoveries" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Recoveries</title>
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
                  <div>
        <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
            ForeColor="#C000C0" Text="Salvage and Third Party Recoveries" Width="256px"></asp:Label><br />
        <br />
        <br />
        <strong>Perils</strong><br />
        <asp:GridView ID="gvPerils" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
            </Columns>
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <EditRowStyle BackColor="#999999" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        &nbsp;&nbsp;<br />
        <asp:RadioButtonList ID="rblSalvageTPRecovery" runat="server" AutoPostBack="True"
            RepeatDirection="Horizontal" Width="320px">
            <asp:ListItem Selected="True" Value="1">Salvage</asp:ListItem>
            <asp:ListItem Value="0">Third Party</asp:ListItem>
        </asp:RadioButtonList><br />
        <br />
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;<br />
        <asp:GridView ID="gvRecoveries" runat="server" AutoGenerateColumns="False" CellPadding="4"
            ForeColor="#333333" GridLines="None">
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
        <br />
        <asp:Button ID="btnAddRecovery" runat="server" Text="Add Recovery" /><br />
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
            <asp:Label ID="lblErrorMessage" runat="server" Width="272px"></asp:Label></asp:Panel>
    
    </div>
    <hr />
        <asp:Button ID="btnOk" runat="server" Text="Ok" />     
                      
                    </td>
                </tr>
            </table>
    </div>
   <%-- <div>
        <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
            ForeColor="#C000C0" Text="Salvage and Third Party Recoveries" Width="256px"></asp:Label><br />
        <br />
        <br />
        <strong>Perils</strong><br />
        <asp:GridView ID="gvPerils" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
            </Columns>
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <EditRowStyle BackColor="#999999" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        &nbsp;&nbsp;<br />
        <asp:RadioButtonList ID="rblSalvageTPRecovery" runat="server" AutoPostBack="True"
            RepeatDirection="Horizontal" Width="320px">
            <asp:ListItem Selected="True" Value="1">Salvage</asp:ListItem>
            <asp:ListItem Value="0">Third Party</asp:ListItem>
        </asp:RadioButtonList><br />
        <br />
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;<br />
        <asp:GridView ID="gvRecoveries" runat="server" AutoGenerateColumns="False" CellPadding="4"
            ForeColor="#333333" GridLines="None">
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
        <br />
        <asp:Button ID="btnAddRecovery" runat="server" Text="Add Recovery" /><br />
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
            <asp:Label ID="lblErrorMessage" runat="server" Width="272px"></asp:Label></asp:Panel>
    
    </div>
    <hr />
        <asp:Button ID="btnOk" runat="server" Text="Ok" />--%>
       
    </form>
</body>
</html>
