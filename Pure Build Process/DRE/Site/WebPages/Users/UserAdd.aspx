<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="UserAdd" Title="Untitled Page" Codebehind="UserAdd.aspx.cs" %>
<%@ Register Src="~/WebUserControls/UserAddEditControl.ascx" TagName="UserAddEditControl"
    TagPrefix="wuc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Panel ID="pnlUserAdd" runat="server" Width="100%">
        <div class="datamaintenance">
            <wuc:UserAddEditControl ID="UserAddEditControl1" runat="server" Display="Add" />
            <div class="buttons">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="UserValidation" Text="Save" />
                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CausesValidation="false" Text ="Cancel" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>

