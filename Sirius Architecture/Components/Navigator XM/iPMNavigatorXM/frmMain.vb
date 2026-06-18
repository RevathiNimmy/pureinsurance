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

Imports SharedFiles
Partial Friend Class frmMain
    Inherits System.Windows.Forms.Form

    'Edit History:
    ' RAW 27/02/2003 : ISS2487 : added tests against return value from object's Start function
    ' JRD 13/12/2004 PN17474
    ' Added optional Map property KeepKeysOnRestart, to allow the Navigator to keep
    ' existing Keys within the Key Array if the Restart option is invoked.  Property set
    ' to True by default for backwards compatability.  Functionality required for multi-map
    ' processes which may be required to be restarted that may be dependant on keys passed
    ' from a previous map.
    ' CJB 01/02/2005 PN18409 Change StartMap to enable the close button if a user exits out of a stage.
    ' MKR 07/02/2005 PN18577 Change StartMap to enable the close button whether the user press yes or no
    ' during creation of Work Manager Task

    Private Const ACClass As String = "frmMain"

    Private Const vbFormCode As Integer = 0
    ' Error checker
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Selected step
    Private m_lSelectedStepIndex As Integer

    ' External keys
    Private m_vExtKeyArray(,) As Object

    ' RestartStep
    Private m_lRestartStep As Integer

    ' Errored?
    Private m_bErrored As Boolean

    ' Status
    Private m_lStatus As gPMConstants.PMEReturnCode

    ' Complete or not?
    Private m_bCompleted As Boolean

    Private m_lError As gPMConstants.PMEReturnCode

    ' XMLFileName
    Private m_sXMLFileName As String = ""

    ' RoadmapPath
    Private m_sRoadmapPath As String = ""

    ' AllowErrorRetry
    Private m_lAllowErrorRetry As gPMConstants.PMEReturnCode

    ' SET 27/01/2004
    Private m_lTaskInstanceCnt As Integer

    ' PUBLIC Events (Begin)
    Public Event SetProcessStatus(ByVal v_bProcessComplete As Boolean)
    Public Event NavigatorClose()
    ' PUBLIC Events (End)

    ' ParentClass
    'Private WithEvents m_oParentClass As ClassInterface 'Alkesh
    Private WithEvents m_oParentClass As Interface_Renamed

    'MKR 19/10/2004 PN 13102
    Private m_lClaimID As Integer

    Private m_lSubNodeCounter As Integer = -1

    Private bIsChildNavigatorON As Boolean = False
    Public Property IsChildNavigatorON() As Boolean
        Get
            Return bIsChildNavigatorON
        End Get
        Set(ByVal value As Boolean)
            bIsChildNavigatorON = value
        End Set
    End Property

    '#If APPDEBUG Then
#If DEBUG Then
    ' Form shows non-modally when in debug (for now!?)
    Private m_bEnded As Boolean
    Public Property Ended() As Boolean
        Get
            Return m_bEnded
        End Get
        Set(ByVal Value As Boolean)
            m_bEnded = Value
        End Set
    End Property
    ''''''

