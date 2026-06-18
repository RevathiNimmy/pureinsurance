Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles

Module pbImportExportControl

    Private Const ACClass As String = conEmptyString

    Private m_lReturn As Integer

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
    Public Function ProduceExtractFile(ByRef oDatabase As dPMDAO.Database, ByRef lDataModelId() As Integer, ByRef sDataModelCode() As String, ByRef tFilePath As String, ByRef tFileName As String, ByRef tFileExtension As String, ByRef sVersionNumber As String) As Integer
        'lDataModelId As Long
        'sDataModelCode as string
        'now arrays to handle multiple data models


        'Define general variables
        Dim iFileNumber As Short

        'lines written, used by import as status indicator
        Dim lTotalLinesWritten As Integer

        Try

            ' Debug message
            Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ProduceExtractFile")

            ProduceExtractFile = gPMConstants.PMEReturnCode.PMTrue

            'Open the extract file
            If conEmptyString <> OpenBinaryFile(i_AccessType:=WriteAccess, i_sFilePath:=tFilePath, i_sFileName:=tFileName, i_sFileExtension:=tFileExtension, o_iFileNumber:=iFileNumber) Then
                writeToStatusBox(("Error opening export file: " & tFilePath))
                Exit Function
            End If

            'Richard Clarke November 2008 - PIE enhancements
            'this block of code sets up the UDL array for export
            ReDim s_aUDLSelected(0)
            'set up the UDL array with user selected UDLs in it.
            Dim iUDLIndex As Short
            For iUDLIndex = 0 To objFrmMainForm.tvUDL.Nodes(0).Nodes.Count - 1
                'if this node is selected then add it to the s_audl
                'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.tvUDL.Nodes has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                If objFrmMainForm.tvUDL.Nodes(0).Nodes.Item(iUDLIndex).Checked Then
                    ReDim Preserve s_aUDLSelected(CInt(UBound(s_aUDLSelected) + 1))
                    'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.tvUDL.Nodes has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                    s_aUDLSelected(CInt(UBound(s_aUDLSelected) - 1)) = Mid(objFrmMainForm.tvUDL.Nodes(0).Nodes.Item(iUDLIndex).Text, 5)
                End If
            Next iUDLIndex


            If UBound(s_aUDLSelected) > 0 Then
                ReDim Preserve s_aUDLSelected(UBound(s_aUDLSelected) - 1)
            Else
                'ToDo: should we ever get here
                ReDim Preserve s_aUDLSelected(0)
            End If

            'now do the same for the datamodels but only check if none were ticked here.
            Dim iDMIndex As Short
            Dim bDataModelSelected As Boolean

            Dim iNodeCount As Integer = objFrmMainForm.tvDataModel.Nodes(0).Nodes.Count - 1

            For iDMIndex = 0 To iNodeCount
                'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.tvDataModel.Nodes has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                If objFrmMainForm.tvDataModel.Nodes(0).Nodes.Item(iDMIndex).Checked Then
                    bDataModelSelected = True
                    Exit For
                End If
            Next iDMIndex

            If bDataModelSelected = False Then
                MsgBox("Please select a Data Model to Export", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "PIE")
                objFrmMainForm.txtWarning(1).Text = objFrmMainForm.txtWarning(1).Text & vbCrLf & "Please select a Data Model to Export"
                objFrmMainForm.ProgressBar1(0).Value = 0
                objFrmMainForm.ProgressBar1(1).Value = 0
                ProduceExtractFile = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            'Richard Clarke November 2008 - PIE enhancements
            '-----------------

            'Initialise all the control arrays
            If InitialiseControlArrays(r_oDatabase:=oDatabase, r_lDataModelId:=lDataModelId, r_sDataModelCode:=sDataModelCode, v_generateObjectConstants:=False, r_lSiriusUserId:=g_lSiriusUserId) <> gPMConstants.PMEReturnCode.PMTrue Then

                'If the initalisation  has failed,
                'end program as the control arrays are vital to the
                'the operation of the program
                ProduceExtractFile = gPMConstants.PMEReturnCode.PMFalse
                CloseBinaryFile(v_iFileNumber:=iFileNumber)
                Exit Function
            End If

            'CREATE GUIDS
            'we need to run the createguids function here, then re-run
            'initialisecontrolarrays with the new column definitions
            m_lReturn = CreateGUIDs(oDatabase, g_aIeControl)
            'Initialise all the control arrays
            If InitialiseControlArrays(r_oDatabase:=oDatabase, r_lDataModelId:=lDataModelId, r_sDataModelCode:=sDataModelCode, v_generateObjectConstants:=False, r_lSiriusUserId:=g_lSiriusUserId) <> gPMConstants.PMEReturnCode.PMTrue Then

                'If the initalisation  has failed,
                'end program as the control arrays are vital to the
                'the operation of the program
                ProduceExtractFile = gPMConstants.PMEReturnCode.PMFalse
                CloseBinaryFile(v_iFileNumber:=iFileNumber)
                Exit Function
            End If
            'Richard Clarke November 2008 - END PIE enhancements

            If g_bStopProcessing Then
                ' Don't do export section
            Else

                objFrmMainForm.StatusBar_TextWrite("Extracting", 0)
                objFrmMainForm.StatusBar_TextWrite("Opening the extract file", 1)

                'Richard Clarke November 2008 - PIE enhancements
                'we need to write the user spufiles out so they can be extracted.
                'check the user has ticked the export spuICCS% box
                If objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_SPUICCS).CheckState = 1 Then
                    'we need to create the user SPU files to directory
                    m_lReturn = CreateUserSPUFiles(r_oDatabase:=oDatabase)
                Else
                    m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    StopProcessing(iFileNumber)
                    ProduceExtractFile = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                'write out the user report spus and crystal report files
                'check the user has ticked the export all reports box
                If objFrmMainForm.chkAdditionalExportOptions(chkAdditionalExportOptions_SPUReports).CheckState = 1 Then
                    'm_lReturn = CreateSPUReportFiles(r_oDatabase:=oDatabase)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        StopProcessing(iFileNumber)
                        ProduceExtractFile = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If
                    'note the crystal report files are exported as binaries
                    'in the main doExport function.

                End If

                'Richard Clarke November 2008 - PIE enhancements
                'renamed the arguments in doexport for data model code and id
                'v_alDataModelId was v_lDataModelId, v_asDataModelCode was v_sDataModelCode

                'Begin the extraction operations
                m_lReturn = doExport(r_oDatabase:=oDatabase, v_iFileNumber:=iFileNumber, v_alDataModelID:=lDataModelId, v_asDataModelCode:=sDataModelCode, g_aIeControl:=g_aIeControl, g_aIeTableDefinitions:=g_aIeTableDefinitions, v_sVersionNumber:=sVersionNumber, r_lTotalLinesWritten:=lTotalLinesWritten)

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                StopProcessing(iFileNumber)
                ProduceExtractFile = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            'Set the second panel to indicate extract processing has conpleted
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "Closing the extract file"
            objFrmMainForm.StatusBar_TextWrite("Closing the extract file", 1)
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = conEmptyString
            objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)
            'Close the extract file
            m_lReturn = CloseBinaryFile(v_iFileNumber:=iFileNumber)

            'now write the length information
            tFilePath = OpenBinaryFile(i_AccessType:=WriteRandom, i_sFilePath:=tFilePath, i_sFileName:=tFileName, i_sFileExtension:=tFileExtension, o_iFileNumber:=iFileNumber)
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            FilePut(iFileNumber, lTotalLinesWritten, 3)
            m_lReturn = CloseBinaryFile(v_iFileNumber:=iFileNumber)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ProduceExtractFile = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            'ensure GUI is correct
            m_lReturn = DoGuiDefaults(v_iImportExport:=1, v_bClearWarning:=False) 'set for export!

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ProduceExtractFile = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            'Stick a message out to indicate success
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If g_bStopProcessing Then
                    m_lReturn = StopProcessing(v_iFileNumber:=iFileNumber)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        writeToStatusBox(("Error(s) during cancellation of export"))
                        ProduceExtractFile = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If
                Else
                    writeToStatusBox(("Export completed sucessfully, " & lTotalLinesWritten & " lines written"))
                End If
            Else
                writeToStatusBox(("Export not completed due to problems"))
            End If

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".ProduceExtractFile")

            Exit Function

        Catch ex As Exception

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ProduceExtractFile")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProduceExtractFile Failed", vApp:=ACApp, vClass:=conEmptyString, vMethod:="ProduceExtractFile", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

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
    Public Function ImportExtractFile(ByRef oDatabase As dPMDAO.Database, ByRef lDataModelId() As Integer, ByRef sDataModelCode() As String, ByRef tFilePath As String, ByRef tFileName As String, ByRef tFileExtension As String, ByVal v_bCheckHeader As Boolean, ByRef sVersionNumber As String, ByVal v_bImportRegistry As Boolean) As Integer

        'lDataModelId As Long, _
        ''sDataModelCode As String, _
        '
        'Define general variables
        Dim iFileNumber As Short
        Dim sMessage As String

        Try

            ' Debug message
            Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ImportExtractFile")

            ImportExtractFile = gPMConstants.PMEReturnCode.PMTrue

            sMessage = "Manual changes have been detected on the target system when compared to its most recent import." & vbCrLf
            sMessage = sMessage & "Click Ok if you wish to proceed with the import or Cancel to review the changes."

            'open the file first incase there is an error
            tFilePath = OpenBinaryFile(i_AccessType:=ReadAccess, i_sFilePath:=tFilePath, i_sFileName:=tFileName, i_sFileExtension:=tFileExtension, o_iFileNumber:=iFileNumber)

            If tFilePath <> conEmptyString Then
                writeToStatusBox(("Error opening import file: " & tFilePath))
                ImportExtractFile = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            'Initialise all the control arrays
            If InitialiseControlArrays(r_oDatabase:=oDatabase, r_lDataModelId:=lDataModelId, r_sDataModelCode:=sDataModelCode, v_generateObjectConstants:=False, r_lSiriusUserId:=g_lSiriusUserId) <> gPMConstants.PMEReturnCode.PMTrue Then

                'If the initalisation  has failed,
                'end program as the control arrays are vital to the
                'the operation of the program
                writeToStatusBox(("Failed to initialise the control arrays"))
                ImportExtractFile = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            '--------------------------------
            'Richard Clarke November 2008 - PIE enhancements
            'check for manual changes to target here.
            If g_bManualChangesChecked = False Then
                m_lReturn = CheckTargetForManualChanges(oDatabase, g_aIeControl)
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    If MsgBox(sMessage, MsgBoxStyle.OkCancel, "PIE") = MsgBoxResult.Ok Then
                        g_bManualChangesChecked = True
                    Else
                        objFrmMainForm.SSTab3.SelectedIndex = 1
                        ImportExtractFile = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If
                End If
            End If
            'Richard Clarke November 2008 - PIE enhancements - END
            '--------------------------------

            '-------------------------------
            'Richard Clarke November 2008 - PIE enhancements
            'need to create the new columns on the tables and then re-run initialise control arrays
            m_lReturn = CreateGUIDs(oDatabase, g_aIeControl)
            'Re-Initialise control arrays now we have new guid and last updated columns
            If InitialiseControlArrays(r_oDatabase:=oDatabase, r_lDataModelId:=lDataModelId, r_sDataModelCode:=sDataModelCode, v_generateObjectConstants:=False, r_lSiriusUserId:=g_lSiriusUserId) <> gPMConstants.PMEReturnCode.PMTrue Then

                'If the initalisation  has failed,
                'end program as the control arrays are vital to the
                'the operation of the program
                writeToStatusBox(("Failed to initialise the control arrays"))
                ImportExtractFile = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            Else
                g_bControlArraysInitialised = True
            End If
            'Richard Clarke November 2008 - PIE enhancements - END
            '-------------------------------

            'Set the first panel to indicate extract processing has started
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "Importing"
            objFrmMainForm.StatusBar_TextWrite("Importing", 1)
            'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = "Opening the extract file"
            objFrmMainForm.StatusBar_TextWrite("Opening the extract file", 2)
            'Begin the import operations
            m_lReturn = doImport(r_oDatabase:=oDatabase, v_iFileNumber:=iFileNumber, v_alDataModelID:=lDataModelId, v_asDataModelCode:=sDataModelCode, g_aIeControl:=g_aIeControl, g_aIeTableDefinitions:=g_aIeTableDefinitions, v_bCheckHeaderOnly:=v_bCheckHeader, v_sVersionNumber:=sVersionNumber, v_bImportRegistry:=v_bImportRegistry)

            'Close the extract file
            '    If m_lReturn = PMTrue Then
            CloseBinaryFile(v_iFileNumber:=iFileNumber)
            '   End If

            'ensure GUI is correct
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = DoGuiDefaults(v_iImportExport:=1, v_bClearWarning:=False) 'set for export!
            End If

            If v_bCheckHeader = False Then
                'Stick a message out to indicate success
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    writeToStatusBox(("Import completed sucessfully"))
                Else
                    writeToStatusBox(("Import not completed due to problems"))
                End If
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ImportExtractFile = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".ImportExtractFile")

            Exit Function

        Catch

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ImportExtractFile")
            writeToStatusBox("Import Process Stopped.")
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportExtractFile Failed", vApp:=ACApp, vClass:=conEmptyString, vMethod:="ImportExtractFile", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try

    End Function
End Module