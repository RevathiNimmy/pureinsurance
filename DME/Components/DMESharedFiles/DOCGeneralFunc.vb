Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports VB6 = Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Diagnostics
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports Artinsoft.VB6
Imports SharedFiles.iPMFunc
Imports SharedFiles.gPMConstants
Imports SharedFiles
Imports System.Windows.Forms


Public Module DOCGeneralFunc
    ' ***************************************************************** '
    '
    ' DocuMaster general functions module. Contains all of the global
    ' functions that might be useful to various DocuMaster components.
    '
    ' ***************************************************************** '
    ' ***************************************************************** '
    ' Edit History :
    ' JH021198 - moved setting_type declaration from MainModule
    ' and functions getregistryvalue and setregistryvalue + 2
    ' registry constants from iDOCOptions for use by other
    ' components. Also - added function SetComboText for use
    ' when setting text of combo boxes - on scan but
    ' can be used eleswhere.
    '
    ' JH260399 - changed ChangeDOCRegSettings for use
    ' in SwapHistoryRoot in bDOCPMBAPI and for when options
    ' are changed in iDOCPMBAPI
    '
    ' JH200599 remove ValidateSQL as it is now in bPMFunc
    '
    '****************************************************************** '


    ' Constant for the methods to identify which class this is.
    Private Const ACClass As String = "bDOCFunc"

    ' Return values
    Private m_lReturn As Integer

    'Used for win API calls - the stuff after this is copied from a MS Web site
    Private Const BIF_RETURNONLYFSDIRS As Integer = 1
    Private Const BIF_DONTGOBELOWDOMAIN As Integer = 2
    Private Const MAX_PATH As Integer = 260

    ' RDC 05012006
    Private Const FILE_SYSTEM_OBJECT As String = "Scripting.FileSystemObject"
    Private Const OBJECT_MANAGER As String = "bObjectManager.ObjectManager"


    Private Declare Function SHBrowseForFolder Lib "shell32" (ByRef lpbi As BrowseInfo) As Integer

    Private Declare Function SHGetPathFromIDList Lib "shell32" (ByVal pidList As Integer, ByVal lpBuffer As String) As Integer

    Private Declare Function lstrcat Lib "kernel32" Alias "lstrcatA" (ByVal lpString1 As String, ByVal lpString2 As String) As Integer

    Public Declare Function GetTempPath Lib "kernel32" Alias "GetTempPathA" (ByVal nBufferLength As Integer, ByVal lpBuffer As String) As Integer

    Public Declare Function RemoveDirectory Lib "kernel32" Alias "RemoveDirectoryA" (ByVal lpPathName As String) As Integer

    'JH021198 - moved from mainModule of iDOCOptions

    Public Const REGISTRY_USER As Integer = 1
    Public Const REGISTRY_SYSTEM As Integer = 2

    ' A "setting"
    Public Structure Setting_Type
        Dim sValue As String 'return value
        Dim sDefault As String ' Default if missing
        Dim bChanged As Boolean ' Changed or not
        Dim sSource As String ' Source (DB or Reg)
        Dim sKey As String ' Key in the registry
        Dim sSubKey As String ' subkey
        Dim iLocation As Integer ' USER or SYSTEM in the registry
        Dim sTable As String ' Location in database
        Dim sColumn As String ' Location in database
        Dim bNumber As Boolean ' Number or string?
        Dim sOptionName As String 'doc_options name
        Public Shared Function CreateInstance() As Setting_Type
            Dim result As New Setting_Type
            result.sValue = String.Empty
            result.sDefault = String.Empty
            result.sSource = String.Empty
            result.sKey = String.Empty
            result.sSubKey = String.Empty
            result.sTable = String.Empty
            result.sColumn = String.Empty
            result.sOptionName = String.Empty
            Return result
        End Function
    End Structure


    Private Structure BrowseInfo
        Dim hWndOwner As Integer
        Dim pIDLRoot As Integer
        Dim pszDisplayName As Integer
        Dim lpszTitle As Integer
        Dim ulFlags As Integer
        Dim lpfnCallback As Integer
        Dim lParam As Integer
        Dim iImage As Integer
    End Structure

    ' ***************************************************************** '
    ' Name: GetDOCRegSettings
    '
    ' Description: Gets DocuMaster specific registry settings. If anything
    ' is missing from registry, then user must be prompted immediately.
    ' If overwrite flag set, then ignore what is already there and force
    ' re-entry of setting.
    '
    ' ***************************************************************** '
    Public Function GetDOCRegSettings(Optional ByRef vTimerInterval As String = "", Optional ByRef vHistoryRoot As String = "", Optional ByRef vScanDirectory As String = "", Optional ByRef bOverWrite As Boolean = False, Optional ByRef hWndParent As Integer = 0, Optional ByRef bKofaxOrTwain As Boolean = False) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sTmp As String = ""
        Dim iButton As Integer
        Dim bMultipleRoots As Boolean
        Dim sSelectBoxTitle, sDriveKey As String
        Dim iHistoryRoots As Integer
        Dim sHistoryRootKey As String = ""
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily


        Try

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFDocumaster
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLCommon

            ' If we have no parent window, then use the desktop
            If Not True Then
                hWndParent = 0
            End If

            'Check if we already have timer interval set, if requested

            If Not Information.IsNothing(vTimerInterval) Then

                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCTimerIntervalKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCDaemonSection)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Check if we force overwrite
                If bOverWrite Then
                    vTimerInterval = ""
                Else
                    vTimerInterval = sTmp
                End If

                'If not in registry, capture timer interval
                If vTimerInterval = "" Then

                    While vTimerInterval = ""

                        'Prompt user for an interval  from 1 to 59 minutes
                        vTimerInterval = Interaction.InputBox("Please enter the Daemon Timer Interval.", "Timer Interval Setting")

                        Dim dbNumericTemp As Double
                        If Not Double.TryParse(vTimerInterval, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                            MessageBox.Show("You must supply a number between 1 and 59.", Application.ProductName)
                            vTimerInterval = ""

                        ElseIf ((CInt(vTimerInterval) < 1) Or (CInt(vTimerInterval) > 59)) Then

                            MessageBox.Show("You must supply a number between 1 and 59.", Application.ProductName)
                            vTimerInterval = ""

                        End If
                    End While

                    'Save to the registry
                    m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCTimerIntervalKey, v_sSettingValue:=vTimerInterval, v_sSubKey:=DOCDaemonSection)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error Message
                        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save Timer Interval", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDOCRegSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If

                End If

            End If

            'Check if we want the  historyroot

            If Not Information.IsNothing(vHistoryRoot) Then

                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCHistoryRootKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCDaemonSection)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Check if we force overwrite
                If bOverWrite Then
                    vHistoryRoot = ""
                Else
                    vHistoryRoot = sTmp
                End If

                If vHistoryRoot = "" Then

                    'JH260399 use multiple history roots?

                    bMultipleRoots = False

                    m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCMultipleHistoryRootKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCDaemonSection)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'don't use
                        bMultipleRoots = False
                    Else
                        If sTmp <> "Y" Then
                            'so they can turn off
                            bMultipleRoots = False
                        Else
                            bMultipleRoots = True
                        End If
                    End If

                    If bMultipleRoots Then

                        'use a maximum
                        iHistoryRoots = DOCMaximumHistoryRoots

                        'also here save "Drive1" as nextdrive value
                        'in case it is cancelled
                        m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCMultipleHistoryNextDriveKey, v_sSettingValue:="Drive1", v_sSubKey:=DOCMultipleHistoryRootSection)

                    Else

                        iHistoryRoots = 1

                    End If

                    For iCounter As Integer = 1 To iHistoryRoots

                        m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCHistoryRootKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCDaemonSection)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If bMultipleRoots Then

                            sDriveKey = "Drive" & iCounter
                            sSelectBoxTitle = "Please Select " & sDriveKey
                            'so that it overwrites each time
                            vHistoryRoot = ""

                        Else

                            sSelectBoxTitle = "Please Select History Root"

                        End If

                        'If not in registry, capture history Root
                        If vHistoryRoot = "" Then

                            While vHistoryRoot = ""

                                'proffer dialog box
                                m_lReturn = BrowseFolder(sFolder:=sTmp, sTitle:=sSelectBoxTitle, hWndParent:=hWndParent)

                                'leave, as we've errored or cancelled (must assume cancel as
                                ' cant distinguish)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return gPMConstants.PMEReturnCode.PMCancel
                                End If

                                vHistoryRoot = sTmp
                            End While

                            'Append a back slash if missing
                            If Not vHistoryRoot.EndsWith("\") Then
                                vHistoryRoot = vHistoryRoot & "\"
                            End If

                            'Save to the registry
                            'JH260399 only if first go round

                            If iCounter = 1 Then
                                m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCHistoryRootKey, v_sSettingValue:=vHistoryRoot, v_sSubKey:=DOCDaemonSection)

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                                    result = gPMConstants.PMEReturnCode.PMFalse

                                    ' Log Error Message
                                    iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save Journal Root", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDOCRegSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                                    Return result
                                End If
                            End If

                            If bMultipleRoots Then

                                'save to entry for drive
                                m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=sDriveKey, v_sSettingValue:=vHistoryRoot, v_sSubKey:=DOCMultipleHistoryRootSection)

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                                    result = gPMConstants.PMEReturnCode.PMFalse

                                    ' Log Error Message
                                    iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save Multiple Journal Root", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDOCRegSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                                    Return result
                                End If

                                'check if they want more drives
                                iButton = MessageBox.Show("Select another drive?", "Remote History Drives", MessageBoxButtons.YesNo)

                                If iButton <> System.Windows.Forms.DialogResult.Yes Then
                                    Exit For
                                End If

                            End If

                        End If

                    Next iCounter

                End If

            End If


            'Check if we want the scan directory

            If Not Information.IsNothing(vScanDirectory) Then

                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCScanDirKey, r_sSettingValue:=sTmp, v_sSubKey:=DOCScanSection)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Check if we force overwrite
                If bOverWrite Then
                    vScanDirectory = ""
                Else
                    vScanDirectory = sTmp
                End If

                'If not in registry, capture scan dir
                If vScanDirectory = "" Then

                    While vScanDirectory = ""

                        'proffer dialog box
                        m_lReturn = BrowseFolder(sFolder:=sTmp, sTitle:="Please Select Scan Folder", hWndParent:=hWndParent)

                        'leave, as we've errored or cancelled (must assume cancel as
                        ' cant distinguish)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMCancel
                        End If

                        vScanDirectory = sTmp
                    End While

                    'Append a back slash if missing
                    If Not vScanDirectory.EndsWith("\") Then
                        vScanDirectory = vScanDirectory & "\"
                    End If

                    'Save to the registry
                    m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCScanDirKey, v_sSettingValue:=vScanDirectory, v_sSubKey:=DOCScanSection)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error Message
                        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save Scan Directory", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDOCRegSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If

                End If

            End If

            'SOB230399Check if we want to use Kofax Or Twain True:=Kofax False:=Twain
            Dim sTmpKofaxOrTwain As String = ""
            If Not False Then
                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCTwainSettingsKey, r_sSettingValue:=sTmpKofaxOrTwain, v_sSubKey:=DOCKofaxOrTwain)
                bKofaxOrTwain = Conversion.Val(sTmpKofaxOrTwain)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Save to the registry
                m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCTwainSettingsKey, v_sSettingValue:=CStr(bKofaxOrTwain), v_sSubKey:=DOCKofaxOrTwain)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message
                    iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save Kofax or Twain Setting", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDOCRegSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDOCRegSettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyFile
    '
    ' Description: Wrapper to filecopy function so we can catch error
    ' fully.
    '
    ' ***************************************************************** '
    Public Function CopyFile(ByRef sFileIn As String, ByRef sFileOut As String) As Integer
        Dim result As Integer = 0
        Dim oFSO As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RDC 13122005 Replace FileCopy Command
            'TODO:CHANGE THE LOGIC TO COPY CONTENTS OF FILE --TARUN 26 
            'oFSO = New Scripting.FileSystemObject()


            oFSO.CopyFile(sFileIn, sFileOut)

            oFSO = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to copy file '" & sFileIn & "' to '" & sFileOut & "'", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RmDirectory
    '
    ' Description: Wrapper to RmDir function so we can catch error fully.
    '
    ' ***************************************************************** ''
    Public Function RmDirectory(ByRef sDirectory As String) As Integer
        Dim result As Integer = 0

        Dim oFSO As Object

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' RDC 04012006 change to FSO method
            'TODO:Needs to be modifed for DirectoryInfo
            'oFSO = New Scripting.FileSystemObject()


            oFSO.DeleteFolder(sDirectory, True)

            oFSO = Nothing


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to remove directory " & sDirectory, vApp:=ACApp, vClass:=ACClass, vMethod:="RmDirectory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: KillFile
    '
    ' Description: Wrapper to Kill function so we can catch error fully.
    '
    ' ***************************************************************** '
    Public Function KillFile(ByRef sFile As String) As Integer
        Dim result As Integer = 0

        Dim oFSO As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RDC 13122005 change to FSO method
            'TODO:Needs to be modifed for DirectoryInfo
            'oFSO = New Scripting.FileSystemObject()



            If oFSO.FileExists(sFile) Then

                oFSO.DeleteFile(sFile, True)
                oFSO = Nothing
            Else
                oFSO = Nothing
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to delete file " & sFile, vApp:=ACApp, vClass:=ACClass, vMethod:="KillFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function KillDir(ByRef DirName As String) As Integer
        Dim result As Integer = 0

        Dim oFSO As Object

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' RDC 15122005 replace with method that uses FSO
            m_lReturn = ClearFolder(DirName)
            'TODO:Needs to be modifed for DirectoryInfo
            'oFSO = New Scripting.FileSystemObject()


            oFSO.DeleteFolder(DirName, True)

            oFSO = Nothing


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception


            Debug.WriteLine(VB6.TabLayout(excep.Message, Information.Err().Number, DirName))

            Return gPMConstants.PMEReturnCode.PMFalse

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: BrowseFolder
    '
    ' Description: Opens a Treeview control that displays the directories
    ' in a computer (code obtained from MS Support site)
    '
    ' ***************************************************************** ''
    Public Function BrowseFolder(ByRef sFolder As String, ByRef sTitle As String, ByRef hWndParent As Integer) As gPMConstants.PMEReturnCode

        'Opens a Treeview control that displays the directories in a computer

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lpIDList As Integer
        Dim sBuffer, szTitle As String
        Dim tBrowseInfo As New BrowseInfo

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            szTitle = sTitle

            With tBrowseInfo
                .hWndOwner = hWndParent
                .lpszTitle = lstrcat(szTitle, "")
                .ulFlags = BIF_RETURNONLYFSDIRS + BIF_DONTGOBELOWDOMAIN
            End With

            lpIDList = SHBrowseForFolder(tBrowseInfo)

            If lpIDList = 0 Then
                'user cancelled
                Return gPMConstants.PMEReturnCode.PMCancel

            Else
                sBuffer = New String(" "c, MAX_PATH)
                SHGetPathFromIDList(lpIDList, sBuffer)
                sBuffer = sBuffer.Substring(0, sBuffer.IndexOf(Strings.Chr(0)))
                sFolder = sBuffer
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="BrowseFolder", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ChangeDOCRegSettings
    '
    ' Description: Allows user to change supplied registry settings.
    '
    ' Edit History:
    '
    ' JH260399 change so that if they want to use multiple
    ' history roots, the settings are saved to registry differently
    '
    '
    ' ***************************************************************** '
    Public Function ChangeDOCRegSettings(Optional ByRef vTimerInterval As String = "", Optional ByRef vHistoryRoot As String = "", Optional ByRef vScanDirectory As String = "", Optional ByRef hWndParent As Integer = 0) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sMsg As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Display current settings and prompt user to cahnge
            sMsg = "Your settings are as follows:" & Strings.Chr(10).ToString() & Strings.Chr(10).ToString()

            ' If we have no parent window, then use the desktop
            If Not True Then
                hWndParent = 0
            End If

            'timer interval

            If Not Information.IsNothing(vTimerInterval) Then
                sMsg = sMsg & DOCTimerIntervalKey & " = " & vTimerInterval & " Minutes." & Strings.Chr(10).ToString()
            End If

            'history root

            If Not Information.IsNothing(vHistoryRoot) Then
                sMsg = sMsg & DOCHistoryRootKey & " = '" & vHistoryRoot & "'" & Strings.Chr(10).ToString()
            End If

            'Scan Directory

            If Not Information.IsNothing(vScanDirectory) Then
                sMsg = sMsg & DOCScanDirKey & " = '" & vScanDirectory & "'" & Strings.Chr(10).ToString()
            End If

            sMsg = sMsg & Strings.Chr(10).ToString() & "Do you wish to make changes ?"

            m_lReturn = MessageBox.Show(sMsg, "DocuMaster Settings", MessageBoxButtons.YesNo)

            'check user response
            If m_lReturn = System.Windows.Forms.DialogResult.Yes Then

                'see if timer interval needs to be changed

                If Not Information.IsNothing(vTimerInterval) Then

                    'get new timer interval
                    m_lReturn = GetDOCRegSettings(vTimerInterval:=vTimerInterval, bOverWrite:=True, hWndParent:=hWndParent)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return m_lReturn
                    End If

                End If

                'see if history root needs to be changed

                If Not Information.IsNothing(vHistoryRoot) Then

                    'get new value
                    m_lReturn = GetDOCRegSettings(vHistoryRoot:=vHistoryRoot, bOverWrite:=True, hWndParent:=hWndParent)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return m_lReturn
                    End If

                End If

                'see if scan directory needs to be changed

                If Not Information.IsNothing(vScanDirectory) Then

                    'get new value
                    m_lReturn = GetDOCRegSettings(vScanDirectory:=vScanDirectory, bOverWrite:=True, hWndParent:=hWndParent)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return m_lReturn
                    End If

                End If

                'Display new settings.
                sMsg = "Your new settings are as follows:" & Strings.Chr(10).ToString() & Strings.Chr(10).ToString()

                'timer interval

                If Not Information.IsNothing(vTimerInterval) Then
                    sMsg = sMsg & DOCTimerIntervalKey & " = " & vTimerInterval & " Minutes." & Strings.Chr(10).ToString()
                End If

                'history root

                If Not Information.IsNothing(vHistoryRoot) Then
                    sMsg = sMsg & DOCHistoryRootKey & " = '" & vHistoryRoot & "'" & Strings.Chr(10).ToString()
                End If

                'Scan Directory

                If Not Information.IsNothing(vScanDirectory) Then
                    sMsg = sMsg & DOCScanDirKey & " = '" & vScanDirectory & "'" & Strings.Chr(10).ToString()
                End If

                MessageBox.Show(sMsg, "DocuMaster Settings")


            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="ChangeDOCRegSettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: MakePath
    '
    ' Description: This fucntion is provided a full DocuMaster Page path,
    ' which it needs to create if not already in existence.
    '
    '   The format will be:      \\SERVER_UNC\SHARE\PAGE_NAME
    '
    ' Clearly then we only need to test for and make the folder structure
    ' after SHARE (ie PAGE_NAME).
    '
    ' ***************************************************************** '
    Public Function MakePath(ByRef sPath As String) As Integer
        Dim result As Integer = 0

        Dim sNextDir As String = ""
        Dim iSlashCnt, iStart As Integer
        Dim oFSO As Object
        Dim oFolder As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sPath = sPath.Trim()

            'Find start position from where we start MkDir'ing (ie after 4th '\')
            For I As Integer = 1 To sPath.Length

                If sPath.Substring(I - 1, 1) = "\" Then

                    iSlashCnt += 1

                    If iSlashCnt = 4 Then
                        iStart = I + 1
                        Exit For
                    End If

                End If

            Next I

            'Now create all folder in what is left of path
            sNextDir = sPath.Substring(0, iStart - 1)
            'TODO:Needs to be modifed for DirectoryInfo
            'oFSO = New Scripting.FileSystemObject()

            For I As Integer = iStart To sPath.Length

                If sPath.Substring(I - 1, 1) <> "\" Then
                    sNextDir = sNextDir & sPath.Substring(I - 1, 1)
                Else

                    If Not oFSO.FolderExists(sNextDir) Then

                        oFolder = oFSO.CreateFolder(sNextDir)
                    End If

                    sNextDir = sNextDir & "\"
                End If
            Next I

            oFSO = Nothing

            Return result

        Catch excep As System.Exception




            If Information.Err().Number = 0 Or (Information.Err().Number = 75) Then
                'This is perfectly agreeable, as just means directory you tried
                'to create is already there, so continue


            End If

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Make Path: " & sPath, vApp:=ACApp, vClass:=ACClass, vMethod:="MakePath", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetFileName
    '
    ' Description: Given a full path, this function returns the actual
    ' file name and the directory it is in.
    '
    ' ***************************************************************** '
    Public Function GetFileName(ByRef sFullPath As String, ByRef sFile As String, ByRef sDir As String) As Integer

        Dim result As Integer = 0
        Dim I, iLen, iSlashPos As Integer
        Const iMinPathLen As Integer = 3


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sFullPath = sFullPath.Trim()
            sFile = ""
            sDir = ""
            'get path length
            iLen = sFullPath.Length

            'find directory delimiter
            For I = iLen To 1 Step -1
                If (sFullPath.Substring(I - 1, 1) = "\") Or (sFullPath.Substring(I - 1, 1) = "/") Then

                    iSlashPos = I

                    Exit For
                End If
            Next I

            'check it's reasonable
            If (I < iMinPathLen) Or (I = iLen) Then
                'error
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get file name from '" & sFullPath & "'", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFileName", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            'Pick out directory and file name
            sDir = sFullPath.Substring(0, iSlashPos - 1)
            sFile = sFullPath.Substring(sFullPath.Length - (iLen - iSlashPos))

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get file name from '" & sFullPath & "'", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFileName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: StripSlashes
    '
    ' Description: This function removes slashes from a path.
    '
    ' ***************************************************************** '
    Public Function StripSlashes(ByRef sStringIn As String, ByRef sStringOut As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sStringOut = ""

            For I As Integer = 1 To sStringIn.Length
                If sStringIn.Substring(I - 1, 1) <> "\" And sStringIn.Substring(I - 1, 1) <> "/" Then
                    sStringOut = sStringOut & sStringIn.Substring(I - 1, 1)
                End If
            Next I

            Return result

        Catch



            result = gPMConstants.PMEReturnCode.PMFalse
            sStringOut = ""
            Return result
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: ExtractNumFromKey
    '
    ' Description: The key of each node (for treeview or listview) contains
    ' the following info
    '
    '       <NodeType>          -   'F' = Folder
    '                               'D' = Document
    '       <Passworded>        -   'P' = Node Passworded
    '                               ' ' = Node Not Passworded
    '       <Date>              -   '99999' = Node date (as long)
    '
    '       <Node number>       -   Node Number
    '
    ' This function gets the folder_num (or doc_num) from the key
    '
    ' Edit history: JH051198 if the node is an 'Add Folders to View' then it
    '               needs to return a message to explain the fault
    '
    ' ***************************************************************** '
    Public Function ExtractNumFromKey(ByRef sKey As String, ByRef lNum As Integer) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If sKey.StartsWith("ADD") Then
                MessageBox.Show("Can't Perform This Action With 'Add Folders to View' Node", "Add Folders to View", MessageBoxButtons.OK)
                Return gPMConstants.PMEReturnCode.PMError
            End If

            'Get num out of key
            lNum = CInt(sKey.Substring(sKey.Length - (sKey.Length - DOCNodeKeyOffSet)))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Log to File
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractNumFromKey", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: iPMFunc.LogMessageScan
    '
    ' Description: Wrapper function for business class in ScanStation
    '
    ' ***************************************************************** '
    Public Function LogMessageScan(ByRef iType As Integer, ByRef sMsg As String, Optional ByRef vApp As Object = Nothing, Optional ByRef vClass As Object = Nothing, Optional ByRef vMethod As Object = Nothing, Optional ByRef vErrNo As Object = Nothing, Optional ByRef vErrDesc As Object = Nothing, Optional ByRef bStandAlone As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If bStandAlone Then






                gPMFunctions.LogMessagePopup(iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), vClass:=CStr(vClass), vMethod:=CStr(vMethod), excep:=New Exception(CStr(vErrDesc)))

            Else

                iPMFunc.LogMessage(sUsername:="", iType:=iType, sMsg:=sMsg, vApp:=vApp, vClass:=vClass, vMethod:=vMethod, vErrNo:=vErrNo, vErrDesc:=vErrDesc)

            End If

            Return result

        Catch




            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: GetFilenameFromPath
    '
    ' Description: Strips the directories from a pathname, and returns
    '              the filename
    '
    ' ***************************************************************** '

    Private Function GetFilenameFromPath(ByRef sPath As String, ByRef sFilename As String) As Integer

        Dim result As Integer = 0
        Dim sTemp As String = ""
        Dim iLen As Integer
        ' developer's guide np. 128
        'Dim sByte_Renamed As New FixedLengthString(1)
        Dim sByte_Renamed As New VB6.FixedLengthString(1)



        result = gPMConstants.PMEReturnCode.PMTrue

        iLen = sPath.Length

        If iLen = 0 Then
            ' Log Error.
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetFilenameFromPath passed zero length string.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFilenameFromPath", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return gPMConstants.PMEReturnCode.PMError
        End If

        sTemp = ""

        For iLoop1 As Integer = iLen To 1 Step -1
            sByte_Renamed.Value = sPath.Substring(iLoop1 - 1, 1)
            If sByte_Renamed.Value = "\" Or sByte_Renamed.Value = "/" Then
                Exit For
            End If
            sTemp = sByte_Renamed.Value & sTemp
        Next iLoop1

        ' Return the new path
        sFilename = sTemp

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: GetDirectoryFromPath
    '
    ' Description: Strips the filename from a pathname, and returns
    '              the directory
    '
    ' ***************************************************************** '

    Private Function GetDirectoryFromPath(ByRef sPath As String, ByRef sDirectory As String) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sFilename As String = ""
        Dim iLen As Integer

        ' If we remove the filename from the path, then the rest
        ' must be the directory & share etc...



        result = gPMConstants.PMEReturnCode.PMTrue

        iLen = sPath.Length

        If iLen = 0 Then
            ' Log Error.
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetDirectoryFromPath passed zero length string.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDirectoryFromPath", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return gPMConstants.PMEReturnCode.PMError
        End If

        m_lReturn = GetFilenameFromPath(sPath:=sPath, sFilename:=sFilename)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error.
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get filename from path.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDirectoryFromPath", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return gPMConstants.PMEReturnCode.PMError
        End If

        ' Return the directory
        sDirectory = sPath.Substring(0, iLen - sFilename.Length)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetDirectoryFromUNC
    '
    ' Description: Given a UNC (eg. \\machine\share\directory)
    '              it will return the directory (eg. \directory)
    '
    ' ***************************************************************** '
    Private Function GetDirectoryFromUNC(ByRef sUNC As String, ByRef sDirectory As String) As Integer

        Dim result As Integer = 0
        Dim iLen As Integer
        Dim sTemp As New StringBuilder
        Dim iStart As Integer
        ' developer's guide np. 128
        Dim sByte_Renamed As New VB6.FixedLengthString(1)



        result = gPMConstants.PMEReturnCode.PMTrue

        iLen = sUNC.Length
        If iLen = 0 Then
            ' Log Error.
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetDirectoryFromUNC passed zero length string.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDirectoryFromUNC", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return gPMConstants.PMEReturnCode.PMError
        End If

        If Not sUNC.StartsWith("\\") Then
            ' Log Error.
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetDirectoryFromUNC not passed a UNC path.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDirectoryFromUNC", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return gPMConstants.PMEReturnCode.PMError
        End If

        iStart = 3

        ' Skip passed the computer name
        Do
            iStart += 1

            If iStart > iLen Then
                ' Log Error.
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetDirectoryFromUNC not passed a UNC path.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDirectoryFromUNC", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMError
            End If

            sByte_Renamed.Value = sUNC.Substring(iStart - 1, 1)
            If (sByte_Renamed.Value = "\" Or sByte_Renamed.Value = "/") Then Exit Do
        Loop

        ' Skip passed the share
        Do
            iStart += 1

            If iStart > iLen Then
                ' Log Error.
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetDirectoryFromUNC not passed a UNC path.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDirectoryFromUNC", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMError
            End If

            sByte_Renamed.Value = sUNC.Substring(iStart - 1, 1)
            If (sByte_Renamed.Value = "\" Or sByte_Renamed.Value = "/") Then Exit Do
        Loop

        sTemp = New StringBuilder("")

        ' Store the path
        For iLoop1 As Integer = iStart To iLen
            sByte_Renamed.Value = sUNC.Substring(iLoop1 - 1, 1)
            sTemp.Append(sByte_Renamed.Value)
        Next iLoop1

        ' Return the path
        sDirectory = sTemp.ToString()

        Return result

    End Function

    ' ******************************************************************** '
    ' Name: MakeDirectory
    '
    ' Description: Makes a directory on a local machine, unlike MakePath
    '              eg. c:\pmdata\mydata\is\here   or
    '              \pmdata\mydata\is\here
    '
    '              The second example will be made on the current drive
    '
    ' ******************************************************************** '

    Private Function MakeDirectory(ByRef sDirectory As String) As Integer
        Dim result As Integer = 0

        ' developer's guide np. 128
        'Dim sDriveLetter As New FixedLengthString(2)
        Dim sDriveLetter As New VB6.FixedLengthString(2)
        Dim sPath As String = ""
        Dim oFSO As Object
        Dim oFolder As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        ' check for drive letter
        If sDirectory.Substring(1, 1) = ":" Then
            ' we have one so get it
            sDriveLetter.Value = sDirectory.Substring(0, 2)
            ' Remove the drive letter
            sDirectory = sDirectory.Substring(sDirectory.Length - (sDirectory.Length - 2))
        Else
            ' we dont have one, so make it from the current drive
            sDriveLetter.Value = Directory.GetCurrentDirectory().Substring(0, 2)
        End If

        ' loop through the path now, and create the directories
        sPath = ""
        'TODO:Needs to be modifed for DirectoryInfo
        'oFSO = New Scripting.FileSystemObject()

        For iLoop1 As Integer = 1 To sDirectory.Length

            sPath = sDirectory.Substring(0, iLoop1)
            sPath = sDriveLetter.Value & sPath

            If sPath.EndsWith("\") Then
                ' reached the end of a dir, make the path so far

                If Not oFSO.FolderExists(sPath) Then
                    ' only if it doesnt exist of course...

                    oFolder = oFSO.CreateFolder(sPath)
                End If
            End If

        Next iLoop1

        ' make the directory again, incase there was no \ on the end of the path

        If Not oFSO.FolderExists(sPath) Then

            oFolder = oFSO.CreateFolder(sPath)
        End If

        oFSO = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name:        CacheFile
    '
    ' Description: Checks if the file exists locally. if not, then
    '              copies it from server. Stores new path in sNewFilename.
    '
    '              If bZipped is True, then it will uncompress it
    '              accross it from the server to the local machine.
    '
    '   Edit History:
    '
    '   MS 270600
    '   Added code to read the filepath locally and not just from Server UNC
    '   (needed in briefcase mode)
    '
    ' ***************************************************************** '
    Public Function CacheFile(ByRef oZipper As Object, ByRef sFilename As String, ByRef sNewFilename As String, ByRef sCachePath As String, Optional ByRef bZipped As Boolean = False) As Integer
        Dim result As Integer = 0

        ' sFilename = the remote copy
        ' sTarget = The local copy
        Dim sTarget, sTemp As String
        'MS 270600
        Dim iBackSlash As Integer
        Dim oFSO As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If not specified that its zipped or not, assume its not
            If Not True Then
                bZipped = False
            End If

            ' sFilename is a local directory (needed in briefcase mode) and not UNC
            If Not sFilename.StartsWith("\\") Then 'MS 270600

                ' the target file will be "xx\xx\xx\xx\xx.xxx"
                ' i.e the filename will always start after the 5th backslash from the end
                For iPos As Integer = sFilename.Length To 1 Step -1
                    If Mid(sFilename, iPos, 1) = "\" Then
                        iBackSlash += 1
                        If iBackSlash = 5 Then
                            sTarget = Mid(sFilename, iPos + 1)
                            Exit For
                        End If
                    End If
                Next iPos
            Else
                'MS 270600
                ' Removes the \\COMPUTER\SHARE bit from a path
                ' Leaves any filename intact though (good in this case)
                m_lReturn = GetDirectoryFromUNC(sFilename, sTarget)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If 'MS 270600

            If Not sCachePath.EndsWith("\") And Not sTarget.StartsWith("\") Then
                sCachePath = sCachePath & "\"
            End If

            sTarget = sCachePath & sTarget
            'TODO:Needs to be modifed for DirectoryInfo
            'oFSO = New Scripting.FileSystemObject()

            ' Check that it isnt cached already

            If Not oFSO.FileExists(sTarget) Then

                ' Get the directory from the path
                m_lReturn = GetDirectoryFromPath(sPath:=sTarget, sDirectory:=sTemp)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetDirectoryFromPath process failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="CacheFile", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                ' make the directory
                m_lReturn = MakeDirectory(sDirectory:=sTemp)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="MakeDirectory process failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="CacheFile", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                ' RDC 20062005 check the file and see if it is zipped
                m_lReturn = ZipCheck(sFilename:=sFilename, bZipped:=bZipped)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' is file zipped?
                If Not bZipped Then

                    ' no then just copy it

                    ' its not cached, cache it
                    m_lReturn = CopyFile(sFilename, sTarget)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error.
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CopyFile process failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="CacheFile", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If

                Else

                    ' unzip the file to the cache directory

                    m_lReturn = oZipper.UnZipFile(sFileIn:=sFilename, sFileOut:=sTarget)
                    If Not m_lReturn Then

                        result = gPMConstants.PMEReturnCode.PMFalse

                        iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="UnZipFile process failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="CacheFile", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                        Return result
                    End If

                End If

            End If

            oFSO = Nothing

            ' Return the location of the local copy
            sNewFilename = sTarget

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CacheFile process failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CacheFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: GetDMEDIR
    '
    ' Description: This fucntion returns the DocuMaster install
    ' directory from the registry
    '
    ' ***************************************************************** '
    Public Function GetDMEDIR(ByRef sDMEDir As String) As Integer

        Dim result As Integer = 0
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFDocumaster
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLCommon

            'get key value from registry
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=DOCDMEDIRKey, r_sSettingValue:=sDMEDir)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & DOCDMEDIRKey & " from Registry", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDMEDIR", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            If CBool(CStr(sDMEDir = "").Trim()) Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDMEDIR", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ****************************************************************************
    'JH021198 - moved from frmInterface of iDOCOptions
    '
    ' Function : GetRegistryValue
    '
    ' Desc.    : Gets a value from the Documaster part of the database.
    '            Always has the "Options" subkey
    '
    ' ****************************************************************************
    Public Function GetRegistryValue(ByRef sKey As String, ByRef sSubKey As String, ByRef sValue As String, ByRef iLocation As Integer) As Integer

        Dim result As Integer = 0
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If iLocation = REGISTRY_USER Then
                ' hkey\current user
                eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser
            Else
                ' hkey\local machine
                eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            End If

            ' client registry
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLCommon
            ' Documaster, wahey
            eProductFamily = gPMConstants.PMEProductFamily.pmePFDocumaster

            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=sKey, r_sSettingValue:=sValue, v_sSubKey:=sSubKey)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' oh no, error
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get value from the registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegistryValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ****************************************************************************
    ''JH021198 - moved from frmInterface of iDOCOptions
    '
    ' Function : SetRegistryValue
    '
    ' Desc.    : Sets a value in the Documaster part of the database.
    '            Always has the "Options" subkey.
    '
    ' ****************************************************************************
    Public Function SetRegistryValue(ByRef sKey As String, ByRef sSubKey As String, ByRef sValue As String, ByRef iLocation As Integer) As Integer

        Dim result As Integer = 0
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If iLocation = REGISTRY_USER Then
                ' hkey\current user
                eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser
            Else
                ' hkey\local machine
                eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            End If

            ' client registry
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLCommon
            ' Documaster, wahey
            eProductFamily = gPMConstants.PMEProductFamily.pmePFDocumaster

            ' Set the registry
            m_lReturn = SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=sKey, v_sSettingValue:=sValue, v_sSubKey:=sSubKey)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' oh no, error
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set value in the registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="SetRegistryValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' JH021198 - to be used for scan settings but may be useful elsewhere
    ' Name: SetComboText
    '
    ' Description:
    ' for when a value has been selected previously and saved in
    ' the registry but is no longer available in the list.
    ' returns iList as the index of the matching text
    '
    ' ***************************************************************** '
    Public Function SetComboText(ByRef Cbo As ComboBox, ByRef sText As String, ByRef iList As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            For I As Integer = 0 To Cbo.Items.Count - 1
                If VB6.GetItemString(Cbo, I) = sText Then
                    iList = I
                    Return result
                End If
            Next I

            'if the program reaches this point the string is not in the list
            iList = -1 'or something else

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetComboTextFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetComboText", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: IsSiriusInstalled
    '
    ' Description: Generic function to check if Sirius is installed
    '
    ' History: 11/07/2003 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function IsSiriusInstalled(ByRef r_bSiriusInstalled As Boolean) As Integer
        Dim result As Integer = 0
        Dim oObjectManager As Object

        'Dim oDocCommit As bDOCCommitServer.Commit
        Dim oDocCommit As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Default to not installed
            r_bSiriusInstalled = False

            ' Get new instance of ObjectManager
            oObjectManager = New bObjectManager.ObjectManager()


            m_lReturn = oObjectManager.Initialise(sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                    ' Log Error Message
                    iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of ObjectManager", vApp:=ACApp, vClass:=ACClass, vMethod:="IsSiriusInstalled", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If
                Return result
            End If

            ' Get an instance of bDOCCommitServer

            m_lReturn = oObjectManager.GetInstance(oObject:=oDocCommit, sClassName:="bDOCCommitServer.Commit", vInstanceManager:=PMGetViaClientManager)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Failed to get instance of bDOCCommitServer.Commit", vApp:=ACApp, vClass:=ACClass, vMethod:="IsSiriusInstalled", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Get if Sirius is installed

            m_lReturn = oDocCommit.IsSBOInstalled(bInstalled:=r_bSiriusInstalled)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogInfo, sMsg:="Failed to call IsSBOInstalled on oDocCommit", vApp:=ACApp, vClass:=ACClass, vMethod:="IsSiriusInstalled", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Terminate and clear up

            oDocCommit.Dispose()
            ' Remove the instance
            oDocCommit = Nothing


            oObjectManager.Dispose()
            ' Remove the instance
            oObjectManager = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsSiriusInstalled Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsSiriusInstalled", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Function Name : SetCacheStatus
    '
    ' Created on    : 08072004
    '
    ' Description   : Gets status of client-side cache - enabled/disabled
    '
    ' Notes         : cache status is stored in DOC_System.cache_disabled
    '                 and is managed via iDOCOptions (administrator only)
    ' Edit History  :
    ' RDC 08072004  : created
    ' RAM20040712   : Bug fix.
    '                 1. Removed the early binding ref to "bObjectManager.ObjectManager"
    '                    since, in business objects there will be no reference to object Manager
    '                 2. Added the sUserName parameter to iPMFunc.LogMessage
    ' ***************************************************************** '
    Public Function SetCacheStatus(ByRef bCacheDisabled As Boolean) As Integer
        Dim result As Integer = 0

        Dim sValue As String = ""
        Dim oObj As Object

        'Dim oOptions As bDOCOptions.form
        Dim oOptions As Object

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            bCacheDisabled = False

            ' get object manager
            oObj = New bObjectManager.ObjectManager()

            If oObj Is Nothing Then
                Return result
            End If


            m_lReturn = oObj.Initialise(sCallingAppName:=ACApp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise Object Manager", vApp:=ACApp, vClass:=ACClass)

                Return result
            End If

            ' get bDOCOptions.Form

            m_lReturn = oObj.GetInstance(oObject:=oOptions, sClassName:="bDOCOptions.form", vInstanceManager:=PMGetViaClientManager)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create/initialise bDOCOptions.form", vApp:=ACApp, vClass:=ACClass)

                Return result
            End If

            ' get setting cache_disabled from DOC_System

            m_lReturn = oOptions.GetSetting(sTable:="DOC_System", sColumn:="cache_disabled", sValue:=sValue)

            ' set value
            bCacheDisabled = (sValue = "1")

            ' close the objects

            oOptions.Dispose()
            oOptions = Nothing


            oObj.Dispose()
            oObj = Nothing


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get cache enabled/disabled status", vApp:=ACApp, vClass:=ACClass, vMethod:="SetCacheStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Function Name : ZipCheck
    '
    ' Created on    : 20062005
    '
    ' Description   : Checks if a file is zipped or not
    '
    ' ***************************************************************** '
    Public Function ZipCheck(ByVal sFilename As String, ByRef bZipped As Boolean) As Integer
        Dim result As Integer = 0
        Dim sZIPSIG As String = ""
        Dim sID As New StringBuilder
        Dim oFSO, oFile As Object

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' signature of a zipped file
            sZIPSIG = Strings.Chr(&H50S).ToString() & Strings.Chr(&H4BS).ToString() & Strings.Chr(3).ToString() & Strings.Chr(4).ToString()

            ' RDC 20062005 check file signature to see if it is zipped
            'TODO:Needs to be modifed for DirectoryInfo
            'oFSO = New Scripting.FileSystemObject()

            'If Not oFSO.FileExists(sFilename) Then
            '    Exit Function
            'End If


            oFile = oFSO.OpenTextFile(sFilename)

            sID = New StringBuilder("")


            Do While sID.ToString().Length < 4 And Not oFile.AtEndOfStream

                sID.Append(oFile.Read(1))
            Loop


            oFile.Close()
            oFile = Nothing
            oFSO = Nothing

            bZipped = sID.ToString() = sZIPSIG


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check if file zipped", vApp:=ACApp, vClass:=ACClass, vMethod:="ZipCheck", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Function Name : ClearFolder
    '
    ' Created on    : 30062005
    '
    ' Description   : empty the files and folders from the
    '                 folder specified
    '
    ' ***************************************************************** '
    Public Function ClearFolder(ByVal sFolder As String) As Integer
        Dim result As Integer = 0

        Dim oFolder As Object 'Scripting.Folder
        Dim oFSO As Object 'Scripting.FileSystemObject

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'TODO:Needs to be modifed for DirectoryInfo
            'oFSO = New Scripting.FileSystemObject()


            oFolder = oFSO.GetFolder(sFolder)

            m_lReturn = PurgeFolder(oFSO, oFolder)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            oFolder = Nothing
            oFSO = Nothing


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearFolder failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearFolder", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Function Name : ClearCache
    '
    ' Created on    : 30062005
    '
    ' Description   : called recursively, as folders cannot be
    '                 deleted until they are empty.
    '
    ' ***************************************************************** '
    Private Function PurgeFolder(ByVal oFSO As Object, ByVal oParentFolder As Object) As Integer

        Dim result As Integer = 0
        'Scripting.Folder
        'Scripting.File



        result = gPMConstants.PMEReturnCode.PMFalse

        ' delete sub-folders first

        For Each oFolder As Object In oParentFolder.SubFolders
            m_lReturn = PurgeFolder(oFSO, oFolder)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            oFSO.DeleteFolder(oFolder, True)
        Next oFolder

        ' then remove any files

        For Each oFile As Object In oParentFolder.Files


            oFSO.DeleteFile(oFile.Path, True)
        Next oFile


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function
End Module

