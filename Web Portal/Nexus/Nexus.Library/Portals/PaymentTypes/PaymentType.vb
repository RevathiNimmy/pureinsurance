Imports System.Configuration

Namespace Config

    Public Class PaymentType : Inherits ConfigurationElementCollection

        Private cpName As ConfigurationProperty
        Private cpType As ConfigurationProperty
        Private cpUrl As ConfigurationProperty
        Private cpPaymentCollectionUrl As ConfigurationProperty
        Private cpDisplayName As ConfigurationProperty
        Private cpNoOfInstalments As ConfigurationProperty
        Private cpFeePercent As ConfigurationProperty
        Private cpRequiredPaymentMethodForMTA As ConfigurationProperty
        Private cpMTADisplayName As ConfigurationProperty
        Private cpUseForQuoteCollection As ConfigurationProperty
        Private cpEnabled As ConfigurationProperty

        Private cpcProperties As ConfigurationPropertyCollection

        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

            cpName = New ConfigurationProperty("Name", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpType = New ConfigurationProperty("Type", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpUrl = New ConfigurationProperty("Url", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpPaymentCollectionUrl = New ConfigurationProperty("PaymentCollectionUrl", GetType(String), Nothing, _
                            ConfigurationPropertyOptions.None)

            cpDisplayName = New ConfigurationProperty("DisplayName", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpNoOfInstalments = New ConfigurationProperty("NoOfInstalments", GetType(Integer), 1)

            cpFeePercent = New ConfigurationProperty("FeePercent", GetType(Decimal), CDec(0))

            cpRequiredPaymentMethodForMTA = New ConfigurationProperty("RequiredPaymentMethodForMTA", GetType(String), Nothing)

            cpMTADisplayName = New ConfigurationProperty("MTADisplayName", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpUseForQuoteCollection = New ConfigurationProperty("UseForQuoteCollection", GetType(String), "False", _
                ConfigurationPropertyOptions.None)

            cpEnabled = New ConfigurationProperty("Enabled", GetType(String), "False", _
                ConfigurationPropertyOptions.IsRequired)

            With cpcProperties
                .Add(cpName)
                .Add(cpType)
                .Add(cpUrl)
                .Add(cpPaymentCollectionUrl)
                .Add(cpDisplayName)
                .Add(cpNoOfInstalments)
                .Add(cpFeePercent)
                .Add(cpRequiredPaymentMethodForMTA)
                .Add(cpMTADisplayName)
                .Add(cpEnabled)
                .Add(cpUseForQuoteCollection)
            End With


        End Sub

        Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
            Get
                Return cpcProperties
            End Get
        End Property

        Public Overrides ReadOnly Property CollectionType() As System.Configuration.ConfigurationElementCollectionType
            Get
                Return ConfigurationElementCollectionType.BasicMap
            End Get
        End Property

        Protected Overrides ReadOnly Property ElementName() As String
            Get
                Return "PaymentProviderValue"
            End Get
        End Property

        Public ReadOnly Property PaymentProviderValue(ByVal Name As String) As PaymentProviderValue
            Get
                Return MyBase.BaseGet(Name)
            End Get
        End Property

        Public ReadOnly Property PaymentProviderValue(ByVal index As Integer) As PaymentProviderValue
            Get
                Return MyBase.BaseGet(index)
            End Get
        End Property

        Public Function GetKey(ByVal index As Integer) As String
            Return CStr(MyBase.BaseGetKey(index))
        End Function

        Protected Overloads Overrides Function CreateNewElement() As System.Configuration.ConfigurationElement
            Return New PaymentProviderValue
        End Function

        Protected Overrides Function GetElementKey(ByVal element As System.Configuration.ConfigurationElement) As Object
            Return CType(element, PaymentProviderValue).key
        End Function

        Public ReadOnly Property Name() As String
            Get
                Return CStr(MyBase.Item(cpName))
            End Get
        End Property

        Public ReadOnly Property Type() As String
            Get
                Return CStr(MyBase.Item(cpType))
            End Get
        End Property

        Public ReadOnly Property Url() As String
            Get
                Return (MyBase.Item(cpUrl)).ToString
            End Get
        End Property

        Public ReadOnly Property PaymentCollectionUrl() As String
            Get
                Return CStr(MyBase.Item(cpPaymentCollectionUrl))
            End Get
        End Property

        Public ReadOnly Property DisplayName() As String
            Get
                Return CStr(MyBase.Item(cpDisplayName))
            End Get
        End Property

        Public ReadOnly Property NoOfInstalments() As Integer
            Get
                Return CInt(MyBase.Item(cpNoOfInstalments))
            End Get
        End Property

        Public ReadOnly Property FeePercent() As Decimal
            Get
                Return CDec(MyBase.Item(cpFeePercent))
            End Get
        End Property

        Public ReadOnly Property RequiredPaymentMethodForMTA() As String
            Get
                Return CStr(MyBase.Item(cpRequiredPaymentMethodForMTA))
            End Get
        End Property

        Public ReadOnly Property MTADisplayName() As String
            Get
                Return CStr(MyBase.Item(cpMTADisplayName))
            End Get
        End Property

        Public ReadOnly Property UseForQuoteCollection() As String
            Get
                Return CStr(MyBase.Item(cpUseForQuoteCollection))
            End Get
        End Property

        Public ReadOnly Property Enabled() As String
            Get
                Return CStr(MyBase.Item(cpEnabled))
            End Get
        End Property
    End Class

End Namespace


