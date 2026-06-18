<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BOCoverDates.ascx.vb"
    Inherits="Nexus.Controls_BOCoverDates" EnableViewState="false" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>

<script type="text/javascript" language="javascript">
    function ChangeEndDate(iPeriod,oControlStartdate,oControlEndDate,MidnightRenewal,iControl)
    {
        //if iControl = 0 then for Renewal date if iControl =1 then Cover To date
        dCoverStartDate=oControlStartdate.value;
        var arStartDate = dCoverStartDate.split('/');
        var dtTempDate = new Date(arStartDate[2],arStartDate[1]-1, arStartDate[0]); 
        if (StartDateClientValidate(dCoverStartDate)==false)
        {
            oControlStartdate.value='';
            oControlEndDate.value='';
            return false;
        }
        if(iControl==1)
        {
          iPeriod=12;
          dtTempDate.setMonth(dtTempDate.getMonth()+parseInt(iPeriod));
        }
        
        if(MidnightRenewal == 1)
        {
            
            if(iControl==1)
            {
                dtTempDate.setDate(dtTempDate.getDate()-1);
            }
            else
            {
                dtTempDate.setDate(dtTempDate.getDate()+1);
            }
        }
        else if(MidnightRenewal == 0)
        {
            dtTempDate.setDate(dtTempDate.getDate());
        }
       if (dtTempDate.getMonth()>8)
        {
        //dCoverEndDate=(dtTempDate.getMonth()+1)+'/'+dtTempDate.getDate()+'/'+dtTempDate.getYear()
        dCoverEndDate= dtTempDate.getDate()+'/'+(dtTempDate.getMonth()+1)+'/'+dtTempDate.getYear() // DD/MM/YYYY Format.
        }
        else
        {
        dCoverEndDate= dtTempDate.getDate()+'/'+'0'+(dtTempDate.getMonth()+1)+'/'+dtTempDate.getYear() // DD/MM/YYYY Format.
        }
        oControlEndDate.value=dCoverEndDate;
    }

    function StartDateClientValidate(valueDate)
    {
       if (valueDate=='')//If Start Date is blank 
       {
            return false;
       }
       else
       {
             var result, regEx;//Declare variables.
             //regEx = new RegExp(/^((0?[13578]|10|12)(-|\/)(([1-9])|(0[1-9])|([12])([0-9]?)|(3[01]?))(-|\/)((19)([2-9])(\d{1})|(20)([01])(\d{1})|([8901])(\d{1}))|(0?[2469]|11)(-|\/)(([1-9])|(0[1-9])|([12])([0-9]?)|(3[0]?))(-|\/)((19)([2-9])(\d{1})|(20)([01])(\d{1})|([8901])(\d{1})))$/);  //Create regular expression object (mm/dd/yyyy)
             regEx = new RegExp(/^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$/);  //Create regular expression object.(DD/MM/YYYY)
             result = regEx.test(valueDate);  //Test for match.
             return result;
       }
    }

    function fillDate()
    {
        var CoverFromDate=$get('<%=txtCoverStartDate.ClientID%>');
        var CoverFromDateValue=CoverFromDate.value;
        var GracePeriod=$get('<%=hiddenGracePeriod.ClientID%>');
        var OptionSetting=$get('<%=hiddenOptionSetting.ClientID%>');
        var MidnightRenewalSettings=$get('<%=hiddenMidnightRenewalSettings.ClientID%>');
        var CoverToDate=$get('<%=txtCoverEndDate.ClientID%>');
        var Inception=$get('<%=txtInception.ClientID%>');
        var Renewal=$get('<%=txtRenewal.ClientID%>');
        var InceptionTPI=$get('<%=txtInceptionTPI.ClientID%>');
        var ProposalDate=$get('<%=txtProposalDate.ClientID%>');
        var QuoteExpiryDate=$get('<%=txtQuoteExpiryDate.ClientID%>');
        
        //For Hidden Fields
        var HiddenInception=$get('<%=hiddenInceptionDate.ClientID%>');
        var HiddenRenewal=$get('<%=hiddenRenewalDate.ClientID%>');
        var HiddenInceptionTPI=$get('<%=hiddenInceptionTPIDate.ClientID%>');
        var HiddenProposalDate=$get('<%=hiddenProposalDate.ClientID%>');
        var HiddenQuoteExpiryDate=$get('<%=hiddenQuoteExpiryDate.ClientID%>');
        
        if(MidnightRenewalSettings.value==1)
        {
            ChangeEndDate(OptionSetting.value,CoverFromDate,CoverToDate,MidnightRenewalSettings.value,1);
            ChangeEndDate(12,CoverToDate,Renewal,MidnightRenewalSettings.value,0);
            //For Hidden Fields
            ChangeEndDate(12,CoverToDate,HiddenRenewal,MidnightRenewalSettings.value,0);             
        }
        else
        {
           ChangeEndDate(OptionSetting.value,CoverFromDate,CoverToDate,MidnightRenewalSettings.value,1);
           ChangeEndDate(12,CoverToDate,Renewal,MidnightRenewalSettings.value,0);
           //For Hidden Fields
           ChangeEndDate(12,CoverToDate,HiddenRenewal,MidnightRenewalSettings.value,0);               
        }
        
        Inception.value=CoverFromDate.value;
        InceptionTPI.value=CoverFromDate.value;
        ProposalDate.value=CoverFromDate.value;
        //For Hidden Fields
        HiddenInception.value=CoverFromDate.value;
        HiddenInceptionTPI.value=CoverFromDate.value;
        HiddenProposalDate.value=CoverFromDate.value;
        //ChangeEndDate(GracePeriod.value,CoverFromDate,QuoteExpiryDate,MidnightRenewalSettings.value,1);
        //alert(CoverFromDate.value)
    }
    function fillDateRenewal()
    {
        var GracePeriod=$get('<%=hiddenGracePeriod.ClientID%>');
        var OptionSetting=$get('<%=hiddenOptionSetting.ClientID%>');
        var MidnightRenewalSettings=$get('<%=hiddenMidnightRenewalSettings.ClientID%>');
        var CoverToDate=$get('<%=txtCoverEndDate.ClientID%>');
        var Renewal=$get('<%=txtRenewal.ClientID%>');
        //For Hidden Fields
        var HiddenRenewal=$get('<%=hiddenRenewalDate.ClientID%>');
        
        if(MidnightRenewalSettings.value==1)
        {
            ChangeEndDate(12,CoverToDate,Renewal,MidnightRenewalSettings.value,0);
            //For Hidden Fields             
            ChangeEndDate(12,CoverToDate,HiddenRenewal,MidnightRenewalSettings.value,0);
        }
        else
        {
            ChangeEndDate(12,CoverToDate,Renewal,MidnightRenewalSettings.value,0);
            //For Hidden Fields
            ChangeEndDate(12,CoverToDate,HiddenRenewal,MidnightRenewalSettings.value,0);
        }
    }
