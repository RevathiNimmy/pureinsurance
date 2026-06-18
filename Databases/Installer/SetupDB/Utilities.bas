Attribute VB_Name = "MUtilities"
' Module:   Small stateless utility functions
' Shared:   No
'
Option Explicit

' Pops up an error dialog which logs to the correct log file, but
' doesn't display any current error information, just the message
' provided.
Public Sub WarningDialog(ByVal db As CDatabase, _
    ByVal sMessage As String, _
    Optional ByVal sTitle As String = "", _
    Optional ByVal sLogInfo As String = "")

    db.ErrorHandler.DialogV2 sMessage, _
        sTitle:=sTitle, _
        nButtons:=ebOK, _
        sLogInfo:=sLogInfo

End Sub

' Logs an error report which logs to the correct log file, but
' doesn't display any current error information, just the message
' provided.
Public Sub WarningReport(ByVal db As CDatabase, _
    ByVal sMessage As String, _
    Optional ByVal sTitle As String = "", _
    Optional ByVal sLogInfo As String = "")

    db.ErrorHandler.ReportV2 sMessage, _
        sTitle:=sTitle, _
        sLogInfo:=sLogInfo

End Sub
