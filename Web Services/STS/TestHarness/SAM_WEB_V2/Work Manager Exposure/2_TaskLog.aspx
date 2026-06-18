<%@ Page Language="VB" AutoEventWireup="false" CodeFile="2_TaskLog.aspx.vb" Inherits="Work_Manager_Exposure_3_TaskLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Task Log</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width: 600px">
                <tr>
                    <td colspan="3" style="height: 45px">
                        <strong>
                            <asp:Menu ID="MnuTaskLog" runat="server" BackColor="#F7F6F3" DynamicHorizontalOffset="2"
                                Font-Names="Verdana" Font-Size="0.8em" ForeColor="#7C6F57" Orientation="Horizontal"
                                StaticSubMenuIndent="10px" Style="z-index: 100; left: 24px; position: absolute;
                                top: 24px" Width="368px">
                                <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                                <DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
                                <DynamicMenuStyle BackColor="#F7F6F3" />
                                <DynamicItemTemplate>
                                    <%# Eval("Text") %>
                                </DynamicItemTemplate>
                                <StaticSelectedStyle BackColor="#5D7B9D" />
                                <DynamicSelectedStyle BackColor="#5D7B9D" />
                                <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                                <Items>
                                    <asp:MenuItem Text="Add" Value="0"></asp:MenuItem>
                                    <asp:MenuItem Text="View" Value="1"></asp:MenuItem>
                                </Items>
                                <StaticItemTemplate>
                                    <%# Eval("Text") %>
                                </StaticItemTemplate>
                                <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                            </asp:Menu>
                            <br />
                            <br />
                            Task Log Entries</strong></td>
                </tr>
                <tr>
                    <td colspan="3" style="height: 100px">
                        <hr />
                        <asp:Panel ID="Panel1" runat="server" Height="300px" ScrollBars="Auto" Width="800px">
                            <asp:GridView ID="gvTasksLog" runat="server" CellPadding="4" ForeColor="#333333"
                                GridLines="None" Width="232px">
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
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="height: 100px">
                        <asp:MultiView ID="TaskLogView" runat="server" ActiveViewIndex="0">
                            <asp:View ID="vwAdd" runat="server">
                                <table style="width: 352px" id="VIEWADD">
                                    <tr>
                                        <td colspan="2">
                                            Log Entry</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 118px; height: 24px">
                                            Created By :</td>
                                        <td style="width: 158px; height: 24px">
                                            <asp:Label ID="lblCreatedBy" runat="server" Text="sirius"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 118px; height: 20px">
                                            Date/Time:</td>
                                        <td style="width: 158px; height: 20px">
                                            <asp:TextBox ID="txtDate" runat="server" Width="120px">
                                            </asp:TextBox>
                                            <asp:TextBox ID="txtTime" runat="server" Width="56px">
                                            </asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 118px; height: 26px">
                                            <strong>Entry:</strong></td>
                                        <td colspan="1" style="height: 26px">
                                            <asp:TextBox ID="txtEntry" runat="server" Rows="5" TextMode="MultiLine" Width="480px">
                                            </asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 118px; height: 26px">
                                        </td>
                                        <td colspan="1" style="height: 26px">
                                            <asp:Button ID="btnOk" runat="server" Text="Ok" /><asp:Button ID="btnCancel" runat="server"
                                                Text="Cancel" /></td>
                                    </tr>
                                </table>
                            </asp:View>
                               <asp:View ID="vwView" runat="server">
                                <table style="width: 352px" id="Table1">
                                    <tr>
                                        <td colspan="2">
                                            Log Entry</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 118px; height: 24px">
                                            Created By :</td>
                                        <td style="width: 158px; height: 24px">
                                            <asp:Label ID="Label1" runat="server" Text="sirius"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 118px; height: 20px">
                                            Date/Time:</td>
                                        <td style="width: 158px; height: 20px">
                                            <asp:TextBox ID="TextBox1" runat="server" Width="120px">
                                            </asp:TextBox>
                                            <asp:TextBox ID="TextBox2" runat="server" Width="56px">
                                            </asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 118px; height: 26px">
                                            <strong>Entry:</strong></td>
                                        <td colspan="1" style="height: 26px">
                                            <asp:TextBox ID="TextBox3" runat="server" Rows="5" TextMode="MultiLine" Width="480px">
                                            </asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 118px; height: 26px">
                                        </td>
                                        <td colspan="1" style="height: 26px">
                                            <asp:Button ID="Button1" runat="server" Text="Ok" /><asp:Button ID="Button2" runat="server"
                                                Text="Cancel" /></td>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <hr />
                        <asp:Button ID="btnViewTask" runat="server" Text="View" />
                        &nbsp;<asp:Button ID="btnAddTask" runat="server" Text="Add" />
                    </td>
                </tr>
                <asp:MultiView ID="MultiView1" runat="server">
                </asp:MultiView></table>
        </div>
    </form>
</body>
</html>
