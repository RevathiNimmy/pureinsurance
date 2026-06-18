<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    ClassName="PopupMenuControl" 
    Inherits="WebUserControls_PopupMenuControl" 
 Codebehind="PopupMenuControl.ascx.cs" %>
<asp:Panel ID="popupMenu" runat="server" CssClass="PopupMenuStyle" style="display:none;">
    <asp:LinkButton ID="lbView" runat="server" CommandName="View" CommandArgument="" Text="View" />
    <asp:LinkButton ID="lbEdit" runat="server" CommandName="Edit" CommandArgument="" Text="Edit" />
    <asp:LinkButton ID="lbEditRules" runat="server" CommandName="EditRules" CommandArgument="" Text="Edit Rules" Visible="false" />
    <asp:LinkButton ID="lbGrids" runat="server" CommandName="ListGrids" CommandArgument="" Text="Grids" Visible="false" />
</asp:Panel>    
<ajax:HoverMenuExtender ID="hoverMenu" runat="server"  
    PopupControlID="popupMenu"
    PopupPosition="Right"
    HoverCssClass="hoverrowstyle"
    TargetControlID="popupMenu"
    PopDelay="50" />

