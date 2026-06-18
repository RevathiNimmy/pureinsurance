<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FindAgent.aspx.vb" Inherits="Nexus.secure_agent_FindAgent"
    MasterPageFile="~/Default.master" ValidateRequest="false" %>

<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div id="Modal_FindAgent">
        <asp:Panel ID="PnlFindAccountHandler" runat="server" DefaultButton="btnSearch" CssClass="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblPageHeader%>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAgent_code" runat="server" AssociatedControlID="txtAgent_code" Text="<%$ Resources:lblAgent_code %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtAgent_code" TabIndex="1" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" Text="<%$ Resources:lblName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtName" TabIndex="2" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblFileCode" runat="server" AssociatedControlID="txtFileCode" Text="<%$ Resources:lblFileCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtFileCode" TabIndex="3" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAgentType" runat="server" AssociatedControlID="ddlAgentType" Text="<%$ Resources:lblAgentType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlAgentType" TabIndex="4" CssClass="field-medium form-control form-select" runat="server" EnableViewState="true">
                                <asp:ListItem Value="ALL" Text="<%$ Resources:ddlAgentType_All %>"></asp:ListItem>
                                <asp:ListItem Value="Broker" Text="<%$ Resources:ddlAgentType_Broker %>"></asp:ListItem>
                                <asp:ListItem Value="CA" Text="<%$ Resources:ddlAgentType_CA %>"></asp:ListItem>
                                <asp:ListItem Value="Inter" Text="<%$ Resources:ddlAgentType_Inter %>"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAgentGroup" runat="server" AssociatedControlID="ddlAgentGroup" Text="<%$ Resources:lblAgentGroup %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlAgentGroup" TabIndex="5" CssClass="field-medium form-control form-select" runat="server"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">

                        <asp:Label ID="lblIncludeClosedBranches" runat="server" AssociatedControlID="chkIncludeClosedBranches" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litIncludeClosedBranches" runat="server" Text="<%$ Resources:chkIncludeClosedBranches  %>"></asp:Literal>
                        </asp:Label><div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="chkIncludeClosedBranches" runat="server" TabIndex="5" Text=" " CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" TabIndex="8" Text="<%$ Resources:btnCancel %>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnSearch" runat="server" TabIndex="7" Text="<%$ Resources:btnSearch %>" SkinID="btnPrimary"></asp:LinkButton>
               
            </div>
        </asp:Panel>
        <nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="No wild card except at the end please" NoWildCardErrorMessage="No Wild card is allowed" ControlsToValidate="txtAgent_code,txtName,txtFileCode" Condition="Auto" Display="none" runat="server" EnableClientScript="true">
        </nexus:WildCardValidator>
        <asp:ValidationSummary ID="vldFindInsuranceFileSummary" runat="server" DisplayMode="BulletList" ShowSummary="true" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>

        <div class="grid-card table-responsive">
            <asp:UpdatePanel ID="updFindAgent" runat="server" UpdateMode="Always" ChildrenAsTriggers="True">
                <ContentTemplate>
                    <asp:GridView ID="grdvSearchResults" runat="server" PageSize="10" AllowPaging="true" DataKeyNames="Key" PagerSettings-Mode="Numeric" AutoGenerateColumns="false" GridLines="None" OnPageIndexChanging="grdvSearchResults_PageIndexChanging" OnRowCommand="grdvSearchResults_RowCommand" OnDataBound="grdvSearchResults_DataBound" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData">
                        <Columns>
                            <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:lblAgentCode_g %>"></asp:BoundField>
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
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblDateCancelled" runat="server" Text='<%#Eval("DateCancelled")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id='menu_<%# Eval("UserName") %>' class="list-inline no-margin">
                                            <li>
                                                <asp:LinkButton id="lnkbtnSelect" runat="server" SkinID="btnGrid"><i class='fa fa-check' aria-hidden='true'></i> Select</asp:LinkButton>
                                            </li>
                                        </ol>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="DataBound"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="grdvSearchResults" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
            <nexus:ProgressIndicator ID="upFndAgent" OverlayCssClass="updating" AssociatedUpdatePanelID="updFindAgent" runat="server">
                <progresstemplate>
                    </progresstemplate>
            </nexus:ProgressIndicator>
        </div>
    </div>
</asp:Content>
