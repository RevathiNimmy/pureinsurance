Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles

Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	
	Private m_lReturn As Integer
	
	Private m_iSourceID As Integer
	Private m_iLanguageID As Integer
	Private m_sUsername As String = ""
	Private m_iUserID As Integer
	Private m_sPassword As String = ""
	
	Private m_sStorePath As String = ""
	Private m_sTempPath As String = ""
	
	Private m_oObjectManager As bObjectManager.ObjectManager
	Private m_oDPMDAO As dPMDAO.Database
	Private m_oFSO As Object 'Scripting.FileSystemObject
	Private m_oZipper As bPMZipper.Business 'bPMZipper.Business
	
	Private Const ACClass As String = "frmInterface"
	
	Private Sub cmdConvert_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdConvert.Click
		
		
		Dim sMsg As String = "This process should be run when there is no other DME activity." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)
		sMsg = sMsg & "It may take a long time to complete. Ok to proceed?"
		
		m_lReturn = MessageBox.Show(sMsg, ACApp, MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
		
		If m_lReturn <> System.Windows.Forms.DialogResult.OK Then
			Exit Sub
		End If
		
		txtStatus.Text = ""
		
		Status("Started at " & DateTime.Now.ToString("HH:mm:ss"))
		
		m_lReturn = ConvertZips()
		
		Status("Finished at " & DateTime.Now.ToString("HH:mm:ss"))
		
	End Sub
	
	Private Sub cmdExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExit.Click
		
		Me.Close()
		
	End Sub
	
	Private Function ConvertZips() As Integer
		
		Dim result As Integer = 0
		Dim vPages As Object
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			Status("Logging in")
			
			' force a login, for security and to check DB access
			m_oObjectManager = New bObjectManager.ObjectManager()
			
			m_lReturn = m_oObjectManager.Initialise(sCallingAppName:=ACApp)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Log Error Message
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get instance of ObjectManager", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertZips")
                Return result
            End If

            With m_oObjectManager
                m_iSourceID = .SourceID
                m_iLanguageID = .LanguageID
                m_iUserID = .UserID
                m_sUsername = .UserName
                m_sPassword = .Password
            End With

            Status("Opening database")

            m_oDPMDAO = New dPMDAO.Database()

            m_lReturn = m_oDPMDAO.OpenDatabase(sSiriusUsername:=m_sUsername, iSourceID:=CShort(m_iSourceID), iLanguageID:=CShort(m_iLanguageID), sCallingAppName:=ACApp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oDPMDAO = Nothing
                m_oObjectManager.Dispose()
                m_oObjectManager = Nothing

                ' Log Error Message
                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to open database", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertZips")

                Return result
            End If

            Status("Get zipped pages")


            m_lReturn = GetZippedPages(vPages)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CloseObjects()

                ' Log Error Message
                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get zipped pages", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertZips")

                Return result
            End If

            If Not Information.IsArray(vPages) Then
                CloseObjects()

                MessageBox.Show("There are no zipped files in the DME store", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Information)

                Return result
            End If

            m_oFSO = New Object()
            m_oZipper = New bPMZipper.Business()

            If m_oZipper Is Nothing Then
                CloseObjects()

                ' Log Error Message
                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create bPMZipper", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertZips")

                Return result
            End If

            Status("Process zipped pages")


            m_lReturn = ProcessZippedPages(vPages)

            Status("Deleting temporary folder")

            If Directory.Exists(m_sTempPath) Then
                Directory.Delete(m_sTempPath)
            End If

            m_oZipper = Nothing
            m_oFSO = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process zipped pages", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertZips")
            End If

            CloseObjects()


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_oDPMDAO = Nothing
            m_oObjectManager.Dispose()
            m_oObjectManager = Nothing

            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="ConvertZips failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertZips", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function GetZippedPages(ByRef vPages(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT doc.doc_num, doc.zipped, pag.page_name "
            sSQL = sSQL & "FROM doc_document doc "
            sSQL = sSQL & "JOIN doc_page pag ON pag.doc_num = doc.doc_num "
            sSQL = sSQL & "WHERE pag.page_type='zip'"

            m_lReturn = m_oDPMDAO.SQLSelect(sSQL:=sSQL, sSQLName:="GetZippedPages", bStoredProcedure:=False, vResultArray:=vPages, lNumberRecords:=gPMConstants.PMAllRecords)

            Debug.WriteLine(vPages.GetUpperBound(1))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetZippedPages failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetZippedPages", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function ProcessZippedPages(ByVal vPages(,) As Object) As Integer

        Dim result As Integer = 0
        Dim bZipped As Boolean
        Dim lDocNum As Integer
        Dim sPageName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = GetStorePath()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get DME Store path", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessZippedPages")

                Return result
            End If

            For lLoop As Integer = vPages.GetLowerBound(1) To vPages.GetUpperBound(1)


                lDocNum = CInt(vPages(0, lLoop))

                bZipped = CStr(vPages(1, lLoop)).ToUpper() = "Y"

                sPageName = CStr(vPages(2, lLoop)).Trim()

                Status("File " & lLoop & "/" & CStr(vPages.GetUpperBound(1)) & " - Page " & sPageName)

                m_lReturn = ProcessPage(lDocNum, bZipped, sPageName)

            Next

            Status("Processed " & vPages.GetUpperBound(1) - vPages.GetLowerBound(1) + 1 & " files")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="ProcessZippedPages failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessZippedPages", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function ProcessPage(ByVal lDocNum As Integer, ByVal bZipped As Boolean, ByVal sPageName As String) As Integer

        Dim result As Integer = 0
        Dim iPos As Integer
        Dim sInputFilePath, sFilename, sNewZipPath, sNewZipFile, sNewZipExt, sDocType, sSQL As String
        Dim oFolder As Scripting.IFolder 'Scripting.Folder
        'Scripting.File

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sInputFilePath = m_sStorePath & sPageName & ".zip"

            m_lReturn = m_oZipper.UnZipFiles(sZipFileName:=sInputFilePath, sDestDirectory:=m_sTempPath, sFileSpec:="*.*", bNoDirectoryNamesFlag:=True)

            ' the zip file may have contained a zip!
            oFolder = New DirectoryInfo(m_sTempPath)

            For Each oFile As FileInfo In oFolder.Files
                If oFile.Name.ToUpper().EndsWith(".ZIP") Then
                    m_lReturn = m_oZipper.UnZipFiles(sZipFileName:=oFile.FullName, sDestDirectory:=m_sTempPath, sFileSpec:="*.*", bNoDirectoryNamesFlag:=True)

                    File.Delete(oFile.FullName)
                End If
            Next oFile

            ' Get original DME file name/number
            m_lReturn = GetFilename(sPageName, sFilename)

            oFolder = New DirectoryInfo(m_sTempPath)

            ' file the file using the original DME file name/number
            ' Copy it back to the source folder
            ' delete the temp files
            For Each oFile As FileInfo In oFolder.Files
                iPos = IIf(oFile.Name = "" And "." = "", 0, (oFile.Name.LastIndexOf(".") + 1))

                sNewZipPath = m_sTempPath & "\" & sFilename & Mid(oFile.Name, iPos)
                sNewZipFile = sFilename & Mid(oFile.Name, iPos)
                sNewZipExt = Mid(oFile.Name, iPos + 1)


                m_lReturn = CInt(m_oZipper.ZipFile(oFile.FullName, sNewZipPath))

                iPos = IIf(sInputFilePath = "" And "\" = "", 0, (sInputFilePath.LastIndexOf("\") + 1))

                File.Copy(sNewZipPath, sInputFilePath.Substring(0, iPos) & sNewZipFile, True)

                File.Delete(oFile.FullName)

                File.Delete(sNewZipPath)

                Do Until Not (File.Exists(sNewZipPath))
                    Application.DoEvents()
                Loop

            Next oFile

            'change page_type in doc_page
            sSQL = "UPDATE doc_page "
            sSQL = sSQL & "SET page_type = '" & sNewZipExt.ToUpper() & "' "
            sSQL = sSQL & "WHERE page_name = '" & sPageName & "'"

            m_lReturn = m_oDPMDAO.SQLAction(sSQL:=sSQL, sSQLName:="UpdateDocPage", bStoredProcedure:=False)

            'change doc_type in doc_document
            Select Case sNewZipExt.ToUpper()
                Case "TIF" : sDocType = "I"
                Case "TXT" : sDocType = "T"
                Case "RTF" : sDocType = "W"
                Case "DOC" : sDocType = "D"
                Case "XLS" : sDocType = "X"
                Case "PPT" : sDocType = "P"
                Case "MDB" : sDocType = "A"
                Case "HTM" : sDocType = "H"
                Case "GIF" : sDocType = "G"
                Case "JPG" : sDocType = "J"
                Case "PDF" : sDocType = "F"
                Case "HLP" : sDocType = "E"
                Case "ZIP" : sDocType = "Z"
                Case Else : sDocType = "U"
            End Select

            sSQL = "UPDATE doc_document "
            sSQL = sSQL & "SET doc_type = '" & sDocType & "' "
            sSQL = sSQL & "WHERE doc_num = " & CStr(lDocNum)

            m_lReturn = m_oDPMDAO.SQLAction(sSQL:=sSQL, sSQLName:="UpdateDocType", bStoredProcedure:=False)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    Private Function GetFilename(ByVal sFilePath As String, ByRef sFilename As String) As Integer

        Dim result As Integer = 0
        Dim iPos As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            iPos = IIf(sFilePath = "" And "\" = "", 0, (sFilePath.LastIndexOf("\") + 1))

            sFilename = Mid(sFilePath, iPos + 1)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    Private Function GetStorePath() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT dev.server_unc, dev.share_name, vol.directory "
            sSQL = sSQL & "FROM doc_device dev "
            sSQL = sSQL & "JOIN doc_volume vol ON vol.device_id = dev.device_id"

            m_lReturn = m_oDPMDAO.SQLSelect(sSQL:=sSQL, sSQLName:="GetStorePath", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If m_oDPMDAO.Records.Count() = 0 Then
                Return result
            End If

            m_sStorePath = m_oDPMDAO.Records.Fields("server_unc") & m_oDPMDAO.Records.Fields("share_name") & m_oDPMDAO.Records.Fields("directory")

            m_sTempPath = m_oDPMDAO.Records.Fields("server_unc") & m_oDPMDAO.Records.Fields("share_name") & "\Temp"

            Status("Creating temporary folder " & m_sTempPath)

            If Not (Directory.Exists(m_sTempPath)) Then
                Directory.CreateDirectory(m_sTempPath)
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetStorePath failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStorePath", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
	
	Private Sub CloseObjects()
		
		m_lReturn = m_oDPMDAO.CloseDatabase()
		m_oObjectManager.Dispose()

        m_oDPMDAO = Nothing
		m_oObjectManager = Nothing
		
	End Sub
	
	Private Sub Status(ByVal sMsg As String)
		
		txtStatus.Text = txtStatus.Text & Strings.Chr(13) & Strings.Chr(10) & sMsg
		
		txtStatus.SelectionStart = Strings.Len(txtStatus.Text)
		txtStatus.SelectionLength = 0
		
		Application.DoEvents()
		
	End Sub
	
	
	
	
	Private isInitializingComponent As Boolean
	Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
		If isInitializingComponent Then
			Exit Sub
		End If
		
		If Me.WindowState = FormWindowState.Minimized Then
			Exit Sub
		End If
		
		If VB6.PixelsToTwipsX(Me.Width) < 6255 Then Me.Width = VB6.TwipsToPixelsX(6255)
		If VB6.PixelsToTwipsY(Me.Height) < 3735 Then Me.Height = VB6.TwipsToPixelsY(3735)
		
		cmdExit.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6.PixelsToTwipsY(cmdConvert.Height) - 120)
		cmdConvert.Top = cmdExit.Top
		
		cmdExit.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - VB6.PixelsToTwipsX(cmdExit.Width) - 120)
		cmdConvert.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdExit.Left) - VB6.PixelsToTwipsX(cmdConvert.Width) - 60)
		
		txtStatus.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(cmdConvert.Top) - VB6.PixelsToTwipsY(txtStatus.Top) - 120)
		txtStatus.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - VB6.PixelsToTwipsX(txtStatus.Left) - 120)
		
		
	End Sub
End Class
