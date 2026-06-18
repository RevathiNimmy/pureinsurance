VERSION 5.00
Begin VB.Form FICCS 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "ICCS Number"
   ClientHeight    =   2055
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   6030
   ClipControls    =   0   'False
   BeginProperty Font 
      Name            =   "Tahoma"
      Size            =   8.25
      Charset         =   0
      Weight          =   400
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   Icon            =   "ICCS.frx":0000
   KeyPreview      =   -1  'True
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2055
   ScaleWidth      =   6030
   ShowInTaskbar   =   0   'False
   StartUpPosition =   2  'CenterScreen
   Begin VB.TextBox txtICCS 
      Height          =   285
      Left            =   840
      MaxLength       =   4
      TabIndex        =   0
      Top             =   960
      Width           =   1215
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   120
      TabIndex        =   1
      Top             =   1560
      Width           =   1215
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      CausesValidation=   0   'False
      Height          =   375
      Left            =   1560
      TabIndex        =   2
      Top             =   1560
      Width           =   1215
   End
   Begin VB.Image Image1 
      Height          =   480
      Left            =   120
      Picture         =   "ICCS.frx":000C
      Top             =   120
      Width           =   480
   End
   Begin VB.Label lblLabel 
      BackStyle       =   0  'Transparent
      Caption         =   $"ICCS.frx":044E
      Height          =   735
      Index           =   0
      Left            =   840
      TabIndex        =   3
      Top             =   120
      UseMnemonic     =   0   'False
      Width           =   5055
   End
End
Attribute VB_Name = "FICCS"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Form:     ICCS Number dialog
' Shared:   No
'
Option Explicit

Private m_sICCS As String

Public Function Dialog(ByVal frmParent As Form) As String

    If DebugLogging Then
        WriteToLog "DEBUG: FICCS.Dialog"
    End If

    m_sICCS = ""
    Show vbModal, frmParent
    Dialog = m_sICCS

End Function

Private Sub cmdCancel_Click()

    If PromptToExitApp() Then
        Unload Me
    End If

End Sub

Private Sub cmdOK_Click()

    If Not ValidateAllControls() Then Exit Sub

    m_sICCS = txtICCS.Text
    Unload Me

End Sub

Private Sub Form_KeyDown(KeyCode As Integer, Shift As Integer)

    ' Sirius standard debug popup.
    If KeyCode = vbKeyF12 And Shift = vbCtrlMask Then
        MsgBox MMain.DebugMessagePopup(), vbInformation
    End If

End Sub

Private Sub Form_Load()

    ' Turn off the window's close button.
    WinEnableClose Me, False

    cmdCancel.Visible = MMain.AllowUserToCancelDialog

End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)

    If Not MMain.AllowUserToCancelDialog Then
        ' Although we have disabled the close box and hidden the cancel button,
        ' the user could still unload the form by pressing Alt+F4. This catches
        ' that as well.
        If UnloadMode = vbFormControlMenu Then
            Cancel = True
        End If
    End If

End Sub

Private Sub txtICCS_KeyPress(KeyAscii As Integer)

    Select Case KeyAscii
    Case Is < vbKeySpace, vbKey0 To vbKey9
        ' valid keypress
    Case Else
        ' invalid keypress
        KeyAscii = 0
        Beep
    End Select

End Sub

Private Function ValidateAllControls() As Boolean

    Const ksMessage2 = _
        "This ICCS number is only meant for use on internal Pure installations for " & _
        "development and testing purposes. If you are installing at a customer site, " & _
        "please enter the correct number that has been issued by Pure Support. " & _
        "Failure to do so will cause future data transfers and upgrades to fail, " & _
        "and may cause unexpected behaviour in parts of the software. " & _
        "Are you sure that you want to install with this number?"

    ValidateAllControls = False

    If txtICCS.Text = "" Then
        MsgBox "A valid ICCS number must be specified.", vbExclamation, Caption
        txtICCS.SetFocus
        Exit Function
    ElseIf txtICCS.Text = ksInternalICCS Then
        If MsgBox(ksMessage2, vbExclamation + vbYesNo + vbDefaultButton2, Caption) = vbNo Then
            txtICCS.SetFocus
            Exit Function
        End If
    End If

    ValidateAllControls = True

End Function
