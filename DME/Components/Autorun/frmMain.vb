Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Drawing
Imports System.Windows.Forms
Friend Partial Class frmMain
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Class Name: AutoRun
	'
	' Date: 27/11/2000
	'
	' Description: DocuMaster Enterprise pre-requisite software install autorun
	'
	' Edit History:
	'
	'
	'   M.Sarwar Created DME v1.2
	'   G.Pagett Updated for DME v1.3
	'   D.Newson Updated for DME v1.4
	'   D.Newson Updated for DME v1.5
	'   D.Newson Updated for DME v1.6
	'   C.Field  Updated for DME v1.6.2
	'- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
	
	' Variables
	Private m_bServer As Boolean
	Private m_bClient As Boolean
	Private m_sCommand As String = ""
	Dim sVersion As String = ""
	
	
	Private Sub Run(ByRef sCommand As String)
		
		Dim sMsg As String = ""
		Dim dHandle As Double
		
		Try 
			
			' Call the shell

			Dim startInfo As ProcessStartInfo = New ProcessStartInfo(sCommand)
			startInfo.WindowStyle = ProcessWindowStyle.Normal
			dHandle = Process.Start(startInfo).Id
		
		Catch 
			
			
			
			' Show unable to run
			sMsg = "Unable to start program : " & Environment.NewLine &  _
			       sCommand
			
			MessageBox.Show(sMsg, "Run", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try
		
		
	End Sub
	
	Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNext.Click
		
		
		fraInstall.Visible = True
		fraQuestion.Visible = False
		cmdNext.Visible = False
		
	End Sub
	
	Private Sub cmdDME_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDME.Click
		
		m_sCommand = My.Application.Info.DirectoryPath & (IIf(My.Application.Info.DirectoryPath.EndsWith("\"), "", "\")) & "DocuMaster\Setup.exe"
		Run(m_sCommand)
		
	End Sub
	
	Private Sub cmdPre_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPre.Click
		
		cmdNext.Tag = "back"
		cmdNext.Text = "<< &Back"
		fraInstall.Visible = False
		fraQuestion.Visible = False
		fraPre.Visible = True
		cmdNext.Visible = True
		
	End Sub
	
	Private Sub cmdPreClient_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPreClient.Click
		
		m_sCommand = My.Application.Info.DirectoryPath & (IIf(My.Application.Info.DirectoryPath.EndsWith("\"), "", "\")) & "prereqs\Setup.exe"
		Run(m_sCommand)
		cmdNext_Click(cmdNext, New EventArgs())
		
	End Sub
	
	Private Sub cmdPreServer_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPreServer.Click
		
		m_sCommand = My.Application.Info.DirectoryPath & (IIf(My.Application.Info.DirectoryPath.EndsWith("\"), "", "\")) & "prereqs\Setup.exe"
		Run(m_sCommand)
		cmdNext_Click(cmdNext, New EventArgs())
		
	End Sub
	
	Private Sub cmdSA_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSA.Click
		
		fraInstall.Visible = False
		fraQuestion.Visible = True
		fraPre.Visible = False
		cmdNext.Tag = "back"
		cmdNext.Text = "<< &Back"
		cmdNext.Visible = True
		
	End Sub
	
	Private Sub cmdSAClient_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSAClient.Click
		
		m_sCommand = My.Application.Info.DirectoryPath & (IIf(My.Application.Info.DirectoryPath.EndsWith("\"), "", "\")) & "SA\Setup.exe"
		Run(m_sCommand)
		cmdNext_Click(cmdNext, New EventArgs())
		
	End Sub
	
	Private Sub cmdSAServer_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSAServer.Click
		
		m_sCommand = My.Application.Info.DirectoryPath & (IIf(My.Application.Info.DirectoryPath.EndsWith("\"), "", "\")) & "SA\Setup.exe BOTH"
		Run(m_sCommand)
		cmdNext_Click(cmdNext, New EventArgs())
		
	End Sub
	

	Private Sub frmMain_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		fraInstall.Top = fraInstall.Top
		fraInstall.Visible = True
		fraQuestion.Visible = False
		fraPre.Visible = False
		cmdNext.Visible = False
		
		GetVersion("SA")
		lblSA.Text = "Sirius Architecture v" & sVersion
		GetVersion("DocuMaster")
		lblDME.Text = "DocuMaster Enterprise v" & sVersion
		GetVersion("PreReqs")
		lblPre.Text = "Sirius Pre-Requisites v" & sVersion
		
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExit.Click
		
		' Exit the pogram
		Environment.Exit(0)
		
	End Sub
	
	Private Sub GetVersion(ByRef sProduct As String)
		
		Dim sLine As String = ""
		
		'Load the .ini file
		Dim sFilename As String = ".\" & sProduct & "\data.tag"
		
		If FileSystem.Dir(sFilename, FileAttribute.Normal) <> "" Then
			
			FileSystem.FileOpen(1, sFilename, OpenMode.Input)
			' Close before reopening in another mode.
			
			If Information.Err().Number = 0 Then
				
				While Not FileSystem.EOF(1)
					FileSystem.Input(1, sLine)
					
					If Mid(sLine, 1, 8) = "Version=" Then
						sVersion = Mid(sLine, 9, sLine.Length - 8)
					End If
					
				End While
				
				
			End If
			
			FileSystem.FileClose(1)
			
		End If
		
	End Sub
End Class