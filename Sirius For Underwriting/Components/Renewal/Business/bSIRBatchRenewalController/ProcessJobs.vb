Imports System.Linq
Imports SharedFiles
Imports SSP.PureInsuranceRestAPIHandler
Imports SSP.PureInsuranceRestAPIHandler.BaseClasses

Public NotInheritable Class ProcessJobs
    Const ACClass = "ProcessJobs"

#Region " Private Declarations "
    Private m_lReturn As Long
    Protected m_oDatabase As dPMDAO.Database = Nothing
    Private sSAMURL As String
    Private sSAMUsername As String
    Private sClientId As String
    Private sTenantId As String
    Private sTokenUrl As String

    Public Enum enumMissingData
        None
        BranchCode
        'InsuranceFileKey
    End Enum

    Public Enum enumInvalidLookup
        None
        BranchCode
    End Enum

    Public Enum enumSTSBusinessError
        None
        InvalidInsuranceFileCnt
    End Enum

#End Region
    Public Sub New(sSAMURL As String, sSAMUsername As String, sClientId As String, sTenantId As String, sTokenUrl As String)
        Me.sSAMURL = sSAMURL
        Me.sSAMUsername = sSAMUsername
        Me.sClientId = sClientId
        Me.sTenantId = sTenantId
        Me.sTokenUrl = sTokenUrl

        DBConnect(m_oDatabase)
    End Sub

