<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ClaimVersion.aspx.vb" Inherits="Nexus.Modal_ClaimVersion"
    MasterPageFile="~/default.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="Modal_ClaimVersion">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal runat="server" Text="<%$ Resources:pageheading %>" ID="ltPageHeading"></asp:Literal>
                </h1>
            </div>
            <div class="card-body clearfix">
                <asp:UpdatePanel ID="updClaimVersion" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
                    <ContentTemplate>
                        <div class="grid-card table-responsive">
                            <asp:GridView ID="grdvClaims" runat="server" Width="98%" GridLines="None" AutoGenerateColumns="false" DataKeyNames="ClaimKey" AllowPaging="true" PageSize="10" OnPageIndexChanging="grdvClaims_PageIndexChanging" AllowSorting="True" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div class="rowMenu">
                                                <ol id='menu_<%# Eval("ClaimKey") %>' class="list-inline no-margin">
                                                    <li>
                                                        <asp:LinkButton ID="btnView" runat="server" Text="<%$ Resources:btnSelect %>" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container, "DataItemIndex") %>' CausesValidation="false" SkinID="btnGrid"></asp:LinkButton>
                                                    </li>
                                                </ol>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="<%$ Resources:lbl_grdvClaims_Version_heading %>" DataField="Version"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:lbl_grdvClaims_TransactionDate_heading %>" DataField="TransactionDate"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:lbl_grdvClaims_TransactionType_heading %>" DataField="TransactionType"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:lbl_grdvClaims_ClaimDescription_heading %>" DataField="ClaimDescription"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:lbl_grdvClaims_ClaimNumber_heading %>" DataField="ClaimNumber"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:lbl_grdvClaims_LossDateFrom_heading %>" DataField="LossFromDate" DataFormatString="{0:d}" HtmlEncode="False"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:lbl_grdvClaims_VersionDescription_heading %>" DataField="VersionDescription"></asp:BoundField>
                                    <Nexus:BoundField HeaderText="<%$ Resources:lbl_grdvClaims_TotalIncurred_heading %>" DataField="TotalIncurred" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:BoundField HeaderText="<%$ Resources:lbl_grdvClaims_TotalPaid_heading %>" DataField="TotalPaid" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:BoundField HeaderText="<%$ Resources:lbl_grdvClaims_ThisRevision_heading %>" DataField="ThisRevision" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:BoundField HeaderText="<%$ Resources:lbl_grdvClaims_ThisPayment_heading %>" DataField="ThisPayment" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:BoundField HeaderText="<%$ Resources:lbl_grdvClaims_ThisSalvageRecovery_heading %>" DataField="ThisSalvageRecovery" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:BoundField HeaderText="<%$ Resources:lbl_grdvClaims_ThisThirdPartyRecovery_heading %>" DataField="ThisThirdPartyRecovery" DataType="Currency"></Nexus:BoundField>
                                    <Nexus:BoundField HeaderText="<%$ Resources:lbl_grdvClaims_CurrentReserve_heading %>" DataField="CurrentReserve" DataType="Currency"></Nexus:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:lbl_grdvClaims_PolicyCurrency_heading %>" DataField="PolicyCurrency"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:lbl_grdvClaims_LossCurrency_heading %>" DataField="LossCurrency"></asp:BoundField>
                                    <asp:BoundField HeaderText="<%$ Resources:lbl_grdvClaims_User_heading %>" DataField="User"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="grdvClaims" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="grdvClaims" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="grdvClaims" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
                <Nexus:ProgressIndicator ID="upClaimVersion" OverlayCssClass="updating" AssociatedUpdatePanelID="updClaimVersion" runat="server">
                    <progresstemplate>
                            </progresstemplate>
                </Nexus:ProgressIndicator>
            </div>
        </div>
        <asp:ValidationSummary ID="vldSearchResultsSummary" runat="server" DisplayMode="BulletList" ShowSummary="true" ValidationGroup="Claims_PayClaim" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
