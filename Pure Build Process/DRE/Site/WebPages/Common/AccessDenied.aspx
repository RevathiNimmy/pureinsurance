<%@ Page Title="" Language="C#" MasterPageFile="~/MainMasterPage.master" AutoEventWireup="true" CodeBehind="AccessDenied.aspx.cs" Inherits="RulesEngine.Website.WebPages.Common.AccessDenied" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="sectionheader"><h2>Access Denied</h2></div>
<div class="datamaintenance">
    <div class="accessDenied">
        <div class="accessDeniedHeaderStyle">
        Insufficient Privileges
        </div>
        <div class="accessDeniedMessage">
            You do not have enough privileges to access this resource or compelete this action.
        </div>
        <div class="buttons">
            <asp:Button ID="btnOK" runat="server" OnClientClick="javascript:history.go(-1);return false;" CausesValidation="false" Text="OK" />
        </div>
    </div>
</div>
</asp:Content>
