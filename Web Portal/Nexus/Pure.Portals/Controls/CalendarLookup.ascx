<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CalendarLookup.ascx.vb"
    Inherits="Nexus.CalendarLookup" %>
<label id="lblCalenderIcon" class="input-group-btn" runat="server">
    <asp:LinkButton ID="btnCalendar" SkinID="btnCalendar" runat="server" href="#" OnClientClick="return false;">
         <i class="fa fa-calendar" aria-hidden="true"></i>
         <span class="btn-calc-txt">Address</span>
    </asp:LinkButton>
</label>
