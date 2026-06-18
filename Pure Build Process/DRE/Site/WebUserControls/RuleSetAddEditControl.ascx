<%@ Control Language="C#" AutoEventWireup="true" ClassName="RuleSetAddEditControl"
    Inherits="WebUserControls_RuleSetAddEditControl" CodeBehind="RuleSetAddEditControl.ascx.cs" %>
<%@ Register Src="~/WebUserControls/RuleSetKeysControl.ascx" TagName="RuleSetKeysControl"
    TagPrefix="wuc" %>
<asp:Panel ID="pnlRuleSetAddEditControl" runat="server" Width="100%">
    <asp:HiddenField ID="hfRuleSetKey" runat="server" />
    <div class="question">
        <asp:Label AssociatedControlID="ddlSystem" runat="Server" ID="lblSystemName" Text="System Name:" />
        <asp:DropDownList ID="ddlSystem" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlSystem_SelectedIndexChanged" />
    </div>
    <div class="question">
        <asp:Label AssociatedControlID="ddlCustomer" runat="Server" ID="lblCustomerName"
            Text="Customer Name:" />
        <asp:DropDownList ID="ddlCustomer" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" />
    </div>
    <div class="question">
        <asp:Label AssociatedControlID="ddlProfile" runat="Server" ID="lblProfileName" Text="Profile Name:" />
        <asp:DropDownList ID="ddlProfile" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlProfile_SelectedIndexChanged" />
        <asp:Label runat="Server" ID="Label3" Text="(*) Denotes System Level Profile" />
        <asp:HiddenField ID="hfProfileIndex" runat="server" />
        <asp:HiddenField ID="hfPreviousStatus" runat="server" />
        <asp:HiddenField ID="hfPreviousEffectiveDate" runat="server" />
        <asp:RequiredFieldValidator ID="rfvProfile" runat="server" ControlToValidate="ddlProfile"
            ValidationGroup="RuleSetValidation" Display="Static" Text="*" ToolTip="Profile is mandatory" />
    </div>
    <div class="question">
        <asp:Label AssociatedControlID="cbxOverrideProfileOutputObject" runat="Server" ID="Label7" Text="Override Profile Output Object?" />
        <asp:CheckBox runat="server" ID="cbxOverrideProfileOutputObject" 
            AutoPostBack="true" 
            oncheckedchanged="cbxOverrideProfileOutputObject_CheckedChanged" />
    </div>
    <asp:Panel runat="server" ID="pnlOverrideProfileOutputObject" class="question" Visible="false">
        <asp:Label ID="ddlOverriddenProfileOutputObjectLabel" AssociatedControlID="ddlOverriddenProfileOutputObject" runat="server" Text="Output Object: " />
        <asp:DropDownList ID="ddlOverriddenProfileOutputObject" runat="server" />
    </asp:Panel>
    <div class="question">
        <asp:Label AssociatedControlID="tbName" runat="Server" ID="lblRuleSetName" Text="RuleSet Name:" />
        <asp:TextBox ID="tbName" runat="server" MaxLength="255" Width="400px" />
        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName"
            ValidationGroup="RuleSetValidation" Display="Static" Text="*" ToolTip="RuleSet name is mandatory" />
        <ajax:TextBoxWatermarkExtender ID="tbweName" runat="server" TargetControlID="tbName"
            WatermarkText="Type RuleSet Name Here" WatermarkCssClass="watermark" />
    </div>
    <div class="question">
        <asp:Label AssociatedControlID="tbDescription" runat="Server" ID="lblDescription"
            Text="RuleSet Description:" />
        <asp:TextBox ID="tbDescription" runat="server" MaxLength="2000" TextMode="MultiLine"
            Width="400px" Height="150px" />
        <ajax:TextBoxWatermarkExtender ID="tbwmDescription" runat="server" TargetControlID="tbDescription"
            WatermarkText="Type RuleSet Description Here" WatermarkCssClass="watermark" />
    </div>
    <div class="question">
        <asp:Label AssociatedControlID="cbxShowAudit" runat="Server" ID="Label5" Text="Show Audit?" />
        <asp:CheckBox ID="cbxShowAudit" runat="server" />
    </div>
    <div class="question">
        <asp:Label AssociatedControlID="cbxShowDifferences" runat="Server" ID="Label6" Text="Show Differences?" />
        <asp:CheckBox ID="cbxShowDifferences" runat="server" />
    </div>
    <div class="question">
        <asp:Label AssociatedControlID="tbEffectiveDate" runat="Server" ID="lblEffectiveDate"
            Text="Effective Date:" />
        <asp:TextBox ID="tbEffectiveDate" runat="server" Width="185px"  />
        <asp:Image ID="imgEffectiveDate" runat="server" ImageUrl="~/App_Themes/Images/Calendar_scheduleHS.png" />
        
        
        
        <asp:RequiredFieldValidator ID="rfvEffectiveDate" runat="server" ControlToValidate="tbEffectiveDate"
            ValidationGroup="RuleSetValidation" Display="Static" Text="*" ToolTip="Effective date is mandatory" />
            
            
        <asp:CustomValidator 
            id="cvEffectiveDate" 
            runat="server" 
            OnServerValidate="cvEffectiveDate_ServerValidate"
            Display="Dynamic"
            Text="*"
            ValidationGroup="RuleSetValidation"
            ToolTip="Must be after today’s date and time" />
                   
        <ajax:CalendarExtender ID="ceEffectiveDate" runat="server" TargetControlID="tbEffectiveDate"
            PopupButtonID="imgEffectiveDate" Enabled="true" />
        <ajax:TextBoxWatermarkExtender ID="tbwmEffectiveDate" runat="server" TargetControlID="tbEffectiveDate"
            WatermarkText="Choose An Effective Date" WatermarkCssClass="watermark" />
    </div>
    <div class="question">
        <asp:Label ID="tbEffectiveTime" runat="Server" Text="Effective Time:" AssociatedControlID="ddlEffectiveTimeHours" />
        <asp:DropDownList ID="ddlEffectiveTimeHours" runat="server" />
        <asp:Label ID="Label4" runat="Server" Text=":&nbsp;" Width="5px" AssociatedControlID="ddlEffectiveTimeMinutes" />
        <asp:DropDownList ID="ddlEffectiveTimeMinutes" runat="server"  />
        <br /><br />
    </div>
    <div class="question">
        <asp:Label AssociatedControlID="ddlStatus" runat="Server" ID="lblStatus" Text="Status:" />
        <asp:DropDownList ID="ddlStatus" runat="server" readonly="readonly" />
    </div>
    <div class="ruleSetKeysControlStyle">
        <wuc:RuleSetKeysControl ID="rkcRuleSetKeys" runat="server" />
    </div>
    <asp:Button ID="fakeButton" runat="server" Style="display: none;" />
    <asp:Button ID="fakeButton1" runat="server" Style="display: none;" />
    <ajax:ModalPopupExtender ID="mpeConfirm" runat="server" PopupControlID="pnlConfirm"
        TargetControlID="fakeButton" BehaviorID="ModalPopupConfirm" BackgroundCssClass="washOut"
        CancelControlID="fakeButton" RepositionMode="None" DynamicServicePath="" Enabled="True" />
    <ajax:ModalPopupExtender ID="mpeKeys" runat="server" PopupControlID="pnlKeys"
        TargetControlID="fakeButton1" BehaviorID="ModalPopupKeysConfirm" BackgroundCssClass="washOut"
        CancelControlID="fakeButton" RepositionMode="None" DynamicServicePath="" Enabled="True" />
    <asp:Panel ID="pnlConfirm" runat="server" Style="display: none;" CssClass="confirmmessagebox">
        <div class="RuleSetKeysControlHeaderStyle">
            <asp:Label ID="lblHeadingConfirm" runat="server" Text="Confirm" />
        </div>
        <div class="confirmmessage">
            <asp:Label ID="lblMessage" runat="server" Text="Selecting a new profile will clear existing RuleSetKeys. <br/>Continue?" />
        </div>
        <div class="buttons">
            <asp:Button ID="btnYes" runat="server" OnClick="btnYes_Click" Text="Yes" />
            <asp:Button ID="btnNo" runat="server" OnClick="btnNo_Click" Text="No" />
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlKeys" runat="server" Style="display: none;" CssClass="confirmmessagebox">
        <div class="RuleSetKeysControlHeaderStyle">
            <asp:Label ID="Label1" runat="server" Text="Confirm" />
        </div>
        <div class="confirmmessage">
            <asp:Label ID="Label2" runat="server" Text="Do you want to add Rule Set Keys to this new Rule Set?" />
        </div>
        <div class="buttons">
            <asp:Button ID="btnKeysYes" runat="server" OnClick="btnKeysYes_Click" Text="Yes" />
            <asp:Button ID="btnKeysNo" runat="server" OnClick="btnKeysNo_Click" Text="No" />
        </div>
    </asp:Panel>
</asp:Panel>
