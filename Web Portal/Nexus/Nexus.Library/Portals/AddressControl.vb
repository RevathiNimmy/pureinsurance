Imports System.Configuration

Namespace Config

    Public Class AddressControl : Inherits ConfigurationElement

        Private cpEnablePostCodeLookup As ConfigurationProperty
        Private cpShowCountry As ConfigurationProperty
        Private cpPostCodeWebServiceUrl As ConfigurationProperty
        Private cpPostcodeUser As ConfigurationProperty
        Private cpPostcodePass As ConfigurationProperty
        Private cpPostcodeSerialNo As ConfigurationProperty

        Private cpcProperties As ConfigurationPropertyCollection

        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

            cpEnablePostCodeLookup = New ConfigurationProperty("EnablePostCodeLookup", GetType(Boolean), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpShowCountry = New ConfigurationProperty("ShowCountry", GetType(Boolean), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpPostCodeWebServiceUrl = New ConfigurationProperty("PostCodeWebServiceUrl", GetType(String), Nothing)

            cpPostcodeUser = New ConfigurationProperty("PostcodeUser", GetType(String), Nothing)

            cpPostcodePass = New ConfigurationProperty("PostcodePass", GetType(String), Nothing)

            cpPostcodeSerialNo = New ConfigurationProperty("PostcodeSerialNo", GetType(String), Nothing)

            cpcProperties.Add(cpEnablePostCodeLookup)
            cpcProperties.Add(cpShowCountry)
            cpcProperties.Add(cpPostCodeWebServiceUrl)
            cpcProperties.Add(cpPostcodeUser)
            cpcProperties.Add(cpPostcodePass)
            cpcProperties.Add(cpPostcodeSerialNo)



        End Sub

        Public ReadOnly Property EnablePostCodeLookup() As Boolean
            Get
                Return CBool(MyBase.Item(cpEnablePostCodeLookup))
            End Get
        End Property

        Public ReadOnly Property ShowCountry() As Boolean
            Get
                Return CBool(MyBase.Item(cpShowCountry))
            End Get
        End Property

        Public ReadOnly Property PostCodeWebServiceUrl() As String
            Get
                Return CStr(MyBase.Item(cpPostCodeWebServiceUrl))
            End Get
        End Property

        Public ReadOnly Property PostcodeUser() As String
            Get
                Return CStr(MyBase.Item(cpPostcodeUser))
            End Get
        End Property

        Public ReadOnly Property PostcodePass() As String
            Get
                Return CStr(MyBase.Item(cpPostcodePass))
            End Get
        End Property

        Public ReadOnly Property PostcodeSerialNo() As String
            Get
                Return CStr(MyBase.Item(cpPostcodeSerialNo))
            End Get
        End Property

        Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
            Get
                Return cpcProperties
            End Get
        End Property

    End Class

End Namespace
