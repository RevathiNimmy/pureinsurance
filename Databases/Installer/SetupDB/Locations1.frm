VERSION 5.00
Begin VB.Form FLocations1 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Folders To Install From"
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
   Icon            =   "Locations1.frx":0000
   KeyPreview      =   -1  'True
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2175
   ScaleWidth      =   7695
   ShowInTaskbar   =   0   'False
   StartUpPosition =   2  'CenterScreen
   Begin VB.ComboBox cboProductDataFolder 
      Height          =   315
      Left            =   840
      TabIndex        =   2
      Top             =   1080
      Width           =   6255
   End
   Begin VB.ComboBox cboInstallFolder 
      Height          =   315
      Left            =   840
      TabIndex        =   0
      Top             =   360
      Width           =   6255
   End
   Begin VB.CommandButton cmdInstallFolder 
      Height          =   330
      Left            =   7200
      Picture         =   "Locations1.frx":000C
      Style           =   1  'Graphical
      TabIndex        =   1
      ToolTipText     =   "Browse for a file folder"
      Top             =   360
      Width           =   375
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   120
      TabIndex        =   3
      Top             =   1680
      Width           =   1215
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      CausesValidation=   0   'False
      Height          =   375
      Left            =   1560
      TabIndex        =   4
      Top             =   1680
      Width           =   1215
   End
   Begin VB.Label lblProductDataFolder 
      BackStyle       =   0  'Transparent
      Caption         =   "Product Data Folder (optional)"
      Height          =   255
      Left            =   840
      TabIndex        =   6
      Top             =   840
      Width           =   2655
   End
   Begin VB.Image imgInstallCD 
      Height          =   480
      Left            =   120
      Picture         =   "Locations1.frx":00A3
      Top             =   120
      Width           =   480
   End
   Begin VB.Label lblInstallFolder 
      BackStyle       =   0  'Transparent
      Caption         =   "Location of Install Scripts"
      Height          =   255
      Left            =   840
      TabIndex        =   5
      Top             =   120
      Width           =   2295
   End
End
Attribute VB_Name = "FLocations1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Form:     Locations dialog 1
' Shared:   No
'
Option Explicit

' Data being edited by this dialog.
Private m_sInstallFolder As String
Private m_sProductDataFolder As String

' Return value.
Private m_bOKPressed As Boolean

Public Function Dialog(ByVal frmParent As Form, _
    ByRef r_sInstallFolder As String, _
    ByRef r_sProductDataFolder As String) As Boolean

    If DebugLogging Then
        WriteToLog "DEBUG: FLocations1.Dialog"
    End If

    m_sInstallFolder = r_sInstallFolder
    m_sProductDataFolder = r_sProductDataFolder

    LoadAllControls

    m_bOKPressed = False
    If Not MMain.BatchMode Then
        Show vbModal, frmParent
    Else
        m_bOKPressed = True
    End If
    Dialog = m_bOKPressed

    If m_bOKPressed Then
        r_sInstallFolder = m_sInstallFolder
        r_sProductDataFolder = m_sProductDataFolder
    End If

End Function

Private Sub cmdCancel_Click()

    If PromptToExitApp() Then
        Unload Me
    End If

End Sub

Private Sub cmdInstallFolder_Click()

    DialogFolderOpenCtrl cboInstallFolder, _
        "Location of Install Scripts", _
        "All Files|*.*", _
        "(Select the folder and click Open)", _
        hWnd
    cboInstallFolder.SetFocus

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

    MRUComboRead cboInstallFolder, "InstallFolders"
    MRUComboRead cboProductDataFolder, "ProductDataFolders"

    ' If any data is already defined, don't let the user change it.
    If m_sInstallFolder <> "" Then
        DisableQuestion lblInstallFolder, cboInstallFolder, cmdInstallFolder
    End If
    If m_sProductDataFolder <> "" Then
        DisableQuestion lblProductDataFolder, cboProductDataFolder
    End If

    ' Deduce a reasonable default for the install folder.
    If m_sInstallFolder = "" Then
        m_sInstallFolder = RegRead(HKEY_LOCAL_MACHINE, ksRegProject, "InstallFolder", "")
    End If

    ' Deduce a reasonable default for the product data folder.
    If m_sProductDataFolder = "" Then
        m_sProductDataFolder = RegRead(HKEY_LOCAL_MACHINE, ksRegProject, "ProductDataFolder", ksParamEmptyValue)
    End If

    cboInstallFolder.Text = m_sInstallFolder
    cboProductDataFolder.Text = m_sProductDataFolder

End Sub

Private Sub SaveAllControls()

    m_sInstallFolder = Trim$(cboInstallFolder.Text)
    m_sProductDataFolder = Trim$(cboProductDataFolder.Text)

    MRUComboWrite cboInstallFolder, "InstallFolders"
    MRUComboWrite cboProductDataFolder, "ProductDataFolders"

End Sub

Private Function ValidateAllControls() As Boolean

    ValidateAllControls = False

    If cboInstallFolder.Text = "" Then
        MsgBox "The install scripts location must be specified.", vbExclamation, Caption
        cboInstallFolder.SetFocus
        Exit Function
    End If
    ' cboProductDataFolder is optional

    ValidateAllControls = True

End Function
