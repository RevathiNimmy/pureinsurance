<%@ Page 
    Language="C#" 
    MasterPageFile="~/MainMasterPage.master" 
    AutoEventWireup="true" 
    Inherits="AppError" 
    Title="Untitled Page" 
    Codebehind="AppError.aspx.cs" 
%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

<h2>
<span style="color:Red;"><%=Request["aspxerrorpath"].ToString() %></span> has caused an unexpected error
</h2>

</asp:Content>

