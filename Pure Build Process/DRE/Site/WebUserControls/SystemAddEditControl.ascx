<%@ Control Language="C#" AutoEventWireup="true" className="SystemAddEditControl" Inherits="WebUserControls_SystemAddEditControl" Codebehind="SystemAddEditControl.ascx.cs" %>
<%@ Register Src="~/WebUserControls/EmailAddressesControl.ascx" TagName="EmailAddressesControl" TagPrefix="wuc" %>
<asp:Panel ID="pnlSystemAddEditControl" runat="server" Width="100%">
    <asp:HiddenField ID="hfSystemKey" runat="server" />
    <asp:HiddenField ID="hfCretaedBy" runat="server" />

    <div class="question">
        <asp:Label AssociatedControlID="tbName" runat="Server" ID="lblSystemName" Text="System Name:" />
        <asp:TextBox ID="tbName" runat="server" MaxLength="255" Width="400px" />
        <asp:CustomValidator ID="cvUniqueName" runat="server" 
            Text="*" 
            onservervalidate="cvUniqueName_ServerValidate" ValidationGroup="SystemValidation" />
        <ajax:TextBoxWatermarkExtender ID="tbweName" runat="server"
            TargetControlID="tbName"
            WatermarkText="Type System Name Here"
            WatermarkCssClass="watermark" />                    
    </div>
    <div class="question">
        <asp:Label AssociatedControlID="tbDescription" runat="Server" ID="lblDescription" Text="System Description:" />
        <asp:TextBox ID="tbDescription" runat="server" MaxLength="2000" TextMode="MultiLine" Width="400px" Height="150px" />
        <ajax:TextBoxWatermarkExtender ID="tbwmDescription" runat="server"
            TargetControlID="tbDescription"
            WatermarkText="Type System Description Here"
            WatermarkCssClass="watermark" />                    
    </div>
    <div class="question">
        <asp:Label AssociatedControlID="ddlStatus" runat="Server" ID="lblStatus" Text="Status:" />
        <asp:DropDownList ID="ddlStatus" runat="server" />
    </div> 
    <div class="question">
        <asp:Label AssociatedControlID="cbLocked" runat="Server" ID="lblLocked" Text="Locked:" />
        <asp:CheckBox ID="cbLocked" runat="server" />
    </div>
    <div class="question">
        <asp:Label AssociatedControlID="cbNotifyAdmin" runat="Server" ID="lblNotifyAdmin" Text="Notify Admin:" />
        <asp:CheckBox ID="cbNotifyAdmin" runat="server" />
    </div>
    <div class="question">
        <asp:Label AssociatedControlID="ddlStyleSheet" runat="Server" ID="lblStyleSheet" Text="Style Sheet:" />
        <asp:DropDownList  ID="ddlStyleSheet" runat="server" />
        <asp:CustomValidator ID="cvStyleSheet" runat="server"
            ControlToValidate="ddlStyleSheet" ValidationGroup="SystemValidation" 
            OnServerValidate="cvStyleSheet_ServerValidate" Display="Static" Text="*" ToolTip="Stylesheet folder does not exist"  />                                
    </div>
    <div class="question">
        <asp:Label AssociatedControlID="eacAdminEmailAddresses" runat="Server" ID="lblAdminEmail" Text="Admin Email Addresses:" />
        <wuc:EmailAddressesControl ID="eacAdminEmailAddresses" ValidationGroup="AdminEmail" runat="server" />
    </div>
    <div class="question">
        <asp:Label AssociatedControlID="eacErrorEmailAddresses" runat="Server" ID="lblErrorEmail" Text="Error Email Addresses:" />
        <wuc:EmailAddressesControl ID="eacErrorEmailAddresses" ValidationGroup="ErrorEmail" runat="server" />
    </div>
    
    <div style="clear:both"></div>
    
</asp:Panel>
