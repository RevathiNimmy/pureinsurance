Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmMultiCurrency
    Inherits System.Windows.Forms.Form
    Implements IDisposable
    '############################################################################
    '
    ' Description:  Displayed from uctPMUPolicyControl.InterfaceToBusiness.
    '
    '               If policy and base currency are different AND ((User cannot
    '               change rates AND System Option 156 enabled) OR user
    '               can change rates)
    '
    ' History :
    ' 12052004 RDC created
    '############################################################################

    Private m_lReturn As Integer
    Private m_lStatus As gPMConstants.PMEReturnCode

    'Input
    Private m_iSourceId As Integer
    Private m_iTransactionCurrencyID As Integer

    'Output
    Private m_dBaseExchangeRate As Double
    Private m_dSystemExchangeRate As Double
    Private m_dtEffectiveDateOfExchange As Date
    Private m_iRateOverrideReasonID As Integer
    Private m_iBaseCurrencyID As Integer
    Private m_lPartyCnt As Integer

    Private Const ACClass As String = "frmMultiCurrency"


    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

    Public WriteOnly Property SourceId() As Integer
        Set(ByVal Value As Integer)
            m_iSourceId = Value
        End Set
    End Property

    Public WriteOnly Property TransactionCurrencyID() As Integer
        Set(ByVal Value As Integer)
            m_iTransactionCurrencyID = Value
        End Set
    End Property

    Public ReadOnly Property BaseExchangeRate() As Double
        Get
            Return m_dBaseExchangeRate
        End Get
    End Property

    Public ReadOnly Property SystemExchangeRate() As Double
        Get
            Return m_dSystemExchangeRate
        End Get
    End Property

    Public ReadOnly Property EffectiveDateOfExchange() As Date
        Get
            Return m_dtEffectiveDateOfExchange
        End Get
    End Property

    Public ReadOnly Property RateOverrideReasonID() As Integer
        Get
            Return m_iRateOverrideReasonID
        End Get
    End Property

    Public ReadOnly Property BaseCurrencyID() As Integer
        Get
            Return m_iBaseCurrencyID
        End Get
    End Property

    Public WriteOnly Property PartyCnt() As Integer
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        Me.Close()

    End Sub

    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click

        m_lReturn = InterfaceToProperties()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        Me.Close()

    End Sub

    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
            'Developer Guide No.9
            m_lReturn = uctSIRMultiCurrency1.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise multi-currency control", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' properties
            uctSIRMultiCurrency1.SourceID = m_iSourceId
            uctSIRMultiCurrency1.TransactionCurrencyID = m_iTransactionCurrencyID
            uctSIRMultiCurrency1.AccountPartyCnt = m_lPartyCnt
            uctSIRMultiCurrency1.NoAmount = True
            uctSIRMultiCurrency1.ShowSystemCurrency = True
            uctSIRMultiCurrency1.EnableBaseGroup = True
            uctSIRMultiCurrency1.EnableSystemGroup = True
            uctSIRMultiCurrency1.EnableTransactionGroup = True

            m_lReturn = uctSIRMultiCurrency1.LoadControl()

            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                'Rates not set up so tell the user what to do.
                MessageBox.Show("The exchange rates for this branch have not been set up." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                "Until this has been completed, you will not be able to continue.", "Incomplete Currency Rates", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return gPMConstants.PMEReturnCode.PMNotFound
            ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load multi-currency control", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

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

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = uctSIRMultiCurrency1.OKClick()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_dBaseExchangeRate = uctSIRMultiCurrency1.BaseExchangeRate
            m_dSystemExchangeRate = uctSIRMultiCurrency1.SystemExchangeRate
            m_dtEffectiveDateOfExchange = uctSIRMultiCurrency1.EffectiveDateOfExchange
            m_iRateOverrideReasonID = uctSIRMultiCurrency1.RateOverrideReasonID
            m_iBaseCurrencyID = uctSIRMultiCurrency1.BaseCurrencyID


            Return gPMConstants.PMEReturnCode.PMTrue

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