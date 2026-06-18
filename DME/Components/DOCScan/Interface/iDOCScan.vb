Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports SharedFiles

Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: {TodaysDate}
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iDOCScan"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
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
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	
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
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	'Public g_iSourceID As Integer
	'Public g_iLanguageID As Integer
	
	' Public instance of the object manager.
	Public g_oObjectManager As bObjectManager.ObjectManager
	Private m_lReturn As Integer
	
	' Public instance of the splash class
	Public g_oSplash As iDOCSplash.Interface_Renamed
	
	' Public instance of the viewbatch object
	Public g_oViewBatch As Object
	
	' Offsets for dats
	Public iDocDateOffset As Integer
	Public iDocExpiryOffset As Integer
	
	' Current Folder Number
	Public lFolderNumber As Integer
	
	' Number of pages scanned this session
	Public g_lPagesScanned As Integer
	
	' Number of documents scanned this session
	Public g_lDocumentsScanned As Integer
	
	' The current page number in the document
	Public g_lCurrentPage As Integer
	
	Public g_iDocNum As Integer
	
	' The Current Password
	Public sCurrentPassword As String = ""
	
	' The current document number
	Public iCurrentDocument As Integer
	
	' Run mode of the scanstation
	Public bIsStandAlone As Boolean
	
	' Name of the root directory for the scans
	Public g_sScanDirectory As String = ""
	
	' Name of the image that is currently in the scan view
	Public g_sCurrentFilename As String = ""
	
	' Name of the last filename for appending mode (mulitpage tiff)
	Public g_sLastFilename As String = ""
	
	' Path to where the pages are currently being saved ("eg. Doc0 etc...")
	Public g_sCurrentPath As String = ""
	
	' Used to store any errors that occur during scanning (stores nError)
	Public iScanError As gPMConstants.PMEReturnCode
	
	' Keyword IDs
	Public vKeywords As Object
	'Global iCurrentKeywordID As Integer
	
	Public sCurrentAnnotation As New FixedLengthString(50)
	
	' Current user and log level
	Public sCurrentUserName As String = ""
	Public iCurrentLogLevel As Integer
	
	Public lCurrentFolderNum As Integer
	
	' Extension for tif files
	Public Const DEFAULTTIFEXTENSION As String = ".TIF"
	
	Public Const SCANNEDDOCTYPE As String = "I"
	Public Const SCANNEDPAGETYPE As String = "TIF"
	
	Public m_oDOCScan As Object
	
	' Byte sizes of pages
	Public g_vCurrentPageInfo() As Object
	
	' Annotations for the documents
	Public vAnnotations As Object
	Public lNumberAnnotations As Integer
	
	' Has the document been saved yet
	Public g_bDocumentSaved As Boolean
	
	' Taken from the Scan(...) parameters
	Public sParentFolder As String = ""
	Public sCurrentFolder As String = ""
	
	' Minimum byte size an image must be to be "valid"
	Public Const MINIMUM_IMAGE_SIZE As Integer = 2000
	
	' Continue or finish
	Public iScanOption As Integer
	Public Const SCAN_CONTINUE As Integer = 1
	Public Const SCAN_FINISH As Integer = 2
	
	' Used to scan the pages until finish
	Public bScanLoop As Boolean
	
	Public bScanBatch As Boolean
	
	Public g_lPagesScannedBatch As Integer
	
	' Display values
	Public Const PMDOCDISPLAYYES As String = "Yes"
	Public Const PMDOCDISPLAYNO As String = "No"
	Public Const PMDOCDISPLAYYESTIMED As String = "Yes - Timed"
	
	Public Const PMDOCDEFAULTTIME As String = "3.0"
	
	' DPI
	Public Const PMDOCDPI75 As String = "75"
	Public Const PMDOCDPI100 As String = "100"
	Public Const PMDOCDPI150 As String = "150"
	Public Const PMDOCDPI200 As String = "200"
	Public Const PMDOCDPI240 As String = "240"
	Public Const PMDOCDPI300 As String = "300"
	Public Const PMDOCDPI400 As String = "400"
	Public Const PMDOCDPI600 As String = "600"
	Public Const PMDOCDPIPANEL As String = "Panel"
	
	' Scan mode
	Public Const PMDOCMODEPANEL As String = "Panel"
	Public Const PMDOCMODELINE As String = "Line"
	Public Const PMDOCMODEPHOTO As String = "Photo"
	Public Const PMDOCMODEMIXED As String = "Mixed"
	
	' Contrast
	Public Const PMDOCCONTRAST1 As String = "1"
	Public Const PMDOCCONTRAST2 As String = "2"
	Public Const PMDOCCONTRAST3 As String = "3"
	Public Const PMDOCCONTRAST4 As String = "4"
	Public Const PMDOCCONTRAST5 As String = "5"
	Public Const PMDOCCONTRAST6 As String = "6"
	Public Const PMDOCCONTRAST7 As String = "7"
	Public Const PMDOCCONTRAST8 As String = "8"
	Public Const PMDOCCONTRASTAUTO As String = "Auto"
	Public Const PMDOCCONTRASTPANEL As String = "Panel"
	
	' Colour modes
	Public Const PMDOCCOLMILLION As String = "Millions of Colours"
	Public Const PMDOCCOL256COL As String = "256 Colours"
	Public Const PMDOCCOL256GRAY As String = "256 Grays"
	Public Const PMDOCCOL16GRAY As String = "16 Grays"
	Public Const PMDOCCOLBI As String = "Bitonal"
	
	'SOB051199 Scanner Page Size
	Public Const PMDOCSIZEA0 As String = "A0"
	Public Const PMDOCSIZEA1 As String = "A1"
	Public Const PMDOCSIZEA2 As String = "A2"
	Public Const PMDOCSIZEA3 As String = "A3"
	Public Const PMDOCSIZEA4 As String = "A4"
	Public Const PMDOCSIZEA5 As String = "A5"
	Public Const PMDOCSIZEB As String = "A6"
	Public Const PMDOCSIZELETTER As String = "Letter"
	Public Const PMDOCSIZEB0 As String = "B0"
	Public Const PMDOCSIZEB1 As String = "B1"
	Public Const PMDOCSIZEB2 As String = "B2"
	Public Const PMDOCSIZEB3 As String = "B3"
	Public Const PMDOCSIZEB4 As String = "B4"
	Public Const PMDOCSIZEB5 As String = "B5"
	Public Const PMDOCSIZEB6 As String = "B6"
	Public Const PMDOCSIZELEGAL As String = "Legal"
	Public Const PMDOCSIZEPANEL As String = "Panel"
	Public Const PMDOCSIZECOUPON As String = "Coupon"
	Public Const PMDOCSIZEPERSONAL As String = "Personal"
	Public Const PMDOCSIZEBUSINESS As String = "Business"
	Public Const PMDOCSIZEMAX As String = "Max"
	Public Const PMDOCSIZEMIN As String = "Min"
	
	Public Const SCROLLBARWIDTH As Integer = 80
	
	Public bAppClosing As Boolean
	
	'ND 201000
	Public g_bSBOInstalled As Boolean
	
	'JH021198 settings for registry
	
	' Image options
	Public Structure ImageOptions_Type
		Dim Contrast As DOCGeneralFunc.Setting_Type
		Dim ScanMode As DOCGeneralFunc.Setting_Type
		Dim DPI As DOCGeneralFunc.Setting_Type
		Dim Colours As DOCGeneralFunc.Setting_Type
		Dim ScanSize As DOCGeneralFunc.Setting_Type
        Public Shared Function CreateInstance() As ImageOptions_Type
            Dim result As New ImageOptions_Type
            result.Contrast = Setting_Type.CreateInstance()
            result.ScanMode = Setting_Type.CreateInstance()
            result.DPI = Setting_Type.CreateInstance()
            result.Colours = Setting_Type.CreateInstance()
            result.ScanSize = Setting_Type.CreateInstance()
            Return result
        End Function
	End Structure
	
	' Scan options
	Public Structure ScanOptions_Type
		Dim Confirm As DOCGeneralFunc.Setting_Type
		Dim Flatbed As DOCGeneralFunc.Setting_Type
		Dim Batch As DOCGeneralFunc.Setting_Type
		Public Shared Function CreateInstance() As ScanOptions_Type
			Dim result As New ScanOptions_Type
			result.Confirm = Setting_Type.CreateInstance()
			result.Flatbed = Setting_Type.CreateInstance()
			result.Batch = Setting_Type.CreateInstance()
			Return result
		End Function
	End Structure
	
	' Advanced options
	Public Structure AdvOptions_Type
		Dim AutoDocName As DOCGeneralFunc.Setting_Type
		Dim Display As DOCGeneralFunc.Setting_Type
		Dim DisplayTime As DOCGeneralFunc.Setting_Type
		Dim MultiTiff As DOCGeneralFunc.Setting_Type
		Dim TabSelected As DOCGeneralFunc.Setting_Type
		Public Shared Function CreateInstance() As AdvOptions_Type
			Dim result As New AdvOptions_Type
			result.AutoDocName = Setting_Type.CreateInstance()
			result.Display = Setting_Type.CreateInstance()
			result.DisplayTime = Setting_Type.CreateInstance()
			result.MultiTiff = Setting_Type.CreateInstance()
			result.TabSelected = Setting_Type.CreateInstance()
			Return result
		End Function
	End Structure
	
	' Sirius options
	Public Structure SiriusOptions_Type
		Dim TaskEnabled As DOCGeneralFunc.Setting_Type
		Public Shared Function CreateInstance() As SiriusOptions_Type
			Dim result As New SiriusOptions_Type
			result.TaskEnabled = Setting_Type.CreateInstance()
			Return result
		End Function
	End Structure
	
	' declare all the options
	Public g_ImageOptions As ImageOptions_Type = ImageOptions_Type.CreateInstance()
	Public g_ScanOptions As ScanOptions_Type = ScanOptions_Type.CreateInstance()
	Public g_AdvOptions As AdvOptions_Type = AdvOptions_Type.CreateInstance()
	Public g_SiriusOptions As SiriusOptions_Type = SiriusOptions_Type.CreateInstance()
	
	' CTAF 20030818
	Public Const ACBlankFileName As String = "BlankPage" & DEFAULTTIFEXTENSION
	
	Sub Main_Renamed()
		
	End Sub
End Module