#Region " Main Methods "
    Public Sub ProcessJobs(Optional ByVal v_sJobCode As String = "",
                            Optional ByVal v_sUserName As String = "",
                            Optional ByVal v_sPassword As String = "",
                            Optional ByVal eMissingData As enumMissingData = enumMissingData.None,
                            Optional ByVal eInvalidLookup As enumInvalidLookup = enumInvalidLookup.None,
                            Optional ByVal eSTSBusinessError As enumSTSBusinessError = enumSTSBusinessError.None)


        'Const kMethod As String = "ProcessJobs"

        Dim nLookupError As Integer = 102
        Dim nBusinessError As Integer = 224

        Dim oReqRenSelection As New RunRenewalSelectionCommand
        Dim oReqRenInvite As New RunRenewalInvitationCommand
        Dim oReqRenAccept As New RunRenewalAcceptCommand

        Dim oReqRenSelectionSync As New RunRenewalSelectionSyncCommand
        Dim oReqRenInviteSync As New RunRenewalInvitationSyncCommand
        Dim oReqRenAcceptSync As New RunRenewalAcceptSyncCommand

        Dim oResRenSelectionSync As New RunRenewalSelectionSyncCommandResponse
        Dim oResRenInviteSync As New RunRenewalInvitationSyncCommandResponse
        Dim oResRenAcceptSync As New RunRenewalAcceptSyncCommandResponse

        Dim oPMLock As Object = Nothing
        Dim obSIRRenewalAcc As bSIRAutomaticRenewalsAccept.Business = Nothing
        Dim vInsuranceFilesArray(,) As Object = Nothing
        Dim lCnt As Long = 0
        Dim lInsuranceFileCnt As Long = 0
        Dim lBatchRenewalJobId As Long = 0
        Dim sBatchRenewalJobType As String = ""
        Dim bRunExtendedRule As Boolean
        Dim lRecordCount As Long = 0
        Dim sLockedBy As String = ""
        Dim sGUID As String
        Dim sSAMServer As String = ""
        Dim sSuccessMsg As String = ""
        Dim sCurrentDate As Date
        Dim lProcessedPolicyCnt As Long = 0
        Dim lPolicyCnt As Long = 0
        Dim lFailureCnt As Long = 0
        Const PMBRenewalStatusTypePolicyChanged As Long = 4

        Try

            sGUID = Guid.NewGuid.ToString

            m_lReturn = GetJobType(v_sJobCode:=v_sJobCode,
                                    r_sBatchRenewalJobType:=sBatchRenewalJobType,
                                    r_lBatchRenewalJobId:=lBatchRenewalJobId,
                                    r_sSamServer:=sSAMServer,
                                    r_bRunExtendedRule:=bRunExtendedRule)


            If sBatchRenewalJobType.Trim = "" Then
                OutputLine("No Job Found for " & v_sJobCode)
                Exit Sub
            End If

            'Get bPMLock
            oPMLock = New bPMLock.User
            m_lReturn = oPMLock.Initialise(
                                            sUsername:="",
                                            sPassword:="",
                                            iUserID:=1,
                                            iSourceID:=1,
                                            iLanguageID:=1,
                                            iCurrencyID:=26,
                                            iLogLevel:=PMELogLevel.PMLogError,
                                            sCallingAppName:=ACApp)

            'Check for any error
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Fails to GetRenewalSelectionPolicyList")
                Exit Sub
            End If
            Select Case sBatchRenewalJobType.ToUpper
                Case "SEL"
                    m_lReturn = GetRenewalSelectionPolicyList(v_sJobCode:=v_sJobCode,
                                                                r_vResultArray:=vInsuranceFilesArray)

                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Throw New Exception("Fails to GetRenewalSelectionPolicyList")
                        Exit Sub
                    End If

                    If Not IsArray(vInsuranceFilesArray) Then
                        OutputLine("No Policy Found for Renewal Selection for Job Code:" & v_sJobCode)
                        Exit Sub
                    End If

                    'Prepare for Renewal selection
                    'delete all in renewal_report table ready for new data
                    'Lock the Key
                    m_lReturn = oPMLock.LockKey(sKeyName:=sBatchRenewalJobType,
                                                    vKeyValue:=lBatchRenewalJobId.ToString,
                                                    iUserID:=1,
                                                    sCurrentlyLockedBy:=sLockedBy)

                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        OutputLine("This Job is Locked by User - " & v_sUserName)
                        Exit Sub
                    End If

                    m_lReturn = DelRenewalReport()

                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Throw New Exception("Fails to Delete Renewal Report")
                        Exit Sub
                    End If

                    'Prepare data for renewal selection
                    sCurrentDate = Convert.ToDateTime(Now).ToLongDateString
                    m_lReturn = DelRenewalStatusPolicies(PMBRenewalStatusTypePolicyChanged, sCurrentDate)

                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Throw New Exception("Fails to Delete Renewal Status Policies")
                        Exit Sub
                    End If
                    lPolicyCnt = UBound(vInsuranceFilesArray, 2) + 1

                    ApiClient._tokenModel = GetApiTokendetails()
                    For lCnt = LBound(vInsuranceFilesArray, 2) To UBound(vInsuranceFilesArray, 2)
                        lInsuranceFileCnt = ToSafeLong(vInsuranceFilesArray(0, lCnt), 0)
                        If lInsuranceFileCnt > 0 Then
                            If My.Settings.SyncRenewals = False Then
                                oReqRenSelection.BranchCode = "HEADOFF"
                                oReqRenSelection.InsuranceFileKey = lInsuranceFileCnt
                                oReqRenSelection.RecordsCount = UBound(vInsuranceFilesArray, 2)
                                oReqRenSelection.BatchRenewalJobKey = lBatchRenewalJobId
                                oReqRenSelection.GUID = sGUID
                                oReqRenSelection.LoginUserName = v_sUserName
                                ApiClient.Post($"/policies/runRenewalSelection", oReqRenSelection)
                            Else
                                oReqRenSelectionSync.BranchCode = "HEADOFF"
                                oReqRenSelectionSync.InsuranceFileKey = lInsuranceFileCnt
                                oReqRenSelectionSync.RecordsCount = UBound(vInsuranceFilesArray, 2) + 1
                                oReqRenSelectionSync.BatchRenewalJobKey = lBatchRenewalJobId
                                oReqRenSelectionSync.GUID = sGUID
                                oReqRenSelectionSync.LoginUserName = v_sUserName
                                oResRenSelectionSync = ApiClient.DeserializeJson(Of RunRenewalSelectionSyncCommandResponse)(CStr(ApiClient.Post($"/policies/runRenewalSelectionSync", oReqRenSelectionSync)))
                                If oResRenSelectionSync.IsProcessed = False Then
                                    lFailureCnt = lFailureCnt + 1
                                End If
                            End If

                            lProcessedPolicyCnt += 1
                        End If
                    Next

                Case "INV"

                    m_lReturn = GetRenewalInvitationPolicyList(v_sJobCode:=v_sJobCode,
                                                                r_vResultArray:=vInsuranceFilesArray)

                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Throw New Exception("Fails to GetRenewalInvitationPolicyList")
                        Exit Sub
                    End If

                    If Not IsArray(vInsuranceFilesArray) Then
                        OutputLine("No Policy Found for Renewal Invitation for Job Code:" & v_sJobCode)
                        Exit Sub
                    End If
                    'Lock the Key
                    m_lReturn = oPMLock.LockKey(sKeyName:=sBatchRenewalJobType,
                                                    vKeyValue:=lBatchRenewalJobId.ToString,
                                                    iUserID:=1,
                                                    sCurrentlyLockedBy:=sLockedBy)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        OutputLine("This Job is Locked by User - " & v_sUserName)
                        Exit Sub
                    End If
                    lPolicyCnt = UBound(vInsuranceFilesArray, 2) + 1

                    ApiClient._tokenModel = GetApiTokendetails()
                    For lCnt = LBound(vInsuranceFilesArray, 2) To UBound(vInsuranceFilesArray, 2)
                        lInsuranceFileCnt = ToSafeLong(vInsuranceFilesArray(0, lCnt), 0)
                        If lInsuranceFileCnt > 0 Then

                            If My.Settings.SyncRenewals = False Then
                                oReqRenInvite.BranchCode = "HEADOFF"
                                oReqRenInvite.InsuranceFileKey = lInsuranceFileCnt
                                oReqRenInvite.RecordsCount = UBound(vInsuranceFilesArray, 2)
                                oReqRenInvite.BatchRenewalJobKey = lBatchRenewalJobId
                                oReqRenInvite.GUID = sGUID
                                oReqRenInvite.LoginUserName = v_sUserName
                                ApiClient.Post($"/policies/runRenewalInvitation", oReqRenInvite)
                            Else

                                oReqRenInviteSync.BranchCode = "HEADOFF"
                                oReqRenInviteSync.InsuranceFileKey = lInsuranceFileCnt
                                oReqRenInviteSync.RecordsCount = UBound(vInsuranceFilesArray, 2)
                                oReqRenInviteSync.BatchRenewalJobKey = lBatchRenewalJobId
                                oReqRenInviteSync.GUID = sGUID
                                oReqRenInviteSync.LoginUserName = v_sUserName
                                oResRenInviteSync = ApiClient.DeserializeJson(Of RunRenewalInvitationSyncCommandResponse)(CStr(ApiClient.Post($"/policies/runRenewalInvitationSync", oReqRenInviteSync)))
                                If oResRenInviteSync.IsProcessed = False Then
                                    lFailureCnt = lFailureCnt + 1
                                End If

                            End If

                            lProcessedPolicyCnt += 1
                        End If
                    Next

                Case "ACC"

                    m_lReturn = GetRenewalAcceptancePolicyList(v_sJobCode:=v_sJobCode, r_vResultArray:=vInsuranceFilesArray)

                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Throw New Exception("Fails to GetRenewalAcceptancePolicyList")
                        Exit Sub
                    End If

                    If Not IsArray(vInsuranceFilesArray) Then
                        OutputLine("No Policy Found for Renewal Acceptance for Job Code:" & v_sJobCode)
                        Exit Sub
                    End If

                    obSIRRenewalAcc = New bSIRAutomaticRenewalsAccept.Business
                    m_lReturn = obSIRRenewalAcc.Initialise(
                                                    sUsername:="",
                                                    sPassword:="",
                                                    iUserID:=1,
                                                    iSourceID:=1,
                                                    iLanguageID:=1,
                                                    iCurrencyID:=26,
                                                    iLogLevel:=PMELogLevel.PMLogError,
                                                    sCallingAppName:=ACApp,
                                                    vDatabase:=m_oDatabase)

                    'Lock the Key
                    m_lReturn = oPMLock.LockKey(sKeyName:=sBatchRenewalJobType,
                                                    vKeyValue:=lBatchRenewalJobId.ToString,
                                                    iUserID:=1,
                                                    sCurrentlyLockedBy:=sLockedBy)

                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        OutputLine("This Job is Locked by User - " & v_sUserName)
                        Exit Sub
                    End If
                    lPolicyCnt = UBound(vInsuranceFilesArray, 2) + 1

                    ApiClient._tokenModel = GetApiTokendetails()
                    For lCnt = LBound(vInsuranceFilesArray, 2) To UBound(vInsuranceFilesArray, 2)
                        lInsuranceFileCnt = ToSafeLong(vInsuranceFilesArray(0, lCnt), 0)
                        If lInsuranceFileCnt > 0 AndAlso ((Not bRunExtendedRule) OrElse (bRunExtendedRule And
                            obSIRRenewalAcc.CheckServiceLevelForRenewalAcceptance(lInsuranceFileCnt:=lInsuranceFileCnt, lBatchRenewalJobID:=lBatchRenewalJobId))) Then
                            If My.Settings.SyncRenewals = False Then
                                oReqRenAccept.BranchCode = "HEADOFF"
                                oReqRenAccept.InsuranceFileKey = lInsuranceFileCnt
                                oReqRenAccept.RecordsCount = UBound(vInsuranceFilesArray, 2)
                                oReqRenAccept.BatchRenewalJobKey = lBatchRenewalJobId
                                oReqRenAccept.GUID = sGUID
                                oReqRenAccept.LoginUserName = v_sUserName
                                ApiClient.Post($"/policies/runRenewalAccept", oReqRenAccept)
                            Else
                                oReqRenAcceptSync.BranchCode = "HEADOFF"
                                oReqRenAcceptSync.InsuranceFileKey = lInsuranceFileCnt
                                oReqRenAcceptSync.RecordsCount = UBound(vInsuranceFilesArray, 2)
                                oReqRenAcceptSync.BatchRenewalJobKey = lBatchRenewalJobId
                                oReqRenAcceptSync.GUID = sGUID
                                oReqRenAcceptSync.LoginUserName = v_sUserName
                                oResRenAcceptSync = ApiClient.DeserializeJson(Of RunRenewalAcceptSyncCommandResponse)(CStr(ApiClient.Post($"/policies/runRenewalAcceptSync", oReqRenAcceptSync)))
                                If oResRenAcceptSync.IsProcessed = False Then
                                    lFailureCnt = lFailureCnt + 1
                                End If
                            End If

                            lProcessedPolicyCnt += 1
                        End If
                    Next

            End Select

            'UnLock the Key

            'Catch ex As AssertionException
            'Throw
        Catch ex As SoapException
            OutputLine("Failed to send all policies to SAM: " & ex.Message)
            'WSETest.HandleException(ex, nWSETestCaseScenario)
        Catch ex As Exception
            OutputLine("Failed to send all policies to SAM: " & ex.Message)
            'WSETest.HandleException(ex)
        Finally
            m_lReturn = oPMLock.UnlockKey(sKeyName:=sBatchRenewalJobType,
                                            vKeyValue:=lBatchRenewalJobId.ToString,
                                            iUserID:=1)
            oPMLock.Dispose()
            oPMLock = Nothing
            oReqRenSelection = Nothing
            oReqRenInvite = Nothing
            oReqRenAccept = Nothing

            Dim midmessage As String
            Dim lastmessage As String
            Dim lOrgPolicyCnt As Long = 0
            lOrgPolicyCnt = lProcessedPolicyCnt

            If My.Settings.SyncRenewals = True Then
                lProcessedPolicyCnt = lProcessedPolicyCnt - lFailureCnt
                midmessage = "processed through"
                If lOrgPolicyCnt > 1 Then
                    lastmessage = " out of " & lOrgPolicyCnt & " policies"
                Else
                    lastmessage = " out of " & lOrgPolicyCnt & " policy"
                End If
            Else
                midmessage = "sent for"
                lastmessage = "..."
            End If

            If sBatchRenewalJobType.ToUpper = "SEL" Then
                If lProcessedPolicyCnt = 1 Then
                    sSuccessMsg = lProcessedPolicyCnt & " policy " & midmessage & " Renewal Selection" & lastmessage
                    ' sSuccessMsg = lProcessedPolicyCnt & " policy sent for Renewal Selection"
                Else
                    sSuccessMsg = lProcessedPolicyCnt & " policies " & midmessage & " Renewal Selection" & lastmessage
                    'sSuccessMsg = lProcessedPolicyCnt & " policies sent for Renewal Selection"
                End If
            End If

            If sBatchRenewalJobType.ToUpper = "INV" Then
                If lProcessedPolicyCnt = 1 Then
                    sSuccessMsg = lProcessedPolicyCnt & " policy " & midmessage & " Renewal Invite" & lastmessage
                    ' sSuccessMsg = lProcessedPolicyCnt & " policy sent for Renewal Invite"
                Else
                    sSuccessMsg = lProcessedPolicyCnt & " policies " & midmessage & " Renewal Invite" & lastmessage
                    ' sSuccessMsg = lProcessedPolicyCnt & " policies sent for Renewal Invite"
                End If
            End If

            If sBatchRenewalJobType.ToUpper = "ACC" Then
                If lProcessedPolicyCnt = 1 Then
                    sSuccessMsg = lProcessedPolicyCnt & " policy " & midmessage & " Renewal Acceptance" & lastmessage
                    'sSuccessMsg = lProcessedPolicyCnt & " policy sent for Renewal Acceptance"
                Else
                    sSuccessMsg = lProcessedPolicyCnt & " policies " & midmessage & " Renewal Acceptance" & lastmessage
                    'sSuccessMsg = lProcessedPolicyCnt & " policies sent for Renewal Acceptance"
                End If
            End If

            OutputLine(sSuccessMsg)
        End Try

    End Sub
    Private Function GetApiTokendetails() As TokenModel
        Dim apiTokenDetails As TokenModel = New TokenModel()
        apiTokenDetails = GenerateToken.GetJwtTokenForBatchProcess(sClientId, sTokenUrl)
        Dim address As String = sSAMURL
        If address.EndsWith("/") Then
            address = address.Substring(0, address.Length - 1)
        End If
        apiTokenDetails.ApiBaseUrl = address
        apiTokenDetails.TokenUrl = sTokenUrl
        Return apiTokenDetails
    End Function

