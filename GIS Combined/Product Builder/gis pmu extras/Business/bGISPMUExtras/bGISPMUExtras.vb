Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports SSP.Shared
Imports System
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  05/05/1999
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bGISPMUExtras"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Username.
    'developer guide no. 107
    <ThreadStatic()>
    Public g_sUsername As String = ""

    ' Password.
    Public g_sPassword As New StringsHelper.FixedLengthString(30)

    ' User ID
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iUserID As Integer

    ' Calling Application
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_sCallingAppName As String = ""
    ' Source ID
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iSourceID As Integer
    ' Language ID
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iLanguageID As Integer
    ' Currency ID
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iCurrencyID As Integer
    ' LogLevel
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iLogLevel As Integer

    'Positional constants for the array
    Public Const ACHLookupHeaderId As Integer = 0
    Public Const ACHCaptionId As Integer = 1
    Public Const ACHCode As Integer = 2
    Public Const ACHDescription As Integer = 3
    Public Const ACHIsDeleted As Integer = 4
    Public Const ACHEffectiveDate As Integer = 5
    Public Const ACHParentId As Integer = 6

    Public Const ACAddressCnt As Integer = 0
    Public Const ACSourceId As Integer = 1
    Public Const ACAddressId As Integer = 2
    Public Const ACAddress1 As Integer = 3
    Public Const ACAddress2 As Integer = 4
    Public Const ACAddress3 As Integer = 5
    Public Const ACAddress4 As Integer = 6
    Public Const ACPostalCode As Integer = 7
    Public Const ACCountryId As Integer = 8
    Public Const ACCreatedById As Integer = 9
    Public Const ACDateCreated As Integer = 10
    Public Const ACModifiedById As Integer = 11
    Public Const ACLastModified As Integer = 12

    ''position 13-16 are not used but get in select query in SP.
    Public Const kAcAddress5 As Integer = 16
    Public Const kAcAddress6 As Integer = 17
    Public Const kAcAddress7 As Integer = 18
    Public Const kAcAddress8 As Integer = 19
    Public Const kAcAddress9 As Integer = 20
    Public Const kAcAddress10 As Integer = 21
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'RAM20021202 : Added the following GIS DataModel Type constants
    '              Ref : NRMA Project Process No 204
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Const GISDMTypeRisk As Integer = 1
    Public Const GISDMTypeClaim As Integer = 2
    Public Const GISDMTypePolicy As Integer = 3
    Public Const GISDMTypeParty As Integer = 4
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    Public Const ACXMLTAGNAMEWorkCLaimPeril As String = "WORK_CLAIM_PERIL"
    Public Const ACXMLPROPERTYNAMEClaimPerilId As String = "CLAIM_PERIL_ID"
    Public Const ACXMLPROPERTYNAMEComment As String = "COMMENTS"
    Public Const ACXMLPROPERTYNAMEDescription As String = "DESCRIPTION"
    Public Const kSystemOptionAllowNegativeReserve As Integer = 1016
End Module