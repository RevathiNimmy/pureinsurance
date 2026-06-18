VERSION 5.00
Begin VB.Form FLocations4 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Backup Database To"
   ClientHeight    =   2055
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
   Icon            =   "Locations4.frx":0000
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2055
   ScaleWidth      =   7695
   ShowInTaskbar   =   0   'False
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton cmdSkip 
      Caption         =   "Skip"
      CausesValidation=   0   'False
      Height          =   375
      Left            =   2880
      TabIndex        =   7
      Top             =   1560
      Width           =   1215
   End
   Begin VB.CommandButton cmdCancel 
      Caption         =   "Cancel"
      CausesValidation=   0   'False
      Height          =   375
      Left            =   1560
      TabIndex        =   4
      Top             =   1560
      Width           =   1215
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   120
      TabIndex        =   3
      Top             =   1560
      Width           =   1215
   End
   Begin VB.ComboBox cboBackupFilesFolder 
      Height          =   315
      Left            =   840
      TabIndex        =   0
      Top             =   360
      Width           =   6255
   End
   Begin VB.CommandButton cmdBackupFilesFolder 
      Height          =   330
      Left            =   7200
      Picture         =   "Locations4.frx":000C
      Style           =   1  'Graphical
      TabIndex        =   1
      ToolTipText     =   "Browse for a file folder"
      Top             =   360
      Width           =   375
   End
   Begin VB.CommandButton cmdUpdateFolderDefaults 
      Caption         =   "Use Default Location"
      Height          =   375
      Left            =   5160
      TabIndex        =   2
      Top             =   1560
      Width           =   2415
   End
   Begin VB.Label lblServerPathsHelp 
      BackStyle       =   0  'Transparent
      Caption         =   $"Locations4.frx":00A3
      Height          =   735
      Left            =   840
      TabIndex        =   6
      Top             =   840
      UseMnemonic     =   0   'False
      Width           =   6735
   End
   Begin VB.Label lblBackupFilesFolder 
      BackStyle       =   0  'Transparent
      Caption         =   "Location of Backup File"
      Height          =   255
      Left            =   840
      TabIndex        =   5
      Top             =   120
      Width           =   1935
   End
   Begin VB.Image imgDBMSPaths 
      Height          =   480
      Left            =   120
      Picture         =   "Locations4.frx":0138
      Top             =   120
      Width           =   480
   End
End
Attribute VB_Name = "FLocations4"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Form:     Locations dialog 4
' Shared:   No
'
Option Explicit

' Data being edited by this dialog.
Private m_sBackupFilesFolder As String

' Return value.
Private m_bOKPressed As Boolean
Private m_bSkipPressed As Boolean

' Control variables.
Private m_bLoading As Boolean
Private m_sServerName As String

Public Function Dialog(ByVal frmParent As Form, _
    ByRef r_sBackupFilesFolder As String, _
    ByVal sServerName As String) As Boolean

    If DebugLogging Then
        WriteToLog "DEBUG: FLocations4.Dialog"
    End If

    m_sBackupFilesFolder = r_sBackupFilesFolder

    m_sServerName = sServerName

    LoadAllControls

    m_bOKPressed = False
    If Not MMain.BatchMode Then
        Show vbModal, frmParent
    Else
        m_bOKPressed = True
    End If
    
    If m_bOKPressed Or m_bSkipPressed Then
        Dialog = True
    Else
        Dialog = False
    End If

    If m_bOKPressed Then
        r_sBackupFilesFolder = m_sBackupFilesFolder
    Else
        r_sBackupFilesFolder = ""
    End If

End Function

Private Sub cmdBackupFilesFolder_Click()

    DialogFolderOpenCtrl cboBackupFilesFolder, _
        lblBackupFilesFolder.Caption, _
        "Backup Files|*.;*.bak;*.backup;*.dat;*.mbk|All Files|*.*", _
        "(Select the folder and click Open)", _
        hWnd
    cboBackupFilesFolder.SetFocus

End Sub

Private Sub cmdCancel_Click()

    If PromptToExitApp() Then
        Unload Me
    End If

End Sub

Private Sub cmdOK_Click()

    If Not ValidateAllControls() Then Exit Sub

    SaveAllControls

    m_bOKPressed = True
    m_bSkipPressed = False
    Unload Me

End Sub

Private Sub cmdSkip_Click()
Dim eResult As VbMsgBoxResult
    
    eResult = MsgBox( _
        "Are you sure that you want to skip the database backup? " & vbCrLf & _
        "Contact SSP for more information.", _
        vbExclamation + vbYesNo + vbDefaultButton2, "Skip Database Backup")

    If eResult = vbYes Then
        m_bOKPressed = False
        m_bSkipPressed = True
        Unload Me
    End If
End Sub

Private Sub cmdUpdateFolderDefaults_Click()

    UpdateFolderDefaults

    cboBackupFilesFolder.SetFocus

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
    cmdSkip.Visible = MMain.AllowUserToSkipBackup

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

    MRUComboRead cboBackupFilesFolder, "BackupFilesFolders"

    UpdateFolderDefaults
    UpdateFolderBrowseButtons

    m_bLoading = False

End Sub

Private Sub SaveAllControls()

    m_sBackupFilesFolder = Trim$(cboBackupFilesFolder.Text)

    MRUComboWrite cboBackupFilesFolder, "BackupFilesFolders"

End Sub

Private Function ValidateAllControls() As Boolean

    Const ksHelpfulMessage = _
        "If you don't know what this should be set to, click on the ""Default " & _
        "Location"" button to use the correct default folder for the SQL Server " & _
        "instance you have selected."

    ValidateAllControls = False

    If cboBackupFilesFolder.Text = "" Then
        MsgBox "The backup files location must be specified. " & ksHelpfulMessage, vbExclamation, Caption
        cboBackupFilesFolder.SetFocus
        Exit Function
    End If

    ValidateAllControls = True

End Function

' Fetch new default values for the folder locations.
Private Sub UpdateFolderDefaults()

    Dim sBaseFolder As String

    sBaseFolder = GetSQLServerDataRootFolder(Me, m_sServerName)
    If sBaseFolder <> "" Then
        cboBackupFilesFolder.Text = AddSlash(sBaseFolder) & "Backup"
    End If

End Sub

' Enable the folder browse buttons only if the server name refers to this computer.
Private Sub UpdateFolderBrowseButtons()

    Dim bEnabled As Boolean

    bEnabled = EnableFolderBrowseButtons(m_sServerName)
    cmdBackupFilesFolder.Enabled = bEnabled
    lblServerPathsHelp.Visible = Not bEnabled

    If Not bEnabled Then
        lblServerPathsHelp.Caption = Replace(lblServerPathsHelp.Caption, "%server%", m_sServerName)
    End If

End Sub
