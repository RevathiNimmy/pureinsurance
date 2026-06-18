<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SelectRiskType.ascx.vb"
    Inherits="Nexus.Controls_SelectRiskType" EnableViewState="true" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>

<script language="javascript" type="text/javascript">
</script>

<div id="Controls_SelectRiskType">
    <div class="card">
        <div class="card-body clearfix">
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblHeading" runat="server" Text="<%$ Resources:lbl_Heading %>"></asp:Label></legend>
                <asp:ScriptManager ID="ScriptManagerMainDetails" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="updSelectRiskType" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True">
                    <ContentTemplate>
                        <div class="grid-card table-responsive">
                            <asp:GridView ID="grdvSelectRiskType" runat="server" AllowPaging="true" AutoGenerateColumns="False" GridLines="None" PagerSettings-Mode="Numeric" PageSize="10" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="<%$ Resources:lbl_Name %>"></asp:BoundField>
                                    <asp:BoundField DataField="RiskCode" HeaderText="<%$ Resources:lbl_RiskCode %>">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DataModelCode" HeaderText="<%$ Resources:lbl_DataModelCode %>">
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Path" HeaderText="<%$ Resources:lbl_Path %>"></asp:BoundField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <div class="rowMenu">
                                                <ol id="menu_<%# Eval("RiskCode") %>" Class="list-inline no-margin">
                                                    <li>
                                                        <asp:LinkButton ID="lnkbtnSelect" runat="server" CausesValidation="False" CommandName="Select" Text="<%$ Resources:lbl_Select %>" SkinID="btnGrid">
                                                        </asp:LinkButton>
                                                    </li>
                                                </ol>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:Button ID="btnHiddenSelectRiskType" runat="server" Style="display: none;" CausesValidation="false"></asp:Button>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="grdvSelectRiskType" EventName="Load"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="grdvSelectRiskType" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
                        <asp:AsyncPostBackTrigger ControlID="grdvSelectRiskType" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                    </Triggers>
                </asp:UpdatePanel>
                <nexus:ProgressIndicator ID="upSelectRiskType" OverlayCssClass="updating" AssociatedUpdatePanelID="updSelectRiskType" runat="server">
                    <ProgressTemplate>
                    </ProgressTemplate>
                </nexus:ProgressIndicator>
            </div>
        </div>
        
    </div>
</div>
