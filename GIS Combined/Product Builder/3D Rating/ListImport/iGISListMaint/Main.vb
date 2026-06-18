Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.IO
Imports SharedFiles
Imports System.Text
Module Main
    ' ***************************************************************** '
    ' Module Name: Main
    '
    ' Date: 28/06/2002
    '
    ' Description:  This contains the main constants
    '
    ' Edit History:
    '   28/06/2002 SJP  - Tidied up after merge from Carole Nash
    ' ***************************************************************** '
    Public Const ACClass As String = "List Imports"
    'developer guide no. 107
    <ThreadStatic()> _
    Public m_lReturn As gPMConstants.PMEReturnCode
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_sUsername As String = ""
    Public g_sPassword As New FixedLengthString(30)
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iUserID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_sCallingAppName As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iCurrencyID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iLogLevel As Integer
    Public m_bIsServer As Boolean
    'developer guide no. 107
    <ThreadStatic()> _
    Public m_oBusiness As Object
    'developer guide no. 107
    <ThreadStatic()> _
    Public m_oLookupBusiness As Object

    Public sTempFolder As String = "c:\temp\"

    Public Const ACApp As String = "List Maintenance"


    Public Const kSelectorChar As String = "..."

    Public Const conMaxRecords As Integer = 500

    ' ***************************************************************** '
    '
    ' Name:             splitfile(r_filename As String, r_filelength As Long)
    '
    ' Description:      splits a file into smaller files
    '
    ' History:          07/01/03    APS     created
    '
    ' ***************************************************************** '
    Public Sub splitfile(ByRef r_filename As String, ByRef r_filelength As Integer)
        Dim bStopped As Boolean
        Dim strSaveFileName As String = ""
        Dim lFreeFile2 As Integer
        Dim sRecord As String = ""


        Dim file As FileStream

        Dim fso As New Object

        Dim lRecordCount As Integer = 0
        Dim iSplitFileNumber As Integer = 0
        Dim lRecordsAlreadyCopied As Integer = 0

        'Ensure the directory exists
        If Not (Directory.Exists(sTempFolder)) Then
            Directory.CreateDirectory(sTempFolder)
        End If

        Do While (lRecordsAlreadyCopied < r_filelength)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            strSaveFileName = sTempFolder & "splitfile00" & iSplitFileNumber & ".000"

            file = New FileStream(strSaveFileName, FileMode.CreateNew)

            lFreeFile2 = FileSystem.FreeFile()
            FileSystem.FileOpen(lFreeFile2, r_filename, OpenMode.Input)

            bStopped = False
            Dim w As StreamWriter
            w = New StreamWriter(file)

            Do While Not FileSystem.EOF(lFreeFile2) And Not bStopped
                sRecord = FileSystem.LineInput(lFreeFile2)
                If (lRecordCount >= lRecordsAlreadyCopied) And (lRecordCount < lRecordsAlreadyCopied + conMaxRecords) Then
                    w.WriteLine(sRecord)
                    w.Flush()
                End If

                If lRecordCount > (lRecordsAlreadyCopied + conMaxRecords) Then
                    bStopped = True
                End If

                lRecordCount += 1
            Loop

            lRecordCount = 0
            iSplitFileNumber += 1

            lRecordsAlreadyCopied += conMaxRecords

            w.Close()

            FileSystem.FileClose(lFreeFile2)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Loop


    End Sub

    ' ***************************************************************** '
    '
    ' Name:             GetFileLength(fileName As String) As Long
    '
    ' Description:      returns the length of a given file
    '
    ' History:          07/01/03    APS     created
    '
    ' ***************************************************************** '
    Function GetFileLength(ByRef fileName As String) As Integer
        Dim sRecord As String = ""
        Dim lRecordCount As Integer

        Dim lFreeFile As Integer = FileSystem.FreeFile()
        Dim lFieldCount As Integer = 0

        FileSystem.FileOpen(lFreeFile, fileName, OpenMode.Input)

        Do While Not FileSystem.EOF(lFreeFile)
            sRecord = FileSystem.LineInput(lFreeFile)
            lRecordCount += 1
        Loop

        FileSystem.FileClose(lFreeFile)

        Return lRecordCount

    End Function

    ' ***************************************************************** '
    '
    ' Name:             deleteSplitFiles()
    '
    ' Description:      deletes any files that were created by spliting them
    ' History:          07/01/03    APS     created
    '
    ' ***************************************************************** '
    Sub deleteSplitFiles()
        Dim file As FileInfo

        Dim fso As New Object

        Dim strFileName As String = FileSystem.Dir(sTempFolder & "*.000", FileAttribute.Normal)

        Do While strFileName <> ""

            strFileName = sTempFolder & strFileName
            If System.IO.File.Exists(strFileName) Then
                file = New FileInfo(strFileName)
                file.Delete()
            End If

            strFileName = FileSystem.Dir(sTempFolder & "*.000", FileAttribute.Normal)

        Loop


    End Sub



    '   SJP 28/06/2002 - Commented out as should use iPMFunc version
    'Public Sub LogMessage( _
    ''    iType As Integer, sMsg As String, Optional vApp As Variant, _
    ''    Optional vClass As Variant, Optional vMethod As Variant, _
    ''    Optional vErrNo As Variant, Optional vErrDesc As Variant)
    '
    'Dim lReturn As Long
    'Dim oMessage As Object
    '
    '    ' CTAF 270701
    '    On Error Resume Next
    '
    '    ' Create an instance of the message object
    '    Set oMessage = CreateObject("iPMMessage.PMMessageV2")
    '
    '    ' CTAF 270701
    '    On Error GoTo Err_LogMessage
    '
    '    If ((oMessage Is Nothing) = False) Then
    '
    '        ' Log the message
    '        lReturn& = oMessage.LogMessage( _
    ''                        iType:=iType, _
    ''                        sMsg:=sMsg, _
    ''                        vApp:=vApp, _
    ''                        vClass:=vClass, _
    ''                        vMethod:=vMethod, _
    ''                        vErrNo:=vErrNo, _
    ''                        vErrDesc:=vErrDesc)
    '        If (lReturn& <> PMTrue) Then
    '            ' If it fails, then
    '            LogMessagePopup _
    ''                iType:=iType%, _
    ''                sMsg:=sMsg$, _
    ''                vApp:=vApp, _
    ''                vClass:=vClass, _
    ''                vMethod:=vMethod, _
    ''                vErrNo:=vErrNo, _
    ''                vErrDesc:=vErrDesc
    '        End If
    '
    '        Set oMessage = Nothing
    '
    '    Else
    '
    '        ' CTAF 270701 - Log the message as normal instead
    '
    '        ' Failed to log message, so we must call the
    '        ' function to popup the message instead.
    '        LogMessagePopup _
    ''            iType:=iType%, _
    ''            sMsg:=sMsg$, _
    ''            vApp:=vApp, _
    ''            vClass:=vClass, _
    ''            vMethod:=vMethod, _
    ''            vErrNo:=vErrNo, _
    ''            vErrDesc:=vErrDesc
    '
    '    End If
    '
    '
    '    Exit Sub
    '
    'Err_LogMessage:
    '
    '    ' Error Section.
    '
    '    ' Failed to log message, so we must call the
    '    ' function to popup the message instead.
    '    LogMessagePopup _
    ''        iType:=iType%, _
    ''        sMsg:=sMsg$, _
    ''        vApp:=vApp, _
    ''        vClass:=vClass, _
    ''        vMethod:=vMethod, _
    ''        vErrNo:=vErrNo, _
    ''        vErrDesc:=vErrDesc
    '
    '    Exit Sub
    '
    'End Sub
    '

    Public Function GetSiriusInstType(ByRef bSiriusInstType As Boolean) As Integer

        Dim result As Integer = 0
        Const ACRegSiriusSetupInstType As String = "Server"
        Dim sSiriusInstType As String = ""

        Try

            result = gPMConstants.PMEReturnCode.pmfalse

            ' get version number
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLSetup, v_sSettingName:=ACRegSiriusSetupInstType, r_sSettingValue:=sSiriusInstType), gPMConstants.PMEReturnCode)

            bSiriusInstType = sSiriusInstType.Trim().ToUpper() = ("Yes").ToUpper()


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function




    ' ***************************************************************** '
    '
    ' Name: ImportList
    '
    ' Description:  This will import a list
    '
    ' History: 28/06/2002 SJP - kept from current code
    '
    ' ***************************************************************** '

    Public Function ImportList(ByRef sFile As String, ByRef vData(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lFreeFile As Integer
        Dim sRecord As String = ""
        Dim lFieldCount, lTotalFields, lLastCommaPos, lRecordCount As Integer
        Dim bIsInBetweenDoubleQuote As Boolean
        Dim lLastDoubleQuotePosition, lCountOfConsecutiveDoubleQuotes As Integer
        Dim sNewStringPartOne As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get free file no
            lFreeFile = FileSystem.FreeFile()

            'set field count to zero
            lFieldCount = 0
            FileSystem.FileClose()
            'open file
            FileSystem.FileOpen(lFreeFile, sFile, OpenMode.Input)

            'set record count to zero
            lRecordCount = -1

            'do until no records are left
            Do While Not FileSystem.EOF(lFreeFile)

                'TR - Reset counters
                lCountOfConsecutiveDoubleQuotes = 0

                'get record from the ile
                sRecord = FileSystem.LineInput(lFreeFile)

                'if first time around then count fields
                If lRecordCount = -1 Then
                    lTotalFields = 0
                    For i As Integer = 1 To sRecord.Length
                        'TR - If we are inbetween Quote marks, ignore commas, otherwise use them to mark new fields
                        If Not bIsInBetweenDoubleQuote Then
                            If Mid(sRecord, i, 1) = "," Then
                                lTotalFields += 1
                            End If
                        End If

                        'TR - Is this a double Quote mark (to allow commas in description)
                        If Mid(sRecord, i, 1) = ChrW(34) Then
                            bIsInBetweenDoubleQuote = Not (bIsInBetweenDoubleQuote)
                        End If
                    Next i
                    'dim array to match fields
                    ReDim vData(lTotalFields, 0)
                End If

                'increment record count
                lRecordCount += 1

                'resize array to add new record
                ReDim Preserve vData(lTotalFields, lRecordCount)

                'set first field start to 0
                lLastCommaPos = 0
                lFieldCount = 0
                lLastDoubleQuotePosition = 0
                bIsInBetweenDoubleQuote = False

                'step thru char by char
                For i As Integer = 1 To sRecord.Length
                    'TR - If this is the char in the last string, then it can't be a delimiter comma
                    If i = sRecord.Length Then
                        'TR - If this is a , or a " then remove it
                        If Mid(sRecord, i, 1) = "," Or Mid(sRecord, i, 1) = ChrW(34) Then
                            'load into array string between last comma and current pos

                            vData(lFieldCount, lRecordCount) = Mid(sRecord, lLastCommaPos + 1, i - lLastCommaPos - 1)
                            If Mid(sRecord, i, 1) = ChrW(34) Then
                                bIsInBetweenDoubleQuote = False
                            End If
                        Else
                            'load into array string between last comma and current pos

                            vData(lFieldCount, lRecordCount) = Mid(sRecord, lLastCommaPos + 1, i - lLastCommaPos)
                        End If
                    Else
                        'TR - If we are inbetween Quote marks, ignore commas, otherwise use them to mark new fields
                        If Not bIsInBetweenDoubleQuote Then
                            'TR - look for delimiter commas
                            If Mid(sRecord, i, 1) = "," Then

                                'load into array string between last comma and current pos

                                vData(lFieldCount, lRecordCount) = Mid(sRecord, lLastCommaPos + 1, i - lLastCommaPos - 1)

                                'increment no of fields
                                lFieldCount += 1

                                'set pos of new comma
                                lLastCommaPos = i
                            End If
                        End If

                        'TR - Is this a double Quote mark (to allow commas in description)
                        If Mid(sRecord, i, 1) = ChrW(34) Then
                            'TR - First check that this double quote does not immediately follow another one
                            If lLastDoubleQuotePosition = i - 1 Then
                                'TR - Are we in a "between quotes" status or merely a "prefix quotes"
                                'Even number of "s (before this new one), remove this one
                                If lCountOfConsecutiveDoubleQuotes Mod 2 = 0 Then
                                    'TR - Remove this " from the string
                                    sNewStringPartOne = sRecord.Substring(0, i - 1) & sRecord.Substring(sRecord.Length - (sRecord.Length - i))
                                    sRecord = sNewStringPartOne
                                    'TR - Reduce i by 1 (to make up for deleting this character)
                                    i -= 1
                                End If
                                'TR -Add one to the count of consecutive Quote marks
                                lCountOfConsecutiveDoubleQuotes += 1
                            Else
                                'TR - Remove this " from the string
                                sNewStringPartOne = sRecord.Substring(0, i - 1) & sRecord.Substring(sRecord.Length - (sRecord.Length - i))
                                sRecord = sNewStringPartOne
                                'TR - Reduce i by 1 (to make up for deleting this character)
                                i -= 1
                                'TR - Reset consecutive counter
                                lCountOfConsecutiveDoubleQuotes = 1
                            End If
                            'Switch modes
                            bIsInBetweenDoubleQuote = Not (bIsInBetweenDoubleQuote)
                            lLastDoubleQuotePosition = i
                        Else
                            lCountOfConsecutiveDoubleQuotes = 0
                        End If
                    End If
                Next i
            Loop

            FileSystem.FileClose(lFreeFile)

            Return result

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to import list", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Module
