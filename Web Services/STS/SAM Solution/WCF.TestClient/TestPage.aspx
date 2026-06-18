<%@ Page Title="SSP" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TestPage.aspx.vb" Inherits="WCF.TestClient.About" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <article>
        <p>        
            <asp:Button ID="btnGetUserDetail" runat="server" Text="Get User Detail" OnClick="btnGetUserDetail_Click" /><br />
            <asp:Button ID="btnFindParty" runat="server" Text="Find Party" OnClick="btnFindParty_Click" /><br />
            <asp:Button ID="btnAddParty" runat="server" Text="Add Party" OnClick="btnAddParty_Click" />
            <asp:Button ID="btnUpdateParty" runat="server" Text="Update Party" OnClick="btnUpdateParty_Click" />
        </p>

        <p>        
            
            
        </p>

        <p>        
            
            
        </p>
        <p>
            
        </p>
    </article>
</asp:Content>