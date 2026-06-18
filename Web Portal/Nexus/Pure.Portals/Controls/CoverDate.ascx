<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CoverDate.ascx.vb" Inherits="Nexus.Controls_CoverDate"
    EnableViewState="false" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>

<script language="javascript" type="text/javascript">
    function ChangeEndDate(tsTimeScale, iPeriod, bTrueMonthlyPolicy, sMidnightRenewal) {
        //Day = 0, Week = 1, Month = 2, Year = 3
        dCoverStartDate = document.getElementById('<%=START_DATE__RISKBASE.ClientID %>').value;

        if (StartDateClientValidate(dCoverStartDate) == false) {
            document.getElementById('<%=START_DATE__RISKBASE.ClientID %>').value = "";
            document.getElementById('<%=END_DATE__RISKBASE.ClientID %>').value = "";
            return false;
        }

        var dtTempDate = new Date(dCoverStartDate);
        if (tsTimeScale == "0")//day
        {
            dtTempDate.setDate(dtTempDate.getDate() + parseInt(iPeriod));
        }
        else if (tsTimeScale == "1")//week
        {
            dtTempDate.setDate(dtTempDate.getDate() + (parseInt(iPeriod)) * 7);
        }
        else if (tsTimeScale == "2")//month
        {
            if (bTrueMonthlyPolicy.toLowerCase() == "true")//TrueMonthlyPolicy is True
            {//at the end of the month after period e.g 16/04 - 31/05

                dtTempDate.setMonth(dtTempDate.getMonth() + parseInt(iPeriod)); // first add the months using parameter iPeriod

                daysinMonth = daysInMonth(dtTempDate.getMonth(), dtTempDate.getYear()); //need days in the month to add

                dtTempDate.setDate(dtTempDate.getDate() + (parseInt(daysinMonth) - parseInt(dtTempDate.getDate())));
                //add the days: days in the month - date
            }
            else//TrueMonthlyPolicy is False, One month time e.g 10/8/2008 - 11/8/2008
            {
                dtTempDate.setMonth(dtTempDate.getMonth() + parseInt(iPeriod));
            }
        }
        else if (tsTimeScale == "3")//year
        {
            dtTempDate.setYear(dtTempDate.getYear() + parseInt(iPeriod));
        }

        if (sMidnightRenewal.toLowerCase() == "true")//If MidnightRenewal is true this means product need 365 days policy
        {
            dtTempDate.setDate(dtTempDate.getDate() - 1);
        }

        //dCoverEndDate=(dtTempDate.getMonth()+1)+"/"+dtTempDate.getDate()+"/"+dtTempDate.getYear()
        dCoverEndDate = dtTempDate.getDate() + "/" + (dtTempDate.getMonth() + 1) + "/" + dtTempDate.getYear()
        document.getElementById('<%=END_DATE__RISKBASE.ClientID %>').value = dCoverEndDate;
    }

    function daysInMonth(iMonth, iYear)//to calculte the days in a passed month and year
    {
        return 32 - new Date(iYear, iMonth, 32).getDate();
    }

    function StartDateClientValidate(valueDate) {
        if (valueDate == "")//If Start Date is blank 
        {
            return false;
        }
        else {
            var result, regEx; //Declare variables.
            regEx = new RegExp(/^((0?[13578]|10|12)(-|\/)(([1-9])|(0[1-9])|([12])([0-9]?)|(3[01]?))(-|\/)((19)([2-9])(\d{1})|(20)([01])(\d{1})|([8901])(\d{1}))|(0?[2469]|11)(-|\/)(([1-9])|(0[1-9])|([12])([0-9]?)|(3[0]?))(-|\/)((19)([2-9])(\d{1})|(20)([01])(\d{1})|([8901])(\d{1})))$/);  //Create regular expression object.
            result = regEx.test(valueDate);  //Test for match.

            return result;
        }
    }
 
</script>

<div id="Controls_CoverDate">
    <asp:Panel ID="PnlCoverDate" runat="server">
        <div class="card">
            <div class="card-body clearfix">
                
                <div class="form-horizontal">
                    <legend>
                        <asp:Label ID="lbl_Header" runat="server" Text="<%$ Resources:lbl_Header %>"></asp:Label></legend>
                    
                
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lbl_CoverStartDate" runat="server" Text="<%$ Resources:lbl_CoverStartDate %>" AssociatedControlID="START_DATE__RISKBASE" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9"><div class="input-group"><asp:TextBox ID="START_DATE__RISKBASE" runat="server" CssClass="field form-control"></asp:TextBox><uc1:CalendarLookup ID="calStartDate" runat="server" LinkedControl="START_DATE__RISKBASE" HLevel="2"></uc1:CalendarLookup></div></div>
                            
                            <asp:RequiredFieldValidator ID="vldCoverStartDate" Display="None" ControlToValidate="START_DATE__RISKBASE" runat="server" ErrorMessage="<%$ Resources:lbl_EnterStartDate %>" SetFocusOnError="True" ValidationGroup=""></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rangevldStartDate" runat="server" Display="none" ErrorMessage="<%$ Resources:lbl_InvalidStartDateFormat %>" ControlToValidate="START_DATE__RISKBASE" SetFocusOnError="True" ValidationGroup=""></asp:RangeValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lbl_CoverEndDate" runat="server" Text="<%$ Resources:lbl_CoverEndDate %>" AssociatedControlID="END_DATE__RISKBASE" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9"><div class="input-group"><asp:TextBox ID="END_DATE__RISKBASE" runat="server" CssClass="field form-control"></asp:TextBox><uc1:CalendarLookup ID="calEndDate" runat="server" LinkedControl="END_DATE__RISKBASE" HLevel="2"></uc1:CalendarLookup></div></div>
                            
                            <asp:RequiredFieldValidator ID="vldCoverEndDate" Display="None" ControlToValidate="END_DATE__RISKBASE" runat="server" ErrorMessage="<%$ Resources:lbl_EnterEndDate %>" SetFocusOnError="True" ValidationGroup=""></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rangevldEndDate" runat="server" Display="none" ErrorMessage="<%$ Resources:lbl_InvalidEndDateFormat %>" ControlToValidate="END_DATE__RISKBASE" SetFocusOnError="True" ValidationGroup=""></asp:RangeValidator>
                        </div>
                    </div>
            </div>
            
        </div>
    </asp:Panel>
</div>
