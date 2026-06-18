Option Strict Off
Option Explicit On
Imports System
'developer guide no. 129    
Imports SharedFiles
Module Main
	
    'flag for having found an item to return to the screen
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_bFoundValues As Boolean
	
	Public m_lReturn As gPMConstants.PMEReturnCode
	
	
	'constants
	Public Const ACAPP As String = "Find Control"
	Public Const ACClass As String = "Product Builder"

	Public PMProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	'control type
	Public Const ACText As Integer = 1
	Public Const ACCombo As Integer = 1
	
	' mapping data array positions
	Public Const kMappingFindControlId As Integer = 0
	Public Const kMappingControlIndex As Integer = 1
	Public Const kMappingViewFieldName As Integer = 2
	Public Const kMappingControlType As Integer = 3
	Public Const kMappingFuzzy As Integer = 4
	Public Const kMappingViewName As Integer = 5
	Public Const kMappingSearchValue As Integer = 6
	Public Const kMappingFoundValue As Integer = 7
	Public Const kMappingGisObjectId As Integer = 8
	Public Const kMappingGisPropertyId As Integer = 9
	Public Const kMappingObjectName As Integer = 10
	Public Const kMappingPropertyName As Integer = 11
	Public Const kMappingGridCaption As Integer = 12
	Public Const kMappingGridPosition As Integer = 13
	Public Const kMappingGridWidth As Integer = 14
	
	
	Public Const ACProperty As Integer = 1
	Public Const ACControl As Integer = 2
	Public Const ACSearchValue As Integer = 6
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sUsername As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
	
	' ***************************************************************** '
	' Name: LogMessage
	'
	' Description: Wrapper function to the LogMessage method call.
	'
	' ***************************************************************** '
	'Public Sub LogMessage( _
	''    sUsername As String, iType As Integer, sMsg As String, Optional vApp As Variant, _
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
	'        If (lReturn& <> pmtrue) Then
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
End Module