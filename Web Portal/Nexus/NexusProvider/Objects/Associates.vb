<Serializable()> Public Class Associates

    Private iClientKey As Integer
    Private iAssociateKey As Integer
    Private sRelationshipCode As String
    Private sRelationshipDescription As String
    Private sAssociateCode As String
    Private sAssociateName As String
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        MyBase.New()

    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("ClientKey : " & iClientKey & "<br />")
        sbPrint.AppendLine("AssociateKey : " & iAssociateKey & "<br />")
        sbPrint.AppendLine("RelationshipCode : " & sRelationshipCode & "<br />")
        sbPrint.AppendLine("RelationshipDescription : " & sRelationshipDescription & "<br />")
        sbPrint.AppendLine("AssociateCode : " & sAssociateCode & "<br />")
        sbPrint.AppendLine("AssociateName : " & sAssociateName & "<br />")
        Return sbPrint.ToString

    End Function
    Public Property ClientKey() As Integer
        Get
            Return iClientKey
        End Get
        Set(ByVal value As Integer)
            iClientKey = value
        End Set
    End Property
    Public Property AssociateKey() As Integer
        Get
            Return iAssociateKey
        End Get
        Set(ByVal value As Integer)
            iAssociateKey = value
        End Set
    End Property

    Public Property RelationshipCode() As String
        Get
            Return sRelationshipCode
        End Get
        Set(ByVal value As String)
            sRelationshipCode = value
        End Set
    End Property
    Public Property RelationshipDescription() As String
        Get
            Return sRelationshipDescription
        End Get
        Set(ByVal value As String)
            sRelationshipDescription = value
        End Set
    End Property
    Public Property AssociateCode() As String
        Get
            Return sAssociateCode
        End Get
        Set(ByVal value As String)
            sAssociateCode = value
        End Set
    End Property
    Public Property AssociateName() As String
        Get
            Return sAssociateName
        End Get
        Set(ByVal value As String)
            sAssociateName = value
        End Set
    End Property
End Class
