<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master" MaintainScrollPositionOnPostback="true"
    CodeFile="MEMDETAILS_GENERAL.aspx.vb" Inherits="Nexus.MEMDETAILS_GENERAL" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="NexusControl" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/Controls/AddressCntrl.ascx" TagName="AddressControl" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/StandardWordings.ascx" TagName="StandardWording" TagPrefix="UC3" %>
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

        function isValidDate(subject) {
            if (subject.match(/^(?:(0[1-9]|1[012])[\- \/.](0[1-9]|[12][0-9]|3[01])[\- \/.](19|20)[0-9]{2})$/)) {
                return true;
            } else {
                return false;
            }
        }

    </script>
    <asp:ScriptManager ID="ScriptManagerMainDetails" runat="server"></asp:ScriptManager>
    <div class="risk-screen">
        <div class="nexus-fluid-layout" id="divMain" onscroll="SetDivPosition()">
            <NexusControl:ProgressBar ID="ucProgressBar" runat="server"></NexusControl:ProgressBar>
            <div class="card">
                <Nexus:tabindex id="ctrlTabIndex" runat="server" cssclass="TabContainer" tabcontainerclass="TabNav" activetabclass="ActiveTab" disabledclass="DisabledTab" scrollable="false"></Nexus:tabindex>
                <div class="card-body clearfix">
                    <div class="form-horizontal">
                        <legend><span>Driver Details </span></legend>

                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                Name:</label><div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="DRIVER__DD_NAME" runat="server" classname="field-medium field-mandatory" MaxLength="255" CssClass="form-control"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="rfv_DRIVERDET__NAME" runat="server" ControlToValidate="DRIVER__DD_NAME" ErrorMessage="Driver name is required " SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                DOB:</label><div class="col-md-8 col-sm-9">
                                    <div class="input-group">
                                        <asp:TextBox ID="DRIVER__DD_DOB" runat="server" classname="field-medium field-mandatory" CssClass="form-control"></asp:TextBox><NexusControl:calendarlookup id="uctCalenderlookupDOB" runat="server" linkedcontrol="DRIVER__DD_DOB" hlevel="1"></NexusControl:calendarlookup>
                                    </div>
                                </div>

                            <asp:RequiredFieldValidator ID="rfv_DRIVERDET__DOB" runat="server" ControlToValidate="DRIVER__DD_DOB" ErrorMessage="Driver's DOB is required " SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegDOB" runat="server" ControlToValidate="DRIVER__DD_DOB" Display="None" ErrorMessage="Please provide a Valid Date Of Birth." ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                DriLicenceNo
                            </label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="DRIVER__DD_DRILIC_NO" runat="server" classname="field-medium field-mandatory" CssClass="form-control"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="rfv_DRIVERDET__DRILICENCENO" runat="server" ControlToValidate="DRIVER__DD_DRILIC_NO" ErrorMessage="Driver's Licvence number is required " SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                        </div>

                       

                    </div>
					<UC3:standardwording runat="server" id="DRIVER__ENDORSEMENT" supportrisklevel="true"></UC3:standardwording>
                    <legend><span>Advanced Driving Course</span></legend>
                    <div class="grid-card table-responsive">
                        <Nexus:itemgrid id="DRIVER__QUALFICATIONS" runat="server" screencode="QUALIFA" autogeneratecolumns="false" gridlines="None" childpage="Child/Qualifications.aspx">
                            <columns><nexus:riskattribute headertext="Passing Year" datafield="QUAL_PASSING_YEAR"></nexus:riskattribute></columns>
                            <columns><nexus:riskattribute headertext="Course Name" datafield="QUAL_COURSE_NAME"></nexus:riskattribute></columns>
                        </Nexus:itemgrid>
                    </div>
					
                </div>
                <div class='card-footer'>
                    <asp:LinkButton ID="btnBack" runat="server" Text="<i class='fa fa-chevron-left' aria-hidden='true'></i> Back" OnClick="BackButton" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton ID="btnNext" runat="server" Text="Next <i class='fa fa-chevron-right' aria-hidden='true'></i>" OnClick="NextButton" SkinID="btnPrimary"></asp:LinkButton>
                    <asp:LinkButton ID="btnFinish" runat="server" Text="<i class='fa fa-check' aria-hidden='true'></i> Finish" OnClick="FinishButton" SkinID="btnPrimary"></asp:LinkButton>
                </div>
            </div>
            <asp:ValidationSummary ID="vldSummaryMainDetails" runat="server" HeaderText="Validation" CssClass="validation-summary"></asp:ValidationSummary>
        </div>
    </div>
	
	

</asp:Content>
