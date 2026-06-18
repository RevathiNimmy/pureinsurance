VERSION 5.00
Begin VB.Form FLogin 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Login"
   ClientHeight    =   3375
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   5535
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
   Icon            =   "Login.frx":0000
   KeyPreview      =   -1  'True
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3375
   ScaleWidth      =   5535
   ShowInTaskbar   =   0   'False
   StartUpPosition =   2  'CenterScreen
   Begin VB.CheckBox chkWindowsNT 
      Caption         =   "Your Windows Login Account"
      Height          =   255
      Left            =   1320
      TabIndex        =   2
      Top             =   2400
      Width           =   2895
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      CausesValidation=   0   'False
      Height          =   375
      Left            =   1560
      TabIndex        =   4
      Top             =   2880
      Width           =   1215
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   120
      TabIndex        =   3
      Top             =   2880
      Width           =   1215
   End
   Begin VB.TextBox txtPassword 
      Height          =   285
      IMEMode         =   3  'DISABLE
      Left            =   1320
      PasswordChar    =   "*"
      TabIndex        =   1
      Top             =   1680
      Width           =   2895
   End
   Begin VB.TextBox txtUserName 
      Height          =   285
      Left            =   1320
      TabIndex        =   0
      Top             =   1320
      Width           =   2895
   End
   Begin VB.Label lblLabel 
      BackStyle       =   0  'Transparent
      Caption         =   "OR"
      Height          =   255
      Index           =   3
      Left            =   1320
      TabIndex        =   8
      Top             =   2070
      Width           =   495
   End
   Begin VB.Label lblPassword 
      BackStyle       =   0  'Transparent
      Caption         =   "Password"
      Height          =   255
      Left            =   120
      TabIndex        =   7
      Top             =   1680
      Width           =   1215
   End
   Begin VB.Label lblUserName 
      BackStyle       =   0  'Transparent
      Caption         =   "User Name"
      Height          =   255
      Left            =   120
      TabIndex        =   6
      Top             =   1320
      Width           =   1215
   End
   Begin VB.Image imgIcon 
      Height          =   480
      Left            =   120
      Picture         =   "Login.frx":000C
      Top             =   120
      Width           =   480
   End
   Begin VB.Label lblLabel 
      BackStyle       =   0  'Transparent
      Caption         =   $"Login.frx":0163
      Height          =   1095
      Index           =   0
      Left            =   840
      TabIndex        =   5
      Top             =   120
      UseMnemonic     =   0   'False
      Width           =   4575
   End
End
Attribute VB_Name = "FLogin"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Form:     SQL Server Login dialog
' Shared:   No
'
Option Explicit

Private m_bTrusted As Boolean
Private m_sUserName As String
Private m_sPassword As String

Private m_bOKPressed As Boolean

Public Function Dialog(ByVal frmParent As Form, _
    ByVal sServerName As String, _
    ByRef r_bTrusted As Boolean, _
    ByRef r_sUserName As String, _
    ByRef r_sPassword As String) As Boolean

    If DebugLogging Then
        WriteToLog "DEBUG: FLogin.Dialog"
    End If

    r_bTrusted = False
    r_sUserName = ""
    r_sPassword = ""

    Caption = "Connect to " & sServerName
    LoadAllControls

    m_bOKPressed = False
    Show vbModal, frmParent
    Dialog = m_bOKPressed

    If m_bOKPressed Then
        r_bTrusted = m_bTrusted
        r_sUserName = m_sUserName
        r_sPassword = m_sPassword
    End If

End Function

Private Sub chkWindowsNT_Click()

    EnableControls chkWindowsNT.Value = vbUnchecked

End Sub

Private Sub cmdCancel_Click()

    Unload Me

End Sub

Private Sub cmdOK_Click()

    If Not ValidateAllControls() Then Exit Sub

    SaveAllControls

    m_bOKPressed = True
    Unload Me

End Sub

Private Sub Form_KeyDown(KeyCode As Integer, Shift As Integer)

    ' Sirius standard debug popup.
    If KeyCode = vbKeyF12 And Shift = vbCtrlMask Then
        MsgBox MMain.DebugMessagePopup(), vbInformation
    End If

End Sub

Private Sub LoadAllControls()

    ' Set defaults.
    txtUserName.Text = "sa"
    chkWindowsNT.Value = vbUnchecked

End Sub

Private Sub SaveAllControls()

    m_bTrusted = (chkWindowsNT.Value = vbChecked)
    If m_bTrusted Then
        m_sUserName = ""
        m_sPassword = ""
    Else
        m_sUserName = txtUserName.Text
        m_sPassword = txtPassword.Text
    End If

End Sub

Private Function ValidateAllControls() As Boolean

    ValidateAllControls = False

    If chkWindowsNT.Value = vbUnchecked Then
        If txtUserName.Text = "" Then
            MsgBox "A valid login name must be specified.", vbExclamation, Caption
            txtUserName.SetFocus
            Exit Function
        ElseIf txtUserName.Text = ksSALoginName Then
            MsgBox "The Sirius login name cannot be used to connect for this purpose.", vbExclamation, Caption
            txtUserName.SetFocus
            Exit Function
        End If
    End If

    ValidateAllControls = True

End Function

Private Sub EnableControls(ByVal bEnabled As Boolean)

    lblUserName.Enabled = bEnabled
    txtUserName.Enabled = bEnabled
    lblPassword.Enabled = bEnabled
    txtPassword.Enabled = bEnabled

End Sub
