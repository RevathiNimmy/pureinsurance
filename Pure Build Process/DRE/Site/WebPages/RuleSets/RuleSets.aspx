<%@ Page 
    Language="C#" 
    MasterPageFile="~/MasterPage.master" 
    AutoEventWireup="true" 
    Inherits="WebPages_RuleSets_RuleSets"
    EnableEventValidation="false" 
    Codebehind="RuleSets.aspx.cs" 
%>
<%@ Register 
    Src="~/WebUserControls/RuleSetTreeControl.ascx" 
    TagName="RuleSetTreeControl"
    TagPrefix="tree" 
%>
<%@ Register 
    Src="~/WebUserControls/RuleSetDataEntryControl.ascx" 
    TagName="RuleSetDataEntryControl"
    TagPrefix="dataEntry" 
%>
<%@ Register 
    Src="~/WebUserControls/RuleLineTreeControl.ascx" 
    TagName="RuleLineTreeControl"
    TagPrefix="ruleLineTree" 
%>
<%@ Register 
    Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI"
    TagPrefix="ajax" 
%>
    
<asp:Content ID="cntMain" ContentPlaceHolderID="MainContent" Runat="Server">
    
    <asp:UpdateProgress ID="UpdateProgress" runat="server">
    <ProgressTemplate>
    <div class="washOut"></div>
    <div class="popup asyncServerCallPopup"><h2>Please wait</h2></div>
    </ProgressTemplate>
    </asp:UpdateProgress>
    
    <ajax:UpdatePanel ID="UpdatePanelRuleLinesGrid" runat="server" ChildrenAsTriggers="true" UpdateMode="Always" EnableViewState="true">
    <ContentTemplate>
        <ruleLineTree:RuleLineTreeControl runat="server" ID="RuleLineTreeControl" />     
    </ContentTemplate>
    </ajax:UpdatePanel>
    
    <input type="button" value="Copy" onclick="
        javascript:document.getElementById('<%=btnCopy.ClientID%>').value = 'Copy';
        javascript:document.getElementById('<%=lblPnlCopyHeader.ClientID%>').value = 'Copy Rules';
        javascript:document.getElementById('<%=hdnCopyMode.ClientID%>').value = 'Copy';
        javascript:HideShowCopyDivs(document.getElementById('<%=ddlCopy.ClientID %>').value); 
        javascript:showPopup('<%=pnlCopy.ClientID%>','popup copyRulesPopup');" />
    <input type="button" value="Move" onclick="
        javascript:document.getElementById('<%=btnCopy.ClientID%>').value = 'Move';
        javascript:document.getElementById('<%=lblPnlCopyHeader.ClientID%>').value = 'Move Rules';
        javascript:document.getElementById('<%=hdnCopyMode.ClientID%>').value = 'Move';
        javascript:HideShowCopyDivs(document.getElementById('<%=ddlCopy.ClientID %>').value); 
        javascript:showPopup('<%=pnlCopy.ClientID%>','popup copyRulesPopup'); " />
    <input type="button" value="Delete" onclick="
        javascript:document.getElementById('<%=hdnCopyMode.ClientID%>').value = 'Delete';
        javascript:HideShowCopyDivs(document.getElementById('<%=ddlDelete.ClientID %>').value); 
        javascript:showPopup('<%=pnlDelete.ClientID%>','popup copyRulesPopup'); " />
     &nbsp;&nbsp;&nbsp;
     <input type="button" value="Back" onclick="javascript:showPopup('<%=pnlConfirmBack.ClientID%>','popup yesNoDialog');" />
     <input type="button" value="Cancel" onclick="javascript:showPopup('<%=pnlConfirmCancel.ClientID%>','popup yesNoDialog');" />
    <%--<asp:Button 
        ID="btnBack" 
        runat="server" 
        CausesValidation="false" 
        OnClientClick="javascript:showPopup('<%# pnlConfirmBack.ClientID %>','popup testPublishPopup')" 
        Text="Back" 
        style="height: 26px" />
   <asp:Button 
        ID="btnCancel" 
        runat="server" 
        CausesValidation="false" 
        OnClientClick="javascript:showPopup('<%# pnlConfirmCancel.ClientID %>','popup testPublishPopup')" 
        Text="Cancel" />--%>
    
