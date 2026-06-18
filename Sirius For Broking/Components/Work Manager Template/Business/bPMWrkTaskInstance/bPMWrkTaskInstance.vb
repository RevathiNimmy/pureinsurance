Option Strict Off
Option Explicit On
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  17 October 1996
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bPMWrkTaskInstanceTemp"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Username.
    'Public g_sUsername As String * 12

    ' Password.
    'Public g_sPassword As String * 30

    ' Calling Application
    'Public g_sCallingAppName As String
    ' Source ID
    'Public g_iSourceID As Integer
    ' Language ID
    'Public g_iLanguageID As Integer
    ' Currency ID
    'Public g_iCurrencyID As Integer
    ' LogLevel
    'Public g_iLogLevel As Integer
    ' UserID
    'Public g_iUserID As Integer
    'developer guide no. 107
    <ThreadStatic()>
    Public g_bInstance As Boolean

    Sub main_Renamed()

        ' Main entry point for the component

    End Sub

    '' ***************************************************************** '
    '' Name: LogMessage
    ''
    '' Description: Wrapper function to the log message method of the
    ''              message object.
    ''
    '' ***************************************************************** '
    'Public Sub LogMessage(iType As Integer, sMsg As String, Optional vApp As Variant, _
    ''        Optional vClass As Variant, Optional vMethod As Variant, _
    ''        Optional vErrNo As Variant, Optional vErrDesc As Variant)
    '
    'Dim lErrorValue As Long
    '
    '    On Error GoTo Err_LogMessage
    '
    '    ' Log Message to File
    '    LogMessagePopup _
    ''        iType:=iType%, _
    ''        sMsg:=sMsg$, _
    ''        vApp:=vApp, _
    ''        vClass:=vClass, _
    ''        vMethod:=vMethod, _
    ''        vErrNo:=vErrNo, _
    ''        vErrDesc:=vErrDesc
    '
    '    Exit Sub
    '
    'Err_LogMessage:
    '
    '    ' Error Section.
    '
    '    ' Failed to log message, so we must call the
    '    ' function to popup the message instead.
    '    LogMessagePopup _
    ''        iType:=iType%, _
    ''        sMsg:=sMsg$, _
    ''        vApp:=vApp, _
    ''        vClass:=vClass, _
    ''        vMethod:=vMethod, _
    ''        vErrNo:=vErrNo, _
    ''        vErrDesc:=vErrDesc
    '
    '    Exit Sub
    '
    'End Sub


    'Sub TEST()
    '
    '
    'Dim oUser As New bPMUser.Business
    'Dim lerror As Long
    'Dim lRow As Long
    'Dim iUserID As Integer
    'Dim iLanguageID As Integer
    'Dim sUsername As String
    'Dim sPassword As String
    'Dim dtPasswordChangeDate As Date
    'Dim dtDateCreated As Date
    'Dim dtLastLogin As Date
    'Dim lPartyCnt As Long
    'Dim iIsDeleted As Integer
    'Dim dtEffectiveDate As Date
    '
    'lerror = oUser.Initialise(sUsername:="1", sPassword:="1", iUserID:=1, iSourceID:=1, iLanguageID:=1, _
    ''    iCurrencyID:=1, iLogLevel:=1, sCallingAppName:="Test")
    '
    'lerror = oUser.CheckLogon(sCheckUsername:="RobC", sCheckPassword:="xHGwNn", dtEffectiveFrom:=CDate("17-AUG-1996"), iLanguageID:=1, lPartyCnt:=0)
    ''lerror = oUser.Add(iUserID:=0, iLanguageID:=1, sUsername:="Hannibal", _
    ''    sPassword:="Lecter", dtPasswordChangeDate:=Now, _
    ''    dtDateCreated:=Now, dtLastLogin:=Now, lPartyCnt:=7, iIsDeleted:=1, _
    ''    dtEffectiveDate:=Now, vTimestamp:=0)
    '
    ''lerror = oUser.GetDetails()
    '
    ''lerror = oUser.GetNext(iUserID, iLanguageID, sUsername, sPassword, dtPasswordChangeDate, _
    ''    dtDateCreated, dtLastLogin, lPartyCnt, iIsDeleted, dtEffectiveDate)
    '
    '
    '
    '
    'sUsername = "RobC"
    'sPassword = "xHGwNn"
    'iLanguageID = 1
    'lRow = 1
    '
    ''lerror = oUser.EditUpdate(lRow, iUserID, iLanguageID, sUsername, sPassword, dtPasswordChangeDate, _
    ''    dtDateCreated, dtLastLogin, lPartyCnt, iIsDeleted, _
    ''    dtEffectiveDate)
    '
    ''lerror = oUser.EditUpdate(lRow, vPassword:=sPassword)
    '
    'lerror = oUser.EditAdd(lRow:=lRow, vUserId:=1, _
    ''    vLanguageID:=1, vUsername:="Graham", vPassword:="Bell")
    '
    ''lerror = oUser.EditDelete(lRow)
    '
    'If (lerror <> PMFalse = True) Then
    '    lerror = oUser.Update
    'Else
    '    MsgBox "failed"
    'End If
    '
    'Set oUser = Nothing
    '
    'End Sub
    '
    '
End Module