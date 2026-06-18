Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmMain
	Inherits System.Windows.Forms.Form
	
	Private m_sZipFile As String = ""
	Private m_sDirectory As String = ""
	Private m_lReturn As Integer
	
	' Files
	Private m_sClientLog As String = ""
	Private m_sServerLog As String = ""
	Private m_sCobolLog As String = ""
	Private m_sRegistry As String = ""
	
	Private Const ACClass As String = "frmMain"
	
	Private m_bClearUp As Boolean
	
	Private m_sMissing As String = ""
	
	Public WriteOnly Property ZipFile() As String
		Set(ByVal Value As String)
			
			m_sZipFile = Value
			
		End Set
	End Property
	

	Private Sub frmMain_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		' Set the caption
		Me.Text = "Sirius Log Viewer - " & m_sZipFile
		
		m_lReturn = LoadFiles()
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: GetDirectory
	'
	' Description: Gets the directory name to unzip the files to
	'
	' History: 23/07/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Private Function GetDirectory(ByVal v_sFilename As String, ByRef r_sDirectory As String) As Integer
		
		Dim result As Integer = 0
		Dim iStart As Integer
		Dim sByte_Renamed As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			iStart = v_sFilename.IndexOf("."c)
			
			r_sDirectory = ""
			
			For iLoop1 As Integer = iStart To 1 Step -1
				' Get the next string
				sByte_Renamed = v_sFilename.Substring(iLoop1 - 1, 1)
				If sByte_Renamed = "\" Then
					Exit For
				End If
				' Concatenate it
				r_sDirectory = sByte_Renamed & r_sDirectory
			Next iLoop1
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDirectory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			


			
			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: UnZipFiles
	'
	' Description:
	'
	' History: 23/07/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Public Function UnZipFiles() As Integer
		
		Dim result As Integer = 0
		Dim oZipper As bPMZipper.Business
		Dim sDirectory As String = ""
		Dim bUnZipped As Boolean
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			
			' Get the directory
			m_lReturn = GetDirectory(v_sFilename:=m_sZipFile, r_sDirectory:=m_sDirectory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_sDirectory = "C:\" & m_sDirectory
			
			' Clear up
			m_lReturn = ClearUp()
			
			' Make the directory
			If Not Directory.Exists(m_sDirectory) Then
				Directory.CreateDirectory(m_sDirectory)
				' We made it, so we'll clear it up
				m_bClearUp = True
			Else
				m_bClearUp = False
			End If
			
			' Create the object
			oZipper = New bPMZipper.Business()
			
			' Unzip
			bUnZipped = oZipper.UnZipFiles(sZipFileName:=m_sZipFile, sDestDirectory:=m_sDirectory, sFileSpec:="*.*")
			
			' Remove the instance
			oZipper = Nothing
			
			m_sClientLog = m_sDirectory & "\" & ACClientLog
			m_sServerLog = m_sDirectory & "\" & ACServerLog
			m_sCobolLog = m_sDirectory & "\" & ACCobolLog
			m_sRegistry = m_sDirectory & "\" & ACRegistry
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnZipFiles Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnZipFiles", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			


			
			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: EncryptString
	'
	' Description:
	'
	' History: 18/07/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Public Function EncryptString(ByVal v_sInString As String, ByRef r_sOutString As String) As Integer
		
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			r_sOutString = ""
			
			For	Each sByte_Renamed As Char In v_sInString
				' Get the next byte
				' Encrypt it with the shaolin magic key
				sByte_Renamed = Strings.Chr(Strings.Asc(sByte_Renamed) Xor &HCFs).ToString()
				' Add it
				r_sOutString = r_sOutString & sByte_Renamed
			Next sByte_Renamed
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EncryptString Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EncryptString", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			


			
			Return result
		End Try
	End Function
	
	Private Function LoadFile(ByRef oForm As frmFile, ByVal v_sFilename As String, ByVal v_bDecrypt As Boolean, ByVal v_sCaption As String) As gPMConstants.PMEReturnCode
		
        Dim sNew, sOld As String
		
		oForm = New frmFile()
		
		' Check the file exists
		' This is a cheap way of checking the validaty of the .zip file
		If FileSystem.Dir(v_sFilename, FileAttribute.Normal) = "" Then
			m_sMissing = m_sMissing & Environment.NewLine & v_sCaption
			Exit Function
		End If
		
		' Load the file
		oForm.Filename = v_sFilename
		
		If v_bDecrypt Then
			' Decrypt it
			sOld = oForm.rtfText.Text
			m_lReturn = EncryptString(v_sInString:=sOld, r_sOutString:=sNew)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			oForm.rtfText.Text = sNew
		End If
		
		' Set the caption
		oForm.Text = v_sCaption
		
		' Show the form
		oForm.Show()
		
	End Function
	
	Private Function LoadFiles() As gPMConstants.PMEReturnCode
		
		Dim oClient, oServer, oCobol, oRegistry As frmFile
		Dim sFilename As String = ""
		
		' Unzip
		m_lReturn = UnZipFiles()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Return gPMConstants.PMEReturnCode.PMFalse
		End If
		
		' Reset the missing files
		m_sMissing = ""
		
		' Client
		m_lReturn = LoadFile(oForm:=oClient, v_sFilename:=m_sClientLog, v_bDecrypt:=True, v_sCaption:="Client Log File")
		
		' Server
		m_lReturn = LoadFile(oForm:=oServer, v_sFilename:=m_sServerLog, v_bDecrypt:=True, v_sCaption:="Server Log File")
		
		' Cobol
		m_lReturn = LoadFile(oForm:=oCobol, v_sFilename:=m_sCobolLog, v_bDecrypt:=True, v_sCaption:="Cobol Log File")
		
		' Registry
		m_lReturn = LoadFile(oForm:=oRegistry, v_sFilename:=m_sRegistry, v_bDecrypt:=False, v_sCaption:="Client Registry File")
		
		' Arrange them so all are visible
		Me.LayoutMdi(MdiLayout.TileHorizontal)
		
		' Display the missing files.
		If m_sMissing <> "" Then
			
			MessageBox.Show("The following files were not in the archive : " &  _
			                Environment.NewLine &  _
			                m_sMissing, "Missing Logs", MessageBoxButtons.OK, MessageBoxIcon.Information)
			
		End If
		
	End Function
	
	Private Sub frmMain_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		
		' Clear up if we made a mess
		If m_bClearUp Then
			m_lReturn = ClearUp()
		End If
		
		' End the program. This ok?!?
		Environment.Exit(0)
		
	End Sub
	
	Public Sub mnuFileExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileExit.Click
		
		' Clear up if we made a mess
		If m_bClearUp Then
			m_lReturn = ClearUp()
		End If
		
		' End the program. This ok?!?
		Environment.Exit(0)
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: ClearUp
	'
	' Description:
	'
	' History: 23/07/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Public Function ClearUp() As Integer
		
		Dim result As Integer = 0
		Dim sFiles As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			sFiles = m_sDirectory & "\*.*"
			
			' Delete all the files in the directory
			If FileSystem.Dir(sFiles, FileAttribute.Normal) <> "" Then
				File.Delete(sFiles)
			End If
			
			' Delete the directory itself
			If Directory.Exists(m_sDirectory) Then
				Directory.Delete(m_sDirectory)
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearUp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearUp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			


			
			Return result
		End Try
	End Function
	
	Public Sub mnuWindowArrange_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _mnuWindowArrange_0.Click, _mnuWindowArrange_1.Click, _mnuWindowArrange_2.Click, _mnuWindowArrange_3.Click
		Dim Index As Integer = Array.IndexOf(mnuWindowArrange, eventSender)
		
		Me.LayoutMdi(Index)
		
	End Sub
End Class
