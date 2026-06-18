<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Error.aspx.vb" Inherits="Nexus._Error"
	MasterPageFile="~/default.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
	<script type="text/javascript">
		$(document).ready(function () {
			var isWindowLogin = $("#<%=hdnIsAuthenticated.ClientID %>").val();
            if (isWindowLogin == "False")
            {
                $('[href$=searchSection]').css('display','none');
                $('#ctl00_cntMainBody_btnBack').css('display','none');
			}
		});
	</script>
	<style type="text/css">
		.md-btn {
			position: relative;
			display: inline-block;
			padding: 6px;
			overflow: hidden;
			font-family: inherit;
			font-size: 12px;
			font-style: inherit;
			font-weight: bold;
			line-height: inherit;
			color: currentColor;
			text-align: center;
			text-decoration: none;
			text-transform: uppercase;
			white-space: nowrap;
			cursor: pointer;
			background: transparent;
			border: 0;
			border-radius: 3px;
			outline: 0;
			transition: box-shadow .4s cubic-bezier(0.25,0.8,0.25,1),background-color .4s cubic-bezier(0.25,0.8,0.25,1),-webkit-transform .4s cubic-bezier(0.25,0.8,0.25,1);
			transition: box-shadow .4s cubic-bezier(0.25,0.8,0.25,1),background-color .4s cubic-bezier(0.25,0.8,0.25,1),transform .4s cubic-bezier(0.25,0.8,0.25,1);
			-webkit-user-select: none;
			-moz-user-select: none;
			-ms-user-select: none;
			user-select: none;
			font-variant: inherit;
		}
		.md-btn.md-raised {
			-webkit-transform: translate3d(0,0,0);
			transform: translate3d(0,0,0);
		}
		.md-btn.md-raised:not([disabled]), .md-btn.md-fab {
			box-shadow: 0 2px 5px 0 rgba(0,0,0,0.26); 
		}
		.red {
			background-color: #f44336;
			color: #fff!important;
		}
		.p-h-lg {
			padding-right: 32px;
			padding-left: 32px;
		}
	</style>
	<ajaxToolkit:ToolkitScriptManager runat="server" ID="ajaxScriptManager" EnablePartialRendering="true" CombineScripts="false"></ajaxToolkit:ToolkitScriptManager>
	<div id="Error">
		<asp:HiddenField ID="hdnIsAuthenticated" Value="" runat="server" />
		<div class="grey-200 bg-big">
			<div class="">
				<asp:Panel ID="pnlError" runat="server" DefaultButton="btnBack" CssClass="text-center">
					<h1 class="text-shadow no-margin text-white text-3x p-v-lg">
						<span class="text-2x font-bold block text-danger">
							<i class="fa fa-warning" aria-hidden="true"></i>
							<asp:Literal ID="lblErrorHeader" runat="server" Text="error" EnableViewState="false" />
						</span>
					</h1>
					<p class="h4 m-v-lg text-black">
						<asp:Literal ID="ltError" runat="server" Text="" />
					</p>
				</asp:Panel>
				<ajaxToolkit:CollapsiblePanelExtender ID="cpeErrorDetail" runat="Server" TargetControlID="pnlErrorDetail"
					CollapsedSize="0" Collapsed="True" AutoCollapse="False" AutoExpand="False"
					ScrollContents="True" TextLabelID="lblErrorDetail" ExpandedText="<%$ Resources:lbl_HideError%>" CollapsedText="<%$ Resources:lbl_ShowError%>"
					ImageControlID="imgExpandCollapse" ExpandedImage="~/images/ribbon-hide.png" CollapsedImage="~/images/ribbon-show.png"
					ExpandControlID="pnlErrorDetailTitle" CollapseControlID="pnlErrorDetailTitle"
					ExpandDirection="Vertical" ExpandedSize="430" />
				<div class="panel panel-card m-b-md">
					<asp:Panel ID="pnlErrorDetailTitle" runat="server" CssClass="panel-heading grey-50 p-sm b-b b-light">
						<h5 class="no-margin font-bold text-danger">
							<asp:Image ID="imgExpandCollapse" runat="server" />
							<asp:Label ID="lblErrorDetail" runat="server"></asp:Label>
						</h5>
					</asp:Panel>
					<asp:Panel ID="pnlErrorDetail" runat="server" CssClass="panel-body p">
						<asp:Literal ID="ltErrorDetail" runat="server" Text="" />
					</asp:Panel>
				</div>
				<div id="divSubmitArea" runat="server" class="text-center p-v-lg">
					<asp:LinkButton ID="btnBack" CssClass="md-btn red md-raised p-h-lg" TabIndex="2" runat="server" Text="<%$ Resources:lbl_btnBack%>" />
					&nbsp;&nbsp;&nbsp;&nbsp;
					<asp:LinkButton ID="btnEmailError" CssClass="md-btn red md-raised p-h-lg" TabIndex="3" Visible="false" runat="server" Text="<%$ Resources:lbl_btnSendEmail%>" />
				</div>
			</div>
		</div>
	</div>
</asp:Content>
