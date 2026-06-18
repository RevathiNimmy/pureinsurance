Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'Developer Guide No. 129
Imports SharedFiles
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 01/08/2002
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History: AMB - 01/08/2002 - Created
    ' ***************************************************************** '


    Public Const ScreenHelpID As Integer = 44000
    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "uctPMUPolicyExplorerControl"
    
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101
    Public Const ACTabTitle2 As Integer = 102
    Public Const ACInceptionDate As Integer = 308
    Public Const ACClientCode As Integer = 103
    Public Const ACStatus As Integer = 104

    Public Const ACListTitlePolicyNumber As Integer = 113
    Public Const ACListTitlePolicyType As Integer = 114
    Public Const ACListTitleRiskType As Integer = 115
    Public Const ACListTitleRegarding As Integer = 116
    Public Const ACListTitleRenewalDate As Integer = 117
    Public Const ACListTitleInsurer As Integer = 118
    Public Const ACListTitlePremium As Integer = 119
    Public Const ACListTitlePolicyStatus As Integer = 120

    Public Const ACListTitleStartDate As Integer = 121
    Public Const ACListTitleEDI As Integer = 122

    Public Const ACListTitleAgent As Integer = 123
    Public Const ACListTitleRiskTypeDescription As Integer = 124

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACNewButton As Integer = 204
    Public Const ACEditButton As Integer = 205
    Public Const ACFindNowButton As Integer = 206
    Public Const ACNewSearchButton As Integer = 207

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303

    Public Const ACClearDetailsTitle As Integer = 304
    Public Const ACClearDetails As Integer = 305
    Public Const ACStatusSearching As Integer = 306
    Public Const ACStatusFound As Integer = 307

    'SB 31/03/98 defect 37
    Public Const ACLookupFailTitle As Integer = 308
    Public Const ACLookupFail As Integer = 309

    ' Menus

    ' Constants for policy search data array indexes.
    Public Const ACIInsFileId As Integer = 0
    Public Const ACIInsFileSourceId As Integer = 1
    Public Const ACIInsFileCnt As Integer = 2
    Public Const ACIInsReference As Integer = 3
    Public Const ACIInsFolderName As Integer = 4
    Public Const ACIInsFileType As Integer = 5
    Public Const ACIInsuredLongName As Integer = 6
    Public Const ACIInsuredShortName As Integer = 7
    Public Const ACIInsuredId As Integer = 8
    Public Const ACIInsuredSourceId As Integer = 9
    Public Const ACILastModified As Integer = 10
    Public Const ACIInsHolderCnt As Integer = 11
    Public Const ACIInsFolderCnt As Integer = 12
    Public Const ACIProductID As Integer = 13
    Public Const ACIProductCode As Integer = 14
    Public Const ACIProductName As Integer = 15
    Public Const ACILeadAgentCnt As Integer = 16
    Public Const ACIDateCreated As Integer = 17
    Public Const ACIStatus As Integer = 18
    'ECK 16/7/99
    Public Const ACIInsurer As Integer = 19
    Public Const ACIPremium As Integer = 20
    'Tomo010200
    Public Const ACIPolicyTypeID As Integer = 21
    Public Const ACIPolicyType As Integer = 22
    Public Const ACIGeminiPolicyStatus As Integer = 23
    'Tomo200300
    'MSS240701
    Public Const ACIRiskDesc As Integer = 24
    Public Const ACIBlank As Integer = 25
    Public Const ACINumberOfClaims As Integer = 26
    Public Const ACIAnniversaryCopy As Integer = 27

    ' Constants for the policy versions search data array indexes.
    Public Const ACPVInsuranceFolderCnt As Integer = 0
    Public Const ACPVInsuranceFileCnt As Integer = 1
    Public Const ACPVInsuranceHolderCnt As Integer = 2
    Public Const ACPVPolicyTypeID As Integer = 3
    Public Const ACPVInsuranceRef As Integer = 4
    Public Const ACPVInsuranceFileType As Integer = 5
    Public Const ACPVProduct As Integer = 6
    Public Const ACPVRenewalDate As Integer = 7
    Public Const ACPVInsurerName As Integer = 8
    Public Const ACPVShortName As Integer = 9
    Public Const ACPVPremium As Integer = 10
    Public Const ACPVInsuranceFileStatus As Integer = 11
    Public Const ACPVInsuranceFileTypeID As Integer = 12
    Public Const ACPVCoverStartDate As Integer = 13
    Public Const ACPVCoverEndDate As Integer = 14
    Public Const ACPVDesc As Integer = 15
    Public Const ACPVTaxAmount As Integer = 16
    Public Const ACPVGracePeriod As Integer = 17
    Public Const ACPVProdID As Integer = 18
    Public Const ACPVAnnualPremium As Integer = 19
    Public Const ACPVLapsedDate As Integer = 20
    Public Const ACPVSourceDesc As Integer = 21
    Public Const ACPVSourceIsDeleted As Integer = 22
    Public Const ACPVSourceAllowTempMTA As Integer = 23
    Public Const ACPVSourceAllowPermMTA As Integer = 24
    Public Const ACPVRegarding As Integer = 25
    Public Const ACPVBaseInsuranceFileCnt As Integer = 26
    Public Const ACPVLastTransDate As Integer = 27
    Public Const ACPVEventDescription As Integer = 28 ' Gaurav Changed

    ' constants for risk search data
    Public Const ACRInsFileCnt As Integer = 0
    Public Const ACIRiskId As Integer = 1
    Public Const ACIRiskDescription As Integer = 2
    Public Const ACIRiskTypeDescription As Integer = 3
    Public Const ACIRiskInceptionDate As Integer = 4
    Public Const ACIRiskExpiryDate As Integer = 5
    ' AM 061200 Add new column for risk status
    Public Const ACIRiskStatus As Integer = 6
    Public Const ACIRiskTotalSumInsured As Integer = 7
    Public Const ACIRiskTotalAnnualPremium As Integer = 8
    Public Const ACIRiskGisScreen As Integer = 9
    Public Const ACIRiskTypeId As Integer = 10
    Public Const ACIInsuranceFolderCnt As Integer = 11
    Public Const ACIRiskStatusFlag As Integer = 12

    Public Const ACIRiskFeeTax As Integer = 19
    Public Const ACIRiskTotalFee As Integer = 21
    Public Const kIRiskLinkStatus As Integer = 42
    Public Const kIRiskLingDate As Integer = 43
    Public Const kIOriginalRiskCnt As Integer = 38
    ' extra policy version details
    Public Const ACXLeadAgentCnt As Integer = 10
    Public Const ACXCurrencyID As Integer = 18
    Public Const ACXPaymentMethod As Integer = 71
    Public Const ACXMediaType As Integer = 134

    Public Const PMKeyNameVehicleRegNo As String = "vehicle_reg"

    'Constants for the columns
    Public Const ACAColumnPolicyNumber As Integer = 1
    Public Const ACAColumnRegarding As Integer = 2
    Public Const ACAColumnRenewalDate As Integer = 3
    Public Const ACAColumnStartDate As Integer = 3 'Also used for start date if GII
    Public Const ACAColumnInsurer As Integer = 4
    Public Const ACAColumnRiskTypeDescription As Integer = 5
    Public Const ACAColumnPremium As Integer = 6
    Public Const ACAColumnPolicyStatus As Integer = 7
    Public Const ACAColumnPolicyType As Integer = 8
    Public Const ACAColumnRiskType As Integer = 9
    Public Const ACAColumnGeminiEDI As Integer = 10

    Public Const ACUColumnPolicyNumber As Integer = 1
    Public Const ACUColumnPolicyType As Integer = 2
    Public Const ACUColumnRiskType As Integer = 3
    Public Const ACUColumnRegarding As Integer = 4
    Public Const ACUColumnRenewalDate As Integer = 5
    Public Const ACUColumnInsurer As Integer = 6
    Public Const ACUColumnPremium As Integer = 7
    Public Const ACUColumnPolicyStatus As Integer = 8
    Public Const ACUColumnRiskTypeDescription As Integer = 9
    Public Const ACUColumnGeminiEDI As Integer = 10

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

    ' Public source and language ID's from the
    ' Object Manager.

    <ThreadStatic()> _
    Public g_iSourceID As Integer

    <ThreadStatic()> _
    Public g_iLanguageID As Integer
    'eck220500

    <ThreadStatic()> _
    Public g_iUserID As Integer
    ' Public instance of the object manager.

    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager


    <ThreadStatic()> _
    Public g_oBusiness As Object

    <ThreadStatic()> _
    Public g_oPMUser As bPMUser.Business

    <ThreadStatic()> _
    Public g_bPMGeminiLink As Boolean

    <ThreadStatic()>
    Public g_bPMSwiftLink As Boolean

    Public Const ACRiskPosCnt As Integer = 0

    ' ***************************************************************** '
    ' Name: Min/Max
    '
    ' Description: Quick maths functions
    ' ***************************************************************** '
    Public Function Min(ByVal Expression1 As Object, ByVal Expression2 As Object) As Object
        Return Math.Min(CDbl(Expression1), CDbl(Expression2))
    End Function

    Public Function Max(ByVal Expression1 As Object, ByVal Expression2 As Object) As Object
        Return Math.Max(CDbl(Expression1), CDbl(Expression2))
    End Function

    Public Function MinMax(ByVal Expression1 As Object, ByVal Expression2 As Object, ByVal Expression3 As Object) As Object
        ' return a value with limits of Expression2 (min) and Expression3 (max)
        If Expression1 < Expression2 Then
            Return Expression2
        ElseIf Expression1 > Expression3 Then
            Return Expression3
        Else
            Return Expression1
        End If

    End Function

    ' ***************************************************************** '
    ' Name: RunReport
    '
    ' Description: Runs Crystal reports.
    '
    ' ***************************************************************** '
    Public Function RunReport(ByVal v_lPartyCnt As Integer, ByVal v_sReportName As String) As Integer

        Dim result As Integer = 0

        'Developer Guide No. 88
        Dim oReportPrint As Object


        Dim vKeyArray(,) As Object
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oReportPrint As Object
            lReturn = g_oObjectManager.GetInstance(temp_oReportPrint, sClassName:="iPMBReportPrint.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oReportPrint = temp_oReportPrint

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ReDim vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3)


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "report_name"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_sReportName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = "report_print_options"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = 0

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = "param_name1"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = v_lPartyCnt
            'No need to send this - catered for in the business object
            '    vKeyArray(PMKeyName, 4) = "param_name2"
            '    vKeyArray(PMKeyValue, 4) = "operator"
            '    vKeyArray(PMKeyName, 5) = "operator"
            '    vKeyArray(PMKeyValue, 5) = g_oObjectManager.UserName


            lReturn = oReportPrint.SetKeys(vKeyArray:=vKeyArray)


            lReturn = oReportPrint.Start

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
            End If


            oReportPrint.Dispose()

            oReportPrint = Nothing

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lPartyCnt", v_lPartyCnt)
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RunReport Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RunReport", excep:=excep, oDicParms:=oDict)

            Return result

        End Try
    End Function
End Module
