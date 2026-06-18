Attribute VB_Name = "MServerInfo"
' Module:   Retrieves info from a SQL Server on the network
' Shared:   No
'
' This functionality is in a separate module to enable us to
' cache information obtained from the most recently accessed
' server instance. This is more user-friendly than continually
' prompting the user again and again.
'
Option Explicit

' Cached information.
Private m_sServerName As String
Private m_bTrusted As Boolean
Private m_sUserName As String
Private m_sPassword As String
Private m_sDataRootFolder As String

' Returns a login name and password with system admin rights
' for the specified server. The user only sees a dialog if the
' server is different to the last one. Returns True only if
' valid information is returned.
Public Function GetLoginAccount(ByVal frmParent As Form, _
    ByVal sServerName As String, _
    ByRef r_bTrusted As Boolean, _
    ByRef r_sUserName As String, _
    ByRef r_sPassword As String) As Boolean

    Dim frm As FLogin
    Dim bOKPressed As Boolean

    GetLoginAccount = False

    ' Sanity check.
    If sServerName = "" Then
        Exit Function
    End If

    ' If server is the one in the cache, then just return the cache data.
    If sServerName = m_sServerName Then
        r_bTrusted = m_bTrusted
        r_sUserName = m_sUserName
        r_sPassword = m_sPassword
        GetLoginAccount = True
        Exit Function
    End If

    ' Prompt the user for new login details.
    Set frm = New FLogin
    bOKPressed = frm.Dialog(frmParent, sServerName, r_bTrusted, r_sUserName, r_sPassword)
    Set frm = Nothing

    ' Update cache data.
    If bOKPressed Then
        m_sServerName = sServerName
        m_bTrusted = r_bTrusted
        m_sUserName = r_sUserName
        m_sPassword = r_sPassword
        GetLoginAccount = True
    End If

End Function

' Attempt to open a SQLDMO connection with the SIRIUS login. If that doesn't work,
' prompt the user for an alternative login. Return an open SQLDMO.SQLServer object
' or Nothing if it didn't work.
Public Function ConnectUsingSQLDMO(ByVal frmParent As Form, _
    ByVal sServerName As String, _
    ByVal sDescription As String) As Object ' SQLDMO.SQLServer

    Dim oServer As Object ' SQLDMO.SQLServer
    Dim bTrusted As Boolean
    Dim sUserName As String
    Dim sPassword As String

    On Error GoTo EH_Handler
    Set oServer = CreateObject("SQLDMO.SQLServer")

ConnectUsingSiriusLogin:
    ' Attempt to connect using the SIRIUS login.
    oServer.LoginSecure = False
    oServer.Connect sServerName, ksSALoginName, ksSALoginPassword

    GoTo EX_NormalExit

ConnectUsingWindowsLogin:

    On Error GoTo EH_Handler2
    ' Attempt to connect using windows login.
    oServer.LoginSecure = True
    oServer.Connect sServerName
    
    GoTo EX_NormalExit

ConnectUsingPromptedLogin:
    ' Attempt to connect using a login entered by the user.
    If Not GetLoginAccount(frmParent, sServerName, bTrusted, sUserName, sPassword) Then
        Exit Function
    End If
    oServer.LoginSecure = bTrusted
    oServer.Connect sServerName, sUserName, sPassword

EX_NormalExit:
    Set ConnectUsingSQLDMO = oServer
    Exit Function

EH_Handler:
    If Err.Number = 18456 Then
        ' [Microsoft][ODBC SQL Server Driver][SQL Server]Login failed for user 'SIRIUS'.
        m_sServerName = "" ' clear cache to prevent possible infinite loop
        Resume ConnectUsingWindowsLogin
    End If
    ' Stress that this is not a fatal error.
    Select Case ErrorDialog(Database, _
        "Cannot connect to the specified SQL Server instance to read " & sDescription & ". " & _
        "This will NOT prevent you from continuing the installation, " & _
        "but you will have to enter the " & sDescription & " yourself.", _
        nButtons:=ebOK)
    Case erRetry
        Resume
    Case Else
        Resume EX_Abort
    End Select

EH_Handler2:
    If Err.Number = 18456 Then
        ' [Microsoft][ODBC SQL Server Driver][SQL Server]Login failed for user 'SIRIUS'.
        m_sServerName = "" ' clear cache to prevent possible infinite loop
        Resume ConnectUsingPromptedLogin
    End If
    ' Stress that this is not a fatal error.
    Select Case ErrorDialog(Database, _
        "Cannot connect to the specified SQL Server instance to read " & sDescription & ". " & _
        "This will NOT prevent you from continuing the installation, " & _
        "but you will have to enter the " & sDescription & " yourself.", _
        nButtons:=ebOK)
    Case erRetry
        Resume
    Case Else
        Resume EX_Abort
    End Select

EX_Abort:
    On Error Resume Next
    oServer.Disconnect
    Set oServer = Nothing
    Exit Function

End Function

' Returns the data root folder relative to the target machine.
' The user only sees a dialog if the server is different to the
' last one. If unsuccessful, a blank string is returned.
Public Function GetSQLServerDataRootFolder(ByVal frmParent As Form, _
    ByVal sServerName As String) As String

    Dim oServer As Object ' SQLDMO.SQLServer

    GetSQLServerDataRootFolder = ""

    ' Sanity check.
    If sServerName = "" Then
        Exit Function
    End If

    ' If server is the one in the cache AND the data root folder
    ' has been filled in already, then just return the cache data.
    If sServerName = m_sServerName And m_sDataRootFolder <> "" Then
        GetSQLServerDataRootFolder = m_sDataRootFolder
        Exit Function
    End If

    On Error GoTo EH_GetSQLServerDataRootFolder
    frmParent.MousePointer = vbHourglass

    ' Connect to the server and retrieve the data root path.
    Set oServer = ConnectUsingSQLDMO(frmParent, sServerName, "default file locations")
    If oServer Is Nothing Then
        Exit Function
    End If
    GetSQLServerDataRootFolder = oServer.Registry.SQLDataRoot

    ' Update cache data.
    m_sServerName = sServerName
    m_sDataRootFolder = GetSQLServerDataRootFolder

EX_GetSQLServerDataRootFolder:
    On Error Resume Next
    oServer.Disconnect
    Set oServer = Nothing
    frmParent.MousePointer = vbDefault
    Exit Function

EH_GetSQLServerDataRootFolder:
    ' Stress that this is not a fatal error.
    Select Case ErrorDialog(Database, _
        "Cannot connect to the specified SQL Server instance to read default file locations. " & _
        "This will NOT prevent you from continuing the installation, " & _
        "but you will have to enter the file locations yourself.", _
        nButtons:=ebOK)
    Case erRetry
        Resume
    Case Else
        Resume EX_GetSQLServerDataRootFolder
    End Select

End Function
