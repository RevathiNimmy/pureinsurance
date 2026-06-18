Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 25/04/1997
	'
	' Description: Main interface.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
    'developer guide no.7
    Private Const vbFormCode As Integer = 0
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	
	' PRIVATE Data Members (Begin)
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	Private m_iTask As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	' {* USER DEFINED CODE (Begin) *}
	Private m_sDecisionTitle As String = ""
	Private m_sDecisionText As String = ""
	' {* USER DEFINED CODE (End) *}
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
	' PRIVATE Data Members (End)
	
	
	' PUBLIC Property Procedures (Begin)
	
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			
			' Standard Property.
			
			' Return any error number that might have
			' occurred on the interface.
			Return m_lErrorNumber
			
		End Get
	End Property
	
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			
			' Standard Property.
			
			' Set the calling application name.
			m_sCallingAppName = Value
			
		End Set
	End Property
	
	' {* USER DEFINED CODE (End) *}
	' PUBLIC Property Procedures (End)
	' PRIVATE Property Procedures (Begin)
	
	'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub Status(ByVal Value As Integer)
		'
		' Standard Property.
		'
		' Set the interface exit status.
		'm_lStatus = Value
		'
	'End Sub
	Public ReadOnly Property Status() As Integer
		Get
			
			' Standard Property.
			
			' Return the interface exit status.
			Return m_lStatus
			
		End Get
	End Property
	
	Public Property Task() As Integer
		Get
			
			' Return the objects task.
			Return m_iTask
			
		End Get
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the objects task.
			m_iTask = Value
			
		End Set
	End Property
	
	Public WriteOnly Property Navigate() As Integer
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the navigate flag.
			m_lNavigate = Value
			
		End Set
	End Property
	
	Public WriteOnly Property ProcessMode() As Integer
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the process mode.
			m_lProcessMode = Value
			
		End Set
	End Property
	
	Public WriteOnly Property TransactionType() As String
		Set(ByVal Value As String)
			
			' Standard Property.
			
			' Set the type of business.
			m_sTransactionType = Value
			
		End Set
	End Property
	
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
			
			' Standard Property.
			
			' Set the effective date.
			m_dtEffectiveDate = Value
			
		End Set
	End Property
	
	' {* USER DEFINED CODE (Begin) *}
	
	Public Property DecisionTitle() As String
		Get
			
			' Return the object parameter value.
			Return m_sDecisionTitle
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects parameter value.
			m_sDecisionTitle = Value
			
		End Set
	End Property
	
	Public Property DecisionText() As String
		Get
			
			' Return the object parameter value.
			Return m_sDecisionText
			
		End Get
		Set(ByVal Value As String)
			
			' Set the objects parameter value.
			m_sDecisionText = Value
			
		End Set
	End Property
	' PRIVATE Property Procedures (End)
	
	
	' PUBLIC Methods (Begin)
	' PUBLIC Methods (End)
	
	
	' PRIVATE Methods (Begin)
	
	' ***************************************************************** '
	' Name: DisplayInterfaceDetails
	'
	' Description: Display the decision title and text.
	'
	' ***************************************************************** '
	Private Function DisplayInterfaceDetails() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = GeneralConst.PMTrue
			
			' Display the title.
			Me.Text = m_sDecisionTitle.Trim()
			
			' Display the text.
			lblDecisionText.Text = m_sDecisionText.Trim()
			
			aniAVI.Open("\Share\FileCopy.avi")
			aniAVI.AutoPlay = True
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = GeneralConst.PMError
			
			' Log Error.
			iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to display the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayInterfaceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	' PRIVATE Methods (End)
	
	
	' PRIVATE Events (Begin)
	
	Private Sub Form_Initialize_Renamed()
		
		' Forms initialise event.
		
		Try 
			
			' Set the mouse pointer to busy.
			iGeneralFunc.SetMousePointer(GeneralConst.PMMouseBusy)
			
			' Initialise the error number value.
			m_lErrorNumber = GeneralConst.PMTrue
			
			' Set the cancelled property to true. This is done
			' so that any interface termination will be noted
			' as cancelled except in the event of accepting
			' the interface.
			m_lStatus = GeneralConst.PMCancel
			
			' Set the mouse pointer to normal.
			iGeneralFunc.SetMousePointer(GeneralConst.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			m_lErrorNumber = GeneralConst.PMError
			
			' Log Error.
			iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Exit Sub
			
		End Try
		
	End Sub
	

	Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		' Forms load event.
		
		Try 
			
			' Check if we have had an error so far.
			' Possibly creating the business object.
			If m_lErrorNumber = GeneralConst.PMFalse Then
				' We have already encountered an error,
				' so we MUST exit now.
				Exit Sub
			End If
			
			' Set the mouse pointer to busy.
			iGeneralFunc.SetMousePointer(GeneralConst.PMMouseBusy)
			
			' Center the interface.
			iGeneralFunc.CenterForm(Me)
			
			' Displays the interface details.
			m_lReturn = DisplayInterfaceDetails()
			
			' Check for errors.
			If m_lReturn <> GeneralConst.PMTrue Then
				' Failed to get the interface details.
				m_lErrorNumber = GeneralConst.PMFalse
				
				' Set the mouse pointer to normal.
				iGeneralFunc.SetMousePointer(GeneralConst.PMMouseNormal)
				
				Exit Sub
			End If
			
			' Set the mouse pointer to normal.
			iGeneralFunc.SetMousePointer(GeneralConst.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			' Log Error.
			iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			' Forms activate event.
			
			Try 
				
				' Set focus to the interface form.
				Me.Activate()
				
				Exit Sub
			
			Catch excep As System.Exception
				
				
				
				' Error Section.
				
				m_lErrorNumber = GeneralConst.PMError
				
				' Log Error.
				iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to activate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Activate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
				
				Exit Sub
				
			End Try
		End If
	End Sub
	
	Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		' Forms query unload event.
		
		Try 
			
			' Set the mouse pointer to busy.
			iGeneralFunc.SetMousePointer(GeneralConst.PMMouseBusy)
			
			' Check if the interface has been terminated by means
			' other than pressing the command buttons.

			If UnloadMode <> vbFormCode Then
				' Check the return value.
				'        If (m_lReturn& <> PMTrue) Then
				' Do not procced with the interface termination.
				'            Cancel = 1
				
				' Set the mouse pointer to normal.
				'            SetMousePointer PMMouseNormal
				
				'            Exit Sub
				'        End If
			End If
			
			' Reset the mouse pointer to normal.
			iGeneralFunc.SetMousePointer(GeneralConst.PMMouseReset)
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			m_lErrorNumber = GeneralConst.PMError
			
			' Log Error.
			iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Exit Sub
			
			eventArgs.Cancel = Cancel <> 0
		End Try
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		' Click event of the OK button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = GeneralConst.PMOK
			
			' Everything OK, so we can hide the interface.
			Me.Hide()
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		' Click event of the Cancel button.
		
		Try 
			
			' Set the interface status.
			m_lStatus = GeneralConst.PMCancel
			
			' Everything OK, so we can hide the interface.
			Me.Hide()
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Exit Sub
			
		End Try
		
	End Sub
	' PRIVATE Events (End)
	Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		MemoryHelper.ReleaseMemory()
	End Sub
End Class