#End If

    'Public Property ParentClass() As ClassInterface 'Alkesh
    Public Property ParentClass() As Interface_Renamed
        Get
            Return m_oParentClass
        End Get
        'Set(ByVal Value As ClassInterface) 'Alkesh
        Set(ByVal Value As Interface_Renamed)
            'Alkesh
            'If Microsoft.VisualBasic.Information.IsReference(Value) And Not (TypeOf Value Is String) Then
            '    m_oParentClass = Value
            'Else
            m_oParentClass = Value
            ' End If
        End Set
    End Property

    'UPGRADE_NOTE: (7001) The following declaration (get AllowErrorRetry) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function AllowErrorRetry() As Integer
    'Return m_lAllowErrorRetry

    'UPGRADE_NOTE: (7001) The following declaration (let AllowErrorRetry) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub AllowErrorRetry(ByVal Value As Integer)
    'm_lAllowErrorRetry = Value

    Friend Property RoadmapPath() As String
        Get
            Return m_sRoadmapPath
        End Get
        Set(ByVal Value As String)
            m_sRoadmapPath = Value
        End Set
    End Property

    Friend Property XMLFileName() As String
        Get
            Return m_sXMLFileName
        End Get
        Set(ByVal Value As String)
            m_sXMLFileName = Value
        End Set
    End Property

    Public WriteOnly Property RestartStep() As Integer
        Set(ByVal Value As Integer)
            m_lRestartStep = Value
        End Set
    End Property

    Friend ReadOnly Property Error_Renamed() As Integer
        Get
            Return m_lError
        End Get
    End Property
    'Alkesh
    Public Property Status() As Integer
        Get
            Status = m_lStatus
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

    Public WriteOnly Property TaskInstanceCnt() As Integer
        Set(ByVal Value As Integer)
            m_lTaskInstanceCnt = Value
        End Set
    End Property

    ' ***************************************************************** '
    '
    ' Name: ShowSteps
    '
    ' Description: Prepares the steps in the tree view
    '
    ' History: 21/08/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ShowSteps() As Integer

        Dim result As Integer = 0
        Dim sImage As String = ""
        Dim nodeX As TreeNode
        Dim lIndent, lOldIndent As Integer
        Dim sParent, sKey As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Me.Text = g_sTitle & " - " & m_sRoadmap & " (" & g_oObjectManager.UserName & ")"
            Me.lvwSummary.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(Me.lvwSummary.Width) / 2) - 40))
            Me.lvwSummary.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(Me.lvwSummary.Width) / 2) - 40))

            If m_bNavigatorDriven Then
                stbStatus.Items.Item(ACPanelNavType).Text = "Navigator driven"
            Else
                stbStatus.Items.Item(ACPanelNavType).Text = "User driven"
                'Steve Watton PN 12070, 20/05/2004, Enable the Close button when the
                'roadmap is user driven.
                cmdClose.Enabled = True
            End If

            ' Clear the summary
            lvwSummary.Items.Clear()

            ' Clear the tree
            With Me.tvwTree
                .Nodes.Clear()
            End With

            ' Add the root
            nodeX = tvwTree.Nodes.Add(ACRootNode, m_sRoadmap, ACIconNavigate)
            nodeX.Tag = CStr(0)

            lOldIndent = 0

            ' Add the children
            For iLoop1 As Integer = m_vSteps.GetLowerBound(0) To m_vSteps.GetUpperBound(0)

                Select Case m_vSteps(iLoop1).Type
                    Case gPMConstants.PMNavComponentFindForm
                        sImage = ACIconFindForm
                    Case gPMConstants.PMNavComponentDataForm
                        sImage = ACIconDataForm
                    Case gPMConstants.PMNavComponentDecisionForm
                        sImage = ACIconQuestion
                    Case gPMConstants.PMNavComponentBusinessObject
                        sImage = ACIconBusiness
                    Case PMNavComponentPrintObject
                        sImage = ACIconPrint
                        ' RDC 19062003 for new secondary steps
                    Case gPMConstants.PMNavComponentDiary
                        sImage = ACIconDiary
                    Case gPMConstants.PMNavComponentEditText
                        sImage = ACIconEditText
                    Case gPMConstants.PMNavComponentRaiseEvent
                        sImage = ACIconRaiseEvent
                    Case gPMConstants.PMNavComponentStandardLetter
                        sImage = ACIconStandardLetter
                    Case gPMConstants.PMNavComponentLaunchEXE
                        sImage = ACIconLaunchEXE
                    Case gPMConstants.PMNavComponentUserComponent
                        sImage = ACIconUserComponent
                    Case Else
                        sImage = ACIconDataForm
                End Select

                ' If its a header then override the icon
                If m_vSteps(iLoop1).IsHeader Then
                    sImage = ACIconSubMap
                End If

                sParent = "NODE" & m_vSteps(iLoop1).ParentID

                sKey = "NODE" & m_vSteps(iLoop1).StepID

                nodeX = tvwTree.Nodes.Find(sParent, True)(0).Nodes.Add(sKey, m_vSteps(iLoop1).Description, sImage, sImage)

                ' Set the tag to the index of the step array
                nodeX.Tag = CStr(iLoop1)

                lOldIndent = lIndent

            Next iLoop1

            ' Expand the root
            tvwTree.Nodes.Item(ACRootNode).Expand()

            ' Start step
            If bIsChildNavigatorON Then
                m_lCurrentStepTemp = m_lCurrentStep
            End If

            iPMNavigatorXM.m_lCurrentStep = 0

            Return result

        Catch excep As System.Exception

            ''Debugger.Break()

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowSteps Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowSteps", excep:=excep)

            Return result
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateWorkManagerTask
    '
    ' Description:
    '
    ' History: 29/08/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function CreateWorkManagerTask() As Integer
        Dim result As Integer = 0

        Dim oBusiness As bPMNavigatorXM.Business
        Dim vTempArray(,) As Object
        Dim lStep As Integer
        Dim vTemporaryWMTaskCntArray As Object
        Dim sPartyName As String = ""
        Dim sTaskDescription As String = ""
        Dim lUserGroupId, lUserId As Integer

        Try

            ' Set the status to complete (so it clears the old one down)
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store the restart step in the key array
            ReDim vTempArray(1, 0)
            vTempArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameCurrentNashStep

            ' Where to restart?
            lStep = m_vSteps(m_lCurrentStep).ResumeStep
            If lStep = ACResumeStepCurrent Then

                vTempArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lCurrentStep
            Else

                vTempArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = lStep
            End If
            m_lReturn = CType(MergeKeyArray(v_vNewKeyArray:=vTempArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Remove our temp array
            vTempArray = Nothing

            ' Create the business object
            Dim temp_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bPMNavigatorXM.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBusiness = temp_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' SET 27/01/2004 - extra bits required to display the WM interface
            If m_vSteps(iPMNavigatorXM.m_lCurrentStep).ShowWMTaskInterface Then
                ' find name of customer

                For iLoop As Integer = m_vKeyArray.GetLowerBound(1) To m_vKeyArray.GetUpperBound(1)

                    If CStr(m_vKeyArray(0, iLoop)) = PMNavKeyConst.PMKeyNameClientName Then

                        sPartyName = CStr(m_vKeyArray(1, iLoop))
                        Exit For
                    End If
                Next iLoop
                'FSA Phase III

                For iLoop As Integer = m_vKeyArray.GetLowerBound(1) To m_vKeyArray.GetUpperBound(1)

                    If CStr(m_vKeyArray(0, iLoop)) = PMNavKeyConst.PMKeyNameTaskCustomer Then

                        sPartyName = CStr(m_vKeyArray(1, iLoop))
                        Exit For
                    End If
                Next iLoop

                'call  DisplayMultipleTaskInstanceDisplayForm to allow user
                'to choose User Group and User for new work manager task

                m_lReturn = CType(DisplayMultipleTaskInstanceDisplayForm(r_vPMWrkTaskInstanceCntArray:=CStr(vTemporaryWMTaskCntArray), r_lUserGroupID:=lUserGroupId, r_lUserID:=lUserId, v_sWMDescription:=g_sWMTaskDescription & " (" & m_vSteps(iPMNavigatorXM.m_lCurrentStep).Description & ")", v_sPartyName:=sPartyName), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create DisplayMultipleTaskInstanceDisplayForm.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWorkManagerTask", excep:=New Exception(Information.Err().Description))
                    Return result
                End If

                If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    result = gPMConstants.PMEReturnCode.PMCancel
                    m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                Else
                    ' Add the task and keys

                    m_lReturn = oBusiness.CreateWorkTask(v_sTaskCode:=g_sWMTaskCode, v_sDescription:=g_sWMTaskDescription & " (" & m_vSteps(m_lCurrentStep).Description & ")", v_lUserGroupID:=lUserGroupId, v_lUserID:=lUserId, r_vKeyArray:=m_vKeyArray)
                End If
            Else
                ' Add the task and keys

                m_lReturn = oBusiness.CreateWorkTask(v_sTaskCode:=g_sWMTaskCode, v_sDescription:=g_sWMTaskDescription & " (" & m_vSteps(m_lCurrentStep).Description & ")", r_vKeyArray:=m_vKeyArray)

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Clear the business object
                oBusiness = Nothing
                ' Log Error Message
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create a work manager task.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateWorkManagerTask", excep:=New Exception(Information.Err().Description))
                Return result
            End If

            ' Terminate the business object

            oBusiness.Dispose()
            ' Remove the business object
            oBusiness = Nothing

            Return result

        Catch excep As System.Exception
            'Debugger.Break()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateWorkManagerTask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateWorkManagerTask", excep:=excep)

            Return result
            Return result
        End Try
    End Function
    Private Function DisplayMultipleTaskInstanceDisplayForm(ByRef r_vPMWrkTaskInstanceCntArray As Object, ByRef r_lUserGroupID As Integer, ByRef r_lUserID As Integer, ByVal v_sWMDescription As String, ByVal v_sPartyName As String) As Integer
        Dim result As Integer = 0
        'Dim iPMWrkTaskInstanceDisplay As Object
        Dim oTaskInstance As iPMWrkTaskInstanceDisplay.Interface_Renamed
        Dim lReturn As Integer
        Dim dtDueDate As Date
        Dim sValue As String = ""
        Dim iTaskDays As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            Select Case v_sWMDescription
                Case "Complete New Business (Add a policy)"
                    m_lReturn = CType(iPMFunc.GetSystemOption(v_iOptionNumber:=19, r_sOptionValue:=sValue), gPMConstants.PMEReturnCode)

                Case "Complete New Business (Raise debit)"
                    m_lReturn = CType(iPMFunc.GetSystemOption(v_iOptionNumber:=21, r_sOptionValue:=sValue), gPMConstants.PMEReturnCode)

                Case "Complete New Business (Cash List)"
                    m_lReturn = CType(iPMFunc.GetSystemOption(v_iOptionNumber:=23, r_sOptionValue:=sValue), gPMConstants.PMEReturnCode)

                Case "Complete New Business (Cash List Item)"
                    m_lReturn = CType(iPMFunc.GetSystemOption(v_iOptionNumber:=23, r_sOptionValue:=sValue), gPMConstants.PMEReturnCode)
            End Select

            iTaskDays = gPMFunctions.ToSafeInteger(sValue)

            'calculate due date
            If iTaskDays <> 0 Then
                dtDueDate = DateTime.Today.AddDays(iTaskDays)
            Else
                dtDueDate = DateTime.Today.AddDays(ACDefaultTaskDays)
            End If

            ' Create the Component
            Dim temp_oTaskInstance As Object
            lReturn = g_oObjectManager.GetInstance(temp_oTaskInstance, sClassName:="iPMWrkTaskInstanceDisplay.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oTaskInstance = temp_oTaskInstance
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Process Modes

            lReturn = oTaskInstance.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired, vEffectiveDate:=DateTime.Now)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Pass the MultipleTaskInstanceDisplayForm relevant values

            oTaskInstance.Customer = v_sPartyName

            oTaskInstance.DueDate = dtDueDate

            oTaskInstance.Description = v_sWMDescription

            oTaskInstance.PMWrkTaskInstanceCnt = m_lTaskInstanceCnt

            oTaskInstance.TaskDescription = g_sWMTaskDescription

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Start the Form

            lReturn = oTaskInstance.Start
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("r_lUserGroupID", r_lUserGroupID)
                oDict.Add("r_lUserID", r_lUserID)
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Start Task Instance Display Form:- iPMWrkTaskInstanceDisplay.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="StartStep", excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'get the results (UserGroupID & UserID) from the multiple task instance display form.

            r_lUserGroupID = oTaskInstance.UserGroupID

            r_lUserID = oTaskInstance.UserID

            ' If the User Canceled then exit as we do not need
            ' to Refresh the Form details.

            If oTaskInstance.Status = gPMConstants.PMEReturnCode.PMCancel Then
                result = gPMConstants.PMEReturnCode.PMCancel
                r_vPMWrkTaskInstanceCntArray = ""

                oTaskInstance.Dispose()
                oTaskInstance = Nothing
                Return result
            End If
            oTaskInstance.Dispose()
            oTaskInstance = Nothing

            Return result

        Catch excep As System.Exception
            'Debugger.Break()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("r_lUserGroupID", r_lUserGroupID)
            oDict.Add("r_lUserID", r_lUserID)
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayMultipleTaskInstanceDisplayForm", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayMultipleTaskInstanceDisplayForm", excep:=excep, oDicParms:=oDict)
            Return result

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CloseDown
    '
    ' Description:
    '
    ' History: 29/08/2001 CTAF - Created.
    '
    '          07/04/2003 Kevin Renshaw (CMG) Task not being created as the status of
    '                     the task has already been changed when the roadmap ends. Code
    '                     moved to startmap
    '          09/04/2003 Kevin Renshaw (CMG) Full Closedown if autoclose selected but
    '                     donot ask closedown question PN3487
    ' ***************************************************************** '

    Private Function CloseDown(Optional ByRef r_vCancel As Byte = 0, Optional ByRef r_bAutoClose As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Stop the timer!
            tmrStart.Enabled = False

            If (Not r_bAutoClose) Then
                ' Prompt to exit or not?
                If MessageBox.Show("Are you sure you wish to close " & m_sRoadmap & "?", "Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                    ' Check if we need to pass back iCancel to QueryUnload

                    If Not Information.IsNothing(r_vCancel) Then
                        r_vCancel = 1
                    End If
                    Return result
                Else
                    m_vSteps = Nothing
                    m_vKeyArray_Dup = m_vKeyArray
                    m_vKeyArray = Nothing
                End If

            End If

            '#If APPDEBUG Then  'Alkesh
#If DEBUG Then

            m_bEnded = True
#End If
            ' Delete object manager
            If Not (g_oObjectManager Is Nothing) And Not bIsChildNavigatorON Then
                ' Terminate it
                g_oObjectManager.Dispose()

                ' Clear up
                g_oObjectManager = Nothing

            End If

            ' Done - Hide
            If bIsChildNavigatorON Then
                m_vSteps = m_vStepsTemp
                m_vStepsTemp = Nothing
                m_vKeyArray = m_vKeyArrayTemp
                m_vKeyArrayTemp = Nothing
                m_lCurrentStep = m_lCurrentStepTemp
                m_lCurrentStepTemp = -1
                m_bEndMap = False
                m_bAutoClose = m_bAutoCloseTemp
                m_sRoadmap = m_sRoadmapTemp
                m_bAutoClose = m_bAutoCloseTemp
                g_vTransactionType = g_vTransactionTypeTemp
                g_vProcessMode = g_vProcessModeTemp
                m_sImageURL = m_sImageURLTemp
                g_sWMTaskCode = g_sWMTaskCodeTemp
                g_sWMTaskDescription = g_sWMTaskDescriptionTemp
                g_sTitle = g_sTitleTemp
                If Not m_vSteps Is Nothing Then
                    g_lNumberOfSteps = m_vSteps.GetUpperBound(0)
                End If

            End If
            RemoveHandler MyBase.FormClosing, AddressOf frmMain_FormClosing
            Me.Hide()

            '#If APPDEBUG Then               ' Alkesh
#If DEBUG Then

            m_bEnded = True
#End If

            'Always set the status of the task to completed. The user would have raised a new task if they wanted to save the task for later use.
            m_oParentClass.SetProcessStatus_Renamed(v_bProcessComplete:=True)

            ' Raise the event to the calling class
            m_oParentClass.NavigatorClose_Renamed()

            Return result

        Catch excep As System.Exception
            'Debugger.Break()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CloseDown Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CloseDown", excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdClose_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClose.Click

        m_lReturn = CType(CloseDown(), gPMConstants.PMEReturnCode)

    End Sub

    ' ***************************************************************** '
    '
    ' Name: GetRegSettings
    '
    ' Description:
    '
    ' History: 11/10/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetRegSettings() As Integer

        Dim result As Integer = 0
        Dim sAllowErrorRetry As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' CTAF - I stole this code from iPMNavigatorV3. HAHAHA
            ' Get Retry setting
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

            ' CTAF - I meant, I'm reusing this code from NavigatorV3...

            Return result

        Catch excep As System.Exception
            'Debugger.Break()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRegSettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegSettings", excep:=excep)
            Return result

        End Try
    End Function
    Private Sub Form_Initialize_Renamed()

        Try

            ' No errors yet...
            m_lError = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise Object Manager

            MainModule.g_oObjectManager = New bObjectManager.ObjectManager()
            m_lReturn = MainModule.g_oObjectManager.Initialise(MainModule.ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lError = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Get the registry settings
            m_lReturn = CType(GetRegSettings(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lError = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

        Catch

            'Debugger.Break()

            Exit Sub
        End Try
    End Sub

    ' ***************************************************************** '
    '
    ' Name: ConfigureBitmap
    '
    ' Description:
    '
    ' History: 01/05/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ConfigureBitmap() As Integer

        Dim result As Integer = 0
        Dim sLogoPath As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=ACNavigatorXMLogo, r_sSettingValue:=sLogoPath), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Just exit and keep the sirius logo
            If sLogoPath = "" Then
                Return result
            End If

            ' Try and load the new one

            picLogo.Image = Image.FromFile(sLogoPath)

            Return result

        Catch excep As System.Exception

            'Debugger.Break()

            result = gPMConstants.PMEReturnCode.PMError
            Select Case Information.Err().Number
                Case 53 ' File not found
                    ' nothing. just exit and use the sirius logo. its hardly
                    ' mission critical to get the logo on there!

                Case Else

                    ' Log Error Message
                    gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConfigureBitmap Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConfigureBitmap", excep:=excep)

            End Select

            Return result
            Return result
        End Try
    End Function

    Private Sub frmMain_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        ' Magic keystroke to get the mystical debug information
        ' Or u can double click on the status...
        If ((Shift And ShiftConstants.CtrlMask) > 0) And (eventArgs.KeyCode = Keys.F12) Then

            ' Show debug
            m_lReturn = CType(ShowDebug(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' who cares
            End If

        End If

    End Sub
    Private Sub frmMain_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Set the mouse cursor
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        ' Set the status
        stbStatus.Items.Item(ACPanelStatus).Text = "Preparing. Please wait..."

        ' Save the external keys

        m_vExtKeyArray = m_vKeyArray

        m_lReturn = CType(ConfigureBitmap(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' We wont worry about it yet - just default to Sirius logo
        End If

        ' Are we allowed to retry?
        If m_lAllowErrorRetry = gPMConstants.PMEReturnCode.PMFalse Then
            mnuProcessRetry.Available = False
        End If

        ' This reads from the path :
        ' HKEY_LOCAL_MACHINE\SOFTWARE\PM\SiriusArchitecture\Common
        ' NavigatorXMPath
        ' Setup the roadmap
        If bIsChildNavigatorON Then
            m_vStepsTemp = m_vSteps
            m_vSteps = Nothing
        End If
        m_lReturn = CType(LoadMap(v_sXMLFileName:=m_sXMLFileName, r_sRoadmapPath:=m_sRoadmapPath), gPMConstants.PMEReturnCode)
        If Not bIsChildNavigatorON Then
            m_bAutoCloseTemp = m_bAutoClose
            m_sRoadmapTemp = m_sRoadmap
            m_bAutoCloseTemp = m_bAutoClose
            g_vTransactionTypeTemp = g_vTransactionType
            g_vProcessModeTemp = g_vProcessMode
            m_sImageURLTemp = m_sImageURL
            g_sWMTaskCodeTemp = g_sWMTaskCode
            g_sWMTaskDescriptionTemp = g_sWMTaskDescription
            g_sTitleTemp = g_sTitle
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lError = gPMConstants.PMEReturnCode.PMFalse
            Exit Sub
        End If

        ' Prep the tree view with the steps
        m_lReturn = CType(ShowSteps(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lError = gPMConstants.PMEReturnCode.PMFalse
            Exit Sub
        End If

        ' Start the roadmap off in 1/2 a second
        tmrStart.Enabled = True
        tmrStart.Interval = 500
        tmrStart.Enabled = True

        stbStatus.Items.Item(ACPanelStatus).Text = "Ready."

        ' Set the mouse cursor
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    End Sub

    ' ***************************************************************** '
    '
    ' Name: StartMap
    '
    ' Description: Does the biz
    '
    ' History: 21/08/2001 CTAF - Created.
    '
    '          08/04/2003 Kevin Renshaw (CMG) Create Task when Roadmap finishes
    '          09/04/2003 Kevin Renshaw (CMG) Full Closedown if autoclose selected PN3487
    ' ***************************************************************** '
    Private Function StartMap() As Integer

        Dim result As Integer = 0
        Dim bCompleted, bCancelled As Boolean
        Dim iCounter As Integer
        Dim lOldStep As Integer
        Dim sNewRoadmap As String = ""
        Dim v_ofrmDebug As New frmDebug
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            m_bEndMap = False

            ' Set the step
            ' If no restart step was passed in then this will be 0
            ' which is cool anyway
            iPMNavigatorXM.m_lCurrentStep = m_lRestartStep

            ' This counter tracks the number of steps so far. So even if
            ' the last step in the map says Forward One, if theres no step
            ' then it wont go forward one and therefore won't error.
            iCounter = 0
            m_lSubNodeCounter = -1
            '#If APPDEBUG Then
            'Alkesh commented code needs to transfer at other place.
            '#If DEBUG Then

            '            frmDebug.XMLFileName = m_sXMLFileName
            '            frmDebug.RoadmapPath = m_sRoadmapPath
            '            'm_lReturn& = frmDebug.RefreshDebug()   'Ankit Jain
            '            m_lReturn = frmDebug.RefreshDebug()
            '            ' Dont care if it fails
            '#End If

            ' Loop while we have something to do
            While (Not m_bEndMap)

                bCompleted = False
                bCancelled = False

                lOldStep = IIf(m_lSubNodeCounter = -1, iPMNavigatorXM.m_lCurrentStep, m_lCurrentStep + m_lSubNodeCounter)

                m_bErrored = False
                sNewRoadmap = ""

                ' Process the next step
                m_lReturn = CType(ProcessCurrentStep(r_bCompleted:=m_bCompleted, r_bCancelled:=bCancelled, r_sNewRoadMap:=sNewRoadmap), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_bEndMap = True
                    'm_bErrored = True
                    If bCancelled Then
                        stbStatus.Items.Item(ACPanelStatus).Text = "User cancelled."

                        ' Checking quote status.
                        If Information.IsArray(m_vKeyArray) Then

                            For iLoop As Integer = m_vKeyArray.GetLowerBound(1) To m_vKeyArray.GetUpperBound(1)

                                If CStr(m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop)) = "quote_status" Then

                                    If CStr(m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop)) <> "" Then
                                        stbStatus.Items.Item(ACPanelStatus).Text = CStr(m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop))
                                    End If
                                    Exit For
                                End If
                            Next iLoop
                        End If
                    Else
                        stbStatus.Items.Item(ACPanelStatus).Text = "Error. ProcessCurrentStep failed."
                        Status = gPMConstants.PMEReturnCode.PMError
                    End If

                Else
                    'Check if we have Sub Nodes
                    If tvwTree.Nodes.Item(0).Nodes.Item(m_lCurrentStep).GetNodeCount(True) > 0 Then
                        If tvwTree.Nodes.Item(0).Nodes.Item(m_lCurrentStep).Nodes.Count = m_lSubNodeCounter + 1 And m_bCompleted Then
                            'If there are no more subnodes left then Reset m_lSubNodeCounter
                            m_lCurrentStep += m_lSubNodeCounter + 1
                            iCounter = m_lCurrentStep
                            m_lSubNodeCounter = -1
                        Else
                            m_lSubNodeCounter = IIf(m_lSubNodeCounter = -1, 0, m_lSubNodeCounter)
                        End If
                    End If
                    'Alkesh : Added to handle global
                    If m_vSteps(lOldStep).CancelAction <> gPMConstants.PMNavActionForwardX Then
                        If m_lSubNodeCounter = -1 Then
                            If m_lCurrentStep >= lOldStep Then
                                m_lCurrentStep = lOldStep + 1
                            End If
                        End If
                    End If
                    ' Count how many steps we've done
                    If m_lSubNodeCounter = -1 Then
                        iCounter += (m_lCurrentStep - lOldStep)
                    End If

                    If iCounter > g_lNumberOfSteps Or m_bEndMap Then
                        bCompleted = True
                        m_bEndMap = True
                        'Alkesh: Added to handle the global varriable for testing
                        m_bCompleted = True
                    Else
                        If bCancelled = False Then
                            m_bCompleted = False
                        End If
                    End If

                End If

                If m_bCompleted Then
                    ' Set the status bar
                    stbStatus.Items.Item(ACPanelStatus).Text = "Complete."
                End If

                'Alkesh commented code needs to transfer at other place.
                '#If DEBUG Then

                '                'm_lReturn& = frmDebug.RefreshDebug() 'Ankit Jain
                '                m_lReturn = frmDebug.RefreshDebug()
                '                ' Dont care if it fails
                '#End If
            End While

            ' ---------------------------------------------------------------
            ' RFC160603 - Add support for Starting a new Roadmap
            If sNewRoadmap <> "" Then

                ' Reset any Steps from the current roadmap
                ReDim m_vSteps(0)

                ' Setup the roadmap
                m_lReturn = CType(LoadMap(v_sXMLFileName:=sNewRoadmap, r_sRoadmapPath:=m_sRoadmapPath), gPMConstants.PMEReturnCode)
                If Not bIsChildNavigatorON Then
                    m_bAutoCloseTemp = m_bAutoClose
                    m_sRoadmapTemp = m_sRoadmap
                    m_bAutoCloseTemp = m_bAutoClose
                    g_vTransactionTypeTemp = g_vTransactionType
                    g_vProcessModeTemp = g_vProcessMode
                    m_sImageURLTemp = m_sImageURL
                    g_sWMTaskCodeTemp = g_sWMTaskCode
                    g_sWMTaskDescriptionTemp = g_sWMTaskDescription
                    g_sTitleTemp = g_sTitle
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lError = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If

                ' Prep the tree view with the steps
                m_lReturn = CType(ShowSteps(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lError = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If

                ' Start the roadmap off in 1/2 a second
                tmrStart.Enabled = True
                tmrStart.Interval = 500
                tmrStart.Enabled = True

                stbStatus.Items.Item(ACPanelStatus).Text = "Ready."
                Return result

            End If
            ' ---------------------------------------------------------------

            ' Save state if we havent finished
            If Not m_bCompleted Then

                ' We dont want to create one if everything worked
                If Not m_bErrored Then
                    ' Do we want to create a work manager task?
                    If m_vSteps(iPMNavigatorXM.m_lCurrentStep).CreateWorkManagerTask Then

                        ' Prompt to create work manager task
                        If MessageBox.Show("Do you wish to create a WorkManager task so that you may resume later?", "Task", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then

                            'DJM 20/02/2004 PN10255 : Only create a work manager task if not testing a new navigator.
                            If IsFromEditor() = gPMConstants.PMEReturnCode.PMFalse Then

                                ' Set to hour glass
                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                                ' Set the status bar
                                stbStatus.Items.Item(0).Text = "Creating WorkManager task. Please wait..."
                                m_lReturn = CType(CreateWorkManagerTask(), gPMConstants.PMEReturnCode)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                End If

                                If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                                    stbStatus.Items.Item(0).Text = "WorkManager task not created"
                                Else
                                    stbStatus.Items.Item(0).Text = "WorkManager task created"
                                End If

                                ' Reset the mouse pointer
                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                            Else
                                MessageBox.Show("Work Manager task would be created at this point during live run.", "Task", MessageBoxButtons.OK)
                            End If

                        End If

                        ' PN18577 Enable the Close button  whether user presses yes or no in question...
                        cmdClose.Enabled = True

                    End If

                End If

            End If

            If m_bCompleted Then
                'DC260404 PN11713 allow close button to be used
                cmdClose.Enabled = True
                tvwTree.Nodes.Item(ACRootNode).Checked = True
            End If

            ' Automatically close the map?
            If m_bAutoClose Then
                ' but only if the map worked
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    CloseDown(r_bAutoClose:=True)
                End If
            End If

            If Not bIsChildNavigatorON Then
                m_lCurrentStep = m_lRestartStep
            End If
            Return result

        Catch excep As System.Exception
            'Debugger.Break()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartMap Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StartMap", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: MergeKeyArray
    '
    ' Description:
    '
    ' History: 22/08/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function MergeKeyArray(ByVal v_vNewKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim bMatch As Boolean
        Dim iIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Don't merge if we dont have any keys
            If Not Information.IsArray(m_vKeyArray) Then
                'Alkesh
                m_vKeyArray = VB6.CopyArray(v_vNewKeyArray)
                'm_vKeyArray = CopyArray(v_vNewKeyArray)
                Return result
            End If

            ' Dont do anything if we have no keys
            If Not Information.IsArray(v_vNewKeyArray) Then
                Return result
            End If

            For iLoop1 As Integer = v_vNewKeyArray.GetLowerBound(1) To v_vNewKeyArray.GetUpperBound(1)

                bMatch = False

                ' Check if it's in the master array

                For iLoop2 As Integer = m_vKeyArray.GetLowerBound(1) To m_vKeyArray.GetUpperBound(1)
                    If Not m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop2) Is Nothing Then
                        If m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop2).Equals(v_vNewKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)) Then
                            ' Update the value
                            ' CTAF 071101 - Pass around objects
                            If Information.IsReference(v_vNewKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)) Then
                                m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop2) = v_vNewKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)
                            Else
                                m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop2) = v_vNewKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)
                            End If

                            bMatch = True
                            Exit For
                        End If
                    End If
                Next iLoop2

                ' Not found so add it
                If Not bMatch Then
                    iIndex = m_vKeyArray.GetUpperBound(1) + 1
                    ReDim Preserve m_vKeyArray(1, iIndex)

                    m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iIndex) = v_vNewKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)

                    ' CTAF 071101 - Pass around objects
                    If Information.IsReference(v_vNewKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)) Then
                        m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iIndex) = v_vNewKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)
                    Else
                        m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iIndex) = v_vNewKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)
                    End If

                End If

            Next iLoop1

            Return result

        Catch excep As System.Exception
            'Debugger.Break()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MergeKeyArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeKeyArray", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessSummary
    '
    ' Description: Updates the summary list
    '
    ' History: 22/08/2001 CTAF - Created.
    '
    ' ***************************************************************** '

    Private Function ProcessSummary(ByVal v_vSummaryArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim bMatch As Boolean
        Dim lstX As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Don't do anything if we dont have summary
            If Not Information.IsArray(v_vSummaryArray) Then
                Return result
            End If

            ' Go round all the entries
            For iLoop1 As Integer = v_vSummaryArray.GetLowerBound(1) To v_vSummaryArray.GetUpperBound(1)

                bMatch = False

                ' Check if it exists
                If lvwSummary.Items.Count > 0 Then

                    For iLoop2 As Integer = 1 To lvwSummary.Items.Count

                        ' Update it
                        If Convert.ToString(v_vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, iLoop1)) = lvwSummary.Items.Item(iLoop2 - 1).Text Then
                            bMatch = True

                            ListViewHelper.GetListViewSubItem(lvwSummary.Items.Item(iLoop2 - 1), 1).Text = CStr(v_vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, iLoop1))
                            Exit For
                        End If

                    Next iLoop2

                End If

                ' New one
                If Not bMatch Then
                    ' Check we have something to show
                    If Convert.ToString(v_vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, iLoop1)).Trim() <> "" Then
                        ' Show it

                        lstX = lvwSummary.Items.Add("S" & lvwSummary.Items.Count + 1, CStr(v_vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, iLoop1)), "")

                        ListViewHelper.GetListViewSubItem(lstX, 1).Text = CStr(v_vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, iLoop1))
                    End If
                End If

            Next iLoop1

            Return result

        Catch excep As System.Exception
            'Debugger.Break()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSummary", excep:=excep)

            Return result
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetKeys
    '
    ' Description: Sets the keys
    '
    ' History: 29/08/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByVal vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the external keys
            If bIsChildNavigatorON Then
                m_vKeyArrayTemp = m_vKeyArray
            End If
            m_vKeyArray = vKeyArray

            Return result

        Catch excep As System.Exception
            'Debugger.Break()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessCurrentStep
    '
    ' Description:
    '
    ' History: 21/08/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessCurrentStep(ByRef r_bCompleted As Boolean, ByRef r_bCancelled As Boolean, ByRef r_sNewRoadMap As String) As Integer

        Dim result As Integer = 0
        Dim sComponentName, sType As String
        Dim bNav3, bNav2 As Boolean
        Dim iStatus As gPMConstants.PMEReturnCode
        Dim sCommand As String = ""
        Dim oObject As Object
        Dim vKeyArray(,) As Object
        Dim vSummaryArray As Object
        Dim lSteps As Integer
        Dim bServerSide As Boolean
        Dim oNode As TreeNode

        result = gPMConstants.PMEReturnCode.PMTrue
        ' Default to not completed
        r_bCompleted = False

        ' Set the status
        stbStatus.Items.Item(ACPanelStatus).Text = "Preparing step. Please wait..."

        ' Highlight the step
        'tvwTree.SelectedNode = tvwTree.Nodes.Item(m_lCurrentStep + 1)
        'tvwTree.SelectedNode = tvwTree.Nodes.Item(m_lCurrentStep)
        If tvwTree.Nodes.Item(0).Nodes.Item(iPMNavigatorXM.m_lCurrentStep).Nodes.Count > 0 Then
            tvwTree.SelectedNode = tvwTree.Nodes.Item(0).Nodes.Item(iPMNavigatorXM.m_lCurrentStep).Nodes.Item(m_lSubNodeCounter)
        Else
            tvwTree.SelectedNode = tvwTree.Nodes.Item(0).Nodes.Item(iPMNavigatorXM.m_lCurrentStep)
        End If
        ' CTAF 120702 Is it just a header?
        If m_vSteps(iPMNavigatorXM.m_lCurrentStep).IsHeader Then
            ' Yes,  move to the next step
            If m_lSubNodeCounter = -1 Then
                iPMNavigatorXM.m_lCurrentStep += 1
                Return result
            End If
        End If

        ' Get the component name and type
        If m_lSubNodeCounter = -1 Then
            sComponentName = m_vSteps(iPMNavigatorXM.m_lCurrentStep).Component
        ElseIf m_lSubNodeCounter = 0 Then
            sComponentName = m_vSteps(m_lCurrentStep + 1).Component
        Else
            sComponentName = m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).Component
        End If

        If m_lSubNodeCounter = -1 Then
            sType = m_vSteps(iPMNavigatorXM.m_lCurrentStep).Type
        ElseIf m_lSubNodeCounter = 0 Then
            sType = m_vSteps(m_lCurrentStep + 1).Type
        Else
            sType = m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).Type
        End If
        ' Set the mouse cursor
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        ' Get the locality of the object
        If m_lSubNodeCounter = -1 Then
            bServerSide = m_vSteps(iPMNavigatorXM.m_lCurrentStep).ServerSide
        ElseIf m_lSubNodeCounter = 0 Then
            sType = m_vSteps(m_lCurrentStep + 1).Type
        Else
            bServerSide = m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).ServerSide
        End If

        If bServerSide Then

            ' Get an instance of the object

            m_lReturn = g_oObjectManager.GetInstance(oObject:=oObject, sClassName:=sComponentName, vInstanceManager:=gPMConstants.PMGetViaClientManager)

        Else

            ' Get an instance of the object

            m_lReturn = g_oObjectManager.GetInstance(oObject:=oObject, sClassName:=sComponentName, vInstanceManager:=gPMConstants.PMGetLocalInterface)

        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of " & sComponentName, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCurrentStep", excep:=New Exception(Information.Err().Description))
            ' Clear up
            oObject = Nothing
            ' RAW 27/02/2003 : ISS2487 : added
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            ' RAW 27/02/2003 : ISS2487 : end
            Return result
        End If

        ' Check if its Nav2 or Nav3

        m_lReturn = CType(CheckNav2or3(v_oObject:=oObject, r_bNav2:=bNav2, r_bNav3:=bNav3), gPMConstants.PMEReturnCode)

        ' Set the mouse cursor
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            oObject = Nothing
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'TF260202 - Ensure NB TransactionType is passed in.
        If bNav3 Then
            ' Set its process modes

            'Alkesh Commented 
            'oObject = New iPMBFindPartyWrapper.NavigatorV3
            If m_lSubNodeCounter = -1 Then
                m_lReturn = oObject.NavigatorV3_SetProcessModes(vTask:=m_vSteps(iPMNavigatorXM.m_lCurrentStep).ComponentAction, vTransactionType:=g_vTransactionType, vProcessMode:=g_vProcessMode)
            ElseIf m_lSubNodeCounter = 0 Then
                m_lReturn = oObject.NavigatorV3_SetProcessModes(vTask:=m_vSteps(m_lCurrentStep + 1).ComponentAction, vTransactionType:=g_vTransactionType, vProcessMode:=g_vProcessMode)
            Else
                m_lReturn = oObject.NavigatorV3_SetProcessModes(vTask:=m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).ComponentAction, vTransactionType:=g_vTransactionType, vProcessMode:=g_vProcessMode)
            End If
        Else
            If m_lSubNodeCounter = -1 Then
                m_lReturn = oObject.SetProcessModes(vTask:=m_vSteps(iPMNavigatorXM.m_lCurrentStep).ComponentAction, vTransactionType:=g_vTransactionType, vProcessMode:=g_vProcessMode)
            ElseIf m_lSubNodeCounter = 0 Then
                m_lReturn = oObject.SetProcessModes(vTask:=m_vSteps(m_lCurrentStep + 1).ComponentAction, vTransactionType:=g_vTransactionType, vProcessMode:=g_vProcessMode)
            Else
                m_lReturn = oObject.SetProcessModes(vTask:=m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).ComponentAction, vTransactionType:=g_vTransactionType, vProcessMode:=g_vProcessMode)
            End If
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Clear up
            oObject = Nothing
            Return result
        End If

        ' Set the default step keys
        If m_lSubNodeCounter = -1 Then
            If Information.IsArray(m_vSteps(iPMNavigatorXM.m_lCurrentStep).DefaultKeys) Then

                m_lReturn = MergeKeyArray(m_vSteps(iPMNavigatorXM.m_lCurrentStep).DefaultKeys)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        ElseIf m_lSubNodeCounter = 0 Then
            If Information.IsArray(m_vSteps(m_lCurrentStep + 1).DefaultKeys) Then
                m_lReturn = MergeKeyArray(m_vSteps(m_lCurrentStep + 1).DefaultKeys)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        Else
            If Information.IsArray(m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).DefaultKeys) Then
                m_lReturn = MergeKeyArray(m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).DefaultKeys)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        ' CTAF 120202
        ' Pass in the current step as a key - every time
        ReDim vKeyArray(1, 0)

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameCurrentNavXMStep

        If m_lSubNodeCounter = -1 Then
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lCurrentStep
        ElseIf m_lSubNodeCounter = 0 Then
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lCurrentStep + 1
        Else
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lCurrentStep + m_lSubNodeCounter + 1
        End If

        If IsFromEditor() Then

            ReDim vKeyArray(1, vKeyArray.GetUpperBound(1) + 1)
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, vKeyArray.GetUpperBound(1)) = "isfromeditor"
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, vKeyArray.GetUpperBound(1)) = IsFromEditor()
        End If
        m_lReturn = CType(MergeKeyArray(v_vNewKeyArray:=vKeyArray), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Remove the array for now
        vKeyArray = Nothing

        If Information.IsArray(m_vKeyArray) Then
            ' Set it's keys
            If bNav3 Then

                m_lReturn = oObject.NavigatorV3_SetKeys(vKeyArray:=m_vKeyArray)
            Else

                m_lReturn = oObject.SetKeys(vKeyArray:=m_vKeyArray)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Clear up
                oObject = Nothing
                Return result
            End If
        End If

        ' Set the status
        stbStatus.Items.Item(ACPanelStatus).Text = "Step Started."

        ' CTAF 071101
        ' Busy mouse pointer if we're on the server
        If bServerSide Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
        End If

        'MKR 18/10/2004 PN 13102
        'If Finding another claim then Unlock current one if is in use
        If sComponentName = "iCLMFindClaim.Interface_Renamed" And m_lClaimID <> 0 Then

            ' Untick all the treeview items
            For iLoop1 As Integer = 1 To tvwTree.Nodes.Item(0).Nodes.Count
                tvwTree.Nodes.Item(0).Nodes(iLoop1 - 1).Checked = False
            Next iLoop1
        End If
        Application.DoEvents()
        ' Start it
        If bNav3 Then

            m_lReturn = oObject.NavigatorV3_Start()
        Else

            m_lReturn = oObject.Start()
            'oObject.Show()
        End If
        ' RAW 27/02/2003 : ISS2487 : added
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            oObject = Nothing
            If bServerSide Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If
            Return result
        End If
        ' RAW 27/02/2003 : ISS2487 : end
        ' Get it's keys
        Application.DoEvents()
        If bNav3 Then

            m_lReturn = oObject.NavigatorV3_GetKeys(vKeyArray:=vKeyArray)
        Else

            m_lReturn = oObject.GetKeys(vKeyArray:=vKeyArray)
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            oObject = Nothing
            ' RAW 27/02/2003 : ISS2487 : added
            If bServerSide Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If
            ' RAW 27/02/2003 : ISS2487 : end
            Return result
        End If
        'MKR 19/10/2004 PN 13102
        'Need to get claim id thats being banded around so as to unlock later
        If Information.IsArray(vKeyArray) Then

            If Not Object.Equals(vKeyArray, Nothing) And Not False Then
                'just to get claim cnr being used

                For iLoop1 As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                    If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)) = PMNavKeyConst.PMKeyNameClaimCnt Then

                        m_lClaimID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                        Exit For
                    End If
                Next iLoop1
            End If
        End If

        ' Get it's summary
        If bNav3 Then

            m_lReturn = oObject.NavigatorV3_GetSummary(vSummaryArray:=vSummaryArray)
        Else

            m_lReturn = oObject.GetSummary(vSummaryArray:=vSummaryArray)
        End If
        m_lReturn = CType(ProcessSummary(v_vSummaryArray:=vSummaryArray), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Dont care as it's not critical.
        End If

        ' Merge the keys back in

        m_lReturn = CType(MergeKeyArray(v_vNewKeyArray:=vKeyArray), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            oObject = Nothing
            ' RAW 27/02/2003 : ISS2487 : added if test
            If bServerSide Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If
            Return result
        End If

        ' Reset Mouse Pointer
        If bServerSide Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Clear up
            oObject = Nothing
            Return result
        End If

        ' Get the status and adjust the current step accordingly
        If bNav3 Then

            iStatus = oObject.NavigatorV3_Status
        Else
            Try

                iStatus = oObject.Status

            Catch
            End Try
        End If

        ' If it is not OK, Cancel or Navigate, assume OK
        If (iStatus <> gPMConstants.PMEReturnCode.PMOK) And (iStatus <> gPMConstants.PMEReturnCode.PMCancel) And (iStatus <> gPMConstants.PMEReturnCode.PMNavigate) And (iStatus <> gPMConstants.PMEReturnCode.PMNavAction1) And (iStatus <> gPMConstants.PMEReturnCode.PMNavAction2) And (iStatus <> gPMConstants.PMEReturnCode.PMFail) Then
            iStatus = gPMConstants.PMEReturnCode.PMOK
        End If

        If iStatus = gPMConstants.PMEReturnCode.PMNavAction1 Then
            ' Get the value from the OK step value
            If m_lSubNodeCounter = -1 Then
                sCommand = m_vSteps(iPMNavigatorXM.m_lCurrentStep).Action1Action
            ElseIf m_lSubNodeCounter = 0 Then
                sCommand = m_vSteps(m_lCurrentStep + 1).Action1Action
            Else
                sCommand = m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).Action1Action
            End If

        ElseIf iStatus = gPMConstants.PMEReturnCode.PMNavAction2 Then
            ' Get the value from the OK step value
            If m_lSubNodeCounter = -1 Then
                sCommand = m_vSteps(iPMNavigatorXM.m_lCurrentStep).Action2Action
            ElseIf m_lSubNodeCounter = 0 Then
                sCommand = m_vSteps(m_lCurrentStep + 1).Action2Action
            Else
                sCommand = m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).Action2Action
            End If
        ElseIf (iStatus = gPMConstants.PMEReturnCode.PMOK) Then
            ' Get the value from the OK step value
            If m_lSubNodeCounter = -1 Then
                sCommand = m_vSteps(iPMNavigatorXM.m_lCurrentStep).OKAction
            ElseIf m_lSubNodeCounter = 0 Then
                sCommand = m_vSteps(m_lCurrentStep + 1).OKAction
            Else
                sCommand = m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).OKAction
            End If
            If m_lSubNodeCounter = -1 Then
                If sCommand = gPMConstants.PMNavActionBackOne And iPMNavigatorXM.m_lCurrentStep = g_lNumberOfSteps Then
                    sCommand = gPMConstants.PMNavActionCompleteProcess
                End If
            ElseIf m_lSubNodeCounter = 0 Then
                If sCommand = gPMConstants.PMNavActionBackOne And iPMNavigatorXM.m_lCurrentStep + 1 = g_lNumberOfSteps Then
                    sCommand = gPMConstants.PMNavActionCompleteProcess
                End If
            Else
                If sCommand = gPMConstants.PMNavActionBackOne And iPMNavigatorXM.m_lCurrentStep + m_lSubNodeCounter = g_lNumberOfSteps Then
                    sCommand = gPMConstants.PMNavActionCompleteProcess
                End If
            End If

        ElseIf (iStatus = gPMConstants.PMEReturnCode.PMFail) Then
            ' Forward one
            'TF100402 - No, Abort!
            sCommand = gPMConstants.PMNavActionExitMap
            cmdClose.Enabled = True 'MKW PN16141 Enable Close Button on Fail.
        ElseIf (iStatus = gPMConstants.PMEReturnCode.PMCancel) Then
            ' Assume error or cancel
            If m_lSubNodeCounter = -1 Then
                sCommand = m_vSteps(iPMNavigatorXM.m_lCurrentStep).CancelAction
            ElseIf m_lSubNodeCounter = 0 Then
                sCommand = m_vSteps(m_lCurrentStep + 1).CancelAction
            Else
                sCommand = m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).CancelAction
            End If

            'VB 04/02/2005 PN17171
            'Only enable Close button when CurrentStep.CancelAction is AbortProcess
            If sCommand = "AP" Then
                cmdClose.Enabled = True
            End If
            If m_lSubNodeCounter = -1 Then
                lSteps = m_vSteps(iPMNavigatorXM.m_lCurrentStep).CancelSteps
            ElseIf m_lSubNodeCounter = 0 Then
                lSteps = m_vSteps(m_lCurrentStep + 1).CancelSteps
            Else
                lSteps = m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).CancelSteps
            End If

            If sCommand = gPMConstants.PMNavActionForwardX And iPMNavigatorXM.m_lCurrentStep + lSteps > g_lNumberOfSteps Then
                sCommand = gPMConstants.PMNavActionCompleteProcess
            End If
        Else
            ' TODO - Fix iPMBRoadmap to return a correct Status
            ' Bug in iPMBRoadmap - Just assume OK if its a print object!

            If m_lSubNodeCounter = -1 Then
                If m_vSteps(iPMNavigatorXM.m_lCurrentStep).Type = PMNavComponentPrintObject Then
                    sCommand = m_vSteps(m_lCurrentStep).OKAction
                Else
                    ' Assume some kind of exit is required
                    sCommand = gPMConstants.PMNavActionExitMap
                End If
            ElseIf m_lSubNodeCounter = 0 Then
                If m_vSteps(m_lCurrentStep + 1).Type = PMNavComponentPrintObject Then
                    sCommand = m_vSteps(m_lCurrentStep + 1).OKAction
                Else
                    ' Assume some kind of exit is required
                    sCommand = gPMConstants.PMNavActionExitMap
                End If
            Else
                If m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).Type = PMNavComponentPrintObject Then
                    sCommand = m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).OKAction
                Else
                    ' Assume some kind of exit is required
                    sCommand = gPMConstants.PMNavActionExitMap
                End If
            End If
        End If

        ' Decipher the command
        Select Case sCommand
            Case gPMConstants.PMNavActionForwardOne
                lSteps = 1
                If m_lSubNodeCounter = -1 Then
                    iPMNavigatorXM.m_lCurrentStep += lSteps
                Else
                    m_lSubNodeCounter += lSteps
                End If
            Case gPMConstants.PMNavActionForwardX
                If iStatus = gPMConstants.PMEReturnCode.PMOK Then
                    If m_lSubNodeCounter = -1 Then
                        lSteps = m_vSteps(iPMNavigatorXM.m_lCurrentStep).OKSteps
                    ElseIf m_lSubNodeCounter = 0 Then
                        lSteps = m_vSteps(m_lCurrentStep + 1).OKSteps
                    Else
                        lSteps = m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).OKSteps
                    End If
                ElseIf (iStatus = gPMConstants.PMEReturnCode.PMNavAction1) Then
                    If m_lSubNodeCounter = -1 Then
                        lSteps = m_vSteps(iPMNavigatorXM.m_lCurrentStep).Action1Steps
                    ElseIf m_lSubNodeCounter = 0 Then
                        lSteps = m_vSteps(m_lCurrentStep + 1).Action1Steps
                    Else
                        lSteps = m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).Action1Steps
                    End If
                ElseIf (iStatus = gPMConstants.PMEReturnCode.PMNavAction2) Then
                    If m_lSubNodeCounter = -1 Then
                        lSteps = m_vSteps(iPMNavigatorXM.m_lCurrentStep).Action2Steps
                    ElseIf m_lSubNodeCounter = 0 Then
                        lSteps = m_vSteps(m_lCurrentStep + 1).Action2Steps
                    Else
                        lSteps = m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).Action2Steps
                    End If
                Else
                    If m_lSubNodeCounter = -1 Then
                        lSteps = m_vSteps(iPMNavigatorXM.m_lCurrentStep).CancelSteps
                    ElseIf m_lSubNodeCounter = 0 Then
                        lSteps = m_vSteps(m_lCurrentStep + 1).CancelSteps
                    Else
                        lSteps = m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).CancelSteps
                    End If
                End If

                ' Peter Finney - 29/05/2003 - START
                ' ForwardX only allows movement within current map

                ' Get the current node
                If m_lSubNodeCounter = -1 Then
                    If tvwTree.Nodes.Count >= iPMNavigatorXM.m_lCurrentStep + 1 Then
                        oNode = tvwTree.Nodes.Item(iPMNavigatorXM.m_lCurrentStep + 1)
                    Else
                        oNode = tvwTree.Nodes.Item(0).Nodes(iPMNavigatorXM.m_lCurrentStep)
                    End If
                Else
                    oNode = tvwTree.Nodes.Item(0).Nodes(iPMNavigatorXM.m_lCurrentStep).Nodes.Item(m_lSubNodeCounter)
                End If

                ' Walk down x amount of siblings
                For iLoop1 As Integer = 1 To lSteps
                    If oNode.NextNode Is Nothing Then
                        ' If we have hit the end of the map exit loop and use last node
                        Exit For
                    Else
                        oNode = oNode.NextNode
                    End If
                Next iLoop1

                ' Set next step as the node we've just found
                If (m_lSubNodeCounter = -1) Then
                    iPMNavigatorXM.m_lCurrentStep = oNode.Index
                Else
                    m_lSubNodeCounter = oNode.Index
                End If

                ' Peter Finney - 29/05/2003 - END

            Case gPMConstants.PMNavActionBackOne
                lSteps = 1
                If (m_lSubNodeCounter = -1) Then
                    iPMNavigatorXM.m_lCurrentStep -= lSteps
                Else
                    m_lSubNodeCounter -= lSteps
                End If

            Case gPMConstants.PMNavActionBackX
                If iStatus = gPMConstants.PMEReturnCode.PMOK Then
                    If m_lSubNodeCounter = -1 Then
                        lSteps = m_vSteps(iPMNavigatorXM.m_lCurrentStep).OKSteps
                    ElseIf m_lSubNodeCounter = 0 Then
                        lSteps = m_vSteps(m_lCurrentStep + 1).OKSteps
                    Else
                        lSteps = m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).OKSteps
                    End If
                ElseIf (iStatus = gPMConstants.PMEReturnCode.PMNavAction1) Then
                    If m_lSubNodeCounter = -1 Then
                        lSteps = m_vSteps(iPMNavigatorXM.m_lCurrentStep).Action1Steps
                    ElseIf m_lSubNodeCounter = 0 Then
                        lSteps = m_vSteps(m_lCurrentStep + 1).Action1Steps
                    Else
                        lSteps = m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).Action1Steps
                    End If
                ElseIf (iStatus = gPMConstants.PMEReturnCode.PMNavAction2) Then
                    If m_lSubNodeCounter = -1 Then
                        lSteps = m_vSteps(iPMNavigatorXM.m_lCurrentStep).Action2Steps
                    ElseIf m_lSubNodeCounter = 0 Then
                        lSteps = m_vSteps(m_lCurrentStep + 1).Action2Steps
                    Else
                        lSteps = m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).Action2Steps
                    End If

                Else
                    If m_lSubNodeCounter = -1 Then
                        lSteps = m_vSteps(iPMNavigatorXM.m_lCurrentStep).CancelSteps
                    ElseIf m_lSubNodeCounter = 0 Then
                        lSteps = m_vSteps(m_lCurrentStep + 1).CancelSteps
                    Else
                        lSteps = m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).CancelSteps
                    End If
                End If

                ' Peter Finney - 29/05/2003 - START
                ' BackX only allows movement within current map

                ' Get the current node
                If m_lSubNodeCounter = -1 Then
                    oNode = tvwTree.Nodes(0).Nodes.Item(iPMNavigatorXM.m_lCurrentStep)
                Else
                    oNode = tvwTree.Nodes.Item(0).Nodes.Item(iPMNavigatorXM.m_lCurrentStep).Nodes.Item(m_lSubNodeCounter)
                End If

                ' Walk down x amount of siblings
                For iLoop1 As Integer = 1 To lSteps
                    If oNode.PrevNode Is Nothing Then
                        ' If we have hit the start of the map exit loop and use first node
                        Exit For
                    Else
                        oNode = oNode.PrevNode
                    End If
                Next iLoop1

                ' Set next step as the node we've just found
                If m_lSubNodeCounter = -1 Then
                    iPMNavigatorXM.m_lCurrentStep = oNode.Index
                Else
                    m_lSubNodeCounter = oNode.Index
                End If

                ' Peter Finney - 29/05/2003 - END

            Case gPMConstants.PMNavActionCompleteProcess
                m_bEndMap = True
                r_bCompleted = True

            Case gPMConstants.PMNavActionAbortProcess, gPMConstants.PMNavActionExitMap
                result = gPMConstants.PMEReturnCode.PMFalse
                m_bEndMap = True
                r_bCancelled = True
                r_bCompleted = False
                Status = gPMConstants.PMEReturnCode.PMCancel

            Case gPMConstants.PMNavActionStartProcess
                m_bEndMap = True
                r_bCompleted = False

                ' Which New Roadmap do we start
                If iStatus = gPMConstants.PMEReturnCode.PMOK Then
                    If m_lSubNodeCounter = -1 Then
                        r_sNewRoadMap = m_vSteps(iPMNavigatorXM.m_lCurrentStep).OKNewRoadmap
                    ElseIf m_lSubNodeCounter = 0 Then
                        r_sNewRoadMap = m_vSteps(m_lCurrentStep + 1).OKNewRoadmap
                    Else
                        r_sNewRoadMap = m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).OKNewRoadmap
                    End If
                Else
                    If m_lSubNodeCounter = -1 Then
                        r_sNewRoadMap = m_vSteps(iPMNavigatorXM.m_lCurrentStep).CancelNewRoadmap
                    ElseIf m_lSubNodeCounter = 0 Then
                        r_sNewRoadMap = m_vSteps(m_lCurrentStep + 1).CancelNewRoadmap
                    Else
                        r_sNewRoadMap = m_vSteps(m_lCurrentStep + m_lSubNodeCounter + 1).CancelNewRoadmap
                    End If
                End If

                r_sNewRoadMap = r_sNewRoadMap.Trim()
                If r_sNewRoadMap = "" Then
                    gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Required action is Start New Process but no NewRoadmap has been defined for the step. Roadmap = " & m_sXMLFileName & " Step = " & m_vSteps(iPMNavigatorXM.m_lCurrentStep).Description, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCurrentStep")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the XML file extension if it isn't there
                If Not r_sNewRoadMap.EndsWith(".xml") Then
                    r_sNewRoadMap = r_sNewRoadMap & ".xml"
                End If

            Case Else
                ' Error! Check the SetupSteps function
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Stop the map
                m_bEndMap = True

        End Select

        ' To tick, or to untick. That is the question
        If (sCommand = gPMConstants.PMNavActionBackX) Or (sCommand = gPMConstants.PMNavActionBackOne) Then

            ' Work Back and untick things

            If m_lSubNodeCounter = -1 Then
                For iLoop1 As Integer = (m_lCurrentStep + lSteps) To (iPMNavigatorXM.m_lCurrentStep) Step -1
                    'tvwTree.Nodes.Item(iLoop1 - 1).Checked = False
                    tvwTree.Nodes(0).Nodes.Item(iLoop1).Checked = False
                Next iLoop1
            ElseIf m_lSubNodeCounter = 0 Then
                For iLoop1 As Integer = ((m_lCurrentStep + 1) + lSteps) To (m_lCurrentStep + 1) Step -1
                    'tvwTree.Nodes.Item(iLoop1 - 1).Checked = False
                    tvwTree.Nodes.Item(0).Nodes.Item(iPMNavigatorXM.m_lCurrentStep).Nodes.Item(iLoop1).Checked = True
                Next iLoop1
            Else
                For iLoop1 As Integer = ((m_lCurrentStep + m_lSubNodeCounter + 1) + lSteps) To (m_lCurrentStep + m_lSubNodeCounter + 1) Step -1
                    'tvwTree.Nodes.Item(iLoop1 - 1).Checked = False
                    tvwTree.Nodes.Item(0).Nodes.Item(iPMNavigatorXM.m_lCurrentStep).Nodes.Item(iLoop1).Checked = True
                Next iLoop1
            End If

        Else
            If result = gPMConstants.PMEReturnCode.PMTrue Then
                ' Tick!
                tvwTree.SelectedNode.Checked = True
            End If
        End If

        ' Terminate it

        oObject.Dispose()
        ' Clear up
        oObject = Nothing

        ' Set focus back to the tree
        tvwTree.Focus()

        Return result

