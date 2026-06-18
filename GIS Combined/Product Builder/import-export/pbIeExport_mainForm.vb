Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
Imports SharedFiles

Partial Friend Class mainForm
    Inherits System.Windows.Forms.Form
    Private Sub mainForm_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
        End If
    End Sub

    Private Const ACClass As String = conEmptyString

    Private m_lReturn As gPMConstants.PMEReturnCode

    Private Sub cboDataModel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDataModel.Click
        Dim iDataModelId As Integer
        Dim vResults(,) As Object

        Try

            iDataModelId = Me.cboDataModel.ItemId

            g_DataModelId = iDataModelId
            g_DataModelCode = ""

            m_lReturn = g_oDatabase.SQLSelect(sSQL:="select code from gis_data_model where gis_data_model_id = " & iDataModelId, sSQLName:="genericEnquire", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If Information.IsArray(vResults) Then

                g_DataModelCode = CStr(vResults(0, 0)).Trim()
            End If

        Catch
        End Try

    End Sub

    Private Sub chkAdditionalExportOptions_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _chkAdditionalExportOptions_6.CheckStateChanged, _chkAdditionalExportOptions_5.CheckStateChanged, _chkAdditionalExportOptions_4.CheckStateChanged, _chkAdditionalExportOptions_3.CheckStateChanged, _chkAdditionalExportOptions_2.CheckStateChanged, _chkAdditionalExportOptions_1.CheckStateChanged, _chkAdditionalExportOptions_0.CheckStateChanged
        Dim Index As Integer = Array.IndexOf(chkAdditionalExportOptions, eventSender)
        If Index = chkAdditionalExportOptions_Documents Then
            chkAdditionalExportOptions(chkAdditionalExportOptions_PBDocsOnly).Enabled = chkAdditionalExportOptions(Index).CheckState = CheckState.Checked And Not g_bIsUnderwriting
        End If
    End Sub

    Private Sub cmdDebug_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdDebug_4.Click, _cmdDebug_3.Click, _cmdDebug_2.Click, _cmdDebug_1.Click, _cmdDebug_0.Click
        Dim Index As Integer = Array.IndexOf(cmdDebug, eventSender)

        If (Index = 0 Or Index = 3 Or Index = 4) And g_DataModelCode = "" Then
            MessageBox.Show("Please set the data model on the export tab 2", "Error", MessageBoxButtons.OK)
            Exit Sub
        End If

        Select Case Index
            Case 1
                DoGuiDefaults(v_iImportExport:=g_iImportExport, v_bClearWarning:=True)

                InitialiseControlArrays(r_oDatabase:=g_oDatabase, r_lDataModelId:=0, r_sDataModelCode:=conEmptyString, v_generateObjectConstants:=True, r_lSiriusUserId:=g_lSiriusUserId)
            Case 0 'default lists
                'create the lookup flat files
                m_lReturn = CType(CreateLookupFile(r_oDatabase:=g_oDatabase, v_sDataModelCode:=g_DataModelCode), gPMConstants.PMEReturnCode)

            Case 2
                m_lReturn = CType(RegenerateCaptions(g_oDatabase), gPMConstants.PMEReturnCode)

            Case 3 'registry settings
                'do we need to do any registry processing
                m_lReturn = CType(CreateRegistrySettings(v_sGISDataModel:=g_DataModelCode), gPMConstants.PMEReturnCode)

            Case 4 ' create XML
                m_lReturn = CType(CreateDataSet(v_sGISDataModel:=g_DataModelCode), gPMConstants.PMEReturnCode)

        End Select

    End Sub

    Private Sub cmdExportCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExportCancel.Click

        If g_bExportInProgress Then
            g_bStopProcessing = True
            cmdExportCancel.Visible = False

            writeToStatusBox("User request to cancel export process.")
        End If

        g_bExportInProgress = False

    End Sub

    Private Sub cmdImport_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdImport.Click
        Dim lDataModelId As Integer
        Dim sDataModelCode As String = ""

        cmdImport.Enabled = False
        g_bStopProcessing = False

        g_iImportExport = 0

        SSTabHelper.SetTabEnabled(SSTab1, 0, False)
        SSTabHelper.SetTabEnabled(SSTab3, 0, False)
        SSTabHelper.SetTabEnabled(SSTab3, 1, False)

        'do UI defaults
        m_lReturn = CType(DoGuiDefaults(v_iImportExport:=g_iImportExport, v_bClearWarning:=True), gPMConstants.PMEReturnCode)

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = CType(SaveRegistrySettings(v_iImportExport:=g_iImportExport), gPMConstants.PMEReturnCode)
        End If

        'Call the routine to produce the extract file
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = CType(ImportExtractFile(oDatabase:=g_oDatabase, lDataModelId:=lDataModelId, sDataModelCode:=sDataModelCode, tFilePath:=txtFilePath(0).Text, tFileName:=txtFileName(0).Text, tFileExtension:=txtFileExtension(0).Text, v_bCheckHeader:=False, sVersionNumber:=conVersionNumber, v_bImportRegistry:=Me.optAdditionalImportOptions(optAdditionalImportOptions_ImportRegistry).Checked), gPMConstants.PMEReturnCode)
        End If

        cmdImport.Enabled = True

        SSTabHelper.SetTabEnabled(SSTab1, 0, True)
        SSTabHelper.SetTabEnabled(SSTab3, 0, True)
        SSTabHelper.SetTabEnabled(SSTab3, 1, True)

    End Sub

    Private Sub cmdExport_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExport.Click

        Dim lDataModelId As Integer
        Dim sDataModelCode As String = ""
        lDataModelId = Me.cboDataModel.ItemId
        cmdExport.Enabled = False

        g_iImportExport = 1
        g_bStopProcessing = False
        g_bExportInProgress = True

        SSTabHelper.SetTabEnabled(SSTab1, 1, False)
        SSTabHelper.SetTabEnabled(SSTab2, 0, False)
        SSTabHelper.SetTabEnabled(SSTab2, 1, False)
        SSTabHelper.SetTabEnabled(SSTab2, 2, False)
        cmdExportCancel.Visible = True

        If Not g_bStopProcessing Then
            m_lReturn = CType(DoGuiDefaults(v_iImportExport:=g_iImportExport, v_bClearWarning:=True), gPMConstants.PMEReturnCode)
        End If

        If Not (g_bStopProcessing) And (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) Then
            m_lReturn = CType(SaveRegistrySettings(v_iImportExport:=g_iImportExport), gPMConstants.PMEReturnCode)
        End If

        'Call the routine to produce the extract file
        If Not (g_bStopProcessing) And (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) Then
            m_lReturn = CType(ProduceExtractFile(oDatabase:=g_oDatabase, lDataModelId:=lDataModelId, sDataModelCode:=sDataModelCode, tFilePath:=txtFilePath(1).Text, tFileName:=txtFileName(1).Text, tFileExtension:=txtFileExtension(1).Text, sVersionNumber:=conVersionNumber), gPMConstants.PMEReturnCode)
        End If

        cmdExport.Enabled = True
        g_bExportInProgress = False

        cmdExportCancel.Visible = False
        SSTabHelper.SetTabEnabled(SSTab1, 1, True)
        SSTabHelper.SetTabEnabled(SSTab2, 0, True)
        SSTabHelper.SetTabEnabled(SSTab2, 1, True)
        SSTabHelper.SetTabEnabled(SSTab2, 2, True)

    End Sub

    Private Sub Form_Initialize_Renamed()
        iPMFunc.ShowFormInTaskBar_Attach()
        'Me.cboDataModel.FirstItem = ""
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
    Public Sub txtWarning_TextWrite(ByVal message As String, ByVal txtwarninglist As Integer)
        If txtwarninglist = 1 Then
            _txtWarning_1.Text = message
            _txtWarning_1.Refresh()
        ElseIf txtwarninglist = 0 Then
            _txtWarning_0.Text = message
            _txtWarning_0.Refresh()
        End If
        Application.DoEvents()
    End Sub
    Private Sub mainForm_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim txtFilePath, txtFileName, txtFileExtension, sMode, sHiddenOption As String
        Me.cboDataModel.FirstItem = ""

        iPMFunc.ShowFormInTaskBar_Detach()

        'AR 07-03-03 if the form was left with the "Export" tab showing,
        'the click event wouldnt get called at startup and the list
        'containing the datamodels wouldnt load - this is to ensure it does.
        SSTab1_SelectedIndexChanged(SSTab1, New EventArgs())

        If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FilePath", r_sSettingValue:=txtFilePath) = gPMConstants.PMEReturnCode.PMTrue Then
        End If

        If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FileName", r_sSettingValue:=txtFileName) = gPMConstants.PMEReturnCode.PMTrue Then
        End If

        If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FileExtension", r_sSettingValue:=txtFileExtension) = gPMConstants.PMEReturnCode.PMTrue Then
        End If

        If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_Mode", r_sSettingValue:=sMode) = gPMConstants.PMEReturnCode.PMTrue Then
        End If

        If sMode = "" Then sMode = CStr(pbIeMode_Default)

        iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTUnderwriting, v_vBranch:=g_iSourceID, r_vUnderwriting:=sHiddenOption)

        If sHiddenOption = "U" Then
            g_bIsUnderwriting = True
        End If

        SetGUIOptionsBasedOnMode(CInt(sMode))

        'load both the filename controls
        loadFilenameControls(v_sPath:=txtFilePath, v_sName:=txtFileName, v_sExtension:=txtFileExtension)

        'ensure GUI is correct
        m_lReturn = CType(DoGuiDefaults(v_iImportExport:=1, v_bClearWarning:=True), gPMConstants.PMEReturnCode) 'set for export!
        'ensure tabs start on first tab
        SSTabHelper.SetSelectedIndex(SSTab1, 0)
        SSTabHelper.SetSelectedIndex(SSTab2, 0)
        SSTabHelper.SetSelectedIndex(SSTab3, 0)

        SSTabHelper.SetTabVisible(Me.SSTab1, 2, False)
        g_bStopProcessing = False

        g_cParentPK = New Collection()

    End Sub

    Private Sub mainForm_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        Environment.Exit(0)
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

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".loadFilenameControls")

        Try

            'load both the file detail controls
            txtFilePath(0).Text = v_sPath
            txtFileName(0).Text = v_sName
            txtFileExtension(0).Text = v_sExtension
            txtFilePath(1).Text = v_sPath
            txtFileName(1).Text = v_sName
            txtFileExtension(1).Text = v_sExtension

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".loadFilenameControls")

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".loadFilenameControls")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="loadFilenameControls Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="loadFilenameControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lblDebugOn_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lblDebugOn.Click
        SSTabHelper.SetTabVisible(Me.SSTab1, 2, True)
    End Sub

    Private Sub SSTab1_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles SSTab1.SelectedIndexChanged

        'ensure child tabs start on first tab
        If SSTabHelper.GetSelectedIndex(SSTab1) = 0 Then
            SSTabHelper.SetSelectedIndex(SSTab2, 0)
            g_iImportExport = 1
        End If
        If SSTabHelper.GetSelectedIndex(SSTab1) = 1 Then
            SSTabHelper.SetSelectedIndex(SSTab3, 0)
            g_iImportExport = 0
        End If
        'Modified by Vijay Pal on 5/27/2010 1:57:11 PM comment the line according the vb code
        'SSTab1PreviousTab = SSTab1.SelectedIndex
    End Sub

    Private Sub SSTab2_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles SSTab2.SelectedIndexChanged

        If SSTabHelper.GetSelectedIndex(SSTab2) = 1 Or SSTabHelper.GetSelectedIndex(SSTab2) = 2 Then
            m_lReturn = CType(BuildHeaderConfirmationText(txtConfirmationText:=txtExportConfirmationText, v_sVersionNumber:=conVersionNumber), gPMConstants.PMEReturnCode)
        End If
        'Modified by Vijay Pal on 5/27/2010 1:57:44 PM comment the line according the vb code
        'SSTab2PreviousTab = SSTab2.SelectedIndex
    End Sub

    Private Sub SSTab3_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles SSTab3.SelectedIndexChanged

        'this is the check import header tab click event
        Dim lDataModelId As Integer
        Dim sDataModelCode As String = ""
        Dim iMode As Integer

        If SSTabHelper.GetSelectedIndex(SSTab3) = 1 Then
            Application.DoEvents()

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'set "import"
            g_iImportExport = 0

            'do UI dfaults
            m_lReturn = CType(DoGuiDefaults(v_iImportExport:=g_iImportExport, v_bClearWarning:=True), gPMConstants.PMEReturnCode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                iMode = pbIeMode_DataModel

                'Call the routine to produce the extract file
                m_lReturn = CType(ImportExtractFile(oDatabase:=g_oDatabase, lDataModelId:=lDataModelId, sDataModelCode:=sDataModelCode, tFilePath:=txtFilePath(0).Text, tFileName:=txtFileName(0).Text, tFileExtension:=txtFileExtension(0).Text, v_bCheckHeader:=True, sVersionNumber:=conVersionNumber, v_bImportRegistry:=False), gPMConstants.PMEReturnCode)

                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    'show tab with warning message on it
                    SSTabHelper.SetSelectedIndex(SSTab3, 2)
                End If
            End If

            iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End If

        SSTab3PreviousTab = SSTab3.SelectedIndex
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
    Public Function SaveRegistrySettings(ByVal v_iImportExport As Integer) As Integer

        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".SaveRegistrySettings")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FilePath", v_sSettingValue:=txtFilePath(v_iImportExport).Text) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FileName", v_sSettingValue:=txtFileName(v_iImportExport).Text) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FileExtension", v_sSettingValue:=txtFileExtension(v_iImportExport).Text) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_Mode", v_sSettingValue:=CStr(buildExportMode())) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".SaveRegistrySettings")

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".SaveRegistrySettings")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveRegistrySettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveRegistrySettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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
        sFilename = sFilename.Substring(0, InStr(sFilename, ".") - 1)


        txtFilePath(0).Text = CommonDialog1Open.FileName.Substring(0, InStr(CommonDialog1Open.FileName, sFilename) - 1)
        txtFileName(0).Text = sFilename
        txtFileExtension(0).Text = Mid(CommonDialog1Open.FileName, InStrRev(CommonDialog1Open.FileName, ".") + 1)
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