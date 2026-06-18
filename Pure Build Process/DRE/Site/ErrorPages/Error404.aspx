<%@ Page 
    Language="C#" 
    MasterPageFile="~/MainMasterPage.master" 
    AutoEventWireup="true" 
    Inherits="Error404" 
    Title="Untitled Page" 
    Codebehind="Error404.aspx.cs" 
%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<h2>
    Resource <span style="color:Red;"><%=Request["aspxerrorpath"].ToString() %></span> not found
</h2>
</asp:Content>

