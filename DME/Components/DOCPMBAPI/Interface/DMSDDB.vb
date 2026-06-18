Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.DB.DAO
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Data
Imports System.Data.Common
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports SharedFiles

Module DMSDDB
	'*********************************************************************
	'
	' General Global Declaration
	'
	'*********************************************************************
	'
	
    Public g_dbDDB As DAO.Database ' DDB Database name
    Public g_dbSDB As DAO.Database ' Scan Database name
	Public g_iDBType As Integer
	
	'Global g_ssSnapshot As Snapshot         ' Now defined in PMFUNC.BAS
	Public g_ssDrawer() As DAO.Snapshot
	
	Public Const DB_ACCESS As Integer = 1 ' Database in use is MS Access
	Public Const DB_SQLSERVER As Integer = 2 ' Database in use is MS SQL Server
	Public Const DB_ODBC As Integer = 3 ' Database in use is ODBC
	
	Public Const CABINET As Integer = 1 '
	Public Const DRAWER As Integer = 2 '
	Public Const FOLDER As Integer = 3 '
	Public Const DOCUMENT As Integer = 4 ' Constants for all functions
	Public Const PAGE As Integer = 5 '
	Public Const keywords As Integer = 6 '
	Public Const DOCINFO As Integer = 7 '
	Public Const DEVICE As Integer = 8 '
	Public Const DEVICETYPE As Integer = 9 '
	Public Const LOGIN As Integer = 10 '
	Public Const RECORDLOCK As Integer = 11 '
	Public Const DOCHISTORY As Integer = 12
	Public Const dmsSYSTEM As Integer = 13
	
	Public Const MAX_READ As Integer = 100 ' Maximum data read per block
	Public Const MAXDOCHISTORY As Integer = 9 ' Number of history records kept
	Public Const ALL As Integer = 1 '
	Public Const SELECTIVE As Integer = 2 '
	
	Public Const RL_SOFT As Integer = 1
	Public Const RL_HARD As Integer = 2
	
	Public Const NEXT_PAGE As Integer = 1
	Public Const PREVIOUS_PAGE As Integer = 2
	Public Const FIRST_PAGE As Integer = 3
	Public Const LAST_PAGE As Integer = 4
	
	Public Const NEXT_DOC As Integer = 1
	Public Const PREVIOUS_DOC As Integer = 2
	
	Public Structure g_utDrawerDataInfo
		Dim CabinetNumber As Integer
		Dim Loaded As Integer
	End Structure
	
	'*********************************************************************
	'
	' Global DDB Structure Declaration
	'
	'*********************************************************************
	'
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Public Structure g_utCabinet ' Cabinet structure
		Dim cabinet_num As Integer
		<VBFixedString(30),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=30)> _
		Public cabinet_name As FixedLengthString
		<VBFixedString(1),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=1)> _
		Public list_src As FixedLengthString
		Dim access_level As Integer
		<VBFixedString(12),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=12)> _
		Public password As FixedLengthString
		<VBFixedString(20),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=20)> _
		Public ex_code As FixedLengthString
		Dim link As Integer
		Public Shared Function CreateInstance() As g_utCabinet
			Dim result As New g_utCabinet
			result.cabinet_name = New FixedLengthString(30)
			result.list_src = New FixedLengthString(1)
			result.password = New FixedLengthString(12)
			result.ex_code = New FixedLengthString(20)
			Return result
		End Function
	End Structure
	
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Public Structure g_utDrawer ' Drawer structure
		Dim drawer_num As Integer
		<VBFixedString(30),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=30)> _
		Public drawer_name As FixedLengthString
		<VBFixedString(20),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=20)> _
		Public ex_code As FixedLengthString
		Dim cabinet_num As Integer
		Dim access_level As Integer
		<VBFixedString(12),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=12)> _
		Public password As FixedLengthString
		Dim link As Integer
		Public Shared Function CreateInstance() As g_utDrawer
			Dim result As New g_utDrawer
			result.drawer_name = New FixedLengthString(30)
			result.ex_code = New FixedLengthString(20)
			result.password = New FixedLengthString(12)
			Return result
		End Function
	End Structure
	
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Public Structure g_utFolder ' Folder structure
		Dim folder_num As Integer
		<VBFixedString(70),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=70)> _
		Public folder_name As FixedLengthString
		<VBFixedString(20),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=20)> _
		Public ex_code As FixedLengthString
		Dim drawer_num As Integer
		Dim access_level As Integer
		<VBFixedString(12),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=12)> _
		Public password As FixedLengthString
		Dim link As Integer
		Public Shared Function CreateInstance() As g_utFolder
			Dim result As New g_utFolder
			result.folder_name = New FixedLengthString(70)
			result.ex_code = New FixedLengthString(20)
			result.password = New FixedLengthString(12)
			Return result
		End Function
	End Structure
	
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Public Structure g_utDocument ' Document structure
		Dim doc_num As Integer
		<VBFixedString(30),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=30)> _
		Public doc_name As FixedLengthString
		Dim folder_num As Integer
		Dim access_level As Integer
		<VBFixedString(12),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=12)> _
		Public password As FixedLengthString
		Dim docset_num As Integer
		Dim create_date As Double
		<VBFixedString(20),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=20)> _
		Public ex_code As FixedLengthString
		Dim link As Integer
		<VBFixedString(1),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=1)> _
		Public doc_type As FixedLengthString
		Public Shared Function CreateInstance() As g_utDocument
			Dim result As New g_utDocument
			result.doc_name = New FixedLengthString(30)
			result.password = New FixedLengthString(12)
			result.ex_code = New FixedLengthString(20)
			result.doc_type = New FixedLengthString(1)
			Return result
		End Function
	End Structure
	
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Public Structure g_utPage ' Page structure
		<VBFixedString(15),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=15)> _
		Public page_name As FixedLengthString
		Dim doc_num As Integer
		<VBFixedString(20),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=20)> _
		Public volume_name As FixedLengthString
		<VBFixedString(20),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=20)> _
		Public backup_name As FixedLengthString
		<VBFixedString(3),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=3)> _
		Public page_type As FixedLengthString
		Dim page_num As Integer
		Dim scan_date As Double
		Dim orientation As Integer
		Public Shared Function CreateInstance() As g_utPage
			Dim result As New g_utPage
			result.page_name = New FixedLengthString(15)
			result.volume_name = New FixedLengthString(20)
			result.backup_name = New FixedLengthString(20)
			result.page_type = New FixedLengthString(3)
			Return result
		End Function
	End Structure
	
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Public Structure g_utUser ' User structure
		<VBFixedString(16),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=16)> _
		Public user_name As FixedLengthString
		<VBFixedString(12),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=12)> _
		Public password As FixedLengthString
		<VBFixedString(30),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=30)> _
		Public title As FixedLengthString
		Dim access_level As Integer
		Dim login_limit As Integer
		Dim retired As Integer
		Dim home_level As Integer
		Dim home_level_num As Integer
		Public Shared Function CreateInstance() As g_utUser
			Dim result As New g_utUser
			result.user_name = New FixedLengthString(16)
			result.password = New FixedLengthString(12)
			result.title = New FixedLengthString(30)
			Return result
		End Function
	End Structure
	
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Structure g_utLogin
		Dim login_num As Integer
		<VBFixedString(16),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=16)> _
		Public user_name As FixedLengthString
		Dim device_num As Integer
		Dim date_Renamed As Double
		Public Shared Function CreateInstance() As g_utLogin
			Dim result As New g_utLogin
			result.user_name = New FixedLengthString(16)
			Return result
		End Function
	End Structure
	
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Public Structure g_utKeyWord ' KeyWord structure
		Dim doc_num As Integer
		Dim key_num As Integer
		<VBFixedString(16),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=16)> _
		Public user_name As FixedLengthString
		Dim create_date As Double
		Public Shared Function CreateInstance() As g_utKeyWord
			Dim result As New g_utKeyWord
			result.user_name = New FixedLengthString(16)
			Return result
		End Function
	End Structure
	
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Public Structure g_utKeyWords ' KeyWords structure
		Dim key_num As Integer
		<VBFixedString(30),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=30)> _
		Public keyword As FixedLengthString
		Dim deleted As Integer
		Public Shared Function CreateInstance() As g_utKeyWords
			Dim result As New g_utKeyWords
			result.keyword = New FixedLengthString(30)
			Return result
		End Function
	End Structure
	
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Public Structure g_utAnnotations ' Annotations structure
		Dim doc_num As Integer
		<VBFixedString(50),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=50)> _
		Public ann_text As FixedLengthString
		<VBFixedString(16),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=16)> _
		Public user_name As FixedLengthString
		Dim create_date As Double
		Public Shared Function CreateInstance() As g_utAnnotations
			Dim result As New g_utAnnotations
			result.ann_text = New FixedLengthString(50)
			result.user_name = New FixedLengthString(16)
			Return result
		End Function
	End Structure
	
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Structure g_utFTAO ' FTAO structure
		Dim doc_num As Integer
		<VBFixedString(16),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=16)> _
		Public ftao_name As FixedLengthString
		Public Shared Function CreateInstance() As g_utFTAO
			Dim result As New g_utFTAO
			result.ftao_name = New FixedLengthString(16)
			Return result
		End Function
	End Structure
	
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Public Structure g_utDocInfo ' Document info structure
		Dim doc_num As Integer
		Dim expiry_date As Double
		<VBFixedString(30),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=30)> _
		Public scan_operator As FixedLengthString
		Dim doc_date As Double
		<VBFixedString(16),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=16)> _
		Public last_user As FixedLengthString
		Dim last_date As Double
		Public Shared Function CreateInstance() As g_utDocInfo
			Dim result As New g_utDocInfo
			result.scan_operator = New FixedLengthString(30)
			result.last_user = New FixedLengthString(16)
			Return result
		End Function
	End Structure
	
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Public Structure g_utVolume ' Volume structure
		<VBFixedString(20),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=20)> _
		Public volume_name As FixedLengthString
		Dim device_num As Integer
		Dim initial_date As Double
		<VBFixedString(16),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=16)> _
		Public initial_user As FixedLengthString
		Dim rewriteable As Integer
		Public Shared Function CreateInstance() As g_utVolume
			Dim result As New g_utVolume
			result.volume_name = New FixedLengthString(20)
			result.initial_user = New FixedLengthString(16)
			Return result
		End Function
	End Structure
	
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Structure g_utHotZone ' HotZone structure
		<VBFixedString(20),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=20)> _
		Public active_volume As FixedLengthString
		<VBFixedString(20),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=20)> _
		Public overflow_volume As FixedLengthString
		Public Shared Function CreateInstance() As g_utHotZone
			Dim result As New g_utHotZone
			result.active_volume = New FixedLengthString(20)
			result.overflow_volume = New FixedLengthString(20)
			Return result
		End Function
	End Structure
	
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Public Structure g_utDevice ' Device structure
		Dim device_num As Integer
		Dim dt_num As Integer
		<VBFixedString(20),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=20)> _
		Public server_name As FixedLengthString
		<VBFixedString(25),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=25)> _
		Public server_root As FixedLengthString
		<VBFixedString(20),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=20)> _
		Public mount_volume As FixedLengthString
		<VBFixedString(30),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=30)> _
		Public location As FixedLengthString
		Public Shared Function CreateInstance() As g_utDevice
			Dim result As New g_utDevice
			result.server_name = New FixedLengthString(20)
			result.server_root = New FixedLengthString(25)
			result.mount_volume = New FixedLengthString(20)
			result.location = New FixedLengthString(30)
			Return result
		End Function
	End Structure
	
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Public Structure g_utDeviceType ' DeviceType structure
		Dim dt_num As Integer
		<VBFixedString(20),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=20)> _
		Public device_type As FixedLengthString
		Dim fixed As Integer
		Dim rewriteable As Integer
		Dim line_type As Integer
		Public Shared Function CreateInstance() As g_utDeviceType
			Dim result As New g_utDeviceType
			result.device_type = New FixedLengthString(20)
			Return result
		End Function
	End Structure
	
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Structure g_utDeviceLock ' DeviceLock structure
		Dim device_num As Integer
		Dim mount As Integer
		<VBFixedString(16),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=16)> _
		Public user As FixedLengthString
		Public Shared Function CreateInstance() As g_utDeviceLock
			Dim result As New g_utDeviceLock
			result.user = New FixedLengthString(16)
			Return result
		End Function
	End Structure
	
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Structure g_utSystem ' System structure
		Dim LOGIN As Integer
		Dim doc_date As Integer
		Dim expiry_date As Integer
		Dim admin_level As Integer
		<VBFixedString(15),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=15)> _
		Public NEXT_PAGE As FixedLengthString
		Dim status As Integer
		Public Shared Function CreateInstance() As g_utSystem
			Dim result As New g_utSystem
			result.NEXT_PAGE = New FixedLengthString(15)
			Return result
		End Function
	End Structure
	
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Structure g_utRecordLock ' RecordLock structure
		Dim lock_num As Integer
		Dim lock_type As Integer
		Dim level As Integer
		Dim code As Integer
		<VBFixedString(16),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=16)> _
		Public user As FixedLengthString
		Dim date_Renamed As Double
		Public Shared Function CreateInstance() As g_utRecordLock
			Dim result As New g_utRecordLock
			result.user = New FixedLengthString(16)
			Return result
		End Function
	End Structure
	
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Public Structure g_utChildData ' Child form structure
		Dim iDeleted As Integer
		Dim iStartPage As Integer
		Dim iEndPage As Integer
		Dim iCurrPage As Integer
		Dim lDocumentNumber As Integer
		<VBFixedString(30),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=30)> _
		Public sDocumentName As FixedLengthString
		Dim iPassword As Integer
		<VBFixedString(32),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=32)> _
		Public sCabinetName As FixedLengthString
		Dim lCabinetNumber As Integer
		<VBFixedString(32),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=32)> _
		Public sDrawerName As FixedLengthString
		Dim lDrawerNumber As Integer
		<VBFixedString(72),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=72)> _
		Public sFolderName As FixedLengthString
		Dim lFolderNumber As Integer
		Dim iNumKeywords As Integer
		Dim iNumAnnotations As Integer
		Dim dDocDate As Double
		'salvo(231096) - Add these to store the values for the shared document
		<VBFixedString(32),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=32)> _
		Public sCabinetNameShare As FixedLengthString
		Dim lCabinetNumberShare As Integer
		<VBFixedString(32),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=32)> _
		Public sDrawerNameShare As FixedLengthString
		Dim lDrawerNumberShare As Integer
		<VBFixedString(72),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=72)> _
		Public sFolderNameShare As FixedLengthString
		Dim lFolderNumberShare As Integer
		Public Shared Function CreateInstance() As g_utChildData
			Dim result As New g_utChildData
			result.sDocumentName = New FixedLengthString(30)
			result.sCabinetName = New FixedLengthString(32)
			result.sDrawerName = New FixedLengthString(32)
			result.sFolderName = New FixedLengthString(72)
			result.sCabinetNameShare = New FixedLengthString(32)
			result.sDrawerNameShare = New FixedLengthString(32)
			result.sFolderNameShare = New FixedLengthString(72)
			Return result
		End Function
	End Structure
	
	Structure g_utSearchInfo ' Document Search info structure
		Dim cabinetname As String
		Dim drawername As String
		Dim foldername As String
		Dim documentname As String
		Dim annotation As String
		Dim docdatefrom As Double
		Dim docdateto As Double
		Dim scandatefrom As Double
		Dim scandateto As Double
		Dim expirydatefrom As Double
		Dim expirydateto As Double
		Dim lastaccessfrom As Double
		Dim lastaccessto As Double
		Dim lastuser As String
		Dim scanop As String
		Public Shared Function CreateInstance() As g_utSearchInfo
			Dim result As New g_utSearchInfo
			result.cabinetname = String.Empty
			result.drawername = String.Empty
			result.foldername = String.Empty
			result.documentname = String.Empty
			result.annotation = String.Empty
			result.lastuser = String.Empty
			result.scanop = String.Empty
			Return result
		End Function
	End Structure
	
	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
	 _
	Structure g_utParentDetails
		Dim CabinetNumber As Integer
		<VBFixedString(30),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=30)> _
		Public cabinetname As FixedLengthString
		Dim drawernumber As Integer
		<VBFixedString(30),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=30)> _
		Public drawername As FixedLengthString
		Dim foldernumber As Integer
		<VBFixedString(70),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=70)> _
		Public foldername As FixedLengthString
		Dim documentnumber As Integer
		<VBFixedString(30),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=30)> _
		Public documentname As FixedLengthString
		Public Shared Function CreateInstance() As g_utParentDetails
			Dim result As New g_utParentDetails
			result.cabinetname = New FixedLengthString(30)
			result.drawername = New FixedLengthString(30)
			result.foldername = New FixedLengthString(70)
			result.documentname = New FixedLengthString(30)
			Return result
		End Function
	End Structure
	
	Public g_utChildInfo() As g_utChildData = Nothing
	Public g_utDrawerData() As g_utDrawerDataInfo = Nothing
	
	Function AddUserLogin(ByRef sUsername As String) As Integer
		
		Dim result As Integer = 0
		Dim dsLogin As DAO.Dynaset
		Dim iLoginID As Integer
		Dim sMsg As String = ""
		Dim iLicenceLimit, iTotalLogins As Integer
		
