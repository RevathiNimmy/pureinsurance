<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SelectBranch.aspx.vb" Inherits="Nexus.SelectBranch"
    MasterPageFile="~/Default.master" EnableViewState="true" %>

<%@ Register Src="~/Controls/MultiBranch.ascx" TagName="MultiBranch" TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" runat="server" ContentPlaceHolderID="cntMainBody">
    <div id="SelectBranch">
        <uc1:MultiBranch ID="ucMultiBranch" runat="server"></uc1:MultiBranch>
    </div>
</asp:Content>
