VERSION 5.00
Begin VB.Form FBrowse 
   BorderStyle     =   4  'Fixed ToolWindow
   ClientHeight    =   2955
   ClientLeft      =   15
   ClientTop       =   15
   ClientWidth     =   4815
   ControlBox      =   0   'False
   BeginProperty Font 
      Name            =   "Tahoma"
      Size            =   8.25
      Charset         =   0
      Weight          =   400
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   Icon            =   "Browse.frx":0000
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2955
   ScaleWidth      =   4815
   ShowInTaskbar   =   0   'False
   Begin VB.ListBox lstItems 
      Appearance      =   0  'Flat
      Height          =   2955
      Left            =   0
      TabIndex        =   0
      Top             =   0
      Width           =   3615
   End
   Begin VB.CommandButton cmdCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      CausesValidation=   0   'False
      Height          =   375
      Left            =   1320
      TabIndex        =   2
      TabStop         =   0   'False
      Top             =   240
      Width           =   1095
   End
   Begin VB.CommandButton cmdOK 
      Caption         =   "OK"
      Default         =   -1  'True
      Height          =   375
      Left            =   120
      TabIndex        =   1
      TabStop         =   0   'False
      Top             =   240
      Width           =   1095
   End
End
Attribute VB_Name = "FBrowse"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
' Form:     Drop-down browse list for various data
' Shared:   No
'
Option Explicit

Private m_sSelected As String

' Windows API functions.
Private Declare Function SetCapture Lib "user32" (ByVal hWnd As Long) As Long
Private Declare Function ReleaseCapture Lib "user32" () As Long

Public Function Dialog(ByVal frmParent As Form, ByVal ctrlParent As Control, _
    Optional ByVal sServerName As String = "") As String

    Dim bContinue As Boolean

    frmParent.MousePointer = vbHourglass

    ' Size and position the form as if it was a combo
    ' drop-down window.
    WinDropDown Me, ctrlParent.hWnd
    Width = ctrlParent.Width
    lstItems.Width = ScaleWidth
    Load Me

    ' If server provided, list databases on that server.
    ' Otherwise, list servers on the network.
    If sServerName <> "" Then
        bContinue = BrowseForDatabases(sServerName)
    Else
        bContinue = BrowseForServers()
    End If

    frmParent.MousePointer = vbDefault

    ' If the list could not be built, exit without displaying
    ' the form, because that's a lot neater.
    If Not bContinue Then
        Unload Me
        Dialog = ""
        Exit Function
    End If

    ' Simulate the behaviour of a combo dropdown window by
    ' closing it as soon as the user clicks off it. We do this
    ' by capturing the mouse outside the list control's borders.
    m_sSelected = ""
    SetCapture lstItems.hWnd
    Show vbModal, frmParent
    ReleaseCapture
    Dialog = m_sSelected

End Function

Private Sub cmdCancel_Click()

    Unload Me

End Sub

Private Sub cmdOK_Click()

    If lstItems.ListIndex = -1 Then Exit Sub

    m_sSelected = lstItems.Text
    Unload Me

End Sub

Private Sub Form_Activate()

    If lstItems.ListCount > 0 Then
        lstItems.ListIndex = 0
    End If
    lstItems.SetFocus

End Sub

Private Sub lstItems_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)

    ' Simulate the behaviour of a combo dropdown window by
    ' closing it as soon as the user clicks off it. We do this
    ' by capturing the mouse outside the list control's borders.
    If X < ScaleLeft Or _
        X > ScaleLeft + ScaleWidth Or _
        Y < ScaleTop Or _
        Y > ScaleTop + ScaleHeight Then
        Unload Me
    End If

End Sub

Private Sub lstItems_DblClick()

    cmdOK = True

End Sub

Private Function BrowseForServers() As Boolean

    Dim oApplication As Object ' SQLDMO.Application
    Dim vsName As Variant ' String

    On Error GoTo EH_BrowseForServers
    MousePointer = vbHourglass
    BrowseForServers = True
    lstItems.Clear

    Set oApplication = CreateObject("SQLDMO.Application")
    For Each vsName In oApplication.ListAvailableSQLServers()
        lstItems.AddItem vsName
    Next

EX_BrowseForServers:
    On Error Resume Next

    Set oApplication = Nothing

    MousePointer = vbDefault
    Exit Function

EH_BrowseForServers:
    ' Stress that this is not a fatal error.
    Select Case ErrorDialog(Database, _
        "Cannot find running instances of SQL Server on the network. " & _
        "This will NOT prevent you from continuing the installation, " & _
        "but you will have to enter the name of the SQL Server yourself.", _
        nButtons:=ebOK)
    Case erRetry
        Resume
    Case Else
        BrowseForServers = False
        Resume EX_BrowseForServers
    End Select

End Function

Private Function BrowseForDatabases(ByVal sServerName As String) As Boolean

    Dim oServer As Object ' SQLDMO.SQLServer
    Dim oDatabase As Object ' SQLDMO.Database

    On Error GoTo EH_BrowseForDatabases
    MousePointer = vbHourglass
    BrowseForDatabases = True
    lstItems.Clear

    Set oServer = ConnectUsingSQLDMO(Me, sServerName, "database names")
    If oServer Is Nothing Then
        Exit Function
    End If
    For Each oDatabase In oServer.Databases
        lstItems.AddItem oDatabase.Name
    Next

EX_BrowseForDatabases:
    On Error Resume Next

    Set oDatabase = Nothing
    oServer.Disconnect
    Set oServer = Nothing

    MousePointer = vbDefault
    Exit Function

EH_BrowseForDatabases:
    ' Stress that this is not a fatal error.
    Select Case ErrorDialog(Database, _
        "Cannot connect to server " & sServerName & " to list databases. " & _
        "This will NOT prevent you from continuing the installation, " & _
        "but you will have to enter the database name yourself.", _
        nButtons:=ebOK)
    Case erRetry
        Resume
    Case Else
        BrowseForDatabases = False
        Resume EX_BrowseForDatabases
    End Select

End Function
