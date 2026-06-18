<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UserInfo.ascx.vb" Inherits="Controls_UserInfo" %>
<%--<%@ Register Src="~/Controls/LoginStatus.ascx" TagName="LoginStatus" TagPrefix="uc1" %>--%>
<div id="Controls_UserInfo" style="font-size:xx-small">
    <%-- <div class="p hidden-folded grey-100" style="background-image: url('../../images/bg.png'); background-size: cover">
        <div class="rounded w-64 bg-white inline pos-rlt">
            <img id="userImage" runat="server" src="~/images/a0.jpg" class="img-responsive rounded" />
        </div>
        <a class="block m-t-sm" ui-toggle-class="hide, show" target="#nav, #account">
            <span class="pull-right auto">
                <i class="fa inline fa-caret-down"></i>
                <i class="fa none fa-caret-up"></i>
            </span>
            <asp:Label ID="lblUsername" CssClass="block font-bold" runat="server"></asp:Label>
            <asp:Label ID="lblCompanyName" CssClass="block font-bold" Visible="false" runat="server"></asp:Label>

            <asp:Label ID="lblBranch" runat="server" Text="Branch:"></asp:Label>
            <asp:Label ID="lblBranchName" runat="server" Text="No Branch"></asp:Label>
        </a>
    </div>--%>
   <%-- <asp:PlaceHolder ID="phldrPasswordExpire" runat="server" Visible="false" >
        <asp:Literal ID="ltPasswordExpireWarning" runat="server" ></asp:Literal>
    </asp:PlaceHolder>--%>
</div>

<a id="nav-link" title="Application Setting" class="d-flex align-items-center" data-bs-toggle="dropdown" aria-expanded="false" href="#" >
    <%--<i class="fa fa-ellipsis-v fa-lg"></i>--%>
    <span class="hide">Setting</span>
    <span style="display:none;">
        <img id="userImage" runat="server" src="~/images/a0.jpg" class="img-responsive w-32 rounded" />
    </span>
    <span class="mx-2 d-none l-h-1x d-lg-block">
         <div style="font-size:smaller">
	    <asp:PlaceHolder ID="phldrPasswordExpire" runat="server" Visible="false">
        <asp:Literal ID="ltPasswordExpireWarning" runat="server"></asp:Literal>
	    </asp:PlaceHolder>
	    </div> 
        <asp:Label ID="lblUsername" CssClass="block font-bold" runat="server"></asp:Label>
        <asp:Label ID="lblCompanyName" CssClass="block font-bold" Visible="false" runat="server"></asp:Label>
        <asp:Label ID="lblBranch" runat="server" Text="Branch - "></asp:Label>
        <asp:Label ID="lblBranchName" runat="server" Text="No Branch"></asp:Label>
    </span>
</a>
<%--<li class="dropdown d-flex align-items-center">
    <a href="#" data-bs-toggle="dropdown" class="d-flex align-items-center">
       
    </a>
</li>--%>
