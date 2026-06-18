Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctSIRMultiCurrency_NET.uctSIRMultiCurrency")>
Partial Public Class uctSIRMultiCurrency
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    Private Const ACClass As String = "uctSIRMultiCurrencyControl"

    'Property Variables
    Private m_iAccountCurrencyID As Integer
    Private m_sAccountGroupLabel As String = ""
    Private m_lAccountID As Integer
    Private m_lAccountPartyCnt As Integer
    Private m_iBaseCurrencyID As Integer
    Private m_sBaseCurrencyCode As String = ""
    Private m_bEnableTransactionGroup As Boolean
    Private m_bEnableBaseGroup As Boolean
    Private m_bEnableAccountGroup As Boolean
    Private m_bEnableSystemGroup As Boolean
    Private m_lInsuranceFileCnt As Integer
    Private m_iLossCurrencyID As Integer
    Private m_cLossCurrencyAmount As Decimal
    Private m_bNoAmount As Boolean
    Private m_lPMUserID As Integer
    Private m_lRateOverrideReasonID As Integer
    Private m_sRateOverrideReasonText As Integer
    Private m_bShowAccountCurrency As Boolean
    Private m_bShowLossCurrency As Boolean
    Private m_bShowSystemCurrency As Boolean
    Private m_lSourceID As Integer
    Private m_iSystemCurrencyID As Integer
    Private m_cTransactionAmount As Decimal
    Private m_iTransactionCurrencyID As Integer
    Private m_dtEffectiveDateOfExchange As Date

    Private m_oBusiness As bACTCurrencyConvert.Form
    Private m_oUserAuthorities As bACTUserAuthorities.Business
    Private m_oFormFields As iPMFormControl.FormFields

    'Property Rates. The rates entered via properties and the rates that are saved to the insurance file.
    Private m_dAccountExchangeRate As Double
    Private m_dBaseExchangeRate As Double
    Private m_dSystemExchangeRate As Double
    'Display Rates. Show rate from transaction amount (i.e trans to base to account rate).
    Private m_dDisplayAccountExchangeRate As Double
    Private m_dDisplayBaseExchangeRate As Double
    Private m_dDisplaySystemExchangeRate As Double
    'Working Rates. Used to calculate the amounts.
    Private m_dTransToBaseExchangeRate As Double
    Private m_dAccountToBaseExchangeRate As Double
    Private m_dSystemToBaseExchangeRate As Double

    'Property Amounts. The amounts that will be returned via properties.
    Private m_cAccountAmount As Decimal
    Private m_cBaseAmount As Decimal
    Private m_cSystemAmount As Decimal
    'Working/Display Amounts. Used when calculating the amounts and displaing them.
    Private m_cAccountCurrentAmount As Decimal
    Private m_cBaseCurrentAmount As Decimal
    Private m_cSystemCurrentAmount As Decimal

    'Dates from insurance file.
    Private m_dtCurrencyBaseDate As Date
    Private m_dtAccountBaseDate As Date
    Private m_dtSystemBaseDate As Date

    Private m_bReasonEnabled As Boolean

    Private Const ACRateDecimalPlaces As Integer = 8
    Private Const ACRateFormat As String = "###0.00######"

    <Browsable(False)>
    Public ReadOnly Property AccountCurrencyID() As Integer
        Get
            Return m_iAccountCurrencyID
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property AccountAmount() As Decimal
        Get
            Return m_cAccountAmount
        End Get
    End Property

    <Browsable(True)>
    Public Property AccountExchangeRate() As Double
        Get
            Return m_dAccountExchangeRate
        End Get
        Set(ByVal Value As Double)
            m_dAccountExchangeRate = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property AccountGroupLabel() As String
        Set(ByVal Value As String)
            m_sAccountGroupLabel = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property AccountID() As Integer
        Set(ByVal Value As Integer)
            m_lAccountID = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property AccountPartyCnt() As Integer
        Set(ByVal Value As Integer)
            m_lAccountPartyCnt = Value
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property BaseCurrencyID() As Integer
        Get
            Return m_iBaseCurrencyID
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property BaseCurrencyCode() As String
        Get
            Return m_sBaseCurrencyCode
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property BaseAmount() As Decimal
        Get
            Return m_cBaseAmount
        End Get
    End Property

    <Browsable(True)>
    Public Property BaseExchangeRate() As Double
        Get
            Return m_dBaseExchangeRate
        End Get
        Set(ByVal Value As Double)
            m_dBaseExchangeRate = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property EnableTransactionGroup() As Boolean
        Set(ByVal Value As Boolean)
            m_bEnableTransactionGroup = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property EnableBaseGroup() As Boolean
        Set(ByVal Value As Boolean)
            m_bEnableBaseGroup = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property EnableAccountGroup() As Boolean
        Set(ByVal Value As Boolean)
            m_bEnableAccountGroup = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property EnableSystemGroup() As Boolean
        Set(ByVal Value As Boolean)
            m_bEnableSystemGroup = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property InsuranceFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property LossCurrencyID() As Integer
        Set(ByVal Value As Integer)
            m_iLossCurrencyID = Value
        End Set
    End Property

    <Browsable(False)>
    Public Property LossCurrencyAmount() As Decimal
        Get
            Return CDec(m_cLossCurrencyAmount)
        End Get
        Set(ByVal Value As Decimal)
            m_cLossCurrencyAmount = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property NoAmount() As Boolean
        Set(ByVal Value As Boolean)
            m_bNoAmount = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property PMUserID() As Integer
        Get
            Return m_lPMUserID
        End Get
        Set(ByVal Value As Integer)
            m_lPMUserID = Value
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property RateOverrideReasonID() As Integer
        Get
            Return m_lRateOverrideReasonID
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property RateOverrideReasonText() As String
        Get
            Return CStr(m_sRateOverrideReasonText)
        End Get
    End Property

    <Browsable(False)>
    Public WriteOnly Property ShowAccountCurrency() As Boolean
        Set(ByVal Value As Boolean)
            m_bShowAccountCurrency = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property ShowLossCurrency() As Boolean
        Set(ByVal Value As Boolean)
            m_bShowLossCurrency = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property ShowSystemCurrency() As Boolean
        Set(ByVal Value As Boolean)
            m_bShowSystemCurrency = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property SourceID() As Integer
        Set(ByVal Value As Integer)
            m_lSourceID = Value
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property SystemCurrencyID() As Integer
        Get
            Return m_iSystemCurrencyID
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property SystemAmount() As Decimal
        Get
            Return m_cSystemAmount
        End Get
    End Property

    <Browsable(True)>
    Public Property SystemExchangeRate() As Double
        Get
            Return m_dSystemExchangeRate
        End Get
        Set(ByVal Value As Double)
            m_dSystemExchangeRate = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property TransactionAmount() As Decimal
        Set(ByVal Value As Decimal)
            m_cTransactionAmount = Value
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property TransactionCurrencyID() As Integer
        Set(ByVal Value As Integer)
            m_iTransactionCurrencyID = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property EffectiveDateOfExchange() As Date
        Get
            Return m_dtEffectiveDateOfExchange
        End Get
        Set(ByVal Value As Date)
            m_dtEffectiveDateOfExchange = Value
        End Set
    End Property

    Public Function LoadControl() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Me.cboLossCurrency.FirstItem = ""
        Me.cboSystemCurrency.FirstItem = ""
        Me.cboReason.FirstItem = "(None)"
        Me.cboTransactionCurrency.FirstItem = ""
        Me.cboAccountCurrency.FirstItem = ""
        Me.cboBaseCurrency.FirstItem = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Validate fields using Forms Control
            lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Default the date.
            'DC160305 : PN19515 : if no date set then set today but if not use one thats passed in

            If Convert.IsDBNull(m_dtEffectiveDateOfExchange) Or IsNothing(m_dtEffectiveDateOfExchange) Or m_dtEffectiveDateOfExchange = DateTime.MinValue Then
                m_dtEffectiveDateOfExchange = DateTime.Today
            End If

            'Set up the form.
            If m_lInsuranceFileCnt <> 0 Then

                lReturn = m_oBusiness.GetInsuranceFileInformation(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_lCompanyID:=m_lSourceID, r_lAccountID:=m_lAccountID, r_iCurrencyID:=m_iTransactionCurrencyID, r_cPremium:=m_cTransactionAmount, r_dCurrencyBaseXrate:=m_dBaseExchangeRate, r_dtCurrencyBaseDate:=m_dtCurrencyBaseDate, r_dAccountBaseXrate:=m_dAccountExchangeRate, r_dtAccountBaseDate:=m_dtAccountBaseDate, r_dSystemBaseXrate:=m_dSystemExchangeRate, r_dtSystemBaseDate:=m_dtSystemBaseDate, r_lRateOverrideReasonID:=m_lRateOverrideReasonID)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If m_dtCurrencyBaseDate <> #12/30/1899# Then
                    'Default the date.
                    m_dtEffectiveDateOfExchange = m_dtCurrencyBaseDate
                End If
            Else
                If m_lAccountID = 0 And m_lAccountPartyCnt <> 0 Then

                    lReturn = m_oBusiness.GetAccountIDFromPartyCnt(v_lPartyCnt:=m_lAccountPartyCnt, v_lCompanyID:=m_lSourceID, r_lAccountID:=m_lAccountID)
                End If
            End If

            'Set working rates from those passed in.
            m_dTransToBaseExchangeRate = m_dBaseExchangeRate
            m_dAccountToBaseExchangeRate = m_dAccountExchangeRate
            m_dSystemToBaseExchangeRate = m_dSystemExchangeRate

            If Not m_bShowAccountCurrency Then
                m_bEnableAccountGroup = False
            End If
            If Not m_bShowSystemCurrency Then
                m_bEnableSystemGroup = False
            End If

            'Use current data to work out rates and amounts..
            lReturn = CType(Recalculate(), gPMConstants.PMEReturnCode)
            If lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMNotFound
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            ElseIf lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set the interface default values.
            lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set interface controls from the data.
            lReturn = CType(DataToInterface(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cboReason.Enabled = m_bReasonEnabled

            'Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadControl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function SetFieldValidation() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTransactionValue, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboTransactionCurrency, lFieldType:=gPMConstants.PMEDataType.PMLookup, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboEffectiveDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateShort, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboBaseCurrency, lFieldType:=gPMConstants.PMEDataType.PMLookup, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBaseCurrencyRate, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDouble, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory, lDecimalPlaces:=ACRateDecimalPlaces)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBaseCurrencyAmount, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboAccountCurrency, lFieldType:=gPMConstants.PMEDataType.PMLookup, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAccountCurrencyRate, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDouble, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory, lDecimalPlaces:=ACRateDecimalPlaces)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAccountCurrencyAmount, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtLossCurrencyAmount, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboSystemCurrency, lFieldType:=gPMConstants.PMEDataType.PMLookup, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSystemCurrencyRate, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDouble, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory, lDecimalPlaces:=ACRateDecimalPlaces)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSystemCurrencyAmount, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboReason, lFieldType:=gPMConstants.PMEDataType.PMLookup, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    Private Function SetInterfaceDefaults() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lOldHeight As Integer
        Dim bModifyDate, bModifyRate As Boolean
        Dim sOptionValue As String = ""
        Dim bEnableSystemFrame As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oUserAuthorities.GetDetails(vUserID:=g_iUserId)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_bNoAmount Then

                lReturn = m_oUserAuthorities.GetNext(vOverridePrePolicyDate:=bModifyDate, vOverridePrePolicyRate:=bModifyRate)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else

                lReturn = m_oUserAuthorities.GetNext(vOverrideDate:=bModifyDate, vOverrideRate:=bModifyRate)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Enable/disable the frames.
            fraTransaction.Enabled = m_bEnableTransactionGroup
            lblTransactionValue.Enabled = m_bEnableTransactionGroup
            lblTransactionCurrency.Enabled = m_bEnableTransactionGroup
            lblEffectiveDate.Enabled = m_bEnableTransactionGroup
            txtTransactionValue.Enabled = False
            cboTransactionCurrency.Enabled = False
            cboEffectiveDate.Enabled = m_bEnableTransactionGroup And bModifyDate

            fraBaseCurrency.Enabled = m_bEnableBaseGroup
            lblBaseCurrency.Enabled = m_bEnableBaseGroup
            lblBaseCurrencyRate.Enabled = m_bEnableBaseGroup
            lblBaseCurrencyAmount.Enabled = m_bEnableBaseGroup
            cboBaseCurrency.Enabled = False
            If m_iBaseCurrencyID = m_iTransactionCurrencyID Then
                txtBaseCurrencyRate.Enabled = False
            Else
                txtBaseCurrencyRate.Enabled = m_bEnableBaseGroup And bModifyRate
            End If
            txtBaseCurrencyAmount.Enabled = False

            fraAccountCurrency.Enabled = m_bEnableAccountGroup
            lblAccountCurrency.Enabled = m_bEnableAccountGroup
            lblAccountCurrencyRate.Enabled = m_bEnableAccountGroup
            lblAccountCurrencyAmount.Enabled = m_bEnableAccountGroup
            cboAccountCurrency.Enabled = False
            txtAccountCurrencyRate.Enabled = m_bEnableAccountGroup And bModifyRate
            txtAccountCurrencyAmount.Enabled = False

            iPMFunc.RetrieveSingleSystemOption(200, sOptionValue)
            bEnableSystemFrame = (sOptionValue <> "1")

            fraSystemCurrency.Enabled = m_bEnableSystemGroup And bEnableSystemFrame
            lblSystemCurrency.Enabled = m_bEnableSystemGroup And bEnableSystemFrame
            lblSystemCurrencyRate.Enabled = m_bEnableSystemGroup And bEnableSystemFrame
            lblSystemCurrencyAmount.Enabled = m_bEnableSystemGroup And bEnableSystemFrame
            cboSystemCurrency.Enabled = False
            If m_iSystemCurrencyID = m_iTransactionCurrencyID Then
                txtSystemCurrencyRate.Enabled = False
            Else
                txtSystemCurrencyRate.Enabled = m_bEnableSystemGroup And bModifyRate And bEnableSystemFrame
            End If
            txtSystemCurrencyAmount.Enabled = False

            'Change account currency frames caption if one has been passed in.
            If m_sAccountGroupLabel <> "" Then
                fraAccountCurrency.Text = m_sAccountGroupLabel
            End If

            'Hide account currency frame and move system currency frame up, if required.
            If Not m_bShowAccountCurrency Then
                fraAccountCurrency.Visible = False
                fraSystemCurrency.Top = fraAccountCurrency.Top
            End If

            'Hide system currency frame, if required.
            If Not m_bShowSystemCurrency Then
                fraSystemCurrency.Visible = False
            End If

            If m_bShowLossCurrency Then
                lblLossCurrency.Visible = True
                cboLossCurrency.Visible = True
                lblLossCurrencyAmount.Visible = True
                txtLossCurrencyAmount.Visible = True
                fraLossCurrency.Visible = True
                fraRateOverrideReason.Left = fraSystemCurrency.Left
            Else
                lblLossCurrency.Visible = False
                cboLossCurrency.Visible = False
                lblLossCurrencyAmount.Visible = False
                txtLossCurrencyAmount.Visible = False
                fraLossCurrency.Visible = False
                fraRateOverrideReason.Left = fraBaseCurrency.Left
            End If

            'Hide amount fields and shuffle fields up, if required
            If m_bNoAmount Then

                lblTransactionValue.Visible = False
                txtTransactionValue.Visible = False

                lOldHeight = fraTransaction.Height
                fraTransaction.Height -= lblTransactionCurrency.Top - lblTransactionValue.Top
                fraBaseCurrency.Top -= lOldHeight - fraTransaction.Height
                fraRateOverrideReason.Top -= lOldHeight - fraTransaction.Height

                lblEffectiveDate.Top = lblTransactionCurrency.Top
                cboEffectiveDate.Top = cboTransactionCurrency.Top
                lblTransactionCurrency.Top = lblTransactionValue.Top
                cboTransactionCurrency.Top = txtTransactionValue.Top

                lblBaseCurrencyAmount.Visible = False
                txtBaseCurrencyAmount.Visible = False

                lOldHeight = fraBaseCurrency.Height
                fraBaseCurrency.Height -= (lblBaseCurrencyAmount.Top + lblBaseCurrencyAmount.Height) - (lblBaseCurrencyRate.Top + lblBaseCurrencyRate.Height)
                fraRateOverrideReason.Top -= lOldHeight - fraBaseCurrency.Height

                If m_bShowAccountCurrency Then
                    lblAccountCurrencyAmount.Visible = False
                    txtAccountCurrencyAmount.Visible = False

                    lOldHeight = fraAccountCurrency.Height
                    fraAccountCurrency.Height -= (lblAccountCurrencyAmount.Top + lblAccountCurrencyAmount.Height) - (lblAccountCurrencyRate.Top + lblAccountCurrencyRate.Height)
                    fraAccountCurrency.Top = fraRateOverrideReason.Top
                    fraAccountCurrency.Left = fraRateOverrideReason.Left
                    fraRateOverrideReason.Top = fraAccountCurrency.Top + fraAccountCurrency.Height + (fraBaseCurrency.Top - (fraTransaction.Top + fraTransaction.Height))
                End If

                If m_bShowSystemCurrency Then
                    lblSystemCurrencyAmount.Visible = False
                    txtSystemCurrencyAmount.Visible = False

                    fraSystemCurrency.Height -= (lblSystemCurrencyAmount.Top + lblSystemCurrencyAmount.Height) - (lblSystemCurrencyRate.Top + lblSystemCurrencyRate.Height)
                    fraSystemCurrency.Top = fraRateOverrideReason.Top
                    fraSystemCurrency.Left = fraRateOverrideReason.Left
                    fraRateOverrideReason.Top = fraSystemCurrency.Top + fraSystemCurrency.Height + (fraBaseCurrency.Top - (fraTransaction.Top + fraTransaction.Height))
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function DisplayCaptions() As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            fraTransaction.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACfraTransaction, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTransactionValue.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblTransactionValue, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTransactionCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblTransactionCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblEffectiveDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblEffectiveDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            fraBaseCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACfraBaseCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBaseCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblBaseCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBaseCurrencyRate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblBaseCurrencyRate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBaseCurrencyAmount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblBaseCurrencyAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            fraAccountCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACfraAccountCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAccountCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblAccountCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAccountCurrencyRate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblAccountCurrencyRate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAccountCurrencyAmount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblAccountCurrencyAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            fraSystemCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACfraSystemCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblSystemCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblSystemCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblSystemCurrencyRate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblSystemCurrencyRate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblSystemCurrencyAmount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblSystemCurrencyAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            fraRateOverrideReason.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACfraRateOverrideReason, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblReason.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AClblReason, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function Initialise() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_iUserId = .UserID
            End With

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            'Get business object.
            Dim temp_m_oBusiness As Object
            lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get User Authorities Business
            Dim temp_m_oUserAuthorities As Object
            lReturn = g_oObjectManager.GetInstance(temp_m_oUserAuthorities, "bACTUserAuthorities.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oUserAuthorities = temp_m_oUserAuthorities
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function DataToInterface() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = m_oFormFields.FormatControl(ctlControl:=txtTransactionValue, vControlValue:=m_cTransactionAmount)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cboTransactionCurrency.ItemId = m_iTransactionCurrencyID

            cboEffectiveDate.Value = m_dtEffectiveDateOfExchange

            cboBaseCurrency.ItemId = m_iBaseCurrencyID

            If m_bEnableBaseGroup Then
                txtBaseCurrencyRate.Text = StringsHelper.Format(m_dDisplayBaseExchangeRate, ACRateFormat)
                lReturn = m_oFormFields.FormatControl(ctlControl:=txtBaseCurrencyAmount, vControlValue:=m_cBaseCurrentAmount)
            Else
                txtBaseCurrencyRate.Text = StringsHelper.Format(m_dBaseExchangeRate, ACRateFormat)
                lReturn = m_oFormFields.FormatControl(ctlControl:=txtBaseCurrencyAmount, vControlValue:=m_cBaseAmount)
            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_bShowAccountCurrency Then
                cboAccountCurrency.ItemId = m_iAccountCurrencyID

                If m_bEnableAccountGroup Then
                    txtAccountCurrencyRate.Text = StringsHelper.Format(m_dDisplayAccountExchangeRate, ACRateFormat)
                    lReturn = m_oFormFields.FormatControl(ctlControl:=txtAccountCurrencyAmount, vControlValue:=m_cAccountCurrentAmount)
                Else
                    txtAccountCurrencyRate.Text = StringsHelper.Format(m_dBaseExchangeRate / m_dAccountExchangeRate, ACRateFormat)
                    lReturn = m_oFormFields.FormatControl(ctlControl:=txtAccountCurrencyAmount, vControlValue:=m_cAccountAmount)
                End If

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If m_bShowSystemCurrency Then
                cboSystemCurrency.ItemId = m_iSystemCurrencyID

                If m_bEnableSystemGroup Then
                    txtSystemCurrencyRate.Text = StringsHelper.Format(m_dDisplaySystemExchangeRate, ACRateFormat)
                    lReturn = m_oFormFields.FormatControl(ctlControl:=txtSystemCurrencyAmount, vControlValue:=m_cSystemCurrentAmount)
                Else
                    txtSystemCurrencyRate.Text = StringsHelper.Format(m_dBaseExchangeRate / m_dSystemExchangeRate, ACRateFormat)
                    lReturn = m_oFormFields.FormatControl(ctlControl:=txtSystemCurrencyAmount, vControlValue:=m_cSystemAmount)
                End If

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If m_bShowLossCurrency Then
                cboLossCurrency.ItemId = m_iLossCurrencyID

                lReturn = m_oFormFields.FormatControl(ctlControl:=txtLossCurrencyAmount, vControlValue:=m_cLossCurrencyAmount)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            cboReason.ItemId = m_lRateOverrideReasonID

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DataToInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function Recalculate() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lSourceID = 0 Then m_lSourceID = 1
            lReturn = m_oBusiness.DoCurrencyConversion(v_lAccountID:=m_lAccountID, v_lCompanyID:=m_lSourceID, v_iCurrencyID:=m_iTransactionCurrencyID, v_cCurrencyAmountUnrounded:=m_cTransactionAmount, r_iBaseCurrencyID:=m_iBaseCurrencyID, r_cBaseAmount:=m_cBaseCurrentAmount, r_iAccountCurrencyID:=m_iAccountCurrencyID, r_cAccountAmount:=m_cAccountCurrentAmount, r_iSystemCurrencyID:=m_iSystemCurrencyID, r_cSystemAmount:=m_cSystemCurrentAmount, r_dCurrencyBaseXrate:=m_dTransToBaseExchangeRate, r_dtCurrencyBaseDate:=m_dtEffectiveDateOfExchange, r_dAccountBaseXrate:=m_dAccountToBaseExchangeRate, r_dtAccountBaseDate:=m_dtEffectiveDateOfExchange, r_dSystemBaseXrate:=m_dSystemToBaseExchangeRate, r_dtSystemBaseDate:=m_dtEffectiveDateOfExchange)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'A zero here means that the exchange rates have not been set up
            If m_dTransToBaseExchangeRate = 0 Or m_dAccountToBaseExchangeRate = 0 Or m_dSystemToBaseExchangeRate = 0 Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' If passed account id = 0 and passed party cnt = 0 and we don't have an account currency id then it must
            ' mean the account doesn't exist (for example, the CLMPAYABLE account before the first claim payment has
            ' been made). If this is the case, then set the account currency id and amount to the base currency id and
            ' amount.
            If m_lAccountID = 0 And m_lAccountPartyCnt = 0 And m_iAccountCurrencyID = 0 Then
                m_iAccountCurrencyID = m_iBaseCurrencyID
                If m_cAccountCurrentAmount = 0 Then
                    m_cAccountCurrentAmount = m_cBaseCurrentAmount
                End If
            End If

            If m_dBaseExchangeRate = 0 Then
                m_dBaseExchangeRate = m_dTransToBaseExchangeRate
            End If
            If m_dAccountExchangeRate = 0 Then
                m_dAccountExchangeRate = m_dAccountToBaseExchangeRate
            End If
            If m_dSystemExchangeRate = 0 Then
                m_dSystemExchangeRate = m_dSystemToBaseExchangeRate
            End If

            'Calculate display rates if we haven't already.
            If m_dDisplayBaseExchangeRate = 0 Then
                m_dDisplayBaseExchangeRate = m_dTransToBaseExchangeRate
            End If
            If m_dDisplayAccountExchangeRate = 0 And m_dAccountToBaseExchangeRate <> 0 Then
                m_dDisplayAccountExchangeRate = m_dTransToBaseExchangeRate / m_dAccountToBaseExchangeRate
            End If
            If m_dDisplaySystemExchangeRate = 0 And m_dSystemToBaseExchangeRate <> 0 Then
                m_dDisplaySystemExchangeRate = m_dTransToBaseExchangeRate / m_dSystemToBaseExchangeRate
            End If

            'Set initial amounts for return parameters.
            If Not m_bEnableBaseGroup And m_dBaseExchangeRate <> 0 And m_cBaseAmount = 0 Then
                m_cBaseAmount = m_cBaseCurrentAmount
            End If
            If Not m_bEnableAccountGroup And m_dAccountExchangeRate <> 0 And m_cAccountAmount = 0 Then
                m_cAccountAmount = m_cAccountCurrentAmount
            End If
            If Not m_bEnableSystemGroup And m_dSystemExchangeRate <> 0 And m_cSystemAmount = 0 Then
                m_cSystemAmount = m_cSystemCurrentAmount
            End If

            If (m_dTransToBaseExchangeRate <> m_dBaseExchangeRate) Or (m_dAccountToBaseExchangeRate <> m_dAccountExchangeRate) Or (m_dSystemToBaseExchangeRate <> m_dSystemExchangeRate) Then

                m_bReasonEnabled = True
            Else
                m_bReasonEnabled = False
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Recalculate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Recalculate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    Private Sub cboEffectiveDate_ValueChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboEffectiveDate.ValueChanged
        Dim dtCurrentEffectiveDate As Date

        If m_dtEffectiveDateOfExchange <> cboEffectiveDate.Value Then
            dtCurrentEffectiveDate = m_dtEffectiveDateOfExchange

            m_dtEffectiveDateOfExchange = cboEffectiveDate.Value

            m_dTransToBaseExchangeRate = 0
            m_dDisplayBaseExchangeRate = 0
            m_dAccountToBaseExchangeRate = 0
            m_dDisplayAccountExchangeRate = 0
            m_dSystemToBaseExchangeRate = 0
            m_dDisplaySystemExchangeRate = 0

            Recalculate()

            If m_dTransToBaseExchangeRate = 0 Or (m_dAccountToBaseExchangeRate = 0 And m_bShowAccountCurrency) Or (m_dSystemToBaseExchangeRate = 0 And m_bShowSystemCurrency) Then

                MessageBox.Show("Rates not set up for date entered", "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                'Reset effective date and recalculate.
                m_dtEffectiveDateOfExchange = dtCurrentEffectiveDate
                Recalculate()
            End If

            DataToInterface()
        End If

        cboReason.Enabled = m_bReasonEnabled
    End Sub

    Private Sub cboEffectiveDate_CloseUp(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboEffectiveDate.CloseUp
        cboEffectiveDate.Value = m_dtEffectiveDateOfExchange
        cboEffectiveDate.Refresh()
    End Sub

    Private Sub cboReason_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboReason.LostFocus
        m_lRateOverrideReasonID = cboReason.ItemId
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub txtAccountCurrencyRate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountCurrencyRate.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        cboReason.Enabled = True
    End Sub

    Private Sub txtAccountCurrencyRate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountCurrencyRate.Leave

        Dim dRateTemp As Double = gPMFunctions.ToSafeDouble(txtAccountCurrencyRate.Text, 0)

        If dRateTemp = 0 Then
            txtAccountCurrencyRate.Text = StringsHelper.Format(m_dDisplayAccountExchangeRate, ACRateFormat)
        Else
            If m_iAccountCurrencyID = m_iSystemCurrencyID Then
                txtSystemCurrencyRate.Text = StringsHelper.Format(txtAccountCurrencyRate.Text, ACRateFormat)
                txtSystemCurrencyRate_Leave(txtSystemCurrencyRate, New EventArgs())
            End If

            'If m_dDisplayAccountExchangeRate <> dRateTemp Then
            m_dDisplayAccountExchangeRate = dRateTemp
            m_dAccountToBaseExchangeRate = m_dTransToBaseExchangeRate / m_dDisplayAccountExchangeRate

            Recalculate()
            DataToInterface()
            'End If
        End If

        cboReason.Enabled = m_bReasonEnabled

    End Sub

    Private Sub txtBaseCurrencyRate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBaseCurrencyRate.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        cboReason.Enabled = True
    End Sub

    Private Sub txtBaseCurrencyRate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBaseCurrencyRate.Leave

        Dim dRateTemp As Double = gPMFunctions.ToSafeDouble(txtBaseCurrencyRate.Text, 0)

        If dRateTemp = 0 Then
            txtBaseCurrencyRate.Text = StringsHelper.Format(m_dDisplayBaseExchangeRate, ACRateFormat)
        Else
            If m_dDisplayBaseExchangeRate <> dRateTemp Then
                m_dDisplayBaseExchangeRate = dRateTemp

                m_dTransToBaseExchangeRate = m_dDisplayBaseExchangeRate

                If m_iBaseCurrencyID = m_iAccountCurrencyID Then
                    txtAccountCurrencyRate.Text = txtBaseCurrencyRate.Text
                    txtAccountCurrencyRate_Leave(txtAccountCurrencyRate, New EventArgs())
                Else
                    If m_dAccountToBaseExchangeRate <> 0 Then
                        'Base rate has changed so need to change the account rate so that the display account rate stays the same.
                        m_dAccountToBaseExchangeRate = (m_dDisplayBaseExchangeRate / m_dTransToBaseExchangeRate) * m_dAccountToBaseExchangeRate
                        m_dDisplayAccountExchangeRate = m_dDisplayBaseExchangeRate / m_dAccountToBaseExchangeRate
                    End If
                End If

                If m_iBaseCurrencyID = m_iSystemCurrencyID Then
                    txtSystemCurrencyRate.Text = StringsHelper.Format(txtBaseCurrencyRate.Text, ACRateFormat)
                    txtSystemCurrencyRate_Leave(txtSystemCurrencyRate, New EventArgs())
                Else
                    If m_dSystemToBaseExchangeRate <> 0 Then
                        'Base rate has changed so need to change the system rate so that the display system rate stays the same.
                        m_dSystemToBaseExchangeRate = (m_dDisplayBaseExchangeRate / m_dTransToBaseExchangeRate) * m_dSystemToBaseExchangeRate
                        m_dDisplaySystemExchangeRate = m_dDisplayBaseExchangeRate / m_dSystemToBaseExchangeRate
                    End If
                End If

                Recalculate()
                DataToInterface()
            End If
        End If

        cboReason.Enabled = m_bReasonEnabled

    End Sub

    Private Sub txtSystemCurrencyRate_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSystemCurrencyRate.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        cboReason.Enabled = True
    End Sub

    Private Sub txtSystemCurrencyRate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSystemCurrencyRate.Leave

        Dim dRateTemp As Double = gPMFunctions.ToSafeDouble(txtSystemCurrencyRate.Text, 0)

        If dRateTemp = 0 Then
            txtSystemCurrencyRate.Text = StringsHelper.Format(m_dDisplaySystemExchangeRate, ACRateFormat)
        Else
            'If m_dDisplaySystemExchangeRate <> dRateTemp Then
            m_dDisplaySystemExchangeRate = dRateTemp
            m_dSystemToBaseExchangeRate = m_dTransToBaseExchangeRate / m_dDisplaySystemExchangeRate

            Recalculate()
            DataToInterface()
            'End If
        End If

        cboReason.Enabled = m_bReasonEnabled

    End Sub

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If

            End If
        End If
        Me.disposedValue = True
    End Sub


    Public Function OKClick() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sMessage As String = ""
        Dim bZeroRates As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If cboReason.Enabled And m_lRateOverrideReasonID = 0 Then
                MessageBox.Show("You must enter a rate override reason.", "Invalid Reason", MessageBoxButtons.OK)
                Return gPMConstants.PMEReturnCode.PMCancel
            End If

            bZeroRates = False

            If m_bShowAccountCurrency Then
                If m_dDisplayAccountExchangeRate = 0 Then
                    bZeroRates = True
                End If
            End If

            If m_bShowSystemCurrency Then
                If m_dDisplaySystemExchangeRate = 0 Then
                    bZeroRates = True
                End If
            End If

            If m_dDisplayBaseExchangeRate = 0 Then
                bZeroRates = True
            End If

            If bZeroRates Then
                MessageBox.Show("Rates can not be zero.", "Invalid Rates", MessageBoxButtons.OK)
                Return gPMConstants.PMEReturnCode.PMCancel
            End If

            'Set working rates from those passed in.
            If m_bEnableBaseGroup Then
                If m_bShowAccountCurrency Then
                    If Not m_bEnableAccountGroup Then
                        m_dDisplayAccountExchangeRate = m_dBaseExchangeRate / m_dAccountExchangeRate
                        m_dAccountExchangeRate = m_dTransToBaseExchangeRate / m_dDisplayAccountExchangeRate
                    End If
                End If
                If m_bShowSystemCurrency Then
                    If Not m_bEnableSystemGroup Then
                        m_dDisplaySystemExchangeRate = m_dBaseExchangeRate / m_dSystemExchangeRate
                        m_dSystemExchangeRate = m_dTransToBaseExchangeRate / m_dDisplaySystemExchangeRate
                    End If
                End If
                m_dBaseExchangeRate = m_dTransToBaseExchangeRate
                m_cBaseAmount = m_cBaseCurrentAmount
                m_dtCurrencyBaseDate = m_dtEffectiveDateOfExchange
            End If

            If m_bShowAccountCurrency Then
                If m_bEnableAccountGroup Then
                    m_dAccountExchangeRate = m_dBaseExchangeRate / m_dDisplayAccountExchangeRate
                    m_cAccountAmount = m_cAccountCurrentAmount
                    m_dtAccountBaseDate = m_dtEffectiveDateOfExchange
                End If
            End If

            If m_bShowSystemCurrency Then
                If m_bEnableSystemGroup Then
                    m_dSystemExchangeRate = m_dBaseExchangeRate / m_dDisplaySystemExchangeRate
                    m_cSystemAmount = m_cSystemCurrentAmount
                    m_dtSystemBaseDate = m_dtEffectiveDateOfExchange
                End If
            End If

            If m_lInsuranceFileCnt <> 0 Then

                lReturn = m_oBusiness.UpdateInsuranceFile(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, v_dCurrencyBaseXrate:=m_dBaseExchangeRate, v_dtCurrencyBaseDate:=m_dtCurrencyBaseDate, v_dAccountBaseXrate:=m_dAccountExchangeRate, v_dtAccountBaseDate:=m_dtAccountBaseDate, v_dSystemBaseXrate:=m_dSystemExchangeRate, v_dtSystemBaseDate:=m_dtSystemBaseDate, v_lRateOverrideReasonID:=m_lRateOverrideReasonID, v_iBaseCurrencyID:=m_iBaseCurrencyID, v_iAccountCurrencyID:=m_iAccountCurrencyID)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OKClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OKClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub UserControl_Initialize()
        'Set this default
        m_bShowLossCurrency = False
        'Me.cboTransactionCurrency.FirstItem = ""
        'Me.cboBaseCurrency.FirstItem = ""
        'Me.cboSystemCurrency.FirstItem = ""
        'Me.cboLossCurrency.FirstItem = ""
        'Me.cboSystemCurrency.FirstItem = ""
    End Sub

    Private Sub uctSIRMultiCurrency_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub cboReason_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboReason.Leave
        m_lRateOverrideReasonID = cboReason.ItemId
    End Sub
End Class