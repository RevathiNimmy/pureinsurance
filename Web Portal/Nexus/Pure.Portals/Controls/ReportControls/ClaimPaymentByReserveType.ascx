<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ClaimPaymentByReserveType.ascx.vb"
    Inherits="Nexus.Controls_ReportControls_ClaimPaymentByReserveType" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>

<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>



<div id="Controls_ReportControls_ClaimPaymentByReserveType">
    <div class="card">
        <div class="card-body clearfix">
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:lbl_header %>"></asp:Label></legend>
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
                    <asp:CompareValidator ID="cmpvldStartEndDates" ForeColor="Red" runat="server" ControlToValidate="RP__START_DATE" ControlToCompare="RP__END_DATE" Operator="LessThanEqual" Type="Date" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalidGreater_StartDate %>" Display="None" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
                </div>


                <!-- Type of Currency -->
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblTypeOfCurrency" runat="server" AssociatedControlID="RP__TYPEOFCURRENCY" Text="<%$ Resources:lbl_TypeOfCurrency %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__TYPEOFCURRENCY" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem>System</asp:ListItem>
                            <asp:ListItem>Base</asp:ListItem>
                            <asp:ListItem>Transaction</asp:ListItem>

                        </asp:DropDownList>
                    </div>
                </div>

                <!-- Group By -->
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lbl_GroupBy" runat="server" AssociatedControlID="RP__GROUPBYCode" Text="<%$ Resources:lbl_GroupBy %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__GROUPBYCode" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem>No Grouping</asp:ListItem>
                            <asp:ListItem>Branch</asp:ListItem>
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

