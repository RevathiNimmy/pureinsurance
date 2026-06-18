Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SSP.Shared
Imports System.Text
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    '*************************************************************************
    ' Class Name:   Business
    ' Date Created: 07/05/1999
    ' Description:  Creatable Business class which contains all the
    '               methods, Business rules required to process AutoMTAs
    ' History:      10/08/2004 TR - RESILIENCE changes
    '*************************************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' ************************************************
    ' Added to replace global variables 26/11/2003
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database
    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean
    Private m_vOptions As Object
    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode
    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    ' local object
    Private m_oAutoMTA As AutoMta
    Private m_bIsSingleInstalmentPlan As Boolean
    Private m_bIsCalledFromPlanMaintainence As Boolean

    'Parameter "v_lIFolderCnt" should not be optional because
    'ACGetPolicyVersionsSQL should be insurance_folder specific - Amit

    Public Function GetPolicyVersions(ByVal m_lIFileCnt As Integer, ByRef vPolicyVersion(,) As Object, Optional ByVal v_lIfolderCnt As Integer = 0) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="IfileCnt", vValue:=CStr(m_lIFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="IfolderCnt", vValue:=CStr(v_lIfolderCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyVersionsSQL, sSQLName:=ACGetPolicyVersionsName, bStoredProcedure:=False, vResultArray:=vPolicyVersion)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyVersions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function
    ''' <summary>
    ''' Get Backdated Policy Versions
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="oBackdatedMTARiskVersions"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function GetBackdatedPolicyVersions(ByVal nInsuranceFileCnt As Integer, ByRef oBackdatedMTARiskVersions(,) As Object) As Integer

        Try

            GetBackdatedPolicyVersions = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nInsurance_file_cnt", vValue:=nInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_SIR_Get_BackdatedPolicyVersions", sSQLName:="spu_SIR_Get_BackdatedPolicyVersions", bStoredProcedure:=True, vResultArray:=oBackdatedMTARiskVersions)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return gPMConstants.PMEReturnCode.PMTrue
        Catch excep As System.Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBackdatedPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBackdatedPolicyVersions ", vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description, excep:=excep)
            Return gPMConstants.PMEReturnCode.PMError

        End Try

    End Function
    '=================
    'PUBLIC PROPERTIES
    '=================
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property
    Public ReadOnly Property DeclineReasons() As Object
        Get
            'RESILIENCE added to mirror interface object
            Return m_oAutoMTA.DeclineReasons
        End Get
    End Property
    Public ReadOnly Property ReferReasons() As Object
        Get
            'RESILIENCE added to mirror interface object
            Return m_oAutoMTA.ReferReasons
        End Get
    End Property

    Public ReadOnly Property RenewalAffectedByBDMTA() As Boolean
        Get
            Return m_oAutoMTA.CheckRENAffectedbyBDMTA()
        End Get
    End Property
    Public WriteOnly Property IsSingleInstalmentPlan() As Boolean
        Set(ByVal bIsSingleInstalmentPlan As Boolean)
            m_bIsSingleInstalmentPlan = bIsSingleInstalmentPlan
        End Set
    End Property

    Public Property IsCalledFromPlanMaintainence() As Boolean
        Get
            Return m_bIsCalledFromPlanMaintainence
        End Get
        Set(ByVal Value As Boolean)
            m_bIsCalledFromPlanMaintainence = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName
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

            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            'Create internal object
            m_oAutoMTA = New AutoMta()
            'Developer Guide No. 97
            m_lReturn = m_oAutoMTA.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

            ' Passing in the Database object, so that every other business object uses
            ' the same connection, to make sure that transaction handling is made easy
            m_lReturn = CType(m_oAutoMTA.CreateBusinessObjectsLocal(v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAutoMTA.CreateBusinessObjectsLocal Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oAutoMTA.IsSingleInstalmentPlan = m_bIsSingleInstalmentPlan
            m_oAutoMTA.IsCalledFromPlanMaintainence = m_bIsCalledFromPlanMaintainence
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                m_oAutoMTA.Terminate()
                m_oAutoMTA = Nothing
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


            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' AutoCancelMTA
    ''' </summary>
    ''' <param name="v_lPartyCnt"></param>
    ''' <param name="v_lInsuranceFolderCnt"></param>
    ''' <param name="v_dtEffectiveDate"></param>
    ''' <param name="v_bIsSingleInstalmentForCCProcess"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function AutoCancelMTA(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer,
                                  ByVal v_dtEffectiveDate As Date,
                                  Optional ByVal v_bIsSingleInstalmentForCCProcess As Boolean = False) As Integer

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            Dim sErrorText As String = ""

            m_oAutoMTA.PartyCnt = v_lPartyCnt
            m_oAutoMTA.InsuranceFolderCnt = v_lInsuranceFolderCnt
            m_oAutoMTA.TransactionType = "MTC"
            m_oAutoMTA.EffectiveDate = v_dtEffectiveDate
            m_oAutoMTA.UpdateStats = True
            m_oAutoMTA.NewInsuranceFileCnt = 0
            m_oAutoMTA.IsSingleInstalmentForCCProcess = v_bIsSingleInstalmentForCCProcess

            m_lReturn = CType(m_oAutoMTA.AutoCancelMTA(r_sErrorText:=sErrorText), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If sErrorText <> "" Then
                    ' Business component message boxes not allowed
                    ' MsgBox sErrorText, vbOKOnly, "AutoCancelMTA"
                Else
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="m_oAutoMTA.AutoCancelMTA Failed", vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="AutoCancelMTA")
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                End If

                Return nResult
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoCancelMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCancelMTA", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function
    ''' <summary>
    ''' Update Policy Status
    ''' </summary>
    ''' <param name="m_lIFileCnt"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function UpdatePolicyStatus(ByVal m_lIFileCnt As Integer) As Integer

        Dim nResult As Integer = 0


        nResult = gPMConstants.PMEReturnCode.PMTrue
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="IfileCnt", vValue:=CStr(m_lIFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        If m_sTransactionType = "MTR" Then
            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdCANStatusSQL,
                                              sSQLName:=ACUpdCANStatusName,
                                              bStoredProcedure:=False)

        Else
            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdREPStatusSQL, sSQLName:=ACUpdREPStatusName, bStoredProcedure:=False)
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return nResult

    End Function
    ''' <summary>
    ''' Delete MTA Links
    ''' </summary>
    ''' <param name="m_lIFileCnt"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function DeleteMTALinks(ByVal m_lIFileCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="nBaseInsuranceFileCnt", vValue:=CStr(m_lIFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute the stored procedure
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACRestoreMTALinkSQL,
                                  sSQLName:=ACRestoreMTALinkName,
                                  bStoredProcedure:=ACRestoreMTALinkStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function



    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name          : TransactPolicyVersions
    ' Description   :
    ' Notes         : 1. This fuction is copied from iPMUAutoMTA.Interface,
    '                    so that this can be used in the business side
    '                    to make sure that the process is resilient.
    '                 2. Called from bSIRChangePolicyStatus.Stateless class
    '                    in the Generic MTA Roadmap
    ' Edit History  :
    ' RAM20040806   : Created
    ' ***************************************************************** '
    Public Function TransactPolicyVersions(ByVal v_lNewInsuranceFileCnt As Integer, Optional ByVal v_bIsReinstateRisk As Boolean = False, Optional ByVal v_lDeletedRiskCnt As Integer = 0, Optional ByVal v_bIsPostStatsForLapsedPolicy As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oAutoMTA.NewInsuranceFileCnt = v_lNewInsuranceFileCnt
            m_oAutoMTA.DeletedRiskCnt = v_lDeletedRiskCnt
            m_oAutoMTA.IsReinstateRisk = v_bIsReinstateRisk
            m_oAutoMTA.IsPostStatsForLapsedPolicy = v_bIsPostStatsForLapsedPolicy

            m_lReturn = CType(m_oAutoMTA.TransactPolicyVersions(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAutoMTA.TransactPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransactPolicyVersions Failed. v_lNewInsuranceFileCnt = " & v_lNewInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPolicyVersions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)




            Return result
        End Try
    End Function

    '*************************************************************************
    ' Name          : RestoreAutoRunMTA
    ' Description   :
    ' Notes         : 1. This fuction is copied from iPMUAutoMTA.Interface,
    '                    so that this can be used in the business side
    '                    to make sure that the process is resilient.
    '                 2. Called from bSIRChangePolicyStatus.Stateless class
    '                    in the Generic MTA Roadmap
    ' Edit History  :
    ' RAM20040806   : Created
    '*************************************************************************
    Public Function RestoreAutoRunMTA(ByVal v_lNewInsuranceFileCnt As Integer, Optional ByRef v_bKeepQuote As Boolean = True) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oAutoMTA.NewInsuranceFileCnt = v_lNewInsuranceFileCnt

            m_lReturn = CType(m_oAutoMTA.RestoreAutoRunMTA(v_bKeepQuote:=v_bKeepQuote), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAutoMTA.RestoreAutoRunMTA Failed. v_lNewInsuranceFileCnt = " & v_lNewInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="RestoreAutoRunMTA")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RestoreAutoRunMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RestoreAutoRunMTA", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ''' <summary>
    ''' QuoteCancellation-Removed v_vStatusBar as a parameter as the business should
    ''' not update an interface's control's properties directly
    ''' Added r_sFailureMessage to pass useful info to Interface
    ''' </summary>
    ''' <param name="v_lPartyCnt"></param>
    ''' <param name="v_lInsuranceFolderCnt"></param>
    ''' <param name="v_dtEffectiveDate"></param>
    ''' <param name="r_lNewInsuranceFileCnt"></param>
    ''' <param name="r_sFailureMessage"></param>
    ''' <param name="lBaseInsuranceFileCnt"></param>
    ''' <param name="bUpdateStats"></param>
    ''' <param name="bBackDateMTA"></param>
    ''' <param name="vPolicyRef"></param>
    ''' <param name="bIsDirty"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function QuoteCancellation(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer,
                                      ByVal v_dtEffectiveDate As Date, ByRef r_lNewInsuranceFileCnt As Integer,
                                      ByRef r_sFailureMessage As String,
                                      Optional ByVal lBaseInsuranceFileCnt As Integer = 0,
                                      Optional ByVal bUpdateStats As Boolean = False,
                                      Optional ByVal bBackDateMTA As Boolean = False,
                                      Optional ByVal vPolicyRef As String = "",
                                      Optional ByRef bIsDirty As Boolean = False) As Integer '71068 parallel

        Dim nResult As Integer = 0
        Const ksFUNCTION_NAME As String = "QuoteCancellation"

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'WPR 33-75 added
            If Not bUpdateStats Then
                m_lReturn = ClearBackdateMTAData(lBaseInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearBackdateMTAData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteCancellation")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            'Use the local AutoMTA class
            With m_oAutoMTA
                .PartyCnt = v_lPartyCnt
                .InsuranceFolderCnt = v_lInsuranceFolderCnt
                .TransactionType = "MTC"
                .EffectiveDate = v_dtEffectiveDate
                .UpdateStats = bUpdateStats
                .NewInsuranceFileCnt = lBaseInsuranceFileCnt
                .BackDateMTA = bBackDateMTA
                .IsCalledFromPlanMaintainence = m_bIsCalledFromPlanMaintainence
                'WPR 33-75 added
                'Run the autoCancel
                m_lReturn = CType(.AutoCancelMTA(r_sErrorText:=r_sFailureMessage, lBaseInsuranceFileCnt:=lBaseInsuranceFileCnt, v_bIsDirty:=bIsDirty, bUpdateStats:=bUpdateStats), gPMConstants.PMEReturnCode)

                r_lNewInsuranceFileCnt = .NewInsuranceFileCnt
            End With
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="m_oAutoMTA.AutoCancelMTA Failed", vApp:=ACApp, vClass:=ACClass,
                                   vMethod:=ksFUNCTION_NAME)
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="QuoteCancellation Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:=ksFUNCTION_NAME, vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message,
                               excep:=excep)
            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' Quote Reinstatement
    ''' </summary>
    ''' <param name="nPartyCnt"></param>
    ''' <param name="nInsuranceFolderCnt"></param>
    ''' <param name="nNewInsuranceFileCnt"></param>
    ''' <param name="dtEffectiveDate"></param>
    ''' <param name="sFailureMessage"></param>
    ''' <param name="nBaseInsuranceFileCnt"></param>
    ''' <param name="bUpdateStats"></param>
    ''' <param name="bBackDateMTA"></param>
    ''' <param name="oPolicyRef"></param>
    ''' <param name="bIsDirty"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function QuoteReinstatement(ByVal nPartyCnt As Integer,
                                        ByVal nInsuranceFolderCnt As Integer,
                                        ByVal nNewInsuranceFileCnt As Integer,
                                        ByVal dtEffectiveDate As Date,
                                        ByRef sFailureMessage As String,
                                        Optional ByVal nBaseInsuranceFileCnt As Integer = 0,
                                        Optional ByVal bUpdateStats As Boolean = 0,
                                        Optional ByVal bBackDateMTA As Boolean = 0,
                                        Optional ByVal oPolicyRef As Object = "",
                                        Optional bIsDirty As Boolean = False) As Integer

        Dim nResult As Integer = 0

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue
            If bUpdateStats = False Then
                m_lReturn = ClearBackdateMTAData(nBaseInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername,
                                     iType:=gPMConstants.PMEReturnCode.PMError,
                                     sMsg:="ClearBackdateMTAData Failed",
                                     vApp:=ACApp,
                                     vClass:=ACClass,
                                     vMethod:="QuoteReinstatement")
                    Return nResult
                End If
            End If

            'Use the local AutoMTA class
            With m_oAutoMTA
                .PartyCnt = nPartyCnt
                .InsuranceFolderCnt = nInsuranceFolderCnt
                .TransactionType = "MTR"
                .EffectiveDate = dtEffectiveDate
                .UpdateStats = bUpdateStats
                .NewInsuranceFileCnt = nBaseInsuranceFileCnt
                .BackDateMTA = bBackDateMTA

                'Run the reinstate
                m_lReturn = .AutoReinstateMTA(sErrorText:=sFailureMessage, v_bIsBackdatedMTA:=bBackDateMTA, nBaseInsuranceFileCnt:=nBaseInsuranceFileCnt, bIsDirty:=bIsDirty)

            End With
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername,
                                 iType:=gPMConstants.PMEReturnCode.PMError,
                                 sMsg:="AutoReinstateMTA Failed",
                                 vApp:=ACApp,
                                 vClass:=ACClass,
                                 vMethod:="QuoteReinstatement")
                Return nResult
            End If

            Return nResult
        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="QuoteReinstatement Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteReinstatement", excep:=ex)
            Return nResult
        End Try

    End Function

    '*************************************************************************
    ' Name:         IsBackdatedMTARequired
    ' Description:
    ' History:      13/08/2004 created by Tracy Richards
    '*************************************************************************
    Public Function IsBackdatedMTARequired(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_dtEffectiveDate As Date, ByVal v_lNewInsuranceFileCnt As Integer) As Boolean

        Dim result As Boolean = False
        Try

            'Use local class object
            With m_oAutoMTA
                .NewInsuranceFileCnt = v_lNewInsuranceFileCnt
                .EffectiveDate = v_dtEffectiveDate
                .InsuranceFolderCnt = v_lInsuranceFolderCnt
                .TransactionType = m_sTransactionType
                'call the function
                'developer guide no. 83
                result = .IsBackdatedMTARequired
            End With

            Return result

        Catch excep As System.Exception


            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsBackdatedMTARequired Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsBackdatedMTARequired", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '*************************************************************************
    ' Name:         MultipleVersionsExist
    ' Description:
    ' History:      13/08/2004 created by Tracy Richards
    '*************************************************************************
    Public Function MultipleVersionsExist(Optional ByVal v_lNewInsuranceFileCnt As Integer = 0) As Boolean

        If v_lNewInsuranceFileCnt <> 0 Then
            m_oAutoMTA.NewInsuranceFileCnt = v_lNewInsuranceFileCnt
        End If
        Return m_oAutoMTA.MultipleVersionsExist

    End Function
    'WPR 33-75 added
    Public Function MarkRiskAsUnquoted(ByVal lInsuranceFileCnt As Object, ByVal lRiskCnt As Integer) As Integer
        Try

            m_lReturn = m_oDatabase.SQLAction("UPDATE Insurance_File_Risk_Link SET status_flag='U' " & "WHERE insurance_file_cnt=" & lInsuranceFileCnt & "AND risk_cnt=" & lRiskCnt, "Set Risk Unquoted", False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("MarkRiskAsUnquoted", "SQL Failed", m_lReturn)
            End If

            m_lReturn = m_oDatabase.SQLAction("UPDATE Risk SET total_this_premium=0, total_annual_premium=0 " & "WHERE risk_cnt=" & lRiskCnt, "Set Risk Unquoted", False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("MarkRiskAsUnquoted", "SQL Failed", m_lReturn)
            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MarkRiskAsUnquoted Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MarkRiskAsUnquoted", vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description, excep:=excep)
            Return gPMConstants.PMEReturnCode.PMError

        End Try

    End Function

    '*************************************************************************
    ' Name:         SetNillPremiumRefund
    ' Description:
    '
    ' History:      31/01/2003 sj - Created.
    '               24/11/2003 RAW : CQ685 : added v_vAffectedInsuranceFileCnts
    '               16/08/2004 TR - Copied from iPMUAutoMTA for RESILIENCE
    '*************************************************************************
    Public Function SetNillPremiumRefund(ByVal v_vAffectedInsuranceFileCnts As Object) As Integer

        Dim result As Integer = 0
        Try

            ' RAW 24/11/2003 : CQ685 : added v_vAffectedInsuranceFileCnts argument


            Return m_oAutoMTA.SetNillPremiumRefund(v_vAffectedInsuranceFileCnts:=v_vAffectedInsuranceFileCnts)

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetNillPremiumRefund Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetNillPremiumRefund", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    Public Function GetBackdatedVersions(ByVal m_lifile_cnt As Integer, ByRef m_vBackdatedMTAVersions(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ifile_Cnt", vValue:=CStr(m_lifile_cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetBackdatedMTAVersionsSQL, sSQLName:=ACGetBackdatedMTAVersionsName, bStoredProcedure:=ACGetBackdatedMTAVersionsStored, vResultArray:=m_vBackdatedMTAVersions)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBackdatedVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBackdatedVersions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function


    Public Function DeletePolicyVersions(ByVal m_vBackdatedMTAVersions(,) As Object, Optional ByVal v_lBaseInsurance_File_Cnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            Dim strBInsurance_file_cnts As New StringBuilder

            For i As Integer = 0 To m_vBackdatedMTAVersions.GetUpperBound(1)
                If CDbl(m_vBackdatedMTAVersions(0, i)) <> gPMFunctions.ToSafeLong(v_lBaseInsurance_File_Cnt) Then
                    strBInsurance_file_cnts.Append(CStr(m_vBackdatedMTAVersions(0, i)) & ",")
                End If
            Next

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="strInsuranceFileCnts", vValue:=strBInsurance_file_cnts.ToString(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure to clear all versions barring base version
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteAllPolicyVersionsSQL, sSQLName:=ACDeletePolicyVersionName, bStoredProcedure:=ACDeletePolicyVersionStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Terminate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Terminate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try

    End Function
    ''' <summary>
    ''' QuoteMTA
    ''' </summary>
    ''' <param name="v_lPartyCnt"></param>
    ''' <param name="v_lInsuranceFolderCnt"></param>
    ''' <param name="v_dtEffectiveDate"></param>
    ''' <param name="lBaseInsuranceFileCnt"></param>
    ''' <param name="r_sFailureMessage"></param>
    ''' <param name="vBackdatedMTAVersions"></param>
    ''' <param name="bUpdateStats"></param>
    ''' <param name="r_bShowQuoteMsg"></param>
    ''' <param name="bIsDirty"></param>
    ''' <param name="bIsInteractive"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function QuoteMTA(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_dtEffectiveDate As Date, ByVal lBaseInsuranceFileCnt As Integer, ByRef r_sFailureMessage As String, ByVal vBackdatedMTAVersions As Object, ByVal bUpdateStats As Boolean, Optional ByRef r_bShowQuoteMsg As Boolean = False, Optional ByVal bIsDirty As Boolean = False, Optional ByVal bIsInteractive As Boolean = False) As Integer

        Dim nResult As Integer = 0
        Dim vOverlappedQuotes As Object

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            Dim lPreviousInsuranceFileCnt As Integer

            If Trim(ToSafeString(m_sTransactionType)) = "" Then
                m_sTransactionType = "MTA"
            End If

            If Not bUpdateStats Then
                m_lReturn = CType(ClearBackdateMTAData(lBaseInsuranceFileCnt), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearBackdateMTAData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteMTA")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Use local class object
            With m_oAutoMTA
                .PartyCnt = v_lPartyCnt
                .InsuranceFolderCnt = v_lInsuranceFolderCnt
                .TransactionType = m_sTransactionType
                .EffectiveDate = v_dtEffectiveDate
                .UpdateStats = bUpdateStats
                .MergeRisks = True
                .NewInsuranceFileCnt = lBaseInsuranceFileCnt
                .IsInteractive = bIsInteractive
                .IsPostStatsForLapsedPolicy = (bUpdateStats = True)
                .BackDateMTA = True
                m_lReturn = CType(.AutoBackdatedMTA(r_sErrorText:=r_sFailureMessage, lBaseInsuranceFileCnt:=lBaseInsuranceFileCnt, r_bShowQuoteMsg:=r_bShowQuoteMsg, v_bIsDirty:=bIsDirty), gPMConstants.PMEReturnCode)
            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oAutoMTA.AutoBackdatedMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteMTA")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' always return false as we have interactive mode for user to quote the renewals
            r_bShowQuoteMsg = False

            'Delete Quotes Versions that lie after MTA effective date
            m_lReturn = CType(GetOverlapQuotes(lBaseInsuranceFileCnt, vOverlappedQuotes, v_lInsuranceFolderCnt), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOverlapQuotes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteMTA")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If bUpdateStats And Informations.IsArray(vOverlappedQuotes) Then

                m_lReturn = CType(DeletePolicyVersions(m_vBackdatedMTAVersions:=vOverlappedQuotes, v_lBaseInsurance_File_Cnt:=lBaseInsuranceFileCnt), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeletePolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteMTA")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            If bUpdateStats Then

                m_lReturn = CType(GetPreviousInsuranceFile(v_lNewInsuranceFileCnt:=lBaseInsuranceFileCnt, r_lPreviousInsuranceFileCnt:=lPreviousInsuranceFileCnt), gPMConstants.PMEReturnCode)

                If gPMFunctions.ToSafeLong(lPreviousInsuranceFileCnt) > 0 Then

                    m_oDatabase.Parameters.Clear()

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="old_insurance_file_cnt", vValue:=CStr(lPreviousInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) ' Sankar - Changed PMInteger to PMLong - 02-01-2009

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="new_insurance_file_cnt", vValue:=CStr(lBaseInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) ' Sankar - Changed PMInteger to PMLong - 02-01-2009

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACMoveSuspendedAgentCommissionSQL, sSQLName:=ACMoveSuspendedAgentCommissionName, bStoredProcedure:=ACMoveSuspendedAgentCommissionStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to move the Suspended Agent Commission", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteMTA", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If


            Return nResult

        Catch excep As System.Exception


            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="QuoteMTA Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QuoteMTA", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    Public Function GetOverlapQuotes(ByVal lBaseInsuranceFileCnt As Integer, ByRef vOverlappedQuotes(,) As Object, ByVal lInsuranceFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="IfileCnt", vValue:=CStr(lBaseInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="IfolderCnt", vValue:=CStr(lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetOverlappedQuotesSQL, sSQLName:=ACGetOverlappedQuotesName, bStoredProcedure:=False, vResultArray:=vOverlappedQuotes)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOverlapQuotes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOverlapQuotes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result


        End Try
    End Function

    Public Function ClearBackdateMTAData(ByVal lBaseInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            Dim vPolicyVersions As Object

            result = gPMConstants.PMEReturnCode.PMTrue

            'WPR 33-75 added
            m_lReturn = CType(GetBackdatedVersions(m_lifile_cnt:=lBaseInsuranceFileCnt, m_vBackdatedMTAVersions:=vPolicyVersions), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearBackdateMTAData")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Delete policy versions generated by QuoteMTA
            If Informations.IsArray(vPolicyVersions) Then

                'WPR 33-75 added
                m_lReturn = CType(DeletePolicyVersions(m_vBackdatedMTAVersions:=vPolicyVersions, v_lBaseInsurance_File_Cnt:=lBaseInsuranceFileCnt), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeletePolicyVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearBackdateMTAData")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            'Delete Replaced event_logs
            m_lReturn = CType(DeleteEventLog(lBaseInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteEventLog Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearBackdateMTAData")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Update Replaced Insurance_file_Status Back to Normal
            m_lReturn = CType(UpdatePolicyStatus(lBaseInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePolicyStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearBackdateMTAData")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Also delete from MTA_insurance_file_link table
            m_lReturn = CType(DeleteMTALinks(lBaseInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePolicyStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearBackdateMTAData")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearBackdateMTAData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearBackdateMTAData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    Private Function DeleteEventLog(ByVal m_lIFileCnt As Integer) As Integer
        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="IfileCnt", vValue:=CStr(m_lIFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute the stored procedure
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteEventLogSQL, sSQLName:=ACDeleteEventLogName, bStoredProcedure:=False)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPreviousInsuranceFile
    '
    ' Description: Returns the previous live Insurance File based on the
    '              new one.
    '
    ' ***************************************************************** '
    Public Function GetPreviousInsuranceFile(ByVal v_lNewInsuranceFileCnt As Object, ByRef r_lPreviousInsuranceFileCnt As Object) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Send the new file in
            m_oDatabase.Parameters.Clear()

            m_oDatabase.Parameters.Add("new_insurance_file_cnt", CStr(v_lNewInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Execute the SP
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPreviousFileSQL, sSQLName:=ACGetPreviousFileName, bStoredProcedure:=ACGetPreviousFileStored, vResultArray:=vResultArray)

            'Determine the result
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
            ElseIf Informations.IsArray(vResultArray) Then


                r_lPreviousInsuranceFileCnt = vResultArray(0, 0)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPreviousInsuranceFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPreviousInsuranceFile", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'WPR 33-75 added
    Public Function GetPreviousRiskCnt(ByVal v_lPreInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByRef r_lPreviousRiskCnt As Integer) As Integer

        Dim vResultArray(,) As Object

        Try

            GetPreviousRiskCnt = gPMConstants.PMEReturnCode.PMTrue

            'Send the new file in
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_file_cnt", vValue:=v_lPreInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_cnt", vValue:=v_lRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            'Execute the SP
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPreviousRiskCntForBackDatedMTASQL, sSQLName:=ACGetPreviousRiskCntForBackDatedMTAName, bStoredProcedure:=True, vResultArray:=vResultArray)

            'Determine the result
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GetPreviousRiskCnt = gPMConstants.PMEReturnCode.PMFalse
            ElseIf Informations.IsArray(vResultArray) Then
                r_lPreviousRiskCnt = ToSafeLong(vResultArray(0, 0))
            End If

            Return m_lReturn

        Catch excep As System.Exception

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, vClass:=ACClass, sMsg:="GetPreviousRiskCnt Failed", vMethod:="GetPreviousRiskCnt", excep:=excep)
            Return gPMConstants.PMEReturnCode.PMError

        End Try

    End Function
    'WPR 33-75 added
    Public Function GetPolicyRisks(ByVal m_lIFileCnt As Integer, ByRef vRisks(,) As Object) As Integer

        Try

            GetPolicyRisks = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=m_lIFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyRisksSQL, sSQLName:=ACGetPolicyRisksName, bStoredProcedure:=ACGetPolicyRisksStored, vResultArray:=vRisks)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, vClass:=ACClass, vMethod:="GetPolicyRisks", sMsg:="GetPolicyRisks Failed", excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try

    End Function
    ''' <summary>
    ''' AddQuotes
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="nPreChangeInsFileCnt"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function AddQuotes(ByVal nInsuranceFileCnt As Integer, ByVal nPreChangeInsFileCnt As Integer) As Integer

        Dim oArray(,) As Object
        Dim nOriginalInsuranceFileCnt As Integer
        Dim nCancelledInsFileCnt As Integer
        Dim dtMTAStartDate As Date
        Dim sTransactionType As String
        Dim nReturn As Integer

        Try

            nReturn = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ninsurance_file_cnt",
                                       vValue:=nInsuranceFileCnt,
                                       iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                       iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetFuturePolicyVersionsSQL,
                                              sSQLName:=ACGetFuturePolicyVersionsName,
                                              bStoredProcedure:=ACGetFuturePolicyVersionsStored,
                                              vResultArray:=oArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nReturn = gPMConstants.PMEReturnCode.PMFalse
                Return nReturn
            ElseIf Informations.IsArray(oArray) = False Then
                nReturn = gPMConstants.PMEReturnCode.PMNotFound
                Return nReturn
            End If

            nOriginalInsuranceFileCnt = ToSafeLong(oArray(ACOriginalInsFileCnt, 0))
            nCancelledInsFileCnt = ToSafeLong(oArray(ACCancelledInsFileCnt, 0))
            dtMTAStartDate = ToSafeDate(oArray(ACCoverStartDate, 0))
            m_oAutoMTA.InsuranceFolderCnt = ToSafeLong(oArray(6, 0))
            m_oAutoMTA.NewInsuranceFileCnt = ToSafeLong(oArray(ACBaseInsFileCnt, 0))
            If ToSafeBoolean(oArray(ACRiskProcessed, False)) = False Then
                m_oAutoMTA.BackDateMTA = True
                m_oAutoMTA.IsInteractive = True
                m_oAutoMTA.MergeRisks = True

                ' Process reversal version
                m_lReturn = m_oAutoMTA.ProcessRisksForMultiThreading(
                                nInsuranceFileCnt:=nCancelledInsFileCnt,
                                sTransactionType:="MTCR",
                                nBaseInsuranceFileCnt:=nOriginalInsuranceFileCnt,
                                bIsBackdatedMTA:=True)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    nReturn = gPMConstants.PMEReturnCode.PMFalse
                    Return nReturn
                End If

                oArray = Nothing
                sTransactionType = "MTA"
                m_lReturn = m_oDatabase.SQLSelect("Select ISNULL(insurance_file_type_id, 0) From insurance_file Where insurance_file_cnt = " & nInsuranceFileCnt, "Get Type", False, , oArray)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    nReturn = gPMConstants.PMEReturnCode.PMFalse
                    Return nReturn
                Else
                    If Informations.IsArray(oArray) Then
                        If oArray(0, 0) = 11 Then
                            sTransactionType = "MTC"
                        ElseIf oArray(0, 0) = 10 Then
                            sTransactionType = "MTR"
                        End If
                    End If
                End If

                ' Process re-applied version
                m_lReturn = m_oAutoMTA.ProcessRisksWithMerge(
                            v_lInsuranceFileCnt:=nInsuranceFileCnt,
                            v_sTransactionType:="MTA",
                            v_dtMTAStartDate:=dtMTAStartDate,
                            v_lPreChangeInsFileCnt:=nPreChangeInsFileCnt,
                            v_lPostChangeInsFileCnt:=nOriginalInsuranceFileCnt,
                            v_bApplyRiskChange:=False,
                            v_bIsBackdatedMTA:=True)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    nReturn = gPMConstants.PMEReturnCode.PMFalse
                    Return nReturn
                End If

                m_lReturn = m_oDatabase.SQLAction("Update insurance_file Set risk_processed = 1 Where insurance_file_cnt = " & nInsuranceFileCnt, "Update processed", False)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    nReturn = gPMConstants.PMEReturnCode.PMFalse
                    Return nReturn
                End If

                If sTransactionType = "MTR" Then
                    m_lReturn = m_oDatabase.SQLAction("Update insurance_file_system Set last_trans_type_id = 20 Where insurance_file_cnt = " & nInsuranceFileCnt, "Update InsuranceFileSystem", False)
                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        nReturn = gPMConstants.PMEReturnCode.PMFalse
                        Return nReturn
                    End If
                End If
            End If

            Return nReturn

        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="AddQuotes", r_lFunctionReturn:=AddQuotes, excep:=ex)
        End Try
    End Function

    ''' <summary>
    ''' Copy Risk Links
    ''' </summary>
    ''' <param name="nOldInsuranceFileCnt"></param>
    ''' <param name="nNewInsuranceFileCnt"></param>
    ''' <param name="bCopyDeletedRisk"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Private Function CopyRiskLinks(ByVal nOldInsuranceFileCnt As Integer,
                                   ByVal nNewInsuranceFileCnt As Integer,
                                   ByVal bCopyDeletedRisk As Boolean) As Integer
        Dim nReturn As Integer

        nReturn = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt",
                                               vValue:=nOldInsuranceFileCnt,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMLong)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            nReturn = gPMConstants.PMEReturnCode.PMFalse
            Return nReturn
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt",
                                               vValue:=nNewInsuranceFileCnt,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMLong)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            nReturn = gPMConstants.PMEReturnCode.PMFalse
            Return nReturn
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="CopyDeletedRisks",
                                               vValue:=bCopyDeletedRisk,
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                               iDataType:=gPMConstants.PMEDataType.PMBoolean)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            nReturn = gPMConstants.PMEReturnCode.PMFalse
            Return nReturn
        End If


        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyRiskLinksSQL,
                                          sSQLName:=ACCopyRiskLinksName,
                                          bStoredProcedure:=ACCopyRiskLinksStored)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            nReturn = gPMConstants.PMEReturnCode.PMFalse
            Return nReturn
        End If

        Return nReturn

    End Function
    ''' <summary>
    ''' GetSavedOOSQuotes
    ''' </summary>
    ''' <param name="v_nInsuranceFolderCnt"></param>
    ''' <param name="r_oResults"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSavedOOSQuotes(ByVal v_nInsuranceFolderCnt As Integer, ByVal v_nInsuranceFileCnt As Integer, ByRef r_oResults(,) As Object) As Integer

        Dim nReturn As Integer
        Try

            nReturn = PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            nReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt",
                                  vValue:=v_nInsuranceFileCnt,
                                  iDirection:=PMEParameterDirection.PMParamInput,
                                  iDataType:=PMEDataType.PMLong)
            If nReturn <> PMEReturnCode.PMTrue Then
                nReturn = PMEReturnCode.PMFalse
                Return nReturn
            End If

            nReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt",
                                   vValue:=v_nInsuranceFolderCnt,
                                   iDirection:=PMEParameterDirection.PMParamInput,
                                   iDataType:=PMEDataType.PMLong)
            If nReturn <> PMEReturnCode.PMTrue Then
                nReturn = PMEReturnCode.PMFalse
                Return nReturn
            End If

            ' Execute the stored procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_get_saved_oos_versions",
                                              sSQLName:="spu_get_saved_oos_versions",
                                              bStoredProcedure:=True,
                                              vResultArray:=r_oResults)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                nReturn = PMEReturnCode.PMFalse
                Return nReturn
            End If
            Return nReturn
        Catch ex As Exception
            nReturn = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername,
                iType:=gPMConstants.PMELogLevel.PMLogOnError,
                sMsg:="GetSavedOOSQuotes Failed",
                vApp:=ACApp,
                vClass:=ACClass,
                vMethod:="GetSavedOOSQuotes",
                vErrNo:=Informations.Err.Number,
                vErrDesc:=Informations.Err.Description,
                excep:=ex)

            Return nReturn
        End Try

    End Function

End Class