</script>

<div id="Controls_BOCoverDates">
    <div class="card">
        <div class="card-body clearfix">
            
            <div class="form-horizontal">
                <legend>
                    <asp:Label ID="lblHeading1" runat="server" Text="<%$ Resources:lbl_Heading1 %>"></asp:Label></legend>
                
            
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCoverStartDate" runat="server" AssociatedControlID="txtCoverStartDate" Text="<%$ Resources:lbl_CoverStartDate%>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9"><div class="input-group"><asp:TextBox ID="txtCoverStartDate" runat="server" CssClass="field-mandatory form-control" onBlur="fillDate()"></asp:TextBox><uc1:CalendarLookup ID="calCoverFromDate" runat="server" LinkedControl="txtCoverStartDate" HLevel="1"></uc1:CalendarLookup></div></div>
                        
                        <asp:RequiredFieldValidator ID="vldrqdCoverFromDate" runat="server" Display="none" ErrorMessage="<%$ Resources:lbl_RqdErrMsgInvalidCoverStartDate %>" ControlToValidate="txtCoverStartDate" Enabled="true" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        <%--<asp:RangeValidator ID="rangevldFromtDate" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_RanErrMsgInvalidCoverStartDate %>"
                            ControlToValidate="txtCoverStartDate" SetFocusOnError="True" Enabled="true" Type="Date" />--%>
                        <asp:CustomValidator ID="custFromDate" runat="server" Display="none" ControlToValidate="txtCoverStartDate" SetFocusOnError="true"></asp:CustomValidator>
                        <asp:RegularExpressionValidator ID="regexvldFromDate" runat="server" ControlToValidate="txtCoverStartDate" Display="None" ErrorMessage="<%$ Resources:lbl_RegExpInvalidCoverStartDateFormat %>" ValidationExpression="(0[1-9]|1[012])[- /\\.](0[1-9]|[12][0-9]|3[01])[- /\\.](19|20)\d\d" SetFocusOnError="true" Enabled="False"></asp:RegularExpressionValidator>
                        <asp:HiddenField ID="hiddenGracePeriod" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hiddenOptionSetting" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hiddenCoverFromDate" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hiddenMidnightRenewalSettings" runat="server"></asp:HiddenField>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbl_CoverEndDate" runat="server" AssociatedControlID="txtCoverEndDate" Text="<%$ Resources:lbl_CoverEndDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtCoverEndDate" runat="server" CssClass="field-date form-control" onblur="fillDateRenewal()"></asp:TextBox></div><uc1:CalendarLookup ID="calCoverEndDate" runat="server" LinkedControl="txtCoverEndDate" HLevel="1"></uc1:CalendarLookup>
                        <asp:RequiredFieldValidator ID="vldrqdCoverEndDate" runat="server" Display="none" ErrorMessage="<%$ Resources:lbl_RqdErrMsgInvalidEndDate %>" ControlToValidate="txtCoverEndDate" Enabled="true" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        <%--<asp:RangeValidator ID="rangevldEndDate" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_RanErrMsgInvalidEndDate %>"
                            ControlToValidate="txtCoverEndDate" SetFocusOnError="True" Enabled="true" Type="Date"/>--%>
                        <asp:RegularExpressionValidator ID="regexvldEndDate" runat="server" ControlToValidate="txtCoverEndDate" Display="None" ErrorMessage="<%$ Resources:lbl_RegExpInvalidEndDateFormat %>" ValidationExpression="(0[1-9]|1[012])[- /\\.](0[1-9]|[12][0-9]|3[01])[- /\\.](19|20)\d\d" SetFocusOnError="true" Enabled="False"></asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="custToDate" runat="server" Display="none" ControlToValidate="txtCoverEndDate" SetFocusOnError="true"></asp:CustomValidator>
                        <asp:CompareValidator ID="compvldCoverEndDate" runat="server" ControlToCompare="txtCoverStartDate" Display="None" ErrorMessage="<%$ Resources:lbl_RegExpInvalidEndDateFormat %>" ControlToValidate="txtCoverEndDate" SetFocusOnError="true" Operator="GreaterThan" Type="Date" Enabled="True"></asp:CompareValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblInception" runat="server" AssociatedControlID="txtInception" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litInception" runat="server" Text="<%$ Resources:lbl_Inception%>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtInception" runat="server" CssClass="field-date form-control" Enabled="false"></asp:TextBox></div>
                        <asp:HiddenField ID="hiddenInceptionDate" runat="server"></asp:HiddenField>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRenewal" runat="server" AssociatedControlID="txtRenewal" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litRenewal" runat="server" Text="<%$ Resources:lbl_Renewal%>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtRenewal" runat="server" CssClass="field-date form-control" Enabled="false"></asp:TextBox></div>
                        <asp:HiddenField ID="hiddenRenewalDate" runat="server"></asp:HiddenField>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblInceptionTPI" runat="server" AssociatedControlID="txtInceptionTPI" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litInceptionTPI" runat="server" Text="<%$ Resources:lbl_InceptionTPI%>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtInceptionTPI" runat="server" CssClass="field-date form-control" Enabled="false"></asp:TextBox></div>
                        <asp:HiddenField ID="hiddenInceptionTPIDate" runat="server"></asp:HiddenField>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblProposalDate" runat="server" AssociatedControlID="txtProposalDate" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="lit" runat="server" Text="<%$ Resources:lbl_ProposalDate %>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtProposalDate" runat="server" CssClass="field-date form-control" Enabled="false"></asp:TextBox></div>
                        <asp:HiddenField ID="hiddenProposalDate" runat="server"></asp:HiddenField>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblQuoteExpiryDate" runat="server" AssociatedControlID="txtQuoteExpiryDate" class="col-md-4 col-sm-3 control-label">
                            <asp:Literal ID="litQuoteExpiryDate" runat="server" Text="<%$ Resources:lbl_QuoteExpiryDate %>"></asp:Literal></asp:Label>
                        <div class="col-md-8 col-sm-9"><asp:TextBox ID="txtQuoteExpiryDate" runat="server" CssClass="field-date form-control" Enabled="false"></asp:TextBox></div>
                        <asp:HiddenField ID="hiddenQuoteExpiryDate" runat="server"></asp:HiddenField>
                    </div>
                </div>
        </div>
        
    </div>
</div>
