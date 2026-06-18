Imports Nexus.Constants.Session
Imports System.Web.Configuration.WebConfigurationManager

Partial Class Modal_EditFee
    Inherits System.Web.UI.Page
    Dim oWebService As NexusProvider.ProviderBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim sRateType As String
            Dim bIsValue, bIsProRated As Boolean
            Dim dRate, dProRataRate As Decimal
            Dim iFeeKey As Integer
            Boolean.TryParse(Request.QueryString("IsValue"), bIsValue)
            Boolean.TryParse(Request.QueryString("IsProRated"), bIsProRated)
            Decimal.TryParse(Request.QueryString("Rate"), dRate)
            Decimal.TryParse(Request.QueryString("ProRataRate"), dProRataRate)
            Integer.TryParse(Request.QueryString("FeeKey"), iFeeKey)
            If Request.QueryString("Type") IsNot Nothing Then
                sRateType = Request.QueryString("Type")
            End If
            'Update the hidden field with values to use it later on
            hRateType.Value = sRateType
            hFeeKey.Value = iFeeKey
            hIsValue.Value = bIsValue
            hRate.Value = dRate
            hIsProRated.Value = bIsProRated
            hProRataRate.Value = dProRataRate
            'Populate the on screen fields 
            If bIsValue = False Then
                rbtRateTypePer.Checked = True
                rbtRateTypeVal.Checked = False
            Else
                rbtRateTypePer.Checked = False
                rbtRateTypeVal.Checked = True
            End If
            txtRate.Text = dRate.ToString

            If hdnIsSuppressDecimals.Value Is Nothing OrElse Trim(hdnIsSuppressDecimals.Value) = "" Then
                Dim oWebService As NexusProvider.ProviderBase = Nothing
                Dim oSuppressDecimalOptionType As New NexusProvider.OptionTypeSetting
                oWebService = New NexusProvider.ProviderManager().Provider
                oSuppressDecimalOptionType = oWebService.GetOptionSetting(NexusProvider.OptionType.ProductOption, NexusProvider.ProductOptions.SuppressDecimalValues)
                If oSuppressDecimalOptionType IsNot Nothing Then
                    hdnIsSuppressDecimals.Value = oSuppressDecimalOptionType.OptionValue
                End If
            End If

        End If
    End Sub

    Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
        MyBase.OnInit(e)
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        'setting the default master page
        CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
    End Sub

    Protected Sub cvFeeValid_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvFeeValid.ServerValidate
        Dim dRate As Decimal
        If Decimal.TryParse(txtRate.Text, dRate) = False Then
            cvFeeValid.ErrorMessage = GetLocalResourceObject("Err_InvalidValues")
            args.IsValid = False
            Exit Sub
        End If
        If rbtRateTypePer.Checked = True Then
            If dRate > 100 Then
                cvFeeValid.ErrorMessage = GetLocalResourceObject("Err_InvalidRange")
                args.IsValid = False
                Exit Sub
            End If
        Else
            If dRate > 1000000000 Then
                cvFeeValid.ErrorMessage = GetLocalResourceObject("Err_InvalidRange")
                args.IsValid = False
                Exit Sub
            End If
        End If
        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
        If oQuote.TransactionType = NexusProvider.Quote.Enum_TransactionType.NB AndAlso txtRate.Text < 0 AndAlso rbtRateTypePer.Checked Then
            cvFeeValid.ErrorMessage = GetLocalResourceObject("Err_NegativePercentage")
            args.IsValid = False
            Exit Sub
        End If
    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If Page.IsValid Then
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim bIsValue As Boolean
            If rbtRateTypePer.Checked = True Then
                bIsValue = False
            Else
                bIsValue = True
            End If
            ' Calling the update fee method of webservice to update the new fee value/percentage entered
            oWebService.UpdateFee(CInt(hFeeKey.Value), bIsValue, CDbl(txtRate.Text), oQuote.BranchCode)
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_updated('','RefreshFees');", True)
        End If
    End Sub

     
End Class
