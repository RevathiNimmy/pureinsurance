Attribute VB_Name = "MDLLWindow"
' Module:   Window persistence and utility functions
' Shared:   Yes (RESTRICTED)
' Needs:    MDLLRegistry, CByteArray
'
' THIS CODE IMPLEMENTS CORRESPONDING FUNCTIONS IN THE DLL.
' IT IS SHARED *ONLY* TO SUPPORT SMALL UTILITIES THAT
' CANNOT REFERENCE THE DLL. *DO NOT* ALTER THIS CODE IN ANY
' WAY UNLESS YOU ARE CHANGING THE INTERNALS OF THE DLL.
'
' To avoid pulling in unnecessary references, this module
' requires conditional compilation constants to be defined.
' * IncludeWinState = 1   includes window state support
'
Option Explicit

' Properties.
Private m_sWinStateRegFolder As String

' API declarations.
Private Const SC_CLOSE As Long = &HF060&
Private Const xSC_CLOSE As Long = -10&
Private Const MIIM_STATE As Long = &H1&
Private Const MIIM_ID As Long = &H2&
Private Const MFS_GRAYED As Long = &H3&
Private Const WM_NCACTIVATE As Long = &H86

Private Type TPositionAndSize
    Left As Long
    Top As Long
    Width As Long
    Height As Long
End Type

Private Type RECT
    Left As Long
    Top As Long
    Right As Long
    Bottom As Long
End Type

Private Type MENUITEMINFO
    cbSize As Long
    fMask As Long
    fType As Long
    fState As Long
    wID As Long
    hSubMenu As Long
    hbmpChecked As Long
    hbmpUnchecked As Long
    dwItemData As Long
    dwTypeData As String
    cch As Long
End Type

Private Declare Function GetSystemMenu Lib "user32" _
    (ByVal hWnd As Long, ByVal bRevert As Long) As Long
Private Declare Function GetMenuItemInfo Lib "user32" Alias "GetMenuItemInfoA" _
    (ByVal hMenu As Long, ByVal un As Long, ByVal b As Boolean, ByRef lpMenuItemInfo As MENUITEMINFO) As Long
Private Declare Function SetMenuItemInfo Lib "user32" Alias "SetMenuItemInfoA" _
    (ByVal hMenu As Long, ByVal un As Long, ByVal bool As Boolean, ByRef lpcMenuItemInfo As MENUITEMINFO) As Long
Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" _
    (ByVal hWnd As Long, ByVal wMsg As Long, ByVal wParam As Long, ByRef lParam As Any) As Long
Private Declare Function GetWindowRect Lib "user32" _
    (ByVal hWnd As Long, ByRef lpRect As RECT) As Long

' Error declarations.
Const ksErrSource = "iSWServices.GWindow"
Const knErrFolderRequired = 5
Const ksErrFolderRequired = "The registry folder must be set before calling this function."
Const knErrObjectNotForm = 13
Const ksErrObjectNotForm = "Type Mismatch (VB Form object required)."

#If IncludeWinState Then
Public Property Get WinStateRegFolder() As String

    WinStateRegFolder = m_sWinStateRegFolder

End Property
#End If

' This must be set at least once at the start of the application.
' It sets the registry folder under which all form coordinates
' are stored.
#If IncludeWinState Then
Public Property Let WinStateRegFolder(ByVal sWinStateRegFolder As String)

    m_sWinStateRegFolder = sWinStateRegFolder

End Property
#End If

