Option Strict Off
Option Explicit On
Imports SharedFiles

Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 12 July 2000
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '
    Private Const ACClass As String = "MainModule"
    ' Declare an instance of the Business object.
    Public g_oBusiness As Object
    Public g_vUserData As Object

    'Richard Clarke NEW November 2008
    Public g_usersLoggedOn As Integer
    Public Const g_PIEAuditTable As String = "pie_audit" 'const representing the audit table name
    Public Const g_PIEGuidCol As String = "pie_guid" 'const representing the PIE GUID column
    Public Const g_PIELastUpdatedCol As String = "pie_last_updated" 'const representing the PID LAST_UPDATED column
    Public Const g_PIEGuidColDef As String = "Varchar(50) NULL" 'const representing guid column definition
    Public Const g_PIELastUpdatedColDef As String = "DateTime NULL" 'const representing last_updated column definition
    Public Const g_PieLastUpdatedTrigger As String = "_pie_last_updated" 'const representing the PIE trigger name
    Public Const g_PieAuditTimestampCol As String = "[audit_date]" 'const representing the PIE Audit table timestamp column
    Public Const g_MinTimeoutForBackupRestore As Short = 600
    Public s_aUDLSelected() As String
    Public g_UserIDs As Object 'to store the user ids of people who can log on to the system
    Public g_bManualChangesChecked As Boolean
    Public g_bControlArraysInitialised As Boolean
    Public g_sBackupPath As String
    Public g_sBackupFilename As String
    Public g_bErrorInImport As Boolean
    Public g_bDocumentTemplateExtracted As Boolean
    Public g_bPropertyDerivedLookupExtracted As Boolean
    Public g_sLastSQLCommandGenerated As String
    'Richard Clarke NEW November 2008

    ' BSJ Sep 2009 - Used in logic for gis_object
    Public g_lOriginalParentID As Integer
    Public g_lNewParentID As Integer
    Public g_bNukeDataModel As Boolean

    Dim fso As New Scripting.FileSystemObject
    Dim fld As Scripting.Folder

    Public Function CheckUsersLoggedOn() As Integer
        Dim oForm As frmUsers
        Dim lReturn As Integer
        Dim iFilenum As Short

        ' RDC 15072002 delete file
        'On Error Resume Next
        Try
            Kill("c:\UsersLoggedOn.txt")
        Catch
        End Try

        Try

            g_oBusiness = New bPMLicenceAdmin.LicenceAdmin 'CreateObject("bPMLicenceAdmin.LicenceAdmin")

            ' Call the initialise method.
            'UPGRADE_WARNING: Couldn't resolve default property of object g_oBusiness.Initialise. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            lReturn = g_oBusiness.Initialise()
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Log Error.
                MsgBox("Unable to Connect to Sirius Architecture Database" & vbCrLf & "UsersLoggedOn will be shut down.", MsgBoxStyle.Critical, "Critical Error")
                Exit Function
            End If

            'call the function selectdata from the business
            'UPGRADE_WARNING: Couldn't resolve default property of object g_oBusiness.Selectdata. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            lReturn = g_oBusiness.Selectdata(r_vUserDataArray:=g_vUserData)
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Log Error.
                MsgBox("Unable to Read Data from Sirius Architecture Database" & vbCrLf & "UsersLoggedOn will be shut down.", MsgBoxStyle.Critical, "Critical Error")
                Exit Function
            End If

            oForm = New frmUsers

            oForm.ShowDialog()
            CheckUsersLoggedOn = g_usersLoggedOn
            'UPGRADE_NOTE: Object oForm may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oForm = Nothing

            Exit Function

        Catch ex As Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Main Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckUsersLoggedOn", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
    End Function

    'Function returns true / false depending on whether
    'this users's reg key values are found
    Public Function CheckForUserSettings() As Integer

        Dim sExportPath As String
        Dim lReturn As Integer

        CheckForUserSettings = gPMConstants.PMEReturnCode.PMFalse

        lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FilePath", r_sSettingValue:=sExportPath)

        If Trim(sExportPath) <> "" Then
            CheckForUserSettings = gPMConstants.PMEReturnCode.PMTrue
        End If

    End Function

    Public Function FindFile(ByVal sFol As String, ByRef sFile As String) As System.Collections.Generic.List(Of String)

        'Dim tFld As Scripting.Folder           
        'Dim FileName As String
        Dim sResult As New System.Collections.Generic.List(Of String)

        Dim fileArr As String()

        Try

            fileArr = System.IO.Directory.GetFiles(sFol, sFile, IO.SearchOption.AllDirectories)
            For Each s As String In fileArr
                sResult.Add(s)
            Next

            Return sResult

        Catch ex As Exception




        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SaveUserSettings
    '
    ' Description: This function saves the user settings from the registry
    '
    ' Added November 2008 Richard Clarke - PIE enhancements
    ' ***************************************************************** '
    Public Function SaveUserSettings() As Integer
        Dim lReturn As Integer
        Dim iDrive As Short
        'save the export tab / control settings
        'the below 4 settings are already saved to another registry location
        'but we need them in current user.
        lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FilePath", v_sSettingValue:=objFrmMainForm.txtFilePath(1).Text)

        lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FileName", v_sSettingValue:=objFrmMainForm.txtFileName(1).Text)

        lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FileExtension", v_sSettingValue:=objFrmMainForm.txtFileExtension(1).Text)

        lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_Mode", v_sSettingValue:=CStr(buildExportMode()))


        'data model treeview
        lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_DataModelChosen", v_sSettingValue:=g_sDataModelIDs)


        'the additional options checkboxes are an array
        lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_ExportRegSettings", v_sSettingValue:=CStr(objFrmMainForm.chkAdditionalExportOptions(0).CheckState))

        lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_ExportDocTemplates", v_sSettingValue:=CStr(objFrmMainForm.chkAdditionalExportOptions(1).CheckState))


        lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_ExportPRuleFiles", v_sSettingValue:=CStr(objFrmMainForm.chkAdditionalExportOptions(2).CheckState))


        lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_ExportUDLs", v_sSettingValue:=CStr(objFrmMainForm.chkAdditionalExportOptions(6).CheckState))

        lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_ExportUserAuthRules", v_sSettingValue:=CStr(objFrmMainForm.chkAdditionalExportOptions(7).CheckState))

        lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_ExportSPUICCS", v_sSettingValue:=CStr(objFrmMainForm.chkAdditionalExportOptions(8).CheckState))


        lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_ExportReports", v_sSettingValue:=CStr(objFrmMainForm.chkAdditionalExportOptions(9).CheckState))

        lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_ImportFilePath", v_sSettingValue:=objFrmMainForm.txtFilePath(0).Text)

        lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_ImportFileName", v_sSettingValue:=objFrmMainForm.txtFileName(0).Text)

        lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_ImportFileExtension", v_sSettingValue:=objFrmMainForm.txtFileExtension(0).Text)

        'get the backup location from the registry
        lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_TargetBackupPath", v_sSettingValue:=objFrmMainForm.txtBackupDir.Text)


        lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_TargetBackupDirectory", v_sSettingValue:=g_sBackupPath)

    End Function


    ' ***************************************************************** '
    ' Name: LoadUserSettings
    '
    ' Description: This function loads the user settings from the registry
    '
    ' Added November 2008 Richard Clarke - PIE enhancements
    ' ***************************************************************** '
    Public Function LoadUserSettings() As Integer
        Dim lReturn As Integer

        'load the export tab / control settings
        lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FilePath", r_sSettingValue:=objFrmMainForm.txtFilePath(1).Text)

        lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FileName", r_sSettingValue:=objFrmMainForm.txtFileName(1).Text)

        lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FileExtension", r_sSettingValue:=objFrmMainForm.txtFileExtension(1).Text)

        ' the exportmode cannot be loaded like this.
        'lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=pmeRSRCurrentUser, _
        'v_lPMEProductFamily:=pmePFSiriusArchitecture, _
        'v_lPMERegSettingLevel:=pmeRSLCommon, _
        'v_sSettingName:="PB_IE_Mode", _
        'r_sSettingValue:=buildExportMode)

        'load all the controls for the data model then the app can call
        'buildexportmode safely later

        'data model
        'lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=pmeRSRCurrentUser, _
        ''                    v_lPMEProductFamily:=pmePFSiriusArchitecture, _
        ''                    v_lPMERegSettingLevel:=pmeRSLCommon, _
        ''                    v_sSettingName:="PB_IE_DataModel", _
        ''                    r_sSettingValue:=objFrmMainForm.radioExportBasedOn(radioExportBasedOn_DataModel))

        'If objFrmMainForm.radioExportBasedOn(radioExportBasedOn_DataModel) = 1 Then
        'do we only need to do this if datamodel is true?
        '    lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=pmeRSRCurrentUser, _
        ''                        v_lPMEProductFamily:=pmePFSiriusArchitecture, _
        ''                        v_lPMERegSettingLevel:=pmeRSLCommon, _
        ''                        v_sSettingName:="PB_IE_DataModelChosen", _
        ''                        r_sSettingValue:=sDataModelsChosen)

        'Richard Clarke November 2008 - PIE enhancements - OLD
        'now we have the chosen data model, loop round the combo and set the correct index
        '        Dim iCount As Integer
        '        Dim lItemID As Long
        '        For iCount = 0 To objFrmMainForm.cboDataModel.ListCount - 1
        '            lItemID = objFrmMainForm.cboDataModel.ItemData(iCount)
        '            If objFrmMainForm.cboDataModel.ItemCaption(lItemID) = sDataModelChosen Then
        '                objFrmMainForm.cboDataModel.ItemId = lItemID
        '            End If
        '        Next

        'Richard Clarke November 2008 - PIE enhancements - NEW
        'need to tick the tvDataModel nodes if they're in the user settings from reg
        'UPGRADE_WARNING: Couldn't resolve default property of object v_iDataModelIDs. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'v_iDataModelIDs = Split(sDataModelsChosen, ",")
        'For iCount = 0 To UBound(v_iDataModelIDs)
        '    'UPGRADE_WARNING: Couldn't resolve default property of object v_iDataModelIDs(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        '    objFrmMainForm.tvDataModel.Nodes.Item("Child" & v_iDataModelIDs(iCount)).Checked = True
        'Next iCount
        'Richard Clarke November 2008 - PIE enhancements - END
        'End If

        'scheme
        'lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=pmeRSRCurrentUser, _
        ''                    v_lPMEProductFamily:=pmePFSiriusArchitecture, _
        ''                    v_lPMERegSettingLevel:=pmeRSLCommon, _
        ''                    v_sSettingName:="PB_IE_Scheme", _
        ''                    r_sSettingValue:=objFrmMainForm.radioExportBasedOn(radioExportBasedOn_Scheme))

        'screen
        'lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=pmeRSRCurrentUser, _
        ''                    v_lPMEProductFamily:=pmePFSiriusArchitecture, _
        ''                    v_lPMERegSettingLevel:=pmeRSLCommon, _
        ''                   v_sSettingName:="PB_IE_Screen", _
        ''                    r_sSettingValue:=objFrmMainForm.radioExportBasedOn(radioExportBasedOn_Screen))

        'migration
        'lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=pmeRSRCurrentUser, _
        ''                    v_lPMEProductFamily:=pmePFSiriusArchitecture, _
        ''                    v_lPMERegSettingLevel:=pmeRSLCommon, _
        ''                    v_sSettingName:="PB_IE_Migration", _
        ''                    r_sSettingValue:=objFrmMainForm.radioExportBasedOn(radioExportBasedOn_Migration))

        lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_ExportRegSettings", r_sSettingValue:=objFrmMainForm.chkAdditionalExportOptions(0).CheckState)

        lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_ExportDocTemplates", r_sSettingValue:=objFrmMainForm.chkAdditionalExportOptions(1).CheckState)
        'PBdocs only is a sub setting of Export Document Templates?
        '    lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=pmeRSRCurrentUser, _
        ''                        v_lPMEProductFamily:=pmePFSiriusArchitecture, _
        ''                        v_lPMERegSettingLevel:=pmeRSLCommon, _
        ''                        v_sSettingName:="PB_IE_ExportPBDocsOnly", _
        ''                        r_sSettingValue:=objFrmMainForm.chkAdditionalExportOptions(5).value)

        lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_ExportPRuleFiles", r_sSettingValue:=objFrmMainForm.chkAdditionalExportOptions(2).CheckState)

        '    lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=pmeRSRCurrentUser,
        '                       v_lPMEProductFamily:=pmePFSiriusArchitecture, _
        ''                        v_lPMERegSettingLevel:=pmeRSLCommon, _
        ''                        v_sSettingName:="PB_IE_ExportRiskGroupsCodes", _
        ''                        r_sSettingValue:=objFrmMainForm.chkAdditionalExportOptions(3).value)

        '    lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=pmeRSRCurrentUser, _
        ''                        v_lPMEProductFamily:=pmePFSiriusArchitecture, _
        ''                        v_lPMERegSettingLevel:=pmeRSLCommon, _
        ''                        v_sSettingName:="PB_IE_Export3DLookups", _
        ''                        r_sSettingValue:=objFrmMainForm.chkAdditionalExportOptions(4).value)

        Dim sChecked As String


        lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_ExportUDLs", r_sSettingValue:=sChecked)

        objFrmMainForm.chkAdditionalExportOptions(6).CheckState = CShort(sChecked)

        lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_ExportUserAuthRules", r_sSettingValue:=sChecked)

        objFrmMainForm.chkAdditionalExportOptions(7).CheckState = CShort(sChecked)

        lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_ExportSPUICCS", r_sSettingValue:=sChecked)

        objFrmMainForm.chkAdditionalExportOptions(8).CheckState = CShort(sChecked)

        lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_ExportReports", r_sSettingValue:=sChecked)

        objFrmMainForm.chkAdditionalExportOptions(9).CheckState = CShort(sChecked)


        'load the import tab / control settings
        lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_ImportFilePath", r_sSettingValue:=objFrmMainForm.txtFilePath(0).Text)

        lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_ImportFileName", r_sSettingValue:=objFrmMainForm.txtFileName(0).Text)

        lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_ImportFileExtension", r_sSettingValue:=objFrmMainForm.txtFileExtension(0).Text)

        Dim sBackupPath As String

        'get the backup location from the registry
        lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_TargetBackupPath", r_sSettingValue:=sBackupPath)

        objFrmMainForm.txtBackupDir.Text = sBackupPath

    End Function


    ' ***************************************************************** '
    ' Name: BackupTarget
    '
    ' Description:'Function checks the backup location specified by the user exists
    ' and backs up the GIS Files, PMDOCS, PM Registry
    '
    ' Added November 2008 Richard Clarke - PIE enhancements
    ' ***************************************************************** '

    Public Function BackupTarget() As Integer

        Dim oFSO As Object
        Dim sBackupPath As String
        Dim sBackupFilename As String
        Dim sPMPath As String
        Dim oFSOTxt As Object
        Dim regFile As Scripting.File
        Dim testFile As Scripting.File
        Dim rootFolder As Scripting.Folder
        Dim lReturn As Integer
        Dim oReturn As Object
        Dim sPMDocPath As String
        Dim sUserSPUPath As String
        Dim sSPUReportPath As String
        Dim sPMReportPath As String

        On Error GoTo Err_BackupTarget

        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        'objFrmMainForm.StatusBar1(0).Items.Item(1).Text = "Backing up Target system prior to Import"
        objFrmMainForm.StatusBar_TextWrite("Backing up Target system prior to Import", 0)
        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = ""
        objFrmMainForm.StatusBar_TextWrite("", 1)
        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = "Backing up directories"
        objFrmMainForm.StatusBar_TextWrite("Backing up directories", 2)

        objFrmMainForm.ProgressBar1(0).Value = 1
        objFrmMainForm.ProgressBar1(1).Value = 20

        'Check the user specified a directory in the form first
        If Trim(objFrmMainForm.txtBackupDir.Text) = "" Then
            MsgBox("Please specify a backup directory on the first tab before attempting an Import", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "PIE")
            BackupTarget = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If
        'get the backup path from the form
        sBackupPath = Trim(objFrmMainForm.txtBackupDir.Text)
        oFSO = CreateObject("Scripting.FileSystemObject")
        'now check that the directory exists and is accessible by the application
        'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.FolderExists. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Not oFSO.FolderExists(sBackupPath) Then
            On Error Resume Next
            'doesn't exist, can we create it.
            'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.CreateFolder. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            oFSO.CreateFolder(sBackupPath)
            'check to see if we created the folder
            If Err.Number <> 0 Then
                'there was an error, we can't perform the backup, warn the user and return false
                MsgBox("The target backup cannot be carried out. Please ensure the application can access the backup location before continuing", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "PIE")
                BackupTarget = gPMConstants.PMEReturnCode.PMFalse
                On Error GoTo 0
                Exit Function
            End If
        End If
        'DN Added
        If Right(sBackupPath, 1) <> "\" Then
            sBackupPath = sBackupPath & "\"
        End If

        On Error Resume Next
        'ok, we've created the folder, can we create files in it?
        'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.CreateTextFile. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        oFSOTxt = oFSO.CreateTextFile(sBackupPath & "test.txt", True)
        'UPGRADE_WARNING: Couldn't resolve default property of object oFSOTxt.Close. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        oFSOTxt.Close()
        If Err.Number <> 0 Then
            'there was an error, we can't perform the backup, warn the user and return false
            MsgBox("The target backup cannot be carried out. Please ensure the application can access the backup location before continuing", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "PIE")
            BackupTarget = gPMConstants.PMEReturnCode.PMFalse
            On Error GoTo 0
            Exit Function

        End If
        'we successfully created the text file - delete it.
        'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.DeleteFile. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        oFSO.DeleteFile(sBackupPath & "test.txt", True)
        On Error GoTo 0

        'now delete previous copy of the backup folders / files
        'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.FolderExists. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If oFSO.FolderExists(sBackupPath & "GIS") Then
            'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.GetFolder. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            rootFolder = oFSO.GetFolder(sBackupPath & "GIS")
            rootFolder.Delete((True))
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.FolderExists. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If oFSO.FolderExists(sBackupPath & "PMDocs") Then
            'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.GetFolder. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            rootFolder = oFSO.GetFolder(sBackupPath & "PMDocs")
            rootFolder.Delete((True))
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.FileExists. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If oFSO.FileExists(sBackupPath & "PMReg\" & "PMReg.reg") Then
            'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.GetFile. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            regFile = oFSO.GetFile(sBackupPath & "PMReg\" & "PMReg.reg")
            regFile.Delete((True))
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.FolderExists. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If oFSO.FolderExists(sBackupPath & "PMReg") Then
            'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.GetFolder. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            rootFolder = oFSO.GetFolder(sBackupPath & "PMReg")
            rootFolder.Delete((True))
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.FolderExists. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If oFSO.FolderExists(sBackupPath & "Database") Then
            'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.GetFolder. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            rootFolder = oFSO.GetFolder(sBackupPath & "Database")
            rootFolder.Delete((True))
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.FolderExists. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If oFSO.FolderExists(sBackupPath & "Reports") Then
            'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.GetFolder. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            rootFolder = oFSO.GetFolder(sBackupPath & "Reports")
            rootFolder.Delete((True))
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.CreateFolder. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        oFSO.CreateFolder(sBackupPath & "Database")
        objFrmMainForm.ProgressBar1(1).Value = 100

        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = "Backing up Database"
        objFrmMainForm.StatusBar_TextWrite("Backing up Database", 2)

        objFrmMainForm.ProgressBar1(0).Value = 30
        objFrmMainForm.ProgressBar1(1).Value = 10

        'back up the database.
        BackupDatabase(sBackupFilename, sBackupPath & "Database") '- Richard Clarke uncomment this line for final UAT

        objFrmMainForm.ProgressBar1(0).Value = 70
        objFrmMainForm.ProgressBar1(1).Value = 0

        'Now backup the directories we need
        'get the paths to the directories we need to copy from the registry
        'HKEY_LOCAL_MACHINE\Software\PM\PMDir & "\PM"
        lReturn = GetPMRegSetting(HKEY_LOCAL_MACHINE, 0, gPMConstants.PMERegSettingLevel.pmeRSLBase, "PMDIR", sPMPath)
        'doc path
        lReturn = GetPMRegSetting(gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, gPMConstants.PMEProductFamily.pmePFSiriusSolutions, gPMConstants.PMERegSettingLevel.pmeRSLClient, "DOCSERVER", sPMDocPath)
        'report path
        lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="Reports", r_sSettingValue:=sPMReportPath)

        'now backup the folders / registry
        'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.CopyFolder. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        oFSO.CopyFolder(sPMPath & "\PM\GIS", sBackupPath & "GIS", True)
        'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.CopyFolder. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        oFSO.CopyFolder(sPMDocPath, sBackupPath & "PMDocs", True)
        sPMReportPath = Left(sPMReportPath, Len(sPMReportPath) - 1)
        'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.CopyFolder. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        oFSO.CopyFolder(sPMReportPath, sBackupPath & "Reports", True)

        'create the reg file
        'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.CreateFolder. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        oFSO.CreateFolder(sBackupPath & "PMReg\")
        'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.CreateTextFile. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        oFSOTxt = oFSO.CreateTextFile(sBackupPath & "PMReg\" & "PMReg.reg", True)
        'UPGRADE_WARNING: Couldn't resolve default property of object oFSOTxt.Close. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        oFSOTxt.Close()
        'export the registry tree
        lReturn = Shell("regedit.exe /e " & Chr(34) & sBackupPath & "PMReg\PMReg.reg" & Chr(34) & " " & Chr(34) & "HKEY_LOCAL_MACHINE\Software\PM" & Chr(34), AppWinStyle.Hide)

        'we now need to create the directory for the user SPUs
        sUserSPUPath = sBackupPath & "spuICCS\"
        'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.FolderExists. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If oFSO.FolderExists(sUserSPUPath) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.GetFolder. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            rootFolder = oFSO.GetFolder(sUserSPUPath)
            rootFolder.Delete((True))
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.CreateFolder. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        oFSO.CreateFolder(sUserSPUPath)

        'we now need to create the directory for the user spuReports
        sSPUReportPath = sBackupPath & "spuReport\"
        'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.FolderExists. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If oFSO.FolderExists(sSPUReportPath) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.GetFolder. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            rootFolder = oFSO.GetFolder(sSPUReportPath)
            rootFolder.Delete((True))
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.CreateFolder. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        oFSO.CreateFolder(sSPUReportPath)

        On Error GoTo 0
        BackupTarget = gPMConstants.PMEReturnCode.PMTrue

        Exit Function

