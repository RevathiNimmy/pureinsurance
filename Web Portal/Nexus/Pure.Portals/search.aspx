<%@ Page Language="VB" AutoEventWireup="false" CodeFile="search.aspx.vb" Inherits="Nexus._Search"
    MasterPageFile="~/Default.master" %>

<%@ Register Src="~/Controls/SearchBox.ascx" TagName="SearchBox" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/SearchResults.ascx" TagName="SearchResults" TagPrefix="uc2" %>
<asp:Content ID="cntMainBody" runat="server" ContentPlaceHolderID="cntMainBody">
    <div id="search">
        
    
            
        
                
                
            
                    
                    <uc1:SearchBox ID="SearchBox" runat="server" Mode="Standard"></uc1:SearchBox>
                    <uc2:SearchResults ID="SearchResults" runat="server"></uc2:SearchResults>
                </div>
</asp:Content>
