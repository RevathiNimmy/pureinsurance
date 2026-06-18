Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")>
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 4/2/98
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    '
    '- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    '
    ' MS 040700 :          Briefcase Functionality project for DME
    '
    '   Module "Start"
    '   Modified code to determine the run mode of DME e.g. Briefcase download mode etc.
    '
    '   Module "Active"
    '   Modified code to determine the run mode of DME..
    '   -
    '   New subs/functions added
    '   BriefCaseProcess, DownLoadProcess, AttachDB,
    '   DetachDB, IsThere, ProcessEnd, Pause
    '   -
    '   Added Forms, Modules
    '   frmProgress.frm - progress bar to show the status of the Briefcase download
    '   ADVReg.bas      - registry functions
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)


    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer
    Public m_lDocNum As Integer ' unique document number
    Public sCommand As String = ""
    Public sExCodes(4) As String
    Private m_sLocalPC As String = "" ' computer name
    Public m_sCurrentUser As String = "" ' current briefcase user
    Private m_sFolderName As String = ""

    Const m_cDataFile As String = "PMBriefcase.mdf"
    Const m_cLogFile As String = "PMBriefcase.ldf"
    Const m_cPMBriefcase As String = "PMBriefcase"

    Private frmProgress As frmProgress
    Private frmInterface As frmInterface
    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    ' PRIVATE Data Members (End)
    Private Declare Function FindExecutable Lib "shell32.dll" Alias "FindExecutableA" (ByVal lpFile As String, ByVal lpDirectory As String, ByVal lpResult As String) As Integer
    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
#If PD_EARLYBOUND = 1 Then

			Set g_oObjectManager = New bObjectManager.ObjectManager
#Else
            g_oObjectManager = New bObjectManager.ObjectManager
