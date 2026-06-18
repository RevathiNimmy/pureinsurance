Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles
Imports System.IO


Module pbImportDocumentTemplate

    Private Const ACClass As String = conEmptyString

    Private m_lReturn As Integer

    ' ***************************************************************** '
    '
    ' Name:        ImportDocumentTemplate
    '
    ' Description: Processes the import of Document Template from the binary
    '              file.  Recieves the current control file array row and
    '              the file number of the file being processed
    '
    ' History:     30/08/2002 JB  - Created.
    '              24/09/2002 SJP - Changed to be a function so can pass
    '                               back if function was successful or not.
    '
    ' ***************************************************************** '
    Public Function ImportDocumentTemplate(ByVal v_iFileNumber As Short, ByVal v_aIeControl As Object, ByVal v_aIeTableDefinitions As Object, ByVal v_iIntegerData As Short) As Integer

        'Define array to hold the retrieved data
        Dim aRetrievedData As Object
        'Get the target path of the file !
        Dim iFileNumber As Short
        Dim sPathName As String
        Dim sDocPath As String
        Dim m_sZIP_DIRECTORY As String
        Dim sDocTempPath() As String
        Dim oZipper As New bPMZipper.Business

        ' Debug message
        Debug.Print(VB.Timer() & ": Entering " & ACApp & conDot & ACClass & ".ImportDocumentTemplate")

        Try

            ImportDocumentTemplate = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of import
            objFrmMainForm.StatusBar_TextWrite("Document Template", 1)
            objFrmMainForm.StatusBar_TextWrite(v_aIeControl(v_iIntegerData)(pbIeControl_objectName), 2)
            'Read a row passing in the definition for the row
            m_lReturn = GetBinaryFileRow(v_iFileNumber:=v_iFileNumber, v_vObjectType:=v_aIeControl(v_iIntegerData)(0), r_aDataDefinition:=v_aIeTableDefinitions(v_iIntegerData)(pbIeTableDefinitions_columnArray), aRetrievedData:=aRetrievedData, rowIndex:=v_iIntegerData)

            '********************************************************
            'Get the value
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                iFileNumber = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="DocServer", v_sSubKey:=conEmptyString, r_sSettingValue:=sPathName)
            End If

            sDocTempPath = aRetrievedData(0).ToString().Trim().Split("|")

            m_sZIP_DIRECTORY = CStr(ReadRegistry(gPMConstants.HKEY_LOCAL_MACHINE, "SOFTWARE\Pure\PureInstallation\Client", "DocZipPMDir")) & "\" & g_sUsername
            m_lReturn = CreateFolderTree(m_sZIP_DIRECTORY, True)

            sDocPath = m_sZIP_DIRECTORY & AddRequiredBackslash(m_sZIP_DIRECTORY) & Path.GetFileName(sDocTempPath(1))

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = WriteCompressedFile(v_sPathName:=sDocPath, r_vTheData:=aRetrievedData(2), v_lTheOriginalSize:=aRetrievedData(1))
            End If

            If File.Exists(sDocPath) Then
                m_lReturn = CreateDirectory(sPathName & AddRequiredBackslash(sPathName) & sDocTempPath(0).Substring(0, sDocTempPath(0).LastIndexOf("\")))
                Dim bDone As Boolean = oZipper.ZipFile(sDocPath, sPathName & AddRequiredBackslash(sPathName) & sDocTempPath(0))
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ImportDocumentTemplate = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            '********************************************************

            ' Debug message
            Debug.Print(VB.Timer() & ": Exiting " & ACApp & conDot & ACClass & ".ImportDocumentTemplate")

            Exit Function

        Catch ex As Exception

            ImportDocumentTemplate = gPMConstants.PMEReturnCode.PMError

            ' Debug message
            Debug.Print(VB.Timer() & ": Errored in " & ACApp & conDot & ACClass & ".ImportDocumentTemplate")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportDocumentTemplate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportDocumentTemplate", vErrNo:=Err.Number, vErrDesc:=Err.Description)

    End Function
End Module