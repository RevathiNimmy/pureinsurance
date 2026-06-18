Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles

Partial Friend Class mainForm
    Inherits System.Windows.Forms.Form

    Private Const BIF_RETURNONLYFSDIRS As Short = 1
    Private Const BIF_DONTGOBELOWDOMAIN As Short = 2
    Private Const MAX_PATH As Short = 260

    'UPGRADE_WARNING: Structure BrowseInfo may require marshalling attributes to be passed as an argument in this Declare statement. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"'
    Private Declare Function SHBrowseForFolder Lib "shell32" (ByRef lpbi As BrowseInfo) As Integer

    Private Declare Function SHGetPathFromIDList Lib "shell32" (ByVal pidList As Integer, ByVal lpBuffer As String) As Integer

    Private Declare Function lstrcat Lib "kernel32" Alias "lstrcatA" (ByVal lpString1 As String, ByVal lpString2 As String) As Integer


    Private Const ACClass As String = conEmptyString

    Private Structure BrowseInfo
        Dim hWndOwner As Integer
        Dim pIDLRoot As Integer
        Dim pszDisplayName As Integer
        Dim lpszTitle As Integer
        Dim ulFlags As Integer
        Dim lpfnCallback As Integer
        Dim lParam As Integer
        Dim iImage As Integer
    End Structure


    'Richard Clarke 23/02/2009 - new const for pie export file extension
    Private Const m_DefaultExportExtension As String = "cie"

    Private m_lReturn As Integer

    Private Sub cboDataModel_Click()
        Dim iDataModelId As Short
        Dim vResults(,) As Object

        Try

            'iDataModelId = mainForm.cboDataModel.ItemId
            g_DataModelId = iDataModelId
            g_DataModelCode = ""


            m_lReturn = g_oDatabase.SQLSelect(sSQL:="select code from gis_data_model where gis_data_model_id = " & iDataModelId, sSQLName:="genericEnquire", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If IsArray(vResults) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                g_DataModelCode = Trim(vResults(0, 0))
            End If
        Catch ex As Exception

        End Try
    End Sub

    'UPGRADE_WARNING: Event chkAdditionalExportOptions.CheckStateChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub chkAdditionalExportOptions_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkAdditionalExportOptions.CheckStateChanged
        Dim Index As Short = chkAdditionalExportOptions.GetIndex(eventSender)
        '    If Index = chkAdditionalExportOptions_Documents Then
        '        If chkAdditionalExportOptions(Index).value = 1 Then 'And g_bIsUnderwriting = False
        '            chkAdditionalExportOptions(chkAdditionalExportOptions_PBDocsOnly).Enabled = True
        '        Else
        '            chkAdditionalExportOptions(chkAdditionalExportOptions_PBDocsOnly).Enabled = False
        '        End If
        '    End If

        'Richard Clarke November 2008 - PIE enhancements
        'if they untick the UDL export option, disable the UDL treeview
        'Removed at SA request on 19/02/2009
        '    If Index = chkAdditionalExportOptions_UDLs Then
        '        If chkAdditionalExportOptions(Index).value = 1 Then
        '            tvUDL.Enabled = True
        '        Else
        '            tvUDL.Enabled = False
        '        End If
        '    End If

    End Sub

    'UPGRADE_WARNING: Event chkTarget.CheckStateChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub chkTarget_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkTarget.CheckStateChanged

        If chkTarget.CheckState = System.Windows.Forms.CheckState.Checked Then
            g_bNukeDataModel = True
        Else
            g_bNukeDataModel = False
        End If
    End Sub

    Private Sub cmdBackupBrowse_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdBackupBrowse.Click
        'set the dialog up to allow directory selection only

        'parse the common dialog into txtBackupDir
        Dim lpIDList As Integer
        Dim sBuffer As String
        Dim szTitle As String
        Dim tBrowseInfo As BrowseInfo

        szTitle = "Choose backup directory"
        With tBrowseInfo
            .hWndOwner = Me.Handle.ToInt32
            .lpszTitle = lstrcat(szTitle, "")
            .ulFlags = BIF_RETURNONLYFSDIRS + BIF_DONTGOBELOWDOMAIN
        End With

        lpIDList = SHBrowseForFolder(tBrowseInfo)

        If (lpIDList) Then
            sBuffer = Space(MAX_PATH)
            SHGetPathFromIDList(lpIDList, sBuffer)
            sBuffer = VB.Left(sBuffer, InStr(sBuffer, vbNullChar) - 1)
        End If

        txtBackupDir.Text = sBuffer

        'set the global backup path here
        g_sBackupPath = txtBackupDir.Text

    End Sub

    Private Sub cmdBrowse_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdBrowse.Click

        Dim fileDialog As New FolderBrowserDialog

        Try
            fileDialog.Description = "Choose export directory"
            fileDialog.ShowDialog()
            txtFilePath(1).Text = fileDialog.SelectedPath

            'added hard coded file extension at request of BAs
            'Richard Clarke 23/02/2008
            txtFileExtension(1).Text = m_DefaultExportExtension
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cmdBrowse2_Click()
        'ToDo: Populate dialogs with values

    End Sub

    Private Sub cmdDebug_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDebug.Click
        Dim Index As Short = cmdDebug.GetIndex(eventSender)

        If (Index = 0 Or Index = 3 Or Index = 4) And g_DataModelCode = "" Then
            MsgBox("Please set the data model on the export tab 2", 0, "Error")
            Exit Sub
        End If

        Select Case Index
            Case 1
                DoGuiDefaults(v_iImportExport:=g_iImportExport, v_bClearWarning:=True)

                'Richard Clarke November 2008 - PIE enhancements - uncomment to use test mode
                'with correct arguments (arrays instead of single values for data model id and code)
                '            InitialiseControlArrays r_oDatabase:=g_oDatabase, _
                ''                                    r_lDataModelId:=0, _
                ''                                    r_sDataModelCode:=conEmptyString, _
                ''                                    v_generateObjectConstants:=True, _
                ''                                    r_lSiriusUserId:=g_lSiriusUserId
            Case 0 'default lists
                'create the lookup flat files
                m_lReturn = CreateLookupFile(r_oDatabase:=g_oDatabase, v_sDataModelCode:=g_DataModelCode)

            Case 2
                m_lReturn = RegenerateCaptions(g_oDatabase)

            Case 3 'registry settings
                'do we need to do any registry processing
                m_lReturn = CreateRegistrySettings(v_sGISDataModel:=g_DataModelCode)

            Case 4 ' create XML
                m_lReturn = CreateDataSet(v_sGISDataModel:=g_DataModelCode)

        End Select




    End Sub

    Private Sub cmdExportCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExportCancel.Click

        If g_bExportInProgress Then
            g_bStopProcessing = True
            cmdExportCancel.Visible = False

            writeToStatusBox(("User request to cancel export process."))
        End If

        g_bExportInProgress = False

    End Sub


    Private Sub cmdImport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdImport.Click
        'Dim lDataModelId As Long
        'Dim sDataModelCode As String

        Dim lDataModelId() As Integer 'Richard Clarke November 2008 - PIE enhancements
        Dim tFilePath As String = Nothing
        Dim tFileName As String = Nothing
        Dim tFileExtension As String = Nothing

        Dim sDataModelCode() As String 'Richard Clarke November 2008 - PIE enhancements

        ReDim lDataModelId(0) 'Richard Clarke November 2008 - PIE enhancements
        ReDim sDataModelCode(0) 'Richard Clarke November 2008 - PIE enhancements

        tFilePath = txtFilePath(0).Text
        tFileName = txtFileName(0).Text
        tFileExtension = txtFileExtension(0).Text
        If MsgBox("Please ensure your Sirius database has been backed up. Click Ok to continue with the import.", MsgBoxStyle.OkCancel + MsgBoxStyle.Information, "PIE") = MsgBoxResult.Ok Then
        Else
            Exit Sub
        End If

        cmdImport.Enabled = False
        g_bStopProcessing = False

        g_iImportExport = 0

        SSTab1.TabPages.Item(0).Enabled = False
        SSTab3.TabPages.Item(0).Enabled = False
        SSTab3.TabPages.Item(1).Enabled = False
        SSTab3.TabPages.Item(2).Enabled = False

        'do UI defaults
        m_lReturn = DoGuiDefaults(v_iImportExport:=g_iImportExport, v_bClearWarning:=True)

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = SaveRegistrySettings(v_iImportExport:=g_iImportExport)
        End If

        'ToDo: Remove the below line for final release
        'SaveUserSettings()

        'Richard Clarke November 2008 - PIE Enhancements
        'we need to check the backup location is valid, then perform the backup.
        g_bFirstImport = False

        ' BSJ March 2009 - don't backup if option checked
        If Not (chkBackup.CheckState = System.Windows.Forms.CheckState.Checked) Then
            m_lReturn = BackupTarget()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'log that the backup failed and exit without doing the import
                LogAuditMessage("Target system backup failed, please check backup settings", True, False, "", False)
            End If
        End If
        ' BSJ March 2009

        'Richard Clarke November 2008 - PIE Enhancements - END
        If txtFilePath(0).Text = "" Then
            MsgBox("Import Path Name is not selected", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "PIE")
            SSTab3.SelectedIndex = 0
        ElseIf txtFileName(0).Text = "" Then
            MsgBox("Import File Name is empty.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "PIE")
            SSTab3.SelectedIndex = 0
        ElseIf txtFileExtension(0).Text = "" Then
            MsgBox("Import File Extension is empty.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "PIE")
            SSTab3.SelectedIndex = 0
        Else
            'Call the routine to produce the extract file
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = ImportExtractFile(oDatabase:=g_oDatabase, lDataModelId:=lDataModelId, sDataModelCode:=sDataModelCode, tFilePath:=tFilePath, tFileName:=tFileName, tFileExtension:=tFileExtension, v_bCheckHeader:=False, sVersionNumber:=conVersionNumber, v_bImportRegistry:=Me.optAdditionalImportOptions(optAdditionalImportOptions_ImportRegistry).Checked)
            End If
        End If

        cmdImport.Enabled = True

        SSTab1.TabPages.Item(0).Enabled = True
        SSTab3.TabPages.Item(0).Enabled = True
        SSTab3.TabPages.Item(1).Enabled = True
        SSTab3.TabPages.Item(2).Enabled = True

    End Sub

    Private Sub cmdExport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExport.Click

        Dim lDataModelId() As Integer 'Richard Clarke November 2008 - PIE enhancements
        'Dim lDataModelId As Long
        'Dim sDataModelCode As String
        Dim tFilePath As String = Nothing
        Dim tFileName As String = Nothing
        Dim tFileExtension As String = Nothing

        Dim sDataModelCode() As String 'Richard Clarke November 2008 - PIE enhancements

        cmdExport.Enabled = False

        tFilePath = txtFilePath(1).Text
        tFileName = txtFileName(1).Text
        tFileExtension = txtFileExtension(1).Text

        ReDim lDataModelId(0)
        ReDim sDataModelCode(0)

        g_iImportExport = 1
        g_bStopProcessing = False
        g_bExportInProgress = True

        SSTab1.TabPages.Item(1).Enabled = False
        SSTab2.TabPages.Item(0).Enabled = False
        SSTab2.TabPages.Item(1).Enabled = False
        SSTab2.TabPages.Item(2).Enabled = False
        cmdExportCancel.Visible = True

        If Not g_bStopProcessing Then
            m_lReturn = DoGuiDefaults(v_iImportExport:=g_iImportExport, v_bClearWarning:=True)
        End If

        If Not (g_bStopProcessing) And (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) Then
            m_lReturn = SaveRegistrySettings(v_iImportExport:=g_iImportExport)
        End If

        'Call the routine to produce the extract file
        If txtFilePath(1).Text = "" Then
            MsgBox("Export Path Name is not selected", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "PIE")
            SSTab2.SelectedIndex = 0
        ElseIf txtFileName(1).Text = "" Then
            MsgBox("Export File Name is empty.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "PIE")
            SSTab2.SelectedIndex = 0
        ElseIf txtFileExtension(1).Text = "" Then
            MsgBox("Export File Extension is empty.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "PIE")
            SSTab2.SelectedIndex = 0
        Else
            If Not (g_bStopProcessing) And (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) Then
                m_lReturn = ProduceExtractFile(oDatabase:=g_oDatabase, lDataModelId:=lDataModelId, sDataModelCode:=sDataModelCode, tFilePath:=tFilePath, tFileName:=tFileName, tFileExtension:=tFileExtension, sVersionNumber:=conVersionNumber)
            End If
        End If
        cmdExport.Enabled = True
        g_bExportInProgress = False

        cmdExportCancel.Visible = False
        SSTab1.TabPages.Item(1).Enabled = True
        SSTab2.TabPages.Item(0).Enabled = True
        SSTab2.TabPages.Item(1).Enabled = True
        SSTab2.TabPages.Item(2).Enabled = True

    End Sub

    Private Sub cmdImportBrowse_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdImportBrowse.Click
        Dim sFileDialog As String

        Dim sFilename As String

        On Error Resume Next

        'CommonDialog2Save.Title = "Import file"
        'CommonDialog2Save.ShowDialog()
        CommonDialog1Open.Title = "Import file"
        CommonDialog1Open.ShowDialog()

        'sFileDialog = CommonDialog2Save.FileName
        sFileDialog = CommonDialog1Open.FileName
        'populate the file path, filename and file extension controls

        'need to instrrev to trim the filename
        sFilename = Mid(CommonDialog1Open.FileName, InStrRev(CommonDialog1Open.FileName, "\") + 1)
        sFilename = VB.Left(sFilename, InStr(sFilename, ".") - 1)

        txtFilePath(0).Text = VB.Left(CommonDialog1Open.FileName, InStr(CommonDialog1Open.FileName, sFilename) - 1)
        txtFileName(0).Text = sFilename
        txtFileExtension(0).Text = Mid(CommonDialog1Open.FileName, InStrRev(CommonDialog1Open.FileName, ".") + 1)
    End Sub

    Private Sub cmdRestoreUsers_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdRestoreUsers.Click

        'Richard Clarke November 2008 - PIE enhancements.
        'if the user clicks this button, we need to reset all the users' logon flags back to their
        'original state.
        'just run the sql statement on the PMUser table set
        'update PM_User set is_deleted = 0 where previous_is_deleted = 0
        Call EnableUserLogins()
        'Richard Clarke Novmeber 2008 - PIE enhancements.

    End Sub

    Private Sub Dir1_Change(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Dir1.Change

        Try

            txtBackupDir.Text = Trim(Dir1.Path) & "\"



        Catch ex As Exception

        End Try
    End Sub

    Private Sub Drive1_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Drive1.SelectedIndexChanged

        Try

            Dir1.Path = Drive1.Drive

        Catch ex As Exception

        End Try
    End Sub

    'UPGRADE_NOTE: Form_Initialize was upgraded to Form_Initialize_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Private Sub Form_Initialize_Renamed()
        ShowFormInTaskBar_Attach()
    End Sub

    Private Sub mainForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Dim txtFilePath As String
        Dim txtFileName As String
        Dim txtFileExtension As String
        Dim sMode As String

        ShowFormInTaskBar_Detach()

        'AR 07-03-03 if the form was left with the "Export" tab showing,
        'the click event wouldnt get called at startup and the list
        'containing the datamodels wouldnt load - this is to ensure it does.
        SSTab1_SelectedIndexChanged(SSTab1, New System.EventArgs())

        If GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FilePath", r_sSettingValue:=txtFilePath) = gPMConstants.PMEReturnCode.PMTrue Then
        End If

        If GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FileName", r_sSettingValue:=txtFileName) = gPMConstants.PMEReturnCode.PMTrue Then
        End If

        If GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FileExtension", r_sSettingValue:=txtFileExtension) = gPMConstants.PMEReturnCode.PMTrue Then
        End If

        If GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_Mode", r_sSettingValue:=sMode) = gPMConstants.PMEReturnCode.PMTrue Then
        End If

        If sMode = "" Then sMode = CStr(pbIeMode_Default)

        ' g_bIsUnderwriting= True always for the Utlity
        g_bIsUnderwriting = True

        SetGUIOptionsBasedOnMode(CInt(sMode))

        'Richard Clarke 19/02/2009

        Me.Text = Me.Text & " " & conVersionNumber


        'load both the filename controls
        loadFilenameControls(v_sPath:=txtFilePath, v_sName:=txtFileName, v_sExtension:=txtFileExtension)

        'ensure GUI is correct
        m_lReturn = DoGuiDefaults(v_iImportExport:=1, v_bClearWarning:=True) 'set for export!

        'ensure tabs start on first tab
        SSTab1.SelectedIndex = 0
        SSTab2.SelectedIndex = 0
        SSTab3.SelectedIndex = 0

        Me.SSTab1.TabPages.Item(2).Visible = False
        g_bStopProcessing = False

        g_cParentPK = New Collection

        '----------------------------
        'Richard Clarke November 2008 - PIE enhancements
        If CheckPieAuditTableExists() = gPMConstants.PMEReturnCode.PMFalse Then
            'if we are importing and no pie audit table, must be first import
            If g_iImportExport = 0 Then
                g_bFirstImport = gPMConstants.PMEReturnCode.PMTrue
            End If
            CreatePIEAuditTable()
        End If

        'PIE enhancements November 2008
        'load the UDL tree nodes and the Data Models tree nodes
        LoadTreeViewNodes("UDL")
        LoadTreeViewNodes("DataModels")

        'check the previous settings exist or can't load them!
        If CheckForUserSettings() = gPMConstants.PMEReturnCode.PMTrue Then
            If MsgBox("Do you wish to re-use your previous PIE configuration settings?", MsgBoxStyle.YesNo + MsgBoxStyle.Information, "PIE") = MsgBoxResult.Yes Then
                m_lReturn = LoadUserSettings()
            Else
                'set the export tab details to blank
                Me.txtFilePath(1).Text = ""
                Me.txtFileName(1).Text = ""
                Me.txtFileExtension(1).Text = ""
                'set the import tab details to blank
                Me.txtFilePath(0).Text = ""
                Me.txtFileName(0).Text = ""
                Me.txtFileExtension(0).Text = ""
                Me.chkAdditionalExportOptions(0).CheckState = 0
                Me.chkAdditionalExportOptions(1).CheckState = 0
                Me.chkAdditionalExportOptions(2).CheckState = 0
                Me.chkAdditionalExportOptions(6).CheckState = 0
                Me.chkAdditionalExportOptions(7).CheckState = 0
                Me.chkAdditionalExportOptions(8).CheckState = 0
                Me.chkAdditionalExportOptions(9).CheckState = 0
            End If
        Else
            'set the export tab details to blank
            Me.txtFilePath(1).Text = ""
            Me.txtFileName(1).Text = ""
            Me.txtFileExtension(1).Text = ""
            'set the import tab details to blank
            Me.txtFilePath(0).Text = ""
            Me.txtFileName(0).Text = ""
            Me.txtFileExtension(0).Text = ""

        End If

        CheckVisibilityOfGenerateUMLScript()
        'Richard Clarke November 2008 - PIE enhancements END
        '----------------------------
    End Sub

    'UPGRADE_WARNING: Event mainForm.Resize may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub mainForm_Resize(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Resize

        ' BSJ Sep 2009
        ' PIE tool errors if minimised
        If WindowState = 1 Then
            Exit Sub
        End If

        'Richard Clarke January 2009 - amendments.
        'resize controls to fit form

        'first resize the parent tab control and frame1
        SSTab1.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) * 0.7)
        SSTab1.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.Width) - 300)
        'need to move frame 1 to just below sstab1
        Frame1.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(SSTab1.Top) + VB6.PixelsToTwipsY(SSTab1.Height) + 285)
        Frame1.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) * 0.18)
        Frame1.Width = SSTab1.Width

        'now we know the height of sstab1, we can resize sstab2 and sstab3
        SSTab2.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(SSTab1.Height) * 0.89)
        SSTab2.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(SSTab1.Width) - 300)

        SSTab3.Height = SSTab2.Height
        SSTab3.Width = SSTab2.Width

        'now we need to resize some of the controls
        'treeviews frames and textboxes.
        'Frame9.Left = Frame4.Left + Frame4.Width + 105
        'Frame9.Width = (SSTab2.Width - (Frame4.Width + 550)) / 2
        'Frame10.Left = SSTab2.Left + 105 'DO NOT SET THIS PROPERTY
        Frame10.Width = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(SSTab2.Width) / 3) - 155)

        Frame9.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Frame10.Left) + VB6.PixelsToTwipsX(Frame10.Width) + 105)
        Frame9.Width = Frame10.Width

        Frame5.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Frame9.Left) + VB6.PixelsToTwipsX(Frame9.Width) + 105)
        Frame5.Width = Frame9.Width

        tvUDL.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Frame9.Width) - 170)
        tvDataModel.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Frame10.Width) - 170)

        'text boxes
        fraExportHeader.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(SSTab2.Width) - 420)
        txtExportConfirmationText.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(fraExportHeader.Width) - 210)
        Frame6.Width = fraExportHeader.Width
        txtExportHeaderComment.Width = txtExportConfirmationText.Width

        'Richard Clarke Januray 2009 - amendments.

        'don't allow the form to go below a size of 11190 x 7380
        If VB6.PixelsToTwipsX(Me.Width) < 11190 Then
            Me.Width = VB6.TwipsToPixelsX(11190)
        End If

        If VB6.PixelsToTwipsY(Me.Height) < 7380 Then
            Me.Height = VB6.TwipsToPixelsY(7380)
        End If

    End Sub

    Private Sub mainForm_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        'Richard Clarke November 2008 - PIE enhancements - NEW
        'save the user settings
        SaveUserSettings()
        're enable the user logins
        EnableUserLogins()
        'Richard Clarke November 2008 - PIE enhancements - END
        End
    End Sub

    ' ***************************************************************** '
    '
    ' Name: loadFilenameControls
    '
    ' Description:
    '
    ' History: 27/09/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Sub loadFilenameControls(ByVal v_sPath As String, ByVal v_sName As String, ByVal v_sExtension As String)

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".loadFilenameControls")

        Try

            'load both the file detail controls
            txtFilePath(0).Text = v_sPath
            txtFileName(0).Text = v_sName
            txtFileExtension(0).Text = v_sExtension
            txtFilePath(1).Text = v_sPath
            txtFileName(1).Text = v_sName
            txtFileExtension(1).Text = v_sExtension

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".loadFilenameControls")



        Catch ex As Exception

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".loadFilenameControls")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="loadFilenameControls Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="loadFilenameControls", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)


        End Try
    End Sub

    Private Sub lblDebugOn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles lblDebugOn.Click
        Me.SSTab1.TabPages.Item(2).Visible = True
    End Sub

    Private Sub SSTab1_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles SSTab1.SelectedIndexChanged
        Static PreviousTab As Short = SSTab1.SelectedIndex()

        'ensure child tabs start on first tab
        If SSTab1.SelectedIndex = 0 Then
            SSTab2.SelectedIndex = 0
            g_iImportExport = 1
        End If
        If SSTab1.SelectedIndex = 1 Then
            SSTab3.SelectedIndex = 0
            g_iImportExport = 0
        End If

        PreviousTab = SSTab1.SelectedIndex()
    End Sub

    Private Sub SSTab2_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles SSTab2.SelectedIndexChanged
        Static PreviousTab As Short = SSTab2.SelectedIndex()

        If SSTab2.SelectedIndex = 1 Or SSTab2.SelectedIndex = 2 Then
            m_lReturn = BuildHeaderConfirmationText(txtConfirmationText:=txtExportConfirmationText, v_sVersionNumber:=conVersionNumber)
        End If

        PreviousTab = SSTab2.SelectedIndex()
    End Sub

    Private Sub SSTab3_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles SSTab3.SelectedIndexChanged
        Static PreviousTab As Short = SSTab3.SelectedIndex()

        If Not (SSTab3.TabPages.Item(2).Enabled) Then
            SSTab3.SelectedIndex = 3
            Exit Sub
        End If
        'this is the check import header tab click event
        'Richard Clarke November 2008 - PIE enhancements OLD
        'Dim lDataModelId As Long
        'Dim sDataModelCode As String

        Dim lDataModelId() As Integer 'Richard Clarke November 2008 - PIE enhancements NEW
        Dim sDataModelCode() As String 'Richard Clarke November 2008 - PIE enhancements NEW

        ReDim lDataModelId(0) 'Richard Clarke November 2008 - PIE enhancements NEW
        ReDim sDataModelCode(0) 'Richard Clarke November 2008 - PIE enhancements NEW

        Dim iMode As Short

        Dim tFilePath As String = Nothing
        Dim tFileName As String = Nothing
        Dim tFileExtension As String = Nothing
        tFilePath = txtFilePath(0).Text
        tFileName = txtFileName(0).Text
        tFileExtension = txtFileExtension(0).Text

        If SSTab3.SelectedIndex = 2 Then
            System.Windows.Forms.Application.DoEvents()

            iPMFunc.SetMousePointer((gPMConstants.PMEMousePointerStatus.PMMouseBusy))

            'set "import"
            g_iImportExport = 0

            'do UI dfaults
            m_lReturn = DoGuiDefaults(v_iImportExport:=g_iImportExport, v_bClearWarning:=True)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                iMode = pbIeMode_DataModel

                'Call the routine to produce the extract file
                If txtFilePath(0).Text = "" Then
                    If MsgBox("Import Path is not set, Do you wish to use import path from configuration settings?", MsgBoxStyle.YesNo + MsgBoxStyle.Information, "PIE") = MsgBoxResult.Yes Then

                        If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FilePath", r_sSettingValue:=txtFilePath(0).Text) = gPMConstants.PMEReturnCode.PMTrue Then
                        End If
                        'If GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FileName", r_sSettingValue:=txtFileName(0).Text) = gPMConstants.PMEReturnCode.PMTrue Then
                        'End If Or txtFileName(0).Text = "" Or txtFileExtension(0).Text = "" /File Name/Extn 

                        'If GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FileExtension", r_sSettingValue:=txtFileExtension(0).Text) = gPMConstants.PMEReturnCode.PMTrue Then
                        'End If
                        If txtFilePath(0).Text = "" Then
                            MsgBox("Import Path Name is not configured", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "PIE")
                            SSTab3.SelectedIndex = 0
                        ElseIf txtFileName(0).Text = "" Then
                            MsgBox("Import File Name is empty.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "PIE")
                            SSTab3.SelectedIndex = 0
                        ElseIf txtFileExtension(0).Text = "" Then
                            MsgBox("Import File Extension is empty.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "PIE")
                            SSTab3.SelectedIndex = 0
                        Else
                            m_lReturn = ImportExtractFile(oDatabase:=g_oDatabase, lDataModelId:=lDataModelId, sDataModelCode:=sDataModelCode, tFilePath:=tFilePath, tFileName:=tFileName, tFileExtension:=tFileExtension, v_bCheckHeader:=True, sVersionNumber:=conVersionNumber, v_bImportRegistry:=False)
                            'Richard Clarke November 2008 - PIE enhancements - MODIFIED
                            'added AND as we don't want to switch tab if we're on the manual changes tab
                            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse And SSTab3.SelectedIndex <> 1 Then
                                'show tab with warning message on it
                                SSTab3.SelectedIndex = 3
                            End If
                        End If


                    Else
                        SSTab3.SelectedIndex = 0
                    End If
                Else
                    If txtFilePath(0).Text = "" Then
                        MsgBox("Import Path Name is not configured.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "PIE")
                        SSTab3.SelectedIndex = 0
                    ElseIf txtFileName(0).Text = "" Then
                        MsgBox("Import File Name is empty.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "PIE")
                        SSTab3.SelectedIndex = 0
                    ElseIf txtFileExtension(0).Text = "" Then
                        MsgBox("Import File Extension is empty.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "PIE")
                        SSTab3.SelectedIndex = 0
                    Else
                        m_lReturn = ImportExtractFile(oDatabase:=g_oDatabase, lDataModelId:=lDataModelId, sDataModelCode:=sDataModelCode, tFilePath:=tFilePath, tFileName:=tFileName, tFileExtension:=tFileExtension, v_bCheckHeader:=True, sVersionNumber:=conVersionNumber, v_bImportRegistry:=False)
                        'Richard Clarke November 2008 - PIE enhancements - MODIFIED
                        'added AND as we don't want to switch tab if we're on the manual changes tab
                        If m_lReturn = gPMConstants.PMEReturnCode.PMFalse AndAlso SSTab3.SelectedIndex <> 1 Then
                            'show tab with warning message on it
                            SSTab3.SelectedIndex = 3
                        End If
                    End If
                End If
            End If

        iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End If

        PreviousTab = SSTab3.SelectedIndex()
    End Sub

    ' ***************************************************************** '
    '
    ' Name: SaveRegistrySettings
    '
    ' Description:
    '
    ' History: 27/09/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function SaveRegistrySettings(ByVal v_iImportExport As Short) As Integer

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".SaveRegistrySettings")

        Try

            SaveRegistrySettings = gPMConstants.PMEReturnCode.PMTrue

            If SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FilePath", v_sSettingValue:=txtFilePath(v_iImportExport).Text) <> gPMConstants.PMEReturnCode.PMTrue Then

                SaveRegistrySettings = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FileName", v_sSettingValue:=txtFileName(v_iImportExport).Text) <> gPMConstants.PMEReturnCode.PMTrue Then

                SaveRegistrySettings = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FileExtension", v_sSettingValue:=txtFileExtension(v_iImportExport).Text) <> gPMConstants.PMEReturnCode.PMTrue Then

                SaveRegistrySettings = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_Mode", v_sSettingValue:=CStr(buildExportMode())) <> gPMConstants.PMEReturnCode.PMTrue Then

                SaveRegistrySettings = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If





            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".SaveRegistrySettings")

            Exit Function

        Catch ex As Exception

            SaveRegistrySettings = gPMConstants.PMEReturnCode.PMError

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".SaveRegistrySettings")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveRegistrySettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveRegistrySettings", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: LoadTreeNodes
    '
    ' Description:
    '
    ' History: 05/11/2008 Richard Clarke - Created.
    ' PIE Enhancements November 2008
    ' ***************************************************************** '

    Public Function LoadTreeViewNodes(ByVal sSource As String) As Integer

        'For the UDLs we need to select name from sysobjects where tablename like 'UDL%'
        'select * from sysobjects where type = 'U' and category = 512 and name like 'UDL%'

        Dim lReturn As Integer
        Dim sSQL As String
        Dim vResults(,) As Object
        Dim iRow As Short
        Dim rootNode As System.Windows.Forms.TreeNode
        Dim childNode As System.Windows.Forms.TreeNode

        'query where we can pull the list of UDL table names or GISDataModels
        If sSource = "UDL" Then
            sSQL = "SELECT name FROM sysobjects WHERE type = 'U' AND name LIKE '" & sSource & "%'"
        Else
            'filter by hidden options option number and value = 1 joined to gis data model product option
            'so anything with a product option of null should be visible and also
            'and product option matching a hidden option with value = 1
            sSQL = "SELECT gis_data_model_id, [description] FROM gis_data_model WHERE product_option IS NULL"
            sSQL = sSQL & " UNION"
            sSQL = sSQL & " SELECT gis_data_model_id, [description] FROM gis_data_model WHERE product_option"
            sSQL = sSQL & " IN (SELECT option_number FROM hidden_options WHERE value = '1')"
        End If

        ' Execute SQL Statement
        lReturn = g_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="LoadTreeViewNodes", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
        'ensure the query returned successfully
        If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            LoadTreeViewNodes = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'ensure we have results
        If IsArray(vResults) Then
            If UBound(vResults) < 0 Then
                LoadTreeViewNodes = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            'now set the treeview control up
            If sSource = "UDL" Then
                With tvUDL
                    'add the root node
                    rootNode = .Nodes.Add("Root", "All")
                    For iRow = LBound(vResults, 2) To UBound(vResults, 2)
                        'add each node to the parent
                        'UPGRADE_WARNING: Couldn't resolve default property of object vResults(0, iRow). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Add method behavior has changed Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="DBD08912-7C17-401D-9BE9-BA85E7772B99"'
                        childNode = .Nodes.Find("Root", True)(0).Nodes.Add("Child" & CStr(iRow), vResults(0, iRow))
                    Next iRow
                End With
            Else 'Source must be DataModels
                With tvDataModel
                    'add the root node
                    rootNode = .Nodes.Add("Root", "All")
                    For iRow = LBound(vResults, 2) To UBound(vResults, 2)
                        'add each node to the parent
                        'UPGRADE_WARNING: Couldn't resolve default property of object vResults(1, iRow). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Add method behavior has changed Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="DBD08912-7C17-401D-9BE9-BA85E7772B99"'
                        childNode = .Nodes.Find("Root", True)(0).Nodes.Add("Child" & CStr(vResults(0, iRow)), vResults(1, iRow))
                    Next iRow
                End With
            End If
            'show all nodes
            rootNode.Expand()
        End If

    End Function

    Private Sub tvDataModel_AfterCheck(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.TreeViewEventArgs) Handles tvDataModel.AfterCheck
        Dim node As System.Windows.Forms.TreeNode = eventArgs.Node
        Dim iNode As Short
        Dim iNodeCount As Integer = objFrmMainForm.tvDataModel.Nodes(0).Nodes.Count - 1

        'if the user clicked the parent node
        If node.Text = "All" Then
            'loop round the children and set to checked / unchecked as appropriate
            For iNode = 0 To iNodeCount
                If node.Checked = True Then
                    'UPGRADE_WARNING: Lower bound of collection tvDataModel.Nodes has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    tvDataModel.Nodes(0).Nodes.Item(iNode).Checked = True
                Else
                    'UPGRADE_WARNING: Lower bound of collection tvDataModel.Nodes has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    tvDataModel.Nodes(0).Nodes.Item(iNode).Checked = False
                End If
            Next iNode
        End If

        tvDataModel.Refresh()
    End Sub

    Private Sub tvUDL_AfterCheck(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.TreeViewEventArgs) Handles tvUDL.AfterCheck
        Dim node As System.Windows.Forms.TreeNode = eventArgs.Node
        Dim iNode As Short
        Dim iNodeCount As Integer = objFrmMainForm.tvUDL.Nodes(0).Nodes.Count - 1
        'if the user clicked the parent node
        If node.Text = "All" Then
            'loop round the children and set to checked / unchecked as appropriate
            For iNode = 0 To iNodeCount
                If node.Checked = True Then
                    'UPGRADE_WARNING: Lower bound of collection tvUDL.Nodes has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    tvUDL.Nodes(0).Nodes.Item(iNode).Checked = True
                Else
                    'UPGRADE_WARNING: Lower bound of collection tvUDL.Nodes has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    tvUDL.Nodes(0).Nodes.Item(iNode).Checked = False
                End If
            Next iNode
        End If

        tvUDL.Refresh()

    End Sub
    Private Sub txtBackupDir_KeyUp(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyEventArgs) Handles txtBackupDir.KeyUp
        Dim KeyCode As Short = eventArgs.KeyCode
        Dim Shift As Short = eventArgs.KeyData \ &H10000
        g_sBackupPath = txtBackupDir.Text
    End Sub

    Public Sub StatusBar_TextWrite(ByVal statusbartext As String, ByVal statusbar As Integer)
        If statusbar = 0 Then
            _StatusBar1_0_Panel1.Text = statusbartext
        ElseIf statusbar = 1 Then
            _StatusBar1_1_Panel1.Text = statusbartext
        ElseIf statusbar = 2 Then
            _StatusBar1_2_Panel1.Text = statusbartext
        End If
        Application.DoEvents()
    End Sub

    Private Sub chkIncludeUMLs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIncludeUMLs.CheckedChanged
        Call CheckVisibilityOfGenerateUMLScript()
    End Sub

    Sub CheckVisibilityOfGenerateUMLScript()
        If chkIncludeUMLs.Checked Then
            chkGenerateUMLScript.Visible = False
            chkGenerateUMLScript.Checked = False
        Else
            chkGenerateUMLScript.Visible = True

        End If
    End Sub
End Class