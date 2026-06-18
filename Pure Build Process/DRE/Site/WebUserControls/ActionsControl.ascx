<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActionsControl.ascx.cs" Inherits="RulesEngine.Website.WebUserControls.ActionsControl" %>

<asp:Panel runat="server" ID="pnlContainer" class="container" style="min-width:75px">
    <asp:Panel runat="server" id="pnlCenter" class="centrecolumn">
        <asp:DropDownList runat="server" ID="ddlActions" Width="100%" />
    </asp:Panel>
    <asp:Panel runat="server" id="pnlRight" class="rightcolumn">
        <asp:ImageButton runat="server" ID="btnDoAction" Text="Go" ImageUrl="~/App_Themes/Images/GoArrow.png" Width="25px" />
    </asp:Panel>
</asp:Panel>

    



    