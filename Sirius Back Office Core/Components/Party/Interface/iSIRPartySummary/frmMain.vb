Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmMain
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name     : frmMain
	' File Name     : frmMain.frm
	' Date          : 16-10-2002
	' Author        : Ram Chandrabose
	' Description   : Interface used to display the Party Summary Details
	'                   for a supplied party_cnt
	'
	' Edit History  :
	' RAM20021016   : Created
	' ***************************************************************** '
    'developer guide  no.7
    Public Const vbFormCode As Integer = 0
	Private Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As Integer) As Integer
	
	Private Const ACClass As String = "frmMain"
	
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	Private m_lError As Integer
	Private m_sCallingAppName As String = ""
	Private m_lStatus As gPMConstants.PMEReturnCode
	Private m_lErrorNumber As Integer
	Private m_iTask As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	Private m_lPartyCnt As Integer
	Private m_sPartyShortName As String = ""
	
	
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			' Return any error number that might have
			' occurred on the interface.
			Return m_lErrorNumber
		End Get
	End Property
	
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			' Set the calling application name.
			m_sCallingAppName = Value
		End Set
	End Property
	
	Public Property Task() As Integer
		Get
			Return m_iTask
		End Get
		Set(ByVal Value As Integer)
			m_iTask = Value
		End Set
	End Property
	
	Public WriteOnly Property Navigate() As Integer
		Set(ByVal Value As Integer)
			m_lNavigate = Value
		End Set
	End Property
	
	Public WriteOnly Property ProcessMode() As Integer
		Set(ByVal Value As Integer)
			m_lProcessMode = Value
		End Set
	End Property
	
	Public Property TransactionType() As String
		Get
			Return m_sTransactionType
		End Get
		Set(ByVal Value As String)
			m_sTransactionType = Value
		End Set
	End Property
	
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
			m_dtEffectiveDate = Value
		End Set
	End Property
	

	'Private Sub Status(ByVal Value As Integer)
		' Set the interface exit status.
		'm_lStatus = Value
	'End Sub
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	Public WriteOnly Property PartyCnt() As Integer
		Set(ByVal Value As Integer)
			m_lPartyCnt = Value
		End Set
	End Property
	
	Public WriteOnly Property PartyShortName() As String
		Set(ByVal Value As String)
			m_sPartyShortName = Value
		End Set
	End Property
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMOk
		
		Me.Hide()
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		Me.Hide()
		
	End Sub
	

	Private Sub frmMain_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		

		
		Try 
			
			m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
			
			If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				m_lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)
			End If
		
		Catch excep As System.Exception
			
			
			
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Form_Load Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub frmMain_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		' Check if the interface has been terminated by means
		' other than pressing the command buttons (OK or Cancel).


        If UnloadMode <> vbFormCode Then

            Cancel = True

            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
            Me.Hide()
        End If

        eventArgs.Cancel = Cancel <> 0
    End Sub
	
	
	' ***************************************************************** '
	'
	' Name          : SetInterfaceDefaults
	'
	' Description   : Sets any defaults to the Interface form
	'
	' Edit History  :
	' RAM20021016   : Created
	' ***************************************************************** '
	Public Function SetInterfaceDefaults() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If m_sPartyShortName.Length > 0 Then
				Me.Text = "Party Summary - " & m_sPartyShortName
			Else
				Me.Text = "Party Summary Screen"
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetInterfaceDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name          : DataToInterface
	'
	' Description   : This function sets the properties of the Party
	'                   Summary Control, so that it can display the
	'                   details of the party
	'
	' Edit History  :
	' RAM20021016   : Created
	' ***************************************************************** '
	Private Function DataToInterface() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			With uctPartySummaryControl
				.CallingAppName = m_sCallingAppName
				.UnderwritingOrAgency = m_sUnderwriting
				.PartyCnt = m_lPartyCnt ' Sets which party to Load
				.Initialise() ' Initialise the Control
				'PN 21867 made changes so that it can pick the captions from resource file
				.LoadControl() ' Will set the contol's default properties and all
				.GetParty() ' Fetch the details for party
			End With
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DataToInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name          : switchTo
	'
	' Description   : Switches focus to this form.
	'
	' Edit History  :
	' RAM20021021   : Created
	' ***************************************************************** '
	Public Function switchTo() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			SetForegroundWindow(Me.Handle.ToInt32())
			
			' Set the focus
			Me.Activate()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="switchTo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="switchTo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function

    Private Sub frmMain_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.Alt And e.KeyCode = Keys.D1 Then
            DirectCast(uctPartySummaryControl.Controls("tabMainTab"), TabControl).SelectedIndex = 0
        End If
    End Sub
End Class
