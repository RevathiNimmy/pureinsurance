<%@ control language="VB" autoeventwireup="false" inherits="Nexus.Controls_ReportControls_PremiumGrossToNet" Codefile="PremiumGrossToNet.ascx.vb"%>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc6" TagName="FindParty" Src="~/Controls/FindParty.ascx" %>


<script language="javascript" type="text/javascript">
   
</script>

<div id="Controls_ReportControls_PremiumGrossToNet">
    <div class="card">
        <div class="card-body clearfix">
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:lbl_header %>"></asp:Label></legend>
                <div id="liBranch" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblBatch" runat="server" AssociatedControlID="RP__BRANCH_ID" Text="<%$ Resources:lbl_Batch %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__BRANCH_ID" runat="server" CssClass="field-medium form-control">
                        </asp:DropDownList>
						
                    </div>
                </div>	
				
				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblPeriodEndDate" runat="server" AssociatedControlID="RP__PERIODDATE" Text="<%$ Resources:lbl_PeriodEndDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__PERIODDATE" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calPeriodEndDate" runat="server" LinkedControl="RP__PERIODDATE" HLevel="1"></uc1:CalendarLookup>
                        </div>
                    </div>

                    <asp:RequiredFieldValidator ID="reqdvldPeriodEndDate" Display="None" ControlToValidate="RP__PERIODDATE" runat="server" ErrorMessage="<%$ Resources:lbl_req_PeriodEndDate %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"> </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="comvldPeriodEndDate" runat="server" Display="None" ControlToValidate="RP__PERIODDATE" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_PeriodEndDate %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
                    <asp:RangeValidator ID="rngvldPeriodEndDate" runat="server" ErrorMessage="<%$ Resources:lbl_invalidrange_PeriodEndDate %>" ControlToValidate="RP__PERIODDATE" Display="None" ValidationGroup="vldReportsControlsGroup">
                    </asp:RangeValidator>
                </div>
				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblsBasis" runat="server" AssociatedControlID="RP__SBASIS" Text="<%$ Resources:lbl_sBasis %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__SBASIS" runat="server" CssClass="field-medium form-control">
                            <asp:ListItem Text="<%$ Resources:li_TransactionDate %>" Value="Transaction Date"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_TransactionPeriod %>" Value="Transaction Period"></asp:ListItem>  
                        </asp:DropDownList>
                    </div>
                </div>
				 <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblGroupByAgent" runat="server" AssociatedControlID="RP__AGENTGROUP" Text="<%$ Resources:lbl_GroupByAgent %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__AGENTGROUP" runat="server" CssClass="field-medium form-control">
                            <asp:ListItem Text="<%$ Resources:li_GroupByAgent_Yes %>" Value="Yes"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_GroupByAgent_No %>" Value="No"></asp:ListItem>  
                        </asp:DropDownList>
                    </div>
                </div>
				 
				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblReportSec" runat="server" AssociatedControlID="RP__REPORTSECTION" Text="<%$ Resources:lbl_ReportSec %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__REPORTSECTION" runat="server" CssClass="field-medium form-control">
                            <asp:ListItem Text="<%$ Resources:li_ReportSec_ALL %>" Value="ALL"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_ReportSec_SelectedPeriod %>" Value="Selected Period"></asp:ListItem>  
							 <asp:ListItem Text="<%$ Resources:li_ReportSec_SelectedYear %>" Value="Selected Year"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_ReportSec_12Months %>" Value="12 Months"></asp:ListItem>  
                        </asp:DropDownList>
                    </div>
                </div>
				  <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblTypeOfCurrency" runat="server" AssociatedControlID="RP__TypeOfCurrency" Text="<%$ Resources:lbl_TypeOfCurrency %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__TypeOfCurrency" runat="server" CssClass="field-medium form-control">
                            <asp:ListItem Text="<%$ Resources:li_TypeOfCurrency_System %>" Value="System"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_TypeOfCurrency_Base %>" Value="Base"></asp:ListItem>  
                        </asp:DropDownList>
                    </div>
                </div>
               
               <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblGroupByCode" runat="server" AssociatedControlID="RP__GROUPBYCODE" Text="<%$ Resources:lbl_GroupByCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__GROUPBYCODE" runat="server" CssClass="field-medium form-control">
                            <asp:ListItem Text="<%$ Resources:li_DetailSummary_Summary %>" Value="No Grouping"></asp:ListItem>
                            <asp:ListItem Text="<%$ Resources:li_DetailSummary_Detail %>" Value="Branch"></asp:ListItem>  
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
