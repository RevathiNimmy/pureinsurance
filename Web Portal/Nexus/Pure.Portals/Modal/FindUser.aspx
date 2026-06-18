<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FindUser.aspx.vb" Inherits="Nexus.Modal_FindUser"
    MasterPageFile="~/default.master" EnableViewState="true" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div id="Modal_FindUser">
        <asp:Panel ID="PnlFindUser" runat="server" DefaultButton="btnFind" CssClass="card">
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblBankGuaranteeDetails" runat="server" Text="<%$ Resources:lbl_FindUser %>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblUserName" runat="server" AssociatedControlID="txtUserName" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litUserName" runat="server" Text="<%$ Resources:lbl_UserName %>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblFullName" runat="server" AssociatedControlID="txtFullName" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litFullName" runat="server" Text="<%$ Resources:lbl_FullName %>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnNewSearch" runat="server" Text="<%$ Resources:btn_NewSearch %>" TabIndex="4" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnFind" runat="server" Text="<%$ Resources:btn_Find %>" TabIndex="3" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </asp:Panel>
        <Nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$Resources:lbl_WildCardAtEnd_Error %>" NoWildCardErrorMessage="<%$Resources:lbl_NoWildCard_Error %>" ControlsToValidate="txtUserName,txtFullName" Condition="Auto" Display="none" runat="server" EnableClientScript="true">
        </Nexus:WildCardValidator>
        <asp:ValidationSummary ID="vldFindInsuranceFileSummary" runat="server" DisplayMode="BulletList" ShowSummary="true" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
        <asp:UpdatePanel ID="updFindUser" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
            <ContentTemplate>
                <div class="grid-card table-responsive">
                    <asp:GridView ID="grdvFindUser" runat="server" AllowPaging="true" AutoGenerateColumns="False" CellPadding="0" CellSpacing="0" GridLines="None" PagerSettings-Mode="Numeric" PageSize="10" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData">
                        <Columns>
                            <asp:BoundField DataField="UserId" HeaderText="ID"></asp:BoundField>
                            <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:lbl_UserName %>"></asp:BoundField>
                            <asp:BoundField DataField="FullName" HeaderText="<%$ Resources:lbl_FullName %>"></asp:BoundField>
                            <asp:BoundField DataField="EffectiveDate" HeaderText="<%$ Resources:lbl_EffectiveDate %>" DataFormatString="{0:d}"></asp:BoundField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id="menu_<%# Eval("UserId") %>" class="list-inline no-margin">
                                            <li>
                                                <asp:LinkButton ID="btnSelect" SkinID="btnGrid" runat="server" OnClientClick=<%# "self.parent.setUser('" + Eval("UserName").ToString() + "');"%>><i class='fa fa-check' aria-hidden='true'></i> Select</asp:LinkButton>
                                                <%--<a href="#" onclick="self.parent.setUser('<%# DataBinder.Eval(Container.DataItem,"UserName")%>');">select</a>--%>

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
                <asp:AsyncPostBackTrigger ControlID="grdvFindUser" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvFindUser" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <Nexus:ProgressIndicator ID="upFndUser" OverlayCssClass="updating" AssociatedUpdatePanelID="updFindUser" runat="server">
            <ProgressTemplate>
            </ProgressTemplate>
        </Nexus:ProgressIndicator>
    </div>
</asp:Content>
