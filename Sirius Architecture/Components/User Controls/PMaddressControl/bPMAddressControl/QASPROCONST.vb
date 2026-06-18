Option Strict Off
Option Explicit On
Module QASPROCONST
    'Attribute VB_Name = "QASProConst"
    '**** Visual Basic (32 bit) header file for QAUPIED.DLL v6.10(49) ****

    Public Const qaproerr_FATAL As Integer = -1000
    Public Const qaproerr_NOMEMORY As Integer = -1001
    Public Const qaproerr_INITINSTANCE As Integer = -1002
    Public Const qaproerr_BADINTERFACE As Integer = -1003
    Public Const qaproerr_INITOOLARGE As Integer = -1005
    Public Const qaproerr_ININOEXTEND As Integer = -1006
    Public Const qaproerr_FILETOOLARGE As Integer = -1008
    Public Const qaproerr_FILECHGDETECT As Integer = -1009
    Public Const qaproerr_FILEOPEN As Integer = -1010
    Public Const qaproerr_FILEEXIST As Integer = -1011
    Public Const qaproerr_FILEREAD As Integer = -1012
    Public Const qaproerr_FILEWRITE As Integer = -1013
    Public Const qaproerr_FILEDELETE As Integer = -1014
    Public Const qaproerr_FILEACCESS As Integer = -1016
    Public Const qaproerr_FILEVERSION As Integer = -1017
    Public Const qaproerr_FILEHANDLE As Integer = -1018
    Public Const qaproerr_FILECREATE As Integer = -1019
    Public Const qaproerr_FILERENAME As Integer = -1020
    Public Const qaproerr_FILEEXPIRED As Integer = -1021
    Public Const qaproerr_FILENOTDEMO As Integer = -1022
    Public Const qaproerr_FILETIMEGET As Integer = -1023
    Public Const qaproerr_FILETIMESET As Integer = -1024
    Public Const qaproerr_READFAIL As Integer = -1025
    Public Const qaproerr_WRITEFAIL As Integer = -1026
    Public Const qaproerr_BADDRIVE As Integer = -1027
    Public Const qaproerr_BADDIR As Integer = -1028
    Public Const qaproerr_DIRCREATE As Integer = -1029
    Public Const qaproerr_BADOPTION As Integer = -1030
    Public Const qaproerr_BADINIFILE As Integer = -1031
    Public Const qaproerr_BADLOGFILE As Integer = -1032
    Public Const qaproerr_BADMEMORY As Integer = -1033
    Public Const qaproerr_BADHOTKEY As Integer = -1034
    Public Const qaproerr_HOTKEYUSED As Integer = -1035
    Public Const qaproerr_BADRESOURCE As Integer = -1036
    Public Const qaproerr_BADDATADIR As Integer = -1037
    Public Const qaproerr_BADTEMPDIR As Integer = -1038
    Public Const qaproerr_NOTDEFINED As Integer = -1040
    Public Const qaproerr_DUPLICATE As Integer = -1041
    Public Const qaproerr_BADACTION As Integer = -1042
    Public Const qaproerr_BADDATE As Integer = -1045
    Public Const qaproerr_BADTIMEZONE As Integer = -1046
    Public Const qaproerr_CCFAILURE As Integer = -1050
    Public Const qaproerr_CCBADCODE As Integer = -1051
    Public Const qaproerr_CCACCESS As Integer = -1052
    Public Const qaproerr_CCNODONGLE As Integer = -1053
    Public Const qaproerr_CCNOUNITS As Integer = -1054
    Public Const qaproerr_CCNOMETER As Integer = -1055
    Public Const qaproerr_CCNOFEATURE As Integer = -1056
    Public Const qaproerr_CCINVALID As Integer = -1057
    Public Const qaproerr_CCINSTALL As Integer = -1060
    Public Const qaproerr_CCEXPIRED As Integer = -1061
    Public Const qaproerr_CCDATETIME As Integer = -1062
    Public Const qaproerr_CCUSERLIMIT As Integer = -1063
    Public Const qaproerr_CCACTIVATE As Integer = -1064
    Public Const qaproerr_CCBADDRIVE As Integer = -1065
    Public Const qaproerr_CCREGISTER As Integer = -1066
    Public Const qaproerr_UNAUTHORISED As Integer = -1070
    Public Const qaproerr_NOLOCALEFILE As Integer = -1074
    Public Const qaproerr_BADLOCALEFILE As Integer = -1075
    Public Const qaproerr_BADLOCALE As Integer = -1076
    Public Const qaproerr_BADCODEPAGE As Integer = -1077
    Public Const qaproerr_RESOURCEFAIL As Integer = -1078
    Public Const qaproerr_NOTHREAD As Integer = -1080
    Public Const qaproerr_NOTLSMEMORY As Integer = -1081
    Public Const qaproerr_NOTASK As Integer = -1090
    Public Const qaproerr_TOOMANYINSTANCES As Integer = -4501
    Public Const qaproerr_MAXRESOURCES As Integer = -4503
    Public Const qaproerr_OPENFAILURE As Integer = -4551
    Public Const qaproerr_APIHANDLE As Integer = -4552
    Public Const qaproerr_OUTOFSEQUENCE As Integer = -4553
    Public Const qaproerr_BUSYHANDLE As Integer = -4554
    Public Const qaproerr_BADINDEX As Integer = -4556
    Public Const qaproerr_BADVALUE As Integer = -4557
    Public Const qaproerr_BADPARAM As Integer = -4558
    Public Const qaproerr_PARAMTRUNCATED As Integer = -4559
    Public Const qaproerr_NOENGINE As Integer = -4560
    Public Const qaproerr_BADLAYOUT As Integer = -4561
    Public Const qaproerr_BADSTEP As Integer = -4562
    Public Const qaproerr_DATASETNOTAVAILABLE As Integer = -4570
    Public Const qaproerr_LICENSINGFAILURE As Integer = -4571
    Public Const qaproerr_NOACTIVEDATASET As Integer = -4572
    Public Const qaproerr_BADCOUNTRYLIST As Integer = -4573
    Public Const qaproerr_DATAMAPNOTAVAILABLE As Integer = -4574
    Public Const qaproerr_SERVERCONNLOST As Integer = -4580
    Public Const qaproerr_SERVERFULL As Integer = -4581
    Public Const qaproerr_BADMONIKER As Integer = -4590
    Public Const qaproerr_MONIKEREXPIRED As Integer = -4591
    Public Const NO_HANDLE As Integer = 0
    Public Const qaprolibflags_NONE As Integer = 0
    Public Const qaprovalue_FALSE As Integer = 0
    Public Const qaprovalue_TRUE As Integer = 1
    Public Const qaproengine_SINGLELINE As Integer = 1
    Public Const qaproengine_TYPEDOWN As Integer = 2
    Public Const qaproengine_BATCH As Integer = 3
    Public Const qaproengine_VERIFICATION As Integer = 4
    Public Const qaproengine_KEYFINDER As Integer = 5
    Public Const qaproengopt_DEFAULT As Integer = 0
    Public Const qaproengopt_ASYNCSEARCH As Integer = 1
    Public Const qaproengopt_ASYNCSTEPIN As Integer = 2
    Public Const qaproengopt_ASYNCREFINE As Integer = 3
    Public Const qaproengopt_THRESHOLD As Integer = 6
    Public Const qaproengopt_TIMEOUT As Integer = 7
    Public Const qaproengopt_SEARCHINTENSITY As Integer = 8
    Public Const qaprointensity_EXACT As Integer = 0
    Public Const qaprointensity_CLOSE As Integer = 1
    Public Const qaprointensity_EXTENSIVE As Integer = 2
    Public Const qaprostate_NOSEARCH As Integer = 1
    Public Const qaprostate_STILLSEARCHING As Integer = 2
    Public Const qaprostate_TIMEOUT As Integer = 4
    Public Const qaprostate_SEARCHCANCELLED As Integer = 8
    Public Const qaprostate_MAXMATCHES As Integer = 16
    Public Const qaprostate_OVERTHRESHOLD As Integer = 32
    Public Const qaprostate_LARGEPOTENTIAL As Integer = 64
    Public Const qaprostate_MOREOTHERMATCHES As Integer = 128
    Public Const qaprostate_REFINING As Integer = 256
    Public Const qaprostate_AUTOSTEPINSAFE As Integer = 512
    Public Const qaprostate_AUTOSTEPINPASTCLOSE As Integer = 1024
    Public Const qaprostate_CANSTEPOUT As Integer = 2048
    Public Const qaprostate_AUTOFORMATSAFE As Integer = 4096
    Public Const qaprostate_AUTOFORMATPASTCLOSE As Integer = 8192
    Public Const qaprossint_PICKLISTSIZE As Integer = 1
    Public Const qaprossint_POTENTIALMATCHES As Integer = 2
    Public Const qaprossint_SEARCHSTATE As Integer = 3
    Public Const qaprossint_ISNOSEARCH As Integer = 4
    Public Const qaprossint_ISSTILLSEARCHING As Integer = 5
    Public Const qaprossint_ISTIMEOUT As Integer = 6
    Public Const qaprossint_ISSEARCHCANCELLED As Integer = 7
    Public Const qaprossint_ISMAXMATCHES As Integer = 8
    Public Const qaprossint_ISOVERTHRESHOLD As Integer = 9
    Public Const qaprossint_ISLARGEPOTENTIAL As Integer = 10
    Public Const qaprossint_ISMOREOTHERMATCHES As Integer = 11
    Public Const qaprossint_ISREFINING As Integer = 12
    Public Const qaprossint_ISAUTOSTEPINSAFE As Integer = 17
    Public Const qaprossint_ISAUTOSTEPINPASTCLOSE As Integer = 18
    Public Const qaprossint_CANSTEPOUT As Integer = 19
    Public Const qaprossint_ISAUTOFORMATSAFE As Integer = 20
    Public Const qaprossint_ISAUTOFORMATPASTCLOSE As Integer = 21
    Public Const qaproresult_FULLADDRESS As Integer = 1
    Public Const qaproresult_MULTIPLES As Integer = 2
    Public Const qaproresult_CANSTEP As Integer = 4
    Public Const qaproresult_ALIASMATCH As Integer = 8
    Public Const qaproresult_POSTCODERECODED As Integer = 16
    Public Const qaproresult_CROSSBORDERMATCH As Integer = 32
    Public Const qaproresult_DUMMYPOBOX As Integer = 64
    Public Const qaproresult_NAME As Integer = 256
    Public Const qaproresult_INFORMATION As Integer = 1024
    Public Const qaproresult_WARNINFORMATION As Integer = 2048
    Public Const qaproresult_INCOMPLETEADDR As Integer = 4096
    Public Const qaproresult_UNRESOLVABLERANGE As Integer = 8192
    Public Const qaproresult_INCLUDESUSERDATA As Integer = 16384
    Public Const qaproresult_PHANTOMPRIMARYPOINT As Integer = 32768
    Public Const qaproresult_RESOLVEDPPP As Integer = 65536
    Public Const qaproresultstr_DATAID As Integer = 1
    Public Const qaproresultstr_DESCRIPTION As Integer = 2
    Public Const qaproresultstr_PARTIALADDRESS As Integer = 3
    Public Const qaproresultstr_MATCHTYPE As Integer = 5
    Public Const qaproresultint_CONFIDENCE As Integer = 6
    Public Const qaproresultint_UNUSEDLINES As Integer = 7
    Public Const qaproresultint_POSTCODEACTION As Integer = 8
    Public Const qaproresultint_ADDRESSACTION As Integer = 9
    Public Const qaproresultint_GENERICINFO As Integer = 10
    Public Const qaproresultint_COUNTRYINFO1 As Integer = 11
    Public Const qaproresultint_COUNTRYINFO2 As Integer = 12
    Public Const qaproresultint_ISFULLADDRESS As Integer = 13
    Public Const qaproresultint_ISMULTIPLES As Integer = 14
    Public Const qaproresultint_ISCANSTEP As Integer = 15
    Public Const qaproresultint_ISALIASMATCH As Integer = 16
    Public Const qaproresultint_ISPOSTCODERECODED As Integer = 17
    Public Const qaproresultint_ISCROSSBORDERMATCH As Integer = 18
    Public Const qaproresultint_ISDUMMYPOBOX As Integer = 19
    Public Const qaproresultint_ISNAME As Integer = 20
    Public Const qaproresultint_ISINFORMATION As Integer = 21
    Public Const qaproresultint_ISWARNINFORMATION As Integer = 22
    Public Const qaproresultint_ISINCOMPLETEADDR As Integer = 23
    Public Const qaproresultint_ISUNRESOLVABLERANGE As Integer = 24
    Public Const qaproresultint_ISINCLUDEUSERDATA As Integer = 25
    Public Const qaproresultstr_ADDRMATCHCODE As Integer = 26
    Public Const qaproresultstr_POSTCODEMATCHED As Integer = 27
    Public Const qaproresultint_ISPHANTOMPRIMARYPOINT As Integer = 28
    Public Const qapropromptint_LINECOUNT As Integer = 1
    Public Const qapropromptint_DYNAMIC As Integer = 2
    Public Const qaprodatastr_ID As Integer = 1
    Public Const qaprodatastr_DESCRIPTION As Integer = 2
    Public Const qaprodatastr_BASE As Integer = 3
    Public Const qaprolicwarn_NONE As Integer = 0
    Public Const qaprolicwarn_DATAEXPIRING As Integer = 10
    Public Const qaprolicwarn_LICENCEEXPIRING As Integer = 20
    Public Const qaprolicwarn_CLICKSLOW As Integer = 25
    Public Const qaprolicwarn_EVALUATION As Integer = 30
    Public Const qaprolicwarn_NOCLICKS As Integer = 35
    Public Const qaprolicwarn_DATAEXPIRED As Integer = 40
    Public Const qaprolicwarn_EVALLICENCEEXPIRED As Integer = 50
    Public Const qaprolicwarn_FULLLICENCEEXPIRED As Integer = 60
    Public Const qaprolicwarn_LICENCENOTFOUND As Integer = 70
    Public Const qaprolicwarn_DATAUNREADABLE As Integer = 80
    Public Const qaprolicencestr_ID As Integer = 1
    Public Const qaprolicencestr_DESCRIPTION As Integer = 2
    Public Const qaprolicencestr_COPYRIGHT As Integer = 3
    Public Const qaprolicencestr_VERSION As Integer = 4
    Public Const qaprolicencestr_BASECOUNTRY As Integer = 5
    Public Const qaprolicencestr_STATUS As Integer = 6
    Public Const qaprolicencestr_SERVER As Integer = 7
    Public Const qaprolicenceint_WARNINGLEVEL As Integer = 8
    Public Const qaprolicenceint_DAYSLEFT As Integer = 9
    Public Const qaprolicenceint_DATADAYSLEFT As Integer = 10
    Public Const qaprolicenceint_LICENCEDAYSLEFT As Integer = 11
    Public Const qaprocancelflag_NONE As Integer = 0
    Public Const qaprocancelflag_BLOCKING As Integer = 1
    Public Const qaproformat_OVERFLOW As Integer = 1
    Public Const qaproformat_TRUNCATED As Integer = 2
    Public Const qaproformat_DPVVALID As Integer = 16
    Public Const qaproformat_DPVINVALID As Integer = 32
    Public Const qaproformat_DPVLOCKED As Integer = 64
    Public Const qaproformatted_NONE As Integer = 0
    Public Const qaproformatted_NAME As Integer = 1
    Public Const qaproformatted_ADDRESS As Integer = 2
    Public Const qaproformatted_ANCILLARY As Integer = 4
    Public Const qaproformatted_DATAPLUS As Integer = 8
    Public Const qaproformatted_TRUNCATED As Integer = 16
    Public Const qaproformatted_OVERFLOW As Integer = 32
    Public Const qaproformatted_DATAPLUSSYNTAX As Integer = 64
    Public Const qaproformatted_DATAPLUSEXPIRED As Integer = 128
    Public Const qaproformatted_DATAPLUSBLANK As Integer = 256
    Public Const qaproformatted_UNMATCHED As Integer = 512
    Public Const qaprosysinfo_SYSTEM As Integer = 1
    Public Const qaprounusedstr_TEXT As Integer = 1
    Public Const qaprounusedint_COMPLETENESS As Integer = 2
    Public Const qaprounusedint_TYPE As Integer = 3
    Public Const qaprounusedint_POSITION As Integer = 4
    Public Const qaprounusedint_ISCAREOF As Integer = 5
    Public Const qaprounusedint_ISPREMSUFFIX As Integer = 6


    Public Const qaprofields_ADDRESSLINE1 As Integer = 0
    Public Const qaprofields_ADDRESSLINE2 As Integer = 1
    Public Const qaprofields_ADDRESSLINE3 As Integer = 2
    Public Const qaprofields_ADDRESSLINE4 As Integer = 3
    Public Const qaprofields_POSTCODE As Integer = 4

    'Declare Function SetEnvironmentVariable Lib "kernel64" Alias "SetEnvironmentVariableA" (ByVal lpName As String, ByVal lpValue As String) As Integer
    Declare Function QA_SetLibraryFlags Lib "QAUPIED.DLL" (ByVal vl1 As Integer) As Integer
    Declare Function QA_Open Lib "QAUPIED.DLL" (ByVal vs1 As String, ByVal vs2 As String, ByRef ri3 As Integer) As Integer
    Declare Function QA_Close Lib "QAUPIED.DLL" (ByVal vi1 As Integer) As Integer
    Declare Sub QA_Shutdown Lib "QAUPIED.DLL" ()
    Declare Function QA_SetEngine Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer) As Integer
    Declare Function QA_GetEngine Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByRef ri2 As Integer) As Integer
    Declare Function QA_SetEngineOption Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByVal vl3 As Integer) As Integer
    Declare Function QA_GetEngineOption Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByRef rl3 As Integer) As Integer
    Declare Function QA_GetEngineStatus Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByRef ri3 As Integer) As Integer
    Declare Function QA_GetPromptStatus Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByRef ri3 As Integer, ByVal rs4 As String, ByVal vi5 As Integer) As Integer
    Declare Function QA_GetPrompt Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByVal rs3 As String, ByVal vi4 As Integer, ByRef ri5 As Integer, ByVal rs6 As String, ByVal vi7 As Integer) As Integer
    Declare Function QA_Search Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vs2 As String) As Integer
    Declare Function QA_CancelSearch Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vl2 As Integer) As Integer
    Declare Function QA_EndSearch Lib "QAUPIED.DLL" (ByVal vi1 As Integer) As Integer
    Declare Function QA_GetSearchStatus Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByRef ri2 As Integer, ByRef ri3 As Integer, ByRef rl4 As Integer) As Integer
    Declare Function QA_GetSearchStatusDetail Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByRef rl3 As Integer, ByVal rs4 As String, ByVal vi5 As Integer) As Integer
    Declare Function QA_StepIn Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer) As Integer
    Declare Function QA_StepOut Lib "QAUPIED.DLL" (ByVal vi1 As Integer) As Integer
    Declare Function QA_GetResult Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByVal rs3 As String, ByVal vi4 As Integer, ByRef ri5 As Integer, ByRef rl6 As Integer) As Integer
    Declare Function QA_GetResultDetail Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByVal vi3 As Integer, ByRef rl4 As Integer, ByVal rs5 As String, ByVal vi6 As Integer) As Integer
    Declare Function QA_FormatResult Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByVal vs3 As String, ByRef ri4 As Integer, ByRef rl5 As Integer) As Integer
    Declare Function QA_GetFormattedLine Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByVal rs3 As String, ByVal vi4 As Integer, ByVal rs5 As String, ByVal vi6 As Integer, ByRef rl7 As Integer) As Integer
    Declare Function QA_GetExampleCount Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByRef ri2 As Integer) As Integer
    Declare Function QA_FormatExample Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByVal rs3 As String, ByVal vi4 As Integer, ByRef ri5 As Integer, ByRef rl6 As Integer) As Integer
    Declare Function QA_GetLayoutCount Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByRef ri2 As Integer) As Integer
    Declare Function QA_GetLayout Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByVal rs3 As String, ByVal vi4 As Integer, ByVal rs5 As String, ByVal vi6 As Integer) As Integer
    Declare Function QA_GetActiveLayout Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal rs2 As String, ByVal vi3 As Integer) As Integer
    Declare Function QA_SetActiveLayout Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vs2 As String) As Integer
    Declare Function QA_GetDataCount Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByRef ri2 As Integer) As Integer
    Declare Function QA_GetData Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByVal rs3 As String, ByVal vi4 As Integer, ByVal rs5 As String, ByVal vi6 As Integer) As Integer
    Declare Function QA_GetDataDetail Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByVal vi3 As Integer, ByRef rl4 As Integer, ByVal rs5 As String, ByVal vi6 As Integer) As Integer
    Declare Function QA_GetLicensingCount Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByRef ri2 As Integer, ByRef rl3 As Integer) As Integer
    Declare Function QA_GetLicensingDetail Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByVal vi3 As Integer, ByRef rl4 As Integer, ByVal rs5 As String, ByVal vi6 As Integer) As Integer
    Declare Function QA_GetActiveData Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal rs2 As String, ByVal vi3 As Integer) As Integer
    Declare Function QA_SetActiveData Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vs2 As String) As Integer
    Declare Function QA_GenerateSystemInfo Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByRef ri3 As Integer) As Integer
    Declare Function QA_GetSystemInfo Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByVal rs3 As String, ByVal vi4 As Integer) As Integer
    Declare Function QA_ErrorMessage Lib "QAUPIED.DLL" (ByVal vi1 As Integer, ByVal rs2 As String, ByVal vi3 As Integer) As Integer
    Declare Function QA_IsFlagSet Lib "QAUPIED.DLL" (ByVal vl1 As Integer, ByVal vl2 As Integer) As Integer

    '**** End of File ****



End Module
