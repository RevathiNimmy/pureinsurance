<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="RuleSetAdd" Title="Untitled Page" Codebehind="RuleSetAdd.aspx.cs" %>
<%@ Register Src="~/WebUserControls/RuleSetAddEditControl.ascx" TagName="RuleSetAddEditControl"
    TagPrefix="wuc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Panel ID="pnlRuleSetAdd" runat="server" Width="100%">
        <div class="datamaintenance">
            <wuc:RuleSetAddEditControl ID="RuleSetAddEditControl1" runat="server" Display="Add" />
            <div class="buttons">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="RuleSetValidation" Text="Save" />
                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CausesValidation="false" Text ="Cancel" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>

