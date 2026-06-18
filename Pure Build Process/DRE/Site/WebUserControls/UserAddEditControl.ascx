<%@ Control Language="C#" AutoEventWireup="true" className="UserAddEditControl" Inherits="WebUserControls_UserAddEditControl" Codebehind="UserAddEditControl.ascx.cs" %>
<%@ Register Src="~/WebUserControls/ChangePasswordControl.ascx" TagName="ChangePasswordControl" TagPrefix="wuc" %>
<asp:Panel ID="pnlUserAddEditControl" runat="server" Width="100%">
    <asp:HiddenField ID="hfUserKey" runat="server" />
        <asp:UpdatePanel runat="server" id="UpdatePanel" UpdateMode="Conditional">
            <ContentTemplate>    
                <div class="question">
                    <asp:Label AssociatedControlID="ddlSystem" runat="Server" ID="lblSystemName" Text="System Name:" />
                    <asp:DropDownList ID="ddlSystem" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlSystem_SelectedIndexChanged" />
                </div>                    
                <div class="question">
                    <asp:Label AssociatedControlID="ddlCustomer" runat="Server" ID="lblCustomerName" Text="Customer Name:" />
                    <asp:DropDownList ID="ddlCustomer" runat="server" />
                </div>                                
                <div class="question">
                    <asp:Label AssociatedControlID="tbLogon" runat="Server" ID="lblLogonName" Text="Logon Name:" />
                    <asp:TextBox ID="tbLogon" runat="server" MaxLength="255" Width="400px" />
                    <asp:CustomValidator ID="cvUniqueName" runat="server" Text="*" ValidationGroup="UserValidation"
                        ToolTip="Logon Name must be unique. Please note that other customers may be using this name." onservervalidate="cvUniqueName_ServerValidate" />
                    <ajax:TextBoxWatermarkExtender ID="tbweLogon" runat="server"
                        TargetControlID="tbLogon"
                        WatermarkText="Type Logon Name Here"
                        WatermarkCssClass="watermark" />                    
                </div>
                <div class="question">    
                    <asp:Label AssociatedControlID="lblPassword" ID="lblPassword" runat="server" Text="Password:" />
                    <asp:LinkButton ID="lbChangePassword" runat="server" Text="Change Password" OnClick="lbChangePassword_Click" />
                    <asp:LinkButton ID="lbResetPassword" runat="server" Text="Reset Password" OnClick="lbResetPassword_Click" />                
                </div>
                <div class="question">
                    <asp:Label AssociatedControlID="tbName" runat="Server" ID="lblUserName" Text="User Name:" />
                    <asp:TextBox ID="tbName" runat="server" MaxLength="255" Width="400px" />
                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="tbName"
                        ValidationGroup="UserValidation" Display="Static" Text="*" ToolTip="User name is mandatory"  />
                    <ajax:TextBoxWatermarkExtender ID="tbweName" runat="server"
                        TargetControlID="tbName"
                        WatermarkText="Type User Name Here"
                        WatermarkCssClass="watermark" />                    
                </div>
                <div class="question">
                    <asp:Label AssociatedControlID="tbDescription" runat="Server" ID="lblDescription" Text="Description:" />
                    <asp:TextBox ID="tbDescription" runat="server" MaxLength="2000" TextMode="MultiLine" Width="400px" Height="150px" />
                    <ajax:TextBoxWatermarkExtender ID="tbwmDescription" runat="server"
                        TargetControlID="tbDescription"
                        WatermarkText="Type User Description Here"
                        WatermarkCssClass="watermark" />                    
                </div>
                <div class="question">
                    <asp:Label AssociatedControlID="tbEmail" runat="Server" ID="lblEmail" Text="Email:" />
                    <asp:TextBox ID="tbEmail" runat="server" MaxLength="255" Width="400px" />
                    <ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server"
                        TargetControlID="tbEmail"
                        WatermarkText="Type Users Email Address Here"
                        WatermarkCssClass="watermark" />      
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                     ControlToValidate="tbEmail" Display="Static" Text="*" ToolTip="Email is mandatory" ValidationGroup="UserValidation" />                     
                    
                    <asp:RegularExpressionValidator ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" ID="revEmailAddress" runat="server"
                        ErrorMessage="Invalid Email Address" ToolTip="Invalid Email Address" Text="*" Display="Static" ControlToValidate="tbEmail" ValidationGroup="UserValidation" >
                    </asp:RegularExpressionValidator>                        
                </div>
                <div class="question">
                    <asp:Label AssociatedControlID="cbActive" runat="Server" ID="lblActive" Text="Active:" />
                    <asp:CheckBox ID="cbActive" runat="server" />
                </div>        
                <div class="question">
                    <asp:Label AssociatedControlID="cbSuperUser" runat="Server" ID="lblSuperUser" Text="Super User:" />
                    <asp:CheckBox ID="cbSuperUser" runat="server" />
                </div>
                <div class="question">
                    <asp:Label AssociatedControlID="cbAdministrator" runat="Server" ID="lblAdministrator" Text="Administrator:" />
                    <asp:CheckBox ID="cbAdministrator" runat="server" />
                </div>
                <div class="question">
                    <asp:Label AssociatedControlID="ddlAccessLevel" runat="Server" ID="lblAccessLevel" Text="Access Level:" />
                    <asp:DropDownList ID="ddlAccessLevel" AutoPostBack="false" runat="server" />
                </div>                                    
                <wuc:ChangePasswordControl ID="cpcChangePassword" runat="server" />
                <div style="clear:both"></div>
        </ContentTemplate>
    </asp:UpdatePanel>        
</asp:Panel>
