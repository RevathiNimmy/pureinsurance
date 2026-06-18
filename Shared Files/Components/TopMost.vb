Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
<System.Runtime.InteropServices.ProgId("TopMost_NET.TopMost")> _
 Public Module TopMost
	Public Const SWP_NOMOVE As Integer = 2
	Public Const SWP_NOSIZE As Integer = 1
	Public Const FLAGS As Integer = SWP_NOMOVE Or SWP_NOSIZE
	Public Const HWND_TOPMOST As Integer = -1
	Public Const HWND_NOTOPMOST As Integer = -2
	
	
	Declare Function SetWindowPos Lib "user32" (ByVal hwnd As Integer, ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer
	
	
	Public Function SetTopmost(ByRef frmForm As Form) As Boolean
		
		
		Dim iReturn As Integer = SetWindowPos(frmForm.Handle.ToInt32(), HWND_TOPMOST, 0, 0, 0, 0, FLAGS)
		
		If iReturn = 0 Then
			Return False
		Else
			Return True
		End If
		
	End Function
	
	Public Function ClearTopmost(ByRef frmForm As Form) As Boolean
		
		
		Dim iReturn As Integer = SetWindowPos(frmForm.Handle.ToInt32(), HWND_NOTOPMOST, 0, 0, 0, 0, FLAGS)
		
		If iReturn = 0 Then
			Return False
		Else
			Return True
		End If
		
	End Function
End Module