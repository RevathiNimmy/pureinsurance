<%@ Page Title="" Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="BackDatedMTA.aspx.vb" Inherits="BackDatedMTA" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div class="card">
        <div class="card-heading">
            <h1>
                <asp:Literal ID="lblHeading" runat="server" Text="<%$ Resources:lblHeading %>"></asp:Literal></h1>
        </div>
        <div class="card-body clearfix">
            <legend>
                <asp:Label ID="lblPageTitle" runat="server" Text="<%$ Resources:lblHeading %>"></asp:Label></legend>
            <div class="grid-card table-responsive">
                <asp:GridView ID="grdBackdatedMTA" runat="server" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="noData" EmptyDataText="<%$ Resources:ErrorMessage %>" DataKeyNames="PrechangeInsuranceFileKey" ShowFooter="true">
                    <Columns>
                        <asp:BoundField DataField="PolicyTypeDescription" HeaderText="<%$ Resources:lbl_grdBackdatedMTA_PolicyType_heading %>"></asp:BoundField>
                        <asp:BoundField DataField="CoverStartDate" HeaderText="<%$ Resources:lbl_grdBackdatedMTA_CoverStartDate_heading %>" HtmlEncode="false" DataFormatString="{0:d}"></asp:BoundField>
                        <asp:BoundField DataField="CoverEndDate" HeaderText="<%$ Resources:lbl_grdBackdatedMTA_CoverEndDate_heading %>" HtmlEncode="false" DataFormatString="{0:d}"></asp:BoundField>
                        <asp:BoundField DataField="QuoteStatus" HeaderText="<%$ Resources:lbl_grdBackdatedMTA_Status_heading %>"></asp:BoundField>
                        <Nexus:BoundField DataField="OriginalMtaPremium" HeaderText="<%$ Resources:lbl_grdBackdatedMTA_GrossOriginalPremium_heading %>" DataType="Currency"></Nexus:BoundField>
                        <Nexus:BoundField DataField="Premium" HeaderText="<%$ Resources:lbl_grdBackdatedMTA_GrossEndorsementPremium_heading %>" DataType="Currency"></Nexus:BoundField>
                        <Nexus:BoundField DataField="OriginalCommission" HeaderText="<%$ Resources:lbl_grdBackdatedMTA_GrossOriginalCommission_heading %>" DataType="Currency"></Nexus:BoundField>
                        <Nexus:BoundField DataField="MtaCommission" HeaderText="<%$ Resources:lbl_grdBackdatedMTA_GrossEndorsementCommission_heading %>" DataType="Currency"></Nexus:BoundField>
                        <Nexus:BoundField DataField="OriginalFee" HeaderText="<%$ Resources:lbl_grdBackdatedMTA_GrossOriginalFees_heading %>" DataType="Currency"></Nexus:BoundField>
                        <Nexus:BoundField DataField="MtaFee" HeaderText="<%$ Resources:lbl_grdBackdatedMTA_GrossEndorsementFees_heading %>" DataType="Currency"></Nexus:BoundField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <div class="rowMenu">
                                    <ol class="list-inline no-margin">
                                        <li class="dropdown no-padding"><a href="#" title="Action Menu" md-ink-ripple="" data-bs-toggle="dropdown" class="md-btn grey-100 md-flat md-btn-circle"><i class="fa fa-ellipsis-v" aria-hidden="true"></i></a>
                                            <ol id='menu_<%# Eval("PolicyVersion") %>' class="dropdown-menu dropdown-menu-scale pull-right pull-up top text-color">
                                                <li>
                                                    <asp:LinkButton ID="lnkbtnEdit" runat="server" CausesValidation="False" CommandName="edit" Text="<%$ Resources:lnkbtnEdit %>"></asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lnkbtnView" runat="server" CausesValidation="False" CommandName="view" Text="<%$ Resources:lnkbtnView %>"></asp:LinkButton>
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
        <div class="card-footer">
            <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:lblbtnCancel%>" SkinID="btnSecondary"></asp:LinkButton>
            <asp:LinkButton ID="btnOK" runat="server" Text="<%$ Resources:lblbtnOK %>" SkinID="btnPrimary"></asp:LinkButton>
            <asp:LinkButton ID="btnSaveOOSMTAQuote" runat="server" Text="<%$ Resources:lblbtnSaveOOSMTAQuote %>" SkinID="btnPrimary"></asp:LinkButton>
        </div>
    </div>
</asp:Content>
