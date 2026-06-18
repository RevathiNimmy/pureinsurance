Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")>
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 21/09/00
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lPMAuthorityLevel As Integer

    Private m_vKeyArray As Object
    Private m_lAgentCnt As Integer
    Private m_lPartyCnt As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_sInsuranceFileRef As String = ""
    Private m_lPolicyTypeId As Integer
    Private m_lProductId As Integer
    Private m_lBusinessTypeId As Integer
    Private m_lRiskTypeId As Integer
    Private m_lScreenId As Integer
    Private m_lRiskId As Integer
    Private m_iIsRiAtRiskLevel As Integer
    Private m_vTransactionArray As Object = Nothing ' DD 5-2-2002
    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer


    Private m_oBusiness As bControlTrans.Automated

    ' {* USER DEFINED CODE (End) *}

    ' Stores the exit status of the interface.
    Private m_lStatus As Integer

    'TR - Commented out as New Business can also have 0 premiums (requirement for TPU)
    'Private m_bIsMTAWithNoPremium As Boolean

    ' TB 020503 - merged instalment plan at MTA?
    Private m_InstalMerge As Integer

    'Float Balance and Pre-Payment
    Dim m_iDebitAgainst As gPMConstants.PMDebitAgainst
    Dim m_lPaymentAccountID As Integer
    'Developer Guide No 17. 
    Dim m_vCreditTransactions As Object
    Dim m_vDebitTransactions As Object
    Dim m_lCashListID As Integer
    Dim m_lCashListItemId As Integer
    Dim m_lTransactionID As Integer
    Dim m_cTransactionAmount As Decimal
    Private m_bIsTrueMonthlyPolicy As Boolean

    ''Start(Saurabh Agrawal) PN58080
    Dim m_sPaymentMethod As String = ""
    ''End(Saurabh Agrawal) PN58080

    'For rollback
    Private m_sOldPolicyNumber As String = ""
    Private m_bProcessSettleTransactions As Boolean
    Private m_cRoundOffAmount As Decimal 'Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            m_lPMAuthorityLevel = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' TB 020503
            m_InstalMerge = 0

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}


                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameInsFileCnt

                        m_lInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNamePaymentAccountID

                        m_lPaymentAccountID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameDebitAgainst

                        m_iDebitAgainst = CType(CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)), gPMConstants.PMDebitAgainst)
                    Case PMNavKeyConst.PMKeyNameCreditTransactions

                        m_vCreditTransactions = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)
                    Case PMNavKeyConst.PMKeyNameDebitTransactions

                        m_vDebitTransactions = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)
                    Case PMNavKeyConst.PMKeyNameCashListID

                        m_lCashListID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameCashListItemID

                        m_lCashListItemId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameTransactionID

                        m_lTransactionID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameTransactionAmount

                        m_cTransactionAmount = CDec(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameIsTrueMonthlyPolicy

                        m_bIsTrueMonthlyPolicy = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameInsFolder 'PN35753 --RC

                        m_lInsuranceFolderCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameQuoteRef

                        m_sOldPolicyNumber = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)

                    Case gSIRLibrary.SIRLookupPaymentMethod

                        m_sPaymentMethod = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)

                    Case PMNavKeyConst.PMKeyNameSettleTransactions

                        m_bProcessSettleTransactions = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameRoundOffAmount

                        m_cRoundOffAmount = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)
                    Case PMNavKeyConst.ACTKeyAllocationCallingAppName
                        m_sCallingAppName = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)

                End Select


            Next lRow

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' DD 5-2-2002 : Taken from iPMBFinanceTransactions
            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vKeyArray(1, 4)

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameFinancePlanTransactions

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_vTransactionArray

            'TR - Commented out as New Business can also have 0 premiums (requirement for TPU)
            '    If IsNull(m_vTransactionArray) = True Or IsEmpty(m_vTransactionArray) = True _
            ''            Then
            '        m_vTransactionArray = 0
            '        If m_iTask = PMAdd Then
            '            If m_bIsMTAWithNoPremium = False Then
            '                m_lStatus = PMCancel
            '            End If
            '        End If
            '    End If

            ' TB 020503 - put another key into the navigator for checking
            ' whether user merged the MTA into existing instalment plan

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = "MTA_Instalments_Merge"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_InstalMerge

            'Float Balance and Pre-Payment


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = "Payment Account ID"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lPaymentAccountID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = "Debit Against"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_iDebitAgainst


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = "Credit Transactions"

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_vCreditTransactions


            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0
        Try


            ' {* USER DEFINED CODE (Begin) *}


            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '*************************************************************************
    ' Name:         Start (Standard Method)
    '
    ' Description:  Entry point for the object to start its processing.
    '
    ' History:      Kevin Renshaw (CMG) Initialise m_bIsMTAWithNoPremium flag
    '               Tracy Richards - 25/06/03 - Commented out
    '               m_bIsMTAWithNoPremium as New Business can also have 0
    '               premiums (requirement for TPU)
    '*************************************************************************
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Default status to OK
            m_lStatus = gPMConstants.PMEReturnCode.PMTrue

            '    m_bIsMTAWithNoPremium = False

            ' Starts the interface processing.
            m_lReturn = ProcessInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '*************************************************************************
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' History    :  Kevin Renshaw (CMG) set m_bIsMTAWithNoPremium flag
    '               Kevin Renshaw (CMG) 11/04/2003 Display a message with the number of
    '               instalment payments remaining
    '               Tracy Richards - 25/06/03 - Commented out
    '               m_bIsMTAWithNoPremium as New Business can also have 0
    '               premiums (requirement for TPU)
    '*************************************************************************
    Private Function ProcessInterface() As Integer
        Dim result As Integer = 0
        Dim sFailureReason As String = ""

        Dim oPFBusiness As bSIRPremiumFinance.Business = Nothing

        Dim oPFInterface As iPMBFinancePlanMaint.Interface_Renamed
        Dim vPlanArray(,) As Object
        Dim sRefund As String = ""
        Dim lOldInsuranceFileCnt As Integer
        Dim sErrMsg As String = ""
        Dim lNoInstalments As Integer
        Dim vValue As String = ""
        Dim sInstalmentWarn As String = ""
        Dim lYesNo As DialogResult
        Dim lLoopCount As Integer


        result = gPMConstants.PMEReturnCode.PMTrue


        'Create the Transaction object
        Dim temp_m_oBusiness As Object = Nothing
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bControlTrans.Automated", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        m_oBusiness = temp_m_oBusiness

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Set the Insurance file count

        m_oBusiness.InsuranceFileCnt = m_lInsuranceFileCnt

        m_oBusiness.CallingAppName = m_sCallingAppName


        m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            m_oBusiness.Dispose()
            m_oBusiness = Nothing
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'Sankar - (WPR67 - Enhancement_Tax_Round Off) - Paralleling

        m_lReturn = m_oBusiness.Start(iPaymentAccountId:=m_lPaymentAccountID, iDebitAgainst:=m_iDebitAgainst, vCreditTransactions:=m_vCreditTransactions, lCashListID:=m_lCashListID, lCashListItemId:=m_lCashListItemId, lTransactionID:=m_lTransactionID, cTransactionAmount:=m_cTransactionAmount, sOldPolicyNumber:=m_sOldPolicyNumber, sPaymentMethod:=m_sPaymentMethod, vDebitTransactions:=m_vDebitTransactions, bProcessSettleTransactions:=m_bProcessSettleTransactions, cRoundOffAmount:=m_cRoundOffAmount)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            'RWH(05/06/01) Let's tell the user if transactions fail.

            sFailureReason = m_oBusiness.Message
            sFailureReason = "Statistics process failed :" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & sFailureReason
            MessageBox.Show(sFailureReason, "Statistics", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End If

        ' DD 5-2-2002 : Get PF Transactions for going through the Navigator

        m_lReturn = m_oBusiness.GetPFTransactions(m_lInsuranceFileCnt, m_vTransactionArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            sFailureReason = "Failed to get the PF Transactions."
            MessageBox.Show(sFailureReason, "Statistics", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        If m_sTransactionType <> "NB" Then
            ' Create object for bSIRPremiumFinance
            'Thinh Nguyen 19/02/2002 - bSIRPremiumFinance.Business instead of bSIRPremFinance.Business
            Dim temp_oPFBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPFBusiness, "bSIRPremiumFinance.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPFBusiness = temp_oPFBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' CHECK MTA
        ' Get Original InsuranceFileCnt
        ' Use GetSingleFinancePlan Plan Array from old File
        ' Use above m_vTransactionArray to do Process MTA
        ' If fails - msgbox to use with reason and then offer NB route on Roadmap
        If m_sTransactionType = "MTA" Then
            'Thinh Nguyen 01/03/2002 (start) - get last version of policy which has instalments
            'm_lReturn = m_oBusiness.GetPreviousInsuranceFile(m_lInsuranceFileCnt, lOldInsuranceFileCnt)

            m_lReturn = m_oBusiness.GetPlanInsuranceFile(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_lPlanInsuranceFileCnt:=lOldInsuranceFileCnt)
            'Thinh Nguyen 01/03/2002 (end) - get last version of policy which has instalments

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'DD 28/05/2003: Changes for 1.9 Instalments
                'm_lReturn = oPFBusiness.GetSingleFinancePlan(Null, Null, vPlanArray, lOldInsuranceFileCnt)

                m_lReturn = oPFBusiness.GetSingleFinancePlanFromInsFileCnt(v_lInsuranceFileCnt:=lOldInsuranceFileCnt, r_vPFPremiumFinance:=vPlanArray)
                'Check to see if a plan exists
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And Information.IsArray(vPlanArray) Then

                    'DD 28/05/2003: Changes for 1.9 Instalments
                    'm_lReturn = oPFBusiness.GetInstalmentsRemaining(r_lpfprem_finance_cnt:=CLng(vPlanArray(0, 0)), r_lpfprem_finance_version:=CLng(vPlanArray(1, 0)), r_lNoInstalments:=lNoInstalments)
                    ' START CHANGES - Changed By: AAB  - Changed On: 03-Jul-2003
                    ' This is a requirement for PRU
                    iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTGenerateAdvanceCreditControlForInstalments, g_iSourceID, vValue)
                    If gPMFunctions.NullToString(vValue) = "1" Then



                        m_lReturn = oPFBusiness.GetInstalmentsRemaining(v_lpfprem_finance_cnt:=CInt(vPlanArray(0, 0)), v_lpfprem_finance_version:=CInt(vPlanArray(1, 0)), r_lNoInstalments:=lNoInstalments)

                        MessageBox.Show("There are " & lNoInstalments & " instalment payments remaining on the " &
                                        "plan", "Instalments", MessageBoxButtons.OK)
                    End If
                    ' END CHANGES - Changed By: AAB  - Changed On: 03-Jul-2003
                End If
            End If
        End If

        ' CHECK CANCEL
        ' Use GetSingleFinancePlan to see if Plan exists
        ' Prompt user for any refund default = 0
        ' Use CancelPlanInHouse
        If m_sTransactionType = "CANCEL" Then


            'Developer Guide No. GetSingleFinancePlan() takes only three parameter.
            m_lReturn = oPFBusiness.GetSingleFinancePlan(Nothing, Nothing, vPlanArray)
            'Check to see if a plan exists
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And Information.IsArray(vPlanArray) Then
                sRefund = Interaction.InputBox("This Policy is on an Instalment Plan. Please enter the " &
                          "refund due for this plan or leave as zero.", "Instalment Plan Refund", CStr(0))

                ' Cancel the Instalment Plan

                m_lReturn = oPFBusiness.CancelPlanInHouse(vPlanArray, Conversion.Val(sRefund))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        ' CHECK RENEWAL
        ' See if Plan exists
        ' Convert Plan to live, passing over transactions
        ' Transact it through Orion
        If m_sTransactionType = "REN" And (UCase(m_sPaymentMethod) <> "INVOICE" And UCase(m_sPaymentMethod) <> "PAYNOW") Then 'please don't change this to a value that is not in the Transaction_Type table

            m_lReturn = oPFBusiness.GetSingleFinancePlanFromInsFileCnt(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_vPFPremiumFinance:=vPlanArray)

            'Check to see if a plan exists
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And Information.IsArray(vPlanArray) Then

                m_lReturn = iPMFunc.GetSystemOption(1024, sInstalmentWarn, g_iSourceID)

                lYesNo = System.Windows.Forms.DialogResult.Yes
                If sInstalmentWarn <> "1" Then
                    lYesNo = MessageBox.Show("Do You Want To Pay By Instalments", "Instalments", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                End If

                If lYesNo = System.Windows.Forms.DialogResult.Yes Then
                    'Convert the Plan to Live

                    m_lReturn = oPFBusiness.TranslateQuoteToPlan(vPlanArray, m_vTransactionArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'If the Plan doesn't have any Bank details or is not live
                    'then get the user to enter them

                    'no need to check Bank details as it should be validated in iPMBFinancePlanMaint
                    'it should check only for PlanStatus (it should be live to process further)
                    'k_PFPlanStatusInd=64 (defined in bSIRPremiumFinance)
                    'PFStatusIndLive="040"(defined in bSIRPremiumFinance)

                    lLoopCount = -1

                    ' if the plan is zero rated its status will be set to completed
                    ' rather than set to live - this is a valid completion status

                    Do While CStr(vPlanArray(64, 0)) <> "040" And CStr(vPlanArray(64, 0)) <> "900"

                        lLoopCount += 1

                        Dim temp_oPFInterface As Object
                        m_lReturn = g_oObjectManager.GetInstance(temp_oPFInterface, "iPMBFinancePlanMaint.Interface_Renamed", gPMConstants.PMGetLocalInterface)
                        oPFInterface = temp_oPFInterface
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If m_sCallingAppName = "iPMURenewalCatchUp" Then
                            oPFInterface.SilentTransact = 1
                        End If
                        'Open the Maintenance Form for Bank details

                        'Thinh Nguyen 01/03/2002 (start)
                        'oPFInterface.TransactionType = "NB" 'Treat as New Business

                        m_lReturn = oPFInterface.SetProcessModes(vTransactionType:="NB", vTask:=gPMConstants.PMEComponentAction.PMEdit)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            'm_lReturn = oPFInterface.Terminate()
                            oPFInterface.Dispose()
                            oPFInterface = Nothing

                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        'Thinh Nguyen 01/03/2002 (end)



                        oPFInterface.FinancePlanCnt = vPlanArray(0, 0)


                        oPFInterface.FinancePlanVersion = vPlanArray(1, 0)

                        oPFInterface.Spawned = True

                        'when payment mode changed to Inst from invoice in REN
                        'and payment mode is CC or DD then we need to fill in required details so Silent Transction needs to be 0



                        If lLoopCount = 0 And ((CStr(vPlanArray(101, 0)).Trim() = "CC" And CStr(vPlanArray(96, 0)) <> "") Or (CStr(vPlanArray(101, 0)).Trim() = "BANK" And CStr(vPlanArray(43, 0)) <> "") Or (CStr(vPlanArray(101, 0)).Trim() <> "CC" And CStr(vPlanArray(101, 0)).Trim() <> "BANK")) Then

                            oPFInterface.SilentTransact = 1
                            oPFInterface.AccountId = m_lPaymentAccountID
                        End If


                        m_lReturn = oPFInterface.Start()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'Clean up

                        'oPFInterface.Terminate()
                        oPFInterface.Dispose()
                        oPFInterface = Nothing

                        'Re-load the Finance Plan Array

                        m_lReturn = oPFBusiness.GetSingleFinancePlanFromInsFileCnt(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_vPFPremiumFinance:=vPlanArray)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound AndAlso m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        If vPlanArray IsNot Nothing Then
                            If lLoopCount = 0 AndAlso ((CStr(vPlanArray(101, 0)).Trim() = "CC" AndAlso CStr(vPlanArray(96, 0)) <> "") OrElse (CStr(vPlanArray(101, 0)).Trim() = "BANK" AndAlso CStr(vPlanArray(43, 0)) <> "") OrElse (CStr(vPlanArray(101, 0)).Trim() <> "CC" AndAlso CStr(vPlanArray(101, 0)).Trim() <> "BANK")) Then
                            Else
                                Exit Do
                            End If
                        Else
                            Exit Do
                        End If
                    Loop
                Else
                    If m_sTransactionType = "REN" Then  'PM033055 Sumit K Delete plan if present when user don't want to pay by instalments
                        m_lReturn = oPFBusiness.DeletePlanForOneInsFile(v_lInsFileCnt:=m_lInsuranceFileCnt)
                    End If

                End If
            End If
        End If

        If m_sTransactionType <> "NB" Then

            oPFBusiness.Dispose()
            oPFBusiness = Nothing
        End If


        m_oBusiness.Dispose()

        m_oBusiness = Nothing

        'Unlock Current Policy for MTA  'PN35753 --RC
        If m_sTransactionType = gSIRLibrary.SIRProcessCodeMTA Or m_sTransactionType = "NB" Then
            UNLOCKPOLICY()
        End If

        Return result

    End Function

    'PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    ' ***************************************************************** '
    ' Name: GetOption (Private)
    '
    ' Description: Get an option.
    '
    ' ***************************************************************** '
    Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String) As Integer
        Dim result As Integer = 0
        Dim oSystemOption As bSIROptions.Business


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oSystemOption As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oSystemOption, "bSIROptions.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oSystemOption = temp_oSystemOption



            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the system option object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If


            m_lReturn = oSystemOption.GetOption(iOptionNumber:=v_iOptionNumber, sValue:=r_sOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

            End If
            oSystemOption.Dispose()

            oSystemOption = Nothing

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UNLOCKPOLICY
    '
    ' Description: UnLock Policy  'PN35753 --RC
    '
    '' ***************************************************************** '
    Private Function UNLOCKPOLICY() As Integer
        Dim result As Integer = 0
        Dim oPMLock As bPMLock.User


        result = gPMConstants.PMEReturnCode.PMTrue

        '   Find the Business Class
        Dim temp_oPMLock As Object = Nothing
        m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oPMLock = temp_oPMLock

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = oPMLock.UnLockKey("insurance_folder_cnt", vKeyValue:=m_lInsuranceFolderCnt, iUserID:=g_oObjectManager.UserID)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to process the interface.
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMLogError1, sMsg:="Error trying to unlock the policy", vApp:="iPMUStats", vClass:="Intreface", vMethod:="UNLOCKPOLICY", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result

        End If

        'Terminate the business object

        'm_lReturn = oPMLock.Terminate()
        oPMLock.Dispose()
        oPMLock = Nothing

        Return result

    End Function
End Class

