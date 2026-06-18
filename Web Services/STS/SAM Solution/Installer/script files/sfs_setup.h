// your script function prototypes
prototype	SFS_OnBegin();
prototype	SFS_OnGeneratingMSIScript();
prototype	SFS_PostFileInstall();

// ----------- SECTION BREAK ----------- //

prototype	SFS_Dummy();
prototype	SFS_ClientOrServer( STRING );
prototype	SFS_ProductUpdateHistory();
prototype	SFS_InstallDatabase( STRING, STRING, BOOL ) ;
prototype	SFS_MessageBox ( STRING, NUMBER ) ;
prototype	SFS_DlgInstallationType();
prototype	SFS_DlgSelectProducts();
prototype	SFS_CheckPermissions();
prototype	SFS_GetInstallDir(); 	// was GetPMDir
prototype	SFS_RegisterSAClient();
prototype	SFS_DlgPMSetupShare( BOOL );
prototype	SFS_ClientInstall ( STRING, STRING, STRING, STRING );
prototype	SFS_Trim ( BYREF STRING ); // Was PMTrim
prototype	SFS_UsersLoggedOn();
prototype	SFS_RemoveUnwantedFiles( STRING );
prototype	SFS_NetShareAdd( STRING, STRING );
prototype	SFS_MsiGetProperty ( NUMBER, STRING, BYREF STRING, LONG );
prototype	SFS_GetAllProperties();
prototype	SFS_GetProperty ( STRING, STRING );
prototype	SFS_Replacements( BYREF STRING );
prototype	SFS_SetVersions();
prototype	SFS_LaunchAppAndWait( STRING, STRING, NUMBER);
prototype	SFS_XCopyFile ( STRING, STRING, NUMBER);
prototype	SFS_InstallDocumentType();
prototype	SFS_SelectSetupType();
prototype 	SFS_StatusWindowType( STRING ); 
prototype	SFS_PreReqs_GetInstalledVersion();
prototype	SFS_PreReqs_GetCDVersion();
prototype	SFS_PreReqs_Install();  
prototype	SFS_SplitVersions ( STRING, BYREF STRING, BYREF STRING, BYREF STRING );

/* Global MSI Variables /////////////////////////////////////////////////////

	PRODUCT_VERSION
	PMDIR

////////////////////////////////////////////////////////////////////////// */

// your global variables
BOOL	g_bClient, g_bServer;
NUMBER	nvSize; 

LIST	g_listStartCopy;

STRING	g_sProduct_Version, 
		g_sPMDIR,
		g_sPMSetup, g_sComputerName;
// #-=-=-=-=-=-=-=-=-=-=-# REQUIRED FOR NetShareAdd #-=-=-=-=-=-=-=-=-=-=-# //


#ifndef __NETOPS_H__
#define __NETOPS_H__

// Drive Mapping definitions
#define RESOURCETYPE_ANY 			0x00000000
#define WN_NO_NETWORK 				1222
#define WN_SUCCESS 					0
#define WN_ACCESS_DENIED 			5
#define ERROR_BAD_DEV_TYPE 			66
#define WN_BAD_PROVIDER 			1204
#define WN_NO_NET_OR_BAD_PATH 		1203
#define WN_BAD_NETNAME 				67
#define WN_BAD_LOCALNAME 			1200
#define WN_BAD_PASSWORD 			86
#define WN_ALREADY_CONNECTED 		85
#define WN_CANNOT_OPEN_PROFILE 		1205
#define WN_BAD_PROFILE 				1206
#define WN_FUNCTION_BUSY 			170
#define WN_EXTENDED_ERROR 			1208
#define WN_BAD_USER 				2202
#define ERROR_BAD_NETPATH 			53

// Function prototypes	
external prototype		MapNetworkDrive(string, string);
external prototype		UNCFromMappedDrive(string, byref string);
external prototype		LoadListWithDriveAndUNCInfo(LIST);
external prototype BOOL	IsUNCMapped(string, byref string);

