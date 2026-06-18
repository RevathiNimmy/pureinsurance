<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="SelectAccount.aspx.vb" Inherits="Nexus.Modal_SelectAccount" Title="Untitled Page" %>


<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <script language="javascript" type="text/javascript">
        function GetAccountType() {

            self.parent.tb_remove();
            self.parent.GetAccountType(document.getElementById("ctl00_cntMainBody_rbClient").checked, document.getElementById("ctl00_cntMainBody_rbAgent").checked)
        }
    </script>
    <div id="Modal_SelectAccount">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_SelectAccount_Title %>"></asp:Literal></h1>
            </div>
            <asp:Panel ID="PnlSelectAccount" runat="server" DefaultButton="btnOK" CssClass="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label runat="server" ID="lblSelectAccount" Text="<%$ Resources:lbl_Header %>"></asp:Label></legend>

                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:RadioButton runat="server" ID="rbClient" Text="Client" GroupName="PayeeParty" Checked="true" CssClass="asp-radio"></asp:RadioButton>

                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:RadioButton runat="server" ID="rbAgent" Text="Agent" GroupName="PayeeParty" CssClass="asp-radio"></asp:RadioButton>
                    </div>
                </div>
            </asp:Panel>
            <div class="card-footer">
                <asp:LinkButton ID="btnOK" runat="server" TabIndex="2" Text="<%$ Resources:btnOK %>" OnClientClick="GetAccountType()" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
