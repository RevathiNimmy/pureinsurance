<%@ Page Language="VB" AutoEventWireup="false" CodeFile="calendar.aspx.vb" Inherits="Nexus.Controls_calendar"
    MasterPageFile="~/default.master" %>

<%@ Register TagPrefix="MM" Namespace="UnStyledCalendar" Assembly="UnStyledCalendar" %>
<asp:Content ID="cntMainBody" runat="server" ContentPlaceHolderID="cntMainBody">
    <div id="Controls_calendar">
        <input id="txtHidden" type="hidden" runat="server">
        <input id="txtAddHidden" type="hidden" runat="server">
        <table class="calendar_main">
            <tr>
                <td align="left">
                    <asp:DropDownList ID="cboMonths" runat="server" Height="25px" Width="130px" AutoPostBack="True" CssClass=" form-select">
                        <asp:ListItem Value="1">January</asp:ListItem>
                        <asp:ListItem Value="2">February</asp:ListItem>
                        <asp:ListItem Value="3">March</asp:ListItem>
                        <asp:ListItem Value="4">April</asp:ListItem>
                        <asp:ListItem Value="5">May</asp:ListItem>
                        <asp:ListItem Value="6">June</asp:ListItem>
                        <asp:ListItem Value="7">July</asp:ListItem>
                        <asp:ListItem Value="8">August</asp:ListItem>
                        <asp:ListItem Value="9">September</asp:ListItem>
                        <asp:ListItem Value="10">October</asp:ListItem>
                        <asp:ListItem Value="11">November</asp:ListItem>
                        <asp:ListItem Value="12">December</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td align="right">
                    <asp:DropDownList ID="cboYears" runat="server" Height="25px" Width="88px" AutoPostBack="True" CssClass=" form-select"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <MM:UnStyledCalendar ID="Calendar1" runat="server" DayNameFormat="FirstLetter" CellPadding="1" CssClass="calendar">
                        <SelectedDayStyle CssClass="selectedday"></SelectedDayStyle>
                        <TodayDayStyle CssClass="today"></TodayDayStyle>
                        <SelectorStyle CssClass="selector"></SelectorStyle>
                        <WeekendDayStyle CssClass="weekendday"></WeekendDayStyle>
                        <OtherMonthDayStyle CssClass="othermonthday"></OtherMonthDayStyle>
                        <NextPrevStyle CssClass="nextprev">
                        </NextPrevStyle>
                        <DayHeaderStyle CssClass="dayheader"></DayHeaderStyle>
                        <TitleStyle CssClass="title"></TitleStyle>
                    </MM:UnStyledCalendar>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2" style="padding-top: 6px;">
                    <input id="cmdAccept" type="button" alt="Accept" value="OK" name="cmdAccept" runat="server">
                    <input id="cmdCancel" type="button" value="Cancel" name="cmdCancel" runat="server" onclick="window.close();">
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
