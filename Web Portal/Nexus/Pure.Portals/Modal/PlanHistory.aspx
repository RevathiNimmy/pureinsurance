<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PlanHistory.aspx.vb" Inherits="Nexus.Modal_PlanHistory"
    MasterPageFile="~/default.master" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        
    </script>

    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div id="Modal_PlanHistory">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label ID="lblPlanHistory" runat="server" Text="<%$ Resources:lbl_PlanHistory %>"></asp:Label>
                </h1>
            </div>
            <div class="card-body clearfix">
                <asp:UpdatePanel ID="updPlanHistory" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
                    <ContentTemplate>
                        <div class="grid-card table-responsive">
                            <asp:GridView ID="grdHistory" runat="server" PageSize="10" AllowSorting="true" AllowPaging="true" PagerSettings-Mode="Numeric" GridLines="None" AutoGenerateColumns="false" EmptyDataText="<%$ Resources:EmptyDataMessage %>" EmptyDataRowStyle-CssClass="noData">
                                <Columns>
                                    <asp:BoundField DataField="PFPremiumFinanceVersionKey" HeaderText="<%$ Resources:grdbf_Version %>" SortExpression="PFPremiumFinanceVersionKey"></asp:BoundField>
                                    <asp:BoundField DataField="PFPremiumFinanceKey" HeaderText="<%$ Resources:grdbf_PlanReference %>" SortExpression="PFPremiumFinanceKey"></asp:BoundField>
                                    <asp:BoundField DataField="StartDate" HeaderText="<%$ Resources:grdbf_Date %>" DataFormatString="{0:d}" SortExpression="StartDate"></asp:BoundField>
                                    <asp:BoundField DataField="StatusInd" HeaderText="<%$ Resources:grdbf_Status %>" SortExpression="StatusInd"></asp:BoundField>
                                    <asp:BoundField DataField="FinanceAmount" HeaderText="<%$ Resources:grdbf_FinancedAmount %>" SortExpression="FinanceAmount"></asp:BoundField>
                                    <asp:BoundField DataField="TotalCost" HeaderText="<%$ Resources:grdbf_TotalAmount %>" SortExpression="TotalCost"></asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnSelect" runat="server" Text="<%$ Resources:btnSelect %>" CommandName="Select" SkinID="btnGrid" CommandArgument='<%# Container.DataItemIndex %>' CausesValidation="false"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <Nexus:ProgressIndicator ID="upPlanHistory" OverlayCssClass="updating" AssociatedUpdatePanelID="updPlanHistory" runat="server">
                    <ProgressTemplate>
                    </ProgressTemplate>
                </Nexus:ProgressIndicator>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:btnOk %>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
