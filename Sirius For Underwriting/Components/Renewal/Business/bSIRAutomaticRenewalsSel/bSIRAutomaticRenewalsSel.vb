Option Strict Off
Option Explicit On
Imports System.IO
'developer guide no. 129
Imports System.Text
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ************************************************
    ' Added to replace global variables 19/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Business"

    Private m_dtSelectionDate As Date


    ' Declare an instance of the Business object.
    Private m_oBusiness As bSIRRenSelection.Business
    Private m_oInsuranceFile As bSIRInsuranceFile.Services
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_oTax As bSIRRITax.Business
    Private m_oReinsurance As Object
    Private m_oChangePolicyStatus As bSIRChangePolicyStatus.Business
    Private m_oAgentCommission As bSirAgentCommission.Business
    Private m_oRiskData As bSIRRiskData.Business
    Private m_oPerilAllocation As bSirPerilAllocation.Business
    Private m_oEvent As bSIREvent.Business
    Private m_bExtraRiskDetails As Boolean
    Private m_oReport As Object
    Private m_oDocManagerWrapper As Object

    Private m_oDatabase As dPMDAO.Database
    Private m_oAutoMTAMerge As AutoMTAMerge
    Private m_oGis As Object
    Private m_oDataset As cGISDataSetControl.Application
    Private m_oPolicyNumMaint As bSIRPolicyNumMaint.Business
    Private m_lReturn As Integer
    Private m_vProductArray(,) As Object
    Private m_lEventType As Integer
    Private m_bCloseDatabase As Boolean

    Private m_sFailureReason As String = ""
    Private m_lInsuranceFileCnt As Integer
    Private m_lNewInsuranceFileCnt As Integer
    Private m_bProductLocked As Boolean
    Private m_sSystemErrorText As String = ""
    Private m_bNoTransactionRequired As Boolean
    Private m_lStartRecord As Integer
    Private m_lRecordIncrement As Integer
    Private m_bCalledFromAutoMTA As Boolean
    Private m_bRI2007Enabled As Object
    Private m_lAffectedInsuranceFileCnt As Integer
#If IN_DEBUG > 0 Then

	Private m_oDebugTimings As Object
#End If


    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public ReadOnly Property NewInsuranceFileCnt() As Integer
        Get
            Return m_lNewInsuranceFileCnt
        End Get
    End Property

    Public ReadOnly Property FailureReason() As String
        Get
            Return m_sFailureReason
        End Get
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public WriteOnly Property dtSelectionDate() As Date
        Set(ByVal Value As Date)
            m_dtSelectionDate = Value
        End Set
    End Property

    Public WriteOnly Property CalledFromAutoMTA() As Boolean
        Set(ByVal Value As Boolean)
            m_bCalledFromAutoMTA = Value
        End Set
    End Property

    Public WriteOnly Property AffectedInsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lAffectedInsuranceFileCnt = Value
        End Set
    End Property
    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Dim nResult As Integer
            nResult = gPMConstants.PMEReturnCode.PMTrue
            Me.disposedValue = True
            If disposing Then
                If m_vProductArray IsNot Nothing Then
                    For iCnt As Integer = m_vProductArray.GetLowerBound(1) To m_vProductArray.GetUpperBound(1)
                        m_lReturn = UnlockProductForRenewal(m_vProductArray(0, iCnt))
                    Next iCnt
                End If

                m_lReturn = CloseBusinessObject()
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
#If IN_DEBUG > 0 Then

			Set m_oDebugTimings = Nothing
#End If

            End If
        End If
        Me.disposedValue = True
    End Sub

    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer,
                               ByRef iLogLevel As Integer, ByRef sCallingAppName As String,
                               Optional ByRef vDatabase As Object = Nothing) As Integer


        Dim nResult As Integer
        Dim sLockedBy As String = ""

        nResult = gPMConstants.PMEReturnCode.PMTrue
        '
        ' *******************************************************************
        ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
        m_sUsername = sUsername
        m_sPassword = sPassword
        m_iUserID = iUserID
        m_sCallingAppName = sCallingAppName
        m_iLanguageID = iLanguageID
        m_iSourceID = iSourceID
        m_iCurrencyID = iCurrencyID
        m_iLogLevel = iLogLevel


        Try


            ' Set Username and Password


            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID,
                                                           v_lPMProductFamily:=PMProductFamily,
                                                           r_bNewInstanceCreated:=m_bCloseDatabase,
                                                           r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = UseExtraRiskData()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'bSIRRenSelection
            m_oBusiness = New bSIRRenSelection.Business

            m_lReturn = m_oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID,
                                         iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID,
                                         iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel,
                                         sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            m_lReturn = m_oBusiness.GetLookUp(v_sTableName:="Product", v_sKeyIDFieldName:="product_id",
                                              v_sDescFieldName:="description", r_vResultArray:=m_vProductArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="m_oBusiness.GetLookUp for table 'Product' failed.", vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="Initialise")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Dim oEventTypeArray(,) As Object = Nothing

            m_lReturn = m_oBusiness.GetLookUp(v_sTableName:="Event_Type", v_sKeyIDFieldName:="Event_Type_id",
                                              v_sDescFieldName:="code", r_vResultArray:=oEventTypeArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="m_oBusiness.GetLookUp for table 'Event_Type' failed.", vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Informations.IsArray(oEventTypeArray) Then

                For iCnt As Integer = 0 To oEventTypeArray.GetUpperBound(1)

                    If CStr(oEventTypeArray(1, iCnt)).Trim() = "RENEWAL" Then

                        m_lEventType = CInt(oEventTypeArray(0, iCnt))
                        Exit For
                    End If
                Next iCnt
            End If

            If Not (m_lEventType > 0) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = CreateBusinessObject()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="CreateBusinessObject failed.", vApp:=ACApp, vClass:=ACClass,
                                   vMethod:="Initialise", vErrNo:=Informations.Err().Number,
                                   vErrDesc:=Informations.Err().Description)

                Return nResult
            End If

#If IN_DEBUG > 0 Then

			'Debug Timings
			Set m_oDebugTimings = CreateLateBoundObject("bSIRDebugTimings.Interface")
			m_oDebugTimings.CallingAppName = ACApp
#End If

            Return nResult

        Catch excep As System.Exception


            nResult = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    Public Function Start() As Integer

        Dim result As Integer = 0
        Dim vRenewalList(,) As Object = Nothing
        Dim lInsuranceFileCnt As Integer
        Dim sInsuranceRef As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "Start"
#End If

            'Delete all policies with a status of PMBRenewalStatusTypePolicyChanged

            m_lReturn =
                m_oBusiness.DelRenewalStatusPolicies(
                    v_lRenewalStatusTypeID:=gPMConstants.PMBRenewalStatusTypePolicyChanged,
                    v_dtCompareDate:=m_dtSelectionDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="m_oBusiness.DelRenewalStatusPolicies failed.", vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="Start")
                Return result
            End If

            'Get a list of all the policies for selection

            m_lReturn = m_oBusiness.GetRenewalSelection(0, v_vBranchID:=0, v_dtCompareDate:=m_dtSelectionDate,
                                                        r_vResultArray:=vRenewalList)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="m_oBusiness.DelRenewalStatusPolicies failed.", vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="Start")
                Return result
            End If

            'delete all in renewal_report table ready for new data

            m_lReturn = m_oBusiness.DelRenewalReport()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="m_oBusiness.DelRenewalReport failed.", vApp:=ACApp, vClass:=ACClass,
                                   vMethod:="Start", vErrNo:=Informations.Err().Number,
                                   vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If Not Informations.IsArray(vRenewalList) Then
                'Nothing to do so exit here
#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "Start"
				m_oDebugTimings.Report
#End If
                Return result
            End If

            m_iTask = gPMConstants.PMEComponentAction.PMEdit


            For iCnt As Integer = vRenewalList.GetLowerBound(1) To vRenewalList.GetUpperBound(1)


                lInsuranceFileCnt = CInt(vRenewalList(PMFieldPosInsuranceFileCnt, iCnt))

                sInsuranceRef = CStr(vRenewalList(4, iCnt))

#If IN_DEBUG > 0 Then

				m_oDebugTimings.StartTiming "CreateRenewalPolicy: " & sInsuranceRef
#End If

                'Start a database transaction
                m_lReturn = BeginTrans(v_vInsuranceFileCnt:=lInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Process renewal policy

                m_lReturn = CreateRenewalPolicy(vRenewalList, iCnt)

#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "CreateRenewalPolicy: " & sInsuranceRef
#End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="CreateRenewalPolicy failed for " & sInsuranceRef, vApp:=ACApp,
                                       vClass:=ACClass, vMethod:="Start")
                    'Rollback the transaction
                    m_lReturn = RollbackTrans(v_vInsuranceFileCnt:=lInsuranceFileCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    'commit the transaction
                    m_lReturn = CommitTrans(v_vInsuranceFileCnt:=lInsuranceFileCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            Next iCnt


            'Print reports
            'Manual Report
            If PrintReport("ManualRenewal") <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="PrintReport failed for 'ManualRenewal'", vApp:=ACApp, vClass:=ACClass,
                                   vMethod:="Start")
                Return result
            End If

            'Automatic Report
            If PrintReport("AutomaticRenewal") <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="PrintReport failed for 'AutomaticRenewal'", vApp:=ACApp, vClass:=ACClass,
                                   vMethod:="Start")
                Return result
            End If

#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "Start"
			m_oDebugTimings.Report
#End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ''' <summary>
    ''' Change current policy status to Renewal
    ''' Create new policy of type Renewal
    ''' </summary>
    ''' <param name="r_vRenewalList"></param>
    ''' <param name="v_lCount"></param>
    ''' <param name="v_bMergeRisks"></param>
    ''' <param name="v_lPreChangeInsFileCnt"></param>
    ''' <param name="v_lPostChangeInsFileCnt"></param>
    ''' <param name="r_lDeletedRiskInsuranceFileCnt"></param>
    ''' <param name="r_lDeletedRiskCnt"></param>
    ''' <param name="v_iRunMode"></param>
    ''' <param name="r_bShowQuoteMsg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateRenewalPolicy(ByRef r_vRenewalList(,) As Object, ByVal v_lCount As Integer,
                                        Optional ByVal v_bMergeRisks As Boolean = False,
                                        Optional ByVal v_lPreChangeInsFileCnt As Integer = 0,
                                        Optional ByVal v_lPostChangeInsFileCnt As Integer = 0,
                                        Optional ByRef r_lDeletedRiskInsuranceFileCnt As Integer = 0,
                                        Optional ByRef r_lDeletedRiskCnt As Integer = 0,
                                        Optional ByVal v_iRunMode As Integer = 0,
                                        Optional ByRef r_bShowQuoteMsg As Boolean = False) As Integer
        Dim nResult As Integer
        Dim nRenewalStatusTypeID As Integer = 0 'renewal status to go on the Renewal Status table
        Dim sFailureCriterion As New StringBuilder
        Dim nNewInsuranceFileCnt As Integer
        Dim sInsuranceRef As String = ""
        Dim lEligibleForRenewal As gPMConstants.PMEReturnCode
        Dim oKeyArray(,) As Object = Nothing
        Dim oInsuranceFileTax As Object = Nothing
        Dim sDescription As String = ""
        Dim oFailureArray(,) As Object = Nothing
        Dim sFailure As String = ""
        Dim lIsQuoted As gPMConstants.PMEReturnCode
        Dim bCreateWorkManagerTask As Boolean
        Dim oArray As Object = Nothing
        Dim nUbound As Integer = 0
        Dim nArrayUBound As Integer = 0
        Dim dtEffectiveDate As Date

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            lEligibleForRenewal = gPMConstants.PMEReturnCode.PMTrue

            sInsuranceRef = gPMFunctions.NullToString(CStr(r_vRenewalList(PMFieldPosInsuranceRef, v_lCount)))

            'Create the policy in back office
#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "CreateBackOfficePolicy"
#End If

            m_lReturn =
                CreateBackOfficePolicy(
                    v_lOldInsuranceFileCnt:=
                                          gPMFunctions.NullToLong(CStr(r_vRenewalList(PMFieldPosInsuranceFileCnt,
                                                                                      v_lCount))),
                    v_sInsuranceRef:=sInsuranceRef,
                    v_dtExpiryDate:=gPMFunctions.NullToDate(r_vRenewalList(PMFieldPosCoverEndDate, v_lCount)),
                    v_dtRenewalDate:=gPMFunctions.NullToDate(r_vRenewalList(PMFieldPosCoverStartDate, v_lCount)),
                    r_lNewInsuranceFileCnt:=nNewInsuranceFileCnt, r_dtEffectiveDate:=dtEffectiveDate,
                    bisTMPPolicy:=r_vRenewalList(kIsTMP, v_lCount))
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "CreateBackOfficePolicy"
#End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="CreateRenewalPolicy Failed", vApp:=ACApp, vClass:=ACClass,
                                   vMethod:="CreateRenewalPolicy")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lNewInsuranceFileCnt = nNewInsuranceFileCnt

            'RWH(22/08/01) Must set the Task in Tax as this is passed into stored procedures
            'as Mode. If it is wrong the new tax records will not be created.

            m_lReturn = m_oTax.SetProcessModes(vTask:=m_iTask, vTransactionType:="MTA")

            'This will be set to false if any of the risks decline or refer as
            'I.A.G only want one workmanager task creating per policy
            bCreateWorkManagerTask = True

            'copy risk details to new policy
