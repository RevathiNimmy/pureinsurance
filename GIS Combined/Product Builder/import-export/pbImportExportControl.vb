Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
Imports SharedFiles

Module pbImportExportControl

    Private Const ACClass As String = conEmptyString

    Private m_lReturn As gPMConstants.PMEReturnCode

    ' ***************************************************************** '
    '
    ' Name:        ProduceExtractFile
    '
    ' Description: Opens and closes the extract file and determines the
    '              processing required for each row in the control array
    '
    ' History:     30/08/2002 JB - Created.
    '
    ' ***************************************************************** '
    Public Function ProduceExtractFile(ByRef oDatabase As dPMDAO.Database, ByRef lDataModelId As Integer, ByRef sDataModelCode As String, ByRef tFilePath As String, ByRef tFileName As String, ByRef tFileExtension As String, ByRef sVersionNumber As String) As Integer

        'Define general variables
        Dim result As Integer = 0
        Dim iFileNumber As Integer

        'lines written, used by import as status indicator
        Dim lTotalLinesWritten As Integer

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ProduceExtractFile")

            result = gPMConstants.PMEReturnCode.PMTrue

            If objFrmMainForm.chkExportSPUICCS.CheckState = 1 Then
                'we need to create the user SPU files to directory
                m_lReturn = CreateUserSPUFiles(r_oDatabase:=oDatabase)
            Else
                m_lReturn = gPMConstants.PMEReturnCode.PMTrue
            End If

            'Open the extract file
            Dim sFileResult As String = OpenBinaryFile(i_AccessType:=WriteAccess, i_sFilePath:=tFilePath, i_sFileName:=tFileName, i_sFileExtension:=tFileExtension, o_iFileNumber:=iFileNumber)
            If sFileResult = "INVALID" Then
                objFrmMainForm.txtWarning_TextWrite("Please set path of Export file.", 1)
                Return gPMConstants.PMEReturnCode.PMFalse
            ElseIf conEmptyString <> sFileResult Then
                'writeToStatusBox("Error opening export file: " & tFilePath)
                objFrmMainForm.txtWarning_TextWrite("Error opening export file: " & tFilePath, 1)
                Return result
            End If

            'Initialise all the control arrays
            If InitialiseControlArrays(r_oDatabase:=oDatabase, r_lDataModelId:=lDataModelId, r_sDataModelCode:=sDataModelCode, v_generateObjectConstants:=False, r_lSiriusUserId:=g_lSiriusUserId) <> gPMConstants.PMEReturnCode.PMTrue Then

                'If the initalisation  has failed,
                'end program as the control arrays are vital to the
                'the operation of the program
                result = gPMConstants.PMEReturnCode.PMFalse
                CloseBinaryFile(v_iFileNumber:=iFileNumber)
                Return result
            End If

            If g_bStopProcessing Then
                ' Don't do export section
            Else
                'Set the first panel to indicate extract processing has started
                'objFrmMainForm.StatusBar1(0).Items.Item(0).Text = "Extracting"
                'objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Opening the extract file"
                objFrmMainForm.StatusBar_TextWrite("Extracting", 0)
                objFrmMainForm.StatusBar_TextWrite("Opening the extract file", 1)
                'Begin the extraction operations
                m_lReturn = CType(doExport(r_oDatabase:=oDatabase, v_iFileNumber:=iFileNumber, v_lDataModelId:=lDataModelId, v_sDataModelCode:=sDataModelCode, g_aIeControl:=g_aIeControl, g_aIeTableDefinitions:=g_aIeTableDefinitions, v_sVersionNumber:=sVersionNumber, r_lTotalLinesWritten:=lTotalLinesWritten), gPMConstants.PMEReturnCode)

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                StopProcessing(iFileNumber)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set the second panel to indicate extract processing has conpleted
            ' objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Closing the extract file"
            objFrmMainForm.StatusBar_TextWrite("Closing the extract file", 1)
            'objFrmMainForm.StatusBar1(2).Items.Item(0).Text = conEmptyString
            objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)
            'Close the extract file
            m_lReturn = CType(CloseBinaryFile(v_iFileNumber:=iFileNumber), gPMConstants.PMEReturnCode)
            If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FileName", r_sSettingValue:=tFileName) = gPMConstants.PMEReturnCode.PMTrue Then
            End If
            'now write the length information
            If conEmptyString <> OpenBinaryFile(i_AccessType:=WriteRandom, i_sFilePath:=tFilePath, i_sFileName:=tFileName, i_sFileExtension:=tFileExtension, o_iFileNumber:=iFileNumber) Then
                objFrmMainForm.txtWarning_TextWrite("Error opening export file: " & tFilePath, 1)
                Return result
            End If

            FileSystem.Seek(iFileNumber, 4)
            FileSystem.FilePut(iFileNumber, lTotalLinesWritten, 3)
            'FileSystem.FilePut(iFileNumber, 3, lTotalLinesWritten)
            m_lReturn = CType(CloseBinaryFile(v_iFileNumber:=iFileNumber), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'ensure GUI is correct
            m_lReturn = CType(DoGuiDefaults(v_iImportExport:=1, v_bClearWarning:=False), gPMConstants.PMEReturnCode) 'set for export!

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Stick a message out to indicate success
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If g_bStopProcessing Then
                    m_lReturn = CType(StopProcessing(v_iFileNumber:=iFileNumber), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'writeToStatusBox("Error(s) during cancellation of export")
                        objFrmMainForm.txtWarning_TextWrite("Error(s) during cancellation of export", 1)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    'writeToStatusBox("Export completed sucessfully, " & lTotalLinesWritten & " lines written")
                    objFrmMainForm.txtWarning_TextWrite("Export completed sucessfully, " & lTotalLinesWritten & " lines written", 1)
                End If
            Else
                'writeToStatusBox("Export not completed due to problems")
                objFrmMainForm.txtWarning_TextWrite("Export not completed due to problems", 1)
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ProduceExtractFile")

            Return result

        Catch excep As System.Exception

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ProduceExtractFile")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProduceExtractFile Failed", vApp:=ACApp, vClass:=conEmptyString, vMethod:="ProduceExtractFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        ImportExtractFile
    '
    ' Description: Imports a perviously created extract file
    '              Opens and closes the extract file and determines the
    '              processing required for each row in the control array
    '
    ' History:     03/09/2002 JB - Created.
    '
    ' ***************************************************************** '
    Public Function ImportExtractFile(ByRef oDatabase As dPMDAO.Database, ByRef lDataModelId As Integer, ByRef sDataModelCode As String, ByRef tFilePath As String, ByRef tFileName As String, ByRef tFileExtension As String, ByVal v_bCheckHeader As Boolean, ByRef sVersionNumber As String, ByVal v_bImportRegistry As Boolean) As Integer

        'Define general variables
        Dim result As Integer = 0
        Dim iFileNumber As Integer

        Try

            Debug.WriteLine(gPMFunctions.ToSafeString(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ImportExtractFile")

            result = gPMConstants.PMEReturnCode.PMTrue
            If tFilePath = "" Then
                If gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="PB_IE_FilePath", r_sSettingValue:=tFilePath) = gPMConstants.PMEReturnCode.PMTrue Then
                End If
            End If
            'open the file first incase there is an error
            Dim sFileResult As String = OpenBinaryFile(i_AccessType:=ReadAccess, i_sFilePath:=tFilePath, i_sFileName:=tFileName, i_sFileExtension:=tFileExtension, o_iFileNumber:=iFileNumber)
            If sFileResult = "INVALID" Then
                objFrmMainForm.txtWarning_TextWrite("Please set path of import file.", 0)
                Return gPMConstants.PMEReturnCode.PMFalse
            ElseIf conEmptyString <> sFileResult Then
                'writeToStatusBox("Error opening import file: " & tFilePath)
                objFrmMainForm.txtWarning_TextWrite("Error opening import file: " & tFilePath, 0)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Initialise all the control arrays
            If InitialiseControlArrays(r_oDatabase:=oDatabase, r_lDataModelId:=lDataModelId, r_sDataModelCode:=sDataModelCode, v_generateObjectConstants:=False, r_lSiriusUserId:=g_lSiriusUserId) <> gPMConstants.PMEReturnCode.PMTrue Then

                'If the initalisation  has failed,
                'end program as the control arrays are vital to the
                'the operation of the program
                'writeToStatusBox("Failed to initialise the control arrays")
                objFrmMainForm.txtWarning_TextWrite("Failed to initialise the control arrays", 0)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set the first panel to indicate extract processing has started
            'objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Importing"
            'objFrmMainForm.StatusBar1(2).Items.Item(0).Text = "Opening the extract file"
            objFrmMainForm.StatusBar_TextWrite("Importing", 1)
            objFrmMainForm.StatusBar_TextWrite("Opening the extract file", 2)

            'Begin the import operations
            m_lReturn = CType(doImport(r_oDatabase:=oDatabase, v_iFileNumber:=iFileNumber, v_lDataModelId:=lDataModelId, v_sDataModelCode:=sDataModelCode, g_aIeControl:=g_aIeControl, g_aIeTableDefinitions:=g_aIeTableDefinitions, v_bCheckHeaderOnly:=v_bCheckHeader, v_sVersionNumber:=sVersionNumber, v_bImportRegistry:=v_bImportRegistry), gPMConstants.PMEReturnCode)

            'Close the extract file
            '    If m_lReturn = PMTrue Then
            CloseBinaryFile(v_iFileNumber:=iFileNumber)
            '   End If

            'ensure GUI is correct
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(DoGuiDefaults(v_iImportExport:=1, v_bClearWarning:=False), gPMConstants.PMEReturnCode) 'set for export!
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not v_bCheckHeader Then
                'Stick a message out to indicate success
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    'writeToStatusBox("Import completed sucessfully")
                    objFrmMainForm.txtWarning_TextWrite("Import completed sucessfully", 0)
                Else
                    'writeToStatusBox("Import not completed due to problems")
                    objFrmMainForm.txtWarning_TextWrite("Import not completed due to problems", 0)
                End If
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ImportExtractFile")

            Return result

        Catch excep As System.Exception

            CloseBinaryFile(v_iFileNumber:=iFileNumber)

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ImportExtractFile")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportExtractFile Failed", vApp:=ACApp, vClass:=conEmptyString, vMethod:="ImportExtractFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function CreateUserSPUFiles(ByRef r_oDatabase As dPMDAO.Database) As Integer

        Dim sSQL As String
        Dim vResults As Object
        Dim lReturn As Integer
        Dim sSPUName As String
        Dim iRecord As Short
        Dim vResultStoreProcBody As Object
        Dim sSPUBody As String
        Dim iSPURec As Short
        Dim iFileNo As Short
        Dim sFilePath As String

        'sSQL = select * from sysobjects where type = 'P' and category = 0 and name like
        'spu_ICCS_%'
        'files will be temporarily stored here

        sFilePath = GetFilePath() & "\spuICCS\"

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

            lReturn = r_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CreateUserSPUFiles", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultStoreProcBody)
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
            System.Windows.Forms.Application.DoEvents()
        Next iRecord


        CreateUserSPUFiles = gPMConstants.PMEReturnCode.PMTrue

    End Function
End Module
