<%@ Page Language="VB" AutoEventWireup="false" CodeFile="LetterWriting.aspx.vb" MasterPageFile="~/default.master" Inherits="Nexus.Modal_LetterWriting" %>

<%@ Register Src="~/Controls/DocumentManager.ascx" TagName="DocumentManager" TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <script type="text/javascript" language="javascript">

        function SetTemplate(sCode) {
            tb_remove();
            __doPostBack("TemplateCode", sCode);
        }
        function SetTemplateName(sName) {
            document.getElementById('<%=txtDocumentTemplates.ClientID%>').value = sName;
        }

    </script>
    <asp:ScriptManager ID="scriptMgr" runat="server"></asp:ScriptManager>
    <div id="Modal_LetterWriting">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lblPageHeader %>"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblDocumentTemplates" runat="server" AssociatedControlID="txtDocumentTemplates" Text="<%$ Resources:btnFindDocumentTemplate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="txtDocumentTemplates" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="btnFindDocumentTemplate" runat="server" SkinID="btnModal" CausesValidation="false">
                                         <i class="fa fa-search"></i>
                                        <span class="btn-fnd-txt">Find Document Template</span>
                                    </asp:LinkButton>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <uc1:DocumentManager ID="docMgr1" runat="server" Visible="true" AutoArchiveSelected="false"></uc1:DocumentManager>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnCancel" runat="server" Text="<%$ Resources:btnCancel %> " CausesValidation="false" OnClientClick="self.parent.tb_remove(); return false;" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnOk" runat="server" Text="<%$ Resources:btn_Ok %>" OnClientClick="self.parent.tb_remove(); return false;" SkinID="btnPrimary"></asp:LinkButton>
                
            </div>
        </div>
        <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
