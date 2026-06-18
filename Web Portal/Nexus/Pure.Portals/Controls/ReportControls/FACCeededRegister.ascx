<%@ Control Language="VB" AutoEventWireup="false" Inherits="Nexus.Controls_ReportControls_FACCeededRegister" CodeFile="FACCeededRegister.ascx.vb" %>

<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>


<script language="javascript" type="text/javascript">
	$(document).ready(function () {

	});
	function addFac(key, code, name, commissionPerc, taxPerc) {

		tb_remove();
		document.getElementById('<%= RP__REINSURER.ClientId%>').value = unescape(code);
		document.getElementById('<%= RP__REINSURER.ClientId%>').focus();
	}


</script>

<div id="Controls_ReportControls_FACCeededRegister">
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
					<asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="RP__REINSURER" Text="<%$ Resources:btn_InsuranceFile %>" ID="lblbtnInsuranceFile"></asp:Label>
					<div class="col-md-8 col-sm-9">
						<div class="input-group">
							<asp:TextBox ID="RP__REINSURER" runat="server" CssClass="form-control" Text="ALL" onBlur="validate(this)"></asp:TextBox><span class="input-group-btn">
								<asp:LinkButton ID="btnInsuranceFile" runat="server" OnClientClick="tb_show(null , '../Modal/FindFAC.aspx?Page=Report&modal=true&KeepThis=true&FromPage=Report&TB_iframe=true&height=500&width=750' , null);return false;" SkinID="btnModal">
                                    <i class="fa fa-search"></i>
                                    <span class="btn-fnd-txt">Policy Number</span>
								</asp:LinkButton></span>
						</div>
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
			</div>
			<asp:HiddenField ID="txtClientType" runat="server"></asp:HiddenField>
		</div>

	</div>
</div>
<div class="card-footer">
	<asp:LinkButton ID="btnGenerateReport" runat="server" Text="<%$ Resources:btnGenerateReport %>" OnClick="GenerateReport" ValidationGroup="vldReportsControlsGroup" SkinID="btnPrimary"></asp:LinkButton>
</div>
</div>
</div>
