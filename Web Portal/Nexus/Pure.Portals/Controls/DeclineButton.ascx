<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DeclineButton.ascx.vb"
    Inherits="Nexus.Controls_DeclineButton" EnableViewState="false" %>

<script type="text/javascript" language="javascript">
    function validateAndClose(element) {

        var reason = $('#<%=txtReason.ClientID %>').val();
        if (reason == '') {
            alert('Please enter reason for decline'); return false;
        }
        else {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm && !prm._postBackSettings) {
                prm._postBackSettings = prm._createPostBackSettings(false, null, null);
            };
            setTimeout('__doPostBack(\'' + element.id + '\',\'\')', 5000);
            tb_remove();
            return true;
        }
    }
    function clearAndClose() {
        document.getElementById('<%=txtReason.ClientId %>').value = '';
        tb_remove();
        return false;
    }    
</script>

<div id="Controls_DeclineButton">
    <input id="_btnDecline" runat="server" class="thickbox" value="Decline" type="button" causesvalidation="false" alt="#TB_inline?height=200&width=800&inlineId=ReasonContainer">
    <div id="ReasonContainer" style="display: none" title=" Decline Reasons ">
        <div class="fieldset-wrapper">
            
            <fieldset>
                <h2>
                    <asp:Label ID="lblCaption" runat="server" Text="<%$ Resources:lbl_Reason %>"></asp:Label>
                </h2>
                <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine"></asp:TextBox>
            </fieldset>
            
        </div>
        <div class="card-footer">
            <asp:LinkButton ID="btnBack" runat="server" Text="<%$ Resources:btnBack %>" OnClientClick="return clearAndClose();" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
            <asp:LinkButton ID="btnSave" runat="server" Text="<%$ Resources:btnSave %>" OnClientClick="return validateAndClose(this);" CausesValidation="false" SkinID="btnPrimary"></asp:LinkButton>
        </div>
    </div>
</div>
