Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms

Imports SharedFiles

Partial Public Class uctInstalments
    Inherits System.Windows.Forms.UserControl

    ' RAW 05/11/2003 : CQ2912, 2976 : introduce MTAType 4 (also replace hard-coded values with constants)
    ' RAW 05/11/2003 : CQ1824 : ensure that the selected cell is kept in focus
    ' RAW 13/11/2003 : CQ1765 : fix bugs with instalment MTAs (includes removing MTAType 4)

    '=================
    'Private Constants
    '=================
    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "uctInstalmentsControl"

    '====================================
    'Private Variables for UCT Properties
    '====================================
    Private m_dtStart_date As Date
    Private m_dtEnd_Date As Date
    Private m_dtQuote_Date As Date
    Private m_sProduct_Code As String = ""
    Private m_crAmount As Decimal
    Private m_lSource_ID As Integer
    Private m_lProduct_ID As Integer
    Private m_lMediaType_ID As Integer
    Private m_lPfFrequency_ID As Integer
    Private m_lCompany_No As Integer
    Private m_lScheme_No As Integer
    Private m_lScheme_Version As Integer
    Private m_lPFRF_ID As Integer
    Private m_lPremiumFinanceCnt As Integer
    Private m_lPremiumFinanceVersion As Integer
    Private m_vPFTransArray As Object
    Private m_lPartyCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_lInsuranceFileCnt_Renewal As Integer 'Alix Bergeret - 20/03/2003 - Issue 2428
    Private m_iMTAType As Integer
    Private m_iPreviousMTAType As Integer ' RAW 13/11/2003 : CQ1765 : added
    Private m_sRunningContext As String = ""
    Private m_crDepositAmount As Decimal
    Private m_lBackDatedMTAType As Integer
    Private m_iTransCurrencyID As Integer
    Private m_iBaseCurrencyID As Integer
    Private m_sTransISOCode As String
    Private m_sBaeISOCode As String
    Private m_dXRate As Double
    'PN:28199
    Private m_lExcluded As Decimal
    Private m_lTaxExcluded As Decimal
    Private m_lGrossDue As Decimal
    Private m_lFeeDeposit As Decimal
    Private m_lTaxDeposit As Decimal

    '==========================
    'Private Standard Variables
    '==========================
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As gPMConstants.PMENavigateButtonStatus
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sStepStatus As New FixedLengthString(2)
    Private m_crTotalPayableAmount As Decimal
    Private m_iUseTransactionCurrency As Integer = -1

    ' Private instance of the business object.
    Private m_oBusiness As Object
    Private m_oInsuranceFile As Object
    Private m_oFormFields As iPMFormControl.FormFields
    Private m_vQuoteArray As Object
    Private m_vMediaColumnsInGrid As Object
    Private m_bProcessingEvent As Boolean
    Private m_bHasDatesChange As Boolean = False
    Private m_bChangingSelectedCell As Boolean
    Private m_bInitialisingGrid As Boolean
    Private m_iSelectedColumnIndex As Integer
    Private m_iSelectedRowIndex As Integer
    Private m_iExistingColumnIndex As Integer
    Private m_iExistingRowIndex As Integer
    Private m_lSelectedQuoteNo As Integer
    Private m_objDataGridRows As clsDataGridRows
    Private m_lGridOffset As Integer
    Private m_lGridMaxRowIndex As Integer
    Private m_lGridMaxColIndex As Integer

    '========================================
    'Private Variables for holding UCT values
    '========================================
    Private m_dOverrideRate As Double
    Private m_bOverrideInterestRate As Boolean
    Private m_sOverrideReference As String = ""
    Private m_bOverrideCommission As Boolean
    Private m_dOverrideDeposit As Double 'PN12144
    Private m_bOverrideDeposit As Boolean 'PN12144
    Private m_bPaymentProtection As Boolean
    Private m_bAllowInstalmentsOverride As Boolean
    Private m_dtPreferredDate As Date
    Private m_iDayOfWeek As Integer
    Private m_iPreviousDayOfMonth As Integer = -1
    Private m_iDayOfMonth As Integer
    Private m_sPaymentPeriodString As String = ""
    Private m_iNumberOfTransactions As Integer
    'TR - Keepins track of First Insurance File counter during backdated MTA
    Private m_crFirstOriginalInstalment As Decimal
    Private m_iFirstPaymentDate As Integer

    'ACR 14-07-05 added to hide/show tabs for sg quotes
    Private m_bSGQuote As Boolean
    Private m_bRequote As Boolean

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' RAM20030403   :  Added the following variables to hold the selected
    '                   bank account details or credit card details
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private m_vBankAccountDetails(,) As Object
    Private m_vCreditCardDetails(,) As Object
    ' Following variable represents the array index of selected Bank Account Details
    Private m_iSelectedBankAccountDetails As Integer
    ' Following variable represents the array index of selected Credit Card Details
    Private m_iSelectedCreditCardDetails As Integer
    Private m_bUseExistingBankAccount As Boolean
    Private m_bUseExistingCreditCard As Boolean
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' RAM20030403   : END
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'CQ1038 - SMJB 08/07/03 - Added for property 'QuoteAvailable'
    Private m_bQuoteAvailable As Boolean

    Private m_GridArray As XArrayHelper
    Private m_SortedColumnIndex As Integer
    Private m_SortedDirection As Integer
    'developer guide no. 162
    Private Const ACQuoteArrayElement As Integer = 9 'PN12449

    'CQ865 MAW 19/09/2003
    Private m_bPaymentOptionsOnly As Boolean
    Private m_bInitialised As Boolean
    Private m_sUnderwriting As String = ""
    Private m_bPlanSelected As Boolean
    Private m_bSinglePlanParty As Boolean
    Private m_bIsSingleInstalmentPlan As Boolean
    'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.9.1)
    Private m_iLanguageID As Integer
    'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.9.1)
    'Start (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)
    Private m_bIsTrueMonthlypolicyandNextInstalmentRenewal As Boolean
    'End (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)
    'PN61609
    Private m_bIsFinanceAmountNetPremium As Boolean
    'developer guide no. 50
    Dim objfrmPassword As frmPassword
    'PN72107
    Private m_bStopClickEvent As Boolean
    ' PN 74070
    Private m_bSchemeResembleToPrevious As Boolean

    Private m_bDataHasChanged As Boolean = False
    Private m_iUseTransCurrency As Integer = 0
    Private m_iACUserCanChangeInstalmentDefaultCurrency As Integer = 0

    Private m_sTransType As String = String.Empty
    Private m_sSchemeName As String = String.Empty
    Private m_bSuppressDecimalValues As Boolean

    Private m_bInValidDates As Boolean = False
    Private m_bFirstTimeLoad As Boolean = False
    Private m_bFirstInstalmentFilled As Boolean = False

    Private m_bHasInstalmentVersions As Boolean
    Private m_bCallFromTab2 As Boolean
    Private m_bEnableOveridesViaTabOnly As Boolean
    Private m_sFrequencyDesc As String = String.Empty

    Public Property HasInstalmentVersions As Boolean
        Get
            Return m_bHasInstalmentVersions
        End Get
        Set(ByVal value As Boolean)
            m_bHasInstalmentVersions = value
        End Set
    End Property

    Public Property EnableOveridesViaTabOnly As Boolean
        Get
            Return m_bEnableOveridesViaTabOnly
        End Get
        Set(ByVal Value As Boolean)
            m_bEnableOveridesViaTabOnly = Value
        End Set
    End Property
    Private Function BuildGridListStyle() As Integer

        Dim result As Integer = 0
        Try

            Dim lQRowCount, lQColCount As Integer

            Dim Col As DataGridViewColumn
            Dim SimpleArray(,) As Object
            Dim iUboundRow, iLBoundRow As Integer

            m_GridArray = New XArrayHelper()

            lQRowCount = m_vQuoteArray.GetUpperBound(1)
            lQColCount = ACQuoteArrayElement

            ReDim SimpleArray(lQRowCount, lQColCount)

            iUboundRow = m_vQuoteArray.GetUpperBound(1)
            iLBoundRow = m_vQuoteArray.GetLowerBound(1)

            For iOrigArrayRowCount As Integer = iLBoundRow To iUboundRow

                SimpleArray(iOrigArrayRowCount, 0) = m_vQuoteArray(k_PFQuoteSchemeName, iOrigArrayRowCount)

                SimpleArray(iOrigArrayRowCount, 1) = m_vQuoteArray(k_PFQuoteFrequencyDescription, iOrigArrayRowCount)

                SimpleArray(iOrigArrayRowCount, 2) = m_vQuoteArray(k_PFQuoteMediaTypeDescription, iOrigArrayRowCount)

                SimpleArray(iOrigArrayRowCount, 3) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatMoney, Math.Round(CDbl(CDbl(m_vQuoteArray(k_PFQuoteDepositAmount, iOrigArrayRowCount))), 2))
                If CDbl(m_vQuoteArray(k_PFQuoteInstalmentsToPay, iOrigArrayRowCount)) = 1 Then

                    SimpleArray(iOrigArrayRowCount, 4) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatMoney, Math.Round(CDbl(CDbl(m_vQuoteArray(k_PFQuoteFirstInstalmentAmount, iOrigArrayRowCount))), 2))
                Else

                    SimpleArray(iOrigArrayRowCount, 4) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatMoney, Math.Round(CDbl(CDbl(m_vQuoteArray(k_PFQuoteOtherInstalmentAmount, iOrigArrayRowCount))), 2))
                End If
                If gPMFunctions.ToSafeString(m_vQuoteArray(k_PFQuoteSchemeName, iOrigArrayRowCount)) = "PCLPSG" Then

                    SimpleArray(iOrigArrayRowCount, 5) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatString, m_vQuoteArray(k_PFSGRef, iOrigArrayRowCount)) 'PN12449

                    SimpleArray(iOrigArrayRowCount, 6) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatString, m_vQuoteArray(k_PFSGSchemeType, iOrigArrayRowCount))
                Else

                    SimpleArray(iOrigArrayRowCount, 5) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatString, m_vQuoteArray(k_PFQuoteProductClass, iOrigArrayRowCount)) 'PN12449

                    SimpleArray(iOrigArrayRowCount, 6) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatString, m_vQuoteArray(k_PFSGTerms, iOrigArrayRowCount))
                End If

                SimpleArray(iOrigArrayRowCount, 7) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatInteger, m_vQuoteArray(k_PFQuoteSchemeNo, iOrigArrayRowCount))
                SimpleArray(iOrigArrayRowCount, 8) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatInteger, m_vQuoteArray(k_PFQuoteSchemeVersion, iOrigArrayRowCount))


                SimpleArray(iOrigArrayRowCount, ACQuoteArrayElement) = iOrigArrayRowCount

                If CBool(m_vQuoteArray(k_PFQuoteHighlightCell, iOrigArrayRowCount)) Then
                    m_lSelectedQuoteNo = iOrigArrayRowCount
                End If

            Next

            m_GridArray.RedimXArray(New Integer() {lQRowCount, lQColCount}, New Integer() {0, 0})

            For lRow As Integer = m_GridArray.GetLowerBound(0) To m_GridArray.GetUpperBound(0)
                For lCol As Integer = m_GridArray.GetLowerBound(1) To m_GridArray.GetUpperBound(1)

                    m_GridArray(lRow, lCol) = SimpleArray(lRow, lCol)
                Next lCol
            Next lRow

            Dim bindingSource As BindingSource = New BindingSource(m_GridArray, "")
            tdgListInstalment.DataSource = bindingSource
            With tdgListInstalment.Columns
                While .Count > 0
                    .RemoveAt(0)
                End While

                While .Count < m_GridArray.GetLength(1) + 1
                    Dim newColumn As Artinsoft.Windows.Forms.DataGridViewExtendedColumn = Nothing
                    newColumn = New Artinsoft.Windows.Forms.DataGridViewExtendedColumn()
                    .Insert(0, newColumn)
                    Col = newColumn
                    Col.Visible = True
                    Col.ReadOnly = True
                End While
                tdgListInstalment.Columns(0).DefaultCellStyle.Alignment = ContentAlignment.MiddleLeft
                tdgListInstalment.Columns(0).HeaderText = "Scheme"
                tdgListInstalment.Columns(0).Width = 190

                tdgListInstalment.Columns(1).DefaultCellStyle.Alignment = ContentAlignment.MiddleCenter
                tdgListInstalment.Columns(1).HeaderText = "Payment"
                tdgListInstalment.Columns(1).Width = 60

                tdgListInstalment.Columns(2).DefaultCellStyle.Alignment = ContentAlignment.MiddleCenter
                tdgListInstalment.Columns(2).HeaderText = "Media Type"
                tdgListInstalment.Columns(2).Width = 80

                tdgListInstalment.Columns(3).DefaultCellStyle.Alignment = ContentAlignment.MiddleRight
                tdgListInstalment.Columns(3).HeaderText = "Deposit"
                tdgListInstalment.Columns(3).Width = 80
                tdgListInstalment.Columns(3).Resizable = False

                tdgListInstalment.Columns(4).DefaultCellStyle.Alignment = ContentAlignment.MiddleRight
                tdgListInstalment.Columns(4).HeaderText = "Amount"
                tdgListInstalment.Columns(4).Width = 100
                tdgListInstalment.Columns(4).Resizable = False

                tdgListInstalment.Columns(5).DefaultCellStyle.Alignment = ContentAlignment.MiddleLeft
                tdgListInstalment.Columns(5).HeaderText = "Type"
                tdgListInstalment.Columns(5).Width = 40

                tdgListInstalment.Columns(5).Resizable = False

                tdgListInstalment.Columns(6).DefaultCellStyle.Alignment = ContentAlignment.MiddleLeft
                tdgListInstalment.Columns(6).HeaderText = "Funding"
                tdgListInstalment.Columns(6).Width = 65
                tdgListInstalment.Columns(6).Resizable = False

                tdgListInstalment.Columns(7).Visible = False
                tdgListInstalment.Columns(8).Visible = False
                tdgListInstalment.Columns(9).Visible = False
            End With
            tdgListInstalment.DataSource = bindingSource
            With tdgListInstalment
                .ReBind()
                .Refresh()

                m_SortedColumnIndex = -1
                m_SortedDirection = 2
                tdgListInstalment_ColumnHeaderMouseClick(tdgListInstalment, New DataGridViewCellMouseEventArgs(0, 0, 0, 0, New MouseEventArgs(Windows.Forms.MouseButtons.None, 0, 0, 0, 0)))


                If Not IsNothing(.CurrentRow) And .CurrentRow.Cells(lQColCount).Visible Then
                    .CurrentCell = .CurrentRow.Cells(lQColCount)
                End If

                m_iSelectedColumnIndex = .CurrentCell.ColumnIndex

                RefreshQuoteDataOnScreen()
            End With
            Dim sSchemeName As String = ""
            If m_bHasDatesChange Then
                m_sSchemeName = ""
                sSchemeName = m_oBusiness.m_sSchemeName
            End If
            If Not m_bFirstTimeLoad Then
                For j As Integer = 0 To tdgListInstalment.RowCount - 1
                    If (Not String.IsNullOrEmpty(m_sSchemeName) AndAlso tdgListInstalment.Rows(j).Cells(0).FormattedValue() = m_sSchemeName) OrElse
                        (Not String.IsNullOrEmpty(sSchemeName) AndAlso tdgListInstalment.Rows(j).Cells(0).FormattedValue() = sSchemeName AndAlso tdgListInstalment.Rows(j).Cells(1).FormattedValue() = m_sFrequencyDesc) Then
                        m_iSelectedRowIndex = j
                        tdgListInstalment.Rows(j).Selected = True
                        m_lSelectedQuoteNo = m_GridArray(CLng(m_iSelectedRowIndex), ACQuoteArrayElement)
                        m_bFirstTimeLoad = True
                        If IsArray(m_vQuoteArray) AndAlso m_vQuoteArray IsNot Nothing Then
                            PopulateFirstInstalmentDate(gPMFunctions.ToSafeInteger(m_vQuoteArray(k_PFQuoteDaysDelay, m_lSelectedQuoteNo)), gPMFunctions.ToSafeInteger(m_vQuoteArray(k_PFStartLimit, m_lSelectedQuoteNo)))
                            cboPreferredDate.SelectedIndex = m_iFirstPaymentDate
                        End If
                        Exit For
                    End If
                Next
            End If
            If (m_iSelectedRowIndex < 0) Then
                m_iSelectedRowIndex = 0
            End If
            ListInstalmentSelectRow()

            If Not IsNothing(tdgListInstalment.PreviousCell) AndAlso tdgListInstalment.PreviousCell.RowIndex <> -1 Then

                For Each dr As DataGridViewRow In tdgListInstalment.Rows
                    If dr.Index = m_iSelectedRowIndex Then
                        dr.Selected = True
                    Else
                        dr.Selected = False
                    End If
                Next
            Else

                ListInstalmentSelectRow()

            End If
            tdgListInstalment_Click(Me.tdgInstalment, Nothing)

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Build Data Array", ACApp, ACClass, "BuildGridListStyle", Information.Err().Number, excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '=================
    'Public Properties
    '=================
    'Write-only

    'CQ865 MAW 19/09/2003
    <Browsable(False)>
    Public WriteOnly Property PaymentOptionsOnly() As Boolean
        Set(ByVal Value As Boolean)
            m_bPaymentOptionsOnly = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property Start_Date() As Date
        Set(ByVal Value As Date)
            m_dtStart_date = Value
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property End_Date() As Date
        Set(ByVal Value As Date)
            m_dtEnd_Date = Value
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property Quote_Date() As Date
        Set(ByVal Value As Date)
            m_dtQuote_Date = Value
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property Product_Code() As String
        Set(ByVal Value As String)
            m_sProduct_Code = Value
            ShowHideTopTabs()
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property Amount() As Decimal
        Set(ByVal Value As Decimal)
            m_crAmount = Value
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property Source_ID() As Integer
        Set(ByVal Value As Integer)
            m_lSource_ID = Value
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property Product_ID() As Integer
        Set(ByVal Value As Integer)
            m_lProduct_ID = Value
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property MediaType_ID() As Integer
        Set(ByVal Value As Integer)
            m_lMediaType_ID = Value
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property PfFrequency_ID() As Integer
        Set(ByVal Value As Integer)
            m_lPfFrequency_ID = Value
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property Company_No() As Integer
        Set(ByVal Value As Integer)
            m_lCompany_No = Value
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property Scheme_No() As Integer
        Set(ByVal Value As Integer)
            m_lScheme_No = Value
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property Scheme_Version() As Integer
        Set(ByVal Value As Integer)
            m_lScheme_Version = Value
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property PFRF_ID() As Integer
        Set(ByVal Value As Integer)
            m_lPFRF_ID = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property MTAType() As Integer
        Get
            Return m_iMTAType
        End Get
        Set(ByVal Value As Integer)
            m_iMTAType = Value
            ShowHideTopTabs()
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property ReQuote() As Boolean
        Set(ByVal Value As Boolean)
            m_bRequote = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property FeeExcluded() As Decimal
        Get
            Return m_lExcluded
        End Get
        Set(ByVal Value As Decimal)
            m_lExcluded = Value

        End Set
    End Property
    <Browsable(True)>
    Public Property GrossDue() As Decimal
        Get
            Return m_lGrossDue
        End Get
        Set(ByVal Value As Decimal)
            m_lGrossDue = Value

        End Set
    End Property

    <Browsable(True)>
    Public Property TaxExcluded() As Decimal
        Get
            Return m_lTaxExcluded
        End Get
        Set(ByVal Value As Decimal)
            m_lTaxExcluded = Value

        End Set
    End Property
    <Browsable(True)>
    Public Property FeeDeposit() As Decimal
        Get
            Return m_lFeeDeposit
        End Get
        Set(ByVal Value As Decimal)
            m_lFeeDeposit = Value

        End Set
    End Property
    <Browsable(True)>
    Public Property TaxDeposit() As Decimal
        Get
            Return m_lTaxDeposit
        End Get
        Set(ByVal Value As Decimal)
            m_lTaxDeposit = Value

        End Set
    End Property
    'Start (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)
    <Browsable(True)>
    Public Property IsTrueMonthlypolicyandNextInstalmentRenewal() As Boolean
        Get
            Return m_bIsTrueMonthlypolicyandNextInstalmentRenewal
        End Get
        Set(ByVal Value As Boolean)
            m_bIsTrueMonthlypolicyandNextInstalmentRenewal = Value
        End Set
    End Property
    'End (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)

    ' RAW 13/11/2003 : CQ1765 : added
    <Browsable(False)>
    Public ReadOnly Property CancelOriginalPFPlanFlag() As Boolean
        Get
            If m_sProduct_Code = "MTA" Then
                If m_iMTAType = bSIRPremFinConst.m_klInstalmentMTAType_AddToNewPlan Then
                    Return False
                Else
                    Return True
                End If
            Else
                Return False
            End If
        End Get
    End Property

    <Browsable(False)>
    Public WriteOnly Property RunningContext() As String
        Set(ByVal Value As String)
            m_sRunningContext = Value
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property BackDatedMTAType() As Integer
        Set(ByVal Value As Integer)
            m_lBackDatedMTAType = Value
            'TR - Enable/Disable the Option Boxes and combo boxes based on this
            If m_lBackDatedMTAType = k_PFLastBDMTA Then
                'TR - During a Backated MTA this should be the only InsuranceFile
                'that has the Quotes screen displyed. But the Options should all
                'be disabled as the user cannot change the selectd Quote as it has
                'already been saved
                EnableOptionBoxes(False)
            Else
                EnableOptionBoxes(True)
            End If
        End Set
    End Property

    'Read/Write
    <Browsable(True)>
    Public Property PremiumFinanceCnt() As Integer
        Get
            Return m_lPremiumFinanceCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPremiumFinanceCnt = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property PremiumFinanceVersion() As Integer
        Get
            Return m_lPremiumFinanceVersion
        End Get
        Set(ByVal Value As Integer)
            m_lPremiumFinanceVersion = Value
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property PartyCnt() As Integer
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property InsuranceFileCnt_Renewal() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt_Renewal = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property PremiumFinanceTransactions() As Object
        Get
            Return m_vPFTransArray
        End Get
        Set(ByVal Value As Object)


            m_vPFTransArray = Value
        End Set
    End Property
    <Browsable(False)>
    Public ReadOnly Property DepositAmount() As Decimal
        Get
            Return m_crDepositAmount
        End Get
    End Property
    <Browsable(True)>
    Public Property IsPlanSelected() As Boolean
        Get
            ' Returns true if user has manually selected a plan
            Return m_bPlanSelected
        End Get
        Set(ByVal Value As Boolean)
            'Sets plan selected status
            m_bPlanSelected = Value
        End Set
    End Property
    <Browsable(False)> _
    Public ReadOnly Property FirstInstalmentAmt() As Decimal
        Get
            Return ToSafeDecimal(txtFirstInstalmentAmount.Text)
        End Get
    End Property

   
    '==========================
    'Standard Public Properties
    '==========================
    <Browsable(False)>
    Public ReadOnly Property ErrorNumber() As Integer
        Get
            ' Standard Property.
            ' Return any error number that might have occurred on the interface.
            Return m_lErrorNumber
        End Get
    End Property
    <Browsable(False)>
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Standard Property.
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property
    <Browsable(False)>
    Public ReadOnly Property Status() As Integer
        Get
            ' Standard Property.
            ' Return the interface exit status.
            Return m_lStatus
        End Get
    End Property
    <Browsable(True)>
    Public Property Task() As Integer
        Get
            ' Return the objects task.
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            ' Standard Property.
            ' Set the objects task.
            m_iTask = Value
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            ' Standard Property.
            ' Set the navigate flag.
            m_lNavigate = Value
        End Set
    End Property
    <Browsable(False)>
    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            ' Standard Property.
            ' Set the process mode.
            m_lProcessMode = Value
        End Set
    End Property
    <Browsable(True)>
    Public Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property
    <Browsable(False)>
    Public Property TransCurrencyID() As Integer
        Get
            Return m_iTransCurrencyID
        End Get
        Set(ByVal Value As Integer)
            m_iTransCurrencyID = Value
        End Set
    End Property
    <Browsable(False)>
    Public Property BaseCurrencyID() As Integer
        Get
            Return m_iBaseCurrencyID
        End Get
        Set(ByVal Value As Integer)
            m_iBaseCurrencyID = Value
        End Set
    End Property
    <Browsable(False)>
    Public Property TransISOCode() As String
        Get
            Return m_sTransISOCode
        End Get
        Set(ByVal Value As String)
            m_sTransISOCode = Value
        End Set
    End Property
    <Browsable(False)>
    Public Property BaseISOCode() As String
        Get
            Return m_sBaeISOCode
        End Get
        Set(ByVal Value As String)
            m_sBaeISOCode = Value
        End Set
    End Property

    Public ReadOnly Property TotalPayableAmount() As Decimal
        Get
            Return m_crTotalPayableAmount
        End Get
    End Property

    <Browsable(False)>
    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            ' Standard Property.
            ' Set the effective date.
            m_dtEffectiveDate = Value
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property StepStatus() As String
        Get
            ' Standard Property.
            ' Return the Steps Status
            Return m_sStepStatus.Value
        End Get
    End Property
    Private Property IsSinglePlanParty() As Boolean
        Get
            Return m_bSinglePlanParty
        End Get
        Set(ByVal Value As Boolean)
            m_bSinglePlanParty = Value
        End Set
    End Property
    '==================
    'Control Properties
    '==================
    <Browsable(False)>
    Public ReadOnly Property Controls_Renamed() As Object
        Get
            'Return Me.Controls_Renamed
            Return Me.Controls
        End Get
    End Property
    <Browsable(False)>
    Public ReadOnly Property Count() As Integer
        Get

            Return MyBase.Controls.Count
        End Get
    End Property
    <Browsable(False)>
    Public Shadows ReadOnly Property Tag() As String
        Get
            Return Convert.ToString(MyBase.Tag)
        End Get
    End Property
    <Browsable(False)>
    Public WriteOnly Property IsSingleInstalmentPlan() As Boolean
        Set(ByVal Value As Boolean)
            m_bIsSingleInstalmentPlan = Value
        End Set
    End Property
    'PN61609
    <Browsable(True)>
    Public Property IsFinanceAmountNetPremium() As Boolean
        Get
            Return m_bIsFinanceAmountNetPremium
        End Get
        Set(ByVal Value As Boolean)
            m_bIsFinanceAmountNetPremium = Value
            If Not (m_oBusiness Is Nothing) Then

                m_oBusiness.FinananceAmountNetPremium = m_bIsFinanceAmountNetPremium
            End If
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property QuoteAvailable() As Boolean
        Get
            Return m_bQuoteAvailable
        End Get
    End Property
    Public Property BaseCurrency As String = ""
    Public ReadOnly Property TotalPayable() As Decimal
        Get
            TotalPayable = ToSafeCurrency(txtTotalPayable.Text)
        End Get
    End Property

    ''' <summary>
    ''' Public function to refresh the Quote data and screen
    ''' </summary>
    ''' <returns>PMTrue for success</returns>
    ''' <remarks></remarks>
    Public Shadows Function Refresh() As Integer

        Dim result As Integer = PMEReturnCode.PMTrue

        Try

            Dim lReturnValue As gPMConstants.PMEReturnCode
            Dim sFrequency As String = ""
            Dim lDayOfWeekOrMonth As Integer
            Dim dtFirstInstalmentDate As Date

            ' temporarily set flag to limit processing of click events when control values are changed
            m_bProcessingEvent = True ' RAW 13/11/2003 : CQ1765 : added


            ' RAW 05/11/2003 : CQ2912, 2976 : moved from within RefreshQuoteData so that it is only executed first time
            If m_lPremiumFinanceVersion <> 0 AndAlso m_lPremiumFinanceCnt <> 0 Then
                ' RAW 05/11/2003 : CQ2912, 2976 : added 1st Instalment Date param
                lReturnValue = CType(GetPreferredDates(m_lPremiumFinanceVersion, m_lPremiumFinanceCnt, sFrequency, lDayOfWeekOrMonth, r_dtFirstInstalmentDate:=dtFirstInstalmentDate, v_sProductCode:=m_sProduct_Code), gPMConstants.PMEReturnCode)

                'If this fails for any reason, display a message and carry on
                'not critical enough to quit the process
                If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to load frequency preferences", vApp:=ACApp, vClass:=ACClass, vMethod:=gPMFunctions.ToSafeString(result))
                    result = gPMConstants.PMEReturnCode.PMFalse
                Else
                    Select Case sFrequency
                        Case "m"
                            cboMonthDay.SelectedIndex = lDayOfWeekOrMonth - 1
                        Case "w"
                            cboWeekDay.SelectedIndex = lDayOfWeekOrMonth - 1
                    End Select

                    'is dtFirstInstalmentDate in the list
                    'if not default to today's date
                    'Note: cboPreferredDate.ListIndex = 0 when combo is loaded
                    For lCount As Integer = 0 To cboPreferredDate.Items.Count - 1
                        If CDate(VB6.GetItemString(cboPreferredDate, lCount)) = dtFirstInstalmentDate Then
                            cboPreferredDate.SelectedIndex = lCount
                        End If
                    Next lCount
                End If

            End If

            'Need to initialise the variable as m_crAmount remains 0 when doing Plan MTA and trying to override the deposit.
            If gPMFunctions.ToSafeCurrency(txtFinancedAmount.Text) < 1 And m_sProduct_Code = "MTA" Then
                chkDepositOverride.Enabled = False
                txtOverrideDeposit.Text = "0.00"
            End If

            ' reset flag
            m_bProcessingEvent = False ' RAW 13/11/2003 : CQ1765 : added

            If m_bSGQuote AndAlso m_bRequote Then
                m_bRequote = False
                m_dOverrideDeposit = 0
                m_dOverrideRate = 0
            End If

            result = RefreshQuoteData()
            If m_sTransactionType = "MTC" AndAlso IsSinglePlanParty Then
                SSTabHelper.SetSelectedIndex(ssTabOptions, 2)
            End If

            Return result

        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="Refresh", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally

        End Try

        Return result
    End Function
    '*************************************************************************
    'Name:          SaveQuote
    'Description:   Public function to save the selected Quote
    'History:       25/11/2002 - TR - Created
    'Pn12144        Force refresh as user may select directly from third tab
    'Do not update display when refreshing quote array (as changing selected quote)
    '*************************************************************************
    Public Function SaveQuote() As Integer
        m_oBusiness.MakeLiveSingleInstalmentPlan = True
        'ACR 09-05-05 Don't requote for SG to persist overriden rates
        If gPMFunctions.ToSafeString(m_vQuoteArray(k_PFQuoteProductClass, m_lSelectedQuoteNo)).Trim() <> "SG" Then
            RefreshQuoteData(True)
        End If
        Return SaveSelectedQuoteToPlan()
    End Function

    Public Function LoadCurrencyInfo() As Integer

        Dim lReturnValue As Integer

        Dim lInsFileCnt As Integer = m_lInsuranceFileCnt

        If m_lInsuranceFileCnt_Renewal > 0 Then
            lInsFileCnt = m_lInsuranceFileCnt_Renewal
        End If


        lReturnValue = m_oBusiness.GetTransactionAndBaseCurrencyCode(lInsFileCnt, m_iTransCurrencyID, m_iBaseCurrencyID, m_sTransISOCode, m_sBaeISOCode, m_dXRate)

        If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If m_iTransCurrencyID <> m_iBaseCurrencyID AndAlso m_iACUserCanChangeInstalmentDefaultCurrency = 1 Then
            chkUseTransCurrency.CheckState = CheckState.Checked
        End If

        If m_iTransCurrencyID <> m_iBaseCurrencyID AndAlso m_iACUserCanChangeInstalmentDefaultCurrency = 1 Then
            chkUseTransCurrency.Enabled = True
        Else
            chkUseTransCurrency.Enabled = False
        End If

        If m_iMTAType = bSIRPremFinConst.m_klInstalmentMTAType_NoAmountChange Then
            chkUseTransCurrency.Enabled = False
        End If

        Return lReturnValue

    End Function

    ''' <summary>
    ''' RefreshQuoteData-Refreshes the data held in the modular array by calling
    ''' the business objects Calculate_Quotes methd
    ''' Added functionality to skip updating display.
    ''' </summary>
    ''' <param name="bDoNotUpdateDisplay"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function RefreshQuoteData(Optional ByRef bDoNotUpdateDisplay As Boolean = False) As Integer

        Dim nResult As gPMConstants.PMEReturnCode = PMEReturnCode.PMTrue
        Try

            Dim vUseExistingPaymentDetails As Object = Nothing '   RAM20030404 : Added this variable
            Dim vValue As Object = Nothing
            Dim dtPreferredDate As Date
            Dim bFinished As Boolean
            Dim lMsgRet As DialogResult
            Dim vSinglePlanParty As Object = Nothing
            Dim lInsuranceFileCnt As Integer
            Dim lMonthDay As Integer
            objfrmPassword = New frmPassword
            ShowProcessMessage(True)

            nResult = ValidateUserInput()

            m_bQuoteAvailable = True

            'Check for single Plan Party
            nResult = m_oBusiness.IsSinglePlanParty(v_lPartyCnt:=m_lInsuranceFileCnt, r_vSinglePlanParty:=vSinglePlanParty)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                'SetInterfaceDefaults = PMFalse
                Return nResult
            End If

            If Information.IsArray(vSinglePlanParty) Then

                If gPMFunctions.ToSafeBoolean(gPMFunctions.ToSafeString(vSinglePlanParty(0, 0))) Then
                    IsSinglePlanParty = True
                End If
                If m_lPremiumFinanceCnt = 0 Then

                    lInsuranceFileCnt = gPMFunctions.ToSafeLong(gPMFunctions.ToSafeString(vSinglePlanParty(0, 1)))
                Else
                    lInsuranceFileCnt = m_lInsuranceFileCnt
                End If
            Else
                lInsuranceFileCnt = m_lInsuranceFileCnt
            End If

            m_iUseTransactionCurrency = 0

            If chkUseTransCurrency.Checked Then
                m_iUseTransactionCurrency = 1
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                m_iUseTransactionCurrency = -1
            End If

            If nResult = gPMConstants.PMEReturnCode.PMTrue Then
                If IsSinglePlanParty Then
                    m_sProduct_Code = "MTA"
                    m_iMTAType = bSIRPremFinConst.m_klInstalmentMTAType_AddAndSpread
                Else
                    'TR - Work out if the Product is an MTA and needs extra data
                    If m_sProduct_Code.ToUpper() = "MTA" Then

                        If m_iMTAType = bSIRPremFinConst.m_klInstalmentMTAType_NoAmountChange Then
                            m_iUseTransactionCurrency = -1
                        Else
                            If optMTAType(0).Checked Then
                                m_iMTAType = bSIRPremFinConst.m_klInstalmentMTAType_AddAndSpread
                                m_iUseTransactionCurrency = -1
                            ElseIf optMTAType(1).Checked Then
                                m_iMTAType = bSIRPremFinConst.m_klInstalmentMTAType_AddToNext
                                m_iUseTransactionCurrency = -1
                            Else
                                m_iMTAType = bSIRPremFinConst.m_klInstalmentMTAType_AddToNewPlan
                            End If
                        End If
                    Else
                        m_iMTAType = bSIRPremFinConst.m_klInstalmentMTAType_AddAndSpread
                    End If
                End If

                lMonthDay = cboMonthDay.SelectedIndex + 1

                'TR - Get any missing data
                If Information.IsArray(m_vPFTransArray) Then
                    m_iNumberOfTransactions = m_vPFTransArray.GetUpperBound(1) + 1
                Else
                    m_iNumberOfTransactions = 1
                End If

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' RAM20030404   : Make sure you pass in the Selected Existing Payment
                '                   Details
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                nResult = CType(GetSelectedPaymentDetails(r_vSelectedPaymentDetails:=vUseExistingPaymentDetails), gPMConstants.PMEReturnCode)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do we need to log error ???

                End If

                If cboPreferredDate.Text = "" Then
                    dtPreferredDate = CDate(DateTime.Now.ToString("d"))
                Else
                    dtPreferredDate = CDate(cboPreferredDate.Text)
                End If



                m_oBusiness.TransType = m_sTransactionType

                nResult = m_oBusiness.SetProcessModes(vTask:=m_iTask)
                m_vQuoteArray = VB6.CopyArray(Nothing)

              '  If lInsuranceFileCnt = 0 AndAlso m_sProduct_Code = "REN" AndAlso m_lInsuranceFileCnt_Renewal <> 0 Then
               '     nResult = m_oBusiness.GetOriginalInsuranceFileCnt(nRenewalInsFileCnt:=m_lInsuranceFileCnt_Renewal, o_nOriginalInsFileCnt:=lInsuranceFileCnt)
              '      If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
              '          RaiseError("RefreshQuoteData", "Failed to get Original InsuranceFileCnt", gPMConstants.PMELogLevel.PMLogError)
              '      End If
              '  End If

                While Not (bFinished)

                    m_oBusiness.UseTransCurrency = m_iUseTransactionCurrency
                    If ToSafeDouble(txtFinancedAmount.Text) <= 1.0 AndAlso txtFinancedAmount.Text.Trim() <> "" Then
                        m_dOverrideDeposit = 0.0
                    ElseIf chkDepositOverride.Checked = False Then
                        m_dOverrideDeposit = -1.0
                    End If
                    nResult = m_oBusiness.Calculate_Quotes(m_lSource_ID, m_sProduct_Code, m_dtQuote_Date, m_dtStart_date, m_dtEnd_Date, dtPreferredDate, lMonthDay,
                                                                cboWeekDay.SelectedIndex + 1, m_crAmount, m_bPaymentProtection, m_dOverrideRate, m_bOverrideInterestRate,
                                                                m_lPartyCnt, m_vQuoteArray, m_lProduct_ID, m_lPremiumFinanceCnt, m_lPremiumFinanceVersion,
                                                                m_iMTAType, m_vPFTransArray, lInsuranceFileCnt, m_lBackDatedMTAType,
                                                                v_lRenewalInsFileCnt:=m_lInsuranceFileCnt_Renewal, v_vUseExistingPaymentDetails:=vUseExistingPaymentDetails,
                                                                v_dOverrideDeposit:=m_dOverrideDeposit, r_sSchemeName:=m_sSchemeName, r_sTransType:=m_sTransType)

                    Dim sTransactionISOCode As String = ""

                    If m_iTask = gPMConstants.PMEComponentAction.PMView Or m_iUseTransactionCurrency = -1 Then
                        If Information.IsArray(m_vQuoteArray) Then
                            If ToSafeInteger(m_vQuoteArray(k_PFUseTransCurrncy, 0)) = 1 Then
                                chkUseTransCurrency.CheckState = CheckState.Checked
                                sTransactionISOCode = m_sTransISOCode

                            Else
                                chkUseTransCurrency.CheckState = CheckState.Unchecked
                                sTransactionISOCode = m_sBaeISOCode
                            End If
                        End If
                    End If

                    If chkUseTransCurrency.Checked Then
                        sTransactionISOCode = m_sTransISOCode
                    Else
                        sTransactionISOCode = m_sBaeISOCode
                    End If

                    fraFinanceDetails.Text = "Finance Details" & " (" & sTransactionISOCode & ")"
                    fraMinDeposit.Text = "Minimum Deposit Required" & " (" & sTransactionISOCode & ")"
                    fraBreakdown.Text = "Breakdown" & " (" & sTransactionISOCode & ")"
                    fraSummary.Text = "Summary" & " (" & sTransactionISOCode & ")"


                    If m_oBusiness.ErrorCode.Trim() <> "" Then
                        If m_oBusiness.ErrorCode = 7 Then
                            lMsgRet = Interaction.MsgBox("There has been an error while connecting to the web service. " &
                                                         "Your user credentials are invalid. Would you like to update " &
                                                         "them now?", MsgBoxStyle.Exclamation Or MsgBoxStyle.YesNo, "Error")

                            If lMsgRet = System.Windows.Forms.DialogResult.Yes Then

                                objfrmPassword.ShowDialog()
                                If objfrmPassword.Status = gPMConstants.PMEReturnCode.PMOK Then
                                    lMsgRet = m_oBusiness.UpdateSGLogonDetails(objfrmPassword.UserName, objfrmPassword.Password, objfrmPassword.BrokerID, m_oBusiness.ErrorSchemeNo)
                                    bFinished = False
                                Else
                                    bFinished = True
                                End If
                                objfrmPassword.Close()
                            Else
                                bFinished = True
                            End If
                        Else
                            lMsgRet = Interaction.MsgBox("There has been an error while connecting to the web service." &
                                                         Strings.Chr(13) & Strings.Chr(10) & m_oBusiness.ErrorText,
                                                         MsgBoxStyle.Exclamation Or MsgBoxStyle.RetryCancel, "Error")

                            bFinished = Not (lMsgRet = System.Windows.Forms.DialogResult.Retry)
                        End If
                    Else
                        bFinished = True
                    End If

                End While

                'TR - Make sure that this worked OK
                If nResult = gPMConstants.PMEReturnCode.PMTrue Then
                    If m_lPremiumFinanceCnt > 0 AndAlso IsArray(m_vQuoteArray) Then
                        m_bHasInstalmentVersions = True
                    End If
                    'TR - If this is the Last BackDated MTA, replace the
                    '"original" Instalment amount with that of the First
                    'InsuranceFileCnt attached to this
                    If m_lBackDatedMTAType = k_PFLastBDMTA Then
                        m_vQuoteArray(k_PFQuoteOriginalOtherInstalmentAmount, 0) = m_crFirstOriginalInstalment
                        'TR - Now Reset the static
                        m_crFirstOriginalInstalment = 0
                    ElseIf m_lBackDatedMTAType = k_PFIntermediateBDMTA Then
                        'TR - If this is one of the "intermediate" instalment
                        'Types, then if it's the first, change the local
                        'variables to hold the amount of the Original Instalment
                        'TR - Only the first when the variable is zero
                        If m_crFirstOriginalInstalment = 0 Then
                            m_crFirstOriginalInstalment = CDec(m_vQuoteArray(k_PFQuoteOriginalOtherInstalmentAmount, 0))
                        End If
                    Else
                        'TR - Not a back dated MTA - do not interfere with Array
                        'data being passed to and from BO
                    End If

                    'TR - Display all the data on screen
                    ' PW210303 - only do this bit if the array is not empty. For MTC's the
                    ' Calculate_Quotes method on bSIRPremFinance does nothing and returns an
                    ' empty array.
                    ' ISS2877/ISS3151

                    If Information.IsArray(m_vQuoteArray) AndAlso Not (bDoNotUpdateDisplay) Then

                        'developer guide no. 98
                        iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTInstalmentDisplayStyle, v_vBranch:=g_iSourceID, r_vUnderwriting:=vValue)
                        If gPMFunctions.NullToString(vValue) = "1" Then
                            tdgListInstalment.Visible = True
                            tdgInstalment.Visible = False
                            nResult = BuildGridListStyle()
                        Else
                            tdgListInstalment.Visible = False
                            tdgInstalment.Visible = True
                            nResult = BuildGridArrayFromResultArray()
                        End If

                    End If

                    ' Alix - Check if we have quotes!
                    If Not Information.IsArray(m_vQuoteArray) Then
                        m_bQuoteAvailable = False
                    End If

                Else
                    'We have no quote available
                    m_bQuoteAvailable = False
                End If
                If m_bCallFromTab2 = False Then
                    'TR - Default to the first tab
                    SSTabHelper.SetSelectedIndex(ssTabMain, 0)
                End If
            End If

            If Not m_bQuoteAvailable Then
                tdgListInstalment.Visible = False
                tdgInstalment.Visible = False
            End If

            ShowProcessMessage(False)
            Return nResult

        Catch excep As System.Exception

            ShowProcessMessage(False)
            nResult = gPMConstants.PMEReturnCode.PMFail
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Refresh Quote Data Failed", ACApp, ACClass, "RefreshQuoteData", Information.Err().Number, excep.Message, excep:=excep)
            Return nResult
        End Try

    End Function

    ''' <summary>
    ''' Saves the Quote selected by the User to the database
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SaveSelectedQuoteToPlan() As Integer

        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Dim nTemp As gPMConstants.PMEReturnCode
        Dim oSinglePlanParty As Object = Nothing
        Dim bIsSinglePlanParty As Boolean

        Try

            'don't pass insurance file cnt to business object here as it will cause wrong insurance file cnt being
            'created for new version of plan.
            'if at this point insurance file cnt is wrong then fix the core rather botching here.
            '    'BE 10/9/2003 CQ1765 - ensure correct InsuranceFileCnt is set
            '    m_oBusiness.InsuranceFileCnt = m_lInsuranceFileCnt

            'TR - Get the business object to save this Quote as a Premium Finance

            m_oBusiness.IsSingleInstalmentPlan = m_bIsSingleInstalmentPlan
            m_vQuoteArray(k_PFQuoteHighlightCell, m_lSelectedQuoteNo) = True

            nResult = m_oBusiness.InsertOrUpdatePremiumFinance(v_vQuoteArray:=m_vQuoteArray, v_lSelectedQuoteIndex:=m_lSelectedQuoteNo,
                                                              v_vPFTransArray:=m_vPFTransArray, r_lPremiumFinanceCnt:=m_lPremiumFinanceCnt, r_lPremiumFinanceVer:=m_lPremiumFinanceVersion,
                                                              v_iMTAType:=m_iMTAType, v_vOriginalPlanArray:=Nothing)

            'TR - Get the selected deposit amount and save it for the public property
            'PN12821 Standardise deposit
            'm_crDepositAmount = m_vQuoteArray(k_PFQuoteDepositAmount, m_lSelectedQuoteNo)
            m_crDepositAmount = CDec(gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatMoney, Math.Round(CDbl(CDbl(m_vQuoteArray(k_PFQuoteDepositAmount, m_lSelectedQuoteNo))), 2)))

            'PN12821End
            'TR - If using the Underwriting Context, show the maintenance screen
            ' Alix Bergeret - 24/03/2003
            ' Don't display form if we are in renewal
            If m_sRunningContext.ToUpper() = "UWNB" AndAlso m_sProduct_Code <> "REN" Then

                'TR - Display the Finance Plan maintenance form
                ' Alix - 06/03/2003 - Issue 2805
                nTemp = OpenMaintenanceForm()
                If nTemp = gPMConstants.PMEReturnCode.PMCancel Then
                    ' Only override result if cancel
                    nResult = gPMConstants.PMEReturnCode.PMCancel
                End If
            End If
            If m_sCallingAppName = "iPMBFinancePlanQuote" AndAlso m_iMTAType = m_klInstalmentMTAType_AddAndSpread Then
                If m_oBusiness.IsSinglePlanAgent(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_vSinglePlanParty:=oSinglePlanParty) <> PMEReturnCode.PMTrue Then
                    RaiseError("m_oBusiness.IsSinglePlanAgent", "v_lInsuranceFileCnt:=" & m_lInsuranceFileCnt, PMELogLevel.PMLogError)
                End If

                If IsArray(oSinglePlanParty) Then
                    If ToSafeBoolean(oSinglePlanParty(0, 0)) Then
                        bIsSinglePlanParty = True
                    End If
                End If

                If bIsSinglePlanParty Then
                    SaveSelectedQuoteToPlan = m_oBusiness.UpdatePFTransId(v_lPfPremFinanceCnt:=m_lPremiumFinanceCnt,
                                                                          v_lInsuranceFileCnt:=m_lInsuranceFileCnt)
                End If
            End If
            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Save selected Quote", ACApp, ACClass, "SaveSelectedQuoteToPlan", Information.Err().Number, excep.Message, excep:=excep)
            Return nResult
        End Try

    End Function

    '*************************************************************************
    'Name:          ShowHideTopTabs
    'Description:   Shows or Hides the MTAType tab in the upper of the 2 tab bars
    'History:       09/12/2002 - TR - Created
    '*************************************************************************
    Private Function ShowHideTopTabs() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ShowHideTopTabs"
        Dim hSSTabMain As IntPtr = ssTabMain.Handle
        Dim hSSTabOptions As IntPtr = ssTabOptions.Handle
        Dim vUnderwriting As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            Dim bShowFirstTab As Boolean = False
            If m_sProduct_Code.ToUpper() = "MTA" Then
                'If the amount is not changing then no need to display the MTA tab
                If m_iMTAType = bSIRPremFinConst.m_klInstalmentMTAType_NoAmountChange Then
                    SSTabHelper.SetTabVisible(ssTabOptions, 1, False)
                    SSTabHelper.SetTabVisible(ssTabOptions, 0, True)
                    SSTabHelper.SetSelectedIndex(ssTabOptions, 0)
                Else
                    If m_bSGQuote Then
                        SSTabHelper.SetTabVisible(ssTabOptions, 1, False)
                        SSTabHelper.SetTabVisible(ssTabOptions, 0, False)
                        'ssTabOptions.Tab = Abs(Not m_bSGQuote)
                        If IsSinglePlanParty Then
                            SSTabHelper.SetTabVisible(ssTabOptions, 1, True)
                            SSTabHelper.SetTabVisible(ssTabOptions, 0, False)
                            SSTabHelper.SetSelectedIndex(ssTabOptions, 1)
                        End If
                    Else
                        SSTabHelper.SetTabVisible(ssTabOptions, 1, True)

                        If m_iMTAType = bSIRPremFinConst.m_klInstalmentMTAType_AddToNewPlan Then

                            bShowFirstTab = True
                            chkUseTransCurrency.Enabled = True
                        Else
                            chkUseTransCurrency.Enabled = False
                        End If

                        SSTabHelper.SetTabVisible(ssTabOptions, 0, bShowFirstTab)

                        If Not bShowFirstTab Then
                            SSTabHelper.SetSelectedIndex(ssTabOptions, 1)

                        End If
                    End If
                End If
            Else
                'Show the Preferred Days tab only
                SSTabHelper.SetTabVisible(ssTabOptions, 0, True)
                SSTabHelper.SetTabVisible(ssTabOptions, 1, False)
                SSTabHelper.SetSelectedIndex(ssTabOptions, 0)

                'Only show the Override tab if underwriting and allowed by Product Options
                iPMFunc.getUnderwritingOrAgency(r_vUnderwriting:=vUnderwriting)
                If vUnderwriting = "A" Then
                    SSTabHelper.SetTabVisible(ssTabMain, 2, False)
                Else
                    SSTabHelper.SetTabVisible(ssTabMain, 2, m_bAllowInstalmentsOverride)
                End If
            End If

            cboWeekDay.Enabled = Not (m_bSGQuote)
            cboMonthDay.Enabled = Not (m_bSGQuote)

            If m_bPaymentOptionsOnly Then
                SSTabHelper.SetTabVisible(ssTabOptions, 2, False)
                SSTabHelper.SetTabVisible(ssTabMain, 2, False)
                cboWeekDay.Enabled = False
                cboMonthDay.Enabled = False
                cboPreferredDate.Enabled = False
            End If

            If m_sUnderwriting = "A" Then
                SSTabHelper.SetTabVisible(ssTabMain, 3, False)
            End If
            '-------------------- PN 4299
            If m_sTransactionType = "MTC" And IsSinglePlanParty Then
                'ssTabOptions.Tab = 2
                SSTabHelper.SetSelectedIndex(ssTabOptions, 2)
            End If
            '-------------------- PN 4299


            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                cboWeekDay.Enabled = False
                cboMonthDay.Enabled = False
                cboPreferredDate.Enabled = False
                fraOverrideOptions.Enabled = False
                fraAdditionalOptions.Enabled = False
                chkUseTransCurrency.Enabled = False
            Else
                cboWeekDay.Enabled = True
                cboMonthDay.Enabled = True
                If Information.IsArray(m_vQuoteArray) AndAlso m_vQuoteArray(k_PFFirstInstalmentAlignWithMonthInDay, m_lSelectedQuoteNo) <> 1 Then
                    cboPreferredDate.Enabled = True
                      If m_sTransactionType <> "MTA" OrElse m_iMTAType <> bSIRPremFinConst.m_klInstalmentMTAType_AddAndSpread Then
                        txtFirstInstalDate.Text = cboPreferredDate.SelectedItem
                      End If 
                End If
                fraOverrideOptions.Enabled = True
                fraAdditionalOptions.Enabled = True
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here



            ' This is for debugging only



        End Try
        Return result
    End Function

    '*************************************************************************
    'Name:          SetInterfaceDefaults
    'Description:   Populates all of the controls with the minimum data and
    '               selects the default values
    'History:       15/11/2002 - TR - Created
    '*************************************************************************
    Private Function SetInterfaceDefaults() As Integer
        'Start (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)
        Dim result As Integer = 0
        Const kMethodName As String = "SetInterfaceDefaults"
        'End (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim sMondayString, sTuesdayString, sWednesdayString, sThursdayString, sFridayString As String


        Try

            'TR - Assume Success
            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Display / hide the top frame according to Product type
            ShowHideTopTabs()

            'TR - Get the localised strings

            'developer guide no.243
            sMondayString = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 704, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'developer guide no.243
            sTuesdayString = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 705, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'developer guide no.243
            sWednesdayString = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 706, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'developer guide no.243
            sThursdayString = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 707, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'developer guide no.243
            sFridayString = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 708, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'developer guide no.243
            m_sPaymentPeriodString = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 709, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'TR - Populate the combo boxes and select default
            With cboWeekDay
                .Items.Insert(0, sMondayString)
                .Items.Insert(1, sTuesdayString)
                .Items.Insert(2, sWednesdayString)
                .Items.Insert(3, sThursdayString)
                .Items.Insert(4, sFridayString)
                .SelectedIndex = 0
            End With

            'TR - Populate the combo boxes and select default
            For iCount As Integer = 1 To 31
                cboMonthDay.Items.Insert(iCount - 1, gPMFunctions.ToSafeString(iCount))
            Next iCount
            cboMonthDay.SelectedIndex = 0

            'ACR changed the following for PCL Personal to allow dates to be selected
            'from 30 days in arrears to 60 days in the future
            'For iCount As Integer = -30 To -1
            '    cboPreferredDate.Items.Add(DateTime.Now.AddDays(iCount).ToString("d"))
            'Next iCount

            'cboPreferredDate.Items.Add(DateTime.Now.ToString("d"))
            ' DD 18/08/2004 - Increased number of days to 400
            'For iCount As Integer = 1 To 400
            '    cboPreferredDate.Items.Add(DateTime.Now.AddDays(iCount).ToString("d"))
            'Next iCount
            'cboPreferredDate.SelectedIndex = 30

            'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.9.1.2)
            ' Display all language specific captions.
            lReturnValue = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '     'TR - Display all language specific captions.
            '    lReturnValue = DisplayCaptions(Me)
            '    'TR - Make sure that this worked OK, don't continue if it failed
            '    If lReturnValue <> PMTrue Then
            '        SetInterfaceDefaults = PMFalse
            '        Exit Function
            '    End If
            'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.9.1.2)

            lReturnValue = CType(iPMForms.SetFieldValidation(Me, m_oFormFields), gPMConstants.PMEReturnCode)

            'TR - Set Override Interest rate to -1 (for Business Object)
            m_dOverrideRate = -1
            m_dOverrideDeposit = -1 'Pn12144
            'TR - Set focus to one of the tabs
            'Call ssTabMain.SetFocus

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20030403   : Added the following code to populate the existing
            '                   Bank Account Details, Credit Card Details from
            '                   existing plan for this customer (if he had one)
            ' Ref.          : Issue 2915 - START
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            lReturnValue = CType(PopulateExistingPlanDetails(v_lPartyCnt:=m_lPartyCnt), gPMConstants.PMEReturnCode)
            ' Do we need to throw error ?
            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20030403   : Issue 2915 Changes - END
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            'TR - Select a default option in case the user doesn't bother
            optMTAType(0).Checked = True

            'Start (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)

            If m_bIsTrueMonthlypolicyandNextInstalmentRenewal Then
                optMTAType(1).Checked = True
            End If
            'End (Girija chokkalingam) - (Tech Spec - LOA004 -MTA Changes From Instalment to Invoice Policy.doc) - (5.2.1.1)



            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '*************************************************************************
    'Name:          Initialise (Standard Method)
    'Description:   initialisation code
    'History:       TR15112002 - Created as per TS23
    '*************************************************************************
    Private Function Initialise() As Integer

        Dim result As Integer = 0
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim sMessage, sTitle As String
        Dim sHelpFile As String = String.Empty
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily
        Dim vAllowInstalmentsOverrideResult As String = ""

        Try

            'TR - Assume success
            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bInitialised Then
                If cboPreferredDate.SelectedIndex = 0 Then
                    RefreshQuoteData()
                End If
                Return result
            Else
                m_bInitialised = True
            End If

            'Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            'Call the initialise method.
            'developer guide no. 123
            lReturnValue = g_oObjectManager.Initialise(sCallingAppName:=MainModule.ACApp)

            'TR - Make sure that this worked
            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Set the object manager to nothing.
                g_oObjectManager = Nothing
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to " & "initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If

            'Store the language ID from the object manager to the private variables
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_sUsername = .UserName
            End With

            ' Initialise the process modes.
            'm_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now
            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFOrion
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient
            m_iMTAType = -1

            'Find out from the registry where the Help File is
            lReturnValue = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)

            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrive Helpfile", Application.ProductName)
                Return result
            End If

            If sHelpFile <> "" Then
                'App.HelpFile = sHelpFile
            End If

            'Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            'TR - Get Hidden options
            iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTAllowInstalmentsOverride, m_lCompany_No, vAllowInstalmentsOverrideResult)

            'TR - Now save the value from this hidden option
            vAllowInstalmentsOverrideResult = "1"
            m_bAllowInstalmentsOverride = If(vAllowInstalmentsOverrideResult = "1", True, False)

            m_bPaymentProtection = True

            'Get an instance of the business object via the public object manager
            Dim temp_m_oBusiness As Object = Nothing
            lReturnValue = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPremiumFinance.Business", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness
            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                'Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                'TR - Get the message to display when the business object cannot
                'be initialised, from the resource file

                'developer guide No. 243
                sTitle = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 702, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                'Developer guide no. 243
                sMessage = gPMFunctions.ToSafeString(iPMFunc.GetResData(g_iLanguageID, 703, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If

            'Get Underwriting or Broking flag
            iPMFunc.getUnderwritingOrAgency(r_vUnderwriting:=m_sUnderwriting)

            Dim vParams As Object = Nothing
            Const ACUserCanChangeInstalmentDefaultCurrency As Integer = 23

            Dim temp_oUserAuthority As Object = Nothing
            Dim oUserAuthority As Object = Nothing

            lReturnValue = g_oObjectManager.GetInstance(temp_oUserAuthority, "bACTUserAuthorities.Business", vInstanceManager:="ClientManager")
            oUserAuthority = temp_oUserAuthority

            result = oUserAuthority.GetDetails(vUserid:=g_oObjectManager.UserID)

            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to initialise bACTUserAuthorities.Business", Application.ProductName)
                Return result
            End If

            result = oUserAuthority.GetNext(vParams:=vParams)

            If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to get the details for the User Authority", Application.ProductName)
                Return result
            End If

            chkUseTransCurrency.Enabled = False

            If Information.IsArray(vParams) Then
                If gPMFunctions.ToSafeInteger(vParams(ACUserCanChangeInstalmentDefaultCurrency), 0) = 1 Then
                    chkUseTransCurrency.Enabled = True
                    m_iACUserCanChangeInstalmentDefaultCurrency = gPMFunctions.ToSafeInteger(vParams(ACUserCanChangeInstalmentDefaultCurrency), 0)
                Else
                    chkUseTransCurrency.Enabled = False
                End If
            End If

            oUserAuthority.Dispose()
            oUserAuthority = Nothing


            'TR - Set the screen up
            lReturnValue = SetInterfaceDefaults()

            'TR - Rerturn this result

            Return lReturnValue

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to initialise the object", ACApp, ACClass, "Initialise", Information.Err().Number, excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    '*************************************************************************
    'Name:          IndexFromBookmark
    'Description:   TDBGrid function
    'History:       20/11/2002 - TR - Created
    '*************************************************************************
    Private Function IndexFromBookmark(ByRef r_vBookmark As String, ByRef r_lOffset As Integer) As Integer
        Dim lRowCounter As Integer

        If Not (m_objDataGridRows Is Nothing) Then

            If Convert.IsDBNull(r_vBookmark) Or IsNothing(r_vBookmark) Then
                If r_lOffset < 0 Then
                    lRowCounter = m_lGridMaxRowIndex + r_lOffset
                Else
                    lRowCounter = -1 + r_lOffset
                End If
            Else
                ' Convert string to long integer
                lRowCounter = gPMFunctions.ToSafeInteger(Conversion.Val(r_vBookmark) + r_lOffset)
            End If

            If lRowCounter >= 0 And lRowCounter < m_lGridMaxRowIndex Then
                Return lRowCounter
            Else
                Return -9999
            End If
        Else
            Return -9999
        End If
    End Function

    Private Function MakeBookmark(ByRef Index As Integer) As String
        Return gPMFunctions.ToSafeString(Index)
    End Function

    '*************************************************************************
    'Name:          GetRelativeBookmark
    'Description:   TDBGrid function
    'History:       20/11/2002 - TR - Created
    '*************************************************************************
    Private Function GetRelativeBookmark(ByRef r_vBookmark As String, ByRef r_lOffset As Integer) As String
        Dim lRowNumber As Integer

        If Not (m_objDataGridRows Is Nothing) Then
            lRowNumber = IndexFromBookmark(r_vBookmark, r_lOffset)
            If lRowNumber < 0 Or lRowNumber >= m_lGridMaxRowIndex Then

                Return Nothing
            Else
                Return MakeBookmark(lRowNumber)
            End If
        Else

            Return Nothing
        End If
    End Function

    '*************************************************************************
    'Name:          GetUserData
    'Description:   TDBGrid function
    'History:       20/11/2002 - TR - Created
    '*************************************************************************
    Private Function GetUserData(ByRef r_vBookmark As String, ByRef r_iColumnIndex As Integer) As String
        Dim sData As String = ""

        Dim lRowIndex As Integer = IndexFromBookmark(r_vBookmark, 0)
        If lRowIndex < 0 Or lRowIndex > m_lGridMaxRowIndex Or r_iColumnIndex < 0 Or r_iColumnIndex > m_lGridMaxColIndex Then
            Return "-"
        Else
            sData = ""
            Try
                sData = m_objDataGridRows.Row(lRowIndex).Cell(r_iColumnIndex).DataDisplayValue
            Catch ex As Exception
            End Try
            If sData.Trim() = "" Then
                Return "-"
            Else
                Return sData
            End If
        End If
    End Function

    ''' <summary>
    '''Before getting new quotes from the business object, this
    '''checks that the user has entered details correctly
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateUserInput() As Integer

        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Try
            If Not m_bEnableOveridesViaTabOnly Then
                Return nResult
            End If

            'TR - Assume success

            'TR - first check that if an interest override has been selected, a
            'rate has been added
            If m_bOverrideInterestRate Then
                If m_dOverrideRate < 0 Then
                    ValidateUserInput = gPMConstants.PMEReturnCode.PMFail
                    'txtNewRate.SetFocus
                    SSTabHelper.SetTabVisible(ssTabMain, 2, True)
                ElseIf m_sOverrideReference = "" Then
                    ValidateUserInput = gPMConstants.PMEReturnCode.PMFail
                    txtOverrideReference.Focus()
                    SSTabHelper.SetTabVisible(ssTabMain, 2, True)
                End If
            ElseIf m_bOverrideCommission Then
                If m_sOverrideReference = "" Then
                    ValidateUserInput = gPMConstants.PMEReturnCode.PMFail
                    txtOverrideReference.Focus()
                    SSTabHelper.SetTabVisible(ssTabMain, 2, True)
                End If
            End If
            'PN12144
            If m_bOverrideDeposit Then
                If m_dOverrideDeposit < 0 Then
                    ValidateUserInput = gPMConstants.PMEReturnCode.PMFail
                    txtOverrideDeposit.Focus()
                    SSTabHelper.SetTabVisible(ssTabMain, 2, True)
                End If
            End If
            '    'PN12144end
            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Validate User Input", ACApp, ACClass, "ValidateUserInput", Information.Err().Number, excep.Message, excep:=excep)
            Return nResult
        End Try

    End Function

    '*************************************************************************
    'Name:          BuildGridArrayFromResultArray
    'Description:   Takes the recordset returned by the Calculate_Quotes call
    '               to the BO. Converts it into another array of the same
    '               shape as the data will look like in the grid
    'History:       20/11/2002 - TR - Created
    '*************************************************************************
    Private Function BuildGridArrayFromResultArray() As Integer

        Dim result As Integer = 0
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim iGridRowIndex, iUboundRow, iLBoundRow, iColumnIndexOfMediaType, iLoopLowestRowNo As Integer
        Dim objCurrentCell As clsDataGridCell
        Dim objCurrentRow As clsDataGridRow
        Dim bNewRow As Boolean

        'TR - Variables to hold each record in the recordset array
        Dim iFrequencyID, iMediaTypeID As Integer
        Dim sMediaTypeDesc, sFrequencyDesc As String
        Dim sAmount As New StringBuilder
        Dim bHighlightCell As Boolean

        'SMJB CQ1824 17/10/03
        Dim bFoundHighlightCell As Boolean

        Try

            'TR - clear the collection of Rows
            m_bInitialisingGrid = True
            m_objDataGridRows = New clsDataGridRows()
            objCurrentRow = New clsDataGridRow()
            ' RAW 05/11/2003 : CQ1824 : removed code that initialises m_iSelectedRow and Column indexes
            m_iExistingColumnIndex = -1
            m_iExistingRowIndex = -1
            m_lSelectedQuoteNo = -1

            m_vMediaColumnsInGrid = Nothing
            iGridRowIndex = -1

            'TR - Loop through all the records in the resulting Array
            iUboundRow = m_vQuoteArray.GetUpperBound(1)
            iLBoundRow = m_vQuoteArray.GetLowerBound(1)

            'TR - Loop through the Rows
            For iOrigArrayRowCount As Integer = iLBoundRow To iUboundRow
                'TR - Get all the required data from this row
                iFrequencyID = gPMFunctions.ToSafeInteger(m_vQuoteArray(k_PFQuoteFrequencyID, iOrigArrayRowCount))
                iMediaTypeID = gPMFunctions.ToSafeInteger(m_vQuoteArray(k_PFQuoteMediaTypeID, iOrigArrayRowCount))
                sMediaTypeDesc = gPMFunctions.ToSafeString(m_vQuoteArray(k_PFQuoteMediaTypeDescription, iOrigArrayRowCount))
                sFrequencyDesc = gPMFunctions.ToSafeString(m_vQuoteArray(k_PFQuoteFrequencyDescription, iOrigArrayRowCount))
                bHighlightCell = gPMFunctions.ToSafeString(m_vQuoteArray(k_PFQuoteHighlightCell, iOrigArrayRowCount)).ToUpper() = "TRUE"
                'TR - In the case of MTA, Display existing amount in brackets
                'after new Quote amount
                If bHighlightCell And m_sProduct_Code.ToUpper() = "MTA" Then
                    txtInvisible.Text = ""
                    m_oFormFields.FormatControl(txtInvisible, gPMFunctions.ToSafeString(m_vQuoteArray(k_PFQuoteOtherInstalmentAmount, iOrigArrayRowCount)))
                    sAmount = New StringBuilder(txtInvisible.Text & " (")
                    txtInvisible.Text = ""
                    m_oFormFields.FormatControl(txtInvisible, gPMFunctions.ToSafeString(m_vQuoteArray(k_PFQuoteOriginalOtherInstalmentAmount, iOrigArrayRowCount)))
                    sAmount.Append(txtInvisible.Text & ")")
                Else
                    txtInvisible.Text = ""
                    m_oFormFields.FormatControl(txtInvisible, gPMFunctions.ToSafeString(m_vQuoteArray(k_PFQuoteOtherInstalmentAmount, iOrigArrayRowCount)))
                    sAmount = New StringBuilder(txtInvisible.Text)
                End If
                'TR - Each Quote in the array represents a new DataGridCell, but
                'not necessarily a new DataGridRow
                objCurrentCell = New clsDataGridCell()
                'TR - Get all the required data from this row
                With objCurrentCell
                    .DataDisplayValue = sAmount.ToString()
                    .FrequencyID = iFrequencyID
                    .MediaTypeID = iMediaTypeID
                    'TR - No Quote ID in array, so use Row No instead
                    .OriginatingQuoteNo = iOrigArrayRowCount
                End With

                'TR - See if this Media has been listed before. If not a
                'new column will be required!
                lReturnValue = CType(FindOrAddMediaColumnIndex(iMediaTypeID, sMediaTypeDesc, iColumnIndexOfMediaType), gPMConstants.PMEReturnCode)

                'TR - Make sure that this worked OK
                If lReturnValue = gPMConstants.PMEReturnCode.PMTrue Then
                    'TR - Set the column No for this cell
                    objCurrentCell.ColumnIndex = iColumnIndexOfMediaType
                    'TR - Do we need to start a new row?
                    'If the frequency is -1 or different to last time round then
                    'this must be the first cell or a new row. New row either way.
                    If objCurrentRow.FrequencyID = -1 Or objCurrentRow.FrequencyID <> iFrequencyID Then
                        'TR - Populate this row
                        objCurrentRow = New clsDataGridRow()
                        iGridRowIndex += 1
                        With objCurrentRow
                            .FrequencyDesc = sFrequencyDesc
                            .FrequencyID = iFrequencyID
                            .MediaTypeDesc = sMediaTypeDesc
                            .RowIndex = iGridRowIndex


                            'Changes as per Vb code
                            '.Cells.Add(objCurrentCell, CStr(objCurrentCell.ColumnIndex))
                            'm_objDataGridRows.Rows.Add(objCurrentRow, CStr(.RowIndex))
                            'TR - Now Create the Frequency Column
                            objCurrentCell = New clsDataGridCell()
                            With objCurrentCell
                                .FrequencyID = iFrequencyID
                                .ColumnIndex = 0
                                .DataDisplayValue = sFrequencyDesc
                                .RowIndex = iGridRowIndex
                            End With


                        End With
                        'TR - Frequencies are the same
                    Else
                        'TR - Add this to the grid on the same line if possible
                        'and if not, put on a new row, but with the same Frequency
                        'as before
                        lReturnValue = CType(FindOrAddMediaFrequencyRow(iMediaTypeID, iFrequencyID, iLoopLowestRowNo, bNewRow), gPMConstants.PMEReturnCode)
                        'TR - Make sure that this worked OK
                        If lReturnValue = gPMConstants.PMEReturnCode.PMTrue Then
                            'TR - Check that this is an existing Frequency Type
                            If iLoopLowestRowNo <> -1 Then
                                'TR - Check if a Row exists with a space for this
                                'Media Type, or if we have to create a new one
                                If bNewRow Then
                                    'TR - Need to add another new row
                                    'TR - Populate this row
                                    objCurrentRow = New clsDataGridRow()
                                    iGridRowIndex += 1
                                    With objCurrentRow
                                        .FrequencyDesc = sFrequencyDesc
                                        .MediaTypeDesc = sMediaTypeDesc
                                        .FrequencyID = iFrequencyID
                                        .RowIndex = iGridRowIndex


                                    End With
                                    'TR - Now Create the Frequency Column
                                    objCurrentCell = New clsDataGridCell()
                                    With objCurrentCell
                                        .ColumnIndex = 0
                                        .FrequencyID = iFrequencyID
                                        .MediaTypeID = -1
                                        .DataDisplayValue = objCurrentRow.FrequencyDesc
                                    End With

                                Else
                                    'TR - Get the existing row
                                    objCurrentRow = m_objDataGridRows.Rows(gPMFunctions.ToSafeString(iLoopLowestRowNo))
                                    'TR - Add this cell to this row

                                End If
                            Else
                                'TR - Need to add another new row
                                'TR - Populate this row
                                objCurrentRow = New clsDataGridRow()
                                iGridRowIndex += 1
                                With objCurrentRow
                                    .FrequencyDesc = sFrequencyDesc
                                    .MediaTypeDesc = sMediaTypeDesc
                                    .FrequencyID = iFrequencyID
                                    .RowIndex = iGridRowIndex


                                End With
                                'TR - Now Create the Frequency Column
                                objCurrentCell = New clsDataGridCell()
                                With objCurrentCell
                                    .ColumnIndex = 0
                                    .FrequencyID = iFrequencyID
                                    .MediaTypeID = -1
                                    .DataDisplayValue = objCurrentRow.FrequencyDesc
                                End With

                            End If
                        End If
                    End If

                    'TR - Do we set this as the highlighted cell?
                    If bHighlightCell Then
                        ' RAW 05/11/2003 : CQ1824 : added if test
                        If m_iSelectedColumnIndex = -1 Then
                            ' only set value if still initialised (ie do not overwrite existing contents)
                            m_iSelectedColumnIndex = iColumnIndexOfMediaType
                        End If
                        m_iExistingColumnIndex = iColumnIndexOfMediaType

                        ' RAW 05/11/2003 : CQ1824 : added if test
                        If m_iSelectedRowIndex = -1 Then
                            ' only set value if still initialised (ie do not overwrite existing contents)
                            m_iSelectedRowIndex = iGridRowIndex
                        End If
                        m_iExistingRowIndex = iGridRowIndex

                        bFoundHighlightCell = True
                        bHighlightCell = False
                    End If

                    'TR - Error, should have found or added to array
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find Media Type", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildGridArrayFromResultArray")

                End If
            Next iOrigArrayRowCount

            'TR - Save the new max Rows/cols values

            m_lGridMaxColIndex = m_vMediaColumnsInGrid.GetUpperBound(1) + 1
            m_lGridMaxRowIndex = iGridRowIndex + 1
            'TR - Build the Grid for this data set
            SetupDataGrid()
            'Call RefreshPlanDataOnScreen
            ChangeSelectedQuote()
            m_bInitialisingGrid = False

            ' RAW 05/11/2003 : CQ1824 : changed conditions for setting focus to cell
            If m_iSelectedColumnIndex = -1 Then
                m_iSelectedColumnIndex = 1
            End If
            If m_iSelectedRowIndex = -1 Then
                m_iSelectedRowIndex = 0
            End If
            If m_iSelectedColumnIndex <= tdgInstalment.Columns.Count Then
                If Not IsNothing(tdgInstalment.CurrentRow) Then
                    tdgInstalment.CurrentCell = tdgInstalment.CurrentRow.Cells(m_iSelectedColumnIndex)
                End If
            End If
            If m_iSelectedRowIndex <= m_objDataGridRows.Rows.Count Then

                'Developer Guide No. no soultion found
                'tdgInstalment.Bookmark = m_iSelectedRowIndex
                ListInstalmentSelectRow()
            End If
            ' RAW 05/11/2003 : CQ1824 : end

            'TR - Destroy all objects
            objCurrentCell = Nothing
            objCurrentRow = Nothing
            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Build Data Array", ACApp, ACClass, "BuildGridArrayFromResultArray", Information.Err().Number, excep.Message, excep:=excep)
            Return result

            Return result
        End Try
    End Function


    '*************************************************************************
    'Name:          SetupDataGrid
    'Description:   After the result set has been processed, but before the
    '               data is displayed, we need to add the correct number of
    '               columns to the grid and setup columns & headers correctly
    'History:       20/11/2002 - TR - Created
    '*************************************************************************
    Private Sub SetupDataGrid()

        Dim iUBoundColumn As Integer
        Dim tdgColumn As DataGridViewColumn

        Try

            'TR - First clear down all the existing headings
            Do Until tdgInstalment.Columns.Count <= 0
                tdgInstalment.Columns.RemoveAt(0)
            Loop

            'TR - Get the number of different media types (zero based)

            iUBoundColumn = m_vMediaColumnsInGrid.GetUpperBound(1)

            Dim newColumn As Artinsoft.Windows.Forms.DataGridViewExtendedColumn
            newColumn = New Artinsoft.Windows.Forms.DataGridViewExtendedColumn()
            tdgInstalment.Columns.Insert(0, newColumn)
            tdgColumn = newColumn
            With tdgColumn
                .ReadOnly = True
                .HeaderText = m_sPaymentPeriodString
                .Visible = True
                .Width = 2000
                .DefaultCellStyle.Alignment = ContentAlignment.MiddleCenter
            End With

            'TR - Give the grid the correct no of columns for the number of
            'different media types
            For iIndex As Integer = 0 To iUBoundColumn
                newColumn = New Artinsoft.Windows.Forms.DataGridViewExtendedColumn()
                tdgInstalment.Columns.Insert(iIndex + 1, newColumn)
                tdgColumn = newColumn
                With tdgColumn

                    'TODO no solution found
                    '.FetchStyle = True
                    .ReadOnly = True

                    .HeaderText = gPMFunctions.ToSafeString(m_vMediaColumnsInGrid(1, iIndex))
                    .Visible = True
                    .Width = 1500
                    .DefaultCellStyle.Alignment = ContentAlignment.MiddleCenter
                End With
            Next iIndex

            'TR - Now get all the data for the grid
            tdgInstalment.ReBind()
            tdgColumn = Nothing

        Catch excep As System.Exception


            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Setup TDBGrid", ACApp, ACClass, "SetupDataGrid", Information.Err().Number, excep.Message, excep:=excep)
            Exit Sub
        End Try

    End Sub

    '*************************************************************************
    'Name:          ChangeSelectedQuote
    'Description:   After the selected cell has been changed, find the correct Quote
    'History:       20/11/2002 - TR - Created
    '*************************************************************************
    Private Sub ChangeSelectedQuote(Optional ByVal v_iSelectedColumnIndex As Integer = -1, Optional ByVal v_iSelectedRowIndex As Integer = -1)

        Dim objDataGridRow As clsDataGridRow
        Dim objDataGridCell As clsDataGridCell
        Dim iSelectedColumnIndex, iSelectedRowIndex, iQuoteLineNumber As Integer


        'developer guide no 32. 


        'TR - Work out which selected Indexes to use
        If v_iSelectedColumnIndex = -1 Then
            iSelectedColumnIndex = m_iSelectedColumnIndex
        Else
            iSelectedColumnIndex = v_iSelectedColumnIndex
        End If
        If v_iSelectedRowIndex = -1 Then
            iSelectedRowIndex = m_iSelectedRowIndex
        Else
            iSelectedRowIndex = v_iSelectedRowIndex
        End If

        'TR - Special Error handling here as selected cell may not have
        'an assciated cell object

        Try

            'TR - Get the Highlighted/selected cell
            objDataGridRow = m_objDataGridRows.Row(iSelectedRowIndex)

            'TR - Get the selected Cell
            objDataGridCell = objDataGridRow.Cells(gPMFunctions.ToSafeString(iSelectedColumnIndex))

            'TR - Does this cell exist?
            If objDataGridCell Is Nothing Then
                'TR - No Quote for this cell, revert to previous - so go back to previous cell
                If m_iSelectedColumnIndex = 0 Then
                    m_iSelectedColumnIndex = 1
                End If

                If Not IsNothing(tdgInstalment.CurrentRow) Then
                    tdgInstalment.CurrentCell = tdgInstalment.CurrentRow.Cells(m_iSelectedColumnIndex)
                End If

                ListInstalmentSelectRow()
                tdgInstalment.SelLength = 0

            Else
                iQuoteLineNumber = objDataGridCell.OriginatingQuoteNo
                m_lSelectedQuoteNo = iQuoteLineNumber
                m_iSelectedColumnIndex = iSelectedColumnIndex
                m_iSelectedRowIndex = iSelectedRowIndex
                'TR - Now refresh the screen
                RefreshQuoteDataOnScreen()
            End If

            Exit Sub

