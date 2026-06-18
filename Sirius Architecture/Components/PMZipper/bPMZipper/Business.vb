Option Strict Off
Option Explicit On
Imports System.IO
Imports System.Runtime.InteropServices
Imports SSP.Shared
Imports System.IO.Compression

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    '***********************************************************************************
    '
    ' IMPORTANT: this project was originally cloned from the bSirZipper project
    ' Created CL170299

    ' RAG060600 AllowAppend property added. Set to True to ignore the "Zip File already exists"
    '           error in method ZipFile.
    '
    '***********************************************************************************
    'Commented the code which used the OLD method of Zip/Unzip.
    '***********************************************************************************

    'This TEXT can be included in the DECLARATIONS area of a Visual Basic Module.  The
    'Values and variables should be GLOBAL to the Visual Basic Application
    '
    'Needed to keep this going when not using PMLibraries
    'Const PMError = 9999


    '------ Types needed for DynaZIP -------------------------------------------------------

    'Private Structure VBSTATBLK
    '	Dim hMinorStat As Integer 'hWnd for the Minor Status Display
    '	Dim hMajorStat As Integer 'hWnd for the Major Status Display
    '	Dim hMsgDispZip As Integer 'Reserved set to 0
    '	Dim hMsgDispUnZip As Integer 'Reserved set to 0
    'End Structure

    '<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
    ' _
    'Private Structure ZIPINFO
    '	Dim Index As Integer
    '	Dim oSize As Integer
    '	Dim cSize As Integer
    '	Dim cMethod As Integer
    '	Dim cOption As Integer
    '	Dim cPathType As Integer
    '	Dim crc_32 As Integer 'Actually unsigned long in "C"
    '	<VBFixedString(18),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=18)> _
    '	Public szDateTime As FixedLengthString
    '	<VBFixedString(260),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=260)> _
    '	Public szFileName As FixedLengthString
    '	Dim attr As Integer
    '	Dim lziResv1 As Integer
    '	Dim lziResv2 As Integer
    '	Public Shared Function CreateInstance() As ZIPINFO
    '		Dim result As New ZIPINFO
    '		result.szDateTime = New FixedLengthString(18)
    '		result.szFileName = New FixedLengthString(260)
    '		Return result
    '	End Function
    'End Structure

    'Public Structure UNZIPCMDSTRUCT
    '	Dim unzipStructSize As Integer ' INT in "C"
    '	Dim function_Renamed As Integer ' INT in "C"
    '	Dim lpszZIPFile As Integer ' LPSTR in "C"
    '	Dim zInfo As Integer ' ZIPINFO far * in "C"
    '	Dim lpszFilespec As Integer ' LPSTR in "C"
    '	Dim Index As Integer ' INT in "C"
    '	Dim lpszDestination As Integer ' LPSTR in "C"
    '	Dim freshenFlag As Integer ' BOOL in "C"
    '	Dim updateFlag As Integer ' BOOL in "C"
    '	Dim overWriteFlag As Integer ' BOOL in "C"
    '	Dim quietFlag As Integer ' BOOL in "C"
    '	Dim testFlag As Integer ' BOOL in "C"
    '	Dim noDirectoryNamesFlag As Integer ' BOOL in "C"
    '	Dim recurseFlag As Integer ' BOOL in "C"
    '	Dim noDirectoryItemsFlag As Integer ' BOOL in "C"
    '	Dim lpMinorStatus As Integer ' FARPROC in "C"
    '	Dim lpMinorUserData As Integer ' void far * in "C"
    '	Dim lpMajorStatus As Integer ' FARPROC in "C"
    '	Dim lpMajorUserData As Integer ' void far * in "C"
    '	Dim returnCount As Integer ' INT in "C"
    '	Dim lpszReturnString As Integer ' LPSTR in "C"
    '	Dim bDiagnostic As Integer ' BOOL in "C"
    '	Dim bLF2CRLFFlag As Integer ' BOOL in "C"
    '	Dim decryptFlag As Integer ' BOOL in "C"
    '	Dim lpszDecryptCode As Integer ' LPSTR in "C"
    '	Dim lpMessageDisplay As Integer ' FARPROC in "C"
    '	Dim lpMessageDisplayData As Integer ' void far * in "C"
    '	Dim wUnzipSubOptions As Integer ' WORD in "C"
    '	' added for rev 3.00
    '	Dim lResv1 As Integer ' LONG in "C"
    '	Dim lResv2 As Integer ' LONG in "C"
    '	Dim lpRenameProc As Integer ' FARPROC in "C"
    '	Dim lpRenameUserData As Integer ' void far * in "C"
    '	Dim lpszExtProgTitle As Integer ' LPSTR in "C"
    '	Dim lpMemBlock As Integer ' void far * in "C"
    '	Dim lMemBlockSize As Integer ' LONG in "C"
    '	Dim lStartingOffset As Integer ' LONG in "C"
    'End Structure

    '<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
    ' _
    'Private Structure VBDUNZIPBLOCK
    '	<VBFixedString(260),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=260)> _
    '	Public szZIPFile As FixedLengthString
    '	<VBFixedString(5120),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=5120)> _
    '	Public szFileSpec As FixedLengthString
    '	<VBFixedString(260),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=260)> _
    '	Public szDestination As FixedLengthString
    '	<VBFixedString(2048),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=2048)> _
    '	Public szReturnString As FixedLengthString
    '	<VBFixedString(66),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=66)> _
    '	Public szDecryptCode As FixedLengthString
    '	'Added for Rev 3
    '	<VBFixedString(260),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=260)> _
    '	Public szExtProgTitle As FixedLengthString
    '	Public Shared Function CreateInstance() As VBDUNZIPBLOCK
    '		Dim result As New VBDUNZIPBLOCK
    '		result.szZIPFile = New FixedLengthString(260)
    '		result.szFileSpec = New FixedLengthString(5120)
    '		result.szDestination = New FixedLengthString(260)
    '		result.szReturnString = New FixedLengthString(2048)
    '		result.szDecryptCode = New FixedLengthString(66)
    '		result.szExtProgTitle = New FixedLengthString(260)
    '		Return result
    '	End Function
    'End Structure

    '<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
    ' _
    'Private Structure VBDZIPBLOCK
    '	<VBFixedString(260),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=260)> _
    '	Public szZIPFile As FixedLengthString
    '	<VBFixedString(5120),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=5120)> _
    '	Public szItemList As FixedLengthString
    '	<VBFixedString(260),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=260)> _
    '	Public szTempPath As FixedLengthString
    '	<VBFixedString(2048),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=2048)> _
    '	Public szComment As FixedLengthString
    '	<VBFixedString(7),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=7)> _
    '	Public szDate As FixedLengthString
    '	<VBFixedString(2048),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=2048)> _
    '	Public szIncludeFollowing As FixedLengthString
    '	<VBFixedString(2048),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=2048)> _
    '	Public szExcludeFollowing As FixedLengthString
    '	<VBFixedString(1024),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=1024)> _
    '	Public szStoreSuffixes As FixedLengthString
    '	<VBFixedString(66),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=66)> _
    '	Public szEncryptCode As FixedLengthString
    '	'Added for Rev 3
    '	<VBFixedString(260),System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr,SizeConst:=260)> _
    '	Public szExtProgTitle As FixedLengthString
    '	Public Shared Function CreateInstance() As VBDZIPBLOCK
    '		Dim result As New VBDZIPBLOCK
    '		result.szZIPFile = New FixedLengthString(260)
    '		result.szItemList = New FixedLengthString(5120)
    '		result.szTempPath = New FixedLengthString(260)
    '		result.szComment = New FixedLengthString(2048)
    '		result.szDate = New FixedLengthString(7)
    '		result.szIncludeFollowing = New FixedLengthString(2048)
    '		result.szExcludeFollowing = New FixedLengthString(2048)
    '		result.szStoreSuffixes = New FixedLengthString(1024)
    '		result.szEncryptCode = New FixedLengthString(66)
    '		result.szExtProgTitle = New FixedLengthString(260)
    '		Return result
    '	End Function
    'End Structure

    'Public Structure ZIPCMDSTRUCT
    '	Dim zipStructSize As Integer 'INT in "C"
    '	Dim function_Renamed As Integer 'INT in "C"
    '	Dim lpszZIPFile As Integer 'LPSTR in "C"
    '	Dim lpszItemList As Integer 'LPSTR in "C"
    '	Dim lpMajorStatus As Integer 'FARPROC in "C"
    '	Dim lpMajorUserData As Integer 'void far * in "C"
    '	Dim lpMinorStatus As Integer 'FARPROC in "C"
    '	Dim lpMinorUserData As Integer 'void far * in "C"
    '	Dim dosifyFlag As Integer 'BOOL in "C"
    '	Dim recurseFlag As Integer 'BOOL in "C"
    '	Dim compFactor As Integer 'INT in "C"
    '	Dim quietFlag As Integer 'BOOL in "C"
    '	Dim pathForTempFlag As Integer 'BOOL in "C"
    '	Dim lpszTempPath As Integer 'LPSTR in "C"
    '	Dim FixFlag As Integer 'BOOL in "C"
    '	Dim fixHarderFlag As Integer 'BOOL in "C"
    '	Dim includeVolumeFlag As Integer 'BOOL in "C"
    '	Dim deleteOriginalFlag As Integer 'BOOL in "C"
    '	Dim growExistingFlag As Integer 'BOOL in "C"
    '	Dim noDirectoryNamesFlag As Integer 'BOOL in "C"
    '	Dim convertLFtoCRLFFlag As Integer 'BOOL in "C"
    '	Dim addCommentFlag As Integer 'BOOL in "C"
    '	Dim lpszComment As Integer 'LPSTR in "C"
    '	Dim afterDateFlag As Integer 'BOOL in "C"
    '	Dim lpszDate As Integer 'LPSTR in "C"
    '	Dim oldAsLatestFlag As Integer 'BOOL in "C"
    '	Dim includeOnlyFollowingFlag As Integer 'BOOL in "C"
    '	Dim lpszIncludeFollowing As Integer 'LPSTR in "C"
    '	Dim excludeFollowingFlag As Integer 'BOOL in "C"
    '	Dim lpszExcludeFollowing As Integer 'LPSTR in "C"
    '	Dim noDirectoryEntriesFlag As Integer 'BOOL in "C"
    '	Dim includeSysHiddenFlag As Integer 'BOOL in "C"
    '	Dim dontCompressTheseSuffixesFlag As Integer 'BOOL in "C"
    '	Dim lpszStoreSuffixes As Integer 'LPSTR in "C"
    '	Dim bDiagnostic As Integer 'BOOL in "C"
    '	Dim encryptFlag As Integer 'BOOL in "C"
    '	Dim lpszEncryptCode As Integer 'LPSTR in "C"
    '	Dim lpMessageDisplay As Integer 'FARPROC in "C"
    '	Dim lpMessageDisplayData As Integer 'void far * in "C"
    '	Dim wMultiVolControl As Integer 'WORD in "C"
    '	Dim wZipSubOptions As Integer 'WORD in "C"
    '	' added for rev 3.00
    '	Dim lResv1 As Integer ' LONG in "C"
    '	Dim lResv2 As Integer ' LONG in "C"
    '	Dim lpRenameProc As Integer ' FARPROC in "C"
    '	Dim lpRenameUserData As Integer ' void far * in "C"
    '	Dim lpszExtProgTitle As Integer ' LPSTR in "C"
    '	Dim lpMemBlock As Integer ' void far * in "C"
    '	Dim lMemBlockSize As Integer ' LONG in "C"
    'End Structure

    'Private Declare Function dunzipVB Lib "dunzip32.dll" (ByRef ucs As UNZIPCMDSTRUCT, ByRef vbduzBlk As VBDUNZIPBLOCK, ByRef zi As ZIPINFO, ByRef statusBlk As VBSTATBLK) As Integer

    'Private Declare Function dzipVB Lib "dzip32.dll" (ByRef zcs As ZIPCMDSTRUCT, ByRef vbdzBlk As VBDZIPBLOCK, ByRef statusBlk As VBSTATBLK) As Integer

    ''Additional flag bit for Encrypted Attribute
    'Const ATTR_ENCRYPT As Integer = &H8000
    'Const ATTR_LFNAME As Integer = &H10000

    '' Multi-Volume Control FLAG definitions
    'Const MV_FORMAT As Integer = &H1s
    'Const MV_LOWDENSE As Integer = &H2s

    'Const MV_WIPE As Integer = &H100s
    'Const MV_SUBDIR As Integer = &H200s
    'Const MV_SYSHIDE As Integer = &H400s

    'Const MV_CDFIRST As Integer = &H1000s
    'Const MV_USEMULTI As Integer = &H8000s

    'Const ZSO_RELATIVEPATHFLAG As Integer = &H1s
    'Const ZSO_MINORCANCEL As Integer = &H2s
    'Const ZSO_EXTERNALPROG As Integer = &H4s
    'Const ZSO_EXTPROGCANCEL As Integer = &H8s
    'Const ZSO_IGNORELONGNAMES As Integer = &H10s
    'Const ZSO_MANGLELONGNAMES As Integer = &H20s

    'Const USO_OVERWRITE_RO As Integer = &H1s
    'Const USO_MINORCANCEL As Integer = &H2s
    'Const USO_EXTERNALPROG As Integer = &H4s
    'Const USO_EXTPROGCANCEL As Integer = &H8s
    'Const USO_IGNORELONGNAMES As Integer = &H10s
    'Const USO_MANGLELONGNAMES As Integer = &H20s


    '' the "function"s that are supported for unzipping
    'Const UNZIP_COUNTALLZIPMEMBERS As Integer = 1
    'Const UNZIP_GETNEXTZIPINFO As Integer = 2
    'Const UNZIP_COUNTNAMEDZIPMEMBERS As Integer = 3
    'Const UNZIP_GETNEXTNAMEDZIPINFO As Integer = 4
    'Const UNZIP_GETCOMMENTSIZE As Integer = 5
    'Const UNZIP_GETCOMMENT As Integer = 6
    'Const UNZIP_GETINDEXEDZIPINFO As Integer = 7
    'Const UNZIP_EXTRACT As Integer = 8
    'Const UNZIP_FILETOMEM As Integer = 9

    ''error codes returned for unzipping
    'Const UE_OK As Integer = 0 ' success
    'Const UE_EOF As Integer = 2 ' unexpected end of zip file
    'Const UE_STRUCT As Integer = 3 ' structure error in zip file
    'Const UE_MEM1 As Integer = 4 ' out of memory
    'Const UE_MEM2 As Integer = 5 ' out of memory
    'Const UE_NOFILE As Integer = 9 ' file not found error
    'Const UE_BORED As Integer = 11 ' nothing to do
    'Const UE_INDEX As Integer = 25 ' index out of bounds
    'Const UE_OUTPUT As Integer = 28 ' error creating output file
    'Const UE_OPEN As Integer = 29 ' error opening output file
    'Const UE_BADCRC As Integer = 39 ' crc error
    'Const UE_ENCRYPT As Integer = 41 ' file skipped, encrypted
    'Const UE_UNKNOWN As Integer = 42 ' unknown compression method
    'Const UE_NOVOL As Integer = 46 ' can't unzip a volume item
    'Const UE_CMDERR As Integer = 47 ' bad command structure
    'Const UE_CANCEL As Integer = 48 ' user cancelled this operation
    'Const UE_SKIP As Integer = 49 ' user skipped this operation
    'Const UE_DISKFULL As Integer = 50 ' disk full


    '' the "function"s that are supported for zipping
    'Const ZIP_FRESHEN As Integer = 1
    'Const ZIP_DELETE As Integer = 2
    'Const ZIP_UPDATE As Integer = 3
    'Const ZIP_ADD As Integer = 4
    'Const ZIP_MEMTOFILE As Integer = 5

    '' Error return values for zipping.  The values 0..4 and 12..18 follow the conventions
    ''   of PKZIP.   The values 4..10 are all assigned to "insufficient memory"
    ''   by PKZIP, so the codes 5..10 are used here for other purposes.

    'Const ZE_MISS As Integer = -1 'used by procname(), zipbare()
    'Const ZE_OK As Integer = 0 'success
    'Const ZE_EOF As Integer = 2 'unexpected end of zip file
    'Const ZE_FORM As Integer = 3 'zip file structure error
    'Const ZE_MEM As Integer = 4 'out of memory
    'Const ZE_LOGIC As Integer = 5 'internal logic error
    'Const ZE_BIG As Integer = 6 'entry too large to split
    'Const ZE_NOTE As Integer = 7 'invalid comment format
    'Const ZE_TEST As Integer = 8 'zip test (-T) failed or out of memory
    'Const ZE_ABORT As Integer = 9 'user interrupt or termination
    'Const ZE_TEMP As Integer = 10 'error using a temp file
    'Const ZE_READ As Integer = 11 'read or seek error
    'Const ZE_NONE As Integer = 12 'nothing to do
    'Const ZE_NAME As Integer = 13 'missing or empty zip file
    'Const ZE_WRITE As Integer = 14 'error writing to a file
    'Const ZE_CREAT As Integer = 15 'couldn't open to write
    'Const ZE_PARMS As Integer = 16 'bad command line
    'Const ZE_INCOMPLETE As Integer = 17 'Could Not Complete Operation
    'Const ZE_OPEN As Integer = 18 'could not open a specified file to read
    'Const ZE_MEDIA As Integer = 19 'Media error or HW failure
    'Const ZE_MVPARMS As Integer = 20 'Invalid combination of control parameters
    'Const ZE_MVUSAGE As Integer = 21 'Improper use of a Multi-Volume Zip file


    '' Global "Visual Basic to C translation" structure variables
    '' that can be used for calls to DZIP and DUNZIP DLLs.  You probably want to declare
    '' these variables as Global since they take a lot of memory.  The init functions also rely on
    '' these variables being Global
    ''

    'Private vbduzBlk As VBDUNZIPBLOCK = VBDUNZIPBLOCK.CreateInstance()

    'Private vbdzBlk As VBDZIPBLOCK = VBDZIPBLOCK.CreateInstance()

    'Private zi As ZIPINFO = ZIPINFO.CreateInstance()
    'Private statusBlk As New VBSTATBLK

    Private g_sTempPath As String = ""

    ' RAG060600
    ' Property AllowAppend added to override "File already exists" error in ZipFile method.
    Private m_bAllowAppend As Boolean = False

    ' RAG120600
    ' Property NoDirectory added to allow No Directory info to be stored in file
    Private m_bNoDirectory As Boolean = False

    ' RDC 26072001 for new GEM2/GIS functionality
    Dim m_OriginalSize As Integer
    Dim m_CompressedSize As Integer

    ' RDC 26072001 for new GEM2/GIS functionality
    Public ReadOnly Property OriginalSize() As Integer
        Get
            Return m_OriginalSize
        End Get
    End Property


    Property AllowAppend() As Boolean
        Get
            Return m_bAllowAppend
        End Get
        Set(ByVal Value As Boolean)
            m_bAllowAppend = Value
        End Set
    End Property


    Property NoDirectory() As Boolean
        Get
            Return m_bNoDirectory
        End Get
        Set(ByVal Value As Boolean)
            m_bNoDirectory = Value
        End Set
    End Property



    'The following declaration describes the Visual Basic DLL Function that
    'fills in the ucs structure with the appropriate far pointers and far references
    'that are required by the "C" version interface in the DynaZip UNZIP DLL
    'INPUTS:
    '  ucs          FAR PTR to an UNZIPCMDSTRUCT structure that will be used by the DLL
    '  vbduzBlk     FAR PTR to a VBDUNZIPBLOCK structure containing properly initialized data
    '  zi           FAR PTR to a ZIPINFO structure that will be used by the DLL
    '  statusBlk    FAR PTR to a ZIP Status Structure containing handles for the Minor and major status
    '
    'OUTPUTS:
    '  ucs          Structure members filled in with return information from the DLL side
    '
    'RETURNS:
    '  A Long Value with an error code
    '
    'ASSUMPTIONS:
    '  All Strings must be fixed length and can NOT be moved in memory during the Status CALLBACKS
    '  All Data Items must not be moved during the Status CALLBACKS
    '

    '
    '
    '
    '--------------------------------------------------------------------------------------------------------------------------------------------
    'The following declaration describes the Visual Basic DLL Function that
    'fills in the zcs structure with the appropriate far pointers and far references
    'that are required by the "C" version interface in the DynaZip ZIP DLL
    'INPUTS:
    '  zcs          FAR PTR to an ZIPCMDSTRUCT structure that will be used by the DLL
    '  vbdzblk      FAR PTR to a VBDZIPBLOCK structure containing properly initialized data
    '  statusBlk    FAR PTR to a ZIP Status Structure containing handles for the Minor and major status
    '
    'OUTPUTS:
    '  ucs          Structure members filled in with return information from the DLL side
    '
    'RETURNS:
    '  A Long Value with an error code
    '
    'ASSUMPTIONS:
    '  All Strings must be fixed length and can NOT be moved in memory during the Status CALLBACKS
    '  All Data Items must not be moved during the Status CALLBACKS
    '

    '---------------------------------------------------------------------------------------------------------------------------------------------
    '
    '
    '
    '-----------------------------------------------------------------------------------------------
    'EXAMPLE CODE for DZIP and DUNZIP Structure INITIALIZATION.  These subroutines should be copied into
    'one of your Visual Basic Modules so that they can be called from any form or other module.
    '
    '

    '
    ' **************************************************************************************
    '
    '  Procedure:  initUNZIPCmdStruct()
    '
    '  Purpose:  make the inputted UNZIP command structure "plain vanilla"
    '
    ' **************************************************************************************
    'Private Sub InitUNZIPCmdStruct(ByRef ucs As UNZIPCMDSTRUCT)

    '	ucs.unzipStructSize = Marshal.SizeOf(ucs)
    '	ucs.function_Renamed = 0 ' No Action
    '	ucs.lpszZIPFile = 0 'NULL
    '	ucs.zInfo = 0 'NULL
    '	ucs.lpszFilespec = 0 'NULL
    '	ucs.Index = -1
    '	ucs.lpszDestination = 0 'NULL
    '	ucs.freshenFlag = 0 'False
    '	ucs.updateFlag = 0 'False
    '	ucs.overWriteFlag = 0 'False
    '	ucs.quietFlag = 0 'False
    '	ucs.testFlag = 0 'False
    '	ucs.noDirectoryNamesFlag = True 'True
    '	ucs.recurseFlag = True 'True
    '	ucs.lpMinorStatus = 0 'NULL
    '	ucs.lpMinorUserData = 0 'NULL
    '	ucs.lpMajorStatus = 0 'NULL
    '	ucs.lpMajorUserData = 0 'NULL
    '	ucs.returnCount = 0
    '	ucs.lpszReturnString = 0 'NULL
    '	ucs.bDiagnostic = 0 'False
    '	ucs.bLF2CRLFFlag = 0 'False
    '	ucs.decryptFlag = 0 'False
    '	ucs.lpszDecryptCode = 0 'NULL
    '	ucs.lpMessageDisplay = 0 'NULL
    '	ucs.lpMessageDisplayData = 0 'NULL
    '	ucs.wUnzipSubOptions = 0 'NO sub options

    '	statusBlk.hMinorStat = 0
    '	statusBlk.hMajorStat = 0
    '	statusBlk.hMsgDispZip = 0
    '	statusBlk.hMsgDispUnZip = 0

    '	vbduzBlk.szZIPFile.Value = ""
    '	vbduzBlk.szFileSpec.Value = ""
    '	vbduzBlk.szDestination.Value = ""
    '	vbduzBlk.szReturnString.Value = ""
    '	vbduzBlk.szDecryptCode.Value = ""

    '	' added for rev 3.00

    '	vbduzBlk.szExtProgTitle.Value = ""

    '	ucs.lResv1 = 0
    '	ucs.lResv2 = 0
    '	ucs.lpRenameProc = 0
    '	ucs.lpRenameUserData = 0
    '	ucs.lpszExtProgTitle = 0
    '	ucs.lpMemBlock = 0
    '	ucs.lMemBlockSize = 0
    '	ucs.lStartingOffset = 0


    'End Sub

    ' **************************************************************************************
    '
    '  Procedure:  initZIPCmdStruct()
    '
    '  Purpose:  make the inputted ZIP command structure "plain vanilla"
    '
    ' **************************************************************************************
    'Private Sub InitZIPCmdStruct(ByRef zcs As ZIPCMDSTRUCT)

    '	zcs.zipStructSize = Marshal.SizeOf(zcs)
    '	zcs.function_Renamed = 0 'No Action
    '	zcs.bDiagnostic = 0 'False
    '	zcs.lpszZIPFile = 0 'NULL
    '	zcs.lpszItemList = 0 'NULL
    '	zcs.lpMajorStatus = 0 'NULL
    '	zcs.lpMajorUserData = 0 'NULL
    '	zcs.lpMinorStatus = 0 'NULL
    '	zcs.lpMinorUserData = 0 'NULL
    '	zcs.dosifyFlag = 0 'False
    '	zcs.recurseFlag = 0 'False
    '	zcs.compFactor = 5
    '	zcs.quietFlag = 0 'False
    '	zcs.pathForTempFlag = -1 'True
    '	'set in vbdzblk
    '	zcs.lpszTempPath = 0 'NULL
    '	zcs.dontCompressTheseSuffixesFlag = 0 'False
    '	zcs.lpszStoreSuffixes = 0 'NULL
    '	zcs.FixFlag = 0 'False
    '	zcs.fixHarderFlag = 0 'False
    '	zcs.includeVolumeFlag = 0 'False
    '	zcs.deleteOriginalFlag = 0 'False
    '	zcs.growExistingFlag = 0 'False
    '	zcs.noDirectoryNamesFlag = 0 'False
    '	zcs.convertLFtoCRLFFlag = 0 'False
    '	zcs.addCommentFlag = 0 'False
    '	zcs.lpszComment = 0 'NULL
    '	zcs.afterDateFlag = 0 'False
    '	zcs.lpszDate = 0 'False
    '	zcs.oldAsLatestFlag = 0 'NULL
    '	zcs.includeOnlyFollowingFlag = 0 'False
    '	zcs.lpszIncludeFollowing = 0 'NULL
    '	zcs.noDirectoryEntriesFlag = 0 'False
    '	zcs.includeSysHiddenFlag = 0 'False
    '	zcs.excludeFollowingFlag = 0 'False
    '	zcs.lpszExcludeFollowing = 0 'NULL
    '	zcs.encryptFlag = 0 'False
    '	zcs.lpszEncryptCode = 0 'NULL
    '	zcs.lpMessageDisplay = 0 'NULL
    '	zcs.lpMessageDisplayData = 0 'NULL
    '	zcs.wMultiVolControl = 0 'NO Multi-Volume
    '	zcs.wZipSubOptions = 0 'NO sub options

    '	statusBlk.hMinorStat = 0
    '	statusBlk.hMajorStat = 0
    '	statusBlk.hMsgDispZip = 0
    '	statusBlk.hMsgDispUnZip = 0

    '	vbdzBlk.szZIPFile.Value = ""
    '	vbdzBlk.szItemList.Value = ""
    '	vbdzBlk.szTempPath.Value = ""
    '	vbdzBlk.szComment.Value = ""
    '	vbdzBlk.szDate.Value = ""
    '	vbdzBlk.szIncludeFollowing.Value = ""
    '	vbdzBlk.szExcludeFollowing.Value = ""
    '	vbdzBlk.szStoreSuffixes.Value = ""
    '	vbdzBlk.szEncryptCode.Value = ""

    '	' added for rev 3.00

    '	vbdzBlk.szExtProgTitle.Value = ""

    '	zcs.lResv1 = 0
    '	zcs.lResv2 = 0
    '	zcs.lpRenameProc = 0
    '	zcs.lpRenameUserData = 0
    '	zcs.lpszExtProgTitle = 0
    '	zcs.lpMemBlock = 0
    '	zcs.lMemBlockSize = 0

    'End Sub
    Public Function ValidZIPFile(ByRef szTestFile As String, ByRef bZIPFile As Boolean) As Integer
        'Dim result As Integer = 1
        'Try
        '    bZIPFile = Ionic.Zip.ZipFile.IsZipFile(szTestFile)
        '    If bZIPFile Then
        '        Using zip As Ionic.Zip.ZipFile = Ionic.Zip.ZipFile.Read(szTestFile)
        '            result = zip.Entries.Count
        '        End Using
        '    End If
        'Catch ex As Exception
        '    result = 0
        'End Try
        'Return result
        'm_lReturn = ZipCheck(sFilename:=sFilename, bZipped:=bZipped)
        Dim sExt As String = ""
        sExt = Path.GetExtension(szTestFile)
        If (sExt = ".zip" Or sExt = ".rar" Or sExt = ".7z") Then
            bZIPFile = True
        End If
    End Function

    '----------------------------------------------------------------------------------------
    'Here's an example of a simple call into the UNZIP DLL
    '
    'Returns the Count of zip files in the ZIP File as a validity check
    ' See ucs.returnCount for number of ZIP files that are ZIPPED into the ZIP file
    '
    '	Public Function ValidZIPFile(ByRef szTestFile As String, ByRef bZIPFile As Boolean) As Integer
    '		Dim result As Integer = 0
    '		Dim response As Integer
    '		Dim ucs As New UNZIPCMDSTRUCT

    '		result = False


    '		Try 
    '			'sj 18/10/2002 - start
    '			If Not gPMFunctions.FileExists(sFile:=szTestFile) Then
    '				'If Dir$(szTestFile) = "" Then
    '				'sj 18/10/2002 - end
    '				' RDC 20122000 no msgboxes in business comps.
    '				'        MsgBox "File: " & szTestFile & " does not exist", vbOKOnly, "ValidZIPFile"
    '				Return result
    '			End If




    '			InitUNZIPCmdStruct(ucs)

    '			vbduzBlk.szZIPFile.Value = szTestFile
    '			ucs.function_Renamed = UNZIP_COUNTALLZIPMEMBERS
    '			ucs.quietFlag = True

    '			Try 

    '				response = dunzipVB(ucs, vbduzBlk, zi, statusBlk)

    '			Catch 
    '			End Try




    '			bZIPFile = response = UE_OK


    '			Return True

    'errValidZIPFile: 

    '			' RDC 20122000 no msgboxes in business comps.
    '			'    MsgBox "Error: " & Err.Number & ": " & Err.Description & " " & szTestFile, _
    '			'vbOKOnly, "ValidZIPFile"
    '			Return False

    '		Catch exc As System.Exception
    '			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
    '		End Try

    '	End Function

    '	Private Function CountZippedFiles(ByRef szTestFile As String) As Integer
    '		Dim errCountZippedFiles As Boolean

    '		This returns the number of files contained in a singles zip file.

    '        Dim response As Integer
    '        Dim ucs As New UNZIPCMDSTRUCT




    '        InitUNZIPCmdStruct(ucs)
    '        vbduzBlk.szZIPFile.Value = szTestFile

    '        ucs.function_Renamed = UNZIP_COUNTALLZIPMEMBERS
    '        ucs.quietFlag = True


    '        Try
    '            response = dunzipVB(ucs, vbduzBlk, zi, statusBlk)

    '            Select Case Informations.Err().Number
    '                Case Is < 0
    '                    Conversion.ErrorToString(5)
    '                Case 1
    '                    GoTo errCountZippedFiles
    '            End Select


    '            If response = UE_OK Then
    '                Return ucs.returnCount
    '            Else
    '                Return False
    '            End If

    'errCountZippedFiles:
    '			 RDC 20122000 no msgboxes in business comps.
    '            MsgBox("Error: " & Err.Number & ": " & Err.Description, vbOKOnly, "CountZippedFiles")
    '            errCountZippedFiles = False

    '		Catch exc As System.Exception
    '			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
    '		End Try
    '	End Function


    '----------------------------------------------------------------------------------------
    'Here's an example of a simple call into the ZIP DLL
    '
    'This function simply performs a FIX operation of the target ZIP File
    '
    'UPGRADE_NOTE: (7001) The following declaration (FixZip) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function FixZip(ByRef szFixFile As String) As Integer
    'Dim zcs As New ZIPCMDSTRUCT
    '
    'InitZIPCmdStruct(zcs)
    '
    'zcs.function_Renamed = ZIP_ADD
    'zcs.FixFlag = True
    'vbdzBlk.szZIPFile.Value = szFixFile
    '
    'Dim response As Integer = dzipVB(zcs, vbdzBlk, statusBlk)
    '
    'If response <> ZE_OK Then
    'Return False
    'Else
    'Return True
    'End If
    '
    'End Function

    '----------------------------------------------------------------------------------------
    'This function simply adds the file specified by szFile to the
    'ZIP file specified by szZIP.  The raw error code from the DZIP.DLL is
    'returned.
    '
    'Public Function addFileToZIP(ByRef szZIP As String, ByRef szFile As String) As Integer

    '    Dim zcs As New ZIPCMDSTRUCT

    '    'Initialize the ZIP command structure
    '    InitZIPCmdStruct(zcs)

    '    'Here is where we determine if a Progress display is needed and
    '    'if Cancelling is allowed.
    '    'You can also set the title that will appear on the external progress
    '    'window's title bar
    '    zcs.wZipSubOptions += ZSO_EXTERNALPROG
    '    zcs.wZipSubOptions += ZSO_EXTPROGCANCEL
    '    vbdzBlk.szExtProgTitle.Value = "Performing ZIP Operation"

    '    ' Check for spaces in szFile - surround with quotes if necessary
    '    If szFile.IndexOf(" "c) >= 0 And szFile.Substring(0, 1) <> """" Then
    '        szFile = """" & szFile & """"
    '    End If

    '    zcs.function_Renamed = ZIP_ADD 'ADD files to the ZIP file
    '    vbdzBlk.szZIPFile.Value = szZIP 'The ZIP file name
    '    vbdzBlk.szItemList.Value = szFile 'The file to be added

    '    'Prepare for the ZIP function
    '    'Note:  vbdzBlk and statusBlk are Globals declared with the
    '    'VBDZIPBLOCK and VBSTATBLK types respectively

    '    'Actually perform the ZIP_ADD
    '    Return dzipVB(zcs, vbdzBlk, statusBlk)


    'End Function

    Public Function UnZipFile(ByRef sZipFileName As String, ByRef sDestDirectory As String) As Boolean

    Dim result As Boolean = False
    Dim destinationPath As String = String.Empty
    Try
        Using archive As ZipArchive = System.IO.Compression.ZipFile.OpenRead(sZipFileName)
          For Each entry As ZipArchiveEntry In archive.Entries
              Dim extractPath As String = Path.Combine(sDestDirectory, entry.FullName)
              If extractPath.StartsWith(sDestDirectory, StringComparison.Ordinal) Then
                  destinationPath = sDestDirectory + Path.DirectorySeparatorChar + Path.GetFileName(extractPath)
                  entry.ExtractToFile(destinationPath, overwrite:=True)
              End If

          Next
        End Using
            result = True
            Return result

    Catch excep As Exception
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Error extracting ZIP: " & excep.Message, vApp:=ACApp, vClass:="Business", vMethod:="UnZipFile")
            Return result
    End Try

    End Function
    '    Public Function UnZipFile(ByRef sFileIn As String, ByRef sFileOut As String) As Boolean
    '        Dim result As Boolean = False
    '        Dim iFileExtension As Integer

    '        'This extracts the contents of FileIn and then renames it to file out

    '        Dim response As Integer
    '        Dim ucs As New UNZIPCMDSTRUCT
    '        Dim sDirectory As String = "" 'Directory where the file will be placed
    '        Dim iTmp As Integer 'Temporary integer
    '        Dim sFileNameInfo As String = "" 'FileName and path of the extracted file
    '        Dim sFileUnZip As String = "" 'File name of the extracted file
    '        Dim sFile As String = "" 'File Name for the file once it is out
    '        Dim sDirectoryUnZip As String = "" 'Directory for where the file will be unzipped to
    '        Dim lIndexCount As Integer
    '        Dim bTempZip As Boolean 'tempzip directory created
    '        Dim sMissingDir, sMissingFile As String




    '        result = True
    '        bTempZip = False

    '        'Check if the filein and out are identical
    '        If sFileOut = sFileIn Then
    '            ' RDC 20122000 no msgboxes in business comps.
    '            '        MsgBox sFileIn$ & " is equivalent to " & sFileOut$, vbOKOnly, "UnZipFile"
    '            Return False
    '        End If

    '        'Pull out the name of the directory the file should be extracted to.
    '        iTmp = GetFileName(sFileOut, sFile, sDirectory)

    '        If Not iTmp Then
    '            ' RDC 20122000 no msgboxes in business comps.
    '            '        MsgBox "Error getting directory to extract to" & iTmp%, vbOKOnly, "UnZipFile"
    '            Return False
    '        End If

    '        'check that the zip file exists

    '        Try
    '            'sj 18/10/2002 - start
    '            If Not gPMFunctions.FileExists(sFile:=sFileIn) Then
    '                'If Dir$(sFileIn$) = "" Then
    '                'sj 18/10/2002 - end
    '                ' RDC 20122000 no msgboxes in business comps.
    '                '        MsgBox "Zipfile " & sFileIn$ & " does not exist", vbOKOnly, "UnZIpFIle"
    '                Return False
    '            End If




    '            'Now check that it only contains one file:
    '            lIndexCount = CountZippedFiles(sFileIn)

    '            'Any zip file contains two indexes for one file if the file is not held in the root of
    '            'the drive: one is the path, the other the file, if it is held as root then we only get the
    '            'file name
    '            If lIndexCount < 1 Or lIndexCount > 2 Then
    '                ' RDC 20122000 no msgboxes in business comps.
    '                '        MsgBox "Zipfile " & sFileIn$ & " contains " & lIndexCount & " indices.", vbOKOnly, "UnZipFile"
    '                Return False
    '            End If

    '            'We need to know the filename inside the zip file (this will include the path)-
    '            'this is the upper index of the two.
    '            iTmp = GetFileNameFromZIP(sFileIn, lIndexCount - 1, sFileNameInfo)

    '            If Not iTmp Then
    '                ' RDC 20122000 no msgboxes in business comps.
    '                '        MsgBox "Error getting file name from ZIP index:" & lIndexCount, vbOKOnly, "UnZipFile"
    '                Return False
    '            End If

    '            ' MS 16/07/01
    '            ' error occured due to file info not being stored correctly before zipping
    '            ' (filename inside the zip file)
    '            ' this is a work around to get the file name
    '            iFileExtension = (sFileNameInfo.IndexOf("."c) + 1)

    '            ' sFileUnZip$ is the file name given to the file as it was zipped
    '            ' the code below will work only if the sFile$ happened to be the same as the original
    '            ' (i.e sFileUnZip$).

    '            If sFileUnZip = "" And iFileExtension = 0 Then

    '                'MS 27/07/01
    '                'sj 18/10/2002 - start
    '                If Not gPMFunctions.FolderExists(sFolder:=sDirectory & "\Missing") Then
    '                    'If Dir$((sDirectory & "\Missing"), vbDirectory) = "" Then
    '                    'sj 18/10/2002 - end
    '                    Directory.CreateDirectory(sDirectory & "\Missing")
    '                End If

    '                sMissingDir = sDirectory & "\Missing"

    '                sFileNameInfo = sMissingDir & "\" & "MISSING.DOC"

    '            End If


    '            'Reduce to the file name - remove path from filenameinfo$
    '            If lIndexCount = 2 Then
    '                iTmp = GetFileName(sFileNameInfo, sFileUnZip, sDirectoryUnZip)

    '                If Not iTmp Then
    '                    ' RDC 20122000 no msgboxes in business comps.
    '                    '            MsgBox "Error getting file name from Zip Path:" & iTmp%, vbOKOnly, "UnZipFile"
    '                    Return False
    '                End If

    '                'JH 26081998 we need to find out if a file already
    '                'exists with this name - if it does, then we unzip
    '                'into a TempZip directory and move it back with the
    '                'correct name (held in sFileOut$)
    '                'sj 18/10/2002 - start
    '                If gPMFunctions.FileExists(sFile:=sDirectory & "\" & sFileUnZip) Then
    '                    'If Dir$((sDirectory$ & "\" & sFileUnZip$), vbNormal) <> "" Then
    '                    If Not gPMFunctions.FolderExists(sFolder:=sDirectory & "\TempZip") Then
    '                        'If Dir$((sDirectory$ & "\TempZip"), vbDirectory) = "" Then
    '                        'sj 18/10/2002 - end
    '                        Directory.CreateDirectory(sDirectory & "\TempZip")
    '                    End If
    '                    sDirectory = sDirectory & "\TempZip"
    '                    bTempZip = True
    '                End If

    '            Else
    '                sFileUnZip = sFileNameInfo
    '            End If

    '            InitUNZIPCmdStruct(ucs)

    '            vbduzBlk.szZIPFile.Value = sFileIn 'Name of Zip file to be unzipped
    '            vbduzBlk.szDestination.Value = sDirectory 'Location to be unzipped to
    '            vbduzBlk.szFileSpec.Value = "*.*" 'extract everything

    '            'MS 27/07/01
    '            'if file is missing then use the missing dir to unzip to
    '            If sFileUnZip = "MISSING.DOC" Then
    '                vbduzBlk.szDestination.Value = sMissingDir
    '            End If

    '            ucs.function_Renamed = UNZIP_EXTRACT
    '            'ucs.bDiagnostic = True

    '            'Unzip all files
    '            response = dunzipVB(ucs, vbduzBlk, zi, statusBlk)

    '            If response = UE_OK Then
    '                result = True
    '            Else
    '                ' RDC 20122000 no msgboxes in business comps.
    '                '        MsgBox "Error UnZipping File:" & response, vbOKOnly, "UnZipFile"
    '                result = False
    '            End If

    '            'MS 27/07/01
    '            ' The filename  zi.szFileName in the .zip file is  missing!
    '            ' this is a fix in order to set the filename to the one just unzipped
    '            ' this fixes everythig in the sense when being zipped up again the valid name is retained
    '            If sFileUnZip = "MISSING.DOC" Then

    '                'get the DOC file there should be one there
    '                sMissingFile = FileSystem.Dir(sMissingDir & "\*.*", FileAttribute.Normal)

    '                'now copy from missing dir to main dir
    '                If FileSystem.Dir(sDirectory & "\" & sMissingFile, FileAttribute.Normal) <> "" Then
    '                    File.Delete(sDirectory & "\" & sMissingFile)
    '                End If

    '                File.Copy(sMissingDir & "\" & sMissingFile, sDirectory & "\" & sMissingFile)

    '                'now kill temp file
    '                File.Delete(sMissingDir & "\" & sMissingFile)

    '                'remove temp dir
    '                'sj 18/10/2002 - start
    '                If gPMFunctions.FolderExists(sFolder:=sMissingDir) Then
    '                    'If Dir$(sMissingDir, vbDirectory) <> "" Then
    '                    'sj 18/10/2002 - end
    '                    Directory.Delete(sMissingDir)
    '                End If

    '                sFileUnZip = sMissingFile

    '            End If

    '            FileSystem.Rename(sDirectory & "\" & sFileUnZip, sFileOut)

    '            'JH 26081998 delete the TempZip directory
    '            If bTempZip Then
    '                Directory.Delete(sDirectory)
    '            End If

    '            Return result

    'errUnZipFile:

    '            ' RDC 20122000 no msgboxes in business comps.
    '            '    MsgBox "Error unzipping file " & sFileIn$ & ": " & Err.Number & ":" & Err.Description, vbOKOnly, "UnZipFile"
    '            Return False

    '        Catch exc As System.Exception
    '            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
    '        End Try

    '    End Function

    'Private Function GetFileNameFromZIP(ByRef sZipFileName As String, ByRef lIndex As Integer, ByRef sFileName As String) As Boolean
    '    Dim result As Boolean = False
    '    Dim response As gPMConstants.PMEReturnCode
    '    Dim sFile, sDir As String
    '    Dim ucs As New UNZIPCMDSTRUCT

    '    Try

    '        result = True

    '        InitUNZIPCmdStruct(ucs)

    '        vbduzBlk.szZIPFile.Value = sZipFileName 'Name of Zip file to be unzipped
    '        ucs.Index = lIndex 'Location to be unzipped to

    '        ucs.function_Renamed = UNZIP_GETINDEXEDZIPINFO

    '        response = CType(dunzipVB(ucs, vbduzBlk, zi, statusBlk), gPMConstants.PMEReturnCode)


    '        If response <> gPMConstants.PMEReturnCode.PMFalse Then
    '            result = False
    '            ' RDC 20122000 no msgboxes in business comps.
    '            '        MsgBox "Failed to extract name from: " & sZipFileName, vbOKOnly, "GetFileNameFromZIP"
    '        End If

    '        sFileName = zi.szFileName.Value

    '        'Remove the directory path if there is one
    '        For iTmp As Integer = sFileName.Length To 1 Step -1
    '            If Strings.Asc(sFileName.Substring(iTmp - 1, 1)(0)) <> 0 Then
    '                sFileName = sFileName.Substring(0, iTmp)
    '                Exit For
    '            End If
    '        Next iTmp

    '        Return result

    '    Catch



    '        ' RDC 20122000 no msgboxes in business comps.
    '        '    MsgBox "Error " & Err.Number & ":" & Err.Description, vbOKOnly, "GetFileNameFromZIP"
    '        Return False
    '    End Try
    'End Function

    Public Function ZipFile(ByRef sFileIn As String, ByRef sFileOut As String) As Boolean

        Dim sTempPath As String
        Dim result As Boolean = False
        Dim sTempFullPath As String
        Try
            Dim zipfileInfo As New Ionic.Zip.ZipFile

            If m_bNoDirectory = True Then
                sTempPath = System.IO.Path.GetTempPath & Guid.NewGuid.ToString
                sTempFullPath = sTempPath & "\" & Path.GetFileName(sFileIn)
                If System.IO.Directory.Exists(sTempPath) = False Then
                    System.IO.Directory.CreateDirectory(sTempPath)
                End If

                File.Copy(sFileIn, sTempFullPath)
                System.IO.Compression.ZipFile.CreateFromDirectory(sTempPath, sFileOut)

            Else
                Using archive As ZipArchive = System.IO.Compression.ZipFile.Open(sFileOut, ZipArchiveMode.Create)

                    Dim sSep As String() = {":\"}
                    Dim sSTR() As String = sFileIn.Split(sSep, StringSplitOptions.None)
                    If sSTR.Count > 1 Then
                        archive.CreateEntryFromFile(sFileIn, sSTR(1))
                    Else
                        archive.CreateEntryFromFile(sFileIn, Path.GetFileName(sFileIn))
                    End If
                End Using
            End If

            'zipfileInfo.Dispose()
            result = True
            Return result
        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError,
                sMsg:="ZipFile failed: " & ex.Message & " | Input: " & sFileIn & " | Output: " & sFileOut,
                vApp:=ACApp, vClass:="Business", vMethod:="ZipFile",
                vErrNo:=0, vErrDesc:=ex.ToString(), excep:=ex)
            Return result
        End Try
    End Function
    'changed function to use ionic.zip dll
    Public Function addFileToZIP(ByRef szZIP As String, ByRef szFile As String) As Boolean
        Dim result As Boolean = False
        Try
            Dim zipfileInfo As New Ionic.Zip.ZipFile
            zipfileInfo = Ionic.Zip.ZipFile.Read(szZIP)
            zipfileInfo.UpdateFile(szFile)
            zipfileInfo.Save(szZIP)
            zipfileInfo.Dispose()
            result = True
            Return result
        Catch
            Return result
        End Try
    End Function
    '    Public Function ZipFile(ByVal sFileIn As String, ByRef sFileOut As String) As gPMConstants.PMEReturnCode


    '        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
    '        Dim zcs As New ZIPCMDSTRUCT
    '        Dim response As Integer

    '        'This function assumes that sFileIn and sFileOut both contain a single file/path
    '        'If these contain spaces they will be checked, encapsulated in quotes and then
    '        'zipped




    '        'Initialize the ZIP command structure

    '        'JH280998 first set the temporary filename
    '        response = funGetTempName(sFileIn)

    '        'check response
    '        If Not response Then
    '            ' RDC 20122000 no msgboxes in business comps.
    '            '        MsgBox "Error " & response, vbOKOnly, "ZipFile"
    '            Return response
    '        End If

    '        'reset response
    '        response = UE_OK

    '        InitZIPCmdStruct(zcs)

    '        'Here is where we determine if a Progress display is needed and
    '        'if Cancelling is allowed.
    '        'You can also set the title that will appear on the external progress
    '        'window's title bar
    '        'zcs.wZipSubOptions = zcs.wZipSubOptions + ZSO_EXTERNALPROG
    '        'zcs.wZipSubOptions = zcs.wZipSubOptions + ZSO_EXTPROGCANCEL
    '        'vbdzBlk.szExtProgTitle = "Performing ZIP Operation"

    '        Try
    '            'Check if the filein and out are identical
    '            If sFileOut = sFileIn Then
    '                ' RDC 20122000 no msgboxes in business comps.
    '                '        MsgBox sFileIn & " is equivalent to " & sFileOut, vbOKOnly, "ZipFile"
    '                Return CType(False, gPMConstants.PMEReturnCode)
    '            End If

    '            'Check for the files

    '            ' RAG060600
    '            ' Ignore file already exists check if in appending mode
    '            If Not m_bAllowAppend Then
    '                If gPMFunctions.FileExists(sFile:=sFileOut) Then
    '                    'If Dir$(sFileOut, vbNormal) <> "" Then
    '                    'The zip file already exists
    '                    ' RDC 20122000 no msgboxes in business comps.
    '                    '            MsgBox "Zip File " & sFileOut & " already exists", vbOKOnly, "ZipFile"
    '                    Return gPMConstants.PMEReturnCode.PMError
    '                End If
    '            End If

    '            'sj 18/10/2002 - start
    '            If Not gPMFunctions.FileExists(sFile:=sFileIn) Then
    '                'If Dir$(sFileIn, vbNormal) = "" Then
    '                'sj 18/10/2002 - end
    '                'The File to be compressed does not exist
    '                ' RDC 20122000 no msgboxes in business comps.
    '                '        MsgBox "Source File " & sFileIn & " does not exist", vbOKOnly, "ZipFile"
    '                Return gPMConstants.PMEReturnCode.PMError
    '            End If

    '            'Check for Spaces in sFileIn - surround with quotes if necessary
    '            If sFileIn.IndexOf(" "c) >= 0 And sFileIn.Substring(0, 1) <> """" Then
    '                sFileIn = """" & sFileIn & """"
    '            End If

    '            'Prepare for the ZIP function
    '            'Note:  vbdzBlk and statusBlk are Globals declared with the
    '            'VBDZIPBLOCK and VBSTATBLK types respectively

    '            zcs.function_Renamed = ZIP_ADD 'ADD files to the ZIP file
    '            vbdzBlk.szZIPFile.Value = sFileOut 'The ZIP file name
    '            vbdzBlk.szItemList.Value = sFileIn 'The file to be added
    '            vbdzBlk.szTempPath.Value = g_sTempPath 'JH280998 the temporary path

    '            If m_bNoDirectory Then
    '                zcs.noDirectoryNamesFlag = 1
    '            End If

    '            'Actually perform the ZIP_ADD
    '            response = dzipVB(zcs, vbdzBlk, statusBlk)

    '            If response = UE_OK Then
    '                result = CType(True, gPMConstants.PMEReturnCode)
    '            Else
    '                ' RDC 20122000 no msgboxes in business comps.
    '                '        MsgBox "Error Zipping file:" & response, vbOKOnly, "ZipFile"
    '                result = CType(False, gPMConstants.PMEReturnCode)
    '            End If

    '            'JH280998 then delete the temp directory
    '            Directory.Delete(g_sTempPath)

    '            Return result

    'errZipFile:

    '            ' RDC 20122000 out, foul msgbox, for this is a business object!
    '            '    MsgBox "Error " & Err.Number & ":" & Err.Description, vbOKOnly, "ZipFile"
    '            Return Informations.Err().Number

    '        Catch exc As System.Exception
    '            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
    '        End Try

    '    End Function



    Private Function GetFileName(ByRef sFullPath As String, ByRef sFile As String, ByRef sDir As String) As Integer

        Dim result As Integer = 0
        Dim i, iLen, iSlashPos As Integer
        Const iMinPathLen As Integer = 3
        'Taken from General Functions Module, this strips a full path in to a file and directory
        'Returns true of ok false if not


        result = True

        sFullPath = sFullPath.Trim()
        sFile = ""
        sDir = ""
        'get path length
        iLen = sFullPath.Length

        'find directory delimiter
        For i = iLen To 1 Step -1
            If (sFullPath.Substring(i - 1, 1) = "\") Or (sFullPath.Substring(i - 1, 1) = "/") Then

                iSlashPos = i

                Exit For
            End If
        Next i

        'check it's reasonable
        If (i < iMinPathLen) Or (i = iLen) Then
            'error

            ' Log Error Message
            ' RDC 20122000 no msgboxes in business comps.
            '        MsgBox "Failed to get file name from '" & sFullPath$ & "'", vbOKOnly, "GetFileName"

            Return False

        End If

        'Pick out directory and file name
        sDir = sFullPath.Substring(0, iSlashPos - 1)
        sFile = sFullPath.Substring(sFullPath.Length - (iLen - iSlashPos))

        Return result


    End Function

    ' ***************************************************************** '
    ' Name: funGetTempName
    '
    ' Description:
    'JH280998 - setting temporary filename from FileIn path
    '
    '
    ' ***************************************************************** '
    Private Function funGetTempName(ByRef sPathIn As String) As Integer

        Dim result As Integer = 0
        Dim sTempFile As String = ""
        Dim sTempDir As String = ""


        result = True

        'split the path into filename and directory
        result = GetFileName(sPathIn, sTempFile, sTempDir)

        'take the name of the directory and add 'temp'
        g_sTempPath = sTempDir & "\Temp\"

        Return result


    End Function


    ' This is a generic method to unzip a number of files (according
    ' to sFileSpec$) into a given directory from a given zipfile
    ' CL190299
    ' RDC 04092000 Additional optional parm: bNoDirectoryNamesFlag
    ' This instructs zipper to preserve/not preserve original folders
    Public Function UnZipFiles(ByRef sZipFileName As String, ByRef sDestDirectory As String, ByRef sFileSpec As String, Optional ByRef bNoDirectoryNamesFlag As Boolean = False) As Boolean

        '    Dim result As Boolean = False
        '    Dim ucs As New UNZIPCMDSTRUCT

        '    Dim response As Integer

        '    Try


        '    InitUNZIPCmdStruct(ucs)

        '    vbduzBlk.szZIPFile.Value = sZipFileName ' Name of Zip file to be unzipped
        '    vbduzBlk.szDestination.Value = sDestDirectory ' Location to be unzipped to
        '    vbduzBlk.szFileSpec.Value = sFileSpec ' *.* to extract everything

        '    ucs.function_Renamed = UNZIP_EXTRACT
        '    ucs.recurseFlag = True ' recurse all zipped sub folders
        '    ' RDC 04092000 new function parm passed to noDirectoryNamesFlag
        '    ucs.noDirectoryNamesFlag = bNoDirectoryNamesFlag ' want the unzipping process to preserve folder hierachy?
        '    'ucs.bDiagnostic = True

        '    response = dunzipVB(ucs, vbduzBlk, zi, statusBlk) ' do it

        '    If response = UE_OK Then
        '        result = True
        '    End If

        '    Return result

        '    Catch



        'Return result
        '    End Try

    End Function



    Public Function ZipFileIonic(ByRef sZipFileName As String, ByRef sDestDirectory As String) As Boolean

        Dim result As Boolean = False
        'Try
        '    Dim unzipfileInfo As New Ionic.Zip.ZipFile
        '    unzipfileInfo = Ionic.Zip.ZipFile.Read(sZipFileName)
        '    unzipfileInfo.ExtractAll(sDestDirectory, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently)
        '    Dim dirInfo As New DirectoryInfo(sDestDirectory)
        '    unzipfileInfo.Dispose()
        '    result = True
        '    Return result
        'Catch
        '    Return result
        'End Try
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: CompressData
    '
    ' Description:
    ' RDC 26072001 new method for GEM2/GIS
    '
    ' ***************************************************************** '
    Public Function CompressData(ByRef TheData() As Byte) As Integer

        Dim result As Integer = 0
        Dim lResult As Integer
        'Allocate memory for byte array
        Dim lBufferSize, iL, iU As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_OriginalSize = 0
            m_CompressedSize = 0

            GetBounds(TheData, iL, iU)

            If iU <= iL Then
                ' nothing to compress
                Return result
            End If

            ' Store the original size of this data:
            m_OriginalSize = iU - iL + 1

            ' Prepare the area to compress into.
            ' Ensure we have sufficient space for the worst possible case (no
            ' compression plus additional space for the compression info):
            lBufferSize = m_OriginalSize
            lBufferSize = ToSafeInteger(lBufferSize + (lBufferSize * 0.01) + 12)
            Dim bTempBuffer(lBufferSize - 1) As Byte

            'Compress byte array (data):
            Dim handle As GCHandle = GCHandle.Alloc(bTempBuffer, GCHandleType.Pinned)
            Dim handle2 As GCHandle = GCHandle.Alloc(lBufferSize, GCHandleType.Pinned)
            Dim handle3 As GCHandle = GCHandle.Alloc(TheData, GCHandleType.Pinned)
            Try
                Dim tmpPtr3 As IntPtr = New IntPtr(handle3.AddrOfPinnedObject().ToInt32() + Marshal.SizeOf(TheData(iL)) * iL)
                Dim tmpPtr2 As IntPtr = handle2.AddrOfPinnedObject()
                Dim tmpPtr As IntPtr = New IntPtr(handle.AddrOfPinnedObject().ToInt32() + Marshal.SizeOf(bTempBuffer(0)) * 0)
                lResult = compress(tmpPtr, tmpPtr2, tmpPtr3, m_OriginalSize)
                lBufferSize = Marshal.ReadInt32(tmpPtr2)
            Finally
                handle.Free()
                handle2.Free()
                handle3.Free()
            End Try

            ' Result is an error code
            If lResult <> ZLIB_NOERROR Then
                ' error
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' lBufferSize will have been set by zlib:
            m_CompressedSize = lBufferSize

            ' If we got data back:
            If m_CompressedSize <= 0 Then
                Erase TheData
            Else
                'Truncate to actual compressed size
                ReDim Preserve TheData(lBufferSize - 1)
                ' Return data in buffer:
                Dim handle4 As GCHandle = GCHandle.Alloc(TheData, GCHandleType.Pinned)
                Dim handle5 As GCHandle = GCHandle.Alloc(bTempBuffer, GCHandleType.Pinned)
                Try
                    Dim tmpPtr5 As IntPtr = New IntPtr(handle5.AddrOfPinnedObject().ToInt32() + Marshal.SizeOf(bTempBuffer(0)) * 0)
                    Dim tmpPtr4 As IntPtr = New IntPtr(handle4.AddrOfPinnedObject().ToInt32() + Marshal.SizeOf(TheData(0)) * 0)
                    CopyMemory(tmpPtr4, tmpPtr5, lBufferSize)
                Finally
                    handle4.Free()
                    handle5.Free()
                End Try
            End If

            'Set properties
            m_CompressedSize = lBufferSize

            'Cleanup
            Erase bTempBuffer

            Return result

        Catch



            ' Failed
            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: CompressString
    '
    ' Description:
    ' RDC 26072001 new method for GEM2/GIS
    '
    ' ***************************************************************** '
    Public Function CompressString(ByRef TheString As String) As Integer

        Dim result As Integer = 0
        Dim lResult, lCmpSize As Integer
        Dim sTBuff As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_CompressedSize = 0
            m_OriginalSize = TheString.Length

            'Allocate string space for the buffers

            lCmpSize = m_OriginalSize
            ' RDC 29102001 allocate more space to output buffer
            ' lCmpSize = lCmpSize + (lCmpSize * 0.01) + 12
            lCmpSize = ToSafeInteger(lCmpSize + (lCmpSize * 0.1) + 12)
            sTBuff = New String(Strings.ChrW(0), lCmpSize)

            'Compress string (temporary string buffer) data
            Dim tmpPtr As IntPtr = Marshal.StringToHGlobalAnsi(sTBuff)
            Dim handle2 As GCHandle = GCHandle.Alloc(lCmpSize, GCHandleType.Pinned)
            Dim tmpPtr3 As IntPtr = Marshal.StringToHGlobalAnsi(TheString)
            Try
                Dim tmpPtr2 As IntPtr = handle2.AddrOfPinnedObject()
                lResult = compress(tmpPtr, tmpPtr2, tmpPtr3, TheString.Length)
                TheString = Marshal.PtrToStringAnsi(tmpPtr3)
                lCmpSize = Marshal.ReadInt32(tmpPtr2)
                sTBuff = Marshal.PtrToStringAnsi(tmpPtr)
            Finally
                Marshal.FreeHGlobal(tmpPtr)
                handle2.Free()
                Marshal.FreeHGlobal(tmpPtr3)
            End Try

            If lResult <> ZLIB_NOERROR Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Crop the string and set it to the actual string.
            TheString = sTBuff.Substring(0, lCmpSize)

            'Set compressed size of string.
            m_CompressedSize = lCmpSize

            'Cleanup
            sTBuff = ""

            Return result

        Catch



            ' failed
            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: DecompressData
    '
    ' Description:
    ' RDC 26072001 new method for GEM2/GIS
    '
    ' ***************************************************************** '
    Public Function DecompressData(ByRef TheData() As Byte, ByRef OrigSize As Integer) As Integer

        Dim result As Integer = 0
        Dim lResult, lBufferSize, iL, iU As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_OriginalSize = 0
            m_CompressedSize = 0

            GetBounds(TheData, iL, iU)

            If iU <= iL Then
                ' nothing to do
                Return result
            End If

            m_OriginalSize = 0
            m_CompressedSize = iU - iL + 1

            lBufferSize = OrigSize + 1
            Dim bTempBuffer(lBufferSize - 1) As Byte

            'Decompress data
            Dim handle As GCHandle = GCHandle.Alloc(bTempBuffer, GCHandleType.Pinned)
            Dim handle2 As GCHandle = GCHandle.Alloc(lBufferSize, GCHandleType.Pinned)
            Dim handle3 As GCHandle = GCHandle.Alloc(TheData, GCHandleType.Pinned)
            Try
                Dim tmpPtr3 As IntPtr = New IntPtr(handle3.AddrOfPinnedObject().ToInt32() + Marshal.SizeOf(TheData(0)) * 0)
                Dim tmpPtr2 As IntPtr = handle2.AddrOfPinnedObject()
                Dim tmpPtr As IntPtr = New IntPtr(handle.AddrOfPinnedObject().ToInt32() + Marshal.SizeOf(bTempBuffer(0)) * 0)
                lResult = uncompress(tmpPtr, tmpPtr2, tmpPtr3, TheData.GetUpperBound(0) + 1)
                lBufferSize = Marshal.ReadInt32(tmpPtr2)
            Finally
                handle.Free()
                handle2.Free()
                handle3.Free()
            End Try

            'Reset properties
            If lResult <> ZLIB_NOERROR Then
                ' error
                m_CompressedSize = 0
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_OriginalSize = lBufferSize

            'Truncate buffer to compressed size
            ReDim Preserve TheData(lBufferSize - 1)
            Dim handle4 As GCHandle = GCHandle.Alloc(TheData, GCHandleType.Pinned)
            Dim handle5 As GCHandle = GCHandle.Alloc(bTempBuffer, GCHandleType.Pinned)
            Try
                Dim tmpPtr5 As IntPtr = New IntPtr(handle5.AddrOfPinnedObject().ToInt32() + Marshal.SizeOf(bTempBuffer(0)) * 0)
                Dim tmpPtr4 As IntPtr = New IntPtr(handle4.AddrOfPinnedObject().ToInt32() + Marshal.SizeOf(TheData(0)) * 0)
                CopyMemory(tmpPtr4, tmpPtr5, lBufferSize)
            Finally
                handle4.Free()
                handle5.Free()
            End Try

            Return result

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function


    ' ***************************************************************** '
    ' Name: DecompressString
    '
    ' Description:
    ' RDC 26072001 new method for GEM2/GIS
    '
    ' ***************************************************************** '
    Public Function DecompressString(ByRef TheString As String, ByRef OrigSize As Integer) As Integer

        Dim result As Integer = 0
        Dim lResult As Integer

        'Allocate string space
        Dim lCmpSize As Integer
        Dim sTBuff As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_CompressedSize = TheString.Length
            m_OriginalSize = 0

            sTBuff = New String(Strings.ChrW(0), OrigSize + 1)
            lCmpSize = sTBuff.Length

            'Decompress
            Dim tmpPtr As IntPtr = Marshal.StringToHGlobalAnsi(sTBuff)
            Dim handle2 As GCHandle = GCHandle.Alloc(lCmpSize, GCHandleType.Pinned)
            Dim tmpPtr3 As IntPtr = Marshal.StringToHGlobalAnsi(TheString)
            Try
                Dim tmpPtr2 As IntPtr = handle2.AddrOfPinnedObject()
                lResult = uncompress(tmpPtr, tmpPtr2, tmpPtr3, m_CompressedSize)
                TheString = Marshal.PtrToStringAnsi(tmpPtr3)
                lCmpSize = Marshal.ReadInt32(tmpPtr2)
                sTBuff = Marshal.PtrToStringAnsi(tmpPtr)
            Finally
                Marshal.FreeHGlobal(tmpPtr)
                handle2.Free()
                Marshal.FreeHGlobal(tmpPtr3)
            End Try

            'Make string the size of the uncompressed string (If Problem returnd by the uncompressed)
            TheString = sTBuff.Substring(0, lCmpSize)
            m_OriginalSize = lCmpSize

            If lResult <> ZLIB_NOERROR Then
                ' Error
                m_CompressedSize = 0
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Make string the size of the uncompressed string
            TheString = sTBuff.Substring(0, lCmpSize)
            m_OriginalSize = lCmpSize

            Return result

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: GetBounds
    '
    ' Description:
    ' RDC 26072001 new method for GEM2/GIS
    '
    ' ***************************************************************** '
    Private Sub GetBounds(ByRef TheData() As Byte, ByRef iL As Integer, ByRef iU As Integer)

        ' irritating issue with ubound & lbound
        iL = TheData.GetLowerBound(0)
        If Informations.Err().Number = 0 Then
            iU = TheData.GetUpperBound(0)
        Else
            iL = 0 : iU = 0
        End If

    End Sub
End Class

