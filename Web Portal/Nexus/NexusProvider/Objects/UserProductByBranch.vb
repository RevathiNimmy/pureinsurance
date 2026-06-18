''' <summary>
''' Property Class for User product
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class UserProductByBranch

    Private sCodeField As String

    Private sDescriptionField As String

    Private iProductKey As Integer

    Private oListOfBranches As BranchCollection
    Public Sub New()

        oListOfBranches = New BranchCollection
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Description : " & sDescriptionField & "<br />")
        sbPrint.AppendLine("Code : " & sCodeField & "<br />")
        sbPrint.AppendLine("UserGroup Key : " & iProductKey.ToString() & "<br />")
        sbPrint.AppendLine("Branches ---------------><br />")

        If oListOfBranches IsNot Nothing Then
            sbPrint.AppendLine(oListOfBranches.Print())
            sbPrint.AppendLine("<br />")
        End If

        Return sbPrint.ToString()

    End Function

    Public Property Code() As String
        Get
            Return Me.sCodeField
        End Get
        Set(ByVal value As String)
            Me.sCodeField = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return Me.sDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sDescriptionField = value
        End Set
    End Property

    Public Property UserProductKey() As Integer
        Get
            Return iProductKey
        End Get
        Set(ByVal value As Integer)
            iProductKey = value
        End Set
    End Property

    Public Property ListOfBranches() As BranchCollection
        Get
            Return oListOfBranches
        End Get
        Set(ByVal value As BranchCollection)
            oListOfBranches = value
        End Set
    End Property
End Class


''' <summary>
''' Collection Class to hold usergroup objects.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class UserProductByBranchCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oUserProduct As UserProductByBranch In List
            sbPrint.AppendLine(oUserProduct.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oUserProduct As UserProductByBranch) As Integer
        Return List.Add(v_oUserProduct)
    End Function

    Public Sub Remove(ByVal v_oUserProduct As UserProductByBranch)
        List.Remove(v_oUserProduct)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As UserProductByBranch
        Get
            Return List(i)
        End Get
        Set(ByVal value As UserProductByBranch)
            List(i) = value
        End Set
    End Property
    Public ReadOnly Property GetProductByCode(ByVal strProductcode As String) As UserProductByBranch

        Get
            Dim i As Integer = MyBase.Count - 1
            Dim oUserProductByBranch As New UserProductByBranch


            While (i >= 0)

                If LCase(CType(Item(i), UserProductByBranch).Code) = LCase(strProductcode) Then
                    oUserProductByBranch = CType(Item(i), UserProductByBranch)
                End If
                i -= 1
            End While
            Return oUserProductByBranch
        End Get
       
    End Property
End Class

