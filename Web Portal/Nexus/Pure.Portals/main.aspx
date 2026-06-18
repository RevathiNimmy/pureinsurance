<%@ Page Language="VB" MasterPageFile="~/Default.master" AutoEventWireup="false"
    CodeFile="main.aspx.vb" Inherits="Nexus.main" %>

<asp:Content ID="cntMainBody" runat="server" ContentPlaceHolderID="cntMainBody">
    <div id="main">
        
    
            
        
                
                
            
                    
                    <div class="card">
                        <div class="card-body clearfix">
                            
                            <div class="card-heading"><h1>
                                <asp:Label ID="LblTitle" runat="server" Text="Label"></asp:Label></h1></div>
                            <asp:Literal ID="ltContent" runat="server" EnableViewState="False"></asp:Literal>
                        </div>
                        
                    </div>
                </div>
</asp:Content>
