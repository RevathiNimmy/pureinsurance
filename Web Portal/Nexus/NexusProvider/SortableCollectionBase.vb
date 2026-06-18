Imports System
Imports System.Collections.Generic
Imports System.Collections
Imports System.Text
Imports System.Reflection

<Serializable()> Public MustInherit Class SortableCollectionBase
    Inherits CollectionBase

    Private _sortcolumn As String = ""
    Private _sortingOrder As GenericComparer.SortOrder = GenericComparer.SortOrder.Ascending
    Private _sortObjectType As Type

    Public Property SortColumn() As String
        Get
            Return _sortcolumn
        End Get
        Set(ByVal value As String)
            _sortcolumn = value
        End Set
    End Property

    Public Property SortingOrder() As GenericComparer.SortOrder
        Get
            Return _sortingOrder
        End Get
        Set(ByVal value As GenericComparer.SortOrder)
            _sortingOrder = value
        End Set
    End Property

    Public Property SortObjectType() As Type
        Get
            Return _sortObjectType
        End Get
        Set(ByVal value As Type)
            _sortObjectType = value
        End Set
    End Property

    Public Overridable Sub Sort()
        If (_sortcolumn = "") Then
            Throw New Exception("Sort column required.")
        End If
        If (_sortObjectType Is Nothing) Then
            Throw New Exception("Sort object type required.")
        End If
        Dim sorter As GenericComparer = New GenericComparer
        sorter.ObjectType = _sortObjectType
        sorter.SortColumn = _sortcolumn
        sorter.SortingOrder = CType(_sortingOrder, Integer)
        InnerList.Sort(sorter)
    End Sub
End Class

Public NotInheritable Class GenericComparer : Implements IComparer

    Private _objectType As Type
    Private _sortcolumn As String = ""
    Private _sortingOrder As Integer = 0

    ''' <summary>
    ''' Type of the object to be compared.
    ''' </summary>
    Public Property ObjectType() As Type
        Get
            Return _objectType
        End Get
        Set(ByVal value As Type)
            _objectType = value
        End Set
    End Property

    ''' <summary>
    ''' Column(public property of the class) to be sorted.
    ''' </summary>
    Public Property SortColumn() As String
        Get
            Return _sortcolumn
        End Get
        Set(ByVal value As String)
            _sortcolumn = value
        End Set
    End Property

    ''' <summary>
    ''' Sorting order.
    ''' </summary>
    Public Property SortingOrder() As Integer
        Get
            Return _sortingOrder
        End Get
        Set(ByVal value As Integer)
            _sortingOrder = value
        End Set
    End Property

    ''' <summary>
    ''' Compare interface implementation
    ''' </summary>
    ''' <param name="x">Object 1</param>
    ''' <param name="y">Object 2</param>
    ''' <returns>Result of comparison</returns>
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
        'Dynamically get the protery info 
        'based on the protery name
        Dim propertyInfo As PropertyInfo = _objectType.GetProperty(_sortcolumn)
        'Get the value of the instance
        Dim obj1 As IComparable = CType(propertyInfo.GetValue(x, Nothing), IComparable)
        Dim obj2 As IComparable = CType(propertyInfo.GetValue(y, Nothing), IComparable)

        'For supporting non-case-sensitive sorting, we have to uppercase the objects before comparision
        'Compare based on the sorting order.
        If TypeOf (obj1) Is String Then
            obj1 = obj1.ToString.ToUpper()
        End If

        If TypeOf (obj2) Is String Then
            obj2 = obj2.ToString.ToUpper()
        End If

        If obj1 Is Nothing AndAlso obj2 Is Nothing Then
            Return 0 'This instance occurs in the same position in the sort order as both objects are nothing. 
        ElseIf (_sortingOrder = 0) Then
            If obj1 Is Nothing Then
                Return 1
            End If
            Return obj1.CompareTo(obj2)
        Else
            If obj2 Is Nothing Then
                Return -1
            End If
            Return obj2.CompareTo(obj1)
        End If
    End Function

    ''' <summary>
    ''' Sorting order
    ''' </summary>
    Public Enum SortOrder
        Ascending = 0
        Descending = 1
    End Enum
End Class
