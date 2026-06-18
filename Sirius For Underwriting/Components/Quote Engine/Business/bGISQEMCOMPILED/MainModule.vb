Option Strict Off
Option Explicit On
Imports System
Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  07/04/1999
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bGISQEMCOMPILED"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' UserID

    ' Polaris Constants

    ' More to do here
    ' Need to get some of these from the registry
    Public Const ACOIMDictionaryPath As String = "Dictionary\"
    Public Const ACOIMMktListFile As String = "MKTLIST.BIN"
    Public Const ACOIMModelFile As String = "MODEL.BIN"
    Public Const ACOIMMsgFile As String = "MODNAMES.BIN"

    Public Const ACOIMAppRuleFile As String = "Aprulcl.bin"
    Public Const ACOIMAppRateFile As String = "Apprate.bin"
    Public Const ACOIMAppEDIDefFile As String = "EDIDEF.BIN"

    Public Const ACOIMSchemesPath As String = "Schemes\"

    Public Const ACOIMSchemeRuleFile As String = "Rulecalc.bin"
    Public Const ACOIMSchemeRateFile As String = "Schrate.bin"

    Public Const ACOIMSchemeExt As String = ".sch"
    Public Const ACOIMSchemeRulExt As String = ".rul"
    Public Const ACOIMSchemeEDIExt As String = ".edi"
    Public Const ACOIMSchemeFormExt As String = ".frm"

    Public Const ACOIMSchemeAppVer As Integer = 4010
    Public Const ACOIMSchemeInsurerId As Integer = 3
    Public Const ACOIMSchemeId As Integer = 1
    Public Const ACOIMSchemeVer As Integer = 1

    ' Being this constant is available in GIS Shared Constant
    ' Note : This constant should not be in GISSharedConstants.bas
    '        This should be declared over here only
    'Public Const ACOIMGISSubKey As String = "GIS" ' CL120899

    Public Const ACOIMEDIMsgID As Integer = 46
    Public Const ACPolExecAppRules As Integer = 57
    Public Const ACPolExecInitRules As Integer = 50
    Public Const ACPolExecMainRules As Integer = 51
    Public Const ACPolExecProRata As Integer = 41
    Public Const ACPolExecCancellation As Integer = 25
    Public Const ACPolExecNewBusOnly As Integer = 16 ' CL160799

    ' Polaris Rules Calc return codes
    Public Const ACPolRulesNormalEnd As Integer = 1
    Public Const ACPolRulesNoRules As Integer = 840

    ' Have to do this for the moment until Polaris & Perwill sort themselves out.
    Public Const ACWrongHeader As String = "EDI-SEND DATA01POLA010301'"
    Public Const ACWrongTrailer As String = "9000+POLA+0103+01+'"
    Public Const ACWrongHeader2 As String = "EDI-SEND DATA01POLA010401'"
    Public Const ACWrongTrailer2 As String = "9000+POLA+0104+01+'"
    Public Const ACReplaceHeader As String = "EDI-SEND DATA01POLA010701'"
    Public Const ACReplaceTrailer As String = "9000+POLA+0107+01+'"

    ' QuoteType Constants
    Public Const ACQuoteTypeQuotesOnly As Integer = 1
    Public Const ACQuoteTypeQuotesDetail As Integer = 2
    Public Const ACQuoteTypeQuotesFull As Integer = 3
    Public Const ACQuoteTypePostQuoteForm As Integer = 4
    Public Const ACQuoteTypePostQuoteEDI As Integer = 5

    Public Const ACGetGISInsurerID As Boolean = True
    Public Const ACGetGISInsurerIDName As String = "GetGISInsurerID"
    Public Const ACGetGISInsurerIDSQL As String = "spu_Get_GIS_Insurer_ID"
End Module