Option Strict Off
Option Explicit On
Imports System.Data
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public Class RI2007DisabledBusiness
    Implements IDisposable

    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_nSourceID As Integer
    Private m_nLanguageID As Integer
    Private m_nCurrencyID As Integer
    Private m_nLogLevel As Integer
    ' ************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "RI2007DisabledBusiness"

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_nCurrentRecord As Integer

    ' Error Code (Private)
    Private m_nReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_nTask As Integer
    Private m_nNavigate As Integer
    Private m_nProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Need for ProcessSinglePolicy()
    Private m_oRenSelection As bSIRRenSelection.Business
    Private m_oInsuranceFile As bSIRInsuranceFile.Services
    Private m_oReinsurance As Object
    Private m_oRiskData As bSIRRiskData.Business
    Private m_oPerilAllocation As Object
    Private m_oControlTrans As Object

    Private m_oTax As Object

    Private m_oValue As Object
    Private m_bIsRI2007Enabled As Boolean
    Private m_oClaimsArray(,) As Object = Nothing
    Private m_oReinsuranceClaim As Object

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get
            Return m_nTask
        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get
            Return m_nNavigate
        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get
            Return m_nProcessMode
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
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                CloseBusinessObjects()
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub

    ''' <summary>
    ''' Entry point for any initialisation code for this
    '''              object.
    ''' </summary>
    ''' <param name="sUsername"></param>
    ''' <param name="sPassword"></param>
    ''' <param name="iUserID"></param>
    ''' <param name="iSourceID"></param>
    ''' <param name="iLanguageID"></param>
    ''' <param name="iCurrencyID"></param>
    ''' <param name="iLogLevel"></param>
    ''' <param name="sCallingAppName"></param>
    ''' <param name="bStandAlone"></param>
    ''' <param name="vDatabase"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_nLanguageID = iLanguageID
            m_nSourceID = iSourceID
            m_nCurrencyID = iCurrencyID
            m_nLogLevel = iLogLevel



            nResult = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_nSourceID, m_nLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_nCurrentRecord = 0
            m_nProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create relevant business objects to do ProcessSinglePolicy()
            nResult = CType(CreateBusinessObjects(), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function
    ''' <summary>
    ''' Set the optional process modes.
    ''' </summary>
    ''' <param name="vTask"></param>
    ''' <param name="vNavigate"></param>
    ''' <param name="vProcessMode"></param>
    ''' <param name="vTransactionType"></param>
    ''' <param name="vEffectiveDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.

            If Not Informations.IsNothing(vTask) Then
                m_nTask = CInt(vTask)
            End If

            If Not Informations.IsNothing(vNavigate) Then
                m_nNavigate = CInt(vNavigate)
            End If

            If Not Informations.IsNothing(vProcessMode) Then
                m_nProcessMode = CInt(vProcessMode)
            End If

            If Not Informations.IsNothing(vTransactionType) Then
                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then
                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' GetPoliciesPortfolioTransferRI2007Off
    ''' </summary>
    ''' <param name="v_nProductID"></param>
    ''' <param name="v_dtTransferDate"></param>
    ''' <param name="r_oPolicyArray"></param>
    ''' <returns></returns>
    Public Function GetPoliciesPortfolioTransfer(ByVal v_nProductID As Integer, ByVal v_dtTransferDate As Date, ByRef r_oPolicyArray(,) As Object) As Integer

        Dim nResult As Integer = 0
        Try

            m_oDatabase.Parameters.Clear()

            ' Add parameters

            nResult = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=v_nProductID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = m_oDatabase.Parameters.Add(sName:="transfer_date", vValue:=CDate(v_dtTransferDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            ' Get policies

            nResult = m_oDatabase.SQLSelect(sSQL:=kGetPoliciesForPortfolioTransferRI2007DisabledSQL, sSQLName:=kGetPoliciesForPortfolioTransferRI2007DisabledName, bStoredProcedure:=kGetPoliciesForPortfolioTransferRI2007DisabledStored, vResultArray:=r_oPolicyArray)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPoliciesPortfolioTransfer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPoliciesPortfolioTransfer", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' Get the 'risk_status' of a risk
    ''' </summary>
    ''' <param name="v_lRiskCnt"></param>
    ''' <param name="r_lRiskStatusID"></param>
    ''' <param name="r_sRiskStatusCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRiskStatus(ByVal v_lRiskCnt As Integer, ByRef r_lRiskStatusID As Integer, ByRef r_sRiskStatusCode As String) As Integer

        Dim oDeferredRIArray(,) As Object = Nothing

        Try

            ' Set the params
            m_oDatabase.Parameters.Clear()

            m_nReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=v_lRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the risk status of the risk

            m_nReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskStatusSQL, sSQLName:=ACGetRiskStatusName, bStoredProcedure:=ACGetRiskStatusStored, vResultArray:=oDeferredRIArray)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_nReturn
            End If

            If Informations.IsArray(oDeferredRIArray) Then
                r_lRiskStatusID = gPMFunctions.NullToLong(oDeferredRIArray(0, 0))
                r_sRiskStatusCode = gPMFunctions.NullToString(oDeferredRIArray(1, 0))
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError
        End Try
    End Function

    ''' <summary>
    ''' ProcessSinglePolicy
    ''' </summary>
    ''' <param name="v_nInsuranceFileCnt"></param>
    ''' <param name="v_dtTransferDate"></param>
    ''' <param name="r_sMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessSinglePolicy(ByVal v_nInsuranceFileCnt As Integer, ByVal v_dtTransferDate As Date,
                                        ByRef r_sMessage As String, Optional ByRef v_nNewInsuranceFileCnt As Integer = 0,
                                        Optional v_bIgnoreClaims As Boolean = False, Optional ByRef r_bPendingRI As Boolean = False) As Integer

        Dim nReturnValue As gPMConstants.PMEReturnCode
        Dim nNewInsurancefileCnt As Integer
        Dim nInsuranceFolderCnt As Integer
        Dim dtPolicyStartDate As Date
        Dim sDescription As String = "" = ""
        Dim sTransactionType As String = ""
        Dim sMessage As String = ""
        Dim sInsuranceRef As String
        Dim nMaxPolicyVersion As Integer
        Dim nOldInsuranceFileStatusId As Integer
        Try

            nReturnValue = gPMConstants.PMEReturnCode.PMTrue

            sTransactionType = "PT"

            ' Set transaction so we can roll back if something gone wrong

            If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                nReturnValue = gPMConstants.PMEReturnCode.PMFalse
                r_sMessage = "Failed to begin SQLTransaction"
                Return nReturnValue
            End If

            '****************** Create new version of policy ************************

            ' Assign current insurance file count to business object

            m_oInsuranceFile.InsuranceFileCnt = v_nInsuranceFileCnt

            ' Get details of current policy

            If m_oInsuranceFile.GetDetails() <> gPMConstants.PMEReturnCode.PMTrue Then
                nReturnValue = gPMConstants.PMEReturnCode.PMFalse
                r_sMessage = "Failed to get policy details for " & v_nInsuranceFileCnt
                Return nReturnValue
            End If

            'PM040269, PM040469 Do not process under renewal and new renewal policy versions.
            If m_oInsuranceFile.insurancefilestatusid = 3 Then
                nReturnValue = m_oDatabase.SQLCommitTrans

                If nReturnValue <> PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to commit SQLTransaction"
                    Return nReturnValue
                End If

                Return nReturnValue
            End If

            ' Set old version to "replaced portfolio transfer"
            nOldInsuranceFileStatusId = m_oInsuranceFile.insurancefilestatusid

            m_oInsuranceFile.InsuranceFileStatus = gSIRLibrary.SIRInsFileStatusReplacedPT

            m_oInsuranceFile.UpdatePolicy()

            If nOldInsuranceFileStatusId = 1 OrElse nOldInsuranceFileStatusId = 2 Then
                m_oInsuranceFile.insurancefilestatusid = nOldInsuranceFileStatusId
            Else
                m_oInsuranceFile.InsuranceFileStatus = Nothing   'set policy status to live
            End If

            m_oInsuranceFile.ThisPremium = 0
            m_oInsuranceFile.NetPremium = 0
            m_oInsuranceFile.EventDescription = "Policy copied for portfolio transfer"
            nReturnValue = GetMaxPolicyVersion(v_nInsuranceFileCnt, nMaxPolicyVersion)
            If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to get Max Policy Version for InsuranceFile " & v_nInsuranceFileCnt
                Return nReturnValue
            End If
            m_oInsuranceFile.PolicyVersion = nMaxPolicyVersion + 1

            m_oInsuranceFile.LastTransType = sTransactionType

            m_oInsuranceFile.CoverStartDate = v_dtTransferDate

            ' E007 Changes : Reversal of claims
            If Not v_bIgnoreClaims Then
                m_oClaimsArray = Nothing
                nReturnValue = ClaimReversingPrePolicyTransfer(v_nInsuranceFileCnt:=v_nInsuranceFileCnt)
                If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to run ClaimReversingPrePolicyTransfer " & v_nInsuranceFileCnt
                    Return nReturnValue
                End If
            End If

            ' create new version of policy based on current policy details
            If m_oInsuranceFile.CreatePolicy() <> gPMConstants.PMEReturnCode.PMTrue Then
                nReturnValue = gPMConstants.PMEReturnCode.PMFalse
                r_sMessage = "Failed to create new version of policy for " & v_nInsuranceFileCnt
                Return nReturnValue
            End If

            '******************** Get new policy version details ******************************************

            nNewInsurancefileCnt = m_oInsuranceFile.InsuranceFileCnt

            nInsuranceFolderCnt = m_oInsuranceFile.InsuranceFolderCnt 'note folder will be the same as previous policy version

            dtPolicyStartDate = m_oInsuranceFile.CoverStartDate

            sInsuranceRef = m_oInsuranceFile.InsuranceRef

            '****************** Copy related data to new policy version ***********************
            ' Copy standard wording

            If m_oRenSelection.CopyPolicyStandardWordings(v_lOldInsuranceFileCnt:=v_nInsuranceFileCnt, v_lNewInsuranceFileCnt:=nNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                nReturnValue = gPMConstants.PMEReturnCode.PMFalse
                r_sMessage = "Failed to copy standard wording to new policy version (current policy :" & v_nInsuranceFileCnt & " new policy " & CStr(nNewInsurancefileCnt) & " )"
                Return nReturnValue
            End If

            ' Copy coinsurance

            If m_oRenSelection.CopyCoinsurance(v_lCurrentInsFileCnt:=v_nInsuranceFileCnt, v_lNewInsFileCnt:=nNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                nReturnValue = gPMConstants.PMEReturnCode.PMFalse
                r_sMessage = "Failed to copy coinsurance details to new policy version (current policy :" & v_nInsuranceFileCnt & " new policy " & CStr(nNewInsurancefileCnt) & " )"
                Return nReturnValue
            End If

            ' Copy agent commission

            If m_oRenSelection.CopyAgentCommission(v_lCurrentInsFileCnt:=v_nInsuranceFileCnt, v_lNewInsFileCnt:=nNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                nReturnValue = gPMConstants.PMEReturnCode.PMFalse
                r_sMessage = "Failed to copy agent commission details to new policy version (current policy :" & v_nInsuranceFileCnt & " new policy " & CStr(nNewInsurancefileCnt) & " )"
                Return nReturnValue
            End If

            ' Copy agents

            If m_oRenSelection.CopyInsuranceFileAgent(v_lCurrentInsFileCnt:=v_nInsuranceFileCnt, v_lNewInsFileCnt:=nNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                nReturnValue = gPMConstants.PMEReturnCode.PMFalse
                r_sMessage = "Failed to copy agent details to new policy version (current policy :" & v_nInsuranceFileCnt & " new policy " & CStr(nNewInsurancefileCnt) & " )"
                Return nReturnValue
            End If


            r_bPendingRI = False
            ' Copy risk details (error messages are dealt within CopyAllRisk())
            If CopyAllRisk(v_nInsuranceFileCnt:=v_nInsuranceFileCnt, v_nNewInsuranceFileCnt:=nNewInsurancefileCnt, v_nInsuranceFolderCnt:=nInsuranceFolderCnt, v_dtPolicyStartDate:=dtPolicyStartDate, v_sTransactionType:=sTransactionType, r_sMessage:=r_sMessage, v_dtTransferDate:=CDate(v_dtTransferDate), v_sInsuranceRef:=sInsuranceRef, r_bPendingRI:=r_bPendingRI) <> gPMConstants.PMEReturnCode.PMTrue Then
                nReturnValue = gPMConstants.PMEReturnCode.PMFalse
                r_sMessage = "Failed to copy risk details to new policy version (current policy :" & v_nInsuranceFileCnt & " new policy " & CStr(nNewInsurancefileCnt) & " )"
                Return nReturnValue
            End If

            If Not r_bPendingRI Then

                ' Process stats (error messages are dealt within CreateAndPostStats())

                If m_oInsuranceFile.InsuranceFileType = "POLICY" OrElse m_oInsuranceFile.InsuranceFileType = "MTA PERM" OrElse m_oInsuranceFile.InsuranceFileType = "MTA TEMP" OrElse m_oInsuranceFile.InsuranceFileType = "MTACAN" OrElse m_oInsuranceFile.InsuranceFileType = "MTAREINS" Then
                    If CreateAndPostStats(v_lInsuranceFileCnt:=nNewInsurancefileCnt, v_sTransactionType:=sTransactionType, v_lPTInsuranceFileCnt:=v_nInsuranceFileCnt, v_bReversePT:=m_oInsuranceFile.InsuranceFileType <> "POLICY", r_sMessage:=r_sMessage) <> gPMConstants.PMEReturnCode.PMTrue Then
                        nReturnValue = gPMConstants.PMEReturnCode.PMFalse
                        r_sMessage = "Failed to create stats for policy version (current policy :" & v_nInsuranceFileCnt & " new policy " & CStr(nNewInsurancefileCnt) & " )"
                        Return nReturnValue
                    End If
                End If

                If Not v_bIgnoreClaims Then
                    ' E007 Changes
                    nReturnValue = TransferClaimToNewRisk(v_nInsuranceFileCnt:=v_nInsuranceFileCnt,
                                        v_nNewInsuranceFileCnt:=nNewInsurancefileCnt,
                                        r_sMessage:=sMessage)
                    If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return nReturnValue
                    End If
                End If

            End If

            v_nNewInsuranceFileCnt = nNewInsurancefileCnt

            nReturnValue = m_oDatabase.SQLCommitTrans

            If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to commit SQLTransaction"
                Return PMEReturnCode.PMFalse
            End If


        Catch ex As Exception

            ' Roll back all transactions as one of the step has failed


            nReturnValue = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(r_sMessage = "", "Failed ProcessSinglePolicy()", r_sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSinglePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
        Finally

            If nReturnValue = gPMConstants.PMEReturnCode.PMFalse Then
                nReturnValue = m_oDatabase.SQLRollbackTrans
            End If
        End Try


        Return nReturnValue

    End Function

    '***********************************************************************************************************************
    ' Name :    CreateBusinessObjects
    ' Desc :    Create required objects for ProcessSinglePolicy
    ' Author :  Alix Bergeret - 08/07/2004
    '***********************************************************************************************************************
    Private Function CreateBusinessObjects() As Integer

        Dim result As Integer = 0
        Dim sMessage As String = "" = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            sMessage = ""

            m_oRenSelection = New bSIRRenSelection.Business
            m_nReturn = m_oRenSelection.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_nSourceID, iLanguageID:=m_nLanguageID, iCurrencyID:=m_nCurrencyID, iLogLevel:=m_nLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRRenSelection.Business"
                Throw New Exception()
            End If

            m_oInsuranceFile = New bSIRInsuranceFile.Services
            m_nReturn = m_oInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_nSourceID, iLanguageID:=m_nLanguageID, iCurrencyID:=m_nCurrencyID, iLogLevel:=m_nLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRInsuranceFile.Services"
                Throw New Exception()
            End If

            ' E007 Portfolio Transfer - Kuljeet Kaur
            m_nReturn = bPMFunc.getProductOptionValue(v_sUsername:="",
                            v_sPassword:="", v_iUserID:=0,
                            v_iMainSourceID:=0, v_iLanguageID:=0,
                            v_iCurrencyID:=0, v_iLogLevel:=0,
                            v_sCallingAppName:="",
                            v_vOptionNumber:=SIRHiddenOptions.SIROPTEnableRI2007,
                            v_vBranch:=m_nSourceID,
                            r_vUnderwriting:=m_oValue)
            If m_oValue = "1" Then
                m_bIsRI2007Enabled = True
            Else
                m_bIsRI2007Enabled = False
            End If

            If m_bIsRI2007Enabled Then
                m_nReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oReinsurance, v_sClassName:="bSIRReinsuranceRI2007.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID, v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            Else
                m_nReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oReinsurance, v_sClassName:="bSIRReinsurance.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID, v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            End If

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRReinsurance.Form"
                Throw New Exception()
            End If

            If gPMComponentServices.CreateBusinessObject(r_oObject:=m_oTax, v_sClassName:="bSIRRITax.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID, v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel, v_oDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRRITax.business"
                Throw New Exception()
            End If

            'm_nReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oRiskData, v_sClassName:="bSIRRiskData.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID, v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            'If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            '    sMessage = "Failed to create an instance of bSIRRiskData.Business"

            'Throw New Exception()
            'End If

            m_nReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oPerilAllocation, v_sClassName:="bSirPerilAllocation.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID, v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRPerilAllocation.Business"
                Throw New Exception()
            End If

            m_nReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oControlTrans, v_sClassName:="bControlTrans.Automated", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID, v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bControlTrans.Automated"
                Throw New Exception()
            End If

            m_nReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oReinsuranceClaim,
                                                        v_sClassName:=If(m_bIsRI2007Enabled,
    "bCLMReinsuranceRI2007.Form", "bCLMReinsurance.Form"),
                                                        v_sCallingAppName:=ACApp,
                                                        v_sUsername:=m_sUsername,
                                                        v_sPassword:=m_sPassword,
                                                        v_iUserID:=m_iUserID,
                                                        v_iSourceID:=m_nSourceID,
                                                        v_iLanguageID:=m_nLanguageID,
                                                        v_iCurrencyID:=m_nCurrencyID,
                                                        v_iLogLevel:=m_nLogLevel,
                                                        v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_bIsRI2007Enabled = True Then
                    sMessage = "Failed to create an instance of bCLMReinsuranceRI2007.Form"
                Else
                    sMessage = "Failed to create an instance of bCLMReinsurance.Form"
                End If
                Throw New Exception()
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed CreateBusinessObjects", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObjects", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            CloseBusinessObjects()

            Return result
        End Try
    End Function

    ''' <summary>
    ''' Close objects open by CreateBusinessObjects()
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CloseBusinessObjects()

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

                m_nReturn = m_oReinsurance.Dispose()
                m_oReinsurance = Nothing
            End If

            If Not (m_oRiskData Is Nothing) Then

                m_oRiskData.Dispose()
                m_oRiskData = Nothing
            End If

            If Not (m_oPerilAllocation Is Nothing) Then

                m_nReturn = m_oPerilAllocation.Dispose()
                m_oPerilAllocation = Nothing
            End If

            If Not (m_oControlTrans Is Nothing) Then

                m_nReturn = m_oControlTrans.Dispose()
                m_oControlTrans = Nothing
            End If
            If (Not (m_oReinsuranceClaim Is Nothing)) Then
                m_nReturn = m_oReinsuranceClaim.Dispose()
                m_oReinsuranceClaim = Nothing
            End If

        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Sub

    ''' <summary>
    ''' Copy related risks details to new policy version
    '''if risk is marked for portfolio transfer then we need to
    '''begin transaction (risk)
    ''' 1. copy risk
    ''' 2. rerate (populate rating sections)
    ''' 3. apply reinsurance
    ''' 4. apply tax - now that risk has been successfully processed
    ''' 5. if valid bands
    '''if new risk is on live RI model (ie one of the bands is now on live model)
    '''5.1 change old risk to quoted so it won't be pick up next time
    '''5.2 change new risk to quoted if current status is pending reinsurance
    '''5.3 move claims to new risk
    '''5.4 if everything is ok then commit risk changes
    '''5.5 set commit policy flag to true
    '''else risk still on deferred model
    '''rollback transaction (risk)
    '''5.1 relink risk to new policy version
    '''5.2 move claims to new policy version same risk
    '''else
    '''1. create a new link in insurance_file_risk_link with status_flag = "U" (unchanged)
    '''2. move claims to new version of policy (same risk)
    '''endif
    '''if commit policy flag is not set then rollback policy (will be done all calling function)
    ''' </summary>
    ''' <param name="v_nInsuranceFileCnt"></param>
    ''' <param name="v_nNewInsuranceFileCnt"></param>
    ''' <param name="v_nInsuranceFolderCnt"></param>
    ''' <param name="v_dtPolicyStartDate"></param>
    ''' <param name="v_sTransactionType"></param>
    ''' <param name="r_sMessage"></param>
    ''' <param name="v_dtTransferDate"></param>
    ''' <param name="v_bIgnoreError"></param>
    ''' <param name="v_sInsuranceRef"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CopyAllRisk(ByVal v_nInsuranceFileCnt As Integer, ByVal v_nNewInsuranceFileCnt As Integer, ByVal v_nInsuranceFolderCnt As Integer,
                                 ByVal v_dtPolicyStartDate As Date, ByVal v_sTransactionType As String, ByRef r_sMessage As String, ByVal v_dtTransferDate As Date,
                                 Optional v_bIgnoreError As Boolean = False, Optional ByVal v_sInsuranceRef As String = "", Optional ByRef r_bPendingRI As Boolean = False) As Integer

        Dim nReturnValue As gPMConstants.PMEReturnCode
        Dim oRiskArray As Object = Nothing
        Dim oGISPolicyLinkArray As Object = Nothing
        Dim nRiskCnt As Integer
        Dim nNewRiskCnt As Integer
        Dim sGISDataModelCode As String = ""
        Dim nGISPolicyLinkID As Integer
        Dim nNewGisPolicyLinkID As Integer
        Dim nPolicyBinderID As Integer
        Dim nNewPolicyBinderID As Integer
        Dim sXMLDataSetDef As String = ""
        Dim sXMLDataSet As String = ""
        Dim nValidRIBand As Object
        Dim nReinsBand As Object = 0 ' Not used - function parameter required
        Dim sDescription As String = "" = ""
        Dim nRiskStatusID As Integer
        Dim sRiskStatusCode As String = ""
        Dim nCommitPolicy As gPMConstants.PMEReturnCode ' Set to true when one of the deferred risk is now on live model and everything is ok
        Dim bMarkedForTransfer As Boolean
        Dim nDeferredRIBandNewRisk As Integer ' Number of bands which are on deferred RI model
        Dim nTransactionStarted As gPMConstants.PMEReturnCode ' Set to pmtrue when m_oDatabase.SQLBeginTrans() is called
        Dim nClaimsCount As Long
        Dim nInsuranceFileType As Long
        Dim nPreviousRiskCnt As Long
        Dim dProRataRate As Double
        Const kFieldPosRiskID As Integer = 0
        Const kFieldPosGisScreenID As Integer = 21

        Try

            nReturnValue = gPMConstants.PMEReturnCode.PMTrue
            r_sMessage = ""
            nValidRIBand = gPMConstants.PMEReturnCode.PMFalse
            nCommitPolicy = gPMConstants.PMEReturnCode.PMFalse
            nTransactionStarted = gPMConstants.PMEReturnCode.PMFalse

            ' Get risk associated with this policy

            If m_oRiskData.GetRisk(v_lInsuranceFileCnt:=CInt(v_nInsuranceFileCnt), r_vResultArray:=oRiskArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to get associated risks for this policy " & v_nInsuranceFileCnt
                Return PMEReturnCode.PMFalse
            End If

            ' Check if we have any risks
            If Not Informations.IsArray(oRiskArray) Then
                Return nReturnValue
            End If

            ' Make sure we have the right transaction type
            If m_oRiskData.SetProcessModes(vTransactionType:=CStr(v_sTransactionType)) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to set process mode to RiskData object (bSIRRiskData.business)"
                Return PMEReturnCode.PMFalse
            End If

            m_nReturn = GetPolicyType(v_nInsuranceFileCnt, nInsuranceFileType)
            ' Process each risk

            For iCount As Integer = 0 To oRiskArray.GetUpperBound(1)

                nTransactionStarted = gPMConstants.PMEReturnCode.PMTrue
                nRiskCnt = gPMFunctions.NullToLong(oRiskArray(kFieldPosRiskID, iCount))

                ' Is Risk Marked For Portfolio Transfer?
                If IsRiskMarkedForPortfolioTransfer(v_lRiskCnt:=nRiskCnt, r_bResult:=bMarkedForTransfer, v_dtTransferDate:=v_dtTransferDate) <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to check if risk is marked for portfolio transfer (Policy: " & v_nInsuranceFileCnt & " Risk: " & CStr(nRiskCnt) & ")"
                    Return PMEReturnCode.PMFalse
                End If

                ' No need to copy risk, simply add a link in insurance_file_risk_link table
                If Not bMarkedForTransfer Then

                    ' Add new risk link
                    If m_oRiskData.AddRiskLink(v_lNewInsuranceFileCnt:=v_nNewInsuranceFileCnt, v_lRiskCnt:=nRiskCnt, v_sStatusFlag:="U") <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to create new insurance file risk link (Policy: " & v_nNewInsuranceFileCnt & " Risk: " & CStr(nRiskCnt) & ")"
                        Return PMEReturnCode.PMFalse
                    End If

                    ' Move claims to new version of policy with same risks
                    If MoveClaimToNewRisk(v_nInsuranceFileCnt:=v_nInsuranceFileCnt, v_nRiskCnt:=nRiskCnt, v_nNewInsuranceFileCnt:=v_nNewInsuranceFileCnt, v_nNewRiskCnt:=nRiskCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to move claims to new version of policy  (old Policy: " & v_nInsuranceFileCnt & " old Risk: " & CStr(nRiskCnt) &
                                     " new policy: " & CStr(v_nNewInsuranceFileCnt) & " new risk: " & CStr(nNewRiskCnt) & ")"
                        Return PMEReturnCode.PMFalse
                    End If

                    ' E007 Changes : Manipulate array
                    If Informations.IsArray(m_oClaimsArray) Then
                        For nClaimsCount = 0 To DirectCast(m_oClaimsArray, Object(,)).GetUpperBound(1)
                            If m_oClaimsArray(2, nClaimsCount) = nRiskCnt Then
                                m_oClaimsArray(3, nClaimsCount) = nNewRiskCnt
                            End If
                        Next
                    End If
                    nTransactionStarted = gPMConstants.PMEReturnCode.PMFalse

                    ' Risk DOES need copying and processing
                Else
                    ' Copy risk details and reset status to unquoted
                    If nInsuranceFileType = 2 Then

                        If oRiskArray(29, iCount) = "D" And v_sTransactionType = "PT" Then  'PM022349 PT Sumit

                            If m_oRiskData.CopyRisk(v_lNewInsuranceFileCnt:=v_nNewInsuranceFileCnt,
                                                        v_vRiskDetail:=oRiskArray,
                                                        v_lPosNo:=iCount,
                                                        r_lRiskCnt:=nNewRiskCnt,
                                                        v_lResetStatus:=PMEReturnCode.PMTrue,
                                                        v_lCreateLinkType:=3) <> PMEReturnCode.PMTrue Then

                                r_sMessage = "Failed to copy risk details to new policy version (old policy :" & v_nInsuranceFileCnt & " new policy " & CStr(v_nNewInsuranceFileCnt) & " Risk :)" & CStr(nRiskCnt) & ")"
                                Return PMEReturnCode.PMFalse
                            End If
                        Else

                            If m_oRiskData.CopyRisk(v_lNewInsuranceFileCnt:=v_nNewInsuranceFileCnt, v_vRiskDetail:=oRiskArray, v_lPosNo:=iCount, r_lRiskCnt:=nNewRiskCnt, v_lResetStatus:=gPMConstants.PMEReturnCode.PMTrue, v_lCreateLinkType:=1) <> gPMConstants.PMEReturnCode.PMTrue Then

                                r_sMessage = "Failed to copy risk details to new policy version (old policy :" & v_nInsuranceFileCnt & " new policy " & CStr(v_nNewInsuranceFileCnt) & " Risk :)" & CStr(nRiskCnt) & ")"
                                Return PMEReturnCode.PMFalse
                            End If
                        End If
                    Else
                        m_nReturn = GetPreviousRiskCnt(v_nPreInsuranceFileCnt:=v_nNewInsuranceFileCnt,
                                                  v_nRiskCnt:=nRiskCnt,
                                                  v_nInsuranceFileCnt:=v_nInsuranceFileCnt,
                                                  r_nPreviousRiskCnt:=nPreviousRiskCnt)

                        If CopyRisk(v_nNewInsuranceFileCnt:=v_nNewInsuranceFileCnt,
                                                v_oRiskDetail:=oRiskArray,
                                                v_nPosNo:=iCount,
                                                r_nRiskCnt:=nNewRiskCnt,
                                                v_nResetStatus:=gPMConstants.PMEReturnCode.PMTrue,
                                                v_nCreateLinkType:=If(oRiskArray(29, iCount) = "D", 3, kInsFileRiskLinkTypeORIGINAL),
                                                v_nOldRiskCnt:=If(nPreviousRiskCnt = 0, nRiskCnt, nPreviousRiskCnt)) <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to copy risk details to new policy version (old policy :" & v_nInsuranceFileCnt & " new policy " & v_nNewInsuranceFileCnt & " Risk :)" & nRiskCnt & ")"
                            Return PMEReturnCode.PMFalse
                        End If
                    End If

                    '************************ copy gis data for this risk *****************************
                    ' Get get policy link for old policy
                    If m_oRiskData.GetGISPolicyLink(v_lInsuranceFolderCnt:=v_nInsuranceFolderCnt, v_lRiskID:=nRiskCnt, r_vResultArray:=oGISPolicyLinkArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to copy gis details to new policy version (old policy :" & v_nInsuranceFileCnt & " new policy: " & CStr(v_nNewInsuranceFileCnt) & " Risk: " & CStr(nRiskCnt) & ")"
                        Return PMEReturnCode.PMFalse
                    End If

                    ' Do we have any gis data
                    nGISPolicyLinkID = gPMFunctions.NullToLong(oGISPolicyLinkArray(0, 0))
                    sGISDataModelCode = gPMFunctions.NullToString(oGISPolicyLinkArray(4, 0)).Trim()

                    If nGISPolicyLinkID = 0 Then
                        r_sMessage = "We have no gis data for policy version (old policy :" & v_nInsuranceFileCnt & " Risk: " & CStr(nRiskCnt) & ")"
                        Return PMEReturnCode.PMFalse
                    End If

                    ' Load gis detail for old risk (Note: gis object store folder_cnt in file_cnt field)
                    If m_oRenSelection.GIS_LoadFromDB(v_sGisDataModelCode:=CStr(sGISDataModelCode), r_vInsuranceFileCnt:=v_nInsuranceFolderCnt, r_vPolicyLinkID:=nGISPolicyLinkID, r_vRiskID:=nRiskCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to load gis data (policy folder :" & v_nInsuranceFolderCnt & " Risk: " & CStr(nRiskCnt) & ")"
                        Return PMEReturnCode.PMFalse
                    End If

                    ' Copy gis data to new risk (Note: gis object store folder_cnt in file_cnt field)
                    If m_oRenSelection.CopyDataSet(v_sDataModelCode:=CStr(sGISDataModelCode), r_lNewGISPolicyLinkId:=nNewGisPolicyLinkID, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet, v_vOldGISPolicyLinkId:=nGISPolicyLinkID, v_vOldInsuranceFileCnt:=v_nInsuranceFolderCnt, v_vOldRiskID:=nRiskCnt, v_vNewInsuranceFileCnt:=v_nInsuranceFolderCnt, v_vNewRiskID:=nNewRiskCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to copy gis data (policy folder :" & v_nInsuranceFolderCnt & "Old Risk: " & CStr(nRiskCnt) & " New Risk: " & CStr(nNewRiskCnt) & ")"
                        Return PMEReturnCode.PMFalse
                    End If

                    ' Initialise the Data Set with the Object/Properties
                    If m_oRenSelection.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed load data from xml (policy folder :" & v_nInsuranceFolderCnt & "Old Risk: " & CStr(nRiskCnt) & " New Risk: " & CStr(nNewRiskCnt) & ")"
                        Return PMEReturnCode.PMFalse
                    End If

                    ' Save gis data for new risk
                    If m_oRenSelection.GIS_SaveToDB(v_sGisDataModelCode:=CStr(sGISDataModelCode)) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to save gis data for new risk (policy folder :" & v_nInsuranceFolderCnt & "Old Risk: " & CStr(nRiskCnt) & " New Risk: " & CStr(nNewRiskCnt) & ")"
                        Return PMEReturnCode.PMFalse
                    End If

                    ' Get policy binder for old policy version
                    If m_oRenSelection.GetPolicyBinderID(v_sDataModelCode:=CStr(sGISDataModelCode), v_lGISPolicyLinkId:=nGISPolicyLinkID, r_lPolicyBinderID:=nPolicyBinderID) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to get policy binder for old policy version (gis policy link: " & nGISPolicyLinkID & ")"
                        Return PMEReturnCode.PMFalse
                    End If

                    ' Get policy binder for new policy version
                    If m_oRenSelection.GetPolicyBinderID(v_sDataModelCode:=CStr(sGISDataModelCode), v_lGISPolicyLinkId:=nNewGisPolicyLinkID, r_lPolicyBinderID:=nNewPolicyBinderID) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to get policy binder for new policy version (gis policy link: " & nNewGisPolicyLinkID & ")"
                        Return PMEReturnCode.PMFalse
                    End If

                    ' Copy standard wording
                    If m_oRenSelection.CopyRiskStandardWordings(v_lOldPolicyBinderID:=nPolicyBinderID, v_lNewPolicyBinderID:=nNewPolicyBinderID, v_sDataModelCode:=sGISDataModelCode) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to copy standard wording (Old Policy Binder: " & nPolicyBinderID & " New Policy Binder: " & CStr(nNewPolicyBinderID) & ")"
                        Return PMEReturnCode.PMFalse
                    End If

                    ' Do we need to do index linking
                    Dim auxVar As Object = oRiskArray(kFieldPosGisScreenID, iCount)

                    If Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) Then
                        'Note: the risk_id and v_nInsuranceFileCnt are not being used so it doesn't matter what we pass in
                        'this is because further up the code we already load up gis info

                        If m_oRenSelection.GISIndexLink(v_lInsuranceFileCnt:=0, v_lRiskID:=0, v_vGisScreenID:=DirectCast(oRiskArray, Object(,))(kFieldPosGisScreenID, iCount), v_dtEffectiveDate:=CDate(v_dtPolicyStartDate), v_sGisDataModelCode:=CStr(sGISDataModelCode)) <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed index linking for policy: " & v_nNewInsuranceFileCnt
                            Return PMEReturnCode.PMFalse
                        End If
                    End If

                    ' Copy gis related sum insured

                    If m_oRiskData.CopyRSASumInsured(v_lOldPolicyLinkID:=CInt(nGISPolicyLinkID), v_lNewPolicyLinkID:=nNewGisPolicyLinkID) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to copy gis related sum insured (Old policy link:" & nGISPolicyLinkID & " New policy link:" & CStr(nNewGisPolicyLinkID) & ")"
                        Return PMEReturnCode.PMFalse
                    End If

                    ' Note : rating section will only work when original risk on a policy is of type (2,5) and status of policy is (3,4,5,6)
                    ' Do peril allocation for new policy version
                    With m_oPerilAllocation

                        .InsuranceFileCnt = v_nNewInsuranceFileCnt
                        .InsuranceFolderCnt = v_nInsuranceFolderCnt
                        .RiskId = nNewRiskCnt
                        .TransactionType = v_sTransactionType
                    End With

                    ' Populate rating sections

                    If m_oPerilAllocation.PopulateRatingSections() <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to populate rating sections for new policy version (Policy: " & v_nNewInsuranceFileCnt & " Risk: " & CStr(nNewRiskCnt) & ")"
                        Return PMEReturnCode.PMFalse
                    End If

                    dProRataRate = m_oPerilAllocation.ProRataRate

                    If oRiskArray(29, iCount) = "D" And v_sTransactionType = "PT" Then
                        If GetPreviousRiskCnt(v_nPreInsuranceFileCnt:=v_nNewInsuranceFileCnt,
                                                       v_nRiskCnt:=nRiskCnt,
                                                       v_nInsuranceFileCnt:=v_nInsuranceFileCnt,
                                                       r_nPreviousRiskCnt:=nPreviousRiskCnt) <> PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to get previous risk cnt for policy version (old policy :" & v_nInsuranceFileCnt & " new policy " & v_nNewInsuranceFileCnt & " Risk :)" & nRiskCnt & ")"
                            Return PMEReturnCode.PMFalse
                        End If
                        dProRataRate = m_oPerilAllocation.ProRataRate
                        If nPreviousRiskCnt = 0 Then nPreviousRiskCnt = nRiskCnt

                        If CopyRatings(v_nNewInsuranceFileCnt, nPreviousRiskCnt, nNewRiskCnt, dProRataRate, True, v_nNewInsuranceFileCnt) <> PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to populate rating sections for new policy version (Policy: " & v_nNewInsuranceFileCnt & " Risk: " & nNewRiskCnt & ")"
                            Return PMEReturnCode.PMFalse
                        End If
                    ElseIf nInsuranceFileType = 5 Or nInsuranceFileType = 8 Or nInsuranceFileType = 9 Then
                        dProRataRate = m_oPerilAllocation.ProRataRate

                        If CopyRatings(v_nInsuranceFileCnt, nRiskCnt, nNewRiskCnt, dProRataRate, True, , nInsuranceFileType, nPreviousRiskCnt) <> PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to populate rating sections for new policy version (Policy: " & v_nNewInsuranceFileCnt & " Risk: " & nNewRiskCnt & ")"
                            Return PMEReturnCode.PMFalse
                        End If
                    End If

                    ' Update risk with premium and suminsured

                    If m_oPerilAllocation.UpdateRisk() <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to update risk's premium and suminsured for new policy version (Policy: " & v_nNewInsuranceFileCnt & " Risk: " & CStr(nNewRiskCnt) & ")"
                        Return PMEReturnCode.PMFalse
                    End If

                    '**************** Note on reinsurance *******************
                    ' Reinsurance will work out which RI model is relevant for this risk
                    ' If it can't find one it will return false

                    'need to tell reinsurance that we are doing Portfolio transfer

                    m_nReturn = m_oReinsurance.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vTransactionType:=CStr(v_sTransactionType), vEffectiveDate:=CDate(v_dtTransferDate))

                    ' Get ready to do reinsurance (risk level)

                    m_oReinsurance.InsuranceFileCnt = v_nNewInsuranceFileCnt

                    m_oReinsurance.RiskId = nNewRiskCnt

                    ' Generate reinsurance for new policy version

                    If m_oReinsurance.CalculateRI(bIsPT:=True) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to generate RI for new policy version (Policy: " & v_nNewInsuranceFileCnt & " Risk: " & CStr(nNewRiskCnt) & ")"
                        Return PMEReturnCode.PMFalse
                    End If

                    ' Get reinsurance details (to fix roundings and validate)

                    If m_oReinsurance.GetDetails() <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to reinsurance details for new policy version (Policy: " & v_nNewInsuranceFileCnt & " Risk: " & CStr(nNewRiskCnt) & ")"
                        Return PMEReturnCode.PMFalse
                    End If

                    ' Do we have valid reinsurance bands ie adds up to 100%

                    If m_oReinsurance.ValidateBands(r_lValid:=nValidRIBand, r_lBand:=nReinsBand) <> gPMConstants.PMEReturnCode.PMTrue Then
                        If v_bIgnoreError = False Then
                            r_sMessage = "Failed to validate RI bands for new policy version (Policy: " & v_nNewInsuranceFileCnt & " Risk: " & nNewRiskCnt & ")"
                            Return PMEReturnCode.PMFalse
                        End If
                    End If

                    ' Save reinsurance details
                    If m_oReinsurance.Update() <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to reinsurance details for new policy version (Policy: " & v_nNewInsuranceFileCnt & " Risk: " & CStr(nNewRiskCnt) & ")"
                        Return PMEReturnCode.PMFalse
                    End If

                    ' Do we have valid bands ie adds up to 100%
                    If nValidRIBand = gPMConstants.PMEReturnCode.PMFalse Then
                        ' Change old risk status to quoted so it won't get pick up again

                        If m_oRiskData.UpdateRiskStatus(v_lRiskCnt:=CInt(nRiskCnt), v_lRiskStatusID:=0, v_sRiskStatusCode:="QUOTED") <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to update old risk status to quoted (Risk: " & nRiskCnt & ")"
                            Return PMEReturnCode.PMFalse
                        End If

                        ' NOTE: after peril allocation risk_status_id = 8 (pending resinsurance) if everything is ok
                        '       reinsurance (business side) shouldn't change this risk status so we are expecting pending reinsurance
                        ' Get risk status for new risk
                        If GetRiskStatus(v_lRiskCnt:=CInt(nNewRiskCnt), r_lRiskStatusID:=nRiskStatusID, r_sRiskStatusCode:=CStr(sRiskStatusCode)) <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to get status for new risk (Risk: " & nNewRiskCnt & ")"
                            Return PMEReturnCode.PMFalse
                        End If

                        If sRiskStatusCode.Trim().ToUpper() = "PENDINGRI" Then 'pending reinsurance
                            ' Somthing went wrong, set risk status to pending RI portfolio transfer
                            sRiskStatusCode = If(nDeferredRIBandNewRisk = 0, "QUOTED", "PENDINGRIP")
                            ' update the status of new risk Quoted as there is a valid RI
                            If m_oRiskData.UpdateRiskStatus(v_lRiskCnt:=CInt(nNewRiskCnt), v_lRiskStatusID:=0, v_sRiskStatusCode:=CStr(sRiskStatusCode)) <> gPMConstants.PMEReturnCode.PMTrue Then
                                r_sMessage = "Failed to update old risk status to quoted (Risk: " & nRiskCnt & ")"
                                Return PMEReturnCode.PMFalse
                            End If

                            r_bPendingRI = False

                            If sRiskStatusCode = "PENDINGRIP" Then
                                r_bPendingRI = True
                            End If
                        Else
                            r_sMessage = "Peril Allocation Failed or more questions need answering (Risk: " & nNewRiskCnt & ")"
                            Return PMEReturnCode.PMFalse
                        End If

                        ' Move claims to new risk
                        ' Maintain/payment of claim will sort out reinsurance when this claim is picked up
                        If MoveClaimToNewRisk(v_nInsuranceFileCnt:=v_nInsuranceFileCnt, v_nRiskCnt:=nRiskCnt, v_nNewInsuranceFileCnt:=v_nNewInsuranceFileCnt, v_nNewRiskCnt:=nNewRiskCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to move claims to new risk  (old Policy: " & v_nInsuranceFileCnt & " old Risk: " & CStr(nRiskCnt) &
                                                 " new policy: " & CStr(v_nNewInsuranceFileCnt) & " new risk: " & CStr(nNewRiskCnt) & ")"
                            Return PMEReturnCode.PMFalse
                        End If

                        ' E007 Changes : Manipulate array
                        If Informations.IsArray(m_oClaimsArray) Then
                            For nClaimsCount = 0 To DirectCast(m_oClaimsArray, Object(,)).GetUpperBound(1)
                                If m_oClaimsArray(2, nClaimsCount) = nRiskCnt Then
                                    m_oClaimsArray(3, nClaimsCount) = nNewRiskCnt
                                End If
                            Next
                        End If

                        nTransactionStarted = gPMConstants.PMEReturnCode.PMFalse

                        ' Set this flag to stop policy from rolling back
                        nCommitPolicy = gPMConstants.PMEReturnCode.PMTrue

                    Else
                        If v_bIgnoreError = False Then
                            r_bPendingRI = True
                            r_sMessage = "Invalid RI Bands ie bands do not add up to 100% - old policy: " & v_nInsuranceFileCnt & "-" & v_sInsuranceRef & " new policy: " & v_nNewInsuranceFileCnt & "-" & v_sInsuranceRef & "RI Portfolio Transfer"
                            Return PMEReturnCode.PMFalse
                        End If
                    End If
                End If

                'Update Risk this_Premium to zero PM100238
                m_oDatabase.Parameters.Clear()
                m_nReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt",
                                      vValue:=nNewRiskCnt,
                                      iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                      iDataType:=gPMConstants.PMEDataType.PMLong)

                m_nReturn = m_oDatabase.SQLAction(sSQL:=kUpdateRiskPremiumSQL,
                                                     sSQLName:=kUpdateRiskPremiumSQLName,
                                                     bStoredProcedure:=kUpdateRiskPremiumSQLStored)

            Next iCount

        Catch ex As Exception

            nReturnValue = gPMConstants.PMEReturnCode.PMFalse

            ' Let calling function log this message
            If r_sMessage = "" Then
                r_sMessage = Informations.Err().Description
            End If

            r_sMessage = r_sMessage & Strings.ChrW(13) & Strings.ChrW(10) & "Failed in CopyAllRisk()"

            Return nReturnValue

        End Try
        Return nReturnValue

    End Function


    '***********************************************************************************************************************
    ' Name :    IsRiskMarkedForPortfolioTransfer
    ' Desc :    Checks if risk is attached to a RI model marked for portfolio transfer
    ' Author :  Alix Bergeret - 08/07/2004
    '***********************************************************************************************************************
    Private Function IsRiskMarkedForPortfolioTransfer(ByVal v_lRiskCnt As Integer, ByVal v_dtTransferDate As Date, ByRef r_bResult As Boolean) As Integer

        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim sMessage As String = "" = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            lReturnValue = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add parameters

            m_nReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=v_lRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_nReturn
            End If


            m_nReturn = m_oDatabase.Parameters.Add(sName:="transfer_date", vValue:=CDate(v_dtTransferDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_nReturn
            End If

            ' Get data

            m_nReturn = m_oDatabase.SQLSelect(sSQL:=ACIsRiskMarkedForPortfolioTransferSQL, sSQLName:=ACIsRiskMarkedForPortfolioTransferName, bStoredProcedure:=ACIsRiskMarkedForPortfolioTransferStored, vResultArray:=vResultArray)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_nReturn
            End If

            ' Return result
            r_bResult = False
            If Informations.IsArray(vResultArray) Then

                If CStr(vResultArray(0, 0)) = "1" Then
                    r_bResult = True
                End If
            End If

        Catch
            lReturnValue = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed IsRiskMarkedForPortfolioTransfer()", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="IsRiskMarkedForPortfolioTransfer", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return lReturnValue

        End Try
        Return lReturnValue

    End Function

    ''' <summary>
    ''' Move all associate claims to new risk
    ''' </summary>
    ''' <param name="v_nInsuranceFileCnt"></param>
    ''' <param name="v_nRiskCnt"></param>
    ''' <param name="v_nNewInsuranceFileCnt"></param>
    ''' <param name="v_nNewRiskCnt"></param>
    ''' <param name="v_nclaimCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function MoveClaimToNewRisk(ByVal v_nInsuranceFileCnt As Integer, ByVal v_nRiskCnt As Integer, ByVal v_nNewInsuranceFileCnt As Integer, ByVal v_nNewRiskCnt As Integer, Optional ByVal v_nclaimCnt As Integer = 0) As Integer

        Dim sMessage As String = "" = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode

        Dim ex As New Exception
        Try
            lReturnValue = gPMConstants.PMEReturnCode.PMTrue
            sMessage = ""


            m_oDatabase.Parameters.Clear()


            m_nReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CInt(v_nInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add insurance file cnt param"
                Throw ex
            End If


            m_nReturn = m_oDatabase.Parameters.Add(sName:="RiskCnt", vValue:=v_nRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add risk cnt param"
                Throw ex
            End If


            m_nReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=CInt(v_nNewInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add new insurance file cnt param"
                Throw ex
            End If


            m_nReturn = m_oDatabase.Parameters.Add(sName:="NewRiskCnt", vValue:=v_nNewRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add new risk cnt param"
                Throw ex
            End If
            ' E007 Changes
            If v_nclaimCnt > 0 Then
                m_nReturn = m_oDatabase.Parameters.Add(sName:="ClaimCnt", vValue:=v_nclaimCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMessage = "Failed to add new risk cnt param"
                    Throw ex
                End If
            End If

            m_nReturn = m_oDatabase.SQLAction(sSQL:=ACMoveClaimToNewRiskSQL, sSQLName:=ACMoveClaimToNewRiskName, bStoredProcedure:=ACMoveClaimToNewRiskStored)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed SQLAction - move claims to new risk"
                Throw ex
            End If

        Catch
            lReturnValue = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed MoveClaimToNewRisk()", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="MoveClaimToNewRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        End Try
        Return lReturnValue
    End Function

    '***********************************************************************************************************************
    ' Name :    CreateAndPostStats
    ' Desc :    Create and post stats to orion
    ' Author :  Thinh Nguyen 27/02/2004
    '***********************************************************************************************************************
    Public Function CreateAndPostStats(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sTransactionType As String, Optional ByVal v_lPTInsuranceFileCnt As Long = 0, Optional ByVal v_bReversePT As Boolean = False, Optional ByRef r_sMessage As String = "") As Integer

        Dim lReturnValue As gPMConstants.PMEReturnCode

        Dim ex As New Exception
        Try
            r_sMessage = ""
            lReturnValue = gPMConstants.PMEReturnCode.PMTrue


            m_oControlTrans.InsuranceFileCnt = v_lInsuranceFileCnt
            m_oControlTrans.PTInsuranceFileCnt = v_lPTInsuranceFileCnt
            m_oControlTrans.ReversePT = v_bReversePT
            m_oControlTrans.IsPT = True

            m_nReturn = m_oControlTrans.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vNavigate:=CInt(m_nNavigate), vProcessMode:=CInt(m_nProcessMode), vTransactionType:=CStr(v_sTransactionType), vEffectiveDate:=CDate(m_dtEffectiveDate))

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to set process mode for bControlTrans.Automated"
                Throw ex
            End If


            If m_oControlTrans.Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to create and post stats"
                Throw ex
            End If

        Catch
            lReturnValue = gPMConstants.PMEReturnCode.PMFalse

            ' Let calling function log this message
            If r_sMessage = "" Then
                r_sMessage = Informations.Err().Description
            End If

            r_sMessage = r_sMessage & Strings.ChrW(13) & Strings.ChrW(10) & "Failed in CreateAndPostStats()"

        End Try

        Return lReturnValue

    End Function


    '***********************************************************************************************************************
    ' Name : DeleteInsFilePTRIUsage
    '
    ' Desc : Delete the Insurance_File_PT_RI_Usage record
    '
    ' Author : Kuljeet Kaur 11/03/2011
    '***********************************************************************************************************************
    Public Function DeleteInsFilePTRIUsage(ByVal v_lInsFilePTRIUsageID As Integer) As Long
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim sMessage As String = ""
        Dim ex As New Exception
        Try

            sMessage = ""
            lReturnValue = gPMConstants.PMEReturnCode.PMTrue
            ' set the params
            m_oDatabase.Parameters.Clear()


            m_nReturn = m_oDatabase.Parameters.Add(sName:="ins_file_PT_RI_usage_id", vValue:=v_lInsFilePTRIUsageID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add parameter ins_file_PT_RI_usage_id"
                Throw ex
            End If

            ' do the call to remove the record from Insurance_File_PT_RI_Usage
            m_nReturn = m_oDatabase.SQLAction(
                sSQL:=ACDeleteInsFilePTRIUsageSQL,
                sSQLName:=ACDeleteInsFilePTRIUsageName,
                bStoredProcedure:=ACDeleteInsFilePTRIUsageStored)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to delete insurance_file_pt_ri_usage"
                Throw ex
            End If
        Catch

            lReturnValue = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed DeleteInsFilePTRIUsage() ", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteInsFilePTRIUsage", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
        End Try
        Return lReturnValue
    End Function

    Public Function GetPTRIPolicy(ByRef r_vResultArray As Object) As Long
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim sMessage As String = ""
        Dim ex As New Exception
        Try
            sMessage = ""
            lReturnValue = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Get the risk status of the risk
            m_nReturn = m_oDatabase.SQLSelect(
                    sSQL:=ACGetPTRIPolicySQL,
                    sSQLName:=ACGetPTRIPolicyName,
                    bStoredProcedure:=ACGetPTRIPolicyStored,
                    vResultArray:=r_vResultArray)


            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to get policy for portfolio transfer"
                Throw ex
            End If

        Catch

            lReturnValue = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed GetPTRIPolicy() ", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="GetPTRIPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
        End Try

        Return lReturnValue
    End Function

    Public Function InsertInsFilePTRIUsage(ByVal v_lInsFileCnt As Long, ByVal v_dtTransferDate As Date) As Long
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim sMessage As String = ""
        Dim ex As New Exception
        Try

            sMessage = ""
            lReturnValue = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_nReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add parameter insurance_file_cnt"
                Throw ex
            End If

            m_nReturn = m_oDatabase.Parameters.Add(sName:="transferDate", vValue:=CDate(v_dtTransferDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add parameter transferDate"
                Throw ex
            End If

            m_nReturn = m_oDatabase.Parameters.Add(sName:="ins_file_PT_RI_usage_id", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add parameter ins_file_PT_RI_usage_id"
                Throw ex
            End If

            m_nReturn = m_oDatabase.SQLAction(
            sSQL:=ACInsertInsFilePTRIUsageSQL,
            sSQLName:=ACInsertInsFilePTRIUsageName,
            bStoredProcedure:=ACInsertInsFilePTRIUsageStored)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add parameter ins_file_PT_RI_usage_id"
                Throw ex
            End If

        Catch

            lReturnValue = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed InsertInsFilePTRIUsage() ", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="InsertInsFilePTRIUsage", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
        End Try
        Return lReturnValue

    End Function

    Public Function SetPTRIStatus(ByVal v_lInsFilePTRIUsageID As Long, ByVal v_lInsFileCnt As Long, ByVal v_lPTRIStatusID As Long) As Long
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim sMessage As String = ""
        Dim ex As New Exception
        Try

            sMessage = ""
            lReturnValue = gPMConstants.PMEReturnCode.PMTrue
            ' set the params
            m_oDatabase.Parameters.Clear()


            m_nReturn = m_oDatabase.Parameters.Add(sName:="ins_file_PT_RI_usage_id", vValue:=v_lInsFilePTRIUsageID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add parameter ins_file_PT_RI_usage_id"
                Throw ex
            End If


            m_nReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add parameter insurance_file_cnt"
                Throw ex
            End If


            m_nReturn = m_oDatabase.Parameters.Add(sName:="PT_RI_status_type_id", vValue:=v_lPTRIStatusID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add parameter PT_RI_status_type_id"
                Throw ex
            End If


            ' do the update call
            m_nReturn = m_oDatabase.SQLAction(
                sSQL:=ACUpdPTRIStatusSQL,
                sSQLName:=ACUpdPTRIStatusName,
                bStoredProcedure:=ACUpdPTRIStatusStored)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to SetPTRIStatus"
                Throw ex
            End If
        Catch

            lReturnValue = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed SetPTRIStatus() ", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="SetPTRIStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
        End Try
        Return lReturnValue

    End Function


    Public Function SetPolicyStatus(ByVal v_lInsuranceFileCnt As Long,
                                ByVal v_lInsuranceFileStatusID As Long,
                                Optional ByVal v_bStartTransaction As Boolean = True,
                                Optional ByRef r_sFailureMessage As String = "GGGGGRRRRRR") As Long

        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim sMessage As String = ""
        Dim sSQL As String

        Dim ex As New Exception
        Try
            sMessage = ""
            lReturnValue = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "UPDATE Insurance_File SET insurance_file_status_id = {InsuranceFileStatusID}" & vbCrLf
            sSQL = sSQL & "WHERE insurance_file_cnt = {InsuranceFileCnt}"

            If v_bStartTransaction Then
                If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                    If r_sFailureMessage <> "GGGGGRRRRRR" Then
                        r_sFailureMessage = "Failed to start transaction"
                    End If
                    Throw ex
                End If
            End If

            m_oDatabase.Parameters.Clear()

            m_nReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileStatusID", vValue:=If(v_lInsuranceFileStatusID = 0, Nothing, v_lInsuranceFileStatusID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to add parameter to PMDAO (InsuranceFileStatusID)"
                End If

                Throw ex
            End If

            m_nReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=If(v_lInsuranceFileStatusID = 0, Nothing, v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to add parameter to PMDAO (InsuranceFileCnt)"
                End If

                Throw ex
            End If

            m_nReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Change Policy Status", bStoredProcedure:=False)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to update policy status"
                End If
                Throw ex
            End If

            If v_bStartTransaction Then
                If m_oDatabase.SQLCommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                    If r_sFailureMessage <> "GGGGGRRRRRR" Then
                        r_sFailureMessage = "Failed to commit transactions"
                    End If

                    Throw ex
                End If
            End If

        Catch

            If r_sFailureMessage <> "GGGGGRRRRRR" Then
                r_sFailureMessage = "SetPolicyStatus() - Errored"
            End If
            lReturnValue = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(r_sFailureMessage = "", "Failed InsertInsFilePTRIUsage() ", r_sFailureMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="InsertInsFilePTRIUsage", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        End Try

        If v_bStartTransaction Then
            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                m_nReturn = m_oDatabase.SQLRollbackTrans()
            End If
        End If
        Return lReturnValue

    End Function

    Public Function RelinkRisk(ByVal v_lOldInsuranceFileCnt As Long, ByVal v_lNewInsuranceFileCnt As Long) As Long

        Dim sSQL As String
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim sMessage As String = ""
        Dim ex As New Exception
        Try

            sMessage = ""
            lReturnValue = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "INSERT INTO insurance_file_risk_link (" & vbCrLf &
                   "insurance_file_cnt," & vbCrLf &
                   "risk_cnt," & vbCrLf &
                   "status_flag, " & vbCrLf &
                   "original_risk_cnt)" & vbCrLf &
                   "SELECT " & v_lNewInsuranceFileCnt & "," & vbCrLf &
                   "risk_cnt," & vbCrLf &
                   "'U'," & vbCrLf &
                   "null" & vbCrLf &
                   "FROM insurance_file_risk_link" & vbCrLf &
                   "WHERE insurance_file_cnt = " & v_lOldInsuranceFileCnt & vbCrLf &
                   "AND status_flag <> 'D'" & vbCrLf


            m_oDatabase.Parameters.Clear()

            If m_oDatabase.SQLAction(sSQL:=sSQL,
                                    sSQLName:="Link old risks to new policy version",
                                    bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw ex
            End If

        Catch

            lReturnValue = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed RelinkRisk() ", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="RelinkRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
        End Try

        Return lReturnValue

    End Function

    Public Function DeletePolicy(ByVal v_lInsuranceFileCnt As Long) As Long
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim sMessage As String = ""
        Dim ex As New Exception
        Try
            sMessage = ""
            lReturnValue = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt",
                                                   vValue:=CInt(v_lInsuranceFileCnt),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw ex
            End If

            DeletePolicy = m_oDatabase.SQLAction(sSQL:=ACDeletePolicySQL,
                                                sSQLName:=ACDeletePolicyName,
                                                bStoredProcedure:=ACDeletePolicyStored)

        Catch

            lReturnValue = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed DeletePolicy() ", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        End Try

        Return lReturnValue

    End Function

    Public Function CopyPolicyHeader(ByVal v_lInsuranceFileCnt As Long,
                                ByVal v_dTransferDate As Date,
                                ByRef r_lNewInsurancefileCnt As Long,
                                ByRef r_sMessage As String,
                                Optional ByRef r_lInsuranceFolderCnt As Long = 0,
                                Optional ByRef r_dtPolicyStartDate As Date = Nothing,
                                Optional ByVal v_sSetOldInsuranceFileStatus As String = "",
                                Optional ByVal v_sSetNewInsuranceFileStatus As String = "",
                                Optional ByVal v_sTransactionType As String = "") As Long

        Dim sDescription As String = ""         'this is not used. just need to call tax function
        Dim vInsuranceFileTax As Object = Nothing   'this is not used. just need to call tax function
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim sMessage As String = ""
        Dim ex As Exception = Nothing
        Try
            ex = New Exception
            lReturnValue = gPMConstants.PMEReturnCode.PMTrue
            '****************** Create new version of policy ************************
            'assign current insurance file count to business object
            m_oInsuranceFile.InsuranceFileCnt = v_lInsuranceFileCnt

            ' get details of current policy
            If (m_oInsuranceFile.GetDetails() <> gPMConstants.PMEReturnCode.PMTrue) Then
                r_sMessage = "Failed to get policy details for " & v_lInsuranceFileCnt
                Throw ex
            End If


            'set last transaction type if required
            If v_sTransactionType <> "" Then
                m_oInsuranceFile.LastTransType = v_sTransactionType
            End If


            m_oInsuranceFile.PolicyVersion = CLng(m_oInsuranceFile.PolicyVersion) + 1
            m_oInsuranceFile.CoverStartDate = v_dTransferDate

            ' Set old version to "replaced portfolio transfer"
            m_oInsuranceFile.InsuranceFileStatus = SIRInsFileStatusReplacedPT

            m_oInsuranceFile.EventDescription = "Policy copied for portfolio transfer"

            m_oInsuranceFile.UpdatePolicy()

            ' create new version of policy based on current policy details
            If m_oInsuranceFile.CreatePolicy() <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to create new version of policy for " & v_lInsuranceFileCnt
                Throw ex
            End If

            '******************** Get new policy version details ******************************************
            r_lNewInsurancefileCnt = m_oInsuranceFile.InsuranceFileCnt

            'do we need to return start date?
            r_dtPolicyStartDate = CDate(m_oInsuranceFile.CoverStartDate)

            'do we need to return insurance_folder_cnt?
            If Not Informations.IsNothing(r_lInsuranceFolderCnt) Then
                r_lInsuranceFolderCnt = m_oInsuranceFile.InsuranceFolderCnt   'note folder will be the same as previous policy version
            End If

            '****************** Copy related data to new policy version ***********************
            'copy standard wording
            If m_oRenSelection.CopyPolicyStandardWordings(v_lOldInsuranceFileCnt:=v_lInsuranceFileCnt, v_lNewInsuranceFileCnt:=r_lNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to copy standard wording to new policy version (current policy :" & v_lInsuranceFileCnt & " new policy " & r_lNewInsurancefileCnt & " )"
                Throw ex
            End If

            'copy coinsurance
            If m_oRenSelection.CopyCoinsurance(v_lCurrentInsFileCnt:=v_lInsuranceFileCnt, v_lNewInsFileCnt:=r_lNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to copy coinsurance details to new policy version (current policy :" & v_lInsuranceFileCnt & " new policy " & r_lNewInsurancefileCnt & " )"
                Throw ex
            End If

            'copy agent commission
            If m_oRenSelection.CopyAgentCommission(v_lCurrentInsFileCnt:=v_lInsuranceFileCnt, v_lNewInsFileCnt:=r_lNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to copy agent commission details to new policy version (current policy :" & v_lInsuranceFileCnt & " new policy " & r_lNewInsurancefileCnt & " )"
                Throw ex
            End If

            'copy agents
            If m_oRenSelection.CopyInsuranceFileAgent(v_lCurrentInsFileCnt:=v_lInsuranceFileCnt, v_lNewInsFileCnt:=r_lNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to copy agent details to new policy version (current policy :" & v_lInsuranceFileCnt & " new policy " & r_lNewInsurancefileCnt & " )"
                Throw ex
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
            If m_oTax.SetProcessModes(vTask:=PMConst.PMEdit) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to SetProcessModes() to Tax object"
                Throw ex
            End If

            m_oTax.InsuranceFileCnt = r_lNewInsurancefileCnt
            If m_oTax.GetInsuranceFileTax(r_vInsuranceFileTax:=vInsuranceFileTax, r_sDescription:=CStr(sDescription)) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to copy policy level tax to new policy version (current policy :" & v_lInsuranceFileCnt & " new policy " & r_lNewInsurancefileCnt & " )"
                Throw ex
            End If

        Catch
            lReturnValue = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed CopyPolicyHeader() ", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyHeader", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return lReturnValue
        End Try
        Return lReturnValue
    End Function

    ' E007 Changes
    Private Function TransferClaimToNewRisk(ByVal v_nInsuranceFileCnt As Integer,
                                        ByVal v_nNewInsuranceFileCnt As Integer,
                                        ByRef r_sMessage As String) As Integer

        Dim nReturnValue As gPMConstants.PMEReturnCode
        Dim sMessage As String = ""

        Dim m_oFindClaim As Object = Nothing
        Dim nNewClaimId As Object = 0
        Dim nClaimsCount As Integer

        Try

            sMessage = ""
            nReturnValue = gPMConstants.PMEReturnCode.PMTrue

            nReturnValue = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oFindClaim,
                                                                v_sClassName:="bCLMFindClaim.Business",
                                                                v_sCallingAppName:=m_sCallingAppName,
                                                                v_sUsername:=m_sUsername,
                                                                v_sPassword:=m_sPassword,
                                                                v_iUserID:=m_iUserID,
                                                                v_iSourceID:=m_nSourceID,
                                                                v_iLanguageID:=m_nLanguageID,
                                                                v_iCurrencyID:=m_nCurrencyID,
                                                                v_iLogLevel:=m_nLogLevel,
                                                                v_oDatabase:=m_oDatabase)

            If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bCLMFindClaim.Business"
                Return nReturnValue
            End If

            If Informations.IsArray(m_oClaimsArray) Then
                For nClaimsCount = 0 To DirectCast(m_oClaimsArray, Object(,)).GetUpperBound(1)
                    If ToSafeLong(m_oClaimsArray(4, nClaimsCount), 0) > 0 Then
                        'Copy the claim
                        nReturnValue = m_oFindClaim.SetProcessModes(vTransactionType:="C_CR")
                        nReturnValue = m_oFindClaim.ProcessCopyClaim(v_lClaimId:=m_oClaimsArray(4, nClaimsCount), r_lCopyClaimId:=nNewClaimId)
                        If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                            sMessage = "Failed to Copy claim for Claim Id " & m_oClaimsArray(4, nClaimsCount)
                            Return nReturnValue
                        End If

                        'Process Claim Perils and Reserves
                        nReturnValue = ProcessClaimPerils(v_nclaimCnt:=nNewClaimId,
                                            v_nPrePortfolioTransfer:=0, v_nInsuranceFileCnt:=v_nNewInsuranceFileCnt,
                                            v_nIsCreated:=1)
                        If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                            sMessage = "Failed to NetOf Perils for Claim" & nNewClaimId
                            Return nReturnValue
                        End If
                    End If
                Next
            End If
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            nReturnValue = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed TransferClaimToNewRisk() ", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="TransferClaimToNewRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return nReturnValue
        Finally
            m_oFindClaim = Nothing
        End Try
        Return nReturnValue
    End Function
    ''' <summary>
    ''' ProcessClaimPerils
    ''' </summary>
    ''' <param name="v_nclaimCnt"></param>
    ''' <param name="v_nPrePortfolioTransfer"></param>
    ''' <param name="v_nInsuranceFileCnt"></param>
    ''' <param name="v_nIsCreated"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ProcessClaimPerils(ByVal v_nclaimCnt As Integer,
                                        ByVal v_nPrePortfolioTransfer As Integer,
                                        ByVal v_nInsuranceFileCnt As Integer,
                                        ByVal v_nIsCreated As Integer) As Integer

        ' Const kMethodName As String = "ProcessClaimPerils"
        Dim nReturnValue As gPMConstants.PMEReturnCode
        Dim sMessage As String = ""
        Dim r_dtNetOfClaimPerils As New DataTable
        Dim iCountPerils As Integer
        Dim r_cThisRevision As Decimal
        Dim r_iStatsFolderCnt As Integer
        Dim oControlTransClaims As Object = Nothing
        Dim bAlreadyCreated As Boolean

        Try

            nReturnValue = gPMConstants.PMEReturnCode.PMTrue

            nReturnValue = GetNetOfClaimPerils(v_lClaimId:=v_nclaimCnt,
                                            dtResult:=r_dtNetOfClaimPerils)
            If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nReturnValue
            End If

            If r_dtNetOfClaimPerils.Rows.Count > 0 Then
                For Each row As DataRow In r_dtNetOfClaimPerils.Rows
                    r_cThisRevision = 0

                    nReturnValue = NetOfClaimPerils(v_nclaimCnt:=v_nclaimCnt,
                                                v_nClaimPerilId:=r_dtNetOfClaimPerils.Rows(iCountPerils)(0).ToString,
                                                v_nPrePortfolioTransfer:=v_nPrePortfolioTransfer,
                                                r_cThisRevision:=r_cThisRevision)
                    If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return nReturnValue
                    End If

                    ' Create gross line
                    If r_cThisRevision <> 0 Then
                        If bAlreadyCreated = False Then
                            nReturnValue = AddClaimsStatsFolder(v_lInsuranceFileCnt:=CInt(v_nInsuranceFileCnt),
                                                        v_sDebitCredit:="D",
                                                        v_sDocumentComment:="Reserve for claim number " & v_nclaimCnt,
                                                        v_iTransactionTypeId:=1,
                                                        v_sTransactionTypeCode:="C_CR",
                                                        v_iUserID:=m_iUserID,
                                                        v_sUsername:=m_sUsername,
                                                        v_iClaimId:=v_nclaimCnt,
                                                        v_iDocumentTypeId:=41,
                                                        r_iStatsFolderCnt:=r_iStatsFolderCnt)
                            If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return nReturnValue
                            End If
                            bAlreadyCreated = True
                        End If

                        ' Add Stats Details
                        nReturnValue = AddClaimsStatsDetail(v_iStatsFolderCnt:=r_iStatsFolderCnt,
                                                v_lClaimId:=v_nclaimCnt,
                                                v_iClaimPerilId:=CInt(r_dtNetOfClaimPerils.Rows(iCountPerils)(0).ToString),
                                                v_sStatsDetailType:="GRS",
                                                v_iClassOfBusiness:=r_dtNetOfClaimPerils.Rows(iCountPerils)(1).ToString,
                                                v_sClassOfBusinessCode:=r_dtNetOfClaimPerils.Rows(iCountPerils)(2).ToString,
                                                v_iRIPartyCnt:=0,
                                                v_sCreditAccountCode:="CLMRES" & r_dtNetOfClaimPerils.Rows(iCountPerils)(2).ToString,
                                                v_iRIPartyType:=0,
                                                v_dRISharePercent:=0,
                                                v_crTransactionAmount:=r_cThisRevision,
                                                v_iDocumentTypeId:=41)
                        If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return nReturnValue
                        End If
                    End If
                    iCountPerils = iCountPerils + 1
                Next
                'Process Claim Reinsurance
                nReturnValue = ProcessClaimReinsurance(v_nclaimCnt:=v_nclaimCnt, v_iIsCreated:=v_nIsCreated)
                If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return nReturnValue
                End If
                If bAlreadyCreated Then
                    ' Accounting Raised only for RI
                    nReturnValue = gPMComponentServices.CreateBusinessObject(r_oObject:=oControlTransClaims,
                                                                        v_sClassName:="bControlTransClaims.Automated",
                                                                        v_sCallingAppName:=ACApp,
                                                                        v_sUsername:=m_sUsername,
                                                                        v_sPassword:=m_sPassword,
                                                                        v_iUserID:=m_iUserID,
                                                                        v_iSourceID:=m_nSourceID,
                                                                        v_iLanguageID:=m_nLanguageID,
                                                                        v_iCurrencyID:=m_nCurrencyID,
                                                                        v_iLogLevel:=m_nLogLevel,
                                                                        v_oDatabase:=m_oDatabase)
                    If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return nReturnValue
                    End If

                    If r_iStatsFolderCnt > 0 Then
                        oControlTransClaims.ClaimId = v_nclaimCnt
                        oControlTransClaims.DocumentTypeID = 41
                        nReturnValue = oControlTransClaims.CreateStatsForCoinsReins(ToSafeLong(r_iStatsFolderCnt, 0))
                        If (nReturnValue <> gPMConstants.PMEReturnCode.PMTrue) Then
                            Return nReturnValue
                        End If

                        nReturnValue = FinaliseStats(v_lClaimId:=v_nclaimCnt,
                                        v_lTransactionTypeId:=41,
                                        v_sTransactionTypeCode:="C_CR",
                                        v_lStatsFolderCnt:=r_iStatsFolderCnt,
                                        v_lStatsSuppressed:=0)
                        If (nReturnValue <> gPMConstants.PMEReturnCode.PMTrue) Then
                            Return nReturnValue
                        End If

                        nReturnValue = oControlTransClaims.CreateTransactions(ToSafeLong(r_iStatsFolderCnt, 0))
                        If (nReturnValue <> gPMConstants.PMEReturnCode.PMTrue) Then
                            Return nReturnValue
                        End If
                    End If
                End If
                nReturnValue = FinaliseClaimDetails(v_lClaimId:=v_nclaimCnt, v_sClaimVersionDescription:="Portfolio Claim Adjustment")
                If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return nReturnValue
                End If

                nReturnValue = UpdateClaimIsDirtyFlag(v_nclaimCnt:=v_nclaimCnt, v_nIsDirty:=0)
                If (nReturnValue <> gPMConstants.PMEReturnCode.PMTrue) Then

                    Return nReturnValue
                End If

            End If

        Catch
            ' DO Not Call any functions before here or the error will be lost
            nReturnValue = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed ProcessClaimPerils() ", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessClaimPerils", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return nReturnValue
        End Try

        Return nReturnValue

    End Function

    Private Function NetOfClaimPerils(ByVal v_nclaimCnt As Integer,
                                    ByVal v_nClaimPerilId As Integer,
                                    ByVal v_nPrePortfolioTransfer As Integer,
                                    ByRef r_cThisRevision As Decimal) As Integer

        Const kMethodName As String = "NetOfClaimPerils"

        Try

            NetOfClaimPerils = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()

                ' Add parameters
                m_nReturn = .Parameters.Add(sName:="claim_id", vValue:=v_nclaimCnt, iDirection:=PMConst.PMParamInput, iDataType:=PMConst.PMLong)
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed to add parameter claim_id ", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_nReturn = .Parameters.Add(sName:="claim_peril_id", vValue:=v_nClaimPerilId, iDirection:=PMConst.PMParamInput, iDataType:=PMConst.PMLong)
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed to add parameter claim_peril_id ", gPMConstants.PMELogLevel.PMLogError)
                End If

                .Parameters.Add("PrePortfolioTransfer", v_nPrePortfolioTransfer, PMConst.PMParamInput, PMConst.PMInteger)
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed to add parameter PrePortfolioTransfer ", gPMConstants.PMELogLevel.PMLogError)
                End If

                .Parameters.Add("this_revision", 0, PMConst.PMParamOutput, PMConst.PMCurrency)
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed to add parameter this_revision ", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_nReturn = .SQLAction(
                        sSQL:=ACNetOfClaimPerilReserveSQL,
                        sSQLName:=ACGetClaimPerilsName,
                        bStoredProcedure:=ACGetClaimPerilsStored)

                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed NetOfClaimPerils ", gPMConstants.PMELogLevel.PMLogError)
                End If

                r_cThisRevision = ToSafeCurrency(.Parameters.Item("this_revision").Value, 0)
            End With

        Catch ex As Exception
            Return gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername,
                v_sClass:=ACClass,
                v_sMethod:="NetOfClaimPerils",
                r_lFunctionReturn:=NetOfClaimPerils, excep:=ex)
        End Try

    End Function

    Private Function ProcessClaimReinsurance(ByVal v_nclaimCnt As Integer, ByVal v_iIsCreated As Integer) As Integer

        Dim sMessage As String = ""

        Const kMethodName As String = "ProcessClaimReinsurance"

        Try

            ProcessClaimReinsurance = gPMConstants.PMEReturnCode.PMTrue

            If v_iIsCreated = 1 Then
                m_oReinsuranceClaim.IsCreated = 1
            End If
            m_oReinsuranceClaim.ClaimId = v_nclaimCnt
            m_oReinsuranceClaim.Task = PMConst.PMEdit
            m_nReturn = m_oReinsuranceClaim.CalculateRI()

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "bCLMReinsurance.CalculateRI Failed for " & v_nclaimCnt
                RaiseError(kMethodName, "Failed NetOfClaimPerils ", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As System.Exception
            Return gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername,
                v_sClass:=ACClass,
                v_sMethod:="ProcessClaimReinsurance",
                r_lFunctionReturn:=ProcessClaimReinsurance, excep:=ex)

        End Try
    End Function
    ''' <summary>
    ''' GetAllClaimsOnPolicy
    ''' </summary>
    ''' <param name="v_nInsuranceFileCnt"></param>
    ''' <param name="r_oResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetAllClaimsOnPolicy(ByVal v_nInsuranceFileCnt As Integer, ByRef r_oResultArray As Object) As Integer

        Const kMethodName As String = "GetAllClaimsOnPolicy"

        Try

            GetAllClaimsOnPolicy = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add parameters
            m_nReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt",
                                                    vValue:=CInt(v_nInsuranceFileCnt),
                                                    iDirection:=PMConst.PMParamInput,
                                                    iDataType:=PMConst.PMLong)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed NetOfClaimPerils ", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Get policies
            m_nReturn = m_oDatabase.SQLSelect(
                    sSQL:=ACGetAllClaimsOnRiskSQL,
                    sSQLName:=ACGetAllClaimsOnRiskName,
                    bStoredProcedure:=ACGetAllClaimsOnRiskStored,
                    vResultArray:=r_oResultArray)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed GetAllClaimsOnPolicy ", gPMConstants.PMELogLevel.PMLogError)

            End If

        Catch excep As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            Return gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername,
                v_sClass:=ACClass,
                v_sMethod:="GetAllClaimsOnPolicy",
                r_lFunctionReturn:=GetAllClaimsOnPolicy, excep:=excep)

        End Try

    End Function
    ''' <summary>
    ''' ClaimReversingPrePolicyTransfer
    ''' </summary>
    ''' <param name="v_nInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ClaimReversingPrePolicyTransfer(ByVal v_nInsuranceFileCnt As Integer) As Integer

        Const kMethodName As String = "ClaimReversingPrePolicyTransfer"

        Dim nReturnValue As Integer
        Dim m_oFindClaim As Object = Nothing = Nothing
        Dim nNewClaimId As Integer
        Dim nClaimsCount As Integer

        Try

            nReturnValue = gPMConstants.PMEReturnCode.PMTrue

            ' Create Object bCLMFindClaim
            nReturnValue = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oFindClaim,
                                                                v_sClassName:="bCLMFindClaim.Business",
                                                                v_sCallingAppName:=ACApp,
                                                                v_sUsername:=m_sUsername,
                                                                v_sPassword:=m_sPassword,
                                                                v_iUserID:=m_iUserID,
                                                                v_iSourceID:=m_nSourceID,
                                                                v_iLanguageID:=m_nLanguageID,
                                                                v_iCurrencyID:=m_nCurrencyID,
                                                                v_iLogLevel:=m_nLogLevel,
                                                                v_oDatabase:=m_oDatabase)
            If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed ClaimReversingPrePolicyTransfer ", gPMConstants.PMELogLevel.PMLogError)

            End If

            If GetAllClaimsOnPolicy(v_nInsuranceFileCnt:=CInt(v_nInsuranceFileCnt),
                                    r_oResultArray:=m_oClaimsArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed ClaimReversingPrePolicyTransfer ", gPMConstants.PMELogLevel.PMLogError)

            End If

            ' Process all claims
            If Informations.IsArray(m_oClaimsArray) Then
                For nClaimsCount = 0 To DirectCast(m_oClaimsArray, Object(,)).GetUpperBound(1)
                    ' Copy the claim by passing MAX ClaimId of Base Claim
                    ' Question : Do we need to check Remaining Reservev before processing claims ?
                    nReturnValue = m_oFindClaim.SetProcessModes(vTransactionType:="C_CR")
                    nReturnValue = m_oFindClaim.ProcessCopyClaim(v_lClaimId:=m_oClaimsArray(1, nClaimsCount), r_lCopyClaimId:=CInt(nNewClaimId))
                    If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                        'RaiseError "ProcessCopyClaim", "Failed to Copy claim for Claim Id " & m_vClaimsArray(0, lClaimsCount)
                        RaiseError(kMethodName, "Failed ClaimReversingPrePolicyTransfer ", gPMConstants.PMELogLevel.PMLogError)

                    End If
                    If nNewClaimId > 0 Then
                        m_oClaimsArray(4, nClaimsCount) = nNewClaimId
                    End If

                    ' No need to MoveClaimToNewRisk
                    'Process Claim Perils and Reserves
                    nReturnValue = ProcessClaimPerils(v_nclaimCnt:=CInt(nNewClaimId),
                                                    v_nPrePortfolioTransfer:=1,
                                                    v_nInsuranceFileCnt:=CInt(v_nInsuranceFileCnt),
                                                    v_nIsCreated:=1)
                    If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                        'RaiseError "ProcessClaimPerils", "Failed to NetOf Perils for Claim" & m_vClaimsArray(0, lClaimsCount)

                        RaiseError(kMethodName, "Failed ClaimReversingPrePolicyTransfer ", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Next
            End If

        Catch excep As System.Exception
            'DO Not Call any functions before here or the error will be lost
            Return gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername,
                v_sClass:=ACClass,
                v_sMethod:="GetAllClaimsOnPolicy",
                r_lFunctionReturn:=ClaimReversingPrePolicyTransfer, excep:=excep)

        End Try
        Return nReturnValue
    End Function

    Private Function AddClaimsStatsFolder(ByVal v_lInsuranceFileCnt As Long,
                                            ByVal v_sDebitCredit As String,
                                            ByVal v_sDocumentComment As String,
                                            ByVal v_iTransactionTypeId As Integer,
                                            ByVal v_sTransactionTypeCode As String,
                                            ByVal v_iUserID As Integer,
                                            ByVal v_sUsername As String,
                                            ByVal v_iClaimId As Integer,
                                            ByVal v_iDocumentTypeId As Integer,
                                            ByRef r_iStatsFolderCnt As Integer) As Long

        Const kMethodName As String = "AddClaimsStatsFolder"

        Try

            AddClaimsStatsFolder = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            ' Add parameters
            m_nReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt",
                                                 vValue:=CInt(v_lInsuranceFileCnt),
                                                 iDirection:=PMConst.PMParamInput,
                                                 iDataType:=PMConst.PMLong)

            m_nReturn = m_oDatabase.Parameters.Add(sName:="debit_credit",
                                                 vValue:=v_sDebitCredit,
                                                 iDirection:=PMConst.PMParamInput,
                                                 iDataType:=PMConst.PMString)


            m_nReturn = m_oDatabase.Parameters.Add(sName:="document_comment",
                                                 vValue:=v_sDocumentComment,
                                                 iDirection:=PMConst.PMParamInput,
                                                 iDataType:=PMConst.PMString)


            m_nReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_id",
                                                 vValue:=v_iTransactionTypeId,
                                                 iDirection:=PMConst.PMParamInput,
                                                 iDataType:=PMConst.PMInteger)


            m_nReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_code",
                                                 vValue:=CStr(v_sTransactionTypeCode),
                                                 iDirection:=PMConst.PMParamInput,
                                                 iDataType:=PMConst.PMString)


            m_nReturn = m_oDatabase.Parameters.Add(sName:="user_id",
                                                 vValue:=v_iUserID,
                                                 iDirection:=PMConst.PMParamInput,
                                                 iDataType:=PMConst.PMInteger)


            m_nReturn = m_oDatabase.Parameters.Add(sName:="user_name",
                                                 vValue:=v_sUsername,
                                                 iDirection:=PMConst.PMParamInput,
                                                 iDataType:=PMConst.PMString)


            m_nReturn = m_oDatabase.Parameters.Add(sName:="claim_id",
                                                 vValue:=v_iClaimId,
                                                 iDirection:=PMConst.PMParamInput,
                                                 iDataType:=PMConst.PMInteger)


            m_nReturn = m_oDatabase.Parameters.Add(sName:="documenttype_id",
                                                 vValue:=v_iDocumentTypeId,
                                                 iDirection:=PMConst.PMParamInput,
                                                 iDataType:=PMConst.PMInteger)


            m_nReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt",
                                                 vValue:=0,
                                                 iDirection:=PMConst.PMParamOutput,
                                                 iDataType:=PMConst.PMInteger)


            m_nReturn = m_oDatabase.SQLAction(
                sSQL:=ACAddStatsFolderClaimsSQL,
                sSQLName:=ACAddStatsFolderClaimsName,
                bStoredProcedure:=ACAddStatsFolderClaimsStored)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed AddClaimsStatsFolder ", gPMConstants.PMELogLevel.PMLogError)

            End If

            ' Output parameter
            r_iStatsFolderCnt = ToSafeInteger(m_oDatabase.Parameters.Item("stats_folder_cnt").Value, 0)


        Catch excep As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            Return gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername,
                v_sClass:=ACClass,
                v_sMethod:="AddClaimsStatsFolder",
                r_lFunctionReturn:=AddClaimsStatsFolder)
        End Try

    End Function

    Private Function GetNetOfClaimPerils(ByVal v_lClaimId As Long,
                                        ByRef dtResult As DataTable) As Long
        Const kMethodName As String = "GetNetOfClaimPerils"

        Try

            GetNetOfClaimPerils = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", CLng(v_lClaimId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Get policies
            m_nReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACGetNetOfClaimPerilSQL, sSQLName:=ACGetNetOfClaimPerilName, bStoredProcedure:=ACGetNetOfClaimPerilStored, oRecordset:=dtResult)


            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed GetNetOfClaimPerils ", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch excep As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            Return gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername,
                v_sClass:=ACClass,
                v_sMethod:="GetNetOfClaimPerils",
                r_lFunctionReturn:=GetNetOfClaimPerils)

        End Try

    End Function

    Private Function AddClaimsStatsDetail(ByVal v_iStatsFolderCnt As Integer,
                                            ByVal v_lClaimId As Long,
                                            ByVal v_iClaimPerilId As Integer,
                                            ByVal v_sStatsDetailType As String,
                                            ByVal v_iClassOfBusiness As Integer,
                                            ByVal v_sClassOfBusinessCode As String,
                                            ByVal v_iRIPartyCnt As Integer,
                                            ByVal v_sCreditAccountCode As String,
                                            ByVal v_iRIPartyType As Integer,
                                            ByVal v_dRISharePercent As Double,
                                            ByVal v_crTransactionAmount As Decimal,
                                            ByVal v_iDocumentTypeId As Integer) As Long


        Const kMethodName As String = "AddClaimsStatsDetail"

        Try


            AddClaimsStatsDetail = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            ' Add parameters
            m_nReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt",
                                                 vValue:=v_iStatsFolderCnt,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_nReturn = m_oDatabase.Parameters.Add(sName:="claim_id",
                                                 vValue:=CInt(v_lClaimId),
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMLong)

            m_nReturn = m_oDatabase.Parameters.Add(sName:="peril_id",
                                                 vValue:=v_iClaimPerilId,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_nReturn = m_oDatabase.Parameters.Add(sName:="stats_detail_type",
                                                 vValue:=v_sStatsDetailType,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMString)


            m_nReturn = m_oDatabase.Parameters.Add(sName:="class_of_business_id",
                                                 vValue:=v_iClassOfBusiness,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_nReturn = m_oDatabase.Parameters.Add(sName:="class_of_business_code",
                                                 vValue:=v_sClassOfBusinessCode,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMString)


            m_nReturn = m_oDatabase.Parameters.Add(sName:="ri_party_cnt",
                                                 vValue:=v_iRIPartyCnt,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_nReturn = m_oDatabase.Parameters.Add(sName:="ri_shortname",
                                                 vValue:=v_sCreditAccountCode,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMString)

            m_nReturn = m_oDatabase.Parameters.Add(sName:="ri_party_type",
                                                 vValue:=v_iRIPartyType,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_nReturn = m_oDatabase.Parameters.Add(sName:="ri_share_percent",
                                                 vValue:=v_dRISharePercent,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMDouble)


            m_nReturn = m_oDatabase.Parameters.Add(sName:="transaction_amount",
                                                 vValue:=v_crTransactionAmount,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMCurrency)

            m_nReturn = m_oDatabase.Parameters.Add(sName:="documenttype_id",
                                                 vValue:=v_iDocumentTypeId,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_nReturn = m_oDatabase.SQLAction(
                sSQL:=ACAddStatsDetailsClaimsSQL,
                sSQLName:=ACAddStatsDetailsClaimsName,
                bStoredProcedure:=ACAddStatsDetailsClaimsStored)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed AddClaimsStatsDetail ", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch excep As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            Return gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername,
                v_sClass:=ACClass,
                v_sMethod:="AddClaimsStatsDetail",
                r_lFunctionReturn:=AddClaimsStatsDetail)

        End Try
    End Function

    Private Function FinaliseStats(ByVal v_lClaimId As Integer,
                                    ByVal v_lTransactionTypeId As Integer,
                                    ByVal v_sTransactionTypeCode As String,
                                    ByVal v_lStatsFolderCnt As Integer,
                                    ByVal v_lStatsSuppressed As Integer) As Long

        Const kMethodName As String = "FinaliseStats"

        Try

            FinaliseStats = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            ' Add parameters

            m_nReturn = m_oDatabase.Parameters.Add(sName:="claim_id",
                                                 vValue:=CInt(v_lClaimId),
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMLong)


            m_nReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_id",
                                                 vValue:=v_lTransactionTypeId,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMLong)

            m_nReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_code",
                                                 vValue:=CStr(v_sTransactionTypeCode),
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMString)

            m_nReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt",
                                                 vValue:=v_lStatsFolderCnt,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_nReturn = m_oDatabase.Parameters.Add(sName:="bstatssuppressed",
                                                 vValue:=v_lStatsSuppressed,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMInteger)


            m_nReturn = m_oDatabase.SQLAction(
                sSQL:=ACUpdateClaimFinaliseStatsSQL,
                sSQLName:=ACUpdateClaimFinaliseStatsName,
                bStoredProcedure:=ACUpdateClaimFinaliseStatsStored)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed FinaliseStats ", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch excep As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            Return gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername,
                v_sClass:=ACClass,
                v_sMethod:="FinaliseStats",
                r_lFunctionReturn:=FinaliseStats)

        End Try
    End Function

    ''' <summary>
    ''' UpdateClaimIsDirtyFlag
    ''' </summary>
    ''' <param name="v_nclaimCnt"></param>
    ''' <param name="v_nIsDirty"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateClaimIsDirtyFlag(ByVal v_nclaimCnt As Integer,
                                    ByVal v_nIsDirty As Integer) As Integer

        Dim nReturn As Integer
        Const kMethodName As String = "UpdateClaimIsDirtyFlag"

        Try

            nReturn = PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            ' Add parameters

            nReturn = m_oDatabase.Parameters.Add(sName:="claim_id",
                                                 vValue:=v_nclaimCnt,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMLong)

            nReturn = m_oDatabase.Parameters.Add(sName:="is_dirty",
                                                 vValue:=v_nIsDirty,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMInteger)

            nReturn = m_oDatabase.SQLAction(
                        sSQL:=ACUpdateClaimStatusSQL,
                        sSQLName:=ACUpdateClaimStatusName,
                        bStoredProcedure:=ACUpdateClaimStatusStored)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed UpdateClaimIsDirtyFlag ", gPMConstants.PMELogLevel.PMLogError)
            End If
        Catch excep As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            ' DO Not Call any functions before here or the error will be lost
            Return gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername,
                v_sClass:=ACClass,
                v_sMethod:="UpdateClaimIsDirtyFlag",
                r_lFunctionReturn:=UpdateClaimIsDirtyFlag, excep:=excep)
            Return nReturn

        End Try
        Return nReturn
    End Function

    Public Function FinaliseClaimDetails(
                        ByVal v_lClaimId As Long,
                        ByVal v_sClaimVersionDescription As String) As Long

        Const kMethodName As String = "FinaliseClaimDetails"

        Dim oChangeClaimStatus As Object = Nothing

        Try

            FinaliseClaimDetails = gPMConstants.PMEReturnCode.PMTrue

            m_nReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oChangeClaimStatus,
                                                                v_sClassName:="bCLMChangeClaimStatus.Business",
                                                                v_sCallingAppName:=m_sCallingAppName,
                                                                v_sUsername:=m_sUsername,
                                                                v_sPassword:=m_sPassword,
                                                                v_iUserID:=m_iUserID,
                                                                v_iSourceID:=m_nSourceID,
                                                                v_iLanguageID:=m_nLanguageID,
                                                                v_iCurrencyID:=m_nCurrencyID,
                                                                v_iLogLevel:=m_nLogLevel,
                                                                v_oDatabase:=m_oDatabase)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed FinaliseClaimDetails ", gPMConstants.PMELogLevel.PMLogError)

            End If

            m_nReturn = oChangeClaimStatus.FinaliseClaimDetails(v_lClaimId:=CInt(v_lClaimId), v_sClaimVersionDescription:="Portfolio Claim Adjustment")

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed FinaliseClaimDetails ", gPMConstants.PMELogLevel.PMLogError)

            End If

        Catch excep As System.Exception
            Return gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername,
                v_sClass:=ACClass,
                v_sMethod:="FinaliseClaimDetails",
                r_lFunctionReturn:=FinaliseClaimDetails)

        Finally

            oChangeClaimStatus = Nothing
        End Try

    End Function


    Public Function GetPolicyType(
            ByVal v_lInsuranceFileCnt As Long,
            ByRef r_lInsuranceFileTypeId As Long) As Long

        Dim dtResult As New DataTable
        Const kMethodName As String = "GetPolicyType"

        Try

            ' set the params
            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "Insurance_File_Cnt", CInt(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


            m_nReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACGetInsuranceRefSQL, sSQLName:=ACGetInsuranceRefName, bStoredProcedure:=ACGetInsuranceRefStored, oRecordset:=dtResult)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed GetPolicyType ", gPMConstants.PMELogLevel.PMLogError)

            End If


            If dtResult IsNot Nothing AndAlso dtResult.Rows IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
                r_lInsuranceFileTypeId = CLng(dtResult.Rows(0)(1).ToString)
            Else
                GetPolicyType = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

        Catch excep As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            Return gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername,
                v_sClass:=ACClass,
                v_sMethod:="GetPolicyType",
                r_lFunctionReturn:=GetPolicyType)
        End Try
        Return m_nReturn
    End Function
    ''' <summary>
    ''' GetPreviousRiskCnt
    ''' </summary>
    ''' <param name="v_nPreInsuranceFileCnt"></param>
    ''' <param name="v_nRiskCnt"></param>
    ''' <param name="v_nInsuranceFileCnt"></param>
    ''' <param name="r_nPreviousRiskCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPreviousRiskCnt(
                        ByVal v_nPreInsuranceFileCnt As Integer,
                        ByVal v_nRiskCnt As Integer,
                        ByVal v_nInsuranceFileCnt As Integer,
                        ByRef r_nPreviousRiskCnt As Integer) As Integer

        Dim dtResult As New DataTable
        Const kMethodName As String = "GetPreviousRiskCnt"
        Try

            GetPreviousRiskCnt = gPMConstants.PMEReturnCode.PMTrue

            r_nPreviousRiskCnt = 0

            'Send the new file in
            m_oDatabase.Parameters.Clear()

            ' Add Parameter
            bPMAddParameter.AddParameterLite(m_oDatabase, "Insurance_file_cnt", CInt(v_nPreInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "Risk_cnt", CInt(v_nRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "processed_Insurance_file_cnt", CInt(v_nInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Execute the SP
            m_nReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACGetPreviousRiskCntForTransferSQL, sSQLName:=ACGetPreviousRiskCntForTransferName, bStoredProcedure:=True, oRecordset:=dtResult)

            'Determine the result
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed GetPolicyType ", gPMConstants.PMELogLevel.PMLogError)
            ElseIf dtResult IsNot Nothing AndAlso dtResult.Rows IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
                r_nPreviousRiskCnt = CLng(dtResult.Rows(0)(0).ToString)
            End If

            Return m_nReturn

        Catch excep As System.Exception

            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername,
                v_sClass:=ACClass,
                v_sMethod:="GetPreviousRiskCnt",
                r_lFunctionReturn:=GetPreviousRiskCnt, excep:=excep)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: CopyRisk (Private)
    '
    ' Desc: copy risk details from OldInsuranceFileCnt to NewInsuranceFileCnt
    '
    ' Changes: RWH(20/11/2000) Updated for new Risk database structure
    '           which uses Insurance_file_risk_link table rather than
    '           storing insurance_file_cnt on the Risk table.
    ' ***************************************************************** '
    Public Function CopyRisk(ByVal v_nNewInsuranceFileCnt As Integer,
                            ByVal v_oRiskDetail As Object,
                            ByVal v_nPosNo As Integer,
                            ByRef r_nRiskCnt As Integer,
                            Optional ByVal v_nResetStatus As Integer = 0,
                            Optional ByVal v_nCreateLinkType As Integer = 0,
                            Optional ByVal v_bAutoCancellation As Boolean = False,
                            Optional v_sRiskMergeStatus As String = "",
                            Optional v_nOldRiskCnt As Integer = 0) As Integer

        Dim nRiskCnt As Integer
        Dim nOldRiskCnt As Integer
        Dim nNewRiskCnt As Integer
        Dim dtResult As New DataTable
        Dim nIsAutoReinsured As Integer
        Dim oArray As Object
        Const kMethodName As String = "CopyRisk"
        Try

            m_nReturn = gPMConstants.PMEReturnCode.PMTrue

            nOldRiskCnt = v_oRiskDetail(0, v_nPosNo)

            'Tomo030801
            'This bit's here because we need to reset the auto reinsured flag to that
            'from the risk type.
            m_oDatabase.Parameters.Clear()

            ' Add Parameter
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_type_id", v_oRiskDetail(4, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_nReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACGetAutoReinsuredSQL, sSQLName:=ACGetAutoReinsuredName, bStoredProcedure:=ACGetAutoReinsuredStored, oRecordset:=dtResult)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed GetPolicyType ", gPMConstants.PMELogLevel.PMLogError)
            End If

            If dtResult IsNot Nothing AndAlso dtResult.Rows IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
                nIsAutoReinsured = CInt(dtResult.Rows(0)(0))
            Else
                nIsAutoReinsured = 0
            End If

            v_oRiskDetail(20, v_nPosNo) = nIsAutoReinsured

            oArray = ""

            m_oDatabase.Parameters.Clear()
            ' Add Parameter
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            'TN20010719 - start
            If v_nResetStatus = gPMConstants.PMEReturnCode.PMTrue Then
                'reset status to UnQuoted
                bPMAddParameter.AddParameterLite(m_oDatabase, "risk_status_id", 4, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                'TN20010719 - end
            Else
                bPMAddParameter.AddParameterLite(m_oDatabase, "risk_status_id", v_oRiskDetail(1, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            End If

            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_folder_cnt", v_oRiskDetail(2, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "accumulation_id", v_oRiskDetail(3, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_type_id", v_oRiskDetail(4, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "description", v_oRiskDetail(5, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            bPMAddParameter.AddParameterLite(m_oDatabase, "sequence_number", v_oRiskDetail(6, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured_requested", v_oRiskDetail(7, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "inception_date", v_oRiskDetail(8, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "expiry_date", v_oRiskDetail(9, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "is_not_index_linked", v_oRiskDetail(10, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            bPMAddParameter.AddParameterLite(m_oDatabase, "is_accumulated", v_oRiskDetail(11, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            bPMAddParameter.AddParameterLite(m_oDatabase, "lapsed_reason_id", v_oRiskDetail(12, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "lapsed_date", v_oRiskDetail(13, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "lapsed_description", v_oRiskDetail(14, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            bPMAddParameter.AddParameterLite(m_oDatabase, "var_data_ref", v_oRiskDetail(15, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "total_sum_insured", v_oRiskDetail(16, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "total_annual_premium", v_oRiskDetail(17, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "total_this_premium", v_oRiskDetail(18, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "is_ri_at_risk_level", v_oRiskDetail(19, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            bPMAddParameter.AddParameterLite(m_oDatabase, "is_auto_reinsured", v_oRiskDetail(20, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            bPMAddParameter.AddParameterLite(m_oDatabase, "gis_screen_id", v_oRiskDetail(21, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "eml_percentage", v_oRiskDetail(22, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_number", v_oRiskDetail(23, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            ' PW311002
            bPMAddParameter.AddParameterLite(m_oDatabase, "variation_number", v_oRiskDetail(24, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            bPMAddParameter.AddParameterLite(m_oDatabase, "is_risk_selected", v_oRiskDetail(25, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            bPMAddParameter.AddParameterLite(m_oDatabase, "coverage", v_oRiskDetail(26, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            bPMAddParameter.AddParameterLite(m_oDatabase, "insured_item", v_oRiskDetail(27, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            bPMAddParameter.AddParameterLite(m_oDatabase, "extensions", v_oRiskDetail(28, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_this_year", v_oRiskDetail(31, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "is_mandatory_risk", v_oRiskDetail(36, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            m_nReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACAddRiskSQL, sSQLName:=ACAddRiskName, bStoredProcedure:=ACAddRiskStored, oRecordset:=dtResult)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed GetPolicyType ", gPMConstants.PMELogLevel.PMLogError)
            End If

            nRiskCnt = m_oDatabase.Parameters.Item("risk_cnt").Value

            If v_bAutoCancellation = False Then

                'RWH(20/11/2000) Add link record into Insurance_file_risk_link.
                If (nRiskCnt <> 0) Then

                    ' Determine whether or not to create a link and
                    ' if so what kind of link
                    Select Case v_nCreateLinkType

                        Case 0 ' standard - original and renewed risk cnt not populated
                            m_nReturn = m_oRiskData.AddRiskLink(
                                        CInt(v_nNewInsuranceFileCnt),
                                        CInt(nRiskCnt),
                                        "C", 0, 0)

                        Case 1 ' populate original_risk_cnt
                            m_nReturn = m_oRiskData.AddRiskLink(
                                CInt(v_nNewInsuranceFileCnt),
                                CInt(nRiskCnt),
                                "C",
                                CInt(v_nOldRiskCnt), 0)

                        Case 2 ' populate renewed_risk_cnt
                            m_nReturn = m_oRiskData.AddRiskLink(
                                CInt(v_nNewInsuranceFileCnt),
                                CInt(nRiskCnt),
                                "C",
                                0,
                                CInt(v_nOldRiskCnt))

                        Case Else
                            ' do nothing
                    End Select

                    If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed GetPolicyType ", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    r_nRiskCnt = nRiskCnt

                    m_nReturn = m_oRiskData.CopyRatingSection(v_lOldRiskCnt:=nOldRiskCnt,
                                                  v_lNewRiskCnt:=nRiskCnt)

                    If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed GetPolicyType ", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

            Else

                ' delete the original insurance file risk link
                m_nReturn = m_oRiskData.DeleteInsuranceFileRiskLink(
                    v_lInsuranceFileCnt:=v_nNewInsuranceFileCnt,
                    v_lRiskCnt:=v_nOldRiskCnt)
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed GetPolicyType ", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' add new
                m_nReturn = m_oRiskData.AddRiskLink(
                    v_nNewInsuranceFileCnt,
                    nRiskCnt,
                    If(v_sRiskMergeStatus = "DP", "D", "C"),
                    v_nOldRiskCnt)

                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed GetPolicyType ", gPMConstants.PMELogLevel.PMLogError)
                End If

                r_nRiskCnt = nRiskCnt
                nNewRiskCnt = nRiskCnt

                m_nReturn = m_oRiskData.CopyRiskExtras(v_lOldRiskCnt:=v_nOldRiskCnt,
                                          v_lNewRiskCnt:=nNewRiskCnt)

                'sj 20/12/2002 - end
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed CopyRisk ", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

        Catch excep As System.Exception

            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername,
                v_sClass:=ACClass,
                v_sMethod:="CopyRisk",
                r_lFunctionReturn:=CopyRisk, excep:=excep)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
        Return m_nReturn
    End Function

    ''' <summary>
    ''' CopyRatings
    ''' </summary>
    ''' <param name="v_nInsuranceFileCnt"></param>
    ''' <param name="r_nOriginalRiskCnt"></param>
    ''' <param name="r_nRiskCnt"></param>
    ''' <param name="dProRataRate"></param>
    ''' <remarks></remarks>
    Public Function CopyRatings(ByVal v_nInsuranceFileCnt As Integer, ByVal r_nOriginalRiskCnt As Integer, ByVal r_nRiskCnt As Integer, ByVal dProRataRate As Double, Optional ByVal IsDeletedRisk As Boolean = False, Optional ByVal New_InsuranceFileCnt As Integer = 0, Optional ByVal nInsuranceFileTypeId As Integer = 0, Optional ByVal v_nPreviousRiskCnt As Integer = 0) As Integer

        Dim j As Integer
        Dim cProrataRate As Double

        Dim dtResult As New DataTable

        Const kMethodName As String = "CopyRatings"

        Try
            m_nReturn = gPMConstants.PMEReturnCode.PMTrue

            'All unedited Risks go through without any pro-rata
            cProrataRate = dProRataRate

            m_oDatabase.Parameters.Clear()

            m_nReturn = m_oDatabase.Parameters.Add(sName:="Risk_Cnt",
                                       vValue:=r_nRiskCnt,
                                       iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                       iDataType:=gPMConstants.PMEDataType.PMLong)


            If IsDeletedRisk Then
                m_nReturn = m_oDatabase.SQLAction(sSQL:=kDelPerilForDeletedRiskSQL,
                                                  sSQLName:=kDelPerilForDeletedRiskName,
                                                  bStoredProcedure:=kDelPerilForDeletedRiskStored)
            Else
                m_nReturn = m_oDatabase.SQLAction(sSQL:=ACDelPerilSQL,
                                                  sSQLName:=ACDelPerilName,
                                                  bStoredProcedure:=ACDelPerilStored)
            End If
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed CopyRatings ", gPMConstants.PMELogLevel.PMLogError)

            End If
            'Del Rating Sections
            m_oDatabase.Parameters.Clear()

            m_nReturn = m_oDatabase.Parameters.Add(sName:="Risk_Cnt",
                                       vValue:=r_nRiskCnt,
                                       iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                       iDataType:=gPMConstants.PMEDataType.PMLong)
            If IsDeletedRisk Then
                m_nReturn = m_oDatabase.SQLAction(sSQL:=kDelRatingSectionForDeletedRiskSQL,
                                                  sSQLName:=kDelRatingSectionForDeletedRiskName,
                                                  bStoredProcedure:=kDelRatingSectionForDeletedRiskStored)
            Else
                m_nReturn = m_oDatabase.SQLAction(sSQL:=ACDelRatingSectionSQL,
                                                  sSQLName:=ACDelRatingSectionName,
                                                  bStoredProcedure:=ACDelRatingSectionStored)
            End If
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed CopyRatings ", gPMConstants.PMELogLevel.PMLogError)

            End If
            'Get Original Rating Sections
            m_oDatabase.Parameters.Clear()

            ' Add Parameter
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", CInt(v_nInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_id", CInt(r_nOriginalRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Fetch the records
            m_nReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACSelectRatingSectionSQL, sSQLName:=ACSelectRatingSectionName, bStoredProcedure:=ACSelectRatingSectionStored, oRecordset:=dtResult)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed CopyRatings ", gPMConstants.PMELogLevel.PMLogError)
            End If

            If dtResult IsNot Nothing AndAlso dtResult.Rows IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then

                If IsDeletedRisk Then
                    For Each row As DataRow In dtResult.Rows
                        m_nReturn = CopyRatingSectionsAndPerils(dtResult, -1, 1, v_nInsuranceFileCnt, r_nRiskCnt, j, cProrataRate, r_nOriginalRiskCnt)
                        If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return m_nReturn
                        End If

                    Next
                    For Each row As DataRow In dtResult.Rows
                        m_nReturn = CopyRatingSectionsAndPerils(dtResult, 1, 0, v_nInsuranceFileCnt, r_nRiskCnt, j, cProrataRate, 0, If(nInsuranceFileTypeId = 8, False, IsDeletedRisk))
                        If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return m_nReturn
                        End If

                    Next
                Else
                    For Each row As DataRow In dtResult.Rows
                        m_nReturn = CopyRatingSectionsAndPerils(dtResult, 1, 0, v_nInsuranceFileCnt, r_nRiskCnt, j, cProrataRate)

                        If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "Failed CopyRatings ", gPMConstants.PMELogLevel.PMLogError)

                        End If
                        j = j + 1
                    Next
                End If
            End If

        Catch excep As System.Exception
            m_nReturn = PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername,
                v_sClass:=ACClass,
                v_sMethod:="CopyRatings",
                r_lFunctionReturn:=m_nReturn)
            Return m_nReturn
        End Try
        Return m_nReturn
    End Function

    ''' <summary>
    ''' CopyRatingSectionsAndPerils
    ''' </summary>
    ''' <param name="v_dtResult"></param>
    ''' <param name="v_nThisPremiumSign"></param>
    ''' <param name="v_nOriginalFlag"></param>
    ''' <param name="v_nInsuranceFileCnt"></param>
    ''' <param name="v_nRiskCnt"></param>
    ''' <param name="iIndex"></param>
    ''' <param name="dProrata"></param>
    ''' <param name="nPreviousRiskCnt"></param>
    ''' <param name="IsDeletedRisk"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CopyRatingSectionsAndPerils(ByVal v_dtResult As DataTable, ByVal v_nThisPremiumSign As Integer, ByVal v_nOriginalFlag As Integer, ByVal v_nInsuranceFileCnt As Integer, ByVal v_nRiskCnt As Integer, iIndex As Integer, Optional dProrata As Double = 0, Optional ByVal nPreviousRiskCnt As Integer = 0, Optional ByVal IsDeletedRisk As Boolean = False) As Integer

        Dim nPolicyRatingSectionTypeId As Integer
        Dim cThisPremium As Decimal
        Dim nInsuranceFileNoOfDp As Integer
        Const kMethodName As String = "CopyRatingSectionsAndPerils"
        Dim nInsuranceFileType As Long

        Try

            m_nReturn = gPMConstants.PMEReturnCode.PMTrue
            m_nReturn = GetPolicyType(v_nInsuranceFileCnt, nInsuranceFileType)

            nPolicyRatingSectionTypeId = -1
            If Informations.IsNothing(dProrata) Then
                cThisPremium = v_nThisPremiumSign * CInt(v_dtResult.Rows(iIndex)(6).ToString)
            Else
                If nInsuranceFileType = 5 Then
                    cThisPremium = v_nThisPremiumSign * (CInt(v_dtResult.Rows(iIndex)(5).ToString) * dProrata)
                Else
                    cThisPremium = v_nThisPremiumSign * (CInt(v_dtResult.Rows(iIndex)(6).ToString) * dProrata)
                End If
            End If
            nInsuranceFileNoOfDp = 2

            m_nReturn = m_oDatabase.Parameters.Add(
                sName:="rating_section_type_id",
                vValue:=CLng(v_dtResult.Rows(iIndex)(10).ToString),
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMLong)

            m_nReturn = m_oDatabase.Parameters.Add(
                sName:="policy_section_type_id",
                vValue:=nPolicyRatingSectionTypeId,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMLong)

            m_nReturn = m_oDatabase.Parameters.Add(
                sName:="insurance_file_cnt",
                vValue:=CInt(v_nInsuranceFileCnt),
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMLong)

            m_nReturn = m_oDatabase.Parameters.Add(
                sName:="risk_id",
                vValue:=v_nRiskCnt,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMLong)
            If IsDeletedRisk Then
                m_nReturn = m_oDatabase.Parameters.Add(
                    sName:="sum_insured",
                    vValue:=0,
                    iDirection:=PMEParameterDirection.PMParamInput,
                    iDataType:=PMEDataType.PMCurrency)
            Else
                m_nReturn = m_oDatabase.Parameters.Add(
                    sName:="sum_insured",
                    vValue:=ToSafeCurrency(v_dtResult.Rows(iIndex)(4).ToString),
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                    iDataType:=gPMConstants.PMEDataType.PMCurrency)
            End If
            m_nReturn = m_oDatabase.Parameters.Add(
                sName:="annual_rate",
                vValue:=ToSafeCurrency(v_dtResult.Rows(iIndex)(3).ToString),
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMCurrency)

            m_nReturn = m_oDatabase.Parameters.Add(
                sName:="annual_premium",
                vValue:=ToSafeCurrency(v_dtResult.Rows(iIndex)(6).ToString),
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMCurrency)

            m_nReturn = m_oDatabase.Parameters.Add(
                sName:="this_premium",
                vValue:=cThisPremium,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMCurrency)

            m_nReturn = m_oDatabase.Parameters.Add(
                sName:="rate_type_id",
                vValue:=CLng(v_dtResult.Rows(iIndex)(12).ToString),
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMLong)

            m_nReturn = m_oDatabase.Parameters.Add(
                sName:="insurance_file_no_of_dp",
                vValue:=nInsuranceFileNoOfDp,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_nReturn = m_oDatabase.Parameters.Add(
                sName:="original_flag",
                vValue:=v_nOriginalFlag,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMLong)

            If v_dtResult.Rows(iIndex)(14).ToString = "" Then
                m_nReturn = m_oDatabase.Parameters.Add(
                    sName:="currency_id",
                    vValue:=Nothing,
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                    iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_nReturn = m_oDatabase.Parameters.Add(
                  sName:="currency_id",
                  vValue:=CInt(v_dtResult.Rows(iIndex)(14).ToString),
                  iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                  iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If (v_dtResult.Rows(iIndex)(15).ToString) = "" Then
                m_nReturn = m_oDatabase.Parameters.Add(
                    sName:="country_id",
                    vValue:=Nothing,
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                    iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_nReturn = m_oDatabase.Parameters.Add(
                    sName:="country_id",
                    vValue:=CInt(v_dtResult.Rows(iIndex)(15).ToString),
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                    iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If (v_dtResult.Rows(iIndex)(16).ToString) = "" Then
                m_nReturn = m_oDatabase.Parameters.Add(
                    sName:="state_id",
                    vValue:=Nothing,
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                    iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_nReturn = m_oDatabase.Parameters.Add(
                   sName:="state_id",
                   vValue:=CInt(v_dtResult.Rows(iIndex)(16).ToString),
                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                   iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If
            m_nReturn = m_oDatabase.Parameters.Add(
                sName:="is_amended",
                vValue:=CInt(v_dtResult.Rows(iIndex)(17).ToString),
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_nReturn = m_oDatabase.Parameters.Add(
                sName:="calculated_premium",
                vValue:=ToSafeCurrency(CDbl(v_dtResult.Rows(iIndex)(18).ToString), 0),
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMCurrency)

            m_nReturn = m_oDatabase.Parameters.Add(
                sName:="override_reason",
                vValue:=CStr(v_dtResult.Rows(iIndex)(19).ToString),
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMString)

            If nPreviousRiskCnt <> 0 Then
                m_nReturn = m_oDatabase.Parameters.Add(
                   sName:="previous_risk_cnt",
                   vValue:=nPreviousRiskCnt,
                   iDirection:=PMEParameterDirection.PMParamInput,
                   iDataType:=PMEDataType.PMLong)
            End If
            m_nReturn = m_oDatabase.Parameters.Add(
                   sName:="is_pt",
                   vValue:=1,
                   iDirection:=PMEParameterDirection.PMParamInput,
                   iDataType:=PMEDataType.PMLong)

            ' Add Section & Perils
            m_nReturn = m_oDatabase.SQLAction(
                sSQL:=ACAddSectionAndPerilsSQL,
                sSQLName:=ACAddSectionAndPerilsName,
                bStoredProcedure:=ACAddSectionAndPerilsStored)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed CopyRatingSectionsAndPerils ", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch excep As System.Exception

            Return gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername,
                v_sClass:=ACClass,
                v_sMethod:="CopyRatingSectionsAndPerils",
                r_lFunctionReturn:=CopyRatingSectionsAndPerils)

        End Try
        Return m_nReturn
    End Function

    Public Function GetAllRiskStatus(
            ByVal v_lInsuranceFileCnt As Long,
            ByRef r_bIsRisksQuoted As Boolean) As Integer

        Dim dtResult As New DataTable

        Const kMethodName As String = "GetAllRiskStatus"

        Try

            GetAllRiskStatus = gPMConstants.PMEReturnCode.PMTrue
            ' set the params
            m_oDatabase.Parameters.Clear()

            ' Add Parameter
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", CLng(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


            ' get the risk status of the risk
            m_nReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACGetAllRiskStatusSQL, sSQLName:=ACGetAllRiskStatusName, bStoredProcedure:=ACGetAllRiskStatusStored, oRecordset:=dtResult)


            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed GetAllRiskStatus ", gPMConstants.PMELogLevel.PMLogError)
            End If

            If dtResult IsNot Nothing AndAlso dtResult.Rows IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
                r_bIsRisksQuoted = False
            Else
                r_bIsRisksQuoted = True
            End If

        Catch excep As System.Exception

            Return gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername,
                v_sClass:=ACClass,
                v_sMethod:="GetAllRiskStatus",
                r_lFunctionReturn:=GetAllRiskStatus, excep:=excep)

        End Try
    End Function

    ''' <summary>
    ''' GetMaxPolicyVersion
    ''' </summary>
    ''' <param name="v_nInsuranceFileCnt"></param>
    ''' <param name="r_nMaxPolicyVersion"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetMaxPolicyVersion(ByVal v_nInsuranceFileCnt As Integer,
                                    ByRef r_nMaxPolicyVersion As Integer) As Integer

        Try

            m_nReturn = PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()

                ' Add parameters
                m_nReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CInt(v_nInsuranceFileCnt), iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMLong)
                If (m_nReturn <> PMEReturnCode.PMTrue) Then
                    Return m_nReturn
                End If

                m_nReturn = .Parameters.Add(sName:="max_version_no", vValue:=r_nMaxPolicyVersion, iDirection:=PMEParameterDirection.PMParamOutput, iDataType:=PMEDataType.PMLong)
                If (m_nReturn <> PMEReturnCode.PMTrue) Then
                    Return m_nReturn
                End If

                m_nReturn = .SQLAction(
                        sSQL:=kGetMaxPolicyVersionSQL,
                        sSQLName:=kGetMaxPolicyVersionName,
                        bStoredProcedure:=kGetMaxPolicyVersionStored)

                If (m_nReturn <> PMEReturnCode.PMTrue) Then
                    Return m_nReturn
                End If

                r_nMaxPolicyVersion = ToSafeCurrency(.Parameters.Item("max_version_no").Value, 0)
            End With

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            m_nReturn = PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername,
                v_sClass:=ACClass,
                v_sMethod:="GetAllRiskStatus",
                r_lFunctionReturn:=m_nReturn, excep:=ex)
        End Try
        Return m_nReturn
    End Function
    ''' <summary>
    ''' Cancel the policy if policy was already cancelled before portfolio transfer and this is MTA cancel.
    ''' </summary>
    ''' <param name="v_nInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckAndCancelPolicy(ByVal v_nInsuranceFileCnt As Integer) As Integer

        Try

            m_nReturn = PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_nReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt",
                                                vValue:=CInt(v_nInsuranceFileCnt),
                                                iDirection:=PMEParameterDirection.PMParamInput,
                                                iDataType:=PMEDataType.PMLong)

            If m_nReturn <> PMEReturnCode.PMTrue Then
                Return m_nReturn
            End If

            m_nReturn = m_oDatabase.SQLAction(sSQL:=kCheckAndCancelPolicySQL,
                                            sSQLName:=kCheckAndCancelPolicyName,
                                            bStoredProcedure:=kCheckAndCancelPolicyStored)
            If m_nReturn <> PMEReturnCode.PMTrue Then
                m_nReturn = "Failed SQLAction - CheckAndCancelPolicy"
                Return m_nReturn
            End If

        Catch excep As Exception
            m_nReturn = PMEReturnCode.PMError

            bPMFunc.LogError(v_sUsername:=m_sUsername,
              v_sClass:=ACClass,
              v_sMethod:="CheckAndCancelPolicy",
              r_lFunctionReturn:=m_nReturn, excep:=excep)
        End Try
        Return m_nReturn
    End Function
    ''' <summary>
    ''' UpdatePortfolio_Renewal_Status
    ''' </summary>
    ''' <param name="v_nInsuranceFileCnt"></param>
    ''' <param name="v_nNewInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdatePortfolio_Renewal_Status(
            ByVal v_nInsuranceFileCnt As Integer,
            ByVal v_nNewInsuranceFileCnt As Integer
            ) As Integer

        Try

            m_nReturn = PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add parameters
            m_nReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt",
                                                    vValue:=CInt(v_nInsuranceFileCnt),
                                                    iDirection:=PMEParameterDirection.PMParamInput,
                                                    iDataType:=PMEDataType.PMLong)
            If m_nReturn <> PMEReturnCode.PMTrue Then
                Return m_nReturn
            End If

            m_nReturn = m_oDatabase.Parameters.Add(sName:="new_insurance_file_cnt",
                                                    vValue:=CInt(v_nNewInsuranceFileCnt),
                                                   iDirection:=PMEParameterDirection.PMParamInput,
                                                    iDataType:=PMEDataType.PMLong)
            If m_nReturn <> PMEReturnCode.PMTrue Then
                Return m_nReturn
            End If

            ' Get policies
            m_nReturn = m_oDatabase.SQLSelect(
                    sSQL:=kUpdatePortfolioRenewalStatusSQL,
                    sSQLName:=kUpdatePortfolioRenewalStatusName,
                    bStoredProcedure:=kUpdatePortfolioRenewalStatusStored)

            If m_nReturn <> PMEReturnCode.PMTrue Then
                Return m_nReturn
            End If

        Catch excep As Exception

            m_nReturn = PMEReturnCode.PMError
            bPMFunc.LogError(v_sUsername:=m_sUsername,
                 v_sClass:=ACClass,
                 v_sMethod:="GUpdatePortfolio_Renewal_Status",
                 r_lFunctionReturn:=m_nReturn, excep:=excep)
        End Try
        Return m_nReturn
    End Function

    ''' <summary>
    ''' GetUnderRenewalPoliciesCount
    ''' </summary>
    ''' <param name="v_nProductID"></param>
    ''' <param name="v_dtTransferDate"></param>
    ''' <param name="r_oPolicyArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUnderRenewalPoliciesCount(
                ByVal v_nProductID As Integer,
                ByVal v_dtTransferDate As Date,
                ByRef r_oPolicyArray(,) As Object) As Integer

        Try
            m_nReturn = PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()

            ' Add parameters
            m_nReturn = m_oDatabase.Parameters.Add(sName:="product_id",
                                                    vValue:=v_nProductID,
                                                    iDirection:=PMEParameterDirection.PMParamInput,
                                                    iDataType:=PMEDataType.PMLong)
            If m_nReturn <> PMEReturnCode.PMTrue Then
                Return m_nReturn
            End If

            m_nReturn = m_oDatabase.Parameters.Add(sName:="transfer_date",
                                                    vValue:=CDate(v_dtTransferDate),
                                                    iDirection:=PMEParameterDirection.PMParamInput,
                                                    iDataType:=PMEDataType.PMDate)
            If m_nReturn <> PMEReturnCode.PMTrue Then
                Return m_nReturn
            End If
            ' Get policies
            m_nReturn = m_oDatabase.SQLSelect(
                    sSQL:=kGetUnderRenewalPoliciesCountSQL,
                    sSQLName:=kGetUnderRenewalPoliciesCountName,
                    bStoredProcedure:=kGetUnderRenewalPoliciesCountStored,
                    vResultArray:=r_oPolicyArray)

            If m_nReturn <> PMEReturnCode.PMTrue Then
                Return m_nReturn
            End If

        Catch excep As Exception
            m_nReturn = PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername,
             v_sClass:=ACClass,
             v_sMethod:="GUpdatePortfolio_Renewal_Status",
             r_lFunctionReturn:=m_nReturn, excep:=excep)
        End Try
        Return m_nReturn
    End Function
    ''' <summary>
    ''' CheckInRenewal
    ''' </summary>
    ''' <param name="v_nInsuranceFileCnt"></param>
    ''' <param name="r_nRenewalStatus"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckInRenewal(ByVal v_nInsuranceFileCnt As Integer,
                               ByRef r_nRenewalStatus As Integer) As Integer

        Dim oArray(,) As Object = Nothing

        Try

            m_nReturn = PMEReturnCode.PMTrue

            r_nRenewalStatus = -1

            m_oDatabase.Parameters.Clear()

            m_nReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt",
                                                   vValue:=CInt(v_nInsuranceFileCnt),
                                                   iDirection:=PMEParameterDirection.PMParamInput,
                                                   iDataType:=PMEDataType.PMLong)

            If (m_nReturn <> PMEReturnCode.PMTrue) Then
                Return m_nReturn
            End If

            m_nReturn = m_oDatabase.SQLSelect(sSQL:=kCheckRenewalsSQL,
                                              sSQLName:=kCheckRenewalsName,
                                              bStoredProcedure:=kCheckRenewalsStored,
                                              vResultArray:=oArray)

            If (m_nReturn <> PMEReturnCode.PMTrue) Then
                Return m_nReturn
            End If

            If Not Informations.IsArray(oArray) Then
                Return m_nReturn
            End If

            r_nRenewalStatus = oArray(0, 0)
            oArray = Nothing



        Catch excep As Exception
            m_nReturn = PMEReturnCode.PMError
            bPMFunc.LogError(v_sUsername:=m_sUsername,
                 v_sClass:=ACClass,
                 v_sMethod:="CheckInRenewal",
                 r_lFunctionReturn:=m_nReturn, excep:=excep)

        End Try
        Return m_nReturn
    End Function

End Class