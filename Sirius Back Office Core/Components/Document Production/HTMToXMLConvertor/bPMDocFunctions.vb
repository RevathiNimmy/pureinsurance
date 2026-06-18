Option Strict Off
Option Explicit On
Imports SharedFiles
Module bPMDocFunctions
	'*******************************************************************************
	' Edit History:
	'   PW270905 - PN24093 - Incorporate retries into the DeleteFile function
	'*******************************************************************************
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "bPMDocFunctions"
	
	Private m_lReturn As Integer
	Private Const ACDocExportDir As String = "DocArchiveTemp\"
	Private m_sUsername As String
	
	' SET 18/10/2004 ISS13245
	Public Const ACMaxDelayInterval As Short = 30
	Private m_sWordVersion As String
	
	Public Declare Function FindWindow Lib "user32.dll"  Alias "FindWindowA"(ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
	
	Public Declare Function IsWindow Lib "user32.dll" (ByVal hwnd As Integer) As Integer
    Public Function GetWordVersion(ByRef r_sVersion As String) As Integer
        Dim lReturn As Integer
        Dim oWord As Object
        Dim sTemp As String
        Dim lHandle As Integer

        Try

            GetWordVersion = gPMConstants.PMEReturnCode.PMTrue

            If Len(m_sWordVersion) > 0 Then
                ' already know this value
                r_sVersion = m_sWordVersion
                Exit Function
            End If

            lReturn = StartWord(r_oWord:=oWord, r_lWordHandle:=lHandle, r_sWordVersion:=sTemp)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start Word", vApp:=ACApp, vClass:=ACClass, vMethod:="GetWordVersion", vErrNo:=Err.Number, vErrDesc:=Err.Description)

                'UPGRADE_NOTE: Object oWord may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oWord = Nothing
                Exit Function
            End If

            r_sVersion = sTemp

            lReturn = CloseWord(r_oWord:=oWord, lHandle:=lHandle, bSaveChanges:=False)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to close Word", vApp:=ACApp, vClass:=ACClass, vMethod:="GetWordVersion", vErrNo:=Err.Number, vErrDesc:=Err.Description)

                'UPGRADE_NOTE: Object oWord may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oWord = Nothing
                Exit Function
            End If


        Catch ex As Exception

            GetWordVersion = gPMConstants.PMEReturnCode.PMError
            'UPGRADE_NOTE: Object oWord may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oWord = Nothing

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetWordVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetWordVersion", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)


        End Try
    End Function

    Public Function StartWord(ByRef r_oWord As Object, Optional ByRef r_lWordHandle As Integer = 0, Optional ByRef r_sWordVersion As String = "") As Integer
        Dim sWordVersion As String
        Dim sTemp As String 'sj 08/11/2001
        Dim iTemp As Short
        Dim oTemp As Object 'MKW 07/11/03 PN7967 Open Word Erroring (with reference to Microsoft KB188546)
        Dim dLoopTime As Date
        Try


            StartWord = gPMConstants.PMEReturnCode.PMTrue

            ' SET 18/10/2004 ISS13245 - always create a new instance of word
            'MKW 07/11/03 PN7967 Open Word Erroring (with reference to Microsoft KB188546) START
            '    Set oTemp = CreateObject("Word.Application")
            r_oWord = CreateObject("Word.Application")
            '
            '    m_lReturn = CloseWord(oTemp, False)
            '    Set oTemp = Nothing

            ' SET 18/10/2004 ISS13245 - wait until word has started
            dLoopTime = DateAdd(Microsoft.VisualBasic.DateInterval.Second, ACMaxDelayInterval, Now)
            Do
                System.Windows.Forms.Application.DoEvents()
            Loop Until (dLoopTime <= Now) Or (WordHasStarted(r_oWord) = True)
            '    Loop Until (dLoopTime <= Now) Or (IsWordRunning(r_oWord) = True)

            ' save the handle for our window
            'UPGRADE_WARNING: Couldn't resolve default property of object r_oWord.Caption. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sTemp = r_oWord.Caption
            'UPGRADE_WARNING: Couldn't resolve default property of object r_oWord.Caption. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            r_oWord.Caption = "Tinny Boy"
            r_lWordHandle = FindWindow(vbNullString, "Tinny Boy")
            'UPGRADE_WARNING: Couldn't resolve default property of object r_oWord.Caption. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            r_oWord.Caption = sTemp

            'UPGRADE_WARNING: Couldn't resolve default property of object r_oWord.CommandBars. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            r_oWord.CommandBars("Standard").Visible = True
            'UPGRADE_WARNING: Couldn't resolve default property of object r_oWord.CommandBars. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            r_oWord.CommandBars("Formatting").Visible = True

            If r_lWordHandle = 0 Then
                StartWord = gPMConstants.PMEReturnCode.PMFalse

                sTemp = "Failed To Get Word Handle"
                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sTemp, vApp:=ACApp, vClass:=ACClass, vMethod:="StartWord")
                Exit Function
            End If

            sWordVersion = ""

            ' Now find the version of Word that we're running
            'UPGRADE_WARNING: Couldn't resolve default property of object r_oWord.Application. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sWordVersion = r_oWord.Application.Version

            If (sWordVersion <> "") Then
                iTemp = InStr(1, sWordVersion, ".")
                If (Val(Left(sWordVersion, iTemp)) < 8) Then
                    'sj 08/11/2001 - start
                    'MsgBox "Incorrect Word version for Sirius (" & sWordVersion & ")." & vbCrLf & "Contact Policy Master Support.", , ACApp
                    sTemp = "Incorrect Word version for Sirius (" & sWordVersion & ")." & vbCrLf & "Contact Policy Master Support."
                    iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sTemp, vApp:=ACApp, vClass:=ACClass, vMethod:="StartWord")
                    'sj 08/11/2001 - end
                    StartWord = gPMConstants.PMEReturnCode.PMFalse
                    'UPGRADE_NOTE: Object r_oWord may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                    r_oWord = Nothing
                    Exit Function
                End If
            Else
                'sj 08/11/2001 - start
                sTemp = "An error has occurred with Microsoft Word." & vbCrLf & "Try starting with Word already open or Contact Policy Master Support."
                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sTemp, vApp:=ACApp, vClass:=ACClass, vMethod:="StartWord")
                'sj 08/11/2001 - end
                StartWord = gPMConstants.PMEReturnCode.PMFalse
                'UPGRADE_NOTE: Object r_oWord may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                r_oWord = Nothing
                Exit Function
            End If

            ' return the version of word
            r_sWordVersion = sWordVersion

            ' store it in a module level variable
            m_sWordVersion = sWordVersion



        Catch ex As Exception

            StartWord = gPMConstants.PMEReturnCode.PMError

            'UPGRADE_NOTE: Object oTemp may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oTemp = Nothing

            ' Log Error.
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start Word", vApp:=ACApp, vClass:=ACClass, vMethod:="StartWord", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)


        End Try
    End Function

    Public Function CloseWord(ByRef r_oWord As Object, ByVal lHandle As Integer, Optional ByRef bSaveChanges As Boolean = True) As Integer
        Dim dLoopTime As Date
        Dim iCnt As Short
        Dim oTemplate As Object

        Try

            CloseWord = gPMConstants.PMEReturnCode.PMTrue

            If r_oWord Is Nothing Then
                Exit Function
            End If

            ' check if word is still running
            If IsWordRunning(lHandle) = False Then
                'UPGRADE_NOTE: Object r_oWord may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                r_oWord = Nothing
                Debug.Print("Close Word - word not running..")
                Exit Function
            End If

            ' set the saved property of the template so we are not prompted
            'UPGRADE_WARNING: Couldn't resolve default property of object r_oWord.Templates. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            For iCnt = 1 To r_oWord.Templates.Count
                'Set oTemplate = r_oWord.Templates.Item(1)
                'UPGRADE_WARNING: Couldn't resolve default property of object r_oWord.Templates. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                r_oWord.Templates.Item(iCnt).Saved = True
                'UPGRADE_WARNING: Couldn't resolve default property of object r_oWord.Templates. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Debug.Print(r_oWord.Templates.Item(iCnt).FullName)
            Next

            If bSaveChanges = True Then
                'UPGRADE_WARNING: Couldn't resolve default property of object r_oWord.Quit. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                r_oWord.Quit()
            Else
                'UPGRADE_WARNING: Couldn't resolve default property of object r_oWord.Quit. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                r_oWord.Quit(savechanges:=0)
            End If

            'MKW 061103 PN7967 Risk Register Hang on WS2K3/OfficeXP START
            dLoopTime = DateAdd(Microsoft.VisualBasic.DateInterval.Second, ACMaxDelayInterval, Now)
            Do
                System.Windows.Forms.Application.DoEvents()
            Loop Until (dLoopTime <= Now) Or (IsWordRunning(lHandle) = False)
            '    Loop Until (dLoopTime <= Now) Or (IsWordRunning(r_oWord) = False)

            'UPGRADE_NOTE: Object r_oWord may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            r_oWord = Nothing



        Catch ex As Exception

            CloseWord = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to close Word", vApp:=ACApp, vClass:=ACClass, vMethod:="CloseWord", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)


        End Try
    End Function

    Private Function IsWordRunning(ByVal lWindowHandle As Integer) As Boolean
        Try
        Dim lTemp As Integer

        lTemp = IsWindow(lWindowHandle)
        If lTemp = 0 Then
            IsWordRunning = False
        Else
            IsWordRunning = True
        End If

        Exit Function

        Catch ex As Exception
        IsWordRunning = False
        End Try
    End Function

    Private Function WordHasStarted(ByRef oWord As Object) As Boolean
        Try
        Dim sTemp As String

        ' Try and get the name of the object
        'UPGRADE_WARNING: Couldn't resolve default property of object oWord.Name. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        sTemp = oWord.Name
        WordHasStarted = True

        Exit Function

        Catch ex As Exception

        WordHasStarted = False

        End Try
    End Function

    Public WriteOnly Property Username() As String
        Set(ByVal Value As String)
            m_sUsername = Value
        End Set
    End Property

    Public Function ClearDirectory(ByVal sDirectory As String) As Integer
        Dim oFSO As Scripting.FileSystemObject
        Dim oFolder As Scripting.Folder
        Dim oSubFolder As Scripting.Folder
        Dim oFile As Scripting.File
        Try

            oFSO = New Scripting.FileSystemObject
        oFolder = oFSO.GetFolder(sDirectory)

        ' Clear the sub folders first
        For Each oSubFolder In oFolder.SubFolders
            oSubFolder.Delete(True)
        Next oSubFolder

        ' Clear the files then
        For Each oFile In oFolder.Files
            ' SET 12/10/2004 ISS15027 - forcibly delete the file if it readonly
            '     - locked files will still fail with a Permission Denied error
            oFile.Delete(True)
        Next oFile

        'UPGRADE_NOTE: Object oFile may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oFile = Nothing
        'UPGRADE_NOTE: Object oFolder may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oFolder = Nothing
        'UPGRADE_NOTE: Object oSubFolder may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oSubFolder = Nothing
        'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oFSO = Nothing

        ClearDirectory = gPMConstants.PMEReturnCode.PMTrue

        Exit Function

        Catch ex As Exception
        'UPGRADE_NOTE: Object oFile may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oFile = Nothing
        'UPGRADE_NOTE: Object oFolder may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oFolder = Nothing
        'UPGRADE_NOTE: Object oSubFolder may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oSubFolder = Nothing
        'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oFSO = Nothing
        ClearDirectory = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearDirectory Failed. Directory Name [" & sDirectory & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearDirectory", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)


        End Try
    End Function


    Public Function MoveFolderContents(ByVal sSourceDir As String, ByVal sDestDir As String) As Integer
        Dim oFSO As Scripting.FileSystemObject
        Dim iFolder As Scripting.Folder
        Dim iSubFolder As Scripting.Folder
        Dim iFile As Scripting.File
        Dim sTemp As String

        Try

            oFSO = New Scripting.FileSystemObject
            iFolder = oFSO.GetFolder(sSourceDir)

            ' copy the folders
            For Each iSubFolder In iFolder.SubFolders
                sTemp = oFSO.BuildPath(sDestDir, iSubFolder.Name)
                Call iSubFolder.Move(sTemp)
            Next iSubFolder

            ' copy the files
            For Each iFile In iFolder.Files
                sTemp = oFSO.BuildPath(sDestDir, iFile.Name)
                Call iFile.Move(sTemp)
            Next iFile

            MoveFolderContents = gPMConstants.PMEReturnCode.PMTrue

            'UPGRADE_NOTE: Object iFile may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            iFile = Nothing
            'UPGRADE_NOTE: Object iFolder may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            iFolder = Nothing
            'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFSO = Nothing
            Exit Function

        Catch ex As Exception
            'UPGRADE_NOTE: Object iFile may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            iFile = Nothing
            'UPGRADE_NOTE: Object iFolder may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            iFolder = Nothing
            'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFSO = Nothing
            MoveFolderContents = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MoveFolderContents Failed." & vbCrLf & "Source Directory [" & sSourceDir & "]" & vbCrLf & "Target Directory [" & sDestDir & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="MoveFolderContents", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)


        End Try
    End Function


    Public Function CreateFolderTree(ByVal sFolderName As String, Optional ByVal ClearDirectoryIfExists As Boolean = False) As Integer

        Dim lOK As Integer
        Dim vFSO As Scripting.FileSystemObject

        Try

        CreateFolderTree = gPMConstants.PMEReturnCode.PMTrue

        sFolderName = Trim(sFolderName)
        'Debug.Print "Checking folder: " & sFolderName

        If vFSO Is Nothing Then
            vFSO = New Scripting.FileSystemObject
        End If

        ' check that a folder name was actually supplied
        If Len(sFolderName) < 1 Then
            CreateFolderTree = gPMConstants.PMEReturnCode.PMError
            Exit Function
        End If

        ' is this the actual drive letter
        If vFSO.GetDriveName(sFolderName) = sFolderName Then
            Exit Function
        End If

        ' does the folder already exist
        If vFSO.FolderExists(sFolderName) = True Then

            ' Check if the flag is set, if the directory exists, we need
            ' to clear the directory
            If ClearDirectoryIfExists Then
                ' Cleaning the existing folder
                'Debug.Print "Cleaning exitsing folder: " & sFolderName
                m_lReturn = ClearDirectory(sFolderName)
            End If

            Exit Function
        End If

        ' attempt to create the parent folder
        lOK = CreateFolderTree(sFolderName:=vFSO.GetParentFolderName(sFolderName))

        ' did we error
        If lOK <> gPMConstants.PMEReturnCode.PMTrue Then
            ' return the error
            CreateFolderTree = lOK
            Exit Function
        End If

        ' create the folder
        'Debug.Print "creating folder: " & sFolderName
        Call vFSO.CreateFolder(sFolderName)

        ' did we succeed...?
        If vFSO.FolderExists(sFolderName) = False Then
            CreateFolderTree = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateFolderTree Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateFolderTree", vErrNo:=0, vErrDesc:="Unable to create the directory (" & sFolderName & ")")
        End If

        'UPGRADE_NOTE: Object vFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        vFSO = Nothing

        Exit Function

        Catch ex As Exception

        CreateFolderTree = gPMConstants.PMEReturnCode.PMError

        'clean up
        If vFSO Is Nothing Then
        Else
            'UPGRADE_NOTE: Object vFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            vFSO = Nothing
        End If

        ' Log Error Message
        iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateFolderTree Failed attempting to create " & sFolderName, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateFolderTree", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

      
        End Try
    End Function

    Public Function GetDocumentDirectory(ByRef r_sDocDirectory As String) As Integer
        Try
        Dim sDir As String

        GetDocumentDirectory = gPMConstants.PMEReturnCode.PMTrue
        sDir = ""

        m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="DocServer", r_sSettingValue:=sDir)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Document directory from Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentDirectory", vErrNo:=Err.Number, vErrDesc:=Err.Description)
            GetDocumentDirectory = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        r_sDocDirectory = sDir
        Exit Function

        Catch ex As Exception
        GetDocumentDirectory = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocumentDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentDirectory", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try
    End Function

    Public Function GetClientDirectory(ByRef r_sClientDir As String, Optional ByVal bUnique As Boolean = False) As Integer
        Dim sDir As String
        Dim sTemp As String
        Dim oFSO As Scripting.FileSystemObject

        Try

            GetClientDirectory = gPMConstants.PMEReturnCode.PMTrue
            oFSO = New Scripting.FileSystemObject
            sDir = ""

            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="DocClient", r_sSettingValue:=sDir)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Client directory from Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientDirectory", vErrNo:=Err.Number, vErrDesc:=Err.Description)

                GetClientDirectory = gPMConstants.PMEReturnCode.PMFalse
                'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oFSO = Nothing
                Exit Function
            End If

            sDir = oFSO.BuildPath(sDir, Trim(m_sUsername))
            If bUnique = True Then
                m_lReturn = GetUniqueName(r_sName:=sTemp)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get a temp directory.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClient", vErrNo:=Err.Number, vErrDesc:=Err.Description)

                    GetClientDirectory = gPMConstants.PMEReturnCode.PMFalse
                    'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                    oFSO = Nothing
                    Exit Function
                End If

                sDir = oFSO.BuildPath(sDir, sTemp)
            End If

            If oFSO.FolderExists(sDir) = False Then
                ' directory doesn't exist so attempt to create it
                m_lReturn = CreateFolderTree(sFolderName:=sDir)

                ' did we succeed...?
                If oFSO.FolderExists(sDir) = False Then
                    iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClientDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientDirectory", vErrNo:=0, vErrDesc:="Unable to create the directory (" & sDir & ")")

                    GetClientDirectory = gPMConstants.PMEReturnCode.PMError
                    'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                    oFSO = Nothing
                    Exit Function
                End If
            End If

            r_sClientDir = sDir
            'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFSO = Nothing
            Exit Function

        Catch ex As Exception
            'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFSO = Nothing
            GetClientDirectory = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClientDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientDirectory", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try
    End Function

    Public Function GetUniqueName(ByRef r_sName As String) As Integer
        Dim oFSO As Scripting.FileSystemObject
        Dim sTemp As String

        Try

            GetUniqueName = gPMConstants.PMEReturnCode.PMTrue

            oFSO = New Scripting.FileSystemObject
            ' Get a Temporary Name
            sTemp = oFSO.GetTempName

            ' Just get the Base Name, with no extension (i.e. without .tmp)
            sTemp = oFSO.GetBaseName(sTemp)

            'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFSO = Nothing
            If Len(sTemp) < 1 Then
                GetUniqueName = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            r_sName = sTemp
            Exit Function

        Catch ex As Exception
            GetUniqueName = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUniqueName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUniqueName", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)


        End Try
    End Function

    Public Function GetExportDirectory(ByRef r_sExportDirectory As String, Optional ByVal bUnique As Boolean = False) As Integer
        Dim sTemp As String
        Dim sDir As String
        Dim oFSO As Scripting.FileSystemObject

        Try

            GetExportDirectory = gPMConstants.PMEReturnCode.PMTrue
            oFSO = New Scripting.FileSystemObject

            'Get the DocZipTemp dir
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="PrntFileDir", r_sSettingValue:=sDir)

            'Make sure we have an install path
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sDir = "" Then
                ' Log Error Message
                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetExportDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExportDirectory", vErrNo:=Err.Number, vErrDesc:="Unable to find the registry entry for the PrintFile directory location")

                GetExportDirectory = gPMConstants.PMEReturnCode.PMError
                'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oFSO = Nothing
                Exit Function
            End If

            sDir = oFSO.BuildPath(sDir, ACDocExportDir & Trim(m_sUsername))

            'DC090904 PN14736 added unique directory so that if processing more than
            '                   one document it will have time to process one before doing the next
            If bUnique = True Then
                m_lReturn = GetUniqueName(r_sName:=sTemp)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get a temp directory.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExportDirectory", vErrNo:=Err.Number, vErrDesc:=Err.Description)

                    GetExportDirectory = gPMConstants.PMEReturnCode.PMFalse
                    'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                    oFSO = Nothing
                    Exit Function
                End If

                sDir = oFSO.BuildPath(sDir, sTemp)
            End If

            If oFSO.FolderExists(sDir) = False Then
                ' directory doesn't exist so attempt to create it
                m_lReturn = CreateFolderTree(sFolderName:=sDir)

                ' did we succeed...?
                If oFSO.FolderExists(sDir) = False Then
                    iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetExportDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExportDirectory", vErrNo:=0, vErrDesc:="Unable to create the directory (" & sDir & ")")

                    GetExportDirectory = gPMConstants.PMEReturnCode.PMError
                    'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                    oFSO = Nothing
                    Exit Function
                End If
            End If

            ' return the directory location
            r_sExportDirectory = sDir

            Exit Function

        Catch ex As Exception
            'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFSO = Nothing
            GetExportDirectory = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetExportDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExportDirectory", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try
    End Function


    Public Function GetZipDirectory(ByRef r_sZipDirectory As String) As Integer
        Dim sTemp As String
        Dim sZipDir As String
        Dim oFSO As Scripting.FileSystemObject

        Try

            GetZipDirectory = gPMConstants.PMEReturnCode.PMTrue
            oFSO = New Scripting.FileSystemObject

            'Get the DocZipTemp dir
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLClient, v_sSettingName:="DocZipPMDir", r_sSettingValue:=sTemp)

            'Make sure we have an install path
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sTemp = "" Then
                ' Log Error Message
                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetZipDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetZipDirectory", vErrNo:=Err.Number, vErrDesc:="Unable to find the registry entry for the doczip directory location")

                GetZipDirectory = gPMConstants.PMEReturnCode.PMError
                'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oFSO = Nothing
                Exit Function
            End If

            sZipDir = oFSO.BuildPath(sTemp, Trim(m_sUsername))

            m_lReturn = CreateFolderTree(sFolderName:=sZipDir, ClearDirectoryIfExists:=True)

            ' did we succeed...?
            If oFSO.FolderExists(sZipDir) = False Then
                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetZipDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetZipDirectory", vErrNo:=0, vErrDesc:="Unable to create the directory (" & sZipDir & ")")

                GetZipDirectory = gPMConstants.PMEReturnCode.PMError
                'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oFSO = Nothing
                Exit Function
            End If


            ' return the directory location
            r_sZipDirectory = sZipDir

            Exit Function

        Catch ex As Exception
            'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFSO = Nothing
            GetZipDirectory = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetZipDirectory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetZipDirectory", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try
    End Function
    Public Function UnZip(ByVal sZipFile As String, ByVal sOutputDirectory As String, Optional ByVal v_bDeleteZipFile As Boolean = False) As Integer
        Dim sFileIn As String
        Dim sFileOut As String
        Dim sTemp As String
        Dim bSuccess As Boolean
        'Dim ozipper As Object
        Dim oZipper As New bPMZipper.Business
        Dim sDependencyDir As String
        Dim oFSO As Scripting.FileSystemObject
        Dim oFolder As Scripting.Folder
        Dim oSubFolder As Scripting.Folder
        Dim oFile As Scripting.File
        Dim bIsHTML As Boolean

        Dim sZipFileName As String
        Dim sZipFileFolderName As String
        Dim m_bSameDirectory As Boolean

        Try

        UnZip = gPMConstants.PMEReturnCode.PMTrue
        oFSO = New Scripting.FileSystemObject
        ' does the file exist
        If oFSO.FileExists(sZipFile) = False Then
            UnZip = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="File (" & sZipFile & ") does not exist", vApp:=ACApp, vClass:=ACClass, vMethod:="UnZip", vErrNo:=Err.Number, vErrDesc:="Unzip failed")
            'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFSO = Nothing
            Exit Function
        End If
        ' does the output folder exist
        If oFSO.FolderExists(sOutputDirectory) = False Then
            UnZip = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Directory (" & sOutputDirectory & ") does not exist", vApp:=ACApp, vClass:=ACClass, vMethod:="UnZip", vErrNo:=Err.Number, vErrDesc:="Unzip failed")
            'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFSO = Nothing
            Exit Function
        End If
        ' Check if the Zip File is in the same output directory
        sZipFileName = oFSO.GetFileName(sZipFile)
        sZipFileName = LCase(sZipFileName)
        sZipFileFolderName = oFSO.GetParentFolderName(sZipFile)
        sZipFileFolderName = LCase(sZipFileFolderName)
        If LCase(sOutputDirectory) <> sZipFileFolderName Then
            ' since the zip file is not in the output directory, so we need to
            '  make sure the Output directory is empty,
            m_lReturn = ClearDirectory(sDirectory:=sOutputDirectory)
            ' Error message to be placed here
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                UnZip = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            ' Build the path
            sFileIn = oFSO.BuildPath(sOutputDirectory, sZipFileName)
            'DC090905 PN14736 have set not to delete, as at end of this
            '                   if option to delete is set it will
            m_lReturn = CopyFile(sZipFile, sFileIn, True, False, sTemp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                UnZip = m_lReturn
                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy file." & vbCrLf & "Source File      : " & sZipFile & vbCrLf & "Destination File : " & sFileIn & vbCrLf & "Error Details    : " & sTemp, vApp:=ACApp, vClass:=ACClass, vMethod:="UnZip", vErrNo:=Err.Number, vErrDesc:=Err.Description)
                Exit Function
            End If
        Else
            ' The zip file is in the same directory (output directory), so we need to
            ' clear the directory other than this zip file
            m_bSameDirectory = True
            sFileIn = sZipFile
            oFolder = oFSO.GetFolder(sZipFileFolderName)
            ' Clear the sub folders first
            For Each oSubFolder In oFolder.SubFolders
                oSubFolder.Delete(True)
            Next oSubFolder
            ' Clear the files then
            For Each oFile In oFolder.Files
                If LCase(oFile.Name) <> sZipFileName Then
                    oFile.Delete(True)
                End If
            Next oFile
        End If
        oZipper = New bPMZipper.Business
        'oZipper = CreateObject("bPmZipper.Business")
        If oZipper Is Nothing Then
            UnZip = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load zipper object", vApp:=ACApp, vClass:=ACClass, vMethod:="UnZip", vErrNo:=Err.Number, vErrDesc:="Unzip failed")
            Exit Function
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object oZipper.UnZipFiles. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'bSuccess = oZipper.UnZipFiles(sZipFileName:=sFileIn, sDestDirectory:=sOutputDirectory, sFileSpec:="*.*", bNoDirectoryNamesFlag:=True)
        bSuccess = oZipper.UnZipFile(sZipFileName:=sFileIn, sDestDirectory:=sOutputDirectory)
        'UPGRADE_NOTE: Object oZipper may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oZipper = Nothing
        If (bSuccess = False) Then
            UnZip = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        If Not m_bSameDirectory Then
            ' remove the zip file in the target directory, since this is
            ' a copy of the original file
            Call oFSO.DeleteFile(sFileIn, True)
        End If

        ' what type of files have we extracted
        m_lReturn = GetFileNameAndType(v_sFolder:=sOutputDirectory, r_sFileName:=sFileOut, r_bHTMLFormat:=bIsHTML)

        If bIsHTML = True Then
            'Move extra files into dependency folder
            sDependencyDir = Left(sFileOut, Len(sFileOut) - 4) & "_files"
            sDependencyDir = oFSO.BuildPath(sOutputDirectory, sDependencyDir)
            If oFSO.FolderExists(sDependencyDir) = False Then
                m_lReturn = CreateFolderTree(sDependencyDir)
                ' did we create the folder
                If oFSO.FolderExists(sDependencyDir) = False Then
                    UnZip = gPMConstants.PMEReturnCode.PMError
                    ' Log Error.
                    iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create directory (" & sDependencyDir & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="UnZip", vErrNo:=Err.Number, vErrDesc:="Unzip failed")
                    Exit Function
                End If
            End If
            ' move the files to the sub directory
            oFolder = oFSO.GetFolder(sOutputDirectory)
            For Each oFile In oFolder.Files
                If LCase(oFile.Name) <> sZipFileName Then
                    ' Don't move the zip file
                    If oFile.Name <> sFileOut Then
                        'Debug.Print "moving file " & oFile.name
                        sFileIn = oFSO.BuildPath(sDependencyDir, oFile.Name)
                        oFile.Move(sFileIn)
                    End If
                End If
            Next oFile
        End If

        ' RAM20040301 : Delete the zip file
        If v_bDeleteZipFile Then
            If (oFSO.FileExists(sZipFile)) Then
                Call oFSO.DeleteFile(sZipFile, True)
            End If
        End If

        'UPGRADE_NOTE: Object oFile may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oFile = Nothing
        'UPGRADE_NOTE: Object oFolder may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oFolder = Nothing
        'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oFSO = Nothing

        Catch ex As Exception

        UnZip = gPMConstants.PMEReturnCode.PMError

        'Clean up
        If oZipper Is Nothing Then
        Else
            'UPGRADE_NOTE: Object oZipper may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oZipper = Nothing
        End If
        If oFile Is Nothing Then
        Else
            'UPGRADE_NOTE: Object oFile may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFile = Nothing
        End If
        If oFolder Is Nothing Then
        Else
            'UPGRADE_NOTE: Object oFolder may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFolder = Nothing
        End If
        If oFSO Is Nothing Then
        Else
            'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFSO = Nothing
        End If

        ' Log Error.
        iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unzip the document. " & vbCrLf & "Zip File [" & sZipFile & "]" & vbCrLf & "sOutputDirectory [" & sOutputDirectory & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="UnZip", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

     
        End Try
    End Function


    Public Function GetFileNameAndType(ByVal v_sFolder As String, ByRef r_sFileName As String, ByRef r_bHTMLFormat As Boolean) As Integer
        Dim oFSO As Scripting.FileSystemObject
        Dim oFolder As Scripting.Folder
        Dim vFile As Object
        Dim sExt As String
        Dim sPath As String


        Try

        GetFileNameAndType = gPMConstants.PMEReturnCode.PMTrue
        r_sFileName = ""
        r_bHTMLFormat = False

        oFSO = New Scripting.FileSystemObject

        ' does this folder actually exist
        If oFSO.FolderExists(v_sFolder) = False Then
            GetFileNameAndType = gPMConstants.PMEReturnCode.PMNotFound
            'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFSO = Nothing
            Exit Function
        End If

        oFolder = oFSO.GetFolder(v_sFolder)

        For Each vFile In oFolder.Files
            'UPGRADE_WARNING: Couldn't resolve default property of object vFile.Name. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            sExt = oFSO.GetExtensionName(sPath & "\" & vFile.Name)
            Select Case (UCase(sExt))
                Case "HTM"
                    'UPGRADE_WARNING: Couldn't resolve default property of object vFile.Name. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    r_sFileName = vFile.Name
                    r_bHTMLFormat = True
                    'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                    oFSO = Nothing
                    'UPGRADE_NOTE: Object oFolder may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                    oFolder = Nothing
                    Exit Function
                Case "DOC"
                    'UPGRADE_WARNING: Couldn't resolve default property of object vFile.Name. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    r_sFileName = vFile.Name
                    r_bHTMLFormat = False
                    'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                    oFSO = Nothing
                    'UPGRADE_NOTE: Object oFolder may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                    oFolder = Nothing
                    Exit Function
            End Select
        Next vFile

        'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oFSO = Nothing
        'UPGRADE_NOTE: Object oFolder may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oFolder = Nothing

        Exit Function

        Catch ex As Exception
        'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oFSO = Nothing
        'UPGRADE_NOTE: Object oFolder may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oFolder = Nothing
        GetFileNameAndType = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFileNameAndType Failed. File Name : " & v_sFolder, vApp:=ACApp, vClass:=ACClass, vMethod:="GetFileNameAndType", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try
    End Function


    Public Function DelDirectory(ByRef r_sDocDirectory As String) As Integer
        Dim oFSO As Scripting.FileSystemObject
        Try

            DelDirectory = gPMConstants.PMEReturnCode.PMTrue


            oFSO = New Scripting.FileSystemObject

            ' does this folder actually exist
            If oFSO.FolderExists(r_sDocDirectory) = True Then

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
                'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                Dir("")

                Call oFSO.DeleteFolder(r_sDocDirectory, Force:=True)
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' RAM20040205 : Bug fix for PN Issue 10231 - END
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            End If

            'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFSO = Nothing


        Catch ex As Exception
            DelDirectory = gPMConstants.PMEReturnCode.PMError

            If oFSO Is Nothing Then
            Else
                'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oFSO = Nothing
            End If

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DelDirectory Failed. Directory Name : " & r_sDocDirectory, vApp:=ACApp, vClass:=ACClass, vMethod:="DelDirectory", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)


        End Try
    End Function


    ' Funtion to check if a Folder exists using FileSystemObject
    Public Function IsFolderExists(ByVal v_sFolderName As String) As Integer
        Dim lReturn As Integer
        Dim oFSO As Object
        Try

        
        oFSO = CreateObject("Scripting.FileSystemObject")

        IsFolderExists = gPMConstants.PMEReturnCode.PMFalse

        'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.FolderExists. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If (oFSO.FolderExists(v_sFolderName)) Then
            IsFolderExists = gPMConstants.PMEReturnCode.PMTrue
        Else
            IsFolderExists = gPMConstants.PMEReturnCode.PMNotFound
        End If

        'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oFSO = Nothing

        Exit Function

        Catch ex As Exception

        IsFolderExists = gPMConstants.PMEReturnCode.PMError

        If oFSO Is Nothing Then
        Else
            'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFSO = Nothing
        End If

        ' Log Error Message
        iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Check FolderExists for '" & v_sFolderName & "'", vApp:=ACApp, vClass:=ACClass, vMethod:="IsFolderExists", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function


    ' Funtion to check if file exists using FileSystemObject
    Public Function IsFileExists(ByVal v_sFileName As String) As Integer
        Dim lReturn As Integer
        Dim oFSO As Object

        Try

            oFSO = CreateObject("Scripting.FileSystemObject")

        IsFileExists = gPMConstants.PMEReturnCode.PMFalse

        'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.FileExists. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If (oFSO.FileExists(v_sFileName)) Then
            IsFileExists = gPMConstants.PMEReturnCode.PMTrue
        Else
            IsFileExists = gPMConstants.PMEReturnCode.PMNotFound
        End If

        'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oFSO = Nothing

        Exit Function

        Catch ex As Exception

        IsFileExists = gPMConstants.PMEReturnCode.PMError

        If oFSO Is Nothing Then
        Else
            'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFSO = Nothing
        End If

        ' Log Error Message
        iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Check FileExists for '" & v_sFileName & "'", vApp:=ACApp, vClass:=ACClass, vMethod:="IsFileExists", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function


    ' Funtion to Create a Folder using FileSystemObject
    Public Function CreateFolder(ByVal v_sFolderName As String) As Integer
        Dim lReturn As Integer
        Dim oFSO As Object
        Try


            oFSO = CreateObject("Scripting.FileSystemObject")

            CreateFolder = gPMConstants.PMEReturnCode.PMFalse

            ' An error will occurs if the specified folder already exists
            ' so check the existance of the folder

            'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.FolderExists. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If (oFSO.FolderExists(v_sFolderName)) Then
                ' do nothing, since the folder is there
                CreateFolder = gPMConstants.PMEReturnCode.PMTrue
            Else
                'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.CreateFolder. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If (oFSO.CreateFolder(v_sFolderName)) Then
                    ' did we succeed...?
                    'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.FolderExists. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If oFSO.FolderExists(v_sFolderName) = True Then
                        CreateFolder = gPMConstants.PMEReturnCode.PMTrue
                    Else
                        CreateFolder = gPMConstants.PMEReturnCode.PMError
                        iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateFolder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateFolder", vErrNo:=0, vErrDesc:="Unable to create the directory (" & v_sFolderName & ")")
                    End If
                End If
            End If

            'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFSO = Nothing

            Exit Function

        Catch ex As Exception

            CreateFolder = gPMConstants.PMEReturnCode.PMError

            If oFSO Is Nothing Then
            Else
                'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oFSO = Nothing
            End If

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CreateFolder Unable to create the directory (" & v_sFolderName & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateFolder", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try
    End Function


    ' Function to delete the file if file exists using FileSystemObject
    ' Incorporate retrying. PN24093.
    Public Function DeleteFile(ByVal v_sFileName As String) As Integer

        Dim iRetries As Short
        Dim lStoreErrNum As Integer
        Dim sStoreErrDesc As String
        Dim dStoreDateTime As Date

        On Error GoTo Err_DeleteFile

        Dim lReturn As Integer
        Dim oFSO As Object
        oFSO = CreateObject("Scripting.FileSystemObject")

        DeleteFile = gPMConstants.PMEReturnCode.PMFalse

        iRetries = PMFileOperationRetryAttempts
        Do Until iRetries = 0
            Err.Clear()
            On Error Resume Next

            ' Delete file
            'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.FileExists. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If oFSO.FileExists(v_sFileName) Then
                ' Setting the ForceDelete Parameter to True, so that it will force
                ' the file to delete (even it is readonly)
                'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.DeleteFile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                oFSO.DeleteFile(v_sFileName, True)
            End If

            lStoreErrNum = Err.Number
            sStoreErrDesc = Err.Description

            On Error GoTo Err_DeleteFile

            iRetries = iRetries - 1

            ' Reconfirm again
            'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.FileExists. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If oFSO.FileExists(v_sFileName) Then
                If iRetries = 0 Then
                    ' Delete file fails
                    ' Log Error Message
                    iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Delete file '" & v_sFileName & "'", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteFile", vErrNo:=Err.Number, vErrDesc:=Err.Description)
                Else
                    LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete " & v_sFileName & ". Retries remaining: " & iRetries, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteFile", excep:= New Exception(sStoreErrDesc))
                    dStoreDateTime = Now
                    'UPGRADE_WARNING: DateDiff behavior may be different. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6B38EC3F-686D-4B2E-B5A5-9E8E7A762E32"'
                    Do Until System.Math.Abs(DateDiff(Microsoft.VisualBasic.DateInterval.Second, dStoreDateTime, Now)) >= PMFileOperationRetryWait
                        System.Windows.Forms.Application.DoEvents()
                    Loop

                End If
            Else
                DeleteFile = gPMConstants.PMEReturnCode.PMTrue
                Exit Do
            End If

        Loop

        'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oFSO = Nothing

        Exit Function

Err_DeleteFile:

        DeleteFile = gPMConstants.PMEReturnCode.PMError

        If oFSO Is Nothing Then
        Else
            'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFSO = Nothing
        End If

        ' Log Error Message
        iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Delete file '" & v_sFileName & "'", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteFile", vErrNo:=Err.Number, vErrDesc:=Err.Description)

        Exit Function
        Resume
    End Function


    Public Function CopyFile(ByVal v_sSourceFile As String, ByVal v_sDestinationFile As String, Optional ByVal v_bOverWriteFiles As Boolean = True, Optional ByVal v_bDeleteSoureFile As Boolean = False, Optional ByRef r_vErrorMessage As Object = Nothing) As Integer

        Dim oFSO As Object
        Dim sDestinationFolderName As String
        Dim oFolder As Object
        Dim sErrorMessage As String
        Dim bCanCopy As Boolean
        Dim bFileCopiped As Boolean
        Dim sFinalFileName As String

        Try

       

        CopyFile = gPMConstants.PMEReturnCode.PMTrue

        v_sSourceFile = Trim(v_sSourceFile)
        v_sDestinationFile = Trim(v_sDestinationFile)
        sErrorMessage = ""

        ' Deal with null input
        If Len(v_sSourceFile) = 0 Then
            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Source file name is null", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFile", vErrNo:=Err.Number, vErrDesc:=Err.Description)
            CopyFile = gPMConstants.PMEReturnCode.PMError
            Exit Function
        End If

        If Len(v_sDestinationFile) = 0 Then
            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Source file name is null", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFile", vErrNo:=Err.Number, vErrDesc:=Err.Description)
            CopyFile = gPMConstants.PMEReturnCode.PMError
            Exit Function
        End If

        ' Create FSO
        oFSO = CreateObject("Scripting.FileSystemObject")

        'Check the Souce file exists
        'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.FileExists. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If oFSO.FileExists(v_sSourceFile) Then

            ' If the destination file name contains a File Extension, then
            ' then v_sDestinationFile is a File Name
            ' else v_sDestinationFile is a Folder Name
            'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.GetExtensionName. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If Len(oFSO.GetExtensionName(v_sDestinationFile)) > 0 Then
                ' Get the destination folder, since we have a file name
                'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.GetParentFolderName. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sDestinationFolderName = oFSO.GetParentFolderName(v_sDestinationFile)
                sFinalFileName = v_sDestinationFile
            Else
                'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.GetAbsolutePathName. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sDestinationFolderName = oFSO.GetAbsolutePathName(v_sDestinationFile)
                'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.GetFileName. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sFinalFileName = sDestinationFolderName & "\" & oFSO.GetFileName(v_sSourceFile)
            End If

            ' Check the destination folder exists
            'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.FolderExists. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If oFSO.FolderExists(sDestinationFolderName) = False Then
                ' Create the folder, since it not exists.
                m_lReturn = CreateFolderTree(sDestinationFolderName)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Clear Memory
                    If oFolder Is Nothing Then
                    Else
                        'UPGRADE_NOTE: Object oFolder may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                        oFolder = Nothing
                    End If
                    If oFSO Is Nothing Then
                    Else
                        'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                        oFSO = Nothing
                    End If

                    CopyFile = gPMConstants.PMEReturnCode.PMError

                    ' Log Error Message
                    iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to create the Destination Folder (" & sDestinationFolderName & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFile", vErrNo:=Err.Number, vErrDesc:=Err.Description)
                    Exit Function
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

            'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.GetFolder. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            oFolder = oFSO.GetFolder(sDestinationFolderName)

            sErrorMessage = "The folder [" & sDestinationFolderName & "] is having Folling Attributes" & vbCrLf

            bCanCopy = True

            ' Check if the folder is ReadOnly
            'UPGRADE_WARNING: Couldn't resolve default property of object oFolder.Attributes. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If oFolder.Attributes And 1 Then
                sErrorMessage = sErrorMessage & "Read-Only" & vbCrLf
                bCanCopy = False
            End If

            'UPGRADE_WARNING: Couldn't resolve default property of object oFolder.Attributes. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If oFolder.Attributes And 4 Then
                sErrorMessage = sErrorMessage & "System File" & vbCrLf
                bCanCopy = False
            End If

            'If oFolder.Attributes And 16 Then
            '    sErrorMessage = sErrorMessage & "Disk drive volume label" & vbCrLf
            '    bCanCopy = False
            'End If

            'UPGRADE_WARNING: Couldn't resolve default property of object oFolder.Attributes. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If oFolder.Attributes And 64 Then
                sErrorMessage = sErrorMessage & "Link or shortcut" & vbCrLf
                bCanCopy = False
            End If

            'UPGRADE_WARNING: Couldn't resolve default property of object oFolder.Attributes. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If oFolder.Attributes And 2048 Then
                sErrorMessage = sErrorMessage & "Compressed file" & vbCrLf
                bCanCopy = False
            End If

            If bCanCopy Then

                ' Issue the copy command
                'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.CopyFile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                oFSO.CopyFile(v_sSourceFile, sFinalFileName, v_bOverWriteFiles)

                ' Do we need to wait here, so that the file exists.
                'Loop until file exists
                Do
                    bFileCopiped = IsFileExists(sFinalFileName)
                Loop While Not bFileCopiped

                'Now the file is copied, check if the v_bDeleteSoureFile flag is set,
                If v_bDeleteSoureFile Then
                    ' We need to delete the source file
                    'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.DeleteFile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    oFSO.DeleteFile(v_sSourceFile, True)
                End If

            Else

                ' Clear Memory
                If oFolder Is Nothing Then
                Else
                    'UPGRADE_NOTE: Object oFolder may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                    oFolder = Nothing
                End If
                If oFSO Is Nothing Then
                Else
                    'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                    oFSO = Nothing
                End If

                ' Return Error Message
                CopyFile = gPMConstants.PMEReturnCode.PMError

                'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"'
                If IsNothing(r_vErrorMessage) Then
                Else
                    'UPGRADE_WARNING: Couldn't resolve default property of object r_vErrorMessage. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    r_vErrorMessage = sErrorMessage
                End If

                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Copy the File (" & v_sSourceFile & ") because" & vbCrLf & sErrorMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFile", vErrNo:=Err.Number, vErrDesc:=Err.Description)
                Exit Function
            End If

        Else

            CopyFile = gPMConstants.PMEReturnCode.PMError
            Exit Function

        End If

        ' Clear Memory
        If oFolder Is Nothing Then
        Else
            'UPGRADE_NOTE: Object oFolder may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFolder = Nothing
        End If
        If oFSO Is Nothing Then
        Else
            'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oFSO = Nothing
        End If

        Exit Function

        Catch ex As Exception



            CopyFile = gPMConstants.PMEReturnCode.PMError

            ' Clear Memory
            If oFolder Is Nothing Then
            Else
                'UPGRADE_NOTE: Object oFolder may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oFolder = Nothing
            End If
            If oFSO Is Nothing Then
            Else
                'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oFSO = Nothing
            End If

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy " & v_sSourceFile & " to " & v_sDestinationFile, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFile", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

             End Try
    End Function


    Public Function CopyFolder(ByVal v_sSourceFolder As String, ByVal v_sDestinationFolder As String, Optional ByVal v_bOverWriteFiles As Boolean = True, Optional ByRef r_vErrorMessage As Object = Nothing) As Integer

        Dim sErrorMessage As String
        Dim oFSO As Object

        Try





            CopyFolder = gPMConstants.PMEReturnCode.PMTrue

            v_sSourceFolder = Trim(v_sSourceFolder)
            v_sDestinationFolder = Trim(v_sDestinationFolder)

            ' Deal with null input
            If Len(v_sSourceFolder) = 0 Then

                sErrorMessage = sErrorMessage & "Source Folder name is null." & vbCrLf
                ' Log Error Message
                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Source Folder name is null", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFolder", vErrNo:=Err.Number, vErrDesc:=Err.Description)
                CopyFolder = gPMConstants.PMEReturnCode.PMError
                Exit Function
            End If

            If Len(v_sDestinationFolder) = 0 Then
                sErrorMessage = sErrorMessage & "Destination Folder name is null." & vbCrLf
                ' Log Error Message
                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Destination Folder name is null", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFolder", vErrNo:=Err.Number, vErrDesc:=Err.Description)
                CopyFolder = gPMConstants.PMEReturnCode.PMError
                Exit Function
            End If

            If Right(v_sSourceFolder, 1) = "\" Then
                v_sSourceFolder = Left(v_sSourceFolder, Len(v_sSourceFolder) - 1)
            End If

            If Right(v_sDestinationFolder, 1) = "\" Then
                v_sDestinationFolder = Left(v_sDestinationFolder, Len(v_sDestinationFolder) - 1)
            End If

            oFSO = CreateObject("Scripting.FileSystemObject")

            ' Check if the Source folder exists
            'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.FolderExists. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If (oFSO.FolderExists(v_sSourceFolder)) Then
            Else
                CopyFolder = gPMConstants.PMEReturnCode.PMError

                sErrorMessage = sErrorMessage & "Missing Source Folder [" & v_sSourceFolder & "]" & vbCrLf

                ' Log Error Message
                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Missing Source Folder [" & v_sSourceFolder & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFolder", vErrNo:=Err.Number, vErrDesc:=Err.Description)
                CopyFolder = gPMConstants.PMEReturnCode.PMError
                Exit Function
            End If

            ' Issue the copy folder command
            'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.CopyFolder. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            oFSO.CopyFolder(v_sSourceFolder, v_sDestinationFolder, v_bOverWriteFiles)

            ' Do we need to check ????
            ' We are not doing it at the moment, if needed may me in the future


            ' Clean up memory
            If oFSO Is Nothing Then
            Else
                'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oFSO = Nothing
            End If

            ' Return the message, if asked for
            'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"'
            If IsNothing(r_vErrorMessage) Then
            Else
                'UPGRADE_WARNING: Couldn't resolve default property of object r_vErrorMessage. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                r_vErrorMessage = sErrorMessage
            End If



        Catch ex As Exception

            CopyFolder = gPMConstants.PMEReturnCode.PMError

            If oFSO Is Nothing Then
            Else
                'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oFSO = Nothing
            End If

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Copy Directory" & vbCrLf & "From : [" & v_sSourceFolder & "]" & vbCrLf & "To   : [" & v_sDestinationFolder & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFolder", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

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

        Dim iTemp As Short
        Dim sFileIn As String
        Dim sFileOut As String
        Dim sParentFile As String
        Dim sDependencyDir As String
        Dim sDependency As String
        Dim oZipper As bPMZipper.Business
        Dim oFSO As Object
        Dim oFolder As Object
        Dim oFilesCollection As Object
        Dim oFile As Object
        Dim sFileNameWithPath As String

        Try



            sFileIn = Left(sPath, Len(sPath) - 3) & sInputDocumentFileExtension
            sFileOut = sPath

            ' Create the zipper component
            oZipper = New bPMZipper.Business
            If oZipper Is Nothing Then
                Zip = gPMConstants.PMEReturnCode.PMError
                ' Log Error.
                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load zipper object", vApp:=ACApp, vClass:=ACClass, vMethod:="Zip", vErrNo:=Err.Number, vErrDesc:="Zip failed")

                Exit Function
            End If

            'UPGRADE_WARNING: Couldn't resolve default property of object oZipper.ZipFile. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            iTemp = oZipper.ZipFile(sFileIn:=sFileIn, sFileOut:=sFileOut)
            If (iTemp = False) Then
                'UPGRADE_NOTE: Object oZipper may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oZipper = Nothing
                Zip = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            'Enable zipping up of multiple files to include HTML dependencies.
            'Check for dependencies and add them to the zip file if they exist.
            sParentFile = Left(sFileIn, Len(sPath) - 4)
            sDependencyDir = sParentFile & "_files"

            ' Create the FSO
            oFSO = CreateObject("Scripting.FileSystemObject")

            ' Check if dependencies folder exist
            'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.FolderExists. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If oFSO.FolderExists(sDependencyDir) = True Then

                'UPGRADE_WARNING: Couldn't resolve default property of object oFSO.GetFolder. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                oFolder = oFSO.GetFolder(sDependencyDir)
                'UPGRADE_WARNING: Couldn't resolve default property of object oFolder.Files. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                oFilesCollection = oFolder.Files

                For Each oFile In oFilesCollection

                    ' Get each file name
                    'UPGRADE_WARNING: Couldn't resolve default property of object oFile.Name. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    sDependency = oFile.Name

                    ' Append the directory name to it
                    sFileNameWithPath = sDependencyDir & "\" & sDependency

                    ' Add it to the zip file
                    'UPGRADE_WARNING: Couldn't resolve default property of object oZipper.addFileToZIP. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    iTemp = oZipper.addFileToZIP(sFileOut, sFileNameWithPath)

                    ' Remvoe the file
                    m_lReturn = DeleteFile(sFileNameWithPath)

                Next oFile

                'Clean up
                'UPGRADE_NOTE: Object oZipper may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oZipper = Nothing
                'UPGRADE_NOTE: Object oFile may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oFile = Nothing
                'UPGRADE_NOTE: Object oFilesCollection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oFilesCollection = Nothing
                'UPGRADE_NOTE: Object oFolder may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oFolder = Nothing
                'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oFSO = Nothing

                'Remove the dependency directory for all
                m_lReturn = DelDirectory(sDependencyDir)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    Zip = gPMConstants.PMEReturnCode.PMError
                    ' Log Error.
                    iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to delete the dependency folder. (" & sDependencyDir & "')", vApp:=ACApp, vClass:=ACClass, vMethod:="Zip", vErrNo:=Err.Number, vErrDesc:="Zip failed")
                End If

            End If

            ' We Zipped the original file with dependency folder(if any), so get rid of source file
            m_lReturn = DeleteFile(sFileIn)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Zip = gPMConstants.PMEReturnCode.PMError
                ' Log Error.
                iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to delete the file. (" & sFileIn & "')", vApp:=ACApp, vClass:=ACClass, vMethod:="Zip", vErrNo:=Err.Number, vErrDesc:="Zip failed")
            End If

            Zip = gPMConstants.PMEReturnCode.PMTrue

            Exit Function

        Catch ex As Exception


            Zip = gPMConstants.PMEReturnCode.PMError

            'Clean up
            If oZipper Is Nothing Then
            Else
                'UPGRADE_NOTE: Object oZipper may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oZipper = Nothing
            End If
            If oFile Is Nothing Then
            Else
                'UPGRADE_NOTE: Object oFile may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oFile = Nothing
            End If
            If oFilesCollection Is Nothing Then
            Else
                'UPGRADE_NOTE: Object oFilesCollection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oFilesCollection = Nothing
            End If
            If oFolder Is Nothing Then
            Else
                'UPGRADE_NOTE: Object oFolder may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oFolder = Nothing
            End If
            If oFSO Is Nothing Then
            Else
                'UPGRADE_NOTE: Object oFSO may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                oFSO = Nothing
            End If

            ' Log Error.
            iPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to zip the document. (" & sFileIn & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="Zip", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try

    End Function
End Module
