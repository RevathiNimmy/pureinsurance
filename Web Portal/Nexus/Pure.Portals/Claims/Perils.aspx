<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Perils.aspx.vb" Inherits="Nexus.Framework_Perils"
    MasterPageFile="~/default.master" %>

<%@ Register Src="~/Controls/Perils.ascx" TagName="Perils" TagPrefix="uc4" %>
<%@ Register TagPrefix="OC" Namespace="Nexus" %>
<%@ Register Src="~/Controls/ClaimsProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/CoInsuranceRecovery.ascx" TagName="CoInsuranceRecovery" TagPrefix="uc5" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="server">
    <div id="Claims_Perils">
        <div class="card">
            <asp:ScriptManager ID="smPerils" runat="server"></asp:ScriptManager>
            <uc3:ProgressBar ID="ucProgressBar" runat="server"></uc3:ProgressBar>
            <OC:ImprovedTabIndex ID="TabIndex" runat="server" CssClass="TabContainer" TabContainerClass="page-progress" ActiveTabClass="ActiveTab" DisabledClass="DisabledTab"></OC:ImprovedTabIndex>
            <div class="card-body clearfix">
                <uc4:Perils ID="perils_ctrl" runat="server"></uc4:Perils>
            </div>
            <uc5:CoInsuranceRecovery ID="CoInsuranceRecovery_ctrl" runat="server"></uc5:CoInsuranceRecovery>
            <div class="card-footer">
                <asp:LinkButton ID="btn_FinishAndPay" runat="server" Text="<%$ Resources:Perils_btn_FinishAndPay %>" CausesValidation="true" OnClick="FinishAndPayButton" OnPreRender="PreRender_FinishAndPay" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btn_Finish" runat="server" Text="<%$ Resources:Perils_btn_Finish %>" CausesValidation="true" OnClick="FinishButton" OnPreRender="PreRenderFinish" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btn_Next" runat="server" Text="<%$ Resources:Perils_btn_Next %>" OnClick="NextButton" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
