VERSION 5.00
Begin VB.Form FMain 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Database Installer"
   ClientHeight    =   2175
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   9150
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
   Icon            =   "Main.frx":0000
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2175
   ScaleWidth      =   9150
   StartUpPosition =   2  'CenterScreen
   Begin VB.Timer tmrUpdateUI 
      Enabled         =   0   'False
      Interval        =   500
      Left            =   8640
      Top             =   1680
   End
   Begin VB.TextBox txtProduct 
      BackColor       =   &H8000000F&
      ForeColor       =   &H80000012&
      Height          =   285
      Left            =   1200
      Locked          =   -1  'True
      TabIndex        =   4
      Top             =   600
      Width           =   4575
   End
   Begin VB.TextBox txtFile 
      BackColor       =   &H8000000F&
      ForeColor       =   &H80000012&
      Height          =   285
      Left            =   1200
      Locked          =   -1  'True
      TabIndex        =   3
      Top             =   960
      Width           =   7815
   End
   Begin VB.TextBox txtStatement 
      BackColor       =   &H8000000F&
      ForeColor       =   &H80000012&
      Height          =   285
      Left            =   1200
      Locked          =   -1  'True
      TabIndex        =   2
      Top             =   1320
      Width           =   1815
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      CausesValidation=   0   'False
      Default         =   -1  'True
      Height          =   375
      Left            =   3960
      MousePointer    =   1  'Arrow
      TabIndex        =   0
      Top             =   1680
      Width           =   1215
   End
   Begin VB.Label lblProduct 
      BackStyle       =   0  'Transparent
      Caption         =   "Product"
      Height          =   255
      Left            =   120
      TabIndex        =   7
      Top             =   600
      Width           =   1095
   End
   Begin VB.Label lblFile 
      BackStyle       =   0  'Transparent
      Caption         =   "File"
      Height          =   255
      Left            =   120
      TabIndex        =   6
      Top             =   960
      Width           =   1095
   End
   Begin VB.Label lblStatement 
      BackStyle       =   0  'Transparent
      Caption         =   "Statement"
      Height          =   255
      Left            =   120
      TabIndex        =   5
      Top             =   1320
      Width           =   1095
   End
   Begin VB.Label lblText 
      BackStyle       =   0  'Transparent
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   120
      TabIndex        =   1
      Top             =   120
      UseMnemonic     =   0   'False
      Width           =   8895
   End
End
Attribute VB_Name = "FMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Form:     Main application form
' Shared:   No
'
Option Explicit

Implements ISQLScriptEvents

' Running/cancelled state.
Private m_bScriptRunning As Boolean
Private m_bCancelled As Boolean
Private m_bCancelButtonPressed As Boolean

' All data displayed on screen.
Private m_sText As String
Private m_sTextPrev As String
Private m_sProduct As String
Private m_sProductPrev As String
Private m_sFile As String
Private m_sFilePrev As String
Private m_nStatement As Long
Private m_nStatementPrev As Long

' The script interpreter.
Private m_oScript As CSQLScript
Attribute m_oScript.VB_VarHelpID = -1

' Error declarations.
Private Const ksErrSource = ksProjectName '& ".FMain"
Private Const ksErrTypeMismatch = "Invalid variable type passed to #prompt."

Public Sub ShowText(ByVal sText As String)

    m_sText = sText

    ' A text change must always display instantly.
    UpdateUI

    tmrUpdateUI.Enabled = True
    DoEvents

End Sub

Public Sub ShowProduct(ByVal sProduct As String)

    m_sProduct = sProduct

    tmrUpdateUI.Enabled = True
    DoEvents

End Sub

Public Sub UpdateUI()

    If m_sTextPrev <> m_sText Then
        lblText.Caption = m_sText
        m_sTextPrev = m_sText
    End If

    If m_sProductPrev <> m_sProduct Then
        txtProduct.Text = m_sProduct
        m_sProductPrev = m_sProduct
    End If

    If m_sFilePrev <> m_sFile Then
        txtFile.Text = m_sFile
        m_sFilePrev = m_sFile
    End If

    If m_nStatementPrev <> m_nStatement Then
        If m_nStatement <> 0 Then
            txtStatement.Text = CStr(m_nStatement)
        Else
            txtStatement.Text = ""
        End If
        m_nStatementPrev = m_nStatement
    End If

End Sub

Public Property Get Cancelled() As Boolean

    Cancelled = m_bCancelled

End Property

Public Property Let Cancelled(ByVal bCancelled As Boolean)

    m_bCancelled = bCancelled

End Property

Public Sub ScriptInitialise()

    ' Initialise the script object.
    Set m_oScript = New CSQLScript
    m_oScript.AllowUserToIgnore = True ' this is dealt with elsewhere
    m_oScript.AutoDrop = False
    m_oScript.CommentAsSeparator = False
    m_oScript.ErrorOnBadMetaCommands = False
    m_oScript.ErrorOnFileNotFound = False
    m_oScript.IgnoreErrors = False
    m_oScript.LogActions = False
    m_oScript.RemoveComments = False
    m_oScript.UserName = ""
    Set m_oScript.Events = Me
    m_oScript.Initialise Database

    ' Turn on "show progress" mode.
    MousePointer = vbHourglass
    ShowProgressControls True
    ShowCancelButton True
    m_bScriptRunning = True
    m_bCancelled = False

    DoEvents

