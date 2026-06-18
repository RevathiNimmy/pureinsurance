Imports System.Configuration

Namespace Config

    Public Class Claims : Inherits ConfigurationElement

        Private cpScreenLocation As ConfigurationProperty
        Private cpNewClaimStatus As ConfigurationProperty
        Private cpRiskTypes As ConfigurationProperty
        Private cpPerilTypes As ConfigurationProperty
        Private cpCorePerilScreenCode As ConfigurationProperty
        Private cpShowSummary As ConfigurationProperty
        Private cpcProperties As ConfigurationPropertyCollection
        Private cpShowClaimStatusOnFindGrid As ConfigurationProperty
        Private cpPaymentClientSearch As ConfigurationProperty

        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

            cpScreenLocation = New ConfigurationProperty("ScreenLocation", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpNewClaimStatus = New ConfigurationProperty("NewClaimStatus", GetType(String), String.Empty, _
               ConfigurationPropertyOptions.IsRequired)

            cpRiskTypes = New ConfigurationProperty("RiskTypes", GetType(ClaimRiskTypes), New ClaimRiskTypes)

            cpPerilTypes = New ConfigurationProperty("PerilTypes", GetType(PerilTypes), New PerilTypes)
            cpCorePerilScreenCode = New ConfigurationProperty("CorePerilScreenCode", GetType(String), String.Empty, _
                        ConfigurationPropertyOptions.IsRequired)
            cpShowSummary = New ConfigurationProperty("ShowSummary", GetType(Boolean), True)
            cpShowClaimStatusOnFindGrid = New ConfigurationProperty("ShowClaimStatusOnFindGrid", GetType(Boolean), False)
            cpPaymentClientSearch = New ConfigurationProperty("PaymentClientSearch", GetType(Boolean), False)

            cpcProperties.Add(cpScreenLocation)
            cpcProperties.Add(cpNewClaimStatus)
            cpcProperties.Add(cpRiskTypes)
            cpcProperties.Add(cpPerilTypes)
            cpcProperties.Add(cpCorePerilScreenCode)
            cpcProperties.Add(cpShowSummary)
            cpcProperties.Add(cpShowClaimStatusOnFindGrid)
            cpcProperties.Add(cpPaymentClientSearch)
        End Sub
        Public ReadOnly Property ShowClaimStatusOnFindGrid() As Boolean
            Get
                Return CBool(MyBase.Item(cpShowClaimStatusOnFindGrid))
            End Get
        End Property
        Public ReadOnly Property ShowSummary() As Boolean
            Get
                Return CBool(MyBase.Item(cpShowSummary))
            End Get
        End Property
        Public ReadOnly Property CorePerilScreenCode() As String
            Get
                Return CStr(MyBase.Item(cpCorePerilScreenCode))
            End Get
        End Property
        Public ReadOnly Property ScreenLocation() As String
            Get
                Return CStr(MyBase.Item(cpScreenLocation))
            End Get
        End Property
        Public ReadOnly Property NewClaimStatus() As String
            Get
                Return CStr(MyBase.Item(cpNewClaimStatus))
            End Get
        End Property

        Public ReadOnly Property RiskTypes() As ClaimRiskTypes
            Get
                Return CType(MyBase.Item(cpRiskTypes), ClaimRiskTypes)
            End Get
        End Property
        Public ReadOnly Property PerilTypes() As PerilTypes
            Get
                Return CType(MyBase.Item(cpPerilTypes), PerilTypes)
            End Get
        End Property

        Public ReadOnly Property PaymentClientSearch() As Boolean
            Get
                Return CBool(MyBase.Item(cpPaymentClientSearch))
            End Get
        End Property

        Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
            Get
                Return cpcProperties
            End Get
        End Property

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class

End Namespace