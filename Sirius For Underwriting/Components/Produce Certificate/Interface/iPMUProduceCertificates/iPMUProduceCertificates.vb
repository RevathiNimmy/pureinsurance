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
    ' Date: 02/07/1998
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMUProduceCertificates"
    Public Const PMKeyNameNavigatorTitle1 As String = ""

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.

    ' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer

    ' Public instance of the object manager.

    'Uncomment
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public oPaymentList As Object
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions


    'Poilcy List Constants
    Public Const ACPColumnPolicyNumber As Integer = 0
    Public Const ACPColumnPolicyType As Integer = 1
    Public Const ACPColumnProductName As Integer = 2
    Public Const ACPColumnRegarding As Integer = 3
    Public Const ACPColumnRenewalDate As Integer = 4
    Public Const ACPColumnAgent As Integer = 5
    Public Const ACPColumnPremium As Integer = 6
    Public Const ACPColumnPolicyStatus As Integer = 7
    Public Const ACPColumnRiskTypeDescription As Integer = 8
    Public Const ACPColumnInsuranceFileCnt As Integer = 9
    Public Const ACPColumnMax As Integer = ACPColumnInsuranceFileCnt

    'PolIcy Array Constants
    Public Const ACPPolicyNumber As Integer = 3
    Public Const ACPPolicyType As Integer = 26
    Public Const ACPProductName As Integer = 15
    Public Const ACPRegarding As Integer = 3 ' still to set
    Public Const ACPRenewalDate As Integer = 21
    Public Const ACPAgent As Integer = 25
    Public Const ACPPremium As Integer = 22
    Public Const ACPPolicyStatus As Integer = 24
    Public Const ACPRiskTypeDescription As Integer = 18 ' still to set
    Public Const ACPInsuranceType As Integer = 27
    Public Const ACPInsuranceFileCnt As Integer = 2

    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303
End Module