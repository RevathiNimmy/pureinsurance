Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Utils
Imports Nexus.Library
Imports CMS.Library


Namespace Nexus
    Partial Class Modal_RatingSectionDetails
        Inherits System.Web.UI.Page
        Dim sMode As String
        Public sOptionValue As String

        ''' <summary>
        ''' Getting the GetProductRiskOptions to Enable and Disable the Input boxes
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oRatingSectionTypesCollection As NexusProvider.RatingSectionTypesCollection
            Dim oRatingSectionTypesByRiskTypeCollection As NexusProvider.RatingSectionTypesCollection
            Dim oRatingCollection As NexusProvider.RatingCollection
            Dim oRating As NexusProvider.Rating
            Dim sProductRiskOption As String
            Dim oOptionType As New NexusProvider.OptionTypeSetting

            If Not IsPostBack Then

                If Session(CNQuote) IsNot Nothing And CType(Session(CNQuote), NexusProvider.Quote).Risks(Session(CNCurrentRiskKey)).XMLDataset IsNot Nothing Then

                    Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                    Dim InsuranceFileKey As String = oQuote.InsuranceFileKey
                    Dim RiskKey As String = oQuote.Risks(Session(CNCurrentRiskKey)).Key
                    Dim RiskTypeCode As String = oQuote.Risks(Session(CNCurrentRiskKey)).RiskTypeCode.Trim
                    Dim sRatingsectionID As String = CType(Request.QueryString("RatingSectionID"), String)

                    sMode = CType(Request.QueryString("Mode"), String)
                    txtProRataRate.Value = oQuote.Risks(Session(CNCurrentRiskKey)).ProRataRate
                    btnOK.Enabled = True
                    'create a unique key and add this to viewstate
                    'this will be used to cache the results of the SAM call
                    Dim RatingSectionTypesCacheID As Guid
                    RatingSectionTypesCacheID = Guid.NewGuid
                    ViewState.Add("RatingSectionTypesCacheID", RatingSectionTypesCacheID.ToString)

                    'Get Rating Section Types
                    oRatingCollection = CType(Session(CNRatingSections), NexusProvider.RatingCollection)
                    oRatingSectionTypesCollection = oWebService.GetRatingSectionTypes(InsuranceFileKey)

                    oRatingSectionTypesByRiskTypeCollection = oWebService.GetRatingSectionByRiskType(RiskTypeCode:=RiskTypeCode)
                    'To Get the value for 6dp is on
                    oOptionType = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, 106)
                    If (oOptionType IsNot Nothing AndAlso String.IsNullOrEmpty(oOptionType.OptionValue) = False) _
                      AndAlso oOptionType.OptionValue = "1" Then
                        hvOptionValue.Value = oOptionType.OptionValue
                    End If
                    ddlRatingSectionType.DataTextField = "Description"
                    ddlRatingSectionType.DataValueField = "RatingSectionTypeCode"
                    ddlRatingSectionType.DataSource = oRatingSectionTypesByRiskTypeCollection
                    ddlRatingSectionType.DataBind()
                    If oRatingSectionTypesByRiskTypeCollection.Count = 0 Then
                        btnOK.Enabled = False
                    End If
                    'put the data in cache
                    Cache.Insert(ViewState("RatingSectionTypesCacheID"), oRatingSectionTypesCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                    If sMode = "Edit" Then
                        'Get the Details from Collection and Populate the Fields
                        oRating = oRatingCollection.GetRatingItem(sRatingsectionID)

                        ddlEarningPattern.Value = oRating.EarningPatternCode
                        If ddlRatingSectionType.Items.FindByValue(oRating.RatingSectionTypeCode) IsNot Nothing Then
                            ddlRatingSectionType.Items.FindByValue(oRating.RatingSectionTypeCode).Selected = True
                        Else
                            btnOK.Enabled = False
                        End If


                        ddlRateType.Value = oRating.RatingTypeCode
                        txtAnnualPremium.Text = Math.Round(oRating.AnnualPremium, 2)
                        txtRate.Text = oRating.AnnualRate
                        txtThisPremium.Text = oRating.ThisPremium
                        txtOverRideReason.Text = oRating.OverrideReason
                        txtSumInsured.Text = oRating.SumInsured
                        txtState.Value = oRating.StateCode
                        txtCountry.Value = oRating.CountryCode
                        ddlRatingSectionType.Enabled = False
                        'Settings the Hidden Fields to Enable posting of ReadOnly Fields
                        hdnThisPremium.Value = oRating.ThisPremium
                        hdnAnnualPremium.Value = oRating.AnnualPremium
                        hdnCalculatedPremium.Value = oRating.ThisPremium
                        txtCurrencyRate.Value = oRating.AnnualRate
                        'Get the Product Risk Option For Rating Sections ( to enable and Disable the Fields)
                        sProductRiskOption = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.Description, NexusProvider.RiskTypeOptions.AllowEditRatingSectionRate, CType(Session(CNQuote), NexusProvider.Quote).ProductCode, RiskTypeCode)
                        If sProductRiskOption = "1" Then
                            txtRate.ReadOnly = False
                        Else
                            txtRate.ReadOnly = True
                        End If

                        sProductRiskOption = ""
                        sProductRiskOption = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.Description, NexusProvider.RiskTypeOptions.AllowEditRatingSectionRateType, CType(Session(CNQuote), NexusProvider.Quote).ProductCode, RiskTypeCode)
                        If sProductRiskOption = "1" Then
                            ddlRateType.Enabled = True
                        Else
                            ddlRateType.Enabled = False
                        End If

                        sProductRiskOption = ""
                        sProductRiskOption = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.Description, NexusProvider.RiskTypeOptions.AllowEditRatingSectionSumInsured, CType(Session(CNQuote), NexusProvider.Quote).ProductCode, RiskTypeCode)
                        If sProductRiskOption = "1" Then
                            txtSumInsured.ReadOnly = False
                        Else
                            txtSumInsured.ReadOnly = True
                        End If

                        sProductRiskOption = ""
                        sProductRiskOption = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.RiskTypeMaintenance, NexusProvider.ProductRiskOptions.Description, NexusProvider.RiskTypeOptions.AllowEditRatingSectionThisPremium, CType(Session(CNQuote), NexusProvider.Quote).ProductCode, RiskTypeCode)
                        If sProductRiskOption = "1" Then
                            txtThisPremium.ReadOnly = False
                        Else
                            txtThisPremium.ReadOnly = True
                        End If
                    End If
                    txtAnnualPremium.ReadOnly = True
                End If


                ' txtSumInsured.Attributes.Add("onkeypress", "javascript:return isNumeric(event);")
                'txtThisPremium.Attributes.Add("onkeypress", "javascript:return isNumeric(event);")

                txtOverRideReason.Attributes.Add("onkeydown", "javascript:return isSpecialChar(event);")
                If sMode = "Add" Then
                    ddlRatingSectionType_SelectedIndexChanged(Nothing, Nothing)
                End If
                If hdnIsSuppressDecimals.Value Is Nothing OrElse Trim(hdnIsSuppressDecimals.Value) = "" Then
                    Dim oSuppressDecimalOptionType As New NexusProvider.OptionTypeSetting
                    oWebService = New NexusProvider.ProviderManager().Provider
                    oSuppressDecimalOptionType = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, NexusProvider.ProductOptions.SuppressDecimalValues)
                    If oSuppressDecimalOptionType IsNot Nothing Then
                        hdnIsSuppressDecimals.Value = oSuppressDecimalOptionType.OptionValue
                        If Trim(hdnIsSuppressDecimals.Value) = "1" Then
                            txtRate.Attributes.Add("onpaste", "javascript:return false;")
                            txtThisPremium.Attributes.Add("onpaste", "javascript:return false;")
                            txtSumInsured.Attributes.Add("onpaste", "javascript:return false;")
                        End If
                    End If
                End If

            End If

            oWebService = Nothing
        End Sub

        Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        ''' <summary>
        ''' Call the SAM Method UpdateRatingSections to update the Values. 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click

            Dim oWebService As NexusProvider.ProviderBase
            Dim oRatingCollection As NexusProvider.RatingCollection
            Dim oRating As NexusProvider.Rating = Nothing
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim InsuranceFileKey As String = oQuote.InsuranceFileKey
            Dim RiskKey As String = oQuote.Risks(Session(CNCurrentRiskKey)).Key
            Dim bTimeStamp() As Byte
            Dim sRatingsectionID As String = CType(Request.QueryString("RatingSectionID"), String)
            Dim dProRataRate As Decimal
            If Page.IsValid Then
                btnOK.Enabled = False
                'Get the Rating section collection from Session
                oRatingCollection = CType(Session(CNRatingSections), NexusProvider.RatingCollection)
                sMode = CType(Request.QueryString("Mode"), String)
                bTimeStamp = oQuote.TimeStamp

                If sMode = "Edit" Then
                    oRating = oRatingCollection.GetRatingItem(sRatingsectionID)
                    oRating.RatingSectionTypeCode = ddlRatingSectionType.SelectedValue
                    oRating.EarningPatternCode = ddlEarningPattern.Value
                    oRating.RateTypeCode = ddlRateType.Value
                    If IsNumeric(txtSumInsured.Text) Then
                        oRating.SumInsured = CDec(txtSumInsured.Text)
                    Else
                        oRating.SumInsured = 0.0
                    End If
                    If IsNumeric(txtRate.Text) Then
                        oRating.AnnualRate = CDec(txtRate.Text)
                    Else
                        oRating.AnnualRate = 0.0
                    End If
                    If txtThisPremium.ReadOnly = True Then
                        If IsNumeric(hdnThisPremium.Value) Then
                            oRating.ThisPremium = CDec(hdnThisPremium.Value)
                        Else
                            oRating.ThisPremium = 0.0
                        End If
                    Else
                        If IsNumeric(txtThisPremium.Text) Then
                            oRating.ThisPremium = CDec(txtThisPremium.Text)
                        Else
                            oRating.ThisPremium = 0.0
                        End If
                    End If
                    If IsNumeric(hdnAnnualPremium.Value) Then
                        If ddlRateType.Value = "O" Then
                            dProRataRate = oQuote.ProRataRate
                            If dProRataRate > 0 Then
                                txtAnnualPremium.Text = CDec(txtThisPremium.Text) / dProRataRate
                                oRating.AnnualPremium = CDec(txtAnnualPremium.Text)
                                hdnAnnualPremium.Value = txtAnnualPremium.Text
                            End If
                        Else
                            oRating.AnnualPremium = hdnAnnualPremium.Value
                        End If
                    Else
                        oRating.AnnualPremium = 0.0
                    End If

                    If IsNumeric(hdnAnnualPremium.Value) Then
                        oRating.AnnualPremium = CDec(hdnAnnualPremium.Value)
                    Else
                        oRating.AnnualPremium = 0.0
                    End If
                    oRating.CalculatedPremium = CDec(hdnCalculatedPremium.Value)
                    oRating.StateCode = txtState.Value
                    oRating.CountryCode = txtCountry.Value
                    oRating.OverrideReason = txtOverRideReason.Text
                    oRating.CurrencyCode = txtCurrency.Value.Trim
                    If Math.Round(CDec(hdnCalculatedPremium.Value), 2) <> Math.Round(CDec(oRating.ThisPremium), 2) Then
                        oRating.IsAmmended = 1
                    Else
                        oRating.IsAmmended = 0
                    End If
                ElseIf sMode = "Add" Then
                    oRating = New NexusProvider.Rating
                    oRating.RatingSectionTypeCode = ddlRatingSectionType.SelectedValue
                    oRating.EarningPatternCode = ddlEarningPattern.Value
                    oRating.RateTypeCode = ddlRateType.Value
                    If IsNumeric(txtSumInsured.Text) Then
                        oRating.SumInsured = CDec(txtSumInsured.Text)
                    Else
                        oRating.SumInsured = 0.0
                    End If
                    If IsNumeric(txtRate.Text) Then
                        oRating.AnnualRate = CDec(txtRate.Text)
                    Else
                        oRating.AnnualRate = 0.0
                    End If

                    If txtThisPremium.ReadOnly = True Then
                        If IsNumeric(hdnThisPremium.Value) Then
                            oRating.ThisPremium = CDec(hdnThisPremium.Value)
                        Else
                            oRating.ThisPremium = 0.0
                        End If
                    Else
                        If IsNumeric(txtThisPremium.Text) Then
                            oRating.ThisPremium = CDec(txtThisPremium.Text)
                            If ddlRateType.Value = "O" Then
                                dProRataRate = oQuote.ProRataRate
                                If dProRataRate > 0 Then
                                    txtAnnualPremium.Text = CDec(txtThisPremium.Text) / dProRataRate
                                    oRating.AnnualPremium = CDec(txtAnnualPremium.Text)
                                    hdnAnnualPremium.Value = txtAnnualPremium.Text
                                End If
                            Else
                                oRating.AnnualPremium = hdnAnnualPremium.Value
                            End If
                        Else
                            oRating.ThisPremium = 0.0
                            oRating.AnnualPremium = 0.0
                            hdnAnnualPremium.Value = 0.0
                        End If

                    End If

                    If IsNumeric(hdnAnnualPremium.Value) Then
                        oRating.AnnualPremium = CDec(hdnAnnualPremium.Value)
                    Else
                        oRating.AnnualPremium = 0.0
                    End If

                    oRating.StateCode = txtState.Value
                    oRating.CountryCode = txtCountry.Value
                    oRating.OverrideReason = txtOverRideReason.Text
                    If txtCurrency.Value.Trim = "" Then
                        oRating.CurrencyCode = oQuote.CurrencyCode
                    Else
                        oRating.CurrencyCode = txtCurrency.Value.Trim
                    End If

                    oRatingCollection.Add(oRating)
                End If

                If Not oRatingCollection Is Nothing Then
                    Try
                        oWebService = New NexusProvider.ProviderManager().Provider
                        oWebService.UpdateRatingSections(v_iriskKeyField:=RiskKey, i_InsuranceFileKey:=InsuranceFileKey, r_bTimeStamp:=bTimeStamp, oRatingCollection:=oRatingCollection)
                        oQuote.TimeStamp = bTimeStamp


                    Catch

                    Finally
                        oWebService = Nothing
                    End Try
                End If
                Session(CNQuote) = oQuote
                oRating = Nothing
                oRatingCollection = Nothing
                oWebService = Nothing

                'add javascript to call script in parent page which will close modal dialog
                Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshRatingGrid") & ";"
                Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
            End If
            Session(CNRefreshRI) = True

            Session(CNRefreshRI) = True

        End Sub


        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            ' code to close the screen on thickbox implementation 
            'add javascript to call script in parent page which will close modal dialog
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub


        Protected Sub Field_Validate(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
            'Validate the Field values
            If IsNumeric(e.Value) Then
                If CDbl(e.Value) > 999999999.99 Then
                    e.IsValid = False
                End If
            Else
                e.IsValid = False
            End If
        End Sub
        ''' <summary>
        '''  Fill State/Currency and Rate Fields - based on RatingSection Type
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub ddlRatingSectionType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRatingSectionType.SelectedIndexChanged
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oRatingSectionTypesCollection As NexusProvider.RatingSectionTypesCollection = Nothing
            Dim sRatingSectionTypeCode As String = ""
            Dim sRatingSectionCurrency As String = ""

            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim InsuranceFileKey As String = oQuote.InsuranceFileKey

            sRatingSectionTypeCode = ddlRatingSectionType.SelectedValue
            txtCurrency.Value = ""
            txtCountry.Value = ""
            txtState.Value = ""

            'try to get the search results from the cache
            Try
                oRatingSectionTypesCollection =
                    CType(Cache.Item(ViewState("RatingSectionTypesCacheID")), NexusProvider.RatingSectionTypesCollection)

                If oRatingSectionTypesCollection Is Nothing Then
                    oRatingSectionTypesCollection = oWebService.GetRatingSectionTypes(InsuranceFileKey)
                    Cache.Insert(ViewState("RatingSectionTypesCacheID"), oRatingSectionTypesCollection, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                End If

                For Each oRatingSectionType As NexusProvider.RatingSectionTypes In oRatingSectionTypesCollection
                    If oRatingSectionType.RatingSectionTypeCode.Trim = sRatingSectionTypeCode Then
                        If oRatingSectionType.EarningPatternCode IsNot Nothing Then
                            ddlEarningPattern.Value = oRatingSectionType.EarningPatternCode.Trim
                            ddlEarningPattern.Enabled = Not (ddlEarningPattern.Value = "FULLY")
                        End If
                        txtRate.Text = oRatingSectionType.Rate
                        If oRatingSectionType.CurrencyCode IsNot Nothing Then
                            txtCurrency.Value = oRatingSectionType.CurrencyCode.ToString
                        End If
                        If oRatingSectionType.CountryCode IsNot Nothing Then
                            txtCountry.Value = oRatingSectionType.CountryCode.ToString.Trim
                        End If
                        If oRatingSectionType.StateCode IsNot Nothing Then
                            txtState.Value = oRatingSectionType.StateCode.ToString.Trim
                        End If
                        If oRatingSectionType.RateTypeCode IsNot Nothing Then
                            ddlRateType.Value = oRatingSectionType.RateTypeCode.Trim
                        End If

                        Exit For
                    End If
                Next
            Catch ex As System.Exception
                Throw New System.Exception(ex.Message.ToString)
            Finally
                oWebService = Nothing
                oRatingSectionTypesCollection = Nothing
            End Try

        End Sub

        ''' <summary>
        ''' Based on RateType, show the MultiCurrency Fields 
        ''' Do the Calculation based on RateType 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub ddlRateType_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRateType.SelectedIndexChange

            Dim oWebService As NexusProvider.ProviderBase
            Dim sCurrencyCodeQuote As String = ""
            Dim dProRataRate As Decimal
            Dim dConversionRate As Decimal = 1.0
            Dim dAnnualPremium As Decimal = 0.0, dThisPremium As Decimal = 0.0
            Dim dAnnualPremium2 As Decimal = 0.0, dThisPremium2 As Decimal = 0.0 'For Multicurrency
            Dim oCurrency As New NexusProvider.Currency
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            'Dim oFormatString As Config.FormatString

            sCurrencyCodeQuote = oQuote.CurrencyCode
            dProRataRate = oQuote.Risks(Session(CNCurrentRiskKey)).ProRataRate
            txtCurrencyRate.Value = 0.0

            'Decide whether additional Fields ( For Multi-Currency) needs to be shown
            If txtCurrency.Value.Trim <> "" And txtCurrency.Value.Trim <> sCurrencyCodeQuote And (ddlRateType.Value = "V" Or ddlRateType.Value = "Q") Then

                txtAnnualPremium2.Visible = True
                lblAnnualPremium2.Visible = True
                txtThisPremium2.Visible = True
                lblThisPremium2.Visible = True
                txtAnnualPremium2.ReadOnly = True
                txtThisPremium2.ReadOnly = txtThisPremium.ReadOnly

                'Change the Label to show the Currency Symbol
                If lblAnnualPremium.Text.IndexOf("(") > 0 Then
                    lblAnnualPremium.Text = lblAnnualPremium.Text.Substring(0, lblAnnualPremium.Text.IndexOf("("))
                End If
                If lblThisPremium.Text.IndexOf("(") > 0 Then
                    lblThisPremium.Text = lblThisPremium.Text.Substring(0, lblThisPremium.Text.IndexOf("("))
                End If

                If lblAnnualPremium2.Text.IndexOf("(") > 0 Then
                    lblAnnualPremium2.Text = lblAnnualPremium2.Text.Substring(0, lblAnnualPremium2.Text.IndexOf("("))
                End If
                If lblThisPremium2.Text.IndexOf("(") > 0 Then
                    lblThisPremium2.Text = lblThisPremium2.Text.Substring(0, lblThisPremium2.Text.IndexOf("("))
                End If

                lblAnnualPremium.Text += " (" & sCurrencyCodeQuote & ")"
                lblThisPremium.Text += " (" & sCurrencyCodeQuote & ")"
                lblAnnualPremium2.Text += " (" & txtCurrency.Value & ")"
                lblThisPremium2.Text += " (" & txtCurrency.Value & ")"

                oWebService = New NexusProvider.ProviderManager().Provider
                'Get the Currency Conversion Rate
                Try
                    oCurrency = oWebService.GetCurrencyToCurrencyExchangeRate(sCurrencyCodeFrom:=txtCurrency.Value, sCurrencyCodeTo:=sCurrencyCodeQuote, dCurrencyAmountUnRounded:=0.0)
                    dConversionRate = oCurrency.TransactionCurrencyRate
                    txtCurrencyRate.Value = dConversionRate
                Catch
                Finally
                    oWebService = Nothing
                End Try
            Else
                'For Single Currency Additional Fields are not Required.
                txtAnnualPremium2.Visible = False
                lblAnnualPremium2.Visible = False
                txtThisPremium2.Visible = False
                lblThisPremium2.Visible = False
                lblAnnualPremium.Text = GetLocalResourceObject("lbl_AnnualPremium").ToString()
                lblThisPremium.Text = GetLocalResourceObject("lbl_ThisPremium").ToString()
            End If

            Dim dRate As Decimal = 0.0, dSI As Decimal = 0.0
            If txtRate.Text.Trim <> "" And IsNumeric(txtRate.Text) Then
                dRate = CDbl(txtRate.Text)
            End If
            If txtSumInsured.Text.Trim <> "" And IsNumeric(txtSumInsured.Text) Then
                dSI = CDbl(txtSumInsured.Text)
            Else
                dSI = 0.0
            End If
            If ddlRateType.Value = "C" Or ddlRateType.Value = "T" Then
                lblRate.Text = GetLocalResourceObject("lbl_Rate").ToString + "(%)"
            Else
                lblRate.Text = GetLocalResourceObject("lbl_Rate").ToString
            End If
            'Do Premium Calculation ( Based on RateType) 
            Select Case ddlRateType.Value
                Case "V"       'VALUE
                    dAnnualPremium = dRate
                    dThisPremium = dRate * dProRataRate
                Case "Q"    'Quantity X rate
                    dAnnualPremium = dRate * dSI
                    dThisPremium = dRate * dSI * dProRataRate
                Case "C"    'Percentage
                    dAnnualPremium = dRate * dSI / 100
                    dThisPremium = dRate * dSI * dProRataRate / 100
                Case "M"    'Per Million
                    dAnnualPremium = dRate * dSI / 1000000.0
                    dThisPremium = dRate * dSI * dProRataRate / 1000000.0
                Case "T"    'Percentage Of Running Total
                    dAnnualPremium = 0.0
                    dThisPremium = 0.0
                Case "O"    'NO Rating
                    dAnnualPremium = 0.0
                    dThisPremium = 0.0
                Case "P"    'Per 1000
                    dAnnualPremium = dRate * dSI / 1000.0
                    dThisPremium = dRate * dSI * dProRataRate / 1000.0
            End Select

            If Not IsNumeric(txtSumInsured.Text.Trim) Or txtSumInsured.Text.Trim = "" Then
                txtSumInsured.Text = "0.00"
            End If
            If Not IsNumeric(txtThisPremium.Text.Trim) Or txtThisPremium.Text.Trim = "" Then
                txtThisPremium.Text = "0.00"
            End If
            If Not IsNumeric(txtThisPremium2.Text.Trim) Or txtThisPremium2.Text.Trim = "" Then
                txtThisPremium2.Text = "0.00"
            End If
            If Not IsNumeric(txtAnnualPremium.Text.Trim) Or txtAnnualPremium.Text.Trim = "" Then
                txtAnnualPremium.Text = "0.00"
            End If
            If Not IsNumeric(txtAnnualPremium2.Text.Trim) Or txtAnnualPremium2.Text.Trim = "" Then
                txtAnnualPremium2.Text = "0.00"
            End If
            dThisPremium2 = dThisPremium / dConversionRate
            dAnnualPremium2 = dAnnualPremium / dConversionRate

            'Round OFF upto zero decimals if it's ON
            If Trim(hdnIsSuppressDecimals.Value) = "1" Then
                dAnnualPremium = Math.Round(dAnnualPremium, 0, MidpointRounding.AwayFromZero)
                dThisPremium = Math.Round(dThisPremium, 0, MidpointRounding.AwayFromZero)
            Else
                hdnAnnualPremium.Value = dAnnualPremium
                hdnThisPremium.Value = dThisPremium
            End If

            txtAnnualPremium.Text = Math.Round(dAnnualPremium, 2)
            txtThisPremium.Text = dThisPremium

            hdnAnnualPremium.Value = dAnnualPremium
            hdnThisPremium.Value = dThisPremium

            If txtAnnualPremium2.Visible = True Then
                txtAnnualPremium2.Text = dAnnualPremium2
                txtThisPremium2.Text = dThisPremium2
            End If

            If (Not IsNothing(Session(CNIsTrueMonthlyPolicy)) AndAlso Session(CNIsTrueMonthlyPolicy) = True) Then
                lblAnnualPremium.Text = lblAnnualPremium.Text.Replace("Annual", "Monthly")
                lblAnnualPremium2.Text = lblAnnualPremium2.Text.Replace("Annual", "Monthly")
            End If

        End Sub

        ''' <summary>
        ''' OverRide the Validate method
        ''' </summary>
        ''' <param name="group"></param>
        ''' <remarks></remarks>
        Public Overrides Sub Validate(ByVal group As String)

            MyBase.Validate(group)

            ' find the first validator that failed
            For Each validator As IValidator In GetValidators(group)
                If TypeOf validator Is BaseValidator AndAlso Not validator.IsValid Then
                    Dim bv As BaseValidator = DirectCast(validator, BaseValidator)

                    ' look up the control that failed validation
                    Dim target As Control = bv.NamingContainer.FindControl(bv.ControlToValidate)
                    Dim t As TextBox = CType(target, TextBox)
                    ' set the focus to it
                    If t.Enabled = True Then
                        target.Focus()
                    End If

                    Exit For
                End If
            Next
        End Sub

    End Class

End Namespace
