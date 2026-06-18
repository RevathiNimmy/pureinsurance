<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master"
    CodeFile="Register.aspx.vb" Inherits="Nexus.Register" %>

<%@ Register Src="~/controls/PersonalClient.ascx" TagName="PersonalClient" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/CorporateClient.ascx" TagName="CorporateClient" TagPrefix="uc2" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <asp:ScriptManager ID="ScriptManagerRegister" runat="server">
    </asp:ScriptManager>
    <div id="Register">
        
    
             
                
                
        
                    
                    <h1>
                        <asp:Literal ID="lblHeading" runat="server" Text="<%$ Resources:lbl_Register_heading %>"></asp:Literal></h1>
                    <p>
                        <asp:Literal ID="lblPageInfo" runat="server" Text="<%$ Resources:lbl_PageInfo %>"></asp:Literal>
                    </p>
                    <asp:Panel ID="PnlRegister" runat="server">
                        <div>
                            <uc1:PersonalClient ID="PersonalClient" runat="server" RegisterClient="True" Visible="False"></uc1:PersonalClient>
                            <uc2:CorporateClient ID="CorporateClient" runat="server" RegisterClient="True" Visible="False"></uc2:CorporateClient>
                            <asp:CustomValidator ID="vldEmailExists" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_EmailExist %>"></asp:CustomValidator>
                            <asp:ValidationSummary ID="ValidationSummary" DisplayMode="BulletList" HeaderText="<%$ Resources:lbl_ValidationSummary %>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
                            <div class="clear">
                            </div>
                        </div>
                    </asp:Panel>
               <div class="card">
                <div class="notice">
                    <p id="PnlCheck" runat="server">
                        <asp:Literal ID="lit_Terms" Text="<%$ Resources:lbl_Terms %>" runat="server"></asp:Literal>
                        <asp:CheckBox ID="ChkConfirmation" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                        <asp:CustomValidator ID="vldConfirmation" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_ConfirmTerms %>" ClientValidationFunction="EnsureChecked"></asp:CustomValidator>
                    </p>
                </div>
                </div>
                <div class="card-footer">
                    <asp:LinkButton ID="BtnSubmit" runat="server" Text="<%$ Resources:AppResources.strings, btn_Submit %>" CausesValidation="true" SkinID="btnPrimary"></asp:LinkButton>
                </div>
            </div>
</asp:Content>
