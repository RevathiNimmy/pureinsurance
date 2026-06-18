<%@ Page Language="VB" AutoEventWireup="false" CodeFile="2_AssignTask.aspx.vb" Inherits="Work_Manager_Exposure_2_AssignTask_" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
      <script language="javascript"  type ="text/javascript">
    
    function LoadWindows(url,width,height)
    {
    window.open(url,"","width=" + width+ ",height="+ height+",scrollbars=1")
    
    }
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 536px">
            <tr>
                <td colspan="1">
                    <asp:Menu ID="mnuAddTask" runat="server" BackColor="#F7F6F3" DynamicHorizontalOffset="2"
                        Font-Names="Verdana" Font-Size="0.8em" ForeColor="#7C6F57" Orientation="Horizontal"
                        StaticSubMenuIndent="10px">
                        <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                        <DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
                        <DynamicMenuStyle BackColor="#F7F6F3" />
                        <StaticSelectedStyle BackColor="#5D7B9D" />
                        <DynamicSelectedStyle BackColor="#5D7B9D" />
                        <DynamicMenuItemStyle HorizontalPadding="5px" ItemSpacing="5px" VerticalPadding="2px" />
                        <Items>
                            <asp:MenuItem Text="1 - Task Details" Value="0"></asp:MenuItem>
                            <asp:MenuItem Text="2 - Audit Details" Value="1"></asp:MenuItem>
                        </Items>
                        <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                    </asp:Menu>
                </td>
            </tr>
            <tr>
                <td style="width: 419px">
                    <asp:MultiView ID="mvAddTask" runat="server" ActiveViewIndex="0">
                        <asp:View ID="vTaskDetails" runat="server">
                            <asp:Panel ID="pnlTaskDetails" runat="server">
                                <table style="width: 352px">
                                    <tr>
                                        <td colspan="4">
                                            Task Details
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 130px; height: 24px">
                                            Task Group:</td>
                                        <td style="width: 158px; height: 24px">
                                            <asp:DropDownList ID="DropDownList3" runat="server">
                                                <asp:ListItem>All</asp:ListItem>
                                                <asp:ListItem Value="0">Personal Client</asp:ListItem>
                                                <asp:ListItem Value="3">Corporate Client</asp:ListItem>
                                                <asp:ListItem Value="1">Group Client</asp:ListItem>
                                            </asp:DropDownList></td>
                                        <td style="height: 24px">
                                            Task:</td>
                                        <td style="width: 3px; height: 24px">
                                            <asp:DropDownList ID="DropDownList5" runat="server">
                                                <asp:ListItem>All</asp:ListItem>
                                                <asp:ListItem Value="0">Personal Client</asp:ListItem>
                                                <asp:ListItem Value="3">Corporate Client</asp:ListItem>
                                                <asp:ListItem Value="1">Group Client</asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 130px; height: 20px">
                                            <strong>Due Date/Time:</strong></td>
                                        <td style="width: 158px; height: 20px">
                                            <asp:DropDownList ID="DropDownList4" runat="server">
                                                <asp:ListItem>All</asp:ListItem>
                                                <asp:ListItem Value="0">Personal Client</asp:ListItem>
                                                <asp:ListItem Value="3">Corporate Client</asp:ListItem>
                                                <asp:ListItem Value="1">Group Client</asp:ListItem>
                                            </asp:DropDownList></td>
                                        <td style="height: 20px">
                                        </td>
                                        <td style="width: 3px; height: 20px">
                                            <asp:TextBox ID="TextBox1" runat="server" Width="56px"></asp:TextBox>
                                            <asp:TextBox ID="TextBox2" runat="server" Width="56px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 130px">
                                            <strong>Client:</strong></td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtFileCode" runat="server" Width="480px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 130px">
                                            <strong>Description :</strong></td>
                                        <td colspan="3">
                                            <asp:TextBox ID="TextBox3" runat="server" Width="480px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 130px; height: 21px">
                                            Urgent
                                        </td>
                                        <td colspan="3" style="height: 21px">
                                            <asp:CheckBox ID="CheckBox1" runat="server" />&nbsp;
                                            <asp:CheckBox ID="CheckBox2" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 130px">
                                        </td>
                                        <td style="width: 158px">
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlAllocation" runat="server" Height="96px" HorizontalAlign="Justify"
                                Width="100%">
                                <table style="width: 100%">
                                    <tr>
                                        <td colspan="4">
                                            Allocation</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 99px; height: 29px">
                                            User Group</td>
                                        <td style="width: 208px; height: 29px">
                                            &nbsp;<asp:DropDownList ID="DropDownList1" runat="server">
                                                <asp:ListItem>Can Run all SAM tasks</asp:ListItem>
                                            </asp:DropDownList></td>
                                        <td style="height: 29px">
                                            User</td>
                                        <td style="height: 29px">
                                            &nbsp;<asp:DropDownList ID="DropDownList2" runat="server">
                                                <asp:ListItem>Sirius</asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="height: 11px">
                                            <asp:Button ID="btnTaskLog" runat="server" Text="Task Log" OnClientClick="LoadWindows('../Work Manager Exposure/2_TaskLog.aspx',600,600)"  /></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:View>
                        <asp:View ID="vAuditDetails" runat="server">
                            <table id="tblAddress" runat="server" style="width: 376px">
                                <tr>
                                    <td colspan="4" style="height: 26px">
                                        Audit</td>
                                </tr>
                                <tr>
                                    <td style="width: 141px; height: 26px">
                                        Logged By :</td>
                                    <td style="width: 227px; height: 26px">
                                        <asp:Label ID="lblLoggedBy" runat="server" Text="Label"></asp:Label></td>
                                    <td style="width: 227px; height: 26px">
                                        At :
                                    </td>
                                    <td style="width: 227px; height: 26px">
                                        <asp:Label ID="lblAtLoggedBy" runat="server" Text="Label"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 141px">
                                        Last Modified By :</td>
                                    <td style="width: 227px">
                                        <asp:Label ID="lblLastModifiedBy" runat="server" Text="Label"></asp:Label></td>
                                    <td style="width: 227px">
                                        At :</td>
                                    <td style="width: 227px">
                                        <asp:Label ID="lblAt" runat="server" Text="Label"></asp:Label></td>
                                </tr>
                            </table>
                        </asp:View>
                    </asp:MultiView></td>
            </tr>
            <tr>
                <td colspan="1">
                    <hr />
                    &nbsp; &nbsp;
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    &nbsp;<input id="btnOk" onclick="Javascript:passresult()" type="button" value="Ok" /></td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
