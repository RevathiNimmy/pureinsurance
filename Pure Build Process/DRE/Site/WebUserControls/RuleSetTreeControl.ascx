<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    Inherits="WebUserControls_RuleSetTreeControl"
    EnableViewState="false"     
 Codebehind="RuleSetTreeControl.ascx.cs" %>
<div class="propertytreetabcontainer">
    <ajax:TabContainer runat="server">
        <ajax:TabPanel ID="TabPanelInput" runat="server" HeaderText="Input" Visible="true">
            <ContentTemplate>
                <div class="propertytreeview">
                <asp:TreeView ID="treInput" Runat="server" />
                </div>                    
            </ContentTemplate>
        </ajax:TabPanel>
        <ajax:TabPanel ID="TabPanelOutput" runat="server" HeaderText="Output" Visible="true">
            <ContentTemplate> 
                <div class="propertytreeview">
                <asp:TreeView ID="treOutput" Runat="server" /> 
                </div>          
            </ContentTemplate>
        </ajax:TabPanel>
    </ajax:TabContainer>  
</div>