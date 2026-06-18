<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="SystemEdit" Title="Untitled Page" Codebehind="SystemEdit.aspx.cs" %>
<%@ Register Src="~/WebUserControls/SystemAddEditControl.ascx" TagName="SystemAddEditControl"
    TagPrefix="wuc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Panel ID="pnlSystemEdit" runat="server" Width="100%">
        <div class="datamaintenance">
            <wuc:SystemAddEditControl ID="SystemAddEditControl1" runat="server" Display="Edit" />
            <div class="buttons">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="SystemValidation" Text="Save" />
                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CausesValidation="false" Text ="Cancel" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>

