Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
Imports SharedFiles

Module pbImportRuleFile

    Private Const ACClass As String = conEmptyString

    Private m_lReturn As gPMConstants.PMEReturnCode

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
    Public Function ImportRuleFile(ByVal v_iFileNumber As Integer, ByVal v_aIeControl As Object, ByVal v_aIeTableDefinitions As Object, ByVal v_lIntegerData As Integer) As Integer

        'Define array to hold the retrieved data
        Dim result As Integer = 0
        Dim aRetrievedData As Object
        Dim sPathName As String = ""

        Try

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ImportRuleFile")

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of import
            objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Rule File"

            'Read a row passing in the definition for the row

            m_lReturn = CType(GetBinaryFileRow(v_iFileNumber:=v_iFileNumber, v_vObjectType:=CInt(v_aIeControl(v_lIntegerData)(0)), r_aDataDefinition:=v_aIeTableDefinitions(v_lIntegerData)(pbIeTableDefinitions_columnArray), aRetrievedData:=aRetrievedData, rowIndex:=v_lIntegerData), gPMConstants.PMEReturnCode)

            objFrmMainForm.StatusBar1(2).Items.Item(0).Text = CStr(aRetrievedData(0))

            'Get the path for the rule files from the registry
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", v_sSubKey:="GIS", r_sSettingValue:=sPathName), gPMConstants.PMEReturnCode)
            End If

            'Set the full path

            sPathName = sPathName & AddRequiredBackslash(sPathName) & CStr(aRetrievedData(0))

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = CType(WriteCompressedFile(v_sPathName:=sPathName, r_vTheData:=aRetrievedData(2), v_lTheOriginalSize:=CInt(aRetrievedData(1))), gPMConstants.PMEReturnCode)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                writeToStatusBox("Error : Could not write " & sPathName) 'don't stop the export
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ImportRuleFile")

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ImportRuleFile")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportRuleFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportRuleFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
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

        Dim result As Integer = 0
        Dim sUNcompressedString As String = ""
        'Dim lError As gPMConstants.PMEReturnCode
        'Dim oPMZipper As bPMZipper.Business
        Dim iFileNumber As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Create a new instrance of the zipper object
            'oPMZipper = New bPMZipper.Business()

            sUNcompressedString = New String(" "c, v_lTheOriginalSize)
            sUNcompressedString = r_vTheData

            'uncompress the file string, overwritting the old value in the process
            'the compression has been removed from the export in function ExtractFile so we needed to remove
            'the decompression call also - this will need investigating and recoding if we have to have file compression
            'lError = oPMZipper.DecompressString(TheString:=sUNcompressedString, OrigSize:=v_lTheOriginalSize)
            'Bin the zipper object
            'oPMZipper = Nothing

            ' BSJ May 09 - Remove this as it is causing last line to be removed
            'Here Remove all blank spaces from the last line of file (SUR)
            '    Dim str() As String
            '    Dim iIndex As Integer
            '    Dim strFin As String
            '    str = Split(sUNcompressedString, Chr(13))
            '    For iIndex = 0 To UBound(str) - 1
            '        strFin = strFin & Chr(13) & Trim(str(iIndex))
            '    Next
            '    sUNcompressedString = strFin
            '
            ' BSJ May 09 - Remove this as it is causing last line to be removed

            '********************************************************
            'Initialise a new file handle for the file to be created
            iFileNumber = FileSystem.FreeFile()

            'Open for write and lock for our use using the common processing.
            'Slightly non standard as the full path and extension is included
            'in the variable sPathName.
            OpenBinaryFile(i_AccessType:=WriteAccess, i_sFilePath:=v_sPathName, i_sFileName:=conEmptyString, i_sFileExtension:=conEmptyString, o_iFileNumber:=iFileNumber)

            'Write the contents of the (uncomressed) data held in the array
            'to the new file
            FileSystem.FilePut(iFileNumber, sUNcompressedString)

            'close the file using the standard file processing
            m_lReturn = CloseBinaryFile(v_iFileNumber:=iFileNumber)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch
            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function
End Module
