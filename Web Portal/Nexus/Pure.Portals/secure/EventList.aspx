<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EventList.aspx.vb" Inherits="Nexus.secure_EventList"
    MasterPageFile="~/Default.master" EnableViewState="true" %>

<%@ Register Src="~/Controls/EventList.ascx" TagName="EventList" TagPrefix="uc1" %>
<asp:Content ID="cntMainBody" runat="server" ContentPlaceHolderID="cntMainBody">
    <asp:ScriptManager ID="smEventDetails" runat="server"></asp:ScriptManager>
    <div id="secure_EventList">
        <div class="card">
            <uc1:EventList ID="ucEventList" runat="server"></uc1:EventList>
        </div>
    </div>
</asp:Content>
