Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles

Module pbImportUserSPU

    Private Const ACClass As String = conEmptyString

    Private m_lReturn As Integer

    ' ***************************************************************** '
    '
    ' Name:        ImportUserSPU
    '
    ' Description: Processes the import of user SPU information from the binary
    '              file. Extracts the file to the target hard disk.
    '              Receives the current control file array row and
    '              the file number of the file being processed
    '
    ' History:     11/11/2008 Richard Clarke  - Created.
    '
    ' ***************************************************************** '
    Public Function ImportUserSPU(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Short, ByVal v_aIeControl As Object, ByVal v_aIeTableDefinitions As Object, ByVal v_iIntegerData As Short) As Integer

        'Define array to hold the retrieved data
        Dim aRetrievedData As Object
        Dim sPathName As String
        Dim sSubKey As String

        Try

            ' Debug message
            Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ImportUsersSPU")

            ImportUserSPU = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of import
            With objFrmMainForm
                'UPGRADE_WARNING: Lower bound of collection mainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                '.StatusBar1(1).Items.Item(1).Text = "ImportUserSPU"
                objFrmMainForm.StatusBar_TextWrite("ImportUserSPU", 1)
                'UPGRADE_WARNING: Lower bound of collection mainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                '.StatusBar1(2).Items.Item(1).Text = conEmptyString
                objFrmMainForm.StatusBar_TextWrite(conEmptyString, 2)

                'Read a row passing in the definition for the row
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(v_iIntegerData)(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                m_lReturn = GetBinaryFileRow(v_iFileNumber:=v_iFileNumber, v_vObjectType:=v_aIeControl(v_iIntegerData)(0), r_aDataDefinition:=v_aIeTableDefinitions(v_iIntegerData)(pbIeTableDefinitions_columnArray), aRetrievedData:=aRetrievedData, rowIndex:=v_iIntegerData)

                'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Lower bound of collection mainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                '.StatusBar1(2).Items.Item(1).Text = aRetrievedData(0)
                objFrmMainForm.StatusBar_TextWrite(aRetrievedData(0), 2)

            End With

            'sSubKey = ACOIMGISSubKey & "\" & v_sDataModelCode & "\" & "ListManagement"

            'this is the path where we're going to export the .sql files to
            sPathName = ""

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                m_lReturn = WriteCompressedFile(v_sPathName:=sPathName & aRetrievedData(0), r_vTheData:=aRetrievedData(2), v_lTheOriginalSize:=aRetrievedData(1))
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ImportUserSPU = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            'now we've extracted the file, we need to execute the SQL script.
            'the filename should be v_sPathName:=sPathName & aRetrievedData(0)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object RunSPUScript(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'Start-PM038738
                'm_lReturn = RunSPUScript(r_oDatabase, sPathName & aRetrievedData(0))
                addToArray(g_UserSPU, sPathName & aRetrievedData(0))
                'End-PM038738
            End If

            ' Debug message
            'Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ImportUserSPU")

            Exit Function

        Catch ex As Exception

            ' Debug message
            'Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ImportUserSPU")

            ImportUserSPU = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportUserSPU Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportUserSPU", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name:        RunSPUScript
    '
    ' Description: Processes the user SPU script file.
    '              Receives the file path and name to execute
    '
    ' History:     11/11/2008 Richard Clarke  - Created.
    '
    ' ***************************************************************** '

    Public Function RunSPUScript(ByRef r_oDatabase As dPMDAO.Database, ByVal sFileInformation As String) As Object

        Dim sSQLFile As String
        Dim sSQLFileLine As String = ""
        Dim iFileNo As Short
        Dim sSQLDropPorc As String = ""
        Dim iLineCount As Integer
        Try
            sSQLFile = ""
            'we need to read in the file contents, and then execute them
            iFileNo = FreeFile
            FileOpen(iFileNo, sFileInformation, OpenMode.Input)

            Do While Not EOF(iFileNo)
                'Input(iFileNo, sSQLFile)
                iLineCount = iLineCount + 1
                sSQLFileLine = LineInput(iFileNo)
                If iLineCount = 1 Then
                    sSQLDropPorc = sSQLFileLine
                ElseIf iLineCount > 1 Then
                    sSQLFile = sSQLFile & vbCrLf & sSQLFileLine
                End If

            Loop
            'close the file (if you dont do this, you wont be able to open it again!)
            FileClose(iFileNo)

            'UPGRADE_WARNING: Couldn't resolve default property of object RunSPUScript. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            RunSPUScript = r_oDatabase.SQLAction(sSQL:=sSQLDropPorc, sSQLName:="RunSPUScript", bStoredProcedure:=False)
            RunSPUScript = r_oDatabase.SQLAction(sSQL:=sSQLFile, sSQLName:="RunSPUScript", bStoredProcedure:=False)

            Exit Function

        Catch ex As Exception

            ' Debug message
            'Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".RunSPUScript")
            'UPGRADE_WARNING: Couldn't resolve default property of object RunSPUScript. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            RunSPUScript = gPMConstants.PMEReturnCode.PMError
            writeToStatusBox("Failed to import Stored Procedure " & sSQLDropPorc & ". Please check log for details.")
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RunSPUScript Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RunSPUScript", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try
    End Function
End Module