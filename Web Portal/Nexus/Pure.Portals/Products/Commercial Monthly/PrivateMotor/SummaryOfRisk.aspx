<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SummaryOfRisk.aspx.vb"
    Inherits="Nexus.Products_UIIC_SummaryOfRisk" MasterPageFile="~/Default.master" EnableViewState="true" %>

<%@ Register Src="~/Controls/RiskFees.ascx" TagName="RiskFeesCntrl" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/RiskTax.ascx" TagName="RiskTaxCntrl" TagPrefix="uc2" %>


<asp:Content ID="cntMainBody" runat="server" ContentPlaceHolderID="cntMainBody">
    <div class="card">
        <div class="card-heading">
            <h1>
                <asp:Literal ID="lblTitle" runat="server" Text="Risk Edit Details"></asp:Literal></h1>
        </div>
        <div class="card-body clearfix">
            <uc1:RiskFeesCntrl ID="RiskFeesCntrl" runat="server"></uc1:RiskFeesCntrl>
            <uc2:RiskTaxCntrl ID="RiskTaxCntrl" runat="server"></uc2:RiskTaxCntrl>
        </div>

        <div class="card-footer">
            <asp:LinkButton ID="btnClose" runat="server" Text="<i class='fa fa-times' aria-hidden='true'></i> Close" OnClick="btnClose_Click" SkinID="btnSecondary"></asp:LinkButton>
        </div>
    </div>
</asp:Content>

