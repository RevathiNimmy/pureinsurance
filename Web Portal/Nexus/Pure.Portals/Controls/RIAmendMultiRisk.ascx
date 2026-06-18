<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RIAmendMultiRisk.ascx.vb"
    Inherits="Nexus.Controls_RIAmendMultiRisk" %>
<div id="Controls_MultiRisk">
    <div class="card">
        <div class="card-body clearfix">
            
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblMultiRisk" runat="server" Text="<%$ Resources:lbl_MultiRisk %>"></asp:Label></legend>
                <div class="grid-card table-responsive">
                    <asp:GridView ID="gvRisk" runat="server" AllowPaging="true" DataKeyNames="Key,RiskNumber" AutoGenerateColumns="False" GridLines="None" PagerSettings-Mode="Numeric" PageSize="5" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                        <Columns>
                            <nexus:BoundField DataField="RiskNumber" HeaderText="Risk Number" DataType="Number">
                            </nexus:BoundField>
                            <asp:BoundField DataField="Description" HeaderText="<%$ Resources:lbl_RiskDescription %>"></asp:BoundField>
                            <nexus:BoundField DataField="TotalSumInsured" HeaderText="<%$ Resources:lbl_SumInsured %>" DataType="Currency">
                            </nexus:BoundField>
                            <nexus:BoundField DataField="Premium" HeaderText="<%$ Resources:lbl_Premium %>" DataType="Currency"></nexus:BoundField>
                            <asp:BoundField DataField="StatusCode" HeaderText="<%$ Resources:lbl_RiskStatus %>"></asp:BoundField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <div class="rowMenu">
                                        <ol id="menu_<%# Eval("RiskNumber") %>" Class="list-inline no-margin">
                                            <li>
                                                <asp:LinkButton ID="lnkbtnEditReinsurance" runat="server" CausesValidation="False" CommandName="EditRI" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Key") %>' Text="<%$ Resources:lbl_EditReinsurance %>" SkinID="btnGrid">
                                                </asp:LinkButton>
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
