<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WMTaskLog.aspx.vb" Inherits="Work_Manager_Exposure_WMTaskLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        &nbsp;
        <asp:TextBox ID="TextBox1" runat="server" Style="z-index: 100; left: 83px; position: absolute;
            top: 7px"></asp:TextBox>
        &nbsp; &nbsp; &nbsp;&nbsp;
        <asp:Button ID="btnOk" runat="server" Style="left: 246px; position: relative; top: -11px"
            Text="Ok" OnClientClick="self.close()" />
        <asp:GridView ID="GridView1" runat="server" Width="378px">
        </asp:GridView>
    
    </div>
       <b> <asp:Label ID="lblEntry" runat="server" Style="z-index: 102; left: 19px; position: absolute;
            top: 7px" Text="Entry"></asp:Label></b>
        <asp:Button ID="btnAdd" runat="server" Style="z-index: 104; left: 252px; position: absolute;
            top: 4px" Text="Add" />
    </form>
</body>
</html>
