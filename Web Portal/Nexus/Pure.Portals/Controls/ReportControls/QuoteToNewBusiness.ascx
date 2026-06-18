<%@ Control Language="VB" AutoEventWireup="false" CodeFile="QuoteToNewBusiness.ascx.vb"
    Inherits="Nexus.Controls_ReportControls_QuoteToNewBusiness" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>

<script language="javascript" type="text/javascript">
    /*
        function CompareDates(source, args) {
            alert("comparing");
             var str1 = document.getElementById('<%=RP__START_DATE.ClientID%>').value;
    var str2 = document.getElementById('<%=RP__END_DATE.ClientID%>').value;
    var dt1  = parseInt(str1.substring(0,2),10);
    var mon1 = parseInt(str1.substring(3,5),10);
    var yr1  = parseInt(str1.substring(6,10),10);
    var dt2  = parseInt(str2.substring(0,2),10);
    var mon2 = parseInt(str2.substring(3,5),10);
    var yr2  = parseInt(str2.substring(6,10),10);
    var date1 = new Date(yr1, mon1, dt1);
    var date2 = new Date(yr2, mon2, dt2);
    alert(date1);
    alert(date2);
            
    if(date2 < date1)
    {
        //alert("To date cannot be greater than from date");
        args.IsValid = false;                
    }
    else
    {
        //alert("Submitting ...");
        args.IsValid = true;                
    }
    }

 */
</script>

<div id="Controls_ReportControls_QuotesToNewBusiness">
    <div class="card">
        <div class="card-body clearfix">
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:lbl_header %>"></asp:Label></legend>
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblStartDate" runat="server" AssociatedControlID="RP__START_DATE" Text="<%$ Resources:lbl_StartDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__START_DATE" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calStartDate" runat="server" LinkedControl="RP__START_DATE" HLevel="1"></uc1:CalendarLookup>
                        </div>
                    </div>

                    <asp:RequiredFieldValidator ID="reqdvldStartDate" Display="None" ControlToValidate="RP__START_DATE" runat="server" ErrorMessage="<%$ Resources:lbl_req_StartDate %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"> </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="comvldStartDate" runat="server" Display="None" ControlToValidate="RP__START_DATE" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_StartDate %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
                    <asp:RangeValidator ID="rngvldStartDate" runat="server" ErrorMessage="<%$ Resources:lbl_invalidrange_StartDate %>" ControlToValidate="RP__START_DATE" Display="None" Enabled="true" ValidationGroup="vldReportsControlsGroup" Type="Date">
                    </asp:RangeValidator>
                </div>

                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblEndDate" runat="server" AssociatedControlID="RP__END_DATE" Text="<%$ Resources:lbl_EndDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <div class="input-group">
                            <asp:TextBox ID="RP__END_DATE" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calEndDate" runat="server" LinkedControl="RP__END_DATE" HLevel="1"></uc1:CalendarLookup>
                        </div>
                    </div>

                    <asp:RequiredFieldValidator ID="reqdvldEndDate" Display="None" ControlToValidate="RP__END_DATE" runat="server" ErrorMessage="<%$ Resources:lbl_req_EndDate %>" SetFocusOnError="True" ValidationGroup="vldReportsControlsGroup"> </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="comvldEndDate" runat="server" Display="None" ControlToValidate="RP__END_DATE" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalid_EndDate %>" Operator="DataTypeCheck" Type="Date" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
                    <asp:RangeValidator ID="rngvldEndDate" runat="server" ErrorMessage="<%$ Resources:lbl_invalidrange_EndDate %>" ControlToValidate="RP__END_DATE" Display="None" ValidationGroup="vldReportsControlsGroup" Type="Date">
                    </asp:RangeValidator>
                    <asp:CompareValidator ID="cmpvldStartEndDates" ForeColor="Red" runat="server" ControlToValidate="RP__START_DATE" ControlToCompare="RP__END_DATE" Operator="LessThanEqual" Type="Date" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_invalidGreater_StartDate %>" Display="None" ValidationGroup="vldReportsControlsGroup"></asp:CompareValidator>
                </div>

                <!-- Type of Currency -->
                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lblTypeOfCurrency" runat="server" AssociatedControlID="RP__TypeOfCurrency" Text="<%$ Resources:lbl_TypeOfCurrency %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__TypeOfCurrency" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem>System</asp:ListItem>
                            <asp:ListItem>Base</asp:ListItem>
                            <asp:ListItem>Account</asp:ListItem>

                        </asp:DropDownList>
                    </div>
                </div>

                <!-- Group By -->
                <div id="liGroupBy" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                    <asp:Label ID="lbl_GroupBy" runat="server" AssociatedControlID="RP__GroupbyCode" Text="<%$ Resources:lbl_GroupBy %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                    <div class="col-md-8 col-sm-9">
                        <asp:DropDownList ID="RP__GroupbyCode" runat="server" CssClass="field-medium form-control form-select">
                            <asp:ListItem>No Grouping</asp:ListItem>
                            <asp:ListItem>Branch</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <asp:HiddenField ID="RP__agent_code" runat="server"></asp:HiddenField>
            </div>
        </div>
        <div class="card-footer">
            <asp:LinkButton ID="btnGenerateReport" runat="server" Text="<%$ Resources:btnGenerateReport %>" OnClick="GenerateReport" ValidationGroup="vldReportsControlsGroup" SkinID="btnPrimary"></asp:LinkButton>
        </div>
    </div>

</div>
