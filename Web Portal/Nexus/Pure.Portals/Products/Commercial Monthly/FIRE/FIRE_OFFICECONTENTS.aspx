<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/default.master" CodeFile="FIRE_OFFICECONTENTS.aspx.vb" Inherits="Nexus.FIRE_OFFICECONTENTS" %>

<%@ Register Src="~/Controls/ProgressBar.ascx" TagName="ProgressBar" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/CalendarLookup.ascx" TagName="CalendarLookup" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/AddressCntrl.ascx" TagName="AddressCntrl" TagPrefix="NexusControl" %>
<%@ Register Src="~/controls/StandardWordings.ascx" TagName="StandardWording" TagPrefix="NexusControl" %>
<%@ Register TagPrefix="Nexus" Namespace="Nexus" %>
<%@ Register TagPrefix="NexusProvider" Namespace="NexusProvider" Assembly="NexusProvider" %>

<asp:Content ID="cntMainBody" ContentPlaceHolderID="cntMainBody" runat="Server">
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            document.getElementById("<%= OC__CF_CONTENT_SUMINSURED.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__CF_CONTENT_BASERATE.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__CF_CONTENT_AGREED_RATE.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__CF_CONTENT_PREMIUM.ClientID%>").readOnly = true;

            document.getElementById("<%= OC__CF_LDOCUMENT_SUMINSURED.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__CF_LDOCUMENT_BASERATE.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__CF_LDOCUMENT_AGREED_RATE.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__CF_LDOCUMENT_PREMIUM.ClientID%>").readOnly = true;

            document.getElementById("<%= OC__CF_LIABILITY_SUMINSURED.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__CF_LIABILITY_BASERATE.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__CF_LIABILITY_AGREED_RATE.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__CF_LIABILITY_PREMIUM.ClientID%>").readOnly = true;

            document.getElementById("<%= OC__CF_THEFT_NF_SUMINSURED.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__CF_THEFT_NF_BASERATE.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__CF_THEFT_NF_AGREED_RATE.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__CF_THEFT_NF_PREMIUM.ClientID%>").readOnly = true;

            document.getElementById("<%= OC__CF_THEFT_F_SUMINSURED.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__CF_THEFT_F_BASERATE.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__CF_THEFT_F_AGREED_RATE.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__CF_THEFT_F_PREMIUM.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__CF_TOTAL_SI.ClientID%>").readOnly = true;

            document.getElementById("<%= OC__EF_AICW_LIMIT_IDEMNITY.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__EF_AICW_RATE.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__EF_AICW_PREMIUM.ClientID%>").readOnly = true;

            document.getElementById("<%= OC__EF_CPC_LIMIT_IDEMNITY.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__EF_CPC_RATE.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__EF_CPC_PREMIUM.ClientID%>").readOnly = true;

            document.getElementById("<%= OC__EF_MDMG_LIMIT_IDEMNITY.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__EF_MDMG_RATE.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__EF_MDMG_PREMIUM.ClientID%>").readOnly = true;

            document.getElementById("<%= OC__EF_RS_PREMIUM.ClientID%>").readOnly = true;

            document.getElementById("<%= OC__CF_TOTAL_PREMIUM.ClientID%>").readOnly = true;

            document.getElementById("<%= OC__CF_TOTAL_SI.ClientID%>").readOnly = true;

            document.getElementById("<%= OC__EF_AICW_PREMIUM.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__EF_CPC_PREMIUM.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__EF_MDMG_PREMIUM.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__EF_RS_PREMIUM.ClientID%>").readOnly = true;


            document.getElementById("<%= OC__CONTENT_TOTAL_SI.ClientID%>").readOnly = true;

            document.getElementById("<%= OC__CONTENT_TARGET_RISK_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__CONTENT_MPL.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__CONTENT_RI_EXPOSURE.ClientID%>").readOnly = true;

            document.getElementById("<%= OC__RENT_TOTAL_SI.ClientID%>").readOnly = true;

            document.getElementById("<%= OC__RENT_TARGET_RISK_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__RENT_MPL.ClientID%>").readOnly = true;

            document.getElementById("<%= OC__RENT_RI_EXPOSURE.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__ICOST_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__ICOST_TARGET_RISK_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__ICOST_MPL.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__ICOST_RI_EXPOSURE.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__CL_TOTAL_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__CL_TARGET_RISK_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__CL_MPL.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__CL_RI_EXPOSURE.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__AIW_TOTAL_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__AIW_TARGET_RISK_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__AIW_MPL.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__AIW_RI_EXPOSURE.ClientID%>").readOnly = true;

            document.getElementById("<%= OC__LI_TOTAL_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__LI_TARGET_RISK_SI.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__LI_MPL.ClientID%>").readOnly = true;
            document.getElementById("<%= OC__LI_RI_EXPOSURE.ClientID%>").readOnly = true;


            ValidatorEnable($("#<%= rfvContentSUMINSURED.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfvContentAgreedRate.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfvContentPremium.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfvLDocumentSumInsured.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfvLDocumentAgreedRate.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfvLDocumentPremium.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfvLiabilityDocumentSumInsured.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfvLiabilityDocumentAgreedRate.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfvLiabilityDocumentPremium.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfvNFSI.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfvNFAgreedRate.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfvNFPremium.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfvFSI.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfvFAgreedRate.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfvFPremium.ClientID%>")[0], false);

            ValidatorEnable($("#<%= rfvLIACW.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfvRateAICW.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfvPremiumAICW.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfvLICPC.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfvRateCPC.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfvPremiumCPC.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfvLIMD.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfvRateMD.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfcPremiumMD.ClientID%>")[0], false);
            ValidatorEnable($("#<%= rfvRoitStrikePremium.ClientID%>")[0], false);


            document.getElementById("<%= OC__CF_CONTENT_SUMINSURED.ClientID%>").className = "form-control";
            document.getElementById("<%= OC__CF_CONTENT_AGREED_RATE.ClientID%>").className = "form-control";
            document.getElementById("<%= OC__CF_CONTENT_PREMIUM.ClientID%>").className = "form-control";

            document.getElementById("<%= OC__CF_LDOCUMENT_SUMINSURED.ClientID%>").className = "form-control";
            document.getElementById("<%= OC__CF_LDOCUMENT_AGREED_RATE.ClientID%>").className = "form-control";
            document.getElementById("<%= OC__CF_LDOCUMENT_PREMIUM.ClientID%>").className = "form-control";

            document.getElementById("<%= OC__CF_LIABILITY_SUMINSURED.ClientID%>").className = "form-control";
            document.getElementById("<%= OC__CF_LIABILITY_AGREED_RATE.ClientID%>").className = "form-control";
            document.getElementById("<%= OC__CF_LIABILITY_PREMIUM.ClientID%>").className = "form-control";

            document.getElementById("<%= OC__CF_THEFT_NF_SUMINSURED.ClientID%>").className = "form-control";
            document.getElementById("<%= OC__CF_THEFT_NF_AGREED_RATE.ClientID%>").className = "form-control";
            document.getElementById("<%= OC__CF_THEFT_NF_PREMIUM.ClientID%>").className = "form-control";

            document.getElementById("<%= OC__CF_THEFT_F_SUMINSURED.ClientID%>").className = "form-control";
            document.getElementById("<%= OC__CF_THEFT_F_AGREED_RATE.ClientID%>").className = "form-control";
            document.getElementById("<%= OC__CF_THEFT_F_PREMIUM.ClientID%>").className = "form-control";


            ToggleContent(($('#<%= OC__CF_OFFICECONTENTS.ClientID%>')[0].checked), 'true');
            ToggleLossDocuments(($('#<%= OC__CF_LOSSOFDOCUMENT.ClientID%>')[0].checked), 'true');
            ToggleLiabilityDocuments(($('#<%= OC__CF_LIABILITY_DOCUMENT.ClientID%>')[0].checked), 'true');
            ToggleTheft(($('#<%= OC__CF_THEFT_NON_FORCIBLE.ClientID%>')[0].checked), 'true');
            ToggleTheftForcible(($('#<%= OC__CF_THEFT_FORCIBLY_ENTRY.ClientID%>')[0].checked), 'true');
            ToggleAICW(($('#<%= OC__EF_ADDITIONAL_COST_WORKING.ClientID%>')[0].checked), 'true');
            ToggleCPC(($('#<%= OC__EF_CLAIM_PREP_COST.ClientID%>')[0].checked), 'true');
            ToggleMD(($('#<%= OC__EF_MALICOIUS_DAMAGE.ClientID%>')[0].checked), 'true');

            ToggleFlatPremium(($('#<%= OC__OCO_FLATPREM.ClientID%>')[0].checked), 'true');

        });

        function ToggleContent(flag, OnLoad) {
            document.getElementById("<%= OC__CF_CONTENT_SUMINSURED.ClientID%>").readOnly = !flag;
            //document.getElementById("<%= OC__CF_CONTENT_BASERATE.ClientID%>").readOnly = !flag;
            document.getElementById("<%= OC__CF_CONTENT_AGREED_RATE.ClientID%>").readOnly = !flag;
            if (($('#<%= OC__OCO_FLATPREM.ClientID%>')[0].checked) && (flag)) {
                document.getElementById("<%= OC__CF_CONTENT_PREMIUM.ClientID%>").readOnly = false;
            }
            else {
                document.getElementById("<%= OC__CF_CONTENT_PREMIUM.ClientID%>").readOnly = true;
            }
            if (OnLoad == "false") {

                document.getElementById("<%= OC__CF_CONTENT_SUMINSURED.ClientID%>").value = "";
                document.getElementById("<%= OC__CF_CONTENT_PREMIUM.ClientID%>").value = "";
                document.getElementById("<%= OC__CF_CONTENT_BASERATE.ClientID%>").value = "";
                document.getElementById("<%= OC__CF_CONTENT_AGREED_RATE.ClientID%>").value = "";
                document.getElementById("<%= OC__CONTENT_MPL.ClientID%>").value = "";
                document.getElementById("<%= OC__RENT_MPL.ClientID%>").value = "";

            }

            if (flag) {

                if ((document.getElementById("<%= OC__CF_CONTENT_BASERATE.ClientID%>").value != document.getElementById("<%= hvBaseRateFinal.ClientID%>").value) || (document.getElementById("<%= OC__CF_CONTENT_BASERATE.ClientID%>").value == document.getElementById("<%= OC__CF_CONTENT_AGREED_RATE.ClientID%>").value)) {
                    document.getElementById("<%= OC__CF_CONTENT_BASERATE.ClientID%>").value = document.getElementById("<%= hvBaseRateFinal.ClientID%>").value;
                    document.getElementById("<%= OC__CF_CONTENT_AGREED_RATE.ClientID%>").value = document.getElementById("<%= hvBaseRateFinal.ClientID%>").value;
                }
                if (document.getElementById("<%= OC__CONTENT_MPL.ClientID%>").value == "") {
                    document.getElementById("<%= OC__CONTENT_MPL.ClientID%>").value = "100";
                    document.getElementById("<%= OC__RENT_MPL.ClientID%>").value = "100";
                }
                ValidatorEnable($("#<%= rfvContentSUMINSURED.ClientID%>")[0], true);
                ValidatorEnable($("#<%= rfvContentAgreedRate.ClientID%>")[0], true);
                //ValidatorEnable($("#<%= rfvContentPremium.ClientID%>")[0], true);

                document.getElementById("<%= OC__CF_CONTENT_SUMINSURED.ClientID%>").className = "form-control field-mandatory";
                document.getElementById("<%= OC__CF_CONTENT_AGREED_RATE.ClientID%>").className = "form-control field-mandatory";
                document.getElementById("<%= OC__CF_CONTENT_PREMIUM.ClientID%>").className = "form-control field-mandatory";
            } else {

                ValidatorEnable($("#<%= rfvContentSUMINSURED.ClientID%>")[0], false);
                ValidatorEnable($("#<%= rfvContentAgreedRate.ClientID%>")[0], false);
                ValidatorEnable($("#<%= rfvContentPremium.ClientID%>")[0], false);

                document.getElementById("<%= OC__CF_CONTENT_SUMINSURED.ClientID%>").className = "form-control";
                document.getElementById("<%= OC__CF_CONTENT_AGREED_RATE.ClientID%>").className = "form-control";
                document.getElementById("<%= OC__CF_CONTENT_PREMIUM.ClientID%>").className = "form-control";
            }
        }

        function ToggleLossDocuments(flag, OnLoad) {

            document.getElementById("<%= OC__CF_LDOCUMENT_SUMINSURED.ClientID%>").readOnly = !flag;
            //document.getElementById("<%= OC__CF_LDOCUMENT_BASERATE.ClientID%>").readOnly = !flag;
            document.getElementById("<%= OC__CF_LDOCUMENT_AGREED_RATE.ClientID%>").readOnly = !flag;

            if (($('#<%= OC__OCO_FLATPREM.ClientID%>')[0].checked) && (flag)) {
                document.getElementById("<%= OC__CF_LDOCUMENT_PREMIUM.ClientID%>").readOnly = false;
            }
            else {
                document.getElementById("<%= OC__CF_LDOCUMENT_PREMIUM.ClientID%>").readOnly = true;
            }
            if (OnLoad == "false") {

                document.getElementById("<%= OC__CF_LDOCUMENT_SUMINSURED.ClientID%>").value = "";
                document.getElementById("<%= OC__CF_LDOCUMENT_PREMIUM.ClientID%>").value = "";
                document.getElementById("<%= OC__CF_LDOCUMENT_BASERATE.ClientID%>").value = "";
                document.getElementById("<%= OC__CF_LDOCUMENT_AGREED_RATE.ClientID%>").value = "";
            }
            if (flag) {
                ValidatorEnable($("#<%= rfvLDocumentSumInsured.ClientID%>")[0], true);
                ValidatorEnable($("#<%= rfvLDocumentAgreedRate.ClientID%>")[0], true);
                //ValidatorEnable($("#<%= rfvLDocumentPremium.ClientID%>")[0], true);

                document.getElementById("<%= OC__CF_LDOCUMENT_BASERATE.ClientID%>").value = document.getElementById("<%= hvBaseRateFinal.ClientID%>").value;
                document.getElementById("<%= OC__CF_LDOCUMENT_AGREED_RATE.ClientID%>").value = document.getElementById("<%= hvBaseRateFinal.ClientID%>").value;

                document.getElementById("<%= OC__CF_LDOCUMENT_SUMINSURED.ClientID%>").className = "form-control field-mandatory";
                document.getElementById("<%= OC__CF_LDOCUMENT_AGREED_RATE.ClientID%>").className = "form-control field-mandatory";
                document.getElementById("<%= OC__CF_LDOCUMENT_PREMIUM.ClientID%>").className = "form-control field-mandatory";
            }
            else {
                ValidatorEnable($("#<%= rfvLDocumentSumInsured.ClientID%>")[0], false);
                ValidatorEnable($("#<%= rfvLDocumentAgreedRate.ClientID%>")[0], false);
                ValidatorEnable($("#<%= rfvLDocumentPremium.ClientID%>")[0], false);
                document.getElementById("<%= OC__CF_LDOCUMENT_SUMINSURED.ClientID%>").className = "form-control";
                document.getElementById("<%= OC__CF_LDOCUMENT_AGREED_RATE.ClientID%>").className = "form-control";
                document.getElementById("<%= OC__CF_LDOCUMENT_PREMIUM.ClientID%>").className = "form-control";
            }
        }

        function ToggleLiabilityDocuments(flag, OnLoad) {
            document.getElementById("<%= OC__CF_LIABILITY_SUMINSURED.ClientID%>").readOnly = !flag;
            //document.getElementById("<%= OC__CF_LIABILITY_BASERATE.ClientID%>").readOnly = !flag;
            document.getElementById("<%= OC__CF_LIABILITY_AGREED_RATE.ClientID%>").readOnly = !flag;
            // document.getElementById("<%= OC__CF_LIABILITY_PREMIUM.ClientID%>").readOnly = !flag;
            if (($('#<%= OC__OCO_FLATPREM.ClientID%>')[0].checked) && (flag)) {
                document.getElementById("<%= OC__CF_LIABILITY_PREMIUM.ClientID%>").readOnly = false;
            }
            else {
                document.getElementById("<%= OC__CF_LIABILITY_PREMIUM.ClientID%>").readOnly = true;
            }
            if (OnLoad == "false") {
                document.getElementById("<%= OC__CF_LIABILITY_SUMINSURED.ClientID%>").value = "";
                document.getElementById("<%= OC__CF_LIABILITY_PREMIUM.ClientID%>").value = "";
                document.getElementById("<%= OC__CF_LIABILITY_BASERATE.ClientID%>").value = "";
                document.getElementById("<%= OC__CF_LIABILITY_AGREED_RATE.ClientID%>").value = "";
                document.getElementById("<%= OC__LI_MPL.ClientID%>").value = "";
            }
            if (flag) {
                document.getElementById("<%= OC__CF_LIABILITY_BASERATE.ClientID%>").value = document.getElementById("<%= hvBaseRateFinal.ClientID%>").value;
                document.getElementById("<%= OC__CF_LIABILITY_AGREED_RATE.ClientID%>").value = document.getElementById("<%= hvBaseRateFinal.ClientID%>").value;
                if (document.getElementById("<%= OC__LI_MPL.ClientID%>").value == "") {
                    document.getElementById("<%= OC__LI_MPL.ClientID%>").value = "100";
                }
                ValidatorEnable($("#<%= rfvLiabilityDocumentSumInsured.ClientID%>")[0], true);
                ValidatorEnable($("#<%= rfvLiabilityDocumentAgreedRate.ClientID%>")[0], true);
                // ValidatorEnable($("#<%= rfvLiabilityDocumentPremium.ClientID%>")[0], true);
                document.getElementById("<%= OC__CF_LIABILITY_SUMINSURED.ClientID%>").className = "form-control field-mandatory";
                document.getElementById("<%= OC__CF_LIABILITY_AGREED_RATE.ClientID%>").className = "form-control field-mandatory";
                document.getElementById("<%= OC__CF_LIABILITY_PREMIUM.ClientID%>").className = "form-control field-mandatory";
            }
            else {
                ValidatorEnable($("#<%= rfvLiabilityDocumentSumInsured.ClientID%>")[0], false);
                ValidatorEnable($("#<%= rfvLiabilityDocumentAgreedRate.ClientID%>")[0], false);
                ValidatorEnable($("#<%= rfvLiabilityDocumentPremium.ClientID%>")[0], false);
                document.getElementById("<%= OC__CF_LIABILITY_SUMINSURED.ClientID%>").className = "form-control";
                document.getElementById("<%= OC__CF_LIABILITY_AGREED_RATE.ClientID%>").className = "form-control";
                document.getElementById("<%= OC__CF_LIABILITY_PREMIUM.ClientID%>").className = "form-control";
            }
        }

        function ToggleTheft(flag, OnLoad) {
            document.getElementById("<%= OC__CF_THEFT_NF_SUMINSURED.ClientID%>").readOnly = !flag;
            //document.getElementById("<%= OC__CF_THEFT_NF_BASERATE.ClientID%>").readOnly = !flag;
            document.getElementById("<%= OC__CF_THEFT_NF_AGREED_RATE.ClientID%>").readOnly = !flag;
            // document.getElementById("<%= OC__CF_THEFT_NF_PREMIUM.ClientID%>").readOnly = !flag;
            if (($('#<%= OC__OCO_FLATPREM.ClientID%>')[0].checked) && (flag)) {
                document.getElementById("<%= OC__CF_THEFT_NF_PREMIUM.ClientID%>").readOnly = false;
            }
            else {
                document.getElementById("<%= OC__CF_THEFT_NF_PREMIUM.ClientID%>").readOnly = true;
            }
            if (OnLoad == "false") {
                document.getElementById("<%= OC__CF_THEFT_NF_SUMINSURED.ClientID%>").value = "";
                document.getElementById("<%= OC__CF_THEFT_NF_PREMIUM.ClientID%>").value = "";
                document.getElementById("<%= OC__CF_THEFT_NF_BASERATE.ClientID%>").value = "";
                document.getElementById("<%= OC__CF_THEFT_NF_AGREED_RATE.ClientID%>").value = "";
            }
            if (flag) {
                document.getElementById("<%= OC__CF_THEFT_NF_BASERATE.ClientID%>").value = document.getElementById("<%= hvBaseRateFinal.ClientID%>").value;
                document.getElementById("<%= OC__CF_THEFT_NF_AGREED_RATE.ClientID%>").value = document.getElementById("<%= hvBaseRateFinal.ClientID%>").value;
                ValidatorEnable($("#<%= rfvNFSI.ClientID%>")[0], true);
                ValidatorEnable($("#<%= rfvNFAgreedRate.ClientID%>")[0], true);
                //ValidatorEnable($("#<%= rfvNFPremium.ClientID%>")[0], true);
                document.getElementById("<%= OC__CF_THEFT_NF_SUMINSURED.ClientID%>").className = "form-control field-mandatory";
                document.getElementById("<%= OC__CF_THEFT_NF_AGREED_RATE.ClientID%>").className = "form-control field-mandatory";
                document.getElementById("<%= OC__CF_THEFT_NF_PREMIUM.ClientID%>").className = "form-control field-mandatory";
            }
            else {
                ValidatorEnable($("#<%= rfvNFSI.ClientID%>")[0], false);
                ValidatorEnable($("#<%= rfvNFAgreedRate.ClientID%>")[0], false);
                ValidatorEnable($("#<%= rfvNFPremium.ClientID%>")[0], false);
                document.getElementById("<%= OC__CF_THEFT_NF_SUMINSURED.ClientID%>").className = "form-control";
                document.getElementById("<%= OC__CF_THEFT_NF_AGREED_RATE.ClientID%>").className = "form-control";
                document.getElementById("<%= OC__CF_THEFT_NF_PREMIUM.ClientID%>").className = "form-control";
            }
        }

        function RemovePercent() {
            $(".e-per").each(function (index) {
                var val = $(this).val();
                if (val.substring(val.length - 1) == "%") {
                    $(this).val(val.substring(0, val.length - 1));
                }
            });
        }


        function ToggleTheftForcible(flag, OnLoad) {
            document.getElementById("<%= OC__CF_THEFT_F_SUMINSURED.ClientID%>").readOnly = !flag;
            //document.getElementById("<%= OC__CF_THEFT_F_BASERATE.ClientID%>").readOnly = !flag;
            document.getElementById("<%= OC__CF_THEFT_F_AGREED_RATE.ClientID%>").readOnly = !flag;
            //document.getElementById("<%= OC__CF_THEFT_F_PREMIUM.ClientID%>").readOnly = !flag;
            if (($('#<%= OC__OCO_FLATPREM.ClientID%>')[0].checked) && (flag)) {
                document.getElementById("<%= OC__CF_THEFT_F_PREMIUM.ClientID%>").readOnly = false;
            }
            else {
                document.getElementById("<%= OC__CF_THEFT_F_PREMIUM.ClientID%>").readOnly = true;
            }
            if (OnLoad == "false") {
                document.getElementById("<%= OC__CF_THEFT_F_SUMINSURED.ClientID%>").value = "";
                document.getElementById("<%= OC__CF_THEFT_F_PREMIUM.ClientID%>").value = "";
                document.getElementById("<%= OC__CF_THEFT_F_BASERATE.ClientID%>").value = "";
                document.getElementById("<%= OC__CF_THEFT_F_AGREED_RATE.ClientID%>").value = "";
            }
            if (flag) {
                document.getElementById("<%= OC__CF_THEFT_F_BASERATE.ClientID%>").value = document.getElementById("<%= hvBaseRateFinal.ClientID%>").value;
                document.getElementById("<%= OC__CF_THEFT_F_AGREED_RATE.ClientID%>").value = document.getElementById("<%= hvBaseRateFinal.ClientID%>").value;
                ValidatorEnable($("#<%= rfvFSI.ClientID%>")[0], true);
                ValidatorEnable($("#<%= rfvFAgreedRate.ClientID%>")[0], true);
                ValidatorEnable($("#<%= rfvFPremium.ClientID%>")[0], true);

                document.getElementById("<%= OC__CF_THEFT_F_SUMINSURED.ClientID%>").className = "form-control field-mandatory";
                document.getElementById("<%= OC__CF_THEFT_F_AGREED_RATE.ClientID%>").className = "form-control field-mandatory";
                document.getElementById("<%= OC__CF_THEFT_F_PREMIUM.ClientID%>").className = "form-control field-mandatory";
            }
            else {
                ValidatorEnable($("#<%= rfvFSI.ClientID%>")[0], false);
                ValidatorEnable($("#<%= rfvFAgreedRate.ClientID%>")[0], false);
                ValidatorEnable($("#<%= rfvFPremium.ClientID%>")[0], false);
                document.getElementById("<%= OC__CF_THEFT_F_SUMINSURED.ClientID%>").className = "form-control";
                document.getElementById("<%= OC__CF_THEFT_F_AGREED_RATE.ClientID%>").className = "form-control";
                document.getElementById("<%= OC__CF_THEFT_F_PREMIUM.ClientID%>").className = "form-control";
            }
        }

        function ToggleAICW(flag, OnLoad) {
            document.getElementById("<%= OC__EF_AICW_LIMIT_IDEMNITY.ClientID%>").readOnly = !flag;
            document.getElementById("<%= OC__EF_AICW_RATE.ClientID%>").readOnly = !flag;
            //document.getElementById("<%= OC__EF_AICW_PREMIUM.ClientID%>").readOnly = !flag;
            if (($('#<%= OC__OCO_FLATPREM.ClientID%>')[0].checked) && (flag)) {
                document.getElementById("<%= OC__EF_AICW_PREMIUM.ClientID%>").readOnly = false;
            }
            else {
                document.getElementById("<%= OC__EF_AICW_PREMIUM.ClientID%>").readOnly = true;
            }

            if (OnLoad == "false") {
                document.getElementById("<%= OC__EF_AICW_LIMIT_IDEMNITY.ClientID%>").value = "";
                document.getElementById("<%= OC__EF_AICW_RATE.ClientID%>").value = "";
                document.getElementById("<%= OC__EF_AICW_PREMIUM.ClientID%>").value = "";
                document.getElementById("<%= OC__ICOST_SI.ClientID%>").value = "";
                document.getElementById("<%= OC__AIW_TOTAL_SI.ClientID%>").value = "";
                document.getElementById("<%= OC__ICOST_TARGET_RISK_SI.ClientID%>").value = "";
                document.getElementById("<%= OC__AIW_TARGET_RISK_SI.ClientID%>").value = "";
                document.getElementById("<%= OC__ICOST_MPL.ClientID%>").value = "";
                document.getElementById("<%= OC__AIW_MPL.ClientID%>").value = "";
            }
            if (flag) {
                ValidatorEnable($("#<%= rfvLIACW.ClientID%>")[0], true);
                ValidatorEnable($("#<%= rfvRateAICW.ClientID%>")[0], true);
                //ValidatorEnable($("#<%= rfvPremiumAICW.ClientID%>")[0], true);
                document.getElementById("<%= OC__EF_AICW_LIMIT_IDEMNITY.ClientID%>").className = "form-control field-mandatory";
                document.getElementById("<%= OC__EF_AICW_RATE.ClientID%>").className = "form-control field-mandatory";
                document.getElementById("<%= OC__EF_AICW_PREMIUM.ClientID%>").className = "form-control field-mandatory";
            }
            else {
                ValidatorEnable($("#<%= rfvLIACW.ClientID%>")[0], false);
                ValidatorEnable($("#<%= rfvRateAICW.ClientID%>")[0], false);
                ValidatorEnable($("#<%= rfvPremiumAICW.ClientID%>")[0], false);
                document.getElementById("<%= OC__EF_AICW_LIMIT_IDEMNITY.ClientID%>").className = "form-control";
                document.getElementById("<%= OC__EF_AICW_RATE.ClientID%>").className = "form-control";
                document.getElementById("<%= OC__EF_AICW_PREMIUM.ClientID%>").className = "form-control";
            }
        }

        function ToggleCPC(flag, OnLoad) {
            document.getElementById("<%= OC__EF_CPC_LIMIT_IDEMNITY.ClientID%>").readOnly = !flag;
            document.getElementById("<%= OC__EF_CPC_RATE.ClientID%>").readOnly = !flag;
            //document.getElementById("<%= OC__EF_CPC_PREMIUM.ClientID%>").readOnly = !flag;
            if (($('#<%= OC__OCO_FLATPREM.ClientID%>')[0].checked) && (flag)) {
                document.getElementById("<%= OC__EF_CPC_PREMIUM.ClientID%>").readOnly = false;
            }
            else {
                document.getElementById("<%= OC__EF_CPC_PREMIUM.ClientID%>").readOnly = true;
            }
            if (OnLoad == "false") {
                document.getElementById("<%= OC__EF_CPC_LIMIT_IDEMNITY.ClientID%>").value = "";
                document.getElementById("<%= OC__EF_CPC_RATE.ClientID%>").value = "";
                document.getElementById("<%= OC__EF_CPC_PREMIUM.ClientID%>").value = "";
                document.getElementById("<%= OC__CL_TOTAL_SI.ClientID%>").value = "";
                document.getElementById("<%= OC__CL_TARGET_RISK_SI.ClientID%>").value = "";
                document.getElementById("<%= OC__CL_MPL.ClientID%>").value = "";
            }
            if (flag) {
                ValidatorEnable($("#<%= rfvLICPC.ClientID%>")[0], true);
                ValidatorEnable($("#<%= rfvRateCPC.ClientID%>")[0], true);
                //ValidatorEnable($("#<%= rfvPremiumCPC.ClientID%>")[0], true);
                document.getElementById("<%= OC__EF_CPC_LIMIT_IDEMNITY.ClientID%>").className = "form-control field-mandatory";
                document.getElementById("<%= OC__EF_CPC_RATE.ClientID%>").className = "form-control field-mandatory";
                document.getElementById("<%= OC__EF_CPC_PREMIUM.ClientID%>").className = "form-control field-mandatory";
            }
            else {
                ValidatorEnable($("#<%= rfvLICPC.ClientID%>")[0], false);
                ValidatorEnable($("#<%= rfvRateCPC.ClientID%>")[0], false);
                ValidatorEnable($("#<%= rfvPremiumCPC.ClientID%>")[0], false);
                document.getElementById("<%= OC__EF_CPC_LIMIT_IDEMNITY.ClientID%>").className = "form-control";
                document.getElementById("<%= OC__EF_CPC_RATE.ClientID%>").className = "form-control";
                document.getElementById("<%= OC__EF_CPC_PREMIUM.ClientID%>").className = "form-control";
            }
        }
        function ToggleMD(flag, OnLoad) {
            document.getElementById("<%= OC__EF_MDMG_LIMIT_IDEMNITY.ClientID%>").readOnly = !flag;
            document.getElementById("<%= OC__EF_MDMG_RATE.ClientID%>").readOnly = !flag;
            //document.getElementById("<%= OC__EF_MDMG_PREMIUM.ClientID%>").readOnly = !flag;
            if (($('#<%= OC__OCO_FLATPREM.ClientID%>')[0].checked) && (flag)) {
                document.getElementById("<%= OC__EF_MDMG_PREMIUM.ClientID%>").readOnly = false;
            }
            else {
                document.getElementById("<%= OC__EF_MDMG_PREMIUM.ClientID%>").readOnly = true;
            }
            if (OnLoad == "false") {
                document.getElementById("<%= OC__EF_MDMG_LIMIT_IDEMNITY.ClientID%>").value = "";
                document.getElementById("<%= OC__EF_MDMG_RATE.ClientID%>").value = "";
                document.getElementById("<%= OC__EF_MDMG_PREMIUM.ClientID%>").value = "";
            }
            if (flag) {
                ValidatorEnable($("#<%= rfvLIMD.ClientID%>")[0], true);
                ValidatorEnable($("#<%= rfvRateMD.ClientID%>")[0], true);
                //ValidatorEnable($("#<%= rfcPremiumMD.ClientID%>")[0], true);
                document.getElementById("<%= OC__EF_MDMG_LIMIT_IDEMNITY.ClientID%>").className = "form-control field-mandatory";
                document.getElementById("<%= OC__EF_MDMG_RATE.ClientID%>").className = "form-control field-mandatory";
                document.getElementById("<%= OC__EF_MDMG_PREMIUM.ClientID%>").className = "form-control field-mandatory";
            }
            else {
                ValidatorEnable($("#<%= rfvLIMD.ClientID%>")[0], false);
                ValidatorEnable($("#<%= rfvRateMD.ClientID%>")[0], false);
                ValidatorEnable($("#<%= rfcPremiumMD.ClientID%>")[0], false);
                document.getElementById("<%= OC__EF_MDMG_LIMIT_IDEMNITY.ClientID%>").className = "form-control";
                document.getElementById("<%= OC__EF_MDMG_RATE.ClientID%>").className = "form-control";
                document.getElementById("<%= OC__EF_MDMG_PREMIUM.ClientID%>").className = "form-control";
            }
        }

        function ToggleRS(flag) {
            if (($('#<%= OC__OCO_FLATPREM.ClientID%>')[0].checked) && (flag)) {
                document.getElementById("<%= OC__EF_RS_PREMIUM.ClientID%>").readOnly = false;
            }
            else {
                document.getElementById("<%= OC__EF_RS_PREMIUM.ClientID%>").readOnly = true;
            }
            //document.getElementById("<%= OC__EF_RS_PREMIUM.ClientID%>").readOnly = !flag;
            document.getElementById("<%= OC__EF_RS_PREMIUM.ClientID%>").value = "";
            if (flag) {

                document.getElementById("<%= OC__EF_RS_PREMIUM.ClientID%>").className = "form-control field-mandatory";
            }
            else {
                ValidatorEnable($("#<%= rfvRoitStrikePremium.ClientID%>")[0], false);
                document.getElementById("<%= OC__EF_RS_PREMIUM.ClientID%>").className = "form-control";
            }
        }
        function ToggleFlatPremium(flag) {
            if ($('#<%= OC__CF_OFFICECONTENTS.ClientID%>')[0].checked) {
                document.getElementById("<%= OC__CF_CONTENT_PREMIUM.ClientID%>").readOnly = !flag;
            }
            if ($('#<%= OC__CF_LOSSOFDOCUMENT.ClientID%>')[0].checked) {
                document.getElementById("<%= OC__CF_LDOCUMENT_PREMIUM.ClientID%>").readOnly = !flag;
            }
            if ($('#<%= OC__CF_LIABILITY_DOCUMENT.ClientID%>')[0].checked) {
                document.getElementById("<%= OC__CF_LIABILITY_PREMIUM.ClientID%>").readOnly = !flag;
            }
            if ($('#<%= OC__CF_THEFT_NON_FORCIBLE.ClientID%>')[0].checked) {
                document.getElementById("<%= OC__CF_THEFT_NF_PREMIUM.ClientID%>").readOnly = !flag;
            }
            if ($('#<%= OC__CF_THEFT_FORCIBLY_ENTRY.ClientID%>')[0].checked) {
                document.getElementById("<%= OC__CF_THEFT_F_PREMIUM.ClientID%>").readOnly = !flag;
            }

            if ($('#<%= OC__EF_ADDITIONAL_COST_WORKING.ClientID%>')[0].checked) {
                document.getElementById("<%= OC__EF_AICW_PREMIUM.ClientID%>").readOnly = !flag;
            }
            if ($('#<%= OC__EF_CLAIM_PREP_COST.ClientID%>')[0].checked) {
                document.getElementById("<%= OC__EF_CPC_PREMIUM.ClientID%>").readOnly = !flag;
            }
            if ($('#<%= OC__EF_MALICOIUS_DAMAGE.ClientID%>')[0].checked) {
                document.getElementById("<%= OC__EF_MDMG_PREMIUM.ClientID%>").readOnly = !flag;
            }
            if ($('#<%= OC__EF_RIOT_STRIKE.ClientID%>')[0].checked) {
                document.getElementById("<%= OC__EF_RS_PREMIUM.ClientID%>").readOnly = !flag;
            }
        }
        function CalculateCoverFramePremiumTotal() {
            document.getElementById("<%= OC__CF_TOTAL_PREMIUM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= OC__CF_CONTENT_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_LDOCUMENT_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_LIABILITY_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_THEFT_NF_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_THEFT_F_PREMIUM.ClientID%>").value) || 0)).toFixed(2);
        }


        function CalculateOCPremium() {
            var OCPremium = (parseFloat(document.getElementById("<%= OC__CF_CONTENT_SUMINSURED.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= OC__CF_CONTENT_AGREED_RATE.ClientID%>").value) || 0) / 100;
            if (OCPremium != 0) {
                document.getElementById("<%= OC__CF_CONTENT_PREMIUM.ClientID%>").value = (parseFloat(OCPremium)).toFixed(2);
            }
            else {
                document.getElementById("<%= OC__CF_CONTENT_PREMIUM.ClientID%>").value = "";
            }
            document.getElementById("<%= OC__CF_TOTAL_SI.ClientID%>").value = (parseFloat(document.getElementById("<%= OC__CF_CONTENT_SUMINSURED.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_LDOCUMENT_SUMINSURED.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_LIABILITY_SUMINSURED.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_THEFT_NF_SUMINSURED.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_THEFT_F_SUMINSURED.ClientID%>").value) || 0);
            document.getElementById("<%= OC__CF_TOTAL_PREMIUM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= OC__CF_CONTENT_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_LDOCUMENT_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_LIABILITY_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_THEFT_NF_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_THEFT_F_PREMIUM.ClientID%>").value) || 0)).toFixed(2);
            document.getElementById("<%= OC__CONTENT_TOTAL_SI.ClientID%>").value = document.getElementById("<%= OC__CF_CONTENT_SUMINSURED.ClientID%>").value;
            document.getElementById("<%= OC__CONTENT_TARGET_RISK_SI.ClientID%>").value = document.getElementById("<%= OC__CF_CONTENT_SUMINSURED.ClientID%>").value;
            document.getElementById("<%= OC__RENT_TOTAL_SI.ClientID%>").value = (parseFloat(document.getElementById("<%= OC__CF_CONTENT_SUMINSURED.ClientID%>").value) || 0) / 4;
            document.getElementById("<%= OC__RENT_TARGET_RISK_SI.ClientID%>").value = (parseFloat(document.getElementById("<%= OC__CF_CONTENT_SUMINSURED.ClientID%>").value) || 0) / 4;
            if (document.getElementById("<%= OC__CF_CONTENT_SUMINSURED.ClientID%>").value != "") {
                document.getElementById("<%= OC__CONTENT_TARGET_RISK_SI.ClientID%>").readOnly = false;
                document.getElementById("<%= OC__CONTENT_MPL.ClientID%>").readOnly = false;
                document.getElementById("<%= OC__RENT_TARGET_RISK_SI.ClientID%>").readOnly = false;
                document.getElementById("<%= OC__RENT_MPL.ClientID%>").readOnly = false;
            }
            CalculateRETotal();
        }
        function CalculateLDPremium() {
            var LDPremium = (parseFloat(document.getElementById("<%= OC__CF_LDOCUMENT_SUMINSURED.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= OC__CF_LDOCUMENT_AGREED_RATE.ClientID%>").value) || 0) / 100;
            if (LDPremium != 0) {
                document.getElementById("<%= OC__CF_LDOCUMENT_PREMIUM.ClientID%>").value = (parseFloat(LDPremium)).toFixed(2);
            }
            else {
                document.getElementById("<%= OC__CF_LDOCUMENT_PREMIUM.ClientID%>").value = "";
            }
            document.getElementById("<%= OC__CF_TOTAL_SI.ClientID%>").value = (parseFloat(document.getElementById("<%= OC__CF_CONTENT_SUMINSURED.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_LDOCUMENT_SUMINSURED.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_LIABILITY_SUMINSURED.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_THEFT_NF_SUMINSURED.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_THEFT_F_SUMINSURED.ClientID%>").value) || 0);
            document.getElementById("<%= OC__CF_TOTAL_PREMIUM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= OC__CF_CONTENT_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_LDOCUMENT_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_LIABILITY_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_THEFT_NF_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_THEFT_F_PREMIUM.ClientID%>").value) || 0)).toFixed(2);
        }
        function CalculateLiabilityDocPremium() {
            var LiabilityDocPremium = (parseFloat(document.getElementById("<%= OC__CF_LIABILITY_SUMINSURED.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= OC__CF_LIABILITY_AGREED_RATE.ClientID%>").value) || 0) / 100;
            if (LiabilityDocPremium != 0) {
                document.getElementById("<%= OC__CF_LIABILITY_PREMIUM.ClientID%>").value = (parseFloat(LiabilityDocPremium)).toFixed(2);
            }
            else {
                document.getElementById("<%= OC__CF_LIABILITY_PREMIUM.ClientID%>").value = "";
            }
            document.getElementById("<%= OC__CF_TOTAL_SI.ClientID%>").value = (parseFloat(document.getElementById("<%= OC__CF_CONTENT_SUMINSURED.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_LDOCUMENT_SUMINSURED.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_LIABILITY_SUMINSURED.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_THEFT_NF_SUMINSURED.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_THEFT_F_SUMINSURED.ClientID%>").value) || 0);
            document.getElementById("<%= OC__CF_TOTAL_PREMIUM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= OC__CF_CONTENT_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_LDOCUMENT_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_LIABILITY_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_THEFT_NF_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_THEFT_F_PREMIUM.ClientID%>").value) || 0)).toFixed(2);
            document.getElementById("<%= OC__LI_TOTAL_SI.ClientID%>").value = document.getElementById("<%= OC__CF_LIABILITY_SUMINSURED.ClientID%>").value;
            document.getElementById("<%= OC__LI_TARGET_RISK_SI.ClientID%>").value = document.getElementById("<%= OC__CF_LIABILITY_SUMINSURED.ClientID%>").value;
            if (document.getElementById("<%= OC__CF_LIABILITY_SUMINSURED.ClientID%>").value != "") {
                document.getElementById("<%= OC__LI_TARGET_RISK_SI.ClientID%>").readOnly = false;
                document.getElementById("<%= OC__LI_MPL.ClientID%>").readOnly = false;
            }
            CalculateRETotal();
        }
        function CalculateTheftPremium() {
            var TheftPremium = (parseFloat(document.getElementById("<%= OC__CF_THEFT_NF_SUMINSURED.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= OC__CF_THEFT_NF_AGREED_RATE.ClientID%>").value) || 0) / 100;
            if (TheftPremium != 0) {
                document.getElementById("<%= OC__CF_THEFT_NF_PREMIUM.ClientID%>").value = (parseFloat(TheftPremium)).toFixed(2);
            }
            else {
                document.getElementById("<%= OC__CF_THEFT_NF_PREMIUM.ClientID%>").value = "";
            }
            document.getElementById("<%= OC__CF_TOTAL_SI.ClientID%>").value = (parseFloat(document.getElementById("<%= OC__CF_CONTENT_SUMINSURED.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_LDOCUMENT_SUMINSURED.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_LIABILITY_SUMINSURED.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_THEFT_NF_SUMINSURED.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_THEFT_F_SUMINSURED.ClientID%>").value) || 0);
            document.getElementById("<%= OC__CF_TOTAL_PREMIUM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= OC__CF_CONTENT_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_LDOCUMENT_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_LIABILITY_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_THEFT_NF_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_THEFT_F_PREMIUM.ClientID%>").value) || 0)).toFixed(2);
        }
        function CalculateTheftFPremium() {
            var TheftFPremium = (parseFloat(document.getElementById("<%= OC__CF_THEFT_F_SUMINSURED.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= OC__CF_THEFT_F_AGREED_RATE.ClientID%>").value) || 0) / 100;
            if (TheftFPremium != 0) {
                document.getElementById("<%= OC__CF_THEFT_F_PREMIUM.ClientID%>").value = (parseFloat(TheftFPremium)).toFixed(2);
            }
            else {
                document.getElementById("<%= OC__CF_THEFT_F_PREMIUM.ClientID%>").value = "";
            }
            document.getElementById("<%= OC__CF_TOTAL_SI.ClientID%>").value = (parseFloat(document.getElementById("<%= OC__CF_CONTENT_SUMINSURED.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_LDOCUMENT_SUMINSURED.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_LIABILITY_SUMINSURED.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_THEFT_NF_SUMINSURED.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_THEFT_F_SUMINSURED.ClientID%>").value) || 0);
            document.getElementById("<%= OC__CF_TOTAL_PREMIUM.ClientID%>").value = (parseFloat(parseFloat(document.getElementById("<%= OC__CF_CONTENT_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_LDOCUMENT_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_LIABILITY_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_THEFT_NF_PREMIUM.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CF_THEFT_F_PREMIUM.ClientID%>").value) || 0)).toFixed(2);
            CalculateRETotal();
        }

        function CalculateAICWPremium() {
            var AICWPremium = (parseFloat(document.getElementById("<%= OC__EF_AICW_LIMIT_IDEMNITY.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= OC__EF_AICW_RATE.ClientID%>").value) || 0) / 100;
            if (AICWPremium != 0) {
                document.getElementById("<%= OC__EF_AICW_PREMIUM.ClientID%>").value = (parseFloat(AICWPremium)).toFixed(2);
            }
            else {
                document.getElementById("<%= OC__EF_AICW_PREMIUM.ClientID%>").value = "";
            }

            document.getElementById("<%= OC__ICOST_SI.ClientID%>").value = document.getElementById("<%= OC__EF_AICW_LIMIT_IDEMNITY.ClientID%>").value;
            document.getElementById("<%= OC__ICOST_TARGET_RISK_SI.ClientID%>").value = document.getElementById("<%= OC__EF_AICW_LIMIT_IDEMNITY.ClientID%>").value;


            if (document.getElementById("<%= OC__EF_AICW_LIMIT_IDEMNITY.ClientID%>").value != "") {
                document.getElementById("<%= OC__ICOST_TARGET_RISK_SI.ClientID%>").readOnly = false;
                document.getElementById("<%= OC__ICOST_MPL.ClientID%>").readOnly = false;
                if (document.getElementById("<%= OC__ICOST_MPL.ClientID%>").value == "") {
                    document.getElementById("<%= OC__ICOST_MPL.ClientID%>").value = "100";
                }
            }
            document.getElementById("<%= OC__AIW_TOTAL_SI.ClientID%>").value = document.getElementById("<%= OC__EF_AICW_LIMIT_IDEMNITY.ClientID%>").value;
            document.getElementById("<%= OC__AIW_TARGET_RISK_SI.ClientID%>").value = document.getElementById("<%= OC__EF_AICW_LIMIT_IDEMNITY.ClientID%>").value;
            if (document.getElementById("<%= OC__EF_AICW_LIMIT_IDEMNITY.ClientID%>").value != "") {
                document.getElementById("<%= OC__AIW_TARGET_RISK_SI.ClientID%>").readOnly = false;
                document.getElementById("<%= OC__AIW_MPL.ClientID%>").readOnly = false;
                if (document.getElementById("<%= OC__AIW_MPL.ClientID%>").value == "") {
                    document.getElementById("<%= OC__AIW_MPL.ClientID%>").value = "100";
                }
            }
            CalculateRETotal();
        }

        function CalculateAICWRate() {
            var AICWRate = ((parseFloat(document.getElementById("<%= OC__EF_AICW_PREMIUM.ClientID%>").value) || 0) / (parseFloat(document.getElementById("<%= OC__EF_AICW_LIMIT_IDEMNITY.ClientID%>").value) || 0)) * 100;
            if (AICWRate != 0 && !isNaN(AICWRate)) {
                document.getElementById("<%= OC__EF_AICW_RATE.ClientID%>").value = parseFloat(AICWRate).toFixed(4);
            }
            else {
                document.getElementById("<%= OC__EF_AICW_RATE.ClientID%>").value = "";
            }
        }

        function CalculateCPCPremium() {
            var CPCPremium = (parseFloat(document.getElementById("<%= OC__EF_CPC_LIMIT_IDEMNITY.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= OC__EF_CPC_RATE.ClientID%>").value) || 0) / 100;
            if (CPCPremium != 0) {
                document.getElementById("<%= OC__EF_CPC_PREMIUM.ClientID%>").value = CPCPremium;
            }
            else {
                document.getElementById("<%= OC__EF_CPC_PREMIUM.ClientID%>").value = "";
            }
            document.getElementById("<%= OC__CL_TOTAL_SI.ClientID%>").value = document.getElementById("<%= OC__EF_CPC_LIMIT_IDEMNITY.ClientID%>").value;
            document.getElementById("<%= OC__CL_TARGET_RISK_SI.ClientID%>").value = document.getElementById("<%= OC__EF_CPC_LIMIT_IDEMNITY.ClientID%>").value;
            if (document.getElementById("<%= OC__EF_CPC_LIMIT_IDEMNITY.ClientID%>").value != "") {
                document.getElementById("<%= OC__CL_TARGET_RISK_SI.ClientID%>").readOnly = false;
                document.getElementById("<%= OC__CL_MPL.ClientID%>").readOnly = false;
                if (document.getElementById("<%= OC__CL_MPL.ClientID%>").value == "") {
                    document.getElementById("<%= OC__CL_MPL.ClientID%>").value = "100";
                }
            }
            CalculateRETotal();
        }

        function CalculateCPCRate() {
            var CPCRate = ((parseFloat(document.getElementById("<%= OC__EF_CPC_PREMIUM.ClientID%>").value) || 0) / (parseFloat(document.getElementById("<%= OC__EF_CPC_LIMIT_IDEMNITY.ClientID%>").value) || 0)) * 100;
            if (CPCRate != 0 && !isNaN(CPCRate)) {
                document.getElementById("<%= OC__EF_CPC_RATE.ClientID%>").value = parseFloat(CPCRate).toFixed(4);
            }
            else {
                document.getElementById("<%= OC__EF_CPC_RATE.ClientID%>").value = "";
            }
        }

        function CalculateMDPremium() {
            var MDPremium = (parseFloat(document.getElementById("<%= OC__EF_MDMG_LIMIT_IDEMNITY.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= OC__EF_MDMG_RATE.ClientID%>").value) || 0) / 100;
            if (MDPremium != 0) {
                document.getElementById("<%= OC__EF_MDMG_PREMIUM.ClientID%>").value = MDPremium;
            }
            else {
                document.getElementById("<%= OC__EF_MDMG_PREMIUM.ClientID%>").value = "";
            }
        }

        function CalculateMDRate() {
            var MDRate = ((parseFloat(document.getElementById("<%= OC__EF_MDMG_PREMIUM.ClientID%>").value) || 0) / (parseFloat(document.getElementById("<%= OC__EF_MDMG_LIMIT_IDEMNITY.ClientID%>").value) || 0)) * 100;
            if (MDRate != 0 && !isNaN(MDRate)) {
                document.getElementById("<%= OC__EF_MDMG_RATE.ClientID%>").value = parseFloat(MDRate).toFixed(4);
            }
            else {
                document.getElementById("<%= OC__EF_MDMG_RATE.ClientID%>").value = "";
            }
        }
        function CalculateRETotal() {
            document.getElementById("<%= OC__TRE_TOTAL_SI.ClientID%>").value = (parseFloat(document.getElementById("<%= OC__CONTENT_TOTAL_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__RENT_TOTAL_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__ICOST_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__LI_TOTAL_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CL_TOTAL_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__AIW_TOTAL_SI.ClientID%>").value) || 0);
            document.getElementById("<%= OC__TRE_TARGET_RISK_SI.ClientID%>").value = (parseFloat(document.getElementById("<%= OC__CONTENT_TARGET_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__RENT_TARGET_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__ICOST_TARGET_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__LI_TARGET_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CL_TARGET_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__AIW_TARGET_RISK_SI.ClientID%>").value) || 0);
        }
        function CalculateTargetRiskSI() {
            document.getElementById("<%= OC__TRE_TARGET_RISK_SI.ClientID%>").value = (parseFloat(document.getElementById("<%= OC__CONTENT_TARGET_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__RENT_TARGET_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__ICOST_TARGET_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__LI_TARGET_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CL_TARGET_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__AIW_TARGET_RISK_SI.ClientID%>").value) || 0);
            document.getElementById("<%= OC__CONTENT_RI_EXPOSURE.ClientID%>").value = ((parseFloat(document.getElementById("<%= OC__CONTENT_TARGET_RISK_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= OC__CONTENT_MPL.ClientID%>").value) || 0)) / 100;
            document.getElementById("<%= OC__RENT_RI_EXPOSURE.ClientID%>").value = ((parseFloat(document.getElementById("<%= OC__RENT_TARGET_RISK_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= OC__RENT_MPL.ClientID%>").value) || 0)) / 100;
            document.getElementById("<%= OC__ICOST_RI_EXPOSURE.ClientID%>").value = ((parseFloat(document.getElementById("<%= OC__ICOST_TARGET_RISK_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= OC__ICOST_MPL.ClientID%>").value) || 0)) / 100;
            document.getElementById("<%= OC__LI_RI_EXPOSURE.ClientID%>").value = ((parseFloat(document.getElementById("<%= OC__LI_TARGET_RISK_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= OC__LI_MPL.ClientID%>").value) || 0)) / 100;
            document.getElementById("<%= OC__CL_RI_EXPOSURE.ClientID%>").value = ((parseFloat(document.getElementById("<%= OC__CL_TARGET_RISK_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= OC__CL_MPL.ClientID%>").value) || 0)) / 100;
            document.getElementById("<%= OC__AIW_RI_EXPOSURE.ClientID%>").value = ((parseFloat(document.getElementById("<%= OC__AIW_TARGET_RISK_SI.ClientID%>").value) || 0) * (parseFloat(document.getElementById("<%= OC__AIW_MPL.ClientID%>").value) || 0)) / 100;
            document.getElementById("<%= OC__TRE_RI_EXPOSURE.ClientID%>").value = (parseFloat(document.getElementById("<%= OC__CONTENT_RI_EXPOSURE.ClientID%>").value) || 0) +
                (parseFloat(document.getElementById("<%= OC__RENT_RI_EXPOSURE.ClientID%>").value) || 0) +
                (parseFloat(document.getElementById("<%= OC__ICOST_RI_EXPOSURE.ClientID%>").value) || 0) +
                (parseFloat(document.getElementById("<%= OC__LI_RI_EXPOSURE.ClientID%>").value) || 0) +
                (parseFloat(document.getElementById("<%= OC__CL_RI_EXPOSURE.ClientID%>").value) || 0) +
                (parseFloat(document.getElementById("<%= OC__AIW_RI_EXPOSURE.ClientID%>").value) || 0);
            document.getElementById("<%= OC__TRE_TOTAL_SI.ClientID%>").value = (parseFloat(document.getElementById("<%= OC__CONTENT_TARGET_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__RENT_TARGET_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__ICOST_TARGET_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__LI_TARGET_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__CL_TARGET_RISK_SI.ClientID%>").value) || 0) + (parseFloat(document.getElementById("<%= OC__AIW_TARGET_RISK_SI.ClientID%>").value) || 0);
        }

        function CalculateOCRate() {
            if (document.getElementById("<%= OC__CF_CONTENT_PREMIUM.ClientID%>").value != "") {
                var OCRate = (parseFloat(document.getElementById("<%= OC__CF_CONTENT_PREMIUM.ClientID%>").value) || 0) / (parseFloat(document.getElementById("<%= OC__CF_CONTENT_SUMINSURED.ClientID%>").value) || 0) * 100;
                if (OCRate != 0) {
                    document.getElementById("<%= OC__CF_CONTENT_AGREED_RATE.ClientID%>").value = (parseFloat(OCRate)).toFixed(4);
                }
                else {
                    document.getElementById("<%= OC__CF_CONTENT_AGREED_RATE.ClientID%>").value = "";
                }
            }
        }
        function CalculateLODRate() {
            if (document.getElementById("<%= OC__CF_LDOCUMENT_PREMIUM.ClientID%>").value != "") {
                var OCRate = (parseFloat(document.getElementById("<%= OC__CF_LDOCUMENT_PREMIUM.ClientID%>").value) || 0) / (parseFloat(document.getElementById("<%= OC__CF_LDOCUMENT_SUMINSURED.ClientID%>").value) || 0) * 100;
                if (OCRate != 0) {
                    document.getElementById("<%= OC__CF_LDOCUMENT_AGREED_RATE.ClientID%>").value = (parseFloat(OCRate)).toFixed(4);
                 }
                 else {
                     document.getElementById("<%= OC__CF_LDOCUMENT_AGREED_RATE.ClientID%>").value = "";
                 }
             }
         }

         function CalculateLIRate() {
             if (document.getElementById("<%= OC__CF_LIABILITY_PREMIUM.ClientID%>").value != "") {
                var OCRate = (parseFloat(document.getElementById("<%= OC__CF_LIABILITY_PREMIUM.ClientID%>").value) || 0) / (parseFloat(document.getElementById("<%= OC__CF_LIABILITY_SUMINSURED.ClientID%>").value) || 0) * 100;
                if (OCRate != 0) {
                    document.getElementById("<%= OC__CF_LIABILITY_AGREED_RATE.ClientID%>").value = (parseFloat(OCRate)).toFixed(4);
                }
                else {
                    document.getElementById("<%= OC__CF_LIABILITY_AGREED_RATE.ClientID%>").value = "";
                }
            }
        }

        function CalculateNFRate() {
            if (document.getElementById("<%= OC__CF_THEFT_NF_SUMINSURED.ClientID%>").value != "") {
                var OCRate = (parseFloat(document.getElementById("<%= OC__CF_THEFT_NF_PREMIUM.ClientID%>").value) || 0) / (parseFloat(document.getElementById("<%= OC__CF_THEFT_NF_SUMINSURED.ClientID%>").value) || 0) * 100;
                if (OCRate != 0) {
                    document.getElementById("<%= OC__CF_THEFT_NF_AGREED_RATE.ClientID%>").value = (parseFloat(OCRate)).toFixed(4);
                }
                else {
                    document.getElementById("<%= OC__CF_THEFT_NF_AGREED_RATE.ClientID%>").value = "";
                }
            }
        }

        function CalculateFRate() {
            if (document.getElementById("<%= OC__CF_THEFT_F_SUMINSURED.ClientID%>").value != "") {
                var OCRate = (parseFloat(document.getElementById("<%= OC__CF_THEFT_F_PREMIUM.ClientID%>").value) || 0) / (parseFloat(document.getElementById("<%= OC__CF_THEFT_F_SUMINSURED.ClientID%>").value) || 0) * 100;
                if (OCRate != 0) {
                    document.getElementById("<%= OC__CF_THEFT_F_AGREED_RATE.ClientID%>").value = (parseFloat(OCRate)).toFixed(4);
                }
                else {
                    document.getElementById("<%= OC__CF_THEFT_F_AGREED_RATE.ClientID%>").value = "";
                }
            }
        }

    </script>
    <div class="risk-screen">
        <NexusControl:ProgressBar ID="ucProgressBar" runat="server"></NexusControl:ProgressBar>
        <div class="card">
            <Nexus:tabindex id="ctrlTabIndex" runat="server" cssclass="TabContainer" tabcontainerclass="page-progress" activetabclass="ActiveTab" disabledclass="DisabledTab" scrollable="false"></Nexus:tabindex>
            <div class="card-body clearfix">
                <div class="form-horizontal">
                    <asp:HiddenField ID="hvBaseRateFinal" runat="server"></asp:HiddenField>
                    <legend><span>Office Contents Overview</span></legend>
                    <div id="liOC__ALARM_WARRANTY" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Alarm Warranty</label><div class="col-md-8 col-sm-9">
                            <NexusProvider:lookuplist id="OC__ALARM_WARRANTY" runat="server" listtype="PMLookup" listcode="UDL_EBPGFOCALARM" defaulttext="(None)" value="0" cssclass="field-mandatory field-medium form-control"></NexusProvider:lookuplist>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvAlarmWarranty" runat="server" ControlToValidate="OC__ALARM_WARRANTY" ErrorMessage="Alarm Warranty is a required field" InitialValue="" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                    </div>
                    <div id="liOC__OCO_FLATPREM" class="form-group form-group-sm col-lg-6 col-md-6 col-sm-12">
                        <label class="col-md-4 col-sm-3 control-label">Flat Premium</label><div class="col-md-8 col-sm-9">
                            <asp:CheckBox ID="OC__OCO_FLATPREM" runat="server" onclick="ToggleFlatPremium(this.checked);" Text=" " CssClass="asp-check"></asp:CheckBox>
                        </div>
                    </div>
                </div>
                <legend>Cover</legend>
                <div class="grid-card table-responsive">
                    <table class="grid-table">
                        <tr>
                            <th width="15%"></th>
                            <th width="5%"></th>
                            <th width="20%">
                                <label>Sum Insured</label></th>
                            <th width="20%">
                                <label>Base Rate</label></th>
                            <th width="20%">
                                <label>Agreed Rate</label></th>
                            <th width="20%">
                                <label>Premium</label></th>
                        </tr>
                        <tr>
                            <td width="15%">
                                <label>Office Contents</label></td>
                            <td width="5%">
                                <asp:CheckBox ID="OC__CF_OFFICECONTENTS" runat="server" onclick="ToggleContent(this.checked,'false');" Text=" " CssClass="asp-check"></asp:CheckBox></td>
                            <td width="20%">
                                <asp:TextBox ID="OC__CF_CONTENT_SUMINSURED" runat="server" CssClass="form-control field-mandatory" onblur="CalculateOCPremium();"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvContentSUMINSURED" runat="server" ControlToValidate="OC__CF_CONTENT_SUMINSURED" ErrorMessage="Sum Insured Office Contents is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator></td>
                            <td width="20%">
                                <asp:TextBox ID="OC__CF_CONTENT_BASERATE" runat="server" CssClass="form-control e-num4"></asp:TextBox>

                            </td>
                            <td width="20%">
                                <asp:TextBox ID="OC__CF_CONTENT_AGREED_RATE" runat="server" CssClass="form-control field-mandatory e-num4" onblur="CalculateOCPremium();"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvContentAgreedRate" runat="server" ControlToValidate="OC__CF_CONTENT_AGREED_RATE" ErrorMessage="Agreed Rate Office Documents is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="OC__CF_CONTENT_PREMIUM" runat="server" CssClass="form-control field-mandatory" onblur="CalculateCoverFramePremiumTotal();CalculateOCRate();"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvContentPremium" runat="server" ControlToValidate="OC__CF_CONTENT_PREMIUM" ErrorMessage="Premium Office Documents is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>

                        </tr>
                        <tr>
                            <td width="15%">
                                <label>Loss of Documents</label></td>
                            <td width="5%">
                                <asp:CheckBox ID="OC__CF_LOSSOFDOCUMENT" runat="server" onclick="ToggleLossDocuments(this.checked,'false');" Text=" " CssClass="asp-check"></asp:CheckBox></td>
                            <td width="20%">
                                <asp:TextBox ID="OC__CF_LDOCUMENT_SUMINSURED" runat="server" CssClass="form-control field-mandatory" onblur="CalculateLDPremium();"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvLDocumentSumInsured" runat="server" ControlToValidate="OC__CF_LDOCUMENT_SUMINSURED" ErrorMessage="Sum Insured Loss of Documents is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="OC__CF_LDOCUMENT_BASERATE" runat="server" CssClass="form-control e-num4"></asp:TextBox>
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="OC__CF_LDOCUMENT_AGREED_RATE" runat="server" CssClass="form-control field-mandatory e-num4" onblur="CalculateLDPremium();"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvLDocumentAgreedRate" runat="server" ControlToValidate="OC__CF_LDOCUMENT_AGREED_RATE" ErrorMessage="Agreed rate Loss of Documents is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="OC__CF_LDOCUMENT_PREMIUM" runat="server" CssClass="form-control field-mandatory" onblur="CalculateCoverFramePremiumTotal();CalculateLODRate();"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvLDocumentPremium" runat="server" ControlToValidate="OC__CF_LDOCUMENT_PREMIUM" ErrorMessage="Premium Loss of Documents is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td width="15%">
                                <label>Liability of Documents</label></td>
                            <td width="5%">
                                <asp:CheckBox ID="OC__CF_LIABILITY_DOCUMENT" runat="server" onclick="ToggleLiabilityDocuments(this.checked,'false');" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="OC__CF_LIABILITY_SUMINSURED" runat="server" CssClass="form-control field-mandatory" onblur="CalculateLiabilityDocPremium();"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvLiabilityDocumentSumInsured" runat="server" ControlToValidate="OC__CF_LIABILITY_SUMINSURED" ErrorMessage="Sum Insured Liability of Documents is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="OC__CF_LIABILITY_BASERATE" runat="server" CssClass="form-control e-num4"></asp:TextBox>

                            </td>
                            <td width="20%">
                                <asp:TextBox ID="OC__CF_LIABILITY_AGREED_RATE" runat="server" CssClass="form-control field-mandatory e-num4" onblur="CalculateLiabilityDocPremium();"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvLiabilityDocumentAgreedRate" runat="server" ControlToValidate="OC__CF_LIABILITY_BASERATE" ErrorMessage="Agreed Rate Liability of Documents is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="OC__CF_LIABILITY_PREMIUM" runat="server" CssClass="form-control field-mandatory" onblur="CalculateCoverFramePremiumTotal();CalculateLIRate();"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvLiabilityDocumentPremium" runat="server" ControlToValidate="OC__CF_LIABILITY_PREMIUM" ErrorMessage="Premium Liability of Documents is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td width="15%">
                                <label>Theft(Non Forcible)</label></td>
                            <td width="5%">
                                <asp:CheckBox ID="OC__CF_THEFT_NON_FORCIBLE" runat="server" onclick="ToggleTheft(this.checked,'false');" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="OC__CF_THEFT_NF_SUMINSURED" runat="server" CssClass="form-control field-mandatory" onblur="CalculateTheftPremium();"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNFSI" runat="server" ControlToValidate="OC__CF_THEFT_NF_SUMINSURED" ErrorMessage="Sum Insured Theft(Non Forcible) is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="OC__CF_THEFT_NF_BASERATE" runat="server" CssClass="form-control e-num4"></asp:TextBox>
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="OC__CF_THEFT_NF_AGREED_RATE" runat="server" CssClass="form-control field-mandatory e-num4" onblur="CalculateTheftPremium();"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNFAgreedRate" runat="server" ControlToValidate="OC__CF_THEFT_NF_AGREED_RATE" ErrorMessage="Agreed Rate Theft(Non Forcible) is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="OC__CF_THEFT_NF_PREMIUM" runat="server" CssClass="form-control field-mandatory" onblur="CalculateCoverFramePremiumTotal();CalculateNFRate();"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNFPremium" runat="server" ControlToValidate="OC__CF_THEFT_NF_PREMIUM" ErrorMessage="Premium Theft(Non Forcible) is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td width="15%">
                                <label>Theft by Forcibly Entry</label></td>
                            <td width="5%">
                                <asp:CheckBox ID="OC__CF_THEFT_FORCIBLY_ENTRY" runat="server" onclick="ToggleTheftForcible(this.checked,'false');" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="OC__CF_THEFT_F_SUMINSURED" runat="server" CssClass="form-control field-mandatory" onblur="CalculateTheftFPremium();"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFSI" runat="server" ControlToValidate="OC__CF_THEFT_F_SUMINSURED" ErrorMessage="Sum Insured Theft by Forcibly Entry is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="OC__CF_THEFT_F_BASERATE" runat="server" CssClass="form-control e-num4"></asp:TextBox>
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="OC__CF_THEFT_F_AGREED_RATE" runat="server" CssClass="form-control field-mandatory e-num4" onblur="CalculateTheftFPremium();"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFAgreedRate" runat="server" ControlToValidate="OC__CF_THEFT_F_AGREED_RATE" ErrorMessage="Agreed Rate Theft by Forcibly Entry is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                            <td width="20%">
                                <asp:TextBox ID="OC__CF_THEFT_F_PREMIUM" runat="server" CssClass="form-control field-mandatory" onblur="CalculateCoverFramePremiumTotal();CalculateFRate();"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFPremium" runat="server" ControlToValidate="OC__CF_THEFT_F_PREMIUM" ErrorMessage="Premium Theft by Forcibly Entry is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td width="15%">
                                <label>Total :</label></td>
                            <td width="5%"></td>
                            <td width="20%">
                                <asp:TextBox ID="OC__CF_TOTAL_SI" runat="server" CssClass="form-control e-num2"></asp:TextBox>

                            </td>
                            <td width="20%"></td>
                            <td width="20%"></td>
                            <td width="20%">
                                <asp:TextBox ID="OC__CF_TOTAL_PREMIUM" runat="server" CssClass="form-control "></asp:TextBox>

                            </td>
                        </tr>
                    </table>
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
                                <label>Premium</label></th>
                        </tr>
                        <tr>
                            <td>
                                <label>Additional Increased Cost of Working</label></td>
                            <td>
                                <asp:CheckBox ID="OC__EF_ADDITIONAL_COST_WORKING" runat="server" onclick="ToggleAICW(this.checked,'false');" Text=" " CssClass="asp-check"></asp:CheckBox></td>
                            <td>
                                <asp:TextBox ID="OC__EF_AICW_LIMIT_IDEMNITY" runat="server" onblur="CalculateAICWPremium();" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvLIACW" runat="server" ControlToValidate="OC__EF_AICW_LIMIT_IDEMNITY" ErrorMessage="LI Additional Increased Cost of Working is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="OC__EF_AICW_RATE" runat="server" onblur="CalculateAICWPremium();" CssClass="form-control e-num4"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvRateAICW" runat="server" ControlToValidate="OC__EF_AICW_RATE" ErrorMessage="Rate Additional Increased Cost of Working is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="OC__EF_AICW_PREMIUM" runat="server" onblur="CalculateAICWRate();" CssClass="form-control e-num2"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPremiumAICW" runat="server" ControlToValidate="OC__EF_AICW_PREMIUM" ErrorMessage="Premium Additional Increased Cost of Working is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Claims Preparation Costs</label></td>
                            <td>
                                <asp:CheckBox ID="OC__EF_CLAIM_PREP_COST" runat="server" onclick="ToggleCPC(this.checked,'false');" Text=" " CssClass="asp-check"></asp:CheckBox>

                            </td>
                            <td>
                                <asp:TextBox ID="OC__EF_CPC_LIMIT_IDEMNITY" runat="server" onblur="CalculateCPCPremium();" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvLICPC" runat="server" ControlToValidate="OC__EF_CPC_LIMIT_IDEMNITY" ErrorMessage="LI Claims Preparation Costs is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>

                            </td>
                            <td>
                                <asp:TextBox ID="OC__EF_CPC_RATE" runat="server" onblur="CalculateCPCPremium();" CssClass="form-control e-num4"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvRateCPC" runat="server" ControlToValidate="OC__EF_CPC_RATE" ErrorMessage="Rate Claims Preparation Costs is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="OC__EF_CPC_PREMIUM" runat="server" onblur="CalculateCPCRate();" CssClass="form-control e-num2"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPremiumCPC" runat="server" ControlToValidate="OC__EF_CPC_RATE" ErrorMessage="Premium Claims Preparation Costs is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>First Loss Average(Theft)</label></td>
                            <td>
                                <asp:CheckBox ID="OC__EF_FIRST_LOSS_AVG" runat="server" Text=" " CssClass="asp-check"></asp:CheckBox></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <label>Malicious Damage</label></td>
                            <td>
                                <asp:CheckBox ID="OC__EF_MALICOIUS_DAMAGE" runat="server" onclick="ToggleMD(this.checked,'false');" Text=" " CssClass="asp-check"></asp:CheckBox>
                            </td>
                            <td>
                                <asp:TextBox ID="OC__EF_MDMG_LIMIT_IDEMNITY" runat="server" onblur="CalculateMDPremium();" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvLIMD" runat="server" ControlToValidate="OC__EF_MDMG_LIMIT_IDEMNITY" ErrorMessage="LI Malicious Damage is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>

                            <td>
                                <asp:TextBox ID="OC__EF_MDMG_RATE" runat="server" onblur="CalculateMDPremium();" CssClass="form-control e-num4"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvRateMD" runat="server" ControlToValidate="OC__EF_MDMG_RATE" ErrorMessage="Rate Malicious Damage is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="OC__EF_MDMG_PREMIUM" runat="server" onblur="CalculateMDRate();" CssClass="form-control e-num2"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfcPremiumMD" runat="server" ControlToValidate="OC__EF_MDMG_PREMIUM" ErrorMessage="Premium Malicious Damage is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Riot  Strike</label></td>
                            <td>
                                <asp:CheckBox ID="OC__EF_RIOT_STRIKE" runat="server" onclick="ToggleRS(this.checked);" Text=" " CssClass="asp-check"></asp:CheckBox>

                            </td>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="OC__EF_RS_PREMIUM" runat="server" CssClass="form-control e-num2"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvRoitStrikePremium" runat="server" ControlToValidate="OC__EF_RS_PREMIUM" ErrorMessage="Premium Riot Strike is a required field" Enabled="false" SetFocusOnError="true" Display="none"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </div>
                <legend>Reinsurance Exposure</legend>
                <div class="grid-card table-responsive">
                    <table class="grid-table">
                        <tr>
                            <th width="20%"></th>
                            <th width="20%">
                                <label>Total Sum Insured</label></th>
                            <th width="20%">
                                <label>Target Risk SI</label></th>
                            <th width="20%">
                                <label>MPL %</label></th>
                            <th width="20%">
                                <label>RI Exposure</label></th>
                        </tr>
                        <tr>
                            <td>Contents</td>
                            <td>
                                <asp:TextBox ID="OC__CONTENT_TOTAL_SI" runat="server" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="OC__CONTENT_TARGET_RISK_SI" runat="server" onblur="CalculateTargetRiskSI();" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="OC__CONTENT_MPL" runat="server" CssClass="form-control e-num2" onblur="CalculateTargetRiskSI();"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="OC__CONTENT_RI_EXPOSURE" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>25% Rent</td>
                            <td>
                                <asp:TextBox ID="OC__RENT_TOTAL_SI" runat="server" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="OC__RENT_TARGET_RISK_SI" runat="server" onblur="CalculateTargetRiskSI();" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="OC__RENT_MPL" runat="server" CssClass="form-control  e-num2" onblur="CalculateTargetRiskSI();"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="OC__RENT_RI_EXPOSURE" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Increased in 
