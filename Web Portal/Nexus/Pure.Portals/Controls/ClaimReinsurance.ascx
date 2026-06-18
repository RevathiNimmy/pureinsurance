<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ClaimReinsurance.ascx.vb"
    Inherits="Nexus.Controls_ClaimReinsurance" %>
<div id="Controls_Reinsurance">
        <div class="card-body no-padding clearfix">
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblReinsuranceMain" runat="server" Text="<%$ Resources:lbl_Reinsurance %>"></asp:Label></legend>

                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblReinsuranceBand" runat="server" AssociatedControlID="litReinsuranceBand" class="col-md-4 col-sm-3 control-label">
                        <asp:Literal ID="litReinsuranceBand" runat="server" Text="<%$ Resources:lblReinsuranceBand %>"></asp:Literal></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="ddlReinsurance" runat="server" AutoPostBack="True" CssClass="form-control form-select">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <asp:UpdatePanel runat="server" ID="updClaimReinsurance" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="grid-card table-responsive">
                    <asp:GridView ID="gvClaimReinsurance" runat="server" AllowPaging="false" AutoGenerateColumns="False" GridLines="None" PageSize="5" ShowHeader="True" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="<%$ Resources:lblName %>" HeaderStyle-CssClass="str" ItemStyle-CssClass="str"></asp:BoundField>
                            <Nexus:BoundField DataField="DefaultPerc" HeaderText="<%$ Resources:lblDefault %>" HeaderStyle-CssClass="Perc" ItemStyle-CssClass="Perc" DataType="Percentage"></Nexus:BoundField>
                            <Nexus:BoundField DataField="ThisPerc" HeaderText="<%$ Resources:lblThis %>" HeaderStyle-CssClass="Perc" ItemStyle-CssClass="Perc" DataType="Percentage"></Nexus:BoundField>
                            <Nexus:BoundField DataField="SumInsured" HeaderText="<%$ Resources:lblSumInsured %>" DataType="Currency"></Nexus:BoundField>
                            <Nexus:BoundField DataField="ReserveToDate" HeaderText="<%$ Resources:lblReserveToDate %>" DataType="Currency"></Nexus:BoundField>
                            <Nexus:BoundField DataField="PaymentToDate" HeaderText="<%$ Resources:lblPaymentToDate %>" DataType="Currency"></Nexus:BoundField>
                            <Nexus:BoundField DataField="ThisPayment" Visible="false" HeaderText="<%$ Resources:lblThisPayment %>" DataType="Currency"></Nexus:BoundField>
                            <Nexus:BoundField DataField="ThisReserve" HeaderText="<%$ Resources:lblThisReserve %>" DataType="Currency"></Nexus:BoundField>
                            <Nexus:BoundField DataField="Balance" HeaderText="<%$ Resources:lblBalance %>" DataType="Currency"></Nexus:BoundField>
                            <asp:BoundField DataField="Agreement" HeaderText="<%$ Resources:lblAgreement %>" HeaderStyle-CssClass="str" ItemStyle-CssClass="str"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="gvClaimReinsurance" EventName="Load"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="gvClaimReinsurance" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <Nexus:ProgressIndicator ID="upClaimReinsurance" OverlayCssClass="updating" AssociatedUpdatePanelID="updClaimReinsurance" runat="server">
            <progresstemplate>
                    </progresstemplate>
        </Nexus:ProgressIndicator>

        <asp:HiddenField ID="hidChkChoice" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidChlClaimClose" runat="server"></asp:HiddenField>
        <asp:HiddenField ID="hidChkPaymentMsg" runat="server"></asp:HiddenField>
        <div class="card-footer no-padding">
            <asp:LinkButton ID="btnNext" runat="server" Text="<%$ Resources:ClaimsResource, claimoverview_btnNext %>" SkinID="btnPrimary"></asp:LinkButton>
        </div>
</div>
