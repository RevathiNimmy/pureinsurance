<%@ Control Language="VB" AutoEventWireup="false" Inherits="Nexus.Controls_ReportControls_PolicyListingLong" CodeFile="PolicyListingLong.ascx.vb" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc6" TagName="FindParty" Src="~/Controls/FindParty.ascx" %>


<script language="javascript" type="text/javascript">
	$(document).ready(function () {

	});
	function setQuote(sPolicyRef, iFileKey, sClientCode) {
		tb_remove();
		document.getElementById('<%= RP__POLICYNUM.ClientId%>').value = sPolicyRef;
		document.getElementById('<%= RP__POLICYNUM.ClientId%>').focus();
		document.getElementById('<%= txtPolicyRefKey.ClientId%>').value = iFileKey;
	}
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

<div id="Controls_ReportControls_PolicyListingLong">
	<div class="card">
		<div class="card-body clearfix">
			<div class="form-horizontal">
				<legend>
					<asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:lbl_header %>"></asp:Label></legend>


				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
					<asp:Label ID="lblSort" runat="server" AssociatedControlID="RP__SORTORDER" Text="<%$ Resources:lbl_Sort %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
					<div class="col-md-8 col-sm-9">
						<asp:DropDownList ID="RP__SORTORDER" runat="server" CssClass="field-medium form-control">
							<asp:ListItem Text="<%$ Resources:li_Client%>" Value="Client"></asp:ListItem>
							<asp:ListItem Text="<%$ Resources:li_Policy %>" Value="Policy"></asp:ListItem>
						</asp:DropDownList>
					</div>
				</div>

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
					<asp:HiddenField ID="txtClientType" runat="server"></asp:HiddenField>
				</div>

				<div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
					<asp:Label runat="Server" CssClass="col-md-4 col-sm-3 control-label" AssociatedControlID="RP__POLICYNUM" Text="<%$ Resources:btn_InsuranceFile %>" ID="lblbtnInsuranceFile"></asp:Label>
					<div class="col-md-8 col-sm-9">
						<div class="input-group">
							<asp:TextBox ID="RP__POLICYNUM" runat="server" CssClass="form-control" Text="ALL" onBlur="validate(this)"></asp:TextBox><span class="input-group-btn">
								<asp:LinkButton ID="btnInsuranceFile" runat="server" OnClientClick="tb_show(null , '../Modal/FindInsuranceFile.aspx?Page=Report&modal=true&KeepThis=true&FromPage=Report&TB_iframe=true&height=500&width=750' , null);return false;" SkinID="btnModal">
                                    <i class="fa fa-search"></i>
                                    <span class="btn-fnd-txt">Policy Number</span>

								</asp:LinkButton></span>
						</div>

					</div>


					<asp:HiddenField ID="txtPolicyRefKey" runat="server"></asp:HiddenField>
					<asp:RequiredFieldValidator ID="rqdPolicy" runat="server" ControlToValidate="RP__POLICYNUM" Display="None" ErrorMessage="<%$ Resources:lbl_req_Policy %>" SetFocusOnError="true" ValidationGroup="vldReportsControlsGroup" Enabled="false"></asp:RequiredFieldValidator>
				</div>

			</div>
			<asp:HiddenField ID="txtCClientKey" runat="server"></asp:HiddenField>
		</div>


	</div>
</div>
<div class="card-footer">
	<asp:LinkButton ID="btnGenerateReport" runat="server" Text="<%$ Resources:btnGenerateReport %>" OnClick="GenerateReport" ValidationGroup="vldReportsControlsGroup" SkinID="btnPrimary"></asp:LinkButton>
</div>