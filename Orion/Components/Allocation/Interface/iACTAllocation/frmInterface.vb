Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    Private Const ACClass As String = "frmAllocate"
    '*****
    'todolist (Commented splits in the whole document)
    '*****

    ' Object parameter members.
    Public CallingAppName As String = ""
    Public Status As gPMConstants.PMEReturnCode
    Public Task As Integer
    Public ErrorNumber As gPMConstants.PMEReturnCode
    Public Navigate As Integer
    Public ProcessMode As Integer
    Public TypeOfBusiness As String = ""
    Public TransactionType As String = ""
    Public EffectiveDate As Date

    Public AllocationArray(,) As Object
    Public AccountID As Integer
    Public ReturnValue As gPMConstants.PMEReturnCode
    Public CompanyID As Integer
    Public CashListTypeID As Integer
    Public CashListItemID As Integer 'eck220904

    ' Stores the return value for a function call.
    Private m_lReturn As Integer

    Private m_iMaxCol As Integer
    Private m_lMaxRow As Integer
    Private m_cWOTotal As Decimal
    Private m_cCurrDiff As Decimal
    Private m_cBalance As Decimal
    Private m_bMultipleWriteOff As Boolean
    Private m_lWriteOffRow As Integer
    Private m_lFirstCreditRow As Integer
    Private m_iCol As Integer


    Private m_oBusiness As bACTFindTransaction.Business

    Private m_oCurrencyConvert As bACTCurrencyConvert.Form

    Private m_oUserAuthorities As bACTUserAuthorities.Business

    'DD 16/10/2003: Moved here for better cosmetic setup
    Private m_bMultiCurrency As Boolean
    'KB 24/10/2003: Distinguish between write-off in currency and currency exchange difference
    Public m_bExchangeRateDiff As Boolean

    Private m_oGridArray As XArrayHelper

    'todolist
    'Private m_oFindTransaction As iACTFindTransaction.Interface_Renamed
    Private m_oFindTransaction As Object
    Private Const k_BackColour As Integer = 13158600 'Light grey

    'DC180205 : PN19114 : underwriting or agency
    Private m_vUnderwritingOrAgency As Object
    Private m_lCurrencyExchangeDiffId As Integer
    Private m_vWriteOffReasons(,) As Object

    Private m_iSelectedCurrencyId As Integer
    Private m_iSelectedSourceId As Integer
    Private bLedgerTypeAllowWriteOff As Boolean

    Public WriteOnly Property SelectedSourceId() As Integer
        Set(ByVal Value As Integer)
            m_iSelectedSourceId = Value
        End Set
    End Property

    Public WriteOnly Property SelectedCurrencyId() As Integer
        Set(ByVal Value As Integer)
            m_iSelectedCurrencyId = Value
        End Set
    End Property

    Public Function Initialise() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: Initialise
        ' PURPOSE:
        ' AUTHOR: Danny Davis
        ' DATE: 19 April 2004, 13:19:14
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_g_oAccount As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oAccount, "bACTAccount.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oAccount = temp_g_oAccount
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to create instance of bACTAccount.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="UnLockTransdetailId")
                Return result
            End If


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally


        End Try
        Return result
    End Function

    Public Function Load_Renamed() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: Load
        ' PURPOSE:
        ' AUTHOR: Danny Davis
        ' DATE: 19 April 2004, 13:19:50
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim bLocked As Boolean


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Lock the Transactions whilst the form is open
            For lRow As Integer = 0 To AllocationArray.GetUpperBound(0)
                If LockTransdetailId(CInt(AllocationArray(lRow, gACTLibrary.k_ACTransDetail_id)), bLocked) = gPMConstants.PMEReturnCode.PMFalse Then
                    ErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                ElseIf bLocked Then
                    ErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            Next lRow

            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTFindTransaction.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to create instance of bACTFindTransaction.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Load")
                Return result
            End If

            'Add/Remove is not available from Find Transaction
            cmdAdd.Visible = (CallingAppName = "iACTCashListItem")
            cmdRemove.Visible = (CallingAppName = "iACTCashListItem")

            'Enable/disable the writeoff controls
            AllowWriteOff()


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally


        End Try
        Return result
    End Function

    Private Sub SetupGrid()
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: SetupGrid
        ' PURPOSE: Initialise the TrueDB Grid
        ' AUTHOR: Danny Davis
        ' DATE: 12/05/2003, 13:34
        ' CHANGES:
        ' ---------------------------------------------------------------------------


        Try

            Dim lRowCount, lColCount, lCol As Integer

            Dim Col As DataGridViewColumn

            Dim S As Artinsoft.VB6.Gui.Split

            Dim SimpleArray() As Object
            Dim iUboundRow, iLBoundRow As Integer
            Dim iOrigArrayRowCount As Integer

            Const k_NumberWidth As Integer = 1350

            'DC180305 : PN19114 : use common variable for underwriting or agency
            'Dim vResult As Variant

            Dim cDiff As Decimal


            m_bMultiCurrency = False
            For lRow As Integer = 0 To AllocationArray.GetUpperBound(0)
                If Not AllocationArray(lRow, gACTLibrary.k_ACTransactionCurrencyID).Equals(AllocationArray(lRow, gACTLibrary.k_ACBaseCurrencyID)) Then
                    m_bMultiCurrency = True
                End If
            Next lRow


            AutoCalculateAllocation()

            'DC180305 : PN19114
            'DC220305 : PN19710 : only find currency exchange difference automatically if no via cashlistitem

            'Set up the internal array
            CopyArray(AllocationArray, m_oGridArray, True, True)

            With tdbGrid.Columns
                While .Count > 0
                    .RemoveAt(0)
                End While

                While .Count <= gACTLibrary.k_ACAllocationArraySize
                    Dim newColumn As Artinsoft.Windows.Forms.DataGridViewExtendedColumn = Nothing
                    newColumn = New Artinsoft.Windows.Forms.DataGridViewExtendedColumn()
                    .Add(newColumn)
                    Col = newColumn
                    Col.Visible = False
                    Col.ReadOnly = True
                    Col.Frozen = True
                    'todolist
                    'Col.AllowFocus = False

                    'todolist
                    'Col.ButtonHeader = False
                    Col.Width = 0
                    Col.Resizable = False

                    Col.HeaderCell.Style.Font = VB6.FontChangeBold(tdbGrid.ColumnHeadersDefaultCellStyle.Font, True)

                    Col.HeaderCell.Style.Font = VB6.FontChangeSize(tdbGrid.ColumnHeadersDefaultCellStyle.Font, 8)

                End While
            End With


            'todolist
            'S = tdbGrid.Splits.Add(0)
            With S



            End With

            'DC180305 : PN19114 : broking always shows multi currency screen


            'todolist (Commented the whole with block)

            'With tdbGrid.Splits(0).Columns
            '    'Add TransDetail ID

            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACTransDetail_id).HeaderText = "transdetail_id"

            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACTransDetail_id).DefaultCellStyle.Format = Artinsoft.Windows.Forms.NumberFormatConstants.GeneralNumber

            '    'Add Document Ref

            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACDocument_Ref).HeaderText = "Document Ref"

            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACDocument_Ref).Visible = True

            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACDocument_Ref).DefaultCellStyle.BackColor = ColorTranslator.FromOle(k_BackColour)

            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACDocument_Ref).Width = 1500

            '    'Add transaction type

            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACTypeCode).HeaderText = "Type"


            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACTypeCode).Visible = True

            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACTypeCode).DefaultCellStyle.BackColor = ColorTranslator.FromOle(k_BackColour)

            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACTypeCode).Width = 1500


            '    'Add tax band code

            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACTaxBandCode).HeaderText = "Tax Band"


            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACTaxBandCode).Visible = True

            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACTaxBandCode).DefaultCellStyle.BackColor = ColorTranslator.FromOle(k_BackColour)

            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACTaxBandCode).Width = 1500


            '    'Add Currency ID

            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACTransactionCurrencyID).HeaderText = "currency_id"

            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACTransactionCurrencyID).DefaultCellStyle.Format = Artinsoft.Windows.Forms.NumberFormatConstants.GeneralNumber

            '    'Add Currency column (visible in Multi Currency)

            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACTransactionCurrency).HeaderText = "Currency"

            '    'DC180305 : PN19114 : always show multicurrecy for broking


            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACTransactionCurrency).Visible = m_bMultiCurrency

            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACTransactionCurrency).Width = 1500 * Math.Abs(CInt(m_bMultiCurrency))


            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACTransactionCurrency).DefaultCellStyle.BackColor = ColorTranslator.FromOle(k_BackColour)

            '    'Add Currency Amount (visible in Multi Currency)

            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACTransactionAmount).HeaderText = "Amount"

            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACTransactionAmount).DefaultCellStyle.Format = Artinsoft.Windows.Forms.NumberFormatConstants.Standard

            '    'DC180305 : PN19114 : always show multicurrecy for broking


            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACTransactionAmount).Visible = m_bMultiCurrency

            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACTransactionAmount).Width = k_NumberWidth * Math.Abs(CInt(m_bMultiCurrency))



            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACTransactionAmount).DefaultCellStyle.BackColor = ColorTranslator.FromOle(k_BackColour)

            '    tdbGrid.Splits(0).Columns(gACTLibrary.k_ACTransactionAmount).DefaultCellStyle.Alignment = ContentAlignment.MiddleRight
            'End With



            'todolist (Commented the whole with block)
            'With tdbGrid.Splits(1).Columns
            '	'Add Base Currency ID

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACBaseCurrencyID).HeaderText = "currency_id"

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACBaseCurrencyID).DefaultCellStyle.Format = Artinsoft.Windows.Forms.NumberFormatConstants.GeneralNumber

            '	'Add Base Currency column

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACBaseCurrency).HeaderText = "Currency"

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACBaseCurrency).Visible = True

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACBaseCurrency).DefaultCellStyle.BackColor = ColorTranslator.FromOle(k_BackColour)

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACBaseCurrency).Width = 1600

            '	'Add O/S Amount

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACBaseOutstanding).HeaderText = "O/S Amount"

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACBaseOutstanding).DefaultCellStyle.Format = Artinsoft.Windows.Forms.NumberFormatConstants.Standard

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACBaseOutstanding).Visible = True

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACBaseOutstanding).DefaultCellStyle.BackColor = ColorTranslator.FromOle(k_BackColour)

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACBaseOutstanding).Width = k_NumberWidth

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACBaseOutstanding).DefaultCellStyle.Alignment = ContentAlignment.MiddleRight

            '	'Add Allocate amount

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACBaseAllocated).HeaderText = "Allocated"

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACBaseAllocated).DefaultCellStyle.Format = Artinsoft.Windows.Forms.NumberFormatConstants.Standard

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACBaseAllocated).Visible = True


            '	.Item(gACTLibrary.k_ACBaseAllocated).AllowFocus = True

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACBaseAllocated).ReadOnly = False

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACBaseAllocated).Width = k_NumberWidth

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACBaseAllocated).DefaultCellStyle.Alignment = ContentAlignment.MiddleRight


            '	.Item(gACTLibrary.k_ACBaseAllocated).FetchStyle = True

            '	'Add Write-Off

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACWriteOff).HeaderText = "Write Off"

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACWriteOff).DefaultCellStyle.Format = Artinsoft.Windows.Forms.NumberFormatConstants.Standard

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACWriteOff).Visible = True


            '	.Item(gACTLibrary.k_ACWriteOff).AllowFocus = True

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACWriteOff).ReadOnly = False

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACWriteOff).Width = k_NumberWidth

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACWriteOff).DefaultCellStyle.Alignment = ContentAlignment.MiddleRight


            '	.Item(gACTLibrary.k_ACWriteOff).FetchStyle = True

            '	'Add Currency Write-Off

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACCurrencyDifference).HeaderText = "Currency Diff."

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACCurrencyDifference).DefaultCellStyle.Format = Artinsoft.Windows.Forms.NumberFormatConstants.Standard

            '	'DC180305 : PN19114 : always show multicurrecy for broking


            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACCurrencyDifference).Visible = m_bMultiCurrency


            '	.Item(gACTLibrary.k_ACCurrencyDifference).AllowFocus = m_bMultiCurrency

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACCurrencyDifference).ReadOnly = Not m_bMultiCurrency

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACCurrencyDifference).Width = k_NumberWidth * Math.Abs(CInt(m_bMultiCurrency))



            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACCurrencyDifference).DefaultCellStyle.Alignment = ContentAlignment.MiddleRight


            '	.Item(gACTLibrary.k_ACCurrencyDifference).FetchStyle = True

            '	'Add Tax Group

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACTTaxGroupCode).HeaderText = "Tax Group"

            '	' Add allocation columns

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACTAllocSequence).HeaderText = "Allocation Sequence"

            '	tdbGrid.Splits(1).Columns(gACTLibrary.k_ACTAllocRule).HeaderText = "Allocation Rule"
            'End With

            '*******Modified Code Start(As Splits functionality does not work in DotNet)
            'With tdbGrid.Columns
            'Add TransDetail ID

            tdbGrid.Columns(gACTLibrary.k_ACTransDetail_id).HeaderText = "transdetail_id"

            tdbGrid.Columns(gACTLibrary.k_ACTransDetail_id).DefaultCellStyle.Format = Artinsoft.Windows.Forms.NumberFormatConstants.GeneralNumber

            'Add Document Ref

            tdbGrid.Columns(gACTLibrary.k_ACDocument_Ref).HeaderText = "Document Ref"

            tdbGrid.Columns(gACTLibrary.k_ACDocument_Ref).Visible = True

            tdbGrid.Columns(gACTLibrary.k_ACDocument_Ref).DefaultCellStyle.BackColor = ColorTranslator.FromOle(k_BackColour)

            tdbGrid.Columns(gACTLibrary.k_ACDocument_Ref).Width = VB6.TwipsToPixelsX(1500)

            'Add transaction type

            tdbGrid.Columns(gACTLibrary.k_ACTypeCode).HeaderText = "Type"


            tdbGrid.Columns(gACTLibrary.k_ACTypeCode).Visible = True

            tdbGrid.Columns(gACTLibrary.k_ACTypeCode).DefaultCellStyle.BackColor = ColorTranslator.FromOle(k_BackColour)

            tdbGrid.Columns(gACTLibrary.k_ACTypeCode).Width = VB6.TwipsToPixelsX(1500)


            'Add tax band code

            tdbGrid.Columns(gACTLibrary.k_ACTaxBandCode).HeaderText = "Tax Band"


            tdbGrid.Columns(gACTLibrary.k_ACTaxBandCode).Visible = True

            tdbGrid.Columns(gACTLibrary.k_ACTaxBandCode).DefaultCellStyle.BackColor = ColorTranslator.FromOle(k_BackColour)

            tdbGrid.Columns(gACTLibrary.k_ACTaxBandCode).Width = VB6.TwipsToPixelsX(1500)


            'Add Currency ID

            tdbGrid.Columns(gACTLibrary.k_ACTransactionCurrencyID).HeaderText = "currency_id"

            tdbGrid.Columns(gACTLibrary.k_ACTransactionCurrencyID).DefaultCellStyle.Format = Artinsoft.Windows.Forms.NumberFormatConstants.GeneralNumber

            'Add Currency column (visible in Multi Currency)

            tdbGrid.Columns(gACTLibrary.k_ACTransactionCurrency).HeaderText = "Currency"

            'DC180305 : PN19114 : always show multicurrecy for broking


            tdbGrid.Columns(gACTLibrary.k_ACTransactionCurrency).Visible = m_bMultiCurrency

            tdbGrid.Columns(gACTLibrary.k_ACTransactionCurrency).Width = VB6.TwipsToPixelsX(1500) * Math.Abs(CInt(m_bMultiCurrency))


            tdbGrid.Columns(gACTLibrary.k_ACTransactionCurrency).DefaultCellStyle.BackColor = ColorTranslator.FromOle(k_BackColour)

            'Add Currency Amount (visible in Multi Currency)

            tdbGrid.Columns(gACTLibrary.k_ACTransactionAmount).HeaderText = "Amount"

            tdbGrid.Columns(gACTLibrary.k_ACTransactionAmount).DefaultCellStyle.Format = Artinsoft.Windows.Forms.NumberFormatConstants.Standard

            'DC180305 : PN19114 : always show multicurrecy for broking


            tdbGrid.Columns(gACTLibrary.k_ACTransactionAmount).Visible = m_bMultiCurrency

            tdbGrid.Columns(gACTLibrary.k_ACTransactionAmount).Width = VB6.TwipsToPixelsX(k_NumberWidth) * Math.Abs(CInt(m_bMultiCurrency))



            tdbGrid.Columns(gACTLibrary.k_ACTransactionAmount).DefaultCellStyle.BackColor = ColorTranslator.FromOle(k_BackColour)

            tdbGrid.Columns(gACTLibrary.k_ACTransactionAmount).DefaultCellStyle.Alignment = ContentAlignment.MiddleRight
            'End With



            'todolist (Commented the whole with block)
            'With tdbGrid
            'Add Base Currency ID

            tdbGrid.Columns(gACTLibrary.k_ACBaseCurrencyID).HeaderText = "currency_id"

            tdbGrid.Columns(gACTLibrary.k_ACBaseCurrencyID).DefaultCellStyle.Format = Artinsoft.Windows.Forms.NumberFormatConstants.GeneralNumber

            'Add Base Currency column

            tdbGrid.Columns(gACTLibrary.k_ACBaseCurrency).HeaderText = "Currency"

            tdbGrid.Columns(gACTLibrary.k_ACBaseCurrency).Visible = True

            tdbGrid.Columns(gACTLibrary.k_ACBaseCurrency).DefaultCellStyle.BackColor = ColorTranslator.FromOle(k_BackColour)

            tdbGrid.Columns(gACTLibrary.k_ACBaseCurrency).Width = VB6.TwipsToPixelsX(1600)

            'Add O/S Amount

            tdbGrid.Columns(gACTLibrary.k_ACBaseOutstanding).HeaderText = "O/S Amount"

            tdbGrid.Columns(gACTLibrary.k_ACBaseOutstanding).DefaultCellStyle.Format = Artinsoft.Windows.Forms.NumberFormatConstants.Standard

            tdbGrid.Columns(gACTLibrary.k_ACBaseOutstanding).Visible = True

            tdbGrid.Columns(gACTLibrary.k_ACBaseOutstanding).DefaultCellStyle.BackColor = ColorTranslator.FromOle(k_BackColour)

            tdbGrid.Columns(gACTLibrary.k_ACBaseOutstanding).Width = VB6.TwipsToPixelsX(k_NumberWidth)

            tdbGrid.Columns(gACTLibrary.k_ACBaseOutstanding).DefaultCellStyle.Alignment = ContentAlignment.MiddleRight

            'Add Allocate amount

            tdbGrid.Columns(gACTLibrary.k_ACBaseAllocated).HeaderText = "Allocated"

            tdbGrid.Columns(gACTLibrary.k_ACBaseAllocated).DefaultCellStyle.Format = Artinsoft.Windows.Forms.NumberFormatConstants.Standard

            tdbGrid.Columns(gACTLibrary.k_ACBaseAllocated).Visible = True


            'todolist
            '.Item(gACTLibrary.k_ACBaseAllocated).AllowFocus = True

            tdbGrid.Columns(gACTLibrary.k_ACBaseAllocated).ReadOnly = False

            tdbGrid.Columns(gACTLibrary.k_ACBaseAllocated).Width = VB6.TwipsToPixelsX(k_NumberWidth)

            tdbGrid.Columns(gACTLibrary.k_ACBaseAllocated).DefaultCellStyle.Alignment = ContentAlignment.MiddleRight


            'todolist
            '.Item(gACTLibrary.k_ACBaseAllocated).FetchStyle = True

            'Add Write-Off

            tdbGrid.Columns(gACTLibrary.k_ACWriteOff).HeaderText = "Write Off"

            tdbGrid.Columns(gACTLibrary.k_ACWriteOff).DefaultCellStyle.Format = Artinsoft.Windows.Forms.NumberFormatConstants.Standard

            tdbGrid.Columns(gACTLibrary.k_ACWriteOff).Visible = True


            'todolist
            '.Item(gACTLibrary.k_ACWriteOff).AllowFocus = True

            If bLedgerTypeAllowWriteOff Then
                tdbGrid.Columns(gACTLibrary.k_ACWriteOff).ReadOnly = True
            Else
                tdbGrid.Columns(gACTLibrary.k_ACWriteOff).ReadOnly = True
            End If

            tdbGrid.Columns(gACTLibrary.k_ACWriteOff).Width = VB6.TwipsToPixelsX(k_NumberWidth)

            tdbGrid.Columns(gACTLibrary.k_ACWriteOff).DefaultCellStyle.Alignment = ContentAlignment.MiddleRight


            'todolist
            '.Item(gACTLibrary.k_ACWriteOff).FetchStyle = True

            'Add Currency Write-Off

            tdbGrid.Columns(gACTLibrary.k_ACCurrencyDifference).HeaderText = "Currency Diff."

            tdbGrid.Columns(gACTLibrary.k_ACCurrencyDifference).DefaultCellStyle.Format = Artinsoft.Windows.Forms.NumberFormatConstants.Standard

            'DC180305 : PN19114 : always show multicurrecy for broking


            tdbGrid.Columns(gACTLibrary.k_ACCurrencyDifference).Visible = m_bMultiCurrency


            'todolist
            '.Item(gACTLibrary.k_ACCurrencyDifference).AllowFocus = m_bMultiCurrency

            tdbGrid.Columns(gACTLibrary.k_ACCurrencyDifference).ReadOnly = Not m_bMultiCurrency

            tdbGrid.Columns(gACTLibrary.k_ACCurrencyDifference).Width = VB6.TwipsToPixelsX(k_NumberWidth) * Math.Abs(CInt(m_bMultiCurrency))



            tdbGrid.Columns(gACTLibrary.k_ACCurrencyDifference).DefaultCellStyle.Alignment = ContentAlignment.MiddleRight


            'todolist
            '.Item(gACTLibrary.k_ACCurrencyDifference).FetchStyle = True

            'Add Tax Group

            tdbGrid.Columns(gACTLibrary.k_ACTTaxGroupCode).HeaderText = "Tax Group"

            ' Add allocation columns

            tdbGrid.Columns(gACTLibrary.k_ACTAllocSequence).HeaderText = "Allocation Sequence"

            tdbGrid.Columns(gACTLibrary.k_ACTAllocRule).HeaderText = "Allocation Rule"
            'End With
            '*******Code End




            'todolist
            'tdbGrid.Splits(1).AllowColSelect = False



            tdbGrid.AllowRowSelection = False

            With tdbGrid
                Dim bindingSource As BindingSource = New BindingSource(m_oGridArray, "")
                .DataSource = bindingSource
                .ReBind()
                .Refresh()

                'todolist
                '.RecordSelectors = False

                'todolist
                '.MarqueeStyle = 4
                'todolist
                '.HighlightRowStyle.BackColor = Color.FromArgb(128, 128, 128)

                'DC180305 : PN19114 : always show multicurrecy for broking
                If m_bMultiCurrency Then
                    m_iMaxCol = gACTLibrary.k_ACAllocationArraySize
                Else
                    m_iMaxCol = gACTLibrary.k_ACWriteOff
                End If


                m_iCol = gACTLibrary.k_ACBaseAllocated
            End With

            'DC180305 : PN19114 : always show multicurrecy for broking
            lblCurrencyTotal.Visible = m_bMultiCurrency
            cboWriteOffReasonId.Enabled = False


            frmInterface_Resize(Me, New EventArgs())
            CalculateTotals()



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="SetupGrid", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Exit Sub
            End Select

        Finally


        End Try
        Exit Sub
    End Sub


    Private Sub CalculateTotals()
        Try

            Dim cOutstanding, cAllocatedTotal As Decimal

            'Initialise
            cAllocatedTotal = 0
            cOutstanding = 0
            m_cBalance = 0
            m_cWOTotal = 0
            m_lWriteOffRow = -1
            m_bMultipleWriteOff = False
            m_lFirstCreditRow = 0
            m_cCurrDiff = 0

            'Ensure the data is in sync with the UI
            tdbGrid.UpdateCurrentRow()

            'Copy the array back
            CopyArray(AllocationArray, m_oGridArray, False, False)

            'Loop through the main array updating the totals
            For lRow As Integer = 0 To m_lMaxRow
                'Note the first credit row
                If gPMFunctions.ToSafeDecimal(gPMFunctions.ToSafeString(AllocationArray(lRow, gACTLibrary.k_ACBaseOutstanding))) < 0 And m_lFirstCreditRow = 0 Then
                    m_lFirstCreditRow = lRow
                End If

                cOutstanding += gPMFunctions.ToSafeDecimal(gPMFunctions.ToSafeString(AllocationArray(lRow, gACTLibrary.k_ACBaseOutstanding)))
                m_cBalance += gPMFunctions.ToSafeDecimal(gPMFunctions.ToSafeString(AllocationArray(lRow, gACTLibrary.k_ACBaseAllocated)))
                cAllocatedTotal += gPMFunctions.ToSafeDecimal(gPMFunctions.ToSafeString(AllocationArray(lRow, gACTLibrary.k_ACBaseAllocated)))

                If (gPMFunctions.ToSafeDecimal(gPMFunctions.ToSafeString(AllocationArray(lRow, gACTLibrary.k_ACWriteOff))) <> 0 Or gPMFunctions.ToSafeDecimal(gPMFunctions.ToSafeString(AllocationArray(lRow, gACTLibrary.k_ACCurrencyDifference))) <> 0) And (m_cWOTotal <> 0 Or m_cCurrDiff <> 0) Then
                    'Multiple write-offs are not supported
                    m_bMultipleWriteOff = True
                ElseIf gPMFunctions.ToSafeDecimal(gPMFunctions.ToSafeString(AllocationArray(lRow, gACTLibrary.k_ACWriteOff))) <> 0 OrElse
                    (gPMFunctions.ToSafeDecimal(gPMFunctions.ToSafeString(AllocationArray(lRow, gACTLibrary.k_ACCurrencyDifference)))) Then

                    m_lWriteOffRow = lRow
                End If

                m_cWOTotal += gPMFunctions.ToSafeDecimal(gPMFunctions.ToSafeString(AllocationArray(lRow, gACTLibrary.k_ACWriteOff)))
                m_cCurrDiff += gPMFunctions.ToSafeDecimal(gPMFunctions.ToSafeString(AllocationArray(lRow, gACTLibrary.k_ACCurrencyDifference)))
            Next lRow

            'Fill in the lables
            lblOSTotal.Text = StringsHelper.Format(cOutstanding, "#,##0.00")
            lblAllocatedTotal.Text = StringsHelper.Format(cAllocatedTotal, "#,##0.00")
            lblCurrencyTotal.Text = StringsHelper.Format(m_cCurrDiff, "#,##0.00")
            lblWriteOffTotal.Text = StringsHelper.Format(m_cWOTotal, "#,##0.00")
            lblBalance.Text = StringsHelper.Format(m_cBalance, "#,##0.00")

            'Enable the write-off combo
            'DC180305 : PN19114 : only applicable for underwriting

            cboWriteOffReasonId.Enabled = (m_cWOTotal <> 0 Or m_cCurrDiff <> 0)


            tdbGrid.Refresh()
            Application.DoEvents()

        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateTotals", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
                    Exit Sub
            End Select

        Finally

        End Try
        Exit Sub
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click
        Dim oSelectedItems As Object
        'DC220305 : PN19710 :
        Dim oAllocation As Object
        Dim lRow As Long

        'Get an Instance of Find Transaction
        If m_oFindTransaction Is Nothing Then
            Dim temp_m_oFindTransaction As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oFindTransaction, sClassName:="iACTFindTransaction.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oFindTransaction = temp_m_oFindTransaction
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to create instance of iACTFindTransaction.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdd_Click")
                Exit Sub
            End If
        End If

        'Fire it up

        m_oFindTransaction.CompanyID = CompanyID

        m_oFindTransaction.AccountID = AccountID

        m_oFindTransaction.CashListTypeID = CashListTypeID

        m_oFindTransaction.DocumentRef = ""

        m_oFindTransaction.CallingAppName = ACApp

        'PN14927 - pass through transaction to exclude during Cash List Allocation
        If CallingAppName = "iACTCashListItem" Then

            'developer guide no.188
            'm_oFindTransaction.ExcludeTransDetailID = m_oGridArray(<untranslated-structure-in-to-res: row>, <untranslated-structure-in-to-res: column>)
            m_oFindTransaction.ExcludeTransDetailID = m_oGridArray(0, k_ACTransDetail_id)
        Else

            m_oFindTransaction.ExcludeTransDetailID = 0
        End If


        m_oFindTransaction.SelectedCurrencyId = m_iSelectedCurrencyId

        m_oFindTransaction.SelectedSourceId = m_iSelectedSourceId

        m_oFindTransaction.Start()

        'Get the returned items

        If m_oFindTransaction.Status = gPMConstants.PMEReturnCode.PMOK Then


            oSelectedItems = m_oFindTransaction.SelectedItems
            'PN16769
            For lRow = 0 To m_oGridArray.Rows.Count - 1
                If m_oFindTransaction.DocumentRef = m_oGridArray(lRow, k_ACDocument_Ref) Then
                    MessageBox.Show("Selected Transaction is already part of this allocation", "Error", MessageBoxButtons.OK)
                    Exit Sub
                End If
            Next

            m_lReturn = g_oObjectManager.GetInstance(oObject:=oAllocation, sClassName:="bACTAllocation.Form", vInstanceManager:=PMGetViaClientManager)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            End If
            'PM019290  Sumit This function is added to check if the scheme type is Third Party
            For lRow = 1 To oSelectedItems.Count
                If ChkDocTypeIsInstalments(Strings.Left(oSelectedItems.Item(lRow).DocumentRef, 3)) = True _
                AndAlso CheckIsLinkedToThirdPartyScheme(oSelectedItems.Item(lRow).DocumentRef.ToString) = False Then
                    MsgBox("Instalment transactions are handled automatically by Sirius. They are " &
                   "not allowed to be manually allocated.", vbCritical, "Invalid Selection")
                    Exit Sub
                End If
            Next lRow
            lRow = 0

            oAllocation = Nothing

            For lRow = 1 To oSelectedItems.Count
                'Appendrows doesnt replicate the increment in no of rows.
                'm_oGridArray.AppendRows()
                m_oGridArray.Rows.InsertAt(m_oGridArray.NewRow, m_oGridArray.Rows.Count)

                m_oGridArray(m_oGridArray.Rows.Count - 1, gACTLibrary.k_ACTransDetail_id) = oSelectedItems.Item(lRow).TransdetailID

                m_oGridArray(m_oGridArray.Rows.Count - 1, gACTLibrary.k_ACDocument_Ref) = oSelectedItems.Item(lRow).DocumentRef

                m_oGridArray(m_oGridArray.Rows.Count - 1, gACTLibrary.k_ACTypeCode) = oSelectedItems.Item(lRow).TransDetailTypeCode

                m_oGridArray(m_oGridArray.Rows.Count - 1, gACTLibrary.k_ACTaxBandCode) = oSelectedItems.Item(lRow).TaxBandCode

                m_oGridArray(m_oGridArray.Rows.Count - 1, gACTLibrary.k_ACTransactionCurrencyID) = oSelectedItems.Item(lRow).CurrencyID

                m_oGridArray(m_oGridArray.Rows.Count - 1, gACTLibrary.k_ACTransactionCurrency) = oSelectedItems.Item(lRow).CurrencyText

                m_oGridArray(m_oGridArray.Rows.Count - 1, gACTLibrary.k_ACTransactionAmount) = oSelectedItems.Item(lRow).CurrencyAmount

                m_oGridArray(m_oGridArray.Rows.Count - 1, gACTLibrary.k_ACBaseCurrencyID) = oSelectedItems.Item(lRow).BaseCurrencyID

                m_oGridArray(m_oGridArray.Rows.Count - 1, gACTLibrary.k_ACBaseCurrency) = oSelectedItems.Item(lRow).BaseCurrencyText

                m_oGridArray(m_oGridArray.Rows.Count - 1, gACTLibrary.k_ACBaseOutstanding) = oSelectedItems.Item(lRow).BaseAmount

                m_oGridArray(m_oGridArray.Rows.Count - 1, gACTLibrary.k_ACTTaxGroupCode) = oSelectedItems.Item(lRow).TaxGroupCode

                m_oGridArray(m_oGridArray.Rows.Count - 1, gACTLibrary.k_ACTAllocSequence) = oSelectedItems.Item(lRow).AllocationSequence

                m_oGridArray(m_oGridArray.Rows.Count - 1, gACTLibrary.k_ACTAllocRule) = oSelectedItems.Item(lRow).AllocationRule
            Next lRow

            'Move the new data into the grid
            CopyArray(AllocationArray, m_oGridArray, False, True)
            AutoCalculateAllocation()

            'DC220305 : PN19710 : calculate if there is a currency exchange difference
            CopyArray(AllocationArray, m_oGridArray, True, False)
            tdbGrid.ReBind()
            tdbGrid.Refresh()
            CalculateTotals()
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Me.Hide()
    End Sub

    Private Function CopyArray(ByRef r_vAllocationArray(,) As Object, ByRef r_oGridArray As XArrayHelper, ByVal bToGrid As Boolean, ByVal bReDim As Boolean) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: CopyArray
        ' PURPOSE: Copy the information from the AllocationArray to/from XArrayDB array
        ' AUTHOR: Danny Davis
        ' DATE: 23 April 2004, 17:13:55
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If bToGrid Then
                If bReDim Then
                    r_oGridArray = New XArrayHelper()
                    r_oGridArray.RedimXArray(New Integer() {AllocationArray.GetUpperBound(0), gACTLibrary.k_ACAllocationArraySize}, New Integer() {0, 0})
                End If

                For lRow As Integer = r_oGridArray.GetLowerBound(0) To r_oGridArray.Rows.Count - 1
                    For lCol As Integer = r_oGridArray.GetLowerBound(1) To r_oGridArray.GetUpperBound(1)
                        'Added the following check to avoid dbnull copy.
                        'start
                        If Information.IsNothing(AllocationArray(lRow, lCol)) Then
                            r_oGridArray(lRow, lCol) = ""
                        Else
                            r_oGridArray(lRow, lCol) = AllocationArray(lRow, lCol)
                        End If
                        'end
                    Next lCol
                Next lRow
            Else
                If bReDim Then
                    ReDim r_vAllocationArray(r_oGridArray.Rows.Count - 1, r_oGridArray.GetUpperBound(1))
                End If

                For lRow As Integer = r_oGridArray.GetLowerBound(0) To r_oGridArray.Rows.Count - 1
                    For lCol As Integer = r_oGridArray.GetLowerBound(1) To r_oGridArray.GetUpperBound(1)
                        'developer guide no.188
                        If AllocationArray(lRow, gACTLibrary.k_ACBaseOutstanding) < 0 And gPMFunctions.ToSafeInteger(m_oGridArray(lRow, lCol)) > 0 And lCol = gACTLibrary.k_ACBaseAllocated Then
                            m_oGridArray(lRow, lCol) = Conversion.Val(m_oGridArray(lRow, lCol)) * -1
                        End If
                        AllocationArray(lRow, lCol) = r_oGridArray(lRow, lCol)
                    Next lCol
                Next lRow
            End If

            'Reset the max row
            m_lMaxRow = r_oGridArray.Rows.Count - 1



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyArray", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


            End Select

        Finally



        End Try
        Return result
    End Function

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        CalculateTotals()
        m_lReturn = PerformAllocation()
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            ReturnValue = gPMConstants.PMEReturnCode.PMTrue
            Status = gPMConstants.PMEReturnCode.PMOk
            Me.Hide()
        End If
    End Sub
    ''' <summary>
    ''' PerformAllocation
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Perform the Allocation on the selection</remarks>
    Private Function PerformAllocation() As Integer
        Const kMethodName As String = "PerformAllocation"

        Dim nResult As Integer = 0
        Dim bACTAllocationManual As Object

        Dim vMatchTrans As Object
        Dim vKeys As Object
        Dim nMainRow As Integer
        Dim nMatchRow As Integer
        Dim lWriteOffReasonID As Integer

        Dim oAllocationManual As bACTAllocationManual.Business
        Dim vTransdetails(,) As Object
        Dim bFound As Boolean
        'developer guide no.33
        Dim vValue As Object
        Dim bCredits As Boolean
        Dim bDebits As Boolean
        Dim bHasOtherTransaction As Boolean
        Dim lCashListCount As Long
        Dim bIsSingleCashListItemAllocation As Boolean
        Dim sOptionValue As String

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=kSysOptSingleCashReceipt, r_sOptionValue:=sOptionValue)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError("PerformAllocation", "GetSystemOption Single Cash Reciept/Payment per Allocation Check Failed", vbObjectError)
            End If

            If sOptionValue = "1" Then
                bIsSingleCashListItemAllocation = True
            Else
                bIsSingleCashListItemAllocation = False
            End If

            If m_bMultipleWriteOff Then
                MessageBox.Show("You can only write-off against one single transaction.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lMaxRow = 0 Then
                MessageBox.Show("You must include more than one single transaction.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_cWOTotal <> 0 And m_cCurrDiff <> 0 Then
                MessageBox.Show("You can only write-off or post a currency difference but not both.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Math.Abs(m_cBalance) >= 0.01 Then
                MessageBox.Show("The amounts do not match - Amounts must balance when part paying.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If (m_cWOTotal + m_cCurrDiff) <> 0 Then
                If ProcessWriteOff(lWriteOffReasonID) = gPMConstants.PMEReturnCode.PMFalse Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Warn about over-allocation
            For lRow As Integer = 0 To AllocationArray.GetUpperBound(0)
                If Math.Abs(ToSafeDouble(AllocationArray(lRow, gACTLibrary.k_ACBaseAllocated))) > Math.Abs(ToSafeDouble(AllocationArray(lRow, gACTLibrary.k_ACBaseOutstanding))) Then
                    MessageBox.Show("Be aware that you are over-allocating a transaction. The allocated amount is greater than the outstanding amount.", "Warning: Over-Allocation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit For
                End If
            Next lRow

            For lRow As Integer = 0 To AllocationArray.GetUpperBound(0)
                If AllocationArray(lRow, gACTLibrary.k_ACDocument_Ref).ToString.StartsWith("I") Then
                    If CheckIsLinkedToThirdPartyScheme(AllocationArray(lRow, gACTLibrary.k_ACDocument_Ref).ToString) = False Then
                        MessageBox.Show("Allocations are not permitted against Instalment Transactions.", "Warning: Invalid Document Ref", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            Next lRow
            For lRow As Integer = 0 To AllocationArray.GetUpperBound(0)
                If AllocationArray(lRow, gACTLibrary.k_ACTransactionAmount) < 0 Then
                    bCredits = True
                Else
                    bDebits = True
                End If
            Next lRow

            If Not bCredits Or Not bDebits Then
                MsgBox("You must select a mix of credits and debits.",
                        vbCritical, "Incorrect Selection")
                Return gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            'You must fully match all transaction lines for an individual document reference on insurer accounts.
            'Except if this product option is set
            iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTAllowPartialAllocationOnInsurer, 1, vValue)

            If gPMFunctions.ToSafeInteger(vValue, 0) <> 1 Then

                If m_oBusiness.IsInsurer(AccountID) Then
                    For lRow As Integer = 0 To AllocationArray.GetUpperBound(0)

                        m_lReturn = m_oBusiness.GetAllTransdetails(AllocationArray(lRow, gACTLibrary.k_ACTransDetail_id), vTransdetails)

                        For lLoopA As Integer = vTransdetails.GetLowerBound(1) To vTransdetails.GetUpperBound(1)
                            bFound = False
                            For lLoopB As Integer = 0 To AllocationArray.GetUpperBound(0)


                                If CInt(vTransdetails(0, lLoopA)) = CDbl(AllocationArray(lLoopB, gACTLibrary.k_ACTransDetail_id)) And CDec(vTransdetails(1, lLoopA)) = Conversion.Val(ToSafeString(AllocationArray(lLoopB, gACTLibrary.k_ACBaseAllocated))) + Conversion.Val(ToSafeString(AllocationArray(lLoopB, gACTLibrary.k_ACWriteOff))) + Conversion.Val(ToSafeString(AllocationArray(lLoopB, gACTLibrary.k_ACCurrencyDifference))) Then
                                    bFound = True
                                    Exit For
                                End If
                            Next
                            If Not bFound Then
                                Exit For
                            End If
                        Next

                        If Not bFound Then
                            Exit For
                        End If
                    Next lRow

                    If Not bFound Then
                        MessageBox.Show("You must fully match all transaction lines for an individual document reference on insurer accounts.", "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Return gPMConstants.PMEReturnCode.PMTrue
                    End If
                End If
            End If

            For lRow As Integer = 0 To AllocationArray.GetUpperBound(0)
                If ((UCase(Strings.Left(AllocationArray(lRow, k_ACDocument_Ref), 3)) = "SRP") Or (UCase(Strings.Left(AllocationArray(lRow, k_ACDocument_Ref), 3)) = "SPY")) Then
                    lCashListCount = lCashListCount + 1
                Else
                    bHasOtherTransaction = True
                End If
            Next lRow
            If ((bHasOtherTransaction = True) And (lCashListCount > 1)) And bIsSingleCashListItemAllocation = True Then
                MsgBox("you can allocate only 1 SRP and 1 SPY in a single allocation with other transaction types", vbOKOnly)
                PerformAllocation = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If MessageBox.Show("Are you sure you want to go ahead with the allocation?", "Perform Allocation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.No Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ReDim vMatchTrans(AllocationArray.GetUpperBound(0) - 1)
            'KB PN 7129 extra key for multi-currency
            ReDim vKeys(1, 6) 'eck220904

            'If we have a write-off row then this is treated as the main transaction
            If m_lWriteOffRow >= 0 Then
                nMainRow = m_lWriteOffRow
            Else
                nMainRow = m_lFirstCreditRow
            End If

            'Build the allocation data
            nMatchRow = 0
            For lRow As Integer = 0 To AllocationArray.GetUpperBound(0)
                If lRow = nMainRow Then
                    ' AllocationTransID

                    vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID

                    vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = CStr(AllocationArray(lRow, gACTLibrary.k_ACTransDetail_id)) & "|" &
                                   Conversion.Val(CStr(AllocationArray(lRow, gACTLibrary.k_ACBaseAllocated)))
                Else

                    vMatchTrans(nMatchRow) = CStr(AllocationArray(lRow, gACTLibrary.k_ACTransDetail_id)) & "|" &
                                            Conversion.Val(CStr(AllocationArray(lRow, gACTLibrary.k_ACBaseAllocated)))
                    nMatchRow += 1
                End If
            Next lRow

            ' AccountID

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = AccountID

            ' CashListItemID

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs


            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vMatchTrans

            ' Write Off Reason

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameWriteOffReasonId

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = lWriteOffReasonID

            ' Write Off Amount

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameWriteOffAmount

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = Math.Round(m_cWOTotal, 2)

            'KB PN 7129
            'Currency difference

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.ACTKeyNameCurrencyDifference

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_cCurrDiff

            'eck220904
            'Pass cash list item Id

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.ACTKeyNameCashListItemId

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = CashListItemID
            'eck220904End

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Create a new instance of the Insurer Payment Allocation
            Dim temp_oAllocationManual As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oAllocationManual, "bACTAllocationManual.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oAllocationManual = temp_oAllocationManual

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return nResult
            End If


            m_lReturn = oAllocationManual.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            ' Set the keys

            m_lReturn = oAllocationManual.SetKeys(vKeyArray:=vKeys)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set navigator keys.", vApp:=ACApp, vClass:=ACClass, vMethod:="PerformAllocation", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return nResult
            End If

            ' Start it
            'eck140501 pass correct company

            oAllocationManual.CompanyId = CompanyID
            '

            m_lReturn = oAllocationManual.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                If oAllocationManual.IsValidWriteOffAccount Then
                    MsgBox("This write off is not permitted as it relates to a currency exchange difference. The exchange rate gains/ losses ledgers should be used instead.", MsgBoxStyle.Critical, "Account Write Off")
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start Allocation Manual.", vApp:=ACApp, vClass:=ACClass, vMethod:="PerformAllocation", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return nResult
            End If


            oAllocationManual.Dispose()


            oAllocationManual = Nothing
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            nResult = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " function failed",
                               vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, sUsername:=g_oObjectManager.UserName, excep:=ex)


            ' If you want to rollback a transaction or something, do it here
        Finally

        End Try
        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: ProcessWriteOff
    '
    ' Description: Process WriteOff
    '
    ' ***************************************************************** '
    Private Function ProcessWriteOff(ByRef lWriteOffReasonID As Integer) As Integer
        Dim result As Integer = 0
        Dim sMsg As String = ""
        Dim vWriteOffValid As Boolean
        Dim cAuthorityAmount As Decimal
        Dim sCurrency As String = ""
        Dim iBaseCurrencyID As Integer
        'developer guide no.101
        Dim vUnderwritingOrAgency As Object
        Dim cAmount As Decimal
        Dim bIsCurrencyDiff As Boolean = False

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.getUnderwritingOrAgency(vUnderwritingOrAgency)

            'DC180305 : PN19114 : change processing of write off reason

            'DC110405 : PN20037 : wrong check to see if no reason selected
            If cboWriteOffReasonId.SelectedIndex = 0 Then
                MessageBox.Show("You must choose a valid Write Off Reason.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                cboWriteOffReasonId.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                'DC180305 : PN19114 : change processing of write off reason
                'lWriteOffReasonID = cboWriteOffReasonId.ItemId
                lWriteOffReasonID = cboWriteOffReasonId.SelectedIndex
            End If


            cAmount = m_cWOTotal + m_cCurrDiff

            If m_cCurrDiff <> 0 Then
                bIsCurrencyDiff = True
            End If

            If cAmount <> 0 Then

                If m_oUserAuthorities Is Nothing Then
                    Dim temp_m_oUserAuthorities As Object
                    m_lReturn = g_oObjectManager.GetInstance(temp_m_oUserAuthorities, "bACTUserAuthorities.Business", vInstanceManager:="ClientManager")
                    m_oUserAuthorities = temp_m_oUserAuthorities
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
                If m_oCurrencyConvert Is Nothing Then
                    Dim temp_m_oCurrencyConvert As Object
                    m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:="ClientManager")
                    m_oCurrencyConvert = temp_m_oCurrencyConvert
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If


                m_lReturn = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyID:=g_iSourceID, r_iBaseCurrencyID:=iBaseCurrencyID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get the user's write off amount

                m_lReturn = m_oUserAuthorities.ValidateAmounts(v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=cAmount, v_lCompanyID:=g_iSourceID, r_vWriteOffValid:=vWriteOffValid, r_cAuthorityAmount:=cAuthorityAmount, r_sCurrency:=sCurrency, bIsCurrencyDiff:=bIsCurrencyDiff)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get write off amount for user " & g_sUsername, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdMatch", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    vWriteOffValid = False
                End If

                'Not able to write off
                If Not vWriteOffValid Then

                    result = gPMConstants.PMEReturnCode.PMFalse
                    If bIsCurrencyDiff = True Then
                        sMsg = "Your currency limit does not allow you to adjust the currency difference amount." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                           "Currency Difference : " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(cAmount)) & Environment.NewLine &
                           "Your Currency limit : " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(cAuthorityAmount)) & " (" & sCurrency.Trim() & ")"
                    Else
                        sMsg = "Your write off limit does not allow you to write off the difference." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                          "Difference : " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(cAmount)) & Environment.NewLine &
                          "Your write off limit : " & gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(cAuthorityAmount)) & " (" & sCurrency.Trim() & ")"
                    End If
                    MessageBox.Show(sMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return result

                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process write off", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessWriteOff", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    'eck240102End

    Private Sub cmdRemove_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRemove.Click

        'DC220305 : PN19710 :
        '  Dim cDiff As Decimal


        If CallingAppName = "iACTCashListItem" And tdbGrid.CurrentRowIndex = 0 Then
            MessageBox.Show("You cannot remove the Receipt/Payment entry from the allocation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        ElseIf tdbGrid.CurrentRowIndex >= 0 Then
            RemoveHandler tdbGrid.CellLeave, AddressOf tdbGrid_CellLeave
            RemoveHandler tdbGrid.CellEnter, AddressOf tdbGrid_CellEnter
            tdbGrid.DeleteCurrentRow()
            AddHandler tdbGrid.CellLeave, AddressOf tdbGrid_CellLeave
            AddHandler tdbGrid.CellEnter, AddressOf tdbGrid_CellEnter

            CopyArray(AllocationArray, m_oGridArray, False, True)
            AutoCalculateAllocation()

            'DC040105 : PN19710 : calculate if there is a currency exchange difference

            CopyArray(AllocationArray, m_oGridArray, True, False)
            tdbGrid.ReBind()
            tdbGrid.Refresh()
            CalculateTotals()
        End If
    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            'TODOLIST: 
            'If CallingAppName = "iACTCashListItem" Then
            '    'This DoEvents is very important as VB won't be able to cleanly
            '    'handle the refreshing events on the iACTFindTransaction form
            '    'without it.
            '    Application.DoEvents()
            '    cmdAdd_Click(cmdAdd, New EventArgs())
            'End If
        End If
    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load


        'DC180305 : PN19114 : get list of write off reasons and add to combo box

        m_lReturn = m_oBusiness.GetWriteOffReasons(v_vWriteOffReasons:=m_vWriteOffReasons)

        m_lCurrencyExchangeDiffId = 0

        cboWriteOffReasonId.Items.Clear()
        Dim cboWriteOffReasonId_NewIndex As Integer = -1
        cboWriteOffReasonId_NewIndex = cboWriteOffReasonId.Items.Add("n/a")
        VB6.SetItemData(cboWriteOffReasonId, cboWriteOffReasonId_NewIndex, 0)

        'set to write off reasons list
        For lRow As Integer = 0 To m_vWriteOffReasons.GetUpperBound(1)

            cboWriteOffReasonId_NewIndex = cboWriteOffReasonId.Items.Add(CStr(m_vWriteOffReasons(2, lRow)))
            VB6.SetItemData(cboWriteOffReasonId, cboWriteOffReasonId_NewIndex, CInt(m_vWriteOffReasons(0, lRow)))

            'set the id for the currency exchange difference
            If CStr(m_vWriteOffReasons(1, lRow)).Trim() = "CurExcDiff" Then
                m_lCurrencyExchangeDiffId = CInt(m_vWriteOffReasons(0, lRow))
            Else
                cboWriteOffReasonId.SelectedIndex = 0
            End If

        Next lRow

        'KB 1/10/2003 PN 7129 Incorporate currency difference
        SetupGrid()
        ReturnValue = gPMConstants.PMEReturnCode.PMTrue
        ErrorNumber = gPMConstants.PMEReturnCode.PMTrue

        'This allows the highlight to move to the first allocated column/row
        'for immediate editing by the user. Setting the col property does not
        'work for grids with a split
        SendKeys.Send("{right}")
        'TODOLIST :Copied from Activated event
        If CallingAppName = "iACTCashListItem" Then
            'This DoEvents is very important as VB won't be able to cleanly
            'handle the refreshing events on the iACTFindTransaction form
            'without it.
            Application.DoEvents()
            cmdAdd_Click(cmdAdd, New EventArgs())
        End If
    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
        Dim vTransdetails As Object

        'Build the lock array
        ReDim vTransdetails(0, AllocationArray.GetUpperBound(0))
        For lRow As Integer = 0 To AllocationArray.GetUpperBound(0)

            vTransdetails(0, lRow) = AllocationArray(lRow, gACTLibrary.k_ACTransDetail_id)
        Next lRow

        'Unlock the Transactions
        m_lReturn = UnLockTransdetailId(v_vLockedTransDetailIds:=vTransdetails)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="UnLockTransdetailId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Unload")
        End If
        eventArgs.Cancel = Cancel <> 0
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: Form_Resize
        ' PURPOSE: Resize the elements on the form to ensure alignment.
        ' AUTHOR: Danny Davis
        ' DATE: 20 April 2004, 11:51:11
        ' CHANGES:
        ' ---------------------------------------------------------------------------


        Try

            Dim iCols As Integer
            Dim lDividerWidth, lSumWidth, lTotalWidth As Integer

            lDividerWidth = 0
            lSumWidth = 0
            lTotalWidth = 0

            'Work out where the divider is


            'Commented the present code, new code available below
            'START
            'todolist
            'iCols = tdbGrid.Splits(1).LeftCol - 1
            'For iCol As Integer = 0 To iCols

            'lDividerWidth = CInt(lDividerWidth + tdbGrid.Splits(0).Columns(iCol).Width)
            'Next iCol

            'Work out the total width of all the columns
            'lTotalWidth = lDividerWidth


            'todolist
            'For iCol As Integer = iCols + 1 To tdbGrid.Splits(1).Columns.Count - 1

            '    lTotalWidth = CInt(lTotalWidth + tdbGrid.Splits(1).Columns(iCol).Width)
            'Next iCol

            'Work out the position of the first column to be summed

            'lSumWidth = CInt(lDividerWidth + tdbGrid.Splits(1).Columns(gACTLibrary.k_ACBaseCurrency).Width)

            'Size the form width to fit everything on
            'Me.Width = VB6.TwipsToPixelsX(tdbGrid.Left + lTotalWidth + 250)

            'Size the grid to show all columns
            'tdbGrid.Width = lTotalWidth

            'Position buttons
            'cmdCancel.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - VB6.PixelsToTwipsX(cmdCancel.Width) - 150)
            'cmdOK.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdCancel.Left) - VB6.PixelsToTwipsX(cmdOK.Width) - 100)
            'cmdCancel.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6.PixelsToTwipsY(cmdOK.Height) - 100)
            'cmdOK.Top = cmdCancel.Top

            'Position bottom line

            'todolist
            'lineBottom.Y1 = VB6.PixelsToTwipsY(cmdCancel.Top) - 150


            'todolist
            'lineBottom.Y2 = lineBottom.Y1


            'todolist
            'lineBottom.X2 = VB6.PixelsToTwipsX(Me.Width) - lineBottom.X1 - 150

            'Position combo

            'todolist
            'cboWriteOffReasonId.Top = VB6.TwipsToPixelsY(lineBottom.Y1 - VB6.PixelsToTwipsY(cboWriteOffReasonId.Height) - 180)

            'todolist
            'lblWriteOffReasonID.Top = VB6.TwipsToPixelsY(lineBottom.Y1 - VB6.PixelsToTwipsY(lblWriteOffReasonID.Height) - 230)

            'Size Grid
            'tdbGrid.Height = VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - (VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6.PixelsToTwipsY(lblWriteOffReasonID.Top)) - 1500

            'Add/Remove Buttons
            'cmdAdd.Top = VB6.TwipsToPixelsY(tdbGrid.Height + tdbGrid.Top + 100)
            'cmdRemove.Top = VB6.TwipsToPixelsY(tdbGrid.Height + tdbGrid.Top + 100)

            'Total fields
            'lblOSTotal.Top = VB6.TwipsToPixelsY(tdbGrid.Height + tdbGrid.Top + 100)

            'DC180305 : PN19114 : always show multicurrecy for broking
            'If m_bMultiCurrency Then
            '    lblOSTotal.Left = VB6.TwipsToPixelsX(lSumWidth + tdbGrid.Left - 150)
            'Else
            '    lblOSTotal.Left = VB6.TwipsToPixelsX(lSumWidth + tdbGrid.Left - 250)
            'End If



            'lblOSTotal.Width = tdbGrid.Splits(1).Columns(gACTLibrary.k_ACBaseOutstanding).Width

            'lblAllocatedTotal.Top = lblOSTotal.Top
            'lblAllocatedTotal.Left = lblOSTotal.Left + lblOSTotal.Width

            'lblAllocatedTotal.Width = tdbGrid.Splits(1).Columns(gACTLibrary.k_ACBaseAllocated).Width

            'lblWriteOffTotal.Top = lblAllocatedTotal.Top
            'lblWriteOffTotal.Left = lblAllocatedTotal.Left + lblAllocatedTotal.Width

            'lblWriteOffTotal.Width = tdbGrid.Splits(1).Columns(gACTLibrary.k_ACWriteOff).Width

            'lblCurrencyTotal.Top = lblWriteOffTotal.Top
            'lblCurrencyTotal.Left = lblWriteOffTotal.Left + lblWriteOffTotal.Width

            'lblCurrencyTotal.Width = tdbGrid.Splits(1).Columns(gACTLibrary.k_ACCurrencyDifference).Width

            'lblTotals.Top = lblOSTotal.Top + VB6.TwipsToPixelsY(20)
            'lblTotals.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lblOSTotal.Left) - VB6.PixelsToTwipsX(lblTotals.Width) - 50)

            'lblBal.Top = lblWriteOffReasonID.Top - VB6.TwipsToPixelsY(500)
            'lblBal.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lblWriteOffTotal.Left) - VB6.PixelsToTwipsX(lblBal.Width) - 50)
            'lblBalance.Top = lblBal.Top - VB6.TwipsToPixelsY(30)
            'lblBalance.Left = lblWriteOffTotal.Left

            'DC180305 : PN19114 : increase length of write off reason display
            'cboWriteOffReasonId.Left = lblAllocatedTotal.Left
            'cboWriteOffReasonId.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lblAllocatedTotal.Width) + VB6.PixelsToTwipsX(lblWriteOffTotal.Width) + 20)
            'lblWriteOffReasonID.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cboWriteOffReasonId.Left) - VB6.PixelsToTwipsX(lblWriteOffReasonID.Width) - 50)


            'Tidy up the last bits
            'DC180305 : PN19114 : always show multicurrecy for broking
            'lblTransactionCurrency.Visible = False
            'lblBaseCurrency.Left = tdbGrid.Left

            'todolist
            'Line2.X1 = VB6.PixelsToTwipsX(lblBaseCurrency.Left) + VB6.PixelsToTwipsX(lblBaseCurrency.Width) + 50

            'todolist
            'Line2.X2 = tdbGrid.Left + tdbGrid.Width
            'END

            '******MODIFIED CODE START
            Dim visCount As Integer = 0
            Dim i As Integer
            For i = 0 To tdbGrid.Columns.Count - 1
                If (tdbGrid.Columns(i).Visible = True) Then
                    visCount = visCount + 1
                End If
            Next
            iCols = visCount
            For iCol As Integer = 0 To iCols

                lDividerWidth = CInt(lDividerWidth + tdbGrid.Columns(iCol).Width)
            Next iCol

            'Work out the total width of all the columns
            lTotalWidth = lDividerWidth



            For iCol As Integer = iCols + 1 To tdbGrid.Columns.Count - 1

                lTotalWidth = CInt(lTotalWidth + tdbGrid.Columns(iCol).Width)
            Next iCol

            'Work out the position of the first column to be summed

            lSumWidth = CInt(lDividerWidth + tdbGrid.Columns(gACTLibrary.k_ACBaseCurrency).Width)

            'Size the form width to fit everything on
            RemoveHandler Resize, AddressOf frmInterface_Resize
            Me.Width = tdbGrid.Left + lTotalWidth + VB6.TwipsToPixelsX(250)
            AddHandler Resize, AddressOf frmInterface_Resize

            'Size the grid to show all columns
            tdbGrid.Width = lTotalWidth

            'Position buttons
            cmdCancel.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - VB6.PixelsToTwipsX(cmdCancel.Width) - 150)
            cmdOK.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdCancel.Left) - VB6.PixelsToTwipsX(cmdOK.Width) - 100)
            cmdCancel.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6.PixelsToTwipsY(cmdOK.Height) - 100)
            cmdOK.Top = cmdCancel.Top

            'Position bottom line

            'todolist
            'lineBottom.Y1 = VB6.PixelsToTwipsY(cmdCancel.Top) - 150
            lineBottom.Location = New Point(lineBottom.Location.X, cmdCancel.Top - VB6.TwipsToPixelsY(150))



            'lineBottom.Y2 = lineBottom.Y1


            'todolist
            'lineBottom.X2 = VB6.PixelsToTwipsX(Me.Width) - lineBottom.X1 - 150
            lineBottom.Width = Me.Width - lineBottom.Location.X - VB6.TwipsToPixelsX(150)

            'Position combo

            'todolist
            'cboWriteOffReasonId.Top = VB6.TwipsToPixelsY(lineBottom.Y1 - VB6.PixelsToTwipsY(cboWriteOffReasonId.Height) - 180)
            cboWriteOffReasonId.Top = (cmdCancel.Top - VB6.TwipsToPixelsY(150)) - cboWriteOffReasonId.Height - VB6.TwipsToPixelsY(180)

            'todolist
            'lblWriteOffReasonID.Top = VB6.TwipsToPixelsY(lineBottom.Y1 - VB6.PixelsToTwipsY(lblWriteOffReasonID.Height) - 230)
            lblWriteOffReasonID.Top = (cmdCancel.Top - VB6.TwipsToPixelsY(150)) - lblWriteOffReasonID.Height - VB6.TwipsToPixelsY(230)

            'Size Grid
            tdbGrid.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - (VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6.PixelsToTwipsY(lblWriteOffReasonID.Top)) - 1500)

            'Add/Remove Buttons
            cmdAdd.Top = tdbGrid.Height + tdbGrid.Top + VB6.TwipsToPixelsY(100)
            cmdRemove.Top = tdbGrid.Height + tdbGrid.Top + VB6.TwipsToPixelsY(100)

            'Total fields
            lblOSTotal.Top = tdbGrid.Height + tdbGrid.Top + VB6.TwipsToPixelsY(100)

            'DC180305 : PN19114 : always show multicurrecy for broking
            If m_bMultiCurrency Then
                lblOSTotal.Left = lSumWidth + tdbGrid.Left - VB6.TwipsToPixelsX(4500)
            Else
                lblOSTotal.Left = lSumWidth + tdbGrid.Left - VB6.TwipsToPixelsX(250)
            End If



            lblOSTotal.Width = tdbGrid.Columns(gACTLibrary.k_ACBaseOutstanding).Width

            lblAllocatedTotal.Top = lblOSTotal.Top
            lblAllocatedTotal.Left = lblOSTotal.Left + lblOSTotal.Width

            lblAllocatedTotal.Width = tdbGrid.Columns(gACTLibrary.k_ACBaseAllocated).Width

            lblWriteOffTotal.Top = lblAllocatedTotal.Top
            lblWriteOffTotal.Left = lblAllocatedTotal.Left + lblAllocatedTotal.Width

            lblWriteOffTotal.Width = tdbGrid.Columns(gACTLibrary.k_ACWriteOff).Width

            lblCurrencyTotal.Top = lblWriteOffTotal.Top
            lblCurrencyTotal.Left = lblWriteOffTotal.Left + lblWriteOffTotal.Width

            lblCurrencyTotal.Width = tdbGrid.Columns(gACTLibrary.k_ACCurrencyDifference).Width

            lblTotals.Top = lblOSTotal.Top + VB6.TwipsToPixelsY(20)
            lblTotals.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lblOSTotal.Left) - VB6.PixelsToTwipsX(lblTotals.Width) - 50)

            lblBal.Top = lblWriteOffReasonID.Top - VB6.TwipsToPixelsY(500)
            lblBal.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lblWriteOffTotal.Left) - VB6.PixelsToTwipsX(lblBal.Width) - 50)
            lblBalance.Top = lblBal.Top - VB6.TwipsToPixelsY(30)
            lblBalance.Left = lblWriteOffTotal.Left

            'DC180305 : PN19114 : increase length of write off reason display
            cboWriteOffReasonId.Left = lblAllocatedTotal.Left
            cboWriteOffReasonId.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lblAllocatedTotal.Width) + VB6.PixelsToTwipsX(lblWriteOffTotal.Width) + 20)
            lblWriteOffReasonID.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cboWriteOffReasonId.Left) - VB6.PixelsToTwipsX(lblWriteOffReasonID.Width) - 50)


            'Tidy up the last bits
            'DC180305 : PN19114 : always show multicurrecy for broking
            lblTransactionCurrency.Visible = False
            lblBaseCurrency.Left = tdbGrid.Left

            'todolist
            'Line2.X1 = VB6.PixelsToTwipsX(lblBaseCurrency.Left) + VB6.PixelsToTwipsX(lblBaseCurrency.Width) + 50
            Line2.Location = New Point(lblBaseCurrency.Left + lblBaseCurrency.Width + VB6.TwipsToPixelsX(50), Line2.Location.Y)

            'todolist
            'Line2.X2 = tdbGrid.Left + tdbGrid.Width
            Line2.Width = tdbGrid.Left + tdbGrid.Width - (lblBaseCurrency.Left + lblBaseCurrency.Width + VB6.TwipsToPixelsX(50))
            'END





            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Resize", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Exit Sub
            End Select

        Finally



        End Try
        Exit Sub
    End Sub


    Private Sub Form_Terminate_Renamed()
        If Not (m_oUserAuthorities Is Nothing) Then

            m_oUserAuthorities.Dispose()
            m_oUserAuthorities = Nothing
        End If
        If Not (m_oCurrencyConvert Is Nothing) Then

            m_oCurrencyConvert.Dispose()
            m_oCurrencyConvert = Nothing
        End If
    End Sub

    'PN 28452 WriteOffReason is mandatory in case Write off amt is entered

    Private Sub lblWriteOffTotal_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lblWriteOffTotal.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        If CallingAppName = "iACTCashListItem" Then
            lblWriteOffReasonID.Font = VB6.FontChangeBold(lblWriteOffReasonID.Font, False)
            Dim dbNumericTemp As Double
            If Double.TryParse(lblWriteOffTotal.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                If Conversion.Val(lblWriteOffTotal.Text) <> 0 Then
                    lblWriteOffReasonID.Font = VB6.FontChangeBold(lblWriteOffReasonID.Font, True)
                End If
            End If
        End If
    End Sub

    Private Sub tdbGrid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles tdbGrid.CellEndEdit
        Dim val As Decimal
        If e.ColumnIndex = 10 Then
            If Decimal.TryParse(tdbGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value, val) Then
                tdbGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Decimal.Round(val, 2)
            End If

        End If
    End Sub




    Private Sub tdbGrid_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles tdbGrid.CellLeave
        If tdbGrid.CurrentCell.ReadOnly = False Then
            If Not sender.EditingControl Is Nothing Then
                tdbGrid.CurrentCell.Value = sender.EditingControl.EditingControlFormattedValue
                tdbGrid.Update()
            End If
        End If
        tdbGrid.Columns(gACTLibrary.k_ACWriteOff).ReadOnly = True
    End Sub

    Private Sub tdbGrid_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles tdbGrid.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If (KeyAscii < 48 Or KeyAscii > 57) And KeyAscii <> 46 And KeyAscii <> 45 And KeyAscii <> 44 And KeyAscii <> 8 Then
            KeyAscii = 0
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
        If tdbGrid.CurrentCell.ColumnIndex = gACTLibrary.k_ACWriteOff AndAlso tdbGrid.CurrentCell.ReadOnly = True Then
            If KeyAscii <> 8 Then
                tdbGrid.CurrentCell.Value = eventArgs.KeyChar
            Else
                tdbGrid.CurrentCell.Value = ""
            End If
            tdbGrid.RefetchCurrentRow()
            tdbGrid.CurrentCell.ReadOnly = False

            tdbGrid.SelStart = Len(tdbGrid.CurrentCell.Value) + 1
            tdbGrid.SelLength = 0
        End If
    End Sub

    Private Sub tdbGrid_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles tdbGrid.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        If KeyCode = 13 Then
            'Handle enter
            If Shift = 0 Then
                If tdbGrid.CurrentRowIndex < m_lMaxRow Then
                    tdbGrid.CurrentRowIndex += 1
                End If
            ElseIf Shift = 1 Then
                If tdbGrid.CurrentRowIndex > 0 Then
                    tdbGrid.CurrentRowIndex -= 1
                End If
            End If
            CalculateTotals()
            If Not IsNothing(tdbGrid.CurrentRow) Then
                tdbGrid.CurrentCell = tdbGrid.CurrentRow.Cells(m_iCol)
            End If
        ElseIf KeyCode = 9 Or KeyCode = 37 Or KeyCode = 39 Then
            'Handle tab or left/right array
            CalculateTotals()
            m_iCol = tdbGrid.CurrentCell.ColumnIndex
        ElseIf KeyCode = 46 Or KeyCode = 8 Then
            'Handle Delete and Backspace
            If tdbGrid.CurrentCell.ReadOnly = True Then
                eventArgs.Handled = True
            End If
            If tdbGrid.CurrentCell.ColumnIndex = gACTLibrary.k_ACWriteOff AndAlso tdbGrid.CurrentCell.ReadOnly = True Then
                tdbGrid.CurrentCell.Value = ""
                tdbGrid.RefetchCurrentRow()
            End If
        End If
    End Sub
    'Private Sub tdbGrid_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tdbGrid.LostFocus
    '	'Ensure the totals are refreshed
    '	tdbGrid.UpdateCurrentRow()
    'End Sub
    Private Sub tdbGrid_CellEnter(ByVal eventSender As Object, ByVal eventArgs As DataGridViewCellEventArgs) Handles tdbGrid.CellEnter
        Dim LastRow As Object = Nothing
        Dim LastCol As Integer = -1
        Dim cDiff As Decimal = 0.0
        Dim lRow As Integer
        'DC180305 : PN19114 : underwriting as was but for broking only allowed to click the column
        '                       if approriate
        'If tdbGrid.CurrentCell.ColumnIndex = gACTLibrary.k_ACWriteOff Or tdbGrid.CurrentCell.ColumnIndex = gACTLibrary.k_ACCurrencyDifference AndAlso bLedgerTypeAllowWriteOff Then

        '    lRow = eventArgs.RowIndex

        '    'todolist
        '    'lRow = CInt(tdbGrid.GetBookmark(0))

        '    'If we're in the Write Off column then auto-calculate
        '    cDiff = ToSafeDecimal(AllocationArray(lRow, gACTLibrary.k_ACBaseOutstanding)) - ToSafeDecimal(AllocationArray(lRow, gACTLibrary.k_ACBaseAllocated))
        '    If cDiff <> 0 Then
        '        If tdbGrid.CurrentCell.ColumnIndex = gACTLibrary.k_ACWriteOff Then
        '            AllocationArray(lRow, gACTLibrary.k_ACCurrencyDifference) = ""
        '            m_oGridArray(lRow, gACTLibrary.k_ACCurrencyDifference) = ""
        '        Else
        '            AllocationArray(lRow, gACTLibrary.k_ACWriteOff) = ""
        '            m_oGridArray(lRow, gACTLibrary.k_ACWriteOff) = ""
        '        End If

        '        AllocationArray(lRow, tdbGrid.CurrentCell.ColumnIndex) = cDiff
        '        m_oGridArray(lRow, tdbGrid.CurrentCell.ColumnIndex) = AllocationArray(lRow, tdbGrid.CurrentCell.ColumnIndex)

        '        tdbGrid.RefetchCurrentRow()
        '        CalculateTotals()
        '    End If
        'End If
        If Not IsNothing(tdbGrid.PreviousCell) Then
            If tdbGrid.PreviousCell.RowIndex > tdbGrid.Rows.Count Then
                'todolist
                'LastRow = tdbGrid.Rows(tdbGrid.PreviousCell.RowIndex)
                LastRow = tdbGrid.PreviousCell.RowIndex
            End If
            LastCol = tdbGrid.PreviousCell.ColumnIndex
        End If
        If tdbGrid.CurrentRowIndex >= 0 Then
            'developer guide no.188 & changed tdbgrid.row to tdbgrid.CurrentRowIndex (todolist)
            If tdbGrid.CurrentCell.ColumnIndex = gACTLibrary.k_ACCurrencyDifference And m_oGridArray(tdbGrid.CurrentRowIndex, k_ACTransactionCurrencyID) = m_oGridArray(tdbGrid.CurrentRowIndex, k_ACBaseCurrencyID) Then
                If Not IsNothing(tdbGrid.CurrentRow) Then
                    'tdbGrid.CurrentCell = tdbGrid.CurrentRow.Cells(gACTLibrary.k_ACWriteOff)
                    'tdbGrid.CurrentColumnIndex = gACTLibrary.k_ACWriteOff
                End If
            End If
            CalculateTotals()
            'PN31383
            If LastCol >= 0 Then

                m_lReturn = CheckWriteOffValue(CInt(eventArgs.RowIndex), LastCol)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If
            End If
        End If



    End Sub

    Private Function AutoCalculateAllocation() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AutoCalculateAllocation
        ' PURPOSE: Automatically works out the best allocation based on the selected
        '          records
        ' AUTHOR: Danny Davis
        ' DATE: 20 April 2004, 17:27:54
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lRows As Integer
            Dim iCol As Integer
            Dim cCROrigTotal, cDROrigTotal, cCRTotal, cDRTotal As Decimal
            Dim vArray(,) As Object
            Dim lNewCol1, lNewCol2, lLastCR, lLastDR As Integer


            vArray = VB6.CopyArray(AllocationArray)

            lNewCol1 = vArray.GetUpperBound(1) + 1

            lNewCol2 = vArray.GetUpperBound(1) + 2

            ReDim Preserve vArray(vArray.GetUpperBound(0), lNewCol2)


            lRows = vArray.GetUpperBound(0)

            'Retain the original row number
            For lRow As Integer = 0 To lRows

                vArray(lRow, lNewCol1) = lRow


                vArray(lRow, lNewCol2) = Math.Abs(CDbl(vArray(lRow, gACTLibrary.k_ACBaseOutstanding)))
            Next lRow

            'Sort into numerical order

            ShellSort2DArray(vArray, lNewCol2)

            'Calculate the total credit
            cCROrigTotal = 0
            cDROrigTotal = 0
            For lRow As Integer = 0 To lRows

                If CDbl(vArray(lRow, gACTLibrary.k_ACBaseOutstanding)) < 0 Then

                    cCROrigTotal += CDbl(vArray(lRow, gACTLibrary.k_ACBaseOutstanding))
                    lLastCR = lRow
                Else

                    cDROrigTotal += CDbl(vArray(lRow, gACTLibrary.k_ACBaseOutstanding))
                    lLastDR = lRow
                End If
            Next lRow

            ' TS374 - If total credit is less than total debt and SFU use new algorithm
            If Math.Abs(cCROrigTotal) < Math.Abs(cDROrigTotal) Then
                ' Pass in what we've already done

                AutoCalculatePartAllocation(vArray, cCROrigTotal, cDROrigTotal)
            Else
                'Work out the lowest of the two
                If Math.Abs(cCROrigTotal) < Math.Abs(cDROrigTotal) Then
                    cCRTotal = cCROrigTotal
                    cDRTotal = cCROrigTotal * -1
                Else
                    cCRTotal = cDROrigTotal * -1
                    cDRTotal = cDROrigTotal
                End If

                'Now assign the credits and debits
                For lRow As Integer = 0 To lRows
                    'Deal with credits

                    If CDbl(vArray(lRow, gACTLibrary.k_ACBaseOutstanding)) < 0 Then
                        'Is entry less than the total left
                        If vArray(lRow, gACTLibrary.k_ACBaseOutstanding) >= cCRTotal Then
                            'Allocate all


                            vArray(lRow, gACTLibrary.k_ACBaseAllocated) = vArray(lRow, gACTLibrary.k_ACBaseOutstanding)

                            cCRTotal = CDec(cCRTotal - CDbl(vArray(lRow, gACTLibrary.k_ACBaseAllocated)))
                            'Are we on the last row
                        ElseIf lRow = lLastCR Then

                            vArray(lRow, gACTLibrary.k_ACBaseAllocated) = cCRTotal
                            cCRTotal = 0
                        ElseIf cCRTotal <> 0 Then

                            vArray(lRow, gACTLibrary.k_ACBaseAllocated) = Math.Round(cCRTotal / 2, 2)

                            cCRTotal = CDec(cCRTotal - CDbl(vArray(lRow, gACTLibrary.k_ACBaseAllocated)))
                        End If
                        'Deal with debits
                    Else
                        'Is entry less than the total left
                        If vArray(lRow, gACTLibrary.k_ACBaseOutstanding) <= cDRTotal Then
                            'Allocate all


                            vArray(lRow, gACTLibrary.k_ACBaseAllocated) = vArray(lRow, gACTLibrary.k_ACBaseOutstanding)

                            cDRTotal = CDec(cDRTotal - CDbl(vArray(lRow, gACTLibrary.k_ACBaseAllocated)))
                            'Are we on the last row
                        ElseIf lRow = lLastDR Then

                            vArray(lRow, gACTLibrary.k_ACBaseAllocated) = cDRTotal
                            cDRTotal = 0
                        ElseIf cDRTotal <> 0 Then

                            vArray(lRow, gACTLibrary.k_ACBaseAllocated) = Math.Round(cDRTotal / 2, 2)

                            cDRTotal = CDec(cDRTotal - CDbl(vArray(lRow, gACTLibrary.k_ACBaseAllocated)))
                        End If
                    End If
                Next lRow
            End If

            'Copy the array back
            For lRow As Integer = 0 To lRows


                AllocationArray(CInt(vArray(lRow, lNewCol1)), gACTLibrary.k_ACBaseAllocated) = vArray(lRow, gACTLibrary.k_ACBaseAllocated)
            Next lRow



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCalculateAllocation", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


            End Select

        Finally



        End Try
        Return result
    End Function

    Private Function AutoCalculatePartAllocation(ByRef vArray(,) As Object, ByVal cCRTotal As Decimal, ByVal cDRTotal As Decimal) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AutoCalculatePartAllocation
        ' PURPOSE: Automatically works out the best allocation based on the selected
        '          records
        ' AUTHOR: Danny Davis
        ' DATE: 20 April 2004, 17:27:54
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lRows As Integer
            Dim iCol As Integer
            Dim lNewCol As Integer
            Dim cPartDebit As Decimal ' The total of all lines to part allocate
            Dim cPartCredit As Decimal ' The credit available to part allocated lines
            Dim lPartCredit As Integer ' The current part credit sequence
            Dim cBalance As Decimal ' The balance available to allocate

            lRows = vArray.GetUpperBound(0)
            lPartCredit = -1

            'Sort into numerical order
            ShellSortAllocationArray(vArray)

            'Now assign the credits and debits
            For lRow As Integer = 0 To lRows
                ' Check if line is credit or debit

                If CDbl(vArray(lRow, gACTLibrary.k_ACBaseOutstanding)) < 0 Then
                    ' This is where our credit comes from and it'll always be sorted first
                    ' so allocate it all and build up our credit total


                    vArray(lRow, gACTLibrary.k_ACBaseAllocated) = vArray(lRow, gACTLibrary.k_ACBaseOutstanding)

                    cBalance = CDec(cBalance - CDbl(vArray(lRow, gACTLibrary.k_ACBaseOutstanding)))

                    ' Store credit available to part lines (in case there are no full ones)
                    cPartCredit = cBalance
                Else
                    ' Note: We always have more debits than credits
                    ' Follow allocation rules

                    If gPMFunctions.ToSafeLong(CDbl(vArray(lRow, gACTLibrary.k_ACTAllocRule))) = 0 Then
                        'Is entry less than the balance
                        If vArray(lRow, gACTLibrary.k_ACBaseOutstanding) < cBalance Then
                            'Allocate all


                            vArray(lRow, gACTLibrary.k_ACBaseAllocated) = vArray(lRow, gACTLibrary.k_ACBaseOutstanding)

                            cBalance = CDec(cBalance - CDbl(vArray(lRow, gACTLibrary.k_ACBaseAllocated)))
                        Else
                            'Allocate remainder

                            vArray(lRow, gACTLibrary.k_ACBaseAllocated) = cBalance
                            cBalance = 0
                        End If

                        ' Set the amount left for part allocation to what's left
                        cPartCredit = cBalance
                    Else

                        If lPartCredit <> CDbl(vArray(lRow, gACTLibrary.k_ACTAllocSequence)) Then
                            ' Reset balance for new part credit

                            lPartCredit = CInt(vArray(lRow, gACTLibrary.k_ACTAllocSequence))
                            cPartDebit = 0

                            'Calculate the total debit on lines to partial allocate at this priority
                            For lPartCreditRow As Integer = 0 To lRows



                                If CDbl(vArray(lPartCreditRow, gACTLibrary.k_ACBaseOutstanding)) > 0 And CDbl(vArray(lPartCreditRow, gACTLibrary.k_ACTAllocRule)) = 1 And CDbl(vArray(lPartCreditRow, gACTLibrary.k_ACTAllocSequence)) = lPartCredit Then

                                    cPartDebit += CDbl(vArray(lPartCreditRow, gACTLibrary.k_ACBaseOutstanding))
                                End If
                            Next lPartCreditRow
                        End If

                        ' Is this the last line?
                        If lRow = lRows Then
                            'Is entry less than the balance .. it should be
                            If vArray(lRow, gACTLibrary.k_ACBaseOutstanding) < cBalance Then
                                'Allocate all


                                vArray(lRow, gACTLibrary.k_ACBaseAllocated) = vArray(lRow, gACTLibrary.k_ACBaseOutstanding)

                                cBalance = CDec(cBalance - CDbl(vArray(lRow, gACTLibrary.k_ACBaseAllocated)))
                            Else
                                'Allocate remainder

                                vArray(lRow, gACTLibrary.k_ACBaseAllocated) = cBalance
                                cBalance = 0
                            End If
                        Else
                            ' Calculate the partial allocation
                            If cPartDebit = 0 Then

                                vArray(lRow, gACTLibrary.k_ACBaseAllocated) = 0
                            Else
                                ' Allocate all part lines proportionally


                                vArray(lRow, gACTLibrary.k_ACBaseAllocated) = Math.Round((CDbl(vArray(lRow, gACTLibrary.k_ACBaseOutstanding)) / cPartDebit) * cPartCredit, 2)

                                cBalance = CDec(cBalance - CDbl(vArray(lRow, gACTLibrary.k_ACBaseAllocated)))
                            End If
                        End If
                    End If
                End If
            Next lRow



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoCalculatePartAllocation", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


            End Select

        Finally



        End Try
        Return result
    End Function

    Public Function ShellSort2DArray(ByRef r_vArray(,) As Object, ByVal v_iSortColumn As Integer, Optional ByVal v_sSortDirection As String = "ASCENDING") As Integer
        ' ************************************************************************ '
        ' Name: ShellSort
        '
        ' Description: Execute SHELL sort on the selected index of the 2D array that is provided.
        '
        ' Parameters:
        '
        ' r_vArray         = Variant 2D array to sort (column, row dimension positioning)
        ' v_iSortColumn    = Holds the index of the column to sort
        ' v_sSortDirection = Specifies the direction (ASCENDING or DESCENDING) to sort
        '
        ' History: 08/02/2000 CJB - Created.
        '
        ' ************************************************************************ '

        Dim result As Integer = 0
        Dim iNoOfColumns As Integer 'Total number of columns in the array
        Dim iNoOfRows As Integer 'Total number of rows in the array
        Dim iFirstRowNo As Integer 'Index of 1st row number
        Dim iLastRowNo As Integer 'Index of last row number
        'Holds current column currently processing
        Dim iCurrentRow As Integer 'Holds current row currently processing
        Dim iDistance As Integer 'Value used in sorting
        Dim iNextRow As Integer 'Holds next row to process
        'developer guide no.33
        Dim vTempStorage As Object 'Holds array element while swapping around
        Dim cDataValue1 As Decimal 'Holds value of string to compare with cDataValue2
        Dim cDataValue2 As Decimal 'Holds value of string to compare with cDataValue1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Find number of columns in the array
            iNoOfColumns = r_vArray.GetUpperBound(1)

            'Save the first row number
            iFirstRowNo = r_vArray.GetLowerBound(0)

            'Save the last row number
            iLastRowNo = r_vArray.GetUpperBound(0)

            'Find no. of rows to traverse
            iNoOfRows = iLastRowNo - iFirstRowNo + 1
            iDistance = 1

            While (iDistance <= iNoOfRows)
                iDistance = 2 * iDistance
            End While

            iDistance = CInt((iDistance / 2) - 1)

            Do While (iDistance > 0)
                iNextRow = iFirstRowNo + iDistance

                'While there are rows to process
                Do While (iNextRow <= iLastRowNo)
                    iCurrentRow = iNextRow
                    Do
                        If iCurrentRow >= (iFirstRowNo + iDistance) Then

                            'Prepare for actual compare of data value

                            cDataValue1 = CDec(r_vArray(iCurrentRow, v_iSortColumn))

                            cDataValue2 = CDec(r_vArray(iCurrentRow - iDistance, v_iSortColumn))

                            'Ascending sort
                            If v_sSortDirection = "ASCENDING" Then

                                'Do the comparison of data values - if unsorted then swap the two rows around
                                If cDataValue1 < cDataValue2 Then

                                    For iCurrentColumn As Integer = 0 To iNoOfColumns

                                        'developer guide no.33
                                        vTempStorage = r_vArray(iCurrentRow, iCurrentColumn)


                                        r_vArray(iCurrentRow, iCurrentColumn) = r_vArray(iCurrentRow - iDistance, iCurrentColumn)

                                        r_vArray(iCurrentRow - iDistance, iCurrentColumn) = vTempStorage
                                    Next
                                    iCurrentRow -= iDistance
                                Else
                                    Exit Do
                                End If
                            Else

                                'Descending sort
                                If v_sSortDirection = "DESCENDING" Then

                                    'Actual compare of data value - if unsorted then swap the two rows around
                                    If cDataValue1 >= cDataValue2 Then
                                        For iCurrentColumn As Integer = 0 To iNoOfColumns

                                            'developer guide no.33
                                            vTempStorage = r_vArray(iCurrentRow, iCurrentColumn)


                                            r_vArray(iCurrentRow, iCurrentColumn) = r_vArray(iCurrentRow - iDistance, iCurrentColumn)

                                            r_vArray(iCurrentRow - iDistance, iCurrentColumn) = vTempStorage
                                        Next
                                        iCurrentRow -= iDistance
                                    Else
                                        Exit Do
                                    End If
                                End If
                            End If
                        Else
                            Exit Do
                        End If
                    Loop
                    iNextRow += 1
                Loop
                iDistance = CInt((iDistance - 1) / 2)
            Loop

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShellSort2DArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShellSort2DArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Disables the writeoff controls for accounts that are not allowed to have writeoffs.
    Private Function AllowWriteOff() As Integer
        Dim result As Integer = 0
        Dim bACTAccount As Object

        Const kMethodName As String = "AllowWriteOff"


        Dim oAccount As bACTAccount.Form
        Dim lLedgerID As Integer
        Dim sLedgerTypeDesc As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oAccount As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oAccount, "bACTAccount.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oAccount = temp_oAccount
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "sClassName:=bACTAccount.Form", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = oAccount.GetAccountLedger(v_lAccountID:=AccountID, v_lLedgerID:=lLedgerID, v_sLedgerCode:=sLedgerTypeDesc)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oAccount.GetAccountLedger", "v_lAccountID:=" & AccountID, gPMConstants.PMELogLevel.PMLogError)
            End If

            Select Case sLedgerTypeDesc
                Case ACCOUNT_LEDGER_CLIENT, ACCOUNT_LEDGER_SUBAGENT, ACCOUNT_LEDGER_PURCHASE, ACCOUNT_LEDGER_AGENT, ACCOUNT_LEDGER_INSURER
                    'Only these accounts are allowed to have writeoffs
                    bLedgerTypeAllowWriteOff = True
                Case Else
                    'Not a writeoffable account slow disable writeoff controls.
                    lblWriteOffReasonID.Enabled = False
                    cboWriteOffReasonId.Enabled = False
                    bLedgerTypeAllowWriteOff = False
                    'tdbGrid.Splits(1).Columns.Item(gACTLibrary.k_ACWriteOff).AllowFocus = False

                    'tdbGrid.Splits(1).Columns(gACTLibrary.k_ACWriteOff).ReadOnly = True
            End Select




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here
            If Not (oAccount Is Nothing) Then

                oAccount.Dispose()
            End If
            oAccount = Nothing



            ' This is for debugging only



        End Try
        Return result
    End Function


    Public Function ShellSortAllocationArray(ByRef r_vArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim iNoOfColumns As Integer 'Total number of columns in the array
        Dim iNoOfRows As Integer 'Total number of rows in the array
        Dim iFirstRowNo As Integer 'Index of 1st row number
        Dim iLastRowNo As Integer 'Index of last row number
        'Holds current column currently processing
        Dim iCurrentRow As Integer 'Holds current row currently processing
        Dim iDistance As Integer 'Value used in sorting
        Dim iNextRow As Integer 'Holds next row to process
        Dim vTempStorage As Object  'Holds array element while swapping around
        Dim lDataValue1 As Integer 'Holds value to compare with lDataValue2
        Dim lDataValue2 As Integer 'Holds value to compare with lDataValue1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Find number of columns in the array
            iNoOfColumns = r_vArray.GetUpperBound(1)

            'Save the first row number
            iFirstRowNo = r_vArray.GetLowerBound(0)

            'Save the last row number
            iLastRowNo = r_vArray.GetUpperBound(0)

            'Ensure rule and sequence set appropriately
            For iCurrentRow = iFirstRowNo To iLastRowNo
                ' Unsupported or non-configured types are always treated as with premium
                Select Case r_vArray(iCurrentRow, gACTLibrary.k_ACTypeCode)
                    Case "TAX"
                        ' A configured type, just ensure we have a valid id


                        r_vArray(iCurrentRow, gACTLibrary.k_ACTAllocRule) = gPMFunctions.ToSafeInteger((r_vArray(iCurrentRow, gACTLibrary.k_ACTAllocRule)), 1)

                        ' Rule 1 is always at same sequence...

                        If CDbl(r_vArray(iCurrentRow, gACTLibrary.k_ACTAllocRule)) = 1 Then

                            r_vArray(iCurrentRow, gACTLibrary.k_ACTAllocSequence) = 1
                        Else


                            r_vArray(iCurrentRow, gACTLibrary.k_ACTAllocSequence) = gPMFunctions.ToSafeInteger((r_vArray(iCurrentRow, gACTLibrary.k_ACTAllocSequence)), 1)
                        End If
                    Case Else

                        r_vArray(iCurrentRow, gACTLibrary.k_ACTAllocRule) = 1

                        r_vArray(iCurrentRow, gACTLibrary.k_ACTAllocSequence) = 1
                End Select
            Next iCurrentRow

            'Find no. of rows to traverse
            iNoOfRows = iLastRowNo - iFirstRowNo + 1
            iDistance = 1

            Do While (iDistance <= iNoOfRows)
                iDistance = 2 * iDistance
            Loop

            iDistance = CInt((iDistance / 2) - 1)

            Do While (iDistance > 0)
                iNextRow = iFirstRowNo + iDistance

                'While there are rows to process
                Do While (iNextRow <= iLastRowNo)
                    iCurrentRow = iNextRow
                    Do
                        If iCurrentRow >= (iFirstRowNo + iDistance) Then
                            'Initial compare is on the credit/debit of the value, not the value

                            lDataValue1 = Math.Sign(CDbl(r_vArray(iCurrentRow, gACTLibrary.k_ACBaseOutstanding)))

                            lDataValue2 = Math.Sign(CDbl(r_vArray(iCurrentRow - iDistance, gACTLibrary.k_ACBaseOutstanding)))

                            ' If amount is same goto rule
                            If lDataValue1 = lDataValue2 Then

                                lDataValue1 = gPMFunctions.ToSafeLong((r_vArray(iCurrentRow, gACTLibrary.k_ACTAllocRule)), 1)

                                lDataValue2 = gPMFunctions.ToSafeLong((r_vArray(iCurrentRow - iDistance, gACTLibrary.k_ACTAllocRule)), 1)

                                ' If rule is same goto sequence
                                If lDataValue1 = lDataValue2 Then

                                    lDataValue1 = gPMFunctions.ToSafeLong((r_vArray(iCurrentRow, gACTLibrary.k_ACTAllocSequence)), 1)

                                    lDataValue2 = gPMFunctions.ToSafeLong((r_vArray(iCurrentRow - iDistance, gACTLibrary.k_ACTAllocSequence)), 1)
                                End If
                            End If

                            'Do the comparison of values - if unsorted then swap the two rows around
                            If lDataValue1 < lDataValue2 Then
                                For iCurrentColumn As Integer = 0 To iNoOfColumns

                                    vTempStorage = (r_vArray(iCurrentRow, iCurrentColumn))


                                    r_vArray(iCurrentRow, iCurrentColumn) = r_vArray(iCurrentRow - iDistance, iCurrentColumn)

                                    r_vArray(iCurrentRow - iDistance, iCurrentColumn) = vTempStorage
                                Next
                                iCurrentRow -= iDistance
                            Else
                                Exit Do
                            End If
                        Else
                            Exit Do
                        End If
                    Loop
                    iNextRow += 1
                Loop
                iDistance = CInt((iDistance - 1) / 2)
            Loop

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShellSortAllocationArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShellSortAllocationArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'PN31383
    Private Function CheckWriteOffValue(ByVal lgrow As Integer, ByVal lCol As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "AllowWriteOff"

        Dim cDiff As Decimal
        Dim lRow As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            'underwriting as was but for broking only allowed to click the column
            'if approriate
            If lCol = gACTLibrary.k_ACWriteOff Or lCol = gACTLibrary.k_ACCurrencyDifference Then

                lRow = lgrow

                'If we're in the Write Off column then auto-calculate
                cDiff = Conversion.Val(CStr(AllocationArray(lRow, gACTLibrary.k_ACBaseOutstanding))) - Conversion.Val(CStr(AllocationArray(lRow, gACTLibrary.k_ACBaseAllocated)))

                If cDiff = 0 Then
                    'developer guide no.248
                    If (gPMFunctions.ToSafeDouble(AllocationArray(lRow, lCol)) <> 0) And gPMFunctions.ToSafeString(AllocationArray(lRow, lCol)).Trim() <> "" Then
                        MessageBox.Show("Write Off is not allowed against a fully allocated line.", ACApp)
                        AllocationArray(lRow, lCol) = ""
                        m_oGridArray(lRow, lCol) = ""
                    End If
                    tdbGrid.RefetchCurrentRow()
                    CalculateTotals()
                End If
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

        End Try
        Return result
    End Function


    Private Sub tdbGrid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles tdbGrid.CellDoubleClick
        Dim LastRow As Object = Nothing
        Dim LastCol As Integer = -1
        Dim cDiff As Decimal = 0.0
        Dim lRow As Integer

        If tdbGrid.CurrentCell.ColumnIndex = gACTLibrary.k_ACWriteOff Or tdbGrid.CurrentCell.ColumnIndex = gACTLibrary.k_ACCurrencyDifference Then

            lRow = e.RowIndex

            'todolist
            'lRow = CInt(tdbGrid.GetBookmark(0))

            'If we're in the Write Off column then auto-calculate
            cDiff = gPMFunctions.ToSafeDecimal(AllocationArray(lRow, gACTLibrary.k_ACBaseOutstanding)) - gPMFunctions.ToSafeDecimal(AllocationArray(lRow, gACTLibrary.k_ACBaseAllocated))
            If cDiff <> 0 Then
                If tdbGrid.CurrentCell.ColumnIndex = gACTLibrary.k_ACWriteOff Then
                    AllocationArray(lRow, gACTLibrary.k_ACCurrencyDifference) = ""
                    m_oGridArray(lRow, gACTLibrary.k_ACCurrencyDifference) = ""
                Else
                    AllocationArray(lRow, gACTLibrary.k_ACWriteOff) = ""
                    m_oGridArray(lRow, gACTLibrary.k_ACWriteOff) = ""
                End If

                AllocationArray(lRow, tdbGrid.CurrentCell.ColumnIndex) = cDiff
                m_oGridArray(lRow, tdbGrid.CurrentCell.ColumnIndex) = AllocationArray(lRow, tdbGrid.CurrentCell.ColumnIndex)

                tdbGrid.RefetchCurrentRow()
                CalculateTotals()
            End If
            tdbGrid.CurrentCell.ReadOnly = False
        End If
    End Sub
End Class
