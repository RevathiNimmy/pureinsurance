<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AgentCommission.ascx.vb"
    Inherits="Nexus.Controls_AgentCommission" EnableViewState="false" %>

<script language="javascript" type="text/javascript">

    function pageLoad() {
        //this is needed if the trigger is external to the update panel   
        var manager = Sys.WebForms.PageRequestManager.getInstance();
        manager.add_beginRequest(OnBeginRequest);
    }

    function OnBeginRequest(sender, args) {
        var postBackElement = args.get_postBackElement();
        if (postBackElement.id == 'hypAgentCommEdit') {
            $get(uprogQuotes).style.display = "block";
        }
    }

</script>

<div id="Controls_AgentCommission">
    <asp:UpdatePanel ID="UpdAgentComm" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
        <ContentTemplate>
            <asp:Panel ID="AgentAmountPanel" runat="server">
                <asp:Label ID="lblAgentDisplay" Font-Bold="true" runat="server" AssociatedControlID="lblAgentCommissionValue">
                    <asp:Literal ID="litAgentDisplay" runat="server" Text="Total Agent Commission"></asp:Literal></asp:Label>
                <asp:Label ID="lblAgentCommissionValue" runat="server"></asp:Label>
            </asp:Panel>
            <div id="agentcommission_control" runat="server">
                <div class="card card-secondary">
                    <div class="card-heading">
                        <h4>

                            <asp:Label ID="lblPolicyFees" runat="server" Text="<%$ Resources:lbl_AgentCommission %>"></asp:Label>
                        </h4>
                    </div>
                    <div class="card-body clearfix">
                        <div class="grid-card table-responsive">
                            <asp:GridView ID="grdvAgentCommission" runat="server" AllowPaging="True" PagerSettings-Mode="Numeric" AutoGenerateColumns="False" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                <Columns>
                                    <asp:BoundField DataField="Agent" HeaderText="<%$ Resources:lbl_Agent_g %>"></asp:BoundField>
                                    <asp:BoundField DataField="AgentType" HeaderText="<%$ Resources:lbl_AgentType_g %>"></asp:BoundField>
                                    <asp:BoundField DataField="RiskType" HeaderText="<%$ Resources:lbl_RiskType_g %>"></asp:BoundField>
                                    <asp:BoundField DataField="CommissionBand" HeaderText="<%$ Resources:lbl_CommissionBand_g %>"></asp:BoundField>
                                    <nexus:BoundField DataField="Premium" HeaderText="<%$ Resources:lbl_Premium_g %>" DataType="Currency"></nexus:BoundField>
                                    <nexus:BoundField HeaderText="<%$ Resources:lbl_Commissionrate_g %>" DataType="Currency"></nexus:BoundField>
                                    <asp:TemplateField HeaderText="<%$ Resources:lbl_IsLeadAgent_g %>" Visible="false">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="IsLeadAgent" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <nexus:BoundField DataField="CommissionValue" HeaderText="<%$ Resources:lbl_CommissionValue_g %>" DataType="Currency"></nexus:BoundField>
                                    <asp:BoundField DataField="TaxGroupDescription" HeaderText="<%$ Resources:lbl_TaxGroup_g %>"></asp:BoundField>
                                    <nexus:BoundField DataField="TaxValue" HeaderText="<%$ Resources:lbl_TaxValue_g %>" DataType="Currency"></nexus:BoundField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <div class="rowMenu">
                                                <ol id="menu_<%# Eval("AgentType") %>" class="list-inline no-margin">
                                                    <li>
                                                        <asp:LinkButton ID="hypAgentCommEdit" runat="server" Text="<%$ Resources:lbl_Edit %>" Visible="false" SkinID="btnGrid"></asp:LinkButton>
                                                    </li>
                                                </ol>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <nexus:ProgressIndicator ID="upAgentCommission" OverlayCssClass="updating" AssociatedUpdatePanelID="UpdAgentComm" runat="server">
        <ProgressTemplate>
        </ProgressTemplate>
    </nexus:ProgressIndicator>
</div>
