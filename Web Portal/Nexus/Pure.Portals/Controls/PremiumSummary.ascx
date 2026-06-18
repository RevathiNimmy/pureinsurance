<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PremiumSummary.ascx.vb" Inherits="Nexus.Controls_PremiumSummary" EnableViewState="false" %>
    <script type="text/javascript" language="javascript">
        function ConfirmAction(path, msg) {
            if (confirm(msg) == '1') {
                tb_show(null, path , null);
                return false;
            }
            else
                return false;
        }
    </script>
<div id="Controls_PremiumSummary">
<asp:Button ID="btnPremiumSummary" runat="server" CausesValidation="false" Text="<%$ Resources:lbl_PremiumSummary %>"></asp:Button>
</div> 
