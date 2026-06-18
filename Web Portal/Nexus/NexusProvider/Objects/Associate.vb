<Serializable()> Public Class Associate

    Private sKey As String
    Private iClientKey As Integer
    Private iAssociateKey As Integer
    Private sRelationshipCode As String
    Private sRelationshipDescription As String
    Private sAssociateCode As String
    Private sAssociateName As String
    Private dAccountBalance As Double
    Private dClaimIncurred As Double
    Private sCurrencyCode As String
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
        sbPrint.AppendLine("AccountBalance : " & dAccountBalance & "<br />")
        sbPrint.AppendLine("ClaimIncurred : " & dClaimIncurred & "<br />")
        sbPrint.AppendLine("CurrencyCode : " & sCurrencyCode & "<br />")
        Return sbPrint.ToString

    End Function

    Public Property Key() As String
        Get
            Return sKey
        End Get
        Set(ByVal value As String)
            sKey = value
        End Set
    End Property

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
    Public Property AccountBalance() As Double
        Get
            Return dAccountBalance
        End Get
        Set(ByVal value As Double)
            dAccountBalance = value
        End Set
    End Property
    Public Property ClaimIncurred() As Double
        Get
            Return dClaimIncurred
        End Get
        Set(ByVal value As Double)
            dClaimIncurred = value
        End Set
    End Property
    Public Property CurrencyCode() As String
        Get
            Return sCurrencyCode
        End Get
        Set(ByVal value As String)
            sCurrencyCode = value
        End Set
    End Property
End Class



<Serializable()> Public Class AssociateCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface to the object
    ''' </summary>
    ''' <returns>An HTML string containining data held within the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oAddress As Address In List
            sbPrint.AppendLine(oAddress.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add an Associate object to the collection
    ''' </summary>
    ''' <param name="v_oAssociate">The Associate object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oAssociate As Associate) As Integer
        v_oAssociate.Key = List.Add(v_oAssociate)
        Return v_oAssociate.Key
    End Function

    ''' <summary>
    ''' Remove an Associate object from the collection
    ''' </summary>
    ''' <param name="v_oAssociate">The Associate object to be removed</param>
    Public Sub Remove(ByVal v_oAssociate As Associate)
        List.Remove(v_oAssociate)
    End Sub

    ''' <summary>
    ''' Remove an Associate object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the Associate object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an Associate object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the Associate object</param>
    ''' <value>The replacement Associate object</value>
    ''' <returns>The Associate object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As Associate
        Get
            Return List(i)
        End Get
        Set(ByVal value As Associate)
            List(i) = value
        End Set
    End Property

    Public Sub Update(ByVal v_oAssociate As Associate)
        List.Item(v_oAssociate.Key) = v_oAssociate
    End Sub

    Public Sub Update(ByVal v_oAssociate As Associate, ByVal index As Integer)
        List.Item(index) = v_oAssociate
    End Sub

    '''' <summary>
    '''' Return the first Associate object in the collection with the specified AssociateType
    '''' </summary>
    '''' <param name="v_oAssociateType">The AssociateType of the Associate object to be returned</param>
    '''' <value>The AssociateType the Associate is to be retrieved by</value>
    '''' <returns>Matching Associate object, if any</returns>
    'Default Public ReadOnly Property Item(ByVal v_oAssociateType As AssociateType) As Associate
    '    Get
    '        Return FindItemByAssociateType(v_oAssociateType)
    '    End Get
    'End Property

    '''' <summary>
    '''' Find the first Associate object in the collection with the specified AssociateType
    '''' </summary>
    '''' <param name="v_oAssociateType">The AssociateType of the Associate object to be returned</param>
    '''' <returns>The matching Associate object, if any</returns>
    'Public Function FindItemByAssociateType(ByVal v_oAssociateType As AssociateType) As Associate

    '    For Each oAssociate As Associate In List
    '        If oAssociate.AssociateType = v_oAssociateType Then
    '            Return oAssociate
    '        End If
    '    Next

    '    Return Nothing

    'End Function

End Class

