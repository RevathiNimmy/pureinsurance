<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="ClaimReinsurance.aspx.vb" Inherits="Nexus.Claims_ClaimReinsurance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="OC" Namespace="Nexus" %>
<%@ Register Src="~/Controls/ClaimsProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/ClaimReinsurance.ascx" TagName="RI" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/ClaimReinsurance2007.ascx" TagName="RI2007" TagPrefix="uc5" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="server">
    <asp:ScriptManager ID="smClaimReinsurance" runat="server"></asp:ScriptManager>
    <div id="Claims_ClaimReinsurance">
        <uc3:ProgressBar ID="ucProgressBar" runat="server"></uc3:ProgressBar>
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal runat="server" Text="<%$ Resources:lbl_PageHeading %>" ID="ltPageHeading"></asp:Literal>
                </h1>
            </div>
            <div class="card-body clearfix">
                <uc4:RI ID="ctrl_RI" runat="server" Visible="false"></uc4:RI>
                <uc5:RI2007 ID="ctrl_RI2007" runat="server" Visible="false"></uc5:RI2007>
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
