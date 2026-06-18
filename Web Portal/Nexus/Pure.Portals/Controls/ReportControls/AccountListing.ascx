<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AccountListing.ascx.vb"
    Inherits="Nexus.Controls_ReportControls_AccountListing" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>



<div id="Controls_ReportControls_AccountListing">
    <div class="card">
        <div class="card-body clearfix">
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:lbl_header %>"></asp:Label></legend>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lbl_Ledger" runat="server" AssociatedControlID="RP__LEDGERNAME" Text="<%$ Resources:lbl_Ledger %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__LEDGERNAME" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_LedgerDefaultText %>" Value="ALL"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_Agent %>" Value="Agent"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_Client %>" Value="Client"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_Commission %>" Value="Commission"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_Discount %>" Value="Discount"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_Fees %>" Value="Fees"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_Insurer %>" Value="Insurer"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_Nominal %>" Value="Nominal"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_OtherPartyPayable %>" Value="Other Party Payable"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_Purchase %>" Value="Purchase"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_Sub Agent %>" Value="Sub Agent"></asp:ListItem>

                        </asp:DropDownList>
                    </div>
                </div>

                <!-- Report format -->

                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblReportFormat" runat="server" AssociatedControlID="RP__REPORTFORMAT" Text="<%$ Resources:lbl_ReportFormat %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__REPORTFORMAT" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem Text="<%$ Resources:li_ReportFormat_Short %>" Value="Short"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_ReportFormat_Long %>" Value="Long"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="liBranch" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblBranch" runat="server" AssociatedControlID="RP__BRANCH_ID" Text="<%$ Resources:lbl_Branch %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__BRANCH_ID" runat="server" CssClass="field-medium form-control form-select">
                        </asp:DropDownList>
                    </div>
                </div>
                <asp:HiddenField ID="RP__agent_code" runat="server"></asp:HiddenField>

            </div>
        </div>
        <div class="card-footer">
            <asp:LinkButton ID="btnGenerateReport" runat="server" Text="<%$ Resources:btnGenerateReport %>" OnClick="GenerateReport" ValidationGroup="vldReportsControlsGroup" SkinID="btnPrimary"></asp:LinkButton>
        </div>
    </div>

</div>
