Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Text
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 20/07/2000
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRProduct.
    '
    ' Edit History:
    ' SJP14062002 - getUnderwritingType uses new product options scheme
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 22/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    Private m_lError As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Calling Application Name

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

    Private lPMAuthorityLevel As Integer

    'JMK 22/10/2001 - Underwriting hidden option
    Private m_sUnderwritingType As String = ""

    ' Primary Keys to work with
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            Value = Value

        End Set
    End Property

    ' JMK 22/10/2001 "A" for Underwriting Agency and "U" for Reinsurance
    Public ReadOnly Property UnderwritingType() As String
        Get

            If m_sUnderwritingType = "" Then
                m_lReturn = getUnderwritingType()
            End If

            Return m_sUnderwritingType

        End Get
    End Property

    ' ***************************************************************** '
    ' Name: GetUnderwritingType
    '
    ' Description:  Finds out Underwriting type - U or A
    '               For labelling: A - Insurer. U - Reinsurer
    '
    ' JMK 22/10/2001    Created
    ' SJP14062002 - getUnderwritingType uses new product options scheme
    ' ***************************************************************** '
    Private Function getUnderwritingType() As Integer

        Dim result As Integer = 0



        Return bPMFunc.getUnderwritingType(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingType)

    End Function

    ''' <summary>
        ''' Initialise (Standard Method)
        ''' Entry point for any initialisation code for this object.
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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, _
                               ByVal iUserID As Integer, ByVal iSourceID As Integer, _
                               ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, _
                               ByVal iLogLevel As Integer, _
                               ByVal sCallingAppName As String, _
                               Optional ByVal bStandAlone As Boolean = False, _
                               Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


        Dim nResult As Integer = 0
        Try

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




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, _
                                                                 v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, _
                                                                 r_oCheckedDatabase:=m_oDatabase, _
                                                                 v_vDatabase:=vDatabase),  _
                                                                 gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function
    ''' <summary>
    ''' Terminate (Standard Method)
    ''' Entry point for any termination code for this   object.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

   Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

    ''' <summary>
    ''' SetProcessModes (Standard Method)
    ''' Set the optional process modes.
    ''' </summary>
    ''' <param name="vTask"></param>
    ''' <param name="vNavigate"></param>
    ''' <param name="vProcessMode"></param>
    ''' <param name="vTransactionType"></param>
    ''' <param name="vEffectiveDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, _
                                    Optional ByRef vNavigate As Object = Nothing, _
                                    Optional ByRef vProcessMode As Object = Nothing, _
                                    Optional ByRef vTransactionType As Object = Nothing, _
                                    Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

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

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function
    ''' <summary>
    ''' BeginTrans (Private)
    ''' Begins a Transaction
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function BeginTrans() As Integer

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ''' <summary>
    ''' CommitTrans (Private)
    ''' Commits a Transaction (Saves changes to DB).
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CommitTrans() As Integer

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function
    ''' <summary>
    ''' RollbackTrans (Private)
    ''' Rollback a Transaction (Undo changes to DB).
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function RollbackTrans() As Integer

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function
    Public Sub New()
        MyBase.New()
    End Sub
    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    ''' <summary>
    ''' Gets all the User Groups
    ''' </summary>
    ''' <param name="r_lUserId"></param>
    ''' <param name="r_vUserGroupInfo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetExternalWorkflowConfigurationUserGroupInfo(ByRef r_vUserGroupInfo(,) As Object) As Integer
        Dim nResult As Integer = 0

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="dtEffective_date", vValue:=Date.Today, _
                                          iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                          iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=KSelExternalWorkFlowConfiguration_UsergroupsSQL, _
                                              sSQLName:=KSelExternalWorkFlowConfiguration_UsergroupName, _
                                              bStoredProcedure:=KSelExternalWorkFlowConfiguration_UsergroupStored, _
                                              vResultArray:=r_vUserGroupInfo, _
                                              bKeepNulls:=True)

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetExternalWorkflowConfigurationInfo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExternalWorkflowConfigurationInfo ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult

        End Try
    End Function
    ''' <summary>
    ''' UpdateAllUserGroups
    ''' Update User Group Info
    ''' </summary>
    ''' <param name="r_lUserId"></param>
    ''' <param name="r_lPMUserGroupId"></param>
    ''' <param name="r_iMode"></param>
    ''' <param name="r_iIsSupervisor"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateExternalWorkflowConfigurationUserGroupInfo( _
                                        ByRef r_lPMUserGroupId As Integer, _
                                        ByRef r_iMode As Integer, _
                                        Optional ByRef r_iIsSupervisor As Integer = 0) As Integer

        Dim nResult As Integer = 0

        Try

            ' Get the Groups
            If r_iMode = 0 Then
                'Deleting the Group  from the User Group table
                nResult = DelUpdateExternalWorkflowConfigurationUserGroupInfo(r_lPMUserGroupId)
            Else
                'Deleting the Group  from the User Group table
                nResult = AddUpdateExternalWorkflowConfigurationUserGroupInfo(r_lPMUserGroupId)

            End If

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            nResult = gPMConstants.PMEReturnCode.PMTrue
            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateExternalWorkflowConfigurationUserGroupInfo", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateExternalWorkflowConfigurationUserGroupInfo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ''' <summary>
    ''' This Method is used to delete the User Group.
    ''' </summary>
    ''' <param name="v_lUserGroupID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DelUpdateExternalWorkflowConfigurationUserGroupInfo(ByVal v_lUserGroupID As Integer) As Integer

        Dim nResult As Integer = 0
        Dim sSQL As String = ""

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="nPMuser_group_id", vValue:=v_lUserGroupID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                          iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=KDelExternalWorkFlowConfiguration_UsergroupSQL, _
                                              sSQLName:=KDeExternalWorkFlowConfiguration_UsergroupName, _
                                              bStoredProcedure:=KDelExternalWorkFlowConfiguration_UsergroupStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
            End If

            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DelUpdateExternalWorkflowConfigurationUserGroupInfo  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DelUpdateExternalWorkflowConfigurationUserGroupInfo ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' This Method is used to Add the User Group.
    ''' </summary>
    ''' <param name="o_lUserGroupID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddUpdateExternalWorkflowConfigurationUserGroupInfo(ByVal o_lUserGroupID As Integer) As Integer

        Dim nResult As Integer = 0
        Dim sSQL As String = ""

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="npmuser_group_id", vValue:=o_lUserGroupID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                          iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=KAddExternalWorkFlowConfiguration_UsergroupSQL, _
                                              sSQLName:=KAddExternalWorkFlowConfiguration_UsergroupName, _
                                              bStoredProcedure:=KAddExternalWorkFlowConfiguration_UsergroupsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
            End If

            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddUpdateExternalWorkflowConfigurationUserGroupInfo  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddUpdateExternalWorkflowConfigurationUserGroupInfo ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function
    ''' <summary>
    ''' This Method is used to Add the User Group.
    ''' </summary>
    ''' <param name="o_bEnablebackgroundjob_ForFailure"></param>
    ''' <param name="o_lExternal_WorkFlow_Config_ID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateExternalWorkflowConfigFlag(ByVal o_bEnablebackgroundjob_ForFailure As Boolean, _
                                                     ByVal o_lExternalWorkFlowConfigID As Integer _
                                                     ) As Integer

        Dim nResult As Integer = 0
        Dim sSQL As String = ""
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="nExternal_WorkFlow_Config_ID", vValue:=o_lExternalWorkFlowConfigID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                          iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.Parameters.Add(sName:="bEnablebackgroundjob_ForFailure", vValue:=IIf(o_bEnablebackgroundjob_ForFailure = True, 1, 0), _
                                          iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                          iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=KAddspu_SIR_ExternalWorkFlowConfigSQL, _
                                             sSQLName:=KAddspu_SIR_ExternalWorkFlowConfigName, _
                                             bStoredProcedure:=KAddspu_SIR_ExternalWorkFlowConfigStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
            End If

            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddUpdateExternalWorkflowConfigurationUserGroupInfo  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddUpdateExternalWorkflowConfigurationUserGroupInfo ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

End Class

