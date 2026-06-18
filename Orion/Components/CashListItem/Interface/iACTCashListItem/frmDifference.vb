Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports System.Data
Imports SharedFiles
Partial Friend Class frmDifference
    Inherits System.Windows.Forms.Form

#Region "Private Constants"
    Private Const ACClass As String = "frmDifference"
#End Region

#Region "Private Variables"
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lReturn As Integer

    Private m_iReceiptDifferenceOption As Integer
    Private m_iReceiptCurrencyID As Integer
    Private m_lReceiptCompanyID As Integer
    Private m_dtReceiptTransactionDate As Date
    Private m_iBaseCurrencyID As Integer
    Private m_cReceiptCurrencyAmount As Decimal
    Private m_cReceiptBaseAmount As Decimal
    Private m_cInstallmentsBaseAmount As Decimal
    Private m_cDifferenceBaseAmount As Decimal
    Private m_bMultiplePlans As Boolean
    Private m_bAllSelected As Boolean
    Private m_bHidden As Boolean
    Private m_nWriteOffReasonID As Integer
    Private m_oCurrencyConvert As bACTCurrencyConvert.Form
    Dim frmWriteOff As frmWriteOffReason
    Private m_oBusiness As bACTCashlistitem.Form
#End Region

#Region "Public Properties"
    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property

    Public Property ReceiptDifferenceOption() As Integer
        Get
            Return m_iReceiptDifferenceOption
        End Get
        Set(ByVal Value As Integer)
            m_iReceiptDifferenceOption = Value
        End Set
    End Property

    Public WriteOnly Property ReceiptCurrencyID() As Integer
        Set(ByVal Value As Integer)
            m_iReceiptCurrencyID = Value
        End Set
    End Property

    Public WriteOnly Property ReceiptCompanyID() As Integer
        Set(ByVal Value As Integer)
            m_lReceiptCompanyID = Value
        End Set
    End Property

    Public WriteOnly Property ReceiptTransactionDate() As Date
        Set(ByVal Value As Date)
            m_dtReceiptTransactionDate = Value
        End Set
    End Property

    Public WriteOnly Property ReceiptAmount() As Decimal
        Set(ByVal Value As Decimal)
            m_cReceiptCurrencyAmount = Value
        End Set
    End Property

    Public WriteOnly Property InstallmentsAmount() As Decimal
        Set(ByVal Value As Decimal)
            m_cInstallmentsBaseAmount = Value
        End Set
    End Property

    Public WriteOnly Property MultiplePlans() As Boolean
        Set(ByVal Value As Boolean)
            m_bMultiplePlans = Value
        End Set
    End Property

    Public WriteOnly Property AllSelected() As Boolean
        Set(ByVal Value As Boolean)
            m_bAllSelected = Value
        End Set
    End Property

    Public ReadOnly Property DifferenceAmount() As Decimal
        Get
            Return m_cDifferenceBaseAmount
        End Get
    End Property

    Public ReadOnly Property ReceiptBaseAmount() As Decimal
        Get
            Return m_cReceiptBaseAmount
        End Get
    End Property

    Public WriteOnly Property Hidden() As Boolean
        Set(ByVal Value As Boolean)
            m_bHidden = Value
        End Set
    End Property
    Public Property WriteOffReasonID() As Integer
        Get
            Return m_nWriteOffReasonID
        End Get
        Set(ByVal value As Integer)
            m_nWriteOffReasonID = value
        End Set
    End Property
