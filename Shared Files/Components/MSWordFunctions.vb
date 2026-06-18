Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports Word = Microsoft.Office.Interop.Word
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Menu
Module MSWordFunctions
	
	
	' Insert Bookmark for MS Word App
	Public Function MSWordInsertBookmark(ByRef sName As String, ByRef sText As String, ByRef sLoop1 As String, ByRef sLoop2 As String, ByRef sLoop3 As String, ByRef sLoop4 As String) As Integer
		Dim m_lReturn As gPMConstants.PMEReturnCode
		
		Dim oBookmark As Word.Bookmark
		Dim iCounter As Integer
		Dim sBMName As String = ""
		Dim IsBookmark As Boolean
		'Dim sName As String
		Dim oLoopBookmark, oEndLoopBookmark As Word.Bookmark
		Dim bCreateLoops As Boolean
		Dim sTemp As String = ""
		
		On Error GoTo Err_InsertBookmark
		
		iCounter = 0
		
		'Don't use with, there's a memory leak if you exit without going through the end with,
		'and we definitely want to get out of here sometimes
		'    With g_oCallingApp
		
		'Just loop1 for now - worry about others when we need to
		If sLoop1 <> "" Then
			sTemp = LoopTag & Separator & sLoop1
			
			On Error Resume Next
			

			oLoopBookmark = g_oCallingApp.ActiveDocument.Bookmarks.Item(sTemp)
			
			sTemp = EndLoopTag & Separator & sLoop1
			

			oEndLoopBookmark = g_oCallingApp.ActiveDocument.Bookmarks.Item(sTemp)
			
			'Back to normal
			On Error GoTo Err_InsertBookmark
			
			'Also for now, worry only about one of each type of loop...
			'Possibly prevent insertion if outside the real loop?
			
			If Not (oLoopBookmark Is Nothing) Then

				If oLoopBookmark.Start > g_oCallingApp.ActiveWindow.Selection.Start Then
					Exit Function
				End If
			End If
			
			If Not (oEndLoopBookmark Is Nothing) Then

				If oEndLoopBookmark.Start < g_oCallingApp.ActiveWindow.Selection.Start Then
					Exit Function
				End If
			End If
			
			If oLoopBookmark Is Nothing Then
				sTemp = LoopTag & Separator & sLoop1

				g_oCallingApp.ActiveWindow.Selection.InsertAfter(sTemp)

				oLoopBookmark = g_oCallingApp.ActiveDocument.Bookmarks.Add(sTemp)
				m_lReturn = CType(MSWordInsertText(Strings.Chr(13) & Strings.Chr(10)), gPMConstants.PMEReturnCode)
				
			End If
			
		End If
		
		' Insert the description of the bookmark into the document

		g_oCallingApp.ActiveWindow.Selection.InsertAfter(sText)
		
		' Increment counter until the bookmark is unique
		Do 
			iCounter += 1
			
			sBMName = sName
			
			'If iCounter% > 1 Then
			' Set the name of bookmark to include counter at end
			sBMName = sName & "_" & CStr(iCounter)
			'End If
			
			' See if the Bookmark already exists :
			' If not, an error will occur (See error trap)
			IsBookmark = True

			oBookmark = g_oCallingApp.ActiveDocument.Bookmarks(sBMName)
			
		Loop Until Not IsBookmark
		
		' Insert the bookmark

		oBookmark = g_oCallingApp.ActiveDocument.Bookmarks.Add(sBMName)
		
		' Set insertion point to after bookmark

		g_oCallingApp.ActiveWindow.Selection.Start = g_oCallingApp.ActiveWindow.Selection.End
		
		If sLoop1 <> "" Then
			If oEndLoopBookmark Is Nothing Then
				m_lReturn = CType(MSWordInsertText(Strings.Chr(13) & Strings.Chr(10)), gPMConstants.PMEReturnCode)
				sTemp = EndLoopTag & Separator & sLoop1

				g_oCallingApp.ActiveWindow.Selection.InsertAfter(sTemp)

				oLoopBookmark = g_oCallingApp.ActiveDocument.Bookmarks.Add(sTemp)

				g_oCallingApp.ActiveWindow.Selection.Start = g_oCallingApp.ActiveWindow.Selection.End
			End If
		End If
		
		' Re-Activate the application

		g_oCallingApp.Activate()
		
		'    End With
		
		' Return Successful
		
		Return gPMConstants.PMEReturnCode.PMTrue
		
		
