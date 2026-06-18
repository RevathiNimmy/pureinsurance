<%@ Control Language="VB" AutoEventWireup="false" Inherits="Nexus.Controls_ReportControls_ClientStatement" CodeFile="ClientStatement.ascx.vb" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc6" TagName="FindParty" Src="~/Controls/FindParty.ascx" %>


<script language="javascript" type="text/javascript">
	$(document).ready(function () {

	});
	function setClient(sClientName, sClientKey) {
		tb_remove();
		if (document.getElementById('<%= txtClientType.ClientId%>').value == 'P') {
		   document.getElementById('<%= RP__PERSONALCLIENT.ClientId%>').value = unescape(sClientName);
		   document.getElementById('<%= txtClientKey.ClientId%>').value = sClientKey;
		   document.getElementById('<%= RP__PERSONALCLIENT.ClientId%>').focus();
	   }
	   else {
		   document.getElementById('<%= RP__CORPORATECLIENT.ClientId%>').value = unescape(sClientName);
		   document.getElementById('<%= txtCClientKey.ClientId%>').value = sClientKey;
		   document.getElementById('<%= RP__CORPORATECLIENT.ClientId%>').focus();
		}
	}

	function setClientType(sClientNameType) {
		document.getElementById('<%= txtClientType.ClientId%>').value = sClientNameType;
	}

</script>

