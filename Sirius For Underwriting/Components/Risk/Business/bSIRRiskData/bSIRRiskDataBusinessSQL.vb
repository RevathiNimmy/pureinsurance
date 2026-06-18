Option Strict Off
Option Explicit On
Module BusinessSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: BusinessSQL
    '
    ' Date: 02/09/2000
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRRenInvitePrint.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    Public Const ACSaaRiskStored As Boolean = True
    Public Const ACSaaRiskName As String = "GetAllRisks"
    Public Const ACSaaRiskSQL As String = "spe_Risk_saa"

    Public Const ACAddRiskStored As Boolean = True
    Public Const ACAddRiskName As String = "AddRisk"
    ' PW021202
    Public Const ACAddRiskSQL As String = "spe_Risk_add"

    Public Const ACSelGisPolicyLinkStored As Boolean = True
    Public Const ACSelGisPolicyLinkName As String = "GetGisPolicyLink"
    Public Const ACSelGisPolicyLinkSQL As String = "spu_gis_policy_link_sel"

    Public Const ACSelRSASumInsuredStored As Boolean = False
    Public Const ACSelRSASumInsuredName As String = "GetRSASumInsured"
    Public Const ACSelRSASumInsuredSQL As String = "SELECT SI.RSA_policy_binder_id, SI.sum_insured_type_id, " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                   "SI.sequence_id, SI.description, SI.reference," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                   "SI.sum_insured, SI.date_added, SI.date_deleted," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                   "SI.is_valuation_required, SI.valuation_date," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                   "SI.rate, premium" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                   "FROM RSA_Policy_Binder PB," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                   "RSA_Sum_Insured SI" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                   "Where PB.gis_policy_link_id ={gis_policy_link_id}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                   "AND PB.RSA_policy_binder_id = SI.RSA_policy_binder_id"


    Public Const ACSelPolicyBinderStored As Boolean = False
    Public Const ACSelPolicyBinderName As String = "GetPolicyBinder"
    Public Const ACSelPolicyBinderSQL As String = "SELECT rsa_policy_binder_id FROM RSA_Policy_Binder" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "WHERE gis_policy_link_id = {gis_policy_link_id}"

    Public Const ACAddSumInsuredStored As Boolean = False
    Public Const ACAddSumInsuredName As String = "AddSumInsured"
    Public Const ACAddSumInsuredSQL As String = "INSERT INTO RSA_Sum_Insured (" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "rsa_policy_binder_id, sum_insured_type_id," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "sequence_id, description," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "reference, sum_insured," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "date_added, date_deleted," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "is_valuation_required, valuation_date," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "rate, premium)" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "VALUES (" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "{rsa_policy_binder_id}," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "{sum_insured_type_id}," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "{sequence_id}," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "{description}," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "{reference}," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "{sum_insured}," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "{date_added}," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "{date_deleted}," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "{is_valuation_required}," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "{valuation_date}," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "{rate}," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "{premium})"


    'RWH(20/11/2000) New proc to add record to insurance_file_risk_link.
    Public Const ACAddRiskLinkStored As Boolean = True
    Public Const ACAddRiskLinkName As String = "AddRiskLink"
    Public Const ACAddRiskLinkSQL As String = "spe_insurance_file_risk_li_add"

    'RWH(23/11/2000) New proc to copy Rating_Section records from old to new risk.
    Public Const ACCopyRatingSectionStored As Boolean = True
    Public Const ACCopyRatingSectionName As String = "CopyRatingSection"
    Public Const ACCopyRatingSectionSQL As String = "spu_Copy_Rating_Section"

    'RWH(23/11/2000) New proc to copy Peril records from old to new risk.
    Public Const ACCopyPerilsStored As Boolean = True
    Public Const ACCopyPerilsName As String = "CopyPerils"
    Public Const ACCopyPerilsSQL As String = "spu_Copy_Perils"

    'TN20010717
    Public Const ACUpdRiskStatusStored As Boolean = False
    Public Const ACUpdRiskStatusName As String = "Update risk status"
    Public Const ACUpdRiskStatusSQL As String = "UPDATE risk SET risk_status_id = {risk_status_id}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "WHERE risk_cnt = {risk_cnt}"

    Public Const ACGetAutoReinsuredStored As Boolean = False
    Public Const ACGetAutoReinsuredName As String = "Get Auto Reinsured"
    Public Const ACGetAutoReinsuredSQL As String = "SELECT is_auto_reinsured FROM risk_type" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                   "WHERE risk_type_id = {risk_type_id}"


    Public Const ACCopySumsInsuredStored As Boolean = True
    Public Const ACCopySumsInsuredName As String = "Copy Sums Insured"
    Public Const ACCopySumsInsuredSQL As String = "spu_copy_sums_insured"

    Public Const ACGetRiskStatusStored As Boolean = True
    Public Const ACGetRiskStatusName As String = "GetRiskStatus"
    Public Const ACGetRiskStatusSQL As String = "spu_GetRiskStatus"

    'sj 29/01/2003 - start
    'PS104
    Public Const ACGetRiskAllStatusesStored As Boolean = True
    Public Const ACGetRiskAllStatusesName As String = "GetRiskAllStatuses"
    Public Const ACGetRiskAllStatusesSQL As String = "spu_SIR_Risk_By_Insurance_File_Sel"
    'sj 29/01/2003 - end

    ' Delete Insurance File Risk Link Details SQL
    Public Const ACDeleteInsuranceFileRiskLinkDetailsStored As Boolean = True
    Public Const ACDeleteInsuranceFileRiskLinkDetailsName As String = "DeleteInsuranceFileRiskLinkDetails"
    Public Const ACDeleteInsuranceFileRiskLinkDetailsSQL As String = "spe_Insurance_File_Risk_Li_del"

    Public Const ACDeleteInsuranceFileRiskLinkDetails2Stored As Boolean = True
    Public Const ACDeleteInsuranceFileRiskLinkDetails2Name As String = "DeleteInsuranceFileRiskLinkDetails2"
    Public Const ACDeleteInsuranceFileRiskLinkDetails2SQL As String = "spu_SIR_insurance_file_risk_link_del"

    ' AMB 26/06/2003: 1.9 IAG PS068 Date Effective Rating - save original_risk_cnt to Risk record
    Public Const ACAddRiskRenewalLinkStored As Boolean = True
    Public Const ACAddRiskRenewalLinkName As String = "AddRiskRenewalLink"
    Public Const ACAddRiskRenewalLinkSQL As String = "spu_SIR_Risk_Renewal_Original_Risk_Cnt_upd"

    'sj 20/12/2002 - start
    'PS104
    Public Const ACCopyRiskExtrasStored As Boolean = True
    Public Const ACCopyRiskExtrasName As String = "CopyRatingSection"
    Public Const ACCopyRiskExtrasSQL As String = "spu_copy_risk_extras"

    'Get Uncopied Risks
    Public Const ACGetAllUnCopiedRiskStored As Boolean = True
    Public Const ACGetAllUnCopiedRiskName As String = "GetAllUnCopiedRisk"
    Public Const ACGetAllUnCopiedRiskSQL As String = "spu_Get_Uncopied_Risks"


    ' Copy Risk Folder SQL
    Public Const ACCopyRiskFolderStored As Boolean = True
    Public Const ACCopyRiskFolderName As String = "CopyRiskFolder"
    Public Const ACCopyRiskFolderSQL As String = "spu_Copy_Risk_Folder"

    Public Const kSaaRenRiskSQL As String = "spe_Risk_RenSel_saa"
    Public Const kSaaRenRiskName As String = "GetAllRenewalRisks"
    Public Const kSaaRenRiskStored As Boolean = True
    'Select 
    Public Const ACGetRIModelTypeStored As Boolean = True
    Public Const ACGetRIModelTypeName As String = "Get RI Model Type"
    Public Const ACGetRIModelTypeSQL As String = "spu_Get_RI_Model_Type"

End Module