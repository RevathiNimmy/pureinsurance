Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Runtime.InteropServices
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  19/08/1998
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bGEMListUpdate"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	
	
	
	
	'------------------------------------------------------------------------
	Public Declare Function GetSystemDirectory Lib "kernel32"  Alias "GetSystemDirectoryA"(ByVal lpBuffer As String, ByVal nSize As Integer) As Integer
	
	Public Const GEMRegKeyListManagement As String = "ListManagement"
	Public Const GEMRegKeyListPolaris As String = "Polaris"
	
	' Registry file paths
	Public Const GEMServerListVersion As String = "ServerListVersion"
	Public Const GEMHKJServerListVersion As String = "ServerHKJListVersion"
	Public Const GEMServerListPrefVersion As String = "ServerListPrefVersion"
	Public Const GEMServerHKJListPrefVersion As String = "ServerHKJListPrefVersion"
	Public Const GEMServerListFileCompressed As String = "ServerListFileCompressed"
	Public Const GEMServerHKJListFileCompressed As String = "ServerHKJListFileCompressed"
	Public Const GEMServerListFilePath As String = "ServerListFilePath"
	Public Const GEMServerHKJListFilePath As String = "ServerHKJListFilePath"
	Public Const GEMCoverDatFilePath As String = "CoverDatFilePath"
	
	Public Const GEMServerPolarisFilePath As String = "ServerPolarisFilePath"
	Public Const GEMServerHKJPolarisFilePath As String = "ServerHKJPolarisFilePath"
	Public Const GEMCommonPolarisAppVer As String = "AppVer"
	Public Const GEMCommonHKJPolarisAppVer As String = "HKJAppVer"
	
	'sj 19/09/2000 - start
	'Changes for windows product writer
	
	'Polaris input file record
	'Type InputRecord
	'    PropertyId As String * 12
	'    Description As String * 72
	'    ABICode As String * 13
	'End Type
	
	'Polaris input file record
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Structure InputRecord
		<VBFixedString(12),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=12)> _
		Public PropertyId As FixedLengthString
		<VBFixedString(70),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=70)> _
		Public Description As FixedLengthString
		<VBFixedString(10),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=10)> _
		Public ABICode As FixedLengthString
		<VBFixedString(18),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=18)> _
		Public Rest As FixedLengthString
		Public Shared Function CreateInstance() As InputRecord
			Dim result As New InputRecord
			result.PropertyId = New FixedLengthString(12)
			result.Description = New FixedLengthString(70)
			result.ABICode = New FixedLengthString(10)
			result.Rest = New FixedLengthString(18)
			Return result
		End Function
	End Structure
	'sj 19/09/2000 - end
	
	'RLDF Header record
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Structure RLDFHeaderRecord
		<VBFixedString(10),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=10)> _
		Public PropertyId As FixedLengthString
		<VBFixedString(80),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=80)> _
		Public Filler As FixedLengthString
		Public Shared Function CreateInstance() As RLDFHeaderRecord
			Dim result As New RLDFHeaderRecord
			result.PropertyId = New FixedLengthString(10)
			result.Filler = New FixedLengthString(80)
			Return result
		End Function
	End Structure
	
	'RLDF Detail record
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Structure RLDFDetailRecord
		<VBFixedString(10),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=10)> _
		Public PropertyId As FixedLengthString
		<VBFixedString(70),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=70)> _
		Public Description As FixedLengthString
		<VBFixedString(10),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=10)> _
		Public ABICode As FixedLengthString
		Public Shared Function CreateInstance() As RLDFDetailRecord
			Dim result As New RLDFDetailRecord
			result.PropertyId = New FixedLengthString(10)
			result.Description = New FixedLengthString(70)
			result.ABICode = New FixedLengthString(10)
			Return result
		End Function
	End Structure
	
	'RLDF Index record
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Structure RLDFIndexRecord
		<VBFixedString(10),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=10)> _
		Public PropertyId As FixedLengthString
		Dim RecordNumber As Integer
		Public Shared Function CreateInstance() As RLDFIndexRecord
			Dim result As New RLDFIndexRecord
			result.PropertyId = New FixedLengthString(10)
			Return result
		End Function
	End Structure
	
	' Polaris Property array subscripts
	Public Const ACPAPropertyId As Integer = 0
	Public Const ACPARecordIndex As Integer = 1
	Public Const ACPACustom As Integer = 2
	
	
	
	
	
	

	Public Sub Main()
		
		
	End Sub
	'UPGRADE_NOTE: (7013) Constructor is just executed once. Please review if Component contains SingleUse classes because they have a different behaviour. More Information: http://www.vbtonet.com/ewis/ewi7013.aspx
	Sub New()
		Main()
	End Sub
	Sub JustForInvokeMain()
	End Sub
End Module