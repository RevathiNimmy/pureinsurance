<%@ Page Title="" Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false" CodeFile="PolicyVersions.aspx.vb" Inherits="Nexus.Modal_PolicyVersions" %>
  
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">  
    <div id="Modal_PolicyVersion">
        <asp:ScriptManager ID="smPolicyVersions" runat="server"></asp:ScriptManager>
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal runat="server" Text="<%$ Resources:lbl_PageHeading %>" ID="ltPageHeading"></asp:Literal>
                </h1>
            </div>
            <div class="card-body clearfix">
                <asp:UpdatePanel ID="updPolicyVersion" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
                    <ContentTemplate>
                        <asp:HiddenField ID="hfAnswerForMarkedQuoteCollection" Value="False" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hfAnswerForMarketPlacePolicy" Value="False" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hvVoidConfirm" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hvVoidMessage" runat="server"></asp:HiddenField>
                        <div class="grid-card table-responsive">
                            <asp:GridView ID="grdPolicyVersions" runat="server" AutoGenerateColumns="False" GridLines="None" PageSize="10" AllowPaging="true" AllowSorting="false" PagerSettings-Mode="Numeric" EmptyDataText="<%$ Resources:lbl_NoResultMessage %>" DataKeyNames="PolicyStatusCode">
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ Resources:gvh_PolicyStatus %>" SortExpression="PolicyStatus">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Status" runat="server" Text='<%# If(CType(Eval("IsCurrent"), Boolean) = True AndAlso (Not Eval("PolicyStatus").ToUpper().Equals("LAPSED")) AndAlso (Not Eval("PolicyStatus").ToUpper().Equals("CANCELLED")), GetLocalResourceObject("lbl_Current"), Eval("PolicyStatus"))%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="PolicyStatus" SortExpression="PolicyStatus" HeaderText="<%$ Resources:gvh_PolicyStatus %>" />--%>
                                    <asp:BoundField DataField="InsuranceFileTypeDesc" SortExpression="InsuranceFileTypeDesc" HeaderText="<%$ Resources:gvh_PolicyType %>"></asp:BoundField>
                                    <asp:BoundField DataField="DateIssued" SortExpression="DateIssued" HeaderText="<%$ Resources:gvh_QuoteDate %>" DataFormatString="{0:d}"></asp:BoundField>
                                    <asp:BoundField DataField="LeadAgent" SortExpression="LeadAgent" HeaderText="<%$ Resources:gvh_AgentName %>"></asp:BoundField>
                                    <asp:BoundField DataField="CoverStartDate" SortExpression="CoverStartDate" HeaderText="<%$ Resources:gvh_CoverFromDate %>" DataFormatString="{0:d}"></asp:BoundField>
                                    <asp:BoundField DataField="RenewalDate" SortExpression="CoverFromDate" HeaderText="<%$ Resources:gvh_RenewalDate %>" DataFormatString="{0:d}"></asp:BoundField>
                                    <asp:BoundField DataField="TransactionDate" SortExpression="TransactionDate" HeaderText="<%$ Resources:gvh_TransactionDate %>" DataFormatString="{0:d}"></asp:BoundField>
                                    <asp:BoundField DataField="PolicyVersion" SortExpression="PolicyVersion" HeaderText="Version"></asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div class="rowMenu">
                                                <ol class="list-inline no-margin">
                                                    <li class="dropdown no-padding">
                                                        <a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                                        <ol id='menu_<%# Eval("InsuranceFileKey") %>' class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                            <li>
                                                                <asp:LinkButton ID="btnView" runat="server" Text="<%$ Resources:lbl_View %>" CommandName="View" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"InsuranceFileKey")%>' CausesValidation="false" Visible="false"></asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="lnkbtnAction" runat="server" Text="<%$ Resources:lbl_Change %>" CommandName="Change" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"InsuranceFileKey")%>' CausesValidation="false" Visible="false"></asp:LinkButton>
                                                            </li>
                                                            <li id="liRenew" runat="server" visible="false">
                                                                <asp:LinkButton ID="lnkbtnRenew" runat="server" Text="<%$ Resources:lbl_Renew %>" CommandName="Renew" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"InsuranceFileKey")%>' CausesValidation="false"></asp:LinkButton>
                                                            </li>
                                                            <li id="liVoid" runat="server" visible="false">
                                                                <asp:LinkButton ID="lnkbtnVoid" runat="server" Text="<%$ Resources:lbl_Void %>" CommandName="Void" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"InsuranceFileKey")%>' CausesValidation="false"></asp:LinkButton>
                                                            </li>
                                                        </ol>
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
                        <asp:AsyncPostBackTrigger ControlID="grdPolicyVersions" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="grdPolicyVersions" EventName="RowCommand"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="grdPolicyVersions" EventName="RowDataBound"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
                <Nexus:ProgressIndicator ID="upgPolicyVersion" OverlayCssClass="updating" AssociatedUpdatePanelID="updPolicyVersion" runat="server">
                    <ProgressTemplate>
                    </ProgressTemplate>
                </Nexus:ProgressIndicator>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:lbl_btnBack%>" OnClientClick="self.parent.tb_remove();" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnNewQuote" runat="server" Text="<%$ Resources:lbl_btnNewQuote %>" Visible="false" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
    </div>  
</asp:Content>

