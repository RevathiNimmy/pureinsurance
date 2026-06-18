<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="CommonView" Title="Untitled Page" Codebehind="View.aspx.cs" %>

<%@ Reference Control="~/WebUserControls/SystemAddEditControl.ascx" %>
<%@ Reference Control="~/WebUserControls/CustomerAddEditControl.ascx" %>
<%@ Reference Control="~/WebUserControls/ProfileAddEditControl.ascx" %>
<%@ Reference Control="~/WebUserControls/RuleSetAddEditControl.ascx" %>
<%@ Reference Control="~/WebUserControls/UserAddEditControl.ascx" %>
<%@ Reference Control="~/WebUserControls/GridAddEditControl.ascx" %>
<%@ Reference Control="~/WebUserControls/RuleLineTreeControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Panel ID="pnlView" runat="server" Width="100%">

        <div class="datamaintenance">
            <asp:PlaceHolder ID="phView"  runat="server"></asp:PlaceHolder>
            <div class="buttons">
                <asp:Button ID="btnBack" runat="server" OnClientClick="javascript:history.go(-1);return false;" CausesValidation="false" Text="Back" />
            </div>
        </div>

    </asp:Panel>    
</asp:Content>

