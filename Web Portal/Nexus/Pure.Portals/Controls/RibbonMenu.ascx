<%--<%@ Control Language="VB" AutoEventWireup="false" Inherits="Nexus.Controls_RibbonMenu, Pure.Portals" %>--%>
<%@ Control Language="VB" AutoEventWireup="false" CodeFile="RibbonMenu.ascx.vb" Inherits="Nexus.Controls_RibbonMenu" %>
<script>
    //$(document).ready(function () {
    //    $("a[id='nav-s-home'] span:first").after('<span class="nav-icon"><i class="icon glyphicon glyphicon-home"></i></span>');
    //    $("a[id='nav-s-client'] span:first").after('<span class="nav-icon"><i class="icon fa fa-users"></i></span>');
    //    $("a[id='nav-s-accounts'] span:first").after('<span class="nav-icon"><i class="icon fa fa-credit-card-alt"></i></span>');
    //    $("a[id='nav-s-favourites'] span:first").after('<span class="nav-icon"><i class="icon glyphicon glyphicon-heart"></i></span>');
    //    $("a[id='nav-s-reports'] span:first").after('<span class="nav-icon"><i class="icon fa fa-bar-chart"></i></span>');
    //    $("a[id='nav-s-Others'] span:first").after('<span class="nav-icon"><i class="icon fa fa-th-large"></i></span>');
    //    $("a[id='nav-s-claim'] span:first").after('<span class="nav-icon"><i class="icon fa fa-legal"></i></span>');
    //    $("a[id='nav-s-policy'] span:first").after('<span class="nav-icon"><i class="icon  fa fa-briefcase"></i></span>');
    //});
    
    </script>
<!-- container for logo and global nav -->
<asp:Repeater ID="rptToolbar" runat="server" Visible="false">
    <ItemTemplate>
        <li id="toolbaritem" runat="server">
            <a href='#<%# CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("id").Value %>'>
                <span class="pull-right text-muted">
                    <i class="fa fa-caret-down"></i>
                </span>
                <i class="pull-right up"></i>
                <i class="icon mdi-toggle-radio-button-on i-20"></i>
                <span><%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("title").Value%></span>
            </a>
        </li>
    </ItemTemplate>
</asp:Repeater>
       
        <asp:Repeater ID="rptCategory" runat="server">
            <ItemTemplate>
                <li id="toolbaritem" runat="server" class='<%# GetCategoryClass(Container.ItemIndex) %>'>
                    <a href="#" id='nav-<%# CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("id").Value %>'>

                        <span class="nav-caret"><i class="fa fa-caret-down"></i></span>
                        <span class="nav-icon"><i id="RptCategoryI" runat="server"></i></span>
                        <span class="nav-text" runat="server" id="RptCategorySpanText"><%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("title").Value%></span>
                        
                    </a>
                    <ul id='nav-sub-<%# CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("id").Value %>' class="nav-sub nav-mega">
                        <asp:Repeater ID="rptSection" runat="server" OnItemDataBound="rptSection_ItemDataBound">
                            <ItemTemplate>
                                <li class="nav-sub-hedding">
                                    <%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("title").Value%>
                                </li>
                                <asp:Repeater ID="rptItem" runat="server" OnItemDataBound="rptItem_ItemDataBound">
                                    <ItemTemplate>
                                        <li>
                                            <a id="A1" runat="server">
                                                <span class="nav-text"><%#CType(Container.DataItem, System.Xml.Linq.XElement).Attribute("title").Value%></span>
                                            </a>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </li>
            </ItemTemplate>
        </asp:Repeater>



