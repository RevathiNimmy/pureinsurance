<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="FinancialDetails.aspx.vb" Inherits="Nexus.Claims_FinancialDetails"
    Title="Financial Details" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ScriptIncludes" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="Claims_FinancialDetails">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Label runat="server" Text="<%$ Resources:lblPageHeading %>" ID="lblPageHeading"></asp:Label></h1>
            </div>
            <div class="card-body clearfix">
                <div class="md-whiteframe-z0 bg-white">
                    <ul class="nav nav-lines nav-tabs b-danger">
                        <li><a href="#tab-total" data-bs-toggle="tab" aria-expanded="true"  class="active">
                            <asp:Literal ID="lblTotalHeading" runat="server" Text="<%$ Resources:lbl_TotalHeading %>"></asp:Literal></a></li>
                        <li><a href="#tab-reservetypes" data-bs-toggle="tab" aria-expanded="true">
                            <asp:Literal ID="lblMainHeading" runat="server" Text="<%$ Resources:lbl_MainHeading %>"></asp:Literal></a></li>
                        <li><a href="#tab-tprecovery" data-bs-toggle="tab" aria-expanded="true">
                            <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:lbl_TPRecoveryHeading %>"></asp:Literal></a></li>
                        <li><a href="#tab-salvage" data-bs-toggle="tab" aria-expanded="true">
                            <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:lbl_SalvageHeading %>"></asp:Literal></a></li>
                        <li><a href="#tab-payment" data-bs-toggle="tab" aria-expanded="true">
                            <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:lbl_PaymentHeading %>"></asp:Literal></a></li>
                        <li><a href="#tab-receipt" data-bs-toggle="tab" aria-expanded="true">
                            <asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:lbl_ReceiptHeading %>"></asp:Literal></a></li>
                    </ul>
                    <div class="tab-content clearfix p b-t b-t-2x">
                        <div id="tab-total" class="tab-pane animated fadeIn active" role="tabpanel">
                           <asp:UpdatePanel ID="UpdateTotal" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlTotal" runat="server">
                                                        <div class="table-wrapper">
                                                            <asp:GridView ID="gvTotal" runat="server" AllowPaging="false" PagerSettings-Mode="Numeric"
                                                                AutoGenerateColumns="False" GridLines="None" EmptyDataRowStyle-CssClass="noData"
                                                                EmptyDataText="<%$ Resources:ErrorMessage %>" AllowSorting="true">
                                                                <Columns>
                                                                    <asp:BoundField DataField="Description" HeaderText="<%$ Resources:lbl_Description %>" SortExpression="Description" />
                                                                    <nexus:BoundField DataField="InitialReserve" HeaderText="<%$ Resources:lbl_InitialReserve %>" DataType="Currency" SortExpression="InitialReserve" />
                                                                    <nexus:BoundField DataField="PaidAmount" HeaderText="<%$ Resources:lbl_Paidtodate %>" DataType="Currency" SortExpression="PaidAmount" />
                                                                    <nexus:BoundField DataField="RevisedReserve" HeaderText="<%$ Resources:lbl_RevisedReserve %>" DataType="Currency" SortExpression="RevisedReserve" />
                                                                    <nexus:BoundField DataField="CurrentReserve" HeaderText="<%$ Resources:lbl_CurrentReserve %>" DataType="Currency" SortExpression="CurrentReserve" />
                                                                    <nexus:BoundField DataField="SumInsured" HeaderText="<%$ Resources:lbl_SumInsured %>" DataType="Currency" SortExpression="SumInsured" />
                                                                    <nexus:BoundField DataField="Average" HeaderText="<%$ Resources:lbl_Average %>" DataType="Currency" SortExpression="Average" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="gvTotal" EventName="DataBound" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvTotal" EventName="PageIndexChanging" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvTotal" EventName="RowCommand" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvTotal" EventName="RowDataBound" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvTotal" EventName="RowEditing" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvTotal" EventName="Sorting" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-reservetypes" class="tab-pane animated fadeIn" role="tabpanel">
                              <asp:UpdatePanel ID="UpdateReserveTypes" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlMain" runat="server">
                                                        <div class="table-wrapper" runat="server" id="DivMain">
                                                            <asp:GridView ID="gvReserveTypes" runat="server" AllowPaging="false" PagerSettings-Mode="Numeric"
                                                                AutoGenerateColumns="False" GridLines="None" EmptyDataRowStyle-CssClass="noData"
                                                                EmptyDataText="<%$ Resources:ErrorMessage %>" AllowSorting="true">
                                                                <Columns>
                                                                    <asp:BoundField DataField="TypeCode" HeaderText="<%$ Resources:Main_ReserveType %>" SortExpression="TypeCode" />
                                                                    <nexus:BoundField DataField="Description" HeaderText="<%$ Resources:Main_Description %>" SortExpression="Description" />
                                                                    <nexus:BoundField DataField="InitialReserve" HeaderText="<%$ Resources:Main_InitialReserve %>"
                                                                        DataType="Currency" SortExpression="InitialReserve" />
                                                                    <nexus:BoundField DataField="PaidAmount" HeaderText="<%$ Resources:Main_PaidToDate %>"
                                                                        DataType="Currency" SortExpression="PaidAmount" />
                                                                    <nexus:BoundField DataField="RevisedReserve" HeaderText="<%$ Resources:Main_RevisedReserve %>"
                                                                        DataType="Currency" SortExpression="RevisedReserve" />
                                                                    <nexus:BoundField DataField="CurrentReserve" HeaderText="<%$ Resources:Main_CurrentReserve %>"
                                                                        DataType="Currency" SortExpression="CurrentReserve" />
                                                                    <nexus:BoundField DataField="SumInsured" HeaderText="<%$ Resources:Main_SumInsured %>"
                                                                        DataType="Currency" SortExpression="SumInsured" />
                                                                    <nexus:BoundField DataField="Average" HeaderText="<%$ Resources:Main_Average %>"
                                                                        DataType="Currency" SortExpression="Average" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="gvReserveTypes" EventName="DataBound" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvReserveTypes" EventName="PageIndexChanging" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvReserveTypes" EventName="RowCommand" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvReserveTypes" EventName="RowDataBound" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvReserveTypes" EventName="RowEditing" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvReserveTypes" EventName="Sorting" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-tprecovery" class="tab-pane animated fadeIn" role="tabpanel">
                            <asp:UpdatePanel ID="UpdateTPRecovery" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlTPRecovery" runat="server">
                                                        <div class="table-wrapper">
                                                            <asp:GridView ID="gvTPRecovery" runat="server" AllowPaging="false" PagerSettings-Mode="Numeric"
                                                                AutoGenerateColumns="False" GridLines="None" EmptyDataRowStyle-CssClass="noData"
                                                                EmptyDataText="<%$ Resources:ErrorMessage %>" AllowSorting="true">
                                                                <Columns>
                                                                    <asp:BoundField DataField="Description" HeaderText="<%$ Resources:lbl_Description %>" SortExpression="Description" />
                                                                    <nexus:BoundField DataField="InitialRecovery" HeaderText="<%$ Resources:lbl_InitialReserve %>" DataType="Currency" SortExpression="InitialRecovery" />
                                                                    <nexus:BoundField DataField="ReceiptedAmount" HeaderText="<%$ Resources:lbl_Receivedtodate %>" DataType="Currency" SortExpression="ReceiptedAmount" />
                                                                    <nexus:BoundField DataField="RevisedRecovery" HeaderText="<%$ Resources:lbl_RevisedReserve %>" DataType="Currency" SortExpression="RevisedRecovery" />
                                                                    <nexus:BoundField DataField="CurrentRecovery" HeaderText="<%$ Resources:lbl_CurrentReserve %>" DataType="Currency" SortExpression="CurrentRecovery" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="gvTPRecovery" EventName="DataBound" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvTPRecovery" EventName="PageIndexChanging" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvTPRecovery" EventName="RowCommand" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvTPRecovery" EventName="RowDataBound" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvTPRecovery" EventName="Sorting" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-salvage" class="tab-pane animated fadeIn" role="tabpanel">
                            <asp:UpdatePanel ID="UpdateSalvage" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlSalvage" runat="server">
                                                        <div class="table-wrapper">
                                                            <asp:GridView ID="gvSalvage" runat="server" AllowPaging="false" PagerSettings-Mode="Numeric"
                                                                AutoGenerateColumns="False" GridLines="None" EmptyDataRowStyle-CssClass="noData"
                                                                EmptyDataText="<%$ Resources:ErrorMessage %>" AllowSorting="true">
                                                                <Columns>
                                                                    <asp:BoundField DataField="Description" HeaderText="<%$ Resources:lbl_Description %>" SortExpression="Description" />
                                                                    <nexus:BoundField DataField="InitialRecovery" HeaderText="<%$ Resources:lbl_InitialReserve %>" DataType="Currency" SortExpression="InitialRecovery" />
                                                                    <nexus:BoundField DataField="ReceiptedAmount" HeaderText="<%$ Resources:lbl_Receivedtodate %>" DataType="Currency" SortExpression="ReceiptedAmount" />
                                                                    <nexus:BoundField DataField="RevisedRecovery" HeaderText="<%$ Resources:lbl_RevisedReserve %>" DataType="Currency" SortExpression="RevisedRecovery" />
                                                                    <nexus:BoundField DataField="CurrentRecovery" HeaderText="<%$ Resources:lbl_CurrentReserve %>" DataType="Currency" SortExpression="CurrentRecovery" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="gvSalvage" EventName="DataBound" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvSalvage" EventName="PageIndexChanging" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvSalvage" EventName="RowCommand" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvSalvage" EventName="RowDataBound" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvSalvage" EventName="Sorting" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-payment" class="tab-pane animated fadeIn" role="tabpanel">
                             <asp:UpdatePanel ID="UpdatePayment" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlPayment" runat="server">
                                                        <div class="table-wrapper">

                                                            <asp:GridView ID="gvPayment" runat="server" AllowPaging="false" PagerSettings-Mode="Numeric"
                                                                AutoGenerateColumns="False" GridLines="None" EmptyDataRowStyle-CssClass="noData"
                                                                EmptyDataText="<%$ Resources:ErrorMessage %>" AllowSorting="true">
                                                                <Columns>
                                                                    <asp:BoundField DataField="PaymentDate" HeaderText="<%$ Resources:lbl_Date %>" DataFormatString="{0:d}" SortExpression="PaymentDate" />
                                                                    <asp:BoundField DataField="PartyPaidName" HeaderText="<%$ Resources:lbl_PartyPaid %>" SortExpression="PartyPaidName" />
                                                                    <asp:BoundField DataField="Payee" HeaderText="<%$ Resources:lbl_Payee %>" SortExpression="Payee" />
                                                                    <asp:BoundField DataField="MediaType" HeaderText="<%$ Resources:lbl_MediaType %>" SortExpression="MediaType" />
                                                                    <asp:BoundField DataField="MediaRefrenece" HeaderText="<%$ Resources:lbl_MediaRefrenece %>" SortExpression="MediaRefrenece" />
                                                                    <asp:BoundField DataField="PaymentAmount" HeaderText="<%$ Resources:lbl_Amount %>" ItemStyle-CssClass="Doub" HeaderStyle-CssClass="Doub" SortExpression="PaymentAmount" />
                                                                    <asp:BoundField DataField="TaxAmount" HeaderText="<%$ Resources:lbl_TaxAmount %>" ItemStyle-CssClass="Doub" HeaderStyle-CssClass="Doub" SortExpression="TaxAmount" />
                                                                    <asp:BoundField DataField="CurrencyDescription" HeaderText="<%$ Resources:lbl_Currency %>" SortExpression="CurrencyDescription" />
                                                                    <asp:BoundField DataField="LossAmount" HeaderText="<%$ Resources:lbl_LossAmount %>" ItemStyle-CssClass="Doub" HeaderStyle-CssClass="Doub" SortExpression="LossAmount" />
                                                                    <asp:BoundField DataField="BaseAmount" HeaderText="<%$ Resources:lbl_BaseAmount %>" ItemStyle-CssClass="Doub" HeaderStyle-CssClass="Doub" SortExpression="BaseAmount" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="gvPayment" EventName="DataBound" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvPayment" EventName="PageIndexChanging" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvPayment" EventName="RowCommand" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvPayment" EventName="RowDataBound" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvPayment" EventName="Sorting" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                        </div>
                        <div id="tab-receipt" class="tab-pane animated fadeIn" role="tabpanel">
                           <asp:UpdatePanel ID="UpdateReceipt" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlReceipt" runat="server">
                                                        <div class="table-wrapper">
                                                            <asp:GridView ID="gvReceipt" runat="server" AllowPaging="false" PagerSettings-Mode="Numeric"
                                                                AutoGenerateColumns="False" GridLines="None" EmptyDataRowStyle-CssClass="noData"
                                                                EmptyDataText="<%$ Resources:ErrorMessage %>" AllowSorting="true">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ReceiptDate" HeaderText="<%$ Resources:lbl_Date %>" DataFormatString="{0:d}" SortExpression="ReceiptDate" />
                                                                    <asp:BoundField DataField="PartyReceiptName" HeaderText="<%$ Resources:lbl_PartyReceived %>" SortExpression="PartyReceiptName" />
                                                                    <asp:BoundField DataField="Payee" HeaderText="<%$ Resources:lbl_Payer %>" SortExpression="Payee" />
                                                                    <asp:BoundField DataField="ReceiptAmount" HeaderText="<%$ Resources:lbl_Amount %>" ItemStyle-CssClass="Doub" HeaderStyle-CssClass="Doub" SortExpression="ReceiptAmount" />
                                                                    <asp:BoundField DataField="TaxAmount" HeaderText="<%$ Resources:lbl_TaxAmount %>" ItemStyle-CssClass="Doub" HeaderStyle-CssClass="Doub" SortExpression="TaxAmount" />
                                                                    <asp:BoundField DataField="CurrencyDescription" HeaderText="<%$ Resources:lbl_Currency %>" SortExpression="CurrencyDescription" />
                                                                    <asp:BoundField DataField="LossAmount" HeaderText="<%$ Resources:lbl_LossAmount %>" ItemStyle-CssClass="Doub" HeaderStyle-CssClass="Doub" SortExpression="LossAmount" />
                                                                    <asp:BoundField DataField="BaseAmount" HeaderText="<%$ Resources:lbl_BaseAmount %>" ItemStyle-CssClass="Doub" HeaderStyle-CssClass="Doub" SortExpression="BaseAmount" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="gvReceipt" EventName="DataBound" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvReceipt" EventName="PageIndexChanging" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvReceipt" EventName="RowCommand" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvReceipt" EventName="RowDataBound" />
                                                    <asp:AsyncPostBackTrigger ControlID="gvReceipt" EventName="Sorting" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:btn_Ok %>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
