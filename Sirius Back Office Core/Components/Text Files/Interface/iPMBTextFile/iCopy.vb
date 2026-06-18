Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports Word = Microsoft.Office.Interop.Word
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Copy_NET.Copy")> _
Public NotInheritable Class Copy
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface

    Private Const ACClass As String = "Copy"

    Private m_sClientDir As String = ""
    Private m_sZipDir As String = ""

    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_oBusiness As Object

    'DJM 24/10/2003 : Copy text files from one policy to another.
    Public Function CopyTextFiles(ByVal v_lOldInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vOldTextFiles As Object = Nothing
        Dim vNewTextFiles(,) As Object = Nothing

        Dim lSlotNumber, lOldFileNumber, lNewFileNumber As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMDocFunctions.Username = g_sUserName

            'Create text file table entries for new policy.

            m_lReturn = m_oBusiness.CopyTextFiles(v_lOldInsuranceFileCnt, v_lNewInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oBusiness.GetTextFiles(v_lOldInsuranceFileCnt, vOldTextFiles)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oBusiness.GetTextFiles(v_lNewInsuranceFileCnt, vNewTextFiles)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'MKW120104 PN9536.  START.  Cannot copy files with no source or destination.
            If Not (Information.IsArray(vOldTextFiles)) Or Not (Information.IsArray(vNewTextFiles)) Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If
            'MKW120104 PN9536.  END.

            'Loop though all text files to copy.

            For lLoop As Integer = vNewTextFiles.GetLowerBound(1) To vNewTextFiles.GetUpperBound(1)


                lSlotNumber = CInt(vNewTextFiles(0, lLoop))

                lOldFileNumber = CInt(vOldTextFiles(1, lLoop))

                lNewFileNumber = CInt(vNewTextFiles(1, lLoop))

                m_lReturn = CType(GetClientDirectory(m_sClientDir, True), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = CType(GetZipDirectory(m_sZipDir), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Retrieve text file
                m_lReturn = CType(CopyServerToClient(lSlotNumber, lOldFileNumber), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Rename and return text file
                m_lReturn = CType(CopyClientToServer(lSlotNumber, lOldFileNumber, lNewFileNumber), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = CType(DelDirectory(m_sClientDir), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next lLoop

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyTextFiles Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyTextFiles", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function CopyServerToClient(ByVal v_lSlotNumber As Integer, ByVal v_lFileNumber As Integer) As Integer

        Dim result As Integer = 0
        Dim sServerPath As String = String.Empty
        Dim sZipPath As String
        Dim lShortFileNumber As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = CType(GetServerPath(sServerPath, v_lSlotNumber, v_lFileNumber), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If FileSystem.Dir(sServerPath, FileAttribute.Normal) <> "" Then

            lShortFileNumber = v_lFileNumber - ((v_lFileNumber \ 500) * 500)

            sZipPath = m_sZipDir & "\Doc " & CStr(lShortFileNumber) & ".zip"

            m_lReturn = CType(UnZip(sServerPath, m_sZipDir), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(MoveFolderContents(m_sZipDir, m_sClientDir), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        Return result

    End Function


    Private Function GetServerPath(ByRef r_sPath As String, ByVal v_lSlotNumber As Integer, ByVal v_lFileNumber As Integer) As Integer

        Dim result As Integer = 0
        Dim sTemp As String = ""
        Dim lTemp, lTemp2 As Integer
        Dim sServerDir As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = CType(GetDocumentDirectory(sServerDir), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lTemp = v_lFileNumber \ 500

        lTemp2 = v_lFileNumber - (lTemp * 500)

        sTemp = "Policy Text Files"

        r_sPath = sServerDir & "\" & sTemp & "\Slot " & CStr(v_lSlotNumber) & "\" & StringsHelper.Format(lTemp, "000") & "\" & StringsHelper.Format(lTemp2, "000") & ".zip"

        Return result

    End Function

    Public Function CopyClientToServer(ByVal v_lSlotNumber As Integer, ByVal v_lOldFileNumber As Integer, ByVal v_lNewFileNumber As Integer) As Integer

        Dim result As Integer = 0
        Dim sServerPath As String = String.Empty
        Dim sClient As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(CopyFilesToZipTemp(m_sZipDir, m_sClientDir, v_lOldFileNumber, v_lNewFileNumber), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(GetAndCreatePath(sServerPath, v_lSlotNumber, v_lNewFileNumber), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sClient = m_sZipDir & "\Doc " & (CStr(v_lNewFileNumber - ((v_lNewFileNumber \ 500) * 500))) & ".zip"

            m_lReturn = CType(Zip(sClient), gPMConstants.PMEReturnCode)

            File.Copy(sClient, sServerPath)

            'Delete the local copy
            File.Delete(sClient)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to copy template from client to server", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClientToServer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function CopyFilesToZipTemp(ByVal v_sZipDir As String, ByVal v_sClientDir As String, ByVal v_lOldFileNumber As Integer, ByVal v_lNewFileNumber As Integer) As Integer

        Dim result As Integer = 0
        Dim sClient, sDependencyDir, sDependency, sParentFile, sDepDirName As String
        Dim lShortOldFileNumber, lShortNewFileNumber As Integer

        'AR20050411 - PN20013
        Dim oFSO As Object
        Dim oFolder As DirectoryInfo
        Dim oWord As Word.Application
        Dim oDocument As Word.Document
        Dim lWordHandle As Integer
        Dim sFileName As String = ""
        Dim bWordOpen As Boolean
        Dim lReturn As gPMConstants.PMEReturnCode
        'AR20050411 - PN20013

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'AR20050411 - PN20013
            oFSO = New Object()
            oFolder = New DirectoryInfo(v_sClientDir)

            'Loop through the files in the client directory
            For Each oFile As FileInfo In oFolder.GetFiles
                'Check for word documents
                If oFile.Name.ToUpper().EndsWith(".DOC") Then

                    'AR20050411 - PN20013 Rename the .doc to the standard naming of Sirius .htm files
                    sFileName = "Doc " & (CStr(v_lOldFileNumber - ((v_lOldFileNumber \ 500) * 500))) & ".HTM"

                    If oWord Is Nothing Then
                        'Start up Microsoft Word
                        lReturn = CType(StartWord(oWord, lWordHandle), gPMConstants.PMEReturnCode)

                        If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                            bWordOpen = True
                        Else
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                    'Developer Guide No.177
                    oDocument = oWord.Documents.Open(oFile.DirectoryName & "\" & oFile.Name, ConfirmConversions:=False)

                    'Save the document as html
                    'Developer Guide No.177
                    oDocument.SaveAs(oFile.DirectoryName & "\" & sFileName, Word.WdSaveFormat.wdFormatHTML)
                    oDocument.Close()

                    oDocument = Nothing

                End If
            Next oFile

            If Not (oWord Is Nothing) Then
                'Close Microsoft Word
                lReturn = CType(CloseWord(oWord, lWordHandle, False), gPMConstants.PMEReturnCode)

                If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    bWordOpen = False
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'AR20050411 - PN20013 Release objects
            oWord = Nothing
            oFolder = Nothing
            oFSO = Nothing

            sClient = FileSystem.Dir(v_sClientDir & "\*.HTM", FileAttribute.Normal)

            If sClient <> "" Then

                'Copy parent file to ZipTemp.
                FileSystem.Rename(v_sClientDir & "\" & sClient, v_sZipDir & "\" & sClient)

                'Check for dependencies and copy them to the temp zip directory if they exist.
                sParentFile = v_sClientDir & "\" & sClient
                sParentFile = sParentFile.Substring(0, sParentFile.Length - 4)

                sDependencyDir = sParentFile & "_files"
                If Directory.Exists(sDependencyDir) Then
                    sDepDirName = sClient.Substring(0, sClient.Length - 4) & "_files"
                    If Not Directory.Exists(v_sZipDir & "\" & sDepDirName) Then
                        Directory.CreateDirectory(v_sZipDir & "\" & sDepDirName)
                    End If
                    sDependency = FileSystem.Dir(sDependencyDir & "\*.*", FileAttribute.Normal)
                    Do While (sDependency <> "")
                        FileSystem.Rename(sDependencyDir & "\" & sDependency, v_sZipDir & "\" & sDepDirName & "\" & sDependency)
                        sDependency = FileSystem.Dir()
                    Loop
                    Directory.Delete(sDependencyDir)

                End If

                lShortOldFileNumber = (v_lOldFileNumber - ((v_lOldFileNumber \ 500) * 500))
                lShortNewFileNumber = (v_lNewFileNumber - ((v_lNewFileNumber \ 500) * 500))

                m_lReturn = CType(UpdateTemplateNumberAndDependencies(v_sZipDir, lShortOldFileNumber, lShortNewFileNumber), gPMConstants.PMEReturnCode)

            End If

            Return result

        Catch excep As System.Exception



            'AR20050411 - PN20013 Release objects

            If Not (oWord Is Nothing) Then
                If bWordOpen Then
                    CloseWord(oWord, lWordHandle)
                End If
                oWord = Nothing
            End If


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyFilesToZipTemp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFilesToZipTemp", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function UpdateTemplateNumberAndDependencies(ByVal v_sPath As String, ByVal v_lOldId As Integer, ByVal v_lNewId As Integer) As Integer
        Dim result As Integer = 0
        Dim sOldClient, sNewClient, sParentFile, sDependency, sDependencyDir, sNewDependencyDir As String
        Dim iFileNumIn, iFileNumOut As Integer
        Dim sTempFile, sCurrentLine As String
        Dim iPos As Integer
        Dim sReplacement, sXML_ListFile As String


        Dim sOldDocRef As String = "Doc%20" & v_lOldId


        result = gPMConstants.PMEReturnCode.PMTrue

        sOldClient = v_sPath & "\Doc " & CStr(v_lOldId) & ".HTM"
        sNewClient = v_sPath & "\Doc " & CStr(v_lNewId) & ".HTM"

        FileSystem.Rename(sOldClient, sNewClient)

        'Check for dependencies and rename directory.
        sParentFile = sOldClient.Substring(0, sOldClient.Length - 4)
        sDependencyDir = sParentFile & "_files"

        If Directory.Exists(sDependencyDir) Then
            'Create directory of correct name.
            sNewDependencyDir = sNewClient.Substring(0, sNewClient.Length - 4) & "_files"
            Directory.CreateDirectory(sNewDependencyDir)
            'Move all dependencies to correct directory.
            sDependency = FileSystem.Dir(sDependencyDir & "\*.*", FileAttribute.Normal)
            Do While (sDependency <> "")
                FileSystem.Rename(sDependencyDir & "\" & sDependency, sNewDependencyDir & "\" & sDependency)
                sDependency = FileSystem.Dir()
            Loop
            'Remove original directory.
            Directory.Delete(sDependencyDir)

            'Analyse template source and replace references to old dependency directory with new.
            sTempFile = sNewClient.Substring(0, sNewClient.Length - 3) & "tmp"

            sReplacement = sOldDocRef.Substring(0, sOldDocRef.Length - 1) & CStr(v_lNewId)

            iFileNumIn = FileSystem.FreeFile()
            FileSystem.FileOpen(iFileNumIn, sNewClient, OpenMode.Input)

            iFileNumOut = FileSystem.FreeFile()
            FileSystem.FileOpen(iFileNumOut, sTempFile, OpenMode.Output)

            Do While Not FileSystem.EOF(iFileNumIn)
                sCurrentLine = FileSystem.LineInput(iFileNumIn)
                iPos = (sCurrentLine.IndexOf(sOldDocRef) + 1)
                If iPos > 0 Then
                    sCurrentLine = sCurrentLine.Substring(0, iPos + 5) & CStr(v_lNewId) & Mid(sCurrentLine, iPos + (sOldDocRef.Length))
                End If

                FileSystem.PrintLine(iFileNumOut, sCurrentLine)

            Loop

            FileSystem.FileClose(iFileNumIn)
            FileSystem.FileClose(iFileNumOut)

            File.Delete(sNewClient)
            File.Copy(sTempFile, sNewClient)
            File.Delete(sTempFile)

            'Now do xml list file.
            sXML_ListFile = sNewDependencyDir & "\filelist.xml"
            sTempFile = sXML_ListFile.Substring(0, sXML_ListFile.Length - 3) & "tmp"

            If FileSystem.Dir(sXML_ListFile, FileAttribute.Normal) <> "" Then

                iFileNumIn = FileSystem.FreeFile()
                FileSystem.FileOpen(iFileNumIn, sXML_ListFile, OpenMode.Input)

                iFileNumOut = FileSystem.FreeFile()
                FileSystem.FileOpen(iFileNumOut, sTempFile, OpenMode.Output)

                Do While Not FileSystem.EOF(iFileNumIn)
                    sCurrentLine = FileSystem.LineInput(iFileNumIn)
                    iPos = (sCurrentLine.IndexOf(sOldDocRef) + 1)
                    If iPos > 0 Then
                        sCurrentLine = sCurrentLine.Substring(0, iPos + 5) & CStr(v_lNewId) & Mid(sCurrentLine, iPos + (sOldDocRef.Length))
                    End If

                    FileSystem.PrintLine(iFileNumOut, sCurrentLine)

                Loop

                FileSystem.FileClose(iFileNumIn)
                FileSystem.FileClose(iFileNumOut)

                File.Delete(sXML_ListFile)
                File.Copy(sTempFile, sXML_ListFile)
                File.Delete(sTempFile)
            End If

        End If

        Return result

    End Function

    Private Function GetAndCreatePath(ByRef r_sPath As String, ByVal v_lSlotNumber As Integer, ByVal v_lFileNumber As Integer) As Integer

        Dim result As Integer = 0
        Dim sTemp, sText As String
        Dim lTemp, lTemp2 As Integer
        Dim sServerDir As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        lTemp = v_lFileNumber \ 500

        lTemp2 = v_lFileNumber - (lTemp * 500)

        sText = "Policy Text Files"

        m_lReturn = CType(GetDocumentDirectory(sServerDir), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        r_sPath = sServerDir

        'Make sure the directory's there
        sTemp = FileSystem.Dir(r_sPath, FileAttribute.Directory)
        If sTemp = "" Then
            Directory.CreateDirectory(r_sPath)
        End If

        r_sPath = r_sPath & "\" & sText

        'Make sure the directory's there
        sTemp = FileSystem.Dir(r_sPath, FileAttribute.Directory)
        If sTemp = "" Then
            Directory.CreateDirectory(r_sPath)
        End If

        r_sPath = r_sPath & "\Slot " & CStr(v_lSlotNumber)

        'Make sure the directory's there
        sTemp = FileSystem.Dir(r_sPath, FileAttribute.Directory)
        If sTemp = "" Then
            Directory.CreateDirectory(r_sPath)
        End If

        r_sPath = r_sPath & "\" & StringsHelper.Format(lTemp, "000")

        'Make sure the directory's there
        sTemp = FileSystem.Dir(r_sPath, FileAttribute.Directory)
        If sTemp = "" Then
            Directory.CreateDirectory(r_sPath)
        End If

        r_sPath = r_sPath & "\" & StringsHelper.Format(lTemp2, "000") & ".zip"

        Return result

    End Function

    Private Function Zip(ByRef sPath As String) As Integer

        Dim result As Integer = 0
        Dim iTemp As Integer
        Dim sFileIn, sFileOut As String
        'KN (CMG) Start 171002
        Dim sParentFile, sDependencyDir, sDependency As String
        'KN (CMG) End 171002



        sFileIn = sPath.Substring(0, sPath.Length - 3) & "HTM"
        sFileOut = sPath

        iTemp = g_oZipper.ZipFile(sFileIn:=sFileIn, sFileOut:=sFileOut)

        If Not iTemp Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'KN (CMG) Start 171002
        'Enable zipping up of multiple files to include HTML dependencies.
        'Check for dependencies and add them to the zip file if they exist.
        sParentFile = sFileIn.Substring(0, sPath.Length - 4)

        sDependencyDir = sParentFile & "_files"
        If Directory.Exists(sDependencyDir) Then
            sDependency = FileSystem.Dir(sDependencyDir & "\*.*", FileAttribute.Normal)
            Do While (sDependency <> "")
                iTemp = g_oZipper.addFileToZIP(sFileOut, sDependencyDir & "\" & sDependency)
                File.Delete(sDependencyDir & "\" & sDependency)
                sDependency = FileSystem.Dir()
            Loop
            Directory.Delete(sDependencyDir)

        End If
        'KN (CMG) End 171002

        File.Delete(sFileIn)

        Return result

    End Function


    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_sUserName = .UserName
            End With

            g_oZipper = New bPMZipper.Business()

            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRTextFile.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If Not (g_oZipper Is Nothing) Then
                    g_oZipper = Nothing
                End If
                If Not (m_oBusiness Is Nothing) Then
                    m_oBusiness = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If

            End If
        End If
        Me.disposedValue = True
    End Sub


    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
