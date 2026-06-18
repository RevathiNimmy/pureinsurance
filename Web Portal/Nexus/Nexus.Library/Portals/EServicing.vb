Imports System.Configuration

Namespace Config

    Public Class EServicing : Inherits ConfigurationElement

        Private cpEnabled As ConfigurationProperty
        Private cpAccounts As ConfigurationProperty

        Private cpcProperties As ConfigurationPropertyCollection

        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

            cpEnabled = New ConfigurationProperty("Enabled", GetType(Boolean), Nothing, _
                ConfigurationPropertyOptions.IsRequired)


            cpAccounts = New ConfigurationProperty("Accounts", GetType(Accounts), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpcProperties.Add(cpEnabled)
            cpcProperties.Add(cpAccounts)

        End Sub

        Public ReadOnly Property Enabled() As Boolean
            Get
                Return CBool(MyBase.Item(cpEnabled))
            End Get
        End Property

        Public ReadOnly Property Accounts() As Accounts
            Get
                Return CType(MyBase.Item(cpAccounts), Accounts)
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
