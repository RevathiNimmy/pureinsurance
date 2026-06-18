<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master" CodeFile="FIRE_RISKSELECTION.aspx.vb" Inherits="Nexus.FIRE_RISKSELECTION" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/AddressCntrl.ascx" TagName="AddressCntrl" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/StandardWordings.ascx" TagName="StandardWording" TagPrefix="NexusControl" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {

            document.getElementById("<%= GROUP_FIRE__LAST_SURVEY_DATE.ClientID%>").readOnly = true;
            $(".ui-datepicker-trigger").each(function () { $(this).hide(); });

            document.getElementById("<%= GROUP_FIRE__DATE_SURVEY_REQUESTED.ClientID%>").readOnly = true;
            document.getElementById("<%= GROUP_FIRE__SURVEY_NUMBER.ClientID%>").readOnly = true;
            $(document.getElementById("<%= GROUP_FIRE__FREQUENCY.ClientID%>")).attr("disabled", true);
            if (document.getElementById("<%= GROUP_FIRE__FIRE.ClientID%>").checked || document.getElementById("<%= GROUP_FIRE__OFFICE_CONTENTS.ClientID%>").checked) {

                document.getElementById("<%= txtRiskSelectionHidden.ClientID%>").value = "1";
            }
            else {
                document.getElementById("<%= txtRiskSelectionHidden.ClientID%>").value = "";
            }
            ToggleSurveyReport(document.getElementById("<%= GROUP_FIRE__APPLICABLE.ClientID%>").checked);
            ValidatorEnable($("#<%= rfvSurveyDate.ClientID%>")[0], false);
        });
        function SetRiskSelection() {
            if (document.getElementById("<%= GROUP_FIRE__FIRE.ClientID%>").checked || document.getElementById("<%= GROUP_FIRE__OFFICE_CONTENTS.ClientID%>").checked) {

                document.getElementById("<%= txtRiskSelectionHidden.ClientID%>").value = "1";
            }
            else {
                document.getElementById("<%= txtRiskSelectionHidden.ClientID%>").value = "";
            }
        }
        function ToggleSurveyReport(flag) {
            document.getElementById("<%= GROUP_FIRE__LAST_SURVEY_DATE.ClientID%>").readOnly = !flag;
            document.getElementById("<%= GROUP_FIRE__DATE_SURVEY_REQUESTED.ClientID%>").readOnly = !flag;
            document.getElementById("<%= GROUP_FIRE__SURVEY_NUMBER.ClientID%>").readOnly = !flag;
            $(document.getElementById("<%= GROUP_FIRE__FREQUENCY.ClientID%>")).attr("disabled", !flag);
            ValidatorEnable($("#<%= rfvSurveyDate.ClientID%>")[0], flag);

            if (flag) {
                document.getElementById("<%= GROUP_FIRE__DATE_SURVEY_REQUESTED.ClientID%>").className = "form-control field-mandatory";
                $(".ui-datepicker-trigger").each(function () { $(this).show(); });
            }
            else {
                document.getElementById("<%= GROUP_FIRE__DATE_SURVEY_REQUESTED.ClientID%>").className = "form-control";
                $(".ui-datepicker-trigger").each(function () { $(this).hide(); });
                document.getElementById("<%= GROUP_FIRE__LAST_SURVEY_DATE.ClientID%>").value = "";
                document.getElementById("<%= GROUP_FIRE__DATE_SURVEY_REQUESTED.ClientID%>").value = "";
                document.getElementById("<%= GROUP_FIRE__SURVEY_NUMBER.ClientID%>").value = "";
                document.getElementById("<%= GROUP_FIRE__FREQUENCY.ClientID%>").value = "";
            }
        }
        jQuery(function ($) {
            $('form').bind('submit', function () {
                $(this).find(':input').removeAttr('disabled');
            });
        });

    </script>

    <div class="risk-screen">
        <NexusControl:ProgressBar ID="ucProgressBar" runat="server"></NexusControl:ProgressBar>
        <div class="card">
            <Nexus:tabindex id="ctrlTabIndex" runat="server" cssclass="TabContainer" tabcontainerclass="page-progress" activetabclass="ActiveTab" disabledclass="DisabledTab" scrollable="false"></Nexus:tabindex>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <legend><span>Fire Sections</span></legend>


                    <div id="liGROUP_FIRE__FIRE" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Fire :</label><div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="GROUP_FIRE__FIRE" runat="server" onclick="SetRiskSelection();" Text=" " CssClass="asp-check"></asp:CheckBox></div>
                    </div>
                    <div id="liGROUP_FIRE__OFFICE_CONTENTS" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Office Contents :</label>
                        <div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="GROUP_FIRE__OFFICE_CONTENTS" runat="server" onclick="SetRiskSelection();" Text=" " CssClass="asp-check"></asp:CheckBox></div>
                    </div>
                    <asp:TextBox ID="txtRiskSelectionHidden" runat="server" Style="display: none;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvRiskSelection" runat="server" ControlToValidate="txtRiskSelectionHidden" Display="none" Enabled="true" SetFocusOnError="true" ErrorMessage="At least one Risk Selection is Mandatory."></asp:RequiredFieldValidator>
                    <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Extended Limits Applicable:</label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:lookuplist id="GROUP_FIRE__EX_LIMIT_APPL" runat="server" listtype="UserDefined" listcode="QUES" parentlookuplistid="" dataitemvalue="Key" dataitemtext="Description" sort="Desc" cssclass="form-control"></NexusProvider:lookuplist></div>
                    </div>
                </div>
                <div class="form-horizontal">
                    <legend><span>Premises Assessment</span></legend>

                    <div id="liGROUP_FIRE__INDUSTRYN" class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12" style="display: none;">
                        <asp:Label ID="Label1" runat="server" Text="Industry" class="col-md-4 col-sm-3 control-label"></asp:Label><div class="col-md-8 col-sm-9">
                            <NexusProvider:lookuplist id="GROUP_FIRE__INDUSTRYN" runat="server" listtype="UserDefined" listcode="INDUSTRY" parentlookuplistid="" dataitemvalue="Key" dataitemtext="Description" defaulttext="(None)" value="0" cssclass="form-control"></NexusProvider:lookuplist></div>
                    </div>

                    <div id="liGROUP_FIRE__MANUAL_RI_LIMIT_NO" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Manual Reinsurance Limit No</label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:lookuplist id="GROUP_FIRE__MANUAL_RI_LIMIT_NO" runat="server" listtype="PMLookup" listcode="UDL_EBPGF_RILIMITSNOS" defaulttext="(None)" value="0" parentlookuplistid="" cssclass="form-control"></NexusProvider:lookuplist></div>
                    </div>
                    <div id="liGROUP_FIRE__TYPE_CONSTRUCTION" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Type of Construction :</label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:lookuplist id="GROUP_FIRE__TYPE_CONSTRUCTION" runat="server" listtype="UserDefined" listcode="CONSTTYPE" parentlookuplistid="" dataitemvalue="Key" dataitemtext="Description" defaulttext="(None)" value="0" cssclass="field-mandatory form-control"></NexusProvider:lookuplist></div>
                        <asp:RequiredFieldValidator ID="RqdLookup_Construction" runat="server" ControlToValidate="GROUP_FIRE__TYPE_CONSTRUCTION" ErrorMessage="Type of Construction is a required field" InitialValue="" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                    </div>
                    <div id="liGROUP_FIRE__RILIMITNUM" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">RI Limit Number :</label>
                        <div class="col-md-8 col-sm-9">
                            <NexusProvider:lookuplist id="GROUP_FIRE__RILIMITNUM" runat="server" listtype="UserDefined" listcode="RILIMITNO" parentlookuplistid="GROUP_FIRE__INDUSTRYN" dataitemvalue="Key" dataitemtext="Description" defaulttext="(None)" value="0" cssclass="field-mandatory form-control"></NexusProvider:lookuplist></div>
                        <asp:RequiredFieldValidator ID="RqdLookup_RILimitNumber" runat="server" ControlToValidate="GROUP_FIRE__RILIMITNUM" ErrorMessage="RI Limit Number is a required field" InitialValue="" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                    </div>


                </div>
                <div class="form-horizontal">
                    <legend><span>Loading and Discount Criteria</span></legend>


                    <div id="liGROUP_FIRE__THREEYEARLOSS" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">3 Year Loss Ratio :</label><div class="col-md-8 col-sm-9">
                            <NexusProvider:lookuplist id="GROUP_FIRE__THREEYEARLOSS" runat="server" listtype="UserDefined" listcode="THREEYLR" parentlookuplistid="" dataitemvalue="Key" dataitemtext="Description" defaulttext="(None)" value="0" cssclass="field-medium form-control"></NexusProvider:lookuplist></div>

                    </div>

                    <div id="liGROUP_FIRE__BRANCH_MARKET_ADJUST" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Branch Market Adjustment :</label><div class="col-md-8 col-sm-9">
                            <NexusProvider:lookuplist id="GROUP_FIRE__BRANCH_MARKET_ADJUST" runat="server" listtype="PMLookup" listcode="UDL_EBPGFBMADJUST" defaulttext="(None)" value="0" cssclass="field-medium form-control"></NexusProvider:lookuplist></div>

                    </div>

                    <div id="liGROUP_FIRE__ADJUST_BRANCH_MKT" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">H/O Market Adjustment :</label><div class="col-md-8 col-sm-9">
                            <NexusProvider:lookuplist id="GROUP_FIRE__ADJUST_BRANCH_MKT" runat="server" listtype="PMLookup" listcode="UDL_EBPGFBMADJUST" defaulttext="(None)" value="0" cssclass="field-medium form-control"></NexusProvider:lookuplist></div>

                    </div>

                    <div id="liGROUP_FIRE__SPECIALS" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Special :</label><div class="col-md-8 col-sm-9">
                            <NexusProvider:lookuplist id="GROUP_FIRE__SPECIALS" runat="server" listtype="PMLookup" listcode="UDL_EBPGFBMADJUST" defaulttext="(None)" value="0" cssclass="field-medium form-control"></NexusProvider:lookuplist></div>

                    </div>

                </div>
                <div class="form-horizontal">
                    <legend><span>Survey Report</span></legend>

                    <div id="liGROUP_FIRE__APPLICABLE" class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Applicable</label><div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="GROUP_FIRE__APPLICABLE" runat="server" onclick="ToggleSurveyReport(this.checked);" Text=" " CssClass="asp-check"></asp:CheckBox></div>
                    </div>
                    <div id="liGROUP_FIRE__LAST_SURVEY_DATE" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Last Survey Date :</label><div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="GROUP_FIRE__LAST_SURVEY_DATE" runat="server" CssClass="form-control"></asp:TextBox><NexusControl:calendarlookup id="calGROUP_FIRE__LAST_SURVEY_DATE" runat="server" linkedcontrol="GROUP_FIRE__LAST_SURVEY_DATE" hlevel="1"></NexusControl:calendarlookup></div>
                        </div>

                        <asp:RegularExpressionValidator ID="regexLastSurveyDate" runat="server" ControlToValidate="GROUP_FIRE__LAST_SURVEY_DATE" Display="None" ErrorMessage="Invalid Format for Last Survey Date" ValidationExpression="^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$" SetFocusOnError="true"></asp:RegularExpressionValidator>
                    </div>
                    <div id="liGROUP_FIRE__SURVEY_NUMBER" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Survey Number :</label><div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="GROUP_FIRE__SURVEY_NUMBER" runat="server" CssClass="form-control"></asp:TextBox></div>
                    </div>
                    <div id="liGROUP_FIRE__DATE_SURVEY_REQUESTED" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Date Survey Requested :</label><div class="col-md-8 col-sm-9">
                            <div class="input-group">
                                <asp:TextBox ID="GROUP_FIRE__DATE_SURVEY_REQUESTED" runat="server" CssClass="form-control"></asp:TextBox><NexusControl:calendarlookup id="calGROUP_FIRE__DATE_SURVEY_REQUESTED" runat="server" linkedcontrol="GROUP_FIRE__DATE_SURVEY_REQUESTED" hlevel="1"></NexusControl:calendarlookup></div>
                        </div>

                        <asp:RequiredFieldValidator ID="rfvSurveyDate" runat="server" ControlToValidate="GROUP_FIRE__DATE_SURVEY_REQUESTED" ErrorMessage="Date Survey Requested is mandatory." Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="regexDateSurveyRequested" runat="server" ControlToValidate="GROUP_FIRE__DATE_SURVEY_REQUESTED" Display="None" ErrorMessage="Invalid Format for Date Survey Requested" ValidationExpression="^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$" SetFocusOnError="true"></asp:RegularExpressionValidator>
                    </div>

                    <div id="liGROUP_FIRE__FREQUENCY" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Frequency :</label><div class="col-md-8 col-sm-9">
                            <NexusProvider:lookuplist id="GROUP_FIRE__FREQUENCY" runat="server" listtype="PMLookup" listcode="UDL_EBPGFSRFREQ" defaulttext="(None)" value="0" cssclass="form-control"></NexusProvider:lookuplist></div>
                    </div>

                </div>
            </div>
            <div class='card-footer'>
                <asp:LinkButton ID="btnBack" runat="server" Text="<i class='fa fa-chevron-left' aria-hidden='true'></i> Back" OnClick="BackButton" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnNext" runat="server" Text="Next <i class='fa fa-chevron-right' aria-hidden='true'></i>" OnClick="NextButton" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnFinish" runat="server" Text="<i class='fa fa-check' aria-hidden='true'></i> Finish" OnClick="FinishButton" OnPreRender="PreRenderFinish" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:ValidationSummary ID="vldsumSummary" DisplayMode="BulletList" HeaderText="<h2>There are errors on this page</h2><p>Please review these errors and re-submit the form</p>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>

    <script language="javascript" type="text/javascript">

        HideTab("fire_firefiref");
        HideTab("fire_officecontents");

        if ($("#<%=GROUP_FIRE__FIRE.ClientID%>")[0].checked) { ShowTab("fire_firefiref"); }
            else { HideTab("fire_firefiref"); }

            if ($("#<%=GROUP_FIRE__OFFICE_CONTENTS.ClientID%>")[0].checked) { ShowTab("fire_officecontents"); }
        else { HideTab("fire_officecontents"); }

        $("#<%=GROUP_FIRE__FIRE.ClientID%>").click(function () {
            if ($(this)[0].checked) { ShowTab("fire_firefiref"); }
            else { HideTab("fire_firefiref"); }
        });
        $("#<%=GROUP_FIRE__OFFICE_CONTENTS.ClientID%>").click(function () {
            if ($(this)[0].checked) { ShowTab("fire_officecontents"); }
            else { HideTab("fire_officecontents"); }
        });


    </script>

</asp:Content>
