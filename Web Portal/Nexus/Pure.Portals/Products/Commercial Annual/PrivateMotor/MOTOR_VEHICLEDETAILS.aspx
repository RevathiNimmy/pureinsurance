<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master"
    MaintainScrollPositionOnPostback="true" CodeFile="MOTOR_VEHICLEDETAILS.aspx.vb"
    Inherits="Nexus.MOTOR_VEHICLEDETAILS" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="NexusControl" %>
<%@ Register Src="~/Controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="NexusControl" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<%@ Register Src="~/Controls/FileView.ascx" TagName="FileView" TagPrefix="uc13" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/AddressCntrl.ascx" TagName="AddressControl" TagPrefix="uc1" %>
<%@ Register TagPrefix="uc2" TagName="CalendarLookup" Src="~/Controls/CalendarLookup.ascx" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">

    <script language="javascript" type="text/javascript">
        window.onload = function () {
            //DisplayLiab();
            var strCook = document.cookie;
            if (strCook.indexOf("!~") != 0) {
                var intS = strCook.indexOf("!~");
                var intE = strCook.indexOf("~!");
                var strPos = strCook.substring(intS + 2, intE);
                document.getElementById("divMain").scrollTop = strPos;

            }
        }

        $(document).ready(function () {
            document.getElementById("<%= VEHDET__QUICK_QUOTE.ClientID%>").style.display = 'none';;
        });

        function SetRate() {
            if (document.getElementById("ctl00_cntMainBody_VEHDET__RATEN").value = "") {
                document.getElementById("ctl00_cntMainBody_VEHDET__RATEN").selectedIndex = "0";

            }
            else {
                document.getElementById("ctl00_cntMainBody_VEHDET__RATEN").selectedIndex = "1";

            }


        }
        function SetRateblank() {

            document.getElementById("ctl00_cntMainBody_VEHDET__RATEN").selectedIndex = "0";



        }
        function SetDivPosition() {
            var intY = document.getElementById("divMain").scrollTop;
            document.cookie = "yPos=!~" + intY + "~!";
        }

        //  function DisplayLiab() {
        //      if ($("#<%= VEHDET__OF_LIABILITY_COVER.ClientID %>")[0].checked == true) {
        //          ShowTab("6");
        //      }
        //      else {
        //          HideTab("6");
        //      }
        //  }

        function isInteger(e) {
            var key = window.event ? e.keyCode : e.which;
            var keychar = String.fromCharCode(key);
            var ValidChars = "0123456789.";
            var IsNumber = true;
            if (ValidChars.indexOf(keychar) == -1) {
                IsNumber = false;
            }
            return IsNumber;
        }


    </script>



    <cc1:ToolkitScriptManager runat="server">
    </cc1:ToolkitScriptManager>
    <div class="risk-screen">
        <div id="divMain" onscroll="SetDivPosition()">
            <NexusControl:ProgressBar ID="ucProgressBar" runat="server"></NexusControl:ProgressBar>
            <div class="card">
                <nexus:tabindex id="ctrlTabIndex" runat="server" cssclass="TabContainer" tabcontainerclass="TabNav" activetabclass="ActiveTab" disabledclass="DisabledTab" scrollable="false"></nexus:tabindex>
                <div class="card-body clearfix">
                    <div class="form-horizontal">
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                Make</label><div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="VEHDET__MAKE" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                Model</label><div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="VEHDET__MODEL" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                CC</label><div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="VEHDET__CC" runat="server" onkeypress="return isInteger(event); " CssClass="form-control"></asp:TextBox>
                                </div>
                        </div>

                        <asp:TextBox ID="VEHDET__QUICK_QUOTE" runat="server" Text="0" CssClass="form-control"></asp:TextBox>

                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                Seating Capacity</label><div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="VEHDET__SEATCAP" runat="server" CssClass="e-num2 form-control"></asp:TextBox>
                                </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label"></label>
                            <div class="col-md-8 col-sm-9">
                                <nexus:FindControl ID="fcModel" runat="server" FindControlKey="2" MappedControls="VEHDET__MAKE;VEHDET__MODEL;VEHDET__CC;VEHDET__SEATCAP" ShowHeader="true"></nexus:FindControl>
                            </div>
                        </div>
                    </div>
                    <div class="form-horizontal">
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">Sum Insured</label>
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="VEHDET__SI" runat="server" onkeypress="if ( isNaN( String.fromCharCode(event.keyCode) )) return false;" CssClass="field-mandatory form-control"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="vldrqdSumInsured" runat="server" ControlToValidate="VEHDET__SI" Display="none" Enabled="true" SetFocusOnError="true" ErrorMessage="Sum Insured is a mandatory field."></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">Type of Vehicle</label>
                            <div class="col-md-8 col-sm-9">
                                <NexusProvider:LookupList ID="VEHDET__TYP_VEHICLE" runat="server" ListType="UserDefined" ListCode="TOV" ParentLookupListID="" DataItemValue="Key" DataItemText="Description" DefaultText="(Please Select)" onclick=" SetRateblank();" CssClass="field-mandatory form-control"></NexusProvider:LookupList>
                            </div>
                            <asp:RequiredFieldValidator ID="VEHDETVehicleReq" runat="server" ControlToValidate="VEHDET__TYP_VEHICLE" Display="none" Enabled="true" SetFocusOnError="true" ErrorMessage="Type of Vehicle is a mandatory field."></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">Class of Use</label>
                            <div class="col-md-8 col-sm-9">
                                <NexusProvider:LookupList ID="VEHDET__CLAS_USE" runat="server" ListType="UserDefined" ListCode="COU" ParentLookupListID="VEHDET__TYP_VEHICLE" DataItemValue="Key" DataItemText="Description" DefaultText="(Please Select)" CssClass="field-mandatory form-control"></NexusProvider:LookupList>
                            </div>
                            <asp:RequiredFieldValidator ID="rfvVehClassUse" runat="server" ControlToValidate="VEHDET__CLAS_USE" Display="none" Enabled="true" SetFocusOnError="true" ErrorMessage="Class of use is a mandatory field."></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">Retroactive Date</label>
                            <div class="col-md-8 col-sm-9">
                                <div class="input-group">
                                    <asp:TextBox ID="S4IDEFAULT__RETROACTIVE_DATE" runat="server" CssClass="form-control"></asp:TextBox><uc2:CalendarLookup ID="CoverStart_uctCalenderlookup4" runat="server" LinkedControl="S4IDEFAULT__RETROACTIVE_DATE" HLevel="1"></uc2:CalendarLookup>
                                </div>
                            </div>

                            <asp:RegularExpressionValidator ID="RegRetroactiveDate" runat="server" EnableClientScript="false" ControlToValidate="S4IDEFAULT__RETROACTIVE_DATE" Display="None" ErrorMessage="Please provide a Valid Retroactive Date." ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">Rate</label>
                            <div class="col-md-8 col-sm-9">
                                <NexusProvider:LookupList ID="VEHDET__RATEN" runat="server" ListType="UserDefined" ListCode="RATE" ParentLookupListID="VEHDET__CLAS_USE" DataItemValue="Key" DataItemText="Description" DefaultText="None" CssClass="field-mandatory form-control"></NexusProvider:LookupList>
                            </div>
                            <asp:RequiredFieldValidator ID="rfvVehDetRate" runat="server" ControlToValidate="VEHDET__CLAS_USE" Display="none" Enabled="true" SetFocusOnError="true" ErrorMessage="Rate is a mandatory field."></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <div class="col-md-8 col-sm-9">
                                <asp:TextBox ID="MS__NUM_SEATS" runat="server" Visible="False" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-horizontal">
                        <legend>
                            <asp:Label ID="Label3" runat="server" Text="Options"></asp:Label>
                        </legend>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblLiabilityCover" runat="server" Text="Liability Cover" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <asp:CheckBox ID="VEHDET__OF_LIABILITY_COVER" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </div>
                        </div>
                    </div>
                    <%--New Frame Introduced End  --%>
                    <div class="form-horizontal">
                        <legend>
                            <asp:Label ID="Label1" runat="server" Text="Client Details"></asp:Label>
                        </legend>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                Company Name</label><div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="CLIENTDETAILS__COMPANY_NAME" runat="server" CssClass="field-mandatory form-control"></asp:TextBox>
                                </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                Telephone</label><div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="CLIENTDETAILS__TELEPHONE" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                Email</label><div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="CLIENTDETAILS__E_MAIL" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            <asp:RegularExpressionValidator runat="server" ID="regEmail" EnableClientScript="false" ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,])*)*" ControlToValidate="CLIENTDETAILS__E_MAIL" Display="None" ErrorMessage="Please Provide a Valid Email ID." SetFocusOnError="true"></asp:RegularExpressionValidator>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                Address Line 1</label><div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="CLIENTDETAILS__ADDRESS_LINE1" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                Address Line 2</label><div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="CLIENTDETAILS__ADDRESS_LINE2" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                Address Line 3</label><div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="CLIENTDETAILS__ADDRESS_LINE3" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">

                            <asp:Label ID="lblCountry" runat="server" AssociatedControlID="CLIENTDETAILS__COUNTRYCODE" Text="Country" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <NexusProvider:LookupList ID="CLIENTDETAILS__COUNTRYCODE" runat="server" DataItemValue="Key" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="COUNTRY" DefaultText="(Please Select)" CssClass="field-mandatory form-control" AutoPostBack="true"></NexusProvider:LookupList>
                            </div>

                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <asp:Label ID="lblState" runat="server" AssociatedControlID="CLIENTDETAILS__ADDRESS_LINE4" Text="State" class="col-md-4 col-sm-3 control-label"></asp:Label>
                            <div class="col-md-8 col-sm-9">
                                <NexusProvider:LookupList ID="CLIENTDETAILS__ADDRESS_LINE4" runat="server" DataItemValue="Key" DataItemText="Description" Sort="ASC" ListType="PMLookup" ListCode="STATE" CssClass="field-medium form-control" ParentLookupListID="CLIENTDETAILS__COUNTRYCODE" ParentFieldName="COUNTRY_ID"></NexusProvider:LookupList>
                            </div>
                        </div>
                        <div class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                            <label class="col-md-4 col-sm-3 control-label">
                                PostCode</label><div class="col-md-8 col-sm-9">
                                    <asp:TextBox ID="CLIENTDETAILS__POSTCODE" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                        </div>
                    </div>
                    <div class="p-v-sm text-right" style="display:none;">
                        <asp:HyperLink ID="HyperLink1" SkinID="btnHSM" runat="server"><i class="fa fa-external-link" aria-hidden="true"></i> Go to DME Page</asp:HyperLink>
                    </div>
                    <legend><span>Document Details</span></legend>
                    <uc13:FileView ID="cnt_FileView" runat="server" EnableFileUpload="true"></uc13:FileView>
                </div>
                <div class='card-footer'>
                    <asp:LinkButton ID="btnNext" runat="server" Text="Next <i class='fa fa-chevron-right' aria-hidden='true'></i>" OnClick="NextButton" SkinID="btnPrimary"></asp:LinkButton>
                    <asp:LinkButton ID="btnFinish" runat="server" Text="<i class='fa fa-check' aria-hidden='true'></i> Finish" OnClick="FinishButton" OnPreRender="PreRenderFinish" SkinID="btnPrimary"></asp:LinkButton>
                </div>
            </div>
            <asp:ValidationSummary ID="vldSummaryMainDetails" runat="server" HeaderText="Validation" CssClass="validation-summary"></asp:ValidationSummary>
        </div>
    </div>


    <script language="javascript" type="text/javascript">
        HideTab("6");
        if ($("#<%=VEHDET__OF_LIABILITY_COVER.ClientID%>")[0].checked) { ShowTab("6"); }
        else { HideTab("6"); }
        $("#<%=VEHDET__OF_LIABILITY_COVER.ClientID%>").click(function () {
            if ($(this)[0].checked) { ShowTab("6"); }
            else { HideTab("6"); }
        });
    </script>

</asp:Content>
