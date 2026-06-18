<%@ Page 
    Language="C#" 
    AutoEventWireup="true" 
    Inherits="WebPages_Grids_GridEdit" 
    MasterPageFile="~/MasterPage.master" 
 Codebehind="GridEdit.aspx.cs" %>
<%@ Register Src="~/WebUserControls/GridAddEditControl.ascx" TagName="GridAddEditControl" TagPrefix="wuc" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="datamaintenance">
        <wuc:GridAddEditControl ID="GridAddEditControl" Display="Edit" runat="server" />

        <div class="buttons">
            <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" CausesValidation="true" ValidationGroup="RuleSetValidation" Text="Save" />
            <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" CausesValidation="false" Text="Cancel" />
        </div>
    </div>
</asp:Content>

