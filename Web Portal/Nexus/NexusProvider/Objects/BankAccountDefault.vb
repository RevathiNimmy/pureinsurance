<Serializable()> Public Class BankAccountDefault

    ''' <summary>
    ''' Bank Account Default ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BankAccountDefaultID() As Integer

    ''' <summary>
    ''' Source ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SourceID() As Integer

    ''' <summary>
    ''' Cash List Type ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CashListTypeID() As Integer
       

    ''' <summary>
    ''' BankAccountID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BankAccountID() As Integer
       

    ''' <summary>
    ''' EffectiveDate
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EffectiveDate() As Date
        

    ''' <summary>
    ''' Bank Description
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Description() As String
        
    ''' <summary>
    ''' Bank Code
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Code() As String
       
    ''' <summary>
    ''' MediaTypeID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MediaTypeID() As Integer
        

    ''' <summary>
    ''' ProductID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ProductID() As Integer
        

    ''' <summary>
    ''' BankAccountCode
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BankAccountCode() As String
       

    ''' <summary>
    ''' CashListTypeCode
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CashListTypeCode() As String
        

    ''' <summary>
    ''' MediaTypeCode
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MediaTypeCode() As String
       

    ''' <summary>
    ''' ProductCode
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ProductCode() As String
        

    ''' <summary>
    ''' CurrencyID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CurrencyID() As Integer
        

    ''' <summary>
    ''' Currency Code
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CurrencyCode() As String

End Class

<Serializable()> Public Class BankAccountDefaults : Inherits SortableCollectionBase
    Public Sub New()
        MyBase.SortObjectType = GetType(BankAccountDefault)
    End Sub

    ''' <summary>
    ''' Add a Bank object to the collection
    ''' </summary>
    ''' <param name="v_oBank">The Bank object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oBank As BankAccountDefault) As String
        v_oBank.Code = List.Add(v_oBank)
        Return v_oBank.Code
    End Function

    ''' <summary>
    ''' Remove an Bank object from the collection
    ''' </summary>
    ''' <param name="v_oBank">The Bank object to be removed</param>
    Public Sub Remove(ByVal v_oBank As BankAccountDefault)
        List.Remove(v_oBank)
    End Sub

    ''' <summary>
    ''' Remove an Bank object from the collection for given index
    ''' </summary>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Update the bank detail in collection
    ''' </summary>
    ''' <param name="v_oBank"></param>
    ''' <remarks></remarks>
    Public Sub Update(ByVal v_oBank As BankAccountDefault)
        List.Item(v_oBank.Code) = v_oBank
    End Sub

    ''' <summary>
    ''' Update the bank detail in collection for given index
    ''' </summary>
    ''' <param name="v_oBank"></param>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
    Public Sub Update(ByVal v_oBank As BankAccountDefault, ByVal index As Integer)
        List.Item(index) = v_oBank
    End Sub

    ''' <summary>
    ''' Default property for an item in collection
    ''' </summary>
    ''' <param name="i"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Default Public Property Item(ByVal i As Integer) As BankAccountDefault
        Get
            Return List(i)
        End Get
        Set(ByVal value As BankAccountDefault)
            List(i) = value
        End Set
    End Property
End Class
