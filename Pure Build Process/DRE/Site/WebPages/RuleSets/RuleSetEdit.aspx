<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    Inherits="RuleSetEdit" Title="Untitled Page" CodeBehind="RuleSetEdit.aspx.cs" %>

<%@ Register Src="~/WebUserControls/RuleSetAddEditControl.ascx" TagName="RuleSetAddEditControl"
    TagPrefix="wuc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="pnlRuleSetEdit" runat="server" Width="100%">
        <div class="datamaintenance">
            <wuc:RuleSetAddEditControl ID="RuleSetAddEditControl1" runat="server" Display="Edit" />
            <div class="buttons">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" CausesValidation="true"
                    ValidationGroup="RuleSetValidation" Text="Save" />
                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CausesValidation="false"
                    Text="Cancel" />                
            </div>
        </div>
    </asp:Panel>
    
    <div class="ruleset">
    
    
        <asp:Panel runat="server" ID="pnlNotIsUse" CssClass="hidden">
            <div class="question">
                <h2>Rule Set will no longer be applied. Are you sure?</h2>
            </div>
            
            <div class="buttons">
                <asp:Button ID="btnContinueWithNotInUse" runat="server" Text="Yes" 
                    onclick="btnContinueWithNotInUse_Click" Width="100px" />&nbsp;&nbsp;
                <input onclick="javascript:hidePopup('<%=pnlNotIsUse.ClientID%>');" 
                    type="button" value="No" style='width:100px' /></div>
        </asp:Panel>
        
        
        <asp:Panel runat="server" ID="pnlLockHasBeenLost" CssClass="hidden">
            <div class="question">
                <h2>
                    Lock has been lost</h2>
                <p>
                    Sorry but you lost the lock.</p>
                <p>
                    You will now be returned to the RuleSet list.</p>
            </div>
            
            <div class="buttons">
                 <asp:Button ID="btnGoBack" runat="server" Text="Go Back" OnClick="btnGoBack_Click" CausesValidation="False" />
            </div>
        </asp:Panel>
        
        
        <div class="hidden" id="washoutbox"></div>
    
        
    </div>
</asp:Content>
