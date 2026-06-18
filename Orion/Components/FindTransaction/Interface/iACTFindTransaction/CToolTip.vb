Option Strict Off
Option Explicit On
Imports System
Imports System.Runtime.InteropServices
Friend NotInheritable Class CTooltip 
	'' Added into iACTFindTransaction project on 26/03/2004 by CJB (sourced by DD)
	
	'' Title: Adding multiline balloon tooltips to ListView items
	'' Description: This code can be used to create multiline balloon tooltips for ListView items.
	'' The code is based on the following simple idea.
	
	'' In the MouseMove event you need to check the index of the item under the mouse pointer, and
	'' if this item is changed, you simply redefine the text of the tooltip attached to the ListView
	'' control. Notice that you should destroy the tooltip if there is no any item under the mouse pointer.
	
	'' This simple idea can be used to create such tooltips for ListBox items, any grid control items and so on.
	
	
	Private Declare Sub InitCommonControls Lib "comctl32.dll" ()
	
	''Windows API Functions
	Private Declare Function CreateWindowEx Lib "user32"  Alias "CreateWindowExA"(ByVal dwExStyle As Integer, ByVal lpClassName As String, ByVal lpWindowName As String, ByVal dwStyle As Integer, ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal hWndParent As Integer, ByVal hMenu As Integer, ByVal hInstance As Integer, ByVal lpParam As Integer) As Integer
	Private Declare Function SendMessage Lib "user32"  Alias "SendMessageA"(ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
	Private Declare Function SendMessageLong Lib "user32"  Alias "SendMessageA"(ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
	Private Declare Function DestroyWindow Lib "user32" (ByVal hwnd As Integer) As Integer
	
	''Windows API Constants
	Private Const WM_USER As Integer = &H400s
	Private Const CW_USEDEFAULT As Integer = &H80000000
	
	''Windows API Types
	Private Structure RECT
		Dim Left As Integer
		Dim Top As Integer
		Dim Right As Integer
		Dim Bottom As Integer
	End Structure
	
	''Tooltip Window Constants
	Private Const TTS_NOPREFIX As Integer = &H2s
	Private Const TTF_TRANSPARENT As Integer = &H100s
	Private Const TTF_CENTERTIP As Integer = &H2s
	Private Const TTM_ADDTOOLA As Integer = (WM_USER + 4)
	Private Const TTM_ACTIVATE As Integer = WM_USER + 1
	Private Const TTM_UPDATETIPTEXTA As Integer = (WM_USER + 12)
	Private Const TTM_SETMAXTIPWIDTH As Integer = (WM_USER + 24)
	Private Const TTM_SETTIPBKCOLOR As Integer = (WM_USER + 19)
	Private Const TTM_SETTIPTEXTCOLOR As Integer = (WM_USER + 20)
	Private Const TTM_SETTITLE As Integer = (WM_USER + 32)
	Private Const TTS_BALLOON As Integer = &H40s
	Private Const TTS_ALWAYSTIP As Integer = &H1s
	Private Const TTF_SUBCLASS As Integer = &H10s
	Private Const TTF_IDISHWND As Integer = &H1s
	Private Const TTM_SETDELAYTIME As Integer = (WM_USER + 3)
	Private Const TTDT_AUTOPOP As Integer = 2
	Private Const TTDT_INITIAL As Integer = 3
	
	Private Const TOOLTIPS_CLASSA As String = "tooltips_class32"
	
	''Tooltip Window Types
	Private Structure TOOLINFO
		Dim lSize As Integer
		Dim lFlags As Integer
		Dim hwnd As Integer
		Dim lId As Integer
		Dim lpRect As RECT
		Dim hInstance As Integer
		Dim lpStr As String
		Dim lParam As Integer
		Public Shared Function CreateInstance() As TOOLINFO
			Dim result As New TOOLINFO
			result.lpStr = String.Empty
			Return result
		End Function
	End Structure
	
	Public Enum ttIconType
		TTNoIcon = 0
		TTIconInfo = 1
		TTIconWarning = 2
		TTIconError = 3
	End Enum
	
	Public Enum ttStyleEnum
		TTStandard
		TTBalloon
	End Enum
	
	'local variable(s) to hold property value(s)
	Private mvarBackColor As Integer
	Private mvarTitle As String = ""
	Private mvarForeColor As Integer
	Private mvarIcon As ttIconType
	Private mvarCentered As Boolean
	Private mvarStyle As ttStyleEnum
	Private mvarTipText As String = ""
	Private mvarVisibleTime As Integer = 5000
	Private mvarDelayTime As Integer = 250
	
	'private data
	Private m_lTTHwnd As Integer ' hwnd of the tooltip
	Private m_lParentHwnd As Integer ' hwnd of the window the tooltip attached to
	Private ti As TOOLINFO = TOOLINFO.CreateInstance()
	
	
	Public Property Style() As ttStyleEnum
		Get
			'used when retrieving value of a property, on the right side of an assignment.
			'Syntax: Debug.Print X.Style
			Return mvarStyle
		End Get
		Set(ByVal Value As ttStyleEnum)
			'used when assigning a value to the property, on the left side of an assignment.
			'Syntax: X.Style = 5
			mvarStyle = Value
		End Set
	End Property
	
	
	Public Property Centered() As Boolean
		Get
			'used when retrieving value of a property, on the right side of an assignment.
			'Syntax: Debug.Print X.Centered
			Return mvarCentered
		End Get
		Set(ByVal Value As Boolean)
			'used when assigning a value to the property, on the left side of an assignment.
			'Syntax: X.Centered = 5
			mvarCentered = Value
		End Set
	End Property
	
	
	Public Property Icon() As ttIconType
		Get
			Return mvarIcon
		End Get
		Set(ByVal Value As ttIconType)
			mvarIcon = Value

			If m_lTTHwnd <> 0 And Not String.IsNullOrEmpty(mvarTitle) And mvarIcon <> ttIconType.TTNoIcon Then
				Dim tmpPtr As IntPtr = Marshal.StringToHGlobalAnsi(mvarTitle)
				Try 
					SendMessage(m_lTTHwnd, CInt(TTM_SETTITLE), mvarIcon, tmpPtr)
					mvarTitle = Marshal.PtrToStringAnsi(tmpPtr)
				Finally 
					Marshal.FreeHGlobal(tmpPtr)
				End Try
			End If
		End Set
	End Property
	
	
	Public Property ForeColor() As Integer
		Get
			Return mvarForeColor
		End Get
		Set(ByVal Value As Integer)
			mvarForeColor = Value
			If m_lTTHwnd <> 0 Then
				Dim handle As GCHandle = GCHandle.Alloc(0, GCHandleType.Pinned)
				Try 
					Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()
					SendMessage(m_lTTHwnd, CInt(TTM_SETTIPTEXTCOLOR), mvarForeColor, tmpPtr)
				Finally 
					handle.Free()
				End Try
			End If
		End Set
	End Property
	
	
	Public Property Title() As String
        'Get
        '	Return ti.lpStr
        'End Get
        'Set(ByVal Value As String)
        '	mvarTitle = Value

        '	If m_lTTHwnd <> 0 And Not String.IsNullOrEmpty(mvarTitle) And mvarIcon <> ttIconType.TTNoIcon Then
        '		Dim tmpPtr As IntPtr = Marshal.StringToHGlobalAnsi(mvarTitle)
        '		Try 
        '			SendMessage(m_lTTHwnd, CInt(TTM_SETTITLE), mvarIcon, tmpPtr)
        '			mvarTitle = Marshal.PtrToStringAnsi(tmpPtr)
        '		Finally 
        '			Marshal.FreeHGlobal(tmpPtr)
        '		End Try
        '	End If
        'End Set
        Get
            Return mvarTitle
        End Get
        Set(ByVal Value As String)
            mvarTitle = Value
            
        End Set
	End Property
	
	
	Public Property BackColor() As Integer
		Get
			Return mvarBackColor
		End Get
		Set(ByVal Value As Integer)
			mvarBackColor = Value
            'If m_lTTHwnd <> 0 Then
            '	Dim handle As GCHandle = GCHandle.Alloc(0, GCHandleType.Pinned)
            '	Try 
            '		Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()
            '		SendMessage(m_lTTHwnd, CInt(TTM_SETTIPBKCOLOR), mvarBackColor, tmpPtr)
            '	Finally 
            '		handle.Free()
            '	End Try
            'End If
		End Set
	End Property
	
	
	Public Property TipText() As String
		Get
			Return mvarTipText
		End Get
		Set(ByVal Value As String)
			mvarTipText = Value
            'ti.lpStr = Value
            'If m_lTTHwnd <> 0 Then
            '	Dim handle As GCHandle = GCHandle.Alloc(ti, GCHandleType.Pinned)
            '	Try 
            '		Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()

            '		SendMessage(m_lTTHwnd, CInt(TTM_UPDATETIPTEXTA), 0, tmpPtr)
            '	Finally 
            '		handle.Free()
            '	End Try
            'End If
		End Set
	End Property
	
	
	Public Property VisibleTime() As Integer
		Get
			Return mvarVisibleTime
		End Get
		Set(ByVal Value As Integer)
			mvarVisibleTime = Value
		End Set
	End Property
	
	
	Public Property DelayTime() As Integer
		Get
			Return mvarDelayTime
		End Get
		Set(ByVal Value As Integer)
			mvarDelayTime = Value
		End Set
	End Property
	
	Public Function Create(ByVal ParentHwnd As Integer) As Boolean
		
		If m_lTTHwnd <> 0 Then
			DestroyWindow(m_lTTHwnd)
		End If
		
		m_lParentHwnd = ParentHwnd
		
		Dim lWinStyle As Integer = TTS_ALWAYSTIP Or TTS_NOPREFIX
		
		''create baloon style if desired
		If mvarStyle = ttStyleEnum.TTBalloon Then lWinStyle = lWinStyle Or TTS_BALLOON
		
		Dim handle As GCHandle = GCHandle.Alloc(0, GCHandleType.Pinned)
		Try 
			Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()
			m_lTTHwnd = CreateWindowEx(0, TOOLTIPS_CLASSA, Nothing, lWinStyle, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, 0, 0, VB6.GetHInstance().ToInt32(), tmpPtr)
		Finally 
			handle.Free()
		End Try
		
		''now set our tooltip info structure
		With ti
			''if we want it centered, then set that flag
			If mvarCentered Then
				.lFlags = TTF_SUBCLASS Or TTF_CENTERTIP Or TTF_IDISHWND
			Else
				.lFlags = TTF_SUBCLASS Or TTF_IDISHWND
			End If
			
			''set the hwnd prop to our parent control's hwnd
			.hwnd = m_lParentHwnd
			.lId = m_lParentHwnd '0
			.hInstance = VB6.GetHInstance().ToInt32()
			'.lpstr = ALREADY SET
			'.lpRect = lpRect

            .lSize = Marshal.SizeOf(ti)
		End With
		
		''add the tooltip structure
        Dim handle2 As GCHandle = GCHandle.Alloc(ti.lId, GCHandleType.Pinned)
		Try 
			Dim tmpPtr2 As IntPtr = handle2.AddrOfPinnedObject()

			SendMessage(m_lTTHwnd, CInt(TTM_ADDTOOLA), 0, tmpPtr2)
		Finally 
			handle2.Free()
		End Try
		
		''if we want a title or we want an icon
		If mvarTitle <> Nothing Or mvarIcon <> ttIconType.TTNoIcon Then
			Dim tmpPtr3 As IntPtr = Marshal.StringToHGlobalAnsi(mvarTitle)
			Try 
				SendMessage(m_lTTHwnd, CInt(TTM_SETTITLE), mvarIcon, tmpPtr3)
				mvarTitle = Marshal.PtrToStringAnsi(tmpPtr3)
			Finally 
				Marshal.FreeHGlobal(tmpPtr3)
			End Try
		End If
		

		If Not mvarForeColor.Equals(0) Then
			Dim handle4 As GCHandle = GCHandle.Alloc(0, GCHandleType.Pinned)
			Try 
				Dim tmpPtr4 As IntPtr = handle4.AddrOfPinnedObject()
				SendMessage(m_lTTHwnd, CInt(TTM_SETTIPTEXTCOLOR), mvarForeColor, tmpPtr4)
			Finally 
				handle4.Free()
			End Try
		End If
		

		If Not mvarBackColor.Equals(0) Then
			Dim handle5 As GCHandle = GCHandle.Alloc(0, GCHandleType.Pinned)
			Try 
				Dim tmpPtr5 As IntPtr = handle5.AddrOfPinnedObject()
				SendMessage(m_lTTHwnd, CInt(TTM_SETTIPBKCOLOR), mvarBackColor, tmpPtr5)
			Finally 
				handle5.Free()
			End Try
		End If
		
		SendMessageLong(m_lTTHwnd, CInt(TTM_SETDELAYTIME), TTDT_AUTOPOP, mvarVisibleTime)
		SendMessageLong(m_lTTHwnd, CInt(TTM_SETDELAYTIME), TTDT_INITIAL, mvarDelayTime)
	End Function
	
	Public Sub New()
		MyBase.New()
		InitCommonControls()
	End Sub
	
	Protected Overrides Sub Finalize()
		Destroy()
	End Sub
	
	Public Sub Destroy()
		If m_lTTHwnd <> 0 Then
			DestroyWindow(m_lTTHwnd)
		End If
	End Sub
End Class
