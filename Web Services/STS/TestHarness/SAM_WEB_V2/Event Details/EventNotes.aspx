<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EventNotes.aspx.vb" Inherits="Event_Details_EventNotes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script language ="javascript" type = "text/javascript">
    function ClosePage()
    {
    window.opener.__doPostBack("","")
    self.close()
        }
    
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Event Notes<br />
        <asp:Button ID="btnAddText" runat="server" Text="Add Text" /><br />
        <asp:ListBox ID="lstEventText" runat="server" Height="219px" Width="319px"></asp:ListBox>
        <br />
        <asp:TextBox ID="txtEventText" runat="server" Height="218px" Visible="False" Width="312px"></asp:TextBox><br />
        <asp:Button ID="btnAddEventNote" runat="server" Text="Ok" />
        <asp:Button ID="Btncancel" runat="server" OnClientClick="ClosePage()" Text="Cancel" /></div>
    </form>
</body>
</html>
