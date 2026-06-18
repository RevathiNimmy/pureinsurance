<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ClaimInfo.ascx.vb" Inherits="Nexus.Controls_ClaimInfo"
    EnableViewState="false" %>

<div id="Controls_ClaimInfo">
    <div class="streamline b-l b-dark m-b">
        <div id="liAgent" runat="server" class="sl-item sl-item-md">
            <div class="sl-icon">
                <i class="fa fa-user-md"></i>
            </div>
            <div class="sl-content">
                <div class="text-dk ">
                    <asp:Label ID="lblAgentNameTitle" runat="server" AssociatedControlID="hypAgentDetails">
                        <asp:Literal runat="server" ID="ltAgentNameTitle" Text="<%$ Resources:lbl_AgentName %>"></asp:Literal></asp:Label>
                    <asp:HyperLink ID="hypAgentDetails" SkinID="lnkHInfo" data="modal" NavigateUrl="~/Modal/AgentDetailsForClaims.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=700" runat="server" Visible="true"></asp:HyperLink>
                </div>
            </div>
        </div>
        <div class="sl-item sl-item-md">
            <div class="sl-icon">
                <i class="glyphicon glyphicon-barcode"></i>
            </div>
            <div class="sl-content">
                <div class="text-dk ">
                    <asp:Label ID="lblPolicyNumberTitle" runat="server" AssociatedControlID="hypPolicyNumber" Text="<%$ Resources:lbl_PolicyNumber %>"></asp:Label>
                    <asp:HyperLink ID="hypPolicyNumber" SkinID="lnkHInfo" data="modal" NavigateUrl="~/Modal/PolicySummary.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=700" runat="server" Visible="true"></asp:HyperLink>
                </div>
            </div>
        </div>
        <div class="sl-item sl-item-md">
            <div class="sl-icon">
                <i class="fa fa-dollar"></i>
            </div>
            <div class="sl-content">
                <div class="text-dk ">
                    <asp:Label ID="lblCurrencyTitle" runat="server" AssociatedControlID="lblCurrency" Text="<%$ Resources:lbl_Currency %>"></asp:Label>
                    <asp:Label ID="lblCurrency" SkinID="badgeInfo" runat="server"></asp:Label>
                </div>
            </div>
        </div>
        <div class="sl-item sl-item-md">
            <div class="sl-icon">
                <i class="glyphicon glyphicon-calendar"></i>
            </div>
            <div class="sl-content">
                <div class="text-dk ">
                    <asp:Label ID="lblDatesTitle" runat="server" AssociatedControlID="lblDates" Text="<%$ Resources:lbl_Dates %>"></asp:Label>
                    <asp:Label ID="lblDates" SkinID="badgeInfo" runat="server"></asp:Label>
                </div>
            </div>
        </div>
        <div class="sl-item sl-item-md">
            <div class="sl-icon">
                <i class="fa fa-user"></i>
            </div>
            <div class="sl-content">
                <div class="text-dk ">
                    <asp:Label ID="lblInsuredNameTitle" runat="server" AssociatedControlID="hypInsured" Text="<%$ Resources:lbl_InsuredName %>"></asp:Label>
                    <asp:HyperLink ID="hypInsured" SkinID="lnkHInfo" data="modal" NavigateUrl="~/Modal/ClientDetailsForClaims.aspx?modal=true&KeepThis=true&TB_iframe=true&height=500&width=700" Text="<%$ Resources:hyp_Details %>" runat="server" Visible="true"></asp:HyperLink>
                </div>
            </div>
        </div>
        <div class="sl-item sl-item-md">
            <div class="sl-icon">
                <i class="glyphicon glyphicon-ok"></i>
            </div>
            <div class="sl-content">
                <div class="text-dk ">
                    <asp:Label ID="lblStatusTitle" runat="server" AssociatedControlID="lblStatus" Text="<%$ Resources:lbl_Status %>"></asp:Label>
                    <asp:Label ID="lblStatus" SkinID="badgeInfo" runat="server"></asp:Label>
                </div>
            </div>
        </div>
        <div class="sl-item sl-item-md">
            <div class="sl-icon">
                <i class="glyphicon glyphicon-barcode"></i>
            </div>
            <div class="sl-content">
                <div class="text-dk ">
                    <asp:Label ID="lblClaimNumberTitle" runat="server" AssociatedControlID="lblClaimNumber" Text="<%$ Resources:lbl_ClaimNumber %>"></asp:Label>
                    <asp:Label ID="lblClaimNumber" SkinID="badgeInfo" runat="server"></asp:Label>
                </div>
            </div>
        </div>
        <div runat="server" id="liClaimDocs">
            <div class="sl-item">
                <div class="sl-content">
                    <div class="text-dk">
                        <asp:HyperLink ID="hypClaimDocs" SkinID="lnkHInfo" data="modal" NavigateUrl="~/Modal/DocumentManager.aspx?modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=550&width=750', null);return false;" runat="server" Text="<%$ Resources:hypDocs %>"></asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
        <div id="liRisk" runat="server" visible="false" class="sl-item sl-item-md">
            <div class="sl-icon">
                <i class="fa fa-ambulance"></i>
            </div>
            <div class="sl-content">
                <div class="text-dk ">
                    <asp:Label ID="lblRiskTitle" runat="server" AssociatedControlID="hypRisk" Text="Risk"></asp:Label>
                    <asp:LinkButton ID="hypRisk" SkinID="lnkInfo" Text="<%$ Resources:hyp_Details %>" runat="server" Visible="false" CausesValidation="false"></asp:LinkButton>
                </div>
            </div>
        </div>
        <div id="liFinancialDetails" runat="server" visible="false">
            <div class="sl-item">
                <div class="sl-content">
                    <div class="text-dk">
                        <asp:HyperLink ID="hypFinancialdetails" SkinID="lnkHInfo" data="modal" NavigateUrl="~/Claims/FinancialDetails.aspx?modal=true&KeepThis=true&TB_iframe=true&height=550&width=750" Text="<%$ Resources:hyp_Financial_Details %>" runat="server"></asp:HyperLink>
                    </div>
                </div>
            </div>
            <div class="sl-item">
                <div class="sl-content">
                    <div class="text-dk">
                        <asp:HyperLink ID="hypEvents" SkinID="lnkHInfo" data="modal" NavigateUrl="~/secure/EventList.aspx?modal=true&ReturnUrl=Claim" Text="<%$ Resources:hyp_Events %>" runat="server"></asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
        <div class="sl-item sl-item-md">
            <div class="sl-icon">
                <i class="fa fa-user"></i>
            </div>
            <div class="sl-content">
                <div class="text-dk text-ellipsis">
                    <asp:HyperLink ID="hypAssociates" SkinID="lnkHInfo" NavigateUrl="~/Modal/PolicyAssociates.aspx?displaymode=view&modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=550&width=750', null);return false;" data="modal" runat="server" Text="<%$ Resources:lbl_Associate %>"></asp:HyperLink>
                </div>
                <p>
                </p>
            </div>
            <div class="sl-item" id="liPolicyAssociate" runat="server">
                <div class="sl-content">
                    <div class="text-dk">
                <asp:Label ID="lblSecondaryPolicyAssociate" runat="server" AssociatedControlID="rptrPolicyAssociate"
                    Text="<%$ Resources:lbl_SecPolicyAssociate %>" />
                <asp:Repeater ID="rptrPolicyAssociate" runat="server">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:HyperLink ID="hypPolicyAssociate"  SkinID="lnkHInfo" runat="server" Text='<%# eval("PartyName") %>' data="modal"
                            CausesValidation="false" />
                    </ItemTemplate>
                    <SeparatorTemplate>
                        </br>
                    </SeparatorTemplate>
                    <FooterTemplate>
                    </FooterTemplate>
                </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
