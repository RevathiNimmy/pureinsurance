Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmMain
	Inherits System.Windows.Forms.Form
	
	Private Const ACClass As String = "frmMain"
	
	' Error checker
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	' Selected step
	Private m_lSelectedStepIndex As Integer
	
	' External keys
	Private m_vExtKeyArray( ,  ) As Object
	
	' RestartStep
	Private m_lRestartStep As Integer
	
	' Errored?
	Private m_bErrored As Boolean
	
	' Status
	Private m_lStatus As gPMConstants.PMEReturnCode
	
	' Complete or not?
	Private m_bCompleted As Boolean
	
	'TF260202 - Transaction Types (from GIIConst.bas)
	Private Const PMTransactionTypeNB As String = "G_NB"
	Private Const PMTransactionTypeReview As String = "G_REVIEW"
	Private Const PMTransactionTypeDefaults As String = "G_DEFAULTS"
	Private Const PMTransactionTypeMTAFullQuote As String = "G_MTA_FQ"
	'ED 09092002 - now in gPMConstants
	'Private Const PMTransactionTypeMTA = "G_MTA"
	'Private Const PMTransactionTypeRenewals = "G_RENEW"
	'sj 23/09/2002 - start
	Private m_bClientDisplayed As Boolean
	'sj 23/09/2002 - end
	
	'To display TaskInstanceDisplay form
	Private m_sTaskDescription As String = ""
	Private m_sPartyName As String = ""
	Private m_lPartyCnt As Integer
	Private m_sWMDescription As String = ""
	
	Private m_lClaimId As Integer
	
#If APPDEBUG Then

	
	' Form shows non-modally when in debug (for now!?)
	Private m_bEnded As Boolean
	
	
	Public Property Get Ended() As Boolean
	Ended = m_bEnded
	End Property
	
