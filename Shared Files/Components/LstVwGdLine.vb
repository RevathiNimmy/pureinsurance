Option Strict Off
Option Explicit On
Imports System
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Public Module modLst
	
	Public Const LVM_FIRST As Integer = &H1000s
	
	Public Const LVM_SETEXTENDEDLISTVIEWSTYLE As Integer = LVM_FIRST + 54
	
	Public Const LVM_GETEXTENDEDLISTVIEWSTYLE As Integer = LVM_FIRST + 55
	
	Public Const LVS_EX_GRIDLINES As Integer = &H1s
	
	Public Const ICC_LISTVIEW_CLASSES As Integer = &H1s
	'Public Const HDF_BITMAP_ON_RIGHT = &H1000
	
	Public Declare Sub InitCommonControls Lib "comctl32.dll" ()
	
	

	Public Declare Function InitCommonControlsEx Lib "comctl32.dll" (ByRef lpInitCtrls As tagINITCOMMONCONTROLSEX) As Boolean
	
	Public Declare Function SendMessageAny Lib "user32"  Alias "SendMessageA"(ByVal hWnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
	
	Public Declare Function SendMessageLong Lib "user32"  Alias "SendMessageA"(ByVal hWnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
	
	
	
	Public Declare Function SetWindowLong Lib "user32"  Alias "SetWindowLongA"(ByVal hWnd As Integer, ByVal nIndex As Integer, ByVal dwNewLong As Integer) As Integer
	
	
	
	Public Structure tagINITCOMMONCONTROLSEX
		Dim dwSize As Integer
		Dim dwICC As Integer
	End Structure
	
	
	
	Public Function InitComctl32(ByRef dwFlags As Integer) As Boolean
		
		Dim icc As New tagINITCOMMONCONTROLSEX
		
		Try 

			icc.dwSize = Marshal.SizeOf(icc)
			icc.dwICC = dwFlags
			
			'Call madeto API
			
			Return InitCommonControlsEx(icc)
		
		Catch 
			
			
			InitCommonControls()
		End Try
		
	End Function
	
	
	
	Public Sub Gridlines(ByRef ctrlLstVw As Control)
		SendMessageLong(ctrlLstVw.Handle.ToInt32(), LVM_SETEXTENDEDLISTVIEWSTYLE, 0, 1)
		
	End Sub
End Module