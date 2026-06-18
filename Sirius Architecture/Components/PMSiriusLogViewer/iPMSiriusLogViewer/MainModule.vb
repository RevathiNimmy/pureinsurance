Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
Module MainModule
	
	' CTAF 190701
	' Constants for temporary file names
	Public Const ACClientLog As String = "~~client.txt"
	Public Const ACServerLog As String = "~~server.txt"
	Public Const ACCobolLog As String = "~~cobol.txt"
	Public Const ACRegistry As String = "~~reg.txt"
	
	Public Const ACApp As String = "iPMSiriusLogViewer"
	Private Const ACClass As String = "MainModule"
	
	Private m_lReturn As Integer
	Private m_sFilename As String = ""
	
	Private Function OpenLogs() As Integer
		
        Try

            ' set the filename
            frmMain.ZipFile = m_sFilename

            ' Show the form
            frmMain.Show()

        Catch



            Exit Function
        End Try

		
	End Function
	
	' ***************************************************************** '
	'
	' Name: Process
	'
	' Description:
	'
	' History: 23/07/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Public Function Process() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Show the drop target
			frmLogDrop.ShowDialog()
			
			' Get the filename
			m_sFilename = frmLogDrop.Filename
			
			If frmLogDrop.Status = gPMConstants.PMEReturnCode.PMOK Then
				
				' Open the files up
				m_lReturn = OpenLogs()
				
			Else
				
				' End the program
				Environment.Exit(0)
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Process Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Process", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			


			
			Return result
		End Try
	End Function
	

	Public Sub main()
		
		' Process the interface
		m_lReturn = Process()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			' Dont care
		End If
		
	End Sub
End Module