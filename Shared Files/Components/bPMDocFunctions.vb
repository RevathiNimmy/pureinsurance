Option Strict Off
Option Explicit On
Imports System.IO
Imports System.Windows.Forms
Imports Word = Microsoft.Office.Interop.Word
Public Module bPMDocFunctions

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "bPMDocFunctions"

    Private m_lReturn As gPMConstants.PMEReturnCode
    Private Const ACDocExportDir As String = "DocArchiveTemp\"
    <ThreadStatic()> Private m_sUsername As String = ""
    ' SET 18/10/2004 ISS13245
    Public Const ACMaxDelayInterval As Integer = 30
    Private m_sWordVersion As String = ""

    Public Declare Function FindWindow Lib "user32.dll" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer

    Public Declare Function IsWindow Lib "user32.dll" (ByVal hwnd As Integer) As Integer

    <ThreadStatic()> Private m_bIsCalledFromBatchProcess As Boolean

    Public Property IsCalledFromBatchProcess() As Boolean
        Get
            Return m_bIsCalledFromBatchProcess
        End Get
        Set(ByVal value As Boolean)
            m_bIsCalledFromBatchProcess = value
        End Set
    End Property


    Public Function GetWordVersion(ByRef r_sVersion As String) As Integer
        Dim result As Integer = 0
        Dim oWord As Object = Nothing
        Try
            Dim lReturn As gPMConstants.PMEReturnCode
            Dim sTemp As String = ""
            Dim lHandle As Integer

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sWordVersion.Length > 0 Then
                ' already know this value
                r_sVersion = m_sWordVersion
                Return result
            End If


            lReturn = CType(StartWord(r_oWord:=oWord, r_lWordHandle:=lHandle, r_sWordVersion:=sTemp), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start Word", vApp:=ACApp, vClass:=ACClass, vMethod:="GetWordVersion", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                oWord = Nothing
                Return result
            End If

            r_sVersion = sTemp

            lReturn = CType(CloseWord(r_oWord:=oWord, lHandle:=lHandle, bSaveChanges:=False), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to close Word", vApp:=ACApp, vClass:=ACClass, vMethod:="GetWordVersion", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                oWord = Nothing
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetWordVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetWordVersion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    Public Function StartWord(ByRef r_oWord As Word.Application, Optional ByRef r_lWordHandle As Integer = 0, Optional ByRef r_sWordVersion As String = "") As Integer
        Dim result As Integer = 0
        Dim sGUID As String = ""

        Try
            Dim sWordVersion, sTemp As String 'sj 08/11/2001
            Dim iTemp As Integer 'MKW 07/11/03 PN7967 Open Word Erroring (with reference to Microsoft KB188546)
            Dim dLoopTime As Date

            result = gPMConstants.PMEReturnCode.PMTrue

            ' SET 18/10/2004 ISS13245 - always create a new instance of word
            'MKW 07/11/03 PN7967 Open Word Erroring (with reference to Microsoft KB188546) START
            '    Set oTemp = CreateObject("Word.Application")
            r_oWord = New Word.Application()

            '
            '    m_lReturn = CloseWord(oTemp, False)
            '    Set oTemp = Nothing

            ' SET 18/10/2004 ISS13245 - wait until word has started
            dLoopTime = DateTime.Now.AddSeconds(ACMaxDelayInterval)
            Do
                Application.DoEvents()
            Loop Until (dLoopTime <= DateTime.Now) Or (WordHasStarted(r_oWord))
            '    Loop Until (dLoopTime <= Now) Or (IsWordRunning(r_oWord) = True)

            ' save the handle for our window
            sTemp = r_oWord.Caption
            r_oWord.Caption = "Tinny Boy"
            r_lWordHandle = FindWindow(Nothing, "Tinny Boy")
            r_oWord.Caption = sTemp


            'r_oWord.CommandBars("Standard").Visible = True

            'r_oWord.CommandBars("Formatting").Visible = True

            If r_lWordHandle = 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse

                sTemp = "Failed To Get Word Handle"
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sTemp, vApp:=ACApp, vClass:=ACClass, vMethod:="StartWord")
                Return result
            End If

            sWordVersion = ""

            ' Now find the version of Word that we're running
            sWordVersion = r_oWord.Application.Version

            If sWordVersion <> "" Then
                iTemp = (sWordVersion.IndexOf("."c) + 1)
                If Conversion.Val(sWordVersion.Substring(0, iTemp)) < 8 Then
                    'sj 08/11/2001 - start
                    'MsgBox "Incorrect Word version for Sirius (" & sWordVersion & ")." & vbCrLf & "Contact Policy Master Support.", , ACApp
                    sTemp = "Incorrect Word version for Sirius (" & sWordVersion & ")." & Strings.Chr(13) & Strings.Chr(10) & "Contact Policy Master Support."
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sTemp, vApp:=ACApp, vClass:=ACClass, vMethod:="StartWord")
                    'sj 08/11/2001 - end
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_oWord = Nothing
                    Return result
                End If
            Else
                'sj 08/11/2001 - start
                sTemp = "An error has occurred with Microsoft Word." & Strings.Chr(13) & Strings.Chr(10) & "Try starting with Word already open or Contact Policy Master Support."
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sTemp, vApp:=ACApp, vClass:=ACClass, vMethod:="StartWord")
                'sj 08/11/2001 - end
                result = gPMConstants.PMEReturnCode.PMFalse
                r_oWord = Nothing
                Return result
            End If

            ' return the version of word
            r_sWordVersion = sWordVersion

            ' store it in a module level variable
            m_sWordVersion = sWordVersion

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start Word", vApp:=ACApp, vClass:=ACClass, vMethod:="StartWord", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    Public Function CloseWord(ByRef r_oWord As Object, ByVal lHandle As Integer, Optional ByRef bSaveChanges As Boolean = True) As Integer
        Dim result As Integer = 0
        Try
            Dim dLoopTime As Date

            result = gPMConstants.PMEReturnCode.PMTrue

            If r_oWord Is Nothing Then
                Return result
            End If

            ' check if word is still running
            If Not IsWordRunning(lHandle) Then
                r_oWord = Nothing
                Debug.WriteLine("Close Word - word not running..")
                Return result
            End If

            ' set the saved property of the template so we are not prompted

            For iCnt As Integer = 1 To r_oWord.Templates.Count

                r_oWord.Templates.Item(iCnt).saved = True

                Debug.WriteLine(r_oWord.Templates.Item(iCnt).FullName)
            Next

            If bSaveChanges Then

                r_oWord.Quit()
            Else

                r_oWord.Quit(SaveChanges:=0)
            End If

            'MKW 061103 PN7967 Risk Register Hang on WS2K3/OfficeXP START
            dLoopTime = DateTime.Now.AddSeconds(ACMaxDelayInterval)
            Do
                Application.DoEvents()
            Loop Until (dLoopTime <= DateTime.Now) Or (Not IsWordRunning(lHandle))
            '    Loop Until (dLoopTime <= Now) Or (IsWordRunning(r_oWord) = False)

            r_oWord = Nothing

            Application.DoEvents()
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            If m_bIsCalledFromBatchProcess = True Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                   sMsg:="Failed to close Word", vApp:=ACApp,
                                                   vClass:=ACClass, vMethod:="CloseWord", vErrNo:=Information.Err().Number,
                                                  vErrDesc:=Information.Err().Description, excep:=Nothing)
            Else
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to close Word", vApp:=ACApp, vClass:=ACClass, vMethod:="CloseWord", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If
            Return result
        End Try
    End Function

    Private Function IsWordRunning(ByVal lWindowHandle As Integer) As Boolean
        Try
            Dim lTemp As Integer

            lTemp = IsWindow(lWindowHandle)

            If lTemp = 0 Then
                Return False
            Else
                Return True
            End If

        Catch
        End Try



        Return False

    End Function

    Private Function WordHasStarted(ByRef oWord As Word.Application) As Boolean
        Try
            Dim sTemp As String = ""

            ' Try and get the name of the object
            sTemp = oWord.Name

            Return True

        Catch



            Return False
        End Try

    End Function

    Public WriteOnly Property Username() As String
        Set(ByVal Value As String)
            m_sUsername = Value
        End Set
    End Property

    Public Function ClearDirectory(ByVal sDirectory As String) As Integer

        Dim result As Integer = 0

        Dim oFolder, oSubFolder As DirectoryInfo
        Dim oFile As FileInfo
        Try


            oFolder = New DirectoryInfo(sDirectory)

            ' Clear the sub folders first
            For Each oSubFolder2 As DirectoryInfo In oFolder.GetDirectories
                oSubFolder = oSubFolder2
                oSubFolder.Delete(True)
            Next oSubFolder2

            ' Clear the files then
            For Each oFile2 As FileInfo In oFolder.GetFiles
                oFile = oFile2
                ' SET 12/10/2004 ISS15027 - forcibly delete the file if it readonly
                '     - locked files will still fail with a Permission Denied error
                oFile.Delete()
            Next oFile2

            oFile = Nothing
            oFolder = Nothing
            oSubFolder = Nothing



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            If m_bIsCalledFromBatchProcess = True Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                   sMsg:="ClearDirectory Failed. Directory Name [" & sDirectory & "]", vApp:=ACApp,
                                                   vClass:=ACClass, vMethod:="ClearDirectory", vErrNo:=Information.Err().Number,
                                                  vErrDesc:=Information.Err().Description, excep:=Nothing)
            Else
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearDirectory Failed. Directory Name [" & sDirectory & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearDirectory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If


            Return result

        End Try
    End Function


    Public Function MoveFolderContents(ByVal sSourceDir As String, ByVal sDestDir As String) As Integer
        Dim result As Integer = 0

        Dim iFolder As DirectoryInfo
        Try
            Dim sTemp As String = ""

            'Changes done as per VB code


            iFolder = New DirectoryInfo(sSourceDir)

            ' copy the folders
            For Each iSubFolder As DirectoryInfo In iFolder.GetDirectories
                sTemp = Path.Combine(sDestDir, iSubFolder.Name)
                iSubFolder.MoveTo(sTemp)
            Next iSubFolder

            ' copy the files
            For Each iFile As FileInfo In iFolder.GetFiles
                sTemp = Path.Combine(sDestDir, iFile.Name)
                iFile.MoveTo(sTemp)
            Next iFile


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            If m_bIsCalledFromBatchProcess = True Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                   sMsg:="MoveFolderContents Failed." & Strings.Chr(13) & Strings.Chr(10) &
                                "Source Directory [" & sSourceDir & "]" & Strings.Chr(13) & Strings.Chr(10) &
                                "Target Directory [" & sDestDir & "]", vApp:=ACApp,
                                                   vClass:=ACClass, vMethod:="MoveFolderContents", vErrNo:=Information.Err().Number,
                                                  vErrDesc:=Information.Err().Description, excep:=Nothing)
            Else
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MoveFolderContents Failed." & Strings.Chr(13) & Strings.Chr(10) &
                                "Source Directory [" & sSourceDir & "]" & Strings.Chr(13) & Strings.Chr(10) &
                                "Target Directory [" & sDestDir & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="MoveFolderContents", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If
            Return result
        End Try
    End Function


    Public Function CreateFolderTree(ByVal sFolderName As String, Optional ByVal ClearDirectoryIfExists As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim lOK As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sFolderName = sFolderName.Trim()
            Debug.WriteLine("Checking folder: " & sFolderName)

            ' check that a folder name was actually supplied
            If sFolderName.Length < 1 Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' is this the actual drive letter
            If Path.GetPathRoot(sFolderName).Substring(0, Path.GetPathRoot(sFolderName).IndexOf(Path.DirectorySeparatorChar)) = sFolderName Then
                Return result
            End If

            ' does the folder already exist
            If Directory.Exists(sFolderName) Then

                ' Check if the flag is set, if the directory exists, we need
                ' to clear the directory
                If ClearDirectoryIfExists Then
                    ' Cleaning the existing folder
                    Debug.WriteLine("Cleaning exitsing folder: " & sFolderName)
                    m_lReturn = CType(ClearDirectory(sFolderName), gPMConstants.PMEReturnCode)
                End If

                Return result
            End If

            ' attempt to create the parent folder
            lOK = CType(CreateFolderTree(sFolderName:=Path.GetDirectoryName(sFolderName)), gPMConstants.PMEReturnCode)

            ' did we error
            If lOK <> gPMConstants.PMEReturnCode.PMTrue Then
                ' return the error
                Return lOK
            End If

            ' create the folder
            Debug.WriteLine("creating folder: " & sFolderName)
            Directory.CreateDirectory(sFolderName)

            ' did we succeed...?
            If Not Directory.Exists(sFolderName) Then
                result = gPMConstants.PMEReturnCode.PMError
                If m_bIsCalledFromBatchProcess = True Then

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                       sMsg:="CreateFolderTree Failed", vApp:=ACApp,
                                                       vClass:=ACClass, vMethod:="CreateFolderTree", vErrNo:=Information.Err().Number,
                                                      vErrDesc:=Information.Err().Description, excep:=Nothing)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateFolderTree Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateFolderTree", vErrNo:=0, vErrDesc:="Unable to create the directory (" & sFolderName & ")")
                End If

            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            If m_bIsCalledFromBatchProcess = True Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                   sMsg:="CreateFolderTree Failed attempting to create " & sFolderName, vApp:=ACApp,
                                                   vClass:=ACClass, vMethod:="CreateFolderTree", vErrNo:=Information.Err().Number,
                                                  vErrDesc:=Information.Err().Description, excep:=Nothing)
            Else
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateFolderTree Failed attempting to create " & sFolderName, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateFolderTree", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If

            Return result

        End Try
    End Function

    Public Function GetDocumentDirectory(ByRef r_sDocDirectory As String) As Integer
        Dim result As Integer = 0
        Try
            Dim sDir As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue
            sDir = ""

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="DocServer", r_sSettingValue:=sDir), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_bIsCalledFromBatchProcess = True Then

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                       sMsg:="Unable to get Document directory from Registry.", vApp:=ACApp,
                                                       vClass:=ACClass, vMethod:="GetDocumentDirectory", vErrNo:=Information.Err().Number,
                                                      vErrDesc:=Information.Err().Description, excep:=Nothing)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Document directory from Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentDirectory", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_sDocDirectory = sDir
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            If m_bIsCalledFromBatchProcess = True Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                   sMsg:="GetDocumentDirectory Failed", vApp:=ACApp,
                                                   vClass:=ACClass, vMethod:="GetDocumentDirectory", vErrNo:=Information.Err().Number,
                                                  vErrDesc:=Information.Err().Description, excep:=Nothing)
            Else
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocumentDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentDirectory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If

            Return result

        End Try
    End Function

    Public Function GetClientDirectory(ByRef r_sClientDir As String, Optional ByVal bUnique As Boolean = False) As Integer
        Dim result As Integer = 0

        Try
            Dim sDir = "", sTemp As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue
            'Changes done as per VB code


            sDir = ""

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="DocClient", r_sSettingValue:=sDir), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_bIsCalledFromBatchProcess = True Then

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                       sMsg:="Unable to get Client directory from Registry.", vApp:=ACApp,
                                                       vClass:=ACClass, vMethod:="GetClientDirectory", vErrNo:=Information.Err().Number,
                                                      vErrDesc:=Information.Err().Description, excep:=Nothing)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Client directory from Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientDirectory", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If
                result = gPMConstants.PMEReturnCode.PMFalse

                Return result
            End If

            sDir = Path.Combine(sDir, m_sUsername.Trim())
            If bUnique Then
                m_lReturn = CType(GetUniqueName(r_sName:=sTemp), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If m_bIsCalledFromBatchProcess = True Then

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                           sMsg:="Unable to get a temp directory.", vApp:=ACApp,
                                                           vClass:=ACClass, vMethod:="GetClientDirectory", vErrNo:=Information.Err().Number,
                                                          vErrDesc:=Information.Err().Description, excep:=Nothing)
                    Else
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get a temp directory.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClient", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    End If

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result
                End If

                sDir = Path.Combine(sDir, sTemp)
            End If

            If Not Directory.Exists(sDir) Then
                ' directory doesn't exist so attempt to create it
                m_lReturn = CType(CreateFolderTree(sFolderName:=sDir), gPMConstants.PMEReturnCode)

                ' did we succeed...?
                If Not Directory.Exists(sDir) Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClientDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientDirectory", vErrNo:=0, vErrDesc:="Unable to create the directory (" & sDir &
                                       ")")

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result
                End If
            End If

            r_sClientDir = sDir
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            If m_bIsCalledFromBatchProcess = True Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                   sMsg:="GetClientDirectory Failed", vApp:=ACApp,
                                                   vClass:=ACClass, vMethod:="GetClientDirectory", vErrNo:=Information.Err().Number,
                                                  vErrDesc:=Information.Err().Description, excep:=Nothing)
            Else
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClientDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientDirectory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If


            Return result

        End Try
    End Function

    Public Function GetUniqueName(ByRef r_sName As String) As Integer
        Dim nResult As Integer = 0
        Try


            Dim sTemp As String = ""
            Dim sTempFileNameWithPath As String = ""
            nResult = PMEReturnCode.PMTrue
             Dim oFSO As Object
            oFSO = New Scripting.FileSystemObject
            ' Get a Temporary Name
            sTemp = oFSO.GetTempName()
            ' Just get the Base Name, with no extension (i.e. without .tmp)
            sTemp = oFSO.GetBaseName(sTemp)
            oFSO = Nothing
            sTempFileNameWithPath = sTemp
            ' Just get the Base Name, with no extension (i.e. without .tmp)
            sTemp = Path.GetFileNameWithoutExtension(sTemp)

            If sTemp.Length < 1 Then
                Return PMEReturnCode.PMFalse
            End If

            r_sName = sTemp
            'delete the temp File
            File.Delete(sTempFileNameWithPath)
            Return nResult

        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            If m_bIsCalledFromBatchProcess = True Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                   sMsg:="GetUniqueName Failed", vApp:=ACApp,
                                                   vClass:=ACClass, vMethod:="GetUniqueName", vErrNo:=Information.Err().Number,
                                                  vErrDesc:=Information.Err().Description, excep:=Nothing)
            Else
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUniqueName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUniqueName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If
            Return nResult
        End Try
    End Function

    Public Function GetExportDirectory(ByRef r_sExportDirectory As String, Optional ByVal bUnique As Boolean = False) As Integer
        Dim result As Integer = 0

        Try
            Dim sTemp = "", sDir As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue
            'Changes done as per VB code



            'Get the DocZipTemp dir
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="PrntFileDir", r_sSettingValue:=sDir), gPMConstants.PMEReturnCode)

            'Make sure we have an install path
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sDir = "" Then
                ' Log Error Message
                If m_bIsCalledFromBatchProcess = True Then

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                       sMsg:="GetExportDirectory Failed", vApp:=ACApp,
                                                       vClass:=ACClass, vMethod:="GetExportDirectory", vErrNo:=Information.Err().Number,
                                                      vErrDesc:=Information.Err().Description, excep:=Nothing)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetExportDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExportDirectory", vErrNo:=Information.Err().Number, vErrDesc:="Unable to find the registry entry for the PrintFile directory location")
                End If
                result = gPMConstants.PMEReturnCode.PMError

                Return result
            End If

            sDir = Path.Combine(sDir, ACDocExportDir & m_sUsername.Trim())
            If bUnique Then
                m_lReturn = CType(GetUniqueName(r_sName:=sTemp), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If m_bIsCalledFromBatchProcess = True Then

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                           sMsg:="Unable to get a temp directory.", vApp:=ACApp,
                                                           vClass:=ACClass, vMethod:="GetExportDirectory", vErrNo:=Information.Err().Number,
                                                          vErrDesc:=Information.Err().Description, excep:=Nothing)
                    Else
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get a temp directory.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExportDirectory", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    End If

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result
                End If

                sDir = Path.Combine(sDir, sTemp)
            End If

            If Not Directory.Exists(sDir) Then
                ' directory doesn't exist so attempt to create it
                m_lReturn = CType(CreateFolderTree(sFolderName:=sDir), gPMConstants.PMEReturnCode)

                ' did we succeed...?
                If Not Directory.Exists(sDir) Then
                    If m_bIsCalledFromBatchProcess = True Then

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                           sMsg:="GetExportDirectory Failed", vApp:=ACApp,
                                                           vClass:=ACClass, vMethod:="GetExportDirectory", vErrNo:=Information.Err().Number,
                                                          vErrDesc:=Information.Err().Description, excep:=Nothing)
                    Else
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetExportDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExportDirectory", vErrNo:=0, vErrDesc:="Unable to create the directory (" & sDir &
                                       ")")
                    End If

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result
                End If
            End If

            ' return the directory location
            r_sExportDirectory = sDir

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            If m_bIsCalledFromBatchProcess = True Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                   sMsg:="GetExportDirectory Failed", vApp:=ACApp,
                                                   vClass:=ACClass, vMethod:="GetExportDirectory", vErrNo:=Information.Err().Number,
                                                  vErrDesc:=Information.Err().Description, excep:=Nothing)
            Else
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetExportDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExportDirectory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If

            'UPGRADE_TODO: (1065) Error handling statement (Resume) could not be converted. More Information: http://www.vbtonet.com/ewis/ewi1065.aspx

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            Return result
        End Try
    End Function


    Public Function GetZipDirectory(ByRef r_sZipDirectory As String) As Integer
        Dim result As Integer = 0

        Try
            Dim sTemp = "", sZipDir As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue
            'Changes done as per VB code



            'Get the DocZipTemp dir
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:=DOCZipPMDirKey, r_sSettingValue:=sTemp), gPMConstants.PMEReturnCode)

            'Make sure we have an install path
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sTemp = "" Then
                ' Log Error Message
                If m_bIsCalledFromBatchProcess = True Then

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                       sMsg:="GetZipDirectory Failed", vApp:=ACApp,
                                                       vClass:=ACClass, vMethod:="GetZipDirectory", vErrNo:=Information.Err().Number,
                                                      vErrDesc:=Information.Err().Description, excep:=Nothing)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetZipDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetZipDirectory", vErrNo:=Information.Err().Number, vErrDesc:="Unable to find the registry entry for the doczip directory location")
                End If

                result = gPMConstants.PMEReturnCode.PMError

                Return result
            End If

            sZipDir = Path.Combine(sTemp, m_sUsername.Trim())

            m_lReturn = CType(CreateFolderTree(sFolderName:=sZipDir, ClearDirectoryIfExists:=True), gPMConstants.PMEReturnCode)

            ' did we succeed...?
            If Not Directory.Exists(sZipDir) Then
                If m_bIsCalledFromBatchProcess = True Then

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                       sMsg:="GetZipDirectory Failed", vApp:=ACApp,
                                                       vClass:=ACClass, vMethod:="GetZipDirectory", vErrNo:=Information.Err().Number,
                                                      vErrDesc:=Information.Err().Description, excep:=Nothing)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetZipDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetZipDirectory", vErrNo:=0, vErrDesc:="Unable to create the directory (" & sZipDir &
                                   ")")
                End If


                result = gPMConstants.PMEReturnCode.PMError

                Return result
            End If


            ' return the directory location
            r_sZipDirectory = sZipDir

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            If m_bIsCalledFromBatchProcess = True Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                   sMsg:="GetZipDirectory Failed", vApp:=ACApp,
                                                   vClass:=ACClass, vMethod:="GetZipDirectory", vErrNo:=Information.Err().Number,
                                                  vErrDesc:=Information.Err().Description, excep:=Nothing)
            Else
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetZipDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetZipDirectory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If

            Return result
            'UPGRADE_TODO: (1065) Error handling statement (Resume) could not be converted. More Information: http://www.vbtonet.com/ewis/ewi1065.aspx

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

        End Try
    End Function


    Public Function UnZip(ByVal sZipFile As String, ByVal sOutputDirectory As String, Optional ByVal v_bDeleteZipFile As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim oZipper As Object = Nothing
        Dim oFolder As DirectoryInfo = Nothing
        Dim oFile As FileInfo = Nothing
        Try
            Dim sFileIn = "", sFileOut = "", sTemp As String = ""
            Dim bSuccess As Boolean
            Dim sDependencyDir As String = ""
            Dim bIsHTML As Boolean

            Dim sZipFileName, sZipFileFolderName As String
            Dim m_bSameDirectory As Boolean

            result = gPMConstants.PMEReturnCode.PMTrue

            'Changes done as per VB code



            ' does the file exist
            If Not File.Exists(sZipFile) Then
                result = gPMConstants.PMEReturnCode.PMError
                ' Log Error.
                If m_bIsCalledFromBatchProcess = True Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                       sMsg:="File (" & sZipFile & ") does not exist", vApp:=ACApp,
                                                       vClass:=ACClass, vMethod:="UnZip", vErrNo:=Information.Err().Number,
                                                      vErrDesc:=Information.Err().Description, excep:=Nothing)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="File (" & sZipFile & ") does not exist", vApp:=ACApp, vClass:=ACClass, vMethod:="UnZip", vErrNo:=Information.Err().Number, vErrDesc:="Unzip failed")
                End If


                Return result
            End If

            ' does the output folder exist
            If Not Directory.Exists(sOutputDirectory) Then
                result = gPMConstants.PMEReturnCode.PMError
                ' Log Error.
                If m_bIsCalledFromBatchProcess = True Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                           sMsg:="Directory (" & sOutputDirectory & ") does not exist", vApp:=ACApp,
                                                                           vClass:=ACClass, vMethod:="UnZip", vErrNo:=Information.Err().Number,
                                                                          vErrDesc:=Information.Err().Description, excep:=Nothing)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Directory (" & sOutputDirectory & ") does not exist", vApp:=ACApp, vClass:=ACClass, vMethod:="UnZip", vErrNo:=Information.Err().Number, vErrDesc:="Unzip failed")
                End If


                Return result
            End If

            ' Check if the Zip File is in the same output directory
            ' Check if the Zip File is in the same output directory
            sZipFileName = New FileInfo(sZipFile).Name.ToLower()
            sZipFileFolderName = Path.GetDirectoryName(sZipFile).ToLower()

            If sOutputDirectory.ToLower() <> sZipFileFolderName Then
                ' since the zip file is not in the output directory, so we need to
                '  make sure the Output directory is empty,
                m_lReturn = CType(ClearDirectory(sDirectory:=sOutputDirectory), gPMConstants.PMEReturnCode)
                ' Error message to be placed here
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Build the path
                sFileIn = Path.Combine(sOutputDirectory, sZipFileName)

                m_lReturn = CType(CopyFile(sZipFile, sFileIn, True, False, sTemp), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    If m_bIsCalledFromBatchProcess = True Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                               sMsg:="Failed to copy file." & Strings.Chr(13) & Strings.Chr(10) &
                                       "Source File      : " & sZipFile & Strings.Chr(13) & Strings.Chr(10) &
                                       "Destination File : " & sFileIn & Strings.Chr(13) & Strings.Chr(10) &
                                       "Error Details    : ", vApp:=ACApp, vClass:=ACClass, vMethod:="UnZip", vErrNo:=Information.Err().Number,
                                                                              vErrDesc:=Information.Err().Description, excep:=Nothing)
                    Else
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy file." & Strings.Chr(13) & Strings.Chr(10) &
                                       "Source File      : " & sZipFile & Strings.Chr(13) & Strings.Chr(10) &
                                       "Destination File : " & sFileIn & Strings.Chr(13) & Strings.Chr(10) &
                                       "Error Details    : " & sTemp, vApp:=ACApp, vClass:=ACClass, vMethod:="UnZip", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    End If

                    Return result
                End If

            Else
                ' The zip file is in the same directory (output directory), so we need to
                ' clear the directory other than this zip file
                m_bSameDirectory = True
                sFileIn = sZipFile

                oFolder = New DirectoryInfo(sZipFileFolderName)

                ' Clear the sub folders first
                For Each oSubFolder As DirectoryInfo In oFolder.GetDirectories
                    oSubFolder.Delete(True)
                Next oSubFolder

                ' Clear the files then
                For Each oFile2 As FileInfo In oFolder.GetFiles
                    oFile = oFile2
                    If oFile.Name.ToLower() <> sZipFileName Then
                        'oFile.Delete(True)
                        oFile.Delete()
                    End If
                Next oFile2

            End If

            oZipper = CreateLateBoundObject("bPMZipper.Business")
            If oZipper Is Nothing Then
                result = gPMConstants.PMEReturnCode.PMError
                ' Log Error.
                If m_bIsCalledFromBatchProcess = True Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                           sMsg:="Failed to load zipper object", vApp:=ACApp,
                                                                           vClass:=ACClass, vMethod:="UnZip", vErrNo:=Information.Err().Number,
                                                                          vErrDesc:=Information.Err().Description, excep:=Nothing)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load zipper object", vApp:=ACApp, vClass:=ACClass, vMethod:="UnZip", vErrNo:=Information.Err().Number, vErrDesc:="Unzip failed")
                End If


                Return result
            End If

            bSuccess = oZipper.UnZipFile(sFileIn, sOutputDirectory)

            oZipper = Nothing
            If Not bSuccess Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not m_bSameDirectory Then
                ' remove the zip file in the target directory, since this is
                ' a copy of the original file
                'UPGRADE_WARNING: (2081) DeleteFile has a new behavior. More Information: http://www.vbtonet.com/ewis/ewi2081.aspx
                File.Delete(sFileIn)
            End If

            ' what type of files have we extracted
            m_lReturn = CType(GetFileNameAndType(v_sFolder:=sOutputDirectory, r_sFileName:=sFileOut, r_bHTMLFormat:=bIsHTML), gPMConstants.PMEReturnCode)

            If bIsHTML Then
                'Move extra files into dependency folder
                sDependencyDir = sFileOut.Substring(0, sFileOut.Length - 4) & "_files"
                sDependencyDir = Path.Combine(sOutputDirectory, sDependencyDir)
                If Not Directory.Exists(sDependencyDir) Then
                    m_lReturn = CType(CreateFolderTree(sDependencyDir), gPMConstants.PMEReturnCode)

                    ' did we create the folder
                    If Not Directory.Exists(sDependencyDir) Then
                        result = gPMConstants.PMEReturnCode.PMError
                        ' Log Error.
                        If m_bIsCalledFromBatchProcess = True Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                                   sMsg:="Failed to create directory (" & sDependencyDir & ")", vApp:=ACApp,
                                                                                   vClass:=ACClass, vMethod:="UnZip", vErrNo:=Information.Err().Number,
                                                                                  vErrDesc:=Information.Err().Description, excep:=Nothing)
                        Else
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create directory (" & sDependencyDir & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="UnZip", vErrNo:=Information.Err().Number, vErrDesc:="Unzip failed")
                        End If
                        Return result
                    End If
                End If

                ' move the files to the sub directory
                oFolder = New DirectoryInfo(sOutputDirectory)
                For Each oFile In oFolder.GetFiles
                    If oFile.Name.ToLower() <> sZipFileName Then
                        ' Don't move the zip file
                        If oFile.Name <> sFileOut Then
                            Debug.WriteLine("moving file " & oFile.Name)
                            sFileIn = Path.Combine(sDependencyDir, oFile.Name)
                            oFile.MoveTo(sFileIn)
                        End If
                    End If
                Next oFile
            End If

            ' RAM20040301 : Delete the zip file
            If v_bDeleteZipFile Then
                If File.Exists(sZipFile) Then
                    'UPGRADE_WARNING: (2081) DeleteFile has a new behavior. More Information: http://www.vbtonet.com/ewis/ewi2081.aspx
                    File.Delete(sZipFile)
                End If
            End If

            oFile = Nothing
            oFolder = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Clean up
            If oZipper Is Nothing Then
            Else
                oZipper = Nothing
            End If
            If oFile Is Nothing Then
            Else
                oFile = Nothing
            End If
            If oFolder Is Nothing Then
            Else
                oFolder = Nothing
            End If


            ' Log Error.
            If m_bIsCalledFromBatchProcess = True Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                       sMsg:="Failed to unzip the document. " & Strings.Chr(13) & Strings.Chr(10) &
                               "Zip File [" & sZipFile & "]" & Strings.Chr(13) & Strings.Chr(10) &
                               "sOutputDirectory [" & sOutputDirectory & "]", vApp:=ACApp,
                                                                       vClass:=ACClass, vMethod:="UnZip", vErrNo:=Information.Err().Number,
                                                                      vErrDesc:=Information.Err().Description, excep:=Nothing)
            Else
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unzip the document. " & Strings.Chr(13) & Strings.Chr(10) &
                               "Zip File [" & sZipFile & "]" & Strings.Chr(13) & Strings.Chr(10) &
                               "sOutputDirectory [" & sOutputDirectory & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="UnZip", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If


            Return result

        End Try
    End Function


    Public Function GetFileNameAndType(ByVal v_sFolder As String, ByRef r_sFileName As String, ByRef r_bHTMLFormat As Boolean) As Integer

        Dim result As Integer = 0

        Dim oFolder As DirectoryInfo
        Try
            Dim sExt = "", sPath As String = ""


            result = gPMConstants.PMEReturnCode.PMTrue
            r_sFileName = ""
            r_bHTMLFormat = False

            'Changes done as per VB code



            ' does this folder actually exist
            If Not Directory.Exists(v_sFolder) Then
                result = gPMConstants.PMEReturnCode.PMNotFound

                Return result
            End If

            oFolder = New DirectoryInfo(v_sFolder)
            Dim cFolder As DirectoryInfo
            cFolder = oFolder
            Do While (oFolder.GetFiles.Length < 1)
                For Each vDir As DirectoryInfo In oFolder.GetDirectories
                    oFolder = vDir
                Next
            Loop

            For Each vFile As FileInfo In oFolder.GetFiles
                sExt = Path.GetExtension(sPath & "\" & vFile.Name).Substring(1)
                Select Case (sExt.ToUpper())
                    Case "HTM"
                        r_sFileName = vFile.Name
                        r_bHTMLFormat = True

                        oFolder = Nothing
                        Return result
                    Case "DOC"
                        r_sFileName = vFile.Name
                        r_bHTMLFormat = False

                        oFolder = Nothing
                        Return result
                End Select
            Next vFile


            oFolder = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            If m_bIsCalledFromBatchProcess = True Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                       sMsg:="GetFileNameAndType Failed. File Name : ", vApp:=ACApp,
                                                                       vClass:=ACClass, vMethod:="GetFileNameAndType", vErrNo:=Information.Err().Number,
                                                                      vErrDesc:=Information.Err().Description, excep:=Nothing)
            Else
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFileNameAndType Failed. File Name : " & v_sFolder, vApp:=ACApp, vClass:=ACClass, vMethod:="GetFileNameAndType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If


            Return result
            'UPGRADE_TODO: (1065) Error handling statement (Resume) could not be converted. More Information: http://www.vbtonet.com/ewis/ewi1065.aspx

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
        End Try
    End Function


    Public Function DelDirectory(ByRef r_sDocDirectory As String) As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Changes done as per VB code



            ' does this folder actually exist
            If Directory.Exists(r_sDocDirectory) Then

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' To avoid the "permission denied" error when trying to delete folders
                ' & files (fso.deletefolder), use the (not well documented) "force"
                ' argument.
                ' The force flag is also usefull where the folder contains subfolders
                ' and/or files as this will cause the contents to be deleted recursively
                ' before the folder it 's self is removed.
                ' RAM20040205 : Bug fix for PN Issue 10231 - START
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                ' When Dir is used with an argument and finds a file it leaves a search
                ' handle open which causes the containing directory to be "locked" until
                ' the handle is closed.  The handle will be closed when Dir is used with
                ' a different argument (as above) or when Dir, used with no argument,
                ' returns an empty string.

                ' The following Dir will release any handle to the directory by the OS
                ' so that the delete folder can happen with no 'Permission Denied' Error

                ' NOTE : WE DON'T NEED TO USE THE FOLLOWING Dir("") Command, if we are
                '        are not using it any where in our software. As we are not, so
                '       the following command is for JIC
                FileSystem.Dir("", FileAttribute.Normal)

                Directory.Delete(r_sDocDirectory, True)
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' RAM20040205 : Bug fix for PN Issue 10231 - END
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            If m_bIsCalledFromBatchProcess = True Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                       sMsg:="DelDirectory Failed. Directory Name : " & r_sDocDirectory, vApp:=ACApp,
                                                                       vClass:=ACClass, vMethod:="DelDirectory", vErrNo:=Information.Err().Number,
                                                                      vErrDesc:=Information.Err().Description, excep:=Nothing)
            Else
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DelDirectory Failed. Directory Name : " & r_sDocDirectory, vApp:=ACApp, vClass:=ACClass, vMethod:="DelDirectory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If


            Return result

        End Try
    End Function


    ' Funtion to check if a Folder exists using FileSystemObject
    Public Function IsFolderExists(ByVal v_sFolderName As String) As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMFalse

            If Directory.Exists(v_sFolderName) Then
                result = gPMConstants.PMEReturnCode.PMTrue
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            If m_bIsCalledFromBatchProcess = True Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                       sMsg:="Failed to Check FolderExists for '" & v_sFolderName & "'", vApp:=ACApp,
                                                                       vClass:=ACClass, vMethod:="IsFolderExists", vErrNo:=Information.Err().Number,
                                                                      vErrDesc:=Information.Err().Description, excep:=Nothing)
            Else
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Check FolderExists for '" & v_sFolderName & "'", vApp:=ACApp, vClass:=ACClass, vMethod:="IsFolderExists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If


            Return result

        End Try
    End Function


    ' Funtion to check if file exists using FileSystemObject
    Public Function IsFileExists(ByVal v_sFileName As String) As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMFalse

            If File.Exists(v_sFileName) Then
                result = gPMConstants.PMEReturnCode.PMTrue
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            If m_bIsCalledFromBatchProcess = True Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                       sMsg:="Failed to Check FileExists for '" & v_sFileName & "'", vApp:=ACApp,
                                                                       vClass:=ACClass, vMethod:="IsFileExists", vErrNo:=Information.Err().Number,
                                                                      vErrDesc:=Information.Err().Description, excep:=Nothing)
            Else
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Check FileExists for '" & v_sFileName & "'", vApp:=ACApp, vClass:=ACClass, vMethod:="IsFileExists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If


            Return result

        End Try
    End Function


    ' Funtion to Create a Folder using FileSystemObject
    Public Function CreateFolder(ByVal v_sFolderName As String) As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMFalse

            ' An error will occurs if the specified folder already exists
            ' so check the existance of the folder

            If Directory.Exists(v_sFolderName) Then
                ' do nothing, since the folder is there
                result = gPMConstants.PMEReturnCode.PMTrue
            Else
                'Modified by Sudhanshu Behera on 5/6/2010 12:15:11 PM refer developer guide no. 172 (guide)
                'If CBool(Directory.CreateDirectory(v_sFolderName).Path) Then
                If Directory.CreateDirectory(v_sFolderName).Exists Then
                    ' did we succeed...?
                    If Directory.Exists(v_sFolderName) Then
                        result = gPMConstants.PMEReturnCode.PMTrue
                    Else
                        result = gPMConstants.PMEReturnCode.PMError
                        If m_bIsCalledFromBatchProcess = True Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                                   sMsg:="CreateFolder Failed", vApp:=ACApp,
                                                                                   vClass:=ACClass, vMethod:="CreateFolder", vErrNo:=Information.Err().Number,
                                                                                  vErrDesc:=Information.Err().Description, excep:=Nothing)
                        Else
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateFolder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateFolder", vErrNo:=0, vErrDesc:="Unable to create the directory (" & v_sFolderName & ")")
                        End If

                    End If
                End If
            End If



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            If m_bIsCalledFromBatchProcess = True Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                       sMsg:="CreateFolder Unable to create the directory (" & v_sFolderName & ")", vApp:=ACApp,
                                                                       vClass:=ACClass, vMethod:="CreateFolder", vErrNo:=Information.Err().Number,
                                                                      vErrDesc:=Information.Err().Description, excep:=Nothing)
            Else
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CreateFolder Unable to create the directory (" & v_sFolderName & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateFolder", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If
            Return result

        End Try
    End Function


    ' Funtion to delete the file if file exists using FileSystemObject
    Public Function DeleteFile(ByVal v_sFileName As String) As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Delete file
            If File.Exists(v_sFileName) Then
                ' Setting the ForceDelete Parameter to True, so that it will force
                ' the file to delete (even it is readonly)

                File.Delete(v_sFileName)
            End If

            ' Reconfirm again
            If File.Exists(v_sFileName) Then
                ' Delete file fails
                ' Log Error Message
                If m_bIsCalledFromBatchProcess = True Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                           sMsg:="Unable to Delete file '" & v_sFileName & "'", vApp:=ACApp,
                                                                           vClass:=ACClass, vMethod:="DeleteFile", vErrNo:=Information.Err().Number,
                                                                          vErrDesc:=Information.Err().Description, excep:=Nothing)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Delete file '" & v_sFileName & "'", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteFile", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If

            Else
                result = gPMConstants.PMEReturnCode.PMTrue
            End If



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            If m_bIsCalledFromBatchProcess = True Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                       sMsg:="Failed to Delete file '" & v_sFileName & "'", vApp:=ACApp,
                                                                       vClass:=ACClass, vMethod:="DeleteFile", vErrNo:=Information.Err().Number,
                                                                      vErrDesc:=Information.Err().Description, excep:=Nothing)
            Else
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Delete file '" & v_sFileName & "'", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If


            Return result

        End Try
    End Function


    Public Function CopyFile(ByVal v_sSourceFile As String, ByVal v_sDestinationFile As String, Optional ByVal v_bOverWriteFiles As Boolean = True, Optional ByVal v_bDeleteSoureFile As Boolean = False, Optional ByRef r_vErrorMessage As String = "", Optional ByVal v_bCalledFromBatchProcessing As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim sDestinationFolderName As String = ""
        Dim oFolder As DirectoryInfo = Nothing
        Dim sErrorMessage As String = ""
        Dim bCanCopy, bFileCopied As Boolean
        Dim sFinalFileName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            v_sSourceFile = IIf(IsNothing(v_sSourceFile), "", v_sSourceFile)
            v_sDestinationFile = v_sDestinationFile.Trim()
            sErrorMessage = ""

            ' Deal with null input
            If v_sSourceFile.Length = 0 Then
                If m_bIsCalledFromBatchProcess Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="Source file name is null", vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="CopyFile", vErrNo:=Information.Err().Number,
                                  vErrDesc:=Information.Err().Description, excep:=Nothing)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Source file name is null", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFile", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If

                Return gPMConstants.PMEReturnCode.PMError
            End If

            If v_sDestinationFile.Length = 0 Then
                If m_bIsCalledFromBatchProcess Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="Destination file name is null", vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="CopyFile", vErrNo:=Information.Err().Number,
                                  vErrDesc:=Information.Err().Description, excep:=Nothing)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Destination file name is null", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFile", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If

                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Create FSO


            'Check the Souce file exists
            If File.Exists(v_sSourceFile) Then

                ' If the destination file name contains a File Extension, then
                ' then v_sDestinationFile is a File Name
                ' else v_sDestinationFile is a Folder Name
                If Path.GetExtension(v_sDestinationFile).Substring(1).Length > 0 Then
                    ' Get the destination folder, since we have a file name
                    sDestinationFolderName = Path.GetDirectoryName(v_sDestinationFile)
                    sFinalFileName = v_sDestinationFile
                Else
                    sDestinationFolderName = New FileInfo(v_sDestinationFile).DirectoryName
                    sFinalFileName = sDestinationFolderName & "\" & New FileInfo(v_sSourceFile).Name
                End If

                ' Check the destination folder exists
                If Not Directory.Exists(sDestinationFolderName) Then
                    ' Create the folder, since it not exists.
                    m_lReturn = CType(CreateFolderTree(sDestinationFolderName), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        ' Clear Memory
                        If oFolder Is Nothing Then
                        Else
                            oFolder = Nothing
                        End If
                        result = gPMConstants.PMEReturnCode.PMError
                        If m_bIsCalledFromBatchProcess Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                           sMsg:="Unable to create the Destination Folder (" & sDestinationFolderName & ")", vApp:=ACApp,
                                           vClass:=ACClass, vMethod:="CopyFile", vErrNo:=Information.Err().Number,
                                          vErrDesc:=Information.Err().Description, excep:=Nothing)
                        Else
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to create the Destination Folder (" & sDestinationFolderName & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFile", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        End If

                        Return result
                    End If
                End If

                ' Now we got the source file and the destination folder
                ' so, we can issue the copy command
                ' Make sure the destination folder is not ReadONLY, if readonly, then we need to
                ' returnt the message, saying that the it is the case

                'Name       Value    Description                        Read/Write attribute
                '
                'Normal     0        Normal File                        Read/write
                'ReadOnly   1       Read-only file                      Read only
                'Hidden     2       Hidden file                         Read/write
                'System     4       System file                         Read/write
                'Volume     8       Disk drive volume label             Read only
                'Directory  16      Folder or directory                 Read-only
                'Archive    32      File has changed since last backup  Read/write
                'Alias      64      Link or shortcut                    Read-only
                'Compressed 2048    Compressed file                     Read-only

                oFolder = New DirectoryInfo(sDestinationFolderName)

                sErrorMessage = "The folder [" & sDestinationFolderName & "] is having Folling Attributes" & Strings.Chr(13) & Strings.Chr(10)

                bCanCopy = True

                If CInt(oFolder.Attributes) = 1 Then
                    sErrorMessage = sErrorMessage & "Read-Only" & Strings.Chr(13) & Strings.Chr(10)
                    bCanCopy = False
                End If

                If CInt(oFolder.Attributes) = 4 Then
                    sErrorMessage = sErrorMessage & "System File" & Strings.Chr(13) & Strings.Chr(10)
                    bCanCopy = False
                End If

                If CInt(oFolder.Attributes) = 64 Then
                    sErrorMessage = sErrorMessage & "Link or shortcut" & Strings.Chr(13) & Strings.Chr(10)
                    bCanCopy = False
                End If

                If bCanCopy Then

                    ' Issue the copy command
                    File.Copy(v_sSourceFile, sFinalFileName, v_bOverWriteFiles)
                    File.SetAttributes(sFinalFileName, FileAttributes.Normal)
                    ' Do we need to wait here, so that the file exists.
                    'Loop until file exists
                    Do
                        bFileCopied = IsFileExists(sFinalFileName)
                    Loop While Not bFileCopied

                    'Now the file is copied, check if the v_bDeleteSoureFile flag is set,
                    If v_bDeleteSoureFile Then
                        ' We need to delete the source file
                        File.Delete(v_sSourceFile)
                    End If

                Else

                    ' Clear Memory
                    If oFolder Is Nothing Then
                    Else
                        oFolder = Nothing
                    End If

                    ' Return Error Message
                    result = gPMConstants.PMEReturnCode.PMError


                    If Information.IsNothing(r_vErrorMessage) Then
                    Else
                        r_vErrorMessage = sErrorMessage
                    End If

                    If m_bIsCalledFromBatchProcess Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="Unable to Copy the File (" & v_sSourceFile & ") because" & Strings.Chr(13) & Strings.Chr(10) & sErrorMessage, vApp:=ACApp,
                                       vClass:=ACClass, vMethod:="CopyFile", vErrNo:=Information.Err().Number,
                                      vErrDesc:=Information.Err().Description, excep:=Nothing)
                    Else
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Copy the File (" & v_sSourceFile & ") because" & Strings.Chr(13) & Strings.Chr(10) &
                                       sErrorMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFile", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    End If

                    Return result
                End If

            Else
                ' Source file Missing
                ' Log Error Message
                If m_bIsCalledFromBatchProcess Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="Source File Missing (" & v_sSourceFile & ")", vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="CopyFile", vErrNo:=Information.Err().Number,
                                  vErrDesc:=Information.Err().Description, excep:=Nothing)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Source File Missing (" & v_sSourceFile & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFile", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If

                Return gPMConstants.PMEReturnCode.PMError

            End If

            ' Clear Memory
            If oFolder Is Nothing Then
            Else
                oFolder = Nothing
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Clear Memory
            If oFolder Is Nothing Then
            Else
                oFolder = Nothing
            End If


            ' Log Error Message
            If m_bIsCalledFromBatchProcess Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="Failed to copy " & v_sSourceFile & " to " & v_sDestinationFile, vApp:=ACApp,
                               vClass:=ACClass, vMethod:="CopyFile", vErrNo:=Information.Err().Number,
                              vErrDesc:=Information.Err().Description, excep:=excep)
            Else
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy " & v_sSourceFile & " to " & v_sDestinationFile, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If
            Return result

        End Try
    End Function


    Public Function CopyFolder(ByVal v_sSourceFolder As String, ByVal v_sDestinationFolder As String, Optional ByVal v_bOverWriteFiles As Boolean = True, Optional ByRef r_vErrorMessage As String = "") As Integer

        Dim result As Integer = 0
        Try


            Dim sErrorMessage As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            v_sSourceFolder = v_sSourceFolder.Trim()
            v_sDestinationFolder = v_sDestinationFolder.Trim()

            ' Deal with null input
            If v_sSourceFolder.Length = 0 Then

                sErrorMessage = sErrorMessage & "Source Folder name is null." & Strings.Chr(13) & Strings.Chr(10)
                ' Log Error Message
                If m_bIsCalledFromBatchProcess = True Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                           sMsg:="Source Folder name is null", vApp:=ACApp,
                                                                           vClass:=ACClass, vMethod:="CopyFolder", vErrNo:=Information.Err().Number,
                                                                          vErrDesc:=Information.Err().Description, excep:=Nothing)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Source Folder name is null", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFolder", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If

                Return gPMConstants.PMEReturnCode.PMError
            End If

            If v_sDestinationFolder.Length = 0 Then
                sErrorMessage = sErrorMessage & "Destination Folder name is null." & Strings.Chr(13) & Strings.Chr(10)
                ' Log Error Message
                If m_bIsCalledFromBatchProcess = True Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                           sMsg:="Destination Folder name is null", vApp:=ACApp,
                                                                           vClass:=ACClass, vMethod:="CopyFolder", vErrNo:=Information.Err().Number,
                                                                          vErrDesc:=Information.Err().Description, excep:=Nothing)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Destination Folder name is null", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFolder", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If

                Return gPMConstants.PMEReturnCode.PMError
            End If

            If v_sSourceFolder.EndsWith("\") Then
                v_sSourceFolder = v_sSourceFolder.Substring(0, v_sSourceFolder.Length - 1)
            End If

            If v_sDestinationFolder.EndsWith("\") Then
                v_sDestinationFolder = v_sDestinationFolder.Substring(0, v_sDestinationFolder.Length - 1)
            End If




            ' Check if the Source folder exists
            If Directory.Exists(v_sSourceFolder) Then
            Else
                result = gPMConstants.PMEReturnCode.PMError

                sErrorMessage = sErrorMessage & "Missing Source Folder [" & v_sSourceFolder & "]" & Strings.Chr(13) & Strings.Chr(10)

                ' Log Error Message
                If m_bIsCalledFromBatchProcess = True Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                           sMsg:="Missing Source Folder [" & v_sSourceFolder & "]", vApp:=ACApp,
                                                                           vClass:=ACClass, vMethod:="CopyFolder", vErrNo:=Information.Err().Number,
                                                                          vErrDesc:=Information.Err().Description, excep:=Nothing)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Missing Source Folder [" & v_sSourceFolder & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFolder", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If

                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Issue the copy folder command
            FileIO.FileSystem.CopyDirectory(v_sSourceFolder, v_sDestinationFolder, v_bOverWriteFiles)

            ' Do we need to check ????
            ' We are not doing it at the moment, if needed may me in the future


            ' Clean up memory

            ' Return the message, if asked for

            If Information.IsNothing(r_vErrorMessage) Then
            Else
                r_vErrorMessage = sErrorMessage
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            If m_bIsCalledFromBatchProcess = True Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                       sMsg:="Failed to Copy Directory" & Strings.Chr(13) & Strings.Chr(10) &
                               "From : [" & v_sSourceFolder & "]" & Strings.Chr(13) & Strings.Chr(10) &
                               "To   : [" & v_sDestinationFolder & "]", vApp:=ACApp,
                                                                       vClass:=ACClass, vMethod:="CopyFolder", vErrNo:=Information.Err().Number,
                                                                      vErrDesc:=Information.Err().Description, excep:=Nothing)
            Else
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Copy Directory" & Strings.Chr(13) & Strings.Chr(10) &
                               "From : [" & v_sSourceFolder & "]" & Strings.Chr(13) & Strings.Chr(10) &
                               "To   : [" & v_sDestinationFolder & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFolder", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If


            Return result

        End Try
    End Function


    Public Function NoOfFilesInDirectory(ByVal v_sDirectoryName As String, Optional ByRef r_vFirstFileName As String = "", Optional ByVal v_sFileExtension As String = "") As Integer

        Dim result As Integer = 0
        Dim oFolder As DirectoryInfo = Nothing
        Dim oFilesCollection As FileInfo() = Nothing
        Dim oFile As FileInfo = Nothing
        Try

            Dim lCount As Integer

            Dim sFirstFileName As String = ""

            ' Set the default to Zero
            lCount = 0



            ' Check if the folder exists
            If Directory.Exists(v_sDirectoryName) Then
                oFolder = New DirectoryInfo(v_sDirectoryName)
                oFilesCollection = oFolder.GetFiles

                'UPGRADE_NOTE: (1021) IsMissing() was changed to IsNothing(). More Information: http://www.vbtonet.com/ewis/ewi1021.aspx
                'If Information.IsNothing(v_sFileExtension) Then
                lCount = oFilesCollection.Length ' Return the number of files

            End If

            'UPGRADE_NOTE: (1021) IsMissing() was changed to IsNothing(). More Information: http://www.vbtonet.com/ewis/ewi1021.aspx
            'If Information.IsNothing(v_sFileExtension) Then
            For Each oFile2 As FileInfo In oFilesCollection
                oFile = oFile2
                'Get the first file file name and exit
                sFirstFileName = oFile.Name

                r_vFirstFileName = sFirstFileName
                v_sFileExtension = Path.GetExtension(oFile.Name).Substring(1)
                If lCount = 1 Then
                    sFirstFileName = oFile.Name
                End If
                Exit For
            Next oFile2
            'Else
            'For Each oFile In oFilesCollection
            '    If v_sFileExtension = Path.GetExtension(oFile.Name).Substring(1) Then
            '        lCount += 1
            '        If lCount = 1 Then
            '            sFirstFileName = oFile.Name
            '        End If
            '    End If
            'Next oFile


            'UPGRADE_NOTE: (1021) IsMissing() was changed to IsNothing(). More Information: http://www.vbtonet.com/ewis/ewi1021.aspx
            If Information.IsNothing(r_vFirstFileName) Then
            Else
                r_vFirstFileName = sFirstFileName
            End If

            result = lCount

            oFile = Nothing
            oFilesCollection = Nothing
            oFolder = Nothing


            Return result

        Catch excep As System.Exception



            If oFile Is Nothing Then
            Else
                oFile = Nothing
            End If
            If oFilesCollection Is Nothing Then
            Else
                oFilesCollection = Nothing
            End If
            If oFolder Is Nothing Then
            Else
                oFolder = Nothing
            End If

            ' Log Error Message
            If m_bIsCalledFromBatchProcess = True Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                       sMsg:="Failed to Get No Of Files In Directory (" & v_sDirectoryName & ")", vApp:=ACApp,
                                                                       vClass:=ACClass, vMethod:="NoOfFilesInDirectory", vErrNo:=Information.Err().Number,
                                                                      vErrDesc:=Information.Err().Description, excep:=Nothing)
            Else
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Get No Of Files In Directory (" & v_sDirectoryName & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="NoOfFilesInDirectory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If


            Return result

        End Try
    End Function


    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Function Name : Zip
    ' Description   : Function to Zip the supplied file
    ' Notes         : This function is moved from iPMBDocTemplate.frmInterface
    ' Edit History  :
    ' RAM20040206   : Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Function Zip(ByVal sPath As String, ByVal sInputDocumentFileExtension As String) As Integer

        Dim result As Integer = 0
        Dim iTemp As Integer
        Dim sFileIn = "", sFileOut, sParentFile, sDependencyDir, sDependency As String
        Dim oZipper As Object = Nothing
        Dim oFolder As DirectoryInfo = Nothing
        'Dim oFilesCollection As Scripting.IFileCollection
        Dim oFilesCollection As FileInfo() = Nothing
        Dim oFile As FileInfo = Nothing
        Dim sFileNameWithPath As String = ""

        Try

            sFileIn = sPath.Substring(0, sPath.Length - 3) & sInputDocumentFileExtension
            sFileOut = sPath

            ' Create the zipper component
            oZipper = CreateLateBoundObject("bPMZipper.Business")
            If oZipper Is Nothing Then
                result = gPMConstants.PMEReturnCode.PMError
                ' Log Error.
                If m_bIsCalledFromBatchProcess = True Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                           sMsg:="Failed to load zipper object", vApp:=ACApp,
                                                                           vClass:=ACClass, vMethod:="Zip", vErrNo:=Information.Err().Number,
                                                                          vErrDesc:=Information.Err().Description, excep:=Nothing)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load zipper object", vApp:=ACApp, vClass:=ACClass, vMethod:="Zip", vErrNo:=Information.Err().Number, vErrDesc:="Zip failed")
                End If
                Return result
            End If

            iTemp = oZipper.ZipFile(sFileIn:=sFileIn, sFileOut:=sFileOut)
            If Not iTemp Then
                oZipper = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Enable zipping up of multiple files to include HTML dependencies.
            'Check for dependencies and add them to the zip file if they exist.
            sParentFile = sFileIn.Substring(0, sPath.Length - 4)
            sDependencyDir = sParentFile & "_files"

            ' Create the FSO
            'Changes done as per VB code



            ' Check if dependencies folder exist
            If Directory.Exists(sDependencyDir) Then

                oFolder = New DirectoryInfo(sDependencyDir)
                oFilesCollection = oFolder.GetFiles

                For Each oFile2 As FileInfo In oFilesCollection
                    oFile = oFile2

                    ' Get each file name
                    sDependency = oFile.Name

                    ' Append the directory name to it
                    sFileNameWithPath = sDependencyDir & "\" & sDependency

                    ' Add it to the zip file
                    iTemp = oZipper.addFileToZIP(sFileOut, sFileNameWithPath)

                    ' Remvoe the file
                    m_lReturn = CType(DeleteFile(sFileNameWithPath), gPMConstants.PMEReturnCode)

                Next oFile2

                'Clean up
                oZipper = Nothing
                oFile = Nothing
                oFilesCollection = Nothing
                oFolder = Nothing


                'Remove the dependency directory for all
                m_lReturn = CType(DelDirectory(sDependencyDir), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMError
                    ' Log Error.
                    If m_bIsCalledFromBatchProcess = True Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                               sMsg:="Unable to delete the dependency folder. (" & sDependencyDir & "')", vApp:=ACApp,
                                                                               vClass:=ACClass, vMethod:="Zip", vErrNo:=Information.Err().Number,
                                                                              vErrDesc:=Information.Err().Description, excep:=Nothing)
                    Else
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to delete the dependency folder. (" & sDependencyDir & "')", vApp:=ACApp, vClass:=ACClass, vMethod:="Zip", vErrNo:=Information.Err().Number, vErrDesc:="Zip failed")
                    End If

                End If

            End If

            ' We Zipped the original file with dependency folder(if any), so get rid of source file
            m_lReturn = CType(DeleteFile(sFileIn), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMError
                ' Log Error.
                If m_bIsCalledFromBatchProcess = True Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                           sMsg:="Unable to delete the file. (" & sFileIn & "')", vApp:=ACApp,
                                                                           vClass:=ACClass, vMethod:="Zip", vErrNo:=Information.Err().Number,
                                                                          vErrDesc:=Information.Err().Description, excep:=Nothing)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to delete the file. (" & sFileIn & "')", vApp:=ACApp, vClass:=ACClass, vMethod:="Zip", vErrNo:=Information.Err().Number, vErrDesc:="Zip failed")
                End If

            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Clean up
            If oZipper Is Nothing Then
            Else
                oZipper = Nothing
            End If
            If oFile Is Nothing Then
            Else
                oFile = Nothing
            End If
            If oFilesCollection Is Nothing Then
            Else
                oFilesCollection = Nothing
            End If
            If oFolder Is Nothing Then
            Else
                oFolder = Nothing
            End If

            ' Log Error.
            If m_bIsCalledFromBatchProcess = True Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                       sMsg:="Failed to zip the document. (" & sFileIn & ")", vApp:=ACApp,
                                                                       vClass:=ACClass, vMethod:="Zip", vErrNo:=Information.Err().Number,
                                                                      vErrDesc:=Information.Err().Description, excep:=Nothing)
            Else
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to zip the document. (" & sFileIn & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="Zip", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If
            Return result

        End Try
    End Function


    Public Function ConvertDocumentUsingSiriusDocumentUtility(ByVal v_sSourceDocument As String, ByVal v_sDestDocument As String) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ConvertDocumentUsingSiriusDocumentUtility"
        Dim oConvert As SiriusDocumentUtility.Document


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            oConvert = New SiriusDocumentUtility.Document()

            oConvert.Convert(v_sSourceDocument, v_sDestDocument)


        Catch ex As Exception

            If m_bIsCalledFromBatchProcess = True Then
                result = gPMConstants.PMEReturnCode.PMError
                iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:="", excep:=ex)
            Else
                iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:="")
            End If

        Finally
            oConvert = Nothing

            '        Return result

            ' This is for debugging only
            '        
            '        Return result
        End Try
        Return result
    End Function

    Public Function PrintDocumentUsingSiriusDocumentUtility(ByVal v_sSourceDocument As String, ByVal v_sPrinterName As String) As Integer
        Dim result As Integer = 0

        Const kMethodName As String = "PrintDocumentUsingSiriusDocumentUtility"
        Dim oConvert As SiriusDocumentUtility.Document


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            oConvert = New SiriusDocumentUtility.Document()


            oConvert.PrintDocument(v_sSourceDocument, v_sPrinterName)


        Catch ex As Exception
            If m_bIsCalledFromBatchProcess = True Then
                result = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                       sMsg:="Failed to print document utility", vApp:=ACApp,
                                                                       vClass:=ACClass, vMethod:="PrintDocumentUsingSiriusDocumentUtility", vErrNo:=Information.Err().Number,
                                                                      vErrDesc:=Information.Err().Description, excep:=Nothing)
            Else
                iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:="")
            End If

        Finally
            oConvert = Nothing

            '        Return result

            ' This is for debugging only
            '      
            '        Return result
        End Try
        Return result
    End Function
    Public Function DocumentTitleCheckUsingSiriusDocumentUtility(ByVal v_sDocument As String) As Integer
        Dim result As Integer = 0
        Dim oConvert As Object
        Const kMethodName As String = "DocumentTitleCheckUsingSiriusDocumentUtility"

        Dim lInputFileNum, lFileLength As Integer
        Dim sAllFile As String = ""



        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lInputFileNum = FileSystem.FreeFile()
            FileSystem.FileOpen(lInputFileNum, v_sDocument, OpenMode.Input)
            lFileLength = FileSystem.LOF(lInputFileNum)
            sAllFile = FileSystem.InputString(lInputFileNum, lFileLength)
            FileSystem.FileClose(lInputFileNum)

            If sAllFile.IndexOf("<o:Title>", StringComparison.CurrentCultureIgnoreCase) >= 0 Then

                oConvert = New SiriusDocumentUtility.Document()

                oConvert.ClearDocumentTitlePropertyIfMergeCode(v_sDocument)

            End If


        Catch ex As Exception
            If m_bIsCalledFromBatchProcess = True Then
                result = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                                       sMsg:="Failed to check title name of document", vApp:=ACApp,
                                                                       vClass:=ACClass, vMethod:="DocumentTitleCheckUsingSiriusDocumentUtility", vErrNo:=Information.Err().Number,
                                                                      vErrDesc:=Information.Err().Description, excep:=Nothing)
            Else
                iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:="")
            End If

        Finally
            oConvert = Nothing

            '        Return result

            ' This is for debugging only
            '        
            '        Return result
        End Try
        Return result
    End Function
End Module
