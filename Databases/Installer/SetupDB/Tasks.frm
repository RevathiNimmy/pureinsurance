VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "mscomctl.ocx"
Begin VB.Form FTasks 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Install Tasks"
   ClientHeight    =   5175
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   7710
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
   Icon            =   "Tasks.frx":0000
   KeyPreview      =   -1  'True
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   5175
   ScaleWidth      =   7710
   ShowInTaskbar   =   0   'False
   StartUpPosition =   2  'CenterScreen
   Begin VB.ComboBox cboVersion 
      Height          =   315
      Left            =   3360
      Style           =   2  'Dropdown List
      TabIndex        =   2
      Top             =   4080
      Width           =   2535
   End
   Begin MSComctlLib.ListView lvwTasks 
      Height          =   3615
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   7455
      _ExtentX        =   13150
      _ExtentY        =   6376
      View            =   3
      LabelEdit       =   1
      LabelWrap       =   0   'False
      HideSelection   =   0   'False
      FullRowSelect   =   -1  'True
      _Version        =   393217
      ForeColor       =   -2147483640
      BackColor       =   -2147483643
      Appearance      =   1
      NumItems        =   3
      BeginProperty ColumnHeader(1) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
         Text            =   "Product"
         Object.Width           =   5292
      EndProperty
      BeginProperty ColumnHeader(2) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
         SubItemIndex    =   1
         Text            =   "Current Version"
         Object.Width           =   3175
      EndProperty
      BeginProperty ColumnHeader(3) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
         SubItemIndex    =   2
         Text            =   "Install This Version"
         Object.Width           =   3528
      EndProperty
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      CausesValidation=   0   'False
      Height          =   375
      Left            =   1560
      TabIndex        =   4
      Top             =   4680
      Width           =   1215
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   120
      TabIndex        =   3
      Top             =   4680
      Width           =   1215
   End
   Begin VB.Label lblProductName 
      BackStyle       =   0  'Transparent
      BorderStyle     =   1  'Fixed Single
      Height          =   315
      Left            =   120
      TabIndex        =   1
      Top             =   4080
      UseMnemonic     =   0   'False
      Width           =   3015
   End
   Begin VB.Label lblLabel 
      BackStyle       =   0  'Transparent
      Caption         =   "Product"
      Height          =   255
      Index           =   0
      Left            =   120
      TabIndex        =   6
      Top             =   3840
      Width           =   975
   End
   Begin VB.Label lblLabel 
      BackStyle       =   0  'Transparent
      Caption         =   "Install This Version"
      Height          =   255
      Index           =   1
      Left            =   3360
      TabIndex        =   5
      Top             =   3840
      Width           =   1815
   End
End
Attribute VB_Name = "FTasks"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Form:     Tasks dialog
' Shared:   No
'
Option Explicit

Const ksVersionNoChangeText = "No Change"

Private m_oTasks As CLogicalDatabases

Private m_bDontHandleComboClick As Boolean

Private m_bOKPressed As Boolean

Public Function Dialog(ByVal frmParent As Form, _
    ByVal oTasks As CLogicalDatabases) As Boolean

    If DebugLogging Then
        WriteToLog "DEBUG: FTasks.Dialog"
    End If

    Set m_oTasks = oTasks
    LoadAllControls

    m_bOKPressed = False
    Show vbModal, frmParent
    Dialog = m_bOKPressed

End Function

Private Sub cboVersion_Click()

    Dim sVersion As String

    If m_bDontHandleComboClick Then
        Exit Sub
    ElseIf lvwTasks.SelectedItem Is Nothing Then
        Exit Sub
    End If

    sVersion = cboVersion.Text
    If sVersion = ksVersionNoChangeText Then
        sVersion = ""
    End If

    lvwTasks.SelectedItem.SubItems(2) = sVersion

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

Private Sub lvwTasks_ItemClick(ByVal Item As MSComctlLib.ListItem)

    UpdateEditControls

End Sub

Private Sub LoadAllControls()

    Dim oTask As CLogicalDatabase

    ' Only show logical databases which have either a
    ' current version or at least one installable version
    ' on CD. This is to allow us to use one XML config
    ' file for all CDs without it looking silly.
    For Each oTask In m_oTasks
        If oTask.CurrentVersion <> "" Or oTask.InstallableVersions.Count > 0 Then
            With lvwTasks.ListItems.Add()
                Set .Tag = oTask
                .Text = oTask.Description
                .SubItems(1) = oTask.CurrentVersion
                .SubItems(2) = oTask.DefaultVersion
            End With
        End If
    Next

    m_bDontHandleComboClick = False

    UpdateEditControls

End Sub

Private Sub SaveAllControls()

    Dim oRow As ListItem

    For Each oRow In lvwTasks.ListItems
        oRow.Tag.Version = oRow.SubItems(2)
    Next

End Sub

Private Function ValidateAllControls() As Boolean

    ' All validation on the task list is performed in
    ' one place after the form is closed.
    ValidateAllControls = True

End Function

Private Sub UpdateEditControls()

    Dim oTask As CLogicalDatabase
    Dim vsVersion As Variant ' String
    Dim i As Long
    Dim sVersion As String

    m_bDontHandleComboClick = True

    If lvwTasks.SelectedItem Is Nothing Then
        cboVersion.Enabled = False

        lblProductName.Caption = ""
        cboVersion.ListIndex = -1
        cboVersion.Clear
    Else
        cboVersion.Enabled = True

        Set oTask = lvwTasks.SelectedItem.Tag

        lblProductName.Caption = oTask.Description
        cboVersion.ListIndex = -1
        cboVersion.Clear

        cboVersion.AddItem ksVersionNoChangeText
        For Each vsVersion In oTask.InstallableVersions
            cboVersion.AddItem vsVersion
        Next

        sVersion = lvwTasks.SelectedItem.SubItems(2)
        If sVersion = "" Then
            sVersion = ksVersionNoChangeText
        End If

        For i = 0 To cboVersion.ListCount - 1
            If cboVersion.List(i) = sVersion Then
                cboVersion.ListIndex = i
                Exit For
            End If
        Next
    End If

    m_bDontHandleComboClick = False

End Sub
