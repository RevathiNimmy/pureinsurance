<Serializable()> Public Class ContactDetail
    Private sItem As String

    Private eItemElementName As ItemChoiceType
    Public Property Item() As String
        Get
            Return Me.sItem
        End Get
        Set(ByVal value As String)
            Me.sItem = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property ItemElementName() As ItemChoiceType
        Get
            Return Me.eItemElementName
        End Get
        Set(ByVal value As ItemChoiceType)
            Me.eItemElementName = value
        End Set
    End Property
End Class
Public Enum ItemChoiceType

    '''<remarks/>
    EmailAddress

    '''<remarks/>
    Number
End Enum
