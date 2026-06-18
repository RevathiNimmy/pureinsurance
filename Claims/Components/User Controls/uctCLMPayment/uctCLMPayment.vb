Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no 129. 
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("uctCLMPayment_NET.uctCLMPayment")> _
Partial Public Class uctCLMPayment
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    Public Event VisibleChange()
    Public Event ShowEditChange()
    Public Event EnabledChange()

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "uctCLMPaymentControl"

    'Default Property Values:
    Private Const m_def_BackColor As Integer = 0
    Private Const m_def_ForeColor As Integer = 0
    Private Const m_def_BackStyle As Integer = 0
    Private Const m_def_BorderStyle As Integer = 0
    Private Const m_def_ShowEdit As Boolean = True
    Private Const m_def_Enabled As Boolean = True
    Private Const m_def_Visible As Boolean = True

    ' column declarations for the listviewarray
    Private Const m_colTag As Integer = 0
    Private Const m_colDescription As Integer = 1
    Private Const m_colInitial As Integer = 2
    Private Const m_colRevisionAmount As Integer = 3
    Private Const m_colPaidToDate As Integer = 4
    Private Const m_colThisPaymentLossCurrency As Integer = 5
    Private Const m_colCurrentReserve As Integer = 6
    Private Const m_colIncurred As Integer = 7
    Private Const m_colAverage As Integer = 8

    Private Const m_colRiskType As Integer = 9
    Private Const m_colThisPayment As Integer = 10
    Private Const m_colLossCurrencyName As Integer = 11
    Private Const m_colPaymentCurrencyName As Integer = 12
    Private Const m_colTaxTypeCode As Integer = 13
    Private Const m_colTaxBand As Integer = 14
    Private Const m_colTaxValue As Integer = 15
    Private Const m_colTotalInclTax As Integer = 16
    Private Const m_colCurrencyRatePayToLoss As Integer = 17
    Private Const m_colPaymentCurrencyID As Integer = 18
    Private Const m_colReserveAdjustment As Integer = 19
    'Next line must match the total "virtual" columns
    Private Const m_colTotalCount As Integer = 19

    ' column declarations for the listview control
    Private Const m_lstInitialReserve As Integer = 1
    Private Const m_lstRevisionAmount As Integer = 2
    Private Const m_lstPaidToDate As Integer = 3
    Private Const m_lstThisPayment As Integer = 4
    Private Const m_lstCurrentReserve As Integer = 5
    Private Const m_lstIncurred As Integer = 6

    'constants for data output to uctRiskScreen (and then to Reserve control)
    Private Const clPaymentIDColumn As Integer = 0
    Private Const clPaidToDateColumn As Integer = 1
    Private Const clPaymentAmountColumn As Integer = 2
    Private Const clReserveIDColumn As Integer = 3
    Private Const clReserveTypeDescColumn As Integer = 4
    Private Const clReserveAdjustmentColumn As Integer = 5

    'Property Variables:
    Private m_BackColor As Integer
    Private m_ForeColor As Integer
    Private m_Enabled As Boolean
    Private m_Font As Font
    Private m_BackStyle As Integer
    Private m_BorderStyle As Integer
    Private m_ShowEdit As Boolean
    Private m_Visible As Boolean
    Private m_bLossSchedule As Boolean 'If enabled, Loss Schedule Will handle payments PS202
    Private m_vThisPayment As Object
    Private m_lMainResvisionId As Integer
    Private m_vReserveTotalArray() As Object
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lReturn As Integer
    Private m_oBusiness As Object
    Private m_oFormFields As iPMFormControl.FormFields
    Private m_vReserveDetailsArray As Object
    Private m_vPaymentDetailArray(,) As Object
    Private m_ListViewArray(,) As Object
    Private m_lAllowNegativeReserve As gPMConstants.PMEReturnCode
    Private m_dCurrencyRatePayToLoss As Double
    Private m_sLossCurrencyName As String = ""
    Private m_vReserveDetails(,) As Object 'to keep track of original values
    Private m_vPaymentDetails(,) As Object
    Private m_cTotalPaymentinPaymentCurrency As Decimal
    Private m_cTotalPaymentinLossCurrency As Decimal
    Private m_sComments As String = ""
    Private m_lPayeeMediaType As Integer
    Private m_sPayeeName As String = ""
    Private m_sPayeeBankName As String = ""
    Private m_sPayeeSortCode As String = ""
    Private m_sPayeeAccountNo As String = ""
    Private m_lPayeeCountry As Integer
    Private m_sPayeeComments As String = ""
    Private m_lPaymentCurrencyID As Integer
    Private m_lCurrencyRatePayToLoss As Double
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lPerilID As Integer
    Private m_lClaimId As Integer
    Private m_lPerilTypeID As Integer
    Private m_sRisktype As String = ""
    Private m_lPartycnt As Integer
    Private m_lRiskID As Integer
    Private m_bStatus As Boolean
    Private m_bIsPostTaxes As Boolean

    ' returns a variant of the data now shown by this control
    'RVH - 27/09/2002   Changed from UDT to VARIANT
    Public Event DataHasChanged(ByVal Sender As Object, ByVal e As DataHasChangedEventArgs)

    Public Structure udtDetails
        Dim lPaymentID As Integer
        Dim cPaidToDate As Decimal
        Dim cPaymentAmount As Decimal
        Dim lReserveID As Integer
        Dim sReserveTypeDesc As String
        Public Shared Function CreateInstance() As udtDetails
            Dim result As New udtDetails
            result.sReserveTypeDesc = String.Empty
            Return result
        End Function
    End Structure

    Public Structure udtReserveDetails
        Dim lReserveID As Integer
        Dim lPaymentID As Integer
        Dim cTotalReserve As Decimal
        Dim cInitialReserve As Decimal
    End Structure

    '=================
    'Public Properties
    '=================
    <Browsable(True)> _
    Public Shadows Property Enabled() As Boolean
        Get
            Return m_Enabled
        End Get
        Set(ByVal Value As Boolean)
            m_Enabled = Value
            fraPaymentDetails.Enabled = m_Enabled
            lstviewPayment.Enabled = m_Enabled
            RaiseEvent EnabledChange()
        End Set
    End Property


    <Browsable(True)> _
    Public Shadows Property Text() As Object
        Get

            Dim result As Object = Nothing
            Dim vTemp(,) As Object
            Dim iList As ListViewItem

            Try

                With lstviewPayment
                    ReDim vTemp(.Items.Count, .Columns.Count)
                    ' get the headers

                    vTemp(0, 0) = ""
                    For iRow As Integer = 1 To .Columns.Count

                        vTemp(0, iRow) = .Columns.Item(iRow - 1).Text.Trim()
                    Next iRow


                    For iRow As Integer = 1 To vTemp.GetUpperBound(0)
                        iList = .Items.Item(iRow - 1)
                        ' save the payment id in the first column
                        ' ids are only valid if > 1
                        If Conversion.Val(Convert.ToString(iList.Tag)) > 1 Then

                            vTemp(iRow, 0) = Conversion.Val(Convert.ToString(iList.Tag))
                        Else

                            vTemp(iRow, 0) = -1
                        End If

                        ' get the label for the row

                        vTemp(iRow, 1) = iList.Text

                        ' get the remaining fields

                        For iCol As Integer = 1 To vTemp.GetUpperBound(1) - 1
                            Dim dbNumericTemp As Double
                            If Double.TryParse(ListViewHelper.GetListViewSubItem(iList, iCol).Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                                vTemp(iRow, iCol + 1) = gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(iList, iCol).Text)
                            Else

                                vTemp(iRow, iCol + 1) = gPMFunctions.ToSafeCurrency(0)
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
            uctCLMPayment_Resize(Me, New EventArgs())

            RaiseEvent ShowEditChange()
        End Set
    End Property


    <Browsable(True)> _
    Public Property Visible_Renamed() As Boolean
        Get
            Return m_Visible
        End Get
        Set(ByVal Value As Boolean)

            m_Visible = Value
            fraPaymentDetails.Visible = m_Visible
            RaiseEvent VisibleChange()
        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property Insurance_File_Cnt() As Integer
        Get
            Return g_lInsurance_file_cnt
        End Get
    End Property
    <Browsable(False)> _
    Public ReadOnly Property PerilID() As Integer
        Get
            Return m_lPerilID
        End Get
    End Property
    <Browsable(False)> _
    Public ReadOnly Property ClaimId() As Integer
        Get
            Return m_lClaimId
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

    '===============
    'Private Methods
    '===============
    Private Function CalcAverage(ByRef iIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim sTemp As Single
        Try

            ' check sum insured <> 0
            If gPMFunctions.ToSafeCurrency(m_vReserveDetails(g_cIRDAsuminsured, iIndex - 2)) <= 0 Then
                Return result
            End If

            sTemp = gPMFunctions.ToSafeCurrency(m_ListViewArray(iIndex, m_colInitial)) + gPMFunctions.ToSafeCurrency(m_ListViewArray(iIndex, m_colRevisionAmount))

            sTemp /= (gPMFunctions.ToSafeCurrency(m_vReserveDetails(g_cIRDAsuminsured, iIndex - 2)))
            Return sTemp * 100

        Catch

            Return result


            Return result
        End Try
    End Function

    Private Function SaveReserveDetails() As Integer

        Dim result As Integer = 0
        Dim vReserves As Object
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vReserves(7, m_vReserveDetails.GetUpperBound(1))

            For iCnt As Integer = m_vReserveDetails.GetLowerBound(1) To m_vReserveDetails.GetUpperBound(1)
                ' user defined peril types only
                'reserve id

                vReserves(0, iCnt) = m_vReserveDetails(g_cIRDAreserveid, iCnt)
                'initial reserve

                vReserves(1, iCnt) = m_ListViewArray(iCnt + 2, m_colInitial)
                'average

                vReserves(3, iCnt) = CalcAverage(iCnt + 2) 'm_vReserveDetails(g_cIRDAaverage, iCnt)

                'paid to date = paid to date + this payment

                vReserves(4, iCnt) = gPMFunctions.ToSafeCurrency(m_ListViewArray(iCnt + 2, m_colPaidToDate)) + gPMFunctions.ToSafeCurrency(m_ListViewArray(iCnt + 2, m_colThisPaymentLossCurrency))

                'this payment

                vReserves(5, iCnt) = m_ListViewArray(iCnt + 2, m_colThisPaymentLossCurrency)

                'revise reserve = revise reserve + this revision

                vReserves(2, iCnt) = m_ListViewArray(iCnt + 2, m_colRevisionAmount)

                ' if open claim then set this revision = initial reserve
                If m_sTransactionType = "C_CO" Then
                    'this revision


                    vReserves(6, iCnt) = vReserves(1, iCnt)
                ElseIf m_sTransactionType = "C_CR" Then


                    vReserves(6, iCnt) = vReserves(2, iCnt)
                Else
                    ' check for negative reserve...
                    ' is total payment greater than total reserve
                    If (gPMFunctions.ToSafeCurrency(vReserves(4, iCnt)) > gPMFunctions.ToSafeCurrency(vReserves(1, iCnt)) + gPMFunctions.ToSafeCurrency(vReserves(2, iCnt))) And m_lAllowNegativeReserve = gPMConstants.PMEReturnCode.PMFalse Then
                        ' yes so update reserve figure...

                        vReserves(6, iCnt) = gPMFunctions.ToSafeCurrency(vReserves(4, iCnt)) - gPMFunctions.ToSafeCurrency(vReserves(1, iCnt)) - gPMFunctions.ToSafeCurrency(vReserves(2, iCnt))
                    Else
                        'this revision

                        vReserves(6, iCnt) = 0  'lstItem.SubItems(3)
                    End If
                End If

            Next iCnt


            m_lReturn = m_oBusiness.UpdateReserveDetails(vReserves)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save the reserve details", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveReserveDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save reserve details", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveReserveDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result



            Return result
        End Try
    End Function

    Private Function SetPayeeDetails(ByVal lPartyCnt As Integer, ByVal sComments As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            For iCnt As Integer = m_vPaymentDetailArray.GetLowerBound(1) To m_vPaymentDetailArray.GetUpperBound(1)
                m_vPaymentDetailArray(g_cIPDAPartyID, iCnt) = lPartyCnt
                m_vPaymentDetailArray(g_cIPDAComments, iCnt) = sComments
            Next iCnt

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No payment details to save", vApp:=ACApp, vClass:=ACClass, vMethod:="Save", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result


            Return result
        End Try
    End Function

    Public Function UpdateReserveValue(ByRef uNewValues As udtReserveDetails) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            With uNewValues
                If .lReserveID < 1 Then
                    Return result
                End If

                If lstviewPayment.Items.Count < 1 Then
                    Return result
                End If

                ' which payment was this reserve against
                With lstviewPayment
                    For iTemp As Integer = m_vReserveDetails.GetLowerBound(1) To m_vReserveDetails.GetUpperBound(1)
                        If Conversion.Val(CStr(m_vReserveDetails(g_cIRDAreserveid, iTemp))) = uNewValues.lReserveID Then
                            ' has the revised or initial reserve changed
                            If gPMFunctions.ToSafeCurrency(Conversion.Val(CStr(m_vReserveDetails(g_cIRDArevisedreserve, iTemp)))) <> uNewValues.cTotalReserve Or gPMFunctions.ToSafeCurrency(Conversion.Val(CStr(m_vReserveDetails(g_cIRDAinitialreserve, iTemp)))) <> uNewValues.cInitialReserve Then

                                m_lReturn = UpdateCurrentPayment(iTemp + 3, uNewValues.cTotalReserve, uNewValues.cInitialReserve)
                            End If
                            Exit For
                        End If
                    Next iTemp
                End With
            End With


            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save the reserve details", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePaymentValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    Public Function Save() As Integer

        Dim result As Integer = 0
        Dim sCOBCode As String = ""
        Dim lCOBId As Integer
        Dim sShortName, sComments, sCommentsOut As String
        Dim lButtonClicked As gPMConstants.PMEReturnCode
        Dim nOptionValue As Integer
        Dim cTaxAmount As Decimal
        Dim sTaxTypeCode As String = ""

        Try

            'CMG/PB 05122002 If enabled, Loss Schedule Will handle payments PS202
            'If LS is enabled then LS will have saved everything already
            If m_bLossSchedule Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If
            'End CMG

            If Not Information.IsArray(m_vPaymentDetailArray) Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No payment details to " & "save", vApp:=ACApp, vClass:=ACClass, vMethod:="Save")
                Return result
            End If

            m_lReturn = SaveReserveDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save the " & "reserve details", vApp:=ACApp, vClass:=ACClass, vMethod:="Save")
                Return result
            End If

            ' Total up the lines - supports paying multiple reserves at once
            m_cTotalPaymentinPaymentCurrency = 0
            m_cTotalPaymentinLossCurrency = 0
            cTaxAmount = 0
            For iCnt As Integer = m_vPaymentDetailArray.GetLowerBound(1) To m_vPaymentDetailArray.GetUpperBound(1)
                m_cTotalPaymentinPaymentCurrency += gPMFunctions.ToSafeCurrency(m_ListViewArray(iCnt + 2, m_colThisPayment))
                m_cTotalPaymentinLossCurrency += gPMFunctions.ToSafeCurrency(m_ListViewArray(iCnt + 2, m_colThisPaymentLossCurrency))
                cTaxAmount += gPMFunctions.ToSafeCurrency(m_ListViewArray(iCnt + 2, m_colTaxValue))
                sTaxTypeCode = CStr(m_vPaymentDetailArray(g_cIPDATaxTypeCode, iCnt))

            Next iCnt

            If m_cTotalPaymentinPaymentCurrency = 0 Then
                ' nothing changed so exit
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            ' what type of accounting system are we running

            m_lReturn = m_oBusiness.getOption(ACOptionNumber, nOptionValue)

            'get class of business

            m_lReturn = m_oBusiness.GetClassOfBusiness(r_lId:=lCOBId, r_sCode:=sCOBCode, v_lClaimPerilId:=m_lPerilID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get class of " & "business." & Constants.vbLf & "Transaction will not be posted to Orion", vApp:=ACApp, vClass:=ACClass, vMethod:="Save")
            End If

            If nOptionValue = ACOptionValueSuspense Then
                m_lReturn = SetPayeeDetails(lPartyCnt:=0, sComments:="")
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the " & "party details", vApp:=ACApp, vClass:=ACClass, vMethod:="Save")
                End If

                'Update the local array
                UpdatePaymentDetails()

                'Save the revised payment details to the database

                m_lReturn = m_oBusiness.UpdatePaymentDetails(m_vPaymentDetailArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save the " & "payment details", vApp:=ACApp, vClass:=ACClass, vMethod:="Save")
                    Return result
                End If


                If sCOBCode <> "" Then
                    m_lReturn = PostPaymentToOrion(v_lInsuranceFileCnt:=g_lInsurance_file_cnt, v_lClaimId:=m_lClaimId, v_lPerilID:=m_lPerilID, v_cPayAmount:=m_cTotalPaymentinPaymentCurrency, v_sCreditAccountCode:="CLMSUS" & sCOBCode, v_sCOBCode:=sCOBCode, v_lCOBId:=lCOBId, v_cTaxAmount:=cTaxAmount, v_sTaxTypeCode:=sTaxTypeCode)
                End If


            ElseIf nOptionValue <> ACOptionValueSuspense Then
                m_lReturn = GetPaymentPartyid(lPaymentPartyId:=m_lPartycnt, sOComments:=sComments, lButtonClicked:=lButtonClicked, sIComments:=sCommentsOut)

                If lButtonClicked = gPMConstants.PMEReturnCode.PMOK Then
                    m_lReturn = SetPayeeDetails(lPartyCnt:=m_lPartycnt, sComments:=sComments)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update " & "the party details", vApp:=ACApp, vClass:=ACClass, vMethod:="Save")
                    End If

                    'Update the local array
                    UpdatePaymentDetails()

                    'Save the revised payment details to the database

                    m_lReturn = m_oBusiness.UpdatePaymentDetails(m_vPaymentDetailArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save the " & "payment details", vApp:=ACApp, vClass:=ACClass, vMethod:="Save")
                        Return result
                    End If

                    If sCOBCode <> "" Then
                        If m_lPartycnt <> 0 Then
                            'get party name

                            m_lReturn = m_oBusiness.GetPartyName(v_lPartyCnt:=m_lPartycnt, v_sFieldName:="shortname", r_sResult:=sShortName)
                        Else
                            sShortName = "CLMPAYABLE"
                        End If

                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                            m_lReturn = PostPaymentToOrion(v_lInsuranceFileCnt:=g_lInsurance_file_cnt, v_lClaimId:=m_lClaimId, v_lPerilID:=m_lPerilID, v_cPayAmount:=m_cTotalPaymentinPaymentCurrency, v_sCreditAccountCode:=sShortName, v_sCOBCode:=sCOBCode, v_lCOBId:=lCOBId, v_lPartyCnt:=m_lPartycnt, v_cTaxAmount:=cTaxAmount, v_sTaxTypeCode:=sTaxTypeCode)
                        Else
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & "party name." & Constants.vbLf & "No payment transaction " & "will be posted to Orion", vApp:=ACApp, vClass:=ACClass, vMethod:="Save")
                        End If

                    End If ' UnderwritingOrAgency
                End If ' lbuttonclick
            End If ' option value


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save the payment " & "details", vApp:=ACApp, vClass:=ACClass, vMethod:="Save", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result


            Return result
        End Try
    End Function

    '******************************************************************************
    ' Name:         GetPaymentPartyid
    ' Description:  Instance PaymentMethod to retrieve Policyholder
    ' Date :        15/07/2000
    ' Edit History :Pandu
    '******************************************************************************
    Public Function GetPaymentPartyid(ByRef lPaymentPartyId As Integer, ByRef sOComments As String, ByRef lButtonClicked As Integer, Optional ByVal sIComments As String = "", Optional ByVal iCurrencyID As Integer = 0) As Integer

        Dim result As Integer = 0
        Const ACPaymentMethod As Integer = 0

        Dim oPaymentMethod As Object
        Dim r_vClaimClientAndAgent As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Find Party object
            Dim temp_oPaymentMethod As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPaymentMethod, sClassName:="iCLMPaymentMethod.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oPaymentMethod = temp_oPaymentMethod
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object " & "'iPMBFindParty.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentPartyid")
                Return result
            End If

            ' Set component properties and start interface

            oPaymentMethod.CallingAppName = ACApp

            oPaymentMethod.ScreenMethod = ACPaymentMethod

            oPaymentMethod.Amount = m_cTotalPaymentinPaymentCurrency

            oPaymentMethod.LossCurrencyAmount = m_cTotalPaymentinLossCurrency

            oPaymentMethod.Comments = sIComments

            oPaymentMethod.CurrencyId = m_lPaymentCurrencyID

            oPaymentMethod.ClaimId = m_lClaimId

            oPaymentMethod.InsuranceFileCnt = g_lInsurance_file_cnt

            oPaymentMethod.LossCurrencyID = g_lCurrencyID

            'DC030402 -start -added check for broking/underwriting


            'JMK 21/08/2001 set agent and party default properties

            m_lReturn = m_oBusiness.GetClaimClientAndAgent(m_lClaimId, r_vClaimClientAndAgent)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Claim " & "Client and Agent details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentPartyid")
                Return result
            End If

            ' JMK 24/08/2001 - amend slightly, no nulls

            If Strings.Len(CStr(r_vClaimClientAndAgent(0, 0))) = 0 Then

                oPaymentMethod.AgentID = 0
            Else


                oPaymentMethod.AgentID = r_vClaimClientAndAgent(0, 0)
            End If

            'CMG/PB 19122002 Bug fix,enables Party_cnt to populate in payment table

            If Strings.Len(CStr(r_vClaimClientAndAgent(1, 0))) = 0 Then

                oPaymentMethod.ClientID = 0
            Else


                oPaymentMethod.ClientID = r_vClaimClientAndAgent(1, 0)
            End If



            oPaymentMethod.AgentName = r_vClaimClientAndAgent(2, 0)


            oPaymentMethod.ClientName = r_vClaimClientAndAgent(3, 0)

            ' Alix - 12/02/2004 - Also pass product id

            If CStr(r_vClaimClientAndAgent(4, 0)) = "" Then

                oPaymentMethod.ProductID = 0
            Else


                oPaymentMethod.ProductID = r_vClaimClientAndAgent(4, 0)
            End If



            m_lReturn = oPaymentMethod.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process object" & " 'iCLMPaymentMethod.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentPartyid")
                Return result
            End If

            ' Retrieve Party Shortname and set as Agent

            lPaymentPartyId = oPaymentMethod.Partyid

            lButtonClicked = oPaymentMethod.ButtonClicked

            sOComments = oPaymentMethod.Comments

            m_lPayeeMediaType = oPaymentMethod.PayeeMediaType

            m_sPayeeName = oPaymentMethod.PayeeName

            m_sPayeeBankName = oPaymentMethod.PayeeBankName

            m_sPayeeSortCode = oPaymentMethod.PayeeSortCode

            m_sPayeeAccountNo = oPaymentMethod.PayeeAccountNo

            m_lPayeeCountry = oPaymentMethod.PayeeCountry

            m_sPayeeComments = oPaymentMethod.PayeeComments

            ' Destroy Find Party object

            oPaymentMethod.Dispose()
            oPaymentMethod = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPaymentPartyid Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentPartyid", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '*************************************************************************************
    ' Name : PostPaymentToOrion
    '
    ' Desc : post payment transactions to orion
    '
    ' Hist : 15/03/2001 Created - Tinny
    '        05/07/01   RWH - Revised production of stats and removed stuff geared to
    '                   production of transactions as these will now be done in stored
    '                   procedures at the end of the roadmap.
    '*************************************************************************************
    Private Function PostPaymentToOrion(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimId As Integer, ByVal v_lPerilID As Integer, ByVal v_cPayAmount As Decimal, ByVal v_sCreditAccountCode As String, ByVal v_sCOBCode As String, ByVal v_lCOBId As Integer, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_cTaxAmount As Decimal = 0, Optional ByVal v_sTaxTypeCode As String = "") As Integer
        Dim result As Integer = 0
        Dim lDebitAccountID, lCreditAccountID As Integer
        Dim sDebitAccountCode As String = ""
        Dim lStatsFolderCnt As Integer

        Dim oClaimTrans As Object
        Dim sCreditAccountCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'A payment will debit the reserve account.
            sDebitAccountCode = "CLMRES" & v_sCOBCode.Trim()

            Dim temp_oClaimTrans As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oClaimTrans, "bControlTransClaims.Automated", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oClaimTrans = temp_oClaimTrans
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an " & "instance of bControlTransClaims object", vApp:=ACApp, vClass:=ACClass, vMethod:="PostPaymentToOrion")
                Return result
            End If

            'get credit account id - use party count if we have it
            If v_lPartyCnt <> 0 Then

                result = oClaimTrans.GetAccountID(r_lAccountID:=lCreditAccountID, v_lPartyCnt:=v_lPartyCnt)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    If result <> gPMConstants.PMEReturnCode.PMNotFound Then

                        oClaimTrans.Dispose()
                        oClaimTrans = Nothing
                        Return result
                    End If
                End If
            End If

            'data which goes in stats folder/detail and transaction detail

            oClaimTrans.DebitAccountID = lDebitAccountID

            oClaimTrans.CreditAccountID = lCreditAccountID

            oClaimTrans.TransactionTypeID = 27

            oClaimTrans.TransactionTypeCode = "C_CP" 'claim payment

            oClaimTrans.DocumentTypeID = 28 'Claim Payment

            oClaimTrans.InsuranceFileCnt = v_lInsuranceFileCnt

            oClaimTrans.ClaimId = v_lClaimId

            oClaimTrans.PerilID = v_lPerilID

            oClaimTrans.DebitCredit = "C"

            oClaimTrans.DocumentComment = "Payment for claim number " & v_lClaimId

            oClaimTrans.TransactionAmount = v_cPayAmount

            'RWH(02/07/01) Need to create stats separately now for each record to
            'account for reins and coins.

            m_lReturn = oClaimTrans.CreateStatsFolder(r_lStatsFolderCnt:=lStatsFolderCnt, v_sTransactionTypeCode:="C_CP")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oClaimTrans.Dispose()
                oClaimTrans = Nothing
                Return result
            End If


            m_lReturn = oClaimTrans.CreateStatsDetails(v_lStatsFolderCnt:=lStatsFolderCnt, v_sStatsDetailType:="GRS", v_lClassOfBusId:=v_lCOBId, v_sClassOfBusCode:=v_sCOBCode, v_lRIPartyCnt:=v_lPartyCnt, v_sRIShortName:=v_sCreditAccountCode, v_lRIPartyType:=0, v_sglRISharePercent:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oClaimTrans.Dispose()
                oClaimTrans = Nothing
                Return result
            End If

            ' Alix - 21/05/2003 - Insert stats details records for VAT (one NET and one GROSS)
            If (v_cTaxAmount <> 0) And m_bIsPostTaxes Then

                ' Pass tax amount

                oClaimTrans.TransactionAmount = v_cTaxAmount

                ' set tan / tag account code
                sCreditAccountCode = "NOTA" & v_sTaxTypeCode.Trim() & "IN"

                ' Create stats for GROSS amount

                m_lReturn = oClaimTrans.CreateStatsDetails(v_lStatsFolderCnt:=lStatsFolderCnt, v_sStatsDetailType:="TAG", v_lClassOfBusId:=v_lCOBId, v_sClassOfBusCode:=v_sCOBCode, v_lRIPartyCnt:=v_lPartyCnt, v_sRIShortName:=sCreditAccountCode, v_lRIPartyType:=0, v_sglRISharePercent:=0)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    oClaimTrans.Dispose()
                    oClaimTrans = Nothing
                    Return result
                End If

                ' Peter Finney - NET Taxes are posted in stored procedures!!!
                '        ' Pass negated tax amount
                '        oClaimTrans.TransactionAmount = -v_cTaxAmount
                '
                '        ' Create stats for NET amount
                '        m_lReturn = oClaimTrans.CreateStatsDetails(v_lStatsFolderCnt:=lStatsFolderCnt, _
                ''                                                    v_sStatsDetailType:="TAN", _
                ''                                                    v_lClassOfBusId:=v_lCOBId, _
                ''                                                    v_sClassOfBusCode:=v_sCOBCode, _
                ''                                                    v_lRIPartyCnt:=v_lPartyCnt, _
                ''                                                    v_sRIShortName:=sCreditAccountCode, _
                ''                                                    v_lRIPartyType:=0, _
                ''                                                    v_sglRISharePercent:=0)
                '        If m_lReturn <> PMTrue Then
                '            PostPaymentToOrion = PMFalse
                '            m_lReturn = oClaimTrans.Terminate()
                '            Set oClaimTrans = Nothing
                '            Exit Function
                '        End If
            End If
            ' /Alix

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to post to Orion", vApp:=ACApp, vClass:=ACClass, vMethod:="PostPaymentToOrion", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End If


            oClaimTrans.Dispose()
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            oClaimTrans.Dispose()

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to post payment transactions to Orion", vApp:=ACApp, vClass:=ACClass, vMethod:="PostPaymentToOrion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function CreateOutputDetails(ByRef vDetails(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            If Not Information.IsArray(m_vPaymentDetailArray) Then
                ' no values
                Return result
            End If

            'This array vDetails is used to pass up to uctRiskScreenControl which then
            'passes it to the reserve control to re-populate the reserve grid.
            'Therefore this must all be populated with amounts in LOSS CURRENCY
            ReDim vDetails(5, m_vPaymentDetailArray.GetUpperBound(1))
            For iCount As Integer = 0 To m_vPaymentDetailArray.GetUpperBound(1)
                'RVH - 27/09/2002   Use constants for array position

                vDetails(clPaymentIDColumn, iCount) = Conversion.Val(CStr(m_vPaymentDetailArray(0, iCount)))

                vDetails(clPaymentAmountColumn, iCount) = gPMFunctions.ToSafeCurrency(m_ListViewArray(iCount + 2, m_colThisPaymentLossCurrency))

                vDetails(clReserveIDColumn, iCount) = m_vReserveDetails(g_cIRDAreserveid, iCount)

                vDetails(clReserveTypeDescColumn, iCount) = ""

                vDetails(clReserveAdjustmentColumn, iCount) = m_ListViewArray(iCount + 2, m_colReserveAdjustment)
                ' get the reserve description from the listview control
                For iInnerCount As Integer = 1 To lstviewPayment.Items.Count

                    If Conversion.Val(Convert.ToString(lstviewPayment.Items.Item(iInnerCount - 1).Tag)) = Conversion.Val(CStr(vDetails(clPaymentIDColumn, iCount))) Then

                        vDetails(clReserveTypeDescColumn, iCount) = lstviewPayment.Items.Item(iInnerCount - 1).Text

                        vDetails(clPaidToDateColumn, iCount) = gPMFunctions.ToSafeCurrency(m_ListViewArray(iInnerCount - 1, m_colPaidToDate))

                        Exit For
                    End If
                Next iInnerCount
            Next iCount
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create output details", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateOutputDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    Private Function RecalcPayments() As Integer

        Dim result As Integer = 0
        Dim iItem As Integer
        Dim cCurrReserve As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            With lstviewPayment
                ' get the currently select item
                If .FocusedItem.Index + 1 < 1 Then
                    ' no item selected
                    Return result
                End If
                iItem = .FocusedItem.Index + 1 - 1
            End With

            'this payment - payment tab
            ' m_ListViewArray(iItem, m_colThisPaymentLossCurrency) = FormatField(PMFormatCurrency, m_ListViewArray(iItem, m_colThisPayment))

            ' current reserve - initial reserve + revise reserve - paid to date - this payment
            cCurrReserve = CDec(gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMFunctions.ToSafeCurrency(m_ListViewArray(iItem, m_colInitial)) + gPMFunctions.ToSafeCurrency(m_ListViewArray(iItem, m_colRevisionAmount)) - gPMFunctions.ToSafeCurrency(m_ListViewArray(iItem, m_colPaidToDate)) - gPMFunctions.ToSafeCurrency(m_ListViewArray(iItem, m_colThisPaymentLossCurrency))))

            If cCurrReserve < 0 And m_lAllowNegativeReserve = gPMConstants.PMEReturnCode.PMFalse Then
                m_ListViewArray(iItem, m_colRevisionAmount) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, CDbl(m_ListViewArray(iItem, m_colRevisionAmount)) + (-1 * cCurrReserve))
                m_ListViewArray(iItem, m_colCurrentReserve) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, 0)
                m_ListViewArray(iItem, m_colIncurred) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMFunctions.ToSafeCurrency(m_ListViewArray(iItem, m_colInitial)) + gPMFunctions.ToSafeCurrency(m_ListViewArray(iItem, m_colRevisionAmount)))
            Else
                m_ListViewArray(iItem, m_colCurrentReserve) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, cCurrReserve)
            End If

            m_lReturn = GetTotalValuesUW()
            m_lReturn = FillGrid()
            ' update the variant array
            m_lReturn = UpdatePaymentDetails(CInt(m_ListViewArray(iItem, m_colTag)))
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to recalculate " & "reserve figures", vApp:=ACApp, vClass:=ACClass, vMethod:="RecalcReserves", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result



            Return result
        End Try
    End Function



    Private Function UpdateCurrentPayment(ByVal iListViewIndex As Integer, ByVal cNewReserve As Decimal, ByVal cInitialReserve As Decimal) As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' update the array
            m_vReserveDetails(g_cIRDArevisedreserve, iListViewIndex - 3) = cNewReserve
            m_vReserveDetails(g_cIRDAinitialreserve, iListViewIndex - 3) = cInitialReserve

            ' update the initial reserve
            m_ListViewArray(iListViewIndex - 1, m_colInitial) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, cInitialReserve)

            ' update the revised reserve
            m_ListViewArray(iListViewIndex - 1, m_colRevisionAmount) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, cNewReserve)

            ' update the incurred
            m_ListViewArray(iListViewIndex - 1, m_colIncurred) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMFunctions.ToSafeCurrency(m_vReserveDetails(g_cIRDArevisedreserve, iListViewIndex - 3)) + gPMFunctions.ToSafeCurrency(Conversion.Val(CStr(m_vReserveDetails(g_cIRDAinitialreserve, iListViewIndex - 3)))))

            ' update the current reserve
            m_ListViewArray(iListViewIndex - 1, m_colCurrentReserve) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMFunctions.ToSafeCurrency(m_vReserveDetails(g_cIRDArevisedreserve, iListViewIndex - 3)) + gPMFunctions.ToSafeCurrency(Conversion.Val(CStr(m_vReserveDetails(g_cIRDAinitialreserve, iListViewIndex - 3)))) - gPMFunctions.ToSafeCurrency(Conversion.Val(CStr(m_vReserveDetails(g_cIRDApaidtodate, iListViewIndex - 3)))) - gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lstviewPayment.Items.Item(iListViewIndex - 1), m_lstThisPayment).Text))

            m_lReturn = GetTotalValuesUW()
            m_lReturn = FillGrid()

            Return result

        Catch


            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function

    '******************************************************************************
    ' Name:         UpdatePaymentDetails
    ' Description:  updates the payment details for a particular Peril and Claim
    '               Populate v_lPaymentLinID to update just the one line, otherwise
    '               all lines will be updated
    ' History:      04/09/2001 Tinny - renumbering columns, add revision amount in
    '               payment tab
    '******************************************************************************
    Private Function UpdatePaymentDetails(Optional ByVal v_lPaymentLineID As Integer = -1) As Integer

        Dim result As Integer = 0
        Dim lCount As Integer
        Dim lstItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lPaymentLineID = -1 Then
                ' updating all items so initialise the array
                If Not Information.IsArray(m_vReserveDetails) Then
                    m_vPaymentDetailArray = Nothing
                    Return result
                Else
                    'If IsArray(m_vPaymentDetailArray) = False Then
                    ReDim m_vPaymentDetailArray(13, m_vReserveDetails.GetUpperBound(1))
                End If
                lCount = 3
            Else
                ' which item are we updating
                lCount = -1
                With lstviewPayment
                    For lCount = 3 To .Items.Count
                        If Conversion.Val(Convert.ToString(.Items.Item(lCount - 1).Tag)) = v_lPaymentLineID Then
                            Exit For
                        End If
                    Next lCount
                    If lCount < 0 Or (lCount = .Items.Count And Conversion.Val(Convert.ToString(.Items.Item(lCount - 1).Tag)) <> v_lPaymentLineID) Then
                        ' haven't found the item so log an error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to locate" & " the payment item", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePaymentDetails")
                        Return result
                    End If
                End With
            End If

            For iCount As Integer = m_vPaymentDetailArray.GetLowerBound(1) To m_vPaymentDetailArray.GetUpperBound(1)
                lstItem = lstviewPayment.Items.Item(iCount + 2)
                If v_lPaymentLineID = -1 Or (v_lPaymentLineID = Conversion.Val(Convert.ToString(lstItem.Tag))) Then

                    m_vPaymentDetailArray(g_cIPDApaymentid, iCount) = Convert.ToString(lstItem.Tag)
                    m_vPaymentDetailArray(g_cIPDAamount, iCount) = m_ListViewArray(iCount + 2, m_colThisPayment)
                    m_vPaymentDetailArray(g_cIPDAPartyID, iCount) = m_lPartycnt
                    m_vPaymentDetailArray(g_cIPDAComments, iCount) = m_sComments
                    m_vPaymentDetailArray(g_cIPDATaxAmount, iCount) = m_ListViewArray(iCount + 2, m_colTaxValue)
                    'm_vPaymentDetailArray(g_cIPDATaxAmount, iCount) = v_cTaxAmount
                    m_vPaymentDetailArray(g_cIPDATaxTypeCode, iCount) = m_lPayeeMediaType
                    m_vPaymentDetailArray(g_cIPDAPayeeName, iCount) = m_sPayeeName
                    m_vPaymentDetailArray(g_cIPDAPayeeBankName, iCount) = m_sPayeeBankName
                    m_vPaymentDetailArray(g_cIPDAPayeeSortCode, iCount) = m_sPayeeSortCode
                    m_vPaymentDetailArray(g_cIPDAPayeeAccountNo, iCount) = m_sPayeeAccountNo
                    m_vPaymentDetailArray(g_cIPDAPayeeCountry, iCount) = m_lPayeeCountry
                    m_vPaymentDetailArray(g_cIPDAPayeeComments, iCount) = m_sPayeeComments
                    'Test next line
                    ' Stop

                    m_vPaymentDetailArray(g_cIPDAPaymentCurrencyID, iCount) = m_lPaymentCurrencyID
                    m_vPaymentDetailArray(g_cIPDAPaymentLossRate, iCount) = m_lCurrencyRatePayToLoss
                End If
            Next iCount

            lstItem = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the " & "UpdatePaymentDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePaymentDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result


            Return result
        End Try
    End Function

    '******************************************************************************
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    '******************************************************************************
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


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    Private Sub cmdHistory_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHistory.Click
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: cmdHistory_Click
        ' PURPOSE: Show the Payment History
        ' AUTHOR: Danny Davis
        ' DATE: 02 November 2004, 11:23:42
        ' CHANGES:
        ' ---------------------------------------------------------------------------


        Try


            'developer guide no. 
            Dim oPaymentList As Object
            Dim iRow As Integer

            If lstviewPayment.FocusedItem.Text = "" Then
                Exit Sub
            End If

            Dim temp_oPaymentList As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPaymentList, sClassName:="iCLMListPayments.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oPaymentList = temp_oPaymentList
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", " + Information.Err().Source + ", Failed to get object iCLMPaymentList.")
            End If

            'Pass through the parameters

            oPaymentList.ClaimId = m_lClaimId

            iRow = lstviewPayment.FocusedItem.Index + 1 - 3

            If lstviewPayment.FocusedItem.Text = "Total" Then

                oPaymentList.ReserveId = 0

                oPaymentList.ReserveText = "All Reserves"
            ElseIf iRow > m_vReserveDetails.GetUpperBound(1) Then
                'This is an invalid payment line, pass through -1 to stop anything being returned

                oPaymentList.ReserveId = -1

                oPaymentList.ReserveText = lstviewPayment.FocusedItem.Text
            Else

                oPaymentList.ReserveId = m_vReserveDetails(0, lstviewPayment.FocusedItem.Index + 1 - 3)

                oPaymentList.ReserveText = lstviewPayment.FocusedItem.Text
            End If

            'Fire it up

            m_lReturn = oPaymentList.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", " + Information.Err().Source + ", Failed to start iCLMPaymentList.")
            End If

            oPaymentList.Dispose()
            oPaymentList = Nothing


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdHistory_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            End Select

        Finally
        End Try
    End Sub

    Private Sub lstviewPayment_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lstviewPayment.Click
        Dim vTagvalue As Integer
        'developer guide no.101
        Dim vValue As Object
        Try

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                ' view mode
                cmdEdit.Enabled = False
                Exit Sub
            End If

            ' edit property of this control is disabled
            If Not m_ShowEdit Then Exit Sub
            If lstviewPayment.Items.Count < 1 Then Exit Sub

            vTagvalue = lstviewPayment.FocusedItem.Index + 1

            vValue = Convert.ToString(lstviewPayment.FocusedItem.Tag)
            If vTagvalue <> 1 And vTagvalue <> 2 Then
                If m_sTransactionType = "C_CP" And vValue <> 0 Then
                    'CMG/PB 05122002 If enabled, Loss Schedule Will handle payments PS202
                    'If LossSchedule is enabled then dont allow any editing
                    cmdEdit.Enabled = Not m_bLossSchedule
                    'End CMG
                Else
                    cmdEdit.Enabled = False
                End If
            Else
                cmdEdit.Enabled = False
            End If

        Catch


            Exit Sub
        End Try

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        Dim bNeed2Update As Boolean
        Dim vNewData(,) As Object

        Dim sTaxTypeCode As String = ""
        'developer guide no 69. 
        Dim FrmDetailsUW As frmDetailsUW = New frmDetailsUW
        Try

            ' edit property of this control is disabled
            If Not m_ShowEdit Then Exit Sub


            With FrmDetailsUW
                'Header details
                .ClaimId = m_lClaimId
                .TransactionType = m_sTransactionType
                .Task = m_iTask
                .AllowNegativeReserve = m_lAllowNegativeReserve
                'The payment currency must be the same accross all reserves.
                .PaymentCurrencyID = m_lPaymentCurrencyID
                .obCLMPeril = m_oBusiness

                'Populate all the fields in the child screen from the Array
                .RiskType = CStr(m_ListViewArray(lstviewPayment.FocusedItem.Index + 1, m_colRiskType))
                'Reset "current reserve" value with "initial reserve" value. The child screen is only interested in the saved reserve at this point and this payment has not yet been saved
                .CurrentReserve = gPMFunctions.ToSafeCurrency(m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colCurrentReserve))
                .PaidToDate = gPMFunctions.ToSafeCurrency(m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colPaidToDate))
                .ThisPaymentLossCurrency = gPMFunctions.ToSafeCurrency(m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colThisPaymentLossCurrency))
                .ThisPayment = gPMFunctions.ToSafeCurrency(m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colThisPayment))
                .LossCurrencyName = CStr(m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colLossCurrencyName))
                .PaymentCurrencyName = CStr(m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colPaymentCurrencyName))
                .TaxTypeCode = CStr(m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colTaxTypeCode))
                .TaxBand = CStr(m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colTaxBand))
                .TaxValue = gPMFunctions.ToSafeCurrency(m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colTaxValue))
                .TotalInclTax = gPMFunctions.ToSafeCurrency(m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colTotalInclTax))
                .CurrencyRatePayToLoss = gPMFunctions.ToSafeDouble(m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colCurrencyRatePayToLoss))

                ' If we are not posting taxes seperately we need to strip out the tax amounts
                If Not m_bIsPostTaxes Then
                    .ThisPayment -= .TaxValue
                    .ThisPaymentLossCurrency -= .TaxValueLossCurrency
                End If

                'Perform any stuff that needs doing before form load
                m_lReturn = .Initialise()
                .ShowDialog()

                'Get the results for the form
                bNeed2Update = .RevisionHasBeenAmended
                If bNeed2Update Then
                    m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colRiskType) = .RiskType
                    m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colCurrentReserve) = .CurrentReserve
                    m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colPaidToDate) = .PaidToDate

                    ' If we are not posting taxes add on the tax amount
                    If m_bIsPostTaxes Then
                        m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colThisPayment) = .ThisPayment
                        m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colThisPaymentLossCurrency) = .ThisPaymentLossCurrency
                    Else
                        m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colThisPayment) = .ThisPayment + .TaxValue
                        m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colThisPaymentLossCurrency) = .ThisPaymentLossCurrency + .TaxValueLossCurrency
                    End If

                    m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colLossCurrencyName) = .LossCurrencyName
                    m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colPaymentCurrencyName) = .PaymentCurrencyName
                    m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colTaxTypeCode) = .TaxTypeCode
                    m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colTaxBand) = .TaxBand
                    m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colTaxValue) = .TaxValue
                    m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colTotalInclTax) = .TotalInclTax
                    m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colCurrencyRatePayToLoss) = .CurrencyRatePayToLoss
                    m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colPaymentCurrencyID) = .PaymentCurrencyID
                    'Not displayed on Payment control, but passed up for reserve control
                    m_ListViewArray(lstviewPayment.FocusedItem.Index + 1 - 1, m_colReserveAdjustment) = .ReserveAdjustment
                    'Has the pyament currency already been selected? (all rows
                    'much have the same payment currency as they will be bundled
                    'into a single payment). 0 = none yet selected
                    If m_lPaymentCurrencyID = 0 Then
                        'Accept this payment currency
                        m_lPaymentCurrencyID = .PaymentCurrencyID
                        m_lCurrencyRatePayToLoss = .CurrencyRatePayToLoss
                    Else
                        'Already selected so this one must match
                        If m_lPaymentCurrencyID <> .PaymentCurrencyID Then
                            MessageBox.Show("Payment currencies differ between rows", Application.ProductName)
                        End If
                    End If

                End If

                'Unload the form to reset all the vars for next payment
                FrmDetailsUW.Close()

                If bNeed2Update Then
                    ' update this listview
                    m_lReturn = RecalcPayments()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to " & "calculate the new reserve figures", vApp:=ACApp, vClass:=ACClass, vMethod:="Edit Button Click Event")
                        Exit Sub
                    End If

                    'RVH - 27/09/2002   Changed from UDT to variant
                    m_lReturn = CreateOutputDetails(vNewData)
                    RaiseEvent DataHasChanged(Me, New DataHasChangedEventArgs(vNewData))
                End If
            End With

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call the edit " & "button event", vApp:=ACApp, vClass:=ACClass, vMethod:="Edit Button Click Event", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try

    End Sub

    Public Function GetDetails(Optional ByVal lPerilID As Integer = 0, Optional ByVal lPerilTypeID As Integer = 0, Optional ByVal lClaimID As Integer = 0, Optional ByVal lRiskID As Integer = 0, Optional ByVal lInsurance_File_Cnt As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim v_Temp As Object
        Dim oRiskDetails As Object
        Dim v_RiskDetails As Object
        Dim bCheckPaymentAuthorisation As Boolean

        Try

            ' set the return value
            result = gPMConstants.PMEReturnCode.PMTrue

            ' check the input parameters
            If lPerilTypeID = 0 And (lPerilID = 0 Or lClaimID = 0 Or lRiskID = 0 Or lInsurance_File_Cnt = 0) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="The parameters provided " & "are insufficient or incorrect", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")
                Return result

            ElseIf lPerilTypeID <> 0 Then
                m_lPerilTypeID = lPerilTypeID
                m_iTask = gPMConstants.PMEComponentAction.PMView
            Else
                m_lPerilID = lPerilID
                m_lClaimId = lClaimID
                m_lRiskID = lRiskID
                g_lInsurance_file_cnt = lInsurance_File_Cnt
            End If

            'Get the risk type
            ' Get an instance of the business object via the public object manager.
            Dim temp_oRiskDetails As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oRiskDetails, "bCLMRiskDetails.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oRiskDetails = temp_oRiskDetails

            ' initialise this new object

            m_lReturn = CType(oRiskDetails, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:=g_oObjectManager.UserName, sPassword:=g_oObjectManager.Password, iuserid:=g_iUserId, isourceid:=g_iSourceID, ilanguageid:=g_iLanguageID, iCurrencyID:=g_oObjectManager.CurrencyID, iloglevel:=g_oObjectManager.LogLevel, sCallingAppName:=ACApp)

            ' get the text description of the risk type

            m_lReturn = oRiskDetails.GetRiskDetails(v_lrisk:=m_lRiskID, v_lpolicyid:=g_lInsurance_file_cnt, r_vdataarray:=v_RiskDetails)

            oRiskDetails.Dispose()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the Risk " & "Details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")
            End If
            If Information.IsArray(v_RiskDetails) Then

                m_sRisktype = CStr(v_RiskDetails(1, 0))
                m_bIsPostTaxes = gPMFunctions.ToSafeBoolean(v_RiskDetails(2, 0), True)
            Else
                m_sRisktype = ""
                m_bIsPostTaxes = True
            End If

            'Get the CurrencyCode for the claim
            If m_lClaimId <> 0 Then

                m_lReturn = m_oBusiness.GetClaimCurrency(v_lClaimId:=m_lClaimId, r_lCurrencyID:=g_lCurrencyID, r_sCurrencyDesc:=m_sLossCurrencyName)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get the claim currency")
                End If
            End If

            ' disable the edit button
            cmdEdit.Enabled = False

            ' check to see if the authorisation scripts for claim payments is switched on...
            m_lReturn = UseAuthorisedScriptsForClaimPayments(bCheckPaymentAuthorisation)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                ' can't get hidden option so assume it is switched off...
                bCheckPaymentAuthorisation = False

                ' log warning...
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Failed during call to UseAuthorisedScriptsForClaimPayments - assuming authorisation scripts are disabled", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")
            End If

            ' only if authorisation scripts for claim payments are on do we need to check for referred payments...
            If bCheckPaymentAuthorisation Then
                'AK 130503 - stop the user from adding any payments, if any existing payment is outstanding - start

                m_lReturn = m_oBusiness.CheckReferredPayment(m_lClaimId, m_bStatus)

                'Edit button will be permanently disabled if task type is set to view
                If m_bStatus Then
                    m_iTask = gPMConstants.PMEComponentAction.PMView
                End If
            End If

            ' set these values for the business object

            m_oBusiness.PerilID = m_lPerilID

            m_oBusiness.PerilTypeID = m_lPerilTypeID

            m_oBusiness.ClaimId = m_lClaimId

            m_oBusiness.PartyCnt = m_lPartycnt

            ' Had to call this function cos it is the only way to pass the above
            ' parameters to the dCLMPeril.Data that is used by the business layer

            m_lReturn = m_oBusiness.GetControls(v_Temp)

            m_lReturn = GetReserveType()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the " & "GetPayment Type", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")
                Return result
            End If

            'DC280302 -start -check for underwriting/broking
            m_lReturn = GetReserveDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetRecoveryDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetPaymentDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetTotalValuesUW()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = FillGrid()

            ' initialise the return array
            UpdatePaymentDetails()


            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result


            Return result
        End Try
    End Function

    Private Function FillGrid() As Integer

        Dim result As Integer = 0
        Dim lItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lstviewPayment.Items.Clear()
            For iRow As Integer = 0 To m_ListViewArray.GetUpperBound(0)

                'developer guide no. Parameter missed
                lItem = lstviewPayment.Items.Add(m_ListViewArray(iRow, m_colDescription))
                With lItem
                    .Text = CStr(m_ListViewArray(iRow, m_colDescription))

                    .Tag = CStr(m_ListViewArray(iRow, m_colTag))
                    'Only loop to 7 (incurred) as the rest of the data in the array is not displayed
                    For iCol As Integer = 2 To m_colIncurred
                        ListViewHelper.GetListViewSubItem(lItem, iCol - 1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_ListViewArray(iRow, m_colTag + iCol))
                    Next iCol
                End With
            Next iRow
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result


            Return result
        End Try
    End Function

    Private Function SetupListview() As Integer

        Dim result As Integer = 0
        Dim sngWidth As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' add the column headers for lstviewPayment
            ' insert the column headers
            lstviewPayment.Columns.Insert(0, "               ", CInt(VB6.TwipsToPixelsX(sngWidth)))
            lstviewPayment.Columns.Item(0).TextAlign = HorizontalAlignment.Left
            lstviewPayment.Columns.Insert(1, "Initial Reserve", CInt(VB6.TwipsToPixelsX(sngWidth)))
            lstviewPayment.Columns.Item(1).TextAlign = HorizontalAlignment.Right
            lstviewPayment.Columns.Insert(2, "Revision Amount", CInt(VB6.TwipsToPixelsX(sngWidth)))
            lstviewPayment.Columns.Item(2).TextAlign = HorizontalAlignment.Right
            lstviewPayment.Columns.Insert(3, "Paid to Date", CInt(VB6.TwipsToPixelsX(sngWidth)))
            lstviewPayment.Columns.Item(3).TextAlign = HorizontalAlignment.Right
            lstviewPayment.Columns.Insert(4, "This Payment", CInt(VB6.TwipsToPixelsX(sngWidth)))
            lstviewPayment.Columns.Item(4).TextAlign = HorizontalAlignment.Right
            lstviewPayment.Columns.Insert(5, "Current Reserve", CInt(VB6.TwipsToPixelsX(sngWidth)))
            lstviewPayment.Columns.Item(5).TextAlign = HorizontalAlignment.Right
            lstviewPayment.Columns.Insert(6, "Incurred", CInt(VB6.TwipsToPixelsX(sngWidth)))
            lstviewPayment.Columns.Item(6).TextAlign = HorizontalAlignment.Right



            'Get the columns to re-size themselves to show as much data as possible
            ListView6Autosize(lstviewPayment, True)

            lstviewPayment.LabelEdit = False

            ' add the grid lines and full row select for the Reserve List view
            m_lReturn = SetExtraListViewProperties(v_hWndList:=lstviewPayment.Handle.ToInt32(), v_vShowGridLines:=True, v_vShowRowSelect:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the " & " Control", vApp:=ACApp, vClass:=ACClass, vMethod:="SetupListview", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    Private Sub lstviewPayment_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lstviewPayment.DoubleClick

        Dim vTagvalue As String = ""

        Try

            If lstviewPayment.Items.Count < 1 Then Exit Sub


            vTagvalue = Convert.ToString(lstviewPayment.FocusedItem.Tag)
            If vTagvalue <> "" And vTagvalue <> "0" And cmdEdit.Enabled Then
                cmdEdit_Click(cmdEdit, New EventArgs())
            End If

            'AK 130503
            If m_bStatus Then
                MessageBox.Show("Awaiting Authorisation for existing payment", "Claim Payment")
            End If

        Catch
        End Try


        Exit Sub
    End Sub

    Private Sub lstviewPayment_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles lstviewPayment.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If KeyAscii = CInt(Keys.Return) And cmdEdit.Enabled Then cmdEdit_Click(cmdEdit, New EventArgs())
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub lstviewPayment_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lstviewPayment.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        lstviewPayment_Click(lstviewPayment, New EventArgs())
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
        m_Visible = m_def_Visible
        m_Enabled = m_def_Enabled
    End Sub



    'developer guide no. 1 of No Solutions
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)


        m_BackColor = CInt(PropBag.ReadProperty("BackColor", m_def_BackColor))


        m_ForeColor = CInt(PropBag.ReadProperty("ForeColor", m_def_ForeColor))


        m_Enabled = CBool(PropBag.ReadProperty("Enabled", m_def_Enabled))


        'developer guide no. 2 of No Solutions
        m_Font = PropBag.ReadProperty("Font", MyBase.Font)


        m_BackStyle = CInt(PropBag.ReadProperty("BackStyle", m_def_BackStyle))


        m_BorderStyle = CInt(PropBag.ReadProperty("BorderStyle", m_def_BorderStyle))


        m_ShowEdit = CBool(PropBag.ReadProperty("ShowEdit", m_def_ShowEdit))


        m_Visible = CBool(PropBag.ReadProperty("Visible", m_def_Visible))


        m_Enabled = CBool(PropBag.ReadProperty("Enabled", m_def_Enabled))
    End Sub


    Private Sub uctCLMPayment_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        Try
            Const ACTwice As Integer = 2
            Const ACThrice As Integer = 3
            Const ACLeftMargin As Integer = 1

            ' check for minimum width
            If ClientRectangle.Width < ACCommandButtonWidth * 2 Then
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
            With fraPaymentDetails
                .Left = VB6.TwipsToPixelsX(ACLeftMargin)
                .Width = MyBase.ClientRectangle.Width - VB6.TwipsToPixelsX(ACTwice * ACLeftMargin)
                .Top = VB6.TwipsToPixelsY(ACLeftMargin)
                .Height = MyBase.ClientRectangle.Height
            End With

            If m_ShowEdit Then
                'resize the list view
                With lstviewPayment
                    ' Set list view's tops, left, height & width
                    .Top = VB6.TwipsToPixelsY(ACListViewTop)
                    .Left = VB6.TwipsToPixelsX(ACCtrlVerticalSpacing)
                    .Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(fraPaymentDetails.Width) - ACCommandButtonWidth - (ACThrice * ACCtrlVerticalSpacing))
                    .Height = fraPaymentDetails.Height - VB6.TwipsToPixelsY(ACTwice * ACListViewTop)
                End With

                ' position the edit button
                With cmdEdit
                    .Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(fraPaymentDetails.Width) - ACCtrlVerticalSpacing - ACCommandButtonWidth)
                    .Width = VB6.TwipsToPixelsX(ACCommandButtonWidth)
                    .Top = VB6.TwipsToPixelsY(ACListViewTop)
                End With

                ' position the history button
                With cmdHistory
                    .Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(fraPaymentDetails.Width) - ACCtrlVerticalSpacing - ACCommandButtonWidth)
                    .Width = VB6.TwipsToPixelsX(ACCommandButtonWidth)
                    .Top = VB6.TwipsToPixelsY(ACListViewTop + 540)
                End With


            Else
                'resize the list view
                With lstviewPayment
                    ' Set list view's tops, left, height & width
                    .Top = VB6.TwipsToPixelsY(ACListViewTop)
                    .Left = VB6.TwipsToPixelsX(ACCtrlVerticalSpacing)
                    .Width = fraPaymentDetails.Width - VB6.TwipsToPixelsX(ACTwice * ACCtrlVerticalSpacing)
                    .Height = fraPaymentDetails.Height - VB6.TwipsToPixelsY(ACTwice * ACListViewTop)
                End With
            End If

        Catch
        End Try



    End Sub

    Private Sub UserControl_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        cmdEdit.Visible = m_ShowEdit
        fraPaymentDetails.Enabled = m_Enabled
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

        PropBag.WriteProperty("Enabled", m_Enabled, m_def_Enabled)

        PropBag.WriteProperty("Visible", m_Visible, m_def_Visible)
    End Sub


    '******************************************************************************
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    '******************************************************************************
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Static bIsInitialised As Boolean

        Dim sTitle, sMessage As String
        Dim vOptionValue, sHelpFile As String
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
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Set the object manager to nothing.
                g_oObjectManager = Nothing
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise " & "the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If

            ' If UserID is 0 assume that user cancelled logon
            If g_oObjectManager.UserID = 0 Then
                ' Exit application
                result = gPMConstants.PMEReturnCode.PMFalse
                g_oObjectManager = Nothing
                Return result
            End If

            ' Store the language ID from the object manager to the public variables,
            ' to enable us to use them throughout the object.
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

            ' Get an instance of the business object via the public object manager.
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

                m_lReturn = CType(m_oBusiness, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:=g_oObjectManager.UserName, sPassword:=g_oObjectManager.Password, iuserid:=g_iUserId, isourceid:=g_iSourceID, ilanguageid:=g_iLanguageID, iCurrencyID:=g_oObjectManager.CurrencyID, iloglevel:=g_oObjectManager.LogLevel, sCallingAppName:=ACApp)
            End If

            'CMG/PB 05122002 If enabled, Loss Schedule Will handle payments PS202
            'Is Loss Schedule Enabled?
            iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTLossSchedule, g_iSourceID, vOptionValue)
            m_bLossSchedule = (gPMFunctions.NullToString(vOptionValue) = "1")

            ' hold Initialised status
            bIsInitialised = True

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise " & "the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)
            Return result

        End Try
    End Function


    '******************************************************************************
    ' Name: LoadControl
    '
    ' Description: Does all the extra stuff that initialise doesn't
    '
    '******************************************************************************
    Public Function LoadControl() As Integer

        Dim result As Integer = 0
        Dim sTemp As String = ""

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

            m_oFormFields = New iPMFormControl.FormFields()

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lAllowNegativeReserve = gPMConstants.PMEReturnCode.PMTrue

            'Do some underwriting specific stuff

            'Get Allow Neg Reserve system option

            m_lReturn = m_oBusiness.GetSystemOption(v_lOptionNumber:=1016, r_sReturn:=sTemp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get claim " & "system option (Allow negative reserve)", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl")
                Return result
            Else
                If sTemp <> "1" Then
                    m_lAllowNegativeReserve = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            'Also if Underwriting - Show the History button
            cmdHistory.Visible = True

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception


            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load control", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result


            Return result
        End Try
    End Function

    '******************************************************************************
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    '******************************************************************************
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}
            m_lReturn = SetupListview()
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

    '******************************************************************************
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    '******************************************************************************
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.

            ' set the caption for the frame

            fraPaymentDetails.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReserveFrame, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' set the caption for the button

            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the " & "language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         GetReserveType
    ' Description:  Gets the Types of reserves
    '******************************************************************************
    Private Function GetReserveType() As Integer

        Dim result As Integer = 0
        Dim vReserveTypeArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetReserveType(vReserveTypeArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'resize the array - Includes unseen data from Payment details screen

            If Information.IsArray(vReserveTypeArray) And Not Object.Equals(vReserveTypeArray, Nothing) Then

                ReDim m_ListViewArray(vReserveTypeArray.GetUpperBound(1) + 4, m_colTotalCount)
            Else
                ReDim m_ListViewArray(3, m_colTotalCount)
            End If

            ' get the specific reserve types

            If Not Information.IsArray(vReserveTypeArray) Or Object.Equals(vReserveTypeArray, Nothing) Then

            Else

                For lCount As Integer = vReserveTypeArray.GetLowerBound(1) To vReserveTypeArray.GetUpperBound(1)

                    m_ListViewArray(lCount + 2, m_colTag) = vReserveTypeArray(0, lCount)

                    m_ListViewArray(lCount + 2, m_colDescription) = vReserveTypeArray(1, lCount)
                    m_ListViewArray(lCount + 2, m_colRiskType) = m_sRisktype
                    m_ListViewArray(lCount + 2, m_colLossCurrencyName) = m_sLossCurrencyName
                Next lCount
            End If

            ' fill the descriptions and tag values
            m_ListViewArray(m_ListViewArray.GetLowerBound(0), m_colTag) = 1
            m_ListViewArray(m_ListViewArray.GetLowerBound(0), m_colDescription) = "Total"
            m_ListViewArray(m_ListViewArray.GetLowerBound(0) + 1, m_colDescription) = ""
            m_ListViewArray(m_ListViewArray.GetUpperBound(0) - 1, m_colTag) = 0
            m_ListViewArray(m_ListViewArray.GetUpperBound(0) - 1, m_colDescription) = "Salvage Recovery"
            m_ListViewArray(m_ListViewArray.GetUpperBound(0), m_colTag) = 0
            m_ListViewArray(m_ListViewArray.GetUpperBound(0), m_colDescription) = "T.P Recovery"


            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the GetReserveType", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '******************************************************************************
    ' Name: GetTotalValuesBR
    '
    ' Description: populates the total row in the reserve listview
    '
    '******************************************************************************


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
    'If lstviewPayment.Items.Count = 0 Then 'Return result
    'For 'lCount As Integer = 3 To lstviewPayment.Items.Count
    'If lCount = CDbl(m_vReserveTotalArray(iCount)) Then
    'DC140302
    'If ListViewHelper.GetListViewSubItem(lstviewPayment.Items.Item(lCount - 1), lCount1).Text = "" Then
    'ListViewHelper.GetListViewSubItem(lstviewPayment.Items.Item(lCount - 1), lCount1).Text = "0.00"
    'End If
    'cTotal += CDbl(ListViewHelper.GetListViewSubItem(lstviewPayment.Items.Item(lCount - 1), lCount1).Text)
    'iCount += 1
    'End If
    'Next lCount
    'ListViewHelper.GetListViewSubItem(lstviewPayment.Items.Item(0), lCount1).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, cTotal)
    'Next lCount1
    '
    ' get the Average Values in the Reserve List View
    'iCount = 0
    'For 'lCount As Integer = 3 To lstviewPayment.Items.Count
    'If gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lstviewPayment.Items.Item(lCount - 1), 4).Text) <> 0 Then
    'If CDbl(m_vReserveTotalArray(iCount)) = lCount Then
    'lAvg = CInt((gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lstviewPayment.Items.Item(lCount - 1), 5).Text) / gPMFunctions.ToSafeCurrency(ListViewHelper.GetListViewSubItem(lstviewPayment.Items.Item(lCount - 1), 4).Text)) * 100)
    'End If
    'If lAvg < 100 And lAvg > 0 Then
    'lAvg = 100
    'End If
    'ListViewHelper.GetListViewSubItem(lstviewPayment.Items.Item(lCount - 1), 6).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, lAvg)
    '
    'Else
    'ListViewHelper.GetListViewSubItem(lstviewPayment.Items.Item(lCount - 1), 6).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, 0)
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

    '******************************************************************************
    ' Name:         GetTotalValuesUW
    ' Description:  populates the total row in the reserve listview
    '******************************************************************************
    Private Function GetTotalValuesUW() As Integer

        Dim result As Integer = 0
        Dim cTotal As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' calculate the totals, column by column
            'Only loop to 7 (incurred) as the rest of the data in the array is not displayed
            'For iCol = 2 to 7....
            For iCol As Integer = m_ListViewArray.GetLowerBound(1) + 2 To m_colIncurred
                cTotal = 0
                'For iRow = 2 to 9
                For iRow As Integer = m_ListViewArray.GetLowerBound(0) + 2 To m_ListViewArray.GetUpperBound(0)
                    cTotal += gPMFunctions.ToSafeCurrency(m_ListViewArray(iRow, iCol))
                Next iRow

                m_ListViewArray(m_ListViewArray.GetLowerBound(0), iCol) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, cTotal)
            Next iCol

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the GetTotalValuesUW", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTotalValuesUW", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    '******************************************************************************
    ' Name: GetRecoveryDetails
    '
    ' Description: Gets the details for a particular Recovery
    '
    ' Hist :
    '******************************************************************************

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
                m_ListViewArray(lCount, m_colInitial) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, 0)
                m_ListViewArray(lCount, m_colRevisionAmount) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, 0)

                m_ListViewArray(lCount, m_colInitial) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, r_vRecoveryDetailsArray(g_cIRecoveryDAinitialreserve, 0))
                m_ListViewArray(lCount, m_colPaidToDate) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, r_vRecoveryDetailsArray(g_cIRecoveryDApaidtodate, 0))

                m_ListViewArray(lCount, m_colCurrentReserve) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMFunctions.ToSafeCurrency(Conversion.Val(CStr(r_vRecoveryDetailsArray(g_cIRecoveryDAinitialreserve, 0)))) + gPMFunctions.ToSafeCurrency(r_vRecoveryDetailsArray(g_cIRecoveryDArevisedreserve, 0)) - gPMFunctions.ToSafeCurrency(r_vRecoveryDetailsArray(g_cIRecoveryDApaidtodate, 0)))
                m_ListViewArray(lCount, m_colIncurred) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMFunctions.ToSafeCurrency(r_vRecoveryDetailsArray(g_cIRecoveryDAinitialreserve, 0)) + gPMFunctions.ToSafeCurrency(r_vRecoveryDetailsArray(g_cIRecoveryDArevisedreserve, 0)))

            End If

            ' call the function to collect the details for the Recovery Type Third Party

            m_lReturn = m_oBusiness.GetRecoveryDetails(0, r_vRecoveryDetailsArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If


            If Information.IsArray(r_vRecoveryDetailsArray) And Not Object.Equals(r_vRecoveryDetailsArray, Nothing) Then
                lCount = m_ListViewArray.GetUpperBound(0)

                If CStr(r_vRecoveryDetailsArray(g_cIRecoveryDArevisedreserve, 0)) = "" Then

                    r_vRecoveryDetailsArray(g_cIRecoveryDArevisedreserve, 0) = 0
                End If

                If CStr(r_vRecoveryDetailsArray(g_cIRecoveryDAinitialreserve, 0)) = "" Then

                    r_vRecoveryDetailsArray(g_cIRecoveryDAinitialreserve, 0) = 0
                End If

                If CStr(r_vRecoveryDetailsArray(g_cIRecoveryDApaidtodate, 0)) = "" Then

                    r_vRecoveryDetailsArray(g_cIRecoveryDApaidtodate, 0) = 0
                End If

                m_ListViewArray(lCount, m_colInitial) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, 0)
                m_ListViewArray(lCount, m_colRevisionAmount) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, 0)

                m_ListViewArray(lCount, m_colInitial) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, r_vRecoveryDetailsArray(g_cIRecoveryDAinitialreserve, 0))
                m_ListViewArray(lCount, m_colPaidToDate) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, r_vRecoveryDetailsArray(g_cIRecoveryDApaidtodate, 0))

                m_ListViewArray(lCount, m_colCurrentReserve) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMFunctions.ToSafeCurrency(r_vRecoveryDetailsArray(g_cIRecoveryDAinitialreserve, 0)) + gPMFunctions.ToSafeCurrency(r_vRecoveryDetailsArray(g_cIRecoveryDArevisedreserve, 0)) - gPMFunctions.ToSafeCurrency(r_vRecoveryDetailsArray(g_cIRecoveryDApaidtodate, 0)))

                m_ListViewArray(lCount, m_colIncurred) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMFunctions.ToSafeCurrency(r_vRecoveryDetailsArray(g_cIRecoveryDAinitialreserve, 0)) + gPMFunctions.ToSafeCurrency(r_vRecoveryDetailsArray(g_cIRecoveryDArevisedreserve, 0)))
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the GetRecoveryDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRecoveryDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function


    '******************************************************************************
    ' Name:         GetReserveDetails
    ' Description:  Gets the reserve details for a particular Peril and Claim
    '******************************************************************************
    Private Function GetReserveDetails() As Integer

        Dim result As Integer = 0
        Dim vReserveDetailsArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetReserveDetails(g_lInsurance_file_cnt, m_lRiskID, vReserveDetailsArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            ' Add the values in the array to the listview control

            If Not Information.IsArray(vReserveDetailsArray) Or Object.Equals(vReserveDetailsArray, Nothing) Then
                ' reset the array
                m_vReserveDetails = Nothing
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

                'CMG/PB 05122002 If enabled, Loss Schedule Will handle payments PS202
                If m_bLossSchedule Then


                    m_vThisPayment = vReserveDetailsArray(9, lCount)

                    m_lMainResvisionId = CInt(vReserveDetailsArray(g_cIRDAreserveid, lCount))

                    If CStr(m_vThisPayment) = "" Then

                        m_vThisPayment = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMFunctions.ToSafeCurrency(0))
                    End If
                End If
                'End CMG

                ' fill in the values in the List View array

                m_ListViewArray(lCount + 2, m_colTag) = vReserveDetailsArray(g_cIRDAreserveid, lCount)

                If CStr(vReserveDetailsArray(g_cIRDAreserveid, lCount)) = "" Then
                    'CMG/PB 05122002 If enabled, Loss Schedule Will handle payments PS202
                    'The SPs spu_get_reserve_details has been altered to not created work_reserve
                    'records if the loss schedule hidden option is on.  This is becasue the LS
                    'component will create the reserve records itself because this uct is not
                    'necessarily included in the claim builder screen, so if LS is enabled her
                    'dont error
                    If m_bLossSchedule Then
                        'Do nothing
                    Else
                        Throw New Exception()
                    End If
                    'End CMG
                End If

                'initial reserve
                m_ListViewArray(lCount + 2, m_colInitial) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, vReserveDetailsArray(g_cIRDAinitialreserve, lCount))

                'add revision amount column
                m_ListViewArray(lCount + 2, m_colRevisionAmount) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, vReserveDetailsArray(g_cIRDArevisedreserve, lCount))

                'pay to date
                m_ListViewArray(lCount + 2, m_colPaidToDate) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, vReserveDetailsArray(g_cIRDApaidtodate, lCount))
                'current reserve

                m_ListViewArray(lCount + 2, m_colCurrentReserve) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMFunctions.ToSafeCurrency(Conversion.Val(CStr(vReserveDetailsArray(g_cIRDArevisedreserve, lCount)))) + gPMFunctions.ToSafeCurrency(Conversion.Val(CStr(vReserveDetailsArray(g_cIRDAinitialreserve, lCount)))) - gPMFunctions.ToSafeCurrency(Conversion.Val(CStr(vReserveDetailsArray(g_cIRDApaidtodate, lCount)))))
                'incurred

                m_ListViewArray(lCount + 2, m_colIncurred) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMFunctions.ToSafeCurrency(Conversion.Val(CStr(vReserveDetailsArray(g_cIRDArevisedreserve, lCount)))) + gPMFunctions.ToSafeCurrency(Conversion.Val(CStr(vReserveDetailsArray(g_cIRDAinitialreserve, lCount)))))


            Next lCount

            'keep track of original values

            m_vReserveDetails = vReserveDetailsArray

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the GetReserveDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    '******************************************************************************
    ' Name: GetPaymentDetails
    '
    ' Description: Gets the Payment Details for the Particular Reserve
    '
    ' Hist :
    '******************************************************************************
    Public Function GetPaymentDetails() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_vPaymentDetails = Nothing


            m_lReturn = m_oBusiness.GetPaymentDetails(m_vPaymentDetails)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the GetPaymentDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            ' default the values to zero
            For lCount As Integer = 2 To m_ListViewArray.GetUpperBound(0)
                ' set default values for 'this payment'
                'CMG/PB 05122002 If enabled, Loss Schedule Will handle payments PS202
                'If LS enabled and this is the mainpayment row then
                If m_bLossSchedule Then
                    If CDbl(m_ListViewArray(lCount, m_colTag)) = m_lMainResvisionId Then
                        If m_lMainResvisionId > 0 Then

                            m_ListViewArray(lCount, m_colThisPaymentLossCurrency) = m_vThisPayment
                        Else
                            m_ListViewArray(lCount, m_colThisPaymentLossCurrency) = "0.00"
                        End If
                    Else
                        If m_lMainResvisionId = 0 Then
                            'Do Nothing. We havent been into LS and created a work record yet
                            'so blank this out
                        Else
                            m_ListViewArray(lCount, m_colThisPaymentLossCurrency) = "0.00"
                        End If
                    End If
                Else
                    m_ListViewArray(lCount, m_colThisPaymentLossCurrency) = "0.00"
                End If
                'End CMG
            Next lCount


            If Not Information.IsArray(m_vPaymentDetails) Or m_vPaymentDetails Is Nothing Then
                'No history - so disable the history button
                cmdHistory.Enabled = False
            Else
                ' Add the values to the List view
                For lCount As Integer = m_vPaymentDetails.GetLowerBound(1) To m_vPaymentDetails.GetUpperBound(1)
                    If CStr(m_vPaymentDetails(g_cIPDApaymentid, lCount)) = "" Then Throw New Exception()

                    ' fill in the values in the Reserve Details List View
                    m_ListViewArray(lCount + 2, m_colTag) = m_vPaymentDetails(g_cIPDApaymentid, lCount)
                    'CMG/PB 05122002 If enabled, Loss Schedule Will handle payments PS202
                    If m_bLossSchedule And CDbl(m_ListViewArray(lCount, m_colTag)) > 0 Then
                        m_ListViewArray(lCount + 2, m_colThisPaymentLossCurrency) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_vThisPayment)
                    Else
                        m_ListViewArray(lCount + 2, m_colThisPaymentLossCurrency) = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, "0")
                    End If
                    'End CMG
                Next lCount
                cmdHistory.Enabled = True
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the " & "GetPaymentDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    '******************************************************************************
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


    ' ***************************************************************** '
    ' Name: UseAuthorisedScriptsForClaimPayments
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 18-03-2005 : PN19467
    ' ***************************************************************** '
    Private Function UseAuthorisedScriptsForClaimPayments(ByRef r_bCheckAuthorisation As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UseAuthorisedScriptsForClaimPayments"

        Dim lReturn As Integer
        Dim vValue As Object

        Try



            '    UseAuthorisedScriptsForClaimPayments = PMTrue
            '
            '    lReturn = getProductOptionValue(SIROPTRunClaimsAuthorisationScript, _
            ''                                    SIRBCHHeadOffice, _
            ''                                    vValue)
            '    If lReturn <> PMTrue Then
            '        RaiseError kMethodName, "getProductOptionValue Failed " & _
            ''                " to return value for Option:" & SIROPTRunClaimsAuthorisationScript, PMLogError
            '    End If
            '
            '    If vValue = "1" Then
            '        r_bCheckAuthorisation = True
            '    Else
            '        r_bCheckAuthorisation = False
            '    End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function
End Class