Imports System.Text.RegularExpressions

''' <summary>
''' Collection of Search Criteria for the Find Control
''' </summary>
<Serializable()> Public Class FindControlCriteriaCollection : Inherits CollectionBase

    ''' <summary>
    ''' Add a FindControlCriteriaItem object to the collection
    ''' </summary>
    ''' <param name="v_oFindControlCriteriaItem">The FindControlCriteriaItem object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oFindControlCriteriaItem As FindControlCriteriaItem) As Integer
        Return List.Add(v_oFindControlCriteriaItem)
    End Function

    ''' <summary>
    ''' Adds a collection of FindControlCriteriaItem's to the collection
    ''' </summary>
    ''' <param name="c">Collection of FindControlCriteriaItem's to be added</param>
    ''' <remarks>Checks are made for duplicate items and if found they are marked as duplicates, but still added</remarks>
    Public Sub AddRange(ByVal c As System.Collections.ICollection)

        Dim i As IEnumerator = c.GetEnumerator

        While i.MoveNext

            If FindItemByObjectAndProperty(i.Current) IsNot Nothing Then
                CType(i.Current, FindControlCriteriaItem).Duplicate = True
            End If

            List.Add(i.Current)

        End While

    End Sub

    ''' <summary>
    ''' Remove an FindControlCriteriaItem object from the collection
    ''' </summary>
    ''' <param name="v_oFindControlCriteriaItem">The FindControlCriteriaItem object to be removed</param>
    Public Sub Remove(ByVal v_oFindControlCriteriaItem As FindControlCriteriaItem)
        List.Remove(v_oFindControlCriteriaItem)
    End Sub

    ''' <summary>
    ''' Remove an FindControlCriteriaItem object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the FindControlCriteriaItem object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an FindControlCriteriaItem object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the FindControlCriteriaItem object</param>
    ''' <value>The replacement FindControlCriteriaItem object</value>
    ''' <returns>The FindControlCriteriaItem object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As FindControlCriteriaItem
        Get
            Return List(i)
        End Get
        Set(ByVal value As FindControlCriteriaItem)
            List(i) = value
        End Set
    End Property

    ''' <summary>
    ''' Checks if a FindControlCritiriaItem already exists in the collection and if so returns
    ''' the first duplicate object found. This only checks by ObjectName and PropertyName though
    ''' as we're only interest in search criteria against the same object/property
    ''' </summary>
    ''' <param name="v_oFindControlCriteriaItem">FindControlCriteriaItem to match againsts</param>
    ''' <returns>The first matching FindControlCriteriaItem object found</returns>
    ''' <remarks></remarks>
    Public Function FindItemByObjectAndProperty(ByVal v_oFindControlCriteriaItem As FindControlCriteriaItem) As FindControlCriteriaItem

        For Each oFindControlCriteria As FindControlCriteriaItem In List

            If oFindControlCriteria.ObjectName = v_oFindControlCriteriaItem.ObjectName _
                And oFindControlCriteria.PropertyName = v_oFindControlCriteriaItem.PropertyName Then

                Return oFindControlCriteria

            End If

        Next

        Return Nothing

    End Function

End Class

