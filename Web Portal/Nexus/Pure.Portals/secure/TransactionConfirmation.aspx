<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="TransactionConfirmation.aspx.vb" Inherits="Nexus.TransactionConfirmation" %>

<%@ Register Src="~/Controls/RiskData.ascx" TagName="TransactionConfirmation" TagPrefix="uc1" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/Document.ascx" TagName="Document" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/DocumentManager.ascx" TagName="documentmanager" TagPrefix="uc5" %>
<asp:Content ID="cntMainBody" runat="server" ContentPlaceHolderID="cntMainBody">
<script language="javascript" type="text/javascript">
    
        setTimeout(function() {
            bajb_backdetect.OnBack = function() {
                if (getQuerystring('dosearch') != "true") {
                    alert("Browser back button is not allowed.");
                    $.blockUI({ message: null });
                    var currentURL = location.toString();
                    window.history.back = function() { document.location = currentURL }
                    $.unblockUI();
                }
            }
        }, 200)

        function getQuerystring(key, default_) {            
            if (default_ == null) default_ = "";
            key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
            var qs = regex.exec(window.location.href);
            if (qs == null)
                return default_;
            else
                return qs[1];
        }

</script>    
    <div id="secure_TransactionConfirmation">
        <uc3:ProgressBar ID="ucProgressBar" runat="server"></uc3:ProgressBar>
        <div class="grey-200">
            <div class="text-center">
                <h1 class="text-shadow no-margin text-4x p-v-lg">
                    <span class="text-xl font-bold text-success m-t-lg block">
                        <asp:Label ID="lblTransactionHeading" runat="server" Text="<%$ Resources:lbl_Transaction_heading %>" />
                    </span>
                </h1>
                <h4 class="h4 m-v-lg TrnsCmplt">
                    <asp:Literal ID="lblTransactionText" runat="server" />
                    <asp:Literal ID="LblOrderID" runat="server" />
                    <asp:Literal ID="lblMTAPremiumReturn" runat="server" />
                    <asp:Literal ID="lblMTAReRunRenewal" runat="server" Mode="Transform" />
                               
                </h4>
                <p class="h4 m-v-lg text-u-c font-bold">
                    <uc1:TransactionConfirmation ID="TransactionConfirmation" runat="server" />
              
            </div>
        </div>
    </div>
</asp:Content>
