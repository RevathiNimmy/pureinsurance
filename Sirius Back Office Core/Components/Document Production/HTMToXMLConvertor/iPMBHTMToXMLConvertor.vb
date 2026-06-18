Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles
Friend Class frmInterface
	Inherits System.Windows.Forms.Form
	Private Const ACClass As String = "frmInterface"
	
	' Object parameter members.
	Private m_sCallingAppName As String
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	Private m_sErrorDesc As String
	
	Private m_iTask As Short
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String
	Private m_dtEffectiveDate As Date
	
	Private m_sStepStatus As String
	
	'Private m_oBusiness As Object
	
	'Users
	Private m_lReturn As Integer
	Private m_vTemplateArray As Object
	Private m_sClient As String
	Private m_sServer As String
	Private m_sTextServer As String
	
	
	Private Const iTEMPLATE_ID As Short = 0
	Private Const iTYPE_ID As Short = 1
	Private Const iCODE As Short = 2
	Private Const iDESCRIPTION As Short = 3
	Private Const iENTITY_TYPE_ID As Short = 4
	Private Const iSLOT As Short = 5
	Private Const iFILE_NUMBER As Short = 6
	
	'Entity Type Id's
	Private Const iCLIENT As Short = 1
	Private Const iPOLICY As Short = 2
	Private Const iCLAIM As Short = 3
	
	Private m_sZIP_DIRECTORY As String
	Private m_lTextFileNumber As Integer
	Private m_lFileNumber As Integer
	Private m_bProcessing As Boolean
	Private m_sDocFileExtension As String
	Private m_lDocumentTemplateId As Integer
	Private m_lDocumentTypeId As Integer
	Private m_sFileCopyMsg As String
	Private m_oWord As Object
	Private m_lWordHwnd As Integer
	Private m_sWordVersion As String
	Private m_sClientDocument As String
	Private m_oDocument As Object
    Dim m_oSiriusDocumentUtility As Object
	
	Private m_bWorking As Boolean
	
	Public ReadOnly Property ErrorDesc() As String
		Get
			ErrorDesc = m_sErrorDesc
		End Get
	End Property
	
	Public ReadOnly Property ErrorNumber() As Integer
		Get
            ' Return any error number that might have
			' occurred on the interface.
			ErrorNumber = m_lErrorNumber
        End Get
	End Property
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property
    ' We have a let status here, so if we're printing that we can set the status to PMOK
	Public Property Status() As Integer
		Get
            ' Return the interface exit status.
			Status = m_lStatus
        End Get
		Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
	End Property
	
	Public Property Task() As Short
		Get
            Task = m_iTask
        End Get
		Set(ByVal Value As Short)
            m_iTask = Value
        End Set
	End Property
	
	Public WriteOnly Property Navigate() As Integer
		Set(ByVal Value As Integer)
            m_lNavigate = Value
        End Set
	End Property
	
	Public WriteOnly Property ProcessMode() As Integer
		Set(ByVal Value As Integer)
            m_lProcessMode = Value
        End Set
	End Property
	
	Public WriteOnly Property TransactionType() As String
		Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
	End Property
	
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
	End Property
	
	
	Public Function GetBusiness() As Integer
		
		GetBusiness = gPMConstants.PMEReturnCode.PMTrue
		
		Dim lCnt As Integer
		Dim lstItem As System.Windows.Forms.ListViewItem
        m_lReturn = GetClient()
        m_lReturn = GetServer()
        Dim lDocumentTypeId As Integer
		Dim lDocumentTemplateId As Integer
        Dim sUserName As String
		
        m_lReturn = g_oBusiness.GetAllTemplates(r_vResultArray:=m_vTemplateArray)
		
		If Not IsArray(m_vTemplateArray) Then
			Exit Function
		End If
		
		For lCnt = 0 To UBound(m_vTemplateArray, 2)
            lDocumentTemplateId = ToSafeLong(m_vTemplateArray(0, lCnt))
            lDocumentTypeId = ToSafeLong(m_vTemplateArray(1, lCnt))
            sUserName = ToSafeString(m_vTemplateArray(7, lCnt))
            If lDocumentTemplateId <> 0 And lDocumentTypeId <> 0 Then
                lstItem = LVList.Items.Add("Code/" & CStr(lDocumentTypeId) & "/" & CStr(lDocumentTemplateId) & "/" & CStr(sUserName), ToSafeString(m_vTemplateArray(2, lCnt)), "")
  
                lstItem.SubItems.Add(ToSafeString(m_vTemplateArray(3, lCnt)))
                lstItem.SubItems.Add(" ")
            End If
        Next
    End Function
	
	Private Function ConvertHTMToXML() As Integer
		
		Try
		
		Dim sSTR() As String
		Dim lCnt As Integer
		Dim sValue As String
		Dim oFolder As Scripting.Folder
		Dim oFSO As Scripting.FileSystemObject
		Dim sUserName As String
		Dim sErrMsg As String
		Dim lPercentage As Integer
		
        If MsgBox("Please take a backup of folder -'" & m_sServer & "' before running this utility. Do you wish to convert HTM documents into XML?", MsgBoxStyle.YesNo, "HTM To XML Convertor") <> MsgBoxResult.Yes Then
            Exit Function
        End If
        If LVList.Items.Count = 0 Then
            Exit Function
        End If
        ConvertHTMToXML = gPMConstants.PMEReturnCode.PMTrue
        oFSO = New Scripting.FileSystemObject
        oFolder = oFSO.GetFolder(m_sClient)
        StatusBar1.Items.Item(3).Text = "Please wait, converting files..."
		
		ProgressBar1.Visible = True
		
		ProgressBar1.Minimum = 0
		ProgressBar1.Maximum = LVList.Items.Count
		
        For lCnt = 0 To LVList.Items.Count - 1

            sErrMsg = ""
            sSTR = Split(LVList.Items.Item(lCnt).Name, "/")

            If UBound(sSTR) > 0 Then
                m_lDocumentTypeId = ToSafeLong(sSTR(1))
                m_lDocumentTemplateId = ToSafeLong(sSTR(2))
                sUserName = ToSafeString(sSTR(3))
            End If

            m_lReturn = CopyServerToClient(sUserName, sErrMsg)

            LVList.Items.Item(lCnt).EnsureVisible()

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CopyClientToServer(sUserName)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    LVList.Items.Item(lCnt).SubItems(2).Text = "Successfully converted into XML"

                Else

                    LVList.Items.Item(lCnt).SubItems(2).Text = "Fails to copy file back to server."

                End If
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then

                LVList.Items.Item(lCnt).SubItems(2).Text = sErrMsg

            Else
                 
                LVList.Items.Item(lCnt).SubItems(2).Text = "Fails to convert into XML - " & sErrMsg

            End If

            System.Windows.Forms.Application.DoEvents()
            lPercentage = CInt((lCnt / LVList.Items.Count) * 100)

            StatusBar1.Items.Item(2).Text = lPercentage & "%"

            ProgressBar1.Value = lCnt
        Next
		
        StatusBar1.Items.Item(3).Text = "Please wait converting user signature files"
		
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
		
		m_lReturn = ConvertSignatureImages
		
        StatusBar1.Items.Item(2).Text = ""
        StatusBar1.Items.Item(3).Text = ""
		ProgressBar1.Value = 0
		ProgressBar1.Visible = False
		

		Catch ex As Exception
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to ConvertHTMToXML", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertHTMToXML", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
		Finally
            WriteRegistry(gpmConstants.HKEY_LOCAL_MACHINE, "Software\PM\SiriusSolutions\Setup\", "DocumentsConvertedToXML", Registry.InTypes.ValString, "1")
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
		End Try
    End Function

    Private Function ConvertSignatureImages() As Integer

        Dim oFSO As Object
        Dim sSignatureDir As String
        Dim sSignatureFile As String
        Dim sTmpFile As String
        Dim sXMLFile As String
        Dim sDirList As String
        Dim sSTR() As String

        ConvertSignatureImages = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="Signatures", r_sSettingValue:=sSignatureDir)


        If m_oWord Is Nothing Then
            m_lReturn = StartWord(r_oWord:=m_oWord, r_lWordHandle:=m_lWordHwnd, r_sWordVersion:=m_sWordVersion)
        End If

        oFSO = CreateObject("Scripting.FileSystemObject")

        sDirList = Dir(sSignatureDir)

        Do While sDirList <> ""
            sSTR = Split(sDirList, ".")

            If UBound(sSTR) = 0 Then
                sSignatureFile = sSignatureDir & sDirList

                sTmpFile = sSignatureDir & "TmpSign" & ".bmp"

                FileCopy(sSignatureFile, sTmpFile)

                sXMLFile = sSignatureDir & sDirList & ".xml"

                m_oWord.Documents.Add(Template:="Normal", NewTemplate:=False, DocumentType:=0)

                m_oWord.Selection.InlineShapes.AddPicture(FileName:=sTmpFile, LinkToFile:=False, SaveWithDocument:=True)

                m_oWord.ActiveDocument.SaveAs(FileName:=sXMLFile, FileFormat:=11)

                m_oWord.Visible = False

                m_oWord.ActiveDocument.Close()

                m_oWord.Visible = False

            End If
            sDirList = Dir()
        Loop

        CloseWord(m_oWord, m_lWordHwnd, False)

    End Function

    Public Function CopyServerToClient(ByVal v_sUsername As Object, ByRef r_sErrMsg As Object) As Integer

        Dim sServer As String
        Dim sClient As String
        Dim sTemp As String
        Dim sTemp2 As String
        Dim sMessage As String
        Dim oTemplate As Object

        Dim sSourceDocument As String
        Dim FS As New Scripting.FileSystemObject

        Const VB_FileAccessError As Short = 75

        Try

        CopyServerToClient = gPMConstants.PMEReturnCode.PMTrue
        If m_oWord Is Nothing Then
            m_lReturn = StartWord(r_oWord:=m_oWord, r_lWordHandle:=m_lWordHwnd, r_sWordVersion:=m_sWordVersion)
        End If
        ' Setup Zip Directory
        m_sZIP_DIRECTORY = ""

        m_lReturn = SetZipDirectory(v_sUsername)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyServerToClient = m_lReturn
            r_sErrMsg = "Failed to setup the default ZIP Directory"
        End If

        ' Create Client Work Folder
        m_lReturn = CreateFolderTree(m_sClient, True)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyServerToClient = m_lReturn
            r_sErrMsg = "Failed to create Client Work Directory. (" & m_sClient & ")"
            Exit Function
        End If


        sServer = m_sServer & "\Type " & m_lDocumentTypeId & "\Doc " & m_lDocumentTemplateId & ".zip"
        If m_lDocumentTypeId = 5 And Dir(sServer) = "" Then
            'Search in subdoc folder
            If Dir(m_sServer & "\Type " & lSUBDOC_TYPE_ID & "\Doc " & m_lDocumentTemplateId & ".zip") <> "" Then
                sServer = m_sServer & "\Type " & lSUBDOC_TYPE_ID & "\Doc " & m_lDocumentTemplateId & ".zip"
            End If
        End If

        sClient = m_sZIP_DIRECTORY & "\Doc " & m_lDocumentTemplateId & ".zip"

        'RWH(26/07/2000) Change to .htm file.
        sTemp = m_sZIP_DIRECTORY & "\Doc " & m_lDocumentTemplateId & "." & m_sDocFileExtension

        'Make sure the file's not there
        m_lReturn = DeleteFile(sTemp) ' RAM20040209 : Bug fix for PN Issue 10231
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyServerToClient = m_lReturn
            r_sErrMsg = "Failed to delete file [" & sTemp & "]"

            Exit Function
        End If
        m_sFileCopyMsg = ""
        'DC250304 PN11138 do not remove blank.zip file so do not delete sourcefile
        If FS.FileExists(sServer) Then
            m_lReturn = CopyFile(sServer, sClient, True, False, m_sFileCopyMsg)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyServerToClient = m_lReturn
                r_sErrMsg = "Failed to copy template from Server To Client."

                Exit Function
            End If

        Else
            CopyServerToClient = gPMConstants.PMEReturnCode.PMNotFound
            r_sErrMsg = "File does not exists."
            Exit Function
        End If

        ' Use the bPMDocFunctions UnZip Function.
        m_lReturn = UnZip(sClient, m_sClient, True) ' m_sZIP_DIRECTORY is a placeholder for Zip File ONLY
        ' m_sClient is the Unique Working Folder for Client for that document
        ' Also, UnZip directly unzip to that working folder
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyServerToClient = m_lReturn

            r_sErrMsg = "Failed to Unzip the Template"
            Exit Function
        End If

        'Convert from .HTM to .XML
        Dim oFSO As New Scripting.FileSystemObject
        If (CheckFileTypeIsHtml = gPMConstants.PMEReturnCode.PMTrue) Then

            oTemplate = m_oWord.Documents.Open(m_sClient & "\" & "Doc " & m_lDocumentTemplateId & ".htm")

            oTemplate.SaveAs(FileName:=m_sClient & "\" & "Doc " & m_lDocumentTemplateId & ".xml", FileFormat:=11)

            oTemplate.Close()

            sSourceDocument = m_sClient & "\" & "Doc " & m_lDocumentTemplateId & ".xml"

            If FS.FileExists(sSourceDocument) Then
                m_oSiriusDocumentUtility.Convert(sSourceDocument, sSourceDocument)
            Else

                'Give another shot...
                oTemplate = m_oWord.Documents.Open(m_sClient & "\" & "Doc " & m_lDocumentTemplateId & ".htm")
                oTemplate.SaveAs(m_sClient & "\" & "Doc " & m_lDocumentTemplateId & ".xml", CDbl(m_sWordVersion))
                oTemplate.Close()
                If FS.FileExists(sSourceDocument) Then
                    m_oSiriusDocumentUtility.Convert(sSourceDocument, sSourceDocument)
                End If

            End If
            oTemplate = Nothing
            ' SET 18/10/2004 ISS13245

            If oFSO.FolderExists(m_sClient & "\Doc " & m_lDocumentTemplateId & "_files") Then
                oFSO.DeleteFolder(m_sClient & "\Doc " & m_lDocumentTemplateId & "_files", True)
            End If

            'DJM 02/09/2002 : Remove old word document.
            sTemp = m_sClient & "\" & "Doc " & m_lDocumentTemplateId & ".htm"

            ' RAM20040209 : Removed unwanted Dir Command, PN Issue 10231
            m_lReturn = DeleteFile(sTemp)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                CopyServerToClient = gPMConstants.PMEReturnCode.PMFalse
                r_sErrMsg = "Failed to Delete File [" & sTemp & "]"

                Exit Function
            End If

        End If

        Exit Function

        Catch ex As Exception

        CopyServerToClient = gPMConstants.PMEReturnCode.PMError

        If Err.Number = VB_FileAccessError Then
            sMessage = "User does not have access to Document server: '" & m_sServer & "'"
        Else
            sMessage = "Failed to copy template from server to client"
        End If

        If oTemplate Is Nothing Then
        Else
            'UPGRADE_NOTE: Object oTemplate may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oTemplate = Nothing
        End If
        ' SET 18/10/2004 ISS13245
        m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyServerToClient", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

       
        End Try
    End Function

    Private Sub cmdQuit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdQuit.Click
        Me.Close()
    End Sub

    Private Sub cmdStart_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdStart.Click
        m_bWorking = True

        Me.cmdStart.Enabled = False

        m_lReturn = ConvertHTMToXML

        Me.cmdStart.Enabled = True

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            MsgBox("All valid documents have been converted into XML. Please see the status for details.", MsgBoxStyle.Information, "Document Conversion")
        End If

        m_bWorking = False
    End Sub

    Private Sub frmInterface_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        m_bProcessing = False

        m_sDocFileExtension = "xml"

        SetFormControls()

        m_lReturn = StartWord(r_oWord:=m_oWord, r_lWordHandle:=m_lWordHwnd, r_sWordVersion:=m_sWordVersion)

        m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

        m_oSiriusDocumentUtility = New SiriusDocumentUtility.Document

    End Sub


    Private Function GetClient() As Integer

        Try

        GetClient = gPMConstants.PMEReturnCode.PMTrue

        If (Trim(m_sClient) > "") Then
            Exit Function
        End If

        m_lReturn = GetClientDirectory(m_sClient, True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New Exception
        End If

        Exit Function

        Catch ex As Exception

        GetClient = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(sUserName:=g_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClient Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClient", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function
    Private Sub SetFormControls()

        StatusBar1.Items.Add(New System.Windows.Forms.ToolStripStatusLabel())
        StatusBar1.Items.Add(New System.Windows.Forms.ToolStripStatusLabel())
        StatusBar1.Items.Add(New System.Windows.Forms.ToolStripStatusLabel())

        StatusBar1.Items.Item(1).Width = VB6.TwipsToPixelsX(3600)
        StatusBar1.Items.Item(2).Width = VB6.TwipsToPixelsX(600)
        StatusBar1.Items.Item(3).Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.Width) - VB6.PixelsToTwipsX(StatusBar1.Items.Item(1).Width) + VB6.PixelsToTwipsX(StatusBar1.Items.Item(2).Width))

        ProgressBar1.Width = VB6.TwipsToPixelsX(3600)
        ProgressBar1.Height = StatusBar1.Height
        ProgressBar1.Top = StatusBar1.Top

        StatusBar1.Items.Item(1).TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        StatusBar1.Items.Item(1).TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        StatusBar1.Items.Item(2).TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        StatusBar1.Items.Item(2).TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        StatusBar1.Items.Item(3).TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        StatusBar1.Items.Item(3).TextAlign = System.Drawing.ContentAlignment.MiddleCenter

        LVList.LabelEdit = False
        LVList.FullRowSelect = True
        LVList.GridLines = False
        LVList.View = System.Windows.Forms.View.Details

        LVList.Columns.Clear()

        LVList.Columns.Add("", "Doc Code", CInt(VB6.TwipsToPixelsX(1500)))
        LVList.Columns.Add("", "Description", CInt(VB6.TwipsToPixelsX(2500)))
        LVList.Columns.Add("", "Status", CInt(VB6.TwipsToPixelsX(3600)))

        Me.ProgressBar1.Visible = False

    End Sub


    ' ***************************************************************** '
    ' Name: GetInterfaceDetails
    '
    ' Description: Gets the interface details and sets the appropriate
    '              sytle.
    '
    ' ***************************************************************** '
    Public Function GetInterfaceDetails() As Integer

        Try

        GetInterfaceDetails = gPMConstants.PMEReturnCode.PMTrue

        ' Check the task.
        If (Me.Task = gPMConstants.PMEComponentAction.PMEdit Or Me.Task = gPMConstants.PMEComponentAction.PMView Or Me.Task = gPMConstants.PMEComponentAction.PMDelete) Then
            ' Get the interface details from the
            ' business object.
            m_lReturn = Me.GetBusiness()

            ' Check for errors.
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to get the details.
                GetInterfaceDetails = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Check for errors
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to assign the details.
                GetInterfaceDetails = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
        End If

        m_lReturn = GetServer

        Exit Function

        Catch ex As Exception

        ' Error Section.

        GetInterfaceDetails = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function


        End Try
    End Function



    Private Function GetServer() As Integer

        Dim sServer As String

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

        GetServer = gPMConstants.PMEReturnCode.PMTrue

        If (Trim(m_sServer) > "") Then
            Exit Function
        End If

        eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
        eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

        sServer = ""

        m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="DocServer", r_sSettingValue:=sServer)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Server from Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetServer", vErrNo:=Err.Number, vErrDesc:=Err.Description)
            GetServer = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        Else
            m_sServer = sServer
        End If

        Exit Function

        Catch ex As Exception

        GetServer = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetServer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetServer", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CopyClientToServer
    '
    ' Description: copies the template from the client to the server
    '
    ' ***************************************************************** '
    Public Function CopyClientToServer(ByVal v_sUsername As Object) As Integer

        Dim sServer As String
        Dim sClient As String
        Dim sTemp As String

        Try

        CopyClientToServer = gPMConstants.PMEReturnCode.PMTrue

        'RWH(01/09/2000) RSAIB Process 108.
        '    UpdateTemplateNumberAndDependencies m_lOldDocumentTemplateId, m_lDocumentTemplateId

        m_lReturn = SetZipDirectory(v_sUsername)

        'RWH(04/09/2000) - RSAIB Process 108.
        'Use new absolute directory to zip & unzip files.
        CopyFilesToZipTemp()

        sServer = m_sServer & "\Type " & m_lDocumentTypeId

        m_lReturn = CreateFolderTree(sServer)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error Message
            CopyClientToServer = m_lReturn
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create destination Folder [" & sServer & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClientToServer", vErrNo:=Err.Number, vErrDesc:=Err.Description)
            Exit Function
        End If

        sServer = sServer & "\Doc " & m_lDocumentTemplateId & ".zip"

        'RWH(04/09/2000) - RSAIB Process 108.
        sClient = m_sZIP_DIRECTORY & "\Doc " & m_lDocumentTemplateId & ".zip"

        ' RAM20040206 : Use the Zip function in bPMDocFunctions.
        m_lReturn = Zip(sClient, "xml")
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error Message
            CopyClientToServer = m_lReturn
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Zip the files in [" & sClient & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClientToServer", vErrNo:=Err.Number, vErrDesc:=Err.Description)
            Exit Function
        End If

        ' RAM20040209 : Use bPMDocFunctions.CopyFile rather than FileCopy
        m_sFileCopyMsg = ""
        m_lReturn = CopyFile(sClient, sServer, True, True, m_sFileCopyMsg)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CopyClientToServer = m_lReturn
            ' Log Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy file from Client To Server." & vbCrLf & "Source File      : " & sClient & vbCrLf & "Destination File : " & sServer & vbCrLf & "Error Details    : " & m_sFileCopyMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClientToServer", vErrNo:=Err.Number, vErrDesc:=Err.Description)
            Exit Function
        End If

        Exit Function

        Catch ex As Exception

        CopyClientToServer = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy template from client to server", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClientToServer", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function




    ' Open a document, resolve the fields, and return the
    ' document object
    Private Function UpdateDocument(ByVal lFileNo As Integer, ByRef bUpdated As Boolean) As Integer

        Dim sFileName As String
        Dim sFileNameOutput As String
        Dim FSO As Scripting.FileSystemObject
        Dim tsTemplate As Scripting.TextStream
        Dim tsOutput As Scripting.TextStream
        Dim sLine As String

        Select Case Err.Number
            Case Is < 0
                Error (5)
            Case 1
                GoTo Err_UpdateDocument
        End Select

        UpdateDocument = gPMConstants.PMEReturnCode.PMTrue

        FSO = New Scripting.FileSystemObject

        bUpdated = False

        sFileName = m_sClient & "\" & "Doc " & lFileNo & ".htm"
        sFileNameOutput = sFileName & "2"

        'Open the chosen template document
        tsTemplate = FSO.OpenTextFile(sFileName, Scripting.IOMode.ForReading)
        tsOutput = FSO.CreateTextFile(sFileNameOutput)

        Do While tsTemplate.AtEndOfStream = False
            sLine = tsTemplate.ReadLine

            If InStr(sLine, "lang=EN-US") > 0 Then
                sLine = Replace(sLine, "lang=EN-US", "lang=EN-GB")
                bUpdated = True
            End If

            If InStr(sLine, "mso-ansi-language:EN-US") > 0 Then
                sLine = Replace(sLine, "mso-ansi-language:EN-US", "mso-ansi-language:EN-GB")
                bUpdated = True
            End If

            tsOutput.WriteLine(sLine)
        Loop

        tsTemplate.Close()
        tsOutput.Close()

        If bUpdated Then
            FSO.DeleteFile(sFileName)
            FSO.MoveFile(sFileNameOutput, sFileName)
        Else
            FSO.DeleteFile(sFileNameOutput)
        End If

        Exit Function

Err_UpdateDocument:

        UpdateDocument = gPMConstants.PMEReturnCode.PMError

        'We don't want to halt processing as process may be unattended.
        m_lErrorNumber = Err.Number
        m_sErrorDesc = Err.Description

    End Function


    Private Function CheckFileTypeIsHtml() As Integer
        Dim sFile As String = ""
        Dim lFileCount As Integer
        Dim lFileNum As Integer
        Dim sLine As String

        Try

        CheckFileTypeIsHtml = gPMConstants.PMEReturnCode.PMFalse

        'DJM 02/09/2002 : Convert files in the zip directory not in the client directory
        'RAM20040205    : Use FSO rather than the Dir Commands
        '                 Use m_sClient instead of m_sZIP_DIRECTORY, since that is the working directory
        lFileCount = NoOfFilesInDirectory(v_sDirectoryName:=m_sClient, r_vFirstFileName:=sFile)

        Select Case lFileCount

            Case Is > 1
                ' Too many files
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Too Many Files in " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileTypeIsHtml", vErrNo:=Err.Number, vErrDesc:=Err.Description)
                Exit Function
            Case 0
                Exit Function
            Case 1
                ' Only one file found, so check the type of the file
                lFileNum = FileSystem.FreeFile()
                FileSystem.FileOpen(lFileNum, m_sClient & "\" & sFile, OpenMode.Binary)
                sLine = InputString(lFileNum, 5)
                FileSystem.FileClose(lFileNum)
                Select Case UCase(sLine)
                    Case "<HTML"
                        CheckFileTypeIsHtml = gPMConstants.PMEReturnCode.PMTrue
                    Case Else
                        lFileNum = FreeFile
                        FileOpen(lFileNum, m_sClient & "\" & sFile, OpenMode.Binary)
                        sLine = InputString(lFileNum, 9)
                        FileClose(lFileNum)
                        If (UCase(Mid(sLine, 4, 5)) = "<HTML") Then CheckFileTypeIsHtml = gPMConstants.PMEReturnCode.PMTrue
                End Select

            Case Else
                ' Some other no, so error

                ' No Files Found
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No Files available in " & m_sClient, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileTypeIsHtml", vErrNo:=Err.Number, vErrDesc:=Err.Description)
                Exit Function
        End Select

            Exit Function

        Catch ex As Exception

        CheckFileTypeIsHtml = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckFileTypeIsHtml Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileTypeIsHtml", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function

    Private Sub frmInterface_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
        Try
            If m_bWorking Then
                Cancel = 1
                Exit Sub
            End If

            m_lReturn = CloseWord(m_oWord, lHandle:=m_lWordHwnd, bSaveChanges:=False)

            m_oSiriusDocumentUtility = Nothing
            Me.Hide()
            eventArgs.Cancel = Cancel
        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckFileTypeIsHtml", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
        End Try
    End Sub
    ' ***************************************************************** '
    '
    ' Name: CopyFilesToZipTemp
    '
    ' Description:
    '
    ' History: 04/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function CopyFilesToZipTemp() As Integer
        Dim sClient As String
        Dim sDependencyDir As String
        Dim sDependency As String
        Dim sParentFile As String
        Dim sDepDirName As String

        Dim sSourceFile As String
        Dim sDestinationFile As String
        Dim lFileCount As Integer

        Try

        CopyFilesToZipTemp = gPMConstants.PMEReturnCode.PMTrue

        'Copy files to ZipTemp.
        'This gives us an absolute directory to zip & unzip to as apposed to the
        'client-specific processing directory.

        lFileCount = NoOfFilesInDirectory(m_sClient, sClient, m_sDocFileExtension)
        If lFileCount > 0 Then

            'Copy parent file to ZipTemp.
            sSourceFile = m_sClient & "\" & sClient
            sDestinationFile = m_sZIP_DIRECTORY & "\" & sClient

            m_lReturn = CopyFile(sSourceFile, sDestinationFile, True, True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyFilesToZipTemp = gPMConstants.PMEReturnCode.PMError
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to Copy File." & vbCrLf & "From [" & sSourceFile & "] " & vbCrLf & "To [" & sDestinationFile & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFilesToZipTemp", vErrNo:=Err.Number, vErrDesc:=Err.Description)
            End If

        End If

        Exit Function

        Catch ex As Exception

        CopyFilesToZipTemp = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyFilesToZipTemp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFilesToZipTemp", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CopyFilesFromZipTemp
    '
    ' Description:
    '
    ' History: 04/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function CopyFilesFromZipTemp() As Integer

        Try

        CopyFilesFromZipTemp = gPMConstants.PMEReturnCode.PMTrue

        Exit Function

        Catch ex As Exception

        CopyFilesFromZipTemp = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyFilesFromZipTemp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFilesFromZipTemp", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function



    ' Edit History  :
    ' RAM20040227   :  Don't use Dir Commands, Use FSO
    Private Function SetZipDirectory(ByVal v_sUsername As String) As Integer

        Try

        Dim sTemp As String

        SetZipDirectory = gPMConstants.PMEReturnCode.PMTrue

        If (m_sZIP_DIRECTORY <> "") Then
            Exit Function
        End If

        ' SET 20/06/2003 ISS4571 - Fix for document locking
        'Get the DocZipTemp dir
        sTemp = gPMFunctions.ToSafeString(ReadRegistry(gPMConstants.HKEY_LOCAL_MACHINE, "SOFTWARE\Pure\PureInstallation\Client", "DocZipPMDir"))

        'Make sure we have an install path
        If (sTemp = "Not Found") Then
            ' Log Error Message
            iPMFunc.LogMessage(sUserName:=g_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetZipDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetZipDirectory", vErrNo:=Err.Number, vErrDesc:="Unable to find the registry entry for the doczip directory location")

            SetZipDirectory = gPMConstants.PMEReturnCode.PMError
            Exit Function
        End If

        ' SET 20/06/2003 ISS4571 - Fix for document locking
        m_sZIP_DIRECTORY = sTemp
        m_sZIP_DIRECTORY = m_sZIP_DIRECTORY & "\" & Trim(v_sUsername)

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' RAM20040227 : Removed all the Dir Commands, Use FSO - START
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' directory doesn't exist so attempt to create it
        m_lReturn = CreateFolderTree(m_sZIP_DIRECTORY, True)

        ' did we succeed...?
        If IsFolderExists(m_sZIP_DIRECTORY) <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(sUserName:=g_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetZipDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetZipDirectory", vErrNo:=0, vErrDesc:="Unable to create the directory (" & m_sZIP_DIRECTORY & ")")

            SetZipDirectory = gPMConstants.PMEReturnCode.PMError
            Exit Function
        End If
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' RAM20040227 : Removed all the Dir Commands, Use FSO - END
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Exit Function

        Catch ex As Exception

        SetZipDirectory = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(sUserName:=g_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetZipDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetZipDirectory", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try
    End Function



    Private Function LaunchOurDoc() As Integer
        Dim iFormat As Short

        'TN20010711
        Dim sWindowText As String

        Try

        LaunchOurDoc = gPMConstants.PMEReturnCode.PMTrue

        ' SET 18/10/2004 ISS13245 - launch word
        m_lReturn = StartWord(r_oWord:=m_oWord, r_lWordHandle:=m_lWordHwnd, r_sWordVersion:=m_sWordVersion)


        'Open current document.
        m_lReturn = OpenDocument()
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            LaunchOurDoc = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'For Debug.
        'm_oWord.Visible = True

        Exit Function

        Catch ex As Exception

        LaunchOurDoc = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LaunchOurDoc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LaunchOurDoc", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: OpenDocument
    '
    ' Description:
    '
    ' History: 28/09/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function OpenDocument() As Integer
        Dim dtPause As Date
        Dim sOptionValue As String

        Try

        OpenDocument = gPMConstants.PMEReturnCode.PMTrue

        ' Alix - 31/10/2002
        If VB.Left(m_sWordVersion, 1) = "8" Then

            ' Avoid message when saving document
            m_oWord.DefaultSaveFormat = "HTML"

            ' Open document, specifying HTML as default opening format
            m_oDocument = m_oWord.Documents.Open(FileName:=m_sClientDocument, Format_Renamed:=11)

        Else

            m_oDocument = m_oWord.Documents.Open(m_sClientDocument, ConfirmConversions:=False)
        End If


        System.Windows.Forms.Application.DoEvents()

        Exit Function

        Catch ex As Exception

        OpenDocument = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OpenDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenDocument", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function

    Private Sub LVList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LVList.SelectedIndexChanged

    End Sub
End Class
