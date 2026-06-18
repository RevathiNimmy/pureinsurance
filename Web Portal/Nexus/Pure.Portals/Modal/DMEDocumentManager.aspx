<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="DMEDocumentManager.aspx.vb" Inherits="Nexus.Modal_DMEDocumentManager" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register Src="~/Controls/FolderView.ascx" TagName="FolderView" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/FileView.ascx" TagName="FileView" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cntMainBody" runat="Server">
    <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></cc1:ToolkitScriptManager>
    <div id="secure_DocumentManager">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lblPageHeader%> "></asp:Literal></h1>
            </div>
            <div id="document-manager">
                <div class="row">
                    <div class="col-sm-4">
                       <uc1:FolderView ID="cnt_FolderView" runat="server" Height="600"></uc1:FolderView>
                    </div>
                    <div class="col-sm-8">
                       <uc2:FileView ID="cnt_FileView" runat="server" Height="600"></uc2:FileView>
                    </div>
                </div>
               
            </div>
        </div>
    </div>



</asp:Content>
