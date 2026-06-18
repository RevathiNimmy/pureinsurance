<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="ClassEdit" Title="Untitled Page" Codebehind="ClassEdit.aspx.cs" %>
<%@ Register Src="~/WebUserControls/ClassAddEditControl.ascx" TagName="ClassAddEditControl"
    TagPrefix="wuc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

    <asp:Panel ID="pnlClassEdit" runat="server">    
        <div class="datamaintenance">
            <wuc:ClassAddEditControl ID="ClassAddEditControl1" runat="server" Display="Edit" />
            <div class="buttons">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CausesValidation="true"
                    ValidationGroup="CustomerValidation" Text="Save" />
                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CausesValidation="false"
                    Text ="Cancel" />
            </div> 
        </div>   
    </asp:Panel>
    
</asp:Content>