Start: 
		result = True
		
        Try

            iLoginID = GetNextLoginID(sUsername)

            If iLoginID > 0 Then
                ' Process for Super User and API's ONLY

                If sUsername.Trim() = "SU" Or sUsername.Trim() = "DMSAPI" Or sUsername.Trim() = "DMSWAPI" Then
                    ' Add user to login table
                    dsLogin = g_dbDDB.CreateDynaset("login")
                    dsLogin.LockEdits = False
                    dsLogin.AddNew()

                    dsLogin("login_num").Value = iLoginID
                    dsLogin("user_name").Value = sUsername
                    dsLogin("device_num").Value = 0
                    dsLogin("date").Value = DateTime.Today.AddDays(DateTimeHelper.Time.ToOADate())

                    g_iTmp = 0
                    g_iRC = UpdateDynaset(dsLogin)
                    Select Case g_iRC
                        Case PM_TRUE
                        Case PM_DUPLICATEKEY
                            GoTo Start
                        Case PM_FALSE
                            Interaction.MsgBox("Update Table failed. (ds)", MB_ICONEXCLAMATION, "AddUserLogin")
                            result = False
                        Case Else
                            Interaction.MsgBox("Update Table failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "AddUserLogin")
                            result = False
                    End Select

                    dsLogin.Close()
                    dsLogin = Nothing

                    g_iLoginID = iLoginID

                    Return result
                End If

                ' Check if havn't exceeded the licence limit
                iLicenceLimit = GetLicenceLimit()

                If iLicenceLimit = 0 Then
                    ' No licence key found. Need to add new licence
                    Interaction.MsgBox("Please contact Policy Master support for a licence number.", MB_ICONINFORMATION, "Licence Number")
                    Return False
                Else
                    If Not (CheckLicence()) Then
                        ' Licence key invalid. Oh dear, they been messing!
                        Interaction.MsgBox("Invalid licence key. " & Strings.Chr(10).ToString() & "Please contact Policy Master support.", MB_ICONINFORMATION, "Invalid Licence Key")
                        Return False
                    End If
                End If

                iTotalLogins = GetTotalLogins()

                If iTotalLogins >= iLicenceLimit Then
                    ' Exceeded licence limit

                    If GetUserLogins(sUsername) > 0 Then
                        ' Override existing login

                        sMsg = "You have exceeded the licence limit." & Strings.Chr(10).ToString() & "Do you wish to terminate a previous login?"
                        If Interaction.MsgBox(sMsg, MB_ICONQUESTION + MB_YESNO + MB_DEFBUTTON2, "Licence Limit") = System.Windows.Forms.DialogResult.Yes Then
                            ' Display available logins to replace
                            iLoginID = GetExistingLogin(sUsername)
                            If iLoginID = 0 Then
                                ' User pressed cancel
                                sMsg = "***  LICENCE LIMIT  ***" & Strings.Chr(10).ToString() & Strings.Chr(10).ToString() & "You have exceeded the licence limit." & Strings.Chr(10).ToString() & "Please contact your System Administrator"
                                Interaction.MsgBox(sMsg, MB_ICONEXCLAMATION, "Licence Limit")
                                Return False
                            Else
                                g_iLoginID = iLoginID
                            End If
                        Else
                            sMsg = "***  LICENCE LIMIT  ***" & Strings.Chr(10).ToString() & Strings.Chr(10).ToString() & "You have exceeded the licence limit." & Strings.Chr(10).ToString() & "Please contact your System Administrator"
                            Interaction.MsgBox(sMsg, MB_ICONEXCLAMATION, "Licence Limit")
                            Return False
                        End If
                    Else
                        sMsg = "***  LICENCE LIMIT  ***" & Strings.Chr(10).ToString() & Strings.Chr(10).ToString() & "You have exceeded the licence limit." & Strings.Chr(10).ToString() & "Please contact your System Administrator"
                        Interaction.MsgBox(sMsg, MB_ICONEXCLAMATION, "Licence Limit")
                        Return False
                    End If
                Else
                    ' Add user to login table
                    dsLogin = g_dbDDB.CreateDynaset("login")
                    dsLogin.LockEdits = False
                    dsLogin.AddNew()

                    dsLogin("login_num").Value = iLoginID
                    dsLogin("user_name").Value = sUsername
                    dsLogin("device_num").Value = 0
                    dsLogin("date").Value = DateTime.Today.AddDays(DateTimeHelper.Time.ToOADate())

                    g_iTmp = 0
                    g_iRC = UpdateDynaset(dsLogin)
                    Select Case g_iRC
                        Case PM_TRUE
                        Case PM_DUPLICATEKEY
                            GoTo Start
                        Case PM_FALSE
                            Interaction.MsgBox("Update Table failed. (ds)", MB_ICONEXCLAMATION, "AddUserLogin")
                            result = False
                        Case Else
                            Interaction.MsgBox("Update Table failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "AddUserLogin")
                            result = False
                    End Select

                    dsLogin.Close()
                    dsLogin = Nothing

                    g_iLoginID = iLoginID
                End If
            Else
                ' User limits
                sMsg = "You have exceeded your login limit. Do you wish to terminate a previous login?"
                If Interaction.MsgBox(sMsg, MB_ICONQUESTION + MB_YESNO + MB_DEFBUTTON2, "Login Limit Exceeded") = System.Windows.Forms.DialogResult.Yes Then
                    ' Display available logins to replace
                    iLoginID = GetExistingLogin(sUsername)
                    If iLoginID = 0 Then
                        ' User pressed cancel
                        result = False
                    Else
                        g_iLoginID = iLoginID
                    End If
                Else
                    result = False
                End If
            End If

            Return result

        Catch ex As Exception

            Return False
        End Try

    End Function
	
	Function AmIExternalData(ByRef iLevel As Integer, ByRef lNumber As Integer) As Integer
		Dim result As Integer = 0
		Dim ssData As DAO.Snapshot
		Dim lFold As Integer
		
		Dim sSQL As String = "SELECT ex_code FROM "
		Select Case iLevel
			Case CABINET
				sSQL = sSQL & "cabinet WHERE cabinet_num = " & CStr(lNumber)
			Case DRAWER
				sSQL = sSQL & "drawer WHERE drawer_num = " & CStr(lNumber)
			Case FOLDER
				sSQL = sSQL & "folder WHERE folder_num = " & CStr(lNumber)
			Case DOCUMENT
				'sSQL = "SELECT folder.ex_code FROM document, folder "
				'sSQL = sSQL & "WHERE document.folder_num = folder.folder_num "
				'sSQL = sSQL & "AND document.doc_num = " & lNumber&
				lFold = GetParentNumber(DOCUMENT, lNumber)
				sSQL = sSQL & "folder WHERE folder_num = " & CStr(lFold)
			Case Else
				sSQL = " "
		End Select
		
		Try 
			
			ssData = g_dbDDB.CreateSnapshot(sSQL)
			DAO_DBEngine_definst.FreeLocks()
			
            result = ssData(0).Value.Trim() <> ""
			
			ssData.Close()
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function CheckAnnotations(ByRef lDocumentNumber As Integer) As Integer
		
		Dim result As Integer = 0
		Dim ssSnapShot As DAO.Snapshot
		
		Try 
			
            ssSnapShot = g_dbDDB.CreateSnapshot("SELECT ann_text FROM annotation WHERE doc_num = " & lDocumentNumber)
			DAO_DBEngine_definst.FreeLocks()
			
			result = ssSnapShot.RecordCount
			
			ssSnapShot.Close()
			ssSnapShot = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
	
	Function CheckExternalFolderUnique(ByRef lDrawerNum As Integer, ByRef sExternalCode As String) As Integer
		'salvo(181096) - This section ensures folders are not created externally
		'               with duplicated codes
		
		Dim result As Integer = 0
		Dim ssData As DAO.Snapshot
		Dim sSQL As String = ""
		
		Try 
			result = True
			
			sSQL = "SELECT ex_code FROM folder WHERE drawer_num = " & lDrawerNum
			
			ssData = g_dbDDB.CreateSnapshot(sSQL)
			DAO_DBEngine_definst.FreeLocks()
			
			While Not ssData.EOF
                If ssData(0).Value.Trim().ToUpper() = sExternalCode.Trim().ToUpper() Then
                    result = False
                End If
				ssData.MoveNext()
			End While
			
			ssData.Close()
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function CheckKeywords(ByRef lDocumentNumber As Integer) As Integer
		
		Dim result As Integer = 0
		Dim ssSnapShot As DAO.Snapshot
		
		Try 
			
			ssSnapShot = g_dbDDB.CreateSnapshot("SELECT key_num FROM keyword WHERE doc_num = " & lDocumentNumber)
			DAO_DBEngine_definst.FreeLocks()
			
			result = ssSnapShot.RecordCount
			
			ssSnapShot.Close()
			ssSnapShot = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
	
	Function CheckLogin() As Integer
		
		Dim result As Integer = 0
		Dim ssLogin As DAO.Snapshot
		Dim sSQLQuery As String = ""
		
		result = True
		
		Try 
			
			sSQLQuery = "SELECT login FROM system"
			ssLogin = g_dbDDB.CreateSnapshot(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			
			If ssLogin.RecordCount = 1 Then
                result = (ssLogin("login").Value)
			End If
			
			ssLogin.Close()
			ssLogin = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function CheckLoginID() As Integer
		
		Dim result As Integer = 0
		Dim ssSnapShot As DAO.Snapshot
		
		result = True
		
		Try 
			
			ssSnapShot = g_dbDDB.CreateSnapshot("SELECT login_num FROM login WHERE login_num = " & g_iLoginID)
			DAO_DBEngine_definst.FreeLocks()
			
			If ssSnapShot.RecordCount = 0 Then
				result = False
			End If
			
			ssSnapShot.Close()
			DAO_DBEngine_definst.FreeLocks()
			ssSnapShot = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return True
		End Try
		
	End Function
	
	Function CheckPassword(ByRef iLevel As Integer, ByRef lSearchNumber As Integer, ByRef sPassword As String) As Integer
		Dim result As Integer = 0
		Dim sSQLQuery As String = ""
		Dim ssTmpSnapshot As DAO.Snapshot
		Dim ltmpSearchNumber As Integer
		Dim sPassCode As String = ""
		
		
		lSearchNumber = CInt(Math.Abs(lSearchNumber))
		
		' Get Password for DDB
		
		
		Try 
			
			Select Case iLevel
				Case CABINET
					sSQLQuery = "SELECT password FROM cabinet WHERE cabinet_num = " & lSearchNumber
				Case DRAWER
					sSQLQuery = "SELECT password FROM drawer WHERE drawer_num = " & lSearchNumber
				Case FOLDER
					sSQLQuery = "SELECT password FROM folder WHERE folder_num = " & lSearchNumber
				Case DOCUMENT
					sSQLQuery = "SELECT password FROM document WHERE doc_num = " & lSearchNumber
			End Select
			
			'encrypt incoming password
			sPassCode = Encryption(sPassword)
			
			If sPassCode.Trim() <> "ERROR" Then
				
				ssTmpSnapshot = g_dbDDB.CreateSnapshot(sSQLQuery)
				DAO_DBEngine_definst.FreeLocks()
				
                result = (sPassCode.Trim() = ssTmpSnapshot(0).Value.Trim())
				
				ssTmpSnapshot.Close()
				ssTmpSnapshot = Nothing
				
			Else
				result = False
			End If
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function CheckRecordLock(ByRef iLevel As Integer, ByRef lCode As Integer) As Integer
		
		Dim result As Integer = 0
		Dim ssSnapShot As DAO.Snapshot
		Dim sSQLQuery, sMsg, sLevel, sUser As String
		Dim dDate As Double
		
		result = False
		
		Select Case (iLevel)
			Case CABINET
				sLevel = "Cabinet"
			Case DRAWER
				sLevel = "Drawer"
			Case FOLDER
				sLevel = "Folder"
			Case DOCUMENT
				sLevel = "Document"
			Case DOCINFO
				sLevel = "Document Information"
		End Select
		
		Try 
			
			sSQLQuery = "SELECT * FROM recordlock WHERE level = " & iLevel & " AND "
			sSQLQuery = sSQLQuery & "code = " & CStr(lCode)
			
			ssSnapShot = g_dbDDB.CreateSnapshot(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			
			If ssSnapShot.RecordCount > 0 Then
                sUser = ssSnapShot("user").Value
                dDate = ssSnapShot("date").Value
				ssSnapShot.Close()
				ssSnapShot = Nothing
				
				Application.DoEvents()
				Application.DoEvents()
				
				result = True
				sMsg = "Unable to view " & sLevel & "." & Strings.Chr(10).ToString() & "Currently "
				sMsg = sMsg & "locked by user, " & sUser.Trim() & " on "
				sMsg = sMsg & DateTime.FromOADate(dDate).ToString("dddddd") & " at "
				sMsg = sMsg & StringsHelper.Format(dDate, "h:m:s am/pm") & Strings.Chr(10).ToString()
				sMsg = sMsg & "Please try later."
				'        MsgBox sMsg, MB_ICONSTOP, "DocuMaster Data Lock"
			Else
				ssSnapShot.Close()
				ssSnapShot = Nothing
			End If
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function CheckUserPassword(ByRef sUsername As String, ByRef sUserPassword As String) As Integer
		
		Dim result As Integer = 0
		Dim sSQLQuery As String = ""
		Dim dsUser As DAO.Dynaset
		Dim frmTmpPassword As frmPassword
		Dim sTmpUserName As New FixedLengthString(30)
		Dim sTmpUserPassword As New FixedLengthString(12)
		Dim iPointer As Integer
		
		' Check Password for DDB
		
		Try 
			result = PM_FALSE
			
			frmTmpPassword = New frmPassword()
			
            'iPointer = Cursor.Current
			
			sTmpUserName.Value = sUsername
			sTmpUserPassword.Value = sUserPassword
			
			sSQLQuery = "SELECT password FROM user WHERE user_name = '" & sUsername & "'"
			
			dsUser = g_dbDDB.CreateDynaset(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			dsUser.LockEdits = False
			
			If dsUser.RecordCount > 0 Then
				

				If Convert.IsDBNull(dsUser("password")) Or IsNothing(dsUser("password")) Then
					
					If Interaction.MsgBox(sUsername.Trim() & " does not have a password" & Strings.Chr(10).ToString() & "Do you want to set your password now?", MB_ICONQUESTION + MB_YESNO) = System.Windows.Forms.DialogResult.Yes Then
						
						Cursor.Current = Cursors.Default
						frmTmpPassword.ShowDialog()
						
						If frmTmpPassword.txtPassword.Text = "" Then
							result = PM_FALSE
						Else
							
							dsUser.Edit()
                            dsUser("password").Value = Encryption(frmTmpPassword.txtPassword.Text)
							dsUser.Update()
							result = PM_TRUE
						End If
					Else
						result = PM_CANCEL
					End If
					

                    'Cursor.Current = iPointer
					frmTmpPassword.Close()
					
				ElseIf Strings.Len(dsUser("password")) < 1 Then  'IsNull(dsUser("password")) Or Trim$(dsUser("password")) = "") Then
					
					If Interaction.MsgBox(sUsername.Trim() & " does not have a password" & Strings.Chr(10).ToString() & "Do you want to set your password now?", MB_ICONQUESTION + MB_YESNO) = System.Windows.Forms.DialogResult.Yes Then
						
						Cursor.Current = Cursors.Default
						frmTmpPassword.ShowDialog()
						
						If frmTmpPassword.txtPassword.Text = "" Then
							result = PM_FALSE
						Else
							
							dsUser.Edit()
                            dsUser("password").Value = Encryption(frmTmpPassword.txtPassword.Text)
							dsUser.Update()
							result = PM_TRUE
						End If
					Else
						result = PM_CANCEL
					End If
					

                    'Cursor.Current = iPointer
					frmTmpPassword.Close()
					
				Else
                    If Encryption(sUserPassword.Trim()) = dsUser("password").Value.Trim() Then
                        result = PM_TRUE
                    End If
				End If
			End If
			
			dsUser.Close()
			Return result
		
		Catch 
			
			
			
			Return PM_FALSE
		End Try
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: CloseDDB
	'      Author: simonb
	'        Date: 11/04/95
	'
	' Description: Closes the Document DataBase.
	'
	'*********************************************************************
	'
	Sub CloseDDB()
		
		Try 
			
			'close global dynasets and snapshots
			g_ssSnapshot.Close()
			g_ssSnapshot = Nothing
			
			g_ssDeviceTypes.Close()
			g_ssDeviceTypes = Nothing
			
			' Close Database
			Artinsoft.VB6.DB.TransactionManager.DeEnlist(g_dbDDB)
			g_dbDDB.Close()
		
		Catch 
		End Try
		
		
		Exit Sub
	End Sub
	
    Function dbVersion(ByRef db As DAO.Database) As Integer

        Dim result As Integer = 0
        Dim ssVer As DAO.Snapshot

        Try

            ssVer = db.CreateSnapshot("SELECT DBVersion FROM System")
            DAO_DBEngine_definst.FreeLocks()

            result = ssVer("DBVersion").Value

            ssVer.Close()
            ssVer = Nothing

            Return result

        Catch



            Return 0
        End Try

    End Function
	
	Function DDBChanged(ByRef iLevel As Integer, ByRef lLevelNumber As Integer, ByRef sLevelName As String) As Integer
		
		Dim result As Integer = 0
		Dim sSQLQuery As String = ""
		Dim dsTmpChanged As DAO.Dynaset
		Dim lLockNum As Integer
		Dim iLockCount As Integer
		
		result = False
		
		Try 
			lLockNum = 0
			
			Select Case iLevel
				Case CABINET
					sSQLQuery = "SELECT cabinet_name FROM cabinet WHERE "
					sSQLQuery = sSQLQuery & "cabinet_num = " & CStr(lLevelNumber)
				Case DRAWER
					sSQLQuery = "SELECT drawer_name FROM drawer WHERE "
					sSQLQuery = sSQLQuery & "drawer_num = " & CStr(lLevelNumber)
				Case FOLDER
					sSQLQuery = "SELECT folder_name FROM folder WHERE "
					sSQLQuery = sSQLQuery & "folder_num = " & CStr(lLevelNumber)
				Case DOCUMENT
					sSQLQuery = "SELECT doc_name FROM document WHERE "
					sSQLQuery = sSQLQuery & "document_num = " & CStr(lLevelNumber)
			End Select
			
			dsTmpChanged = g_dbDDB.CreateDynaset(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			dsTmpChanged.LockEdits = False
			
			If dsTmpChanged.RecordCount > 0 Then
				
				
                If dsTmpChanged(0).Value.Trim() <> sLevelName.Trim() Then
                    result = True

                    ' Descriptions are different, must update record
                    dsTmpChanged.Edit()

                    dsTmpChanged(0).Value = sLevelName

                    g_iTmp = 0
                    g_iRC = UpdateDynaset(dsTmpChanged)
                    Select Case g_iRC
                        Case PM_TRUE
                            result = True
                        Case PM_FALSE
                            Interaction.MsgBox("Change Failed. (ds)", MB_ICONEXCLAMATION, "DDBChanged")
                            result = False
                        Case PM_CANCEL
                            Interaction.MsgBox("Change Canceled. (ds)", MB_ICONEXCLAMATION, "DDBChanged")
                            result = False
                        Case Else
                            Interaction.MsgBox("Change Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DDBChanged")
                            result = False
                    End Select

                End If
			End If
			
			dsTmpChanged.Close()
			dsTmpChanged = Nothing
			
			Return result
		
		Catch 
			
			
			
			'    If lLockNum& > 0 Then
			'        If RemoveRecordLock(lLockNum&) Then
			'        End If
			'    End If
			
			'Debug.Print "Failed to rename level description"
			Return result
		End Try
		
	End Function
	
	Function DDBExists(ByRef iLevel As Integer, ByRef sCode As String) As Integer
		Dim result As Integer = 0
		Dim sSQLQuery As String = ""
		Dim ssTmpExists As DAO.Snapshot
		
		Select Case iLevel
			Case CABINET
				sSQLQuery = "SELECT cabinet_num FROM cabinet WHERE "
				sSQLQuery = sSQLQuery & "ex_code = '" & sCode.Trim() & "'"
			Case DRAWER
				sSQLQuery = "SELECT drawer_num FROM drawer WHERE "
				sSQLQuery = sSQLQuery & "ex_code = '" & sCode.Trim() & "'"
			Case FOLDER
				sSQLQuery = "SELECT folder_num FROM folder WHERE "
				sSQLQuery = sSQLQuery & "ex_code = '" & sCode.Trim() & "'"
			Case DOCUMENT
				sSQLQuery = "SELECT doc_num FROM document WHERE "
				sSQLQuery = sSQLQuery & "ex_code = '" & sCode.Trim() & "'"
		End Select
		
		Try 
			
			ssTmpExists = g_dbDDB.CreateSnapshot(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			
            result = ssTmpExists(0).Value
			
			ssTmpExists.Close()
			Return result
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: DeleteAnnotations
	'      Author: simonb
	'        Date: 06/06/95
	'
	' Description: Deletes the record level structure from the DDB.
	'
	'*********************************************************************
	'
	Function DeleteAnnotations(ByRef lDocNumber As Integer, ByRef sAnnotation As String) As Integer
		
		Dim result As Integer = 0
		Dim dsTmpAnn As DAO.Dynaset
		Dim sSQLQuery As String = ""
		
		Try 
			
			If sAnnotation = "" Then
				' Select all document numbers
				sSQLQuery = "SELECT * FROM annotation WHERE doc_num = " & lDocNumber
			Else
				' Select single annotation
				sSQLQuery = "SELECT * FROM annotation WHERE doc_num = " & lDocNumber & " AND "
				sSQLQuery = sSQLQuery & "ann_text = '" & sAnnotation & "'"
			End If
			
			dsTmpAnn = g_dbDDB.CreateDynaset(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			dsTmpAnn.LockEdits = False
			
			If dsTmpAnn.RecordCount = 0 Then
				dsTmpAnn.Close()
				dsTmpAnn = Nothing
				Return True
			End If
			
			While Not dsTmpAnn.EOF
				g_iTmp = 0
				g_iRC = DeleteDynaset(dsTmpAnn)
				Select Case g_iRC
					Case PM_TRUE
						result = True
					Case PM_FALSE, PM_CANCEL
						Interaction.MsgBox("Delete failed. (ds)", MB_ICONEXCLAMATION, "DeleteAnnotations")
						result = False
					Case Else
						Interaction.MsgBox("Delete failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteAnnotations")
						result = False
				End Select
				
				dsTmpAnn.MoveNext()
				
			End While
			
			dsTmpAnn.Close()
			dsTmpAnn = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	'
	'
	'*********************************************************************
	'
	' Module Name: DeleteDDB
	'      Author: simonb
	'        Date: 12/04/95
	'
	' Description: Deletes the record level structure from the DDB.
	'
	'*********************************************************************
	'
	Function DeleteDDB(ByRef iLevel As Integer, ByRef lLevelNumber As Integer) As Integer
		Dim result As Integer = 0
		Dim dsTmpDynaset As DAO.Dynaset
		Dim sSQLQuery As String = ""
		Dim iDelete, iLinks As Integer
		Dim lDocNum, lLockNum As Integer
		Dim iLockCount As Integer
		
		
		Try 
			result = True
			lLockNum = 0
			
			' If this item has others liked to it, it can not be deleted, just sever it upwared link...
			iLinks = LinkOwner(iLevel, lLevelNumber)
			iDelete = Not (iLinks > 0)
			
			Select Case iLevel
				Case CABINET
					If DeleteDrawers(lLevelNumber) Then
						' Delete Cabinet
						sSQLQuery = "SELECT * FROM cabinet WHERE cabinet_num = " & lLevelNumber & " AND "
						sSQLQuery = sSQLQuery & "access_level >= " & CStr(g_iUserAccessLevel)
						
						dsTmpDynaset = g_dbDDB.CreateDynaset(sSQLQuery)
						DAO_DBEngine_definst.FreeLocks()
						dsTmpDynaset.LockEdits = False
						
						
                        If dsTmpDynaset("ex_code").Value.Trim() <> "" Then
                            If DelHistoryData(CABINET, dsTmpDynaset("cabinet_num").Value) Then
                                g_iTmp = 0
                                g_iRC = DeleteDynaset(dsTmpDynaset)
                                Select Case g_iRC
                                    Case PM_TRUE
                                        result = True
                                    Case PM_FALSE, PM_CANCEL
                                        Interaction.MsgBox("Delete failed. (ds)", MB_ICONEXCLAMATION, "DeleteDDB")
                                        result = False
                                    Case Else
                                        Interaction.MsgBox("Delete failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteDDB")
                                        result = False
                                End Select

                            Else
                                result = False
                                'MsgBox "Failed to delete Cabinet from external History", MB_ICONEXCLAMATION
                            End If
                        Else
                            g_iTmp = 0
                            g_iRC = DeleteDynaset(dsTmpDynaset)
                            Select Case g_iRC
                                Case PM_TRUE
                                    result = True
                                Case PM_FALSE, PM_CANCEL
                                    Interaction.MsgBox("Delete failed. (ds)", MB_ICONEXCLAMATION, "DeleteDDB")
                                    result = False
                                Case Else
                                    Interaction.MsgBox("Delete failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteDDB")
                                    result = False
                            End Select

                        End If
						
						'Else
						'    if dsTmpDynaset("link_count") <= 1 then
						'        dsTmpDynaset.delete
						'    else
						'       dsTmpDynaset.Edit
						'        dsTmpDynaset("link_count") = dsTmpDynaset("link_count") - 1
						'        dsTmpDynaset.update
						'    end if
						'end if
					Else
						result = False
					End If
					
				Case DRAWER
					sSQLQuery = "SELECT * FROM drawer WHERE drawer_num = " & lLevelNumber & " AND "
					sSQLQuery = sSQLQuery & "access_level >= " & CStr(g_iUserAccessLevel)
					
					dsTmpDynaset = g_dbDDB.CreateDynaset(sSQLQuery)
					DAO_DBEngine_definst.FreeLocks()
					dsTmpDynaset.LockEdits = False
					
					If Not iDelete Then
                        If dsTmpDynaset("ex_code").Value.Trim() <> "" Then
                            If DelHistoryData(DRAWER, dsTmpDynaset("drawer_num").Value) Then
                                dsTmpDynaset.Edit()
                                dsTmpDynaset("cabinet_num").Value = 0

                                g_iTmp = 0
                                g_iRC = UpdateDynaset(dsTmpDynaset)
                                Select Case g_iRC
                                    Case PM_TRUE
                                        result = True
                                    Case PM_FALSE
                                        Interaction.MsgBox("Delete Failed. (ds)", MB_ICONEXCLAMATION, "DeleteDDB")
                                        result = False
                                    Case PM_CANCEL
                                        Interaction.MsgBox("Delete Canceled. (ds)", MB_ICONEXCLAMATION, "DelteDDB")
                                        result = False
                                    Case Else
                                        Interaction.MsgBox("Delete Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteDDB")
                                        result = False
                                End Select

                            Else
                                result = False
                            End If
                        Else
                            dsTmpDynaset.Edit()
                            dsTmpDynaset("cabinet_num").Value = 0

                            g_iTmp = 0
                            g_iRC = UpdateDynaset(dsTmpDynaset)
                            Select Case g_iRC
                                Case PM_TRUE
                                    result = True
                                Case PM_FALSE
                                    Interaction.MsgBox("Delete Failed. (ds)", MB_ICONEXCLAMATION, "DeleteDDB")
                                    result = False
                                Case PM_CANCEL
                                    Interaction.MsgBox("Delete Canceled. (ds)", MB_ICONEXCLAMATION, "DelteDDB")
                                    result = False
                                Case Else
                                    Interaction.MsgBox("Delete Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteDDB")
                                    result = False
                            End Select

                        End If
					Else
						If DeleteFolders(lLevelNumber) Then
                            If dsTmpDynaset("ex_code").Value.Trim() <> "" Then
                                If DelHistoryData(DRAWER, dsTmpDynaset("drawer_num").Value) Then
                                    g_iTmp = 0
                                    g_iRC = DeleteDynaset(dsTmpDynaset)
                                    Select Case g_iRC
                                        Case PM_TRUE
                                            result = True
                                        Case PM_FALSE
                                            Interaction.MsgBox("Delete Failed. (ds)", MB_ICONEXCLAMATION, "DeleteDDB")
                                            result = False
                                        Case PM_CANCEL
                                            Interaction.MsgBox("Delete Canceled. (ds)", MB_ICONEXCLAMATION, "DelteDDB")
                                            result = False
                                        Case Else
                                            Interaction.MsgBox("Delete Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteDDB")
                                            result = False
                                    End Select

                                Else
                                    result = False
                                    'MsgBox "Failed to delete Drawer from external History", MB_ICONEXCLAMATION
                                End If
                            Else
                                g_iTmp = 0
                                g_iRC = DeleteDynaset(dsTmpDynaset)
                                Select Case g_iRC
                                    Case PM_TRUE
                                        result = True
                                    Case PM_FALSE
                                        Interaction.MsgBox("Delete Failed. (ds)", MB_ICONEXCLAMATION, "DeleteDDB")
                                        result = False
                                    Case PM_CANCEL
                                        Interaction.MsgBox("Delete Canceled. (ds)", MB_ICONEXCLAMATION, "DelteDDB")
                                        result = False
                                    Case Else
                                        Interaction.MsgBox("Delete Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteDDB")
                                        result = False
                                End Select

                            End If
						Else
							result = False
						End If
					End If
					
				Case FOLDER
					sSQLQuery = "SELECT * FROM folder WHERE folder_num = " & lLevelNumber & " AND "
					sSQLQuery = sSQLQuery & "access_level >= " & CStr(g_iUserAccessLevel)
					
					dsTmpDynaset = g_dbDDB.CreateDynaset(sSQLQuery)
					DAO_DBEngine_definst.FreeLocks()
					dsTmpDynaset.LockEdits = False
					
					If Not iDelete Then
                        If dsTmpDynaset("ex_code").Value.Trim() <> "" Then
                            If DelHistoryData(FOLDER, dsTmpDynaset("folder_num").Value) Then
                                dsTmpDynaset.Edit()
                                dsTmpDynaset("drawer_num").Value = 0

                                g_iTmp = 0
                                g_iRC = UpdateDynaset(dsTmpDynaset)
                                Select Case g_iRC
                                    Case PM_TRUE
                                        result = True
                                    Case PM_FALSE
                                        Interaction.MsgBox("Delete Failed. (ds)", MB_ICONEXCLAMATION, "DeleteDDB")
                                        result = False
                                    Case PM_CANCEL
                                        Interaction.MsgBox("Delete Canceled. (ds)", MB_ICONEXCLAMATION, "DeleteDDB")
                                        result = False
                                    Case Else
                                        Interaction.MsgBox("Delete Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteDDB")
                                        result = False
                                End Select

                            Else
                                result = False
                            End If
                        Else
                            dsTmpDynaset.Edit()
                            dsTmpDynaset("drawer_num").Value = 0

                            g_iTmp = 0
                            g_iRC = UpdateDynaset(dsTmpDynaset)
                            Select Case g_iRC
                                Case PM_TRUE
                                    result = True
                                Case PM_FALSE
                                    Interaction.MsgBox("Delete Failed. (ds)", MB_ICONEXCLAMATION, "DeleteDDB")
                                    result = False
                                Case PM_CANCEL
                                    Interaction.MsgBox("Delete Canceled. (ds)", MB_ICONEXCLAMATION, "DeleteDDB")
                                    result = False
                                Case Else
                                    Interaction.MsgBox("Delete Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteDDB")
                                    result = False
                            End Select

                        End If
						
					Else
						If DeleteDocuments(lLevelNumber) Then
                            If dsTmpDynaset("ex_code").Value.Trim() <> "" Then
                                If DelHistoryData(FOLDER, dsTmpDynaset("folder_num").Value) Then
                                    g_iTmp = 0
                                    g_iRC = DeleteDynaset(dsTmpDynaset)
                                    Select Case g_iRC
                                        Case PM_TRUE
                                            result = True
                                        Case PM_FALSE
                                            Interaction.MsgBox("Delete Failed. (ds)", MB_ICONEXCLAMATION, "DeleteDDB")
                                            result = False
                                        Case PM_CANCEL
                                            Interaction.MsgBox("Delete Canceled. (ds)", MB_ICONEXCLAMATION, "DeleteDDB")
                                            result = False
                                        Case Else
                                            Interaction.MsgBox("Delete Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteDDB")
                                            result = False
                                    End Select

                                Else
                                    result = False
                                    'MsgBox "Failed to delete Folder from external History", MB_ICONEXCLAMATION
                                End If
                            Else
                                g_iTmp = 0
                                g_iRC = DeleteDynaset(dsTmpDynaset)
                                Select Case g_iRC
                                    Case PM_TRUE
                                        result = True
                                    Case PM_FALSE
                                        Interaction.MsgBox("Delete Failed. (ds)", MB_ICONEXCLAMATION, "DeleteDDB")
                                        result = False
                                    Case PM_CANCEL
                                        Interaction.MsgBox("Delete Canceled. (ds)", MB_ICONEXCLAMATION, "DeleteDDB")
                                        result = False
                                    Case Else
                                        Interaction.MsgBox("Delete Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteDDB")
                                        result = False
                                End Select

                            End If
						Else
							result = False
						End If
					End If
					
				Case DOCUMENT
					
					result = DeleteDoc(lLevelNumber)
					
			End Select
			
			If iLevel <> DOCUMENT Then
				dsTmpDynaset.Close()
				dsTmpDynaset = Nothing
			End If
			
			Return result
		
		Catch 
			
			
			
			Return False
			
			
			
			dsTmpDynaset.Close()
			Interaction.MsgBox("Failed to delete from external History", MB_ICONEXCLAMATION)
			Return True
		End Try
		
	End Function
	
	Function DeleteDoc(ByRef lDocNum As Integer) As Integer
		Dim result As Integer = 0
		Dim dsDoc As DAO.Dynaset
		Dim iDeleteOK As Integer
		Dim lLockNum As Integer
		
		result = True
		Try 
			iDeleteOK = True
			
			dsDoc = g_dbDDB.CreateDynaset("SELECT * FROM document WHERE doc_num = " & lDocNum.ToString())
			DAO_DBEngine_definst.FreeLocks()
			
			If dsDoc.RecordCount <> 1 Then
				result = False
				dsDoc.Close()
				dsDoc = Nothing
			Else
				
                If dsDoc("access_level").Value >= g_iUserAccessLevel Then

                    DAO_DBEngine_definst.BeginTrans()

                    ' delete history first
                    If Not DelHistoryData(DOCUMENT, dsDoc("doc_num").Value) Then
                        'if Del history fails its probably not external data...
                    End If

                    'if this is a linked item ...
                    If LinkOwner(DOCUMENT, dsDoc("doc_num").Value) > 0 Then
                        'dont delete it ...
                        dsDoc.Edit()
                        dsDoc("folder_num").Value = 0

                        g_iTmp = 0
                        g_iRC = UpdateDynaset(dsDoc)
                        Select Case g_iRC
                            Case PM_TRUE
                                result = True
                            Case PM_FALSE
                                Interaction.MsgBox("Delete Failed. (ds)", MB_ICONEXCLAMATION, "DeleteDoc")
                                result = False
                            Case PM_CANCEL
                                Interaction.MsgBox("Delete Canceled. (ds)", MB_ICONEXCLAMATION, "DelteDoc")
                                result = False
                            Case Else
                                Interaction.MsgBox("Delete Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteDoc")
                                result = False
                        End Select

                        g_iTmp = 0
                        g_iRC = CommitDatabase()
                        Select Case g_iRC
                            Case PM_TRUE
                                result = True
                            Case PM_FALSE
                                DAO_DBEngine_definst.Rollback()
                                Interaction.MsgBox("Delete Failed. (cdb)", MB_ICONEXCLAMATION, "DeleteDoc")
                                result = False
                            Case PM_CANCEL
                                DAO_DBEngine_definst.Rollback()
                                Interaction.MsgBox("Delete Canceled. (cdb)", MB_ICONEXCLAMATION, "DelteDoc")
                                result = False
                            Case Else
                                DAO_DBEngine_definst.Rollback()
                                Interaction.MsgBox("Delete Failed. (cdb) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteDoc")
                                result = False
                        End Select

                    Else
                        ' if its not a link
                        'if all deletes work...
                        If DeletePages(lDocNum) Then
                            If DeleteDocInfo(lDocNum) Then
                                If DeleteAnnotations(lDocNum, "") Then

                                    g_iTmp = 0
                                    g_iRC = DeleteDynaset(dsDoc)
                                    Select Case g_iRC
                                        Case PM_TRUE
                                            result = True
                                        Case PM_FALSE
                                            Interaction.MsgBox("Delete Failed. (ds)", MB_ICONEXCLAMATION, "DeleteDoc")
                                            result = False
                                        Case PM_CANCEL
                                            Interaction.MsgBox("Delete Canceled. (ds)", MB_ICONEXCLAMATION, "DelteDoc")
                                            result = False
                                        Case Else
                                            Interaction.MsgBox("Delete Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteDoc")
                                            result = False
                                    End Select

                                    g_iTmp = 0
                                    g_iRC = CommitDatabase()
                                    Select Case g_iRC
                                        Case PM_TRUE
                                            result = True
                                        Case PM_FALSE
                                            DAO_DBEngine_definst.Rollback()
                                            Interaction.MsgBox("Delete Failed. (cdb)", MB_ICONEXCLAMATION, "DeleteDoc")
                                            result = False
                                        Case PM_CANCEL
                                            DAO_DBEngine_definst.Rollback()
                                            Interaction.MsgBox("Delete Canceled. (cdb)", MB_ICONEXCLAMATION, "DelteDoc")
                                            result = False
                                        Case Else
                                            DAO_DBEngine_definst.Rollback()
                                            Interaction.MsgBox("Delete Failed. (cdb) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteDoc")
                                            result = False
                                    End Select

                                Else
                                    DAO_DBEngine_definst.Rollback()
                                    result = False
                                End If
                            Else
                                DAO_DBEngine_definst.Rollback()
                                result = False
                            End If
                        Else
                            DAO_DBEngine_definst.Rollback()
                            result = False
                        End If

                    End If

                Else
                    ' User does not have access to delete this doc...
                    result = False
                End If
			End If
			
			dsDoc.Close()
			dsDoc = Nothing
			
			Return result
		
		Catch 
			
			
			
			result = False
			dsDoc = Nothing
			
			Return result
		End Try
		
	End Function
	
	Function DeleteDocInfo(ByRef lDocumentNumber As Integer) As Integer
		Dim result As Integer = 0
		Dim dsTmpDocInfo As DAO.Dynaset
		Dim sSQLQuery As String = ""
		Dim iTmpNum As Integer
		Dim iDeleteOK As Integer
		
		Try 
			
			' Select all document numbers
			sSQLQuery = "SELECT * FROM docinfo WHERE doc_num = " & lDocumentNumber
			
			dsTmpDocInfo = g_dbDDB.CreateDynaset(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			dsTmpDocInfo.LockEdits = False
			
			If dsTmpDocInfo.RecordCount = 0 Then
				dsTmpDocInfo.Close()
				dsTmpDocInfo = Nothing
				Return True
			End If
			
			iDeleteOK = True
			
			While Not dsTmpDocInfo.EOF
				' Delete Annotations
                iTmpNum = dsTmpDocInfo("doc_num").Value
				If Not (DeleteAnnotations(iTmpNum, "")) Then
					iDeleteOK = False
				End If
				' Delete F.T.A.O's
                iTmpNum = dsTmpDocInfo("doc_num").Value
				If Not (DeleteFTAO(iTmpNum)) Then
					iDeleteOK = False
				End If
				' Delete KeyWords
                iTmpNum = dsTmpDocInfo("doc_num").Value
				If Not (DeleteKeyWord(iTmpNum)) Then
					iDeleteOK = False
				End If
				
				If iDeleteOK Then
					g_iTmp = 0
					g_iRC = DeleteDynaset(dsTmpDocInfo)
					Select Case g_iRC
						Case PM_TRUE
							result = True
						Case PM_FALSE, PM_CANCEL
							Interaction.MsgBox("Delete failed. (ds)", MB_ICONEXCLAMATION, "DeleteDocInfo")
							result = False
						Case Else
							Interaction.MsgBox("Delete failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteDocInfo")
							result = False
					End Select
					
				End If
				
				dsTmpDocInfo.MoveNext()
			End While
			
			dsTmpDocInfo.Close()
			dsTmpDocInfo = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: DeleteDocuments
	'      Author: simonb
	'        Date: 12/04/95
	'
	' Description: Deletes the record level structure from the DDB.
	'
	'*********************************************************************
	'
	Function DeleteDocuments(ByRef lFolderNumber As Integer) As Integer
		Dim ErrDelDocs As Boolean = False
		Dim ErrDeleteDocuments As Boolean = False
		Dim dsTmpDocuments As DAO.Dynaset
		Dim sSQLQuery As String = ""
		Dim iDeleteOK As Integer
		Dim iTmpDocNum As Integer
		Dim iRC As Integer
		Dim lLockNum As Integer
		Dim iLockCount As Integer
		
		Try 
			ErrDeleteDocuments = True
			ErrDelDocs = False
			lLockNum = 0
			
			' Select all document numbers
			sSQLQuery = "SELECT * FROM document WHERE folder_num = " & lFolderNumber
			
			dsTmpDocuments = g_dbDDB.CreateDynaset(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			dsTmpDocuments.LockEdits = False
			
			iDeleteOK = True
			iRC = True
			
			If dsTmpDocuments.RecordCount = 0 Then
				dsTmpDocuments.Close()
				dsTmpDocuments = Nothing
				Return iDeleteOK
			End If
			
			ErrDelDocs = True
			ErrDeleteDocuments = False
			While Not dsTmpDocuments.EOF
				
                If Not DeleteDoc(dsTmpDocuments("doc_num").Value) Then
                    iDeleteOK = False
                End If
				
				dsTmpDocuments.MoveNext()
				
			End While
			
			dsTmpDocuments.Close()
			dsTmpDocuments = Nothing
			
			Return iDeleteOK
		
		Catch excep As System.Exception
			If Not ErrDelDocs And Not ErrDeleteDocuments Then
				Throw excep
			End If
			
			If ErrDeleteDocuments Then
				
				
				Return False
				
			End If
			If ErrDelDocs Or ErrDeleteDocuments Then
				
				
				dsTmpDocuments.Close()
				dsTmpDocuments = Nothing
				Return False
				
			End If
		End Try
	End Function
	
	'*********************************************************************
	'
	' Module Name: DeleteDrawers
	'      Author: simonb
	'        Date: 12/04/95
	'
	' Description: Deletes the record level structure from the DDB.
	'
	'*********************************************************************
	'
	Function DeleteDrawers(ByRef lCabinetNumber As Integer) As Integer
		Dim result As Integer = 0
		Dim dsTmpDrawers As DAO.Dynaset
		Dim sSQLQuery As String = ""
		Dim iDeleteOK As Integer
		Dim iTmpDocNum As Integer
		Dim iRC As Integer
		Dim lLockNum As Integer
		Dim iLockCount As Integer
		
		Try 
			lLockNum = 0
			
			' Select all drawer numbers
			sSQLQuery = "SELECT * FROM drawer WHERE cabinet_num = " & lCabinetNumber
			
			dsTmpDrawers = g_dbDDB.CreateDynaset(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			dsTmpDrawers.LockEdits = False
			
			iDeleteOK = True
			iRC = True
			
			If dsTmpDrawers.RecordCount = 0 Then
				dsTmpDrawers.Close()
				dsTmpDrawers = Nothing
				Return iDeleteOK
			End If
			
			While Not dsTmpDrawers.EOF
				'  Check to see if we can delete it first...
                If dsTmpDrawers("access_level").Value >= g_iUserAccessLevel Then

                    iDeleteOK = True


                    ' Delete Folders
                    iTmpDocNum = dsTmpDrawers("drawer_num").Value

                    DAO_DBEngine_definst.BeginTrans()

                    ' if drawer is external ...
                    If dsTmpDrawers("ex_code").Value.Trim() <> "" Then
                        If Not DelHistoryData(DRAWER, dsTmpDrawers("drawer_num").Value) Then
                            iDeleteOK = False
                            iRC = False
                        End If
                    End If

                    If iDeleteOK Then
                        ' if this is not a linked folder ...
                        If LinkOwner(DRAWER, dsTmpDrawers("drawer_num").Value) = 0 Then
                            If Not (DeleteFolders(iTmpDocNum)) Then
                                iDeleteOK = False
                                iRC = False
                                DAO_DBEngine_definst.Rollback()
                            Else

                                g_iTmp = 0
                                g_iRC = DeleteDynaset(dsTmpDrawers)
                                Select Case g_iRC
                                    Case PM_TRUE
                                        result = True
                                    Case PM_FALSE, PM_CANCEL
                                        Interaction.MsgBox("Delete failed. (ds)", MB_ICONEXCLAMATION, "DeleteDrawers")
                                        iRC = False
                                    Case Else
                                        Interaction.MsgBox("Delete failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteDrawers")
                                        iRC = False
                                End Select

                                g_iTmp = 0
                                g_iRC = CommitDatabase()
                                Select Case g_iRC
                                    Case PM_TRUE
                                        result = True
                                    Case PM_FALSE, PM_CANCEL
                                        DAO_DBEngine_definst.Rollback()
                                        Interaction.MsgBox("Update failed. (cdb)", MB_ICONEXCLAMATION, "DeleteDrawers")
                                        iRC = False
                                    Case Else
                                        DAO_DBEngine_definst.Rollback()
                                        Interaction.MsgBox("Update failed. (cdb) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteDrawers")
                                        iRC = False
                                End Select

                            End If
                        Else
                            'if this is a linked folder, dont delete it ...
                            dsTmpDrawers.Edit()
                            dsTmpDrawers("drawer_num").Value = 0

                            g_iTmp = 0
                            g_iRC = UpdateDynaset(dsTmpDrawers)
                            Select Case g_iRC
                                Case PM_TRUE
                                    result = True
                                Case PM_FALSE
                                    Interaction.MsgBox("Delete Failed. (ds)", MB_ICONEXCLAMATION, "DeleteDrawers")
                                    iRC = False
                                Case PM_CANCEL
                                    Interaction.MsgBox("Delete Canceled. (ds)", MB_ICONEXCLAMATION, "DeleteDrawers")
                                    iRC = False
                                Case Else
                                    Interaction.MsgBox("Delete Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteDrawers")
                                    iRC = False
                            End Select

                            g_iTmp = 0
                            g_iRC = CommitDatabase()
                            Select Case g_iRC
                                Case PM_TRUE
                                    result = True
                                Case PM_FALSE
                                    DAO_DBEngine_definst.Rollback()
                                    Interaction.MsgBox("Delete Failed. (cdb)", MB_ICONEXCLAMATION, "DeleteDrawers")
                                    iRC = False
                                Case PM_CANCEL
                                    DAO_DBEngine_definst.Rollback()
                                    Interaction.MsgBox("Delete Canceled. (cdb)", MB_ICONEXCLAMATION, "DeleteDrawers")
                                    iRC = False
                                Case Else
                                    DAO_DBEngine_definst.Rollback()
                                    Interaction.MsgBox("Delete Failed. (cdb) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteDrawers")
                                    iRC = False
                            End Select

                        End If
                    Else
                        iRC = False
                        DAO_DBEngine_definst.Rollback()
                    End If

                Else
                    iRC = False
                End If
				
				dsTmpDrawers.MoveNext()
			End While
			
			
			dsTmpDrawers.Close()
			dsTmpDrawers = Nothing
			
			Return iRC
		
		Catch 
			
			
			
			Return False
		End Try
		
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: DeleteFolders
	'      Author: simonb
	'        Date: 12/04/95
	'
	' Description: Deletes the record level structure from the DDB.
	'
	'*********************************************************************
	'
	Function DeleteFolders(ByRef lDrawerNumber As Integer) As Integer
		Dim result As Integer = 0
		Dim dsTmpFolders As DAO.Dynaset
		Dim sSQLQuery As String = ""
		Dim iDeleteOK As Integer
		Dim iTmpDocNum As Integer
		Dim iRC As Integer
		Dim lLockNum As Integer
		Dim iLockCount As Integer
		
		Try 
			lLockNum = 0
			
			' Select all folder numbers
			sSQLQuery = "SELECT * FROM folder WHERE drawer_num = " & lDrawerNumber
			
			dsTmpFolders = g_dbDDB.CreateDynaset(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			dsTmpFolders.LockEdits = False
			
			iDeleteOK = True
			iRC = True
			
			If dsTmpFolders.RecordCount = 0 Then
				dsTmpFolders.Close()
				dsTmpFolders = Nothing
				Return iDeleteOK
			End If
			
			While Not dsTmpFolders.EOF
				'  Check to see if we can delete it first...
                If dsTmpFolders("access_level").Value >= g_iUserAccessLevel Then
                    iDeleteOK = True

                    ' Delete Documents
                    iTmpDocNum = dsTmpFolders("folder_num").Value

                    DAO_DBEngine_definst.BeginTrans()

                    ' folder is external ...
                    If dsTmpFolders("ex_code").Value.Trim() <> "" Then
                        If Not DelHistoryData(FOLDER, dsTmpFolders("folder_num").Value) Then
                            iDeleteOK = False
                            iRC = False
                        End If
                    End If

                    If iDeleteOK Then
                        ' if this is not a linked folder ...
                        If LinkOwner(FOLDER, dsTmpFolders("folder_num").Value) = 0 Then
                            If Not (DeleteDocuments(iTmpDocNum)) Then
                                iDeleteOK = False
                                iRC = False
                                DAO_DBEngine_definst.Rollback()
                            Else
                                g_iTmp = 0
                                g_iRC = DeleteDynaset(dsTmpFolders)
                                Select Case g_iRC
                                    Case PM_TRUE
                                        result = True
                                    Case PM_FALSE, PM_CANCEL
                                        Interaction.MsgBox("Delete failed. (cdb)", MB_ICONEXCLAMATION, "DeleteFolders")
                                        result = False
                                    Case Else
                                        Interaction.MsgBox("Delete failed. (cdb) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteFolders")
                                        result = False
                                End Select

                                g_iTmp = 0
                                g_iRC = CommitDatabase()
                                Select Case g_iRC
                                    Case PM_TRUE
                                        result = True
                                    Case PM_FALSE, PM_CANCEL
                                        DAO_DBEngine_definst.Rollback()
                                        Interaction.MsgBox("Update failed. (cdb)", MB_ICONEXCLAMATION, "DeleteFolders")
                                        result = False
                                    Case Else
                                        DAO_DBEngine_definst.Rollback()
                                        Interaction.MsgBox("Update failed. (cdb) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteFolders")
                                        result = False
                                End Select

                            End If
                        Else
                            'if this is a linked folder, dont delete it ...
                            dsTmpFolders.Edit()
                            dsTmpFolders("drawer_num").Value = 0

                            g_iTmp = 0
                            g_iRC = UpdateDynaset(dsTmpFolders)
                            Select Case g_iRC
                                Case PM_TRUE
                                    result = True
                                Case PM_FALSE
                                    Interaction.MsgBox("Delete Failed. (ds)", MB_ICONEXCLAMATION, "DeleteFolders")
                                    result = False
                                Case PM_CANCEL
                                    Interaction.MsgBox("Delete Canceled. (ds)", MB_ICONEXCLAMATION, "DeleteFolders")
                                    result = False
                                Case Else
                                    Interaction.MsgBox("Delete Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteFolders")
                                    result = False
                            End Select

                            g_iTmp = 0
                            g_iRC = CommitDatabase()
                            Select Case g_iRC
                                Case PM_TRUE
                                    result = True
                                Case PM_FALSE
                                    DAO_DBEngine_definst.Rollback()
                                    Interaction.MsgBox("Delete Failed. (cdb)", MB_ICONEXCLAMATION, "DeleteFolders")
                                    result = False
                                Case PM_CANCEL
                                    DAO_DBEngine_definst.Rollback()
                                    Interaction.MsgBox("Delete Canceled. (cdb)", MB_ICONEXCLAMATION, "DeleteFolders")
                                    result = False
                                Case Else
                                    DAO_DBEngine_definst.Rollback()
                                    Interaction.MsgBox("Delete Failed. (cdb) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteFolders")
                                    result = False
                            End Select

                        End If
                    Else
                        iRC = False
                        DAO_DBEngine_definst.Rollback()
                    End If

                Else
                    iRC = False
                End If
				
				dsTmpFolders.MoveNext()
			End While
			
			dsTmpFolders.Close()
			dsTmpFolders = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function DeleteFTAO(ByRef lDocNumber As Integer) As Integer
		Dim result As Integer = 0
		Dim dsTmpFTAO As DAO.Dynaset
		Dim sSQLQuery As String = ""
		
		Try 
			
			sSQLQuery = "SELECT * FROM FTAO WHERE doc_num = " & lDocNumber
			
			dsTmpFTAO = g_dbDDB.CreateDynaset(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			dsTmpFTAO.LockEdits = False
			
			If dsTmpFTAO.RecordCount = 0 Then
				dsTmpFTAO.Close()
				dsTmpFTAO = Nothing
				Return True
			End If
			
			While Not dsTmpFTAO.EOF
				g_iTmp = 0
				g_iRC = DeleteDynaset(dsTmpFTAO)
				Select Case g_iRC
					Case PM_TRUE
						result = True
					Case PM_FALSE, PM_CANCEL
						Interaction.MsgBox("Delete failed. (ds)", MB_ICONEXCLAMATION, "DeleteFTAO")
						result = False
					Case Else
						Interaction.MsgBox("Delete failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteFTAO")
						result = False
				End Select
				
				dsTmpFTAO.MoveNext()
			End While
			
			dsTmpFTAO.Close()
			dsTmpFTAO = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function DeleteKeyWord(ByRef lDocNumber As Integer) As Integer
		Dim result As Integer = 0
		Dim dsTmpKey As DAO.Dynaset
		Dim sSQLQuery As String = ""
		
		Try 
			
			sSQLQuery = "SELECT * FROM keyword WHERE doc_num = " & lDocNumber
			
			dsTmpKey = g_dbDDB.CreateDynaset(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			dsTmpKey.LockEdits = False
			
			If dsTmpKey.RecordCount = 0 Then
				dsTmpKey.Close()
				dsTmpKey = Nothing
				Return True
			End If
			
			While Not dsTmpKey.EOF
				g_iTmp = 0
				g_iRC = DeleteDynaset(dsTmpKey)
				Select Case g_iRC
					Case PM_TRUE
						result = True
					Case PM_FALSE, PM_CANCEL
						Interaction.MsgBox("Delete failed. (ds)", MB_ICONEXCLAMATION, "DeleteKeyWord")
						result = False
					Case Else
						Interaction.MsgBox("Delete failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteKeyWord")
						result = False
				End Select
				
				dsTmpKey.MoveNext()
			End While
			
			dsTmpKey.Close()
			dsTmpKey = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function DeleteKeywords(ByRef lKeywordsNumber As Integer) As Integer
		
		Dim result As Integer = 0
		Dim tbKeywords As DAO.Dynaset
		Dim sSQL As String = ""
		Dim ssSnap As DAO.Snapshot
		Dim lLockNum As Integer
		Dim iLockCount As Integer
		
		Try 
			lLockNum = 0
			
			sSQL = "SELECT * FROM keywords WHERE key_num = " & lKeywordsNumber
			
			tbKeywords = g_dbDDB.CreateDynaset(sSQL)
			DAO_DBEngine_definst.FreeLocks()
			tbKeywords.LockEdits = False
			
			tbKeywords.Edit()
			
            tbKeywords("deleted").Value = True
			
			g_iTmp = 0
			g_iRC = UpdateDynaset(tbKeywords)
			Select Case g_iRC
				Case PM_TRUE
					result = True
				Case PM_FALSE
					Interaction.MsgBox("Delete Failed. (ds)", MB_ICONEXCLAMATION, "DeleteKeywords")
					result = False
				Case PM_CANCEL
					Interaction.MsgBox("Delete Canceled. (ds)", MB_ICONEXCLAMATION, "DeleteKeywords")
					result = False
				Case Else
					Interaction.MsgBox("Delete Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DeleteKeywords")
					result = False
			End Select
			
			
			tbKeywords.Close()
			tbKeywords = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: DeletePages
	'      Author: simonb
	'        Date: 12/04/95
	'
	' Description: Deletes the record level structure from the DDB.
	'
	'*********************************************************************
	'
	Function DeletePages(ByRef lDocumentNumber As Integer) As Integer
		Dim result As Integer = 0
		Dim dsTmpPage As DAO.Dynaset
		Dim sSQLQuery, sVolPath As String
		
		Try 
			result = True
			
			' Delete pages
			sSQLQuery = "SELECT * FROM page WHERE doc_num = " & lDocumentNumber
			
			dsTmpPage = g_dbDDB.CreateDynaset(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			dsTmpPage.LockEdits = False
			
			If dsTmpPage.RecordCount <> 0 Then
				
				While Not dsTmpPage.EOF
					' Delete Pages
                    sVolPath = GetVolumePath(dsTmpPage("volume_name").Value.Trim())
					
					' If the volume is not available, we can't do this.
					
					If sVolPath = "" Then
						result = False
						dsTmpPage.Close()
						dsTmpPage = Nothing
						Return result
					End If
					
					
					' if we fail to delete the actual file, set the page.doc_num = 0. This will
					' then be picked up by the DDB cleanup routine and remove the file and the
					' record from the page table. It's quite likely that this file delete will fail
					' as the NT filestore is typically setup to disallow file deletes.
					
                    If KillFile(sVolPath & dsTmpPage("page_name").Value.Trim() & "." & dsTmpPage("page_type").Value.Trim()) = PM_FALSE Then
                        dsTmpPage.Edit()
                        dsTmpPage("doc_num").Value = 0

                        g_iTmp = 0
                        g_iRC = UpdateDynaset(dsTmpPage)
                        Select Case g_iRC
                            Case PM_TRUE
                                result = True
                            Case PM_FALSE, PM_CANCEL
                                Interaction.MsgBox("Update failed. (ds)", MB_ICONEXCLAMATION, "DeletePages")
                                result = False
                            Case Else
                                Interaction.MsgBox("Update failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DeletePages")
                                result = False
                        End Select
                    Else
                        g_iTmp = 0
                        g_iRC = DeleteDynaset(dsTmpPage)
                        Select Case g_iRC
                            Case PM_TRUE
                                result = True
                            Case PM_FALSE, PM_CANCEL
                                Interaction.MsgBox("Delete failed. (ds)", MB_ICONEXCLAMATION, "DeletePages")
                                result = False
                            Case Else
                                Interaction.MsgBox("Delete failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DeletePages")
                                result = False
                        End Select
                    End If
					
					dsTmpPage.MoveNext()
				End While
				
			End If
			
			dsTmpPage.Close()
			dsTmpPage = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function DeleteScanAnnotations(ByRef lDocNumber As Integer, ByRef sAnnotation As String) As Integer
		
		Dim dsTmpAnn As DAO.Dynaset
		Dim sSQLQuery As String = ""
		
		Try 
			
			If sAnnotation = "" Then
				' Select all document numbers
				sSQLQuery = "SELECT * FROM annotation WHERE doc_num = " & lDocNumber
			Else
				' Select single annotation
				sSQLQuery = "SELECT * FROM annotation WHERE doc_num = " & lDocNumber & " AND "
				sSQLQuery = sSQLQuery & "ann_text = '" & sAnnotation & "'"
			End If
			
			dsTmpAnn = g_dbSDB.CreateDynaset(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			dsTmpAnn.LockEdits = False
			
			If dsTmpAnn.RecordCount = 0 Then
				dsTmpAnn.Close()
				dsTmpAnn = Nothing
				Return True
			End If
			
			While Not dsTmpAnn.EOF
				dsTmpAnn.Delete()
				dsTmpAnn.MoveNext()
			End While
			
			dsTmpAnn.Close()
			dsTmpAnn = Nothing
			
			Return True
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function DeleteScanKeyWord(ByRef lDocumentNumber As Integer, ByRef lKeyWordNumber As Integer) As Integer
		
		Dim sSQLQuery As String = ""
		Dim dsTmpKeyWord As DAO.Dynaset
		
		Cursor.Current = Cursors.WaitCursor
		
		Try 
			
			sSQLQuery = "SELECT * FROM keyword "
			sSQLQuery = sSQLQuery & "WHERE doc_num = " & CStr(lDocumentNumber) & " AND "
			sSQLQuery = sSQLQuery & "key_num = " & CStr(lKeyWordNumber)
			
			dsTmpKeyWord = g_dbSDB.CreateDynaset(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			dsTmpKeyWord.LockEdits = False
			
			If dsTmpKeyWord.RecordCount = 0 Then
				dsTmpKeyWord.Close()
				dsTmpKeyWord = Nothing
				Cursor.Current = Cursors.Default
				Return False
			End If
			
			dsTmpKeyWord.Delete()
			dsTmpKeyWord.Close()
			dsTmpKeyWord = Nothing
			
			Cursor.Current = Cursors.Default
			Return True
		
		Catch 
			
			
			
			Cursor.Current = Cursors.Default
			Return False
		End Try
		
	End Function
	
	Function DeleteScanKeywords(ByRef lKeywordsNumber As Integer) As Integer
		
		Dim tbKeywords As DAO.Dynaset
		Dim sSQL As String = ""
		Dim ssSnap As DAO.Snapshot
		Dim lLockNum As Integer
		Dim iLockCount As Integer
		
		Try 
			lLockNum = 0
			
			sSQL = "SELECT * FROM keywords WHERE key_num = " & lKeywordsNumber
			
			tbKeywords = g_dbSDB.CreateDynaset(sSQL)
			DAO_DBEngine_definst.FreeLocks()
			tbKeywords.LockEdits = False
			
			tbKeywords.Edit()
			
            tbKeywords("deleted").Value = True
			
			tbKeywords.Update()
			tbKeywords.Close()
			tbKeywords = Nothing
			
			'    If RemoveRecordLock(lLockNum&) Then
			'    End If
			
			Return True
		
		Catch 
			
			
			
			If lLockNum > 0 Then
				'        If RemoveRecordLock(lLockNum&) Then
				'        End If
			End If
			Return False
		End Try
		
	End Function
	
	Function DelHistoryData(ByRef iLevel As Integer, ByRef lNode As Integer) As Integer
		Dim result As Integer = 0
		Dim utHist As DMSHIST.g_utDMSHistData = DMSHIST.g_utDMSHistData.CreateInstance()
		Dim lDraw, lFold, lCab As Integer
		
		Try 
			
			result = True
			
			'   If g_sAppType = "WAPI" Then
			'       Exit Function
			'   End If
			
			Select Case iLevel
				Case CABINET
					utHist.cabinetcode.Value = GetDDBCode(CABINET, lNode)
					
					If utHist.cabinetcode.Value.Trim() = "" Then
						Return False
					End If
					
					If Not (UpdateHDB(DELCABINET, g_sHistoryRoot, utHist)) Then
						result = False
					End If
					
				Case DRAWER
					lCab = GetParentNumber(DRAWER, lNode)
					
					utHist.cabinetcode.Value = GetDDBCode(CABINET, lCab)
					utHist.drawercode.Value = GetDDBCode(DRAWER, lNode)
					
					If utHist.cabinetcode.Value.Trim() = "" Or utHist.drawercode.Value.Trim() = "" Then
						Return False
					End If
					
					If Not (UpdateHDB(DELDRAWER, g_sHistoryRoot, utHist)) Then
						result = False
					End If
					
				Case FOLDER
					lDraw = GetParentNumber(FOLDER, lNode)
					lCab = GetParentNumber(DRAWER, lDraw)
					
					utHist.cabinetcode.Value = GetDDBCode(CABINET, lCab)
					utHist.drawercode.Value = GetDDBCode(DRAWER, lDraw)
					utHist.foldercode.Value = GetDDBCode(FOLDER, lNode)
					
					If utHist.cabinetcode.Value.Trim() = "" Or utHist.drawercode.Value.Trim() = "" Or utHist.foldercode.Value.Trim() = "" Then
						Return False
					End If
					
					If Not (UpdateHDB(DELFOLDER, g_sHistoryRoot, utHist)) Then
						result = False
					End If
					
				Case DOCUMENT
					lFold = GetParentNumber(DOCUMENT, lNode)
					lDraw = GetParentNumber(FOLDER, lFold)
					lCab = GetParentNumber(DRAWER, lDraw)
					
					utHist.cabinetcode.Value = GetDDBCode(CABINET, lCab)
					utHist.drawercode.Value = GetDDBCode(DRAWER, lDraw)
					utHist.foldercode.Value = GetDDBCode(FOLDER, lFold)
					utHist.docref.Value = "000000000"
					Mid(utHist.docref.Value, 10 - Conversion.Str(lNode).Trim().Length, Conversion.Str(lNode).Trim().Length) = Conversion.Str(lNode).Trim()
					utHist.date_Renamed.Value = DateTime.FromOADate(GetDocDate(lNode)).ToString("yyyyMMdd")
					utHist.time.Value = DateTime.FromOADate(GetDocDate(lNode)).ToString("HHMMss")
					
					If utHist.cabinetcode.Value.Trim() = "" Or utHist.drawercode.Value.Trim() = "" Or utHist.foldercode.Value.Trim() = "" Or utHist.docref.Value.Trim() = "0" Then
						Return False
					End If
					
					If Not (UpdateHDB(DELDOCUMENT, g_sHistoryRoot, utHist)) Then
						result = False
					End If
			End Select
			Return result
		
		Catch 
			
			
			
			result = False
			Interaction.MsgBox("Delete Failed - " & Information.Err().Number, MB_ICONEXCLAMATION, "DelHistoryData")
			Return result
		End Try
		
	End Function
	
	Function DisableLogins(ByRef iAction As Integer) As Integer
		Dim result As Integer = 0
		Dim dsSystem As DAO.Dynaset
		
		Try 
			result = True
			
			dsSystem = g_dbDDB.CreateDynaset("SELECT login FROM System")
			DAO_DBEngine_definst.FreeLocks()
			dsSystem.LockEdits = False
			
			If dsSystem.RecordCount <> 1 Then
				result = False
			Else
				dsSystem.Edit()
				If iAction Then
					' disable logins
                    dsSystem("login").Value = False
				Else
					' enable logins
                    dsSystem("login").Value = True
				End If
				
				g_iRC = UpdateDynaset(dsSystem)
				Select Case g_iRC
					Case PM_TRUE
						result = True
					Case PM_FALSE
						result = False
					Case Else
						Interaction.MsgBox("Update Failed (ds) - " & g_iRC, MB_ICONEXCLAMATION, "DisableLogins")
						result = False
				End Select
			End If
			
			dsSystem.Close()
			dsSystem = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function DocExists(ByRef lFoldCode As Integer, ByRef sDocName As String) As Integer
		Dim result As Integer = 0
		Dim ssTmpExists As DAO.Snapshot
		
		Dim sSQLQuery As String = "SELECT doc_num FROM document WHERE "
		sSQLQuery = sSQLQuery & "ex_code = '" & sDocName & "' AND "
		sSQLQuery = sSQLQuery & "folder_num = " & CStr(lFoldCode)
		
		Try 
			
			ssTmpExists = g_dbDDB.CreateSnapshot(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			
            result = ssTmpExists(0).Value
			
			ssTmpExists.Close()
			Return result
		
		Catch 
			
			
			
			ssTmpExists.Close()
			Return 0
		End Try
		
	End Function
	
	Function DrawerExists(ByRef lCabCode As Integer, ByRef sDrawName As String) As Integer
		Dim result As Integer = 0
		Dim ssTmpExists As DAO.Snapshot
		
		Dim sSQLQuery As String = "SELECT drawer_num FROM drawer WHERE "
		sSQLQuery = sSQLQuery & "ex_code = '" & sDrawName & "' AND "
		sSQLQuery = sSQLQuery & "cabinet_num = " & CStr(lCabCode)
		
		Try 
			
			ssTmpExists = g_dbDDB.CreateSnapshot(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			
            result = ssTmpExists(0).Value
			
			ssTmpExists.Close()
			Return result
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
	
	'RAM20030102 : Changed the function name from FolderExists To FolderExistsInDB
	'               since it conflicts with gPMFunctions
	Function FolderExistsInDB(ByRef lDrawCode As Integer, ByRef sFoldName As String) As Integer
		Dim result As Integer = 0
		Dim ssTmpExists As DAO.Snapshot
		
		Dim sSQLQuery As String = "SELECT folder_num FROM folder WHERE "
		sSQLQuery = sSQLQuery & "ex_code = '" & sFoldName & "' AND "
		sSQLQuery = sSQLQuery & "drawer_num = " & CStr(lDrawCode)
		
		Try 
			
			ssTmpExists = g_dbDDB.CreateSnapshot(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			
            result = ssTmpExists(0).Value
			
			ssTmpExists.Close()
			Return result
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: FormatDataBlock
	'      Author: simonb
	'        Date: 11/04/95
	'
	' Description:
	'
	'*********************************************************************
	'
	Function FormatDataBlock(ByRef iLevel As Integer, ByRef ssSnapshotName As DAO.Snapshot, ByRef sBlockDataArray() As String, ByRef lBlockNumArray() As Integer, ByRef sBlockStartArray() As String, ByRef iBlockNumber As Integer) As Integer
		Dim ErrFormatDataBlock As Boolean = False
		Dim ErrBlockDataArray As Boolean = False
		Dim ErrBlockStartArray As Boolean = False
		Dim result As Integer = 0
		Dim iBlockCntr As Integer
		Dim sSQLQuery, sTmpPassword As String
		
		' Check If Any Records Where Found
		If ssSnapshotName.RecordCount = 0 Then
			Return False
		End If
		
		' Get Number Of Blocks
		Dim iMaxBlocks As Integer = Math.Floor(ssSnapshotName.RecordCount / MAX_READ)
		
		If ssSnapshotName.RecordCount Mod MAX_READ > 0 Then
			iMaxBlocks += 1
		End If
		
		' Check MaxBlocks Is Greater Than BlockNumber
		If iBlockNumber > iMaxBlocks Then
			Return False
		Else
			result = iMaxBlocks
		End If
		
		Try 
			ErrBlockStartArray = True
			ErrFormatDataBlock = False
			ErrBlockDataArray = False
			
			' Create New record in the array
			If sBlockStartArray.GetUpperBound(0) < (iBlockNumber + 1) Then
				ReDim Preserve sBlockStartArray(iBlockNumber + 1)
			End If
			
			ReDim sBlockDataArray(0)
			ReDim lBlockNumArray(0)
			
			If iBlockNumber > 1 Then
				If sBlockStartArray(iBlockNumber) = "" Then
					' Generate Start Array
					sBlockStartArray(1) = ""
					ssSnapshotName.MoveFirst()
					
					iBlockCntr = 2
					While (iBlockCntr <= iBlockNumber)
						For iReadCntr As Integer = 1 To MAX_READ
							ssSnapshotName.MoveNext()
						Next iReadCntr
						sBlockStartArray(iBlockCntr) = ssSnapshotName(1)
						
						iBlockCntr += 1
					End While
				Else
					' Seek to Start of Block Number
					Select Case iLevel
						Case CABINET
							sSQLQuery = "cabinet_name"
						Case DRAWER
							sSQLQuery = "drawer_name"
						Case FOLDER
							sSQLQuery = "folder_name"
						Case DOCUMENT
							sSQLQuery = "doc_name"
						Case PAGE
							sSQLQuery = "page_name"
					End Select
					
					' salvo(090796) - Correct this (used to be a >)
					ssSnapshotName.MoveFirst()
					ssSnapshotName.FindNext(sSQLQuery & " = " & Strings.Chr(34).ToString() & sBlockStartArray(iBlockNumber) & Strings.Chr(34).ToString())
					DAO_DBEngine_definst.FreeLocks()
					
				End If
			Else
				ssSnapshotName.MoveFirst()
			End If
			
			iBlockCntr = 0
			
			' Load Data Array With Requested Block
			While (Not ssSnapshotName.EOF And iBlockCntr < MAX_READ)
				ErrBlockDataArray = True
				ErrFormatDataBlock = False
				ErrBlockStartArray = False
				
				ReDim Preserve sBlockDataArray(sBlockDataArray.GetUpperBound(0) + 1)
				ReDim Preserve lBlockNumArray(lBlockNumArray.GetUpperBound(0) + 1)
				
				ErrFormatDataBlock = True
				ErrBlockDataArray = False
				ErrBlockStartArray = False
				
				If iLevel = PAGE Then
					
                    sBlockDataArray(sBlockDataArray.GetUpperBound(0)) = ssSnapshotName(0).Value & "." & ssSnapshotName(1).Value
                    sBlockStartArray(iBlockNumber + 1) = ssSnapshotName(0).Value
					
				Else

					If Convert.IsDBNull(ssSnapshotName(1)) Or IsNothing(ssSnapshotName(1)) Then
						'If the description is null...
						sBlockDataArray(sBlockDataArray.GetUpperBound(0)) = "<no description>"
						
					Else
                        If ssSnapshotName(1).Value.Trim() = "" Then
                            'if the description is blank...
                            sBlockDataArray(sBlockDataArray.GetUpperBound(0)) = "<no description>"
                        Else
                            sBlockDataArray(sBlockDataArray.GetUpperBound(0)) = ssSnapshotName(1).Value
                            sBlockStartArray(iBlockNumber + 1) = ssSnapshotName(1).Value
                        End If
					End If
					
					' Check if Password is present, if so make
					' number a minis

					If Convert.IsDBNull(ssSnapshotName(2)) Or IsNothing(ssSnapshotName(2)) Then
						sTmpPassword = ""
					Else
                        sTmpPassword = ssSnapshotName(2).Value.Trim()
					End If
					
					If sTmpPassword = "" Then
                        lBlockNumArray(lBlockNumArray.GetUpperBound(0)) = ssSnapshotName(0).Value
					Else
                        lBlockNumArray(lBlockNumArray.GetUpperBound(0)) = (CInt(ssSnapshotName(0).Value * -1))
					End If
					
				End If
				
				iBlockCntr += 1
				
				' Move To Next Record
				ssSnapshotName.MoveNext()
			End While
			
			ErrFormatDataBlock = True
			ErrBlockDataArray = False
			ErrBlockStartArray = False
			Return result
		
		Catch excep As System.Exception
			If Not ErrFormatDataBlock And Not ErrBlockDataArray And Not ErrBlockStartArray Then
				Throw excep
			End If
			
			If ErrBlockStartArray Then
				
				ReDim sBlockStartArray(iBlockNumber + 1)


				
			End If
			If ErrBlockDataArray Or ErrBlockStartArray Then
				
				ReDim sBlockDataArray(1)
				ReDim lBlockNumArray(1)


				
			End If
			If ErrFormatDataBlock Or ErrBlockDataArray Or ErrBlockStartArray Then
				
				
				Return 0
				
			End If
		End Try
	End Function
	
	Function GetActiveVolumeName() As String
		Dim result As String = String.Empty
		Dim ssVolume As DAO.Snapshot
		
		Try 
			
			ssVolume = g_dbDDB.CreateSnapshot("SELECT active_volume FROM hotzone")
			DAO_DBEngine_definst.FreeLocks()
			
            result = ssVolume("active_volume").Value.Trim()
			
			ssVolume.Close()
			Return result
		
		Catch 
			
			
			
			Return ""
		End Try
		
	End Function
	
	Function GetAdminLevel() As Integer
		
		Dim result As Integer = 0
		Dim ssSnapShot As DAO.Snapshot
		
		Try 
			
			ssSnapShot = g_dbDDB.CreateSnapshot("SELECT admin_level FROM system")
			DAO_DBEngine_definst.FreeLocks()
			
			If ssSnapShot.RecordCount < 1 Then
				Interaction.MsgBox("Error Reading System table", MB_ICONEXCLAMATION, "Table Error")
				Return result
			End If
			
            If ssSnapShot("admin_level").Value < 1 Or ssSnapShot("admin_level").Value > 9 Then
                result = 1
            Else
                result = ssSnapShot("admin_level").Value
            End If
			
			ssSnapShot.Close()
			ssSnapShot = Nothing
			
			Return result
		
		Catch 
			
			
			
			Interaction.MsgBox("Error Reading System table", MB_ICONEXCLAMATION, "Table Error")
			Return 1
		End Try
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: GetALLCabinet
	'      Author: simonb
	'        Date: 11/04/95
	'
	' Description: Gets all the cabinet data from the DDB.
	'
	'*********************************************************************
	'
	Function GetALLCabinet(ByRef sBlockStartArray() As String, ByRef iBlockNumber As Integer) As Integer
		
		Return GetDDBData(CABINET, ALL, "", "", 0, sBlockStartArray, iBlockNumber)
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: GetALLDocument
	'      Author: simonb
	'        Date: 11/04/95
	'
	' Description: Gets all the document data from the DDB.
	'
	'*********************************************************************
	'
	Function GetALLDocument(ByRef iSearchNumber As Integer, ByRef sBlockStartArray() As String, ByRef iBlockNumber As Integer) As Integer
		
		Return GetDDBData(DOCUMENT, ALL, "", "", iSearchNumber, sBlockStartArray, iBlockNumber)
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: GetALLDrawer
	'      Author: simonb
	'        Date: 11/04/95
	'
	' Description: Gets all the drawer data from the DDB.
	'
	'*********************************************************************
	'
	Function GetALLDrawer(ByRef iSearchNumber As Integer, ByRef sBlockStartArray() As String, ByRef iBlockNumber As Integer) As Integer
		
		Return GetDDBData(DRAWER, ALL, "", "", iSearchNumber, sBlockStartArray, iBlockNumber)
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: GetALLFolder
	'      Author: simonb
	'        Date: 11/04/95
	'
	' Description: Gets all the folder data from the DDB.
	'
	'*********************************************************************
	'
	Function GetALLFolder(ByRef iSearchNumber As Integer, ByRef sBlockStartArray() As String, ByRef iBlockNumber As Integer) As Integer
		
		Return GetDDBData(FOLDER, ALL, "", "", iSearchNumber, sBlockStartArray, iBlockNumber)
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: GetALLPage
	'      Author: simonb
	'        Date: 11/04/95
	'
	' Description: Gets all the page data from the DDB.
	'
	'*********************************************************************
	'
	Function GetALLPage(ByRef iSearchNumber As Integer, ByRef sBlockStartArray() As String, ByRef iBlockNumber As Integer) As Integer
		
		Return GetDDBData(PAGE, ALL, "", "", iSearchNumber, sBlockStartArray, iBlockNumber)
		
	End Function
	
	Function GetDDBCode(ByRef iLevel As Integer, ByRef lSearchNumber As Integer) As String
		
		Dim result As String = String.Empty
		Dim ssSnapShot As DAO.Snapshot
		Dim sSQLQuery As String = ""
		
		Try 
			
			Select Case iLevel
				Case CABINET
					sSQLQuery = "SELECT ex_code FROM cabinet WHERE cabinet_num = " & lSearchNumber
				Case DRAWER
					sSQLQuery = "SELECT ex_code FROM drawer WHERE drawer_num = " & lSearchNumber
				Case FOLDER
					sSQLQuery = "SELECT ex_code FROM folder WHERE folder_num = " & lSearchNumber
			End Select
			
			ssSnapShot = g_dbDDB.CreateSnapshot(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			
            result = ssSnapShot("ex_code").Value.Trim()
			
			ssSnapShot.Close()
			ssSnapShot = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return ""
		End Try
		
	End Function
	
	'*********************************************************************
	'
	' Module Name:GetDDBData
	'      Author: simonb
	'        Date: 11/04/95
	'
	' Description:
	'
	'*********************************************************************
	'
	Function GetDDBData(ByRef iLevel As Integer, ByRef iSearchType As Integer, ByRef sQStart As String, ByRef sQEnd As String, ByRef lSearchNumber As Integer, ByRef sBlockStartArray() As String, ByRef iBlockNumber As Integer) As Integer
		
		Dim sSQLQuery As String = ""
		Dim iBlockCntr As Integer
		


		
		If sBlockStartArray(iBlockNumber) = "" Then
			' Forces the first array element to be created
		End If
		
		Select Case iLevel
			Case CABINET
				sSQLQuery = "SELECT cabinet_num, cabinet_name, password FROM cabinet "
				sSQLQuery = sSQLQuery & "WHERE access_level >= " & CStr(g_iUserAccessLevel) & " "
				If iSearchType = SELECTIVE Then
					sSQLQuery = sSQLQuery & "AND cabinet_name >= '" & sQStart & "' AND cabinet_name <= '" & sQEnd & "Z' "
				End If
				sSQLQuery = sSQLQuery & "ORDER BY cabinet_name"
				
			Case DRAWER
				sSQLQuery = "SELECT drawer_num, drawer_name, password FROM drawer "
				sSQLQuery = sSQLQuery & "WHERE drawer.cabinet_num = " & CStr(lSearchNumber) & " "
				sSQLQuery = sSQLQuery & "AND access_level >= " & CStr(g_iUserAccessLevel) & " "
				If iSearchType = SELECTIVE Then
					sSQLQuery = sSQLQuery & "AND drawer_name >= '" & sQStart & "' AND drawer_name <= '" & sQEnd & "Z' "
				End If
				sSQLQuery = sSQLQuery & "ORDER BY drawer_name"
				
			Case FOLDER
				sSQLQuery = "SELECT folder_num, folder_name, password FROM folder "
				sSQLQuery = sSQLQuery & "WHERE folder.drawer_num = " & CStr(lSearchNumber) & " "
				sSQLQuery = sSQLQuery & "AND access_level >= " & CStr(g_iUserAccessLevel) & " "
				If iSearchType = SELECTIVE Then
					sSQLQuery = sSQLQuery & "AND folder_name >= '" & sQStart & "' AND folder_name <= '" & sQEnd & "Z' "
				End If
				sSQLQuery = sSQLQuery & "ORDER BY folder_name"
				
			Case DOCUMENT
				sSQLQuery = "SELECT document.doc_num, doc_name, password, create_date, doc_type, doc_date FROM document, docinfo "
				sSQLQuery = sSQLQuery & "WHERE document.doc_num = docinfo.doc_num AND "
				sSQLQuery = sSQLQuery & "document.folder_num = " & CStr(lSearchNumber) & " "
				sSQLQuery = sSQLQuery & "AND access_level >= " & CStr(g_iUserAccessLevel) & " "
				If iSearchType = SELECTIVE Then
					sSQLQuery = sSQLQuery & "AND doc_name >= '" & sQStart & "' AND doc_name <= '" & sQEnd & "Z' "
				End If
				sSQLQuery = sSQLQuery & "ORDER BY docinfo.doc_date DESC, document.doc_name"
				'  MsgBox sSQLQuery
			Case PAGE
				sSQLQuery = "SELECT page_name, page.page_type, page_num, scan_date FROM page "
				sSQLQuery = sSQLQuery & "WHERE page.doc_num = " & CStr(lSearchNumber) & " "
				If iSearchType = SELECTIVE Then
					sSQLQuery = sSQLQuery & "AND page_name >= '" & sQStart & "' AND page_name <= '" & sQEnd & "Z' "
				End If
				sSQLQuery = sSQLQuery & "ORDER BY page_num"
				
			Case DOCINFO
				sSQLQuery = "SELECT * FROM docinfo "
				sSQLQuery = sSQLQuery & "WHERE doc_num = " & CStr(lSearchNumber) & " "
		End Select
		
		Try 
			
			g_ssSnapshot = g_dbDDB.CreateSnapshot(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			
			
			Return g_ssSnapshot.RecordCount
		
		Catch 
			
			
			
			Return 0
			
ErrBlockStartArray2: 
			ReDim sBlockStartArray(iBlockNumber + 1)


		End Try
		
	End Function
	
	Function GetDDBName(ByRef iLevel As Integer, ByRef lSearchNumber As Integer) As String
		
		Dim result As String = String.Empty
		Dim ssSnapShot As DAO.Snapshot
		Dim sSQLQuery As String = ""
		
		Try 
			
			Select Case iLevel
				Case CABINET
					sSQLQuery = "SELECT cabinet_name FROM cabinet WHERE cabinet_num = " & lSearchNumber
				Case DRAWER
					sSQLQuery = "SELECT drawer_name FROM drawer WHERE drawer_num = " & lSearchNumber
				Case FOLDER
					sSQLQuery = "SELECT folder_name FROM folder WHERE folder_num = " & lSearchNumber
				Case DOCUMENT, DOCINFO, DOCHISTORY
					sSQLQuery = "SELECT doc_name FROM document WHERE doc_num = " & lSearchNumber
				Case DEVICE
					sSQLQuery = "SELECT location FROM device WHERE device_num = " & lSearchNumber
				Case keywords
					sSQLQuery = "SELECT keyword FROM keywords WHERE key_num = " & lSearchNumber
			End Select
			
			ssSnapShot = g_dbDDB.CreateSnapshot(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			
			result = ssSnapShot(0)
			
			ssSnapShot.Close()
			ssSnapShot = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return ""
		End Try
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: GetDDBNumber
	'      Author: simonb
	'        Date: 28/04/95
	'
	' Description: Gets the ID number from the external code.
	'
	'*********************************************************************
	'
	Function GetDDBNumber(ByRef iLevel As Integer, ByRef sExternalCode As String, ByRef sItemDesc As String) As Integer
		
		Dim result As Integer = 0
		Dim ssNumber As DAO.Snapshot
		Dim sSQLQuery As String = ""
		Dim sTmpExternalCode As New FixedLengthString(20)
		Dim sTmpPassword As String = ""
		
		Try 
			
			sTmpExternalCode.Value = sExternalCode
			
			Select Case iLevel
				Case CABINET
					sSQLQuery = "SELECT cabinet_num, cabinet_name, password FROM cabinet WHERE ex_code = '" & sTmpExternalCode.Value.Trim() & "'"
				Case DRAWER
					sSQLQuery = "SELECT drawer_num, drawer_name, password FROM drawer WHERE ex_code = '" & sTmpExternalCode.Value.Trim() & "'"
				Case FOLDER
					sSQLQuery = "SELECT folder_num, folder_name, password FROM folder WHERE ex_code = '" & sTmpExternalCode.Value.Trim() & "'"
				Case DOCUMENT
					sSQLQuery = "SELECT doc_num, doc_name, password FROM document WHERE ex_code = '" & sTmpExternalCode.Value.Trim() & "'"
			End Select
			
			ssNumber = g_dbDDB.CreateSnapshot(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			
			' Check if any records where found
			If ssNumber.RecordCount = 0 Then
				result = 0
				sItemDesc = ""
			Else
				' Check if Password is present, if so make
				' number a minis

				If Convert.IsDBNull(ssNumber("password")) Or IsNothing(ssNumber("password")) Then
					sTmpPassword = ""
				Else
                    sTmpPassword = ssNumber("password").Value.Trim()
				End If
				
				If sTmpPassword = "" Then
                    result = ssNumber(0).Value
				Else
                    result = (CInt(ssNumber(0).Value * -1))
				End If
				
                sItemDesc = ssNumber(1).Value
			End If
			
			ssNumber.Close()
			ssNumber = Nothing
			
			Return result
		
		Catch 
			
			
			
			sItemDesc = ""
			Return result
		End Try
		
	End Function
	
	Function GetDocDate(ByRef lDocumentNumber As Integer) As Double
		
		Dim result As Double = 0
		Dim ssDocDate As DAO.Snapshot
		
		Try 
			
			ssDocDate = g_dbDDB.CreateSnapshot("SELECT doc_date FROM docinfo WHERE doc_num = " & lDocumentNumber)
			DAO_DBEngine_definst.FreeLocks()
			
			If ssDocDate.RecordCount = 1 Then
                result = ssDocDate("doc_date").Value
			Else
				result = 0
			End If
			
			ssDocDate.Close()
			Return result
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: GetALLCabinet
	'      Author: simonb
	'        Date: 11/04/95
	'
	' Description: Gets all the cabinet data from the DDB.
	'
	'*********************************************************************
	'
	Function GetDocInfo(ByRef lDocNumber As Integer) As Integer
		Dim sArr() As String
		
		Return GetDDBData(DOCINFO, ALL, "", "", lDocNumber, sArr, 0)
		
	End Function
	
	Function GetDocPageFile(ByRef lDocNum As Integer) As String
		Dim result As String = String.Empty
		Dim ssPage As DAO.Snapshot
		
		Try 
			
			ssPage = g_dbDDB.CreateSnapshot("SELECT page_name FROM page WHERE doc_num = " & lDocNum.ToString())
			DAO_DBEngine_definst.FreeLocks()
			
			If ssPage.RecordCount > 0 Then
                result = ssPage("page_name").Value.Trim()
			Else
				result = ""
			End If
			
			ssPage.Close()
			Return result
		
		Catch 
			
			
			
			Return ""
		End Try
		
	End Function
	
	Function GetDocType(ByRef lDocNum As Integer) As String
		
		Dim result As String = String.Empty
		Dim ssDoc As DAO.Snapshot
		
		Try 
			
			ssDoc = g_dbDDB.CreateSnapshot("SELECT page_type FROM page WHERE doc_num = " & lDocNum.ToString())
			DAO_DBEngine_definst.FreeLocks()
			
			If ssDoc.RecordCount = 1 Then
                result = ssDoc("page_type").Value.Trim()
			Else
				result = ""
			End If
			
			ssDoc.Close()
			Return result
		
		Catch 
			
			
			
			Return ""
		End Try
		
	End Function
	
	Function GetDocument(ByRef iDirection As Integer, ByRef lFolderNumber As Integer, ByRef lCurrentDocNumber As Integer) As String
		
		Dim result As String = String.Empty
		Dim ssDocs As DAO.Snapshot
		Dim sSQLQuery, sReturnCode As String
		Dim iCntr, iFound As Integer
		Dim lTmpCabinetNumber, lTmpDrawerNumber, lTmpFolderNumber, lTmpDocumentNumber As Integer
		
		Try 
			
			sSQLQuery = "SELECT document.doc_num, document.password, "
			sSQLQuery = sSQLQuery & "folder.folder_num, folder.password, drawer.drawer_num, "
			sSQLQuery = sSQLQuery & "drawer.password, cabinet.cabinet_num, cabinet.password "
			sSQLQuery = sSQLQuery & "FROM folder, document, drawer, cabinet, docinfo "
			sSQLQuery = sSQLQuery & "WHERE document.folder_num = folder.folder_num AND "
			sSQLQuery = sSQLQuery & "folder.drawer_num = drawer.drawer_num AND "
			sSQLQuery = sSQLQuery & "drawer.cabinet_num = cabinet.cabinet_num AND "
			sSQLQuery = sSQLQuery & "docinfo.doc_num = document.doc_num AND "
			sSQLQuery = sSQLQuery & "folder.folder_num = " & CStr(Math.Abs(lFolderNumber)) & " "
			sSQLQuery = sSQLQuery & "ORDER BY docinfo.doc_date DESC"
			
			ssDocs = g_dbDDB.CreateSnapshot(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			
			If ssDocs.RecordCount < 1 Then
				' Failed to get documents
				result = ""
				
				ssDocs.Close()
				ssDocs = Nothing
				
				Return result
			End If
			
			' Search through all documents to try
			' and found current document
			
			iFound = False
			While (Not ssDocs.EOF And Not iFound)
                If ssDocs("document.doc_num").Value = Math.Abs(lCurrentDocNumber) Then
                    ' Found Match
                    iFound = True
                Else
                    ssDocs.MoveNext()
                End If
			End While
			
			If iFound Then
				' We have found the current document,
				' now its time to get the next/previous document
				
				If iDirection = NEXT_DOC Then
					ssDocs.MoveNext()
					
					If Not (ssDocs.EOF) Then
						' Yes we've got the next document.
						' Construct the return string
						

						If Convert.IsDBNull(ssDocs("cabinet.password")) Or IsNothing(ssDocs("cabinet.password")) Then
                            lTmpCabinetNumber = ssDocs("cabinet.cabinet_num").Value
						Else
                            If ssDocs("cabinet.password").Value.Trim() <> "" Then
                                lTmpCabinetNumber = CInt(ssDocs("cabinet.cabinet_num").Value * -1)
                            Else
                                lTmpCabinetNumber = ssDocs("cabinet.cabinet_num").Value
                            End If
						End If
						

						If Convert.IsDBNull(ssDocs("drawer.password")) Or IsNothing(ssDocs("drawer.password")) Then
                            lTmpDrawerNumber = ssDocs("drawer.drawer_num").Value
						Else
                            If ssDocs("drawer.password").Value.Trim() <> "" Then
                                lTmpDrawerNumber = CInt(ssDocs("drawer.drawer_num").Value * -1)
                            Else
                                lTmpDrawerNumber = ssDocs("drawer.drawer_num").Value
                            End If
						End If
						

						If Convert.IsDBNull(ssDocs("folder.password")) Or IsNothing(ssDocs("folder.password")) Then
                            lTmpFolderNumber = ssDocs("folder.cabinet_num").Value
						Else
                            If ssDocs("folder.password").Value.Trim() <> "" Then
                                lTmpFolderNumber = CInt(ssDocs("folder.folder_num").Value * -1)
                            Else
                                lTmpFolderNumber = ssDocs("folder.folder_num").Value
                            End If
						End If
						

						If Convert.IsDBNull(ssDocs("document.password")) Or IsNothing(ssDocs("document.password")) Then
                            lTmpDocumentNumber = ssDocs("document.doc_num").Value
						Else
                            If ssDocs("document.password").Value.Trim() <> "" Then
                                lTmpDocumentNumber = CInt(ssDocs("document.doc_num").Value * -1)
                            Else
                                lTmpDocumentNumber = ssDocs("document.doc_num").Value
                            End If
						End If
						
						sReturnCode = CStr(lTmpCabinetNumber) & "|" & CStr(lTmpDrawerNumber)
						sReturnCode = sReturnCode & "|" & CStr(lTmpFolderNumber)
						sReturnCode = sReturnCode & "|" & CStr(lTmpDocumentNumber)
						result = sReturnCode
					Else
						result = ""
					End If
				Else
					ssDocs.MovePrevious()
					
					If Not (ssDocs.BOF) Then
						' Yes we've got the previous document.
						' Construct the return string
						

						If Convert.IsDBNull(ssDocs("cabinet.password")) Or IsNothing(ssDocs("cabinet.password")) Then
                            lTmpCabinetNumber = ssDocs("cabinet.cabinet_num").Value
						Else
                            If ssDocs("cabinet.password").Value.Trim() <> "" Then
                                lTmpCabinetNumber = CInt(ssDocs("cabinet.cabinet_num").Value * -1)
                            Else
                                lTmpCabinetNumber = ssDocs("cabinet.cabinet_num").Value
                            End If
						End If
						

						If Convert.IsDBNull(ssDocs("drawer.password")) Or IsNothing(ssDocs("drawer.password")) Then
                            lTmpDrawerNumber = ssDocs("drawer.drawer_num").Value
						Else
                            If ssDocs("drawer.password").Value.Trim() <> "" Then
                                lTmpDrawerNumber = CInt(ssDocs("drawer.drawer_num").Value * -1)
                            Else
                                lTmpDrawerNumber = ssDocs("drawer.drawer_num").Value
                            End If
						End If
						

						If Convert.IsDBNull(ssDocs("folder.password")) Or IsNothing(ssDocs("folder.password")) Then
                            lTmpFolderNumber = ssDocs("folder.cabinet_num").Value
						Else
                            If ssDocs("folder.password").Value.Trim() <> "" Then
                                lTmpFolderNumber = CInt(ssDocs("folder.folder_num").Value * -1)
                            Else
                                lTmpFolderNumber = ssDocs("folder.folder_num").Value
                            End If
						End If
						

						If Convert.IsDBNull(ssDocs("document.password")) Or IsNothing(ssDocs("document.password")) Then
                            lTmpDocumentNumber = ssDocs("document.doc_num").Value
						Else
                            If ssDocs("document.password").Value.Trim() <> "" Then
                                lTmpDocumentNumber = CInt(ssDocs("document.doc_num").Value * -1)
                            Else
                                lTmpDocumentNumber = ssDocs("document.doc_num").Value
                            End If
						End If
						
						sReturnCode = CStr(lTmpCabinetNumber) & "|" & CStr(lTmpDrawerNumber)
						sReturnCode = sReturnCode & "|" & CStr(lTmpFolderNumber)
						sReturnCode = sReturnCode & "|" & CStr(lTmpDocumentNumber)
						result = sReturnCode
					Else
						result = ""
					End If
				End If
			End If
			
			ssDocs.Close()
			ssDocs = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return ""
		End Try
		
	End Function
	
	Sub GetDocumentTypes(ByRef f_ssDocument As DAO.Snapshot, ByRef iDocumentType() As Integer)
		
		Dim iCntr As Integer
		
		ReDim iDocumentType(0)
		f_ssDocument.MoveFirst()
		
		While Not f_ssDocument.EOF
			ReDim Preserve iDocumentType(iDocumentType.GetUpperBound(0) + 1)
			
			Select Case (f_ssDocument("doc_type"))
				Case "I"
					iDocumentType(iDocumentType.GetUpperBound(0)) = TYPE_TIF
				Case "L", "N"
					iDocumentType(iDocumentType.GetUpperBound(0)) = TYPE_TEXT
					' Salvo(071096) - WordMaster now differntiates betwixt word docs
					'                 saved as text (L) or RTF (W)
				Case "W"
					iDocumentType(iDocumentType.GetUpperBound(0)) = TYPE_RTF
				Case Else
					iDocumentType(iDocumentType.GetUpperBound(0)) = 0
			End Select
			
			f_ssDocument.MoveNext()
		End While
		
	End Sub
	
	Function GetDocVolume(ByRef lDocNum As Integer) As String
		Dim result As String = String.Empty
		Dim ssPage As DAO.Snapshot
		
		Try 
			
			ssPage = g_dbDDB.CreateSnapshot("SELECT volume_name FROM page WHERE doc_num = " & lDocNum.ToString())
			DAO_DBEngine_definst.FreeLocks()
			
			If ssPage.RecordCount > 0 Then
                result = ssPage("volume_name").Value.Trim()
			Else
				result = ""
			End If
			
			ssPage.Close()
			Return result
		
		Catch 
			
			
			
			Return ""
		End Try
		
	End Function
	
	Function GetDrawerLoadedNumber(ByRef lCabinetNumber As Integer, ByRef iDrawerLoadedNumber As Integer) As Integer
		
		
		Try 
			
			iDrawerLoadedNumber = 0
			
			' Check if the drawer has been loaded already.
			' We do this by checking if the Cabinet number
			' exists in the array
			For iCntr As Integer = 1 To g_utDrawerData.GetUpperBound(0)
				If g_utDrawerData(iCntr).CabinetNumber = Math.Abs(lCabinetNumber) Then
					iDrawerLoadedNumber = iCntr
					Exit For
				End If
			Next iCntr
			
			
			If iDrawerLoadedNumber = 0 Then
				' Didn't find the Cabinet number, so we
				' must create a place in the array for him
				ReDim Preserve g_utDrawerData(g_utDrawerData.GetUpperBound(0) + 1)
				ReDim Preserve g_ssDrawer(g_ssDrawer.GetUpperBound(0) + 1)
				g_utDrawerData(g_utDrawerData.GetUpperBound(0)).CabinetNumber = CInt(Math.Abs(lCabinetNumber))
				g_utDrawerData(g_utDrawerData.GetUpperBound(0)).Loaded = False
				
				' Because we just created the new array we know
				' that the drawer has NOT been loaded before.
				iDrawerLoadedNumber = g_utDrawerData.GetUpperBound(0)
				Return PM_FALSE
			Else
				' Found a Cabinet number, so now its time
				' to see if the drawer has been loaded in
				' his previous life
				If g_utDrawerData(g_utDrawerData.GetUpperBound(0)).Loaded Then
					Return PM_TRUE
				Else
					Return PM_FALSE
				End If
			End If
		
		Catch 
		End Try
		
		
		
		Return PM_ERROR
		
	End Function
	
	Function GetEventType(ByRef lDocNum As Integer) As String
		Dim result As String = String.Empty
		Dim ssDoc As DAO.Snapshot
		
		Try 
			
			ssDoc = g_dbDDB.CreateSnapshot("SELECT doc_type FROM document WHERE doc_num = " & lDocNum.ToString())
			DAO_DBEngine_definst.FreeLocks()
			
			If ssDoc.RecordCount = 1 Then
                result = ssDoc("doc_type").Value.Trim()
			Else
				result = ""
			End If
			
			ssDoc.Close()
			Return result
		
		Catch 
			
			
			
			Return ""
		End Try
		
	End Function
	
	Function GetExistingLogin(ByRef sUsername As String) As Integer
		
		Dim result As Integer = 0
		Dim frmTmpLogin As frmExistingLogin
		Dim dsLogin As DAO.Dynaset
		Dim iLoginID As Integer
		
		Try 
			
			frmTmpLogin = New frmExistingLogin()
			
			frmTmpLogin.Tag = sUsername

			frmTmpLogin.ShowDialog()
			
			If CDbl(Convert.ToString(frmTmpLogin.Tag)) > 0 Then
				message(frmTmpLogin, "Please wait... Overriding existing logon")
				' Override existing logon
				' Add user to login table
				dsLogin = g_dbDDB.CreateDynaset("login")
				dsLogin.LockEdits = False
				dsLogin.AddNew()
				
                dsLogin("user_name").Value = sUsername
                dsLogin("device_num").Value = 0
                dsLogin("date").Value = DateTime.Today.AddDays(DateTimeHelper.Time.ToOADate())
				
				iLoginID = GetNextDDBNumber(LOGIN)
                dsLogin("login_num").Value = iLoginID
				
                If dsLogin("login_num").Value > 0 Then

                    g_iTmp = 0
                    g_iRC = UpdateDynaset(dsLogin)
                    Select Case g_iRC
                        Case PM_TRUE
                        Case PM_FALSE, PM_CANCEL
                            Interaction.MsgBox("Update Table failed. (ds)", MB_ICONEXCLAMATION, "GetExistingLogin")
                            result = False
                        Case Else
                            Interaction.MsgBox("Update Table failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "GetExistingLogin")
                            result = False
                    End Select
                    ' Remove old logon
                    g_iLoginID = CInt(Convert.ToString(frmTmpLogin.Tag))

                    If Not (RemoveUserLogin(g_iLoginID)) Then
                        ' Failed to remove login
                        message(frmTmpLogin, "")
                        result = 0
                    Else
                        result = iLoginID
                    End If
                Else
                    result = 0
                End If
				
				dsLogin.Close()
				dsLogin = Nothing
				
			End If
			
			message(frmTmpLogin, "")
			frmTmpLogin.Close()
			
			Return result
		
		Catch 
			
			
			
			message(frmTmpLogin, "")
			Interaction.MsgBox("Failed to override previous login" & Strings.Chr(10).ToString() & Conversion.ErrorToString(), MB_ICONEXCLAMATION, "Login Override Error")
			frmTmpLogin.Close()
			Return result
		End Try
		
	End Function
	
	Sub GetLicenceDetails(ByRef sCompanyName As String, ByRef sLicenceLimit As String, ByRef sProductCode As String)
		
		Dim ssSnapShot As DAO.Snapshot
		
		sCompanyName = ""
		sLicenceLimit = ""
		sProductCode = ""
		
		Try 
			
			ssSnapShot = g_dbDDB.CreateSnapshot("SELECT * FROM licence")
			DAO_DBEngine_definst.FreeLocks()
			
			If ssSnapShot.RecordCount > 0 Then
				sCompanyName = ssSnapShot("company_name")
				sLicenceLimit = ssSnapShot("licence_limit")
				sProductCode = ssSnapShot("product_code")
			End If
			
			ssSnapShot.Close()
		
		Catch 
			
			
			
			sCompanyName = ""
			sLicenceLimit = ""
			sProductCode = ""
			Exit Sub
		End Try
		
	End Sub
	
	Function GetNextDDBNumber(ByRef iLevel As Integer) As Integer
		
		Dim ssNextNumber As DAO.Snapshot
		Dim sSQLQuery As String = ""
		Dim iNextNumber As Integer
		
		Try 
			
			'salvo(120996) - Optimize the following sql queries on huge tables
			
			Select Case iLevel
				Case CABINET
					sSQLQuery = "SELECT cabinet_num FROM cabinet ORDER BY cabinet_num DESC"
				Case DRAWER
					'   sSQLQuery = "SELECT drawer_num FROM drawer ORDER BY drawer_num DESC"
					sSQLQuery = "SELECT MAX(drawer_num) FROM drawer"
				Case FOLDER
					'   sSQLQuery = "SELECT folder_num FROM folder ORDER BY folder_num DESC"
					sSQLQuery = "SELECT MAX(folder_num) FROM folder"
				Case DOCUMENT
					'   sSQLQuery = "SELECT doc_num FROM document ORDER BY doc_num DESC"
					sSQLQuery = "SELECT MAX(doc_num) FROM document"
				Case keywords
					sSQLQuery = "SELECT key_num FROM keywords ORDER BY key_num DESC"
				Case DEVICE
					sSQLQuery = "SELECT device_num FROM device ORDER BY device_num DESC"
				Case DEVICETYPE
					sSQLQuery = "SELECT dt_num FROM devicetype ORDER BY dt_num DESC"
				Case LOGIN
					sSQLQuery = "SELECT login_num FROM login ORDER BY login_num DESC"
				Case RECORDLOCK
					sSQLQuery = "SELECT lock_num FROM recordlock ORDER BY lock_num DESC"
			End Select
			
			ssNextNumber = g_dbDDB.CreateSnapshot(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			
			' Check if any records where found
			If ssNextNumber.RecordCount > 0 Then
                iNextNumber = (CInt(ssNextNumber(0).Value + 1))
			Else
				iNextNumber = 1
			End If
			
			ssNextNumber.Close()
			ssNextNumber = Nothing
			
			Return iNextNumber
		
		Catch 
			
			
			
			Return 1
		End Try
		
	End Function
	
	Function GetNextLoginID(ByRef sUsername As String) As Integer
		Dim ErrGetNextLoginID As Boolean = False
		Dim ErrLoginLimit As Boolean = False
		
		Dim result As Integer = 0
		Dim ssSnapShot As DAO.Snapshot
		Dim iLoginLimit, iUserLogins As Integer
		
		Try 
			ErrLoginLimit = True
			ErrGetNextLoginID = False
			
			' Get the user limit
			ssSnapShot = g_dbDDB.CreateSnapshot("SELECT login_limit FROM user WHERE user_name = '" & sUsername & "'")
			DAO_DBEngine_definst.FreeLocks()
			
			If ssSnapShot.RecordCount = 1 Then
                iLoginLimit = ssSnapShot("login_limit").Value
			Else
				iLoginLimit = 0
			End If
			
			' Make an exception for the DMS API's or Super User
			If sUsername = "DMSAPI" Or sUsername = "DMSWAPI" Or sUsername = "SU" Then
				iLoginLimit = 1
			End If
			
			ErrGetNextLoginID = True
			ErrLoginLimit = False
			
			'DMSWAPI must always be allowed to log in, it has no limit.
			If sUsername <> "DMSWAPI" Then
				iUserLogins = GetUserLogins(sUsername)
				
				If iUserLogins < iLoginLimit Then
					result = GetNextDDBNumber(LOGIN)
				Else
					result = 0
				End If
			Else
				result = GetNextDDBNumber(LOGIN)
			End If
			
			ssSnapShot.Close()
			ssSnapShot = Nothing
			
			Return result
		
		Catch excep As System.Exception
			If Not ErrGetNextLoginID And Not ErrLoginLimit Then
				Throw excep
			End If
			
			If ErrLoginLimit Then
				
				
				iLoginLimit = 0


				
			End If
			If ErrGetNextLoginID Or ErrLoginLimit Then
				
				
				Return 0
				
			End If
		End Try
	End Function
	
	Function GetNextPageName() As String
		
		Dim result As String = String.Empty
		Dim sFilename, sTmp As String
		Dim iDataLen, id, iB, iA, iC, iE As Integer
		Dim dsSystem As DAO.Dynaset
		
		
		Try 
			
			dsSystem = g_dbDDB.CreateDynaset("SELECT next_page FROM system")
			DAO_DBEngine_definst.FreeLocks()
			dsSystem.LockEdits = False
			
			If dsSystem.RecordCount <> 1 Then
				result = "ERROR"
				
				dsSystem.Close()
				dsSystem = Nothing
				
				Return result
			Else
				sFilename = dsSystem("next_page")
			End If
			
			If sFilename.Length <> 15 Then
				result = "ERROR"
				
				dsSystem.Close()
				dsSystem = Nothing
				
				Return result
			End If
			
			iA = Conversion.Val(sFilename.Substring(1, Math.Min(sFilename.Length, 2)))
			iB = Conversion.Val(sFilename.Substring(4, Math.Min(sFilename.Length, 2)))
			iC = Conversion.Val(sFilename.Substring(7, Math.Min(sFilename.Length, 2)))
			id = Conversion.Val(sFilename.Substring(10, Math.Min(sFilename.Length, 2)))
			iE = Conversion.Val(sFilename.Substring(13, Math.Min(sFilename.Length, 2)))
			
			iE += 1
			If iE > 99 Then
				iE = 0
				id += 1
				If id > 99 Then
					id = 0
					iC += 1
					If iC > 99 Then
						iC = 0
						iB += 1
						If iB > 99 Then
							iB = 0
							iA += 1
							If iA > 99 Then
								result = "ERROR"
								
								dsSystem.Close()
								dsSystem = Nothing
								Return result
							End If
						End If
					End If
				End If
			End If
			
			result = sFilename
			
			If iA < 10 Then
				Mid(sFilename, 2, 2) = "0" & iA.ToString()
			Else
				Mid(sFilename, 2, 2) = iA.ToString()
			End If
			
			If iB < 10 Then
				Mid(sFilename, 5, 2) = "0" & iB.ToString()
			Else
				Mid(sFilename, 5, 2) = iB.ToString()
			End If
			
			If iC < 10 Then
				Mid(sFilename, 8, 2) = "0" & iC.ToString()
			Else
				Mid(sFilename, 8, 2) = iC.ToString()
			End If
			
			If id < 10 Then
				Mid(sFilename, 11, 2) = "0" & id.ToString()
			Else
				Mid(sFilename, 11, 2) = id.ToString()
			End If
			
			If iE < 10 Then
				Mid(sFilename, 14, 2) = "0" & iE.ToString()
			Else
				Mid(sFilename, 14, 2) = iE.ToString()
			End If
			
			dsSystem.Edit()
            dsSystem("next_page").Value = sFilename
			
			g_iTmp = 0
			g_iRC = UpdateDynaset(dsSystem)
			Select Case g_iRC
				Case PM_TRUE
				Case PM_FALSE, PM_CANCEL
					Interaction.MsgBox("Update System Table failed. (ds)", MB_ICONEXCLAMATION, "GetNextPageName")
					result = ""
				Case Else
					Interaction.MsgBox("Update System Table failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "GetNextPageName")
					result = ""
			End Select
			
			dsSystem.Close()
			dsSystem = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return "ERROR"
		End Try
		
	End Function
	
	'
	'*********************************************************************
	'
	' Module Name: GetNextPageNumber
	'      Author: simonb
	'        Date: 16/05/95
	'
	' Description: Gets the next available unique page number to be used
	'              with the data functions.
	'
	'*********************************************************************
	'
	Function GetNextPageNumber(ByRef lDocumentNumber As Integer) As Integer
		Dim ssNextNumber As DAO.Snapshot
		Dim sSQLQuery As String = ""
		Dim iNextNumber As Integer
		
		Try 
			
			sSQLQuery = "SELECT page_num FROM page "
			sSQLQuery = sSQLQuery & "WHERE doc_num = " & CStr(lDocumentNumber)
			sSQLQuery = sSQLQuery & " ORDER BY page_num DESC"
			
			ssNextNumber = g_dbDDB.CreateSnapshot(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			
			
			' Check if any records where found
			If ssNextNumber.RecordCount > 0 Then
                iNextNumber = (CInt(ssNextNumber(0).Value + 1))
			Else
				iNextNumber = 1
			End If
			
			ssNextNumber.Close()
			ssNextNumber = Nothing
			
			Return iNextNumber
		
		Catch 
			
			
			
			Return 1
		End Try
		
	End Function
	
	Function GetPageScanDate(ByRef sPageName As String) As String
		
		Dim result As String = String.Empty
		Dim ssTmpSnapshot As DAO.Snapshot
		Dim sSQLQuery As String = ""
		
		' Strip page name
		Dim stmpPageName As String = sPageName.Substring(sPageName.Length - 19).Substring(0, 15)
		
		Try 
			
			sSQLQuery = "SELECT scan_date FROM page "
			sSQLQuery = sSQLQuery & "WHERE page_name = '" & stmpPageName & "'"
			
			ssTmpSnapshot = g_dbDDB.CreateSnapshot(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			
			If ssTmpSnapshot.RecordCount = 1 Then
				result = StringsHelper.Format(ssTmpSnapshot(0), "dd/mm/yy - hh:mm am/pm")
			Else
				result = ""
			End If
			
			ssTmpSnapshot.Close()
			Return result
		
		Catch 
			
			
			
			Return ""
		End Try
		
	End Function
	
	'RAM20030102 - Changed the GetParent Function name to GetParentNumber
	'               since, gPMFunction contains an API function, with same name
	Function GetParentNumber(ByRef iLevel As Integer, ByRef lNode As Integer) As Integer
		Dim result As Integer = 0
		Dim sSQL As String = ""
		Dim ssTmp As DAO.Snapshot
		
		Try 
			
			Select Case iLevel
				Case CABINET
					Return 0
				Case DRAWER
					sSQL = "SELECT cabinet_num FROM drawer WHERE drawer_num = " & lNode.ToString()
				Case FOLDER
					sSQL = "SELECT drawer_num FROM folder WHERE folder_num = " & lNode.ToString()
				Case DOCUMENT
					sSQL = "SELECT folder_num FROM document WHERE doc_num = " & lNode.ToString()
			End Select
			
			ssTmp = g_dbDDB.CreateSnapshot(sSQL)
			DAO_DBEngine_definst.FreeLocks()
			
			ssTmp.MoveLast()
			If ssTmp.RecordCount = 1 Then
                result = ssTmp(0).Value
			Else
				result = 0
			End If
			
			ssTmp.Close()
			Return result
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
	
	Function GetParentDetails(ByRef iLevel As Integer, ByRef lNode As Integer) As Integer
		
		Dim result As Integer = 0
		Dim sSQLQuery As String = ""
		Dim ssSnapShot As DAO.Snapshot
		
		result = True
		
		Try 
			
			' Select case
			
			ssSnapShot.Close()
			ssSnapShot = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: GetSELECTIVECabinet
	'      Author: simonb
	'        Date: 11/04/95
	'
	' Description: Gets all the cabinet data from the DDB, where the
	'              the data in between the sQStart and sQEnd strings.
	'
	'*********************************************************************
	'
	Function GetSELECTIVECabinet(ByRef sQStart As String, ByRef sQEnd As String, ByRef sBlockStartArray() As String, ByRef iBlockNumber As Integer) As Integer
		
		Return GetDDBData(CABINET, SELECTIVE, sQStart, sQEnd, 0, sBlockStartArray, iBlockNumber)
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: GetSELECTIVEDocument
	'      Author: simonb
	'        Date: 11/04/95
	'
	' Description: Gets all the document data from the DDB, where the
	'              the data in between the sQStart and sQEnd strings.
	'
	'*********************************************************************
	'
	Function GetSELECTIVEDocument(ByRef iSearchNumber As Integer, ByRef sQStart As String, ByRef sQEnd As String, ByRef sBlockStartArray() As String, ByRef iBlockNumber As Integer) As Integer
		
		Return GetDDBData(DOCUMENT, SELECTIVE, sQStart, sQEnd, iSearchNumber, sBlockStartArray, iBlockNumber)
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: GetSELECTIVEDrawer
	'      Author: simonb
	'        Date: 11/04/95
	'
	' Description: Gets all the drawer data from the DDB, where the
	'              the data in between the sQStart and sQEnd strings.
	'
	'*********************************************************************
	'
	Function GetSELECTIVEDrawer(ByRef iSearchNumber As Integer, ByRef sQStart As String, ByRef sQEnd As String, ByRef sBlockStartArray() As String, ByRef iBlockNumber As Integer) As Integer
		
		Return GetDDBData(DRAWER, SELECTIVE, sQStart, sQEnd, iSearchNumber, sBlockStartArray, iBlockNumber)
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: GetSELECTIVEFolder
	'      Author: simonb
	'        Date: 11/04/95
	'
	' Description: Gets all the folder data from the DDB, where the
	'              the data in between the sQStart and sQEnd strings.
	'
	'*********************************************************************
	'
	Function GetSELECTIVEFolder(ByRef iSearchNumber As Integer, ByRef sQStart As String, ByRef sQEnd As String, ByRef sBlockStartArray() As String, ByRef iBlockNumber As Integer) As Integer
		
		Return GetDDBData(FOLDER, SELECTIVE, sQStart, sQEnd, iSearchNumber, sBlockStartArray, iBlockNumber)
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: GetSELECTIVEPage
	'      Author: simonb
	'        Date: 11/04/95
	'
	' Description: Gets all the page data from the DDB, where the
	'              the data in between the sQStart and sQEnd strings.
	'
	'*********************************************************************
	'
	Function GetSELECTIVEPage(ByRef iSearchNumber As Integer, ByRef sQStart As String, ByRef sQEnd As String, ByRef sBlockStartArray() As String, ByRef iBlockNumber As Integer) As Integer
		
		Return GetDDBData(PAGE, SELECTIVE, sQStart, sQEnd, iSearchNumber, sBlockStartArray, iBlockNumber)
		
	End Function
	
	Function GetTotalLogins() As Integer
		
		Dim result As Integer = 0
		Dim ssSnapShot As DAO.Snapshot
		
		Try 
			
			ssSnapShot = g_dbDDB.CreateSnapshot("SELECT login_num FROM login WHERE (user_name <> 'DMSAPI' AND user_name <> 'SU')")
			DAO_DBEngine_definst.FreeLocks()
			
			ssSnapShot.MoveLast()
			result = ssSnapShot.RecordCount
			
			ssSnapShot.Close()
			Return result
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
	
	Function GetUserLogins(ByRef sUsername As String) As Integer
		
		Dim result As Integer = 0
		Dim ssLogins As DAO.Snapshot
		Dim sTmpUserName As New FixedLengthString(16)
		
		sTmpUserName.Value = sUsername
		
		Try 
			
			ssLogins = g_dbDDB.CreateSnapshot("SELECT COUNT(*) FROM login WHERE user_name = '" & sTmpUserName.Value & "'")
			DAO_DBEngine_definst.FreeLocks()
			
            result = ssLogins(0).Value
			
			ssLogins.Close()
			Return result
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
	
	Function IsRecordFree(ByRef iLevel As Integer, ByRef lNode As Integer) As Integer
		Dim result As Integer = 0
		Dim ssLock As DAO.Snapshot
		
		Try 
			
			ssLock = g_dbDDB.CreateSnapshot("SELECT lock_num FROM recordlock WHERE level = " & iLevel & " AND code = " & CStr(lNode))
			DAO_DBEngine_definst.FreeLocks()
			
			ssLock.MoveLast()
			result = ssLock.RecordCount
			
			ssLock.Close()
			Return result
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
	
	Function KeywrdExists(ByRef lDocumentNumber As Integer, ByRef KeywordNum As Integer) As Integer
		
		Dim result As Integer = 0
		Dim ssSnapShot As DAO.Snapshot
		
		Try 
			
			ssSnapShot = g_dbDDB.CreateSnapshot("SELECT key_num FROM keyword WHERE doc_num = " & lDocumentNumber & " AND key_num = " & CStr(KeywordNum))
			DAO_DBEngine_definst.FreeLocks()
			
			If ssSnapShot.RecordCount > 0 Then
				result = PM_TRUE
			Else
				result = PM_FALSE
			End If
			
			ssSnapShot.Close()
			ssSnapShot = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return PM_FALSE
		End Try
		
	End Function
	
	Function LinkedData(ByRef iLevel As Integer, ByRef lNumber As Integer) As Integer
		Dim result As Integer = 0
		Dim ssTmp As DAO.Snapshot
		
		Dim sSQL As String = "SELECT link FROM "
		
		
		Select Case iLevel
			Case CABINET
				sSQL = sSQL & "cabinet WHERE cabinet_num = " & lNumber.ToString()
			Case DRAWER
				sSQL = sSQL & "drawer WHERE drawer_num = " & lNumber.ToString()
			Case FOLDER
				sSQL = sSQL & "folder WHERE folder_num = " & lNumber.ToString()
			Case DOCUMENT
				sSQL = sSQL & "document WHERE doc_num = " & lNumber.ToString()
		End Select
		
		Try 
			
			ssTmp = g_dbDDB.CreateSnapshot(sSQL)
			DAO_DBEngine_definst.FreeLocks()
			
			If ssTmp.RecordCount = 1 Then
                If ssTmp("link").Value <> 0 Then
                    result = ssTmp("link").Value
                Else
                    result = lNumber
                End If
			Else
				result = 0
			End If
			
			ssTmp.Close()
			Return result
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
	
	Function LinkOwner(ByRef iLevel As Integer, ByRef lNumber As Integer) As Integer
		Dim result As Integer = 0
		Dim ssLinks As DAO.Snapshot
		Dim sSQL As String = ""
		
		Try 
			sSQL = "SELECT link FROM "
			
			Select Case iLevel
				Case CABINET
					sSQL = sSQL & "cabinet "
				Case DRAWER
					sSQL = sSQL & "drawer "
				Case FOLDER
					sSQL = sSQL & "folder "
				Case DOCUMENT
					sSQL = sSQL & "document "
				Case Else
					Return 0
			End Select
			
			sSQL = sSQL & "WHERE link = " & lNumber.ToString()
			
			ssLinks = g_dbDDB.CreateSnapshot(sSQL)
			DAO_DBEngine_definst.FreeLocks()
			
			ssLinks.MoveLast()
			result = ssLinks.RecordCount
			
			ssLinks.Close()
			ssLinks = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: OpenDDB
	'      Author: simonb
	'        Date: 11/04/95
	'
	' Description: Opens the Document DataBase.
	'
	'*********************************************************************
	'
	Function OpenDDB() As Boolean
		Dim result As Boolean = False
		Dim iDataLen As Integer
		
		Dim sDDB, sMsg, sIni, sOpen As String
		Dim iPointerIN As Integer
		Dim sTmp As String = ""
		
		
		Try 
			
			result = True
			
			' Set Mouse Pointer To HourClass
            'iPointerIN = Cursor.Current
			Cursor.Current = Cursors.WaitCursor
			
			
			If LoginAllowed() = PM_FALSE Then
				Interaction.MsgBox("Logins have been disabled by the system administrator", MB_ICONEXCLAMATION, "DocuMaster Login")
				Environment.Exit(0)
			End If
			
			If GetIniFileVar("Paths", "dbRoot", g_sDBRoot, True) = PM_FALSE Then
				Return False
			End If
			
			'Salvo remove this stuff and replace with hard coding the DB to be access, as no point releaseing
			' this till we have ODBC (ie V2)
			'If GetIniFileVar("Paths", "dbType", sTmp$, True) = PM_FALSE Then
			'    OpenDDB = False
			'    Exit Function
			'End If
			
			'g_iDBType = CInt(sTmp$)
			g_iDBType = DB_ACCESS ' inserted by me
			'Salvo remove this stuff and replace with hard coding the DB to be access, as no point releaseing
			' this till we have ODBC (ie V2)
			
			
			Select Case g_iDBType
				Case DB_ACCESS
					'Access Database
					
					If GetIniFileVar("Paths", "dbName", g_sDBName, True) = PM_FALSE Then
						Return False
					End If
					
					sDDB = g_sDBRoot & "\" & g_sDBName

					g_dbDDB = DBEngineHelper.Instance(Artinsoft.VB6.DB.AdoFactoryManager.GetFactory()).OpenDatabase(sDDB)
					
				Case DB_SQLSERVER, DB_ODBC
					'ODBC or MS SQL Server Database
					

					g_dbDDB = DBEngineHelper.Instance(Artinsoft.VB6.DB.AdoFactoryManager.GetFactory()).OpenDatabase("")
					
				Case Else
					
					Interaction.MsgBox("Unrecognised database type", MB_ICONSTOP, "Database Connect Error")
					Environment.Exit(0)
					
			End Select
			
			Select Case dbVersion(g_dbDDB)
				Case DDB_VERSION
					'OK...
				Case Is < DDB_VERSION
					Interaction.MsgBox("The DocuMaster Database needs updating. Advise your DocuMaster administrator", MB_ICONSTOP, "DDB Database Version Error")
					Return False
				Case Is > DDB_VERSION
					Interaction.MsgBox("This DocuMaster Application needs updating. Advise your DocuMaster administrator", MB_ICONSTOP, "Application Version Error")
					Return False
			End Select
			
			' Check if user is able to login
			If Not (CheckLogin()) Then
				Interaction.MsgBox("System is currently open with exclusive access by the system administrator", MB_ICONEXCLAMATION, "DDB Database Open Error")
				result = False

                'Cursor.Current = iPointerIN
				Return result
			End If
			
			' Get the system administrator level
			g_iAdministrator = GetAdminLevel()
			

            'Cursor.Current = iPointerIN
			Return result
		
		Catch 
			
			
			
			Select Case (Information.Err().Number)
				Case 3000
					sMsg = "Database is currently open with exclusive access"
					Interaction.MsgBox(sMsg, MB_ICONSTOP, "DDB Database Open Error")
					
				Case 3024 'can not find database
					' If (g_sAppType <> "SCAN") Then
					' if application is not the ScanStation, then
					' display this message
					sMsg = "Can not open database " & g_sDBRoot & "\" & g_sDBName.ToUpper() & Strings.Chr(10).ToString() & "Check path is available or Database name is correct in pmdms.ini"
					Interaction.MsgBox(sMsg, MB_ICONSTOP, "DDB Database Open Error")
					' End If
				Case 3049
					sMsg = "Database is damaged. Contact your DocuMaster System Administrator"
					Interaction.MsgBox(sMsg, MB_ICONSTOP, "DDB Database Open Error")
					
				Case Else
					sMsg = "Error " & Information.Err().Number & ": " & Conversion.ErrorToString()
					Interaction.MsgBox(sMsg, MB_ICONSTOP, "DDB Database Open Error")
			End Select
			
			result = False
			
			' Set Mouse Pointer To DEFAULT

            'Cursor.Current = iPointerIN
			
			Return result
			
			
			
			result = False
			Interaction.MsgBox("ERROR: DataBase repair failed. Log ALL users off and try again", MB_ICONSTOP)
			Return result
		End Try
		
	End Function
	
	Function protectDDB(ByRef iLevel As Integer, ByRef lLevelNumber As Integer, ByRef sPassword As String) As Integer
		Dim result As Integer = 0
		Dim dsTmpDynaset As DAO.Dynaset
		Dim sSQLQuery As String = ""
		Dim iLockCount As Integer
		Dim lLockNum As Integer
		
		Try 
			result = True
			
			Select Case iLevel
				Case CABINET
					' Delete Cabinet
					sSQLQuery = "SELECT * FROM cabinet WHERE cabinet_num = " & lLevelNumber & " AND "
					sSQLQuery = sSQLQuery & "access_level >= " & CStr(g_iUserAccessLevel)
					dsTmpDynaset = g_dbDDB.CreateDynaset(sSQLQuery)
					DAO_DBEngine_definst.FreeLocks()
					dsTmpDynaset.LockEdits = False
					
				Case DRAWER
					' Delete Drawer
					sSQLQuery = "SELECT * FROM drawer WHERE drawer_num = " & lLevelNumber & " AND "
					sSQLQuery = sSQLQuery & "access_level >= " & CStr(g_iUserAccessLevel)
					dsTmpDynaset = g_dbDDB.CreateDynaset(sSQLQuery)
					DAO_DBEngine_definst.FreeLocks()
					dsTmpDynaset.LockEdits = False
					
				Case FOLDER
					' Delete folder
					sSQLQuery = "SELECT * FROM folder WHERE folder_num = " & lLevelNumber & " AND "
					sSQLQuery = sSQLQuery & "access_level >= " & CStr(g_iUserAccessLevel)
					dsTmpDynaset = g_dbDDB.CreateDynaset(sSQLQuery)
					DAO_DBEngine_definst.FreeLocks()
					dsTmpDynaset.LockEdits = False
					
				Case DOCUMENT
					' Delete Document
					sSQLQuery = "SELECT * FROM document WHERE doc_num = " & lLevelNumber & " AND "
					sSQLQuery = sSQLQuery & "access_level >= " & CStr(g_iUserAccessLevel)
					dsTmpDynaset = g_dbDDB.CreateDynaset(sSQLQuery)
					DAO_DBEngine_definst.FreeLocks()
					dsTmpDynaset.LockEdits = False
					
			End Select
			
			If dsTmpDynaset.RecordCount = 1 Then
				dsTmpDynaset.Edit()
                dsTmpDynaset("password").Value = Encryption(sPassword)
				
                If dsTmpDynaset("password").Value.Trim().ToUpper() = "ERROR" Then
                    dsTmpDynaset("password").Value = ""
                End If
				
				g_iTmp = 0
				g_iRC = UpdateDynaset(dsTmpDynaset)
				Select Case g_iRC
					Case PM_TRUE
						result = True
					Case PM_FALSE, PM_CANCEL
						Interaction.MsgBox("Update failed. (ds)", MB_ICONEXCLAMATION, "ProtectDDB")
						result = False
					Case Else
						Interaction.MsgBox("Update failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "ProtectDDB")
						result = False
				End Select
				
			End If
			
			dsTmpDynaset.Close()
			dsTmpDynaset = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function RemoveRecordLock(ByRef lLockNumber As Integer) As Integer
		
		Dim result As Integer = 0
		Dim dsDynaset As DAO.Dynaset
		
		result = True
		
		Try 
			
			dsDynaset = g_dbDDB.CreateDynaset("SELECT * FROM recordlock WHERE lock_num = " & lLockNumber)
			DAO_DBEngine_definst.FreeLocks()
			dsDynaset.LockEdits = False
			
			If dsDynaset.RecordCount = 0 Then
				result = False
			Else
				g_iTmp = 0
				g_iRC = DeleteDynaset(dsDynaset)
				Select Case g_iRC
					Case PM_TRUE
					Case PM_FALSE
						Interaction.MsgBox("Delete Failed. (ds)", MB_ICONEXCLAMATION, "RemoveRecordLock")
					Case PM_CANCEL
						Interaction.MsgBox("Delete Cancelled. (ds)", MB_ICONEXCLAMATION, "RemoveRecordLock")
					Case Else
						Interaction.MsgBox("Delete Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "RemoveRecordLock")
				End Select
			End If
			
			dsDynaset.Close()
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function RemoveUserLogin(ByRef iLoginID As Integer) As Integer
		
		Dim result As Integer = 0
		Dim dsUserLogin As DAO.Dynaset
		Dim sSQLQuery As String = ""
		
		result = True
		
		Try 
			
			sSQLQuery = "SELECT * FROM login WHERE login_num = " & iLoginID
			
			dsUserLogin = g_dbDDB.CreateDynaset(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			dsUserLogin.LockEdits = False
			
			If dsUserLogin.RecordCount = 0 Then
				dsUserLogin.Close()
				dsUserLogin = Nothing
				Return True
			End If
			
			DAO_DBEngine_definst.BeginTrans()
			
			g_iTmp = 0
			g_iRC = DeleteDynaset(dsUserLogin)
			Select Case g_iRC
				Case PM_TRUE
					result = True
				Case PM_FALSE, PM_CANCEL
					Interaction.MsgBox("Delete failed. (ds)", MB_ICONEXCLAMATION, "RemoveUserLogin")
					result = False
				Case Else
					Interaction.MsgBox("Delete failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "RemoveUserLogin")
					result = False
			End Select
			
			Select Case CommitDatabase()
				Case PM_TRUE
					result = True
				Case Else
					DAO_DBEngine_definst.Rollback()
					result = False
			End Select
			
			dsUserLogin.Close()
			dsUserLogin = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function RenameDDB(ByRef iLevel As Integer, ByRef lLevelNumber As Integer, ByRef sNewName As String) As Integer
		Dim result As Integer = 0
		Dim dsTmpDynaset As DAO.Dynaset
		Dim sSQLQuery As String = ""
		Dim lLockNum As Integer
		Dim iLockCount As Integer
		
		Try 
			g_iRC = PM_FALSE
			
			Select Case iLevel
				Case CABINET
					' Delete Cabinet
					sSQLQuery = "SELECT * FROM cabinet WHERE cabinet_num = " & lLevelNumber & " AND "
					sSQLQuery = sSQLQuery & "access_level >= " & CStr(g_iUserAccessLevel)
					
					dsTmpDynaset = g_dbDDB.CreateDynaset(sSQLQuery)
					DAO_DBEngine_definst.FreeLocks()
					dsTmpDynaset.LockEdits = False
					
					If dsTmpDynaset.RecordCount = 1 Then
						dsTmpDynaset.Edit()
                        dsTmpDynaset("cabinet_name").Value = sNewName
						
						g_iTmp = 0
						g_iRC = UpdateDynaset(dsTmpDynaset)
						Select Case g_iRC
							Case PM_TRUE
							Case PM_FALSE
								Interaction.MsgBox("Update Failed. (ds)", MB_ICONEXCLAMATION, "RenameDDB")
							Case PM_CANCEL
								Interaction.MsgBox("Update Cancelled. (ds)", MB_ICONEXCLAMATION, "RenameDDB")
							Case Else
								Interaction.MsgBox("Update Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "RenameDDB")
						End Select
					End If
					
				Case DRAWER
					' Delete Drawer
					sSQLQuery = "SELECT * FROM drawer WHERE drawer_num = " & lLevelNumber & " AND "
					sSQLQuery = sSQLQuery & "access_level >= " & CStr(g_iUserAccessLevel)
					
					dsTmpDynaset = g_dbDDB.CreateDynaset(sSQLQuery)
					DAO_DBEngine_definst.FreeLocks()
					dsTmpDynaset.LockEdits = False
					
					If dsTmpDynaset.RecordCount = 1 Then
						dsTmpDynaset.Edit()
                        dsTmpDynaset("drawer_name").Value = sNewName
						
						g_iTmp = 0
						g_iRC = UpdateDynaset(dsTmpDynaset)
						Select Case g_iRC
							Case PM_TRUE
								result = PM_TRUE
							Case PM_FALSE
								Interaction.MsgBox("Update Failed. (ds)", MB_ICONEXCLAMATION, "RenameDDB")
							Case PM_CANCEL
								Interaction.MsgBox("Update Cancelled. (ds)", MB_ICONEXCLAMATION, "RenameDDB")
							Case Else
								Interaction.MsgBox("Update Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "RenameDDB")
						End Select
					End If
					
				Case FOLDER
					' Delete folder
					sSQLQuery = "SELECT * FROM folder WHERE folder_num = " & lLevelNumber & " AND "
					sSQLQuery = sSQLQuery & "access_level >= " & CStr(g_iUserAccessLevel)
					
					dsTmpDynaset = g_dbDDB.CreateDynaset(sSQLQuery)
					DAO_DBEngine_definst.FreeLocks()
					dsTmpDynaset.LockEdits = False
					
					If dsTmpDynaset.RecordCount = 1 Then
						dsTmpDynaset.Edit()
                        dsTmpDynaset("folder_name").Value = sNewName
						
						g_iTmp = 0
						g_iRC = UpdateDynaset(dsTmpDynaset)
						Select Case g_iRC
							Case PM_TRUE
							Case PM_FALSE
								Interaction.MsgBox("Update Failed. (ds)", MB_ICONEXCLAMATION, "RenameDDB")
							Case PM_CANCEL
								Interaction.MsgBox("Update Cancelled. (ds)", MB_ICONEXCLAMATION, "RenameDDB")
							Case Else
								Interaction.MsgBox("Update Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "RenameDDB")
						End Select
					End If
					
				Case DOCUMENT
					' Delete Document
					sSQLQuery = "SELECT * FROM document WHERE doc_num = " & lLevelNumber & " AND "
					sSQLQuery = sSQLQuery & "access_level >= " & CStr(g_iUserAccessLevel)
					
					dsTmpDynaset = g_dbDDB.CreateDynaset(sSQLQuery)
					DAO_DBEngine_definst.FreeLocks()
					dsTmpDynaset.LockEdits = False
					
					If dsTmpDynaset.RecordCount = 1 Then
						dsTmpDynaset.Edit()
                        dsTmpDynaset("doc_name").Value = sNewName
						
						g_iTmp = 0
						g_iRC = UpdateDynaset(dsTmpDynaset)
						Select Case g_iRC
							Case PM_TRUE
							Case PM_FALSE
								Interaction.MsgBox("Update Failed. (ds)", MB_ICONEXCLAMATION, "RenameDDB")
							Case PM_CANCEL
								Interaction.MsgBox("Update Cancelled. (ds)", MB_ICONEXCLAMATION, "RenameDDB")
							Case PM_DATACHANGED
								Interaction.MsgBox("Details have been changed by another user", MB_ICONEXCLAMATION, "RenameDDB")
							Case Else
								Interaction.MsgBox("Update Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "RenameDDB")
						End Select
						
					End If
					
			End Select
			
			result = g_iRC
			
			dsTmpDynaset.Close()
			dsTmpDynaset = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function RepairADatabase(ByRef sDBName As String) As Integer
		
		Dim result As Integer = 0
		Try 
			
			DAO_DBEngine_definst.RepairDatabase(sDBName)
			
			
			If Not OpenDDB() Then
				Return False
			Else
				Return True
			End If
		
		Catch 
		End Try
		
		
		
		Select Case Information.Err().Number
			Case -8194
				result = False
				Interaction.MsgBox("ERROR: DataBase repair failed. Log ALL users of and try again", MB_ICONSTOP, "RepairADatabase")
			Case Else
				result = False
				Interaction.MsgBox("ERROR: Repair database failed - " & Information.Err().Number & Strings.Chr(10).ToString() & "Log ALL users of and try again", MB_ICONSTOP, "RepairADatabase")
		End Select
		
		Environment.Exit(0)
		Return result
		
	End Function
	
	Function RollBackAll() As Integer
		
		Try 
			
			DAO_DBEngine_definst.Rollback()
			Return True
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: SaveAnnotationsData
	'      Author: simonb
	'        Date: 05/06/95
	'
	' Description: Saves the record structure into the annotations DDB.
	'
	'*********************************************************************
	'
	Function SaveAnnotationsData(ByRef utStruct As g_utAnnotations) As Integer
		
		Dim result As Integer = 0
		Dim tbAnnotationsData As DAO.Table
		
		result = True
		
		Try 
			
			tbAnnotationsData = g_dbDDB.OpenTable("annotation")
			tbAnnotationsData.AddNew()
			
            tbAnnotationsData("doc_num").Value = utStruct.doc_num
            tbAnnotationsData("ann_text").Value = utStruct.ann_text.Value
            tbAnnotationsData("user_name").Value = utStruct.user_name.Value
            tbAnnotationsData("create_date").Value = utStruct.create_date
			
			g_iTmp = 0
			g_iRC = UpdateTable(tbAnnotationsData)
			Select Case g_iRC
				Case PM_TRUE
				Case PM_FALSE, PM_CANCEL
					Interaction.MsgBox("Update Table failed. (ds)", MB_ICONEXCLAMATION, "SaveAnnotationsData")
					result = False
				Case Else
					Interaction.MsgBox("Update Table failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "SaveAnnotationsData")
					result = False
			End Select
			
			tbAnnotationsData.Close()
			tbAnnotationsData = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: SaveCabinetData
	'      Author: simonb
	'        Date: 12/04/95
	'
	' Description: Saves the record structure into the cabinet DDB.
	'
	'*********************************************************************
	'
	Function SaveCabinetData(ByRef utStruct As g_utCabinet) As Integer
		Dim result As Integer = 0
		Dim tbCabinetData As DAO.Table
		Dim iTmpNextNumber As Integer
		
		Try 
			
			tbCabinetData = g_dbDDB.OpenTable("cabinet")
			tbCabinetData.LockEdits = False
			tbCabinetData.AddNew()
			
            tbCabinetData("cabinet_name").Value = utStruct.cabinet_name.Value
            tbCabinetData("list_src").Value = utStruct.list_src.Value
            tbCabinetData("access_level").Value = utStruct.access_level
            tbCabinetData("password").Value = utStruct.password.Value
            tbCabinetData("ex_code").Value = utStruct.ex_code.Value
            tbCabinetData("link").Value = utStruct.link
			
			For iTmp As Integer = 0 To UPDATERETRY
				' Get the next ID number
                tbCabinetData("cabinet_num").Value = GetNextDDBNumber(CABINET)
				
                result = tbCabinetData("cabinet_num").Value
				
                If tbCabinetData("cabinet_num").Value > 0 Then

                    g_iTmp = 0
                    g_iRC = UpdateTable(tbCabinetData)
                    Select Case g_iRC
                        Case PM_TRUE
                            Exit For
                        Case PM_DUPLICATEKEY
                        Case PM_FALSE
                            Interaction.MsgBox("Save Failed (tbl) ", MB_ICONEXCLAMATION, "SaveCabinetData")
                            result = 0
                            Exit For
                        Case Else
                            Interaction.MsgBox("Save Failed (tbl) - " & g_iRC, MB_ICONEXCLAMATION, "SaveCabinetData")
                            result = 0
                            Exit For
                    End Select
                End If
			Next iTmp
			
			tbCabinetData.Close()
			tbCabinetData = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return PM_ERROR
		End Try
		
	End Function
	
	Function SaveDeviceData(ByRef utStruct As g_utDevice) As Integer
		Dim result As Integer = 0
		Dim dsDTData As DAO.Dynaset
		
		Try 
			
			dsDTData = g_dbDDB.CreateDynaset("device")
			dsDTData.AddNew()
			
            dsDTData("dt_num").Value = utStruct.dt_num
            dsDTData("server_name").Value = utStruct.server_name.Value
            dsDTData("server_root").Value = utStruct.server_root.Value
            dsDTData("mount_volume").Value = utStruct.mount_volume.Value
            dsDTData("location").Value = utStruct.location.Value
			
			For iTmp As Integer = 0 To UPDATERETRY
				' Get the next ID number
                dsDTData("device_num").Value = GetNextDDBNumber(DEVICE) 'utStruct.device_num
				
                result = dsDTData("device_num").Value
				
                If dsDTData("device_num").Value > 0 Then

                    g_iTmp = 0
                    g_iRC = UpdateDynaset(dsDTData)
                    Select Case g_iRC
                        Case PM_TRUE
                            Exit For
                        Case PM_DUPLICATEKEY
                        Case PM_FALSE
                            Interaction.MsgBox("Save Failed (ds)", MB_ICONEXCLAMATION, "SaveDeviceData")
                            result = 0
                            Exit For
                        Case Else
                            Interaction.MsgBox("Save Failed (ds) - " & g_iRC, MB_ICONEXCLAMATION, "SaveDeviceData")
                            result = 0
                            Exit For
                    End Select
                Else
                    Pause(1)
                End If
			Next iTmp
			
			dsDTData.Close()
			dsDTData = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
		
	End Function
	
	Function SaveDeviceTypeData(ByRef utStruct As g_utDeviceType) As Integer
		Dim result As Integer = 0
		Dim dsDTData As DAO.Dynaset
		
		Try 
			
			dsDTData = g_dbDDB.CreateDynaset("devicetype")
			dsDTData.AddNew()
			
            dsDTData("device_type").Value = utStruct.device_type.Value
            dsDTData("fixed").Value = utStruct.fixed
            dsDTData("rewriteable").Value = utStruct.rewriteable
            dsDTData("line_type").Value = utStruct.line_type
			
			For iTmp As Integer = 0 To UPDATERETRY
				' Get the next ID number
                dsDTData("dt_num").Value = GetNextDDBNumber(DEVICETYPE)
				
                result = dsDTData("dt_num").Value
				
                If dsDTData("device_num").Value > 0 Then

                    g_iTmp = 0
                    g_iRC = UpdateDynaset(dsDTData)
                    Select Case g_iRC
                        Case PM_TRUE
                            Exit For
                        Case PM_DUPLICATEKEY
                        Case PM_FALSE
                            Interaction.MsgBox("Save Failed (ds)", MB_ICONEXCLAMATION, "SaveDeviceTypeData")
                            result = 0
                            Exit For
                        Case Else
                            Interaction.MsgBox("Save Failed (ds) - " & g_iRC, MB_ICONEXCLAMATION, "SaveDeviceTypeData")
                            result = 0
                            Exit For
                    End Select
                Else
                    Pause(1)
                End If
			Next iTmp
			
			
			dsDTData.Close()
			dsDTData = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: SaveDocInfoData
	'      Author: simonb
	'        Date: 12/04/95
	'
	' Description: Saves the record structure into the docinfo DDB.
	'
	'*********************************************************************
	'
	Function SaveDocInfoData(ByRef utStruct As g_utDocInfo) As Integer
		
		Dim result As Integer = 0
		Dim tbDocInfoData As DAO.Table
		Dim iTmp As Integer
		
		Try 
			
			tbDocInfoData = g_dbDDB.OpenTable("docinfo")
			tbDocInfoData.LockEdits = False
			tbDocInfoData.AddNew()
			
            tbDocInfoData("doc_num").Value = utStruct.doc_num
            tbDocInfoData("expiry_date").Value = utStruct.expiry_date
            tbDocInfoData("scan_operator").Value = utStruct.scan_operator.Value
            tbDocInfoData("doc_date").Value = utStruct.doc_date
            tbDocInfoData("last_user").Value = utStruct.last_user.Value
            tbDocInfoData("last_date").Value = utStruct.last_date
			
			g_iTmp = 0
			g_iRC = UpdateTable(tbDocInfoData)
			Dim dsInfo As DAO.Dynaset
			Select Case g_iRC
				Case PM_TRUE
					result = True
				Case PM_DUPLICATEKEY
					
					
					dsInfo = g_dbDDB.CreateDynaset("Select * FROM docinfo WHERE doc_num = " & utStruct.doc_num)
					dsInfo.LockEdits = False
					dsInfo.Edit()
					
					If dsInfo.RecordCount = 1 Then
                        dsInfo("expiry_date").Value = utStruct.expiry_date
                        dsInfo("scan_operator").Value = utStruct.scan_operator.Value
                        dsInfo("doc_date").Value = utStruct.doc_date
                        dsInfo("last_user").Value = utStruct.last_user.Value
                        dsInfo("last_date").Value = utStruct.last_date
						
						g_iTmp = 0
						g_iRC = UpdateDynaset(dsInfo)
						Select Case g_iRC
							Case PM_TRUE
								result = True
							Case PM_DUPLICATEKEY, PM_FALSE
								Interaction.MsgBox("Save Failed (tbl)", MB_ICONEXCLAMATION, "SaveDocInfoData")
								result = False
							Case Else
								Interaction.MsgBox("Save Failed (tbl) - " & g_iRC, MB_ICONEXCLAMATION, "SaveDocInfoData")
								result = False
						End Select
					End If
					
					dsInfo.Close()
					dsInfo = Nothing
					
				Case PM_FALSE
					Interaction.MsgBox("Save Failed (tbl)", MB_ICONEXCLAMATION, "SaveDocInfoData")
					result = False
				Case Else
					Interaction.MsgBox("Save Failed (tbl) - " & g_iRC, MB_ICONEXCLAMATION, "SaveDocInfoData")
					result = False
			End Select
			
			tbDocInfoData.Close()
			tbDocInfoData = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function SaveDocumentData(ByRef utStruct As g_utDocument) As Integer
		
		Dim result As Integer = 0
		Dim tbDocumentData As DAO.Table
		Dim iTmpNextNumber As Integer
		
		Try 
			
			g_iRC = 0
			
			tbDocumentData = g_dbDDB.OpenTable("document")
			tbDocumentData.LockEdits = False
			tbDocumentData.AddNew()
			
            tbDocumentData("doc_name").Value = utStruct.doc_name.Value
            tbDocumentData("folder_num").Value = utStruct.folder_num
            tbDocumentData("access_level").Value = utStruct.access_level
            tbDocumentData("password").Value = utStruct.password.Value
            tbDocumentData("docset_num").Value = utStruct.docset_num
            tbDocumentData("create_date").Value = DateTime.Today.AddDays(DateTimeHelper.Time.ToOADate())
            tbDocumentData("ex_code").Value = utStruct.ex_code.Value
            tbDocumentData("link").Value = utStruct.link
            tbDocumentData("doc_type").Value = utStruct.doc_type.Value
			
			
			For iTmp As Integer = 0 To UPDATERETRY
				' Get the next ID number
                tbDocumentData("doc_num").Value = GetNextDDBNumber(DOCUMENT)
                result = tbDocumentData("doc_num").Value
				
                If tbDocumentData("doc_num").Value > 0 Then
                    g_iTmp = 0
                    g_iRC = UpdateTable(tbDocumentData)
                    Select Case g_iRC
                        Case PM_TRUE
                            Exit For
                        Case PM_DUPLICATEKEY
                            ' salvo(260996) - Deal with duplicate key
                            If iTmp = UPDATERETRY Then
                                Interaction.MsgBox("Save Failed - Duplicate Key (tbl) - " & g_iRC, MB_ICONEXCLAMATION, "SaveDocumentData")
                                result = 0
                            End If
                            Pause(iTmp)
                        Case PM_FALSE
                            Interaction.MsgBox("Save Failed (tbl) - " & g_iRC, MB_ICONEXCLAMATION, "SaveDocumentData")
                            result = 0
                            Exit For
                        Case Else
                            Interaction.MsgBox("Save Failed (tbl) - " & g_iRC, MB_ICONEXCLAMATION, "SaveDocumentData")
                            result = 0
                            Exit For
                    End Select
                End If
				
				Pause(1)
				
			Next iTmp
			
			tbDocumentData.Close()
			tbDocumentData = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
	
	Function SaveDocumentHistory(ByRef lDocumentNumber As Integer) As Integer
		
		Dim result As Integer = 0
		Dim dsDocHistory As DAO.Dynaset
		Dim tbDocHistory As DAO.Table
		Dim sSQLQuery As String = ""
		Dim lLockNum As Integer
		Dim iLockCount, iTmp As Integer
		Dim dHistDate As Double
		
		result = True
		g_iRC = 0
		
		Try 
			
			' Check if more than ten records
			sSQLQuery = "SELECT * FROM dochistory WHERE doc_num = " & lDocumentNumber & " "
			sSQLQuery = sSQLQuery & "ORDER BY date DESC"
			
			dsDocHistory = g_dbDDB.CreateDynaset(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			dsDocHistory.LockEdits = False
			
			dHistDate = DateTime.Today.AddDays(DateTimeHelper.Time.ToOADate()).ToOADate
			
			If dsDocHistory.RecordCount > MAXDOCHISTORY Then
				' Reached maximum records, update oldest record
				dsDocHistory.MoveLast()
				dsDocHistory.Edit()
				
                dsDocHistory("doc_num").Value = lDocumentNumber
                dsDocHistory("user_name").Value = g_sUsername
                dsDocHistory("date").Value = dHistDate
				
				g_iTmp = 0
				g_iRC = UpdateDynaset(dsDocHistory)
				Select Case g_iRC
					Case PM_TRUE
						result = True
					Case PM_FALSE, PM_CANCEL
						Interaction.MsgBox("Update failed. (ds)", MB_ICONEXCLAMATION, "SaveDocumentHistory")
						result = False
					Case Else
						Interaction.MsgBox("Update failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "SaveDocumentHistory")
						result = False
				End Select
				
				dsDocHistory.Close()
				dsDocHistory = Nothing
			Else
				' Add record
				dsDocHistory.Close()
				dsDocHistory = Nothing
				
				tbDocHistory = g_dbDDB.OpenTable("dochistory")
				tbDocHistory.AddNew()
				
                tbDocHistory("doc_num").Value = lDocumentNumber
                tbDocHistory("user_name").Value = g_sUsername
                tbDocHistory("date").Value = dHistDate
				
				g_iTmp = 0
				g_iRC = UpdateTable(tbDocHistory)
				Select Case g_iRC
					Case PM_TRUE
						result = True
					Case PM_FALSE, PM_CANCEL
						Interaction.MsgBox("Update failed. (tbl)", MB_ICONEXCLAMATION, "SaveDocumentHistory")
						result = False
					Case Else
						Interaction.MsgBox("Update failed. (tbl) - " & g_iRC, MB_ICONEXCLAMATION, "SaveDocumentHistory")
						result = False
				End Select
				
				tbDocHistory.Close()
				dsDocHistory = Nothing
				
			End If
			
			If g_iRC = PM_TRUE Then
				sSQLQuery = "SELECT last_user, last_date FROM docinfo "
				sSQLQuery = sSQLQuery & "WHERE docinfo.doc_num = " & CStr(lDocumentNumber)
				
				dsDocHistory = g_dbDDB.CreateDynaset(sSQLQuery)
				DAO_DBEngine_definst.FreeLocks()
				dsDocHistory.LockEdits = False
				
				If dsDocHistory.RecordCount > 0 Then
					dsDocHistory.Edit()
                    dsDocHistory("last_user").Value = g_sUsername
                    dsDocHistory("last_date").Value = dHistDate
					
					g_iTmp = 0
					g_iRC = UpdateDynaset(dsDocHistory)
					Select Case g_iRC
						Case PM_TRUE
							result = True
						Case PM_FALSE, PM_CANCEL
							Interaction.MsgBox("Update failed. (ds)", MB_ICONEXCLAMATION, "SaveDocumentHistory, docinfo")
							result = False
						Case Else
							Interaction.MsgBox("Update failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "SaveDocumentHistory, docinfo")
							result = False
					End Select
					
				End If
				
				dsDocHistory.Close()
				dsDocHistory = Nothing
			End If
			
			Return g_iRC
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: SaveDrawerData
	'      Author: simonb
	'        Date: 12/04/95
	'
	' Description: Saves the record structure into the drawer DDB.
	'
	'*********************************************************************
	'
	Function SaveDrawerData(ByRef utStruct As g_utDrawer) As Integer
		Dim result As Integer = 0
		Dim tbDrawerData As DAO.Table
		Dim iTmpNextNumber As Integer
		
		Try 
			g_iRC = 0
			
			tbDrawerData = g_dbDDB.OpenTable("drawer")
			tbDrawerData.LockEdits = False
			tbDrawerData.AddNew()
			
            tbDrawerData("drawer_name").Value = utStruct.drawer_name.Value
            tbDrawerData("ex_code").Value = utStruct.ex_code.Value
            tbDrawerData("cabinet_num").Value = utStruct.cabinet_num
            tbDrawerData("access_level").Value = utStruct.access_level
            tbDrawerData("password").Value = utStruct.password.Value
            tbDrawerData("link").Value = utStruct.link
			
			For iTmp As Integer = 0 To UPDATERETRY
                tbDrawerData("drawer_num").Value = GetNextDDBNumber(DRAWER)
				
                result = tbDrawerData("drawer_num").Value
				
                If tbDrawerData("drawer_num").Value > 0 Then
                    g_iTmp = 0
                    g_iRC = UpdateTable(tbDrawerData)
                    Select Case g_iRC
                        Case PM_TRUE
                            Exit For
                        Case PM_DUPLICATEKEY
                        Case PM_FALSE
                            Interaction.MsgBox("Save Failed (tbl)", MB_ICONEXCLAMATION, "SaveDrawerData")
                            result = 0
                            Exit For
                        Case Else
                            Interaction.MsgBox("Save Failed (tbl) - " & g_iRC, MB_ICONEXCLAMATION, "SaveDrawerData")
                            result = 0
                            Exit For
                    End Select
                End If
				
				Pause(1)
				
			Next iTmp
			
			
			tbDrawerData.Close()
			tbDrawerData = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: SaveFolderData
	'      Author: simonb
	'        Date: 12/04/95
	'
	' Description: Saves the record structure into the Folder DDB.
	'
	'*********************************************************************
	'
	Function SaveFolderData(ByRef utStruct As g_utFolder) As Integer
		Dim result As Integer = 0
		Dim tbFolderData As DAO.Table
		Dim iTmpNextNumber As Integer
		
		Try 
			g_iRC = 0
			
			tbFolderData = g_dbDDB.OpenTable("Folder")
			tbFolderData.LockEdits = False
			tbFolderData.AddNew()
			
            tbFolderData("folder_name").Value = utStruct.folder_name.Value
            tbFolderData("ex_code").Value = utStruct.ex_code.Value
            tbFolderData("drawer_num").Value = utStruct.drawer_num
            tbFolderData("access_level").Value = utStruct.access_level
            tbFolderData("password").Value = utStruct.password.Value
            tbFolderData("link").Value = utStruct.link
			
			
			For iTmp As Integer = 0 To UPDATERETRY
                tbFolderData("folder_num").Value = GetNextDDBNumber(FOLDER)
				
                result = tbFolderData("folder_num").Value
				
                If tbFolderData("folder_num").Value > 0 Then
                    g_iTmp = 0
                    g_iRC = UpdateTable(tbFolderData)
                    Select Case g_iRC
                        Case PM_TRUE
                            Exit For
                        Case PM_DUPLICATEKEY
                        Case PM_FALSE
                            Interaction.MsgBox("Save Failed (tbl)", MB_ICONEXCLAMATION, "SaveFolderData")
                            result = 0
                            Exit For
                        Case Else
                            Interaction.MsgBox("Save Failed (tbl) - " & g_iRC, MB_ICONEXCLAMATION, "SaveFolderData")
                            result = 0
                            Exit For
                    End Select
                End If
				
				Pause(1)
				
			Next iTmp
			
			tbFolderData.Close()
			tbFolderData = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: SaveKeyWordData
	'      Author: simonb
	'        Date: 10/05/95
	'
	' Description: Saves the record structure into the keyword DDB.
	'
	'*********************************************************************
	'
	Function SaveKeyWordData(ByRef utStruct As g_utKeyWord) As Integer
		
		Dim result As Integer = 0
		Dim tbKeyWordData As DAO.Table
		
		result = True
		
		Try 
			
			If KeywrdExists(utStruct.doc_num, utStruct.key_num) = PM_TRUE Then
				Interaction.MsgBox("Keyword already attached", MB_ICONINFORMATION, "Attach Keywords")
				Return result
			End If
			
			tbKeyWordData = g_dbDDB.OpenTable("keyword")
			tbKeyWordData.AddNew()
			
            tbKeyWordData("doc_num").Value = utStruct.doc_num
            tbKeyWordData("key_num").Value = utStruct.key_num
            tbKeyWordData("user_name").Value = utStruct.user_name.Value
            tbKeyWordData("create_date").Value = utStruct.create_date
			
			g_iRC = 0
			For iTmp As Integer = 0 To UPDATERETRY
				g_iTmp = 0
				g_iRC = UpdateTable(tbKeyWordData)
				Select Case g_iRC
					Case PM_TRUE
						result = True
						Exit For
					Case PM_DUPLICATEKEY
					Case PM_FALSE
						result = False
						Interaction.MsgBox("Update Table failed. (tbl)", MB_ICONEXCLAMATION, "SaveKeyWordData")
						Exit For
					Case Else
						result = False
						Interaction.MsgBox("Update Table failed. (tbl) - " & g_iRC, MB_ICONEXCLAMATION, "SaveKeyWordData")
						Exit For
				End Select
				
				Pause(1)
				
			Next iTmp
			
			tbKeyWordData.Close()
			tbKeyWordData = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: SaveKeyWordsData
	'      Author: simonb
	'        Date: 10/05/95
	'
	' Description: Saves the record structure into the keywords DDB.
	'
	'*********************************************************************
	'
	Function SaveKeyWordsData(ByRef utStruct As g_utKeyWords) As Integer
		
		Dim result As Integer = 0
		Dim tbKeyWordsData As DAO.Table
		Dim iTmpNextNumber As Integer
		
		Try 
			g_iRC = 0
			
			tbKeyWordsData = g_dbDDB.OpenTable("keywords")
			tbKeyWordsData.AddNew()
			
            tbKeyWordsData("keyword").Value = utStruct.keyword.Value
            tbKeyWordsData("deleted").Value = False
			
			For iTmp As Integer = 0 To UPDATERETRY
                tbKeyWordsData("key_num").Value = GetNextDDBNumber(keywords)
				
                result = tbKeyWordsData("key_num").Value
				
                If tbKeyWordsData("key_num").Value > 0 Then
                    g_iTmp = 0
                    g_iRC = UpdateTable(tbKeyWordsData)
                    Select Case g_iRC
                        Case PM_TRUE
                            Exit For
                        Case PM_DUPLICATEKEY
                        Case PM_FALSE
                            Interaction.MsgBox("Save Failed (tbl)", MB_ICONEXCLAMATION, "SaveKeyWordsData")
                            result = 0
                            Exit For
                        Case Else
                            Interaction.MsgBox("Save Failed (tbl) - " & g_iRC, MB_ICONEXCLAMATION, "SaveKeyWordsData")
                            result = 0
                            Exit For
                    End Select
                End If
				
				Pause(1)
				
			Next iTmp
			
			tbKeyWordsData.Close()
			tbKeyWordsData = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: SavePageData
	'      Author: simonb
	'        Date: 12/04/95
	'
	' Description: Saves the record structure into the page DDB.
	'
	'*********************************************************************
	'
	Function SavePageData(ByRef utStruct As g_utPage) As String
		
		Dim result As String = String.Empty
		Dim tbPageData As DAO.Table
		Dim sPageName As String = ""
		
		Try 
			g_iRC = 0
			
			tbPageData = g_dbDDB.OpenTable("page")
			tbPageData.AddNew()
			
            tbPageData("doc_num").Value = utStruct.doc_num
            tbPageData("volume_name").Value = utStruct.volume_name.Value
            tbPageData("backup_name").Value = utStruct.backup_name.Value
            tbPageData("page_type").Value = utStruct.page_type.Value
            tbPageData("page_num").Value = utStruct.page_num
            tbPageData("scan_date").Value = utStruct.scan_date
            tbPageData("orientation").Value = 0
			
			For iTmp As Integer = 0 To UPDATERETRY
				sPageName = GetNextPageName()
                tbPageData("page_name").Value = sPageName
				
				If tbPageData("page_name") <> "" Then
					
					g_iTmp = 0
					g_iRC = UpdateTable(tbPageData)
					Select Case g_iRC
						Case PM_TRUE
							result = sPageName
							Exit For
						Case PM_DUPLICATEKEY
						Case PM_FALSE
							result = ""
							Interaction.MsgBox("Save Failed (tbl)", MB_ICONEXCLAMATION, "SavePageData")
							Exit For
						Case Else
							result = ""
							Interaction.MsgBox("Save Failed (tbl) - " & g_iRC, MB_ICONEXCLAMATION, "SavePageData")
							Exit For
					End Select
				End If
				
				Pause(1)
				
			Next iTmp
			
			tbPageData.Close()
			tbPageData = Nothing
			
			Return result
		
		Catch 
			
			
			
			Return ""
		End Try
		
	End Function
	
	Function SaveScanAnnotationsData(ByRef utStruct As g_utAnnotations) As Integer
		
		Dim dsAnnotationsData As DAO.Dynaset
		
		Try 
			
			dsAnnotationsData = g_dbSDB.CreateDynaset("annotation")
			dsAnnotationsData.AddNew()
			
            dsAnnotationsData("doc_num").Value = utStruct.doc_num
            dsAnnotationsData("ann_text").Value = utStruct.ann_text.Value
            dsAnnotationsData("user_name").Value = utStruct.user_name.Value
            dsAnnotationsData("create_date").Value = utStruct.create_date
			
			dsAnnotationsData.Update()
			dsAnnotationsData.Close()
			dsAnnotationsData = Nothing
			
			Return True
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function SaveScanKeyWordData(ByRef utStruct As g_utKeyWord) As Integer
		
		Dim dsKeyWordData As DAO.Dynaset
		
		Try 
			
			dsKeyWordData = g_dbSDB.CreateDynaset("keyword")
			dsKeyWordData.AddNew()
			
            dsKeyWordData("doc_num").Value = utStruct.doc_num
            dsKeyWordData("key_num").Value = utStruct.key_num
            dsKeyWordData("user_name").Value = utStruct.user_name.Value
            dsKeyWordData("create_date").Value = utStruct.create_date
			
			dsKeyWordData.Update()
			dsKeyWordData.Close()
			dsKeyWordData = Nothing
			
			Return True
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function SaveScanKeyWordsData(ByRef utStruct As g_utKeyWords) As Integer
		
		Dim result As Integer = 0
		Dim dsKeyWordsData As DAO.Dynaset
		Dim iTmpNextNumber As Integer
		
		Try 
			
			dsKeyWordsData = g_dbSDB.CreateDynaset("keywords")
			dsKeyWordsData.LockEdits = False
			dsKeyWordsData.AddNew()
			
			' Get the next ID number
			'    iTmpNextNumber = GetNextSDBNumber(KEYWORDS)
			result = iTmpNextNumber
			
			result = True
			
			'    If (iTmpNextNumber = 0) Then
			'        SaveScanKeyWordsData = False
			'        Exit Function
			'    End If
			
			'    tbKeyWordsData("key_num") = iTmpNextNumber
            dsKeyWordsData("key_num").Value = utStruct.key_num
            dsKeyWordsData("keyword").Value = utStruct.keyword.Value
            dsKeyWordsData("deleted").Value = False
			
			dsKeyWordsData.Update()
			dsKeyWordsData.Close()
			dsKeyWordsData = Nothing
			
			Return result
		
		Catch 
			
			
			
			'MsgBox "ERROR, " & Err & ", " & Error, MB_ICONEXCLAMATION, "SaveScanKeyWordsData"
			Return False
		End Try
		
	End Function
	
	Function SaveUserData(ByRef utStruct As g_utUser, ByRef sType As String) As Integer
		
		Dim dsUserData, dsUser As DAO.Dynaset
		Dim iTmp As Integer
		
		Try 
			g_iRC = 0
			
			If sType = "N" Then
				'add a new user
				
				dsUserData = g_dbDDB.CreateDynaset("user")
				dsUserData.AddNew()
				
                dsUserData("user_name").Value = utStruct.user_name.Value
				
				If utStruct.password.Value.Trim() <> "" Then
                    dsUserData("password").Value = Encryption(utStruct.password.Value.Trim())
				Else
                    dsUserData("password").Value = ""
				End If
				
                dsUserData("title").Value = utStruct.title.Value
                dsUserData("access_level").Value = utStruct.access_level
                dsUserData("login_limit").Value = utStruct.login_limit
				
				g_iTmp = 0
				g_iRC = UpdateDynaset(dsUserData)
				Select Case g_iRC
					Case PM_TRUE
					Case PM_DUPLICATEKEY
						Interaction.MsgBox("Login ID Exists already, try again.", MB_ICONEXCLAMATION, "Save User Details")
					Case PM_FALSE
						Interaction.MsgBox("Save Failed (ds)", MB_ICONEXCLAMATION, "SaveUserData")
					Case Else
						Interaction.MsgBox("Save Failed (ds) - " & g_iRC, MB_ICONEXCLAMATION, "SaveUserData")
				End Select
				
				dsUserData.Close()
				dsUserData = Nothing
				
			Else
				'change the user details
				dsUser = g_dbDDB.CreateDynaset("SELECT * FROM user WHERE user_name = '" & utStruct.user_name.Value & "'")
				DAO_DBEngine_definst.FreeLocks()
				dsUser.LockEdits = False
				
				If dsUser.RecordCount = 0 Then
					g_iRC = PM_FALSE
				Else
					dsUser.Edit()
					
					If utStruct.password.Value.Trim() <> "" Then
                        dsUser("password").Value = Encryption(utStruct.password.Value.Trim())
					End If
					
                    dsUser("title").Value = utStruct.title.Value
                    dsUser("access_level").Value = utStruct.access_level
                    dsUser("login_limit").Value = utStruct.login_limit
					
					g_iTmp = 0
					g_iRC = UpdateDynaset(dsUser)
					Select Case g_iRC
						Case PM_TRUE
						Case PM_DUPLICATEKEY
							Interaction.MsgBox("Save failed - Duplicate key", MB_ICONEXCLAMATION, "SaveUserData")
						Case PM_FALSE
							Interaction.MsgBox("Save Failed (ds)", MB_ICONEXCLAMATION, "SaveUserData")
						Case Else
							Interaction.MsgBox("Save Failed (ds) - " & g_iRC, MB_ICONEXCLAMATION, "SaveUserData")
					End Select
					
					dsUser.Close()
					dsUser = Nothing
				End If
				
			End If
			
			Return g_iRC
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function SaveVolumeData(ByRef utStruct As g_utVolume) As Integer
		
		Dim dsVolData As DAO.Dynaset
		Dim iTmp As Integer
		
		Try 
			g_iRC = 0
			
			dsVolData = g_dbDDB.CreateDynaset("volume")
			dsVolData.AddNew()
			
            dsVolData("volume_name").Value = utStruct.volume_name.Value
            dsVolData("device_num").Value = utStruct.device_num
            dsVolData("initial_date").Value = utStruct.initial_date
            dsVolData("initial_user").Value = utStruct.initial_user.Value
            dsVolData("rewriteable").Value = utStruct.rewriteable
			
			g_iTmp = 0
			g_iRC = UpdateDynaset(dsVolData)
			Select Case g_iRC
				Case PM_TRUE
				Case PM_DUPLICATEKEY, PM_FALSE
					Interaction.MsgBox("Save Failed (ds)", MB_ICONEXCLAMATION, "SaveVolumeData")
				Case Else
					Interaction.MsgBox("Save Failed (ds) - " & g_iRC, MB_ICONEXCLAMATION, "SaveVolumeData")
			End Select
			
			dsVolData.Close()
			dsVolData = Nothing
			
			Return g_iRC
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	Function SetRecordLock(ByRef iLevel As Integer, ByRef lCode As Integer, ByRef iLockType As Integer) As Integer
		
		Dim result As Integer = 0
		Dim dsRecordLock As DAO.Dynaset
		Dim ssLock As DAO.Snapshot
		Dim lNextLockNumber As Integer
		
		Return True
		
		Try 
			
			ssLock = g_dbDDB.CreateSnapshot("SELECT lock_type FROM recordlock WHERE level = " & iLevel & " AND code = " & CStr(lCode) & " AND lock_type = " & CStr(RL_HARD))
			DAO_DBEngine_definst.FreeLocks()
			
			If ssLock.RecordCount > 0 Then
				result = -1
			Else
				
				dsRecordLock = g_dbDDB.CreateDynaset("recordlock")
				dsRecordLock.LockEdits = False
				dsRecordLock.AddNew()
				
				lNextLockNumber = GetNextDDBNumber(RECORDLOCK)
				
				If lNextLockNumber = 0 Then
					' Failed to get next lock number
					result = 0
				Else
					
                    dsRecordLock("lock_num").Value = lNextLockNumber
                    dsRecordLock("lock_type").Value = iLockType
                    dsRecordLock("level").Value = iLevel
                    dsRecordLock("code").Value = lCode
                    dsRecordLock("user").Value = g_sUsername
                    dsRecordLock("date").Value = DateTime.Today.AddDays(DateTimeHelper.Time.ToOADate())
					
					g_iTmp = 0
					g_iRC = UpdateDynaset(dsRecordLock)
					Select Case g_iRC
						Case PM_TRUE
						Case PM_FALSE
							Interaction.MsgBox("Update Failed. (ds)", MB_ICONEXCLAMATION, "SetRecordLock")
						Case PM_CANCEL
							Interaction.MsgBox("Update Cancelled. (ds)", MB_ICONEXCLAMATION, "SetRecordLock")
						Case Else
							Interaction.MsgBox("Update Failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "SetRecordLock")
					End Select
					
					result = lNextLockNumber
				End If
				
				dsRecordLock.Close()
				dsRecordLock = Nothing
			End If
			
			ssLock.Close()
			Return result
		
		Catch 
			
			
			
			Return 0
		End Try
		
	End Function
	
	Function UpdateUserPassword(ByRef sUsername As String, ByRef sNewPass As String) As Integer
		
		Dim result As Integer = 0
		Dim tbUserData As DAO.Dynaset
		Dim sSQL As String = ""
		Dim ssSnap As DAO.Snapshot
		Dim lLockNum As Integer
		Dim iLockCount, iTmp As Integer
		
		Try 
			g_iRC = 0
			
			sSQL = "SELECT * FROM user WHERE user_name = '" & sUsername & "'"
			
			tbUserData = g_dbDDB.CreateDynaset(sSQL)
			DAO_DBEngine_definst.FreeLocks()
			tbUserData.LockEdits = False
			
			tbUserData.Edit()
			
            tbUserData("password").Value = Encryption(sNewPass)
			
			g_iTmp = 0
			g_iRC = UpdateDynaset(tbUserData)
			Select Case g_iRC
				Case PM_TRUE
					result = True
				Case PM_FALSE, PM_CANCEL
					Interaction.MsgBox("Update failed. (ds)", MB_ICONEXCLAMATION, "UpdateUserPassword")
					result = False
				Case Else
					Interaction.MsgBox("Update failed. (ds) - " & g_iRC, MB_ICONEXCLAMATION, "UpdateUserPassword")
					result = False
			End Select
			
			tbUserData.Close()
			tbUserData = Nothing
			
			' UpdateUserPassword = g_iRC%
			Return result
		
		Catch 
			
			
			
			Return False
		End Try
		
	End Function
	
	'*********************************************************************
	'
	' Module Name: UserExists
	'      Author: simonb
	'        Date: 24/04/95
	'
	' Description: Checks if the user name exists in the DDB.
	'
	'*********************************************************************
	'
	Function UserExists(ByRef sUsername As String) As Integer
		Dim result As Integer = 0
		Dim sSQLQuery As String = ""
		Dim ssTmpSnapshot As DAO.Snapshot
		Dim sTmpUserName As New FixedLengthString(30)
		
		' Get Password for DDB
		
		Try 
			
			sTmpUserName.Value = sUsername
			
			sSQLQuery = "SELECT access_level FROM user WHERE retired = " & False & " AND user_name = '" & sTmpUserName.Value & "'"
			
			ssTmpSnapshot = g_dbDDB.CreateSnapshot(sSQLQuery)
			DAO_DBEngine_definst.FreeLocks()
			
			If ssTmpSnapshot.RecordCount > 0 Then
                result = ssTmpSnapshot(0).Value
			Else
				result = -1
			End If
			
			ssTmpSnapshot.Close()
			Return result
		
		Catch 
			
			
			
			Return -1
		End Try
		
	End Function
End Module