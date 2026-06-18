<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Perils.ascx.vb" Inherits="Nexus.Controls_Perils" %>
<%@ Register TagPrefix="OC" Namespace="Nexus" %>
<div id="Controls_Perils">
    <div class="card card-secondary">
        <div class="card-heading">
            <h1>
                <asp:Literal runat="server" Text="<%$ Resources:Perils_pageheading %>" ID="ltPageHeading"></asp:Literal>
            </h1>
        </div>
        <div class="card-body clearfix">
            <asp:Literal runat="server" Text="<%$ Resources:Perils_ViewReservesMessage %>" ID="ltViewReservesMessage"></asp:Literal>
            <div class="grid-card table-responsive">
                <asp:GridView ID="grdvPerils" runat="server" AutoGenerateColumns="false" GridLines="None" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>">
                    <Columns>
                        <asp:BoundField HeaderText="<%$ Resources:lbl_grdvPerils_Description_heading %>" DataField="Description" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                        <Nexus:TemplateField HeaderText="<%$ Resources:lbl_grdvPerils_SumInsured_heading %>" DataType="Currency">
                            <itemtemplate>
                                    <asp:Label ID="lblSumInsured" runat="server"></asp:Label>
                                </itemtemplate>
                        </Nexus:TemplateField>
                        <Nexus:TemplateField HeaderText="<%$ Resources:lbl_grdvPerils_ReserveTotal_heading %>" DataType="Currency">
                            <itemtemplate>
                                    <asp:Label ID="lblReserveTotal" runat="server"></asp:Label>
                                </itemtemplate>
                        </Nexus:TemplateField>
                        <Nexus:TemplateField HeaderText="<%$ Resources:lbl_grdvPerils_AmountPaid_heading %>" DataType="Currency">
                            <itemtemplate>
                                    <asp:Label ID="lblAmountPaid" runat="server"></asp:Label>
                                </itemtemplate>
                        </Nexus:TemplateField>
                        <Nexus:TemplateField HeaderText="<%$ Resources:lbl_grdvPerils_CurrentReserves_heading %>" DataType="Currency">
                            <itemtemplate>
                                    <asp:Label ID="lblCurrentReserves" runat="server"></asp:Label>
                                </itemtemplate>
                        </Nexus:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <div class="rowMenu">
                                    <ol class="list-inline no-margin">
                                        <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                            <ol id="menu_<%# Eval("ClaimKey") %>" class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                <li>
                                                    <asp:HyperLink ID="lnkReserves" runat="server" Text="<%$ Resources:lbl_grdvPerils_linkReserves_text %>"></asp:HyperLink>
                                                </li>
                                                <li>
                                                    <asp:HyperLink ID="lnkRecoveries" Visible="false" runat="server" Text="<%$ Resources:lbl_grdvPerils_linkRecoveries_text %>"></asp:HyperLink>
                                                </li>
                                                <li id="liSalvageClaim" runat="server" visible="false">
                                                    <asp:LinkButton ID="lnkSalvageClaim" runat="server" Text="<%$ Resources:lbl_grdvPerils_linkSalvage_text %>" Visible="false"></asp:LinkButton>
                                                </li>
                                                <li id="liTPRecovery" runat="server" visible="false">
                                                    <asp:LinkButton ID="lnkTPRecovery" runat="server" Text="<%$ Resources:lbl_grdvPerils_linkTPRecovery_text %>" Visible="false"></asp:LinkButton>
                                                </li>
                                            </ol>
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
    <asp:CustomValidator ID="IsPaymentReceived" runat="server" CssClass="error" ErrorMessage="<%$ Resources:lbl_PaymentReceived_Error %>" Display="none"></asp:CustomValidator>
    <asp:CustomValidator ID="IsValidReserve" runat="server" Display="none"></asp:CustomValidator>
    <asp:HiddenField ID="hidChkChoice" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hidChlClaimClose" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hidChkPaymentMsg" runat="server"></asp:HiddenField>
</div>