#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "CopyRiskData"
#End If
            If v_bMergeRisks Then
                ' Update policy premium upfront for already available risks.
                ' All other risks here should either be Unchanged or unquoted
                m_lReturn = m_oChangePolicyStatus.UpdatePolicyPremium(
                    v_lInsuranceFileCnt:=nNewInsuranceFileCnt)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    bPMFunc.LogMessage(m_sUsername,
                        iType:=gPMConstants.PMELogLevel.PMLogOnError,
                        sMsg:="m_oChangePolicyStatus.UpdatePolicyPremium Failed for " & sInsuranceRef,
                        vApp:=ACApp,
                        vClass:=ACClass,
                        vMethod:="CreateRenewalPolicy")
                    CreateRenewalPolicy = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
                'Backdated MTA - we are reapplying the renewal and merging any
                ' changes which were made
                m_lReturn = CopyRiskDataWithMerge(r_vRenewalList:=r_vRenewalList, v_lCount:=v_lCount,
                                                  v_lNewInsuranceFileCnt:=nNewInsuranceFileCnt,
                                                  r_sFailureReason:=sFailureCriterion.ToString(),
                                                  r_lEligibleForRenewal:=lEligibleForRenewal,
                                                  r_bCreateWorkManagerTask:=bCreateWorkManagerTask,
                                                  v_dtEffectiveDate:=dtEffectiveDate,
                                                  v_lPreChangeInsFileCnt:=v_lPreChangeInsFileCnt,
                                                  v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt)
            ElseIf v_iRunMode = 1 Then
                'The risk was deleted at renewal
                m_lReturn = CopyRiskDataReinstateDeletedRisk(r_vRenewalList:=r_vRenewalList, v_lCount:=v_lCount,
                                                             v_lNewInsuranceFileCnt:=nNewInsuranceFileCnt,
                                                             r_sFailureReason:=sFailureCriterion.ToString(),
                                                             r_lEligibleForRenewal:=lEligibleForRenewal,
                                                             r_bCreateWorkManagerTask:=bCreateWorkManagerTask,
                                                             v_dtEffectiveDate:=dtEffectiveDate,
                                                             v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt,
                                                             r_lDeletedRiskInsuranceFileCnt:=
                                                                r_lDeletedRiskInsuranceFileCnt,
                                                             r_lDeletedRiskCnt:=r_lDeletedRiskCnt,
                                                             v_iRunMode:=v_iRunMode)
            ElseIf v_iRunMode = 2 Then
                'The risk was deleted prior to renewal so append the deleted risk
                ' to the original renewal
                m_lReturn = CopyRiskDataApplyDeletedRisk(r_vRenewalList:=r_vRenewalList, v_lCount:=v_lCount,
                                                         v_lNewInsuranceFileCnt:=nNewInsuranceFileCnt,
                                                         r_sFailureReason:=sFailureCriterion.ToString(),
                                                         r_lEligibleForRenewal:=lEligibleForRenewal,
                                                         r_bCreateWorkManagerTask:=bCreateWorkManagerTask,
                                                         v_dtEffectiveDate:=dtEffectiveDate,
                                                         v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt,
                                                         r_lDeletedRiskInsuranceFileCnt:=
                                                            r_lDeletedRiskInsuranceFileCnt,
                                                         r_lDeletedRiskCnt:=r_lDeletedRiskCnt,
                                                         v_iRunMode:=v_iRunMode)
            Else
                m_lReturn = CopyRiskData(r_vRenewalList:=r_vRenewalList, v_lCount:=v_lCount,
                                         v_lNewInsuranceFileCnt:=nNewInsuranceFileCnt,
                                         r_sFailureReason:=sFailureCriterion.ToString(),
                                         r_lEligibleForRenewal:=lEligibleForRenewal,
                                         r_bCreateWorkManagerTask:=bCreateWorkManagerTask,
                                         v_dtEffectiveDate:=dtEffectiveDate)
            End If
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "CopyRiskData"
#End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="CopyRiskData Failed for " & sInsuranceRef, vApp:=ACApp, vClass:=ACClass,
                                   vMethod:="CreateRenewalPolicy")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' added CalledFromAutoMTA test
            If m_bCalledFromAutoMTA Then
                ' automatically renew policy regardless of auto renewal criteria if part of an auto MTA process
                ' (ie replacing an existing renewal)
                ' reverse fee
                If m_lAffectedInsuranceFileCnt <> 0 Then
                    m_oDatabase.Parameters.Clear()
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt",
                                                           vValue:=NullToLong(m_lAffectedInsuranceFileCnt),
                                                           iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                           iDataType:=gPMConstants.PMEDataType.PMInteger)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt",
                                                           vValue:=nNewInsuranceFileCnt,
                                                           iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                           iDataType:=gPMConstants.PMEDataType.PMInteger)
                    m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_sir_policy_fee_rev", sSQLName:="Reverse Fee",
                                                      bStoredProcedure:=True)
                Else
                    m_oDatabase.Parameters.Clear()
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt",
                                                           vValue:=
                                                              NullToLong(r_vRenewalList(PMFieldPosInsuranceFileCnt,
                                                                                        v_lCount)),
                                                           iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                           iDataType:=gPMConstants.PMEDataType.PMInteger)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt",
                                                           vValue:=nNewInsuranceFileCnt,
                                                           iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                           iDataType:=gPMConstants.PMEDataType.PMInteger)
                    m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_sir_policy_fee_rev", sSQLName:="Reverse Fee",
                                                      bStoredProcedure:=True)
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            Else
                'Check Auto-renewal eligibility.
#If IN_DEBUG > 0 Then

				m_oDebugTimings.StartTiming "m_oBusiness.CheckRenewalCriteria"
#End If


                If m_oBusiness.CheckRenewalCriteria(r_vRenewalList, ToSafeInteger(v_lCount)) = gPMConstants.PMEReturnCode.PMFalse Then
                    lEligibleForRenewal = gPMConstants.PMEReturnCode.PMFalse
                End If

#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "m_oBusiness.CheckRenewalCriteria"
#End If


                'Get the reasons for failure.
#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "m_oBusiness.GetRenewalFailures"
#End If


                m_lReturn =
                    m_oBusiness.GetRenewalFailures(
                        gPMFunctions.NullToString(CStr(r_vRenewalList(ToSafeInteger(PMFieldPosInsuranceRef), v_lCount))), CType(oFailureArray, Object))
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "m_oBusiness.GetRenewalFailures"
#End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="m_oBusiness.GetRenewalFailures Failed for " & sInsuranceRef, vApp:=ACApp,
                                       vClass:=ACClass, vMethod:="CreateRenewalPolicy")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            If lEligibleForRenewal = gPMConstants.PMEReturnCode.PMTrue Then

#If IN_DEBUG > 0 Then

				m_oDebugTimings.StartTiming "m_oBusiness.IsQuoted"
#End If

                m_lReturn = m_oBusiness.IsQuoted(v_lInsuranceFileCnt:=nNewInsuranceFileCnt, r_lIsQuoted:=lIsQuoted)
#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "m_oBusiness.IsQuoted"
#End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    lIsQuoted = gPMConstants.PMEReturnCode.PMFalse
                End If

                If lIsQuoted = gPMConstants.PMEReturnCode.PMFalse Then
                    lEligibleForRenewal = gPMConstants.PMEReturnCode.PMFalse
                    sFailureCriterion = New StringBuilder(PMIsQuotedDesc)
                    r_bShowQuoteMsg = True
                End If

                If lEligibleForRenewal = gPMConstants.PMEReturnCode.PMFalse Then
                    If m_lInsuranceFileCnt = 0 Then
#If IN_DEBUG > 0 Then

						m_oDebugTimings.StartTiming "m_oBusiness.AddRenewalReport"
#End If

                        m_lReturn = m_oBusiness.AddRenewalReport(v_sReportType:="ManualRenewal",
                                                                 v_vClientName:=
                                                                    r_vRenewalList(PMFieldPosClientName, v_lCount),
                                                                 v_vPolicyNumber:=
                                                                    r_vRenewalList(PMFieldPosInsuranceRef, v_lCount),
                                                                 v_vAgentCode:=
                                                                    r_vRenewalList(PMFieldPosAgentName, v_lCount),
                                                                 v_vCoverStartDate:=
                                                                    r_vRenewalList(PMFieldPosCoverStartDate, v_lCount),
                                                                 v_vCoverEndDate:=
                                                                    r_vRenewalList(PMFieldPosCoverEndDate, v_lCount),
                                                                 v_vProductCode:=
                                                                    r_vRenewalList(PMFieldPosProductCode, v_lCount),
                                                                 v_vFailureCriterion:=sFailureCriterion.ToString(),
                                                                 v_vFailureDetail:="")
#If IN_DEBUG > 0 Then

						m_oDebugTimings.EndTiming "m_oBusiness.AddRenewalReport"
#End If
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                               sMsg:="m_oBusiness.AddRenewalReport Failed for " & sInsuranceRef,
                                               vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRenewalPolicy")
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        m_sFailureReason = sFailureCriterion.ToString()
                    End If

                End If
            End If
            'If ineligible for renewal, either because a risk failed rating OR
            'policy level auto-renewal criteria were failed, then set RenewalStatusType
            'and message appropriately.
            If lEligibleForRenewal = gPMConstants.PMEReturnCode.PMFalse Then
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
            Else

                'Set status to say we are ready to print the renewal notice.
                nRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeAutoRated

                'DO POLICY LEVEL STUFF

                'Reset RiskId to ensure Policy level reinsurance is done.

                m_oReinsurance.InsuranceFileCnt = nNewInsuranceFileCnt

                m_oReinsurance.RiskId = 0
#If IN_DEBUG > 0 Then

				m_oDebugTimings.StartTiming "m_oReinsurance.GetDetails"
#End If

                m_lReturn = m_oReinsurance.Getdetails
                ' Not found is not a system error
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound _
                    Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="m_oReinsurance.GetDetails Failed for " & sInsuranceRef, vApp:=ACApp,
                                       vClass:=ACClass, vMethod:="CreateRenewalPolicy")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "m_oReinsurance.GetDetails"
#End If
                'Do policy taxes.

                m_oTax.InsuranceFileCnt = nNewInsuranceFileCnt
#If IN_DEBUG > 0 Then

				m_oDebugTimings.StartTiming "m_oTax.GetInsuranceFileTax"
#End If

                m_lReturn = m_oTax.GetInsuranceFileTax(oInsuranceFileTax, sDescription)
#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "m_oTax.GetInsuranceFileTax"
#End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="m_oTax.GetInsuranceFileTax Failed for " & sInsuranceRef, vApp:=ACApp,
                                       vClass:=ACClass, vMethod:="CreateRenewalPolicy")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If m_bCalledFromAutoMTA = True Then
                    m_oDatabase.Parameters.Clear()
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt", vValue:=NullToLong(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=nNewInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_sir_agent_commission_rev", sSQLName:="Reverse Comm", bStoredProcedure:=True)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="m_oTax.ReverseComm Failed for " & sInsuranceRef, vApp:=ACApp,
                                       vClass:=ACClass, vMethod:="CreateRenewalPolicy")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' reverse fee
                    If m_lAffectedInsuranceFileCnt <> 0 Then
                        m_oDatabase.Parameters.Clear()
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt", vValue:=NullToLong(m_lAffectedInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=nNewInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_sir_policy_fee_rev", sSQLName:="Reverse Fee", bStoredProcedure:=True)
                    Else
                        m_oDatabase.Parameters.Clear()
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt", vValue:=NullToLong(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=nNewInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_sir_policy_fee_rev", sSQLName:="Reverse Fee", bStoredProcedure:=True)
                    End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="m_oTax.ReverseFee Failed for " & sInsuranceRef, vApp:=ACApp,
                                       vClass:=ACClass, vMethod:="CreateRenewalPolicy")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=nNewInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_code", vValue:="REN", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_SAM_Update_Insurance_File_System", sSQLName:="UpdateInsuranceFileSystem", bStoredProcedure:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="Update insurance file system " & sInsuranceRef, vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="CreateRenewalPolicy")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                If m_bCalledFromAutoMTA = False Or v_bMergeRisks = False Then

                    'Update policy premium.
#If IN_DEBUG > 0 Then

				m_oDebugTimings.StartTiming "m_oChangePolicyStatus.UpdatePolicyPremium"
#End If

                    m_lReturn = m_oChangePolicyStatus.UpdatePolicyPremium(v_lInsuranceFileCnt:=nNewInsuranceFileCnt)
#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "m_oChangePolicyStatus.UpdatePolicyPremium"
#End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                           sMsg:="m_oChangePolicyStatus.UpdatePolicyPremium Failed for " & sInsuranceRef,
                                           vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRenewalPolicy")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Do agent commission.

                    m_oAgentCommission.InsuranceFileCnt = nNewInsuranceFileCnt

                    'Tomo17102001 - The Get deletes the existing records, and recalculates them
                    'but does _not_ write them back to the database.  The calculate does...
                    m_lReturn = m_oAgentCommission.GetAgentCommission(v_lInsuranceFileCnt:=nNewInsuranceFileCnt, r_vntResult:=oArray)
#If IN_DEBUG > 0 Then

				m_oDebugTimings.StartTiming "m_oAgentCommission.CalculateAgentCommission"
#End If

                    m_lReturn = m_oAgentCommission.CalculateAgentCommission(v_lInsuranceFileCnt:=nNewInsuranceFileCnt,
                                                                            v_sTransactionType:="REN",
                                                                            r_vntResult:=oArray)
#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "m_oAgentCommission.CalculateAgentCommission"
#End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                           sMsg:="m_oAgentCommission.CalculateAgentCommission Failed for " & sInsuranceRef,
                                           vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRenewalPolicy")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
                'add to Renewal_Report table
                If m_lInsuranceFileCnt = 0 Then
#If IN_DEBUG > 0 Then

					m_oDebugTimings.StartTiming "m_oBusiness.AddRenewalReport"
#End If

                    m_lReturn =
                        m_oBusiness.AddRenewalReport(
                            v_sReportType:=
                                                        If(
                                                            nRenewalStatusTypeID =
                                                            gPMConstants.PMBRenewalStatusTypeAutoRated, "AutoRenewal",
                                                            "ManualRenewal"),
                            v_vClientName:=r_vRenewalList(PMFieldPosClientName, v_lCount),
                            v_vPolicyNumber:=r_vRenewalList(PMFieldPosInsuranceRef, v_lCount),
                            v_vAgentCode:=r_vRenewalList(PMFieldPosAgentName, v_lCount),
                            v_vCoverStartDate:=r_vRenewalList(PMFieldPosCoverStartDate, v_lCount),
                            v_vCoverEndDate:=r_vRenewalList(PMFieldPosCoverEndDate, v_lCount),
                            v_vProductCode:=r_vRenewalList(PMFieldPosProductCode, v_lCount), v_vFailureCriterion:="")
