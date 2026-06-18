  <%@ Control Language="VB" AutoEventWireup="false" CodeFile="AgeAnalysis.ascx.vb" Inherits="Nexus.Controls_ReportControls_AgeAnalysis" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc6" TagName="FindParty" Src="~/Controls/FindParty.ascx" %>

<script language="javascript" type="text/javascript">

</script>
<div id="Controls_ReportControls_AgeAnalysis">
    <div class="card">
        <div class="card-body clearfix">

            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:lbl_header %>"></asp:Label></legend>
                <div id="liBranch" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblBranch" runat="server" AssociatedControlID="RP__BRANCH_ID" Text="<%$ Resources:lbl_Branch %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__BRANCH_ID" runat="server" CssClass="field-medium form-control">
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblStatement" AssociatedControlID="RP__Statement_Type" runat="server" Text="<%$ Resources:lbl_StatementType %>" class="col-md-4 col-sm-3 control-label">
                    </asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__Statement_Type" runat="server" CssClass="field-medium form-control">
                            <asp:ListItem Text="<%$ Resources:li_StatementType_Client %>" Value="Client"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_StatementType_Agent %>" Value="Agent"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_StatementType_SubAgent %>" Value="Sub Agent"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_StatementType_Purchase %>" Value="Purchase"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_StatementType_Reinsurer %>" Value="Reinsurer"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>


                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblFromDate" runat="server" AssociatedControlID="RP__end_Date" Text="<%$ Resources:lbl_AsAt %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__end_Date" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calEndDate" runat="server" LinkedControl="RP__end_Date" HLevel="1"></uc1:CalendarLookup>
                        </div>
                    </div>

                    <asp:RequiredFieldValidator ID="reqdvldEndDate" Display="None" ControlToValidate="RP__end_Date" runat="server" ErrorMessage="<%$ Resources:lbl_req_EndDate %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"> </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="comvldEndDate" runat="server" Display="None" ControlToValidate="RP__end_Date" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_EndDate %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
                    <asp:RangeValidator ID="rngvldEndDate" runat="server" ErrorMessage="<%$ Resources:lbl_invalidrange_EndDate %>" ControlToValidate="RP__end_Date" Display="None" ValidationGroup="vldReportsControlsGroup">
                    </asp:RangeValidator>
                </div>


                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblDateBasis" runat="server" AssociatedControlID="RP__Basis" Text="<%$ Resources:lbl_DateBasis %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__Basis" runat="server" CssClass="field-medium form-control">
                            <asp:ListItem Text="<%$ Resources:li_DateBasis_TransactionDate %>" Value="Transaction Date"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_DateBasis_EffectiveDate %>" Value="Effective Date"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_DateBasis_DueDate %>" Value="Due Date"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblCurrencyType" runat="server" AssociatedControlID="RP__TypeOfCurrency" Text="<%$ Resources:lbl_CurrencyType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__TypeOfCurrency" runat="server" CssClass="field-medium form-control">
                            <asp:ListItem Text="<%$ Resources:li_CurrencyType_Account %>" Value="Account"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_CurrencyType_Base %>" Value="Base"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_CurrencyType_System %>" Value="System"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_CurrencyType_Transaction%>" Value="Transaction"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="liGroupBy" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblGroupBy" runat="server" AssociatedControlID="RP__GroupBy" Text="<%$ Resources:lbl_GroupBy %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__GroupBy" runat="server" CssClass="field-medium form-control">
                            <asp:ListItem Text="<%$ Resources:li_GroupBy_NoGrouping %>" Value="No Grouping"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_GroupBy_Branch %>" Value="Branch"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblTransactionType" runat="server" AssociatedControlID="RP__TransactionType" Text="<%$ Resources:lbl_TransactionType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__TransactionType" runat="server" CssClass="field-medium form-control">
                            <asp:ListItem Text="<%$ Resources:li_TransactionType_PremiumClaimTransactions %>" Value="Premium & Claim Transactions"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_TransactionType_ClaimsTransactionsOnly %>" Value="Claims Transactions Only"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_TransactionType_PremiumTransactionsOnly %>" Value="Premium Transactions Only"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblTransaction" runat="server" AssociatedControlID="RP__Transactions" Text="<%$ Resources:lbl_Transaction %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__Transactions" runat="server" CssClass="field-medium form-control">
                            <asp:ListItem Text="<%$ Resources:li_Transaction_Summary %>" Value="Summary"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_Transaction_Detail %>" Value="Detail Transaction"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <div class="card-footer">
            <asp:LinkButton ID="btnGenerateReport" runat="server" Text="<%$ Resources:btnGenerateReport %>" OnClick="GenerateReport" ValidationGroup="vldReportsControlsGroup" SkinID="btnPrimary"></asp:LinkButton>
        </div>
    </div>

</div>
