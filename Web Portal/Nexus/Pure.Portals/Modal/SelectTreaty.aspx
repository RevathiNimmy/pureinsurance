<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="SelectTreaty.aspx.vb" Inherits="Nexus.Modal_SelectTreaty" Title="Untitled Page" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="Modal_SelectTreaty">
        <asp:ValidationSummary ID="ValidationSummary" runat="server" DisplayMode="BulletList" ShowSummary="true" CssClass="validation-summary" ValidationGroup="TreatyGroup"></asp:ValidationSummary>
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="Add Treaty"></asp:Literal></h1>
            </div>
            <div class="card-body" style="display:flex; align-items:center; gap:10px; padding:10px 15px;">
                <asp:Label ID="lblTreaty" runat="server" AssociatedControlID="ddlRIPropTreaties" Text="Treaty" style="white-space:nowrap;"></asp:Label>
                <asp:DropDownList ID="ddlRIPropTreaties" runat="server" CssClass="form-control form-select" AutoPostBack="True" style="flex:1;"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvTreaty" runat="server" InitialValue="" ControlToValidate="ddlRIPropTreaties" ErrorMessage="<%$ Resources:err_SelectTreaty %>" Display="None" SetFocusOnError="True" ValidationGroup="TreatyGroup"></asp:RequiredFieldValidator>
                <asp:LinkButton ID="btnOk" SkinID="btnPrimary" runat="server" Text="Ok" CausesValidation="true" ValidationGroup="TreatyGroup" style="white-space:nowrap;"></asp:LinkButton>
                <asp:LinkButton ID="btnCancel" SkinID="btnSecondary" runat="server" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" style="white-space:nowrap;"></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
