<%@ Page Language="VB" MasterPageFile="~/default.master" AutoEventWireup="false"
    CodeFile="SelectBranch.aspx.vb" Inherits="Nexus.Modal_SelectBranch" Title="Untitled Page" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="Modal_SelectBranch">
        <div class="card">
            <div class="card-heading">
                <h1>
                    <asp:Literal ID="lblTitle" runat="server" Text="<%$ Resources:lbl_SelectBranch_Title %>"></asp:Literal></h1>
            </div>
            <asp:Panel ID="PnlSelectBranch" runat="server" DefaultButton="btnOK" CssClass="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label runat="server" ID="lblSelectBranch" Text="<%$ Resources:lbl_Header %>"></asp:Label></legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblBranch" runat="server" AssociatedControlID="ddlBranchCode" Text="<%$ Resources:lbl_SelectBranch %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlBranchCode" DataTextField="Description" DataValueField="Code" runat="server" CssClass="field-medium form-control form-select"></asp:DropDownList>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <div class="card-footer">
                <asp:LinkButton ID="btnOK" runat="server" TabIndex="2" Text="<%$ Resources:btnOK %>" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