#If IncludeWinState Then
Public Sub WinStateRead(ByVal frmForm As Object, _
    Optional ByVal bSizeable As Boolean = True, _
    Optional ByVal sWinStateRegFolder As String = "")

    Const klMinTopLeft = -360&      ' 1/4 inch (in twips)
    Const klAssumeTopLeft = 180&    ' 1/8 inch (in twips)

    Dim sFolder As String
    Dim lScreenWidth As Long
    Dim lScreenHeight As Long
    Dim baFormState As New CByteArray
    Dim nFormState As Integer
    Dim lFormLeft As Long
    Dim lFormTop As Long
    Dim lFormWidth As Long
    Dim lFormHeight As Long

    ' If called from the DLL class, override the global
    ' folder with the class instance-specific folder.
    If sWinStateRegFolder <> "" Then
        sFolder = sWinStateRegFolder
    Else
        sFolder = m_sWinStateRegFolder
    End If

    ' Safety checks.
    If sFolder = "" Then
        Err.Raise knErrFolderRequired, ksErrSource, ksErrFolderRequired
    ElseIf Not IsForm(frmForm, False) Then
        Err.Raise knErrObjectNotForm, ksErrSource, ksErrObjectNotForm
    End If

    ' Read in previous window size, position and state.
    ' Default to design-time values if settings have never been
    ' saved.
    With baFormState
        .Resize 18, False
        .ItemInteger(0) = vbNormal
        .ItemLong(2) = frmForm.Left
        .ItemLong(6) = frmForm.Top
        .ItemLong(10) = frmForm.Width
        .ItemLong(14) = frmForm.Height
        .ValueArray = RegRead(HKEY_CURRENT_USER, sFolder, frmForm.Name, .ValueArray)
        .Resize 18, True
        nFormState = .ItemInteger(0)
        lFormLeft = .ItemLong(2)
        lFormTop = .ItemLong(6)
        lFormWidth = .ItemLong(10)
        lFormHeight = .ItemLong(14)
    End With

    ' If window was minimised or maximised last time, VB returns
    ' the form dimensions as if the window was actually the size
    ' of an icon or the size of the screen. Thus we can only
    ' assume dimensions for these.

    ' First set normal because VB won't let you change dimensions otherwise.
    frmForm.WindowState = vbNormal
    lScreenWidth = Screen.Width
    lScreenHeight = Screen.Height

    Select Case nFormState
    Case vbMinimized
        ' Do nothing (icon startup will confuse typical user).

    Case vbMaximized
        frmForm.Move klAssumeTopLeft, klAssumeTopLeft, lScreenWidth, lScreenHeight
        frmForm.WindowState = vbMaximized

    Case Else
        ' Check values for validity.
        If lFormLeft < klMinTopLeft Then
            lFormLeft = klMinTopLeft
        ElseIf lFormLeft > lScreenWidth - klAssumeTopLeft Then
            lFormLeft = lScreenWidth - klAssumeTopLeft
        End If
        If lFormTop < klMinTopLeft Then
            lFormTop = klMinTopLeft
        ElseIf lFormTop > lScreenHeight - klAssumeTopLeft Then
            lFormTop = lScreenHeight - klAssumeTopLeft
        End If
        If bSizeable Then
            If lFormWidth < klAssumeTopLeft Then
                lFormWidth = klAssumeTopLeft
            ElseIf lFormWidth > lScreenWidth Then
                lFormWidth = lScreenWidth
            End If
            If lFormHeight < klAssumeTopLeft Then
                lFormHeight = klAssumeTopLeft
            ElseIf lFormHeight > lScreenHeight Then
                lFormHeight = lScreenHeight
            End If
            frmForm.Move lFormLeft, lFormTop, lFormWidth, lFormHeight
        Else
            frmForm.Move lFormLeft, lFormTop
        End If

    End Select

End Sub
#End If

#If IncludeWinState Then
Public Sub WinStateWrite(ByVal frmForm As Object, _
    Optional ByVal sWinStateRegFolder As String = "")

    Dim sFolder As String
    Dim baFormState As New CByteArray

    ' If called from the DLL class, override the global
    ' folder with the class instance-specific folder.
    If sWinStateRegFolder <> "" Then
        sFolder = sWinStateRegFolder
    Else
        sFolder = m_sWinStateRegFolder
    End If

    ' Safety checks.
    If sFolder = "" Then
        Err.Raise knErrFolderRequired, ksErrSource, ksErrFolderRequired
    ElseIf Not IsForm(frmForm, False) Then
        Err.Raise knErrObjectNotForm, ksErrSource, ksErrObjectNotForm
    End If

    ' Write out current window size, position and state.
    ' Always write the size, just in case the window is made
    ' sizeable in future.
    With baFormState
        .Resize 18, False
        .ItemInteger(0) = frmForm.WindowState
        .ItemLong(2) = frmForm.Left
        .ItemLong(6) = frmForm.Top
        .ItemLong(10) = frmForm.Width
        .ItemLong(14) = frmForm.Height
        RegWrite HKEY_CURRENT_USER, sFolder, frmForm.Name, .ValueArray
    End With

