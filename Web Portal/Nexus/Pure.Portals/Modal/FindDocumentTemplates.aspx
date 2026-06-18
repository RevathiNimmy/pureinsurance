<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FindDocumentTemplates.aspx.vb" MasterPageFile="~/default.master" Inherits="Nexus.Modal_FindDocumentTemplates" %>

<%@ Register Src="~/Controls/FindDocumentTemplates.ascx" TagName="FindDocumentTemplates" TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="Modal_FindDocumentTemplates">
                <uc1:FindDocumentTemplates ID="ucFindDocumentTemplates" runat="server"></uc1:FindDocumentTemplates>
    </div>
</asp:Content>
