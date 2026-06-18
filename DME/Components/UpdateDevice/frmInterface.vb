Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	
	Private m_bAbort As Boolean
	
	Private m_lReturn As Integer
	Private m_lFile As Integer
	
	Private m_sCmd As String = ""
	Private m_sMsg As String = ""
	
	Private m_sSystem As String = ""
	
#If PD_EARLYBOUND = 1 Then

	Private m_oDAO As dPMDAO.Database
#Else
	Private m_oDAO As dPMDAO.Database
#End If
	
	' command line parms - constants in MainModule
	Private m_sCommandLine() As String
	
	Public Function Start() As Integer
		
		Dim result As Integer = 0

		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' force display of the status window
			Me.Show()
			Application.DoEvents()
			
			m_bAbort = False
			
			' get command line and parse parameters
			m_sCmd = Interaction.Command()
			
			' get parms from command line
			m_lReturn = ParseCommandLine(m_sCmd, m_sCommandLine)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("ParseCommandLine has failed - program will abort", "PMProductUpdateHistory", MessageBoxButtons.OK, MessageBoxIcon.Error)
				Return result
			End If
			
			
			' get a free file handle
			m_lFile = FileSystem.FreeFile()
			
			' log file
			FileSystem.FileOpen(m_lFile, "c:\" & m_sCommandLine(COMMANDLINE_PRODUCT_CODE) & "UpdateDevice.log", OpenMode.Output)
			
			WriteStatus("Initialising ...")
			
			WriteFile("UpdateDevice started.")
			
			' connect to DME database
			m_lReturn = OpenDatabase()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_bAbort = True
				Return result
			End If
			
			' update DOC_Device table
			m_lReturn = UpdateDevice()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				m_bAbort = True
				Return result
			End If
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			WriteFile("Start routine failed")
			WriteFile(CStr(Information.Err().Number) & "  " & excep.Message)
			
			Return result
		End Try
	End Function
	
	' show status message on progress form
	Private Sub WriteStatus(ByVal sMsg As String)
		
		lblStatus.Text = sMsg
		Application.DoEvents()
		
	End Sub
	
	' write message to log file
	Private Sub WriteFile(ByVal sMsg As String)
		
		If sMsg = "" Then
			FileSystem.PrintLine(m_lFile, "")
		Else
			FileSystem.PrintLine(m_lFile, DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") & "   " & sMsg)
		End If
		
	End Sub
	
	Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		
		WriteFile("")
		
		' check abort flag and write appropriate message
		If Not m_bAbort Then
			WriteFile("COMPLETE: NO ERRORS")
		Else
			WriteFile("COMPLETE: WITH ERRORS")
		End If
		
		FileSystem.FileClose(m_lFile)
		
		' finished with database, ta
		m_lReturn = m_oDAO.CloseDatabase()
		
	End Sub
	
	' establish connection to Sirius database
	Private Function OpenDatabase() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
#If PD_EARLYBOUND = 1 Then

			Set m_oDAO = New dPMDAO.Database
#Else
			m_oDAO = New dPMDAO.Database()
#End If
			
            m_lReturn = NewDatabase(v_sUsername:=g_sUsername, v_iSourceID:=g_iSourceID, v_iLanguageID:=g_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, r_oDatabase:=m_oDAO)
			
			
			Return m_lReturn
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			WriteFile("OpenDatabase failed:")
			WriteFile(CStr(Information.Err().Number) & "  " & excep.Message)
			
			Return result
		End Try
	End Function
	
	Public Function UpdateDevice() As Integer
		
		Dim result As Integer = 0
        Dim sSQL As String

		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			m_sMsg = "Updating DOC_Device table"
			
			WriteStatus(m_sMsg & " ...")
			WriteFile(m_sMsg)
			
			' build UPDATE query
			
			sSQL = "UPDATE DOC_device "
			sSQL = sSQL & "SET server_unc = "
			sSQL = sSQL & m_sCommandLine(COMMANDLINE_DOCUMENT_STORE_PATH)
			sSQL = sSQL & ", share_name = "
			sSQL = sSQL & m_sCommandLine(COMMANDLINE_DOCUMENT_SHARE)
			
			m_lReturn = m_oDAO.SQLAction(sSQL:=sSQL, sSQLName:="UpdateDevice", bStoredProcedure:=False)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				WriteFile("Failed to insert record into DOC_device")
			End If
			
			
			Return m_lReturn
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			WriteFile("UpdateDevice failed")
			WriteFile(CStr(Information.Err().Number) & "  " & excep.Message)
			
			Return result
		End Try
	End Function
End Class