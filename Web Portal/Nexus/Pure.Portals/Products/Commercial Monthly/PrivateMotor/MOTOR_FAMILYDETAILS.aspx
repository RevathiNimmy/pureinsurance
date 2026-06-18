<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master" MaintainScrollPositionOnPostback="true"
    CodeFile="MOTOR_FAMILYDETAILS.aspx.vb" Inherits="Nexus.MOTOR_FAMILYDETAILS" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="NexusControl" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/Controls/AddressCntrl.ascx" TagName="AddressControl" TagPrefix="uc1" %>
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
    </script>
    <div class="risk-screen">
        <div id="divMain" onscroll="SetDivPosition()">
            <NexusControl:ProgressBar ID="ucProgressBar" runat="server"></NexusControl:ProgressBar>
            <div class="card">
                <Nexus:tabindex id="ctrlTabIndex" runat="server" cssclass="TabContainer" tabcontainerclass="TabNav" activetabclass="ActiveTab" disabledclass="DisabledTab" scrollable="false"></Nexus:tabindex>
                <div class="card-body clearfix">
                    <div class="form-horizontal">
                        <legend><span>FamilyDetails</span></legend>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                Previous Insurer Name</label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="VEHDET__DD_PREV_INS_NAME" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                Previous Policy No.</label><div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="VEHDET__DD_PREV_POL_NO" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-horizontal">
                        <legend><span>MembersDetials</span></legend>
                        <div class="grid-card table-responsive">
                            <Nexus:itemgrid id="MS__DRIVER" runat="server" screencode="DRIVERDETA" autogeneratecolumns="false" gridlines="None" childpage="Child/MEMDETAILS_GENERAL.aspx">
                                <columns><nexus:riskattribute headertext="Name" datafield="DD_NAME"></nexus:riskattribute></columns>
                                <columns><nexus:riskattribute headertext="Date of Birth" datafield="DD_DOB"></nexus:riskattribute></columns>
                                <columns><nexus:riskattribute headertext="Licence Number" datafield="DD_DRILIC_NO"></nexus:riskattribute></columns>
                            </Nexus:itemgrid>
                        </div>
                    </div>
                </div>
                <div class='card-footer'>
                    <asp:LinkButton ID="btnBack" runat="server" Text="<i class='fa fa-chevron-left' aria-hidden='true'></i> Back" OnClick="BackButton" SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton ID="btnNext" runat="server" Text="Next <i class='fa fa-chevron-right' aria-hidden='true'></i>" OnClick="NextButton" SkinID="btnPrimary"></asp:LinkButton>
                    <asp:LinkButton ID="btnFinish" runat="server" Text="<i class='fa fa-check' aria-hidden='true'></i> Finish" OnClick="FinishButton" OnPreRender="PreRenderFinish" SkinID="btnPrimary"></asp:LinkButton>
                </div>
            </div>
        </div>
        <asp:ValidationSummary ID="vldSummaryMainDetails" runat="server" HeaderText="Validation" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
