VERSION 5.00
Begin VB.Form FError 
   AutoRedraw      =   -1  'True
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Error"
   ClientHeight    =   4455
   ClientLeft      =   360
   ClientTop       =   960
   ClientWidth     =   8415
   ClipControls    =   0   'False
   Icon            =   "Error.frx":0000
   MaxButton       =   0   'False
   MinButton       =   0   'False
   MousePointer    =   1  'Arrow
   ScaleHeight     =   4455
   ScaleWidth      =   8415
   ShowInTaskbar   =   0   'False
   StartUpPosition =   2  'CenterScreen
   Begin VB.TextBox txtMoreInfo 
      BackColor       =   &H8000000F&
      ForeColor       =   &H80000012&
      Height          =   1935
      Left            =   120
      Locked          =   -1  'True
      MultiLine       =   -1  'True
      ScrollBars      =   2  'Vertical
      TabIndex        =   7
      Top             =   2400
      Width           =   6855
   End
   Begin VB.CommandButton cmdStartSystemInfo 
      Caption         =   "&System Info"
      Height          =   375
      Left            =   7080
      TabIndex        =   9
      Top             =   2880
      Width           =   1215
   End
   Begin VB.CommandButton cmdStartDiagnostics 
      Caption         =   "&Diagnostics"
      Height          =   375
      Left            =   7080
      TabIndex        =   8
      Top             =   2400
      Width           =   1215
   End
   Begin VB.CommandButton cmdInfo 
      Caption         =   "&More Info >>"
      Height          =   375
      Left            =   7080
      TabIndex        =   5
      Top             =   1680
      Width           =   1215
   End
   Begin VB.CommandButton cmdIgnoreAll 
      Caption         =   "Ignore &All"
      Height          =   375
      Left            =   3720
      TabIndex        =   3
      Top             =   1680
      Width           =   1095
   End
   Begin VB.CommandButton cmdIgnore 
      Caption         =   "&Ignore"
      Height          =   375
      Left            =   2520
      TabIndex        =   2
      Top             =   1680
      Width           =   1095
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   4920
      TabIndex        =   4
      Top             =   1680
      Width           =   1095
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   375
      Left            =   1320
      TabIndex        =   1
      Top             =   1680
      Width           =   1095
   End
   Begin VB.CommandButton cmdRetry 
      Caption         =   "&Retry"
      Height          =   375
      Left            =   120
      TabIndex        =   0
      Top             =   1680
      Width           =   1095
   End
   Begin VB.TextBox txtErrorMessage 
      BackColor       =   &H8000000F&
      BorderStyle     =   0  'None
      ForeColor       =   &H80000012&
      Height          =   975
      Left            =   720
      Locked          =   -1  'True
      MultiLine       =   -1  'True
      TabIndex        =   6
      Top             =   120
      Width           =   7575
   End
   Begin VB.Label lblStandardMessage 
      Height          =   495
      Left            =   720
      TabIndex        =   10
      Top             =   1080
      UseMnemonic     =   0   'False
      Width           =   7575
   End
End
Attribute VB_Name = "FError"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Form:     Error handler dialog box
' Shared:   Yes (RESTRICTED)
'
' THIS CODE MODULE IS NOW SHARED DIRECTLY WITH THE SIRIUS DATABASE
' INSTALLER PROJECT, DUE TO ITS UNIQUE REQUIREMENT FOR NO VB DLL
' REFERENCES. ALL SWIFT CODE SHOULD REFERENCE THIS DLL AS NORMAL.
'
Option Explicit

' Window dimensions
Const klHeightLessInfo = 2550
Const klHeightMoreInfo = 4830

Private m_sAppTitle As String
Private m_sAppVersion As String
Private m_sDiagnosticsProgram As String
Private m_sSystemInfoProgram As String
Private m_sStandardMessage As String
Private m_bShowMoreInfo As Boolean
Private m_bDiagnostics As Boolean
Private m_sEmailToName As String
Private m_sEmailToAddress As String
Private m_sEmailCcName As String
Private m_sEmailCcAddress As String

