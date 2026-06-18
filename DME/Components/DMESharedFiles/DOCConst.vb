Option Strict Off
Option Explicit On
Imports System
Public Module DOCConst
    ' ***************************************************************** '
    ' Name: DOCConst
    '
    ' Date: 03/12/97
    '
    ' Description: DocuMaster general constants module. Contains all of
    '               the global constants needed to be included into
    '               DocuMaster application.
    '
    ' Edit History: JH021198 - added a load of constants for registry settings
    '
    ' JH050199 DOCHiddenSortColumn constant added for sorting by date
    '
    ' ***************************************************************** '


    'Who am I?
    Public Const DOCAppName As String = "DocuMaster"

    'View Modes of the document manger interface
    Public Const DOCViewModeMain As Integer = 1
    Public Const DOCViewModeFavourites As Integer = 2
    Public Const DOCViewModeBC As Integer = 3
    Public Const DOCViewModeFindResults As Integer = 4

    'Constants to identify between a folder node and document node
    Public Const DOCNode_Folder As Integer = 1
    Public Const DOCNode_Document As Integer = 2

    'constants for max document and folder and length
    Public Const DOCDoc_Name_Max As Integer = 50
    Public Const DOCFolder_Name_Max As Integer = 70
    Public Const DOCKeyword_Max As Integer = 30

    'JH051198 constant for maximum maxautoexpand
    Public Const DOCMaxMaxAutoExpand As Integer = 5000
    'constant for default maxautoexpand
    Public Const DOCDefaultMaxAutoExpand As Integer = 500
    'constant for default maxautoexpand for WAN
    Public Const DOCDefaultMaxAutoExpandWAN As Integer = 50


    'The first few chars of a node key contain various other info other than the
    'the folder/doc number. This indicates how many characters of info there are.
    Public Const DOCNodeKeyOffSet As Integer = 7

    'JH241298 name and path of help file
    Public Const DOCHelpFileName As String = "\DME.hlp"
    Public Const DOCHelpFilePath As String = "\common\help"

    ' Name of stand alone scan folder
    Public Const DOCDefaultScanFolder As String = "Scan Folder"

    Public Const DOCDefaultImageViewer As String = "Default Image Viewer"

    ' Name of cache
    Public Const DOCCacheName As String = "DocuMaster Cache"

    'Location of AVI Files directory (under the install directory)
    Public Const DOCAVIDir As String = "\common\misc"

    'Folder level - for backward compatibiliy, ie
    'a folder node with folder_level 1 was previously
    'a drawer in DocuMaster V2.
    Public Const DOCCabinet As Integer = 0
    Public Const DOCDrawer As Integer = 1
    Public Const DOCFolder As Integer = 2
    Public Const DOCDocument As Integer = 3

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' RAM20021205 : The above 4 Constants are to be replaced with the
    '               following constants (to support 5 Levels)
    '               Ref. NRMA Project Changes. Sirius Process No. 189 - Start
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Const DOCTypeDocument As String = "DOCUMENT"
    Public Const DOCTypeFolder As String = "FOLDER"

    Public Const DOCFolderLevelBranchDesc As String = "Branch"
    Public Const DOCFolderLevelClientDesc As String = "Client"
    Public Const DOCFolderLevelPolicyDesc As String = "Policy"
    Public Const DOCFolderLevelClaimDesc As String = "Claim"
    Public Const DOCFolderLevelLossScheduleDesc As String = "LossSchedule"
    Public Const DOCFolderLevelDocumentDesc As String = "Document"

    Public Const DOCFolderLevelBranch As Integer = 0
    Public Const DOCFolderLevelClient As Integer = 1
    Public Const DOCFolderLevelPolicy As Integer = 2
    Public Const DOCFolderLevelClaim As Integer = 3
    Public Const DOCFolderLevelLossSchedule As Integer = 4
    Public Const DOCFolderLevelDocument As Integer = 5

    Public Const DOCFolderLevelMAX As Integer = 5 ' ie. 0 To 5  (so 6 Levels, being 6th is Document)
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' RAM20021205 : NRMA Project Changes. Sirius Process No. 189 - END
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    'Document Types
    '090699SOB Added new DOC constants to have a full range of documents
    Public Const DOCUnknown As String = "Unknown Document Type"
    Public Const DOCImage As String = "TIF Image"
    Public Const DOCText As String = "Plain Text"
    Public Const DOCRTF As String = "RTF Document"
    Public Const DOCWRD As String = "Word Document" 'Ms Word
    Public Const DOCEXL As String = "Excel File" 'Excel
    Public Const DOCPWP As String = "Power Point" 'PowerPoint
    Public Const DOCACC As String = "Access Database" 'Access
    Public Const DOCHTM As String = "HTML Page" 'HTML
    Public Const DOCGIF As String = "GIF Image" 'GIF
    Public Const DOCJPG As String = "JPEG Image" 'JPEG
    Public Const DOCEML As String = "E-Mail Document" 'EML Email Doc
    Public Const DOCPDF As String = "Adobe Document" 'Adobe Documents
    Public Const DOCHLP As String = "Help File" 'Help Files
    Public Const DOCZIP As String = "Zip File" 'Zip Files

    '090699SOB Using Constants to cordinate document Types
    'File Types these should cornatate with Document Types
    'Longer term using a char in the database will be very restricting
    Public Const kDocFileTypeUnknown As String = "U" 'Unknown
    Public Const kDocFileTypeTIF As String = "I" 'TIF
    Public Const kDocFileTypeTXT As String = "T" 'TXT
    Public Const kDocFileTypeRTF As String = "W" 'RTF
    Public Const kDocFileTypeWRD As String = "D" 'Ms Word
    Public Const kDocFileTypeEXL As String = "X" 'Excel
    Public Const kDocFileTypePWP As String = "P" 'PowerPoint
    Public Const kDocFileTypeACC As String = "A" 'Access
    Public Const kDocFileTypeHTM As String = "H" 'HTML
    Public Const kDocFileTypeGIF As String = "G" 'GIF
    Public Const kDocFileTypeJPG As String = "J" 'JPEG
    Public Const kDocFileTypeEML As String = "M" 'EML Email Doc
    Public Const kDocFileTypePDF As String = "F" 'Adobe Documents
    Public Const kDocFileTypeHLP As String = "E" 'Help Files
    Public Const kDocFileTypeZIP As String = "Z" 'Zip Files


    'User defined type for storing key and texts of nodes selected for
    'moving
    Structure DOCNodes
        Dim Key As String
        Dim Text As String
        Public Shared Function CreateInstance() As DOCNodes
            Dim result As New DOCNodes
            result.Key = String.Empty
            result.Text = String.Empty
            Return result
        End Function
    End Structure

    'User defined type for storing external code ancestry of a folder
    Structure DOCExCodes
        Dim CabExCode As String
        Dim DrawExCode As String
        Dim FoldExCode As String
        Public Shared Function CreateInstance() As DOCExCodes
            Dim result As New DOCExCodes
            result.CabExCode = String.Empty
            result.DrawExCode = String.Empty
            result.FoldExCode = String.Empty
            Return result
        End Function
    End Structure

    'Various Splash Types
    Public Const DOCSplash_Copying As Integer = 1
    Public Const DOCSplash_Moving As Integer = 2
    Public Const DOCSplash_Deleting As Integer = 3
    Public Const DOCSplash_Retrieving As Integer = 4
    Public Const DOCSplash_Processing As Integer = 5
    Public Const DOCSplash_Message As Integer = 6

    'Volume ID and name of the server hard drive - always used by default
    Public Const DOCHD1_ID As Integer = 1
    Public Const DOCHD1_NAME As String = "DMS HD1"

    'Registry section names
    Public Const DOCScanSection As String = "Scan"
    Public Const DOCDaemonSection As String = "Daemon"
    Public Const DOCOptionsSection As String = "Options"
    Public Const DOCStartUpSection As String = "Start Up"

    'JH021198 added for scan options
    Public Const DOCScanOptionsSection As String = "Scan Options"

    'JH260399 added for multiple history roots
    Public Const DOCMultipleHistoryRootSection As String = "History Root Drives"

    'Registry key names
    Public Const DOCScanDirKey As String = "Scan Directory"
    Public Const DOCTimerIntervalKey As String = "Timer Interval"
    Public Const DOCHistoryRootKey As String = "History Root"
    Public Const DOCRunningKey As String = "Running"
    Public Const DOCCommitLockKey As String = "Commit Locked"
    Public Const DOCMaxFoldersKey As String = "Max Folders"
    Public Const DOCMaxFilterFoldersKey As String = "Max Filter Folders"

    'SOB230399 added for scan drivers
    Public Const DOCKofaxOrTwain As String = "Kofax Or Twain"
    Public Const DOCTwainSettingsKey As String = "Twain Settings" 'check this

    'JH051198 added for folder processing
    Public Const DOCMaxAutoExpandKey As String = "Auto-Expand Folders"
    Public Const DOCStartHome As String = "Start Home"
    Public Const DOCDisplayFoldersOnRightKey As String = "Display Folders On Right"
    Public Const DOCCacheLocationKey As String = "Cache Location"
    Public Const DOCScanToExternalWarning As String = "Scan to External Folder Warning"
    Public Const DOCMoveToNonFolderWarning As String = "Move to Non-V2 Folder Warning"
    Public Const DOCScanStationKey As String = "ScanStation"
    Public Const DOCFormWidthKey As String = "Form Width"
    Public Const DOCFormHeightKey As String = "Form Height"
    Public Const DOCSplitterVLeftKey As String = "Splitter V Left"
    Public Const DOCSplitterHTopKey As String = "Splitter H Top"
    Public Const DOCWindowStateKey As String = "Window State"
    Public Const DOCExtrasAnnotationsKey As String = "Extras Annotations"
    Public Const DOCExtrasKeywordsKey As String = "Extras Keywords"
    Public Const DOCDMEDIRKey As String = "DMEDIR"
    'WR77 Documaster Enhancements START
    Public Const DOCSplitterBCHTopKey As String = "Splitter BC H Top"
    'WR77 Documaster Enhancements END

    'JH281098 constants added for WAN and Word Optimise
    Public Const DOCWANOptimiseKey As String = "Optimise for WAN"
    Public Const DOCPrintWordKey As String = "Print using Word"
    Public Const DOCViewWordKey As String = "View using Word"

    'MS250900   constant added for auto fire up of keyword/annotation windows
    Public Const DOCVAutoKeyword As String = "Auto Keyword/Annotations"

    'JH021198 added for scan options
    Public Const DOCScanColoursKey As String = "Colours"
    Public Const DOCScanContrastKey As String = "Contrast"
    Public Const DOCScanModeKey As String = "Scan Mode"
    Public Const DOCScanDPIKey As String = "DPI"
    Public Const DOCScanConfirmKey As String = "Confirm"
    Public Const DOCScanFlatbedKey As String = "Flatbed"
    Public Const DOCScanBatchKey As String = "Batch"
    Public Const DOCScanAutoDocNameKey As String = "Auto Document Name"
    Public Const DOCScanMultiTiffKey As String = "Multi-Tiff"
    Public Const DOCScanDisplayKey As String = "Display"
    Public Const DOCScanDisplayTimeKey As String = "Display Time"
    Public Const DOCScanTabSelectedKey As String = "Tab Selected"
    Public Const DOCScanSizeKey As String = "Scan Size"
    Public Const DOCScanTaskEnabledKey As String = "Task Enabled" 'DN 15/05/01

    'JH260399 added for multiple history roots
    Public Const DOCMultipleHistoryRootKey As String = "Multiple History Roots"
    Public Const DOCMultipleHistoryNextDriveKey As String = "Next Drive"
    Public Const DOCMaximumHistoryRoots As Integer = 10

    'Constants for commit status
    Public Const DOCCommitStarted As Integer = 1
    Public Const DOCCommitCancelled As Integer = 2
    Public Const DOCCommitFinished As Integer = 3
    Public Const DOCCommitLocked As Integer = 4

    'Constants representing the 'Task' in the history database
    Public Const DOCADDCABINET As Integer = 1
    Public Const DOCDELCABINET As Integer = 2
    Public Const DOCMODCABINET As Integer = 3
    Public Const DOCADDDRAWER As Integer = 4
    Public Const DOCDELDRAWER As Integer = 5
    Public Const DOCMODDRAWER As Integer = 6
    Public Const DOCADDFOLDER As Integer = 7
    Public Const DOCDELFOLDER As Integer = 8
    Public Const DOCMODFOLDER As Integer = 9
    Public Const DOCADDDOCUMENT As Integer = 10
    Public Const DOCDELDOCUMENT As Integer = 11
    Public Const DOCMODDOCUMENT As Integer = 12

    'Constants for logging messages to the PMB API log - for backward compatibility
    'with DocuMaster 2
    Public Const LMSG As Integer = 1
    Public Const LERR As Integer = 2
    Public Const LLOG As Integer = 3
    Public Const LWRN As Integer = 4
    Public Const LDBG As Integer = 5

    ' JH050199 for sorting by date
    Public Const DOCHiddenSortColumn As Integer = 4
End Module