<%@ Control Language="VB" AutoEventWireup="false" CodeFile="LoginStatus.ascx.vb"
    Inherits="Nexus.LoginStatus" %>

<ul class="dropdown-menu dropdown-menu-scale pull-right text-color" aria-labelledby="nav-link">
    <li>
        <asp:PlaceHolder ID="PnlBranchName" runat="server">
            <asp:HyperLink ID="hypChangeBranchName" runat="server" data="modal" Visible="false"><i class="fa fa-map-marker" aria-hidden="true"></i> Change Branch</asp:HyperLink>
        </asp:PlaceHolder>
    </li>
    <li>
        <asp:LinkButton ID="lbtnChangePassword" runat="server" CausesValidation="false"><i class="fa fa-lock" aria-hidden="true"></i> Change Password</asp:LinkButton>
    </li>
    <li class="divider"></li>
    <li><a id="navLogout" runat="server" href="~/logout.aspx"><i class="fa fa-power-off" aria-hidden="true"></i> Logout</a></li>
</ul>