Err_BackupTarget:
        'log the error to the audit table
        BackupTarget = gPMConstants.PMEReturnCode.PMError
        Exit Function
    End Function

    ' ***************************************************************** '
    ' Name: CheckTargetForManualChanges
    '
    ' Description: Get and display all of the form's default values.
    ' Added November 2008 Richard Clarke - PIE enhancements
    ' ***************************************************************** '
    Public Function CheckTargetForManualChanges(ByRef r_oDatabase As dPMDAO.Database, ByRef g_aIeControl() As Object) As Integer

        Dim lReturn As Integer
        Dim sSQL As String
        Dim vResults(,) As Object
        Dim iTableIndex As Integer
        Dim sTableName As String
        Dim iRowIndex As Integer
        Dim vDateResults(,) As Object
        Dim lastImport As Date
        Dim vTablesResults(,) As Object

        'Using initialise-control-arrays as basis for how to loop over the tables
        'we need to check for changes more recent than last_successful import in Audit table
        objFrmMainForm.txtManualChanges.Text = ""
        'we only need to do this if there is a PIE_Audit table though, otherwise nothing to compare against

        'check the PIE_Audit table exists first otherwise this is the first time the new
        'IMPORT PROCESS has been run against this target
        'the pie audit table is created in the main form load function

        CheckTargetForManualChanges = gPMConstants.PMEReturnCode.PMFalse

        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        'objFrmMainForm.StatusBar1(0).Items.Item(1).Text = "Checking target for manual changes"
        objFrmMainForm.StatusBar_TextWrite("Checking target for manual changes", 0)
        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "Tables"
        objFrmMainForm.StatusBar_TextWrite("Tables", 1)
        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = ""
        objFrmMainForm.StatusBar_TextWrite("", 2)

        sSQL = "SELECT name FROM sysobjects WHERE type = 'U' AND name = '" & g_PIEAuditTable & "'"
        lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CheckTargetForManualChanges", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
        'ensure the query returned successfully
        If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            CheckTargetForManualChanges = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'ensure we have results - if not, then this is the first time the new Import process has been run
        If Not IsArray(vResults) Then
            CheckTargetForManualChanges = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If
        If UBound(vResults, 2) >= 0 Then
            objFrmMainForm.ProgressBar1(1).Value = 100 / ((UBound(g_aIeControl) + 1) / (iTableIndex + 1))

            'get the last updated time from the audit table
            sSQL = "SELECT max(" & g_PieAuditTimestampCol & ") FROM " & g_PIEAuditTable & " WHERE Import = 1 AND successful = 1"
            lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CheckTargetForManualChanges", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vDateResults)

            If IsArray(vDateResults) AndAlso UBound(vDateResults, 2) >= 0 AndAlso Not (IsDBNull(vDateResults(0, 0)) OrElse IsNothing(vDateResults(0, 0))) Then
                lastImport = vDateResults(0, 0)
            Else
                CheckTargetForManualChanges = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            'now we need to get a list of every table that has a pie_last_updated column
            sSQL = "select table_name from INFORMATION_SCHEMA.COLUMNS where column_name = 'pie_last_updated' order by table_name asc"
            lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CheckTargetForManualChanges", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vTablesResults)

            'special table case - gis_find_mapping, gis_screen_detail - add to array
            ReDim Preserve vTablesResults(0, UBound(vTablesResults, 2) + 2)
            'UPGRADE_WARNING: Couldn't resolve default property of object vTablesResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vTablesResults(0, UBound(vTablesResults, 2) - 1) = "gis_find_mapping"
            'UPGRADE_WARNING: Couldn't resolve default property of object vTablesResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vTablesResults(0, UBound(vTablesResults, 2)) = "gis_screen_detail"

            'loop around the table list that have a pie_last_updated column
            For iTableIndex = 0 To UBound(vTablesResults, 2)

                System.Windows.Forms.Application.DoEvents()

                'UPGRADE_WARNING: Couldn't resolve default property of object vTablesResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sTableName = vTablesResults(0, iTableIndex)
                'no need for pmlogicaldatabase here
                If LCase(sTableName) <> LCase("pmlogicaldatabase") Then

                    'get the table name
                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = sTableName
                    objFrmMainForm.StatusBar_TextWrite(sTableName, 2)
                    'get all rows updated since the last successful import
                    sSQL = "SELECT " & g_PIEGuidCol & ", " & g_PIELastUpdatedCol & " FROM " & sTableName & " WHERE " & g_PIELastUpdatedCol & " > '" & lastImport & "' ORDER BY " & g_PIELastUpdatedCol & " DESC"
                    'loop round results and for each result that's newer, log it to the audit table
                    lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CheckTargetForManualChanges", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)

                    If IsArray(vResults) Then
                        'loop around array and add a log message
                        For iRowIndex = 0 To UBound(vResults, 2)
                            System.Windows.Forms.Application.DoEvents()
                            'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            LogAuditMessage("Manual change detected in table " & sTableName & " for row with GUID " & vResults(0, iRowIndex), True, False, "", gPMConstants.PMEReturnCode.PMFalse)
                            'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            objFrmMainForm.txtManualChanges.Text = objFrmMainForm.txtManualChanges.Text & "Manual change detected in table " & sTableName & " for row with GUID " & CStr(vResults(0, iRowIndex)) & vbCrLf
                            CheckTargetForManualChanges = gPMConstants.PMEReturnCode.PMTrue
                            System.Windows.Forms.Application.DoEvents()
                        Next iRowIndex
                    End If
                End If
                System.Windows.Forms.Application.DoEvents()
            Next iTableIndex

        End If

    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateGUIDs
    '
    ' Description: This function creates the GUIDs for tables
    '           that don't already have them on the source database
    '
    ' History: November 2008 Richard Clarke - Created.
    '
    ' ***************************************************************** '
    Public Function CreateGUIDs(ByRef r_oDatabase As dPMDAO.Database, ByRef g_aIeControl() As Object) As Integer

        Try

            'we need to check the table has guid_id column for each table first
            'if it doesn't have the guid_id column then we need to create it

            Dim lReturn As Integer
            Dim sSQL As String
            Dim vResults(,) As Object
            Dim iTableIndex As Integer
            Dim sTableName As String

            'Using initialise-control-arrays as basis for how to loop over the tables
            'we need to check each table to see if it has a guid column already. If not, alter table
            'add the guid_id column and the last_updated column, and add the last_updated table trigger
            'and the default value for the guid column

            'loop around the control array itself
            For iTableIndex = 0 To UBound(g_aIeControl)

                'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                'objFrmMainForm.StatusBar1(0).Items.Item(1).Text = "Checking source for GUIDs"
                objFrmMainForm.StatusBar_TextWrite("Checking source for GUIDs", 0)
                'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "Tables"
                objFrmMainForm.StatusBar_TextWrite("Tables", 1)
                'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = ""
                objFrmMainForm.StatusBar_TextWrite("", 2)

                'is this a table object?
                Select Case g_aIeControl(iTableIndex)(pbIeControl_objectType)
                    Case pbIeOt_dbTable_fixed, pbIeOt_dbTable_userdefined, pbIeOt_dbTable_child, pbIeOt_RiskGroupsCodes
                        'get the table name
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl(iTableIndex)(pbIeControl_objectName). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = g_aIeControl(iTableIndex)(pbIeControl_objectName)
                        objFrmMainForm.StatusBar_TextWrite(g_aIeControl(iTableIndex)(pbIeControl_objectName), 2)
                        'UPGRADE_WARNING: Couldn't resolve default property of object g_aIeControl()(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sTableName = Trim(g_aIeControl(iTableIndex)(pbIeControl_objectName))

                        If LCase(sTableName) <> LCase("pmlogicaldatabase") Then
                            'Now check if the table has a guid_id column
                            sSQL = "SELECT column_name FROM information_schema.columns where TABLE_NAME = '" & sTableName & "' and COLUMN_NAME = '" & g_PIEGuidCol & "'"
                            lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CheckForGuidColumn", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
                            'ensure the query returned successfully
                            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                CreateGUIDs = gPMConstants.PMEReturnCode.PMFalse
                                Exit Function
                            End If
                            'now check to see if the column existed
                            If Not IsArray(vResults) Then
                                'column did not exist, we need to create the columns and triggers and indices
                                lReturn = CreateNewPIEColumns(r_oDatabase, sTableName)
                            End If

                            '17/06/2013 - UPDATED
                            'Richard Clarke - something was wrong with SETGUIDValues - it was always setting the GUID on export
                            'which is dangerous if there's already a value there as we MIGHT end up overwriting it which would be disastrous
                            'when the import is done on the target system
                            'I've added this initial check in to see if ANY ROWS in the table actually need a GUID (they shouldn't unless the columns have only
                            'just been added (initial export on a new source database for example)
                            'if there is a table that has GUIDs except on recently added data rows then the table trigger is either missing or not working
                            'so check the triggers and where they're added first before fixing here
                            Dim lMissingGUIDCount As Integer
                            Dim vGUIDResults(,) As Object
                            sSQL = "SELECT COUNT(1) FROM " & sTableName & " WITH (nolock) WHERE " & g_PIEGuidCol & " = '' OR pie_guid IS NULL "
                            lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CheckBlankGUIDs", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vGUIDResults)
                            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                CreateGUIDs = gPMConstants.PMEReturnCode.PMFalse
                                Exit Function
                            End If

                            lMissingGUIDCount = Convert.ToInt32(vGUIDResults(0, 0))

                            'check the global flag to see if import or export
                            If g_iImportExport = 1 AndAlso lMissingGUIDCount > 0 Then
                                'now we can safely add values into the GUID column (for export only)
                                'this should only set a guid the first time the new PIE export is run
                                lReturn = SetGUIDValues(r_oDatabase, sTableName)
                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    CreateGUIDs = gPMConstants.PMEReturnCode.PMFalse
                                    Exit Function
                                End If
                            End If

                        End If
                End Select
            Next iTableIndex

            Exit Function

        Catch ex As Exception


            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProduceExtractFile Failed", vApp:=ACApp, vClass:=conEmptyString, vMethod:="ProduceExtractFile", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function
        End Try

    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateNewPIEColumns
    '
    ' Description: This function creates the PIE_GUID and PIE last updated columns
    '
    '
    ' History: November 2008 Richard Clarke - Created.
    '
    ' ***************************************************************** '
    Public Function CreateNewPIEColumns(ByRef r_oDatabase As dPMDAO.Database, ByVal sTableName As String) As Integer

        'we need to run T-SQL to add the new columns, and the column trigger
        Dim sSQL As String
        Dim lReturn As Integer
        Dim sTriggerName As String

        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        'objFrmMainForm.StatusBar1(0).Items.Item(1).Text = "Creating new PIE columns and table trigger"
        objFrmMainForm.StatusBar_TextWrite("Creating new PIE columns and table trigger", 0)
        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "Tables"
        objFrmMainForm.StatusBar_TextWrite("Tables", 1)
        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = sTableName
        objFrmMainForm.StatusBar_TextWrite(sTableName, 2)

        'add the guid column
        sSQL = "EXEC DDLADDCOLUMN '" & sTableName & "', '" & g_PIEGuidCol & "', '" & g_PIEGuidColDef & " DEFAULT (NEWID())'  "
        lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="CreateNewPIEColumns", bStoredProcedure:=False)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CreateNewPIEColumns = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        'add the last updated column
        sSQL = "EXEC DDLADDCOLUMN '" & sTableName & "', '" & g_PIELastUpdatedCol & "', '" & g_PIELastUpdatedColDef & "'"
        lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="CreateNewPIEColumns", bStoredProcedure:=False)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            CreateNewPIEColumns = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        sTriggerName = sTableName & g_PieLastUpdatedTrigger

        'now we need to add the trigger to the last_updated column of this table
        'BSJ Changed = to IN for trigger
        sSQL = "EXEC DDLDROPTRIGGER '" & sTriggerName & "'"
        lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="CreateNewPIEColumns", bStoredProcedure:=False)
        'add the trigger
        sSQL = "CREATE TRIGGER " & sTriggerName & " ON " & sTableName & " FOR INSERT, UPDATE AS UPDATE "
        sSQL = sSQL & sTableName & " SET " & g_PIELastUpdatedCol & " = getdate() WHERE " & g_PIEGuidCol & " IN "
        sSQL = sSQL & "(Select " & g_PIEGuidCol & " FROM Inserted)"
        lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="CreateNewPIEColumns", bStoredProcedure:=False)

        CreateNewPIEColumns = gPMConstants.PMEReturnCode.PMTrue
    End Function


    Public Function SetGUIDValues(ByRef r_oDatabase As dPMDAO.Database, ByVal sTableName As String) As Integer

        Dim lReturn As Integer
        Dim sSQL As String
        Dim iRecord As Integer
        Dim sGISListsWhere As String

        Try
            lReturn = gPMConstants.PMEReturnCode.PMTrue

            'select the PK column values
            sSQL = "UPDATE " & sTableName & " SET pie_guid = NEWID() WHERE pie_guid = '' OR pie_guid IS NULL "

            'if this is gis_list_item table we need to filter by the udl selected
            If LCase(sTableName) = LCase("gis_list_items") AndAlso objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_UDLs).CheckState = 1 Then
                'add the where clause to the sSQL
                'we've got the udl name in the s_audl array so use it to join on gis_list_type
                'where code = name from array then the gis_list_type_usage table
                'can be filtered by gis_list_type and return us a list of gis_list_items_ids
                'need to loop round the array of udls...
                If UBound(s_aUDLSelected) >= 0 Then
                    sGISListsWhere = sGISListsWhere & " code='" & s_aUDLSelected(0) & "'"
                    For iRecord = 1 To UBound(s_aUDLSelected)
                        sGISListsWhere = sGISListsWhere & " OR code = '" & s_aUDLSelected(iRecord) & "'"
                    Next iRecord
                End If

                sSQL = sSQL & " AND gis_list_items_id IN (SELECT gis_list_items_id From GIS_List_Type_Usage Where "
                sSQL = sSQL & " gis_list_type_id IN(SELECT gis_list_type_id From GIS_List_Type Where "
                sSQL = sSQL & sGISListsWhere & "))"
            End If

            'same as above for gis_list_type_usage
            If LCase(sTableName) = LCase("gis_list_type_usage") Then
                sGISListsWhere = ""
                If UBound(s_aUDLSelected) >= 0 Then
                    sGISListsWhere = sGISListsWhere & " code='" & s_aUDLSelected(0) & "'"
                    For iRecord = 1 To UBound(s_aUDLSelected)
                        sGISListsWhere = sGISListsWhere & " OR code = '" & s_aUDLSelected(iRecord) & "'"
                    Next iRecord
                End If
                sSQL = sSQL & " AND gis_list_type_id IN (SELECT gis_list_type_id From GIS_List_Type Where "
                sSQL = sSQL & sGISListsWhere & ")"
            End If

            lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False)

            Return lReturn

        Catch ex As Exception

            lReturn = gPMConstants.PMEReturnCode.PMError
            LogAuditMessage("Error in SetGUIDValues for table " & sTableName & ": " & ex.ToString, False, True, "", False)
            Return lReturn

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateUserSPUFiles
    '
    ' Description: This function creates the user spu_ICCS% files
    '           so they can be exported in the binary file
    '
    ' History: November 2008 Richard Clarke - Created.
    '
    ' ***************************************************************** '
    Public Function CreateUserSPUFiles(ByRef r_oDatabase As dPMDAO.Database) As Integer

        Dim sSQL As String
        Dim vResults(,) As Object
        Dim lReturn As Integer
        Dim sSPUName As String
        Dim iRecord As Short
        Dim vResultStoreProcBody(,) As Object
        Dim sSPUBody As String
        Dim iSPURec As Short
        Dim iFileNo As Short
        Dim sFilePath As String

        'sSQL = select * from sysobjects where type = 'P' and category = 0 and name like
        'spu_ICCS_%'
        'files will be temporarily stored here

        sFilePath = objFrmMainForm.txtBackupDir.Text & "spuICCS\"

        If objFrmMainForm.txtBackupDir.Text = "" Then
            sFilePath = objFrmMainForm.txtFilePath(1).Text & "\spuICCS\"
        End If

        'make the path if it doesn't exist
       
        'now check that the directory exists and is accessible by the application
        If Not FolderExists(sFilePath) Then
            On Error Resume Next
            'doesn't exist, can we create it.
            CreateFolder(sFilePath)
            'check to see if we created the folder
            If Err.Number <> 0 Then
                'there was an error, we can't perform the backup, warn the user and return false
                MsgBox("The export cannot be performed. Please ensure the application can access " & sFilePath & " before continuing", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "PIE")
                CreateUserSPUFiles = gPMConstants.PMEReturnCode.PMFalse
                On Error GoTo 0
                Exit Function
            End If
        End If

        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        'objFrmMainForm.StatusBar1(0).Items.Item(1).Text = "Creating user SPU Files "
        objFrmMainForm.StatusBar_TextWrite("Creating user SPU Files ", 0)
        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "SPU"
        objFrmMainForm.StatusBar_TextWrite("SPU", 1)

        sSQL = "SELECT name FROM sysobjects WHERE type='P' AND category = 0 AND name LIKE 'spu_ICCS_%'"
        lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CreateUserSPUFiles", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
        If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            CreateUserSPUFiles = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If
        'loop round results
        objFrmMainForm.ProgressBar1(1).Maximum = 100
        For iRecord = 0 To UBound(vResults, 2)
            'get the sproc help text and output to file.
            'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sSPUName = vResults(0, iRecord)
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = sSPUName
            objFrmMainForm.StatusBar_TextWrite(sSPUName, 2)
            objFrmMainForm.ProgressBar1(1).Value = 1
            'ok now we need to get the stored procedure body and add the ddl drop command to it
            'sSQL = "exec sp_helptext '" & sSPUName & "'"
            sSQL = "select syscomments.text From sysobjects, syscomments where sysobjects.id = "
            sSQL = sSQL & "syscomments.id and sysobjects.type = 'P' and sysobjects.name = '" & sSPUName & "'"

            sSPUBody = ""
            lReturn = r_oDatabase.SQLSelectTextField(sSQL:=sSQL, sSQLName:="CreateUserSPUFiles", bStoredProcedure:=False, sTextData:=sSPUBody)


            'add the ddl drop procedure call to the body
            sSPUBody = "EXEC ddlDropProcedure '" & sSPUName & "'" & vbCrLf & sSPUBody
            'For iSPURec = 0 To UBound(vResultStoreProcBody, 2)
            '    'tag the text onto the end of our string
            '    'UPGRADE_WARNING: Couldn't resolve default property of object vResultStoreProcBody(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            '    sSPUBody = sSPUBody & vResultStoreProcBody(0, iSPURec)
            'Next iSPURec
            'write the spu to an .sql file called <sproc name>.sql
            iFileNo = FreeFile()
            FileOpen(iFileNo, sFilePath & sSPUName & ".sql", OpenMode.Output)
            PrintLine(iFileNo, sSPUBody)

            FileClose(iFileNo)
            objFrmMainForm.ProgressBar1(1).Value = objFrmMainForm.ProgressBar1(1).Maximum
            System.Windows.Forms.Application.DoEvents()
        Next iRecord


        CreateUserSPUFiles = gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateSPUReportFiles
    '
    ' Description: This function creates the user spu_report_% files
    '           so they can be exported in the binary file
    '
    ' History: November 2008 Richard Clarke - Created.
    '
    ' ***************************************************************** '
    Public Function CreateSPUReportFiles(ByRef r_oDatabase As dPMDAO.Database) As Integer

        Dim sSQL As String
        Dim vResults(,) As Object
        Dim lReturn As Integer
        Dim sSPUName As String
        Dim iRecord As Short
        Dim vResultStoreProcBody(,) As Object
        Dim sSPUBody As String
        Dim iSPURec As Short
        Dim iFileNo As Short
        Dim sFilePath As String

        'sSQL = select * from sysobjects where type = 'P' and category = 0 and name like
        'spu_ICCS_%'
        'files will be temporarily stored here
        sFilePath = objFrmMainForm.txtBackupDir.Text & "spuReport\"

        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        'objFrmMainForm.StatusBar1(0).Items.Item(1).Text = "Creating user SPU Files "
        objFrmMainForm.StatusBar_TextWrite("Creating user SPU Files ", 0)
        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "SPU"
        objFrmMainForm.StatusBar_TextWrite("SPU", 1)
        sSQL = "SELECT name FROM sysobjects WHERE type='P' AND category = 0 AND name LIKE 'spu_report_%'"
        lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CreateSPUReportFiles", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
        If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            CreateSPUReportFiles = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If
        'loop round results
        objFrmMainForm.ProgressBar1(1).Maximum = 100
        For iRecord = 0 To UBound(vResults, 2)
            'get the sproc help text and output to file.
            'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sSPUName = vResults(0, iRecord)
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            objFrmMainForm.StatusBar_TextWrite(sSPUName, 2)
            objFrmMainForm.ProgressBar1(1).Value = 1
            'ok now we need to get the stored procedure body and add the ddl drop command to it
            'sSQL = "exec sp_helptext '" & sSPUName & "'"
            sSQL = "select syscomments.text From sysobjects, syscomments where sysobjects.id = "
            sSQL = sSQL & "syscomments.id and sysobjects.type = 'P' and sysobjects.name = '" & sSPUName & "'"

            lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CreateSPUReportFiles", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultStoreProcBody)
            sSPUBody = ""

            'add the ddl drop procedure call to the body
            sSPUBody = sSPUBody & "EXEC ddlDropProcedure '" & sSPUName & "'" & vbCrLf
            For iSPURec = 0 To UBound(vResultStoreProcBody, 2)
                'tag the text onto the end of our string
                'UPGRADE_WARNING: Couldn't resolve default property of object vResultStoreProcBody(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sSPUBody = sSPUBody & vResultStoreProcBody(0, iSPURec)
            Next iSPURec
            'write the spu to an .sql file called <sproc name>.sql
            iFileNo = FreeFile()
            FileOpen(iFileNo, sFilePath & sSPUName & ".sql", OpenMode.Output)
            PrintLine(iFileNo, sSPUBody)

            FileClose(iFileNo)
            objFrmMainForm.ProgressBar1(1).Value = objFrmMainForm.ProgressBar1(1).Maximum
        Next iRecord

        CreateSPUReportFiles = gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' ***************************************************************** '
    ' Name: CheckPieAuditTableExists
    '
    ' Description: This function checks to see if the PIE audit table exists
    '
    ' Added November 2008 Richard Clarke - PIE enhancements
    ' ***************************************************************** '
    Public Function CheckPieAuditTableExists() As Integer

        Dim sSQL As String
        Dim aResults(,) As Object

        CheckPieAuditTableExists = gPMConstants.PMEReturnCode.PMFalse

        sSQL = "select name from sysobjects where xtype='u' and name = '" & g_PIEAuditTable & "'"

        g_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Check Pie Audit table exists", bStoredProcedure:=False, vResultArray:=aResults)

        If IsArray(aResults) Then
            CheckPieAuditTableExists = gPMConstants.PMEReturnCode.PMTrue
        End If

        Exit Function

    End Function

    ' ***************************************************************** '
    ' Name: CreatePIEAuditTable
    '
    ' Description: This function creates the PIE audit table
    '
    ' Added November 2008 Richard Clarke - PIE enhancements
    ' ***************************************************************** '
    Public Function CreatePIEAuditTable() As Integer

        Dim sSQL As String

        ' BSJ changed pmuser id to varchar
        sSQL = "CREATE TABLE [" & g_PIEAuditTable & "]("
        sSQL = sSQL & " [pie_audit_cnt] [int] IDENTITY PRIMARY KEY NOT NULL,"
        sSQL = sSQL & "[message] [varchar](500) NOT NULL,"
        sSQL = sSQL & g_PieAuditTimestampCol & " [datetime] NOT NULL DEFAULT getdate(),"
        sSQL = sSQL & "[import] [Boolean] NOT NULL,"
        sSQL = sSQL & "[export] [Boolean] NOT NULL,"
        sSQL = sSQL & "[successful] [Boolean] NOT NULL,"
        sSQL = sSQL & "[pmuser_id] [varchar] (500) NOT NULL,"
        sSQL = sSQL & "[remedial_action] [varchar](500) NULL)"
        'sSQL = sSQL & "CONSTRAINT [PK__PIE_Audit] PRIMARY KEY CLUSTERED ([pie_audit_cnt] Asc)"
        'sSQL = sSQL & "WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, " & _
        ''            "ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]) ON [PRIMARY]"

        g_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="CreatePIEAuditTable", bStoredProcedure:=False)

        CreatePIEAuditTable = gPMConstants.PMEReturnCode.PMTrue
        Exit Function

    End Function

    ' ***************************************************************** '
    ' Name: BackupDatabase
    '
    ' Description: This function backs the target database up
    '
    ' Added November 2008 Richard Clarke - PIE enhancements
    ' ***************************************************************** '
    Public Function BackupDatabase(ByRef sFilename As String, ByVal sBackupPath As String) As Integer

        Dim lCommandTimeout As Integer
        Dim sBackupFilename As String
        Dim sDatabaseName As String
        Dim lReturn As Integer
        Dim sSQL As String
        'make sure the backup dir is available through the machine name

        Try

            'get the database name from the registry
            GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="Database", r_sSettingValue:=sDatabaseName, v_sSubKey:="Databases\SIRIUSSOLUTIONS")

            sFilename = VB6.Format(Now, "yyyy-mm-dd hh-nn-ss") & ".bak"
            'store the bak file name in the global for the restore later (if required)
            g_sBackupFilename = sFilename
            g_sBackupPath = sBackupPath
            sBackupFilename = sBackupPath & "\" & sFilename

            sFilename = sBackupFilename

            lCommandTimeout = g_oDatabase.QueryTimeout
            If lCommandTimeout < g_MinTimeoutForBackupRestore Then
                g_oDatabase.QueryTimeout = g_MinTimeoutForBackupRestore
            End If

            sSQL = "backup database " & sDatabaseName & " to disk = '" & sBackupFilename & "'"

            objFrmMainForm.ProgressBar1(1).Value = 50

            lReturn = g_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="backup database", bStoredProcedure:=False)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'could not backup database
                MsgBox("Database could not be backed up, please ensure " & sBackupPath & " is visible from the SQL server", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "PIE")
            End If

            g_oDatabase.QueryTimeout = lCommandTimeout
            objFrmMainForm.ProgressBar1(1).Value = 100

            BackupDatabase = gPMConstants.PMEReturnCode.PMTrue
            Exit Function

        Catch ex As Exception
            MsgBox("Database could not be backed up, please ensure " & sBackupPath & " is visible from the SQL server", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "PIE")
            BackupDatabase = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LogAuditMessage
    '
    ' Description: This function writes messages to the PIE audit table
    '
    ' Added November 2008 Richard Clarke - PIE enhancements
    ' ***************************************************************** '
    Public Function LogAuditMessage(ByVal sMessage As String, ByVal bImport As Boolean, ByVal bExport As Boolean, ByVal sRemedialAction As String, ByVal bSuccessful As Boolean) As Integer

        Dim sSQL As String
        Dim iImport As Short
        Dim iExport As Short
        Dim iSuccessful As Short

        Try

            'convert true / false to 1 / 0
            If bImport = True Then
                iImport = 1
            Else
                iImport = 0
            End If

            If bExport = True Then
                iExport = 1
            Else
                iExport = 0
            End If

            If bSuccessful = True Then
                iSuccessful = 1
            Else
                iSuccessful = 0
            End If

            sSQL = "INSERT INTO " & g_PIEAuditTable & " ([message], " & g_PieAuditTimestampCol & ", import, export, successful, [pmuser_id], remedial_action) VALUES ('" & sMessage & "', "
            sSQL = sSQL & "getdate(), " & iImport & ", " & iExport & "," & iSuccessful & ",'" & g_sUsername & "', '"
            sSQL = sSQL & sRemedialAction & "')"

            g_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False)

        Catch ex As Exception
        End Try



    End Function

    Public Function CheckSingleTableGuid(ByRef r_oDatabase As dPMDAO.Database, ByVal sTableName As String) As Integer

        Dim sSQL As String
        Dim vResults(,) As Object
        Dim lReturn As Integer
        'don't do anything for pmlogicaldatabase
        If LCase(sTableName) <> LCase("pmlogicaldatabase") Then

            'Now check if the table has a guid_id column
            sSQL = "SELECT column_name FROM information_schema.columns where TABLE_NAME = '" & sTableName & "' and COLUMN_NAME = '" & g_PIEGuidCol & "'"
            lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CheckForGuidColumn", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
            'ensure the query returned successfully
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                CheckSingleTableGuid = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            'now check to see if the column existed already
            If Not IsArray(vResults) Then

                'first create the columns etc
                CheckSingleTableGuid = gPMConstants.PMEReturnCode.PMTrue
                CheckSingleTableGuid = CreateNewPIEColumns(r_oDatabase, sTableName)

                'if this is an export set guid values
                If g_iImportExport = 1 And CheckSingleTableGuid Then
                    CheckSingleTableGuid = SetGUIDValues(r_oDatabase, sTableName)
                End If
                'now add the table constraint for the default value for the guid column
                sSQL = "ALTER TABLE " & sTableName & " ADD CONSTRAINT [" & sTableName & "_defaultguid]"
                sSQL = sSQL & " DEFAULT (NEWID()) FOR [" & g_PIEGuidCol & "]"
                lReturn = r_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="GUID default constraint", bStoredProcedure:=False)

            End If
        End If

    End Function

    ' ***************************************************************** '
    ' Name: LogErrorMessage
    '
    ' Description: This function writes error messages to the PIE audit table
    '               and prompts the user to continue or cancel
    '               the error report is displayed on tab 2 (actual index 1) of SStab3
    ' Added November 2008 Richard Clarke - PIE enhancements
    ' ***************************************************************** '
    Public Function LogPIEError(ByVal sMessage As String, ByVal bImport As Boolean, ByVal bExport As Boolean, ByVal sRemedialAction As String, ByVal bSuccessful As Boolean, ByVal sDataModel As String, ByVal sTableName As String, Optional ByVal sPKColName As String = "", Optional ByVal sPKColValue As String = "", Optional ByVal sGUIDValue As String = "") As Integer

        Try

            sMessage = "Error in import occurred in " & sTableName
            If sPKColName <> "" Then
                sMessage = sMessage & "Primary key column: " & sPKColName & " "
            End If

            If sPKColValue <> "" Then
                sMessage = sMessage & "Primary key value: " & sPKColValue & " "
            End If

            If sGUIDValue <> "" Then
                sMessage = sMessage & "Row GUID value: " & sGUIDValue & " "
            End If

            'first update the text box with the message
            objFrmMainForm.txtManualChanges.Text = objFrmMainForm.txtManualChanges.Text & sMessage & vbCrLf
            objFrmMainForm.txtManualChanges.Text = objFrmMainForm.txtManualChanges.Text & " last sql command generated was: " & g_sLastSQLCommandGenerated & vbCrLf

            'now log the error message to the PIE_Audit Log table
            LogAuditMessage(sMessage, True, False, "", False)
            g_bErrorInImport = True

        Catch ex As Exception
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: RestoreDatabase
    '
    ' Description: This function restores the target database to the
    '               back up state pre import
    '
    ' Added November 2008 Richard Clarke - PIE enhancements
    ' ***************************************************************** '
    Public Function RestoreDatabase() As Integer

        Dim sSQL As String
        Dim vResults(,) As Object
        Dim sMDFFullPath As String
        Dim sLDFFullPath As String
        Dim sMDFLogicalName As String
        Dim sLDFLogicalName As String
        Dim iCount As Short

        Dim con As ADODB.Connection
        Dim sDatabaseName As String
        Dim sServerName As String

        Try

            GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="Database", r_sSettingValue:=sDatabaseName, v_sSubKey:="Databases\SIRIUSSOLUTIONS")

            GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="Server", r_sSettingValue:=sServerName, v_sSubKey:="Databases\SIRIUSSOLUTIONS")

            'this query returns the logical name and physical file paths for the MDF and LDF
            sSQL = "RESTORE FILELISTONLY FROM DISK = '" & g_sBackupPath & "\" & g_sBackupFilename & "'"

            g_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Restore Database", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)

            If IsArray(vResults) Then
                'get the logical names and physical paths
                'LogicalName column and PhysicalName
                For iCount = 0 To UBound(vResults, 2)
                    'check to see if Database or Log (type = D or type = L) - column 2
                    'UPGRADE_WARNING: Couldn't resolve default property of object vResults(2, iCount). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If vResults(2, iCount) = "D" Then
                        'database - get the name and path
                        'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sMDFFullPath = vResults(1, iCount)
                        'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sMDFLogicalName = vResults(0, iCount)
                    Else
                        'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sLDFFullPath = vResults(1, iCount)
                        'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        sLDFLogicalName = vResults(0, iCount)
                    End If
                Next iCount

            End If

            'cannot use g_oDatabase to do the restore, need an ADO connection to
            'connect to MASTER then run the restore command
            'note that following this, the user cannot use their current logon to carry out any actions

            sSQL = "RESTORE DATABASE " & sDatabaseName & " FROM DISK = '" & g_sBackupPath & "\" & g_sBackupFilename & "'"
            sSQL = sSQL & " With Move '" & sMDFLogicalName & "' TO '" & sMDFFullPath & "',"
            sSQL = sSQL & " Move '" & sLDFLogicalName & "' TO '" & sLDFFullPath & "', REPLACE"

            'set up the ADO connection
            con = New ADODB.Connection
            'use windows authentication - this may cause problems for users with insufficient credentials
            con.CursorLocation = ADODB.CursorLocationEnum.adUseServer
            con.ConnectionString = "Provider=SQLOLEDB" & ";Data Source=" & sServerName & ";Initial Catalog=MASTER" & ";Integrated Security=SSPI" & ";OLE DB Services=-1"

            con.Open() 'open the connection
            'set to single user mode so we can execute the restore command
            con.Execute("ALTER DATABASE " & sDatabaseName & " SET SINGLE_USER WITH ROLLBACK IMMEDIATE")
            'execute the restore command
            con.Execute(sSQL)

            'success - now the user must close PIE and log off

            RestoreDatabase = gPMConstants.PMEReturnCode.PMTrue
            Exit Function
        Catch ex As Exception


            'check the connection is available
            If con.State <> 1 Then
                'connecting to the database failed
                'inform that the user they are logged on as doesn't have sql server login rights
                'and they must perform a manual restore
                MsgBox("Your windows user profile does not have sufficient rights to perform database " & "restoration operations. Please peform a manual restore of file " & g_sBackupPath & "\" & g_sBackupFilename, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "PIE")
                RestoreDatabase = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            Else
                con.Execute("ALTER DATABASE " & sDatabaseName & " SET MULTI_USER GO")
            End If
            'failure so reset the db to multi user mode
            RestoreDatabase = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RestoreTarget
    '
    ' Description: This function restores the target database to the
    '               back up state pre import
    '
    ' Added November 2008 Richard Clarke - PIE enhancements
    ' ***************************************************************** '
    Public Function RestoreTarget() As Integer

        Try

            'first restore the database to it's pre-import state
            'bak filename needed
            RestoreDatabase()
            'copy back all the directories we backed up to their original locations

            'restore the registry key we backed up

            'success

            MsgBox("You must now close the Product Import / Export application and log off Pure", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "PIE")
            Exit Function
        Catch ex As Exception
            'automatic restore failed - prompt the user to carry out a manual restore
            MsgBox("The automatic restore process failed, please carry out a manual restore", MsgBoxStyle.OKOnly + MsgBoxStyle.Critical, "PIE")

    End Function
    Public Function EnableUserLogins() As Integer

        Dim sSQL As String
        Dim iCounter As Short
        Dim vResults(,) As Object

        On Error GoTo Err_EnableUserLogins

        'enable them all
        '    If IsArray(g_UserIDs) Then
        '        For iCounter = 0 To UBound(g_UserIDs, 2)
        '            'enable this user's login ability
        '            sSQL = "UPDATE PMUser SET is_deleted = 0 WHERE user_id = " & g_UserIDs(0, iCounter)
        '            g_oDatabase.SQLAction sSQL:=sSQL, sSQLName:="Disabling user logins", bStoredProcedure:=False
        '        Next iCounter
        '    End If

        'Richard Clarke - NEW January 2009
        'we now have an additional column on PMUser called previous_is_deleted
        'get the users who previous_is_deleted = 0
        sSQL = "SELECT user_id FROM PMUser WHERE previous_is_deleted = 0"
        g_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Enabling user logins", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)

        If IsArray(vResults) Then
            For iCounter = 0 To UBound(vResults, 2)
                'UPGRADE_WARNING: Couldn't resolve default property of object vResults(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sSQL = "UPDATE PMUser SET is_deleted = 0 WHERE user_id = " & vResults(0, iCounter)
                g_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Enabling user logins", bStoredProcedure:=False)
            Next iCounter
        End If

        EnableUserLogins = gPMConstants.PMEReturnCode.PMTrue
        Exit Function

Err_EnableUserLogins:
        EnableUserLogins = gPMConstants.PMEReturnCode.PMFalse
        Exit Function
    End Function
End Module