Attribute VB_Name = "MStandardIcons"
' Module:   Draw standard Windows icons
' Shared:   Yes
' Needs:    Nothing
'
Option Explicit

Public Enum EStandardIcons
    knIconInformation = &H7F04&
    knIconExclamation = &H7F03&
    knIconError = &H7F01&
    knIconQuestion = &H7F02&
    knIconApplication = &H7F00&
    knIconWindowsLogo = &H7F05&
End Enum

Private Declare Function LoadIcon Lib "user32" Alias "LoadIconA" _
    (ByVal hInstance As Long, ByVal nIcon As EStandardIcons) As Long
Private Declare Function DrawIcon Lib "user32" _
    (ByVal hDC As Long, ByVal X As Long, ByVal Y As Long, ByVal hIcon As Long) As Long

' Draws a Windows standard icon on the specified
' device context. Coordinates are in pixels not twips.
Public Sub DrawStandardIcon(ByVal hDC As Long, _
    ByVal X As Long, _
    ByVal Y As Long, _
    ByVal nIcon As EStandardIcons)

    DrawIcon hDC, X, Y, LoadIcon(0, nIcon)

End Sub
