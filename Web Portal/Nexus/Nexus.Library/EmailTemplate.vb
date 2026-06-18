Imports System.Configuration

Namespace Config

    Public Class EmailTemplate : Inherits ConfigurationElement

        Private cpID As ConfigurationProperty
        Private cpPath As ConfigurationProperty
        Private cpSender As ConfigurationProperty
        Private cpRecipient As ConfigurationProperty
        Private cpSubject As ConfigurationProperty
        Private cpProductCode As ConfigurationProperty
        Private cpEMailTemplateCode As ConfigurationProperty
        Private cpSubjectTemplateCode As ConfigurationProperty
        Private cpTransactionType As ConfigurationProperty

        Private cpcProperties As ConfigurationPropertyCollection

        Public Sub New()

            cpcProperties = New ConfigurationPropertyCollection

            cpID = New ConfigurationProperty("ID", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpPath = New ConfigurationProperty("Path", GetType(String), Nothing, _
                ConfigurationPropertyOptions.IsRequired)

            cpSender = New ConfigurationProperty("Sender", GetType(String), Nothing)

            cpRecipient = New ConfigurationProperty("Recipient", GetType(String), Nothing)

            cpSubject = New ConfigurationProperty("Subject", GetType(String), Nothing)
            cpEMailTemplateCode = New ConfigurationProperty("EmailTemplateCode", GetType(String), String.Empty)
            cpSubjectTemplateCode = New ConfigurationProperty("SubjectTemplateCode", GetType(String), String.Empty)

            cpProductCode = New ConfigurationProperty("ProductCode", GetType(String), Nothing)
            cpTransactionType = New ConfigurationProperty("TransactionType", GetType(String), Nothing)

            cpcProperties.Add(cpID)
            cpcProperties.Add(cpPath)
            cpcProperties.Add(cpSender)
            cpcProperties.Add(cpRecipient)
            cpcProperties.Add(cpSubject)
            cpcProperties.Add(cpProductCode)
            cpcProperties.Add(cpEMailTemplateCode)
            cpcProperties.Add(cpSubjectTemplateCode)
            cpcProperties.Add(cpTransactionType)
        End Sub

        Public ReadOnly Property ID() As String
            Get
                Return CStr(MyBase.Item(cpID))
            End Get
        End Property

        Public ReadOnly Property Path() As String
            Get
                Return CStr(MyBase.Item(cpPath))
            End Get
        End Property

        Public ReadOnly Property Sender() As String
            Get
                Return CStr(MyBase.Item(cpSender))
            End Get
        End Property

        Public ReadOnly Property Recipient() As String
            Get
                Return CStr(MyBase.Item(cpRecipient))
            End Get
        End Property

        Public ReadOnly Property Subject() As String
            Get
                Return CStr(MyBase.Item(cpSubject))
            End Get
        End Property

        Public ReadOnly Property ProductCode() As String
            Get
                Return CStr(MyBase.Item(cpProductCode))
            End Get
        End Property

        Public ReadOnly Property EmailTemplateCode() As String
            Get
                Return CStr(MyBase.Item(cpEMailTemplateCode))
            End Get
        End Property
        Public ReadOnly Property SubjectTemplateCode() As String
            Get
                Return CStr(MyBase.Item(cpSubjectTemplateCode))
            End Get
        End Property

        Public ReadOnly Property TransactionType() As String
            Get
                Return CStr(MyBase.Item(cpTransactionType))
            End Get
        End Property
        Protected Overrides ReadOnly Property Properties() As System.Configuration.ConfigurationPropertyCollection
            Get
                Return cpcProperties
            End Get
        End Property

    End Class

End Namespace