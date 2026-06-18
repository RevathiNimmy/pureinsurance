<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ClaimHeader.ascx.vb"
    Inherits="Nexus.Controls_ClaimHeader" EnableViewState="false" %>
<div id="Controls_ClaimHeader">
    <div class="card">
        <div class="card-body clearfix">
            
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblHeading" runat="server" Text="<%$ Resources:lbl_Claim_Header %>"></asp:Label></legend>
                
                
            
                    <div id="liAgent" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAgentNameTitle" runat="server" AssociatedControlID="lblAgentName" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal runat="server" ID="ltAgentNameTitle" Text="<%$ Resources:lbl_AgentName %>"></asp:Literal></asp:Label>
                        <asp:Label ID="lblAgentName" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:HyperLink ID="hypAgentDetails" NavigateUrl="~/Modal/AgentDetailsForClaims.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=700" CssClass="thickbox" Text="<%$ Resources:hyp_Details %>" runat="server" Visible="true"></asp:HyperLink>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPolicyNumberTitle" runat="server" AssociatedControlID="lblPolicyNumber" Text="<%$ Resources:lbl_PolicyNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Label ID="lblPolicyNumber" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:HyperLink ID="hypPolicyNumber" NavigateUrl="~/Modal/PolicySummary.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=700" CssClass="thickbox" Text="<%$ Resources:hyp_Details %>" runat="server" Visible="true"></asp:HyperLink>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCurrencyTitle" runat="server" AssociatedControlID="lblCurrency" Text="<%$ Resources:lbl_Currency %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Label ID="lblCurrency" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblDatesTitle" runat="server" AssociatedControlID="lblDates" Text="<%$ Resources:lbl_Dates %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Label ID="lblDates" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblInsuredNameTitle" runat="server" AssociatedControlID="lblInsuredName" Text="<%$ Resources:lbl_InsuredName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Label ID="lblInsuredName" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:HyperLink ID="hypInsured" NavigateUrl="~/Modal/ClientDetailsForClaims.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=700" CssClass="thickbox" Text="<%$ Resources:hyp_Details %>" runat="server" Visible="true"></asp:HyperLink>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblStatusTitle" runat="server" AssociatedControlID="lblStatus" Text="<%$ Resources:lbl_Status %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Label ID="lblStatus" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClaimNumberTitle" runat="server" AssociatedControlID="lblClaimNumber" Text="<%$ Resources:lbl_ClaimNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Label ID="lblClaimNumber" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    </div>
                    <div id="liRisk" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRiskTitle" runat="server" AssociatedControlID="lblRisk" Text="Risk" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Label ID="lblRisk" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:LinkButton ID="hypRisk" Text="<%$ Resources:hyp_Details %>" runat="server" Visible="false"></asp:LinkButton>
                    </div>
                
                    <div id="liFinancialDetails" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:HyperLink ID="hypFinancialdetails" Text="<%$ Resources:hyp_Financial_Details %>" NavigateUrl="~/Claims/FinancialDetails.aspx?modal=true" CssClass="thickbox" runat="server"></asp:HyperLink>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:HyperLink ID="hypEvents" Text="<%$ Resources:hyp_Events %>" NavigateUrl="~/secure/EventList.aspx?modal=true&ReturnUrl=Claim" CssClass="thickbox" runat="server"></asp:HyperLink>

                    </div>
                </div>
        </div>
        
    </div>
</div>
