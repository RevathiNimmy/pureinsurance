<%@ Control Language="VB" AutoEventWireup="false" CodeFile="HelpLink.ascx.vb" Inherits="Nexus.controls_HelpLink"
    EnableViewState="false" %>
<div id="Controls_HelpLink">
    <div class="panel" id="helpText" runat="server">
        <p>
            <a href="#" class="helplink" onclick="helptoggle(this); return false;"><span>Help on/off</span></a></p>
        <div class="clearer">
        </div>
        <div class="help">
            <div class="point">
            </div>
            <div class="text">
                <asp:Label ID="lblHelpText" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <!-- END help-->
        <hr>
    </div>
</div>
<!-- END panel -->
