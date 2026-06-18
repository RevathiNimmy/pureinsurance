<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RiskTax.ascx.vb" Inherits="Nexus.Controls_RiskTax"
    EnableViewState="false" %>
<div id="Controls_RiskTax">
    <div id="risktax_control" runat="server">
        <div class="card card-secondary">
            <div class="card-heading">
                <h4>
                    <asp:Label ID="lblRiskTax" runat="server" Text="<%$ Resources:lbl_RiskTax %>"></asp:Label></h4>
            </div>
            <div class="card-body clearfix">
                <div class="grid-card table-responsive no-margin">
                    <asp:GridView ID="grdvRiskTax" runat="server" AllowPaging="false" PagerSettings-Mode="Numeric" AutoGenerateColumns="False" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                        <Columns>
                            <asp:BoundField DataField="TaxGroup" HeaderText="<%$ Resources:lbl_Taxgroup_g %>"></asp:BoundField>
                            <asp:BoundField DataField="Sequence" HeaderText="<%$ Resources:lbl_Sequence_g %>"></asp:BoundField>
                            <asp:BoundField DataField="TaxBand" HeaderText="<%$ Resources:lbl_Taxband_g %>"></asp:BoundField>
                            <asp:BoundField DataField="TaxAmount" HeaderText="<%$ Resources:lbl_Taxamount_g %>" DataFormatString="{0:N2}"></asp:BoundField>
                            <asp:BoundField DataField="CalculationBasis" HeaderText="<%$ Resources:lbl_CalculationBasics_g %>"></asp:BoundField>
                            <%--<asp:BoundField DataField="Rate" HeaderText="<%$ Resources:lbl_Rate_g %>" DataFormatString="{0:N2}%" />--%>
                            <Nexus:BoundField DataField="Rate" HeaderText="<%$ Resources:lbl_Rate_g %>" DataType="Currency"></Nexus:BoundField>
                            <asp:BoundField DataField="ClassOfBusiness" HeaderText="<%$ Resources:lbl_ClassOfBusiness_g %>"></asp:BoundField>
                            <asp:BoundField DataField="Country" HeaderText="<%$ Resources:lbl_Country_g %>"></asp:BoundField>
                            <asp:BoundField DataField="State" HeaderText="<%$ Resources:lbl_State_g %>"></asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_AppliedToClient_g %>">
                                <ItemTemplate>
                                    <asp:CheckBox ID="IsNotAppliedToClient" Enabled="false" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_IncluedTaxInstalment_g %>">
                                <ItemTemplate>
                                    <asp:CheckBox ID="IncludeInInstallment" Enabled="false" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_SpreadTaxAcrossInstalment_g %>">
                                <ItemTemplate>
                                    <asp:CheckBox ID="SpreadAcrossInstallment" Enabled="false" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ApplyTaxBy" HeaderText="<%$ Resources:lbl_ApplyTax_g %>"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</div>
