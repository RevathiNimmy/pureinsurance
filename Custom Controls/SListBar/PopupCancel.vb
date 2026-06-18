Option Explicit On
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports SharedFiles
'Imports SharedFiles.gPMLibrary
Namespace ListBarControl
    ''' <summary>
    ''' Represents the method that handles the <see cref="PopupCancelNotifier.PopupCancel"/> event
    ''' raised by this class.
    ''' </summary>
    Friend Delegate Sub PopupCancelEventHandler(sender As Object, e As EventArgs)

#Region "PopupCancelNotifier"
    ''' <summary>
    ''' 
    ''' A class which provides the functionality required to 
    ''' cancel a popup window.  This class wraps two pieces of 
    ''' functionality:
    ''' 
    ''' <list type="number">Firstly, it checks whether the form (or the form owner
    ''' for the control) receives a <c>WM_ACTIVATE</c> message with
    ''' wParam = 0.  This indicates the window has gone out
    ''' of focus because the user has clicked on another one.</list>
    ''' <list type="number">Secondly, it installs a Win32 Mouse Hook and checks
    ''' for mouse presses anywhere else in the application.
    ''' This is the same technique that's used in the Framework
    ''' Class Library to implement drop-down designer windows.</list>
    ''' 
    ''' However, this functionality may cause a problem because 
    ''' the CLR will mark this code as non-type safe owing to 
    ''' the unmanaged code in the Hook.  If you don't need in-place 
    ''' editing of items, then you can remove this code and any 
    ''' reference to it from the <c>ListBar</c> control and recompile 
    ''' for a 100% managed type-safe version of the control.
    ''' 
    ''' <remarks>
    ''' Copyright &#169; 2003 Steve McMahon for vbAccelerator.com.
    ''' vbAccelerator is a Trade Mark of vbAccelerator Ltd.  All Rights
    ''' Reserved.  Please visit http://vbaccelerator.com/ for more
    ''' on this and other VB and .NET Framework code.
    ''' </remarks>
    ''' 
    ''' </summary>
    Friend Class PopupCancelNotifier
        Inherits NativeWindow
        Implements IDisposable

        ''' <summary>
        ''' The PopupCancel event is raised whenever the popup should be
        ''' cancelled.
        ''' </summary>

        Public Event PopupCancel As PopupCancelEventHandler

        Private mouseHook As New MouseHook()

        ''' <summary>
        ''' Message sent to a window when activation state
        ''' changes
        ''' </summary>
        Private Const WM_ACTIVATE As Integer = &H6
        ''' <summary>
        ''' Window handle to track for popup cancellation
        ''' </summary>
        Private trackHandle As IntPtr = IntPtr.Zero
        ''' <summary>
        ''' Whether this object has been disposed or not
        ''' </summary>
        Private disposed As Boolean = False

        ''' <summary>
        ''' Start tracking for a popup cancellation.
        ''' </summary>
        ''' <param name="ctl">The <c>Control</c> or <c>Form</c>
        ''' to use when tracking Window inactivation messages. This can
        ''' either be a control or a Form.</param>
        Public Sub StartTracking(ctl As Control)
            Dim handle As IntPtr = IntPtr.Zero

            Dim ctlOwnerForm As Control = ctl
            Dim ctlTest As Control = Nothing
            While Not GetType(Form).IsAssignableFrom(ctlOwnerForm.[GetType]())
                ctlTest = ctlOwnerForm.Parent
                If ctlTest Is Nothing Then
                    Exit While
                Else
                    ctlOwnerForm = ctlTest
                End If
            End While

            Me.trackHandle = ctlOwnerForm.Handle
            Me.AssignHandle(trackHandle)
            Me.mouseHook.Install()
            AddHandler Me.mouseHook.MouseHookEvent, New MouseHookEventHandler(AddressOf mouseHook_MouseHookEvent)
        End Sub

        Private Sub mouseHook_MouseHookEvent(sender As Object, ByRef e As MouseHookEventArgs)
            If e.Button <> MouseButtons.None Then
                OnPopupCancel(New EventArgs())
            End If
        End Sub

        ''' <summary>
        ''' Check for the WM_ACTIVATE message and stop
        ''' tracking if the window is inactivated.
        ''' </summary>
        ''' <param name="msg">Message details for this window procedure
        ''' event.</param>
        Protected Overrides Sub WndProc(ByRef msg As Message)
            MyBase.WndProc(msg)
            If msg.Msg = WM_ACTIVATE Then
                If gPMFunctions.ToSafeInteger(msg.WParam) = 0 Then
                    OnPopupCancel(New EventArgs())
                End If
            End If
        End Sub

        ''' <summary>
        ''' Stop tracking. Called automatically if this class determines
        ''' the popup should be cancelled.
        ''' </summary>
        Public Sub StopTracking()
            If Not Me.trackHandle.Equals(IntPtr.Zero) Then
                Me.ReleaseHandle()
                Me.trackHandle = IntPtr.Zero
                RemoveHandler Me.mouseHook.MouseHookEvent, New MouseHookEventHandler(AddressOf mouseHook_MouseHookEvent)
                Me.mouseHook.Uninstall()
            End If
        End Sub

        ''' <summary>
        ''' Notify when the popup should be cancelled,
        ''' and uninstall tracking.
        ''' </summary>
        Protected Overridable Sub OnPopupCancel(e As EventArgs)
            RaiseEvent PopupCancel(Me, e)
            StopTracking()
        End Sub


        Public Sub New()
        End Sub

        ''' <summary>
        ''' Finalises the class and clears up resources if the
        ''' Dispose() method has not been called.
        ''' </summary>
        Protected Overrides Sub Finalize()
            Try
                If Not disposed Then
                    StopTracking()
                End If
            Finally
                MyBase.Finalize()
            End Try
        End Sub

        ''' <summary>
        ''' Ensures any resources associated with this object are
        ''' cleared up.
        ''' </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            StopTracking()
            disposed = True
            GC.SuppressFinalize(Me)
        End Sub

    End Class
