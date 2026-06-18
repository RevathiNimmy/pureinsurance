Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Text
Imports Word = Microsoft.Office.Interop.Word
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("clsPremiumFinance_NET.clsPremiumFinance")> _
Public NotInheritable Class clsPremiumFinance

    Implements IDisposable
    ' ************************************************
    ' Added to replace global variables 19/09/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************


    '--------------------------------------------------------------------------------
    ' I M P O R T A N T    N O T E
    '--------------------------------------------------------------------------------
    ' This component has been copied from the bPMPremFinance component developed by
    ' the Marsh team (their component itself was copied from a legacy system
    ' previously developed by PM). Enhancements have been made to the database tables
    ' and code in this module to suit the purposes of GeminiNet. Some functionality
    ' and data items in the tables have been left in for possible future uses whereas
    ' some unknown data items and ones of no use have been removed. Note that for this
    ' first release we will NOT be making use of anything other than a simple DD for
    ' NB finance calculation (Personal Lines). Functionality that addresses areas such
    ' as edi, document production, storage of accepted Premium Finance data into an
    ' output table etc etc will not be required and in many areas has not been developed
    ' or tested. Note the only public function that will be called in this first release
    ' is CalculateFinance.
    '
    ' Have fun with it !

    ' Chris Barnes 12/9/2000
    '
    '--------------------------------------------------------------------------------

    'Private variables used for the process of quote calculation

    Private m_iCmpNo As Integer
    Private m_iSchemeNo As Integer
    Private m_iSchemeVer As Integer
    Private m_vStartDate As String = ""
    Private m_vEndDate As String = ""
    Private m_sPayProtect As String = ""
    Private m_sProdClass As String = ""
    Private m_sTransType As String = ""
    Private m_sDepositReq As String = ""
    Private m_dDepositPCent As Double
    Private m_dPayProtectPCent As Double
    Private m_dMinFinanceCharge As Double
    Private m_vCalcInd As String = ""
    Private m_sCompanyName As String = ""
    Private m_sSchemeName As String = ""

    'Financial values
    Private m_dAmountToFinance As Double
    Private m_dDeposit As Double
    Private m_dNetAmount As Double
    Private m_dFirstInstalment As Double
    Private m_dOthInstalments As Double
    Private m_iNoOfInstalments As Integer
    Private m_dAPR As Double
    Private m_dIntRate As Double
    Private m_dCostOfProtect As Double
    Private m_iDaysDelay As Integer
    Private m_dArrangementFee As Double
    Private m_dInterestCost As Double
    Private m_dTotalCost As Double

    'Private variables for other purposes
    Private m_oDbConn As dPMDAO.Database
    Private m_vQuoteArray As Array
    Private m_oEDIMessage As Object

    Private m_bUseFlipArray As Boolean
    Private m_bCloseDatabase As Boolean

    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    'ND 041200 New variable from annual calculation
    Private m_bJustCalculateAnnual As Boolean
    Private m_vAnnualFromMonthlyPremium As Object

    'Private Constants
    Private Const ACClass As String = "clsPremFinance"
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public Property CompanyNo() As Integer
        Get
            Return m_iCmpNo
        End Get
        Set(ByVal Value As Integer)
            m_iCmpNo = Value
        End Set
    End Property
    Public Property SchemeNo() As Integer
        Get
            Return m_iSchemeNo
        End Get
        Set(ByVal Value As Integer)
            m_iSchemeNo = Value
        End Set
    End Property
    Public Property SchemeVer() As Integer
        Get
            Return m_iSchemeVer
        End Get
        Set(ByVal Value As Integer)
            m_iSchemeVer = Value
        End Set
    End Property
    Public Property StartDate() As String
        Get
            Return m_vStartDate
        End Get
        Set(ByVal Value As String)

            m_vStartDate = CStr(Value)
        End Set
    End Property
    Public Property EndDate() As String
        Get
            Return m_vEndDate
        End Get
        Set(ByVal Value As String)

            m_vEndDate = CStr(Value)
        End Set
    End Property
    Public Property PayProtection() As String
        Get
            Return m_sPayProtect
        End Get
        Set(ByVal Value As String)
            m_sPayProtect = Value
        End Set
    End Property
    Public Property ProdClass() As String
        Get
            Return m_sProdClass
        End Get
        Set(ByVal Value As String)
            m_sProdClass = Value
        End Set
    End Property
    Public Property TransType() As String
        Get
            Return m_sTransType
        End Get
        Set(ByVal Value As String)
            m_sTransType = Value
        End Set
    End Property
    Public Property AmountToFinance() As Double
        Get
            Return m_dAmountToFinance
        End Get
        Set(ByVal Value As Double)
            m_dAmountToFinance = Value
        End Set
    End Property
    Public Property Deposit() As Double
        Get
            Return m_dDeposit
        End Get
        Set(ByVal Value As Double)
            m_dDeposit = Value
        End Set
    End Property
    Public Property NetAmount() As Double
        Get
            Return m_dNetAmount
        End Get
        Set(ByVal Value As Double)
            m_dNetAmount = Value
        End Set
    End Property
    Public Property FirstInstallment() As Double
        Get
            Return m_dFirstInstalment
        End Get
        Set(ByVal Value As Double)
            m_dFirstInstalment = Value
        End Set
    End Property
    Public Property OthInstallments() As Double
        Get
            Return m_dOthInstalments
        End Get
        Set(ByVal Value As Double)
            m_dOthInstalments = Value
        End Set
    End Property
    Public Property NoOfInstallments() As Integer
        Get
            Return m_iNoOfInstalments
        End Get
        Set(ByVal Value As Integer)
            m_iNoOfInstalments = Value
        End Set
    End Property
    Public Property APR() As Double
        Get
            Return m_dAPR
        End Get
        Set(ByVal Value As Double)
            m_dAPR = Value
        End Set
    End Property
    Public Property IntRate() As Double
        Get
            Return m_dIntRate
        End Get
        Set(ByVal Value As Double)
            m_dIntRate = Value
        End Set
    End Property
    Public Property CostOfProtect() As Double
        Get
            Return m_dCostOfProtect
        End Get
        Set(ByVal Value As Double)
            m_dCostOfProtect = Value
        End Set
    End Property
    Public Property DaysDelay() As Integer
        Get
            Return m_iDaysDelay
        End Get
        Set(ByVal Value As Integer)
            m_iDaysDelay = Value
        End Set
    End Property

    Public Property ArrangementFee() As Integer
        Get
            Return m_dArrangementFee
        End Get
        Set(ByVal Value As Integer)
            m_dArrangementFee = Value
        End Set
    End Property

    Public Property InterestCost() As Double
        Get
            Return m_dInterestCost
        End Get
        Set(ByVal Value As Double)
            m_dInterestCost = Value
        End Set
    End Property
    Public Property TotalCost() As Double
        Get
            Return m_dTotalCost
        End Get
        Set(ByVal Value As Double)
            m_dTotalCost = Value
        End Set
    End Property
    Public Property DepositReq() As String
        Get
            Return m_sDepositReq
        End Get
        Set(ByVal Value As String)
            m_sDepositReq = Value
        End Set
    End Property
    Public Property DepositPCent() As Double
        Get
            Return m_dDepositPCent
        End Get
        Set(ByVal Value As Double)
            m_dDepositPCent = Value
        End Set
    End Property
    Public Property PayProtectPCent() As Double
        Get
            Return m_dPayProtectPCent
        End Get
        Set(ByVal Value As Double)
            m_dPayProtectPCent = Value
        End Set
    End Property
    Public Property MinFinanceCharge() As Double
        Get
            Return m_dMinFinanceCharge
        End Get
        Set(ByVal Value As Double)
            m_dMinFinanceCharge = Value
        End Set
    End Property
    Public Property CalcInd() As String
        Get
            Return m_vCalcInd
        End Get
        Set(ByVal Value As String)
            m_vCalcInd = CStr(Value)
        End Set
    End Property
    Public Property CompanyName() As String
        Get
            Return m_sCompanyName
        End Get
        Set(ByVal Value As String)
            m_sCompanyName = Value
        End Set
    End Property
    Public Property SchemeName() As String
        Get
            Return m_sSchemeName
        End Get
        Set(ByVal Value As String)
            m_sSchemeName = Value
        End Set
    End Property

    ' ***************************************************************** '
    '
    ' Name: Calculate_Quotes
    '
    ' Description:
    '
    ' History: 11/09/2000 CJB - Created.
    '
    ' ***************************************************************** '
    Public Function Calculate_Quotes(ByRef vQuoteableArray As Array) As Integer
        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Not (m_vQuoteArray Is Nothing) Then
                m_vQuoteArray = Nothing
                m_vQuoteArray = Nothing
            End If

            If m_bUseFlipArray Then
                'Call Array Flipper Here!
                lResult = CType(gPMFunctions.FlipArray(vQuoteableArray), gPMConstants.PMEReturnCode)
                If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lResult
                End If
            End If

            'Calculate quotations
            For iRowCounter As Integer = vQuoteableArray.GetLowerBound(0) To vQuoteableArray.GetUpperBound(0)
                'Call calculator function for each scheme in the array
                lResult = CType(Calculator(vQuoteableArray, iRowCounter), gPMConstants.PMEReturnCode)
                If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lResult
                End If
            Next iRowCounter

            vQuoteableArray = Nothing

            vQuoteableArray = m_vQuoteArray

            m_vQuoteArray = Nothing

            If m_bUseFlipArray Then
                'Call Array Flipper Here!
                lResult = CType(gPMFunctions.FlipArray(vQuoteableArray), gPMConstants.PMEReturnCode)
                If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lResult
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Calculate_Quotes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Calculate_Quotes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Calculator
    '
    ' Description:
    '
    ' History: 19/04/2000 IAC - Created.
    '          04/12/2000 ND  - Checks for JustCalculateAnnual flag being set
    '                           if so calls CalculateAnnualFromMonthly function
    '                           for Ratebeater and then exits
    '
    ' ***************************************************************** '
    Public Function Calculator(ByRef vQuoteableArray As Array, ByRef iRow As Integer) As Integer

        Dim result As Integer = 0
        Dim iColumn, iLastColumn As Integer
        Dim lResult As gPMConstants.PMEReturnCode
        Dim sPPInd, sQDocName, sQDocPath, sBDocPath, sCDocPath, sBDocName, sCDocName, sClientRef, sPaymentMethod As String
        Dim vAutoGenPlanRef As String = ""
        Dim vFinCollPlanRef As String = ""
        Dim vPremFinCnt As String = ""
        Dim vTempArray As Array
        Dim vPolicyCnt As String = ""
        Dim dTenPaymentCheck As Double
        Dim lTenPaymentCheck As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get scheme information from array
            iLastColumn = vQuoteableArray.GetUpperBound(1)
            iColumn = vQuoteableArray.GetLowerBound(1)


            CompanyNo = CInt(vQuoteableArray(iRow, m_cCompanyNo))


            SchemeNo = CInt(vQuoteableArray(iRow, m_cSchemeNo))

            SchemeVer = CInt(vQuoteableArray(iRow, m_cSchemeVer))


            StartDate = CStr(vQuoteableArray(iRow, m_cStartDate))



            If Convert.IsDBNull(vQuoteableArray(iRow, m_cEndDate)) Or IsNothing(vQuoteableArray(iRow, m_cEndDate)) Or Object.Equals(vQuoteableArray(iRow, m_cEndDate), Nothing) Or CStr(vQuoteableArray(iRow, m_cEndDate)) = "" Then

                EndDate = Nothing
            Else

                EndDate = CStr(vQuoteableArray(iRow, m_cEndDate))
            End If


            AmountToFinance = CDbl(vQuoteableArray(iRow, m_cAmountToFinance))


            ProdClass = CStr(vQuoteableArray(iRow, m_cProdClass))

            'Check for nulls

            If Not (Convert.IsDBNull(vQuoteableArray(iRow, m_cBasisOfCalc)) Or IsNothing(vQuoteableArray(iRow, m_cBasisOfCalc))) Then

                CalcInd = CStr(vQuoteableArray(iRow, m_cBasisOfCalc))
            End If


            If Not (Convert.IsDBNull(vQuoteableArray(iRow, m_cCompanyName)) Or IsNothing(vQuoteableArray(iRow, m_cCompanyName))) Then

                CompanyName = CStr(vQuoteableArray(iRow, m_cCompanyName))
            End If


            If Not (Convert.IsDBNull(vQuoteableArray(iRow, m_cSchemeName)) Or IsNothing(vQuoteableArray(iRow, m_cSchemeName))) Then

                SchemeName = CStr(vQuoteableArray(iRow, m_cSchemeName))
            End If


            If Not (Convert.IsDBNull(vQuoteableArray(iRow, m_cTransType)) Or IsNothing(vQuoteableArray(iRow, m_cTransType))) Then

                TransType = CStr(vQuoteableArray(iRow, m_cTransType))
            End If


            If Not (Convert.IsDBNull(vQuoteableArray(iRow, m_cQDocPath)) Or IsNothing(vQuoteableArray(iRow, m_cQDocPath))) Then

                sQDocPath = CStr(vQuoteableArray(iRow, m_cQDocPath))
            End If


            If Not (Convert.IsDBNull(vQuoteableArray(iRow, m_cQDocName)) Or IsNothing(vQuoteableArray(iRow, m_cQDocName))) Then

                sQDocName = CStr(vQuoteableArray(iRow, m_cQDocName))
            End If


            If Not (Convert.IsDBNull(vQuoteableArray(iRow, m_cBDocPath)) Or IsNothing(vQuoteableArray(iRow, m_cBDocPath))) Then

                sBDocPath = CStr(vQuoteableArray(iRow, m_cBDocPath))
            End If


            If Not (Convert.IsDBNull(vQuoteableArray(iRow, m_cBDocName)) Or IsNothing(vQuoteableArray(iRow, m_cBDocName))) Then

                sBDocName = CStr(vQuoteableArray(iRow, m_cBDocName))
            End If


            If Not (Convert.IsDBNull(vQuoteableArray(iRow, m_cCDocPath)) Or IsNothing(vQuoteableArray(iRow, m_cCDocPath))) Then

                sCDocPath = CStr(vQuoteableArray(iRow, m_cCDocPath))
            End If


            If Not (Convert.IsDBNull(vQuoteableArray(iRow, m_cCdocName)) Or IsNothing(vQuoteableArray(iRow, m_cCdocName))) Then

                sCDocName = CStr(vQuoteableArray(iRow, m_cCdocName))
            End If


            If Not (Convert.IsDBNull(vQuoteableArray(iRow, m_cClientId)) Or IsNothing(vQuoteableArray(iRow, m_cClientId))) Then

                sClientRef = CStr(vQuoteableArray(iRow, m_cClientId))
            End If


            If Not (Convert.IsDBNull(vQuoteableArray(iRow, m_cPaymentMethod)) Or IsNothing(vQuoteableArray(iRow, m_cPaymentMethod))) Then

                sPaymentMethod = CStr(vQuoteableArray(iRow, m_cPaymentMethod))
            End If

            'End of null checking

            'The following fields can allow null values

            vPremFinCnt = CStr(vQuoteableArray(iRow, m_cPremFinCnt))

            vAutoGenPlanRef = CStr(vQuoteableArray(iRow, m_cAutoGenPlanRef))

            vFinCollPlanRef = CStr(vQuoteableArray(iRow, m_cFinCollPlanRef))

            vPolicyCnt = CStr(vQuoteableArray(iRow, m_cPolicyCnt))

            'Get other details from rate table
            lResult = GetRateInfo()

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            'ND 041200 - If we just want the annual premium calculated from the annual then do it now
            If m_bJustCalculateAnnual Then

                lResult = CalculateAnnualFromMonthly()

                If lResult <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = lResult
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error - Monthly premium must be greater than zero", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateAnnualFromMonthly")

                End If

                ' we don't want to do any more here now
                Return result
            End If

            If NoOfInstallments > 1 Then
                'Use information from rate table to calculate quote. Deposit & Amount to Finance already
                'calculated
                Select Case CalcInd.ToUpper()
                    Case "A"
                        'Add deposit on to amount to finance to calc interest due
                        InterestCost = CDbl(StringsHelper.Format(AmountToFinance * (IntRate / 100), "###,##0.00"))
                        If InterestCost < MinFinanceCharge Then
                            InterestCost = CDbl(StringsHelper.Format(MinFinanceCharge, "###,##0.00"))
                        End If

                        'Calculate cost of payment protection
                        Select Case sPPInd.ToUpper()
                            Case "A"
                                CostOfProtect = CDbl(StringsHelper.Format((AmountToFinance + InterestCost) * (PayProtectPCent / 100), "###,##0.00"))
                            Case "B"
                                CostOfProtect = CDbl(StringsHelper.Format(((AmountToFinance - Deposit) + InterestCost) * (PayProtectPCent / 100), "###,##0.00"))
                            Case Else
                                CostOfProtect = CDbl(StringsHelper.Format(0, "###,##0.00"))
                        End Select

                        'Calculate totalcost without payment protection
                        TotalCost = CDbl(StringsHelper.Format((AmountToFinance) + InterestCost, "###,##0.00"))

                        'Adjust totalcost to reflect payment protection
                        TotalCost = CDbl(StringsHelper.Format(TotalCost + CostOfProtect, "###,##0.00"))

                        'Decide which interest rate to use for deposit amt
                        If DepositReq.ToUpper() = "Y" And DepositPCent > 0 Then
                            'calc deposit & remove from amount to be financed
                            Deposit = CDbl(StringsHelper.Format(TotalCost * (DepositPCent / 100), "###,##0.00"))
                        Else
                            Deposit = 0
                        End If

                    Case "B"

                        TotalCost = AmountToFinance

                        'Decide which interest rate to use for deposit amt
                        If DepositReq.ToUpper() = "Y" And DepositPCent > 0 Then
                            'calc deposit & remove from amount to be financed
                            Deposit = CDbl(StringsHelper.Format(TotalCost * (DepositPCent / 100), "###,##0.00"))
                        Else
                            Deposit = 0
                        End If

                        'Calc interest due on amount to finance without deposit
                        InterestCost = CDbl(StringsHelper.Format((AmountToFinance - Deposit) * (IntRate / 100), "###,##0.00"))
                        If InterestCost < MinFinanceCharge Then
                            InterestCost = CDbl(StringsHelper.Format(MinFinanceCharge, "###,##0.00"))
                        End If

                        'Calculate cost of payment protection
                        Select Case sPPInd.ToUpper()
                            Case "A"
                                CostOfProtect = CDbl(StringsHelper.Format((AmountToFinance + InterestCost) * (PayProtectPCent / 100), "###,##0.00"))
                            Case "B"
                                CostOfProtect = CDbl(StringsHelper.Format(((AmountToFinance - Deposit) + InterestCost) * (PayProtectPCent / 100), "###,##0.00"))
                            Case Else
                                CostOfProtect = CDbl(StringsHelper.Format(0, "###,##0.00"))
                        End Select

                        'Calculate totalcost without payment protection
                        TotalCost = CDbl(StringsHelper.Format((TotalCost - Deposit) + InterestCost, "###,##0.00"))

                        'Adjust totalcost to reflect payment protection
                        TotalCost = CDbl(StringsHelper.Format(TotalCost + CostOfProtect, "###,##0.00"))

                        TotalCost += Deposit
                    Case Else
                        'A Problem has occurred so produce error and exit.
                        Return gPMConstants.PMEReturnCode.PMFalse
                End Select

                'Add any Arrangement Fee on to the total amt (note interest is NOT charged on the fee)
                TotalCost = CDbl(StringsHelper.Format(TotalCost + ArrangementFee, "###,##0.00"))

                dTenPaymentCheck = (TotalCost - Deposit) / NoOfInstallments
                dTenPaymentCheck *= 100

                lTenPaymentCheck = CInt(Math.Floor(dTenPaymentCheck))

                dTenPaymentCheck = (lTenPaymentCheck / 100) * NoOfInstallments

                If dTenPaymentCheck = (TotalCost - Deposit) Then
                    OthInstallments = CDbl(StringsHelper.Format((TotalCost - Deposit) / NoOfInstallments, "###,##0.00"))
                    FirstInstallment = CDbl(StringsHelper.Format(OthInstallments, "###,##0.00"))
                Else
                    'Calculate Instalments
                    OthInstallments = CDbl(StringsHelper.Format((TotalCost - Deposit) / NoOfInstallments, "###,##0.000"))

                    'Adjust OthInstalments for rounding
                    OthInstallments = (Math.Floor(OthInstallments * 100)) / 100
                    FirstInstallment = CDbl(StringsHelper.Format((TotalCost - Deposit) - (OthInstallments * (NoOfInstallments - 1)), "###,##0.00"))
                End If


            Else
                'Add any Arrangement Fee on to the total amt (note interest is NOT charged on the fee)
                TotalCost = CDbl(StringsHelper.Format(AmountToFinance + ArrangementFee, "###,##0.00"))
                FirstInstallment = 0
                OthInstallments = 0
                CostOfProtect = 0
                InterestCost = 0
                Deposit = CDbl(StringsHelper.Format(TotalCost, "###,##0.00"))
            End If

            'Insert results into array ready for returning to the calling object

            'Move contents from existing array to a temporary array, allowing us to
            'resize the original array.
            If Information.IsArray(m_vQuoteArray) Then
                vTempArray = Nothing

                vTempArray = m_vQuoteArray

                'Resize original array to include another row of data, and move the contents of the
                'temporary array back to the original array before appending extra row
                m_vQuoteArray = Nothing
                m_vQuoteArray = Array.CreateInstance(GetType(Object), New Integer() {iRow + 1, m_cMainArray + 1}, New Integer() {0, 0})

                For iRowCount As Integer = vTempArray.GetLowerBound(0) To vTempArray.GetUpperBound(0)
                    For iColumnCount As Integer = vTempArray.GetLowerBound(1) To vTempArray.GetUpperBound(1)
                        m_vQuoteArray(iRowCount, iColumnCount) = vTempArray(iRowCount, iColumnCount)
                    Next iColumnCount
                Next iRowCount

                'Release memory allocated for temporary array
                vTempArray = Nothing
            Else
                m_vQuoteArray = Array.CreateInstance(GetType(Object), New Integer() {iRow + 1, m_cMainArray + 1}, New Integer() {0, 0})
            End If

            'Append additional row of data
            m_vQuoteArray(iRow, m_cCompanyName) = CompanyName
            m_vQuoteArray(iRow, m_cSchemeName) = SchemeName
            m_vQuoteArray(iRow, m_cSchemeNo) = SchemeNo
            m_vQuoteArray(iRow, m_cSchemeVer) = SchemeVer
            m_vQuoteArray(iRow, m_cStartDate) = StartDate
            m_vQuoteArray(iRow, m_cEndDate) = EndDate
            m_vQuoteArray(iRow, m_cProdClass) = ProdClass
            m_vQuoteArray(iRow, m_cTransType) = TransType
            m_vQuoteArray(iRow, m_cBasisOfCalc) = CalcInd
            m_vQuoteArray(iRow, m_cAmountToFinance) = StringsHelper.Format(AmountToFinance, "###,##0.00")
            m_vQuoteArray(iRow, m_cAPR) = StringsHelper.Format(APR, "#0.00")
            m_vQuoteArray(iRow, m_cIntRate) = StringsHelper.Format(IntRate, "#0.00")
            m_vQuoteArray(iRow, m_cDaysDelay) = DaysDelay
            m_vQuoteArray(iRow, m_cNoOfInstalments) = NoOfInstallments
            m_vQuoteArray(iRow, m_cFirstInstalment) = StringsHelper.Format(FirstInstallment, "###,##0.00")
            m_vQuoteArray(iRow, m_cOthInstalments) = StringsHelper.Format(OthInstallments, "###,##0.00")
            m_vQuoteArray(iRow, m_cCostOfProtect) = StringsHelper.Format(CostOfProtect, "###,##0.00")
            m_vQuoteArray(iRow, m_cDeposit) = StringsHelper.Format(Deposit, "###,##0.00")
            m_vQuoteArray(iRow, m_cNetAmount) = StringsHelper.Format(NetAmount, "###,##0.00")
            m_vQuoteArray(iRow, m_cTotalCost) = StringsHelper.Format(TotalCost, "###,##0.00")
            m_vQuoteArray(iRow, m_cInterestCost) = StringsHelper.Format(InterestCost, "###,##0.00")
            m_vQuoteArray(iRow, m_cArrangementFee) = StringsHelper.Format(ArrangementFee, "###,##0.00")
            m_vQuoteArray(iRow, m_cDepositPercent) = StringsHelper.Format(DepositPCent, "##0.00")
            m_vQuoteArray(iRow, m_cMinFinanceCharge) = StringsHelper.Format(MinFinanceCharge, "###,##0.00")
            m_vQuoteArray(iRow, m_cPayProtection) = PayProtection
            m_vQuoteArray(iRow, m_cQDocPath) = sQDocPath
            m_vQuoteArray(iRow, m_cQDocName) = sQDocName
            m_vQuoteArray(iRow, m_cBDocPath) = sBDocPath
            m_vQuoteArray(iRow, m_cBDocName) = sBDocName
            m_vQuoteArray(iRow, m_cCompanyNo) = CompanyNo
            m_vQuoteArray(iRow, m_cCDocPath) = sCDocPath
            m_vQuoteArray(iRow, m_cCdocName) = sCDocName
            m_vQuoteArray(iRow, m_cClientId) = sClientRef
            m_vQuoteArray(iRow, m_cPaymentMethod) = sPaymentMethod
            m_vQuoteArray(iRow, m_cAutoGenPlanRef) = vAutoGenPlanRef
            m_vQuoteArray(iRow, m_cFinCollPlanRef) = vFinCollPlanRef
            m_vQuoteArray(iRow, m_cPremFinCnt) = vPremFinCnt
            m_vQuoteArray(iRow, m_cPolicyCnt) = vPolicyCnt

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Calculator Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Calculator", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetRateInfo
    '
    ' Description:
    '
    ' History: 19/04/2000 IAC - Created.
    '
    ' ***************************************************************** '
    Public Function GetRateInfo() As Integer
        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode
        Dim sSqlString, sSqlName As String
        Dim bStoredProc, bKeepNulls As Boolean
        Dim iFirstRow As Integer
        Dim vResultArray(,) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSqlString = "SELECT enddate, daysdelay, noofinstall, depositreq, " & _
                         "depositpc, allowprotection, protectrate, mininterest, " & _
                         "min1, max1, rate1, apr1, min2, max2, rate2, apr2, arrangementfee " & _
                         "FROM GIS_PFRF " & _
                         "WHERE companyno = '" & CStr(CompanyNo) & "' " & _
                         "AND schemeno = '" & CStr(SchemeNo) & "' " & _
                         "AND schemeversion = '" & CStr(SchemeVer) & "' " & _
                         "AND startdate = '" & CDate(StartDate).ToString("yyyy/MM/dd") & "' " & _
                         "AND busclass = '" & ProdClass & "'"

            sSqlName = "Ret Schemes"
            bStoredProc = False
            bKeepNulls = True

            lResult = m_oDbConn.SQLSelect(sSqlString, sSqlName, bStoredProc, , vResultArray, , , , bKeepNulls)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                'Return error to calling object/module
                Return lResult
            End If

            'Populate module variables for use in quote calculation

            iFirstRow = vResultArray.GetLowerBound(0)



            If CStr(vResultArray(iFirstRow, 0)) <> "" Then


                Dim auxVar As Object = vResultArray(iFirstRow, 0)


                If Convert.IsDBNull(auxVar) Or IsNothing(auxVar) Or Object.Equals(vResultArray(iFirstRow, 0), Nothing) Or CStr(vResultArray(iFirstRow, 0)) = "" Then
                    'Do NOT assign any value to enddate as field does NOT contain a vaild date
                Else

                    EndDate = CDate(vResultArray(iFirstRow, 0)).ToString("dd/MM/yyyy")
                End If
            Else
                'Do NOT assign any value to enddate as field does NOT contain a vaild date
            End If


            DaysDelay = CInt(vResultArray(iFirstRow + 1, 0))

            NoOfInstallments = CInt(vResultArray(iFirstRow + 2, 0))

            DepositReq = CStr(vResultArray(iFirstRow + 3, 0))


            If CStr(vResultArray(iFirstRow + 4, 0)) <> "" Then

                Dim auxVar_2 As Object = vResultArray(iFirstRow + 4, 0)


                If Convert.IsDBNull(auxVar_2) Or IsNothing(auxVar_2) Or Object.Equals(vResultArray(iFirstRow + 4, 0), Nothing) Then
                    DepositPCent = 0
                Else

                    DepositPCent = CDbl(vResultArray(iFirstRow + 4, 0))
                End If
            Else
                Deposit = 0
            End If


            PayProtection = CStr(vResultArray(iFirstRow + 5, 0))


            If CStr(vResultArray(iFirstRow + 6, 0)) <> "" Then

                Dim auxVar_3 As Object = vResultArray(iFirstRow + 6, 0)


                If Convert.IsDBNull(auxVar_3) Or IsNothing(auxVar_3) Or Object.Equals(vResultArray(iFirstRow + 6, 0), Nothing) Then
                    PayProtectPCent = 0
                Else

                    PayProtectPCent = CDbl(vResultArray(iFirstRow + 6, 0))
                End If
            Else
                PayProtectPCent = 0
            End If


            MinFinanceCharge = CDbl(vResultArray(iFirstRow + 7, 0))

            ''  'Decide which interest rate and apr% to use
            ''  If UCase(DepositReq) = "Y" And DepositPCent > 0 Then
            ''      'calc deposit & remove from amount to be financed
            ''      Deposit = (AmountToFinance * (DepositPCent / 100))
            ''  Else
            ''      Deposit = 0
            ''  End If

            NetAmount = AmountToFinance - Deposit

            'Verify Financed Amount is within both value range(s).  If Not then return
            'with unable to premium finance amount



            If AmountToFinance >= CDbl(vResultArray(iFirstRow + 8, 0)) And AmountToFinance <= CDbl(vResultArray(iFirstRow + 9, 0)) Then

                IntRate = CDbl(vResultArray(iFirstRow + 10, 0))

                APR = CDbl(vResultArray(iFirstRow + 11, 0))
            Else


                If AmountToFinance >= CDbl(vResultArray(iFirstRow + 12, 0)) And AmountToFinance <= CDbl(vResultArray(iFirstRow + 13, 0)) Then

                    IntRate = CDbl(vResultArray(iFirstRow + 14, 0))

                    APR = CDbl(vResultArray(iFirstRow + 15, 0))
                Else
                    Return m_cNoFinanceRate
                End If
            End If


            ArrangementFee = CInt(vResultArray(iFirstRow + 16, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRateInfo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRateInfo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetAvailableSchemes
    '
    ' Description:
    '
    ' History: 11/09/2000 CJB - Created.
    '
    ' ***************************************************************** '
    Public Function GetAvailableSchemes(ByRef vProductFamily() As Object, ByRef vBusinessTypeCode As String, ByRef vDataModelCode As String, ByRef vTransactionType As String, ByRef vPaymentMethod As String, ByRef vStartDate As String, Optional ByRef vNoOfInstalments As String = "", Optional ByRef vActionType As Object = Nothing, Optional ByRef vResultArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode
        Dim sSqlName, sSqlString As String
        Dim sProdClassString As New StringBuilder
        Dim bStoredProc, bKeepNulls As Boolean
        Dim iFirstRow As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_bUseFlipArray = Information.IsNothing(vActionType)

            'Format dates to correct format
            Dim TempDate As Date
            vStartDate = IIf(DateTime.TryParse(vStartDate, TempDate), TempDate.ToString("yyyy/MM/dd"), vStartDate)

            'Build product class search string
            iFirstRow = vProductFamily.GetLowerBound(0)

            sProdClassString = New StringBuilder("AND (productfamily = '" & CStr(vProductFamily(iFirstRow)) & "'")
            For iCount As Integer = vProductFamily.GetLowerBound(0) + 1 To vProductFamily.GetUpperBound(0)

                sProdClassString.Append(" OR productfamily = '" & _
                                        CStr(vProductFamily(iCount)) & "'")
            Next iCount

            sProdClassString.Append(") ")

            sSqlString = "SELECT companyname, schemename, schemeversion, " & _
                         "startdate, companyno, schemeno, " & _
                         "productfamily, basisofcalc, " & _
                         "qdocpath, qdocname, bdocpath, bdocname, cdocpath, cdocname, " & _
                         "enddate, paymentmethod FROM gis_pfscheme " & _
                         "WHERE quoteableind = 'Y' " & _
                         "AND startdate <= '" & vStartDate & "' " & _
                         "AND (enddate is NULL " & _
                         "OR enddate >= '" & vStartDate & "') " & sProdClassString.ToString() & _
                         "AND paymentmethod = '" & vPaymentMethod & "' " & _
                         "AND transactiontype = '" & vTransactionType & "' " & _
                         "AND datamodelcode = '" & vDataModelCode & "' " & _
                         "AND businesstypecode = '" & vBusinessTypeCode & "' "

            If vNoOfInstalments <> "" Then
                sSqlString = sSqlString & "AND noofinstallments = '" & vNoOfInstalments & "'"
            End If

            sSqlName = "Retrieve Schemes"
            bStoredProc = False
            bKeepNulls = True

            lResult = m_oDbConn.SQLSelect(sSqlString, sSqlName, bStoredProc, , vResultArray, , , , bKeepNulls)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                'Return error to calling object/module
                Return lResult
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAvailableSchemes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAvailableSchemes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetQuoteDetails
    '
    ' Description: This function returns 1 row of data for the client
    '               to enable the re-printing of premium finance
    '               documents
    '
    ' History: 15/05/2000 IAC - Created.
    '
    ' ***************************************************************** '
    Public Function GetQuoteDetails(ByRef vClientKey As Array) As Integer
        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode
        Dim sSqlString, sSqlName As String
        Dim bStoredProc As Boolean
        Dim vResultArray As Array
        Dim iPremFinCnt As Integer
        'Dimension temporary variables for Broker details

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Build SQL String to get all of the fields from PFPremiumFinance

            sSqlString = "SELECT clientid, companyno, schemeno, schemeversion, startdate, " & _
                         "clientname, clientaddr1, clientaddr2, clientaddr3, clientaddr4, clientpcode, " & _
                         "transtype, totalcost, apr, noofinstallments, othinstallments, interestcost, " & _
                         "costofprotection, amounttofinance, bankname, bankbranch, bankpcode, " & _
                         "bankaccountname, bankaccountno, banksortcode, " & _
                         "quotedocpath, quotedocname, bankdocpath, bankdocname, " & _
                         "cdtagreedocpath, cdtagreedocname, autogeneratedplanref " & _
                         "FROM PFPremiumFinance " & _
                         "WHERE pfprem_finance_cnt = '" & CStr(vClientKey(0, 10)) & "'"

            sSqlName = "Retrieve Quote From PFPremiumFinance"
            bStoredProc = False

            lResult = m_oDbConn.SQLSelect(sSqlString, sSqlName, bStoredProc, , vResultArray)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                'Return error to calling object/module
                Return lResult
            End If

            'Prepare vClientKey to return an array sized to m_cMainArray, which allows for future
            'enhancements and compatability with the other functions.

            iPremFinCnt = CInt(vClientKey(0, 10))
            vClientKey = Nothing
            vClientKey = Array.CreateInstance(GetType(Object), New Integer() {1, m_cMainArray + 1}, New Integer() {0, 0})

            'Flip the array into row/column format
            lResult = CType(gPMFunctions.FlipArray(vResultArray), gPMConstants.PMEReturnCode)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            'Build vClientKey from vResultArray

            vClientKey(0, m_cClientId) = vResultArray(0, 0)

            vClientKey(0, m_cCompanyNo) = vResultArray(0, 1)

            vClientKey(0, m_cSchemeNo) = vResultArray(0, 2)

            vClientKey(0, m_cSchemeVer) = vResultArray(0, 3)

            vClientKey(0, m_cStartDate) = vResultArray(0, 4)

            vClientKey(0, m_cClient) = vResultArray(0, 5)

            vClientKey(0, m_cClntAddr1) = vResultArray(0, 6)

            vClientKey(0, m_cClntAddr2) = vResultArray(0, 7)

            vClientKey(0, m_cClntAddr3) = vResultArray(0, 8)

            vClientKey(0, m_cClntAddr4) = vResultArray(0, 9)

            vClientKey(0, m_cClntPCode) = vResultArray(0, 10)

            vClientKey(0, m_cTransType) = vResultArray(0, 11)

            vClientKey(0, m_cTotalCost) = vResultArray(0, 12)

            vClientKey(0, m_cAPR) = vResultArray(0, 13)

            vClientKey(0, m_cNoOfInstalments) = vResultArray(0, 14)

            vClientKey(0, m_cOthInstalments) = vResultArray(0, 15)

            vClientKey(0, m_cInterestCost) = vResultArray(0, 16)

            vClientKey(0, m_cCostOfProtect) = vResultArray(0, 17)

            vClientKey(0, m_cAmountToFinance) = vResultArray(0, 18)

            vClientKey(0, m_cBankName) = vResultArray(0, 19)

            vClientKey(0, m_cBankBranch) = vResultArray(0, 20)

            vClientKey(0, m_cBankPCode) = vResultArray(0, 21)

            vClientKey(0, m_cBankAccountName) = vResultArray(0, 22)

            vClientKey(0, m_cBankAccountNo) = vResultArray(0, 23)

            vClientKey(0, m_cBankSortCode) = vResultArray(0, 24)

            vClientKey(0, m_cQDocPath) = vResultArray(0, 25)

            vClientKey(0, m_cQDocName) = vResultArray(0, 26)

            vClientKey(0, m_cBDocPath) = vResultArray(0, 27)

            vClientKey(0, m_cBDocName) = vResultArray(0, 28)

            vClientKey(0, m_cCDocPath) = vResultArray(0, 29)

            vClientKey(0, m_cCdocName) = vResultArray(0, 30)

            vClientKey(0, m_cPremFinCnt) = iPremFinCnt

            vClientKey(0, m_cAutoGenPlanRef) = vResultArray(0, 31)

            'Retrieve Broker details and append onto the vClientArray
            'This bit of code will need ajustments made to the field names when
            'the next version of the sbo database is installed.
            lResult = CType(RetrieveBroker(vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            If Information.IsArray(vResultArray) Then

                'Setup Broker details

                vClientKey(0, m_cBrkName) = vResultArray(0, 0)

                vClientKey(0, m_cBrkAddr1) = vResultArray(0, 1)

                vClientKey(0, m_cBrkAddr2) = vResultArray(0, 2)

                vClientKey(0, m_cBrkAddr3) = vResultArray(0, 3)

                vClientKey(0, m_cBrkPCode) = vResultArray(0, 4)

                vClientKey(0, m_cBrkAddr4) = vResultArray(0, 5)

            End If

            'lResult = oSource.GetNext(vDescription:=vBrokerName, _
            'vAddress1:=vBrokerAddr1, _
            'vAddress2:=vBrokerAddr2, _
            'vAddress3:=vBrokerAddr3, _
            'vPostalCode:=vBrokerPCode, _
            'vAddress4:=vBrokerAddr4)

            vResultArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQuoteDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQuoteDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PrintQuote
    '
    ' Description:
    '
    ' History: 08/05/2000 IAC - Created.
    '
    ' ***************************************************************** '
    Public Function PrintQuote(ByVal vQuoteDetails As Array) As Integer
        Dim result As Integer = 0
        Dim oMSWord As Word.Application
        Dim sDocName As String = ""
        Dim sDocPath As New StringBuilder
        Dim vBlankField As Object 'This is for future use
        Dim lResult As gPMConstants.PMEReturnCode
        Dim sSourceDoc, sDestinDoc As String
        Dim vPolicyCnt As Object
        Dim iPremFinCnt As Integer
        Dim vPolicyDets, vInsurer As Object
        Dim vCoverType As String = ""
        Dim iNoOfPolicies As Integer
        Dim vInsurerArray(,) As Object
        Dim vResultArray(,) As Object
        Dim lDocSuffix As Integer
        Dim vDocumentName As String = ""


        Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_PrintQuote)")

        result = gPMConstants.PMEReturnCode.PMTrue

        'Decide whether need to retrieve data from premium finance table
        If vQuoteDetails.GetUpperBound(1) < m_cMainArray Then
            lResult = CType(GetQuoteDetails(vQuoteDetails), gPMConstants.PMEReturnCode)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If
        Else
            'Retrieve Broker details and append onto the vClientArray
            'This bit of code will need ajustments made to the field names when
            'the next version of the sbo database is installed.
            vResultArray = Nothing

            lResult = CType(RetrieveBroker(vResultArray:=vResultArray), gPMConstants.PMEReturnCode)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            If Information.IsArray(vResultArray) Then
                'Populate vQuoteDetails with broker details


                vQuoteDetails(0, m_cBrkName) = vResultArray(0, 0)


                vQuoteDetails(0, m_cBrkAddr1) = vResultArray(0, 1)


                vQuoteDetails(0, m_cBrkAddr2) = vResultArray(0, 2)


                vQuoteDetails(0, m_cBrkAddr3) = vResultArray(0, 3)


                vQuoteDetails(0, m_cBrkPCode) = vResultArray(0, 4)


                vQuoteDetails(0, m_cBrkAddr4) = vResultArray(0, 5)
            End If
        End If

        If m_bUseFlipArray Then
            'Call Array Flipper Here!
            lResult = CType(gPMFunctions.FlipArray(vQuoteDetails), gPMConstants.PMEReturnCode)
            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If
        End If


        vBlankField = Nothing

        For iNoOfSchemes As Integer = vQuoteDetails.GetLowerBound(0) To vQuoteDetails.GetUpperBound(0)
            'Policy cnt from pfpolicy_code

            iPremFinCnt = CInt(vQuoteDetails(iNoOfSchemes, m_cPremFinCnt))

            'Test to see if partially created information is required
            If iPremFinCnt <> 0 Then
                'Retrieve policy details

                lResult = CType(GetPolicyCount(iPremFinCnt:=iPremFinCnt, vPolicyCnt:=vPolicyCnt), gPMConstants.PMEReturnCode)

                If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lResult
                End If
            Else
                'set no of policies to 0 elements.  This must be an array to allow
                'any further processing to occur.
                ReDim vPolicyCnt(0, 0)
            End If

            'Find the number of policies returned

            iNoOfPolicies = vPolicyCnt.GetUpperBound(0) + 1
            'Dimension vInsurerArray for the number of returned policies max 5 columns (0 thru 4)
            ReDim vInsurerArray(iNoOfPolicies - 1, 4)


            For iNoOfPolicies = vPolicyCnt.GetLowerBound(0) To vPolicyCnt.GetUpperBound(0)
                'Get policy details

                lResult = CType(GetPolicyDetails(vPartyCnt:=vPolicyCnt(iNoOfPolicies, 0), vPolicyDets:=vPolicyDets), gPMConstants.PMEReturnCode)

                If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lResult
                End If

                Select Case Information.IsArray(vPolicyDets)
                    Case True
                        ReDim vInsurer(0, 2)


                        vInsurer(0, 0) = vPolicyDets(0, 3)

                        vInsurer(0, 1) = "7" 'Default for Insurer record type


                        vInsurer(0, 2) = vPolicyDets(0, 4)


                        lResult = CType(GetInsurerDetails(vInsurer), gPMConstants.PMEReturnCode)

                        If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return lResult
                        End If


                        vCoverType = CStr(vPolicyDets(0, 6))

                        lResult = CType(GetPolicyCoverType(vCoverType), gPMConstants.PMEReturnCode)

                        If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return lResult
                        End If



                        vInsurerArray(iNoOfPolicies, 2) = vPolicyDets(0, 8)


                        vInsurerArray(iNoOfPolicies, 3) = vPolicyDets(0, 0)


                        vInsurerArray(iNoOfPolicies, 4) = vPolicyDets(0, 7)
                    Case Else

                        vInsurerArray(iNoOfPolicies, 2) = ""

                        vInsurerArray(iNoOfPolicies, 3) = ""

                        vInsurerArray(iNoOfPolicies, 4) = ""
                End Select

                'Append returned data to vInsurerArray
                If Information.IsArray(vInsurer) Then


                    vInsurerArray(iNoOfPolicies, 0) = vInsurer(0, 0)
                Else

                    vInsurerArray(iNoOfPolicies, 0) = ""
                End If


                vInsurerArray(iNoOfPolicies, 1) = vCoverType

            Next iNoOfPolicies

            oMSWord = New Word.Application()

            'copy file to temp file
            'developer guide no. 114 (latest guide)

            sDocPath = New StringBuilder(vQuoteDetails(iNoOfSchemes, m_cQDocPath).ToString.Trim())

            sDocName = CStr(vQuoteDetails(iNoOfSchemes, m_cQDocName)).Trim()

            'Make sure that there is a / at the end of the paths
            If Not sDocPath.ToString().EndsWith("\") Then
                sDocPath.Append("\")
            End If

            
            'Reason for Mod: Generate a random number using the system date & time to apply
            '                as a suffix to the document filename

            'Create Unique Document Suffix
            lResult = CType(DocRndNumberGen(lDocSuffix:=lDocSuffix), gPMConstants.PMEReturnCode)

            'End of Modification IC 18/07/2000

            sSourceDoc = sDocPath.ToString() & sDocName
            sDestinDoc = sDocPath.ToString() & "QuoteDetailsTemp" & CStr(lDocSuffix) & ".doc"
            vDocumentName = "QuoteDetailsTemp" & lDocSuffix & ".doc"

            File.Copy(sSourceDoc, sDestinDoc)

            oMSWord.Documents.Open(FileName:=sDestinDoc)

            'send the parameters

            oMSWord.Documents.Item(sDestinDoc).PrintQuoteDetails(vQuoteDetails(iNoOfSchemes, m_cClient), vQuoteDetails(iNoOfSchemes, m_cClntAddr1), vQuoteDetails(iNoOfSchemes, m_cClntAddr2), vQuoteDetails(iNoOfSchemes, m_cClntAddr3), vQuoteDetails(iNoOfSchemes, m_cClntAddr4), vQuoteDetails(iNoOfSchemes, m_cClntPCode), vQuoteDetails(iNoOfSchemes, m_cBrkName), vQuoteDetails(iNoOfSchemes, m_cBrkAddr1), vQuoteDetails(iNoOfSchemes, m_cBrkAddr2), vQuoteDetails(iNoOfSchemes, m_cBrkAddr3), vQuoteDetails(iNoOfSchemes, m_cBrkAddr4), vQuoteDetails(iNoOfSchemes, m_cBrkPCode), vQuoteDetails(iNoOfSchemes, m_cTransType), vBlankField, vQuoteDetails(iNoOfSchemes, m_cTotalCost), vQuoteDetails(iNoOfSchemes, m_cAPR), vQuoteDetails(iNoOfSchemes, m_cNoOfInstalments), vQuoteDetails(iNoOfSchemes, m_cOthInstalments), vQuoteDetails(iNoOfSchemes, m_cInterestCost), vQuoteDetails(iNoOfSchemes, m_cCostOfProtect), vQuoteDetails(iNoOfSchemes, m_cAmountToFinance), vInsurerArray, vDocumentName)

            'print the word document here
            oMSWord.PrintOut(Background:=False, FileName:=sDestinDoc)
            oMSWord.Documents.Close(SaveChanges:=False)

            'close the document
            'oMSWord.Application.Quit

            'kill word object
            Try
                File.Delete(sDestinDoc)

            Catch
            End Try

            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_PrintQuote)")

            oMSWord = Nothing

            'Print Direct Debit instructions
            lResult = CType(PrintBankDetails(vQuoteDetails, iNoOfSchemes), gPMConstants.PMEReturnCode)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            'Print Credit agreement
            lResult = CType(PrintCreditAgreement(vQuoteDetails, iNoOfSchemes), gPMConstants.PMEReturnCode)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

        Next iNoOfSchemes

        Return result

Err_PrintQuote:

        result = Information.Err().Number

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintQuote", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: PrintBankDetails
    '
    ' Description:
    '
    ' History: 10/05/2000 IAC - Created.
    '
    ' ***************************************************************** '
    Public Function PrintBankDetails(ByRef vBankDetails As Array, Optional ByRef vArrayRef As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim oMSWord As Object
        Dim sDocName, sDocPath, sSourceDoc, sDestinDoc As String
        Dim lResult As gPMConstants.PMEReturnCode
        Dim lDocSuffix As Integer
        Dim vDocumentName As String = ""


        Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_PrintBankDetails)")

        result = gPMConstants.PMEReturnCode.PMTrue

        If m_bUseFlipArray Then
            'Call Array Flipper Here!
            lResult = CType(gPMFunctions.FlipArray(vBankDetails), gPMConstants.PMEReturnCode)
            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If
        End If


        If Information.IsNothing(vArrayRef) Then
            vArrayRef = 0
        End If

        'need to get the path and the document name from the array

        sDocName = CStr(vBankDetails(vArrayRef, m_cBDocName)).Trim()

        sDocPath = CStr(vBankDetails(vArrayRef, m_cBDocPath)).Trim()

        'Make sure that there is a \ at the end of the path
        If Not sDocPath.EndsWith("\") Then
            sDocPath = sDocPath & "\"
        End If

        
        'Start of Modification IC 18/07/2000
        'Reason for Mod: Generate a random number using the system date & time to apply
        '                as a suffix to the document filename

        'Create Unique Document Suffix
        lResult = CType(DocRndNumberGen(lDocSuffix:=lDocSuffix), gPMConstants.PMEReturnCode)

        'End of Modification IC 18/07/2000

        sSourceDoc = sDocPath & sDocName
        sDestinDoc = sDocPath & "ClientDetailsTemp" & CStr(lDocSuffix) & ".doc"
        vDocumentName = "ClientDetailsTemp" & lDocSuffix & ".doc"

        File.Copy(sSourceDoc, sDestinDoc)

        'start instance of word app
        'Set oMSWord = CreateObject("Word.Application")
        oMSWord = System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application")

        'open required doc

        oMSWord.Documents.Open(filename:=sDestinDoc)

        ' init is a subroutine defined in the VBA of the target doc

        oMSWord.Documents(sDestinDoc).PrintClientDetails(vBankDetails(vArrayRef, m_cBankName), vBankDetails(vArrayRef, m_cBankBranch), vBankDetails(vArrayRef, m_cBankPCode), vBankDetails(vArrayRef, m_cBankAccountName), vBankDetails(vArrayRef, m_cBankAccountNo), vBankDetails(vArrayRef, m_cBankSortCode), vBankDetails(vArrayRef, m_cClient), vBankDetails(vArrayRef, m_cClntPCode), vBankDetails(vArrayRef, m_cAutoGenPlanRef), vDocumentName)


        ' print, then close it

        oMSWord.PrintOut(Background:=False, filename:=sDestinDoc)

        oMSWord.Documents.Close(SaveChanges:=False)

        ' done
        'oMSWord.Application.Quit

        'Delete the temporary file
        Try
            File.Delete(sDestinDoc)

        Catch
        End Try

        Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_PrintBankDetails)")

        'Kill the word object
        oMSWord = Nothing

        Return result

Err_PrintBankDetails:

        result = Information.Err().Number

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintBankDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintBankDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: PrintCreditAgreement
    '
    ' Description:
    '
    ' History: 24/05/2000 IAC - Created.
    '
    ' ***************************************************************** '
    Public Function PrintCreditAgreement(ByRef vCreditDetails As Array, Optional ByRef vArrayRef As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim oMSWord As Object
        Dim sDocName, sDocPath, sSourceDoc, sDestinDoc As String
        Dim lResult As gPMConstants.PMEReturnCode
        Dim lDocSuffix As Integer
        Dim vDocumentName As String = ""


        Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_PrintCreditAgreement)")

        result = gPMConstants.PMEReturnCode.PMTrue

        If m_bUseFlipArray Then
            'Call Array Flipper Here!
            lResult = CType(gPMFunctions.FlipArray(vCreditDetails), gPMConstants.PMEReturnCode)
            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If
        End If


        If Information.IsNothing(vArrayRef) Then
            vArrayRef = 0
        End If

        'need to get the path and the document name from the array

        sDocName = CStr(vCreditDetails(vArrayRef, m_cCdocName)).Trim()

        sDocPath = CStr(vCreditDetails(vArrayRef, m_cCDocPath)).Trim()

        If Not sDocPath.EndsWith("\") Then
            sDocPath = sDocPath & "\"
        End If

       
        'Start of Modification IC 18/07/2000
        'Reason for Mod: Generate a random number using the system date & time to apply
        '                as a suffix to the document filename

        'Create Unique Document Suffix
        lResult = CType(DocRndNumberGen(lDocSuffix:=lDocSuffix), gPMConstants.PMEReturnCode)

        'End of Modification IC 18/07/2000

        sSourceDoc = sDocPath & sDocName
        sDestinDoc = sDocPath & "creditdetailstemp" & CStr(lDocSuffix) & ".doc"
        vDocumentName = "creditdetailstemp" & lDocSuffix & ".doc"

        File.Copy(sSourceDoc, sDestinDoc)

        'start instance of word app
        'Set oMSWord = CreateObject("Word.Application")
        oMSWord = System.Runtime.InteropServices.Marshal.GetActiveObject("Word.Application")

        'open required doc

        oMSWord.Documents.Open(filename:=sDestinDoc)

        ' init is a subroutine defined in the VBA of the target doc

        oMSWord.Documents(sDestinDoc).PrintCreditAgreement(vCreditDetails(vArrayRef, m_cClient), vCreditDetails(vArrayRef, m_cClntAddr1), vCreditDetails(vArrayRef, m_cClntAddr2), vCreditDetails(vArrayRef, m_cClntAddr3), vCreditDetails(vArrayRef, m_cClntAddr4), vCreditDetails(vArrayRef, m_cClntPCode), vCreditDetails(vArrayRef, m_cBrkName), vCreditDetails(vArrayRef, m_cBrkAddr1), vCreditDetails(vArrayRef, m_cBrkAddr2), vCreditDetails(vArrayRef, m_cBrkAddr3), vCreditDetails(vArrayRef, m_cBrkAddr4), vCreditDetails(vArrayRef, m_cBrkPCode), vCreditDetails(vArrayRef, m_cAPR), vDocumentName)


        ' print, then close it

        oMSWord.PrintOut(Background:=False, filename:=sDestinDoc)

        oMSWord.Documents.Close(SaveChanges:=False)

        ' done

        oMSWord.Application.Quit()

        'Delete the temporary file
        Try
            File.Delete(sDestinDoc)

        Catch
        End Try

        Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_PrintCreditAgreement)")

        'Kill the word object
        oMSWord = Nothing

        Return result

Err_PrintCreditAgreement:

        result = Information.Err().Number

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintCreditAgreement Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintCreditAgreement", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: InsertNewRecord
    '
    ' Description:
    '
    ' History: 10/05/2000 IAC - Created.
    '
    ' ***************************************************************** '
    Public Function InsertNewRecord(ByRef vNewRecord As Array) As Integer
        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode
        Dim sSqlString, sSqlName, sStatusStart As String
        Dim bStoredProc As Boolean
        Dim iPFCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Retrieve next sequential count from pfpremimfinance
            lResult = CType(GetNextPFCount(iPFCount), gPMConstants.PMEReturnCode)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            'Convert all date formats to yyyy/mm/dd from dd/mm/yyyy


            vNewRecord(0, m_cStartDate) = CDate(vNewRecord(0, m_cStartDate)).ToString("yyyy/MM/dd")


            If Not (Convert.IsDBNull(vNewRecord(0, m_cEndDate)) Or IsNothing(vNewRecord(0, m_cEndDate))) Then


                vNewRecord(0, m_cEndDate) = CDate(vNewRecord(0, m_cEndDate)).ToString("yyyy/MM/dd")
            End If

            sStatusStart = "010"

            'Build Sql String from flipped array
            sSqlString = "INSERT INTO PFPremiumFinance "

            'Fields to be inserted.
            sSqlString = sSqlString & "(pfprem_finance_cnt, clientid , companyno, schemeno, schemeversion, startdate, " & _
                         "companyname, schemename, productclass, transtype, amounttofinance, apr, " & _
                         "interestrate, daysdelay, noofinstallments, firstinstallment, othinstallments, " & _
                         "costofprotection, deposit, netamount, totalcost, interestcost, minfinancechrge, " & _
                         "payprotection, quotedocpath, quotedocname, bankdocpath, bankdocname, clientname, " & _
                         "clientaddr1, clientaddr2, clientaddr3, clientpcode, clientaddr4, " & _
                         "statusind, cdtagreedocpath, cdtagreedocname, clientcode, " & _
                         "paymentmethod, enddate, autogeneratedplanref, " & _
                         "financecollatedplanref " & ")"

            'Values to be inserted into above fields.
            sSqlString = sSqlString & " VALUES ("

            sSqlString = sSqlString & "{pfPremFinance_Cnt}, " & "{ClientId}, " & "{CompanyNo}, " & "{SchemeNo}, " & _
                         "{SchemeVer}, " & _
                         "{StartDate}, " & "{CompanyName}, " & "{SchemeName}, " & "{ProdClass}, " & "{TransType}, " & _
                         "{AmtToFinance}, " & "{APR}, " & "{IntRate}, " & "{DaysDelay}, " & "{NoOfInstall}, " & _
                         "{FirstInstall}, " & "{OtherInstall}, " & "{CostOfPP}, " & "{Deposit}, " & "{NetAmount}, " & _
                         "{TotalCost}, " & "{InterestCost}, " & "{MinFinCharge}, " & "{PayProtect}, " & _
                         "{QDocPath}, " & "{QDocName}, " & "{BDocPath}, " & "{BDocName}, " & "{Client}, " & _
                         "{ClntAddr1}, " & "{ClntAddr2}, " & "{ClntAddr3}, " & "{ClntPCode}, " & "{ClntAddr4}, " & _
                         "{Status}, " & _
                         "{CDocPath}, " & "{CDocName}, " & "{ClientCode}, " & _
                         "{PaymentMethod}, " & "{EndDate}, " & _
                         "{AutoGenPlanRef}, " & "{FinCollPlanRef} " & ")"

            'Build Parameters Here!
            With m_oDbConn.Parameters
                .Clear()
                .Add("pfPremFinance_Cnt", CStr(iPFCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                .Add("ClientId", CStr(vNewRecord(0, m_cClientId)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("CompanyNo", CStr(CInt(vNewRecord(0, m_cCompanyNo))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                .Add("SchemeNo", CStr(CInt(vNewRecord(0, m_cSchemeNo))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                .Add("SchemeVer", CStr(CInt(vNewRecord(0, m_cSchemeVer))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                .Add("StartDate", CStr(vNewRecord(0, m_cStartDate)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)

                .Add("CompanyName", CStr(vNewRecord(0, m_cCompanyName)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("SchemeName", CStr(vNewRecord(0, m_cSchemeName)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("ProdClass", CStr(vNewRecord(0, m_cProdClass)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("TransType", CStr(vNewRecord(0, m_cTransType)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("AmtToFinance", CStr(CDbl(vNewRecord(0, m_cAmountToFinance))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                .Add("APR", CStr(CDbl(vNewRecord(0, m_cAPR))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                .Add("IntRate", CStr(CDbl(vNewRecord(0, m_cIntRate))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                .Add("DaysDelay", CStr(CInt(vNewRecord(0, m_cDaysDelay))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                .Add("NoOfInstall", CStr(CInt(vNewRecord(0, m_cNoOfInstalments))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                .Add("FirstInstall", CStr(CDbl(vNewRecord(0, m_cFirstInstalment))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                .Add("OtherInstall", CStr(CDbl(vNewRecord(0, m_cOthInstalments))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                .Add("CostOfPP", CStr(CDbl(vNewRecord(0, m_cCostOfProtect))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                .Add("Deposit", CStr(CDbl(vNewRecord(0, m_cDeposit))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                .Add("NetAmount", CStr(CDbl(vNewRecord(0, m_cNetAmount))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                .Add("TotalCost", CStr(CDbl(vNewRecord(0, m_cTotalCost))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                .Add("InterestCost", CStr(CDbl(vNewRecord(0, m_cInterestCost))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                .Add("MinFinCharge", CStr(CDbl(vNewRecord(0, m_cMinFinanceCharge))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                .Add("PayProtect", CStr(vNewRecord(0, m_cPayProtection)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("QDocPath", CStr(vNewRecord(0, m_cQDocPath)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("QDocName", CStr(vNewRecord(0, m_cQDocName)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("BDocPath", CStr(vNewRecord(0, m_cBDocPath)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("BDocName", CStr(vNewRecord(0, m_cBDocName)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("Client", CStr(vNewRecord(0, m_cClient)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("ClntAddr1", CStr(vNewRecord(0, m_cClntAddr1)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("ClntAddr2", CStr(vNewRecord(0, m_cClntAddr2)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("ClntAddr3", CStr(vNewRecord(0, m_cClntAddr3)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("ClntPCode", CStr(vNewRecord(0, m_cClntPCode)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("ClntAddr4", CStr(vNewRecord(0, m_cClntAddr4)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                .Add("Status", sStatusStart, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("CDocPath", CStr(vNewRecord(0, m_cCDocPath)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("CDocName", CStr(vNewRecord(0, m_cCdocName)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("PaymentMethod", CStr(vNewRecord(0, m_cPaymentMethod)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("EndDate", CStr(vNewRecord(0, m_cEndDate)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)

                .Add("AutoGenPlanRef", CStr(vNewRecord(0, m_cAutoGenPlanRef)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("FinCollPlanRef", CStr(vNewRecord(0, m_cFinCollPlanRef)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            End With

            bStoredProc = False
            sSqlName = "InsertNew Premium Finance Record"

            'Call SQL.Action from within PMDAO
            lResult = m_oDbConn.SQLAction(sSqlString, sSqlName, bStoredProc)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If


            'NowUpdate Child Table PFPolicy_Code
            sSqlString = "INSERT INTO PFPolicy_Code "

            sSqlString = sSqlString & _
                         "(pfprem_finance_cnt, pfpolicy_id) " & _
                         "VALUES ( " & _
                         "{PremFinance_Cnt}, " & "{Policy_Id} " & ")"

            With m_oDbConn.Parameters
                .Clear()
                .Add("PremFinance_Cnt", CStr(iPFCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                .Add("Policy_Id", CStr(CInt(vNewRecord(0, m_cPolicyCnt))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            End With

            sSqlName = "InsertNew PF Policy Code Record"

            'Call SQL.Action from within PMDAO
            lResult = m_oDbConn.SQLAction(sSqlString, sSqlName, bStoredProc)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InsertNewRecord Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InsertNewRecord", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateExistingRecord
    '
    ' Description:
    '
    ' History: 10/05/2000 IAC - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateExistingRecord(ByRef vExistingRecord As Object, ByRef vClientKey(,) As Object) As Integer
        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode
        Dim sSqlName, sSqlString, sStatusInd As String
        Dim bStoredProc As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set status for process
            sStatusInd = "011"

            'Build Sql String
            sSqlString = "UPDATE PFPremiumFinance " & _
                         "SET clientid = " & "{ClientID}, " & _
                         "clientname = " & "{Client}, " & _
                         "clientaddr1 = " & "{ClntAddr1}, " & _
                         "clientaddr2 = " & "{ClntAddr2}, " & _
                         "clientaddr3 = " & "{ClntAddr3}, " & _
                         "clientpcode = " & "{ClntPCode}, " & _
                         "clienttown = " & "{ClntTown}, " & _
                         "clientaddr4 = " & "{ClntAddr4}, " & _
                         "clientcountry = " & "{ClntCountry}, " & _
                         "clientareacode = " & "{ClntAreaCode}, " & _
                         "clientphoneno = " & "{ClntPhone}, " & _
                         "clientextension = " & "{ClntExtn}, " & _
                         "clientfaxareacode = " & "{ClntFaxCode}, " & _
                         "clientfaxno = " & "{ClntFaxNo}, " & _
                         "bankname = " & "{BankName}, " & _
                         "banksortcode = " & "{SortCode}, " & _
                         "bankaccountno = " & "{AccountNo}, " & _
                         "bankbranch = " & "{Branch}, " & _
                         "bankaddr1 = " & "{BankAddr1}, " & _
                         "bankaddr2 = " & "{BankAddr2}, " & _
                         "bankaddr3 = " & "{BankAddr3}, " & _
                         "banktown = " & "{BankTown}, " & _
                         "bankpcode = " & "{BankPCode}, " & _
                         "bankregion = " & "{BankAddr4}, "

            sSqlString = sSqlString & _
                         "bankareacode = " & "{BankAreaCode}, " & _
                         "bankcountry = " & "{BankCountry}, " & _
                         "bankaccountname = " & "{AccountName}, " & _
                         "bankphoneno = " & "{BankPhoneNo}, " & _
                         "bankextension = " & "{BankPhoneExtn}, " & _
                         "bankfaxareacode = " & "{BankFaxCode}, " & _
                         "bankfaxno = " & "{BankFaxNo}, " & _
                         "statusind = " & "{Status}, " & _
                         "autogeneratedplanref = " & "{AutoGenPlanRef}, " & _
                         "financecollatedplanref = " & "{FinCollPlanRef}, " & _
                         "clientcode = " & "{ClientCode} "


            'Build Parameters Here!
            With m_oDbConn.Parameters
                .Clear()

                .Add("ClientID", CStr(vExistingRecord(0, m_cClientId)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("Client", CStr(vExistingRecord(0, m_cClient)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("ClntAddr1", CStr(vExistingRecord(0, m_cClntAddr1)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("ClntAddr2", CStr(vExistingRecord(0, m_cClntAddr2)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("ClntAddr3", CStr(vExistingRecord(0, m_cClntAddr3)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("ClntPCode", CStr(vExistingRecord(0, m_cClntPCode)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("ClntTown", CStr(vExistingRecord(0, m_cClntAddr4)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("ClntAddr4", CStr(vExistingRecord(0, m_cClntRegion)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("ClntCountry", CStr(vExistingRecord(0, m_cClntCountry)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("ClntAreaCode", CStr(vExistingRecord(0, m_cClntAreaCode)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("ClntPhone", CStr(vExistingRecord(0, m_cClntPhone)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("ClntExtn", CStr(vExistingRecord(0, m_cClntExtn)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("ClntFaxCode", CStr(vExistingRecord(0, m_cClntFaxCode)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("ClntFaxNo", CStr(vExistingRecord(0, m_cClntFax)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("BankName", CStr(vExistingRecord(0, m_cBankName)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("SortCode", CStr(vExistingRecord(0, m_cBankSortCode)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("AccountNo", CStr(vExistingRecord(0, m_cBankAccountNo)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("Branch", CStr(vExistingRecord(0, m_cBankBranch)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("BankAddr1", CStr(vExistingRecord(0, m_cBankAddr1)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("BankAddr2", CStr(vExistingRecord(0, m_cBankAddr2)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("BankAddr3", CStr(vExistingRecord(0, m_cBankAddr3)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("BankTown", CStr(vExistingRecord(0, m_cBankTown)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("BankPCode", CStr(vExistingRecord(0, m_cBankPCode)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("BankAddr4", CStr(vExistingRecord(0, m_cBankAddr4)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("BankAreaCode", CStr(vExistingRecord(0, m_cBankAreaCode)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("BankCountry", CStr(vExistingRecord(0, m_cBankCountry)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("AccountName", CStr(vExistingRecord(0, m_cBankAccountName)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("BankPhoneNo", CStr(vExistingRecord(0, m_cBankPhoneNo)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("BankPhoneExtn", CStr(vExistingRecord(0, m_cBankPhoneExt)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("BankFaxCode", CStr(vExistingRecord(0, m_cBankFaxAreaCode)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("BankFaxNo", CStr(vExistingRecord(0, m_cBankFaxNo)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                .Add("Status", sStatusInd, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("AutoGenPlanRef", CStr(vExistingRecord(0, m_cAutoGenPlanRef)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                .Add("FinCollPlanRef", CStr(vExistingRecord(0, m_cFinCollPlanRef)).Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            End With


            sSqlString = sSqlString & " WHERE pfprem_finance_cnt = '" & CStr(CInt(vClientKey(0, 10))) & "' "

            'Update existing record
            bStoredProc = False
            sSqlName = "UpdateExistingRecord"

            lResult = m_oDbConn.SQLAction(sSqlString, sSqlName, bStoredProc)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            'NowUpdate Child Table PFPolicy_Code
            sSqlString = "UPDATE PFPolicy_Code "


            sSqlString = sSqlString & _
                         "SET pfpolicy_id = " & "{PolicyId} " & _
                         "WHERE pfprem_finance_cnt = '" & CStr(CInt(vClientKey(0, 10))) & "' "


            With m_oDbConn.Parameters
                .Clear()

                .Add("PolicyId", CStr(CInt(vExistingRecord(0, m_cPolicyCnt))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            End With

            sSqlName = "Update PF Policy Code Record"

            'Call SQL.Action from within PMDAO
            lResult = m_oDbConn.SQLAction(sSqlString, sSqlName, bStoredProc)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateExistingRecord Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateExistingRecord", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 09/05/2000 IAC - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise(Optional ByRef sUsername As String = "", Optional ByRef sPassword As String = "", Optional ByRef iUserID As Integer = 0, Optional ByRef iSourceID As Integer = 0, Optional ByRef iLanguageID As Integer = 0, Optional ByRef iCurrencyID As Integer = 0, Optional ByRef iLogLevel As Integer = 0, Optional ByRef sCallingAppName As String = "", Optional ByRef vDatabase As Object = Nothing, Optional ByRef sDataModelCode As String = "") As Integer



        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode
        Dim bNew As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            'Initialise Objects
            m_oDbConn = New dPMDAO.Database()


            lResult = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDbConn, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Open database connection using 'SIRIUS' privilages.
            'Password $1R1U5

            'lResult = m_oDbConn.OpenDatabase(, , PMSiriusSolutionsDSN)

            'Open database using DSN registry setting for the specified data model code
            '(or generic GIS DSN if no solution specific reg key found)
            lResult = CType(GISSharedConstants.CheckGISDSN(v_sDataModelCode:=sDataModelCode, r_oDatabase:=m_oDbConn, r_bNew:=bNew), gPMConstants.PMEReturnCode)
            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Set Username and Password

            ' Set the ProcessMode etc.
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 09/05/2000 IAC - Created.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then

                m_oDbConn.CloseDatabase()

                m_oDbConn = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: Class_Initialize
    '
    ' Description:
    '
    ' History: 11/09/2000 CJB - Created.
    '
    ' ***************************************************************** '
    Public Sub New()
        MyBase.New()



        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: Class_Terminate
    '
    ' Description:
    '
    ' History: 11/09/2000 CJB - Created.
    '
    ' ***************************************************************** '
    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    '
    ' Name: GetPartyId
    '
    ' Description:
    '
    ' History: 30/05/2000 IAC - Created.
    '
    ' ***************************************************************** '
    Public Function GetPartyId(ByRef vPartyCnt As String, ByRef bUsePartyCnt As Boolean, ByRef vPartyId As Array) As Integer
        Dim result As Integer = 0
        Dim sSqlString, sSqlName As String
        Dim bStoredProc, bKeepNull As Boolean
        Dim lResult As gPMConstants.PMEReturnCode


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get ClientId from the Party Table
            sSqlString = "SELECT party_id, shortname from party "

            If bUsePartyCnt Then
                sSqlString = sSqlString & "WHERE party_cnt = '" & vPartyCnt & "'"
            Else
                sSqlString = sSqlString & "WHERE party_cnt = '" & vPartyCnt & "'"
            End If

            sSqlName = "Get Client Id"
            bStoredProc = False
            bKeepNull = True

            lResult = m_oDbConn.SQLSelect(sSqlString, sSqlName, bStoredProc, , vPartyId, , , , bKeepNull)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            If Information.IsArray(vPartyId) Then
                lResult = CType(gPMFunctions.FlipArray(vPartyId), gPMConstants.PMEReturnCode)

                If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lResult
                End If
            Else
                Return m_cInvalidParty
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyId", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetPolicyDetails
    '
    ' Description:
    '
    ' History: 11/09/2000 CJB - Created.
    '
    ' ***************************************************************** '
    Public Function GetPolicyDetails(ByRef vPartyCnt As Object, ByRef vPolicyDets As Array) As Integer
        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode
        Dim sSqlString, sSqlName As String
        Dim bStoredProc, bKeepNull As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get underlying policy details from the insurance_file table

            sSqlString = "SELECT DISTINCT insurance_ref, cover_start_date, expiry_date, " & _
                         "lead_insurer_cnt, product_id, insured_cnt, risk_code_id, renewal_date, " & _
                         "this_premium " & _
                         "FROM insurance_file " & _
                         "WHERE insurance_file_cnt = '" & CStr(vPartyCnt) & "' "

            sSqlName = "Get Policy Details"
            bStoredProc = False
            bKeepNull = True

            lResult = m_oDbConn.SQLSelect(sSqlString, sSqlName, bStoredProc, , vPolicyDets, , , , bKeepNull)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            If Information.IsArray(vPolicyDets) Then
                lResult = CType(gPMFunctions.FlipArray(vPolicyDets), gPMConstants.PMEReturnCode)

                If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lResult
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: FindClientDetails
    '
    ' Description:
    '
    ' History: 30/05/2000 IAC - Created.
    '
    ' ***************************************************************** '
    Public Function FindClientDetails(ByRef vClientID As Array) As Integer
        Dim result As Integer = 0
        Dim sSqlString, sSqlName As String
        Dim bStoredProc, bKeepNull As Boolean
        Dim lResult As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set m_bUseFlipArray to false because array will be in correct
            'format
            m_bUseFlipArray = False

            'Get ClientId from the Party Table


            sSqlString = "SELECT clientid, clientname, schemeno, schemeversion, " & _
                         "startdate, autogeneratedplanref, statusind, companyno, " & _
                         "companyname, schemename, pfprem_finance_cnt, " & _
                         "paymentmethod, productclass " & _
                         "from PFPremiumFinance WHERE clientid = '" & CStr(vClientID(0, 0)) & "' " & _
                         "or financecollatedplanref = '" & CStr(vClientID(0, 1)) & "'"

            sSqlName = "Find PF Client"
            bStoredProc = False
            bKeepNull = True

            vClientID = Nothing

            lResult = m_oDbConn.SQLSelect(sSqlString, sSqlName, bStoredProc, , vClientID, , , , bKeepNull)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            If Information.IsArray(vClientID) Then
                'Call Array Flipper Here!
                lResult = CType(gPMFunctions.FlipArray(vClientID), gPMConstants.PMEReturnCode)

                If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lResult
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindClientDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindClientDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: StatusUpdate
    '
    ' Description:
    '
    ' History: 01/06/2000 IAC - Created.
    '
    ' ***************************************************************** '
    Public Function StatusUpdate(ByRef vClientKey(,) As Object, ByRef vStatusInd As String) As Integer
        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode
        Dim sSqlString, sSqlName As String
        Dim bStoredProc As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSqlString = "UPDATE PFPremiumFinance " & _
                         "SET statusind = " & "'" & vStatusInd & "' "


            sSqlString = sSqlString & "WHERE pfprem_finance_cnt = '" & CStr(CInt(vClientKey(0, 10))) & "' "

            'Update existing record
            bStoredProc = False
            sSqlName = "UpdateStatusInd"

            lResult = m_oDbConn.SQLAction(sSqlString, sSqlName, bStoredProc)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StatusUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StatusUpdate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetInsurerDetails
    '
    ' Description: This function interogates two tables.
    '               Party & Product.
    '               This is to retreive the transaction insurer and
    '               transaction description
    '
    ' History: 11/09/2000 CJB - Created.
    '
    ' ***************************************************************** '
    Public Function GetInsurerDetails(ByRef vPartyId(,) As Object) As Integer
        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode
        Dim sSqlString, sSqlName As String
        Dim bStoredProc, bKeepNull As Boolean
        Dim vTransInsurer, vTransDescr As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Phase one retreive transaction insurer from the party tbl


            sSqlString = "SELECT name from party WHERE party_cnt = '" & CStr(vPartyId(0, 0)) & "' " & _
                         "AND party_type_id = '" & CStr(vPartyId(0, 1)) & "' "
            sSqlName = "Get Transaction Insurer"
            bStoredProc = False
            bKeepNull = True

            lResult = m_oDbConn.SQLSelect(sSqlString, sSqlName, bStoredProc, , vTransInsurer, , , , bKeepNull)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            'Phase two retreive transaction description from the product tbl

            sSqlString = "SELECT description from product WHERE product_id = '" & CStr(vPartyId(0, 2)) & "' "
            sSqlName = "Get Transaction Description"

            lResult = m_oDbConn.SQLSelect(sSqlString, sSqlName, bStoredProc, , vTransDescr, , , , bKeepNull)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            'Reset the vpartyid array with the results
            vPartyId = Nothing
            ReDim vPartyId(0, 1)


            vPartyId(0, 0) = vTransInsurer(0, 0)


            vPartyId(0, 1) = vTransDescr(0, 0)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsurerDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsurerDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' *************************************************************************** '
    '
    ' Name: CalculateFinance
    '
    ' Description: Select a Premium Finance Scheme and subsequent Rate table entry
    '              based upon the information given. Calculate the payments etc and
    '              return information in an array.
    '
    ' History: 11/09/2000 CJB - Created.
    '          04/12/2000 ND  - Added new ActionType (AnnualFromMonthly) to set
    '                           JustCalculateAnnual flag.
    '
    ' *************************************************************************** '
    Public Function CalculateFinance(ByRef v_ProductFamily As Object, ByRef v_BusinessTypeCode As String, ByRef v_DataModelCode As String, ByRef v_TransactionType As String, ByRef v_PaymentMethod As String, ByRef v_StartDate As Object, ByRef v_AmountToFinance As String, ByRef v_NoOfInstalments As Object, ByRef v_ActionType As String, ByRef v_RequestedDepositPercent As Object, Optional ByRef r_vResultString As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim vRequestArray(,) As Object
        Dim vSchemeArray As Array
        Dim vProdClass As Object
        Dim vOutputArray(16) As String
        Dim lResult As gPMConstants.PMEReturnCode
        Dim sDataModelCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sDataModelCode = v_DataModelCode
            'Simulate access via com
            lResult = CType(Initialise(, , , , , , , , , sDataModelCode), gPMConstants.PMEReturnCode)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                r_vResultString = "Error - Failed to Initialise Object bGISPremiumFinance"
                Return result
            End If

            'Perform basic validation on parameters, ensuring datatype is correct
            If Not Information.IsDate(v_StartDate) Then
                result = gPMConstants.PMEReturnCode.PMFalse

                r_vResultString = "Error - Invalid Start Date"
                Return result
            End If


            If Convert.IsDBNull(v_AmountToFinance) Or IsNothing(v_AmountToFinance) Or v_AmountToFinance = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse

                r_vResultString = "Error - Finance Amount Must Be At Least ?1"
                Return result
            End If

            If StringsHelper.ToDoubleSafe(v_AmountToFinance) < 1 Then
                result = gPMConstants.PMEReturnCode.PMFalse

                r_vResultString = "Error - Finance Amount Must Be At Least ?1"
                Return result
            End If

            v_PaymentMethod = v_PaymentMethod.ToUpper()
            v_BusinessTypeCode = v_BusinessTypeCode.ToUpper()
            v_DataModelCode = v_DataModelCode.ToUpper()
            v_TransactionType = v_TransactionType.ToUpper()
            v_PaymentMethod = v_PaymentMethod.ToUpper()

            If v_ActionType.ToUpper() = "QUOTE" Or v_ActionType.ToUpper() = "ACCEPT" Or v_ActionType.ToUpper() = "ANNUALFROMMONTHLY" Then
            Else
                result = gPMConstants.PMEReturnCode.PMFalse

                r_vResultString = "Error - Action Type Must be 'QUOTE' or 'ACCEPT'."
                Return result
            End If

            ' ND 041200 - See if we just want to calculate and return the annual premium.
            m_bJustCalculateAnnual = False
            If v_ActionType = "AnnualFromMonthly" Then
                m_bJustCalculateAnnual = True
            End If

            'Apply any formating to the parameters prior to retrieving scheme



            lResult = CType(FormatParameters(vStartDate:=CStr(v_StartDate), vProductFamily:=CStr(v_ProductFamily), vTransType:=v_TransactionType, vProductClassCodes:=vProdClass), gPMConstants.PMEReturnCode)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then

                r_vResultString = "Error - Formatting Input Parameters"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Initialiase return array

            vRequestArray = Nothing

            'If reached this point then start to process request



            lResult = CType(GetAvailableSchemes(vProductFamily:=vProdClass, vBusinessTypeCode:=v_BusinessTypeCode, vDataModelCode:=v_DataModelCode, vTransactionType:=v_TransactionType, vPaymentMethod:=v_PaymentMethod, vStartDate:=CStr(v_StartDate), vNoOfInstalments:=CStr(v_NoOfInstalments), vActionType:=v_ActionType, vResultArray:=vRequestArray), gPMConstants.PMEReturnCode)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Build the selection array and perform calculations before displaying the results.
            If Not Information.IsArray(vRequestArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse

                r_vResultString = "No Premium Finance Schemes Available"
                Return result
            End If

            'BuildQuote Array ready to send for calculation
            vSchemeArray = Nothing

            lResult = CType(BuildQuoteArray(v_AmountToFinance, v_TransactionType, vRequestArray, vSchemeArray), gPMConstants.PMEReturnCode)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then

                r_vResultString = "Error - Building of Quote Failed"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Call to calculate premium finance
            lResult = CType(Calculate_Quotes(vSchemeArray), gPMConstants.PMEReturnCode)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then

                r_vResultString = "Error - Calcuation Process Failed"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_ActionType = "Quote" Then
                'Build Return Message - Return Message is to contain the following values :-
                'Amount to Finance
                'Interest Rate
                'APR
                'Interest Charged
                'No of Instalments
                'First Instalment
                'Other Instalments

                'r_vResultString = vSchemeArray(0, m_cAmountToFinance) & "^" & _
                ''   vSchemeArray(0, m_cIntRate) & "^" & _
                ''  vSchemeArray(0, m_cAPR) & "^" & _
                '' vSchemeArray(0, m_cInterestCost) & "^" & _
                ''vSchemeArray(0, m_cNoOfInstalments) & "^" & _
                ''vSchemeArray(0, m_cFirstInstalment) & "^" & _
                ''vSchemeArray(0, m_cOthInstalments)

                vOutputArray(0) = CStr(vSchemeArray(0, m_cAmountToFinance))
                vOutputArray(1) = CStr(vSchemeArray(0, m_cIntRate))
                vOutputArray(2) = CStr(vSchemeArray(0, m_cAPR))
                vOutputArray(3) = CStr(vSchemeArray(0, m_cInterestCost))
                vOutputArray(4) = CStr(vSchemeArray(0, m_cNoOfInstalments))
                vOutputArray(5) = CStr(vSchemeArray(0, m_cFirstInstalment))
                vOutputArray(6) = CStr(vSchemeArray(0, m_cOthInstalments))
                vOutputArray(7) = CStr(vSchemeArray(0, m_cDeposit))
                vOutputArray(8) = CStr(vSchemeArray(0, m_cArrangementFee))
                vOutputArray(9) = CStr(vSchemeArray(0, m_cNetAmount))
                vOutputArray(10) = CStr(vSchemeArray(0, m_cDepositPercent))
                vOutputArray(11) = CStr(vSchemeArray(0, m_cCompanyName))
                vOutputArray(12) = CStr(vSchemeArray(0, m_cCompanyNo))
                vOutputArray(13) = CStr(vSchemeArray(0, m_cSchemeName))
                vOutputArray(14) = CStr(vSchemeArray(0, m_cSchemeNo))
                vOutputArray(15) = CStr(vSchemeArray(0, m_cSchemeVer))
                vOutputArray(16) = CStr(vSchemeArray(0, m_cBasisOfCalc))



                r_vResultString = vOutputArray

                ' ND 041200 just return the new annual premium
            ElseIf v_ActionType = "AnnualFromMonthly" Then



                r_vResultString = m_vAnnualFromMonthlyPremium

            Else
                'v_UseFlipArrary is set to a value of 9 so commit re-calcuated results to
                'PFPremiumFinance table
                lResult = CType(InsertNewRecord(vSchemeArray), gPMConstants.PMEReturnCode)

                If lResult <> gPMConstants.PMEReturnCode.PMTrue Then

                    r_vResultString = "Error - Could Not Create Premium Finance Record"
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalculateFinance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateFinance", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: FormatParameters
    '
    ' Description:
    '
    ' History: 11/09/2000 CJB - Created.
    '
    ' ***************************************************************** '
    Public Function FormatParameters(ByRef vStartDate As String, ByRef vProductFamily As String, ByRef vTransType As String, Optional ByRef vProductClassCodes() As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim sSecondElement, sFirstElement, sThirdElement, sFifthElement, sFourthElement, sSixthElement As String
        Dim iNumberOfElements As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'if it is commercial and new business
            If (vProductFamily = "Commercial Lines Business Only") And (vTransType = "NB") Then
                iNumberOfElements = 3
                sFirstElement = "C"
                sSecondElement = "I"
                sThirdElement = "N"
                sFourthElement = "A"

                'if it is personal and new business
            ElseIf (vProductFamily = "Personal Lines Business Only") And (vTransType = "NB") Then
                iNumberOfElements = 3
                sFirstElement = "P"
                sSecondElement = "W"
                sThirdElement = "N"
                sFourthElement = "A"

                'if it is commercial and renewal
            ElseIf (vProductFamily = "Commercial Lines Business Only") And (vTransType = "REN") Then
                iNumberOfElements = 3
                sFirstElement = "M"
                sSecondElement = "I"
                sThirdElement = "R"
                sFourthElement = "A"

                'if it is personal and renewal
            ElseIf (vProductFamily = "Personal Lines Business Only") And (vTransType = "REN") Then
                iNumberOfElements = 3
                sFirstElement = "L"
                sSecondElement = "W"
                sThirdElement = "R"
                sFourthElement = "A"

                'if it is any business and new business
            ElseIf (vProductFamily = "Any Business Type") And (vTransType = "NB") Then
                iNumberOfElements = 5
                sFirstElement = "N"
                sSecondElement = "C"
                sThirdElement = "P"
                sFourthElement = "A"
                sFifthElement = "I"
                sSixthElement = "W"

                'if it is any business and renewal
            ElseIf (vProductFamily = "Any Business Type") And (vTransType = "REN") Then

                iNumberOfElements = 5
                sFirstElement = "R"
                sSecondElement = "M"
                sThirdElement = "L"
                sFourthElement = "A"
                sFifthElement = "I"
                sSixthElement = "W"

            End If

            'redimension the array to be the number of elements equal to the number of codes
            ReDim vProductClassCodes(iNumberOfElements)

            If iNumberOfElements = 3 Then

                vProductClassCodes(0) = sFirstElement

                vProductClassCodes(1) = sSecondElement

                vProductClassCodes(2) = sThirdElement

                vProductClassCodes(3) = sFourthElement
            Else

                vProductClassCodes(0) = sFirstElement

                vProductClassCodes(1) = sSecondElement

                vProductClassCodes(2) = sThirdElement

                vProductClassCodes(3) = sFourthElement

                vProductClassCodes(4) = sFifthElement

                vProductClassCodes(5) = sSixthElement
            End If

            Dim TempDate As Date
            vStartDate = IIf(DateTime.TryParse(vStartDate, TempDate), TempDate.ToString("yyyy/MM/dd"), vStartDate)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatParameters Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatParameters", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: BuildQuoteArray
    '
    ' Description:
    '
    ' History: 11/09/2000 CJB - Created.
    '
    ' ***************************************************************** '
    Public Function BuildQuoteArray(ByRef vAmountToFinance As String, ByRef vTransType As String, ByRef vRequestArray(,) As Object, Optional ByRef vSchemeArray As Array = Nothing) As Integer
        Dim result As Integer = 0
        Dim iNoOfSchemes As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'For the present set iNoOfSchemes to 0.  This indicates that only the
            'first scheme has been selected from the returned array
            iNoOfSchemes = 0

            vSchemeArray = Nothing

            vSchemeArray = Array.CreateInstance(GetType(Object), New Integer() {iNoOfSchemes + 1, m_cMainArray + 1}, New Integer() {0, 0})

            For iSelScheme As Integer = 0 To iNoOfSchemes


                vSchemeArray(iSelScheme, m_cCompanyNo) = vRequestArray(4, iNoOfSchemes)


                vSchemeArray(iSelScheme, m_cSchemeNo) = vRequestArray(5, iNoOfSchemes)


                vSchemeArray(iSelScheme, m_cSchemeVer) = vRequestArray(2, iNoOfSchemes)


                vSchemeArray(iSelScheme, m_cStartDate) = vRequestArray(3, iNoOfSchemes)


                vSchemeArray(iSelScheme, m_cAmountToFinance) = vAmountToFinance



                vSchemeArray(iSelScheme, m_cProdClass) = vRequestArray(6, iNoOfSchemes)


                vSchemeArray(iSelScheme, m_cBasisOfCalc) = vRequestArray(7, iNoOfSchemes)



                vSchemeArray(iSelScheme, m_cCompanyName) = vRequestArray(0, iNoOfSchemes)


                vSchemeArray(iSelScheme, m_cSchemeName) = vRequestArray(1, iNoOfSchemes)

                If vTransType = "NB" Then

                    vSchemeArray(iSelScheme, m_cTransType) = "N"
                Else

                    vSchemeArray(iSelScheme, m_cTransType) = "R"
                End If



                vSchemeArray(iSelScheme, m_cQDocPath) = vRequestArray(8, iNoOfSchemes)


                vSchemeArray(iSelScheme, m_cQDocName) = vRequestArray(9, iNoOfSchemes)


                vSchemeArray(iSelScheme, m_cBDocPath) = vRequestArray(10, iNoOfSchemes)


                vSchemeArray(iSelScheme, m_cBDocName) = vRequestArray(11, iNoOfSchemes)


                vSchemeArray(iSelScheme, m_cEndDate) = vRequestArray(14, iNoOfSchemes)


                vSchemeArray(iSelScheme, m_cCDocPath) = vRequestArray(12, iNoOfSchemes)


                vSchemeArray(iSelScheme, m_cCdocName) = vRequestArray(13, iNoOfSchemes)



                vSchemeArray(iSelScheme, m_cPaymentMethod) = vRequestArray(15, iNoOfSchemes)

                vSchemeArray(iSelScheme, m_cAutoGenPlanRef) = ""

                vSchemeArray(iSelScheme, m_cPremFinCnt) = "0"
            Next iSelScheme


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BuildQuoteArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildQuoteArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPolicyCoverType
    '
    ' Description:
    '
    ' History: 11/09/2000 CJB - Created.
    '
    ' ***************************************************************** '
    Public Function GetPolicyCoverType(ByRef vCoverType As String) As Integer
        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode
        Dim vResultArray(,) As Object
        Dim sSqlString, sSqlName As String
        Dim bStoredProc, bKeepNull As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Phase one retreive transaction insurer from the party tbl
            sSqlString = "SELECT description from risk_code WHERE risk_code_id = '" & vCoverType & "' "
            sSqlName = "Get Cover Type"
            bStoredProc = False
            bKeepNull = True

            lResult = m_oDbConn.SQLSelect(sSqlString, sSqlName, bStoredProc, , vResultArray, , , , bKeepNull)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If


            vCoverType = CStr(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyCoverType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyCoverType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetNextPFCount
    '
    ' Description: To allocate next sequential count on the premium finance
    '              table
    '
    ' History: 11/09/2000 CJB - Created.
    '
    ' ***************************************************************** '
    Public Function GetNextPFCount(ByRef iPFCount As Integer) As Integer
        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode
        Dim vResultArray(,) As Object
        Dim sSqlString, sSqlName As String
        Dim bStoredProc, bKeepNull As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSqlString = "SELECT MAX (pfprem_finance_cnt) from pfpremiumfinance "

            sSqlName = "Alloc Next Count On PFPremiumFinance"
            bStoredProc = False
            bKeepNull = True

            lResult = m_oDbConn.SQLSelect(sSqlString, sSqlName, bStoredProc, , vResultArray)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If


            If Not Information.IsArray(vResultArray) Or CStr(vResultArray(0, 0)) = "" Then
                iPFCount = 1
            Else
                'Increment the count

                iPFCount = CDbl(vResultArray(0, 0)) + 1
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNextPFCount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextPFCount", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetPolicyCount
    '
    ' Description:
    '
    ' History: 11/09/2000 CJB - Created.
    '
    ' ***************************************************************** '
    Public Function GetPolicyCount(ByRef iPremFinCnt As Integer, ByRef vPolicyCnt As Array) As Integer
        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode
        Dim vResultArray As Array
        Dim sSqlString, sSqlName As String
        Dim bStoredProc, bKeepNull As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSqlString = "SELECT pfpolicy_id FROM pfpolicy_code " & _
                         "WHERE pfprem_finance_cnt = '" & CStr(iPremFinCnt) & "'"

            sSqlName = "Get Count From PFPremiumFinance"
            bStoredProc = False
            bKeepNull = True

            lResult = m_oDbConn.SQLSelect(sSqlString, sSqlName, bStoredProc, , vResultArray, , , , bKeepNull)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            If Not Information.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                vPolicyCnt = vResultArray
                lResult = CType(gPMFunctions.FlipArray(vPolicyCnt), gPMConstants.PMEReturnCode)
                If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lResult
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyCount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyCount", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: ProduceEDIMessage
    '
    ' Description:
    '
    ' History: 11/09/2000 CJB - Created.
    '
    ' ***************************************************************** '
    Public Function ProduceEDIMessage(ByVal vEDIMessage As Object, ByVal vReTransmit As Object) As Integer
        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Create object for EDIMessage

            lResult = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oEDIMessage, v_sClassName:="bPMPFEDIMessage.clsPFEDIMessage", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)



            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oEDIMessage = Nothing
                Return lResult
            End If


            lResult = m_oEDIMessage.GetFields(vEDIMessage, vReTransmit)

            m_oEDIMessage = Nothing

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProduceEDIMessage Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProduceEDIMessage", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    

    ' ***************************************************************** '
    '
    ' Name: RetrieveBroker
    '
    ' Description:
    '
    ' History: 11/09/2000 CJB - Created.
    '
    ' ***************************************************************** '
    Public Function RetrieveBroker(ByRef vResultArray As Array) As Integer
        Dim result As Integer = 0
        Dim lResult As gPMConstants.PMEReturnCode
        Dim sSqlString, sSqlName As String
        Dim bStoredProc As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Retrieve Broker details and append onto the vClientArray
            'This bit of code will need ajustments made to the field names when
            'the next version of the sbo database is installed.
            sSqlString = "SELECT description, address1, address2, address3, postal_code, " & _
                         "address4 " & _
                         "FROM source " & _
                         "WHERE source_id = '1'"

            'lResult = oSource.GetDetails("1")

            sSqlName = "Retrieve broker From Source"
            bStoredProc = False

            vResultArray = Nothing

            lResult = m_oDbConn.SQLSelect(sSqlString, sSqlName, bStoredProc, , vResultArray, , , , True)

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lResult
            End If

            If Information.IsArray(vResultArray) Then
                lResult = CType(gPMFunctions.FlipArray(vResultArray), gPMConstants.PMEReturnCode)

                If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lResult
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RetrieveBroker Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RetrieveBroker", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DocRndNumberGen
    '
    ' Description:
    '
    ' History: 11/09/2000 CJB - Created.
    '
    ' ***************************************************************** '
    Public Function DocRndNumberGen(ByRef lDocSuffix As Integer) As Integer
        Dim result As Integer = 0
        Dim vSeed As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Generate a random number for use as a suffix on the premium finance documents.
            'To prevent any document locking.

            'Initialise the Random Number Generator

            vSeed = DateTime.Now

            VBMath.Randomize(CDbl(vSeed))

            'Generate a random number between 1 and System Date & Time

            lDocSuffix = CInt(Math.Floor((CDbl(vSeed) * VBMath.Rnd()) + 1))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DocRndNumberGen Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DocRndNumberGen", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CalculateAnnualFromMonthly
    '
    ' Description: Receives a monthly premium for ratebeater to beat
    '              and returns the annual premium for such a monthly
    '              premium based on ratebeater rules
    '
    ' History: 01/12/00 - ND Created.
    '
    ' ***************************************************************** '
    Public Function CalculateAnnualFromMonthly() As Integer

        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check that monthly premium is greater than zero
            If AmountToFinance <= 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error - Monthly premium must be greater than zero", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateAnnualFromMonthly")

                Return result
            End If


            ' The following comes from :
            ' Annual Premium = Original Premium + Interest
            '
            ' Interest = Original Premium * (Deposit Percentage / 100) * (Interest Rate / 100)
            '
            ' This assumes that the Annual Premium is 12 times the monthly premium - which
            ' introduces a small error into the solution.
            '

            m_vAnnualFromMonthlyPremium = (120000 * AmountToFinance) / (10000 + (100 * IntRate) - (DepositPCent * IntRate))



            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalculateAnnualFromMonthly Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateAnnualFromMonthly", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
    Private Shared _DefaultInstance As clsPremiumFinance = Nothing
    Public Shared ReadOnly Property DefaultInstance() As clsPremiumFinance
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New clsPremiumFinance
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class
