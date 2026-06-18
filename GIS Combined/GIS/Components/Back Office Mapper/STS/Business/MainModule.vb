Option Strict Off
Option Explicit On
Imports SSP.Shared
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  CL020600
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    Public Const ACDataModelCode As String = "AOL"
    Public Const ACBusinessTypeCode As String = "BTC"

    Public Const ACDataModelCodePrefix As String = ""

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bGISBOM" & ACDataModelCode

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' ***********************************************************'
    ' SQL String Constants
    ' ***********************************************************'

    ' Common
    Public Const ACSQLStartCol As String = " ( "
    Public Const ACSQLEndCol As String = " ) "
    Public Const ACSQLStartParam As String = " {"
    Public Const ACSQLEndParam As String = "} "
    Public Const ACSQLSeparator As String = " , "
    Public Const ACSQLAnd As String = " AND "
    Public Const ACSQLEquals As String = " = "
    Public Const ACSQLWhere As String = " WHERE "

    ' Add SQL Constants
    Public Const ACAddSQLStart As String = "INSERT INTO "
    Public Const ACAddSQLValues As String = " VALUES "

    ' Update SQL Constants
    Public Const ACUpdSQLStart As String = "UPDATE "
    Public Const ACUpdSQLSet As String = " SET "

    ' Select SQL Constants
    Public Const ACSelSQLSelect As String = "SELECT "
    Public Const ACSelSQLFrom As String = " FROM "
    Public Const ACSelSQLOrderBy As String = " ORDER BY "
    Public Const ACSelSQLAsc As String = " ASC"

    ' ID Col
    Public Const ACIDCol As String = "_ID"

    Public Const ACOIMGISSubKey As String = "GIS" ' CL230100


    Public Const ACAdditionalArrayRowName As Integer = 0 ' CL161100
    Public Const ACAdditionalArrayRowValue As Integer = 1 ' CL161100
    Public Const ACAdditionalArrayColumnBrand As Integer = 0 ' CL161100

    ' RAG201100
    ' Email types for SendEmail Method
    Public Const ACEmailTellAFriend As Integer = 1
    Public Const ACEmailConsumerMTA As Integer = 2

    ' Registry Sub Key for GIS Settings
    Public Const GISRegLegalPartyCnt As String = "GISRegLegalPartyCnt"
    Public Const GISRegBreakdownPartyCnt As String = "GISRegBreakdownPartyCnt"
    Public Const GISRegPromptPartyCnt As String = "GISRegPromptPartyCnt"
    Public Const GISRegCreditCardChargePartyCnt As String = "GISRegCreditCardChargePartyCnt"
    Public Const GISRegLegalamount As String = "GISRegLegalAmount"
    Public Const GISRegLegalCommissionamount As String = "GISRegLegalCommissionAmount"
    Public Const GISRegBreakdownamount As String = "GISRegBreakdownAmount"
    Public Const GISRegBreakdownCommissionamount As String = "GISRegBreakdownCommissionAmount"

    '**** START CHANGES - Changed By: AAB  - Changed On: 05-Sep-2002 10:59   ****
    '**** Constants for QuotePolicy.GetRiskByProduct Methond
    Public Const ACGetRiskByProductStored As Boolean = False
    Public Const ACGetRiskByProductName As String = "RiskByProduct"
    Public Const ACGetRiskByProductSQL As String = "SELECT DISTINCT rt.risk_type_id, c.caption, rt.gis_screen_id, gdm.code AS DataModel, gs.code AS Screen " & "FROM Risk_Type rt INNER JOIN GIS_Screen gs ON rt.gis_screen_id = gs.gis_screen_id INNER JOIN " & "GIS_Data_Model gdm ON gs.gis_data_model_id = gdm.gis_data_model_id INNER JOIN " & "Risk_Type_Usage rtu ON rt.risk_type_id = rtu.risk_type_id INNER JOIN " & "Product_Risk_Type_Group prtg ON rtu.risk_type_group_id = prtg.risk_type_group_id INNER JOIN " & "PMCaption c ON rt.caption_id = c.caption_id " & "WHERE (c.language_id = 1) And (rt.is_deleted = 0)  And (rt.gis_screen_id Is Not Null) " & "And (prtg.product_id = {Product_ID})"

    Public Const ACGetQuoteDetailsStored As Boolean = False
    Public Const ACGetQuoteDetailsName As String = "QuoteDetials"
    ' CTAF 20030716 - Underwriting version
    Public Const ACGetQuoteDetailsSFUSQL As String = "SELECT ifi.insurance_file_cnt, ifi.policy_version, ifs.last_trans_date, ift.code, ifi.cover_start_date, ifi.expiry_date, ifi.renewal_date, ifi.insurance_ref, isnull(ifs.last_trans_description,  ' ') as last_trans_description, " & "ifs.date_created, Party.resolved_name as Name, ifi.insured_name as InsuredName,  ifi.product_id, Insurance_Folder.insurance_folder_cnt, ifi.currency_id, Insurance_Folder.description, Insurance_Folder.insurance_holder_cnt, ifi.quote_expiry_date, sb.code, ifi.lead_allow_consolidated_commission, ifi.sub_allow_consolidated_commission " & "FROM Insurance_File ifi INNER JOIN " & "Insurance_File_System ifs ON ifi.insurance_file_cnt = ifs.insurance_file_cnt INNER JOIN " & "Insurance_File ifi2 ON ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt INNER JOIN " & "Insurance_File_Type ift ON ifi.insurance_file_type_id = ift.insurance_file_type_id INNER JOIN " & "Insurance_Folder ON ifi.insurance_folder_cnt = Insurance_Folder.insurance_folder_cnt INNER JOIN " & "Party ON Insurance_Folder.insurance_holder_cnt = Party.party_cnt INNER JOIN " & "sub_branch sb on party.sub_branch_id = sb.sub_branch_id " & "WHERE (ifi.policy_ignore Is Null) And (ifi2.insurance_file_cnt = {Insurance_File_Cnt}) And (ifi.insurance_file_status_id Is Null) " & "ORDER BY ifi.cover_start_date, ifi.expiry_date "
    ' CTAF 20030716 - Broking version
    'Public Const ACGetQuoteDetailsSBOSQL = "SELECT ifi.insurance_file_cnt, ifi.policy_version, ifs.last_trans_date, ift.code, ifi.cover_start_date, ifi.expiry_date, ifi.renewal_date, ifi.insurance_ref, isnull(ifs.last_trans_description,  ' ') as last_trans_description, " _
    ''                                    & "ifs.date_created, Party.resolved_name as Name, ifi.insured_name as InsuredName,  rc.risk_group_id, Insurance_Folder.insurance_folder_cnt, ifi.risk_code_id " _
    ''                                    & "FROM Insurance_File ifi INNER JOIN " _
    ''                                    & "Insurance_File_System ifs ON ifi.insurance_file_cnt = ifs.insurance_file_cnt INNER JOIN " _
    ''                                    & "Insurance_File ifi2 ON ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt INNER JOIN " _
    ''                                    & "Insurance_File_Type ift ON ifi.insurance_file_type_id = ift.insurance_file_type_id INNER JOIN " _
    ''                                    & "Insurance_Folder ON ifi.insurance_folder_cnt = Insurance_Folder.insurance_folder_cnt INNER JOIN " _
    ''                                    & "Party ON Insurance_Folder.insurance_holder_cnt = Party.party_cnt " _
    ''                                    & "INNER JOIN Risk_Code rc ON rc.risk_code_id = ifi.risk_code_id " _
    ''                                    & "WHERE (ifi.policy_ignore Is Null) And (ifi2.insurance_file_cnt = {Insurance_File_Cnt}) And (ifi.insurance_file_status_id Is Null) " _
    ''                                    & "ORDER BY ifi.cover_start_date, ifi.expiry_date "


    Public Const ACGetRiskScreenDataModelCodeStored As Boolean = False
    Public Const ACGetRiskScreenDataModelCodeName As String = "QuoteDetials"
    Public Const ACGetRiskScreenDataModelCodeSQL As String = "SELECT GIS_Screen.code 'Screen Code', GIS_Data_Model.code 'DataModel' " & "FROM GIS_Screen INNER JOIN GIS_Data_Model ON GIS_Screen.gis_data_model_id = GIS_Data_Model.gis_data_model_id " & "WHERE GIS_Screen_Id = {Screen_ID} "

    ' CTAF 20030728 - Start
    Public Const ACGetPolicyLinkIDSQL As String = "spu_gis_policy_link_sel"
    Public Const ACGetPolicyLinkIDName As String = "GetPolicyLinkID"
    Public Const ACGetPolicyLinkIDStored As Boolean = True
    ' CTAF 20030728 - End

    'SIRIUS ID constants
    'Logon details
    Public Const SIRIUS_USERID As Integer = 1
    Public Const SIRIUS_SOURCEID As Integer = 1
    Public Const SIRIUS_LANGUAGEID As Integer = 1
    Public Const SIRIUS_CURRENCYID As Integer = 26
    '****   END CHANGES - Changed By: AAB  - Changed On: 05-Sep-2002 10:59   ****

    '**** START CHANGES - Changed By: AAB  - Changed On: 30102002   ****
    '**** Added these constast to be used in NBQuoteAfter
    Public Const CNCurrentRiskCnt As String = "CURRENT_RISK_CNT"
    Public Const CNInsuranceFolderCnt As String = "CONTROL__INSURANCE_FOLDER_CNT"
    Public Const CNInsuranceFileCnt As String = "CONTROL__INSURANCE_FILE_CNT"
    Public Const CNTransactionType As String = "TRANSACTION_TYPE"
    '****   END CHANGES - Changed By: AAB  - Changed On: 30102002   ****
    'AAB-20112002 - Added this to be used in NBTransactAfter
    Public Const CNClaimMode As String = "CLAIM_MODE"
    Public Const CNNBTransactMessage As String = "NBTRANSACT_MESSAGE"
    Public Const CNNewPolicyNumber As String = "NEW_POLICY_NUMBER"
    'AAB-11122002 - Added to used in NBQuoteAfter
    Public Const CNTaxesArray As String = "TAXES_ARRAY"
    Public Const CNAgentCommArray As String = "AGENT_COMMISSION_ARRAY"
    'RDT 11022003
    Public Const CNInsurerCnt As String = "CONTROL__INSURER_CNT"
    Public Const CNRiskGroupId As String = "CONTROL__RISK_GROUP_ID"
    Public Const CNRiskCodeId As String = "CONTROL__RISK_CODE_ID"
    Public Const CNCoverFromDate As String = "CONTROL__COVER_FROM_DATE"
    Public Const CNCoverToDate As String = "CONTROL__COVER_TO_DATE"
    Public Const CNCoverType As String = "CONTROL__COVER_TYPE"
    Public Const CNSchemeId As String = "CONTROL__SCHEME_ID"
    Public Const CNNewSchemeId As String = "CONTROL__NEW_SCHEME_ID"
    Public Const CNEdiSolution As String = "EDI_SOLUTION"
    Public Const CNLastEdiMessageCountSent As String = "LAST_EDI_MESSAGE_COUNT_SENT"
    'RDT 13032003 - Added constant for Additional data array
    Public Const CNCalledFromSTS As String = "CalledFromSTS"
    Public Const CNSkipSaveToDB As String = "SKIPSAVE"
    ' CTAF 20030306 - GetProductsByAgent
    Public Const ACGetProductsSQL As String = "spu_GetProductsForAgentsViaBranch"
    Public Const ACGetProductsStored As Boolean = True
    Public Const ACGetProductsName As String = "GetProductsForAgents"
    Public Const CNAddressCountry As String = "AddressCountry"

    ' Error codes
    Public Const ACTransErrPeriodNotDef As Integer = 1000

    ' CTAF 20040302 - Codes for checking alternate reference SQL - start
    Public Const ACCheckAltRefSQL As String = "spu_chk_alt_ref_in_use"
    Public Const ACCheckAltRefName As String = "CheckAltRef"
    Public Const ACCheckAltRefStored As Boolean = True
    ' CTAF 20040302 - Codes for checking alternate reference SQL - end
    'SJ 08/03/2004 - start
    Public Const ACGetPolicyVersionForMtaAltReferenceSQL As String = "spu_GetPolicyVersionForMtaAltReference"
    Public Const ACGetPolicyVersionForMtaAltReferenceName As String = "GetPolicyVersionForMtaAltReference"
    Public Const ACGetPolicyVersionForMtaAltReferenceStored As Boolean = True
    '
    'Public Const ACGetFPLEDILastMessageReceivedSQL As String = "{call spu_Get_FPLEDI_last_message_received(?)}"
    'Public Const ACGetFPLEDILastMessageReceivedName As String = "GetFPLEDILastMessageReceived"
    'Public Const ACGetFPLEDILastMessageReceivedStored As Boolean = True

    Public Const ACUpdateLastEdiMessageCountReceivedSQL As String = "spu_last_message_count_received_upd"
    Public Const ACUpdateLastEdiMessageCountReceivedName As String = "UpdateLastMessageReceived"
    Public Const ACUpdateLastEdiMessageCountReceivedStored As Boolean = True

    Public Const ACGetRiskGroupCodeIdsStored As Boolean = True
    Public Const ACGetRiskGroupCodeIdsName As String = "GetRiskGroupCodeIds"
    Public Const ACGetRiskGroupCodeIdsSQL As String = "spu_Get_Risk_Group_Code_Ids"

    Public Const ACGetMaxPolVersionStored As Boolean = True
    Public Const ACGetMaxPolVersionName As String = "GetMaxPolVersion"
    Public Const ACGetMaxPolVersionSQL As String = "spu_SIR_Get_max_PolVersion"
    'SJ 08/03/2004 - end

    'SJ 27/05/2004 - start
    Public Const ACGetQuoteDetailsSBOStored As Boolean = True
    Public Const ACGetQuoteDetailsSBOName As String = "GetQuoteDetailsSBO"
    Public Const ACGetQuoteDetailsSBOSQL As String = "spu_gis_get_quote_details_sbo"
    'SJ 27/05/2004 - end

    Public Const ACGetGISPolicyLinkStored As Boolean = True
    Public Const ACGetGISPolicyLinkName As String = "GetGISPolicyLink"
    Public Const ACGetGISPolicyLinkSQL As String = "spu_gis_policy_link_sel"

    Public Const ACGetIPTFromGPLStored As Boolean = True
    Public Const ACGetIPTFromGPLName As String = "GetIPTFromGPL"
    Public Const ACGetIPTFromGPLSQL As String = "spu_ipt_from_policy_link_sel"

    Public Const ACPremiumTolerancesStored As Boolean = True
    Public Const ACPremiumTolerancesName As String = "PremiumTolerances"
    Public Const ACPremiumTolerancesSQL As String = "spu_SIRRen_Get_Premium_Tolerances"

    Public Const ACGetSchemeGroupStored As Boolean = True
    Public Const ACGetSchemeGroupName As String = "GetRiskGroup"
    Public Const ACGetSchemeGroupSQL As String = "spu_PMB_Scheme_Group_Sel"

    Public Const ACGetIptStored As Boolean = True
    Public Const ACGetIptName As String = "GetIpt"
    Public Const ACGetIptSQL As String = "spu_cnc_get_ipt"

    Public Const ACDeleteExistingReinsuranceStored As Boolean = True
    Public Const ACDeleteExistingReinsuranceName As String = "DeleteRiskReinsurance"
    Public Const ACDeleteExistingReinsuranceSQL As String = "spu_SAM_Delete_Risk_Reinsurance"

    Public Const ACDeleteOriginalReinsuranceStored As Boolean = True
    Public Const ACDeleteOriginalReinsuranceName As String = "DeleteOriginalRiskReinsurance"
    Public Const ACDeleteOriginalReinsuranceSQL As String = "spu_SAM_Delete_Original_Risk_Reinsurance"

    ' ***************************************************************** '
    ' Name: GeneratePassword
    '
    ' Description: Generate a Password containing 8 random A to Z Chars.
    '
    ' ***************************************************************** '
    Public Function GeneratePassword(ByRef r_sPassword As String) As Integer

        Dim result As Integer = 0
        Dim MyValue As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialize random-number generator.
            Dim rnd As New Random

            ' Generate 8 Chars
            For lSub As Integer = 1 To 8

                ' Generate random value between 65 and 91.
                ' i.e. ASCII A to Z - but exclude any vowels
                Do
                    MyValue = Rnd.Next(65, 91)
                Loop While MyValue = 65 Or MyValue = 69 Or MyValue = 73 Or MyValue = 79 Or MyValue = 85

                ' Convert to a Char and add to Password
                r_sPassword = r_sPassword & Strings.ChrW(MyValue).ToString()

            Next lSub

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GeneratePasswordFailed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="GeneratePassword", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Function: CreateNewBusinessObject
    '
    ' Description: Create and initialises a business object
    '
    ' Edit History:
    '   PWF 06/11/2001 - Created
    ' ***************************************************************** '
    Public Function CreateNewBusinessObject(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, ByRef r_oObject As Object, ByVal v_sClassName As String, Optional ByVal v_oDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object
            r_oObject = gPMFunctions.CreateLateBoundObject(v_sClassName)

            ' Initialise the object
            If False Then

                lReturn = r_oObject.Initialise(sUsername:=ToSafeString(sUserName), sPassword:=ToSafeString(sPassword), iUserID:=ToSafeInteger(iUserID),
                                               iSourceID:=ToSafeInteger(iSourceID), iLanguageID:=ToSafeInteger(iLanguageID), iCurrencyID:=ToSafeInteger(iCurrencyID),
                                               iLogLevel:=ToSafeInteger(iLogLevel), sCallingAppName:=ToSafeString(ACApp))
            Else

                lReturn = r_oObject.Initialise(sUserName:=ToSafeString(sUserName), sPassword:=ToSafeString(sPassword), iUserID:=ToSafeInteger(iUserID),
                                               iSourceID:=ToSafeInteger(iSourceID), iLanguageID:=ToSafeInteger(iLanguageID), iCurrencyID:=ToSafeInteger(iCurrencyID),
                                               iLogLevel:=ToSafeInteger(iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=v_oDatabase)
            End If

            ' Check error
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_oObject = Nothing
                result = lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=ToSafeString(sUserName), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateNewBusinessObject Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="CreateNewBusinessObject", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Function: AddToArray
    '
    ' Description: Add a new element pair to the additional data array
    '
    ' Edit History:
    '   PWF 08/11/2001 - Created
    ' ***************************************************************** '
    Public Function AddToArray(ByVal v_sUsername As String, ByRef r_vAdditionalDataArray As Array, ByVal v_vElementName As Object, ByVal v_vElementValue As Object) As Integer

        Dim result As Integer = 0
        Dim iLower, iUpper As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Ensure we have an array
            If Informations.IsArray(r_vAdditionalDataArray) Then
                iLower = r_vAdditionalDataArray.GetLowerBound(1)
                iUpper = r_vAdditionalDataArray.GetUpperBound(1) + 1

                r_vAdditionalDataArray = ArraysHelper.RedimPreserve(Of Object(,))(r_vAdditionalDataArray, New Integer() {2, iUpper - iLower + 1}, New Integer() {0, iLower})
            Else
                r_vAdditionalDataArray = Array.CreateInstance(GetType(Object), New Integer() {2, 1}, New Integer() {0, 0})
            End If



            r_vAdditionalDataArray(0, iUpper) = v_vElementName


            r_vAdditionalDataArray(1, iUpper) = v_vElementValue

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddToArray Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddToArray", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Function: ParseArray
    '
    ' Description: Parse the additional data array for the given element pair
    '
    ' Edit History:
    '   PWF 08/11/2001 - Created
    ' ***************************************************************** '
    Public Function ParseArray(ByVal v_sUsername As String, ByVal v_vAdditionalDataArray As Array, ByVal v_vElementName As Object, ByRef r_vElementValue As Object) As Integer

        Dim result As Integer = 0
        Dim iLower, iUpper As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Ensure we have an array
            If Informations.IsArray(v_vAdditionalDataArray) Then
                iLower = v_vAdditionalDataArray.GetLowerBound(1)
                iUpper = v_vAdditionalDataArray.GetUpperBound(1)

                For iCount As Integer = iLower To iUpper


                    If v_vAdditionalDataArray(0, iCount).Equals(v_vElementName) Then


                        r_vElementValue = v_vAdditionalDataArray(1, iCount)
                        Exit For
                    End If
                Next iCount
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ParseArray Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="ParseArray", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************************
    '
    ' Property    : IsUnderwriting186
    '
    ' Description : Returns True or False depending on whether the current
    '               system is Underwriting version 1.8.6
    '
    ' History     : CTAF 20031126 Created
    '
    ' ***************************************************************************
    Public ReadOnly Property IsUnderWriting186() As Boolean
        Get

            Dim sVersion, sRelease, sSiriusType As String
            Dim lReturn As Integer

            Try


                '    ' Get the values
                '    lReturn& = GetSiriusVersion( _
                ''                        sVersion:=sVersion, _
                ''                        sRelease:=sRelease, _
                ''                        sSiriusType:=sSiriusType)
                '
                '    If (lReturn <> PMTrue) Then
                '        IsUnderWriting186 = False
                '        Exit Property
                '    End If
                '
                '    Select Case sSiriusType
                '        Case "Underwriting"
                '
                '            Select Case sVersion
                '                Case "1.8.6"
                '                    IsUnderWriting186 = True
                '
                '                Case Else
                '                    IsUnderWriting186 = False
                '            End Select
                '
                '        Case Else
                '            IsUnderWriting186 = False
                '
                '    End Select

                Return True

            Catch excep As System.Exception



                ' Log Error Message
                bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsUnderWriting186 Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="IsUnderWriting186", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Exit Property

            End Try
        End Get
    End Property

    ' ************************************************************************************************
    '
    ' Function: AddToKeyArray
    '
    ' Description: Adds a name/value pair to a key array, resizing the array if needed
    '
    ' History: CTAF 20040304 - Created
    '
    ' ************************************************************************************************
    Public Function AddToKeyArray(ByRef r_vKeyArray(,) As Object, ByVal v_sName As String, ByVal v_vValue As Object) As Integer

        Dim lMax As Integer

        Try

            ' Resize the array
            If Informations.IsArray(r_vKeyArray) Then
                lMax = r_vKeyArray.GetUpperBound(1) + 1
                ReDim Preserve r_vKeyArray(1, lMax)
            Else
                lMax = 0
                ReDim r_vKeyArray(1, 0)
            End If

            ' Store the value

            r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lMax) = v_sName


            r_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lMax) = v_vValue

        Catch excep As System.Exception



            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddToKeyArray Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="AddToKeyArray", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

        End Try

    End Function

    Public Function SavePolNoInDB(ByVal v_oDatabase As dPMDAO.Database, ByVal v_sGisBusinessTypeCode As String, ByVal v_lPolicyBinderID As Integer, ByVal v_lPolicyID As Integer, ByVal v_sPolNo As String, Optional ByVal v_lInsuranceFileCnt As String = "") As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Save in the GIS Area
            sSQL = "UPDATE " & v_sGisBusinessTypeCode & "_POLICY "
            sSQL = sSQL & "SET Policy_No = '" & v_sPolNo & "' "
            sSQL = sSQL & "WHERE " & v_sGisBusinessTypeCode & "_Policy_binder_id = " & CStr(v_lPolicyBinderID) & " "
            sSQL = sSQL & "AND " & v_sGisBusinessTypeCode & "_Policy_id = " & CStr(v_lPolicyID)

            ' Call the SQL
            lReturn = v_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Update Pol No", bStoredProcedure:=False)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on SQLAction", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="SavePolNoInDB")
                Return result
            End If



            If (Not String.IsNullOrEmpty(v_lInsuranceFileCnt)) And (Not Informations.IsNothing(v_lInsuranceFileCnt)) Then
                ' Save in the Back Office Area
                sSQL = "UPDATE Insurance_File "
                sSQL = sSQL & "SET Insurance_Ref = '" & v_sPolNo & "' "
                sSQL = sSQL & "WHERE Insurance_File_Cnt = " & v_lInsuranceFileCnt

                ' Call the SQL
                lReturn = v_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Update Pol No", bStoredProcedure:=False)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on SQLAction", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="SavePolNoInDB")
                    Return result
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Run-time Error", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="SavePolNoInDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function
End Module
