Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
Imports SharedFiles
Imports System.IO
Imports Ionic.Zip

Module pbImportDocumentTemplate

    Private Const ACClass As String = conEmptyString
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_sDocPath As String

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
    Public Function ImportDocumentTemplate(ByVal v_iFileNumber As Integer, ByVal v_aIeControl() As Object, ByVal v_aIeTableDefinitions As Object, _
                                           ByVal v_iIntegerData As Integer) As Integer

        'Define array to hold the retrieved data
        Dim result As Integer = 0

        Dim aRetrievedData As Object        
        Dim iFileNumber As gPMConstants.PMEReturnCode
        Dim sPathName As String = ""

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ImportDocumentTemplate")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the panel to indicate the  type of import
            objFrmMainForm.StatusBar1(1).Items.Item(0).Text = "Document Template"
            objFrmMainForm.StatusBar1(2).Items.Item(0).Text = CStr(v_aIeControl(v_iIntegerData)(pbIeControl_objectName))

            'Read a row passing in the definition for the row
            m_lReturn = GetBinaryFileRow(v_iFileNumber:=v_iFileNumber, v_vObjectType:=CInt(v_aIeControl(v_iIntegerData)(0)), r_aDataDefinition:=v_aIeTableDefinitions(v_iIntegerData)(pbIeTableDefinitions_columnArray), aRetrievedData:=aRetrievedData, rowIndex:=v_iIntegerData)

            'Get the value
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                iFileNumber = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="DocServer", v_sSubKey:=conEmptyString, r_sSettingValue:=sPathName), gPMConstants.PMEReturnCode)
            End If

            sPathName = sPathName & AddRequiredBackslash(sPathName) & CStr(aRetrievedData(0))

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = WriteDocumentTemplate(aRetrievedData)
                'm_lReturn = CType(WriteCompressedFile(v_sPathName:=sPathName, r_vTheData:=aRetrievedData(2), v_lTheOriginalSize:=CInt(aRetrievedData(1))), gPMConstants.PMEReturnCode)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '********************************************************

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ImportDocumentTemplate")

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ImportDocumentTemplate")

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ImportDocumentTemplate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportDocumentTemplate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    'New function to output document template files from the PIE file.
    Private Function WriteDocumentTemplate(ByVal aRetrievedData As Object) As Long

        Dim lReturn As Long

        'all we need to do is write the string out to a temp file in the docs folder
        'delete the existing zip file then create a new one with the correct paths and filename using zipfile

        Dim sXMLFileName As String
        Dim sFolderFile As String
        Dim sBinaryString As String
        Dim sTempFile As String
        Dim sFolderFileArray As String()
        Dim sZipPath As String
        Dim sXMLFilePath As String
        Dim sFilePathArray As String()
        Dim sTypeDir As String
        Dim sTempDel As String

        lReturn = gPMConstants.PMEReturnCode.PMTrue

        Try

            If m_sDocPath = "" Then
                m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="DocServer", v_sSubKey:=conEmptyString, r_sSettingValue:=m_sDocPath)
            End If

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'we need to get the values from the array before we can use them
            sBinaryString = aRetrievedData(2)
            sFolderFile = aRetrievedData(0)

            sTypeDir = Split(sFolderFile, "\")(0)

            If Not Directory.Exists(m_sDocPath + "\" + sTypeDir) Then
                CreateDirectory(m_sDocPath + "\" + sTypeDir + "\")
            End If

            sFolderFileArray = Split(sFolderFile, "|")
            sZipPath = sFolderFileArray(0)
            sXMLFilePath = sFolderFileArray(1)
            sXMLFilePath = Replace(sXMLFilePath, "/", "\")
            sFilePathArray = Split(sXMLFilePath, "\")
            sXMLFileName = sFilePathArray(UBound(sFilePathArray))
            'our documents path and the xml file name
            sTempFile = m_sDocPath + "\" + sXMLFileName
            'remove the .xml filename from the path
            sXMLFilePath = Replace(sXMLFilePath, sXMLFileName, "")
            'write the string out in the stream to the temp xml file in the pm docs folder.
            Dim sw As New StreamWriter(sTempFile)
            m_lReturn = RemoveInvalidCharacters(sBinaryString)
            sw.Write(sBinaryString)
            sw.Close()

            sTempDel = Replace(sZipPath, ".zip", ".DEL")

            If File.Exists(m_sDocPath + "\" + sZipPath) Then
                'now rename the existing one prior to deletion (just in case zip creation fails then we can rename the existing one back
                Rename(m_sDocPath + "\" + sZipPath, m_sDocPath + "\" + sTempDel)
            End If

            'now create the zip and write it
            Dim zFile As New ZipFile()
            'this call to addfile adds the file in sTempFile to the structure in sxmlfilepath inside the new zip
            zFile.AddFile(sTempFile, sXMLFilePath)
            zFile.Save(m_sDocPath + "\" + sZipPath)

            'now we're done delete the temp file(s)
            File.Delete(sTempFile)
            File.Delete(m_sDocPath + "\" + sTempDel)

            Return lReturn

        Catch ex As Exception
            Rename(m_sDocPath + "\" + sTempDel, m_sDocPath + "\" + sZipPath)
            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    Public Function RemoveInvalidCharacters(ByRef r_sXML As String) As Integer
        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim sTemp, sReplaceTemp As String
        If r_sXML Is Nothing Then
            r_sXML = ""
        End If
        If Len(r_sXML) > 0 Then
            r_sXML = Replace(r_sXML, Chr(145), Chr(39), , , vbTextCompare)
            r_sXML = Replace(r_sXML, Chr(146), Chr(39), , , vbTextCompare)
            r_sXML = Replace(r_sXML, Chr(147), Chr(34), , , vbTextCompare)
            r_sXML = Replace(r_sXML, Chr(148), Chr(34), , , vbTextCompare)
            r_sXML = Replace(r_sXML, Chr(150), Chr(45), , , vbTextCompare)
            r_sXML = Replace(r_sXML, Chr(151), Chr(45), , , vbTextCompare)
            r_sXML = Replace(r_sXML, Chr(160), "", , , vbTextCompare)
            r_sXML = Replace(r_sXML, "&", "&amp;", , , vbTextCompare)
            r_sXML = Replace(r_sXML, "&amp;lt;", "&lt;", , , vbTextCompare)
            r_sXML = Replace(r_sXML, "&amp;gt;", "&gt;", , , vbTextCompare)
            r_sXML = Replace(r_sXML, "&amp;amp;", "&amp;", , , vbTextCompare)
            r_sXML = Replace(r_sXML, Chr(133), "...", , , vbTextCompare)
            sTemp = "<w:lvlText w:val=" + """" + "?" + """/>"
            sReplaceTemp = "<w:lvlText w:val=" + """" + "" + """/>"
            r_sXML = Replace(r_sXML, sTemp, sReplaceTemp, , , vbTextCompare)
            sTemp = "<w:t>?</w:t>"
            sReplaceTemp = "<w:t></w:t>"
            r_sXML = Replace(r_sXML, sTemp, sReplaceTemp, , , vbTextCompare)
            r_sXML = Replace(r_sXML, Chr(183), "•", , , vbTextCompare)
            r_sXML = Replace(r_sXML, Chr(148), Chr(34))
            r_sXML = Replace(r_sXML, Chr(147), Chr(34))
        End If
        Return result

    End Function

End Module