#End Region

#Region "LocalWindowsHook"
    '
    ' The LocalWindowsHook code is based mainly on 
    ' Dino Esposito's Cutting Edge column in the MSDN October 
    ' 2002 issue, "Cutting Edge: Windows Hooks in the .NET Framework".
    ' Changes:
    ' 1) Change the hook event handling to an override-based hook 
    '    mechanism rather than an event-based one.
    ' 2) The event information needs to be by ref so we can modify the
    '    details returned to Windows.
    ' 3) Some half-hearted documentation and  field renaming.
    '

#Region "Class HookEventArgs"
    ''' <summary>
    ''' Arguments for the Hook event
    ''' </summary>
    ''' <remarks>This code is based on code published by Dino Esposito
    ''' in the article "Cutting Edge: Windows Hooks in the .NET Framework"
    ''' published in the October 2002 edition of MSDN and available online
    ''' at http://msdn.microsoft.com/
    ''' </remarks>
    Friend Class HookEventArgs
        Inherits EventArgs
        ''' <summary>
        ''' Hook code
        ''' </summary>
        Public HookCode As Integer
        ''' <summary>
        ''' WPARAM argument
        ''' </summary>
        Public wParam As IntPtr
        ''' <summary>
        ''' LPARAM argument
        ''' </summary>
        Public lParam As IntPtr
    End Class
#End Region

#Region "Enum HookType"
    ''' <summary>
    ''' Hook Types available under Windows. TODO: documentation
    ''' </summary>
    ''' <remarks>This code is based on code published by Dino Esposito
    ''' in the article "Cutting Edge: Windows Hooks in the .NET Framework"
    ''' published in the October 2002 edition of MSDN and available online
    ''' at http://msdn.microsoft.com/
    ''' </remarks>
    Friend Enum HookType As Integer
        WH_JOURNALRECORD = 0
        WH_JOURNALPLAYBACK = 1
        WH_KEYBOARD = 2
        WH_GETMESSAGE = 3
        WH_CALLWNDPROC = 4
        WH_CBT = 5
        WH_SYSMSGFILTER = 6
        WH_MOUSE = 7
        WH_HARDWARE = 8
        WH_DEBUG = 9
        WH_SHELL = 10
        WH_FOREGROUNDIDLE = 11
        WH_CALLWNDPROCRET = 12
        WH_KEYBOARD_LL = 13
        WH_MOUSE_LL = 14
    End Enum
#End Region


    'internal abstract class LocalWindowsHook : IDisposable'' 
    Friend MustInherit Class LocalWindowsHook
        Implements IDisposable
#Region "Unmanaged code"
        <DllImport("user32.dll")> _
        Public Shared Function SetWindowsHookEx(code As HookType, func As HookProc, hInstance As IntPtr, threadID As Integer) As IntPtr
        End Function
        <DllImport("user32.dll")> _
        Public Shared Function UnhookWindowsHookEx(hhook As IntPtr) As Integer
        End Function
        <DllImport("user32.dll")> _
        Protected Shared Function CallNextHookEx(hhook As IntPtr, code As Integer, wParam As IntPtr, lParam As IntPtr) As Integer
        End Function
#End Region

        ''' <summary>
        ''' Filter function delegate
        ''' </summary>
        Public Delegate Function HookProc(code As Integer, wParam As IntPtr, lParam As IntPtr) As Integer

