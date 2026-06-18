<%@ Page Language="VB" AutoEventWireup="false" CodeFile="4_Perils.aspx.vb" Inherits="OpenClaim_Peril" %>

<%@ Register Src="../UserControl/Header.ascx" TagName="Header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Perils and Reserves</title>
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
            ForeColor="#C000C0" Text="Perils" Width="73px"></asp:Label><br />
        <table>
            <tr>
                <td style="width: 567px">
                    <asp:Menu ID="Menu1" runat="server" BackColor="#F7F6F3" DynamicHorizontalOffset="2"
                        Font-Names="Verdana" Font-Size="0.8em" ForeColor="#7C6F57" Orientation="Horizontal"
                        StaticSubMenuIndent="10px" Width="100%">
                        <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                        <DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
                        <DynamicMenuStyle BackColor="#F7F6F3" />
                        <StaticSelectedStyle BackColor="#5D7B9D" />
                        <DynamicSelectedStyle BackColor="#5D7B9D" />
                        <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                        <Items>
                            <asp:MenuItem Text="Perils" Value="0"></asp:MenuItem>
                            <asp:MenuItem Text="Genral Details" Value="1"></asp:MenuItem>
                        </Items>
                        <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                    </asp:Menu>
                </td>
            </tr>
            <tr>
                <td style="width: 567px">
                    <asp:MultiView ID="MvPerils" runat="server" ActiveViewIndex="0">
                        <asp:View ID="Perils" runat="server">
                            <asp:GridView ID="gvPerils" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                                Width="100%">
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" />
                                </Columns>
                                <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                                <EditRowStyle BackColor="#999999" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="Smaller" ForeColor="White" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                            <asp:Button ID="btnAddPeril" runat="server" Text="Add Peril" /><br />
                            <br />
                            <asp:Panel ID="pnlAddPeril" runat="server" Height="72px" Visible="False" Width="240px">
                                <br />
                                Peril: &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                <asp:DropDownList ID="ddlperils" runat="server" Width="144px">
                                </asp:DropDownList><br />
                                Description:<asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
                                <asp:Button ID="btnPerilOk" runat="server" Text="Ok" Width="64px" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" /></asp:Panel>
                            <br />
                            <br />
                            <asp:Label ID="lblReserves" runat="server" Text="Reserves"></asp:Label>
                            <br />
                            <br />
                            <asp:GridView ID="gvReserves" runat="server" CellPadding="4" ForeColor="#333333"
                                GridLines="None" Width="100%" AutoGenerateColumns="False">
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <Columns>
                                    <asp:CommandField ShowEditButton="True" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" ReadOnly="True" />
                                    <asp:BoundField DataField="InitialReserve" HeaderText="Initial Reserve" />
                                    <asp:BoundField DataField="RevisionAmount" HeaderText="Revision Amount" ReadOnly="True" />
                                    <asp:BoundField DataField="ThisRevision" HeaderText="This Revision" ReadOnly="True" />
                                    <asp:BoundField DataField="CurrentReserve" HeaderText="Current Reserve" ReadOnly="True" />
                                    <asp:BoundField DataField="Incurred" HeaderText="Incurred" ReadOnly="True" />
                                    <asp:BoundField DataField="Average" HeaderText="Average" ReadOnly="True" />
                                </Columns>
                                <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                                <EditRowStyle BackColor="#999999" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="Smaller" ForeColor="White" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                        </asp:View>
                        <asp:View ID="GenralDetails" runat="server">
                        </asp:View>
                    </asp:MultiView></td>
            </tr>
            <tr>
                <td style="width: 100%">
                <%--<hr />--%>
                    <asp:Button ID="btnOk" runat="server" Text="Ok" /></td>
            </tr>
        </table>
                   </td>
                </tr>
            </table>
    
    
    
    
    
    
       <%-- <asp:Label ID="Label1" runat="server" BorderStyle="Double" Font-Bold="True" Font-Italic="True"
            ForeColor="#C000C0" Text="Perils" Width="73px"></asp:Label><br />
        <table>
            <tr>
                <td style="width: 567px">
                    <asp:Menu ID="Menu1" runat="server" BackColor="#F7F6F3" DynamicHorizontalOffset="2"
                        Font-Names="Verdana" Font-Size="0.8em" ForeColor="#7C6F57" Orientation="Horizontal"
                        StaticSubMenuIndent="10px" Width="100%">
                        <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                        <DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
                        <DynamicMenuStyle BackColor="#F7F6F3" />
                        <StaticSelectedStyle BackColor="#5D7B9D" />
                        <DynamicSelectedStyle BackColor="#5D7B9D" />
                        <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                        <Items>
                            <asp:MenuItem Text="Perils" Value="0"></asp:MenuItem>
                            <asp:MenuItem Text="Genral Details" Value="1"></asp:MenuItem>
                        </Items>
                        <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                    </asp:Menu>
                </td>
            </tr>
            <tr>
                <td style="width: 567px">
                    <asp:MultiView ID="MvPerils" runat="server" ActiveViewIndex="0">
                        <asp:View ID="Perils" runat="server">
                            <asp:GridView ID="gvPerils" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                                Width="100%">
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <Columns>
                                    <asp:CommandField ShowSelectButton="True" />
                                </Columns>
                                <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                                <EditRowStyle BackColor="#999999" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="Smaller" ForeColor="White" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                            <asp:Button ID="btnAddPeril" runat="server" Text="Add Peril" /><br />
                            <br />
                            <asp:Panel ID="pnlAddPeril" runat="server" Height="72px" Visible="False" Width="240px">
                                <br />
                                Peril: &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                <asp:DropDownList ID="ddlperils" runat="server" Width="144px">
                                </asp:DropDownList><br />
                                Description:<asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
                                <asp:Button ID="btnPerilOk" runat="server" Text="Ok" Width="64px" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" /></asp:Panel>
                            <br />
                            <br />
                            Reserves<br />
                            <br />
                            <asp:GridView ID="gvReserves" runat="server" CellPadding="4" ForeColor="#333333"
                                GridLines="None" Width="100%" AutoGenerateColumns="False">
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <Columns>
                                    <asp:CommandField ShowEditButton="True" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                    <asp:BoundField DataField="InitialReserve" HeaderText="Initial Reserve" />
                                    <asp:BoundField DataField="RevisionAmount" HeaderText="Revision Amount" />
                                    <asp:BoundField DataField="ThisRevision" HeaderText="This Revision" />
                                    <asp:BoundField DataField="CurrentReserve" HeaderText="Current Reserve" />
                                    <asp:BoundField DataField="Incurred" HeaderText="Incurred" />
                                    <asp:BoundField DataField="Average" HeaderText="Average" />
                                </Columns>
                                <RowStyle BackColor="#F7F6F3" Font-Size="Smaller" ForeColor="#333333" />
                                <EditRowStyle BackColor="#999999" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="Smaller" ForeColor="White" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                        </asp:View>
                        <asp:View ID="GenralDetails" runat="server">
                        </asp:View>
                    </asp:MultiView></td>
            </tr>
            <tr>
                <td style="width: 567px">
                <hr />
                    <asp:Button ID="btnOk" runat="server" Text="Ok" /></td>
            </tr>
        </table>--%>
    
    </div>
    </form>
</body>
</html>