Err_ProcessCurrentStep:

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessCurrentStep Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCurrentStep", excep:=New Exception(Information.Err().Description))

        Return result
        Return result
    End Function

    Private Sub frmMain_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        If UnloadMode <> vbFormCode Then
            m_lReturn = CType(CloseDown(r_vCancel:=Cancel), gPMConstants.PMEReturnCode)
        End If
        eventArgs.Cancel = Cancel <> 0
    End Sub

    ' ***************************************************************** '
    '
    ' Name: ResizeForm
    '
    ' Description:
    '
    ' History: 28/08/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function ResizeForm() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Dont resize if the window is minimized
            If Me.WindowState = FormWindowState.Minimized Then
                Return result
            End If

            ' Don't let the form resize too small
            If VB6.PixelsToTwipsX(Me.Width) < 5580 Then
                Me.Width = VB6.TwipsToPixelsX(5580)
            End If

            If VB6.PixelsToTwipsY(Me.Height) < 4275 Then
                Me.Height = VB6.TwipsToPixelsY(4275)
            End If

            ' Panel at the top
            lblBanner.Width = Me.Width - VB6.TwipsToPixelsX(140)

            'lnBanner1.X2 = VB6.PixelsToTwipsX(Me.Width) - 140
            lnBanner1.Width = VB6.PixelsToTwipsX(Me.Width) - 140

            'lnBanner2.X2 = VB6.PixelsToTwipsX(Me.Width) - 140
            lnBanner2.Width = VB6.PixelsToTwipsX(Me.Width) - 140

            ' Logo
            picLogo.Left = Me.Width - picLogo.Width - VB6.TwipsToPixelsX(130)

            ' List View
            lvwSummary.Left = picLogo.Left

            ' Tree View
            tvwTree.Width = Me.Width - lvwSummary.Width - VB6.TwipsToPixelsX(200)
            tvwTree.Height = Me.Height - lblBanner.Height - VB6.TwipsToPixelsY(1675)

            lvwSummary.Height = tvwTree.Height - picLogo.Height - VB6.TwipsToPixelsY(70) - lblSummaryDetails.Height
            lblSummaryDetails.Left = picLogo.Left

            ' Button - Close
            cmdClose.Left = Me.Width - cmdClose.Width - VB6.TwipsToPixelsX(140)

            cmdClose.Top = Me.Height - VB6.TwipsToPixelsY(1400) + 18
            lblStatus.Width = stbStatus.Width - navtype.Width - 15

            ' Button - Continue
            cmdContinue.Top = cmdClose.Top

            ' Refresh the controls to avoid mankyness
            tvwTree.Refresh()
            lvwSummary.Refresh()

            Return result

        Catch excep As System.Exception
            'Debugger.Break()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ResizeForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ResizeForm", excep:=excep)

            Return result

        End Try
    End Function

    Private isInitializingComponent As Boolean
    Private Sub frmMain_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        m_lReturn = CType(ResizeForm(), gPMConstants.PMEReturnCode)

    End Sub

    ' ***************************************************************** '
    '
    ' Name: RestartProcess
    '
    ' Description: Restarts the map
    '
    ' History: 28/08/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function RestartProcess() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Configuration.ResetKeysOnRestart Then
                'Remove all keys
                m_vKeyArray = Nothing
            End If

            ' Clear the summary list view
            lvwSummary.Items.Clear()

            ' Untick all the treeview items
            For iLoop1 As Integer = 1 To tvwTree.Nodes.Item(0).Nodes.Count
                tvwTree.Nodes.Item(0).Nodes(iLoop1 - 1).Checked = False
                If tvwTree.Nodes.Item(0).Nodes.Item(iLoop1 - 1).Nodes.Count > 0 Then
                    For iLoop2 As Integer = 1 To tvwTree.Nodes.Item(0).Nodes.Item(iLoop1 - 1).Nodes.Count
                        tvwTree.Nodes.Item(0).Nodes.Item(iLoop1 - 1).Nodes(iLoop2 - 1).Checked = False
                    Next
                End If
            Next iLoop1

            ' Step back to 1
            m_lRestartStep = 0
            iPMNavigatorXM.m_lCurrentStep = 0

            ' Navigator driven?
            If m_bNavigatorDriven Then
                ' Start it off again
                m_lReturn = CType(StartMap(), gPMConstants.PMEReturnCode)
            Else
                ' Select the first one
                tvwTree.SelectedNode = tvwTree.Nodes.Item(ACRootNode)
            End If

            Return result

        Catch excep As System.Exception
            'Debugger.Break()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RestartProcess Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RestartProcess", excep:=excep)

            Return result

        End Try
    End Function
    Private Sub lvwSummary_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwSummary.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        ' Magic keystroke to get the mystical debug information
        ' Or u can double click on the status...
        If ((Shift And ShiftConstants.CtrlMask) > 0) And (eventArgs.KeyCode = Keys.F12) Then

            ' Show debug
            m_lReturn = CType(ShowDebug(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' who cares
            End If

        End If

    End Sub

    Private Sub lvwSummary_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSummary.MouseUp
        Dim Button As Integer = 2
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If eventArgs.Button = MouseButtons.Right Then
            Ctx_mnuSummary.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
        End If

    End Sub

    Public Sub mnuProcessExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuProcessExit.Click

        ' Just fake a call to the close button
        cmdClose_Click(cmdClose, New EventArgs())

    End Sub

    Public Sub mnuProcessRestart_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuProcessRestart.Click

        m_lReturn = CType(RestartProcess(), gPMConstants.PMEReturnCode)

    End Sub

    Public Sub mnuProcessRetry_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuProcessRetry.Click

        m_lReturn = CType(RestartMap(), gPMConstants.PMEReturnCode)

    End Sub

    Public Sub mnuSummaryCopy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuSummaryCopy.Click

        m_lReturn = CType(CopyClipboard(), gPMConstants.PMEReturnCode)

    End Sub

    Private Sub picLogo_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles picLogo.DoubleClick

        ' Launch their web site
        ShellExecute(hwnd:=Me.Handle.ToInt32(), lpOperation:="open", lpFile:=m_sImageURL, lpParameters:="", lpDirectory:="", nShowCmd:=1)

    End Sub

    ' ***************************************************************** '
    '
    ' Name: ShowDebug
    '
    ' Description:
    '
    ' History: 08/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function ShowDebug() As Integer

        Dim result As Integer = 0
        'Dim frmDebug As New frmDebug  'Alkesh

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Tell the user whats about to bang in their face
            MessageBox.Show("The debug screen will now display." & Environment.NewLine & _
                            "To continue the roadmap you will need to close it.", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Prep the list for display
            m_lReturn = CType(objfrmDebug.RefreshDebug(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Set the error level
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to refresh debug screen.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDebug", excep:=New Exception(Information.Err().Description))
                Return result
            End If

            ' Set the properties
            objfrmDebug.XMLFileName = m_sXMLFileName
            objfrmDebug.RoadmapPath = m_sRoadmapPath

            ' Show the form modadidlio
            objfrmDebug.ShowDialog()

            Return result

        Catch excep As System.Exception
            'Debugger.Break()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowDebug Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDebug", excep:=excep)

            Return result
            Return result
        End Try
    End Function

    Private Sub stbStatus_PanelDblClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lblStatus.DoubleClick, navtype.DoubleClick
        Dim Panel As ToolStripStatusLabel = CType(eventSender, ToolStripStatusLabel)

        ' CTAF 161002 - This is disabled for now
        Exit Sub

        If Panel.Name = ACPanelStatus Then

            m_lReturn = CType(ShowDebug(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Oh my god. wow
            End If

        End If

    End Sub

    Private Sub tmrStart_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tmrStart.Tick

        Try
            ' Disable the timer now
            tmrStart.Enabled = False

            ' Start the map!
            If m_bNavigatorDriven Then
                ' Set the status
                stbStatus.Items.Item(ACPanelStatus).Text = "In Progress..."
                ' Start it
                m_lReturn = CType(StartMap(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Umm dunno. Not a lot we can do here.
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    ' ***************************************************************** '
    '
    ' Name: ProcessOneStep
    '
    ' Description:
    '
    ' History: 28/08/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessOneStep(ByVal v_lOneStep As Integer) As Integer

        Dim result As Integer = 0
        Dim bCompleted, bCancelled As Boolean
        Dim sNewRoadmap As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the step
            iPMNavigatorXM.m_lCurrentStep = v_lOneStep

            ' Process it
            m_lReturn = CType(ProcessCurrentStep(r_bCompleted:=bCompleted, r_bCancelled:=bCancelled, r_sNewRoadMap:=sNewRoadmap), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If bCancelled Then
                    stbStatus.Items.Item(ACPanelStatus).Text = "User cancelled."
                Else
                    stbStatus.Items.Item(ACPanelStatus).Text = "Error."
                End If

            Else

                stbStatus.Items.Item(ACPanelStatus).Text = "Ready."

            End If

            ' Save state if we havent finished
            If Not bCompleted Then

                ' We dont want to create one if everything worked
                If Not m_bErrored Then
                    ' Do we want to create a work manager task?
                    If m_vSteps(iPMNavigatorXM.m_lCurrentStep).CreateWorkManagerTask Then

                        ' Prompt to create work manager task
                        If MessageBox.Show("Do you wish to create a WorkManager task so that you may resume later?", "Task", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then

                            'DJM 20/02/2004 PN10255 : Only create a work manager task if not testing a new navigator.
                            If IsFromEditor() = gPMConstants.PMEReturnCode.PMFalse Then

                                ' Set to hour glass
                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                                ' Set the status bar
                                stbStatus.Items.Item(0).Text = "Creating WorkManager task. Please wait..."
                                m_lReturn = CType(CreateWorkManagerTask(), gPMConstants.PMEReturnCode)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                End If

                                If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                                    stbStatus.Items.Item(0).Text = "WorkManager task not created"
                                Else
                                    stbStatus.Items.Item(0).Text = "WorkManager task created"
                                End If

                                ' Reset the mouse pointer
                                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            Else
                                MessageBox.Show("Work Manager task would be created at this point during live run.", "Task", MessageBoxButtons.OK)

                            End If
                        End If
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception
            'Debugger.Break()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessOneStep Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOneStep", excep:=excep)

            Return result

        End Try
    End Function

    Private Sub tvwTree_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tvwTree.DoubleClick

        Try

            ' If its navigator driven then dont do anything
            If (m_bNavigatorDriven) And (m_lAllowErrorRetry = gPMConstants.PMEReturnCode.PMFalse) Then
                Exit Sub
            End If

            ' Anything selected?
            '    If (m_lSelectedStepIndex = 0) Then
            ' SET 27/04/2004 ISS10933 - amended to check selected step is
            '       outside the bounds of the steps array
            If m_lSelectedStepIndex < m_vSteps.GetLowerBound(0) Or m_lSelectedStepIndex > m_vSteps.GetUpperBound(0) Then
                Exit Sub
            End If

            ' Exit if they clicked on a branch node

            If tvwTree.SelectedNode.ImageKey = ACIconNavigate Or tvwTree.SelectedNode.ImageKey = ACIconSubMap Then
                Exit Sub
            End If

            ' Start up again
            m_lReturn = CType(RestartMap(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Blah
            End If

        Catch excep As System.Exception
            'Debugger.Break()
            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="tvwTree_DblClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="tvwTree_DblClick", excep:=excep)

        End Try

    End Sub

    Private Sub tvwTree_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles tvwTree.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Try

            ' Magic keystroke to get the mystical debug information
            ' Or u can double click on the status...
            If ((Shift And ShiftConstants.CtrlMask) > 0) And (eventArgs.KeyCode = Keys.F12) Then

                ' Show debug
                m_lReturn = CType(ShowDebug(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' who cares
                End If

            End If

        Catch excep As System.Exception
            'Debugger.Break()
            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="tvwTree_KeyDown Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="tvwTree_KeyDown", excep:=excep)

        End Try

    End Sub

    Private Sub tvwTree_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles tvwTree.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000

        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        Try
            If tvwTree.Nodes.Count = 0 Then
                Exit Sub
            End If
            ' If an item was selected then
            If Not (tvwTree.GetNodeAt(x, y) Is Nothing) Then
                ' Get its tag
                m_lSelectedStepIndex = Convert.ToString(tvwTree.GetNodeAt(x, y).Tag)
                tvwTree.SelectedNode = tvwTree.GetNodeAt(x, y)
            Else
                ' SET 17/04/2004 ISS10933
                m_lSelectedStepIndex = -1
            End If

        Catch excep As System.Exception
            'Debugger.Break()
            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="tvwTree_MouseDown Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="tvwTree_KeyDown", excep:=excep)
        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: CopyClipboard
    '
    ' Description: Copies the summary item to the clipboard
    '
    ' History: 18/09/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function CopyClipboard() As Integer

        Dim result As Integer = 0
        Dim sText As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If lvwSummary.FocusedItem Is Nothing Then
                Return result
            End If

            'sText = lvwSummary.listViewHelper1.GetListViewSubItem(lvwSummary.FocusedItem, 1).Text
            sText = lvwSummary.FocusedItem.SubItems(1).Text

            ' Clear the clipboard
            My.Computer.Clipboard.Clear()
            ' Add the new entry

            My.Computer.Clipboard.SetText(sText, TextDataFormat.Text)

            Return result

        Catch excep As System.Exception
            'Debugger.Break()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyClipboard Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClipboard", excep:=excep)

            Return result
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RestartMap
    '
    ' Description: Restarts either the map, or the current step
    '
    ' History: 12/04/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function RestartMap() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Don't let them retry if completed
            If m_bCompleted Then
                MessageBox.Show("The process has completed.", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            End If

            ' Start where we left off
            ' SET 27/04/2004 ISS10933

            iPMNavigatorXM.m_lCurrentStep = m_lSelectedStepIndex
            m_lRestartStep = iPMNavigatorXM.m_lCurrentStep
            If m_bNavigatorDriven Then
                ' We want to start the map up again
                m_lReturn = CType(StartMap(), gPMConstants.PMEReturnCode)
            Else
                ' Just want to try the one current step
                m_lReturn = ProcessOneStep(iPMNavigatorXM.m_lCurrentStep)
            End If

            ' Error check
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception
            'Debugger.Break()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RestartMap Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RestartMap", excep:=excep)

            Return result
            Return result
        End Try
    End Function

    Private Sub tvwTree_AfterCheck(ByVal eventSender As Object, ByVal eventArgs As TreeViewEventArgs) Handles tvwTree.AfterCheck
        Dim Node As TreeNode = eventArgs.Node

        ' TODO - Someone needs to stop the users clicking/unclicking when
        '        they have time to code it.. Too much other stuff to do now

    End Sub

    Private Function IsFromEditor() As Integer
        'DJM 20/02/2004 PN10255 : If this is just a test run from the editor then the XML filename will be in the format "tempXXX.xml".
        Dim result As Integer = 0
        Dim sNumber As String = ""
        Dim lLength As Integer

        Try

            'Set the default to false
            result = gPMConstants.PMEReturnCode.PMFalse

            If m_sXMLFileName.ToUpper().StartsWith("TEMP") And m_sXMLFileName.ToUpper().EndsWith(".XML") Then
                lLength = m_sXMLFileName.Length - 8
                sNumber = Mid(m_sXMLFileName, 5, lLength)
                Dim dbNumericTemp As Double
                If Double.TryParse(sNumber, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    'This has come from the editor.
                    result = gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            Return result

        Catch excep As System.Exception
            'Debugger.Break()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsFromEditor Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsFromEditor", excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetKeys
    '
    ' Description: Gets the keys
    '
    ' History: VB - Created.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the external keys
            If m_oParentClass.CallingAppName.ToUpper = "uctCLMCaseClaimList".ToUpper Then
                vKeyArray = m_vKeyArray_Dup
            Else
                If Information.IsArray(m_vKeyArray) Then

                    vKeyArray = m_vKeyArray
                End If
            End If
            Return result

        Catch excep As System.Exception
            'Debugger.Break()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", excep:=excep)

            Return result

        End Try
    End Function
End Class