#If IN_DEBUG > 0 Then

					m_oDebugTimings.EndTiming "m_oBusiness.AddRenewalReport"
#End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                           sMsg:="m_oBusiness.AddRenewalReport Failed for " & sInsuranceRef,
                                           vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRenewalPolicy")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If
            'Update the status
#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "m_oBusiness.AddRenewalStatus"
#End If


            m_lReturn =
                m_oBusiness.AddRenewalStatus(
                        v_lProductId:=gPMFunctions.NullToLong(CStr(r_vRenewalList(PMFieldPosProductID, v_lCount))),
                        v_lRenewalStatusTypeID:=nRenewalStatusTypeID,
                        v_lInsuranceHolderCnt:=
                                                gPMFunctions.NullToLong(CStr(r_vRenewalList(PMFieldPosInsuranceHolderCnt,
                                                                                            v_lCount))),
                        v_lInsuranceFileCnt:=
                                                gPMFunctions.NullToLong(CStr(r_vRenewalList(PMFieldPosInsuranceFileCnt,
                                                                                            v_lCount))),
                        v_vLeadAgentCnt:=r_vRenewalList(PMFieldPosLeadAgentCnt, v_lCount),
                        v_lRenewalInsuranceFileCnt:=nNewInsuranceFileCnt)
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "m_oBusiness.AddRenewalStatus"
#End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="m_oBusiness.AddRenewalStatus Failed for " & sInsuranceRef, vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="CreateRenewalPolicy")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'If we have errors then create an event for each error and one work manager
            'task for this policy
            If Informations.IsArray(oFailureArray) OrElse sFailureCriterion.ToString().Trim() <> "" Then
                lEligibleForRenewal = gPMConstants.PMEReturnCode.PMFalse
            End If

            If sFailureCriterion.ToString().Trim() <> "" Then
                If Informations.IsArray(oFailureArray) Then

                    nUbound = oFailureArray.GetUpperBound(1) + 1
                    ReDim Preserve oFailureArray(1, nUbound)

                    oFailureArray(1, nUbound) = sFailureCriterion.ToString()
                Else
                    ReDim oFailureArray(1, 0)

                    oFailureArray(0, 0) = sFailureCriterion.ToString()
                End If
            End If

            If lEligibleForRenewal = gPMConstants.PMEReturnCode.PMFalse Then
                sFailureCriterion = New StringBuilder("Renewal - " & sInsuranceRef & " - ")


                nArrayUBound = oFailureArray.GetUpperBound(1) ' RAM20030221 : Code Optimisation
                For iCnt As Integer = 0 To nArrayUBound


                    sFailure = oFailureArray(0, iCnt) +
                               (If(CStr(oFailureArray(1, iCnt)) = "", "", ":" + oFailureArray(1, iCnt)))
                    sFailureCriterion.Append(sFailure & ", ")
#If IN_DEBUG > 0 Then

					m_oDebugTimings.StartTiming "CreateEvent"
#End If
                    m_lReturn = CreateEvent(vPartyCnt:=r_vRenewalList(PMFieldPosInsuranceHolderCnt, v_lCount),
                                            vInsuranceFolderCnt:=
                                               r_vRenewalList(PMFieldPosInsuranceFolderCnt, v_lCount),
                                            vInsuranceFileCnt:=nNewInsuranceFileCnt,
                                            vDescription:="Renewal Failure: " & sFailure)
#If IN_DEBUG > 0 Then

					m_oDebugTimings.EndTiming "CreateEvent"
#End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                           sMsg:="CreateEvent Failed for " & sInsuranceRef, vApp:=ACApp,
                                           vClass:=ACClass, vMethod:="CreateRenewalPolicy")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Next iCnt

                ReDim oKeyArray(1, 1)

                oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "insurance_file_cnt"

                oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = nNewInsuranceFileCnt

                oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameRunMode

                oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = 1

#If IN_DEBUG > 0 Then

				m_oDebugTimings.StartTiming "m_oBusiness.AddTaskToWorkManager"
#End If

                If bCreateWorkManagerTask And v_bMergeRisks = False Then
                    m_lReturn =
                        m_oBusiness.AddTaskToWorkManager(
                                v_sClientName:=
                                                            gPMFunctions.NullToString(
                                                                CStr(r_vRenewalList(PMFieldPosClientName, v_lCount))),
                                v_sDescription:=sFailureCriterion.ToString(),
                                v_dtDueDate:=Informations.DateAdd("ww", 1, DateTime.Today), v_vKeyArray:=CType(oKeyArray, Object))
                End If

#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "m_oBusiness.AddTaskToWorkManager"
#End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="m_oBusiness.AddTaskToWorkManager Failed for " & sInsuranceRef,
                                       vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRenewalPolicy")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If


            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateRenewalPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRenewalPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try
    End Function
    ''' <summary>
    '''  Code changes to make sure we set the original_cover_start_date for the
    '''policy, as we are creating a new renewal policy
    ''' </summary>
    ''' <param name="v_lOldInsuranceFileCnt"></param>
    ''' <param name="v_sInsuranceRef"></param>
    ''' <param name="v_dtRenewalDate"></param>
    ''' <param name="v_dtExpiryDate"></param>
    ''' <param name="r_lNewInsuranceFileCnt"></param>
    ''' <param name="r_dtEffectiveDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateBackOfficePolicy(ByVal v_lOldInsuranceFileCnt As Integer, ByVal v_sInsuranceRef As String,
                                            ByVal v_dtRenewalDate As Date, ByVal v_dtExpiryDate As Date,
                                            ByRef r_lNewInsuranceFileCnt As Object, ByRef r_dtEffectiveDate As Date,
                                            ByVal bisTMPPolicy As Boolean) _
        As Integer

        Dim result As Integer = 0
        Dim lProductId As Integer
        Dim lPolicyVersion As Integer
        Dim iPartyCnt As Integer

        'Start(Saurabh Agrawal) Tech Spec VAL p14 Policy Numbering (5.3.1)
        Dim bChanged As Boolean
        Const kPolicyBusinessType As Integer = 2
        'End(Saurabh Agrawal) Tech Spec VAL p14 Policy Numbering (5.3.1)



        result = gPMConstants.PMEReturnCode.PMTrue

        'assign current InsuranceFileCnt to object
        'Get the details at the same time
#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "m_oInsuranceFile.InsuranceFileCnt"
#End If

        With m_oInsuranceFile ' RAM20030221   : Use of With

            'Suppress the automatic loading of the policy data

            .DataTransfer = 1


            .InsuranceFileCnt = v_lOldInsuranceFileCnt


            m_lReturn = .GetDetailsNoLookup

#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "m_oInsuranceFile.InsuranceFileCnt"
#End If

            'set policy status to Renewal

            .InsuranceFileStatus = "REN"

            'update current policy to status Renewal
#If IN_DEBUG > 0 Then

				m_oDebugTimings.StartTiming "m_oInsuranceFile.UpdatePolicy"
#End If


            m_lReturn = .UpdatePolicy

#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "m_oInsuranceFile.UpdatePolicy"
#End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="bSirInsuranceFile.Services.UpdatePolicy Failed for " & v_sInsuranceRef,
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBackOfficePolicy")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'set policy type to Renewal

            .InsuranceFileType = "RENEWAL"

            'set policy to Live (i.e. status = Null).


            .InsuranceFileStatus = Nothing


            'Start(Saurabh Agrawal) Tech Spec VAL p14 Policy Numbering (5.3.1)

            v_sInsuranceRef = .InsuranceRef

            If m_bCalledFromAutoMTA = False Then
                m_lReturn =
                    m_oPolicyNumMaint.GenerateRenewalPolicyNumber(
                        v_iPolicy_cnt:=gPMFunctions.ToSafeInteger(v_lOldInsuranceFileCnt),
                        v_lBusinessType:=kPolicyBusinessType,
                        v_iBranch:=gPMFunctions.ToSafeInteger(m_oInsuranceFile.SourceID),
                        v_lProductId:=gPMFunctions.ToSafeLong(.ProductID),
                        v_lAgent:=gPMFunctions.ToSafeLong(m_oInsuranceFile.LeadAgentCnt),
                        r_sGeneratedPolicyNumber:=v_sInsuranceRef, r_bChanged:=bChanged)

                iPartyCnt = .InsuredCnt

                m_lReturn = m_oPolicyNumMaint.GenerateRenewalPolicyNumber(v_iPolicy_cnt:=gPMFunctions.ToSafeInteger(v_lOldInsuranceFileCnt), v_lBusinessType:=kPolicyBusinessType, v_iBranch:=gPMFunctions.ToSafeInteger(m_oInsuranceFile.SourceID), v_lProductId:=gPMFunctions.ToSafeLong(.ProductID), v_lAgent:=gPMFunctions.ToSafeLong(m_oInsuranceFile.LeadAgentCnt), r_sGeneratedPolicyNumber:=v_sInsuranceRef, r_bChanged:=bChanged, v_lPartyCnt:=iPartyCnt)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'create new policy of type Renewal
                If bChanged Then

                    .InsuranceRef = v_sInsuranceRef
                End If
            End If
            'End(Saurabh Agrawal) Tech Spec VAL p14 Policy Numbering (5.3.1)


            lProductId = .ProductID

#If IN_DEBUG > 0 Then

				m_oDebugTimings.StartTiming "m_oBusiness.GetMidnightRenewal"
#End If


            .CoverStartDate = v_dtRenewalDate
            r_dtEffectiveDate = v_dtRenewalDate


            .ExpiryDate = v_dtExpiryDate

            'RWH(17/05/2001) Set new Renewal Date.
            If bisTMPPolicy = True Then
                .RenewalDate = Informations.DateAdd("m", 1, v_dtRenewalDate)
            Else
                .RenewalDate = v_dtRenewalDate.AddYears(1)
            End If


            .EventDescription = "Policy Copied To Renewal"

            'sj 11/02/2003 - start
            'PS104
            'Get the next policy version

            m_lReturn = m_oBusiness.GetNextPolicyVersion(v_lInsuranceFileCnt:=v_lOldInsuranceFileCnt,
                                                         r_lPolicyVersion:=lPolicyVersion)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError,
                                   sMsg:="GetNextPolicyVersion Failed", vApp:=ACApp, vClass:=ACClass,
                                   vMethod:="CreateBackOfficePolicy")
                Return result
            End If


            .PolicyVersion = lPolicyVersion

            'sj 11/02/2003 - end

            'create new policy of type Renewal
#If IN_DEBUG > 0 Then

				m_oDebugTimings.StartTiming "m_oInsuranceFile.CreatePolicy"
#End If


            m_lReturn = .CreatePolicy

#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "m_oInsuranceFile.CreatePolicy"
#End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="bSIRInsuranceFile.Services.CreatePolicy failed for " & v_sInsuranceRef,
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBackOfficePolicy")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'get new insurance file cnt


            r_lNewInsuranceFileCnt = .InsuranceFileCnt

        End With

#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "m_oBusiness.CopyPolicyStandardWordings"
#End If

        m_lReturn = m_oBusiness.CopyPolicyStandardWordings(v_lOldInsuranceFileCnt:=v_lOldInsuranceFileCnt,
                                                           v_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt)
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "m_oBusiness.CopyPolicyStandardWordings"
#End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="m_oBusiness.CopyPolicyStandardWordings failed for " & v_sInsuranceRef,
                               vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBackOfficePolicy")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'RWH(14/06/01) Copy coinsurance.
#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "m_oBusiness.CopyCoinsurance"
#End If

        m_lReturn = m_oBusiness.CopyCoinsurance(v_lCurrentInsFileCnt:=v_lOldInsuranceFileCnt,
                                                v_lNewInsFileCnt:=r_lNewInsuranceFileCnt)
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "m_oBusiness.CopyCoinsurance"
#End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="m_oBusiness.CopyCoinsurance failed for " & v_sInsuranceRef, vApp:=ACApp,
                               vClass:=ACClass, vMethod:="CreateBackOfficePolicy")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        If m_bCalledFromAutoMTA = False Then
            'Copy agent commission.
#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "m_oBusiness.CopyAgentCommission"
#End If

            m_lReturn = m_oBusiness.CopyAgentCommission(v_lCurrentInsFileCnt:=v_lOldInsuranceFileCnt,
                                                        v_lNewInsFileCnt:=r_lNewInsuranceFileCnt)
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "m_oBusiness.CopyAgentCommission"
#End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="m_oBusiness.CopyAgentCommission failed for " & v_sInsuranceRef,
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBackOfficePolicy")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Copy insurance file agents.
#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "m_oBusiness.CopyInsuranceFileAgent"
#End If

            m_lReturn = m_oBusiness.CopyInsuranceFileAgent(v_lCurrentInsFileCnt:=v_lOldInsuranceFileCnt,
                                                           v_lNewInsFileCnt:=r_lNewInsuranceFileCnt)
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "m_oBusiness.CopyInsuranceFileAgent"
#End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="m_oBusiness.CopyInsuranceFileAgent failed for " & v_sInsuranceRef,
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBackOfficePolicy")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CopyRiskData
    '
    ' Desc: copy all Risks attached to OldInsuranceFileCnt to NewInsuranceFileCnt
    '          copy all GIS details attached to each risk to NewInsuranceFileCnt
    '
    ' RWH(13/08/01) Ensure whole process is completed even if one stage fails.
    '
    ' ***************************************************************** '
    Private Function CopyRiskData(ByRef r_vRenewalList(,) As Object, ByVal v_lCount As Integer,
                                  ByVal v_lNewInsuranceFileCnt As Integer, ByRef r_lEligibleForRenewal As Integer,
                                  ByRef r_sFailureReason As String, ByRef r_bCreateWorkManagerTask As Boolean,
                                  ByRef v_dtEffectiveDate As Date) As Integer

        Dim result As Integer = 0
        Dim vRiskArray(,) As Object = Nothing
        Dim sInsuranceRef As String

        Dim sDescription As String = ""

        'TN20010719
        Dim lUbound As Integer ' RAM20030221   : Declared this variable
        'Unused Constant
        'Const ACFieldPosRiskID As Integer = 0
        'Const ACFieldPosGisScreenID As Integer = 21


        ' AMB 02/07/2003: 1.9 IAG PS068
        Dim bPolicyRerenewed As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        r_lEligibleForRenewal = gPMConstants.PMEReturnCode.PMTrue


        sInsuranceRef = gPMFunctions.NullToString(CStr(r_vRenewalList(PMFieldPosInsuranceRef, v_lCount)))

        Debug.WriteLine("Copying Risks - Get relevant risks")

        'get all risks associate with OldInsuranceFileCnt
