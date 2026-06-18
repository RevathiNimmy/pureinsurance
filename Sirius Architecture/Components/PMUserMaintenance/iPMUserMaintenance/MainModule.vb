Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System

Imports SharedFiles
Public Module MainModule

    Public Const ACApp As String = "UserMaintenance"

    ' RDC 13062002 gPMLibraries replaced with gPM* BAS modules
    'Private m_lReturn As gPMLibraries.PMEReturnCode
    Private m_lReturn As gPMConstants.PMEReturnCode

    Public Const USRAddUser As Integer = 0
    Public Const USREditUser As Integer = 1

    ' Username.

    <ThreadStatic()> _
    Public g_sUsername As String = ""

    ' Password.
    Public g_sPassword As New FixedLengthString(30)

    ' Calling Application

    <ThreadStatic()> _
    Public g_sCallingAppName As String = ""
    ' Source ID

    <ThreadStatic()> _
    Public g_iSourceID As Integer
    ' Language ID

    <ThreadStatic()> _
    Public g_iLanguageID As Integer
    ' Currency ID

    <ThreadStatic()> _
    Public g_iCurrencyID As Integer
    ' LogLevel

    <ThreadStatic()> _
    Public g_iLogLevel As Integer
    ' UserID

    <ThreadStatic()> _
    Public g_iUserID As Integer


    <ThreadStatic()> _
    Public g_bReminded As Boolean

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons

    ' Form

    ' Buttons

    ' Messages

    ' Menus

    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    'Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling - Changed from 12 to 13
    Public Const kParamsCount As Integer = 28
    ' Constant for the maxiumum search details.
    Public Const ACMaxSearchDetails As Integer = 500

    ' Constant for the miniumum search length.
    Public Const ACMinSearchLength As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public instance of the object manager.

    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager

    ' Public instance of the business object.

    <ThreadStatic()> _
    Public g_oBusiness As Object


    <ThreadStatic()> _
    Public g_oAuthority As bACTUserAuthorities.Business

    <ThreadStatic()> _
    Public g_iSystemSecurityModel As Integer

    <ThreadStatic()> _
    Public g_bValidDomainAccount As Boolean

    Public Const ACIsRecommenderArrPos As Integer = 0
    Public Const ACRecommendationCurrencyArrPos As Integer = 1
    Public Const ACRecommendationAmountArrPos As Integer = 2

    'Payment Maintenance
    Public Const ACCanReverseAllocationArrPos As Integer = 3
    Public Const ACTimePeriodForReversalArrPos As Integer = 4

    'Electra M3 Gaps Find Transaction Changes
    Public Const ACCanReverseReplaceTransactionsArrPos As Integer = 5
    Public Const ACMTAAuthorityArrPos As Integer = 6
    Public Const ACChequeNumberArrPos As Integer = 7

    'Start(Saurabh Agrawal)- Tech Spec WR3 User Level RI Display Restriction(5.1.1)
    Public Const ACDisplayReinsuranceScreen As Integer = 8
    Public Const ACDisplayClaimReinsurance As Integer = 9
    'End(Saurabh Agrawal)- Tech Spec WR3 User Level RI Display Restriction(5.1.1)

    'Start(Gaurav Arora)- Tech Spec WR6 Bank Guarantee
    Public Const ACMakeLiveBankGuarantee As Integer = 10
    'End(Gaurav Arora)- Tech Spec WR6 Bank Guarantee

    Public Const ACCanBackdateCollectionDate As Integer = 11
    Public Const ACEditDefaultCommission As Integer = 12
    Public Const ACMakeLiveCashDeposit As Integer = 13 'Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
    Public Const ACUserCanDebugDynamicLogicScripts As Integer = 14
    Public Const ACUserServerScriptsRunInDebug As Integer = 15
    Public Const kEditDefaultCommissionNBRN As Integer = 16
    Public Const kEditDefaultCommissionMTA As Integer = 17
    Public Const kEditDefaultCommissionMTR As Integer = 18
    Public Const kEditDefaultCommissionMTC As Integer = 19
    Public Const kEditAgentDuringMTAMTC As Integer = 20
    Public Const ACUserCanChangeInstalmentDefaultCurrency As Integer = 23 'WPR IH 10

    Public Const ACInstalmentStatus As Integer = 24
    'DC150803 -PS254 -fsa compliance
    Public Const ACCanEditInstalmentDueDate As Integer = 25
    Public Const ACEditInstalmentDateByNoOfDays As Integer = 26

    Public Const ACCanReverseReceiptArrPos As Integer = 21
    Public Const kACCViewBatchProcessStatus As Integer = 22
    Public Const kACCCanExtractClientData As Integer = 27
    Public Function GetHiddenOption(ByVal v_lSourceId As Integer, Optional ByRef r_vEnableFSACompliance As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer


        Dim vValue As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Link Account Executives To Commission

            If Not Information.IsNothing(r_vEnableFSACompliance) Then
                lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableFSACompliance, v_vBranch:=v_lSourceId, r_vUnderwriting:=vValue)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTLinkCommACCToACCExec, vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOption")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                If Convert.ToString(vValue) <> "" Then
                    r_vEnableFSACompliance = (CBool(vValue))
                Else
                    r_vEnableFSACompliance = False
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetHiddenOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Sub Main()
        Dim Procesos() As Process = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName)
        Dim iInstances As Integer = 0
        If Procesos.Length > 1 Then
            For index As Integer = 0 To Procesos.Length - 1
                If Procesos(index).SessionId = Process.GetCurrentProcess.SessionId Then
                    iInstances += 1
                End If
            Next

            If iInstances > 1 Then
                MessageBox.Show("User Maintenance is already running. Only one instance may be run at a time.", "User Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
        End If
        Dim objInterface As New Interface_Renamed
        Dim m_lreturn As Integer = objInterface.Initialise()
        If m_lreturn = gPMConstants.PMEReturnCode.PMTrue Then
            objInterface.Start()
            objInterface.Dispose()
        End If
    End Sub
End Module