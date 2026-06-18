<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="Commission.aspx.vb" Inherits="Nexus.Modal_Commission" Title="Premium and Commission by Policy Section" %>

<%@ Register Src="~/Controls/Commission.ascx" TagName="Commission" TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <script language="javascript" type="text/javascript">
        function CloseCommission() {
            tb_remove();
        }
    </script>
    <div id="Modal_Commission">
        <uc1:Commission ID="Commission" ShowEditLinks="false" cssclass="submit" runat="server"></uc1:Commission>
    </div>
</asp:Content>

