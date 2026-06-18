Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bGisGroupImport"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	
	
	
	
	
	
	Sub Main_Renamed()
		
		
	End Sub
	
	' RDC 02082002 removed suring scalability changes
	' ***************************************************************** '
	' Name: LogMessage
	'
	' Description: Wrapper function to the log message method of the
	'              message object.
	'
	' ***************************************************************** '
	'Public Sub LogMessage(iType As Integer, sMsg As String, Optional vApp As Variant, _
	''        Optional vClass As Variant, Optional vMethod As Variant, _
	''        Optional vErrNo As Variant, Optional vErrDesc As Variant)
	'
	'Dim lErrorValue As Long
	'Dim vTimestamp As Variant
	'Dim lMessageID As Long
	'Dim oPMMessage As bPMMessage.Business
	'
	'    On Error GoTo Err_LogMessage
	'
	'    ' Check if we need to log this message.
	'
	'    If (iType% > g_iLogLevel%) _
	''    Or (iType% > PMLogOnError) Then
	'        ' Do not need to log message
	'        Exit Sub
	'    End If
	'
	'    ' Create PMMessage
	'    Set oPMMessage = New bPMMessage.Business
	'
	'    ' Initialise PMMessage
	'    lErrorValue = oPMMessage.Initialise( _
	''        sUsername:=g_sUsername, _
	''        sPassword:=g_sPassword, _
	''        iUserID:=g_iUserID, _
	''        iSourceID:=g_iSourceID, _
	''        iLanguageID:=g_iLanguageID, _
	''        iCurrencyID:=g_iCurrencyID, _
	''        iLogLevel:=g_iLogLevel, _
	''        sCallingAppName:=g_sCallingAppName)
	'
	'    If (lErrorValue <> PMTrue) Then
	'
	'        Set oPMMessage = Nothing
	'
	'        ' We cannot Initialise PMMessage, Log to Screen
	'        LogMessagePopup _
	''            iType:=iType%, _
	''            sMsg:=sMsg$, _
	''            vApp:=vApp, _
	''            vClass:=vClass, _
	''            vMethod:=vMethod, _
	''            vErrNo:=vErrNo, _
	''            vErrDesc:=vErrDesc
	'
	'        Exit Sub
	'
	'    End If
	'
	'    ' If Error Type is not supplied, set it to Fatal Error
	'    If (iType% = 0) Then
	'        iType% = PMLogFatal
	'    End If
	'
	'    ' Set the Optional Parameters to a default value if they are not supplied.
	'    If (IsMissing(vErrNo) = True) Then
	'        vErrNo = 0
	'    End If
	'
	'    If (IsMissing(vErrDesc) = True) Then
	'        vErrDesc = ""
	'    End If
	'
	'    If (IsMissing(vApp) = True) Then
	'        vApp = ""
	'    End If
	'
	'    If (IsMissing(vClass) = True) Then
	'        vClass = ""
	'    End If
	'
	'    If (IsMissing(vMethod) = True) Then
	'        vMethod = ""
	'    End If
	'
	'    ' Log message.
	'    lErrorValue& = oPMMessage.Add(lMessageID, g_iSourceID, g_sUsername, _
	''        Now, iType, g_sCallingAppName, sMsg, CLng(vErrNo), _
	''        CStr(vErrDesc), CStr(vApp), CStr(vClass), CStr(vMethod), vTimestamp)
	'
	'    Set oPMMessage = Nothing
	'
	'    Exit Sub
	'
	'Err_LogMessage:
	'
	'    ' Error Section.
	'
	'    ' Display the Message we tried to add.
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
	'
	'
End Module