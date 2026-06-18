<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master" CodeFile="FIRE_FIREFIREF.aspx.vb" Inherits="Nexus.FIRE_FIREFIREF" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/AddressCntrl.ascx" TagName="AddressCntrl" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/StandardWordings.ascx" TagName="StandardWording" TagPrefix="NexusControl" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            document.getElementById("<%= FIRE__FOV_TOTAL_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= RI_EXP__RI_RENT_RI_EXPOSURE.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__FOV_TOTAL_PREM.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__RD1_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__RD2_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__RD1_RATE.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__RD2_RATE.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__RD1_PREMIUM.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__RD2_PREMIUM.ClientID%>").readOnly = true;

            document.getElementById("<%= FIRE__RD1_NO_OF_MONTHS.ClientID%>").readOnly = true;

            document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PERC.ClientID%>").readOnly = true;

            document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_RATE.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PREMIUM.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_PERC.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_RATE.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_PREMIUM.ClientID%>").readOnly = true;

            document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_PERC.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_RATE.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_PREMIUM.ClientID%>").readOnly = true;

            document.getElementById("<%= FIRE__CLAIM_PREP_LMT_IDEMNITY.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__CLAIM_PREP_RATE.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__CLAIM_PREPCOST_PREM.ClientID%>").readOnly = true;

            document.getElementById("<%= FIRE__DISPOSAL_SALV_PREM.ClientID%>").readOnly = true;

            document.getElementById("<%= FIRE__LEAKAGE_LMT_IDEMNITY.ClientID%>").readOnly = true;


            document.getElementById("<%= FIRE__RIOT_STRIKE_PREM.ClientID%>").readOnly = true;

            document.getElementById("<%= FIRE__LANDSLIP_LMT_IDEMNITY.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__LANDSLIP_RATE.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__LANDSLIP_TOTAL_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__LANDSLIP_PREM.ClientID%>").readOnly = true;

            document.getElementById("<%= RI_EXP__RI_BUILDING_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= RI_EXP__RI_IEP_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= RI_EXP__RI_RENT_SI.ClientID%>").readOnly = true;

            document.getElementById("<%= RI_EXP__TOTAL_FIRE_RI_RISK_S.ClientID%>").readOnly = true;
            document.getElementById("<%= RI_EXP__TOTAL_FIRE_RI_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= RI_EXP__TOTAL_FIRE_RI_EXPOSURE.ClientID%>").readOnly = true;

            document.getElementById("<%= FIRE__CLAIM_PREPCOST_PREM.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__DISPOSAL_SALV_PREM.ClientID%>").readOnly = true;

            document.getElementById("<%= FIRE__RIOT_STRIKE_PREM.ClientID%>").readOnly = true;

            document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_SI.ClientID%>").readOnly = true;

            document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PREMIUM.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_PREMIUM.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_PREMIUM.ClientID%>").readOnly = true;
            document.getElementById("<%= RI_EXP__RI_CLAIMPREP_SI.ClientID%>").readOnly = true;

            document.getElementById("<%= RI_EXP__RI_CLAIMPREP_RISK_RI_EXPOSURE.ClientID%>").readOnly = true;
            document.getElementById("<%= RI_EXP__RI_BUILDING_RI_EXPOSURE.ClientID%>").readOnly = true;
            document.getElementById("<%= RI_EXP__RI_STOCK_RI_EXPOSURE.ClientID%>").readOnly = true;

            document.getElementById("<%= RI_EXP__RI_IEP_RI_EXPOSURE.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__CLAIM_PREPCOST_PREM.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__DISPOSAL_SALV_PREM.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__LEAKAGE_PREM.ClientID%>").readOnly = true;

            document.getElementById("<%= FIRE__RIOT_STRIKE_PREM.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__LANDSLIP_PREM.ClientID%>").readOnly = true;

            document.getElementById("<%= FIRE__CLAIM_PREP_RATE.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__LEAKAGE_RATE.ClientID%>").readOnly = true;
            document.getElementById("<%= FIRE__LANDSLIP_RATE.ClientID%>").readOnly = true;

            document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PERC.ClientID%>").className = "form-control";

            ValidatorEnable($("#<%= rfvFireClaimLmtIdemn.ClientID%>")[0], false);
            document.getElementById("<%= FIRE__CLAIM_PREP_LMT_IDEMNITY.ClientID%>").className = "form-control";

            ValidatorEnable($("#<%= rfvClaimPrepRate.ClientID%>")[0], false);
            document.getElementById("<%= FIRE__CLAIM_PREP_RATE.ClientID%>").className = "form-control";

            ValidatorEnable($("#<%= rfvFireLandLmtIdemn.ClientID%>")[0], false);
            document.getElementById("<%= FIRE__LANDSLIP_LMT_IDEMNITY.ClientID%>").className = "form-control";
            ValidatorEnable($("#<%= rfvFireLndSlipRate.ClientID%>")[0], false);
            document.getElementById("<%= FIRE__LANDSLIP_RATE.ClientID%>").className = "form-control";

            document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PERC.ClientID%>").className = "form-control";
            document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_PERC.ClientID%>").className = "form-control";
            document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_PERC.ClientID%>").className = "form-control";


            ValidatorEnable($("#<%= rfvFireRD1SI.ClientID%>")[0], false);

            ValidatorEnable($("#<%= rfvFireBldgEscPerc.ClientID%>")[0], false);


            document.getElementById("<%= FIRE__RD1_NO_OF_MONTHS.ClientID%>").className = "form-control";

            if (($('#<%= FIRE__RD1_MULTIPLE_BUILDINGS.ClientID%>')[0].checked)) {
                $('#<%= FIRE__IPF.ClientID%>')[0].disabled = false;
            }
            else { $('#<%= FIRE__IPF.ClientID%>')[0].disabled = true; }

            document.getElementById("<%= RI_EXP__RI_CLAIMPREP_RISK_RI_EXPOSURE.ClientID%>").readOnly = true;

            $("input.sum_cal").blur(function () {
                fnSum_Cal();
            });
            $("input.pre_cal").blur(function () {
                fnPre_Cal();
            });


            $('#<%= FIRE__FIRE_BUILDINGS_ESCALATION_INFLATION.ClientID%>').click(function () {
                ToggleBuildingsEscalation(($('#<%= FIRE__FIRE_BUILDINGS_ESCALATION_INFLATION.ClientID%>')[0].checked), 'true');
                fnSum_Cal();
                fnPre_Cal();
            });
            ToggleMultipleBuildings(($('#<%= FIRE__RD1_MULTIPLE_BUILDINGS.ClientID%>')[0].checked), false)

            ToggleRentRD(($('#<%= FIRE__RD1_RENT.ClientID%>')[0].checked), 'true');
            ToggleFire(($('#<%= FIRE__FOV_FLAT_PREM.ClientID%>')[0].checked));

            ToggleBuildingsEscalation(($('#<%= FIRE__FIRE_BUILDINGS_ESCALATION_INFLATION.ClientID%>')[0].checked), 'true');

            ToggleCPC(($('#<%= FIRE__EF_CLAIM_PREP_COST.ClientID%>')[0].checked));
            ToggleDS(($('#<%= FIRE__EF_DISPOSAL_SALV.ClientID%>')[0].checked));
            ToggleLeakage(($('#<%= FIRE__EF_LEAKAGE.ClientID%>')[0].checked));
            ToggleRiotStrike(($('#<%= FIRE__EF_RIOT_STRIKE.ClientID%>')[0].checked));
            ToggleSL(($('#<%= FIRE__EF_SUBSIDENCE_LANDSLIP.ClientID%>')[0].checked));
            ToggleBuildingsRD(($('#<%= FIRE__RD1_BUILDINGS.ClientID%>')[0].checked), 'true');
            calculate_builder('FIRE__RD1_SI');

            if (($('#<%= FIRE__EF_CLAIM_PREP_COST.ClientID%>')[0].checked)) {
                CalculateLIClaimPPrem();
                document.getElementById("<%= RI_EXP__RI_CLAIMPREP_RISK_RI_EXPOSURE.ClientID%>").value = ((parseFloat(document.getElementById("<%= RI_EXP__RI_CLAIMPREP_RISK_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= RI_EXP__RI_CLAIMPREP_RISK_PERC.ClientID%>").value) || 0)) * .01;
            }
            if (($('#<%= FIRE__EF_LEAKAGE.ClientID%>')[0].checked)) {
                CalculateLeakPPrem();
            }

            if (($('#<%= FIRE__EF_SUBSIDENCE_LANDSLIP.ClientID%>')[0].checked)) {
                CalculateSLandPrem();
            }

            if (($('#<%= FIRE__RD1_RENT.ClientID%>')[0].checked)) {
                calculate_builder('FIRE__RD2_SI');
            }


            if (document.getElementById("<%= RI_EXP__RI_RENT_RISK_SI.ClientID%>").value != "") {
                document.getElementById("<%= RI_EXP__RI_RENT_RI_EXPOSURE.ClientID%>").value = (parseFloat(parseFloat(cleanNumber(document.getElementById("<%= RI_EXP__RI_RENT_RISK_SI.ClientID%>").value)) * parseFloat(document.getElementById("<%= RI_EXP__RI_RENT_MPL_PERC.ClientID%>").value) * .01)).toFixed(2);
            }

            if (($('#<%= FIRE__RD1_MULTIPLE_BUILDINGS.ClientID%>')[0].checked)) {

                document.getElementById("<%= RI_EXP__RI_BUILDING_SI.ClientID%>").value = document.getElementById("<%= hvBuildingTotalSI.ClientID%>").value;
                document.getElementById("<%= RI_EXP__RI_BUILDING_RISK_SI.ClientID%>").value = document.getElementById("<%= hvBuildingTotalSI.ClientID%>").value;
                document.getElementById("<%= RI_EXP__RI_INFLATION_SI.ClientID%>").value = document.getElementById("<%= hvInfEscTotalSI.ClientID%>").value;
                document.getElementById("<%= RI_EXP__RI_INFLATION_RISK_SI.ClientID%>").value = document.getElementById("<%= hvInfEscTotalSI.ClientID%>").value;

                if (document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value == "") {
                    document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value = "100";
                }
                document.getElementById("<%= RI_EXP__RI_INFLATION_RI_EXPOSURE.ClientID%>").value = (parseFloat(document.getElementById("<%= RI_EXP__RI_INFLATION_RISK_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value) || 0) * .01;
                if (document.getElementById("<%= RI_EXP__RI_BUILDING_MPL_PERC.ClientID%>").value == "") {
                    document.getElementById("<%= RI_EXP__RI_BUILDING_MPL_PERC.ClientID%>").value = "100";
                }
                document.getElementById("<%= RI_EXP__RI_BUILDING_RI_EXPOSURE.ClientID%>").value = (parseFloat(document.getElementById("<%= RI_EXP__RI_BUILDING_RISK_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= RI_EXP__RI_BUILDING_MPL_PERC.ClientID%>").value) || 0) * .01;
                if (document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value != "") {
                    document.getElementById("<%= FIRE__RD1_RATE.ClientID%>").value = (((parseFloat(document.getElementById("<%= FIRE__RD1_PREMIUM.ClientID%>").value) || 0) * 100) / (parseFloat(document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value) || 0)).toFixed(4);
                }
            }

        });


        function fnSum_Cal() {
            var sival = 0;
            var sirent = 0;

            if (($('#<%= FIRE__FIRE_BUILDINGS_ESCALATION_INFLATION.ClientID%>')[0].checked)) {
                $('input.sum_cal').each(function () {
                    if ($(this).val() != "") {
                        sival += parseFloat($(this).val())
                    }
                });
            }
            else {
                if ($('#<%= FIRE__RD1_SI.ClientID%>').val() != "") {
                    sival = document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value;
                }

                if ($('#<%= FIRE__RD2_SI.ClientID%>').val() != "") {
                    sirent = parseFloat($('#<%= FIRE__RD2_SI.ClientID%>').val());


                }

            }

            document.getElementById("<%= FIRE__FOV_TOTAL_SI.ClientID%>").value = (parseFloat(parseFloat(sival) + parseFloat(sirent))).toFixed(2);

            fnPre_Cal();
        }

        function fnPre_Cal() {
            var preval = 0;
            var prerent = 0;

            if (($('#<%= FIRE__FIRE_BUILDINGS_ESCALATION_INFLATION.ClientID%>')[0].checked)) {
                $('input.pre_cal').each(function () {
                    if ($(this).val() != "") {
                        preval += parseFloat($(this).val())
                    }
                });
            }
            else {
                if ($('#<%= FIRE__RD1_PREMIUM.ClientID%>').val() != "") {
                    preval = document.getElementById("<%= FIRE__RD1_PREMIUM.ClientID%>").value;
                }
                if ($('#<%= FIRE__RD2_PREMIUM.ClientID%>').val() != "") {
                    prerent = parseFloat($('#<%= FIRE__RD2_PREMIUM.ClientID%>').val());
                }
            }
            document.getElementById("<%= FIRE__FOV_TOTAL_PREM.ClientID%>").value = (parseFloat(parseFloat(preval) + parseFloat(prerent))).toFixed(2);
        }

        function ToggleFire(flag) {
            document.getElementById("<%= FIRE__RD1_PREMIUM.ClientID%>").readOnly = !flag;
            document.getElementById("<%= FIRE__RD2_PREMIUM.ClientID%>").readOnly = !flag;

            document.getElementById("<%= FIRE__CLAIM_PREPCOST_PREM.ClientID%>").readOnly = !flag;
            document.getElementById("<%= FIRE__DISPOSAL_SALV_PREM.ClientID%>").readOnly = !flag;
            document.getElementById("<%= FIRE__LEAKAGE_PREM.ClientID%>").readOnly = !flag;
            document.getElementById("<%= FIRE__RIOT_STRIKE_PREM.ClientID%>").readOnly = !flag;
            document.getElementById("<%= FIRE__LANDSLIP_PREM.ClientID%>").readOnly = !flag;

            if (!($('#<%= FIRE__RD1_RENT.ClientID%>')[0].checked)) {
                document.getElementById("<%= FIRE__RD2_PREMIUM.ClientID%>").value = "";

            }

        }



        function ToggleBuildingsRD(flag, OnLoad) {
            document.getElementById("<%= FIRE__RD1_SI.ClientID%>").readOnly = !flag;
            document.getElementById("<%= FIRE__RD1_MULTIPLE_BUILDINGS.ClientID%>").disabled = !flag;

            if (!flag) {
                if (OnLoad == "false") {
                    document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value = "";
                    document.getElementById("<%= FIRE__RD1_RATE.ClientID%>").value = "";
                    document.getElementById("<%= FIRE__RD1_PREMIUM.ClientID%>").value = "";
                    $('#<%= FIRE__RD1_MULTIPLE_BUILDINGS.ClientID%>')[0].checked = false;
                    $('#<%= FIRE__FIRE_BUILDINGS_ESCALATION_INFLATION.ClientID%>')[0].checked = false;
                    $('#<%= FIRE__FIRE_BUILDINGS_ESCALATION_INFLATION.ClientID%>')[0].checked = false;
                    document.getElementById("<%= FIRE__FIRE_BUILDINGS_ESCALATION_INFLATION.ClientID%>").disabled = !flag;
                    $('#<%= FIRE__IPF.ClientID%>')[0].disabled = true;
                }
                else {
                    document.getElementById("<%= FIRE__RD1_RATE.ClientID%>").value = "";
                }
                ToggleBuildingsEscalation(flag, OnLoad);
            }

            else {
                if (($('#<%= FIRE__RD1_MULTIPLE_BUILDINGS.ClientID%>')[0].checked)) {
                    document.getElementById("<%= FIRE__FIRE_BUILDINGS_ESCALATION_INFLATION.ClientID%>").disabled = true;
                    if (document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value != "") {
                        document.getElementById("<%= FIRE__RD1_RATE.ClientID%>").value = (((parseFloat(document.getElementById("<%= FIRE__RD1_PREMIUM.ClientID%>").value) || 0) * 100) / (parseFloat(document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value) || 0)).toFixed(4);
                    }
                }
                else {
                    document.getElementById("<%= FIRE__FIRE_BUILDINGS_ESCALATION_INFLATION.ClientID%>").disabled = false;
                    if (!($('#<%= FIRE__FOV_FLAT_PREM.ClientID%>')[0].checked)) {
                        document.getElementById("<%= FIRE__RD1_RATE.ClientID%>").value = (parseFloat(document.getElementById("<%= hvAgreedRateFinal.ClientID%>").value) || 0);
                    }
                }
                fnSum_Cal();
                fnPre_Cal();
            }
        }

        //Multiple Building
        function ToggleMultipleBuildings(flag, OnLoad) {

            if (flag) {


                if (document.getElementById("<%= hvmultibulsi.ClientID%>").value != "") {

                    document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value = document.getElementById("<%= hvmultibulsi.ClientID%>").value;
                    document.getElementById("<%= FIRE__RD1_PREMIUM.ClientID%>").value = document.getElementById("<%= hvmultibulpre.ClientID%>").value;

                    if (document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value != "") {
                        document.getElementById("<%= FIRE__RD1_RATE.ClientID%>").value = (((parseFloat(document.getElementById("<%= FIRE__RD1_PREMIUM.ClientID%>").value) || 0) * 100) / (parseFloat(document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value) || 0)).toFixed(4);
                    }
                    document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PREMIUM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_RATE.ClientID%>").value) || 0) * .01).toFixed(2);



                }
                else {
                    document.getElementById("<%= FIRE__RD1_SI.ClientID%>").readOnly = true;
                    document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value = "";
                    document.getElementById("<%= FIRE__RD1_PREMIUM.ClientID%>").value = "";
                    document.getElementById("<%= FIRE__RD1_RATE.ClientID%>").value = "";
                }
                document.getElementById("<%= RI_EXP__RI_BUILDING_SI.ClientID%>").value = document.getElementById("<%= hvBuildingTotalSI.ClientID%>").value;
                document.getElementById("<%= RI_EXP__RI_BUILDING_RISK_SI.ClientID%>").value = document.getElementById("<%= hvBuildingTotalSI.ClientID%>").value;
                document.getElementById("<%= RI_EXP__RI_INFLATION_SI.ClientID%>").value = document.getElementById("<%= hvInfEscTotalSI.ClientID%>").value;
                document.getElementById("<%= RI_EXP__RI_INFLATION_RISK_SI.ClientID%>").value = document.getElementById("<%= hvInfEscTotalSI.ClientID%>").value;

                if (document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value == "") {
                    document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value = "100";
                }
                document.getElementById("<%= RI_EXP__RI_INFLATION_RI_EXPOSURE.ClientID%>").value = (parseFloat(document.getElementById("<%= RI_EXP__RI_INFLATION_RISK_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value) || 0) * .01;
                if (document.getElementById("<%= RI_EXP__RI_BUILDING_MPL_PERC.ClientID%>").value == "") {
                    document.getElementById("<%= RI_EXP__RI_BUILDING_MPL_PERC.ClientID%>").value = "100";
                }

                document.getElementById("<%= RI_EXP__RI_BUILDING_RI_EXPOSURE.ClientID%>").value = (parseFloat(document.getElementById("<%= RI_EXP__RI_BUILDING_RISK_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= RI_EXP__RI_BUILDING_MPL_PERC.ClientID%>").value) || 0) * .01;
                if (!($('#<%= FIRE__FOV_FLAT_PREM.ClientID%>')[0].checked)) {
                    if (document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value != "") {
                        document.getElementById("<%= FIRE__RD1_RATE.ClientID%>").value = (((parseFloat(document.getElementById("<%= FIRE__RD1_PREMIUM.ClientID%>").value) || 0) * 100) / (parseFloat(document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value) || 0)).toFixed(4);
                    }
                }
                $('#<%= FIRE__FIRE_BUILDINGS_ESCALATION_INFLATION.ClientID%>')[0].checked = false;
                $('#<%= FIRE__IPF.ClientID%>')[0].disabled = false;
                document.getElementById("<%= FIRE__FIRE_BUILDINGS_ESCALATION_INFLATION.ClientID%>").disabled = true;

                document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PERC.ClientID%>").value = "";
                document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_SI.ClientID%>").value = "";
                document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_RATE.ClientID%>").value = "";
                document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PREMIUM.ClientID%>").value = "";

                document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_PERC.ClientID%>").value = "";
                document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_SI.ClientID%>").value = "";
                document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_RATE.ClientID%>").value = "";
                document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_PREMIUM.ClientID%>").value = "";

                document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_PERC.ClientID%>").value = "";
                document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_SI.ClientID%>").value = "";
                document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_RATE.ClientID%>").value = "";
                document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_PREMIUM.ClientID%>").value = "";




                document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PERC.ClientID%>").readOnly = true;
                document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_PERC.ClientID%>").readOnly = true;
                document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_PERC.ClientID%>").readOnly = true;
                ValidatorEnable($("#<%= rfvFireBldgEscPerc.ClientID%>")[0], false);
                document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PERC.ClientID%>").className = "form-control";



                ValidatorEnable($("#<%= rfvFireLkgLmtIdemnity.ClientID%>")[0], false);
                document.getElementById("<%= FIRE__LEAKAGE_LMT_IDEMNITY.ClientID%>").className = "form-control";
                ValidatorEnable($("#<%= rfvFireLkgRate.ClientID%>")[0], false);
                document.getElementById("<%= FIRE__LEAKAGE_RATE.ClientID%>").className = "form-control";

            }
            else {

                document.getElementById("<%= FIRE__FIRE_BUILDINGS_ESCALATION_INFLATION.ClientID%>").disabled = false;
                document.getElementById("<%= FIRE__RD1_SI.ClientID%>").readOnly = false;
                document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value = "";
                document.getElementById("<%= RI_EXP__RI_BUILDING_RI_EXPOSURE.ClientID%>").value = "";

                document.getElementById("<%= RI_EXP__RI_INFLATION_RI_EXPOSURE.ClientID%>").value = "";
                document.getElementById("<%= RI_EXP__RI_BUILDING_MPL_PERC.ClientID%>").value = "";
                if (!($('#<%= FIRE__FOV_FLAT_PREM.ClientID%>')[0].checked)) {
                    document.getElementById("<%= FIRE__RD1_RATE.ClientID%>").value = (parseFloat(document.getElementById("<%= hvAgreedRateFinal.ClientID%>").value) || 0);
                }
                $('#<%= FIRE__IPF.ClientID%>')[0].disabled = true;
            }
            fnSum_Cal();
            fnPre_Cal();
            CalculateTotalSI();
        }

        function ToggleRentRD(flag, OnLoad) {

            document.getElementById("<%= FIRE__RD2_SI.ClientID%>").readOnly = !flag;
            document.getElementById("<%= FIRE__RD1_NO_OF_MONTHS.ClientID%>").readOnly = !flag;

            if (!flag) {
                if (OnLoad == "false") {
                    document.getElementById("<%= FIRE__RD2_SI.ClientID%>").value = "";
                    document.getElementById("<%= FIRE__RD2_RATE.ClientID%>").value = "";
                    document.getElementById("<%= FIRE__RD2_PREMIUM.ClientID%>").value = "";
                }
                fnSum_Cal();
                fnPre_Cal();
            }
            else {
                if (!($('#<%= FIRE__FOV_FLAT_PREM.ClientID%>')[0].checked)) {
                    document.getElementById("<%= FIRE__RD2_RATE.ClientID%>").value = (parseFloat(document.getElementById("<%= hvAgreedRateFinal.ClientID%>").value) || 0);
                }
            }
        }

        function ToggleBuildingsEscalation(flag, OnLoad) {

            document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PERC.ClientID%>").readOnly = !flag;
            document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_RATE.ClientID%>").readOnly = !flag;
            document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PREMIUM.ClientID%>").readOnly = !flag;

            document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_PERC.ClientID%>").readOnly = !flag;
            document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_RATE.ClientID%>").readOnly = !flag;
            document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_PREMIUM.ClientID%>").readOnly = !flag;

            document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_PERC.ClientID%>").readOnly = !flag;
            document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_RATE.ClientID%>").readOnly = !flag;
            document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_PREMIUM.ClientID%>").readOnly = !flag;

            if (flag) {

                if (OnLoad == "true") {

                    ValidatorEnable($("#<%= rfvFireBldgEscPerc.ClientID%>")[0], true);


                    document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PERC.ClientID%>").readOnly = false;
                    //document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_SI.ClientID%>").readOnly = true ;
                    document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_RATE.ClientID%>").readOnly = true;
                    document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PREMIUM.ClientID%>").readOnly = true;

                    document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_PERC.ClientID%>").readOnly = false;
                    //document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_SI.ClientID%>").readOnly = false;
                    document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_RATE.ClientID%>").readOnly = true;
                    document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_PREMIUM.ClientID%>").readOnly = true;

                    document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_PERC.ClientID%>").readOnly = false;
                    //document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_SI.ClientID%>").readOnly = false;
                    document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_RATE.ClientID%>").readOnly = true;
                    document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_PREMIUM.ClientID%>").readOnly = true;
                    document.getElementById("<%= RI_EXP__RI_IEP_RI_EXPOSURE.ClientID%>").value = ((parseFloat(document.getElementById("<%= RI_EXP__RI_IEP_RISK_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= RI_EXP__RI_IEP_MPL_PERC.ClientID%>").value) || 0)) * .01;

                }
            }
            else {
                if (OnLoad == "false") {
                    ValidatorEnable($("#<%= rfvFireBldgEscPerc.ClientID%>")[0], false);
                    document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PERC.ClientID%>").className = "form-control";



                    document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PERC.ClientID%>").value = "";
                    document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_SI.ClientID%>").value = "";
                    document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_RATE.ClientID%>").value = "";
                    document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PREMIUM.ClientID%>").value = "";

                    document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_PERC.ClientID%>").value = "";
                    document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_SI.ClientID%>").value = "";
                    document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_RATE.ClientID%>").value = "";
                    document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_PREMIUM.ClientID%>").value = "";

                    document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_PERC.ClientID%>").value = "";
                    document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_SI.ClientID%>").value = "";
                    document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_RATE.ClientID%>").value = "";
                    document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_PREMIUM.ClientID%>").value = "";

                    document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PERC.ClientID%>").className = "form-control";
                    document.getElementById("<%= RI_EXP__RI_IEP_RI_EXPOSURE.ClientID%>").value = "";
                }

            }

        }
        function ToggleCPC(flag) {
            document.getElementById("<%= FIRE__CLAIM_PREP_LMT_IDEMNITY.ClientID%>").readOnly = !flag;
            //document.getElementById("<%= FIRE__CLAIM_PREP_RATE.ClientID%>").readOnly = !flag;
            //document.getElementById("<%= FIRE__CLAIM_PREPCOST_PREM.ClientID%>").readOnly = !flag;    
            if (($('#<%= FIRE__FOV_FLAT_PREM.ClientID%>')[0].checked) && (flag)) {
                document.getElementById("<%= FIRE__CLAIM_PREPCOST_PREM.ClientID%>").readOnly = false;
            }
            else {
                document.getElementById("<%= FIRE__CLAIM_PREPCOST_PREM.ClientID%>").readOnly = true;
                document.getElementById("<%= FIRE__CLAIM_PREPCOST_PREM.ClientID%>").value = "";
            }
            if (!flag) {
                document.getElementById("<%= FIRE__CLAIM_PREP_LMT_IDEMNITY.ClientID%>").value = "";
                document.getElementById("<%= FIRE__CLAIM_PREP_RATE.ClientID%>").value = "";
                document.getElementById("<%= FIRE__CLAIM_PREPCOST_PREM.ClientID%>").value = "";
                document.getElementById("<%= RI_EXP__RI_CLAIMPREP_SI.ClientID%>").value = "";
                document.getElementById("<%= RI_EXP__RI_CLAIMPREP_RISK_SI.ClientID%>").value = "";
                document.getElementById("<%= RI_EXP__RI_CLAIMPREP_RISK_PERC.ClientID%>").value = "";
                document.getElementById("<%= RI_EXP__RI_CLAIMPREP_RISK_RI_EXPOSURE.ClientID%>").value = "";

                document.getElementById("<%= FIRE__CLAIM_PREP_RATE.ClientID%>").className = ""
                document.getElementById("<%= FIRE__CLAIM_PREP_LMT_IDEMNITY.ClientID%>").className = "";
                ValidatorEnable($("#<%= rfvFireClaimLmtIdemn.ClientID%>")[0], false);
                document.getElementById("<%= FIRE__CLAIM_PREP_LMT_IDEMNITY.ClientID%>").className = "form-control";
                document.getElementById("<%= FIRE__CLAIM_PREP_RATE.ClientID%>").className = "form-control";
            }
            else {
                document.getElementById("<%= FIRE__CLAIM_PREP_RATE.ClientID%>").value = (parseFloat(document.getElementById("<%= hvAgreedRateFinal.ClientID%>").value) || 0);
                ValidatorEnable($("#<%= rfvClaimPrepRate.ClientID%>")[0], true);
                ValidatorEnable($("#<%= rfvFireClaimLmtIdemn.ClientID%>")[0], true);
                document.getElementById("<%= FIRE__CLAIM_PREP_LMT_IDEMNITY.ClientID%>").className = "form-control field-mandatory";
                document.getElementById("<%= FIRE__CLAIM_PREP_RATE.ClientID%>").className = "form-control field-mandatory";
            }
        }

        function ToggleDS(flag) {
            if (($('#<%= FIRE__FOV_FLAT_PREM.ClientID%>')[0].checked) && (flag)) {
                document.getElementById("<%= FIRE__DISPOSAL_SALV_PREM.ClientID%>").readOnly = false;
                document.getElementById("<%= FIRE__DISPOSAL_SALV_PREM.ClientID%>").className = "form-control field-mandatory";
            }
            else {
                document.getElementById("<%= FIRE__DISPOSAL_SALV_PREM.ClientID%>").readOnly = true;
                document.getElementById("<%= FIRE__DISPOSAL_SALV_PREM.ClientID%>").value = "";
                document.getElementById("<%= FIRE__DISPOSAL_SALV_PREM.ClientID%>").className = "form-control";
            }
        }

        function ToggleLeakage(flag) {
            document.getElementById("<%= FIRE__LEAKAGE_LMT_IDEMNITY.ClientID%>").readOnly = !flag;
            //document.getElementById("<%= FIRE__LEAKAGE_RATE.ClientID%>").readOnly = !flag;
            //document.getElementById("<%= FIRE__LEAKAGE_PREM.ClientID%>").readOnly = !flag;
            if (($('#<%= FIRE__FOV_FLAT_PREM.ClientID%>')[0].checked) && (flag)) {
                document.getElementById("<%= FIRE__LEAKAGE_PREM.ClientID%>").readOnly = false;
                }
                else {
                    document.getElementById("<%= FIRE__LEAKAGE_PREM.ClientID%>").readOnly = true;
                    document.getElementById("<%= FIRE__LEAKAGE_PREM.ClientID%>").value = "";
                }
                if (!flag) {
                    document.getElementById("<%= FIRE__LEAKAGE_LMT_IDEMNITY.ClientID%>").className = "";
                    document.getElementById("<%= FIRE__LEAKAGE_RATE.ClientID%>").className = "";
                    document.getElementById("<%= FIRE__LEAKAGE_LMT_IDEMNITY.ClientID%>").value = "";
                    document.getElementById("<%= FIRE__LEAKAGE_RATE.ClientID%>").value = "";
                    ValidatorEnable($("#<%= rfvFireLkgLmtIdemnity.ClientID%>")[0], false);
                    document.getElementById("<%= FIRE__LEAKAGE_LMT_IDEMNITY.ClientID%>").className = "form-control";
                    ValidatorEnable($("#<%= rfvFireLkgRate.ClientID%>")[0], false);
                    document.getElementById("<%= FIRE__LEAKAGE_RATE.ClientID%>").className = "form-control";
                }
                else {
                    document.getElementById("<%= FIRE__LEAKAGE_RATE.ClientID%>").value = (parseFloat(document.getElementById("<%= hvAgreedRateFinal.ClientID%>").value) || 0);
                    ValidatorEnable($("#<%= rfvFireLkgLmtIdemnity.ClientID%>")[0], true);
                    ValidatorEnable($("#<%= rfvFireLkgRate.ClientID%>")[0], true);
                    document.getElementById("<%= FIRE__LEAKAGE_LMT_IDEMNITY.ClientID%>").className = "form-control field-mandatory";
                    document.getElementById("<%= FIRE__LEAKAGE_RATE.ClientID%>").className = "form-control field-mandatory";
                }
            }


            function ToggleRiotStrike(flag) {
                if (($('#<%= FIRE__FOV_FLAT_PREM.ClientID%>')[0].checked) && (flag)) {
                    document.getElementById("<%= FIRE__RIOT_STRIKE_PREM.ClientID%>").readOnly = false;
                    document.getElementById("<%= FIRE__RIOT_STRIKE_PREM.ClientID%>").className = "form-control field-mandatory";
                }
                else {
                    document.getElementById("<%= FIRE__RIOT_STRIKE_PREM.ClientID%>").readOnly = true;
                    document.getElementById("<%= FIRE__RIOT_STRIKE_PREM.ClientID%>").value = "";
                    document.getElementById("<%= FIRE__RIOT_STRIKE_PREM.ClientID%>").className = "form-control";
                }
            }

            function ToggleSL(flag) {
                document.getElementById("<%= FIRE__LANDSLIP_LMT_IDEMNITY.ClientID%>").readOnly = !flag;
                //document.getElementById("<%= FIRE__LANDSLIP_RATE.ClientID%>").readOnly = !flag;
                document.getElementById("<%= FIRE__LANDSLIP_TOTAL_SI.ClientID%>").readOnly = !flag;
                //document.getElementById("<%= FIRE__LANDSLIP_PREM.ClientID%>").readOnly = !flag;
                if (($('#<%= FIRE__FOV_FLAT_PREM.ClientID%>')[0].checked) && (flag)) {
                    document.getElementById("<%= FIRE__LANDSLIP_PREM.ClientID%>").readOnly = false;
                }
                else {
                    document.getElementById("<%= FIRE__LANDSLIP_PREM.ClientID%>").readOnly = true;
                    document.getElementById("<%= FIRE__LANDSLIP_PREM.ClientID%>").value = "";
                }
                if (!flag) {
                    document.getElementById("<%= FIRE__LANDSLIP_LMT_IDEMNITY.ClientID%>").className = "";
                    document.getElementById("<%= FIRE__LANDSLIP_RATE.ClientID%>").className = "";
                    document.getElementById("<%= FIRE__LANDSLIP_LMT_IDEMNITY.ClientID%>").value = "";
                    document.getElementById("<%= FIRE__LANDSLIP_RATE.ClientID%>").value = "";
                    ValidatorEnable($("#<%= rfvFireLandLmtIdemn.ClientID%>")[0], false);
                    document.getElementById("<%= FIRE__LANDSLIP_LMT_IDEMNITY.ClientID%>").className = "form-control";
                    ValidatorEnable($("#<%= rfvFireLndSlipRate.ClientID%>")[0], false);
                    document.getElementById("<%= FIRE__LANDSLIP_RATE.ClientID%>").className = "form-control";
                }
                else {
                    document.getElementById("<%= FIRE__LANDSLIP_RATE.ClientID%>").value = (parseFloat(document.getElementById("<%= hvAgreedRateFinal.ClientID%>").value) || 0);
                    ValidatorEnable($("#<%= rfvFireLandLmtIdemn.ClientID%>")[0], true);
                    ValidatorEnable($("#<%= rfvFireLndSlipRate.ClientID%>")[0], true);
                    document.getElementById("<%= FIRE__LANDSLIP_LMT_IDEMNITY.ClientID%>").className = "form-control field-mandatory";
                    document.getElementById("<%= FIRE__LANDSLIP_RATE.ClientID%>").className = "form-control field-mandatory";
                }
            }



            function calculat_eesc_si(ctrl) {
                var ctrl_SI = ctrl + "_SI";
                var ctrl_RISK_SI = ctrl + "_RISK_SI";
                var ctrl_MPL_PERC = ctrl + "MPL_PERC";
                var ctrl_RI_EXPOSURE = ctrl + "RI_EXPOSURE";

                if (($('#<%= FIRE__FIRE_BUILDINGS_ESCALATION_INFLATION.ClientID%>')[0].checked)) {

                }

            }

            function calculate_builder(ctrl) {
                var FIRE__RD1_BUILDINGS_check = $('#<%= FIRE__RD1_BUILDINGS.ClientID%>')[0].checked;
                var FIRE__RD1_RENT_check = $('#<%= FIRE__RD1_RENT.ClientID%>')[0].checked;
                var FIRE__Bldg_ESCINFL_check = $('#<%= FIRE__FIRE_BUILDINGS_ESCALATION_INFLATION.ClientID%>')[0].checked;
                var FIRE__EF_CLAIM_PREP_COST_Check = $('#<%= FIRE__EF_CLAIM_PREP_COST.ClientID%>')[0].checked;
                var FIRE__RD1_MULTIPLE_BUILDINGS_Check = $('#<%= FIRE__RD1_MULTIPLE_BUILDINGS.ClientID%>')[0].checked;


                document.getElementById("<%= RI_EXP__RI_BUILDING_SI.ClientID%>").readOnly = true;
                document.getElementById("<%= RI_EXP__RI_BUILDING_RISK_SI.ClientID%>").readOnly = !FIRE__RD1_BUILDINGS_check;
                document.getElementById("<%= RI_EXP__RI_BUILDING_MPL_PERC.ClientID%>").readOnly = !FIRE__RD1_BUILDINGS_check;
                //document.getElementById("<%= RI_EXP__RI_BUILDING_RI_EXPOSURE.ClientID%>").readOnly = !FIRE__RD1_BUILDINGS_check;

                document.getElementById("<%= RI_EXP__RI_RENT_SI.ClientID%>").readOnly = true;
                document.getElementById("<%= RI_EXP__RI_RENT_RISK_SI.ClientID%>").readOnly = !FIRE__RD1_RENT_check;
                document.getElementById("<%= RI_EXP__RI_RENT_MPL_PERC.ClientID%>").readOnly = !FIRE__RD1_RENT_check;
                document.getElementById("<%= RI_EXP__RI_RENT_RI_EXPOSURE.ClientID%>").readOnly = true;

                //Building  SI
                if (FIRE__RD1_BUILDINGS_check) {

                    //  if (ctrl == "FIRE__RD1_SI") {
                    if (!($('#<%= FIRE__FOV_FLAT_PREM.ClientID%>')[0].checked)) {
                        document.getElementById("<%= FIRE__RD1_PREMIUM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= FIRE__RD1_RATE.ClientID%>").value) || 0) * .01).toFixed(2);
                    }

                    if (FIRE__RD1_MULTIPLE_BUILDINGS_Check) {
                        document.getElementById("<%= RI_EXP__RI_BUILDING_SI.ClientID%>").value = document.getElementById("<%= hvBuildingTotalSI.ClientID%>").value;
                        document.getElementById("<%= RI_EXP__RI_BUILDING_RISK_SI.ClientID%>").value = document.getElementById("<%= hvBuildingTotalSI.ClientID%>").value;
                        document.getElementById("<%= RI_EXP__RI_INFLATION_SI.ClientID%>").value = document.getElementById("<%= hvInfEscTotalSI.ClientID%>").value;
                        document.getElementById("<%= RI_EXP__RI_INFLATION_RISK_SI.ClientID%>").value = document.getElementById("<%= hvInfEscTotalSI.ClientID%>").value;
                        if (document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value != "") {
                            document.getElementById("<%= FIRE__RD1_RATE.ClientID%>").value = (((parseFloat(document.getElementById("<%= FIRE__RD1_PREMIUM.ClientID%>").value) || 0) * 100) / (parseFloat(document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value) || 0)).toFixed(4);
                        }
                    } else {
                        document.getElementById("<%= RI_EXP__RI_BUILDING_SI.ClientID%>").value = document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value;
                        document.getElementById("<%= RI_EXP__RI_BUILDING_RISK_SI.ClientID%>").value = document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value;
                        if (!($('#<%= FIRE__FOV_FLAT_PREM.ClientID%>')[0].checked)) {
                            document.getElementById("<%= FIRE__RD1_RATE.ClientID%>").value = (parseFloat(document.getElementById("<%= hvAgreedRateFinal.ClientID%>").value) || 0);
                        }
                    }

                    if (document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value == "") {
                        document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value = "100";
                }
                document.getElementById("<%= RI_EXP__RI_INFLATION_RI_EXPOSURE.ClientID%>").value = (parseFloat(document.getElementById("<%= RI_EXP__RI_INFLATION_RISK_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value) || 0) * .01;

                    if (document.getElementById("<%= RI_EXP__RI_BUILDING_MPL_PERC.ClientID%>").value == "") {
                        document.getElementById("<%= RI_EXP__RI_BUILDING_MPL_PERC.ClientID%>").value = "100";
                    }

                    document.getElementById("<%= RI_EXP__RI_BUILDING_RI_EXPOSURE.ClientID%>").value = (parseFloat(document.getElementById("<%= RI_EXP__RI_BUILDING_RISK_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= RI_EXP__RI_BUILDING_MPL_PERC.ClientID%>").value) || 0) * .01;

                    //}
                    if ((ctrl == "RI_EXP__RI_BUILDING_RISK_SI") || (ctrl == "RI_EXP__RI_BUILDING_MPL_PERC")) {

                        // document.getElementById("<%= FIRE__RD1_PREMIUM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= FIRE__RD1_RATE.ClientID%>").value) || 0).toFixed(2));

                        if (FIRE__RD1_MULTIPLE_BUILDINGS_Check) {
                            document.getElementById("<%= RI_EXP__RI_BUILDING_SI.ClientID%>").value = document.getElementById("<%= hvBuildingTotalSI.ClientID%>").value;
                            document.getElementById("<%= RI_EXP__RI_BUILDING_RISK_SI.ClientID%>").value = document.getElementById("<%= hvBuildingTotalSI.ClientID%>").value;
                            document.getElementById("<%= RI_EXP__RI_INFLATION_SI.ClientID%>").value = document.getElementById("<%= hvInfEscTotalSI.ClientID%>").value;
                            document.getElementById("<%= RI_EXP__RI_INFLATION_RISK_SI.ClientID%>").value = document.getElementById("<%= hvInfEscTotalSI.ClientID%>").value;

                        } else {
                            document.getElementById("<%= RI_EXP__RI_BUILDING_SI.ClientID%>").value = document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value;
                            document.getElementById("<%= RI_EXP__RI_BUILDING_RISK_SI.ClientID%>").value = document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value;
                        }
                        if (document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value == "") {
                            document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value = "100";
                    }
                    document.getElementById("<%= RI_EXP__RI_INFLATION_RI_EXPOSURE.ClientID%>").value = (parseFloat(document.getElementById("<%= RI_EXP__RI_INFLATION_RISK_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value) || 0) * .01;

                        if (document.getElementById("<%= RI_EXP__RI_BUILDING_MPL_PERC.ClientID%>").value == "") {
                            document.getElementById("<%= RI_EXP__RI_BUILDING_MPL_PERC.ClientID%>").value = "100";
                        }
                        document.getElementById("<%= RI_EXP__RI_BUILDING_RI_EXPOSURE.ClientID%>").value = (parseFloat(parseFloat(cleanNumber(document.getElementById("<%= RI_EXP__RI_BUILDING_RISK_SI.ClientID%>").value)) * parseFloat(document.getElementById("<%= RI_EXP__RI_BUILDING_MPL_PERC.ClientID%>").value) * .01)).toFixed(2);
                    }
                    if (ctrl == "FIRE__RD1_PREMIUM") {
                        if ((document.getElementById("<%= FIRE__RD1_PREMIUM.ClientID%>").value != '') && (document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value != '')) {
                            document.getElementById("<%= FIRE__RD1_RATE.ClientID%>").value = (((parseFloat(document.getElementById("<%= FIRE__RD1_PREMIUM.ClientID%>").value) || 0) * 100) / (parseFloat(document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value) || 0)).toFixed(4);
                        }
                    }
                }
                else {
                    document.getElementById("<%= RI_EXP__RI_BUILDING_SI.ClientID%>").value = "";
                    document.getElementById("<%= RI_EXP__RI_BUILDING_RISK_SI.ClientID%>").value = "";
                    document.getElementById("<%= RI_EXP__RI_BUILDING_MPL_PERC.ClientID%>").value = "";
                    document.getElementById("<%= RI_EXP__RI_BUILDING_RI_EXPOSURE.ClientID%>").value = "";
                }
                //Premium RD


                //Rent SI
                if (FIRE__RD1_RENT_check) {

                    if (!($('#<%= FIRE__FOV_FLAT_PREM.ClientID%>')[0].checked)) {
                        document.getElementById("<%= FIRE__RD2_PREMIUM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= FIRE__RD2_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= FIRE__RD2_RATE.ClientID%>").value) || 0) * .01).toFixed(2);
                    }

                    if (ctrl == "FIRE__RD2_SI") {
                        //document.getElementById("<%= FIRE__RD2_PREMIUM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= FIRE__RD2_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= FIRE__RD2_RATE.ClientID%>").value) || 0) * .01).toFixed(2);
                        document.getElementById("<%= RI_EXP__RI_RENT_SI.ClientID%>").value = document.getElementById("<%= FIRE__RD2_SI.ClientID%>").value;
                        document.getElementById("<%= RI_EXP__RI_RENT_RISK_SI.ClientID%>").value = document.getElementById("<%= FIRE__RD2_SI.ClientID%>").value;
                        document.getElementById("<%= RI_EXP__RI_RENT_MPL_PERC.ClientID%>").value = "100";
                        document.getElementById("<%= RI_EXP__RI_RENT_RI_EXPOSURE.ClientID%>").value = (parseFloat(parseFloat(cleanNumber(document.getElementById("<%= RI_EXP__RI_RENT_RISK_SI.ClientID%>").value)) * parseFloat(document.getElementById("<%= RI_EXP__RI_RENT_MPL_PERC.ClientID%>").value) * .01)).toFixed(2);
                    }
                    else if ((ctrl == "RI_EXP__RI_RENT_RISK_SI") || (ctrl == "RI_EXP__RI_RENT_MPL_PERC")) {

                        //document.getElementById("<%= FIRE__RD2_PREMIUM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= FIRE__RD2_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= FIRE__RD2_RATE.ClientID%>").value) || 0) * .01).toFixed(2);
                    document.getElementById("<%= RI_EXP__RI_RENT_SI.ClientID%>").value = document.getElementById("<%= FIRE__RD2_SI.ClientID%>").value;
                    document.getElementById("<%= RI_EXP__RI_RENT_RI_EXPOSURE.ClientID%>").value = (parseFloat(parseFloat(cleanNumber(document.getElementById("<%= RI_EXP__RI_RENT_RISK_SI.ClientID%>").value)) * parseFloat(document.getElementById("<%= RI_EXP__RI_RENT_MPL_PERC.ClientID%>").value) * .01)).toFixed(2);
                }

            if (ctrl == "FIRE__RD2_PREMIUM") {

                if ((document.getElementById("<%= FIRE__RD2_PREMIUM.ClientID%>").value != '') && (document.getElementById("<%= FIRE__RD2_SI.ClientID%>").value != '')) {
                    document.getElementById("<%= FIRE__RD2_RATE.ClientID%>").value = (((parseFloat(document.getElementById("<%= FIRE__RD2_PREMIUM.ClientID%>").value) || 0) * 100) / (parseFloat(document.getElementById("<%= FIRE__RD2_SI.ClientID%>").value) || 0)).toFixed(4);
                }
            }

        }
        else {
            document.getElementById("<%= RI_EXP__RI_RENT_SI.ClientID%>").value = "";
                    document.getElementById("<%= RI_EXP__RI_RENT_RISK_SI.ClientID%>").value = "";
                    document.getElementById("<%= RI_EXP__RI_RENT_MPL_PERC.ClientID%>").value = "";
                    document.getElementById("<%= RI_EXP__RI_RENT_RI_EXPOSURE.ClientID%>").value = "";
                    document.getElementById("<%= FIRE__RD1_NO_OF_MONTHS.ClientID%>").value = "";
                }
                //CPC N
                if (FIRE__EF_CLAIM_PREP_COST_Check) {
                    if ((ctrl == "RI_EXP__RI_CLAIMPREP_RISK_SI") || (ctrl == "RI_EXP__RI_CLAIMPREP_RISK_PERC")) {
                        document.getElementById("<%= RI_EXP__RI_CLAIMPREP_RISK_RI_EXPOSURE.ClientID%>").value = ((parseFloat(document.getElementById("<%= RI_EXP__RI_CLAIMPREP_RISK_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= RI_EXP__RI_CLAIMPREP_RISK_PERC.ClientID%>").value) || 0)) * .01;
                    }

                }

                //Escalation Reinsurance Exposure
                if (FIRE__Bldg_ESCINFL_check) {

                    if (document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PERC.ClientID%>").value != "") { CalculateEscSI(); }
                if (document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_PERC.ClientID%>").value != "") { CalculateInfOneSI(); }
                if (document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_PERC.ClientID%>").value != "") { CalculateInfTwoSI(); }



                if (ctrl == "FIRE__BUILDINGS_ESCALATION_RATE") {
                    document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PREMIUM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_RATE.ClientID%>").value) || 0) * .01).toFixed(2);

                }
                if (ctrl == "FIRE__BUILDINGS_INFL_1YR_RATE") {
                    document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_PREMIUM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_RATE.ClientID%>").value) || 0) * .01).toFixed(2);

                }
                if (ctrl == "FIRE__BUILDINGS_INFL_2YR_RATE") {
                    document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_PREMIUM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_RATE.ClientID%>").value) || 0) * .01).toFixed(2);

                }
                document.getElementById("<%= RI_EXP__RI_INFLATION_SI.ClientID%>").value = (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_SI.ClientID%>").value) || 0);
                document.getElementById("<%= RI_EXP__RI_INFLATION_RISK_SI.ClientID%>").value = (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_SI.ClientID%>").value) || 0);

                if (document.getElementById("<%= RI_EXP__RI_INFLATION_SI.ClientID%>").value != "") {

                    if (document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value == "") {
                        document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value = "100";
                    }
                    document.getElementById("<%= RI_EXP__RI_INFLATION_RI_EXPOSURE.ClientID%>").value = (parseFloat(document.getElementById("<%= RI_EXP__RI_INFLATION_RISK_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value) || 0) * .01;

                }

            }
            else {
                document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PREMIUM.ClientID%>").value = "";
                document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_PREMIUM.ClientID%>").value = "";
                document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_PREMIUM.ClientID%>").value = "";
                if (!FIRE__RD1_MULTIPLE_BUILDINGS_Check) {
                    document.getElementById("<%= RI_EXP__RI_INFLATION_SI.ClientID%>").value = "";
                    document.getElementById("<%= RI_EXP__RI_INFLATION_RISK_SI.ClientID%>").value = "";
                    document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value = "";
                    document.getElementById("<%= RI_EXP__RI_INFLATION_RI_EXPOSURE.ClientID%>").value = "";
                }
            }
            fnSum_Cal();
            fnPre_Cal();
            CalculateTotalSI();
        }

        function CalculateEscSI() {

            if (($('#<%= FIRE__FIRE_BUILDINGS_ESCALATION_INFLATION.ClientID%>')[0].checked)) {

                document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_SI.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PERC.ClientID%>").value) || 0) * .01).toFixed(2);
                document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_RATE.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= FIRE__RD1_RATE.ClientID%>").value) || 0) * .5).toFixed(4);
                document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PREMIUM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_RATE.ClientID%>").value) || 0) * .01).toFixed(2);

                document.getElementById("<%= RI_EXP__RI_BUILDING_RI_EXPOSURE.ClientID%>").value = (parseFloat(parseFloat(cleanNumber(document.getElementById("<%= RI_EXP__RI_BUILDING_MPL_PERC.ClientID%>").value)) * parseFloat(document.getElementById("<%= RI_EXP__RI_BUILDING_RISK_SI.ClientID%>").value) * .01)).toFixed(2);

                fnSum_Cal();
                fnPre_Cal();
            }
            if (($('#<%= FIRE__RD1_BUILDINGS.ClientID%>')[0].checked)) {
                document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PREMIUM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_RATE.ClientID%>").value) || 0) * .01).toFixed(2);
                if (($('#<%= FIRE__RD1_MULTIPLE_BUILDINGS.ClientID%>')[0].checked)) {
                    document.getElementById("<%= RI_EXP__RI_BUILDING_SI.ClientID%>").value = document.getElementById("<%= hvBuildingTotalSI.ClientID%>").value;

                    document.getElementById("<%= RI_EXP__RI_INFLATION_SI.ClientID%>").value = document.getElementById("<%= hvInfEscTotalSI.ClientID%>").value;

                    document.getElementById("<%= RI_EXP__RI_INFLATION_RISK_SI.ClientID%>").value = document.getElementById("<%= hvInfEscTotalSI.ClientID%>").value;
                    document.getElementById("<%= RI_EXP__RI_BUILDING_RISK_SI.ClientID%>").value = document.getElementById("<%= hvBuildingTotalSI.ClientID%>").value;
                } else {
                    document.getElementById("<%= RI_EXP__RI_BUILDING_SI.ClientID%>").value = document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value;
                    document.getElementById("<%= RI_EXP__RI_BUILDING_RISK_SI.ClientID%>").value = document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value;
                    document.getElementById("<%= RI_EXP__RI_INFLATION_SI.ClientID%>").value = (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_SI.ClientID%>").value) || 0);
                    document.getElementById("<%= RI_EXP__RI_INFLATION_RISK_SI.ClientID%>").value = (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_SI.ClientID%>").value) || 0);


                }

            }
            if (document.getElementById("<%= RI_EXP__RI_INFLATION_SI.ClientID%>").value != "") {

                if (document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value == "") {
                    document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value = "100";
                }
                document.getElementById("<%= RI_EXP__RI_INFLATION_RI_EXPOSURE.ClientID%>").value = (parseFloat(document.getElementById("<%= RI_EXP__RI_INFLATION_RISK_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value) || 0) * .01;

            }
            if (($('#<%= FIRE__RD1_RENT.ClientID%>')[0].checked)) {
                //document.getElementById("<%= FIRE__RD2_PREMIUM.ClientID%>").value = (parseFloat(document.getElementById("<%= FIRE__RD2_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= FIRE__RD2_RATE.ClientID%>").value) || 0);
                document.getElementById("<%= RI_EXP__RI_RENT_SI.ClientID%>").value = document.getElementById("<%= FIRE__RD2_SI.ClientID%>").value;
                document.getElementById("<%= RI_EXP__RI_RENT_RISK_SI.ClientID%>").value = document.getElementById("<%= FIRE__RD2_SI.ClientID%>").value;
                document.getElementById("<%= RI_EXP__RI_RENT_RI_EXPOSURE.ClientID%>").value = (parseFloat(cleanNumber(document.getElementById("<%= RI_EXP__RI_RENT_RISK_SI.ClientID%>").value) * document.getElementById("<%= RI_EXP__RI_RENT_MPL_PERC.ClientID%>").value * .01)).toFixed(2);



            }

            if (document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value != "") {

                document.getElementById("<%= RI_EXP__RI_BUILDING_SI.ClientID%>").readOnly = false;
                document.getElementById("<%= RI_EXP__RI_RENT_SI.ClientID%>").readOnly = false;
                document.getElementById("<%= RI_EXP__RI_IEP_SI.ClientID%>").readOnly = false;
            }
            if (document.getElementById("<%= RI_EXP__TOTAL_FIRE_RI_SI.ClientID%>").value != "") {

                document.getElementById("<%= RI_EXP__RI_IEP_SI.ClientID%>").readOnly = false;
           }

           CalculateTotalSI();


       }

       function CalculateInfOneSI() {

           if (($('#<%= FIRE__FIRE_BUILDINGS_ESCALATION_INFLATION.ClientID%>')[0].checked)) {
               document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_SI.ClientID%>").value = (parseFloat((parseFloat(document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_SI.ClientID%>").value) || 0)) * (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_PERC.ClientID%>").value) || 0) * .01).toFixed(2);
               document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_RATE.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= FIRE__RD1_RATE.ClientID%>").value) || 0) * .25).toFixed(4);
               document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_PREMIUM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_RATE.ClientID%>").value) || 0) * .01).toFixed(2);
               fnSum_Cal();
               fnPre_Cal();
               document.getElementById("<%= RI_EXP__RI_INFLATION_SI.ClientID%>").value = (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_SI.ClientID%>").value) || 0);
                document.getElementById("<%= RI_EXP__RI_INFLATION_RISK_SI.ClientID%>").value = (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_SI.ClientID%>").value) || 0);

               if (document.getElementById("<%= RI_EXP__RI_INFLATION_SI.ClientID%>").value != "") {

                   if (document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value == "") {
                        document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value = "100";
                    }
                    document.getElementById("<%= RI_EXP__RI_INFLATION_RI_EXPOSURE.ClientID%>").value = (parseFloat(document.getElementById("<%= RI_EXP__RI_INFLATION_RISK_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value) || 0) * .01;

                }
            }
        }

        function CalculateInfTwoSI() {

            if (($('#<%= FIRE__FIRE_BUILDINGS_ESCALATION_INFLATION.ClientID%>')[0].checked)) {
                document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_SI.ClientID%>").value = (parseFloat((parseFloat(document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_SI.ClientID%>").value) || 0)) * (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_PERC.ClientID%>").value) || 0) * .01).toFixed(2);
                document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_RATE.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= FIRE__RD1_RATE.ClientID%>").value) || 0) * .25).toFixed(4);
                document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_PREMIUM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_RATE.ClientID%>").value) || 0) * .01).toFixed(2);
                fnSum_Cal();
                fnPre_Cal();
                document.getElementById("<%= RI_EXP__RI_INFLATION_SI.ClientID%>").value = (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_SI.ClientID%>").value) || 0);
                document.getElementById("<%= RI_EXP__RI_INFLATION_RISK_SI.ClientID%>").value = (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_SI.ClientID%>").value) || 0);

                if (document.getElementById("<%= RI_EXP__RI_INFLATION_SI.ClientID%>").value != "") {

                    if (document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value == "") {
                        document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value = "100";
                    }
                    document.getElementById("<%= RI_EXP__RI_INFLATION_RI_EXPOSURE.ClientID%>").value = (parseFloat(document.getElementById("<%= RI_EXP__RI_INFLATION_RISK_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= RI_EXP__RI_INFLATION_MPL_PERC.ClientID%>").value) || 0) * .01;

                }
            }
        }

        function CalculateTotalSI() {

            document.getElementById("<%= RI_EXP__TOTAL_FIRE_RI_SI.ClientID%>").value = (parseFloat(document.getElementById("<%= RI_EXP__RI_BUILDING_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= RI_EXP__RI_INFLATION_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= RI_EXP__RI_PEMC_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= RI_EXP__RI_IEP_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= RI_EXP__RI_STOCK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= RI_EXP__RI_RENT_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= RI_EXP__RI_CLAIMPREP_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= RI_EXP__RI_MSC_ITEMS_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= RI_EXP__PM_STOCK_SI.ClientID%>").value) || 0);
            document.getElementById("<%= RI_EXP__TOTAL_FIRE_RI_RISK_S.ClientID%>").value = (parseFloat(document.getElementById("<%= RI_EXP__RI_BUILDING_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= RI_EXP__RI_INFLATION_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= RI_EXP__RI_PEMC_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= RI_EXP__RI_IEP_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= RI_EXP__RI_STOCK_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= RI_EXP__RI_RENT_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= RI_EXP__RI_CLAIMPREP_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= RI_EXP__RI_MSC_ITEMS_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= RI_EXP__PM_STOCK_RISK_SI.ClientID%>").value) || 0);
            document.getElementById("<%= RI_EXP__TOTAL_FIRE_RI_EXPOSURE.ClientID%>").value = (parseFloat(cleanNumber((document.getElementById("<%= RI_EXP__RI_BUILDING_RI_EXPOSURE.ClientID%>").value)) || 0)) +
                                                                                             (parseFloat(cleanNumber(document.getElementById("<%= RI_EXP__RI_INFLATION_RI_EXPOSURE.ClientID%>").value)) || 0) +
                                                                                             (parseFloat(cleanNumber(document.getElementById("<%= RI_EXP__RI_PEMC_RI_EXPOSURE.ClientID%>").value)) || 0) +
                                                                                             (parseFloat(cleanNumber(document.getElementById("<%= RI_EXP__RI_IEP_RI_EXPOSURE.ClientID%>").value)) || 0) +
                                                                                             (parseFloat(cleanNumber(document.getElementById("<%= RI_EXP__RI_STOCK_RI_EXPOSURE.ClientID%>").value)) || 0) +
                                                                                             (parseFloat(cleanNumber(document.getElementById("<%= RI_EXP__RI_RENT_RI_EXPOSURE.ClientID%>").value)) || 0) +
                                                                                             (parseFloat(cleanNumber(document.getElementById("<%= RI_EXP__RI_CLAIMPREP_RISK_RI_EXPOSURE.ClientID%>").value)) || 0) +
                                                                                             (parseFloat(cleanNumber(document.getElementById("<%= RI_EXP__RI_MSC_ITEMS_RI_EXPOSURE.ClientID%>").value)) || 0) +
                                                                                             (parseFloat(cleanNumber(document.getElementById("<%= RI_EXP__PM_STOCK_RI_EXPOSURE.ClientID%>").value)) || 0);

        }
        function CalculateLIClaimPPrem() {

            var FIRE__EF_CLAIM_PREP_COST = false;
            FIRE__EF_CLAIM_PREP_COST = $('#<%= FIRE__EF_CLAIM_PREP_COST.ClientID%>')[0].checked;

            //CPC   
            if (FIRE__EF_CLAIM_PREP_COST) {
                document.getElementById("<%= FIRE__CLAIM_PREPCOST_PREM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= FIRE__CLAIM_PREP_LMT_IDEMNITY.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= FIRE__CLAIM_PREP_RATE.ClientID%>").value) || 0) * .01).toFixed(2);
                document.getElementById("<%= RI_EXP__RI_CLAIMPREP_SI.ClientID%>").value = document.getElementById("<%= FIRE__CLAIM_PREP_LMT_IDEMNITY.ClientID%>").value;
                document.getElementById("<%= RI_EXP__RI_CLAIMPREP_RISK_SI.ClientID%>").value = document.getElementById("<%= FIRE__CLAIM_PREP_LMT_IDEMNITY.ClientID%>").value;
                document.getElementById("<%= RI_EXP__RI_CLAIMPREP_RISK_PERC.ClientID%>").value = "100";
                document.getElementById("<%= RI_EXP__RI_CLAIMPREP_RISK_RI_EXPOSURE.ClientID%>").value = parseFloat(document.getElementById("<%= RI_EXP__RI_CLAIMPREP_RISK_SI.ClientID%>").value) * parseFloat(document.getElementById("<%= RI_EXP__RI_CLAIMPREP_RISK_PERC.ClientID%>").value) * .01;

            }
            else {
                document.getElementById("<%= RI_EXP__RI_CLAIMPREP_SI.ClientID%>").value = "";
                document.getElementById("<%= RI_EXP__RI_CLAIMPREP_RISK_SI.ClientID%>").value = "";
                document.getElementById("<%= RI_EXP__RI_CLAIMPREP_RISK_PERC.ClientID%>").value = "";
                document.getElementById("<%= RI_EXP__RI_CLAIMPREP_RISK_RI_EXPOSURE.ClientID%>").value = "";
            }
            CalculateTotalSI();

        }

        function CalculateLeakPPrem() {

            if (($('#<%= FIRE__EF_LEAKAGE.ClientID%>')[0].checked)) {
                document.getElementById("<%= FIRE__LEAKAGE_PREM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= FIRE__LEAKAGE_LMT_IDEMNITY.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= FIRE__LEAKAGE_RATE.ClientID%>").value) || 0) * .01).toFixed(2);
            }
        }

        function CalculateSLandPrem() {

            if (($('#<%= FIRE__EF_SUBSIDENCE_LANDSLIP.ClientID%>')[0].checked)) {
                document.getElementById("<%= FIRE__LANDSLIP_PREM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= FIRE__LANDSLIP_RATE.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= FIRE__LANDSLIP_LMT_IDEMNITY.ClientID%>").value) || 0) * .01).toFixed(2);
            }
        }

        function CalculateFireSIPrem() {
            if (($('#<%= FIRE__FOV_FLAT_PREM.ClientID%>')[0].checked)) {
                document.getElementById("<%= FIRE__FOV_TOTAL_SI.ClientID%>").value = (parseFloat(document.getElementById("<%= FIRE__RD1_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__RD2_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_SI.ClientID%>").value) || 0);
                document.getElementById("<%= FIRE__RD1_PREMIUM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= FIRE__RD1_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__RD2_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_ESCALATION_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_1YR_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= FIRE__BUILDINGS_INFL_2YR_PREMIUM.ClientID%>").value) || 0)).toFixed(2);
            }
        }
        function SetNoofMonthsDefault() {
            if (document.getElementById("<%= FIRE__RD1_NO_OF_MONTHS.ClientID%>").value == "" && document.getElementById("<%= FIRE__RD1_NO_OF_MONTHS.ClientID%>").readOnly == false) {
                document.getElementById("<%= FIRE__RD1_NO_OF_MONTHS.ClientID%>").value = "36";
            }
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
    </script>
    <div class="risk-screen">


        <NexusControl:ProgressBar ID="ucProgressBar" runat="server"></NexusControl:ProgressBar>
        <div class="card">
            <Nexus:tabindex id="ctrlTabIndex" runat="server" cssclass="TabContainer" tabcontainerclass="page-progress" activetabclass="ActiveTab" disabledclass="DisabledTab" scrollable="false"></Nexus:tabindex>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <asp:HiddenField ID="hvAgreedRateFinal" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hvmultibulpre" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hvmultibulsi" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hvBuildingTotalSI" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hvInfEscTotalSI" runat="server"></asp:HiddenField>
                    <legend>FIRE</legend>


                    <div id="liFIRE__FOV_FLAT_PREM" class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Flat Premium</label><div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="FIRE__FOV_FLAT_PREM" runat="server" onclick="ToggleFire(this.checked);" Text=" " CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>
                    <div id="liFIRE__FOV_TOTAL_SI" class="doublewidth form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Total Sum Insured</label><div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="FIRE__FOV_TOTAL_SI" runat="server" CssClass="e-num2 form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div id="liFIRE__FOV_TOTAL_PREM" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Total Premium</label><div class="col-md-8 col-sm-9">
                            <asp:TextBox ID="FIRE__FOV_TOTAL_PREM" runat="server" CssClass="e-num2 form-control"></asp:TextBox>
                        </div>
                    </div>

                </div>
                <legend>Risk Data</legend>
                <div class="grid-card table-responsive">
                    <table class="grid-table">
                        <tr>
                            <th style="width: 33%"></th>
                            <th style="width: 33%">
                                <label>Buildings</label>
                                <asp:CheckBox ID="FIRE__RD1_BUILDINGS" runat="server" onclick="ToggleBuildingsRD(this.checked,'false'); " Text=" " CssClass="asp-check"></asp:CheckBox></th>
                            <th style="width: 33%">
                                <label>Rent</label>
                                <asp:CheckBox ID="FIRE__RD1_RENT" runat="server" onclick="ToggleRentRD(this.checked,'false'); calculate_builder('FIRE__RD2_SI');" Text=" " CssClass="asp-check"></asp:CheckBox></th>

                        </tr>
                        <tr>
                            <td>
                                <label>Sum Insured</label></td>
                            <td>
                                <asp:TextBox ID="FIRE__RD1_SI" runat="server" CssClass="form-control sum_cal" onblur="calculate_builder('FIRE__RD1_SI');"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFireRD1SI" runat="server" ControlToValidate="FIRE__RD1_SI" ErrorMessage="Sum Insured for RD1 required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="FIRE__RD2_SI" runat="server" CssClass="form-control sum_cal" onblur="calculate_builder('FIRE__RD2_SI');"></asp:TextBox>

                            </td>

                        </tr>
                        <tr>
                            <td>
                                <label>Rate</label></td>
                            <td>
                                <asp:TextBox ID="FIRE__RD1_RATE" runat="server" CssClass="form-control e-num4"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="FIRE__RD2_RATE" runat="server" CssClass="form-control e-num4"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <label>Premium</label></td>
                            <td>
                                <asp:TextBox ID="FIRE__RD1_PREMIUM" runat="server" CssClass="form-control pre_cal" onblur="calculate_builder('FIRE__RD1_PREMIUM');"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="FIRE__RD2_PREMIUM" runat="server" CssClass="form-control pre_cal" onblur="calculate_builder('FIRE__RD2_PREMIUM');"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 33%"></td>
                            <td style="width: 27%">
                                <label>Multiple Buildings </label>
                                <asp:CheckBox ID="FIRE__RD1_MULTIPLE_BUILDINGS" runat="server" onclick="ToggleMultipleBuildings(this.checked,'false');" Text=" " CssClass="asp-check"></asp:CheckBox></td>
                            <td style="width: 40%">
                                <label>No. Months </label>
                                <asp:TextBox ID="FIRE__RD1_NO_OF_MONTHS" runat="server" CssClass="form-control field-mandatory" onkeypress="javascript:return isInteger(event);" onblur="SetNoofMonthsDefault();"></asp:TextBox>
                            </td>
                        </tr>
                    </table>

                </div>
                <legend>Fire - Buildings Escalation</legend>
                <div class="grid-card table-responsive">
                    <table class="grid-table">
                        <tr>
                            <th style="width: 52%">
                                <label>Fire Building - Escalation/Inflation</label>
                                <asp:CheckBox ID="FIRE__FIRE_BUILDINGS_ESCALATION_INFLATION" runat="server" onclick="ToggleBuildingsEscalation(this.checked,'false');" onblur="CalculateEscSI();" Text=" " CssClass="asp-check"></asp:CheckBox>
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
                            <td style="width: 52%">
                                <label>Escalation </label>
                            </td>
                            <td style="width: 12%">
                                <asp:TextBox ID="FIRE__BUILDINGS_ESCALATION_PERC" CssClass="form-control field-mandatory e-num4" runat="server" onblur="CalculateEscSI();"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFireBldgEscPerc" runat="server" ControlToValidate="FIRE__BUILDINGS_ESCALATION_PERC" ErrorMessage="Sum Insured FIRE BUILDINGS ESCALATION PERC is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                            <td style="width: 13%">
                                <asp:TextBox ID="FIRE__BUILDINGS_ESCALATION_SI" CssClass="form-control sum_cal" runat="server"></asp:TextBox></td>
                            <td style="width: 12%">
                                <asp:TextBox ID="FIRE__BUILDINGS_ESCALATION_RATE" runat="server" CssClass="form-control e-num4" onblur="calculate_builder('FIRE__BUILDINGS_ESCALATION_RATE');"></asp:TextBox></td>
                            <td style="width: 20%">
                                <asp:TextBox ID="FIRE__BUILDINGS_ESCALATION_PREMIUM" runat="server" CssClass="form-control pre_cal e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 52%">
                                <label>Inf. 1st Year </label>
                            </td>
                            <td style="width: 12%">
                                <asp:TextBox ID="FIRE__BUILDINGS_INFL_1YR_PERC" runat="server" CssClass="form-control field-mandatory e-num4" onblur="CalculateInfOneSI();"></asp:TextBox>

                            </td>
                            <td style="width: 13%">
                                <asp:TextBox ID="FIRE__BUILDINGS_INFL_1YR_SI" CssClass="form-control sum_cal" runat="server"></asp:TextBox></td>
                            <td style="width: 12%">
                                <asp:TextBox ID="FIRE__BUILDINGS_INFL_1YR_RATE" runat="server" CssClass="form-control e-num4" onblur="calculate_builder('FIRE__BUILDINGS_INFL_1YR_RATE');"></asp:TextBox></td>
                            <td style="width: 20%">
                                <asp:TextBox ID="FIRE__BUILDINGS_INFL_1YR_PREMIUM" runat="server" CssClass="form-control pre_cal e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 52%">
                                <label>Inf. 2nd Year </label>
                            </td>
                            <td style="width: 12%">
                                <asp:TextBox ID="FIRE__BUILDINGS_INFL_2YR_PERC" runat="server" CssClass="form-control field-mandatory e-num4" onblur="CalculateInfTwoSI();"></asp:TextBox>

                            </td>
                            <td style="width: 13%">
                                <asp:TextBox ID="FIRE__BUILDINGS_INFL_2YR_SI" CssClass="form-control sum_cal" runat="server"></asp:TextBox></td>
                            <td style="width: 12%">
                                <asp:TextBox ID="FIRE__BUILDINGS_INFL_2YR_RATE" runat="server" CssClass="form-control e-num4" onblur="calculate_builder('FIRE__BUILDINGS_INFL_2YR_RATE');"></asp:TextBox></td>
                            <td style="width: 20%">
                                <asp:TextBox ID="FIRE__BUILDINGS_INFL_2YR_PREMIUM" runat="server" CssClass="form-control pre_cal e-num2"></asp:TextBox></td>
                        </tr>
                    </table>
                </div>
                <legend><span>Multiple Buildings</span></legend>
                <div class="grid-card table-responsive">
                    <Nexus:itemgrid id="FIRE__IPF" runat="server" screencode="BLDGSP" autogeneratecolumns="false" gridlines="None" childpage="Child/BLDGSP_GENERAL.aspx">
                        <columns>
                                            <nexus:riskattribute headertext="Description" datafield="IPARTIES_ITEM_DESC"></nexus:riskattribute>
                                            <nexus:riskattribute headertext="Sum Insured" datafield="IPARTIES_TOTAL_SI"></nexus:riskattribute>
                                            <nexus:riskattribute headertext="Premium" datafield="IPARTIES_TOTAL_PREMIUM"></nexus:riskattribute>                                            
                                         </columns>
                    </Nexus:itemgrid>
                </div>
                <legend>Extensions</legend>
                <div class="grid-card table-responsive">
                    <table class="grid-table">
                        <tr>
                            <th width="15%"></th>
                            <th width="5%"></th>
                            <th width="20%">
                                <label>Limit of Indemnity</label></th>
                            <th width="20%">
                                <label>Rate</label></th>
                            <th width="20%">
                                <label>% of Total Sum Insured</label></th>
                            <th width="20%">
                                <label>Premium</label></th>
                        </tr>
                        <tr>
                            <td width="15%">
                                <label>Claims Preparation Costs</label></td>
                            <td width="5%">
                                <asp:CheckBox ID="FIRE__EF_CLAIM_PREP_COST" runat="server" onclick="ToggleCPC(this.checked);" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="FIRE__CLAIM_PREP_LMT_IDEMNITY" runat="server" onblur="CalculateLIClaimPPrem();" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFireClaimLmtIdemn" runat="server" ControlToValidate="FIRE__CLAIM_PREP_LMT_IDEMNITY" ErrorMessage="FIRE CLAIM PREP LMT IDEMNITY is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="FIRE__CLAIM_PREP_RATE" runat="server" onblur="CalculateLIClaimPPrem();" CssClass="form-control e-num4"></asp:TextBox></td>
                            <asp:RequiredFieldValidator ID="rfvClaimPrepRate" runat="server" ControlToValidate="FIRE__CLAIM_PREP_RATE" ErrorMessage="FIRE CLAIM PREP RATE is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            <td width="20%"></td>
                            <td width="20%">
                                <asp:TextBox ID="FIRE__CLAIM_PREPCOST_PREM" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td width="15%">
                                <label>Disposal of Salvage</label></td>
                            <td width="5%">
                                <asp:CheckBox ID="FIRE__EF_DISPOSAL_SALV" runat="server" onclick="ToggleDS(this.checked);" Text=" " CssClass="asp-check"></asp:CheckBox></td>
                            <td width="20%"></td>
                            <td width="20%"></td>
                            <td width="20%"></td>
                            <td width="20%">
                                <asp:TextBox ID="FIRE__DISPOSAL_SALV_PREM" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td width="15%">
                                <label>Leakage</label></td>
                            <td width="5%">
                                <asp:CheckBox ID="FIRE__EF_LEAKAGE" runat="server" onclick="ToggleLeakage(this.checked);" Text=" " CssClass="asp-check"></asp:CheckBox></td>
                            <td width="20%">
                                <asp:TextBox ID="FIRE__LEAKAGE_LMT_IDEMNITY" runat="server" onblur="CalculateLeakPPrem();" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFireLkgLmtIdemnity" runat="server" ControlToValidate="FIRE__LEAKAGE_LMT_IDEMNITY" ErrorMessage="FIRE LEAKAGE LMT IDEMNITY is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="FIRE__LEAKAGE_RATE" runat="server" onblur="CalculateLeakPPrem();" CssClass="form-control e-num4"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFireLkgRate" runat="server" ControlToValidate="FIRE__LEAKAGE_RATE" ErrorMessage="FIRE LEAKAGE RATE is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                            <td width="20%"></td>
                            <td width="20%">
                                <asp:TextBox ID="FIRE__LEAKAGE_PREM" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td width="15%">
                                <label>Riot & Strike</label></td>
                            <td width="5%">
                                <asp:CheckBox ID="FIRE__EF_RIOT_STRIKE" runat="server" onclick="ToggleRiotStrike(this.checked);" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </td>
                            <td width="20%"></td>
                            <td width="20%"></td>
                            <td width="20%"></td>
                            <td width="20%">
                                <asp:TextBox ID="FIRE__RIOT_STRIKE_PREM" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td width="15%">
                                <label>Subsidence and Landslip</label></td>
                            <td width="5%">
                                <asp:CheckBox ID="FIRE__EF_SUBSIDENCE_LANDSLIP" runat="server" onclick="ToggleSL(this.checked);" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="FIRE__LANDSLIP_LMT_IDEMNITY" runat="server" onblur="CalculateSLandPrem();" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFireLandLmtIdemn" runat="server" ControlToValidate="FIRE__LANDSLIP_LMT_IDEMNITY" ErrorMessage="FIRE LANDSLIP LMT IDEMNITY is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="FIRE__LANDSLIP_RATE" runat="server" onblur="CalculateSLandPrem();" CssClass="form-control e-num4"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFireLndSlipRate" runat="server" ControlToValidate="FIRE__LANDSLIP_RATE" ErrorMessage="FIRE LANDSLIP RATE is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="FIRE__LANDSLIP_TOTAL_SI" runat="server" CssClass="form-control"></asp:TextBox></td>
                            <td width="20%">
                                <asp:TextBox ID="FIRE__LANDSLIP_PREM" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                    </table>
                </div>
                <legend>Reinsurance Exposure</legend>
                <div class="grid-card table-responsive">
                    <table class="grid-table">
                        <tr>
                            <th style="width: 20%"></th>
                            <th style="width: 20%">
                                <label>Total Sum Insured</label></th>
                            <th style="width: 20%">
                                <label>Target Risk SI</label></th>
                            <th style="width: 20%">
                                <label>MPL %</label></th>
                            <th style="width: 20%">
                                <label>RI Exposure</label></th>
                        </tr>
                        <tr>
                            <td>Buildings</td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_BUILDING_SI" runat="server" onblur="CalculateEscSI();" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_BUILDING_RISK_SI" runat="server" CssClass="form-control e-num2" onblur="CalculateEscSI();"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_BUILDING_MPL_PERC" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                            <asp:RangeValidator ID="rngRI_EXP__RI_BUILDING_MPL_PERC" runat="server" Type="Double" MinimumValue="10" MaximumValue="100" ControlToValidate="RI_EXP__RI_BUILDING_MPL_PERC" SetFocusOnError="true" Display="None" ErrorMessage="Please enter BUILDING MPL b/W 10 to 100."></asp:RangeValidator>

                            <td>
                                <asp:TextBox ID="RI_EXP__RI_BUILDING_RI_EXPOSURE" runat="server" onblur="CalculateEscSI();" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Inflation/Escalation(Buildings).</td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_INFLATION_SI" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_INFLATION_RISK_SI" onblur="CalculateEscSI();" runat="server" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_INFLATION_MPL_PERC" runat="server" onblur="calculate_builder('RI_EXP__RI_INFLATION_MPL_PERC');" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_INFLATION_RI_EXPOSURE" runat="server" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Plnt/Equ/Mac/Cont</td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_PEMC_SI" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_PEMC_RISK_SI" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_PEMC_MPL_PERC" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_PEMC_RI_EXPOSURE" runat="server" CssClass="form-control e-num2" ReadOnly="true"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Inflation/Escalation(Plant etc.)</td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_IEP_SI" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_IEP_RISK_SI" runat="server" ReadOnly="true" onblur="calculate_builder('RI_EXP__RI_IEP_RISK_SI');" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_IEP_MPL_PERC" runat="server" ReadOnly="true" onblur="calculate_builder('RI_EXP__RI_IEP_MPL_PERC');" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_IEP_RI_EXPOSURE" runat="server" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="height: 26px">Stock</td>
                            <td style="height: 26px">
                                <asp:TextBox ID="RI_EXP__RI_STOCK_SI" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox></td>
                            <td style="height: 26px">
                                <asp:TextBox ID="RI_EXP__RI_STOCK_RISK_SI" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox></td>
                            <td style="height: 26px">
                                <asp:TextBox ID="RI_EXP__RI_STOCK_MPL_PERC" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox></td>
                            <td style="height: 26px">
                                <asp:TextBox ID="RI_EXP__RI_STOCK_RI_EXPOSURE" runat="server" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>RENT(Coloumn 2)</td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_RENT_SI" runat="server" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_RENT_RISK_SI" CssClass="form-control e-num2" runat="server" onblur="calculate_builder('RI_EXP__RI_RENT_RISK_SI');"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_RENT_MPL_PERC" CssClass="form-control e-num2" runat="server" onblur="calculate_builder('RI_EXP__RI_RENT_MPL_PERC');"></asp:TextBox></td>
                            <asp:RangeValidator ID="rngRI_EXP__RI_RENT_MPL_PERC" Type="Double" runat="server" MinimumValue="10" MaximumValue="100" ControlToValidate="RI_EXP__RI_RENT_MPL_PERC" SetFocusOnError="true" Display="None" ErrorMessage="Please enter RENT MPL b/W 10 to 100."></asp:RangeValidator>

                            <td>
                                <asp:TextBox ID="RI_EXP__RI_RENT_RI_EXPOSURE" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Claim Prep Cost</td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_CLAIMPREP_SI" runat="server" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_CLAIMPREP_RISK_SI" CssClass="form-control" runat="server" onblur="calculate_builder('RI_EXP__RI_CLAIMPREP_RISK_SI');"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_CLAIMPREP_RISK_PERC" CssClass="form-control" runat="server" onblur="calculate_builder('RI_EXP__RI_CLAIMPREP_RISK_PERC');"></asp:TextBox></td>
                            <asp:RangeValidator ID="rngRI_EXP__RI_CLAIMPREP_RISK_PERC" runat="server" Type="Double" MinimumValue="10.00" MaximumValue="100.00" ControlToValidate="RI_EXP__RI_CLAIMPREP_RISK_PERC" SetFocusOnError="true" Display="None" ErrorMessage="Please enter Claim MPL b/W 10 to 100"></asp:RangeValidator>

                            <td>
                                <asp:TextBox ID="RI_EXP__RI_CLAIMPREP_RISK_RI_EXPOSURE" runat="server" CssClass="form-control"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Miscellans items</td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_MSC_ITEMS_SI" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_MSC_ITEMS_RISK_SI" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_MSC_ITEMS_RISK_PERC" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__RI_MSC_ITEMS_RI_EXPOSURE" runat="server" ReadOnly="true" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Plant Machine Stock</td>
                            <td>
                                <asp:TextBox ID="RI_EXP__PM_STOCK_SI" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__PM_STOCK_RISK_SI" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__PM_STOCK_RISK_PREC" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__PM_STOCK_RI_EXPOSURE" runat="server" CssClass="form-control e-num2" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Total Fire RI Exposure</td>
                            <td>
                                <asp:TextBox ID="RI_EXP__TOTAL_FIRE_RI_SI" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__TOTAL_FIRE_RI_RISK_S" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="RI_EXP__TOTAL_FIRE_RI_EXPOSURE" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                    </table>
                </div>
                <div class="form-horizontal">
                    <asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>

                    <legend>Endorsement</legend>
                    <NexusControl:standardwording runat="server" id="FIRE__ENDORESEMENT" supportrisklevel="true"></NexusControl:standardwording>
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


