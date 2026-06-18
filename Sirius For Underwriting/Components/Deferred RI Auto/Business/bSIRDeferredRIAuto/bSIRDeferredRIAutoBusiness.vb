Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'refer Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 27/09/00
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRRenewal.
    '
    ' Edit History:
    ' ***************************************************************** '


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
    ' PM Event Business Component (Private)
    Private oEvent As bSIREvent.Business

    'Private oAccumulation As iPMUAccumulationValues.Interface_Renamed

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    'Thinh Nguyen 27/02/2004 - need for ProcessSingleDefRIPolicy()
    Private m_oRenSelection As bSIRRenSelection.Business
    Private m_oInsuranceFile As bSIRInsuranceFile.Services
    Private m_oReinsurance As Object = Nothing
    Private m_oTax As bSIRRITax.Business
    Private m_oRiskData As bSIRRiskData.Business
    Private m_oPerilAllocation As bSirPerilAllocation.Business
    Private m_oAgentCommission As bSirAgentCommission.Business
    Private m_oControlTrans As bControlTrans.Automated
    Private aClaimsArray(,) As Object

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property




    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property



    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
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




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            'create relevant business objects to do ProcessSingleDefRIPolicy()
            m_lReturn = CType(CreateDefRIBusinessObject(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

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
            If disposing Then
                CloseDefRIBusinessObject()
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", excep:=excep)

            Return result

        End Try
    End Function

    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub



    ' ---------------------------------------------------------------------------
    ' PROCEDURE:   GetDeferredRIPolicies
    ' AUTHOR:      AMB
    ' DATE:        05-Sep-03
    ' DESCRIPTION: get policies with risk on deferref ri model
    ' HISTORY:     Created for 1.8.6 Deferred RI development
    ' ---------------------------------------------------------------------------
    Public Function GetDeferredRIPolicies(ByRef r_vDeferredRIArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kSelDefRIPoliciesSQL, sSQLName:=kSelDefRIPoliciesName, bStoredProcedure:=kSelDefRIPoliciesStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vDeferredRIArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDeferredRIPolicies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDeferredRIPolicies", excep:=excep)

            Return result
        End Try
    End Function
    ' ---------------------------------------------------------------------------
    ' PROCEDURE:   SetDeferredRIStatus
    ' AUTHOR:      AMB
    ' DATE:        09-Sep-03
    ' DESCRIPTION: Update the Insurance_File_Deferred_RI_Usage table
    ' HISTORY:     Created for 1.8.6 Deferred RI development
    ' ---------------------------------------------------------------------------
    Public Function SetDeferredRIStatus(ByVal v_lInsFileDeferredRIUsageID As Integer, ByVal v_lInsFileCnt As Integer, ByVal v_lDefRIStatusID As Integer) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Try

            ' set the params
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ins_file_deferred_RI_usage_id", vValue:=CStr(v_lInsFileDeferredRIUsageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="deferred_RI_status_type_id", vValue:=CStr(v_lDefRIStatusID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' do the update call
            m_lReturn = m_oDatabase.SQLAction(sSQL:=kUpdDefRIPoliciesSQL, sSQLName:=kUpdDefRIPoliciesName, bStoredProcedure:=kUpdDefRIPoliciesStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetDeferredRIStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetDeferredRIStatus", excep:=excep)

            Return nResult
        End Try
    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE:   InsertInsFileDefRIUsage
    ' AUTHOR:      AMB
    ' DATE:        09-Sep-03
    ' DESCRIPTION: Insert a record into the Insurance_File_Deferred_RI_Usage table
    ' HISTORY:     Created for 1.8.6 Deferred RI development
    '              03/03/2004 - don't need risk count as all risks will be picked up from listrisk screen
    ' ---------------------------------------------------------------------------
    Public Function InsertInsFileDefRIUsage(ByVal v_lInsFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try
            ' set the params
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ins_file_deferred_RI_usage_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' do the call to insert the record into Insurance_File_Deferred_RI_Usage
            m_lReturn = m_oDatabase.SQLAction(sSQL:=kInsertInsFileDefRIUsageSQL, sSQLName:=kInsertInsFileDefRIUsageName, bStoredProcedure:=kInsertInsFileDefRIUsageStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InsertInsFileDefRIUsage Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InsertInsFileDefRIUsage", excep:=excep)
            Return result
        End Try
    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE:   UpdateRiskRIModel
    ' AUTHOR:      AMB
    ' DATE:        24-Sep-03
    ' DESCRIPTION: Update the risk's RI_Model if a new one has come into effect
    ' HISTORY:     Created for 1.8.6 Deferred RI development
    ' ---------------------------------------------------------------------------
    Public Function UpdateRiskRIModel(ByVal v_lInsFileCnt As Integer, ByVal v_lRiskCnt As Integer) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Try

            ' set the params
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(v_lRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' do the call to insert the record into Insurance_File_Deferred_RI_Usage
            m_lReturn = m_oDatabase.SQLAction(sSQL:=kUpdateRiskRIModelSQL, sSQLName:=kUpdateRiskRIModelName, bStoredProcedure:=kUpdateRiskRIModelStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRiskRIModel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskRIModel", excep:=excep)

            Return nResult
        End Try
    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE:   GetRiskStatus
    ' AUTHOR:      AMB
    ' DATE:        24-Sep-03
    ' DESCRIPTION: Get the 'risk_status' of a risk (doh)
    ' HISTORY:     Created for 1.8.6 Deferred RI development
    ' ---------------------------------------------------------------------------
    Public Function GetRiskStatus(ByVal v_lRiskCnt As Integer, ByRef r_lRiskStatusID As Integer, ByRef r_sRiskStatusCode As String) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim aDeferredRIArray(,) As Object

        Try

            ' set the params
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(v_lRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' get the risk status of the risk
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetRiskStatusSQL, sSQLName:=kGetRiskStatusName, bStoredProcedure:=kGetRiskStatusStored, vResultArray:=aDeferredRIArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If Information.IsArray(aDeferredRIArray) Then
                r_lRiskStatusID = gPMFunctions.NullToLong(aDeferredRIArray(0, 0))
                r_sRiskStatusCode = gPMFunctions.NullToString(aDeferredRIArray(1, 0))
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskStatus", excep:=excep)

            Return nResult
        End Try
    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE:   DeleteInsFileDefRIUsage
    ' AUTHOR:      AMB
    ' DATE:        09-Sep-03
    ' DESCRIPTION: Delete the Insurance_File_Deferred_RI_Usage record
    ' HISTORY:     Created for 1.8.6 Deferred RI development
    ' ---------------------------------------------------------------------------
    Public Function DeleteInsFileDefRIUsage(ByVal v_lInsFileDeferredRIUsageID As Integer) As Integer

        Dim result As Integer = 0
        Try

            ' set the params
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ins_file_deferred_RI_usage_id", vValue:=CStr(v_lInsFileDeferredRIUsageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' do the call to remove the record from Insurance_File_Deferred_RI_Usage
            m_lReturn = m_oDatabase.SQLAction(sSQL:=kDeleteInsFileDefRIUsageSQL, sSQLName:=kDeleteInsFileDefRIUsageName, bStoredProcedure:=kDeleteInsFileDefRIUsageStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteInsFileDefRIUsage Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteInsFileDefRIUsage", excep:=excep)

            Return result
        End Try
    End Function

    '***********************************************************************************************************************
    ' Name : ProcessSingleDefRIPolicy
    '
    ' Desc : create new version of policy (same type as current version) and copy related data
    '        new version of policy might still be on Deferred RI model if live model doesn't exist
    ' Author : Thinh Nguyen 27/02/2004
    ' Note   : new version of policy will have the same policy type as deferred version, deferred version will be set to
    '           "Replaced - Deferred Reinsurance". transaction type for processing will be DRI.
    '***********************************************************************************************************************
    Public Function ProcessSingleDefRIPolicy(ByVal nInsuranceFileCnt As Integer, Optional ByRef nInsStatus As Integer = 0, Optional ByRef nNewInsurancefileCount As Integer = 0, Optional ByVal sInsuranceFileType As String = "") As Integer

        Dim bReturnValue As gPMConstants.PMEReturnCode
        Dim sMessage As String = ""
        Dim nNewInsurancefileCnt, nInsuranceFolderCnt As Integer
        Dim dtPolicyStartDate As Date
        Dim sTransactionType As String = ""
        Dim bReturn As gPMConstants.PMEReturnCode
        Dim nPartyCnt As Integer
        Dim nIsValid As Integer
        Const kMethodName As String = "ProcessSingleDefRIPolicy"


        Try
            bReturnValue = gPMConstants.PMEReturnCode.PMTrue
            sMessage = ""

            sTransactionType = "DRI"

            'set transaction so we can roll back if something gone wrong
            If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                bReturnValue = gPMConstants.PMEReturnCode.PMFalse
                sMessage = "Failed to begin SQLTransaction"
                Throw New Exception(sMessage)
            End If

            If (sInsuranceFileType = "QUOTE" Or sInsuranceFileType = "RENEWAL" Or sInsuranceFileType = "MTAQUOTE" Or m_oInsuranceFile.InsuranceFileType = "MTAQTETEMP" Or sInsuranceFileType = "MTAQREINS") Then
                m_lReturn = RecalculateRIQuote(nInsuranceFileCnt:=nInsuranceFileCnt,
                                      nIsValid:=nIsValid)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMessage = "Failed to recalculate RI for quote"
                    'RaiseError(kMethodName, sMessage, gPMConstants.PMELogLevel.PMLogError)
                    Throw New Exception(sMessage)
                End If
                nNewInsurancefileCnt = nInsuranceFileCnt
            Else
                CreateDefRIBusinessObject()

                aClaimsArray = Nothing
                m_lReturn = ClaimReversingPrePolicyTransfer(nInsuranceFileCnt:=nInsuranceFileCnt, nPartyCnt:=nPartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception(sMessage)
                End If

                'copy current policy details and all policy level details
                If CopyPolicyHeader(nInsuranceFileCnt:=nInsuranceFileCnt, nNewInsurancefileCnt:=nNewInsurancefileCnt, r_sMessage:=sMessage, nInsuranceFolderCnt:=nInsuranceFolderCnt, sSetOldInsuranceFileStatus:=gSIRLibrary.SIRInsFileStatusReplacedDRI, sTransactionType:=sTransactionType, nPartyCnt:=nPartyCnt, sInsStatus:=nInsStatus) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New Exception(sMessage)
                End If

                'copy risk details (error messages are dealt within CopyDefRIRisks())
                'copy risks as mta's in order to rollout previous deferred values
                If CopyDefRIRisks(v_lInsuranceFileCnt:=nInsuranceFileCnt, v_lNewInsuranceFileCnt:=nNewInsurancefileCnt, v_lInsuranceFolderCnt:=nInsuranceFolderCnt, v_dtPolicyStartDate:=dtPolicyStartDate, v_sTransactionType:=sTransactionType, r_sMessage:=sMessage) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception(sMessage)
                End If

                If UpdateDeferredRI_Renewal_Status(nInsuranceFileCnt:=nInsuranceFileCnt, nNewInsuranceFileCnt:=nNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception(sMessage)
                End If

                'process stats (error messages are dealt within CreateAndPostStats())

                If m_oInsuranceFile.InsuranceFileType = "POLICY" Or m_oInsuranceFile.InsuranceFileType = "MTA PERM" Or m_oInsuranceFile.InsuranceFileType = "MTA TEMP" Or m_oInsuranceFile.InsuranceFileType = "MTACAN" Or m_oInsuranceFile.InsuranceFileType = "MTAREINS" Then
                    If CreateAndPostStats(v_lInsuranceFileCnt:=nNewInsurancefileCnt, v_sTransactionType:=sTransactionType, r_sMessage:=sMessage) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception(sMessage)
                    End If
                End If

                m_lReturn = TransferClaimToNewRisk(nInsuranceFileCnt:=nInsuranceFileCnt, nNewInsuranceFileCnt:=nNewInsurancefileCnt, r_sMessage:=sMessage, nPartyCnt:=nPartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception(sMessage)
                End If

                ' clear up any entries in the defered ri usage table that are no longer applicable
                ' as they have now been processed by the batch utility...
                bReturn = CType(DeleteInsFileDeferredRIUsage(v_lInsuranceFileCnt:=nInsuranceFileCnt), gPMConstants.PMEReturnCode)
                If bReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception(sMessage)
                End If

            End If
            'save all transactions to database
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to commit SQLTransaction"
                Throw New Exception(sMessage)
            End If

            If (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) Then
                nNewInsurancefileCount = nNewInsurancefileCnt
            End If
        Catch ex As Exception

            'roll back all transactions as one of the step has failed
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            bReturnValue = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=IIf(sMessage = "", "Failed ProcessSingleDefRIPolicy()", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSingleDefRIPolicy", excep:=ex)


        Finally



        End Try
        Return bReturnValue
    End Function


    '***********************************************************************************************************************
    ' Name : CreateDefRIBusinessObject
    '
    ' Desc : create required objects for ProcessSingleDefRIPolicy
    '
    ' Author : Thinh Nguyen 27/02/2004
    '***********************************************************************************************************************
    Private Function CreateDefRIBusinessObject() As Integer

        Dim result As Integer = 0
        Dim sMessage As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            sMessage = ""


            m_oRenSelection = New bSIRRenSelection.Business
            m_lReturn = m_oRenSelection.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRRenSelection.Business"
                Throw New Exception()
            End If


            m_oInsuranceFile = New bSIRInsuranceFile.Services
            m_lReturn = m_oInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRInsuranceFile.Services"
                Throw New Exception()
            End If


            Dim sIsRI2007 As String = ""

            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword,
                                                      v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID,
                                                      v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID,
                                                      v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp,
                                                      v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007,
                                                      v_vBranch:=m_iSourceID, r_vUnderwriting:=sIsRI2007)

            If sIsRI2007 = "1" Then
                m_lReturn = CreateBusinessObject(r_oObject:=m_oReinsurance, v_sClassName:="bSIRReinsuranceRI2007.Form")
            Else
                m_lReturn = CreateBusinessObject(r_oObject:=m_oReinsurance, v_sClassName:="bSIRReinsurance.Form")
            End If


            'm_oReinsurance = New bSIRReinsurance.Form
            'm_lReturn = m_oReinsurance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRReinsurance.Form"
                Throw New Exception()
            End If

            'changes done in name bSIRRITax.business to bSIRRITax.Business due to dll issue

            m_oTax = New bSIRRITax.Business
            m_lReturn = m_oTax.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRRITax.Business"
                Throw New Exception()
            End If

            'changes done in name bSIRRiskData.business to bSIRRiskData.Business due to dll issue

            m_oRiskData = New bSIRRiskData.Business
            m_lReturn = m_oRiskData.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRRiskData.Business"
                Throw New Exception()
            End If

            'changes done in name bSIRPerilAllocation.Business to bSirPerilAllocation.Business due to dll issue

            m_oPerilAllocation = New bSirPerilAllocation.Business
            m_lReturn = m_oPerilAllocation.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSirPerilAllocation.Business"
                Throw New Exception()
            End If

            'changes done in name bSIRAgentCommission.Business to bSirAgentCommission.Business due to dll issue

            m_oAgentCommission = New bSirAgentCommission.Business
            m_lReturn = m_oAgentCommission.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRAgentCommission.Business"
                Throw New Exception()
            End If



            m_oControlTrans = New bControlTrans.Automated
            m_lReturn = m_oControlTrans.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceId:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bControlTrans.Automated"
                Throw New Exception()
            End If


            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=IIf(sMessage = "", "Failed CreateDefRIBusinessObject", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDefRIBusinessObject", excep:=excep)
            CloseDefRIBusinessObject()
            Return result
        End Try
    End Function

    Private Function CreateBusinessObject(ByRef r_oObject As Object, ByVal v_sClassName As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Return gPMComponentServices.CreateBusinessObject(r_oObject:=r_oObject, v_sClassName:=v_sClassName, v_sCallingAppName:=m_sCallingAppName, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

    End Function

    '***********************************************************************************************************************
    ' Name : CloseDefRIBusinessObject
    '
    ' Desc : close objects open by CreateDefRIBusinessObject()
    '
    ' Author : Thinh Nguyen 27/02/2004
    '***********************************************************************************************************************
    Private Sub CloseDefRIBusinessObject()


        Try

            If Not (m_oRenSelection Is Nothing) Then

                m_oRenSelection.Dispose()
                m_oRenSelection = Nothing
            End If

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

            If Not (m_oControlTrans Is Nothing) Then

                m_oControlTrans.Dispose()
                m_oControlTrans = Nothing
            End If

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try


    End Sub

    '***********************************************************************************************************************
    ' Name : CopyDefRIRisks
    '
    ' Desc : copy related risks details to new policy version
    '
    ' Author : Thinh Nguyen 27/02/2004
    ' Note : if all new version of risks are still on deferred then rollback policy

    '************************ Note on Risk **************************
    'if risk has deferred RI model then we need to
    'begin transaction (risk)
    ' 1. copy risk
    ' 2. rerate (populate rating sections)
    ' 3. apply reinsurance
    ' 4. apply tax - now that risk has been successfully processedOn Error GoTo Err_CopyDefRIRisks
    ' 5. if valid bands
    'if new risk is on live RI model (ie one of the bands is now on live model)
    '5.1 change old risk to quoted so it won't be pick up next time
    '5.2 change new risk to quoted if current status is pending reinsurance
    '5.3 move claims to new risk
    '5.4 if everything is ok then commit risk changes
    '5.5 set commit policy flag to true
    'else risk still on deferred model
    'rollback transaction (risk)
    '5.1 relink risk to new policy version
    '5.2 move claims to new policy version same risk
    'else
    '1. create a new link in insurance_file_risk_link with status_flag = "U" (unchanged)
    '2. move claims to new version of policy (same risk)
    'endif
    'if commit policy flag is not set then rollback policy (will be done on calling function)
    '***********************************************************************************************************************
    Public Function CopyDefRIRisks(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_dtPolicyStartDate As Date, ByVal v_sTransactionType As String, ByRef r_sMessage As String, Optional ByRef v_bIgnoreError As Boolean = False) As Integer

        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim vRiskArray(,) As Object
        Dim vGISPolicyLinkArray(,) As Object
        Dim lRiskCnt, lNewRiskCnt As Integer
        Dim sGISDataModelCode As String = ""
        Dim lGISPolicyLinkID, lNewGisPolicyLinkID, lPolicyBinderID, lNewPolicyBinderID As Integer
        Dim sXMLDataSetDef, sXMLDataSet As String
        Dim lValidRIBand As gPMConstants.PMEReturnCode
        Dim lReinsBand As Integer 'not used - function required 
        Dim vRiskTax As Object
        Dim sDescription As String = ""
        Dim lRiskStatusID As Integer
        Dim sRiskStatusCode As String = ""
        Dim lCommitPolicy As gPMConstants.PMEReturnCode 'set to true when one of the deferred risk is now on live model and everything is ok 
        Dim lDeferredRIBand As Integer 'number of bands which are on deferred RI model 
        Dim lDeferredRIBandNewRisk As Integer 'number of bands which are on deferred RI model 
        Dim lTransactionStarted As gPMConstants.PMEReturnCode 'set to pmtrue when m_oDatabase.SQLBeginTrans() is called 
        Dim vResultArray(,) As Object
        Dim sInsuranceRef As String = ""
        Dim sMessage As String
        Dim lClaimsCount As Long
        Dim lInsuranceFileType As Integer

        Dim lPreviousRiskCnt As Integer

        Const ACFieldPosRiskID As Integer = 0
        Const ACFieldPosGisScreenID As Integer = 21
        Const ACFieldPosIsRIDeferred As Integer = 7

        Try
            lReturnValue = gPMConstants.PMEReturnCode.PMTrue
            r_sMessage = ""
            lValidRIBand = gPMConstants.PMEReturnCode.PMFalse
            lCommitPolicy = gPMConstants.PMEReturnCode.PMFalse
            lTransactionStarted = gPMConstants.PMEReturnCode.PMFalse


            If m_oRiskData.GetRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vResultArray:=vRiskArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to get associated risks for this policy " & v_lInsuranceFileCnt
                Throw New Exception(r_sMessage)
            End If

            'do we have any risks
            If Not Information.IsArray(vRiskArray) Then
                Return lReturnValue
            End If

            m_lReturn = GetPolicyType(v_lInsuranceFileCnt, lInsuranceFileType)

            'make sure we have the right transaction type

            If m_oRiskData.SetProcessModes(vTransactionType:=v_sTransactionType) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to set process mode to RiskData object (bSIRRiskData.business)"
                Throw New Exception(r_sMessage)
            End If

            'process each risk
            m_lReturn = GetPolicyType(v_lInsuranceFileCnt, lInsuranceFileType)
            For lCount As Integer = 0 To vRiskArray.GetUpperBound(1)
                If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed SQLBeginTrans()"
                    Throw New Exception(r_sMessage)
                End If

                lTransactionStarted = gPMConstants.PMEReturnCode.PMTrue

                lRiskCnt = gPMFunctions.NullToLong(vRiskArray(ACFieldPosRiskID, lCount))

                'how many bands are on deferred ri model for this risk
                If GetRiskDeferredRIBand(v_lRiskCnt:=lRiskCnt, r_lDeferredRIBand:=lDeferredRIBand) <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to get RI model for old risk (Policy: " & v_lInsuranceFileCnt & " Risk: " & CStr(lRiskCnt) & ")"
                    Throw New Exception(r_sMessage)
                End If

                'either reinsurance is not apply or this risk is on a live RI model
                'we just need to create the insurance file link and move on to process next risk
                If lDeferredRIBand = 0 Then

                    'add new risk link

                    If m_oRiskData.AddRiskLink(v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_lRiskCnt:=lRiskCnt, v_sStatusFlag:="U") <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to create new insurance file risk link (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & CStr(lRiskCnt) & ")"
                        Throw New Exception(r_sMessage)
                    End If

                    'move claims to new version of policy with same risks
                    If MoveClaimToNewRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCnt:=lRiskCnt, v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_lNewRiskCnt:=lRiskCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to move claims to new version of policy  (old Policy: " & v_lInsuranceFileCnt & " old Risk: " & CStr(lRiskCnt) &
                                     " new policy: " & CStr(v_lNewInsuranceFileCnt) & " new risk: " & CStr(lNewRiskCnt) & ")"
                        Throw New Exception(r_sMessage)
                    End If
                    If aClaimsArray IsNot Nothing Then
                        For lClaimsCount = 0 To UBound(aClaimsArray, 2)
                            If aClaimsArray(2, lClaimsCount) = lRiskCnt Then
                                aClaimsArray(3, lClaimsCount) = lNewRiskCnt
                            End If
                        Next
                    End If
                    'save changes
                    If m_oDatabase.SQLCommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed SQLCommitTrans()"
                        Throw New Exception(r_sMessage)
                    End If

                    lTransactionStarted = gPMConstants.PMEReturnCode.PMFalse
                Else
                    'risk is on deferred ri model

                    '************** we are on a deferred RI model ***********************
                    'copy risk details and reset status to unquoted
                    If lInsuranceFileType = 2 Then
                        If m_oRiskData.CopyRisk(v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_vRiskDetail:=vRiskArray, v_lPosNo:=lCount, r_lRiskCnt:=lNewRiskCnt, v_lResetStatus:=gPMConstants.PMEReturnCode.PMTrue, v_lCreateLinkType:=kInsFileRiskLinkTypeORIGINAL) <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to copy risk details to new policy version (old policy :" & v_lInsuranceFileCnt & " new policy " & CStr(v_lNewInsuranceFileCnt) & " Risk :)" & CStr(lRiskCnt) & ")"
                            Throw New Exception(r_sMessage)
                        End If
                    Else
                        m_lReturn = GetPreviousRiskCnt(v_lPreInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCnt:=lRiskCnt, v_lInsuranceFileCnt:=v_lNewInsuranceFileCnt, r_lPreviousRiskCnt:=lPreviousRiskCnt)

                        If CopyRisk(v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_vRiskDetail:=vRiskArray, v_lPosNo:=lCount, r_lRiskCnt:=lNewRiskCnt, v_lResetStatus:=gPMConstants.PMEReturnCode.PMTrue, v_lCreateLinkType:=kInsFileRiskLinkTypeORIGINAL, v_lOldRiskCnt:=IIf(lPreviousRiskCnt = 0, lRiskCnt, lPreviousRiskCnt)) <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to copy risk details to new policy version (old policy :" & v_lInsuranceFileCnt & " new policy " & CStr(v_lNewInsuranceFileCnt) & " Risk :)" & CStr(lRiskCnt) & ")"
                            Throw New Exception(r_sMessage)
                        End If

                    End If

                    If lInsuranceFileType = 5 Or lInsuranceFileType = 6 Then
                        If UpdatePremium(lNewRiskCnt, m_oPerilAllocation.ProRataRate, lInsuranceFileType) <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to update premium for Policy: " & v_lNewInsuranceFileCnt & " Risk: " & lNewRiskCnt & ")"
                            Throw New Exception(r_sMessage)
                        End If
                    End If



                    'If m_oRiskData.CopyRisk(v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_vRiskDetail:=vRiskArray, v_lPosNo:=lCount, r_lRiskCnt:=lNewRiskCnt, v_lResetStatus:=gPMConstants.PMEReturnCode.PMTrue, v_lCreateLinkType:=gPMConstants.kInsFileRiskLinkTypeORIGINAL) <> gPMConstants.PMEReturnCode.PMTrue Then

                    '    r_sMessage = "Failed to copy risk details to new policy version (old policy :" & v_lInsuranceFileCnt & " new policy " & CStr(v_lNewInsuranceFileCnt) & " Risk :)" & CStr(lRiskCnt) & ")"
                    '    Throw New Exception(r_sMessage)
                    'End If

                    If UpdateRiskLink(lNewRiskCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception(r_sMessage)
                    End If

                    '************************ copy gis data for this risk *****************************
                    'get get policy link for old policy

                    If m_oRiskData.GetGISPolicyLink(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lRiskID:=lRiskCnt, r_vResultArray:=vGISPolicyLinkArray) <> gPMConstants.PMEReturnCode.PMTrue Then

                        r_sMessage = "Failed to copy gis details to new policy version (old policy :" & v_lInsuranceFileCnt & " new policy: " & CStr(v_lNewInsuranceFileCnt) & " Risk: " & CStr(lRiskCnt) & ")"
                        Throw New Exception(r_sMessage)
                    End If

                    'do we have any gis data
                    lGISPolicyLinkID = gPMFunctions.NullToLong(vGISPolicyLinkArray(0, 0))
                    sGISDataModelCode = gPMFunctions.NullToString(vGISPolicyLinkArray(4, 0)).Trim()

                    If lGISPolicyLinkID = 0 Then
                        r_sMessage = "We have no gis data for policy version (old policy :" & v_lInsuranceFileCnt & " Risk: " & CStr(lRiskCnt) & ")"
                        Throw New Exception(r_sMessage)
                    End If

                    'load gis detail for old risk (Note: gis object store folder_cnt in file_cnt field)

                    If m_oRenSelection.GIS_LoadFromDB(v_sGisDataModelCode:=sGISDataModelCode, r_vInsuranceFileCnt:=v_lInsuranceFolderCnt, r_vPolicyLinkID:=lGISPolicyLinkID, r_vRiskID:=lRiskCnt) <> gPMConstants.PMEReturnCode.PMTrue Then

                        r_sMessage = "Failed to load gis data (policy folder :" & v_lInsuranceFolderCnt & " Risk: " & CStr(lRiskCnt) & ")"
                        Throw New Exception(r_sMessage)
                    End If

                    'copy gis data to new risk (Note: gis object store folder_cnt in file_cnt field)

                    If m_oRenSelection.CopyDataSet(v_sDataModelCode:=sGISDataModelCode, r_lNewGISPolicyLinkId:=lNewGisPolicyLinkID, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet, v_vOldGISPolicyLinkId:=lGISPolicyLinkID, v_vOldInsuranceFileCnt:=v_lInsuranceFolderCnt, v_vOldRiskID:=lRiskCnt, v_vNewInsuranceFileCnt:=v_lInsuranceFolderCnt, v_vNewRiskID:=lNewRiskCnt) <> gPMConstants.PMEReturnCode.PMTrue Then

                        r_sMessage = "Failed to copy gis data (policy folder :" & v_lInsuranceFolderCnt & "Old Risk: " & CStr(lRiskCnt) & " New Risk: " & CStr(lNewRiskCnt) & ")"
                        Throw New Exception(r_sMessage)
                    End If

                    'Initialise the Data Set with the Object/Properties

                    If m_oRenSelection.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed load data from xml (policy folder :" & v_lInsuranceFolderCnt & "Old Risk: " & CStr(lRiskCnt) & " New Risk: " & CStr(lNewRiskCnt) & ")"
                        Throw New Exception(r_sMessage)
                    End If

                    'save gis data for new risk

                    If m_oRenSelection.GIS_SaveToDB(v_sGisDataModelCode:=sGISDataModelCode) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to save gis data for new risk (policy folder :" & v_lInsuranceFolderCnt & "Old Risk: " & CStr(lRiskCnt) & " New Risk: " & CStr(lNewRiskCnt) & ")"
                        Throw New Exception(r_sMessage)
                    End If

                    'get policy binder for old policy version

                    If m_oRenSelection.GetPolicyBinderId(v_sDataModelCode:=sGISDataModelCode, v_lGISPolicyLinkId:=lGISPolicyLinkID, r_lPolicyBinderId:=lPolicyBinderID) <> gPMConstants.PMEReturnCode.PMTrue Then

                        r_sMessage = "Failed to get policy binder for old policy version (gis policy link: " & lGISPolicyLinkID & ")"
                        Throw New Exception(r_sMessage)
                    End If

                    'get policy binder for new policy version

                    If m_oRenSelection.GetPolicyBinderId(v_sDataModelCode:=sGISDataModelCode, v_lGISPolicyLinkId:=lNewGisPolicyLinkID, r_lPolicyBinderId:=lNewPolicyBinderID) <> gPMConstants.PMEReturnCode.PMTrue Then

                        r_sMessage = "Failed to get policy binder for new policy version (gis policy link: " & lNewGisPolicyLinkID & ")"
                        Throw New Exception(r_sMessage)
                    End If

                    'copy standard wording

                    If m_oRenSelection.CopyRiskStandardWordings(v_lOldPolicyBinderId:=lPolicyBinderID, v_lNewPolicyBinderId:=lNewPolicyBinderID, v_sDataModelCode:=sGISDataModelCode) <> gPMConstants.PMEReturnCode.PMTrue Then

                        r_sMessage = "Failed to copy standard wording (Old Policy Binder: " & lPolicyBinderID & " New Policy Binder: " & CStr(lNewPolicyBinderID) & ")"
                        Throw New Exception(r_sMessage)
                    End If

                    'do we need to do index linking
                    Dim auxVar As Object = vRiskArray(ACFieldPosGisScreenID, lCount)


                    If Not (Convert.IsDBNull(auxVar) Or IsNothing(auxVar)) Then
                        'Note: the risk_id and v_lInsuranceFileCnt are not being used so it doesn't matter what we pass in
                        'this is because further up the code we already load up gis info

                        If m_oRenSelection.GisIndexLink(v_lInsuranceFileCnt:=0, v_lRiskID:=0, v_vGisScreenID:=vRiskArray(ACFieldPosGisScreenID, lCount), v_dtEffectiveDate:=v_dtPolicyStartDate, v_sGisDataModelCode:=sGISDataModelCode) <> gPMConstants.PMEReturnCode.PMTrue Then

                            r_sMessage = "Failed index linking for policy: " & v_lNewInsuranceFileCnt
                            Throw New Exception(r_sMessage)
                        End If
                    End If

                    'copy gis related sum insured

                    If m_oRiskData.CopyRSASumInsured(v_lOldPolicyLinkID:=lGISPolicyLinkID, v_lNewPolicyLinkID:=lNewGisPolicyLinkID) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to copy gis related sum insured (Old policy link:" & lGISPolicyLinkID & " New policy link:" & CStr(lNewGisPolicyLinkID) & ")"
                        Throw New Exception(r_sMessage)
                    End If

                    'Note : rating section will only work when original risk on a policy is of type (2,5) and status of policy is (3,4,5)
                    'do peril allocation for new policy version
                    With m_oPerilAllocation

                        .InsuranceFileCnt = v_lNewInsuranceFileCnt

                        .InsuranceFolderCnt = v_lInsuranceFolderCnt

                        .RiskID = lNewRiskCnt

                        .TransactionType = v_sTransactionType
                    End With

                    'populate rating sections

                    If m_oPerilAllocation.PopulateRatingSections() <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to populate rating sections for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & CStr(lNewRiskCnt) & ")"
                        Throw New Exception(r_sMessage)
                    End If

                    If lInsuranceFileType = 6 OrElse lInsuranceFileType = 8 OrElse lInsuranceFileType = 9 Then
                        m_oPerilAllocation.ProRataRate = 1
                    End If

                    If lInsuranceFileType = 3 OrElse lInsuranceFileType = 5 OrElse lInsuranceFileType = 6 OrElse lInsuranceFileType = 8 OrElse lInsuranceFileType = 9 Then
                        If CopyRatings(v_lInsuranceFileCnt, lRiskCnt, lNewRiskCnt, m_oPerilAllocation.ProRataRate, v_lNewInsuranceFileCnt, lInsuranceFileType) <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to populate rating sections for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & lNewRiskCnt & ")"
                            Throw New Exception(r_sMessage)
                        End If
                    End If

                    'update risk with premium and suminsured
                    If lInsuranceFileType <> 5 And lInsuranceFileType <> 6 And lInsuranceFileType <> 8 Then
                        If m_oPerilAllocation.UpdateRisk() <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to update risk's premium and suminsured for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & CStr(lNewRiskCnt) & ")"
                            Throw New Exception(r_sMessage)
                        End If
                    End If


                    'do risk tax
                    m_oTax.RiskCnt = lNewRiskCnt
                    If m_oTax.GetRiskTax(r_vRiskTax:=vRiskTax, r_sDescription:=sDescription) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to do risk tax for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & CStr(lNewRiskCnt) & ")"
                        Throw New Exception(r_sMessage)
                    End If

                    If lInsuranceFileType = 2 Then
                        If DeleteOriginal(lNewRiskCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to delete rating sections for Policy: " & v_lNewInsuranceFileCnt & " Risk: " & lNewRiskCnt & ")"
                            Throw New Exception(r_sMessage)
                        End If
                    End If

                    '**************** Note on reinsurance *******************
                    'reinsurance will work out which RI model is relevant for this risk
                    'if it can't find one it will return false

                    ' ensure that the m_oReinsurance.Task property is not set to PMView
                    ' as the CalculateRI wont be run.

                    m_oReinsurance.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vTransactionType:=v_sTransactionType)

                    'get ready to do reinsurance (risk level)

                    m_oReinsurance.InsuranceFileCnt = v_lNewInsuranceFileCnt

                    m_oReinsurance.RiskID = lNewRiskCnt

                    'generate reinsurance for new policy version

                    If m_oReinsurance.CalculateRI() <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to generate RI for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & CStr(lNewRiskCnt) & ")"
                        Throw New Exception(r_sMessage)
                    End If

                    'Note : load and save reinsurance details to fix any roundings
                    'get reinsurance details

                    If m_oReinsurance.GetDetails() <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to reinsurance details for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & CStr(lNewRiskCnt) & ")"
                        Throw New Exception(r_sMessage)
                    End If

                    'save new reinsurance details

                    If m_oReinsurance.Update() <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to reinsurance details for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & CStr(lNewRiskCnt) & ")"
                        Throw New Exception(r_sMessage)
                    End If

                    'do we have valid reinsurance bands ie adds up to 100%
                    If v_bIgnoreError = False Then
                        If m_oReinsurance.ValidateBands(r_lValid:=lValidRIBand, r_lBand:=lReinsBand) <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to validate RI bands for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & CStr(lNewRiskCnt) & ")"
                            Throw New Exception(r_sMessage)
                        End If
                    End If

                    'do risk tax

                    m_oTax.RiskCnt = lNewRiskCnt

                    If m_oTax.GetRiskTax(r_vRiskTax:=vRiskTax, r_sDescription:=sDescription) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to do risk tax for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & CStr(lNewRiskCnt) & ")"
                        Throw New Exception(r_sMessage)
                    End If

                    'do we have valid bands ie adds up to 100%
                    If lValidRIBand = gPMConstants.PMEReturnCode.PMFalse And v_bIgnoreError = False Then

                        'Note: for now we just check to see if any of the original deferred bands been moved onto live band.
                        ' in the future we'll have to check for any changes in the bands
                        ' this is a big job and involve changes in reinsurance as well

                        'how many band are on deferred ri model for new risk
                        If GetRiskDeferredRIBand(v_lRiskCnt:=lNewRiskCnt, r_lDeferredRIBand:=lDeferredRIBandNewRisk) <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to get RI model for old risk (Policy: " & v_lInsuranceFileCnt & " Risk: " & CStr(lRiskCnt) & ")"
                            Throw New Exception(r_sMessage)
                        End If

                        'has any of the deferred band on this new risk moved to live ri model
                        If lDeferredRIBandNewRisk <> lDeferredRIBand Then
                            'change old risk status to quoted so it won't get pick up again

                            If m_oRiskData.UpdateRiskStatus(v_lRiskCnt:=lRiskCnt, v_lRiskStatusID:=0, v_sRiskStatusCode:="QUOTED") <> gPMConstants.PMEReturnCode.PMTrue Then
                                r_sMessage = "Failed to update old risk status to quoted (Risk: " & lRiskCnt & ")"
                                Throw New Exception(r_sMessage)
                            End If

                            'NOTE: after peril allocation risk_status_id = 8 (pending resinsurance) if everything is ok
                            '      reinsurance (business side) shouldn't change this risk status so we are expecting pending reinsurance
                            'get risk status for new risk
                            If GetRiskStatus(v_lRiskCnt:=lNewRiskCnt, r_lRiskStatusID:=lRiskStatusID, r_sRiskStatusCode:=sRiskStatusCode) <> gPMConstants.PMEReturnCode.PMTrue Then
                                r_sMessage = "Failed to get status for new risk (Risk: " & lNewRiskCnt & ")"
                                Throw New Exception(r_sMessage)
                            End If

                            If sRiskStatusCode.Trim().ToUpper() = "PENDINGRI" Then 'pending reinsurance
                                'if we still have deferred band on this risk then set it to RIDEFERRED
                                sRiskStatusCode = IIf(lDeferredRIBandNewRisk = 0, "QUOTED", "RIDEFERRED")

                                'update risk status

                                If m_oRiskData.UpdateRiskStatus(v_lRiskCnt:=lNewRiskCnt, v_lRiskStatusID:=0, v_sRiskStatusCode:=sRiskStatusCode) <> gPMConstants.PMEReturnCode.PMTrue Then
                                    r_sMessage = "Failed to update new risk status to quoted (Risk: " & lNewRiskCnt & ")"
                                    Throw New Exception(r_sMessage)
                                End If
                            Else
                                r_sMessage = "Peril Allocation Failed or more questions need answering (Risk: " & lNewRiskCnt & ")"
                                Throw New Exception(r_sMessage)
                            End If

                            'move claims to new risk
                            'maintain/payment of claim will sort out reinsurance when this claim is picked up
                            If MoveClaimToNewRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCnt:=lRiskCnt, v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_lNewRiskCnt:=lNewRiskCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                                r_sMessage = "Failed to move claims to new risk  (old Policy: " & v_lInsuranceFileCnt & " old Risk: " & CStr(lRiskCnt) &
                                             " new policy: " & CStr(v_lNewInsuranceFileCnt) & " new risk: " & CStr(lNewRiskCnt) & ")"
                                Throw New Exception(r_sMessage)
                            End If

                            If (aClaimsArray IsNot Nothing) Then
                                For lClaimsCount = 0 To UBound(aClaimsArray, 2)
                                    If aClaimsArray(2, lClaimsCount) = lRiskCnt Then
                                        aClaimsArray(3, lClaimsCount) = lNewRiskCnt
                                    End If
                                Next
                            End If


                            'save changes to risk
                            If m_oDatabase.SQLCommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                                r_sMessage = "Failed to commit risk changes SQLCommitTrans()"
                                Throw New Exception(r_sMessage)
                            End If

                            lTransactionStarted = gPMConstants.PMEReturnCode.PMFalse

                            'set this flag to stop policy from rolling back
                            lCommitPolicy = gPMConstants.PMEReturnCode.PMTrue
                        Else
                            'none of the bands move to live model
                            'rollback changes on risk
                            If m_oDatabase.SQLRollbackTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                                r_sMessage = "Failed SQLRollbackTrans()"
                                Throw New Exception(r_sMessage)
                            End If

                            lTransactionStarted = gPMConstants.PMEReturnCode.PMFalse
                            lCommitPolicy = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If 'lDeferredRIBandNewRisk <> lDeferredRIBand
                    ElseIf v_bIgnoreError = False Then
                        'bands are not add up to 100%
                        r_sMessage = "Invalid RI Bands ie bands do not add up to 100%"
                        Throw New Exception(r_sMessage)
                    ElseIf v_bIgnoreError = True Then
                        If m_oDatabase.SQLCommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to commit risk changes SQLCommitTrans()"
                            Throw New Exception(r_sMessage)
                        End If
                    End If

                End If 'lDeferredRIBand = 0

            Next lCount

            'rollback policy if none of new risks are on live ri model
            If lCommitPolicy <> gPMConstants.PMEReturnCode.PMTrue And v_bIgnoreError = False Then

                'PN49053
                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to add Parameter InsuranceFileCnt"
                    Throw New Exception(r_sMessage)
                End If

                vResultArray = Nothing
                sInsuranceRef = ""
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetInsuranceRefSQL, sSQLName:=kGetInsuranceRefName, bStoredProcedure:=kInsertInsFileDefRIUsageStored, vResultArray:=vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to get Insurance Ref: SQLSelect"
                    Throw New Exception(r_sMessage)
                End If

                If Information.IsArray(vResultArray) Then

                    sInsuranceRef = CStr(vResultArray(0, 0)).Trim()
                End If

                r_sMessage = "All risks are still on deferred model" & " Insurance Ref : " & sInsuranceRef
                Throw New Exception(r_sMessage)
            End If

            Return lReturnValue

        Catch ex As Exception
            If lTransactionStarted = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
            End If
            lReturnValue = gPMConstants.PMEReturnCode.PMFalse
            If r_sMessage = "" Then
                r_sMessage = Information.Err().Description
            End If
            r_sMessage = r_sMessage & Strings.Chr(13) & Strings.Chr(10) & "Failed in CopyDefRIRisks()"

        Finally


        End Try
        Return lReturnValue
    End Function

    '***********************************************************************************************************************
    ' Name : GetRiskRIModel
    '
    ' Desc : get number of bands on deferred ri model for this risk
    '
    ' Author : Thinh Nguyen 27/02/2004
    '***********************************************************************************************************************
    Private Function GetRiskDeferredRIBand(ByVal v_lRiskCnt As Integer, ByRef r_lDeferredRIBand As Integer) As Integer

        Dim sMessage As String = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim vResultArray(,) As Object

        lReturnValue = gPMConstants.PMEReturnCode.PMTrue

        vResultArray = Nothing

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="RiskCnt", vValue:=CStr(v_lRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add risk param"
            Return lReturnValue
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetDeferredRIBandSQL, sSQLName:=kGetDeferredRIBandName, bStoredProcedure:=kGetDeferredRIBandStored, vResultArray:=vResultArray, bKeepNulls:=True)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to get risk RI model"
            Return lReturnValue
        End If

        If Information.IsArray(vResultArray) Then
            r_lDeferredRIBand = gPMFunctions.NullToLong(vResultArray(0, 0))
        End If

        Return lReturnValue
    End Function

    '***********************************************************************************************************************
    ' Name : MoveClaimToNewRisk
    '
    ' Desc : move all associate claims to new risk
    '
    ' Author : Thinh Nguyen 27/02/2004
    '***********************************************************************************************************************
    Private Function MoveClaimToNewRisk(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_lNewRiskCnt As Integer) As Integer

        Dim sMessage As String = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode

        lReturnValue = gPMConstants.PMEReturnCode.PMTrue
        sMessage = ""

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add insurance file cnt param"
            Return lReturnValue
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="RiskCnt", vValue:=CStr(v_lRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add risk cnt param"
            Return lReturnValue
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=CStr(v_lNewInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add new insurance file cnt param"
            Return lReturnValue
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="NewRiskCnt", vValue:=CStr(v_lNewRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add new risk cnt param"
            Return lReturnValue
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=kMoveClaimToNewRiskSQL, sSQLName:=kMoveClaimToNewRiskName, bStoredProcedure:=kMoveClaimToNewRiskStored)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed SQLAction - move claims to new risk"
            Return lReturnValue
        End If

        Return lReturnValue
    End Function

    '***********************************************************************************************************************
    ' Name : CreateAndPostStats
    '
    ' Desc : create and post stats to orion
    '
    ' Author : Thinh Nguyen 27/02/2004
    '***********************************************************************************************************************
    Public Function CreateAndPostStats(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sTransactionType As String, ByRef r_sMessage As String) As Integer

        Dim lReturnValue As gPMConstants.PMEReturnCode

        Try
            r_sMessage = ""
            lReturnValue = gPMConstants.PMEReturnCode.PMTrue


            m_oControlTrans.InsuranceFileCnt = v_lInsuranceFileCnt


            m_lReturn = m_oControlTrans.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=v_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to set process mode for bControlTrans.Automated"
                Throw New Exception(r_sMessage)
            End If


            If m_oControlTrans.Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to create and post stats"
                Throw New Exception(r_sMessage)
            End If


        Catch ex As Exception
            lReturnValue = gPMConstants.PMEReturnCode.PMFalse

            'let calling function log this message
            If r_sMessage = "" Then
                r_sMessage = Information.Err().Description
            End If

            r_sMessage = r_sMessage & Strings.Chr(13) & Strings.Chr(10) & "Failed in CreateAndPostStats()"


        Finally



        End Try
        Return lReturnValue
    End Function

    '***********************************************************************************************************************
    ' Name : GetDeferredRIPolicy
    '
    ' Desc : get all policies which are in Insurance_File_Deferred_RI_Usage (policies which failed auto-deferred batch)
    '
    ' Author : Thinh Nguyen 20/09/2004
    '***********************************************************************************************************************
    Public Function GetDeferredRIPolicy(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.SQLSelect(sSQL:=kSelDeferredRIPolicyForAmendSQL, sSQLName:=kSelDeferredRIPolicyForAmendName, bStoredProcedure:=kSelDeferredRIPolicyForAmendStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)


        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get policies for manual deferred reinsurance", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDeferredRIPolicy", excep:=ex)

        Finally

        End Try
        Return result
    End Function

    '***********************************************************************************************************************
    ' Name : CopyPolicyHeader
    '
    ' Desc : create new version of policy and copy relevant policy level data
    '
    ' Author : Thinh Nguyen 20/09/2004
    '***********************************************************************************************************************
    Public Function CopyPolicyHeader(ByVal nInsuranceFileCnt As Integer, ByRef nNewInsurancefileCnt As Integer, ByRef r_sMessage As String, Optional ByRef nInsuranceFolderCnt As Integer = 0, Optional ByRef dtPolicyStartDate As Date = #12/30/1899#, Optional ByVal sSetOldInsuranceFileStatus As String = "", Optional ByVal sSetNewInsuranceFileStatus As String = "", Optional ByVal sTransactionType As String = "", Optional ByRef nPartyCnt As Integer = 0, Optional ByRef sInsStatus As String = "") As Integer

        Dim nresult As Integer = 0
        Dim sDescription As String = "" 'this is not used. just need to call tax function
        Dim vInsuranceFileTax As Object 'this is not used. just need to call tax function
        Dim nOldInsuranceFileStatusId As Integer
        Try

            nresult = gPMConstants.PMEReturnCode.PMTrue

            '****************** Create new version of policy ************************
            'assign current insurance file count to business object

            m_oInsuranceFile.InsuranceFileCnt = nInsuranceFileCnt
            nOldInsuranceFileStatusId = m_oInsuranceFile.InsuranceFileStatusID
            ' get details of current policy

            If m_oInsuranceFile.GetDetails() <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to get policy details for " & nInsuranceFileCnt
                Throw New Exception(r_sMessage)
            End If

            ' change old policy version status if required
            If sSetOldInsuranceFileStatus <> "" Then

                m_oInsuranceFile.InsuranceFileStatus = sSetOldInsuranceFileStatus
            End If


            m_oInsuranceFile.UpdatePolicy()
            nPartyCnt = m_oInsuranceFile.InsuredCnt

            m_oInsuranceFile.InsuranceFileStatusID = nOldInsuranceFileStatusId 'set policy status to as it is 

            'set last transaction type if required
            If sTransactionType <> "" Then

                m_oInsuranceFile.LastTransType = sTransactionType
            End If


            m_oInsuranceFile.EventDescription = "Policy copied for deferred reinsurance processing"
            '''' Find Max Policy Version        
            Dim vArray As Object
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_file_cnt", vValue:=nInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                CopyPolicyHeader = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetMaxVersionSQL, sSQLName:=kGetMaxVersionName, bStoredProcedure:=kGetMaxVersionStored, vResultArray:=vArray)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                CopyPolicyHeader = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If IsArray(vArray) Then
                m_oInsuranceFile.PolicyVersion = ToSafeLong(vArray(0, 0)) + 1
            Else
                m_oInsuranceFile.PolicyVersion += 1
            End If

            If m_oInsuranceFile.InsuranceFileTypeID <> 3 Then m_oInsuranceFile.ThisPremium = 0
            If m_oInsuranceFile.InsuranceFileTypeID <> 3 Then m_oInsuranceFile.NetPremium = 0

            ' create new version of policy based on current policy details

            If m_oInsuranceFile.CreatePolicy() <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to create new version of policy for " & nInsuranceFileCnt
                Throw New Exception(r_sMessage)
            End If

            m_oInsuranceFile.UpdatePolicy()

            '******************** Get new policy version details ******************************************

            nNewInsurancefileCnt = m_oInsuranceFile.InsuranceFileCnt

            'do we need to return start date?
            If Not False Then

                dtPolicyStartDate = m_oInsuranceFile.CoverStartDate
            End If

            'do we need to return insurance_folder_cnt?
            If Not False Then

                nInsuranceFolderCnt = m_oInsuranceFile.InsuranceFolderCnt 'note folder will be the same as previous policy version
            End If

            '****************** Copy related data to new policy version ***********************
            'copy standard wording

            If m_oRenSelection.CopyPolicyStandardWordings(v_lOldInsuranceFileCnt:=nInsuranceFileCnt, v_lNewInsuranceFileCnt:=nNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to copy standard wording to new policy version (current policy :" & nInsuranceFileCnt & " new policy " & CStr(nNewInsurancefileCnt) & " )"
                Throw New Exception(r_sMessage)
            End If

            'copy coinsurance

            If m_oRenSelection.CopyCoinsurance(v_lCurrentInsFileCnt:=nInsuranceFileCnt, v_lNewInsFileCnt:=nNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to copy coinsurance details to new policy version (current policy :" & nInsuranceFileCnt & " new policy " & CStr(nNewInsurancefileCnt) & " )"
                Throw New Exception(r_sMessage)
            End If

            'copy agent commission

            If m_oRenSelection.CopyAgentCommission(v_lCurrentInsFileCnt:=nInsuranceFileCnt, v_lNewInsFileCnt:=nNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to copy agent commission details to new policy version (current policy :" & nInsuranceFileCnt & " new policy " & CStr(nNewInsurancefileCnt) & " )"
                Throw New Exception(r_sMessage)
            End If

            'copy agents

            If m_oRenSelection.CopyInsuranceFileAgent(v_lCurrentInsFileCnt:=nInsuranceFileCnt, v_lNewInsFileCnt:=nNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to copy agent details to new policy version (current policy :" & nInsuranceFileCnt & " new policy " & CStr(nNewInsurancefileCnt) & " )"
                Throw New Exception(r_sMessage)
            End If

            '************** Policy level reinsurance is not supported at present **************************
            '    'copy policy level reinsurance
            '    m_oReinsurance.InsuranceFileCnt = r_lNewInsurancefileCnt
            '    m_oReinsurance.RiskId = 0
            '
            '    If m_oReinsurance.GetDetails() <> PMTrue Then
            '        r_sMessage = "Failed to copy policy level reinsurance to new policy version (current policy :" & v_lInsuranceFileCnt & " new policy " & r_lNewInsurancefileCnt & " )"
            '        GoTo Err_ProcessSingleDefRIPolicy
            '    End If

            ' copy policy level tax
            ' Must set the Task in Tax as this is passed into stored procedures as Mode. If it is wrong the new tax records will not be created.

            If m_oTax.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to SetProcessModes() to Tax object"
                Throw New Exception(r_sMessage)
            End If


            m_oTax.InsuranceFileCnt = nNewInsurancefileCnt

            If m_oTax.GetInsuranceFileTax(r_vInsuranceFileTax:=vInsuranceFileTax, r_sDescription:=sDescription) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to copy policy level tax to new policy version (current policy :" & nInsuranceFileCnt & " new policy " & CStr(nNewInsurancefileCnt) & " )"
                Throw New Exception(r_sMessage)
            End If

            'copy policy associates'
            m_lReturn = CopyPolicyAssociates(oldInsuranceFileCnt:=nInsuranceFileCnt, newInsuranceFileCnt:=nNewInsurancefileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nresult = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPolicyAssociates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyAssociates")

                Return nresult
            End If

        Catch ex As Exception

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy policy insurance_file_cnt = " & nInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyHeader", excep:=ex)

        Finally

        End Try
        Return nresult
    End Function

    '***********************************************************************************************************************
    ' Name : RelinkRisk
    '
    ' Desc : link risks from old policy version to new policy version (insurance_file_risk_link)
    '
    ' Author : Thinh Nguyen 20/09/2004
    '***********************************************************************************************************************
    Public Function RelinkRisk(ByVal v_lOldInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "INSERT INTO insurance_file_risk_link (" & Strings.Chr(13) & Strings.Chr(10) & "insurance_file_cnt," & Strings.Chr(13) & Strings.Chr(10) & "risk_cnt," & Strings.Chr(13) & Strings.Chr(10) &
                   "status_flag, " & Strings.Chr(13) & Strings.Chr(10) &
                   "original_risk_cnt)" & Strings.Chr(13) & Strings.Chr(10) &
                   "SELECT " & CStr(v_lNewInsuranceFileCnt) & "," & Strings.Chr(13) & Strings.Chr(10) &
                   "risk_cnt," & Strings.Chr(13) & Strings.Chr(10) &
                   "'U'," & Strings.Chr(13) & Strings.Chr(10) &
                   "null" & Strings.Chr(13) & Strings.Chr(10) &
                   "FROM insurance_file_risk_link" & Strings.Chr(13) & Strings.Chr(10) &
                   "WHERE insurance_file_cnt = " & CStr(v_lOldInsuranceFileCnt) & Strings.Chr(13) & Strings.Chr(10) &
                   "AND status_flag <> 'D'" & Strings.Chr(13) & Strings.Chr(10)


            m_oDatabase.Parameters.Clear()

            If m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Link old risks to new policy version", bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to relink risks to new policy version", vApp:=ACApp, vClass:=ACClass, vMethod:="RelinkRisk", excep:=ex)

        Finally



        End Try
        Return result
    End Function

    '***********************************************************************************************************************
    ' Name : DeletePolicy
    '
    ' Desc : delete this policy version and all dependant data
    '        if one of the step failed the whole thing will rollback
    ' Author : Thinh Nguyen 20/09/2004
    '***********************************************************************************************************************
    Public Function DeletePolicy(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="nInsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            result = m_oDatabase.SQLAction(sSQL:=kDeletePolicySQL, sSQLName:=kDeletePolicyName, bStoredProcedure:=kDeletePolicyStored)


        Catch ex As Exception

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete policy version - " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePolicy", excep:=ex)

        Finally



        End Try
        Return result
    End Function

    '***********************************************************************************************************************
    ' Name : SetPolicyStatus
    '
    ' Desc : change policy status
    '
    ' Author : Thinh Nguyen 20/09/2004
    '***********************************************************************************************************************
    Public Function SetPolicyStatus(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFileStatusID As Integer, Optional ByVal v_bStartTransaction As Boolean = True, Optional ByRef r_sFailureMessage As String = "GGGGGRRRRRR") As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "UPDATE Insurance_File SET insurance_file_status_id = {InsuranceFileStatusID}" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "WHERE insurance_file_cnt = {InsuranceFileCnt}"

            If v_bStartTransaction Then
                If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                    If r_sFailureMessage <> "GGGGGRRRRRR" Then
                        r_sFailureMessage = "Failed to start transaction"
                    End If

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If

            m_oDatabase.Parameters.Clear()


            'refer Developer Guide No. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileStatusID", vValue:=IIf(v_lInsuranceFileStatusID = 0, DBNull.Value, CStr(v_lInsuranceFileStatusID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to add parameter to PMDAO (InsuranceFileStatusID)"
                End If

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to add parameter to PMDAO (InsuranceFileCnt)"
                End If

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Change Policy Status", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to update policy status"
                End If

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            If v_bStartTransaction Then
                If m_oDatabase.SQLCommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                    If r_sFailureMessage <> "GGGGGRRRRRR" Then
                        r_sFailureMessage = "Failed to commit transactions"
                    End If

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If



        Catch ex As Exception

            If r_sFailureMessage <> "GGGGGRRRRRR" Then
                r_sFailureMessage = "SetPolicyStatus() - Errored"
            End If

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get renewals which await notice print", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPolicyStatus", excep:=ex)

        Finally

            If v_bStartTransaction Then
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = m_oDatabase.SQLRollbackTrans()
                End If
            End If


        End Try
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: DeleteInsFileDeferredRIUsage
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-12-2004 : PN15678
    ' ***************************************************************** '
    Public Function DeleteInsFileDeferredRIUsage(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DeleteInsFileDeferredRIUsage"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            lReturn = CType(AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute Action Query
            lReturn = m_oDatabase.SQLAction(sSQL:=kDeleteInsFileDeferredRIUsageSQL, sSQLName:=kDeleteInsFileDeferredRIUsageName, bStoredProcedure:=True)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kDeleteInsFileDeferredRIUsageSQL, "Failed", gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: AddInputParameter
    '
    ' Parameters: v_sName   : Parameter Name
    '             v_vValue  : Parameter Value
    '             v_iType   : Parameter DataType
    '
    ' Description: Adds an input parameter to the database parameters
    '
    ' History:
    '           Created : MEvans : 18-12-2002 : 202
    ' ***************************************************************** '
    Private Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddInputParameter"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object

        If m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType) <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error.
            result = gPMConstants.PMEReturnCode.PMFalse


            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to add parameter name:" & v_sName &
                                      ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)

        End If

        Return result

    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE:   UpdateRiskLink
    ' AUTHOR:      AMB
    ' DATE:        06 Dec 2013
    ' DESCRIPTION: Update Risk Link. set is_risk_edited = 1. Since this is automated process and need to
    'similarly run as deffered Amend process.
    'PM032518
    ' ---------------------------------------------------------------------------
    Public Function UpdateRiskLink(ByVal v_lRiskCnt As Integer) As Integer

        Dim Result As Integer = 0
        Try
            ' set the params
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=v_lRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=kUpdateRiskLinkSQL, sSQLName:=kUpdateRiskLinkName, bStoredProcedure:=kUpdateRiskLinkStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception
            Result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRiskLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskLink", excep:=excep)
            Return Result

        End Try
    End Function

    'JP
    Private Function TransferClaimToNewRisk(ByVal nInsuranceFileCnt As Integer, ByVal nNewInsuranceFileCnt As Integer, ByRef r_sMessage As String, Optional ByVal nPartyCnt As Integer = 0) As Integer

        Dim nResult As Integer = 0
        Dim sMessage As String
        Dim nReturnValue As Integer

        Dim oFindClaim As New bCLMFindClaim.Business
        Dim nNewClaimId As Integer
        Dim aNewClaimArray(,) As Object
        Dim nClaimsCount As Integer
        Dim nEventCnt As Integer
        Const sDescription As String = "Claims regenerated for Deferred RI Transfer"

        Const kMethodName As String = "TransferClaimToNewRisk"




        TransferClaimToNewRisk = gPMConstants.PMEReturnCode.PMTrue
        m_lReturn = oFindClaim.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        'm_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oFindClaim, v_sClassName:="bCLMFindClaim.Business", v_sCallingAppName:=m_sCallingAppName, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to create an instance of bCLMFindClaim.Business"
            TransferClaimToNewRisk = gPMConstants.PMEReturnCode.PMFalse

        End If
        If IsArray(aClaimsArray) Then
            For nClaimsCount = 0 To UBound(aClaimsArray, 2)
                If ToSafeLong(aClaimsArray(4, nClaimsCount), 0) > 0 Then
                    'Copy the claim
                    nReturnValue = oFindClaim.SetProcessModes(vTransactionType:="C_CR")
                    nReturnValue = oFindClaim.ProcessCopyClaim(v_lClaimId:=aClaimsArray(4, nClaimsCount), r_lCopyClaimId:=nNewClaimId)
                    If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to Copy claim for Claim Id " & aClaimsArray(4, nClaimsCount)
                        TransferClaimToNewRisk = gPMConstants.PMEReturnCode.PMFalse

                    End If
                    'log event on sucessfull
                    If TransferClaimToNewRisk = gPMConstants.PMEReturnCode.PMTrue Then
                        'create event log
                        m_lReturn = CreateEvent(nEventCnt:=nEventCnt, nPartyCnt:=nPartyCnt, nInsuranceFolderCnt:=Nothing, nInsuranceFileCnt:=nInsuranceFileCnt, nClaimCnt:=nNewClaimId, nDocumentCnt:=Nothing, nOldAddressCnt:=Nothing, nNewAddressCnt:=Nothing, nCampaignId:=Nothing, nDocumentTypeId:=Nothing, nReportTypeId:=Nothing, nEventTypeId:=3, dtEventDate:=Date.Now, sDescription:=sDescription)


                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create event log", vApp:=ACApp, vClass:=ACClass, vMethod:="TransferClaimToNewRisk", excep:=Err.GetException)
                        End If

                    End If
                    'Process Claim Perils and Reserves
                    nReturnValue = ProcessClaimPerils(nclaimCnt:=nNewClaimId, nPreDeferred_RITransfer:=0, nInsuranceFileCnt:=nNewInsuranceFileCnt, nIsCreated:=1)
                    If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to NetOf Perils for Claim" & nNewClaimId
                        TransferClaimToNewRisk = gPMConstants.PMEReturnCode.PMFalse

                    End If
                End If
            Next
        End If


    End Function

    Private Function ProcessClaimPerils(ByVal nclaimCnt As Integer, ByVal nPreDeferred_RITransfer As Integer, ByVal nInsuranceFileCnt As Long, ByVal nIsCreated As Integer) As Integer

        Const kMethodName As String = "ProcessClaimPerils"
        Dim oControlTransClaims As New bControlTransClaims.Automated
        Dim r_vNetOfClaimPerils As Object
        Dim nCountPerils As Integer
        Dim nIsCloned As Integer
        Dim crThisRevision As Decimal
        Dim nStatsFolderCnt As Long
        Dim bAlreadyCreated As Boolean


        ProcessClaimPerils = gPMConstants.PMEReturnCode.PMTrue




        m_lReturn = GetNetOfClaimPerils(v_lClaimId:=nclaimCnt, r_vNetOfClaimPerils:=r_vNetOfClaimPerils)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ProcessClaimPerils = gPMConstants.PMEReturnCode.PMFalse
        End If

        If IsArray(r_vNetOfClaimPerils) Then
            For nCountPerils = 0 To UBound(r_vNetOfClaimPerils, 2)
                crThisRevision = 0

                m_lReturn = NetOfClaimPerils(nclaimCnt:=nclaimCnt, nClaimPerilId:=r_vNetOfClaimPerils(0, nCountPerils), nPreDeferred_RITransfer:=nPreDeferred_RITransfer, crThisRevision:=crThisRevision)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ProcessClaimPerils = gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Create gross line
                If crThisRevision <> 0 Then
                    If bAlreadyCreated = False Then
                        m_lReturn = AddClaimsStatsFolder(nInsuranceFileCnt:=nInsuranceFileCnt, sDebitCredit:="D", sDocumentComment:="Reserve for claim number " & nclaimCnt, nTransactionTypeId:=1, sTransactionTypeCode:="C_CR", nUserID:=m_iUserID, sUsername:=m_sUsername, nClaimId:=nclaimCnt, nDocumentTypeId:=41, nStatsFolderCnt:=nStatsFolderCnt)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ProcessClaimPerils = gPMConstants.PMEReturnCode.PMFalse
                        End If
                        bAlreadyCreated = True
                    End If

                    ' Add Stats Details
                    m_lReturn = AddClaimsStatsDetail(nStatsFolderCnt:=nStatsFolderCnt, nClaimId:=nclaimCnt, nClaimPerilId:=ToSafeLong(r_vNetOfClaimPerils(0, nCountPerils), 0), sStatsDetailType:="GRS", nClassOfBusiness:=ToSafeInteger(r_vNetOfClaimPerils(1, nCountPerils), 0), sClassOfBusinessCode:=ToSafeString(r_vNetOfClaimPerils(2, nCountPerils), ""), nRIPartyCnt:=0, sCreditAccountCode:="CLMRES" & ToSafeString(r_vNetOfClaimPerils(2, nCountPerils), ""), nRIPartyType:=0, v_dRISharePercent:=0, crTransactionAmount:=crThisRevision, nDocumentTypeId:=41)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ProcessClaimPerils = gPMConstants.PMEReturnCode.PMFalse
                    End If


                    'Process Claim Reinsurance
                    m_lReturn = ProcessClaimReinsurance(v_lclaimCnt:=nclaimCnt, v_iIsCreated:=nIsCreated)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'RaiseError "ProcessClaimReinsurance", "Failed to Process Reinsurance for Claim Id " & v_lclaimCnt
                        ProcessClaimPerils = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Accounting Raised only for RI
                    m_lReturn = oControlTransClaims.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                    'm_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oControlTransClaims, v_sClassName:="bControlTransClaims.Automated", v_sCallingAppName:=m_sCallingAppName, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'RaiseError "CreateBusinessObject", "Failed to Create Object bControlTransClaims.Automated"
                        ProcessClaimPerils = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If nStatsFolderCnt > 0 Then
                        oControlTransClaims.ClaimID = nclaimCnt
                        oControlTransClaims.DocumentTypeID = 41
                        oControlTransClaims.IsCloned = 1
                        m_lReturn = oControlTransClaims.CreateStatsForCoinsReins(ToSafeLong(nStatsFolderCnt, 0))
                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            ProcessClaimPerils = gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = FinaliseStats(nClaimId:=nclaimCnt, nTransactionTypeId:=1, sTransactionTypeCode:="C_CR", nStatsFolderCnt:=nStatsFolderCnt, nStatsSuppressed:=0)
                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            ProcessClaimPerils = gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_lReturn = oControlTransClaims.CreateTransactions(ToSafeInteger(nStatsFolderCnt, 0))
                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            ProcessClaimPerils = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                    m_lReturn = FinaliseClaimDetails(nClaimId:=nclaimCnt, sClaimVersionDescription:="Deferred_RI Claim Adjustment")
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'RaiseError "FinaliseClaimDetails", "Failed to FinaliseClaim for Claim Id " & v_lclaimCnt
                        ProcessClaimPerils = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = UpdateClaimIsDirtyFlag(nclaimCnt:=nclaimCnt, nIsDirty:=0)
                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        ProcessClaimPerils = gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            Next
        End If


        Return ProcessClaimPerils
    End Function

    Private Function NetOfClaimPerils(ByVal nclaimCnt As Integer, ByVal nClaimPerilId As Integer, ByVal nPreDeferred_RITransfer As Integer, ByRef crThisRevision As Decimal) As Integer

        Const kMethodName As String = "NetOfClaimPerils"



        NetOfClaimPerils = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            .Parameters.Clear()

            ' Add parameters
            m_lReturn = .Parameters.Add(sName:="claim_id", vValue:=nclaimCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                NetOfClaimPerils = m_lReturn
            End If

            m_lReturn = .Parameters.Add(sName:="claim_peril_id", vValue:=nClaimPerilId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                NetOfClaimPerils = m_lReturn
            End If

            .Parameters.Add("PrePortfolioTransfer", nPreDeferred_RITransfer, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                NetOfClaimPerils = m_lReturn
            End If

            .Parameters.Add("this_revision", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDecimal)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                NetOfClaimPerils = m_lReturn
            End If

            m_lReturn = .SQLAction(sSQL:=kNetOfClaimPerilReserveSQL, sSQLName:=kGetClaimPerilsName, bStoredProcedure:=kGetClaimPerilsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                NetOfClaimPerils = m_lReturn

            End If

            crThisRevision = ToSafeCurrency(.Parameters.Item("this_revision").Value, 0)
        End With




    End Function

    Private Function ProcessClaimReinsurance(ByVal v_lclaimCnt As Integer, ByVal v_iIsCreated As Integer) As Integer
        Dim oReinsurance As New bCLMReinsurance.Form
        Dim sMessage As String

        Const kMethodName As String = "ProcessClaimReinsurance"



        ProcessClaimReinsurance = gPMConstants.PMEReturnCode.PMTrue
        m_lReturn = oReinsurance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        'm_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oReinsurance, v_sClassName:="bCLMReinsurance.Form", v_sCallingAppName:=m_sCallingAppName, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to create an instance of bCLMReinsuranceRI2007.Business"
            ProcessClaimReinsurance = gPMConstants.PMEReturnCode.PMFalse

        End If

        If v_iIsCreated = 1 Then
            oReinsurance.IsCreated = 1
        End If
        oReinsurance.ClaimID = v_lclaimCnt
        oReinsurance.Task = gPMConstants.PMEComponentAction.PMEdit
        m_lReturn = oReinsurance.CalculateRI()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "bCLMReinsurance.CalculateRI Failed for " & v_lclaimCnt
            ProcessClaimReinsurance = gPMConstants.PMEReturnCode.PMFalse

        End If

        ProcessClaimReinsurance = gPMConstants.PMEReturnCode.PMTrue


    End Function

    Private Function GetAllClaimsOnPolicy(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Const kMethodName As String = "GetAllClaimsOnPolicy"



        GetAllClaimsOnPolicy = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        ' Add parameters
        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            GetAllClaimsOnPolicy = m_lReturn

        End If

        ' Get policies
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetAllClaimsOnRiskSQL, sSQLName:=kGetAllClaimsOnRiskName, bStoredProcedure:=kGetAllClaimsOnRiskStored, vResultArray:=r_vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            GetAllClaimsOnPolicy = m_lReturn

        End If

        GetAllClaimsOnPolicy = gPMConstants.PMEReturnCode.PMTrue



    End Function
    ' ***************************************************************** '
    ' Name: CreateEvent (Private)
    '
    ' Description: Create an event record.
    '
    ' History: JSB 19/06/2001 - Made function Public, to enable it to be called directly from GII wrapper
    '
    ' ***************************************************************** '
    Public Function CreateEvent(ByRef nEventCnt As Integer, ByVal nPartyCnt As Integer, ByVal nInsuranceFolderCnt As Integer, ByVal nInsuranceFileCnt As Integer, ByVal nClaimCnt As Integer, ByVal nDocumentCnt As Integer, ByVal nOldAddressCnt As Integer, ByVal nNewAddressCnt As Integer, ByVal nCampaignId As Integer, ByVal nDocumentTypeId As Integer, ByVal nReportTypeId As Integer, ByVal nEventTypeId As Integer, ByVal dtEventDate As Date, ByVal sDescription As String, Optional nIsmanualDescription As Integer = 0) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Try
            If oEvent Is Nothing Then
                ' oEvent = CreateObject("bSIREvent.Business")
                oEvent = New bSIREvent.Business
                m_lReturn = oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                ' Check for errors
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the event object", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Err.Number, vErrDesc:=Err.Description)
                    Return nResult
                End If
            End If

            ' Directly add the event
            m_lReturn = oEvent.DirectAdd(vEventCnt:=nEventCnt, vPartyCnt:=nPartyCnt, vInsuranceFolderCnt:=nInsuranceFolderCnt, vInsuranceFileCnt:=nInsuranceFileCnt, vClaimCnt:=nClaimCnt, vDocumentCnt:=nDocumentCnt, vOldAddressCnt:=nOldAddressCnt, vNewAddressCnt:=nNewAddressCnt, vCampaignId:=nCampaignId, vDocumentType:=nDocumentTypeId, vReportType:=nReportTypeId, vEventType:=nEventTypeId, vUserId:=m_iUserID, vEventDate:=dtEventDate, vDescription:=sDescription, v_vIsManualDescription:=nIsmanualDescription)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Err.Number, vErrDesc:=Err.Description)
                Return nResult
            End If

            Return nResult

        Catch excep As System.Exception
            ' Log Error Message
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", excep:=excep)
            Return CreateEvent
        End Try
        Return nResult

    End Function
    ''' <summary>
    ''' ClaimReversingPrePolicyTransfer
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="nPartyCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ClaimReversingPrePolicyTransfer(ByVal nInsuranceFileCnt As Integer, Optional ByVal nPartyCnt As Integer = 0) As Integer
        Dim oCLMFindClaimObj As New bCLMFindClaim.Business
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim nNewClaimId As Integer
        Dim nClaimsCount As Integer
        Dim nEventCnt As Integer
        Const sDescription As String = "Claims reversed for Deferred_RI Transfer"
        m_lReturn = oCLMFindClaimObj.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = gPMConstants.PMEReturnCode.PMFalse
        End If

        If GetAllClaimsOnPolicy(v_lInsuranceFileCnt:=nInsuranceFileCnt, r_vResultArray:=aClaimsArray) <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = gPMConstants.PMEReturnCode.PMFalse
        End If
        If IsArray(aClaimsArray) Then
            ' Process all claims
            For nClaimsCount = 0 To UBound(aClaimsArray, 2)
                ' Copy the claim by passing MAX ClaimId of Base Claim
                ' Question : Do we need to check Remaining Reservev before processing claims ?
                m_lReturn = oCLMFindClaimObj.SetProcessModes(vTransactionType:="C_CR")
                m_lReturn = oCLMFindClaimObj.ProcessCopyClaim(v_lClaimId:=aClaimsArray(1, nClaimsCount), r_lCopyClaimId:=nNewClaimId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'RaiseError "ProcessCopyClaim", "Failed to Copy claim for Claim Id " & m_vClaimsArray(0, lClaimsCount)
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                End If
                If nNewClaimId > 0 Then
                    aClaimsArray(4, nClaimsCount) = nNewClaimId
                End If
                'log event on sucessfull
                If nResult = gPMConstants.PMEReturnCode.PMTrue Then
                    'create event log
                    m_lReturn = CreateEvent(nEventCnt:=nEventCnt, nPartyCnt:=nPartyCnt, nInsuranceFolderCnt:=Nothing, nInsuranceFileCnt:=nInsuranceFileCnt, nClaimCnt:=nNewClaimId, nDocumentCnt:=Nothing, nOldAddressCnt:=Nothing, nNewAddressCnt:=Nothing, nCampaignId:=Nothing, nDocumentTypeId:=Nothing, nReportTypeId:=Nothing, nEventTypeId:=3, dtEventDate:=Date.Now, sDescription:=sDescription)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create event log", vApp:=ACApp, vClass:=ACClass, vMethod:="ClaimReversingPrePolicyTransfer", vErrNo:=Err.Number, vErrDesc:=Err.Description)
                        Return nResult
                    End If
                End If
                ' No need to MoveClaimToNewRisk
                'Process Claim Perils and Reserves
                m_lReturn = ProcessClaimPerils(nclaimCnt:=nNewClaimId, nPreDeferred_RITransfer:=1, nInsuranceFileCnt:=nInsuranceFileCnt, nIsCreated:=0)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                End If
            Next
        End If
        Return nResult
    End Function

    Private Function AddClaimsStatsFolder(ByVal nInsuranceFileCnt As Integer, ByVal sDebitCredit As String, ByVal sDocumentComment As String, ByVal nTransactionTypeId As Integer, ByVal sTransactionTypeCode As String, ByVal nUserID As Integer, ByVal sUsername As String, ByVal nClaimId As Integer, ByVal nDocumentTypeId As Integer, ByRef nStatsFolderCnt As Integer) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        m_oDatabase.Parameters.Clear()
        ' Add parameters
        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=nInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="debit_credit", vValue:=sDebitCredit, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="document_comment", vValue:=sDocumentComment, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_id", vValue:=nTransactionTypeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_code", vValue:=sTransactionTypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=nUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="user_name", vValue:=sUsername, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=nClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="documenttype_id", vValue:=nDocumentTypeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=kAddStatsFolderClaimsSQL, sSQLName:=kAddStatsFolderClaimsName, bStoredProcedure:=kAddStatsFolderClaimsStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        ' Output parameter
        nStatsFolderCnt = ToSafeInteger(m_oDatabase.Parameters.Item("stats_folder_cnt").Value, 0)


        Return nResult
    End Function

    Private Function GetNetOfClaimPerils(ByVal v_lClaimId As Integer, ByRef r_vNetOfClaimPerils As Object) As Integer
        GetNetOfClaimPerils = gPMConstants.PMEReturnCode.PMTrue
        m_oDatabase.Parameters.Clear()
        ' Add parameters
        m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=v_lClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            GetNetOfClaimPerils = m_lReturn
            Return GetNetOfClaimPerils
        End If
        ' Get policies
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetNetOfClaimPerilSQL, sSQLName:=kGetNetOfClaimPerilName, bStoredProcedure:=kGetNetOfClaimPerilStored, vResultArray:=r_vNetOfClaimPerils)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            GetNetOfClaimPerils = m_lReturn
            Return GetNetOfClaimPerils
        End If
        Return GetNetOfClaimPerils
    End Function

    Private Function AddClaimsStatsDetail(ByVal nStatsFolderCnt As Integer, ByVal nClaimId As Integer, ByVal nClaimPerilId As Integer,
                                          ByVal sStatsDetailType As String, ByVal nClassOfBusiness As Integer, ByVal sClassOfBusinessCode As String,
                                          ByVal nRIPartyCnt As Integer, ByVal sCreditAccountCode As String, ByVal nRIPartyType As Integer,
                                          ByVal v_dRISharePercent As Double, ByVal crTransactionAmount As Decimal, ByVal nDocumentTypeId As Integer) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        m_oDatabase.Parameters.Clear()
        ' Add parameters
        m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt", vValue:=nStatsFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=nClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="peril_id", vValue:=nClaimPerilId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_detail_type", vValue:=sStatsDetailType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="class_of_business_id", vValue:=nClassOfBusiness, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="class_of_business_code", vValue:=sClassOfBusinessCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="ri_party_cnt", vValue:=nRIPartyCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="ri_shortname", vValue:=sCreditAccountCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="ri_party_type", vValue:=nRIPartyType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="ri_share_percent", vValue:=v_dRISharePercent, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_amount", vValue:=crTransactionAmount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDecimal)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="documenttype_id", vValue:=nDocumentTypeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=kAddStatsDetailsClaimsSQL, sSQLName:=kAddStatsDetailsClaimsName, bStoredProcedure:=kAddStatsDetailsClaimsStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If
        Return nResult
    End Function
    ''' <summary>
    ''' Finalise Status
    ''' </summary>
    ''' <param name="nClaimId"></param>
    ''' <param name="nTransactionTypeId"></param>
    ''' <param name="sTransactionTypeCode"></param>
    ''' <param name="nStatsFolderCnt"></param>
    ''' <param name="nStatsSuppressed"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function FinaliseStats(ByVal nClaimId As Integer, ByVal nTransactionTypeId As Integer,
                                   ByVal sTransactionTypeCode As String, ByVal nStatsFolderCnt As Integer,
                                   ByVal nStatsSuppressed As Integer) As Integer
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        m_oDatabase.Parameters.Clear()
        ' Add parameters

        m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=nClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_id", vValue:=nTransactionTypeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_code", vValue:=sTransactionTypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt", vValue:=nStatsFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="nIsCloned", vValue:=1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="bstatssuppressed", vValue:=nStatsSuppressed, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_Deferred", vValue:=1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If
        m_lReturn = m_oDatabase.SQLAction(sSQL:=kUpdateClaimFinaliseStatsSQL, sSQLName:=kUpdateClaimFinaliseStatsName, bStoredProcedure:=kUpdateClaimFinaliseStatsStored)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'RaiseError "m_oDatabase.SQLAction", "Unable to run: " & ACUpdateClaimFinaliseStatsSQL
            nResult = m_lReturn
            Return nResult
        End If
        Return nResult
    End Function

    ''' <summary>
    ''' Update Claim IsDirty Flag
    ''' </summary>
    ''' <param name="nclaimCnt"></param>
    ''' <param name="nIsDirty"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateClaimIsDirtyFlag(ByVal nclaimCnt As Integer, ByVal nIsDirty As Integer) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        m_oDatabase.Parameters.Clear()
        ' Add parameters

        m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=nclaimCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_dirty", vValue:=nIsDirty, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=kUpdateClaimStatusSQL, sSQLName:=kUpdateClaimStatusName, bStoredProcedure:=kUpdateClaimStatusStored)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = m_lReturn
            Return nResult
        End If



        Return nResult
    End Function
    ''' <summary>
    ''' FinaliseClaimDetails
    ''' </summary>
    ''' <param name="nClaimId"></param>
    ''' <param name="sClaimVersionDescription"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FinaliseClaimDetails(ByVal nClaimId As Integer, ByVal sClaimVersionDescription As String) As Integer

        Const kMethodName As String = "FinaliseClaimDetails"
        Dim oChangeClaimStatus As New bCLMChangeClaimStatus.Business
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try
            m_lReturn = oChangeClaimStatus.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceId:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = m_lReturn
                Return nResult
            End If

            m_lReturn = oChangeClaimStatus.FinaliseClaimDetails(v_lClaimId:=nClaimId, v_sClaimVersionDescription:="Deferred RI Claim Adjustment")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = m_lReturn
                Return nResult
            End If

        Catch excep As System.Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=FinaliseClaimDetails, excep:=excep)
            ' If you want to rollback a transaction or something, do it here

        Finally
            oChangeClaimStatus.Dispose()
            oChangeClaimStatus = Nothing
        End Try
        Return nResult
    End Function
    ''' <summary>
    ''' Update Deferred for Renewel Status
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="nNewInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateDeferredRI_Renewal_Status(ByVal nInsuranceFileCnt As Integer, ByVal nNewInsuranceFileCnt As Integer) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Try
            m_oDatabase.Parameters.Clear()

            ' Add parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=nInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = m_lReturn
                Return nResult
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_insurance_file_cnt", vValue:=nNewInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = m_lReturn
                Return nResult
            End If

            ' Get policies
            m_lReturn = m_oDatabase.SQLAction(sSQL:=kUpdateDeferredRIRenewalStatusSQL, sSQLName:=kUpdateDeferredRIRenewalStatusName, bStoredProcedure:=kUpdateDeferredRIRenewalStatusStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = m_lReturn
                Return nResult
            End If

        Catch excep As System.Exception
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateDeferredRI_Renewal_Status Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GUpdateDeferredRI_Renewal_Status", excep:=excep)
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' All unedited Risks go through without any pro-rata
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="nOriginalRiskCnt"></param>
    ''' <param name="nRiskCnt"></param>
    ''' <param name="dProRataRate"></param>
    ''' <param name="nNew_InsuranceFileCnt"></param>
    ''' <param name="nInsuranceFileTypeId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CopyRatings(ByVal nInsuranceFileCnt As Integer, ByVal nOriginalRiskCnt As Integer, ByVal nRiskCnt As Integer, ByVal dProRataRate As Double, Optional ByVal nNew_InsuranceFileCnt As Integer = 0, Optional ByVal nInsuranceFileTypeId As Integer = 0) As Integer
        Dim nCnt1 As Integer
        Dim nCnt2 As Integer
        'Dim dProrataRate1 As Double
        Dim aResultArray2(,) As Object
        Dim iInsuranceFileCnt As Integer
        Try
            CopyRatings = gPMConstants.PMEReturnCode.PMTrue
            'dProrataRate1 = dProRataRate
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Cnt", vValue:=nRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.SQLAction(sSQL:=kDelPerilForDeletedRiskSQL, sSQLName:=kDelPerilForDeletedRiskName, bStoredProcedure:=kDelPerilForDeletedRiskStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRatings = gPMConstants.PMEReturnCode.PMFalse
                Return CopyRatings
            End If

            'Del Rating Sections
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Cnt", vValue:=nRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.SQLAction(sSQL:=kDelRatingSectionForDeletedRiskSQL, sSQLName:=kDelRatingSectionForDeletedRiskName, bStoredProcedure:=kDelRatingSectionForDeletedRiskStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRatings = gPMConstants.PMEReturnCode.PMFalse
                Return CopyRatings
            End If

            'Get Original Rating Sections
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=nInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_id", vValue:=nOriginalRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            'Fetch the records
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kSelectRatingSectionSQL, sSQLName:=kSelectRatingSectionName, bStoredProcedure:=kSelectRatingSectionStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=aResultArray2)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRatings = gPMConstants.PMEReturnCode.PMFalse
                Return CopyRatings
            End If

            If ToSafeInteger(nNew_InsuranceFileCnt) <> 0 Then
                iInsuranceFileCnt = nNew_InsuranceFileCnt
            Else
                iInsuranceFileCnt = nInsuranceFileCnt
            End If

            If IsArray(aResultArray2) Then

                nCnt1 = LBound(aResultArray2, 2)
                nCnt2 = UBound(aResultArray2, 2)

                For j As Integer = nCnt1 To nCnt2
                    m_lReturn = CopyRatingSectionsAndPerils(aResultArray2, 1, aResultArray2(13, j), iInsuranceFileCnt, nRiskCnt, j, dProRataRate)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        CopyRatings = gPMConstants.PMEReturnCode.PMFalse
                        Return CopyRatings
                    End If
                Next j

            End If

        Catch excep As System.Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UCopyRatings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRatings Failed", excep:=excep)
        Finally
        End Try
        Return CopyRatings
    End Function
    ''' <summary>
    ''' Copy Rating Section and Perils
    ''' </summary>
    ''' <param name="aResultArray"></param>
    ''' <param name="nThisPremiumSign"></param>
    ''' <param name="nOriginalFlag"></param>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="nRiskCnt"></param>
    ''' <param name="nIndex"></param>
    ''' <param name="dProrata"></param>
    ''' <param name="nPreviousRiskCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CopyRatingSectionsAndPerils(ByVal aResultArray(,) As Object, ByVal nThisPremiumSign As Integer, ByVal nOriginalFlag As Integer, ByVal nInsuranceFileCnt As Long, ByVal nRiskCnt As Integer, nIndex As Integer, Optional dProrata As Double = 0, Optional ByVal nPreviousRiskCnt As Integer = 0, Optional v_lInsuranceFileType As Integer = 0) As Integer
        Dim nPolicyRatingSectionTypeId As Integer
        Dim crThisPremium As Decimal
        Dim nInsuranceFileNoOfDp As Integer

        Try
            CopyRatingSectionsAndPerils = gPMConstants.PMEReturnCode.PMTrue

            nPolicyRatingSectionTypeId = -1
            If v_lInsuranceFileType = 8 Then
                crThisPremium = (nThisPremiumSign * (aResultArray(6, nIndex) * dProrata)) * -1
            Else
                crThisPremium = nThisPremiumSign * (aResultArray(6, nIndex) * dProrata)
            End If
            nInsuranceFileNoOfDp = 2

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="rating_section_type_id", vValue:=aResultArray(10, nIndex), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="policy_section_type_id", vValue:=nPolicyRatingSectionTypeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=nInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_id", vValue:=nRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="sum_insured", vValue:=aResultArray(4, nIndex), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDecimal)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="annual_rate", vValue:=aResultArray(3, nIndex), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDecimal)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="annual_premium", vValue:=aResultArray(6, nIndex), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDecimal)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="this_premium", vValue:=crThisPremium, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDecimal)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="rate_type_id", vValue:=aResultArray(12, nIndex), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_no_of_dp", vValue:=nInsuranceFileNoOfDp, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="original_flag", vValue:=nOriginalFlag, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="currency_id", vValue:=IIf(aResultArray(14, nIndex) = "", DBNull.Value, aResultArray(14, nIndex)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="country_id", vValue:=IIf(aResultArray(15, nIndex) = "", DBNull.Value, aResultArray(15, nIndex)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="state_id", vValue:=IIf(aResultArray(16, nIndex) = "", DBNull.Value, aResultArray(16, nIndex)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_amended", vValue:=aResultArray(17, nIndex), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="calculated_premium", vValue:=ToSafeCurrency(aResultArray(18, nIndex), 0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDecimal)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="override_reason", vValue:=aResultArray(19, nIndex), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            ' Add Section & Perils
            m_lReturn = m_oDatabase.SQLAction(sSQL:=kAddSectionAndPerilsSQL, sSQLName:=kAddSectionAndPerilsName, bStoredProcedure:=kAddSectionAndPerilsStored)
            Return m_lReturn

        Catch excep As System.Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRatingSectionsAndPerils Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRatingSectionsAndPerils", excep:=excep)
        End Try
        Return m_lReturn
    End Function
    ''' <summary>
    '''  get policies type
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="nInsuranceFileTypeId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPolicyType(ByVal nInsuranceFileCnt As Integer, ByRef nInsuranceFileTypeId As Integer) As Integer

        Dim aArray(,) As Object
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Const kMethodName As String = "GetPolicyType"

        Try
            ' set the params
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=nInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            ' get the risk status of the risk
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetInsuranceRefSQL, sSQLName:=kGetInsuranceRefName, bStoredProcedure:=kGetInsuranceRefStored, vResultArray:=aArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = m_lReturn
                Return nResult
            End If

            If IsArray(aArray) Then
                nInsuranceFileTypeId = NullToLong(aArray(1, 0))
            Else
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            nResult = gPMConstants.PMEReturnCode.PMTrue
            Return nResult

        Catch excep As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetPolicyType, excep:=excep)
            Return nResult
        Finally
        End Try
        Return nResult
    End Function

    Public Function GetPreviousRiskCnt(ByVal v_lPreInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByRef r_lPreviousRiskCnt As Integer) As Integer

        Dim dtResult As New DataTable

        Try

            GetPreviousRiskCnt = gPMConstants.PMEReturnCode.PMTrue

            r_lPreviousRiskCnt = 0
            'Send the new file in
            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "Insurance_File_Cnt", CLng(v_lPreInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "Risk_cnt", CLng(v_lRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "processed_Insurance_file_cnt", CLng(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Execute the SP
            m_lReturn = m_oDatabase.ExecuteDataTable(sSQL:=kGetPreviousRiskCntForTransferSQL, sSQLName:=kGetPreviousRiskCntForTransferName, bStoredProcedure:=True, oRecordset:=dtResult)

            'Determine the result
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GetPreviousRiskCnt = gPMConstants.PMEReturnCode.PMFalse
            ElseIf dtResult IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
                r_lPreviousRiskCnt = ToSafeInteger(dtResult.Rows(0)(0).ToString)
            End If

            Exit Function

        Catch excep As System.Exception
            GetPreviousRiskCnt = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetPreviousRiskCnt", r_lFunctionReturn:=GetPreviousRiskCnt)

            Exit Function
        End Try
    End Function
    Public Function CopyRisk(ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_vRiskDetail As Object, ByVal v_lPosNo As Integer, ByRef r_lRiskCnt As Integer, Optional ByVal v_lResetStatus As Integer = 0, Optional ByVal v_lCreateLinkType As Integer = 0, Optional ByVal v_bAutoCancellation As Boolean = False, Optional ByRef v_sRiskMergeStatus As String = "", Optional ByRef v_lOldRiskCnt As Integer = 0) As Integer

        Dim lRiskCnt As Integer

        Dim lOldRiskCnt As Integer
        Dim lNewRiskCnt As Integer

        Dim iIsAutoReinsured As Short
        Dim vArray As Object

        On Error GoTo Err_CopyRisk

        CopyRisk = gPMConstants.PMEReturnCode.PMTrue

        'UPGRADE_WARNING: Couldn't resolve default property of object v_vRiskDetail(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        lOldRiskCnt = v_vRiskDetail(0, v_lPosNo)

        'Tomo030801
        'This bit's here because we need to reset the auto reinsured flag to that
        'from the risk type.
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=v_vRiskDetail(4, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetAutoReinsuredSQL, sSQLName:=kGetAutoReinsuredName, bStoredProcedure:=kGetAutoReinsuredStored, vResultArray:=vArray)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        If IsArray(vArray) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object vArray(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iIsAutoReinsured = vArray(0, 0)
        Else
            iIsAutoReinsured = 0
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object v_vRiskDetail(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        v_vRiskDetail(20, v_lPosNo) = iIsAutoReinsured

        'UPGRADE_WARNING: Couldn't resolve default property of object vArray. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vArray = ""

        'Tomo030801 - End

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=0, idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'TN20010719 - start
        If v_lResetStatus = gPMConstants.PMEReturnCode.PMTrue Then
            'reset status to UnQuoted
            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_status_id", vValue:=4, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            'TN20010719 - end
        Else
            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_status_id", vValue:=v_vRiskDetail(1, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_folder_cnt", vValue:=v_vRiskDetail(2, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="accumulation_id", vValue:=v_vRiskDetail(3, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=v_vRiskDetail(4, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=v_vRiskDetail(5, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="sequence_number", vValue:=v_vRiskDetail(6, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="sum_insured_requested", vValue:=v_vRiskDetail(7, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="inception_date", vValue:=v_vRiskDetail(8, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="expiry_date", vValue:=v_vRiskDetail(9, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_not_index_linked", vValue:=v_vRiskDetail(10, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_accumulated", vValue:=v_vRiskDetail(11, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="lapsed_reason_id", vValue:=v_vRiskDetail(12, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="lapsed_date", vValue:=v_vRiskDetail(13, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="lapsed_description", vValue:=v_vRiskDetail(14, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="var_data_ref", vValue:=v_vRiskDetail(15, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="total_sum_insured", vValue:=v_vRiskDetail(16, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="total_annual_premium", vValue:=v_vRiskDetail(17, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="total_this_premium", vValue:=v_vRiskDetail(18, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_ri_at_risk_level", vValue:=v_vRiskDetail(19, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_auto_reinsured", vValue:=v_vRiskDetail(20, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_screen_id", vValue:=v_vRiskDetail(21, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="eml_percentage", vValue:=v_vRiskDetail(22, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        ' PW311002
        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_number", vValue:=v_vRiskDetail(23, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        ' PW021202
        m_lReturn = m_oDatabase.Parameters.Add(sName:="variation_number", vValue:=v_vRiskDetail(24, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        ' PW021202
        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_risk_selected", vValue:=v_vRiskDetail(25, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        ' PW311002
        m_lReturn = m_oDatabase.Parameters.Add(sName:="coverage", vValue:=v_vRiskDetail(26, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        ' PW311002
        m_lReturn = m_oDatabase.Parameters.Add(sName:="insured_item", vValue:=v_vRiskDetail(27, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        ' PW311002
        m_lReturn = m_oDatabase.Parameters.Add(sName:="extensions", vValue:=v_vRiskDetail(28, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="premium_this_year", vValue:=v_vRiskDetail(31, v_lPosNo), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        ' Sasria
        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_mandatory_risk", vValue:=ToSafeInteger(v_vRiskDetail(36, v_lPosNo), 0), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=kAddRiskSQL, sSQLName:=kAddRiskName, bStoredProcedure:=kAddRiskStored)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            CopyRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        lRiskCnt = m_oDatabase.Parameters.Item("risk_cnt").Value

        If v_bAutoCancellation = False Then

            'RWH(20/11/2000) Add link record into Insurance_file_risk_link.
            If (lRiskCnt <> 0) Then

                ' Determine whether or not to create a link and
                ' if so what kind of link
                Select Case v_lCreateLinkType

                    Case 0 ' standard - original and renewed risk cnt not populated
                        m_lReturn = m_oRiskData.AddRiskLink(v_lNewInsuranceFileCnt, lRiskCnt, "C", 0, 0)

                    Case 1 ' populate original_risk_cnt
                        m_lReturn = m_oRiskData.AddRiskLink(v_lNewInsuranceFileCnt, lRiskCnt, "C", v_lOldRiskCnt, 0)

                    Case 2 ' populate renewed_risk_cnt
                        m_lReturn = m_oRiskData.AddRiskLink(v_lNewInsuranceFileCnt, lRiskCnt, "C", 0, v_lOldRiskCnt)

                    Case Else
                        ' do nothing
                End Select

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                r_lRiskCnt = lRiskCnt

                m_lReturn = m_oRiskData.CopyRatingSection(v_lOldRiskCnt:=lOldRiskCnt, v_lNewRiskCnt:=lRiskCnt)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End If

        Else

            ' delete the original insurance file risk link
            m_lReturn = m_oRiskData.DeleteInsuranceFileRiskLink(v_lInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_lRiskCnt:=v_lOldRiskCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' add new
            m_lReturn = m_oRiskData.AddRiskLink(v_lNewInsuranceFileCnt, lRiskCnt, IIf(v_sRiskMergeStatus = "DP", "D", "C"), v_lOldRiskCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            r_lRiskCnt = lRiskCnt
            lNewRiskCnt = lRiskCnt

            m_lReturn = m_oRiskData.CopyRiskExtras(v_lOldRiskCnt:=v_lOldRiskCnt, v_lNewRiskCnt:=lNewRiskCnt)

            'sj 20/12/2002 - end
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

        End If

        Exit Function

Err_CopyRisk:

        CopyRisk = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRisk", vErrNo:=Err.Number, vErrDesc:=Err.Description)

        Exit Function

    End Function
    Public Function CopyRatings(ByRef v_lInsuranceFileCnt As Integer, ByRef r_lOriginalRiskCnt As Integer, ByRef r_lRiskCnt As Integer, Optional ByVal v_lInsuranceFileType As Integer = 0) As Object
        Dim r_vResultArray(,) As Object
        Dim r_vResultArray2(,) As Object
        Dim iCount1 As Short
        Dim iCount2 As Short
        Dim lCnt1 As Integer
        Dim lCnt2 As Integer
        Dim i As Short
        Dim j As Short
        Dim m_dProrataRate As Double
        Dim m_lInsFolderCnt As Integer
        Dim vArray As Object
        Dim vValue As Object
        Dim vArray1 As Object
        Dim dtold_cover_start_date As Date
        Dim dtold_expiry_date As Date
        Dim dtcover_start_date As Date
        Dim dtexpiry_date As Date
        Dim lProduct_id As Integer


        On Error GoTo Catch_Renamed

Try_Renamed:
        CopyRatings = gPMConstants.PMEReturnCode.PMTrue

        'All unedited Risks go through without any pro-rata
        m_dProrataRate = 1

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Cnt", vValue:=r_lRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLAction(sSQL:=kDelPerilSQL, sSQLName:=kDelPerilName, bStoredProcedure:=kDelPerilStored)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRatings = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If


        'Del Rating Sections
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Cnt", vValue:=r_lRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLAction(sSQL:=kDelRatingSectionSQL, sSQLName:=kDelRatingSectionName, bStoredProcedure:=kDelRatingSectionStored)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRatings = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'Get Original Rating Sections
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_id", vValue:=r_lOriginalRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        'Fetch the records
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=kSelectRatingSectionSQL, sSQLName:=kSelectRatingSectionName, bStoredProcedure:=kSelectRatingSectionStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray2)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRatings = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        If IsArray(r_vResultArray2) Then

            lCnt1 = LBound(r_vResultArray2, 2)
            lCnt2 = UBound(r_vResultArray2, 2)

            For j = lCnt1 To lCnt2
                m_lReturn = CopyRatingSectionsAndPerils(r_vResultArray2, 1, 0, v_lInsuranceFileCnt, r_lRiskCnt, j, m_dProrataRate, 0, v_lInsuranceFileType)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    CopyRatings = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            Next

        End If
        m_lReturn = UpdatePremium(r_lRiskCnt, m_dProrataRate, v_lInsuranceFileType)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyRatings = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If
        GoTo Finally_Renamed

Catch_Renamed:

        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRatings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRatings", vErrNo:=Err.Number, vErrDesc:=Err.Description)

Finally_Renamed:

        Exit Function
        Resume

    End Function
    Public Function CopyRatingSectionsAndPerils(ByVal r_vResultArray As Object, ByVal i_ThisPremiumSign As Short, ByVal i_OriginalFlag As Short, ByVal m_lInsuranceFileCnt As Integer, ByVal m_lRiskCnt As Integer, ByRef iIndex As Short, Optional ByRef dProrata As Double = 0) As Integer

        Dim m_lPolicyRatingSectionTypeId As Integer
        Dim m_cThisPremium As Decimal
        Dim m_cAnnualPremium As Decimal
        Dim m_lInsuranceFileNoOfDp As Integer

        On Error GoTo Err_CopyRatingSectionsAndPerils

        CopyRatingSectionsAndPerils = gPMConstants.PMEReturnCode.PMTrue

        m_lPolicyRatingSectionTypeId = -1
        If IsNothing(dProrata) Then
            m_cThisPremium = i_ThisPremiumSign * r_vResultArray(5, iIndex)
        Else
            m_cThisPremium = i_ThisPremiumSign * (r_vResultArray(5, iIndex) * dProrata)
        End If

        If r_vResultArray(6, iIndex) = 0 Then
            m_cAnnualPremium = i_ThisPremiumSign * r_vResultArray(5, iIndex)
        Else
            m_cAnnualPremium = r_vResultArray(6, iIndex)
        End If

        m_lInsuranceFileNoOfDp = 2

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="rating_section_type_id", vValue:=r_vResultArray(10, iIndex), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="policy_section_type_id", vValue:=m_lPolicyRatingSectionTypeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=m_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_id", vValue:=m_lRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="sum_insured", vValue:=r_vResultArray(4, iIndex), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="annual_rate", vValue:=r_vResultArray(3, iIndex), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="annual_premium", vValue:=m_cAnnualPremium, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="this_premium", vValue:=m_cThisPremium, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="rate_type_id", vValue:=r_vResultArray(12, iIndex), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_no_of_dp", vValue:=m_lInsuranceFileNoOfDp, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="original_flag", vValue:=i_OriginalFlag, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="currency_id", vValue:=IIf(r_vResultArray(14, iIndex) = "", System.DBNull.Value, r_vResultArray(14, iIndex)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="country_id", vValue:=IIf(r_vResultArray(15, iIndex) = "", System.DBNull.Value, r_vResultArray(15, iIndex)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="state_id", vValue:=IIf(r_vResultArray(16, iIndex) = "", System.DBNull.Value, r_vResultArray(16, iIndex)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_amended", vValue:=r_vResultArray(17, iIndex), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="calculated_premium", vValue:=ToSafeCurrency(r_vResultArray(18, iIndex), 0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="override_reason", vValue:=r_vResultArray(19, iIndex), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


        ' Add Section & Perils
        m_lReturn = m_oDatabase.SQLAction(sSQL:=kAddSectionAndPerilsSQL, sSQLName:=kAddSectionAndPerilsName, bStoredProcedure:=kAddSectionAndPerilsStored)


        Exit Function

Err_CopyRatingSectionsAndPerils:
        CopyRatingSectionsAndPerils = gPMConstants.PMEReturnCode.PMFalse

        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRatingSectionsAndPerils Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRatingSectionsAndPerils", vErrNo:=Err.Number, vErrDesc:=Err.Description)
    End Function



    Public Function UpdatePremium(ByVal v_lRiskCnt As Integer, ByVal v_bProRata As Double, Optional v_lInsuranceFileType As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            ' set the params
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(v_lRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pro_rata_rate", vValue:=v_bProRata, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_lInsuranceFileType = 8 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="UpdateRiskToZero", vValue:=1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If v_lInsuranceFileType = 5 And v_lInsuranceFileType = 6 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="UpdateRiskToZero", vValue:=2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            ' do the call to insert the record into Insurance_File_Deferred_RI_Usage
            m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_update_risk_values", sSQLName:="spu_update_risk_values", bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePremium Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskRIModel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Private Function CopyPolicyAssociates(ByRef oldInsuranceFileCnt As Integer, ByRef newInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sDate As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        ' GIS policy link id
        result = m_oDatabase.Parameters.Add(sName:="Old_Insurance_File_Cnt", vValue:=CStr(oldInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add OldInsuranceFileCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyAssociates")

            Return result
        End If

        ' GIS scheme id
        result = m_oDatabase.Parameters.Add(sName:="New_Insurance_File_Cnt", vValue:=CStr(newInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add NewInsuranceFileCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyAssociates")

            Return result
        End If

        ' Call the SQL
        result = m_oDatabase.SQLAction(sSQL:=ACCopyPolicyAssociatesSQL, sSQLName:=ACCopyPolicyAssociatesName, bStoredProcedure:=ACCopyPolicyAssociatesStored)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLAction Failed for " & ACCopyPolicyAssociatesSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyAssociates")

            Return result
        End If

        Return result

    End Function
    ''' <summary>
    ''' calculate RI for single quote
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="nIsValid"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function RecalculateRIQuote(
                                       ByVal nInsuranceFileCnt As Integer,
                                       ByRef nIsValid As Integer) As Integer

        Dim nResult As Integer
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue
            ' set the params
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="nInsurance_file_cnt",
                vValue:=nInsuranceFileCnt,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="nIs_valid",
                vValue:=1,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput,
                iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' get the risk status of the risk
            m_lReturn = m_oDatabase.SQLAction(
                sSQL:=kRecalculateRIQuoteSQL,
                sSQLName:=kRecalculateRIQuoteName,
                bStoredProcedure:=kRecalculateRIQuoteStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            nIsValid = Convert.ToInt16(m_oDatabase.Parameters.Item("nIs_valid").Value)

        Catch ex As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMTrue
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RecalculateRIQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RecalculateRIQuote", vErrNo:=Err.Number, vErrDesc:=ex.Message, excep:=ex)


        End Try
        Return nResult
    End Function

    Public Function DeleteOriginal(ByVal v_lRiskCnt As Integer) As Integer

        Dim result As Integer = 0

        Try

            ' set the params
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Cnt", vValue:=v_lRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=kDelOriginalPerilSQL, sSQLName:=kDelOriginalPerilName, bStoredProcedure:=kDelOriginalPerilStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                DeleteOriginal = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If


            'Del Rating Sections
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Cnt", vValue:=v_lRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=kDelOriginalRatingSectionSQL, sSQLName:=kDelOriginalRatingSectionName, bStoredProcedure:=kDelOriginalRatingSectionStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                DeleteOriginal = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePremium Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskRIModel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function


End Class



