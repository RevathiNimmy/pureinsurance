<%@ Page 
    Language="C#" 
    MasterPageFile="~/MasterPage.master" 
    AutoEventWireup="true" 
    Inherits="EnvironmentAdd" 
    Title="Untitled Page" Codebehind="EnvironmentAdd.aspx.cs" %>
<%@ Register Src="~/WebUserControls/LocationAddEditControl.ascx" TagName="LocationAddEditControl" TagPrefix="wuc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Panel ID="pnlCustomerAdd" runat="server" Width="100%">

        <div class="datamaintenance">
            <wuc:LocationAddEditControl ID="LocationAddEditControl1" runat="server" Display="Add" />
            <div class="buttons">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="CustomerValidation" Text="Save" />
                <asp:Button ID="btnCancel" runat="server" OnClientClick="javascript:history.go(-1);return false;" CausesValidation="false" Text="Back" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>

