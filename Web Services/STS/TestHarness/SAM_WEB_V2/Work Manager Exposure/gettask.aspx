<%@ Page Language="VB" AutoEventWireup="false" CodeFile="gettask.aspx.vb" Inherits="gettask"
   %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 

"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Work Manager Get Task</title>

    <script language="javascript" type="text/javascript">
    
    function LoadWindows(url,width,height)
    {
   
    window.open(url,"","width=" + width+ ",height="+ height+",scrollbars=1")
    
    }
    function CheckRun()
    {
    
       a= confirm("Has the Task been completed")
       //alert(a)
       
        //confirm('Are you sure you want to delete this record?')
        if(a)
        {
          document.getElementById("CurrentRun").value = true
          
        }
        else
        {
          document.getElementById("CurrentRun").value = false
        }
   }
    

    </script>

</head>
<body>
    <form id="form2" runat="server">
        <table style="width: 600px">
            <tr>
                <td colspan="3" style="height: 41px">
                    <strong>Scheduled Tasks</strong></td>
            </tr>
            <tr>
                <td style="width: 648px">
                    <asp:DropDownList ID="ddlStatus" runat="server" Width="96px" AutoPostBack="True">
                        <asp:ListItem>New</asp:ListItem>
                        <asp:ListItem>In Progress</asp:ListItem>
                        <asp:ListItem>InComplete</asp:ListItem>
                        <asp:ListItem>Complete</asp:ListItem>
                        <asp:ListItem Value="All">All</asp:ListItem>
                        <asp:ListItem>(Not Complete)</asp:ListItem>
                    </asp:DropDownList>&nbsp;
                    <asp:DropDownList ID="ddlGroups" runat="server" AutoPostBack="True">
                    </asp:DropDownList>&nbsp;
                    <asp:DropDownList ID="ddlALLUser" runat="server" AutoPostBack="True">
                    </asp:DropDownList>&nbsp;
                    <asp:DropDownList ID="ddlDates" runat="server" AutoPostBack="True">
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
                    </asp:DropDownList>&nbsp;
                    <asp:DropDownList ID="ddlSystem" runat="server" AutoPostBack="True">
                        <asp:ListItem Value="User"></asp:ListItem>
                        <asp:ListItem>System</asp:ListItem>
                        <asp:ListItem Value="All">(All)</asp:ListItem>
                    </asp:DropDownList></td>
                <td colspan="2" style="width: 9px">
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="3" style="height: 87px">
                    <hr />
                    <asp:Panel ID="Panel1" runat="server" Height="300px" ScrollBars="Auto" Width="800px">
                        <asp:GridView ID="gvTasks" runat="server" Width="232px" AutoGenerateColumns="False">
                            <RowStyle Font-Size="Smaller" />
                            <Columns>
                                <asp:CommandField ShowSelectButton="True" />
                                <asp:BoundField DataField="Urgent" HeaderText="Urgent" />
                                <asp:BoundField DataField="TaskStatusKey" HeaderText="TaskStatusKey" />
                                <asp:BoundField DataField="Type" HeaderText="Type" />
                                <asp:BoundField DataField="DueDate" HeaderText="DueDate" />
                                <asp:BoundField DataField="Customer" HeaderText="Customer" />
                                <asp:BoundField DataField="Description" HeaderText="Description" />
                                <asp:BoundField DataField="UserGroupKey" HeaderText="UserGroupKey" />
                                <asp:BoundField DataField="UserKey" HeaderText="UserKey" />
                                <asp:BoundField DataField="TaskInstanceKey" HeaderText="TaskInstanceKey" />
                                <asp:BoundField DataField="UserGroupDescription" HeaderText="UserGroupDescription" />
                                <asp:BoundField DataField="UserGroupCode" HeaderText="UserGroupCode" />
                                <asp:BoundField DataField="UserCode" HeaderText="UserCode" />
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                    <asp:Button ID="btnAddTask" runat="server" Text="Add Task" />
                    <asp:Button ID="btnViewTask" runat="server" Text="View Task" />
                    <asp:Button ID="btnEditTask" runat="server" Text="Edit Task" /><asp:Button ID="btnAssignTask"
                        runat="server" Text="Assign Task" />
                    <asp:Button ID="btnRunTask" runat="server" Text="Run Task" Width="66px" OnClientClick="CheckRun()" />
                    <asp:Button ID="btnDeleteTask" runat="server" OnClientClick="return confirm('Are you sure you want to delete this record?');"
                        CommandName="btnDeleteTask" Text="Delete Task" Width="74px" />
                    <asp:Button ID="btnComplete" runat="server" Text="Complete" />
                    <asp:Button ID="btnIncomplete" runat="server" Text="Incomplete" />
                    <asp:Button ID="btnAssignSingletask" runat="server" Text="AssignSingleTask" /></td>
            </tr>
        </table>
        &nbsp;
        <asp:Label ID="Label1" runat="server" Text="Label" Width="109px" Visible="False"></asp:Label>
        <input type="hidden" name="currentRun" id="CurrentRun" runat="server" />
    </form>
</body>
</html>
