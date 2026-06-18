Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles

Module pbImportRuleFile

    Private Const ACClass As String = conEmptyString

    Private m_lReturn As Integer

    ' ***************************************************************** '
    '
    ' Name:        ImportRuleFile
    '
    ' Description: Processes the import of Rule files from the binary
    '              file.  Recieves the current control file array row and
    '              the file number of the file being processed
    '
    ' History:     30/08/2002 JB  - Created.
    '              24/09/2002 SJP - Changed to be a function so can pass
    '                               back if function was successful or not.
    '
    ' ***************************************************************** '
    Public Function ImportRuleFile(ByVal v_iFileNumber As Short, ByVal v_aIeControl As Object, ByVal v_aIeTableDefinitions As Object, ByVal v_lIntegerData As Integer) As Integer

        'Define array to hold the retrieved data
        Dim aRetrievedData As Object
        Dim sPathName As String

        Try

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ImportRuleFile")

        ImportRuleFile = gPMConstants.PMEReturnCode.PMTrue

        'Set the panel to indicate the  type of import
        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        'objFrmMainForm.StatusBar1(1).Items.Item(1).Text = "Rule File"
        objFrmMainForm.StatusBar_TextWrite("Rule File", 1)
        'Read a row passing in the definition for the row
        'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(v_lIntegerData)(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        m_lReturn = GetBinaryFileRow(v_iFileNumber:=v_iFileNumber, v_vObjectType:=v_aIeControl(v_lIntegerData)(0), r_aDataDefinition:=v_aIeTableDefinitions(v_lIntegerData)(pbIeTableDefinitions_columnArray), aRetrievedData:=aRetrievedData, rowIndex:=v_lIntegerData)

        'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Lower bound of collection objFrmMainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'objFrmMainForm.StatusBar1(2).Items.Item(1).Text = aRetrievedData(0)
        objFrmMainForm.StatusBar_TextWrite(aRetrievedData(0), 2)
        'Get the path for the rule files from the registry
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", v_sSubKey:="GIS", r_sSettingValue:=sPathName)
        End If

        'Set the full path
        'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sPathName = sPathName & AddRequiredBackslash(sPathName) & aRetrievedData(0)

        If (aRetrievedData(1).ToString = "0") Then
            aRetrievedData(2) = ""
        End If
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            m_lReturn = WriteCompressedFile(v_sPathName:=sPathName, r_vTheData:=aRetrievedData(2), v_lTheOriginalSize:=aRetrievedData(1))
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            writeToStatusBox("Error : Could not write " & sPathName)
            ImportRuleFile = gPMConstants.PMEReturnCode.PMTrue 'don't stop the export
            Exit Function
        End If

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ImportRuleFile")

        Exit Function

Catch ex As Exception

        ImportRuleFile = gPMConstants.PMEReturnCode.PMError

        ' Debug message
        Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ImportRuleFile")

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportRuleFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportRuleFile", vErrNo:=Err.Number, vErrDesc:=Err.Description)

    End Function

    ' ***************************************************************** '
    '
    ' Name: WriteCompressedFile
    '
    ' Description:
    '
    ' History: 23/09/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function WriteCompressedFile(ByVal v_sPathName As String, ByRef r_vTheData As Object, ByVal v_lTheOriginalSize As Integer) As Integer

        Dim sUNcompressedString As String
        Dim lError As Integer
        Dim oPMZipper As bPMZipper.Business
        Dim iFileNumber As Short

        Try

            ' Debug message
            Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".WriteCompressedFile")

            WriteCompressedFile = gPMConstants.PMEReturnCode.PMTrue

            'Create a new instrance of the zipper object
            oPMZipper = New bPMZipper.Business

            sUNcompressedString = Space(v_lTheOriginalSize)
            'UPGRADE_WARNING: Couldn't resolve default property of object r_vTheData. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sUNcompressedString = r_vTheData

            If Not UCase(v_sPathName).Contains(".RPT") Then
                'uncompress the file string, overwritting the old value in the process
                lError = oPMZipper.DecompressString(TheString:=sUNcompressedString, OrigSize:=v_lTheOriginalSize)
                'Bin the zipper object
                'UPGRADE_NOTE: Object oPMZipper may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oPMZipper = Nothing
            End If

            '********************************************************
            'Initialise a new file handle for the file to be created
            iFileNumber = FreeFile()

            'Open for write and lock for our use using the common processing.
            'Slightly non standard as the full path and extension is included
            'in the variable sPathName.
            Call OpenBinaryFile(i_AccessType:=WriteAccess, i_sFilePath:=v_sPathName, i_sFileName:=conEmptyString, i_sFileExtension:=conEmptyString, o_iFileNumber:=iFileNumber)

            'Write the contents of the (uncomressed) data held in the array
            'to the new file
            'UPGRADE_WARNING: Put was upgraded to FilePut and has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            Try
                FilePut(iFileNumber, CStr(sUNcompressedString))
            Catch ex As Exception

            End Try


            'close the file using the standard file processing
            m_lReturn = CloseBinaryFile(v_iFileNumber:=iFileNumber)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                WriteCompressedFile = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".WriteCompressedFile")

            Exit Function

        Catch
            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".WriteCompressedFile")

            WriteCompressedFile = gPMConstants.PMEReturnCode.PMError

            Exit Function
        End Try
    End Function
End Module
