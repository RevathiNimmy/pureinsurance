<%@ Control Language="C#" AutoEventWireup="true" className="LocationAddEditControl" Inherits="WebUserControls_LocationAddEditControl" Codebehind="LocationAddEditControl.ascx.cs" %>
<asp:Panel ID="pnlLocationAddEditControl" runat="server">
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <asp:HiddenField ID="hfLocationKey" runat="server" />
    <div class="question">
        <asp:Label AssociatedControlID="ddlClass" runat="Server" ID="lblSystemName" Text="Class Name:" />
        <asp:DropDownList ID="ddlClass" Enabled="false" AutoPostBack="false" runat="server"></asp:DropDownList>
    </div>                
    <div class="question">
        <asp:Label AssociatedControlID="tbName" runat="Server" ID="lblClassificationName" Text="Classification Name:" />

        <asp:TextBox ID="tbName" runat="server" MaxLength="255" Width="400px" />
        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName"
                ValidationGroup="CustomerValidation" Display="Static" Text="*" ToolTip="Class name is mandatory"  />
        <ajax:TextBoxWatermarkExtender ID="tbweName" runat="server"
                TargetControlID="tbName"
                WatermarkText="Classification Name"
                WatermarkCssClass="watermark" />    
    </div>   
    <div class="question">
        <asp:Label AssociatedControlID="tbLocation" runat="Server" ID="Label1" Text="Location:" />
        <asp:TextBox ID="tbLocation" runat="server" MaxLength="255" Width="400px" 
            CausesValidation="True" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbLocation"
                ValidationGroup="CustomerValidation" Display="Static" Text="*" ToolTip="Location is mandatory"  />
        <asp:CustomValidator ID="CustomValidator1" runat="server" 
            ErrorMessage="Invalid&nbsp;Location" ControlToValidate="tbLocation" Display="Static"  ValidationGroup="CustomerValidation"
            onservervalidate="CustomValidator1_ServerValidate"></asp:CustomValidator>
        <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server"
                TargetControlID="tbLocation"
                WatermarkText="Executor URL"
                WatermarkCssClass="watermark" />    
    </div>       
    
    <div class="question">
        <asp:Label AssociatedControlID="cbLive" runat="Server" ID="lblLive" Text="Active:" />
        <asp:CheckBox ID="cbLive" runat="server" />
    </div>
    <div style="clear:both"></div>
</asp:Panel>
