Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Developer Guide no.129
Imports SharedFiles
Friend NotInheritable Class ExportCreditControl
    '************************************************************************
    ' Class/Module: ExportCreditControl
    '
    ' Description : Prints credit control letters and reports for all Credit
    '               Control Items for a branch
    '
    ' Created: PW130103
    '************************************************************************


    ' ************************************************
    ' Added to replace global variables 27/11/2003
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    Private m_iBatchID As Integer
    Private m_nTotalRecords As Integer = 0
    Protected Const kBatchStatusComplete As String = "C"
    Protected Const kBatchStatusFailed As String = "F"
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "ExportCreditControl"

    ' Private variables
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_oBusiness As Business
    Private m_oDatabase As dPMDAO.Database

    ' Constants for the HeaderData array
    Private Const kbHDBranch As Byte = 9
    Private Const kbHDAsOfDate As Byte = 10
    Private Const kbHDSpoolDoc As Byte = 11
    Private Const kbHDArchiveDoc As Byte = 12

    ' Result Array columns for Credit Control Item
    Private Const kbCCItemID As Byte = 0
    Private Const kbCCReason As Byte = 1
    Private Const kbCCAccountID As Byte = 2
    Private Const kbCCDocumentID As Byte = 3
    Private Const kbCCDocumentDate As Byte = 4
    Private Const kbCCInsuranceFileCnt As Byte = 5
    Private Const kbCCPFPremFinanceCnt As Byte = 6
    Private Const kbCCPFPremFinanceVersion As Byte = 7
    Private Const kbCCAmount As Byte = 8
    Private Const kbCCCanAutoCancel As Byte = 9
    Private Const kbCCWillAutoCancel As Byte = 10
    Private Const kbCCStepID As Byte = 11
    Private Const kbCCCreatedDate As Byte = 12
    Private Const kbCCDueDate As Byte = 13
    Private Const kbCCLetterSent As Byte = 14
    Private Const kbCCRecurrenceCount As Byte = 15
    Private Const kbCCRecurringDays As Byte = 16
    Private Const kbCCRecurringLetters As Byte = 17
    Private Const kbCCAutoCancelPolicy As Byte = 18
    Private Const kbCCCheckAutoCancel As Byte = 19
    Private Const kbCCJumpToNextStep As Byte = 20
    Private Const kbCCPMUserGroupID As Byte = 21
    Private Const kbCCPMWrkTaskID As Byte = 22
    Private Const kbCCNumberOfDays As Byte = 23
    Private Const kbCCBrokerDays As Byte = 24
    Private Const kbCCPolicyToleranceAmount As Byte = 25
    Private Const kbCCAccountToleranceAmount As Byte = 26
    Private Const kbCCBusinessType As Byte = 27
    Private Const kbCCAccountStatus As Byte = 28
    Private Const kbCCPolicyType As Byte = 29
    Private Const kbCCCustomerName As Byte = 30
    Private Const kbCCNextStepID As Byte = 31
    Private Const kbCCPolicyRiskCount As Byte = 32
    Private Const kbCCpmuser_group_id As Byte = 33
    Private Const kbCCpmuser_id As Byte = 34
    Private Const kbCCclaim_id As Byte = 35
    Private Const kbCCclaim_debt_id As Byte = 36
    Private Const kbCCclaim_debt_version As Byte = 37
    Private Const kbCCpartial_amount As Byte = 38
    Private Const kbCCis_deleted As Byte = 39
    Private Const kbCCpfinstalments_id As Byte = 40
    Private Const kbCCAction_type_id As Byte = 41
    Private Const kbCCSecond_pmwrk_task_id As Byte = 42
    Private Const kbCCSecond_pmuser_group_id As Byte = 43
    Private Const kbCCSecond_action_type_id As Byte = 44
    Private Const kbCCSecond_letter_id As Byte = 45
    Private Const kbCCSecond_oip_letter_id As Byte = 46
    Private Const kbCCPercentage_step_one As Byte = 47
    Private Const kbCCPercentage_step_two As Byte = 48
    Private Const kbCCOIPDocumentTemplateID As Byte = 49
    Private Const kbCCItemUpdated As Byte = 50
    Private Const kbCCInsuranceFolderCnt As Byte = 51
    Private Const kbCCDocumentTemplateCode As Integer = 52
    Private Const kbCCSecondDocumentTemplateCode As Integer = 53
    Private Const kbCCDocumentTemplateId As Integer = 54
    Private Const kbCCClaimDebtorId As Integer = 55
    Private Const kbCCStepPMWrkTaskGroupId As Integer = 56
    Private Const kbCCStepDescription As Integer = 57
    Private Const kbCCStopAccount As Integer = 58

    Private Const kbCCAutoLapseRenewal As Integer = 59
    Private Const kbCCIsBalanceAmount As Integer = 60

    ' Component Object variables
    Private m_oCreditControl As bACTCreditControl.Business
    Private m_oCreditControlItem As bACTCreditControlItem.Business
    Private m_oAccount As bACTAccount.Form

    Private m_oRenewal As bSIRRenewal.Business

    Private m_dAsOfDate As Date
    Private m_bSpoolDoc As Boolean
    Private m_bArchiveDoc As Boolean
    Private m_sUnderwritingOrAgency As String = ""

    ' Stored procedures
    'developer guide no. 39
    Private Const ksSPExportCreditControlSQL As String = "spu_ACT_Spoke_ExportCreditControl"
    Private Const ksSPExportCreditControlName As String = "GetExportCreditControl"
    Private Const ksSPExportCreditControlStored As Boolean = True

    'developer guide no. 39
    Private Const ksSPLapsedReasonFromCodeSQL As String = "spu_Lapsed_Reason_Sel_From_Code"
    Private Const ksSPLapsedReasonFromCodeName As String = "LapsedReasonFromCode"
    Private Const ksSPLapsedReasonFromCodeStore As Boolean = True

    'developer guide no. 39
    Private Const ksSPRenewalStatusFromRenInsFileCntSQL As String = "spu_Get_Renewal_Status_from_RenInsFileCnt"
    Private Const ksSPRenewalStatusFromRenInsFileCntName As String = "RenewalStatusFromRenInsFileCnt"
    Private Const ksSPRenewalStatusFromRenInsFileCntStore As Boolean = True

    'developer guide no. 39
    Private Const ksSPACTGetPartyCntFromAccountIdSQL As String = "spu_ACT_Get_PartyCnt_From_AccountID"
    Private Const ksSPACTGetPartyCntFromAccountIdName As String = "PartyCntFromAccountID"
    Private Const ksSPACTGetPartyCntFromAccountIdStore As Boolean = True

    ' PW161203 - CQ3554 - SP to get the Other Interested Parties
    Private Const ksSPGetOtherInterestedPartiesName As String = "GetOtherInterestedParties"
    'developer guide no. 39
    Private Const ksSPGetOtherInterestedPartiesSQL As String = "spu_ACT_Get_Other_Interested_Parties"
    Private Const ksSPGetOtherInterestedPartiesStored As Boolean = True


    Friend WriteOnly Property Business() As Business
        Set(ByVal Value As Business)

            m_oBusiness = Value

        End Set
    End Property
    Friend WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property


    '*************************************************************************
    ' Name: OIPsExist (private)
    '
    ' Description: This function determines if Other Interested Parties exist
    '              for an insurance file.
    '
    ' History: PW161203 - created (CQ3554)
    '*************************************************************************
    'UPGRADE_NOTE: (7001) The following declaration (OIPsExist) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function OIPsExist(ByVal lInsuranceFileCnt As Integer) As Boolean
    '
    'Dim vOIPArray(,) As Object
    '
    'Try 
    '
    ' Clear the Database Parameters Collection
    'm_oDatabase.Parameters.Clear()
    '
    ' Add the Insurance File Cnt parameter
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(lInsuranceFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Exit Function
    'End If
    '
    ' Execute SQL Statement to get OIPs
    'm_lReturn = m_oDatabase.SQLSelect(sSQL:=ksSPGetOtherInterestedPartiesSQL, sSQLName:=ksSPGetOtherInterestedPartiesName, bStoredProcedure:=ksSPGetOtherInterestedPartiesStored, vResultArray:=vOIPArray)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Exit Function
    'End If
    '
    '
    'Return Information.IsArray(vOIPArray)
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Log Error.
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="OIPsExist", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'End Try
    'End Function

    Friend Function PassThroughLogin(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef sCallingAppName As String, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: PassThroughLogin
        ' PURPOSE: Pass through the module level login information to the Class.
        ' This is for COM+. Normally a business class will not require this but the Spoke
        ' design means that Classes are instantiated by the Business class and can
        ' no longer rely on global variables.
        ' AUTHOR: Danny Davis
        ' DATE: 24 September 2003, 11:55 AM
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iSourceID = iSourceID
            m_iLanguageID = iLanguageID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="PassThroughLogin", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally



        End Try
        Return result
    End Function


    '*************************************************************************
    ' Name: GetCreditControlItems
    '
    ' Description: This function loops through all Credit Control items
    '              for the passed Branch and processes each, and produces
    '              Credit Control reports at the end. Unlike other Export
    '              classes, this one does not return any records, so a local
    '              variant array is declared to store the returned records
    '              from the initial select stored procedure.
    '
    ' Created: PW130103
    '*************************************************************************

    Private Function GetCreditControlItems(ByVal v_sBranchCode As String) As Integer

        Dim result As Integer = 0
        Const k_sFunctionName As String = "GetCreditControlItems"

        Dim lClientItemsArray() As Object, lOIPItemsArray() As Object, lAutoCancelItemsArray() As Object, lDeleteItemsArray() As Object
        Dim lClientItems As Integer ' RAW 11/10/2004 : CQ4811 : changed from integer to long 
        Dim lOIPItems As Integer ' RAW 11/10/2004 : CQ4811 : changed from integer to long 
        Dim lAutoCancelItems As Integer ' RAW 11/10/2004 : CQ4811 : changed from integer to long 
        Dim lDeleteItems As Integer ' RAW 11/10/2004 : CQ4811 : changed from integer to long 
        Dim bInstalment As Boolean
        Dim vResultArray, vValue As Object
        Dim bAdvancedCCItemsForInstalments As Boolean
        Dim lStopAccountArray() As Object
        Dim lStopAccountItems As Integer

        Dim lLapseRenewalArray() As Object
        Dim lLapseRenewalItems, lLapsedReasonID, lLiveInsuranceFileCnt, lRenewalStatusID, lPartyCnt As Integer
        Dim bDeletePermanent As Boolean

        ' HG110204 CQ4231 Converted integer to an long to prevent overflow

        '**************
        ' MEvans : 14-10-2004 : CQ4656
        Dim bProcessItem As Boolean
        Dim sLastClaimDebtNo, sClaimDebtNo As String
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResult1, vResult2, vResult3(,) As Object
        '**************

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add branch_code as an input param
            If m_oDatabase.Parameters.Add(sName:="branch_code", vValue:=v_sBranchCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Execute SQL Statement to get Credit Control Items
            'spu_ACT_Spoke_ExportCreditControl
            If m_oDatabase.SQLSelect(sSQL:=ksSPExportCreditControlSQL, sSQLName:=ksSPExportCreditControlName, bStoredProcedure:=ksSPExportCreditControlStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Process only if something returned
            If Information.IsArray(vResultArray) Then

                ' Create an instance of the Credit Control object
                m_oCreditControl = New bACTCreditControl.Business
                If m_oCreditControl.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                ' Create an instance of the Credit Control Item object
                m_oCreditControlItem = New bACTCreditControlItem.Business
                If m_oCreditControlItem.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                ' Create an instance of the Account object
                m_oAccount = New bACTAccount.Form
                If m_oAccount.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                If bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTGenerateAdvanceCreditControlForInstalments, v_vBranch:=1, r_vUnderwriting:=vValue) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                'Create an instance of bSIRrenewal
                m_oRenewal = New bSIRRenewal.Business
                If m_oRenewal.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                bAdvancedCCItemsForInstalments = (gPMFunctions.ToSafeInteger(vValue, 0) = 1)

                m_lReturn = CType(bPMFunc.getUnderwritingOrAgency(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, r_vUnderwriting:=m_sUnderwritingOrAgency), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="getUnderwritingOrAgency call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCreditControlItems")
                    Return result
                End If

                ' Loop through all Credit Control Items found

                For i As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                    ' Process the debt

                    bInstalment = (CStr(vResultArray(kbCCBusinessType, i)).Trim() = "INS") Or (CStr(vResultArray(kbCCBusinessType, i)).Trim() = "INSC") Or (CStr(vResultArray(kbCCBusinessType, i)).Trim() = "INSH")


                    If ProcessCreditControlItem(v_bInstalment:=bInstalment, r_vItemArray:=vResultArray, i:=i, r_vClientItemsArray:=lClientItemsArray, r_lClientItems:=lClientItems, r_vOIPItemsArray:=lOIPItemsArray, r_lOIPItems:=lOIPItems, r_vAutoCancelItemsArray:=lAutoCancelItemsArray, r_lAutoCancelItems:=lAutoCancelItems, r_vDeleteItemsArray:=lDeleteItemsArray, r_lDeleteItems:=lDeleteItems, r_vStopAccountArray:=lStopAccountArray, r_lStopAccountItems:=lStopAccountItems, r_vLapseRenewalArray:=lLapseRenewalArray, r_lLapseRenewalItems:=lLapseRenewalItems, bAdvancedCCItemsForInstalments:=bAdvancedCCItemsForInstalments) <> gPMConstants.PMEReturnCode.PMTrue Then

                        ' RAW 24/02/2004 : CQ4106 : added
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="ProcessCreditControlItem call failed for credit control item " & CStr(vResultArray(0, i)), vApp:=ACApp, vClass:=ACClass, vMethod:="GetCreditControlItems")
                        'Return result

                    End If

                Next

                ' Produce client letters, if necessary
                If lClientItems > 0 Then

                    If m_oCreditControl.ProduceClientLetters(v_vCreditControlItems:=lClientItemsArray, v_bSpoolDocuments:=m_bSpoolDoc, v_bArchiveDocuments:=m_bArchiveDoc) <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' RAW 24/02/2004 : CQ4106 : added
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oCreditControl.ProduceClientLetters call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCreditControlItems")

                        Return result
                    End If
                End If

                ' Produce other interested party letters, if necessary
                If lOIPItems > 0 Then


                    If m_oCreditControl.ProduceOIPLetters(v_vCreditControlItems:=lOIPItemsArray, v_bSpoolDocuments:=m_bSpoolDoc, v_bArchiveDocuments:=m_bArchiveDoc) <> gPMConstants.PMEReturnCode.PMTrue Then

                        ' RAW 24/02/2004 : CQ4106 : added
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oCreditControl.ProduceOIPLetters call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCreditControlItems")

                        Return result
                    End If
                End If

                ' Produce auto-cancellation report, if necessary
                If lAutoCancelItems > 0 Then

                    If m_oCreditControl.AutoCancelReport(v_vCreditControlItems:=lAutoCancelItemsArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' RAW 24/02/2004 : CQ4106 : added
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oCreditControl.AutoCancelReport call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCreditControlItems")
                        Return result
                    End If
                End If

                ' Stop Accounts
                If lStopAccountItems > 0 Then
                    For i As Integer = 0 To lStopAccountItems - 1
                        With m_oDatabase
                            .Parameters.Clear()
                            .Parameters.Add("account_id", CStr(lStopAccountArray(i)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                            .Parameters.Add("accountstatus_id", CStr(gACTLibrary.ACTAccountStatusStopped), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                            lReturn = .SQLAction("spu_ACT_Update_Account_AccountStatus", "Update Account Status", True)

                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="spu_ACT_Update_Account_AccountStatus failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCreditControlItems")
                            End If
                        End With
                    Next i
                End If

                ' Delete items in the delete array
                For i As Integer = 0 To lDeleteItems - 1
                    For cnt As Integer = 0 To lLapseRenewalItems - 1
                        bDeletePermanent = False
                        If Information.IsArray(lDeleteItemsArray) Then

                            If lDeleteItemsArray(i) = CDbl(vResultArray(kbCCItemID, lLapseRenewalArray(cnt))) Then
                                bDeletePermanent = True
                                'It will be deleted. don't need to update

                                vResultArray(kbCCItemUpdated, lLapseRenewalArray(cnt)) = 0
                                Exit For
                            End If
                        End If
                    Next cnt


                    If m_oCreditControlItem.DirectDelete(v_lCreditControlItemID:=Conversion.Val(CStr(lDeleteItemsArray(i))), v_bDeletePermanent:=bDeletePermanent) <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' RAW 24/02/2004 : CQ4106 : added
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oCreditControlItem.DirectDelete call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCreditControlItems")
                        Return result
                    End If
                Next i

                If lLapseRenewalItems > 0 Then
                    'Lapsed Reason ID for Code 'CCNTRL'
                    m_oDatabase.Parameters.Clear()
                    lReturn = m_oDatabase.SQLSelect(sSQL:=ksSPLapsedReasonFromCodeSQL, sSQLName:=ksSPLapsedReasonFromCodeName, bStoredProcedure:=ksSPLapsedReasonFromCodeStore, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResult1)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to get Lapsed Reason ID")
                    End If
                    ' Get Lapsed Reason ID
                    If Information.IsArray(vResult1) Then

                        lLapsedReasonID = CInt(vResult1.GetValue(0, 0)) 'm_oDatabase.Parameters.Item("lapsed_reason_id").Value
                    End If
                End If

                ' Auto Lapse the Policies
                For i As Integer = 0 To lLapseRenewalItems - 1
                    m_oDatabase.Parameters.Clear()

                    m_oDatabase.Parameters.Add("ren_ins_file_cnt", CStr(vResultArray.GetValue(kbCCInsuranceFileCnt, lLapseRenewalArray(i))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                    lReturn = m_oDatabase.SQLSelect(sSQL:=ksSPRenewalStatusFromRenInsFileCntSQL, sSQLName:=ksSPRenewalStatusFromRenInsFileCntName, bStoredProcedure:=ksSPRenewalStatusFromRenInsFileCntStore, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResult2)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to get Renewal Status")
                    End If
                    ' Get InsuranceFileCnt & RenewalStatusID
                    If Information.IsArray(vResult2) Then

                        lLiveInsuranceFileCnt = CInt(vResult2(0, 0))

                        lRenewalStatusID = CInt(vResult2(1, 0))
                    End If


                    'Party Cnt for vResultArray(kbCCAccountID, lLapseRenewalArray(i))
                    m_oDatabase.Parameters.Clear()

                    m_oDatabase.Parameters.Add("accountid", CStr(vResultArray(kbCCAccountID, lLapseRenewalArray(i))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                    lReturn = m_oDatabase.SQLSelect(sSQL:=ksSPACTGetPartyCntFromAccountIdSQL, sSQLName:=ksSPACTGetPartyCntFromAccountIdName, bStoredProcedure:=ksSPACTGetPartyCntFromAccountIdStore, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResult3)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to get Party Cnt")
                    End If
                    ' Get Party CNT
                    If Information.IsArray(vResult3) Then

                        lPartyCnt = CInt(vResult3(0, 0))
                    End If


                    If m_oRenewal.LapseRenewal(v_lRenewalCnt:=gPMFunctions.ToSafeLong(vResultArray(kbCCInsuranceFileCnt, lLapseRenewalArray(i))), v_lLivePolicyCnt:=lLiveInsuranceFileCnt, v_lStatusId:=lRenewalStatusID, v_lReasonID:=lLapsedReasonID, v_sReasonDesc:="Lapsed due to Credit Control Processing", v_lPartyCnt:=lPartyCnt, v_lInsFolderCnt:=gPMFunctions.ToSafeLong(vResultArray(kbCCInsuranceFolderCnt, lLapseRenewalArray(i)))) <> gPMConstants.PMEReturnCode.PMTrue Then

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oRenewal.LapseRenewal call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCreditControlItems")
                        Return result
                    End If
                Next

                'Now let update the database if updated the array
                ' Loop through all Credit Control Items found

                For i As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                    If gPMFunctions.ToSafeDouble(vResultArray(kbCCItemUpdated, i)) = 1 Then



                        If m_oCreditControlItem.DirectEdit(v_vCreditControlItemID:=Conversion.Val(CStr(vResultArray(kbCCItemID, i))), v_vCreditControlReason:=IIf(CStr(vResultArray(kbCCReason, i)) <> "", vResultArray(kbCCReason, i), DBNull.Value), v_vAccountID:=IIf(CStr(vResultArray(kbCCAccountID, i)) <> "", vResultArray(kbCCAccountID, i), DBNull.Value), v_vDocumentID:=IIf(CStr(vResultArray(kbCCDocumentID, i)) <> "", vResultArray(kbCCDocumentID, i), DBNull.Value), v_vDocumentDate:=IIf(CStr(vResultArray(kbCCDocumentDate, i)) <> "", vResultArray(kbCCDocumentDate, i), DBNull.Value), v_vInsuranceFileCnt:=IIf(CStr(vResultArray(kbCCInsuranceFileCnt, i)) <> "", vResultArray(kbCCInsuranceFileCnt, i), DBNull.Value), v_vPFPremFinanceCnt:=IIf(CStr(vResultArray(kbCCPFPremFinanceCnt, i)) <> "", vResultArray(kbCCPFPremFinanceCnt, i), DBNull.Value), v_vPFPremFinanceVersion:=IIf(CStr(vResultArray(kbCCPFPremFinanceVersion, i)) <> "", vResultArray(kbCCPFPremFinanceVersion, i), DBNull.Value), v_vAmount:=IIf(CStr(vResultArray(kbCCAmount, i)) <> "", vResultArray(kbCCAmount, i), DBNull.Value), v_vCanAutoCancel:=IIf(CStr(vResultArray(kbCCCanAutoCancel, i)) <> "", vResultArray(kbCCCanAutoCancel, i), DBNull.Value), v_vWillAutoCancel:=IIf(CStr(vResultArray(kbCCWillAutoCancel, i)) <> "", vResultArray(kbCCWillAutoCancel, i), DBNull.Value), v_vCreditControlStepID:=IIf(CStr(vResultArray(kbCCStepID, i)) <> "", vResultArray(kbCCStepID, i), DBNull.Value), v_vCreatedDate:=IIf(CStr(vResultArray(kbCCCreatedDate, i)) <> "", vResultArray(kbCCCreatedDate, i), DBNull.Value), v_vDueDate:=IIf(CStr(vResultArray(kbCCDueDate, i)) <> "", vResultArray(kbCCDueDate, i), DBNull.Value), v_vLetterSent:=IIf(CStr(vResultArray(kbCCLetterSent, i)) <> "", vResultArray(kbCCLetterSent, i), DBNull.Value), v_vRecurrenceCount:=IIf(CStr(vResultArray(kbCCRecurrenceCount, i)) <> "", vResultArray(kbCCRecurrenceCount, i), DBNull.Value), v_vPMUserGroupId:=IIf(CStr(vResultArray(kbCCpmuser_group_id, i)) <> "", vResultArray(kbCCpmuser_group_id, i), DBNull.Value), v_vPMUserId:=IIf(CStr(vResultArray(kbCCpmuser_id, i)) <> "", vResultArray(kbCCpmuser_id, i), DBNull.Value), v_vClaimId:=IIf(CStr(vResultArray(kbCCclaim_id, i)) <> "", vResultArray(kbCCclaim_id, i), DBNull.Value), v_vClaimDebtId:=IIf(CStr(vResultArray(kbCCclaim_debt_id, i)) <> "", vResultArray(kbCCclaim_debt_id, i), DBNull.Value), v_vClaimDebtVersion:=IIf(CStr(vResultArray(kbCCclaim_debt_version, i)) <> "", vResultArray(kbCCclaim_debt_version, i), DBNull.Value), v_vPartialAmount:=IIf(CStr(vResultArray(kbCCpartial_amount, i)) <> "", vResultArray(kbCCpartial_amount, i), DBNull.Value), v_vIsDeleted:=IIf(CStr(vResultArray(kbCCis_deleted, i)) <> "", vResultArray(kbCCis_deleted, i), DBNull.Value), v_vPFInstalmentsId:=IIf(CStr(vResultArray(kbCCpfinstalments_id, i)) <> "", vResultArray(kbCCpfinstalments_id, i), DBNull.Value)) <> gPMConstants.PMEReturnCode.PMTrue Then

                            'DD 03/07/2003: We will log the update failure but continue to loop

                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update the Credit Control Item ID=" & CStr(vResultArray(kbCCItemID, i)), vApp:=ACApp, vClass:=ACClass, vMethod:="GetCreditControlItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        End If
                    End If
                Next

                ' Kill the instance of the Credit Control object

                m_oCreditControl.Dispose()
                m_oCreditControl = Nothing

                ' Kill the instance of the Credit Control Item object

                m_oCreditControlItem.Dispose()
                m_oCreditControlItem = Nothing

                ' Kill the instance of the Account object

                m_oAccount.Dispose()
                m_oAccount = Nothing

                result = gPMConstants.PMEReturnCode.PMTrue
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCreditControlItems", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


        Finally
            ' destroy object references
            'Set m_oEventTask = Nothing
            'Set m_oDocManagerWrapper = Nothing
            'Set m_oFindDocTemplate = Nothing
            m_oCreditControl = Nothing
            m_oCreditControlItem = Nothing
            m_oAccount = Nothing
        End Try



        Return result

    End Function

    '*********************************************************************
    ' Name: Start
    '
    ' Description: Start process for use case
    '
    ' Created: PW130103
    '*********************************************************************
    Friend Function Start(ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByRef r_vHeaderData() As Object, ByRef r_vDetailData As Object, ByVal bCreateBatch As Boolean) As Integer

        Dim result As Integer = 0
        Dim sBranchCode As String = ""
        Dim bDBTransStarted As Boolean

        Const k_Header_Values As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Return codes
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_FAILED

            'We need valid database and business objects
            If (m_oBusiness Is Nothing) Or (m_oDatabase Is Nothing) Then
                ' RAW 24/02/2004 : CQ4106 : added
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Business and Database object are not set")
            End If

            'OK do the Export processing...

            ' Assign the values

            sBranchCode = CStr(r_vHeaderData(k_Header_Values)(kbHDBranch))

            If r_vHeaderData(k_Header_Values).GetUpperBound(0) = kbHDArchiveDoc Then
                'We have the 12th element set the module level variable

                m_dAsOfDate = CDate(r_vHeaderData(k_Header_Values)(kbHDAsOfDate))

                m_bSpoolDoc = CBool(r_vHeaderData(k_Header_Values)(kbHDSpoolDoc))

                m_bArchiveDoc = CBool(r_vHeaderData(k_Header_Values)(kbHDArchiveDoc))
            Else
                'set it to default Value
                m_dAsOfDate = CDate("1/1/1900")
                m_bSpoolDoc = False
                m_bArchiveDoc = False
            End If

            If bCreateBatch Then
                CreateBatch()
            End If

            'Begin a transaction
            If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Failed to begin transaction")
            End If

            bDBTransStarted = True

            ' Get the credit control items from the database based on the
            ' passed criteria
            m_lReturn = CType(GetCreditControlItems(v_sBranchCode:=sBranchCode), gPMConstants.PMEReturnCode)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue

                    result = gPMConstants.PMEReturnCode.PMTrue
                    'commit transaction
                    If m_oDatabase.SQLCommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Failed to commit transaction")
                    End If
                    If bCreateBatch Then
                        UpdateBatchTask(kBatchStatusComplete, m_iBatchID, m_nTotalRecords, 0)
                    End If
                    bDBTransStarted = False

                Case gPMConstants.PMEReturnCode.PMNotFound

                    result = gPMConstants.PMEReturnCode.PMNotFound
                    bDBTransStarted = False ' set this before doing rollback to avoid it being called again by error handler if it fails
                    'rollback transaction
                    If m_oDatabase.SQLRollbackTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Failed to rollback transaction")
                    End If
                    If bCreateBatch Then
                        UpdateBatchTask(kBatchStatusComplete, m_iBatchID, 0, 0)
                    End If

                Case Else

                    result = gPMConstants.PMEReturnCode.PMFalse

                    bDBTransStarted = False ' set this before doing rollback to avoid it being called again by error handler if it fails
                    'rollback transaction
                    If m_oDatabase.SQLRollbackTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Failed to rollback transaction")
                    End If
                    If bCreateBatch Then
                        UpdateBatchTask(kBatchStatusFailed, m_iBatchID, 0, 0)
                    End If
                    ' RAW 24/02/2004 : CQ4106 : added
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Call to GetCreditControlItems failed")
            End Select

            'Return codes
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_COMPLETE
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_COMPLETE

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            ' RAW 24/02/2004 : CQ4106 : added
            If bDBTransStarted Then
                m_oDatabase.SQLRollbackTrans()
            End If
            If bCreateBatch Then
                UpdateBatchTask(kBatchStatusFailed, m_iBatchID, 0, 0)
            End If
            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ProcessCreditControlItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 21-02-2005 : Credit Control RetroFit
    ' ***************************************************************** '
    'Developer Guide no.33
    Public Function ProcessCreditControlItem(ByVal v_bInstalment As Boolean, ByRef r_vItemArray(,) As Object, ByVal i As Integer, ByRef r_vClientItemsArray() As Object, ByRef r_lClientItems As Integer, ByRef r_vOIPItemsArray() As Object, ByRef r_lOIPItems As Integer, ByRef r_vAutoCancelItemsArray() As Object, ByRef r_lAutoCancelItems As Integer, ByRef r_vDeleteItemsArray() As Object, ByRef r_lDeleteItems As Integer, ByRef r_vStopAccountArray() As Object, ByRef r_lStopAccountItems As Integer, ByRef r_vLapseRenewalArray() As Object, ByRef r_lLapseRenewalItems As Integer, Optional ByVal bAdvancedCCItemsForInstalments As Boolean = False) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "ProcessCreditControlItem"

        Dim lCreditControlItemID As Integer
        Dim dDueDate As Date
        Dim iRecurringDays As Integer
        Dim bItemUpdated, bCanAutoCancel, bDirectPolicy As Boolean
        Dim cAmountOwing, cAccountBalance, cToleranceAmount As Decimal
        Dim bIsPaidInFull, bIsPartiallyPaid, bHasLiveMTA As Boolean
        Dim dCompareDate As Date
        Dim bProcessThis, bFirstProcessedItemForPolicy, bFirstProcessedItemForVersion As Boolean
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bJumpToNextStep As Boolean
        Dim lNextStepId, lPMWrkTaskInstanceCnt As Integer
        Dim bStopAccount, bPreviousAmountIsBalance As Boolean

        Static vPreviousItemArrayIndex As Object
        Static vPreviousProcessedItemArrayIndex As Object

        Dim bAutoLapseRenewal As Boolean

        Dim bGenerateDocument As Boolean

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store oft-used details
            bItemUpdated = False

            dDueDate = CDate(r_vItemArray(kbCCDueDate, i))

            iRecurringDays = Conversion.Val(CStr(r_vItemArray(kbCCRecurringDays, i)))

            lCreditControlItemID = CInt(Conversion.Val(CStr(r_vItemArray(kbCCItemID, i))))

            bDirectPolicy = True

            If ToSafeString(r_vItemArray(kbCCPolicyType, i)).Trim().ToUpper = "AGENCY" Then
                bDirectPolicy = False
            End If

            'Or (CStr(r_vItemArray(kbCCBusinessType, i)).Trim() = "INS") Or (CStr(r_vItemArray(kbCCBusinessType, i)).Trim() = "INSC") Or (CStr(r_vItemArray(kbCCBusinessType, i)).Trim() = "INSH")
            bStopAccount = gPMFunctions.ToSafeBoolean(r_vItemArray(kbCCStopAccount, i), False)

            bAutoLapseRenewal = gPMFunctions.ToSafeBoolean(r_vItemArray(kbCCAutoLapseRenewal, i), False)
            bGenerateDocument = True

            ' if not instalments
            If Not v_bInstalment Then

                'Allow DI to pick up partially paid items
                'Establish if the policy is partially paid

                If CStr(r_vItemArray(kbCCBusinessType, i)) <> "REN WTG UPDATE" Then


                    lReturn = m_oCreditControl.IsPolicyPaid(v_lInsuranceFileCnt:=Conversion.Val(CStr(r_vItemArray(kbCCInsuranceFileCnt, i))), r_bIsPaidInFull:=bIsPaidInFull, r_bIsPartiallyPaid:=bIsPartiallyPaid, r_cAmountOwing:=cAmountOwing, r_bHasLiveMTA:=bHasLiveMTA)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "m_oCreditControl.IsPolicyPaid call failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Else
                    bIsPaidInFull = False
                    bIsPartiallyPaid = False
                    cAmountOwing = gPMFunctions.ToSafeDouble(r_vItemArray(kbCCAmount, i))
                    bHasLiveMTA = False
                End If
            Else
                bIsPaidInFull = False
                bIsPartiallyPaid = False
                cAmountOwing = gPMFunctions.ToSafeDouble(r_vItemArray(kbCCAmount, i))
                bHasLiveMTA = False
            End If


            ' If the amount owing value has changed then update the Credit Control item
            ' so that the new values can be picked up by document production

            If Conversion.Val(CStr(r_vItemArray(kbCCAmount, i))) <> cAmountOwing Then
                lReturn = CType(UpdateCreditControlItem(r_vItemArray, i, cAmountOwing), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' do nothing - the failure is already logged by updatecreditcontrolitem
                    ' and a failure here should not result in the whole process failing
                End If
            End If

            ' Get the amount owing and tolerancy amount, if not on instalments
            If bDirectPolicy Then

                ' use the Policy tolerance figure


                cToleranceAmount = IIf(CStr(r_vItemArray(kbCCPolicyToleranceAmount, i)) = "", 0, r_vItemArray(kbCCPolicyToleranceAmount, i))

            Else

                ' determine what the outstanding balance is on the agents account
                ' in order to validate the tolerance check later on in the process

                lReturn = m_oAccount.GetAccountBalance(r_vdAccountBalance:=cAccountBalance, v_vAccountID:=r_vItemArray(kbCCAccountID, i), v_vAccountingDate:=Nothing, r_vResultArray:=Nothing, r_vlAccountCurrencyId:=Nothing, r_vdAccountDebt:=cAmountOwing, r_vAccountFloatBalance:=Nothing, r_vAccountOverDraftBalance:=Nothing)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "m_oAccount.GetAccountBalance call failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' use the Agents Account tolerance figure


                cToleranceAmount = IIf(CStr(r_vItemArray(kbCCAccountToleranceAmount, i)) = "", 0, r_vItemArray(kbCCAccountToleranceAmount, i))
            End If

            ' If there is no outstanding debt then delete the Credit Control Item
            ' and exit the process
            ' the amount owing will be either be the amount on the policy or the amount
            ' on the outstanding on the agents account....
            If cAmountOwing = 0 Then

                lReturn = m_oCreditControlItem.DirectDelete(v_lCreditControlItemID:=lCreditControlItemID)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "m_oCreditControlItem.DirectDelete call failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' quit the process via the exit label so any clean up still gets done.
                Return result
            End If



            '***********************************************************
            ' Determine if this credit control item should be processed....
            ' rules.
            ' if the item is now due - check due date against compare date
            ' if the account is still active
            ' if the tolerance limit has been reached
            ' or if instalments then process the credit control item

            ' determine what the compare date should be
            ' date is either passed in by the calling process
            ' or todays date is used as default


            If m_dAsOfDate > CDate("1/1/1900") Then
                dCompareDate = m_dAsOfDate
            Else
                dCompareDate = DateTime.Today
            End If

            ' Check if an action is due to be taken
            If dDueDate <= dCompareDate Then
                ' this item is now due
                bProcessThis = True
            End If

            ' if the item's duedate indicates it is to be processed
            If bProcessThis Then
                ' reset the process flag
                bProcessThis = False
                ' confirm the account is still active
                m_nTotalRecords += 1
                If CStr(r_vItemArray(kbCCAccountStatus, i)).Trim() = "ACTIVE" Then
                    ' if this is for instalements
                    If v_bInstalment Then
                        ' this is an instalment so ignore tolerance limit
                        ' and just process the item
                        bProcessThis = True
                    Else
                        ' if the tolerance limits have been reached or exceeded
                        ' then this item needs to be processed
                        If cAmountOwing >= cToleranceAmount Then
                            bProcessThis = True
                        End If
                    End If
                End If
            End If
            '***********************************************************

            ' if the credit control item is to be processed
            If bProcessThis Then

                ' See if the Account needs stopping
                If bStopAccount Then
                    '************************************
                    ' Add the Item to the Stop Account array
                    r_lStopAccountItems += 1
                    ReDim Preserve r_vStopAccountArray(r_lStopAccountItems - 1)


                    r_vStopAccountArray(r_lStopAccountItems - 1) = r_vItemArray(kbCCAccountID, i)
                    '************************************
                End If

                ' See if the Renewal needs lapsing
                If bAutoLapseRenewal Then
                    ' Add the Item to the Stop Account array
                    r_lLapseRenewalItems += 1
                    ReDim Preserve r_vLapseRenewalArray(r_lLapseRenewalItems - 1)

                    r_vLapseRenewalArray(r_lLapseRenewalItems - 1) = i

                    ' Add the Credit Control Item to the delete array
                    ' so that this item can be deleted later in the processing.
                    r_lDeleteItems += 1
                    ReDim Preserve r_vDeleteItemsArray(r_lDeleteItems - 1)

                    r_vDeleteItemsArray(r_lDeleteItems - 1) = lCreditControlItemID
                End If


                ' Is this the fist item to be processed for a policy or policy version
                bFirstProcessedItemForPolicy = True
                bFirstProcessedItemForVersion = True

                ' if this credit control item has an insurance folder cnt

                If Not (Convert.IsDBNull(r_vItemArray(kbCCInsuranceFolderCnt, i)) Or IsNothing(r_vItemArray(kbCCInsuranceFolderCnt, i))) Then

                    ' if the index value of the previously processed item has been stored

                    'If Not vPreviousProcessedItemArrayIndex.Equals(0) Then
                    If Not Object.Equals(vPreviousProcessedItemArrayIndex, Nothing) Then
                        ' if the insurance folder cnt of the currenct credit control item
                        ' matches the insurance_folder_cnt of the previously processed item


                        If r_vItemArray(kbCCInsuranceFolderCnt, i).Equals(r_vItemArray(kbCCInsuranceFolderCnt, vPreviousProcessedItemArrayIndex)) Then

                            ' then set the indicator that this is not the
                            ' first item processed against this policy (insurance_folder_cnt)
                            bFirstProcessedItemForPolicy = False
                        End If

                        ' if the insurance_file_cnt of the currenct item maches the
                        ' insurance_file_cnt of the previous processed item


                        If r_vItemArray(kbCCInsuranceFileCnt, i).Equals(r_vItemArray(kbCCInsuranceFileCnt, vPreviousProcessedItemArrayIndex)) Then

                            ' then set the indicator to show this is not the
                            ' first item processed against this policy_version (insurance_file_cnt)
                            bFirstProcessedItemForVersion = False
                        End If

                        If Not bFirstProcessedItemForVersion Then
                            'relevant to Adcanced CC process for Instalments only
                            'if it's next item in the instalments list and previous ones are partial amounts
                            If bAdvancedCCItemsForInstalments And v_bInstalment And gPMFunctions.ToSafeInteger(r_vItemArray(kbCCIsBalanceAmount, vPreviousProcessedItemArrayIndex)) = 1 Then
                                bPreviousAmountIsBalance = True
                            End If
                        End If
                    End If
                End If

                '***********************************************************
                ' POLICY LEVEL ACTIONS

                ' if this is not the first item processed against this policy
                If Not bFirstProcessedItemForPolicy Then

                    ' inherit the auto cancel values from the previous processed item for this policy
                    ' NB - any items set to auto cancel will have been sorted first
                    ' so that subsequent items will use these same values

                    bCanAutoCancel = CBool(r_vItemArray(kbCCWillAutoCancel, vPreviousProcessedItemArrayIndex))



                    r_vItemArray(kbCCAutoCancelPolicy, i) = r_vItemArray(kbCCAutoCancelPolicy, vPreviousProcessedItemArrayIndex)

                Else
                    ' This the first item for this policy to be processed
                    ' NB: the following actions will only be done once per policy

                    ' Check if the policy can be auto cancelled



                    If (CStr(r_vItemArray(kbCCAutoCancelPolicy, i)) = "1" Or CStr(r_vItemArray(kbCCCheckAutoCancel, i)) = "1") AndAlso CStr(r_vItemArray(kbCCCanAutoCancel, i)) = "1" AndAlso CStr(r_vItemArray(kbCCBusinessType, i)) <> "REN WTG UPDATE" Then

                        ' Check the Auto Cancel rules

                        lReturn = m_oCreditControl.AutoCancel(v_lCreditControlItemId:=lCreditControlItemID, v_bCheckRulesOnly:=True, r_bAutoCancelResult:=bCanAutoCancel, v_bArchiveDoc:=m_bArchiveDoc, v_bSpoolDoc:=m_bSpoolDoc)

                        ' if no outstanding transactions have been found
                        ' against the account associated with the current credit control item
                        If lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                            ' set the is_deleted flag against the credit control item
                            ' as there is no longer any debt to pursue. The policy has
                            ' been fully paid.

                            r_vItemArray(kbCCis_deleted, i) = 1

                            ' indicate the item has been updated and needs updating on the db

                            r_vItemArray(kbCCItemUpdated, i) = 1

                            ' leave the function via the finally label to ensure all clean up actions take place.
                            Return result

                        ElseIf lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "m_oCreditControl.AutoCancel call failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If

                    ' reset the bCanAutoCancel indicator if a live MTA is on the policy
                    ' NB: auto-cancel is not allowed if a live MTA is on the Policy
                    bCanAutoCancel = bCanAutoCancel And Not bHasLiveMTA

                    ' if the policy can be autocancelled

                    If bCanAutoCancel Then

                        ' Update the Item with will_auto_cancel = 1

                        r_vItemArray(kbCCWillAutoCancel, i) = "1"

                        ' indicate the item has been updated and needs updating on the db
                        bItemUpdated = True

                        '************************************
                        ' Add the Item to the auto cancel array
                        r_lAutoCancelItems += 1
                        ReDim Preserve r_vAutoCancelItemsArray(r_lAutoCancelItems - 1)

                        'r_vAutoCancelItemsArray(r_lAutoCancelItems) = lCreditControlItemID
                        r_vAutoCancelItemsArray(r_lAutoCancelItems - 1) = lCreditControlItemID
                        '************************************

                        ' else if a work manager task has been specified
                    ElseIf Conversion.Val(CStr(r_vItemArray(kbCCPMWrkTaskID, i))) > 0 And Conversion.Val(CStr(r_vItemArray(kbCCPMUserGroupID, i))) > 0 Then

                        ' create the task defined against the credit control step



                        lReturn = m_oCreditControl.CreateTask(v_lPMWrkTaskID:=Conversion.Val(CStr(r_vItemArray(kbCCPMWrkTaskID, i))), v_lPMWrkTaskGroupID:=IIf(CStr(r_vItemArray(kbCCStepPMWrkTaskGroupId, i)) = "", 1, r_vItemArray(kbCCStepPMWrkTaskGroupId, i)), v_sCustomer:=CStr(r_vItemArray(kbCCCustomerName, i)).Trim(), v_dtTaskDueDate:=DateTime.Now, v_lPMUserGroupID:=Conversion.Val(CStr(r_vItemArray(kbCCPMUserGroupID, i))), v_sDescription:=r_vItemArray(kbCCStepDescription, i), v_iTaskStatus:=0, v_iIsUrgent:=0, r_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt)

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "CreateTask Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If
                End If

                ' END OF POLICY LEVEL ACTIONS
                '***********************************************************


                'RFC , PM018813
                'Run_Auto_Cancel_Rules checked in Credit Control
                If r_vItemArray(kbCCAutoCancelPolicy, i) = 1 And bCanAutoCancel = False Then
                    bGenerateDocument = False
                End If

                If Val(r_vItemArray(kbCCPMWrkTaskID, i)) > 0 AndAlso
                    Val(r_vItemArray(kbCCPMUserGroupID, i)) > 0 Then

                    ' create the task defined against the credit control step
                    lReturn = m_oCreditControl.CreateTask(
                            v_lPMWrkTaskID:=Val(r_vItemArray(kbCCPMWrkTaskID, i)),
                            v_lPMWrkTaskGroupID:=IIf(r_vItemArray(kbCCStepPMWrkTaskGroupId, i) = "", 1,
                            r_vItemArray(kbCCStepPMWrkTaskGroupId, i)),
                            v_sCustomer:=Trim$(r_vItemArray(kbCCCustomerName, i)),
                            v_dtTaskDueDate:=Now,
                            v_lPMUserGroupID:=Val(r_vItemArray(kbCCPMUserGroupID, i)),
                            v_sDescription:=r_vItemArray(kbCCStepDescription, i),
                            v_iTaskStatus:=0,
                            v_iIsUrgent:=0,
                            r_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "CreateTask Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

                '***********************************************************
                ' POLICY VERSION LEVEL ACTIONS

                ' if this is the first item to be processed for this policy_version (insurance_file_cnt)
                ' or previous item was balance instalment
                If (bFirstProcessedItemForVersion Or bPreviousAmountIsBalance) And bGenerateDocument = True Then

                    ' the following actions are only done once per policy_version

                    ' Check if a client letter is due to be sent
                    ' NB: This process does not actually take into account whether a valid template id
                    ' has been specified on the credit control step,
                    ' the logic to do this is in the produceclientletters function called
                    ' later in the process



                    If (CStr(r_vItemArray(kbCCLetterSent, i)) = "1" And CStr(r_vItemArray(kbCCRecurringLetters, i)) = "1") Or Conversion.Val(CStr(r_vItemArray(kbCCLetterSent, i))) = 0 Then

                        r_lClientItems += 1
                        ReDim Preserve r_vClientItemsArray(r_lClientItems - 1)

                        'r_vClientItemsArray(r_lClientItems) = lCreditControlItemID
                        r_vClientItemsArray(r_lClientItems - 1) = lCreditControlItemID

                        ' Set letter sent

                        r_vItemArray(kbCCLetterSent, i) = "1"
                        bItemUpdated = True
                    End If

                End If

                ' END OF POLICY VERSION LEVEL ACTIONS
                '***********************************************************

                ' We are no longer processing just the first item for a policy

                ' store next step indicators

                bJumpToNextStep = CStr(r_vItemArray(kbCCJumpToNextStep, i)) = "1"

                lNextStepId = CInt(Conversion.Val(CStr(r_vItemArray(kbCCNextStepID, i))))

                ' If applicable, auto cancel the policy

                If bCanAutoCancel And CStr(r_vItemArray(kbCCAutoCancelPolicy, i)) = "1" Then

                    ' if this is not the first processed item for the policy
                    If Not bFirstProcessedItemForPolicy Then
                        ' do nothing - as the policy will have already been cancelled by an earlier CCI
                    Else
                        ' else this is the first credit control item for policy
                        ' and so the auto cancel needs to be done here

                        lReturn = m_oCreditControl.AutoCancel(v_lCreditControlItemID:=lCreditControlItemID, v_bCheckRulesOnly:=False, r_bAutoCancelResult:=bCanAutoCancel, v_bArchiveDoc:=m_bArchiveDoc, v_bSpoolDoc:=m_bSpoolDoc)

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue AndAlso lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                            gPMFunctions.RaiseError(kMethodName, "m_oCreditControl.AutoCancel call failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If

                    ' Add the Credit Control Item to the delete array
                    ' so that this item can be deleted later in the processing.
                    r_lDeleteItems += 1
                    ReDim Preserve r_vDeleteItemsArray(r_lDeleteItems - 1)

                    'r_vDeleteItemsArray(r_lDeleteItems) = lCreditControlItemID
                    r_vDeleteItemsArray(r_lDeleteItems - 1) = lCreditControlItemID

                    ' indicate the item has been updated and needs updating on the db
                    bItemUpdated = False

                    ' if this credit control item is based on instalments
                    ' and jump to next step is set and is valid
                    ' or ( not instalments and next step id is set)
                ElseIf (v_bInstalment And bJumpToNextStep And lNextStepId <> 0 Or (Not v_bInstalment And lNextStepId <> 0)) Then

                    ' update the step id to the next step id

                    r_vItemArray(kbCCStepID, i) = lNextStepId

                    ' set the due date for the next step
                    ' NB: due date should be NOW + days not due_date + days
                    ' for advanced cc for instalments, this uses the original due date
                    If bDirectPolicy Then

                        If CStr(r_vItemArray(kbCCBusinessType, i)) = "REN WTG UPDATE" Then


                            r_vItemArray(kbCCDueDate, i) = dDueDate.AddDays(Conversion.Val(CStr(r_vItemArray(kbCCNumberOfDays, i))))
                        ElseIf v_bInstalment Then

                            r_vItemArray(kbCCDueDate, i) = DateAdd("d", Val(r_vItemArray(kbCCNumberOfDays, i)), IIf(bAdvancedCCItemsForInstalments, dDueDate, DateTime.Today))
                        Else

                            r_vItemArray(kbCCDueDate, i) = DateAdd("d", Val(r_vItemArray(kbCCNumberOfDays, i)), dDueDate)
                        End If
                    Else

                        If CStr(r_vItemArray(kbCCBusinessType, i)) = "REN WTG UPDATE" Then


                            r_vItemArray(kbCCDueDate, i) = dDueDate.AddDays(Conversion.Val(CStr(r_vItemArray(kbCCBrokerDays, i))))
                        Else


                            r_vItemArray(kbCCDueDate, i) = (IIf(bAdvancedCCItemsForInstalments, dDueDate, DateTime.Today)).AddDays(Conversion.Val(CStr(r_vItemArray(kbCCBrokerDays, i))))
                        End If
                    End If

                    ' reset the Letter Sent Indicator to 0 when moving to next step

                    r_vItemArray(kbCCLetterSent, i) = "0"

                    ' indicate the item has been updated and needs updating on the db
                    bItemUpdated = True

                    ' this is the last step in the credit control package
                Else

                    ' if the number of recurring days has been set

                    If Conversion.Val(CStr(r_vItemArray(kbCCRecurringDays, i))) <> 0 Then

                        ' NB: due date should be NOW + recurring days not due_date + recurring days
                        ' for advanced cc for instalments, this uses the original due date


                        r_vItemArray(kbCCDueDate, i) = DateAdd("d", Val(r_vItemArray(kbCCRecurringDays, i)), dDueDate)


                        ' Increment the recurrence count field to show how many time
                        ' this recurring process has taken place


                        r_vItemArray(kbCCRecurrenceCount, i) = Conversion.Val(CStr(r_vItemArray(kbCCRecurrenceCount, i))) + 1

                        ' indicate the item has been updated and needs updating on the db
                        bItemUpdated = True
                    End If
                End If

            End If

            'Set the value of ItemUpdated in the array

            r_vItemArray(kbCCItemUpdated, i) = Math.Abs(CInt(bItemUpdated))

            ' store the current index for use when processing the next debt
            vPreviousItemArrayIndex = i

            If bProcessThis Then
                vPreviousProcessedItemArrayIndex = i
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: UpdateCreditControlItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 21-02-2005 : Credit Control RetroFit
    ' ***************************************************************** '
    Public Function UpdateCreditControlItem(ByRef r_vItemArray(,) As Object, ByVal i As Integer, ByVal v_cAmountOwing As Decimal) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateCreditControlItem"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = m_oCreditControlItem.DirectEdit(v_vCreditControlItemID:=Conversion.Val(CStr(r_vItemArray(kbCCItemID, i))), v_vCreditControlReason:=IIf(CStr(r_vItemArray(kbCCReason, i)) <> "", r_vItemArray(kbCCReason, i), DBNull.Value), v_vAccountID:=IIf(CStr(r_vItemArray(kbCCAccountID, i)) <> "", r_vItemArray(kbCCAccountID, i), DBNull.Value), v_vDocumentID:=IIf(CStr(r_vItemArray(kbCCDocumentID, i)) <> "", r_vItemArray(kbCCDocumentID, i), DBNull.Value), v_vDocumentDate:=IIf(CStr(r_vItemArray(kbCCDocumentDate, i)) <> "", r_vItemArray(kbCCDocumentDate, i), DBNull.Value), v_vInsuranceFileCnt:=IIf(CStr(r_vItemArray(kbCCInsuranceFileCnt, i)) <> "", r_vItemArray(kbCCInsuranceFileCnt, i), DBNull.Value), v_vPFPremFinanceCnt:=IIf(CStr(r_vItemArray(kbCCPFPremFinanceCnt, i)) <> "", r_vItemArray(kbCCPFPremFinanceCnt, i), DBNull.Value), v_vPFPremFinanceVersion:=IIf(CStr(r_vItemArray(kbCCPFPremFinanceVersion, i)) <> "", r_vItemArray(kbCCPFPremFinanceVersion, i), DBNull.Value), v_vAmount:=v_cAmountOwing, v_vCanAutoCancel:=IIf(CStr(r_vItemArray(kbCCCanAutoCancel, i)) <> "", r_vItemArray(kbCCCanAutoCancel, i), DBNull.Value), v_vWillAutoCancel:=IIf(CStr(r_vItemArray(kbCCWillAutoCancel, i)) <> "", r_vItemArray(kbCCWillAutoCancel, i), DBNull.Value), v_vCreditControlStepID:=IIf(CStr(r_vItemArray(kbCCStepID, i)) <> "", r_vItemArray(kbCCStepID, i), DBNull.Value), v_vCreatedDate:=IIf(CStr(r_vItemArray(kbCCCreatedDate, i)) <> "", r_vItemArray(kbCCCreatedDate, i), DBNull.Value), v_vDueDate:=IIf(CStr(r_vItemArray(kbCCDueDate, i)) <> "", r_vItemArray(kbCCDueDate, i), DBNull.Value), v_vLetterSent:=IIf(CStr(r_vItemArray(kbCCLetterSent, i)) <> "", r_vItemArray(kbCCLetterSent, i), DBNull.Value), v_vRecurrenceCount:=IIf(CStr(r_vItemArray(kbCCRecurrenceCount, i)) <> "", r_vItemArray(kbCCRecurrenceCount, i), DBNull.Value), v_vPMUserGroupId:=IIf(CStr(r_vItemArray(kbCCpmuser_group_id, i)) <> "", r_vItemArray(kbCCpmuser_group_id, i), DBNull.Value), v_vPMUserId:=IIf(CStr(r_vItemArray(kbCCpmuser_id, i)) <> "", r_vItemArray(kbCCpmuser_id, i), DBNull.Value), v_vClaimId:=IIf(CStr(r_vItemArray(kbCCclaim_id, i)) <> "", r_vItemArray(kbCCclaim_id, i), DBNull.Value), v_vClaimDebtId:=IIf(CStr(r_vItemArray(kbCCclaim_debt_id, i)) <> "", r_vItemArray(kbCCclaim_debt_id, i), DBNull.Value), v_vClaimDebtVersion:=IIf(CStr(r_vItemArray(kbCCclaim_debt_version, i)) <> "", r_vItemArray(kbCCclaim_debt_version, i), DBNull.Value), v_vPartialAmount:=IIf(CStr(r_vItemArray(kbCCpartial_amount, i)) <> "", r_vItemArray(kbCCpartial_amount, i), DBNull.Value), v_vIsDeleted:=IIf(CStr(r_vItemArray(kbCCis_deleted, i)) <> "", r_vItemArray(kbCCis_deleted, i), DBNull.Value), v_vPFInstalmentsId:=IIf(CStr(r_vItemArray(kbCCpfinstalments_id, i)) <> "", r_vItemArray(kbCCpfinstalments_id, i), DBNull.Value))

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'DD 03/07/2003: We will log the update failure but continue to loop

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update the Credit Control Item ID=" & CStr(r_vItemArray(kbCCItemID, i)), vApp:=ACApp, vClass:=ACClass, vMethod:="GetCreditControlItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    ''' <summary>
    '''  Create New Credit Control batch
    ''' </summary>
    Private Sub CreateBatch()
        Dim nReturn As PMEReturnCode = PMEReturnCode.PMTrue
        AddParameterLite(m_oDatabase, "batch_id", System.DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger, True)
        nReturn = m_oDatabase.SQLAction("spu_Create_CreditControl_Batch", "Create Credit Control Batch", True)
        If nReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to execute 'spu_Create_CreditControl_Batch'")
        End If
        ' Get batch id
        m_iBatchID = m_oDatabase.Parameters.Item("batch_id").Value
    End Sub
    ''' <summary>
    '''  Update Batch Task
    ''' </summary>
    ''' <param name="sBatchStatusCode"></param>
    ''' <param name="nBatchId"></param>
    ''' <param name="nTotal_Transactions"></param>
    ''' <param name="nReject_transactions"></param>
    Public Sub UpdateBatchTask(ByVal sBatchStatusCode As String, ByVal nBatchId As Integer, ByVal nTotal_Transactions As Integer, ByVal nReject_transactions As Integer)
        Dim nReturnValue As Integer
        Try
            AddParameterLite(m_oDatabase, "Batch_Id", nBatchId, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
            AddParameterLite(m_oDatabase, "FileName", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "BatchStatusCode", sBatchStatusCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "Total_Transactions", nTotal_Transactions, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "Reject_Transactions", nReject_transactions, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            nReturnValue = m_oDatabase.SQLSelect("spu_Update_BatchTask", "Update Batch in Batch", True)
            If nReturnValue <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_Update_BatchTask'")
            End If
        Catch ex As Exception
            Throw New Exception("Unable to update entry in Batch", ex)
        End Try
    End Sub
End Class
