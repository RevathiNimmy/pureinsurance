Imports System.Configuration

Namespace Config

    Public Class NexusFrameWork : Inherits ConfigurationSection

        Private cpMainContainerName As ConfigurationProperty
        Private cpBranchCode As ConfigurationProperty
        Private cpProductsFolder As ConfigurationProperty
        Private cpPortals As ConfigurationProperty
        Private cpSecurity As ConfigurationProperty
        Private cpCurrencies As ConfigurationProperty
        Private cpGridRowSize As ConfigurationProperty
        Private cpcProperties As ConfigurationPropertyCollection

        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

            cpMainContainerName = New ConfigurationProperty("MainContainerName", GetType(String), "cntMainBody")
            cpBranchCode = New ConfigurationProperty("BranchCode", GetType(String), "HEADOFF") 'no longer need with providers
            cpProductsFolder = New ConfigurationProperty("ProductsFolder", GetType(String), "Products")
            cpGridRowSize = New ConfigurationProperty("FinanceGridSize", GetType(String), "10")
            cpPortals = New ConfigurationProperty("Portals", GetType(Portals), Nothing,
                ConfigurationPropertyOptions.IsRequired)

            cpSecurity = New ConfigurationProperty("Security", GetType(Security), Nothing, ConfigurationPropertyOptions.IsRequired)

            cpCurrencies = New ConfigurationProperty("Currencies", GetType(Currencies), Nothing,
                ConfigurationPropertyOptions.IsRequired)

            cpcProperties.Add(cpMainContainerName)
            cpcProperties.Add(cpBranchCode)
            cpcProperties.Add(cpProductsFolder)
            cpcProperties.Add(cpGridRowSize)
            cpcProperties.Add(cpPortals)
            cpcProperties.Add(cpSecurity)
            cpcProperties.Add(cpCurrencies)

        End Sub

        Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
            Get
                Return cpcProperties
            End Get
        End Property

        Public ReadOnly Property MainContainerName() As String
            Get
                Return CStr(MyBase.Item(cpMainContainerName))
            End Get
        End Property

        Public ReadOnly Property BranchCode() As String
            Get
                Return CStr(MyBase.Item(cpBranchCode))
            End Get
        End Property

        Public ReadOnly Property ProductsFolder() As String
            Get
                Return CStr(MyBase.Item(cpProductsFolder))
            End Get
        End Property

        Public ReadOnly Property Portals() As Portals
            Get
                Return CType(MyBase.Item(cpPortals), Portals)
            End Get
        End Property

        Public ReadOnly Property Security() As Security
            Get
                Return CType(MyBase.Item(cpSecurity), Security)
            End Get
        End Property

        Public ReadOnly Property Currencies() As Currencies
            Get
                Return CType(MyBase.Item(cpCurrencies), Currencies)
            End Get
        End Property
        Public ReadOnly Property FinanceGridSize() As String
            Get
                Return CStr(MyBase.Item(cpGridRowSize))
            End Get
        End Property

    End Class

End Namespace
