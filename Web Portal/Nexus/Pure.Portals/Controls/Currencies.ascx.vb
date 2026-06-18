Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    <ValidationProperty("Text")> Partial Class Controls_Currencies
        Inherits System.Web.UI.UserControl

        Private bFilterByBranch As Boolean = False
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then

                Dim iCounter As Integer = 0
                If bFilterByBranch = True Then
                    'only those currencies valid for the current branch will be shown
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oCurrencyCollection As NexusProvider.CurrencyCollection
                    Dim oUserDetails As NexusProvider.UserDetails
                    Dim sBranchCode As String = ""

                    If Session(CNAgentDetails) IsNot Nothing Then
                        'need to find the BranchCode for the Agent to get the associated Currencies
                        'no else, for user this will be default
                        oUserDetails = Session(CNAgentDetails)
                        sBranchCode = oUserDetails.ListOfBranches(0).Code
                    End If

                    oCurrencyCollection = oWebService.GetCurrenciesByBranch(sBranchCode)

                    For iCounter = 0 To oCurrencyCollection.Count - 1
                        ddlCurrencylst.Items.Add(New ListItem(oCurrencyCollection(iCounter).Description.Trim, oCurrencyCollection(iCounter).CurrencyCode.Trim))
                    Next

                Else
                    'No Filtering, adding all the available currencies from the web.config
                    Dim sProdCurrencies() As String

                    Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                    Dim oProductConfig As Config.Product = oNexusConfig.Portals.Portal(Portal.GetPortalID()).Products.Product(Session.Item(CNDataModelCode))
                    sProdCurrencies = oProductConfig.Currencies.Split(",")

                    Dim oCurrencies As Config.Currencies = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Currencies
                    'no Currencies specified, so allowed all
                    If sProdCurrencies.Length = 1 And String.IsNullOrEmpty(sProdCurrencies(0)) Then
                        For Each oCurrency As Config.Currency In oCurrencies
                            ddlCurrencylst.Items.Add(New ListItem(oCurrency.Display, oCurrency.Code))
                        Next
                    Else 'Else show only allowed currencies
                        Dim sDisplay As String = ""

                        'loop for allowed currencies for product
                        For iCounter = 0 To sProdCurrencies.Length - 1
                            'taking Diplay from the system level configuration
                            sDisplay = oCurrencies.Currency(sProdCurrencies(iCounter)).Display
                            ddlCurrencylst.Items.Add(New ListItem(sDisplay, sProdCurrencies(iCounter)))
                        Next
                    End If
                End If

                'default selected value
                If GetLocalResourceObject("ddl_Currencylst_defaulttext").ToString().Trim.Length <> 0 Then
                    ddlCurrencylst.Items.Insert(0, New ListItem(GetLocalResourceObject("ddl_Currencylst_defaulttext")))
                Else
                    If ddlCurrencylst.Items.Count > 0 And Session(CNCurrenyCode) Is Nothing Then
                        'if local resource does not have test then first currency will se selected by default
                        ddlCurrencylst.SelectedIndex = 0
                        Session(CNCurrenyCode) = ddlCurrencylst.SelectedValue
                    End If
                End If
                'to set the currency values if values exist in session
                If Not Session(CNCurrenyCode) Is Nothing Then
                    ddlCurrencylst.SelectedValue = Session(CNCurrenyCode)
                End If
            End If
        End Sub

        Public WriteOnly Property FilterByBranch() As Boolean
            Set(ByVal value As Boolean)
                bFilterByBranch = value
            End Set
        End Property

        Protected Sub ddlCurrencylst_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCurrencylst.SelectedIndexChanged
            If ddlCurrencylst.SelectedValue.ToUpper() <> GetLocalResourceObject("ddl_Currencylst_defaulttext").ToString().ToUpper Then
                Session(CNCurrenyCode) = ddlCurrencylst.SelectedValue
            Else
                Session(CNCurrenyCode) = Nothing
            End If

        End Sub
        Public ReadOnly Property Text()
            Get
                Dim CurrencyString As String
                If Session(CNCurrenyCode) Is Nothing Then
                    CurrencyString = GetLocalResourceObject("ddl_Currencylst_defaulttext").ToString().Trim
                Else
                    CurrencyString = Session(CNCurrenyCode)
                End If
                
                Return CurrencyString
            End Get
        End Property
        Public Property SelectedValue() As String
            Get
                Return Session(CNCurrenyCode)
            End Get
            Set(ByVal value As String)
                Session(CNCurrenyCode) = value
            End Set
        End Property
        Public WriteOnly Property Enabled() As Boolean
            Set(ByVal value As Boolean)
                ddlCurrencylst.Enabled = value
            End Set
        End Property
    End Class
    
End Namespace
