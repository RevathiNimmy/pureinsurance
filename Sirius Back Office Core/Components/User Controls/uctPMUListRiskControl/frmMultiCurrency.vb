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
    ' Description:  Display frmMultiCurrency if policy and base currency are
    '               different AND ((User cannot change rates AND System Option
    '               156 enabled) OR user can change rates)
    '
    ' History :
    ' 12052004 RDC created
    '############################################################################

    Private m_lReturn As Integer
    Private m_lStatus As gPMConstants.PMEReturnCode

    Private m_lLeadAgentCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_lTransactionCurrencyID As Integer
    Private m_bDirectBusiness As Boolean

    Private Const ACClass As String = "frmMultiCurrency"

    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property

    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public WriteOnly Property DirectBusiness() As Boolean
        Set(ByVal Value As Boolean)
            m_bDirectBusiness = Value
        End Set
    End Property

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        Me.Close()

    End Sub

    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click

        m_lReturn = InterfaceToProperties()

        'RKS 230904 PN14885
        'OnClick method of uctMultiCurrency returns False in case the
        'rate override reason is not entered (it's only a warning and
        'uctMultiCurrency data screen is not completed yet
        'so exit silently
        If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
            Exit Sub
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to write interface to properties", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOk_Click")

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

            'm_lReturn = CType(uctSIRMultiCurrency1, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            m_lReturn = uctSIRMultiCurrency1.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise multi-currency control", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' properties
            uctSIRMultiCurrency1.InsuranceFileCnt = m_lInsuranceFileCnt
            uctSIRMultiCurrency1.EnableTransactionGroup = True
            uctSIRMultiCurrency1.EnableAccountGroup = True
            uctSIRMultiCurrency1.EnableBaseGroup = False
            'uctSIRMultiCurrency1.EnableSystemGroup = False
            uctSIRMultiCurrency1.EnableSystemGroup = True
            uctSIRMultiCurrency1.ShowAccountCurrency = True
            uctSIRMultiCurrency1.ShowSystemCurrency = True
            If m_bDirectBusiness Then
                uctSIRMultiCurrency1.AccountGroupLabel = "Client Account Currency"
            Else
                uctSIRMultiCurrency1.AccountGroupLabel = "Lead Agent Account Currency"
            End If

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


    Private Function InterfaceToProperties() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = uctSIRMultiCurrency1.OKClick()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


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