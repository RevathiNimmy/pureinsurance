<%@ Control Language="VB" AutoEventWireup="false" CodeFile="OutstandingLossReserves.ascx.vb"
    Inherits="Nexus.Controls_ReportControls_OutstandingLossReserves" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>

<div id="Controls_ReportControls_OutstandingLossReserves">
    <div class="card">
        <div class="card-body clearfix">
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:lbl_header %>"></asp:Label></legend>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblFacility" runat="server" AssociatedControlID="RP__Treaty" Text="<%$ Resources:lbl_Facility %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__Treaty" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_Facility_ArchSurplusTreaty %>" Value="Arch Surplus Treaty (SM11)"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_Facility_DeferredInsurance %>" Value="Deferred Reinsurance"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_Facility_Retained %>" Value="Retained"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_Facility_Retained1 %>" Value="Retained 1"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_Facility_Surplus1 %>" Value="Surplus 1"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <!-- Include Zero reserves -->
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lbl_IncludeZeroReserves" runat="server" AssociatedControlID="RP__INCLUDEZERORES" Text="<%$ Resources:lbl_ZeroReserves %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__INCLUDEZERORES" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_IncludeZeroReserves_No %>" Value="No"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_IncludeZeroReserves_Yes %>" Value="Yes"></asp:ListItem>
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

                        </asp:DropDownList>
                    </div>
                </div>

                <!-- Group By -->
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lbl_GroupBy" runat="server" AssociatedControlID="RP__GroupByCode" Text="<%$ Resources:lbl_GroupBy %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
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
