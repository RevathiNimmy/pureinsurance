Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("NavigateControl_NET.NavigateControl")> _
Public NotInheritable Class NavigateControl
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: NavigateControl
    '
    ' Date: 03/09/1998
    '
    ' Description: This Class provides the control for Navigator.
    '              It drives the control of the Navigator process
    '              both from the business and interface point of view.
    '
    ' Edit History:
    ' RFC111298 - Navigable Process Type Added.
    ' RFC160299 - Data Capture Process Type Added.
    ' DAK240899 - Amendment to form refresh in UpdateStep Function
    ' DAK250100 - Allow to continue if error in step when AllowErrorRetry
    '             is set.
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "NavigateControl"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    Private m_vSetKeyArray As Object
    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lProcessID As Integer
    ' Are we in Navigator Driven Mode
    Private m_bDrivenMode As Boolean
    ' Has the Process Finished
    Private m_bFinished As Boolean
    ' Is the Process Complete
    Private m_bProcessComplete As Boolean
    ' Are we in Development Mode
    Private m_bDevelopmentMode As Boolean

    ' RFC160299 - RestartEnabled is NOT Used By Work Manager.
    '' Whether Restart should be enabled or not (Assigned by Work Manager)
    'Private m_bRestartEnabled As Boolean

    ' The Users Authority Level for this Process
    ' Note: This is passed to NavV3 compliant components only.
    Private m_lPMAuthorityLevel As gPMConstants.PMEAuthorityLevel
    ' Whether Navigator should Start Maximized or Normal
    Private m_bStartMaximized As Boolean

    ' Used to keep the Transaction Type and
    ' Process Mode if the first Process that is started.
    ' These values are always used no matter how many
    ' other sub processes are started.
    Private m_sTransactionType As String = ""
    Private m_lProcessMode As Integer

    Private m_oProcess As iPMNavigator.Process
    Private WithEvents m_fNavForm As frmInterface

    'DAK250100
    ' AllowErrorRetry
    Private m_lAllowErrorRetry As gPMConstants.PMEReturnCode

    'For PN-43657
    Private m_vPolicySharesArray As Object

    ' Return Code
    Private m_lReturn As gPMConstants.PMEReturnCode
    ' PRIVATE Data Members (End)

    ' PUBLIC Events (Begin)
    ' RFC160299 - Navigate, JumpToStep & Restart Events Removed
    '             as they were NOT used by WorkManager.
    Public Event SetProcessStatus(ByVal v_bProcessComplete As Boolean)
    Public Event NavigatorClose()
    ' PUBLIC Events (End)

    ' PRIVATE Property Procedures (Begin)

    'For PN-43657
    Public Property PolicySharesArray() As Object
        Get
            Return m_vPolicySharesArray
        End Get
        Set(ByVal Value As Object)


            m_vPolicySharesArray = Value
        End Set
    End Property


    'DAK310700 - Get changed to public so that it can be read
    '            in the form.
    Public Property DrivenMode() As Boolean
        Get
            Return m_bDrivenMode
        End Get
        Set(ByVal Value As Boolean)
            m_bDrivenMode = Value
        End Set
    End Property

    Private Property Finished() As Boolean
        Get
            Return m_bFinished
        End Get
        Set(ByVal Value As Boolean)
            m_bFinished = Value
        End Set
    End Property

    Private Property ProcessComplete() As Boolean
        Get
            Return m_bProcessComplete
        End Get
        Set(ByVal Value As Boolean)
            m_bProcessComplete = Value
        End Set
    End Property

    Private Property DevelopmentMode() As Boolean
        Get
            Return m_bDevelopmentMode
        End Get
        Set(ByVal Value As Boolean)
            m_bDevelopmentMode = Value
        End Set
    End Property

    Private Property Process() As iPMNavigator.Process
        Get
            ' Return the Process property
            Return m_oProcess
        End Get
        Set(ByVal Value As iPMNavigator.Process)

            ' Set the Process Property
            m_oProcess = Value

        End Set
    End Property

    Private Property TransactionType() As String
        Get
            Return m_sTransactionType.Trim()
        End Get
        Set(ByVal Value As String)
            m_sTransactionType = Value.Trim()
        End Set
    End Property

    Private Property ProcessMode() As Integer
        Get
            Return m_lProcessMode
        End Get
        Set(ByVal Value As Integer)
            m_lProcessMode = Value
        End Set
    End Property
    ' PRIVATE Property Procedures (End)
    ' PUBLIC Property Procedures (Begin)
    Public Property CallingAppName() As String
        Get
            ' Return the callinf application name.
            Return m_sCallingAppName
        End Get
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property

    Public Property ProcessID() As Integer
        Get
            ' Return the process ID.
            Return m_lProcessID
        End Get
        Set(ByVal Value As Integer)
            ' Set the process ID.
            m_lProcessID = Value
        End Set
    End Property

    ' RFC160299 - RestartEnabled is NOT Used By Work Manager.
    '' Whether Restart should be enabled or Not (Assigned by Work Manager)
    'Public Property Let RestartEnabled(bRestartEnabled As Boolean)
    '    m_bRestartEnabled = bRestartEnabled
    '    If (m_fNavForm Is Nothing = False) Then
    '        m_fNavForm.cmdContinue.Enabled = m_bRestartEnabled
    '    End If
    'End Property
    'Public Property Get RestartEnabled() As Boolean
    '    RestartEnabled = m_bRestartEnabled
    'End Property

    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            ' Make sure that the Authority level supplied is Valid
            Select Case Value
                Case gPMConstants.PMEAuthorityLevel.pmeALUser, gPMConstants.PMEAuthorityLevel.pmeALSupervisor, gPMConstants.PMEAuthorityLevel.pmeALSysAdmin
                    m_lPMAuthorityLevel = Value
                Case Else
                    m_lPMAuthorityLevel = gPMConstants.PMEAuthorityLevel.pmeALUser
            End Select
        End Set
    End Property

    Private Property StartMaximized() As Boolean
        Get
            Return m_bStartMaximized
        End Get
        Set(ByVal Value As Boolean)
            m_bStartMaximized = Value
        End Set
    End Property

    'DAK250100
    Public Property AllowErrorRetry() As Integer
        Get
            Return m_lAllowErrorRetry
        End Get
        Set(ByVal Value As Integer)
            m_lAllowErrorRetry = Value
        End Set
    End Property

    ' PUBLIC Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: SetKeys
    '
    ' Description: Accepts an Array in the format KeyName, KeyValue.
    '              The array will contain the key values required by
    '              Navigator to start the required Process.
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a valid array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            m_vSetKeyArray = vKeyArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' We want control to return to the calling app (WorkManager)
            ' immediately. Therefore a Timer is used to start the Process.
            ' The Timer will raise the Form StartProcess Event which will be
            ' handled by this Class.

            ' Start The Timer
            m_fNavForm.tmrStartProcess.Enabled = True

            ' Return control to the Calling App.
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Are we in development mode.
#If (ccdevelopmentmode = 1) Then

			DevelopmentMode = True
#Else
            DevelopmentMode = False
