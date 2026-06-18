<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PolicyTax.ascx.vb" Inherits="Nexus.Controls_PolicyTax"
    EnableViewState="false" %>

<div id="Controls_PolicyTax">
    <asp:Panel ID="TaxAmountPanel" runat="server">
        <asp:Label ID="lblTaxDisplay" Font-Bold="true" runat="server" AssociatedControlID="lblTaxValue">
            <asp:Literal ID="litTaxDisplay" runat="server" Text="Total Tax"></asp:Literal></asp:Label>
        <asp:Label ID="lblTaxValue" runat="server"></asp:Label>
    </asp:Panel>
    <div id="policytax_control" runat="server">
        <div class="card card-secondary">
            <div class="card-heading">
                <h4>
                    <asp:Label ID="lblPolicyTax" runat="server" Text="<%$ Resources:lbl_PolicyTax %>"></asp:Label></h4>
            </div>
            <div class="card-body clearfix">
                <div class="grid-card table-responsive no-margin">
                    <asp:GridView ID="grdvPolicyTax" runat="server" AllowPaging="false" PagerSettings-Mode="Numeric" AutoGenerateColumns="False" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                        <Columns>
                            <asp:BoundField DataField="TaxGroup" HeaderText="<%$ Resources:lbl_Taxgroup_g %>"></asp:BoundField>
                            <Nexus:BoundField DataField="Sequence" HeaderText="<%$ Resources:lbl_Sequence_g %>" DataType="Number"></Nexus:BoundField>
                            <asp:BoundField DataField="TaxBand" HeaderText="<%$ Resources:lbl_Taxband_g %>"></asp:BoundField>
                            <Nexus:BoundField DataField="TaxAmount" HeaderText="<%$ Resources:lbl_Taxamount_g %>" DataType="Currency"></Nexus:BoundField>
                            <asp:BoundField DataField="CalculationBasis" HeaderText="<%$ Resources:lbl_CalculationBasics_g %>"></asp:BoundField>
                            <asp:BoundField DataField="Rate" HeaderText="<%$ Resources:lbl_Rate_g %>"></asp:BoundField>
                            <asp:BoundField DataField="ClassOfBusiness" HeaderText="<%$ Resources:lbl_ClassOfBusiness_g %>"></asp:BoundField>
                            <asp:BoundField DataField="Country" HeaderText="<%$ Resources:lbl_Country_g %>"></asp:BoundField>
                            <asp:BoundField DataField="State" HeaderText="<%$ Resources:lbl_State_g %>"></asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_AppliedToClient_g %>" Visible="false">
                                <ItemTemplate>
                                    <asp:CheckBox ID="IsNotAppliedToClient" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_IncluedTaxInstalment_g %>" Visible="false">
                                <ItemTemplate>
                                    <asp:CheckBox ID="IncludeInInstallment" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_SpreadTaxAcrossInstalment_g %>" Visible="false">
                                <ItemTemplate>
                                    <asp:CheckBox ID="SpreadAcrossInstallment" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ApplyTaxBy" HeaderText="<%$ Resources:lbl_ApplyTax_g %>" Visible="false"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</div>
