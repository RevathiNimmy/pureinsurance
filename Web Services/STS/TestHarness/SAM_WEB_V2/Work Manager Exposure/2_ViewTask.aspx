-<%@ Page Language="VB" AutoEventWireup="false" CodeFile="2_ViewTask.aspx.vb" Inherits="Work_Manager_Exposure_2_ViewTask" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Task</title>

    <script language="javascript" type="text/javascript">
    
    function LoadWindows(url,width,height)
    {
   
    window.open(url,"","width=" + width+ ",height="+ height+",scrollbars=1")
    
    }


function TABLE1_onclick() {

}

    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
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
                                    <table style="width: 352px" id="TABLE1" onclick="return TABLE1_onclick()">
                                        <tr>
                                            <td colspan="4" align="left">
                                                <strong>Task Details</strong></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 130px; height: 24px" align="right">
                                                Task Group:</td>
                                            <td style="width: 158px; height: 24px">
                                                <asp:DropDownList ID="ddlTaskGroup" runat="server">
                                                </asp:DropDownList></td>
                                            <td style="height: 24px">
                                                Task:</td>
                                            <td style="width: 3px; height: 24px">
                                                <asp:DropDownList ID="ddlTask" runat="server" Width="200px">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 130px; height: 20px" align="right">
                                                <strong>Due Date/Time:</strong></td>
                                            <td>
                                                <asp:DropDownList ID="ddlDueDateTime" runat="server" Width="300px">
                                                </asp:DropDownList></td>
                                            <td style="height: 20px">
                                            </td>
                                            <td style="width: 3px; height: 20px">
                                                <asp:TextBox ID="txtDate" runat="server" Width="192px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 130px" align="right">
                                                <strong>Client:</strong></td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtClient" runat="server" Width="480px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 130px; height: 26px;" align="right">
                                                <strong>Description :</strong></td>
                                            <td colspan="3" style="height: 26px">
                                                <asp:TextBox ID="txtDescription" runat="server" Width="480px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 130px; height: 21px" align="right">
                                                Urgent:</td>
                                            <td colspan="3" style="height: 21px">
                                                <asp:CheckBox ID="chkUrgent" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td align="right" style="width: 130px; height: 21px">
                                                Task Review</td>
                                            <td colspan="3" style="height: 21px">
                                                <asp:CheckBox ID="chkIsTaskReview" runat="server" Height="10px" Enabled="False" /></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 130px" align="right">
                                                Complete:</td>
                                            <td style="width: 158px">
                                                <asp:CheckBox ID="chkComplete" runat="server" />&nbsp;</td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlAllocation" runat="server" Height="96px" HorizontalAlign="Justify"
                                    Width="100%">
                                    <table style="width: 100%">
                                        <tr>
                                            <td colspan="4">
                                                <strong>Allocation</strong></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 99px; height: 29px" align="right">
                                                User Group</td>
                                            <td style="width: 208px; height: 29px">
                                                &nbsp;<asp:DropDownList ID="ddlUserGroup" runat="server">
                                                </asp:DropDownList></td>
                                            <td style="height: 29px" align="right">
                                                User</td>
                                            <td style="height: 29px">
                                                &nbsp;<asp:DropDownList ID="ddlUser" runat="server">
                                                    <asp:ListItem>Sirius</asp:ListItem>
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="height: 11px" align="right">
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Button ID="btnTaskLog" runat="server" Text="Task Log" OnClientClick="LoadWindows('../Work Manager Exposure/WMTaskLog.aspx?disable=YES',600,600)" /></asp:View>
                            <asp:View ID="vAuditDetails" runat="server">
                                <table id="tblAddress" runat="server" style="width: 100%">
                                    <tr>
                                        <td colspan="4" style="height: 26px">
                                            <strong>Audit</strong></td>
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
                    </td>
                </tr>
                <tr>
                    <td colspan="1">
                        &nbsp;<asp:Button ID="btnOk" runat="server" Text="Ok" /></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
