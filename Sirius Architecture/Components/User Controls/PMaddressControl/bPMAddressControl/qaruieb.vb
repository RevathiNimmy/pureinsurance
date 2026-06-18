Option Strict Off
Option Explicit On
Module QASRapidConst
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    '**** Visual Basic (32 bit) header file for QARUIEB.DLL v3.15(68) ****


    ' RDC 24082000
    ' Constants and function/sub declarations for QAS Rapid
    ' Duplicate constants deleted and aliases included, allowing module to co-exist with QASConst (QASPro)


    Public Const qaerr_CCNODONGLE As Integer = -1053
    Public Const qaerr_CCNOUNITS As Integer = -1054
    Public Const qaerr_CCNOMETER As Integer = -1055

    Declare Sub R_QAInitialise Lib "QARUIEB.DLL" Alias "QAInitialise" (ByVal vi1 As Integer)
    Declare Sub R_QAErrorMessage Lib "QARUIEB.DLL" Alias "QAErrorMessage" (ByVal vi1 As Integer, ByVal rs2 As String, ByVal vi3 As Integer)
    Declare Sub R_QAVersionInfo Lib "QARUIEB.DLL" Alias "QAVersionInfo" (ByVal rs1 As String, ByVal vi2 As Integer)
    Declare Sub R_QAUpdateKey Lib "QARUIEB.DLL" Alias "QAUpdateKey" (ByVal rs1 As String, ByVal vi2 As Integer)
    Declare Function R_QAUpdateCode Lib "QARUIEB.DLL" Alias "QAUpdateCode" (ByVal vs1 As String) As Integer
    Declare Function R_QALicenseInfo Lib "QARUIEB.DLL" Alias "QALicenseInfo" (ByRef ri1 As Integer, ByRef ri2 As Integer, ByRef ri3 As Integer) As Integer
    Declare Function R_QAAuthorise Lib "QARUIEB.DLL" Alias "QAAuthorise" (ByVal vs1 As String, ByVal vl2 As Integer) As Integer
    Declare Function R_QAErrorLevel Lib "QARUIEB.DLL" Alias "QAErrorLevel" (ByVal vi1 As Integer) As Integer
    Declare Function R_QAErrorHistory Lib "QARUIEB.DLL" Alias "QAErrorHistory" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByVal rs3 As String, ByVal vi4 As Integer) As Integer
    Declare Function R_QADataInfo Lib "QARUIEB.DLL" Alias "QADataInfo" (ByVal rs1 As String, ByVal vi2 As Integer, ByRef ri3 As Integer) As Integer
    Declare Function R_QASystemInfo Lib "QARUIEB.DLL" Alias "QASystemInfo" (ByVal vi1 As Integer, ByVal rs2 As String, ByVal vi3 As Integer) As Integer

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

    Declare Function QADP_Startup Lib "QARUIEB.DLL" (ByVal vs1 As String, ByVal vs2 As String) As Integer
    Declare Function QADP_Shutdown Lib "QARUIEB.DLL" () As Integer
    Declare Function QADP_Open Lib "QARUIEB.DLL" (ByVal vs1 As String, ByVal vs2 As String, ByVal vs3 As String, ByVal vi4 As Integer) As Integer
    Declare Function QADP_Close Lib "QARUIEB.DLL" (ByVal vi1 As Integer) As Integer
    Declare Function QADP_DataSpec Lib "QARUIEB.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByVal rs3 As String, ByVal vi4 As Integer, ByVal rs5 As String, ByVal vi6 As Integer) As Integer
    Declare Function QADP_ItemCount Lib "QARUIEB.DLL" (ByVal vi1 As Integer) As Integer
    Declare Function QADP_ID Lib "QARUIEB.DLL" (ByVal vs1 As String) As Integer
    Declare Function QADP_Name Lib "QARUIEB.DLL" (ByVal vi1 As Integer, ByVal rs2 As String, ByVal vi3 As Integer) As Integer
    Declare Function QADP_ItemName Lib "QARUIEB.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByVal rs3 As String, ByVal vi4 As Integer) As Integer
    Declare Function QADP_MaxID Lib "QARUIEB.DLL" () As Integer
    Declare Function QADP_OpenCount Lib "QARUIEB.DLL" () As Integer
    Declare Function QADP_LookUp Lib "QARUIEB.DLL" (ByVal vs1 As String, ByVal vi2 As Integer) As Integer
    Declare Function QADP_CodeCount Lib "QARUIEB.DLL" (ByVal vi1 As Integer) As Integer
    Declare Function QADP_Get Lib "QARUIEB.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByVal rs3 As String, ByVal vi4 As Integer, ByVal rs5 As String, ByVal vi6 As Integer) As Integer
    Declare Function QADP_GetItem Lib "QARUIEB.DLL" (ByVal vi1 As Integer, ByVal vi2 As Integer, ByVal vi3 As Integer, ByVal rs4 As String, ByVal vi5 As Integer, ByVal rs6 As String, ByVal vi7 As Integer) As Integer
    Declare Function QADP_GetItemByName Lib "QARUIEB.DLL" (ByVal vs1 As String, ByVal vi2 As Integer, ByVal vs3 As String, ByVal rs4 As String, ByVal vi5 As Integer) As Integer
    Declare Function QADP_GetDPP Lib "QARUIEB.DLL" (ByVal vs1 As String) As Integer
    Declare Function QADP_Status Lib "QARUIEB.DLL" (ByVal vi1 As Integer) As Integer
    Declare Function QADP_State Lib "QARUIEB.DLL" (ByVal vi1 As Integer) As Integer
    Declare Function QADP_MatchLevel Lib "QARUIEB.DLL" (ByVal vi1 As Integer) As Integer
    Declare Function QADP_Release Lib "QARUIEB.DLL" (ByVal vi1 As Integer) As Integer

    Public Const qaerr_TIMEDOUT As Integer = -10001
    Public Const qaopevent_START As Integer = 0
    Public Const qaopevent_PROGRESSINFO As Integer = 1
    Public Const qaopevent_POLLCANCEL As Integer = 2
    Public Const qaopevent_STOP As Integer = 3
    Public Const qaerr_RAPIDOPEN As Integer = -9800
    Public Const qaerr_NONRAPIDFILE As Integer = -9801
    Public Const qaerr_INVALIDAREA As Integer = -9802
    Public Const qaerr_AREALEVEL As Integer = -9803
    Public Const qaerr_DISTRICTLEVEL As Integer = -9804
    Public Const qaerr_SECTORLEVEL As Integer = -9805
    Public Const qaerr_HALFSECTORLEVEL As Integer = -9806
    Public Const qaerr_NOCODES As Integer = -9807
    Public Const qaerr_NOAREADATA As Integer = -9809
    Public Const qaerr_NUMBEREDFLAT As Integer = -9810
    Public Const qaerr_POSTCODERECODED As Integer = -9811
    Public Const qaerr_SUBSMADE As Integer = -9813
    Public Const qaerr_BADPOSTCODECHAR As Integer = -9814
    Public Const qaerr_RAPIDNOTSTARTED As Integer = -9815
    Public Const rawaddress_DEPTHORO As Integer = 5
    Public Const rawaddress_THORO As Integer = 6
    Public Const rawaddress_DDEPLOCAL As Integer = 7
    Public Const rawaddress_DEPLOCAL As Integer = 8
    Public Const rawaddress_POSTTOWN As Integer = 9
    Public Const rawaddress_COUNTY As Integer = 10
    Public Const rawaddress_POSTCODE As Integer = 11

    Declare Sub QARapid_EndSearch Lib "QARUIEB.DLL" ()
    Declare Sub QARapid_Close Lib "QARUIEB.DLL" ()
    Declare Function QARapid_Open Lib "QARUIEB.DLL" (ByVal vs1 As String, ByVal vs2 As String) As Integer
    Declare Function QARapid_ChangeFormat Lib "QARUIEB.DLL" (ByVal vs1 As String) As Integer
    Declare Function QARapid_Search Lib "QARUIEB.DLL" (ByVal vs1 As String) As Integer
    Declare Function QARapid_Count Lib "QARUIEB.DLL" () As Integer
    Declare Function QARapid_ListItem Lib "QARUIEB.DLL" (ByVal vl1 As Integer, ByVal rs2 As String, ByVal vi3 As Integer, ByVal vi4 As Integer) As Integer
    Declare Function QARapid_AddrLine Lib "QARUIEB.DLL" (ByVal vl1 As Integer, ByVal vi2 As Integer, ByVal rs3 As String, ByVal vi4 As Integer) As Integer
    Declare Function QARapid_FormatLine Lib "QARUIEB.DLL" (ByVal vl1 As Integer, ByVal vi2 As Integer, ByVal rs3 As String, ByVal vi4 As Integer) As Integer
    Declare Function QARapid_FormatAddr Lib "QARUIEB.DLL" (ByVal vl1 As Integer, ByVal rs2 As String, ByVal vi3 As Integer) As Integer

    Public Const qaret_SUCCESS As Integer = 1
    Public Const qaret_OVERFLOW As Integer = 2
    Public Const qaret_FIELDTRUNCATED As Integer = 4
    Public Const qaret_POSTCODERECODED As Integer = 8
    Public Const qaret_NUMBEREDFLAT As Integer = 16
    Public Const qaret_SUBSMADE As Integer = 32

    Declare Sub QARapidUI_Shutdown Lib "QARUIEB.DLL" (ByVal vi1 As Integer)
    Declare Sub QARapidUI_IniSection Lib "QARUIEB.DLL" (ByVal rs1 As String, ByVal vi2 As Integer)
    Declare Function QARapidUI_Startup Lib "QARUIEB.DLL" (ByVal vs1 As String, ByVal vs2 As String, ByVal vs3 As String, ByVal vl4 As Integer) As Integer
    Declare Function QARapidUI_DPPopup Lib "QARUIEB.DLL" (ByVal vs1 As String, ByVal rs2 As String, ByVal vi3 As Integer, ByVal vs4 As String) As Integer
    Declare Function QARapidUI_Popup Lib "QARUIEB.DLL" (ByVal vs1 As String, ByVal rs2 As String, ByVal vi3 As Integer) As Integer
    Declare Function QARapidUI_Config Lib "QARUIEB.DLL" (ByVal vs1 As String) As Integer

    '**** End of File ****
End Module