#Region "Properties"
        ''' <summary>
        ''' The handle to the Windows hook.
        ''' </summary>
        Protected HookHandle As IntPtr = IntPtr.Zero
        ''' <summary>
        ''' The hook filter function.
        ''' </summary>
        Protected FilterFunc As HookProc = Nothing
        ''' <summary>
        ''' The type of hook installed.
        ''' </summary>
        Protected HookType As HookType
#End Region
        Private disposed As Boolean = False

        ''' <summary>
        ''' Represents the method that handles the HookInvoked event
        ''' raised by this class.
        ''' </summary>
        Public Delegate Sub HookInvokedEventHandler(sender As Object, ByRef e As HookEventArgs)

        ''' <summary>
        ''' The HookInvoked event is raised whenever the hook fires.
        ''' </summary>
        Public Event HookInvoked As HookInvokedEventHandler

        ''' <summary>
        ''' Raises the HookInvoked event. This method can be overriden
        ''' for particular implementations of a hook, or an implementation
        ''' can respond to the HookInvoked event.
        ''' </summary>
        ''' <param name="e">The HookEventArgs for this hook
        ''' event.</param>
        Protected Overridable Sub OnHookInvoked(ByRef e As HookEventArgs)
            RaiseEvent HookInvoked(Me, e)
        End Sub


        ''' <summary>
        ''' Constructs a new instance of this class with
        ''' the specified Hook Type.
        ''' </summary>
        ''' <param name="hookType">Hook type to create</param>
        Public Sub New(hookType As HookType)
            Me.HookType = hookType
            Me.FilterFunc = New HookProc(AddressOf Me.CoreHookProc)
        End Sub

        ''' <summary>
        ''' Default filter function.
        ''' </summary>
        ''' <param name="code">Hook code</param>
        ''' <param name="wParam">Hook wParam</param>
        ''' <param name="lParam">Hook lParam</param>
        ''' <returns></returns>
        Private Function CoreHookProc(code As Integer, wParam As IntPtr, lParam As IntPtr) As Integer
            ' According to MSDN docs, if code < 0 then you must call
            ' the next hook in the chain:
            If code < 0 Then
                Return CallNextHookEx(Me.HookHandle, code, wParam, lParam)
            Else
                ' Call the event:
                Dim e As New HookEventArgs()
                e.HookCode = code
                e.wParam = wParam
                e.lParam = lParam
                OnHookInvoked(e)

                ' Yield to the next hook in the chain:
                Return CallNextHookEx(Me.HookHandle, e.HookCode, e.wParam, e.lParam)
            End If
        End Function

        ''' <summary>
        ''' Install the hook.
        ''' </summary>
        Public Sub Install()
            Me.HookHandle = SetWindowsHookEx(Me.HookType, Me.FilterFunc, IntPtr.Zero, gPMFunctions.ToSafeInteger(AppDomain.GetCurrentThreadId()))
            Trace.Assert((Not Me.HookHandle.Equals(IntPtr.Zero)), "Failed to install hook!")
        End Sub

        ''' <summary>
        ''' Uninstall the hook.
        ''' </summary>
        Public Sub Uninstall()
            If Me.HookHandle <> IntPtr.Zero Then
                UnhookWindowsHookEx(Me.HookHandle)
            End If
            Me.HookHandle = IntPtr.Zero
        End Sub

        ''' <summary>
        ''' Clear up any resources associated with the hook if 
        ''' Dispose() has not been called.
        ''' </summary>
        Protected Overrides Sub Finalize()
            Try
                If Not Me.disposed Then
                    Uninstall()
                End If
            Finally
                MyBase.Finalize()
            End Try
        End Sub

        ''' <summary>
        ''' Clear up any resources associated with this hook.
        ''' </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            If Not Me.disposed Then
                Uninstall()
                Me.disposed = True
                GC.SuppressFinalize(Me)
            End If
        End Sub

    End Class
