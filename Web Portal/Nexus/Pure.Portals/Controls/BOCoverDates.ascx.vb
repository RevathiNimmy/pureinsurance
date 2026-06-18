Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports System.Web.Configuration
Imports Nexus.Library
Imports Nexus.Utils
Imports System.Web.HttpContext
Imports Nexus.Constants
Imports Nexus.Constants.Session
Namespace Nexus

    Partial Class Controls_BOCoverDates : Inherits System.Web.UI.UserControl

        Public Property CoverStartDate() As DateTime
            Get
                Return Me.txtCoverStartDate.Text
            End Get
            Set(ByVal value As DateTime)
                Me.txtCoverStartDate.Text = value
            End Set
        End Property
        Public Property CoverEndDate() As DateTime
            Get
                Return Me.txtCoverEndDate.Text
            End Get
            Set(ByVal value As DateTime)
                Me.txtCoverEndDate.Text = value
            End Set
        End Property
        Public Property InceptionDate() As DateTime
            Get
                'Return Me.txtInception.Text
                Return Me.hiddenInceptionDate.Value
            End Get
            Set(ByVal value As DateTime)
                Me.txtInception.Text = value
                Me.hiddenInceptionDate.Value = value
            End Set
        End Property
        Public Property InceptionTPIDate() As DateTime
            Get
                'Return Me.txtInceptionTPI.Text
                Return Me.hiddenInceptionTPIDate.Value
            End Get
            Set(ByVal value As DateTime)
                Me.txtInceptionTPI.Text = value
                Me.hiddenInceptionTPIDate.Value = value.ToString
            End Set
        End Property
        Public Property QuoteExpiryDate() As DateTime
            Get
                'Return Me.txtQuoteExpiryDate.Text
                Return Me.hiddenQuoteExpiryDate.Value
            End Get
            Set(ByVal value As DateTime)
                Me.txtQuoteExpiryDate.Text = value
                Me.hiddenQuoteExpiryDate.Value = value
            End Set
        End Property

        Public Property ProposalDate() As DateTime
            Get
                'Return Me.txtProposalDate.Text
                Return Me.hiddenProposalDate.Value
            End Get
            Set(ByVal value As DateTime)
                Me.txtProposalDate.Text = value
                Me.hiddenProposalDate.Value = value
            End Set
        End Property

        Public Property RenewalDate() As DateTime
            Get
                'Return Me.txtRenewal.Text
                Return Me.hiddenRenewalDate.Value
            End Get
            Set(ByVal value As DateTime)
                Me.txtRenewal.Text = value
                Me.hiddenRenewalDate.Value = value
            End Set
        End Property
        Shared sErrorMsg As String
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim iGracePeriod As Integer = 0
            Dim oOptionSettings, oFrmDateOptionSettings As NexusProvider.OptionTypeSetting
            Dim sOptionTypeSetting As String
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oRiskTypes As NexusProvider.RiskType = Session(CNRiskType)

            Try
                If Not IsPostBack Then
                    iGracePeriod = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.GracePeriod, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, oRiskTypes.RiskCode)
                    oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1009)
                    oFrmDateOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 1008)
                    hiddenCoverFromDate.Value = oFrmDateOptionSettings.OptionValue
                    sOptionTypeSetting = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsMidnightRenewal, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, oRiskTypes.RiskCode)
                    Dim sOptionValue As Integer = Val(sOptionTypeSetting.ToString)
                    hiddenGracePeriod.Value = iGracePeriod
                    hiddenOptionSetting.Value = oOptionSettings.OptionValue
                    hiddenMidnightRenewalSettings.Value = sOptionValue
                    sErrorMsg = Date.Now.AddMonths(Val(oFrmDateOptionSettings.OptionValue)).ToShortDateString

                    If Not Session(CNMTAType) Is Nothing Then
                        txtCoverStartDate.Enabled = False
                    ElseIf Session(CNQuoteMode) = QuoteMode.ReQuote And Session(CNRenewal) Is Nothing Then
                        txtCoverStartDate.Enabled = False
                    ElseIf Session(CNQuoteMode) = QuoteMode.ReQuote And Session(CNRenewal) IsNot Nothing Then
                        txtCoverStartDate.Enabled = True
                    End If
                    If Session(CNQuoteMode) <> QuoteMode.ReQuote And Session(CNMTAType) Is Nothing Then

                        Me.txtCoverStartDate.Text = DateTime.Now.ToShortDateString() 'DateTime.Now.ToString("MM/dd/yyyy")
                        txtCoverEndDate.Text = CDate(txtCoverStartDate.Text).AddYears(1).ToShortDateString()
                        txtInception.Text = CDate(txtCoverStartDate.Text).ToShortDateString()
                        txtInceptionTPI.Text = DateTime.Now.ToShortDateString()
                        txtQuoteExpiryDate.Text = CDate(txtCoverStartDate.Text).AddDays(hiddenGracePeriod.Value).ToShortDateString()
                        txtProposalDate.Text = CDate(txtCoverStartDate.Text).ToShortDateString()

                        'Checkhing the Value of Midnight Renewal Settings
                        If hiddenMidnightRenewalSettings.Value = 1 Then
                            'Adding 366 days to Renewal Date and cover to date
                            txtCoverEndDate.Text = CDate(txtCoverEndDate.Text).AddDays(-1).ToShortDateString()
                            txtRenewal.Text = CDate(txtCoverEndDate.Text).AddDays(1).ToShortDateString()
                        Else
                            'Adding 365 days to Renewal Date
                            txtRenewal.Text = CDate(txtCoverEndDate.Text).ToShortDateString()
                        End If

                        'For Hidden Fields
                        Me.hiddenInceptionDate.Value = CDate(txtCoverStartDate.Text).ToShortDateString()
                        Me.hiddenInceptionTPIDate.Value = DateTime.Now.ToShortDateString()
                        Me.hiddenQuoteExpiryDate.Value = CDate(txtCoverStartDate.Text).AddDays(hiddenGracePeriod.Value).ToShortDateString()
                        Me.hiddenProposalDate.Value = CDate(txtCoverStartDate.Text).ToShortDateString()

                        'Checkhing the Value of Midnight Renewal Settings
                        If hiddenMidnightRenewalSettings.Value = 1 Then
                            'Adding 366 days to Renewal Date and cover to date
                            Me.hiddenRenewalDate.Value = CDate(txtCoverEndDate.Text).AddDays(1).ToShortDateString()
                        Else
                            'Adding 365 days to Renewal Date
                            Me.hiddenRenewalDate.Value = CDate(txtCoverEndDate.Text).ToShortDateString()
                        End If
                    End If
                End If
            Catch ex As System.Exception
            Finally

            End Try

        End Sub

        Protected Sub custFromDate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles custFromDate.ServerValidate
            If txtCoverStartDate.Text.Trim.Length <> 0 Then
                If IsDate(txtCoverStartDate.Text.Trim) = True Then
                    Dim dCovertStartDate As Date = CDate(txtCoverStartDate.Text)
                    Dim dMinDate, dMaxDate As Date
                    dMinDate = Date.Now.AddYears(-2)
                    dMaxDate = Date.Now.AddMonths(Val(hiddenCoverFromDate.Value))
                    If dCovertStartDate < dMinDate Then
                        args.IsValid = False
                        custFromDate.ErrorMessage = GetLocalResourceObject("lbl_RanErrMsgInvalidCoverStartDateBackDated")

                    ElseIf dCovertStartDate > dMaxDate Then
                        args.IsValid = False
                        custFromDate.ErrorMessage = GetLocalResourceObject("lbl_RanErrMsgInvalidCoverStartDateFutureDated")
                        custFromDate.ErrorMessage = custFromDate.ErrorMessage.Replace("#CoverStartDate", sErrorMsg)
                    End If
                End If
            End If

        End Sub
 
        Protected Sub custToDate_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles custToDate.ServerValidate
            If txtCoverEndDate.Text.Trim.Length <> 0 Then
                If IsDate(txtCoverEndDate.Text.Trim) = True Then
                    Dim dCovertEndDate As Date = CDate(txtCoverEndDate.Text)
                    Dim dMinDate, dMaxDate As Date
                    dMinDate = CDate(txtCoverStartDate.Text.Trim)
                    If hiddenMidnightRenewalSettings.Value = 1 Then
                        dMaxDate = dMinDate.AddMonths(Val(hiddenOptionSetting.Value))
                        dMaxDate = dMaxDate.AddDays(-1)
                    Else
                        dMaxDate = dMinDate.AddMonths(Val(hiddenOptionSetting.Value))
                    End If

                    custToDate.ErrorMessage = GetLocalResourceObject("lbl_RanErrMsgInvalidEndDate")
                    custToDate.ErrorMessage = custToDate.ErrorMessage.Replace("#CoverEndDate", dMaxDate.ToShortDateString)
                    If dCovertEndDate < dMinDate Or dCovertEndDate > dMaxDate Then
                        args.IsValid = False
                    End If
                End If
            End If

        End Sub
    End Class
    

End Namespace