Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Module BusinessSQL
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllSIRDocTemplate"

    Public Const ACGetAllDetailsSQL As String = "spe_document_template_saa"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckSIRDocTemplateID"

    Public Const ACCheckIDSQL As String = "spe_document_template_check_id"

    ' Check Code SQL
    Public Const ACCheckCodeStored As Boolean = True
    Public Const ACCheckCodeName As String = "CheckSIRDocTemplateCode"
    Public Const ACCheckCodeSQL As String = "spu_SIR_Check_DocTemplate_Code"

    'TN20010801 - start
    Public Const ACGetTemplatePrinterStored As Boolean = False
    Public Const ACGetTemplatePrinterName As String = "Get Document Template Printer Name"
    Public Const ACGetTemplatePrinterSQL As String = "SELECT printer FROM document_template" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                     "WHERE document_template_id = {document_template_id}"
    'TN20010801 - end

    'PN 34077 Get user Printer from PMuser table
    Public Const ACGetUserPrinterStored As Boolean = True
    Public Const ACGetUserPrinterName As String = "Get User Printer Name"
    Public Const ACGetUserPrinterSQL As String = "spu_Get_User_Printer"

    'AK 090402 - Chaser related SQL queries - begin
    Public Const ACGetAllChaserStored As Boolean = True
    Public Const ACGetAllChaserName As String = "GetAllChasers"

    Public Const ACGetAllChaserSQL As String = "spu_get_all_chasers"

    Public Const ACGetTemplateChaserStored As Boolean = True
    Public Const ACGetTemplateChaserName As String = "GetTemplateChaser"

    Public Const ACGetTemplateChaserSQL As String = "spu_get_template_chaser"

    'AK 090402 - Chaser related SQL queries - end

    'MKW 020503 PN4432 Start
    Public Const ACGetRiskGroupStored As Boolean = True
    Public Const ACGetRiskGroupName As String = "GetRiskGroup"

    Public Const ACGetRiskGroupSQL As String = "spu_select_risk_group_details_for_branch"

    Public Const ACGetRiskCodeStored As Boolean = True
    Public Const ACGetRiskCodeName As String = "GetRiskCode"

    Public Const ACGetRiskCodeSQL As String = "spu_select_risk_code_details_for_branch"
    'MKW 020503 PN4432 End

    Public Const ACGetDocumentTemplateSQL As String = "spu_get_document_template_saa"
    Public Const ACGetDocumentTemplateName As String = "GetDocumentTemplate"
    Public Const ACGetDocumentTemplateStored As Boolean = True

    Public Const kGetDocumentTemplateWordingWordingLinksName As String = "Returns the documents that are referencing this document"

    Public Const kGetDocumentTemplateWordingWordingLinksSQL As String = "spu_SIR_get_wording_wording_links"
	

	Public Const ACGetClientCode As String = "GetClientCode"
	Public Const ACGetClientCodeStored As Boolean = True
	Public Const ACGetClientCodeSQL As String = "spu_get_ClientCode"
	
	Public Const ACGetPMwrkTaskId As String = "GetPMwrkTaskId"
	Public Const ACGetPMwrkTaskIdStored As Boolean = True
	Public Const ACGetPMwrkTaskIdSQL As String = "spu_get_PMWrk_task_ID"
    Public Const ACSelEmailContactStored As Boolean = True
	Public Const ACSelEmailContactName As String = "Get Main Email ContactAddress"
	Public Const ACSelEmailContactSQL As String = "spu_email_contact_select"
    Public Const ACGetFurtherDetailsStored As Boolean = True
	Public Const ACGetFurtherDetailsName As String = "GetFurtherDocTemplateDetails"
	Public Const ACGetFurtherDetailsSQL As String = "spu_GetFurther_doc_template_details"
    Public Const kGetCopiesForDocumentTemplateName As String = "Returns the document template code and no of copies for to this document template"
    Public Const kGetCopiesForDocumentTemplateSQL As String = "spu_SIR_get_copies_for_document_template"

    Public Const kGetUniqueClauseCodeName As String = "Gets unique document template code"
    Public Const kGetUniqueClauseCodeSQL As String = "Spu_Get_Unique_Clause_Code"

    Public Const kUpdateDocumentTemplateDescName As String = "UpdateDocumentTemplateDesc"
    Public Const kUpdateDocumentTemplateDescSQL As String = "spu_document_template_description_upd"
    Public Const ACSelPolicyLevelEmailStored As Boolean = True
    Public Const ACSelPolicyLevelEmailName As String = "Get Policy Level Email Address"
    Public Const ACSelPolicyLevelEmailSQL As String = "spu_Get_Policy_Level_Email_Address"

    Public Const ACSelSenderEmailStored As Boolean = True
    Public Const ACSelSenderEmailName As String = "Get Sender Email Address"
    Public Const ACSelSenderEmailSQL As String = "spu_Get_Sender_Email_Address"
End Module