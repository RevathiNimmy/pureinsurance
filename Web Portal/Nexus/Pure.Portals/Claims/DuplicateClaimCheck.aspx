<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DuplicateClaimCheck.aspx.vb"
    Inherits="Nexus.Claims_DuplicateClaimCheck" MasterPageFile="~/Default.master"
    EnableViewState="true" %>

<asp:Content ID="cntMainBody" runat="server" ContentPlaceHolderID="cntMainBody">
    <asp:ScriptManager ID="smDuplicateClaims" runat="server"></asp:ScriptManager>
    <div id="Claims_DuplicateClaimCheck">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal runat="server" Text="<%$ Resources:lbl_PageHeading %>" ID="ltPageHeading"></asp:Literal>
                </h1>
            </div>
            <div class="card-body clearfix">
                <div class="grid-card table-responsive">
                    <asp:GridView ID="grdvDuplicateClaims" runat="server" AutoGenerateColumns="false" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                        <Columns>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_grdvDuplicateClaims_ClaimNumber_heading %>" DataField="ClaimNumber"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_grdvDuplicateClaims_Description_heading %>" DataField="ClaimDescription"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_grdvDuplicateClaims_ProgressStatus_heading %>" DataField="ProgressStatusDescription"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_grdvDuplicateClaims_PrimaryCauseDescription_heading %>" DataField="PrimaryCauseDescription"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_grdvDuplicateClaims_ReportedDate_heading %>" DataField="ReportedDate" DataFormatString="{0:d}"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_grdvDuplicateClaims_LastModifiedDate_heading %>" DataField="LastModifiedDate" DataFormatString="{0:d}"></asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:lbl_grdvDuplicateClaims_ClaimStatus_heading %>" DataField="ClaimStatus"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <asp:UpdatePanel ID="updDuplicateClaims" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlDuplicateClaimOverride" runat="server" Visible="false">
                        <div class="card-body clearfix">
                            <div class="form-horizontal">
                                <legend>
                                    <asp:Label runat="server" Text="<%$ Resources:lbl_ApprovarDetails %>" ID="lblApproverTitle"></asp:Label>
                                </legend>
                                <div class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblDuplicateClaim" runat="server" CssClass="doublewidth" Text="<%$ Resources:lbl_DuplicateClaim_Message %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblUserName" runat="server" AssociatedControlID="txtUserName" Text="<%$ Resources:lbl_UserName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtUserName" CssClass="field-mandatory form-control" ValidationGroup="DuplicateClaimOverrideGroup" runat="server"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvUserName" ControlToValidate="txtUserName" ValidationGroup="DuplicateClaimOverrideGroup" runat="server" ErrorMessage="<%$ Resources:lbl_error_UserName %>" Display="none" Enabled="false" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" Text="<%$ Resources:lbl_Password %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="txtPassword" CssClass="field-mandatory form-control" ValidationGroup="DuplicateClaimOverrideGroup" runat="server" TextMode="Password"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvPassword" ControlToValidate="txtPassword" ValidationGroup="DuplicateClaimOverrideGroup" runat="server" ErrorMessage="<%$ Resources:lbl_error_Password %>" Display="none" Enabled="false" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="card-footer">
                        <asp:LinkButton ID="btnBack" runat="server" Text="<%$ Resources:DuplicateClaims_btnBack %>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                        <asp:LinkButton ID="btnNext" runat="server" Text="<%$ Resources:DuplicateClaims_btnNext %>" SkinID="btnPrimary"></asp:LinkButton>
                        <asp:LinkButton ID="btnOK" runat="server" ValidationGroup="DuplicateClaimOverrideGroup" Text="<%$ Resources:DuplicateClaims_btnOK %>" Visible="false" SkinID="btnPrimary"></asp:LinkButton>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
            <Nexus:ProgressIndicator ID="UpDuplicateClaims" OverlayCssClass="updating" AssociatedUpdatePanelID="updDuplicateClaims" runat="server">
                <ProgressTemplate>
                </ProgressTemplate>
            </Nexus:ProgressIndicator>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" ValidationGroup="DuplicateClaimOverrideGroup" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
