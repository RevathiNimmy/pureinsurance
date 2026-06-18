Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports System.Windows.Forms
Imports SharedFiles
Imports System.IO
Imports Artinsoft.VB6

<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 13/05/1998
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"
    Private Const DOCDefaultImageViewer As String = "Default Image Viewer"
    Private Const DOCOptionsSection As String = "Options"
    Private Const REGISTRY_USER As Integer = 1
    Private Const SE_ERR_FNF As Integer = 2
    Private Const SE_ERR_PNF As Integer = 3
    Private Const SE_ERR_ACCESSDENIED As Integer = 5
    Private Const SE_ERR_OOM As Integer = 8
    Private Const SE_ERR_DLLNOTFOUND As Integer = 32
    Private Const SE_ERR_SHARE As Integer = 26
    Private Const SE_ERR_ASSOCINCOMPLETE As Integer = 27
    Private Const SE_ERR_DDETIMEOUT As Integer = 28
    Private Const SE_ERR_DDEFAIL As Integer = 29
    Private Const SE_ERR_DDEBUSY As Integer = 30
    Private Const SE_ERR_NOASSOC As Integer = 31
    Private Const ERROR_BAD_FORMAT As Integer = 11
    Private oViewRTF As frmChildRTF
    Private oViewOFF As frmChildOFF
    Private oViewTIF As frmChildTIF
    Dim Err_Msg As String = ""
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' instace of the zipper class
    Private m_oZipper As bPMZipper.Business  ' bSIRZipper.Zipper

    Dim m_lReturn As Integer

    Dim bZipSuccess As Boolean = False
    Dim unzipTargetFolder As String = ""

    Dim sNewFileZip As String
    Dim sBackupFile As String
    ' PRIVATE Data Members (End)

    Private Declare Function ShellExecute Lib "shell32.dll" Alias "ShellExecuteA" (ByVal hwnd As Integer, ByVal lpOperation As String, ByVal lpFile As String, ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowCmd As Integer) As Integer

    Private Declare Function GetDesktopWindow Lib "user32" () As Integer

    ' PUBLIC Property Procedures (Begin)
    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Description: Initialise
    '
    ' Edit History  :
    ' RAM20030506   : Added the 4 Optional Parameters
    '                   a) v_vUserID
    '                   b) v_vUserName
    '                   c) v_vUserPassword
    '                   d) v_vSourceID
    ' ***************************************************************** '
    Public Function Initialise(ByRef frmManager As Object, Optional ByVal v_vUserID As Object = Nothing, Optional ByVal v_vUserName As Object = Nothing, Optional ByVal v_vUserPassword As Object = Nothing, Optional ByVal v_vSourceID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Create an instance of the object manager.

            MainModule.g_oObjectManager = New bObjectManager.ObjectManager

            ' Call the initialise method.
            m_lReturn = MainModule.g_oObjectManager.Initialise(sCallingAppName:=ACApp)


            m_oZipper = New bPMZipper.Business()  'bSIRZipper.Zipper()

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLCommon
            eProductFamily = gPMConstants.PMEProductFamily.pmePFDocumaster

            ' get the path of the cache
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCCacheLocationKey, r_sSettingValue:=g_sCachePath, v_sSubKey:="Cache")

            ' If no cache path then
            If g_sCachePath.Trim() = "" Then
                ' Display the message
                MessageBox.Show("The cache directory is currently not configured." & _
                                Environment.NewLine & _
                                "Please set one from Options in Manager.", "Cache Directory", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                ' DN 13/12/00 - Set to invalid request help fire up Options Manager
                result = gPMConstants.PMEReturnCode.PMInvalidRequest
            End If

            ' Set the global variable if passed in

            If Not Information.IsNothing(v_vUserID) Then
                ' RDC 08122004 -1000 indicates DocViewer called by BriefcaseViewer

                g_iUserID = CInt(v_vUserID)
            End If


            If Not Information.IsNothing(v_vUserName) Then

                g_sUsername = CStr(v_vUserName)
            End If


            If Not Information.IsNothing(v_vUserPassword) Then

                g_sPassword.Value = CStr(v_vUserPassword)
            End If


            If Not Information.IsNothing(v_vSourceID) Then

                g_iSourceID = CInt(v_vSourceID)
            End If

            ' RDC 08122004 moved from before point where globals are set
            ' and Show the parent MDI Form


            If g_frmManager Is Nothing Then
                g_frmManager = frmManager
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise process failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ViewDocument
    '
    ' Description: ViewDocument
    '
    '
    ' History:          - BB. Created
    '          5/jun/98 - CF. Converted DocumentNumber to DocumentKey
    '                     CF. Changed ByVal to ByRef
    '                     CF. Added v_sParents
    '         16/jun/98 - CF. Changed call to caching function
    '         12/Dec/02 - KR. send BMP/JPG files to TIF Form
    '         RAM20030428   : Added the optional v_vAdditionalDataArray Parameter
    '                         Ref. NRMA Project Changes. Document Issuance
    '                         Note : If the document template id is supplied, within the
    '                                additional data array, then we need to show the
    '                                Sirius Document Template viewer in read-only mode.
    ' ***************************************************************** '
    Public Function ViewDocument(ByRef v_sDocumentKey As String, ByRef v_sDocumentName As String, ByRef v_sParents As String, ByRef v_vFileArray() As Object, ByRef v_bZipped As Boolean, Optional ByRef v_bShowOnly As Boolean = False, Optional ByRef v_vAdditionalDataArray(,) As Object = Nothing, Optional ByRef v_bAllowCopyPaste As Boolean = False) As Integer

        Dim result As Integer = 0
        ' Const SIROPTSiriusViewInDocumaster As Integer = 45

        Dim lFirstFileIndex, lLastFileIndex As Integer
        Dim iFileType As Integer
        Dim lReturn As Integer

        ' File path and name
        Dim sFilePath As String = ""

        ' looooooooopy loop

        ' The new filename that will be returned after caching
        Dim sNewFile As String = ""

        Dim oSplash As iDOCSplash.Interface_Renamed

        Dim bHideSplash As Boolean
        Dim sRegValue As String = ""

        Dim sValue As String = ""
        Dim m_bSiriusViewInDocumaster As Boolean

        ' CTAF 20030711
        Dim bSiriusInstalled As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'added to create object of frmParentMDI
            If objfrmParentMDI Is Nothing Then
                objfrmParentMDI = New frmParentMDI
            End If
            ' enable the common buttons
            lReturn = objfrmParentMDI.EnableCommon()

            ' unhide view form if hidden
            ' If objfrmParentMDI.WindowState = FormWindowState.Maximized Then
            If v_bShowOnly Then
                objfrmParentMDI.Show()
            End If

            If v_sDocumentName.Trim() <> "" Then
                ' Get an instance of the SPLASH object
                'oSplash = System.Runtime.InteropServices.Marshal.GetActiveObject("iDOCSplash.Interface")
                If oSplash Is Nothing Then
                    oSplash = New iDOCSplash.Interface_Renamed
                End If

                ' Show the splash
                m_lReturn = oSplash.Show(DOCSplash_Message, "Retrieving " & v_sDocumentName)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                bHideSplash = True

            Else

                bHideSplash = False

            End If

            'if showonly passed as true, then just show and go
            If True Then
                If v_bShowOnly Then
                    objfrmParentMDI.Show()
                    ' Return result
                    Exit Function
                End If
            End If

            If v_sDocumentName.Trim() = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Blank Document Name passed", vApp:=ACApp, vClass:=ACClass, vMethod:="ViewDocument")
                Return result
            End If

            If Not Information.IsArray(v_vFileArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No File Array passed", vApp:=ACApp, vClass:=ACClass, vMethod:="ViewDocument")
                Return result
            End If


            m_lReturn = GetRegistryValue(DOCDefaultImageViewer, DOCOptionsSection, sRegValue, REGISTRY_USER)

            ' File Array should contain a single TXT or RTF file
            ' or a single multipage TIF or several single page TIFs
            ' If TIFs pass the whole array to the Child Form to deal with

            lFirstFileIndex = v_vFileArray.GetLowerBound(0)
            lLastFileIndex = v_vFileArray.GetUpperBound(0)


            sFilePath = CStr(v_vFileArray(lFirstFileIndex)).Trim()

            m_lReturn = GetFileExtension(v_sFileName:=sFilePath, r_iFileType:=iFileType)

            ' Check that each file is cached
            For iLoop1 As Integer = lFirstFileIndex To lLastFileIndex

                ' Cache the file (if it needs it)

                m_lReturn = CacheFile(oZipper:=m_oZipper, sFilename:=CStr(v_vFileArray(iLoop1)), sNewFilename:=sNewFile, sCachePath:=g_sCachePath, bZipped:=v_bZipped)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Update the array with the path of the local cached file

                v_vFileArray(iLoop1) = sNewFile

                'sGetFileCacheName = sNewFile

            Next iLoop1
            ' And thats the caching taken care of...

            If bHideSplash Then
                m_lReturn = oSplash.Hide()
            End If

            ' Maximise the viewer if its minimized
            If objfrmParentMDI.WindowState = FormWindowState.Minimized Then
                objfrmParentMDI.WindowState = FormWindowState.Maximized
            End If

            ' CTAF 20030711 - We should not be going to the Sirius tables without
            '                 checking if Sirius is installed first...
            ' RDC 08122004 BriefcaseViewer should not do this
            If g_iUserID <> USER_IS_BRIEFCASE Then
                m_lReturn = IsSiriusInstalled(r_bSiriusInstalled:=bSiriusInstalled)
            End If

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                ' Default to not installed if it failed
                bSiriusInstalled = False
            End If

            If bSiriusInstalled Then

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' RAM20030428   :   1. Also check if the options is set in Product Options
                '                       (SIROPTSiriusViewInDocumaster)
                '                   2. Check if the document Template ID is sent in
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' Get SiriusViewInDocumaster product option
                ' RDC 08122004 BriefcaseViewer should not do this
                If g_iUserID <> USER_IS_BRIEFCASE Then
                    m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTSiriusViewInDocumaster, v_vBranch:=g_iSourceID, r_vUnderwriting:=sValue)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get SiriusViewInDocumaster product option", vApp:=ACApp, vClass:=ACClass, vMethod:="ViewDocument")
                        Return result
                    End If
                End If

                m_bSiriusViewInDocumaster = (sValue = "1")

            Else

                ' No Sirius so no we don't want to view in Sirius
                m_bSiriusViewInDocumaster = False

            End If

            If m_bSiriusViewInDocumaster Then


                If Not Information.IsNothing(v_vAdditionalDataArray) Then

                    ' sNewFile  : The path of the file to open

                    m_lReturn = ViewSiriusDocumentTemplate(v_vFileDetailsArray:=v_vAdditionalDataArray)

                    ' We finished viewing it.
                    ' So exit fucntion
                    Return result
                End If
            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20030428   : END
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Select Case iFileType
                ' Could be an array of single page TIFs so
                ' pass whole array
                Case ACFileTypeTIF, ACFileTypeJPG, ACFileTypeBMP, ACFileTypePNG
                    Dim fInfo As New FileInfo(sNewFile)
                    If sRegValue = "Y" Then
                        ' PW131003 - CQ1413 - change API call so that it works
                        m_lReturn = ShellExecute(hwnd:=objfrmParentMDI.Handle.ToInt32(), lpOperation:="open", lpFile:=sNewFile, lpParameters:=Nothing, lpDirectory:=".", nShowCmd:=1)
                    Else
                        If iFileType = ACFileTypeTIF Then
                            sNewFile = v_vFileArray(0).ToString().Substring(0, v_vFileArray(0).ToString().LastIndexOf(".") + 1) & "JPG"
                            If Not FileExists(sNewFile) Then
                                Dim imgJPEG As Bitmap = New Bitmap(v_vFileArray(0).ToString())
                                imgJPEG.Save(sNewFile, System.Drawing.Imaging.ImageFormat.Jpeg)
                                imgJPEG = Nothing
                            End If
                        End If
                        m_lReturn = ViewOLEDocument(v_sDocumentKey:=v_sDocumentKey, v_sDocumentName:=v_sDocumentName, v_sParents:=v_sParents, v_iFileType:=iFileType, v_sFilePath:=sNewFile)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                Case ACFileTypeTXT
                    ' We should only have one  RTF or TXT file

                    If lFirstFileIndex <> lLastFileIndex Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="More than one TXT File passed", vApp:=ACApp, vClass:=ACClass, vMethod:="ViewDocument")
                        Return result
                    End If
                    Dim fInfo As New FileInfo(sNewFile)
                    bZipSuccess = CheckZipFileStatus(sNewFile)

                    If Not bZipSuccess Then

                        m_lReturn = ViewRTFDocument(v_sDocumentKey:=v_sDocumentKey, v_sDocumentName:=v_sDocumentName, v_sParents:=v_sParents, v_iFileType:=iFileType, v_sFilePath:=sNewFile)
                        fInfo = New FileInfo(sNewFileZip)
                        fInfo.Delete()
                        fInfo = Nothing
                        Return m_lReturn
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        Dim ofiles As String() = Directory.GetFiles(unzipTargetFolder + "\temp")
                        Dim sTarget As String = ""
                        If ofiles IsNot Nothing Then
                            sTarget = ofiles(0).ToString
                        End If
                        m_lReturn = ViewRTFDocument(v_sDocumentKey:=v_sDocumentKey, v_sDocumentName:=v_sDocumentName, v_sParents:=v_sParents, v_iFileType:=iFileType, v_sFilePath:=sTarget)
                    End If
                    fInfo = New FileInfo(sNewFileZip)
                    fInfo.Delete()
                    fInfo = New FileInfo(sBackupFile)
                    fInfo.Delete()
                    fInfo = Nothing

                Case ACFileTypeWRD, ACFileTypeEXL

                    Dim fInfo As New FileInfo(sNewFile)
                    bZipSuccess = CheckZipFileStatus(sNewFile)
                    If Not bZipSuccess Then

                        m_lReturn = ViewWRDDocument(v_sDocumentKey:=v_sDocumentKey, v_sDocumentName:=v_sDocumentName, v_sParents:=v_sParents, v_iFileType:=iFileType, v_sFilePath:=sNewFile, v_bAllowCopyPaste:=v_bAllowCopyPaste)
                        fInfo = New FileInfo(sNewFileZip)
                        fInfo.Delete()
                        fInfo = Nothing
                        Return m_lReturn
                    Else
                        Dim ofiles As String() = Directory.GetFiles(unzipTargetFolder + "\temp")
                        Dim sTarget As String = ""
                        If ofiles IsNot Nothing Then
                            sTarget = ofiles(0).ToString
                        End If
                        m_lReturn = ViewWRDDocument(v_sDocumentKey:=v_sDocumentKey, v_sDocumentName:=v_sDocumentName, v_sParents:=v_sParents, v_iFileType:=iFileType, _
                                               v_sFilePath:=sTarget, v_bAllowCopyPaste:=v_bAllowCopyPaste)
                    End If

                    fInfo = New FileInfo(sNewFileZip)
                    fInfo.Delete()
                    fInfo = New FileInfo(sBackupFile)
                    fInfo.Delete()
                    fInfo = Nothing

                Case ACFileTypeEML

                    Dim fInfo As New FileInfo(sNewFile)
                    bZipSuccess = CheckZipFileStatus(sNewFile)

                    If Not bZipSuccess Then
                        m_lReturn = ShellExecute(hwnd:=objfrmParentMDI.Handle.ToInt32(), lpOperation:="open", lpFile:=sNewFile, lpParameters:=Nothing, lpDirectory:="C:\", nShowCmd:=1)
                    Else
                        Dim ofiles As String() = Directory.GetFiles(unzipTargetFolder + "\temp")
                        Dim sTarget As String = ""
                        If ofiles IsNot Nothing Then
                            sTarget = ofiles(0).ToString
                        End If
                        m_lReturn = ShellExecute(hwnd:=objfrmParentMDI.Handle.ToInt32(), lpOperation:="open", lpFile:=sTarget, lpParameters:=Nothing, lpDirectory:="C:\", nShowCmd:=1)
                        fInfo = New FileInfo(sNewFileZip)
                        fInfo.Delete()
                        fInfo = New FileInfo(sBackupFile)
                        fInfo.Delete()
                        fInfo = Nothing
                    End If


                    If m_lReturn < 32 Then
                        'There was an error
                        Select Case m_lReturn
                            Case SE_ERR_FNF
                                Err_Msg = "File not found"
                            Case SE_ERR_PNF
                                Err_Msg = "Path not found"
                            Case SE_ERR_ACCESSDENIED
                                Err_Msg = "Access denied"
                            Case SE_ERR_OOM
                                Err_Msg = "Out of memory"
                            Case SE_ERR_DLLNOTFOUND
                                Err_Msg = "DLL not found"
                            Case SE_ERR_SHARE
                                Err_Msg = "A sharing violation occurred"
                            Case SE_ERR_ASSOCINCOMPLETE
                                Err_Msg = "Incomplete or invalid file association"
                            Case SE_ERR_DDETIMEOUT
                                Err_Msg = "DDE Time out"
                            Case SE_ERR_DDEFAIL
                                Err_Msg = "DDE transaction failed"
                            Case SE_ERR_DDEBUSY
                                Err_Msg = "DDE busy"
                            Case SE_ERR_NOASSOC
                                Err_Msg = "No association for file extension"
                            Case ERROR_BAD_FORMAT
                                Err_Msg = "Invalid EXE file or error in EXE image"
                            Case Else
                                Err_Msg = "Unknown error"
                        End Select

                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=Err_Msg, vApp:=ACApp, vClass:=ACClass, vMethod:="ViewDocument")

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case Else
                    ' View it

                    If sRegValue = "Y" Then

                        m_lReturn = ShellExecute(hwnd:=objfrmParentMDI.Handle.ToInt32(), lpOperation:="open", lpFile:=sNewFile, lpParameters:=Nothing, lpDirectory:=".", nShowCmd:=1)

                    Else
                        Dim fInfo As New FileInfo(sNewFile)
                        bZipSuccess = CheckZipFileStatus(sNewFile)

                        If Not bZipSuccess Then

                            m_lReturn = ViewOLEDocument(v_sDocumentKey:=v_sDocumentKey, v_sDocumentName:=v_sDocumentName, v_sParents:=v_sParents, v_iFileType:=iFileType, v_sFilePath:=sNewFile)
                            fInfo = New FileInfo(sNewFileZip)
                            fInfo.Delete()
                            fInfo = Nothing
                            Return m_lReturn
                        Else
                            Dim ofiles As String() = Directory.GetFiles(unzipTargetFolder + "\temp")
                            Dim sTarget As String = ""
                            If ofiles IsNot Nothing Then
                                sTarget = ofiles(0).ToString
                            End If
                            m_lReturn = ViewOLEDocument(v_sDocumentKey:=v_sDocumentKey, v_sDocumentName:=v_sDocumentName, v_sParents:=v_sParents, v_iFileType:=iFileType, v_sFilePath:=sTarget)
                        End If

                        fInfo = New FileInfo(sNewFileZip)
                        fInfo.Delete()
                        fInfo = New FileInfo(sBackupFile)
                        Dim attributes As FileAttributes
                        attributes = File.GetAttributes(sBackupFile)
                        attributes = RemoveAttribute(attributes, FileAttributes.ReadOnly)
                        File.SetAttributes(sBackupFile, attributes)

                        fInfo.Delete()
                        fInfo = Nothing
                        End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

            End Select
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ViewDocument process failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ViewDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate
    '
    ' Description: Terminate
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                m_oZipper = Nothing
            End If
            If Not objfrmParentMDI Is Nothing Then
                objfrmParentMDI.UnloadForm = True
                objfrmParentMDI.Close()
                objfrmParentMDI = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: ViewTIFDocument
    '
    ' Description: ViewTIFDocument
    '
    ' ***************************************************************** '
    Public Function ViewTIFDocument(ByVal v_sDocumentKey As String, ByVal v_sDocumentName As String, ByVal v_sParents As String, ByVal v_vFileArray As Object) As Integer

        Dim result As Integer = 0


        Dim sErrMsg As New StringBuilder

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            oViewTIF = New frmChildTIF

            m_lReturn = oViewTIF.DocumentOpen(v_sDocumentKey:=v_sDocumentKey, v_sDocumentName:=v_sDocumentName, v_sParents:=v_sParents, v_vFileArray:=v_vFileArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Corrupt or missing
                sErrMsg = New StringBuilder("Unable to load tif image." & Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & _
                          Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & _
                          "Error loading the file(s) :" & Strings.Chr(10).ToString() & Strings.Chr(13).ToString())

                ' Add the list of files trying to be loaded
                For Each v_vFileArray_item As Object In v_vFileArray

                    sErrMsg.Append(CStr(v_vFileArray_item) & Strings.Chr(10).ToString() & Strings.Chr(13).ToString())
                Next v_vFileArray_item

                ' Log Error.
                LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sErrMsg.ToString(), vApp:=ACApp, vClass:=ACClass, vMethod:="ViewTIFDocument", excep:=New Exception(Information.Err().Description))

                ' unload the form
                oViewTIF.Close()

                oViewTIF = Nothing


                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            oViewTIF.Show()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Corrupt or missing
            sErrMsg = New StringBuilder("Unable to load tif image." & Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & _
                      Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & _
                      "Error loading the file(s) :" & Strings.Chr(10).ToString() & Strings.Chr(13).ToString())

            ' Add the list of files trying to be loaded
            For iLoop1 As Integer = 0 To v_vFileArray.GetUpperBound(0)

                sErrMsg.Append(CStr(v_vFileArray(iLoop1)) & Strings.Chr(10).ToString() & Strings.Chr(13).ToString())
            Next iLoop1

            ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sErrMsg.ToString(), vApp:=ACApp, vClass:=ACClass, vMethod:="ViewTIFDocument", excep:=excep)

            ' unload the form
            oViewTIF.Close()


            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ViewRTFDocument
    '
    ' Description: ViewRTFDocument
    '
    ' ***************************************************************** '
    Private Function ViewRTFDocument(ByVal v_sDocumentKey As String, ByVal v_sDocumentName As String, ByVal v_sParents As String, ByVal v_iFileType As Integer, ByVal v_sFilePath As Object) As Integer

        Dim result As Integer = 0





        result = gPMConstants.PMEReturnCode.PMTrue


        oViewRTF = New frmChildRTF
        'Load(oViewRTF)



        m_lReturn = oViewRTF.DocumentOpen(v_sDocumentKey:=v_sDocumentKey, v_sDocumentName:=v_sDocumentName, v_sParents:=v_sParents, v_iFileType:=v_iFileType, v_sFilePath:=CStr(v_sFilePath))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            oViewRTF.Close()
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        If objfrmParentMDI.Visible = False Then
            objfrmParentMDI.Show()
        End If

        oViewRTF.Show()
        ' Check that there isnt a form loaded with the document number



        Return result

    End Function
    ' ***************************************************************** '
    ' Name: ViewWRDDocument
    '
    ' Description: ViewWRDDocument
    '
    ' ***************************************************************** '
    Private Function ViewWRDDocument(ByVal v_sDocumentKey As String, ByVal v_sDocumentName As String, ByVal v_sParents As String, ByVal v_iFileType As Integer, ByVal v_sFilePath As Object, ByVal v_bAllowCopyPaste As Boolean) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        oViewOFF = New frmChildOFF
        oViewOFF.AllowCopyPaste = v_bAllowCopyPaste
        oViewOFF.Initialise()
        ' RDC 22062005


        m_lReturn = oViewOFF.DocumentOpen(v_sDocumentKey:=v_sDocumentKey, v_sDocumentName:=v_sDocumentName, v_sParents:=v_sParents, v_iFileType:=v_iFileType, v_sFilePath:=v_sFilePath)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            ' In case of failed to open document as a word file, here a workaround is provided - open that document as a pdf file, will be revert if
            ' get any proper solution
            If m_lReturn = 5 Then

                oViewOFF.MdiParent.Close()

                Dim oViewOLE As New frmBrowser

                ' enable the common buttons
                m_lReturn = objfrmParentMDI.EnableCommon()
                If Not IO.File.Exists(Left(v_sFilePath, InStr(v_sFilePath, ".") - 1) & ".pdf") Then
                    m_lReturn = ConvertDocumentUsingSiriusDocumentUtility(v_sFilePath, Left(v_sFilePath, InStr(v_sFilePath, ".") - 1) & ".pdf")
                End If
                v_sFilePath = Left(v_sFilePath, InStr(v_sFilePath, ".") - 1) & ".pdf"

                m_lReturn = oViewOLE.DocumentOpen(v_sDocumentKey:=v_sDocumentKey, v_sDocumentName:=v_sDocumentName, v_sParents:=v_sParents, v_iFileType:=v_iFileType, v_sFilePath:=CStr(v_sFilePath))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    oViewOLE.MdiParent.Close()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oViewOLE.Show()

                Return result

            Else
                oViewOFF.MdiParent.Close()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If


        oViewOFF.Show()

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ViewRTFDocument
    '
    ' Description: ViewRTFDocument
    '
    ' ***************************************************************** '
    Private Function ViewOLEDocument(ByVal v_sDocumentKey As String, ByVal v_sDocumentName As String, ByVal v_sParents As String, ByVal v_iFileType As Integer, ByVal v_sFilePath As Object) As Integer

        Dim result As Integer = 0
        Dim oViewOLE As New frmBrowser
        Dim lReturn As Integer




        result = gPMConstants.PMEReturnCode.PMTrue

        ' enable the common buttons
        lReturn = objfrmParentMDI.EnableCommon()


        m_lReturn = oViewOLE.DocumentOpen(v_sDocumentKey:=v_sDocumentKey, v_sDocumentName:=v_sDocumentName, v_sParents:=v_sParents, v_iFileType:=v_iFileType, v_sFilePath:=CStr(v_sFilePath))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            oViewOLE.Close()
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        oViewOLE.Show()
        'oViewOLE.Close()

        Return result

    End Function




    ' ***************************************************************** '
    ' Name: GetFileExtension
    '
    ' Description: GetFileExtension
    '
    ' ***************************************************************** '
    Private Function GetFileExtension(ByVal v_sFileName As String, ByRef r_iFileType As Integer) As Integer

        Dim result As Integer = 0
        Dim sFileName, sFileExtension As String
        Dim lExtLen As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        sFileName = v_sFileName.Trim()

        lExtLen = 0

        ' Find the position of the last dot in the filename
        ' and get the length of the file extension from that
        For lPos As Integer = sFileName.Length To 1 Step -1
            If Mid(sFileName, lPos, 1) = "." Then
                lExtLen = sFileName.Length - lPos
                Exit For
            End If
        Next

        ' Get trailing chars up to dot and convert to uppercase
        If lExtLen > 0 Then
            sFileExtension = sFileName.Substring(sFileName.Length - lExtLen).ToUpper()
        Else
            sFileExtension = ""
        End If

        ' Set returned file type
        Select Case sFileExtension
            Case "TIF", "TIFF"
                r_iFileType = ACFileTypeTIF
            Case "RTF"
                r_iFileType = ACFileTypeRTF
            Case "TXT", "TEXT", "ASCI"
                r_iFileType = ACFileTypeTXT
            Case "DOC", "DOCX", "DOT", "DOTX", "ASC", "ANS", "MCW", "WPS" 'SOB 01/06/99 WORD FILES
                r_iFileType = ACFileTypeWRD
            Case "XLS", "XLSX", "XLT", "XLS", "CSV", "WK1", "WK2", "WK3", "WK4", "WQ1", "PRN", "DIF", "SLK", "XLA", "TAB" 'SOB 01/06/99 EXCEL Files
                r_iFileType = ACFileTypeEXL
            Case "PPT", "PPTX", "POT", "POTX", "PPS", "PPSX", "PPA" 'SOB 01/06/99 Power Point Files
                r_iFileType = ACFileTypePWP
            Case "MDB", "ADP", "MDW", "MDA", "MDE", "ADE", "DBF", "DB" 'SOB 01/06/99 Ms Access Files
                r_iFileType = ACFileTypeACC
            Case "HTM", "HTML", "SHTM", "SHTML", "STM", "ASP", "HTT", "CSS", "CFML", "XML" 'SOB 01/06/99 IE, Netscape Files
                r_iFileType = ACFileTypeHTM
            Case "GIF", "GIFF"
                r_iFileType = ACFileTypeGIF 'SOB 01/06/99 GIF Files
            Case "JPEG", "JPG"
                r_iFileType = ACFileTypeJPG
            Case "EML", "OFT", "MSG", "EML" 'SOB 01/06/99 E-Mail Doc
                r_iFileType = ACFileTypeEML
            Case "PDF"
                r_iFileType = ACFileTypePDF 'SOB 01/06/99 Adobe Accrobat Files
            Case "HLP"
                r_iFileType = ACFileTypeHLP 'SOB 01/06/99 Help Files
            Case "ZIP", "GZ"
                r_iFileType = ACFileTypeZIP 'SOB 01/06/99 ZIP Files
            Case "BMP"
                r_iFileType = ACFileTypeBMP 'KIR 17/12/02 Bitmap files
            Case Else
                r_iFileType = ACFileTypeUnknown
        End Select

        Return result

    End Function

    ' PRIVATE Methods (End)
    Public Sub New()
        MyBase.New()
    End Sub


    ' ***************************************************************** '
    ' Function Name : ViewSiriusDocumentTemplate
    '
    ' Created on    : 2003/04/28
    '
    ' Description   : Function to display Sirius Documents in View Only Mode.
    '
    ' Notes         : 1.Since, the document is already available within DME,
    '                   we don't need to re-merge the fields. we only need
    '                   to show the file in view mode.
    '                 2.The File details are in the v_vFileDetailsArray Param
    '
    ' Edit History  :
    ' RAM20030428   : Created.
    ' ***************************************************************** '
    Private Function ViewSiriusDocumentTemplate(ByVal v_vFileDetailsArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim objDocTemplate As iPMBDocTemplate.Interface_Renamed
        Dim strFileNameToView As String = ""
        Dim lDocumentTemplateID, lDocumentTypeID, lPartyCnt, lInsuranceFolderCnt, lInsuranceFileCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(v_vFileDetailsArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Document Details Missing. v_vFileDetailsArray is Empty.", vApp:=ACApp, vClass:=ACClass, vMethod:="ViewSiriusDocumentTemplate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Step through the array.
            For lRow As Integer = v_vFileDetailsArray.GetLowerBound(1) To v_vFileDetailsArray.GetUpperBound(1)

                Select Case CStr(v_vFileDetailsArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case "DocumentTemplateID"

                        lDocumentTemplateID = CInt(v_vFileDetailsArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case "DocumentTypeID"

                        lDocumentTypeID = CInt(v_vFileDetailsArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case "PartyCnt"

                        lPartyCnt = CInt(v_vFileDetailsArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case "InsuranceFolderCnt"

                        lInsuranceFolderCnt = CInt(v_vFileDetailsArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case "InsuranceFileCnt"

                        lInsuranceFileCnt = CInt(v_vFileDetailsArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case "FileNameToView"

                        strFileNameToView = CStr(v_vFileDetailsArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                End Select
            Next lRow

            objDocTemplate = New iPMBDocTemplate.Interface_Renamed()

            m_lReturn = CType(objDocTemplate, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get an instance of iPMBDocTemplate.Interface.", vApp:=ACApp, vClass:=ACClass, vMethod:="ViewSiriusDocumentTemplate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            With objDocTemplate

                .DocumentTemplateId = lDocumentTemplateID
                .DocumentTypeId = lDocumentTypeID
                .PartyCnt = lPartyCnt
                .InsuranceFolderCnt = lInsuranceFolderCnt
                .InsuranceFileCnt = lInsuranceFileCnt

                ' .DocumentToView = strFileNameToView ' Document To Show
                .Mode = ACViewMode ' Mode which will trigger the View Mode

                m_lReturn = .Start()

            End With

            ' Clear iPMBDocTemplate.Interface
            If objDocTemplate Is Nothing Then
            Else
                objDocTemplate.Dispose()
                objDocTemplate = Nothing
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Clear iPMBDocTemplate.Interface
            If objDocTemplate Is Nothing Then
            Else
                objDocTemplate.Dispose()
                objDocTemplate = Nothing
            End If

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to view Sirius Document.", vApp:=ACApp, vClass:=ACClass, vMethod:="ViewSiriusDocumentTemplate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
            Return result
        End Try
    End Function
    Private Function CheckZipFileStatus(ByVal sNewFile As String) As Boolean

        Dim fInfo As New FileInfo(sNewFile)
        Dim result As Integer = 0
        Dim attributes As FileAttributes
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            unzipTargetFolder = fInfo.DirectoryName

            sNewFileZip = Left(sNewFile, InStrRev(sNewFile, ".") - 1)
            'now rename it to a zip file
            sNewFileZip = sNewFileZip + "_copy.zip"
            fInfo.CopyTo(sNewFileZip, True)

            'rename to a backup file so we can delete it later
            sBackupFile = (unzipTargetFolder + "\" + fInfo.Name + ".backup")
            fInfo = New FileInfo(sBackupFile)
            If fInfo.Exists Then
                fInfo.Delete()
            End If

            fInfo = New FileInfo(sNewFile)
            fInfo.MoveTo(sBackupFile)
            Dim sTarget As String = ""
            Dim sExt As String = ""
            sExt = Path.GetExtension(sNewFile).ToUpper()

            If Not Directory.Exists(unzipTargetFolder + "\temp") Then
                Directory.CreateDirectory(unzipTargetFolder + "\temp")
            End If
            Dim sfiles As String = String.Empty
            For Each sfiles In Directory.GetFiles(unzipTargetFolder + "\temp")

            Next
            If Path.GetExtension(sfiles).ToUpper() = ".PDF" Or Path.GetExtension(sfiles).ToUpper() = ".HTM" Or Path.GetExtension(sfiles).ToUpper() = ".RTF" Then

                fInfo = New FileInfo(sfiles)

                attributes = File.GetAttributes(unzipTargetFolder + "\temp\" + fInfo.Name)

                If (attributes And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then

                    attributes = RemoveAttribute(attributes, FileAttributes.ReadOnly)
                    File.SetAttributes(unzipTargetFolder + "\temp\" + fInfo.Name, attributes)

                End If
            End If

            If Not sfiles = String.Empty Then
                File.Delete(sfiles)
            End If

            If sExt <> ".DOCX" And sExt <> ".XLSX" Then
                bZipSuccess = m_oZipper.UnZipFile(sNewFileZip, unzipTargetFolder + "\temp")
            End If

            If Directory.GetFiles(unzipTargetFolder + "\temp").Length > 0 Then
                For Each sfiles In Directory.GetFiles(unzipTargetFolder + "\temp")
                    File.Delete(sfiles)
                Next
                bZipSuccess = False
            End If

            fInfo = New FileInfo(sNewFileZip)
            attributes = File.GetAttributes(sNewFileZip)
            attributes = RemoveAttribute(attributes, FileAttributes.ReadOnly)
            File.SetAttributes(sNewFileZip, attributes)

            If Not bZipSuccess Then
                sBackupFile = Replace(sBackupFile, ".backup", "")
                fInfo = New FileInfo(sBackupFile)
                If fInfo.Exists Then
                    fInfo.Delete()
                End If
                ''Move file from backup to Original
                fInfo = New FileInfo(sNewFile + ".backup")
                fInfo.MoveTo(sBackupFile)
            End If

            Return bZipSuccess

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckZipFileStatus", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckZipFileStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try

    End Function
    Public Shared Function RemoveAttribute(ByVal attributes As FileAttributes, ByVal attributesToRemove As FileAttributes) As FileAttributes
        Return attributes And (Not attributesToRemove)
    End Function
    Private Shared _DefaultInstance As Interface_Renamed = Nothing
    Public Shared ReadOnly Property DefaultInstance() As Interface_Renamed
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New Interface_Renamed
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class