End Sub
#End If

' If you are cascading an MDI child window, pass the
' MDI parent form in the last parameter. Otherwise leave
' it out.
Public Sub WinCascade(ByVal frmForm As Object, _
    ByVal frmParent As Object, _
    Optional ByVal frmAppMain As Object = Nothing)

    Const klCascadeHeight = 325
    Const klCascadeWidth = 325

    Dim lLeft As Long
    Dim lTop As Long
    Dim lMaxLeft As Long
    Dim lMaxTop As Long

    ' Safety checks.
    If Not IsForm(frmForm, False) Then
        Err.Raise knErrObjectNotForm, ksErrSource, ksErrObjectNotForm
    ElseIf Not IsForm(frmParent, False) Then
        Err.Raise knErrObjectNotForm, ksErrSource, ksErrObjectNotForm
    ElseIf Not IsForm(frmAppMain, True) Then
        Err.Raise knErrObjectNotForm, ksErrSource, ksErrObjectNotForm
    End If

    ' Calculate ideal position of top of form.
    lLeft = frmParent.Left + klCascadeWidth
    lTop = frmParent.Top + klCascadeHeight

    If Not TypeOf frmParent Is VB.MDIForm Then
        If frmParent.MDIChild Then
            If Not frmAppMain Is Nothing Then
                ' lTop and lLeft are in MDI parent coordinates rather than screen
                ' coordinates. Therefore we add the MDI parent's coordinates.
                lLeft = lLeft + frmAppMain.Left + klCascadeWidth
                lTop = lTop + frmAppMain.Top + klCascadeHeight
            End If
        End If
    End If

    ' Ensure right of form does not go past right of screen.
    lMaxLeft = Screen.Width - frmForm.Width
    If lLeft > lMaxLeft Then lLeft = lMaxLeft

    If frmAppMain Is Nothing Then
        ' Ensure bottom of form does not go past bottom of screen.
        lMaxTop = Screen.Height - frmForm.Height
    Else
        ' Ensure bottom of form does not obscure the status bar (ie. does
        ' not extend beyond the bottom of the main application form).
        lMaxTop = frmAppMain.Top + frmAppMain.Height - frmForm.Height
    End If
    If lTop > lMaxTop Then lTop = lMaxTop

    ' Ensure form does not go past top left of screen. This is the highest priority test.
    If lLeft < 0 Then lLeft = 0
    If lTop < 0 Then lTop = 0

    ' Position the form as calculated.
    frmForm.Move lLeft, lTop

End Sub

' Positions the form just below the specified window or control
' on screen.
Public Sub WinDropDown(ByVal frmForm As Object, _
    ByVal hWndControl As Long)

    Dim poszControl As TPositionAndSize
    Dim poszForm As TPositionAndSize

    ' Safety checks.
    If Not IsForm(frmForm, False) Then
        Err.Raise knErrObjectNotForm, ksErrSource, ksErrObjectNotForm
    End If

    ' Get the absolute screen position of the control.
    If Not GetAbsoluteCoords(hWndControl, poszControl) Then
        Exit Sub
    End If

    ' Now calculate screen position of form.
    poszForm.Width = frmForm.Width
    poszForm.Left = poszControl.Left
    If poszForm.Left + poszForm.Width > Screen.Width Then
        poszForm.Left = poszControl.Left - poszForm.Width + poszControl.Width
    End If
    If poszForm.Left + poszForm.Width > Screen.Width Then
        poszForm.Left = poszControl.Left - poszForm.Width
    End If
    If poszForm.Left < 0 Then
        poszForm.Left = 0
    End If

    poszForm.Height = frmForm.Height
    poszForm.Top = poszControl.Top + poszControl.Height
    If poszForm.Top + poszForm.Height > Screen.Height Then
        poszForm.Top = poszControl.Top - poszForm.Height
    End If
    If poszForm.Top < 0 Then
        poszForm.Top = 0
    End If

    ' Position the form, ignoring any remaining errors.
    On Error Resume Next
    frmForm.Move poszForm.Left, poszForm.Top

