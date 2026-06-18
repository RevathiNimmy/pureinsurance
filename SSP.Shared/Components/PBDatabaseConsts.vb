Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("PBDatabaseConsts_NET.PBDatabaseConsts")>
Public Module PBDatabaseConsts
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    'Array constants for the Screen Details
    'spu_GIS_Screen_detail_saa
    Public Const ACDGISScreenId As Integer = 0
    Public Const ACDScreenDetailCnt As Integer = 1
    Public Const ACDGISObjectId As Integer = 2
    Public Const ACDGISPropertyId As Integer = 3
    Public Const ACDIsFrame As Integer = 4
    Public Const ACDTabNumber As Integer = 5
    Public Const ACDCaption As Integer = 6
    Public Const ACDTop As Integer = 7
    Public Const ACDLeft As Integer = 8
    Public Const ACDHeight As Integer = 9
    Public Const ACDWidth As Integer = 10
    Public Const ACDColumnWidth As Integer = 11
    Public Const ACDPreQuoteRequirement As Integer = 12
    Public Const ACDPostQuoteRequirement As Integer = 13
    Public Const ACDPurchaseRequirement As Integer = 14
    Public Const ACDParentId As Integer = 15
    Public Const ACDHelpText As Integer = 16
    Public Const ACDDefaultObjectId As Integer = 17
    Public Const ACDDefaultPropertyId As Integer = 18
    Public Const ACDIsValuation As Integer = 19
    Public Const ACDIsRateAndPremium As Integer = 20
    Public Const ACDChildScreenId As Integer = 21
    Public Const ACDDataModelType As Integer = 22
    Public Const ACDPMFormat As Integer = 23
    Public Const ACDColumnPosition As Integer = 24
    Public Const ACDTabSetIndex As Integer = 25
    Public Const ACDLastArrayPosition As Integer = 25

    'Array constants for the extra Screen Details
    'spu_GIS_Screen_detail_extra_saa
    'all values below 26 are the same as ACD's
    Public Const ACDExtraGISObjectName As Integer = 26
    Public Const ACDExtraGISTableName As Integer = 27
    Public Const ACDExtraGISPropertyName As Integer = 28
    Public Const ACDExtraGISColumnName As Integer = 29
    Public Const ACDExtraEditFlags As Integer = 30
    Public Const ACDExtraSpecialsType As Integer = 31
    Public Const ACDExtraSpecialsTypeReference As Integer = 32
    Public Const ACDExtraParentObjectName As Integer = 33
    Public Const ACDExtraObjectType As Integer = 34
    Public Const ACDExtraIsFormattedText As Integer = 35

    'Array constants for the Screen Header
    Public Const ACHScreenId As Integer = 0
    Public Const ACHCaptionId As Integer = 1
    Public Const ACHCode As Integer = 2
    Public Const ACHDescription As Integer = 3
    Public Const ACHIsDeleted As Integer = 4
    Public Const ACHEffectiveDate As Integer = 5
    Public Const ACHParentId As Integer = 6
    Public Const ACHIsMaintainable As Integer = 7
    Public Const ACHGISDataModelId As Integer = 8
    Public Const ACHScriptDefaults As Integer = 9
    Public Const ACHScriptDynamicLogic As Integer = 10
    Public Const ACHScreenType As Integer = 11 'object type from GISDataModelType.bas
    'Tomo151002
    Public Const ACHScreenHeight As Integer = 12
    Public Const ACHScreenWidth As Integer = 13
    Public Const ACHEnableCompiledRule As Integer = 14
    Public Const ACHCompiledRuleAssemblyDefaults As Integer = 15
    Public Const ACHCompiledRuleAssemblyValidation As Integer = 16
    Public Const ACHLastArrayPosition As Integer = 16


    ' Select Screen Header SQL
    Public Const ACGetAllScreenHeaderStored As Boolean = True
    Public Const ACGetAllScreenHeaderName As String = "SelectAllScreenHeader"
    'Modified by Alkesh Kumar on 10/05/2010 10:45:16 refer developer guide no.39 
    Public Const ACGetAllScreenHeaderSQL As String = "spe_GIS_screen_sel"
End Module
