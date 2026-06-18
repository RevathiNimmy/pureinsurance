<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RiskFees.ascx.vb" Inherits="Nexus.Controls_RiskFees"
    EnableViewState="false" %>
<div id="Controls_RiskFees">
    <div id="riskfees_control" runat="server">
        <div class="card card-secondary">
            <div class="card-heading">
                <h4><span id="lgRisk" runat="server">
                    <asp:Literal ID="litRiskFees" runat="server" Text="<%$ Resources:lbl_RiskFees %>"></asp:Literal></span></h4>
            </div>
            <div class="card-body clearfix">
                <div class="grid-card table-responsive no-margin">
                    <asp:GridView ID="grdvRiskFees" runat="server" AllowPaging="false" PagerSettings-Mode="Numeric" AutoGenerateColumns="False" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                        <Columns>
                            <asp:BoundField DataField="FeeName" HeaderText="<%$ Resources:lbl_FeeName_g %>"></asp:BoundField>
                            <asp:BoundField DataField="CurrencyCode" HeaderText="<%$ Resources:lbl_CurrencyCode_g %>"></asp:BoundField>
                            <asp:BoundField DataField="FeeAppliedTo" HeaderText="<%$ Resources:lbl_AppliedTo_g %>"></asp:BoundField>
                            <Nexus:BoundField DataField="Premium" HeaderText="<%$ Resources:lbl_Premium_g %>" DataType="Currency"></Nexus:BoundField>
                            <Nexus:TemplateField HeaderText="<%$ Resources:lbl_Rate_g %>" DataType="Currency">
                                <itemtemplate>
                                        <asp:Label ID="lblRate" runat="server"></asp:Label>
                                    </itemtemplate>
                            </Nexus:TemplateField>
                            <Nexus:BoundField DataField="FeeAmount" HeaderText="<%$ Resources:lbl_FeeAmount_g %>" DataType="Currency"></Nexus:BoundField>
                            <Nexus:BoundField DataField="TaxAmount" HeaderText="<%$ Resources:lbl_TaxAmount_g %>" DataType="Currency"></Nexus:BoundField>
                            <Nexus:BoundField DataField="TotalAmount" HeaderText="<%$ Resources:lbl_TotalAmount_g %>" DataType="Currency"></Nexus:BoundField>
                            <asp:BoundField DataField="TaxGroup" HeaderText="<%$ Resources:lbl_TotalGroup_g %>"></asp:BoundField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_FeeInInstalment_g %>">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkIncludeInInstallment" Enabled="false" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:lbl_SpreadAcrossInstallment_g %>">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSpreadAcrossInstallment" Enabled="false" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id='menu_<%# Eval("RiskFeeKey") %>' class="list-inline no-margin">
                                            <li>
                                                <asp:LinkButton ID="hypRiskFeeEdit" runat="server" Text="<%$ Resources:lbl_Edit %>" SkinID="btnGrid"></asp:LinkButton>
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
</div>
