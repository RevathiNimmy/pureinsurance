<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Header.ascx.vb" Inherits="UserControl_Header" %>
<link href="../App_Themes/GlobalTheme/GlobalStyleSheet.css" rel="stylesheet" type="text/css" />
<table width="100%" style="height: 99px">
    <tr>
        <td align="left" valign="bottom" >
            &nbsp;<asp:Image ID="Image1" ImageUrl="~/Images/Top_header.gif" ImageAlign="Middle" runat="server" />
        </td>
    </tr>
    <tr>
        <td align ="right" class="top_nav" valign="bottom" >
        <a href ="../UIIC_demo/HomePage.aspx"><img src="../Images/home_icon.gif" alt="Go to Home" border="0" /></a>
        </td>
    </tr>
</table>
