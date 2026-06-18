Option Strict Off
Option Explicit On
Imports System
Imports SSP.Shared
Module Stuff
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	Public Const ACApp As String = "bPBFindCOntrol"
	

	Public PMProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	'***********************************************
	' TO DO: GLOBAL VARIABLE IN BAS MODULE
	'Public m_lReturn As Long
	'***********************************************
	
	'm_vdataarray column meanings
	Public Const ACFindControlID As Integer = 0
	Public Const ACControlIndex As Integer = 1
	Public Const ACFieldName As Integer = 2
	Public Const ACControlType As Integer = 3
	Public Const ACFuzzy As Integer = 4
	Public Const ACViewName As Integer = 5
	Public Const ACSearchValue As Integer = 6
	Public Const ACFoundValue As Integer = 7
	
	Public Const ACProperty As Integer = 1
	Public Const ACControl As Integer = 2
	
	
	'Public Sub LogMessage( _
	''    sUserName As String, iType As Integer, sMsg As String, Optional vApp As Variant, _
	''    Optional vClass As Variant, Optional vMethod As Variant, _
	''    Optional vErrNo As Variant, Optional vErrDesc As Variant)
	'
	'Dim lReturn As Long
	'Dim oMessage As Object
	'
	'    ' CTAF 270701
	'    On Error Resume Next
	'
	'    ' Create an instance of the message object
	'    Set oMessage = CreateObject("iPMMessage.PMMessageV2")
	'
	'    ' CTAF 270701
	'    On Error GoTo Err_LogMessage
	'
	'    If ((oMessage Is Nothing) = False) Then
	'
	'        ' Log the message
	'        lReturn& = oMessage.LogMessage( _
	''                        iType:=iType, _
	''                        sMsg:=sMsg, _
	''                        vApp:=vApp, _
	''                        vClass:=vClass, _
	''                        vMethod:=vMethod, _
	''                        vErrNo:=vErrNo, _
	''                        vErrDesc:=vErrDesc)
	'        If (lReturn& <> PMTrue) Then
	'            ' If it fails, then
	'            LogMessagePopup _
	''                iType:=iType%, _
	''                sMsg:=sMsg$, _
	''                vApp:=vApp, _
	''                vClass:=vClass, _
	''                vMethod:=vMethod, _
	''                vErrNo:=vErrNo, _
	''                vErrDesc:=vErrDesc
	'        End If
	'
	'        Set oMessage = Nothing
	'
	'    Else
	'
	'        ' CTAF 270701 - Log the message as normal instead
	'
	'        ' Failed to log message, so we must call the
	'        ' function to popup the message instead.
	'        LogMessagePopup _
	''            iType:=iType%, _
	''            sMsg:=sMsg$, _
	''            vApp:=vApp, _
	''            vClass:=vClass, _
	''            vMethod:=vMethod, _
	''            vErrNo:=vErrNo, _
	''            vErrDesc:=vErrDesc
	'
	'    End If
	'
	'
	'    Exit Sub
	'Err_LogMessage:
	'
	'End Sub
End Module