End Sub

Public Sub WinEnableClose(ByVal frmForm As Object, _
    ByVal bEnabled As Boolean)

    Dim hWnd As Long
    Dim hMenu As Long
    Dim mii As MENUITEMINFO
    Dim lMenuID As Long

    ' Safety checks.
    If Not IsForm(frmForm, False) Then
        Err.Raise knErrObjectNotForm, ksErrSource, ksErrObjectNotForm
    End If

    ' Retrieve a handle to the window's system menu
    hWnd = frmForm.hWnd
    hMenu = GetSystemMenu(hWnd, 0)

    ' Retrieve the menu item information for the Max menu item/button
    mii.cbSize = Len(mii)
    mii.dwTypeData = String$(80, 0)
    mii.cch = Len(mii.dwTypeData)
    mii.fMask = MIIM_STATE

    If bEnabled Then
        mii.wID = xSC_CLOSE
    Else
        mii.wID = SC_CLOSE
    End If

    If GetMenuItemInfo(hMenu, mii.wID, False, mii) = 0 Then
        ' Menu Item Not Found
        Exit Sub
    End If

    ' Switch the ID of the menu item so that VB can not undo the action itself
    lMenuID = mii.wID

    If bEnabled Then
        mii.wID = SC_CLOSE
    Else
        mii.wID = xSC_CLOSE
    End If

    mii.fMask = MIIM_ID
    If SetMenuItemInfo(hMenu, lMenuID, False, mii) = 0 Then
        ' Error encountered while changing ID
        Exit Sub
    End If

    ' Set the enabled / disabled state of the menu item
    If bEnabled Then
        mii.fState = mii.fState And Not MFS_GRAYED
    Else
        mii.fState = mii.fState Or MFS_GRAYED
    End If

    mii.fMask = MIIM_STATE
    If SetMenuItemInfo(hMenu, mii.wID, False, mii) = 0 Then
        ' Error encountered while changing state
        Exit Sub
    End If

    ' Activate the non-client area of the window to update
    ' the titlebar.
    SendMessage hWnd, WM_NCACTIVATE, True, 0

End Sub

' Returns True for any object derived from Form or MDIForm or,
' optionally, set to Nothing.
Private Function IsForm(ByVal o As Object, _
    ByVal bAllowNothing As Boolean) As Boolean

    IsForm = False
    If o Is Nothing Then
        IsForm = bAllowNothing
    ElseIf TypeOf o Is VB.Form Then
        IsForm = True
    End If

End Function

' Get Window size and position in absolute screen coordinates
' (twips). This is only a separate function in case we need to
' expose it as public in future.
Private Function GetAbsoluteCoords(ByVal hWnd As Long, _
    ByRef poszWindow As TPositionAndSize) As Boolean

    Dim rcWin As RECT
    Dim bSuccess As Boolean

    bSuccess = GetWindowRect(hWnd, rcWin)
    GetAbsoluteCoords = bSuccess

    If Not bSuccess Then Exit Function

    With rcWin
        poszWindow.Left = .Left * Screen.TwipsPerPixelX
        poszWindow.Top = .Top * Screen.TwipsPerPixelY
        poszWindow.Width = (.Right - .Left) * Screen.TwipsPerPixelX
        poszWindow.Height = (.Bottom - .Top) * Screen.TwipsPerPixelY
    End With

End Function
