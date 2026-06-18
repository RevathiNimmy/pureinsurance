<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="ProfileAdd" Title="Untitled Page" Codebehind="ProfileAdd.aspx.cs" %>
<%@ Register Src="~/WebUserControls/ProfileAddEditControl.ascx" TagName="ProfileAddEditControl"
    TagPrefix="wuc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Panel ID="pnlProfileAdd" runat="server" Width="100%">
        <div class="datamaintenance">
            <wuc:ProfileAddEditControl ID="ProfileAddEditControl1" runat="server" Display="Add" />
            <div class="buttons">
                        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="ProfileValidation" Text="Save" />
                        <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CausesValidation="false" Text ="Cancel" />
            </div>
        </div>
    </asp:Panel>                    
</asp:Content>

