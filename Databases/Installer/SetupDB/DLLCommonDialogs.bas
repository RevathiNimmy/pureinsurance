Attribute VB_Name = "MDLLCommonDialogs"
' Module:   Common dialog functions
' Shared:   Yes (RESTRICTED)
' Needs:    MDLLString
'
' THIS CODE IMPLEMENTS CORRESPONDING FUNCTIONS IN THE DLL.
' IT IS SHARED *ONLY* TO SUPPORT SMALL UTILITIES THAT
' CANNOT REFERENCE THE DLL. *DO NOT* ALTER THIS CODE IN ANY
' WAY UNLESS YOU ARE CHANGING THE INTERNALS OF THE DLL.
'
' To avoid pulling in unnecessary references, this module
' requires conditional compilation constants to be defined.
' * IncludeDialogFont = 1   includes font support
'
Option Explicit

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Windows API Constants
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Private Const CC_ANYCOLOR = &H100&
Private Const CC_ENABLEHOOK = &H10&
Private Const CC_ENABLETEMPLATE = &H20&
Private Const CC_ENABLETEMPLATEHANDLE = &H40&
Private Const CC_FULLOPEN = &H2&
Private Const CC_PREVENTFULLOPEN = &H4&
Private Const CC_RGBINIT = &H1&
Private Const CC_SHOWHELP = &H8&
Private Const CC_SOLIDCOLOR = &H80&

Private Const CF_APPLY = &H200&
Private Const CF_EFFECTS = &H100&
Private Const CF_ENABLEHOOK = &H8&
Private Const CF_ENABLETEMPLATE = &H10&
Private Const CF_ENABLETEMPLATEHANDLE = &H20&
Private Const CF_FIXEDPITCHONLY = &H4000&
Private Const CF_FORCEFONTEXIST = &H10000
Private Const CF_INITTOLOGFONTSTRUCT = &H40&
Private Const CF_LIMITSIZE = &H2000&
Private Const CF_NOFACESEL = &H80000
Private Const CF_NOSCRIPTSEL = &H800000
Private Const CF_NOSIMULATIONS = &H1000&
Private Const CF_NOSIZESEL = &H200000
Private Const CF_NOSTYLESEL = &H100000
Private Const CF_NOVECTORFONTS = &H800&
Private Const CF_NOVERTFONTS = &H1000000
Private Const CF_OWNERDISPLAY = &H80&
Private Const CF_PRINTERFONTS = &H2&
Private Const CF_PRIVATEFIRST = &H200&
Private Const CF_PRIVATELAST = &H2FF&
Private Const CF_SCALABLEONLY = &H20000
Private Const CF_SCREENFONTS = &H1&
Private Const CF_SCRIPTSONLY = &H400&
Private Const CF_SELECTSCRIPT = &H400000
Private Const CF_SHOWHELP = &H4&
Private Const CF_TTONLY = &H40000
Private Const CF_USESTYLE = &H80&
Private Const CF_WYSIWYG = &H8000&

Private Const LF_FACESIZE = 32

Private Const FW_BOLD = 700

Private Const OFN_ALLOWMULTISELECT = &H200&
Private Const OFN_CREATEPROMPT = &H2000&
Private Const OFN_ENABLEHOOK = &H20&
Private Const OFN_ENABLETEMPLATE = &H40&
Private Const OFN_ENABLETEMPLATEHANDLE = &H80&
Private Const OFN_EXPLORER = &H80000
Private Const OFN_EXTENSIONDIFFERENT = &H400&
Private Const OFN_FILEMUSTEXIST = &H1000&
Private Const OFN_HIDEREADONLY = &H4&
Private Const OFN_LONGNAMES = &H200000
Private Const OFN_NOCHANGEDIR = &H8&
Private Const OFN_NODEREFERENCELINKS = &H100000
Private Const OFN_NOLONGNAMES = &H40000
Private Const OFN_NONETWORKBUTTON = &H20000
Private Const OFN_NOREADONLYRETURN = &H8000&
Private Const OFN_NOTESTFILECREATE = &H10000
Private Const OFN_NOVALIDATE = &H100&
Private Const OFN_OVERWRITEPROMPT = &H2&
Private Const OFN_PATHMUSTEXIST = &H800&
Private Const OFN_READONLY = &H1&
Private Const OFN_SHAREAWARE = &H4000&
Private Const OFN_SHAREFALLTHROUGH = &H2&
Private Const OFN_SHARENOWARN = &H1&
Private Const OFN_SHAREWARN = &H0&
Private Const OFN_SHOWHELP = &H10&
Private Const OFS_MAXPATHNAME = 128

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Windows API Error Constants
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Private Const CDERR_DIALOGFAILURE = &HFFFF&
Private Const CDERR_FINDRESFAILURE = &H6&
Private Const CDERR_INITIALIZATION = &H2&
Private Const CDERR_LOADRESFAILURE = &H7&
Private Const CDERR_LOADSTRFAILURE = &H5&
Private Const CDERR_LOCKRESFAILURE = &H8&
Private Const CDERR_MEMALLOCFAILURE = &H9&
Private Const CDERR_MEMLOCKFAILURE = &HA&
Private Const CDERR_NOHINSTANCE = &H4&
Private Const CDERR_NOHOOK = &HB&
Private Const CDERR_NOTEMPLATE = &H3&
Private Const CDERR_REGISTERMSGFAIL = &HC&
Private Const CDERR_STRUCTSIZE = &H1&

