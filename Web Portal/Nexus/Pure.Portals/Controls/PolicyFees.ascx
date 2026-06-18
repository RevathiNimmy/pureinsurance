<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PolicyFees.ascx.vb" Inherits="Nexus.Controls_PolicyFees"
    EnableViewState="false" %>
<div id="Controls_PolicyFees">
    <asp:Panel ID="FeeAmountPanel" runat="server">
        <asp:Label ID="lblFeeDisplay" Font-Bold="true" runat="server" AssociatedControlID="lblFeeValue">
            <asp:Literal ID="litFeeDisplay" runat="server" Text="Total Fee"></asp:Literal></asp:Label>

        <asp:Label ID="lblFeeValue" runat="server"></asp:Label>
    </asp:Panel>
    <div id="policyfees_control" runat="server">
        <div class="card card-secondary">
            <div class="card-heading">
                <h4>
                    <asp:Label ID="lblPolicyFees" runat="server" Text="<%$ Resources:lbl_PolicyFees %>"></asp:Label></h4>
            </div>
            <div class="card-body clearfix">
                <div class="grid-card table-responsive no-margin">
                    <asp:GridView ID="grdvPolicyFees" runat="server" AllowPaging="false" PagerSettings-Mode="Numeric" AutoGenerateColumns="False" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                        <Columns>
                            <asp:BoundField DataField="FeeName" HeaderText="<%$ Resources:lbl_FeeName_g %>"></asp:BoundField>
                            <asp:BoundField DataField="CurrencyCode" HeaderText="<%$ Resources:lbl_CurrencyCode_g %>"></asp:BoundField>
                            <Nexus:BoundField DataField="Premium" HeaderText="<%$ Resources:lbl_Premium_g %>" DataType="Currency"></Nexus:BoundField>
                            <Nexus:TemplateField HeaderText="<%$ Resources:lbl_Rate_g %>" DataType="Currency">
                                <itemtemplate>
                                        <asp:Label ID="lblRate" runat="server"></asp:Label>
                                    </itemtemplate>
                            </Nexus:TemplateField>
                            <%-- <asp:BoundField DataField="Rate" HeaderText="<%$ Resources:lbl_Rate_g %>" DataFormatString="{0:N2}%" />--%>
                            <Nexus:BoundField DataField="FeeAmount" HeaderText="<%$ Resources:lbl_FeeAmount_g %>" DataType="Currency"></Nexus:BoundField>
                            <Nexus:BoundField DataField="TaxAmount" HeaderText="<%$ Resources:lbl_TaxAmount_g %>" DataType="Currency"></Nexus:BoundField>
                            <Nexus:BoundField DataField="TotalAmount" HeaderText="<%$ Resources:lbl_TotalAmount_g %>" DataType="Currency"></Nexus:BoundField>
                            <asp:BoundField DataField="TaxGroup" HeaderText="<%$ Resources:lbl_TotalGroup_g %>"></asp:BoundField>
                            <asp:BoundField DataField="IncludeInInstallment" HeaderText="<%$ Resources:lbl_FeeInInstalment_g %>" Visible="false"></asp:BoundField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id='menu_<%# Eval("PolicyFeeKey") %>' class="list-inline no-margin">
                                            <li>
                                                <asp:LinkButton ID="hypPolicyFeeEdit" runat="server" Text="<%$ Resources:lbl_Edit %>" SkinID="btnGrid"></asp:LinkButton>
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
