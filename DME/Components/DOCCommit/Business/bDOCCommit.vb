Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmTimer
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmTimer
	'
	' Date: {17/4/98}
	'
	' Description: Dummy interface. This form exists only so we can have
	' a timer control.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmTimer"
	
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	
	' PRIVATE Data Members (Begin)
	
	
	'***Insert Form Constants***
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	
	' {* USER DEFINED CODE (Begin) *}
	
	' {* USER DEFINED CODE (End) *}
	
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
	
	' ***************************************************************** '
	'
	' This timer is used to kick the commit process off asynchronously.
	' It constantly polls the RunStatus variable, and when it is set
	' to start, it kicks off a new instance of the form class, just to
	' commit. This is a separate thread, so the user can then interact
	' with the form quite happily.
	'
	' ***************************************************************** '
	Private Sub tmr1_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tmr1.Tick
		
		Dim oForm As Form
		Dim bLocked As Boolean
		
		
		Try 
			
			'have we cancelled
			If g_iRunStatus = DOCCommitCancelled Then
				'let interface know commit has finished
				g_iRunStatus = DOCCommitFinished
			End If
			
			'has has commit been started ?
			If g_iRunStatus = DOCCommitStarted Then
				
				tmr1.Enabled = False
				
				oForm = New bDOCCommit.Form()
				
				'do the commit
				m_lReturn = oForm.CommitBatch(bLocked)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					'doesnt matter yet
				End If
				
				oForm = Nothing
				
				'let interface know commit has finished
				If bLocked Then
					g_iRunStatus = DOCCommitLocked
				Else
					g_iRunStatus = DOCCommitFinished
				End If
				
				tmr1.Enabled = True
				
			End If
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="tmr1_Timer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
End Class