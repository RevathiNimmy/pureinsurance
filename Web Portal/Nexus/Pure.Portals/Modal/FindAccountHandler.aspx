<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FindAccountHandler.aspx.vb"
    Inherits="Nexus.secure_agent_FindAccountHandler" MasterPageFile="~/default.master"
    EnableViewState="false" %>

<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div id="Modal_FindAccountHandler">
        <asp:Panel ID="PnlFindAccountHandler" runat="server" DefaultButton="btnSearch" CssClass="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_Heading_Details %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAccounthandler_code" runat="server" AssociatedControlID="txtAccounthandler_code" Text="<%$ Resources:lblAccounthandler_code %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtAccounthandler_code" TabIndex="1" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" Text="<%$ Resources:lblName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtName" TabIndex="2" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblType" runat="server" AssociatedControlID="txtType" Text="<%$ Resources:lblType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtType" TabIndex="3" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblIncludeClosedBranches" runat="server" AssociatedControlID="chkIncludeClosedBranches" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litIncludeClosedBranches" runat="server" Text="<%$ Resources:chkIncludeClosedBranches  %>"></asp:Literal>
                        </asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="chkIncludeClosedBranches" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" TabIndex="5" Text="<%$ Resources:btnCancel %>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnSearch" runat="server" TabIndex="5" Text="<%$ Resources:btnSearch %>" SkinID="btnPrimary"></asp:LinkButton>
                
            </div>
        </asp:Panel>
        <asp:ValidationSummary ID="vldFindInsuranceFileSummary" runat="server" DisplayMode="BulletList" ShowSummary="true" HeaderText="<%$Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
        <asp:UpdatePanel ID="updFindAcHandler" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
            <ContentTemplate>
                <div class="grid-card table-responsive">
                    <asp:GridView ID="grdvSearchResults" runat="server" PageSize="10" AllowPaging="true" DataKeyNames="Key" PagerSettings-Mode="Numeric" AutoGenerateColumns="false" GridLines="None" OnPageIndexChanging="grdvSearchResults_PageIndexChanging" OnRowCommand="grdvSearchResults_RowCommand" EmptyDataText="<%$ Resources:ErrorMessage %>" EmptyDataRowStyle-CssClass="noData" OnDataBound="grdvSearchResults_DataBound">
                        <Columns>
                            <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:lblAccounthandlerCode_g %>"></asp:BoundField>
                            <asp:BoundField DataField="Name" HeaderText="<%$ Resources:lblName_g %>"></asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id='menu_<%# Eval("Key") %>' class="list-inline no-margin">
                                            <li>
                                                 <asp:LinkButton ID="btnSelect" SkinID="btnGrid"  runat="server" OnClientClick=<%# "self.parent.setAccountHandler('" + Eval("ResolvedName").ToString() + "','" + Eval("Key").ToString() + "','" + Eval("UserName").ToString() + "');"%>><i class='fa fa-check' aria-hidden='true'></i> Select</asp:LinkButton>
                                                <%--<a href="#" onclick="self.parent.setAccountHandler('<%# DataBinder.Eval(Container.DataItem,"ResolvedName")%>','<%# DataBinder.Eval(Container.DataItem,"Key")%>','<%# DataBinder.Eval(Container.DataItem,"UserName")%>');">select</a>--%>
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
        <nexus:ProgressIndicator ID="upFndAcHndlr" OverlayCssClass="updating" AssociatedUpdatePanelID="updFindAcHandler" runat="server">
            <progresstemplate>
                </progresstemplate>
        </nexus:ProgressIndicator>
        <nexus:WildCardValidator ID="vldWildCard" AllowWildCardAtEndErrorMessage="<%$ Resources:lbl_WildCardAtEnd_Error %>" NoWildCardErrorMessage="<%$ Resources:lbl_NoWildCard_Error %>" ControlsToValidate="txtAccounthandler_code,txtName,txtType" Condition="Auto" Display="none" runat="server" EnableClientScript="true">
        </nexus:WildCardValidator>
        
    </div>
</asp:Content>
