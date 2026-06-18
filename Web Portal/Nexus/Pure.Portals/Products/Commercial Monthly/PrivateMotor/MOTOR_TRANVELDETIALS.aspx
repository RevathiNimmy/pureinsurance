<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master"
    MaintainScrollPositionOnPostback="true" CodeFile="MOTOR_TRANVELDETIALS.aspx.vb"
    Inherits="Nexus.MOTOR_TRANVELDETIALS" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="NexusControl" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
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
                        <legend><span>Previous Insurer</span></legend>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                Previous Insurer Name</label><div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="PREVINSURER__PREVINSURERNAME" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                Previous Policy No</label><div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="PREVINSURER__PREVPOLNO" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                        </div>
                    </div>
                    <legend>Driver Detail</legend>
                    <div class="grid-card table-responsive">
                        <Nexus:itemgrid id="VEHDET__DRIVERDET" runat="server" childcontainer="VEHDET__DRIVERDET__CHILDSCREEN" autogeneratecolumns="false" gridlines="None" width="100%" screencode="DRIDET&#8221;">
                            <headerstyle horizontalalign="Left"></headerstyle>
                            <columns>
                                        <nexus:riskattribute datafield="NAME" headertext="Name"></nexus:riskattribute>
                                        <nexus:riskattribute datafield="DOB" headertext="Date Of Birth" htmlencode="false" dataformatstring="{0:dd/MM/yyyy}"></nexus:riskattribute>
                                        <nexus:riskattribute datafield="DriLicenceNo" headertext="Licence Number" htmlencode="false"></nexus:riskattribute>                                             
                                    </columns>
                        </Nexus:itemgrid>
                    </div>
                    <Nexus:riskcontainer id="VEHDET__DRIVERDET__CHILDSCREEN" runat="server" visible="True" screencode="DRIDET">
                        <div class="form-horizontal">
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <label class="col-md-4 col-sm-3 control-label">
                                    Name</label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="DRIVERDET__NAME" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <label class="col-md-4 col-sm-3 control-label">
                                    DOB</label><div class="col-md-8 col-sm-9">
                                        <div class="input-group">
                                            <asp:TextBox ID="DRIVERDET__DOB" runat="server" CssClass="form-control"></asp:TextBox><NexusControl:calendarlookup id="calDRIVERDET__DOB" runat="server" linkedcontrol="DRIVERDET__DOB" hlevel="2"></NexusControl:calendarlookup>
                                        </div>
                                    </div>

                            </div>
                            <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                                <label class="col-md-4 col-sm-3 control-label">
                                    DriLicenceNo</label><div class="col-md-8 col-sm-9">
                                        <asp:TextBox ID="DRIVERDET__DRILICENCENO" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                            </div>
                        </div>
                    </Nexus:riskcontainer>
                    <div class="card-footer">
                        <asp:LinkButton ID="btnBack" runat="server" Text="<i class='fa fa-chevron-left' aria-hidden='true'></i> Back" OnClick="BackButton" SkinID="btnSecondary"></asp:LinkButton>
                        <asp:LinkButton ID="btnNext" runat="server" Text="Next <i class='fa fa-chevron-right' aria-hidden='true'></i>" OnClick="NextButton" SkinID="btnPrimary"></asp:LinkButton>
                        <asp:LinkButton ID="btnFinish" runat="server" Text="<i class='fa fa-check' aria-hidden='true'></i> Finish" OnClick="FinishButton" OnPreRender="PreRenderFinish" SkinID="btnPrimary"></asp:LinkButton>
                    </div>
                </div>
                <div class='card-footer'>
                    <asp:LinkButton ID="btnAdd" CommandArgument="VEHDET__DRIVERDET__CHILDSCREEN" runat="server" Text="Add" OnClick="SaveChildButton" UseSubmitBehavior="true" SkinID="btnSecondary"></asp:LinkButton>
                    <asp:LinkButton ID="BtnCancel" runat="server" Text="Cancel" OnClick="CancelChildButton" SkinID="btnSecondary"></asp:LinkButton>
                </div>
            </div>
            <asp:ValidationSummary ID="vldSummaryMainDetails" runat="server" HeaderText="Validation" CssClass="validation-summary"></asp:ValidationSummary>
        </div>
    </div>
</asp:Content>
