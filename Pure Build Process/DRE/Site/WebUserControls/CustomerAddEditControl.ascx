<%@ Control Language="C#" AutoEventWireup="true" className="CustomerAddEditControl" Inherits="WebUserControls_CustomerAddEditControl" Codebehind="CustomerAddEditControl.ascx.cs" %>
<%@ Register Src="~/WebUserControls/EmailAddressesControl.ascx" TagName="EmailAddressesControl" TagPrefix="wuc" %>
<asp:Panel ID="pnlCustomerAddEditControl" runat="server">

    <asp:HiddenField ID="hfCustomerKey" runat="server" />
    <asp:HiddenField ID="hfCretaedBy" runat="server" />

    <div class="question">
        <asp:Label AssociatedControlID="ddlSystem" runat="Server" ID="lblSystemName" Text="System Name:" />
        <asp:DropDownList ID="ddlSystem" AutoPostBack="false" runat="server"></asp:DropDownList>
    </div>                

    <div class="question">
        <asp:Label AssociatedControlID="tbName" runat="Server" ID="lblCustomerName" Text="Customer Name:" />

        <asp:TextBox ID="tbName" runat="server" MaxLength="255" Width="400px" />
                        
        <asp:CustomValidator 
            ID="cvUniqueCustomer" runat="server" ToolTip="Customer Name must be unique" 
            Text="*" OnServerValidate="cvUniqueCustomer_ServerValidate" ValidationGroup="CustomerValidation" />
        
        <ajax:TextBoxWatermarkExtender ID="tbweName" runat="server"
                TargetControlID="tbName"
                WatermarkText="Type Customer Name Here"
                WatermarkCssClass="watermark" />    
    </div>   
            
    <div class="question">
        <asp:Label AssociatedControlID="tbDescription" runat="Server" ID="lblDescription" Text="Customer Description:" />
        <asp:TextBox ID="tbDescription" runat="server" MaxLength="2000" TextMode="MultiLine" Width="400px" Height="150px" />
        <ajax:TextBoxWatermarkExtender ID="tbwmDescription" runat="server"
            TargetControlID="tbDescription"
            WatermarkText="Type Customer Description Here"
            WatermarkCssClass="watermark" />                    
    </div>

    <div class="question">
        <asp:Label AssociatedControlID="ddlStatus" runat="Server" ID="lblStatus" Text="Status:" />
        <asp:DropDownList ID="ddlStatus" runat="server" readonly="readonly" />
    </div>

    <div class="question">
        <asp:Label AssociatedControlID="cbNotifyAdmin" runat="Server" ID="lblNotifyAdmin" Text="Notify Admin:" />
        <asp:CheckBox ID="cbNotifyAdmin" runat="server" />
    </div>
            
    <div class="question">    
        <asp:Label runat="Server" AssociatedControlID="ddlStyleSheet" ID="lblStyleSheet" Text="Style Sheet:" />
        <asp:DropDownList  ID="ddlStyleSheet" runat="server" />
        <asp:CustomValidator ID="cvStyleSheet" runat="server"
                ControlToValidate="ddlStyleSheet" ValidationGroup="CustomerValidation" 
                OnServerValidate="cvStyleSheet_ServerValidate" Display="Static" Text="*" ToolTip="Stylesheet folder does not exist"  />                
    </div>

    <div class="question">
        <asp:Label AssociatedControlID="tbForcePasswordResetEvery" runat="Server" ID="lbForcePassword" Text="Force Password Reset Every:" />
        <asp:TextBox ID="tbForcePasswordResetEvery" runat="server" MaxLength="3" ToolTip="How many days to before the password is forcibly reset" />
        <asp:CustomValidator ID="cvtbForcePasswordResetEvery" runat="server"
                ValidationGroup="CustomerValidation" 
                OnServerValidate="cvtbForcePasswordResetEvery_ServerValidate" Display="Static" Text="*" ToolTip="A number greater than 0 must be entered"  />                
    </div>

    <div class="question">   
        <asp:Label runat="Server" AssociatedControlID="eacErrorEmailAddresses" ID="lblErrorEmailAddresses" Text="Error Email Addresses:" />
        <wuc:EmailAddressesControl ID="eacErrorEmailAddresses" ValidationGroup="ErrorEmail" runat="server" />
    </div>
    
    <div style="clear:both"></div>

</asp:Panel>
