Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmAddEditComments
	Inherits System.Windows.Forms.Form
    Private Const vbFormCode As Integer = 0
    Private Sub frmAddEditComments_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
        End If
    End Sub
	'*************************************************************************
	' Form Name:    frmAddEditComments
	'
	' Description:  Popup form to allow user to add/edit a comment. Initially
	'               created for a document comment held on the TransDetail
	'               table but could be re-used for other purposes.
	'
	' History:      CJB 25/03/2004 - Created
	'
	'*************************************************************************
	
	'=================
	'Private Variables
	'=================
	Private m_enStatus As gPMConstants.PMEReturnCode
	
	' Module variable to hold the comment in (accessed by Property Procedures)
	Private m_sComment As String = ""
	
	Private m_oFormfields As Object
	Private m_lReturn As Integer
	
	'=================
	'Public Properties
	'=================
	'Read/Write
	Public Property Comment() As String
		Get 'Amount
			Return txtComment.Text.Trim()
		End Get
		Set(ByVal Value As String)
			txtComment.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatStringMultiLine, Value)
			m_sComment = Value
		End Set
	End Property
	Public WriteOnly Property ReadOnly_Renamed() As Boolean
		Set(ByVal Value As Boolean)
			' Enable/Disable fields
			txtComment.ReadOnly = Value
		End Set
	End Property
	'Read Only
	Public ReadOnly Property Status() As gPMConstants.PMEReturnCode
		Get
			Return m_enStatus
		End Get
	End Property
	
	'===========
	'Form Events
	'===========
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		' Assume failure
		m_enStatus = gPMConstants.PMEReturnCode.PMCancel
		m_enStatus = gPMConstants.PMEReturnCode.PMOK
		' Hide the form to give calling client access to data
		Me.Hide()
	End Sub
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		' Just set the status to User Canelled
		m_enStatus = gPMConstants.PMEReturnCode.PMCancel
		' Hide the form to give calling client access to data
		Me.Hide()
	End Sub
	
	

	Private Sub frmAddEditComments_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        'Call SetFieldValidation(Me, m_oFormfields)
		'Call DisplayCaptions(Me)
	End Sub
	
	Private Sub frmAddEditComments_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		'PN48688
        If UnloadMode <> vbFormCode Then 'If X is pressed
            m_enStatus = gPMConstants.PMEReturnCode.PMCancel
        End If
		eventArgs.Cancel = Cancel <> 0
	End Sub
End Class