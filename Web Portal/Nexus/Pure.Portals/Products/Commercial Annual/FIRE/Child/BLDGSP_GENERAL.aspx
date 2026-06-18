<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master" CodeFile="BLDGSP_GENERAL.aspx.vb" Inherits="Nexus.BLDGSP_GENERAL" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/AddressCntrl.ascx" TagName="AddressCntrl" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/StandardWordings.ascx" TagName="StandardWording" TagPrefix="NexusControl" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            document.getElementById("<%= IPF__IPARTIES_TOTAL_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= IPF__IPARTIES_TOTAL_PREMIUM.ClientID%>").readOnly = true;

            document.getElementById("<%= IPF__IPARTIES_ITEM_RATE.ClientID%>").value = document.getElementById("<%= hvAgreedRateValue.ClientID%>").value;
            ValidatorEnable($("#<%= rfvFireBldgEscPerc.ClientID%>")[0], false);



            document.getElementById("<%= IPF__IPARTIES_ESCALATION_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= IPF__IPARTIES_ESCALATION_RATE.ClientID%>").readOnly = true;
            document.getElementById("<%= IPF__IPARTIES_ESCALATION_PREM.ClientID%>").readOnly = true;


            document.getElementById("<%= IPF__IPARTIES_INFLATION_Y1_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= IPF__IPARTIES_INFLATION_Y1_RATE.ClientID%>").readOnly = true;
            document.getElementById("<%= IPF__IPARTIES_INFLATION_Y1_PREM.ClientID%>").readOnly = true;


            document.getElementById("<%= IPF__IPARTIES_INFLATION_Y2_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= IPF__IPARTIES_INFLATION_Y2_RATE.ClientID%>").readOnly = true;
            document.getElementById("<%= IPF__IPARTIES_INFLATION_Y2_PREM.ClientID%>").readOnly = true;
            ToggleInfEscContent($(this)[0].checked);

            if (document.getElementById("<%= IPF__IPARTIES_ITEM_SI.ClientID%>").value == "") {

                document.getElementById("<%= IPF__IPARTIES_TOTAL_SI.ClientID%>").value = "0";
                document.getElementById("<%= IPF__IPARTIES_TOTAL_PREMIUM.ClientID%>").value = "0";
            }
            $("input.sum_cal").blur(function () {
                fnSum_Cal();
            });

            $("input.pre_cal").blur(function () {
                fnPre_Cal();
            });


            $('#<%= IPF__IPARTIES_INFLATION_ESCALATION.ClientID%>').click(function () {
                ToggleInfEscContent($(this)[0].checked);
                fnSum_Cal();
                fnPre_Cal();
            });
        });


        function fnSum_Cal() {
            var sival = 0;
            if (($('#<%= IPF__IPARTIES_INFLATION_ESCALATION.ClientID%>')[0].checked)) {
                $('input.sum_cal').each(function () {
                    if ($(this).val() != "") {
                        sival += parseFloat($(this).val())
                    }
                });
            }
            else {
                sival = document.getElementById("<%= IPF__IPARTIES_ITEM_SI.ClientID%>").value;

            }
            document.getElementById("<%= IPF__IPARTIES_TOTAL_SI.ClientID%>").value = parseFloat(sival || 0);
        }


        function fnPre_Cal() {
            var preval = 0;
            if (($('#<%= IPF__IPARTIES_INFLATION_ESCALATION.ClientID%>')[0].checked)) {
                $('input.pre_cal').each(function () {
                    if ($(this).val() != "") {
                        preval += parseFloat($(this).val())
                    }
                });
            }
            else {
                preval = document.getElementById("<%= IPF__IPARTIES_ITEM_PREM.ClientID%>").value;

            }
            document.getElementById("<%= IPF__IPARTIES_TOTAL_PREMIUM.ClientID%>").value = parseFloat(preval || 0).toFixed(2);
        }

        function CalculateIPFSIPrem() {
            if (($('#<%= IPF__IPARTIES_ITEM_FLAT_PREM.ClientID%>')[0].checked)) {

                document.getElementById("<%= IPF__IPARTIES_TOTAL_SI.ClientID%>").value = (parseFloat(document.getElementById("<%= IPF__IPARTIES_ITEM_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= IPF__IPARTIES_ESCALATION_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= IPF__IPARTIES_INFLATION_Y1_SI.ClientID%>").value) || 0) + +(parseFloat(document.getElementById("<%= IPF__IPARTIES_INFLATION_Y2_SI.ClientID%>").value) || 0);
                document.getElementById("<%= IPF__IPARTIES_TOTAL_PREMIUM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= IPF__IPARTIES_ITEM_PREM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= IPF__IPARTIES_ESCALATION_PREM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= IPF__IPARTIES_INFLATION_Y1_PREM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= IPF__IPARTIES_INFLATION_Y2_PREM.ClientID%>").value) || 0) || 0).toFixed(2);
            }
        }

        function CalculateSIPremTotal() {
            var d_sumins = (parseFloat(document.getElementById("<%= IPF__IPARTIES_ITEM_SI.ClientID%>").value) || 0);
            var d_prem = (parseFloat(document.getElementById("<%= IPF__IPARTIES_ITEM_PREM.ClientID%>").value) || 0);
            var d_rate = (parseFloat(document.getElementById("<%= IPF__IPARTIES_ITEM_RATE.ClientID%>").value) || 0);

            if (($('#<%= IPF__IPARTIES_ITEM_FLAT_PREM.ClientID%>')[0].checked)) {
                document.getElementById("<%= IPF__IPARTIES_ITEM_RATE.ClientID%>").value = (d_prem * 100) / d_sumins;
            }
            else {
                document.getElementById("<%= IPF__IPARTIES_ITEM_PREM.ClientID%>").value = d_sumins * d_rate * .01;
            }
            if (($('#<%= IPF__IPARTIES_INFLATION_ESCALATION.ClientID%>')[0].checked)) {
                CalculateEscSI();
                CalculateInfOneSI();
                CalculateInfTwoSI();
            }
            fnSum_Cal();
            fnPre_Cal();
        }
        function ToggleContent(flag) {
            document.getElementById("<%= IPF__IPARTIES_ITEM_RATE.ClientID%>").readOnly = true;
            document.getElementById("<%= IPF__IPARTIES_ITEM_PREM.ClientID%>").readOnly = !flag;
            document.getElementById("<%= IPF__IPARTIES_ITEM_SI.ClientID%>").value = "";
            document.getElementById("<%= IPF__IPARTIES_ITEM_PREM.ClientID%>").value = "";
            document.getElementById("<%= IPF__IPARTIES_ITEM_RATE.ClientID%>").value = document.getElementById("<%= hvAgreedRateValue.ClientID%>").value;

            if (flag) {
                document.getElementById("<%= IPF__IPARTIES_ITEM_SI.ClientID%>").className = "form-control field-mandatory sum_cal";
                document.getElementById("<%= IPF__IPARTIES_ITEM_RATE.ClientID%>").value = ""

            }
            else {
                document.getElementById("<%= IPF__IPARTIES_ITEM_SI.ClientID%>").className = "form-control sum_cal";
                document.getElementById("<%= IPF__IPARTIES_ITEM_RATE.ClientID%>").value = document.getElementById("<%= hvAgreedRateValue.ClientID%>").value;
            }


            fnSum_Cal();
            fnPre_Cal();
        }


        function CalculateEscSI() {

            if (($('#<%= IPF__IPARTIES_INFLATION_ESCALATION.ClientID%>')[0].checked)) {
                ValidatorEnable($("#<%= rfvFireBldgEscPerc.ClientID%>")[0], true);
                document.getElementById("<%= IPF__IPARTIES_ESCALATION_PERC.ClientID%>").className = "form-control";
                if (document.getElementById("<%= IPF__IPARTIES_ESCALATION_PERC.ClientID%>").value != "") {

                    document.getElementById("<%= IPF__IPARTIES_ESCALATION_SI.ClientID%>").value = ((parseFloat(document.getElementById("<%= IPF__IPARTIES_ITEM_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= IPF__IPARTIES_ESCALATION_PERC.ClientID%>").value) || 0)) / 100;
                    document.getElementById("<%= IPF__IPARTIES_ESCALATION_RATE.ClientID%>").value = ((parseFloat(document.getElementById("<%= IPF__IPARTIES_ITEM_RATE.ClientID%>").value) || 0) * 0.5).toFixed(4);
                    document.getElementById("<%= IPF__IPARTIES_ESCALATION_PREM.ClientID%>").value = (parseFloat(document.getElementById("<%= IPF__IPARTIES_ESCALATION_RATE.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= IPF__IPARTIES_ESCALATION_SI.ClientID%>").value) || 0) * .01;
                    fnSum_Cal();
                    fnPre_Cal();
                }
            }
            else {
                ValidatorEnable($("#<%= rfvFireBldgEscPerc.ClientID%>")[0], false);
                document.getElementById("<%= IPF__IPARTIES_ESCALATION_PERC.ClientID%>").className = "form-control";
            }
        }

        function CalculateInfOneSI() {

            if (($('#<%= IPF__IPARTIES_INFLATION_ESCALATION.ClientID%>')[0].checked)) {
                if (document.getElementById("<%= IPF__IPARTIES_INFLATION_Y1_PERC.ClientID%>").value != "") {
                    document.getElementById("<%= IPF__IPARTIES_INFLATION_Y1_SI.ClientID%>").value = ((parseFloat(document.getElementById("<%= IPF__IPARTIES_ITEM_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= IPF__IPARTIES_ESCALATION_SI.ClientID%>").value) || 0)) * (parseFloat(document.getElementById("<%= IPF__IPARTIES_INFLATION_Y1_PERC.ClientID%>").value) || 0) * .01;
                    document.getElementById("<%= IPF__IPARTIES_INFLATION_Y1_RATE.ClientID%>").value = ((parseFloat(document.getElementById("<%= IPF__IPARTIES_ESCALATION_RATE.ClientID%>").value) || 0) * 0.5).toFixed(4);
                    document.getElementById("<%= IPF__IPARTIES_INFLATION_Y1_PREM.ClientID%>").value = (parseFloat(document.getElementById("<%= IPF__IPARTIES_INFLATION_Y1_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= IPF__IPARTIES_ESCALATION_RATE.ClientID%>").value) || 0) * .01;
                }
                fnSum_Cal();
                fnPre_Cal();
            }
        }

        function CalculateInfTwoSI() {
            if (($('#<%= IPF__IPARTIES_INFLATION_ESCALATION.ClientID%>')[0].checked)) {
                if (document.getElementById("<%= IPF__IPARTIES_INFLATION_Y2_PERC.ClientID%>").value != "") {
                    document.getElementById("<%= IPF__IPARTIES_INFLATION_Y2_SI.ClientID%>").value = ((parseFloat(document.getElementById("<%= IPF__IPARTIES_ITEM_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= IPF__IPARTIES_ESCALATION_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= IPF__IPARTIES_INFLATION_Y1_SI.ClientID%>").value) || 0)) * (parseFloat(document.getElementById("<%= IPF__IPARTIES_INFLATION_Y2_PERC.ClientID%>").value) || 0) * .01;
                    document.getElementById("<%= IPF__IPARTIES_INFLATION_Y2_RATE.ClientID%>").value = ((parseFloat(document.getElementById("<%= IPF__IPARTIES_ESCALATION_RATE.ClientID%>").value) || 0) * 0.5).toFixed(4);
                    document.getElementById("<%= IPF__IPARTIES_INFLATION_Y2_PREM.ClientID%>").value = (parseFloat(document.getElementById("<%= IPF__IPARTIES_INFLATION_Y2_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= IPF__IPARTIES_INFLATION_Y2_RATE.ClientID%>").value) || 0) * .01;
                }
                fnSum_Cal();
                fnPre_Cal();
            }
        }



        function ToggleInfEscContent(flag, OnLoad) {

            document.getElementById("<%= IPF__IPARTIES_ESCALATION_PERC.ClientID%>").readOnly = !flag;

            //document.getElementById("<%= IPF__IPARTIES_ESCALATION_SI.ClientID%>").readOnly = !flag;
            //document.getElementById("<%= IPF__IPARTIES_ESCALATION_RATE.ClientID%>").readOnly = !flag;
            //document.getElementById("<%= IPF__IPARTIES_ESCALATION_PREM.ClientID%>").readOnly = !flag;


            document.getElementById("<%= IPF__IPARTIES_INFLATION_Y1_PERC.ClientID%>").readOnly = !flag;

            // document.getElementById("<%= IPF__IPARTIES_INFLATION_Y1_SI.ClientID%>").readOnly = !flag;
            //document.getElementById("<%= IPF__IPARTIES_INFLATION_Y1_RATE.ClientID%>").readOnly = !flag;
            //document.getElementById("<%= IPF__IPARTIES_INFLATION_Y1_PREM.ClientID%>").readOnly = !flag;

            document.getElementById("<%= IPF__IPARTIES_INFLATION_Y2_PERC.ClientID%>").readOnly = !flag;

            //document.getElementById("<%= IPF__IPARTIES_INFLATION_Y2_SI.ClientID%>").readOnly = !flag;
            //document.getElementById("<%= IPF__IPARTIES_INFLATION_Y2_RATE.ClientID%>").readOnly = !flag;
            //document.getElementById("<%= IPF__IPARTIES_INFLATION_Y2_PREM.ClientID%>").readOnly = !flag;


            document.getElementById("<%= IPF__IPARTIES_ESCALATION_PERC.ClientID%>").value = "";
            if (flag) {

                if (OnLoad == "true") {

                    document.getElementById("<%= IPF__IPARTIES_ESCALATION_PERC.ClientID%>").className = "form-control field-mandatory";
                    document.getElementById("<%= IPF__IPARTIES_ESCALATION_PERC.ClientID%>").readOnly = false;
                }
            }
            else {
                //if (OnLoad == "false") {
                document.getElementById("<%= IPF__IPARTIES_ESCALATION_PERC.ClientID%>").value = ""
                document.getElementById("<%= IPF__IPARTIES_ESCALATION_RATE.ClientID%>").value = ""
                document.getElementById("<%= IPF__IPARTIES_ESCALATION_PREM.ClientID%>").value = ""
                document.getElementById("<%= IPF__IPARTIES_ESCALATION_SI.ClientID%>").value = ""

                document.getElementById("<%= IPF__IPARTIES_INFLATION_Y1_PERC.ClientID%>").value = ""
                document.getElementById("<%= IPF__IPARTIES_INFLATION_Y1_SI.ClientID%>").value = ""
                document.getElementById("<%= IPF__IPARTIES_INFLATION_Y1_RATE.ClientID%>").value = ""
                document.getElementById("<%= IPF__IPARTIES_INFLATION_Y1_PREM.ClientID%>").value = ""

                document.getElementById("<%= IPF__IPARTIES_INFLATION_Y2_PERC.ClientID%>").value = ""
                document.getElementById("<%= IPF__IPARTIES_INFLATION_Y2_SI.ClientID%>").value = ""
                document.getElementById("<%= IPF__IPARTIES_INFLATION_Y2_RATE.ClientID%>").value = ""
                document.getElementById("<%= IPF__IPARTIES_INFLATION_Y2_PREM.ClientID%>").value = ""
                //}
            }
        }
    </script>

    <div class="risk-screen">


        <NexusControl:ProgressBar ID="ucProgressBar" runat="server"></NexusControl:ProgressBar>
        <div class="card">
            <nexus:tabindex id="ctrlTabIndex" runat="server" cssclass="TabContainer" tabcontainerclass="page-progress" activetabclass="ActiveTab" disabledclass="DisabledTab" scrollable="false"></nexus:tabindex>
            <div class="card-body clearfix">
                <asp:HiddenField ID="hvAgreedRateValue" runat="server"></asp:HiddenField>
                <legend>Specified Item Overview
                </legend>
                <div class="form-horizontal">
                    <div id="liIPF__IPARTIES_TOTAL_SI" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Total Sum Insured</label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="IPF__IPARTIES_TOTAL_SI" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div id="liIPF__IPARTIES_TOTAL_PREMIUM" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Total Premium</label><div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="IPF__IPARTIES_TOTAL_PREMIUM" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <legend>Specified Item
                </legend>
                <div class="form-horizontal">
                    <div id="liIPF__IPARTIES_ITEM_FLAT_PREM" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Flat Premium</label>
                        <div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="IPF__IPARTIES_ITEM_FLAT_PREM" runat="server" onclick="ToggleContent(this.checked);" Text=" " CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>
                    <div id="liIPF__IPARTIES_ITEM_DESC" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Description</label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="IPF__IPARTIES_ITEM_DESC" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvIPF__IPARTIES_ITEM_DESC" runat="server" ControlToValidate="IPF__IPARTIES_ITEM_DESC" ErrorMessage="Description is required field" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                    </div>
                    <div id="liIPF__IPARTIES_ITEM_SI" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Sum Insured</label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="IPF__IPARTIES_ITEM_SI" runat="server" CssClass="sum_cal form-control" onblur="CalculateSIPremTotal();"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="rfv_IPF__IPARTIES_ITEM_SI" runat="server" ControlToValidate="IPF__IPARTIES_ITEM_SI" ErrorMessage="Sum Insured is required field" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                    </div>

                    <div id="liIPF__IPARTIES_ITEM_RATE" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Rate</label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="IPF__IPARTIES_ITEM_RATE" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div id="liIPF__IPARTIES_ITEM_PREM" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Premium</label>
                        <div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="IPF__IPARTIES_ITEM_PREM" runat="server" CssClass="pre_cal form-control" onblur="CalculateSIPremTotal();"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="grid-card table-responsive">
                    <table class="grid-table">
                        <tr>
                            <th style="width: 52%">
                                <label>Inflation/Escalation</label>
                                <asp:CheckBox ID="IPF__IPARTIES_INFLATION_ESCALATION" runat="server" onclick="CalculateEscSI();" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </th>
                            <th style="width: 12%">
                                <label>Perc.</label></th>
                            <th style="width: 13%">
                                <label>Sum Insured</label></th>
                            <th style="width: 12%">
                                <label>Rate</label></th>
                            <th style="width: 22%">
                                <label>Premium</label></th>
                        </tr>
                        <tr>
                            <td>
                                <label>Escalation</label></td>
                            <td>
                                <asp:TextBox ID="IPF__IPARTIES_ESCALATION_PERC" runat="server" onblur="CalculateEscSI();" CssClass="form-control"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="rfvFireBldgEscPerc" runat="server" ControlToValidate="IPF__IPARTIES_ESCALATION_PERC" ErrorMessage="Sum Insured FIRE BUILDINGS ESCALATION PERC is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator></td>
                            <td>
                                <asp:TextBox ID="IPF__IPARTIES_ESCALATION_SI" runat="server" CssClass="sum_cal form-control" onblur="CalculateIPFSIPrem();"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="IPF__IPARTIES_ESCALATION_RATE" runat="server" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="IPF__IPARTIES_ESCALATION_PREM" CssClass="pre_cal e-num2 form-control" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <label>Inflation Y1</label></td>
                            <td>
                                <asp:TextBox ID="IPF__IPARTIES_INFLATION_Y1_PERC" runat="server" onblur="CalculateInfOneSI();" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="IPF__IPARTIES_INFLATION_Y1_SI" CssClass="sum_cal form-control" runat="server"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="IPF__IPARTIES_INFLATION_Y1_RATE" runat="server" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="IPF__IPARTIES_INFLATION_Y1_PREM" CssClass="pre_cal e-num2 form-control" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Inflation Y2</label></td>

                            <td>
                                <asp:TextBox ID="IPF__IPARTIES_INFLATION_Y2_PERC" runat="server" onblur="CalculateInfTwoSI();" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="IPF__IPARTIES_INFLATION_Y2_SI" CssClass="sum_cal form-control" runat="server"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="IPF__IPARTIES_INFLATION_Y2_RATE" runat="server" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="IPF__IPARTIES_INFLATION_Y2_PREM" CssClass="pre_cal e-num2 form-control" runat="server"></asp:TextBox></td>
                        </tr>
                    </table>
                </div>
                <legend>Interested Parties</legend>
                <div class="grid-card table-responsive">
                    <nexus:itemgrid id="IPF__PNAME" runat="server" screencode="INTPARTY" autogeneratecolumns="false" gridlines="None" childpage="Child/Interested_Party.aspx">
                        <columns>
                                          <nexus:riskattribute headertext="NAME" datafield="IPARTY_NAME"></nexus:riskattribute>
                                    </columns>
                    </nexus:itemgrid>
                </div>
            </div>
            <div class='card-footer'>
                <asp:LinkButton ID="btnBack" runat="server" Text="<i class='fa fa-chevron-left' aria-hidden='true'></i> Back" OnClick="BackButton" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnNext" runat="server" Text="Next <i class='fa fa-chevron-right' aria-hidden='true'></i>" OnClick="NextButton" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnFinish" runat="server" Text="<i class='fa fa-check' aria-hidden='true'></i> Finish" OnClick="FinishButton" OnPreRender="PreRenderFinish" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:ValidationSummary ID="vldsumSummary" DisplayMode="BulletList" HeaderText="Summary" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