#End Region

#Region "Mouse Hook Implementation"

#Region "MOUSEHOOKSTRUCT for interop with the Windows Mouse Hook"
    ''' <summary>
    ''' The Windows API <c>MOUSEHOOKSTRUCT</c> which is passed in the 
    ''' <c>lParam</c> of a Mouse Hook event.
    ''' </summary>
    <StructLayoutAttribute(LayoutKind.Sequential)> _
    Friend Structure MOUSEHOOKSTRUCT
        ''' <summary>
        ''' Mouse X Position
        ''' </summary>
        Public x As Integer
        ''' <summary>
        ''' Mouse Y Position
        ''' </summary>
        Public y As Integer
        ''' <summary>
        ''' Handle of window mouse is over
        ''' </summary>
        Public handle As IntPtr
        ''' <summary>
        ''' Hit test code returned
        ''' </summary>
        Public wHitTestCode As Integer
        ''' <summary>
        ''' Other information about the mouse event
        ''' </summary>
        Public dwExtraInfo As Integer

        ' note that under Windows2000 and XP there is 
        ' also an additional mouseData DWORD which supplies
        ' the mouse wheel information

        ''' <summary>
        ''' Provides a human-readable string displaying the contents of
        ''' this structure.
        ''' </summary>
        ''' <returns>A string containing details of the contents of
        ''' this structure.</returns>
        Public Overrides Function ToString() As String
            Return [String].Format("{0} x={1},y={2},hWnd={3},wHitTestCode={4},dwExtraInfo={5}", GetType(MOUSEHOOKSTRUCT).FullName, Me.x, Me.y, Me.handle, Me.wHitTestCode, _
                Me.dwExtraInfo)
        End Function
    End Structure
#End Region

#Region "Enumerations associated with the Mouse Hook class"

    ''' <summary>
    ''' Types of MouseHook events which are recorded:
    ''' </summary>
    Friend Enum MouseHookEventType As Integer
        ''' <summary>
        ''' The mouse has moved
        ''' </summary>
        MouseMove
        ''' <summary>
        ''' A mouse button has been depressed
        ''' </summary>
        MouseDown
        ''' <summary>
        ''' A mouse button has been released
        ''' </summary>
        MouseUp
        ''' <summary>
        ''' A mouse wheel action has occurred
        ''' </summary>
        MouseWheel
        ''' <summary>
        ''' A mouse button has been double-clicked
        ''' </summary>
        DblClick
    End Enum

    ''' <summary>
    ''' The location of the mouse when a mouse hook event is recorded.
    ''' </summary>
    Friend Enum MouseHookEventLocation As Integer
        ''' <summary>
        ''' The mouse event occurred in the client area.
        ''' </summary>
        Client
        ''' <summary>
        ''' The mouse event occurred in a non-client area.
        ''' </summary>
        NonClient
    End Enum
