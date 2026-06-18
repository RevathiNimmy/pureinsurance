Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Imports System.Drawing.Printing
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 13/05/1998
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	'              SOB 01/06/99 Added OLE Doc Viewer using Web control
	'                           Must have OLE viewer installed on client
	' ***************************************************************** '
	
    Public objfrmParentMDI As frmParentMDI
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iDOCViewer"
	Const ACClass As String = "MainModule"
	
	' File Types
	Public Const ACFileTypeUnknown As Integer = 0
	Public Const ACFileTypeTIF As Integer = 1
	Public Const ACFileTypeTXT As Integer = 2
	Public Const ACFileTypeRTF As Integer = 3
	Public Const ACFileTypeWRD As Integer = 4 'Ms Word
	Public Const ACFileTypeEXL As Integer = 5 'Excel
	Public Const ACFileTypePWP As Integer = 6 'PowerPoint
	Public Const ACFileTypeACC As Integer = 7 'Access
	Public Const ACFileTypeHTM As Integer = 8 'HTML
	Public Const ACFileTypeGIF As Integer = 9 'GIF
	Public Const ACFileTypeJPG As Integer = 10 'JPEG
	Public Const ACFileTypeEML As Integer = 11 'EML Email Doc
	Public Const ACFileTypePDF As Integer = 12 'Adobe Documents
	Public Const ACFileTypeHLP As Integer = 13 'Help Files
	Public Const ACFileTypeZIP As Integer = 14 'Zip Files
	Public Const ACFileTypeBMP As Integer = 15 'Bit Map
	Public Const ACFileTypePNG As Integer = 16 'PNG Map


	' Viewer Types
	Public Const ACViewerTypeUnknown As Integer = 0
	Public Const ACViewerTypeTIF As Integer = 1
	Public Const ACViewerTypeRTF As Integer = 2
	Public Const ACViewerTypeTXT As Integer = 3 'TEXT
	Public Const ACViewerTypeWRD As Integer = 4 'Ms Word
	Public Const ACViewerTypeEXL As Integer = 5 'Excel
	Public Const ACViewerTypePWP As Integer = 6 'PowerPoint
	Public Const ACViewerTypeACC As Integer = 7 'Access
	Public Const ACViewerTypeHTM As Integer = 8 'HTML
	Public Const ACViewerTypeGIF As Integer = 9 'GIF
	Public Const ACViewerTypeJPG As Integer = 10 'JPEG
	Public Const ACViewerTypeEML As Integer = 11 'EML Email Doc
	Public Const ACViewerTypePDF As Integer = 12 'Adobe Documents
	Public Const ACViewerTypeHLP As Integer = 13 'Help Files
	Public Const ACViewerTypeZIP As Integer = 14 'Zip Files
	
	Public Const FIT_NONE As Integer = -1
	Public Const FIT_HEIGHT As Integer = 2
	Public Const FIT_WIDTH As Integer = 1
	Public Const FIT_SCREEN As Integer = 0
	
	Public Const MOUSE_NONE As Integer = 0
	Public Const MOUSE_MOVE As Integer = 1
	Public Const MOUSE_ZOOM As Integer = 2
	
	' RDC 22022005
	Public Const USER_IS_BRIEFCASE As Integer = -1000
	
	' For the combo box
	Public bDontUpdate As Boolean
	
	' Instance of iDOCInformation
	Public g_oDOCInformation As iDOCInformation.Interface_Renamed
	
	Public g_frmManager As Object
	
	' location of the cache
	Public g_sCachePath As String = ""
	
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
	' RAM20030428   : Introduced the following constants, which
	'                   are used by iPMBDocumentTemplate.
	'                 Ref. NRMA Project Changes. Document Issuance
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
	Public Const ACNormalMode As Integer = 0
	Public Const ACMergeMode As Integer = 1
	Public Const ACPrintMode As Integer = 2
	Public Const ACPrintSilentMode As Integer = 3
	Public Const ACSpoolDocMode As Integer = 4
	Public Const ACSpoolReportMode As Integer = 5
	' Public Const ACViewMode = 6  'duplicated from gSIRLibrary
	
	' Public source and language ID's from the Object Manager.
	' User ID
    Public g_iUserID As Integer
	' Username.
	Public g_sUsername As String = ""
	' Password.
	Public g_sPassword As New FixedLengthString(30)
	' Source ID
	Public g_iSourceID As Integer
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
	' RAM20030428   : END
	''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '33223
    'TODO
    Public mPrint As PrintDocument
	Public Const HWND_TOPMOST As Integer = -1
	Public Const HWND_TOP As Integer = 0
	Public Const SWP_NOMOVE As Integer = &H2s
	Public Const SWP_NOSIZE As Integer = &H1s
    Public g_oObjectManager As bObjectManager.ObjectManager
	'API to put a window on top of all screen 33223
	Public Declare Function SetWindowPos Lib "user32" (ByVal hwnd As Integer, ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer
	
	
	' CTAF 20031125 - Start
	Public ReadOnly Property Username() As String
		Get
			Return g_sUsername
		End Get
	End Property
	' CTAF 20031125 - End
	
	Public Function RefreshFormControl(ByVal v_sExceptionDocumentKey As String) As Integer
		
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Check that there isnt a form loaded with the document number
			For lLoop As Integer = 0 To Application.OpenForms.Count - 1
				' Make sure its an OLE child
				If TypeOf Application.OpenForms.Item(lLoop) Is frmChildOFF Then

                    'If Application.OpenForms.Item(lLoop).DocumentKey <> v_sExceptionDocumentKey Then
                    '	
                    'Application.OpenForms.Item(lLoop).FCOffice.Activate()
                    'End If
                End If
            Next
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshFormControl process failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshFormControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	'33223
	Public Function SetWinPos(ByRef lHWnd As Integer) As Boolean
		' Run the API SetWindowPos function
		Dim result As Boolean = False
		If SetWindowPos(lHWnd, HWND_TOP, 0, 0, 0, 0, SWP_NOMOVE + SWP_NOSIZE) Then
			' If the function is greater than 0 (FALSE) then the operation was successful.  Return a True for to indicate such.
			result = True ' True or false can be returned from this function, but not absolutely necessary
		End If
		Return result
	End Function
End Module