#End If

            ' Initialisation Code.
            Process = Nothing

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the component.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            ' Create & load the Navigator Form
            m_fNavForm = New frmInterface()

            'Load(m_fNavForm)
            ' Set the Forms Parent
            m_fNavForm.Parent_Renamed = Me

            Process = New iPMNavigator.Process()

            Finished = False
            ProcessComplete = False

            'DAK250100
            m_lReturn = CType(GetRegistrySettings(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            Me.disposedValue = True
            If disposing Then

                Process = Nothing
                If Not (m_fNavForm Is Nothing) Then
                    m_fNavForm.Parent_Renamed = Nothing
                    m_fNavForm.Close()
                    m_fNavForm = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: MinimiseNavigator
    '
    ' Description: Minimide the Navigator Form. Called by Work Manager.
    '
    ' ***************************************************************** '
    Public Sub MinimiseNavigator()

        Try

            ' Minimise the Form
            m_fNavForm.WindowState = FormWindowState.Minimized

        Catch



            Exit Sub
        End Try


    End Sub

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: NavigateProcess
    '
    ' Description: Navigates through all Roadmaps within the current
    '              process.
    '
    ' ***************************************************************** '
    Private Function NavigateProcess() As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Show the Navigator Form
        m_lReturn = m_fNavForm.ShowForm()

        ' Do this Loop at Least Once.
        ' At the end of the Loop if the local ProcessID does not
        ' equal the Loaded Process.Process ID then there is a New
        ' Process to start, so loop again.
        Do

            ' Do we need to Start a New Process
            If ProcessID <> Process.ProcessID Then
                ' Yes, Start a New Process
                m_lReturn = CType(StartNewProcess(v_lProcessID:=ProcessID, v_vSetKeyArray:=m_vSetKeyArray), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(EndProcess(v_bError:=True), gPMConstants.PMEReturnCode)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else

                m_lReturn = DriveProcess()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(EndProcess(v_bError:=True), gPMConstants.PMEReturnCode)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

        Loop Until ProcessID = Process.ProcessID

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: StartNewProcess
    '
    ' Description: Starts a New Navigator Process
    '
    ' ***************************************************************** '
    Private Function StartNewProcess(ByVal v_lProcessID As Integer, ByVal v_vSetKeyArray As Object) As Integer

        Dim result As Integer = 0
        Dim vStepsArray As Object
        Static bSubProcess As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Is this a Sub Process or the Start one.
        If Not bSubProcess Then

            ' Start Process

            ' Set the Form Size as we have been to told to
            If StartMaximized Then
                m_fNavForm.WindowState = FormWindowState.Maximized
            Else
                m_fNavForm.WindowState = FormWindowState.Normal
            End If

        End If

        ' Initialisation Code.
        Process = New iPMNavigator.Process()
        'developer guide no. 9
        m_lReturn = Process.Initialise()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Start the Process
        m_lReturn = CType(Process.StartProcess(v_lProcessID:=v_lProcessID, v_vSetKeyArray:=v_vSetKeyArray), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Is this the First Process Started
        If Not bSubProcess Then
            ' Yes, so store the Transaction Type and Process Mode
            TransactionType = Process.TransactionType
            ProcessMode = Process.ProcessMode
            ' Set the Flag, so that if we 'StartNewProcess' again we
            ' will know that it is a Sub Process.
            bSubProcess = True
        End If

        ' Get the Steps for Start Map
        m_lReturn = CType(Process.StartMap.ReturnStepsAsArray(r_vMapStepsArray:=vStepsArray), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Tell the Form to start the new Process
        m_lReturn = CType(m_fNavForm.StartNewProcess(v_sProcessCaption:=Process.Caption, v_vStartMapSteps:=vStepsArray, v_eIsUserDriven:=Process.IsUserDriven), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        vStepsArray = Nothing

        ' What Type of Process are we Starting

        Select Case Process.IsUserDriven
            ' Navigator Driven
            Case MainModule.ACENavProcessType.aceProcTypeNavDriven

                ' Drive the User through the Steps
                m_lReturn = DriveProcess()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' User Driven
            Case MainModule.ACENavProcessType.aceProcTypeUserDriven

                ' Wait for the User to decide what to do.
                DrivenMode = False

                'RFC160299 - Data Capture Process Type Added.
                ' Navigable OR Data Capture
            Case MainModule.ACENavProcessType.aceProcTypeNavigable, MainModule.ACENavProcessType.aceProcTypeDataCapture

                ' Hide the Process Tree While it is building
                With m_fNavForm
                    .treDummy.Top = .treMainData.Top
                    .treDummy.Left = .treMainData.Left
                    .treDummy.Height = .treMainData.Height
                    .treDummy.Width = .treMainData.Width
                    .treDummy.Visible = True
                    .treMainData.Visible = False
                End With

                ' Build all Sub Maps
                m_lReturn = CType(BuildAllSubMaps(v_lMapID:=Process.StartMap.MapID), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Close All Sub Map Nodes
                m_fNavForm.CloseAllSubMapNodes()

                ' Unhide the Process Tree
                With m_fNavForm
                    .treMainData.Visible = True
                    .treDummy.Visible = False
                End With

                ' Process the First Step and then Navigate
                m_lReturn = DriveProcess()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

        End Select

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DriveProcess
    '
    ' Description: Navigates through the current roadmap and any other
    '              possible Roadmaps.
    '
    ' ***************************************************************** '
    Private Function DriveProcess() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lReturn As Integer

        Dim sStepKey As String
        Dim sNavigateStatus As String = ""

        Dim oCurrentStep As iPMNavigator.Step_Renamed
        Dim lLastAction As gPMConstants.PMEReturnCode
        Dim lWhatNext As Integer
        Dim vMapStepsArray As Object
        Dim oMap As iPMNavigator.Map



        result = gPMConstants.PMEReturnCode.PMTrue

        'DAK310700 - disable close while driven process is running
        m_fNavForm.cmdClose.Enabled = False
        m_fNavForm.mnuHelp.Enabled = False
        ' Initialise the Driven Mode Property
        DrivenMode = True

        ' Initialise the Last Action
        lLastAction = gPMConstants.PMEReturnCode.PMFalse

        ' Drive the User Through ALL of the Steps, until
        ' we are NOT in this mode.
        Do Until (Not DrivenMode)

            ' Find Out What to do next
            m_lReturn = CType(WhatNext(v_lLastAction:=lLastAction, r_lWhatNext:=lWhatNext), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                'DAK310700 - re-enable close command button
                DrivenMode = False
                m_fNavForm.cmdClose.Enabled = True
                m_fNavForm.mnuHelp.Enabled = True
                Return result
            End If

            ' Check the return code to decide the action.

            Select Case (lWhatNext)
                Case gPMConstants.PMEReturnCode.PMNavBuildMap
                    ' Build a new sub roadmap.
                    ' Add the Sub Map for the Current Step
                    oCurrentStep = Process.CurrentMap.CurrentStep
                    oMap = Process.Maps.Add(v_lMapID:=oCurrentStep.SubMapID, v_oParentStep:=oCurrentStep)
                    If oMap Is Nothing Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Exit Do
                    End If

                    ' Highlight the new roadmap step in the interface.
                    m_lReturn = CType(m_fNavForm.SelectStep(v_sStepKey:=oCurrentStep.StepKey), gPMConstants.PMEReturnCode)

                    ' Get the Steps for this Map
                    m_lReturn = CType(oMap.ReturnStepsAsArray(r_vMapStepsArray:=vMapStepsArray), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Exit Do
                    End If

                    ' Build the sub roadmap.
                    lReturn = m_fNavForm.BuildRoadmap(v_vMapStepsArray:=vMapStepsArray, sStepKey:=oCurrentStep.StepKey)

                    ' Check for errors.
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to build the sub roadmap.
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Exit Do
                    End If

                    lLastAction = gPMConstants.PMEReturnCode.PMNavBuildMap

                    ' End the Current Map
                Case gPMConstants.PMEReturnCode.PMNavEndMap

                    ' Close the Current Sub Map
                    m_fNavForm.CloseSubMap()
                    lLastAction = gPMConstants.PMEReturnCode.PMNavEndMap

                    ' Repeat the Sub Map
                Case gPMConstants.PMEReturnCode.PMNavRepeatMap

                    ' Untick the roadmap
                    m_fNavForm.ResetCurrentSubMap()
                    lLastAction = gPMConstants.PMEReturnCode.PMNavRepeatMap

                    ' Call the step component.
                Case gPMConstants.PMEReturnCode.PMNavCallComponent
                    ' Process all of the actions for this current roadmap step.
                    m_lReturn = CType(ProcessStep(r_lActionTaken:=lLastAction), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        DrivenMode = False
                    End If

                    ' Start a new process.
                Case gPMConstants.PMEReturnCode.PMNavStartNewProcess
                    m_lReturn = CType(ReturnKeyValuesAsArray(v_colKeysToReturn:=Process.GetKeys, v_colKeyValueCollection:=Process.StartMap.CurrentKeys, r_vKeyValueArray:=m_vSetKeyArray), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        DrivenMode = False
                    End If
                    DrivenMode = False

                    ' Let the User Navigate Around
                Case gPMConstants.PMEReturnCode.PMNavNavigate

                    ' Raise the Navigate Event
                    ' RaiseEvent Navigate

                    ' Set the Start Map Icons to greyed
                    ' and Close Sub Map Steps
                    sStepKey = Process.StartMap.CurrentStep.StepKey
                    m_lReturn = CType(m_fNavForm.SetForNavigateMode(v_sStartMapStepKey:=sStepKey), gPMConstants.PMEReturnCode)

                    DrivenMode = False

                    ' End the Process
                Case gPMConstants.PMEReturnCode.PMNavEndProcess
                    DrivenMode = False
                    m_lReturn = CType(EndProcess(v_bError:=False), gPMConstants.PMEReturnCode)

                    ' An unexpected error has occured.
                Case Else
                    DrivenMode = False

            End Select

        Loop

        'DAK310700 - re-enable close command button
        DrivenMode = False
        m_fNavForm.cmdClose.Enabled = True
        m_fNavForm.mnuHelp.Enabled = True

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ProcessStep
    '
    ' Description: Process all of the actions for the current Roadmap
    '              step.
    '
    ' ***************************************************************** '
    Private Function ProcessStep(ByRef r_lActionTaken As Integer) As Integer

        Dim result As Integer = 0
        Dim sStepKey, sComponentType, sComponentName As String
        Dim iIsServerSide As Integer
        Dim lTask As Integer
        Dim iIsHidden As gPMConstants.PMEReturnCode
        Dim vSetKeyArray As Object
        Dim oStepComponent As Object
        Dim lNavigateStatus As gPMConstants.PMENavigateButtonStatus
        Dim vGetKeyArray As Object
        'developer guide no. 17
        Dim vSummaryArray As Object
        Dim lStepAction As gPMConstants.PMEReturnCode
        Dim oNavV2Component As aPMNav.NavigatorV2
        Dim oNavV3Component As aPMNav.NavigatorV3
        Dim eNavVersion As NavigatorConstants.ACENavigatorVersion



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the Step Details
        With Process.CurrentMap.CurrentStep
            sStepKey = .StepKey
            sComponentType = .ComponentType
            sComponentName = .ObjectName & "." & .ClassName
            iIsServerSide = .IsServerSide
            lTask = .Task
            lNavigateStatus = CInt(.NavigateStatus)
            iIsHidden = .IsHidden
        End With

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' If in Development Mode allways allow Navigate
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If DevelopmentMode Then
            lNavigateStatus = gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
        End If

        'RFC111298 - Navigable Process Type Added.
        'RFC160299 - Data Capture Process Type Added.
        ' If the Process is Navigable OR Data Capture
        ' (i.e. Not Navigator Driven or User Driven)
        If (Process.IsUserDriven = MainModule.ACENavProcessType.aceProcTypeNavigable) Or (Process.IsUserDriven = MainModule.ACENavProcessType.aceProcTypeDataCapture) Then

            ' Update the START Map Keys

            ' Key Value Changes
            '        ' Set Initial Key Values
            '        m_lReturn = UpdateFromSetKeyInitValues( _
            ''            v_colSetKeys:=Process.CurrentMap.CurrentStep.SetKeys, _
            ''            v_colKeysToBeUpdated:=Process.StartMap.CurrentKeys)
            '        If (m_lReturn <> PMTrue) Then
            '            ProcessStep = PMFalse
            '            Exit Function
            '        End If

            ' Populate Set Key Array
            m_lReturn = CType(ReturnKeyValuesAsArray(v_colKeysToReturn:=Process.CurrentMap.CurrentStep.SetKeys, v_colKeyValueCollection:=Process.StartMap.CurrentKeys, r_vKeyValueArray:=vSetKeyArray), gPMConstants.PMEReturnCode)
            'DAK200300
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If AllowErrorRetry <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

        Else

            ' Update the CURRENT Map Keys

            ' Key Value Changes
            '        ' Set Initial Key Values
            '        m_lReturn = UpdateFromSetKeyInitValues( _
            ''            v_colSetKeys:=Process.CurrentMap.CurrentStep.SetKeys, _
            ''            v_colKeysToBeUpdated:=Process.CurrentMap.CurrentKeys)
            '        If (m_lReturn <> PMTrue) Then
            '            ProcessStep = PMFalse
            '            Exit Function
            '        End If

            ' Populate Set Key Array
            m_lReturn = CType(ReturnKeyValuesAsArray(v_colKeysToReturn:=Process.CurrentMap.CurrentStep.SetKeys, v_colKeyValueCollection:=Process.CurrentMap.CurrentKeys, r_vKeyValueArray:=vSetKeyArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If AllowErrorRetry <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

        End If

        ' Select the current roadmap step in the interface.
        m_lReturn = CType(m_fNavForm.SelectStep(sStepKey), gPMConstants.PMEReturnCode)

        ' Set the mouse pointer.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        ' Display status message.
        m_fNavForm.DisplayStatusMessage(iMessageType:=ACStatusMessageProcessStep)

        ' Create the step component.
        m_lReturn = CType(CreateComponent(iIsServerSide:=iIsServerSide, sComponentName:=sComponentName, oComponent:=oStepComponent), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            If AllowErrorRetry <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to create the component.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Display status message.
                m_fNavForm.DisplayStatusMessage(iMessageType:=ACStatusMessageNone)
                ' Set the mouse pointer.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If
        End If

        ' Check to see if this component is Navigator Version
        ' 3, 2 or 1 compliant

        Try
            ' If we get a Type Mismatch then this is NOT Version 3 compliant
            'developer guide no. 
            Dim iErrNo As Integer = 0
            Try
                oNavV3Component = oStepComponent
            Catch
                iErrNo = Information.Err().Number
            End Try

            'If Information.Err().Number = 0 Then
            If iErrNo = 0 Then
                ' Version 3
                eNavVersion = NavigatorConstants.ACENavigatorVersion.aceNavVersion3
            Else
                Information.Err().Clear()
                'developer guide no. 
                iErrNo = 0
                Try
                    oNavV2Component = oStepComponent
                Catch
                    iErrNo = Information.Err().Number
                End Try
                'If Information.Err().Number = 0 Then
                If iErrNo = 0 Then
                    ' Version 2
                    eNavVersion = NavigatorConstants.ACENavigatorVersion.aceNavVersion2
                Else
                    ' Version 1
                    eNavVersion = NavigatorConstants.ACENavigatorVersion.aceNavVersion1
                End If
            End If

            Information.Err().Clear()



            Select Case eNavVersion
                ' Version3 Compliant
                Case NavigatorConstants.ACENavigatorVersion.aceNavVersion3


                    m_lReturn = CType(ProcessV3Step(oNavV3Component:=oNavV3Component, sStepKey:=sStepKey, sComponentType:=sComponentType, sComponentName:=sComponentName, iIsServerSide:=iIsServerSide, lTask:=lTask, lNavigateStatus:=lNavigateStatus, vSetKeyArray:=vSetKeyArray, vGetKeyArray:=vGetKeyArray, vSummaryArray:=vSummaryArray, lStepAction:=lStepAction), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'DAK250100
                        If AllowErrorRetry = gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                            lStepAction = gPMConstants.PMEReturnCode.PMNavigate
                        Else
                            ' Set the mouse pointer.
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            Return m_lReturn
                        End If
                    End If

                    ' Version2 Compliant
                Case NavigatorConstants.ACENavigatorVersion.aceNavVersion2

                    m_lReturn = CType(ProcessV2Step(oNavV2Component:=oNavV2Component, sStepKey:=sStepKey, sComponentType:=sComponentType, sComponentName:=sComponentName, iIsServerSide:=iIsServerSide, lTask:=lTask, lNavigateStatus:=lNavigateStatus, vSetKeyArray:=vSetKeyArray, vGetKeyArray:=vGetKeyArray, vSummaryArray:=vSummaryArray, lStepAction:=lStepAction), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'DAK250100
                        If AllowErrorRetry = gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                            lStepAction = gPMConstants.PMEReturnCode.PMNavigate
                        Else
                            ' Set the mouse pointer.
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            Return m_lReturn
                        End If
                    End If

                    ' Type Mismatch - Version1
                Case NavigatorConstants.ACENavigatorVersion.aceNavVersion1

                    'developer guide no. 98
                    m_lReturn = CType(ProcessV1Step(oStepComponent:=oStepComponent, sStepKey:=sStepKey, sComponentType:=sComponentType, sComponentName:=sComponentName, iIsServerSide:=iIsServerSide, lTask:=lTask, lNavigateStatus:=lNavigateStatus, vSetKeyArray:=vSetKeyArray, vGetKeyArray:=vGetKeyArray, lStepAction:=lStepAction), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'DAK250100
                        If AllowErrorRetry = gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                            lStepAction = gPMConstants.PMEReturnCode.PMNavigate
                        Else
                            ' Set the mouse pointer.
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            Return m_lReturn
                        End If
                    End If
                    'developer guide no. 17
                    vSummaryArray = Nothing

                    ' Other Error
                Case Else
            End Select

            ' Terminate the component.
            'DAK080300 - do not attempt to Terminate a component if it is set to nothing and
            '            the AllowRetry registry entry is set
            If Not (oStepComponent Is Nothing) Or AllowErrorRetry = gPMConstants.PMEReturnCode.PMFalse Then


                oStepComponent.Dispose()
            End If

            ' Destroy the instance of the component from memory.
            oStepComponent = Nothing
            oNavV2Component = Nothing
            oNavV3Component = Nothing


            ' Return the Step Action
            r_lActionTaken = lStepAction

            ' If the Step is Hidden and the Component
            ' has returned Navigate, then
            ' Use OK Action.
            ' Note: A Hidden Step Cannot Return Navigate as it causes problems with Restart
            ' and the setting of the Icons.
            If (iIsHidden = gPMConstants.PMEReturnCode.PMTrue) And (r_lActionTaken = gPMConstants.PMEReturnCode.PMNavigate) Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="A Hidden Step Cannot Return Navigate")
                r_lActionTaken = gPMConstants.PMEReturnCode.PMOK
            End If

            ' Process the status of the current step.

            m_lReturn = CType(UpdateStep(sStepKey:=sStepKey, lStatus:=lStepAction, sComponentType:=sComponentType, vGetKeyArray:=vGetKeyArray, vSummaryArray:=vSummaryArray), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'DAK080300 - continue if AllowErrorRetry registry setting is set
                If AllowErrorRetry <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the step status.
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Set the mouse pointer.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

Err_ProcessStep:

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Set the mouse pointer.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the current roadmap step", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessStep", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result
        Catch exc As System.Exception
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: ProcessV1Step
    '
    ' Description: Process all of the actions for the current Roadmap
    '              step.
    '
    ' ***************************************************************** '
    'developer guide no. 17
    Private Function ProcessV1Step(ByRef oStepComponent As Object, ByRef sStepKey As String, ByRef sComponentType As String, ByRef sComponentName As String, ByRef iIsServerSide As Integer, ByRef lTask As Integer, ByRef lNavigateStatus As Integer, ByRef vSetKeyArray As Object, ByRef vGetKeyArray As Object, ByRef lStepAction As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' ********************************************************* '
        ' Note: As this is a pre Navigator V2 component we cannot
        '       be sure what methods & properties the component has.
        '       Therefore On Error Resume Next is used before all
        '       access to the component so that if the property
        '       method is NOT supported we can use the default.
        '
        ' ********************************************************* '

        ' Set the calling application name.
        Try

            oStepComponent.CallingAppName = ACApp
        Catch
        End Try

        ' Set the process modes for the step component.

        Try

            m_lReturn = oStepComponent.SetProcessModes(vTask:=lTask, vNavigate:=lNavigateStatus, vProcessMode:=ProcessMode, vTransactionType:=TransactionType, vEffectiveDate:=DateTime.Now)

            ' Check if we have an error.
            If Information.Err().Number <> 0 Then
                ' The above generated an error.


            Else


                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to set the process status.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Destroy the object from memory.
                    oStepComponent = Nothing
                    ' Set the mouse pointer.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    ' Display status message.
                    m_fNavForm.DisplayStatusMessage(iMessageType:=ACStatusMessageNone)
                    Return result
                End If
            End If

            ' Set the keys needed to use this component.
            If Information.IsArray(vSetKeyArray) Then

                m_lReturn = oStepComponent.SetKeys(vKeyArray:=vSetKeyArray)
                If Information.Err().Number <> 0 Then
                    ' The above generated an error.


                Else

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to set the keys.
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Destroy the object from memory.
                        oStepComponent = Nothing
                        ' Set the mouse pointer.
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        ' Display status message.
                        m_fNavForm.DisplayStatusMessage(iMessageType:=ACStatusMessageNone)
                        Return result
                    End If
                End If
            End If

            ' Display status message.
            m_fNavForm.DisplayStatusMessage(iMessageType:=ACStatusMessageNone)

            ' Is this a Server Side Component
            If iIsServerSide = gPMConstants.PMEReturnCode.PMTrue Then
                ' Yes it is, so leave the cursor busy
            Else
                ' No its Client Side, so reset the cursor.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Start the component.
            ' This Method MUST exist.

            m_lReturn = oStepComponent.Start()

            ' Check if we have an error.
            If Information.Err().Number <> 0 Then
                ' The Start Method MUST Exist
                ' Failed to set the process status.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Destroy the object from memory.
                oStepComponent = Nothing
                ' Set the mouse pointer.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Display status message.
                m_fNavForm.DisplayStatusMessage(iMessageType:=ACStatusMessageNone)
                Return result
            Else


                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to set the process status.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Destroy the object from memory.
                    oStepComponent = Nothing
                    ' Set the mouse pointer.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    ' Display status message.
                    m_fNavForm.DisplayStatusMessage(iMessageType:=ACStatusMessageNone)
                    Return result
                End If
            End If

            ' Get the keys set within the component.

            m_lReturn = oStepComponent.GetKeys(vKeyArray:=vGetKeyArray)

            ' Check if we have an error.
            If Information.Err().Number <> 0 Then
                ' The above generated an error.


                ' Default the Array
                'developer guide no.17 
                vGetKeyArray = Nothing
            Else


                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to set the process status.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Destroy the object from memory.
                    oStepComponent = Nothing
                    ' Set the mouse pointer.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    ' Display status message.
                    m_fNavForm.DisplayStatusMessage(iMessageType:=ACStatusMessageNone)
                    Return result
                End If
            End If

            ' Initialise the Component Status
            lStepAction = gPMConstants.PMEReturnCode.PMFalse
            Try
                ' Get the Component Status

                lStepAction = oStepComponent.Status
            Catch
                ' Assume OK if it doesn't support the property
                lStepAction = gPMConstants.PMEReturnCode.PMOK
            End Try


            ' If it is not OK, Cancel or Navigate, assume OK
            If (lStepAction <> gPMConstants.PMEReturnCode.PMOK) And (lStepAction <> gPMConstants.PMEReturnCode.PMCancel) And (lStepAction <> gPMConstants.PMEReturnCode.PMNavigate) Then
                lStepAction = gPMConstants.PMEReturnCode.PMOK
            End If

            Return result

Err_ProcessV1Step:

            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError
            ' Set the mouse pointer.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the current roadmap step", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessV1Step", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        Catch exc As System.Exception

        End Try

    End Function

    ' ***************************************************************** '
    ' Name: ProcessV2Step
    '
    ' Description: Process all of the actions for the current Roadmap
    '              step.
    '
    ' ***************************************************************** '
    Private Function ProcessV2Step(ByRef oNavV2Component As aPMNav.NavigatorV2, ByRef sStepKey As String, ByRef sComponentType As String, ByRef sComponentName As String, ByRef iIsServerSide As Integer, ByRef lTask As Integer, ByRef lNavigateStatus As Integer, ByRef vSetKeyArray As Object, ByRef vGetKeyArray As Object, ByRef vSummaryArray As Object, ByRef lStepAction As Integer) As Integer

        Dim result As Integer = 0
        Dim sProcessCompletionStatus, sMapCompletionStatus, sStepCompletionStatus As String



        result = gPMConstants.PMEReturnCode.PMTrue

        ' ***********************************************************
        ' Note: We are NOT Tracking Completion Status in this version
        ' of Navigator, therefore this will always be unknown
        sProcessCompletionStatus = gPMConstants.PMNavStatusUnknown
        sMapCompletionStatus = gPMConstants.PMNavStatusUnknown
        sStepCompletionStatus = gPMConstants.PMNavStatusUnknown
        ' ***********************************************************

        ' Set the calling application name.
        oNavV2Component.CallingAppName = ACApp

        ' Set the process modes for the step component.
        m_lReturn = CType(oNavV2Component.SetProcessModes(vTask:=lTask, vNavigate:=lNavigateStatus, vProcessMode:=ProcessMode, vTransactionType:=TransactionType, vEffectiveDate:=DateTime.Now), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to set the process modes.
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Destroy the object from memory.
            oNavV2Component = Nothing
            ' Set the mouse pointer.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            ' Display status message.
            m_fNavForm.DisplayStatusMessage(iMessageType:=ACStatusMessageNone)
            Return result
        End If


        ' Set the Status Properties
        m_lReturn = CType(oNavV2Component.SetStatus(sProcessStatus:=sProcessCompletionStatus, sMapStatus:=sMapCompletionStatus, sStepStatus:=sStepCompletionStatus), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to set the process modes.
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Destroy the object from memory.
            oNavV2Component = Nothing
            ' Set the mouse pointer.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            ' Display status message.
            m_fNavForm.DisplayStatusMessage(iMessageType:=ACStatusMessageNone)
            Return result
        End If

        ' Set the keys needed to use this component.
        If Information.IsArray(vSetKeyArray) Then
            m_lReturn = CType(oNavV2Component.SetKeys(vKeyArray:=vSetKeyArray), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to set the keys.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Destroy the object from memory.
                oNavV2Component = Nothing
                ' Set the mouse pointer.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Display status message.
                m_fNavForm.DisplayStatusMessage(iMessageType:=ACStatusMessageNone)
                Return result
            End If
        End If

        ' Display status message.
        m_fNavForm.DisplayStatusMessage(iMessageType:=ACStatusMessageNone)

        ' Is this a Server Side Component
        If iIsServerSide = gPMConstants.PMEReturnCode.PMTrue Then
            ' Yes it is, so leave the cursor busy
        Else
            ' No its Client Side, so reset the cursor.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End If

        ' Start the component.
        m_lReturn = CType(oNavV2Component.Start(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to start the component.
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Destroy the object from memory.
            oNavV2Component = Nothing
            Return result
        End If

        ' Get the keys set within the component.
        m_lReturn = CType(oNavV2Component.GetKeys(vKeyArray:=vGetKeyArray), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get the keys
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Destroy the object from memory.
            oNavV2Component = Nothing
            Return result
        End If

        ' Get the summary details set within the component.
        m_lReturn = CType(oNavV2Component.GetSummary(vSummaryArray:=vSummaryArray), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Destroy the object from memory.
            oNavV2Component = Nothing
            Return result
        End If

        ' Initialise the Component Status
        lStepAction = gPMConstants.PMEReturnCode.PMFalse
        ' Get the Component Status
        lStepAction = oNavV2Component.Status
        ' If it is not OK, Cancel or Navigate, assume OK
        If (lStepAction <> gPMConstants.PMEReturnCode.PMOK) And (lStepAction <> gPMConstants.PMEReturnCode.PMCancel) And (lStepAction <> gPMConstants.PMEReturnCode.PMNavigate) Then
            lStepAction = gPMConstants.PMEReturnCode.PMOK
        End If

        sStepCompletionStatus = oNavV2Component.StepStatus.Trim()

        ' Default the Step Status to Unknown
        If sStepCompletionStatus = gPMConstants.PMNavStatusUnknown Then
            sStepCompletionStatus = gPMConstants.PMNavStatusComplete
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ProcessV3Step
    '
    ' Description: Process all of the actions for the current Roadmap
    '              step.
    '
    ' ***************************************************************** '
    Private Function ProcessV3Step(ByRef oNavV3Component As aPMNav.NavigatorV3, ByRef sStepKey As String, ByRef sComponentType As String, ByRef sComponentName As String, ByRef iIsServerSide As Integer, ByRef lTask As Integer, ByRef lNavigateStatus As Integer, ByRef vSetKeyArray As Object, ByRef vGetKeyArray(,) As Object, ByRef vSummaryArray As Object, ByRef lStepAction As Integer) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set the calling application name.
        oNavV3Component.CallingAppName = ACApp

        ' Set the process modes for the step component.
        m_lReturn = CType(oNavV3Component.SetProcessModes(vTask:=lTask, vNavigate:=lNavigateStatus, vProcessMode:=ProcessMode, vTransactionType:=TransactionType, vEffectiveDate:=DateTime.Now), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to set the process modes.
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Destroy the object from memory.
            oNavV3Component = Nothing
            ' Set the mouse pointer.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            ' Display status message.
            m_fNavForm.DisplayStatusMessage(iMessageType:=ACStatusMessageNone)
            Return result
        End If


        ' Set the keys needed to use this component.
        If Information.IsArray(vSetKeyArray) Then
            m_lReturn = CType(oNavV3Component.SetKeys(vKeyArray:=vSetKeyArray), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to set the keys.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Destroy the object from memory.
                oNavV3Component = Nothing
                ' Set the mouse pointer.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                ' Display status message.
                m_fNavForm.DisplayStatusMessage(iMessageType:=ACStatusMessageNone)
                Return result
            End If
        End If

        ' Set the Authority Level on the Component.
        oNavV3Component.PMAuthorityLevel = PMAuthorityLevel

        ' Display status message.
        m_fNavForm.DisplayStatusMessage(iMessageType:=ACStatusMessageNone)

        ' Is this a Server Side Component
        If iIsServerSide = gPMConstants.PMEReturnCode.PMTrue Then
            ' Yes it is, so leave the cursor busy
        Else
            ' No its Client Side, so reset the cursor.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End If

        ' Start the component.
        m_lReturn = CType(oNavV3Component.Start(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to start the component.
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Destroy the object from memory.
            oNavV3Component = Nothing
            Return result
        End If

        ' Get the keys set within the component.
        m_lReturn = CType(oNavV3Component.GetKeys(vKeyArray:=vGetKeyArray), gPMConstants.PMEReturnCode)
        If Information.IsArray(vGetKeyArray) Then
            If vGetKeyArray.GetUpperBound(1) = 5 Then
                If Information.IsArray(vGetKeyArray(1, 5)) Then


                    PolicySharesArray = vGetKeyArray(1, 5)
                End If
            End If
        End If
        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get the keys
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Destroy the object from memory.
            oNavV3Component = Nothing
            Return result
        End If

        ' Get the summary details set within the component.
        m_lReturn = CType(oNavV3Component.GetSummary(vSummaryArray:=vSummaryArray), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Destroy the object from memory.
            oNavV3Component = Nothing
            Return result
        End If

        ' Initialise the Component Status
        lStepAction = gPMConstants.PMEReturnCode.PMFalse
        ' Get the Component Status
        lStepAction = oNavV3Component.Status
        ' If it is not OK, Cancel or Navigate, assume OK
        If (lStepAction <> gPMConstants.PMEReturnCode.PMOK) And (lStepAction <> gPMConstants.PMEReturnCode.PMCancel) And (lStepAction <> gPMConstants.PMEReturnCode.PMNavigate) Then
            lStepAction = gPMConstants.PMEReturnCode.PMOK
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CreateComponent
    '
    ' Description: Creates a component using the details passed.
    '
    ' ***************************************************************** '
    Private Function CreateComponent(ByRef iIsServerSide As Integer, ByRef sComponentName As String, ByRef oComponent As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' If in Development Mode allways create the Test Component.
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If DevelopmentMode Then
            sComponentName = "iTestInterfaceComponent.NavigatorV2"
            iIsServerSide = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check how to create the component then proceed with creation.
        If iIsServerSide = gPMConstants.PMEReturnCode.PMTrue Then

            m_lReturn = g_oObjectManager.GetInstance(oObject:=oComponent, sClassName:=sComponentName, vInstanceManager:=gPMConstants.PMGetViaClientManager)
        Else

            m_lReturn = g_oObjectManager.GetInstance(oObject:=oComponent, sClassName:=sComponentName, vInstanceManager:=gPMConstants.PMGetLocalInterface)

            If sComponentName = "iPMBDocTemplate.NavigatorV3" Then


                oComponent.PolicySharesArray = m_vPolicySharesArray
            End If
        End If

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to create component.
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Create Component :- " & sComponentName)
            Return result
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateStep
    '
    ' Description: Process the status of the step just being processed.
    '
    ' ***************************************************************** '
    Private Function UpdateStep(ByRef sStepKey As String, ByRef lStatus As Integer, ByRef sComponentType As String, ByRef vGetKeyArray(,) As Object, ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Repaint the Navigator Form
        'm_fNavForm.Refresh
        ' David Kyle - 24/08/1999 : Code supplied by Ashok Sharma
        ' Version 3.3.2
        ' Refresh the individual parts of the form to prevent flickering
        Application.DoEvents()
        m_fNavForm.lstwSummaryDetails.Refresh()
        m_fNavForm.panProcessTitle.Refresh()
        m_fNavForm.panSummaryTitle.Refresh()

        ' Set the Form Step Completion Status
        m_lReturn = CType(m_fNavForm.SetStepIcon(v_sStepKey:=sStepKey, v_lStatus:=lStatus), gPMConstants.PMEReturnCode)

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' If in Development Mode allways allow Navigate
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If DevelopmentMode Then
            m_lReturn = CType(ReturnKeyValuesAsArray(v_colKeysToReturn:=Process.CurrentMap.CurrentStep.GetKeys, v_colKeyValueCollection:=Process.CurrentMap.CurrentStep.GetKeys, r_vKeyValueArray:=vGetKeyArray), gPMConstants.PMEReturnCode)
            If Information.IsArray(vGetKeyArray) Then
                For lRow As Integer = vGetKeyArray.GetLowerBound(1) To vGetKeyArray.GetUpperBound(1)
                    If Information.VarType(vGetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) = VariantType.Object Then

                        MessageBox.Show("Object Reference in Key" & CStr(vGetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)), Application.ProductName)
                    Else

                        If Convert.IsDBNull(vGetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) Or IsNothing(vGetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)) Then

                            vGetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow) = "NULL"
                        End If



                        vGetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow) = Interaction.InputBox("Enter a Key Value For - " & CStr(vGetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)), , CStr(vGetKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    End If
                Next lRow
            End If
        End If
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'RFC111298 - Navigable Process Type Added.
        'RFC160299 - Data Capture Process Type Added.
        ' If the Process is Navigable OR Data Capture
        ' (i.e. Not Navigator Driven or User Driven)
        If (Process.IsUserDriven = MainModule.ACENavProcessType.aceProcTypeNavigable) Or (Process.IsUserDriven = MainModule.ACENavProcessType.aceProcTypeDataCapture) Then

            ' Update the START Map Keys with the Get Keys from the Step
            m_lReturn = CType(UpdateKeyValuesFromArray(v_colKeysToUpdate:=Process.CurrentMap.CurrentStep.GetKeys, v_colKeysToBeUpdated:=Process.StartMap.CurrentKeys, v_vKeyValueFrom:=vGetKeyArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Else

            ' Update the CURRENT Map Keys with the Get Keys from the Step
            m_lReturn = CType(UpdateKeyValuesFromArray(v_colKeysToUpdate:=Process.CurrentMap.CurrentStep.GetKeys, v_colKeysToBeUpdated:=Process.CurrentMap.CurrentKeys, v_vKeyValueFrom:=vGetKeyArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        ' Update the summary details.

        m_lReturn = CType(m_fNavForm.AddSummary(v_vSummaryArray:=vSummaryArray), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: WhatNext
    '
    ' Description: Calculates what to do next based on the Current Step
    '              and what the User did at the Step.
    ' ***************************************************************** '
    Private Function WhatNext(ByVal v_lLastAction As Integer, ByRef r_lWhatNext As Integer) As Integer

        Dim result As Integer = 0
        Dim sNextStepInstKey As String = ""

        Dim oCurrentStep, oNextStep As iPMNavigator.Step_Renamed
        Dim lStepProcessID, lStepNoOfSteps As Integer
        Dim sStepAction As New FixedLengthString(2)
        Dim lNextStepItemNumber As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        oCurrentStep = Process.CurrentMap.CurrentStep

        ' Has the user already done something at this step
        If (v_lLastAction = gPMConstants.PMEReturnCode.PMOK) Or (v_lLastAction = gPMConstants.PMEReturnCode.PMCancel) Then

            ' Get the Step Action according to the User Action Taken (OK, Cancel or Navigate)

            Select Case v_lLastAction
                ' OK
                Case gPMConstants.PMEReturnCode.PMOK
                    With oCurrentStep
                        sStepAction.Value = .OkAction
                        lStepProcessID = .OkProcessID
                        lStepNoOfSteps = .OkNoOfSteps
                    End With

                    ' Cancel
                Case gPMConstants.PMEReturnCode.PMCancel
                    With oCurrentStep
                        sStepAction.Value = .CancelAction
                        lStepProcessID = .CancelProcessID
                        lStepNoOfSteps = .CancelNoOfSteps
                    End With

                    ' Error - Assume OK
                Case Else
                    With oCurrentStep
                        sStepAction.Value = .OkAction
                        lStepProcessID = .OkProcessID
                        lStepNoOfSteps = .OkNoOfSteps
                    End With

            End Select

            ' After Processing a Step for a Navigable Process,
            ' always Navigate UNLESS the action is
            ' Complete OR Abort OR Start Process
            If Process.IsUserDriven = MainModule.ACENavProcessType.aceProcTypeNavigable Then
                If sStepAction.Value = gPMConstants.PMNavActionCompleteProcess Or sStepAction.Value = gPMConstants.PMNavActionAbortProcess Or sStepAction.Value = gPMConstants.PMNavActionStartProcess Then
                    ' Handle Complete OR Abort OR Start Process
                Else
                    r_lWhatNext = gPMConstants.PMEReturnCode.PMNavNavigate
                    Return result
                End If
            End If

            'RFC160299
            ' After Processing a SINGLE Step for a Data Capture Process,
            ' always Navigate UNLESS the action is
            ' Complete OR Abort OR Start Process
            If (Process.IsUserDriven = MainModule.ACENavProcessType.aceProcTypeDataCapture) And (m_fNavForm.cmdContinue.Enabled) Then
                If sStepAction.Value = gPMConstants.PMNavActionCompleteProcess Or sStepAction.Value = gPMConstants.PMNavActionAbortProcess Or sStepAction.Value = gPMConstants.PMNavActionStartProcess Then
                    ' Handle Complete OR Abort OR Start Process
                Else
                    r_lWhatNext = gPMConstants.PMEReturnCode.PMNavNavigate
                    Return result
                End If
            End If

            ' If this is User Driven Process
            ' AND we are on Start Map Step
            ' AND it is NOT a hidden step
            ' then ignore the user action and Navigate
            If (Process.IsUserDriven = MainModule.ACENavProcessType.aceProcTypeUserDriven) And (Process.CurrentMap.IsStartMap = gPMConstants.PMEReturnCode.PMTrue) And (oCurrentStep.IsHidden = gPMConstants.PMEReturnCode.PMFalse) Then
                r_lWhatNext = gPMConstants.PMEReturnCode.PMNavNavigate
                Return result
            End If

            ' Pocess the Step Action

            Select Case sStepAction.Value.Trim()
                ' Forward One Step or Forward X Number Of Steps
                Case gPMConstants.PMNavActionForwardOne, gPMConstants.PMNavActionForwardX, gPMConstants.PMNavActionBackOne, gPMConstants.PMNavActionBackX


                    ' If Forward One force Number of steps to be 1
                    If sStepAction.Value.Trim() = gPMConstants.PMNavActionForwardOne Then
                        lStepNoOfSteps = 1
                    End If

                    ' If Backward One force Number of steps to be 1
                    If sStepAction.Value.Trim() = gPMConstants.PMNavActionBackOne Then
                        lStepNoOfSteps = -1
                    End If

                    ' If Backward X Number of steps is negative
                    If sStepAction.Value.Trim() = gPMConstants.PMNavActionBackX Then
                        lStepNoOfSteps = -lStepNoOfSteps
                    End If

                    ' Calculate Next Step

                    ' Get the Item Number of the Current Step
                    lNextStepItemNumber = oCurrentStep.ItemNumber
                    ' Add the number of Steps to the Item Number
                    lNextStepItemNumber += lStepNoOfSteps

                    ' Get a Reference to the Next Step
                    'developer guide no. 98
                    oNextStep = Process.CurrentMap.Steps.Item(lNextStepItemNumber)

                    ' If no Next Step Instance then error
                    If oNextStep Is Nothing Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Next Step found for Step " & oCurrentStep.StepKey, vApp:=ACApp, vClass:=ACClass, vMethod:="WhatNext")
                        r_lWhatNext = gPMConstants.PMEReturnCode.PMNavEndProcess
                        Return result
                    End If

                    ' All Ok so set the Current Step to be this Step
                    Process.CurrentMap.CurrentStep = oNextStep
                    oCurrentStep = Nothing
                    oNextStep = Nothing

                    ' Exit Map
                Case gPMConstants.PMNavActionExitMap
                    ' If the Current Map is the Start Map
                    ' then End Process.
                    If Process.CurrentMap.IsStartMap = gPMConstants.PMEReturnCode.PMTrue Then
                        r_lWhatNext = gPMConstants.PMEReturnCode.PMNavEndProcess
                    Else
                        r_lWhatNext = gPMConstants.PMEReturnCode.PMNavEndMap
                    End If
                    Return result

                    ' Repeat Map
                Case gPMConstants.PMNavActionRepeatMap
                    r_lWhatNext = gPMConstants.PMEReturnCode.PMNavRepeatMap
                    Return result

                    ' Start NewProcess
                Case gPMConstants.PMNavActionStartProcess
                    r_lWhatNext = gPMConstants.PMEReturnCode.PMNavStartNewProcess
                    ' Set the ProcessID Property to be the Process to Start
                    ProcessID = lStepProcessID
                    Return result

                    ' End the Process - COMPLETE
                Case gPMConstants.PMNavActionCompleteProcess
                    ProcessComplete = True
                    r_lWhatNext = gPMConstants.PMEReturnCode.PMNavEndProcess
                    Return result

                    ' End the Process - ABORT
                Case gPMConstants.PMNavActionAbortProcess
                    r_lWhatNext = gPMConstants.PMEReturnCode.PMNavEndProcess
                    Return result

                    ' Unknown Action
                Case Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unknown Step Action - " & sStepAction.Value, vApp:=ACApp, vClass:=ACClass, vMethod:="WhatNext")
                    r_lWhatNext = gPMConstants.PMEReturnCode.PMNavEndProcess
                    Return result

            End Select

        ElseIf (v_lLastAction = gPMConstants.PMEReturnCode.PMNavigate) Then
            ' User selected Navigate
            r_lWhatNext = gPMConstants.PMEReturnCode.PMNavNavigate
            Return result

        ElseIf (v_lLastAction = gPMConstants.PMEReturnCode.PMNavBuildMap) Then
            ' Start the Sub Map
            m_lReturn = CType(Process.StartSubMap(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        ElseIf (v_lLastAction = gPMConstants.PMEReturnCode.PMNavEndMap) Then
            ' Exit the Sub Map
            m_lReturn = CType(Process.ExitSubMap(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' If the New Current Map is the Start Map
            ' AND this is a User Driven Process, then Navigate
            If (Process.CurrentMap.IsStartMap = gPMConstants.PMEReturnCode.PMTrue) And (Process.IsUserDriven = MainModule.ACENavProcessType.aceProcTypeUserDriven) Then
                r_lWhatNext = gPMConstants.PMEReturnCode.PMNavNavigate
                Return result
            End If

        ElseIf (v_lLastAction = gPMConstants.PMEReturnCode.PMNavRepeatMap) Then
            ' Repeat the Sub Map
            m_lReturn = CType(Process.RepeatSubMap(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        ' We have calculated the NEW Current Step.
        ' Is it a Step OR a Sub Map

        ' Get a Reference to the Current Step
        oCurrentStep = Process.CurrentMap.CurrentStep

        ' Is the Step is a Sub Map
        If oCurrentStep.IsSubMap = gPMConstants.PMEReturnCode.PMTrue Then
            ' Yes, then build it.
            r_lWhatNext = gPMConstants.PMEReturnCode.PMNavBuildMap
        Else
            ' No, so call component.
            r_lWhatNext = gPMConstants.PMEReturnCode.PMNavCallComponent
        End If
        oCurrentStep = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: EndProcess
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function EndProcess(ByVal v_bError As Boolean) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set the Finished Flag
        Finished = True

        ' As we have finished the Process, Let Work Manager Know
        ' whether the Process was completed or not.
        RaiseEvent SetProcessStatus(ProcessComplete)

        ' Tell the Form to End
        m_lReturn = CType(m_fNavForm.EndProcess(v_bError:=v_bError, v_bProcessComplete:=ProcessComplete), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If m_fNavForm.AutomaticCloseForm Then
            m_fNavForm.Parent_Renamed = Nothing
            m_fNavForm.Close()
            m_fNavForm = Nothing
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: BuildAllSubMaps
    '
    ' RFC111298 - Navigable Process Type Added.
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function BuildAllSubMaps(ByVal v_lMapID As Integer, Optional ByVal v_oParentStep As Step_Renamed = Nothing) As Integer

        Dim result As Integer = 0
        Dim oMap As iPMNavigator.Map
        Dim oStep As Step_Renamed
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vMapStepsArray As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        If v_oParentStep Is Nothing Then

            oMap = Process.StartMap

        Else

            oMap = Process.Maps.Add(v_lMapID:=v_lMapID, v_oParentStep:=v_oParentStep)
            If oMap Is Nothing Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Load Map ID = " & v_lMapID, vApp:=ACApp, vClass:=ACClass, vMethod:="Add")
                Return result
            End If

            ' Get the Steps for this Map
            lReturn = CType(oMap.ReturnStepsAsArray(r_vMapStepsArray:=vMapStepsArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Build the sub roadmap.
            lReturn = CType(m_fNavForm.BuildRoadmap(v_vMapStepsArray:=vMapStepsArray, sStepKey:=v_oParentStep.StepKey), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        For Each oStep2 As Step_Renamed In oMap.Steps
            oStep = oStep2
            With oStep
                If .SubMapID > 0 Then

                    lReturn = CType(BuildAllSubMaps(v_lMapID:=.SubMapID, v_oParentStep:=oStep), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lReturn
                    End If

                End If
            End With
        Next oStep2

        oStep = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetRegistrySettings
    '
    ' Description: Get Navigator Registry settings
    '
    ' History: 25/01/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Private Function GetRegistrySettings() As Integer
        Dim result As Integer = 0
        Dim sAllowErrorRetry As String = ""




        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=gPMConstants.ACRegKeyAllowErrorRetry, r_sSettingValue:=sAllowErrorRetry, v_sSubKey:=gPMConstants.ACNavRegSubKey), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Is there a setting
        Dim dbNumericTemp As Double
        If sAllowErrorRetry.Trim() = "" Then
            ' No, so do not allow
            m_lAllowErrorRetry = gPMConstants.PMEReturnCode.PMFalse
            ' Is it Numeric
        ElseIf (Double.TryParse(sAllowErrorRetry, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
            ' Is it True or False
            If CInt(sAllowErrorRetry) = gPMConstants.PMEReturnCode.PMTrue Then
                ' True, so allow
                m_lAllowErrorRetry = gPMConstants.PMEReturnCode.PMTrue
            Else
                ' False, so do NOT allow
                m_lAllowErrorRetry = gPMConstants.PMEReturnCode.PMFalse
            End If
        Else
            ' Not Numeric so do not allow
            m_lAllowErrorRetry = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    Private Sub m_fNavForm_CloseNavigator() Handles m_fNavForm.CloseNavigator

        ' Raise the Close Event
        RaiseEvent NavigatorClose()

        ' Terminate Myself
        Dispose()

    End Sub

    Private Sub m_fNavForm_DoubleClickStep(ByVal v_sStepKey As String) Handles m_fNavForm.DoubleClickStep



        ' If Navigator has finished then exit
        If Finished Then
            Exit Sub
        End If

        ' Can we Jump to the Step Selected
        m_lReturn = CType(Process.JumpToStep(v_sStepKey:=v_sStepKey), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        Else

            ' Raise the Jump To Step Event
            ' RaiseEvent JumpToStep

            ' Reset the Step Icons
            m_lReturn = CType(m_fNavForm.ReSetForJumpToStep(v_sStepKey:=Process.CurrentMap.CurrentStep.StepKey), gPMConstants.PMEReturnCode)

            ' Display the Step.
            m_lReturn = CType(NavigateProcess(), gPMConstants.PMEReturnCode)

        End If


    End Sub

    Private Sub m_fNavForm_ContinueProcess() Handles m_fNavForm.ContinueProcess

        Dim oStep As Step_Renamed
        Dim sCurrentStepKey, sNextStartMapStepKey As String
        Dim lItemNumber As Integer



        ' Raise the Restart Event
        ' RaiseEvent Restart

        ' Get the Current Step Key
        sCurrentStepKey = Process.CurrentMap.CurrentStep.StepKey

        ' Work out if we are in a Sub Map
        If sCurrentStepKey = Process.StartMap.CurrentStep.StepKey Then
            sNextStartMapStepKey = ""
        Else
            oStep = Process.StartMap.CurrentStep

            If oStep Is Nothing Then
                sNextStartMapStepKey = ""
            Else
                lItemNumber = oStep.ItemNumber
                lItemNumber += 1
                'developer guide no. 98
                oStep = Process.StartMap.Steps.Item(lItemNumber)
                If oStep Is Nothing Then
                    sNextStartMapStepKey = ""
                Else
                    sNextStartMapStepKey = oStep.StepKey
                End If
            End If
            oStep = Nothing
        End If

        ' Reset the Step Icons
        m_lReturn = CType(m_fNavForm.ReSetForContinue(v_sCurrentStepKey:=sCurrentStepKey, v_sNextStartMapStepKey:=sNextStartMapStepKey), gPMConstants.PMEReturnCode)

        ' Continue the Process
        m_lReturn = CType(NavigateProcess(), gPMConstants.PMEReturnCode)


    End Sub

    Private Sub m_fNavForm_StartProcess() Handles m_fNavForm.StartProcess

        ' Disable the Start Process Timer, so that it does
        ' not try and keep starting the process.
        m_fNavForm.tmrStartProcess.Enabled = False

        ' Start a New Process
        m_lReturn = CType(NavigateProcess(), gPMConstants.PMEReturnCode)

    End Sub

    'RFC160299
    Private Sub m_fNavForm_RestartProcess() Handles m_fNavForm.RestartProcess



        ' Set the Process Current Step
        m_lReturn = Process.RestartProcess()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        ' Reset Icons
        m_lReturn = m_fNavForm.ReSetForStartAgain()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        ' Start it Again
        m_lReturn = CType(NavigateProcess(), gPMConstants.PMEReturnCode)


    End Sub
End Class
