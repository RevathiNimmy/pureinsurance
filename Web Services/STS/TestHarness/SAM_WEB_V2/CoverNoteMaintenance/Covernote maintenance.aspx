<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Covernote maintenance.aspx.vb" Inherits="Covernote_mainteneace" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
     <script language="javascript" type ="text/javascript">
     function passresult()
    {  
    
    self.close();

   }


    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br />
        <span style="background-color: #006699"><span style="color: #ffffff"> cover
            note wook has created sheet records<br />
            <br />
            </span>
        </span><span style="color: #ffffff; background-color: #006699">Proceed?</span><br />
        <br />
        <asp:Button ID="btnyes" runat="server" Text="Yes" />&nbsp;<asp:Button ID="BtnNo"
            runat="server" Text="No" OnClientClick = "passresult()"  /></div>
    </form>
</body>
</html>
