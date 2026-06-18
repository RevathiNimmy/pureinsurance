<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ClientSummary.ascx.vb"
    Inherits="Nexus.Controls_ClientSummary" EnableViewState="false" %>
<div id="Controls_ClientSummary">

    <div class="form-horizontal">
        <legend>
            <asp:Label ID="lbl_ClientSummary_header" runat="server" Text="<%$ Resources:lbl_ClientSummary_header %>" EnableViewState="false"></asp:Label></legend>
        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
            <asp:Label ID="lblAccountBalanceTitle" AssociatedControlID="lblAccountBalance" CssClass="col-md-4 col-sm-3 control-label" runat="server"
                Text="<%$ Resources:lit_AccountBalanceTitle %>" />
            <div class="col-md-8 col-sm-9">
                <p class="form-control-static font-bold">
                    <asp:Label ID="lblAccountBalance" runat="server" />
                </p>
            </div>
        </div>
        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
            <asp:Label ID="lblNoofOpenClaimsTitle" AssociatedControlID="lblNoofOpenClaims" CssClass="col-md-4 col-sm-3 control-label" runat="server"
                Text="<%$ Resources:lbl_NoofOpenClaimsTitle %>" />
            <div class="col-md-8 col-sm-9">
                <p class="form-control-static font-bold">
                    <asp:Label ID="lblNoofOpenClaims" runat="server" />
                </p>
            </div>
        </div>
        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">

            <asp:Label ID="lblNoofPoliciesTitle" AssociatedControlID="lblNoofPolicies" CssClass="col-md-4 col-sm-3 control-label" runat="server"
                Text="<%$ Resources:lbl_NoofPoliciesTitle %>" />
            <div class="col-md-8 col-sm-9">
                <p class="form-control-static font-bold">
                    <asp:Label ID="lblNoofPolicies" runat="server" />
                </p>
            </div>
        </div>
        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
            <asp:Label ID="lblNoofClosedClaimsTitle" AssociatedControlID="lblNoofClosedClaims" CssClass="col-md-4 col-sm-3 control-label"
                runat="server" Text="<%$ Resources:lbl_NoofClosedClaimsTitle %>" />
            <div class="col-md-8 col-sm-9">
                <p class="form-control-static font-bold">
                    <asp:Label ID="lblNoofClosedClaims" runat="server" />
                </p>
            </div>
        </div>
        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
            <asp:Label ID="lblAssociatedClientSummaryTitle"
                runat="server" Text="<%$ Resources:lbl_AssociatedClientSummaryTitle %>" CssClass="lblAssociatesTheme" Visible="false" />
        </div>
    </div>
</div>
