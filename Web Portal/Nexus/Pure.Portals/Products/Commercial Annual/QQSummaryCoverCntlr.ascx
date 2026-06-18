<%@ Control Language="VB" AutoEventWireup="false" CodeFile="QQSummaryCoverCntlr.ascx.vb"
    Inherits="Products_TestMOTOR_TestMOTOR_QQSummaryOfCover" %>
<%@ Register Src="~/Controls/PolicyFees.ascx" TagName="PolicyFeesCntrl" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/PolicyTax.ascx" TagName="PolicyTaxCntrl" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/AgentCommission.ascx" TagName="AgentCommissionCntrl"
    TagPrefix="uc3" %>
<%@ Register Src="~/Controls/PolicyDetails.ascx" TagName="PolicyDetails" TagPrefix="uc7" %>

<div class="personal-client-details">
   <asp:Panel ID="pnlSummary" runat="server">
    <p>
    <h2>
   <asp:Label ID="lblPremiumIndicationText" runat="server" Text="Our Premium Indication is" />&nbsp;
   <asp:Label ID="lblPremiumIndication" runat="server" /></h2>
   </p>
   </asp:Panel>
    <uc7:PolicyDetails ID="ucPolicyDetails" runat="server"  />
    <uc1:PolicyFeesCntrl ID="PolicyFeesCntrl" runat="server" />
    <uc2:PolicyTaxCntrl ID="PolicyTaxCntrl" runat="server" />
    <uc3:AgentCommissionCntrl ID="AgentCommissionCntrl" runat="server" />
</div>