Private Const FNERR_BUFFERTOOSMALL = &H3003&
Private Const FNERR_FILENAMECODES = &H3000&
Private Const FNERR_INVALIDFILENAME = &H3002&
Private Const FNERR_SUBCLASSFAILURE = &H3001&

Private Const CCERR_CHOOSECOLORCODES = &H5000&

Private Const CFERR_CHOOSEFONTCODES = &H2000&
Private Const CFERR_MAXLESSTHANMIN = &H2002&
Private Const CFERR_NOFONTS = &H2001&

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Windows API Structures
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Private Type LOGFONT
    lfHeight As Long
    lfWidth As Long
    lfEscapement As Long
    lfOrientation As Long
    lfWeight As Long
    lfItalic As Byte
    lfUnderline As Byte
    lfStrikeOut As Byte
    lfCharSet As Byte
    lfOutPrecision As Byte
    lfClipPrecision As Byte
    lfQuality As Byte
    lfPitchAndFamily As Byte
    lfFaceName(0 To LF_FACESIZE) As Byte
End Type

Private Type CHOOSECOLOR
    lStructSize As Long
    hWndOwner As Long
    hInstance As Long
    rgbResult As Long
    lpCustColors As Long
    dwFlags As Long
    lCustData As Long
    lpfnHook As Long
    lpTemplateName As String
End Type

Private Type CHOOSEFONT
    lStructSize As Long
    hWndOwner As Long
    hDC As Long
    lpLogFont As Long
    iPointSize As Long
    dwFlags As Long
    rgbColors As Long
    lCustData As Long
    lpfnHook As Long
    lpTemplateName As String
    hInstance As Long
    lpszStyle As String
    nFontType As Integer
    MISSING_ALIGNMENT As Integer
    nSizeMin As Long
    nSizeMax As Long
End Type

Private Type OPENFILENAME
    lStructSize As Long
    hWndOwner As Long
    hInstance As Long
    lpstrFilter As String
    lpstrCustomFilter As String
    nMaxCustFilter As Long
    nFilterIndex As Long
    lpstrFile As String
    nMaxFile As Long
    lpstrFileTitle As String
    nMaxFileTitle As Long
    lpstrInitialDir As String
    lpstrTitle As String
    dwFlags As Long
    nFileOffset As Integer
    nFileExtension As Integer
    lpstrDefExt As String
    lCustData As Long
    lpfnHook As Long
    lpTemplateName As String
End Type

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Windows API Functions
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Private Declare Function CommDlgExtendedError Lib "comdlg32.dll" () As Long

Private Declare Function APIGetSysColor Lib "user32" Alias "GetSysColor" (ByVal nIndex As Long) As Long

Private Declare Function APIChooseColor Lib "comdlg32.dll" Alias "ChooseColorA" (ByRef lpChooseColor As CHOOSECOLOR) As Long
Private Declare Function APIChooseFont Lib "comdlg32.dll" Alias "ChooseFontA" (ByRef lpChooseFont As CHOOSEFONT) As Long

Private Declare Function APIGetOpenFileName Lib "comdlg32.dll" Alias "GetOpenFileNameA" (ByRef lpOpenFilename As OPENFILENAME) As Long
Private Declare Function APIGetSaveFileName Lib "comdlg32.dll" Alias "GetSaveFileNameA" (ByRef lpOpenFilename As OPENFILENAME) As Long

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Private Variables
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

