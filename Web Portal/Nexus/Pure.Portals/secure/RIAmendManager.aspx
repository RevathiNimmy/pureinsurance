<%@ Page Language="VB" MasterPageFile="~/default.master"
    AutoEventWireup="false" CodeFile="RIAmendManager.aspx.vb" Inherits="Nexus.secure_RIAmendManager" %>

<asp:Content ID="cntMainBody" runat="server" ContentPlaceHolderID="cntMainBody">
    <asp:ScriptManager ID="ScriptManagerRenewalManager" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    <div id="secure_RIAmendManager">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblHeader_PT %>"></asp:Literal>
                </h1>
            </div>
            <div class="card-body clearfix">
                <asp:UpdatePanel ID="updpnlRIAmendManager" runat="server">
                    <ContentTemplate>
                        <div class="grid-card table-responsive">
                            <asp:GridView ID="gvRIAmendManager" runat="server" AutoGenerateColumns="False" DataKeyNames="InsuranceFileKey" GridLines="None" PageSize="10" AllowPaging="true" PagerSettings-Mode="Numeric" AllowSorting="true" EmptyDataText="<%$ Resources:msg_NoRecordFound %>" EmptyDataRowStyle-CssClass="noData">
                                <Columns>
                                    <%--<asp:BoundField DataField="InsuranceFileKey" />--%>
                                    <asp:TemplateField ShowHeader="true">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkSelectAll" runat="server" OnCheckedChanged="chkSelectAll_CheckedChanged" AutoPostBack="true" EnableViewState="true" CausesValidation="False" Text=" " CssClass="asp-check"></asp:CheckBox>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelection" AutoPostBack="true" runat="server" OnCheckedChanged="chkSelection_CheckedChanged" CausesValidation="False" Text=" " CssClass="asp-check"></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="InsuranceFileRef" HeaderText="<%$ Resources:lbl_InsuranceFileRef %>" SortExpression="InsuranceFileRef"></asp:BoundField>
                                    <asp:BoundField DataField="BranchDescription" HeaderText="<%$ Resources:lbl_Branch %>" SortExpression="BranchDescription"></asp:BoundField>
                                    <asp:BoundField DataField="PTRIStatus" HeaderText="<%$ Resources:lbl_InuranceFileStatus %>" SortExpression="PTRIStatus"></asp:BoundField>
                                    <asp:BoundField DataField="PartyShortName" HeaderText="<%$ Resources:lbl_PartyShortName %>" SortExpression="PartyShortName"></asp:BoundField>
                                    <asp:BoundField DataField="PartyName" HeaderText="<%$ Resources:lbl_PartyName %>" SortExpression="PartyName"></asp:BoundField>
                                    <asp:BoundField DataField="ProductDescription" HeaderText="<%$ Resources:lbl_ProductDescription %>" SortExpression="ProductDescription"></asp:BoundField>
                                    <asp:TemplateField ShowHeader="false">
                                        <ItemTemplate>
                                            <div class="rowMenu">
                                                <ol id='menu_<%# Eval("InsuranceFileKey") %>' class="list-inline no-margin">
                                                    <li>
                                                        <asp:LinkButton ID="lnkbtnAmend" Text="<%$ Resources:lbl_lnkbtnAmend %>" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"InsuranceFileKey")%>' CausesValidation="False" CommandName="EditRI" SkinID="btnGrid"></asp:LinkButton>
                                                    </li>
                                                </ol>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <Nexus:ProgressIndicator ID="piRIAmendManager" OverlayCssClass="updating" AssociatedUpdatePanelID="updpnlRIAmendManager" runat="server">
                    <ProgressTemplate>
                    </ProgressTemplate>
                </Nexus:ProgressIndicator>
            </div>
            <asp:Panel ID="pnlButtons" runat="server" Visible="true">
                <div class="card-footer">
                    <asp:UpdatePanel ID="updSubmitArea" runat="server">
                        <ContentTemplate>
                            <asp:LinkButton ID="btnSelectAll" runat="server" Visible="false" SkinID="btnPrimary" Text="<%$ Resources:lbl_btnSelectAll %>"></asp:LinkButton>
                            <asp:LinkButton ID="btnAccept" runat="server" SkinID="btnPrimary" Enabled="false" Text="<%$ Resources:lbl_btnAccept %>"></asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
        </div>
        <asp:ValidationSummary ID="vldRIAmendManager" runat="server" DisplayMode="BulletList" ShowSummary="true" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>

    </div>
</asp:Content>
