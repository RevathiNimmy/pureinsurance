Option Strict Off
Option Explicit On
Imports System
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
 Public Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 12/07/2000
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMUProductMaint"


    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons


    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303
    Public Const ACStatusSearching As Integer = 306
    Public Const ACStatusFound As Integer = 307

    ' Menus


    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Constants for the search data array indexes.
    Public Const ACIProductid As Integer = 0
    Public Const ACICaptionId As Integer = 1
    Public Const ACICode As Integer = 2
    Public Const ACIDescription As Integer = 3
    Public Const ACIProductEffectiveDate As Integer = 4
    Public Const ACIIsDeleted As Integer = 5
    Public Const ACISchemeAgencyRef As Integer = 6
    Public Const ACIBlockNo As Integer = 7
    Public Const ACIIsTaxSuppressed As Integer = 8
    Public Const ACIQuoteAutoNumberingID As Integer = 9
    Public Const ACIIsShortPeriodRated As Integer = 10
    Public Const ACIIsMidnightRenewal As Integer = 11
    Public Const ACIIsAutoRenewable As Integer = 12
    Public Const ACIRenewalWeeks As Integer = 13
    Public Const ACIPolicyAutoNumberingID As Integer = 14
    Public Const ACIProvClaimAutoNumberingID As Integer = 15
    Public Const ACIFullClaimAutoNumberingID As Integer = 16
    Public Const ACIAccumulation As Integer = 17
    Public Const ACIRIPointer As Integer = 18
    Public Const ACIReportPointer As Integer = 19

    'TN2001101 (Start) process 29
    Public Const ACIClaimYearToCheck As Integer = 20
    Public Const ACIMaxSingleClaimValue As Integer = 21
    Public Const ACIMaxNumberOfClaim As Integer = 22
    Public Const ACIMaxTotalClaimValue As Integer = 23
    'TN20001101 (End) process 29


    ' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oGIS As Object

    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    Public Const ScreenhelpID As Integer = 4088




    Sub Main_Renamed()

    End Sub
End Module