Err_InsertBookmark: 
		
		' If error is caused by Bookmark not found in collection,
		' continue to say bookmark name is not yet used.
		If Information.Err().Number = 5941 Then
			IsBookmark = False
			Resume Next
		End If
		
        'Modified by Deepak Sharma on 4/20/2010 3:08:53 PM refer developer guide no. 30 (No Solutions)
        'DisplayError(Information.Err().Number, Information.Err().Description, "MSWordInsertBookmark")
		
		Exit Function
		
	End Function
	
	' Insert Text for MS Word App
	Public Function MSWordInsertText(ByRef sText As String) As Integer
		
		Dim iCounter As Integer
		Dim sBMName As String = ""
		Dim IsText As Boolean
		
		Try 
			
			iCounter = 0
			
			With g_oCallingApp
				
				' Insert the description of the Text into the document

				.ActiveWindow.Selection.InsertAfter(sText)
				
				' Set insertion point to after Text

				.ActiveWindow.Selection.Start = .ActiveWindow.Selection.End
				
				' Re-Activate the application

				.Activate()
				
			End With
			
			' Return Successful
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			
			' If error is caused by Text not found in collection,
			' continue to say Text name is not yet used.
			If Information.Err().Number = 5941 Then
				IsText = False


			End If
			
			DisplayError(Information.Err().Number, excep.Message, "MSWordInsertText")
			
			Exit Function
			
		End Try
	End Function
	
	' Get list of Bookmarks from an MSWord App
	Public Function MSWordGetBookmarks(ByRef sBMlist() As String) As Integer
		
		Dim result As Integer = 0
		Dim lCount As Integer
		
		Try 
			
			result = -1
			
			lCount = 0
			

			For	Each oBookmark As Object In g_oCallingApp.ActiveDocument.Bookmarks
				
				ReDim Preserve sBMlist(lCount)
				

				sBMlist(lCount) = oBookmark.Name
				
				lCount += 1
				
			Next oBookmark
			
			
			Return lCount
		
		Catch excep As System.Exception
            'Modified by Deepak Sharma on 4/20/2010 3:08:19 PM refer developer guide no. 30 (No Solutions)
            'DisplayError(Information.Err().Number, excep.Message, "MSWordGetBookmarks")
			
			Return result
		End Try
	End Function
	
	
	' Delete a Bookmark from an MSWord App
	Public Function MSWordDeleteBookmark(ByRef sName As String) As Integer
		
		Dim result As Integer = 0
		result = gPMConstants.PMEReturnCode.PMTrue
		
		Try 
			
			Dim a As Word.Application
			
			With g_oCallingApp
				

				.ActiveDocument.Bookmarks(sName).Select()

				.ActiveWindow.Selection.Delete()

				.ActiveDocument.Bookmarks(sName).Delete()

				.Activate()
				
			End With
			
			Return result
		
		Catch excep As System.Exception
			
			
            'Modified by Deepak Sharma on 4/20/2010 3:08:46 PM refer developer guide no. 30 (No Solutions)
            'DisplayError(Information.Err().Number, excep.Message, "MSWordDeleteBookmark")
			
			Return result
			
		End Try
	End Function
	
	' Goto a Bookmark in an MSWord App
	Public Function MSWordGotoBookmark(ByRef sName As String) As Integer
		
		Dim result As Integer = 0
		result = gPMConstants.PMEReturnCode.PMTrue
		
		Try 
			
			With g_oCallingApp
				

				.ActiveDocument.Bookmarks(sName).Select()

				.Activate()
				
			End With
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			DisplayError(Information.Err().Number, excep.Message, "MSWordGotoBookmark")
			
			Return result
			
		End Try
	End Function
    'Modified by Deepak Sharma on 4/20/2010 2:07:53 PM refer developer guide no. 29(No Solution)
    'Shared Sub New()
    '    MainModule.JustForInvokeMain()
    'End Sub
End Module