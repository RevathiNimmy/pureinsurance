Imports System.Configuration

Namespace Config

    Public Class Security : Inherits ConfigurationElement

        Private cpPasswordValidationRegex As ConfigurationProperty

        Private cpcProperties As ConfigurationPropertyCollection

        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

            cpPasswordValidationRegex = New ConfigurationProperty("PasswordValidationRegex", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpcProperties.Add(cpPasswordValidationRegex)

        End Sub

        Public ReadOnly Property PasswordValidationRegex() As String
            Get
                Return CStr(MyBase.Item(cpPasswordValidationRegex))
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