#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "m_oRiskData.GetRisk"
#End If


        m_lReturn =
            m_oRiskData.GetRisk(
                v_lInsuranceFileCnt:=
                                   gPMFunctions.NullToLong(CStr(r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount))),
                r_vResultArray:=vRiskArray)
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "m_oRiskData.GetRisk"
#End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="m_oRiskData.GetRisk Failed for " & sInsuranceRef, vApp:=ACApp,
                               vClass:=ACClass, vMethod:="CopyRiskData")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'do we have any risks
        If Not Informations.IsArray(vRiskArray) Then
            'RWH(16/11/2000) We do not need to report an error if there are no risks.
            r_sFailureReason = "No risks found"
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If


        lUbound = vRiskArray.GetUpperBound(1) ' RAM20030221   : Code Optimisation
        'loop thro and copy each risk details
        For lCount As Integer = 0 To lUbound

            Debug.WriteLine("Copying Risk Data")


            m_lReturn = CopySingleRiskData(r_vRenewalList:=r_vRenewalList,
                                           v_sInsuranceRef:=
                                              gPMFunctions.NullToString(CStr(r_vRenewalList(PMFieldPosInsuranceRef,
                                                                                            v_lCount))),
                                           v_lInsuranceFolderCnt:=
                                              gPMFunctions.NullToLong(
                                                  CStr(r_vRenewalList(PMFieldPosInsuranceFolderCnt, v_lCount))),
                                           v_dtCoverStartDate:=
                                              gPMFunctions.NullToDate(r_vRenewalList(PMFieldPosCoverStartDate,
                                                                                     v_lCount)),
                                           v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                                           v_vRiskArray:=vRiskArray, v_lCount:=v_lCount, v_lRiskCount:=lCount,
                                           r_lEligibleForRenewal:=r_lEligibleForRenewal,
                                           r_bCreateWorkManagerTask:=r_bCreateWorkManagerTask,
                                           v_dtEffectiveDate:=v_dtEffectiveDate,
                                           v_bPolicyReRenewed:=bPolicyRerenewed)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                   sMsg:="CopySingleRiskData Failed for " & sInsuranceRef, vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="CopyRiskData")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Next lCount

        Return result

    End Function

    ''' <summary>
    ''' CopySingleRiskData
    ''' </summary>
    ''' <param name="r_vRenewalList"></param>
    ''' <param name="v_sInsuranceRef"></param>
    ''' <param name="v_lInsuranceFolderCnt"></param>
    ''' <param name="v_lNewInsuranceFileCnt"></param>
    ''' <param name="v_dtCoverStartDate"></param>
    ''' <param name="v_vRiskArray"></param>
    ''' <param name="v_lCount"></param>
    ''' <param name="v_lRiskCount"></param>
    ''' <param name="r_lEligibleForRenewal"></param>
    ''' <param name="r_bCreateWorkManagerTask"></param>
    ''' <param name="v_dtEffectiveDate"></param>
    ''' <param name="v_bMergeRisks"></param>
    ''' <param name="r_lNewRiskCnt"></param>
    ''' <param name="v_bPolicyReRenewed"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CopySingleRiskData(ByRef r_vRenewalList As Object, ByVal v_sInsuranceRef As String,
                                        ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer,
                                        ByVal v_dtCoverStartDate As Date, ByVal v_vRiskArray(,) As Object,
                                        ByVal v_lCount As Integer, ByVal v_lRiskCount As Integer,
                                        ByRef r_lEligibleForRenewal As Integer,
                                        ByRef r_bCreateWorkManagerTask As Boolean, ByVal v_dtEffectiveDate As Date,
                                        Optional ByVal v_bMergeRisks As Boolean = False,
                                        Optional ByRef r_lNewRiskCnt As Integer = 0,
                                        Optional ByVal v_bPolicyReRenewed As Boolean = False) As Integer

        ' AMB 07/07/2003: 1.9 IAG PS068 Date Effective Rating - v_bPolicyRerenewed param added

        Dim result As Integer = 0
        Const ACFieldPosGisScreenID As Integer = 21

        Dim lNewRiskCnt As Integer
        Dim vGisPolicyLinkArray(,) As Object = Nothing
        Dim sDataModelCode As String = ""
        Dim lOldGisPolicyLinkId As String = ""
        Dim sXMLDataSetDef As String = ""
        Dim sXMLDataSet As String = ""
        Dim lNewPolicyBinderId, lOldPolicyBinderId, lOldRiskId, lNewGisPolicyLinkID, vGisScreenId As Integer
        Dim sFailureDetail As String = ""
        Dim bApplyReinsurance As Object
        Dim vRiskTax As Object = Nothing
        Dim sDescription As String = ""
        Dim bScriptError As Boolean

        ' AMB 02/07/2003: 1.9 IAG PS068
        Dim vRatingSectionArray As Object = Nothing
        Dim cTotalAnnualTax As Decimal
        'Unused Constant
        'Const klRiskFolderCol As Integer = 2
        'Const klIFPRLInsFileCntCol As Integer = 0 ' IFPRL = insurance_file_persistent_risk_link
        'Const klIFPRLRiskCntCol As Integer = 1
        ' Const klIFPRLRiskFolderCntCol As Integer = 6




        result = gPMConstants.PMEReturnCode.PMTrue

        bScriptError = False

        Dim lReinsPremiumOrSumInsured, lReinsBand As Integer
        Dim bIsRIValid As Boolean

        'WPR 75 Added
        If m_oAutoMTAMerge Is Nothing = False Then
            If m_oAutoMTAMerge.MergeStatus = "M" And m_oAutoMTAMerge.PostChangeRiskCnt > 0 Then
                v_vRiskArray(ACRiskPosCnt, v_lRiskCount) = m_oAutoMTAMerge.PostChangeRiskCnt
            End If
            'WPR 75 End
        End If
        lOldRiskId = CInt(v_vRiskArray(ACRiskPosCnt, v_lRiskCount))

        vGisScreenId = CInt(v_vRiskArray(ACFieldPosGisScreenID, v_lRiskCount))

        If m_bCalledFromAutoMTA = True And v_bMergeRisks = True Then
            If m_oAutoMTAMerge.MergeStatus = "A" Then
                'skip adding risk for now as it should happen as part of policy version editing
                Return gPMConstants.PMEReturnCode.PMTrue
            Else
                m_lReturn = m_oRiskData.AddRiskLink(
                                        v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                                        v_lRiskCnt:=v_vRiskArray(ACRiskPosCnt, v_lRiskCount),
                                        v_sStatusFlag:="R")

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                       sMsg:="m_oRiskData.AddRiskLink Failed for " & v_sInsuranceRef, vApp:=ACApp,
                                       vClass:=ACClass, vMethod:="CopySingleRiskData")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' remaining processing will done as part of editing of quote
                Return gPMConstants.PMEReturnCode.PMTrue
            End If
        End If
        ' ***************************************************************** '
        'copy risk to NewInsuranceFileCnt
        ' ***************************************************************** '
#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "m_oRiskData.CopyRisk"
#End If

        ' RAW 15/11/2004 : Pricing Changes : added lTransactionType param

        m_lReturn = m_oRiskData.CopyRisk(v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                                         v_vRiskDetail:=v_vRiskArray, v_lPosNo:=v_lRiskCount,
                                         r_lRiskCnt:=lNewRiskCnt,
                                         v_lResetStatus:=gPMConstants.PMEReturnCode.PMTrue)


#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "m_oRiskData.CopyRisk"
#End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="m_oRiskData.CopyRisk Failed for " & v_sInsuranceRef, vApp:=ACApp,
                               vClass:=ACClass, vMethod:="CopySingleRiskData")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        r_lNewRiskCnt = lNewRiskCnt
        m_oDatabase.Parameters.Clear()
        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lNewInsuranceFileCnt,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=r_lNewRiskCnt,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
        m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_Update_IFRLink_Risk_Edited",
                                          sSQLName:="UpdateIFRLinkRiskEdited", bStoredProcedure:=True)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If


        'prepare details to copy GIS Stuff attached to current risk

        ' ***************************************************************** '
        'get the old gis policy link id
        ' ***************************************************************** '
        'RWH(20/11/2000) Pass folder_cnt instead of file_cnt.
#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "m_oRiskData.GetGISPolicyLink"
#End If

        m_lReturn = m_oRiskData.GetGISPolicyLink(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt,
                                                 v_lRiskID:=lOldRiskId, r_vResultArray:=vGisPolicyLinkArray)
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "m_oRiskData.GetGISPolicyLink"
#End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="m_oRiskData.GetGISPolicyLink Failed for " & v_sInsuranceRef, vApp:=ACApp,
                               vClass:=ACClass, vMethod:="CopySingleRiskData")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'do we have any data
        Dim auxVar As Object = vGisPolicyLinkArray(0, 0)


        If Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar) Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="m_oRiskData.GetGISPolicyLink Failed for " & v_sInsuranceRef, vApp:=ACApp,
                               vClass:=ACClass, vMethod:="CopySingleRiskData")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        sDataModelCode = CStr(vGisPolicyLinkArray(4, 0)).Trim()

        lOldGisPolicyLinkId = CStr(vGisPolicyLinkArray(0, 0))

        ' ***************************************************************** '
        'Copy the gis data creating a new risk
        ' ***************************************************************** '
        'RWH(20/11/2000) REMEMBER we are storing folder_cnt in file_cnt field now !!!!!
        'So we pass existing folder_cnt in for old and new file_cnt.
#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "m_oBusiness.CopyDataSet"
#End If


        m_lReturn = m_oBusiness.CopyDataSet(v_sDataModelCode:=sDataModelCode,
                                            r_lNewGISPolicyLinkId:=lNewGisPolicyLinkID,
                                            r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet,
                                            v_vOldGISPolicyLinkId:=lOldGisPolicyLinkId,
                                            v_vOldInsuranceFileCnt:=v_lInsuranceFolderCnt,
                                            v_vOldRiskID:=lOldRiskId,
                                            v_vNewInsuranceFileCnt:=v_lInsuranceFolderCnt,
                                            v_vNewRiskID:=lNewRiskCnt)
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "m_oBusiness.CopyDataSet"
#End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="m_oBusiness.CopyDataSet Failed for " & v_sInsuranceRef, vApp:=ACApp,
                               vClass:=ACClass, vMethod:="CopySingleRiskData")
            Return gPMConstants.PMEReturnCode.PMFalse

        End If

        ' ***************************************************************** '
        'Load the new copied data back into the dataset object
        ' ***************************************************************** '
        ' Initialise the Data Set with the Object/Properties
#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "m_oBusiness.LoadFromXML"
#End If

        m_lReturn = m_oBusiness.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "m_oBusiness.LoadFromXML"
#End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="m_oBusiness.LoadFromXML Failed for " & v_sInsuranceRef, vApp:=ACApp,
                               vClass:=ACClass, vMethod:="CopySingleRiskData")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' ***************************************************************** '
        'Save the output back to the database
        '' CQ2623
        ' ***************************************************************** '
#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "m_oBusiness.GIS_SaveToDB"
#End If

        m_lReturn = m_oBusiness.GIS_SaveToDB(v_sGisDataModelCode:=sDataModelCode)
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "m_oBusiness.GIS_SaveToDB"
#End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="m_oBusiness.GIS_SaveToDB Failed for " & v_sInsuranceRef, vApp:=ACApp,
                               vClass:=ACClass, vMethod:="CopySingleRiskData")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'WPR 75 Added
        lNewPolicyBinderId = lNewGisPolicyLinkID
        lOldPolicyBinderId = lOldGisPolicyLinkId
        'WPR 75 End
        ' ***************************************************************** '
        ' If we are reapplying the renewal after a backdated MTA then merge
        ' in any changes
        ' ***************************************************************** '
        'WPR 75 Added Commented as per WPR
        'If v_bMergeRisks Then
        '    m_oAutoMTAMerge.InsuranceFolderCnt = v_lInsuranceFolderCnt
        '    m_oAutoMTAMerge.NewRiskCnt = lNewRiskCnt
        '    m_oAutoMTAMerge.DataModelCode = sDataModelCode

        '    m_lReturn = m_oAutoMTAMerge.MergeExistingMTAChanges()
        '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '        result = gPMConstants.PMEReturnCode.PMFalse
        '        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="m_oAutoMTAMerge.MergeExistingMTAChanges Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopySingleRiskData")
        '        Return result
        '    End If

        '    ' HG201103 - Return Back XML Dataset and Reload
        '    sXMLDataSetDef = m_oAutoMTAMerge.XMLDataSetDef
        '    sXMLDataSet = m_oAutoMTAMerge.XMLDataSet

        '    m_lReturn = m_oBusiness.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
        'WPR 75 END
#If IN_DEBUG > 0 Then
             'WPR 75 Added Commented as per WPR
				'm_oDebugTimings.EndTiming "m_oBusiness.LoadFromXML"
             m_oDebugTimings.StartTiming "m_oBusiness.GIS_NBQuote"
            'WPR 75 END
#End If
        m_lReturn = m_oBusiness.DeleteOutputTable(v_sDataModelCode:=sDataModelCode.Trim,
                                                  v_lPolicyBinderId:=lNewPolicyBinderId)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'WPR 75 Added Commented as per WPR
            'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oBusiness.LoadFromXML Failed for " & v_sInsuranceRef, vApp:=ACApp, vClass:=ACClass, vMethod:="CopySingleRiskData")
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="m_oBusiness.DeleteOutputTable" & v_sInsuranceRef, vApp:=ACApp,
                               vClass:=ACClass, vMethod:="CopySingleRiskData")
            'WPR 75 END
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'WPR 75 Added Commented as per WPR
        ' End If


        '            lNewPolicyBinderId = lNewGisPolicyLinkID
        '            lOldPolicyBinderId = CInt(lOldGisPolicyLinkId)


        '            ' ***************************************************************** '
        '            ' Run renewal script ( quote type = 5)
        '            ' ***************************************************************** '
        '#If IN_DEBUG > 0 Then

        '			m_oDebugTimings.StartTiming "m_oBusiness.GIS_NBQuote"
        '#End If
        'WPR 75 END

        m_lReturn = m_oBusiness.GIS_NBQuote(v_sGisDataModelCode:=sDataModelCode, v_lQuoteType:=5,
                                            r_sXMLDataSet:=sXMLDataSet, r_sXMLDataSetDef:=sXMLDataSetDef)
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "m_oBusiness.GIS_NBQuote"
#End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="m_oBusiness.GIS_NBQuote Failed for " & v_sInsuranceRef, vApp:=ACApp,
                               vClass:=ACClass, vMethod:="CopySingleRiskData")
            'bScriptError = True

            ' HG070903 - Abort renewal if renewal script fails
            ' This will force a rollback (IAG Requirements)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' ***************************************************************** '
        'Save the output back to the database
        ' ***************************************************************** '
#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "m_oBusiness.GIS_SaveToDB"
#End If

        m_lReturn = m_oBusiness.GIS_SaveToDB(v_sGisDataModelCode:=sDataModelCode)
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "m_oBusiness.GIS_SaveToDB"
#End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="m_oBusiness.GIS_SaveToDB Failed for " & v_sInsuranceRef, vApp:=ACApp,
                               vClass:=ACClass, vMethod:="CopySingleRiskData")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'TN20010711 - end

        ' ***************************************************************** '
        ' SMJB - CQ1508 26/06/2003 -
        ' Check output table to find out if risk was declined or referred
        ' ***************************************************************** '
#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "m_oBusiness.CheckOutputTable"
#End If

        m_lReturn = m_oBusiness.CheckOutputTable(v_sDataModelCode:=sDataModelCode,
                                                 v_lPolicyBinderId:=lNewPolicyBinderId,
                                                 r_sReasons:=sFailureDetail)
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "m_oBusiness.CheckOutputTable"
#End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="m_oBusiness.CheckOutputTable Failed for " & v_sInsuranceRef, vApp:=ACApp,
                               vClass:=ACClass, vMethod:="CopySingleRiskData")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' ***************************************************************** '
        ' Copy the standard wordings table from the old to the new risk
        ' ***************************************************************** '
        Debug.WriteLine("Copying Risk Data - Standard Wordings")
#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "m_oBusiness.CopyRiskStandardWordings"
#End If

        m_lReturn = m_oBusiness.CopyRiskStandardWordings(v_lOldPolicyBinderId:=lOldPolicyBinderId,
                                                         v_lNewPolicyBinderId:=lNewPolicyBinderId,
                                                         v_sDataModelCode:=sDataModelCode)
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "m_oBusiness.CopyRiskStandardWordings"
#End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="m_oBusiness.CopyRiskStandardWordings Failed for " & v_sInsuranceRef,
                               vApp:=ACApp, vClass:=ACClass, vMethod:="CopySingleRiskData")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Debug.WriteLine("Copying risk data - Index linking")

        ' ***************************************************************** '
        'Apply index linking to all the gis properties which require it
        ' ***************************************************************** '

        If Not (Convert.IsDBNull(vGisScreenId) Or Informations.IsNothing(vGisScreenId)) Then
            'index link GIS
#If IN_DEBUG > 0 Then

				m_oDebugTimings.StartTiming "m_oBusiness.GisIndexLink"
#End If

            m_lReturn = m_oBusiness.GisIndexLink(v_lInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                                                 v_lRiskID:=lOldRiskId, v_vGisScreenID:=vGisScreenId,
                                                 v_dtEffectiveDate:=v_dtCoverStartDate.AddYears(1),
                                                 v_sGisDataModelCode:=sDataModelCode)
#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "m_oBusiness.GisIndexLink"
#End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                   sMsg:="m_oBusiness.GisIndexLink Failed for " & v_sInsuranceRef, vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="CopySingleRiskData")
                Return gPMConstants.PMEReturnCode.PMFalse

            End If
        End If

        ' ***************************************************************** '
        'Copy extra risk details for RSA ONLY !
        ' ***************************************************************** '
        'SET 10/06/2002 - check to see if this option is in use
        If m_bExtraRiskDetails Then
            Debug.WriteLine("Copying risk data - Sum insured")
            'copy RSA_Sum_Insured
#If IN_DEBUG > 0 Then

				m_oDebugTimings.StartTiming "m_oRiskData.CopyRSASumInsured"
#End If

            m_lReturn = m_oRiskData.CopyRSASumInsured(v_lOldPolicyLinkID:=lOldGisPolicyLinkId,
                                                      v_lNewPolicyLinkID:=lNewGisPolicyLinkID)
#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "m_oRiskData.CopyRSASumInsured"
#End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                   sMsg:="m_oRiskData.CopyRSASumInsured Failed for " & v_sInsuranceRef,
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="CopySingleRiskData")
                Return gPMConstants.PMEReturnCode.PMFalse

            End If
        End If

        ' ***************************************************************** '
        ' Run Underwriting Authority Limits script ( quote type = 3)
        ' SMJB - CQ1508 26/06/2003 -
        ' Only run UA script if risk has not been referred or declined already
        ' ***************************************************************** '
        If sFailureDetail = "" Then
#If IN_DEBUG > 0 Then

				m_oDebugTimings.StartTiming "m_oBusiness.GIS_NBQuote"
#End If
            'WPR 75 Added
            m_lReturn = m_oBusiness.DeleteOutputTable(v_sDataModelCode:=Trim(sDataModelCode),
                                                      v_lPolicyBinderId:=lNewPolicyBinderId)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                bPMFunc.LogMessage(m_sUsername,
                                   iType:=gPMConstants.PMELogLevel.PMLogError,
                                   sMsg:="m_oBusiness.DeleteOutputTable Failed for " & v_sInsuranceRef,
                                   vApp:=ACApp,
                                   vClass:=ACClass,
                                   vMethod:="CopySingleRiskData")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'WPR 75 End

            m_lReturn = m_oBusiness.GIS_NBQuote(v_sGisDataModelCode:=sDataModelCode, v_lQuoteType:=2,
                                                r_sXMLDataSet:=sXMLDataSet, r_sXMLDataSetDef:=sXMLDataSetDef)
#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "m_oBusiness.GIS_NBQuote"
#End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                   sMsg:="m_oBusiness.GIS_NBQuote Failed for " & v_sInsuranceRef, vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="CopySingleRiskData")
                bScriptError = True
            End If
        End If


        ' ***************************************************************** '
        ' Added SMJB 27/06/2003 CQ1508 -
        ' Save the output back to the database
        ' Must save to DB before we can tell if it was declined or referred
        ' Only save if not already declined or referred
        ' ***************************************************************** '
        If sFailureDetail = "" Then
#If IN_DEBUG > 0 Then

				m_oDebugTimings.StartTiming "m_oBusiness.GIS_SaveToDB"
#End If

            m_lReturn = m_oBusiness.GIS_SaveToDB(v_sGisDataModelCode:=sDataModelCode)
#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "m_oBusiness.GIS_SaveToDB"
#End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                   sMsg:="m_oBusiness.GIS_SaveToDB Failed for " & v_sInsuranceRef, vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="CopySingleRiskData")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' ***************************************************************** '
        ' Added SMJB CQ1508 26/06/2003 -
        ' Check output table to find out if risk was declined or referred
        ' (Only check if it hasn't already been declined or referred)
        ' ***************************************************************** '
        If sFailureDetail = "" Then
#If IN_DEBUG > 0 Then

				m_oDebugTimings.StartTiming "m_oBusiness.CheckOutputTable"
#End If

            m_lReturn = m_oBusiness.CheckOutputTable(v_sDataModelCode:=sDataModelCode,
                                                     v_lPolicyBinderId:=lNewPolicyBinderId,
                                                     r_sReasons:=sFailureDetail)
#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "m_oBusiness.CheckOutputTable"
#End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                   sMsg:="m_oBusiness.CheckOutputTable Failed for " & v_sInsuranceRef,
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="CopySingleRiskData")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        ' ***************************************************************** '
        ' Run the standard rating rules quote ( quote type = 1)
        ' SMJB CQ1508 26/06/2003 -
        ' Only run rating script if risk has not been referred or declined already
        ' ***************************************************************** '
        If sFailureDetail = "" Then
#If IN_DEBUG > 0 Then

				m_oDebugTimings.StartTiming "m_oBusiness.GIS_NBQuote"
#End If
            'WPR 75 Added
            m_lReturn = m_oBusiness.DeleteOutputTable(v_sDataModelCode:=Trim$(sDataModelCode),
                                                      v_lPolicyBinderId:=lNewPolicyBinderId)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                bPMFunc.LogMessage(m_sUsername,
                                   iType:=gPMConstants.PMELogLevel.PMLogError,
                                   sMsg:="m_oBusiness.DeleteOutputTable Failed for " & v_sInsuranceRef,
                                   vApp:=ACApp,
                                   vClass:=ACClass,
                                   vMethod:="CopySingleRiskData")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'WPR 75 End
            ' RAW 15/11/2004 : Pricing Changes : added r_vAdditionalDataArray param

            m_lReturn = m_oBusiness.GIS_NBQuote(v_sGisDataModelCode:=sDataModelCode, v_lQuoteType:=1,
                                                r_sXMLDataSet:=sXMLDataSet, r_sXMLDataSetDef:=sXMLDataSetDef)
#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "m_oBusiness.GIS_NBQuote"
#End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                   sMsg:="m_oBusiness.GIS_NBQuote Failed for " & v_sInsuranceRef, vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="CopySingleRiskData")
                bScriptError = True
            End If
        End If
#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "m_oBusiness.GIS_SaveToDB"
#End If

        If bScriptError Then
            'There has been an error in at least one of the scripts so exit out here
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '***************************************************************** '
        'Save the output back to the database
        ' Only save if not already declined or referred
        ' ***************************************************************** '
        If sFailureDetail = "" Then

            m_lReturn = m_oBusiness.GIS_SaveToDB(v_sGisDataModelCode:=sDataModelCode)
#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "m_oBusiness.GIS_SaveToDB"
#End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                   sMsg:="m_oBusiness.GIS_SaveToDB Failed for " & v_sInsuranceRef, vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="CopySingleRiskData")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' ***************************************************************** '
        'Check the output table to see if the risk has declined or referred
        ' ***************************************************************** '
        ' sFailureDetail = "" - Removed - SMJB CQ1508 26/06/2003 -
        ' need to keep this value from previous tests
        ' Only check if it hasn't already been declined or referred
        If sFailureDetail = "" Then
#If IN_DEBUG > 0 Then

				m_oDebugTimings.StartTiming "m_oBusiness.CheckOutputTable"
#End If

            m_lReturn = m_oBusiness.CheckOutputTable(v_sDataModelCode:=sDataModelCode,
                                                     v_lPolicyBinderId:=lNewPolicyBinderId,
                                                     r_sReasons:=sFailureDetail)
#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "m_oBusiness.CheckOutputTable"
#End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                   sMsg:="m_oBusiness.CheckOutputTable Failed for " & v_sInsuranceRef,
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="CopySingleRiskData")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' HG250604 CQ357 If a failure reason exists, then the policy has been declined\referred
        ' and hence we assume that the customers products have delt with raising the work manager
        ' task.
        If sFailureDetail <> "" Then
            r_bCreateWorkManagerTask = False
        End If

        ' ***************************************************************** '
        'Only do Peril Allocation if rating passed.
        ' ***************************************************************** '
        If sFailureDetail = "" Then
            'PerilAllocation - Rating Sections

            Debug.WriteLine("Copying risk data - Peril allocation")

            With m_oPerilAllocation ' RAM20030221   : Use of With

                'Set required params for PerilAllocation

                .InsuranceFileCnt = v_lNewInsuranceFileCnt

                .InsuranceFolderCnt = v_lInsuranceFolderCnt

                .RiskId = lNewRiskCnt

                'RWH(24/08/01) Change put in for Tom.

                .TransactionType = "REN"

                'Do PerilAllocation/Rating Sections stuff.
#If IN_DEBUG > 0 Then

					m_oDebugTimings.StartTiming "m_oPerilAllocation.PopulateRatingSections"
#End If

                ' RAW 05/05/2004 : CQ753 : added r_vResultArray param

                m_lReturn = .PopulateRatingSections(r_vResultArray:=vRatingSectionArray)

#If IN_DEBUG > 0 Then

					m_oDebugTimings.EndTiming "m_oPerilAllocation.PopulateRatingSections"
#End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                       sMsg:=
                                          "m_oPerilAllocation.PopulateRatingSections Failed for " & v_sInsuranceRef,
                                       vApp:=ACApp, vClass:=ACClass, vMethod:="CopySingleRiskData")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                ' RAW 05/05/2004 : CQ753 : added
                ' only call this to get tax

                m_lReturn = .RecalculatePremium(r_vRatingSection:=vRatingSectionArray,
                                                r_cTotalAnnualTax:=cTotalAnnualTax)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="m_oPerilAllocation.RecalculatePremium Failed", vApp:=ACApp,
                                       vClass:=ACClass, vMethod:="CopySingleRiskData")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                .AnnualTaxTotal = cTotalAnnualTax
                ' RAW 05/05/2004 : CQ753 : end


                'RWH(24/08/01) Change put in for Tom.
                'Update the risk premium
#If IN_DEBUG > 0 Then

					m_oDebugTimings.StartTiming "m_oPerilAllocation.UpdateRisk"
#End If


                m_lReturn = .UpdateRisk

#If IN_DEBUG > 0 Then

					m_oDebugTimings.EndTiming "m_oPerilAllocation.UpdateRisk"
#End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                       sMsg:="m_oPerilAllocation.UpdateRisk Failed for " & v_sInsuranceRef,
                                       vApp:=ACApp, vClass:=ACClass, vMethod:="CopySingleRiskData")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

        End If

        ' ***************************************************************** '
        'Check if we need to apply reinsurance
        ' ***************************************************************** '
        'RWH(13/08/01) Moved the reinsurance part from within condition above.
        'Reinsurance should be copied whether rating has succeeded or not.
        Debug.WriteLine("Copying risk data - Risk reinsurance")

        With m_oReinsurance ' RAM20030221   : Use of With

            .SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            .InsuranceFileCnt = v_lNewInsuranceFileCnt

            .RiskId = lNewRiskCnt
            'WPR 75 Added
            If m_bCalledFromAutoMTA Then
                ' put the FAC back on new renewal version
                If m_bRI2007Enabled = "1" Then
                    .CopyFACRiskCnt = lOldRiskId
                End If
            End If
            'WPR 75 End
#If IN_DEBUG > 0 Then

				m_oDebugTimings.StartTiming "m_oReinsurance.ApplyReinsurance"
#End If


            m_lReturn = .ApplyReinsurance(r_bApplyReinsurance:=bApplyReinsurance)

#If IN_DEBUG > 0 Then

				m_oDebugTimings.EndTiming "m_oReinsurance.ApplyReinsurance"
#End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                   sMsg:="m_oReinsurance.ApplyReinsurance Failed for " & v_sInsuranceRef,
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="CopySingleRiskData")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' ***************************************************************** '
            'Apply reinsurance if required
            ' ***************************************************************** '
            If bApplyReinsurance Then
#If IN_DEBUG > 0 Then

					m_oDebugTimings.StartTiming "m_oReinsurance.CalculateRI"
#End If


                m_lReturn = .CalculateRI

#If IN_DEBUG > 0 Then

					m_oDebugTimings.EndTiming "m_oReinsurance.CalculateRI"
#End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                       sMsg:="m_oReinsurance.CalculateRI Failed for " & v_sInsuranceRef,
                                       vApp:=ACApp, vClass:=ACClass, vMethod:="CopySingleRiskData")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

#If IN_DEBUG > 0 Then

					m_oDebugTimings.StartTiming "m_oReinsurance.Getdetails"
#End If


                m_lReturn = .Getdetails

#If IN_DEBUG > 0 Then

					m_oDebugTimings.EndTiming "m_oReinsurance.Getdetails"
#End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue AndAlso m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                       sMsg:="m_oReinsurance.CalculateRI Failed for " & v_sInsuranceRef,
                                       vApp:=ACApp, vClass:=ACClass, vMethod:="CopySingleRiskData")
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

#If IN_DEBUG > 0 Then

					m_oDebugTimings.StartTiming "m_oReinsurance.Update"
#End If


                m_lReturn = .Update

#If IN_DEBUG > 0 Then

					m_oDebugTimings.EndTiming "m_oReinsurance.Update"
#End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                       sMsg:="m_oReinsurance.Update Failed for " & v_sInsuranceRef, vApp:=ACApp,
                                       vClass:=ACClass, vMethod:="CopySingleRiskData")
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

#If IN_DEBUG > 0 Then

					m_oDebugTimings.StartTiming "m_oReinsurance.ValidateBands"
#End If


                m_lReturn = .ValidateBands(ToSafeInteger(lReinsPremiumOrSumInsured), ToSafeInteger(lReinsBand))
                '
#If IN_DEBUG > 0 Then

					m_oDebugTimings.EndTiming "m_oReinsurance.ValidateBands"
#End If
                '             Must return true AND a zero for lReinsPremiumOrSumInsured
                bIsRIValid = (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) And (lReinsPremiumOrSumInsured = 0)

                If Not bIsRIValid Then
                    m_sFailureReason = PMIsQuotedDesc
                End If

            End If
        End With


        ' ***************************************************************** '
        'Process Risk Tax
        ' ***************************************************************** '
        m_oTax.InsuranceFileCnt = v_lNewInsuranceFileCnt
        m_oTax.RiskCnt = lNewRiskCnt
#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "m_oTax.GetRiskTax"
#End If

        m_lReturn = m_oTax.GetRiskTax(CType(vRiskTax, Object), sDescription)
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "m_oTax.GetRiskTax"
#End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="m_oTax.GetRiskTax Failed for " & v_sInsuranceRef, vApp:=ACApp,
                               vClass:=ACClass, vMethod:="CopySingleRiskData")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' ***************************************************************** '
        'set risk status to QUOTED if reinsurance is complete, Unquoted otherwise
        ' ***************************************************************** '
#If IN_DEBUG > 0 Then

			m_oDebugTimings.StartTiming "m_oRiskData.UpdateRiskStatus"
#End If


        m_lReturn = m_oRiskData.UpdateRiskStatus(v_lRiskCnt:=lNewRiskCnt, v_lRiskStatusID:=If(bIsRIValid, 3, 4))
#If IN_DEBUG > 0 Then

			m_oDebugTimings.EndTiming "m_oRiskData.UpdateRiskStatus"
#End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="m_oRiskData.UpdateRiskStatus Failed for " & v_sInsuranceRef, vApp:=ACApp,
                               vClass:=ACClass, vMethod:="CopySingleRiskData")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' ***************************************************************** '
        ' If the risk has failed to quote (i.e. the rules script declined or referred)
        ' ***************************************************************** '
        If (sFailureDetail <> "") And (r_lEligibleForRenewal = gPMConstants.PMEReturnCode.PMTrue) Then
            'The rating script called by the Gis has failed
            r_lEligibleForRenewal = gPMConstants.PMEReturnCode.PMFalse
            If m_lInsuranceFileCnt = 0 Then

                m_lReturn = m_oBusiness.AddRenewalReport(v_sReportType:="ManualRenewal",
                                                         v_vClientName:=
                                                            r_vRenewalList(PMFieldPosClientName, v_lCount),
                                                         v_vPolicyNumber:=v_sInsuranceRef,
                                                         v_vAgentCode:=
                                                            r_vRenewalList(PMFieldPosAgentName, v_lCount),
                                                         v_vCoverStartDate:=
                                                            r_vRenewalList(PMFieldPosCoverStartDate, v_lCount),
                                                         v_vCoverEndDate:=
                                                            r_vRenewalList(PMFieldPosCoverEndDate, v_lCount),
                                                         v_vProductCode:=
                                                            r_vRenewalList(PMFieldPosProductCode, v_lCount),
                                                         v_vFailureCriterion:=PMFailedReRateDesc,
                                                         v_vFailureDetail:=sFailureDetail)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                       sMsg:="m_oBusiness.AddRenewalReport Failed for " & v_sInsuranceRef,
                                       vApp:=ACApp, vClass:=ACClass, vMethod:="CopySingleRiskData")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'sj 21/01/2003 - start
                'PS104
            Else
                m_sFailureReason = PMFailedReRateDesc
            End If
            'sj 21/01/2003 - end

        End If

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: UsesExtraRiskData
    '
    ' Description: Reads registry value for 'ExtraRiskDetails' in
    '              'HKEY_LOCAL_MACHINE\SOFTWARE\PM\SiriusSolutions\Server'
    '              and sets m_bExtraRiskDetails to true if this value is 1.
    '              This is used to decide whether to execute RSA specific code.
    '
    ' Hist : SET 10/06/2002 - function created
    '
    ' ***************************************************************** '
    Private Function UseExtraRiskData() As Integer
        Dim result As Integer = 0
        Dim sFile As String = ""
        Dim m_lReturn As gPMConstants.PMEReturnCode

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily



        ' set default values
        m_bExtraRiskDetails = False
        result = gPMConstants.PMEReturnCode.PMTrue
        eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
        eProductFamily = g_sProductFamily
        eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLServer

        m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot,
                                                       v_lPMEProductFamily:=eProductFamily,
                                                       v_lPMERegSettingLevel:=eRegSettingLevel,
                                                       v_sSettingName:="ExtraRiskData", r_sSettingValue:=sFile),
                          gPMConstants.PMEReturnCode)

        'if we have a valid return
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            If Val(sFile) = 1 Then
                m_bExtraRiskDetails = True
            End If
        End If

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: CreateBusinessObject
    '
    ' Description: create required business objects to run renewal
    '
    ' History: 24/07/2001 TN - Created.
    '
    ' ***************************************************************** '
    Private Function CreateBusinessObject() As Integer

        Dim result As Integer = 0
        Dim sObjName As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        'Get bPMLock

        m_oInsuranceFile = New bSIRInsuranceFile.Services
        m_lReturn = m_oInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID,
                                         iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID,
                                         iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel,
                                         sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = GetProductOptions(lOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007,
                                      r_vValue:=m_bRI2007Enabled)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        Else
            If m_bRI2007Enabled <> "1" Then
                m_lReturn = InitialiseBusinessObject(r_oObject:=m_oReinsurance, v_sClassName:="bSIRReinsurance.Form")
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            ElseIf m_bRI2007Enabled = "1" Then
                m_lReturn = InitialiseBusinessObject(r_oObject:=m_oReinsurance,
                                                     v_sClassName:="bSIRReinsuranceRI2007.Form")
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        m_oTax = New bSIRRITax.Business
        m_lReturn = m_oTax.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID,
                                         iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID,
                                         iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel,
                                         sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_oRiskData = New bSIRRiskData.Business
        m_lReturn = m_oRiskData.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID,
                                         iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID,
                                         iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel,
                                         sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_oPerilAllocation = New bSirPerilAllocation.Business
        m_lReturn = m_oPerilAllocation.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID,
                                         iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID,
                                         iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel,
                                         sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        m_oAgentCommission = New bSirAgentCommission.Business
        m_lReturn = m_oAgentCommission.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID,
                                         iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID,
                                         iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel,
                                         sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        m_oChangePolicyStatus = New bSIRChangePolicyStatus.Business
        m_lReturn = m_oChangePolicyStatus.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID,
                                         iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID,
                                         iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel,
                                         sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        'm_oReport = New bSIRReportPrint.Business()
        'm_lReturn = m_oReport.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
        '    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="bSIRReportPrint.Business.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
        '    Return gPMConstants.PMEReturnCode.PMFalse
        'End If




        m_oEvent = New bSIREvent.Business
        m_lReturn = m_oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID,
                                         iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID,
                                         iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel,
                                         sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        sObjName = "bSIRDocManagerWrapper.Interface_Renamed"
        Dim FullName As Object = CType(System.Reflection.Assembly.GetAssembly(
                    Type.GetType(sObjName + "," + sObjName.Substring(0, sObjName.LastIndexOf(".")))).FullName, Object)
        m_oDocManagerWrapper =
            Activator.CreateInstance(
                FullName, CType(sObjName, Object)).Unwrap()

        m_lReturn = m_oDocManagerWrapper.InitialiseBusiness(ToSafeString(m_sUsername), ToSafeString(m_sPassword), ToSafeInteger(m_iUserID), ToSafeInteger(m_iSourceID),
                                                            ToSafeInteger(m_iLanguageID), ToSafeInteger(m_iCurrencyID), ToSafeInteger(m_iLogLevel), ToSafeString(m_sCallingAppName),
                                                            CType(m_oDatabase, dPMDAO.Database))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError("CreateBusinessObject", "DocManagerWrapper.InitialiseBusiness Failed.")
        End If

        m_oPolicyNumMaint = New bSIRPolicyNumMaint.Business
        m_lReturn = m_oPolicyNumMaint.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID,
                                         iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID,
                                         iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel,
                                         sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_oDataset = New cGISDataSetControl.Application()

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: InitialiseBusinessObject
    '
    ' Description:
    '
    ' History: 30/10/2002 sj - Created.
    '
    ' ***************************************************************** '
    Private Function InitialiseBusinessObject(ByRef r_oObject As Object, ByVal v_sClassName As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim FullName As Object = CType(System.Reflection.Assembly.GetAssembly(
                    Type.GetType(v_sClassName + "," + v_sClassName.Substring(0, v_sClassName.LastIndexOf(".")))).
                                        FullName, Object)
        r_oObject =
            Activator.CreateInstance(
                FullName, CType(v_sClassName, Object)).Unwrap()
        Dim oDataBase As Object = Nothing
        oDataBase = m_oDatabase
        m_lReturn = r_oObject.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID),
                                         iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID),
                                         iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel),
                                         sCallingAppName:=ToSafeString(m_sCallingAppName), vDatabase:=oDataBase)
      '  m_oDatabase = CType(oDataBase, dPMDAO.Database)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:=v_sClassName & ".Initialise Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:="InitialiseBusinessObject")
            Return result
        End If

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: CloseBusinessObject
    '
    ' Description: close down business objects required to run renewal
    '
    ' History: 24/07/2001 TN - Created.
    '
    ' ***************************************************************** '
    Private Function CloseBusinessObject() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        If Not (m_oInsuranceFile Is Nothing) Then

            m_oInsuranceFile.Dispose()
            m_oInsuranceFile = Nothing
        End If

        If Not (m_oReinsurance Is Nothing) Then

            m_oReinsurance.Dispose()
            m_oReinsurance = Nothing
        End If

        If Not (m_oTax Is Nothing) Then

            m_oTax.Dispose()
            m_oTax = Nothing
        End If

        If Not (m_oRiskData Is Nothing) Then

            m_oRiskData.Dispose()
            m_oRiskData = Nothing
        End If

        If Not (m_oPerilAllocation Is Nothing) Then

            m_oPerilAllocation.Dispose()
            m_oPerilAllocation = Nothing
        End If

        If Not (m_oAgentCommission Is Nothing) Then

            m_oAgentCommission.Dispose()
            m_oAgentCommission = Nothing
        End If

        If Not (m_oChangePolicyStatus Is Nothing) Then

            m_oChangePolicyStatus.Dispose()
            m_oChangePolicyStatus = Nothing
        End If

        If Not (m_oDocManagerWrapper Is Nothing) Then

            m_oDocManagerWrapper.Dispose()
            m_oDocManagerWrapper = Nothing
        End If

        'Event
        If Not (m_oEvent Is Nothing) Then

            m_oEvent.Dispose()
            m_oEvent = Nothing
        End If

        If Not (m_oGis Is Nothing) Then

            m_oGis.Dispose()
            m_oGis = Nothing
        End If

        m_oAutoMTAMerge = Nothing

        If Not (m_oDataset Is Nothing) Then
            m_oDataset.Dispose()
            m_oDataset = Nothing
        End If
        'Start(Saurabh Agrawal) Tech Spec VAL P14 Policy Numbering (5.3.2)
        If Not (m_oPolicyNumMaint Is Nothing) Then

            m_oPolicyNumMaint.Dispose()
            m_oPolicyNumMaint = Nothing
        End If
        'End(Saurabh Agrawal) Tech Spec VAL P14 Policy Numbering (5.3.2)
        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: CreateEvent
    '
    ' Description: add record to event_log
    '
    ' History: 04/07/2001 TN - Created.
    '
    ' ***************************************************************** '
    Private Function CreateEvent(ByRef vPartyCnt As Object, ByRef vInsuranceFolderCnt As Object,
                                 ByRef vInsuranceFileCnt As Object, ByRef vDescription As Object) As Integer


        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        '    Set m_oEvent = CreateObject("bSIREvent.Business")


        result = m_oEvent.DirectAdd(vPartyCnt:=vPartyCnt, vInsuranceFolderCnt:=vInsuranceFolderCnt,
                                    vInsuranceFileCnt:=vInsuranceFileCnt, vEventType:=m_lEventType,
                                    vDescription:=vDescription)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            'Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="Failed to add event.", vApp:=ACApp, vClass:=ACClass,
                               vMethod:="CreateEvent", vErrNo:=Informations.Err().Number,
                               vErrDesc:=Informations.Err().Description)
            Return result
        End If

        '    'Terminate the object and clear it up
        '    m_lReturn& = m_oEvent.Terminate()
        '
        '    Set m_oEvent = Nothing


        Return result

    End Function


    Private Function UnlockProductForRenewal(ByRef v_sProduct As Object) As Integer

        Dim result As Integer = 0
        Dim oPMLock As bpmlock.User



        result = gPMConstants.PMEReturnCode.PMTrue

        'Get bPMLock
        oPMLock = New bpmlock.User()
        m_lReturn = oPMLock.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID,
                                       iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID,
                                       iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel,
                                       sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            oPMLock = Nothing

            ' Error message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="Failed to initiaise bPMLock.User object", vApp:=ACApp, vClass:=ACClass,
                               vMethod:="UnlockProductForRenewal", vErrNo:=Informations.Err().Number,
                               vErrDesc:=Informations.Err().Description)

            Return result

        End If


        m_lReturn = oPMLock.UnLockKey(sKeyName:="renewal", vKeyValue:=v_sProduct, iUserID:=m_iUserID)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="Failed to unlock the risk", vApp:=ACApp, vClass:=ACClass,
                               vMethod:="UnlockProductForRenewal", vErrNo:=Informations.Err().Number,
                               vErrDesc:=Informations.Err().Description)

            Return result

        End If
        oPMLock.Dispose()
        oPMLock = Nothing
        Return result

    End Function


    Private Function PrintReport(ByRef sReportName As String) As Integer

        Dim result As Integer = 0
        Dim sExportFile As Object = ""
        Dim sReportOutputLocation As String = ""




        result = gPMConstants.PMEReturnCode.PMTrue

        'Get the path of the report
        m_lReturn = GetReportPath(r_sReportOutputLocation:=sReportOutputLocation)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="Failed To Get Document Type ID For Code (REPORT)", vApp:=ACApp,
                               vClass:=ACClass, vMethod:="PrintReport")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'delete previous version report
        'If FileSystem.Dir(sReportOutputLocation & sReportName & ".*", FileAttribute.Normal) <> "" Then
        '    File.Delete(sReportOutputLocation & sReportName & ".*")
        'End If

        Dim oFile As Array = Directory.GetFiles(sReportOutputLocation & sReportName & ".*", FileAttribute.Normal)
        For i As Integer = 0 To oFile.Length - 1
            If oFile(i).ToString.Contains(sReportOutputLocation & sReportName & ".*") Then
                File.Delete(sReportOutputLocation & sReportName & ".*")
                Exit For
            End If
        Next


        Dim vDefaultValues As Object = Nothing
        Dim vParameters As Object = Nothing

        result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oReport, v_sClassName:="bSIRReportPrint.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Dim r_sMessage As String = "Failed to create an instance of bSIRReportPrint.Business"
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bSIRReportPrint.Business", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
            Return result
        End If

        m_oReport.reportName = sReportName


        m_lReturn = m_oReport.getparameters(vParameters, vDefaultValues)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="m_oReport.getparameters failed.", vApp:=ACApp, vClass:=ACClass,
                               vMethod:="PrintReport")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'export to word format

        m_lReturn = m_oReport.ExportToDisk(r_ExportFile:=sExportFile, v_iFormatType:=0,
                                           v_vParameters:=vParameters)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="Failed To Export '" & sReportName & "' To Word Format", vApp:=ACApp,
                               vClass:=ACClass, vMethod:="PrintReport")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not (m_oReport Is Nothing) Then

            m_oReport.Dispose()
            m_oReport = Nothing
        End If
        'spool the report

        ' Build the full path of the report
        sExportFile = sReportOutputLocation &
                      sReportName &
                      m_iUserID &
                      ".doc"

        'Use the document manager Wrapper to spool the report
        With m_oDocManagerWrapper
            'Initialise variables from previous run

            .PartyCnt = 0

            .InsuranceFileCnt = 0

            .InsuranceFolderCnt = 0

            .ProcessTypesDocsId = 0

            .DocumentTemplateId = 0

            'Set up properties for report run

            .DocName = sExportFile


            m_lReturn = .DocumentTypeCode("REPORT")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="Failed to set m_oDocManagerWrapper.DocumentTypeCode = 'REPORT'",
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="PrintReport")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            .SpoolDesc = sReportName

            .Mode = gSIRLibrary.ACSpoolReportMode


            m_lReturn = .Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="Failed to spool '" & sReportName & "'", vApp:=ACApp, vClass:=ACClass,
                                   vMethod:="PrintReport")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: GetReportPath
    '
    ' Description: Gets the Report Templates location from the registry.
    '
    ' ***************************************************************** '
    Private Function GetReportPath(ByRef r_sReportOutputLocation As String) As Integer

        Dim result As Integer = 0
        Dim sRegPath As String = ""

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily



        result = gPMConstants.PMEReturnCode.PMTrue

        r_sReportOutputLocation = ""

        ' Set to LocalMachine/Sirius/Client
        eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
        eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

        ' Location for Exported Reports
        sRegPath = ""
        m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot,
                                                 v_lPMEProductFamily:=eProductFamily,
                                                 v_lPMERegSettingLevel:=eRegSettingLevel,
                                                 v_sSettingName:="PrntFileDir", r_sSettingValue:=sRegPath)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="Unable to get Report Destination directory from Registry.", vApp:=ACApp,
                               vClass:=ACClass, vMethod:="GetReportPath")
            Return gPMConstants.PMEReturnCode.PMFalse
        Else
            r_sReportOutputLocation = sRegPath
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans(Optional ByVal v_vInsuranceFileCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            Dim sMessage As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If Not Informations.IsNothing(v_vInsuranceFileCnt) Then

                    sMessage = "Failed to Commit database transaction for " & CStr(v_vInsuranceFileCnt)
                Else
                    sMessage = "Failed to Commit database transaction"
                End If
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage,
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans(Optional ByVal v_vInsuranceFileCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            Dim sMessage As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If Not Informations.IsNothing(v_vInsuranceFileCnt) Then

                    sMessage = "Failed to Commit database transaction for " & CStr(v_vInsuranceFileCnt)
                Else
                    sMessage = "Failed to Commit database transaction"
                End If
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage,
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans(Optional ByVal v_vInsuranceFileCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            Dim sMessage As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If Not Informations.IsNothing(v_vInsuranceFileCnt) Then

                    sMessage = "Failed to Rollback database transaction for " & CStr(v_vInsuranceFileCnt)
                Else
                    sMessage = "Failed to Rollback database transaction"
                End If
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage,
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: RenewalSelectionForMTA
    '
    ' Description:
    '
    ' History: 21/01/2003 sj - Created.
    '
    ' ***************************************************************** '
    Public Function RenewalSelectionForMTA(Optional ByVal v_bMergeRisks As Boolean = False,
                                           Optional ByVal v_lPreChangeInsFileCnt As Integer = 0,
                                           Optional ByVal v_lPostChangeInsFileCnt As Integer = 0,
                                           Optional ByRef r_lDeletedRiskInsuranceFileCnt As Integer = 0,
                                           Optional ByRef r_lDeletedRiskCnt As Integer = 0,
                                           Optional ByVal v_iRunMode As Integer = 0,
                                           Optional ByRef r_bShowQuoteMsg As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vRenewalList As Object = Nothing
            Dim lInsuranceFileCnt As Integer
            Dim sInsuranceRef As String = ""

            m_sFailureReason = ""

            If m_lInsuranceFileCnt = 0 Then
                Return result
            ElseIf v_lPostChangeInsFileCnt = 0 Then
                v_lPostChangeInsFileCnt = m_lInsuranceFileCnt
            End If

            m_lReturn = m_oBusiness.GetRenewalSelectionPolicyDetails(lInsurance_file_cnt:=v_lPostChangeInsFileCnt,
                                                                     vResultArray:=vRenewalList)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="m_oBusiness.DelRenewalStatusPolicies failed.", vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="Start")
                Return result
            End If


            If Not Informations.IsArray(vRenewalList) Then
                'Nothing to do so exit here
                m_lNewInsuranceFileCnt = 0
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_iTask = gPMConstants.PMEComponentAction.PMEdit


            lInsuranceFileCnt = gPMFunctions.NullToLong(CStr(vRenewalList(PMFieldPosInsuranceFileCnt, 0)))

            sInsuranceRef = gPMFunctions.NullToString(CStr(vRenewalList(4, 0))).Trim()


            'Process renewal policy


            m_lReturn = CreateRenewalPolicy(r_vRenewalList:=vRenewalList, v_lCount:=vRenewalList.GetUpperBound(1),
                                            v_bMergeRisks:=v_bMergeRisks,
                                            v_lPreChangeInsFileCnt:=v_lPreChangeInsFileCnt,
                                            v_lPostChangeInsFileCnt:=v_lPostChangeInsFileCnt,
                                            r_lDeletedRiskInsuranceFileCnt:=r_lDeletedRiskInsuranceFileCnt,
                                            r_lDeletedRiskCnt:=r_lDeletedRiskCnt, v_iRunMode:=v_iRunMode,
                                            r_bShowQuoteMsg:=r_bShowQuoteMsg)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="CreateRenewalPolicy failed for " & sInsuranceRef, vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="Start")

                'Rollback the transaction
                m_lReturn = RollbackTrans(v_vInsuranceFileCnt:=lInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            If Not r_bShowQuoteMsg Then
                'Set the renewal status to "UPDATE"

                m_lReturn = m_oBusiness.UpdateRenewalStatus(v_lInsuranceFileCnt:=m_lNewInsuranceFileCnt,
                                                            v_sRenewalStatusTypeCode:="UPDATE")
            Else

                m_lReturn = m_oBusiness.UpdatePolicyRenewalStatus(v_lInsuranceFileCnt:=m_lNewInsuranceFileCnt)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError,
                                   sMsg:="m_oBusiness.UpdateRenewalStatus Failed", vApp:=ACApp, vClass:=ACClass,
                                   vMethod:="RenewalSelectionForMTA")
                Return result
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenewalSelectionForMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalSelectionForMTA", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: CopyRiskDataWithMerge
    '
    ' Desc: copy all Risks attached to OldInsuranceFileCnt to NewInsuranceFileCnt
    '          copy all GIS details attached to each risk to NewInsuranceFileCnt
    '
    '       RWH(13/08/01) Ensure whole process is completed even if one stage fails.
    '
    ' ***************************************************************** '
    Private Function CopyRiskDataWithMerge(ByRef r_vRenewalList(,) As Object, ByVal v_lCount As Integer,
                                           ByVal v_lNewInsuranceFileCnt As Integer,
                                           ByRef r_lEligibleForRenewal As Integer, ByRef r_sFailureReason As String,
                                           ByRef r_bCreateWorkManagerTask As Boolean, ByVal v_dtEffectiveDate As Date,
                                           ByVal v_lPreChangeInsFileCnt As Integer,
                                           ByVal v_lPostChangeInsFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vRiskArray As Object
        Dim sInsuranceRef As String = ""
        Dim lRiskCount, lFindRiskArrayIndex As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        r_lEligibleForRenewal = gPMConstants.PMEReturnCode.PMTrue


        sInsuranceRef = gPMFunctions.NullToString(CStr(r_vRenewalList(PMFieldPosInsuranceRef, v_lCount)))

        Debug.WriteLine("Copying Risks - Get relevant risks")

        'get all risks associate with OldInsuranceFileCnt
        'Get a list of all the risks
        m_oAutoMTAMerge = New AutoMTAMerge()

        With m_oAutoMTAMerge


            'developer guide no. 24
            .Gis = m_oGis
            .PreChangeInsFileCnt = v_lPreChangeInsFileCnt
            .PostChangeInsFileCnt = v_lPostChangeInsFileCnt
            .CurrentInsFileCnt = m_lInsuranceFileCnt 'r_vRenewalList(PMFieldPosInsuranceFileCnt, v_lCount&)


            'developer guide no. 24
            .RiskData = m_oRiskData

            'developer guide no. 24
            .Dataset = m_oDataset
            m_lReturn = .GetListOfRisks()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="m_oAutoMTAMerge.GetListOfRisks Failed", vApp:=ACApp, vClass:=ACClass,
                                   vMethod:="CopyRiskDataWithMerge")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End With

        lRiskCount = m_oAutoMTAMerge.RiskCount
        If lRiskCount = -1 Then
            r_sFailureReason = "No risks found"
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If


        'loop thro and copy each risk details
        For lCount As Integer = 0 To lRiskCount

            m_oAutoMTAMerge.CurrentRiskIndex = lCount


            vRiskArray = m_oAutoMTAMerge.FindRiskArray
            lFindRiskArrayIndex = m_oAutoMTAMerge.FindRiskArrayIndex

            Debug.WriteLine("Copying Risk Data")

            If m_oAutoMTAMerge.MergeStatus <> gSIRLibrary.ACRStatusDeletedPostChange Then


                m_lReturn = CopySingleRiskData(r_vRenewalList:=r_vRenewalList,
                                               v_sInsuranceRef:=
                                                  gPMFunctions.NullToString(
                                                      CStr(r_vRenewalList(PMFieldPosInsuranceRef, v_lCount))),
                                               v_lInsuranceFolderCnt:=
                                                  gPMFunctions.NullToLong(
                                                      CStr(r_vRenewalList(PMFieldPosInsuranceFolderCnt, v_lCount))),
                                               v_dtCoverStartDate:=
                                                  gPMFunctions.NullToDate(r_vRenewalList(PMFieldPosCoverStartDate,
                                                                                         v_lCount)),
                                               v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                                               v_vRiskArray:=vRiskArray, v_lCount:=v_lCount,
                                               v_lRiskCount:=lFindRiskArrayIndex,
                                               r_lEligibleForRenewal:=r_lEligibleForRenewal,
                                               r_bCreateWorkManagerTask:=r_bCreateWorkManagerTask,
                                               v_dtEffectiveDate:=v_dtEffectiveDate, v_bMergeRisks:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                       sMsg:="CopySingleRiskData Failed for " & sInsuranceRef, vApp:=ACApp,
                                       vClass:=ACClass, vMethod:="CopyRiskDataWithMerge")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        Next lCount

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CopyRiskDataReinstateDeletedRisk
    '
    ' ***************************************************************** '
    Private Function CopyRiskDataReinstateDeletedRisk(ByRef r_vRenewalList(,) As Object, ByVal v_lCount As Integer,
                                                      ByVal v_lNewInsuranceFileCnt As Integer,
                                                      ByRef r_lEligibleForRenewal As Integer,
                                                      ByRef r_sFailureReason As String,
                                                      ByRef r_bCreateWorkManagerTask As Boolean,
                                                      ByVal v_dtEffectiveDate As Date,
                                                      Optional ByVal v_lPostChangeInsFileCnt As Integer = 0,
                                                      Optional ByRef r_lDeletedRiskInsuranceFileCnt As Integer = 0,
                                                      Optional ByRef r_lDeletedRiskCnt As Integer = 0,
                                                      Optional ByVal v_iRunMode As Integer = 0) As Integer

        Dim result As Integer = 0
        Const ACRiskDataRiskStatusFlag As Integer = 30

        Dim vRiskArray(,) As Object = Nothing
        Dim sInsuranceRef As String = ""
        Dim bProcess As Boolean
        Dim lNewRiskCnt As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        r_lEligibleForRenewal = gPMConstants.PMEReturnCode.PMTrue

        'Delete the original insurance_file_risk_entries for the copied
        'unchanged risk links

        m_lReturn = m_oRiskData.DeleteInsuranceFileRiskLink(v_lInsuranceFileCnt:=v_lNewInsuranceFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="m_oRiskData.DeleteInsuranceFileRiskLink Failed", vApp:=ACApp,
                               vClass:=ACClass, vMethod:="CopyRiskDataReinstateDeletedRisk")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        sInsuranceRef = gPMFunctions.NullToString(CStr(r_vRenewalList(PMFieldPosInsuranceRef, v_lCount)))

        'get all risks associate with the original renewal

        m_lReturn = m_oRiskData.GetRiskAllStatuses(v_lInsuranceFileCnt:=v_lPostChangeInsFileCnt,
                                                   r_vResultArray:=vRiskArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="m_oRiskData.GetRisk Failed for " & sInsuranceRef, vApp:=ACApp,
                               vClass:=ACClass, vMethod:="CopyRiskDataReinstateDeletedRisk")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'do we have any risks
        If Not Informations.IsArray(vRiskArray) Then
            'RWH(16/11/2000) We do not need to report an error if there are no risks.
            r_sFailureReason = "No risks found"
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        'loop thro and copy each risk details

        For lCount As Integer = 0 To vRiskArray.GetUpperBound(1)

            bProcess = False


            If CStr(vRiskArray(ACRiskDataRiskStatusFlag, lCount)) <> "D" Then
                bProcess = True
            ElseIf CDbl(vRiskArray(ACRiskPosCnt, lCount)) = r_lDeletedRiskCnt Then
                bProcess = True
            End If

            If bProcess Then


                m_lReturn = CopySingleRiskData(r_vRenewalList:=r_vRenewalList,
                                               v_sInsuranceRef:=
                                                  gPMFunctions.NullToString(
                                                      CStr(r_vRenewalList(PMFieldPosInsuranceRef, v_lCount))),
                                               v_lInsuranceFolderCnt:=
                                                  gPMFunctions.NullToLong(
                                                      CStr(r_vRenewalList(PMFieldPosInsuranceFolderCnt, v_lCount))),
                                               v_dtCoverStartDate:=
                                                  gPMFunctions.NullToDate(r_vRenewalList(PMFieldPosCoverStartDate,
                                                                                         v_lCount)),
                                               v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                                               v_vRiskArray:=vRiskArray, v_lCount:=v_lCount,
                                               v_lRiskCount:=lCount,
                                               r_lEligibleForRenewal:=r_lEligibleForRenewal,
                                               r_bCreateWorkManagerTask:=r_bCreateWorkManagerTask,
                                               v_dtEffectiveDate:=v_dtEffectiveDate, r_lNewRiskCnt:=lNewRiskCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                       sMsg:="CopySingleRiskData Failed for " & sInsuranceRef, vApp:=ACApp,
                                       vClass:=ACClass, vMethod:="CopyRiskDataReinstateDeletedRisk")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If CDbl(vRiskArray(ACRiskPosCnt, lCount)) = r_lDeletedRiskCnt Then
                r_lDeletedRiskCnt = lNewRiskCnt
            End If

        Next lCount

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: CopyRiskDataApplyDeletedRisk
    '
    ' ***************************************************************** '
    Private Function CopyRiskDataApplyDeletedRisk(ByRef r_vRenewalList(,) As Object, ByVal v_lCount As Integer,
                                                  ByVal v_lNewInsuranceFileCnt As Integer,
                                                  ByRef r_lEligibleForRenewal As Integer,
                                                  ByRef r_sFailureReason As String,
                                                  ByRef r_bCreateWorkManagerTask As Boolean,
                                                  ByVal v_dtEffectiveDate As Date,
                                                  Optional ByVal v_lPostChangeInsFileCnt As Integer = 0,
                                                  Optional ByRef r_lDeletedRiskInsuranceFileCnt As Integer = 0,
                                                  Optional ByRef r_lDeletedRiskCnt As Integer = 0,
                                                  Optional ByVal v_iRunMode As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vRiskArray(,) As Object = Nothing
        Dim sInsuranceRef As String = ""
        Dim lNewRiskCnt As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        r_lEligibleForRenewal = gPMConstants.PMEReturnCode.PMTrue

        'Delete the original insurance_file_risk_entries for the copied
        'unchanged risk links

        m_lReturn = m_oRiskData.DeleteInsuranceFileRiskLink(v_lInsuranceFileCnt:=v_lNewInsuranceFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="m_oRiskData.DeleteInsuranceFileRiskLink Failed", vApp:=ACApp,
                               vClass:=ACClass, vMethod:="CopyRiskDataApplyDeletedRisk")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        sInsuranceRef = gPMFunctions.NullToString(CStr(r_vRenewalList(PMFieldPosInsuranceRef, v_lCount)))

        'get all risks associate with the original renewal

        m_lReturn = m_oRiskData.GetRisk(v_lInsuranceFileCnt:=v_lPostChangeInsFileCnt, r_vResultArray:=vRiskArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="m_oRiskData.GetRisk Failed for " & sInsuranceRef, vApp:=ACApp,
                               vClass:=ACClass, vMethod:="CopyRiskDataApplyDeletedRisk")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'do we have any risks
        If Not Informations.IsArray(vRiskArray) Then
            'RWH(16/11/2000) We do not need to report an error if there are no risks.
            r_sFailureReason = "No risks found"
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        'loop thro and copy each risk details

        For lCount As Integer = 0 To vRiskArray.GetUpperBound(1)


            m_lReturn = CopySingleRiskData(r_vRenewalList:=r_vRenewalList,
                                           v_sInsuranceRef:=
                                              gPMFunctions.NullToString(CStr(r_vRenewalList(PMFieldPosInsuranceRef,
                                                                                            v_lCount))),
                                           v_lInsuranceFolderCnt:=
                                              gPMFunctions.NullToLong(
                                                  CStr(r_vRenewalList(PMFieldPosInsuranceFolderCnt, v_lCount))),
                                           v_dtCoverStartDate:=
                                              CDate(r_vRenewalList(PMFieldPosCoverStartDate, v_lCount)),
                                           v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                                           v_vRiskArray:=vRiskArray, v_lCount:=v_lCount, v_lRiskCount:=lCount,
                                           r_lEligibleForRenewal:=r_lEligibleForRenewal,
                                           r_bCreateWorkManagerTask:=r_bCreateWorkManagerTask,
                                           v_dtEffectiveDate:=v_dtEffectiveDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                   sMsg:="CopySingleRiskData Failed for " & sInsuranceRef, vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="CopyRiskDataApplyDeletedRisk")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Next lCount

        'Get all the risks from the last version which has the restored risk

        m_lReturn = m_oRiskData.GetRiskAllStatuses(v_lInsuranceFileCnt:=r_lDeletedRiskInsuranceFileCnt,
                                                   r_vResultArray:=vRiskArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="m_oRiskData.GetRisk Failed for " & sInsuranceRef, vApp:=ACApp,
                               vClass:=ACClass, vMethod:="CopyRiskDataApplyDeletedRisk")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'do we have any risks
        If Not Informations.IsArray(vRiskArray) Then
            'RWH(16/11/2000) We do not need to report an error if there are no risks.
            r_sFailureReason = "No risks found"
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        'loop thro and copy each risk details

        For lCount As Integer = 0 To vRiskArray.GetUpperBound(1)


            If CDbl(vRiskArray(ACRiskPosCnt, lCount)) = r_lDeletedRiskCnt Then
                'Reinstate the deleted risk


                m_lReturn = CopySingleRiskData(r_vRenewalList:=r_vRenewalList,
                                               v_sInsuranceRef:=
                                                  gPMFunctions.NullToString(
                                                      CStr(r_vRenewalList(PMFieldPosInsuranceRef, v_lCount))),
                                               v_lInsuranceFolderCnt:=
                                                  gPMFunctions.NullToLong(
                                                      CStr(r_vRenewalList(PMFieldPosInsuranceFolderCnt, v_lCount))),
                                               v_dtCoverStartDate:=
                                                  gPMFunctions.NullToDate(r_vRenewalList(PMFieldPosCoverStartDate,
                                                                                         v_lCount)),
                                               v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                                               v_vRiskArray:=vRiskArray, v_lCount:=v_lCount,
                                               v_lRiskCount:=lCount,
                                               r_lEligibleForRenewal:=r_lEligibleForRenewal,
                                               v_dtEffectiveDate:=v_dtEffectiveDate,
                                               r_bCreateWorkManagerTask:=r_bCreateWorkManagerTask,
                                               r_lNewRiskCnt:=lNewRiskCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError,
                                       sMsg:="CopySingleRiskData Failed for " & sInsuranceRef, vApp:=ACApp,
                                       vClass:=ACClass, vMethod:="CopyRiskDataApplyDeletedRisk")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                r_lDeletedRiskCnt = lNewRiskCnt
            End If
        Next lCount

        r_lDeletedRiskInsuranceFileCnt = v_lNewInsuranceFileCnt

        Return result

    End Function

    Private Function GetProductOptions(ByVal lOptionNumber As Long,
                                       ByRef r_vValue As Object) As Long

        Const kMethodName As String = "GetProductOptions"

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        '*****************************************


        m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:="",
                                                  v_sPassword:="", v_iUserID:=0,
                                                  v_iMainSourceID:=0, v_iLanguageID:=0,
                                                  v_iCurrencyID:=0, v_iLogLevel:=0,
                                                  v_sCallingAppName:="",
                                                  v_vOptionNumber:=lOptionNumber,
                                                  v_vBranch:=SIRBCHHeadOffice,
                                                  r_vUnderwriting:=r_vValue)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            RaiseError(kMethodName, "getProductOptionValue Failed " &
                                    " to return value for Option:" &
                                    gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007,
                       gPMConstants.PMELogLevel.PMLogError)
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
End Class
