Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	'#######################################################################
	' Program: PMInstallUnzipper
	' Created: 20/10/2000
	'
	'          Copies zipped install set to server and unzips files.
	'          Unzipped files can then used for auto client installs.
	'#######################################################################
	
	
	
	Private m_lReturn As Integer
	
	Private m_bAbort As Boolean 'if some processes fail, this is set to true
	
	Private m_lFile As Integer ' handle to log file
	Private m_sCmd As String = "" ' command line arg should contain product name and path to zip file
	Private m_sMsg As String = "" ' used for messages to screen and log file
	
	Private m_sProductVersion As String = "" ' passed from install program
	Private m_lProductID As Integer ' primary key on PMProduct
	Private m_sProductCode As String = "" ' passed from install program e.g. Sirius
	Private m_sZipPath As String = "" ' full path to zip file on installation media
	Private m_sZipName As String = "" ' just the zip filename e.g. SA.zip
	
	Private m_sPMSetupPath As String = "" ' e.g. \\ServerNT1\\PMSetup
	Private m_sProductPath As String = "" ' e.g. \\PMSetup\SA
	Private m_sProductFolder As String = "" ' e.g. SA
	Private m_sProductSetupProgram As String = "" ' e.g. setup.exe
	
	Private m_sServerName As String = "" 'e.g. ServerNT1
	
	Private m_oDAO As dPMDAO.Database
	
	Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		
		WriteFile("")
		
		' check abort flag and write appropriate message
		If Not m_bAbort Then
			WriteFile("COMPLETE: NO ERRORS")
		Else
			WriteFile("COMPLETE: WITH ERRORS")
		End If
		
		FileSystem.FileClose(m_lFile)
		
	End Sub
	
	' main process kicks off from here
	Public Sub Start()
		
		Dim lResponse As Integer
		
		Try 
			
			' force form to display
			Me.Show()
			Application.DoEvents()
			
			' show status message on form
			WriteStatus("Initialising ...")
			
			m_bAbort = False
			
			' path to zip files passed from calling program (installer)
			' format: product code (PMProduct),path to zip file
			m_sCmd = Interaction.Command()
			
			' for testing only
			If m_sCmd = "" Then
				m_sCmd = "2.0.2,Sirius,D:\My Documents\SourceSafe\Sirius Architecture\Sirius Architecture Install\Media\2.0.1\Disk Images\Disk1\Sirius Architecture\Zipped Install Set\SAInstallSet.zip"
			End If
			
			' get the product name from the command line input
			m_bAbort = GetParameters()
			
			If m_bAbort Then
				MessageBox.Show("Failed to get parameters from command line", "PMInstallUnzipper", MessageBoxButtons.OK, MessageBoxIcon.Error)
				m_bAbort = True
				Exit Sub
			End If
			
			' get a file handle
			m_lFile = FileSystem.FreeFile()
			
			' log file
			FileSystem.FileOpen(m_lFile, "c:\" & m_sProductCode & "Unzipper.log", OpenMode.Output)
			
			WriteFile("InstallUnzipper started.")
			
			WriteFile("Command line: " & m_sCmd)
			WriteFile("Product code: " & m_sProductCode)
			WriteFile("Product version: " & m_sProductVersion)
			WriteFile("Zip path: " & m_sZipPath)
			WriteFile("Zip file name: " & m_sZipName)
			
			' did we find a product code?
			If m_sProductCode = "" Then
				WriteFile("Unable to resolve Product Name from command line arguments")
				Exit Sub
			End If
			
			' used to resolve file paths
			m_lReturn = gPMFunctions.GetSystemName(m_sServerName)
			
			WriteFile("System name: " & m_sServerName)
			
			' set up path to PMSetup share on the server
			m_sPMSetupPath = "\\" & m_sServerName & "\\PMSetup"
			
			' ensure product folder exists under PMSetup, create if not
			m_lReturn = CheckPMSetupFolder()
			
			WriteFile("PMSetup path: " & m_sPMSetupPath)
			
			' constants in MainModule for each supported product
			Select Case m_sProductCode
				Case PRODUCT_CODE_SA
					' Sirius Architecture
					m_sProductPath = m_sPMSetupPath & "\" & PRODUCT_FOLDER_SA
					m_sProductFolder = PRODUCT_FOLDER_SA
					m_sProductSetupProgram = PRODUCT_SETUP_PROGRAM_SA
				Case PRODUCT_CODE_DME
					' Documaster
					m_sProductPath = m_sPMSetupPath & "\" & PRODUCT_FOLDER_DME
					m_sProductFolder = PRODUCT_FOLDER_DME
					m_sProductSetupProgram = PRODUCT_SETUP_PROGRAM_DME
				Case PRODUCT_CODE_SBO
					' Sirius Back-office
					m_sProductPath = m_sPMSetupPath & "\" & PRODUCT_FOLDER_SBO
					m_sProductFolder = PRODUCT_FOLDER_SBO
					m_sProductSetupProgram = PRODUCT_SETUP_PROGRAM_SBO
				Case Else
					' don't know the product
					WriteFile("Unknown product name")
					m_bAbort = True
					Exit Sub
			End Select
			
			WriteFile("Product path: " & m_sProductPath)
			WriteFile("Product folder: " & m_sProductFolder)
			
			' open database
			m_bAbort = OpenDatabase()
			
			' error?
			If m_bAbort Then
				WriteFile("Process aborted")
				Exit Sub
			End If
			
			' set \\SERVERNAME\PMSetup in PMProduct_Client_Install
			m_bAbort = UpdateDatabase()
			
			' error?
			If m_bAbort Then
				WriteFile("Process aborted")
				Exit Sub
			End If
			
			' close database
			m_bAbort = CloseDatabase()
			
			ShowMessage(MsgBoxStyle.Question, "Copy the Client Install Set to this machine?" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Skip this procedure if install set already exists." & Strings.Chr(13) & Strings.Chr(10), "PMInstallUnzipper", 30, lResponse)
			
			If lResponse = System.Windows.Forms.DialogResult.Cancel Then
				WriteFile("InstallUnzipper has been cancelled")
				m_bAbort = True
				Exit Sub
			End If
			
			' copy zip file to target machine
			m_bAbort = CopyZipFile()
			
			' error?
			If m_bAbort Then
				WriteFile("CopyZipFile aborted")
				Exit Sub
			End If
			
			' unzip files on target machine
			m_bAbort = UnZipFile()
			
			' error?
			If m_bAbort Then
				WriteFile("UnZipFile aborted")
				Exit Sub
			End If
		
		Catch 
			
			
			
			Me.Close()
		End Try
		
		
	End Sub
	
	' breaks command line down into separate parms
	Private Function GetParameters() As Boolean
		
		Dim result As Boolean = False
		Dim bFound As Boolean
		Dim iLoop As Integer
		Dim vTemp As Object
		
		Try 
			
			result = True
			

			vTemp = m_sCmd.Split(","c)
			
			' product version number, product code and full zip file path

            m_sProductVersion = CStr(vTemp(0))

            m_sProductCode = CStr(vTemp(1))

            m_sZipPath = CStr(vTemp(2))

            ' search for zip file name
            bFound = False

            For iLoop = m_sZipPath.Length To 1 Step -1
                If Mid(m_sZipPath, iLoop, 1) = "\" Then
                    bFound = True
                    Exit For
                End If
            Next

            ' found zip file name?
            If bFound Then
                m_sZipName = Mid(m_sZipPath, iLoop + 1)
            Else
                Return result
            End If


            Return False

        Catch



            Return result
        End Try
    End Function

    ' open connection to SA database
    Private Function OpenDatabase() As Boolean

        Dim result As Boolean = False
        Try

            result = True

            ' grab dPMDAO
            m_oDAO = New dPMDAO.Database()

            If m_oDAO Is Nothing Then
                ' failed
                WriteFile("Failed to create dPMDAO")
                Return result
            End If

            ' SiriusArch database, PMProduct_Client_Install
            ' RDC 27062002 use Comp Serv to open database
            m_lReturn = gPMComponentServices.NewDatabase("", 1, 1, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=m_oDAO)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' failed
                WriteFile("Failed to open Sirius Architecture database - check DSN")
                m_oDAO = Nothing
                Return result
            End If


            Return False

        Catch



            WriteFile("Failed to open the SA database")

            'WriteFile(CStr(Information.Err().Number) & "  " & Information.Err().Desc)
            WriteFile(CStr(Information.Err().Number) & "  " & Information.Err().Description)

            Return result
        End Try
    End Function

    ' er, closes the database connection
    Private Function CloseDatabase() As Boolean

        Dim result As Boolean = False
        Try

            result = True

            m_lReturn = m_oDAO.CloseDatabase()
            m_oDAO = Nothing


            Return False

        Catch



            WriteFile("Failed to close the database")

            'WriteFile(CStr(Information.Err().Number) & "  " & Information.Err().Desc)
            WriteFile(CStr(Information.Err().Number) & "  " & Information.Err().Description)
            Return result
        End Try
    End Function

    ' copy zip file from installation media to target machine
    Private Function CopyZipFile() As Boolean

        Dim result As Boolean = False
        Try

            result = True

            m_sMsg = "Copying zip file from installation media"

            WriteStatus(m_sMsg & " ...")
            WriteFile(m_sMsg)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                WriteFile("Failed to locate or create " & m_sProductPath & " on server")
                Return result
            End If

            ' delete existing files, if any.
            ' not used - old product folders are archived
            m_lReturn = DeleteExistingFiles(m_sProductPath)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                WriteFile("Failed to delete existing setup in " & m_sProductPath & " on server")
                Return result
            End If

            ' copy zip file from installation media to PMSetup share
            File.Copy(m_sZipPath, m_sProductPath & "\" & m_sZipName)

            WriteFile("CopyZipFile complete")


            Return False

        Catch



            WriteFile("CopyZipFile failed:")

            'WriteFile(CStr(Information.Err().Number) & "  " & Information.Err().Desc)
            WriteFile(CStr(Information.Err().Number) & "  " & Information.Err().Description)
            Return result
        End Try
    End Function

    ' zip file has been copied to target folder, unzip it
    Private Function UnZipFile() As Boolean

        Dim result As Boolean = False
        Dim bZipReturn As Boolean
        Dim sDestFolder As String = ""
        Dim oZipper As bPMZipper.Business

        Try

            result = True

            m_sMsg = "Unzipping files to target folders"

            WriteStatus(m_sMsg & " ...")
            WriteFile(m_sMsg)

            ' create the zipper component and call unzip function
            oZipper = New bPMZipper.Business()

            ' zip file, targetfolder, all files, create sub-folders
            bZipReturn = oZipper.UnZipFiles(m_sProductPath & "\" & m_sZipName, m_sProductPath, "*.*", False)

            oZipper = Nothing

            ' how'd we do?
            If Not bZipReturn Then
                WriteFile("Failure in bPMZipper.Business")
                Return result
            End If

            ' finished with the Zip file, so delete it
            File.Delete(m_sProductPath & "\" & m_sZipName)

            ' successful
            WriteFile("Unzip complete")


            Return False

        Catch



            WriteFile("UnZipFile failed:")

            'WriteFile(CStr(Information.Err().Number) & "  " & Information.Err().Desc)
            WriteFile(CStr(Information.Err().Number) & "  " & Information.Err().Description)

            Return result
        End Try
    End Function

    ' Update PMProduct_Client_Install with PMSetup location
    Private Function UpdateDatabase() As Boolean

        Dim result As Boolean = False 
        Dim lNumOfRecs As Integer 
        Dim sSQL As String = "" 
        Dim vData(,) As Object 

        Try

            result = True

            m_sMsg = "Updating setup program location in SA database"

            WriteStatus(m_sMsg & " ...")
            WriteFile(m_sMsg)

            ' get the product code from SiriusArchitecture.PMProduct
            sSQL = "SELECT pmproduct_id "
            sSQL = sSQL & "FROM PMProduct "
            sSQL = sSQL & "WHERE code = '" & m_sProductCode & "'"

            lNumOfRecs = 9

            m_lReturn = m_oDAO.SQLSelect(sSQL:=sSQL, sSQLName:="GetProductID", bStoredProcedure:=False, lNumberRecords:=lNumOfRecs, vResultArray:=vData)

            ' how'd we do?
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vData) Then
                ' oops
                WriteFile("Failed to get ProductID from PMProduct")
                Return result
            End If


            m_lProductID = CInt(vData(0, 0))

            m_lReturn = CheckForClientInstallEntry()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                WriteFile("Failed to check that Client Install record exists")
                Return result
            End If

            ' set PMSetup location using Product_ID from PMProduct
            sSQL = "UPDATE PMProduct_Client_Install "
            sSQL = sSQL & "SET client_install_path = '" & m_sProductPath & "\', "
            sSQL = sSQL & "required_server_version = '" & m_sProductVersion & "', "
            sSQL = sSQL & "latest_client_version = '" & m_sProductVersion & "', "
            sSQL = sSQL & "client_install_program = '" & m_sProductSetupProgram & "', "
            sSQL = sSQL & "client_install_description = '" & m_sProductCode & " client install' "

            sSQL = sSQL & "WHERE pmproduct_id = " & CStr(m_lProductID)

            m_lReturn = m_oDAO.SQLAction(sSQL:=sSQL, sSQLName:="UpdateClientInstallLocation", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' failed
                WriteFile("Update to Client Install Path (PMProduct_Client_Install) has failed")
            End If

            WriteFile("Update complete")


            Return False

        Catch



            WriteFile("UpdateDatabase failed:")

            'WriteFile(CStr(Information.Err().Number) & "  " & Information.Err().Desc)
            WriteFile(CStr(Information.Err().Number) & "  " & Information.Err().Description)
            Return result
        End Try
    End Function
	
	' ensure that record exists for the product
	Private Function CheckForClientInstallEntry() As Integer
		
		Dim result As Integer = 0 
		Dim lNumOfRecs As Integer 
		Dim sSQL As String = "" 
		Dim vData(,) As Object 
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			WriteFile("Checking for product client install entry")
			
			lNumOfRecs = 10
			
			sSQL = "SELECT * FROM pmproduct_client_install WHERE pmproduct_id = " & m_lProductID
			
			m_lReturn = m_oDAO.SQLSelect(sSQL:=sSQL, sSQLName:="CheckClientInstall", bStoredProcedure:=False, lNumberRecords:=lNumOfRecs, vResultArray:=vData)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(vData) Then
				WriteFile("Product missing from Client Install table")
				
				m_lReturn = CreateClientInstallEntry()
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					WriteFile("Failed to create Client Install entry")
					Return result
				End If
			End If
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			WriteFile("CheckForClientInstallEntry failed:")

            'WriteFile(CStr(Information.Err().Number) & "  " & Information.Err().Desc)
            WriteFile(CStr(Information.Err().Number) & "  " & Information.Err().Description)
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	' creates entry in pmproduct_client_install
	Private Function CreateClientInstallEntry() As Integer
		
		Dim result As Integer = 0
		Dim sDate, sSQL As String

		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			WriteFile("Creating entry in pmproduct client install table")
			
			sDate = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss")
			
			sSQL = "INSERT INTO pmproduct_client_install ("
			sSQL = sSQL & "pmproduct_id, "
			sSQL = sSQL & "required_server_version, "
			sSQL = sSQL & "server_software_date, "
			sSQL = sSQL & "latest_client_version, "
			sSQL = sSQL & "client_software_date, "
			sSQL = sSQL & "is_latest_client_mandatory, "
			sSQL = sSQL & "is_client_auto_installable, "
			sSQL = sSQL & "client_install_path, "
			sSQL = sSQL & "client_install_program, "
			sSQL = sSQL & "client_install_description, "
			sSQL = sSQL & "client_reboot_level"
			sSQL = sSQL & ") VALUES ("
			sSQL = sSQL & "" & CStr(m_lProductID) & ", "
			sSQL = sSQL & "'" & m_sProductVersion & "', "
			sSQL = sSQL & "'" & sDate & "', "
			sSQL = sSQL & "'" & m_sProductVersion & "', "
			sSQL = sSQL & "'" & sDate & "', "
			sSQL = sSQL & "1, 1, '', "
			sSQL = sSQL & "'" & m_sProductSetupProgram & "', "
			sSQL = sSQL & "'Sirius Architecture client install', "
			sSQL = sSQL & "0)"
			
			m_lReturn = m_oDAO.SQLAction(sSQL:=sSQL, sSQLName:="CreateClientInstallEntry", bStoredProcedure:=False)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' failed
				WriteFile("Update to Client Install Path (PMProduct_Client_Install) has failed")
				Return result
			End If
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			WriteFile("CreateClientInstallEntry failed:")

            'WriteFile(CStr(Information.Err().Number) & "  " & Information.Err().Desc)
            WriteFile(CStr(Information.Err().Number) & "  " & Information.Err().Description)
			Return gPMConstants.PMEReturnCode.PMError
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
	
	' check product folder exists in PMSetup, create if not.
	' PMSetup must already exist, but this should have been checked
	' earlier by the program
	Private Function CheckPMSetupFolder() As Integer
		
		Dim result As Integer = 0
		Dim lFh, lError As Integer
		Dim bFound As Boolean
		Dim sFolder As String = ""
		


		
		result = gPMConstants.PMEReturnCode.PMFalse
		
		sFolder = ""
		
		WriteFile("Checking for PMSetup folder")
		
		' just for the next command
		Try 
			
			' check that share PMSetup exists
			sFolder = FileSystem.Dir(m_sPMSetupPath & "\*.*", FileAttribute.Directory)
		
		Catch 
		End Try
		
		' resume normal error handling


		
		' if share des not exist, create on hard drive
		' if share exists, check it doesn't point to CD drive
		If sFolder = "" Then
			WriteFile("Share does not exist")
			
			' create the share
			m_lReturn = CreatePMSetupFolder()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Failed to create PMSetup folder", "PMInstallUnzipper", MessageBoxButtons.OK, MessageBoxIcon.Error)
				WriteFile("Failed to createPMSetup folder")
				Return result
			End If
		Else
			WriteFile("Share already exists")
			' share exists, so attempt to write a file to it
			' if share points to CD, it will fail
			lFh = FileSystem.FreeFile()
			lError = gPMConstants.PMEReturnCode.PMTrue
			
			Try 
				
				' create temporary file, then delete it
				FileSystem.FileOpen(lFh, m_sPMSetupPath & "\FileCreateTest.txt", OpenMode.Output)
				FileSystem.PrintLine(lFh, "test")
				FileSystem.FileClose(lFh)
				
				File.Delete(m_sPMSetupPath & "\FileCreateTest.txt")
				
				lError = gPMConstants.PMEReturnCode.PMFalse
			
			Catch 
			End Try
			
			
			
			If lError = gPMConstants.PMEReturnCode.PMTrue Then
				WriteFile("PMSetup share set to read only device")
				' didn't work
				' delete existing PMSetup share and create new one
				m_lReturn = CreatePMSetupFolder()
			End If
			
		End If
		
		' share PMSetup exists, check for product sub-folder
		bFound = False
		
		Do While sFolder <> ""
			If sFolder.ToUpper() = m_sProductFolder.ToUpper() Then
				bFound = True
				Exit Do
			End If
			
			sFolder = FileSystem.Dir()
			
		Loop 
		
		If bFound Then
			WriteFile("Renaming old product folder for archiving purposes")
			' rename with timestamp for version control
			FileSystem.Rename(m_sProductPath, m_sProductPath & " OLD " & DateTime.Now.ToString("yyyyMMdd HHmmss"))
		End If
		
		' create new product folder
		m_lReturn = CreateFolder(m_sProductPath)
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Return result
		End If
		
		
		Return gPMConstants.PMEReturnCode.PMTrue
		
CheckPMSetupFolder_Error: 
		
		Return gPMConstants.PMEReturnCode.PMError
		
	End Function
	
	' create PMSetup folder and share
	Private Function CreatePMSetupFolder() As Integer
		
		Dim result As Integer = 0
		Dim lStatus As Integer
		Dim vInput As String = ""
		Dim oInput As frmInputScreen
		
		Dim s As Single
        Dim sChar As String
		Dim bCreated As Boolean
		Dim sFile As String = ""
		


		
		result = gPMConstants.PMEReturnCode.PMFalse
		
		' delete existing share, if there is one

		Dim startInfo As ProcessStartInfo = New ProcessStartInfo("net share PMSetup /delete")
		startInfo.WindowStyle = ProcessWindowStyle.Normal
		m_lReturn = CInt(Process.Start(startInfo).Id)
		
		' get location for new share from user, defaults to D:\PMSetup,
		' but they may wish to change the drive letter
		oInput = New frmInputScreen()
		
		' set defaults for input screen
		oInput.Message = "Enter the location for the new PMSetup folder:"
		oInput.FolderPath = "\PMSetup"
		


		m_lReturn = oInput.Start()
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			' a problem
			MessageBox.Show("User input screen failed", "PMInstallUnzipper", MessageBoxButtons.OK, MessageBoxIcon.Error)
			Return result
		End If
		
		' get screen details
		lStatus = oInput.Status
		vInput = oInput.FolderPath
		
		oInput.Close()
		
		oInput = Nothing
		
		If lStatus <> gPMConstants.PMEReturnCode.PMOK Then
			' user has aborted the setup program
			MessageBox.Show("Procedure aborted:" & Strings.Chr(13) & Strings.Chr(10) &  _
			                "Creation of PMSetup folder has been cancelled by user.", "PMInstallUnzipper", MessageBoxButtons.OK, MessageBoxIcon.Error)
			
			WriteFile("User aborted the PMSetup creation process")
			
			Return result
		End If
		
		' create the folder
		Try 
			
			Directory.CreateDirectory(vInput)
		
		Catch 
		End Try
		


		
		' create the share

		Dim startInfo2 As ProcessStartInfo = New ProcessStartInfo("net share PMSetup=" & vInput)
		startInfo2.WindowStyle = ProcessWindowStyle.Normal
		m_lReturn = CInt(Process.Start(startInfo2).Id)
		
		' this section waits until the share can be seen through the network.
		' Shares can take a few seconds to become available. If the program did
		' not wait, the copy and unzip would fail.
		sChar = "\"
		
		bCreated = False
		
		Do Until bCreated
			
			WriteStatus("Waiting for PMSetup share to take effect ... " & sChar)
			
			s = DateTime.Now.TimeOfDay.TotalSeconds
			Do Until DateTime.Now.TimeOfDay.TotalSeconds > s + 0.5
				Application.DoEvents()
			Loop 
			
			Try 
				
				sFile = FileSystem.Dir(m_sPMSetupPath, FileAttribute.Normal)
				
				bCreated = True
			
			Catch 
			End Try
			
			
			


			
			Select Case sChar
				Case "\"
					sChar = "/"
				Case "/"
					sChar = "\"
			End Select
		Loop 
		
		
		Return gPMConstants.PMEReturnCode.PMTrue
		
CreatePMSetupFolder_Error: 
		
		Return gPMConstants.PMEReturnCode.PMError
		
	End Function
	
	' creates a named folder, funnily enough
	Private Function CreateFolder(ByVal sFolder As String) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			Directory.CreateDirectory(sFolder)
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	' deletes files from specified folder
	Private Function DeleteExistingFiles(ByVal sFolder As String) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' does nothing at the moment, as old folder is renamed, rather than emptied
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
			
			
			
			Return gPMConstants.PMEReturnCode.PMError
		End Try
		
	End Function
	
	' RDC 04012001 MsgBox that times out after a set number of seconds
	Private Sub ShowMessage(ByVal lMsgType As Integer, ByVal sMsgText As String, ByVal sMsgCaption As String, ByVal lCountdown As Integer, ByRef lResponse As Integer)
		
		
		Dim oMsgBox As New frmMsgBox
		
		' icon, message, form caption, timeout period in seconds
		oMsgBox.MessageType = lMsgType
		oMsgBox.MessageText = sMsgText
		oMsgBox.MessageCaption = sMsgCaption
		oMsgBox.Countdown = lCountdown
		


		oMsgBox.ShowDialog()
		
		lResponse = oMsgBox.Response
		
		oMsgBox.Close()
		
		
	End Sub
End Class
