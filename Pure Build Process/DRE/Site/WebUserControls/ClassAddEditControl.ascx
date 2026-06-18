<%@ Control Language="C#" AutoEventWireup="true" className="ClassAddEditControl" Inherits="WebUserControls_ClassAddEditControl" Codebehind="ClassAddEditControl.ascx.cs" %>
<asp:Panel ID="pnlClassAddEditControl" runat="server">

    <asp:HiddenField ID="hfClassKey" runat="server" />
    <asp:HiddenField ID="hfCreatedBy" runat="server" />

    <div class="question">
        <asp:Label AssociatedControlID="ddlSystem" runat="Server" ID="lblSystemName" Text="System Name:" />
        <asp:DropDownList ID="ddlSystem" Enabled="false" AutoPostBack="false" runat="server"></asp:DropDownList>
    </div>                
    <div class="question">
        <asp:Label AssociatedControlID="ddlCustomer" runat="Server" ID="lblCustomerName" Text="Customer Name:" />
        <asp:DropDownList ID="ddlCustomer" AutoPostBack="false" runat="server"></asp:DropDownList>
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
        <asp:Label AssociatedControlID="cbLive" runat="Server" ID="lblLive" Text="Live Servers:" />
        <asp:CheckBox ID="cbLive" runat="server" />
    </div>
    <div style="clear:both"></div>
</asp:Panel>
