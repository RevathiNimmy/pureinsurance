<%@ Control Language="VB" AutoEventWireup="false" CodeFile="OutstandingClaims.ascx.vb" Inherits="Nexus.Controls_ReportControls_OutstandingClaims" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>

<div id="Controls_ReportControls_AgentAnalysis">
    <div class="card">
        <div class="card-body clearfix">
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:lbl_header %>"></asp:Label></legend>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblReportBasis" runat="server" AssociatedControlID="RP__ReportBasis" Text="<%$ Resources:lbl_ReportBasis %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__ReportBasis" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_ReportBasis_Client %>" Value="Client"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_ReportBasis_Policy %>" Value="Policy"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_ReportBasis_Agent %>" Value="Agent"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_ReportBasis_LossDate %>" Value="Loss Date"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>


                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblSalvageRecovery" runat="server" AssociatedControlID="RP__SalvageAndTPRecovery" Text="<%$ Resources:lbl_SalvageRecovery %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__SalvageAndTPRecovery" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_SalvageRecovery_Exclude %>" Value="Exclude"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_SalvageRecovery_Include %>" Value="Include"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_SalvageRecovery_Only %>" Value="Only"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>


                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblFromDate" runat="server" AssociatedControlID="RP__Start_Date" Text="<%$ Resources:lbl_FromDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__Start_Date" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calFromDate" runat="server" LinkedControl="RP__Start_Date" HLevel="1"></uc1:CalendarLookup>
                        </div>
                    </div>

                    <asp:RequiredFieldValidator ID="reqdvldFromDate" Display="None" ControlToValidate="RP__Start_Date" runat="server" ErrorMessage="<%$ Resources:lbl_req_FromDate %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"> </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="comvldFromDate" runat="server" Display="None" ControlToValidate="RP__Start_Date" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_FromDate %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>

                    <asp:RangeValidator ID="rngvldStartDate" runat="server" ErrorMessage="<%$ Resources:lbl_invalidrange_StartDate %>" ControlToValidate="RP__START_DATE" Display="None" Enabled="true" ValidationGroup="vldReportsControlsGroup" Type="Date">
                    </asp:RangeValidator>
                </div>


                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblToDate" runat="server" AssociatedControlID="RP__End_Date" Text="<%$ Resources:lbl_ToDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__End_Date" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="CalToDate" runat="server" LinkedControl="RP__End_Date" HLevel="1"></uc1:CalendarLookup>
                        </div>
                    </div>

                    <asp:RequiredFieldValidator ID="reqdvldToDate" Display="None" ControlToValidate="RP__End_Date" runat="server" ErrorMessage="<%$ Resources:lbl_req_ToDate %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"> </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="comvldToDate" runat="server" Display="None" ControlToValidate="RP__End_Date" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_ToDate %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>

                    <asp:RangeValidator ID="rngvldEndDate" runat="server" ErrorMessage="<%$ Resources:lbl_invalidrange_EndDate %>" ControlToValidate="RP__END_DATE" Display="None" ValidationGroup="vldReportsControlsGroup" Type="Date">
                    </asp:RangeValidator>

                    <asp:CompareValidator ID="cmpvldStartEndDates" ForeColor="Red" runat="server" ControlToValidate="RP__START_DATE" ControlToCompare="RP__END_DATE" Operator="LessThanEqual" Type="Date" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalidGreater_StartDate %>" Display="None" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
                </div>


                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblDateRange" runat="server" AssociatedControlID="RP__DateRange" Text="<%$ Resources:lbl_DateRange %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__DateRange" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_SelectDateRange_DefaultText %>" Value="Specify Dates"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_SelectDateRange_Yesterday %>" Value="Yesterday"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_SelectDateRange_Today %>" Value="Today"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_SelectDateRange_LastFullWeek%>" Value="Last Full Week"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_SelectDateRange_ThisWeek %>" Value="This Week"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_SelectDateRange_LastFullMonth %>" Value="Last Full Month"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_SelectDateRange_ThisMonth %>" Value="This Month"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>


                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblDateFilter" runat="server" AssociatedControlID="RP__DateType" Text="<%$ Resources:lbl_DateFilter %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__DateType" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_DateFilter_LossDate %>" Value="Loss Date"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_DateFilter_ReportedDate %>" Value="Reported Date"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblCurrencyType" runat="server" AssociatedControlID="RP__TypeOfCurrency" Text="<%$ Resources:lbl_CurrencyType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__TypeOfCurrency" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_CurrencyType_System %>" Value="System"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_CurrencyType_Base %>" Value="Base"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_CurrencyType_Transaction%>" Value="Transaction"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>


                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblGroupBy" runat="server" AssociatedControlID="RP__GroupByCode" Text="<%$ Resources:lbl_GroupBy %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__GroupByCode" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_GroupBy_NoGrouping %>" Value="No Grouping"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_GroupBy_Branch %>" Value="Branch"></asp:ListItem>

                        </asp:DropDownList>
                    </div>
                </div>

                <asp:HiddenField ID="RP__TPACode" runat="server"></asp:HiddenField>
            </div>
        </div>
        <div class="card-footer">
            <asp:LinkButton ID="btnGenerateReport" runat="server" Text="<%$ Resources:btnGenerateReport %>" OnClick="GenerateReport" ValidationGroup="vldReportsControlsGroup" SkinID="btnPrimary"></asp:LinkButton>
        </div>
    </div>

</div>
