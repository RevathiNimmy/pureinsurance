// Avoid multiple include collisions.
#ifndef _BRWSDLG_H_
#define _BRWSDLG_H_

// Options for Flags member of OPENFILENAME.
#define OFN_READONLY                 0x00000001
#define OFN_OVERWRITEPROMPT          0x00000002
#define OFN_HIDEREADONLY             0x00000004
#define OFN_NOCHANGEDIR              0x00000008
#define OFN_SHOWHELP                 0x00000010
#define OFN_ENABLEHOOK               0x00000020
#define OFN_ENABLETEMPLATE           0x00000040
#define OFN_ENABLETEMPLATEHANDLE     0x00000080
#define OFN_NOVALIDATE               0x00000100
#define OFN_ALLOWMULTISELECT         0x00000200
#define OFN_EXTENSIONDIFFERENT       0x00000400
#define OFN_PATHMUSTEXIST            0x00000800
#define OFN_FILEMUSTEXIST            0x00001000
#define OFN_CREATEPROMPT             0x00002000
#define OFN_SHAREAWARE               0x00004000
#define OFN_NOREADONLYRETURN         0x00008000
#define OFN_NOTESTFILECREATE         0x00010000
#define OFN_NONETWORKBUTTON          0x00020000
#define OFN_NOLONGNAMES              0x00040000
#define OFN_EXPLORER                 0x00080000
#define OFN_NODEREFERENCELINKS       0x00100000
#define OFN_LONGNAMES                0x00200000

// CommDlgExtendedError-related defines.
#define CDERR_DIALOGFAILURE 0xFFFF
#define CDERR_DIALOGFAILURE_MSG "The file browse dialog box could not be created."
   // The dialog box could not be created. The common dialog box function
   // call to the DialogBox function failed. For example, this error occurs
   // if the common dialog box call specifies an invalid window handle."
#define CDERR_FINDRESFAILURE 0x0006
#define CDERR_FINDRESFAILURE_MSG "The file browse dialog box function failed to find a specified resource."
   // The common dialog box function failed to find a specified resource.
#define CDERR_INITIALIZATION 0x0002
#define CDERR_INITIALIZATION_MSG "The file browse dialog box function failed during initialization."
   // The common dialog box function failed during initialization. This error
   // often occurs when sufficient memory is not available.
#define CDERR_LOADRESFAILURE 0x0007
#define CDERR_LOADRESFAILURE_MSG "The file browse dialog box function failed to load a specified resource."
   // The common dialog box function failed to load a specified resource.
#define CDERR_LOADSTRFAILURE 0x0005
#define CDERR_LOADSTRFAILURE_MSG "The file browse dialog box function failed to load a specified string."
   // The common dialog box function failed to load a specified string.
#define CDERR_LOCKRESFAILURE 0x0008
#define CDERR_LOCKRESFAILURE_MSG "The file browse dialog box function failed to lock a specified resource."
   // The common dialog box function failed to lock a specified resource.
#define CDERR_MEMALLOCFAILURE 0x0009
#define CDERR_MEMALLOCFAILURE_MSG "The common dialog box function was unable to allocate memory for internal structures."
   // The common dialog box function was unable to allocate memory
   // for internal structures.
#define CDERR_MEMLOCKFAILURE 0x000A
#define CDERR_MEMLOCKFAILURE_MSG "The common dialog box function was unable to lock the memory associated with a handle."
   // The common dialog box function was unable to lock the memory associated
   // with a handle.
#define CDERR_NOHINSTANCE 0x0004
#define CDERR_NOHINSTANCE_MSG "The ENABLETEMPLATE flag was set, but you failed to provide a corresponding instance handle."
   // The ENABLETEMPLATE flag was set in the Flags member of the initialization
   // structure for the corresponding common dialog box, but you failed to provide
   // a corresponding instance handle.
#define CDERR_NOHOOK 0x000B
#define CDERR_NOHOOK_MSG "The ENABLEHOOK flag was set, but you failed to provide a pointer to a corresponding hook procedure."
   // The ENABLEHOOK flag was set in the Flags member of the initialization
   // structure for the corresponding common dialog box, but you failed to provide
   // a pointer to a corresponding hook procedure.
#define CDERR_NOTEMPLATE 0x0003
#define CDERR_NOTEMPLATE_MSG "The ENABLETEMPLATE flag was set, but you failed to provide a corresponding template."
   // The ENABLETEMPLATE flag was set in the Flags member of the initialization
   // structure for the corresponding common dialog box, but you failed to provide
   // a corresponding template.
#define CDERR_REGISTERMSGFAIL 0x000C
#define CDERR_REGISTERMSGFAIL_MSG "The RegisterWindowMessage function returned an error code when it was called by the file browse dialog box function. "
   // The RegisterWindowMessage function returned an error code when it was called
   // by the common dialog box function.
#define CDERR_STRUCTSIZE 0x0001
#define CDERR_STRUCTSIZE_MSG "The lStructSize member of the initialization structure for the corresponding file browse dialog box is invalid. "
   // The lStructSize member of the initialization structure for the corresponding
   // common dialog box is invalid.
#define FNERR_BUFFERTOOSMALL 0x3003
#define FNERR_BUFFERTOOSMALL_MSG "The buffer pointed to by the lpstrFile member of the OPENFILENAME structure is too small for the filename specified by the user."
   // The buffer pointed to by the lpstrFile member of the OPENFILENAME structure
   // is too small for the filename specified by the user. The first  two bytes
   // of the lpstrFile buffer contain an  integer value specifying the size, in bytes
   // (ANSI version) or 16-bit characters (Unicode  version), required to receive the full name.
#define FNERR_INVALIDFILENAME 0x3002
#define FNERR_INVALIDFILENAME_MSG "A filename is invalid."
   // A filename is invalid.
#define FNERR_SUBCLASSFAILURE 0x3001
#define FNERR_SUBCLASSFAILURE_MSG "An attempt to subclass a list box failed because sufficient memory was not available."
   // An attempt to subclass a list box failed because sufficient memory was not available.


// OPENFILENAME structure. Notice that all string members below are declared
// as LONG. For example, lpstrFilter. Do not use STRING.
typedef OPENFILENAME
	begin
		LONG     lStructSize;
		HWND     hwndOwner;
		HWND     hInstance;
		POINTER  lpstrFilter;
		POINTER  lpstrCustomFilter;
		LONG     nMaxCustFilter;
		LONG     nFilterIndex;
		POINTER  lpstrFile;
		LONG     nMaxFile;
		POINTER  lpstrFileTitle;
		LONG     nMaxFileTitle;
		POINTER  lpstrInitialDir;
		POINTER  lpstrTitle;
		LONG     Flags;
		SHORT    nFileOffset;
		SHORT    nFileExtension;
		POINTER  lpstrDefExt;
		POINTER  lCustData;
		POINTER  lpfnHook;
		POINTER  lpTemplateName;
	end;
		
// Windows API declares.
prototype comdlg32.GetOpenFileNameA( LONG );
prototype comdlg32.CommDlgExtendedError();
prototype user32.wsprintf(BYREF STRING, BYREF STRING, POINTER);

// Our file browse API, defined in matching .rul file.
prototype FileBrowseDlg( BYREF STRING, STRING, STRING, STRING, BOOL, LIST , BOOL);

#endif




