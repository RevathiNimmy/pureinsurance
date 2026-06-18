Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'Developer Guide No.129
Imports SharedFiles
Module MainModule

    Public Const ACApp As String = "GroupMaintenance"

    ' RDC 13062002 gPMLibraries replaced with gPM* BAS modules
    'Private m_lReturn As gPMLibraries.PMEReturnCode
    Private m_lReturn As gPMConstants.PMEReturnCode

    Public Const USRAddGroup As Integer = 0
    Public Const USREditGroup As Integer = 1

    Public Const ACCancelDetailsTitleText As String = "Cancel Details"
    Public Const ACCancelDetailsText As String = "Cancelling will lose any changes" & Strings.Chr(13) & Strings.Chr(10) & "Do you want to cancel?"

    Public Const ACSaveDetailsTitleText As String = "Save Details"
    Public Const ACSaveDetailsText As String = "Details have changed" & Strings.Chr(13) & Strings.Chr(10) & "Do you want to save?"

    Public Const ACBusinessFailTitleText As String = "Business Object"
    Public Const ACBusinessFailText As String = "Unable to gain access to the business object" & Strings.Chr(13) & Strings.Chr(10) & "Please try later"

    ' Username.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sUsername As String = ""

    ' Password.
    Public g_sPassword As New FixedLengthString(30)

    ' Calling Application
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sCallingAppName As String = ""
    ' Source ID
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    ' Language ID
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    ' Currency ID
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
    ' LogLevel
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLogLevel As Integer
    ' UserID
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons

    ' Form

    ' Buttons

    ' Messages

    ' Menus

    ' Constants for the search data array indexes.
    Public Const ACIPartyCnt As Integer = 0
    Public Const ACIPartyType As Integer = 1
    Public Const ACIShortName As Integer = 2
    Public Const ACILongName As Integer = 3
    Public Const ACIAddress1 As Integer = 4
    Public Const ACIPostalCode As Integer = 5
    Public Const ACISourceID As Integer = 6
    Public Const ACIPartyID As Integer = 7
    Public Const ACITelAreaCode As Integer = 8
    Public Const ACITelNumber As Integer = 9
    Public Const ACIPartyStatus As Integer = 10
    'sj 3/11/99 - start
    Public Const ACIInvariantKey As Integer = 11
    Public Const ACISource As Integer = 12

    Public Const ACIResolvedName As Integer = 13 'CT 19/07/00
    Public Const ACIAgentType As Integer = 14 'CT 10/08/00

    ' CTAF 190900 - Constant for File Code
    Public Const ACIFileCode As Integer = 15

    ' CTAF 260900
    Public Const ACIDOB As Integer = 16
    Public Const ACISwiftPartyID As Integer = 17

    'sj 14/06/2002 - start
    Public Const ACIAddress2 As Integer = 18
    'Public Const ACIMax = 18
    'sj 14/06/2002 - end

    'sj 22/08/2002 - start
    Public Const ACISourceName As Integer = 19
    Public Const ACIAgentCnt As Integer = 20 'PN13921
    'Public Const ACIMax = 19
    'sj 22/08/2002 - end

    'sj 12/11/2002 - start
    'ISS1271
    'JAS(CMG) 03/09/02 - start
    'Public Const ACIrecord_status = 20
    ''Public Const ACIMax = 20
    ''JAS(CMG) 03/09/02 - end
    ''sj 23/09/2002 - start
    'Public Const ACIPartyTypeCode = 21
    'Public Const ACIMax = 21
    ''sj 23/09/2002 - end

    Public Const ACIrecord_status As Integer = 21
    Public Const ACIPartyTypeCode As Integer = 22
    Public Const ACIPartyDateCancelled As Integer = 23
    Public Const ACIOnlineAccess As Integer = 24
    Public Const ACIMax As Integer = 24

    'Constants for List View Column Headers
    Public Const g_kLvwColumnClientCode As Integer = 1
    Public Const g_kLvwColumnName As Integer = 2
    Public Const g_kLvwColumnAddressLine1 As Integer = 3
    Public Const g_kLvwColumnAddressLine2 As Integer = 4
    Public Const g_kLvwColumnPostCode As Integer = 5

    ' Constants for ChangeArray
    Public Const ACICAPartyCnt As Byte = 0
    Public Const ACICAPartyShortName As Byte = 1
    Public Const ACICAAccessStatus As Byte = 2
    Public Const ACICAFailureReason As Byte = 3
    Public Const ACICAMax As Byte = 3

    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the maxiumum search details.
    Public Const ACMaxSearchDetails As Integer = 500

    ' Constant for the miniumum search length.
    Public Const ACMinSearchLength As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
End Module