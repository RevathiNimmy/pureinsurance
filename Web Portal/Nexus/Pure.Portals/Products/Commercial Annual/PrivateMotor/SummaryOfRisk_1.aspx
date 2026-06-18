<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SummaryOfRisk.aspx.vb" 
     Inherits="Nexus.Products_UIIC_SummaryOfRisk" MasterPageFile="~/Default.master"  EnableViewState="true"%>
     
    <%@ Register Src="~/Controls/RiskFees.ascx" TagName="RiskFeesCntrl" TagPrefix="uc1" %>
     <%@ Register Src="~/Controls/RiskTax.ascx" TagName="RiskTaxCntrl" TagPrefix="uc2" %>
     
       
<asp:Content ID="cntMainBody" runat="server" ContentPlaceHolderID="cntMainBody">
 
     <uc1:RiskFeesCntrl ID="RiskFeesCntrl" runat="server" />
     <uc2:RiskTaxCntrl ID="RiskTaxCntrl" runat="server" />
  
</asp:Content>

