<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddTask.aspx.vb" Inherits="Secure_AddTask" %>
<%@ Register Src="~/Controls/AddTaskButton.ascx" TagName="AddTask" TagPrefix="uc6" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="secure_AddTask">
    <uc6:AddTask ID="AddTask1" runat="server"></uc6:AddTask>
    </div>
    </form>
</body>
</html>
