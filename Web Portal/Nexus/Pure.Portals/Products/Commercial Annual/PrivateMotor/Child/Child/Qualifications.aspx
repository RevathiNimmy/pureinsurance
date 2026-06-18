<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master"
    MaintainScrollPositionOnPostback="true" CodeFile="Qualifications.aspx.vb" Inherits="Nexus.Qualifications" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="NexusControl" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/Controls/AddressCntrl.ascx" TagName="AddressControl" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/StandardWordings.ascx" TagName="StandardWording" TagPrefix="NexusControl" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        window.onload = function () {
            var strCook = document.cookie;
            if (strCook.indexOf("!~") != 0) {
                var intS = strCook.indexOf("!~");
                var intE = strCook.indexOf("~!");
                var strPos = strCook.substring(intS + 2, intE);
                document.getElementById("divMain").scrollTop = strPos;
            }
        }
        function SetDivPosition() {
            var intY = document.getElementById("divMain").scrollTop;
            document.cookie = "yPos=!~" + intY + "~!";
        }

        function yearValidation(year, ev) {

            var text = /^[0-9]+$/;
            if (ev.type == "blur" || year.length == 4 && ev.keyCode != 8 && ev.keyCode != 46) {
                if (year != 0) {
                    if ((year != "") && (!text.test(year))) {

                        //alert("Please Enter Numeric Values Only");
                        return false;
                    }

                    if (year.length != 4) {
                        //alert("Year is not proper. Please check");
                        return false;
                    }
                    var current_year = new Date().getFullYear();

                    return true;
                }
            }
        }

    </script>

    <asp:ScriptManager ID="ScriptManagerMainDetails" runat="server"></asp:ScriptManager>
    <div class="risk-screen">
        <div id="divMain" onscroll="SetDivPosition()">
            <NexusControl:ProgressBar ID="ucProgressBar" runat="server"></NexusControl:ProgressBar>
            <div class="card">
                <Nexus:tabindex id="ctrlTabIndex" runat="server" cssclass="TabContainer" tabcontainerclass="TabNav" activetabclass="ActiveTab" disabledclass="DisabledTab" scrollable="false"></Nexus:tabindex>
                <div class="card-body clearfix">
                    <legend>Course Details </legend>
                    <div class="form-horizontal">

                        <%--<fieldset>
                                <ol>
                                    <label>
                                        Address</label>
                                    <uc1:AddressControl runat="server" ID="QUALFICATIONS__ADDRESS_CNT" />
                                </ol>
                            </fieldset>--%>

                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                PassingYear</label><div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="QUALFICATIONS__QUAL_PASSING_YEAR" runat="server" MaxLength="4" onkeypress="if ( isNaN( String.fromCharCode(event.keyCode) )) return false;" CssClass="field-mandatory form-control"></asp:TextBox></div>
                        </div>

                        <asp:RequiredFieldValidator ID="rqdPassingYear" runat="server" ControlToValidate="QUALFICATIONS__QUAL_PASSING_YEAR" Display="none" Enabled="true" SetFocusOnError="true" ErrorMessage="Passing Year is a mandatory field."></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegDOB" runat="server" ControlToValidate="QUALFICATIONS__QUAL_PASSING_YEAR" Display="None" ErrorMessage="Please provide a Valid Passing Year." ValidationExpression="^[12][0-9]{3}$"></asp:RegularExpressionValidator>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                Course Name</label><div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="QUALFICATIONS__QUAL_COURSE_NAME" runat="server" CssClass="field-mandatory form-control"></asp:TextBox></div>
                            <asp:RequiredFieldValidator ID="rfvCourseName" runat="server" ControlToValidate="QUALFICATIONS__QUAL_COURSE_NAME" Display="none" Enabled="true" SetFocusOnError="true" ErrorMessage="Course Name is a mandatory field."></asp:RequiredFieldValidator>

                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                Area</label>
                            <%--<NexusProvider:LookupList ID="QUALFICATIONS__QUAL_AREA" runat="server" DataItemText="Description"
                                            DataItemValue="Key" ListType="PMLookup" ListCode="AREA" DefaultText="(Please Select)" />
                            --%>

                            <div class="col-md-8 col-sm-9">
                                <NexusProvider:lookuplist id="QUALFICATIONS__QUAL_AREA" runat="server" dataitemtext="Description" dataitemvalue="Key" listtype="PMLookup" listcode="AREA" defaulttext="(Please Select)" cssclass="field-mandatory form-control"></NexusProvider:lookuplist></div>
                            <asp:RequiredFieldValidator ID="rfvQualArea" runat="server" ControlToValidate="QUALFICATIONS__QUAL_AREA" Display="none" Enabled="true" SetFocusOnError="true" ErrorMessage="Area is a mandatory field." InitialValue=""></asp:RequiredFieldValidator>

                        </div>
                        <%--<li>
                                        <label>
                                            Certificate Type</label><NexusProvider:LookupList ID="QUALFICATIONS__CERTIFICATE_TYPE"
                                                runat="server" ListType="UserDefined" ListCode="S4ICERTTYP" ParentLookupListID=""
                                                DataItemValue="Key" DataItemText="Description" />
                                    </li>--%>
                    </div>
                    <legend>Endorsement</legend>
                    <NexusControl:standardwording runat="server" id="QUALFICATIONS__ENDORSEMENT" supportrisklevel="true"></NexusControl:standardwording>
                </div>
                <div class='card-footer'>
                    <asp:LinkButton ID="btnBack" runat="server" Text="<i class='fa fa-chevron-left' aria-hidden='true'></i> Back" OnClick="BackButton" SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton ID="btnNext" runat="server" Text="Next <i class='fa fa-chevron-right' aria-hidden='true'></i>" OnClick="NextButton" SkinID="btnPrimary"></asp:LinkButton>
                    <asp:LinkButton ID="btnFinish" runat="server" Text="<i class='fa fa-check' aria-hidden='true'></i> Finish" OnClick="FinishButton" OnPreRender="PreRenderFinish" SkinID="btnPrimary"></asp:LinkButton>
                </div>
            </div>
            <asp:ValidationSummary ID="vldSummaryMainDetails" runat="server" HeaderText="Validation" CssClass="validation-summary"></asp:ValidationSummary>
        </div>
    </div>
</asp:Content>