#End Region


#Region "MouseHookEventArgs class"
    ''' <summary>
    ''' Information about a Mouse Hook event
    ''' which has occured.
    ''' TODO: X buttons
    ''' </summary>
    Friend Class MouseHookEventArgs
        Inherits EventArgs
        Private m_eventType As MouseHookEventType
        Private m_eventLocation As MouseHookEventLocation
        Private m_button As MouseButtons
        Private m_x As Integer
        Private m_y As Integer
        Private m_handle As IntPtr
        Private hitTestCode As Integer
        Private extraData As Integer

        Private Const WM_MOUSEMOVE As Integer = &H200
        Private Const WM_LBUTTONDOWN As Integer = &H201
        Private Const WM_LBUTTONUP As Integer = &H202
        Private Const WM_LBUTTONDBLCLK As Integer = &H203
        Private Const WM_RBUTTONDOWN As Integer = &H204
        Private Const WM_RBUTTONUP As Integer = &H205
        Private Const WM_RBUTTONDBLCLK As Integer = &H206
        Private Const WM_MBUTTONDOWN As Integer = &H207
        Private Const WM_MBUTTONUP As Integer = &H208
        Private Const WM_MBUTTONDBLCLK As Integer = &H209
        Private Const WM_MOUSEWHEEL As Integer = &H20A
        Private Const WM_XBUTTONDOWN As Integer = &H20B
        Private Const WM_XBUTTONUP As Integer = &H20C
        Private Const WM_XBUTTONDBLCLK As Integer = &H20D
        Private Const WM_NCLBUTTONDOWN As Integer = &HA1
        Private Const WM_NCLBUTTONUP As Integer = &HA2
        Private Const WM_NCLBUTTONDBLCLK As Integer = &HA3
        Private Const WM_NCRBUTTONDOWN As Integer = &HA4
        Private Const WM_NCRBUTTONUP As Integer = &HA5
        Private Const WM_NCRBUTTONDBLCLK As Integer = &HA6
        Private Const WM_NCMBUTTONDOWN As Integer = &HA7
        Private Const WM_NCMBUTTONUP As Integer = &HA8
        Private Const WM_NCMBUTTONDBLCLK As Integer = &HA9
        Private Const WM_NCXBUTTONDOWN As Integer = &HAB
        Private Const WM_NCXBUTTONUP As Integer = &HAC
        Private Const WM_NCXBUTTONDBLCLK As Integer = &HAD

        ''' <summary>
        ''' Gets the type of mouse event.
        ''' </summary>
        Public ReadOnly Property EventType() As MouseHookEventType
            Get
                Return Me.m_eventType
            End Get
        End Property

        ''' <summary>
        ''' Gets the location in which the mouse event
        ''' occurred.
        ''' </summary>
        Public ReadOnly Property EventLocation() As MouseHookEventLocation
            Get
                Return Me.m_eventLocation
            End Get
        End Property

        ''' <summary>
        ''' Gets the button which is involved in the action
        ''' (or MouseButtons.None if no button is used).
        ''' </summary>
        Public ReadOnly Property Button() As MouseButtons
            Get
                Return Me.m_button
            End Get
        End Property

        ''' <summary>
        ''' Returns the X location of the mouse when the event
        ''' occurred.
        ''' </summary>
        Public ReadOnly Property X() As Integer
            Get
                Return Me.m_x
            End Get
        End Property

        ''' <summary>
        ''' Returns the Y location of the mouse when the event
        ''' occurred.
        ''' </summary>
        Public ReadOnly Property Y() As Integer
            Get
                Return Me.Y
            End Get
        End Property

        ''' <summary>
        ''' Gets the window handle of the object the mouse
        ''' was over.
        ''' </summary>
        Public ReadOnly Property Handle() As IntPtr
            Get
                Return Me.m_handle
            End Get
        End Property


        ''' <summary>
        ''' Constructs a new MouseHookEvent
        ''' </summary>
        ''' <param name="wParam">The <c>wParam</c> (Message code) for the
        ''' Mouse Hook event</param>
        ''' <param name="mhs">The <c>MOUSEHOOKEVENT</c> structure
        ''' for the hook event.</param>
        Public Sub New(wParam As IntPtr, mhs As MOUSEHOOKSTRUCT)
            Select Case gPMFunctions.ToSafeInteger(wParam)
                Case WM_MOUSEMOVE
                    Me.m_eventType = MouseHookEventType.MouseMove
                    ' we could check if we're over a non-client
                    ' area here etc
                    Me.m_button = MouseButtons.None
                    Exit Select

                Case WM_LBUTTONDOWN
                    Me.m_eventType = MouseHookEventType.MouseDown
                    Me.m_button = MouseButtons.Left
                    Me.m_eventLocation = MouseHookEventLocation.Client
                    Exit Select

                Case WM_LBUTTONUP
                    Me.m_eventType = MouseHookEventType.MouseUp
                    Me.m_button = MouseButtons.Left
                    Me.m_eventLocation = MouseHookEventLocation.Client
                    Exit Select

                Case WM_LBUTTONDBLCLK
                    Me.m_eventType = MouseHookEventType.DblClick
                    Me.m_button = MouseButtons.Left
                    Me.m_eventLocation = MouseHookEventLocation.Client
                    Exit Select

                Case WM_MBUTTONDOWN
                    Me.m_eventType = MouseHookEventType.MouseDown
                    Me.m_button = MouseButtons.Middle
                    Me.m_eventLocation = MouseHookEventLocation.Client
                    Exit Select

                Case WM_MBUTTONUP
                    Me.m_eventType = MouseHookEventType.MouseUp
                    Me.m_button = MouseButtons.Middle
                    Me.m_eventLocation = MouseHookEventLocation.Client
                    Exit Select

                Case WM_MBUTTONDBLCLK
                    Me.m_eventType = MouseHookEventType.DblClick
                    Me.m_button = MouseButtons.Middle
                    Me.m_eventLocation = MouseHookEventLocation.Client
                    Exit Select

                Case WM_RBUTTONDOWN
                    Me.m_eventType = MouseHookEventType.MouseDown
                    Me.m_button = MouseButtons.Right
                    Me.m_eventLocation = MouseHookEventLocation.Client
                    Exit Select

                Case WM_RBUTTONUP
                    Me.m_eventType = MouseHookEventType.MouseUp
                    Me.m_button = MouseButtons.Right
                    Me.m_eventLocation = MouseHookEventLocation.Client
                    Exit Select

                Case WM_RBUTTONDBLCLK
                    Me.m_eventType = MouseHookEventType.DblClick
                    Me.m_button = MouseButtons.Right
                    Me.m_eventLocation = MouseHookEventLocation.Client
                    Exit Select

                Case WM_NCLBUTTONDOWN
                    Me.m_eventType = MouseHookEventType.MouseDown
                    Me.m_button = MouseButtons.Left
                    Me.m_eventLocation = MouseHookEventLocation.NonClient
                    Exit Select

                Case WM_NCLBUTTONUP
                    Me.m_eventType = MouseHookEventType.MouseUp
                    Me.m_button = MouseButtons.Left
                    Me.m_eventLocation = MouseHookEventLocation.NonClient
                    Exit Select

                Case WM_NCLBUTTONDBLCLK
                    Me.m_eventType = MouseHookEventType.DblClick
                    Me.m_button = MouseButtons.Left
                    Me.m_eventLocation = MouseHookEventLocation.NonClient
                    Exit Select

                Case WM_NCMBUTTONDOWN
                    Me.m_eventType = MouseHookEventType.MouseDown
                    Me.m_button = MouseButtons.Middle
                    Me.m_eventLocation = MouseHookEventLocation.NonClient
                    Exit Select

                Case WM_NCMBUTTONUP
                    Me.m_eventType = MouseHookEventType.MouseUp
                    Me.m_button = MouseButtons.Middle
                    Me.m_eventLocation = MouseHookEventLocation.NonClient
                    Exit Select

                Case WM_NCMBUTTONDBLCLK
                    Me.m_eventType = MouseHookEventType.DblClick
                    Me.m_button = MouseButtons.Middle
                    Me.m_eventLocation = MouseHookEventLocation.NonClient
                    Exit Select

                Case WM_NCRBUTTONDOWN
                    Me.m_eventType = MouseHookEventType.MouseDown
                    Me.m_button = MouseButtons.Right
                    Me.m_eventLocation = MouseHookEventLocation.NonClient
                    Exit Select

                Case WM_NCRBUTTONUP
                    Me.m_eventType = MouseHookEventType.MouseUp
                    Me.m_button = MouseButtons.Right
                    Me.m_eventLocation = MouseHookEventLocation.NonClient
                    Exit Select

                Case WM_NCRBUTTONDBLCLK
                    Me.m_eventType = MouseHookEventType.DblClick
                    Me.m_button = MouseButtons.Right
                    Me.m_eventLocation = MouseHookEventLocation.NonClient
                    Exit Select
            End Select

            Me.m_x = mhs.x
            Me.m_y = mhs.y

            Me.m_handle = mhs.handle

            Me.hitTestCode = mhs.wHitTestCode
            Me.extraData = mhs.dwExtraInfo
        End Sub
    End Class
#End Region


#Region "MouseHook class delegates"
    ''' <summary>
    ''' Represents the method that handles the HookInvoked event
    ''' raised by this class.
    ''' </summary>
    Friend Delegate Sub MouseHookEventHandler(sender As Object, ByRef e As MouseHookEventArgs)
#End Region

#Region "MouseHook Class"
    Friend Class MouseHook
        Inherits LocalWindowsHook

        ''' <summary>
        ''' The HookInvoked event is raised whenever the hook fires.
        ''' </summary>
        Public Event MouseHookEvent As MouseHookEventHandler


        ''' <summary>
        ''' Override for the generic hook's invoked event to
        ''' convert to a strongly typed MouseHookEvent:
        ''' </summary>
        ''' <param name="e">Generic Hook event argument details</param>
        Protected Overrides Sub OnHookInvoked(ByRef e As HookEventArgs)
            ' Convert into mouse details:
            Dim mhs As MOUSEHOOKSTRUCT = CType(Marshal.PtrToStructure(e.lParam, GetType(MOUSEHOOKSTRUCT)), MOUSEHOOKSTRUCT)

            Dim mhe As New MouseHookEventArgs(e.wParam, mhs)
            OnMouseHookEvent(mhe)

        End Sub

        ''' <summary>
        ''' Raises the MouseHookEvent event.
        ''' </summary>
        ''' <param name="e">The MouseHook event details associated
        ''' with this mouse hook event.</param>
        Protected Overridable Sub OnMouseHookEvent(ByRef e As MouseHookEventArgs)
            RaiseEvent MouseHookEvent(Me, e)

        End Sub

        ''' <summary>
        ''' Constructs a new instance of a MouseHook.
        ''' </summary>
        Public Sub New()
            ' intentionally blank
            MyBase.New(HookType.WH_MOUSE)
        End Sub

    End Class
#End Region

#End Region

End Namespace
