Imports System.Configuration

Namespace Config

    Public Class Product : Inherits ConfigurationElement

        Private cpName As ConfigurationProperty
        'Private cpDataModelCode As ConfigurationProperty
        ' Private cpDataSetDefinitionFile As ConfigurationProperty
        'Private cpRiskCode As ConfigurationProperty
        Private cpProductCode As ConfigurationProperty
        Private cpQuickQuoteConfig As ConfigurationProperty
        Private cpFullQuoteConfig As ConfigurationProperty
        Private cpRiskTypes As ConfigurationProperty
        Private cpCoverDate As ConfigurationProperty
        Private cpEmailTemplates As ConfigurationProperty
        Private cpDocuments As ConfigurationProperty
        Private cpAllowedAgent As ConfigurationProperty
        Private cpCurrencies As ConfigurationProperty
        Private cpReinstatement As ConfigurationProperty
        Private cpSubmitFromAnyPage As ConfigurationProperty
        Private cpAllowMultiRisks As ConfigurationProperty
        Private cpAllowRole As ConfigurationProperty
        Private cpAutoSave As ConfigurationProperty
        Private cpMakeLiveVisible As ConfigurationProperty
        Private cpAllowAnonymousQuote As ConfigurationProperty
        Private cpcProperties As ConfigurationPropertyCollection
        Private cpAllowEditFees As ConfigurationProperty
        Private cpRequoteFields As ConfigurationProperty
        Private cpNavigationOnRiskDeletion As ConfigurationProperty
        Private cpShowRiskSummary As ConfigurationProperty


        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

            cpName = New ConfigurationProperty("Name", GetType(String), Nothing,
                ConfigurationPropertyOptions.IsRequired)

            cpProductCode = New ConfigurationProperty("ProductCode", GetType(String), String.Empty,
                ConfigurationPropertyOptions.IsRequired)

            cpQuickQuoteConfig = New ConfigurationProperty("QuickQuoteConfig", GetType(String), String.Empty)

            cpFullQuoteConfig = New ConfigurationProperty("FullQuoteConfig", GetType(String), Nothing,
                ConfigurationPropertyOptions.IsRequired)

            cpRiskTypes = New ConfigurationProperty("RiskTypes", GetType(RiskTypes), New RiskTypes)

            cpCoverDate = New ConfigurationProperty("CoverDate", GetType(CoverDate), New CoverDate)

            cpEmailTemplates = New ConfigurationProperty("EmailTemplates", GetType(EmailTemplates), Nothing)

            cpDocuments = New ConfigurationProperty("RiskTypes", GetType(RiskTypes), Nothing)

            cpDocuments = New ConfigurationProperty("Documents", GetType(Documents), Nothing)

            cpAllowedAgent = New ConfigurationProperty("AllowedAgent", GetType(String), String.Empty,
                           ConfigurationPropertyOptions.IsRequired)

            cpCurrencies = New ConfigurationProperty("Currencies", GetType(String), Nothing)

            cpReinstatement = New ConfigurationProperty("MTAReasonForReinstatement", GetType(String), Nothing,
                ConfigurationPropertyOptions.IsRequired)

            cpCurrencies = New ConfigurationProperty("Currencies", GetType(String), Nothing)

            cpSubmitFromAnyPage = New ConfigurationProperty("SubmitFromAnyPage", GetType(Boolean), Nothing,
                ConfigurationPropertyOptions.IsRequired)

            cpAllowMultiRisks = New ConfigurationProperty("AllowMultiRisks", GetType(Boolean), Nothing,
                            ConfigurationPropertyOptions.IsRequired)

            cpAllowRole = New ConfigurationProperty("AllowRole", GetType(String), Nothing)

            cpAutoSave = New ConfigurationProperty("AutoSave", GetType(Boolean), Nothing,
                           ConfigurationPropertyOptions.IsRequired)

            cpAllowEditFees = New ConfigurationProperty("AllowEditFees", GetType(Boolean), False)



            cpMakeLiveVisible = New ConfigurationProperty("MakeLiveVisible", GetType(Boolean), True)
            cpAllowAnonymousQuote = New ConfigurationProperty("AllowAnonymousQuote", GetType(Boolean), Nothing,
                         ConfigurationPropertyOptions.IsRequired)

            cpRequoteFields = New ConfigurationProperty("RequoteFields", GetType(String), Nothing)

            cpNavigationOnRiskDeletion = New ConfigurationProperty("NavigationOnRiskDeletion", GetType(Integer), 0,
                        ConfigurationPropertyOptions.None)

            cpShowRiskSummary = New ConfigurationProperty("ShowRiskSummary", GetType(Boolean), False)

            cpcProperties.Add(cpName)
            cpcProperties.Add(cpProductCode)
            cpcProperties.Add(cpQuickQuoteConfig)
            cpcProperties.Add(cpFullQuoteConfig)
            cpcProperties.Add(cpRiskTypes)
            cpcProperties.Add(cpCoverDate)
            cpcProperties.Add(cpEmailTemplates)
            cpcProperties.Add(cpDocuments)
            cpcProperties.Add(cpAllowedAgent)
            cpcProperties.Add(cpCurrencies)
            cpcProperties.Add(cpReinstatement)
            cpcProperties.Add(cpSubmitFromAnyPage)
            cpcProperties.Add(cpAllowMultiRisks)
            cpcProperties.Add(cpAllowRole)
            cpcProperties.Add(cpAutoSave)
            cpcProperties.Add(cpAllowEditFees)
            cpcProperties.Add(cpMakeLiveVisible)
            cpcProperties.Add(cpAllowAnonymousQuote)
            cpcProperties.Add(cpRequoteFields)
            cpcProperties.Add(cpNavigationOnRiskDeletion)
            cpcProperties.Add(cpShowRiskSummary)

        End Sub

        Public ReadOnly Property Name() As String
            Get
                Return CStr(MyBase.Item(cpName))
            End Get
        End Property

        Public ReadOnly Property AllowEditFees() As Boolean
            Get
                Return CBool(MyBase.Item(cpAllowEditFees))
            End Get
        End Property

        Public ReadOnly Property MakeLiveVisible() As Boolean
            Get
                Return CBool(MyBase.Item(cpMakeLiveVisible))
            End Get
        End Property
        '29-11-07 - DH - ProductCode is mandatory for S4i, so it may as well be for S4b.
        Public ReadOnly Property AutoSave() As Boolean
            Get
                Return CBool(MyBase.Item(cpAutoSave))
            End Get
        End Property
        Public ReadOnly Property ProductCode() As String
            Get
                Return CStr(MyBase.Item(cpProductCode))
            End Get
        End Property

        Public ReadOnly Property QuickQuoteConfig() As String
            Get
                Return CStr(MyBase.Item(cpQuickQuoteConfig))
            End Get
        End Property

        Public ReadOnly Property FullQuoteConfig() As String
            Get
                Return CStr(MyBase.Item(cpFullQuoteConfig))
            End Get
        End Property

        Public ReadOnly Property RiskTypes() As RiskTypes
            Get
                Return CType(MyBase.Item(cpRiskTypes), RiskTypes)
            End Get
        End Property

        Public ReadOnly Property CoverDate() As CoverDate
            Get
                Return CType(MyBase.Item(cpCoverDate), CoverDate)
            End Get
        End Property

        Public ReadOnly Property EmailTemplates() As EmailTemplates
            Get
                Return CType(MyBase.Item(cpEmailTemplates), EmailTemplates)
            End Get
        End Property

        Public ReadOnly Property Documents() As Documents
            Get
                Return CType(MyBase.Item(cpDocuments), Documents)
            End Get
        End Property
        '1.3
        Public ReadOnly Property AllowedAgent() As String
            Get
                Return CStr(MyBase.Item(cpAllowedAgent))
            End Get
        End Property

        Public ReadOnly Property Currencies() As String
            Get
                Return CStr(MyBase.Item(cpCurrencies))
            End Get
        End Property

        Public ReadOnly Property MTAReasonForReinstatement() As String
            Get
                Return CStr(MyBase.Item(cpReinstatement))
            End Get
        End Property

        Public ReadOnly Property AllowMultiRisks() As String
            Get
                Return CStr(MyBase.Item(cpAllowMultiRisks))
            End Get
        End Property
        Public ReadOnly Property SubmitFromAnyPage() As String
            Get
                Return CStr(MyBase.Item(cpSubmitFromAnyPage))
            End Get
        End Property
        Public ReadOnly Property AllowAnonymousQuote() As String
            Get
                Return CStr(MyBase.Item(cpAllowAnonymousQuote))
            End Get
        End Property
        Public ReadOnly Property AllowRole() As String
            Get
                Return CStr(MyBase.Item(cpAllowRole))
            End Get
        End Property

        Public ReadOnly Property NavigationOnRiskDeletion() As Integer
            Get
                Return CStr(MyBase.Item(cpNavigationOnRiskDeletion))
            End Get
        End Property

        Public ReadOnly Property ShowRiskSummary() As Boolean
            Get
                Return CBool(MyBase.Item(cpShowRiskSummary))
            End Get
        End Property

        Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
            Get
                Return cpcProperties
            End Get
        End Property


        ''' <summary>
        ''' WPR 109 - Fields to unquote all risks on basis of given fields
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property RequoteFields() As String
            Get
                Return CStr(MyBase.Item(cpRequoteFields))
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class

End Namespace
