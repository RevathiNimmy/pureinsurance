Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Runtime.InteropServices
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 16/09/1998
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iGISListManager"
	
	' {* USER DEFINED CODE (Begin) *}
	
	' {* USER DEFINED CODE (End) *}
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' RDC 31052001
	Public g_sUsername As String = ""
    ' developer guide no. 107(Guide)	
	' Public source and language ID's from the
    ' Object Manager.

    'Public g_iSourceID As Integer
    'Public g_iLanguageID As Integer
	' Public instance of the object manager.
    'Public g_oObjectManager As bObjectManager.ObjectManager
	
	Public Declare Function GetSystemDirectory Lib "kernel32"  Alias "GetSystemDirectoryA"(ByVal lpBuffer As String, ByVal nSize As Integer) As Integer
	
	Public Const GEMRegKeyListManagement As String = "ListManagement"
	
	Public Const GEMClientListVersion As String = "ClientListVersion"
	Public Const GEMClientListPrefVersion As String = "ClientListPrefVersion"
	Public Const GEMClientListFilePath As String = "ClientListFilePath"
	
    ''RLDF Detail record
    'developer guide no. 107(Guide)	
    '<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
    ' _
    'Structure RLDFDetailRecord
    '	<VBFixedString(10),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=10)> _
    '	Public PropertyId As FixedLengthString
    '	<VBFixedString(70),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=70)> _
    '	Public Description As FixedLengthString
    '	<VBFixedString(10),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=10)> _
    '	Public ABICode As FixedLengthString
    '	Public Shared Function CreateInstance() As RLDFDetailRecord
    '		Dim result As New RLDFDetailRecord
    '		result.PropertyId = New FixedLengthString(10)
    '		result.Description = New FixedLengthString(70)
    '		result.ABICode = New FixedLengthString(10)
    '		Return result
    '	End Function
    'End Structure
	
    'RLDF Index record
    'developer guide no. 107(Guide)	
    '<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
    ' _
    'Structure RLDFIndexRecord
    '	<VBFixedString(10),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=10)> _
    '	Public PropertyId As FixedLengthString
    '	Dim RecordNumber As Integer
    '	Public Shared Function CreateInstance() As RLDFIndexRecord
    '		Dim result As New RLDFIndexRecord
    '		result.PropertyId = New FixedLengthString(10)
    '		Return result
    '	End Function
    'End Structure
	
	Public Const GEMMaxListItems As Integer = 2500
	Public Const RLDF_RECORD_LENGTH As Integer = 128
	Public Const RECORD_BUFFER As Integer = 256000
	Public Const RECORDS_PER_READ As Integer = 2000
    'developer guide no. 107(Guide)	
    '   Private _cFiles As Collection = Nothing
    'Public Property cFiles() As Collection
    '	Get
    '		If _cFiles Is Nothing Then
    '			_cFiles = New Collection()
    '		End If
    '		Return _cFiles
    '	End Get
    '	Set(ByVal Value As Collection)
    '		_cFiles = value
    '	End Set
    'End Property
	
	
	
	Sub Main_Renamed()
		
	End Sub
	
	' RFC270701 -  iPMFunc REMOVED and The following has been added so that Error Messages do NOT
	' get logged to the screen. Remember this component gets called within an IIS/ASP environment.
	' ***************************************************************** '
	' Name: LogMessage
	'
	' Description: Wrapper function to the log message method of the
	'              message object.
	'
	' ***************************************************************** '
	'Public Sub LogMessage(sUsername As String, iType As Integer, sMsg As String, Optional vApp As Variant, _
	''        Optional vClass As Variant, Optional vMethod As Variant, _
	''        Optional vErrNo As Variant, Optional vErrDesc As Variant)
	'
	'Dim lErrorValue As Long
	'Dim vTimestamp As Variant
	'Dim lMessageID As Long
	'
	'    On Error GoTo Err_LogMessage
	'
	'    ' Check if we need to log this message.
	'
	'
	'    ' We cannot Initialise PMMessage, Log to Screen
	'    LogMessageToFile _
	''        sUsername:=g_sUsername, _
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
	'    Exit Sub
	'
	'End Sub
	
	' ************************************************************************ '
	' Name: ShellSort
	'
	' Description: Shell Sort half inched off the web.
	' A = Variant Array, Lb = Lower Bound of Array, Ub = Upper Bound of Array
	' ************************************************************************ '
	
	Public Sub ShellSort(ByRef A() As Object, ByVal Lb As Integer, ByVal Ub As Integer)
		
		Dim j As Integer
		Dim t As String = ""
		
		' sort array[lb..ub]
		
		' compute largest increment
		Dim n As Integer = Ub - Lb + 1
		Dim h As Integer = 1
		If n < 14 Then
			h = 1
		Else
			Do While h < n
				h = 3 * h + 1
			Loop 
			h = h \ 3
			h = h \ 3
		End If
		
		Do While h > 0
			' sort by insertion in increments of h
			For i As Integer = Lb + h To Ub

				t = CStr(A(i))
				For j = i - h To Lb Step -h

					If CStr(A(j)) <= t Then Exit For


					A(j + h) = A(j)
				Next j

				A(j + h) = t
			Next i
			h = h \ 3
		Loop 
		
	End Sub
End Module
