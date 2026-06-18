<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SessionExpired.aspx.vb" Inherits="Nexus.SessionExpired"
    MasterPageFile="~/default.master" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="server">
    <div id="SessionExpired">
        
    
            
        
                
                
            
                    
                    <div class="card">
                        <div class="card-heading"><h1>
                            <asp:Label ID="lblTitle" runat="server" Text="<%$ Resources:lbl_Title %>"></asp:Label></h1></div>
                        <p>
                            <asp:Literal ID="ltContent" runat="server" Text="<%$ Resources:lt_Content %>"></asp:Literal>
                        </p>
                    </div>
                </div>
</asp:Content>