#End If
	
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
	
    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property
	
	' ***************************************************************** '
	'
	' Name: ShowSteps
	'
	' Description:
	'
	' History: 21/08/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function ShowSteps() As Integer
		
		Dim result As Integer = 0
		Dim sImage As String = ""
		Dim nodeX As TreeNode
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Me.Text = ACTitle & " - " & m_sRoadmap & " (" & g_oObjectManager.UserName & ")"
			Me.lvwSummary.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(Me.lvwSummary.Width) / 2) - 40))
			Me.lvwSummary.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(Me.lvwSummary.Width) / 2) - 40))
			
			If m_bNavigatorDriven Then
				stbStatus.Items.Item(ACPanelNavType).Text = "Navigator driven"
			Else
				stbStatus.Items.Item(ACPanelNavType).Text = "User driven"
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
					Case Else
						sImage = ACIconDataForm
				End Select
				

				nodeX = tvwTree.Nodes.Find(ACRootNode, True)(0).Nodes.Add("NODE" & iLoop1, m_vSteps(iLoop1).Description, sImage)
				
				' Set the tag to the index of the step array
				nodeX.Tag = CStr(iLoop1)
				
			Next iLoop1
			
			' Expand the root
			tvwTree.Nodes.Item(ACRootNode).Expand()
			
			' Start step
			m_lCurrentStep = 0
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowSteps Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowSteps", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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

	'Private Function CreateWorkManagerTask() As Integer
		'Dim result As Integer = 0
		'Dim bCNRoadmap As Object
		'

		'Dim oBusiness As bCNRoadmap.Business
		'Dim vTempArray As Object
		'Dim lStep As Integer
		'Dim vTemporaryWMTaskDetailsArray, vTemporaryWMTaskCntArray As Object
		'Dim lUserGroupId, lUserId As Integer
		'Dim iArrayPointer As Integer
		'
		'Try 
			'
			' Set the status to complete (so it clears the old one down)
			'm_lStatus = gPMConstants.PMEReturnCode.PMOK
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' Store the restart step in the key array
			''ReDim vTempArray(1, 0)
			'

			'vTempArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameCurrentNashStep
			'
			' Where to restart?

			'lStep = m_vSteps(m_lCurrentStep).ResumeStep
			'If lStep = ACResumeStepCurrent Then

				'vTempArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lCurrentStep
			'Else

				'vTempArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = lStep
			'End If
			'

			'm_lReturn = CType(MergeKeyArray(v_vNewKeyArray:=vTempArray), gPMConstants.PMEReturnCode)
			'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'Return gPMConstants.PMEReturnCode.PMFalse
			'End If
			'
			' Remove our temp array
			'vTempArray = Nothing
			'
			' Create the business object
			'Dim temp_oBusiness As Object
			'm_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bCNRoadmap.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			'oBusiness = temp_oBusiness
			'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'Return gPMConstants.PMEReturnCode.PMFalse
			'End If
			'
			' TODO - GET THE STORED PROCEDURE OF GIITEST!!!
			'
			'JAS====================================
			'Call GetTemporaryWMTaskCnt from business object to get temporary task cnt and
			'and task description

			'm_lReturn = oBusiness.GetTemporaryWMTaskDetails(r_vResultArray:=vTemporaryWMTaskDetailsArray)
			'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'result = gPMConstants.PMEReturnCode.PMFalse
				' Log Error Message
				'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to read Temporary (in progress)work manager task.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWorkManagerTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				'Return result
			'End If
			'If Information.IsArray(vTemporaryWMTaskDetailsArray) Then

    'm_sTaskDescription = CStr(vTemporaryWMTaskDetailsArray(1, 0))
    ''ReDim vTemporaryWMTaskCntArray(0, 0)


    'vTemporaryWMTaskCntArray(0, 0) = vTemporaryWMTaskDetailsArray(0, 0)
    'End If
    '
    '
    'find name of customer

    'For 'iLoop As Integer = m_vKeyArray.GetLowerBound(1) To m_vKeyArray.GetUpperBound(1)

    'If CStr(m_vKeyArray(0, iLoop)) = PMNavKeyConst.PMKeyNameClientName Then
    'iArrayPointer = iLoop
    'End If
    'Next iLoop

    'm_sPartyName = CStr(m_vKeyArray(1, iArrayPointer))
    '
    'find description
    'm_sWMDescription = ACTaskDescription
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get customer name.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWorkManagerTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'Return result
    'End If
    '
    'call  DisplayMultipleTaskInstanceDisplayForm to allow user
    'to choose User Group and User for new work manager task
    'm_lReturn = CType(DisplayMultipleTaskInstanceDisplayForm(r_vPMWrkTaskInstanceCntArray:=vTemporaryWMTaskCntArray, r_lUserGroupID:=lUserGroupId, r_lUserID:=lUserId), gPMConstants.PMEReturnCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create DisplayMultipleTaskInstanceDisplayForm.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWorkManagerTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'Return result
    'End If
    'End JAS================================
    '
    '
    '
    '
    '
    '
    ' Add the task and keys

    'm_lReturn = oBusiness.CreateWorkTask(v_sTaskCode:=ACWMTaskCode, v_sDescription:=ACTaskDescription, v_lUserGroupID:=lUserGroupId, v_lUserID:=lUserId, r_vKeyArray:=m_vKeyArray)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Clear the business object
    'oBusiness = Nothing
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create a work manager task.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateWorkManagerTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'Return result
    'End If
    '
    ' Terminate the business object

    'm_lReturn = oBusiness.Terminate()
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' Remove the business object
    'oBusiness = Nothing
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateWorkManagerTask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateWorkManagerTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: CloseDown
    '
    ' Description:
    '
    ' History: 29/08/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function CloseDown(Optional ByRef iCancel As Integer = 0) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Stop the timer!
            tmrStart.Enabled = False

            ' Prompt to exit or not?
            If MessageBox.Show("Are you sure you wish to close " & m_sRoadmap & "?", "Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                ' Check if we need to pass back iCancel to QueryUnload
                If Not False Then
                    iCancel = 1
                End If
#If APPDEBUG Then

				m_bEnded = True
#End If
                Return result
            End If

            If m_lClaimId <> 0 Then
                m_lReturn = CType(UnlockClaim(), gPMConstants.PMEReturnCode)
            End If

            ' Delete object manager
            If Not (g_oObjectManager Is Nothing) Then

                ' Terminate it
		g_oObjectManager.Dispose()

                ' Clear up
                g_oObjectManager = Nothing

            End If

            ' Done - Hide
            Me.Hide()

#If APPDEBUG Then

			m_bEnded = True
#End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CloseDown Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CloseDown", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdClose_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClose.Click

        m_lReturn = CType(CloseDown(), gPMConstants.PMEReturnCode)

    End Sub

    ' ***************************************************************** '
    '
    ' Name: UnlockClaim
    '
    ' Description:
    '
    ' History: 01/05/03 DC
    '
    ' ***************************************************************** '
    Private Function UnlockClaim() As Integer
        Dim result As Integer = 0
        Dim oPMLock As bPMLock.User

        Dim temp_oPMLock As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oPMLock = temp_oPMLock

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Failed to process the interface.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Else


            m_lReturn = oPMLock.UnlockKey(sKeyName:="claim_id", vKeyValue:=m_lClaimId, iUserID:=g_oObjectManager.UserID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unlock claim", vApp:=ACApp, vClass:=ACClass, vMethod:="CloseDown", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End If

        End If

        Return result



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unlock Claim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    Private Sub Form_Initialize_Renamed()

        Try

            ' No errors yet...
            m_lError = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise Object Manager
            g_oObjectManager = New bObjectManager.ObjectManager()

            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lError = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

        Catch



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

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="ProcessLogo", r_sSettingValue:=sLogoPath), gPMConstants.PMEReturnCode)
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



            result = gPMConstants.PMEReturnCode.PMError


            Select Case Information.Err().Number
                Case 53 ' File not found
                    ' nothing. just exit and use the sirius logo. its hardly
                    ' mission critical to get the logo on there!

                Case Else

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConfigureBitmap Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConfigureBitmap", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            End Select

            Return result




            Return result
        End Try
    End Function



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

        ' Setup the roadmap
        m_lReturn = CType(SetupSteps(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lError = gPMConstants.PMEReturnCode.PMFalse
            Exit Sub
        End If

        ' Set up the initial keys
        m_lReturn = CType(SetupKeys(), gPMConstants.PMEReturnCode)
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
    ' ***************************************************************** '
    Private Function StartMap() As Integer

        Dim result As Integer = 0
        Dim bCompleted, bCancelled As Boolean
        Dim iCounter As Integer
        Dim lOldStep As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            m_bEndMap = False

            ' Set the step
            ' If no restart step was passed in then this will be 0
            ' which is cool anyway
            m_lCurrentStep = m_lRestartStep

            ' This counter tracks the number of steps so far. So even if
            ' the last step in the map says Forward One, if theres no step
            ' then it wont go forward one and therefore won't error.
            iCounter = 0

#If APPDEBUG Then

			m_lReturn& = frmDebug.RefreshDebug()
			' Dont care if it fails
#End If

            ' Loop while we have something to do
            While (Not m_bEndMap)

                bCompleted = False
                bCancelled = False

                lOldStep = m_lCurrentStep

                m_bErrored = False
                ' Process the next step
                m_lReturn = CType(ProcessCurrentStep(r_bCompleted:=m_bCompleted, r_bCancelled:=bCancelled), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_bEndMap = True
                    'm_bErrored = True
                    If bCancelled Then
                        stbStatus.Items.Item(ACPanelStatus).Text = "User cancelled."
                    Else
                        stbStatus.Items.Item(ACPanelStatus).Text = "Error. ProcessCurrentStep failed."
                    End If

                Else

                    ' Count how many steps we've done
                    iCounter += (m_lCurrentStep - lOldStep)

                    'ED 22082002 Add any Switched steps
                    If iCounter > (ACMaxSteps + g_iSwitchedSteps) Then
                        bCompleted = True
                        m_bEndMap = True
                    End If

                End If

                If m_bCompleted Then
                    ' Set the status bar
                    stbStatus.Items.Item(ACPanelStatus).Text = "Complete."
                End If

#If APPDEBUG Then

				m_lReturn& = frmDebug.RefreshDebug()
				' Dont care if it fails
#End If

            End While

            If bCompleted Then
                tvwTree.Nodes.Item(ACRootNode).Checked = True
            End If

            ' Automatically close the map?
            If m_bAutoClose Then
                ' but only if the map worked
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    Me.Hide()
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartMap Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StartMap", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: MergeKeyArray
    '
    ' Description: Merges a new key array into the global one
    '
    ' History: 22/08/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function MergeKeyArray(ByVal v_vNewKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim iLoop2 As Integer
        Dim bMatch As Boolean
        Dim iIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Don't merge if we dont have any keys
            If Not Information.IsArray(m_vKeyArray) Then

                m_vKeyArray = VB6.CopyArray(v_vNewKeyArray)
                Return result
            End If

            ' Dont do anything if we have no keys
            If Not Information.IsArray(v_vNewKeyArray) Then
                Return result
            End If

            ' SET 25102002 - Add a key for the task code
            If m_lCurrentStep = 0 Then
                bMatch = False
                ' Check if it's in the master array

                For iLoop2 = m_vKeyArray.GetLowerBound(1) To m_vKeyArray.GetUpperBound(1)

                    If CStr(m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop2)) = PMNavKeyConst.PMKeyNameWMTask Then
                        bMatch = True
                        Exit For
                    End If
                Next iLoop2
                If bMatch Then

                    m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop2) = ACWMTaskCode
                Else
                    ' not found so add to the end of the array

                    iIndex = m_vKeyArray.GetUpperBound(1) + 1
                    ReDim Preserve m_vKeyArray(1, iIndex)


                    m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iIndex) = PMNavKeyConst.PMKeyNameWMTask

                    m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iIndex) = ACWMTaskCode
                End If
            End If
            ' SET 25102002 - End

            For iLoop1 As Integer = v_vNewKeyArray.GetLowerBound(1) To v_vNewKeyArray.GetUpperBound(1)

                bMatch = False

                ' Check if it's in the master array

                For iLoop2 = m_vKeyArray.GetLowerBound(1) To m_vKeyArray.GetUpperBound(1)

                    If Not m_bClientDisplayed And CStr(m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop2)) = "long_name" Then

                        Me.Text = Me.Text & "    " & CStr(m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop2))
                        m_bClientDisplayed = True
                    End If


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
                Next iLoop2

                ' Not found so add it
                If Not bMatch Then

                    ' CTAF 220702
                    If Not Information.IsArray(v_vNewKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)) Then


                        iIndex = m_vKeyArray.GetUpperBound(1) + 1
                        ReDim Preserve m_vKeyArray(1, iIndex)



                        m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iIndex) = v_vNewKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)

                        ' CTAF 071101 - Pass around objects
                        If Information.IsReference(v_vNewKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)) Then
                            m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iIndex) = v_vNewKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)
                        Else


                            m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iIndex) = v_vNewKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)
                        End If

                    Else

                        MessageBox.Show("An array has been passed back from a component." & Environment.NewLine & "It is possible that work manager tasks will fail to work properly.", "Array Alert", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    End If

                End If

            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MergeKeyArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeKeyArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

                        If CStr(v_vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, iLoop1)) = lvwSummary.Items.Item(iLoop2 - 1).Text Then
                            bMatch = True

                            ListViewHelper.GetListViewSubItem(lvwSummary.Items.Item(iLoop2 - 1), 1).Text = CStr(v_vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, iLoop1))
                            Exit For
                        End If

                    Next iLoop2

                End If

                ' New one
                If Not bMatch Then
                    ' Check we have something to show

                    If CStr(v_vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, iLoop1)).Trim() <> "" Then
                        ' Show it

                        lstX = lvwSummary.Items.Add("S" & lvwSummary.Items.Count + 1, CStr(v_vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, iLoop1)), "")

                        ListViewHelper.GetListViewSubItem(lstX, 1).Text = CStr(v_vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, iLoop1))
                    End If
                End If

            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            m_vKeyArray = vKeyArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function ProcessCurrentStep(ByRef r_bCompleted As Boolean, ByRef r_bCancelled As Boolean) As Integer

        Dim result As Integer = 0
        Dim sComponentName, sType As String
        Dim bNav3, bNav2 As Boolean
        Dim iStatus As gPMConstants.PMEReturnCode
        Dim sCommand As String = ""
        Dim oObject As Object
        Dim vKeyArray(,) As Object
        Dim vSummaryArray(,) As Object
        Dim lSteps As Integer
        Dim bServerSide As Boolean

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Default to not completed
            r_bCompleted = False

            ' Set the status
            stbStatus.Items.Item(ACPanelStatus).Text = "Preparing step. Please wait..."

            ' Highlight the step
            tvwTree.SelectedNode = tvwTree.Nodes.Item(m_lCurrentStep + 1)

            ' Get the component name and type

            sComponentName = m_vSteps(m_lCurrentStep).Component

            sType = m_vSteps(m_lCurrentStep).Type

            ' Set the mouse cursor
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get the locality of the object

            bServerSide = m_vSteps(m_lCurrentStep).ServerSide

            If bServerSide Then

                ' Get an instance of the object

                m_lReturn = g_oObjectManager.GetInstance(oObject:=oObject, sClassName:=sComponentName, vInstanceManager:=gPMConstants.PMGetViaClientManager)

            Else

                ' Get an instance of the object

                m_lReturn = g_oObjectManager.GetInstance(oObject:=oObject, sClassName:=sComponentName, vInstanceManager:=gPMConstants.PMGetLocalInterface)

            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oObject = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check if its Nav2 or Nav3

            m_lReturn = CType(CheckNav2or3(v_oObject:=oObject, r_bNav2:=bNav2, r_bNav3:=bNav3), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oObject = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the mouse cursor
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of " & sComponentName, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCurrentStep", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                ' Clear up
                oObject = Nothing
                Return result
            End If

            'TF260202 - Ensure NB TransactionType is passed in.
            If bNav3 Then
                ' Set its process modes


                m_lReturn = oObject.NavigatorV3_SetProcessModes(vTask:=m_vSteps(m_lCurrentStep).ComponentAction, vTransactionType:=g_vTransactionType, vProcessMode:=g_vProcessMode)
            Else
                ' Set its process modes


                m_lReturn = oObject.SetProcessModes(vTask:=m_vSteps(m_lCurrentStep).ComponentAction, vTransactionType:=g_vTransactionType, vProcessMode:=g_vProcessMode)

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Clear up
                oObject = Nothing
                Return result
            End If

            ' Set the default step keys

            If Information.IsArray(m_vSteps(m_lCurrentStep).DefaultKeys) Then

                m_lReturn = CType(MergeKeyArray(v_vNewKeyArray:=m_vSteps(m_lCurrentStep).DefaultKeys), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' CTAF 120202
            ' Pass in the current step as a key - every time

            ReDim vKeyArray(1, 0)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameCurrentNashStep

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lCurrentStep


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

            'If Finding another claim then Unlock current one if in use
            If sComponentName = "iCLMFindClaim.Interface" And m_lClaimId <> 0 Then

                m_lReturn = CType(UnlockClaim(), gPMConstants.PMEReturnCode)
                ' Untick all the treeview items
                For iLoop1 As Integer = 1 To tvwTree.Nodes.Count
                    tvwTree.Nodes.Item(iLoop1 - 1).Checked = False
                Next iLoop1

            End If

            ' Start it
            If bNav3 Then

                m_lReturn = oObject.NavigatorV3_Start()
            Else

                m_lReturn = oObject.Start()
            End If

            ' CTAF 071101
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

            ' Get it's keys
            If bNav3 Then

                m_lReturn = oObject.NavigatorV3_GetKeys(vKeyArray:=vKeyArray)
            Else

                m_lReturn = oObject.GetKeys(vKeyArray:=vKeyArray)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oObject = Nothing
                Return result
            End If

            'Need to get claim id thats being banded around so as to unlock later
            If Information.IsArray(vKeyArray) Then


                If Not Object.Equals(vKeyArray, Nothing) And Not False Then

                    'just to get claim cnr being used

                    For lCount As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)


                        If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lCount)) = PMNavKeyConst.PMKeyNameClaimCnt Then


                            m_lClaimId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lCount))

                        End If

                    Next lCount

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
                Return result
            End If

            ' Get the status and adjust the current step accordingly
            If bNav3 Then

                iStatus = oObject.NavigatorV3_Status
            Else

                iStatus = oObject.Status1
            End If

            If iStatus = gPMConstants.PMEReturnCode.PMOK Then
                ' Get the value from the OK step value

                sCommand = m_vSteps(m_lCurrentStep).OKAction
            ElseIf (iStatus = gPMConstants.PMEReturnCode.PMFail) Then
                ' Forward one
                'TF100402 - No, Abort!
                sCommand = gPMConstants.PMNavActionExitMap
            ElseIf (iStatus = gPMConstants.PMEReturnCode.PMCancel) Then
                ' Assume error or cancel
                If sComponentName = "iCLMFindClaim.Interface" Then

                    sCommand = m_vSteps(m_lCurrentStep).CancelAction
                Else

                    sCommand = m_vSteps(m_lCurrentStep).OKAction
                    iStatus = gPMConstants.PMEReturnCode.PMOK
                End If
            Else
                If sComponentName = "iCLMFindClaim.Interface" Then

                    sCommand = m_vSteps(m_lCurrentStep).CancelAction
                Else
                    If sComponentName = "iCLMGetClaimLetter.Interface" Then

                        sCommand = m_vSteps(m_lCurrentStep).OKAction
                    Else

                        ' TODO - Fix iPMBRoadmap to return a correct Status
                        ' Bug in iPMBRoadmap - Just assume OK if its a print object!

                        If m_vSteps(m_lCurrentStep).Type = PMNavComponentPrintObject Then

                            sCommand = m_vSteps(m_lCurrentStep).OKAction
                        Else
                            ' Assume some kind of exit is required
                            sCommand = gPMConstants.PMNavActionExitMap
                        End If

                    End If
                End If
            End If

            ' Decipher the command
            Select Case sCommand
                Case gPMConstants.PMNavActionForwardOne
                    lSteps = 1
                    m_lCurrentStep += lSteps

                Case gPMConstants.PMNavActionForwardX
                    If iStatus = gPMConstants.PMEReturnCode.PMOK Then

                        lSteps = m_vSteps(m_lCurrentStep).OKSteps
                        m_lCurrentStep += lSteps
                    Else

                        lSteps = m_vSteps(m_lCurrentStep).CancelSteps
                        m_lCurrentStep += lSteps
                    End If

                Case gPMConstants.PMNavActionBackOne
                    lSteps = 1
                    m_lCurrentStep -= lSteps

                Case gPMConstants.PMNavActionBackX
                    If iStatus = gPMConstants.PMEReturnCode.PMOK Then

                        lSteps = m_vSteps(m_lCurrentStep).OKSteps
                        m_lCurrentStep -= lSteps
                    Else

                        lSteps = m_vSteps(m_lCurrentStep).CancelSteps
                        m_lCurrentStep -= lSteps
                    End If

                Case gPMConstants.PMNavActionCompleteProcess
                    m_bEndMap = True
                    r_bCompleted = True

                Case gPMConstants.PMNavActionAbortProcess, gPMConstants.PMNavActionExitMap
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_bEndMap = True
                    r_bCancelled = True
                    r_bCompleted = False

                Case Else
                    ' Error! Check the SetupSteps function
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Stop the map
                    m_bEndMap = True

            End Select

            ' To tick, or to untick. That is the question
            If (sCommand = gPMConstants.PMNavActionBackX) Or (sCommand = gPMConstants.PMNavActionBackOne) Then

                ' Work Back and untick things
                For iLoop1 As Integer = (m_lCurrentStep + 2 + lSteps) To (m_lCurrentStep + 2) Step -1
                    tvwTree.Nodes.Item(iLoop1 - 1).Checked = False
                Next iLoop1

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

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessCurrentStep Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCurrentStep", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function
    Private Const vbFormCode As Integer = 0

    Private Sub frmMain_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)


        'Developer Guide No.7
        'If UnloadMode <> vbFormCode Then
        If UnloadMode <> vbFormCode Then
            m_lReturn = CType(CloseDown(iCancel:=Cancel), gPMConstants.PMEReturnCode)
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

            'TODO check at run time
            'lnBanner1.X2 = VB6.PixelsToTwipsX(Me.Width) - 140


            'TODO check at run time
            'lnBanner2.X2 = VB6.PixelsToTwipsX(Me.Width) - 140

            ' Logo
            picLogo.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.Width) - VB6.PixelsToTwipsX(picLogo.Width) - 130)

            ' List View
            lvwSummary.Left = picLogo.Left

            ' Tree View
            tvwTree.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.Width) - VB6.PixelsToTwipsX(lvwSummary.Width) - 200)
            tvwTree.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) - VB6.PixelsToTwipsY(lblBanner.Height) - 1600)

            lvwSummary.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(tvwTree.Height) - VB6.PixelsToTwipsY(picLogo.Height) - 70)

            ' Button - Close
            cmdClose.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.Width) - VB6.PixelsToTwipsX(cmdClose.Width) - 140)
            cmdClose.Top = Me.Height - VB6.TwipsToPixelsY(1400)

            ' Button - Continue
            cmdContinue.Top = cmdClose.Top

            ' Refresh the controls to avoid mankyness
            tvwTree.Refresh()
            lvwSummary.Refresh()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ResizeForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ResizeForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' Set the keys back

            m_vKeyArray = VB6.CopyArray(m_vExtKeyArray)

            ' Clear the summary list view
            lvwSummary.Items.Clear()

            ' Untick all the treeview items
            For iLoop1 As Integer = 1 To tvwTree.Nodes.Count
                tvwTree.Nodes.Item(iLoop1 - 1).Checked = False
            Next iLoop1

            ' Step back to 1
            m_lRestartStep = 0
            m_lCurrentStep = 0

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



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RestartProcess Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RestartProcess", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub lvwSummary_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSummary.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        If Button = MouseButtonConstants.RightButton Then
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
        ShellExecute(hwnd:=Me.Handle.ToInt32(), lpOperation:="open", lpFile:=ACURL, lpParameters:="", lpDirectory:="", nShowCmd:=1)

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
        Dim frmDebug As New frmDebug
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Tell the user whats about to bang in their face
            MessageBox.Show("The debug screen will now display." & Environment.NewLine & _
                            "To continue the roadmap you will need to close it.", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Prep the list for display
            m_lReturn = CType(frmDebug.RefreshDebug(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Set the error level
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to refresh debug screen.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDebug", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Show the form modadidlio
            frmDebug.ShowDialog()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowDebug Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDebug", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function


    Private Sub stbStatus_PanelDblClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Status1.DoubleClick, navtype.DoubleClick
        Dim Panel As ToolStripStatusLabel = CType(eventSender, ToolStripStatusLabel)

        If Panel.Name = ACPanelStatus Then

            m_lReturn = CType(ShowDebug(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Oh my god. wow
            End If

        End If

    End Sub

    Private Sub tmrStart_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tmrStart.Tick

        ' Disable the timer now
        tmrStart.Enabled = False

        ' Start the map!
        If m_bNavigatorDriven Then
            ' Set the status
            stbStatus.Items.Item(ACPanelStatus).Text = "In Progress..."
            ' Start it
            m_lReturn = CType(StartMap(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Umm dunno
            End If
        End If

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

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the step
            m_lCurrentStep = v_lOneStep

            ' Process it
            m_lReturn = CType(ProcessCurrentStep(r_bCompleted:=bCompleted, r_bCancelled:=bCancelled), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If bCancelled Then
                    stbStatus.Items.Item(ACPanelStatus).Text = "User cancelled."
                Else
                    stbStatus.Items.Item(ACPanelStatus).Text = "Error."
                End If

            Else

                stbStatus.Items.Item(ACPanelStatus).Text = "Ready."

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessOneStep Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOneStep", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name : DisplayMultipleTaskInstanceDisplayForm
    ' Desc : Displays the Task Instances form for Multiple Scheduled Task
    '
    ' Edit History
    ' JAS081102 : Created
    ' ***************************************************************** '
    Private Function DisplayMultipleTaskInstanceDisplayForm(ByRef r_vPMWrkTaskInstanceCntArray(,) As Object, ByRef r_lUserGroupID As Integer, ByRef r_lUserID As Integer) As Integer
        Dim result As Integer = 0
        Dim oTaskInstance As iPMWrkTaskInstanceDisplay.Interface_Renamed
        Dim lReturn As Integer
        Dim dtDueDate As Date

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            'calculate due date
            dtDueDate = DateTime.Today.AddDays(ACDefaultTaskDays)

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

            oTaskInstance.Customer = m_sPartyName

            oTaskInstance.DueDate = dtDueDate

            oTaskInstance.Description = m_sWMDescription


            oTaskInstance.PMWrkTaskInstanceCnt = r_vPMWrkTaskInstanceCntArray(0, 0)

            oTaskInstance.TaskDescription = m_sTaskDescription



            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Start the Form

            lReturn = oTaskInstance.Start
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Start Task Instance Display Form:- iPMWrkTaskInstanceDisplay.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="StartStep", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'get the results (UserGroupID & UserID) from the multiple task instance display form.

            r_lUserGroupID = oTaskInstance.UserGroupID

            r_lUserID = oTaskInstance.UserID



            ' If the User Canceled then exit as we do not need
            ' to Refresh the Form details.

            If oTaskInstance.Status = gPMConstants.PMEReturnCode.PMCancel Then

                r_vPMWrkTaskInstanceCntArray = Nothing

                oTaskInstance.Dispose()
                oTaskInstance = Nothing
                Return result
            End If


            oTaskInstance.Dispose()
            oTaskInstance = Nothing

            Return result

        Catch excep As System.Exception





            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayMultipleTaskInstanceDisplayForm", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayMultipleTaskInstanceDisplayForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result


        End Try
    End Function
	
	
	Private Sub tvwTree_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tvwTree.DoubleClick
		
		' If its navigator driven then dont do anything
		If m_bNavigatorDriven Then
			Exit Sub
		End If
		
		' Check we have a valid step id
		If m_lSelectedStepIndex = -1 Then
			Exit Sub
		End If
		
		m_lReturn = CType(ProcessOneStep(v_lOneStep:=m_lSelectedStepIndex), gPMConstants.PMEReturnCode)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			' Blah
		End If
		
		If m_bNavigatorDriven = gPMConstants.PMEReturnCode.PMFalse And stbStatus.Items.Item(ACPanelStatus).Text = "User cancelled." Then
			
			MessageBox.Show("Roadmap will end, as process has been cancelled and no claim selected to maintain.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
			' Done - Hide
			Me.Hide()
			
#If APPDEBUG Then

			m_bEnded = True
#End If
			
		End If
		
	End Sub
	
	Private Sub tvwTree_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles tvwTree.KeyDown
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
	
	Private Sub tvwTree_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles tvwTree.MouseDown
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		
		' If an item was selected then
		If Not (tvwTree.GetNodeAt(x, y) Is Nothing) Then
			' Get its tag
			m_lSelectedStepIndex = Convert.ToString(tvwTree.GetNodeAt(x, y).Tag)
		Else
			m_lSelectedStepIndex = -1
		End If
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: CopyClipboard
	'
	' Description:
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
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyClipboard Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClipboard", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			


			
			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: RestartMap
	'
	' Description:
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
			m_lRestartStep = m_lCurrentStep
			
			m_lReturn = CType(StartMap(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RestartMap Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RestartMap", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			


			
			Return result
		End Try
	End Function
End Class
