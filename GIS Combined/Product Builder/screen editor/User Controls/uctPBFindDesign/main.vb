Option Strict Off
Option Explicit On
Imports System
'Modified by Alkesh Kumar on 11/05/2010 16:26:48 refer developer guide no. 129
Imports SharedFiles
Module main
	
	'the id of this particular control on the risk screen ( from GIS screen details )
	
	'used to transfer data to the list form
    	'Modified by Alkesh Kumar on 11/05/2010 16:27:20 refer developer guide no. 17
    	'Public m_vDataArray(,) As String
	Public m_vDataArray As Object
	
	'used to hold risk screen details ( screendetailID )
	
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vMappings As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_iMapCount As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_lReturn As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_iLineCount As Integer
	
	'constants
	Public Const ACAPP As String = "Find Control"
	Public Const ACClass As String = "Product Builder"

	Public PMProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	'control type
	Public Const ACText As Integer = 1
	Public Const ACCombo As Integer = 1
	
	''m_vdataarray column meanings
	'Public Const ACFindControlID = 0
	'Public Const ACControlIndex = 1
	'Public Const ACFieldName = 2
	'Public Const ACControlType = 3
	'Public Const ACFuzzy = 4
	'Public Const ACViewName = 5
	'Public Const ACSearchValue = 6
	'Public Const ACFoundValue = 7
	'Public Const ACGisObjectId = 8
	'Public Const ACGisPropertyId = 9
	'Public Const ACGisObjectName = 10
	'Public Const ACGisPropertyName = 11
	'Public Const kFindGridCustomCaption = 12
	'Public Const kFindGridPosition = 13
	'Public Const kFindGridCustomWidth = 14
	'Public Const ACLastValue = 14
	
	Public Const ACProperty As Integer = 1
	Public Const ACControl As Integer = 2
	
    Public Const ACLineX1 As Integer = 160 '3120
    Public Const ACLineX2 As Integer = 204 '4080
    Public Const ACGap As Integer = 11 '210
    Public Const ACGap2 As Integer = 12 '225
	
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
	Public Const kMappingLastValue As Integer = 14
	
	' screen control list view
	Public Const kSubItemType As Integer = 1
	Public Const kSubItemIndex As Integer = 2
	Public Const kSubItemGridCaption As Integer = 3
	Public Const kSubItemGridWidth As Integer = 4
	Public Const kSubItemGridPosition As Integer = 5
	
	
	' mapping data array positions
	Public Const kGridColumnBlank As Integer = 0
	Public Const kGridColumnMappedTo As Integer = 1
	Public Const kGridColumnControls As Integer = 2
	Public Const kGridColumnType As Integer = 3
	Public Const kGridColumnGridCaption As Integer = 4
	Public Const kGridColumnGridWidth As Integer = 5
	Public Const kGridColumnGridPosition As Integer = 6
	Public Const kGridColumnIndex As Integer = 7
	Public Const kGridColumnGisObjectId As Integer = 8
	Public Const kGridColumnGisPropertyId As Integer = 9
	Public Const kGridColumnObjectName As Integer = 10
	Public Const kGridColumnPropertyName As Integer = 11
	
	Public Const kControlTypeText As String = "Text"
	Public Const kControlTypeList As String = "List"
	Public Const kControlTypeIdText As Integer = 1
	Public Const kControlTypeIdList As Integer = 2
	
	Public Const kFirstEditableCol As Integer = kGridColumnGridCaption
	Public Const kLastEditableCol As Integer = kGridColumnGridPosition
	
	Public Const kGridNotMapped As String = "Not Mapped"
	
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