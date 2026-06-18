''' <summary>
''' Nexus LookupListItem object
''' </summary>
<Serializable()> Public Class LookupListItem

    Private iKey As Integer
    Private iParentKey As Integer
    Private sCode As String
    Private sDescription As String
    Private dtEffectiveDate As DateTime
    Private bIsDeleted As Boolean
    Private bIsDefault As Boolean

    ''' <summary>
    ''' Constructor for LookupListItem object
    ''' </summary>
    ''' <param name="v_iKey">Key</param>
    ''' <param name="v_iParentKey">Parent Key</param>
    ''' <param name="v_sCode">Code</param>
    ''' <param name="v_sDescription">Description</param>
    ''' <param name="v_dtEffectiveDate">Effective Date</param>
    ''' <param name="v_bIsDeleted">Is Deleted</param>
    ''' <remarks></remarks>

    Public Sub New()

    End Sub
    Public Sub New(ByVal v_iKey As Integer, _
                ByVal v_iParentKey As Integer, _
                ByVal v_sCode As String, _
                ByVal v_sDescription As String, _
                ByVal v_dtEffectiveDate As DateTime, _
                ByVal v_bIsDeleted As Boolean, _
                ByVal v_bIsDefault As Boolean)

        iKey = v_iKey
        iParentKey = v_iParentKey
        sCode = v_sCode
        sDescription = v_sDescription
        dtEffectiveDate = v_dtEffectiveDate
        bIsDeleted = v_bIsDeleted
        bIsDefault = v_bIsDefault

    End Sub

    ''' <summary>
    ''' Key
    ''' </summary>
    ''' <value>Set the Key</value>
    ''' <returns>Get the Key</returns>
    Public Property Key() As Integer
        Get
            Return iKey
        End Get
        Set(ByVal value As Integer)
            iKey = value
        End Set
    End Property

    ''' <summary>
    ''' Parent Key
    ''' </summary>
    ''' <value>Set the Parent Key</value>
    ''' <returns>Get the Parent Key</returns>
    Public Property ParentKey() As Integer
        Get
            Return iParentKey
        End Get
        Set(ByVal value As Integer)
            iParentKey = value
        End Set
    End Property

    ''' <summary>
    ''' Code
    ''' </summary>
    ''' <value>Set the Code</value>
    ''' <returns>Get the Code</returns>    
    Public Property Code() As String
        Get
            Return sCode
        End Get
        Set(ByVal value As String)
            sCode = value
        End Set
    End Property

    ''' <summary>
    ''' Description
    ''' </summary>
    ''' <value>Set the Description</value>
    ''' <returns>Get the Description</returns>
    Public Property Description() As String
        Get
            Return sDescription
        End Get
        Set(ByVal value As String)
            sDescription = value
        End Set
    End Property

    ''' <summary>
    ''' Effective Date
    ''' </summary>
    ''' <value>Set the Effective Date</value>
    ''' <returns>Get the Effective Date</returns>
    Public Property EffectiveDate() As DateTime
        Get
            Return dtEffectiveDate
        End Get
        Set(ByVal value As DateTime)
            dtEffectiveDate = value
        End Set
    End Property

    ''' <summary>
    ''' Deleted status
    ''' </summary>
    ''' <value>Set the Deleted status</value>
    ''' <returns>Get the Deleted status</returns>
    Public Property IsDeleted() As Boolean
        Get
            Return bIsDeleted
        End Get
        Set(ByVal value As Boolean)
            bIsDeleted = value
        End Set
    End Property
    Public Property IsDefault() As Boolean
        Get
            Return bIsDefault
        End Get
        Set(ByVal value As Boolean)
            bIsDefault = value
        End Set
    End Property

    ''' <summary>
    ''' A class implementing IComparer to sort a collection of LookUpListItem objects by key
    ''' </summary>
    Public Class SortByKey : Implements IComparer

        ''' <summary>
        ''' Compare two objects of type LookupListItem by their key attribute
        ''' </summary>
        ''' <param name="x">Left object</param>
        ''' <param name="y">right object</param>
        ''' <returns>A integer value depending on the result;
        ''' 0 - Equal
        ''' 1 - Left object is greater then right object
        ''' -1 - Right object is greater than left object</returns>
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

            Dim oLeft, oRight As LookupListItem

            If TypeOf x Is LookupListItem Then
                oLeft = x
            Else
                Throw New ArgumentNullException(x)
            End If

            If TypeOf y Is LookupListItem Then
                oRight = y
            Else
                Throw New ArgumentNullException(y)
            End If

            If oLeft.Key = 0 And oRight.Key = 0 Then
                Return 0
            ElseIf oLeft.Key = 0 Then
                Return -1
            ElseIf oRight.Key = 0 Then
                Return 1
            ElseIf oLeft.Key < oRight.Key Then
                Return -1
            ElseIf oLeft.Key = oRight.Key Then
                Return 0
            Else
                Return 1
            End If

        End Function

    End Class

    ''' <summary>
    ''' A class implementing IComparer to sort a collection of LookUpListItem objects by code
    ''' </summary>
    Public Class SortByCode : Implements IComparer

        ''' <summary>
        ''' Compare two objects of type LookupListItem by their code attribute
        ''' </summary>
        ''' <param name="x">Left object</param>
        ''' <param name="y">right object</param>
        ''' <returns>A integer value depending on the result;
        ''' 0 - Equal
        ''' 1 - Left object is greater than right object
        ''' -1 - Right object is greater than left object</returns>
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

            Dim oLeft, oRight As LookupListItem

            If TypeOf x Is LookupListItem Then
                oLeft = x
            Else
                Throw New ArgumentNullException(x)
            End If

            If TypeOf y Is LookupListItem Then
                oRight = y
            Else
                Throw New ArgumentNullException(y)
            End If

            If String.IsNullOrEmpty(oLeft.Code) And String.IsNullOrEmpty(oRight.Code) Then
                Return 0
            ElseIf String.IsNullOrEmpty(oLeft.Code) Then
                Return -1
            ElseIf String.IsNullOrEmpty(oRight.Code) Then
                Return 1
            ElseIf oLeft.Code.ToUpper < oRight.Code.ToUpper Then
                Return -1
            ElseIf oLeft.Code.ToUpper = oRight.Code.ToUpper Then
                Return 0
            Else
                Return 1
            End If

        End Function

    End Class

    ''' <summary>
    ''' A class implementing IComparer to sort a collection of LookUpListItem objects by description
    ''' </summary>
    Public Class SortByDescription : Implements IComparer

        ''' <summary>
        ''' Compare two objects of type LookupListItem by their description attribute
        ''' </summary>
        ''' <param name="x">Left object</param>
        ''' <param name="y">right object</param>
        ''' <returns>A integer value depending on the result;
        ''' 0 - Equal
        ''' 1 - Left object is greater then right object
        ''' -1 - Right object is greater than left object</returns>
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

            Dim oLeft, oRight As LookupListItem

            If TypeOf x Is LookupListItem Then
                oLeft = x
            Else
                Throw New ArgumentNullException(x)
            End If

            If TypeOf y Is LookupListItem Then
                oRight = y
            Else
                Throw New ArgumentNullException(y)
            End If

            If String.IsNullOrEmpty(oLeft.Description) And String.IsNullOrEmpty(oRight.Description) Then
                Return 0
            ElseIf String.IsNullOrEmpty(oLeft.Description) Then
                Return -1
            ElseIf String.IsNullOrEmpty(oRight.Description) Then
                Return 1
                'ElseIf oLeft.Description < oRight.Description Then
                '    Return -1
            ElseIf IsNumeric(oLeft.Description) And IsNumeric(oRight.Description) Then
                If CInt(oLeft.Description) < CInt(oRight.Description) Then
                    Return -1
                Else
                    Return 1
                End If
            ElseIf Not (IsNumeric(oLeft.Description) And oRight.Description.Contains("+")) Then
                Return String.Compare(oLeft.Description, oRight.Description, StringComparison.OrdinalIgnoreCase)
            ElseIf oLeft.Description = oRight.Description Then
                Return 0
            Else
                Return 1
            End If

        End Function

    End Class
End Class

''' <summary>
''' Collection of LookupListItem objects
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class LookupListCollection : Inherits CollectionBase

    ''' <summary>
    ''' Add a LookupListItem object to the collection
    ''' </summary>
    ''' <param name="v_oLookupListItem">The LookupListItem object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oLookupListItem As LookupListItem) As Integer
        Return List.Add(v_oLookupListItem)
    End Function

    ''' <summary>
    ''' Remove an LookupListItem object from the collection
    ''' </summary>
    ''' <param name="v_oLookupListItem">The LookupListItem object to be removed</param>
    Public Sub Remove(ByVal v_oLookupListItem As LookupListItem)
        List.Remove(v_oLookupListItem)
    End Sub

    ''' <summary>
    ''' Remove an LookupListItem object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the LookupListItem object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Sort the collection
    ''' </summary>
    ''' <param name="oItem">LookupListItem attribute to sort by</param>
    ''' <param name="oDirection">Sort order</param>
    Public Sub Sort(ByVal oItem As DataItemTypes, ByVal oDirection As Direction)

        Select Case oItem
            Case DataItemTypes.Key
                InnerList.Sort(New LookupListItem.SortByKey())
            Case DataItemTypes.Code
                InnerList.Sort(New LookupListItem.SortByCode())
            Case DataItemTypes.Description
                InnerList.Sort(New LookupListItem.SortByDescription())
        End Select

        If oDirection = Direction.Desc Then
            InnerList.Reverse()
        End If

    End Sub

    ''' <summary>
    ''' Retrieve or replace an LookupListItem object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the LookupListItem object</param>
    ''' <value>The replacement LookupListItem object</value>
    ''' <returns>The LookupListItem object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As LookupListItem
        Get
            Return List(i)
        End Get
        Set(ByVal value As LookupListItem)
            List(i) = value
        End Set
    End Property

    ''' <summary>
    ''' Return the first LookupListItem object in the collection with the specified Code
    ''' </summary>
    ''' <param name="v_sCode">The Code of the LookupListItem object to be returned</param>
    ''' <value>The Code the LookupListItem is to be retrieved by</value>
    ''' <returns>Matching Contact object, if any</returns>
    Default Public ReadOnly Property Item(ByVal v_sCode As String) As LookupListItem
        Get
            Return FindItemByCode(v_sCode)
        End Get
    End Property

    ''' <summary>
    ''' Find the first LookupListItem object in the collection with the specified Code
    ''' </summary>
    ''' <param name="v_sCode">The Code of the LookupListItem object to be returned</param>
    ''' <returns>The matching LookupListItem object, if any</returns>
    Public Function FindItemByCode(ByVal v_sCode As String) As LookupListItem

        For Each oItem As LookupListItem In List
            If oItem.Code = v_sCode Then
                Return oItem
            End If
        Next

        Return Nothing

    End Function

    ''' <summary>
    ''' Find the first LookupListItem object in the collection with the specified Key
    ''' </summary>
    ''' <param name="v_iKey">The Key of the LookupListItem object to be returned</param>
    ''' <returns>The matching LookupListItem object, if any</returns>
    Public Function FindItemByKey(ByVal v_iKey As Integer) As LookupListItem

        For Each oItem As LookupListItem In List
            If oItem.Key = v_iKey Then
                Return oItem
            End If
        Next

        Return Nothing

    End Function

    ''' <summary>
    ''' Find the first LookupListItem object in the collection with the specified Description
    ''' </summary>
    ''' <param name="v_sDescription">The Description of the LookupListItem object to be returned</param>
    ''' <returns>The matching LookupListItem object, if any</returns>
    Public Function FindItemByDescription(ByVal v_sDescription As String) As LookupListItem

        For Each oItem As LookupListItem In List
            If oItem.Description = v_sDescription Then
                Return oItem
            End If
        Next

        Return Nothing

    End Function

End Class
