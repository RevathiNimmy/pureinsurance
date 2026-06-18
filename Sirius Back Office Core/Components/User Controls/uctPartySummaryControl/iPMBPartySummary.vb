Option Strict Off
Option Explicit On
Imports System
Imports SharedFiles
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 23/06/1998
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "uctPartySummaryControl"
    Public Const ACStatusActWebLoading As String = "Loading..........."
    Public Const AddressImage As String = "AddressImage"
    Public Const ContactImage As String = "ContactImage"
    Public Const LifestyleImage As String = "LifestyleImage"
    Public Const ConvictionImage As String = "ConvictionImage"
    Public Const CampaignImage As String = "CampaignImage"
    Public Const PolicyImage As String = "PolicyImage"
    Public Const CreditCard As String = "Credit Card"
    Public Const DebitCard As String = "Debit Card"

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons

    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101

    Public Const ACReference As Integer = 102
    Public Const ACName As Integer = 103
    Public Const ACAddress As Integer = 104
    Public Const ACAgent As Integer = 105
    Public Const ACConsultant As Integer = 106
    Public Const ACBranch As Integer = 107
    Public Const ACHomePhone As Integer = 108
    Public Const ACHomeFax As Integer = 109
    Public Const ACWorkPhone As Integer = 110
    Public Const ACWorkFax As Integer = 111
    Public Const ACMobilePhone As Integer = 112
    Public Const ACEmailAddress As Integer = 113
    Public Const ACWebAddress As Integer = 114
    Public Const ACClientContactDetails As Integer = 115

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303
    'SD 17/07/2002
    Public Const ACServicesFailTitle As Integer = 310
    Public Const ACServicesFail As Integer = 311

    ' Menus


    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iCurrencyId As Integer
    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    Public Const ScreenHelpID As Integer = 2


    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    'Constant for the system option number
    Public Const ACSwiftInstalledOption As Integer = 14

    Sub Main_Renamed()

    End Sub
End Module