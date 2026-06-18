<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="CustomerEdit" Title="Untitled Page" Codebehind="CustomerEdit.aspx.cs" %>
<%@ Register Src="~/WebUserControls/CustomerAddEditControl.ascx" TagName="CustomerAddEditControl"
    TagPrefix="wuc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

    <asp:Panel ID="pnlCustomerEdit" runat="server">    
        <div class="datamaintenance">
            <wuc:CustomerAddEditControl ID="CustomerAddEditControl1" runat="server" Display="Edit" />
            <div class="buttons">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CausesValidation="true"
                    ValidationGroup="CustomerValidation" Text="Save" />
                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CausesValidation="false"
                    Text ="Cancel" />
            </div> 
        </div>   
    </asp:Panel>
    
</asp:Content>

