<%@ Page Title="" Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="RIAmendRiskList.aspx.vb" Inherits="Nexus.secure_RIAmendRiskList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/Navigator.ascx" TagName="Navigator" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/RIAmendMultiRisk.ascx" TagName="MultiRisk" TagPrefix="uc5" %>
<%@ Register Src="~/Controls/PolicyDetails.ascx" TagName="PolicyDetails" TagPrefix="uc7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="smRIAmendRiskList" runat="server"></asp:ScriptManager>
    <div id="secure_RIAmendRiskList">
        <uc3:ProgressBar ID="ucProgressBar" runat="server"></uc3:ProgressBar>
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblSummaryheadertitle" runat="server" Text="<%$ Resources:lbl_Summary_headertitle%>"></asp:Literal>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblPageheader" runat="server" Text="<%$ Resources:lbl_page_header %>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPremiumDisplay" CssClass="col-md-4 col-sm-3 control-label" runat="server" AssociatedControlID="lblPremiumValue">
                            <asp:Literal ID="litPremiumDisplay" runat="server" Text="<%$ Resources:lbl_PremiumDisplay %>"></asp:Literal>
                        </asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="lblPremiumValue" runat="server"></asp:Label>
                            </p>
                        </div>
                    </div>
                </div>
                <uc5:MultiRisk ID="MultiRisk1" runat="server"></uc5:MultiRisk>
                <uc7:PolicyDetails ID="ucPolicyDetails" runat="server"></uc7:PolicyDetails>

            </div>
            <asp:Panel ID="PanelButton" runat="server">
                <div class="card-footer">

                    <asp:LinkButton ID="btnAccept" runat="server" Text="<%$ Resources:btn_Accept%>" SkinID="btnPrimary"></asp:LinkButton>
                    <asp:LinkButton ID="btnSave" runat="server" Text="<%$ Resources:btn_Save%>" CausesValidation="False" SkinID="btnPrimary"></asp:LinkButton>
                </div>
            </asp:Panel>
        </div>
        <asp:CustomValidator ID="vldChkStatus" runat="server" Display="None" OnServerValidate="vldChkStatus_ServerValidate" ErrorMessage="<%$ Resources:lbl_Check_Status %>"></asp:CustomValidator>
        <asp:ValidationSummary ID="ValidationSummary1" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
