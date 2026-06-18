Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms

Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed

    Implements IDisposable
    ''' <summary>
    ''' Class Name: Interface
    ''' Main public class to accompany the interface form.
    ''' Date:10/07/2014
    ''' </summary>
    ''' <remarks></remarks>
    Public m_oFrmExternalWorkFlowConfiguration As FrmExternalWorkFlowConfiguration

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"
#Region "Private Variables"
    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lPMAuthorityLevel As Integer

    ' SourceArray
    Private m_vSourceArray As Object

    ' RDC 01102002
    Private m_sSysOption As String = ""

    Private m_bUserMaintenanceLocked As Boolean
    Private Const ACUserMaintenanceLockName As String = "User Maintenance"
    Private Const ACUserMaintenanceLockId As Integer = 1
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' User ID
    Private m_iUserID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Source ID
    Private m_iSourceID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    Private m_iHomeCountryID As Integer
    Private m_iLicenceLimit As Integer
    Private m_iPoolSize As Integer
#End Region
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get

            ' Standard Property.

            ' Return the task.
            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            ' Standard Property.

            ' Return the navigate flag.
            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            ' Standard Property.

            ' Return the process mode.
            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            ' Standard Property.

            ' Return the effective date.
            Return m_dtEffectiveDate

        End Get
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property
    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            m_lPMAuthorityLevel = Value

        End Set
    End Property
    ''' <summary>
    ''' this is an Standard method to initialize the Inteferface
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Initialise() As Integer

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                nResult = gPMConstants.PMEReturnCode.PMFalse
                ' Set the object manager to nothing.
                g_oObjectManager = Nothing
                Return nResult
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bSIRExternalWorkflowConfiguration.Business", vInstanceManager:="ClientManager")
            g_oBusiness = temp_g_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Failed to get an instance of the business object.
                nResult = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("The was a problem creating bSIRExternalWorkflowConfiguration.Business", "bSIRExternalWorkflowConfiguration.Interface", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return nResult
            End If

            'm_lReturn = g_oBusiness.GetAllSources(m_vSourceArray)
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    Return m_lReturn
            'End If

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    Return m_lReturn
            'End If

            Return nResult

        Catch excep As System.Exception


            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", _
                                         vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return nResult

        End Try
    End Function

    ''' <summary>
    ''' This is an standard Method to terminate the intance.
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
            Me.disposedValue = True
            If disposing Then
                If g_oBusiness IsNot Nothing Then
                    g_oBusiness.Dispose()
                    g_oBusiness = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub

    ''' <summary>
    ''' this method is used to setting the keys
    ''' </summary>
    ''' <param name="vKeyArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim nResult As Integer = 0

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()

                End Select

            Next lRow

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' Stores all of the key array with the parameter
    ''' members.
    ''' </summary>
    ''' <param name="vKeyArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim nResult As Integer = 0
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            ReDim vKeyArray(1, 0)

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ''' <summary>
    ''' (Its An Standard Method)
    ''' Stores all of the summary array with the parameter members.
    ''' </summary>
    ''' <param name="vSummaryArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSummary(ByRef vSummaryArray(,) As Object) As Integer

        Dim nResult As Integer = 0

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ReDim vSummaryArray(1, 0)
            ' Assign the key array with the parameter members.

            vSummaryArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "Dummy Key"

            vSummaryArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = "Dummy Value"

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

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

            ' Set the process modes for the business object.
            If Not (g_oBusiness Is Nothing) Then

                m_lReturn = g_oBusiness.SetProcessModes(vTask:=vTask, vNavigate:=vNavigate, vProcessMode:=vProcessMode, _
                                                        vTransactionType:=vTransactionType, vEffectiveDate:=vEffectiveDate)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, _
                                       sMsg:="Failed to set the process modes for the business object", _
                                       vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes")

                    nResult = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return nResult

        Catch excep As System.Exception
            ' Error Section.

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function
    ''' <summary>
    ''' Start (Standard Method)
    '''  Entry point for the object to start its processing.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Start() As Integer

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Starts the interface processing.
            m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function
    ''' <summary>
    ''' ProcessInterface (Standard Method)
    ''' Calls the appropriate methods to process the interface.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ProcessInterface() As Integer

        Dim nResult As Integer = 0
       
            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Load the interface into memory.
            m_lReturn = CType(LoadInterface(), gPMConstants.PMEReturnCode)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to load the interface.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Display the interface.
            m_lReturn = CType(ShowInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to display the inteface.
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Destroy the interface from memory.
            m_lReturn = CType(UnLoadInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to unload the interface.
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

    End Function
    ''' <summary>
    ''' LoadInterface (Standard Method)
    ''' Loads the instance of the interface into memory and   passes the parameters in
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function LoadInterface() As Integer

        Dim nResult As Integer = 0


            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_oFrmExternalWorkFlowConfiguration = New FrmExternalWorkFlowConfiguration()

            With m_oFrmExternalWorkFlowConfiguration
                .CallingAppName = m_sCallingAppName
                '.Navigate = m_lNavigate
                .ProcessMode = m_lProcessMode
                .TransactionType = m_sTransactionType
                .EffectiveDate = m_dtEffectiveDate
                .SourceArray = m_vSourceArray
            End With

            ' Load the instance of the interface into memory.

            Dim tempLoadForm As FrmExternalWorkFlowConfiguration = m_oFrmExternalWorkFlowConfiguration

            ' Check if we have had an error so far.

            'If frmUserMaintenance.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            If m_oFrmExternalWorkFlowConfiguration.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST return the error.

                nResult = m_oFrmExternalWorkFlowConfiguration.ErrorNumber
            End If


        Return nResult

    End Function
    ''' <summary>
    ''' UnLoadInterface (Standard Method)
    ''' Unloads the instance of the interface from memory.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UnLoadInterface() As Integer

        Dim nResult As Integer = 0


        nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the property members from the interface parameters.

            With m_oFrmExternalWorkFlowConfiguration
                m_lStatus = .Status

                ' {* USER DEFINED CODE (Begin) *}
                ' {* USER DEFINED CODE (End) *}
            End With

            ' Unload and destroy the instance of the interface
            ' from memory.

            m_oFrmExternalWorkFlowConfiguration.Close()

            m_oFrmExternalWorkFlowConfiguration = Nothing

        Return nResult

    End Function

    ''' <summary>
    ''' it display the  interface ShowInterface
    ''' </summary>
    ''' <param name="lDisplayState"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
   Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer

        Dim nResult As Integer = 0


        nResult = gPMConstants.PMEReturnCode.PMTrue

        ' Display the interface.

        VB6.ShowForm(m_oFrmExternalWorkFlowConfiguration, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.

            If m_oFrmExternalWorkFlowConfiguration.ErrorNumber <> 0 Then

                nResult = m_oFrmExternalWorkFlowConfiguration.ErrorNumber
            End If
        End If
        Return nResult

    End Function
    'PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    ' ***************************************************************** '
    ' Name: LockUserMaintenance
    '
    ' Description: Lock the User Maintenance function (only one user is
    '              allowed at a time).
    '
    ' History: CJB 090205 Created as part of PN18636
    ' ***************************************************************** '
    '    Private Function LockUserMaintenance() As Integer
    '        Dim result As Integer = 0
    '        Dim bPMLock As Object


    '        Dim oLock As bPMLock.User
    '        Dim sLockedBy As String = ""

    '        On Error GoTo Catch_Renamed


    '        result = gPMConstants.PMEReturnCode.PMTrue

    '        ' Get instance of bPMLock.User
    '        Dim temp_oLock As Object
    '        m_lReturn = g_oObjectManager.GetInstance(temp_oLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
    '        oLock = temp_oLock

    '        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '            gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Failed to get instance of bPMLock.User", gPMConstants.PMELogLevel.PMLogError)
    '        End If

    '        ' Lock this function (just pass '1' as the id)

    '        m_lReturn = oLock.LockKey(sKeyName:=ACUserMaintenanceLockName, vKeyValue:=ACUserMaintenanceLockId, iUserID:=g_oObjectManager.UserID, sCurrentlyLockedBy:=sLockedBy)

    '        If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then

    '            If sLockedBy = "ERROR" Then
    '                gPMFunctions.RaiseError("oLock.LockKey", "Error trying to lock 'User Maintenance' record", gPMConstants.PMELogLevel.PMLogError)
    '            Else
    '                result = gPMConstants.PMEReturnCode.PMFalse
    '                MessageBox.Show("User Maintenance is currently locked by " & sLockedBy & "." & _
    '                                Strings.Chr(13) & Strings.Chr(10) & "Please try later.", "User Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '                GoTo Finally_Renamed
    '            End If

    '        End If

    '        ' Flag that we have successfully locked the function (so that we know to unlock later)
    '        m_bUserMaintenanceLocked = True

    '        GoTo Finally_Renamed

    'Catch_Renamed:
    '        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockUserMaintenance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockUserMaintenance", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '        result = gPMConstants.PMEReturnCode.PMFalse
    'Finally_Renamed:
    '        Return result
    '        Resume
    '        Return result
    '    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UnlockUserMaintenance() As Integer
        Dim result As Integer = 0
        Dim bPMLock As Object


        Dim oLock As bPMLock.User

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get instance of bPMLock.User
        Dim temp_oLock As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oLock = temp_oLock

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Failed to get instance of bPMLock.User", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Unlock this function (if we successfully locked it in the first place!)
        If m_bUserMaintenanceLocked Then

            m_lReturn = oLock.UnLockKey(sKeyName:=ACUserMaintenanceLockName, vKeyValue:=ACUserMaintenanceLockId, iUserID:=g_oObjectManager.UserID)

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                gPMFunctions.RaiseError("oLock.UnLockKey", "Error trying to unlock 'User Maintenance'", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_bUserMaintenanceLocked = False
        End If
        Return result
    End Function
End Class