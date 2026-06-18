<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ClientInfo.ascx.vb" Inherits="Nexus.ClientInfo" %>

<div id="Controls_ClientInfo">
    <div class="streamline b-l b-dark m-b">
        <asp:PlaceHolder ID="phldrClientName" runat="server" Visible="false">
            <div class="sl-item sl-item-md">
                <div class="sl-icon">
                    <i class="fa fa-user"></i>
                </div>
                <div class="sl-content">
                    <div class="text-dk ">
                        <asp:Label ID="lblClientName" runat="server" Text="<%$ Resources:lbl_Client %>" AssociatedControlID="hypClientName"></asp:Label>
                        <asp:Label ID="ltClientName" SkinID="badgeInfo" runat="server"></asp:Label>
                        <asp:HyperLink ID="hypClientName" SkinID="lnkHInfo" runat="server"></asp:HyperLink>
                    </div>
                </div>
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="phldrCompanyName" runat="server" Visible="false">
            <div class="sl-item sl-item-md">
                <div class="sl-icon">
                    <i class="fa fa-twitter"></i>
                </div>
                <div class="sl-content">
                    <div class="text-dk ">
                        <asp:Label ID="lblCompanyName" runat="server" Text="<%$ Resources:lbl_Company %>" AssociatedControlID="hypCompanyName"></asp:Label>
                        <asp:Label SkinID="badgeInfo" ID="ltCompanyName" runat="server"></asp:Label>
                    </div>
                    <p>
                        <asp:HyperLink ID="hypCompanyName" SkinID="lnkHInfo" runat="server"></asp:HyperLink>
                    </p>
                </div>
            </div>

        </asp:PlaceHolder>
        <div class="sl-item">
            <div class="sl-content">
                <div class="text-dk">
                    <asp:HyperLink ID="hypClientDocs" SkinID="lnkHInfo" data="modal" runat="server" Text="<%$ Resources:hypDocs %>"></asp:HyperLink>
                </div>
            </div>
        </div>
        <div runat="server" id="liClientCode" class="sl-item sl-item-md">
            <div class="sl-icon">
                <i class="glyphicon glyphicon-barcode"></i>
            </div>
            <div class="sl-content">
                <div class="text-dk ">
                    <asp:Label ID="lblClientCode" runat="server" Text="<%$ Resources:lbl_ClientCode %>" AssociatedControlID="ltClientCode"></asp:Label>
                    <asp:Label ID="ltClientCode" SkinID="badgeInfo" runat="server"></asp:Label>
                </div>
            </div>
        </div>
        <asp:PlaceHolder runat="server" ID="plcPolicy" Visible="false">
            <div class="sl-item sl-item-md">
                <div class="sl-icon">
                    <i class="glyphicon glyphicon-hand-right"></i>
                </div>
                <div class="sl-content">
                    <div class="text-dk ">
                        <asp:Label ID="lblRef" runat="server" Text="<%$ Resources:lbl_Ref %>" AssociatedControlID="lblPolicyRef"></asp:Label>
                        <asp:Label SkinID="badgeInfo" ID="lblPolicyRef" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="sl-item">
                <div class="sl-content">
                    <div class="text-dk">
                        <asp:HyperLink ID="hypQuoteDocs" SkinID="lnkHInfo" data="modal" runat="server" Text="<%$ Resources:hypDocs %>"></asp:HyperLink>
                    </div>
                </div>
            </div>
            <div class="sl-item">
                <div class="sl-content">
                    <div class="text-dk">
                        <asp:HyperLink ID="hypEventList" SkinID="lnkHInfo" runat="server" Text="<%$ Resources:hyp_Events %>"></asp:HyperLink>
                    </div>
                </div>
            </div>
        </asp:PlaceHolder>
    </div>
</div>