#End Region

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Me.Hide()
    End Sub

    Private Sub cmdTakeExact_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdTakeExact.Click
        'Set option to take exact amount in case we had a choice.
        m_iReceiptDifferenceOption = 1

        m_lStatus = gPMConstants.PMEReturnCode.PMOK
        Me.Hide()
    End Sub

    Private Sub cmdWriteOff_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdWriteOff.Click
        Dim nCount As Integer = 0
        Dim obResultArray As DataRow() = Nothing
        'Set option to write off in case we had a choice.
        If m_iBaseCurrencyID = m_iReceiptCurrencyID Then

            m_lReturn = g_oObjectManager.GetInstance(m_oBusiness, "bACTCashlistitem.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_lReturn = m_oBusiness.CheckWriteOffReason(obResultArray)

            m_oBusiness = Nothing
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lStatus = gPMConstants.PMEReturnCode.PMError
            End If

            If obResultArray.Length = 0 Then
                MessageBox.Show("No Write Off Reason is configured for instalments. Contact the System Administrator.", "Write Off Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            If obResultArray.Length = 1 Then
                WriteOffReasonID = ToSafeInteger(obResultArray(0)(0))
                m_iReceiptDifferenceOption = 0
                m_lStatus = gPMConstants.PMEReturnCode.PMOK
                Me.Hide()
                Exit Sub
            End If

            frmWriteOff = New frmWriteOffReason()
            frmWriteOff.Title = "Write-Off Reason"
            frmWriteOff.WhereCondition = "is_valid_for_instalments = 1"

            frmWriteOff.lblMessage.Visible = False
            frmWriteOff.Height = frmWriteOff.Height - frmWriteOff.lblMessage.Height

            frmWriteOff.lblWriteOffReasonID.Location = New Point(frmWriteOff.lblWriteOffReasonID.Location.X, frmWriteOff.lblWriteOffReasonID.Location.Y - frmWriteOff.lblMessage.Height)
            frmWriteOff.cboWriteOffReasonID.Location = New Point(frmWriteOff.cboWriteOffReasonID.Location.X, frmWriteOff.cboWriteOffReasonID.Location.Y - frmWriteOff.lblMessage.Height)

            frmWriteOff.cmdOK.Location = New Point(frmWriteOff.cmdOK.Location.X, frmWriteOff.cmdOK.Location.Y - frmWriteOff.lblMessage.Height)
            frmWriteOff.Cancel.Location = New Point(frmWriteOff.Cancel.Location.X, frmWriteOff.Cancel.Location.Y - frmWriteOff.lblMessage.Height)
            frmWriteOff.StartPosition = FormStartPosition.CenterParent

            frmWriteOff.ShowDialog()
            If frmWriteOff.ReturnValue = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            Else
                WriteOffReasonID = frmWriteOff.WriteOffReasonID
            End If
            frmWriteOff.Close()
            frmWriteOff = Nothing
        End If
        m_iReceiptDifferenceOption = 0

        m_lStatus = gPMConstants.PMEReturnCode.PMOK
        Me.Hide()
    End Sub

    Private Sub Form_Initialize_Renamed()
        m_lStatus = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_m_oCurrencyConvert As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        m_oCurrencyConvert = temp_m_oCurrencyConvert
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lStatus = gPMConstants.PMEReturnCode.PMError
        End If
    End Sub

    'Private Sub frmDifference_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
    Public Sub frmDifferenceLoad()
        If m_lStatus = gPMConstants.PMEReturnCode.PMError Then
            Exit Sub
        End If

        m_lReturn = CalculateAmounts()

        If m_cDifferenceBaseAmount = 0 Then
            'Set option to take exact amount as their is no difference.
            m_iReceiptDifferenceOption = 1
            m_lStatus = gPMConstants.PMEReturnCode.PMOK
            Exit Sub
        ElseIf m_bMultiplePlans Then
            'Cancel out as receiving amounts for multiple plans only works if their is no difference.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
            Exit Sub
        ElseIf m_bHidden Then
            'Hidden, automatically write off difference
            m_iReceiptDifferenceOption = 0
            m_lStatus = gPMConstants.PMEReturnCode.PMOK
            Exit Sub
        End If

        m_lReturn = SetInterfaceDefaults()

        m_lReturn = SetInterfaceValues()

    End Sub

    Private Sub Form_Terminate_Renamed()
        If Not (m_oCurrencyConvert Is Nothing) Then

            m_oCurrencyConvert.Dispose()
            m_oCurrencyConvert = Nothing
        End If
    End Sub

    ''' <summary>
    ''' SetInterfaceDefaults
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetInterfaceDefaults() As Integer
        Dim nResult As Integer = 0
        Dim sMessage, sFormattedCurrency As String

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Create top message
            If m_iReceiptCurrencyID <> m_iBaseCurrencyID Then
                sMessage = "Due to currency fluctuations the receipt amount is "
                If m_cReceiptBaseAmount < m_cInstallmentsBaseAmount Then
                    sMessage = sMessage & "less "
                Else
                    sMessage = sMessage & "more "
                End If
                sMessage = sMessage & "than the instalment amount."
                Me.Text = "Currency Difference"
            Else
                sMessage = "The receipt amount is different to the instalment amounts."
                Me.Text = "Instalment Receipt Difference"
                pnlDiffCurrencyContainer.Visible = False
                Me.Height = Me.Height - pnlDiffCurrencyContainer.Height
                lblMessageBottom.Location = New Point(lblMessageBottom.Location.X, pnlDiffCurrencyContainer.Location.Y)
                cmdWriteOff.Location = New Point(cmdWriteOff.Location.X, pnlDiffCurrencyContainer.Location.Y + lblMessageBottom.Location.Y)
                cmdTakeExact.Location = New Point(cmdTakeExact.Location.X, pnlDiffCurrencyContainer.Location.Y + lblMessageBottom.Location.Y)
                cmdCancel.Location = New Point(cmdCancel.Location.X, pnlDiffCurrencyContainer.Location.Y + lblMessageBottom.Location.Y)
            End If

            lblMessageTop.Text = sMessage

            'Get the formatted difference amount so that we ca n use it in the bottom message

            m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iBaseCurrencyID, vCurrencyAmount:=m_cDifferenceBaseAmount, vFormattedCurrency:=sFormattedCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Select Case m_iReceiptDifferenceOption
                Case 0 'Always write-off
                    cmdWriteOff.Text = "OK"
                    cmdWriteOff.Left = cmdTakeExact.Left
                    cmdTakeExact.Visible = False

                    'Create bottom message
                    sMessage = "Confirming this receipt will take a "
                    If m_cReceiptBaseAmount < m_cInstallmentsBaseAmount Then
                        sMessage = sMessage & "loss "
                    Else
                        sMessage = sMessage & "gain "
                    End If
                    sMessage = sMessage & "of " & sFormattedCurrency & " on the instalment."
                    lblMessageBottom.Text = sMessage

                Case 1 'Always take exact amount
                    cmdTakeExact.Text = "OK"
                    cmdWriteOff.Visible = False

                    'Create bottom message
                    sMessage = "Confirming this receipt will "
                    If m_cReceiptBaseAmount < m_cInstallmentsBaseAmount Then
                        sMessage = sMessage & "partially pay one of the instalments selected."
                    Else
                        If m_bAllSelected Then
                            sMessage = sMessage & "leave a credit of " & sFormattedCurrency & " on the customers account."
                        Else
                            sMessage = sMessage & "partially pay another of the instalments for this plan."
                        End If
                    End If
                    lblMessageBottom.Text = sMessage
                Case 2 'Users choice
                    'Create bottom message
                    If m_iReceiptCurrencyID <> m_iBaseCurrencyID Then
                        sMessage = "You can write-off the difference or collect the exact amount by "
                        If m_cReceiptBaseAmount < m_cInstallmentsBaseAmount Then
                            sMessage = sMessage & "partially paying one of the instalments selected."


                        Else
                            If m_bAllSelected Then
                                sMessage = sMessage & "leaving a credit on the customers account."
                            Else
                                sMessage = sMessage & "partially paying another of the instalments for this plan."
                            End If
                        End If
                    Else
                        sMessage = "You can write-off the difference or collect the exact amount by partially paying one of the instalments selected."
                    End If
                    lblMessageBottom.Text = sMessage
                    lblMessageBottom.Visible = True
            End Select

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed in SetInterfaceDefaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", excep:=excep)

            Return nResult
        End Try
    End Function

    Private Function CalculateAmounts() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the base currency

            m_lReturn = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyId:=m_lReceiptCompanyID, r_iBaseCurrencyID:=m_iBaseCurrencyID)

            'Calculate the receipts base amount

            m_lReturn = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=m_iReceiptCurrencyID, lCompanyID:=m_lReceiptCompanyID, cBaseAmount:=m_cReceiptBaseAmount, cCurrencyAmount:=m_cReceiptCurrencyAmount, vConversionDate:=m_dtReceiptTransactionDate)

            'Calculate the difference
            
            m_cDifferenceBaseAmount = m_cInstallmentsBaseAmount - m_cReceiptBaseAmount
            

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed in CalculateAmounts", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateAmounts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function SetInterfaceValues() As Integer
        Dim result As Integer = 0
        Dim sFormattedCurrency As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iReceiptCurrencyID, vCurrencyAmount:=m_cReceiptCurrencyAmount, vFormattedCurrency:=sFormattedCurrency)
            txtReceiptAmount.Text = sFormattedCurrency
            cboReceiptAmount.CurrencyId = m_iReceiptCurrencyID

            m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iBaseCurrencyID, vCurrencyAmount:=m_cReceiptBaseAmount, vFormattedCurrency:=sFormattedCurrency)
            txtReceiptBaseCurrency.Text = sFormattedCurrency
            cboReceiptBaseCurrency.CurrencyId = m_iBaseCurrencyID

            m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iBaseCurrencyID, vCurrencyAmount:=m_cInstallmentsBaseAmount, vFormattedCurrency:=sFormattedCurrency)
            txtInstalmentsMarked.Text = sFormattedCurrency
            cboInstalmentsMarked.CurrencyId = m_iBaseCurrencyID

            m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iBaseCurrencyID, vCurrencyAmount:=Math.Abs(m_cDifferenceBaseAmount), vFormattedCurrency:=sFormattedCurrency)
            txtDifference.Text = sFormattedCurrency
            cboDifference.CurrencyId = m_iBaseCurrencyID

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed in SetInterfaceValues", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class