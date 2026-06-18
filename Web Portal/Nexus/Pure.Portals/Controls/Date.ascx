<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Date.ascx.vb" Inherits="Nexus.controls_Date"
    EnableViewState="false" %>

<script language="Javascript" type="text/javascript">

    //TODO: NEED TO REPLACE THE RELATIVE PATH GIVEN IN WINDOW.OPEN - MB - 16-MAY-07
    function GetDate(CtrlName) {
        ChildWindow = window.open('../../controls/calendar.aspx?FormName=' + document.forms[0].name +
        '&CtrlName=' + CtrlName, "PopUpCalendar", "width=245,height=280,top=200,left=450,toolbars=no,scrollbars=no,status=no,resizable=no");
    }

    function CheckWindow() {
        ChildWindow.close();
    }
</script>

<script type="text/javascript" language="Javascript">

    function ClearDate(box) {
        if (box.value == box.defaultValue) {
            box.value = "";
        }
    }
   
</script>

<div id="Controls_Date">
    <div class="card">
        <div class="card-body clearfix">
            
            <asp:TextBox ID="TxtRenewalDate" TabIndex="6" Width="100" runat="server" Text="DD/MM/YYYY" CssClass="field-mandatory" onfocus="ClearDate(this);"></asp:TextBox>
            <asp:HyperLink ID="hypCalendarPopup" runat="server">
                <asp:Image AlternateText="<%$ Resources:imgCalendar_AltText %>" SkinID="calendar.icon" ID="ImgCalendar" runat="server"></asp:Image></asp:HyperLink>
            <asp:RequiredFieldValidator ID="vldrqdDate" Display="Dynamic" ControlToValidate="TxtRenewalDate" runat="server" ErrorMessage="<%$ Resources:rvDate_ErrorMsg %>" SetFocusOnError="True"></asp:RequiredFieldValidator>
            <asp:RangeValidator ID="rvDate" runat="Server" Type="Date" MinimumValue="" MaximumValue="" Display="Dynamic" ErrorMessage="<%$ Resources:vldrqdDate_ErrorMsg %>" ControlToValidate="TxtRenewalDate" SetFocusOnError="True" Text="Invalid"></asp:RangeValidator>
        </div>
        
    </div>
</div>