Err_ChangeSelectedQuote:
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Select this Quote", vApp:=ACApp, vClass:=ACClass, vMethod:="ChangeSelectedQuote", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Catch exc As System.Exception
        End Try
    End Sub

    '*************************************************************************
    'Name:          RefreshQuoteDataOnScreen
    'Description:   After the selected Quote has been changed, display the
    '               new Quote data - using m_lSelectedQuoteNo
    'History:       20/11/2002 - TR - Created
    '*************************************************************************
    Private Sub RefreshQuoteDataOnScreen()
        Dim iReturn As Integer = 0
        Try
            With m_oFormFields
                'TR - Populate the controls
                .FormatControl(txtTransactions, m_iNumberOfTransactions)
                .FormatControl(txtFinancedAmount, m_vQuoteArray(k_PFQuoteTotalAmountInput, m_lSelectedQuoteNo))
                .FormatControl(txtRate, m_vQuoteArray(k_PFQuoteInterestRate, m_lSelectedQuoteNo))
                .FormatControl(txtTotalPayable, m_vQuoteArray(k_PFQuoteTotalInstalmentsAmount, m_lSelectedQuoteNo))
                .FormatControl(txtInstalments, m_vQuoteArray(k_PFQuoteInstalmentsToPay, m_lSelectedQuoteNo))
                .FormatControl(txtApr, m_vQuoteArray(k_PFQuoteAprRate, m_lSelectedQuoteNo))
                .FormatControl(txtDeposit, m_vQuoteArray(k_PFQuoteDepositAmount, m_lSelectedQuoteNo))
                .FormatControl(txtProtection, m_vQuoteArray(k_PFQuoteProtectionAmount, m_lSelectedQuoteNo))
                .FormatControl(txtAdminCharge, m_vQuoteArray(k_PFQuoteFinanceCharge, m_lSelectedQuoteNo))
                .FormatControl(txtInterest, m_vQuoteArray(k_PFQuoteInterestAmount, m_lSelectedQuoteNo))

                'MKW 140604 PN12422 Do Not Populate First, Next and Last Instalments for third party Instalments START
                If gPMFunctions.NullToString(m_vQuoteArray(k_PFQuoteSchemeTypeCode, m_lSelectedQuoteNo)) = "TP" Then
                    .FormatControl(txtFirstInstalDate, "")
                    .FormatControl(txtNextInstalDate, "")
                    .FormatControl(txtLastInstalDate, "")
                Else
                    .FormatControl(txtFirstInstalDate, CDate(m_vQuoteArray(k_PFQuoteFirstInstalmentDate, m_lSelectedQuoteNo)).ToString("d"))
                    .FormatControl(txtNextInstalDate, CDate(m_vQuoteArray(k_PFQuoteNextInstalmentDate, m_lSelectedQuoteNo)).ToString("d"))
                    .FormatControl(txtLastInstalDate, CDate(m_vQuoteArray(k_PFQuoteLastInstalmentDate, m_lSelectedQuoteNo)).ToString("d"))
                End If
                'MKW 140604 PN12422 END

                .FormatControl(txtFirstInstalmentAmount, m_vQuoteArray(k_PFQuoteFirstInstalmentAmount, m_lSelectedQuoteNo))
                .FormatControl(txtOtherInstalmentAmount, m_vQuoteArray(k_PFQuoteOtherInstalmentAmount, m_lSelectedQuoteNo))
                .FormatControl(txtTaxes, m_vQuoteArray(k_PFQuoteTaxAmount, m_lSelectedQuoteNo))
                lblSummary.Text = gPMFunctions.ToSafeString(m_vQuoteArray(k_PFQuoteMediaTypeDescription, m_lSelectedQuoteNo)) & ", " &
                      gPMFunctions.ToSafeString(m_vQuoteArray(k_PFQuoteFrequencyDescription, m_lSelectedQuoteNo))

                'BPIS

                If chkUseTransCurrency.Checked = False And m_dXRate <> 1 Then
                    'Finance Details Conversion
                    txtGross.Text = StringsHelper.Format(m_lGrossDue * m_dXRate, "0.00")
                    txtTotalFee.Text = StringsHelper.Format(m_lExcluded * m_dXRate, "0.00")
                    txtTotalTax.Text = StringsHelper.Format(m_lTaxExcluded * m_dXRate, "0.00")
                    txtAmountFinanced.Text = StringsHelper.Format((m_lGrossDue - (m_lExcluded + m_lTaxExcluded)) * m_dXRate, "0.00")
                    'Minimum deposit required Conversion
                    txtFeeDeposit.Text = StringsHelper.Format(m_lFeeDeposit * m_dXRate, "0.00")
                    txtTaxDeposit.Text = StringsHelper.Format(m_lTaxDeposit * m_dXRate, "0.00")
                    txtMinDeposit.Text = StringsHelper.Format((m_lFeeDeposit + m_lTaxDeposit) * m_dXRate, "0.00")
                Else
                    txtGross.Text = StringsHelper.Format(m_lGrossDue, "0.00")
                    txtTotalFee.Text = StringsHelper.Format(m_lExcluded, "0.00")
                    txtTotalTax.Text = StringsHelper.Format(m_lTaxExcluded, "0.00")
                    txtAmountFinanced.Text = StringsHelper.Format(m_lGrossDue - (m_lExcluded + m_lTaxExcluded), "0.00")
                    txtFeeDeposit.Text = StringsHelper.Format(m_lFeeDeposit, "0.00")
                    txtTaxDeposit.Text = StringsHelper.Format(m_lTaxDeposit, "0.00")
                    txtMinDeposit.Text = StringsHelper.Format(m_lFeeDeposit + m_lTaxDeposit, "0.00")
                End If

                If m_lGrossDue = 0 Then

                    Dim dGrossDue As Decimal = ToSafeDouble(m_vQuoteArray(k_PFPlanAmountToFinance, m_lSelectedQuoteNo))
                    If chkUseTransCurrency.Checked = False And m_dXRate <> 0.0 Then
                        dGrossDue = dGrossDue / m_dXRate
                    End If

                    txtGross.Text = StringsHelper.Format(dGrossDue + (m_lExcluded + m_lTaxExcluded), "0.00")
                    txtAmountFinanced.Text = StringsHelper.Format(dGrossDue, "0.00")

                End If

                m_crTotalPayableAmount = Val(txtTotalPayable.Text)

                'PN72107
                cboPreferredDate.Enabled = True
                m_bStopClickEvent = True
                If Not m_bHasDatesChange Then
                    For iCount As Integer = 0 To cboPreferredDate.Items.Count - 1
                        If VB6.GetItemString(cboPreferredDate, iCount) = gPMFunctions.ToSafeString(CDate(m_vQuoteArray(k_PFQuoteFirstInstalmentDate, m_lSelectedQuoteNo)), "dd/mm/yyyy") Then
                            cboPreferredDate.SelectedIndex = iCount
                            Exit For
                        End If
                    Next
                End If
                If m_vQuoteArray(k_PFFirstInstalmentAlignWithMonthInDay, m_lSelectedQuoteNo) = 1 Then
                    cboPreferredDate.Enabled = False
                End If
                'PN72107
                m_bStopClickEvent = False
                'Dim sname As String = ""
                'sname = m_oBusiness.m_sSchemeName
                'For j As Integer = 0 To tdgListInstalment.RowCount - 1
                '    If tdgListInstalment.Rows(j).Cells(0).FormattedValue() = sname Then
                '        tdgListInstalment.Rows(j).Selected = True

                '    End If
                'Next
                If m_vQuoteArray(k_PFAlignTo, m_lSelectedQuoteNo) = 1 Then
                    cboMonthDay.Enabled = True
                ElseIf m_vQuoteArray(k_PFAlignTo, m_lSelectedQuoteNo) = 0 Then
                    cboMonthDay.Enabled = False
                End If
                If m_vQuoteArray(k_PFQuoteTotalAmountInput, m_lSelectedQuoteNo) > 0 Then
                    chkDepositOverride.Enabled = True
                Else
                    chkDepositOverride.Enabled = False
                End If
            End With

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Refresh Screen", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshQuoteDataOnScreen", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try
    End Sub

    '*************************************************************************
    'Name:          FindOrAddMediaColumnIndex
    'Description:   Looks in an array for a media Type that has been listed
    '               before. If cannot find it, will add new item to array and
    '               return index to new item. One based (zero is not a media
    '               type column)
    'History:       20/11/2002 - TR - Created
    '*************************************************************************
    Private Function FindOrAddMediaColumnIndex(ByVal v_iMediaTypeID As Integer, ByVal v_sMediaDescription As String, ByRef r_iIndexInArray As Integer) As Integer

        Dim result As Integer = 0
        Dim iUbound, iNewIndexOfMedia, iIndex As Integer

        Try

            'TR - Assume success
            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(m_vMediaColumnsInGrid) Then

                'TR - Find the first and last entries in the array

                iIndex = m_vMediaColumnsInGrid.GetLowerBound(1)

                iUbound = m_vMediaColumnsInGrid.GetUpperBound(1)

                'TR - Loop through them all. Look for MediaTypeID
                Do Until iIndex > iUbound

                    If CDbl(m_vMediaColumnsInGrid(0, iIndex)) = v_iMediaTypeID Then
                        'TR - Stop looking. Return the Index of this Entry plus 1
                        '(to allow for first column containing Frequency description
                        r_iIndexInArray = iIndex + 1
                        Return result
                    End If
                    iIndex += 1
                Loop

                'TR - If we have reached here then this media type is not listed already
                iNewIndexOfMedia = iUbound + 1

                'TR - Add 1 record to the variant.
                ReDim Preserve m_vMediaColumnsInGrid(1, iNewIndexOfMedia)

                'TR - Add this new Media Type to the array

                m_vMediaColumnsInGrid(0, iNewIndexOfMedia) = v_iMediaTypeID

                m_vMediaColumnsInGrid(1, iNewIndexOfMedia) = v_sMediaDescription

                'TR - Now return the new Index (plus one for Frequency column)
                r_iIndexInArray = iNewIndexOfMedia + 1
            Else
                'TR - Create the variant
                ReDim m_vMediaColumnsInGrid(1, 0)

                'TR - Add this new Media Type to the array

                m_vMediaColumnsInGrid(0, 0) = v_iMediaTypeID

                m_vMediaColumnsInGrid(1, 0) = v_sMediaDescription

                'TR - Now return the new Index
                r_iIndexInArray = 1
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFail
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error during Media Array lookup", vApp:=ACApp, vClass:=ACClass, vMethod:="FindOrAddMediaColumnIndex", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    '*************************************************************************
    'Name:          FindOrAddMediaFrequencyRow
    'Description:   Looks through all the existing Row and Cell obejects for
    '               an existing value for this MediaType/Frequency combination
    '               Returns the number of the first empty row for this.
    'History:       20/11/2002 - TR - Created
    '*************************************************************************
    Private Function FindOrAddMediaFrequencyRow(ByVal v_iMediaTypeID As Integer, ByVal v_iFrequencyTypeID As Integer, ByRef r_iRowIndex As Integer, ByRef r_bCreateNewRow As Boolean) As Integer

        Dim result As Integer = 0
        Try

            Dim iHighestRowNoPopulatedWithThisMediaType, iLowestRowNoForThisFrequency, iHighestRowNoForThisFrequency As Integer

            'TR - Assume success
            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Intialise variables
            iHighestRowNoPopulatedWithThisMediaType = -1
            iLowestRowNoForThisFrequency = -1
            iHighestRowNoForThisFrequency = -1
            r_bCreateNewRow = False

            'TR - loop through all the existing rows
            For Each objDataGridRow As clsDataGridRow In m_objDataGridRows.Rows
                With objDataGridRow
                    'TR - Only bother looking if the Frequency Type and Media
                    'Type are right
                    If .FrequencyID = v_iFrequencyTypeID Then
                        'TR - This Frequency has had at least one row created
                        'for it before so log the number of it if it is the lowest
                        If iLowestRowNoForThisFrequency <> -1 Then
                            If .RowIndex < iLowestRowNoForThisFrequency Then
                                iLowestRowNoForThisFrequency = .RowIndex
                            End If
                            If .RowIndex > iHighestRowNoForThisFrequency Then
                                iHighestRowNoForThisFrequency = .RowIndex
                            End If
                        Else
                            iLowestRowNoForThisFrequency = .RowIndex
                            iHighestRowNoForThisFrequency = iLowestRowNoForThisFrequency
                        End If
                        For Each objDataGridCell As clsDataGridCell In .Cells
                            If objDataGridCell.MediaTypeID = v_iMediaTypeID Then
                                If .RowIndex > iHighestRowNoPopulatedWithThisMediaType Then
                                    iHighestRowNoPopulatedWithThisMediaType = .RowIndex
                                    Exit For
                                End If
                            End If
                        Next objDataGridCell
                    End If
                End With
            Next objDataGridRow

            'TR - Has this Frequency / Media Type combination appeared already?
            If iHighestRowNoPopulatedWithThisMediaType > -1 Then
                r_iRowIndex = iHighestRowNoPopulatedWithThisMediaType + 1
                If iHighestRowNoPopulatedWithThisMediaType = iHighestRowNoForThisFrequency Then
                    r_bCreateNewRow = True
                End If
            Else
                r_iRowIndex = iLowestRowNoForThisFrequency
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFail
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error during Frequency / Media lookup", vApp:=ACApp, vClass:=ACClass, vMethod:="FindOrAddMediaFrequencyRow", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    '*************************************************************************
    'Name:          OpenMaintenanceForm
    'Description:   Passes properties and dislays the PF Maint screen
    '               If the user cancels this screen, then PF is deleted
    'History:       23/01/2003 - TR - Created
    ' RAM20030404   : Code changes related to Issue 2915
    '*************************************************************************
    Public Function OpenMaintenanceForm() As Integer

        Dim result As Integer = 0

        'To do List ALkesh
        'Dim oPFInterface As ClassInterface
        Dim oPFInterface As Object
        Dim lReturn As Integer
        Dim vUseExistingPaymentDetails As Object = Nothing ' RAM20030404  : Added this variable

        Try

            'TR - Assume success
            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Get an instance of the Maintenance Form
            Dim temp_oPFInterface As Object = Nothing
            lReturn = g_oObjectManager.GetInstance(temp_oPFInterface, "iPMBFinancePlanMaint.Interface_Renamed", gPMConstants.PMGetLocalInterface)
            oPFInterface = temp_oPFInterface
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TR - Pass in the Product Type

            lReturn = oPFInterface.SetProcessModes(vTransactionType:=m_sProduct_Code)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oPFInterface.Dispose()
                oPFInterface = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20030404   : Make sure you pass in the Selected Existing Payment
            '                   Details
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            lReturn = GetSelectedPaymentDetails(r_vSelectedPaymentDetails:=vUseExistingPaymentDetails)
            If lReturn = gPMConstants.PMEReturnCode.PMTrue And Information.IsArray(vUseExistingPaymentDetails) Then
                ' We have some values, so set it to the Finance Plan component

                lReturn = oPFInterface.SetSelectedPaymentDetails(v_vSelectedPaymentDetails:=vUseExistingPaymentDetails, v_bUseExistingBankAccount:=m_bUseExistingBankAccount, v_bUseExistingCreditCard:=m_bUseExistingCreditCard)
                ' Do we need to log error ????
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    oPFInterface.Dispose()
                    oPFInterface = Nothing
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error setting the selected payment details.", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenMaintenanceForm", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20030404   : END
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            'TR - Set the properties that the form needs to know
            With oPFInterface

                .FinancePlanCnt = m_lPremiumFinanceCnt

                .FinancePlanVersion = m_lPremiumFinanceVersion


                .FinancePlanMTATransArray = m_vPFTransArray

                .PartyCnt = m_lPartyCnt

                .Spawned = True

                .RunningContext = m_sRunningContext
                'TR - Display the form

                lReturn = .Start()
            End With
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TR - Make sure this screen was not cancelled. If it was, delete the plan
            'Pass in status of "010" (saved) just to ensure the delete

            If oPFInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then

                lReturn = m_oBusiness.DeletePlan(m_lPremiumFinanceCnt, m_lPremiumFinanceVersion, "010")
                ' Alix - 06/03/2003 - Issue 2805
                result = gPMConstants.PMEReturnCode.PMCancel
                ' /Alix
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Clean up

            oPFInterface.Dispose()
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFail
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error during Display Main Screen", vApp:=ACApp, vClass:=ACClass, vMethod:="OpenMaintenanceForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    Private Sub EnableOverrideFields()


        'TR - Set the private variables based on user selection
        m_bOverrideCommission = IIf((chkCommissionOverride.CheckState = 1), True, False)
        m_bOverrideInterestRate = IIf((chkOverrideInterestRate.CheckState = 1), True, False)
        m_bOverrideDeposit = IIf((chkDepositOverride.CheckState = 1), True, False)    'PN12144

        'TR - Validate the fields
        'TR - Enable / Diasable the New Interest Rate field
        If m_bOverrideInterestRate = False Then
            txtNewRate.Enabled = False
            txtNewRate.BackColor = SystemColors.Control
        Else
            txtNewRate.Enabled = True
            txtNewRate.BackColor = SystemColors.Window
        End If

        'TR - Enable / Diasable the Reference field
        If m_bOverrideCommission = True Or m_bOverrideInterestRate = True Then
            txtOverrideReference.Enabled = True
            txtOverrideReference.BackColor = SystemColors.Window
        Else
            txtOverrideReference.Enabled = False
            txtOverrideReference.BackColor = SystemColors.Control
        End If
        'PN12144
        If m_bOverrideDeposit = True AndAlso gPMFunctions.ToSafeCurrency(m_vQuoteArray(k_PFQuoteTotalAmountInput, m_lSelectedQuoteNo)) > 0 Then
            txtOverrideDeposit.Enabled = True
            txtOverrideDeposit.BackColor = SystemColors.Window
            _optDepositOverride_0.Enabled = True
            _optDepositOverride_1.Enabled = True
        Else
            txtOverrideDeposit.Enabled = False
            txtOverrideDeposit.BackColor = SystemColors.Control
            m_oFormFields.FormatControl(txtOverrideDeposit, "0.00")
            m_dOverrideDeposit = -1
            _optDepositOverride_0.Enabled = False
            _optDepositOverride_1.Enabled = False
            _optDepositOverride_1.Checked = True
        End If
        'PN12144End
        'End If
    End Sub

    Private Sub EnableOptionBoxes(ByVal v_bEnable As Boolean)

        Dim lBackColour As Integer

        'TR - Select correct BackColour
        If v_bEnable Then
            lBackColour = &H80000005
        Else
            lBackColour = &H8000000F
        End If

        'TR - Disable the option boxes
        For iCount As Integer = optMTAType.GetLowerBound(0) To optMTAType.GetUpperBound(0)
            optMTAType(iCount).Enabled = v_bEnable
            optMTAType(iCount).BackColor = ColorTranslator.FromOle(lBackColour)
        Next iCount
        'TR - Disable the Combo boxes
        cboMonthDay.Enabled = v_bEnable
        cboMonthDay.BackColor = ColorTranslator.FromOle(lBackColour)
        cboPreferredDate.Enabled = v_bEnable
        cboPreferredDate.BackColor = ColorTranslator.FromOle(lBackColour)
        cboWeekDay.Enabled = v_bEnable
        cboWeekDay.BackColor = ColorTranslator.FromOle(lBackColour)
    End Sub

    '==============
    'Control Events
    '==============

    Private tmp_isInitializingComponent As Boolean
    Private Sub SetFirstPaymentDate()
        Dim iMonthDay As Integer = ToSafeInteger(cboMonthDay.Text)

        For iCnt As Integer = 0 To cboPreferredDate.Items.Count - 1

            Dim iDayOfPreferedDate As Integer = Microsoft.VisualBasic.DateAndTime.Day(cboPreferredDate.Items.Item(iCnt).ToString)

            If iMonthDay = iDayOfPreferedDate OrElse (iMonthDay > System.DateTime.DaysInMonth(DateAndTime.Year(cboPreferredDate.Items.Item(0).ToString), DateAndTime.Month(cboPreferredDate.Items.Item(0).ToString)) AndAlso iDayOfPreferedDate = 1 AndAlso iCnt > 0) Then
                If CDate(cboPreferredDate.Items.Item(iCnt).ToString) >= m_dtEffectiveDate.Date Then
                    cboPreferredDate.SelectedIndex = iCnt
                    Exit For
                End If
            End If

        Next

    End Sub
    Private Sub cboMonthDay_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboMonthDay.TextChanged
        If cboMonthDay.SelectedIndex <> m_iDayOfMonth Then
            If Not m_bProcessingEvent Then
                m_iDayOfMonth = cboMonthDay.SelectedIndex
                m_bProcessingEvent = True
                RefreshQuoteData()
                m_bProcessingEvent = False
            End If
        End If
    End Sub
    Private Sub cboMonthDay_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboMonthDay.SelectedIndexChanged
        If cboMonthDay.SelectedIndex <> m_iDayOfMonth Then
            m_iDayOfMonth = cboMonthDay.SelectedIndex
            If Not m_bProcessingEvent Then
                m_bProcessingEvent = True
                 If Information.IsArray(m_vQuoteArray) AndAlso m_vQuoteArray(k_PFFirstInstalmentAlignWithMonthInDay, m_lSelectedQuoteNo) <> 0 Then
                    SetFirstPaymentDate()
                End If
                RefreshQuoteData()
                ListInstalmentSelectRow()

                m_bHasDatesChange = True
                RefreshQuoteData()
                ListInstalmentSelectRow()
                ListInstalment_Click()
                m_bHasDatesChange = False
                m_bProcessingEvent = False
            End If
        End If
    End Sub
    Private Sub cboMonthDay_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboMonthDay.Leave
        If cboMonthDay.SelectedIndex <> m_iDayOfMonth Then
            If Not m_bProcessingEvent Then
                m_iDayOfMonth = cboMonthDay.SelectedIndex
                m_bProcessingEvent = True
                RefreshQuoteData()
                m_bProcessingEvent = False
            End If
        End If
    End Sub

    Private Sub cboPreferredDate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPreferredDate.SelectedIndexChanged
        If Not m_bStopClickEvent Then
            If m_dtPreferredDate <> CDate(cboPreferredDate.Text) Then
                m_dtPreferredDate = CDate(cboPreferredDate.Text)
                Dim dtFirstPossibleDateFromToday As Date
                Dim dtLastPossibleDateFromToday As Date
                If Not m_bProcessingEvent Then

                    If CDate(m_oBusiness.StartDate) > Date.Now.Date Then
                        dtFirstPossibleDateFromToday = DateAndTime.DateAdd("w", m_vQuoteArray(k_PFQuoteDaysDelay, m_lSelectedQuoteNo), CDate(m_vQuoteArray(k_PFStartDate, m_lSelectedQuoteNo)))
                        dtLastPossibleDateFromToday = DateAndTime.DateAdd("w", m_vQuoteArray(k_PFDelayULimit, m_lSelectedQuoteNo), CDate(m_vQuoteArray(k_PFStartDate, m_lSelectedQuoteNo)))
                    Else
                        dtFirstPossibleDateFromToday = DateAndTime.DateAdd("w", m_vQuoteArray(k_PFQuoteDaysDelay, m_lSelectedQuoteNo), DateTime.Now.Date)
                        dtLastPossibleDateFromToday = DateAndTime.DateAdd("w", m_vQuoteArray(k_PFDelayULimit, m_lSelectedQuoteNo), DateTime.Now.Date)
                    End If

                    If (CDate(cboPreferredDate.Text) >= dtFirstPossibleDateFromToday AndAlso
                        CDate(cboPreferredDate.Text) <= dtLastPossibleDateFromToday) Then
                        m_bProcessingEvent = True
                        m_bHasDatesChange = True
                        RefreshQuoteData()
                        ListInstalmentSelectRow()

                        ListInstalment_Click()
                        m_bProcessingEvent = False
                        m_bHasDatesChange = False
                    Else
                        cboPreferredDate.Text = dtFirstPossibleDateFromToday.ToString("d")

                    End If
                Else
                    cboPreferredDate.Text = dtFirstPossibleDateFromToday.ToString("d")
                End If
            End If
        End If
    End Sub
    Private Sub cboPreferredDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPreferredDate.Leave
        If m_dtPreferredDate <> CDate(cboPreferredDate.Text) Then
            If Not m_bProcessingEvent Then
                m_dtPreferredDate = CDate(cboPreferredDate.Text)
                m_bProcessingEvent = True
                RefreshQuoteData()
                m_bProcessingEvent = False
            End If
        End If

    End Sub
    Private Sub cboWeekDay_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboWeekDay.SelectedIndexChanged
        If cboWeekDay.SelectedIndex <> m_iDayOfWeek Then
            m_iDayOfWeek = cboWeekDay.SelectedIndex
            If Not m_bProcessingEvent Then
                m_bProcessingEvent = True
                m_bHasDatesChange = True
                RefreshQuoteData()
                ListInstalmentSelectRow()
                ListInstalment_Click()

                m_bProcessingEvent = False
                m_bHasDatesChange = False
            End If
        End If
    End Sub
    Private Sub cboWeekDay_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboWeekDay.Leave
        If cboWeekDay.SelectedIndex <> m_iDayOfWeek Then
            If Not m_bProcessingEvent Then
                m_iDayOfWeek = cboWeekDay.SelectedIndex
                m_bProcessingEvent = True
                RefreshQuoteData()
                m_bProcessingEvent = False
            End If
        End If
    End Sub

    Public Function OverrideQuote() As Integer
        'this button should only show for PCLSG quotes remember..

        Dim result As Integer = 0
        Try

            Dim lQuoteIndex As Integer
            Dim lReturn As gPMConstants.PMEReturnCode
            Dim vValue As Object = Nothing
            Dim bFinished As Boolean
            Dim sProviderCode As String = ""

            If m_bSGQuote Then
                If m_GridArray(CLng(m_iSelectedRowIndex), 6) = "Net" Then
                    MessageBox.Show("Unable to override this scheme type, please choose a different instalment plan", "Override", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return result
                End If
            End If

            lQuoteIndex = m_lSelectedQuoteNo

            'create a new instance of the form
            Dim frmOR As frmOverride
            frmOR = New frmOverride()

            frmOR.AllowRateOverride = True


            lReturn = m_oBusiness.GetFinanceProviderDetails(v_lCompanyNo:=gPMFunctions.ToSafeLong(gPMFunctions.ToSafeString(m_vQuoteArray(bSIRPremFinConst.k_PFPlanCompanyNo, lQuoteIndex))), v_lSchemeNo:=gPMFunctions.ToSafeLong(gPMFunctions.ToSafeString(m_vQuoteArray(bSIRPremFinConst.k_PFPlanSchemeNo, lQuoteIndex))), v_lSchemeVersion:=gPMFunctions.ToSafeLong(gPMFunctions.ToSafeString(m_vQuoteArray(bSIRPremFinConst.k_PFPlanSchemeVersion, lQuoteIndex))), r_sShortName:=sProviderCode)

            With frmOR
                'pass in the existing values..
                If CBool(gPMFunctions.ToSafeString(gPMFunctions.ToSafeString(m_vQuoteArray(k_PFQuoteSchemeName, lQuoteIndex)) = "PCLSG").Trim()) Then
                    .OverrideRate = CDbl(m_vQuoteArray(k_PFQuoteInterestRate, lQuoteIndex))
                Else
                    If m_dOverrideDeposit > 0 Then .DepositOverride = m_dOverrideDeposit
                    If m_dOverrideRate > 0 Then .OverrideRate = m_dOverrideRate
                    If m_sOverrideReference <> "" Then .OverrideReference = m_sOverrideReference
                End If

                .AllowCommissionOverride = (gPMFunctions.ToSafeString(m_vQuoteArray(k_PFQuoteProductClass, lQuoteIndex)) <> "SG")
                .AllowDepositOverride = (gPMFunctions.ToSafeString(m_vQuoteArray(k_PFQuoteProductClass, lQuoteIndex)) <> "SG")
                .AllowPaymentProtection = (sProviderCode <> "PCLSG" And sProviderCode <> "CLOSEIP")
                .SuppressDecimalValues = m_bSuppressDecimalValues
            End With

            While Not (bFinished) And frmOR.Status <> gPMConstants.PMEReturnCode.PMCancel
                'show the form
                frmOR.ShowDialog()

                If (frmOR.Override And frmOR.OverrideRate <= 0) And frmOR.Status <> gPMConstants.PMEReturnCode.PMCancel Then
                    MessageBox.Show("Please enter a valid rate", "Rate Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    'Developer Guide No 115
                ElseIf frmOR.chkOverrideInterestRate.CheckState = CheckState.Checked AndAlso frmOR.OverrideRate > 14.5 AndAlso Information.IsNothing(m_vQuoteArray(k_PFQuoteXSLCode, lQuoteIndex)) AndAlso Convert.ToString(m_vQuoteArray(k_PFQuoteXSLCode, lQuoteIndex)).Trim() = "PCLPSG" Then
                    If frmOR.Status <> gPMConstants.PMEReturnCode.PMCancel Then
                        MessageBox.Show("Override rate must be 14.5% or lower for PCLP scheme", "PCLP", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Else
                    bFinished = True
                End If
            End While

            If frmOR.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Return result
            End If

            ShowProcessMessage(True)


            m_bPaymentProtection = frmOR.PaymentProtection
            'get the data back from the form

            'Update the Latest override values in Override tab too.
            If frmOR.chkOverrideInterestRate.CheckState = CheckState.Checked Then
                txtNewRate.Text = frmOR.OverrideRate
                m_dOverrideRate = frmOR.OverrideRate
            End If


            'Update the Latest override values in Override tab too.
            If frmOR.chkCommissionOverride.CheckState = CheckState.Checked Then
                txtOverrideReference.Text = Trim(frmOR.OverrideReference)
                m_sOverrideReference = frmOR.OverrideReference
            End If


            If frmOR.chkPaymentProtection.CheckState = CheckState.Checked Then
                chkPaymentProtection.Checked = True
            End If


            If frmOR.chkDepositOverride.CheckState = CheckState.Checked Then
                chkDepositOverride.Checked = True
                m_dOverrideDeposit = frmOR.DepositOverride
                txtOverrideDeposit.Text = frmOR.DepositOverride
                _optDepositOverride_1.Checked = True

                'Else
                '    m_dOverrideDeposit = 0
            End If




            If sProviderCode = "PCLSG" Then
                'now we need to offset the customers rate against the net rate
                m_dOverrideRate = (CDbl(m_dOverrideRate - CDbl(m_vQuoteArray(k_PFOriginalRate, lQuoteIndex))))
            End If

            'process the options

            'check the user entered a new value and didn't cancel..
            If frmOR.Override And frmOR.OverrideRate <> 0 And frmOR.OverrideRate <> gPMFunctions.ToSafeDouble(txtRate.Text, 0) Then
                If gPMFunctions.ToSafeString(m_vQuoteArray(k_PFQuoteProductClass, lQuoteIndex)).Trim() = "SG" Then
                    If gPMFunctions.ToSafeString(m_vQuoteArray(k_PFQuoteSchemeName, lQuoteIndex)) = "PCLPSG" Then
                        Refresh()
                    Else

                        lReturn = m_oBusiness.OverrideSGRate(lQuoteIndex, m_vQuoteArray, m_dOverrideRate)
                        If lReturn = gPMConstants.PMEReturnCode.PMError Then

                            If m_oBusiness.ErrorText.Trim() <> "" Then

                                MessageBox.Show("Failed to override rate. " & m_oBusiness.ErrorText, "Connectivity Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                                Return result
                            End If
                        ElseIf lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMError
                            Throw New Exception
                        End If


                        'developer guide no. 98
                        iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTInstalmentDisplayStyle, v_vBranch:=g_iSourceID, r_vUnderwriting:=vValue)
                        If gPMFunctions.NullToString(vValue) = "1" Then
                            tdgListInstalment.Visible = True
                            tdgInstalment.Visible = False
                            lReturn = BuildGridListStyle()
                        Else
                            tdgListInstalment.Visible = False
                            tdgInstalment.Visible = True
                            lReturn = BuildGridArrayFromResultArray()
                        End If
                    End If

                    'refresh the screen
                    m_lSelectedQuoteNo = lQuoteIndex
                    RefreshQuoteDataOnScreen()

                    If frmOR.OverrideReference.Trim() = "" Then
                        Return result
                    End If
                End If
            End If

            'commission override
            If Not m_bProcessingEvent Then
                m_bProcessingEvent = True
                RefreshQuoteData()
                m_bProcessingEvent = False
            End If

            'destroy the reference
            frmOR = Nothing





            Return result
        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to override rate", ACApp, ACClass, "cmdOverride_Click", Information.Err().Number, Information.Err().Description, excep:=ex)

            Return result

        Finally
            ShowProcessMessage(False)
        End Try
    End Function

    Private Sub chkCommissionOverride_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkCommissionOverride.CheckStateChanged
        EnableOverrideFields()
    End Sub

    Private Sub chkDepositOverride_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkDepositOverride.CheckStateChanged
        EnableOverrideFields()
    End Sub

    Private Sub chkOverrideInterestRate_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkOverrideInterestRate.CheckStateChanged
        EnableOverrideFields()
    End Sub

    Private Sub chkPaymentProtection_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPaymentProtection.CheckStateChanged
        EnableOverrideFields()
    End Sub
    Private Sub optMTAType_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _optMTAType_2.Click, _optMTAType_1.Click, _optMTAType_0.Click
        Dim hTabOptions As IntPtr = ssTabOptions.Handle
        If eventSender.Checked Then
            Dim Index As Integer = Array.IndexOf(optMTAType, eventSender)
            Dim bFullRefresh As Boolean = False
            If Index = bSIRPremFinConst.m_klInstalmentMTAType_AddToNewPlan AndAlso m_iPreviousDayOfMonth = -1 Then
                m_iPreviousDayOfMonth = m_iDayOfMonth
            End If

            If Index = bSIRPremFinConst.m_klInstalmentMTAType_AddAndSpread AndAlso m_iPreviousDayOfMonth <> -1 Then
                m_iSelectedRowIndex = 0
                m_iDayOfMonth = m_iPreviousDayOfMonth
                cboMonthDay.Text = m_iDayOfMonth + 1
            End If

            'TR - If the user selects "add mta to new plan" then show the
            'preferred days tab as we need to know this info
            If Index = bSIRPremFinConst.m_klInstalmentMTAType_AddToNewPlan Then
                SSTabHelper.SetTabVisible(ssTabOptions, 0, True)
                SSTabHelper.SetSelectedIndex(ssTabOptions, 0)
            Else
                SSTabHelper.SetTabVisible(ssTabOptions, 0, False)
            End If
            If Not m_bProcessingEvent Then
                m_bProcessingEvent = True

                ' RAW 13/11/2003 : CQ1765 : added tests for a full refresh
                If Index = bSIRPremFinConst.m_klInstalmentMTAType_AddToNewPlan And m_iPreviousMTAType <> bSIRPremFinConst.m_klInstalmentMTAType_AddToNewPlan Then

                    ' moving to a new plan
                    bFullRefresh = True

                ElseIf Index <> bSIRPremFinConst.m_klInstalmentMTAType_AddToNewPlan And m_iPreviousMTAType = bSIRPremFinConst.m_klInstalmentMTAType_AddToNewPlan Then

                    ' moving from a new plan
                    bFullRefresh = True
                End If

                If bFullRefresh Then
                    ' reset cell selections
                    m_iSelectedColumnIndex = -1
                    m_iSelectedRowIndex = -1
                    m_iExistingColumnIndex = -1
                    m_iExistingRowIndex = -1
                End If
                ' RAW 13/11/2003 : CQ1765 : end

                RefreshQuoteData()


                m_iPreviousMTAType = Index ' RAW 13/11/2003 : CQ1765 : added

                m_bProcessingEvent = False
            End If
        Else
            m_iUseTransactionCurrency = -1
        End If

        If (tdgListInstalment.Rows.Count > 0 AndAlso m_iSelectedRowIndex > -1) Then
            tdgListInstalment.Rows.Item(m_iSelectedRowIndex).Selected = True
            If IsArray(m_vQuoteArray) AndAlso m_vQuoteArray IsNot Nothing Then
                m_lSelectedQuoteNo = m_GridArray(CLng(m_iSelectedRowIndex), ACQuoteArrayElement)
                PopulateFirstInstalmentDate(gPMFunctions.ToSafeInteger(m_vQuoteArray(k_PFQuoteDaysDelay, m_lSelectedQuoteNo)), gPMFunctions.ToSafeInteger(m_vQuoteArray(k_PFStartLimit, m_lSelectedQuoteNo)))
                cboPreferredDate.SelectedIndex = 0
            End If
            m_bHasDatesChange = True
            ListInstalment_Click()
            m_bHasDatesChange = False
        End If
    End Sub
    Private Sub PopulateFirstInstalmentDate(ByVal nStartday As Integer, ByVal nEndDay As Integer)
        cboPreferredDate.Items.Clear()
        Dim dtPreferredDate As Date
        Dim bFirstInstalmentDateSelected As Boolean = False
        m_iFirstPaymentDate = 0
        For nCount As Integer = nStartday To nEndDay
            If ToSafeDate(CDate(m_vQuoteArray(k_PFStartDate, m_lSelectedQuoteNo))) >= ToSafeDate(Date.Now.Date()) Then
            dtPreferredDate = CDate(m_vQuoteArray(k_PFStartDate, m_lSelectedQuoteNo)).AddDays(nCount)
            Else
            dtPreferredDate = Date.Now.Date().AddDays(nCount)
            End If
            cboPreferredDate.Items.Add(dtPreferredDate.ToString("d"))
            If dtPreferredDate.Day.ToString = cboMonthDay.Text AndAlso m_vQuoteArray(k_PFFirstInstalmentAlignWithMonthInDay, m_lSelectedQuoteNo) = 1 AndAlso m_iFirstPaymentDate = 0 AndAlso Not bFirstInstalmentDateSelected _
            OrElse (cboMonthDay.Text > System.DateTime.DaysInMonth(DateAndTime.Year(CDate(m_vQuoteArray(k_PFStartDate, m_lSelectedQuoteNo)).ToString), DateAndTime.Month(CDate(m_vQuoteArray(k_PFStartDate, m_lSelectedQuoteNo)).ToString)) AndAlso dtPreferredDate.Day.ToString = "1") Then
            m_iFirstPaymentDate = nCount - nStartday
            bFirstInstalmentDateSelected = True
            End If
        Next nCount
    End Sub

    Private Sub ssTabMain_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ssTabMain.Click
        If ssTabMainPreviousTab = 2 Then
            If m_bProcessingEvent = False Then
                m_bProcessingEvent = True
                m_bCallFromTab2 = True
                Call ValidateUserInput()
                Refresh()
                m_bProcessingEvent = False
                m_bCallFromTab2 = False
            End If
        End If
    End Sub

    Private Sub ssTabMain_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ssTabMain.SelectedIndexChanged
        'TR - If user is trying to leave Override tab - make sure all
        'relevant fields are filled in
        If ssTabMainPreviousTab = 2 AndAlso m_bDataHasChanged Then
            If Not m_bProcessingEvent Then
                m_bProcessingEvent = True
                ValidateUserInput()
                Refresh()
                m_bDataHasChanged = False
                m_bProcessingEvent = False
            End If
        End If
        ssTabMainPreviousTab = ssTabMain.SelectedIndex
    End Sub

    Private Sub tdgInstalment_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tdgInstalment.DoubleClick
        SSTabHelper.SetSelectedIndex(ssTabMain, 1)
    End Sub




    Private Sub tdgListInstalment_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tdgListInstalment.DoubleClick

        SSTabHelper.SetSelectedIndex(ssTabMain, 1)
    End Sub

    Private Sub tdgListInstalment_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tdgListInstalment.Click
        m_bPlanSelected = True
        m_iSelectedRowIndex = tdgListInstalment.CurrentRowIndex
        Dim CellStyle As New DataGridViewCellStyle
        'PN72107
        m_bStopClickEvent = True
        If tdgListInstalment.Rows.Count > 0 Then
            With tdgListInstalment
                m_iSelectedColumnIndex = ACQuoteArrayElement

                m_lSelectedQuoteNo = m_GridArray(CLng(m_iSelectedRowIndex), CLng(m_iSelectedColumnIndex))
                m_vQuoteArray(k_PFQuoteHighlightCell, m_iSelectedRowIndex) = True
                If m_GridArray(CLng(m_iSelectedRowIndex), 5).Trim() = "SG" Then
                    m_bSGQuote = True
                Else
                    If InStr(1, m_GridArray(CLng(m_iSelectedRowIndex), 0), "SG", vbTextCompare) > 0 Then
                        m_bSGQuote = True
                    Else
                        m_bSGQuote = False
                    End If
                End If
                If eventArgs IsNot Nothing Then
                    PopulateFirstInstalmentDate(gPMFunctions.ToSafeInteger(m_vQuoteArray(k_PFQuoteDaysDelay, m_lSelectedQuoteNo)), gPMFunctions.ToSafeInteger(m_vQuoteArray(k_PFStartLimit, m_lSelectedQuoteNo)))
                    If m_vQuoteArray(k_PFFirstInstalmentAlignWithMonthInDay, m_lSelectedQuoteNo) = 1 Then
                        cboPreferredDate.SelectedIndex = m_iFirstPaymentDate
                    Else
                        cboPreferredDate.SelectedIndex = 0
                    End If

                    m_bHasDatesChange = True
                End If
                RefreshQuoteDataOnScreen()
                ShowHideTopTabs()
            End With
        End If
        'PN72107
        m_bStopClickEvent = False
    End Sub
    Private Sub ListInstalment_Click()
        m_bPlanSelected = True
        If Not m_bHasDatesChange Then
            m_iSelectedRowIndex = tdgListInstalment.CurrentRowIndex
        Else
            ListInstalmentSelectRow()
        End If

        Dim CellStyle As New DataGridViewCellStyle
        'PN72107
        m_bStopClickEvent = True
        With tdgListInstalment
            m_iSelectedColumnIndex = ACQuoteArrayElement
            If Not m_GridArray Is Nothing Then
                m_lSelectedQuoteNo = m_GridArray(CLng(m_iSelectedRowIndex), CLng(m_iSelectedColumnIndex))
                If Not m_vQuoteArray Is Nothing Then
                    m_vQuoteArray(k_PFQuoteHighlightCell, m_iSelectedRowIndex) = True
                End If

                If m_GridArray(CLng(m_iSelectedRowIndex), 5).Trim() = "SG" Then
                    m_bSGQuote = True
                Else
                    If InStr(1, m_GridArray(CLng(m_iSelectedRowIndex), 0), "SG", vbTextCompare) > 0 Then
                        m_bSGQuote = True
                    Else
                        m_bSGQuote = False
                    End If
                End If
            End If
            ShowHideTopTabs()
            If Not m_vQuoteArray Is Nothing Then
                RefreshQuoteDataOnScreen()
            End If
        End With
        'PN72107
        m_bStopClickEvent = False
    End Sub
    Private Sub tdgListInstalment_ColumnHeaderMouseClick(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellMouseEventArgs) Handles tdgListInstalment.ColumnHeaderMouseClick
        Dim ColIndex As Integer = eventArgs.ColumnIndex
        Dim iCnt As Integer
        ' PN 74070
        Dim vPreviousSchemeDetails As Object = Nothing
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lpfschemeno, lpfschemeversion As Integer
        Dim sFrequencyDesc As String = String.Empty

        If ColIndex <> m_SortedColumnIndex AndAlso m_SortedColumnIndex <> -1 Then
            Select Case ColIndex
                Case 0, 1, 2
                    m_GridArray.QuickSort(m_GridArray.GetLowerBound(0), m_GridArray.GetUpperBound(0), ColIndex, XArrayHelper.XArraySortOrder.ASCENDING, XArrayHelper.XArrayColumnTypes.StringCaseSensitive)
                Case 3, 4
                    m_GridArray.QuickSort(m_GridArray.GetLowerBound(0), m_GridArray.GetUpperBound(0), ColIndex, XArrayHelper.XArraySortOrder.ASCENDING, XArrayHelper.XArrayColumnTypes.Currency)
            End Select
            m_SortedDirection = 1
        ElseIf ColIndex = m_SortedColumnIndex Then
            If m_SortedDirection = 1 Then
                Select Case ColIndex
                    Case 0, 1, 2
                        m_GridArray.QuickSort(m_GridArray.GetLowerBound(0), m_GridArray.GetUpperBound(0), ColIndex, XArrayHelper.XArraySortOrder.DESCENDING, XArrayHelper.XArrayColumnTypes.StringCaseSensitive)
                    Case 3, 4
                        m_GridArray.QuickSort(m_GridArray.GetLowerBound(0), m_GridArray.GetUpperBound(0), ColIndex, XArrayHelper.XArraySortOrder.DESCENDING, XArrayHelper.XArrayColumnTypes.Currency)
                End Select
                m_SortedDirection = 2
            ElseIf m_SortedDirection = 2 Then
                Select Case ColIndex
                    Case 0, 1, 2
                        m_GridArray.QuickSort(m_GridArray.GetLowerBound(0), m_GridArray.GetUpperBound(0), ColIndex, XArrayHelper.XArraySortOrder.ASCENDING, XArrayHelper.XArrayColumnTypes.StringCaseSensitive)
                    Case 3, 4
                        m_GridArray.QuickSort(m_GridArray.GetLowerBound(0), m_GridArray.GetUpperBound(0), ColIndex, XArrayHelper.XArraySortOrder.ASCENDING, XArrayHelper.XArrayColumnTypes.Currency)
                End Select
                m_SortedDirection = 1
            End If
        End If

        m_SortedColumnIndex = ColIndex
        tdgListInstalment.Refresh()
        If Not IsNothing(tdgListInstalment.CurrentRow) Then
            If tdgListInstalment.CurrentRow.Cells(m_GridArray.GetUpperBound(1) - 1).Visible = True Then
                tdgListInstalment.CurrentCell = tdgListInstalment.CurrentRow.Cells(m_GridArray.GetUpperBound(1) - 1)
            End If
        End If
        'tdgListInstalment.Row = m_GridArray.LowerBound(1)

        ' PN 74070
        ' Under Renewal just set it once to allow manual change if done by user
        If m_sProduct_Code = "REN" And m_bPlanSelected And Not m_bSchemeResembleToPrevious Then
            ' If not already selected then do this thing
            ' set focus on matched schemeno and schemeversion
            lReturn = m_oBusiness.GetPreviousPlanSelectedFromInsuranceFile(v_lInsuranceFileCnt:=m_lInsuranceFileCnt_Renewal, r_vPreviousSchemeDetails:=vPreviousSchemeDetails)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("tdgListInstalment_HeadClick", "Failed to get Previous Scheme Details")
            End If
            If Information.IsArray(vPreviousSchemeDetails) Then
                ' Array contains
                ' pfprem_finance_cnt, pfprem_finance_version, companyno, schemeno, schemeversion, schemename, productclass, transtype
                lpfschemeno = gPMFunctions.ToSafeLong(CStr(vPreviousSchemeDetails(3, 0)))
                lpfschemeversion = gPMFunctions.ToSafeLong(CStr(vPreviousSchemeDetails(4, 0)))
                sFrequencyDesc = gPMFunctions.ToSafeString(CStr(vPreviousSchemeDetails.GetValue(8, 0)))
                m_sFrequencyDesc = sFrequencyDesc
            End If
            For iCnt = m_GridArray.GetLowerBound(0) To m_GridArray.GetUpperBound(0)
                'If gPMFunctions.ToSafeLong(m_GridArray(iCnt, 7).Trim()) = lpfschemeno And gPMFunctions.ToSafeLong(m_GridArray(iCnt, 8).Trim()) = lpfschemeversion Then
                If gPMFunctions.ToSafeLong(Convert.ToString(m_GridArray(iCnt, 7)).Trim()) = lpfschemeno AndAlso
                    gPMFunctions.ToSafeLong(Convert.ToString(m_GridArray(iCnt, 8)).Trim()) = lpfschemeversion AndAlso
                    gPMFunctions.ToSafeString(m_GridArray(iCnt, 1)).Trim().ToUpper() = sFrequencyDesc.Trim().ToUpper() Then
                    m_lSelectedQuoteNo = gPMFunctions.ToSafeLong(Convert.ToString(m_GridArray(iCnt, ACQuoteArrayElement)).Trim())
                    m_bSchemeResembleToPrevious = True
                    Exit For
                End If
            Next
        End If

        For iCnt = m_GridArray.GetLowerBound(0) To m_GridArray.GetUpperBound(0)
            If m_GridArray(iCnt, ACQuoteArrayElement) = m_lSelectedQuoteNo Then
                m_iSelectedRowIndex = iCnt
                Exit For
            End If
        Next
        If iCnt > m_GridArray.GetUpperBound(0) Then
            iCnt = m_GridArray.GetUpperBound(0)
        End If

        'If Not IsNothing(tdgListInstalment.CurrentRow) Then
        '    iCnt = 0
        'End If

        If m_iTask = gPMConstants.PMEComponentAction.PMView Then

            For iCount As Integer = 0 To tdgListInstalment.RowCount - 1
                If tdgListInstalment.Rows(iCount).Cells(0).FormattedValue() = m_sSchemeName AndAlso tdgListInstalment.Rows(iCount).Cells(5).FormattedValue() = m_sTransType Then

                    tdgListInstalment.Rows.Item(iCount).Selected = True
                    tdgListInstalment.CurrentCell = tdgListInstalment.SelectedRows(0).Cells.Item(0)
                    tdgListInstalment.FirstDisplayedScrollingRowIndex = iCount
                    m_iSelectedRowIndex = iCount
                    Exit For
                End If
            Next
            tdgListInstalment.Enabled = False
        End If

        m_iSelectedColumnIndex = tdgListInstalment.CurrentCell.ColumnIndex
        'm_iSelectedRowIndex = iCnt
        If m_iSelectedRowIndex = -1 Then
            m_iSelectedRowIndex = 0
        End If
        'm_lSelectedQuoteNo = m_GridArray(<untranslated-structure-in-to-res: row>, <untranslated-structure-in-to-res: column>)
        m_lSelectedQuoteNo = m_GridArray(CLng(m_iSelectedRowIndex), ACQuoteArrayElement)
        RefreshQuoteDataOnScreen()

        If iCnt <= m_GridArray.GetUpperBound(0) Then
            ListInstalmentSelectRow()
        End If

    End Sub

    Private Sub tdgListInstalment_CellEnter(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellEventArgs)
        Dim LastRow As DataGridViewRow = Nothing
        Dim LastCol As Integer = -1
        If Not IsNothing(tdgListInstalment.PreviousCell) Then
            If tdgListInstalment.PreviousCell.RowIndex > tdgListInstalment.Rows.Count Then
                LastRow = tdgListInstalment.Rows(tdgListInstalment.PreviousCell.RowIndex)
            End If
            LastCol = tdgListInstalment.PreviousCell.ColumnIndex
        End If

        Dim CellStyle As New DataGridViewCellStyle
        'PN72107
        m_bStopClickEvent = True
        With tdgListInstalment
            m_iSelectedColumnIndex = ACQuoteArrayElement


            'TODO: no solution found
            m_iSelectedRowIndex = gPMFunctions.ToSafeInteger(.CurrentRowIndex)
            m_lSelectedQuoteNo = m_GridArray(CLng(m_iSelectedRowIndex), CLng(m_iSelectedColumnIndex))

            If m_GridArray(CLng(m_iSelectedRowIndex), 5).Trim() = "SG" Then
                m_bSGQuote = True
            Else
                If InStr(1, m_GridArray(CLng(m_iSelectedRowIndex), 0), "SG", vbTextCompare) > 0 Then
                    m_bSGQuote = True
                Else
                    m_bSGQuote = False
                End If
            End If
            ShowHideTopTabs()

            RefreshQuoteDataOnScreen()
        End With
        'PN72107
        m_bStopClickEvent = False
    End Sub

    Private Sub tdgInstalment_CellEnter(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellEventArgs) Handles tdgInstalment.CellEnter
        Dim LastRow As Object = Nothing
        Dim LastCol As Integer = -1
        If Not IsNothing(tdgInstalment.PreviousCell) Then
            If tdgInstalment.PreviousCell.RowIndex > tdgInstalment.Rows.Count Then
                LastRow = tdgInstalment.Rows(tdgInstalment.PreviousCell.RowIndex)
            End If
            LastCol = tdgInstalment.PreviousCell.ColumnIndex
        End If

        Try
            'TR - Make sure we aren't looping back into this event
            If Not m_bInitialisingGrid And Not m_bChangingSelectedCell Then
                'Stop further changes
                m_bChangingSelectedCell = True
                With tdgInstalment
                    'TR - Don't do anything if the user clicked on the Payment Period column
                    If .CurrentCell.ColumnIndex <> 0 Then
                        ' note - use the cell that is currently in focus
                        ChangeSelectedQuote(.CurrentCell.ColumnIndex, .CurrentRow.Index) ' RAW 05/11/2003 : CQ1824 : replaced .RowBookmark(.Row) with .Bookmark
                    Else
                        If LastCol = 0 Then
                            If Not IsNothing(.CurrentRow) Then
                                .CurrentCell = .CurrentRow.Cells(1)
                            End If
                        Else

                            If (Not (Convert.IsDBNull(LastCol) Or IsNothing(LastCol))) And (LastCol <> -1) Then
                                If Not IsNothing(.CurrentRow) Then
                                    .CurrentCell = .CurrentRow.Cells(LastCol)
                                End If
                            Else
                                If Not IsNothing(.CurrentRow) Then
                                    .CurrentCell = .CurrentRow.Cells(1)
                                End If
                            End If
                        End If


                        If (Not (Convert.IsDBNull(LastRow) Or IsNothing(LastRow))) And (gPMFunctions.ToSafeString(LastRow) <> "") Then

                            .CurrentRowIndex = gPMFunctions.ToSafeInteger(LastRow)
                        End If
                    End If
                    m_bChangingSelectedCell = False
                End With
            End If

        Catch exc As System.Exception
        End Try
    End Sub


    Private Sub tdgInstalment_CellValueNeededEx(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellValueEventArgs) Handles tdgInstalment.CellValueNeeded
        Dim v_objRowBuf As DataGridViewRow = tdgInstalment.Rows(eventArgs.RowIndex)
        Dim r_vStartLocation As Object = v_objRowBuf
        Dim v_lOffset As Integer = 0
        Dim r_lApproximatePosition As Integer = -1
        Dim iActualColumnIndex As Integer
        Dim vBookmark As Object

        Dim iNoOfRowsFetched As Integer = 0

        'TR - Store the Offset
        m_lGridOffset = v_lOffset

        For lRowIndex As Integer = 0 To 1 - 1
            ' Get the vBookmark of the next available row

            vBookmark = GetRelativeBookmark(gPMFunctions.ToSafeString(r_vStartLocation), v_lOffset + lRowIndex)

            ' If the next row is BOF or EOF, then stop fetching
            ' and return any rows fetched up to this point.

            If Convert.IsDBNull(vBookmark) Or IsNothing(vBookmark) Then Exit For

            ' Place the record data into the row buffer
            For iColumnIndex As Integer = 0 To v_objRowBuf.Cells.Count - 1
                iActualColumnIndex = v_objRowBuf.Cells(iColumnIndex).ColumnIndex

                v_objRowBuf.Cells(iColumnIndex).Value = GetUserData(vBookmark, iActualColumnIndex)
            Next iColumnIndex

            ' Set the vBookmark for the row


            v_objRowBuf = vBookmark

            ' Increment the count of fetched rows
            iNoOfRowsFetched += 1
        Next lRowIndex

        ' Tell the grid how many rows were fetched

        'Developer Guide No. no solution found
        'v_objRowBuf.RowCount = iNoOfRowsFetched

        'TR - Work out the start row for the next chunk of data

        Dim lNextRowToStartOn As Integer = IndexFromBookmark(gPMFunctions.ToSafeString(r_vStartLocation), v_lOffset)
        If lNextRowToStartOn >= 0 Then
            'TR - Change the size of the grid
            r_lApproximatePosition = lNextRowToStartOn
        End If
    End Sub
    Private Sub txtNewRate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtNewRate.TextChanged
        m_dOverrideRate = gPMFunctions.ToSafeDouble(txtNewRate.Text)
        If txtNewRate.Text.Trim.Length > 0 Then
            m_bDataHasChanged = True
        End If
    End Sub
    Private Sub txtOverrideDeposit_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtOverrideDeposit.Leave
        If m_oFormFields IsNot Nothing Then

            If txtOverrideDeposit.Text.Trim = "" Then
                txtOverrideDeposit.Text = "0.00"
            End If
            Dim dDepositPC As Double = m_oFormFields.UnformatControl(txtOverrideDeposit)

            If _optDepositOverride_0.Checked = True Then

                m_dOverrideDeposit = gPMFunctions.ToSafeRound(((m_vQuoteArray(k_PFQuoteTotalAmountInput, m_lSelectedQuoteNo)) * (m_oFormFields.UnformatControl(txtOverrideDeposit)) / 100), 2, m_bSuppressDecimalValues)
            Else
                m_dOverrideDeposit = gPMFunctions.ToSafeRound(m_oFormFields.UnformatControl(txtOverrideDeposit), 2, m_bSuppressDecimalValues)
            End If
            If txtOverrideDeposit.Text.Trim.Length > 0 Then
                m_bDataHasChanged = True
            End If

        End If
    End Sub

    Private Sub txtOverrideReference_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtOverrideReference.TextChanged
        m_sOverrideReference = gPMFunctions.ToSafeString(txtOverrideReference.Text)
        If txtOverrideReference.Text.Trim.Length > 0 Then
            m_bDataHasChanged = True
        End If
    End Sub

    Private Sub UserControl_Initialize()
        m_crFirstOriginalInstalment = 0
        ' RAW 05/11/2003 : CQ1824 : added
        m_iSelectedColumnIndex = -1
        m_iSelectedRowIndex = -1
        m_iExistingColumnIndex = -1
        m_iExistingRowIndex = -1
        ' RAW 05/11/2003 : CQ1824 : end
        'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.9.1.1)
        Dim lReturn As gPMConstants.PMEReturnCode = CType(gPMFunctions.GetUserIsAmericanLanguageID(m_iLanguageID), gPMConstants.PMEReturnCode)
        'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.9.1.1)
        If Not DesignMode Then
            ' run time mode
            m_bProcessingEvent = True
            m_bInitialisingGrid = True
            Initialise()
            m_bProcessingEvent = False

        Else
            ' design time mode
        End If
    End Sub
    Private Sub UserControl_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        'TR - Don't allow any events thorugh while we're initialising the user control
        ' RAW 05/11/2003 : CQ2912, 2976 : added test for run-time mode
        'If Not DesignMode Then
        '    ' run time mode
        '    m_bProcessingEvent = True
        '    m_bInitialisingGrid = True
        '    Initialise()
        '    m_bProcessingEvent = False

        'Else
        '    ' design time mode
        'End If
        'Get the Decimal Suppression flag
        Dim sTempOptionValue As String = ""
        iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTEnableDecimalsSuppression, v_vBranch:=1, r_vUnderwriting:=sTempOptionValue)
        If Trim(sTempOptionValue) <> "" AndAlso Trim(sTempOptionValue) = "1" Then
            m_bSuppressDecimalValues = True
        End If

    End Sub

    Private Sub UserControl_Terminate()
        m_bProcessingEvent = True
        m_oBusiness = Nothing
    End Sub

    '*************************************************************************
    'Name:          DeletePlanForOneInsFile
    'Description:   Delete all finance plans for the given policy
    'History:       24/03/2003 - Created by Alix Bergeret
    '*************************************************************************
    Public Function DeletePlanForOneInsFile(ByVal v_lInsFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Alix - Check an insurance file cnt was passed in
            If v_lInsFileCnt = 0 Then
                result = gPMConstants.PMEReturnCode.PMFail
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Fail to delete finance plan for this policy. Policy ID was not passed in.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePlanForOneInsFile", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Do the deletion

            lReturn = m_oBusiness.DeletePlanForOneInsFile(v_lInsFileCnt:=v_lInsFileCnt)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFail
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Fail to delete finance plan for this policy.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePlanForOneInsFile", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFail
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error during DeletePlanForOneInsFile", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePlanForOneInsFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '***************************************************************************
    ' Name          : PopulateExistingPlanDetails
    ' Description   : Private function to populate the existing Instalment Plan
    '                   details for the client (if he/she had one)
    '                   (i.e. Bank Account Details, Credit Card Details)
    ' Author        : Ram Chandrabose
    ' Created on    : 2003/04/03
    ' Notes         : Introduced as a part of Issue 2915 Changes
    ' Edit History  :
    ' RAM20030403   : Created
    '***************************************************************************
    Private Function PopulateExistingPlanDetails(ByVal v_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Const k_PFPlanClientBankSortCode As Integer = 1
        Const k_PFPlanClientBankAccountNo As Integer = 2

        Dim lReturnValue As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the combo boxes
            cboExistingBankAccDtls.Items.Clear()
            cboExistingCreditCardDtls.Items.Clear()

            ' Populate default values (empty values)
            cboExistingBankAccDtls.Items.Insert(0, "Use new account")
            cboExistingCreditCardDtls.Items.Insert(0, "Use new card")

            ' Check if we have a valid Party Cnt
            If v_lPartyCnt > 0 Then

                ' Check if the client have any existing Bank Account details.

                lReturnValue = m_oBusiness.BankAccountDetailsList(v_lPartyCnt, m_vBankAccountDetails)
                If lReturnValue = gPMConstants.PMEReturnCode.PMTrue And Information.IsArray(m_vBankAccountDetails) Then
                    With cboExistingBankAccDtls
                        For iCounter As Integer = m_vBankAccountDetails.GetLowerBound(1) To m_vBankAccountDetails.GetUpperBound(1)
                            .Items.Add(gPMFunctions.ToSafeString(m_vBankAccountDetails(k_PFPlanClientBankSortCode, iCounter)) & " - " & gPMFunctions.ToSafeString(m_vBankAccountDetails(k_PFPlanClientBankAccountNo, iCounter)))
                        Next iCounter
                    End With
                    '        Else
                    '            PopulateExistingPlanDetails = lReturnValue
                    '            LogMessage iType:=PMLogOnError, _
                    ''                        sMsg:="Populate Existing Bank Account Details Failed", _
                    ''                        vApp:=ACApp, _
                    ''                        vClass:=ACClass, _
                    ''                        vMethod:="PopulateExistingPlanDetails", _
                    ''                        vErrNo:=Err.Number, _
                    ''                        vErrDesc:=Err.Description
                End If


                ' Check if the client have any existing Credit Card details.

                lReturnValue = m_oBusiness.CreditCardDetailsList(v_lPartyCnt, m_vCreditCardDetails)
                If lReturnValue = gPMConstants.PMEReturnCode.PMTrue Then
                    With cboExistingCreditCardDtls
                        For iCounter As Integer = m_vCreditCardDetails.GetLowerBound(1) To m_vCreditCardDetails.GetUpperBound(1)
                            ' Note : We can't display all the digits of the credit card !!!
                            '          so displaying only the last four digits
                            If gPMFunctions.ToSafeString(m_vCreditCardDetails(1, iCounter)).Length - 4 > 0 Then
                                .Items.Add("**** **** **** " & gPMFunctions.ToSafeString(m_vCreditCardDetails(1, iCounter)).Substring(gPMFunctions.ToSafeString(m_vCreditCardDetails(1, iCounter)).Length - 4))
                            End If
                        Next iCounter
                    End With
                    '        Else
                    '            PopulateExistingPlanDetails = lReturnValue
                    '            LogMessage iType:=PMLogOnError, _
                    ''                        sMsg:="Populate Existing Credit Card Details Failed", _
                    ''                        vApp:=ACApp, _
                    ''                        vClass:=ACClass, _
                    ''                        vMethod:="PopulateExistingPlanDetails", _
                    ''                        vErrNo:=Err.Number, _
                    ''                        vErrDesc:=Err.Description
                End If

            End If

            ' Note  : Setting the ListIndexs of the comboBoxes to -1, won't trigger the Click Event
            '          if they are Set to zero, the ListIndex Property is changes and will trigger the
            '           ComboBox Click Event to occur. So we don't wan't want to trigger that event at
            '           the moment. Only the user can change it by clicking it using the mouse
            cboExistingBankAccDtls.SelectedIndex = -1
            cboExistingCreditCardDtls.SelectedIndex = -1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateExistingPlanDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateExistingPlanDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    Private Sub cboExistingBankAccDtls_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboExistingBankAccDtls.SelectedIndexChanged

        If cboExistingBankAccDtls.SelectedIndex > 0 Then
            m_bUseExistingBankAccount = True
            cboExistingCreditCardDtls.SelectedIndex = 0
            m_bUseExistingCreditCard = False
        Else
            m_bUseExistingBankAccount = False
        End If

        ' Make sure the Credit Card Details list index is set to Zero, if the current
        ' index of the bank account details is > 0
        ' Note : This may cause recursive events. so make sure any future changes to
        '         this method should be treated carefully

        ' Note : This -1 is for the default value
        If cboExistingBankAccDtls.SelectedIndex - 1 <> m_iSelectedBankAccountDetails Then
            If Not m_bProcessingEvent Then
                m_iSelectedBankAccountDetails = cboExistingBankAccDtls.SelectedIndex - 1
                m_bProcessingEvent = True
                RefreshQuoteData()
                m_bProcessingEvent = False
            End If
        End If

    End Sub


    Private Sub cboExistingCreditCardDtls_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboExistingCreditCardDtls.SelectedIndexChanged

        If cboExistingCreditCardDtls.SelectedIndex > 0 Then
            m_bUseExistingCreditCard = True
            cboExistingBankAccDtls.SelectedIndex = 0
            m_bUseExistingBankAccount = False
        Else
            m_bUseExistingCreditCard = False
        End If

        ' Make sure the Bank Account Details list index is set to Zero, if the current
        ' index of the credit card details is > 0
        ' Note : This may cause recursive events. so make sure any future changes to
        '         this method should be treated carefully

        ' Note : This -1 is for the default value, "Donot use existing"
        If cboExistingCreditCardDtls.SelectedIndex - 1 <> m_iSelectedCreditCardDetails Then
            ' Only if we need to calculate the quote based on the credit card details
            If m_bUseExistingCreditCard Then
                If Not m_bProcessingEvent Then
                    ' To know which credit card information
                    m_iSelectedCreditCardDetails = cboExistingCreditCardDtls.SelectedIndex - 1
                    m_bProcessingEvent = True
                    RefreshQuoteData()
                    m_bProcessingEvent = False
                End If
            End If
        End If

    End Sub


    '***************************************************************************
    ' Name          : GetSelectedPaymentDetails
    ' Description   : Private function to get all the details of the selected
    '                   payment options (i.e. Bank Account Details, Credit Card Details)
    ' Author        : Ram Chandrabose
    ' Created on    : 2003/04/03
    ' Notes         : Introduced as a part of Issue 2915 Changes
    ' Edit History  :
    ' RAM20030403   : Created
    '***************************************************************************
    Private Function GetSelectedPaymentDetails(ByRef r_vSelectedPaymentDetails(,) As Object) As Integer

        Dim result As Integer = 0
        Dim iNoofFields As Integer
        Dim vReturnArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bUseExistingBankAccount And m_iSelectedBankAccountDetails >= 0 Then

                iNoofFields = m_vBankAccountDetails.GetUpperBound(0)
                ReDim vReturnArray(iNoofFields, 0)

                For iCounter As Integer = 0 To iNoofFields

                    vReturnArray(iCounter, 0) = m_vBankAccountDetails(iCounter, m_iSelectedBankAccountDetails)
                Next iCounter

            ElseIf m_bUseExistingCreditCard And m_iSelectedCreditCardDetails >= 0 Then

                iNoofFields = m_vCreditCardDetails.GetUpperBound(0)
                ReDim vReturnArray(iNoofFields, 0)

                For iCounter As Integer = 0 To iNoofFields

                    vReturnArray(iCounter, 0) = m_vCreditCardDetails(m_iSelectedCreditCardDetails, 0)
                Next iCounter

            End If


            r_vSelectedPaymentDetails = vReturnArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSelectedPaymentDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSelectedPaymentDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetPrefferedDates
    '
    ' Description: Gets the customer's preferred day of week / month based _
    ''                                       on the previous plan.
    '
    ' Written By: Simon.Baynes
    '
    ' Returns:
    '
    ' Date: 17/10/2003
    ' RAW 05/11/2003 : CQ2912, 2976 : added 1st Instalment Date param
    ' ***************************************************************** '
    Private Function GetPreferredDates(ByVal v_lPremFinanceVersion As Integer, ByVal v_lPremFinanceCnt As Integer, ByRef r_sFrequency As String, ByRef r_lDayOfWeekOrMonth As Integer, ByRef r_dtFirstInstalmentDate As Date, Optional ByVal v_sProductCode As String = "") As Integer


        Dim result As Integer = 0
        Dim lReturnValue As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RAW 05/11/2003 : CQ2912, 2976 : added 1st Instalment Date param

            lReturnValue = m_oBusiness.GetPreferredDates(v_lPremFinanceVersion:=v_lPremFinanceVersion, v_lPremFinanceCnt:=v_lPremFinanceCnt, r_sFrequency:=r_sFrequency, r_lDayOfWeekOrMonth:=r_lDayOfWeekOrMonth, r_dtFirstInstalmentDate:=r_dtFirstInstalmentDate, v_sProductCode:=v_sProductCode)



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPreferredDates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPreferredDates", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function Initialise_Control() As Integer
        Initialise()
    End Function

    'displays the "processing" messagebox for when we are busy..
    Private Sub ShowProcessMessage(ByRef bShow As Boolean)
        'size the window to the center of the screen
        picProcessing.Height = shpProcessing.Height
        picProcessing.Width = shpProcessing.Width

        picProcessing.Left = (Width / 2) - (picProcessing.Width / 2)
        picProcessing.Top = (Height / 2) - (picProcessing.Height / 2)

        'set the visible status of the box as requested
        picProcessing.Visible = bShow

    End Sub
    'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.9.1.3)
    Public Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Dim lCaptionID As Integer
        Dim iSkip As Integer
        'Used in the .Tag property of ListView.CoulmnHeading
        Const ksHidden As String = "HIDDEN"
        Const kMethodName As String = "DisplayCaptions"
        Dim IChildCapId As Integer = 0
        Dim IChildChieldCapId As Integer = 0
        Dim iGridColumnheaderCount As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iControlCount As Integer = 0 To Me.Count - 1

                'developer guide no 270. 
                lCaptionID = iPMForms.GetCaptionID(Convert.ToString(Me.Controls_Renamed(iControlCount).Tag))
                If lCaptionID > 0 Then

                    Select Case Me.Controls_Renamed(iControlCount).GetType().Name
                        Case "TabControl"

                            'developer guide no 55.
                            For iColumnheaderCount As Integer = 0 To Me.Controls_Renamed(iControlCount).TabCount - 1


                                'developer guide no.243,55
                                IChildCapId = iPMForms.GetCaptionID(Convert.ToString(Me.Controls_Renamed(iControlCount).TabPages(iColumnheaderCount).Tag))
                                Dim m_iResId As Integer = iColumnheaderCount
                                Select Case (Me.Controls_Renamed(iControlCount).TabPages(iColumnheaderCount).Name)
                                    Case _ssTabMain_TabPage0.Name
                                        m_iResId = 0
                                        Exit Select
                                    Case _ssTabMain_TabPage1.Name
                                        m_iResId = 1
                                        Exit Select
                                    Case _ssTabMain_TabPage2.Name
                                        m_iResId = 2
                                        Exit Select
                                    Case _ssTabMain_TabPage3.Name
                                        m_iResId = 3
                                        Exit Select
                                    Case _ssTabOptions_TabPage0.Name
                                        m_iResId = 0
                                        Exit Select
                                    Case _ssTabOptions_TabPage1.Name
                                        m_iResId = 1
                                        Exit Select
                                    Case _ssTabOptions_TabPage2.Name
                                        m_iResId = 2
                                        Exit Select
                                End Select
                                Me.Controls_Renamed(iControlCount).TabPages(iColumnheaderCount).Text = iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=lCaptionID + m_iResId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)

                                For iChildControl As Integer = 0 To Me.Controls_Renamed(iControlCount).TabPages(iColumnheaderCount).Controls.Count - 1
                                    IChildCapId = iPMForms.GetCaptionID(Convert.ToString(Me.Controls_Renamed(iControlCount).TabPages(iColumnheaderCount).Controls(iChildControl).Tag))
                                    If IChildCapId > 0 Then
                                        Select Case Me.Controls_Renamed(iControlCount).TabPages(iColumnheaderCount).Controls(iChildControl).GetType().Name
                                            Case "Label"
                                                Me.Controls_Renamed(iControlCount).TabPages(iColumnheaderCount).Controls(iChildControl).Text = iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=IChildCapId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
                                            Case "GroupBox"
                                                Me.Controls_Renamed(iControlCount).TabPages(iColumnheaderCount).Controls(iChildControl).Text = iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=IChildCapId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
                                                For iChildChildControl As Integer = 0 To Me.Controls_Renamed(iControlCount).TabPages(iColumnheaderCount).Controls(iChildControl).Controls.Count - 1
                                                    IChildChieldCapId = iPMForms.GetCaptionID(Convert.ToString(Me.Controls_Renamed(iControlCount).TabPages(iColumnheaderCount).Controls(iChildControl).Controls(iChildChildControl).Tag))
                                                    If IChildChieldCapId > 0 Then
                                                        Select Case Me.Controls_Renamed(iControlCount).TabPages(iColumnheaderCount).Controls(iChildControl).Controls(iChildChildControl).GetType().Name
                                                            Case "Label"
                                                                Me.Controls_Renamed(iControlCount).TabPages(iColumnheaderCount).Controls(iChildControl).Controls(iChildChildControl).Text = iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=IChildChieldCapId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
                                                        End Select
                                                    End If
                                                Next iChildChildControl

                                        End Select
                                    End If
                                Next iChildControl
                            Next iColumnheaderCount
                            'Added ListView column headers
                        Case "ListView"
                            iSkip = 0

                            For iColumnheaderCount As Integer = 1 To Me.Controls_Renamed(iControlCount).ColumnHeaders.Count
                                'Test for hidden ListView columns and skip as appropriate

                                If gPMFunctions.ToSafeString(Me.Controls_Renamed(iControlCount).ColumnHeaders(iColumnheaderCount).Tag).IndexOf(ksHidden) >= 0 Then
                                    iSkip += 1
                                Else
                                    'PWC - 16/10/2002 - No need to get caption if skipping this time


                                    'developer guide no.243
                                    Me.Controls_Renamed(iControlCount).ColumnHeaders(iColumnheaderCount).Text = iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=lCaptionID + iColumnheaderCount - iSkip - 1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
                                End If
                            Next iColumnheaderCount
                            'Added Picklist
                        Case "PickList"


                            'developer guide no.243
                            Me.Controls_Renamed(iControlCount).AvailableCaption = iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=lCaptionID, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
                        Case Else


                            'developer guide no.243
                            Me.Controls_Renamed(iControlCount).Text = iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=lCaptionID, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)
                    End Select
                End If
            Next iControlCount
            Return result
        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally

        End Try
        Return result
    End Function
    'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.9.1.3)


    ' ***************************************************************** '
    ' Name: PopulateGrossDue
    '
    ' Parameters: n/a
    '
    ' Description: Update renewal finance amount to add previous put_on_mta amount
    '
    ' History:
    '           Created : Prabodh : Date : 03 Feb 2009
    ' ***************************************************************** '
    Public Sub PopulateGrossDue()

        Const kMethodName As String = "PopulateGrossDue"

        Dim m_lReturn As gPMConstants.PMEReturnCode
        Dim vAmountToFinance As Object = Nothing

        Try



            If m_lInsuranceFileCnt_Renewal > 0 And m_sProduct_Code.ToUpper() = "REN" And m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                'Fetch renewal amount including any previous MTA amount to put on

                m_lReturn = m_oBusiness.GetRenewalAmountToFinance(v_lInsuranceFileCnt:=m_lInsuranceFileCnt_Renewal, r_vResults:=vAmountToFinance)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    If Information.IsArray(vAmountToFinance) Then

                        If gPMFunctions.ToSafeCurrency(gPMFunctions.ToSafeString(vAmountToFinance(0, 0)), 0) > 0 Then
                            'if valid amount found replace else continue with the original amount

                            m_lGrossDue = gPMFunctions.ToSafeCurrency(gPMFunctions.ToSafeString(vAmountToFinance(0, 0)), 0)
                        End If
                    End If
                End If
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
    End Sub

    Private Sub HasCheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverrideInterestRate.CheckedChanged, chkCommissionOverride.CheckedChanged, chkDepositOverride.CheckedChanged, chkPaymentProtection.CheckedChanged
        Dim oCheckBox As CheckBox = DirectCast(sender, CheckBox)
        If oCheckBox.Name = "chkDepositOverride" AndAlso chkDepositOverride.Checked = True Then
            _optDepositOverride_1.Checked = CheckState.Checked
        End If
        m_bDataHasChanged = True
    End Sub
    Private Sub chkUseTransCurrency_CheckedChanged(sender As Object, e As EventArgs) Handles chkUseTransCurrency.CheckedChanged

        RefreshQuoteData(True)
        If tdgListInstalment.Visible Then
            If tdgListInstalment.Rows.Count > 0 Then
                tdgListInstalment.Rows.Item(m_iSelectedRowIndex).Selected = True
            End If
        End If

        m_bHasDatesChange = True
        ListInstalment_Click()
        m_bHasDatesChange = False

    End Sub

    Private Sub ListInstalmentSelectRow()
        If m_iSelectedRowIndex >= 0 Then
            tdgListInstalment.FirstDisplayedScrollingRowIndex = m_iSelectedRowIndex
            tdgListInstalment.Rows(m_iSelectedRowIndex).Selected = True
            tdgListInstalment.Rows(m_iSelectedRowIndex).Cells(0).Selected = True
            tdgListInstalment.PerformLayout()
            If IsArray(m_vQuoteArray) AndAlso m_vQuoteArray IsNot Nothing AndAlso Not m_bFirstInstalmentFilled Then
                PopulateFirstInstalmentDate(gPMFunctions.ToSafeInteger(m_vQuoteArray(k_PFQuoteDaysDelay, m_lSelectedQuoteNo)), gPMFunctions.ToSafeInteger(m_vQuoteArray(k_PFStartLimit, m_lSelectedQuoteNo)))
                cboPreferredDate.SelectedIndex = 0
                m_bFirstInstalmentFilled = True
            End If
        End If
    End Sub

    Private Sub optDepositOverride_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _optDepositOverride_0.CheckedChanged, _optDepositOverride_1.CheckedChanged
        If eventSender.Checked Then
            Dim iOptDepositTypeIndex As Integer = Array.IndexOf(optDepositType, eventSender)

            Dim dFieldValue As Double
            If m_bSuppressDecimalValues AndAlso _optDepositOverride_1.Checked Then
                txtOverrideDeposit.Text = 0
            End If

            With m_oFormFields
                For nIndex As Integer = 1 To .Count()
                    If .Item(nIndex).ControlType = SharedFiles.gPMConstants.PMEControlType.PMTextBox Then
                        If DirectCast(.Item(nIndex).FormControl, System.Windows.Forms.TextBox).Name = "txtOverrideDeposit" Then
                            dFieldValue = ToSafeDecimal(m_oFormFields.Item(nIndex).FieldValue)
                            If iOptDepositTypeIndex = 0 Then
                                .Item(nIndex).FieldFormat = gPMConstants.PMEFormatStyle.PMFormatPercent
                            Else
                                .Item(nIndex).FieldFormat = gPMConstants.PMEFormatStyle.PMFormatCurrency
                            End If
                            .FormatControl(txtOverrideDeposit, dFieldValue)
                            txtOverrideDeposit_TextChanged(Nothing, Nothing)
                            Exit For
                        End If
                    End If
                Next nIndex
            End With

            Exit Sub

        End If
    End Sub

    Public Function ValidateData() As Integer

        Dim nResult As Integer = 0
        Dim bIsPlanOnInstalment As Boolean = True
        Try

            If m_sProduct_Code = "MTA" AndAlso m_iMTAType <> bSIRPremFinConst.m_klInstalmentMTAType_AddToNewPlan Then
                If Not IsArray(m_vQuoteArray) AndAlso m_vQuoteArray Is Nothing Then
                    bIsPlanOnInstalment = False
                End If
            End If

            If txtOverrideDeposit.Text.Trim = "" Then
                txtOverrideDeposit.Text = "0.00"
            End If

            If bIsPlanOnInstalment Then
                If Val(txtFinancedAmount.Text) = 0 AndAlso m_sProduct_Code.ToUpper() = "MTA" Then
                    MessageBox.Show("Since it is a Zero Premium transaction so cannot proceed with instalment.", "Instalment Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                Dim dDepositPC As Double = m_oFormFields.UnformatControl(txtOverrideDeposit)
                Dim dFinanceAmount As Double = m_vQuoteArray(k_PFQuoteTotalAmountInput, m_lSelectedQuoteNo)

                If (_optMTAType_0.Checked OrElse _optMTAType_1.Checked) AndAlso m_sProduct_Code.ToUpper() = "MTA" Then
                    dFinanceAmount = m_vQuoteArray(k_PFQuoteTotalInstalmentsAmount, m_lSelectedQuoteNo)
                End If

                If (_optDepositOverride_0.Checked AndAlso (dDepositPC < 0 Or dDepositPC > 100)) Then
                    MessageBox.Show("The deposit percentage should be between 0 and 100. Please enter a valid deposit percentage.", "Deposit Override Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtOverrideDeposit.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                If (_optDepositOverride_1.Checked AndAlso (dDepositPC < 0 Or dDepositPC > dFinanceAmount)) Then
                    MessageBox.Show("The deposit amount should be between 0 and the financed amount. Please enter a valid deposit amount.", "Deposit Override Validation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtOverrideDeposit.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Validate User Data", ACApp, ACClass, "ValidateData", excep:=excep)
            Return nResult
        End Try
    End Function
    Private Sub txtOverrideDeposit_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtOverrideDeposit.KeyPress
        If m_bSuppressDecimalValues AndAlso _optDepositOverride_1.Checked Then
            'Disallow the decimals
            gPMFunctions.NumPress(sender, e)
        End If

    End Sub

    Private Sub _ssTabMain_TabPage0_Click(sender As Object, e As EventArgs) Handles _ssTabMain_TabPage0.Click

    End Sub

    Public Function GetAttachedPlanStatus(ByRef o_sPlanStatusInd As String, Optional ByVal sBusinessType As String = "") As Integer
        Const kMethodName As String = "GetAttachedPlanStatus"
        Dim oPFPremiumFinance As Object
        Dim nResult As PMEReturnCode

        Try
            nResult = gPMConstants.PMEReturnCode.PMFalse
            nResult = m_oBusiness.GetLatestValidFinancePlan(nInsuranceFileCnt:=m_lInsuranceFileCnt, r_oPFPremiumFinance:=oPFPremiumFinance, sBusinessType:=sBusinessType)

            If Not IsArray(oPFPremiumFinance) Then
                o_sPlanStatusInd = ""
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If
            o_sPlanStatusInd = ToSafeString(oPFPremiumFinance(k_PFPlanStatusInd, 0))
        Catch ex As Exception
            iPMFunc.LogError(v_sUsername:="", v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult)
        End Try

        Return nResult
    End Function

End Class
