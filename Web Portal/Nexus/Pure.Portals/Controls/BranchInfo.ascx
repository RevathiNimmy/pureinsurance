<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BranchInfo.ascx.vb" Inherits="Controls_BranchInfo" %>
<div class="branch-details">
    <div class="branch-rounded">
        <h3>
            <asp:Label ID="lblHeader" Text="<%$ Resources:lbl_Header %>" runat="server"></asp:Label>
        </h3>
        <ul>
            <asp:PlaceHolder ID="PnlBranchName" runat="server" Visible="false">
                <li>
                    <asp:Label ID="lblBranch" runat="server" Text="<%$ Resources:lbl_Branch %>" AssociatedControlID="hypChangeBranchName"></asp:Label>
                    <asp:Label ID="lblBranchName" runat="server"></asp:Label>
                </li>
                <li>
                    <asp:HyperLink ID="hypChangeBranchName" runat="server" CssClass="thickbox" Text="<%$ Resources:lbl_ChangeBranch %>" Visible="false"></asp:HyperLink>
                </li>
            </asp:PlaceHolder>
        </ul>
    </div>
</div>
