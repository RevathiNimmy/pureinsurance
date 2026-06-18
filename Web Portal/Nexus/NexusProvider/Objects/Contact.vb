''' <summary>
''' Nexus party contact object
''' </summary>
<Serializable()> Public Class Contact

    Private iKey As String
    Private oContactType As ContactType
    Private sAreaCode As String
    Private sNumber As String
    Private sDescription As String
    Private sExtension As String
    Private sContactDetailType As ItemChoiceTypes
    Private sOtherContactTypeCode As String
    Private sContactTypeDescriptionField As String
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' Constructor for contact types without area codes e.g Email Address, Web Address
    ''' </summary>
    ''' <param name="v_oContactType">Contact Type</param>
    ''' <param name="v_sNumber">Contact detail, not neccessarily numeric</param>
    Public Sub New(ByVal v_oContactType As ContactType, ByVal v_sNumber As String)
        oContactType = v_oContactType
        sNumber = v_sNumber
    End Sub

    ''' <summary>
    ''' Constructor for contact types with all attributes e.g Telephone Number, Fax Number
    ''' </summary>
    ''' <param name="v_oContactType">Contact Type</param>
    ''' <param name="v_sAreaCode">Area Code</param>
    ''' <param name="v_sNumber">Telephone Number</param>
    Public Sub New(ByVal v_oContactType As ContactType, ByVal v_sAreaCode As String, ByVal v_sNumber As String)
        oContactType = v_oContactType
        sAreaCode = v_sAreaCode
        sNumber = v_sNumber
    End Sub

    ''' <summary>
    ''' Debug interface for Contact object
    ''' </summary>
    ''' <returns>A HTML string containg the object details</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Contact Type : " & ContactType & "<br />")
        sbPrint.AppendLine("AreaCode : " & AreaCode & "<br />")
        sbPrint.AppendLine("Number : " & Number & "<br />")

        Return sbPrint.ToString

    End Function
    Public Property ContactDetailType() As ItemChoiceTypes
        Get
            Return sContactDetailType
        End Get
        Set(ByVal value As ItemChoiceTypes)
            sContactDetailType = value
        End Set
    End Property
    Public Property Key() As String
        Get
            Return iKey
        End Get
        Set(ByVal value As String)
            iKey = value
        End Set
    End Property

    ''' <summary>
    ''' Area Code
    ''' </summary>
    ''' <value>Area Code</value>
    ''' <returns>Area Code</returns>
    Public Property AreaCode() As String
        Get
            Return sAreaCode
        End Get
        Set(ByVal value As String)
            sAreaCode = value
        End Set
    End Property

    ''' <summary>
    ''' Contact Detail e.g Telephone Number, Email Address
    ''' </summary>
    ''' <value>Contact Detail</value>
    ''' <returns>Contact Detail</returns>
    Public Property Number() As String
        Get
            Return sNumber
        End Get
        Set(ByVal value As String)
            sNumber = value
        End Set
    End Property

    ''' <summary>
    ''' Contact Type
    ''' </summary>
    ''' <value>Contact Type</value>
    ''' <returns>Contact Type</returns>
    Public Property ContactType() As ContactType
        Get
            Return oContactType
        End Get
        Set(ByVal value As ContactType)
            oContactType = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return Me.sDescription
        End Get
        Set(ByVal value As String)
            Me.sDescription = value
        End Set
    End Property

    Public Property Extension() As String
        Get
            Return Me.sExtension
        End Get
        Set(ByVal value As String)
            Me.sExtension = value
        End Set
    End Property

    Public Property OtherContactTypeCode() As String
        Get
            Return Me.sOtherContactTypeCode
        End Get
        Set(value As String)
            Me.sOtherContactTypeCode = value
        End Set
    End Property

    Public Property ContactTypeDescription() As String
        Get
            Return Me.sContactTypeDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sContactTypeDescriptionField = value
        End Set
    End Property
End Class

''' <summary>
''' Collection of Contact objects
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class ContactCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oContact As Contact In List
            sbPrint.AppendLine(oContact.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add an Contact object to the collection
    ''' </summary>
    ''' <param name="v_oContact">The Contact object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oContact As Contact) As Integer
        v_oContact.Key = List.Add(v_oContact)
        Return v_oContact.Key
    End Function

    ''' <summary>
    ''' Remove an Contact object from the collection
    ''' </summary>
    ''' <param name="v_oContact">The Contact object to be removed</param>
    Public Sub Remove(ByVal v_oContact As Contact)
        List.Remove(v_oContact)
    End Sub

    ''' <summary>
    ''' Remove an Contact object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the Contact object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an Contact object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the Contact object</param>
    ''' <value>The replacement Contact object</value>
    ''' <returns>The Contact object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As Contact
        Get
            Return List(i)
        End Get
        Set(ByVal value As Contact)
            List(i) = value
        End Set
    End Property

    Public Sub Update(ByVal v_oContacts As Contact)
        List.Item(v_oContacts.Key) = v_oContacts
    End Sub

    Public Sub Update(ByVal v_oContacts As Contact, ByVal index As Integer)
        List.Item(index) = v_oContacts
    End Sub

    ''' <summary>
    ''' Return the first Contact object in the collection with the specified ContactType
    ''' </summary>
    ''' <param name="v_oContactType">The ContactType of the Contact object to be returned</param>
    ''' <value>The ContactType the Contact is to be retrieved by</value>
    ''' <returns>Matching Contact object, if any</returns>
    Default Public ReadOnly Property Item(ByVal v_oContactType As ContactType) As Contact
        Get
            Return FindItemByContactType(v_oContactType)
        End Get
    End Property

    ''' <summary>
    ''' Find the first Contact object in the collection with the specified ContactType
    ''' </summary>
    ''' <param name="v_oContactType">The ContactType of the Contact object to be returned</param>
    ''' <returns>The matching Contact object, if any</returns>
    Public Function FindItemByContactType(ByVal v_oContactType As ContactType) As Contact

        For Each oContact As Contact In List
            If oContact.ContactType = v_oContactType Then
                Return oContact
            End If
        Next

        Return Nothing

    End Function

End Class

Public Enum ItemChoiceTypes

    '''<remarks/>
    EmailAddress

    '''<remarks/>
    Number
End Enum
Public Enum ContactType


    ''' <summary>
    ''' Email Address
    ''' </summary>
    Email = 0

    ''' <summary>
    ''' Home Phone number
    ''' </summary>
    HomePhone = 1

    ''' <summary>
    ''' Mobile Phone number
    ''' </summary>
    Mobile = 2

    ''' <summary>
    ''' Fax number
    ''' </summary>
    Fax = 3

    ''' <summary>
    ''' Website Address
    ''' </summary>
    Web = 4

    ''' <summary>
    ''' Main Contact number
    ''' </summary>
    ''' <remarks></remarks>
    Main = 5

    ''' <summary>
    '''  Other
    ''' </summary>
    ''' <remarks></remarks>
    Other = 6

    ''' <summary>
    ''' Main Email Contact
    ''' </summary>
    ''' <remarks></remarks>
    MEMAIL = 7

    ''' <summary>
    ''' Telephone Contact
    ''' </summary>
    ''' <remarks></remarks>
    Telephone = 8

    ''' <summary>
    ''' Letter Contact
    ''' </summary>
    ''' <remarks></remarks>
    Letter = 9

    ''' <summary>
    ''' Work Phone
    ''' </summary>
    ''' <remarks></remarks>
    WorkPhone = 10

    ''' <summary>
    ''' More Names
    ''' </summary>
    ''' <remarks></remarks>
    MoreName = 11
End Enum
