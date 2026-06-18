<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CoInsuranceRecovery.ascx.vb"
    Inherits="Nexus.Controls_CoInsuranceRecovery" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<div id="Controls_CoInsuranceRecovery">
    <div class="card card-secondary">
        <asp:Panel ID="pnlCoInsuranceGrid" runat="server" visible="false">
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblCoInsuranceRecovery" runat="server" Text="<%$ Resources:lbl_CoInsuranceRecovery %>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblClaimNumber" runat="server" AssociatedControlID="txtClaimNumber" Text="<%$ Resources:lbl_ClaimNumber %>"
                            class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtClaimNumber" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCoinTreatment" runat="server" AssociatedControlID="GISLookup_Type" Text="<%$ Resources:lbl_CoinTreatment %>"
                            class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="GISLookup_Type" runat="server" DataItemValue="Code" Enabled="false" DataItemText="Description"
                                Sort="ASC" ListType="PMLookup" ListCode="CoInsurance_Treatment" CssClass="form-control form-select" AutoPostBack ="true"></NexusProvider:LookupList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblTotalShare" runat="server" Text="<%$ Resources:lbl_TotalShare %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtTotalShare" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCurrentShareValue" runat="server" Text="<%$ Resources:lbl_CurrentShareValue %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtTotalCurrentShareValue" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
       <%-- </asp:Panel>--%>

        <%-- Normal coinsurance recovery grid --%>
       <%-- <asp:Panel ID="pnlCoInsuranceGrid" runat="server" Visible="true">--%>
            <div class="grid-card table-responsive">
                <asp:GridView ID="gvCoInsurerBreakdown" runat="server" AutoGenerateColumns="False" GridLines="None"
                    EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:lbl_ErrorMessage_CoInsurer %>" CssClass="noborder">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="<%$ Resources:lbl_CoinsurerName %>"></asp:BoundField>
                        <Nexus:BoundField DataField="Share" DataType="Percentage"   HeaderText="<%$ Resources:lbl_SharePerc %>"></Nexus:BoundField>
                       <Nexus:BoundField DataField="ShareValue" DataType="Currency" HeaderText="<%$ Resources:lbl_ShareValue %>"></Nexus:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>

        <%-- Salvage and TP recovery grids --%>
        <asp:Panel ID="pnlRecoveryGrids" runat="server" Visible="false">
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblSalvageRecovery" runat="server" Text="<%$ Resources:lbl_SalvageRecovery %>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRecoveryTypeFilter" runat="server" AssociatedControlID="ddlRecoveryTypeFilter" Text="<%$ Resources:lbl_RecoveryTypeFilter %>"
                            class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlRecoveryTypeFilter" runat="server" CssClass="form-control form-select" AutoPostBack="true"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRecoveryAmount" runat="server" AssociatedControlID="txtRecoveryAmount" Visible="false" Text="<%$ Resources:lbl_RecoveryAmount %>"
                            class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="txtRecoveryAmount" Enabled="false" CssClass="form-control" Visible="false" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="grid-card table-responsive">
                <asp:GridView ID="gvSalvageRecovery" runat="server" AutoGenerateColumns="False" GridLines="None"
                    EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:lbl_ErrorMessage_Salvage %>" CssClass="noborder">
                    <Columns>
                        <asp:BoundField DataField="PerilDescription" HeaderText="<%$ Resources:lbl_PerilDescription %>"></asp:BoundField>
                        <asp:BoundField DataField="RecoveryType" HeaderText="<%$ Resources:lbl_RecoveryType_Grid %>"></asp:BoundField>
                        <asp:BoundField DataField="Coinsurer" HeaderText="<%$ Resources:lbl_Coinsurer %>"></asp:BoundField>
                        <Nexus:BoundField DataField="SharePercent" HeaderText="<%$ Resources:lbl_SharePercent %>" DataType="Percentage"></Nexus:BoundField>
                        <Nexus:BoundField DataField="RecoveryToDate" HeaderText="<%$ Resources:lbl_RecoveryToDate %>" DataType="Currency"></Nexus:BoundField>
                        <Nexus:BoundField DataField="TotalThisRecovery" DataType="Currency" HeaderText="<%$ Resources:lbl_TotalThisRecovery %>"></Nexus:BoundField>
                        <Nexus:BoundField DataField="ThisRecovery" DataType="Currency" HeaderText="<%$ Resources:lbl_ThisRecoverySplit %>"></Nexus:BoundField>
                    </Columns>
                </asp:GridView>
            </div>          
        </asp:Panel>
    </div>
</div>
