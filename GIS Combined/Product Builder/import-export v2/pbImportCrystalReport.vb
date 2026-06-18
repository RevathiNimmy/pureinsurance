Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles

Module pbImportCrystalReport
    ' ***************************************************************** '
    '
    ' Name:        ImportCrystalReport
    '
    ' Description: Processes the import of crystal report files from the binary
    '              file. Extracts the file to the target hard disk.
    '              Receives the current control file array row and
    '              the file number of the file being processed
    '
    ' History:     11/11/2008 Richard Clarke  - Created.
    '
    ' ***************************************************************** '
    Public Function ImportCrystalReport(ByRef r_oDatabase As dPMDAO.Database, ByVal v_iFileNumber As Short, ByVal v_aIeControl As Object, ByVal v_aIeTableDefinitions As Object, ByVal v_iIntegerData As Short) As Integer
        Dim m_lReturn As Object
        Dim ACClass As Object

        'Define array to hold the retrieved data
        Dim aRetrievedData As Object
        Dim sPathName As String
        Dim sSubKey As String

        Try

            ' Debug message
            'UPGRADE_WARNING: Couldn't resolve default property of object ACClass. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ImportCrystalReport")

            ImportCrystalReport = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of import
            With objFrmMainForm
                .StatusBar_TextWrite("ImportCrystalReport", 1)
                'UPGRADE_WARNING: Lower bound of collection mainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                '.StatusBar1(1).Items.Item(1).Text = "ImportCrystalReport"
                'UPGRADE_WARNING: Lower bound of collection mainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                .StatusBar_TextWrite(conEmptyString, 2)
                '.StatusBar1(2).Items.Item(1).Text = conEmptyString

                'Read a row passing in the definition for the row
                'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(v_iIntegerData)(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object m_lReturn. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                m_lReturn = GetBinaryFileRow(v_iFileNumber:=v_iFileNumber, v_vObjectType:=v_aIeControl(v_iIntegerData)(0), r_aDataDefinition:=v_aIeTableDefinitions(v_iIntegerData)(pbIeTableDefinitions_columnArray), aRetrievedData:=aRetrievedData, rowIndex:=v_iIntegerData)

                'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Lower bound of collection mainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                .StatusBar_TextWrite(aRetrievedData(0), 2)
                '.StatusBar1(2).Items.Item(1).Text = aRetrievedData(0)

            End With

            'sPathName = "D:\Program Files\PM\Sirius Core\Server\Reports"
            'UPGRADE_WARNING: Couldn't resolve default property of object m_lReturn. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="Reports", r_sSettingValue:=sPathName)
            'full path shouled by in the array of retrieved data
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object m_lReturn. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                m_lReturn = WriteCompressedFile(v_sPathName:=sPathName & aRetrievedData(0), r_vTheData:=aRetrievedData(2), v_lTheOriginalSize:=aRetrievedData(1))
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ImportCrystalReport = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            Exit Function

        Catch Ex As Exception

            LogPIEError("", True, False, "", False, "", "")

            ' Debug message
            'UPGRADE_WARNING: Couldn't resolve default property of object ACClass. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ImportCrystalReport")

            ImportCrystalReport = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportCrystalReport Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportCrystalReport", vErrNo:=Err.Number, vErrDesc:=Err.Description)

    End Function
End Module