&nbsp;<div class="ruleset">

        <br />

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
                 <asp:Button ID="btnGoBack" runat="server" Text="Go Back" 
                     CausesValidation="False" onclick="btnGoBack_Click" />
            </div>
        </asp:Panel>
        
 
        <asp:Panel runat="server" ID="cpe">
        
            <asp:Label runat="server" ID="lblDataEntry" CssClass="dataentrylabel">Data Entry</asp:Label>
            
            <div class="dataentry">
            <asp:Panel runat="server" ID="pnlDataEntry">
                
                <tree:RuleSetTreeControl Height="180" runat="server" ID="treePropertyExplorer" /> 
                
                <ajax:UpdatePanel ID="UpdatePanelDataEntryControl" ChildrenAsTriggers="true" runat="server" UpdateMode="Always" EnableViewState="true">
                <ContentTemplate> 
                <dataEntry:RuleSetDataEntryControl runat="server" ID="DataEntryControl" />
                </ContentTemplate>
                </ajax:UpdatePanel>
                
                <div style="clear:both" />
                
            </asp:Panel> 
            </div>            
                
        </asp:Panel>
        
        <asp:Panel runat="server" ID="pnlRuleSetInfo" CssClass="hidden">
        <div>        
            <h2>Ruleset Information</h2>
            
            <div class="question">
                <asp:Label ID="Label1" runat="server" Text="Name:" AssociatedControlID="lblPnlRuleSetInfoName" />
                <asp:Label ID="lblPnlRuleSetInfoName" runat="server" />
            </div>
            
            <div class="question">
                <asp:Label ID="Label3" runat="server" Text="Profile:" AssociatedControlID="lblPnlRuleSetInfoProfile" />
                <asp:Label ID="lblPnlRuleSetInfoProfile" runat="server" />
            </div>
            
            <br />
                                   
            <div class="question">
                <asp:Label ID="Label7" runat="server" Text="Status:" AssociatedControlID="lblPnlRuleSetInfoStatus" />
                <asp:Label ID="lblPnlRuleSetInfoStatus" runat="server" />
            </div>
            <div class="question">
                <asp:Label ID="Label4" runat="server" Text="Line Count:" AssociatedControlID="lblPnlRuleSetInfoLineCount" />
                <asp:Label ID="lblPnlRuleSetInfoLineCount" runat="server" />
            </div>
                  
            <br />
                        
            <div class="question">
                <asp:Label ID="Label2" runat="server" Text="Effective Date:" AssociatedControlID="lblPnlRuleSetInfoEffectiveDate" />
                <asp:Label ID="lblPnlRuleSetInfoEffectiveDate" runat="server" />
            </div>
            
            <br />
            
            <div class="question">
                <asp:Label ID="Label5" runat="server" Text="Last Updated By:" AssociatedControlID="lblPnlRuleSetInfoLastUpdatedBy" />
                <asp:Label ID="lblPnlRuleSetInfoLastUpdatedBy" runat="server" />
            </div>
            
            <div class="question">
                <asp:Label ID="Label6" runat="server" Text="Last Updated:" AssociatedControlID="lblPnlRuleSetInfoLastUpdated" />
                <asp:Label ID="lblPnlRuleSetInfoLastUpdated" runat="server" />
            </div>
                        
            <div class="buttons">
                <input type="button" value="Close" onclick="javascript:hidePopup('<%= pnlRuleSetInfo.ClientID %>');" />
            </div>
        </div>
        </asp:Panel>
        
        <asp:Panel runat="server" ID="pnlPublish" CssClass="hidden">
        <div>
            <asp:HiddenField ID="hfListOfLiveAndIntermediate" runat="server" />
        
            <h2>Publish</h2>
                        
            <div class="question">
                <asp:Label ID="Label21" runat="server" AssociatedControlID="rsExportFirstlineHeaders">Publish&nbsp;To&nbsp;:&nbsp;</asp:Label>
                <asp:DropDownList runat="server" ID="PublishLocation" onselectedindexchanged="PublishLocation_SelectedIndexChanged" AutoPostBack="true" />
            </div>          
            
            <br />
            
            <div class="question">
                <asp:Panel Visible="false" runat="server" ID="pnlPublishIntermediate">By selecting this option you will be making the rule set available in your test environment and creating a binary.<br /><br />It will be possible to amend this version of the rule set once it has been published.</asp:Panel>
                <asp:Panel Visible="false" runat="server" ID="pnlPublishLive">By selecting this option you will be making the rule set live and creating a binary.<br /><br />It will not be possible to amend this version of the rule set once it has been published.</asp:Panel>               
            </div>
            
            <p>Are you sure?</p> 
            
            <div class="buttons">     
                <asp:Button value="Publish" onclick="btnPublish_Click" runat="server" Text="Publish" ID="PublishButton" />
                <input type="button" value="Cancel" onclick="javascript:hidePopup('<%=pnlPublish.ClientID%>');" />
            </div> 
        </div>
        </asp:Panel>
        
        <asp:Panel runat="server" ID="pnlTest" CssClass="hidden">        
        <div>
            <h2>Test</h2>
            <p>Please select a sample file to test</p>
            <div class="question">
                <label for="file">Input file</label>
                <asp:FileUpload runat="server" id="testFileImport" />
            </div>
            <div class="buttons">     
                <asp:Button type="button" runat="server" value="Test" Text="Test" onclick="btnTest_Click" />
                <input type="button" value="Cancel" onclick="javascript:hidePopup('<%=pnlTest.ClientID%>');" />
            </div> 
        </div>
        </asp:Panel>
        
        <asp:Panel runat="server" ID="pnlExport" CssClass="hidden">
        <div>
            <h2>Export</h2>           
            <div class="question">
                <asp:Label runat="server" AssociatedControlID="rsExportFirstlineHeaders">First line contains headers</asp:Label>
                <asp:CheckBox runat="server" Checked="true" ID="rsExportFirstlineHeaders" />
            </div>
            <div class="buttons"> 
                <asp:Button runat="server" ID="btnExport" OnClick="btnExport_Click" Text="Export" />
                <input type="button" value="Cancel" onclick="javascript:hidePopup('<%=pnlExport.ClientID%>');" />
            </div>         
        </div>
        </asp:Panel>
        
        <asp:Panel runat="server" ID="pnlImport" CssClass="hidden">
        <div>        
            <h2>Import</h2>
            <div class="question">
                <asp:Label runat="server" AssociatedControlID="rsImportFile">CSV RuleSet</asp:Label>
                <asp:FileUpload runat="server" ID="rsImportFile" />
                &nbsp;
                <asp:CustomValidator ID="importCustomValidator" ValidationGroup="ImportValidation" runat="server" Text="*" OnServerValidate="importCustomValidator_ServerValidate" />
            </div>
            <div class="question">
                <asp:Label runat="server" AssociatedControlID="rsImportFirstlineHeaders">First line contains headers</asp:Label>
                <asp:CheckBox runat="server" Checked="true" ID="rsImportFirstlineHeaders" />
            </div>
            <div class="question">
                <asp:Label runat="server" AssociatedControlID="rsImportOverwriteOrAppend">Read mode</asp:Label>
                <asp:DropDownList runat="server" ID="rsImportOverwriteOrAppend">
                    <asp:ListItem Text="Overwrite" Value="Overwrite" />
                    <asp:ListItem Text="Append" Value="Append" />
                </asp:DropDownList>
            </div>
            <div class="buttons">
                <asp:Button runat="server" ID="btnImport" ValidationGroup="ImportValidation" OnClick="btnImport_Click" Text="Import" />    
                <input type="button" value="Cancel" onclick="javascript:hidePopup('<%=pnlImport.ClientID%>');" />
            </div>
        </div>
        </asp:Panel>
        
        <asp:Panel runat="server" ID="pnlProfileCorrupted" CssClass="hidden">
        <div>
            <h2>Profile Corrupted</h2>
            <p>Please view the <a runat="server" id="profileCorruptedEditProfileLink">profile page</a> to correct this.</p>
        </div>
        </asp:Panel>
        
        <asp:Panel runat="server" ID="pnlCopy" CssClass="hidden">
        <div>        
            <h2><asp:Label ID="lblPnlCopyHeader" runat="server" Text="Copy rules" /></h2>
            <div class="question">
                <asp:Label ID="lblCopy" runat="server" AssociatedControlID="ddlCopy">Source</asp:Label>
                <asp:DropDownList runat="server" ID="ddlCopy">
                    <asp:ListItem Text="Lines" Value="Lines" Selected="True" />
                    <asp:ListItem Text="Group" Value="Group" />
                </asp:DropDownList>
            </div>
            <div id="divCopyLines">
                <div class="question">
                    <asp:Label ID="lblSourceLineNumbers" runat="server" AssociatedControlID="txtCopySourceStart">Source Line Numbers</asp:Label>
                    <asp:TextBox runat="server" ID="txtCopySourceStart" />
                    <span>to</span>
                    <asp:TextBox runat="server" ID="txtCopySourceEnd" />
                </div>
            </div>
            <div id="divCopyGroup" class="hidden">
                <div class="question">
                    <asp:Label ID="lblSourceGroup" runat="server" AssociatedControlID="ddlSourceGroup">Source Group</asp:Label>
                    <asp:DropDownList runat="server" ID="ddlSourceGroup">
                    </asp:DropDownList>
                </div>
            </div>
            <asp:Panel ID="pnlTargetLineNumber" runat="server" CssClass="question">
                <asp:Label ID="lblTargetLineNumber" runat="server" AssociatedControlID="txtCopyTargetLineNumber">Target Line Number</asp:Label>
                <asp:TextBox runat="server" ID="txtCopyTargetLineNumber" />
            </asp:Panel>
            <div class="buttons">
                <asp:Button runat="server" ID="btnCopy" OnClick="btnCopy_Click" Text="Copy" />    
                <input type="button" value="Cancel" onclick="javascript:hidePopup('<%=pnlCopy.ClientID%>');" />
            </div>
            <asp:HiddenField ID="hdnCopyMode" runat="server" />
        </div>
       
        </asp:Panel>
        
        <asp:Panel runat="server" ID="pnlDelete" CssClass="hidden">
        <div>        
            <h2><asp:Label ID="lblPnlDeleteHeader" runat="server" Text="Delete rules" /></h2>
            <div class="question">
                <asp:Label ID="lblDelete" runat="server" AssociatedControlID="ddlDelete">Source</asp:Label>
                <asp:DropDownList runat="server" ID="ddlDelete">
                    <asp:ListItem Text="Lines" Value="Lines" Selected="True" />
                    <asp:ListItem Text="Group" Value="Group" />
                </asp:DropDownList>
            </div>
            <div id="divDeleteLines">
                <div class="question">
                    <asp:Label ID="lblDeleteSourceLineNumbers" runat="server" AssociatedControlID="txtDeleteSourceStart">Source Line Numbers</asp:Label>
                    <asp:TextBox runat="server" ID="txtDeleteSourceStart" />
                    <span>to</span>
                    <asp:TextBox runat="server" ID="txtDeleteSourceEnd" />
                </div>
            </div>
            <div id="divDeleteGroup" class="hidden">
                <div class="question">
                    <asp:Label ID="lblDeleteSourceGroup" runat="server" AssociatedControlID="ddlDeleteSourceGroup">Source Group</asp:Label>
                    <asp:DropDownList runat="server" ID="ddlDeleteSourceGroup">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="buttons">
                <asp:Button runat="server" ID="btnDelete" OnClick="btnDelete_Click" Text="Delete" />    
                <input type="button" value="Cancel" onclick="javascript:hidePopup('<%=pnlDelete.ClientID%>');" />
            </div>
        </div>
       
        </asp:Panel>
       
        <asp:Panel ID="pnlConfirmBack" runat="server" CssClass="hidden">
            <div>
                <h2>Are you sure?</h2>
                <div class="question">
                    Are you sure you wish to go back?<br />
                    All unsaved changes will be lost and you will be taken back to the ruleset list.
                </div>
            </div>
            <div class="buttons">
                <asp:Button ID="btnConfirmBack" runat="server" OnClick="btnConfirmBack_Click" Text="Yes" />
                <input type="button" value="No" onclick="javascript:hidePopup('<%= pnlConfirmBack.ClientID %>');" />
            </div>
        </asp:Panel>
        
        <asp:Panel ID="pnlConfirmCancel" runat="server" CssClass="hidden">
            <div>
                <h2>Are you sure?</h2>
                <div class="question">
                    Are you sure you wish to cancel?<br />
                    All unsaved changes will be lost.
                </div>
            </div>
            <div class="buttons">
                <asp:Button ID="btnConfirmCancel" runat="server" OnClick="btnConfirmCancel_Click" Text="Yes" />
                <input type="button" value="No" onclick="javascript:hidePopup('<%= pnlConfirmCancel.ClientID %>');" />
            </div>
        </asp:Panel>

        <div class="hidden" id="washoutbox"></div>
                
        <div style="clear:both" />
    </div>
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ExtraContent">

<asp:Panel runat="server" ID="ruleSetPublishBox">
<div class="ruleSetPublishBox">
    <div class="head">Rule Set</div>
    <div class="detail">
        <asp:Label runat="server" ID="lblRuleSetPublishBoxName" /><br />
        <asp:Label runat="server" ID="lblRuleSetPublishBoxDateEffective" /><br />
      
        <div class="buttons">
            <input type="button" value="More Info" onclick="javascript:showPopup('<%=pnlRuleSetInfo.ClientID%>','popup ruleSetInfo');" />
            
            <asp:Button runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click" />
            
            <asp:Button runat="server" ID="btnTest" Text="Test" UseSubmitBehavior="false"  OnClientClick="<Set on page load>" />
            <asp:Button runat="server" ID="btnPublish" Text="Publish" UseSubmitBehavior="false" OnClientClick="<Set on page load>" />
            
            <input type="button" value="Import" onclick="javascript:showPopup('<%=pnlImport.ClientID%>','popup exportImportRuleSetPopup');" />
            <input type="button" value="Export" onclick="javascript:showPopup('<%=pnlExport.ClientID%>','popup exportImportRuleSetPopup');" />
        </div>
    </div>
</div>
</asp:Panel>

</asp:Content>