Private m_nResult As EErrorResults
Private m_nOrgMousePointer As Integer
Private m_sEmailDateTime As String

Private m_bJustLoaded As Boolean

#If SiriusDatabaseInstaller Then
Private m_bKeyChordCancel As Boolean
Private m_bKeyChordIgnore As Boolean
Private m_bKeyChordIgnoreAll As Boolean
#End If

' Displays the error form and returns the user response.
Public Function Dialog(ByVal sMessage As String, _
    ByVal sErrorsCollection As String, _
    ByVal bCritical As Boolean, _
    ByVal nButtons As EErrorButtons, _
    ByVal sTitle As String, _
    ByVal sLogInfo As String, _
    ByVal sAppTitle As String, _
    ByVal sAppVersion As String, _
    ByVal sDiagnosticsProgram As String, _
    ByVal sSystemInfoProgram As String, _
    ByVal sStandardMessage As String, _
    ByVal bShowMoreInfo As Boolean, _
    ByVal bDiagnostics As Boolean, _
    ByVal sEmailToName As String, _
    ByVal sEmailToAddress As String, _
    ByVal sEmailCcName As String, _
    ByVal sEmailCcAddress As String) As EErrorResults

    Dim bRetryVisible As Boolean
    Dim bCancelVisible As Boolean
    Dim bIgnoreVisible As Boolean
    Dim bIgnoreAllVisible As Boolean
    Dim bOKVisible As Boolean

    On Error GoTo EH_Handler
    m_nOrgMousePointer = Screen.MousePointer
    Screen.MousePointer = vbDefault

    ' Save data passed in.
    m_sAppTitle = sAppTitle
    m_sAppVersion = sAppVersion
    m_sDiagnosticsProgram = sDiagnosticsProgram
    m_sSystemInfoProgram = sSystemInfoProgram
    m_sStandardMessage = sStandardMessage
    m_bShowMoreInfo = bShowMoreInfo
    m_bDiagnostics = bDiagnostics
    m_sEmailToName = sEmailToName
    m_sEmailToAddress = sEmailToAddress
    m_sEmailCcName = sEmailCcName
    m_sEmailCcAddress = sEmailCcAddress

    ' Get the exact date/time that the error occurred.
    ' Force the date and time into the following formats because
    ' they are used for emailing and the recipient would want dates
    ' and times to be consistent rather than dependent on the
    ' sender's Control Panel settings.
    m_sEmailDateTime = Format$(Now(), """on"" dddd d mmmm yyyy ""at"" h:mm am/pm")

    ' Form Caption
    Caption = sTitle

    ' Determine what bits of text to put on screen.
    txtErrorMessage = sMessage
    lblStandardMessage = m_sStandardMessage
    txtMoreInfo = sErrorsCollection & sLogInfo

    ' Display the correct icon on the form. We ask Windows
    ' to draw the icon instead of providing one ourselves.
    If bCritical Then
        DrawStandardIcon hDC, 8, 8, knIconError
    Else
        DrawStandardIcon hDC, 8, 8, knIconExclamation
    End If

    ' Determine which form buttons should be visible.
    bRetryVisible = False
    bCancelVisible = False
    bIgnoreVisible = False
    bIgnoreAllVisible = False
    bOKVisible = False
    Select Case nButtons
    Case ebOK
        bOKVisible = True
        cmdOK.Left = cmdRetry.Left
    Case ebRetryCancelIgnore
        bRetryVisible = True
        bCancelVisible = True
        bIgnoreVisible = True
    Case ebRetryCancelIgnoreIgnoreAll
        bRetryVisible = True
        bCancelVisible = True
        bIgnoreVisible = True
        bIgnoreAllVisible = True
    Case Else ' Retry/Cancel
        bRetryVisible = True
        bCancelVisible = True
    End Select

    #If SiriusDatabaseInstaller Then
        ' If certain buttons were requested but have been globally disabled, then
        ' make them available via hidden key chords and a password dialog instead.
        m_bKeyChordCancel = False
        m_bKeyChordIgnore = False
        m_bKeyChordIgnoreAll = False
        If Not MMain.AllowUserToCancelDialog Then
            If bCancelVisible Then
                bCancelVisible = False
                m_bKeyChordCancel = True
            End If
        End If
        If Not MMain.AllowUserToIgnore Then
            If bIgnoreVisible Then
                bIgnoreVisible = False
                m_bKeyChordIgnore = True
            End If
            If bIgnoreAllVisible Then
                bIgnoreAllVisible = False
                m_bKeyChordIgnoreAll = True
            End If
        End If
        If m_bKeyChordCancel Or m_bKeyChordIgnore Or m_bKeyChordIgnoreAll Then
            Randomize
        End If
        KeyPreview = True ' always need to recognise Ctrl+F12
        AdjustFormAppearance
    #End If

    cmdRetry.Visible = bRetryVisible
    cmdCancel.Visible = bCancelVisible
    cmdIgnore.Visible = bIgnoreVisible
    cmdIgnoreAll.Visible = bIgnoreAllVisible
    cmdOK.Visible = bOKVisible

    ' Determine if the buttons in the extra information section are visible.
    cmdStartDiagnostics.Visible = (m_bDiagnostics And Len(m_sDiagnosticsProgram) <> 0)
    cmdStartSystemInfo.Visible = (m_bDiagnostics And Len(m_sSystemInfoProgram) <> 0)

    ' Don't display 'More Info' button if there are no more options or information.
    If Len(txtMoreInfo.Text) = 0 And _
        Not cmdStartDiagnostics.Visible And _
        Not cmdStartSystemInfo.Visible Then
        cmdInfo.Visible = False
    End If

    Height = klHeightLessInfo
    
    Show vbModal
    Dialog = m_nResult

    Screen.MousePointer = m_nOrgMousePointer
    Exit Function

EH_Handler:
    SimpleInternalError "A program error has occurred, but the error information cannot be displayed."
    MousePointer = vbDefault
    Exit Function

End Function

Private Sub cmdCancel_Click()

    m_nResult = erCancel
    Unload Me
    
End Sub

Private Sub cmdIgnore_Click()

    m_nResult = erIgnore
    Unload Me
    
End Sub

Private Sub cmdIgnoreAll_Click()

    m_nResult = erIgnoreAll
    Unload Me
    
End Sub

Private Sub cmdInfo_Click()

    If Height = klHeightLessInfo Then
        cmdInfo.Caption = "<< &Less Info"
        Height = klHeightMoreInfo
    Else
        cmdInfo.Caption = "&More Info >>"
        Height = klHeightLessInfo
    End If

End Sub

Private Sub cmdOK_Click()

    m_nResult = erOK
    Unload Me

End Sub

Private Sub cmdRetry_Click()

    m_nResult = erRetry
    Unload Me

End Sub

Private Sub cmdStartDiagnostics_Click()

    RunDiagnostics

End Sub

Private Sub cmdStartSystemInfo_Click()

    RunSystemInfo

End Sub

Private Sub Form_Activate()

    If m_bJustLoaded Then
        ' Press the "More Info" button automatically if required.
        If m_bShowMoreInfo And cmdInfo.Visible Then
            cmdInfo = True
        End If
        m_bJustLoaded = False
    End If

End Sub

#If SiriusDatabaseInstaller Then
Private Sub Form_KeyDown(KeyCode As Integer, Shift As Integer)

    ' Sirius standard debug popup.
    If KeyCode = vbKeyF12 And Shift = vbCtrlMask Then
        MsgBox MMain.DebugMessagePopup(), vbInformation
    End If

End Sub
#End If

#If SiriusDatabaseInstaller Then
Private Sub Form_KeyPress(KeyAscii As Integer)

    Select Case KeyAscii
    Case 3 ' Ctrl+C
        If m_bKeyChordCancel Then
            If CheckPassword("Cancel This Error") Then
                cmdCancel_Click
            End If
        End If
    Case 9 ' Ctrl+I
        If m_bKeyChordIgnore Then
            If CheckPassword("Ignore This Error") Then
                cmdIgnore_Click
            End If
        End If
    Case 1 ' Ctrl+A
        If m_bKeyChordIgnoreAll Then
            If CheckPassword("Ignore All Errors") Then
                cmdIgnoreAll_Click
            End If
        End If
    End Select

End Sub
#End If

Private Sub Form_Load()

    ' Turn off the window's close button.
    WinEnableClose Me, False

    m_bJustLoaded = True

End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)

    ' Don't allow the user to click on the close box on the
    ' title bar, only via one of the command buttons.
    If UnloadMode = vbFormControlMenu Then
        Cancel = True
    End If

End Sub

#If SiriusDatabaseInstaller Then
Private Function CheckPassword(ByVal sTitle As String) As Boolean

    Const ksPromptInput = "To proceed with this action, please enter " & _
        "the password corresponding to the seed value %sv%." & vbCrLf & _
        vbCrLf & _
        "WARNING - This password must be entered within about the next " & _
        "7-8 minutes or it will cease to be valid."

    Const ksPromptError = "Incorrect password. Requested action cannot be " & _
        "performed."

    Dim nSeed As Long
    Dim nPassword As Long
    Dim sPassword As String

    On Error GoTo EH_Handler
    CheckPassword = False

    sTitle = sTitle & " (Version " & knPasswordAlgorithmVersion & ")"
    nSeed = CLng(Rnd() * 1000000#)
    nPassword = GeneratePassword(nSeed)

    sPassword = InputBox(Replace(ksPromptInput, "%sv%", nSeed), sTitle, "")

    If sPassword = "" Then
        ' User pressed cancel, so abort without an error.
    ElseIf ToLong(sPassword) = nPassword Then
        CheckPassword = True
    Else
        MsgBox ksPromptError, vbExclamation, sTitle
    End If
    Exit Function

EH_Handler:
    SimpleInternalError "Cannot check the password for validity."
    Exit Function

End Function
#End If

#If SiriusDatabaseInstaller Then
Private Sub AdjustFormAppearance()

    Dim ctrl As Control

    ' Purely for appearance's sake, change the font to Tahoma.
    On Error Resume Next

    Me.Font.Name = "Tahoma"

    For Each ctrl In Me.Controls
        Select Case TypeName(ctrl)
        Case "Label", "TextBox", "CommandButton"
            ctrl.Font.Name = "Tahoma"
        End Select
    Next

End Sub
#End If

Private Sub RunDiagnostics()

    On Error GoTo EH_Handler
    MousePointer = vbHourglass

    StartExecutable m_sDiagnosticsProgram

    MousePointer = vbDefault
    Exit Sub

EH_Handler:
    SimpleInternalError "Cannot start the Swift Diagnostics program."
    MousePointer = vbDefault
    Exit Sub

End Sub

Private Sub RunSystemInfo()

    On Error GoTo EH_Handler
    MousePointer = vbHourglass

    StartExecutable m_sSystemInfoProgram

    MousePointer = vbDefault
    Exit Sub

EH_Handler:
    SimpleInternalError "Cannot start the Microsoft System Info program."
    MousePointer = vbDefault
    Exit Sub

End Sub

Private Sub SimpleInternalError(ByVal sMessage As String)

    ' Always display a message box because the user interface
    ' has to be enabled if we're executing form code!
    MsgBox sMessage & vbCrLf & vbCrLf & _
        Trim$(RemoveChars(Err.Description, vbCrLf)) & " (" & Err.Number & ") (" & Err.Source & ")", _
        vbExclamation, Caption

End Sub
