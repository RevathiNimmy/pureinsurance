Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms

Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctCLMListPaymentsC_NET.uctCLMListPaymentsC")> _
Partial Public Class uctCLMListPaymentsC
    Inherits System.Windows.Forms.UserControl
    Public Event InitialisedChange()
    Public Event ClaimIDChange()
    Public Event ReserveIDChange()
    Public Event visibleCmdViewChange()

    '-----------------------------------------------
    'Local Private Variables declaration section
    Private m_lClaimID As Integer
    Private m_lReserveID As Integer
    Private m_vPaymentList(,) As Object
    Private m_oBusiness As Object
    Private m_lReturn As Integer
    Private m_oGeneral As Object

    Private m_oCurrencyConvert As bACTCurrencyConvert.Form
    Private m_bInitialised As Boolean
    Private m_bVisible As Boolean
    Private m_iCount As Integer
    Private m_iColumn() As String
    Private m_sSelItem As Integer
    Private m_lClaimPaymentIsGross As Boolean
    '-----------------------------------------------


    '-----------------------------------------------
    'Local Private Constants declaration section
    Private Const kACPaymentID As Integer = 0
    Private Const kACDate As Integer = 1
    Private Const kACResolvedName As Integer = 2
    Private Const kACPayee As Integer = 3
    Private Const kACAmount As Integer = 4
    Private Const kACCurrency As Integer = 5
    Private Const kACLossAmount As Integer = 6
    Private Const kACBaseAmount As Integer = 7
    Private Const kACPaymentCurrencyID As Integer = 8
    Private Const kACLossCurrencyID As Integer = 9
    Private Const kACBaseCurrencyID As Integer = 10
    Private Const kACTaxAmount As Integer = 11
    Private Const ACClass As String = "MainModule"

    Private Const kACClaimPerilId As Integer = 12
    Private Const kACMediaRef As Integer = 13
    Private m_bShowPaymentView As Boolean
    'End - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc) - (5.4.1)

    'Start(Saurabh Agrawal ) Tech Spec LOA010 Claim Payment Improvements
    Private Const kACBankName As Integer = 14
    Private Const kACBankAccountNo As Integer = 15
    Private Const kACBankCode As Integer = 16
    Private Const kACStatus As Integer = 17
    Private Const kBIC As Integer = 18
    Private Const kIBAN As Integer = 19
    'End(Saurabh Agrawal ) Tech Spec LOA010 Claim Payment Improvements

    '-----------------------------------------------


    <Browsable(True)> _
    Public Property ShowPaymentView() As Boolean
        Get
            Return m_bShowPaymentView
        End Get
        Set(ByVal Value As Boolean)
            m_bShowPaymentView = Value
        End Set
    End Property
    'End - (Sankar) - (Tech Spec - QBENZCR007 - Authorise Claim payments.doc)
    '-----------------------------------------------
    'Property designed to set ot get selected item text of
    'a list view

    <Browsable(True)> _
    Public Property selectedItem() As Integer
        Get
            Return m_sSelItem
        End Get
        Set(ByVal Value As Integer)
            m_sSelItem = Value
        End Set
    End Property
    '-----------------------------------------------


    <Browsable(True)> _
    Public Property CountColumn() As Integer
        Get
            Return m_iCount
        End Get
        Set(ByVal Value As Integer)
            m_iCount = Value
            ReDim m_iColumn(m_iCount - 1)
        End Set
    End Property


    <Browsable(True)> _
    Public Property ColumnCaption(ByVal index As Integer) As String
        Get
            Return m_iColumn(index)
        End Get
        Set(ByVal Value As String)
            m_iColumn(index) = Value
        End Set
    End Property


    <Browsable(True)> _
    Public Property visibleCmdView() As Boolean
        Get
            Return m_bVisible
        End Get
        Set(ByVal Value As Boolean)
            m_bVisible = Value
            If Not m_bVisible Then
                lvwPayments.Height = MyBase.Height - lvwPayments.Top - 30
                'cmdViewPayment.Visible = False
            Else
                lvwPayments.Height = MyBase.Height - VB6.TwipsToPixelsY(450) - 20
                cmdViewPayment.Visible = True


            End If

            RaiseEvent visibleCmdViewChange()
        End Set
    End Property

    <Browsable(False)> _
    Public WriteOnly Property ReserveID() As Integer
        Set(ByVal Value As Integer)
            m_lReserveID = Value
            RaiseEvent ReserveIDChange()
        End Set
    End Property


    <Browsable(True)> _
    Public Property ClaimId() As Integer
        Get
            Return m_lClaimID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimID = Value
            RaiseEvent ClaimIDChange()
        End Set
    End Property


    Private Property Initialised() As Boolean
        Get
            Return m_bInitialised
        End Get
        Set(ByVal Value As Boolean)
            m_bInitialised = Value
            RaiseEvent InitialisedChange()
        End Set
    End Property

    Private Function prepareListView() As Integer

        For i As Integer = 0 To CountColumn - 1
            lvwPayments.Columns.Insert(i, ColumnCaption(i), 94)
        Next
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Initialised Then
                m_lReturn = Initialise()
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                Initialised = True
            End If

            ' {* USER DEFINED CODE (Begin) *}


            m_lReturn = m_oBusiness.GetPaymentList(lClaimId:=m_lClaimID, lReserveID:=m_lReserveID, r_vPaymentList:=m_vPaymentList)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                'LogError.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            ' set the default claim payment option
            GetProductDetails()

            m_lReturn = BusinessToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' dont display the payment id column
            lvwPayments.Columns.Item(0).Width = CInt(0)

            Return result

        Catch excep As System.Exception

            'Error Section.GetBusiness = PMError

            'LogError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function Initialise() As Integer

        Dim result As Integer = 0
        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            ' Initialise the error number value.
            m_lReturn = gPMConstants.PMEReturnCode.PMTrue

            g_oObjectManager = New bObjectManager.ObjectManager()
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMPeril.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                'm_lErrorNumber& = PMFalse

                ' Display error stating the problem.

                ' Display message.
                MessageBox.Show("Cannot get an instance of bCLMPeril.", "Failed to get business object.", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

            Dim temp_m_oCurrencyConvert As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oCurrencyConvert = temp_m_oCurrencyConvert

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                'm_lErrorNumber& = PMFalse

                ' Display error stating the problem.

                ' Display message.
                MessageBox.Show("Cannot get an instance of bACTCurrencyConvert.", "Failed to get business object.", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            result = m_lReturn

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return result

        Catch excep As System.Exception

            'Error Section.GetBusiness = PMError

            'LogError.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise uctCLMListPayments", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Updates all interface details from the business object.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function BusinessToInterface() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim nStart, nEnd As Integer
        Dim dNetAmount As Double
        Dim sAmount As String = ""

        Try
            cmdViewPayment.Enabled = False
            prepareListView()

            'Assign the details to the interface.
            '    ListViewBatchStart lvwPayments

            lvwPayments.Items.Clear()
            If Information.IsArray(m_vPaymentList) Then

                nStart = m_vPaymentList.GetLowerBound(1)
                nEnd = m_vPaymentList.GetUpperBound(1)

                For lRow As Integer = nStart To nEnd

                    lvwPayments.Items.Insert(lRow, "P" & "-" & CStr(lRow) & "-" & CStr(m_vPaymentList(kACClaimPerilId, lRow)), CStr(m_vPaymentList(kACPaymentID, lRow)), "")
                    lvwPayments.Items.Item(lRow).SubItems.Add(DateTime.Parse(m_vPaymentList(kACDate, lRow)).ToString("d"))
                    lvwPayments.Items.Item(lRow).SubItems.Add(CStr(m_vPaymentList(kACResolvedName, lRow)).Trim())
                    lvwPayments.Items.Item(lRow).SubItems.Add(CStr(m_vPaymentList(kACPayee, lRow)).Trim())
                  '  If m_lClaimPaymentIsGross Then
                 '       dNetAmount = gPMFunctions.ToSafeDecimal(m_vPaymentList(kACAmount, lRow))
                   ' Else
                        dNetAmount = gPMFunctions.ToSafeDecimal(m_vPaymentList(kACAmount, lRow)) + gPMFunctions.ToSafeDecimal(m_vPaymentList(kACTaxAmount, lRow))
                '    End If

                    m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_vPaymentList(kACPaymentCurrencyID, lRow), vCurrencyAmount:=dNetAmount, vFormattedCurrency:=sAmount)
                    lvwPayments.Items.Item(lRow).SubItems.Add(sAmount)
                    m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_vPaymentList(kACPaymentCurrencyID, lRow), vCurrencyAmount:=m_vPaymentList(kACTaxAmount, lRow), vFormattedCurrency:=sAmount)
                    lvwPayments.Items.Item(lRow).SubItems.Add(sAmount)
                    lvwPayments.Items.Item(lRow).SubItems.Add(CStr(m_vPaymentList(kACCurrency, lRow)).Trim())
                    m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_vPaymentList(kACLossCurrencyID, lRow), vCurrencyAmount:=m_vPaymentList(kACLossAmount, lRow), vFormattedCurrency:=sAmount)
                    lvwPayments.Items.Item(lRow).SubItems.Add(sAmount)
                    m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_vPaymentList(kACBaseCurrencyID, lRow), vCurrencyAmount:=m_vPaymentList(kACBaseAmount, lRow), vFormattedCurrency:=sAmount)
                    lvwPayments.Items.Item(lRow).SubItems.Add(sAmount)
                    lvwPayments.Items.Item(lRow).SubItems.Add(CStr(m_vPaymentList(kACBankName, lRow)).Trim())
                    lvwPayments.Items.Item(lRow).SubItems.Add(CStr(m_vPaymentList(kACBankAccountNo, lRow)).Trim())
                    lvwPayments.Items.Item(lRow).SubItems.Add(CStr(m_vPaymentList(kACBankCode, lRow)).Trim())
                    lvwPayments.Items.Item(lRow).SubItems.Add(CStr(m_vPaymentList(kBIC, lRow)).Trim())
                    lvwPayments.Items.Item(lRow).SubItems.Add(CStr(m_vPaymentList(kIBAN, lRow)).Trim())
                    lvwPayments.Items.Item(lRow).SubItems.Add(CStr(m_vPaymentList(kACStatus, lRow)).Trim())
                    lvwPayments.Items.Item(lRow).SubItems.Add(CStr(m_vPaymentList(kACMediaRef, lRow)).Trim())

                Next lRow
                cmdViewPayment.Enabled = True
            End If

            If Information.IsArray(m_vPaymentList) Then
                lvwPayments.Items.Item(nEnd).Selected = True
                lvwPayments.HideSelection = False
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return nResult

        Catch excep As System.Exception


            ListViewFunc.ListViewBatchEnd()

            'Error Section.BusinessToInterface = PMError

            ' LogError.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    Private Sub cmdViewPayment_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdViewPayment.Click

        Const kMethodName As String = "cmdViewPayment_Click"
        Dim oPaymentList As Object
        Dim temp_oPaymentList As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oPaymentList, sClassName:="iCLMListPayments.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oPaymentList = temp_oPaymentList

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get an instance of the business object.
            'm_lErrorNumber& = PMFalse
            ' Display message.
            MessageBox.Show("Cannot get an instance of iCLMListPayments", "Failed to get business object.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        m_lReturn = oPaymentList.Initialise()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Display message.
            MessageBox.Show("Cannot get an instance of iCLMListPayments", "Failed to get business object.", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        m_lReturn = oPaymentList.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vTransactionType:="C_CP")
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "SetProcessModes Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        If Not Information.IsNothing(lvwPayments.FocusedItem) Then
            oPaymentList.ClaimPaymentId = lvwPayments.Items.Item(lvwPayments.FocusedItem.Index).Text.Trim()
        ElseIf lvwPayments.SelectedItems.Count > 0 Then
            oPaymentList.ClaimPaymentId = lvwPayments.Items.Item(lvwPayments.SelectedItems(0).Index).Text.Trim()
        End If

        oPaymentList.ClaimId = m_lClaimID

        If Not Information.IsNothing(lvwPayments.FocusedItem) Then
            Dim iPos As Integer = IIf(lvwPayments.FocusedItem.Name = "" And "-" = "", 0, (lvwPayments.FocusedItem.Name.LastIndexOf("-") + 1))
            oPaymentList.WorkClaimPerilId = lvwPayments.FocusedItem.Name.Substring(lvwPayments.FocusedItem.Name.Length - (Strings.Len(lvwPayments.FocusedItem.Name) - iPos))
        ElseIf lvwPayments.SelectedItems.Count > 0 Then
            Dim iPos As Integer = IIf(lvwPayments.SelectedItems(0).Name = "" And "-" = "", 0, (lvwPayments.SelectedItems(0).Name.LastIndexOf("-") + 1))
            oPaymentList.WorkClaimPerilId = lvwPayments.SelectedItems(0).Name.Substring(lvwPayments.SelectedItems(0).Name.Length - (Strings.Len(lvwPayments.SelectedItems(0).Name) - iPos))
        End If

        oPaymentList.ShowPaymentView = m_bShowPaymentView

        m_lReturn = oPaymentList.RunPaymentItem()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "RunPaymentItem Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


		oPaymentList.Dispose()
       
        oPaymentList = Nothing

        MyBase.ParentForm.Focus()
    End Sub

    Private Sub lvwPayments_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwPayments.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwPayments.Columns(eventArgs.Column)
        Dim lReturn As Integer
        Select Case ColumnHeader.Text.ToUpper()
            Case "DATE"
                If ListViewHelper.GetSortOrderProperty(lvwPayments) = SortOrder.Ascending Then

                    lReturn = ListViewFunc.ListViewSortByDate(lvwPayments, ColumnHeader.Index + 1 - 1, SortOrder.Descending)
                Else

                    lReturn = ListViewFunc.ListViewSortByDate(lvwPayments, ColumnHeader.Index + 1 - 1, SortOrder.Ascending)
                End If
        End Select
    End Sub

    Private Sub uctCLMListPaymentsC_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        lvwPayments.Left = MyBase.ClientRectangle.Left + VB6.TwipsToPixelsX(50)
        lvwPayments.Width = MyBase.Width - VB6.TwipsToPixelsX(50)
        lvwPayments.Height = MyBase.Height - cmdViewPayment.Height - VB6.TwipsToPixelsY(250)

        cmdViewPayment.Left = VB6.TwipsToPixelsX(50)
        cmdViewPayment.Top = lvwPayments.Height + lvwPayments.Top + 10
    End Sub

    Private Sub UserControl_InitProperties()
        visibleCmdView = True
    End Sub

    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)
        visibleCmdView = CBool(PropBag.ReadProperty("visibleCmdView", True))
    End Sub

    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)
        'visibleCmdView = PropBag.ReadProperty("visibleCmdView", True)
        PropBag.WriteProperty("visibleCmdView", m_bVisible, True)
    End Sub
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub

    Private Sub lvwPayments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwPayments.Click
        selectedItem = CInt(sender.FocusedItem.Text)
    End Sub

    Private Function GetProductDetails() As Integer
        Const kMethodName As String = "GetProductDetails"
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim o_ProductBusiness As Object

        lReturn = gPMConstants.PMEReturnCode.PMTrue
        Try
            Dim temp_o_ProductBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_o_ProductBusiness, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)

            o_ProductBusiness = temp_o_ProductBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRProduct.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = o_ProductBusiness.GetProductLevelOptionsForClaim(v_lClaimID:=m_lClaimID, r_bIs_Gross_Claim_Payment_Amount:=m_lClaimPaymentIsGross)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetProductDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
        Catch excep As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            lReturn = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn)
        End Try
        Return lReturn
    End Function

End Class