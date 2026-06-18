<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="DuplicateClients.aspx.vb" Inherits="Nexus.Modal_DuplicateClients" Title="Untitled Page" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div id="Modal_DuplicateClients">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label runat="server" ID="lblDuplicateClientCheck" Text="<%$ Resources:lbl_DuplicateClientCheck %>"></asp:Label>
                </h1>
            </div>
            <div id="Div1" class="card-body clearfix" runat="server">
                <asp:UpdatePanel ID="updDuplicateClients" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
                    <ContentTemplate>
                        <div class="grid-card table-responsive">
                            <asp:Label runat="server" ID="lblDuplicateClientWarning" Text="<%$ Resources:lbl_DuplicateClientWarning %>"></asp:Label>
                            <asp:GridView ID="grdvSearchResults" runat="server" AllowPaging="True" PagerSettings-Mode="Numeric" AutoGenerateColumns="False" GridLines="None" OnPageIndexChanging="grdvSearchResults_PageIndexChanging" OnRowCommand="grdvSearchResults_RowCommand" DataKeyNames="UserName" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                <Columns>
                                    <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:lblRegistration_Firstname_g %>"></asp:BoundField>
                                    <asp:BoundField DataField="ResolvedName" HeaderText="<%$ Resources:lblBusiness_Lastname_g %>"></asp:BoundField>
                                    <asp:TemplateField HeaderText="<%$ Resources:lblPhone_g %>">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltPhone" runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:lblRiskIndex_g %>">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltRiskIndex" runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:lblPostcode_g %>">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltPostcode" runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:lblCustomerType_g %>">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltCustomerType" runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div class="rowMenu">
                                                <ol id="menu_<%# Eval("UserName") %>" class="list-inline no-margin">
                                                    <li>
                                                        <asp:LinkButton Text="<%$ Resources:lbl_select %>" ID="lnkDetails" runat="server" SkinID="btnGrid"></asp:LinkButton>
                                                    </li>
                                                </ol>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
                <Nexus:ProgressIndicator ID="upDupClients" OverlayCssClass="updating" AssociatedUpdatePanelID="updDuplicateClients" runat="server">
                    <ProgressTemplate>
                    </ProgressTemplate>
                </Nexus:ProgressIndicator>
                <asp:Panel CssClass="error" ID="PnlError" runat="server" Visible="false">
                    <asp:Label runat="server" ID="lblError" ForeColor="black"></asp:Label>
                </asp:Panel>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnIgnore" runat="server" Text="<%$ Resources:btn_Ignore %>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" HeaderText="<%$ Resources:lbl_ValidationSummary %>" DisplayMode="BulletList" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
