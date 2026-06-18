<%@ Page Language="C#" MasterPageFile="~/MainMasterPage.master" AutoEventWireup="true" Inherits="ForceChangePassword" Title="Untitled Page" Codebehind="ForceChangePassword.aspx.cs" %>
<%@ Register Src="~/WebUserControls/ChangePasswordControl.ascx" TagName="ChangePasswordControl" TagPrefix="wuc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="datamaintenance">
        <wuc:ChangePasswordControl ID="cpcChangePassword" runat="server" />
    </div>
</asp:Content>