' Use a large buffer to accommodate all possible lengths.
Private Const klFilespecBufferLength = 1024&

' Array of custom colors, lasts for whole life of application.
Private Const knFirstColour = 0
Private Const knLastColour = 15
Private m_alCustomColours(knFirstColour To knLastColour) As Long

' Error declarations.
Const ksErrSource = "iSWServices.GCommonDialogs"
Const knErrObjectNotTextBox = 13
Const ksErrObjectNotTextBox = "Type Mismatch (VB TextBox, ComboBox or UEditString object required)."

' Display a Colour dialog. Returns True if the user selected
' one. Returns the selected colour by reference.
Public Function DialogColour(ByRef r_lColour As Long, _
    Optional ByVal bAllowCustomise As Boolean = False, _
    Optional ByVal bShowCustomise As Boolean = False, _
    Optional ByVal hWndOwner As Long = 0&) As Boolean

    Dim lpChooseColor As CHOOSECOLOR
    Dim bSuccess As Long

    DialogColour = False

    ' Initialise required structures.
    With lpChooseColor
        .lStructSize = Len(lpChooseColor)
        .hWndOwner = hWndOwner
        .rgbResult = r_lColour
        .lpCustColors = VarPtr(m_alCustomColours(0))
        .dwFlags = CC_ANYCOLOR Or CC_RGBINIT Or _
            IIf(bAllowCustomise, 0, CC_PREVENTFULLOPEN) Or _
            IIf(bShowCustomise, CC_FULLOPEN, 0)
    End With

    ' Call the API function (and raise an error if it failed).
    bSuccess = APIChooseColor(lpChooseColor)
    If bSuccess = 0 Then
        RaiseComDlgError CommDlgExtendedError()
        Exit Function
    End If

    ' Return results.
    r_lColour = lpChooseColor.rgbResult

    DialogColour = True

End Function

#If IncludeDialogFont Then
' Display a Font dialog. Returns True if the user selected one.
' Returns the selected font by reference.
Public Function DialogFont(ByRef r_fonFont As Font, _
    ByRef r_lColour As Long, _
    Optional ByVal bShowScreenFonts As Boolean = True, _
    Optional ByVal bShowPrinterFonts As Boolean = False, _
    Optional ByVal bShowTTOnly As Boolean = False, _
    Optional ByVal bShowFixedOnly As Boolean = False, _
    Optional ByVal bShowScalableOnly As Boolean = False, _
    Optional ByVal bShowScriptOnly As Boolean = False, _
    Optional ByVal bShowNoSimulations As Boolean = False, _
    Optional ByVal bShowNoVectorFonts As Boolean = False, _
    Optional ByVal bShowHorizontalOnly As Boolean = False, _
    Optional ByVal bShowWYSIWYG As Boolean = False, _
    Optional ByVal lMinimumSize As Long = 0&, _
    Optional ByVal lMaximumSize As Long = 0&, _
    Optional ByVal hDCPrinter As Long = 0&, _
    Optional ByVal hWndOwner As Long = 0&) As Boolean

    Const knTwipsPerPoint = 20
    Dim lpLogFont As LOGFONT
    Dim lpChooseFont As CHOOSEFONT
    Dim baFaceName As New CByteArray
    Dim i As Long
    Dim bSuccess As Long

    DialogFont = False

    ' Initialise required structures.
    With lpChooseFont
        .lStructSize = Len(lpChooseFont)
        .hWndOwner = hWndOwner
        .hDC = hDCPrinter
        .rgbColors = r_lColour
        .nSizeMin = lMinimumSize
        .nSizeMax = lMaximumSize
        .dwFlags = CF_EFFECTS Or CF_INITTOLOGFONTSTRUCT Or _
            IIf(lMinimumSize > 0 Or lMaximumSize > 0, CF_LIMITSIZE, 0) Or _
            IIf(bShowScreenFonts, CF_SCREENFONTS, 0) Or _
            IIf(bShowPrinterFonts, CF_PRINTERFONTS, 0) Or _
            IIf(bShowTTOnly, CF_TTONLY, 0) Or _
            IIf(bShowFixedOnly, CF_FIXEDPITCHONLY, 0) Or _
            IIf(bShowScalableOnly, CF_SCALABLEONLY, 0) Or _
            IIf(bShowScriptOnly, CF_SCRIPTSONLY, 0) Or _
            IIf(bShowNoSimulations, CF_NOSIMULATIONS, 0) Or _
            IIf(bShowNoVectorFonts, CF_NOVECTORFONTS, 0) Or _
            IIf(bShowHorizontalOnly, CF_NOVERTFONTS, 0) Or _
            IIf(bShowWYSIWYG, CF_WYSIWYG, 0)
    End With

    ' If a font was passed in, then initialise all structure
    ' members with properties of the font. Otherwise, initialise
    ' all members with default values, because if we don't then
    ' the API function won't actually return the font selected!
    If Not r_fonFont Is Nothing Then
        lpLogFont.lfHeight = -r_fonFont.Size * knTwipsPerPoint / Screen.TwipsPerPixelY
        lpLogFont.lfWeight = r_fonFont.Weight
        lpLogFont.lfItalic = r_fonFont.Italic
        lpLogFont.lfUnderline = r_fonFont.Underline
        lpLogFont.lfStrikeOut = r_fonFont.Strikethrough
        ' Convert the font name into a small fixed-length ANSI
        ' buffer.
        baFaceName.ValueANSI = r_fonFont.Name
        baFaceName.Resize LF_FACESIZE + 1, True
        For i = 0 To LF_FACESIZE
            lpLogFont.lfFaceName(i) = baFaceName.ItemByte(i)
        Next
        lpChooseFont.iPointSize = r_fonFont.Size * 10
    End If

    ' Only set the pointer when the LOGFONT structure
    ' is completely initialised.
    lpChooseFont.lpLogFont = VarPtr(lpLogFont)

    ' Call the API function (and raise an error if it failed).
    bSuccess = APIChooseFont(lpChooseFont)
    If bSuccess = 0 Then
        RaiseComDlgError CommDlgExtendedError()
        Exit Function
    End If

    ' Create a new font object if the calling procedure
    ' didn't actually pass one in.
    If r_fonFont Is Nothing Then
        Set r_fonFont = New StdFont
    End If

    ' Return results.
    r_lColour = lpChooseFont.rgbColors
    r_fonFont.Bold = (lpLogFont.lfWeight = FW_BOLD)
    r_fonFont.Italic = lpLogFont.lfItalic
    r_fonFont.Strikethrough = lpLogFont.lfStrikeOut
    r_fonFont.Underline = lpLogFont.lfUnderline
    r_fonFont.Weight = lpLogFont.lfWeight
    r_fonFont.Size = lpChooseFont.iPointSize / 10
    ' Convert the font name from the fixed-length ANSI buffer
    ' back into a variable-length Unicode string.
    baFaceName.ValueArray = lpLogFont.lfFaceName
    r_fonFont.Name = RemoveTZ(baFaceName.ValueANSI)

    DialogFont = True

