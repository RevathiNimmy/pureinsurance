' WPR - Multicurrency Maintenance - Modal Dialog
Imports System.Web.Configuration.WebConfigurationManager
Imports CrystalDecisions.ReportAppServer.DataDefModel
Imports Nexus.Constants
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports NexusProvider

Namespace Nexus
    Partial Class Modal_Multicurrency : Inherits System.Web.UI.Page

        Dim oWebservice As NexusProvider.ProviderBase
        Dim oBusiness As Object
        Dim oUserAuthorities As Object

        ' Property Variables
        Private Property m_iAccountCurrencyID As Integer
            Get
                Return If(ViewState("m_iAccountCurrencyID") IsNot Nothing, CInt(ViewState("m_iAccountCurrencyID")), 0)
            End Get
            Set(v As Integer)
                ViewState("m_iAccountCurrencyID") = v
            End Set
        End Property
        Private Property m_lAccountID As Integer
            Get
                Return If(ViewState("m_lAccountID") IsNot Nothing, CInt(ViewState("m_lAccountID")), 0)
            End Get
            Set(v As Integer)
                ViewState("m_lAccountID") = v
            End Set
        End Property
        Private Property m_lAccountPartyCnt As Integer
            Get
                Return If(ViewState("m_lAccountPartyCnt") IsNot Nothing, CInt(ViewState("m_lAccountPartyCnt")), 0)
            End Get
            Set(v As Integer)
                ViewState("m_lAccountPartyCnt") = v
            End Set
        End Property
        Private Property m_iBaseCurrencyID As Integer
            Get
                Return If(ViewState("m_iBaseCurrencyID") IsNot Nothing, CInt(ViewState("m_iBaseCurrencyID")), 0)
            End Get
            Set(v As Integer)
                ViewState("m_iBaseCurrencyID") = v
            End Set
        End Property
        Private Property m_sBaseCurrencyCode As String
            Get
                Return If(ViewState("m_sBaseCurrencyCode") IsNot Nothing, CStr(ViewState("m_sBaseCurrencyCode")), "")
            End Get
            Set(v As String)
                ViewState("m_sBaseCurrencyCode") = v
            End Set
        End Property
        Private Property m_bEnableTransactionGroup As Boolean
            Get
                Return If(ViewState("m_bEnableTransactionGroup") IsNot Nothing, CBool(ViewState("m_bEnableTransactionGroup")), False)
            End Get
            Set(v As Boolean)
                ViewState("m_bEnableTransactionGroup") = v
            End Set
        End Property
        Private Property m_bEnableBaseGroup As Boolean
            Get
                Return If(ViewState("m_bEnableBaseGroup") IsNot Nothing, CBool(ViewState("m_bEnableBaseGroup")), False)
            End Get
            Set(v As Boolean)
                ViewState("m_bEnableBaseGroup") = v
            End Set
        End Property
        Private Property m_bEnableAccountGroup As Boolean
            Get
                Return If(ViewState("m_bEnableAccountGroup") IsNot Nothing, CBool(ViewState("m_bEnableAccountGroup")), False)
            End Get
            Set(v As Boolean)
                ViewState("m_bEnableAccountGroup") = v
            End Set
        End Property
        Private Property m_bEnableSystemGroup As Boolean
            Get
                Return If(ViewState("m_bEnableSystemGroup") IsNot Nothing, CBool(ViewState("m_bEnableSystemGroup")), False)
            End Get
            Set(v As Boolean)
                ViewState("m_bEnableSystemGroup") = v
            End Set
        End Property
        Private Property m_lInsuranceFileCnt As Integer
            Get
                Return If(ViewState("m_lInsuranceFileCnt") IsNot Nothing, CInt(ViewState("m_lInsuranceFileCnt")), 0)
            End Get
            Set(v As Integer)
                ViewState("m_lInsuranceFileCnt") = v
            End Set
        End Property
        Private Property m_iLossCurrencyID As Integer
            Get
                Return If(ViewState("m_iLossCurrencyID") IsNot Nothing, CInt(ViewState("m_iLossCurrencyID")), 0)
            End Get
            Set(v As Integer)
                ViewState("m_iLossCurrencyID") = v
            End Set
        End Property
        Private Property m_cLossCurrencyAmount As Decimal
            Get
                Return If(ViewState("m_cLossCurrencyAmount") IsNot Nothing, CDec(ViewState("m_cLossCurrencyAmount")), 0D)
            End Get
            Set(v As Decimal)
                ViewState("m_cLossCurrencyAmount") = v
            End Set
        End Property
        Private Property m_lSourceID As Integer
            Get
                Return If(ViewState("m_lSourceID") IsNot Nothing, CInt(ViewState("m_lSourceID")), 0)
            End Get
            Set(v As Integer)
                ViewState("m_lSourceID") = v
            End Set
        End Property
        Private Property m_iSystemCurrencyID As Integer
            Get
                Return If(ViewState("m_iSystemCurrencyID") IsNot Nothing, CInt(ViewState("m_iSystemCurrencyID")), 0)
            End Get
            Set(v As Integer)
                ViewState("m_iSystemCurrencyID") = v
            End Set
        End Property
        Private Property m_cTransactionAmount As Decimal
            Get
                Return If(ViewState("m_cTransactionAmount") IsNot Nothing, CDec(ViewState("m_cTransactionAmount")), 0D)
            End Get
            Set(v As Decimal)
                ViewState("m_cTransactionAmount") = v
            End Set
        End Property
        Private Property m_iTransactionCurrencyID As Integer
            Get
                Return If(ViewState("m_iTransactionCurrencyID") IsNot Nothing, CInt(ViewState("m_iTransactionCurrencyID")), 0)
            End Get
            Set(v As Integer)
                ViewState("m_iTransactionCurrencyID") = v
            End Set
        End Property
        Private Property m_dtEffectiveDateOfExchange As Date
            Get
                Return If(ViewState("m_dtEffectiveDateOfExchange") IsNot Nothing, CDate(ViewState("m_dtEffectiveDateOfExchange")), Date.MinValue)
            End Get
            Set(v As Date)
                ViewState("m_dtEffectiveDateOfExchange") = v
            End Set
        End Property
        Private Property m_lRateOverrideReasonID As Integer
            Get
                Return If(ViewState("m_lRateOverrideReasonID") IsNot Nothing, CInt(ViewState("m_lRateOverrideReasonID")), 0)
            End Get
            Set(v As Integer)
                ViewState("m_lRateOverrideReasonID") = v
            End Set
        End Property
        Private Property m_bShowAccountCurrency As Boolean
            Get
                Return If(ViewState("m_bShowAccountCurrency") IsNot Nothing, CBool(ViewState("m_bShowAccountCurrency")), False)
            End Get
            Set(v As Boolean)
                ViewState("m_bShowAccountCurrency") = v
            End Set
        End Property
        Private Property m_bShowLossCurrency As Boolean
            Get
                Return If(ViewState("m_bShowLossCurrency") IsNot Nothing, CBool(ViewState("m_bShowLossCurrency")), False)
            End Get
            Set(v As Boolean)
                ViewState("m_bShowLossCurrency") = v
            End Set
        End Property
        Private Property m_bShowSystemCurrency As Boolean
            Get
                Return If(ViewState("m_bShowSystemCurrency") IsNot Nothing, CBool(ViewState("m_bShowSystemCurrency")), False)
            End Get
            Set(v As Boolean)
                ViewState("m_bShowSystemCurrency") = v
            End Set
        End Property
        Private Property m_bNoAmount As Boolean
            Get
                Return If(ViewState("m_bNoAmount") IsNot Nothing, CBool(ViewState("m_bNoAmount")), False)
            End Get
            Set(v As Boolean)
                ViewState("m_bNoAmount") = v
            End Set
        End Property

        ' Exchange Rates
        Private Property m_dAccountExchangeRate As Double
            Get
                Return If(ViewState("m_dAccountExchangeRate") IsNot Nothing, CDbl(ViewState("m_dAccountExchangeRate")), 0.0)
            End Get
            Set(v As Double)
                ViewState("m_dAccountExchangeRate") = v
            End Set
        End Property
        Private Property m_dBaseExchangeRate As Double
            Get
                Return If(ViewState("m_dBaseExchangeRate") IsNot Nothing, CDbl(ViewState("m_dBaseExchangeRate")), 0.0)
            End Get
            Set(v As Double)
                ViewState("m_dBaseExchangeRate") = v
            End Set
        End Property
        Private Property m_dSystemExchangeRate As Double
            Get
                Return If(ViewState("m_dSystemExchangeRate") IsNot Nothing, CDbl(ViewState("m_dSystemExchangeRate")), 0.0)
            End Get
            Set(v As Double)
                ViewState("m_dSystemExchangeRate") = v
            End Set
        End Property
        Private Property m_dLossExchangeRate As Double
            Get
                Return If(ViewState("m_dLossExchangeRate") IsNot Nothing, CDbl(ViewState("m_dLossExchangeRate")), 0.0)
            End Get
            Set(v As Double)
                ViewState("m_dLossExchangeRate") = v
            End Set
        End Property
        Private Property m_dDisplayAccountExchangeRate As Double
            Get
                Return If(ViewState("m_dDisplayAccountExchangeRate") IsNot Nothing, CDbl(ViewState("m_dDisplayAccountExchangeRate")), 0.0)
            End Get
            Set(v As Double)
                ViewState("m_dDisplayAccountExchangeRate") = v
            End Set
        End Property
        Private Property m_dDisplayBaseExchangeRate As Double
            Get
                Return If(ViewState("m_dDisplayBaseExchangeRate") IsNot Nothing, CDbl(ViewState("m_dDisplayBaseExchangeRate")), 0.0)
            End Get
            Set(v As Double)
                ViewState("m_dDisplayBaseExchangeRate") = v
            End Set
        End Property
        Private Property m_dDisplaySystemExchangeRate As Double
            Get
                Return If(ViewState("m_dDisplaySystemExchangeRate") IsNot Nothing, CDbl(ViewState("m_dDisplaySystemExchangeRate")), 0.0)
            End Get
            Set(v As Double)
                ViewState("m_dDisplaySystemExchangeRate") = v
            End Set
        End Property
        Private Property m_dTransToBaseExchangeRate As Double
            Get
                Return If(ViewState("m_dTransToBaseExchangeRate") IsNot Nothing, CDbl(ViewState("m_dTransToBaseExchangeRate")), 0.0)
            End Get
            Set(v As Double)
                ViewState("m_dTransToBaseExchangeRate") = v
            End Set
        End Property
        Private Property m_dAccountToBaseExchangeRate As Double
            Get
                Return If(ViewState("m_dAccountToBaseExchangeRate") IsNot Nothing, CDbl(ViewState("m_dAccountToBaseExchangeRate")), 0.0)
            End Get
            Set(v As Double)
                ViewState("m_dAccountToBaseExchangeRate") = v
            End Set
        End Property
        Private Property m_dSystemToBaseExchangeRate As Double
            Get
                Return If(ViewState("m_dSystemToBaseExchangeRate") IsNot Nothing, CDbl(ViewState("m_dSystemToBaseExchangeRate")), 0.0)
            End Get
            Set(v As Double)
                ViewState("m_dSystemToBaseExchangeRate") = v
            End Set
        End Property
        Private Property m_dLossToBaseExchangeRate As Double
            Get
                Return If(ViewState("m_dLossToBaseExchangeRate") IsNot Nothing, CDbl(ViewState("m_dLossToBaseExchangeRate")), 0.0)
            End Get
            Set(v As Double)
                ViewState("m_dLossToBaseExchangeRate") = v
            End Set
        End Property

        ' Amounts
        Private Property m_cAccountAmount As Decimal
            Get
                Return If(ViewState("m_cAccountAmount") IsNot Nothing, CDec(ViewState("m_cAccountAmount")), 0D)
            End Get
            Set(v As Decimal)
                ViewState("m_cAccountAmount") = v
            End Set
        End Property
        Private Property m_cBaseAmount As Decimal
            Get
                Return If(ViewState("m_cBaseAmount") IsNot Nothing, CDec(ViewState("m_cBaseAmount")), 0D)
            End Get
            Set(v As Decimal)
                ViewState("m_cBaseAmount") = v
            End Set
        End Property
        Private Property m_cSystemAmount As Decimal
            Get
                Return If(ViewState("m_cSystemAmount") IsNot Nothing, CDec(ViewState("m_cSystemAmount")), 0D)
            End Get
            Set(v As Decimal)
                ViewState("m_cSystemAmount") = v
            End Set
        End Property
        Private Property m_cLossAmount As Decimal
            Get
                Return If(ViewState("m_cLossAmount") IsNot Nothing, CDec(ViewState("m_cLossAmount")), 0D)
            End Get
            Set(v As Decimal)
                ViewState("m_cLossAmount") = v
            End Set
        End Property
        Private Property m_cAccountCurrentAmount As Decimal
            Get
                Return If(ViewState("m_cAccountCurrentAmount") IsNot Nothing, CDec(ViewState("m_cAccountCurrentAmount")), 0D)
            End Get
            Set(v As Decimal)
                ViewState("m_cAccountCurrentAmount") = v
            End Set
        End Property
        Private Property m_cBaseCurrentAmount As Decimal
            Get
                Return If(ViewState("m_cBaseCurrentAmount") IsNot Nothing, CDec(ViewState("m_cBaseCurrentAmount")), 0D)
            End Get
            Set(v As Decimal)
                ViewState("m_cBaseCurrentAmount") = v
            End Set
        End Property
        Private Property m_cSystemCurrentAmount As Decimal
            Get
                Return If(ViewState("m_cSystemCurrentAmount") IsNot Nothing, CDec(ViewState("m_cSystemCurrentAmount")), 0D)
            End Get
            Set(v As Decimal)
                ViewState("m_cSystemCurrentAmount") = v
            End Set
        End Property
        Private Property m_cLossCurrentAmount As Decimal
            Get
                Return If(ViewState("m_cLossCurrentAmount") IsNot Nothing, CDec(ViewState("m_cLossCurrentAmount")), 0D)
            End Get
            Set(v As Decimal)
                ViewState("m_cLossCurrentAmount") = v
            End Set
        End Property

        ' Dates
        Private Property m_dtCurrencyBaseDate As Date
            Get
                Return If(ViewState("m_dtCurrencyBaseDate") IsNot Nothing, CDate(ViewState("m_dtCurrencyBaseDate")), Date.MinValue)
            End Get
            Set(v As Date)
                ViewState("m_dtCurrencyBaseDate") = v
            End Set
        End Property
        Private Property m_dtAccountBaseDate As Date
            Get
                Return If(ViewState("m_dtAccountBaseDate") IsNot Nothing, CDate(ViewState("m_dtAccountBaseDate")), Date.MinValue)
            End Get
            Set(v As Date)
                ViewState("m_dtAccountBaseDate") = v
            End Set
        End Property
        Private Property m_dtSystemBaseDate As Date
            Get
                Return If(ViewState("m_dtSystemBaseDate") IsNot Nothing, CDate(ViewState("m_dtSystemBaseDate")), Date.MinValue)
            End Get
            Set(v As Date)
                ViewState("m_dtSystemBaseDate") = v
            End Set
        End Property

        Private Property m_bReasonEnabled As Boolean
            Get
                Return If(ViewState("m_bReasonEnabled") IsNot Nothing, CBool(ViewState("m_bReasonEnabled")), False)
            End Get
            Set(v As Boolean)
                ViewState("m_bReasonEnabled") = v
            End Set
        End Property

        Private Const ACRateFormat As String = "###0.00######"
        Dim bCanOverrideExchangeDate As Boolean = False
        Dim bCanOverrideExchangeRate As Boolean = False

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                Try
                    oWebservice = New NexusProvider.ProviderManager().Provider
                    Dim sProcessName As String = String.Empty
                    If Request.QueryString("ProcessName") IsNot Nothing Then
                        sProcessName = Request.QueryString("ProcessName")
                    End If

                    Dim oQuote As NexusProvider.Quote = Session(CNQuote)

                    Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
                    For iCount As Integer = 0 To oUserDetails.ListOfBranches.Count - 1
                        If oUserDetails.ListOfBranches(iCount).Code.ToUpper() = Session(CNBranchCode).ToString.ToUpper() Then
                            m_lSourceID = oUserDetails.ListOfBranches(iCount).BranchKey
                            Exit For
                        End If
                    Next

                    If Request.QueryString("TransactionCurrencyCode") <> "" Then
                        Dim sValue As String = GetDescriptionForCode(NexusProvider.ListType.PMLookup, Request.QueryString("TransactionCurrencyCode").ToString(), "Currency")
                        m_iTransactionCurrencyID = GetKeyForDescription(NexusProvider.ListType.PMLookup, sValue, "Currency")
                    End If

                    If Request.QueryString("TransactionAmount") IsNot Nothing Then
                        m_cTransactionAmount = CDec(Request.QueryString("TransactionAmount"))
                    End If

                    ' Use process name for conditional logic
                    Select Case sProcessName.ToUpper()
                        Case "POLICY"
                            m_lAccountPartyCnt = oQuote.PartyKey
                            m_iTransactionCurrencyID = oQuote.TransCurrencyID
                            m_bNoAmount = True
                            m_bShowSystemCurrency = True
                            m_bEnableBaseGroup = True
                            m_bEnableSystemGroup = True
                            m_bEnableTransactionGroup = True

                        Case "CLAIM PAYMENT"
                            If Request.QueryString("LossCurrencyAmount") IsNot Nothing Then
                                m_cLossCurrencyAmount = CDec(Request.QueryString("LossCurrencyAmount"))
                            End If

                            If Request.QueryString("LossCurrencyCode") <> "" Then
                                m_iLossCurrencyID = GetKeyForDescription(NexusProvider.ListType.PMLookup, Request.QueryString("LossCurrencyCode").ToString(), "Currency")
                            End If

                            m_bEnableAccountGroup = True
                            m_bEnableBaseGroup = True
                            m_bEnableSystemGroup = True
                            m_bEnableTransactionGroup = True
                            m_bShowAccountCurrency = True
                            m_bShowSystemCurrency = True
                            m_bShowLossCurrency = True

                        Case "RECEIPT"

                            m_bShowAccountCurrency = True
                            m_bEnableAccountGroup = True
                            m_bShowSystemCurrency = True
                            m_bEnableBaseGroup = True
                            m_bEnableSystemGroup = True
                            m_bEnableTransactionGroup = True

                        Case "PAYMENT"

                            m_bShowAccountCurrency = True
                            m_bEnableAccountGroup = True
                            m_bShowSystemCurrency = True
                            m_bEnableBaseGroup = True
                            m_bEnableSystemGroup = True
                            m_bEnableTransactionGroup = True

                    End Select

                    LoadParameters()

                    If m_dtEffectiveDateOfExchange = DateTime.MinValue Then
                        m_dtEffectiveDateOfExchange = DateTime.Today
                    End If

                    LoadInsuranceFileData()

                    m_dTransToBaseExchangeRate = m_dBaseExchangeRate
                    m_dAccountToBaseExchangeRate = m_dAccountExchangeRate
                    m_dSystemToBaseExchangeRate = m_dSystemExchangeRate

                    If Not m_bShowAccountCurrency Then
                        m_bEnableAccountGroup = False
                    End If
                    If Not m_bShowSystemCurrency Then
                        m_bEnableSystemGroup = False
                    End If

                    ' Recalculate rates and amounts
                    Recalculate()
                    CheckUserPermissions()
                    ApplyFieldPermissions()
                    DataToInterface()
                Finally
                    'oWebservice = Nothing
                End Try
            End If
        End Sub

        Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Private Sub LoadParameters()
            If Request.QueryString("InsuranceFileCnt") IsNot Nothing Then
                m_lInsuranceFileCnt = CInt(Request.QueryString("InsuranceFileCnt"))
            End If
            If Request.QueryString("AccountID") IsNot Nothing Then
                m_lAccountID = CInt(Request.QueryString("AccountID"))
            End If
            If Request.QueryString("EffectiveDate") IsNot Nothing Then
                m_dtEffectiveDateOfExchange = CDate(Request.QueryString("EffectiveDate"))
            End If
        End Sub

        Private Sub LoadInsuranceFileData()
            Try
                oWebservice = New NexusProvider.ProviderManager().Provider
                Dim oGetInsuranceFileInformation As NexusProvider.GetInsuranceFileInformation

                If Convert.IsDBNull(m_dtEffectiveDateOfExchange) Or IsNothing(m_dtEffectiveDateOfExchange) Or m_dtEffectiveDateOfExchange = DateTime.MinValue Then
                    m_dtEffectiveDateOfExchange = DateTime.Today
                End If

                If m_lInsuranceFileCnt <> 0 Then
                    oGetInsuranceFileInformation = oWebService.GetInsuranceFileInformation(m_lInsuranceFileCnt)
                    If oGetInsuranceFileInformation IsNot Nothing Then
                        m_lSourceID = oGetInsuranceFileInformation.CompanyId
                        m_lAccountID = oGetInsuranceFileInformation.AccountId
                        m_iTransactionCurrencyID = oGetInsuranceFileInformation.CurrencyId
                        m_cTransactionAmount = oGetInsuranceFileInformation.Premium
                        m_dBaseExchangeRate = oGetInsuranceFileInformation.CurrencyBaseXrate
                        m_dtCurrencyBaseDate = oGetInsuranceFileInformation.CurrencyBaseDate
                        m_dAccountExchangeRate = oGetInsuranceFileInformation.AccountBaseXrate
                        m_dtAccountBaseDate = oGetInsuranceFileInformation.AccountBaseDate
                        m_dSystemExchangeRate = oGetInsuranceFileInformation.SystemBaseXrate
                        m_dtSystemBaseDate = oGetInsuranceFileInformation.SystemBaseDate
                        m_lRateOverrideReasonID = oGetInsuranceFileInformation.RateOverrideReasonId
                    End If

                    If m_dtCurrencyBaseDate <> #12/30/1899# Then
                        m_dtEffectiveDateOfExchange = m_dtCurrencyBaseDate
                    End If
                Else
                    If m_lAccountID = 0 And m_lAccountPartyCnt <> 0 Then
                        m_lAccountID = oWebservice.GetAccountIdFromPartyCnt(m_lAccountPartyCnt, m_lSourceID)
                    End If
                End If
            Catch ex As Exception
            End Try
        End Sub

        Private Sub CheckUserPermissions()
            Try
                oWebservice = New NexusProvider.ProviderManager().Provider
                Dim oCurrencyOverride = oWebService.GetCurrencyOverride()

                If Request.QueryString("ProcessName").ToUpper() = "POLICY" Then
                    bCanOverrideExchangeDate = oCurrencyOverride.PrePolicyDateOverrideAllowed
                    bCanOverrideExchangeRate = oCurrencyOverride.PrePolicyRateOverrideAllowed
                Else
                    bCanOverrideExchangeDate = oCurrencyOverride.DateOverrideAllowed
                    bCanOverrideExchangeRate = oCurrencyOverride.RateOverrideAllowed
                End If

            Catch ex As Exception
                bCanOverrideExchangeDate = False
                bCanOverrideExchangeRate = False
            End Try
        End Sub

        Private Sub ApplyFieldPermissions()

            ddlTransactionCurrency.Enabled = False
            txtTransactionAmount.Enabled = False
            txtDateOfExchange.Enabled = m_bEnableTransactionGroup And bCanOverrideExchangeDate

            ddlBaseCurrency.Enabled = False
            txtBaseCurrencyRate.Enabled = m_bEnableBaseGroup And bCanOverrideExchangeRate And (m_iBaseCurrencyID <> m_iTransactionCurrencyID)
            txtBaseCurrencyAmount.Enabled = False

            ddlAccountCurrency.Enabled = False
            txtAccountCurrencyRate.Enabled = m_bEnableAccountGroup And bCanOverrideExchangeRate
            txtAccountCurrencyAmount.Enabled = False

            oWebservice = New NexusProvider.ProviderManager().Provider
            Dim oSystemOption As NexusProvider.OptionTypeSetting = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 200)

            ddlSystemCurrency.Enabled = False
            txtSystemCurrencyAmount.Enabled = False
            If m_iSystemCurrencyID = m_iTransactionCurrencyID Then
                txtSystemCurrencyRate.Enabled = False
            Else
                txtSystemCurrencyRate.Enabled = m_bEnableSystemGroup And bCanOverrideExchangeRate And oSystemOption.OptionValue <> "1"
            End If

            ' Hide Account Currency section only when not required
            If Not m_bShowAccountCurrency Then
                pnlAccountCurrencySection.Visible = False
            End If

            ' Hide System Currency section only when not required
            If Not m_bShowSystemCurrency Then
                pnlSystemCurrencySection.Visible = False
            End If

            ' Show/Hide Loss Currency section
            If m_bShowLossCurrency Then
                pnlLossCurrencySection.Visible = True
                txtLossCurrencyAmount.Enabled = False
                ddlLossCurrency.Enabled = False
            Else
                pnlLossCurrencySection.Visible = False
            End If

            If m_bNoAmount Then
                pnlTransactionAmount.Visible = False
                pnlBaseCurrencyAmount.Visible = False

                If m_bShowAccountCurrency Then
                    pnlAccountCurrencyAmount.Visible = False
                End If

                If m_bShowSystemCurrency Then
                    pnlSystemCurrencyAmount.Visible = False
                End If
            End If

            ddlReason.Enabled = m_bReasonEnabled
        End Sub

        Private Function Recalculate() As Integer

            Try
                oWebservice = New NexusProvider.ProviderManager().Provider
                Dim oCurrencyResult As Object

                If m_lSourceID = 0 Then m_lSourceID = 1

                oCurrencyResult = oWebservice.DoCurrencyConversion(m_lAccountID, m_lSourceID, m_iTransactionCurrencyID, m_cTransactionAmount,
                                                               m_iBaseCurrencyID, m_cBaseCurrentAmount, m_iAccountCurrencyID, m_cAccountCurrentAmount,
                                                               m_iSystemCurrencyID, m_cSystemCurrentAmount, m_dTransToBaseExchangeRate, m_dtEffectiveDateOfExchange,
                                                               m_dAccountToBaseExchangeRate, m_dtEffectiveDateOfExchange, m_dSystemToBaseExchangeRate,
                                                               m_dtEffectiveDateOfExchange)
                If oCurrencyResult Is Nothing Then
                    Return -1
                End If

                Dim oCurrency As NexusProvider.DoCurrencyConversion = CType(oCurrencyResult, NexusProvider.DoCurrencyConversion)
                m_iBaseCurrencyID = oCurrency.BaseCurrencyId
                m_cBaseCurrentAmount = oCurrency.BaseAmount
                m_iAccountCurrencyID = oCurrency.AccountCurrencyId
                m_cAccountCurrentAmount = oCurrency.AccountAmount
                m_iSystemCurrencyID = oCurrency.SystemCurrencyId
                m_cSystemCurrentAmount = oCurrency.SystemAmount
                m_dTransToBaseExchangeRate = oCurrency.CurrencyBaseXrate
                m_dtEffectiveDateOfExchange = oCurrency.CurrencyBaseDate
                m_dAccountToBaseExchangeRate = oCurrency.AccountBaseXrate
                m_dSystemToBaseExchangeRate = oCurrency.SystemBaseXrate

                If m_dTransToBaseExchangeRate = 0 Or m_dAccountToBaseExchangeRate = 0 Or m_dSystemToBaseExchangeRate = 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "incompleteCurrencyRates", "alert('The exchange rates for this branch have not been set up.\n\nUntil this has been completed, you will not be able to continue.'); if (window.parent && window.parent.$) { window.parent.$('.modal').modal('hide'); } else if (self.parent.tb_remove) { self.parent.tb_remove(); } else { window.close(); }", True)
                    Session.Remove("MulticurrencyHandled")
                    Session.Remove("CashListMulticurrency")
                    Session.Remove("ClaimPaymentMulticurrency")
                    Return -1
                End If

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

                Return 0
            Catch ex As Exception
                Session.Remove("MulticurrencyHandled")
                Session.Remove("CashListMulticurrency")
                Session.Remove("ClaimPaymentMulticurrency")
                Return -1
            End Try
        End Function

        Private Sub DataToInterface()
            Try
                If m_iTransactionCurrencyID > 0 Then
                    For Each item As NexusProvider.LookupListItem In ddlTransactionCurrency.Items
                        If item.Key = m_iTransactionCurrencyID.ToString() Then
                            ddlTransactionCurrency.Value = item.Code
                            Exit For
                        End If
                    Next
                End If
                txtTransactionAmount.Text = m_cTransactionAmount.ToString("N2")
                txtDateOfExchange.Text = m_dtEffectiveDateOfExchange.ToString("dd/MM/yyyy")

                If m_iBaseCurrencyID > 0 Then
                    For Each item As NexusProvider.LookupListItem In ddlBaseCurrency.Items
                        If item.Key = m_iBaseCurrencyID.ToString() Then
                            ddlBaseCurrency.Value = item.Code
                            Exit For
                        End If
                    Next
                End If
                txtBaseCurrencyRate.Text = m_dDisplayBaseExchangeRate.ToString(ACRateFormat)
                txtBaseCurrencyAmount.Text = m_cBaseCurrentAmount.ToString("N2")

                If m_bShowAccountCurrency Then
                    If m_iAccountCurrencyID > 0 Then
                        For Each item As NexusProvider.LookupListItem In ddlAccountCurrency.Items
                            If item.Key = m_iAccountCurrencyID.ToString() Then
                                ddlAccountCurrency.Value = item.Code
                                Exit For
                            End If
                        Next
                    End If
                    txtAccountCurrencyRate.Text = m_dDisplayAccountExchangeRate.ToString(ACRateFormat)
                    txtAccountCurrencyAmount.Text = m_cAccountCurrentAmount.ToString("N2")
                End If

                If m_bShowSystemCurrency Then
                    If m_iSystemCurrencyID > 0 Then
                        For Each item As NexusProvider.LookupListItem In ddlSystemCurrency.Items
                            If item.Key = m_iSystemCurrencyID.ToString() Then
                                ddlSystemCurrency.Value = item.Code
                                Exit For
                            End If
                        Next
                    End If
                    txtSystemCurrencyRate.Text = m_dDisplaySystemExchangeRate.ToString(ACRateFormat)
                    txtSystemCurrencyAmount.Text = m_cSystemCurrentAmount.ToString("N2")
                End If

                If m_bShowLossCurrency Then
                    If m_iLossCurrencyID > 0 Then
                        For Each item As NexusProvider.LookupListItem In ddlLossCurrency.Items
                            If item.Key = m_iLossCurrencyID.ToString() Then
                                ddlLossCurrency.Value = item.Code
                                Exit For
                            End If
                        Next
                    End If
                    txtLossCurrencyAmount.Text = m_cLossCurrencyAmount.ToString("N2")
                End If

                If m_lRateOverrideReasonID > 0 Then
                    For Each item As NexusProvider.LookupListItem In ddlReason.Items
                        If item.Key = m_lRateOverrideReasonID.ToString() Then
                            ddlReason.Value = item.Code
                            Exit For
                        End If
                    Next
                End If
            Catch ex As Exception
            End Try
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Dim bIsCashListMulticurrency As Boolean = Session("CashListMulticurrency") IsNot Nothing

            Session.Remove("MulticurrencyHandled")
            Session.Remove("CashListMulticurrency")
            Session.Remove("ClaimPaymentMulticurrency")

            If bIsCashListMulticurrency Then
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeModal", "if (self.parent.tb_remove) { self.parent.tb_remove(); self.parent.__doPostBack('', 'CancelMulticurrency'); } else { window.close(); }", True)
            Else
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeModal", "if (window.parent && window.parent.$) { window.parent.$('.modal').modal('hide'); } else if (self.parent.tb_remove) { self.parent.tb_remove(); } else { window.close(); }", True)
            End If
        End Sub

        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            If Not Page.IsValid Then Exit Sub

            oWebservice = New NexusProvider.ProviderManager().Provider
            Dim sValue As String = GetDescriptionForCode(NexusProvider.ListType.PMLookup, ddlReason.Value, "Exchange_Rate_Override_Reason")
            m_lRateOverrideReasonID = GetKeyForDescription(NexusProvider.ListType.PMLookup, sValue, "Exchange_Rate_Override_Reason")

            If ddlReason.Enabled And m_lRateOverrideReasonID = 0 Then
                vldInvalidCurrency.Enabled = True
                vldInvalidCurrency.IsValid = False
                vldInvalidCurrency.ErrorMessage = "You must enter a rate override reason."
                Exit Sub
            End If

            Dim bZeroRates As Boolean = False

            If m_bShowAccountCurrency AndAlso m_dDisplayAccountExchangeRate = 0 Then bZeroRates = True
            If m_bShowSystemCurrency AndAlso m_dDisplaySystemExchangeRate = 0 Then bZeroRates = True
            If m_dDisplayBaseExchangeRate = 0 Then
                bZeroRates = True
            End If

            If bZeroRates Then
                vldInvalidCurrency.Enabled = True
                vldInvalidCurrency.IsValid = False
                vldInvalidCurrency.ErrorMessage = "Rates can not be zero."
                Exit Sub
            End If

            If m_bEnableBaseGroup Then
                If m_bShowAccountCurrency And Not m_bEnableAccountGroup Then
                    m_dDisplayAccountExchangeRate = m_dBaseExchangeRate / m_dAccountExchangeRate
                    m_dAccountExchangeRate = m_dTransToBaseExchangeRate / m_dDisplayAccountExchangeRate
                End If
                If m_bShowSystemCurrency And Not m_bEnableSystemGroup Then
                    m_dDisplaySystemExchangeRate = m_dBaseExchangeRate / m_dSystemExchangeRate
                    m_dSystemExchangeRate = m_dTransToBaseExchangeRate / m_dDisplaySystemExchangeRate
                End If
                m_dBaseExchangeRate = m_dTransToBaseExchangeRate
                m_cBaseAmount = m_cBaseCurrentAmount
                m_dtCurrencyBaseDate = m_dtEffectiveDateOfExchange
            End If

            If m_bShowAccountCurrency And m_bEnableAccountGroup Then
                m_dAccountExchangeRate = m_dBaseExchangeRate / m_dDisplayAccountExchangeRate
                m_cAccountAmount = m_cAccountCurrentAmount
                m_dtAccountBaseDate = m_dtEffectiveDateOfExchange
            End If

            If m_bShowSystemCurrency And m_bEnableSystemGroup Then
                m_dSystemExchangeRate = m_dBaseExchangeRate / m_dDisplaySystemExchangeRate
                m_cSystemAmount = m_cSystemCurrentAmount
                m_dtSystemBaseDate = m_dtEffectiveDateOfExchange
            End If

            If m_lInsuranceFileCnt <> 0 Then
                Dim bValue As Boolean = oWebservice.UpdateInsuranceFile(v_iInsuranceFileCnt:=m_lInsuranceFileCnt, v_dCurrencyBaseRate:=m_dBaseExchangeRate, v_dtCurrencyBaseDate:=m_dtCurrencyBaseDate, v_dAccountBaseRate:=m_dAccountExchangeRate, v_dtAccountBaseDate:=m_dtAccountBaseDate, v_dSystemBaseRate:=m_dSystemExchangeRate, v_dtSystemBaseDate:=m_dtSystemBaseDate, v_iRateOverrideReasonID:=m_lRateOverrideReasonID, v_iBaseCurrencyID:=m_iBaseCurrencyID, v_iAccountCurrencyID:=m_iAccountCurrencyID)
            End If

            If Session("MulticurrencyHandled") IsNot Nothing Then

                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Session(CNCurrencyConversion) = New Nexus.Constants.Session.CurrencyConversion() With {
                    .CurrencyBaseDate = m_dtCurrencyBaseDate,
                    .CurrencyBaseXrate = m_dBaseExchangeRate,
                    .AccountBaseDate = DateTime.MinValue,
                    .AccountBaseXrate = 0,
                    .SystemBaseDate = m_dtSystemBaseDate,
                    .SystemBaseXrate = m_dSystemExchangeRate,
                    .OverrideReason = m_lRateOverrideReasonID
                }
                Session.Remove("MulticurrencyHandled")
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeAndContinue",
                        "self.parent.tb_remove(); self.parent.__doPostBack('', 'ContinueAfterMulticurrency');", True)
            End If

            If Session("CashListMulticurrency") IsNot Nothing Then
                Dim oUserAuthority As New NexusProvider.UserAuthority
                oUserAuthority.UserCode = Session(CNLoginName)
                oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.HasPaymentsAuthority
                oWebservice.GetUserAuthorityValue(oUserAuthority)


                Dim oConvertRequest As New NexusProvider.ConvertCurrencytoBaseParameters
                Dim cPaymentBaseLimit As Decimal
                Dim sPaymentBaseLimit As Decimal
                Dim vPaymentsAmount As Decimal
                Dim vPaymentsCurrencyID As Integer
                Dim vHasPaymentLimit As Integer

                If oUserAuthority IsNot Nothing Then
                    vPaymentsCurrencyID = oUserAuthority.UserAuthorityOptionalValue1
                    vPaymentsAmount = oUserAuthority.UserAuthorityOptionalValue2
                    vHasPaymentLimit = oUserAuthority.UserAuthorityValue
                End If

                With oConvertRequest
                    .CurrencyID = vPaymentsCurrencyID
                    .CompanyID = m_lSourceID
                    .BaseAmount = cPaymentBaseLimit
                    .CurrencyAmount = vPaymentsAmount
                    .ConversionDate = Nothing
                    .ConversionRate = Nothing
                    .IsMultiplier = False
                    .Rounded = Nothing
                    .BaseRoundingDifference = Nothing
                    .CurrencyRoundingDifference = Nothing
                    .FormattedBase = Nothing
                    .FormattedCurrency = sPaymentBaseLimit
                    .Euro = 0
                    .EuroAmount = 0
                    .EuroCCyXrate = Nothing
                    .EuroBaseXRate = Nothing
                    .CcyAmountUnRounded = Nothing
                    .BaseAmountUnRounded = 0
                End With

                If vHasPaymentLimit = 1 Then
                    Dim oConvertResponse As NexusProvider.ConvertCurrencytoBaseResponseParameters = oWebservice.ConvertCurrencyToBase(oConvertRequest)
                    With oConvertResponse
                        cPaymentBaseLimit = .BaseAmount
                        vPaymentsAmount = .CurrencyAmount
                        sPaymentBaseLimit = .FormattedCurrency
                    End With


                    If Session("ModeType") = "Payment" OrElse (Session("ModeValue") = "IP" AndAlso Session("Type").Trim() = Nexus.Constants.Constant.PaymentType.P.ToString()) Then

                        If cPaymentBaseLimit < (Session(CNAmount) * m_dBaseExchangeRate) Then
                            vldInvalidCurrency.Enabled = True
                            vldInvalidCurrency.IsValid = False
                            vldInvalidCurrency.ErrorMessage = "This Payment exceeds your limit of " & sPaymentBaseLimit & "."
                            Exit Sub
                        End If
                    End If
                End If
                Session(CNCashListCurrencyRates) = New Nexus.Constants.Session.CashListCurrencyRates() With {
                    .CurrencyBaseDate = m_dtCurrencyBaseDate,
                    .CurrencyBaseXrate = m_dBaseExchangeRate,
                    .AccountBaseDate = m_dtAccountBaseDate,
                    .AccountBaseXrate = m_dAccountExchangeRate,
                    .SystemBaseDate = m_dtSystemBaseDate,
                    .SystemBaseXrate = m_dSystemExchangeRate,
                    .OverrideReason = m_lRateOverrideReasonID,
                    .BaseAmount = m_cBaseAmount
                }

                If Session(CNCashListItemPending) IsNot Nothing AndAlso Session(CNCashListItem) IsNot Nothing Then
                    Dim pendingItem = CType(Session(CNCashListItemPending), NexusProvider.PaymentItems)
                    Dim rates = CType(Session(CNCashListCurrencyRates), Nexus.Constants.Session.CashListCurrencyRates)
                    If Session("ModeType") = "Payment" OrElse (Session("ModeValue") = "IP" AndAlso Session("Type").Trim() = Nexus.Constants.Constant.PaymentType.P.ToString()) Then
                        Dim cashList = CType(Session(CNCashListItem), NexusProvider.PaymentCashListItemType)
                        For Each item As NexusProvider.PaymentItems In cashList.PaymentItems
                            If item.AccountShortCode = pendingItem.AccountShortCode AndAlso item.Amount = pendingItem.Amount Then
                                item.CurrencyBaseDate = rates.CurrencyBaseDate
                                item.CurrencyBaseXrate = rates.CurrencyBaseXrate
                                item.AccountBaseDate = rates.AccountBaseDate
                                item.AccountBaseXrate = rates.AccountBaseXrate
                                item.SystemBaseDate = rates.SystemBaseDate
                                item.SystemBaseXrate = rates.SystemBaseXrate
                                item.OverrideReason = rates.OverrideReason
                            End If
                        Next
                        Session(CNCashListItem) = cashList
                    Else
                        Dim cashList = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)
                        For Each item As NexusProvider.PaymentItems In cashList.ReceiptItems
                            If item.AccountShortCode = pendingItem.AccountShortCode AndAlso item.Amount = pendingItem.Amount Then
                                item.CurrencyBaseDate = rates.CurrencyBaseDate
                                item.CurrencyBaseXrate = rates.CurrencyBaseXrate
                                item.AccountBaseDate = rates.AccountBaseDate
                                item.AccountBaseXrate = rates.AccountBaseXrate
                                item.SystemBaseDate = rates.SystemBaseDate
                                item.SystemBaseXrate = rates.SystemBaseXrate
                                item.OverrideReason = rates.OverrideReason
                            End If
                        Next
                        Session(CNCashListItem) = cashList
                    End If
                    Session.Remove(CNCashListItemPending)
                End If
                Session.Remove("CashListMulticurrency")
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeAndContinue",
                "self.parent.tb_remove(); self.parent.__doPostBack('', 'ContinueAfterMulticurrency');", True)
            End If

            If Session("ClaimPaymentMulticurrency") IsNot Nothing Then
                Session.Remove("ClaimPaymentMulticurrency")
                Dim sArgs As String = String.Format("ContinueAfterMulticurrency|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}",
                    m_lRateOverrideReasonID,
                    m_dtEffectiveDateOfExchange.ToString("yyyy-MM-dd"),
                    m_cLossCurrencyAmount,
                    m_dBaseExchangeRate,
                    m_dAccountExchangeRate,
                    m_dSystemExchangeRate,
                    Request.QueryString("LossCurrencyCode"),
                    ddlBaseCurrency.Value,
                        m_cBaseAmount)
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "closeAndContinue",
                    String.Format("self.parent.tb_remove(); self.parent.__doPostBack('', '{0}');", sArgs), True)
            End If

        End Sub

        Protected Sub txtBaseCurrencyRate_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBaseCurrencyRate.TextChanged
            ddlReason.Enabled = True
            ApplyBaseCurrencyRate()
        End Sub

        Private Sub ApplyBaseCurrencyRate()
            Dim dRateTemp As Double
            If Not Double.TryParse(txtBaseCurrencyRate.Text, dRateTemp) OrElse dRateTemp = 0 Then
                txtBaseCurrencyRate.Text = m_dDisplayBaseExchangeRate.ToString(ACRateFormat)
            Else
                If m_dDisplayBaseExchangeRate <> dRateTemp Then
                    m_dDisplayBaseExchangeRate = dRateTemp
                    m_dTransToBaseExchangeRate = m_dDisplayBaseExchangeRate

                    If m_iBaseCurrencyID = m_iAccountCurrencyID Then
                        txtAccountCurrencyRate.Text = txtBaseCurrencyRate.Text
                        ApplyAccountCurrencyRate()
                    ElseIf m_dAccountToBaseExchangeRate <> 0 Then
                        m_dAccountToBaseExchangeRate = (m_dDisplayBaseExchangeRate / m_dTransToBaseExchangeRate) * m_dAccountToBaseExchangeRate
                        m_dDisplayAccountExchangeRate = m_dDisplayBaseExchangeRate / m_dAccountToBaseExchangeRate
                    End If

                    If m_iBaseCurrencyID = m_iSystemCurrencyID Then
                        txtSystemCurrencyRate.Text = txtBaseCurrencyRate.Text
                        ApplySystemCurrencyRate()
                    ElseIf m_dSystemToBaseExchangeRate <> 0 Then
                        m_dSystemToBaseExchangeRate = (m_dDisplayBaseExchangeRate / m_dTransToBaseExchangeRate) * m_dSystemToBaseExchangeRate
                        m_dDisplaySystemExchangeRate = m_dDisplayBaseExchangeRate / m_dSystemToBaseExchangeRate
                    End If

                    Recalculate()
                    DataToInterface()
                End If
            End If
            ddlReason.Enabled = m_bReasonEnabled
        End Sub

        Protected Sub txtAccountCurrencyRate_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtAccountCurrencyRate.TextChanged
            ddlReason.Enabled = True
            ApplyAccountCurrencyRate()
        End Sub

        Private Sub ApplyAccountCurrencyRate()
            Dim dRateTemp As Double
            If Not Double.TryParse(txtAccountCurrencyRate.Text, dRateTemp) OrElse dRateTemp = 0 Then
                txtAccountCurrencyRate.Text = m_dDisplayAccountExchangeRate.ToString(ACRateFormat)
            Else
                If m_iAccountCurrencyID = m_iSystemCurrencyID Then
                    txtSystemCurrencyRate.Text = dRateTemp.ToString(ACRateFormat)
                    ApplySystemCurrencyRate()
                End If

                m_dDisplayAccountExchangeRate = dRateTemp
                m_dAccountToBaseExchangeRate = m_dTransToBaseExchangeRate / m_dDisplayAccountExchangeRate

                Recalculate()
                DataToInterface()
            End If
            ddlReason.Enabled = m_bReasonEnabled
        End Sub

        Protected Sub txtSystemCurrencyRate_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtSystemCurrencyRate.TextChanged
            ddlReason.Enabled = True
            ApplySystemCurrencyRate()
        End Sub

        Private Sub ApplySystemCurrencyRate()
            Dim dRateTemp As Double
            If Not Double.TryParse(txtSystemCurrencyRate.Text, dRateTemp) OrElse dRateTemp = 0 Then
                txtSystemCurrencyRate.Text = m_dDisplaySystemExchangeRate.ToString(ACRateFormat)
            Else
                m_dDisplaySystemExchangeRate = dRateTemp
                m_dSystemToBaseExchangeRate = m_dTransToBaseExchangeRate / m_dDisplaySystemExchangeRate

                Recalculate()
                DataToInterface()
            End If
            ddlReason.Enabled = m_bReasonEnabled
        End Sub

        Protected Sub txtDateOfExchange_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDateOfExchange.TextChanged
            Dim dtNewDate As DateTime
            If Not DateTime.TryParse(txtDateOfExchange.Text, dtNewDate) Then
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "dateAlert", "alert('Invalid date entered.');", True)
                Exit Sub
            End If

            If m_dtEffectiveDateOfExchange <> dtNewDate Then
                Dim dtCurrentEffectiveDate As Date = m_dtEffectiveDateOfExchange
                m_dtEffectiveDateOfExchange = dtNewDate

                m_dTransToBaseExchangeRate = 0
                m_dDisplayBaseExchangeRate = 0
                m_dAccountToBaseExchangeRate = 0
                m_dDisplayAccountExchangeRate = 0
                m_dSystemToBaseExchangeRate = 0
                m_dDisplaySystemExchangeRate = 0

                Recalculate()
                If m_dTransToBaseExchangeRate = 0 Or (m_dAccountToBaseExchangeRate = 0 And m_bShowAccountCurrency) Or (m_dSystemToBaseExchangeRate = 0 And m_bShowSystemCurrency) Then
                    ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "dateAlert", "alert('Rates not set up for date entered');", True)
                    m_dtEffectiveDateOfExchange = dtCurrentEffectiveDate
                    Recalculate()
                End If

                DataToInterface()
            End If

            ddlReason.Enabled = m_bReasonEnabled
        End Sub
    End Class
End Namespace