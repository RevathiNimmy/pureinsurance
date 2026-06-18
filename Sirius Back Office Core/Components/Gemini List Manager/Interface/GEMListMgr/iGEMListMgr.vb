Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 10/02/1999
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iGEMListMgr"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACTabTitle2 As Integer = 102
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	' Menus
	
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	'MN160799 - Use HKJ instead option
	Public Const ACCommercialDictionary As Integer = 0
	Public Const ACHKJDictionary As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_bIgnoreActivate As Boolean
	
	' 10/02/2004 - GP - Function duplicate in iPMFunc
	'' ***************************************************************** '
	'' Name: GetResData
	''
	'' Description: Gets a data value from the resource file using the
	''              ID passed.
	''
	'' ***************************************************************** '
	'Public Function GetResData(iLangID As Integer, lID As Long, _
	''        iDataType As Integer) As Variant
	'
	'Dim lLangID As Long
	'
	'    On Error GoTo Err_GetResData
	'
	'    ' Get data value from the resource file using
	'    ' the data type to determine what type of value
	'    ' to retrieve.
	'
	'    ' If language ID is missing assume English
	'    If iLangID% < 1 Then
	'        iLangID% = 1
	'    End If
	'
	'    ' Calculate language offset from the language
	'    ' ID passed.
	'    lLangID& = (iLangID% * PMLangOffSetValue) + lID&
	'
	'    Select Case (iDataType%)
	'        Case PMResString
	'            ' Get string value.
	'            GetResData = LoadResString(lLangID&)
	'
	'        Case PMResBitmap
	'            ' Get bitmap value.
	'            GetResData = LoadResPicture(lLangID&, vbResBitmap)
	'
	'        Case PMResIcon
	'            ' Get Icon value.
	'            GetResData = LoadResPicture(lLangID&, vbResIcon)
	'
	'        Case PMResCursor
	'            ' Get cursor value.
	'            GetResData = LoadResPicture(lLangID&, vbResCursor)
	'    End Select
	'
	'    Exit Function
	'
	'Err_GetResData:
	'
	'    ' Error Section.
	'
	'    GetResData = ""
	'
	'    ' Log Error.
	'    LogMessagePopup _
	''        iType:=PMLogOnError, _
	''        sMsg:="Failed to get data from the resource file", _
	''        vApp:=ACApp, _
	''        vClass:=ACClass, _
	''        vMethod:="GetResData", _
	''        vErrNo:=Err.Number, _
	''        vErrDesc:=Err.Description
	'
	'    Exit Function
	'
	'End Function
	'
	
	' ***************************************************************** '
	' Module Name: SubStr
	'
	' Date: 15/01/1999
	'
	' Description: Function to return a substring
	'
	' ***************************************************************** '
	Public Function SubStr(ByRef sString As String, ByRef iStrNum As Integer, Optional ByRef sSep As String = "") As String
		
		Dim result As String = String.Empty
		Dim iStartPos, iEndPos, iPos As Integer
		
		Try 
			
			result = ""
			
			If iStrNum = 0 Then
				Return result
			End If
			
			'Use comma if separator is missing

			If Information.IsNothing(sSep) Then
				sSep = ","
			End If
			
			iStartPos = 1
			iEndPos = 0
			iPos = 1
			
			For iPtr As Integer = 1 To iStrNum
				
				iPos = Strings.InStr(iPos, sString, sSep)
				
				iStartPos = iEndPos + 1
				
				iEndPos = iPos
				
				If iEndPos = 0 Then
					iPos = sString.Length + 1
					iEndPos = iPos
				End If
				
				iPos += 1
				
			Next iPtr
			
			'Get the sub string position
			
			Return Mid(sString, iStartPos, iEndPos - iStartPos).Trim()
		
		Catch 
			
			
			Return result
		End Try
	End Function
	
	Sub Main_Renamed()
		
	End Sub
End Module
