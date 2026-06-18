<%@ Control Language="VB" AutoEventWireup="false" CodeFile="FindParty.ascx.vb" Inherits="Nexus.Controls_FindParty" EnableViewState="false" %>
<div id="Controls_FindParty" class="form-group form-group-sm ">
    <asp:Label ID="lblFindParty" runat="server" AssociatedControlID="txtPartyName" CssClass="col-md-4 col-sm-3 control-label"
        Text="<%$ Resources:lbl_FindParty %>" />
    <div class="col-md-8 col-sm-9">
        <div class="input-group">
            <asp:TextBox ID="txtPartyName" runat="server" AutoPostBack="true" CssClass="form-control"></asp:TextBox>
            <asp:TextBox ID="hPartyType" runat="server" Value="" CssClass="form-control" Style="display: none" />
            <span class="input-group-btn">
                <asp:LinkButton ID="btnFindParty" runat="server" SkinID="btnModal" CausesValidation="false">
                    <i class="fa fa-search"></i>
                    <span class="btn-fnd-txt">TPA</span>
                </asp:LinkButton>
            </span>
        </div>
        <asp:HiddenField ID="hPartyKey" runat="server" Value="0" />
        <asp:HiddenField ID="hPartyCode" runat="server" Value="0" />
    </div>
</div>
