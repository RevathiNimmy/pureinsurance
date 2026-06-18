<%@ Page Language="VB" AutoEventWireup="false" CodeFile="1_GetTasks.aspx.vb" Inherits="Work_Manager_Exposure_1_GetTasks" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Get Task </title>
    <script language="javascript"  type ="text/javascript">
    
    function LoadWindows(url,width,height)
    {
    window.open(url,"","width=" + width+ ",height="+ height+",scrollbars=1")
    
    }
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
            <table style="width: 600px">
            <tr>
                <td colspan="3" style="height: 41px">
                    <strong>Scheduled Tasks</strong></td>
            </tr>
            <tr>
                <td style="width: 648px">                    
                    <asp:DropDownList ID="ddlStatus" runat="server" Width="96px">
                        <asp:ListItem Value="All">All</asp:ListItem>
                        <asp:ListItem>New</asp:ListItem>
                        <asp:ListItem>In Progress</asp:ListItem>
                        <asp:ListItem>Complete</asp:ListItem>
                        <asp:ListItem>InComplete</asp:ListItem>
                        <asp:ListItem>(Not Complete)</asp:ListItem>
                    </asp:DropDownList>&nbsp;
                    <asp:DropDownList ID="ddlUserGroup" runat="server">
                    </asp:DropDownList>&nbsp;
                    <asp:DropDownList ID="ddlUser" runat="server">
                    </asp:DropDownList>&nbsp;
                    <asp:DropDownList ID="ddlDates" runat="server">
                        <asp:ListItem>(All Dates)</asp:ListItem>
                        <asp:ListItem>Today</asp:ListItem>
                        <asp:ListItem>Tomorrow</asp:ListItem>
                        <asp:ListItem>Next 2 Days</asp:ListItem>
                        <asp:ListItem>Next 3 Days</asp:ListItem>
                        <asp:ListItem>Next 4 Days</asp:ListItem>
                        <asp:ListItem>Next 5 Days</asp:ListItem>
                        <asp:ListItem>Next 6 Days</asp:ListItem>
                        <asp:ListItem>Next 7 Days</asp:ListItem>
                        <asp:ListItem>Next 14 Days</asp:ListItem>
                        <asp:ListItem>Next 28 Days</asp:ListItem>
                    </asp:DropDownList>&nbsp;<asp:DropDownList ID="DropDownList5" runat="server">
                        <asp:ListItem>(All)</asp:ListItem>
                        <asp:ListItem>User</asp:ListItem>
                        <asp:ListItem>System</asp:ListItem>
                    </asp:DropDownList></td>
                <td colspan="2" style="width: 80px">
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="3" style="height: 100px">
                <hr />
                    &nbsp;<asp:Panel ID="Panel1" runat="server" Height="300px" ScrollBars="Auto" Width="800px">
                    <asp:GridView ID="gvTasks" runat="server" Width="232px" CellPadding="4" ForeColor="#333333" GridLines="None">
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
                <td colspan="3">
                <hr />
                    &nbsp;<asp:Button ID="btnAddTask" runat="server" Text="Add Task" OnClientClick="LoadWindows('../Work Manager Exposure/2_RunTask.aspx',600,600)" />
                    <asp:Button ID="btnViewTask" runat="server" Text="View Task"  OnClientClick="LoadWindows('../Work Manager Exposure/2_ViewTask.aspx',600,600)"/>
                    <asp:Button ID="btnEditTask" runat="server" Text="Edit Task" OnClientClick="LoadWindows('../Work Manager Exposure/2_EditTask.aspx',800,600)" />
                    <asp:Button ID="btnAssignTask" runat="server" Text="Assign Task" OnClientClick="LoadWindows('../Work Manager Exposure/2_AssignTask.aspx',600,600)" />
                    <asp:Button ID="btnRunTask" runat="server" Text="Run Task" OnClientClick="LoadWindows('../Work Manager Exposure/2_ViewTask.aspx',600,600)" />
                    <asp:Button ID="btnDeleteTask" runat="server" Text="Delete Task" OnClientClick="LoadWindows('../Work Manager Exposure/2_ViewTask.aspx',600,600)" />
                    <asp:Button ID="btnNewTask" runat="server" TextStyle="z-index: 100; left: 584px; position: absolute;top: 464px" Text="Schedule New Task" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
