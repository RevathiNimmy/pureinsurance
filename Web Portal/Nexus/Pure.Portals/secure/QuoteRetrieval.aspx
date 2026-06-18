<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Default.master"
    CodeFile="QuoteRetrieval.aspx.vb" Inherits="Nexus.secure_QuoteRetrieval" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<%@ Register Src="../Controls/ClientQuotes.ascx" TagName="ClientQuotes" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/ClientClaims.ascx" TagName="ClientClaims" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/NewQuote.ascx" TagName="NewQuote" TagPrefix="uc4" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="secure_QuoteRetrieval">
        <uc3:ProgressBar ID="ucProgressBar" runat="server"></uc3:ProgressBar>
        <asp:ScriptManager ID="scriptBankGuarantee" runat="server">
        </asp:ScriptManager>
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label ID="lblPageHeading" runat="server" Text="<%$ Resources:lbl_Page_header%>"></asp:Label></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblNameTitle" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Literal ID="lblName" runat="server"></asp:Literal>
                    </div>
                    <div id="liMainContact" runat="server" visible="false" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblMainContactTitle" runat="server" Text="<%$ Resources:lbl_MainContact %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Literal ID="lblMainContact" runat="server"></asp:Literal>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblEmailTitle" runat="server" Text="<%$ Resources:lbl_EmailAddress %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Literal ID="LblEmail" runat="server"></asp:Literal>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblTelephoneTitle" runat="server" Text="<%$ Resources:lbl_Telephone %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Literal ID="LblTelephone" runat="server"></asp:Literal>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAddress1Title" runat="server" Text="<%$ Resources:lbl_Address1 %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Literal ID="LblAddress1" runat="server"></asp:Literal>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAddress2Title" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Literal ID="LblAddress2" runat="server"></asp:Literal>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAddress3Title" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Literal ID="LblAddress3" runat="server"></asp:Literal>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAddress4Title" runat="server" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Literal ID="LblAddress4" runat="server"></asp:Literal>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPostcodeTitle" runat="server" Text="<%$ Resources:lbl_Postcode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Literal ID="LblPostcode" runat="server"></asp:Literal>
                    </div>
                    <div id="licountry" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCountryTitle" runat="server" Text="<%$ Resources:lbl_Country %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <asp:Literal ID="lblCountry" runat="server"></asp:Literal>
                    </div>
                </div>
                <asp:Label ID="lblconfirmmsg" runat="server" Text="<%$ Resources:lbl_confirm_msg %>"></asp:Label>

                <legend>
                    <asp:Literal ID="lblQuoteHeader" runat="server" Text="<%$ Resources:lbl_Quote_header %>"></asp:Literal></legend>
                <uc1:ClientQuotes ID="ctrlClientQuotes" runat="server"></uc1:ClientQuotes>
                <legend>
                    <asp:Literal ID="lbl_Claims_header" runat="server" Text="<%$ Resources:lbl_Claims_header %>"></asp:Literal></legend>
                <uc2:ClientClaims ID="ctrlClientClaims" runat="server"></uc2:ClientClaims>
                <uc4:NewQuote ID="ctrlNewQuote" runat="server"></uc4:NewQuote>
            </div>
            <div class="card-footer">
                <asp:LinkButton runat="server" ID="btnEditClient" Text="<%$ Resources:lbl_EditDetails %>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
