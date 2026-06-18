<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Events.aspx.vb" Inherits="Nexus.secure_Events"
    MasterPageFile="~/Default.master" EnableViewState="true" %>

<%@ Register Src="~/Controls/Events.ascx" TagName="Events" TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" runat="server" ContentPlaceHolderID="cntMainBody">
    <asp:ScriptManager ID="smEventDetails" runat="server"></asp:ScriptManager>
    <div id="secure_EventList">
        <div class="card">
            <uc1:Events ID="ucEvents" runat="server"></uc1:Events>
        </div>
    </div>
</asp:Content>
