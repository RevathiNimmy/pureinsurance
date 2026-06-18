<%@ Page Language="VB" AutoEventWireup="false" CodeFile="logout.aspx.vb" MasterPageFile="~/default.master" Inherits="Nexus.secure_logout" %>
<asp:Content ID="cntMainBody" runat="server" ContentPlaceHolderID="cntMainBody">
   <script language="javascript" type="text/javascript">
       function logout(sMessage) {
           if (sMessage != '') {
               alert(sMessage);
           }
           location.href = "default.aspx";
       }     
</script>
</asp:Content>