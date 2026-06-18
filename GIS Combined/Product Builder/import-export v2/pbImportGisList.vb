Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles

Module pbImportGisList

    Private Const ACClass As String = conEmptyString

    Private m_lReturn As Integer

    ' ***************************************************************** '
    '
    ' Name:        ImportGisList
    '
    ' Description: Processes the import of Gis List information from the binary
    '              file.  Recieves the current control file array row and
    '              the file number of the file being processed
    '
    ' History:     30/08/2002 JB  - Created.
    '              24/09/2002 SJP - Changed to be a function, so can
    '                               pass back if function was successful.
    '
    ' ***************************************************************** '
    Public Function ImportGisList(ByVal v_iFileNumber As Short, ByVal v_aIeControl As Object, ByVal v_aIeTableDefinitions As Object, ByVal v_lIntegerData As Integer, ByVal v_sDataModelCode As String) As Integer

        'Define array to hold the retrieved data
        Dim aRetrievedData As Object
        Dim sPathName As String
        Dim sSubKey As String

        On Error GoTo Err_ImportGisList

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ImportGisList")

        ImportGisList = gPMConstants.PMEReturnCode.PMTrue

        'Set the panel to indicate the  type of import
        With objFrmMainForm
            'UPGRADE_WARNING: Lower bound of collection mainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            '.StatusBar1(1).Items.Item(1).Text = "ImportGisList"
            .StatusBar_TextWrite("ImportGisList", 1)
            'UPGRADE_WARNING: Lower bound of collection mainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            '.StatusBar1(2).Items.Item(1).Text = conEmptyString
            .StatusBar_TextWrite(conEmptyString, 2)

            'Read a row passing in the definition for the row
            'UPGRADE_WARNING: Couldn't resolve default property of object v_aIeControl(v_lIntegerData)(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            m_lReturn = GetBinaryFileRow(v_iFileNumber:=v_iFileNumber, v_vObjectType:=v_aIeControl(v_lIntegerData)(0), r_aDataDefinition:=v_aIeTableDefinitions(v_lIntegerData)(pbIeTableDefinitions_columnArray), aRetrievedData:=aRetrievedData, rowIndex:=v_lIntegerData)

            'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Lower bound of collection mainForm.StatusBar1().Panels has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            '.StatusBar1(2).Items.Item(1).Text = aRetrievedData(0)
            .StatusBar_TextWrite(aRetrievedData(0), 2)
        End With

        sSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & v_sDataModelCode & "\" & "ListManagement"

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ServerListFilePath", r_sSettingValue:=sPathName, v_sSubKey:=sSubKey)
        End If

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(0). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object aRetrievedData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            m_lReturn = WriteCompressedFile(v_sPathName:=sPathName & aRetrievedData(0), r_vTheData:=aRetrievedData(2), v_lTheOriginalSize:=aRetrievedData(1))
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ImportGisList = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ImportGisList")

        Exit Function

Err_ImportGisList:

        ' Debug message
        Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ImportGisList")

        ImportGisList = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportGisList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportGisList", vErrNo:=Err.Number, vErrDesc:=Err.Description)

    End Function
End Module