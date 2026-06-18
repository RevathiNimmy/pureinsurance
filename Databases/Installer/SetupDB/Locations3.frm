VERSION 5.00
Begin VB.Form FLocations3 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Create Database In"
   ClientHeight    =   2655
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   7695
   BeginProperty Font 
      Name            =   "Tahoma"
      Size            =   8.25
      Charset         =   0
      Weight          =   400
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   Icon            =   "Locations3.frx":0000
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2655
   ScaleWidth      =   7695
   ShowInTaskbar   =   0   'False
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      CausesValidation=   0   'False
      Height          =   375
      Left            =   1560
      TabIndex        =   6
      Top             =   2160
      Width           =   1215
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   120
      TabIndex        =   5
      Top             =   2160
      Width           =   1215
   End
   Begin VB.ComboBox cboDataFilesFolder 
      Height          =   315
      Left            =   840
      TabIndex        =   0
      Top             =   360
      Width           =   6255
   End
   Begin VB.CommandButton cmdDataFilesFolder 
      Height          =   330
      Left            =   7200
      Picture         =   "Locations3.frx":000C
      Style           =   1  'Graphical
      TabIndex        =   1
      ToolTipText     =   "Browse for a file folder"
      Top             =   360
      Width           =   375
   End
   Begin VB.CommandButton cmdLogFilesFolder 
      Height          =   330
      Left            =   7200
      Picture         =   "Locations3.frx":00A3
      Style           =   1  'Graphical
      TabIndex        =   3
      ToolTipText     =   "Browse for a file folder"
      Top             =   960
      Width           =   375
   End
   Begin VB.ComboBox cboLogFilesFolder 
      Height          =   315
      Left            =   840
      TabIndex        =   2
      Top             =   960
      Width           =   6255
   End
   Begin VB.CommandButton cmdUpdateFolderDefaults 
      Caption         =   "Use Default Locations"
      Height          =   375
      Left            =   3480
      TabIndex        =   4
      Top             =   2160
      Width           =   2415
   End
   Begin VB.Label lblServerPathsHelp 
      BackStyle       =   0  'Transparent
      Caption         =   $"Locations3.frx":013A
      Height          =   735
      Left            =   840
      TabIndex        =   9
      Top             =   1440
      UseMnemonic     =   0   'False
      Width           =   6735
   End
   Begin VB.Label lblDataFilesFolder 
      BackStyle       =   0  'Transparent
      Caption         =   "Location of Data File"
      Height          =   255
      Left            =   840
      TabIndex        =   8
      Top             =   120
      Width           =   1935
   End
   Begin VB.Image imgDBMSPaths 
      Height          =   480
      Left            =   120
      Picture         =   "Locations3.frx":01D0
      Top             =   120
      Width           =   480
   End
   Begin VB.Label lblLogFilesFolder 
      BackStyle       =   0  'Transparent
      Caption         =   "Location of Log File"
      Height          =   255
      Left            =   840
      TabIndex        =   7
      Top             =   720
      Width           =   1935
   End
End
Attribute VB_Name = "FLocations3"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Form:     Locations dialog 3
' Shared:   No
'
Option Explicit

' Data being edited by this dialog.
Private m_sDataFilesFolder As String
Private m_sLogFilesFolder As String

' Return value.
Private m_bOKPressed As Boolean

' Control variables.
Private m_bLoading As Boolean
Private m_sServerName As String

Public Function Dialog(ByVal frmParent As Form, _
    ByRef r_sDataFilesFolder As String, _
    ByRef r_sLogFilesFolder As String, _
    ByVal sServerName As String) As Boolean

    If DebugLogging Then
        WriteToLog "DEBUG: FLocations3.Dialog"
    End If

    m_sDataFilesFolder = r_sDataFilesFolder
    m_sLogFilesFolder = r_sLogFilesFolder

    m_sServerName = sServerName

    LoadAllControls

    m_bOKPressed = False
    If Not MMain.BatchMode Then
        Show vbModal, frmParent
    Else
        m_bOKPressed = True
    End If
    Dialog = m_bOKPressed

    If m_bOKPressed Then
        r_sDataFilesFolder = m_sDataFilesFolder
        r_sLogFilesFolder = m_sLogFilesFolder
    End If