<div id="Controls_ReportControls_ClientStatement">
	<div class="card">
		<div class="card-body clearfix">
			<div class="form-horizontal">
				<legend>
					<asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:lbl_header %>"></asp:Label></legend>

				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
					<asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="RP__PERSONALCLIENT" Text="<%$ Resources:btn_PersonalClient %>" ID="lblbtnPersonalClient"></asp:Label>
					<div class="col-md-8 col-sm-9">
						<div class="input-group">
							<asp:TextBox ID="RP__PersonalClient" runat="server" CssClass="form-control" Text="ALL" onBlur="validate(this)"></asp:TextBox>
							<span class="input-group-btn">
								<asp:LinkButton ID="btnPersonalClient" runat="server" OnClientClick="setClientType('P'); tb_show(null , '../Secure/Agent/FindClient.aspx?modal=true&RequestPage=BG&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=750' , null);return false;" SkinID="btnModal">
									<i class="fa fa-search"></i>
									<span class="btn-fnd-txt">Client</span>
								</asp:LinkButton>
							</span>
						</div>
					</div>
					<asp:HiddenField ID="txtClientKey" runat="server"></asp:HiddenField>
				</div>


				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
					<asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="RP__GROUPCLIENT" Text="<%$ Resources:btn_GroupClient %>" ID="lblbtnGroupClient"></asp:Label>
					<div class="col-md-8 col-sm-9">
						<div class="input-group">
							<asp:TextBox ID="RP__GroupClient" runat="server" CssClass="form-control" Text="ALL" onBlur="validate(this)"></asp:TextBox>
							<span class="input-group-btn">
								<asp:LinkButton ID="btnGroupClient" runat="server" OnClientClick="tb_show(null , '../Secure/Agent/FindClient.aspx?modal=true&RequestPage=BG&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=750' , null);return false;" SkinID="btnModal">
									<i class="fa fa-search"></i>
									<span class="btn-fnd-txt">Group Client</span>
								</asp:LinkButton>
							</span>
						</div>
					</div>
					<asp:HiddenField ID="txtClientKey3" runat="server"></asp:HiddenField>
				</div>

				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
					<asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="RP__CORPORATECLIENT" Text="<%$ Resources:btn_Client %>" ID="lblbtnClient"></asp:Label>
					<div class="col-md-8 col-sm-9">
						<div class="input-group">
							<asp:TextBox ID="RP__CorporateClient" runat="server" CssClass="form-control" Text="ALL" onBlur="validate(this)"></asp:TextBox><span class="input-group-btn">
								<asp:LinkButton ID="btnClient" runat="server" OnClientClick="setClientType('C'); tb_show(null , '../Secure/Agent/FindClient.aspx?modal=true&RequestPage=BG&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=750' , null);return false;" SkinID="btnModal">
                                <i class="fa fa-search"></i>
                                 <span class="btn-fnd-txt">Client</span>
								</asp:LinkButton></span>
						</div>
					</div>
					<asp:HiddenField ID="txtCClientKey" runat="server"></asp:HiddenField>
				</div>

				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
					<asp:Label ID="lblUnderwritingYear" runat="server" AssociatedControlID="RP__Underwriting_Year" Text="<%$ Resources:lbl_UnderwritingYear %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
					<div class="col-md-8 col-sm-9">
						<asp:TextBox ID="RP__Underwriting_Year" runat="server" CssClass="field-medium form-control">                           
						</asp:TextBox>
					</div>
				</div>

				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
					<asp:Label ID="lblTypeOfCurrency" runat="server" AssociatedControlID="RP__TypeOfCurrency" Text="<%$ Resources:lbl_TypeOfCurrency %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
					<div class="col-md-8 col-sm-9">
						<asp:DropDownList ID="RP__TypeOfCurrency" runat="server" CssClass="field-medium form-control">
							<asp:ListItem Text="<%$ Resources:li_TypeOfCurrency_Account %>" Value="Account"></asp:ListItem>
							<asp:ListItem Text="<%$ Resources:li_TypeOfCurrency_System %>" Value="System"></asp:ListItem>
							<asp:ListItem Text="<%$ Resources:li_TypeOfCurrency_Base %>" Value="Base"></asp:ListItem>
							<asp:ListItem Text="<%$ Resources:li_TypeOfCurrency_Transaction %>" Value="Transaction"></asp:ListItem>
						</asp:DropDownList>
					</div>
				</div>

				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
					<asp:Label ID="lblGroupByCode" runat="server" AssociatedControlID="RP__GROUPBY" Text="<%$ Resources:lbl_GroupByCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
					<div class="col-md-8 col-sm-9">
						<asp:DropDownList ID="RP__GroupBy" runat="server" CssClass="field-medium form-control">
							<asp:ListItem Text="<%$ Resources:li_DetailSummary_Summary %>" Value="No Grouping"></asp:ListItem>
							<asp:ListItem Text="<%$ Resources:li_DetailSummary_Detail %>" Value="Branch"></asp:ListItem>
						</asp:DropDownList>
					</div>
				</div>

				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
					<asp:Label ID="lblPeriodStartDate" runat="server" AssociatedControlID="RP__START_DATE" Text="<%$ Resources:lbl_PeriodStartDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
					<div class="col-md-8 col-sm-9">
						<div class="input-group">
							<asp:TextBox ID="RP__START_DATE" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calPeriodStartDate" runat="server" LinkedControl="RP__START_DATE" HLevel="1"></uc1:CalendarLookup>
						</div>
					</div>

					<asp:RequiredFieldValidator ID="reqdvldPeriodStartDate" Display="None" ControlToValidate="RP__START_DATE" runat="server" ErrorMessage="<%$ Resources:lbl_req_PeriodStartDate %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"> </asp:RequiredFieldValidator>
					<asp:CompareValidator ID="comvldPeriodStartDate" runat="server" Display="None" ControlToValidate="RP__START_DATE" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_PeriodStartDate %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
					<asp:RangeValidator ID="rngvldPeriodStartDate" runat="server" ErrorMessage="<%$ Resources:lbl_invalidrange_PeriodStartDate %>" ControlToValidate="RP__START_DATE" Display="None" ValidationGroup="vldReportsControlsGroup">
					</asp:RangeValidator>
				</div>
				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
					<asp:Label ID="lblPeriodEndDate" runat="server" AssociatedControlID="RP__END_DATE" Text="<%$ Resources:lbl_PeriodEndDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
					<div class="col-md-8 col-sm-9">
						<div class="input-group">
							<asp:TextBox ID="RP__END_DATE" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calPeriodEndDate" runat="server" LinkedControl="RP__END_DATE" HLevel="1"></uc1:CalendarLookup>
						</div>
					</div>

					<asp:RequiredFieldValidator ID="reqdvldPeriodEndDate" Display="None" ControlToValidate="RP__END_DATE" runat="server" ErrorMessage="<%$ Resources:lbl_req_PeriodEndDate %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"> </asp:RequiredFieldValidator>
					<asp:CompareValidator ID="comvldPeriodEndDate" runat="server" Display="None" ControlToValidate="RP__END_DATE" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_PeriodEndDate %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
					<asp:RangeValidator ID="rngvldPeriodEndDate" runat="server" ErrorMessage="<%$ Resources:lbl_invalidrange_PeriodEndDate %>" ControlToValidate="RP__END_DATE" Display="None" ValidationGroup="vldReportsControlsGroup">
					</asp:RangeValidator>
				</div>

				<%--<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblPartyCnt" runat="server" AssociatedControlID="RP__party_cnt" Text="<%$ Resources:lbl_PartyCnt %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:TextBox ID="RP__party_cnt" runat="server" CssClass="field-medium form-control">                           
                        </asp:TextBox>
                    </div>
                </div>--%>

				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
					<asp:Label ID="lblTransactionType" runat="server" AssociatedControlID="RP__TRANSACTIONTYPE" Text="<%$ Resources:lbl_TransactionType %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
					<div class="col-md-8 col-sm-9">
						<asp:DropDownList ID="RP__TransactionType" runat="server" CssClass="field-medium form-control">
							<asp:ListItem Text="<%$ Resources:li_TransactionType_PCT %>" Value="Premium & Claim Transactions"></asp:ListItem>
							<asp:ListItem Text="<%$ Resources:li_TransactionType_PTO %>" Value="Premium Transactions Only"></asp:ListItem>
							<asp:ListItem Text="<%$ Resources:li_TransactionType_CTO %>" Value="Claim Transactions Only"></asp:ListItem>
						</asp:DropDownList>
					</div>
				</div>

				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
					<asp:Label ID="lblUnallocate" runat="server" AssociatedControlID="RP__AgeAndUnalloc" Text="<%$ Resources:lbl_Unallocate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
					<div class="col-md-8 col-sm-9">
						<asp:DropDownList ID="RP__AgeAndUnalloc" runat="server" CssClass="field-medium form-control">
							<asp:ListItem Text="<%$ Resources:li_Unallocated_Yes %>" Value="Yes"></asp:ListItem>
							<asp:ListItem Text="<%$ Resources:li_Unallocated_No %>" Value="No"></asp:ListItem>
						</asp:DropDownList>
					</div>
				</div>


				<asp:HiddenField ID="txtClientType" runat="server"></asp:HiddenField>
			</div>
		</div>


		<div class="card-footer">
			<asp:LinkButton ID="btnGenerateReport" runat="server" Text="<%$ Resources:btnGenerateReport %>" OnClick="GenerateReport" ValidationGroup="vldReportsControlsGroup" SkinID="btnPrimary"></asp:LinkButton>
		</div>
	</div>
</div>
