Option Strict Off
Option Explicit On
Imports System
Imports System.Text
Imports System.Windows.Forms
Imports SharedFiles
Imports Microsoft.Office.Interop



Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 17/02/1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	Public Const ACApp As String = "iFieldManager"
	
	Public Const MSWordApp As String = "MSWord"
	
	Public Const InsertFileCaption As String = "Insert File ..."
	
	Public Const PMField As Integer = 1
	Public Const PMLoop As Integer = 2
	
	'RWH(25/01/2001)
	Public Const ACMaxRiskLoops As Integer = 4
	
	Public Const DbTag As String = "DB"
	Public Const TableTag As String = "TBL"
	Public Const FieldTag As String = "FLD"
	Public Const LoopTag As String = "LOOP"
	Public Const EndLoopTag As String = "ENDLOOP"
	Public Const FileTag As String = "FILE"
	Public Const QuestionTag As String = "KEY0"
	
	'Thinh Nguyen 10/10/2002 start - mandatory question tag
	Public Const MandQuestionTag As String = "KEY0M"
	'Thinh Nguyen 10/10/2002 end - mandatory question tag
	
	Public Const Separator As String = "_"
	Public Const ClauseTag As String = "CL" 'RWH(08/08/2000) RSAIB Process 12.
	Public Const RiskLoopTag As String = "RSKLOOP" 'RWH(12/09/2000) RSAIB Process 28.
	Public Const RiskHeaderTag As String = "RSKHEADER" 'RWH(12/09/2000) RSAIB Process 28.
	Public Const StandardWordingsTag As String = "STANDARDWORDINGS" 'RWH(23/04/2001) RSAIB Process 28.
    Public Const StandardWordingNPTag As String = "STANDARDWORDINGSNP"
    Public Const StandardWordingsDescTag As String = "STANDARDWORDINGS_DESC"
	Public Const StandardWordingsCodeTag As String = "STANDARDWORDINGS_CODE"
	Public Const DocumentSplitTag As String = "DOCUMENTSPLIT"

	'RWH(27/07/2000)
	Public Const g_sFIELD_START_MARKER As String = "<@"
	Public Const g_sFIELD_END_MARKER As String = "@>"
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
	
    ' Reference to the Calling Application
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oCallingApp As Object
	
    ' Name of the Calling App
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sCallingAppName As String = ""
	

	Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	Public Const ScreenHelpID As Integer = 20
	
	Public Const SubDocumentTag As String = "SUBDOC" ' RAM20050104 - Added to Support Sub Document
	
	Public Const ACFields_FieldName As Integer = 0
	Public Const ACFields_SQL As Integer = 1
	Public Const ACFields_ColumnName As Integer = 2
	Public Const ACFields_ColumnType As Integer = 3
	Public Const ACFields_MainGroup As Integer = 4
	Public Const ACFields_SubGroup As Integer = 5
	Public Const ACFields_DisplayName As Integer = 6
	Public Const ACFields_IsDisplayed As Integer = 7
	Public Const ACFields_Loop1 As Integer = 8
	Public Const ACFields_Loop2 As Integer = 9
	Public Const ACFields_Loop3 As Integer = 10
	Public Const ACFields_Loop4 As Integer = 11
	Public Const ACFields_ProductFamily As Integer = 12
	Public Const ACFields_DataModel As Integer = 13
	Public Const ACFields_PropertyID As Integer = 14
	Public Const ACFields_SubGroup2 As Integer = 15
	Public Const ACFields_SubGroup3 As Integer = 16
	Public Const ACFields_SubGroup4 As Integer = 17
	Public Const ACFields_SpecialsType As Integer = 18
	
	
	' Entry point for the Object

	Public Sub Main()
		
		'frmInterface.Show 1
		
	End Sub
	
	' Insert a bookmark into the Document
	Public Function InsertBookmark(ByRef sName As String, ByRef sText As String, ByRef sLoop1 As String, ByRef sLoop2 As String, ByRef sLoop3 As String, ByRef sLoop4 As String) As Integer
		
		
		Dim result As Integer = 0
		Select Case g_sCallingAppName
			Case MSWordApp
				' Microsoft Word
                result = MSWordInsertBookmark(sName:=sName, sText:=sText, sLoop1:=sLoop1, sLoop2:=sLoop2, sLoop3:=sLoop3, sLoop4:=sLoop4)

			Case Else
				
		End Select
		
		Return result
	End Function
	
	' Insert text into the Document
	Public Function InsertText(ByRef sText As String) As Integer
		
		
		Dim result As Integer = 0
		Select Case g_sCallingAppName
			Case MSWordApp
				' Microsoft Word
				result = MSWordInsertText(sText)
				
			Case Else
				
		End Select
		
		Return result
	End Function
	
	
	
	Public Function GetBookMarks(ByRef sBMlist() As String) As Integer
		
		
		Dim result As Integer = 0
		Select Case g_sCallingAppName
			Case MSWordApp
				' Microsoft Word
                result = MSWordGetBookmarks(sBMlist)


			Case Else
				
		End Select
		
		Return result
	End Function
	
	Public Function GotoBookmark(ByRef sName As String) As Integer
		
		
		Dim result As Integer = 0
		Select Case g_sCallingAppName
			Case MSWordApp
				' Microsoft Word
				result = MSWordGotoBookmark(sName)
				
			Case Else
				
		End Select
		
		Return gPMConstants.PMEReturnCode.PMTrue
		
	End Function
	
	Public Function DeleteBookmark(ByRef sName As String) As Integer
		
		
		Dim result As Integer = 0
		Select Case g_sCallingAppName
			Case MSWordApp
				' Microsoft Word
				result = MSWordDeleteBookmark(sName)
				
			Case Else
				
		End Select
		
		Return gPMConstants.PMEReturnCode.PMTrue
		
	End Function
	
	
	Public Function RemoveSpaces(ByRef sInput As String) As String
		
		Dim sChar As String = ""
		Dim sOutput As New StringBuilder
		
		
		Try 
			
			sOutput = New StringBuilder("")
			
			For	Each sChar In sInput
				
				If sChar <> " " Then
					sOutput.Append(sChar)
				End If
				
			Next sChar
			
			
			Return sOutput.ToString()
		
		Catch 
			
			
			
			Return sInput
		End Try
		
	End Function
	
	Public Function InsertSpaces(ByRef sInput As String) As String
		
		Dim sChar As String = ""
		Dim sOutput As New StringBuilder
		
		
		Try 
			
			sOutput = New StringBuilder("")
			
			For i As Integer = 1 To sInput.Length
				
				sChar = sInput.Substring(i - 1, 1)
				
				If (sChar >= "A") And (sChar <= "Z") And (i > 1) Then
					sOutput.Append(" ")
				End If
				
				sOutput.Append(sChar)
			Next 
			
			
			Return sOutput.ToString()
		
		Catch 
			
			
			
			Return sInput
		End Try
		
	End Function
	
	Public Function DisplayError(ByRef lNumber As Integer, ByRef sDesc As String, ByRef sTitle As String) As Object
		
		MessageBox.Show("Error " & lNumber & " : " & sDesc, sTitle)
		
	End Function

	Sub New()
		Main()
	End Sub
	Sub JustForInvokeMain()
	End Sub
End Module