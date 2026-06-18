Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms

Imports SharedFiles
Partial Friend Class frmDebug
    Inherits System.Windows.Forms.Form
    Private Const ACClass As String = "frmDebug"

    ' RoadmapPath
    Private m_sRoadmapPath As String = ""

    ' XMLFileName
    Private m_sXMLFileName As String = ""

    Public Property XMLFileName() As String
        Get
            Return m_sXMLFileName
        End Get
        Set(ByVal Value As String)
            m_sXMLFileName = Value
        End Set
    End Property

    Friend Property RoadmapPath() As String
        Get
            Return m_sRoadmapPath
        End Get
        Set(ByVal Value As String)
            m_sRoadmapPath = Value
        End Set
    End Property

    ' ***************************************************************** '
    '
    ' Name: RefreshDebug
    '
    ' Description: Refreshes the debug information
    '
    ' History: 18/09/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function RefreshDebug() As Integer
        Dim result As Integer = 0
        Dim sText As String = ""
        Dim strTypeName As String = ""
        Dim lstItem As ListViewItem
        Dim sKey As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Current step
            lblStep.Text = CStr(m_lCurrentStep) & " - " & m_vSteps(m_lCurrentStep).Description

            ' Clear the keys list
            lvwKeys.Items.Clear()

            ' Keys
            If Information.IsArray(m_vKeyArray) Then
                For iLoop1 As Integer = 0 To m_vKeyArray.GetUpperBound(1)

                    sKey = "K" & iLoop1

                    sText = CStr(m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1))

                    ' Add an entry
                    lstItem = lvwKeys.Items.Add(sKey, sText, "")

                    'If Information.IsReference(m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)) Then
                    strTypeName = TypeName(m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                    If strTypeName = "Object" Or strTypeName = "Object()" Or strTypeName = "Object[]" Then
                        sText = "(Object)"
                    ElseIf (Information.IsArray(m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))) Then
                        sText = "*** Warning : (Array) ***"
                    Else
                        sText = Convert.ToString(m_vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                    End If

                    ' Set the value
                    ListViewHelper.GetListViewSubItem(lstItem, 1).Text = sText

                Next iLoop1

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshDebug Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshDebug", excep:=excep)

            Return result

            Return result
        End Try
    End Function

    Private Sub cmdClose_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClose.Click

        Me.Hide()

    End Sub

    ' ***************************************************************** '
    '
    ' Name: ShowLog
    '
    ' Description: Shows the sirius log file
    '
    ' History: 08/07/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ShowLog() As Integer
        Dim result As Integer = 0
        Dim m_lReturn As Integer

        Dim sLogFile As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Read the location of the log file from the registry
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="LogFileName", r_sSettingValue:=sLogFile)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Return false
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowLog Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowLog", excep:=New Exception(Information.Err().Description))
                Return result
            End If

            ' CTAF - Ideally this should use the default viewer...
            m_lReturn = ShellExecute(hwnd:=Me.Handle.ToInt32(), lpOperation:="OPEN", lpFile:="notepad", lpParameters:=sLogFile, lpDirectory:="C:\", nShowCmd:=1)
            ' We could check m_lReturn here but its not a proper return value
            ' so there's no point...

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowLog Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowLog", excep:=excep)

            Return result
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessMenu
    '
    ' Description:
    '
    ' History: 19/07/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessMenu() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display the menu
            'Ctx_mnuFile.Show(Me, 0, 0)
            'PointToClient(Cursor.Position).X
            Ctx_mnuFile.Show(Me, cmdShow.Left + cmdShow.Width, PointToClient(Cursor.Position).Y)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessMenu Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessMenu", excep:=excep)

            Return result
            Return result
        End Try
    End Function
    Private Sub cmdShow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdShow.Click
        Dim m_lReturn As Integer

        ' Show the popup menu
        m_lReturn = ProcessMenu()

    End Sub
    Private Sub frmDebug_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Move to the top right
        Me.Top = 0
        Me.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Screen.PrimaryScreen.Bounds.Width) - VB6.PixelsToTwipsX(Me.Width) - 80)
        'Me.Height = Screen.Height - 200

    End Sub

    Private Sub frmDebug_Paint(ByVal eventSender As Object, ByVal eventArgs As PaintEventArgs) Handles MyBase.Paint

        'SetTopmost Me

    End Sub

    Private Sub frmDebug_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        'ClearTopmost Me

        eventArgs.Cancel = Cancel <> 0
    End Sub

    Private Sub lvwKeys_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwKeys.MouseUp
        Dim Button As Integer = CInt(2)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If Button = MouseButtonConstants.RightButton Then
            Ctx_mnuEntry.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)
        End If

    End Sub

    Public Sub mnuEntryCopy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuEntryCopy.Click
        Dim m_lReturn As Integer

        m_lReturn = CopyClipboard()

    End Sub

    ' ***************************************************************** '
    '
    ' Name: CopyClipboard
    '
    ' Description:
    '
    ' History: 18/09/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function CopyClipboard() As Integer

        Dim result As Integer = 0
        Dim sText As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If lvwKeys.FocusedItem Is Nothing Then
                Return result
            End If

            sText = lvwKeys.FocusedItem.SubItems(1).Text

            ' Clear the clipboard
            My.Computer.Clipboard.Clear()
            ' Add the new entry

            My.Computer.Clipboard.SetText(sText, TextDataFormat.Text)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyClipboard Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyClipboard", excep:=excep)

            Return result
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ShowRootXMLFile
    '
    ' Description: Displays the root xml file in the default browser
    '
    ' History: 19/07/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ShowRootXMLFile() As Integer
        Dim result As Integer = 0
        Dim m_lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Open the XML file
            m_lReturn = ShellExecute(hwnd:=Me.Handle.ToInt32(), lpOperation:="open", lpFile:=m_sRoadmapPath & m_sXMLFileName, lpParameters:="", lpDirectory:=m_sRoadmapPath, nShowCmd:=1)
            ' No point checking m_lReturn as its nothing useful

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowRootXMLFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRootXMLFile", excep:=excep)

            Return result
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: BrowseXMLFolder
    '
    ' Description: Opens explorer at the folder with the XML files in
    '
    ' History: 19/07/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function BrowseXMLFolder() As Integer
        Dim result As Integer = 0
        Dim m_lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Open the XML file
            m_lReturn = ShellExecute(hwnd:=Me.Handle.ToInt32(), lpOperation:="open", lpFile:=m_sRoadmapPath, lpParameters:="", lpDirectory:=m_sRoadmapPath, nShowCmd:=1)
            ' No point checking m_lReturn as its nothing useful

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BrowseXMLFolder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BrowseXMLFolder", excep:=excep)

            Return result
            Return result
        End Try
    End Function
    Public Sub mnuFileExplore_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileExplore.Click
        Dim m_lReturn As Integer

        m_lReturn = BrowseXMLFolder()

    End Sub

    Public Sub mnuFileRootXML_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileRootXML.Click
        Dim m_lReturn As Integer

        m_lReturn = ShowRootXMLFile()

    End Sub

    Public Sub mnuFileSiriusLog_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileSiriusLog.Click
        Dim m_lReturn As Integer

        ' Show the log file
        m_lReturn = ShowLog()

    End Sub
End Class