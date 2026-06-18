<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FindReinsurer.aspx.vb" Inherits="Nexus.secure_agent_FindReinsurer"
    MasterPageFile="~/default.master" EnableViewState="false" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div id="Modal_FindReinsurer">
        <asp:Panel ID="PnlFindReInsurer" runat="server" DefaultButton="btnSearch" CssClass="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPageHeader" runat="server" Text="ReInsurer"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblReinsurerCode" runat="server" AssociatedControlID="txtReinsurerCode" Text="<%$ Resources:lblReinsurerCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtReinsurerCode" TabIndex="1" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" Text="<%$ Resources:lblname %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtName" TabIndex="2" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblFileCode" runat="server" AssociatedControlID="txtFileCode" Text="<%$ Resources:lblFileCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtFileCode" TabIndex="4" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label"></label>
                        <div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="chkIncludeClosedBranches" runat="server" Text="<%$ Resources:chkIncludeClosedBranches %>" CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" TabIndex="5" Text="<%$ Resources:btnCancel %>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnSearch" runat="server" TabIndex="5" Text="<%$ Resources:btnSearch %>" SkinID="btnPrimary"></asp:LinkButton>
               
            </div>
        </asp:Panel>
        <Nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$Resources:lbl_WildCardAtEnd_Error %>" NoWildCardErrorMessage="<%$Resources:lbl_NoWildCard_Error %>" ControlsToValidate="txtReinsurerCode,txtName,txtFileCode" Condition="Auto" Display="none" runat="server" EnableClientScript="true">
        </Nexus:WildCardValidator>
        <asp:ValidationSummary ID="vldFindInsuranceFileSummary" runat="server" DisplayMode="BulletList" ShowSummary="true" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
        <asp:UpdatePanel ID="updFindReinsurer" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
            <ContentTemplate>
                <div class="grid-card table-responsive">
                    <asp:GridView ID="grdvSearchResults" runat="server" PageSize="10" AllowPaging="true" PagerSettings-Mode="Numeric" AutoGenerateColumns="false" GridLines="None" Caption="<%$ Resources:lblGridHeading %>" OnPageIndexChanging="grdvSearchResults_PageIndexChanging" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                        <Columns>
                            <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:lblReinsurerCode_g %>"></asp:BoundField>
                            <asp:BoundField DataField="ResolvedName" HeaderText="<%$ Resources:lblName_g %>"></asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:lblAddressLine1_g %>">
                                <ItemTemplate>
                                    <asp:Literal ID="ltAddressLine1" runat="server"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:lblAddressLine2_g %>">
                                <ItemTemplate>
                                    <asp:Literal ID="ltAddressLine2" runat="server"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id="menu_<%# Eval("UserName") %>" class="list-inline no-margin">
                                            <li>
                                                <asp:LinkButton ID="btnSelect" SkinID="btnGrid"  runat="server" OnClientClick=<%# "self.parent.setReInsurer('" + Eval("ResolvedName").ToString() + "','" + Eval("Key").ToString() + "','" + Eval("UserName").ToString() + "');"%>><i class='fa fa-check' aria-hidden='true'></i> Select</asp:LinkButton>
                                               <%-- <a href="#" onclick="self.parent.setReInsurer('<%# DataBinder.Eval(Container.DataItem,"ResolvedName")%>','<%# DataBinder.Eval(Container.DataItem,"Key")%>','<%# DataBinder.Eval(Container.DataItem,"UserName")%>');">select</a>--%>
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
                <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="DataBound"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="RowCommand"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <Nexus:ProgressIndicator ID="upFndReinsurer" OverlayCssClass="updating" AssociatedUpdatePanelID="updFindReinsurer" runat="server">
            <ProgressTemplate>
            </ProgressTemplate>
        </Nexus:ProgressIndicator>

    </div>
</asp:Content>
