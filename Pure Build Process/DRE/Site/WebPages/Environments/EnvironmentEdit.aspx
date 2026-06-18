<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="EnvironmentEdit" Title="Untitled Page" Codebehind="EnvironmentEdit.aspx.cs" %>
<%@ Register Src="~/WebUserControls/LocationAddEditControl.ascx" TagName="LocationAddEditControl"
    TagPrefix="wuc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

    <asp:Panel ID="pnlCustomerEdit" runat="server">    
        <div class="datamaintenance">
            <wuc:LocationAddEditControl ID="LocationAddEditControl1" runat="server" Display="Edit" />
            <div class="buttons">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CausesValidation="true"
                    ValidationGroup="CustomerValidation" Text="Save" />
                <asp:Button ID="btnGoBack" runat="server" OnClientClick="javascript:history.go(-1);return false;" CausesValidation="false" Text="Back" />
            </div> 
        </div>   
    </asp:Panel>
    
</asp:Content>

