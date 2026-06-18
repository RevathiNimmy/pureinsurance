Option Strict Off
Option Explicit On
Module QASConst
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    '**** Visual Basic (32 bit) header file for QAPROEA.DLL v1.00(34) ****

    ' RDC 10072002 constants for warning messages
    Public Const PMQAS_WARN_NUMBEREDFLAT As String = "Address may be a block of flats"
    Public Const PMQAS_WARN_POSTCODERECODED As String = "Postcode has been recoded"
    Public Const PMQAS_WARN_SUBSMADE As String = "Substitutions have been made"
    Public Const PMQAS_WARN_AREALEVEL As String = "Correct to area level"
    Public Const PMQAS_WARN_DISTRICTLEVEL As String = "Correct to district level"
    Public Const PMQAS_WARN_SECTORLEVEL As String = "Correct to sector level"
    Public Const PMQAS_WARN_HALFSECTORLEVEL As String = "Correct to half sector level"

    Public Const qaerr_FATAL As Integer = -1000
    Public Const qaerr_NOMEMORY As Integer = -1001
    Public Const qaerr_INITOOLARGE As Integer = -1005
    Public Const qaerr_ININOEXTEND As Integer = -1006
    Public Const qaerr_FILEOPEN As Integer = -1010
    Public Const qaerr_FILEEXIST As Integer = -1011
    Public Const qaerr_FILEREAD As Integer = -1012
    Public Const qaerr_FILEWRITE As Integer = -1013
    Public Const qaerr_FILEDELETE As Integer = -1014
    Public Const qaerr_FILEACCESS As Integer = -1016
    Public Const qaerr_FILEVERSION As Integer = -1017
    Public Const qaerr_FILEHANDLE As Integer = -1018
    Public Const qaerr_FILECREATE As Integer = -1019
    Public Const qaerr_FILERENAME As Integer = -1020
    Public Const qaerr_FILEEXPIRED As Integer = -1021
    Public Const qaerr_FILENOTDEMO As Integer = -1022
    Public Const qaerr_READFAIL As Integer = -1025
    Public Const qaerr_WRITEFAIL As Integer = -1026
    Public Const qaerr_BADDRIVE As Integer = -1027
    Public Const qaerr_BADDIR As Integer = -1028
    Public Const qaerr_DIRCREATE As Integer = -1029
    Public Const qaerr_BADOPTION As Integer = -1030
    Public Const qaerr_BADINIFILE As Integer = -1031
    Public Const qaerr_BADLOGFILE As Integer = -1032
    Public Const qaerr_BADMEMORY As Integer = -1033
    Public Const qaerr_BADHOTKEY As Integer = -1034
    Public Const qaerr_HOTKEYUSED As Integer = -1035
    Public Const qaerr_BADRESOURCE As Integer = -1036
    Public Const qaerr_BADDATADIR As Integer = -1037
    Public Const qaerr_BADTEMPDIR As Integer = -1038
    Public Const qaerr_NOTDEFINED As Integer = -1040
    Public Const qaerr_DUPLICATE As Integer = -1041
    Public Const qaerr_BADACTION As Integer = -1042
    Public Const qaerr_CCFAILURE As Integer = -1050
    Public Const qaerr_CCBADCODE As Integer = -1051
    Public Const qaerr_CCACCESS As Integer = -1052
    Public Const qaerr_CCINSTALL As Integer = -1060
    Public Const qaerr_CCEXPIRED As Integer = -1061
    Public Const qaerr_CCDATETIME As Integer = -1062
    Public Const qaerr_CCUSERLIMIT As Integer = -1063
    Public Const qaerr_CCACTIVATE As Integer = -1064
    Public Const qaerr_CCBADDRIVE As Integer = -1065
    Public Const qaerr_UNAUTHORISED As Integer = -1070
    Public Const qaerr_NOTHREAD As Integer = -1080
    Public Const qaerr_NOTASK As Integer = -1090

    Declare Sub QAInitialise Lib "QAPROEA.DLL" (ByVal vi1 As Integer)
    Declare Sub QAErrorMessage Lib "QAPROEA.DLL" (ByVal vi1 As Integer, ByVal rs2 As String, ByVal vi3 As Integer)
    Declare Function QAErrorLevel Lib "QAPROEA.DLL" (ByVal vi1 As Integer) As Integer
    Declare Sub QAVersionInfo Lib "QAPROEA.DLL" (ByVal rs1 As String, ByVal vi2 As Integer)
    Declare Function QADataInfo Lib "QAPROEA.DLL" (ByVal rs1 As String, ByVal vi2 As Integer, ByRef ri3 As Integer) As Integer
    Declare Function QASystemInfo Lib "QAPROEA.DLL" (ByVal vi1 As Integer, ByVal rs2 As String, ByVal vi3 As Integer) As Integer
    Declare Sub QAUpdateKey Lib "QAPROEA.DLL" (ByVal rs1 As String, ByVal vi2 As Integer)
    Declare Function QAUpdateCode Lib "QAPROEA.DLL" (ByVal vs1 As String) As Integer
    Declare Function QALicenseInfo Lib "QAPROEA.DLL" (ByRef ri1 As Integer, ByRef ri2 As Integer, ByRef ri3 As Integer) As Integer
    Declare Function QAAuthorise Lib "QAPROEA.DLL" (ByVal vs1 As String, ByVal vl2 As Integer) As Integer

    Public Const COM_ORG_START As Integer = 80000000
    Public Const LARGE_ORG_START As Integer = 90000000
    Public Const SMALL_ORG_START As Integer = 100000000
    Public Const LASTORG As Integer = 1
    Public Const CURRENT As Integer = 2
    Public Const DELETED As Integer = 0
    Public Const qaerr_KEYEOF As Integer = -32000
    Public Const qaerr_KEYISOPEN As Integer = -32001
    Public Const qaerr_KEYNOPEN As Integer = -32002
    Public Const qaerr_KEYBMKEY As Integer = -32003
    Public Const qaerr_KEYNFND As Integer = -32004
    Public Const qaerr_KEYBUFOVF As Integer = -32005
    Public Const qaerr_KEYORGNFND As Integer = -32006
    Public Const qaerr_KEYNSRCH As Integer = -32007
    Public Const qaerr_KEYBGET As Integer = -32008
    Public Const qaerr_KEYBADSEQ As Integer = -32009
    Public Const qaerr_KEYNCURRENT As Integer = -32010
    Public Const qaerr_KEYBORG As Integer = -32011

    Declare Function QAProKey_Open Lib "QAPROEA.DLL" () As Integer
    Declare Function QAProKey_Close Lib "QAPROEA.DLL" () As Integer
    Declare Function QAProKey_Search Lib "QAPROEA.DLL" (ByVal vi1 As Integer, ByVal vs2 As String, ByVal vs3 As String, ByRef ri4 As Integer) As Integer
    Declare Function QAProKey_Get Lib "QAPROEA.DLL" (ByVal vi1 As Integer, ByVal rs2 As String, ByRef ri3 As Integer, ByRef ri4 As Integer, ByVal rs5 As String, ByVal rs6 As String) As Integer

    Public Const qaerr_OVERFLOW As Integer = -1300
    Public Const qaerr_NOSPEC As Integer = -1301
    Public Const qaerr_FIELDTRUNCATED As Integer = -1302
    Public Const INVALID_SOCKET As Integer = -1
    Public Const SOCKET_ERROR As Integer = -1
    Public Const qaerr_SOCKETCREATE As Integer = -9299
    Public Const qaerr_SOCKETCONNECT As Integer = -9298
    Public Const qaerr_SOCKETERROR As Integer = -9297
    Public Const qaerr_SOCKETTIMEOUT As Integer = -9296
    Public Const qaerr_NAMELOOKUP As Integer = -9295
    Public Const qaerr_SOCKETBIND As Integer = -9294
    Public Const qaerr_SOCKETLISTEN As Integer = -9293
    Public Const qaerr_SOCKETACCEPT As Integer = -9292
    Public Const qaerr_SOCKETBADIP As Integer = -9291
    Public Const qaerr_NODATASETONSERVER As Integer = -9300
    Public Const qaerr_TOOMANYCODES As Integer = -9979
    Public Const qaerr_SYNTAX As Integer = -9988
    Public Const qaerr_CANTSTEP As Integer = -9987
    Public Const qaerr_NONUMBER As Integer = -9984
    Public Const qaerr_NODP As Integer = -9983
    Public Const qaerr_NOMATCH As Integer = -9978
    Public Const qaerr_INDEXOPEN As Integer = -9977
    Public Const qaerr_PREMISESOPEN As Integer = -9976
    Public Const qaerr_STREETSOPEN As Integer = -9975
    Public Const qaerr_INDEXVERSION As Integer = -9970
    Public Const qaerr_PREMISESVERSION As Integer = -9969
    Public Const qaerr_TOOSHORT As Integer = -9968
    Public Const qaerr_STREETSVERSION As Integer = -9967
    Public Const qaerr_PRODATAEXPIRED As Integer = -9966
    Public Const qaerr_SURNAMEINDEXOPEN As Integer = -9990
    Public Const qaerr_FORENAMEINDEXOPEN As Integer = -9991
    Public Const qaerr_BADSEARCHDESC As Integer = -10000
    Public Const qaerr_BADFILEFORMAT As Integer = -9989
    Public Const qaerr_CANCELLED As Integer = -9982
    Public Const qaerr_TOOMANYMATCHES As Integer = -9980
    Public Const qaerr_NOSUCHAREA As Integer = -9981
    Public Const qaerr_BADPHASE As Integer = -9974
    Public Const qaerr_NESTEDTOODEEP As Integer = -9973
    Public Const qaerr_BADINFO As Integer = -9972
    Public Const qaerr_TOOLONG As Integer = -9971
    Public Const qafields_ORGANISATION As Integer = 0
    Public Const qafields_POBOX As Integer = 1
    Public Const qafields_SUBPREMISES As Integer = 2
    Public Const qafields_BUILDINGNAME As Integer = 3
    Public Const qafields_BUILDINGNUMBER As Integer = 4
    Public Const qafields_DEPTHORO As Integer = 5
    Public Const qafields_THORO As Integer = 6
    Public Const qafields_DEPLOCAL As Integer = 7
    Public Const qafields_LOCAL As Integer = 8
    Public Const qafields_POSTTOWN As Integer = 9
    Public Const qafields_COUNTY As Integer = 10
    Public Const qafields_POSTCODE As Integer = 11
    Public Const qapro_STEPINFO As Integer = 0
    Public Const qapro_RANGEINFO As Integer = 1
    Public Const qapro_TYPEINFO As Integer = 2
    Public Const qapro_DPPINFO As Integer = 3
    Public Const qapro_QUALITYINFO As Integer = 4

    Declare Function QAPro_Open Lib "QAPROEA.DLL" (ByVal vs1 As String, ByVal vs2 As String) As Integer
    Declare Sub QAPro_SetTimeout Lib "QAPROEA.DLL" (ByVal vl1 As Integer)
    Declare Sub QAPro_Close Lib "QAPROEA.DLL" ()
    Declare Function QAPro_ChangeFormat Lib "QAPROEA.DLL" (ByVal vs1 As String) As Integer
    Declare Function QAPro_Search Lib "QAPROEA.DLL" (ByVal vs1 As String) As Integer
    Declare Function QAPro_SearchDPP Lib "QAPROEA.DLL" (ByVal vs1 As String, ByVal vi2 As Integer) As Integer
    Declare Sub QAPro_EndSearch Lib "QAPROEA.DLL" ()
    Declare Function QAPro_Count Lib "QAPROEA.DLL" () As Integer
    Declare Function QAPro_ListItem Lib "QAPROEA.DLL" (ByVal vl1 As Integer, ByVal rs2 As String, ByVal vi3 As Integer, ByVal vi4 As Integer) As Integer
    Declare Function QAPro_StepIn Lib "QAPROEA.DLL" (ByVal vl1 As Integer) As Integer
    Declare Function QAPro_StepOut Lib "QAPROEA.DLL" () As Integer
    Declare Function QAPro_Select Lib "QAPROEA.DLL" (ByVal vs1 As String, ByVal vi2 As Integer, ByVal vi3 As Integer) As Integer
    Declare Function QAPro_EndSelect Lib "QAPROEA.DLL" () As Integer
    Declare Function QAPro_Pick Lib "QAPROEA.DLL" (ByVal vs1 As String, ByVal vi2 As Integer) As Integer
    Declare Function QAPro_EndPick Lib "QAPROEA.DLL" () As Integer
    Declare Function QAPro_Back Lib "QAPROEA.DLL" () As Integer
    Declare Function QAPro_First Lib "QAPROEA.DLL" (ByVal vs1 As String, ByVal vi2 As Integer, ByRef rl3 As Integer) As Integer
    Declare Function QAPro_AddrLine Lib "QAPROEA.DLL" (ByVal vl1 As Integer, ByVal vi2 As Integer, ByVal vs3 As String, ByVal rs4 As String, ByVal vi5 As Integer) As Integer
    Declare Function QAPro_FormatLine Lib "QAPROEA.DLL" (ByVal vl1 As Integer, ByVal vi2 As Integer, ByVal vs3 As String, ByVal rs4 As String, ByVal vi5 As Integer) As Integer
    Declare Function QAPro_FormatAddr Lib "QAPROEA.DLL" (ByVal vl1 As Integer, ByVal vs2 As String, ByVal rs3 As String, ByVal vi4 As Integer) As Integer
    Declare Function QAPro_FormatCount Lib "QAPROEA.DLL" () As Integer
    Declare Function QAPro_GetItemInfo Lib "QAPROEA.DLL" (ByVal vl1 As Integer, ByVal vi2 As Integer, ByRef rl3 As Integer) As Integer
    Declare Function QAPro_GetDPP Lib "QAPROEA.DLL" (ByVal vl1 As Integer, ByVal vs2 As String, ByRef ri3 As Integer) As Integer
    Declare Sub QAPro_CCUserUpdate Lib "QAPROEA.DLL" ()
    Declare Function QAPro_CCReadCounter Lib "QAPROEA.DLL" () As Integer

    Public Const qaattribs_NONE As Integer = 0
    Public Const qaattribs_NODIALOG As Integer = 1
    Public Const qaattribs_NOWAITENTER As Integer = 2
    Public Const qaattribs_NOLAYOUTBUTTON As Integer = 4
    Public Const qaattribs_NOHELPBUTTON As Integer = 8
    Public Const qaattribs_NOWARNCC As Integer = 16
    Public Const qaattribs_SUPPRESSNAMES As Integer = 32
    Public Const qaattribs_NONAMES As Integer = 64
    Public Const qaattribs_KEEPRESULTS As Integer = 16384
    Public Const qaattribs_DEMOMODE As Integer = 32768
    Public Const qaerr_UINOTSTARTED As Integer = -1900
    Public Const qaerr_UISTARTED As Integer = -1901
    Public Const qaerr_NORESULTS As Integer = -1920
    Public Const qaerr_MOREMATCHES As Integer = -1921
    Public Const qaerr_SEARCHRECODED As Integer = -1922
    Public Const qaerr_NOTEXACT As Integer = -1923
    Public Const qaerr_DATALOST As Integer = -1924

    Declare Function QAProUI_Startup Lib "QAPROEA.DLL" (ByVal vs1 As String, ByVal vs2 As String, ByVal vs3 As String, ByVal vl4 As Integer) As Integer
    Declare Function QAProUI_DPPopup Lib "QAPROEA.DLL" (ByVal vs1 As String, ByVal rs2 As String, ByVal vi3 As Integer, ByVal vs4 As String, ByRef ri5 As Integer) As Integer
    Declare Function QAProUI_Popup Lib "QAPROEA.DLL" (ByVal vs1 As String, ByVal rs2 As String, ByVal vi3 As Integer) As Integer
    Declare Sub QAProUI_Shutdown Lib "QAPROEA.DLL" (ByVal vi1 As Integer)
    Declare Function QAProUI_Config Lib "QAPROEA.DLL" (ByVal vs1 As String) As Integer
    Declare Sub QAProUI_IniSection Lib "QAPROEA.DLL" (ByVal rs1 As String, ByVal vi2 As Integer)

    '**** End of File ****
End Module