End Function
#End If

' Displays a File Open dialog. Returns the complete filespec
' if selected, or blank if the user cancelled.
Public Function DialogFileOpen( _
    Optional ByVal sInitialFolder As String = "", _
    Optional ByVal sInitialFilename As String = "", _
    Optional ByVal sTitle As String = "", _
    Optional ByVal sFilter As String = "", _
    Optional ByVal sDefaultExtension As String = "", _
    Optional ByVal hWndOwner As Long = 0&) As String

    Dim lpOpenFilename As OPENFILENAME
    Dim bSuccess As Long
    Dim sFilenameBuffer As String
    Dim sFolderBuffer As String

    DialogFileOpen = ""

    ' Initialise required structures.
    sFilenameBuffer = Left$(sInitialFilename & String$(klFilespecBufferLength, 0), klFilespecBufferLength)
    sFolderBuffer = sInitialFolder

    With lpOpenFilename
        .lStructSize = Len(lpOpenFilename)
        .hWndOwner = hWndOwner
        .lpstrFilter = VBFilterToAPIFilter(sFilter)
        .lpstrFile = sFilenameBuffer
        .nMaxFile = klFilespecBufferLength
        .lpstrFileTitle = String$(klFilespecBufferLength, 0)
        .nMaxFileTitle = klFilespecBufferLength
        .lpstrInitialDir = sFolderBuffer
        .lpstrTitle = sTitle
        .dwFlags = OFN_FILEMUSTEXIST Or OFN_HIDEREADONLY
        '.nFileOffset = ???
        '.nFileExtension = ???
        .lpstrDefExt = sDefaultExtension
        ' TODO: sDefaultExtension does not seem to work
    End With

    ' Call the API function (and raise an error if it failed).
    bSuccess = APIGetOpenFileName(lpOpenFilename)
    If bSuccess = 0 Then
        RaiseComDlgError CommDlgExtendedError()
        Exit Function
    End If

    ' Return results.
    DialogFileOpen = RemoveTZ(lpOpenFilename.lpstrFile)

