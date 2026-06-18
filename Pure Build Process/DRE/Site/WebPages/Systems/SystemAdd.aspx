<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="SystemAdd" Title="Untitled Page" Codebehind="SystemAdd.aspx.cs" %>
<%@ Register Src="~/WebUserControls/SystemAddEditControl.ascx" TagName="SystemAddEditControl"
    TagPrefix="wuc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Panel ID="pnlSystemAdd" runat="server" Width="100%">
        <div class="datamaintenance">
            <wuc:SystemAddEditControl ID="SystemAddEditControl1" runat="server" Display="Add" />
            <div class="buttons">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="SystemValidation" Text="Save" />
                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CausesValidation="false" Text ="Cancel" />
            </div>
        </div>    
    </asp:Panel>
</asp:Content>

