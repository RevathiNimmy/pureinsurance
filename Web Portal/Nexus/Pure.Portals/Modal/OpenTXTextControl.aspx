<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    ValidateRequest="false" CodeFile="OpenTXTextControl.aspx.vb" Title="Edit TXTextControl"
    Inherits="Nexus.Modal_OpenTXTextControl" %>
<%@ Register Assembly="TXTextControl.Web" Namespace="TXTextControl.Web" TagPrefix="cc1" %>


<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <script type="text/javascript" language="javascript">
        var bReadOnly = false;
        if ('<%= sMode %>' == 'View') {
            bReadOnly = true;
        }

    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="Modal_OpenTXTextControl">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblPageHeader" runat="server" Text="<%$ Resources:lbl_Edit_g %>"></asp:Literal></h1>
            </div>
             <div>
                        <asp:LinkButton ID="btnDownload" runat="server" Visible="true" Text="<%$ Resources:btnDownload %>"  OnClick="DownloadAttachments"  CausesValidation="false"  style="text-decoration:underline;font-size:12px;color:blue;"></asp:LinkButton>
                   </div>
            <div class="card-body clearfix">
                
                <div class="form-horizontal">
                  
                    <div >
                           <cc1:TextControl ID="TextControl1" style="display:inline-block;height: 800px;width:100%;" runat="server" />
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
