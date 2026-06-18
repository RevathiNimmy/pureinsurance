Option Strict Off
Option Explicit On
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
	Public Const ACApp As String = "iGEMListManager"
	
	' {* USER DEFINED CODE (Begin) *}
	
	' {* USER DEFINED CODE (End) *}
	
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
	
	Public Declare Function GetSystemDirectory Lib "kernel32"  Alias "GetSystemDirectoryA"(ByVal lpBuffer As String, ByVal nSize As Integer) As Integer
	
	Public Const GEMRegKeyListManagement As String = "ListManagement"
	
	'MN300699 - More constants for HKJ registry settings
	Public Const GEMClientListVersion As String = "ClientListVersion"
	Public Const GEMClientHKJListVersion As String = "ClientHKJListVersion"
	Public Const GEMClientListPrefVersion As String = "ClientListPrefVersion"
	Public Const GEMClientHKJListPrefVersion As String = "ClientHKJListPrefVersion"
	Public Const GEMClientListFilePath As String = "ClientListFilePath"
	Public Const GEMClientHKJListFilePath As String = "ClientHKJListFilePath"
	
	'RLDF Detail record
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Structure RLDFDetailRecord
        'developer guide no. 130(Guide)
        <VBFixedString(10)> Public PropertyId As String
        'developer guide no. 130(Guide)
        <VBFixedString(70)> Public Description As String
        'developer guide no. 130(Guide)
        <VBFixedString(10)> Public ABICode As String
		Public Shared Function CreateInstance() As RLDFDetailRecord
			Dim result As New RLDFDetailRecord
            'developer guide no. 130(Guide)
            Return result
		End Function
	End Structure
	
	'RLDF Index record
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Structure RLDFIndexRecord
        ' developer guide no. 130 (Guide)
        <VBFixedString(10)> Public PropertyId As String
		Dim RecordNumber As Integer
		Public Shared Function CreateInstance() As RLDFIndexRecord
			Dim result As New RLDFIndexRecord
            'developer guide no. 130(Guide)
            Return result
		End Function
	End Structure
	
	Public Const GEMMaxListItems As Integer = 500
	
	
	
	Sub Main_Renamed()
		
	End Sub
End Module