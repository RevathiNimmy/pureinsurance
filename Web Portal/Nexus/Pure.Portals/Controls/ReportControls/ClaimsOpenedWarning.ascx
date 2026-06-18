<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ClaimsOpenedWarning.ascx.vb"
    Inherits="Nexus.Controls_ReportControls_ClaimsOpenedWarning" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>


<div id="Controls_ReportControls_ClaimsOpenedWarning">
    <div class="card">
        <div class="card-body clearfix">
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:lbl_header %>"></asp:Label></legend>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblElapsedDays" runat="server" AssociatedControlID="RP__iELAPSEDDAYS" Text="<%$ Resources:lblElapsedDays %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox ID="RP__iELAPSEDDAYS" runat="server" CssClass="form-control"></asp:TextBox></div>
                    <asp:RequiredFieldValidator ID="reqdvldElapsedDays" Display="None" ControlToValidate="RP__iELAPSEDDAYS" runat="server" ErrorMessage="<%$ Resources:lbl_req_ElapsedDays %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"> </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="comvldElapsedDays" runat="server" Display="None" ControlToValidate="RP__iELAPSEDDAYS" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_ElapsedDays %>" Operator="DataTypeCheck" Type="Integer" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>


                </div>

                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblStartDate" runat="server" AssociatedControlID="RP__START_DATE" Text="<%$ Resources:lbl_StartDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__START_DATE" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calStartDate" runat="server" LinkedControl="RP__START_DATE" HLevel="1"></uc1:CalendarLookup>
                        </div>
                    </div>

                    <asp:RequiredFieldValidator ID="reqdvldStartDate" Display="None" ControlToValidate="RP__START_DATE" runat="server" ErrorMessage="<%$ Resources:lbl_req_StartDate %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"> </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="comvldStartDate" runat="server" Display="None" ControlToValidate="RP__START_DATE" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_StartDate %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
                    <asp:RangeValidator ID="rngvldStartDate" runat="server" ErrorMessage="<%$ Resources:lbl_invalidrange_StartDate %>" ControlToValidate="RP__START_DATE" Display="None" ValidationGroup="vldReportsControlsGroup" Type="Date">
                    </asp:RangeValidator>
                </div>

                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblEndDate" runat="server" AssociatedControlID="RP__END_DATE" Text="<%$ Resources:lbl_EndDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__END_DATE" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calEndDate" runat="server" LinkedControl="RP__END_DATE" HLevel="1"></uc1:CalendarLookup>
                        </div>
                    </div>

                    <asp:RequiredFieldValidator ID="reqdvldEndDate" Display="None" ControlToValidate="RP__END_DATE" runat="server" ErrorMessage="<%$ Resources:lbl_req_EndDate %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"> </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="comvldEndDate" runat="server" Display="None" ControlToValidate="RP__END_DATE" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_EndDate %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
                    <asp:RangeValidator ID="rngvldEndDate" runat="server" ErrorMessage="<%$ Resources:lbl_invalidrange_EndDate %>" ControlToValidate="RP__END_DATE" Display="None" ValidationGroup="vldReportsControlsGroup" Type="Date">
                    </asp:RangeValidator>
                </div>

                <!-- Date Range -->
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblDateRange" runat="server" AssociatedControlID="RP__DATERANGE" Text="<%$ Resources:lbl_DateRange %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__DATERANGE" runat="server" CssClass="field-medium form-control form-select">
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
                <!-- Date To Filter on -->
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lbl_DateToFilter" runat="server" AssociatedControlID="RP__DATETYPE" Text="<%$ Resources:lbl_DateToFilter %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__DATETYPE" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_DateToFilter_LossDate %>" Value="Loss Date"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_DateToFilter_ReportedDate %>" Value="Reported Date"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <!-- Type of Currency -->
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblTypeOfCurrency" runat="server" AssociatedControlID="RP__TYPEOFCURRENCY" Text="<%$ Resources:lbl_TypeOfCurrency %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__TYPEOFCURRENCY" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_TypeOfCurrency_System %>" Value="System"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_TypeOfCurrency_Base %>" Value="Base"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_TypeOfCurrency_Transaction %>" Value="Transaction"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <!-- Group By -->
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lbl_GroupBy" runat="server" AssociatedControlID="RP__GROUPBYCODE" Text="<%$ Resources:lbl_GroupBy %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__GROUPBYCODE" runat="server" CssClass="field-medium form-control form-select">
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

