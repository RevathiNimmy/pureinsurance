<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    ValidateRequest="false" CodeFile="EditDocument.aspx.vb" Title="Edit Document"
    Inherits="Nexus.Modal_EditDocument" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <script type="text/javascript" language="javascript">
		$(document).ready(function () {
			var tools = [
				['style', ['bold', 'italic', 'underline']],
				['font', ['strikethrough', 'superscript', 'subscript', 'clear']],
				['fontsize', ['fontsize']],
				['color', ['color']],
				['para', ['ul', 'ol', 'paragraph']],
				['table', ['table']],
				['insert', ['link', 'picture', 'video']],
				['view', ['undo', 'redo', 'fullscreen', 'help']]
			];
			if ('<%= sMode %>' == 'View') {
			tools = false;
		}
		$('#ctl00_cntMainBody_txtDocumentEditor').summernote({
			tabsize: 2,
			height: 275,
			toolbar: tools,
		});
	});
	</script>
	<style>
		.dropdown-toggle:after {content: none;}
	</style>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="Modal_EditDocument">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lbl_Edit_g %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:TextBox ID="txtDocumentEditor" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="card-footer"> 
                  <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:btn_Cancel %>" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnSave" runat="server" Text="<%$ Resources:btn_Ok %>" UseSubmitBehavior="true" SkinID="btnPrimary"></asp:LinkButton>
             
            </div>            
        </div>
        <asp:ValidationSummary ID="ValidationSummary" ShowSummary="true" DisplayMode="BulletList" HeaderText="Error List" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