#End If

            ' Call the initialise method.

            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    'Fine


                Case gPMConstants.PMEReturnCode.PMCancel
                    'Fine, go
                    result = m_lReturn
                    g_oObjectManager = Nothing
                    Return result

                Case Else
                    ' Failed .
                    result = m_lReturn

                    ' Set the object manager to nothing.
                    g_oObjectManager = Nothing

                    ' Log Error.
                    LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                    Return result

            End Select

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager

                g_iLanguageID = .LanguageID

                g_iSourceID = .SourceID

                g_sUserName = .UserName
            End With


            ' Get the name of the local laptop/PC.
            m_lReturn = CType(gPMFunctions.GetSystemName(sSystemName:=m_sLocalPC), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to get system name of the computer ", "Terminating DME Briefcase Download", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
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
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start(Optional ByRef vCmd As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim iInstances As Integer = 0

        Dim bDocInList As Object
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ''When Exe is running and task is launched  then exit
            Dim Procesos() As Process = Process.GetProcessesByName("PMWorkManager")
            If Procesos.Length > 0 Then
                For index As Integer = 0 To Procesos.Length - 1
                    If Procesos(index).MainWindowTitle.ToString.ToUpper.Trim = "DOCUMASTER ENTERPRISE" AndAlso Procesos(index).SessionId = Process.GetCurrentProcess.SessionId Then
                        iInstances += 1
                    End If
                Next
                If iInstances > 0 Then
                    MessageBox.Show("Instance of Documaster that is already open", "DocuMaster EnterPrise", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Function
                End If
            End If

            ' If there is already a DocManager task running then exit
            If Application.OpenForms.Count > 0 Then
                For Each frm As Form In Application.OpenForms
                    If frm.GetType.ToString.Equals("iDOCManager.frmInterface") Then
                        If vCmd Is Nothing Then
                            frm.Activate()
                        Else
                            Activate(ToSafeString(vCmd))
                        End If
                        result = gPMConstants.PMEReturnCode.PMTrue
                        Return result
                    End If
                Next
            End If
            'When task is running and exe is launched  then exit
            If Process.GetProcessesByName("PMWorkManager").Length > 0 AndAlso Process.GetCurrentProcess.ProcessName.Contains("iDOCManager") Then
                If Process.GetProcessesByName("PMWorkManager")(0).SessionId = Process.GetCurrentProcess.SessionId Then
                    For Indx As Integer = 0 To Process.GetProcessesByName("PMWorkManager")(0).Modules.Count - 1
                        If Process.GetProcessesByName("PMWorkManager")(0).Modules(Indx).ModuleName.Contains("iDocViewer") Then
                            result = gPMConstants.PMEReturnCode.PMTrue
                            Return result
                        End If
                    Next
                End If
            End If
            frmInterface = New frmInterface
            frmInterface.FrmManager = Me
            If vCmd IsNot Nothing AndAlso vCmd <> "" Then
                frmInterface.oDOCManagerInterface = Me
                Me.sCommand = vCmd
                frmInterface.SetModes()
                ' Display the interface.
                VB6.ShowForm(frmInterface, 0)
                frmInterface.SendToBack()
            Else
                ' Display the interface.
                VB6.ShowForm(frmInterface, 1)
            End If



            If frmInterface.tvwMain.Nodes.Count = 0 Then
                MessageBox.Show("DocuMaster Briefcase Database is empty!", "Quitting DocuMaster", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                'release object references
                frmInterface.Close()
            End If


            result = gPMConstants.PMEReturnCode.PMTrue
            Return result




        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: Activate (Standard Method)
    '
    ' Description: Entry point for the object when it has already been
    ' instanced.
    '
    ' ***************************************************************** '
    Public Function Activate(ByRef sCommand As String) As Integer

        Dim result As Integer = 0
        Dim sExCodes(4) As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '  Display the interface.
            If frmInterface Is Nothing Then
                For Each frm As Form In System.Windows.Forms.Application.OpenForms
                    If frm.Name = "frmInterface" AndAlso (Not frm.Tag Is Nothing) AndAlso frm.Tag.ToString().Contains("DocuMaster Enterprise") Then
                        frmInterface = frm
                        Exit For
                    End If
                Next
            End If



            ' if currently minimised, make it normal
            If frmInterface.WindowState = FormWindowState.Minimized Then
                frmInterface.WindowState = FormWindowState.Maximized
            End If


            ' request to display a document triggered by SBO
            If m_lDocNum > 0 Then

                SBOViewDocument()

                Return result

            End If


            'uppercase and trim command
            sCommand = sCommand.ToUpper()
            sCommand = sCommand.Trim()


            'Sirius Briefcase command line detected

            If sCommand.StartsWith("BC") Then

                ' Process Briefcase download or run command line for viewing
                m_lReturn = CType(BriefCaseProcess(sCommand), gPMConstants.PMEReturnCode)

                'if briefcase download was performed, attach database back
                If sCommand.StartsWith("BC ") Then
                    m_lReturn = CType(AttachDB(), gPMConstants.PMEReturnCode)
                End If


                If m_sCurrentUser = g_sUserName Then
                    'reset the briefcase user on server registry as download has finished

                    m_lReturn = g_oBusiness.BriefcaseUser(sMode:="SET", sUser:="")

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to reset Briefcase user details in server registry", "Terminating Briefcase Download", MessageBoxButtons.OK, MessageBoxIcon.Error)

                        ' don't want to run anything else after this..
                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If
                End If


            End If



            'SBO command line detected, request to display folder

            If sCommand.StartsWith("SBO") Then

                m_lReturn = CType(SBODisplayClient(sCommand, True), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Cannot run iDocManager.SBODisplayClient", "DocuMaster Folder Display", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                ' don't want to run anything else after this..
                Return result

            End If

            ' Archive document request from Document Viewer
            If sCommand.StartsWith("ARCHIVE") Then


                m_lReturn = CType(ProcessArchive(sCommand), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Cannot run iDocManager.ProcessArchive", "DocuMaster Document Archive Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                'don't want to run anything else after this..
                Return result

            End If



            'any other calls..

            If sCommand <> "" Then
                'splice the command
                m_lReturn = CType(GetExternalCodes(sCommand, sExCodes), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                frmInterface.ConstructView(sCabExCode:=sExCodes(1), sDrawExCode:=sExCodes(2), sFoldExCode:=sExCodes(3), sDocExCode:=sExCodes(4))
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Activate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: GetExternalCodes
    '
    ' Description: Get the external codes from the command string.
    '
    ' ***************************************************************** '
    Public Function GetExternalCodes(ByRef sCommand As String, ByRef sArgArr() As String) As Integer

        Dim result As Integer = 0
        Dim iArrCntr As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Read Argument list into an array
            For I As Integer = 1 To 4
                sArgArr(I) = ""
            Next I

            iArrCntr = 1
            For iStrCntr As Integer = 1 To sCommand.Length
                If sCommand.Substring(iStrCntr - 1, 1) = "|" Then
                    iArrCntr += 1
                Else
                    sArgArr(iArrCntr) = sArgArr(iArrCntr) & sCommand.Substring(iStrCntr - 1, 1)
                End If
            Next iStrCntr

            ' Trim all arguments
            For I As Integer = 1 To 4
                sArgArr(I) = sArgArr(I).Trim()
            Next I

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="GetExternalCodes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name:         BriefCaseProcess
    '
    ' Description:  Main Briefcase download process
    '
    ' ***************************************************************** '
    Public Function BriefCaseProcess(ByRef sCommand As String) As Integer

        Dim result As Integer = 0
        Dim sExCodes(4) As String 'array to store the four values for cabinet, drawer, folder & doc
        Dim iFreeFile As Integer
        Dim sLine, sType, sPath, sCabExCode, sDrawerExCode, sFolderExCode As String
        Dim nLineNo As Integer
        Dim vPageList As Object
        Dim sDir As String = ""
        Dim nErrorTotal As Integer
        Dim sClientName, sErrMsg, sUser As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Determine the type of command to be executed       (1st 3 chars indicate type)
            sType = sCommand.Substring(0, 3)

            Select Case sType
                Case "BC " 'execute a download to Briefcase
                    sType = "DATAFILE"

                Case "BCD" 'execute single drawer command for display only
                    sType = "DRAWER"


                Case "BCF" 'execute a folder command line for display only
                    sType = "FOLDER"


                Case Else 'invalid command enetered in command line

                    Return gPMConstants.PMEReturnCode.PMFalse

            End Select


            ' with external code supplied display Drawer using construct view
            If sType = "DRAWER" Then

                'The ex_code starts from 4th char in the sCommand parameter
                sDrawerExCode = Mid(sCommand, 4)

                'Get drawer's parent i.e. cabinet ex_code

                m_lReturn = g_oBusiness.GetParentExCode(sChildExCode:=sDrawerExCode, sParentExCode:=sCabExCode, lChildFoldLevel:=DOCDrawer)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Failed to get folder " & Strings.Chr(13) & Strings.Chr(10) &
                                    "Drawer : " & sDrawerExCode & Strings.Chr(13) & Strings.Chr(10) &
                                    "Cabinet: " & sCabExCode, "DocuMaster Display Folder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If

                'construct view from supplied cabinet and drawer info
                frmInterface.ConstructView(sCabExCode:=sCabExCode, sDrawExCode:=sDrawerExCode, sFoldExCode:="", sDocExCode:="")
            End If


            ' with external code supplied display Folder using construct view
            If sType = "FOLDER" Then

                'The ex_code starts from 4th character in the sCommand parameter
                sFolderExCode = Mid(sCommand, 4)

                'Get external code for parent of Folder (Drawer)

                m_lReturn = g_oBusiness.GetParentExCode(sChildExCode:=sFolderExCode, sParentExCode:=sDrawerExCode, lChildFoldLevel:=DOCFolder)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Failed to get folder record. External codes." & Strings.Chr(13) & Strings.Chr(10) &
                                    "Drawer: " & sDrawerExCode & Strings.Chr(13) & Strings.Chr(10) &
                                    "Folder: " & sFolderExCode, "DocuMaster Display Folder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result

                End If

                'Get external code for parent of Drawer (Cabinet)

                m_lReturn = g_oBusiness.GetParentExCode(sChildExCode:=sDrawerExCode, sParentExCode:=sCabExCode, lChildFoldLevel:=DOCDrawer)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Failed to get folder " & Strings.Chr(13) & Strings.Chr(10) &
                                    "Drawer : " & sDrawerExCode & Strings.Chr(13) & Strings.Chr(10) &
                                    "Cabinet: " & sCabExCode, "DocuMaster Display Folder", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If

                'construct view from cabinet and drawer info
                frmInterface.ConstructView(sCabExCode:=sCabExCode, sDrawExCode:=sDrawerExCode, sFoldExCode:=sFolderExCode, sDocExCode:="")
            End If


            'Data file command line run
            If sType = "DATAFILE" Then

                ' check if a user is already downloading, if so then exit the whole process
                ' else set to current user and commence download

                m_lReturn = g_oBusiness.BriefcaseUser(sMode:="GET", sUser:=sUser)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to get Briefcase user details from server registry", "Terminating Briefcase Download", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    frmProgress.Close()
                    Return result
                End If

                If sUser <> "" Then
                    MessageBox.Show("'" & sUser & "' is currently downloading from DME" & Strings.Chr(13) & Strings.Chr(10) & "Please try again later when that user has finished", "DME Briefcase Download", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return result
                Else

                    m_lReturn = g_oBusiness.BriefcaseUser(sMode:="SET", sUser:=g_sUserName)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to set Briefcase user details on Server registry", "Terminating Briefcase Download", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    ' this will ensure registry can be reset only by the download user
                    m_sCurrentUser = g_sUserName

                End If
                '

                'dir of file starts from 4th char in command line string
                sCommand = Mid(sCommand, 4)

                sPath = FileSystem.Dir(sCommand, FileAttribute.Normal)

                ' if data file dir is entered wrong in command line then end program
                If sPath = "" Then
                    MessageBox.Show("Briefcase DME manifest file does not exist, terminating program ", " Dir path: " & sCommand, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Set Briefcase download mode to ON in the registry
                ' Ensures the "GetDataPath" function gets the page files from server dir
                ' in order to copy across to briefcase db
                m_lReturn = CType(SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="BRIEFCASEdownload", v_sSettingValue:="ON"), gPMConstants.PMEReturnCode)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to set Briefcase download status to ON in registry ", "Terminating Briefcase Download", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
                'developer guide no. 50
                frmProgress = New frmProgress
                ' set up the progress bar

                'frmProgress.ProgressBar1.Max = 100
                frmProgress.ProgressBar1.Maximum = 100

                'Dim tempLoadForm As frmProgress = frmProgress

                frmProgress.Show()


                frmProgress.ProgressBar1.Value = 5
                frmProgress.lblStatus.Text = "DME Briefcase download..."

                ' prompt user to set dir path for briefcase document store in registry
                m_lReturn = CType(SetBriefCaseDir(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to set Briefcase directory in registry", "Terminating Briefcase Download", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    frmProgress.Close()
                    Return result
                End If



                frmProgress.ProgressBar1.Value = 40
                frmProgress.lblStatus.Text = "Setting up Briefcase database on Server..."

                ' Detach PMBriefcase db on laptop in order to copy it across the Server PC
                m_lReturn = CType(DetachDB(bKillFiles:=CBool("False")), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to transfer Briefcase database from laptop to Server", "Terminating Briefcase Download. Detach failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    frmProgress.Close()
                    Return result
                End If

                ' Connect to Server PC passing Laptop PC Name in order to copy data across to Laptop

                m_lReturn = g_oBusiness.ConnectToLocalDB(sPCName:=m_sLocalPC)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to set up Briefcase database on Server" & Strings.Chr(13) & Strings.Chr(10) &
                                    Strings.Chr(13) & Strings.Chr(10) & "Possible reasons..." & Strings.Chr(13) & Strings.Chr(10) &
                                    Strings.Chr(13) & Strings.Chr(10) & "The Server may be busy or down, re-run Arrivals again" & Strings.Chr(13) & Strings.Chr(10) &
                                    Strings.Chr(13) & Strings.Chr(10) & "MSDE directory (<Drive>:\MSSQL7) must be shared as 'MSSQL7' on your laptop" &
                                    Strings.Chr(13) & Strings.Chr(10) & "Briefcase database files maybe missing on Server or laptop" &
                                    Strings.Chr(13) & Strings.Chr(10) & "Server must have at least 8MB free disk space", "Terminating Briefcase Download", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    frmProgress.Close()
                    Return result
                End If


                iFreeFile = FileSystem.FreeFile()

                FileSystem.FileOpen(iFreeFile, sCommand, OpenMode.Input)


                frmProgress.ProgressBar1.Value = 80
                frmProgress.lblStatus.Text = "Clearing Briefcase database details..."

                ' Clear out all tables in briefcase db and set up defaults

                m_lReturn = g_oBusiness.UpdateLocalDB
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to clear down and set up Briefcase database", "Terminating Briefcase Download", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    frmProgress.Close()
                    Return result
                End If


                frmProgress.ProgressBar1.Value = 100
                frmProgress.lblStatus.Text = "Updating Briefcase database..."

                While Not FileSystem.EOF(iFreeFile)

                    FileSystem.Input(iFreeFile, sLine)

                    If sLine <> "" Then

                        nLineNo += 1

                        'splice the command line it returns into this format, Cabinet|Drawer|Folder|Doc
                        m_lReturn = CType(GetExternalCodes(sLine, sExCodes), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            MessageBox.Show("Failed to extract external code(s) for processing" & Strings.Chr(13) & Strings.Chr(10) &
                                            "Line : " & sLine, "DocuMaster Briefcase Download", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If

                        frmProgress.lblProcess.Text = "processing record " & nLineNo

                        frmProgress.ProgressBar1.Value = 50

                        ' commence download from server to briefcase db
                        '                           Cabinet|Drawer|Folder
                        m_lReturn = CType(DownLoadProcess(sCabExCode:=sExCodes(1), sDrawExCode:=sExCodes(2), sFoldExCode:=sExCodes(3)), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            nErrorTotal += 1

                            If m_sFolderName = "" Then
                                m_sFolderName = "<unavailable>"
                            End If

                            MessageBox.Show("Document download failed for record number: " & nLineNo & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                                            "Folder name: " & m_sFolderName & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                                            "External codes: " & sLine, "DocuMaster Briefcase Download", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If


                        frmProgress.ProgressBar1.Value = 100

                    End If
                End While

                FileSystem.FileClose(iFreeFile)

                frmProgress.lblProcess.Text = ""

                frmProgress.ProgressBar1.Value = 10
                frmProgress.lblStatus.Text = "downloading DME document records..."

                'GENERIC DOWNLOAD

                sErrMsg = "Recoverable errors occured in certain : " & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10)

                ' download document records to briefcase db from server

                m_lReturn = g_oBusiness.UpdateDocuments

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sErrMsg = sErrMsg & "...document records" & Strings.Chr(13) & Strings.Chr(10)
                End If


                ' download linked referenced document records to briefcase db from server

                m_lReturn = g_oBusiness.UpdateLinkedDocs

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sErrMsg = sErrMsg & "...linked document records" & Strings.Chr(13) & Strings.Chr(10)
                End If

                ' download doc_info records to briefcase db from server

                m_lReturn = g_oBusiness.UpdateDocInfoRecs
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sErrMsg = sErrMsg & "...document information records" & Strings.Chr(13) & Strings.Chr(10)
                End If


                frmProgress.ProgressBar1.Value = 40
                frmProgress.lblStatus.Text = "downloading keyword and annotation records..."

                'download doc_keyword records

                m_lReturn = g_oBusiness.UpdateKeywordInfo
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sErrMsg = sErrMsg & "...keyword records"
                End If


                'download annotations records

                m_lReturn = g_oBusiness.UpdateAnnotation
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sErrMsg = sErrMsg & "...annotation records" & Strings.Chr(13) & Strings.Chr(10)
                End If

                ' download page records

                m_lReturn = g_oBusiness.UpdatePageInfo
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sErrMsg = sErrMsg & "...page records" & Strings.Chr(13) & Strings.Chr(10)
                End If


                frmProgress.lblProcess.Text = ""

                frmProgress.ProgressBar1.Value = 60
                frmProgress.lblStatus.Text = "getting page file details from Server..."


                ' get details of all page files from server

                m_lReturn = g_oBusiness.TransferPages(vResultArray:=vPageList, sDataPath:=sDir)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Failed to get certain page file details from Server", "Terminating Briefcase Download", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

                ' copy the pages to briefcase laptop dir

                m_lReturn = CType(CopyPages(vPageArray:=vPageList, sServerPath:=sDir), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Failed to copy page files from Server to Laptop. Re-run Arrivals", "DocuMaster Briefcase Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    frmProgress.Close()
                    Return result
                End If

                ' Download complete, set the registry setting to OFF
                ' Ensures "GetPathData" function gets the briefcase document store
                ' from registry (BriefcaseDir) when using DME normally
                m_lReturn = CType(SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="BRIEFCASEdownload", v_sSettingValue:="OFF"), gPMConstants.PMEReturnCode)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Failed to set Briefcase download status to OFF in registry ", "DocuMaster Briefcase Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If


                ' Delete the PMBriefcase db on laptop
                m_lReturn = CType(DetachDB(bKillFiles:=CBool("True")), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Failed to detach Briefcase database. Re-run Arrivals", "DocuMaster Briefcase Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    frmProgress.Close()
                    Return result
                End If


                frmProgress.ProgressBar1.Value = 90
                frmProgress.lblStatus.Text = "Disconnecting from server and updating database status..."
                'close PMBriefcase down on Server and copy across to laptop

                m_lReturn = g_oBusiness.DisconnectLocalDB(sPCName:=m_sLocalPC)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to copy Briefcase database over from Server to Laptop", "DocuMaster Briefcase Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                ' attach PMBriefcase db on laptop
                m_lReturn = CType(AttachDB(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to Attach Briefcase database on Laptop" & Strings.Chr(13) & Strings.Chr(10) &
                                    "Please re-install your Briefcase setup program ", "DocuMaster Briefcase Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

                ' display erorrs if sections of download errored
                If sErrMsg.Length > 45 Then
                    MessageBox.Show(sErrMsg, "DocuMaster Briefcase Download", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                If nErrorTotal > 0 Then
                    MessageBox.Show("There were " & nErrorTotal & " recoverable error(s) in " & CStr(nLineNo) & " record(s) read ", "DocuMaster Briefcase Download", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If



                frmProgress.ProgressBar1.Value = 100
                frmProgress.lblStatus.Text = "Download completed from Server to Laptop"
                MessageBox.Show("To run Sirius Briefcase from your Laptop, log off Sirius" & Strings.Chr(13) & Strings.Chr(10) &
                                "and re-log on using the 'My Computer' option ", "DME Briefcase download completed", MessageBoxButtons.OK)


                'close down progress bar
                frmProgress.Close()

                'minimise Documater
                frmInterface.WindowState = FormWindowState.Minimized

            End If 'sType = "DATAFILE"


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="BriefCaseProcess", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result


        End Try
    End Function


    ' ***************************************************************** '
    ' Name:         DownLoadProcess
    '
    ' Description: Main download process. Reads folders passed and downloads
    '              all related data from server db
    '
    ' *****************************************************************
    Public Function DownLoadProcess(ByRef sCabExCode As String, ByRef sDrawExCode As String, ByRef sFoldExCode As String) As Integer

        Dim result As Integer = 0
        Dim lParentNum, lDrawerNum As Integer
        Dim vResultArray(,) As Object
        Dim vChildArray(,) As Object
        Dim vTempArray(,) As Object
        Dim vGeneralArray(,) As Object
        Dim iRow As Integer

        On Error GoTo Err_DownLoadProcess

        result = gPMConstants.PMEReturnCode.PMTrue


        frmProgress.ProgressBar1.Value = 60
        frmProgress.lblStatus.Text = "Downloading company/client/folder details..."

        If sCabExCode.Trim() <> "" Then
            ' Get Cabinet folder record from server. Cabinet does not have a parent

            m_lReturn = g_oBusiness.GetFolderRec(sExCode:=sCabExCode, lFolderLevel:=DOCCabinet, lParentNum:=0, vResultArray:=vResultArray)

            'store folder name for using in erorr msg in calling procedure
            If Information.IsArray(vResultArray) Then

                m_sFolderName = CStr(vResultArray(1, 0))
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GoTo Err_Handler
            End If

            'Insert into briefcase db

            m_lReturn = g_oBusiness.InsertFolderRec(vArray:=vResultArray, iRow:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GoTo Err_Handler
            End If

        End If

        ' Get Drawer record from server. Cabinet folder_num (above) being the Parent
        If (sDrawExCode.Trim() <> "") And (Information.IsArray(vResultArray)) Then

            'lParentNum = folder.folder_num i.e the Cabinet above

            lParentNum = CInt(vResultArray(0, 0))


            m_lReturn = g_oBusiness.GetFolderRec(sExCode:=sDrawExCode, lFolderLevel:=DOCDrawer, lParentNum:=lParentNum, vResultArray:=vResultArray)

            'store folder name for using in erorr msg in calling procedure
            If Information.IsArray(vResultArray) Then

                m_sFolderName = CStr(vResultArray(1, 0))
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GoTo Err_Handler
            End If

            'store drawer folder_num

            lDrawerNum = CInt(vResultArray(0, 0))
            'Insert into briefcase db

            m_lReturn = g_oBusiness.InsertFolderRec(vArray:=vResultArray, iRow:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GoTo Err_Handler
            End If

        End If


        ' NOTE: The folder code in the command line  may not be supplied
        ' This is what will happen...

        ' If Cabinet & Drawer & Folder details were passed then get all child pertaining to
        ' Folder (the code below will run and set the parent to Folder)
        ' the code after it will get all child folders

        ' If Cabinet & Drawer  codes were supplied only then all child folders under
        ' drawer is copied over



        frmProgress.ProgressBar1.Value = 70
        frmProgress.lblStatus.Text = "downloading folder details..."

        If (sFoldExCode.Trim() <> "") And (Information.IsArray(vResultArray)) Then


            lParentNum = CInt(vResultArray(0, 0)) ' lParentNum = folder.parent_num
            'get folder rec from server

            m_lReturn = g_oBusiness.GetFolderRec(sExCode:=sFoldExCode, lFolderLevel:=DOCFolder, lParentNum:=lParentNum, vResultArray:=vResultArray)


            'store folder name for using in erorr msg in calling procedure
            If Information.IsArray(vResultArray) Then

                m_sFolderName = CStr(vResultArray(1, 0))
            End If

            ' the Server does not have this record so mark it as faulty
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vResultArray) Then
                'Insert into briefcase db


                m_lReturn = g_oBusiness.InsertFolderRec(vArray:=vResultArray, iRow:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GoTo Err_Handler
                End If
            End If
        End If


        frmProgress.ProgressBar1.Value = 80
        frmProgress.lblStatus.Text = "downloading child folders..."

        ' Copy all child folders for folder (or for drawer if folder was not supplied)

        ' link folder.folder_num with folder.parent_num
        If Information.IsArray(vResultArray) Then


            m_lReturn = g_oBusiness.GetChildFolder(lFolderNum:=CInt(vResultArray(0, 0)), vArray:=vResultArray)
            ' recursively get folders of all child folders
            If Information.IsArray(vResultArray) Then

                m_lReturn = g_oBusiness.FolderRecursiveCopy(vFolderArray:=vResultArray)

                'store folder name for using in erorr msg in calling procedure
                If Information.IsArray(vResultArray) Then

                    m_sFolderName = CStr(vResultArray(1, 0))
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GoTo Err_Handler
                End If

            End If
        End If


        ' Ensure GENERAL folder for client is copied over

        If lDrawerNum > 0 Then


            m_lReturn = g_oBusiness.GetGeneralFolder(lDrawerNum:=lDrawerNum, vResultArray:=vGeneralArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GoTo Err_Handler
            End If

            If Information.IsArray(vGeneralArray) Then
                'Insert into briefcase db

                m_lReturn = g_oBusiness.InsertFolderRec(vArray:=vGeneralArray, iRow:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GoTo Err_Handler
                End If
            End If

        End If

        Return result


Err_DownLoadProcess:

        result = gPMConstants.PMEReturnCode.PMFalse

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="DownLoadProcess", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result


Err_Handler:
        MessageBox.Show("Errors occured due to one or more of the following reasons" & Strings.Chr(13) & Strings.Chr(10) &
                        "Class: iDOCManager" & Strings.Chr(13) & Strings.Chr(10) &
                        "Method: DownLoadProcess" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                        "1. DME Record does not exist on the Server" & Strings.Chr(13) & Strings.Chr(10) &
                        "2. Failed to read details from Server" & Strings.Chr(13) & Strings.Chr(10) &
                        "3. Failed to write to Briefcase database" & Strings.Chr(13) & Strings.Chr(10) &
                        "   See 'Sirius Message Viewer' and log file on Server", "DocuMaster Briefcase Download", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Return result

    End Function


    ' ***************************************************************** '
    ' Name: SetBriefCaseDir
    '
    ' Description: Set briefcase dir path in registry where briefcase should
    '              store all page documents
    '
    ' ***************************************************************** '
    Public Function SetBriefCaseDir() As Integer

        Dim result As Integer = 0
        Dim sDir As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' GET Briefcase dir store (BriefcaseDir)
            m_lReturn = CType(GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="BriefcaseDir", r_sSettingValue:=sDir), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' use the current dir
            If sDir > "" Then
                If MessageBox.Show("Dir: " & sDir & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                                   "Store DocuMaster documents in this directory ? ", "Set Briefcase local document path", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then

                    ' user is happy, we can exit
                    Return result
                End If
            End If

            ' no registry value exists so SET a new registry value via a browser input

            ' reset dir
            sDir = ""
            ' display dir browser
            Do While (sDir.Trim()) = ""

                m_lReturn = BrowseFolder(sFolder:=sDir, sTitle:="Please set a directory on your local drive " &
                            Strings.Chr(13) & Strings.Chr(10) & "where Briefcase should store all documents to", hWndParent:=0)

                ' confirm dir path
                If sDir > "" Then
                    If MessageBox.Show("Dir :    " & sDir & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                                       "Are you sure? ", "Confirm DocuMaster Briefcase document directory setting", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then
                        ' user is happy with setting
                        Exit Do
                    Else
                        ' reset sDir to get it again
                        sDir = ""
                    End If
                End If

            Loop

            ' set Briefcase dir store in registry (BriefcaseDir key to dir of sDir)
            m_lReturn = CType(SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="BriefcaseDir", v_sSettingValue:=sDir), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="SetBriefCaseDir", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyPages
    '
    ' Description: Copy document page files from Server to laptop
    '
    ' ***************************************************************** '
    Public Function CopyPages(ByRef vPageArray(,) As Object, ByRef sServerPath As String) As Integer


        Dim result As Integer = 0
        Dim iRow, iLastSlash As Integer
        Dim sSourceFile, sDestinationFile, sDestPath, sBriefcaseDir, sPageDir, sPath, sFullDir, sDriveLetter As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'No pages found so exit
            If Not Information.IsArray(vPageArray) Then
                Return result
            End If

            ' dir path for DME documents  is not set up correctly as a share on the server
            If CBool(CStr(Not sServerPath.StartsWith("\\")).Trim()) Then
                MessageBox.Show("Please set the correct DME data directory on the Server and re-run Arrivals" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & " Server DME data dir:   " & sServerPath & Strings.Chr(13) & Strings.Chr(10) &
                                " The directory must be a share in this format '\\<Servername>\..\.. '", "Fatal Error: Server DME directory path is invalid", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' set local dir where the server pages are to be copied
            ' GET Briefcase dir store (BriefcaseDir)
            m_lReturn = CType(GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="BriefcaseDir", r_sSettingValue:=sBriefcaseDir), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Construct path for each page (assume all on same volume for now)
            For I As Integer = vPageArray.GetLowerBound(1) To vPageArray.GetUpperBound(1)


                sSourceFile = sServerPath & CStr(vPageArray(0, I)).Trim() &
                              "." & CStr(vPageArray(1, I)).Trim()


                sDestinationFile = CStr(vPageArray(0, I)).Trim() &
                                   "." & CStr(vPageArray(1, I)).Trim()

                If Not sDestinationFile.StartsWith("\") Then
                    sDestinationFile = "\" & sDestinationFile
                End If

                ' make the directory of briefcase db (to be copied to) same as page_name (incs. dir)
                ' on server the actual dir ends at the penultimate slash after which is the actual
                ' page (file) name

                ' page.page_name which includes the dir struct which is xx\xx\xx\xx\<page name|no>

                sPageDir = CStr(vPageArray(0, I))

                'start from the last pos and work backwards to find the last slash
                For iInd As Integer = sPageDir.Length To 1 Step -1
                    If Mid(sPageDir, iInd, 1) = "\" Then
                        iLastSlash = iInd
                        Exit For
                    End If
                Next iInd

                'set sPageDir to just the dir whilst omitting the file name
                sPageDir = Mid(sPageDir, 1, iLastSlash)

                'NOW BUILD DIR PATH (as specified in sPageDir if it is not there)

                sFullDir = sBriefcaseDir & sPageDir
                ' check for drive letter
                If sFullDir.Substring(1, 1) = ":" Then
                    ' we have one so get it
                    sDriveLetter = sFullDir.Substring(0, 2)
                    ' Remove the drive letter
                    sFullDir = sFullDir.Substring(sFullDir.Length - (sFullDir.Length - 2))
                Else
                    ' we dont have one, so make it from the current drive
                    sDriveLetter = Directory.GetCurrentDirectory().Substring(0, 2)
                End If

                ' loop through the path now, and create the dirs (i.e  00\00\00\00 tree expected)
                sPath = ""

                For iInd As Integer = 1 To sFullDir.Length

                    sPath = sFullDir.Substring(0, iInd)
                    sPath = sDriveLetter & sPath

                    If sPath.EndsWith("\") Then
                        ' reached the end of a dir, make the path so far
                        If Not Directory.Exists(sPath) Then
                            ' only if it doesnt exist of course...
                            Directory.CreateDirectory(sPath)
                        End If
                    End If

                Next iInd

                ' make the directory again, incase there was no \ on the end of the path
                If Not Directory.Exists(sPath) Then
                    Directory.CreateDirectory(sPath)
                End If

                'kill the file is it already exists
                If FileSystem.Dir(sBriefcaseDir & sDestinationFile, FileAttribute.Normal) > "" Then
                    File.Delete(sBriefcaseDir & sDestinationFile)
                End If

                ' Copy file from server to briefcase hd
                File.Copy(sSourceFile, sBriefcaseDir & sDestinationFile)


            Next I ' Array loop

            Return result

        Catch excep As System.Exception




            ' file not found
            If Information.Err().Number = 53 Then

                Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Next Statement")
            End If

            ' the operating system couldn't make a connection between the path and the file name.
            If Information.Err().Number = 75 Then

                Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Next Statement")
            End If

            ' permission denied i.e another user is using it
            If Information.Err().Number = 70 Then
                MessageBox.Show("Could not copy file '" & sSourceFile & "'" & Strings.Chr(13) & Strings.Chr(10) &
                                "This file may be in use by another user/program" & Strings.Chr(13) & Strings.Chr(10) &
                                "Please re-run Arrivals again when the file is free", "DME Briefcase copy, permission denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Next Statement")
            End If

            ' path not found, directory/file missing from server!
            If Information.Err().Number = 76 Then

                Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Next Statement")
            End If

            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPages", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function



    Public Function DetachDB(ByRef bKillFiles As Boolean) As Integer
        Dim result As Integer = 0
        Dim SQLDMO As Object

        Dim sMSDEPath, sSQLServerName, sPassword As String
        Dim lProcessID As Integer
        Dim dAppID As Double

        Dim oSQLServer As SQLDMO.SQLServer = New SQLDMO.SQLServer()

        On Error GoTo Err_DetachDB

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the laptop's MSDE (SQLServer7) dir path for where data files are stored
        sMSDEPath = ReadRegistry(gpmConstants.HKEY_LOCAL_MACHINE, "SOFTWARE\Microsoft\MSSQLServer\MSSQLServer\Parameters", "SQLArg0")
        If sMSDEPath = "Not Found" Then
            sMSDEPath = ""
        End If

        If sMSDEPath.Substring(0, Math.Min(sMSDEPath.Length, 2)) = "-d" Then
            sMSDEPath = sMSDEPath.Substring(sMSDEPath.Length - (sMSDEPath.Length - 2))
        End If

        Do While sMSDEPath.Length > 0
            If sMSDEPath.EndsWith("\") Then
                Exit Do
            End If
            sMSDEPath = sMSDEPath.Substring(0, sMSDEPath.Length - 1)
        Loop

        sSQLServerName = "(local)"
        sPassword = ""


Login_Resume:

        result = gPMConstants.PMEReturnCode.PMTrue


        ' Log onto the SQL Server. If service not started then this will run again

        oSQLServer.Connect(sSQLServerName, "sa", sPassword)


        If bKillFiles Then
            ' delete files that already exist in MSDE before copying
            If FileSystem.Dir(sMSDEPath & m_cDataFile, FileAttribute.Normal) <> "" Then
                File.Delete(sMSDEPath & m_cDataFile)
            End If

            If FileSystem.Dir(sMSDEPath & m_cLogFile, FileAttribute.Normal) <> "" Then
                File.Delete(sMSDEPath & m_cLogFile)
            End If

        Else

            ' if PMBriefcase db exists then detach it

            If IsThere(oSQLServer.Databases, m_cPMBriefcase) Then

                oSQLServer.DetachDB(m_cPMBriefcase, True)

            Else
                ' do nothing as it is already detached
            End If
        End If


        Return result

Err_DetachDB:


        If Information.Err().Number = -2147221504 Then

            ' MSDE service was not started so start that and resume
            ' DAK150600 - net start doesn't work on Windows 95


            dAppID = Process.Start("net start MSSQLServer").Id

            dAppID = Process.Start(sMSDEPath & "\Binn\scm.exe -Action 1 -Silent 1").Id
            If dAppID = 0 Then
                GoTo Err_DetachDB
            End If
            'wait for process to end
            ProcessEnd(CInt(dAppID))
            Resume Login_Resume


        ElseIf Information.Err().Number = -2147204362 Then


            dAppID = Process.Start(sMSDEPath & "\Binn\sqlmangr.exe").Id
            If dAppID = 0 Then
                GoTo Err_DetachDB
            End If
            ProcessEnd(CInt(dAppID))
            Resume Login_Resume

        Else
            '
        End If

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DetachDBFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DetachDB", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function


    Public Function AttachDB() As Integer
        Dim result As Integer = 0
        Dim SQLDMO As Object

        Dim sMSDEPath, sSQLServerName, sPassword As String
        Dim lProcessID As Integer
        Dim dAppID As Double

        Dim oSQLServer As SQLDMO.SQLServer = New SQLDMO.SQLServer()
        Dim bPathExists As Boolean

        On Error GoTo Err_AttachDB

        result = gPMConstants.PMEReturnCode.PMTrue


        'Get the SQL Data path
        sMSDEPath = ReadRegistry(gpmConstants.HKEY_LOCAL_MACHINE, "SOFTWARE\Microsoft\MSSQLServer\MSSQLServer\Parameters", "SQLArg0")
        If sMSDEPath = "Not Found" Then
            sMSDEPath = ""
        End If

        If sMSDEPath.Substring(0, Math.Min(sMSDEPath.Length, 2)) = "-d" Then
            sMSDEPath = sMSDEPath.Substring(sMSDEPath.Length - (sMSDEPath.Length - 2))
        End If

        Do While sMSDEPath.Length > 0
            If sMSDEPath.EndsWith("\") Then
                Exit Do
            End If
            sMSDEPath = sMSDEPath.Substring(0, sMSDEPath.Length - 1)
        Loop


        sSQLServerName = "(local)"
        sPassword = ""


Login_Resume:

        result = gPMConstants.PMEReturnCode.PMTrue


        ' Log onto the SQL Server. If service not started then this will run again

        oSQLServer.Connect(sSQLServerName, "sa", sPassword)

        ' if already PMBriefcase exists then exit sub

        If IsThere(oSQLServer.Databases, m_cPMBriefcase) Then
            Return result
        End If

        ' create Briefcase db on the Server by attaching files

        'ensure the files exist in order to attach them
        bPathExists = True

        If FileSystem.Dir(sMSDEPath & m_cDataFile, FileAttribute.Normal) = "" Then
            bPathExists = False
        End If

        If FileSystem.Dir(sMSDEPath & m_cLogFile, FileAttribute.Normal) = "" Then
            bPathExists = False
        End If

        If bPathExists Then

            oSQLServer.AttachDB(m_cPMBriefcase, sMSDEPath & m_cDataFile & ", " &
                                sMSDEPath & m_cLogFile)

        Else
            MessageBox.Show("Briefcase database files do not exist in MSSQL data directory!" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                            "Please RE-INSTALL the SIRIUS BRIEFCASE setup program", "DME Briefcase download Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return gPMConstants.PMEReturnCode.PMFalse

        End If


        Return result

Err_AttachDB:


        If Information.Err().Number = -2147221504 Then

            ' MSDE service was not started so start that and resume
            ' DAK150600 - net start doesn't work on Windows 95


            dAppID = Process.Start("net start MSSQLServer").Id

            dAppID = Process.Start(sMSDEPath & "\Binn\scm.exe -Action 1 -Silent 1").Id
            If dAppID = 0 Then
                GoTo Err_AttachDB
            End If
            'wait for process to end
            ProcessEnd(CInt(dAppID))
            Resume Login_Resume


        ElseIf Information.Err().Number = -2147204362 Then


            dAppID = Process.Start(sMSDEPath & "\Binn\sqlmangr.exe").Id
            If dAppID = 0 Then
                GoTo Err_AttachDB
            End If
            ProcessEnd(CInt(dAppID))
            Resume Login_Resume

        Else
            '
        End If

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AttachDBFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="AttachDB", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    Private Function IsThere(ByRef cObjects As Object, ByRef sName As String) As Boolean
        Dim result As Boolean = False
        Dim oObject As Object




        For Each oObject2 As Object In cObjects
            oObject = oObject2


            If oObject.Name.ToUpper() = sName.ToUpper() Then
                Return True
            End If

        Next oObject2

        oObject = Nothing

        Return result

    End Function



    ' ***************************************************************** '
    ' Name: ProcessEnd(lProcessID as long)
    '
    ' Description:
    ' this loops around until a process whose id has been passed across
    ' is no longer existent
    ' ***************************************************************** '
    Public Sub ProcessEnd(ByRef lProcessID As Integer)
        Dim lReturn, lCnt As Integer

        Try

            'Give the process a chance to start
            Pause(2)

            Do

                Interaction.AppActivate(CStr(lProcessID))

                Pause(1)

                lCnt += 1

                If (lCnt / 900) = Math.Floor(lCnt / 900) Then
                    'If this has been ongoing for more than 15 minutes we'd
                    'better warn the user
                    MessageBox.Show("Process (" & lProcessID & " )still exists", Application.ProductName)
                End If

            Loop

        Catch



            'Process has ended so we can go back
            Exit Sub
        End Try


    End Sub


    ' ***************************************************************** '
    ' Name: Pause
    '
    ' Description: Waits for n seconds
    '
    ' ***************************************************************** '
    Private Sub Pause(ByRef Seconds As Single)


        Dim StartTime As Single = DateTime.Now.TimeOfDay.TotalSeconds



        While DateTime.Now.TimeOfDay.TotalSeconds < StartTime + Seconds

            Application.DoEvents()

        End While


    End Sub

    ' ***************************************************************** '
    ' Name: SBODisplayClient
    '
    ' Description:
    '       Display the drawer or folder details i.e. client or policy
    '       (requested by SBO via the command line prompt)
    '
    ' ***************************************************************** '

    Public Function SBODisplayClient(ByRef sCommand As String, Optional ByRef bCalledViaActivate As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim sFolderName, sFolderExCode, sDrawerExCode, sCabExCode As String
        Dim lFolderNum, lDrawerNum, lFolderLevel As Integer
        Dim sDrawerName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' format of parameter =  SBO<foldername><folderlevel>
            ' First 3 chars indicate calling program i.e. SBO, subsequent chars
            ' used for folder name (until penultimate char), last char is the folder level.

            ' extract folder name, starts from 4th char, ends at penultimate char
            sFolderName = Mid(sCommand, 4)
            ' then trim last char off
            sFolderName = sFolderName.Substring(0, sFolderName.Length - 1)

            ' now extract folder level, it's the last char
            lFolderLevel = CInt(sCommand.Substring(sCommand.Length - 1))


            '
            '   FOLDER VIEW
            '

            ' find the folder details if folder passed
            If lFolderLevel = DOCFolder Then

                ' get folder ex_code and folder_num, use folder_name as key

                m_lReturn = g_oBusiness.GetSBOClient(sFolderName:=sFolderName, lFolderLevel:=DOCFolder, sExCode:=sFolderExCode, lFolderNum:=lFolderNum)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Folder does not exist... " & Strings.Chr(13) & Strings.Chr(10) &
                                    Strings.Chr(13) & Strings.Chr(10) & "Folder    : " & sFolderName &
                                    Strings.Chr(13) & Strings.Chr(10) & "FolderNum : " & CStr(lFolderNum) &
                                    Strings.Chr(13) & Strings.Chr(10) & "Ex code   : " & sFolderExCode, "DocuMaster Folder Display", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If


                ' get parent of folder, i.e. drawer

                m_lReturn = g_oBusiness.GetParentFolder(lFolderLevel:=DOCFolder, lFolderNum:=lFolderNum, lParentNum:=lDrawerNum)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Parent Folder does not exist... " & Strings.Chr(13) & Strings.Chr(10) &
                                    Strings.Chr(13) & Strings.Chr(10) & "Parent Folder Number  : " & CStr(lDrawerNum) &
                                    Strings.Chr(13) & Strings.Chr(10) & "Child Folder Number   : " & CStr(lFolderNum), "DocuMaster Folder Display", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If


                ' pass in drawer number, get ex_code for drawer and cabinet

                m_lReturn = g_oBusiness.GetDrawerCabExCode(lDrawerNum:=lDrawerNum, sDrawerExCode:=sDrawerExCode, sDrawerName:=sDrawerName, sCabExCode:=sCabExCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    Interaction.MsgBox("Folder does not exist... " & Strings.Chr(13) & Strings.Chr(10) &
                                       Strings.Chr(13) & Strings.Chr(10) & "Drawer    : " & sDrawerName &
                                       Strings.Chr(13) & Strings.Chr(10) & "Num       : " & CStr(lDrawerNum) &
                                       Strings.Chr(13) & Strings.Chr(10) & "Ex code   : " & sDrawerExCode, Strings.Chr(13) & Strings.Chr(10) & "Cabinet Ex code   : " & sDrawerExCode, CStr(MsgBoxStyle.Critical))
                    Return result
                End If

                frmInterface.bReloadmode = bCalledViaActivate
                ' construct folder view
                frmInterface.ConstructView(sCabExCode:=sCabExCode, sDrawExCode:=sDrawerExCode, sFoldExCode:=sFolderExCode, sDocExCode:="")
                frmInterface.bReloadmode = False
            End If


            '
            ' DRAWER VIEW
            '

            ' find the drawer details if folder passed
            If lFolderLevel = DOCDrawer Then

                ' get drawer ex_code and folder_num, use folder_name as key

                m_lReturn = g_oBusiness.GetSBOClient(sFolderName:=sFolderName, lFolderLevel:=DOCDrawer, sExCode:=sDrawerName, lFolderNum:=lDrawerNum)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Folder does not exist... " & Strings.Chr(13) & Strings.Chr(10) &
                                    Strings.Chr(13) & Strings.Chr(10) & "Folder    : " & sFolderName &
                                    Strings.Chr(13) & Strings.Chr(10) & "FolderNum : " & CStr(lFolderNum) &
                                    Strings.Chr(13) & Strings.Chr(10) & "Ex code   : " & sFolderExCode, "DocuMaster Folder Display", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If

                ' pass in drawer number, get ex_code for drawer and cabinet

                m_lReturn = g_oBusiness.GetDrawerCabExCode(lDrawerNum:=lDrawerNum, sDrawerExCode:=sDrawerExCode, sDrawerName:=sDrawerName, sCabExCode:=sCabExCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    Interaction.MsgBox("Folder does not exist... " & Strings.Chr(13) & Strings.Chr(10) &
                                       Strings.Chr(13) & Strings.Chr(10) & "Drawer    : " & sDrawerName &
                                       Strings.Chr(13) & Strings.Chr(10) & "Num       : " & CStr(lDrawerNum) &
                                       Strings.Chr(13) & Strings.Chr(10) & "Ex code   : " & sDrawerExCode, Strings.Chr(13) & Strings.Chr(10) & "Cabinet Ex code   : " & sDrawerExCode, CStr(MsgBoxStyle.Critical))
                    Return result
                End If

                frmInterface.bReloadmode = bCalledViaActivate
                ' construct view for drawer
                frmInterface.ConstructView(sCabExCode:=sCabExCode, sDrawExCode:=sDrawerExCode, sFoldExCode:="", sDocExCode:="")
                frmInterface.bReloadmode = False
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SBODisplayClientFAILED", vApp:=ACApp, vClass:=ACClass, vMethod:="SBODisplayClient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SBOViewDocument
    '
    ' Description:
    '
    '      Constructs the folder view in respect to where the document resides in,
    '      Fires off iDocViewer and displays the document (task request from SBO)
    '
    '
    ' ***************************************************************** '

    Public Sub SBOViewDocument()

        Dim lDocNum, lNodeNum, lDrawerNum, lFolderNum, lParentNum As Integer
        Dim sFilename, sDocName, sDocType, sDocExCode, sExCode, sDrawerExCode, sCabExCode, sDrawerName, sPassword As String
        Dim vPageArray As Object
        Dim bZipped As Boolean
        Dim dCreateDate As Date
        Dim oZipper, oViewer As Object
        Dim iFolderLevel, iAccessLevel, I As Integer
        Dim sSBOMessage As String = ""

        Try

            sSBOMessage = "The document you are trying to view is currently inside a non Sirius Back-Office folder." & Strings.Chr(13) & Strings.Chr(10) &
                          "You cannot view this document unless it is moved into a  Back-Office Sirius folder" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                          "Document location and details:" & Strings.Chr(13) & Strings.Chr(10)



            m_lReturn = g_oBusiness.GetDocInfo(lDocNum:=m_lDocNum, sDocName:=sDocName, lFolderNum:=lNodeNum, sDocExCode:=sDocExCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get document details, doc_num : " & m_lDocNum, vApp:=ACApp, vClass:=ACClass, vMethod:="SBOViewDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            'default the external code to the doc number (ExCode is actually the Doc Number)
            If sDocExCode.Trim() = "" Then
                sDocExCode = CStr(m_lDocNum)
            End If

            ' get document's folder details in order to expand the folder view

            m_lReturn = g_oBusiness.GetFolderInformation(lNodeNum:=lNodeNum, sExCode:=sExCode, iFolderLevel:=iFolderLevel, iAccessLevel:=iAccessLevel, sPassword:=sPassword, dCreateDate:=dCreateDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to document details, doc_num : " & lNodeNum, vApp:=ACApp, vClass:=ACClass, vMethod:="SBOViewDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            ' construct the folders and fire off iDocViewer displaying document

            Select Case iFolderLevel
                ' expand just the cabinet level
                Case DOCCabinet

                    ' Oh no, folder is not an SBO external folder so inform user and exit
                    If sExCode.Trim() = "" Then
                        MessageBox.Show(sSBOMessage &
                                        "Cabinet folder does not have a SBO external code." & Strings.Chr(13) & Strings.Chr(10) &
                                        "Folder Number: " & CStr(lNodeNum) & Strings.Chr(13) & Strings.Chr(10) &
                                        "Document Ex code/Number : " & sDocExCode & Strings.Chr(13) & Strings.Chr(10) &
                                        "Document Name : " & sDocName, "DocuMaster Folder Display", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                        Exit Sub
                    End If

                    frmInterface.ConstructView(sCabExCode:=sExCode, sDrawExCode:="", sFoldExCode:="", sDocExCode:=sDocExCode)


                    'expand cabinet and drawer level
                Case DOCDrawer

                    ' pass in drawer number, get ex_code for drawer and cabinet

                    m_lReturn = g_oBusiness.GetDrawerCabExCode(lDrawerNum:=lNodeNum, sDrawerExCode:=sDrawerExCode, sDrawerName:=sDrawerName, sCabExCode:=sCabExCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Client folder does not exist... " & Strings.Chr(13) & Strings.Chr(10) &
                                        "Folder Name : " & sDrawerName & Strings.Chr(13) & Strings.Chr(10) &
                                        "Number : " & CStr(lDrawerNum) & Strings.Chr(13) & Strings.Chr(10) &
                                        "ExCode : " & sDrawerExCode & Strings.Chr(13) & Strings.Chr(10) &
                                        "Co. ExCode : " & sDrawerExCode, "DocuMaster Folder Display", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If

                    ' Oh no, folder is not an SBO external folder so inform user and exit
                    If sDrawerExCode.Trim() = "" Then
                        MessageBox.Show(sSBOMessage &
                                        "Client folder does not have an SBO external code." & Strings.Chr(13) & Strings.Chr(10) &
                                        "Folder Number: " & CStr(lNodeNum) & Strings.Chr(13) & Strings.Chr(10) &
                                        "Client Folder Name : " & sDrawerName & Strings.Chr(13) & Strings.Chr(10) &
                                        "Document Ex code/Number : " & sDocExCode & Strings.Chr(13) & Strings.Chr(10) &
                                        "Document Name : " & sDocName, "DocuMaster Folder Display", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End If

                    ' construct cabinet view
                    frmInterface.ConstructView(sCabExCode:=sCabExCode, sDrawExCode:=sDrawerExCode, sFoldExCode:="", sDocExCode:=sDocExCode)

                    'expand cabinet, drawer and folder level
                Case DOCFolder

                    ' using folder details, get the drawer info

                    m_lReturn = g_oBusiness.GetParentFolder(lFolderLevel:=DOCFolder, lFolderNum:=lNodeNum, lParentNum:=lDrawerNum)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        MessageBox.Show("Parent Folder does not exist... " & Strings.Chr(13) & Strings.Chr(10) &
                                        Strings.Chr(13) & Strings.Chr(10) & "Parent Folder Number : " & CStr(lDrawerNum) &
                                        Strings.Chr(13) & Strings.Chr(10) & "Child Folder Number : " & CStr(lFolderNum), "DocuMaster Folder Display", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If


                    ' pass in drawer number, get ex_code for drawer and cabinet

                    m_lReturn = g_oBusiness.GetDrawerCabExCode(lDrawerNum:=lDrawerNum, sDrawerExCode:=sDrawerExCode, sDrawerName:=sDrawerName, sCabExCode:=sCabExCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Folder does not exist... " & Strings.Chr(13) & Strings.Chr(10) &
                                        Strings.Chr(13) & Strings.Chr(10) & "Name : " & sDrawerName &
                                        Strings.Chr(13) & Strings.Chr(10) & "Number : " & CStr(lDrawerNum) &
                                        Strings.Chr(13) & Strings.Chr(10) & "Ex Code : " & sDrawerExCode &
                                        Strings.Chr(13) & Strings.Chr(10) & "Co. Ex Code : " & sCabExCode, "DocuMaster Folder Display", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If

                    ' Cabinet is not an SBO external folder so inform user and exit
                    If sDrawerExCode.Trim() = "" Or sExCode.Trim() = "" Then
                        MessageBox.Show(sSBOMessage &
                                        "Folder does not have an SBO external code." & Strings.Chr(13) & Strings.Chr(10) &
                                        "Folder Number: " & CStr(lNodeNum) & Strings.Chr(13) & Strings.Chr(10) &
                                        "Document Ex Code/Number : " & sDocExCode & Strings.Chr(13) & Strings.Chr(10) &
                                        "Document Name : " & sDocName, "DocuMaster Folder Display", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End If

                    ' construct folder view
                    frmInterface.ConstructView(sCabExCode:=sCabExCode, sDrawExCode:=sDrawerExCode, sFoldExCode:=sExCode, sDocExCode:=sDocExCode)
                Case Else

                    ' nothing to do here

            End Select

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SBOViewDocumentFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SBOViewDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub



    ' The Calling application or component name.
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            m_lPMAuthorityLevel = Value

        End Set
    End Property


    ' ***************************************************************** '
    ' The following properties need to be set by the                    '
    ' component so that Navigator can tell what happened.               '
    ' ***************************************************************** '

    ' The user status of the form on exit
    ' i.e. PMOK, PMCancel or PMNavigate.
    ' Business objects will normally just set this to PMOK,
    ' unless they want the ability to adjust the route through a process.
    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property


    ' ***************************************************************** '
    ' The following methods are called by Navigator BEFORE              '
    ' the component is told to Start its job.                           '
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Name: SetKeys (Navigator Standard Method)
    '
    ' Description: Accepts an Array in the format KeyName, KeyValue.
    '              The array will contain the key values required by the
    '              component to do its job.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}


                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case "TaskDescription"

                        m_lDocNum = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                End Select

                ' {* USER DEFINED CODE (End) *}
            Next lRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' The following methods are called by Navigator AFTER               '
    ' the component has done its job.                                   '
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Name: GetKeys (Navigator Standard Method)
    '
    ' Description: Accepts an Array in the format KeyName, KeyValue.
    '              The component populates the array with
    '              key values. i.e. If the component is
    '              FindParty it will return the PartyCnt of the Party
    '              selected by the user.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer



        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vKeyArray(1, 0)

            ' Assign the key array with the parameter members.
            '    vKeyArray(PMKeyName, 0) = PMKeyNameQuickQuoteInProgress
            '    vKeyArray(PMKeyValue, 0) = m_lQuickQuoteInProgress

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.

            ' Assign the key array with the parameter members.

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetSummary (Navigator Standard Method)
    '
    ' Description: Accepts an Array in the format Summary Level, Summary
    '              Heading, Summary Value.
    '
    '              The component populates the array with any
    '              summary information it wants to return to Navigator.
    '
    '              There are three levels of Summary, Process,
    '              Map Instance and Map.
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer

        Try


            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: SetProcessModes (Navigator Standard Method)
    '
    ' Description: Sets the mode of operation for the Component.
    '              The properties are described individually above.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try


            '''    ' Assign the process modes to the property members.
            '''
            '''    If (IsMissing(vTask) = False) Then
            '''       '' m_iTask% = CInt(vTask)
            '''    End If
            '''
            '''    If (IsMissing(vNavigate) = False) Then
            '''        m_lNavigate& = CLng(vNavigate)
            '''    End If
            '''
            '''    If (IsMissing(vProcessMode) = False) Then
            '''        m_lProcessMode& = CLng(vProcessMode)
            '''    End If
            '''
            '''    If (IsMissing(vTransactionType) = False) Then
            '''        m_sTransactionType$ = CStr(vTransactionType)
            '''    End If
            '''
            '''    If (IsMissing(vEffectiveDate) = False) Then
            '''        m_dtEffectiveDate = CDate(vEffectiveDate)
            '''    End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Function ProcessArchive(ByRef sCommand As String) As Integer

        Dim result As Integer = 0
        Dim sDocKey As String = ""
        Dim bInDocList As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        ' frmInterface.SetViewModeFindResults()

        'Set up the controls positions, according to the two splitter bars

        ' the 8th char onward is the document Key
        sDocKey = Mid(sCommand, 8)

        bInDocList = False
        'Go thru doc list and select the document passes and unselect any others

        'developer guid no. 49
        ' For I As Integer = 1 To frmInterface.lvwDocList.ListItems.Count
        For I As Integer = 0 To frmInterface.lvwDocList.Items.Count - 1
            'developer guid no. 49
            If frmInterface.lvwDocList.Items(I).Name = sDocKey Then
                frmInterface.lvwDocList.Items(I).Selected = True
                bInDocList = True
                Exit For
            End If

        Next I

        'can only archive if the doc is in the doc list of the current folder
        If Not bInDocList Then

            MessageBox.Show("You can only archive documents found in the current documents window" & Environment.NewLine &
                            "The document resides elsewhere, select correct folder and try again" & Environment.NewLine &
                            "(the Document Viewer provides the document's folder details on it's title bar)", "Document Archive", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

            Return result

        End If

        m_lReturn = CType(frmInterface.ArchiveDocument(), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            MessageBox.Show("Cannot run frmInterface.ArchiveDocument", "DocuMaster Document Archive Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return result
        End If

        Return result

    End Function
    Public Function PrintPDFs(ByVal sFilename As String) As Integer
        Dim Error282Count As Integer
        Dim AcroDDEFailed As Boolean
        Const Max282Errors As Integer = 6
        Try
            '' Count of "Can't open DDE channel" errors '' Set to true if a DDE connection cannot be established
            Dim sCmd As String = "" '' DDE command
            Dim lStatus As Integer '' response from ShellExecute command '' Number of times we will ignore "Can't open DDE channel" errors
            '' before accepting the fact that Acrobat is not started. We need
            '' to test more than once, because it might just be busy loading
            Dim sAcroPath As String = "" '' Path to acrobat, determined by ShellExecute
            Dim bCloseAcrobat As Boolean '' If we open acrobat, we will close it when we are done

            '' If acrobat is already running (and hidden), shelling it will cause it to be shown.
            '' We do not want that. So try a DDE connect, which will fail if acrobat is not running
            '' I have looked at other API means of testing this, but it may be running as a process (no window)
            Error282Count = Max282Errors '' we only need to try once to see if it is already running.
            AcroDDEFailed = False '' ErrHandler will set to true if Acro is not running
            'TODO: Needs to check for LinkMode,linktopic etc

            'frmInterface.txtAcrobatDDE.LinkMode = 0 '' Close any current DDE Link
            '
            'frmInterface.txtAcrobatDDE.LinkTopic = "acroview|control" '' Acrobat's DDE Application|Topic
            '
            'frmInterface.txtAcrobatDDE.LinkMode = 2 '' Try to establish 'Manual' DDE Link. This will fail
            ' '' if Acrobat is not ready (or in this case, not running)

            If AcroDDEFailed Then
                '' We could not set our linkmode, so Acro is not running. Find it and launch it
                '' Use the ShellExecute API function to grab the path to our PDF handler.
                '' This should be Acrobat Reader or Acrobat, but it might be something else.
                '' When we try to DDE link to it, non-acrobat will error out. This is ok.
                sAcroPath = New String(Strings.Chr(32), 128)
                lStatus = FindExecutable(sFilename, Nothing, sAcroPath)
                If lStatus <= 32 Then
                    MessageBox.Show("Acrobat could not be found on this computer. Printing cancelled", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Function
                End If

                '' Launch the PDF handler

                Dim startInfo As ProcessStartInfo = New ProcessStartInfo(sAcroPath)
                startInfo.WindowStyle = ProcessWindowStyle.Hidden
                lStatus = CInt(Process.Start(startInfo).Id)
                If (lStatus >= 0) And (lStatus <= 32) Then
                    MessageBox.Show("An error occured launching Acrobat. Printing cancelled", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Function
                End If
                bCloseAcrobat = True '' We will try to close Acrobat when we are done
            End If

            Pause(2) '' Lets take a break here to let Acrobat finish loading

            Error282Count = 0 '' This time, we will allow all acceptable tries, as
            AcroDDEFailed = False '' Acrobat is running, but may be busy loading its modules

            'frmInterface.txtAcrobatDDE.LinkMode = 0
            '
            'frmInterface.txtAcrobatDDE.LinkTopic = "acroview|control"
            '
            'frmInterface.txtAcrobatDDE.LinkTimeout = 2500 ' 3 minute timeout delay. Should be moer than enough
            '
            'frmInterface.txtAcrobatDDE.LinkMode = 2

            If AcroDDEFailed Then
                MessageBox.Show("An error occured connecting to Acrobat. Printing cancelled", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Function
            End If

            '' Send the PDF's to the printer. In my testing, this was very immediate
            sCmd = "[FilePrintSilent(" & Strings.Chr(34).ToString() & sFilename & Strings.Chr(34).ToString() & ")]"

            'frmInterface.txtAcrobatDDE.LinkExecute(sCmd)


            If bCloseAcrobat Then
                '' [AppExit()] causes memory errors with v6.0 and 6.1, so avoid closing these versions
                If (sAcroPath.IndexOf("6.0") + 1) = 0 Then
                    sCmd = "[AppExit()]"

                    'frmInterface.txtAcrobatDDE.LinkExecute(sCmd)
                End If
            End If

            '' Close the DDE Connection

            'frmInterface.txtAcrobatDDE.LinkMode = 0

        Catch


            If Information.Err().Number = 282 Then '' Can't open DDE channel
                '' This error may happen because Acro is not fully loaded.
                '' Give it Max282Errors attempts before returning AcroDDEFailed = True
                Error282Count += 1
                If Error282Count <= Max282Errors Then
                    Pause(3)

                    Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")
                Else
                    AcroDDEFailed = True

                    Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Next Statement")
                End If
            End If
        End Try

    End Function
End Class
