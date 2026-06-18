Imports System.Configuration

Namespace Config

    Public Class RiskType : Inherits ConfigurationElement


        Private cpName As ConfigurationProperty
        Private cpRiskCode As ConfigurationProperty
        Private cpDataModelCode As ConfigurationProperty
        Private cpPath As ConfigurationProperty
        Private cpIsMandatory As ConfigurationProperty

        Private cpcProperties As ConfigurationPropertyCollection
        Private cpValidationEnabled As ConfigurationProperty
        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

            cpName = New ConfigurationProperty("Name", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)
            cpRiskCode = New ConfigurationProperty("RiskCode", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)
            cpDataModelCode = New ConfigurationProperty("DataModelCode", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)
            cpPath = New ConfigurationProperty("Path", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)
            cpIsMandatory = New ConfigurationProperty("IsMandatory", GetType(Boolean), False)
            cpValidationEnabled = New ConfigurationProperty("ValidationEnabled", GetType(Boolean), True)

            cpcProperties.Add(cpName)
            cpcProperties.Add(cpRiskCode)
            cpcProperties.Add(cpDataModelCode)
            cpcProperties.Add(cpPath)
            cpcProperties.Add(cpIsMandatory)
            cpcProperties.Add(cpValidationEnabled)

        End Sub

#Region "Public ReadOnly Property"

        Public ReadOnly Property Name() As String
            Get
                Return CStr(MyBase.Item(cpName))
            End Get
        End Property

        Public ReadOnly Property RiskCode() As String
            Get
                Return CStr(MyBase.Item(cpRiskCode))
            End Get
        End Property

        Public ReadOnly Property DataModelCode() As String
            Get
                Return CStr(MyBase.Item(cpDataModelCode))
            End Get
        End Property


        Public ReadOnly Property Path() As String
            Get
                Return CStr(MyBase.Item(cpPath))
            End Get
        End Property
        ''' <summary>
        ''' Mandatory Property for Bank 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsMandatory() As String
            Get
                Return CStr(MyBase.Item(cpIsMandatory))
            End Get
        End Property
        Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
            Get
                Return cpcProperties
            End Get
        End Property

        Public ReadOnly Property ValidationEnabled() As String
            Get
                Return CStr(MyBase.Item(cpValidationEnabled))
            End Get
        End Property
#End Region

    End Class

End Namespace


