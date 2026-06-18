Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Windows.Forms
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 20/1/98
	'
	' Description: Main interface data transfer from documaster 2 to
	' documaster enterprise. This is really a business object, but the
	' interface is in here as this is a one off process, always run on
	' the server.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
	
	'Hold the status property
	Private m_iStatus As Integer
	Private Sub cmdAbort_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAbort.Click
		
		'If running, we abort, else just cancel and go.
		If m_iStatus = ACStarted Then
			m_iStatus = ACAbort
			ProcessCommand()
			
		Else
			m_iStatus = ACCancel
			ProcessCommand()
			Me.Close()
		End If
		
	End Sub
	Private Sub cmdStart_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdStart.Click
		
		'start the transfer process
		m_iStatus = ACStarted
		cmdAbort.Text = "Abort"
		cmdViewReport.Enabled = False
		
		ProcessCommand()
		
		m_iStatus = False
		cmdAbort.Text = "Cancel"
		cmdViewReport.Enabled = True
		
	End Sub
	Private Sub cmdViewReport_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdViewReport.Click
		
		'View the report if a transfer has taken place
		m_iStatus = ACViewReport
		ProcessCommand()
		
	End Sub
	
	
	Public Property Status() As Integer
		Get
			
			Return m_iStatus
			
		End Get
		Set(ByVal Value As Integer)
			

			m_iStatus = CInt(Value)
			
		End Set
	End Property
End Class