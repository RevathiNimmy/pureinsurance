<%@ Control Language="VB" AutoEventWireup="false" CodeFile="SearchBox.ascx.vb" Inherits="Nexus.controls_SearchBox" EnableViewState="true" %>

<div id="Controls_SearchBox">
<asp:MultiView ID="MultiView1" runat="server">
    <asp:View ID="vwStandardSearch" runat="server">
    <div class="panel">
        Please enter your search criteria<br><br>
        <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>&nbsp;
        <asp:LinkButton ID="cmdSearch" runat="server">Search</asp:LinkButton>&nbsp;
        <asp:LinkButton ID="cmdShowAdv" runat="server">Advanced</asp:LinkButton>    
    </div>
    </asp:View>
    <asp:View ID="vwAdvancedSearch" runat="server">
    <div class="panel">
        <strong>Advanced Search Panel</strong><br><br>
        Please enter your search criteria<br><br>
        <asp:TextBox ID="txtAdvSearchKeywords" runat="server"></asp:TextBox>&nbsp;
        <asp:LinkButton ID="cmdAdvSearch" runat="server">Search</asp:LinkButton><br>
        <asp:TextBox ID="txtPerPage" runat="server">5</asp:TextBox>&nbsp;matches per page<br>
        <asp:CheckBoxList ID="chkIncludeInSearch" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="asp-check">
            <asp:ListItem Selected="True">Page Content</asp:ListItem>
            <%--<asp:ListItem Selected="True">News</asp:ListItem>--%>
            <asp:ListItem Selected="True">Media/Documents</asp:ListItem>
        </asp:CheckBoxList>&nbsp; 
     </div>   
   </asp:View> 
</asp:MultiView>
</div>