Cost of Working</td>
                            <td>
                                <asp:TextBox ID="OC__ICOST_SI" runat="server" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="OC__ICOST_TARGET_RISK_SI" runat="server" onblur="CalculateTargetRiskSI();" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="OC__ICOST_MPL" runat="server" CssClass="form-control  e-num2" onblur="CalculateTargetRiskSI();"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="OC__ICOST_RI_EXPOSURE" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Liability Document
Loss of Document</td>
                            <td>
                                <asp:TextBox ID="OC__LI_TOTAL_SI" runat="server" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="OC__LI_TARGET_RISK_SI" runat="server" onblur="CalculateTargetRiskSI();" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="OC__LI_MPL" runat="server" CssClass="form-control e-num2" onblur="CalculateTargetRiskSI();"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="OC__LI_RI_EXPOSURE" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Claims Preparation 
Costs</td>
                            <td>
                                <asp:TextBox ID="OC__CL_TOTAL_SI" runat="server" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="OC__CL_TARGET_RISK_SI" runat="server" onblur="CalculateTargetRiskSI();" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="OC__CL_MPL" runat="server" onblur="CalculateTargetRiskSI();" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="OC__CL_RI_EXPOSURE" runat="server" CssClass="form-control"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Additional Increase in
 Cost of Working</td>
                            <td>
                                <asp:TextBox ID="OC__AIW_TOTAL_SI" runat="server" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="OC__AIW_TARGET_RISK_SI" runat="server" onblur="CalculateTargetRiskSI();" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="OC__AIW_MPL" runat="server" CssClass="form-control e-num2" onblur="CalculateTargetRiskSI();"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="OC__AIW_RI_EXPOSURE" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Total RI Exposure</td>
                            <td>
                                <asp:TextBox ID="OC__TRE_TOTAL_SI" runat="server" CssClass="form-control"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="OC__TRE_TARGET_RISK_SI" runat="server" CssClass="form-control"></asp:TextBox></td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="OC__TRE_RI_EXPOSURE" runat="server" CssClass="form-control e-num2"></asp:TextBox></td>
                        </tr>
                    </table>
                </div>
                <asp:HiddenField ID="HiddenField1" runat="server"></asp:HiddenField>
                <legend><span>Office Contents Overview</span></legend>
                <legend>Endorsement</legend>
                <NexusControl:standardwording runat="server" id="OC__ENDORESEMENT" supportrisklevel="true"></NexusControl:standardwording>
            </div>
            <div class='card-footer'>
                <asp:LinkButton ID="btnBack" runat="server" Text="<i class='fa fa-chevron-left' aria-hidden='true'></i> Back" OnClick="BackButton" CausesValidation="false" SkinID="btnSecondary"></asp:LinkButton>
                <asp:LinkButton ID="btnNext" runat="server" Text="Next <i class='fa fa-chevron-right' aria-hidden='true'></i>" OnClick="NextButton" OnClientClick="RemovePercent();" SkinID="btnPrimary"></asp:LinkButton>
                <asp:LinkButton ID="btnFinish" runat="server" Text="<i class='fa fa-check' aria-hidden='true'></i> Finish" OnClick="FinishButton" OnPreRender="PreRenderFinish" SkinID="btnPrimary"></asp:LinkButton>
            </div>
        </div>
        <asp:ValidationSummary ID="vldsumSummary" DisplayMode="BulletList" HeaderText="<h2>There are errors on this page</h2><p>Please review these errors and re-submit the form</p>" runat="server" CssClass="validation-summary"></asp:ValidationSummary>
    </div>
</asp:Content>
