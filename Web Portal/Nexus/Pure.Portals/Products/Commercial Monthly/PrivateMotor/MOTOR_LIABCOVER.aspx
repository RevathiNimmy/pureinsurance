<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master" MaintainScrollPositionOnPostback="true"
    CodeFile="MOTOR_LIABCOVER.aspx.vb" Inherits="Nexus.MOTOR_FAMILYDETAILS" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="NexusControl" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>
<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <script language="javascript" type="text/javascript">
        window.onload = function () {
            Toggle(false, 'load');
            var strCook = document.cookie;
            if (strCook.indexOf("!~") != 0) {
                var intS = strCook.indexOf("!~");
                var intE = strCook.indexOf("~!");
                var strPos = strCook.substring(intS + 2, intE);
                document.getElementById("divMain").scrollTop = strPos;
            }
            CalcPremTPL();
            CalcPremCarHire();
            CalcPremContLiab();
            CalcPremShrtFall();
            CalcPremWreck();

        }
        function SetDivPosition() {
            var intY = document.getElementById("divMain").scrollTop;
            document.cookie = "yPos=!~" + intY + "~!";
        }


        function CalcPremTPL() {
            if (document.getElementById('<%=VEHDET__TPL_RATEN.ClientID%>').value >= 0 && document.getElementById('<%=VEHDET__TPL_RATEN.ClientID%>').value <= 100) {
                document.getElementById('<%=VEHDET__TPL_PREMIUM.ClientID%>').value = parseFloat(document.getElementById('<%=VEHDET__TPL_LIMIT_IDEMNITY.ClientID%>').value * document.getElementById('<%=VEHDET__TPL_RATEN.ClientID%>').value * .01).toFixed(2);
            }
            else {
                document.getElementById('<%=VEHDET__TPL_PREMIUM.ClientID%>').value = "";
            }
        }
        function CalcPremCarHire() {
            if (document.getElementById('<%=MS__DLC_CHR_RATEN.ClientID%>').value >= 0 && document.getElementById('<%=MS__DLC_CHR_RATEN.ClientID%>').value <= 100) {
                document.getElementById('<%=MS__DLC_CHR_PREM.ClientID%>').value =
                   parseFloat(document.getElementById('<%=MS__DLC_CHR_LIMIT_IDEMN.ClientID%>').value * document.getElementById('<%=MS__DLC_CHR_RATEN.ClientID%>').value * .01).toFixed(2);

            }
            else {
                document.getElementById('<%=MS__DLC_CHR_PREM.ClientID%>').value = "";

            }
        }
        function CalcPremContLiab() {
            if (document.getElementById('<%=MS__DLC_CL_RATEN.ClientID%>').value >= 0 && document.getElementById('<%=MS__DLC_CL_RATEN.ClientID%>').value <= 100) {
                document.getElementById('<%=MS__DLC_CL_PREM.ClientID%>').value =
               parseFloat(document.getElementById('<%=MS__DLC_CL_LIMIT_IDEMN.ClientID%>').value * document.getElementById('<%=MS__DLC_CL_RATEN.ClientID%>').value * .01).toFixed(2);
            }
            else { document.getElementById('<%=MS__DLC_CL_PREM.ClientID%>').value = ""; }
        }

        function CalcPremShrtFall() {
            if (document.getElementById('<%=MS__DLC_CR_RATEN.ClientID%>').value >= 0 && document.getElementById('<%=MS__DLC_CR_RATEN.ClientID%>').value <= 100) {
                document.getElementById('<%=MS__DLC_CR_PREM.ClientID%>').value =
               parseFloat(document.getElementById('<%=MS__DLC_CR_LIMIT_IDEMN.ClientID%>').value * document.getElementById('<%=MS__DLC_CR_RATEN.ClientID%>').value * .01).toFixed(2);
            }
            else { document.getElementById('<%=MS__DLC_CR_PREM.ClientID%>').value = ""; }
        }
        function CalcPremWreck() {
            if (document.getElementById('<%=MS__DLC_WR_RATEN.ClientID%>').value >= 0 && document.getElementById('<%=MS__DLC_WR_RATEN.ClientID%>').value <= 100) {
                document.getElementById('<%=MS__DLC_WR_PREM.ClientID%>').value =
                parseFloat(document.getElementById('<%=MS__DLC_WR_LIMIT_IDEMN.ClientID%>').value * document.getElementById('<%=MS__DLC_WR_RATEN.ClientID%>').value * .01).toFixed(2);
            }
            else { document.getElementById('<%=MS__DLC_WR_PREM.ClientID%>').value = ""; }
        }

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

        function ValidateNumberOnly() {
            if ((event.keyCode < 48 || event.keyCode > 57)) {
                event.returnValue = false;
            }
        }

                function Toggle(flag, ctrl) {

                    if (flag) {

                        if (ctrl == 'VEHDET__DLC_THIRD_PARTY_LIABILITY') {

                            document.getElementById("<%= VEHDET__TPL_LIMIT_IDEMNITY.ClientID%>").readOnly = false;
                            document.getElementById("<%= VEHDET__TPL_RATEN.ClientID%>").readOnly = false;
                            ValidatorEnable($("#<%= rfv_VEHDET__TPL_LIMIT_IDEMNITY.ClientID%>")[0], true);
                            ValidatorEnable($("#<%= rfv_VEHDET__TPL_RATEN.ClientID%>")[0], true);
                            ValidatorEnable($("#<%= rng_MS__DLC_CHR_RATEN.ClientID%>")[0], true);
                            document.getElementById("<%= VEHDET__TPL_LIMIT_IDEMNITY.ClientID%>").value = "2500000";
                        }



                        if (ctrl == 'MS__DLC_CAR_HIRE') {
                            document.getElementById("<%= MS__DLC_CHR_LIMIT_IDEMN.ClientID%>").readOnly = false;
                            document.getElementById("<%= MS__DLC_CHR_RATEN.ClientID%>").readOnly = false;
                            ValidatorEnable($("#<%= rfv_MS_DLC_CHR_LIMIT_IDEMN.ClientID%>")[0], true);
                            ValidatorEnable($("#<%= rfv_MS_DLC_CHR_RATEN.ClientID%>")[0], true);
                            ValidatorEnable($("#<%= rng_MS__DLC_CHR_RATEN.ClientID%>")[0], true);

                        }



                        if (ctrl == 'MS__DLC_CONTIGENT_LIAB') {

                            document.getElementById("<%= MS__DLC_CL_LIMIT_IDEMN.ClientID%>").readOnly = false;
                            document.getElementById("<%= MS__DLC_CL_RATEN.ClientID%>").readOnly = false;
                            ValidatorEnable($("#<%= rfv_MS__DLC_CL_LIMIT_IDEMN.ClientID%>")[0], true);
                            ValidatorEnable($("#<%= rfv_MS__DLC_CL_RATEN.ClientID%>")[0], true);
                            ValidatorEnable($("#<%= rng_MS__DLC_CL_RATEN.ClientID%>")[0], true);

                        }


                        if (ctrl == 'MS__DLC_CR_SHRTFALL') {
                            document.getElementById("<%= MS__DLC_CR_LIMIT_IDEMN.ClientID%>").readOnly = false;
                    document.getElementById("<%= MS__DLC_CR_RATEN.ClientID%>").readOnly = false;
                    ValidatorEnable($("#<%= rfv_MS__DLC_CR_LIMIT_IDEMN.ClientID%>")[0], true);
                    ValidatorEnable($("#<%= rfv_MS__DLC_CR_RATEN.ClientID%>")[0], true);
                    ValidatorEnable($("#<%= rng_MS__DLC_CR_RATEN.ClientID%>")[0], true);


                }
                if (ctrl == 'MS__DLC_WRECKAGE_REMOVAL') {

                    document.getElementById("<%= MS__DLC_WR_LIMIT_IDEMN.ClientID%>").readOnly = false;
                    document.getElementById("<%= MS__DLC_WR_RATEN.ClientID%>").readOnly = false;
                    ValidatorEnable($("#<%= rfv_MS__DLC_WR_LIMIT_IDEMN.ClientID%>")[0], true);
                    ValidatorEnable($("#<%= rfv_MS__DLC_WR_RATEN.ClientID%>")[0], true);
                    ValidatorEnable($("#<%= rng_MS__DLC_WR_RATEN.ClientID%>")[0], true);


                }
            }


            else {

                if (!($('#<%= VEHDET__DLC_THIRD_PARTY_LIABILITY.ClientID%>')[0].checked)) {

                            document.getElementById("<%= VEHDET__TPL_LIMIT_IDEMNITY.ClientID%>").readOnly = true;
                    document.getElementById("<%= VEHDET__TPL_RATEN.ClientID%>").readOnly = true;
                    ValidatorEnable($("#<%= rfv_VEHDET__TPL_LIMIT_IDEMNITY.ClientID%>")[0], false);
                    ValidatorEnable($("#<%= rfv_VEHDET__TPL_RATEN.ClientID%>")[0], false);
                    ValidatorEnable($("#<%= rng_MS__DLC_CHR_RATEN.ClientID%>")[0], false);
                    document.getElementById("<%= VEHDET__TPL_LIMIT_IDEMNITY.ClientID%>").value = "";
                    document.getElementById("<%= VEHDET__TPL_RATEN.ClientID%>").value = "";
                }


                if (!($('#<%= MS__DLC_CAR_HIRE.ClientID%>')[0].checked)) {
                            document.getElementById("<%= MS__DLC_CHR_LIMIT_IDEMN.ClientID%>").readOnly = true;
                    document.getElementById("<%= MS__DLC_CHR_RATEN.ClientID%>").readOnly = true;
                    ValidatorEnable($("#<%= rfv_MS_DLC_CHR_LIMIT_IDEMN.ClientID%>")[0], false);
                    ValidatorEnable($("#<%= rfv_MS_DLC_CHR_RATEN.ClientID%>")[0], false);
                    ValidatorEnable($("#<%= rng_MS__DLC_CHR_RATEN.ClientID%>")[0], false);
                    document.getElementById("<%= MS__DLC_CHR_LIMIT_IDEMN.ClientID%>").value = "";
                    document.getElementById("<%= MS__DLC_CHR_RATEN.ClientID%>").value = "";
                }

                if (!($('#<%= MS__DLC_CONTIGENT_LIAB.ClientID%>')[0].checked)) {

                            document.getElementById("<%= MS__DLC_CL_LIMIT_IDEMN.ClientID%>").readOnly = true;
                document.getElementById("<%= MS__DLC_CL_RATEN.ClientID%>").readOnly = true;
                ValidatorEnable($("#<%= rfv_MS__DLC_CL_LIMIT_IDEMN.ClientID%>")[0], false);
                ValidatorEnable($("#<%= rfv_MS__DLC_CL_RATEN.ClientID%>")[0], false);
                ValidatorEnable($("#<%= rng_MS__DLC_CL_RATEN.ClientID%>")[0], false);
                document.getElementById("<%= MS__DLC_CL_LIMIT_IDEMN.ClientID%>").value = "";
                document.getElementById("<%= MS__DLC_CL_RATEN.ClientID%>").value = "";

            }

            if (!($('#<%= MS__DLC_CR_SHRTFALL.ClientID%>')[0].checked)) {


                            document.getElementById("<%= MS__DLC_CR_LIMIT_IDEMN.ClientID%>").readOnly = true;
                document.getElementById("<%= MS__DLC_CR_RATEN.ClientID%>").readOnly = true;
                ValidatorEnable($("#<%= rfv_MS__DLC_CR_LIMIT_IDEMN.ClientID%>")[0], false);
                ValidatorEnable($("#<%= rfv_MS__DLC_CR_RATEN.ClientID%>")[0], false);
                ValidatorEnable($("#<%= rng_MS__DLC_CR_RATEN.ClientID%>")[0], false);
                document.getElementById("<%= MS__DLC_CR_LIMIT_IDEMN.ClientID%>").value = "";
                document.getElementById("<%= MS__DLC_CR_RATEN.ClientID%>").value = "";


            }


            if (!($('#<%= MS__DLC_WRECKAGE_REMOVAL.ClientID%>')[0].checked)) {


                            document.getElementById("<%= MS__DLC_WR_LIMIT_IDEMN.ClientID%>").readOnly = false;
                document.getElementById("<%= MS__DLC_WR_RATEN.ClientID%>").readOnly = true;
                ValidatorEnable($("#<%= rfv_MS__DLC_WR_LIMIT_IDEMN.ClientID%>")[0], false);
                ValidatorEnable($("#<%= rfv_MS__DLC_WR_RATEN.ClientID%>")[0], false);
                ValidatorEnable($("#<%= rng_MS__DLC_WR_RATEN.ClientID%>")[0], false);
                document.getElementById("<%= MS__DLC_WR_LIMIT_IDEMN.ClientID%>").value = "";
                document.getElementById("<%= MS__DLC_WR_RATEN.ClientID%>").value = "";




            }
        }

    }




    $(document).ready(function () {
        document.getElementById("<%= VEHDET__TPL_PREMIUM.ClientID%>").readOnly = true;
            document.getElementById("<%= MS__DLC_CHR_PREM.ClientID%>").readOnly = true;
            document.getElementById("<%= MS__DLC_CL_PREM.ClientID%>").readOnly = true;
            document.getElementById("<%= MS__DLC_CR_PREM.ClientID%>").readOnly = true;
            document.getElementById("<%= MS__DLC_WR_PREM.ClientID%>").readOnly = true;


        });

    </script>


    <div class="risk-screen">
        <div id="divMain" onscroll="SetDivPosition()">
            <NexusControl:ProgressBar ID="ucProgressBar" runat="server"></NexusControl:ProgressBar>
            <div class="card">
                <Nexus:tabindex id="ctrlTabIndex" runat="server" cssclass="TabContainer" tabcontainerclass="TabNav" activetabclass="ActiveTab" disabledclass="DisabledTab" scrollable="false"></Nexus:tabindex>
                <div class="card-body clearfix">
                    <legend>Liability Section</legend>
                    <table class="grid-table">
                        <tr>
                            <th>Default Liability Cover
                            </th>
                            <th>Limit of Indemnity  
                            </th>
                            <th>Rate  
                            </th>
                            <th>Premium  
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <label>Third Party Liability</label>
                                <asp:CheckBox ID="VEHDET__DLC_THIRD_PARTY_LIABILITY" runat="server" onclick="Toggle(this.checked,'VEHDET__DLC_THIRD_PARTY_LIABILITY');" Enabled="False" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </td>
                            <td>
                                <asp:TextBox ID="VEHDET__TPL_LIMIT_IDEMNITY" runat="server" CssClass="form-control" onblur='return CalcPremTPL();' onkeypress="if ( isNaN( String.fromCharCode(event.keyCode) )) return false;"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfv_VEHDET__TPL_LIMIT_IDEMNITY" runat="server" ControlToValidate="VEHDET__TPL_LIMIT_IDEMNITY" SetFocusOnError="true" Display="None" ErrorMessage="Limit is a mandatory field for Third Party ."></asp:RequiredFieldValidator>
                            </td>
                            <td class="fapPerc">
                                <asp:TextBox ID="VEHDET__TPL_RATEN" runat="server" onblur='return CalcPremTPL();' CssClass="form-control e-num4"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfv_VEHDET__TPL_RATEN" runat="server" ControlToValidate="VEHDET__TPL_RATEN" SetFocusOnError="true" Display="None" ErrorMessage="Rate is a mandatory field for Third Party."></asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="rng_VEHDET__TPL_RATEN" runat="server" Type="Double" MinimumValue="0" MaximumValue="100" ControlToValidate="VEHDET__TPL_RATEN" SetFocusOnError="true" Display="None" ErrorMessage="Rate must be B\w 0 to 100% for Third Party."></asp:RangeValidator>

                            </td>
                            <td class="premium">
                                <asp:TextBox ID="VEHDET__TPL_PREMIUM" runat="server" Enabled="false" CssClass="form-control e-num2"></asp:TextBox>

                            </td>
                        </tr>
                    </table>
                    <table class="grid-table">
                        <tr>
                            <th>Optional Cover
                            </th>
                            <th>Limit of Indemnity
                            </th>
                            <th>Rate
                            </th>
                            <th>Premium
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Cars Hire
                                </label>
                                <asp:CheckBox ID="MS__DLC_CAR_HIRE" runat="server" onclick="Toggle(this.checked,'MS__DLC_CAR_HIRE');" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </td>
                            <td>

                                <asp:TextBox ID="MS__DLC_CHR_LIMIT_IDEMN" runat="server" onblur='return CalcPremCarHire();' onkeypress="if ( isNaN( String.fromCharCode(event.keyCode) )) return false;"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfv_MS_DLC_CHR_LIMIT_IDEMN" runat="server" ControlToValidate="MS__DLC_CHR_LIMIT_IDEMN" SetFocusOnError="true" Display="None" Enabled="false" ErrorMessage="Limit is a mandatory field for Cars Hire ."></asp:RequiredFieldValidator>
                            </td>
                            <td class="fapPerc">
                                <asp:TextBox ID="MS__DLC_CHR_RATEN" runat="server" onblur='return CalcPremCarHire();' CssClass="form-control e-num4"></asp:TextBox>
                                <asp:RangeValidator ID="rng_MS__DLC_CHR_RATEN" Type="Double" runat="server" MinimumValue="0" MaximumValue="100" ControlToValidate="MS__DLC_CHR_RATEN" SetFocusOnError="true" Display="None" ErrorMessage="Rate must be B\w 0 to 100% for Cars Hire."></asp:RangeValidator>
                                <asp:RequiredFieldValidator ID="rfv_MS_DLC_CHR_RATEN" runat="server" ControlToValidate="MS__DLC_CHR_RATEN" SetFocusOnError="true" Display="None" Enabled="false" ErrorMessage="Rate is a mandatory field for Cars Hire ."></asp:RequiredFieldValidator>
                            </td>
                            <td class="premium">
                                <asp:TextBox ID="MS__DLC_CHR_PREM" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Contingent Liability</label>
                                <asp:CheckBox ID="MS__DLC_CONTIGENT_LIAB" runat="server" onclick="Toggle(this.checked,'MS__DLC_CONTIGENT_LIAB');" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </td>
                            <td>
                                <asp:TextBox ID="MS__DLC_CL_LIMIT_IDEMN" runat="server" onblur='return CalcPremContLiab();' CssClass="form-control" onkeypress="if ( isNaN( String.fromCharCode(event.keyCode) )) return false;"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfv_MS__DLC_CL_LIMIT_IDEMN" runat="server" ControlToValidate="MS__DLC_CL_LIMIT_IDEMN" SetFocusOnError="true" Display="None" Enabled="false" ErrorMessage="Limit is a mandatory field for Contingent Liability ."></asp:RequiredFieldValidator>
                            </td>
                            <td class="fapPerc">
                                <asp:TextBox ID="MS__DLC_CL_RATEN" runat="server" onblur='return CalcPremContLiab();' CssClass="form-control e-num4"></asp:TextBox>
                                <asp:RangeValidator ID="rng_MS__DLC_CL_RATEN" runat="server" Type="Double" MinimumValue="0" MaximumValue="100" ControlToValidate="MS__DLC_CL_RATEN" SetFocusOnError="true" Display="None" ErrorMessage="Rate must be B\w 0 to 100% for Contingent Liability."></asp:RangeValidator>
                                <asp:RequiredFieldValidator ID="rfv_MS__DLC_CL_RATEN" runat="server" ControlToValidate="MS__DLC_CL_RATEN" SetFocusOnError="true" Display="None" Enabled="false" ErrorMessage="Rate is a mandatory field for Contingent Liability."></asp:RequiredFieldValidator>
                            </td>
                            <td class="premium">
                                <asp:TextBox ID="MS__DLC_CL_PREM" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Credit Shortfall</label>
                                <asp:CheckBox ID="MS__DLC_CR_SHRTFALL" runat="server" onclick="Toggle(this.checked,'MS__DLC_CR_SHRTFALL');" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </td>
                            <td>
                                <asp:TextBox ID="MS__DLC_CR_LIMIT_IDEMN" runat="server" onblur='return CalcPremShrtFall();' CssClass="form-control" onkeypress="if ( isNaN( String.fromCharCode(event.keyCode) )) return false;"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfv_MS__DLC_CR_LIMIT_IDEMN" runat="server" ControlToValidate="MS__DLC_CR_LIMIT_IDEMN" SetFocusOnError="true" Display="None" Enabled="false" ErrorMessage="Limit is a mandatory field for credit shortfall."></asp:RequiredFieldValidator>
                            </td>
                            <td class="fapPerc">
                                <asp:TextBox ID="MS__DLC_CR_RATEN" CssClass="form-control e-num4" runat="server" onblur='return CalcPremShrtFall();'></asp:TextBox>
                                <asp:RangeValidator ID="rng_MS__DLC_CR_RATEN" Type="Double" runat="server" MinimumValue="0" MaximumValue="100" ControlToValidate="MS__DLC_CR_RATEN" SetFocusOnError="true" Display="None" ErrorMessage="Rate must be B\w 0 to 100% credit shortfall."></asp:RangeValidator>
                                <asp:RequiredFieldValidator ID="rfv_MS__DLC_CR_RATEN" runat="server" ControlToValidate="MS__DLC_CR_RATEN" SetFocusOnError="true" Display="None" Enabled="false" ErrorMessage="Rate is a mandatory field for credit shortfall."></asp:RequiredFieldValidator>
                            </td>
                            <td class="premium">
                                <asp:TextBox ID="MS__DLC_CR_PREM" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Wreckage Removal
                                </label>
                                <asp:CheckBox ID="MS__DLC_WRECKAGE_REMOVAL" runat="server" onclick="Toggle(this.checked,'MS__DLC_WRECKAGE_REMOVAL');" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </td>
                            <td>
                                <asp:TextBox ID="MS__DLC_WR_LIMIT_IDEMN" runat="server" onblur='return CalcPremWreck();' CssClass="form-control" onkeypress="if ( isNaN( String.fromCharCode(event.keyCode) )) return false;"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfv_MS__DLC_WR_LIMIT_IDEMN" runat="server" ControlToValidate="MS__DLC_WR_LIMIT_IDEMN" SetFocusOnError="true" Display="None" Enabled="false" ErrorMessage="Limit is a mandatory field for Wreckage Removal."></asp:RequiredFieldValidator>
                            </td>
                            <td class="fapPerc">
                                <asp:TextBox ID="MS__DLC_WR_RATEN" runat="server" onblur='return CalcPremWreck();' CssClass="form-control e-num4"></asp:TextBox>
                                <asp:RangeValidator ID="rng_MS__DLC_WR_RATEN" Type="Double" runat="server" MinimumValue="0" MaximumValue="100" ControlToValidate="MS__DLC_WR_RATEN" SetFocusOnError="true" Display="None" ErrorMessage="Rate must be B\w 0 to 100%  Wreckage Removal."></asp:RangeValidator>
                                <asp:RequiredFieldValidator ID="rfv_MS__DLC_WR_RATEN" runat="server" ControlToValidate="MS__DLC_WR_RATEN" SetFocusOnError="true" Display="None" Enabled="false" ErrorMessage="Rate is a mandatory field for Wreckage Removal."></asp:RequiredFieldValidator>
                            </td>
                            <td class="premium">
                                <asp:TextBox ID="MS__DLC_WR_PREM" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>

                    </table>
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