End Sub

Public Sub ScriptRun(ByVal sFile As String, _
    ByVal sVersionFrom As String, _
    ByVal sVersionTo As String, _
    ByVal bDetectLoggingProcedures As Boolean)

    ' Set the from and to versions for writing to the script file audit table.
    m_oScript.VersionFrom = sVersionFrom
    m_oScript.VersionTo = sVersionTo

    ' Re-detect logging procedures if requested (they might have been added during the previous script run).
    If bDetectLoggingProcedures Then
        m_oScript.DetectLoggingProcedures False
    End If

    ' Run a script.
    m_oScript.Run sFile

    ' If the user cancelled the script, abandon the
    ' application immediately. The error code will be
    ' able to tell what happened by examining the Cancelled
    ' property.
    If m_oScript.Cancelled Then
        Err.Raise knErrAbort
    End If

End Sub

Public Sub ScriptTerminate()

    ' Turn off "show progress" mode.
    m_bCancelled = False
    m_bScriptRunning = False
    ShowCancelButton False
    ShowProgressControls False
    MousePointer = vbDefault

    DoEvents

    ' Terminate the script object.
    If Not m_oScript Is Nothing Then
        m_oScript.Terminate
        Set m_oScript = Nothing
    End If

End Sub

Private Sub cmdCancel_Click()

    ' Ask the user to confirm they wish to cancel.
    m_bCancelled = PromptToExitApp()

    ' Tell the running script to cancel. Do a safety check in case this code is
    ' accidentally called when the script is not instantiated.
    If Not m_oScript Is Nothing Then
        m_oScript.Cancelled = m_bCancelled
    End If

End Sub

Private Sub Form_Load()

    ' Set initial defaults.
    m_bScriptRunning = False

    m_sText = ""
    m_sTextPrev = ""
    m_sProduct = ""
    m_sProductPrev = ""
    m_sFile = ""
    m_sFilePrev = ""
    m_nStatement = 0
    m_nStatementPrev = 0

    ' Make the window title the application title.
    Caption = App.Title

    ' Turn off the window's close button.
    WinEnableClose Me, False

    ShowCancelButton False

    ShowProgressControls False

End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)

    ' If the form was unloaded by the user, then always refuse. All user cancellation
    ' code should be routed through the cancel button, whether it is available or not.
    ' If the form is unloaded from code, always allow that because that's how the
    ' application is normally terminated.
    If UnloadMode = vbFormControlMenu Then
        Cancel = True
    End If

End Sub

Private Sub Form_Unload(Cancel As Integer)

    ' Terminate the script object if it's not already done.
    On Error Resume Next
    ScriptTerminate

    ' Finish the application.
    MainShutdown

End Sub

Private Sub tmrUpdateUI_Timer()

    UpdateUI

End Sub

Private Sub ISQLScriptEvents_ProcessFile(ByVal sFile As String)

    m_sFile = sFile

    tmrUpdateUI.Enabled = True
    DoEvents

    ' Record this script file in the log.
    WriteToLog "Run script " & sFile

End Sub

Private Sub ISQLScriptEvents_ProcessStatement(ByVal nStatement As Long, ByVal sSQL As String, ByRef r_bProcess As Boolean)

    m_nStatement = nStatement

    tmrUpdateUI.Enabled = True
    DoEvents

End Sub

Private Sub ISQLScriptEvents_PromptForValue(ByVal sPromptText As String, ByVal vDefault As Variant, ByRef r_vValue As Variant)

    ' Very simple UI to prompt for values.
    Select Case VarType(vDefault)
    Case vbBoolean, vbEmpty
        r_vValue = (MsgBox(sPromptText, vbQuestion + vbYesNo) = vbYes)
    Case vbByte
        r_vValue = ToByte(InputBox(sPromptText, , vDefault))
    Case vbInteger
        r_vValue = ToInteger(InputBox(sPromptText, , vDefault))
    Case vbLong
        r_vValue = ToLong(InputBox(sPromptText, , vDefault))
    Case vbDouble
        r_vValue = ToDouble(InputBox(sPromptText, , vDefault))
    Case vbString
        r_vValue = InputBox(sPromptText, , vDefault)
    Case Else
        Err.Raise knErrTypeMismatch, ksErrSource, ksErrTypeMismatch
    End Select

End Sub

Private Sub ShowCancelButton(ByVal bVisible As Boolean)

    cmdCancel.Visible = bVisible And MMain.AllowUserToCancelMain

End Sub

Private Sub ShowProgressControls(ByVal bVisible As Boolean)

    lblProduct.Visible = bVisible
    txtProduct.Visible = bVisible
    lblFile.Visible = bVisible
    txtFile.Visible = bVisible
    lblStatement.Visible = bVisible
    txtStatement.Visible = bVisible

End Sub
