''' <summary>
''' Propery Class for UnallocatedCredits
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class UnallocatedCredit

    Private iTransDetailKeyField As Integer

    Private sDocumentRefField As String

    Private sMediaTypeField As String

    Private sReferenceField As String

    Private dAmountField As Double

    Private iAccountKeyField As Integer

    Private dtCollectionDateField As DateTime

    Private bCollectionDateFieldSpecified As Boolean
    Private bIsSelected As Boolean
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("TransDetail Key : " & iTransDetailKeyField.ToString() & "<br />")
        sbPrint.AppendLine("Document Ref : " & sDocumentRefField & "<br />")
        sbPrint.AppendLine("Media Type : " & sMediaTypeField & "<br />")
        sbPrint.AppendLine("Reference : " & sReferenceField & "<br />")
        sbPrint.AppendLine("Amount : " & dAmountField.ToString() & "<br />")
        sbPrint.AppendLine("Account Key : " & iAccountKeyField.ToString() & "<br />")
        sbPrint.AppendLine("Collection Date : " & dtCollectionDateField.ToString() & "<br />")
        sbPrint.AppendLine("Collection Date Specified : " & bCollectionDateFieldSpecified.ToString() & "<br />")

        Return sbPrint.ToString()

    End Function

    Public Property TransDetailKey() As Integer
        Get
            Return Me.iTransDetailKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iTransDetailKeyField = value
        End Set
    End Property

    Public Property DocumentRef() As String
        Get
            Return Me.sDocumentRefField
        End Get
        Set(ByVal value As String)
            Me.sDocumentRefField = value
        End Set
    End Property

    Public Property MediaType() As String
        Get
            Return Me.sMediaTypeField
        End Get
        Set(ByVal value As String)
            Me.sMediaTypeField = value
        End Set
    End Property

    Public Property Reference() As String
        Get
            Return Me.sReferenceField
        End Get
        Set(ByVal value As String)
            Me.sReferenceField = value
        End Set
    End Property

    Public Property Amount() As Double
        Get
            Return Me.dAmountField
        End Get
        Set(ByVal value As Double)
            Me.dAmountField = value
        End Set
    End Property

    Public Property AccountKey() As Integer
        Get
            Return Me.iAccountKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iAccountKeyField = value
        End Set
    End Property

    Public Property CollectionDate() As DateTime
        Get
            Return Me.dtCollectionDateField
        End Get
        Set(ByVal value As Date)
            Me.dtCollectionDateField = value
        End Set
    End Property

    Public Property CollectionDateSpecified() As Boolean
        Get
            Return Me.bCollectionDateFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bCollectionDateFieldSpecified = value
        End Set
    End Property
    Public Property IsSelected() As Boolean
        Get
            Return Me.bIsSelected
        End Get
        Set(ByVal value As Boolean)
            Me.bIsSelected = value
        End Set
    End Property

End Class


''' <summary>
''' Collection Class to hold UnallocatedCredit objects.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class UnallocatedCreditCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oUnallocatedCredit As UnallocatedCredit In List
            sbPrint.AppendLine(oUnallocatedCredit.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oUnallocatedCredit As UnallocatedCredit) As Integer
        Return List.Add(v_oUnallocatedCredit)
    End Function

    Public Sub Remove(ByVal v_oUnallocatedCredit As UnallocatedCredit)
        List.Remove(v_oUnallocatedCredit)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As UnallocatedCredit
        Get
            Return List(i)
        End Get
        Set(ByVal value As UnallocatedCredit)
            List(i) = value
        End Set
    End Property
    Public Sub Sort(ByVal oItem As UnallocatedCreditSort, ByVal oDirection As Direction)

        Select Case oItem
            Case PolicySort.Reference
                InnerList.Sort(New SortByCollectionDate())
        End Select

        If oDirection = Direction.Desc Then
            InnerList.Reverse()
        End If

    End Sub
End Class
<Serializable()> Public Class SortByCollectionDate : Implements IComparer
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

        Dim oLeft, oRight As UnallocatedCredit

        If TypeOf x Is UnallocatedCredit Then
            oLeft = x
        Else
            Throw New ArgumentNullException(x)
        End If

        If TypeOf y Is UnallocatedCredit Then
            oRight = y
        Else
            Throw New ArgumentNullException(y)
        End If

        If String.IsNullOrEmpty(oLeft.CollectionDate) And String.IsNullOrEmpty(oRight.CollectionDate) Then
            Return 0
        ElseIf String.IsNullOrEmpty(oLeft.CollectionDate) Then
            Return -1
        ElseIf String.IsNullOrEmpty(oRight.CollectionDate) Then
            Return 1
        ElseIf oLeft.CollectionDate < oRight.CollectionDate Then
            Return -1
        ElseIf oLeft.CollectionDate = oRight.CollectionDate Then
            Return 0
        Else
            Return 1
        End If

    End Function
End Class
Public Enum UnallocatedCreditSort

    CollectionDate = 0
End Enum