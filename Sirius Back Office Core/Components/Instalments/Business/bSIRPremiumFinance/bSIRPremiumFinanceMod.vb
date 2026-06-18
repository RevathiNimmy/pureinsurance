Option Strict Off
Option Explicit On
Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Module Name: MainModule
    ' Date:  11/03/1998
    ' Description: Main Module.
    ' Edit History:
    '   PF200901 - Added useful functions
    ' ***************************************************************** '

    Public Const kActionCodeAmendment As String = "Amendment"
    Public Const kActionCodeCancellation As String = "Cancellation"
    Public Const kMainTabBankDetails As Integer = 2

    ' Constant for the functions to identify which class this is.
    Public Const ACApp As String = "bSIRPremiumFinance"
    Private Const ACClass As String = "MainModule"

    Public Const ACRateEndDate As Integer = 0
    Public Const ACRateDaysDelay As Integer = 1
    Public Const ACRateDepositReq As Integer = 2
    Public Const ACRateDepositPC As Integer = 3
    Public Const ACRateAllowProtection As Integer = 4
    Public Const ACRateProtectRate As Integer = 5
    Public Const ACRateMinInterest As Integer = 6
    Public Const ACRateMin1 As Integer = 7
    Public Const ACRateMax1 As Integer = 8
    Public Const ACRateRate1 As Integer = 9
    Public Const ACRateAPR1 As Integer = 10
    Public Const ACRateR1Com As Integer = 11
    Public Const ACRateAPR1Com As Integer = 12
    Public Const ACRateCom1PC As Integer = 13
    Public Const ACRateMin2 As Integer = 14
    Public Const ACRateMax2 As Integer = 15
    Public Const ACRateRate2 As Integer = 16
    Public Const ACRateAPR2 As Integer = 17
    Public Const ACRateR2Com As Integer = 18
    Public Const ACRateAPR2Com As Integer = 19
    Public Const ACRateCom2PC As Integer = 20
    Public Const ACRateMin3 As Integer = 21
    Public Const ACRateMax3 As Integer = 22
    Public Const ACRateRate3 As Integer = 23
    Public Const ACRateAPR3 As Integer = 24
    Public Const ACRateR3Com As Integer = 25
    Public Const ACRateAPR3Com As Integer = 26
    Public Const ACRateCom3PC As Integer = 27
    Public Const ACRateMin4 As Integer = 28
    Public Const ACRateMax4 As Integer = 29
    Public Const ACRateRate4 As Integer = 30
    Public Const ACRateAPR4 As Integer = 31
    Public Const ACRateR4Com As Integer = 32
    Public Const ACRateAPR4Com As Integer = 33
    Public Const ACRateCom4PC As Integer = 34
    Public Const ACRateMin5 As Integer = 35
    Public Const ACRateMax5 As Integer = 36
    Public Const ACRateRate5 As Integer = 37
    Public Const ACRateAPR5 As Integer = 38
    Public Const ACRateR5Com As Integer = 39
    Public Const ACRateAPR5Com As Integer = 40
    Public Const ACRateCom5PC As Integer = 41
    Public Const ACArrangementFee As Integer = 42
    Public Const ACPaymentMethod As Integer = 43
    Public Const ACMinMTA As Integer = 44
    Public Const ACMinMTAInstalments As Integer = 45
    Public Const ACBasisOfCalcNew As Integer = 46
    Public Const ACBasisOfCalcMTA As Integer = 47
    Public Const ACBasisOfCalcRenewal As Integer = 48

    Public Const ACSpoolSilentMode As Integer = 4
    Public Const ACDocTypeSchedule As Integer = 10

    Public Const PMKeyNameInsFileCnt As String = "insurance_file_cnt"
    Public Const ACTKeyNameDocumentID As String = "document_id"
    Public Const PMKeyNameInsFolderCnt As String = "insurance_folder_cnt"
    Public Const PMKeyNamePartyCnt As String = "party_cnt"
    Public Const PMKeyNameProductID As String = "Product_id"
    ' ***************************************************************** '
    ' TR071102 - Added for TS23 (start)
    ' Not Used by Stored Procedures. Only used to create the Quotes array
    ' manually, which is passed externally to the Quotes User control.
    ' ***************************************************************** '
    Public Const k_PFQuoteCompanyNo As Integer = 0
    Public Const k_PFQuoteCompanyName As Integer = 1
    Public Const k_PFQuoteSchemeNo As Integer = 2
    Public Const k_PFQuoteSchemeVersion As Integer = 3
    Public Const k_PFQuoteSchemeName As Integer = 4
    Public Const k_PFQuoteFrequencyID As Integer = 5
    Public Const k_PFQuoteFrequencyDescription As Integer = 6
    Public Const k_PFQuoteMediaTypeID As Integer = 7
    Public Const k_PFQuoteMediaTypeDescription As Integer = 8
    Public Const k_PFQuoteProductClass As Integer = 9
    Public Const k_PFQuoteProductCode As Integer = 10
    Public Const k_PFQuoteTotalAmountInput As Integer = 11
    Public Const k_PFQuoteInstalmentsToPay As Integer = 12
    Public Const k_PFQuoteFirstInstalmentDate As Integer = 13
    Public Const k_PFQuoteNextInstalmentDate As Integer = 14
    Public Const k_PFQuoteLastInstalmentDate As Integer = 15
    Public Const k_PFQuoteFirstInstalmentAmount As Integer = 16
    Public Const k_PFQuoteOtherInstalmentAmount As Integer = 17
    Public Const k_PFQuoteTotalInstalmentsAmount As Integer = 18
    Public Const k_PFQuoteAprRate As Integer = 19
    Public Const k_PFQuoteInterestRate As Integer = 20
    Public Const k_PFQuoteDaysDelay As Integer = 21
    Public Const k_PFQuoteDepositAmount As Integer = 22
    Public Const k_PFQuoteInterestAmount As Integer = 23
    Public Const k_PFQuoteTaxAmount As Integer = 24
    Public Const k_PFQuoteFinanceCharge As Integer = 25
    Public Const k_PFQuoteProtectionAmount As Integer = 26
    Public Const k_PFQuoteOriginalOtherInstalmentAmount As Integer = 27
    Public Const k_PFQuoteHighlightCell As Integer = 28
    Public Const k_PFQuoteSchemeTypeCode As Integer = 29
    Public Const k_PFQuoteMediaTypeValidation As Integer = 30
    Public Const k_PFQuoteFrequencyPerYear As Integer = 31
    Public Const k_PFQuotePFRF_ID As Integer = 32
    Public Const k_PFQuoteFrequencyPeriod As Integer = 33
    Public Const k_PFQuoteFrequencyAmount As Integer = 34
    'TR - 24/03/03 - TS17 Recovery By Instalments changes
    Public Const k_PFQuoteOriginalAmount As Integer = 35
    Public Const k_PFQuoteClaimDebtID As Integer = 36
    Public Const k_PFQuoteUserID As Integer = 37
    Public Const k_PFQuoteAgentCnt As Integer = 38
    Public Const k_PFQuoteAgentRef As Integer = 39
    Public Const k_PFQuoteLastInstalmentAmount As Integer = 40
    Public Const k_PFSGUsername As Integer = 41
    Public Const k_PFSGPassword As Integer = 42
    Public Const k_PFSGBrokerID As Integer = 43
    Public Const k_PFSGBrokerURL As Integer = 44
    Public Const k_PFSGTimeout As Integer = 45
    Public Const k_PFSGProviderCode As Integer = 46
    Public Const k_PFSGTerms As Integer = 47
    Public Const k_PFSGRef As Integer = 48
    Public Const k_PFOriginalRate As Integer = 49
    Public Const k_PFQuoteRefundType As Integer = 50
    Public Const k_PFQuoteMinMTA As Integer = 51
    Public Const k_PFQuoteTaxGroupID As Integer = 52
    Public Const k_PFQuoteXSLCode As Integer = 53
    Public Const k_PFSGSchemeType As Integer = 54
    Public Const k_PFDepositAsInstalment As Integer = 55
    Public Const k_PFAlignTo As Integer = 56
    Public Const k_PFBranchCodeMandatory As Integer = 57
    Public Const k_PFBranchNameMandatory As Integer = 58
    Public Const k_PFBankNameMandatory As Integer = 59
    Public Const k_PFBankAddressMandatory As Integer = 60
    Public Const k_PFStartLimit As Integer = 61
    Public Const k_PFStartDate As Integer = 62
    Public Const k_PFDelayULimit As Integer = 63
    Public Const k_PFSingleInstalmentPerMonth As Integer = 64
    Public Const k_PFFirstInstalmentAlignWithMonthInDay As Integer = 65
    Public Const k_PFUseTransCurrncy As Integer = 66
    Public Const k_PFFinanceToNet As Integer = 67


    'Start (Sriram P)Tech Spec - Valiant - P16 - Broker Instalments.doc section(5.3.1)
    Public Const k_AgentCommission As Integer = 17
    Public Const k_TaxOnCommission As Integer = 16
    'End (Sriram P)Tech Spec - Valiant - P16 - Broker Instalments.doc section(5.3.1)

    'TR071102 - Added for TS23 (end)
    'The constant below should be set to the value of the last constant above
    'as this is used for dimensioning within the class module clsPremFinance.
    Public Const k_PFQuoteUBound As Integer = k_PFFinanceToNet

    'AAB - 03/24/2003 - Added to support 1.9 instalments integration
    Public Const ACTKeyNameAccountID As String = "account_id"
    Public Const ACTKeyNameTransDetailID As String = "trans_detail_id"
    Public Const ACTKeyNameTransDetailIDs As String = "trans_detail_ids"

    Public Const kSystemOptionCreditControlEnabled As Integer = 5001
    Public Const kparty_cnt As Integer = 0
    Public Const kShortName As Integer = 1
    Public Const kResolvedName As Integer = 2
    Public Const kAddress1 As Integer = 3
    Public Const kAddress2 As Integer = 4
    Public Const kAddress3 As Integer = 5
    Public Const kAddress4 As Integer = 6
    Public Const kPostalCode As Integer = 7
    Public Const kCountryId As Integer = 8
    Public Const kDescription As Integer = 9
    Public Const kSingleInstalmentPlan As Integer = 10
    Sub Main_Renamed()
        ' Do not put any code in here (It will not be executed).
        ' So why even bother putting the function here?
    End Sub

    ' ***************************************************************** '
    ' Function: Between
    ' Description: Returns True is the expression is between the min and
    '              max values
    ' Edit History:
    '   PF200901 - Created
    ' ***************************************************************** '
    Public Function Between(ByVal Expression As Object, ByVal MinValue As Object, ByVal MaxValue As Object) As Boolean

        Dim result As Boolean = False
        Try

            result = (Expression >= MinValue) And (Expression <= MaxValue)

        Catch
        End Try

        Return result
    End Function

    ' ***************************************************************** '
    ' Function: NullIfEmpty
    ' Description: Returns Null if the value is empty, useful for passing
    '              parameters to stored procedures
    ' Edit History:
    '   PF240901 - Created
    ' ***************************************************************** '
    Public Function NullIfEmpty(ByVal Expression As Object) As Object

        Try

            If Expression IsNot Nothing AndAlso (CStr(Expression)).Length Then
                Return Expression
            Else

                Return DBNull.Value
            End If

        Catch
        End Try

        Return Expression

    End Function

    ' ***************************************************************** '
    ' Function: BlankIfEmpty
    '
    ' Description: Returns Blank string if the value is empty, useful for passing
    '              parameters to stored procedures
    '
    ' Edit History:
    '   PF240901 - Created
    ' ***************************************************************** '
    Public Function BlankIfEmpty(ByVal Expression As Object) As String

        Try

            If Expression IsNot Nothing AndAlso (CStr(Expression)).Length Then

                Return CStr(Expression).Trim()
            Else
                Return ""
            End If

        Catch
        End Try

        Return CStr(Expression)
    End Function
End Module