Option Strict Off
Option Explicit On
Module QASNamesConst
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    '**** Visual Basic (32 bit) header file for QANUIEB.DLL v3.20(80) ****

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
    Public Const qaerr_CCNODONGLE As Integer = -1053
    Public Const qaerr_CCNOUNITS As Integer = -1054
    Public Const qaerr_CCNOMETER As Integer = -1055
    Public Const qaerr_CCNOFEATURE As Integer = -1056
    Public Const qaerr_CCINSTALL As Integer = -1060
    Public Const qaerr_CCEXPIRED As Integer = -1061
    Public Const qaerr_CCDATETIME As Integer = -1062
    Public Const qaerr_CCUSERLIMIT As Integer = -1063
    Public Const qaerr_CCACTIVATE As Integer = -1064
    Public Const qaerr_CCBADDRIVE As Integer = -1065
    Public Const qaerr_UNAUTHORISED As Integer = -1070
    Public Const qaerr_NOTHREAD As Integer = -1080
    Public Const qaerr_NOTASK As Integer = -1090

    Declare Sub N_QAInitialise Lib "QANUIEB.DLL" Alias "QAInitialise" (ByVal vi1 As Integer)
    Declare Sub N_QAErrorMessage Lib "QANUIEB.DLL" Alias "QAErrorMessage" (ByVal vi1 As Integer, ByVal rs2 As String, ByVal vi3 As Integer)
    Declare Function N_QAErrorLevel Lib "QANUIEB.DLL" Alias "QAErrorLevel" (ByVal vi1 As Integer) As Integer
    Declare Function N_QAErrorHistory Lib "QANUIEB.DLL" Alias "QAErrorHistory" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByVal rs3 As String, ByVal vi4 As Integer) As Integer
    Declare Sub N_QAVersionInfo Lib "QANUIEB.DLL" Alias "QAVersionInfo" (ByVal rs1 As String, ByVal vi2 As Integer)
    Declare Function N_QADataInfo Lib "QANUIEB.DLL" Alias "QADataInfo" (ByVal rs1 As String, ByVal vi2 As Integer, ByRef ri3 As Integer) As Integer
    Declare Function N_QASystemInfo Lib "QANUIEB.DLL" Alias "QASystemInfo" (ByVal vi1 As Integer, ByVal rs2 As String, ByVal vi3 As Integer) As Integer
    Declare Sub N_QAUpdateKey Lib "QANUIEB.DLL" Alias "QAUpdateKey" (ByVal rs1 As String, ByVal vi2 As Integer)
    Declare Function N_QAUpdateCode Lib "QANUIEB.DLL" Alias "QAUpdateCode" (ByVal vs1 As String) As Integer
    Declare Function N_QALicenseInfo Lib "QANUIEB.DLL" Alias "QALicenseInfo" (ByRef ri1 As Integer, ByRef ri2 As Integer, ByRef ri3 As Integer) As Integer
    Declare Function N_QAAuthorise Lib "QANUIEB.DLL" Alias "QAAuthorise" (ByVal vs1 As String, ByVal vl2 As Integer) As Integer

    Public Const matchlevel_NOLOOKUP As Integer = 0
    Public Const matchlevel_AREANOTFOUND As Integer = 2
    Public Const matchlevel_AREA As Integer = 3
    Public Const matchlevel_DISTRICT As Integer = 4
    Public Const matchlevel_SECTOR As Integer = 5
    Public Const matchlevel_HALFSECTOR As Integer = 6
    Public Const matchlevel_POSTCODE As Integer = 7
    Public Const matchlevel_DELIVPT As Integer = 8
    Public Const storelevel_NECESSARYPOST As Integer = 0
    Public Const storelevel_EXACTPOST As Integer = 1
    Public Const storelevel_DELIVPT As Integer = 2
    Public Const storelevel_HOUSE As Integer = 3
    Public Const storelevel_NAME As Integer = 4
    Public Const dpst_EMPTY As Integer = 0
    Public Const dpst_CLOSED As Integer = 1
    Public Const dpst_OPENFAILED As Integer = 2
    Public Const dpst_OPENING As Integer = 3
    Public Const dpst_ATTACHING As Integer = 4
    Public Const dpst_OPEN As Integer = 5
    Public Const dpst_ATTACHED As Integer = 6
    Public Const dpfmt_STANDARD As Integer = 0
    Public Const dpfmt_DISPLAY As Integer = 1
    Public Const dpfmt_LANDRANGER As Integer = 2
    Public Const dpfmt_10KM As Integer = 4
    Public Const dpfmt_1KM As Integer = 8
    Public Const dpfmt_100M As Integer = 12
    Public Const dpfmt_10M As Integer = 16
    Public Const dpfmt_1M As Integer = 20
    Public Const dpfmt_ADDP As Integer = 24
    Public Const dp_GRIDRESBITS As Integer = 28
    Public Const dpfmt_SPACEPAD As Integer = 32
    Public Const dpfmt_OLDGRID As Integer = 64
    Public Const dpfmt_OLDAKEY As Integer = 65
    Public Const dpfmt_BMWAKEY As Integer = 66
    Public Const dpfmt_DELV1 As Integer = 67
    Public Const dpfmt_DELV2 As Integer = 68
    Public Const codetype_TEXT As Integer = 0
    Public Const codetype_BINARY As Integer = 1
    Public Const qaerr_DATNOTSTARTED As Integer = -1700
    Public Const qaerr_NODATASETSOPEN As Integer = -1701
    Public Const qaerr_DATISSTARTED As Integer = -1702
    Public Const qaerr_NOTOPEN As Integer = -1703
    Public Const qaerr_CLOSEERRORS As Integer = -1704
    Public Const qaerr_ISOPEN As Integer = -1705
    Public Const qaerr_NEEDLATERREV As Integer = -1709
    Public Const qaerr_CANTOPENINFFILE As Integer = -1710
    Public Const qaerr_BADINFFILE As Integer = -1711
    Public Const qaerr_CANTOPENDATFILE As Integer = -1712
    Public Const qaerr_BADDATFILE As Integer = -1713
    Public Const qaerr_NOTRAILER As Integer = -1714
    Public Const qaerr_FILEMISMATCH As Integer = -1715
    Public Const qaerr_COPYCONTROL As Integer = -1716
    Public Const qaerr_NOUNITS As Integer = -1717
    Public Const qaerr_DATASETCONFIG As Integer = -1718
    Public Const qaerr_V1DATASET As Integer = -1719
    Public Const qaerr_POSTCODEINVALID As Integer = -1720
    Public Const qaerr_DPPINVALID As Integer = -1721
    Public Const qaerr_DATAFORMAT As Integer = -1722
    Public Const qaerr_DATASETNOTFOUND As Integer = -1730
    Public Const qaerr_IDEMPTY As Integer = -1731
    Public Const qaerr_IDINUSE As Integer = -1732
    Public Const qaerr_IDINVALID As Integer = -1733
    Public Const qaerr_CODESELINVALID As Integer = -1734
    Public Const qaerr_ITEMSELINVALID As Integer = -1735
    Public Const qaerr_ITEMNAMEINVALID As Integer = -1736
    Public Const qaerr_INFIDINVALID As Integer = -1737
    Public Const qaerr_NOLOOKUP As Integer = -1740
    Public Const qaerr_NOCODE As Integer = -1741
    Public Const qaerr_CODETRUNC As Integer = -1742
    Public Const qaerr_NOINF As Integer = -1743
    Public Const qaerr_BLANKINF As Integer = -1744
    Public Const qaerr_INFTRUNC As Integer = -1745
    Public Const qaerr_CODEINFTRUNC As Integer = -1746
    Public Const qaerr_ITEMTRUNC As Integer = -1747
    Public Const qaerr_CODESONLY As Integer = -1748
    Public Const qaerr_NOBUFFER As Integer = -1749
    Public Const qaerr_BUFFERTOOSMALL As Integer = -1750
    Public Const qaerr_NODPSDATA As Integer = -1760
    Public Const qaerr_POSTCODENOTFOUND As Integer = -1761
    Public Const qaerr_DPSNOTFOUND As Integer = -1762
    Public Const qaerr_DPSINVALID As Integer = -1763
    Public Const dpinf_TITLE As Integer = 0
    Public Const dpinf_VERSION As Integer = 1
    Public Const dpinf_STORELEVEL As Integer = 2
    Public Const dpinf_MULTCODES As Integer = 3
    Public Const dpinf_VARILENCODES As Integer = 4
    Public Const dpinf_CODETYPE As Integer = 5
    Public Const dpinf_METERED As Integer = 6
    Public Const dpinf_CODENAME As Integer = 7
    Public Const dpinf_MAXCODELEN As Integer = 8
    Public Const dpinf_CODEITEMSEP As Integer = 9
    Public Const dpinf_INFNAME As Integer = 10
    Public Const dpinf_MAXINFLEN As Integer = 11
    Public Const dpinf_INFITEMSEP As Integer = 12

    Declare Function QADP_Startup Lib "QANUIEB.DLL" (ByVal vs1 As String, ByVal vs2 As String) As Integer
    Declare Function QADP_Shutdown Lib "QANUIEB.DLL" () As Integer
    Declare Function QADP_Open Lib "QANUIEB.DLL" (ByVal vs1 As String, ByVal vs2 As String, ByVal vs3 As String, ByVal vi4 As Integer) As Integer
    Declare Function QADP_Close Lib "QANUIEB.DLL" (ByVal vi1 As Integer) As Integer
    Declare Function QADP_DataSpec Lib "QANUIEB.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByVal rs3 As String, ByVal vi4 As Integer, ByVal rs5 As String, ByVal vi6 As Integer) As Integer
    Declare Function QADP_ItemCount Lib "QANUIEB.DLL" (ByVal vi1 As Integer) As Integer
    Declare Function QADP_ID Lib "QANUIEB.DLL" (ByVal vs1 As String) As Integer
    Declare Function QADP_Name Lib "QANUIEB.DLL" (ByVal vi1 As Integer, ByVal rs2 As String, ByVal vi3 As Integer) As Integer
    Declare Function QADP_ItemName Lib "QANUIEB.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByVal rs3 As String, ByVal vi4 As Integer) As Integer
    Declare Function QADP_MaxID Lib "QANUIEB.DLL" () As Integer
    Declare Function QADP_OpenCount Lib "QANUIEB.DLL" () As Integer
    Declare Function QADP_LookUp Lib "QANUIEB.DLL" (ByVal vs1 As String, ByVal vi2 As Integer) As Integer
    Declare Function QADP_LookUpOne Lib "QANUIEB.DLL" (ByVal vi1 As Integer, ByVal vs2 As String, ByVal vi3 As Integer) As Integer
    Declare Function QADP_EndLookUp Lib "QANUIEB.DLL" () As Integer
    Declare Function QADP_CodeCount Lib "QANUIEB.DLL" (ByVal vi1 As Integer) As Integer
    Declare Function QADP_Get Lib "QANUIEB.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByVal rs3 As String, ByVal vi4 As Integer, ByVal rs5 As String, ByVal vi6 As Integer) As Integer
    Declare Function QADP_GetItem Lib "QANUIEB.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByVal vi3 As Integer, ByVal rs4 As String, ByVal vi5 As Integer, ByVal rs6 As String, ByVal vi7 As Integer) As Integer
    Declare Function QADP_GetItemByName Lib "QANUIEB.DLL" (ByVal vs1 As String, ByVal vi2 As Integer, ByVal vs3 As String, ByVal rs4 As String, ByVal vi5 As Integer) As Integer
    Declare Function QADP_GetDPP Lib "QANUIEB.DLL" (ByVal vs1 As String) As Integer
    Declare Function QADP_Status Lib "QANUIEB.DLL" (ByVal vi1 As Integer) As Integer
    Declare Function QADP_State Lib "QANUIEB.DLL" (ByVal vi1 As Integer) As Integer
    Declare Function QADP_MatchLevel Lib "QANUIEB.DLL" (ByVal vi1 As Integer) As Integer
    Declare Function QADP_Release Lib "QANUIEB.DLL" (ByVal vi1 As Integer) As Integer

    Public Const qaerr_OVERFLOW As Integer = -1300
    Public Const qaerr_NOSPEC As Integer = -1301
    Public Const qaerr_FIELDTRUNCATED As Integer = -1302
    'Global Const qaerr_CANCELLED = -9982
    Public Const qaerr_TIMEDOUT As Integer = -10001
    Public Const qaopevent_START As Integer = 0
    Public Const qaopevent_PROGRESSINFO As Integer = 1
    Public Const qaopevent_POLLCANCEL As Integer = 2
    Public Const qaopevent_STOP As Integer = 3
    'Global Const qafields_ORGANISATION = 0
    'Global Const qafields_POBOX = 1
    'Global Const qafields_SUBPREMISES = 2
    'Global Const qafields_BUILDINGNAME = 3
    'Global Const qafields_BUILDINGNUMBER = 4
    'Global Const qafields_DEPTHORO = 5
    'Global Const qafields_THORO = 6
    'Global Const qafields_DEPLOCAL = 7
    'Global Const qafields_LOCAL = 8
    'Global Const qafields_POSTTOWN = 9
    'Global Const qafields_COUNTY = 10
    'Global Const qafields_POSTCODE = 11
    Public Const qafields_ABBREVCOUNTY As Integer = 12

    Public Const qafields_SURNAME As Integer = 13
    Public Const qafields_FORENAME As Integer = 14
    Public Const qafields_KEY As Integer = 15
    Public Const qaerr_RAPIDOPEN As Integer = -9800
    'Global Const qaerr_NONRAPIDFILE = -9801
    'Global Const qaerr_INVALIDAREA = -9802
    'Global Const qaerr_AREALEVEL = -9803
    'Global Const qaerr_DISTRICTLEVEL = -9804
    'Global Const qaerr_SECTORLEVEL = -9805
    'Global Const qaerr_HALFSECTORLEVEL = -9806
    'Global Const qaerr_NOCODES = -9807
    'Global Const qaerr_NOAREADATA = -9809
    'Global Const qaerr_NUMBEREDFLAT = -9810
    'Global Const qaerr_POSTCODERECODED = -9811
    'Global Const qaerr_SUBSMADE = -9813
    'Global Const qaerr_BADPOSTCODECHAR = -9814
    'Global Const qaerr_RAPIDNOTSTARTED = -9815
    'Global Const rawaddress_DEPTHORO = 5
    'Global Const rawaddress_THORO = 6
    'Global Const rawaddress_DDEPLOCAL = 7
    'Global Const rawaddress_DEPLOCAL = 8
    'Global Const rawaddress_POSTTOWN = 9
    'Global Const rawaddress_COUNTY = 10
    'Global Const rawaddress_POSTCODE = 11

    Public Const qaerr_PRODATAEXPIRED As Integer = -9966
    Public Const qaerr_STREETSVERSION As Integer = -9967
    Public Const qaerr_TOOSHORT As Integer = -9968
    Public Const qaerr_PREMISESVERSION As Integer = -9969
    Public Const qaerr_INDEXVERSION As Integer = -9970
    Public Const qaerr_TOOLONG As Integer = -9971
    Public Const qaerr_BADINFO As Integer = -9972
    'Global Const qaerr_NESTEDTOODEEP = -9973
    'Global Const qaerr_BADPHASE = -9974
    Public Const qaerr_STREETSOPEN As Integer = -9975
    Public Const qaerr_PREMISESOPEN As Integer = -9976
    Public Const qaerr_INDEXOPEN As Integer = -9977
    Public Const qaerr_NOMATCH As Integer = -9978
    Public Const qaerr_TOOMANYCODES As Integer = -9979
    'Global Const qaerr_TOOMANYMATCHES = -9980
    'Global Const qaerr_NOSUCHAREA = -9981
    Public Const qaerr_NODP As Integer = -9983
    Public Const qaerr_NONUMBER As Integer = -9984
    Public Const qaerr_CANTSTEP As Integer = -9987
    Public Const qaerr_SYNTAX As Integer = -9988
    'Global Const qaerr_BADFILEFORMAT = -9989
    Public Const qaerr_SURNAMEINDEXOPEN As Integer = -9990
    Public Const qaerr_FORENAMEINDEXOPEN As Integer = -9991
    Public Const qaerr_BADSEARCHDESC As Integer = -10000
    'Global Const qapro_STEPINFO = 0
    'Global Const qapro_RANGEINFO = 1
    'Global Const qapro_TYPEINFO = 2
    'Global Const qapro_DPPINFO = 3
    'Global Const qapro_QUALITYINFO = 4

    Declare Function N_QAPro_Open Lib "QANUIEB.DLL" Alias "QAPro_Open" (ByVal vs1 As String, ByVal vs2 As String) As Integer
    Declare Sub N_QAPro_Close Lib "QANUIEB.DLL" Alias "QAPro_Close" ()
    Declare Sub N_QAPro_SetTimeout Lib "QANUIEB.DLL" Alias "QAPro_SetTimeout" (ByVal vl1 As Integer)
    Declare Function N_QAPro_ChangeFormat Lib "QANUIEB.DLL" Alias "QAPro_ChangeFormat" (ByVal vs1 As String) As Integer
    Declare Function N_QAPro_Search Lib "QANUIEB.DLL" Alias "QAPro_Search Lib" (ByVal vs1 As String) As Integer
    Declare Function N_QAPro_SearchDPP Lib "QANUIEB.DLL" Alias "QAPro_SearchDPP" (ByVal vs1 As String, ByVal vi2 As Integer) As Integer
    Declare Sub N_QAPro_EndSearch Lib "QANUIEB.DLL" Alias "QAPro_EndSearch" ()
    Declare Function N_QAPro_Count Lib "QANUIEB.DLL" Alias "QAPro_Count" () As Integer
    Declare Function N_QAPro_ListItem Lib "QANUIEB.DLL" Alias "QAPro_ListItem" (ByVal vl1 As Integer, ByVal rs2 As String, ByVal vi3 As Integer, ByVal vi4 As Integer) As Integer
    Declare Function N_QAPro_StepIn Lib "QANUIEB.DLL" Alias "QAPro_StepIn" (ByVal vl1 As Integer) As Integer
    Declare Function N_QAPro_StepOut Lib "QANUIEB.DLL" Alias "QAPro_StepOut" () As Integer
    Declare Function N_QAPro_Select Lib "QANUIEB.DLL" Alias "QAPro_Select" (ByVal vs1 As String, ByVal vi2 As Integer, ByVal vi3 As Integer) As Integer
    Declare Function N_QAPro_EndSelect Lib "QANUIEB.DLL" Alias "QAPro_EndSelect" () As Integer
    Declare Function N_QAPro_Pick Lib "QANUIEB.DLL" Alias "QAPro_Pick" (ByVal vs1 As String, ByVal vi2 As Integer) As Integer
    Declare Function N_QAPro_EndPick Lib "QANUIEB.DLL" Alias "QAPro_EndPick" () As Integer
    Declare Function N_QAPro_Back Lib "QANUIEB.DLL" Alias "QAPro_Back" () As Integer
    Declare Function N_QAPro_First Lib "QANUIEB.DLL" Alias "QAPro_First" (ByVal vs1 As String, ByVal vi2 As Integer, ByRef rl3 As Integer) As Integer
    Declare Function N_QAPro_AddrLine Lib "QANUIEB.DLL" Alias "QAPro_AddrLine" (ByVal vl1 As Integer, ByVal vi2 As Integer, ByVal vs3 As String, ByVal rs4 As String, ByVal vi5 As Integer) As Integer
    Declare Function N_QAPro_FormatLine Lib "QANUIEB.DLL" Alias "QAPro_FormatLine" (ByVal vl1 As Integer, ByVal vi2 As Integer, ByVal vs3 As String, ByVal rs4 As String, ByVal vi5 As Integer) As Integer
    Declare Function N_QAPro_FormatAddr Lib "QANUIEB.DLL" Alias "QAPro_FormatAddr" (ByVal vl1 As Integer, ByVal vs2 As String, ByVal rs3 As String, ByVal vi4 As Integer) As Integer
    Declare Function N_QAPro_FormatCount Lib "QANUIEB.DLL" Alias "QAPro_FormatCount" () As Integer
    Declare Function N_QAPro_GetItemInfo Lib "QANUIEB.DLL" Alias "QAPro_GetItemInfo" (ByVal vl1 As Integer, ByVal vi2 As Integer, ByRef rl3 As Integer) As Integer
    Declare Function N_QAPro_GetDPP Lib "QANUIEB.DLL" Alias "QAPro_GetDPP" (ByVal vl1 As Integer, ByVal vs2 As String, ByRef ri3 As Integer) As Integer
    Declare Sub N_QAPro_CCUserUpdate Lib "QANUIEB.DLL" Alias "QAPro_CCUserUpdate" ()
    Declare Function N_QAPro_CCReadCounter Lib "QANUIEB.DLL" Alias "QAPro_CCReadCounter" () As Integer

    Public Const qanamefields_FULLNAME As Integer = 20
    Public Const qanamefields_TITLE As Integer = 21
    Public Const qanamefields_FORENAME As Integer = 22
    Public Const qanamefields_INITIAL As Integer = 23
    Public Const qanamefields_SURNAME As Integer = 24
    Public Const qafreefields_USER1 As Integer = 25
    Public Const qafreefields_USER2 As Integer = 26
    Public Const qafreefields_USER3 As Integer = 27
    Public Const qafreefields_USER4 As Integer = 28
    Public Const qafreefields_USER5 As Integer = 29
    Public Const qafreefields_USER6 As Integer = 30
    Public Const qafreefields_USER7 As Integer = 31
    Public Const qaextrafields_COUNTRY As Integer = 14
    Public Const qaerr_NAMESEXPANDED As Integer = -1800
    Public Const qaerr_INVALIDNAMESSEARCH As Integer = -1801
    Public Const qaerr_NAMESNOTOPEN As Integer = -1802
    Public Const qaerr_NAMESDEPTH As Integer = -1803
    Public Const qaerr_NAMESOPENED As Integer = -1804
    Public Const qaerr_BADNAMEINFO As Integer = -1805
    Public Const qaerr_CANTGOBACK As Integer = -1806
    Public Const qaerr_NONAMES As Integer = -1807
    Public Const qaerr_NOEXTRA As Integer = -1808
    Public Const qaerr_NAMESFAILED As Integer = -1809
    Public Const qaerr_NAMESCORRUPT As Integer = -1810
    Public Const qaerr_NAMESVERSION As Integer = -1811
    Public Const qaerr_NAMESNOTPRESENT As Integer = -1812
    Public Const qaerr_INVALIDNAMESTRING As Integer = -1813
    Public Const qaerr_NAMESINDEXOPEN As Integer = -1814
    Public Const qaerr_NONNAMESINDEXFILE As Integer = -1815
    Public Const qaerr_NAMESINDEXVERSION As Integer = -1816
    Public Const qaerr_NONDATAFILE As Integer = -1817
    Public Const qaerr_NAMESALIASFILEINVALID As Integer = -1818
    Public Const qaerr_NAMESALIASFILEVERSION As Integer = -1819
    Public Const qaerr_INVALIDPROSEARCH As Integer = -1820
    Public Const qanames_DPPINFO As Integer = 0
    Public Const qanames_TYPEINFO As Integer = 1
    Public Const qanames_STEPINFO As Integer = 2
    Public Const qanames_RANGEINFO As Integer = 3
    Public Const qanames_QUALITYINFO As Integer = 4
    Public Const qanames_NAMEINFO As Integer = 5
    Public Const nametype_STREET As Integer = 0
    Public Const nametype_PREMISES As Integer = 1
    Public Const nametype_PREMGROUP As Integer = 2
    Public Const nametype_PREMPLUSSUBPREMS As Integer = 3
    Public Const nametype_NAME As Integer = 4

    Declare Function QANames_Open Lib "QANUIEB.DLL" (ByVal vs1 As String, ByVal vs2 As String) As Integer
    Declare Sub QANames_Close Lib "QANUIEB.DLL" ()
    Declare Sub QANames_SetTimeout Lib "QANUIEB.DLL" (ByVal vl1 As Integer)
    Declare Function QANames_ChangeFormat Lib "QANUIEB.DLL" (ByVal vs1 As String) As Integer
    Declare Function QANames_Search Lib "QANUIEB.DLL" (ByVal vs1 As String) As Integer
    Declare Sub QANames_EndSearch Lib "QANUIEB.DLL" ()
    Declare Function QANames_Count Lib "QANUIEB.DLL" () As Integer
    Declare Function QANames_ListItem Lib "QANUIEB.DLL" (ByVal vl1 As Integer, ByVal rs2 As String, ByVal vi3 As Integer, ByVal vi4 As Integer) As Integer
    Declare Function QANames_StepIn Lib "QANUIEB.DLL" (ByVal vl1 As Integer) As Integer
    Declare Function QANames_Select Lib "QANUIEB.DLL" (ByVal vs1 As String, ByVal vi2 As Integer) As Integer
    Declare Function QANames_Pick Lib "QANUIEB.DLL" (ByVal vs1 As String, ByVal vi2 As Integer) As Integer
    Declare Function QANames_SearchDPP Lib "QANUIEB.DLL" (ByVal vs1 As String, ByVal vi2 As Integer) As Integer
    Declare Function QANames_PickNumber Lib "QANUIEB.DLL" (ByVal vs1 As String, ByVal vl2 As Integer) As Integer
    Declare Function QANames_Back Lib "QANUIEB.DLL" () As Integer
    Declare Function QANames_First Lib "QANUIEB.DLL" (ByVal vs1 As String, ByVal vi2 As Integer, ByRef rl3 As Integer) As Integer
    Declare Function QANames_ExpandNames Lib "QANUIEB.DLL" () As Integer
    Declare Function QANames_AddrLine Lib "QANUIEB.DLL" (ByVal vl1 As Integer, ByVal vi2 As Integer, ByVal rs3 As String, ByVal vi4 As Integer) As Integer
    Declare Function QANames_FormatCount Lib "QANUIEB.DLL" () As Integer
    Declare Function QANames_FormatLine Lib "QANUIEB.DLL" (ByVal vl1 As Integer, ByVal vi2 As Integer, ByVal rs3 As String, ByVal vi4 As Integer) As Integer
    Declare Function QANames_FormatAddr Lib "QANUIEB.DLL" (ByVal vl1 As Integer, ByVal rs2 As String, ByVal vi3 As Integer) As Integer
    Declare Function QANames_GetItemInfo Lib "QANUIEB.DLL" (ByVal vl1 As Integer, ByVal vi2 As Integer, ByRef rl3 As Integer) As Integer
    Declare Function QANames_GetExtraData Lib "QANUIEB.DLL" (ByVal vl1 As Integer, ByVal vs2 As String, ByVal rs3 As String, ByVal vi4 As Integer, ByVal rs5 As String, ByVal vi6 As Integer) As Integer

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
    Public Const COM_ORG_START As Integer = 80000000
    Public Const LARGE_ORG_START As Integer = 90000000
    Public Const SMALL_ORG_START As Integer = 100000000

    Declare Function QAProKey_Open Lib "QANUIEB.DLL" () As Integer
    Declare Function QAProKey_Close Lib "QANUIEB.DLL" () As Integer
    Declare Function QAProKey_Search Lib "QANUIEB.DLL" (ByVal vi1 As Integer, ByVal vs2 As String, ByVal vs3 As String, ByRef ri4 As Integer) As Integer
    Declare Function QAProKey_Get Lib "QANUIEB.DLL" (ByVal vi1 As Integer, ByVal rs2 As String, ByRef ri3 As Integer, ByRef ri4 As Integer, ByVal rs5 As String, ByVal rs6 As String) As Integer

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
    Public Const qaret_SUCCESS As Integer = 1
    Public Const qaret_OVERFLOW As Integer = 2
    Public Const qaret_FIELDTRUNCATED As Integer = 4
    Public Const qaret_POSTCODERECODED As Integer = 8
    Public Const qaret_NUMBEREDFLAT As Integer = 16
    Public Const qaret_SUBSMADE As Integer = 32
    Public Const qaerr_UINOTSTARTED As Integer = -1900
    Public Const qaerr_UISTARTED As Integer = -1901
    Public Const qaerr_NORESULTS As Integer = -1920
    Public Const qaerr_MOREMATCHES As Integer = -1921
    Public Const qaerr_SEARCHRECODED As Integer = -1922
    Public Const qaerr_NOTEXACT As Integer = -1923
    Public Const qaerr_DATALOST As Integer = -1924

    Declare Function QANamesUI_Startup Lib "QANUIEB.DLL" (ByVal vs1 As String, ByVal vs2 As String, ByVal vs3 As String, ByVal vl4 As Integer) As Integer
    Declare Function QANamesUI_DPPopup Lib "QANUIEB.DLL" (ByVal vs1 As String, ByVal rs2 As String, ByVal vi3 As Integer, ByVal vs4 As String, ByRef ri5 As Integer, ByRef ri6 As Integer) As Integer
    Declare Function QANamesUI_Popup Lib "QANUIEB.DLL" (ByVal vs1 As String, ByVal rs2 As String, ByVal vi3 As Integer) As Integer
    Declare Sub QANamesUI_Shutdown Lib "QANUIEB.DLL" (ByVal vi1 As Integer)
    Declare Function QANamesUI_Config Lib "QANUIEB.DLL" (ByVal vs1 As String) As Integer
    Declare Sub QANamesUI_IniSection Lib "QANUIEB.DLL" (ByVal rs1 As String, ByVal vi2 As Integer)

    '**** End of File ****
End Module