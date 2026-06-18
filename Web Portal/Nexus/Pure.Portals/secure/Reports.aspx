<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master"
    CodeFile="Reports.aspx.vb" Inherits="Nexus.secure_Reports" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:Panel ID="pnlReports" runat="server" CssClass="card" DefaultButton="btnSubmit">
        <div class="card-heading">
            <h1>
                <asp:Literal ID="lblHeader" runat="server" Text="<%$ Resources:lbl_header %>"></asp:Literal></h1>
        </div>
        <div class="card-body clearfix">
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblLegendHeader" runat="server" Text="<%$ Resources:lbl_Page_header%>"></asp:Label></legend>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblReportsType" runat="server" AssociatedControlID="ddlReportsType" Text="<%$ Resources:lbl_SelectReport%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="ddlReportsType" runat="server" CssClass="form-control form-select">
                        </asp:DropDownList>
                    </div>
                    <asp:RequiredFieldValidator ID="vldrqdReportsType" runat="server" ControlToValidate="ddlReportsType" Display="none" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_ReportTypeRequiredMsg%>" InitialValue="" ValidationGroup="vldReportsGroup"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="card-footer">
            <asp:LinkButton ID="btnSubmit" runat="server" SkinID="btnPrimary" Text="<%$ Resources: btnSubmit %>" ValidationGroup="vldReportsGroup"></asp:LinkButton>
        </div>
    </asp:Panel>
    <asp:PlaceHolder ID="plcReportForm" runat="server" EnableViewState="false" Visible="false"></asp:PlaceHolder>
    <asp:ValidationSummary ID="vldSummaryReportsGroup" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" ValidationGroup="vldReportsGroup" CssClass="validation-summary"></asp:ValidationSummary>
    <asp:ValidationSummary ID="vldSummaryReportsControlsGroup" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" ValidationGroup="vldReportsControlsGroup" CssClass="validation-summary"></asp:ValidationSummary>
    <asp:CustomValidator ID="cusReportForm" runat="server" ErrorMessage="<%$ Resources:lbl_ReportCustomValidatorMsg %>" Display="none" SetFocusOnError="true" ValidationGroup="vldReportsControlsGroup"></asp:CustomValidator>
    

</asp:Content>
