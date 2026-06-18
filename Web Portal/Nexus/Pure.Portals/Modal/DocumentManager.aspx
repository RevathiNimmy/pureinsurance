<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DocumentManager.aspx.vb"
    MasterPageFile="~/default.master" Inherits="Modal_DocumentManager" %>

<%@ Register Src="~/Controls/DocumentManager.ascx" TagName="DocumentManager" TagPrefix="uc" %>
<%@ Register Src="~/Controls/SharepointView.ascx" TagName="SharepointView" TagPrefix="uc" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="Modal_DocumentManager">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="ltPageHeader" runat="server"></asp:Literal></h1>
            </div>
            <div class="card-body clearfix">
                <div class="md-whiteframe-z0 bg-white">
                    <ul class="nav nav-lines nav-tabs b-danger">
                        <li><a href="#tab-documents" data-bs-toggle="tab" aria-expanded="true" class="active">
                            <asp:Literal ID="lbltab_basicdetails" runat="server" Text="<%$Resources:lbl_tab_documents %>"></asp:Literal></a></li>
                        <li><a href="#tab-document-archive" data-bs-toggle="tab" aria-expanded="true">
                            <asp:Literal ID="lbltab_editaddresses" runat="server" Text="<%$Resources:lbl_tab_document_archive %>"></asp:Literal></a></li>
                    </ul>
                    <div class="tab-content clearfix p b-t b-t-2x">
                        <div id="tab-documents" class="tab-pane animated fadeIn active" role="tabpanel">
                            <uc:DocumentManager runat="server" ID="DocumentManager"></uc:DocumentManager>
                        </div>
                        <div id="tab-document-archive" class="tab-pane animated fadeIn" role="tabpanel">
                            <uc:SharepointView runat="server" ID="SharepointView"></uc:SharepointView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
