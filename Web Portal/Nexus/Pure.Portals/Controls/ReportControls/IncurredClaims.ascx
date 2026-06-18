<%@ Control Language="VB" AutoEventWireup="false" CodeFile="IncurredClaims.ascx.vb" Inherits="Nexus.Controls_ReportControls_IncurredClaims" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>

<div id="Controls_ReportControls_AgentAnalysis">
    <div class="card">
        <div class="card-body clearfix">
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:lbl_header %>"></asp:Label></legend>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblIncludeRecoveries" runat="server" AssociatedControlID="RP__IncludeRecoveries" Text="<%$ Resources:lbl_IncludeRecoveries %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__IncludeRecoveries" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_IncludeRecoveries_Yes %>" Value="Yes"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_IncludeRecoveries_No %>" Value="No"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>


                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblIncludeClosedClaim" runat="server" AssociatedControlID="RP__IncludeClosed" Text="<%$ Resources:lbl_IncludeClosedClaim %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__IncludeClosed" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_IncludeClosedClaims_Yes %>" Value="Yes"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_IncludeClosedClaims_No %>" Value="No"></asp:ListItem>
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

                    <asp:RangeValidator ID="rngvldEndDate" runat="server" ErrorMessage="<%$ Resources:lbl_invalidrange_EndDate %>" ControlToValidate="RP__END_DATE" Display="None" ValidationGroup="vldReportsControlsGroup" Type="Date">
                    </asp:RangeValidator>

                    <asp:CompareValidator ID="comvldToDate" runat="server" Display="None" ControlToValidate="RP__End_Date" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_ToDate %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
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