#End Region

#Region " Missing Data "


    Public Sub InvalidData_Missing_BranchCode()
        ProcessJobs(eMissingData:=enumMissingData.None)
    End Sub

    Public Sub InvalidData_Missing_InsuranceFileKey()
        ProcessJobs(eMissingData:=enumMissingData.None)
    End Sub

#End Region


#Region "Private Methods"
    Private Function GetJobType(ByVal v_sJobCode As String,
                                ByRef r_sBatchRenewalJobType As String,
                                ByRef r_lBatchRenewalJobId As Long,
                                ByRef r_sSamServer As String,
                                ByRef r_bRunExtendedRule As Boolean)


        Const kMethod As String = "GetJobID"


        Dim vResultArray As Object = String.Empty

        GetJobType = PMEReturnCode.PMTrue

        'Clear Pramaeters List
        m_oDatabase.Parameters.Clear()

        'Add Prameters
        AddParameterLite(m_oDatabase,
                        "Batch_Renewal_Job_Code",
                        v_sJobCode,
                        PMEParameterDirection.PMParamInput,
                        PMEDataType.PMString, True)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetJobDetailsSQL,
                                            sSQLName:=ACGetJobDetailsName,
                                            bStoredProcedure:=ACGetJobDetailsStored,
                                            lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

        If IsArray(vResultArray) Then
            r_lBatchRenewalJobId = ToSafeLong(vResultArray(0, 0))
            r_sBatchRenewalJobType = vResultArray(4, 0).ToString
            r_sSamServer = vResultArray(7, 0).ToString
            r_bRunExtendedRule = ToSafeBoolean(vResultArray(10, 0))
        End If

        If m_lReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Fails to Get Job Type")
        End If



    End Function

    Private Function GetRenewalSelectionPolicyList(ByVal v_sJobCode As String, ByRef r_vResultArray(,) As Object) As Long
        Const kMethod As String = "GetRenewalSelectionPolicyList"



        GetRenewalSelectionPolicyList = PMEReturnCode.PMTrue

        'Clear Pramaeters List
        m_oDatabase.Parameters.Clear()

        'Add Prameters
        AddParameterLite(m_oDatabase,
                        "Batch_Renewal_Job_Code",
                        v_sJobCode,
                        PMEParameterDirection.PMParamInput,
                        PMEDataType.PMString, True)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRenewalSelectionPolicyListSQL,
                                            sSQLName:=ACGetRenewalSelectionPolicyListName,
                                            bStoredProcedure:=ACGetRenewalSelectionPolicyListStored,
                                            lNumberRecords:=gPMConstants.PMAllRecords,
                                            vResultArray:=r_vResultArray)


        If m_lReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Fails to GetRenewalSelectionPolicyList")
        End If



    End Function

    Private Function GetRenewalInvitationPolicyList(ByVal v_sJobCode As String, ByRef r_vResultArray(,) As Object) As Long
        Const kMethod As String = "GetRenewalInvitationPolicyList"



        GetRenewalInvitationPolicyList = PMEReturnCode.PMTrue

        'Clear Pramaeters List
        m_oDatabase.Parameters.Clear()

        'Add Prameters
        AddParameterLite(m_oDatabase,
                        "Batch_Renewal_Job_Code",
                        v_sJobCode,
                        PMEParameterDirection.PMParamInput,
                        PMEDataType.PMString, True)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRenewalInvitationPolicyListSQL,
                                            sSQLName:=ACGetRenewalInvitationPolicyListName,
                                            bStoredProcedure:=ACGetRenewalInvitationPolicyListStored,
                                            lNumberRecords:=gPMConstants.PMAllRecords,
                                            vResultArray:=r_vResultArray)


        If m_lReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Fails to GetRenewalInvitationPolicyList")
        End If



    End Function

    Private Function GetRenewalAcceptancePolicyList(ByVal v_sJobCode As String, ByRef r_vResultArray(,) As Object) As Long
        Const kMethod As String = "GetRenewalAcceptancePolicyList"



        GetRenewalAcceptancePolicyList = PMEReturnCode.PMTrue

        'Clear Pramaeters List
        m_oDatabase.Parameters.Clear()

        'Add Prameters
        AddParameterLite(m_oDatabase,
                        "Batch_Renewal_Job_Code",
                        v_sJobCode,
                        PMEParameterDirection.PMParamInput,
                        PMEDataType.PMString, True)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRenewalAcceptancePolicyListSQL,
                                            sSQLName:=ACGetRenewalAcceptancePolicyListName,
                                            bStoredProcedure:=ACGetRenewalAcceptancePolicyListStored,
                                            lNumberRecords:=gPMConstants.PMAllRecords,
                                            vResultArray:=r_vResultArray)


        If m_lReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Fails to GetRenewalAcceptancePolicyList")
        End If



    End Function

    Private Function DelRenewalReport() As Long
        Const kMethod As String = "DelRenewalReport"



        DelRenewalReport = PMEReturnCode.PMTrue



        m_oDatabase.Parameters.Clear()

        'Add Prameters
        AddParameterLite(m_oDatabase,
                        "user_id",
                        1,
                        PMEParameterDirection.PMParamInput,
                        PMEDataType.PMLong, True)

        m_oDatabase.SQLAction(sSQL:=ACDelRenewalReportSQL,
                                    sSQLName:=ACDelRenewalReportName,
                                    bStoredProcedure:=ACDelRenewalReportStored)


        If m_lReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Fails to GetRenewalAcceptancePolicyList")
        End If



    End Function

    Private Function DelRenewalStatusPolicies(ByVal v_lRenewalStatusTypeID As Long,
                                        ByVal v_dtCompareDate As Date) As Long


        Const kMethod As String = "DelRenewalStatusPolicies"

        Try

            DelRenewalStatusPolicies = PMEReturnCode.PMTrue

            '***********************NOTES******************************
            '********These functions must be executed in the correct order*************

            m_lReturn = BeginTrans()
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                DelRenewalStatusPolicies = PMEReturnCode.PMFalse
                Exit Function
            End If

            'delete renewal version policies from insurance file
            m_lReturn = DelRenewalPolicies(v_lRenewalStatusTypeID:=v_lRenewalStatusTypeID, v_dtCompareDate:=v_dtCompareDate)

            If (m_lReturn <> PMEReturnCode.PMTrue) And (m_lReturn <> PMEReturnCode.PMNotFound) Then
                DelRenewalStatusPolicies = m_lReturn
                m_lReturn& = RollbackTrans()
                Exit Function
            End If

            'change status of renewal (original) policies to Null (Live)
            m_lReturn = UpdRenewalPolicies(v_lRenewalStatusTypeID:=v_lRenewalStatusTypeID, v_dtCompareDate:=v_dtCompareDate)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                DelRenewalStatusPolicies = m_lReturn
                m_lReturn& = RollbackTrans()
                Exit Function
            End If

            'delete from last print run
            m_lReturn = DeleteLastPrintRun(v_lRenewalStatusTypeID:=v_lRenewalStatusTypeID, v_dtCompareDate:=v_dtCompareDate)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                DelRenewalStatusPolicies = m_lReturn
                m_lReturn& = RollbackTrans()
                Exit Function
            End If

            'delete renewal policies from renewal status
            m_lReturn = DelRenewalStatus(v_lRenewalStatusTypeID:=v_lRenewalStatusTypeID, v_dtCompareDate:=v_dtCompareDate)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                DelRenewalStatusPolicies = m_lReturn
                m_lReturn& = RollbackTrans()
                Exit Function
            End If

            m_lReturn = CommitTrans()
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                m_lReturn& = RollbackTrans()
                DelRenewalStatusPolicies = PMEReturnCode.PMFalse
                Exit Function
            End If

        Catch ex As Exception
            m_lReturn& = RollbackTrans()
            DelRenewalStatusPolicies = PMEReturnCode.PMError
            Throw New ArgumentException(GetErrorMsgString(ACClass, kMethod) & " Fails to Delete Renewal Status Policies", ex)
        Finally

        End Try
    End Function

    Private Function DelRenewalPolicies(ByVal v_lRenewalStatusTypeID As Long,
                                    ByVal v_dtCompareDate As Date)


        Const kMethod As String = "DelRenewalPolicies"
        Dim lCount As Long
        Dim vRenewalPolicies As Object = Nothing


        DelRenewalPolicies = PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        'Add Prameters
        AddParameterLite(m_oDatabase,
                        "renewal_status_type_id",
                        v_lRenewalStatusTypeID,
                        PMEParameterDirection.PMParamInput,
                        PMEDataType.PMLong)


        AddParameterLite(m_oDatabase,
                        "compare_Date",
                        v_dtCompareDate,
                        PMEParameterDirection.PMParamInput,
                        PMEDataType.PMDate)

        DelRenewalPolicies = m_oDatabase.SQLSelect(sSQL:=ACSelRenewalPolicySQL,
                                                    sSQLName:=ACSelRenewalPolicyName,
                                                    bStoredProcedure:=ACSelRenewalPolicyStored,
                                                    lNumberRecords:=gPMConstants.PMAllRecords,
                                                    vResultArray:=vRenewalPolicies,
                                                    bKeepNulls:=True)


        If DelRenewalPolicies <> PMEReturnCode.PMTrue Then
            Exit Function
        End If

        If Not IsArray(vRenewalPolicies) Then
            DelRenewalPolicies = PMEReturnCode.PMNotFound
            Exit Function
        End If

        For lCount& = 0 To UBound(vRenewalPolicies, 2)
            DelRenewalPolicies = DeletePolicy(v_lInsuranceFileCnt:=vRenewalPolicies(0, lCount&))

            If DelRenewalPolicies <> PMEReturnCode.PMTrue Then
                Exit For
            End If
        Next

        Exit Function


    End Function

    Private Function UpdRenewalPolicies(ByVal v_lRenewalStatusTypeID As Long,
                                    ByVal v_dtCompareDate As Date)


        Const kMethod As String = "UpdRenewalPolicies"



        UpdRenewalPolicies = PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        'Add Prameters
        AddParameterLite(m_oDatabase,
                        "renewal_status_type_id",
                        v_lRenewalStatusTypeID,
                        PMEParameterDirection.PMParamInput,
                        PMEDataType.PMLong)


        AddParameterLite(m_oDatabase,
                        "compare_Date",
                        v_dtCompareDate,
                        PMEParameterDirection.PMParamInput,
                        PMEDataType.PMDate)


        UpdRenewalPolicies = m_oDatabase.SQLAction(sSQL:=ACUpdRenewalPolicySQL,
                    sSQLName:=ACUpdRenewalPolicyName,
                    bStoredProcedure:=ACUpdRenewalPolicyStored)


        Exit Function


    End Function

    Private Function DeleteLastPrintRun(ByVal v_lRenewalStatusTypeID As Long,
                                    ByVal v_dtCompareDate As Date)


        Const kMethod As String = "DeleteLastPrintRun"



        DeleteLastPrintRun = PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        'Add Prameters
        AddParameterLite(m_oDatabase,
                        "renewal_status_type_id",
                        v_lRenewalStatusTypeID,
                        PMEParameterDirection.PMParamInput,
                        PMEDataType.PMLong)


        AddParameterLite(m_oDatabase,
                        "compare_Date",
                        v_dtCompareDate,
                        PMEParameterDirection.PMParamInput,
                        PMEDataType.PMDate)


        DeleteLastPrintRun = m_oDatabase.SQLAction(sSQL:=ACDelLastPrintRunSQL,
                                    sSQLName:=ACDelLastPrintRunName,
                                    bStoredProcedure:=ACDelLastPrintRunStored)


        Exit Function


    End Function

    Private Function DelRenewalStatus(ByVal v_lRenewalStatusTypeID As Long,
                                    ByVal v_dtCompareDate As Date)


        Const kMethod As String = "DelRenewalStatus"



        DelRenewalStatus = PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        'Add Prameters
        AddParameterLite(m_oDatabase,
                        "renewal_status_type_id",
                        v_lRenewalStatusTypeID,
                        PMEParameterDirection.PMParamInput,
                        PMEDataType.PMLong)


        AddParameterLite(m_oDatabase,
                        "compare_Date",
                        v_dtCompareDate,
                        PMEParameterDirection.PMParamInput,
                        PMEDataType.PMDate)


        DelRenewalStatus = m_oDatabase.SQLAction(sSQL:=ACDelRenewalStatusSQL,
                                    sSQLName:=ACDelRenewalStatusName,
                                    bStoredProcedure:=ACDelRenewalStatusStored)


        Exit Function


    End Function


    Private Function DeletePolicy(ByVal v_lInsuranceFileCnt As Long)


        Const kMethod As String = "DeletePolicy"
        Dim lReturn As Long

        Try

            DeletePolicy = PMEReturnCode.PMTrue

            lReturn = BeginTrans()

            'delete all dependencies first
            lReturn = DeletePolicyDependant(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
            If (lReturn <> PMEReturnCode.PMTrue) Then
                lReturn = RollbackTrans()
                DeletePolicy = PMEReturnCode.PMFalse
                Exit Function
            End If

            'delete Insurance_File_System record.
            lReturn = DeleteInsuranceFileSystem(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
            If (lReturn <> PMEReturnCode.PMTrue) Then
                lReturn = RollbackTrans()
                DeletePolicy = PMEReturnCode.PMFalse
                Exit Function
            End If

            'Finally delete Insurance_File record itself.
            lReturn = DeleteInsuranceFile(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
            If (lReturn <> PMEReturnCode.PMTrue) Then
                lReturn = RollbackTrans()
                DeletePolicy = PMEReturnCode.PMFalse
                Exit Function
            End If

            lReturn = CommitTrans()
            If (lReturn <> PMEReturnCode.PMTrue) Then
                lReturn = RollbackTrans()
                DeletePolicy = PMEReturnCode.PMFalse
                Exit Function
            End If

            Exit Function

        Catch ex As Exception
            lReturn = RollbackTrans()
            DeletePolicy = PMEReturnCode.PMError
            Throw New ArgumentException(GetErrorMsgString(ACClass, kMethod) & " Fails to DeletePolicy", ex)
        Finally

        End Try
    End Function

    Private Function DeletePolicyDependant(ByVal v_lInsuranceFileCnt As Long) As Long

        Const kMethod As String = "DeletePolicyDependant"



        DeletePolicyDependant = PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        'Add Prameters
        AddParameterLite(m_oDatabase,
                        "insurance_file_cnt",
                        v_lInsuranceFileCnt,
                        PMEParameterDirection.PMParamInput,
                        PMEDataType.PMLong)


        DeletePolicyDependant = m_oDatabase.SQLAction(sSQL:=ACDelPolicyDependantSQL,
                                                    sSQLName:=ACDelPolicyDependantName,
                                                    bStoredProcedure:=ACDelPolicyDependantStored)




        Exit Function


    End Function

    Private Function DeleteInsuranceFileSystem(ByVal v_lInsuranceFileCnt As Long) As Long

        Const kMethod As String = "DeleteInsuranceFileSystem"



        DeleteInsuranceFileSystem = PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        'Add Prameters
        AddParameterLite(m_oDatabase,
                        "insurance_file_cnt",
                        v_lInsuranceFileCnt,
                        PMEParameterDirection.PMParamInput,
                        PMEDataType.PMLong)


        DeleteInsuranceFileSystem = m_oDatabase.SQLAction(sSQL:=ACDelInsFileSystemSQL,
                                    sSQLName:=ACDelInsFileSystemName,
                                    bStoredProcedure:=ACDelInsFileSystemStored)


        Exit Function


    End Function

    Private Function DeleteInsuranceFile(ByVal v_lInsuranceFileCnt As Long) As Long

        Const kMethod As String = "DeleteInsuranceFile"

        Try

            DeleteInsuranceFile = PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            'Add Prameters
            AddParameterLite(m_oDatabase,
                            "insurance_file_cnt",
                            v_lInsuranceFileCnt,
                            PMEParameterDirection.PMParamInput,
                            PMEDataType.PMLong)


            DeleteInsuranceFile = m_oDatabase.SQLAction(sSQL:=ACDelInsFileSQL,
                                        sSQLName:=ACDelInsFileName,
                                        bStoredProcedure:=ACDelInsFileStored)

            Exit Function

        Catch ex As Exception
            DeleteInsuranceFile = PMEReturnCode.PMError
            Throw New ArgumentException(GetErrorMsgString(ACClass, kMethod) & " Fails to DeleteInsuranceFile", ex)
        Finally

        End Try
    End Function

    Private Function BeginTrans() As Long
        Const kMethod As String = "BeginTrans"

        Try

            BeginTrans = PMEReturnCode.PMTrue


            ' Begin the Transaction
            m_lReturn& = m_oDatabase.SQLBeginTrans

            If (m_lReturn& <> PMEReturnCode.PMTrue) Then
                BeginTrans = PMEReturnCode.PMFalse
                Exit Function
            End If

        Catch ex As Exception
            BeginTrans = PMEReturnCode.PMError
            Throw New ArgumentException(GetErrorMsgString(ACClass, kMethod) & " Fails to BeginTrans", ex)
        Finally

        End Try
    End Function

    Private Function CommitTrans() As Long
        Const kMethod As String = "CommitTrans"

        Try

            CommitTrans = PMEReturnCode.PMTrue


            ' Commits the Transaction
            m_lReturn& = m_oDatabase.SQLCommitTrans

            If (m_lReturn& <> PMEReturnCode.PMTrue) Then
                CommitTrans = PMEReturnCode.PMFalse
                Exit Function
            End If

        Catch ex As Exception
            CommitTrans = PMEReturnCode.PMError
            Throw New ArgumentException(GetErrorMsgString(ACClass, kMethod) & " Fails to CommitTrans", ex)
        Finally

        End Try
    End Function

    Private Function RollbackTrans() As Long
        Const kMethod As String = "RollbackTrans"

        Try

            RollbackTrans = PMEReturnCode.PMTrue


            ' Rollback the Transaction
            m_lReturn& = m_oDatabase.SQLRollbackTrans

            If (m_lReturn& <> PMEReturnCode.PMTrue) Then
                RollbackTrans = PMEReturnCode.PMFalse
                Exit Function
            End If

        Catch ex As Exception
            RollbackTrans = PMEReturnCode.PMError
            Throw New ArgumentException(GetErrorMsgString(ACClass, kMethod) & " Fails to RollbackTrans", ex)
        Finally

        End Try
    End Function

#End Region

#Region "Methods"


    ' close database connection
    Public Sub CloseDBConnection()
        DBDisconnect(m_oDatabase)
    End Sub

    Public Sub CleanUpInterops()
        ' clean up the database interop
        '        DestroyCOMInterop(m_oDatabase, True)
        m_oDatabase = Nothing

    End Sub


#End Region
#Region "Creator"

    Public Sub New()

        ' Connect to database
        DBConnect(m_oDatabase)
    End Sub

    Protected Overrides Sub Finalize()
        'MyBase.Finalize()
    End Sub

#End Region

End Class


