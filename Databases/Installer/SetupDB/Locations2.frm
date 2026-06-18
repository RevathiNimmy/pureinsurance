VERSION 5.00
Begin VB.Form FLocations2 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Database To Install To"
   ClientHeight    =   2175
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   7695
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
   Icon            =   "Locations2.frx":0000
   KeyPreview      =   -1  'True
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2175
   ScaleWidth      =   7695
   ShowInTaskbar   =   0   'False
   StartUpPosition =   2  'CenterScreen
   Begin VB.CheckBox chkBackupData 
      Caption         =   "Back up any existing data before upgrading (highly recommended)"
      Height          =   255
      Left            =   840
      TabIndex        =   4
      Top             =   1080
      Value           =   1  'Checked
      Width           =   5415
   End
   Begin VB.CommandButton cmdDatabaseName 
      Height          =   330
      Left            =   5520
      Picture         =   "Locations2.frx":000C
      Style           =   1  'Graphical
      TabIndex        =   3
      ToolTipText     =   "List all databases on the selected server"
      Top             =   600
      Width           =   375
   End
   Begin VB.CommandButton cmdServerName 
      Height          =   330
      Left            =   5520
      Picture         =   "Locations2.frx":00C1
      Style           =   1  'Graphical
      TabIndex        =   1
      ToolTipText     =   "List all running SQL Servers on the network"
      Top             =   120
      Width           =   375
   End
   Begin VB.ComboBox cboServerName 
      Height          =   315
      Left            =   2280
      Sorted          =   -1  'True
      TabIndex        =   0
      Top             =   120
      Width           =   3135
   End
   Begin VB.ComboBox cboDatabaseName 
      Height          =   315
      Left            =   2280
      TabIndex        =   2
      Top             =   600
      Width           =   3135
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   120
      TabIndex        =   5
      Top             =   1680
      Width           =   1215
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      CausesValidation=   0   'False
      Height          =   375
      Left            =   1560
      TabIndex        =   6
      Top             =   1680
      Width           =   1215
   End
   Begin VB.Image imgDBMS 
      Height          =   480
      Left            =   120
      Picture         =   "Locations2.frx":0172
      Top             =   120
      Width           =   480
   End
   Begin VB.Label lblServerName 
      BackStyle       =   0  'Transparent
      Caption         =   "Server Name"
      Height          =   255
      Left            =   840
      TabIndex        =   8
      Top             =   120
      Width           =   1455
   End
   Begin VB.Label lblDatabaseName 
      BackStyle       =   0  'Transparent
      Caption         =   "Database Name"
      Height          =   255
      Left            =   840
      TabIndex        =   7
      Top             =   600
      Width           =   1455
   End
End
Attribute VB_Name = "FLocations2"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Form:     Locations dialog 2
' Shared:   No
'
Option Explicit

' Data being edited by this dialog.
Private m_sServerName As String
Private m_sDatabaseName As String
Private m_sBackupData As String ' ksParamTrue or ksParamFalse

' Return value.
Private m_bOKPressed As Boolean

' Control variables.
Private m_bLoading As Boolean
Private m_sDefaultPMDAODataSource As String
Private m_sDefaultODBCDSN As String
Private m_sDefaultServerName As String
Private m_sDefaultDatabaseName As String
Private m_sDefaultBackupData As String

Public Function Dialog(ByVal frmParent As Form, _
    ByRef r_sServerName As String, _
    ByRef r_sDatabaseName As String, _
    ByRef r_sBackupData As String, _
    ByVal sDefaultPMDAODataSource As String, _
    ByVal sDefaultODBCDSN As String, _
    ByVal sDefaultServerName As String, _
    ByVal sDefaultDatabaseName As String, _
    ByVal sDefaultBackupData As String) As Boolean

    If DebugLogging Then
        WriteToLog "DEBUG: FLocations2.Dialog"
    End If

    m_sServerName = r_sServerName
    m_sDatabaseName = r_sDatabaseName
    m_sBackupData = r_sBackupData

    m_sDefaultPMDAODataSource = sDefaultPMDAODataSource
    m_sDefaultODBCDSN = sDefaultODBCDSN
    m_sDefaultServerName = sDefaultServerName
    m_sDefaultDatabaseName = sDefaultDatabaseName
    m_sDefaultBackupData = sDefaultBackupData

    LoadAllControls

    m_bOKPressed = False
    If Not MMain.BatchMode Then
        Show vbModal, frmParent
    Else
        m_bOKPressed = True
    End If
    Dialog = m_bOKPressed

    If m_bOKPressed Then
        r_sServerName = m_sServerName
        r_sDatabaseName = m_sDatabaseName
        r_sBackupData = m_sBackupData
    End If

End Function