End Function

Private Sub cmdCancel_Click()

    If PromptToExitApp() Then
        Unload Me
    End If

End Sub

Private Sub cmdDataFilesFolder_Click()

    DialogFolderOpenCtrl cboDataFilesFolder, _
        lblDataFilesFolder.Caption, _
        "Database Files|*.mdf;*.ndf;*.ldf|All Files|*.*", _
        "(Select the folder and click Open)", _
        hWnd
    cboDataFilesFolder.SetFocus

End Sub

Private Sub cmdLogFilesFolder_Click()

    DialogFolderOpenCtrl cboLogFilesFolder, _
        lblLogFilesFolder.Caption, _
        "Database Files|*.mdf;*.ndf;*.ldf|All Files|*.*", _
        "(Select the folder and click Open)", _
        hWnd
    cboLogFilesFolder.SetFocus

End Sub

Private Sub cmdOK_Click()

    If Not ValidateAllControls() Then Exit Sub

    SaveAllControls

    m_bOKPressed = True
    Unload Me

End Sub

Private Sub cmdUpdateFolderDefaults_Click()

    UpdateFolderDefaults

    cboDataFilesFolder.SetFocus

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

Private Sub LoadAllControls()

    m_bLoading = True

    MRUComboRead cboDataFilesFolder, "DataFilesFolders"
    MRUComboRead cboLogFilesFolder, "LogFilesFolders"

    UpdateFolderDefaults
    UpdateFolderBrowseButtons

    m_bLoading = False

End Sub

Private Sub SaveAllControls()

    m_sDataFilesFolder = Trim$(cboDataFilesFolder.Text)
    m_sLogFilesFolder = Trim$(cboLogFilesFolder.Text)

    MRUComboWrite cboDataFilesFolder, "DataFilesFolders"
    MRUComboWrite cboLogFilesFolder, "LogFilesFolders"

End Sub

Private Function ValidateAllControls() As Boolean

    Const ksHelpfulMessage = _
        "If you don't know what this should be set to, click on the ""Default " & _
        "Locations"" button to use the correct default folders for the SQL Server " & _
        "instance you have selected."

    ValidateAllControls = False

    If cboDataFilesFolder.Text = "" Then
        MsgBox "The data files location must be specified. " & ksHelpfulMessage, vbExclamation, Caption
        cboDataFilesFolder.SetFocus
        Exit Function
    End If
    If cboLogFilesFolder.Text = "" Then
        MsgBox "The log files location must be specified. " & ksHelpfulMessage, vbExclamation, Caption
        cboLogFilesFolder.SetFocus
        Exit Function
    End If

    ValidateAllControls = True

End Function

' Fetch new default values for the folder locations.
Private Sub UpdateFolderDefaults()

    Dim sBaseFolder As String

    sBaseFolder = GetSQLServerDataRootFolder(Me, m_sServerName)
    If sBaseFolder <> "" Then
        cboDataFilesFolder.Text = AddSlash(sBaseFolder) & "Data"
        cboLogFilesFolder.Text = AddSlash(sBaseFolder) & "Data"
    End If

End Sub

' Enable the folder browse buttons only if the server name refers to this computer.
Private Sub UpdateFolderBrowseButtons()

    Dim bEnabled As Boolean

    bEnabled = EnableFolderBrowseButtons(m_sServerName)
    cmdDataFilesFolder.Enabled = bEnabled
    cmdLogFilesFolder.Enabled = bEnabled
    lblServerPathsHelp.Visible = Not bEnabled

    If Not bEnabled Then
        lblServerPathsHelp.Caption = Replace(lblServerPathsHelp.Caption, "%server%", m_sServerName)
    End If

End Sub