// DLL function prototypes
prototype Mpr.WNetAddConnection2A(pointer, pointer, pointer, long);
prototype Mpr.WNetGetConnectionA(byref string, byref string, byref long);
prototype int KERNEL.GetLastError();

typedef NETRESOURCE
begin
	LONG	dwScope, dwType, dwDisplayType, dwUsage;
	POINTER lpLocalName, lpRemoteName, lpComment, lpProvider;
end;

NETRESOURCE netRes;

// NetShare

#define CP_ACP							0		   // default to ANSI code page
#define CP_OEMCP						1		   // default to OEM  code page
#define CP_MACCP						2		   // default to MAC  code page
#define CP_THREAD_ACP					3		   // current thread's ANSI code page
#define CP_SYMBOL						42		  // SYMBOL translations

#define CP_UTF7							65000	   // UTF-7 translation
#define CP_UTF8							65001	   // UTF-8 translation

#define MB_PRECOMPOSED					0x00000001  // use precomposed chars
#define MB_COMPOSITE					0x00000002  // use composite chars
#define MB_USEGLYPHCHARS				0x00000004  // use glyph chars, not ctrl chars
#define MB_ERR_INVALID_CHARS			0x00000008  // error for invalid chars

#define STYPE_DISKTREE					0
#define STYPE_PRINTQ					1
#define STYPE_DEVICE					2
#define STYPE_IPC						3

#define SHARE_NETNAME_PARMNUM			1
#define SHARE_TYPE_PARMNUM				3
#define SHARE_REMARK_PARMNUM			4
#define SHARE_PERMISSIONS_PARMNUM		5
#define SHARE_MAX_USES_PARMNUM			6
#define SHARE_CURRENT_USES_PARMNUM		7
#define SHARE_PATH_PARMNUM				8
#define SHARE_PASSWD_PARMNUM			9
#define SHARE_FILE_SD_PARMNUM			501

#define STYPE_SPECIAL					2147483648

#define ACCESS_READ						0x01
#define ACCESS_WRITE					2
#define ACCESS_CREATE					4
#define ACCESS_EXEC						8
#define ACCESS_DELETE					16
#define ACCESS_ATRIB					32
#define ACCESS_PERM						64

#define ACCESS_GROUP					32768

#define ACCESS_ALL						(ACCESS_READ+ACCESS_WRITE+ACCESS_CREATE+ACCESS_EXEC+ACCESS_DELETE+ACCESS_ATRIB+ACCESS_PERM)

#define SHI_USES_UNLIMITED				-1

//
// Flags values for the 501, 1005, and 1007 infolevels
//

#define SHI1005_FLAGS_DFS				1	// Share is in the DFS
#define SHI1005_FLAGS_DFS_ROOT			2	// Share is root of DFS

#define COW_PERMACHINE					4	// Share data is per-machine data
#define COW_PERUSER						8	// Share data is per-user data

#define CSC_CACHEABLE					16   // Client can cache files for off-line access
#define CSC_NOFLOWOPS					32   // Client need not flow operations to the server
#define CSC_AUTO_INWARD					64   // Auto inward propagation (server->client) w/o UI
#define CSC_AUTO_OUTWARD				128  // Auto outward propagation(client->server) w/o UI

prototype LONG netapi32.NetShareAdd(POINTER, LONG, POINTER, POINTER); 
prototype LONG kernel32.MultiByteToWideChar(LONG, LONG, POINTER, LONG, POINTER, LONG);

typedef _SHARE_INFO_2
begin
	LPSTR			shi2_netname;
	number			shi2_type;
	LPSTR			shi2_remark;
	number			shi2_permissions;
	number			shi2_max_uses;
	number			shi2_current_uses;
	LPSTR			shi2_path;
	LPSTR			shi2_passwd;
end;

typedef _WIDESTRING
begin
	STRING			Wide[400];
end;

// Function prototypes
external prototype AddShareNet(STRING, STRING);

#endif
