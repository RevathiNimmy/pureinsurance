Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports System
Imports System.Windows.Forms
Friend Partial Class Form1
	Inherits System.Windows.Forms.Form
	
	Private Declare Function OpenClipboard Lib "user32" (ByVal hwnd As Integer) As Integer
	Private Declare Function CloseClipboard Lib "user32" () As Integer
	
	Private oWordApp As Object 'refers to Word Application object
	Private oCurrentDoc As Object 'refers to Word Document object
	Private mOCRetVal As Integer 'return value of calling the OpenClipboard api function
	
	Private Sub Command1_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Command1.Click
		'FramerControl1.Toolbars = False
		'FramerControl1.Titlebar = False
		'FramerControl1.Open(Document:="c:\Doc1.doc", ReadOnly:=True)

		'  Obtain objects for automation:
		'oCurrentDoc = FramerControl1.ActiveDocument 'returns Word.Document object

		oWordApp = oCurrentDoc.Application 'returns Word.Application object
		
		' Optional: Turn off the display of the rulers:

		oWordApp.ActiveWindow.DisplayRulers = False
		
		' Optional: Display in Normal View:

		oWordApp.ActiveWindow.view.Type = 1 'wdNormalView=1
		
		' Optional: Disable the popup menu when right-clicking in a document:

		oCurrentDoc.CommandBars("Text").Enabled = False
		
		' Prevent the document from being edited:
		Try  'in case it is already protected
			'oCurrentDoc.Protect 1, , "mypassword"

			oCurrentDoc.Protect(Type:=2)
		
		Catch 
		End Try
	End Sub
	

	Private Sub Form1_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		' Disable the Clipboard while this form is open:
		mOCRetVal = OpenClipboard(0)
		If mOCRetVal = 0 Then 'OpenClipboard failed. Can't disable clipboard.
			MessageBox.Show("The Clipboard is locked by another application. Unable to view this form.", Application.ProductName)
			' Close this form, which will end the application if no other forms are open:
			Me.Close()
		End If
	End Sub
	
	Private Sub Form1_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		' Re-enable the Clipboard when we close this form,
		'  so other apps can now use the clipboard!
		If mOCRetVal <> 0 Then CloseClipboard()
		
		' Re-enable the DisplayRulers Property,
		'  since this is a permanent setting:

		If oWordApp.GetType().Name = "Application" Then 'valid instance exists:

			oWordApp.ActiveWindow.DisplayRulers = True
		End If
		MemoryHelper.ReleaseMemory()
	End Sub
End Class