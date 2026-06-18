<%@ Page Language="VB" MasterPageFile="~/Default.master" AutoEventWireup="false"
    CodeFile="LoggedIn.aspx.vb" Inherits="Nexus.LoggedIn" %>

<asp:Content ID="cntMainBody" runat="server" ContentPlaceHolderID="cntMainBody">
    <div id="LoggedIn">
        <div class="home-cms">
            <h1 class="filled"><asp:Label ID="LblTitle" runat="server" EnableViewState="False"></asp:Label></h1>
            <asp:Literal id="ltContent" runat="server" EnableViewState="False"></asp:Literal>
        </div>
    </div>
    <div class="home-side-column">
        <div class="home-promos">
            <ul class="promos">
	        <li class="quickquote">
	        <!--Currently Quick Quote is not working correctly , So link is commnented but it should be active-->
	        <%--<a href="Products/Retailers/MainDetails.aspx?newquote=true">Quick Quote <br /><span>Get an instant Quotation</span></a>--%>
	       <a href="#">Quick Quote<br><span>Get an instant Quotation</span></a>
	        </li>
	        <li class="li-addclient"><a href="secure/agent/PersonalClientDetails.aspx?mode=add">Add a new Client</a></li>
                <li class="li-searchcp"><a href="secure/agent/FindClient.aspx">Search for a Client<br> or Policy</a> </li>
                <li class="li-servicingcp"><a href="#">Servicing your Clients &amp; their Policies</a></li>
            </ul>
        </div>
    </div>
</asp:Content>
