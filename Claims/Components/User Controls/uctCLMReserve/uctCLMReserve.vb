Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.Globalization
Imports System.IO
Imports System.Windows.Forms
'developer guide no 129. 
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("uctCLMReserve_NET.uctCLMReserve")> _
Partial Public Class uctCLMReserve
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    Public Event VisibleChange()
    Public Event EnabledChange()
    Public Event ShowCoInsurersChange()
    Public Event ShowEditChange()

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "uctCLMReserveControl"

    'Default Property Values:
    Private Const m_def_BackColor As Integer = 0
    Private Const m_def_ForeColor As Integer = 0
    Private Const m_def_Enabled As Boolean = True
    Private Const m_def_Visible As Boolean = True
    Private Const m_def_BackStyle As Integer = 0
    Private Const m_def_BorderStyle As Integer = 0
    Private Const m_def_ShowEdit As Boolean = True
    Private Const m_def_ShowCoInsurers As Boolean = False

    'Property Variables:
    Private m_BackColor As Integer
    Private m_ForeColor As Integer
    Private m_Enabled As Boolean
    Private m_Font As Font
    Private m_BackStyle As Integer
    Private m_BorderStyle As Integer
    Private m_ShowEdit As Boolean
    Private m_ShowCoInsurers As Boolean
    Private m_Visible As Boolean

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' column declarations for the listview control
    Private Const m_lstInitialReserve As Integer = 1
    Private Const m_lstRevisionAmount As Integer = 2
    Private Const m_lstThisRevision As Integer = 3
    Private Const m_lstCurrentReserve As Integer = 4
    Private Const m_lstIncurred As Integer = 5
    Private Const m_lstSumInsured As Integer = 6
    Private Const m_lstAverage As Integer = 7

    ' column declarations for the listviewarray
    Private Const m_colTag As Integer = 0
    Private Const m_colDescription As Integer = 1
    Private Const m_colInitial As Integer = 2
    Private Const m_colRevisionAmount As Integer = 3
    Private Const m_colThisRevision As Integer = 4
    Private Const m_colCurrentReserve As Integer = 5
    Private Const m_colIncurred As Integer = 6
    Private Const m_colSumInsured As Integer = 7
    Private Const m_colAverage As Integer = 8

    ' Variables
    Private m_lReturn As gPMConstants.PMEReturnCode

    'Event Declarations:

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    Private m_oFormFields As iPMFormControl.FormFields

    Private m_vReserveDetailsArray(,) As Object
    Private m_ListViewArray(,) As Object
    Private m_lstItem As ListViewItem
    Private m_bAllowNegativeReserve As Boolean

    'private variables
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lPerilID As Integer
    Private m_lClaimID As Integer
    Private m_lPerilTypeID As Integer
    Private m_sRisktype As String = ""
    Private m_lPartycnt As Integer
    Private m_lInsurance_file_cnt As Integer
    Private m_lRiskID As Integer

    'eck 11/2005
    Private m_vCoInsurerSplit(,) As Object
    Private m_bCoInsurerUpdate As Boolean
    Private m_lReserveId As Integer
    'DC080606 Add Coinsurer Details for Datasure
    Private m_cTotalCurrentReserve As Decimal
    Private m_cTotalRevisedReserve As Decimal
    Private m_iReserveChanged As Integer = 0

    'RVH - 27/09/2002   Added constants for columns
    Private Const clReserveIDColumn As Integer = 0
    Private Const clRevisedReserveColumn As Integer = 1
    Private Const clInitialReserveColumn As Integer = 2
    Private Const clPaidToDateColumn As Integer = 3
    Private Const clSumInsuredColumn As Integer = 4
    Private Const clThisRevisionColumn As Integer = 5
    Private Const clRevisedEnteredColumn As Integer = 6
    Private Const clAverageColumn As Integer = 7
    Private Const clReserveTypeIDColumn As Integer = 8
    Private Const clReserveTypeDescColumn As Integer = 9

    'AK 130503
    Private m_bStatus As Boolean
    Private m_bIsReservesReadOnly As Boolean?

    Private ChangedRevision As Integer
    Private ChangedInitReserve As Integer

    Public Structure udtReserveDetails
        Dim lReserveId As Integer
        Dim cRevisedReserve As Decimal
        Dim cInitialReserve As Decimal
        Dim cPaidToDate As Decimal
        Dim cSumInsured As Decimal
        Dim cThisRevision As Decimal
        Dim cRevisedEntered As Decimal
        Dim sngAverage As Single
        Dim lReserveTypeID As Integer
        Dim sReserveTypeDesc As String
        Public Shared Function CreateInstance() As udtReserveDetails
            Dim result As New udtReserveDetails
            result.sReserveTypeDesc = String.Empty
            Return result
        End Function
    End Structure

    Public Structure udtPaymentDetails
        Dim lReserveId As Integer
        Dim cTotalPayment As Decimal
        Dim cReserveAdjustment As Decimal
    End Structure

    ' returns the data now shown by this control
    'RVH - 27/09/2002   Changed from UDT to VARIANT
    Public Event DataHasChanged(ByVal Sender As Object, ByVal e As DataHasChangedEventArgs)

    Private m_lCurrencyID As Integer
    Private m_sCurrencyDesc As String = ""
    Private m_bIsRI2007Enabled As Boolean
    Private m_lParentHwnd As Integer
    Private m_bOpenClaimNoTrans As Boolean

    Private m_dUserReserveLimit As Decimal
    Private m_bReserveLimitExceeded As Boolean
    Private m_dExceededReserve As Decimal

    Private Function FillGrid() As Integer
        Dim result As Integer = 0
        Try
            Dim lItem As ListViewItem
            'DC080606 Add Coinsurer Details for Datasure
            Dim lSelectedItemIndex As Integer

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not (lstviewReserve.FocusedItem Is Nothing) Then
                lSelectedItemIndex = lstviewReserve.FocusedItem.Index + 1
            End If

            lstviewReserve.Items.Clear()
            For iRow As Integer = 0 To m_ListViewArray.GetUpperBound(0)

                'Developer Guide No.
                lItem = lstviewReserve.Items.Add(CStr(m_ListViewArray(iRow, m_colDescription)))
                With lItem
                    .Text = CStr(m_ListViewArray(iRow, m_colDescription))

                    .Tag = CStr(m_ListViewArray(iRow, m_colTag))
                    For iCol As Integer = 2 To m_ListViewArray.GetUpperBound(1)
                        ListViewHelper.GetListViewSubItem(lItem, iCol - 1).Text = CStr(m_ListViewArray(iRow, m_colTag + iCol))
                    Next iCol
                End With
            Next iRow

            If lSelectedItemIndex > 0 Then
                lstviewReserve.Items.Item(lSelectedItemIndex - 1).Selected = True
            End If

            'DC080606 Add Coinsurer Details for Datasure

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    '*************************************************************************************
    ' Name : PostReserveToOrion
    '
    ' Desc : post reserve transactions to orion
    '
    ' Hist : 15/03/2001 Created - Tinny
    '        05/07/01   RWH - Revised production of stats and removed stuff geared to
    '                   production of transactions as these will now be done in stored
    '                   procedures at the end of the roadmap.
    '*************************************************************************************
    Private Function PostReserveToOrion(ByVal v_vReserveArray As Object, ByVal v_oListView As ListView, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimID As Integer, ByVal v_lPerilID As Integer, ByVal v_sCOBCode As String, ByVal v_lCOBId As Integer) As Integer
        Dim result As Integer = 0
        Dim cTotalReserveTrans, cCurrentValue, cNewValue As Decimal
        Dim lTransactionTypeID As Integer
        Dim sTransactionTypeCode As String = ""
        Dim lDebitAccountID, lCreditAccountID As Integer
        Dim sDebitAccountCode, sCreditAccountCode As String
        Dim lStatsFolderCnt As Integer

        Dim g_oClaimTrans As bControlTransClaims.Automated
        Const ACInitialReserve As Integer = 1
        Const ACThisRevision As Integer = 3

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'view mode, who cares
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                Return result
            End If


            Dim temp_g_oClaimTrans As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oClaimTrans, "bControlTransClaims.Automated", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oClaimTrans = temp_g_oClaimTrans
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bControlTransClaims object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            'initialising debit/credit account
            sDebitAccountCode = "CLMEXP" & v_sCOBCode.Trim()
            sCreditAccountCode = "CLMRES" & v_sCOBCode.Trim()

            cTotalReserveTrans = 0
            cNewValue = 0
            cCurrentValue = 0

            'check to see if there are any changes in the reserves
            If v_oListView.Items.Count <> 0 Then

                Select Case m_sTransactionType ' frmInterface.TransactionType
                    Case "C_CO" 'from Open claim (initial reserve can be changed)
                        lTransactionTypeID = 26
                        sTransactionTypeCode = "C_CO" 'claim open

                        'total up new initial reserve
                        For lCount As Integer = 3 To v_oListView.Items.Count - 2
                            cNewValue += CDec(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lCount - 1), ACInitialReserve).Text.Trim())
                        Next

                        ' total up the initial reserve as it was when the control was loaded...changed to use different
                        ' array to fix PN20525
                        If Information.IsArray(g_vReserveDetails) Then

                            For lCount As Integer = 0 To g_vReserveDetails.GetUpperBound(1)

                                cCurrentValue += CDec(g_vReserveDetails(g_cIRDAinitialreserve, lCount))
                            Next
                        End If

                        cTotalReserveTrans = cNewValue - cCurrentValue

                    Case "C_CR", "C_CP"
                        'C_CP - reserves might have been adjusted because payment is greater than current reserve
                        'C_CR - reserves might have been revised
                        lTransactionTypeID = 28
                        sTransactionTypeCode = "C_CR"

                        'loop thro and total this revision column
                        For lCount As Integer = 3 To v_oListView.Items.Count - 2
                            cTotalReserveTrans += CDec(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lCount - 1), ACThisRevision).Text.Trim())
                        Next

                End Select

            End If

            'MSS011001 - Added switch for merge. Seemed 2 completely different ways of doing it

            'post to Orion only when reserves has been added/changed
            If cTotalReserveTrans <> 0 Then


                'data which goes in stats folder/detail and transaction detail

                g_oClaimTrans.DebitAccountID = lDebitAccountID 'claim expense

                g_oClaimTrans.CreditAccountID = lCreditAccountID 'claim reserve

                g_oClaimTrans.TransactionTypeID = lTransactionTypeID

                g_oClaimTrans.TransactionTypeCode = sTransactionTypeCode

                g_oClaimTrans.DocumentTypeID = 35 'Transferred Debit

                g_oClaimTrans.InsuranceFileCnt = v_lInsuranceFileCnt

                g_oClaimTrans.ClaimID = v_lClaimID

                g_oClaimTrans.PerilID = v_lPerilID

                g_oClaimTrans.DebitCredit = "D"

                g_oClaimTrans.DocumentComment = "Reserve for claim number " & v_lClaimID

                g_oClaimTrans.TransactionAmount = cTotalReserveTrans

                'RWH(02/07/01) Need to create stats separately now for each record to
                'account for reins and coins.

                m_lReturn = g_oClaimTrans.CreateStatsFolder(r_lStatsFolderCnt:=lStatsFolderCnt, v_sTransactionTypeCode:=sTransactionTypeCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'RWH(02/07/01) Create stats_detail for main payment.

                m_lReturn = g_oClaimTrans.CreateStatsDetails(v_lStatsFolderCnt:=lStatsFolderCnt, v_sStatsDetailType:="GRS", v_lClassOfBusId:=v_lCOBId, v_sClassOfBusCode:=v_sCOBCode, v_lRIPartyCnt:=lCreditAccountID, v_sRIShortName:=sCreditAccountCode, v_lRIPartyType:=0, v_sglRISharePercent:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                '        PostReserveToOrion = g_oClaimTrans.ProcessJournal()
                '        PostReserveToOrion = g_oClaimTrans.ProcessTransactions()

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to post to Orion", vApp:=ACApp, vClass:=ACClass, vMethod:="PostReserveToOrion", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                End If

            End If

            'MSS011001 - Merge end

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to post reserve transactions to Orion", vApp:=ACApp, vClass:=ACClass, vMethod:="PostReserveToOrion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    Public Function Save() As Integer
        Dim result As Integer = 0
        Try

            Dim sCOBCode As String = ""
            Dim cNewAmount, cNewReserve As Decimal
            Dim sEventDescription As String = ""
            Dim cInitialReserve As Decimal
            Dim bReserveEdit As Boolean
            Dim cPaymentAmount As Decimal
            Dim cNewCurrencyRate As Decimal
            Dim cOldCurrencyRate As Decimal
            Dim oResults As Object
            Dim sOverridenMessage As String


            If Not Information.IsArray(m_vReserveDetailsArray) Then
                MessageBox.Show("Warning! Peril is not linked to any reserves", "Claim Reserve", MessageBoxButtons.OK, MessageBoxIcon.Information)

                gPMFunctions.LogMessageToFile(g_oObjectManager.UserName, gPMConstants.PMELogLevel.PMLogWarning, "Warning! Peril is not linked to any reserves", ACApp, ACClass, "Save")

                Return gPMConstants.PMEReturnCode.PMTrue
            End If
            bReserveEdit = False
            cNewAmount = 0
            cInitialReserve = 0
            cPaymentAmount = 0
            If ChangedRevision = gPMConstants.PMEReturnCode.PMTrue Or ChangedInitReserve = gPMConstants.PMEReturnCode.PMTrue Then
                For iCnt As Integer = m_vReserveDetailsArray.GetLowerBound(1) To m_vReserveDetailsArray.GetUpperBound(1)
                    If CDec(m_vReserveDetailsArray(6, iCnt)) <> 0 OrElse m_iReserveChanged = 1 Then
                        bReserveEdit = True
                    End If
                    If m_bOpenClaimNoTrans Then
                        m_vReserveDetailsArray(6, iCnt) = CDec(m_vReserveDetailsArray(1, iCnt)) + CDec(m_vReserveDetailsArray(6, iCnt))
                    End If
                    cNewAmount += CDec(m_vReserveDetailsArray(6, iCnt))
                    cInitialReserve += CDec(m_vReserveDetailsArray(1, iCnt))
                    cNewReserve += CDec(m_vReserveDetailsArray(2, iCnt))
                    cPaymentAmount += CDec(m_vReserveDetailsArray(4, iCnt))
                Next iCnt
            End If
            If cNewAmount = 0 And Not bReserveEdit Then
                'no change
                Return gPMConstants.PMEReturnCode.PMTrue
            End If
            cNewCurrencyRate = -1
            cOldCurrencyRate = -1

                If m_sTransactionType = "C_CR" Then

                    result = m_oBusiness.GetCurrencyRatesToOverride(v_nClaimID:=m_lClaimID,
                                                            r_oResults:=oResults)

                End If
                If IsArray(oResults) Then
                    cNewCurrencyRate = oResults(0, 0)
                    cOldCurrencyRate = oResults(1, 0)

                End If
                If cNewCurrencyRate <> cOldCurrencyRate Then

                    sOverridenMessage = "The current currency rate (" & cNewCurrencyRate & ") is found different " &
                                 "from the currency rate (" & cOldCurrencyRate & ") when claim is opened." &
                                 vbCrLf & "This time, Do you want to override currency rate to " & cOldCurrencyRate & " ?"

                    If MessageBox.Show(sOverridenMessage, "Claim Reserve", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = vbYes Then

                        result = m_oBusiness.OverrideClaimCurrencyRate(v_nClaimID:=m_lClaimID,
                                                            r_oOverriddenCurrencyRate:=cOldCurrencyRate)

                        If result <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save the reserve details", vApp:=ACApp, vClass:=ACClass, vMethod:="Save", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                            Return result
                        End If
                    Else

                        result = m_oBusiness.OverrideClaimCurrencyRate(v_nClaimID:=m_lClaimID,
                                                            r_oOverriddenCurrencyRate:=0)
                        If result <> gPMConstants.PMEReturnCode.PMTrue Then

                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save the reserve details", vApp:=ACApp, vClass:=ACClass, vMethod:="Save", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                            Return result
                        End If
                    End If
                End If

                result = m_oBusiness.UpdateReserveDetails(m_vReserveDetailsArray)


            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save the reserve details", vApp:=ACApp, vClass:=ACClass, vMethod:="Save", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save the reserve details", vApp:=ACApp, vClass:=ACClass, vMethod:="Save", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    <Browsable(True)> _
    Public Shadows Property Text() As Object
        Get
            Dim result As Object = Nothing
            Try
                Dim vTemp(,) As Object
                Dim iList As ListViewItem

                With lstviewReserve
                    ReDim vTemp(.Items.Count, .Columns.Count)
                    ' get the headers

                    vTemp(0, 0) = ""
                    For iRow As Integer = 1 To .Columns.Count

                        vTemp(0, iRow) = .Columns.Item(iRow - 1).Text.Trim()
                    Next iRow


                    For iRow As Integer = 1 To vTemp.GetUpperBound(0)
                        iList = .Items.Item(iRow - 1)
                        ' save the reserve id in the first column
                        ' ids are only valid if > 1
                        If Conversion.Val(Convert.ToString(iList.Tag)) > 1 Then

                            vTemp(iRow, 0) = Conversion.Val(Convert.ToString(iList.Tag))
                        Else

                            vTemp(iRow, 0) = -1
                        End If

                        ' get the label for the row

                        vTemp(iRow, 1) = iList.Text

                        ' get the average value

                        vTemp(iRow, 8) = ListViewHelper.GetListViewSubItem(iList, 7).Text

                        ' get the remaining fields

                        For iCol As Integer = 1 To vTemp.GetUpperBound(1) - 2
                            Dim dbNumericTemp As Double
                            If Double.TryParse(ListViewHelper.GetListViewSubItem(iList, iCol).Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                                vTemp(iRow, iCol + 1) = CDec(ListViewHelper.GetListViewSubItem(iList, iCol).Text)
                            Else

                                vTemp(iRow, iCol + 1) = 0
                            End If
                        Next iCol
                    Next iRow
                End With

                result = vTemp
                iList = Nothing

                Return result

            Catch



                Return result
            End Try
        End Get
        Set(ByVal Value As Object)

        End Set
    End Property
    ''' <summary>
    ''' Holds the flag for IsReserveReadOnly configured in product maintenance.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property IsReservesReadOnly() As Boolean
        Get
            If Not m_bIsReservesReadOnly.HasValue Then
                GetIsReservesReadOnly()
            End If
            Return m_bIsReservesReadOnly
        End Get
    End Property

    <Browsable(True)> _
    Public Property ShowEdit() As Boolean
        Get
            Return m_ShowEdit
        End Get
        Set(ByVal Value As Boolean)
            m_ShowEdit = Value

            ' show/hide the edit button
            cmdEdit.Visible = Value
            m_ShowEdit = Value

            ' redraw the control
            uctCLMReserve_Resize(Me, New EventArgs())

            RaiseEvent ShowEditChange()
        End Set
    End Property

    <Browsable(True)> _
    Public Property ShowCoInsurers() As Boolean
        Get
            Return m_ShowCoInsurers
        End Get
        Set(ByVal Value As Boolean)
            m_ShowCoInsurers = Value

            'DC080606 Add Coinsurer Details for Datasure
            ' show/hide the CoInsurers frame
            fraCoInsurers.Visible = Value

            ' redraw the control
            uctCLMReserve_Resize(Me, New EventArgs())

            RaiseEvent ShowCoInsurersChange()
        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property Insurance_File_Cnt() As Integer
        Get
            Return m_lInsurance_file_cnt
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property PerilID() As Integer
        Get
            Return m_lPerilID
        End Get
    End Property



    <Browsable(False)> _
    Public ReadOnly Property ClaimID() As Integer
        Get
            Return m_lClaimID
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property PerilTypeID() As Integer
        Get
            Return m_lPerilTypeID
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property RiskID() As Integer
        Get
            Return m_lRiskID
        End Get
    End Property


    <Browsable(True)> _
    Public Shadows Property Enabled() As Boolean
        Get
            Return m_Enabled
        End Get
        Set(ByVal Value As Boolean)

            m_Enabled = Value
            fraReserveDetails.Enabled = m_Enabled
            lstviewReserve.Enabled = m_Enabled
            RaiseEvent EnabledChange()

        End Set
    End Property


    <Browsable(True)> _
    Public Property Visible_Renamed() As Boolean
        Get
            Return m_Visible
        End Get
        Set(ByVal Value As Boolean)

            m_Visible = Value
            fraReserveDetails.Visible = m_Visible
            RaiseEvent VisibleChange()

        End Set
    End Property

    <Browsable(False)> _
    Public WriteOnly Property IsRI2007Enabled() As Boolean
        Set(ByVal Value As Boolean)
            m_bIsRI2007Enabled = Value
        End Set
    End Property


    <Browsable(True)> _
    Public Property IsOpenClaimNoTrans() As Boolean
        Get
            Return m_bOpenClaimNoTrans
        End Get
        Set(ByVal Value As Boolean)
            m_bOpenClaimNoTrans = Value
        End Set
    End Property

    Private Function UpdateCurrentReserve(ByVal iListViewIndex As Integer, ByVal cNewPayment As Decimal, ByVal cReserveAdjustment As Decimal) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim cCurrReserve As Decimal

        Try

            'Update the revision amount
            m_ListViewArray(iListViewIndex - 1, m_colThisRevision) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(cReserveAdjustment))

            cCurrReserve = CDec(gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(CDec(m_ListViewArray(iListViewIndex - 1, m_colInitial)) + CDec(m_ListViewArray(iListViewIndex - 1, m_colRevisionAmount)) + CDec(m_ListViewArray(iListViewIndex - 1, m_colThisRevision)) - cNewPayment)))

            'sum insured
            m_ListViewArray(iListViewIndex - 1, m_colSumInsured) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(CDec(m_ListViewArray(iListViewIndex - 1, m_colSumInsured))))

            If cCurrReserve < 0 And Not m_bAllowNegativeReserve And Not m_bOpenClaimNoTrans Then
                ' this revision
                m_ListViewArray(iListViewIndex - 1, m_colThisRevision) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(CDec(m_ListViewArray(iListViewIndex - 1, m_colThisRevision)) + -1 * cCurrReserve))
                ' current reserve
                m_ListViewArray(iListViewIndex - 1, m_colCurrentReserve) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(0))
                ' incurred
                m_ListViewArray(iListViewIndex - 1, m_colIncurred) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(CDec(m_ListViewArray(iListViewIndex - 1, m_colInitial)) + CDec(m_ListViewArray(iListViewIndex - 1, m_colRevisionAmount)) + CDec(m_ListViewArray(iListViewIndex - 1, m_colThisRevision))))
            Else
                'Current Reserve = initial reserve + revise reserve + this revision - paid to date
                m_ListViewArray(iListViewIndex - 1, m_colCurrentReserve) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(cCurrReserve))

                If m_bOpenClaimNoTrans Then
                    m_ListViewArray(iListViewIndex - 1, m_colCurrentReserve) = StringsHelper.Format(cCurrReserve, "0.00")
                    m_ListViewArray(iListViewIndex - 1, m_colIncurred) = StringsHelper.Format(CDec(m_ListViewArray(iListViewIndex - 1, m_colInitial)) + CDec(m_ListViewArray(iListViewIndex - 1, m_colThisRevision)), "0.00")
                    m_lReturn = CType(UpdateReserveItem(CInt(m_ListViewArray(iListViewIndex - 1, m_colTag))), gPMConstants.PMEReturnCode)
                End If
            End If

            ' update the values in the array

            g_vReserveDetails(g_cIRDApaidtodate, iListViewIndex - 3) = cNewPayment

            m_lReturn = GetTotalValues()
            m_lReturn = FillGrid()


            Return result

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the current" & " reserve", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateCurrentReserve", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    Public Function UpdatePaymentValue(ByRef uNewValues As udtPaymentDetails) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If uNewValues.lReserveId < 1 Then
                Return result
            End If

            If lstviewReserve.Items.Count < 1 Then
                Return result
            End If

            ' which reserve was this payment against
            With lstviewReserve
                For iTemp As Integer = 1 To .Items.Count
                    If Conversion.Val(Convert.ToString(.Items.Item(iTemp - 1).Tag)) = uNewValues.lReserveId Then
                        ' has it changed

                        If (CDbl(g_vReserveDetails(g_cIRDApaidtodate, iTemp - 3))) <> Conversion.Val(CStr(uNewValues.cTotalPayment)) Or m_bOpenClaimNoTrans Then

                            m_lReturn = UpdateCurrentReserve(iTemp, uNewValues.cTotalPayment, uNewValues.cReserveAdjustment)
                        End If
                        Exit For
                    End If
                Next iTemp
            End With

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save the reserve " & "details", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePaymentValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    Private Function CreateOutputDetails(ByRef vDetails As Object) As Integer
        'RVH - 27/09/2002   Changed vDetails from UDT to VARIANT
        Dim result As Integer = 0
        Try

            If Not Information.IsArray(m_vReserveDetailsArray) Then
                ' no values
                Return result
            End If

            ' resize the array according to the number of
            ' reserve types we have
            ReDim vDetails(9, m_vReserveDetailsArray.GetUpperBound(1))
            For iCnt As Integer = 0 To m_vReserveDetailsArray.GetUpperBound(1)
                'RVH - 27/09/2002   Used constants for columns

                vDetails(clReserveIDColumn, iCnt) = m_vReserveDetailsArray(0, iCnt)

                vDetails(clInitialReserveColumn, iCnt) = CDec(m_vReserveDetailsArray(1, iCnt))

                vDetails(clRevisedReserveColumn, iCnt) = CDec(m_vReserveDetailsArray(2, iCnt))

                vDetails(clAverageColumn, iCnt) = Conversion.Val(CStr(m_vReserveDetailsArray(3, iCnt)))

                vDetails(clPaidToDateColumn, iCnt) = CDec(m_vReserveDetailsArray(4, iCnt))


                vDetails(clReserveTypeIDColumn, iCnt) = g_vReserveDetails(g_cIRDAreservetype, iCnt)


                vDetails(clSumInsuredColumn, iCnt) = CDec(g_vReserveDetails(g_cIRDAsuminsured, iCnt))
                'ECK Oct 2005  Removed Underwriting checks not sure that Broking version is correct

                vDetails(clThisRevisionColumn, iCnt) = Conversion.Val(CStr(m_vReserveDetailsArray(6, iCnt)))


                If m_vReserveDetailsArray.GetUpperBound(0) >= g_cIRDArevisedentered Then

                    vDetails(clRevisedEnteredColumn, iCnt) = Conversion.Val(CStr(m_vReserveDetailsArray(7, iCnt)))
                Else

                    vDetails(clRevisedEnteredColumn, iCnt) = 0
                End If


                vDetails(clReserveTypeDescColumn, iCnt) = ""
                ' get the reserve description from the listview control
                For iItems As Integer = 1 To lstviewReserve.Items.Count

                    If Conversion.Val(Convert.ToString(lstviewReserve.Items.Item(iItems - 1).Tag)) = Conversion.Val(CStr(vDetails(clReserveIDColumn, iCnt))) Then

                        vDetails(clReserveTypeDescColumn, iCnt) = lstviewReserve.Items.Item(iItems - 1).Text
                        Exit For
                    End If
                Next iItems
            Next iCnt

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create output details", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOutputDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    Private Function RecalcReserves(Optional ByVal cNewRevision As Decimal = -1.9, Optional ByVal cInitialReserve As Decimal = -1.9) As Integer
        Dim result As Integer = 0
        Try
            Dim iItem As Integer

            result = gPMConstants.PMEReturnCode.PMTrue

            With lstviewReserve
                ' get the currently select item
                'If .FocusedItem.Index + 1 < 1 Then
                If .SelectedItems(0).Index + 1 < 1 Then
                    ' no item selected
                    Return result
                End If
                iItem = .SelectedItems(0).Index + 1 - 1

                If cInitialReserve <> -1.9 Then
                    'initial reserve - reserve tab
                    m_ListViewArray(iItem, m_colInitial) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(cInitialReserve))

                    cNewRevision = 0
                Else
                    'this revision - reserve tab
                    m_ListViewArray(iItem, m_colThisRevision) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(cNewRevision))
                End If

                'incurred = initial reserve + revise reserve + this revision (reserve tab)
                m_ListViewArray(iItem, m_colIncurred) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(CDec(m_ListViewArray(iItem, m_colInitial)) + CDec(m_ListViewArray(iItem, m_colRevisionAmount)) + CDec(m_ListViewArray(iItem, m_colThisRevision))))

                ' current reserve = initial reserve + revise reserve + this revision - paid to date

                m_ListViewArray(iItem, m_colCurrentReserve) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(CDec(m_ListViewArray(iItem, m_colInitial)) + CDec(m_ListViewArray(iItem, m_colRevisionAmount)) + cNewRevision - Conversion.Val(CStr(g_vReserveDetails(g_cIRDApaidtodate, iItem - 2)))))

            End With

            m_lReturn = GetTotalValues()
            m_lReturn = FillGrid()

            ' update the variant array
            m_lReturn = CType(UpdateReserveItem(CInt(m_ListViewArray(iItem, m_colTag))), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to recalculate reserve figures", vApp:=ACApp, vClass:=ACClass, vMethod:="RecalcReserves", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Function UpdateReserveItem(Optional ByVal lReserveId As Integer = -1) As Integer
        Dim result As Integer = 0
        Dim lstItem As ListViewItem
        Try
            Dim lCount As Integer

            result = gPMConstants.PMEReturnCode.PMTrue


            If Not Information.IsArray(g_vReserveDetails) Or Object.Equals(g_vReserveDetails, Nothing) Then
                Return result
            End If

            If lReserveId = -1 Then
                ' updating all items so initialise the array
                If Not Information.IsArray(g_vReserveDetails) Then
                    m_vReserveDetailsArray = Nothing
                    Return result
                Else

                    ReDim m_vReserveDetailsArray(7, g_vReserveDetails.GetUpperBound(1))
                End If

                ' set default values
                For lCount = m_vReserveDetailsArray.GetLowerBound(1) To m_vReserveDetailsArray.GetUpperBound(1)
                    'paid to date = paid to date + this payment

                    m_vReserveDetailsArray(4, lCount) = CDec(g_vReserveDetails(g_cIRDApaidtodate, lCount))

                    'this payment
                    m_vReserveDetailsArray(5, lCount) = 0
                Next lCount
                lCount = 3
            Else
                ' which item are we updating
                lCount = -1
                With lstviewReserve
                    For lCount = 3 To .Items.Count
                        If Conversion.Val(Convert.ToString(.Items.Item(lCount - 1).Tag)) = lReserveId Then
                            Exit For
                        End If
                    Next lCount
                    If lCount < 0 Or (lCount = .Items.Count And Conversion.Val(Convert.ToString(.Items.Item(lCount - 1).Tag)) <> lReserveId) Then
                        ' haven't found the item so log an error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to locate the reserve item", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateReserveItem", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If
                End With
            End If

            For iItem As Integer = m_vReserveDetailsArray.GetLowerBound(1) To m_vReserveDetailsArray.GetUpperBound(1)
                '        Set lstItem = lstviewReserve.ListItems(lCount).tag
                lstItem = lstviewReserve.Items.Item(iItem + 2)
                If lReserveId = -1 Or (lReserveId = Conversion.Val(Convert.ToString(lstItem.Tag))) Then
                    'Set lstItem = lstviewReserve.ListItems(lCount)
                    'reserve id

                    m_vReserveDetailsArray(0, iItem) = Convert.ToString(lstItem.Tag)
                    'initial reserve
                    m_vReserveDetailsArray(1, iItem) = ListViewHelper.GetListViewSubItem(lstItem, 1).Text
                    'Sum Insured
                    m_vReserveDetailsArray(7, iItem) = ListViewHelper.GetListViewSubItem(lstItem, 6).Text
                    'average
                    m_vReserveDetailsArray(3, iItem) = gPMFunctions.UnFormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, gPMConstants.PMEFormatStyle.PMFormatLong, ListViewHelper.GetListViewSubItem(lstItem, 7).Text)

                    If m_bOpenClaimNoTrans Then
                        m_vReserveDetailsArray(6, iItem) = ListViewHelper.GetListViewSubItem(lstItem, 3).Text

                        'revise reserve
                        m_vReserveDetailsArray(2, iItem) = ListViewHelper.GetListViewSubItem(lstItem, 2).Text
                        'set this_revision to initial reserve when open claim
                    ElseIf m_sTransactionType = "C_CO" Then

                        'set this revision = initial reserve
                        If IsReservesReadOnly Then
                            m_vReserveDetailsArray(6, iItem) = 0
                        Else
                            m_vReserveDetailsArray(6, iItem) = ListViewHelper.GetListViewSubItem(lstItem, 1).Text
                        End If
                        'revise reserve
                        m_vReserveDetailsArray(2, iItem) = ListViewHelper.GetListViewSubItem(lstItem, 2).Text
					Else
                            ' set this_revision to this_revision column when maintain claim
                            'this revision
                            m_vReserveDetailsArray(6, iItem) = ListViewHelper.GetListViewSubItem(lstItem, 3).Text

                            'revise reserve = revise reserve + this revision
                            m_vReserveDetailsArray(2, iItem) = CDec(ListViewHelper.GetListViewSubItem(lstItem, 2).Text) + CDec(ListViewHelper.GetListViewSubItem(lstItem, 3).Text)
                        End If

                    End If
			Next iItem
            lstItem = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the reserve item", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateReserveItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



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

                m_iTask = CType(CInt(vTask), gPMConstants.PMEComponentAction)
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



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub lstviewReserve_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lstviewReserve.Click
        Dim vTagvalue As Object
        Dim vValue As Object
        Try

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                ' view mode
                cmdEdit.Enabled = False
                Exit Sub
            End If

            ' edit property of this control is disabled
            If Not m_ShowEdit Then Exit Sub
            If lstviewReserve.Items.Count < 1 Then Exit Sub

            vTagvalue = lstviewReserve.FocusedItem.Index + 1

            vValue = Convert.ToString(lstviewReserve.FocusedItem.Tag)
            If vTagvalue <> 1 And vTagvalue <> 2 And vValue <> 0 Then
                'eck 11/2005 check for Coinsurer details
                If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                    'PN: 48486, 48488
                    If (m_sTransactionType <> "C_CP") And (m_sTransactionType <> "C_RV") And (m_sTransactionType <> "C_SA") Then
                        '_
                        '                    And (m_sTransactionType$ = "C_CR") _
                        ''                    And (vValue <> 0) Then
                        cmdEdit.Enabled = vValue <> 0
                    End If

                End If

            Else
                cmdEdit.Enabled = False
            End If

        Catch


            Exit Sub
        End Try

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
        Try
            Dim vNewData As Object
            Dim crInitReserve, crRevisedReserve As Decimal
            Const kEventTypeNewClaim As Integer = 3
            Const kEventTypeMaintainClaim As Integer = 6

            'developer guide no. New Instance is created.
            Dim FrmDetailsUW As frmDetailsUW = New frmDetailsUW
            ' edit property of this control is disabled
            If Not m_ShowEdit Then Exit Sub
            'ECK Oct 2005  Removed Underwriting checks so that SOMETHING happens for Broking
            With FrmDetailsUW
                .LossCurrency = m_sCurrencyDesc
                .AllowNegativeReserve = m_bAllowNegativeReserve
                .IsOpenClaimNoTrans = m_bOpenClaimNoTrans

                If m_bOpenClaimNoTrans Then
                    crRevisedReserve = CDec(ListViewHelper.GetListViewSubItem(lstviewReserve.Items.Item(lstviewReserve.FocusedItem.Index), m_lstThisRevision).Text)
                Else
                    crRevisedReserve = CDec(ListViewHelper.GetListViewSubItem(lstviewReserve.Items.Item(lstviewReserve.FocusedItem.Index), m_lstRevisionAmount).Text)
                End If


                m_lReturn = CType(.Initialise(sTransactionType:=m_sTransactionType, iTask:=m_iTask, sRiskType:=m_sRisktype, iInitialReserve:=CDec(ListViewHelper.GetListViewSubItem(lstviewReserve.Items.Item(lstviewReserve.FocusedItem.Index), m_lstInitialReserve).Text), iRevisionAmount:=crRevisedReserve, iPaidToDate:=CDec(g_vReserveDetails(g_cIRecoveryDApaidtodate, lstviewReserve.FocusedItem.Index + 1 - 3))), gPMConstants.PMEReturnCode)

                'CenterForm frmDetailsUW

                m_lParentHwnd = FrmDetailsUW.Handle.ToInt32()

                ' Since this control can also be invoked via claim builder
                ' so to keep this form in front of all other claim builder
                ' screens we are calling the below function

                KeepWindowOnTop(True)


                VB6.ShowForm(FrmDetailsUW, FormShowConstants.Modal, MyBase.FindForm()) ' Me
                ChangedRevision = .RevisionHasBeenAmended
                ChangedInitReserve = .InitialReserveHasBeenSet
                m_iReserveChanged = ChangedInitReserve
                If m_bOpenClaimNoTrans Then
                    crInitReserve = .InitialReserve
                    crRevisedReserve = .Revision
                ElseIf ChangedInitReserve Then
                    ' figure represents initial reserve
                    crInitReserve = .InitialReserve
                ElseIf ChangedRevision Then
                    crRevisedReserve = .Revision
                End If
                FrmDetailsUW.Close()
                cmdEdit.Enabled = False
            End With


            If ChangedRevision = gPMConstants.PMEReturnCode.PMTrue Or ChangedInitReserve = gPMConstants.PMEReturnCode.PMTrue Then

                If m_dUserReserveLimit > 0 Then '' This means no limit has been set for a user and he can process with adding the reserves.
                    m_lReturn = CheckRevisionWithinUserLimit(crInitReserve, crRevisedReserve)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate users reserve limit.", vApp:=ACApp, vClass:=ACClass, vMethod:="Edit Button Click Event", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Exit Sub
                    End If
                End If

                If Not m_bReserveLimitExceeded Then
                    ' update this listview
                    If m_bOpenClaimNoTrans Then
                        m_lReturn = CType(RecalcReserves(cInitialReserve:=crInitReserve), gPMConstants.PMEReturnCode)
                        m_lReturn = CType(RecalcReserves(cNewRevision:=crRevisedReserve), gPMConstants.PMEReturnCode)
                    ElseIf ChangedInitReserve Then
                        m_lReturn = CType(RecalcReserves(cInitialReserve:=crInitReserve), gPMConstants.PMEReturnCode)
                    ElseIf ChangedRevision Then
                        m_lReturn = CType(RecalcReserves(cNewRevision:=crRevisedReserve), gPMConstants.PMEReturnCode)
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to calculate the new reserve figures", vApp:=ACApp, vClass:=ACClass, vMethod:="Edit Button Click Event", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Exit Sub
                    End If

                    'RVH - 27/09/2002   Changed from UDT to VARIANT
                    m_lReturn = CType(CreateOutputDetails(vNewData), gPMConstants.PMEReturnCode)
                    RaiseEvent DataHasChanged(Me, New DataHasChangedEventArgs(vNewData))
                Else

                    MessageBox.Show("Reserve Limit has been exceeded for the User '" & g_oObjectManager.UserName & "' and Rejected. The navigator would will now terminate. A task has been created for this action.")
                    ReflectionHelper.SetMember(MyBase.ParentForm, "ReserveLimitExceeded", True)
                    ReflectionHelper.SetMember(MyBase.ParentForm, "ExceededReserve", m_dExceededReserve)
                    MyBase.ParentForm.Close()
                    Exit Sub
                End If
            End If
                'developer guide no.(For the focus to remain on the parent form)
                MyBase.ParentForm.Focus()
        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call the edit button event", vApp:=ACApp, vClass:=ACClass, vMethod:="Edit Button Click Event", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub


    Public Function GetDetails(Optional ByVal lPerilID As Integer = 0, Optional ByVal lPerilTypeID As Integer = 0, Optional ByVal lClaimID As Integer = 0, Optional ByVal lRiskID As Integer = 0, Optional ByVal lInsurance_File_Cnt As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim v_Temp As Object
        Dim oRiskDetails As Object
        Dim v_RiskDetails As Object

        Try

            ' set the return value
            result = gPMConstants.PMEReturnCode.PMTrue

            ' check the input parameters
            If lPerilTypeID = 0 And (lPerilID = 0 Or lClaimID = 0 Or lRiskID = 0 Or lInsurance_File_Cnt = 0) Then

                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="The parameters provided are insufficient or incorrect", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")
                Return result

            ElseIf lPerilTypeID <> 0 Then
                m_lPerilTypeID = lPerilTypeID
                m_lPerilID = lPerilID
                m_lClaimID = lClaimID
                m_lRiskID = lRiskID
                m_lInsurance_file_cnt = lInsurance_File_Cnt
            Else
                m_lPerilID = lPerilID
                m_lClaimID = lClaimID
                m_lRiskID = lRiskID
                m_lInsurance_file_cnt = lInsurance_File_Cnt
            End If


            ' disable the edit button
            cmdEdit.Enabled = False

            'AK 130503 - stop the user from adding any payments, if any existing payment is outstanding - start

            m_lReturn = m_oBusiness.CheckReferredPayment(m_lClaimID, m_bStatus)

            'Edit button will be permanently disabled if task type is set to view
            If m_bStatus Then
                m_iTask = gPMConstants.PMEComponentAction.PMView
            End If

            'AK 130503 - End
            ' set these values for the business object

            m_oBusiness.PerilID = m_lPerilID

            m_oBusiness.PerilTypeID = m_lPerilTypeID

            m_oBusiness.ClaimID = m_lClaimID

            ' Had to call this function cos it is the
            ' only way to pass the above parameters to
            ' the dCLMPeril.Data that is used by the business layer

            m_lReturn = m_oBusiness.GetControls(v_Temp)

            m_lReturn = m_oBusiness.GetUsersReserveLimit(m_dUserReserveLimit)

            'reset the array
            m_ListViewArray = Nothing

            m_lReturn = GetReserveType()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the GetReserve Type", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If
            m_lReturn = CType(GetProductDetails(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            m_lReturn = CType(GetReserveDetails(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(GetRecoveryDetails(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = CType(GetTotalValues(), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = FillGrid()

            ' initialise the return array
            UpdateReserveItem()


            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_oRiskDetails As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oRiskDetails, "bCLMRiskDetails.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oRiskDetails = temp_oRiskDetails

            ' initialise this new object

            m_lReturn = oRiskDetails.Initialise(sUsername:=g_oObjectManager.UserName, sPassword:=g_oObjectManager.Password, iUserID:=g_iUserId, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_oObjectManager.CurrencyID, iLogLevel:=g_oObjectManager.LogLevel, sCallingAppName:=ACApp)

            ' get the text description of the risk type

            m_lReturn = oRiskDetails.GetRiskDetails(v_lrisk:=m_lRiskID, v_lpolicyid:=m_lInsurance_file_cnt, r_vdataarray:=v_RiskDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the Risk Details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End If


            oRiskDetails.Dispose()
            If Information.IsArray(v_RiskDetails) Then

                m_sRisktype = CStr(v_RiskDetails(1, 0))
            Else
                m_sRisktype = ""
            End If


            If m_lClaimID <> 0 Then

                m_lReturn = m_oBusiness.GetClaimCurrency(v_lClaimID:=m_lClaimID, r_lCurrencyID:=m_lCurrencyID, r_sCurrencyDesc:=m_sCurrencyDesc)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get the claim currency")
                End If
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    'eck 11/2005 Determine whether there is a coinsurer breakdown
    Public Function GetCoInsurerDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'DC080606 Add Coinsurer Details for Datasure

            m_lReturn = m_oBusiness.GetCoInsurerDetails(v_lInsuranceFileCnt:=m_lInsurance_file_cnt, v_lClaimID:=m_lClaimID, r_vResults:=m_vCoInsurerSplit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the CoInsurer Details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End If

            'DC080606 Add Coinsurer Details for Datasure
            If Information.IsArray(m_vCoInsurerSplit) Then
                If CStr(m_vCoInsurerSplit(0, 0)) = "MULTI" Or m_vCoInsurerSplit.GetUpperBound(1) > 0 Then
                    fraCoInsurers.Visible = True
                    fraCoInsurers.Enabled = True
                Else
                    fraCoInsurers.Visible = False
                    fraCoInsurers.Enabled = False

                End If
            Else
                fraCoInsurers.Visible = False
                fraCoInsurers.Enabled = False
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the coInsurer details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCoInsurerDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function SetupListview() As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            'ECK Oct 2005  Uses common headers
            ' insert the column headers
            lstviewReserve.Columns.Insert(0, "", 150) ', Width:=sngWidth
            lstviewReserve.Columns.Item(0).TextAlign = HorizontalAlignment.Left
            lstviewReserve.Columns.Insert(1, "Initial Reserve", 110) ', Width:=sngWidth
            lstviewReserve.Columns.Item(1).TextAlign = HorizontalAlignment.Right
            lstviewReserve.Columns.Insert(2, "Revision Amount", 110) ', Width:=sngWidth
            lstviewReserve.Columns.Item(2).TextAlign = HorizontalAlignment.Right
            lstviewReserve.Columns.Insert(3, "This Revision", 110) ', Width:=sngWidth
            lstviewReserve.Columns.Item(3).TextAlign = HorizontalAlignment.Right
            lstviewReserve.Columns.Insert(4, "Current Reserve", 110) ', Width:=sngWidth
            lstviewReserve.Columns.Item(4).TextAlign = HorizontalAlignment.Right
            lstviewReserve.Columns.Insert(5, "Incurred", 110) ', Width:=sngWidth
            lstviewReserve.Columns.Item(5).TextAlign = HorizontalAlignment.Right
            lstviewReserve.Columns.Insert(6, "Sum Insured", 110) ', Width:=sngWidth
            lstviewReserve.Columns.Item(6).TextAlign = HorizontalAlignment.Right
            lstviewReserve.Columns.Insert(7, "Average", 110) ', Width:=sngWidth
            lstviewReserve.Columns.Item(7).TextAlign = HorizontalAlignment.Right
            '    Else
            ' insert the column headers
            '        lstviewReserve.ColumnHeaders.Add Index:=1, Text:="               " ', Width:=sngWidth
            '        lstviewReserve.ColumnHeaders.Item(1).Alignment = lvwColumnLeft
            '        lstviewReserve.ColumnHeaders.Add Index:=2, Text:="Initial Reserve" ', Width:=sngWidth
            '        lstviewReserve.ColumnHeaders.Item(2).Alignment = lvwColumnRight
            '        lstviewReserve.ColumnHeaders.Add Index:=3, Text:="Revised Reserve" ', Width:=sngWidth
            '        lstviewReserve.ColumnHeaders.Item(3).Alignment = lvwColumnRight
            '        lstviewReserve.ColumnHeaders.Add Index:=4, Text:="Current Reserve" ', Width:=sngWidth
            '        lstviewReserve.ColumnHeaders.Item(4).Alignment = lvwColumnRight
            '        lstviewReserve.ColumnHeaders.Add Index:=5, Text:="Incurred" ', Width:=sngWidth
            '        lstviewReserve.ColumnHeaders.Item(5).Alignment = lvwColumnRight
            '        lstviewReserve.ColumnHeaders.Add Index:=6, Text:="Sum Insured" ', Width:=sngWidth
            '        lstviewReserve.ColumnHeaders.Item(6).Alignment = lvwColumnRight
            '        lstviewReserve.ColumnHeaders.Add Index:=7, Text:="Average" ', Width:=sngWidth
            '        lstviewReserve.ColumnHeaders.Item(7).Alignment = lvwColumnRight
            '        lstviewReserve.ColumnHeaders.Add Index:=8, Text:="Revised Entered", Width:=0
            '        lstviewReserve.ColumnHeaders.Item(8).Alignment = lvwColumnRight
            '
            '    End If

            'Get the columns to re-size themselves to show as much data as possible
            'ListView6Autosize(lstviewReserve, True)

            lstviewReserve.LabelEdit = False

            ' add the grid lines and full row select for the Reserve List view
            'developer guide no.(The following function does not work)
            'start
            'm_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lstviewReserve.Handle.ToInt32(), v_vShowGridLines:=True, v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)
            lstviewReserve.GridLines = True
            lstviewReserve.FullRowSelect = True
            'end
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                Return result
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the " & "Control", vApp:=ACApp, vClass:=ACClass, vMethod:="SetupListview", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Sub lstviewReserve_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lstviewReserve.DoubleClick
        Dim vTagvalue As Object
        Try

            If lstviewReserve.Items.Count < 1 Then Exit Sub


            vTagvalue = Convert.ToString(lstviewReserve.FocusedItem.Tag)

            'AK 130503
            If m_bStatus Then
                MessageBox.Show("Awaiting Authorisation for existing payment", "Claim Payment")
            End If

            If vTagvalue <> "" And vTagvalue <> "0" And cmdEdit.Enabled Then
                cmdEdit_Click(cmdEdit, New EventArgs())
            End If

        Catch
        End Try

        Exit Sub
    End Sub

    Private Sub lstviewReserve_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles lstviewReserve.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If KeyAscii = CInt(Keys.Return) And cmdEdit.Enabled Then cmdEdit_Click(cmdEdit, New EventArgs())
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub lstviewReserve_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lstviewReserve.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        lstviewReserve_Click(lstviewReserve, New EventArgs())
    End Sub

    'Initialize Properties for User Control

    Private Sub UserControl_InitProperties()
        m_BackColor = m_def_BackColor
        m_ForeColor = m_def_ForeColor
        m_Enabled = m_def_Enabled

        'developer guide no. 2 of No Solutions
        'm_Font = Ambient.Font
        m_BackStyle = m_def_BackStyle
        m_BorderStyle = m_def_BorderStyle
        m_ShowEdit = m_def_ShowEdit
        m_ShowCoInsurers = m_def_ShowCoInsurers
        m_Visible = m_def_Visible
    End Sub




    'developer guide no. 1 of No Solutions
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)


        m_BackColor = CInt(PropBag.ReadProperty("BackColor", m_def_BackColor))


        m_ForeColor = CInt(PropBag.ReadProperty("ForeColor", m_def_ForeColor))


        m_Enabled = CBool(PropBag.ReadProperty("Enabled", m_def_Enabled))


        'developer guide no. 2 of No Solutions
        'm_Font = PropBag.ReadProperty("Font", Ambient.Font)
        m_Font = PropBag.ReadProperty("Font", MyBase.Font)


        m_BackStyle = CInt(PropBag.ReadProperty("BackStyle", m_def_BackStyle))


        m_BorderStyle = CInt(PropBag.ReadProperty("BorderStyle", m_def_BorderStyle))


        m_ShowEdit = CBool(PropBag.ReadProperty("ShowEdit", m_def_ShowEdit))


        m_ShowCoInsurers = CBool(PropBag.ReadProperty("ShowCoInsurers", m_def_ShowCoInsurers))


        m_Visible = CBool(PropBag.ReadProperty("Visible", m_def_Visible))
    End Sub


    Private Sub uctCLMReserve_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        Try
            Const ACTwice As Integer = 2
            Const ACThrice As Integer = 3
            'Const ACFormMargin As Integer = 45
            Const ACLeftMargin As Integer = 1
            'Const ACHeightDifference As Integer = 8
            'Const ACMinWidth As Integer = 5300
            'Const ACMinHeight As Integer = 3000

            ' check for minimum width
            'If ClientRectangle.Width < ACCommandButtonWidth * 2 Then
            If VB6.PixelsToTwipsY(ClientRectangle.Width) < ACCommandButtonWidth * 2 Then
                Width = VB6.TwipsToPixelsX(ACCommandButtonWidth * 2)
                Exit Sub
            Else
                Width = MyBase.ClientRectangle.Width
            End If

            ' check for minimum height
            If VB6.PixelsToTwipsY(ClientRectangle.Height) < ACCommandButtonHeight * 2 Then
                Height = VB6.TwipsToPixelsY(ACCommandButtonHeight * 2)
                Exit Sub
            Else
                Height = MyBase.ClientRectangle.Height
            End If

            ' resize the frame
            With fraReserveDetails

                .Left = VB6.TwipsToPixelsX(ACLeftMargin)
                .Width = MyBase.ClientRectangle.Width - VB6.TwipsToPixelsX(ACTwice * ACLeftMargin)
                .Top = VB6.TwipsToPixelsY(ACLeftMargin)
                If Not m_ShowCoInsurers Then
                    .Height = MyBase.ClientRectangle.Height
                Else
                    .Height = MyBase.ClientRectangle.Height / 2
                End If

            End With

            If m_ShowCoInsurers Then

                With fraCoInsurers
                    .Left = VB6.TwipsToPixelsX(ACLeftMargin)
                    .Width = MyBase.ClientRectangle.Width - VB6.TwipsToPixelsX(ACTwice * ACLeftMargin)
                    .Top = MyBase.ClientRectangle.Height / 2
                    .Height = MyBase.ClientRectangle.Height / 2
                End With

            End If

            If m_ShowEdit Then
                'resize the list view
                With lstviewReserve
                    ' Set list view's tops, left, height & width
                    .Top = VB6.TwipsToPixelsY(ACListViewTop)
                    .Left = VB6.TwipsToPixelsX(ACCtrlVerticalSpacing)
                    .Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(fraReserveDetails.Width) - ACCommandButtonWidth - (ACThrice * ACCtrlVerticalSpacing))
                    .Height = fraReserveDetails.Height - VB6.TwipsToPixelsY(ACTwice * ACListViewTop)
                End With

                'DC080606 Add Coinsurer Details for Datasure
                If m_ShowCoInsurers Then

                    txtPercentageAllocated.Top = VB6.TwipsToPixelsY(ACListViewTop)
                    lblPercentageAllocated.Top = VB6.TwipsToPixelsY(ACListViewTop)
                    txtTotalAllocated.Top = VB6.TwipsToPixelsY(ACListViewTop)
                    lblTotalAllocated.Top = VB6.TwipsToPixelsY(ACListViewTop)

                    With lvwCoinsurers
                        ' Set list view's tops, left, height & width
                        .Top = VB6.TwipsToPixelsY(ACListViewTop + 400)
                        .Left = VB6.TwipsToPixelsX(ACCtrlVerticalSpacing)
                        .Width = fraCoInsurers.Width - VB6.TwipsToPixelsX(ACThrice * ACCtrlVerticalSpacing)
                        .Height = VB6.TwipsToPixelsY((VB6.PixelsToTwipsY(fraCoInsurers.Height) - (ACTwice * ACListViewTop)) - 400)
                    End With
                End If

                ' position the edit button
                With cmdEdit
                    .Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(fraReserveDetails.Width) - ACCtrlVerticalSpacing - ACCommandButtonWidth)
                    .Width = VB6.TwipsToPixelsX(ACCommandButtonWidth)
                    .Top = VB6.TwipsToPixelsY(ACListViewTop)
                End With
            Else
                'resize the list view
                With lstviewReserve
                    ' Set list view's tops, left, height & width
                    .Top = VB6.TwipsToPixelsY(ACListViewTop)
                    .Left = VB6.TwipsToPixelsX(ACCtrlVerticalSpacing)
                    .Width = fraReserveDetails.Width - VB6.TwipsToPixelsX(ACTwice * ACCtrlVerticalSpacing)
                    If Not m_ShowCoInsurers Then
                        .Height = fraReserveDetails.Height - VB6.TwipsToPixelsY(ACTwice * ACListViewTop)
                    Else
                        .Height = VB6.TwipsToPixelsY((VB6.PixelsToTwipsY(fraReserveDetails.Height) - (ACTwice * ACListViewTop)) / 2)
                    End If
                End With

                'DC080606 Add Coinsurer Details for Datasure
                'resize the list view
                If m_ShowCoInsurers Then

                    txtPercentageAllocated.Top = VB6.TwipsToPixelsY(ACListViewTop)
                    lblPercentageAllocated.Top = VB6.TwipsToPixelsY(ACListViewTop)
                    txtTotalAllocated.Top = VB6.TwipsToPixelsY(ACListViewTop)
                    lblTotalAllocated.Top = VB6.TwipsToPixelsY(ACListViewTop)

                    With lvwCoinsurers
                        ' Set list view's tops, left, height & width
                        .Top = VB6.TwipsToPixelsY(ACListViewTop + 400)
                        .Left = VB6.TwipsToPixelsX(ACCtrlVerticalSpacing)
                        .Width = fraCoInsurers.Width - VB6.TwipsToPixelsX(ACTwice * ACCtrlVerticalSpacing)
                        .Height = VB6.TwipsToPixelsY((VB6.PixelsToTwipsY(fraCoInsurers.Height) - (ACTwice * ACListViewTop)) - 400)
                    End With
                End If

            End If

        Catch
        End Try



    End Sub

    Private Sub UserControl_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        cmdEdit.Visible = m_ShowEdit
        'DC080606 Add Coinsurer Details for Datasure
        fraCoInsurers.Visible = m_ShowCoInsurers
        fraReserveDetails.Enabled = m_Enabled
    End Sub

    'developer guide no. 1 of No Solutions
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)

        PropBag.WriteProperty("BackColor", m_BackColor, m_def_BackColor)

        PropBag.WriteProperty("ForeColor", m_ForeColor, m_def_ForeColor)

        PropBag.WriteProperty("Enabled", m_Enabled, m_def_Enabled)


        'developer guide no. 2 of No Solutions
        'PropBag.WriteProperty("Font", m_Font, Ambient.Font)
        PropBag.WriteProperty("Font", m_Font, MyBase.Font)

        PropBag.WriteProperty("BackStyle", m_BackStyle, m_def_BackStyle)

        PropBag.WriteProperty("BorderStyle", m_BorderStyle, m_def_BorderStyle)

        PropBag.WriteProperty("ShowEdit", m_ShowEdit, m_def_ShowEdit)

        PropBag.WriteProperty("ShowCoInsurers", m_ShowCoInsurers, m_def_ShowCoInsurers)

        PropBag.WriteProperty("Visible", m_Visible, m_def_Visible)
    End Sub

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Static bIsInitialised As Boolean

        Dim sTitle, sMessage As String
        Dim vReturn, sHelpFile As String
        Dim m_lReturn As gPMConstants.PMEReturnCode
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if already initialised
            If bIsInitialised Then
                Return result
            End If

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

            ' If UserID is 0 assume that user cancelled logon
            If g_oObjectManager.UserID = 0 Then
                ' Exit application
                result = gPMConstants.PMEReturnCode.PMFalse
                g_oObjectManager = Nothing
                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_iUserId = .UserID
            End With

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrieve Helpfile", Application.ProductName)
                Return result
            End If

            If sHelpFile <> "" Then
                'App.HelpFile = sHelpFile
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMPeril.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            Else
                ' initialise this new object

                m_lReturn = m_oBusiness.Initialise(sUsername:=g_oObjectManager.UserName, sPassword:=g_oObjectManager.Password, iUserID:=g_iUserId, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_oObjectManager.CurrencyID, iLogLevel:=g_oObjectManager.LogLevel, sCallingAppName:=ACApp)
            End If
            ''Start (Saurabh Agrawal) Tech Spec Claims Recovery Reinsurance
            ''Get the Product option value for RI 2007 Enabled
            m_lReturn = CType(iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=gPMConstants.SIRBCHHeadOffice, r_vUnderwriting:=vReturn), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Initialse", "GetproductOptionValue Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If vReturn = "1" Then
                m_bIsRI2007Enabled = True
            End If
            ''End (Saurabh Agrawal) Tech Spec Claims Recovery Reinsurance
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' hold Initialised status
            bIsInitialised = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: LoadControl
    '
    ' Description: Does all the extra stuff that initialise doesn't
    '
    ' ***************************************************************** '
    Public Function LoadControl() As Integer

        Dim result As Integer = 0
        Dim sTemp As String = ""
        'Const kMethodName As String = "LoadControl"
        ' Forms load event.

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl")

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}
            '    m_oBusiness.ClaimID = m_lClaimId
            ' {* USER DEFINED CODE (End) *}

            m_oFormFields = New iPMFormControl.FormFields()

            ' Validate fields using Forms Control
            '    m_lReturn& = SetFieldValidation()


            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_bAllowNegativeReserve = True

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load control", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}
            m_lReturn = CType(SetupListview(), gPMConstants.PMEReturnCode)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.

            ' set the caption for the frame

            fraReserveDetails.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReserveFrame, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' set the caption for the button

            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetReserveType
    '
    ' Description: Gets the Types of reserves
    '
    ' RWH(07/02/2001) RSAB #218 Remove Salvage/Third Party from Total
    '                 Initial Reserve for Underwriting.
    ' ***************************************************************** '

    Private Function GetReserveType() As Integer
        Dim result As Integer = 0
        Dim r_vReserveTypeArray(,) As Object

        Try
            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetReserveType(r_vReserveTypeArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'resize the array based on the number of reserve types

            If Information.IsArray(r_vReserveTypeArray) And Not Object.Equals(r_vReserveTypeArray, Nothing) Then

                ReDim m_ListViewArray(r_vReserveTypeArray.GetUpperBound(1) + 4, m_colAverage)
            Else
                ReDim m_ListViewArray(3, m_colAverage)
            End If

            ' fill the descriptions and tag values
            m_ListViewArray(m_ListViewArray.GetLowerBound(0), m_colTag) = 1
            m_ListViewArray(m_ListViewArray.GetLowerBound(0), m_colDescription) = "Total"
            m_ListViewArray(m_ListViewArray.GetLowerBound(0) + 1, m_colDescription) = ""
            m_ListViewArray(m_ListViewArray.GetUpperBound(0) - 1, m_colTag) = 0
            m_ListViewArray(m_ListViewArray.GetUpperBound(0) - 1, m_colDescription) = "Salvage Recovery"
            m_ListViewArray(m_ListViewArray.GetUpperBound(0), m_colTag) = 0
            m_ListViewArray(m_ListViewArray.GetUpperBound(0), m_colDescription) = "T.P Recovery"

            ' get the specific reserve types

            If Not Information.IsArray(r_vReserveTypeArray) Or Object.Equals(r_vReserveTypeArray, Nothing) Then

            Else

                For lCount As Integer = r_vReserveTypeArray.GetLowerBound(1) To r_vReserveTypeArray.GetUpperBound(1)

                    m_ListViewArray(lCount + 2, m_colDescription) = r_vReserveTypeArray(1, lCount)
                Next lCount
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the GetReserveType", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetTotalValuesBR
    '
    ' Description: populates the total row in the reserve listview
    '
    ' ***************************************************************** '


    'Private Function GetTotalValuesBR() As Integer
    'Dim result As Integer = 0
    'Dim iCount As Integer
    'Dim cTotal As Decimal
    'Dim lAvg As Integer
    '
    'Try 
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get the total Values in the Reserve Listview
    'For 'lCount1 As Integer = 1 To 5
    'cTotal = 0
    'iCount = 0
    'If lstviewReserve.Items.Count = 0 Then 'Return result
    '
    'For 'lCount As Integer = 3 To lstviewReserve.Items.Count

    'If lCount = CDbl(v_vReserveTotalArray(iCount)) Then
    'DC140302
    'If ListViewHelper.GetListViewSubItem(lstviewReserve.Items.Item(lCount - 1), lCount1).Text = "" Then
    'ListViewHelper.GetListViewSubItem(lstviewReserve.Items.Item(lCount - 1), lCount1).Text = "0.00"
    'End If
    'cTotal += CDbl(ListViewHelper.GetListViewSubItem(lstviewReserve.Items.Item(lCount - 1), lCount1).Text)
    'iCount += 1
    'End If
    'Next lCount
    '
    'ListViewHelper.GetListViewSubItem(lstviewReserve.Items.Item(0), lCount1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(cTotal))
    '
    'Next lCount1
    '
    ' get the Average Values in the Reserve List View
    'iCount = 0
    'For 'lCount As Integer = 3 To lstviewReserve.Items.Count
    'If CDec(ListViewHelper.GetListViewSubItem(lstviewReserve.Items.Item(lCount - 1), 4).Text) <> 0 Then

    'If CDbl(v_vReserveTotalArray(iCount)) = lCount Then
    'lAvg = CInt((CDec(ListViewHelper.GetListViewSubItem(lstviewReserve.Items.Item(lCount - 1), 5).Text) / CDec(ListViewHelper.GetListViewSubItem(lstviewReserve.Items.Item(lCount - 1), 4).Text)) * 100)
    'End If
    'If lAvg < 100 And lAvg > 0 Then
    'lAvg = 100
    'End If
    'ListViewHelper.GetListViewSubItem(lstviewReserve.Items.Item(lCount - 1), 6).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, CStr(lAvg))
    '
    'Else
    'ListViewHelper.GetListViewSubItem(lstviewReserve.Items.Item(lCount - 1), 6).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, CStr(0))
    'End If
    'Next lCount
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the GetTotalValuesBR", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTotalValuesBR", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetTotalValues
    '
    ' Description: populates the total row in the reserve listview
    '
    ' ***************************************************************** '

    Private Function GetTotalValues() As Integer
        Dim result As Integer = 0
        Try
            Dim cTotal, cAvg As Decimal

            result = gPMConstants.PMEReturnCode.PMTrue

            'DC080606 Add Coinsurer Details for Datasure
            m_cTotalCurrentReserve = 0
            m_cTotalRevisedReserve = 0

            ' calculate the totals, column by column
            For iCol As Integer = m_ListViewArray.GetLowerBound(1) + 2 To m_ListViewArray.GetUpperBound(1) - 1
                cTotal = 0
                For iRow As Integer = m_ListViewArray.GetLowerBound(0) To m_ListViewArray.GetUpperBound(0)
                    ' ignore totals, salvage, 3rd party & blank rows
                    If Conversion.Val(CStr(m_ListViewArray(iRow, m_colTag))) > 1 Then
                        cTotal += CDbl(m_ListViewArray(iRow, iCol))
                    End If
                    ''Start(Saurabh Agrawal) Tech Spec - Claim Recovery Reinsurance
                    If m_bIsRI2007Enabled Then
                        'Start (Sriram P)PN 54072
                        If iCol = m_colIncurred And Conversion.Val(CStr(m_ListViewArray(iRow, m_colTag))) <= 1 And iRow > 1 Then
                            'End (Sriram P)PN 54072
                            cTotal = CDec(cTotal - CDbl(m_ListViewArray(iRow, iCol)))
                        End If
                    End If
                    ''End(Saurabh Agrawal) Tech Spec - Claim Recovery Reinsurance
                Next iRow


                m_ListViewArray(m_ListViewArray.GetLowerBound(0), iCol) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(cTotal))
                'DC080606 Add Coinsurer Details for Datasure
                If iCol = 5 Then
                    m_cTotalCurrentReserve = cTotal
                End If
                If iCol = 4 Then
                    m_cTotalRevisedReserve = cTotal
                End If




            Next iCol

            ' get the Average Values
            For iRow As Integer = 2 To m_ListViewArray.GetUpperBound(0)
                If CDec(m_ListViewArray(iRow, m_colIncurred)) <> 0 And CDec(m_ListViewArray(iRow, m_colSumInsured)) <> 0 Then
                    cAvg = (CDec(m_ListViewArray(iRow, m_colIncurred)) / CDec(m_ListViewArray(iRow, m_colSumInsured))) * 100
                    cAvg = Math.Round(cAvg, 0)
                    m_ListViewArray(iRow, m_colAverage) = CStr(cAvg) & "%"
                Else
                    m_ListViewArray(iRow, m_colAverage) = "0%"
                End If
            Next iRow

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the GetTotalValues", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTotalValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRecoveryDetails
    '
    ' Description: Gets the details for a particular Recovery
    '
    ' Hist :
    ' ***************************************************************** '

    Private Function GetRecoveryDetails() As Integer

        Dim result As Integer = 0
        Dim r_vRecoveryDetailsArray(,) As Object
        Dim lCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' call the function to collect the details for the Recovery type Salvage


            r_vRecoveryDetailsArray = Nothing

            m_lReturn = m_oBusiness.GetRecoveryDetails(1, r_vRecoveryDetailsArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If


            If Information.IsArray(r_vRecoveryDetailsArray) And Not Object.Equals(r_vRecoveryDetailsArray, Nothing) Then
                ' get the salvage details

                If CStr(r_vRecoveryDetailsArray(g_cIRecoveryDAinitialreserve, 0)) = "" Then

                    r_vRecoveryDetailsArray(g_cIRecoveryDAinitialreserve, 0) = 0
                End If

                If CStr(r_vRecoveryDetailsArray(g_cIRecoveryDArevisedreserve, 0)) = "" Then

                    r_vRecoveryDetailsArray(g_cIRecoveryDArevisedreserve, 0) = 0
                End If

                If CStr(r_vRecoveryDetailsArray(g_cIRecoveryDApaidtodate, 0)) = "" Then

                    r_vRecoveryDetailsArray(g_cIRecoveryDApaidtodate, 0) = 0
                End If

                lCount = m_ListViewArray.GetUpperBound(0) - 1


                m_ListViewArray(lCount, m_colInitial) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(r_vRecoveryDetailsArray(g_cIRecoveryDAinitialreserve, 0)))

                m_ListViewArray(lCount, m_colRevisionAmount) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(r_vRecoveryDetailsArray(g_cIRecoveryDArevisedreserve, 0)))

                m_ListViewArray(lCount, m_colThisRevision) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(0))



                m_ListViewArray(lCount, m_colCurrentReserve) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(CDec(r_vRecoveryDetailsArray(g_cIRecoveryDAinitialreserve, 0)) + CDec(r_vRecoveryDetailsArray(g_cIRecoveryDArevisedreserve, 0)) - CDec(r_vRecoveryDetailsArray(g_cIRecoveryDApaidtodate, 0))))

                ''Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
                ''Check for RI 2007 if enabled display Incurred as sum of “Initial Reserve” + “Revision Amount” + "RecevivedToDate"
                ''Else Display in the existing way

                If m_bIsRI2007Enabled Then

                    m_ListViewArray(lCount, m_colIncurred) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(CDec(r_vRecoveryDetailsArray(g_cIRecoveryDApaidtodate, 0)) * -1))
                Else


                    m_ListViewArray(lCount, m_colIncurred) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(CDec(r_vRecoveryDetailsArray(g_cIRecoveryDAinitialreserve, 0)) + CDec(r_vRecoveryDetailsArray(g_cIRecoveryDArevisedreserve, 0))))
                End If
                m_ListViewArray(lCount, m_colSumInsured) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(0))


                m_ListViewArray(lCount, m_colAverage) = "0.00"

            End If

            ' call the function to collect the details for the Recovery Type Third Party

            m_lReturn = m_oBusiness.GetRecoveryDetails(0, r_vRecoveryDetailsArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If


            If Information.IsArray(r_vRecoveryDetailsArray) And Not Object.Equals(r_vRecoveryDetailsArray, Nothing) Then
                lCount = m_ListViewArray.GetUpperBound(0)

                If CStr(r_vRecoveryDetailsArray(g_cIRecoveryDAinitialreserve, 0)) = "" Then

                    r_vRecoveryDetailsArray(g_cIRecoveryDAinitialreserve, 0) = 0
                End If

                If CStr(r_vRecoveryDetailsArray(g_cIRecoveryDArevisedreserve, 0)) = "" Then

                    r_vRecoveryDetailsArray(g_cIRecoveryDArevisedreserve, 0) = 0
                End If

                If CStr(r_vRecoveryDetailsArray(g_cIRecoveryDApaidtodate, 0)) = "" Then

                    r_vRecoveryDetailsArray(g_cIRecoveryDApaidtodate, 0) = 0
                End If


                m_ListViewArray(lCount, m_colInitial) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(r_vRecoveryDetailsArray(g_cIRecoveryDAinitialreserve, 0)))

                m_ListViewArray(lCount, m_colRevisionAmount) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(r_vRecoveryDetailsArray(g_cIRecoveryDArevisedreserve, 0)))

                m_ListViewArray(lCount, m_colThisRevision) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(0))



                m_ListViewArray(lCount, m_colCurrentReserve) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(CDec(r_vRecoveryDetailsArray(g_cIRecoveryDAinitialreserve, 0)) + CDec(r_vRecoveryDetailsArray(g_cIRecoveryDArevisedreserve, 0)) - CDec(r_vRecoveryDetailsArray(g_cIRecoveryDApaidtodate, 0))))
                ''Start(Saurabh Agrawal) Tech Spec QBENZCR004 Claim Recovery Reinsurance
                ''Check for RI 2007 if enabled display Incurred as sum of “Initial Reserve” + “Revision Amount” + "RecevivedToDate"
                ''Else Display in the existing way

                If m_bIsRI2007Enabled Then

                    m_ListViewArray(lCount, m_colIncurred) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(CDec(r_vRecoveryDetailsArray(g_cIRecoveryDApaidtodate, 0)) * -1))
                Else


                    m_ListViewArray(lCount, m_colIncurred) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(CDec(r_vRecoveryDetailsArray(g_cIRecoveryDAinitialreserve, 0)) + CDec(r_vRecoveryDetailsArray(g_cIRecoveryDArevisedreserve, 0))))
                End If

                m_ListViewArray(lCount, m_colSumInsured) = "0.00"
                m_ListViewArray(lCount, m_colAverage) = "0.00"



            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the GetRecoveryDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRecoveryDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'Resume
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetReserveDetails
    '
    ' Description: Gets the reserve details for a particular Peril and Claim
    '
    ' ***************************************************************** '

    Private Function GetReserveDetails() As Integer
        Dim result As Integer = 0
        Dim vReserveDetailsArray(,) As Object

        Try
            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetReserveDetails(m_lInsurance_file_cnt, m_lRiskID, vReserveDetailsArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            ' Add the values in the array to the listview control

            If Not Information.IsArray(vReserveDetailsArray) Or Object.Equals(vReserveDetailsArray, Nothing) Then
                ' reset the array
                g_vReserveDetails = Nothing
                Return result
            End If

            ' Add the values to the List view

            For lCount As Integer = vReserveDetailsArray.GetLowerBound(1) To vReserveDetailsArray.GetUpperBound(1) ' lstviewReserve.ListItems.Count
                ' replace any blank values with zeros

                If CStr(vReserveDetailsArray(g_cIRDAinitialreserve, lCount)) = "" Then

                    vReserveDetailsArray(g_cIRDAinitialreserve, lCount) = 0
                End If

                If CStr(vReserveDetailsArray(g_cIRDArevisedreserve, lCount)) = "" Then

                    vReserveDetailsArray(g_cIRDArevisedreserve, lCount) = 0
                End If

                If CStr(vReserveDetailsArray(g_cIRDAsuminsured, lCount)) = "" Then

                    vReserveDetailsArray(g_cIRDAsuminsured, lCount) = 0
                End If

                If CStr(vReserveDetailsArray(g_cIRDApaidtodate, lCount)) = "" Then

                    vReserveDetailsArray(g_cIRDApaidtodate, lCount) = 0
                End If

                If CStr(vReserveDetailsArray(g_cIRDAsuminsured, lCount)) = "" Then

                    vReserveDetailsArray(g_cIRDAsuminsured, lCount) = 0
                End If

                If CStr(vReserveDetailsArray(g_cIRDAaverage, lCount)) = "" Then

                    vReserveDetailsArray(g_cIRDAaverage, lCount) = 0
                End If

                ' fill in the values in the List View array

                m_ListViewArray(lCount + 2, m_colTag) = vReserveDetailsArray(g_cIRDAreserveid, lCount)

                If CStr(vReserveDetailsArray(g_cIRDAreserveid, lCount)) = "" Then Throw New Exception()
                ' initial reserve

                m_ListViewArray(lCount + 2, m_colInitial) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(vReserveDetailsArray(g_cIRDAinitialreserve, lCount)))
                ' revision amount

                m_ListViewArray(lCount + 2, m_colRevisionAmount) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(vReserveDetailsArray(g_cIRDAlastversionrevisedreserve, lCount)))
                'ECK Oct 2005  Bring Broking display into line with Underwriting
                ' This Revision

                m_ListViewArray(lCount + 2, m_colThisRevision) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(vReserveDetailsArray(g_cIRDArevisedentered, lCount)))

                ' Current Reserve



                m_ListViewArray(lCount + 2, m_colCurrentReserve) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(CDec(vReserveDetailsArray(g_cIRDArevisedreserve, lCount)) + CDec(vReserveDetailsArray(g_cIRDAinitialreserve, lCount)) - CDec(vReserveDetailsArray(g_cIRDApaidtodate, lCount))))
                ' incurred


                m_ListViewArray(lCount + 2, m_colIncurred) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(CDec(vReserveDetailsArray(g_cIRDArevisedreserve, lCount)) + CDec(vReserveDetailsArray(g_cIRDAinitialreserve, lCount))))
                ' average

                m_ListViewArray(lCount + 2, m_colAverage) = vReserveDetailsArray(g_cIRDAaverage, lCount)
                ' sum insured

                m_ListViewArray(lCount + 2, m_colSumInsured) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CStr(vReserveDetailsArray(g_cIRDAsuminsured, lCount)))
            Next lCount

            'keep track of original values


            g_vReserveDetails = vReserveDetailsArray

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the GetReserveDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPaymentDetails
    '
    ' Description: Gets the Payment Details for the Particular Reserve
    '
    ' Hist :
    ' ***************************************************************** '

    Public Function GetPaymentDetails() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' call the function to collect the details for the Payment type Salvage


            g_vPaymentDetails = DBNull.Value


            m_lReturn = m_oBusiness.GetPaymentDetails(g_vPaymentDetails)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the GetPaymentDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If


            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the GetPaymentDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
                If m_oFormFields IsNot Nothing Then
                    m_oFormFields.Dispose()

                End If


                m_oFormFields = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    Private Function KeepWindowOnTop(ByVal bKeepOnTop As Boolean) As Integer

        Try


            m_lReturn = CType(iPMFunc.SetWindowPlacement(m_lParentHwnd, bKeepOnTop), gPMConstants.PMEReturnCode)
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then Throw New Exception



        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="KeepWindowOnTop function failed", vApp:=ACApp, vClass:=ACClass, vMethod:="KeepWindowOnTop", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
        End Try
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    ' ***************************************************************** '
    ' Name: GetReserveGridInArray
    ' Description: Returns reserve Details to be manipulated through logic scripts
    ' vReserveArray 0 Reserve_id (for matching)
    '               1 Reserve Type
    '               2 Revision
    ' ***************************************************************** '
    Public Function GetReserveGridInArray(ByRef vReserveArray(,) As Object) As Integer
        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(m_vReserveDetailsArray) Then
                ReDim vReserveArray(2, m_vReserveDetailsArray.GetUpperBound(1))

                For cnt As Integer = 0 To m_vReserveDetailsArray.GetUpperBound(1)


                    vReserveArray(0, cnt) = g_vReserveDetails(g_cIRDAreserveid, cnt)


                    vReserveArray(1, cnt) = g_vReserveDetails(g_cIRDAreservetype, cnt)


                    vReserveArray(2, cnt) = g_vReserveDetails(g_cIRDAinitialreserve, cnt)
                Next cnt
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReserveGridInArray function failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveGridInArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            'Tidy Up code goes here
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SaveScriptArrayToReserve
    ' Description: Save reserve Details to reserve control (for logic scripts)
    ' vReserveArray 0 Reserve_id (for matching)
    '               1 Reserve Type
    '               2 Revision
    ' ***************************************************************** '
    Public Function SaveScriptArrayToReserve(ByVal vReserveArray(,) As Object) As Integer
        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(vReserveArray) And (m_sTransactionType = "C_CO" Or m_sTransactionType = "C_CR") Then
                'Update listview array
                For cntr As Integer = 0 To m_ListViewArray.GetUpperBound(0) - 4

                    If m_ListViewArray(cntr + 2, m_colTag).Equals(vReserveArray(0, cntr)) Then
                        lstviewReserve.Items.Item(cntr + 2).Selected = True
                        If m_sTransactionType = "C_CO" Then

                            m_lReturn = CType(RecalcReserves(cInitialReserve:=CDec(vReserveArray(2, cntr))), gPMConstants.PMEReturnCode)
                        Else

                            m_lReturn = CType(RecalcReserves(cNewRevision:=CDec(vReserveArray(2, cntr))), gPMConstants.PMEReturnCode)
                        End If
                    End If
                Next cntr
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveScriptArrayToReserve function failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveScriptArrayToReserve", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            'Tidy Up code goes here
            Return result
        End Try
    End Function

    Private Function GetProductDetails() As Integer
        Dim result As Integer = 0
        Dim bSIRProduct As Object

        Const kMethodName As String = "GetProductDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim o_ProductBusiness As bSIRProduct.Business
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_o_ProductBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_o_ProductBusiness, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            o_ProductBusiness = temp_o_ProductBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRProduct.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            lReturn = o_ProductBusiness.GetProductLevelOptionsForClaim(v_lClaimID:=m_lClaimID, r_bAllow_Negative_Reserve:=m_bAllowNegativeReserve)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetProductDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If




        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally




        End Try
        Return result
    End Function
    ''' <summary>
    ''' GetIsReservesReadOnly
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetIsReservesReadOnly()
        Dim nProductID As Integer
        Dim oProduct As bSIRProduct.Business = Nothing
        Dim oIsReservesReadonly(,) As Object = Nothing
        If g_oObjectManager.GetInstance(oProduct, "bSIRProduct.Business", PMGetViaClientManager) <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Failed to get object Instance - bSIRProduct.Busines")
        End If

        If oProduct.GetProductid(ifilecnt:=Insurance_File_Cnt, vProduct_id:=nProductID) <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Failed to execute oProduct.GetProductid")
        End If
        If oProduct.GetProductValue(nProductID, "is_reserves_read_only", oIsReservesReadonly) <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Failed to execute oProduct.GetProductValue")
        End If

        m_bIsReservesReadOnly = CStr(oIsReservesReadonly(0, 0)) = "1"

        oProduct.Dispose()
        oProduct = Nothing
        oIsReservesReadonly = Nothing

    End Sub

    Private Function CheckRevisionWithinUserLimit(ByVal cInitialReserve As Decimal, ByVal cRevisedReserve As Decimal) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If (m_sTransactionType = "C_CO" And (m_ListViewArray(0, m_colCurrentReserve) + cInitialReserve) > m_dUserReserveLimit) Then
                m_bReserveLimitExceeded = True
                m_dExceededReserve = m_ListViewArray(0, m_colCurrentReserve) + cInitialReserve
            ElseIf (m_sTransactionType = "C_CR" And (m_ListViewArray(0, m_colCurrentReserve) + cRevisedReserve) > m_dUserReserveLimit) Then
                m_bReserveLimitExceeded = True
                m_dExceededReserve = m_ListViewArray(0, m_colCurrentReserve) + cRevisedReserve
            End If

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckRevisionWithinUserLimit validation failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckRevisionWithinUserLimit", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        End Try
        Return result
    End Function
End Class