<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Reserves.aspx.vb" Inherits="Nexus.Framework_Reserves"
    MasterPageFile="~/default.master" %>

<%@ Register TagPrefix="OC" Namespace="Nexus" %>
<%@ Register Src="~/Controls/ClaimsProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="Claims_Reserves">
        <uc3:ProgressBar ID="ucProgressBar" runat="server"></uc3:ProgressBar>
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal runat="server" Text="<%$ Resources:Reserves_pageheading %>" ID="ltPageHeading"></asp:Literal>
                </h1>
            </div>
            <div class="card-body clearfix">
                <asp:Literal runat="server" Text="<%$ Resources:Reserves_OverrideMessage %>" ID="ltOverrideMessage"></asp:Literal>
                <div class="grid-card table-responsive">
                    <asp:UpdatePanel ID="updateReserves" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <asp:GridView ID="grdvPerils" runat="server" AllowSorting="true" AutoGenerateColumns="false" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                <Columns>
                                    <asp:BoundField HeaderText="<%$ Resources:lbl_grdvPerils_Description_heading %>" DataField="Description"></asp:BoundField>
                                    <asp:TemplateField HeaderText="<%$ Resources:lbl_grdvPerils_SumInsured_heading %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSumInsured" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:lbl_grdvPerils_ReserveTotal_heading %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReserveTotal" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkRecoveries" runat="server" SkinID="btnHGrid" Text="<%$ Resources:lbl_grdvPerils_linkRecoveries_text %>"></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:Button ID="btnRefreshReserves" runat="server" Text="<%$Resources:btn_RefreshReserves %>" Style="display: none" CausesValidation="False" OnClick="btnRefreshReserves_Click"></asp:Button>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnRefreshReserves" EventName="Click"></asp:AsyncPostBackTrigger>
                            <asp:AsyncPostBackTrigger ControlID="grdvPerils" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                        </Triggers>
                    </asp:UpdatePanel>
                    <Nexus:ProgressIndicator ID="upReserves" OverlayCssClass="updating" AssociatedUpdatePanelID="updateReserves" runat="server">
                        <progresstemplate>
                            </progresstemplate>
                    </Nexus:ProgressIndicator>
                </div>
            </div>
            <div class="card-footer">
                    <asp:LinkButton SkinID="btnSecondary" ID="btnBack" runat="server" Text="<%$ Resources:btn_Back %>"></asp:LinkButton>
                    <asp:LinkButton ID="btnSubmit" SkinID="btnPrimary" runat="server" Text="<%$ Resources:btn_Next %>" CausesValidation="true" ValidationGroup="ReserveGroup"></asp:LinkButton>
            </div>
        </div>
        <asp:CustomValidator ID="IsValidReserve" runat="server" ErrorMessage="<%$ Resources:Reserves_ValidReserveItem %>" Display="none" ValidationGroup="ReserveGroup"></asp:CustomValidator>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" ValidationGroup="ReserveGroup" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
