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
    ' Date: 20/07/2000
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRProduct.Business class.
    '
    ' Edit History:
    ' 25/07/2001    JMK     2 more columns in Product Table - Add and Update
    ' ***************************************************************** '

    'SQL Statements

    ' Select All SIRProduct SQL
    ' PW311002 - add follow up time frame and grace period
    Public Const ACAddProductStored As Boolean = True
    Public Const ACAddProductName As String = "AddProduct"
    Public Const ACAddProductSQL As String = "spe_Product_Add"

    ' PW311002 - add follow up time frame and grace period
    Public Const ACUpdProductStored As Boolean = True
    Public Const ACUpdProductName As String = "UpdProduct"
    Public Const ACUpdProductSQL As String = "spe_Product_upd"

    Public Const ACDelProductStored As Boolean = True
    Public Const ACDelProductName As String = "DeleteProduct"
    Public Const ACDelProductSQL As String = "spe_Product_Del"

    Public Const ACSelProductStored As Boolean = True
    Public Const ACSelProductName As String = "SelProduct"
    Public Const ACSelProductSQL As String = "spe_Product_Sel"

    Public Const ACSaaProductStored As Boolean = True
    Public Const ACSaaProductName As String = "SelAllProduct"
    'Public Const ACSaaProductSQL = "{call spe_Product_Saa}"
    Public Const ACSaaProductSQL As String = "spu_Product_Saa"

    Public Const ACAddCaptionIDStored As Boolean = True
    Public Const ACAddCaptionIDName As String = "AddPMCaption"
    Public Const ACAddCaptionIDSQL As String = "spu_pm_caption_id_return"

    Public Const ACSaaRiskTypeGroupStored As Boolean = True
    Public Const ACSaaRiskTypeGroupName As String = "SelAllRiskTypeGroup"
    Public Const ACSaaRiskTypeGroupSQL As String = "spe_Risk_Type_Group_saa"

    Public Const ACSelProductRiskTypeGroupStored As Boolean = True
    Public Const ACSelProductRiskTypeGroupName As String = "SelProductRiskTypeGroup"
    Public Const ACSelProductRiskTypeGroupSQL As String = "spu_Product_Risk_Type_Group_Sel"

    Public Const ACDelProductRiskTypeGroupStored As Boolean = True
    Public Const ACDelProductRiskTypeGroupName As String = "DeleteProductRiskTypeGroup"
    Public Const ACDelProductRiskTypeGroupSQL As String = "spu_Product_Risk_Type_Group_del"

    Public Const ACAddProductRiskTypeGroupStored As Boolean = True
    Public Const ACAddProductRiskTypeGroupName As String = "AddProductRiskTypeGroup"
    Public Const ACAddProductRiskTypeGroupSQL As String = "spu_Product_Risk_Type_Group_add"

    'True monthly Policy
    Public Const ACSelProductSuspendStored As Boolean = True
    Public Const ACSelProductSuspendName As String = "SelProductSuspend"
    Public Const ACSelProductSuspendSQL As String = "spe_Product_suspend"



    'TN20001031 (Start) process 29
    Public Const ACAddProductAllowedCausationStored As Boolean = True
    Public Const ACAddProductAllowedCausationName As String = "AddProductAllowedCausation"
    Public Const ACAddProductAllowedCausationSQL As String = "spu_Product_Causation_add"

    Public Const ACDelProductAllowedCausationStored As Boolean = True
    Public Const ACDelProductAllowedCausationName As String = "DelProductAllowedCausation"
    Public Const ACDelProductAllowedCausationSQL As String = "spu_Product_Causation_Del"

    Public Const ACSelProductAllowedCausationStored As Boolean = False
    Public Const ACSelProductAllowedCausationName As String = "SelProductAllowedCausation"
    Public Const ACSelProductAllowedCausationSQL As String = "SELECT pac.primary_cause_id, code, cap.caption" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                             "FROM Product_Allowed_Causation pac," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                             "Primary_Cause pc," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                             "PMCaption cap" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                             "WHERE pac.product_id ={product_id}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                             "AND pac.primary_cause_id = pc.primary_cause_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                             "AND pc.caption_id = cap.caption_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                             "AND cap.language_id = {language_id}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                             "ORDER BY 3"

    Public Const ACSelAvailableCausationStored As Boolean = False
    Public Const ACSelAvailableCausationName As String = "SelAvailableCausation"
    Public Const ACSelAvailableCausationSQL As String = "SELECT pc.primary_cause_id, code, cap.caption" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                        "FROM Primary_Cause pc," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                        "PMCaption cap" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                        "WHERE pc.caption_id = cap.caption_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                        "AND cap.language_id = {language_id}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                        "AND pc.is_deleted <> 1" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                        "AND pc.effective_date <= {effective_date}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                        "AND pc.primary_cause_id NOT IN" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                        "(SELECT primary_cause_id FROM Product_Allowed_Causation" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                        "WHERE product_id = {product_id})" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                        "ORDER BY 3"

    Public Const ACSelGetDocProduceSQL As String = "spu_getDocumentsFlag"
    Public Const ACSelGetDocProduceName As String = "GetDocumentProduce"


    'TN20001031 (End) process 29

    Public Const kGetNoOfPoliciesOnProductName As String = "Returns the number of policies on the specified product"
    Public Const kGetNoOfPoliciesOnProductSQL As String = "spu_Product_Get_No_Of_Policies"

    Public Const kGetSysAdminMemoTaskDetailsName As String = "Returns the required parameters for transaction suppression changed task"
    Public Const kGetSysAdminMemoTaskDetailsSQL As String = "spu_SIR_ProductRisk_Required_Task_Params_Select"

    Public Const ACChkAccountCodeName As String = "Validate Account Code"

    'Modifying the inline query to make it compatible with SQL server 2005
    Public Const ACChkAccountCodeSQL As String = "SELECT Short_code FROM Account LEFT OUTER JOIN party ON Account.Account_key=party.party_cnt AND party.is_deleted = 0 WHERE Short_code= {Code}"

    Public Const ACUpdPaymentMethodName As String = "Update Payment Method"
    Public Const ACUpdPaymentMethodSQL As String = " Update product set default_payment_method={paymentmethod} where product_id={product_id}"

    Public Const ACGetProductIDName As String = "Get Product ID"
    Public Const ACGetProductIDSQL As String = "Select Product_id from insurance_file where insurance_file_cnt = {ifilecnt}"

    Public Const ACGetProductDetailsForClaimName As String = "Get Product Details for Claim"
    Public Const ACGetProductDetailsForClaimSQL As String = "spu_Get_Product_Details_For_Claim"

    Public Const kGetClaimWorkFlowName As String = "GetClaimWorkFlow"
    Public Const kGetClaimWorkFlowSQL As String = "spu_SIR_Product_Claims_Workflow_Sel"

    Public Const kAddClaimWorkFlowName As String = "GetClaimWorkFlow"
    Public Const kAddClaimWorkFlowSQL As String = "spu_SIR_Product_Claims_Workflow_Add"

    Public Const kUpdClaimWorkFlowName As String = "GetClaimWorkFlow"
    Public Const kUpdClaimWorkFlowSQL As String = "spu_SIR_Product_Claims_Workflow_Upd"

    Public Const kGetClaimWorkflowForClaimName As String = "GetClaimWorkFlowForClaim"
    Public Const kGetClaimWorkflowForClaimSQL As String = "spu_SIR_Product_Claims_Workflow_Sel_By_Claim"

    'Start (Sriram P) Tech Spec WR19 - Cover Note Functionality.doc section(4.8.1.1)

    Public Const ACGetProductValueStored As Boolean = True
    Public Const ACGetProductValueName As String = "Product select"
    'Developer Guide No 39. 
    Public Const ACGetProductValueSQL As String = "spu_SAM_Product_sel"

    'End (Sriram P) Tech Spec WR19 - Cover Note Functionality.doc section(4.8.1.1)

    'Start-(Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)-(6.4.2.1)
    Public Const ACGetProductValuesFromInsuranceFileIDName As String = "Get Product Value from Insurance File Cnt"
    Public Const ACGetProductValuesFromInsuranceFileIDSQL As String = "spu_Get_Product_Values_From_Insurance_File_Cnt"
    Public Const ACGetProductValuesFromInsuranceFileIDStored As Boolean = True
    'End-(Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)-(6.4.2.1)

    Public Const ACSaveProductBranchSQL As String = "spu_SIR_Save_ProductSource"
    Public Const ACSaveProductCausationSQL As String = "spu_SIR_SaveCausation"

End Module