<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PerilDetails.aspx.vb" Inherits="Nexus.Claims_PerilDetails"
    MasterPageFile="~/default.master" %>

<%@ Register TagPrefix="OC" Namespace="Nexus" %>
<%@ Register Src="~/Controls/ClaimsProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/ReserveAndRecovery.ascx" TagName="ReserveARecovery"
    TagPrefix="uc4" %>
<%@ Register Src="~/Controls/PayClaim.ascx" TagName="PayClaim" TagPrefix="uc5" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="server">
   <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            //SET TAB DEFAULTS
            if ($('#<%= hfRememberTabs.ClientID %>').val() == "1" || $('#<%= hfRememberTabs.ClientID %>').val() == "3") {
                $('#liReseveAndRecovery a').tab('show');
            }
            if ($('#<%= hfRememberTabs.ClientID %>').val() == "2") {
                $('#liPaymentDetail a').tab('show');
                document.getElementById('<%= hfRememberTabs.ClientId%>').value = 1;
                $('#<%= hfRememberTabs.ClientID %>').val('1');
            }
        });

    </script>
    <div id="Claims_EditReserveItems">
        <uc3:ProgressBar ID="ucProgressBar" runat="server"></uc3:ProgressBar>
        <OC:ImprovedTabIndex ID="TabIndex" runat="server" CssClass="TabContainer" TabContainerClass="page-progress" ActiveTabClass="ActiveTab" DisabledClass="DisabledTab"></OC:ImprovedTabIndex>
        <asp:ScriptManager ID="smPerilDetails" runat="server"></asp:ScriptManager>

        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal runat="server" Text="<%$ Resources:PerilDetails_pageheading %>" ID="ltPageHeading"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="md-whiteframe-z0 bg-white">
                    <ul class="nav nav-lines nav-tabs b-danger">
                        <li id="liReseveAndRecovery"><a href="#tab-ReseveAndRecovery" data-bs-toggle="tab" aria-expanded="true" class="active">
                            <asp:Literal ID="liTabReserveAndRecovery" Text="<%$ Resources:lbl_TabReserveAndRecovery %>" runat="server"></asp:Literal></a></li>
                        <li id="liPaymentDetail"><a href="#tab-PaymentDetails" data-bs-toggle="tab" aria-expanded="true">
                            <asp:Literal ID="liTabPaymentDetails" Text="<%$ Resources:lbl_TabPaymentDetails %>" runat="server"></asp:Literal></a></li>
                    </ul>
                    <div class="tab-content clearfix p b-t b-t-2x">
                        <div id="tab-ReseveAndRecovery" class="tab-pane animated fadeIn active" role="tabpanel">
                            <uc4:ReserveARecovery ID="ReserveARecovery_ctrl" runat="server"></uc4:ReserveARecovery>
                        </div>
                        <div id="tab-PaymentDetails" class="tab-pane animated fadeIn" role="tabpanel">
                            <uc5:PayClaim ID="PayClaim_ctrl" runat="server"></uc5:PayClaim>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btn_Back" runat="server" UseSubmitBehavior="false" Text="<%$ Resources:btn_Back %>" OnClick="BackButton" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnInstalments" runat="server" SkinID="btnPrimary" Text="Instalments" Visible="false" CausesValidation="false"></asp:LinkButton>
                <asp:LinkButton ID="btn_Next" runat="server" UseSubmitBehavior="true" Text="<%$ Resources:btn_Ok %>" OnClick="NextButton" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:CustomValidator ID="IsValidReserve" runat="server" Display="none"></asp:CustomValidator>
        <asp:CustomValidator ID="cvMediaTypeAndDefaultBankAccountForReciept" runat="server" Display="none"></asp:CustomValidator>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
        <asp:HiddenField ID="hfRememberTabs" Value="1" runat="server"></asp:HiddenField>
    </div>
</asp:Content>