Private Sub chkBackupData_Click()

    Const ksMessage = "SSP HIGHLY RECOMMEND backing up your data before " & _
        "undertaking a major operation of this nature. You should only choose " & _
        "'Yes' if you already have an up-to-date backup of your data. " & _
        "If data corruption occurs during the upgrade, and you do not have an " & _
        "up-to-date backup, SSP cannot be held responsible for any loss of " & _
        "data that may occur as a result." & vbCrLf & _
        vbCrLf & _
        "Are you sure you wish to proceed without a backup?"

    If m_bLoading Then Exit Sub

    If chkBackupData.Value <> vbChecked Then
        If MsgBox(ksMessage, vbExclamation + vbYesNo + vbDefaultButton2) = vbNo Then
            chkBackupData.Value = vbChecked
        End If
    End If

End Sub

Private Sub cmdCancel_Click()

    If PromptToExitApp() Then
        Unload Me
    End If

End Sub

Private Sub cmdDatabaseName_Click()

    Dim frm As FBrowse
    Dim sSelected As String

    ' Browse databases on the server.
    Set frm = New FBrowse
    sSelected = frm.Dialog(Me, cboDatabaseName, cboServerName)
    Set frm = Nothing

    ' If line selected, use it.
    If sSelected <> "" Then
        cboDatabaseName.Text = sSelected
    End If

    cboDatabaseName.SetFocus

End Sub

Private Sub cmdOK_Click()

    If Not ValidateAllControls() Then Exit Sub

    SaveAllControls

    m_bOKPressed = True
    Unload Me

End Sub

Private Sub cmdServerName_Click()

    Dim frm As FBrowse
    Dim sSelected As String

    ' Browse servers on the network.
    Set frm = New FBrowse
    sSelected = frm.Dialog(Me, cboServerName)
    Set frm = Nothing

    ' If line selected, use it.
    If sSelected <> "" Then
        cboServerName.Text = sSelected
    End If

    cboServerName.SetFocus

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

    MRUComboRead cboServerName, "ServerNames"
    MRUComboRead cboDatabaseName, "DatabaseNames"

    ' If any data is already defined, don't let the user change it.
    If m_sServerName <> "" Then
        DisableQuestion lblServerName, cboServerName, cmdServerName
    End If
    If m_sDatabaseName <> "" Then
        DisableQuestion lblDatabaseName, cboDatabaseName, cmdDatabaseName
    End If
    If m_sBackupData <> "" Then
        chkBackupData.Enabled = False
    End If

    ' Deduce a reasonable default for the server name.
    If m_sServerName = "" Then
        m_sServerName = ReadFromPMDAODataSource(m_sDefaultPMDAODataSource, "Server")
    End If
    If m_sServerName = "" And m_sDefaultODBCDSN <> "" Then
        m_sServerName = ReadFromODBCDSN(m_sDefaultODBCDSN, "Server")
    End If
    If m_sServerName = "" Then
        m_sServerName = m_sDefaultServerName
    End If

    ' Deduce a reasonable default for the database name.
    If m_sDatabaseName = "" Then
        m_sDatabaseName = ReadFromPMDAODataSource(m_sDefaultPMDAODataSource, "Database")
    End If
    If m_sDatabaseName = "" And m_sDefaultODBCDSN <> "" Then
        m_sDatabaseName = ReadFromODBCDSN(m_sDefaultODBCDSN, "Database")
    End If
    If m_sDatabaseName = "" Then
        m_sDatabaseName = m_sDefaultDatabaseName
    End If

    ' Deduce a reasonable default for the backup data flag.
    If m_sBackupData = "" Then
        m_sBackupData = m_sDefaultBackupData
    End If

    cboServerName.Text = m_sServerName
    cboDatabaseName.Text = m_sDatabaseName
    chkBackupData.Value = IIf(ParseBoolean(m_sBackupData, False), vbChecked, vbUnchecked)

    m_bLoading = False

End Sub

Private Sub SaveAllControls()

    m_sServerName = Trim$(cboServerName.Text)
    m_sDatabaseName = Trim$(cboDatabaseName.Text)
    m_sBackupData = IIf(chkBackupData.Value = vbChecked, ksParamTrue, ksParamFalse)

    MRUComboWrite cboServerName, "ServerNames"
    MRUComboWrite cboDatabaseName, "DatabaseNames"

End Sub

Private Function ValidateAllControls() As Boolean

    ValidateAllControls = False

    If cboServerName.Text = "" Then
        MsgBox "The server name must be specified.", vbExclamation, Caption
        cboServerName.SetFocus
        Exit Function
    End If
    If cboDatabaseName.Text = "" Then
        MsgBox "The database name must be specified.", vbExclamation, Caption
        cboDatabaseName.SetFocus
        Exit Function
    End If

    ValidateAllControls = True

End Function
