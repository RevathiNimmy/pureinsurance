Option Strict Off
Option Explicit On
Imports System.Runtime.InteropServices
Imports SSP.Shared
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
    ' RDC 05082004 let's give it the same name as the project, to aid bug tracking!
    'Public Const ACApp = "bDOCPMBAPI"
    Public Const ACApp As String = "bSIRDOCAPI"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public instance of the object manager.
    'developer guide no. 107
    '<ThreadStatic()>
    'Public g_oObjectManager As bObjectManager.ObjectManager

    ' Username.
    'Public g_sUsername As String * 12

    ' Password.
    'Public g_sPassword As String * 30

    ' User ID
    'Public g_iUserID As Integer

    ' Calling Application
    'Public g_sCallingAppName As String
    ' Source ID
    'Public g_iSourceID As Integer
    ' Language ID
    'Public g_iLanguageID As Integer
    ' Currency ID
    'Public g_iCurrencyID As Integer
    ' LogLevel
    'Public g_iLogLevel As Integer

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
        '	'<VBFixedString(20), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=20)>
        Public cabinetcode As StringsHelper.FixedLengthString
        '<VBFixedString(30), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=30)>
        Public cabinetname As StringsHelper.FixedLengthString
        '<VBFixedString(20), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=20)>
        Public drawercode As StringsHelper.FixedLengthString
        '<VBFixedString(30), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=30)>
        Public drawername As StringsHelper.FixedLengthString
        '<VBFixedString(20), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=20)>
        Public foldercode As StringsHelper.FixedLengthString
        '<VBFixedString(70), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=70)>
        Public foldername As StringsHelper.FixedLengthString
        '<VBFixedString(9), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=9)>
        Public docref As StringsHelper.FixedLengthString
        '<VBFixedString(8), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=8)>
        Public date_Renamed As StringsHelper.FixedLengthString
        '<VBFixedString(6), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=6)>
        Public time As StringsHelper.FixedLengthString
        '<VBFixedString(1), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=1)>
        Public eventtype As StringsHelper.FixedLengthString
        '<VBFixedString(30), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=30)>
        Public description As StringsHelper.FixedLengthString
        '<VBFixedString(20), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=20)>
        Public volume As StringsHelper.FixedLengthString
        '<VBFixedString(10), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=10)>
        Public pagefile As StringsHelper.FixedLengthString
        '<VBFixedString(3), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=3)>
        Public doctype As StringsHelper.FixedLengthString
        Dim hdError As Integer
        Dim create_date As Double
        '<VBFixedString(3), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=3)>
        Public filler As StringsHelper.FixedLengthString
        Public Shared Function CreateInstance() As g_utDMSHistData
            Dim result As New g_utDMSHistData
            result.cabinetcode = New StringsHelper.FixedLengthString(20)
            result.cabinetname = New StringsHelper.FixedLengthString(30)
            result.drawercode = New StringsHelper.FixedLengthString(20)
            result.drawername = New StringsHelper.FixedLengthString(30)
            result.foldercode = New StringsHelper.FixedLengthString(20)
            result.foldername = New StringsHelper.FixedLengthString(70)
            result.docref = New StringsHelper.FixedLengthString(9)
            result.time = New StringsHelper.FixedLengthString(6)
            result.eventtype = New StringsHelper.FixedLengthString(1)
            result.description = New StringsHelper.FixedLengthString(30)
            result.volume = New StringsHelper.FixedLengthString(20)
            result.pagefile = New StringsHelper.FixedLengthString(10)
            result.doctype = New StringsHelper.FixedLengthString(3)
            result.filler = New StringsHelper.FixedLengthString(3)
            Return result
        End Function
    End Structure

    ' Parameters passed to the DMS History DLL.
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
                                                                 _
    Structure g_utDMSHistParams
        Dim NewRec As g_utDMSHistData
        '<VBFixedString(80), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=80)>
        Public DMSDir As StringsHelper.FixedLengthString
        '<VBFixedString(4), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=4)>
        Public ReturnCode As StringsHelper.FixedLengthString
        '<VBFixedString(2), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=2)>
        Public FileStatus As StringsHelper.FixedLengthString
        Public Shared Function CreateInstance() As g_utDMSHistParams
            Dim result As New g_utDMSHistParams
            result.NewRec = g_utDMSHistData.CreateInstance()
            result.DMSDir = New StringsHelper.FixedLengthString(80)
            result.ReturnCode = New StringsHelper.FixedLengthString(4)
            result.FileStatus = New StringsHelper.FixedLengthString(2)
            Return result
        End Function
    End Structure

    ' New parameter for 32bit DLL (cannot pass user defined types
    ' any more)
    Public g_sHistParam As New StringsHelper.FixedLengthString(376)


    ' Get Initial Insurance File Cnt
    Public Const ACGetInitialInsuranceFileCntStored As Boolean = True
    Public Const ACGetInitialInsuranceFileCntName As String = "Get Initial Insurance File Cnt"
    Public Const ACGetInitialInsuranceFileCntSQL As String = "spu_get_initial_insurance_file_cnt"

    ' Get Policy All versions
    Public Const ACGetInsuranceRefStored As Boolean = True
    Public Const ACGetInsuranceRefName As String = "Get Policy Reference For Insurance Folder Cnt"
    Public Const ACGetInsuranceRefSQL As String = "spu_SIR_GetInsuranceRefForInsuranceFolderCnt"

    Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As Integer, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer

    Sub Main_Renamed()


    End Sub
End Module