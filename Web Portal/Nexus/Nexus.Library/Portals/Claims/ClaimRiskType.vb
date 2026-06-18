Imports System.Configuration

Namespace Config

    Public Class ClaimRiskType : Inherits ConfigurationElement

        Private cpRiskCode As ConfigurationProperty
        Private cpPath As ConfigurationProperty
        Private cpShowRecovery As ConfigurationProperty
        Private cpDisplayClaimReinsurance As ConfigurationProperty
        Private cpcProperties As ConfigurationPropertyCollection

        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

            cpRiskCode = New ConfigurationProperty("Code", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)
            cpPath = New ConfigurationProperty("Folder", GetType(String), Nothing)
            cpShowRecovery = New ConfigurationProperty("ShowRecovery", GetType(Boolean), False)

            cpDisplayClaimReinsurance = New ConfigurationProperty("DisplayClaimReinsurance", GetType(Boolean), True)
            cpShowRecovery = New ConfigurationProperty("ShowRecovery", GetType(Boolean), True)

            cpcProperties.Add(cpRiskCode)
            cpcProperties.Add(cpPath)
            cpcProperties.Add(cpShowRecovery)
            cpcProperties.Add(cpDisplayClaimReinsurance)
            cpcProperties.Add(cpShowRecovery)

        End Sub

#Region "Public ReadOnly Property"
        Public ReadOnly Property ShowRecovery() As Boolean
            Get
                Return CBool(MyBase.Item(cpShowRecovery))
            End Get
        End Property
        Public ReadOnly Property Code() As String
            Get
                Return CStr(MyBase.Item(cpRiskCode))
            End Get
        End Property

        Public ReadOnly Property Folder() As String
            Get
                Return CStr(MyBase.Item(cpPath))
            End Get
        End Property
        Public ReadOnly Property DisplayClaimReinsurance() As Boolean
            Get
                Return CBool(MyBase.Item(cpDisplayClaimReinsurance))
            End Get
        End Property
        Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
            Get
                Return cpcProperties
            End Get
        End Property

#End Region

    End Class

End Namespace


