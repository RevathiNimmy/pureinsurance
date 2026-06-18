Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles

Partial Friend Class frmMultiCurrency
    Inherits System.Windows.Forms.Form
    Implements IDisposable
    '############################################################################
    '
    ' Description:  If policy and base currency are different AND ((User cannot
    '               change rates AND System Option 156 enabled) OR user
    '               can change rates)
    '
    ' History :
    ' 12052004 RDC created
    '############################################################################

    Private m_lReturn As Integer
    Private m_lStatus As gPMConstants.PMEReturnCode

    Private m_iTransactionCurrencyID As Integer
    Private m_cTransactionAmount As Decimal
    Private m_lPartyCnt As Integer
    Private m_lSourceID As Integer
    Private m_lClaimID As Integer
    Private m_lScreenMethod As Integer
    Private m_iLossCurrencyID As Integer
    Private m_cLossCurrencyAmount As Decimal
    Private m_lAccountID As Integer

    Private Const ACClass As String = "frmMultiCurrency"


    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

    Public WriteOnly Property TransactionCurrencyID() As Integer
        Set(ByVal Value As Integer)
            m_iTransactionCurrencyID = Value
        End Set
    End Property

    Public WriteOnly Property PartyCnt() As Integer
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    Public WriteOnly Property SourceID() As Integer
        Set(ByVal Value As Integer)
            m_lSourceID = Value
        End Set
    End Property

    Public WriteOnly Property TransactionAmount() As Decimal
        Set(ByVal Value As Decimal)
            m_cTransactionAmount = Value
        End Set
    End Property

    Public WriteOnly Property ClaimID() As Integer
        Set(ByVal Value As Integer)
            m_lClaimID = Value
        End Set
    End Property

    Public WriteOnly Property ScreenMethod() As Integer
        Set(ByVal Value As Integer)
            m_lScreenMethod = Value
        End Set
    End Property

    Public WriteOnly Property LossCurrencyID() As Integer
        Set(ByVal Value As Integer)
            m_iLossCurrencyID = Value
        End Set
    End Property

    Public WriteOnly Property LossCurrencyAmount() As Decimal
        Set(ByVal Value As Decimal)
            m_cLossCurrencyAmount = Value
        End Set
    End Property


    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        Me.Close()

    End Sub

    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click

        m_lReturn = InterfaceToProperties()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTRue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to write interface to properties", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOk_Click")

            Exit Sub
        End If

        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        Me.Close()

    End Sub

    Public Function Initialise() As Integer
        Dim result As Integer = 0
        Dim lAccountID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            m_lReturn = CType(uctSIRMultiCurrency1, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTRue Then
                Throw New System.Exception(m_lReturn.ToString() + ", " + +", Failed to initialise multi-currency control")
            End If

            ' properties
            uctSIRMultiCurrency1.TransactionCurrencyID = m_iTransactionCurrencyID
            uctSIRMultiCurrency1.TransactionAmount = m_cTransactionAmount
            uctSIRMultiCurrency1.SourceID = m_lSourceID

            If m_lPartyCnt = 0 Then
                'This is the claim payable account

                m_lReturn = g_oBusiness.GetClaimPayableAccountID(lAccountID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTRue Then
                    Throw New System.Exception(m_lReturn.ToString() + ", " + +", Failed to get the Claim Payable Account")
                End If
                uctSIRMultiCurrency1.AccountID = lAccountID
            End If

            uctSIRMultiCurrency1.AccountPartyCnt = m_lPartyCnt
            uctSIRMultiCurrency1.EnableAccountGroup = True
            uctSIRMultiCurrency1.EnableBaseGroup = True
            uctSIRMultiCurrency1.EnableSystemGroup = True
            uctSIRMultiCurrency1.EnableTransactionGroup = True
            uctSIRMultiCurrency1.ShowAccountCurrency = True
            uctSIRMultiCurrency1.ShowSystemCurrency = True
            uctSIRMultiCurrency1.ShowLossCurrency = True
            uctSIRMultiCurrency1.LossCurrencyAmount = m_cLossCurrencyAmount
            uctSIRMultiCurrency1.LossCurrencyID = m_iLossCurrencyID

            m_lReturn = uctSIRMultiCurrency1.LoadControl()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTRue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load multi-currency control", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTRue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


	Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                 
            End If
        End If
        Me.disposedValue = True
    End Sub


    Public Function InterfaceToProperties() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTRue

            m_lReturn = uctSIRMultiCurrency1.OKClick()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTRue And m_lReturn <> gPMConstants.PMEReturnCode.PMFalse Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function uctSIRMultiCurrency1.OKClick failed.")
            End If


            m_lReturn = g_oBusiness.UpdateOverrideRates(v_lScreenMethod:=m_lScreenMethod, v_lClaimID:=m_lClaimID, v_lOverrideID:=uctSIRMultiCurrency1.RateOverrideReasonID, v_dtRateDate:=uctSIRMultiCurrency1.EffectiveDateOfExchange, v_dCurrencyBaseRate:=uctSIRMultiCurrency1.BaseExchangeRate, v_dAccountBaseRate:=uctSIRMultiCurrency1.AccountExchangeRate, v_dSystemBaseRate:=uctSIRMultiCurrency1.SystemExchangeRate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTRue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Function g_oBusiness.UpdatePaymentRates failed.")
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InterfaceToProperties failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Sub frmMultiCurrency_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        If m_lStatus = gPMConstants.PMEReturnCode.PMOK Then
            Exit Sub
        End If

        m_lReturn = MessageBox.Show("Changes will be lost." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Ok to cancel?", "Currency Conversions", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)

        If m_lReturn = System.Windows.Forms.DialogResult.Cancel Then
            Cancel = 1
        End If

        eventArgs.Cancel = Cancel <> 0
    End Sub
End Class