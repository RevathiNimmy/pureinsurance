<%@ Control Language="C#" AutoEventWireup="true" Inherits="WebUserControls_ChangePasswordControl"
    CodeBehind="ChangePasswordControl.ascx.cs" %>
<asp:Button ID="fakeButton" runat="server" Style="display: none;" />
<ajax:ModalPopupExtender ID="mpeChangePassword" runat="server" PopupControlID="pnlPopupChangePassword"
    TargetControlID="fakeButton" BehaviorID="ModalPopupChangePassword" BackgroundCssClass="washOut"
    CancelControlID="fakeButton" RepositionMode="None" DynamicServicePath="" Enabled="True" />
<asp:Panel ID="pnlPopupChangePassword" runat="server" Style="display: none;" CssClass="ChangePasswordDialog">
    <asp:UpdatePanel ID="UpdatePanelChangePassword" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="popupTitle">
                <asp:Label ID="lblHeadingChangePassword" runat="server" Text="Change Password" />
            </div>
            <div class="question">
                <asp:Label AssociatedControlID="tbPasswordOld" ID="lblPasswordOld" runat="server"
                    Text="Old Password:" />
                <asp:TextBox ID="tbPasswordOld" runat="server" TextMode="Password" MaxLength="255" />
                <%--<ajax:TextBoxWatermarkExtender ID="tbwmUsername" TargetControlID="tbPasswordOld" runat="server" WatermarkText="Type Your Old Password Here" WatermarkCssClass="watermark" />--%>
                <asp:RequiredFieldValidator ID="rfvPasswordOld" runat="server" ControlToValidate="tbPasswordOld"
                    Display="Static" ValidationGroup="ChangePasswordValidation" Text="*" ToolTip="Old password is mandatory" />
            </div>
            <div class="question">
                <asp:Label AssociatedControlID="tbPasswordNew" ID="lblPasswordNew" runat="server"
                    Text="New Password:" />
                <asp:TextBox ID="tbPasswordNew" runat="server" TextMode="Password" MaxLength="255" />
                <%--<ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" TargetControlID="tbPasswordNew" runat="server" WatermarkText="Type Your New Password Here" WatermarkCssClass="watermark" />--%>
                <asp:RequiredFieldValidator ID="rfvPasswordNew" runat="server" ControlToValidate="tbPasswordNew"
                    Display="Static" ValidationGroup="ChangePasswordValidation" Text="*" ToolTip="New password is mandatory" />
                <asp:RegularExpressionValidator ID="revPasswordNew" runat="server" ControlToValidate="tbPasswordNew"
                    Display="Static" ValidationGroup="ChangePasswordValidation" Text="*" ToolTip="Password must contain at least 8 characters including at least one uppercase 	character, lowercase character and number"
                    ValidationExpression="^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).*$" />
            </div>
            <div class="question">
                <asp:Label AssociatedControlID="tbConfirmPasswordNew" ID="lblConfirmPasswordNew"
                    runat="server" Text="Confirm Password:" />
                <asp:TextBox ID="tbConfirmPasswordNew" runat="server" TextMode="Password" MaxLength="255" />
                <%--<ajax:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" TargetControlID="tbConfirmPasswordNew" runat="server" WatermarkText="Type Your New Password Again Here" WatermarkCssClass="watermark" />--%>
                <asp:RequiredFieldValidator ID="rfvConfirmPasswordNew" runat="server" ControlToValidate="tbConfirmPasswordNew"
                    Display="Static" ValidationGroup="ChangePasswordValidation" Text="*" ToolTip="Confirm password is mandatory" />
                <asp:CompareValidator ID="cvConfirmPasswordNew" runat="server" Display="Static" ControlToValidate="tbConfirmPasswordNew"
                    ControlToCompare="tbPasswordNew" ValidationGroup="ChangePasswordValidation" Text="*"
                    ToolTip="New password and confirm password should be same" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnChangePassword" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="buttons PasswordChangeButtons">
        <asp:Button ID="btnChangePassword" runat="server" OnClick="btnChangePassword_Click"
            Text="Change" ValidationGroup="ChangePasswordValidation" />
        <asp:Button ID="btnCancelPopup" runat="server" OnClick="btnCancelPopup_Click" Text="Cancel" />
    </div>
</asp:Panel>
