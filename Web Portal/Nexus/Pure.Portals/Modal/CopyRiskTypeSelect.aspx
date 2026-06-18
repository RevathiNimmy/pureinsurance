<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CopyRiskTypeSelect.aspx.vb"
    Inherits="Modal_CopyRiskTypeSelect" MasterPageFile="~/default.master" EnableViewState="false" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <div id="Modal_CopyRiskTypeSelect">
        <div class="card">
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:lbl_Header %>"></asp:Label>
                    </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCopyRiskType" runat="server" Text="<%$ Resources:lbl_CopyRiskType %>" AssociatedControlID="ddlCopyRiskType" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="ddlCopyRiskType" runat="server" CssClass="form-control form-select">
                                <asp:ListItem Text="Duplicate" Value="D"></asp:ListItem>
                                <asp:ListItem Text="Comparative" Value="C"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer">
                <asp:LinkButton ID="btnSubmit" SkinID="btnPrimary" runat="server" Text="<%$ Resources:btn_Submit %>" OnClientClick="this.setAttribute('disabled','true');return true;"></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