''' <summary>
''' Nexus FindControl search criteria object
''' </summary>
<Serializable()> Public Class FindControlCriteriaItem


    Private sControlName As String
    Private sObjectName As String
    Private sPropertyName As String
    Private sValue As String
    Private bDuplicate As Boolean = False

    ''' <summary>
    ''' Constructor for FindControlCriteriaItem
    ''' </summary>
    ''' <param name="v_sControlName">Form control ID</param>
    ''' <param name="v_sObjectName">Object name within the risk that the form control relates to</param>
    ''' <param name="v_sPropertyName">Property name within the risk that the form control relates to,
    ''' the property must also be within the ObjectName object</param>    
    Public Sub New(ByVal v_sControlName As String, ByVal v_sObjectName As String, ByVal v_sPropertyName As String)

        sControlName = v_sControlName.ToUpper()
        sObjectName = v_sObjectName
        sPropertyName = v_sPropertyName

    End Sub
    ''' <summary>
    ''' Overload Constructor for FindControlCriteriaItem
    ''' </summary>
    ''' <param name="v_sDisplayControlName">Form core control ID</param>
    ''' <remarks>Store the collection of core control ID</remarks>

    Public Sub New(ByVal v_sDisplayControlName As String)

        sControlName = v_sDisplayControlName.ToUpper()

    End Sub

    ''' <summary>
    ''' Function to convert a Form Control ID into a FindControl search criteria object
    ''' </summary>
    ''' <param name="v_sControlName">The form control to be converted</param>
    ''' <returns>The converted FindControlCriteriaItem object</returns>
    ''' <exception cref="ArgumentException">Mapped Controls must contain ObjectName and PropertyName components seperated by '__'</exception>
    ''' <remarks>The function will fail if the control ID's aren't in the recognised risk
    ''' screen format e.g ObjectName__PropertyName</remarks>
    Public Shared Function MapControl(ByVal v_sControlName As String) As FindControlCriteriaItem

        Dim oFindControlCriteriaItem As FindControlCriteriaItem
        Dim oControlName() As String = Regex.Split(v_sControlName.ToUpper(), "__")

        If oControlName IsNot Nothing Then
            If oControlName.Length > 1 Then
                oFindControlCriteriaItem = New FindControlCriteriaItem(v_sControlName, oControlName(0), oControlName(1))
            Else
                Throw New ArgumentException("Mapped Controls must contain ObjectName and PropertyName components seperated by '__'", "MappedControl")
            End If
        Else
            Throw New ArgumentException("Mapped Controls must contain ObjectName and PropertyName components seperated by '__'", "MappedControl")
        End If

        Return oFindControlCriteriaItem

    End Function

    ''' <summary>
    ''' Function the List of Core Controls used on the form
    ''' Works in parallel to MappedControl Property
    ''' </summary>
    ''' <param name="v_sControlName">The form control to be converted</param>
    ''' <returns>The converted FindControlCriteriaItem object</returns>
    ''' <remarks>The function will fail if the control ID's aren't in the form</remarks>
    Public Shared Function DispalyControl(ByVal v_sControlName As String) As FindControlCriteriaItem

        Dim oFindControlCriteriaItem As FindControlCriteriaItem
        Dim oControlName As String = v_sControlName.ToUpper()

        If oControlName IsNot Nothing Then
            If oControlName.Length > 1 Then
                oFindControlCriteriaItem = New FindControlCriteriaItem(v_sControlName.ToUpper())
            Else
                Throw New ArgumentException("Display Controls must be the Core Controls used in the Address Control", "DisplayControl")
            End If
        Else
            Throw New ArgumentException("Display Controls must be the Core Controls used in the Address Control '__'", "DisplayControl")
        End If

        Return oFindControlCriteriaItem
    End Function
    ''' <summary>
    ''' Form control ID
    ''' </summary>
    ''' <value>Set the Control Name</value>
    ''' <returns>Get the Control Name</returns>
    Public Property ControlName() As String
        Get
            Return sControlName
        End Get
        Set(ByVal value As String)
            sControlName = value.ToUpper()
        End Set
    End Property

    ''' <summary>
    ''' Object Name
    ''' </summary>
    ''' <value>Set the Object Name</value>
    ''' <returns>Get the Object Name</returns>
    Public Property ObjectName() As String
        Get
            Return sObjectName
        End Get
        Set(ByVal value As String)
            sObjectName = value
        End Set
    End Property

    ''' <summary>
    ''' Property Name
    ''' </summary>
    ''' <value>Set the Property Name</value>
    ''' <returns>Get the Property Name</returns>
    Public Property PropertyName() As String
        Get
            Return sPropertyName
        End Get
        Set(ByVal value As String)
            sPropertyName = value
        End Set
    End Property

    ''' <summary>
    ''' The value of the search criteria to used in the search
    ''' </summary>
    ''' <value>Set the value, this will usually be the value of the form control the
    ''' current FincdControlCriteriaItem object represents</value>
    ''' <returns>Get the value</returns>
    Public Property Value() As String
        Get
            Return sValue
        End Get
        Set(ByVal value As String)
            sValue = value
        End Set
    End Property

    ''' <summary>
    ''' Is the current object a duplicate of another FindControlCriteriaItem within the FindControlCriteriaCollection
    ''' </summary>
    ''' <value>Set the duplicate status</value>
    ''' <returns>Get the duplicate status</returns>
    Public Property Duplicate() As Boolean
        Get
            Return bDuplicate
        End Get
        Set(ByVal value As Boolean)
            bDuplicate = value
        End Set
    End Property


End Class