Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 02/10/1997
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' RFC160299 - Data Capture Process Type Added. (Including StartAgain)
    ' DAK200600 - Clear summary information when adding new summary data.
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    ' PUBLIC Events (Begin)
    Public Event DoubleClickStep(ByVal sStepKey As String)
    Public Event CloseNavigator()
    Public Event StartProcess()
    ' RFC160299 - RestartProcess Changed to Continue Process
    Public Event ContinueProcess()
    ' RFC160299 - Data Capture Process Type Added. (Including StartAgain)
    Public Event RestartProcess()
    ' PUBLIC Events (End)

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Parent - A reference back to the control class ,
    ' so that the Form will keep this Instance of Navigator alive
    ' even if there are NO other references to it.
    Private m_oParent As iPMNavigator.NavigateControl

    ' Object parameter members.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Selected node values.
    Private m_lSelectedNodeIndex As Integer
    Private m_sSelectedNodeKey As String = ""

    ' Slider bar data.
    Private m_bSliderMoved As Boolean
    Private m_lSliderPosition As Integer

    Private m_bAutomaticCloseForm As Boolean

    Private m_bFormShown As Boolean
    Private m_bFinished As Boolean
    Private m_eProcessType As MainModule.ACENavProcessType
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)
    Public Property Parent_Renamed() As iPMNavigator.NavigateControl
        Get
            ' Return the Parent property
            Return m_oParent
        End Get
        Set(ByVal Value As iPMNavigator.NavigateControl)

            '    '	' Set the Parent Property
            m_oParent = Value

        End Set
    End Property

    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)
    Public Property AutomaticCloseForm() As Boolean
        Get
            Return m_bAutomaticCloseForm
        End Get
        Set(ByVal Value As Boolean)
            m_bAutomaticCloseForm = Value
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
    Private Property ProcessType() As MainModule.ACENavProcessType
        Get
            Return m_eProcessType
        End Get
        Set(ByVal Value As MainModule.ACENavProcessType)
            m_eProcessType = Value
        End Set
    End Property

    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: ShowForm
    '
    ' Description: Shows the Form if it is not already Shown
    ' ***************************************************************** '
    Public Function ShowForm() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not m_bFormShown Then
                Me.Show()
                m_bFormShown = True
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowFormFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: HideForm
    '
    ' Description: Hides the Form
    ' ***************************************************************** '
    Public Function HideForm() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Me.Hide()
            m_bFormShown = False

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="HideFormFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="HideForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: StartNewProcess
    '
    ' Description: Sets the Navigator to start a new process.
    '
    ' ***************************************************************** '
    Public Function StartNewProcess(ByVal v_sProcessCaption As String, ByVal v_vStartMapSteps As Object, ByVal v_eIsUserDriven As MainModule.ACENavProcessType) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Display status message.
            DisplayStatusMessage(iMessageType:=ACStatusMessageNewProcess)

            ' What Type of Process Is this

            Select Case v_eIsUserDriven
                ' Navigator Driven
                Case MainModule.ACENavProcessType.aceProcTypeNavDriven
                    ProcessType = MainModule.ACENavProcessType.aceProcTypeNavDriven
                    DisplayProcessType(ACNavigatorDriven)
                    DisplayProcessStatus(MainModule.ACEProcStatus.aceProcStatusInProgress)

                    ' User Driven
                Case MainModule.ACENavProcessType.aceProcTypeUserDriven
                    ProcessType = MainModule.ACENavProcessType.aceProcTypeUserDriven
                    DisplayProcessType(ACUserDriven)
                    DisplayProcessStatus(MainModule.ACEProcStatus.aceProcStatusNavigate)

                    ' Navigable
                Case MainModule.ACENavProcessType.aceProcTypeNavigable
                    ProcessType = MainModule.ACENavProcessType.aceProcTypeNavigable
                    DisplayProcessType(ACNavigable)
                    DisplayProcessStatus(MainModule.ACEProcStatus.aceProcStatusInProgress)

                    ' RFC160299 - Data Capture Process Type Added.
                Case MainModule.ACENavProcessType.aceProcTypeDataCapture
                    ProcessType = MainModule.ACENavProcessType.aceProcTypeDataCapture
                    DisplayProcessType(ACDataCapture)
                    DisplayProcessStatus(MainModule.ACEProcStatus.aceProcStatusInProgress)
                    cmdRestart.Visible = True

            End Select

            ' Disable the Continue & Start Again Button
            cmdContinue.Enabled = False
            cmdRestart.Enabled = False

            ' Clear the interface.
            treMainData.Nodes.Clear()

            ' Do not clear the Summary Here as we want it
            ' to remian when one Process Starts another.
            '    ' Clear the Summary Details
            '    lstwSummaryDetails.ListItems.Clear

            ' Display the process caption.
            m_lReturn = DisplayProcessCaption(v_sProcessCaption:=v_sProcessCaption)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to display the process caption.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display status message.
                DisplayStatusMessage(iMessageType:=ACStatusMessageNone)

                ' Set the mouse pointer.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            ' Display status message.
            DisplayStatusMessage(iMessageType:=ACStatusMessageNone)

            ' Set the mouse pointer.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Build the Roadmap using the map ID.
            m_lReturn = CType(BuildRoadmap(v_vMapStepsArray:=v_vStartMapSteps), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to start the Roadmap
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            treMainData.ExpandAll()
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start a new navigator process", vApp:=ACApp, vClass:=ACClass, vMethod:="StartNewProcess", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EndProcess
    '
    ' Description: If Automatic Close Down is set, then hide the form.
    '              Otherwise, disable the Continue button and display a
    '              status of Finished.
    ' ***************************************************************** '
    Public Function EndProcess(ByVal v_bError As Boolean, ByVal v_bProcessComplete As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Finished = True

            ' Disable the Continue & Start Again Button
            cmdContinue.Enabled = False
            cmdRestart.Enabled = False

            If AutomaticCloseForm Then
                m_lReturn = HideForm()
            Else
                ' Error takes precedence over Complete/Abort.
                If v_bError Then
                    DisplayProcessStatus(MainModule.ACEProcStatus.aceProcStatusError)
                Else
                    If v_bProcessComplete Then
                        DisplayProcessStatus(MainModule.ACEProcStatus.aceProcStatusComplete)
                    Else
                        DisplayProcessStatus(MainModule.ACEProcStatus.aceProcStatusIncomplete)
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EndProcessFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="EndProcess", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BuildRoadmap
    '
    ' Description: Builds a Navigator Roadmap from the map ID.
    '
    ' ***************************************************************** '
    Public Function BuildRoadmap(ByVal v_vMapStepsArray As Object, Optional ByVal sStepKey As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Display status message.
            DisplayStatusMessage(iMessageType:=ACStatusMessageBuildRoadmap)

            ' Display the roadmap captions.

            m_lReturn = CType(DisplayMapCaptions(vCaptionArray:=v_vMapStepsArray, sStepKey:=sStepKey), gPMConstants.PMEReturnCode)

            '    ' Display the frame for the summary details.
            '    m_lReturn& = DisplaySummaryDetailsFrame()

            ' Destroy the splash screen (if currently displayed).
            m_lReturn = CType(SplashScreen(bDisplay:=False), gPMConstants.PMEReturnCode)

            ' Display status message.
            DisplayStatusMessage(iMessageType:=ACStatusMessageNone)

            ' Set the mouse pointer.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to build the Roadmap", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildRoadmap", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectStep
    '
    ' Description: Selects the supplied Step
    ' ***************************************************************** '
    Public Function SelectStep(ByVal v_sStepKey As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Focus to the Tree View
            treMainData.Focus()

            ' Select the current roadmap step in the interface.
            If treMainData.Nodes.Item(0).Name = v_sStepKey Then
                treMainData.SelectedNode = treMainData.Nodes.Item(0)
            Else
                For i As Integer = 0 To treMainData.Nodes.Item(0).Nodes.Count - 1
                    If treMainData.Nodes.Item(0).Nodes(i).Name = v_sStepKey Then
                        treMainData.SelectedNode = treMainData.Nodes.Item(0).Nodes.Item(i)
                    End If
                Next
            End If

            Return result

        Catch



            ' If we get to here, then the Step we have been asked to Select
            ' is a Hidden Step and therefore not in the TreeView.
            ' Do NOT display an error.

            Return result
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: CloseSubMap
    '
    ' Description: Closes the current Sub Map.
    ' ***************************************************************** '
    Public Sub CloseSubMap()

        Try

            ' Set the Current Sub Map Step Icons to Normal
            ResetCurrentSubMap()

            ' Close the Sub Map
            treMainData.Nodes.Item(treMainData.SelectedNode.Index - 1).Parent.Collapse()

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CloseSubMapFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CloseSubMap", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: ResetCurrentSubMap
    '
    ' Description: Sets the Current Sub Map Step Icons to normal.
    '
    '
    ' ***************************************************************** '
    Public Sub ResetCurrentSubMap()

        Try


            m_lReturn = CType(SetStepIconMultiple(treMainData.Nodes.Item(treMainData.SelectedNode.Index).FirstNode.Index, treMainData.Nodes.Item(treMainData.SelectedNode.Index - 1).LastNode.Index, gPMConstants.PMEReturnCode.PMTrue), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ResetCurrentSubMapFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ResetCurrentSubMap", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: SetForNavigateMode
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function SetForNavigateMode(ByVal v_sStartMapStepKey As String) As Integer


        Dim result As Integer = 0
        Dim oNode As TreeNode = Nothing
        Dim lDisallowJumpFrom, lDisallowJumpTo As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Show that we are in Navigate Mode
            DisplayProcessStatus(MainModule.ACEProcStatus.aceProcStatusNavigate)

            ' RFC160299 - Data Capture Process Type Added.
            ' If this is a Navigable OR Data Capture Process
            If (ProcessType = MainModule.ACENavProcessType.aceProcTypeNavigable) Or (ProcessType = MainModule.ACENavProcessType.aceProcTypeDataCapture) Then

                ' Enable Continue & Start Again for Data Capture
                If ProcessType = MainModule.ACENavProcessType.aceProcTypeDataCapture Then
                    cmdContinue.Enabled = True
                    cmdRestart.Enabled = True
                End If

                ' Nothing else to do, so exit
                Return result

            Else
                ' Otherwise, Close all Sub Map Steps
                m_lReturn = CloseAllSubMapNodes()
            End If

            ' Disable Steps if this is a Navigator Driven Process
            If ProcessType = MainModule.ACENavProcessType.aceProcTypeNavDriven Then

                ' Enable the Continue Button
                cmdContinue.Enabled = True

                ' Work out which nodes to disable.
                'Added By Sumeet - loop through nodes for correct functioning.
                'start
                'oNode = treMainData.Nodes.Item(v_sStartMapStepKey)
                If treMainData.Nodes.Item(0).Name = v_sStartMapStepKey Then
                    oNode = treMainData.Nodes.Item(0)
                Else
                    For i As Integer = 0 To treMainData.Nodes.Item(0).Nodes.Count - 1
                        If treMainData.Nodes.Item(0).Nodes(i).Name = v_sStartMapStepKey Then
                            oNode = treMainData.Nodes.Item(0).Nodes.Item(i)
                        End If
                    Next
                End If
                'end

                lDisallowJumpFrom = oNode.Index + 1

                'developer guide no. 34
                lDisallowJumpTo = oNode.LastNode.Index

                ' Disable them
                m_lReturn = CType(SetStepIconMultiple(v_lFromStepIndex:=lDisallowJumpFrom, v_lToStepIndex:=lDisallowJumpTo, v_lStatus:=gPMConstants.PMEReturnCode.PMFalse), gPMConstants.PMEReturnCode)
                oNode = Nothing

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetForNavigateModeFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetForNavigateMode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ReSetForJumpToStep
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function ReSetForJumpToStep(ByVal v_sStepKey As String) As Integer

        Dim result As Integer = 0
        Dim oNode As TreeNode = Nothing
        Dim lFrom, lTo As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Show that we are In Progress
            DisplayProcessStatus(MainModule.ACEProcStatus.aceProcStatusInProgress)

            ' If this is NOT a Navigator Driven Process then no steps
            ' will have been greyed, therefore we do not need to reset them
            If ProcessType <> MainModule.ACENavProcessType.aceProcTypeNavDriven Then
                Return result
            End If

            ' Disable the Continue Button
            cmdContinue.Enabled = False
            cmdRestart.Enabled = False

            ' Get the Node
            'Added By Sumeet - loop through nodes for correct functioning.
            'start
            'oNode = treMainData.Nodes.Item(v_sStepKey)
            If treMainData.Nodes.Item(0).Name = v_sStepKey Then
                oNode = treMainData.Nodes.Item(0)
            Else
                For i As Integer = 0 To treMainData.Nodes.Item(0).Nodes.Count - 1
                    If treMainData.Nodes.Item(0).Nodes(i).Name = v_sStepKey Then
                        oNode = treMainData.Nodes.Item(0).Nodes.Item(i)
                    End If
                Next
            End If
            'end

            ' Reset this node and all following Nodes.
            lFrom = oNode.Index

            'developer guide no. 34
            lTo = oNode.LastNode.Index

            m_lReturn = CType(SetStepIconMultiple(lFrom, lTo, gPMConstants.PMEReturnCode.PMTrue), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReSetForJumpToStepFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReSetForJumpToStep", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ReSetForContinue
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function ReSetForContinue(ByVal v_sCurrentStepKey As String, Optional ByVal v_sNextStartMapStepKey As String = "") As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Show that we are In Progress
            DisplayProcessStatus(MainModule.ACEProcStatus.aceProcStatusInProgress)

            ' Disable the Continue & Start Again Button
            cmdContinue.Enabled = False
            cmdRestart.Enabled = False

            ' If this is NOT a Navigator Driven Process then no steps
            ' will have been greyed, therefore we do not need to reset them
            If ProcessType <> MainModule.ACENavProcessType.aceProcTypeNavDriven Then
                Return result
            End If

            ' Reset the Icons for the Current Step and any following Steps
            m_lReturn = CType(ReSetForJumpToStep(v_sStepKey:=v_sCurrentStepKey), gPMConstants.PMEReturnCode)

            ' As the Current Step MAY be in a Sub Map
            ' We need to reset any steps in the Start Map
            ' after the Sub Map we are in.
            If v_sNextStartMapStepKey.Trim() <> "" Then
                m_lReturn = CType(ReSetForJumpToStep(v_sStepKey:=v_sNextStartMapStepKey), gPMConstants.PMEReturnCode)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReSetForContinueFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReSetForContinue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'RFC160299
    ' ***************************************************************** '
    ' Name: ReSetForStartAgain
    '
    ' Description: Resets all the Icons to their Normal values and
    '              closes all Sub Map Nodes.
    ' ***************************************************************** '
    Public Function ReSetForStartAgain() As Integer

        Dim result As Integer = 0
        Dim oNode As TreeNode
        Dim lFrom, lTo As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Show that we are In Progress
            DisplayProcessStatus(MainModule.ACEProcStatus.aceProcStatusInProgress)

            ' Disable the Continue Button
            cmdContinue.Enabled = False
            cmdRestart.Enabled = False

            ' Get First Step in the Start Map
            ' Note: Node one is the Process
            oNode = treMainData.Nodes.Item(0)

            ' Reset this node and all following Nodes.
            lFrom = oNode.Index
            'developer guide no. 34
            lTo = oNode.LastNode.Index

            m_lReturn = CType(SetStepIconMultiple(v_lFromStepIndex:=lFrom, v_lToStepIndex:=lTo, v_lStatus:=gPMConstants.PMEReturnCode.PMTrue), gPMConstants.PMEReturnCode)

            m_lReturn = CType(CloseAllSubMapNodes(), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReSetForStartAgainFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReSetForStartAgain", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetStepIcon
    '
    ' Description: Set the Icon for the Step just processed.
    '
    ' ***************************************************************** '
    Public Function SetStepIcon(ByVal v_sStepKey As String, ByVal v_lStatus As Integer) As Integer

        Dim result As Integer = 0
        Dim sIcon, sComponentType As String
        Dim oNode As TreeNode = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get a Reference to the Step
            'Added By Sumeet - loop through nodes for correct functioning.
            'start
            'oNode = treMainData.Nodes.Item(v_sStepKey)
            If treMainData.Nodes.Item(0).Name = v_sStepKey Then
                oNode = treMainData.Nodes.Item(0)
            Else
                For i As Integer = 0 To treMainData.Nodes.Item(0).Nodes.Count - 1
                    If treMainData.Nodes.Item(0).Nodes(i).Name = v_sStepKey Then
                        oNode = treMainData.Nodes.Item(0).Nodes.Item(i)
                    End If
                Next
            End If
            'end

            ' If we cannot find the Step in the Tree View
            ' it is a Hidden Step, so just exit.
            If oNode Is Nothing Then
                Return result
            End If

            ' Get the Component Type From the Node Tag.
            sComponentType = Convert.ToString(oNode.Tag).Trim()

            ' Get the new Icon for the Step.
            sIcon = DeriveStepIcon(v_lStatus:=v_lStatus, v_sComponentType:=sComponentType)

            ' Change the icon of the completed step.
            With oNode

                .ImageKey = sIcon

                '.SelectedImageIndex = CInt(sIcon) - 1

                'Developer guide No:210
                .SelectedImageKey = sIcon
            End With

            oNode = Nothing

            Return result

        Catch



            ' If we get to here, then the Step we have been asked to Set the Icon for
            ' is a Hidden Step and therefore not in the TreeView.
            ' Do NOT display an error.

            Return result
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: DisplayStatusMessage
    '
    ' Description: Displays the appropriate message within the status
    '              bar.
    '
    ' ***************************************************************** '
    Public Sub DisplayStatusMessage(ByRef iMessageType As Integer)

        Static sMessageNewProcess, sMessageBuildRoadmap, sMessageProcessStep As String
        Dim sMessage As String = ""

        Try

            ' Get all of the captions from the resource file
            ' for speed (if not done already).
            If sMessageNewProcess = "" Then
                ' Get the status message for a new process.

                'Developer Guide no: 243
                sMessageNewProcess = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACStatusNewProcess, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Get the status message for a build roadmap.

                'Developer Guide no: 243
                sMessageBuildRoadmap = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACStatusBuildRoadmap, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Get the status message for a process step.

                'Developer Guide no: 243
                sMessageProcessStep = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACStatusProcessStep, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Select the appropriate message.
            Select Case (iMessageType)
                Case ACStatusMessageNone
                    sMessage = ""

                Case ACStatusMessageNewProcess
                    sMessage = sMessageNewProcess

                Case ACStatusMessageBuildRoadmap
                    sMessage = sMessageBuildRoadmap

                Case ACStatusMessageProcessStep
                    sMessage = sMessageProcessStep
            End Select

            ' Display the status bar message.
            staStatus.Items.Item(2).Text = " " & sMessage.Trim()
            staStatus.Refresh()

        Catch excep As System.Exception



            ' Error Section.

            staStatus.Items.Item(2).Text = ""
            staStatus.Refresh()

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusMessage", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: AddSummary
    '
    ' Description: Adds the Summary details supplied to the ListView
    ' ***************************************************************** '
    Public Function AddSummary(ByVal v_vSummaryArray As Object) As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If there is no Summary to add, just exit.
            If Not Information.IsArray(v_vSummaryArray) Then
                Return result
            End If

            'DAK200600 - Clear previous summary information
            lstwSummaryDetails.Items.Clear()

            ' Add Each Row in the Array to the List
            For lRow As Integer = v_vSummaryArray.GetLowerBound(1) To v_vSummaryArray.GetUpperBound(1)

                ' If is the Very First Item of Summary for this Process
                If lstwSummaryDetails.Items.Count = 0 Then
                    ' Add it to the Form Caption

                    Me.Text = Me.Text & " (" & CStr(v_vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, lRow)).Trim() & ")"
                End If

                ' Add the Summary to the List View

                oListItem = lstwSummaryDetails.Items.Add(CStr(v_vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, lRow)).Trim())

                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(v_vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, lRow)).Trim()

            Next lRow

            lstwSummaryDetails.Refresh()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddSummaryFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: DisplayProcessType
    '
    ' Description: Displays the appropriate message within the status
    '              bar.
    '
    ' ***************************************************************** '
    Private Sub DisplayProcessType(ByRef lProcessType As Integer)

        Dim sMessage As String = ""

        Try

            ' Get the process Type String from the Resource file

            'Developer Guide no: 243
            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=lProcessType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Display the status bar message.
            staStatus.Items.Item(0).Text = " " & sMessage.Trim()
            staStatus.Refresh()

        Catch excep As System.Exception



            ' Error Section.

            staStatus.Items.Item(0).Text = ""
            staStatus.Refresh()

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayProcessType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: DisplayProcessStatus
    '
    ' Description: Displays the appropriate message within the status
    '              bar.
    '
    ' ***************************************************************** '
    Private Sub DisplayProcessStatus(ByRef eProcessStatus As MainModule.ACEProcStatus)

        Dim sMessage As String = ""

        Try

            ' Get the process Type String from the Resource file

            'Developer Guide no: 243
            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=eProcessStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Display the status bar message.
            staStatus.Items.Item(1).Text = " " & sMessage.Trim()
            staStatus.Refresh()

        Catch excep As System.Exception



            ' Error Section.

            staStatus.Items.Item(1).Text = ""
            staStatus.Refresh()

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayProcessStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: ConfirmClose
    '
    ' Description:
    ' ***************************************************************** '
    Private Function ConfirmClose() As Integer

        Dim result As Integer = 0
        Dim lReturn As DialogResult
        Dim sHeader, sMsg As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If the Process Has finished then do no confirm
            If Finished Then
                Return result
            End If


            'Developer Guide no: 243
            sHeader = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACConfirmCloseHeader, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACConfirmCloseMsg, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            If sMsg.Trim() = "" Then
                sMsg = "The current process has not finished." & Strings.Chr(13) & Strings.Chr(10) & "Are you sure you want to close Navigator?"
            End If

            If sHeader.Trim() = "" Then
                sHeader = "Confirm Close of Navigator"
            End If

            lReturn = MessageBox.Show(sMsg, sHeader, MessageBoxButtons.YesNo)

            If lReturn = System.Windows.Forms.DialogResult.No Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConfirmCloseFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConfirmClose", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DisplayProcessCaption
    '
    ' Description: Displays the Navigator process caption.
    '
    ' ***************************************************************** '
    Private Function DisplayProcessCaption(ByVal v_sProcessCaption As String) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oProcessNode As TreeNode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Me.Text = v_sProcessCaption

            ' Display the Navigator process caption to the interface.
            oProcessNode = treMainData.Nodes.Add(ACProcessIndex, v_sProcessCaption, ACIconProcess, ACIconProcess)

            ' Use the Tag Property to store the Component Type
            ' "P" equals Process
            oProcessNode.Tag = ACProcessIndex

            ' Expand this node.
            oProcessNode.Expand()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the navigator process caption", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayProcessCaption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeriveStepIcon
    '
    ' Description: Works out what the Step Icon should be based on the
    '              type of step and the status of the Step.
    '              PMTrue = Normal
    '              PMOK = Ticked
    '              PMCancel = Crossed
    '              PMFalse = Greyed
    ' ***************************************************************** '
    Public Function DeriveStepIcon(ByVal v_lStatus As Integer, Optional ByVal v_sComponentType As String = "") As String

        Dim result As String = String.Empty
        Dim sNormalIcon, sTickIcon, sCrossIcon, sGreyedIcon As String

        Try

            result = ACIconProcess

            ' Get the Normal, Tickes, Crossed & Greyed Icon, Dependant on the Step Type

            Select Case v_sComponentType.Trim()
                ' Data Form
                Case gPMConstants.PMNavComponentDataForm
                    sNormalIcon = ACIconStepDataForm
                    sTickIcon = ACIconStepDataFormTick
                    sCrossIcon = ACIconStepDataFormCross
                    sGreyedIcon = ACIconStepDataFormGrey

                    ' Find Form
                Case gPMConstants.PMNavComponentFindForm
                    sNormalIcon = ACIconStepFind
                    sTickIcon = ACIconStepFindTick
                    sCrossIcon = ACIconStepFindCross
                    sGreyedIcon = ACIconStepFindGrey

                    ' Decision Form
                Case gPMConstants.PMNavComponentDecisionForm
                    sNormalIcon = ACIconStepDecision
                    sTickIcon = ACIconStepDecisionTick
                    sCrossIcon = ACIconStepDecisionCross
                    sGreyedIcon = ACIconStepDecisionGrey

                    ' Business Object
                Case gPMConstants.PMNavComponentBusinessObject
                    sNormalIcon = ACIconStepNoForm
                    sTickIcon = ACIconStepNoFormTick
                    sCrossIcon = ACIconStepNoFormCross
                    sGreyedIcon = ACIconStepNoFormGrey

                    ' Sub Map
                Case ""
                    ' Note: Sub Maps do not have Ticked & Crossed Versions
                    sNormalIcon = ACIconSubMap
                    sTickIcon = ACIconSubMap
                    sCrossIcon = ACIconSubMap
                    sGreyedIcon = ACIconSubMapGrey

                    ' Unknown Component Type, Use Data Form
                Case Else
                    sNormalIcon = ACIconStepDataForm
                    sTickIcon = ACIconStepDataFormTick
                    sCrossIcon = ACIconStepDataFormCross
                    sGreyedIcon = ACIconStepDataFormGrey

            End Select

            ' Return the correct Icon for the Step Status

            Select Case (v_lStatus)
                Case gPMConstants.PMEReturnCode.PMOK
                    Return sTickIcon

                Case gPMConstants.PMEReturnCode.PMCancel
                    Return sCrossIcon

                Case gPMConstants.PMEReturnCode.PMTrue
                    Return sNormalIcon

                Case gPMConstants.PMEReturnCode.PMFalse
                    Return sGreyedIcon

                Case Else
                    Return sNormalIcon
            End Select

        Catch
        End Try



        ' Error Section.

        result = ACIconProcess

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to work out the correct Icon for the step.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeriveStepIcon", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DisplayMapCaptions
    '
    ' Description: Displays the roadmap captions.
    '
    ' ***************************************************************** '
    Private Function DisplayMapCaptions(ByRef vCaptionArray(,) As Object, Optional ByRef sStepKey As String = "") As Integer

        Dim result As Integer = 0
        Dim oStepNode As TreeNode
        Dim sStepIcon As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if we have a blank step key.
            If sStepKey = "" Then
                ' Set the step key to the process index.
                sStepKey = ACProcessIndex
            End If

            ' If the Parent Step has Children then we have already added this Map
            If treMainData.Nodes.Item(sStepKey).GetNodeCount(False) > 0 Then
                Return result
            End If

            ' Display the roadmap step captions.
            For lRow As Integer = vCaptionArray.GetLowerBound(1) To vCaptionArray.GetUpperBound(1)

                ' Check if the step is a sub roadmap.
                If vCaptionArray(gPMConstants.PMENavCaptionArrayColPosition.PMNavCaptionIsSubMap, lRow) = gPMConstants.PMEReturnCode.PMTrue Then

                    ' Derive the Step Icon
                    sStepIcon = DeriveStepIcon(v_lStatus:=gPMConstants.PMEReturnCode.PMTrue)

                    ' Display sub roadmap step.

                    oStepNode = treMainData.Nodes.Find(sStepKey, True)(0).Nodes.Add(vCaptionArray(gPMConstants.PMENavCaptionArrayColPosition.PMNavCaptionStepKey, lRow), CStr(vCaptionArray(gPMConstants.PMENavCaptionArrayColPosition.PMNavCaptionCaption, lRow)).Trim(), sStepIcon, sStepIcon)

                    ' Set expanded.
                    oStepNode.Expand()

                    ' Use the Node Tag Property to store the Component Type
                    ' Empty String uquals Sub Map
                    oStepNode.Tag = ""

                Else

                    ' Derive the Step Icon

                    sStepIcon = DeriveStepIcon(v_lStatus:=gPMConstants.PMEReturnCode.PMTrue, v_sComponentType:=CStr(vCaptionArray(gPMConstants.PMENavCaptionArrayColPosition.PMNavCaptionComponentType, lRow)).Trim())

                    ' Display step.

                    oStepNode = treMainData.Nodes.Find(sStepKey, True)(0).Nodes.Add(vCaptionArray(gPMConstants.PMENavCaptionArrayColPosition.PMNavCaptionStepKey, lRow), CStr(vCaptionArray(gPMConstants.PMENavCaptionArrayColPosition.PMNavCaptionCaption, lRow)).Trim(), sStepIcon, sStepIcon)

                    ' Use the Node Tag Property to store the Component Type

                    oStepNode.Tag = CStr(vCaptionArray(gPMConstants.PMENavCaptionArrayColPosition.PMNavCaptionComponentType, lRow)).Trim()

                End If

            Next lRow

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the Map captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayMapCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetStepIconMultiple
    '
    ' Description: Set the Step Icon for the Range of Steps.
    '
    ' ***************************************************************** '
    Private Function SetStepIconMultiple(ByVal v_lFromStepIndex As Integer, ByVal v_lToStepIndex As Integer, ByVal v_lStatus As Integer) As Integer

        Dim result As Integer = 0
        Dim lSubLoopFrom, lSubLoopTo As Integer
        Dim sNewStepKey As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode
        Static lRecursionCount As Integer
        Dim oNode As TreeNode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If lRecursionCount > 1000 Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Too Many Sub Maps - Recursion Error", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStepIconMultiple")
                Return result
            End If

            ' For Each Node
            For lIndex As Integer = v_lFromStepIndex To v_lToStepIndex

                oNode = treMainData.Nodes.Item(lIndex - 1)
                If oNode Is Nothing Then
                    oNode = treMainData.Nodes.Item(0).Nodes.Item(lIndex - 1)
                End If

                ' Get the Step Index
                sNewStepKey = oNode.Name
                ' Set the Icon for the Step
                lReturn = CType(SetStepIcon(v_sStepKey:=sNewStepKey, v_lStatus:=v_lStatus), gPMConstants.PMEReturnCode)

                ' Is the Node a Sub Map
                If treMainData.Nodes.Item(lIndex - 1).GetNodeCount(False) > 0 Then
                    ' Yes - Its a Sub Map

                    ' Get the First and Last Node Index

                    lSubLoopFrom = oNode.Nodes(0).Index

                    lSubLoopTo = oNode.FirstNode.LastNode.Index

                    ' Call Myself to Set the Icons for the Sub Map
                    lRecursionCount += 1
                    lReturn = CType(SetStepIconMultiple(v_lFromStepIndex:=lSubLoopFrom, v_lToStepIndex:=lSubLoopTo, v_lStatus:=v_lStatus), gPMConstants.PMEReturnCode)
                    lRecursionCount -= 1
                End If

            Next lIndex

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Set the mouse pointer.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If lRecursionCount < 1000 Then

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetStepIconMultiple the current roadmap", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStepIconMultiple", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Else

                ' Log Recursion Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Recursion Error: count greater than 1000", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStepIconMultiple", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CloseAllSubMapNodes
    '
    ' Description: Closes all Sub Map Nodes in the Tree
    '
    ' ***************************************************************** '
    Public Function CloseAllSubMapNodes() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Note: We go from Node 2 as Node 1 is the Process Node.
            For lIndex As Integer = 2 To treMainData.Nodes.Count
                ' If the Node has Children, it is a Sub Map
                If treMainData.Nodes.Item(lIndex - 1).GetNodeCount(False) < 1 Then
                Else
                    ' Close the Node
                    treMainData.Nodes.Item(lIndex - 1).Collapse()
                End If
            Next lIndex

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Set the mouse pointer.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Log Recursion Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error Closing All Sub Map Nodes", vApp:=ACApp, vClass:=ACClass, vMethod:="CloseAllSubMapNodes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Dim sHeight, sWidth As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Me.WindowState <> FormWindowState.Maximized Then

                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=ACNavRegMainHeight, r_sSettingValue:=sHeight, v_sSubKey:=gPMConstants.ACNavRegSubKey), gPMConstants.PMEReturnCode)

                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=ACNavRegMainWidth, r_sSettingValue:=sWidth, v_sSubKey:=gPMConstants.ACNavRegSubKey), gPMConstants.PMEReturnCode)

                Dim dbNumericTemp As Double
                If Double.TryParse(sHeight, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    If StringsHelper.ToDoubleSafe(sHeight) > 0 Then
                        Me.Height = VB6.TwipsToPixelsY(CDbl(sHeight))
                    End If
                End If

                Dim dbNumericTemp2 As Double
                If Double.TryParse(sWidth, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                    If StringsHelper.ToDoubleSafe(sWidth) > 0 Then
                        Me.Width = VB6.TwipsToPixelsX(CDbl(sWidth))
                    End If
                End If

            End If

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Resize the Column Widths in the Summary Details List View
            lstwSummaryDetails.Columns.Item(0).Width = CInt(lstwSummaryDetails.Width / 2)
            lstwSummaryDetails.Columns.Item(1).Width = CInt(lstwSummaryDetails.Width / 2)

            ' Set any other default values to the interface.
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '    ' BB030298 If Simple Display set then setup a reduced interface display
            '    If AutomaticCloseForm = PMDisplaySimple Then
            '
            '        ' Reduce overall form width
            '        frmInterface.Width = 3825
            '        ' Resize the interface details to match new width
            '        m_lReturn& = ResizeInterfaceDetails()
            '
            '    End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.


            'Developer Guide no: 243
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            'Developer Guide no: 243
            cmdClose.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCloseButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            cmdContinue.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACContinueButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            'Developer Guide no: 243
            cmdRestart.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRestartButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (Begin) *}



            'developer guide no.26
            'Developer Guide no: 243
            ' panProcessTitle.Name = iPMFunc.GetResData(g_iLanguageID, ACProcessTitle, gPMConstants.PMEResourseFileDataType.PMResString)
            panProcessTitlelbl.Text = iPMFunc.GetResData(g_iLanguageID, ACProcessTitle, gPMConstants.PMEResourseFileDataType.PMResString, My.Resources.ResourceManager)



            'Developer Guide no: 243
            'panSummaryTitle.Name = iPMFunc.GetResData(g_iLanguageID, ACSummaryTitle, gPMConstants.PMEResourseFileDataType.PMResString)
            panSummaryTitlelbl.Text = iPMFunc.GetResData(g_iLanguageID, ACSummaryTitle, gPMConstants.PMEResourseFileDataType.PMResString, My.Resources.ResourceManager)

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ResizeInterfaceDetails
    '
    ' Description: Resizes all of the interface details.
    '
    ' ***************************************************************** '
    Private Function ResizeInterfaceDetails() As Integer

        Dim result As Integer = 0
        Dim lRepositionValue As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Resize the interface controls.

            With Me
                lRepositionValue = CInt((VB6.PixelsToTwipsX(.Width) - VB6.PixelsToTwipsX(cmdHelp.Width)) - 150)
                If lRepositionValue > 0 Then
                    cmdHelp.Left = VB6.TwipsToPixelsX(lRepositionValue)
                End If
                lRepositionValue = CInt((VB6.PixelsToTwipsY(.Height) - VB6.PixelsToTwipsY(cmdHelp.Height)) - 1065)
                'If lRepositionValue > 0 Then
                '    cmdHelp.Top = VB6.TwipsToPixelsY(lRepositionValue)
                'End If
                cmdHelp.Top = Me.Height - VB6.TwipsToPixelsY(1150)

                lRepositionValue = CInt((VB6.PixelsToTwipsX(.Width) - VB6.PixelsToTwipsX(cmdClose.Width)) - 1350)
                If lRepositionValue > 0 Then
                    cmdClose.Left = VB6.TwipsToPixelsX(lRepositionValue)
                End If
                cmdClose.Top = cmdHelp.Top

                cmdContinue.Top = cmdHelp.Top
                cmdRestart.Top = cmdHelp.Top

                '        lRepositionValue& = .Height - 2280
                lRepositionValue = CInt(VB6.PixelsToTwipsY(.Height) - 2000)
                If lRepositionValue > 0 Then
                    treMainData.Height = VB6.TwipsToPixelsY(lRepositionValue)
                End If
                lstwSummaryDetails.Height = treMainData.Height


                '        panSliderBar.Height = treMainData.Height + 300
                panSliderBar.Height = treMainData.Height - VB6.TwipsToPixelsY(10)
                '        LinSliderBar.Y2 = treMainData.Height + 300

                'LinSliderBar.Y2 = VB6.PixelsToTwipsY(treMainData.Height) - 10
                LinSliderBar.Height = VB6.PixelsToTwipsY(treMainData.Height) - 10

                lRepositionValue = CInt((VB6.PixelsToTwipsX(.Width) - VB6.PixelsToTwipsX(treMainData.Width)) - 180)
                If lRepositionValue > 0 Then
                    '            treMainSummDetails.Width = lRepositionValue&
                    lstwSummaryDetails.Width = VB6.TwipsToPixelsX(lRepositionValue)
                End If

                ' PH041297 - Remove Right-Hand Pane (BEGIN)
                lRepositionValue = CInt((VB6.PixelsToTwipsX(.Width) - VB6.PixelsToTwipsX(treMainData.Width)) - 180)
                If lRepositionValue > 0 Then
                    ListView1.Width = VB6.TwipsToPixelsX(lRepositionValue)
                End If

                ListView1.Top = treMainData.Top

                ListView1.Height = treMainData.Height - ListView1.Top + treMainData.Top

                ' PH041297 - Remove Right-Hand Pane (END)

                '        panSummaryDetails.Top = _
                ''            (treMainSummDetails.Top + treMainSummDetails.Height) + 165
                '
                '        lstwSummaryDetails.Top = _
                ''            (treMainSummDetails.Top + treMainSummDetails.Height) + _
                ''            (panSummaryDetails.Height + 200)

                lstwSummaryDetails.Height = treMainData.Height - lstwSummaryDetails.Top + treMainData.Top

                ' Resize the Column Widths in the Summary Details List View
                lstwSummaryDetails.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lstwSummaryDetails.Width) / 2 - 50))
                lstwSummaryDetails.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lstwSummaryDetails.Width) / 2 - 50))

                panSummaryTitle.Width = lstwSummaryDetails.Width - VB6.TwipsToPixelsX(10)
                panSummaryDetails.Width = lstwSummaryDetails.Width - VB6.TwipsToPixelsX(10)


                'linMenuLine1.X2 = VB6.PixelsToTwipsX(.Width)
                linMenuLine1.Width = VB6.PixelsToTwipsX(.Width)

                linMenuLine2.Width = VB6.PixelsToTwipsX(.Width)
            End With

            Return result

        Catch



            ' Error Section.


            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: SplashScreen
    '
    ' Description: Displays/Hides the splash screen.
    '
    ' ***************************************************************** '
    Private Function SplashScreen(ByRef bDisplay As Boolean) As Integer

        Dim result As Integer = 0
        Static oPMSplash As Object = Nothing

        Try


            ' We are not currently using the Splash Screen from Navigator
            ' so just exit.
            Return gPMConstants.PMEReturnCode.PMTrue


            ' Check if need to display the splash.
            If bDisplay Then
                ' Display the splash screen.

                ' Get an instance of the splash object.
                If oPMSplash Is Nothing Then
                    ' Get an instance of the object.
                    '            Set oPMSplash = New iPMSplash.Interface

                    ' Set the properties.

                    oPMSplash.CallingAppName = "TEST APP"

                    oPMSplash.TitleName = "Insurance Navigator"


                    m_lReturn = oPMSplash.Start()

                    ' Check for errors.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return m_lReturn
                    End If
                End If
            Else
                ' Destroy the splash screen.

                If Not (oPMSplash Is Nothing) Then

                    m_lReturn = oPMSplash.Finish()

                    oPMSplash = Nothing
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the splash screen", vApp:=ACApp, vClass:=ACClass, vMethod:="SplashScreen", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (End)


    Private Sub cmdContinue_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdContinue.Click

        ' Raise the Continue Event
        RaiseEvent ContinueProcess()

    End Sub

    Private Sub cmdRestart_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRestart.Click

        ' RFC160299 - Data Capture Process Type Added. (Including StartAgain)
        RaiseEvent RestartProcess()

    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try

            iPMFunc.ShowFormInTaskBar_Attach()

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the Form.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            iPMFunc.ShowFormInTaskBar_Detach()

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            Static bActivated As Boolean

            ' Forms load event.

            Try

                ' Check if the interface has already been
                ' activated.
                If bActivated Then
                    Exit Sub
                Else
                    bActivated = True
                End If

                ' Make sure the interface has been displayed.
                Me.Visible = True

                ' Start the process navigation.
                '    m_lReturn& = NavigateProcess()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                End If



                Exit Sub

            Catch excep As System.Exception



                ' Error Section.

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to activate interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Activate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Exit Sub

            End Try
        End If
    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Dim lReturn As gPMConstants.PMEReturnCode

        ' Forms query unload event.

        Try

            'DAK310700 - cannot exit while proces is being driven
            'developer guide no. 131
            If Not Parent_Renamed Is Nothing Then
                If Parent_Renamed.DrivenMode Then
                    Cancel = 1
                    Exit Sub
                End If
            End If
            ' Confirm that the user really wants to Close
            lReturn = ConfirmClose()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Cancel = True
                Exit Sub
            End If

            ' Tell Navigate Control to Close
            RaiseEvent CloseNavigator()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Just incase the splash screen is still displayed.
            m_lReturn = CType(SplashScreen(bDisplay:=False), gPMConstants.PMEReturnCode)

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        ' Forms resize event.

        Try

            ' Resize the interface details.
            m_lReturn = CType(ResizeInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to resize the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Resize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdClose_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClose.Click

        ' Click event of the OK button.

        Try

            ' Unload
            ' Note: This will confirm whether the user really want to
            ' close and act accordingly.
            Me.Close()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdClose_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        ' If the Form is NOT Minimized, save the Height/Width
        If Me.WindowState <> FormWindowState.Minimized Then

            m_lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=ACNavRegMainHeight, v_sSettingValue:=CStr(VB6.PixelsToTwipsY(Me.Height)), v_sSubKey:=gPMConstants.ACNavRegSubKey), gPMConstants.PMEReturnCode)

            m_lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=ACNavRegMainWidth, v_sSettingValue:=CStr(VB6.PixelsToTwipsX(Me.Width)), v_sSubKey:=gPMConstants.ACNavRegSubKey), gPMConstants.PMEReturnCode)

        End If

    End Sub

    Public Sub mnuHelpAbout_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpAbout.Click

        Dim oPMAbout As iPMAbout.Interface_Renamed

        Dim sTitle, sVersionNumber, sVersionDate, sComponent As String
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            ' Set the application title
            sTitle = "Sirius Architecture Tools"
            sComponent = "Navigator"

            ' Set the version number and date
            sVersionNumber = CStr(My.Application.Info.Version.Major) & "." & CStr(My.Application.Info.Version.Minor) & "." & CStr(My.Application.Info.Version.Revision)
            sVersionDate = "08/01/1999"

            ' Create the object
            oPMAbout = New iPMAbout.Interface_Renamed()

            ' Initialise it. No parameters
            'developer guide no. 9
            lReturn = oPMAbout.Initialise()

            ' Display the about screen modally
            lReturn = oPMAbout.Show(sTitle:=sTitle, sVersionNumber:=sVersionNumber, sVersionDate:=sVersionDate, sComponent:=sComponent)

            ' Terminate it, and...
            oPMAbout.Dispose()

            ' ...remove it from memory
            oPMAbout = Nothing

        Catch



            Exit Sub
        End Try


    End Sub

    Public Sub mnuHelpContents_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpContents.Click

        Try



            '	With Me.dlgHelp
            '		
            '		
            '		.HelpCommand = cdlHelpContents
            '
            '				
            '				.HelpContext = 3
            '				
            '				.HelpFile = My.Application.Info.DirectoryPath & ACHelpFileLocation
            '
            '				
            '				
            '				.ShowHelp()
            '
            '				
            '			End With

            PMHelpFunc.ShowHelp(Me, 1)

        Catch



            Exit Sub
        End Try


    End Sub

    Public Sub mnuHelpSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpSearch.Click

        Try



            '    With Me.dlgHelp


            '        .HelpCommand = cdlHelpKey

            '        .HelpContext = 0

            '        .HelpFile = My.Application.Info.DirectoryPath & ACHelpFileLocation



            '        .ShowHelp()

            PMHelpFunc.ShowHelp(Me)
            '    End With

        Catch



            Exit Sub
        End Try


    End Sub

    Private Sub tmrStartProcess_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tmrStartProcess.Tick
        RaiseEvent StartProcess()
    End Sub

    Private Sub treMainData_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles treMainData.DoubleClick

        ' Raise the Event so that Navigator
        ' Control knows to handle it.
        RaiseEvent DoubleClickStep(m_sSelectedNodeKey)

    End Sub

    Private Sub treMainData_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles treMainData.Enter
        VB6.SetDefault(cmdContinue, False)
    End Sub

    Private Sub treMainData_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles treMainData.Leave
        VB6.SetDefault(cmdContinue, True)
    End Sub

    Private Sub treMainData_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles treMainData.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no.70
        Dim X As Single = eventArgs.X
        Dim Y As Single = eventArgs.Y

        Try

            ' Check if we are pressing the left mouse button.
            If Button = MouseButtons.Left Then
                ' Display the banding details using the
                ' selected node details.
                With treMainData.Nodes.Item(m_lSelectedNodeIndex - 1)
                    ' Select the node.

                    'developer guide no. 35
                    .Checked = True
                End With
            End If

        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private Sub panSliderBar_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles panSliderBar.MouseDown
        ' Slider bar mouse down event.

        Try

            If e.Button = MouseButtonConstants.LeftButton And Not m_bSliderMoved Then
                m_bSliderMoved = True
                m_lSliderPosition = 0
                LinSliderBar.BackColor = SystemColors.GrayText
            End If

        Catch



            ' Error Section.

            Exit Sub
        End Try
    End Sub

    Private Sub panSliderBar_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles panSliderBar.MouseMove
        Static lx As Integer

        ' Slider bar mouse move event.

        Try

            If m_bSliderMoved Then
                If e.Button > 0 Then
                    If lx <> e.X Then
                        If VB6.PixelsToTwipsX(panSliderBar.Left) + e.X > 1000 And VB6.PixelsToTwipsX(panSliderBar.Left) + e.X < VB6.PixelsToTwipsX(Me.Width) - 500 Then
                            panSliderBar.Left += VB6.TwipsToPixelsX(e.X)
                            lx = CInt(e.X)
                            m_lSliderPosition = CInt(m_lSliderPosition + e.X)
                        End If
                    End If
                End If
            End If

        Catch



            ' Error Section.

            Exit Sub
        End Try
    End Sub

    Private Sub panSliderBar_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles panSliderBar.MouseUp
        ' Slider bar mouse up event.

        Try

            If m_bSliderMoved Then
                m_bSliderMoved = False

                treMainData.Width += VB6.TwipsToPixelsX(m_lSliderPosition)
                '        treMainSummDetails.Left = treMainSummDetails.Left + m_lSliderPosition&
                lstwSummaryDetails.Left += VB6.TwipsToPixelsX(m_lSliderPosition)
                '        treMainSummDetails.Width = (Me.Width - treMainData.Width) - 180
                lstwSummaryDetails.Width = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(Me.Width) - VB6.PixelsToTwipsX(treMainData.Width)) - 180)

                panProcessTitle.Width = treMainData.Width - VB6.TwipsToPixelsX(15)

                panSummaryTitle.Left = lstwSummaryDetails.Left + VB6.TwipsToPixelsX(15)
                panSummaryTitle.Width = lstwSummaryDetails.Width - VB6.TwipsToPixelsX(30)

                panSummaryDetails.Left = lstwSummaryDetails.Left + VB6.TwipsToPixelsX(15)
                panSummaryDetails.Width = lstwSummaryDetails.Width - VB6.TwipsToPixelsX(30)

                ' PH041297 - Remove Right-Hand Pane (BEGIN)

                ListView1.Left += VB6.TwipsToPixelsX(m_lSliderPosition)
                ListView1.Width = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(Me.Width) - VB6.PixelsToTwipsX(treMainData.Width)) - 180)

                ' PH041297 - Remove Right-Hand Pane (END)

                LinSliderBar.BackColor = SystemColors.ControlLight
            End If

        Catch



            ' Error Section.

            Exit Sub
        End Try
    End Sub

    Private Sub treMainData_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles treMainData.NodeMouseClick
        Dim Node As TreeNode = e.Node

        Try

            ' Store the node index, so the mouse up
            ' event can use it.
            m_lSelectedNodeIndex = Node.Index
            m_sSelectedNodeKey = Node.Name

            ' *********** WARNING, WARNING, WARNING ***********
            '
            ' The following code is a complete BODGE. This must
            ' be changed at a later date if the control
            ' improves.
            '
            ' I've HAD to put the following event call here
            ' because when using the arrow keys, the control
            ' doesn't call any other event except this one.
            ' Therefore I've used the following to display the
            ' details.
            treMainData_MouseUp(treMainData, New MouseEventArgs(MouseButtons.Left, 0, 1, 1, 0))

        Catch



            ' Error Section.

            Exit Sub
        End Try

    End Sub
End Class
