<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/Default.master"
    CodeFile="MainDetails.aspx.vb" Inherits="Nexus.secure_MainDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="uc3" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/BOCoverDates.ascx" TagName="CoverDate" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/SubAgents.ascx" TagName="SubAgents" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/PolicyHeader.ascx" TagName="PolicyHeader" TagPrefix="uc5" %>
<%@ Register Src="~/Controls/PolicyAssociates.ascx" TagName="PolicyAssociates" TagPrefix="ucPA" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        //Mark AgentCode and Handler textboxes to be readonly using jquery

        var IsUnifiedRenewalDateChanged = 0;

        $(document).ready(function () {
            DisableTextboxForFindControls();
            if ($('#<%= hfNeedToUpdateOnStartChange.ClientID%>').val() == "True") {
                SetTransactionType();
            }
            if ($('#<%=hdnIsBroker.ClientID %>').val() == "1") {
                $('#<%=POLICYHEADER__BUSINESSTYPE.ClientID %>').attr('disabled', true);
                $('#<%= POLICYHEADER__AGENTCODE.ClientID%>').attr('readonly', true);
                $('#<%= btnAgentCode.ClientID%>').attr('disabled', true);
            }
        });

        function DisableTextboxForFindControls() {
            $('#<%= POLICYHEADER__AGENTCODE.ClientId%>').attr("readonly", true);
            $('#<%= POLICYHEADER__HANDLER.ClientId%>').attr("readonly", true);
        }

        function setAgent(sName, sKey, sCode, sAgentType) {
            tb_remove();
            document.getElementById('<%= POLICYHEADER__AGENTCODE.ClientId%>').value = unescape(sCode);
            document.getElementById('<%= POLICYHEADER__AGENT.ClientId%>').value = sKey;
            document.getElementById('<%= hAgentType.ClientId%>').value = sAgentType;
            //Need to clean all prevoius validation to get the page refreshed
            if (Page_IsValid == false) {
                Page_ClientValidateReset();
                    // Debit order fields validation makes teh page validation fail due to which analysis_code field is disabled
               // ValidatorEnable(document.getElementById('<%= rfvCollectionFrequency.ClientID %>'), false);
               // ValidatorEnable(document.getElementById('<%= rfvPaymentTerm.ClientID %>'), false);
                Page_ClientValidate();
                //ValidatorEnable(document.getElementById('<%= rfvCollectionFrequency.ClientID %>'), true);
                //ValidatorEnable(document.getElementById('<%= rfvPaymentTerm.ClientID %>'), true);
            }
            __doPostBack('Agent', 'RefreshAgent');
        }

        function Page_ClientValidateReset() {
            if (typeof (Page_Validators) != "undefined") {
                for (var i = 0; i < Page_Validators.length; i++) {
                    var validator = Page_Validators[i];
                    validator.isvalid = true;
                    ValidatorUpdateDisplay(validator);
                }
            }
        }
        	 function  policyBusinessType()
             {
                  __doPostBack('Agent', 'RefreshBusinessType');
             }
        function setAccountHandler(sName, sKey, sCode) {
            tb_remove();
            document.getElementById('<%= POLICYHEADER__HANDLER.ClientId%>').value = unescape(sCode);
             if (sKey != document.getElementById('<%= hfHandlerCode.ClientId%>').value) {
                 TrackChanges();
             }
             document.getElementById('<%= hfHandlerCode.ClientId%>').value = sKey;
             document.getElementById('<%= POLICYHEADER__HANDLER.ClientId%>').focus;
         }

         function ChangeEndDate(iPeriod, oControlStartdate, oControlEndDate, MidnightRenewal, iControl) {
             var dCoverStartDate = oControlStartdate.value;
             var oControlAnniversaryDate = $get('<%=POLICYHEADER__ANNIVERSARY.ClientID%>');
             var RenewalFrequency = document.getElementById('<%= hfRenewalFrequency.ClientID%>').value;
            if (StartDateClientValidate(dCoverStartDate) == false) {
                oControlStartdate.value = '';
                oControlEndDate.value = '';
                oControlAnniversaryDate.value = '';
                return false;
            }
            var IsTrueMonthlyProduct = $get('<%=hfIsTrueMonthlyProduct.ClientID%>');
            var IsUnifiedRenewalDayReadOnly = $get('<%=hfIsUnifiedRenewalDayReadOnly.ClientID%>');
            if (IsTrueMonthlyProduct.value == 1) {
                // Retrieve selected index of Dropdown List
                var IndexValue = $get('<%=POLICYHEADER__UNIFIEDRENEWALDAY.ClientID %>').selectedIndex;
                // Retrieve selected value of Dropdown List
                var UnifiedRenewalDay = $get('<%=POLICYHEADER__UNIFIEDRENEWALDAY.ClientID %>').options[IndexValue].value;
                //Cover Start Date
                dCoverStartDate = $get('<%=POLICYHEADER__COVERSTARTDATE.ClientID%>').value;
                var arStartDate = dCoverStartDate.split('/');
                var oControlRenewalDate = $get('<%=POLICYHEADER__RENEWAL.ClientID%>');
                var CoverToDate = $get('<%=POLICYHEADER__COVERENDDATE.ClientID%>');

                if (iControl == 0) {
                    if (MidnightRenewal == 1) {
                        var arRenewalDate = CoverToDate.value.split('/');
                        var dtTempDate = new Date(arRenewalDate[2], arRenewalDate[1] - 1, arRenewalDate[0]);
                        var dtRenewalDate = new Date(dtTempDate);
                        dtRenewalDate.setDate(dtRenewalDate.getDate() + 1);
                        if ((dtRenewalDate.getMonth() + 1) > 12) {
                            oControlRenewalDate.value = formatDate(dtRenewalDate.getDate() + '/' + 01 + '/' + (dtRenewalDate.getFullYear() + 1));
                        }
                        else {
                            oControlRenewalDate.value = formatDate(dtRenewalDate.getDate() + '/' + (dtRenewalDate.getMonth() + 1) + '/' + dtRenewalDate.getFullYear());
                        }
                    }
                    else {
                        //Set Renewal Date = Cover To Date
                        oControlRenewalDate.value = CoverToDate.value;
                    }
                    var arRenewalDate = oControlRenewalDate.value.split('/');
                    var UnifiedRenewalDay = arRenewalDate[0];

                    $('#<%=POLICYHEADER__UNIFIEDRENEWALDAY.ClientID %>').val(parseInt(UnifiedRenewalDay, 10));
                }

                if (iControl == 1) {

                if (parseInt(UnifiedRenewalDay, 10) > parseInt(arStartDate[0], 10)) {
                    //Set Renewal Date= Selected Value from UNIFIED RENEWAL DAY  +"/" + Month of CoverStartDate + 1 + "/" + Year of Cover Start Date
                    oControlRenewalDate.value = formatDate(UnifiedRenewalDay + '/' + arStartDate[1] + '/' + arStartDate[2]);
                }
                else {
                    //Set Renewal Date= Selected Value from UNIFIED RENEWAL DAY + "/" + Month of CoverStartDate  + "/" + Year of Cover Start Date 
                    if ((parseInt(arStartDate[1], 10) + 1) > 12) {
                        oControlRenewalDate.value = formatDate(UnifiedRenewalDay + '/' + 01 + '/' + (parseInt(arStartDate[2], 10) + 1));
                    }
                    else {
                        oControlRenewalDate.value = formatDate(UnifiedRenewalDay + '/' + (parseInt(arStartDate[1], 10) + 1) + '/' + arStartDate[2]);
                    }
                }

                    if (MidnightRenewal == 1) {
                        var arRenewalDate = oControlRenewalDate.value.split('/');
                        var dtTempDate = new Date(arRenewalDate[2], arRenewalDate[1] - 1, arRenewalDate[0]);
                        //Set Cover To Date= Renewal Date - 1
                        var dtCoverEndDate = new Date(dtTempDate);
                        dtCoverEndDate.setDate(dtCoverEndDate.getDate() - 1);
                        if ((dtCoverEndDate.getMonth() + 1) > 12) {
                            CoverToDate.value = formatDate(dtCoverEndDate.getDate() + '/' + 01 + '/' + (dtCoverEndDate.getFullYear() + 1));
                        }
                        else {
                            CoverToDate.value = formatDate(dtCoverEndDate.getDate() + '/' + (dtCoverEndDate.getMonth() + 1) + '/' + dtCoverEndDate.getFullYear());
                        }
                    }
                    else {
                        //Set Cover To Date= Renewal Date
                        CoverToDate.value = oControlRenewalDate.value;
                    }
                }
                if (iControl == 2) {
                    if (MidnightRenewal == 1) {
                        var arRenewalDate = oControlRenewalDate.value.split('/');
                        var dtTempDate = new Date(arRenewalDate[2], arRenewalDate[1] - 1, arRenewalDate[0]);
                        //Set Cover To Date= Renewal Date - 1
                        var dtCoverEndDate = new Date(dtTempDate);
                        dtCoverEndDate.setDate(dtCoverEndDate.getDate() - 1);
                        if ((dtCoverEndDate.getMonth() + 1) > 12) {
                            CoverToDate.value = formatDate(dtCoverEndDate.getDate() + '/' + 01 + '/' + (dtCoverEndDate.getFullYear() + 1));
                        }
                        else {
                            CoverToDate.value = formatDate(dtCoverEndDate.getDate() + '/' + (dtCoverEndDate.getMonth() + 1) + '/' + dtCoverEndDate.getFullYear());
                        }
                    }
                    else {
                        //Set Cover To Date= Renewal Date
                        CoverToDate.value = oControlRenewalDate.value;
                    }
                    var arRenewalDate = oControlRenewalDate.value.split('/');
                    var UnifiedRenewalDay = arRenewalDate[0];

                    $('#<%=POLICYHEADER__UNIFIEDRENEWALDAY.ClientID %>').val(parseInt(UnifiedRenewalDay, 10));
                }
                FillAnniversaryDate();
            }
            else {
                var sDefaultCoverToDateToLastDay = $get('<%=hfDefaultCoverToDateToLastDay.ClientID%>')
                dCoverStartDate = oControlStartdate.value;
                var arStartDate = dCoverStartDate.split('/');
                var dDateYearLength = arStartDate[2].length // 2 Digits or 4 Digits
                var dtTempDate = new Date(arStartDate[2], arStartDate[1] - 1, arStartDate[0]);
                                if (StartDateClientValidate(dCoverStartDate) == false) {
                    oControlStartdate.value = '';
                    oControlEndDate.value = '';
                    return false;
                }
                if (sDefaultCoverToDateToLastDay.value == "1") {
                    if (iControl == 1) {
                        iPeriod = RenewalFrequency;
                        dtTempDate.setMonth(dtTempDate.getMonth() + parseInt(iPeriod, 10));
                        if (dtTempDate.getMonth() == 0) {
                            if (dDateYearLength == 2) {
                                dCoverEndDate = daysInMonth(dtTempDate.getMonth(), dtTempDate.getFullYear()) + '/12/' + (dtTempDate.getFullYear() - 1).toString().substring(2, dDateYearLength + 2);
                            }
                            else
                            dCoverEndDate = daysInMonth(dtTempDate.getMonth(), dtTempDate.getFullYear()) + '/12/' + (dtTempDate.getFullYear() - 1);
                        }
                        else {
                            if (dDateYearLength == 2) {
                                dCoverEndDate = daysInMonth(dtTempDate.getMonth(), dtTempDate.getFullYear()) + '/' + dtTempDate.getMonth() + '/' + dtTempDate.getFullYear().toString().substring(2, dDateYearLength + 2);
                            }
                            else
                            dCoverEndDate = daysInMonth(dtTempDate.getMonth(), dtTempDate.getFullYear()) + '/' + dtTempDate.getMonth() + '/' + dtTempDate.getFullYear();
                        }
                    }
                    else //renewal date
                    {
                        if (MidnightRenewal == 1) {
                            if (iControl == 1) {
                                dtTempDate.setDate(dtTempDate.getDate() - 1);
                            }
                            else {
                                dtTempDate.setDate(dtTempDate.getDate() + 1);
                            }
                        }
                        else if (MidnightRenewal == 0) {
                            dtTempDate.setDate(dtTempDate.getDate());
                        }
                        if (dtTempDate.getMonth() > 8) {
                            if (dDateYearLength == 2) {
                                dCoverEndDate = dtTempDate.getDate() + '/' + (dtTempDate.getMonth() + 1) + '/' + dtTempDate.getFullYear().toString().substring(2, dDateYearLength + 2) // DD/MM/YYYY Format.
                            }
                            else
                            dCoverEndDate = dtTempDate.getDate() + '/' + (dtTempDate.getMonth() + 1) + '/' + dtTempDate.getFullYear() // DD/MM/YYYY Format.
                        
                        }
                        else {
                            if (dDateYearLength == 2) {
                                dCoverEndDate = dtTempDate.getDate() + '/' + '0' + (dtTempDate.getMonth() + 1) + '/' + dtTempDate.getFullYear().toString().substring(2, dDateYearLength + 2) // DD/MM/YYYY Format.
                            }
                            else
                            dCoverEndDate = dtTempDate.getDate() + '/' + '0' + (dtTempDate.getMonth() + 1) + '/' + dtTempDate.getFullYear() // DD/MM/YYYY Format.
                        }
                    }
                }
                else {
                    if (iControl == 1) {
                        iPeriod = RenewalFrequency;
                        dtTempDate.setMonth(dtTempDate.getMonth() + parseInt(iPeriod, 10));
                    }
                    if (MidnightRenewal == 1) {
                        if (iControl == 1) {
                            dtTempDate.setDate(dtTempDate.getDate() - 1);
                        }
                        else {
                            dtTempDate.setDate(dtTempDate.getDate() + 1);
                        }
                    }
                    else if (MidnightRenewal == 0) {
                        dtTempDate.setDate(dtTempDate.getDate());
                    }
                    if (dtTempDate.getMonth() > 8) {
                        if (dDateYearLength == 2) {
                            dCoverEndDate = dtTempDate.getDate() + '/' + (dtTempDate.getMonth() + 1) + '/' + dtTempDate.getFullYear().toString().substring(2, dDateYearLength + 2) // DD/MM/YYYY Format.
                        }
                        else
                        dCoverEndDate = dtTempDate.getDate() + '/' + (dtTempDate.getMonth() + 1) + '/' + dtTempDate.getFullYear() // DD/MM/YYYY Format.
                    }
                    else {
                        if (dDateYearLength == 2) {
                            dCoverEndDate = dtTempDate.getDate() + '/' + '0' + (dtTempDate.getMonth() + 1) + '/' + dtTempDate.getFullYear().toString().substring(2, dDateYearLength + 2) // DD/MM/YYYY Format.
                        }
                        else
                        dCoverEndDate = dtTempDate.getDate() + '/' + '0' + (dtTempDate.getMonth() + 1) + '/' + dtTempDate.getFullYear() // DD/MM/YYYY Format.
                    }
                }
                oControlEndDate.value = formatDate(dCoverEndDate);
            }
        }

        function daysInMonth(month, year) {
            return new Date(year, month, 0).getDate();
        }

        function StartDateClientValidate(valueDate) {
            if (valueDate == '')//If Start Date is blank
            {
                return false;
            }
            else {
                var result, regEx;//Declare variables.
                regEx = new RegExp(/^(?=\d)(?:(?:31(?!.(?:0?[2469]|11))|(?:30|29)(?!.0?2)|29(?=.0?2.(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00)))(?:\x20|$))|(?:2[0-8]|1\d|0?[1-9]))([-./])(?:1[012]|0?[1-9])\1(?:1[6-9]|[2-9]\d)?\d\d(?:(?=\x20\d)\x20|$))?(((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$/);  //Create regular expression object.(DD/MM/YYYY)
                result = regEx.test(valueDate);  //Test for match.
                return result;
            }
        }

        function fillDate(iMode) {
            
            var CoverFromDate = $get('<%=POLICYHEADER__COVERSTARTDATE.ClientID%>');
            var CoverFromDateValue = CoverFromDate.value;
            var GracePeriod = $get('<%=hfGracePeriod.ClientID%>');
            var MonthInFutureAllowedForCoverToDate = $get('<%=hfMonthInFutureAllowedForCoverToDate.ClientID%>');
            var MidnightRenewalSettings = $get('<%=hfMidnightRenewalSettings.ClientID%>');
            var CoverToDate = $get('<%=POLICYHEADER__COVERENDDATE.ClientID%>');
            var Inception = $get('<%=POLICYHEADER__INCEPTION.ClientID%>');
            var Renewal = $get('<%=POLICYHEADER__RENEWAL.ClientID%>');
            var InceptionTPI = $get('<%=POLICYHEADER__INCEPTIONTPI.ClientID%>');
            var ProposalDate = $get('<%=POLICYHEADER__PROPOSALDATE.ClientID%>');
            var QuoteExpiryDate = $get('<%=POLICYHEADER__QUOTEEXPIRYDATE.ClientID%>');

            if ($('#<%= hfNeedToUpdateOnStartChange.ClientID%>').val() == "True") {
                ChangeEndDate(MonthInFutureAllowedForCoverToDate.value, CoverFromDate, CoverToDate, MidnightRenewalSettings.value, 1);
                ChangeEndDate(12, CoverToDate, Renewal, MidnightRenewalSettings.value, 0);
            }
            
            if ('<%=iMode%>' == 0) {
                Inception.value = CoverFromDate.value;
            }
            InceptionTPI.value = CoverFromDate.value;

            ProposalDate.value = CoverFromDate.value;

            var arStartDate = CoverFromDateValue.split('/');
            var optVal = $("#<%= POLICYHEADER__UNDERWRITINGYEAR.ClientID %> option:contains('" + arStartDate[2] + "')").attr('value');
            $('#<%= POLICYHEADER__UNDERWRITINGYEAR.ClientID%>').val(optVal);
        }

        function fillDateRenewal(iControl) {
            IsUnifiedRenewalDateChanged = iControl;
            var IsTrueMonthlyProduct = $get('<%=hfIsTrueMonthlyProduct.ClientID%>');
            var sDefaultCoverToDateToLastDay = $get('<%=hfDefaultCoverToDateToLastDay.ClientID%>')
            var GracePeriod = $get('<%=hfGracePeriod.ClientID%>');
            var OptionSetting = $get('<%=hfMonthInFutureAllowedForCoverToDate.ClientID%>');
            var MidnightRenewalSettings = $get('<%=hfMidnightRenewalSettings.ClientID%>');
            var CoverToDate = $get('<%=POLICYHEADER__COVERENDDATE.ClientID%>');
            var Renewal = $get('<%=POLICYHEADER__RENEWAL.ClientID%>');

            if (IsTrueMonthlyProduct.value == 1) {
                ChangeEndDate(1, CoverToDate, Renewal, MidnightRenewalSettings.value, iControl);
            }
            else {
                if (MidnightRenewalSettings.value == 1) {
                    ChangeEndDate(12, CoverToDate, Renewal, MidnightRenewalSettings.value, iControl);
                }
                else {
                    ChangeEndDate(12, CoverToDate, Renewal, MidnightRenewalSettings.value, iControl);
                }
            }
            if (iControl == 0) {
                CheckCoverToDateIsValid();
        }
        }

        function ValidateAnniversaryDate(source, arguments) {
            // Retrieve hidden field hfIsTrueMonthlyProduct and IsUnifiedRenewalDayReadOnly
            var IsTrueMonthlyProduct = $get('<%=hfIsTrueMonthlyProduct.ClientID%>');
            // Retrieve Anniversary Date Control
            var oControlAnniversaryDate = $get('<%=POLICYHEADER__ANNIVERSARY.ClientID%>');
            var dAnniversaryDate = oControlAnniversaryDate.value;
            if (StartDateClientValidate(dAnniversaryDate) == false) {
                oControlAnniversaryDate.value = '';
                return false;
            }
            if (IsTrueMonthlyProduct.value == 1) {
                // Retrieve selected index of Dropdown List
                var IndexValue = $get('<%=POLICYHEADER__UNIFIEDRENEWALDAY.ClientID %>').selectedIndex;
                // Retrieve selected value of Dropdown List
                var UnifiedRenewalDay = $get('<%=POLICYHEADER__UNIFIEDRENEWALDAY.ClientID %>').options[IndexValue].value;
                // Retrieve Renewal Date Control        
                var oControlRenewalDate = $get('<%=POLICYHEADER__RENEWAL.ClientID%>');
                var dAnniversaryDate = oControlAnniversaryDate.value;
                var dRenewalDate = oControlRenewalDate.value;
                var arAnniversaryDate = dAnniversaryDate.split('/');
                 //JavaScript counts months from 0 to 11. so subtracting 1 from Month.
                var dtTempDate = new Date(arAnniversaryDate[2], arAnniversaryDate[1]-1, arAnniversaryDate[0]);
                var AnniversaryDay = dtTempDate.getDate();

                if (AnniversaryDay != UnifiedRenewalDay) {
                    if ('<%= sMsgAnniversaryDayEquallRenewalDay %>' == '') {
                        alert("Anniversary day has to be the same as the monthly renewal day");
                    }
                    else {
                        alert('<%= sMsgAnniversaryDayEquallRenewalDay %>');
                    }
                    oControlAnniversaryDate.value = document.getElementById('<%=hfOriginalAnniversaryDate.ClientID%>').value; // DD/MM/YYYY Format.
                }
                else {
                    var dAnniversaryDate = new Date(oControlAnniversaryDate.value);
                    var dRenewalDate = new Date(oControlRenewalDate.value);
                    if (dAnniversaryDate < dRenewalDate) {
                        if ('<%= sMsgAnniversaryDayGreaterRenewalDay %>' == '') {
                            alert("Anniversary Date must be after Renewal Date")
                        }
                        else {
                            alert('<%= sMsgAnniversaryDayGreaterRenewalDay %>');
                        }

                        //On OK return to Policy screen and change Anniversary date back to Default
                        if (document.getElementById('<%=hfOriginalAnniversaryDate.ClientID%>').value != '')
                            oControlAnniversaryDate.value = document.getElementById('<%=hfOriginalAnniversaryDate.ClientID%>').value;
                        else
                        oControlAnniversaryDate.value = formatDate(oControlRenewalDate.value)
                    }
                }
            }
        }

        function FillAnniversaryDate() {
            // Retrieve Cover From date.
            var dCoverStartDate = $get('<%=POLICYHEADER__COVERSTARTDATE.ClientID%>').value;
            // Retrieve selected index of Dropdown List
            var IndexValue = $get('<%=POLICYHEADER__UNIFIEDRENEWALDAY.ClientID %>').selectedIndex;
            // Retrieve selected value of Dropdown List
            var UnifiedRenewalDay = $get('<%=POLICYHEADER__UNIFIEDRENEWALDAY.ClientID %>').options[IndexValue].value;
            // Retrieve Anniversary Date Control
            var oControlAnniversaryDate = $get('<%=POLICYHEADER__ANNIVERSARY.ClientID%>');

            if (IsUnifiedRenewalDateChanged == 1 && oControlAnniversaryDate.value != "") {
                var arOldAnniversaryDate = oControlAnniversaryDate.value.split('/');
                oControlAnniversaryDate.value = formatDate(UnifiedRenewalDay + '/' + arOldAnniversaryDate[1] + '/' + arOldAnniversaryDate[2]);
            }
            else {
            var arStartDate = dCoverStartDate.split('/');
            var dtTempDate = new Date(arStartDate[2], arStartDate[1], arStartDate[0]);
            if (parseInt(UnifiedRenewalDay, 10) > parseInt(arStartDate[0], 10)) {
                oControlAnniversaryDate.value = formatDate(UnifiedRenewalDay + '/' + arStartDate[1] + '/' + (parseInt(arStartDate[2], 10) + 1));
            }
            else {
                oControlAnniversaryDate.value = formatDate(UnifiedRenewalDay + '/' + arStartDate[1] + '/' + (parseInt(arStartDate[2], 10) + 1));
            }
        }
            //Keeping initial Anniversary Date
            document.getElementById('<%=hfOriginalAnniversaryDate.ClientID%>').value = oControlAnniversaryDate.value;
        }

        //DD/MM/YYYY
        function formatDate(tempDate) {
            var artempDate = tempDate.split('/');
            var strFormatedDate;
            if (artempDate[0].length == 1) {
                artempDate[0] = '0' + artempDate[0];
            }
            if (artempDate[1].length == 1) {
                artempDate[1] = '0' + artempDate[1];
            }
            return strFormatedDate = artempDate[0] + '/' + artempDate[1] + '/' + artempDate[2];
        }

        //Validate And Reset Branch
        function ShowValidationAndResetBranch(sBranchCode) {
            alert('The Agent does not have access to the selected branch');
            $('#<%=POLICYHEADER__BRANCH.ClientID%>').val(sBranchCode);
        }

        //Enable and disable Business Type and Agent Code button
        function DisableBusinessType_AgentCodeBtn()
        {
            $('#<%= POLICYHEADER__BUSINESSTYPE.ClientID%>').attr("disabled", true);
            $('#<%= btnAgentCode.ClientID%>').attr("disabled", true);
            
        }
        
         //Validate back dated renewal
        function ValidateCoverDatesForRenewal() {
            alert("Cover Start Date cannot be prior to the Renewal Date of the Current Term");
            return false;
        }

        //Validate agency cancellation date
        function ValidateAgencyCancellationDate() {
            if ($('#<%=hfAgencyCancellationDate.ClientID%>').val() != '') {
                var dtAgencyCancellationDate = new Date($('#<%=hfAgencyCancellationDate.ClientID%>').val());
                var dtDateToValidateForAgencyCancellation = new Date($('#<%=hfDateToValidateForAgencyCancellation.ClientID%>').val());
                if (dtAgencyCancellationDate <= dtDateToValidateForAgencyCancellation) {
                    return confirm('<%= sMsgAgencyCancellation %>');
                }
            }
        }

        function ValidateCoverDates() {
            var MonthInFutureAllowedForCoverFromDate = $('#<%=hfMonthInFutureAllowedForCoverFromDate.ClientID%>').val();  
            var MonthInFutureAllowedForCoverToDate = $('#<%=hfMonthInFutureAllowedForCoverToDate.ClientID%>').val();  
            var CoverFromDate = $('#<%=POLICYHEADER__COVERSTARTDATE.ClientID%>').val()
            var CoverFromDateValue = formatDate(CoverFromDate);
            var arCoverFromDateValue = CoverFromDateValue.split('/');
            var dtCoverFromDateValue = new Date(arCoverFromDateValue[2], arCoverFromDateValue[1] - 1, arCoverFromDateValue[0]);

            var CoverToDate = $('#<%=POLICYHEADER__COVERENDDATE.ClientID%>').val() 
            var CoverToDateValue = formatDate(CoverToDate);
            var arCoverToDateValue = CoverToDateValue.split('/');
            var dtCoverToDateValue = new Date(arCoverToDateValue[2], arCoverToDateValue[1] - 1, arCoverToDateValue[0]);

            var CoverFromDateInFutureValidationMessage = '<%= sMsg_CoverFromDateInFutureValidationMessage%>';
            var CoverToDateInFutureValidationMessage = '<%= sMsg_CoverToDateInFutureValidationMessage%>';
            var sServerDate = formatDate('<%= sServerDate %>');
            var arServerDate = sServerDate.split('/');

            var dtMaxCoverFromDate = new Date(arServerDate[2], arServerDate[1] - 1, arServerDate[0]);
            dtMaxCoverFromDate.setMonth(dtMaxCoverFromDate.getMonth() + parseInt(MonthInFutureAllowedForCoverFromDate, 10));

            var dtMaxCoverToDate = new Date(arCoverFromDateValue[2], arCoverFromDateValue[1] - 1, arCoverFromDateValue[0]);
            dtMaxCoverToDate.setMonth(dtMaxCoverToDate.getMonth() + parseInt(MonthInFutureAllowedForCoverToDate, 10));

            if (dtCoverFromDateValue > dtMaxCoverFromDate) {
                if (confirm(CoverFromDateInFutureValidationMessage)) {
                    if (dtCoverToDateValue > dtMaxCoverToDate) {
                        alert(CoverToDateInFutureValidationMessage);
                        return false;
                    }
                    else {
                        return true;
                    }
                }
                else {
                    return false;
                }
            }
            else {
                if (dtCoverToDateValue > dtMaxCoverToDate) {
                    alert(CoverToDateInFutureValidationMessage);
                    return false;
                }
            }
        }

        //Compare Start and End Date
        // If Start Date is small return 1
        //If Start Date is greater return 2
        //If both date Equal return 0
        function fn_DateDiff(DateA, DateB) {
            var d1_str = DateA;
            var d2_str = DateB;
            var d1 = new Date(d1_str.split('/')[2], (d1_str.split('/')[1] - 1), d1_str.split('/')[0]);
            var d2 = new Date(d2_str.split('/')[2], (d2_str.split('/')[1] - 1), d2_str.split('/')[0]);
            if (d1.getTime() < d2.getTime()) {
                return 1;
            } else if (d1.getTime() > d2.getTime()) {
                return -1;
            } else {
                return 0;
            }
        }

        function ConfirmCoverEndDate() {
            var OriginalCoverToDate = formatDate($('#<%= hfOriginalCoverEndDate.ClientID%>').val());
            var arOriginalCoverToDate = OriginalCoverToDate.split('/');
            var dtOriginalCoverToDate = new Date(arOriginalCoverToDate[2], arOriginalCoverToDate[1] - 1, arOriginalCoverToDate[0]);

            var CurrentCoverToDate = formatDate($('#<%= POLICYHEADER__COVERENDDATE.ClientID%>').val());
            var arCurrentCoverToDate = CurrentCoverToDate.split('/');
            var dtCurrentCoverToDate = new Date(arCurrentCoverToDate[2], arCurrentCoverToDate[1] - 1, arCurrentCoverToDate[0]);

            var OriginalCoverFromDate = formatDate($('#<%= hfOriginalCoverStartDate.ClientID%>').val());
            var arOriginalCoverFromDate = OriginalCoverFromDate.split('/');
            var dtOriginalCoverFromDate = new Date(arOriginalCoverFromDate[2], arOriginalCoverFromDate[1] - 1, arOriginalCoverFromDate[0]);

            var CurrentCoverFromDate = formatDate($('#<%= POLICYHEADER__COVERSTARTDATE.ClientID%>').val());
            var arCurrentCoverFromDate = CurrentCoverFromDate.split('/');
            var dtCurrentCoverFromDate = new Date(arCurrentCoverFromDate[2], arCurrentCoverFromDate[1] - 1, arCurrentCoverFromDate[0]);
            var result = CheckCoverToDateIsValid();
            if (result == false) {
                return false;
            }
            var mode = '<%=Session(Nexus.Constants.CNMode)%>';
            if (mode != "View") {
                if ((dtOriginalCoverFromDate.getTime() != dtCurrentCoverFromDate.getTime()) && ('<%=Session(Nexus.Constants.CNMTAType) Is Nothing%>' == 'True') && ('<%= CType(Session(Nexus.Constants.CNQuote), NexusProvider.Quote).Risks.Count > 0%>' == 'True')) {
                    return confirm('<%=GetLocalResourceObject("msgChangeInCoverFromDate")%>');
                }
                else if ((dtOriginalCoverToDate.getTime() != dtCurrentCoverToDate.getTime()) && ('<%=Session(Nexus.Constants.CNMTAType) Is Nothing%>' == 'False')) {
                    return confirm('<%=GetLocalResourceObject("msgChangeInCoverToDate")%>');
                }
        }
            else {
                return true;
            }
            return ValidateCoverDates();
        }

        function CheckCoverToDateIsValid() {
            var bIsPolicyInRenewal = $get('<%=hfIsPolicyInRenewal.ClientID%>').value;
		    var bDoNotDeleteRenewalQuoteOnMTA = $get('<%=hfDoNotDeleteRenewalQuoteOnMTA.ClientID%>').value;
		    var dRenewalVersionStartDate = $get('<%=hfRenewalVersionStartDate.ClientID%>').value;
		    var dCoverToDate = $get('<%=hfRequiredCoverEndDate.ClientID%>').value;
		    var IsTrueMonthlyProduct = $get('<%=hfIsTrueMonthlyProduct.ClientID%>').value;
		    var MidnightRenewalSettings = $get('<%=hfMidnightRenewalSettings.ClientID%>').value;
		    var hfDeletePolicyFromRenewal = $get('<%=hfDeletePolicyFromRenewal.ClientID%>')
		    var dCurrentRenewalDate;

		    if (IsTrueMonthlyProduct != 1 && hfDeletePolicyFromRenewal.value != "True") {
		        if (MidnightRenewalSettings == 1) {
		            var CoverToDate = $get('<%=POLICYHEADER__COVERENDDATE.ClientID%>');
                    var arRenewalDate = CoverToDate.value.split('/');
                    var dtTempDate = new Date(arRenewalDate[2], arRenewalDate[1] - 1, arRenewalDate[0]);
                    var dtRenewalDate = new Date(dtTempDate);
                    dtRenewalDate.setDate(dtRenewalDate.getDate() + 1);
                    if ((dtRenewalDate.getMonth() + 1) > 12) {
                        dCurrentRenewalDate = formatDate(dtRenewalDate.getDate() + '/' + 01 + '/' + (dtRenewalDate.getFullYear() + 1));
                    }
                    else {
                        dCurrentRenewalDate = formatDate(dtRenewalDate.getDate() + '/' + (dtRenewalDate.getMonth() + 1) + '/' + dtRenewalDate.getFullYear());
                    }
                }
                else {
                    //Set Renewal Date = Cover To Date
                    dCurrentRenewalDate = CoverToDate.value;
                }
                if (bIsPolicyInRenewal == "True") {
                    if (bDoNotDeleteRenewalQuoteOnMTA == "1" && dCurrentRenewalDate != dRenewalVersionStartDate) {
                        var msg = '<%=GetLocalResourceObject("msgPolicyIsInRenewalConflict")%>';
                        var result = confirm(msg);
                        if (result == true) {
                            hfDeletePolicyFromRenewal.value = "True";
                            return true;
                        }
                        else {
                            hfDeletePolicyFromRenewal.value = "False";
                            CoverToDate.value = dCoverToDate;
                            fillDateRenewal(0);
                            return false;
                        }
                    }
                }
            }
        }

    </script>

    <asp:ScriptManager ID="ScriptManagerMainDetails" runat="server"></asp:ScriptManager>

    <uc3:ProgressBar ID="ProgressBar1" runat="server"></uc3:ProgressBar>
    <uc5:PolicyHeader ID="ctrlTabIndex" runat="server"></uc5:PolicyHeader>
    <asp:HiddenField ID="hdnIsBroker" runat="server" Value="0"></asp:HiddenField>
    <div class="tab-content p b-a no-b-t bg-white m-b-md">
        <asp:Panel ID="pageContainerDiv" runat="server">
            <div class="card-body clearfix no-padding">
                <div class="form-horizontal">
                                <legend>
                        <asp:Label ID="lblBasicDetails" runat="server" Text="<%$ Resources:lblBasicDetails %>"></asp:Label>
                                </legend>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblInsuredName" runat="server" AssociatedControlID="POLICYHEADER__INSUREDNAME" Text="<%$ Resources:lblInsuredName %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="POLICYHEADER__INSUREDNAME" runat="server" CssClass="field-mandatory form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="vldrqdInsuredName" runat="server" ControlToValidate="POLICYHEADER__INSUREDNAME" Display="none" Enabled="true" SetFocusOnError="true" ErrorMessage="<%$ Resources:vldInsuredName %>"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lbAlternateRef" runat="server" AssociatedControlID="POLICYHEADER__ALTERNATEREF" Text="<%$ Resources:lbAlternateRef %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="POLICYHEADER__ALTERNATEREF" runat="server" CssClass="form-control" MaxLength="80"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="vldrqdAlternateRef" runat="server" ControlToValidate="POLICYHEADER__ALTERNATEREF" Display="none" Enabled="false" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_vldAlternateRef %>"></asp:RequiredFieldValidator>
                         <asp:RegularExpressionValidator ID="RegexAlternateRef" runat="server" Display="None" ControlToValidate="POLICYHEADER__ALTERNATEREF" ErrorMessage="<%$ Resources:lbl_RegexAlternateRef %>" ValidationExpression="^[a-zA-Z0-9\-_()\/\s]+$" SetFocusOnError="True" Enabled="true" />
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPolicyNumber" runat="server" AssociatedControlID="POLICYHEADER__POLICYNUMBER" Text="<%$ Resources:lblPolicyNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="POLICYHEADER__POLICYNUMBER" runat="server" CssClass="field-mandatory form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvPolicyNumber" runat="server" ControlToValidate="POLICYHEADER__POLICYNUMBER" Display="none" Enabled="true" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_vldPolicyNumber %>"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblProduct" runat="server" AssociatedControlID="POLICYHEADER__PRODUCT" Text="<%$ Resources:lblProduct %>" class="col-md-4 col-sm-3 control-label"> 
                                        </asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="POLICYHEADER__PRODUCT" runat="server" CssClass=" form-control form-select"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPolicyStatus" runat="server" AssociatedControlID="POLICYHEADER__POLICYSTATUSCODE" Text="<%$ Resources:lblPolicyStatus %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="POLICYHEADER__POLICYSTATUSCODE" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Policy_Status" DefaultText="<%$ Resources:lblDefaultText %>" CssClass=" form-control"></NexusProvider:LookupList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblBranchCode" runat="server" AssociatedControlID="POLICYHEADER__BRANCH" Text="<%$ Resources:lblBranchCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:DropDownList ID="POLICYHEADER__BRANCH" runat="server" AutoPostBack="true" CausesValidation="false" CssClass=" field-mandatory form-control form-select"></asp:DropDownList>
                        </div>
                        <asp:RequiredFieldValidator ID="vldBranchCodeRequired" runat="server" Display="None" ControlToValidate="POLICYHEADER__BRANCH" ErrorMessage="<%$ Resources:vldBranchCode %>" SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblAnalysisCode" runat="server" AssociatedControlID="POLICYHEADER__ANALYSISCODE" Text="<%$ Resources:lblAnalysisCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="POLICYHEADER__ANALYSISCODE" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Analysis_Code" DefaultText="<%$ Resources:lblDefaultText %>" CssClass=" form-control"></NexusProvider:LookupList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblSubBranchCode" runat="server" AssociatedControlID="POLICYHEADER__SUBBRANCH" Text="<%$ Resources:lblSubBranchCode %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <asp:UpdatePanel ID="updSubBranch" runat="server" UpdateMode="Conditional" class="col-md-8 col-sm-9">
                                            <ContentTemplate>
                                                <div>
                                                    <asp:DropDownList ID="POLICYHEADER__SUBBRANCH" runat="server" CssClass=" field-mandatory form-control form-select"></asp:DropDownList>
                                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="POLICYHEADER__BRANCH" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                                            </Triggers>
                                        </asp:UpdatePanel>
                        <asp:RequiredFieldValidator ID="vldSubBranchCodeRequired" runat="server" Display="None" ControlToValidate="POLICYHEADER__SUBBRANCH" ErrorMessage="<%$ Resources:vldSubBranchCode %>" SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblBusinessSource" runat="server" AssociatedControlID="POLICYHEADER__BUSINESSTYPE" Text="<%$ Resources:lblBusinessSource %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                        <asp:UpdatePanel ID="updBusinessType" runat="server" class="col-md-8 col-sm-9">
                                            <ContentTemplate>
                                                <div>
                                                    <NexusProvider:LookupList ID="POLICYHEADER__BUSINESSTYPE" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Business_Type" DefaultText="<%$ Resources:lblDefaultText %>" CssClass=" field-mandatory form-control" AutoPostBack="true"></NexusProvider:LookupList>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                        <asp:RequiredFieldValidator ID="vldrqdBusinessSource" runat="server" ControlToValidate="POLICYHEADER__BUSINESSTYPE" Display="none" Enabled="true" SetFocusOnError="true" ErrorMessage="<%$ Resources:vldBusiness %>"></asp:RequiredFieldValidator>
                    </div>
                    <div id="liCoinsurancePlacement" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:UpdatePanel ID="POLICYHEADER__updCoinsurancePlacement" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblCoinsurancePlacement" runat="server" AssociatedControlID="POLICYHEADER__COINSURANCEPLACEMENT" Text="<%$ Resources:lblCoinsurancePlacement %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:RadioButtonList ID="POLICYHEADER__COINSURANCEPLACEMENT" runat="server" Enabled="false" RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="asp-radio">
                                        <asp:ListItem Text="<%$ Resources:lblCoinsurancePlacementGrossText %>"  Value="GROSS"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources:lblCoinsurancePlacementNetText %>"  Value="NETT"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>


                                <asp:RequiredFieldValidator ID="vldrqdCoinsurancePlacement" runat="server" ControlToValidate="POLICYHEADER__COINSURANCEPLACEMENT" Display="none" Enabled="false" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_vldCoinsurancePlacement %>"></asp:RequiredFieldValidator>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="POLICYHEADER__BUSINESSTYPE" EventName="SelectedIndexChange"></asp:AsyncPostBackTrigger>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:UpdatePanel ID="POLICYHEADER__updPanelAgent" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblAgentCode" AssociatedControlID="POLICYHEADER__AGENTCODE" Text="<%$ Resources:lbl_btnAgentCode %>" class="col-md-4 col-sm-3 control-label">
                                                </asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="POLICYHEADER__AGENTCODE" runat="server" CssClass="form-control"></asp:TextBox>
                                        <span class="input-group-btn">
                                            <asp:LinkButton ID="btnAgentCode" runat="server" SkinID="btnModal" CausesValidation="false">
                                                <i class="fa fa-search"></i>
                                                <span class="btn-fnd-txt">Agent Code</span>
                                            </asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <asp:HiddenField ID="POLICYHEADER__AGENT" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hAgentCode" runat="server"></asp:HiddenField>
                                <asp:HiddenField ID="hAgentType" runat="server"></asp:HiddenField>
                                <asp:RequiredFieldValidator ID="rfvAgentCode" runat="server" ControlToValidate="POLICYHEADER__AGENTCODE" Display="none" Enabled="false" SetFocusOnError="true" ErrorMessage="<%$ Resources:lbl_vldAgentCode %>"></asp:RequiredFieldValidator>
                                            </ContentTemplate>
                                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="POLICYHEADER__BUSINESSTYPE" EventName="SelectedIndexChange"></asp:AsyncPostBackTrigger>
                                <asp:AsyncPostBackTrigger ControlID="POLICYHEADER__BRANCH" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                                            </Triggers>
                                        </asp:UpdatePanel>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:UpdatePanel ID="POLICYHEADER__UpdatePanelCurrency" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                <asp:Label ID="lblCurrency" runat="server" AssociatedControlID="POLICYHEADER__CURRENCY" Text="<%$ Resources:lblCurrency %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:DropDownList ID="POLICYHEADER__CURRENCY" runat="server" CssClass=" field-mandatory form-control form-select"></asp:DropDownList>
                                </div>
                                            </ContentTemplate>
                                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="POLICYHEADER__BRANCH" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                                            </Triggers>
                                        </asp:UpdatePanel>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label runat="server" ID="lblAccountHandler" AssociatedControlID="POLICYHEADER__HANDLER" Text="<%$ Resources:lbl_btnHandler %>" class="col-md-4 col-sm-3 control-label">
                                        </asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="POLICYHEADER__HANDLER" runat="server" CssClass="form-control"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="btnHandler" runat="server" CausesValidation="false" SkinID="btnModal">
                                        <i class="fa fa-search"></i>
                                        <span class="btn-fnd-txt">A/C Handler</span>
                                    </asp:LinkButton>
                                </span>
                            </div>
                        </div>
                        <asp:HiddenField ID="hfHandlerCode" runat="server"></asp:HiddenField>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblFrequency" runat="server" AssociatedControlID="POLICYHEADER__FREQUENCY" Text="<%$ Resources:lblFrequency %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="POLICYHEADER__FREQUENCY" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Renewal_Frequency" DefaultText="<%$ Resources:lblDefaultText %>" CssClass=" field-mandatory form-control" AutoPostBack="true"></NexusProvider:LookupList>
                        </div>
                        <asp:RequiredFieldValidator ID="vldrqdFrequency" runat="server" ControlToValidate="POLICYHEADER__FREQUENCY" ErrorMessage="<%$ Resources:lbl_vldFrequency %>" Display="None" SetFocusOnError="true" Enabled="true"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRegarding" runat="server" AssociatedControlID="POLICYHEADER__REGARDING" Text="<%$ Resources:lblRegarding %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="POLICYHEADER__REGARDING" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCoverStartDate" runat="server" AssociatedControlID="POLICYHEADER__COVERSTARTDATE" Text="<%$ Resources:lblCoverStartDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>

                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="POLICYHEADER__COVERSTARTDATE" runat="server" CssClass="field-mandatory form-control" onBlur=" if($(this).attr('readonly')!='readonly'){ fillDate('<%=iMode%>')}"></asp:TextBox><uc1:CalendarLookup ID="calCoverFromDate" runat="server" LinkedControl="POLICYHEADER__COVERSTARTDATE" HLevel="2"></uc1:CalendarLookup>
                            </div>
                        </div>

                        <asp:RequiredFieldValidator ID="vldrqdCoverFromDate" runat="server" Display="none" ErrorMessage="<%$ Resources:lbl_vldCoverStartDate %>" ControlToValidate="POLICYHEADER__COVERSTARTDATE" Enabled="true" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="custFromDate" runat="server" Display="none" ControlToValidate="POLICYHEADER__COVERSTARTDATE" SetFocusOnError="true"></asp:CustomValidator>
                        <asp:RegularExpressionValidator ID="regexvldFromDate" runat="server" ControlToValidate="POLICYHEADER__COVERSTARTDATE" Display="None" ErrorMessage="<%$ Resources:lbl_vldCoverStartDate %>" ValidationExpression="(0[1-9]|1[012])[- /\\.](0[1-9]|[12][0-9]|3[01])[- /\\.](19|20)\d\d" SetFocusOnError="true" Enabled="False"></asp:RegularExpressionValidator>
                        <asp:HiddenField ID="hfOriginalCoverStartDate" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hfGracePeriod" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hfMonthInFutureAllowedForCoverToDate" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hfMonthInFutureAllowedForCoverFromDate" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hfMidnightRenewalSettings" runat="server"></asp:HiddenField>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblCoverEndDate" runat="server" AssociatedControlID="POLICYHEADER__COVERENDDATE" Text="<%$ Resources:lblCoverEndDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>

                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="POLICYHEADER__COVERENDDATE" runat="server" CssClass="field-mandatory form-control" onblur="if($(this).attr('readonly')!=true) { fillDateRenewal(0); }"></asp:TextBox><uc1:CalendarLookup ID="calCoverEndDate" runat="server" LinkedControl="POLICYHEADER__COVERENDDATE" HLevel="2"></uc1:CalendarLookup>
                            </div>
                        </div>

                        <asp:RequiredFieldValidator ID="vldrqdCoverEndDate" runat="server" Display="none" ErrorMessage="<%$ Resources:lbl_vldCoverEndDate %>" ControlToValidate="POLICYHEADER__COVERENDDATE" Enabled="true" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="regexvldEndDate" runat="server" ControlToValidate="POLICYHEADER__COVERENDDATE" Display="None" ErrorMessage="<%$ Resources:lbl_vldCoverEndDateFormat %>" ValidationExpression="(0[1-9]|1[012])[- /\\.](0[1-9]|[12][0-9]|3[01])[- /\\.](19|20)\d\d" SetFocusOnError="true" Enabled="False"></asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="custToDate" runat="server" Display="none" ControlToValidate="POLICYHEADER__COVERENDDATE" SetFocusOnError="true"></asp:CustomValidator>
                        <asp:CompareValidator ID="compvldCoverEndDate" runat="server" ControlToCompare="POLICYHEADER__COVERSTARTDATE" Display="None" ErrorMessage="<%$ Resources:lbl_vldCoverEndDate %>" ControlToValidate="POLICYHEADER__COVERENDDATE" SetFocusOnError="true" Operator="GreaterThanEqual" Type="Date" Enabled="True"></asp:CompareValidator>
                        <asp:HiddenField ID="hfOriginalCoverEndDate" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hfNeedToUpdateOnStartChange" runat="server"></asp:HiddenField>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblConsolidatedLeadAgentCommission" runat="server" AssociatedControlID="POLICYHEADER__CONSOLIDATEDLEADAGENTCOMMISSION" Text="<%$ Resources:lblConsolidatedLeadAgentCommission %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="POLICYHEADER__CONSOLIDATEDLEADAGENTCOMMISSION" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblRenewalMethod" runat="server" AssociatedControlID="POLICYHEADER__RENEWALMETHOD" Text="<%$ Resources:lblRenewalMethod %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="POLICYHEADER__RENEWALMETHOD" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="Renewal_Method" DefaultText="<%$ Resources:lblDefaultText %>" CssClass=" form-control"></NexusProvider:LookupList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblLTUExpiry" runat="server" AssociatedControlID="POLICYHEADER__LTUEXPIRYDATE" Text="<%$ Resources:lblLTUExpiry %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="POLICYHEADER__LTUEXPIRYDATE" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calLTUExpiry" runat="server" LinkedControl="POLICYHEADER__LTUEXPIRYDATE" HLevel="2"></uc1:CalendarLookup>
                            </div>
                        </div>

                        <asp:CompareValidator ID="cmpLTUExpiryDate" Type="Date" ControlToValidate="POLICYHEADER__LTUEXPIRYDATE" Operator="DataTypeCheck" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_vldLTUExpiry %>"></asp:CompareValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblLapseCancelReason" runat="server" AssociatedControlID="POLICYHEADER__LAPSECANCELREASON" Text="<%$ Resources:lblLapseCancelReason %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="POLICYHEADER__LAPSECANCELREASON" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="lapsed_reason" CssClass=" form-control" DefaultText="<%$ Resources:lblDefaultText %>"></NexusProvider:LookupList>
                        </div>
                        <asp:RequiredFieldValidator ID="vldrqdLapseCancelReason" runat="server" ControlToValidate="POLICYHEADER__LAPSECANCELREASON" ErrorMessage="<%$ Resources:lbl_vldLapseCancelReason %>" Display="None" SetFocusOnError="true" Enabled="false" ValidationGroup="grpLapseQuote"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblStopReason" runat="server" AssociatedControlID="POLICYHEADER__STOPREASON" Text="<%$ Resources:lblStopReason %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="POLICYHEADER__STOPREASON" runat="server" DataItemValue="Code" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="renewal_stop_code" CssClass=" form-control" DefaultText="<%$ Resources:lblDefaultText %>"></NexusProvider:LookupList>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblLapseCancelDate" runat="server" Text="<%$ Resources:lblLapseCancelDate %>" AssociatedControlID="POLICYHEADER__LAPSECANCELDATE" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="POLICYHEADER__LAPSECANCELDATE" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calLapseDate" runat="server" LinkedControl="POLICYHEADER__LAPSECANCELDATE" HLevel="2"></uc1:CalendarLookup>
                            </div>
                        </div>

                        <asp:RequiredFieldValidator ID="vldrqdLapseCancelDate" runat="server" ControlToValidate="POLICYHEADER__LAPSECANCELDATE" ErrorMessage="Lapse/Cancel date not specified" Display="None" SetFocusOnError="true" Enabled="false" ValidationGroup="grpLapseQuote"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="regexvldLapseCancelDate" runat="server" ControlToValidate="POLICYHEADER__LAPSECANCELDATE" Display="None" ErrorMessage="Invalid Lapse/Cancellation Date1" ValidationExpression="(0[1-9]|1[012])[- /\\.](0[1-9]|[12][0-9]|3[01])[- /\\.](19|20)\d\d" SetFocusOnError="true" Enabled="False"></asp:RegularExpressionValidator>
                        <asp:RangeValidator ID="rngLapseCancelDate" runat="server" ControlToValidate="POLICYHEADER__LAPSECANCELDATE" Display="None" ErrorMessage="Please enter a date between 01/01/1900 and 12/12/9998" SetFocusOnError="true" Type="Date" MaximumValue="12/12/9998" MinimumValue="01/01/1900" ValidationGroup="grpLapseQuote"></asp:RangeValidator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblPOLICYHEADER_CORRESPONDENCETYPE" runat="server" AssociatedControlID="POLICYHEADER__CORRESPONDENCETYPE"  class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:LookupList ID="POLICYHEADER__CORRESPONDENCETYPE" runat="server" DataItemValue="Code" DefaultText="(Please Select)" DataItemText="Description" Sort="Asc" ListType="PMLookup" ListCode="Correspondence_Type" onchange='onValidate_POLICYHEADER__CORRESPONDENCETYPE(null, null, this);' CssClass=" form-control" AutoPostBack="True" ParentFieldName="" ParentLookupListID="" Value="" data-type="List"></NexusProvider:LookupList>
                            <asp:TextBox ID="POLICYHEADER__DEFAULTPREFERREDCORRESPONDENCE" runat="server" CssClass="form-control m-t-xs" data-type="Text" Visible="false" Enabled="false"></asp:TextBox>
                        </div>
                        <asp:HiddenField ID="POLICYHEADER__DEFAULTCORRESPONDENCECODE" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="POLICYHEADER__RECEIVESCLIENTCORRESPONDENCE" runat="server"></asp:HiddenField>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblTimesRenewed" runat="server" AssociatedControlID="lblRenewedTimes" Text="<%$ Resources:lblTimesRenewed %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <p class="form-control-static font-bold">
                                <asp:Label ID="lblRenewedTimes" runat="server"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblReferredAtRenewal" runat="server" AssociatedControlID="POLICYHEADER__REFERREDATRENEWAL" Text="<%$ Resources:lblReferredAtRenewal %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="POLICYHEADER__REFERREDATRENEWAL" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblReferredAtMTA" runat="server" AssociatedControlID="POLICYHEADER__REFERREDATMTA" Text="<%$ Resources:lblReferredAtMTA %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="POLICYHEADER__REFERREDATMTA" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>
                    <div id="liContactName" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                        <asp:UpdatePanel ID="POLICYHEADER__updPanelContactName" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblWhoCanContacted" Text="<%$ Resources:lblWhoCanContacted %>" AssociatedControlID="POLICYHEADER__CONTACT_NAME" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                <div class="col-md-8 col-sm-9">
                                    <asp:DropDownList ID="POLICYHEADER__CONTACT_NAME" runat="server" CssClass=" form-control form-select" Enabled="false" AutoPostBack="true" onChange="TrackChanges();"></asp:DropDownList>
                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                        <nexus:ProgressIndicator ID="upContactName" OverlayCssClass="updating" AssociatedUpdatePanelID="POLICYHEADER__updPanelContactName" runat="server">
                                            <ProgressTemplate>
                                            </ProgressTemplate>
                                        </nexus:ProgressIndicator>
                    </div>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <asp:Label ID="lblOldPolicyNo" runat="server" AssociatedControlID="POLICYHEADER__OldPolicyNo" Text="<%$ Resources:lblOldPolicyNo %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="POLICYHEADER__OldPolicyNo" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                        </div>
                                       
                    </div>
                </div>
                            <asp:Panel ID="POLICYHEADER__COVERDATESPANEL" runat="server">
                    <div class="form-horizontal">
                                    <legend>
                            <asp:Label ID="lblCoverDateHeading" runat="server" Text="<%$ Resources:lblCoverDateHeading %>"></asp:Label>
                                    </legend>

                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblInception" runat="server" AssociatedControlID="POLICYHEADER__INCEPTION" Text="<%$ Resources:lblInception %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="POLICYHEADER__INCEPTION" runat="server" CssClass="field-mandatory form-control"></asp:TextBox><uc1:CalendarLookup ID="calInceptionDate" runat="server" HLevel="3" LinkedControl="POLICYHEADER__INCEPTION" Enabled="false"></uc1:CalendarLookup>
                                </div>
                            </div>

                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblRenewalDate" runat="server" AssociatedControlID="POLICYHEADER__RENEWAL" Text="<%$ Resources:lblRenewalDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="POLICYHEADER__RENEWAL" runat="server" onblur="if($(this).attr('readonly')!=true) { fillDateRenewal(2); }" CssClass="field-mandatory form-control"></asp:TextBox><uc1:CalendarLookup ID="calRenewalDate" runat="server" HLevel="3" LinkedControl="POLICYHEADER__RENEWAL" Enabled="false"></uc1:CalendarLookup>
                                </div>
                            </div>

                        </div>
                        <div id="liUnifiedRenewalDay" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblUnifiedRenewalDay" runat="server" AssociatedControlID="POLICYHEADER__UNIFIEDRENEWALDAY" Text="<%$ Resources:lblUnifiedRenewalDay %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="POLICYHEADER__UNIFIEDRENEWALDAY" runat="server" onChange="fillDateRenewal(1);TrackChanges();" CssClass="form-control form-select">
                                            </asp:DropDownList>
                            </div>
                            <asp:HiddenField ID="hfIsTrueMonthlyProduct" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="hfIsUnifiedRenewalDayReadOnly" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="hfCoverStartDate" runat="server"></asp:HiddenField>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblInceptionTPI" runat="server" AssociatedControlID="POLICYHEADER__INCEPTIONTPI" Text="<%$ Resources:lblInceptionTPI %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="POLICYHEADER__INCEPTIONTPI" runat="server" CssClass="field-mandatory form-control"></asp:TextBox><uc1:CalendarLookup ID="calInceptionTPI" runat="server" HLevel="3" Enabled="false" LinkedControl="POLICYHEADER__INCEPTIONTPI"></uc1:CalendarLookup>
                                </div>
                            </div>

                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblProposalDate" runat="server" AssociatedControlID="POLICYHEADER__PROPOSALDATE" Text="<%$ Resources:lblProposalDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="POLICYHEADER__PROPOSALDATE" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calProposalDate" runat="server" HLevel="3" LinkedControl="POLICYHEADER__PROPOSALDATE"></uc1:CalendarLookup>
                                </div>
                            </div>

                            <asp:CompareValidator ID="CompareValidator1" Type="Date" ControlToValidate="POLICYHEADER__PROPOSALDATE" Operator="DataTypeCheck" runat="server" Display="None" ErrorMessage="<%$ Resources:lbl_vldProposalDate %>"></asp:CompareValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblQuoteExpiryDate" runat="server" AssociatedControlID="POLICYHEADER__QUOTEEXPIRYDATE" Text="<%$ Resources:lblQuoteExpiryDate %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="POLICYHEADER__QUOTEEXPIRYDATE" runat="server" CssClass="field-mandatory form-control"></asp:TextBox><uc1:CalendarLookup ID="calQuoteExpiryDate" runat="server" HLevel="3" Enabled="false" LinkedControl="POLICYHEADER__QUOTEEXPIRYDATE"></uc1:CalendarLookup>
                                </div>
                            </div>

                        </div>
                        <div id="liUnderwritingYear" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblUnderwritingYear" runat="server" AssociatedControlID="POLICYHEADER__UNDERWRITINGYEAR" Text="<%$ Resources:lblUnderwritingYear %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <NexusProvider:LookupList ID="POLICYHEADER__UNDERWRITINGYEAR" runat="server" DataItemValue="Key" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="UnderWriting_Year" DefaultText="<%$ Resources:lblDefaultText %>" CssClass="form-control"></NexusProvider:LookupList>
                            </div>
                            <asp:RequiredFieldValidator ID="rfvUnderwritingYear" runat="server" ErrorMessage="<%$ Resources:lbl_vldUnderwritingYear %>" SetFocusOnError="true" Display="None" ControlToValidate="POLICYHEADER__UNDERWRITINGYEAR" Enabled="false"></asp:RequiredFieldValidator>
                        </div>
                        <div id="liAnniversaryDate" runat="server" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblAnniversaryDate" runat="server" AssociatedControlID="POLICYHEADER__ANNIVERSARY" Text="<%$ Resources:lblAnniversary %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="POLICYHEADER__ANNIVERSARY" runat="server" CssClass="form-control"></asp:TextBox><uc1:CalendarLookup ID="calAnniversary" runat="server" LinkedControl="POLICYHEADER__ANNIVERSARY" HLevel="4"></uc1:CalendarLookup>
                                </div>
                            </div>

                            <asp:RequiredFieldValidator ID="rfvAnniversaryDate" runat="server" Display="none" ErrorMessage="<%$ Resources:lbl_vldInvalidAnniversaryDate %>" Enabled="false" ControlToValidate="POLICYHEADER__ANNIVERSARY"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cvAnniversaryDate" runat="server" ControlToValidate="POLICYHEADER__ANNIVERSARY" ClientValidationFunction="ValidateAnniversaryDate" Enabled="false" ErrorMessage="<%$ Resources:lbl_vldInvalidAnniversaryDate %>"></asp:CustomValidator>
                            <asp:HiddenField ID="hfOriginalAnniversaryDate" runat="server"></asp:HiddenField>
                        </div>
                        <asp:UpdatePanel ID="updPaymentMethod" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblPaymentMethod" runat="server" AssociatedControlID="POLICYHEADER__PAYMENTMETHOD" Text="<%$ Resources:lblPaymentMethod %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <asp:DropDownList ID="POLICYHEADER__PAYMENTMETHOD" TabIndex="5" CssClass=" form-control form-select" runat="server" EnableViewState="true" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvPaymentMethod" runat="server" ErrorMessage="<%$ Resources:vld_PaymentMethod %>" SetFocusOnError="true" Enabled="false" Display="None" ControlToValidate="POLICYHEADER__PAYMENTMETHOD"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblCollectionFrequency" runat="server" AssociatedControlID="POLICYHEADER__COLLECTIONFREQUENCY" Text="<%$ Resources:lblCollectionFrequency %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <NexusProvider:LookupList ID="POLICYHEADER__COLLECTIONFREQUENCY" runat="server" DataItemValue="Key" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="CollectionFrequency" CssClass=" form-control" DefaultText="<%$ Resources:lblDefaultText %>"></NexusProvider:LookupList>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvCollectionFrequency" runat="server" ErrorMessage="<%$ Resources:vld_CollectionFrequency %>" SetFocusOnError="true" Enabled="false" Display="None" ControlToValidate="POLICYHEADER__COLLECTIONFREQUENCY"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                    <asp:Label ID="lblPaymentTerm" runat="server" AssociatedControlID="POLICYHEADER__PAYMENTTERM" Text="<%$ Resources:lblPaymentTerm %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                                    <div class="col-md-8 col-sm-9">
                                        <NexusProvider:LookupList ID="POLICYHEADER__PAYMENTTERM" runat="server" DataItemValue="Key" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="DOPaymentTerms" CssClass=" form-control" DefaultText="<%$ Resources:lblDefaultText %>"></NexusProvider:LookupList>
                                    </div>
                                    <asp:RequiredFieldValidator ID="rfvPaymentTerm" runat="server" ErrorMessage="<%$ Resources:vld_PaymentTerm %>" SetFocusOnError="true" Enabled="false" Display="None" ControlToValidate="POLICYHEADER__PAYMENTTERM"></asp:RequiredFieldValidator>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                            </asp:Panel>
                <asp:UpdatePanel ID="updCoverNote" runat="server">
                    <ContentTemplate>
                            <asp:Panel ID="POLICYHEADER__COVERNOTEPANEL" runat="server">
                    <div class="form-horizontal">
                                    <legend>
                            <asp:Label ID="lblCoverNoteHeading" runat="server" Text="<%$ Resources:lblCoverNoteHeading %>"></asp:Label>
                                    </legend>
                         
                                <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblIsCoverNote" runat="server" AssociatedControlID="chkIsCoverNoteUsed" Text="<%$ Resources:lblIsCoverNote %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:CheckBox ID="chkIsCoverNoteUsed" runat="server" AutoPostBack="true" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblCoverNoteNumber" runat="server" AssociatedControlID="POLICYHEADER__COVERNOTESHEETNO" Text="<%$ Resources:lblCoverNoteNumber %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="POLICYHEADER__COVERNOTESHEETNO" runat="server" CssClass=" form-control form-select"></asp:DropDownList>
                        </div>
                            <asp:RequiredFieldValidator ID="vldrqdCoverNoteSheetNumber" runat="server" ErrorMessage="Please Select the Cover Note Sheet No" SetFocusOnError="true" Enabled="false" Display="None" ControlToValidate="POLICYHEADER__COVERNOTESHEETNO" InitialValue="<%$ Resources:lblDefaultText %>"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtCoverNoteSheetNo" runat="server" Visible="false" CssClass="" ReadOnly="true"></asp:TextBox>
                    </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblCoverNoteBookNo" runat="server" AssociatedControlID="POLICYHEADER__COVERNOTEBOOKNO" Text="<%$ Resources:lblCoverNoteBookNo %>" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:DropDownList ID="POLICYHEADER__COVERNOTEBOOKNO" AutoPostBack="true" runat="server" CssClass=" form-control form-select"></asp:DropDownList>
                </div>
                            <asp:TextBox ID="txtCoverNoteBookNo" runat="server" Visible="false" CssClass="" ReadOnly="true"></asp:TextBox>
                    </div>
                        <asp:HiddenField ID="hfDefaultCoverToDateToLastDay" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hfAgencyCancellationDate" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hfDateToValidateForAgencyCancellation" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hfRenewalFrequency" runat="server" />

                </div>
                    
                </asp:Panel> 
                    </ContentTemplate>
                </asp:UpdatePanel>
                <ucPA:PolicyAssociates ID="ctrlPolicyAssociates" runat="server" Visible="false" />
            </div>
        </asp:Panel>
        <div class="card-footer no-padding-h">
            <asp:LinkButton ID="btnCancelMTA" runat="server" Text="<%$ Resources:btnCancelMTA %>" CausesValidation="false" Visible="false" SkinID="btnSecondary"></asp:LinkButton>
            <asp:LinkButton ID="btnSaveQuote" runat="server" Text="<%$ Resources:btnSaveQuote %>" SkinID="btnPrimary"></asp:LinkButton>
            <asp:LinkButton ID="btnNext" OnClientClick="return ConfirmCoverEndDate();" runat="server" Text="<%$ Resources:btnNext %>" SkinID="btnPrimary"></asp:LinkButton>
        </div>
    </div>

    <asp:ValidationSummary ID="vldSummaryMainDetails" runat="server" HeaderText="<%$ Resources:lbl_ValidationSummary %>" CssClass="validation-summary"></asp:ValidationSummary>
    <asp:HiddenField ID="hfIsPolicyInRenewal" runat="server" />
    <asp:HiddenField ID="hfDoNotDeleteRenewalQuoteOnMTA" runat="server" />
    <asp:HiddenField ID="hfDeletePolicyFromRenewal" runat="server" />
    <asp:HiddenField ID="hfRenewalVersionStartDate" runat="server" />
    <asp:HiddenField ID="hfRequiredCoverEndDate" runat="server" />
    <asp:HiddenField ID="hfRenewalInsuranceFileKey" runat="server" />
    <asp:HiddenField ID="hfShowCurrencyModal" runat="server" Value="0" />
</asp:Content>
