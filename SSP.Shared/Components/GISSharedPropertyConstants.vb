Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("GISSharedPropertyConstants_NET.GISSharedPropertyConstants")>
Public Module GISSharedPropertyConstants
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Module Name: GIS Property Constants
    '
    ' Date:  02/07/02
    '
    ' Description: Deleted fields constants for GIS_property table
    '
    ' Edit History: Created by GSD 02/07/02
    ' ***************************************************************** '


    'Array constants for the new look GIS_Property table
    Public Const ACOSpecialNone As Integer = 0
    Public Const ACOGISListID As Integer = 1
    Public Const ACOPMLookupTableName As Integer = 2
    Public Const ACOPartyTypeID As Integer = 3
    Public Const ACOSumInsuredTypeID As Integer = 4
    Public Const ACOStdWordingType As Integer = 5
    Public Const ACOGISUserDefHeaderID As Integer = 6
    Public Const ACOProductID As Integer = 7
    Public Const ACOReserveID As Integer = 8
    Public Const ACOPaymentID As Integer = 9
    Public Const ACOSwiftCommonCode As Integer = 10
    Public Const ACOSwiftClientSelector As Integer = 11
    Public Const ACOSwiftAddressSelector As Integer = 12
    Public Const ACOSwiftListView As Integer = 13
    Public Const ACOSwiftSpecialType As Integer = 14
    Public Const ACODocuMaster As Integer = 15
    Public Const ACOSwiftNotes As Integer = 16
    Public Const ACOSwiftAddress As Integer = 17
    Public Const ACOComboLookup As Integer = 18
    Public Const ACOCaseHeader As Integer = 19
    Public Const ACOCaseClaimList As Integer = 20
    Public Const ACOSpecialLastIndex As Integer = ACOComboLookup

    'Bit flags for Swift modes
    Public Const SwiftMode_DisplayBasicTypes As Integer = 1
    Public Const SwiftMode_DisplaySpecialsList As Integer = 2
    Public Const SwiftMode_UserDefinesOnAllObjects As Integer = 4
    Public Const SwiftMode_NotRenderedByPB As Integer = 8

    'Public Const ACOPeril = 10
    'Public Const ACOAssociatedClients = 11
    'Public Const ACOSpecialLastIndex = 11


    '------------------------------------------------------------------------------
    '   11/07/2002 RVH  BEGIN
    '                   New constant declarations added
    '------------------------------------------------------------------------------
    'Constants for edit flags (these can be at GIS_Object or GIS_Property level)
    Public Const GISDSEditNone As Integer = 0 'no limitations
    Public Const GISDSEditMandatory As Integer = 1
    Public Const GISDSEditReadOnly As Integer = 2 'not editable in the data model editor
    Public Const GISDSEditNoDBColumn As Integer = 4 'no DB column is created for this


    '------------------------------------------------------------------------------
    '   11/07/2002 RVH  END
    '------------------------------------------------------------------------------

    'Property name of special sequence field
    Public Const cProperty_SequenceId As String = "sequence_id"
End Module