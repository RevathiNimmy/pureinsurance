<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TrialBalance.ascx.vb" Inherits="Nexus.Controls_ReportControls_TrialBalance" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc6" TagName="FindParty" Src="~/Controls/FindParty.ascx" %>

<script language="javascript" type="text/javascript">

</script>
<div id="Controls_ReportControls_TrialBalance">
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
                    <asp:Label ID="lblEndDate" runat="server" AssociatedControlID="RP__PeriodDate" Text="<%$ Resources:lbl_EndDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__PeriodDate" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calEndDate" runat="server" LinkedControl="RP__PeriodDate" HLevel="1"></uc1:CalendarLookup>
                        </div>
                    </div>

                    <asp:RequiredFieldValidator ID="reqdvldEndDate" Display="None" ControlToValidate="RP__PeriodDate" runat="server" ErrorMessage="<%$ Resources:lbl_req_EndDate %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"> </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="comvldEndDate" runat="server" Display="None" ControlToValidate="RP__PeriodDate" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_EndDate %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
                    <asp:RangeValidator ID="rngvldEndDate" runat="server" ErrorMessage="<%$ Resources:lbl_invalidrange_EndDate %>" ControlToValidate="RP__PeriodDate" Display="None" ValidationGroup="vldReportsControlsGroup">
                    </asp:RangeValidator>
                </div>

                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblDateBasis" runat="server" AssociatedControlID="RP__Basis" Text="<%$ Resources:lbl_DateBasis %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__Basis" runat="server" CssClass="field-medium form-control">
                            <asp:ListItem Text="<%$ Resources:li_DateBasis_TransactionDate %>" Value="Transaction Date"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_DateBasis_TransactionPeriod %>" Value="Transaction Period"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblCurrencyType" runat="server" AssociatedControlID="RP__TypeOfCurrency" Text="<%$ Resources:lbl_CurrencyType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__TypeOfCurrency" runat="server" CssClass="field-medium form-control">
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
                            <asp:ListItem Text="<%$ Resources:li_GroupBy_Branch %>" Value="Branch"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_GroupBy_NoGrouping %>" Value="No Grouping"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblExcludeEOYJournals" runat="server" AssociatedControlID="RP__Exclude_EOY_Journals" Text="<%$ Resources:lbl_Exclude_EOY_Journals %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__Exclude_EOY_Journals" runat="server" CssClass="field-medium form-control">
                            <asp:ListItem Text="<%$ Resources:li_Exclude_EOY_Journals_Yes %>" Value="Yes"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_Exclude_EOY_Journals_No %>" Value="No"></asp:ListItem>
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
