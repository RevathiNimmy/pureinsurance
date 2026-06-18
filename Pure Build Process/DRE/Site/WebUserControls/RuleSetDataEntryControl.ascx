<%@ Control 
    Language="C#"
    AutoEventWireup="true" 
    Inherits="WebUserControls_RuleSetDataEntryControl" 
    EnableViewState="false"
 Codebehind="RuleSetDataEntryControl.ascx.cs" %>
  
<div class="dataentryfields" id="dataentryfields"> 
               
    <asp:Panel runat="server" ID="pnlEntryControls" />
             
    <div class="buttons">
        <asp:Button runat="server" ID="btnCreate" Text="Add" OnClick="btnCreate_Click" />
        <asp:Button runat="server" ID="btnUpdate" Text="Update" OnClick="btnUpdate_Click" />
        <asp:Button runat="server" ID="btnDelete" Text="Delete" OnClick="btnDelete_Click" />
        &nbsp;
        <asp:Button runat="server" ID="btnCancel" Text="Cancel Update" OnClick="btnCancelUpdate_Click" />
    </div>
    
    
</div>

 <div class="ruleset">
    
    <asp:Panel runat="server" ID="pnlAddGroup" CssClass="hidden">
        
        <div class="question">
            <h2>Add Group</h2>
        </div>
        
        <div class="question">
            <asp:Label ID="lblNewGroupName" runat="server" AssociatedControlID="txtNewGroupName">Group Name</asp:Label>
            <asp:TextBox runat="server" ID="txtNewGroupName"></asp:TextBox>
            <ajax:TextBoxWatermarkExtender ID="tbwmNewGroupName" TargetControlID="txtNewGroupName" runat="server" WatermarkText="Type Group Name Here" WatermarkCssClass="watermark" />
        </div>
        
        <div class="buttons">
            <asp:Button runat="server" Text="Create" ID="btnNewGroup" OnClick="btnNewGroup_Click" />
            <input type="button" value="Cancel"  onclick="javascript:hidePopup('<%=pnlAddGroup.ClientID%>');" />
        </div>
        
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlRemoveGroup" CssClass="hidden">
        
        <div class="question">
            <h2>Remove Group</h2>
        </div>
        
        <div class="question">
            <asp:Label ID="Label1" runat="server">Are you sure – all rule lines in the group will be deleted?</asp:Label>
        </div>
        
        <div class="buttons">
            <asp:Button runat="server" Text="Delete" ID="btnRemoveGroup" OnClick="btnRemoveGroup_Click" />
            <input type="button" value="Cancel"  onclick="javascript:hidePopup('<%=pnlRemoveGroup.ClientID%>');" />
        </div>
    
    </asp:Panel>
        
    <div class="hidden" id="washoutbox">
    
 </div>