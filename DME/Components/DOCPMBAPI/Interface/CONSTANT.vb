Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
Module CONSTANT
	''''''''''''''''''''''''''''
	' Visual Basic global constant file. This file can be loaded
	' into a code module.
	'
	' Some constants are commented out because they have
	' duplicates (e.g., NONE appears several places).
	'
	' If you are updating a Visual Basic application written with
	' an older version, you should replace your global constants
	' with the constants in this file.
	'
	''''''''''''''''''''''''''''
	
	' General
	
	' Clipboard formats
	Public Const CF_LINK As Integer = &HBF00s
	Public Const CF_TEXT As Integer = 1
	Public Const CF_BITMAP As Integer = 2
	Public Const CF_METAFILE As Integer = 3
	Public Const CF_DIB As Integer = 8
	Public Const CF_PALETTE As Integer = 9
	
	' DragOver
	Public Const ENTER As Integer = 0
	Public Const LEAVE As Integer = 1
	Public Const OVER As Integer = 2
	
	' Drag (controls)
	Public Const CANCEL As Integer = 0
	Public Const BEGIN_DRAG As Integer = 1
	Public Const END_DRAG As Integer = 2
	
	' Show parameters
	Public Const MODAL As FormShowConstants = FormShowConstants.Modal
	Public Const MODELESS As Integer = 0
	
	' Arrange Method
	' for MDI Forms
	Public Const CASCADE As Integer = 0
	Public Const TILE_HORIZONTAL As Integer = 1
	Public Const TILE_VERTICAL As Integer = 2
	Public Const ARRANGE_ICONS As Integer = 3
	
	'ZOrder Method
	Public Const BRINGTOFRONT As Integer = 0
	Public Const SENDTOBACK As Integer = 1
	
	' Key Codes
	Public Const KEY_LBUTTON As Integer = &H1s
	Public Const KEY_RBUTTON As Integer = &H2s
	Public Const KEY_CANCEL As Integer = &H3s
	Public Const KEY_MBUTTON As Integer = &H4s ' NOT contiguous with L & RBUTTON
	Public Const KEY_BACK As Integer = &H8s
	Public Const KEY_TAB As Integer = &H9s
	Public Const KEY_CLEAR As Integer = &HCs
	Public Const KEY_RETURN As Integer = &HDs
	Public Const KEY_SHIFT As Integer = &H10s
	Public Const KEY_CONTROL As Integer = &H11s
	Public Const KEY_MENU As Integer = &H12s
	Public Const KEY_PAUSE As Integer = &H13s
	Public Const KEY_CAPITAL As Integer = &H14s
	Public Const KEY_ESCAPE As Integer = &H1Bs
	Public Const KEY_SPACE As Integer = &H20s
	Public Const KEY_PRIOR As Integer = &H21s
	Public Const KEY_NEXT As Integer = &H22s
	Public Const KEY_END As Integer = &H23s
	Public Const KEY_HOME As Integer = &H24s
	Public Const KEY_LEFT As Integer = &H25s
	Public Const KEY_UP As Integer = &H26s
	Public Const KEY_RIGHT As Integer = &H27s
	Public Const KEY_DOWN As Integer = &H28s
	Public Const KEY_SELECT As Integer = &H29s
	Public Const KEY_PRINT As Integer = &H2As
	Public Const KEY_EXECUTE As Integer = &H2Bs
	Public Const KEY_SNAPSHOT As Integer = &H2Cs
	Public Const KEY_INSERT As Integer = &H2Ds
	Public Const KEY_DELETE As Integer = &H2Es
	Public Const KEY_HELP As Integer = &H2Fs
	
	' KEY_A thru KEY_Z are the same as their ASCII equivalents: 'A' thru 'Z'
	' KEY_0 thru KEY_9 are the same as their ASCII equivalents: '0' thru '9'
	
	Public Const KEY_NUMPAD0 As Integer = &H60s
	Public Const KEY_NUMPAD1 As Integer = &H61s
	Public Const KEY_NUMPAD2 As Integer = &H62s
	Public Const KEY_NUMPAD3 As Integer = &H63s
	Public Const KEY_NUMPAD4 As Integer = &H64s
	Public Const KEY_NUMPAD5 As Integer = &H65s
	Public Const KEY_NUMPAD6 As Integer = &H66s
	Public Const KEY_NUMPAD7 As Integer = &H67s
	Public Const KEY_NUMPAD8 As Integer = &H68s
	Public Const KEY_NUMPAD9 As Integer = &H69s
	Public Const KEY_MULTIPLY As Integer = &H6As
	Public Const KEY_ADD As Integer = &H6Bs
	Public Const KEY_SEPARATOR As Integer = &H6Cs
	Public Const KEY_SUBTRACT As Integer = &H6Ds
	Public Const KEY_DECIMAL As Integer = &H6Es
	Public Const KEY_DIVIDE As Integer = &H6Fs
	Public Const KEY_F1 As Integer = &H70s
	Public Const KEY_F2 As Integer = &H71s
	Public Const KEY_F3 As Integer = &H72s
	Public Const KEY_F4 As Integer = &H73s
	Public Const KEY_F5 As Integer = &H74s
	Public Const KEY_F6 As Integer = &H75s
	Public Const KEY_F7 As Integer = &H76s
	Public Const KEY_F8 As Integer = &H77s
	Public Const KEY_F9 As Integer = &H78s
	Public Const KEY_F10 As Integer = &H79s
	Public Const KEY_F11 As Integer = &H7As
	Public Const KEY_F12 As Integer = &H7Bs
	Public Const KEY_F13 As Integer = &H7Cs
	Public Const KEY_F14 As Integer = &H7Ds
	Public Const KEY_F15 As Integer = &H7Es
	Public Const KEY_F16 As Integer = &H7Fs
	
	Public Const KEY_NUMLOCK As Integer = &H90s
	
	' Variant VarType tags
	
	Public Const V_EMPTY As Integer = 0
	Public Const V_NULL As Integer = 1
	Public Const V_INTEGER As Integer = 2
	Public Const V_LONG As Integer = 3
	Public Const V_SINGLE As Integer = 4
	Public Const V_DOUBLE As Integer = 5
	Public Const V_CURRENCY As Integer = 6
	Public Const V_DATE As Integer = 7
	Public Const V_STRING As Integer = 8
	
	
	' Event Parameters
	
	' ErrNum (LinkError)
	Public Const WRONG_FORMAT As Integer = 1
	Public Const DDE_SOURCE_CLOSED As Integer = 6
	Public Const TOO_MANY_LINKS As Integer = 7
	Public Const DATA_TRANSFER_FAILED As Integer = 8
	
	' QueryUnload
	Public Const FORM_CONTROLMENU As Integer = 0
	Public Const FORM_CODE As Integer = 1
	Public Const APP_WINDOWS As Integer = 2
	Public Const APP_TASKMANAGER As Integer = 3
	Public Const FORM_MDIFORM As Integer = 4
	
	' Properties
	
	' Colors
	Public Const BLACK As Integer = &H0
	Public Const RED As Integer = &HFF
	Public Const GREEN As Integer = &HFF00
	Public Const YELLOW As Integer = &HFFFF
	Public Const BLUE As Integer = &HFF0000
	Public Const MAGENTA As Integer = &HFF00FF
	Public Const CYAN As Integer = &HFFFF00
	Public Const WHITE As Integer = &HFFFFFF
	
	' System Colors
	Public Const SCROLL_BARS As Integer = &H80000000 ' Scroll-bars gray area.
	Public Const DESKTOP As Integer = &H80000001 ' Desktop.
	Public Const ACTIVE_TITLE_BAR As Integer = &H80000002 ' Active window caption.
	Public Const INACTIVE_TITLE_BAR As Integer = &H80000003 ' Inactive window caption.
	Public Const MENU_BAR As Integer = &H80000004 ' Menu background.
	Public Const WINDOW_BACKGROUND As Integer = &H80000005 ' Window background.
	Public Const WINDOW_FRAME As Integer = &H80000006 ' Window frame.
	Public Const MENU_TEXT As Integer = &H80000007 ' Text in menus.
	Public Const WINDOW_TEXT As Integer = &H80000008 ' Text in windows.
	Public Const TITLE_BAR_TEXT As Integer = &H80000009 ' Text in caption, size box, scroll-bar arrow box..
	Public Const ACTIVE_BORDER As Integer = &H8000000A ' Active window border.
	Public Const INACTIVE_BORDER As Integer = &H8000000B ' Inactive window border.
	Public Const APPLICATION_WORKSPACE As Integer = &H8000000C ' Background color of multiple document interface (MDI) applications.
	Public Const HIGHLIGHT As Integer = &H8000000D ' Items selected item in a control.
	Public Const HIGHLIGHT_TEXT As Integer = &H8000000E ' Text of item selected in a control.
	Public Const BUTTON_FACE As Integer = &H8000000F ' Face shading on command buttons.
	Public Const BUTTON_SHADOW As Integer = &H80000010 ' Edge shading on command buttons.
	Public Const GRAY_TEXT As Integer = &H80000011 ' Grayed (disabled) text.  This color is set to 0 if the current display driver does not support a solid gray color.
	Public Const BUTTON_TEXT As Integer = &H80000012 ' Text on push buttons.
	
	' Enumerated Types
	
	' Align (picture box)
	Public Const NONE As Integer = 0
	Public Const ALIGN_TOP As Integer = 1
	Public Const ALIGN_BOTTOM As Integer = 2
	
	' Alignment
	Public Const LEFT_JUSTIFY As Integer = 0 ' 0 - Left Justify
	Public Const RIGHT_JUSTIFY As Integer = 1 ' 1 - Right Justify
	Public Const CENTER As Integer = 2 ' 2 - Center
	
	' BorderStyle (form)
	'Global Const NONE = 0          ' 0 - None
	Public Const FIXED_SINGLE As Integer = 1 ' 1 - Fixed Single
	Public Const SIZABLE As Integer = 2 ' 2 - Sizable (Forms only)
	Public Const FIXED_DOUBLE As FormBorderStyle = FormBorderStyle.FixedDialog ' 3 - Fixed Double (Forms only)
	
	' BorderStyle (Shape and Line)
	'Global Const TRANSPARENT = 0    '0 - Transparent
	'Global Const SOLID = 1          '1 - Solid
	'Global Const DASH = 2         ' 2 - Dash
	'Global Const DOT = 3          ' 3 - Dot
	'Global Const DASH_DOT = 4     ' 4 - Dash-Dot
	'Global Const DASH_DOT_DOT = 5 ' 5 - Dash-Dot-Dot
	'Global Const INSIDE_SOLID = 6 ' 6 - Inside Solid
	
	' MousePointer
	Public Const DEFAULT_Renamed As Integer = 0 ' 0 - Default
	Public Const ARROW As Integer = 1 ' 1 - Arrow
	Public Const CROSSHAIR As Integer = 2 ' 2 - Cross
	Public Const IBEAM As Integer = 3 ' 3 - I-Beam
	Public Const ICON_POINTER As Integer = 4 ' 4 - Icon
	Public Const SIZE_POINTER As Integer = 5 ' 5 - Size
	Public Const SIZE_NE_SW As Integer = 6 ' 6 - Size NE SW
	Public Const SIZE_N_S As Integer = 7 ' 7 - Size N S
	Public Const SIZE_NW_SE As Integer = 8 ' 8 - Size NW SE
	Public Const SIZE_W_E As Integer = 9 ' 9 - Size W E
	Public Const UP_ARROW As Integer = 10 ' 10 - Up Arrow
	Public Const HOURGLASS As Integer = 11 ' 11 - Hourglass
	Public Const NO_DROP As Integer = 12 ' 12 - No drop
	
	' DragMode
	Public Const MANUAL As Integer = 0 ' 0 - Manual
	Public Const AUTOMATIC As Integer = 1 ' 1 - Automatic
	
	' DrawMode
	Public Const BLACKNESS As Integer = 1 ' 1 - Blackness
	Public Const NOT_MERGE_PEN As Integer = 2 ' 2 - Not Merge Pen
	Public Const MASK_NOT_PEN As Integer = 3 ' 3 - Mask Not Pen
	Public Const NOT_COPY_PEN As Integer = 4 ' 4 - Not Copy Pen
	Public Const MASK_PEN_NOT As Integer = 5 ' 5 - Mask Pen Not
	Public Const INVERT As Integer = 6 ' 6 - Invert
	Public Const XOR_PEN As Integer = 7 ' 7 - Xor Pen
	Public Const NOT_MASK_PEN As Integer = 8 ' 8 - Not Mask Pen
	Public Const MASK_PEN As Integer = 9 ' 9 - Mask Pen
	Public Const NOT_XOR_PEN As Integer = 10 ' 10 - Not Xor Pen
	Public Const NOP As Integer = 11 ' 11 - Nop
	Public Const MERGE_NOT_PEN As Integer = 12 ' 12 - Merge Not Pen
	Public Const COPY_PEN As Integer = 13 ' 13 - Copy Pen
	Public Const MERGE_PEN_NOT As Integer = 14 ' 14 - Merge Pen Not
	Public Const MERGE_PEN As Integer = 15 ' 15 - Merge Pen
	Public Const WHITENESS As Integer = 16 ' 16 - Whiteness
	
	' DrawStyle
	Public Const SOLID As Integer = 0 ' 0 - Solid
	Public Const DASH As Integer = 1 ' 1 - Dash
	Public Const DOT As Integer = 2 ' 2 - Dot
	Public Const DASH_DOT As Integer = 3 ' 3 - Dash-Dot
	Public Const DASH_DOT_DOT As Integer = 4 ' 4 - Dash-Dot-Dot
	Public Const INVISIBLE As Integer = 5 ' 5 - Invisible
	Public Const INSIDE_SOLID As Integer = 6 ' 6 - Inside Solid
	
	' FillStyle
	' Global Const SOLID = 0           ' 0 - Solid
	Public Const TRANSPARENT As Integer = 1 ' 1 - Transparent
	Public Const HORIZONTAL_LINE As Integer = 2 ' 2 - Horizontal Line
	Public Const VERTICAL_LINE As Integer = 3 ' 3 - Vertical Line
	Public Const UPWARD_DIAGONAL As Integer = 4 ' 4 - Upward Diagonal
	Public Const DOWNWARD_DIAGONAL As Integer = 5 ' 5 - Downward Diagonal
	Public Const CROSS As Integer = 6 ' 6 - Cross
	Public Const DIAGONAL_CROSS As Integer = 7 ' 7 - Diagonal Cross
	
	' LinkMode (forms and controls)
	' Global Const NONE = 0         ' 0 - None
	Public Const LINK_SOURCE As Integer = 1 ' 1 - Source (forms only)
	Public Const LINK_AUTOMATIC As Integer = 1 ' 1 - Automatic (controls only)
	Public Const LINK_MANUAL As Integer = 2 ' 2 - Manual (controls only)
	Public Const LINK_NOTIFY As Integer = 3 ' 3 - Notify (controls only)
	
	' LinkMode (kept for VB1.0 compatibility, use new constants instead)
	Public Const HOT As Integer = 1 ' 1 - Hot (controls only)
	Public Const SERVER As Integer = 1 ' 1 - Server (forms only)
	Public Const COLD As Integer = 2 ' 2 - Cold (controls only)
	
	
	' ScaleMode
	Public Const USER As Integer = 0 ' 0 - User
	Public Const TWIPS As Integer = 1 ' 1 - Twip
	Public Const POINTS As Integer = 2 ' 2 - Point
	Public Const PIXELS As Integer = 3 ' 3 - Pixel
	Public Const CHARACTERS As Integer = 4 ' 4 - Character
	Public Const INCHES As Integer = 5 ' 5 - Inch
	Public Const MILLIMETERS As Integer = 6 ' 6 - Millimeter
	Public Const CENTIMETERS As Integer = 7 ' 7 - Centimeter
	
	' ScrollBar
	' Global Const NONE     = 0 ' 0 - None
	Public Const HORIZONTAL As Integer = 1 ' 1 - Horizontal
	Public Const VERTICAL As Integer = 2 ' 2 - Vertical
	Public Const BOTH As Integer = 3 ' 3 - Both
	
	' Shape
	Public Const SHAPE_RECTANGLE As Integer = 0
	Public Const SHAPE_SQUARE As Integer = 1
	Public Const SHAPE_OVAL As Integer = 2
	Public Const SHAPE_CIRCLE As Integer = 3
	Public Const SHAPE_ROUNDED_RECTANGLE As Integer = 4
	Public Const SHAPE_ROUNDED_SQUARE As Integer = 5
	
	' WindowState
	Public Const NORMAL As Integer = 0 ' 0 - Normal
	Public Const MINIMIZED As FormWindowState = FormWindowState.Minimized ' 1 - Minimized
	Public Const MAXIMIZED As Integer = 2 ' 2 - Maximized
	
	' Check Value
	Public Const UNCHECKED As Integer = 0 ' 0 - Unchecked
	Public Const CHECKED As Integer = 1 ' 1 - Checked
	Public Const GRAYED As Integer = 2 ' 2 - Grayed
	
	' Shift parameter masks
	Public Const SHIFT_MASK As Integer = 1
	Public Const CTRL_MASK As Integer = 2
	Public Const ALT_MASK As Integer = 4
	
	' Button parameter masks
	Public Const LEFT_BUTTON As Integer = 1
	Public Const RIGHT_BUTTON As Integer = 2
	Public Const MIDDLE_BUTTON As Integer = 4
	
	' Function Parameters
	' MsgBox parameters
	Public Const MB_OK As Integer = 0 ' OK button only
	Public Const MB_OKCANCEL As Integer = 1 ' OK and Cancel buttons
	Public Const MB_ABORTRETRYIGNORE As Integer = 2 ' Abort, Retry, and Ignore buttons
	Public Const MB_YESNOCANCEL As Integer = 3 ' Yes, No, and Cancel buttons
	Public Const MB_YESNO As Integer = 4 ' Yes and No buttons
	Public Const MB_RETRYCANCEL As Integer = 5 ' Retry and Cancel buttons
	
	Public Const MB_ICONSTOP As MsgBoxStyle = MsgBoxStyle.Critical ' Critical message
	Public Const MB_ICONQUESTION As Integer = 32 ' Warning query
	Public Const MB_ICONEXCLAMATION As MsgBoxStyle = MsgBoxStyle.Exclamation ' Warning message
	Public Const MB_ICONINFORMATION As MsgBoxStyle = MsgBoxStyle.Information ' Information message
	
	Public Const MB_APPLMODAL As Integer = 0 ' Application Modal Message Box
	Public Const MB_DEFBUTTON1 As Integer = 0 ' First button is default
	Public Const MB_DEFBUTTON2 As Integer = 256 ' Second button is default
	Public Const MB_DEFBUTTON3 As Integer = 512 ' Third button is default
	Public Const MB_SYSTEMMODAL As Integer = 4096 'System Modal
	
	' MsgBox return values
	Public Const IDOK As Integer = 1 ' OK button pressed
	Public Const IDCANCEL As Integer = 2 ' Cancel button pressed
	Public Const IDABORT As Integer = 3 ' Abort button pressed
	Public Const IDRETRY As Integer = 4 ' Retry button pressed
	Public Const IDIGNORE As Integer = 5 ' Ignore button pressed
	Public Const IDYES As Integer = 6 ' Yes button pressed
	Public Const IDNO As Integer = 7 ' No button pressed
	
	' SetAttr, Dir, GetAttr functions
	Public Const ATTR_NORMAL As FileAttribute = FileAttribute.Normal
	Public Const ATTR_READONLY As Integer = 1
	Public Const ATTR_HIDDEN As Integer = 2
	Public Const ATTR_SYSTEM As Integer = 4
	Public Const ATTR_VOLUME As Integer = 8
	Public Const ATTR_DIRECTORY As Integer = 16
	Public Const ATTR_ARCHIVE As Integer = 32
	
	'Grid
	'ColAlignment,FixedAlignment Properties
	Public Const GRID_ALIGNLEFT As Integer = 0
	Public Const GRID_ALIGNRIGHT As Integer = 1
	Public Const GRID_ALIGNCENTER As Integer = 2
	
	'Fillstyle Property
	Public Const GRID_SINGLE As Integer = 0
	Public Const GRID_REPEAT As Integer = 1
	
	
	'Data control
	'Error event Response arguments
	Public Const DATA_ERRCONTINUE As Integer = 0
	Public Const DATA_ERRDISPLAY As Integer = 1
	
	'Editmode property values
	Public Const DATA_EDITNONE As Integer = 0
	Public Const DATA_EDITMODE As Integer = 1
	Public Const DATA_EDITADD As Integer = 2
	
	' Options property values
	Public Const DATA_DENYWRITE As Integer = &H1s
	Public Const DATA_DENYREAD As Integer = &H2s
	Public Const DATA_READONLY As Integer = &H4s
	Public Const DATA_APPENDONLY As Integer = &H8s
	Public Const DATA_INCONSISTENT As Integer = &H10s
	Public Const DATA_CONSISTENT As Integer = &H20s
	Public Const DATA_SQLPASSTHROUGH As Integer = &H40s
	
	'Validate event Action arguments
	Public Const DATA_ACTIONCANCEL As Integer = 0
	Public Const DATA_ACTIONMOVEFIRST As Integer = 1
	Public Const DATA_ACTIONMOVEPREVIOUS As Integer = 2
	Public Const DATA_ACTIONMOVENEXT As Integer = 3
	Public Const DATA_ACTIONMOVELAST As Integer = 4
	Public Const DATA_ACTIONADDNEW As Integer = 5
	Public Const DATA_ACTIONUPDATE As Integer = 6
	Public Const DATA_ACTIONDELETE As Integer = 7
	Public Const DATA_ACTIONFIND As Integer = 8
	Public Const DATA_ACTIONBOOKMARK As Integer = 9
	Public Const DATA_ACTIONCLOSE As Integer = 10
	Public Const DATA_ACTIONUNLOAD As Integer = 11
	
	
	'OLE Client Control
	'Actions
	Public Const OLE_CREATE_EMBED As Integer = 0
	Public Const OLE_CREATE_NEW As Integer = 0 'from ole1 control
	Public Const OLE_CREATE_LINK As Integer = 1
	Public Const OLE_CREATE_FROM_FILE As Integer = 1 'from ole1 control
	Public Const OLE_COPY As Integer = 4
	Public Const OLE_PASTE As Integer = 5
	Public Const OLE_UPDATE As Integer = 6
	Public Const OLE_ACTIVATE As Integer = 7
	Public Const OLE_CLOSE As Integer = 9
	Public Const OLE_DELETE As Integer = 10
	Public Const OLE_SAVE_TO_FILE As Integer = 11
	Public Const OLE_READ_FROM_FILE As Integer = 12
	Public Const OLE_INSERT_OBJ_DLG As Integer = 14
	Public Const OLE_PASTE_SPECIAL_DLG As Integer = 15
	Public Const OLE_FETCH_VERBS As Integer = 17
	Public Const OLE_SAVE_TO_OLE1FILE As Integer = 18
	
	'OLEType
	Public Const OLE_LINKED As Integer = 0
	Public Const OLE_EMBEDDED As Integer = 1
	Public Const OLE_NONE As Integer = 3
	
	'OLETypeAllowed
	Public Const OLE_EITHER As Integer = 2
	
	'UpdateOptions
	Public Const OLE_AUTOMATIC As Integer = 0
	Public Const OLE_FROZEN As Integer = 1
	Public Const OLE_MANUAL As Integer = 2
	
	'AutoActivate modes
	'Note that OLE_ACTIVATE_GETFOCUS only applies to objects that
	'support "inside-out" activation.  See related Verb notes below.
	Public Const OLE_ACTIVATE_MANUAL As Integer = 0
	Public Const OLE_ACTIVATE_GETFOCUS As Integer = 1
	Public Const OLE_ACTIVATE_DOUBLECLICK As Integer = 2
	
	'SizeModes
	Public Const OLE_SIZE_CLIP As Integer = 0
	Public Const OLE_SIZE_STRETCH As Integer = 1
	Public Const OLE_SIZE_AUTOSIZE As Integer = 2
	
	'DisplayTypes
	Public Const OLE_DISPLAY_CONTENT As Integer = 0
	Public Const OLE_DISPLAY_ICON As Integer = 1
	
	'Update Event Constants
	Public Const OLE_CHANGED As Integer = 0
	Public Const OLE_SAVED As Integer = 1
	Public Const OLE_CLOSED As Integer = 2
	Public Const OLE_RENAMED As Integer = 3
	
	'Special Verb Values
	Public Const VERB_PRIMARY As Integer = 0
	Public Const VERB_SHOW As Integer = -1
	Public Const VERB_OPEN As Integer = -2
	Public Const VERB_HIDE As Integer = -3
	Public Const VERB_INPLACEUIACTIVATE As Integer = -4
	Public Const VERB_INPLACEACTIVATE As Integer = -5
	'The last two verbs are for objects that support "inside-out" activation,
	'meaning they can be edited in-place, and that they support being left
	'in-place-active even when the input focus moves to another control or form.
	'These objects actually have 2 levels of being active.  "InPlace Active"
	'means that the object is ready for the user to click inside it and start
	'working with it.  "In-Place UI-Active" means that, in addition, if the object
	'has any other UI associated with it, such as floating palette windows,
	'that those windows are visible and ready for use.  Any number of objects
	'can be "In-Place Active" at a time, although only one can be 
	'"InPlace UI-Active".  
	
	'You can cause an object to move to either one of states programmatically by 
	'setting the Verb property to the appropriate verb and setting 
	'Action=OLE_ACTIVATE.  
	
	'Also, if you set AutoActivate = OLE_ACTIVATE_GETFOCUS, the server will 
	'automatically be put into "InPlace UI-Active" state when the user clicks
	'on or tabs into the control.
	
	'VerbFlag Bit Masks 
	Public Const VERBFLAG_GRAYED As Integer = &H1s
	Public Const VERBFLAG_DISABLED As Integer = &H2s
	Public Const VERBFLAG_CHECKED As Integer = &H8s
	Public Const VERBFLAG_SEPARATOR As Integer = &H800s
	
	'MiscFlag Bits - Or these together as desired for special behaviors
	
	'MEMSTORAGE causes the control to use memory to store the object while
	'           it is loaded.  This is faster than the default (disk-tempfile),
	'           but can consume a lot of memory for objects whose data takes
	'           up a lot of space, such as the bitmap for a paint program.
	Public Const OLE_MISCFLAG_MEMSTORAGE As Integer = &H1s
	
	'DISABLEINPLACE overrides the control's default behavior of allowing 
	'           in-place activation for objects that support it.  If you
	'           are having problems activating an object inplace, you can
	'           force it to always activate in a separate window by setting this
	'           bit
	Public Const OLE_MISCFLAG_DISABLEINPLACE As Integer = &H2s
	
	'Common Dialog Control
	'Action Property
	Public Const DLG_FILE_OPEN As Integer = 1
	Public Const DLG_FILE_SAVE As Integer = 2
	Public Const DLG_COLOR As Integer = 3
	Public Const DLG_FONT As Integer = 4
	Public Const DLG_PRINT As Integer = 5
	Public Const DLG_HELP As Integer = 6
	
	'File Open/Save Dialog Flags
	Public Const OFN_READONLY As Integer = &H1
	Public Const OFN_OVERWRITEPROMPT As Integer = &H2
	Public Const OFN_HIDEREADONLY As Integer = &H4
	Public Const OFN_NOCHANGEDIR As Integer = &H8
	Public Const OFN_SHOWHELP As Integer = &H10
	Public Const OFN_NOVALIDATE As Integer = &H100
	Public Const OFN_ALLOWMULTISELECT As Integer = &H200
	Public Const OFN_EXTENSIONDIFFERENT As Integer = &H400
	Public Const OFN_PATHMUSTEXIST As Integer = &H800
	Public Const OFN_FILEMUSTEXIST As Integer = &H1000
	Public Const OFN_CREATEPROMPT As Integer = &H2000
	Public Const OFN_SHAREAWARE As Integer = &H4000
	Public Const OFN_NOREADONLYRETURN As Integer = &H8000
	
	'Color Dialog Flags
	Public Const CC_RGBINIT As Integer = &H1
	Public Const CC_FULLOPEN As Integer = &H2
	Public Const CC_PREVENTFULLOPEN As Integer = &H4
	Public Const CC_SHOWHELP As Integer = &H8
	
	'Fonts Dialog Flags
	Public Const CF_SCREENFONTS As Integer = &H1
	Public Const CF_PRINTERFONTS As Integer = &H2
	Public Const CF_BOTH As Integer = &H3
	Public Const CF_SHOWHELP As Integer = &H4
	Public Const CF_INITTOLOGFONTSTRUCT As Integer = &H40
	Public Const CF_USESTYLE As Integer = &H80
	Public Const CF_EFFECTS As Integer = &H100
	Public Const CF_APPLY As Integer = &H200
	Public Const CF_ANSIONLY As Integer = &H400
	Public Const CF_NOVECTORFONTS As Integer = &H800
	Public Const CF_NOSIMULATIONS As Integer = &H1000
	Public Const CF_LIMITSIZE As Integer = &H2000
	Public Const CF_FIXEDPITCHONLY As Integer = &H4000
	Public Const CF_WYSIWYG As Integer = &H8000 'must also have CF_SCREENFONTS & CF_PRINTERFONTS
	Public Const CF_FORCEFONTEXIST As Integer = &H10000
	Public Const CF_SCALABLEONLY As Integer = &H20000
	Public Const CF_TTONLY As Integer = &H40000
	Public Const CF_NOFACESEL As Integer = &H80000
	Public Const CF_NOSTYLESEL As Integer = &H100000
	Public Const CF_NOSIZESEL As Integer = &H200000
	
	'Printer Dialog Flags
	Public Const PD_ALLPAGES As Integer = &H0
	Public Const PD_SELECTION As Integer = &H1
	Public Const PD_PAGENUMS As Integer = &H2
	Public Const PD_NOSELECTION As Integer = &H4
	Public Const PD_NOPAGENUMS As Integer = &H8
	Public Const PD_COLLATE As Integer = &H10
	Public Const PD_PRINTTOFILE As Integer = &H20
	Public Const PD_PRINTSETUP As Integer = &H40
	Public Const PD_NOWARNING As Integer = &H80
	Public Const PD_RETURNDC As Integer = &H100
	Public Const PD_RETURNIC As Integer = &H200
	Public Const PD_RETURNDEFAULT As Integer = &H400
	Public Const PD_SHOWHELP As Integer = &H800
	Public Const PD_USEDEVMODECOPIES As Integer = &H40000
	Public Const PD_DISABLEPRINTTOFILE As Integer = &H80000
	Public Const PD_HIDEPRINTTOFILE As Integer = &H100000
	
	'Help Constants
	Public Const HELP_CONTEXT As Integer = &H1s 'Display topic in ulTopic
	Public Const HELP_QUIT As Integer = &H2s 'Terminate help
	Public Const HELP_INDEX As Integer = &H3s 'Display index
	Public Const HELP_CONTENTS As Integer = &H3s
	Public Const HELP_HELPONHELP As Integer = &H4s 'Display help on using help
	Public Const HELP_SETINDEX As Integer = &H5s 'Set the current Index for multi index help
	Public Const HELP_SETCONTENTS As Integer = &H5s
	Public Const HELP_CONTEXTPOPUP As Integer = &H8s
	Public Const HELP_FORCEFILE As Integer = &H9s
	Public Const HELP_KEY As Integer = &H101s 'Display topic for keyword in offabData
	Public Const HELP_COMMAND As Integer = &H102s
	Public Const HELP_PARTIALKEY As Integer = &H105s 'call the search engine in winhelp
	
	'Error Constants
	Public Const CDERR_DIALOGFAILURE As Integer = -32768
	
	Public Const CDERR_GENERALCODES As Integer = &H7FFFs
	Public Const CDERR_STRUCTSIZE As Integer = &H7FFEs
	Public Const CDERR_INITIALIZATION As Integer = &H7FFDs
	Public Const CDERR_NOTEMPLATE As Integer = &H7FFCs
	Public Const CDERR_NOHINSTANCE As Integer = &H7FFBs
	Public Const CDERR_LOADSTRFAILURE As Integer = &H7FFAs
	Public Const CDERR_FINDRESFAILURE As Integer = &H7FF9s
	Public Const CDERR_LOADRESFAILURE As Integer = &H7FF8s
	Public Const CDERR_LOCKRESFAILURE As Integer = &H7FF7s
	Public Const CDERR_MEMALLOCFAILURE As Integer = &H7FF6s
	Public Const CDERR_MEMLOCKFAILURE As Integer = &H7FF5s
	Public Const CDERR_NOHOOK As Integer = &H7FF4s
	
	'Added for CMDIALOG.VBX
	Public Const CDERR_CANCEL As Integer = &H7FF3s
	Public Const CDERR_NODLL As Integer = &H7FF2s
	Public Const CDERR_ERRPROC As Integer = &H7FF1s
	Public Const CDERR_ALLOC As Integer = &H7FF0s
	Public Const CDERR_HELP As Integer = &H7FEFs
	
	Public Const PDERR_PRINTERCODES As Integer = &H6FFFs
	Public Const PDERR_SETUPFAILURE As Integer = &H6FFEs
	Public Const PDERR_PARSEFAILURE As Integer = &H6FFDs
	Public Const PDERR_RETDEFFAILURE As Integer = &H6FFCs
	Public Const PDERR_LOADDRVFAILURE As Integer = &H6FFBs
	Public Const PDERR_GETDEVMODEFAIL As Integer = &H6FFAs
	Public Const PDERR_INITFAILURE As Integer = &H6FF9s
	Public Const PDERR_NODEVICES As Integer = &H6FF8s
	Public Const PDERR_NODEFAULTPRN As Integer = &H6FF7s
	Public Const PDERR_DNDMMISMATCH As Integer = &H6FF6s
	Public Const PDERR_CREATEICFAILURE As Integer = &H6FF5s
	Public Const PDERR_PRINTERNOTFOUND As Integer = &H6FF4s
	
	Public Const CFERR_CHOOSEFONTCODES As Integer = &H5FFFs
	Public Const CFERR_NOFONTS As Integer = &H5FFEs
	
	Public Const FNERR_FILENAMECODES As Integer = &H4FFFs
	Public Const FNERR_SUBCLASSFAILURE As Integer = &H4FFEs
	Public Const FNERR_INVALIDFILENAME As Integer = &H4FFDs
	Public Const FNERR_BUFFERTOOSMALL As Integer = &H4FFCs
	
	Public Const FRERR_FINDREPLACECODES As Integer = &H3FFFs
	Public Const CCERR_CHOOSECOLORCODES As Integer = &H2FFFs
	
	
	'---------------------------------------------------------
	'      Table of Contents for Visual Basic Professional
	'
	'       1.  3-D Controls
	'           (Frame/Panel/Option/Check/Command/Group Push)
	'       2.  Animated Button
	'       3.  Gauge Control
	'       4.  Graph Control Section
	'       5.  Key Status Control
	'       6.  Spin Button
	'       7.  MCI Control (Multimedia)
	'       8.  Masked Edit Control
	'       9.  Comm Control
	'       10. Outline Control
	'---------------------------------------------------------
	
	
	'-------------------------------------------------------------------
	'3D Controls
	'-------------------------------------------------------------------
	'Alignment (Check Box)
	Public Const SSCB_TEXT_RIGHT As Integer = 0 '0 - Text to the right
	Public Const SSCB_TEXT_LEFT As Integer = 1 '1 - Text to the left
	
	'Alignment (Option Button)
	Public Const SSOB_TEXT_RIGHT As Integer = 0 '0 - Text to the right
	Public Const SSOB_TEXT_LEFT As Integer = 1 '1 - Text to the left
	
	'Alignment (Frame)
	Public Const SSFR_LEFT_JUSTIFY As Integer = 0 '0 - Left justify text
	Public Const SSFR_RIGHT_JUSTIFY As Integer = 1 '1 - Right justify text
	Public Const SSFR_CENTER As Integer = 2 '2 - Center text
	
	'Alignment (Panel)
	Public Const SSPN_LEFT_TOP As Integer = 0 '0 - Text to left and top
	Public Const SSPN_LEFT_MIDDLE As Integer = 1 '1 - Text to left and middle
	Public Const SSPN_LEFT_BOTTOM As Integer = 2 '2 - Text to left and bottom
	Public Const SSPN_RIGHT_TOP As Integer = 3 '3 - Text to right and top
	Public Const SSPN_RIGHT_MIDDLE As Integer = 4 '4 - Text to right and middle
	Public Const SSPN_RIGHT_BOTTOM As Integer = 5 '5 - Text to right and bottom
	Public Const SSPN_CENTER_TOP As Integer = 6 '6 - Text to center and top
	Public Const SSPN_CENTER_MIDDLE As Integer = 7 '7 - Text to center and middle
	Public Const SSPN_CENTER_BOTTOM As Integer = 8 '8 - Text to center and bottom
	
	'Autosize (Command Button)
	Public Const SS_AUTOSIZE_NONE As Integer = 0 '0 - No Autosizing
	Public Const SSPB_AUTOSIZE_PICTOBUT As Integer = 1 '0 - Autosize Picture to Button
	Public Const SSPB_AUTOSIZE_BUTTOPIC As Integer = 2 '0 - Autosize Button to Picture
	
	'Autosize (Ribbon Button)
	'Global Const SS_AUTOSIZE_NONE      = 0  '0 - No Autosizing
	Public Const SSRI_AUTOSIZE_PICTOBUT As Integer = 1 '0 - Autosize Picture to Button
	Public Const SSRI_AUTOSIZE_BUTTOPIC As Integer = 2 '0 - Autosize Button to Picture
	
	'Autosize (Panel)
	'Global Const SS_AUTOSIZE_NONE    = 0    '0 - No Autosizing
	Public Const SSPN_AUTOSIZE_WIDTH As Integer = 1 '1 - Autosize Panel width to Caption
	Public Const SSPN_AUTOSIZE_HEIGHT As Integer = 2 '2 - Autosize Panel height to Caption
	Public Const SSPN_AUTOSIZE_CHILD As Integer = 3 '3 - Autosize Child to Panel
	
	'BevelInner (Panel)
	Public Const SS_BEVELINNER_NONE As Integer = 0 '0 - No Inner Bevel
	Public Const SS_BEVELINNER_INSET As Integer = 1 '1 - Inset Inner Bevel
	Public Const SS_BEVELINNER_RAISED As Integer = 2 '2 - Raised Inner Bevel
	
	'BevelOuter (Panel)
	Public Const SS_BEVELOUTER_NONE As Integer = 0 '0 - No Outer Bevel
	Public Const SS_BEVELOUTER_INSET As Integer = 1 '1 - Inset Outer Bevel
	Public Const SS_BEVELOUTER_RAISED As Integer = 2 '2 - Raised Outer Bevel
	
	'FloodType (Panel)
	Public Const SS_FLOODTYPE_NONE As Integer = 0 '0 - No flood
	Public Const SS_FLOODTYPE_L_TO_R As Integer = 1 '1 - Left to light
	Public Const SS_FLOODTYPE_R_TO_L As Integer = 2 '2 - Right to left
	Public Const SS_FLOODTYPE_T_TO_B As Integer = 3 '3 - Top to bottom
	Public Const SS_FLOODTYPE_B_TO_T As Integer = 4 '4 - Bottom to top
	Public Const SS_FLOODTYPE_CIRCLE As Integer = 5 '5 - Widening circle
	
	'Font3D (Panel, Command Button, Option Button, Check Box, Frame)
	Public Const SS_FONT3D_NONE As Integer = 0 '0 - No 3-D text
	Public Const SS_FONT3D_RAISED_LIGHT As Integer = 1 '1 - Raised with light shading
	Public Const SS_FONT3D_RAISED_HEAVY As Integer = 2 '2 - Raised with heavy shading
	Public Const SS_FONT3D_INSET_LIGHT As Integer = 3 '3 - Inset with light shading
	Public Const SS_FONT3D_INSET_HEAVY As Integer = 4 '4 - Inset with heavy shading
	
	'PictureDnChange (Ribbon Button)
	Public Const SS_PICDN_NOCHANGE As Integer = 0 '0 - Use 'Up'bitmap with no change
	Public Const SS_PICDN_DITHER As Integer = 1 '1 - Dither 'Up'bitmap
	Public Const SS_PICDN_INVERT As Integer = 2 '2 - Invert 'Up'bitmap
	
	'ShadowColor (Panel, Frame)
	Public Const SS_SHADOW_DARKGREY As Integer = 0 '0 - Dark grey shadow
	Public Const SS_SHADOW_BLACK As Integer = 1 '1 - Black shadow
	
	'ShadowStyle (Frame)
	Public Const SS_SHADOW_INSET As Integer = 0 '0 - Shadow inset
	Public Const SS_SHADOW_RAISED As Integer = 1 '1 - Shadow raised
	
	
	'---------------------------------------
	'Animated Button
	'---------------------------------------
	'Cycle property
	Public Const ANI_ANIMATED As Integer = 0
	Public Const ANI_MULTISTATE As Integer = 1
	Public Const ANI_TWO_STATE As Integer = 2
	
	'Click Filter property
	Public Const ANI_ANYWHERE As Integer = 0
	Public Const ANI_IMAGE_AND_TEXT As Integer = 1
	Public Const ANI_IMAGE As Integer = 2
	Public Const ANI_TEXT As Integer = 3
	
	'PicDrawMode Property
	Public Const ANI_XPOS_YPOS As Integer = 0
	Public Const ANI_AUTOSIZE As Integer = 1
	Public Const ANI_STRETCH As Integer = 2
	
	'SpecialOp Property
	Public Const ANI_CLICK As Integer = 1
	
	'TextPosition Property
	Public Const ANI_CENTER As Integer = 0
	Public Const ANI_LEFT As Integer = 1
	Public Const ANI_RIGHT As Integer = 2
	Public Const ANI_BOTTON As Integer = 3
	Public Const ANI_TOP As Integer = 4
	
	
	'---------------------------------------
	'GAUGE
	'---------------------------------------
	'Style Property
	Public Const GAUGE_HORIZ As Integer = 0
	Public Const GAUGE_VERT As Integer = 1
	Public Const GAUGE_SEMI As Integer = 2
	Public Const GAUGE_FULL As Integer = 3
	
	
	'----------------------------------------
	'Graph Control
	'----------------------------------------
	'General
	Public Const G_NONE As Integer = 0
	Public Const G_DEFAULT As Integer = 0
	
	Public Const G_OFF As Integer = 0
	Public Const G_ON As Integer = 1
	
	Public Const G_MONO As Integer = 0
	Public Const G_COLOR As Integer = 1
	
	'Graph Types
	Public Const G_PIE2D As Integer = 1
	Public Const G_PIE3D As Integer = 2
	Public Const G_BAR2D As Integer = 3
	Public Const G_BAR3D As Integer = 4
	Public Const G_GANTT As Integer = 5
	Public Const G_LINE As Integer = 6
	Public Const G_LOGLIN As Integer = 7
	Public Const G_AREA As Integer = 8
	Public Const G_SCATTER As Integer = 9
	Public Const G_POLAR As Integer = 10
	Public Const G_HLC As Integer = 11
	
	'Colors
	Public Const G_BLACK As Integer = 0
	Public Const G_BLUE As Integer = 1
	Public Const G_GREEN As Integer = 2
	Public Const G_CYAN As Integer = 3
	Public Const G_RED As Integer = 4
	Public Const G_MAGENTA As Integer = 5
	Public Const G_BROWN As Integer = 6
	Public Const G_LIGHT_GRAY As Integer = 7
	Public Const G_DARK_GRAY As Integer = 8
	Public Const G_LIGHT_BLUE As Integer = 9
	Public Const G_LIGHT_GREEN As Integer = 10
	Public Const G_LIGHT_CYAN As Integer = 11
	Public Const G_LIGHT_RED As Integer = 12
	Public Const G_LIGHT_MAGENTA As Integer = 13
	Public Const G_YELLOW As Integer = 14
	Public Const G_WHITE As Integer = 15
	Public Const G_AUTOBW As Integer = 16
	
	'Patterns
	Public Const G_SOLID As Integer = 0
	Public Const G_HOLLOW As Integer = 1
	Public Const G_HATCH1 As Integer = 2
	Public Const G_HATCH2 As Integer = 3
	Public Const G_HATCH3 As Integer = 4
	Public Const G_HATCH4 As Integer = 5
	Public Const G_HATCH5 As Integer = 6
	Public Const G_HATCH6 As Integer = 7
	Public Const G_BITMAP1 As Integer = 16
	Public Const G_BITMAP2 As Integer = 17
	Public Const G_BITMAP3 As Integer = 18
	Public Const G_BITMAP4 As Integer = 19
	Public Const G_BITMAP5 As Integer = 20
	Public Const G_BITMAP6 As Integer = 21
	Public Const G_BITMAP7 As Integer = 22
	Public Const G_BITMAP8 As Integer = 23
	Public Const G_BITMAP9 As Integer = 24
	Public Const G_BITMAP10 As Integer = 25
	Public Const G_BITMAP11 As Integer = 26
	Public Const G_BITMAP12 As Integer = 27
	Public Const G_BITMAP13 As Integer = 28
	Public Const G_BITMAP14 As Integer = 29
	Public Const G_BITMAP15 As Integer = 30
	Public Const G_BITMAP16 As Integer = 31
	
	'Symbols
	Public Const G_CROSS_PLUS As Integer = 0
	Public Const G_CROSS_TIMES As Integer = 1
	Public Const G_TRIANGLE_UP As Integer = 2
	Public Const G_SOLID_TRIANGLE_UP As Integer = 3
	Public Const G_TRIANGLE_DOWN As Integer = 4
	Public Const G_SOLID_TRIANGLE_DOWN As Integer = 5
	Public Const G_SQUARE As Integer = 6
	Public Const G_SOLID_SQUARE As Integer = 7
	Public Const G_DIAMOND As Integer = 8
	Public Const G_SOLID_DIAMOND As Integer = 9
	
	'Line Styles
	'Global Const G_SOLID = 0
	Public Const G_DASH As Integer = 1
	Public Const G_DOT As Integer = 2
	Public Const G_DASHDOT As Integer = 3
	Public Const G_DASHDOTDOT As Integer = 4
	
	'Grids
	Public Const G_HORIZONTAL As Integer = 1
	Public Const G_VERTICAL As Integer = 2
	
	'Statistics
	Public Const G_MEAN As Integer = 1
	Public Const G_MIN_MAX As Integer = 2
	Public Const G_STD_DEV As Integer = 4
	Public Const G_BEST_FIT As Integer = 8
	
	'Data Arrays
	Public Const G_GRAPH_DATA As Integer = 1
	Public Const G_COLOR_DATA As Integer = 2
	Public Const G_EXTRA_DATA As Integer = 3
	Public Const G_LABEL_TEXT As Integer = 4
	Public Const G_LEGEND_TEXT As Integer = 5
	Public Const G_PATTERN_DATA As Integer = 6
	Public Const G_SYMBOL_DATA As Integer = 7
	Public Const G_XPOS_DATA As Integer = 8
	Public Const G_ALL_DATA As Integer = 9
	
	'Draw Mode
	Public Const G_NO_ACTION As Integer = 0
	Public Const G_CLEAR As Integer = 1
	Public Const G_DRAW As Integer = 2
	Public Const G_BLIT As Integer = 3
	Public Const G_COPY As Integer = 4
	Public Const G_PRINT As Integer = 5
	Public Const G_WRITE As Integer = 6
	
	'Print Options
	Public Const G_BORDER As Integer = 2
	
	'Pie Chart Options             '
	Public Const G_NO_LINES As Integer = 1
	Public Const G_COLORED As Integer = 2
	Public Const G_PERCENTS As Integer = 4
	
	'Bar Chart Options             '
	'Global Const G_HORIZONTAL = 1
	Public Const G_STACKED As Integer = 2
	Public Const G_PERCENTAGE As Integer = 4
	Public Const G_Z_CLUSTERED As Integer = 6
	
	'Gantt Chart Options           '
	Public Const G_SPACED_BARS As Integer = 1
	
	'Line/Polar Chart Options      '
	Public Const G_SYMBOLS As Integer = 1
	Public Const G_STICKS As Integer = 2
	Public Const G_LINES As Integer = 4
	
	'Area Chart Options            '
	Public Const G_ABSOLUTE As Integer = 1
	Public Const G_PERCENT As Integer = 2
	
	'HLC Chart Options             '
	Public Const G_NO_CLOSE As Integer = 1
	Public Const G_NO_HIGH_LOW As Integer = 2
	
	
	'---------------------------------------
	'Key Status Control
	'---------------------------------------
	'Style
	Public Const KEYSTAT_CAPSLOCK As Integer = 0
	Public Const KEYSTAT_NUMLOCK As Integer = 1
	Public Const KEYSTAT_INSERT As Integer = 2
	Public Const KEYSTAT_SCROLLLOCK As Integer = 3
	
	
	'---------------------------------------
	'MCI Control (Multimedia)
	'---------------------------------------
	'NOTE:
	'Please use the updated Multimedia constants
	'in the WINMMSYS.TXT file from the \VB\WINAPI
	'subdirectory.
	
	'Mode Property
	'Global Const MCI_MODE_NOT_OPEN = 11
	'Global Const MCI_MODE_STOP = 12
	'Global Const MCI_MODE_PLAY = 13
	'Global Const MCI_MODE_RECORD = 14
	'Global Const MCI_MODE_SEEK = 15
	'Global Const MCI_MODE_PAUSE = 16
	'Global Const MCI_MODE_READY = 17
	
	'NotifyValue Property
	'Global Const MCI_NOTIFY_SUCCESSFUL = 1
	'Global Const MCI_NOTIFY_SUPERSEDED = 2
	'Global Const MCI_ABORTED = 4
	'Global Const MCI_FAILURE = 8
	
	'Orientation Property
	'Global Const MCI_ORIENT_HORZ = 0
	'Global Const MCI_ORIENT_VERT = 1
	
	'RecordMode Porperty
	'Global Const MCI_RECORD_INSERT = 0
	'Global Const MCI_RECORD_OVERWRITE = 1
	
	'TimeFormat Property
	'Global Const MCI_FORMAT_MILLISECONDS = 0
	'Global Const MCI_FORMAT_HMS = 1
	'Global Const MCI_FORMAT_MSF = 2
	'Global Const MCI_FORMAT_FRAMES = 3
	'Global Const MCI_FORMAT_SMPTE_24 = 4
	'Global Const MCI_FORMAT_SMPTE_25 = 5
	'Global Const MCI_FORMAT_SMPTE_30 = 6
	'Global Const MCI_FORMAT_SMPTE_30DROP = 7
	'Global Const MCI_FORMAT_BYTES = 8
	'Global Const MCI_FORMAT_SAMPLES = 9
	'Global Const MCI_FORMAT_TMSF = 10
	
	
	'---------------------------------------
	'Spin Button
	'---------------------------------------
	'SpinOrientation
	Public Const SPIN_VERTICAL As Integer = 0
	Public Const SPIN_HORIZONTAL As Integer = 1
	
	
	'---------------------------------------
	'Masked Edit Control
	'---------------------------------------
	'ClipMode
	Public Const ME_INCLIT As Integer = 0
	Public Const ME_EXCLIT As Integer = 1
	
	
	'---------------------------------------
	'Comm Control
	'---------------------------------------
	'Handshaking
	Public Const MSCOMM_HANDSHAKE_NONE As Integer = 0
	Public Const MSCOMM_HANDSHAKE_XONXOFF As Integer = 1
	Public Const MSCOMM_HANDSHAKE_RTS As Integer = 2
	Public Const MSCOMM_HANDSHAKE_RTSXONXOFF As Integer = 3
	
	'Event constants
	Public Const MSCOMM_EV_SEND As Integer = 1
	Public Const MSCOMM_EV_RECEIVE As Integer = 2
	Public Const MSCOMM_EV_CTS As Integer = 3
	Public Const MSCOMM_EV_DSR As Integer = 4
	Public Const MSCOMM_EV_CD As Integer = 5
	Public Const MSCOMM_EV_RING As Integer = 6
	Public Const MSCOMM_EV_EOF As Integer = 7
	
	'Error code constants
	Public Const MSCOMM_ER_BREAK As Integer = 1001
	Public Const MSCOMM_ER_CTSTO As Integer = 1002
	Public Const MSCOMM_ER_DSRTO As Integer = 1003
	Public Const MSCOMM_ER_FRAME As Integer = 1004
	Public Const MSCOMM_ER_OVERRUN As Integer = 1006
	Public Const MSCOMM_ER_CDTO As Integer = 1007
	Public Const MSCOMM_ER_RXOVER As Integer = 1008
	Public Const MSCOMM_ER_RXPARITY As Integer = 1009
	Public Const MSCOMM_ER_TXFULL As Integer = 1010
	
	
	'---------------------------------------
	' MAPI SESSION CONTROL CONSTANTS
	'---------------------------------------
	'Action
	Public Const SESSION_SIGNON As Integer = 1
	Public Const SESSION_SIGNOFF As Integer = 2
	
	
	'---------------------------------------
	' MAPI MESSAGE CONTROL CONSTANTS
	'---------------------------------------
	'Action
	Public Const MESSAGE_FETCH As Integer = 1 ' Load all messages from message store
	Public Const MESSAGE_SENDDLG As Integer = 2 ' Send mail bring up default mapi dialog
	Public Const MESSAGE_SEND As Integer = 3 ' Send mail without default mapi dialog
	Public Const MESSAGE_SAVEMSG As Integer = 4 ' Save message in the compose buffer
	Public Const MESSAGE_COPY As Integer = 5 ' Copy current message to compose buffer
	Public Const MESSAGE_COMPOSE As Integer = 6 ' Initialize compose buffer (previous
	' data is lost
	Public Const MESSAGE_REPLY As Integer = 7 ' Fill Compose buffer as REPLY
	Public Const MESSAGE_REPLYALL As Integer = 8 ' Fill Compose buffer as REPLY ALL
	Public Const MESSAGE_FORWARD As Integer = 9 ' Fill Compose buffer as FORWARD
	Public Const MESSAGE_DELETE As Integer = 10 ' Delete current message
	Public Const MESSAGE_SHOWADBOOK As Integer = 11 ' Show Address book
	Public Const MESSAGE_SHOWDETAILS As Integer = 12 ' Show details of the current recipient
	Public Const MESSAGE_RESOLVENAME As Integer = 13 ' Resolve the display name of the recipient
	Public Const RECIPIENT_DELETE As Integer = 14 ' Fill Compose buffer as FORWARD
	Public Const ATTACHMENT_DELETE As Integer = 15 ' Delete current message
	
	
	'---------------------------------------
	'  ERROR CONSTANT DECLARATIONS (MAPI CONTROLS)
	'---------------------------------------
	Public Const SUCCESS_SUCCESS As Integer = 32000
	Public Const MAPI_USER_ABORT As Integer = 32001
	Public Const MAPI_E_FAILURE As Integer = 32002
	Public Const MAPI_E_LOGIN_FAILURE As Integer = 32003
	Public Const MAPI_E_DISK_FULL As Integer = 32004
	Public Const MAPI_E_INSUFFICIENT_MEMORY As Integer = 32005
	Public Const MAPI_E_ACCESS_DENIED As Integer = 32006
	Public Const MAPI_E_TOO_MANY_SESSIONS As Integer = 32008
	Public Const MAPI_E_TOO_MANY_FILES As Integer = 32009
	Public Const MAPI_E_TOO_MANY_RECIPIENTS As Integer = 32010
	Public Const MAPI_E_ATTACHMENT_NOT_FOUND As Integer = 32011
	Public Const MAPI_E_ATTACHMENT_OPEN_FAILURE As Integer = 32012
	Public Const MAPI_E_ATTACHMENT_WRITE_FAILURE As Integer = 32013
	Public Const MAPI_E_UNKNOWN_RECIPIENT As Integer = 32014
	Public Const MAPI_E_BAD_RECIPTYPE As Integer = 32015
	Public Const MAPI_E_NO_MESSAGES As Integer = 32016
	Public Const MAPI_E_INVALID_MESSAGE As Integer = 32017
	Public Const MAPI_E_TEXT_TOO_LARGE As Integer = 32018
	Public Const MAPI_E_INVALID_SESSION As Integer = 32019
	Public Const MAPI_E_TYPE_NOT_SUPPORTED As Integer = 32020
	Public Const MAPI_E_AMBIGUOUS_RECIPIENT As Integer = 32021
	Public Const MAPI_E_MESSAGE_IN_USE As Integer = 32022
	Public Const MAPI_E_NETWORK_FAILURE As Integer = 32023
	Public Const MAPI_E_INVALID_EDITFIELDS As Integer = 32024
	Public Const MAPI_E_INVALID_RECIPS As Integer = 32025
	Public Const MAPI_E_NOT_SUPPORTED As Integer = 32026
	
	Public Const CONTROL_E_SESSION_EXISTS As Integer = 32050
	Public Const CONTROL_E_INVALID_BUFFER As Integer = 32051
	Public Const CONTROL_E_INVALID_READ_BUFFER_ACTION As Integer = 32052
	Public Const CONTROL_E_NO_SESSION As Integer = 32053
	Public Const CONTROL_E_INVALID_RECIPIENT As Integer = 32054
	Public Const CONTROL_E_INVALID_COMPOSE_BUFFER_ACTION As Integer = 32055
	Public Const CONTROL_E_FAILURE As Integer = 32056
	Public Const CONTROL_E_NO_RECIPIENTS As Integer = 32057
	Public Const CONTROL_E_NO_ATTACHMENTS As Integer = 32058
	
	
	'---------------------------------------
	'  MISCELLANEOUS GLOBAL CONSTANT DECLARATIONS (MAPI CONTROLS)
	'---------------------------------------
	Public Const RECIPTYPE_ORIG As Integer = 0
	Public Const RECIPTYPE_TO As Integer = 1
	Public Const RECIPTYPE_CC As Integer = 2
	Public Const RECIPTYPE_BCC As Integer = 3
	
	Public Const ATTACHTYPE_DATA As Integer = 0
	Public Const ATTACHTYPE_EOLE As Integer = 1
	Public Const ATTACHTYPE_SOLE As Integer = 2
	
	
	'-------------------------------------------------
	'  Outline
	'-------------------------------------------------
	' PictureType
	Public Const MSOUTLINE_PICTURE_CLOSED As Integer = 0
	Public Const MSOUTLINE_PICTURE_OPEN As Integer = 1
	Public Const MSOUTLINE_PICTURE_LEAF As Integer = 2
	
	'Outline Control Error Constants
	Public Const MSOUTLINE_BADPICFORMAT As Integer = 32000
	Public Const MSOUTLINE_BADINDENTATION As Integer = 32001
	Public Const MSOUTLINE_MEM As Integer = 32002
	Public Const MSOUTLINE_PARENTNOTEXPANDED As Integer = 32003
End Module