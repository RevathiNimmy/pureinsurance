<Serializable()> Public Class SupplierBusiness
    Private sBusinessCode As String
    Private sSpecialityCode As String
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
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("sBusinessCode : " & sBusinessCode & "<br />")
        sbPrint.AppendLine("sSpecialityCode : " & sSpecialityCode & "<br />")
        Return sbPrint.ToString

    End Function
    Public Property BusinessCode() As String
        Get
            Return sBusinessCode
        End Get
        Set(ByVal value As String)
            sBusinessCode = value
        End Set
    End Property
    Public Property SpecialityCode() As String
        Get
            Return sSpecialityCode
        End Get
        Set(ByVal value As String)
            sSpecialityCode = value
        End Set
    End Property

End Class

<Serializable()> Public Class SupplierBusinessCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface to the object
    ''' </summary>
    ''' <returns>An HTML string containining data held within the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oSupplies As SupplierBusiness In List
            sbPrint.AppendLine(oSupplies.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add an Supplies object to the collection
    ''' </summary>
    ''' <param name="v_oSupply">The Supplies object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oSupply As SupplierBusiness) As Integer
        Return List.Add(v_oSupply)
    End Function

    ''' <summary>
    ''' Remove an Supplies object from the collection
    ''' </summary>
    ''' <param name="v_oSupplies">The Supplies object to be removed</param>
    Public Sub Remove(ByVal v_oSupplies As SupplierBusiness)
        List.Remove(v_oSupplies)
    End Sub

    ''' <summary>
    ''' Remove an Supplies object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the Supplies object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an Supplies object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the Supplies object</param>
    ''' <value>The replacement Supplies object</value>
    ''' <returns>The Supplies object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As SupplierBusiness
        Get
            Return List(i)
        End Get
        Set(ByVal value As SupplierBusiness)
            List(i) = value
        End Set
    End Property

    Public Sub Update(ByVal v_oSupplies As SupplierBusiness)
        List.Item(v_oSupplies.BusinessCode) = v_oSupplies
    End Sub

    Public Sub Update(ByVal v_oSupplies As SupplierBusiness, ByVal index As Integer)
        List.Item(index) = v_oSupplies
    End Sub

End Class