End Function

' Displays a File Save As dialog. Returns the complete filespec
' if selected, or blank if the user cancelled.
Public Function DialogFileSaveAs( _
    Optional ByVal sInitialFolder As String = "", _
    Optional ByVal sInitialFilename As String = "", _
    Optional ByVal sTitle As String = "", _
    Optional ByVal sFilter As String = "", _
    Optional ByVal sDefaultExtension As String = "", _
    Optional ByVal hWndOwner As Long = 0&) As String

    Dim lpOpenFilename As OPENFILENAME
    Dim bSuccess As Long
    Dim sFilenameBuffer As String
    Dim sFolderBuffer As String

    DialogFileSaveAs = ""

    ' Initialise required structures.
    sFilenameBuffer = Left$(sInitialFilename & String$(klFilespecBufferLength, 0), klFilespecBufferLength)
    sFolderBuffer = sInitialFolder

    With lpOpenFilename
        .lStructSize = Len(lpOpenFilename)
        .hWndOwner = hWndOwner
        .lpstrFilter = VBFilterToAPIFilter(sFilter)
        .lpstrFile = sFilenameBuffer
        .nMaxFile = klFilespecBufferLength
        .lpstrFileTitle = String$(klFilespecBufferLength, 0)
        .nMaxFileTitle = klFilespecBufferLength
        .lpstrInitialDir = sFolderBuffer
        .lpstrTitle = sTitle
        .dwFlags = OFN_PATHMUSTEXIST Or OFN_HIDEREADONLY Or OFN_OVERWRITEPROMPT
        '.nFileOffset = ???
        '.nFileExtension = ???
        .lpstrDefExt = sDefaultExtension
        ' TODO: sDefaultExtension does not seem to work
    End With

    ' Call the API function (and raise an error if it failed).
    bSuccess = APIGetSaveFileName(lpOpenFilename)
    If bSuccess = 0 Then
        RaiseComDlgError CommDlgExtendedError()
        Exit Function
    End If

    ' Return results.
    DialogFileSaveAs = RemoveTZ(lpOpenFilename.lpstrFile)

End Function

' Displays a Folder Open dialog. Returns the folder path
' if selected, or blank if the user cancelled.
Public Function DialogFolderOpen( _
    Optional ByVal sInitialFolder As String = "", _
    Optional ByVal sTitle As String = "", _
    Optional ByVal sFilter As String = "", _
    Optional ByVal sSuggestionText As String = "", _
    Optional ByVal hWndOwner As Long = 0&) As String

    ' We simulate a folder select dialog by using a normal
    ' file select dialog with the following changes:
    ' * User can type in a dummy filename without error
    ' * Specify "help text" instead of an initial filename or extension
    ' * Returns the folder path with the filename stripped off

    Dim lpOpenFilename As OPENFILENAME
    Dim bSuccess As Long
    Dim sFilenameBuffer As String
    Dim sFolderBuffer As String

    DialogFolderOpen = ""

    ' Initialise required structures.
    sFilenameBuffer = Left$(sSuggestionText & String$(klFilespecBufferLength, 0), klFilespecBufferLength)
    sFolderBuffer = sInitialFolder

    With lpOpenFilename
        .lStructSize = Len(lpOpenFilename)
        .hWndOwner = hWndOwner
        .lpstrFilter = VBFilterToAPIFilter(sFilter)
        .lpstrFile = sFilenameBuffer
        .nMaxFile = klFilespecBufferLength
        .lpstrFileTitle = String$(klFilespecBufferLength, 0)
        .nMaxFileTitle = klFilespecBufferLength
        .lpstrInitialDir = sFolderBuffer
        .lpstrTitle = sTitle
        .dwFlags = OFN_PATHMUSTEXIST Or OFN_HIDEREADONLY
    End With

    ' Call the API function (and raise an error if it failed).
    bSuccess = APIGetOpenFileName(lpOpenFilename)
    If bSuccess = 0 Then
        RaiseComDlgError CommDlgExtendedError()
        Exit Function
    End If

    ' Return results.
    sFolderBuffer = RemoveTZ(lpOpenFilename.lpstrFile)
    DialogFolderOpen = ParsePathFile(sFolderBuffer, "")

End Function

