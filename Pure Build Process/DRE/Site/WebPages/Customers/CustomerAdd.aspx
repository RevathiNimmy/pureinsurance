<%@ Page 
    Language="C#" 
    MasterPageFile="~/MasterPage.master" 
    AutoEventWireup="true" 
    Inherits="CustomerAdd" 
    Title="Untitled Page" Codebehind="CustomerAdd.aspx.cs" %>
<%@ Register Src="~/WebUserControls/CustomerAddEditControl.ascx" TagName="CustomerAddEditControl" TagPrefix="wuc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Panel ID="pnlCustomerAdd" runat="server" Width="100%">

        <div class="datamaintenance">
            <wuc:CustomerAddEditControl ID="CustomerAddEditControl1" runat="server" Display="Add" />
            <div class="buttons">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="CustomerValidation" Text="Save" />
                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CausesValidation="false" Text ="Cancel" />
            </div>
        </div>

    </asp:Panel>
</asp:Content>

