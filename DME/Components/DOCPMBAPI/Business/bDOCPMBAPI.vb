Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  03/12/97
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bDOCPMBAPI"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Username.
	Public g_sUsername As String = ""
	
	' Password.
	Public g_sPassword As New FixedLengthString(30)
	
	' User ID
	Public g_iUserID As Integer
	
	' Calling Application
	Public g_sCallingAppName As String = ""
	' Source ID
	Public g_iSourceID As Integer
	' Language ID
	Public g_iLanguageID As Integer
	' Currency ID
	Public g_iCurrencyID As Integer
	' LogLevel
	Public g_iLogLevel As Integer
	
	'Indicate daemon has been cancelled
	Public g_bCancelProcessing As Boolean
	
	'Indicate daemon has been paused
	Public g_bPauseProcessing As Boolean
	
	'Holds data from PMB control files
	Structure g_utControlData
		Dim task As String
		Dim cabinetname As String
		Dim drawername As String
		Dim foldername As String
		Dim linkfolder As String
		Dim documentname As String
		Dim keywords As String
		Dim event_Renamed As String
		Dim doctype As String
		Dim filename As String
		Dim annotation As String
		Dim access As Integer
		Dim hdbonly As Integer
		Dim username As String
		Dim emptyonly As Integer
		Dim message As String
		Dim external As Integer
		Public Shared Function CreateInstance() As g_utControlData
			Dim result As New g_utControlData
			result.task = String.Empty
			result.cabinetname = String.Empty
			result.drawername = String.Empty
			result.foldername = String.Empty
			result.linkfolder = String.Empty
			result.documentname = String.Empty
			result.keywords = String.Empty
			result.doctype = String.Empty
			result.filename = String.Empty
			result.annotation = String.Empty
			result.username = String.Empty
			result.message = String.Empty
			Return result
		End Function
	End Structure
	
	' Record type containing DMS data for History file.
	' Record Length = 290
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Structure g_utDMSHistData
		<VBFixedString(20),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=20)> _
		Public cabinetcode As FixedLengthString
		<VBFixedString(30),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=30)> _
		Public cabinetname As FixedLengthString
		<VBFixedString(20),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=20)> _
		Public drawercode As FixedLengthString
		<VBFixedString(30),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=30)> _
		Public drawername As FixedLengthString
		<VBFixedString(20),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=20)> _
		Public foldercode As FixedLengthString
		<VBFixedString(70),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=70)> _
		Public foldername As FixedLengthString
		<VBFixedString(9),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=9)> _
		Public docref As FixedLengthString
		<VBFixedString(8),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=8)> _
		Public date_Renamed As FixedLengthString
		<VBFixedString(6),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=6)> _
		Public time As FixedLengthString
		<VBFixedString(1),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=1)> _
		Public eventtype As FixedLengthString
		<VBFixedString(30),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=30)> _
		Public description As FixedLengthString
		<VBFixedString(20),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=20)> _
		Public volume As FixedLengthString
		<VBFixedString(10),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=10)> _
		Public pagefile As FixedLengthString
		<VBFixedString(3),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=3)> _
		Public doctype As FixedLengthString
		Dim hdError As Integer
		Dim create_date As Double
		<VBFixedString(3),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=3)> _
		Public filler As FixedLengthString
		Public Shared Function CreateInstance() As g_utDMSHistData
			Dim result As New g_utDMSHistData
			result.cabinetcode = New FixedLengthString(20)
			result.cabinetname = New FixedLengthString(30)
			result.drawercode = New FixedLengthString(20)
			result.drawername = New FixedLengthString(30)
			result.foldercode = New FixedLengthString(20)
			result.foldername = New FixedLengthString(70)
			result.docref = New FixedLengthString(9)
			result.time = New FixedLengthString(6)
			result.eventtype = New FixedLengthString(1)
			result.description = New FixedLengthString(30)
			result.volume = New FixedLengthString(20)
			result.pagefile = New FixedLengthString(10)
			result.doctype = New FixedLengthString(3)
			result.filler = New FixedLengthString(3)
			Return result
		End Function
	End Structure
	
	' Parameters passed to the DMS History DLL.
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Structure g_utDMSHistParams
		Dim NewRec As g_utDMSHistData
		<VBFixedString(80),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=80)> _
		Public DMSDir As FixedLengthString
		<VBFixedString(4),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=4)> _
		Public ReturnCode As FixedLengthString
		<VBFixedString(2),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=2)> _
		Public FileStatus As FixedLengthString
		Public Shared Function CreateInstance() As g_utDMSHistParams
			Dim result As New g_utDMSHistParams
			result.NewRec = g_utDMSHistData.CreateInstance()
			result.DMSDir = New FixedLengthString(80)
			result.ReturnCode = New FixedLengthString(4)
			result.FileStatus = New FixedLengthString(2)
			Return result
		End Function
	End Structure
	
	' New parameter for 32bit DLL (cannot pass user defined types
	' any more)
	Public g_sHistParam As New FixedLengthString(376)
	
	Declare Function GetPrivateProfileString Lib "kernel32"  Alias "GetPrivateProfileStringA"(ByVal lpApplicationName As String, ByVal lpKeyName As Integer, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
	
	Sub Main_Renamed()
		
		
	End Sub
End Module