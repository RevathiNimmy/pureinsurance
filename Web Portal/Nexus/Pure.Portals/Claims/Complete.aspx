<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="Complete.aspx.vb" Inherits="Nexus.Framework_Complete" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register TagPrefix="OC" Namespace="Nexus" %>
<%@ Register Src="~/Controls/ClaimsProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/RiskData.ascx" TagName="TransactionConfirmation" TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <div id="Claims_Complete">
        <uc3:ProgressBar ID="ucProgressBar" runat="server"></uc3:ProgressBar>
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label ID="ltClaimStatus" runat="server" Text="<%$ resources:lbl_ClaimStatus_Heading %>"></asp:Label>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div class="list-group list-group-sm list-group-gap">
                    <div class="list-group-item md-whiteframe-z0 b-l-success b-l-3x text-success-dk">
                        <i class="fa fa-check i-16 m-r-sm"></i>
                        <asp:Literal runat="server" ID="ltrlThankyouRegister" Visible="false" Text="<%$ Resources:ltrl_thankyou_register %>" />
                        <asp:Literal runat="server" ID="ltrlThankyouUpdate" Visible="false" Text="<%$ Resources:ltrl_thankyou_update %>" />
                        <asp:Literal runat="server" ID="ltrlAuthorizeClaimPayment" Visible="false" Text="<%$ Resources:ltrlAuthorizeClaimPayment %>" />
                        <asp:Literal runat="server" ID="ltrlCloseClaim" Visible="false" Text="<%$ Resources:ltrl_ltrlCloseClaim %>" />
                       <asp:Literal runat="server" ID="ltrlUnableToCloseClaim" Visible="false" Text="<%$ Resources:ltrl_UnableToCloseClaim %>" />                                        
                         <asp:LinkButton ID="hypClaimNumber" CssClass="btn btn-default btn-xs text-success font-bold" runat="server" />
                    </div>
                </div>
                <uc1:TransactionConfirmation ID="TransactionConfirmation" runat="server"></uc1:TransactionConfirmation>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnReturnToCase" runat="server" Text="<%$ Resources:btn_ReturnToCase %>" Visible="false" SkinID="btnSecondary"></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