Public Function DialogFileOpenCtrl(ByVal txtText As Object, _
    Optional ByVal sTitle As String = "", _
    Optional ByVal sFilter As String = "", _
    Optional ByVal sDefaultExtension As String = "", _
    Optional ByVal hWndOwner As Long = 0&) As String

    Dim sFolder As String
    Dim sFileName As String

    ' Safety checks.
    If Not IsTextBox(txtText, False) Then
        Err.Raise knErrObjectNotTextBox, ksErrSource, ksErrObjectNotTextBox
    End If

    sFolder = ParsePathFile(txtText.Text, sFileName)

    sFileName = DialogFileOpen(sFolder, sFileName, sTitle, sFilter, sDefaultExtension, hWndOwner)

    If sFileName <> "" Then
        txtText.Text = sFileName
    End If

End Function

Public Function DialogFileSaveAsCtrl(ByVal txtText As Object, _
    Optional ByVal sTitle As String = "", _
    Optional ByVal sFilter As String = "", _
    Optional ByVal sDefaultExtension As String = "", _
    Optional ByVal hWndOwner As Long = 0&) As String

    Dim sFolder As String
    Dim sFileName As String

    ' Safety checks.
    If Not IsTextBox(txtText, False) Then
        Err.Raise knErrObjectNotTextBox, ksErrSource, ksErrObjectNotTextBox
    End If

    sFolder = ParsePathFile(txtText.Text, sFileName)

    sFileName = DialogFileSaveAs(sFolder, sFileName, sTitle, sFilter, sDefaultExtension, hWndOwner)

    If sFileName <> "" Then
        txtText.Text = sFileName
    End If

End Function

Public Function DialogFolderOpenCtrl(ByVal txtText As Object, _
    Optional ByVal sTitle As String = "", _
    Optional ByVal sFilter As String = "", _
    Optional ByVal sSuggestionText As String = "", _
    Optional ByVal hWndOwner As Long = 0&) As String

    Dim sFolder As String

    ' Safety checks.
    If Not IsTextBox(txtText, False) Then
        Err.Raise knErrObjectNotTextBox, ksErrSource, ksErrObjectNotTextBox
    End If

    sFolder = txtText.Text

    sFolder = DialogFolderOpen(sFolder, sTitle, sFilter, sSuggestionText, hWndOwner)

    If sFolder <> "" Then
        txtText.Text = sFolder
    End If

End Function

' Access all custom colours that the user has defined.
Public Property Get CustomColour(ByVal iColour As Integer) As Long

    If iColour >= knFirstColour And iColour <= knLastColour Then
        CustomColour = m_alCustomColours(iColour)
    Else
        CustomColour = -1 ' set to an obviously illegal value
    End If

End Property

' Access all custom colours that the user has defined.
Public Property Let CustomColour(ByVal iColour As Integer, ByVal lColour As Long)

    If iColour >= knFirstColour And iColour <= knLastColour Then
        m_alCustomColours(iColour) = lColour
    End If

End Property

Public Sub CustomColourInitialise()

    ' Set all colours to grey (normal defaults).
    Const klGrey = &HC0C0C0
    Dim i As Integer

    For i = knFirstColour To knLastColour
        m_alCustomColours(i) = klGrey
    Next

End Sub

Public Sub CustomColourTerminate()

    ' Set all colours to zero.
    Erase m_alCustomColours

End Sub

Private Function VBFilterToAPIFilter(ByVal sVBFilter As String) As String

    VBFilterToAPIFilter = Replace$(Trim$(sVBFilter), "|", vbNullChar) & vbNullChar & vbNullChar

End Function

Private Sub RaiseComDlgError(ByVal lComDlgError As Long)

    Const knOffset = VBA.Constants.vbObjectError + &H200&

    Select Case lComDlgError
    Case 0 ' No Error
        Exit Sub
    Case Else
        Err.Raise knOffset + lComDlgError, "Windows", "Common Dialog returned an unrecognised error (0x" & Hex$(lComDlgError) & ")."
    End Select

End Sub

' Returns True for a TextBox or ComboBox object or,
' optionally, set to Nothing.
Private Function IsTextBox(ByVal o As Object, _
    ByVal bAllowNothing As Boolean) As Boolean

    IsTextBox = False
    If o Is Nothing Then
        IsTextBox = bAllowNothing
    Else
        Select Case TypeName(o)
            Case "TextBox", "ComboBox", "UEditString"
                IsTextBox = True
            Case Else
                IsTextBox = False
        End Select
    End If

End Function
