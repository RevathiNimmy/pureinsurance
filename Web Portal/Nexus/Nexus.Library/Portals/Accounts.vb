Imports System.Configuration

Namespace Config

    Public Class Accounts : Inherits ConfigurationElement

        Private cpDisplayPeriodInDays As ConfigurationProperty

        Private cpcProperties As ConfigurationPropertyCollection

        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

            cpDisplayPeriodInDays = New ConfigurationProperty("DisplayPeriodInDays", GetType(Integer), "30")

            cpcProperties.Add(cpDisplayPeriodInDays)

        End Sub

        Public ReadOnly Property UserName() As String
            Get
                Return CBool(MyBase.Item(cpDisplayPeriodInDays))
            End Get
        End Property

        Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
            Get
                Return cpcProperties
            End Get
        End Property

    End Class

End Namespace