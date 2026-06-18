Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Globalization
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
	
	Private m_oDAO As dPMDAO.Database
	
	Private Declare Function GetComputerName Lib "kernel32"  Alias "GetComputerNameA"(ByVal lpBuffer As String, ByRef nSize As Integer) As Integer
	
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

            ' for testing only
            If m_sCmd = "" Then
                m_sCmd = "Sirius,2.0.2,\\RCLOSE_SA\\c_drive\SA165.htm,Sirius Architecture v1.6.5,d:\program files"
            End If

            ' get parms from command line
            m_lReturn = ParseCommandLine(m_sCmd, m_sCommandLine)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("ParseCommandLine has failed - program will abort", "PMProductUpdateHistory", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If


            ' get a free file handle
            m_lFile = FileSystem.FreeFile()

            ' log file
            FileSystem.FileOpen(m_lFile, "c:\" & m_sCommandLine(COMMANDLINE_PRODUCT_CODE) & "UpdateHistory.log", OpenMode.Output)

            WriteStatus("Initialising ...")

            WriteFile("PMProductUpdateHistory started.")

            ' connect to SA database
            m_lReturn = OpenDatabase()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_bAbort = True
                Return result
            End If

            ' update PMProduct_Update_History
            m_lReturn = ProductUpdateHistory()

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
	
	' establish connection to Sirius Architecture database
	Private Function OpenDatabase() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			m_oDAO = New dPMDAO.Database()
			
			' RDC 27062002 use Comp Serv to open database
			m_lReturn = gPMComponentServices.NewDatabase("", 1, 1, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=m_oDAO)
			
			
			Return m_lReturn
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			WriteFile("OpenDatabase failed:")
			WriteFile(CStr(Information.Err().Number) & "  " & excep.Message)
			
			Return result
		End Try
	End Function
	
	Public Function ProductUpdateHistory() As Integer
		
		Dim result As Integer = 0 
        Dim lNextID, lProductID As Integer 
		Dim sSQL, sDate, sShare As String 
		Dim vData(,) As Object 
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' check for existing PMHist share, create if not there
			m_lReturn = CheckDocumentShare()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				WriteFile("CheckDocumentShare failed")
				Return result
			End If
			
			m_sMsg = "Updating PMProduct_Update_History table"
			
			WriteStatus(m_sMsg & " ...")
			WriteFile(m_sMsg)
			
			' get the relevant product ID
			sSQL = "SELECT pmproduct_id "
			sSQL = sSQL & "FROM pmproduct "
			sSQL = sSQL & "WHERE code = '" & m_sCommandLine(COMMANDLINE_PRODUCT_CODE) & "'"
			
			m_lReturn = m_oDAO.SQLSelect(sSQL:=sSQL, sSQLName:="GetProductID", bStoredProcedure:=False, vResultArray:=vData)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vData) Then
				WriteFile("Failed to get product ID from PMProduct (Code: " & m_sCommandLine(COMMANDLINE_PRODUCT_CODE) & ")")
				Return result
			End If
			

            lProductID = CInt(vData(0, 0))

            ' get rid of any existing history records for this version
            WriteFile("Deleting any existing product history record")

            sSQL = "DELETE FROM "
            sSQL = sSQL & "pmproduct_update_history "
            sSQL = sSQL & "WHERE new_product_version = '" & m_sCommandLine(COMMANDLINE_PRODUCT_VERSION) & "'"

            m_lReturn = m_oDAO.SQLAction(sSQL:=sSQL, sSQLName:="DeleteProductHistory", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                WriteFile("Failed to delete existing Product Update History record")

                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            WriteFile("Adding product history record")

            ' get the next key from PMProduct_Update_History
            sSQL = "Select MAX(PMupdate_ID) FROM PMProduct_Update_History"


            vData = Nothing

            m_lReturn = m_oDAO.SQLSelect(sSQL:=sSQL, sSQLName:="GetNextIndexValue", bStoredProcedure:=False, vResultArray:=vData)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vData) Then
                ' no records
                lNextID = 1
            Else
                ' add 1 to current highest

                Dim dbNumericTemp As Double 
                If Double.TryParse(CStr(vData(0, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                    lNextID = CInt(CDbl(vData(0, 0)) + 1)
                Else
                    lNextID = 1
                End If
            End If
			
			' path to share folder
			sShare = "\\" & m_sSystem & "\\PMHist"
			
			WriteFile("Share is " & sShare)
			
			' build INSERT query
			sDate = DateTime.Now.ToString("dd-MMM-yyyy")
			
			sSQL = "INSERT INTO PMProduct_Update_History ("
			sSQL = sSQL & "pmupdate_id, pmproduct_id, new_product_version, install_date, release_notes_path, update_description"
			sSQL = sSQL & ") VALUES ("
			sSQL = sSQL & CStr(lNextID) & ", "
			sSQL = sSQL & CStr(lProductID) & ", "
			sSQL = sSQL & "'" & m_sCommandLine(COMMANDLINE_PRODUCT_VERSION) & "', "
			sSQL = sSQL & "'" & sDate & "', "
			sSQL = sSQL & "'" & sShare & "\" & m_sCommandLine(COMMANDLINE_RELEASE_NOTES) & "', "
			sSQL = sSQL & "'" & m_sCommandLine(COMMANDLINE_UPDATE_DESCRIPTION) & "')"
			
			m_lReturn = m_oDAO.SQLAction(sSQL:=sSQL, sSQLName:="UpdateProductHistory", bStoredProcedure:=False)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				WriteFile("Failed to insert record into PMProduct_Update_History")
			End If
			
			
			Return m_lReturn
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			WriteFile("ProductUpdateHistory failed")
			WriteFile(CStr(Information.Err().Number) & "  " & excep.Message)
			
			Return result
		End Try
	End Function
	
	Private Function CheckDocumentShare() As Integer
		
		Dim result As Integer = 0
		Dim bFound As Boolean
		Dim sShare, sFile As String
		Dim mypos As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			WriteFile("Checking PMHist share exists")
			
			' share is PMHist on current machine
			m_lReturn = gPMFunctions.GetSystemName(m_sSystem)
			
			mypos = (m_sSystem.IndexOf("_sid") + 1)
			If mypos > 0 Then
				m_sSystem = Mid(m_sSystem, 1, mypos - 1)
			End If
			
			sShare = "\\" & m_sSystem & "\PMHist"
			
			bFound = False
			
			Try 
				
				' attempt to get a file. It'll fail if share doesn't exist
				sFile = FileSystem.Dir(sShare & "\*.*", FileAttribute.Directory)
				
				' share exists
				bFound = True
			
			Catch 
			End Try
			
			
			
			
			If Not bFound Then
				' share is missing so create it
				WriteFile("PMHist share missing - creating")
				

				Dim startInfo As ProcessStartInfo = New ProcessStartInfo("net share PMHist=""" & m_sCommandLine(COMMANDLINE_TARGET_DIR) & """")
				startInfo.WindowStyle = ProcessWindowStyle.Normal
				m_lReturn = CInt(Process.Start(startInfo).Id)
			Else
				WriteFile("PMHist share already exists")
			End If
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			WriteFile("Failed to check document folder share")
			WriteFile(CStr(Information.Err().Number) & "  " & excep.Message)
			
			Return result
		End Try
	End Function
End Class
