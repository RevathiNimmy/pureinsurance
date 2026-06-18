<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PlanTransactions.aspx.vb"
    Inherits="Nexus.Modal_PlanTransactions" MasterPageFile="~/default.master" %>

<%@ Register Src="~/Controls/PlanTransactions.ascx" TagName="PlanTransactions" TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <uc1:PlanTransactions ID="ucPlanTransactions" runat="server" />
</asp:Content>
